using Application.Common;
using Application.GrpcService;
using AuthServiceProvider.Protos;
using Domain.DTOs;
using FluentValidation;
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
               // .WithRequestValidation<CreateEmployeeRequest>()
                .Produces<EmployeeResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .RequireAuthorization(policy => policy.RequireRole("Admin"));
        }       

        private async Task<IResult> Handler(
            CreateEmployeeRequest request,
            IUnitOfWork db,
            GrpcClient grpcClient,
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

            
            
            var grpcRequest = new AuthServiceProvider.Protos.CreateUserRequest
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Role = "Employee"
            };

            var authResponse = await grpcClient.CreateUserAsync(grpcRequest, cancellationToken);

            if (!authResponse.Success)
            {
                return Results.BadRequest(new { Error = $"Auth User Creation Failed: {authResponse.Message}" });
            }

            if(request.ManagerId.HasValue)
            {
                var manager = await db.Employees
                    .FirstOrDefaultAsync(e => e.Id == request.ManagerId);
                if (manager == null)
                {
                    return Results.BadRequest("Manager not found.");
                }

                var grpcPromotionRequest = new AuthServiceProvider.Protos.PromotionRequest
                {
                    Email = manager.Email,
                    Role = "Manager"
                };

                var authRes = await grpcClient.PromoteToManagerAsync(grpcPromotionRequest, cancellationToken);

            }





            await db.SaveChangesAsync(cancellationToken);            

            return Results.Created($"{employee.Id}",request);
        }
    }
}
