using Kloon.EmployeePerformance.Logic.Services.Base;
using Kloon.EmployeePerformance.Models.Common;
using Kloon.EmployeePerformance.Models.User;
using Kloon.EmployeePerformance.Logic.Services.Base;
using Kloon.EmployeePerformance.DataAccess.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kloon.EmployeePerformance.DataAccess;

namespace Kloon.EmployeePerformance.Logic.Services
{
    public interface IUserService
    {
        ResultModel<List<UserModel>> GetAll(string searchText = "");
        ResultModel<List<UserModel>> GetById(int userId);
        ResultModel<UserModel> Create(UserModel userModel, string password);
        ResultModel<UserModel> Update(UserModel userModel);
        ResultModel<bool> Delete(int userId);
    }
    public class UserService : IUserService
    {
        private readonly IAuthenLogicService<UserService> _logicService;
        private readonly IUnitOfWork<EmployeePerformanceContext> _dbContext;

        private readonly IEntityRepository<User> _users;
        public UserService(IAuthenLogicService<UserService> logicService)
        {
            _logicService = logicService;
            _dbContext = logicService.DbContext;

            _users = _dbContext.GetRepository<User>();
        }

        public ResultModel<UserModel> Create(UserModel userModel, string password)
        {
            throw new NotImplementedException();
        }

        public ResultModel<bool> Delete(int userId)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<UserModel>> GetAll(string searchText)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<UserModel>> GetById(int userId)
        {
            throw new NotImplementedException();
        }

        public ResultModel<UserModel> Update(UserModel userModel)
        {
            throw new NotImplementedException();
        }
    }
}
