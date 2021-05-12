using System;

namespace Kloon.EmployeePerformance.Logic.Caches.Data
{
    public partial class CriteriaTypeMD
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int OrderNo { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
