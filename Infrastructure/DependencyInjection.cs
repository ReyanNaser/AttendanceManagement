using Infrastructure.Persistance;
using Application.Common.Interfaces;
using Infrastructure.Messaging;
using Infrastructure.Services;
using Infrastructure.NotificationHub;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using NATS.Client.Core;
using NATS.Client.JetStream;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AttendanceDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddHttpContextAccessor();

        services.AddScoped<IAttendanceDbContext>(provider => provider.GetRequiredService<AttendanceDbContext>());

        services.AddSignalR();
        services.AddScoped<IRealTimeNotifier, SignalRNotifier>();
        
        services.AddScoped<IEventPublisher, NatsEventPublisher>();
        services.AddScoped<IIdentityService, GrpcIdentityService>();

        services.AddSingleton<INatsConnection>(sp =>
        {
            var url = configuration["Nats:Url"] ?? "nats://127.0.0.1:4222";
            var opts = NatsOpts.Default with 
            { 
                Url = url,
                SerializerRegistry = NATS.Client.Serializers.Json.NatsJsonSerializerRegistry.Default
            };

            return new NatsConnection(opts);
        });
        // Register the v2 JetStream Context
        services.AddSingleton<INatsJSContext>(sp =>
        {
            var connection = sp.GetRequiredService<INatsConnection>();

            return new NatsJSContext(connection);
        });


        return services;
    }
}
