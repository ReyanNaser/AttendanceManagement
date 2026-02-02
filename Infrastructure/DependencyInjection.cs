using Domain.Persistance;
using Infrastructure.Repository;
using Infrastructure.UnitofWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AttendanceDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddHttpContextAccessor();

        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
