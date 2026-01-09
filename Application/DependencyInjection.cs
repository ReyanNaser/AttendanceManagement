using Application.EmailService;
using Application.GrpcService;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Added
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens; // Added
using System;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<GrpcClient>();

           
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
    }
}