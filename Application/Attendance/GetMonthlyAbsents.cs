using Application.Common;
using Domain.DTOs;
using Domain.Entities.Enums;
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
    public class GetMonthlyAbsents : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/absents", Handler)
                .WithName("GetMonthlyAbsents")
                .WithTags("Attendance")
                .Produces<List<AbsentDayDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound);
        }
        private static async Task<IResult> Handler(Guid employeeId,int year,int month,IUnitOfWork db,CancellationToken cancellationToken)
        {
            
            var employeeExists = await db.Employees.AnyAsync(e => e.Id == employeeId && e.IsActive, cancellationToken);
            if (!employeeExists)
            {
                return Results.NotFound(new { Error = "Employee not found." });
            }
            
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var startDate = new DateOnly(year, month, 1);
            var endDate = new DateOnly(year, month, daysInMonth);
            
            var records = await db.AttendanceRecords.GetMany(
                a => a.EmployeeId == employeeId 
                && a.Date >= startDate 
                && a.Date <= endDate 
                && a.IsActive, cancellationToken);

            var leaves = await db.LeaveRequests.GetMany(
                l => l.EmployeeId == employeeId 
                && l.FromDate <= endDate 
                && l.ToDate >= startDate                
                && l.IsActive, cancellationToken);

            var wfh = await db.WorkFromHomes.GetMany(
                w => w.EmployeeId == employeeId 
                && w.FromDate <= endDate 
                && w.ToDate >= startDate 
                && w.IsActive, cancellationToken);


            var result = new List<AbsentDayDto>();
         

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateOnly(year, month, day);
                bool isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
               


                var dailyRecord = records.FirstOrDefault(r => r.Date == date);
                var hasLeave = leaves.Any(l => date >= l.FromDate && date <= l.ToDate);
                var hasWfh = wfh.Any(w => date >= w.FromDate && date <= w.ToDate);
                

                bool isAccountedFor = (dailyRecord != null && dailyRecord.Status != AttendanceStatus.Absent) || hasLeave || hasWfh;
                bool isAbsent = !isWeekend && !isAccountedFor;
                var dto = new AbsentDayDto
                {
                    Date = date,
                    DayName = date.DayOfWeek.ToString(),
                    IsWeekend = isWeekend,
                    IsAbsent = isAbsent || (dailyRecord?.Status == AttendanceStatus.Absent)
                };
               

                if (isWeekend)
                {
                    dto.Status = "Weekend";
                }
                else if (dailyRecord?.Status == AttendanceStatus.Absent)
                {
                    dto.Status = "Marked Absent";
                    dto.CanCorrectWithLeave = true;
                    dto.CanCorrectWithWFH = true;
                }
                else if (isAbsent)
                {
                    dto.Status = "Missing Record";
                    dto.CanCorrectWithLeave = true;
                    dto.CanCorrectWithWFH = true;
                }
                else
                {
                    // Day is accounted for
                    dto.Status = dailyRecord != null ? dailyRecord.Status.ToString() : (hasLeave ? "On Leave" : "WFH");
                }
                result.Add(dto);
            }
            return Results.Ok(result);
        }
    }
}
