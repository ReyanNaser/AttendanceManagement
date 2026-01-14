using Application.Common;
using Application.NotificationService;
using Domain.DTOs;
using Domain.Entities;
using Domain.Entities.Enums;
using Infrastructure.UnitofWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application.Attendance;

public class CheckIn : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/checkin", Handler)
            .WithName("AddAttendance")
            .WithTags("Attendance")
            .RequireAuthorization();
    }

    private static async Task<IResult> Handler(AddAttendanceRequest request,IUnitOfWork db, INotificationService nt, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        
        

        var employeeExists = await db.Employees.AnyAsync(e => e.Id == request.EmployeeId && e.IsActive, cancellationToken);
        if (!employeeExists) 
            return Results.BadRequest("Employee not found.");

        // 2. Check if already checked in today
        var existingRecord = await db.AttendanceRecords.FirstOrDefaultAsync(
            a => a.EmployeeId == request.EmployeeId 
            && a.Date == DateOnly.FromDateTime(now.DateTime) 
            && a.IsActive, cancellationToken);

        if (existingRecord != null) 
            return Results.BadRequest("Already checked in for today.");

        
        var localTime = now.ToLocalTime();
        var hour = localTime.Hour;
        var minute = localTime.Minute;

        AttendanceStatus status;
        if (hour < 9 || (hour == 9 && minute <= 30))
            status = AttendanceStatus.Present; 
        else if (hour == 9 || (hour == 10 && minute <= 15))
            status = AttendanceStatus.Present; 
        else if ((hour == 10 && minute > 15) || (hour == 11 && minute == 0))
            status = AttendanceStatus.Late;    
        else
            status = AttendanceStatus.HalfDay; 

        var record = new AttendanceRecord
        {
            EmployeeId = request.EmployeeId,
            Date = DateOnly.FromDateTime(now.DateTime),
            CheckInTime = now,
            Status = status,
        };

        await db.AttendanceRecords.AddAsync(record, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

         await nt.NotifyAsync(request.EmployeeId, "Attendance Checked In", $"You have successfully checked in on {record.Date}. Status: {record.Status}", cancellationToken);
        return Results.Created("Attendance marked successfully", record);
    }
}

