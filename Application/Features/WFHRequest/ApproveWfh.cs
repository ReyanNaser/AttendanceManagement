using Application.Common;
using Domain.DTOs;
using Domain.Entities.Enums;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.WFHRequest
{
    public class ApproveWfh : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPut(nameof(ApproveWfh), Handler)            
            .WithTags("Manager Actions")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(policy => policy.RequireRole("Manager"));
        }
        private static async Task<IResult> Handler(ApprovalRequestDto request, IAttendanceDbContext db, CancellationToken cancellationToken)
        {
            var wfh = await db.WorkFromHomes.FirstOrDefaultAsync(a => a.Id == request.RequestId);
            if (wfh == null)
            {
                return Results.NotFound("Request not found");
            }

            var employee = await db.Employees.FirstOrDefaultAsync(e => e.Id == wfh.EmployeeId);
            if (employee == null)
            {
                return Results.NotFound("Employee Not Found");
            }

            if(employee.ManagerId != request.ManagerId)
            {
                return Results.BadRequest("Unauthorized.");
            }

            wfh.Status = request.IsApproved ? RequestStatus.Approved : RequestStatus.Rejected;

            await db.SaveChangesAsync(cancellationToken);
            return Results.Ok("Work from home request processed successfully");
        }
    }
}
