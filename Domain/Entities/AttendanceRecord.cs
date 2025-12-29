using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class AttendanceRecord: Common
    {
        public Guid EmployeeId { get; set; }
        public DateOnly Date { get; set; }
        public DateTimeOffset CheckInTime { get; set; }
        public DateTimeOffset? CheckOutTime { get; set; }
        public AttendanceStatus Status { get; set; }
    }
}
