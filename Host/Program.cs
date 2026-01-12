using Application;
using Application.Common;
using Application.GrpcService;
using Domain.Persistance;
using FluentValidation;
using Host.Common;
using Host.Middleware;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
// Removed: Microsoft.AspNetCore.Authentication.JwtBearer and Microsoft.IdentityModel.Tokens
using static Application.Employee.CreateEmployee;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// This single line now handles Business Services + gRPC + Auth
builder.Services.AddBusinessLayer(builder.Configuration);

// Register all endpoints from Application assembly dynamically
builder.Services.AddEndpoints(typeof(IEndpoint).Assembly);

// --- All gRPC, AddAuthentication, and AddAuthorization blocks removed from here ---

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