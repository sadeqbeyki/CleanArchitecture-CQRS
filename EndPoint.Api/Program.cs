using Application;
using Application.Mapper;
using EndPoint.Api.Helper;
using EndPoint.Api.Middlewares;
using Identity.Application;
using Identity.Application.Mapper;
using Identity.Infrastructure;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//_______________________________Call API
builder.Services.AddHttpClient();

builder.Services.AddHttpContextAccessor();

//_______________Caching
#region Cache
builder.Services.AddStackExchangeRedisCache(redisOption =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    redisOption.Configuration = connection;
    redisOption.InstanceName = "localRedis_";

});
#endregion

//_______________Logging
#region Logging
//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
    );
#endregion

//_______________ Add DependencyInjection
#region DependencyInjection
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentityApplication();
builder.Services.AddIdentityInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCustomSwagger(builder.Configuration);
builder.Services.AddCustomCors();
builder.Services.AddCustomIdentity();
builder.Services.AddCustomLocalization();
builder.Services.AddAppSettings(builder.Configuration);
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AuthProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ShopProfile).Assembly);
//Global Exception
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
#endregion

var app = builder.Build();

//_____________________ Create Db When Dosent Exist
#region CreateDbWhenDosentExist
app.CreateDatabase();
app.CreateIdentityDatabase();

app.SeedData();
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerCustom(builder.Configuration);
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseSerilogRequestLogging();
app.AddCustomSerilogLogging();

app.ConfigureLogExceptionMiddleware();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
