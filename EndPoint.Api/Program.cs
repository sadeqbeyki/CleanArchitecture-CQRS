using Application;
using Application.Mapper;
using Autofac.Core;
using EndPoint.Api.Helper;
using EndPoint.Api.Middlewares;
using Identity.Application;
using Identity.Application.Mapper;
using Identity.Infrastructure;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Serilog;
using StackExchange.Redis;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

//_______________Caching
//builder.Services.AddMemoryCache();

//builder.Services.AddStackExchangeRedisCache(options => {
//    options.Configuration = builder.Configuration.GetConnectionString("Redis");
//    options.InstanceName = "localRedis_";
//});
builder.Services.AddDistributedRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisConnectionString"];
    options.InstanceName = "localRedis_";
});

//_______________Logging
//Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

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
builder.Services.AddDistributedRedisCache(option =>
{
    option.Configuration = builder.Configuration["RedisConnectionString"];
});

#endregion

var app = builder.Build();


//_____________________ Create Db When Dosent Exist
#region CreateDbWhenDosentExist
app.CreateDatabase();
app.CreateIdentityDatabase();

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerCustom(builder.Configuration);
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.ConfigureLogExceptionMiddleware();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
