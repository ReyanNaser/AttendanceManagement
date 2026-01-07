using Application.Common;
using Domain.DTOs;
using Domain.Entities.Enums;
using Infrastructure.UnitofWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.WFHRequest
{
    public class WorkFromHomeRequest: IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/wfhrequest", Handler)
                .WithTags("WFHRequest")
                .WithName("WorkFromHomeRequest")
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .RequireAuthorization();
        }
        private static async Task<IResult> Handler(WorkFromHomeRequestDto request, IUnitOfWork db, CancellationToken cancellationToken)
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
                return Results.BadRequest("Work from home dates cannot be in the past." );
            }

            var leavecheck = await db.LeaveRequests
            .AnyAsync(a => a.EmployeeId == request.EmployeeId
                     && a.FromDate <= request.ToDate &&
                        a.ToDate >= request.FromDate
                        && a.IsActive, cancellationToken);

            if (leavecheck)
            {
                return Results.BadRequest("Leave request already applied for this date." );
            }

            var duplicaterequest = await db.WorkFromHomes
            .AnyAsync(a => a.EmployeeId == request.EmployeeId
                     && a.FromDate <= request.ToDate &&
                        a.ToDate >= request.FromDate
                        && a.IsActive, cancellationToken);

            if (duplicaterequest)
            {
                return Results.BadRequest("Work from home request already applied for this date.");
            }

            var wfhrequest = new Domain.Entities.WorkFromHome
            {
                EmployeeId = request.EmployeeId,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                Status = RequestStatus.Pending,
                Reason = request.Reason
            };

            await db.WorkFromHomes.AddAsync(wfhrequest, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            return Results.Ok("Work from home request submitted successfully.");
        }
    }
}
