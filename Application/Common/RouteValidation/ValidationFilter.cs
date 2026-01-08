using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.RouteValidation;

public sealed class ValidationFilter<T> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices
            .GetRequiredService<IValidator<T>>();

        var argument = context.Arguments
            .OfType<T>()
            .FirstOrDefault();

        if (argument is null)
            return Results.BadRequest("Invalid request payload.");

        var result = await validator.ValidateAsync(argument);

        if (!result.IsValid)
        {
            var errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

            return Results.ValidationProblem(errors);
        }

        return await next(context);
    }
}
