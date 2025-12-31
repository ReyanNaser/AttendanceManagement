using System;

namespace Domain.DTOs
{
    public class CreateEmployeeRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public Guid ManagerId { get; set; }
    }

    public class EmployeeResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public bool IsActive { get; set; }
    }
}
