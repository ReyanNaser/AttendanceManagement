using Application.EmailService;
using Application.GrpcService;
using Application.NotificationService;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.Serializers.Json;
using static Application.Employee.CreateEmployee;
using Scrutor;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<GrpcClient>();
            services.AddValidatorsFromAssemblyContaining<RequestValidator>();
            services.AddScoped<INotificationService, NotificationService.NotificationService>();



            services.AddGrpcClient<AuthServiceProvider.Protos.AuthService.AuthServiceClient>(o =>
            {
                o.Address = new Uri("https://localhost:7144");
            });

            services.AddGrpcClient<AuthServiceProvider.Protos.RoleService.RoleServiceClient>(o =>
            {
                o.Address = new Uri("https://localhost:7144");
            });

            // Register Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["Authentication:Authority"];
                    options.Audience = configuration["Authentication:Audience"];
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuers = new[] { configuration["Authentication:Authority"], configuration["Authentication:Authority"]?.TrimEnd('/') + "/" }
                    };
                });

            // Register Authorization
            services.AddAuthorization();

            return services;
        }




        //public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        //{
        //    // 🔹 Scrutor: scan Business layer
        //    services.Scan(scan => scan
        //        .FromAssemblyOf<NotificationService.NotificationService>()

        //        // IEmailSender → EmailSender
        //        .AddClasses(classes => classes.AssignableTo<IEmailSender>())
        //            .AsImplementedInterfaces()
        //            .WithScopedLifetime()

        //        // INotificationService → NotificationService
        //        .AddClasses(classes => classes.AssignableTo<INotificationService>())
        //            .AsImplementedInterfaces()
        //            .WithScopedLifetime()
        //    );

        //    // Manual registrations (not suitable for Scrutor)
        //    services.AddScoped<GrpcClient>();

        //    services.AddValidatorsFromAssemblyContaining<RequestValidator>();

        //    // gRPC clients (must be explicit)
        //    services.AddGrpcClient<AuthServiceProvider.Protos.AuthService.AuthServiceClient>(o =>
        //    {
        //        o.Address = new Uri("https://localhost:7144");
        //    });

        //    services.AddGrpcClient<AuthServiceProvider.Protos.RoleService.RoleServiceClient>(o =>
        //    {
        //        o.Address = new Uri("https://localhost:7144");
        //    });

        //    // Authentication
        //    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //        .AddJwtBearer(options =>
        //        {
        //            options.Authority = configuration["Authentication:Authority"];
        //            options.Audience = configuration["Authentication:Audience"];
        //            options.RequireHttpsMetadata = false;

        //            options.TokenValidationParameters = new TokenValidationParameters
        //            {
        //                ValidateIssuer = true,
        //                ValidIssuers = new[]
        //                {
        //                configuration["Authentication:Authority"],
        //                configuration["Authentication:Authority"]?.TrimEnd('/') + "/"
        //                }
        //            };
        //        });

        //    services.AddAuthorization();

        //    return services;
        //}

    }
}