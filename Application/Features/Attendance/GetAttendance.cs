using Application.Common;
using Infrastructure.UnitofWork;
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
                .ProducesProblem(StatusCodes.Status404NotFound);
        }

        private static async Task<IResult> Handler(Guid Id, IUnitOfWork db, CancellationToken cancellationToken)
        {
            var attendance = await db.AttendanceRecords.FindAsync(
                a => a.EmployeeId == Id);
            if (attendance == null)
            {
                return Results.NotFound("Attendance Not Found");
            }
            return Results.Ok(attendance);
        }
    }
}
