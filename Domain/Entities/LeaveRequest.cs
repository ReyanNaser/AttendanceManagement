using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class LeaveRequest: Common
    {
        public Guid EmployeeId { get;  set; }
        public DateOnly FromDate { get;  set; }
        public DateOnly ToDate { get;  set; }
        public LeaveType LeaveType { get;  set; }
        public LeaveStatus Status { get;  set; }
        public string? Reason { get;  set; }
    }
}
