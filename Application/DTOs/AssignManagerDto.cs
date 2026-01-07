using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AssignManagerDto
    {
        public Guid ManagerId { get; set; }
        public List<Guid>? EmployeeIds {  get; set; }
    }
}
