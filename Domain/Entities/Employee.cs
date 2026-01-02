using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Employee: Common
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public Guid? ManagerId { get; set; }
        //public Manager? ReportingManager { get; set; }
    }
}
