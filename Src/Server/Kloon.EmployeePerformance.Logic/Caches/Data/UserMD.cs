using System;

namespace Kloon.EmployeePerformance.Logic.Caches.Data
{
    public partial class UserMD
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PositionId { get; set; }
        public bool Sex { get; set; }
        public DateTime DoB { get; set; }
        public string PhoneNo { get; set; }
        public int RoleId { get; set; }
    }
}
