using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.RouteValidation;

public static class ValidationExtensions
{
    public static RouteHandlerBuilder WithRequestValidation<T>(
    this RouteHandlerBuilder builder)
    {
        return builder.AddEndpointFilter<ValidationFilter<T>>();
    }
}
