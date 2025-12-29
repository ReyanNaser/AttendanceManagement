using Application.Common;
using Host.Common;
using Domain.Persistance;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AttendanceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register Infrastructure (UnitOfWork, Repositories)
builder.Services.AddInfrastructure();

// Register all endpoints from Application assembly dynamically
builder.Services.AddEndpoints(typeof(IEndpoint).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Map all minimal API endpoints dynamically
app.MapEndpoints();

app.Run();
