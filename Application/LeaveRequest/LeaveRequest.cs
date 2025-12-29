using Application.Common;
using Domain.DTOs;
using Infrastructure.UnitofWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.LeaveRequest
{
    public class LeaveRequest: IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/leaverequest", Handler)
                .WithName("LeaveRequest")
                .WithTags("LeaveRequest");
        }
        private static async Task<IResult> Handler(LeaveRequestDto request, IUnitOfWork db, CancellationToken cancellationToken)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var employeeExists = await db.Employees
           .AnyAsync(e => e.Id == request.EmployeeId && e.IsActive, cancellationToken);

            if (!employeeExists)
            {
                return Results.BadRequest("Employee not found or inactive." );
            }

            if (request.FromDate < today || request.ToDate < today)
            {
                return Results.BadRequest("Leave dates cannot be in the past." );
            }

            var duplicateLeave = await db.LeaveRequests
            .AnyAsync(a => a.EmployeeId == request.EmployeeId
                     && a.FromDate <= request.ToDate &&
                        a.ToDate >= request.FromDate
                        && a.IsActive, cancellationToken);

            if (duplicateLeave)
            {
                return Results.BadRequest("Leave request already applied for this date." );
            }

            var leaveRequest = new Domain.Entities.LeaveRequest
            {
                EmployeeId = request.EmployeeId,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                LeaveType = request.LeaveType,
                Status = request.Status,
                Reason = request.Reason
            };

            await db.LeaveRequests.AddAsync(leaveRequest, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            return Results.Ok("Leave request submitted successfully." );
        }
    }
}
