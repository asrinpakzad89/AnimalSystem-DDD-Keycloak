using AnimalIdentifier.Application.Animals.Mapping;
using AnimalIdentifier.Application.Commands;
using AnimalIdentifier.Application.Validations;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalIdentifier.Application.Extentions;

public static class Extentions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateAnimalCommandValidator>();
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        services.AddAutoMapper(typeof(AnimalProfile).Assembly);
        return services;
    }
}
