using Application.Common;
using Application.GrpcService;
using Domain.Persistance;
using Host.Common;
using Host.Middleware;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Application.Employee.CreateEmployee;
using FluentValidation;
using FluentValidation.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AttendanceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructure();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddValidatorsFromAssemblyContaining<RequestValidator>();


// Register all endpoints from Application assembly dynamically
builder.Services.AddEndpoints(typeof(IEndpoint).Assembly);

// Register gRPC Client for Auth Service
builder.Services.AddGrpcClient<AuthServiceProvider.Protos.AuthService.AuthServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:7144"); // Address of AuthServiceProvider
});

builder.Services.AddGrpcClient<AuthServiceProvider.Protos.RoleService.RoleServiceClient>(o =>
{
    o.Address = new Uri("https://localhost:7144");
});

builder.Services.AddScoped<GrpcClient>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Authentication:Authority"];
        options.Audience = builder.Configuration["Authentication:Audience"];
        options.RequireHttpsMetadata = false; // Set to true in production

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuers = new[] { builder.Configuration["Authentication:Authority"], builder.Configuration["Authentication:Authority"]?.TrimEnd('/') + "/" }
            // RoleClaimType = "role" // Removed to allow default mapping (role -> ClaimTypes.Role) to work
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();
app.MapControllers();

// Map all minimal API endpoints dynamically
app.MapEndpoints();

app.Run();
