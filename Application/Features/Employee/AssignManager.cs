using Application.Common;
using AuthServiceProvider.Protos;
using Domain.DTOs;
using Infrastructure.UnitofWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Employee
{
    public class AssignManager: IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut(nameof(AssignManager), Handler)
                .WithName("AssignManager")
                .WithTags("Employee")
                .Produces<EmployeeResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest);
        }
        private static async Task<IResult> Handler(AssignManagerDto request, IUnitOfWork db, RoleService.RoleServiceClient roleClient, CancellationToken cancellationToken)
        {

            if (request.EmployeeIds == null || !request.EmployeeIds.Any())
                return Results.BadRequest("No employees provided.");

            if (request.EmployeeIds.Contains(request.ManagerId))
                return Results.BadRequest("A manager cannot manage themselves.");

            var manager = await db.Employees.FirstOrDefaultAsync(m=> m.Id
            == request.ManagerId);

            if(manager == null)
            {
                return Results.BadRequest("Manager not found.");
            }

            var distempids = request.EmployeeIds.Distinct().ToList();
            var employees = await db.Employees
            .GetManyTracking(e => distempids.Contains(e.Id), cancellationToken);

            if (employees.Count != request.EmployeeIds.Count)
            {
                return Results.NotFound("One or more employees not found.");
            }

            var authRes = await roleClient.PromoteToManagerAsync( new PromotionRequest { Email = manager.Email, Role = "Manager" });

            if (!authRes.Success)
            {       
                return Results.BadRequest($"Auth promotion failed: {authRes.Message}");
            }

            foreach (var employee in employees)
            {
                employee.ManagerId = request.ManagerId;
               
            }

            
            await db.SaveChangesAsync(cancellationToken);

            return Results.Ok("Manager assigned succfully.");
        }
    }
}
