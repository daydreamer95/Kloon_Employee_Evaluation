namespace Kloon.EmployeePerformance.Logic.Caches.Data
{
    public partial class ProjectMD
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public int? ProjectRoleId { get; set; }
    }
}
