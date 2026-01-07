using Application.Common;
using Domain.DTOs;
using Infrastructure.UnitofWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.Employee
{
    public class GetEmployee : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/getemployee", Handler)
                .WithName("GetEmployee")
                .WithTags("Employee")
                .Produces<EmployeeResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .RequireAuthorization(policy =>policy.RequireRole("Manager"));
        }
        private static async Task<IResult> Handler(Guid Id, IUnitOfWork db, CancellationToken cancellationToken)
        {
            var employee = await db.Employees.GetByIdAsync(Id);
            if (employee == null)
            {
                return Results.NotFound("Employee Not Found");
            }
            return Results.Ok(employee);
        }
    }
}
