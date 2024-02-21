using Serilog;
using Application;
using WebAPI.Helper;
using Infrastructure;
using WebAPI.Middlewares;
using Application.Mapper;
using Identity.Application;
using Identity.Infrastructure;
using Identity.Application.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//_______________________________Call external API
builder.Services.AddHttpClient();


#region DependencyInjection

//cache
builder.Services.AddStackExchangeRedisCache(redisOption =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    redisOption.Configuration = connection;
    redisOption.InstanceName = "localRedis_";

});

//log
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
    );
//Elmah
builder.Services.AddElmahIo(o =>
{
    o.ApiKey = "748cb69254d14f64bf47ed09abb94f65";
    o.LogId = new Guid("7f6096d3-c6d6-4233-84da-39828f6030f0");
});

//_______________ Add DependencyInjection

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentityApplication();
builder.Services.AddIdentityInfrastructure(builder.Configuration);

builder.Services.AddCustomSwagger(builder.Configuration);
builder.Services.AddAppSettings(builder.Configuration);
builder.Services.AddCustomIdentity();
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AuthProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ShopProfile).Assembly);

//builder.Services.AddCustomCors();
//builder.Services.AddCustomLocalization();
//Global Exception
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
//builder.Services.AddProblemDetails();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwaggerCustom(builder.Configuration);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseElmahIo();

app.CustomSerilogLogging();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.ConfigureLogExceptionMiddleware();

app.UseRouting();

//app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
