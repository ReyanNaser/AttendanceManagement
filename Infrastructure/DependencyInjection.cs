using Domain.Persistance;
using Infrastructure.NotificationHub;
using Infrastructure.Repository;
using Infrastructure.UnitofWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.Serializers.Json;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AttendanceDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddHttpContextAccessor();

        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

        services.AddSignalR();
        services.AddScoped(typeof(IRealTimeNotifier), typeof(SignalRNotifier));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<INatsConnection>(sp =>
        {
            var opts = new NatsOpts
            {
                Url = "nats://localhost:4222",
                SerializerRegistry = NatsJsonSerializerRegistry.Default,
                Name = "AttendanceService"
            };
            return new NatsConnection(opts);
        });

        services.AddSingleton<INatsJSContext>(sp =>
        {
            var nats = sp.GetRequiredService<INatsConnection>();
            return new NatsJSContext(nats);
        });

        return services;
    }
}
