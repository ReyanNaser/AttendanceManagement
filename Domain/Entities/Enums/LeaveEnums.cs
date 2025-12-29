using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Enums
{
    public enum LeaveType
    {
        SickLeave = 1,
        CasualLeave = 2,
        MaternityLeave = 3,
        PaternityLeave = 4,
        AnnualLeave = 5
    }

    public enum LeaveStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Cancelled = 4
    }
}
