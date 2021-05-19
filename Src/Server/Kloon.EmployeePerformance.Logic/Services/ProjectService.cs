using Kloon.EmployeePerformance.DataAccess;
using Kloon.EmployeePerformance.DataAccess.Domain;
using Kloon.EmployeePerformance.Logic.Caches.Data;
using Kloon.EmployeePerformance.Logic.Common;
using Kloon.EmployeePerformance.Logic.Services.Base;
using Kloon.EmployeePerformance.Models.Common;
using Kloon.EmployeePerformance.Models.Project;
using Kloon.EmployeePerformance.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.Logic.Services
{
    public interface IProjectService
    {
        ResultModel<List<ProjectModel>> GetAll(string searchText = "");
        ResultModel<ProjectModel> GetById(int projectId);
        ResultModel<ProjectModel> Create(ProjectModel projectModel);
        ResultModel<ProjectModel> Update(ProjectModel projectModel);
        ResultModel<bool> Delete(int id);

    }
    public class ProjectService : IProjectService
    {
        private readonly IAuthenLogicService<ProjectService> _logicService;
        private readonly IUnitOfWork<EmployeePerformanceContext> _dbContext;

        private readonly IEntityRepository<Project> _projects;
        private readonly IEntityRepository<ProjectUser> _projectUsers;

        public ProjectService(
            IAuthenLogicService<ProjectService> logicService
        )
        {
            _logicService = logicService;
            _dbContext = logicService.DbContext;

            _projects = _dbContext.GetRepository<Project>();
            _projectUsers = _dbContext.GetRepository<ProjectUser>();
        }

        public ResultModel<List<ProjectModel>> GetAll(string searchText = "")
        {
            var result = _logicService
                .Start()
                .ThenAuthorize(Roles.ADMINISTRATOR, Roles.USER)
                .ThenValidate(currentUser => null)
                .ThenImplement(currentUser =>
                {
                    IEnumerable<ProjectMD> query = null;
                    if (currentUser.Role == Roles.USER)
                    {
                        query = _logicService.Cache.Users.GetProjects(currentUser.Id);
                    }
                    else
                    {
                        query = _logicService.Cache.Projects.GetValues();
                    }

                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        searchText = searchText.Trim();
                        query = query.Where(x => x.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));
                    }

                    var record = query
                        .OrderBy(x => x.Name)
                        .Where(x => x.DeletedBy == null && x.DeletedDate == null)
                        .Select(t => new ProjectModel
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Description = t.Description,
                            Status = t.Status
                        })
                        .ToList();
                    return record;
                });
            return result;
        }

        public ResultModel<ProjectModel> GetById(int projectId)
        {
            Project project = null;
            var result = _logicService
                .Start()
                .ThenAuthorize(Roles.ADMINISTRATOR, Roles.USER)
                .ThenValidate(current =>
                {
                    project = _projects.Query(x => x.Id == projectId && x.DeletedBy == null && x.DeletedDate == null).FirstOrDefault();
                    if (project == null)
                    {
                        return new ErrorModel(ErrorType.NOT_EXIST, "Project not found");
                    }

                    return null;
                })
                .ThenImplement(current =>
                {
                    var projectVM = new ProjectModel()
                    {
                        Id = project.Id,
                        Description = project.Description,
                        Name = project.Name,
                        Status = project.Status
                    };
                    return projectVM;
                });
            return result;
        }

        public ResultModel<ProjectModel> Create(ProjectModel projectModel)
        {
            var now = DateTime.Now;
            var result = _logicService
               .Start()
               .ThenAuthorize(Roles.ADMINISTRATOR)
               .ThenValidate(current =>
               {
                   var error = ValidateProject(projectModel);
                   if (error != null)
                   {
                       return error;
                   }
                   var hasSameProjectName = _logicService.Cache
                        .Projects
                        .GetValues()
                        .Where(x => x.DeletedDate == null && x.DeletedBy == null)
                        .Any(x => x.Name.Equals(projectModel.Name, StringComparison.OrdinalIgnoreCase));
                   if (hasSameProjectName)
                   {
                       return new ErrorModel(ErrorType.DUPLICATED, "The Project Name already exists");
                   }
                   return null;
               })
               .ThenImplement(current =>
               {
                   var project = new Project()
                   {
                       Name = projectModel.Name.Trim(),
                       Status = projectModel.Status,
                       Description = projectModel.Description,
                       CreatedBy = current.Id,
                       CreatedDate = now
                   };
                   _projects.Add(project);

                   int result = _dbContext.Save();
                   _logicService.Cache.Projects.Clear();
                   return projectModel;
               });
            return result;
        }

        public ResultModel<bool> Delete(int id)
        {
            var now = DateTime.Now;
            Project project = null;
            var result = _logicService
                .Start()
                .ThenAuthorize(Roles.ADMINISTRATOR)
                .ThenValidate(currentUser =>
                {
                    project = _projects
                        .Query(x => x.Id == id && x.DeletedBy == null && x.DeletedDate == null)
                        .FirstOrDefault();
                    if (project == null)
                    {
                        return new ErrorModel(ErrorType.NOT_EXIST, "Project with Id = " + id + " not found");
                    }
                    return null;
                })
                .ThenImplement(currentUser =>
                {

                    List<ProjectUser> projectUsers = _projectUsers.Query(x => x.ProjectId == id && x.DeletedBy == null && x.DeletedDate == null)
                        .ToList();

                    foreach (var item in projectUsers)
                    {
                        item.DeletedBy = currentUser.Id;
                        item.DeletedDate = now;
                    }

                    project.DeletedBy = currentUser.Id;
                    project.DeletedDate = now;

                    int result = _dbContext.Save();

                    _logicService.Cache.Projects.Clear();
                    return true;
                });
            return result;
        }

       

        public ResultModel<ProjectModel> Update(ProjectModel projectModel)
        {
            var now = DateTime.Now;
            Project project = null;
            var result = _logicService
               .Start()
               .ThenAuthorize(Roles.ADMINISTRATOR)
               .ThenValidate(current =>
               {
                   var error = ValidateProject(projectModel);
                   if (error != null)
                   {
                       return error;
                   }
                   var hasSameProjectName = _logicService.Cache
                        .Projects
                        .GetValues()
                         .Where(x => x.DeletedDate == null && x.DeletedBy == null)
                        .Any(x => x.Id != projectModel.Id && x.Name.Equals(projectModel.Name, StringComparison.OrdinalIgnoreCase));
                   if (hasSameProjectName)
                   {
                       return new ErrorModel(ErrorType.DUPLICATED, "The Project Name already exists");
                   }
                   project = _projects.Query(x => x.Id == projectModel.Id).FirstOrDefault();
                   if (project == null)
                   {
                       return new ErrorModel(ErrorType.NOT_EXIST, "Project with Id = " + projectModel.Id + " not found");
                   }

                   if (project.DeletedBy != null && project.DeletedDate != null)
                   {
                       return new ErrorModel(ErrorType.NOT_EXIST, "Project with Id = " + projectModel.Id + " not found");
                   }
                   return null;
               })
               .ThenImplement(current =>
               {
                   project.Name = projectModel.Name.Trim();
                   project.Status = projectModel.Status;
                   project.Description = projectModel.Description;
                   project.ModifiedBy = current.Id;
                   project.ModifiedDate = now;
                   int result = _dbContext.Save();

                   _logicService.Cache.Projects.Clear();
                   return projectModel;
               });
            return result;
        }

        private ErrorModel ValidateProject(ProjectModel projectModel)
        {
            if (projectModel == null)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Please fill in the required files");
            }
            if (string.IsNullOrEmpty(projectModel.Name))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Name is required");
            }
            if (projectModel.Name.Length > 50)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Max length of Project name is 50");
            }
            if ((!string.IsNullOrEmpty(projectModel.Description) && projectModel.Description.Length > 500))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Max length of Description is 500");
            }

            if (projectModel.Status <= 0 || projectModel.Status >= 4)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Status must be selected");
            }

            return null;
        }
    }
}
