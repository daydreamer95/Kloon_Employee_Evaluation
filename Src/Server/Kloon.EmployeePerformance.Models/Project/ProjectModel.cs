using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.Models.Project
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        //public int? DeletedBy { get; set; }
        //public DateTime? DeletedDate { get; set; }

        public string StatusText
        {
            get
            {
                if (Status == 1)
                    return "Open";
                else if (Status == 2)
                    return "Pending";
                else if (Status == 3)
                    return "Closed";
                return "";
            }
        }
    }
}
