using Application;
using Application.Mapper;
using EndPoint.Api.Helper;
using EndPoint.Api.Middlewares;
using Identity.Application;
using Identity.Application.Mapper;
using Identity.Infrastructure;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//_______________________________Call external API
builder.Services.AddHttpClient();

//builder.Services.AddHttpContextAccessor();

#region DependencyInjection

//cache
builder.Services.AddStackExchangeRedisCache(redisOption =>
{
    var connection = builder.Configuration.GetConnectionString("Redis");
    redisOption.Configuration = connection;
    redisOption.InstanceName = "localRedis_";

});

//log
//Add support to logging with SERILOG
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

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCustomSwagger(builder.Configuration);
builder.Services.AddCustomIdentity();
builder.Services.AddCustomCors();//
builder.Services.AddCustomLocalization();//
builder.Services.AddAppSettings(builder.Configuration);
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddAutoMapper(typeof(AuthProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ShopProfile).Assembly);
//Global Exception
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
//builder.Services.AddProblemDetails();
#endregion

var app = builder.Build();


#region CreateDbWhenDosentExist 
//_____________________ Create Db When Dosent Exist
app.CreateDatabase();
app.CreateIdentityDatabase();

app.SeedData();
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerCustom(builder.Configuration);
}

app.UseElmahIo();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.AddCustomSerilogLogging();

app.ConfigureLogExceptionMiddleware();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
