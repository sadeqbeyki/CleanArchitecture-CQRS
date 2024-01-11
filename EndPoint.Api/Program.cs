using Application;
using Application.Mapper;
using Autofac.Core;
using EndPoint.Api.Helper;
using Identity.Application;
using Identity.Application.Mapper;
using Identity.Infrastructure;
using Infrastructure;
using Infrastructure.ACL;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//_______________Ilogger - EventSource



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
    //app.UseSwagger();
    app.UseSwaggerCustom(builder.Configuration);
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
