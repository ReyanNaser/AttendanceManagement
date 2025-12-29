using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Enums;

namespace Domain.Entities
{
    public class WorkFromHome: Common
    {
        public Guid EmployeeId { get; private set; }
        public DateOnly FromDate { get; private set; }
        public DateOnly ToDate { get; private set; }
        public RequestStatus Status { get; private set; }
        public string? Reason { get; private set; }
    }
}
