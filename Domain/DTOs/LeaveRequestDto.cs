using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class LeaveRequestDto
    {
        public Guid EmployeeId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public LeaveStatus Status { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
