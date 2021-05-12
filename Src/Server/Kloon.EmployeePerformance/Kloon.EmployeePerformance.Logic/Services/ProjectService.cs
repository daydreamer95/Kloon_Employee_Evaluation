using Kloon.EmployeePerformance.DataAccess;
using Kloon.EmployeePerformance.DataAccess.Domain;
using Kloon.EmployeePerformance.Logic.Services.Base;
using Kloon.EmployeePerformance.Models.Common;
using Kloon.EmployeePerformance.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.Logic.Services
{
    public interface IProjectService
    {
        ResultModel<ProjectViewModel> GetAll(string searchText = "");
    }
    public class ProjectService : IProjectService
    {
        private readonly IAuthenLogicService<ProjectService> _logicService;
        private readonly IUnitOfWork<EmployeePerformanceContext> _dbContext;

        private readonly IEntityRepository<Project> _project;

        public ProjectService(
            IAuthenLogicService<ProjectService> logicService
        )
        {
            _logicService = logicService;
            _dbContext = logicService.DbContext;

            _project = _dbContext.GetRepository<Project>();
        }
        public ResultModel<ProjectViewModel> GetAll(string searchText = "")
        {
            var result = _logicService
                .Start()
                .ThenAuthorize()
                .ThenValidate(current => null)
                .ThenImplement(current =>
                {
                    ProjectViewModel projectViewModel = new ProjectViewModel();
                    var query = _project.Query();
                    var record = query
                        .OrderBy(x => x.Name)
                        .Select(t => new ProjectModel
                        {
                        })
                        .ToList();
                    projectViewModel.ProjectModels = record;
                    return projectViewModel;
                });
            return result;
        }
    }
}
