using Application.Common;
using Application.Common.RouteValidation;
using Application.EmailService;
using Application.GrpcService;
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
               .WithRequestValidation<CreateEmployeeRequest>()
               .Produces<EmployeeResponse>(StatusCodes.Status201Created)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .RequireAuthorization(policy => policy.RequireRole("Manager"));
        }       

        public class RequestValidator: AbstractValidator<CreateEmployeeRequest>
        {
            public RequestValidator() 
            {
                RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);

                RuleFor(x => x.LastName)
                    .NotEmpty()
                    .MaximumLength(50);

                RuleFor(x => x.Email)
                    .NotEmpty()
                    .EmailAddress()
                    .MaximumLength(100);

                RuleFor(x => x.Address)
                    .NotEmpty()
                    .MaximumLength(200);

                RuleFor(x => x.City)
                    .NotEmpty()
                    .MaximumLength(50);

                RuleFor(x => x.Designation)
                    .NotEmpty()
                    .MaximumLength(100);

                RuleFor(x => x.Department)
                    .NotEmpty()
                    .MaximumLength(100);

                RuleFor(x => x.ManagerId)
                    .NotEqual(Guid.Empty)
                    .When(x => x.ManagerId.HasValue)
                    .WithMessage("ManagerId must be a valid GUID.");
            }
        }
        private async Task<IResult> Handler(
            CreateEmployeeRequest request,
            IUnitOfWork db,
            GrpcClient grpcClient,
            IEmailSender emailSender,
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


            await emailSender.SenEmailAsync(
                request.Email,
                "Welcome to the Company",
                $"Hello {request.FirstName},\n\nWelcome to the company!\n\nRegards,\nHR Team"
            );
            return Results.Created($"{employee.Id}",request);
        }
    }
}
