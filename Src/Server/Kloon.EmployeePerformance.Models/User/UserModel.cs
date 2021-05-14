using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Position { get; set; }
        public bool Gender { get; set; }
        public DateTime Dob { get; set; }
        public string PhoneNo { get; set; }
        public int Role { get; set; }
    }
}
