using Application.Common;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.UnitofWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.Attendance;

public class AddAttendance : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/attendance", Handler)
            .WithName("AddAttendance")
            .WithTags("Attendance");
    }

    private static async Task<IResult> Handler(AddAttendanceRequest request, IUnitOfWork db, CancellationToken cancellationToken)
    {
        
        var employeeExists = await db.Employees
            .AnyAsync(e => e.Id == request.EmployeeId && e.IsActive, cancellationToken);

        if (!employeeExists)
        {
            return Results.BadRequest(new { Error = "Employee not found or inactive." });
        }

        
        var duplicateAttendance = await db.AttendanceRecords
            .AnyAsync(a => a.CreatedBy == request.EmployeeId.ToString() 
                        && a.Date == request.Date 
                        && a.IsActive, cancellationToken);

        if (duplicateAttendance)
        {
            return Results.BadRequest(new { Error = "Attendance already recorded for this date." });
        }

        var attendanceRecord = new AttendanceRecord
        {
            EmployeeId = request.EmployeeId,
            Date = request.Date,
            CheckInTime = request.CheckInTime.ToUniversalTime(),
            CheckOutTime = request.CheckOutTime?.ToUniversalTime(),
            Status = request.Status
        };

        await db.AttendanceRecords.AddAsync(attendanceRecord, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        

        return Results.Created($"/api/attendance/{attendanceRecord.Id}", request);
    }
}
