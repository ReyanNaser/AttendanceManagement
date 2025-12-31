using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AbsentDayDto
    {
        public DateOnly Date { get; set; }
        public string DayName { get; set; } = string.Empty;
        public bool IsWeekend { get; set; }
        public bool IsAbsent { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool CanCorrectWithLeave { get; set; }
        public bool CanCorrectWithWFH { get; set; }
    }
}
