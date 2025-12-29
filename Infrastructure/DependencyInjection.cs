using Infrastructure.Repository;
using Infrastructure.UnitofWork;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register generic repository
        // Register generic repository
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        
        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
