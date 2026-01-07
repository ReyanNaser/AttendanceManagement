using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.Enums
{
    public enum AttendanceStatus
    {
        Present = 1,
        Absent = 2,
        Late = 4,
        HalfDay = 5
    }
}
