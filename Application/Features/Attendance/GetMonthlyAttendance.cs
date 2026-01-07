using Application.Common;
using Domain.DTOs;
using Infrastructure.UnitofWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Application.Attendance
{
    public class GetMonthlyAttendance : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/attendancemonthly", Handler)
            .WithTags("Attendance")
            .WithName("GetMonthlyAttendance")
            .Produces<MonthlyAttendanceResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization();
        }
        private static async Task<IResult> Handler(Guid employeeId, int year, int month, IUnitOfWork db, CancellationToken cancellationToken)
        {
            var employeeExists = await db.Employees.AnyAsync(e => e.Id == employeeId && e.IsActive, cancellationToken);
            if (!employeeExists)            
                return Results.NotFound(new { Error = "Employee not found." });

            var daysInMonth = DateTime.DaysInMonth(year, month);
            var startDate = new DateOnly(year, month, 1);
            var endDate = new DateOnly(year, month, daysInMonth);            

            var records = await db.AttendanceRecords.GetMany(
                a => a.EmployeeId == employeeId
                     && a.Date >= startDate
                     && a.Date <= endDate
                     && a.IsActive,
                cancellationToken);

            var response = new MonthlyAttendanceResponse
            {
                EmployeeId = employeeId,
                Year = year,
                Month = month
            };

            var recordDict = records.ToDictionary(r => r.Date);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateOnly(year, month, day);

                recordDict.TryGetValue(date, out var record);

                bool isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

                response.Days.Add(new AttendanceDayDto
                {
                    Date = date,
                    DayName = date.DayOfWeek.ToString(),
                    CheckInTime = record?.CheckInTime,
                    CheckOutTime = record?.CheckOutTime,
                    Status = record?.Status,
                    IsWeekend = isWeekend
                });
            }

            return Results.Ok(response);
        }


    }
}
