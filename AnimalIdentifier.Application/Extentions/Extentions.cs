using AnimalIdentifier.Application.Animals.Mapping;
using AnimalIdentifier.Application.Behaviors;
using AnimalIdentifier.Application.Validations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AnimalIdentifier.Application.Extentions;

public static class Extentions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        
        return services;
    }
}
