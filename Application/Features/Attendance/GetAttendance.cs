using Application.Common;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Attendance
{
    public class GetAttendance : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/getattendance", Handler)
                .WithName("GetAttendance")
                .WithTags("Attendance")
                .Produces<Domain.Entities.AttendanceRecord>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .RequireAuthorization();
        }

        private static async Task<IResult> Handler(Guid Id, IAttendanceDbContext db, CancellationToken cancellationToken)
        {
            var attendance = await db.AttendanceRecords.FirstOrDefaultAsync(
                a => a.EmployeeId == Id, cancellationToken);
            if (attendance == null)
            {
                return Results.NotFound("Attendance Not Found");
            }
            return Results.Ok(attendance);
        }
    }
}
