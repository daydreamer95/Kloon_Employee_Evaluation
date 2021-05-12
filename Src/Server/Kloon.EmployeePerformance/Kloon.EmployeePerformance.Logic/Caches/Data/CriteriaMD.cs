using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.Logic.Caches.Data
{
    public partial class CriteriaMD
    {
        public int Id { get; set; }
        public int CriteriaTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrderNo { get; set; }
    }
}
