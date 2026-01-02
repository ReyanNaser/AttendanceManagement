using Application.Common;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.UnitofWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.Employee
{
    public class CreateEmployee : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/employees", Handler)
                .WithName("CreateEmployee")
                .WithTags("Employee")
                .Produces<EmployeeResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }

        private static async Task<IResult> Handler(
            CreateEmployeeRequest request,
            IUnitOfWork db,
            AuthServiceProvider.Protos.AuthService.AuthServiceClient authClient,
            CancellationToken cancellationToken)
        {
            // Check if email already exists
            var emailExists = await db.Employees
                .AnyAsync(e => e.Email == request.Email, cancellationToken);

            if (emailExists)
            {
                return Results.BadRequest(new { Error = "Email already exists." });
            }

            var employee = new Domain.Entities.Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Address = request.Address,
                City = request.City,
                Designation = request.Designation,
                Department = request.Department,
                ManagerId = request.ManagerId,
            };

            await db.Employees.AddAsync(employee, cancellationToken);

            
            try 
            {
                var grpcRequest = new AuthServiceProvider.Protos.CreateUserRequest
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Role = "Employee"
                };

                var authResponse = await authClient.CreateUserAsync(grpcRequest, cancellationToken: cancellationToken);

                if (!authResponse.Success)
                {
                    return Results.BadRequest(new { Error = $"Auth User Creation Failed: {authResponse.Message}" });
                }
            }
            catch (Exception ex)
            {
                 // Log error
                 return Results.Problem($"gRPC Connection Failed: {ex.Message}");
            }

            await db.SaveChangesAsync(cancellationToken);            

            return Results.Created($"{employee.Id}",request);
        }
    }
}
