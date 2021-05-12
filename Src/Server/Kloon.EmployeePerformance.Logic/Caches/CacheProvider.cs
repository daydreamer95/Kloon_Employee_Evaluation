using Kloon.EmployeePerformance.DataAccess;
using Kloon.EmployeePerformance.DataAccess.Domain;
using Kloon.EmployeePerformance.Logic.Caches.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.Logic.Caches
{
    public class CacheProvider
    {
        private readonly IServiceProvider _serviceProvider;

        #region Users

        public UserAllCache Users { get; private set; }


        private DataKeyCache<int, List<ProjectMD>> _userProjects = new DataKeyCache<int, List<ProjectMD>>();

        #endregion

        #region Criterias

        public CriteriaTypeAllCache CriteriaTypes { get; private set; }

        private DataKeyCache<int, List<CriteriaMD>> _criterias = new DataKeyCache<int, List<CriteriaMD>>();

        public CriteriaAllCache Criterias { get; private set; } = new CriteriaAllCache();

        #endregion

        #region Other

        public ProjectAllCache Projects { get; private set; } = new ProjectAllCache();
        public PositionAllCache Position { get; private set; } = new PositionAllCache();

        #endregion

        #region Constructor and private function

        public CacheProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            Users = new UserAllCache(this);
            CriteriaTypes = new CriteriaTypeAllCache(this);

            RegisterData();
        }

        private T UserDbContext<T>(Func<IUnitOfWork<EmployeePerformanceContext>, T> func)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<IUnitOfWork<EmployeePerformanceContext>>();
                return func.Invoke(dbContext);
            }
        }

        private void RegisterData()
        {
            Users.OnNeedResource += Users_OnNeedResource;
            _userProjects.OnNeedValueIfKeyNotFound += _userProject_OnNeedValueIfKeyNotFound;

            CriteriaTypes.OnNeedResource += CriteriaTypes_OnNeedResource;
            _criterias.OnNeedValueIfKeyNotFound += _criterias_OnNeedValueIfKeyNotFound;

            Criterias.OnNeedResource += Criterias_OnNeedResource;

            Projects.OnNeedResource += Projects_OnNeedResource;
            Position.OnNeedResource += Position_OnNeedResource;
        }

        #endregion

        #region Events

        #region Users

        private Dictionary<int, UserMD> Users_OnNeedResource(object sender, params object[] paramArrs)
        {
            return UserDbContext(dbContext =>
            {
                var result = dbContext.GetRepository<User>()
                    .Query()
                    .ToDictionary(t => t.Id, t => new UserMD
                    {
                        Id = t.Id,
                        FirstName = t.FirstName,
                        LastName = t.LastName,
                        Email = t.Email,
                        DoB = t.DoB,
                        PhoneNo = t.PhoneNo,
                        PositionId = t.PositionId,
                        RoleId = t.RoleId,
                        Sex = t.Sex,
                        DeletedBy = t.DeletedBy,
                        DeletedDate = t.DeletedDate
                    });
                return result;
            });
        }

        private List<ProjectMD> _userProject_OnNeedValueIfKeyNotFound(object sender, params object[] paramArrs)
        {
            return UserDbContext(dbContext =>
            {
                var userId = (int)paramArrs.First();

                var data = dbContext.GetRepository<ProjectUser>()
                    .Query(x => x.UserId == userId)
                    .ToDictionary(t => t.ProjectId, t => new
                    {
                        ProjectId = t.ProjectId,
                        ProjectRoleId = t.ProjectRoleId
                    });

                var result = dbContext.GetRepository<Project>()
                    .Query(x => data.ContainsKey(x.Id))
                    .Select(t => new ProjectMD()
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        Status = t.Status,
                        ProjectRoleId = data.Where(x => x.Key == t.Id).First().Value.ProjectRoleId,
                        DeletedBy = t.DeletedBy,
                        DeletedDate = t.DeletedDate
                    }).ToList();

                return result;
            });
        }

        #endregion

        #region Criteria

        private Dictionary<int, CriteriaTypeMD> CriteriaTypes_OnNeedResource(object sender, params object[] paramArrs)
        {
            return UserDbContext(dbContext =>
            {
                var result = dbContext.GetRepository<CriteriaType>()
                    .Query()
                    .ToDictionary(t => t.Id, t => new CriteriaTypeMD
                    {
                        Id = t.Id,
                        Description = t.Description,
                        Name = t.Name,
                        OrderNo = t.OrderNo,
                        DeletedBy = t.DeletedBy,
                        DeletedDate = t.DeletedDate
                    });
                return result;
            });
        }

        private List<CriteriaMD> _criterias_OnNeedValueIfKeyNotFound(object sender, params object[] paramArrs)
        {
            return UserDbContext(dbContext =>
            {
                var criteriaTypeId = (int)paramArrs.First();
                var result = dbContext.GetRepository<Criteria>()
                    .Query(t => t.CriteriaTypeId == criteriaTypeId && t.DeletedBy != null && t.DeletedDate != null)
                    .Select(t => new CriteriaMD
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        CriteriaTypeId = t.CriteriaTypeId,
                        OrderNo = t.OrderNo,
                        DeletedBy = t.DeletedBy,
                        DeletedDate = t.DeletedDate
                    }).ToList();
                return result;
            });
        }

        private Dictionary<int, CriteriaMD> Criterias_OnNeedResource(object sender, params object[] paramArrs)
        {
            return UserDbContext(dbContext =>
            {
                var result = dbContext.GetRepository<CriteriaMD>()
                    .Query()
                    .ToDictionary(t => t.Id, t => new CriteriaMD
                    {
                        Id = t.Id,
                        CriteriaTypeId = t.CriteriaTypeId,
                        Description = t.Description,
                        Name = t.Name,
                        OrderNo = t.OrderNo,
                        DeletedBy = t.DeletedBy,
                        DeletedDate = t.DeletedDate
                    });
                return result;
            });
        }

        #endregion

        #region Others
        private Dictionary<int, ProjectMD> Projects_OnNeedResource(object sender, params object[] paramArrs)
        {
            return UserDbContext(dbContext =>
            {
                var result = dbContext.GetRepository<Project>()
                    .Query()
                    .ToDictionary(t => t.Id, t => new ProjectMD
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description,
                        Status = t.Status,
                        DeletedBy = t.DeletedBy,
                        DeletedDate = t.DeletedDate
                    });
                return result;
            });
        }

        private Dictionary<int, PositionMD> Position_OnNeedResource(object sender, params object[] paramArrs)
        {
            return UserDbContext(dbContext =>
            {
                var result = dbContext.GetRepository<Position>()
                    .Query()
                    .ToDictionary(t => t.Id, t => new PositionMD
                    {
                        Id = t.Id,
                        Name = t.Name,
                        DeletedBy = t.DeletedBy,
                        DeletedDate = t.DeletedDate
                    });
                return result;
            });
        }
        #endregion

        #endregion

        #region Cache Extension

        public class UserAllCache : DataAllCache<int, UserMD> 
        {
            private readonly CacheProvider _provider;
            public UserAllCache(CacheProvider provider)
            {
                _provider = provider;
            }

            public List<ProjectMD> GetProjects(int userId)
            {
                return _provider._userProjects.Get(userId);
            }

            public ProjectMD GetProject(int userId)
            {
                var projects = _provider._userProjects.Get(userId);
                if (projects == null || projects.Count == 0)
                {
                    return null;
                }
                return projects.FirstOrDefault();
            }

            public override bool Remove(int userId)
            {
                base.Remove(userId);
                _provider._userProjects.Remove(userId);
                return true;
            }
            public override void Clear()
            {
                base.Clear();

                _provider._userProjects.Clear();
            }
        }

        public class ProjectAllCache : DataAllCache<int, ProjectMD>
        {
            
        }
        public class PositionAllCache : DataAllCache<int, PositionMD>
        {

        }

        public class CriteriaTypeAllCache : DataAllCache<int, CriteriaTypeMD>
        {
            private readonly CacheProvider _provider;
            public CriteriaTypeAllCache(CacheProvider provider)
            {
                _provider = provider;
            }

            public List<CriteriaMD> GetCriterias(int criteriaTypeId)
            {
                var criterias = _provider._criterias.Get(criteriaTypeId);
                return criterias;
            }

            public override bool Remove(int criteriaTypeId)
            {
                base.Remove(criteriaTypeId);
                _provider._criterias.Remove(criteriaTypeId);

                return true;
            }

            public override void Clear()
            {
                base.Clear();
                _provider._criterias.Clear();
            }
        }

        public class CriteriaAllCache : DataAllCache<int, CriteriaMD>
        {
        }

        #endregion
    }
}
