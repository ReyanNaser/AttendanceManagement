using Application.Common;
using Domain.DTOs;
using Domain.Entities.Enums;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.LeaveRequest;

public class ApproveLeave : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(nameof(ApproveLeave), Handler)
            .WithName("ApproveLeave")
            .WithTags("Manager Actions")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization(policy => policy.RequireRole("Manager"));
    }

    private static async Task<IResult> Handler(ApprovalRequestDto request, IAttendanceDbContext db,CancellationToken ct)
    {
        var leave = await db.LeaveRequests.FirstOrDefaultAsync(l => l.Id == request.RequestId, ct);
        if (leave == null) 
            return Results.BadRequest("Leave request not found.");


        var employee = await db.Employees.FirstOrDefaultAsync(e => e.Id == leave.EmployeeId, ct);
        if (employee == null || employee.ManagerId != request.ManagerId)
        {
            return Results.BadRequest("Unauthorized: You are not the manager for this employee.");
        }

        leave.Status = request.IsApproved ? LeaveStatus.Approved : LeaveStatus.Rejected;
       

        await db.SaveChangesAsync(ct);
        return Results.Ok("Leave request processed.");
    }
}
