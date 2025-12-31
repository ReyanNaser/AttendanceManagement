using System;

namespace Domain.Entities
{
    public class Manager : Common
    {
        public Guid EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Designation { get; set; }
        public ICollection<Employee> ManagedEmployees { get; set; }
    }
}