using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ApprovalRequestDto
    {
        public Guid RequestId { get; set; }
        public Guid ManagerId { get; set; }
        public bool IsApproved { get; set; }
        public string? Remarks { get; set; }
    }
}
