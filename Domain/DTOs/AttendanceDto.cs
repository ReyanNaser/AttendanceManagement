using Domain.Entities.Enums;
using System;

namespace Domain.DTOs
{
    public class AddAttendanceRequest
    {
        public Guid EmployeeId { get; set; }
        public DateOnly Date { get; set; }
        public DateTimeOffset CheckInTime { get; set; }
        public DateTimeOffset? CheckOutTime { get; set; }
        public AttendanceStatus Status { get; set; }
    }

    public class MonthlyAttendanceResponse
    {
        public Guid EmployeeId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public List<AttendanceDayDto> Days { get; set; } = new();
    }
    public class AttendanceDayDto
    {
        public DateOnly Date { get; set; }
        public string DayName { get; set; } = string.Empty;
        public DateTimeOffset? CheckInTime { get; set; }
        public DateTimeOffset? CheckOutTime { get; set; }
        public AttendanceStatus? Status { get; set; }
        public bool IsWeekend { get; set; }
    }
}
