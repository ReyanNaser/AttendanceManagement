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

    public class AddAttendanceResponse
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public AttendanceStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
