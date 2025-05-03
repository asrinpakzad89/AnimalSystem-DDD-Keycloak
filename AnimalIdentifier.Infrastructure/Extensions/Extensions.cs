using AnimalIdentifier.Domain.AggregatesModel.AnimalAggregate;
using AnimalIdentifier.Domain.Seedwork;
using AnimalIdentifier.Infrastructure.Data;
using AnimalIdentifier.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace AnimalIdentifier.Infrastructure.Extentions;

public static class Extensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AnimalDbContext>(options =>
           options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        AddRepositories(services);
        return services;
    }
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAnimalRepository), typeof(AnimalRepository));
    }
}
