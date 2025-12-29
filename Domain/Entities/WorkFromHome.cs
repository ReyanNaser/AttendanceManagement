using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class WorkFromHome: Common
    {
        public Guid EmployeeId { get;  set; }
        public DateOnly FromDate { get;  set; }
        public DateOnly ToDate { get;  set; }
        public RequestStatus Status { get;  set; }
        public string? Reason { get;  set; }
    }
}
