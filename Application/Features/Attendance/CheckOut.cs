using Application.Common;
using Domain.DTOs;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.Attendance
{
    public class CheckOut : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/checkout", Handler)
                .WithTags("CheckOuts")
                .Produces(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .RequireAuthorization();
        }
        private static async Task<IResult> Handler(AddAttendanceRequest request, IAttendanceDbContext db, CancellationToken cancellationToken)
        {
            var now = DateTimeOffset.UtcNow;            


            var record = await db.AttendanceRecords.FirstOrDefaultAsync(
                a => a.EmployeeId == request.EmployeeId 
                && a.Date == DateOnly.FromDateTime(now.DateTime)
                && a.IsActive, cancellationToken);

            if (record == null)
                return Results.BadRequest( "No check-in record found for today.");

            if (record.CheckOutTime != null)
                return Results.BadRequest( "Already checked out for today.");
            
            record.CheckOutTime = now;

            await db.SaveChangesAsync(cancellationToken);
            return Results.Ok("CheckedOut Successfully");
        }

    }
}
