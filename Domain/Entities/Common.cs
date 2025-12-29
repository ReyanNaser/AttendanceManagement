using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Common
    {
        public Guid Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
