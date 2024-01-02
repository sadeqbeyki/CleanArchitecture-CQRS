using Application.DTOs;
using Application.Validation;
using Domain.Entities.Products;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ServiceExtentions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(conf => conf.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped<IValidator<AddProductDto>, ProductValidator>();

    }
}
