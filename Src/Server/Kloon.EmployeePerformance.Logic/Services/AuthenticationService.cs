using Kloon.EmployeePerformance.DataAccess;
using Kloon.EmployeePerformance.DataAccess.Domain;
using Kloon.EmployeePerformance.Logic.Caches;
using Kloon.EmployeePerformance.Logic.Common;
using Kloon.EmployeePerformance.Logic.Services.Base;
using Kloon.EmployeePerformance.Models.Common;
using Kloon.EmployeePerformance.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.Logic.Services
{
    public interface IAuthenticationService
    {
        ResultModel<UserAuthModel> Login(string email, string password);
    }
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork<EmployeePerformanceContext> _dbContext;
        private readonly ILogicService<AuthenticationService> _logicService;
        private readonly IEntityRepository<User> _users;


        public AuthenticationService(IIdentityService identityService, IServiceProvider serviceProvider,
              IUnitOfWork<EmployeePerformanceContext> dbContext,CacheProvider cacheProvider, ILogicService<AuthenticationService> logicService)
        {
            _identityService = identityService;
            _dbContext = dbContext;
            _users = _dbContext.GetRepository<User>();
        }

        public ResultModel<UserAuthModel> Login(string email, string password)
        {
            UserAuthModel user = null;

            var result = _logicService
                .Start()
                .ThenValidate(() =>
                {
                    //TODO: CHECK VALID EMAIL

                    var data = _users
                        .Query(t => t.Email.Equals(email))
                        .Select(t => new {
                            User = new UserAuthModel
                            {
                                Id = t.Id,
                                Email = t.Email,
                                FirstName = t.FirstName,
                                LastName = t.LastName,
                                PositionId = t.PositionId,
                                PasswordHash = t.PasswordHash,
                                PasswordSalt = t.PasswordSalt,
                                Role = (Roles)t.RoleId
                            }
                        })
                        .FirstOrDefault();

                    if (data == null)
                    {
                        return new ErrorModel(ErrorType.NOT_EXIST, "CANT NOT IDENTITY USER");
                    }

                    var encryptedPassword = Utils.EncryptedPassword(password, data.User.PasswordSalt);
                    if (data.User.PasswordHash != encryptedPassword)
                    {
                        return new ErrorModel(ErrorType.NOT_EXIST, "WRONG PASSWORD");
                    }

                    user = data.User;
                    return null;
                })
                .ThenImplement(() =>
                {
                    return user;
                });

            return result;
        }

    }
}
