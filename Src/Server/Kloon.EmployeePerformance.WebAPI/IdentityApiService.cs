using Kloon.EmployeePerformance.Logic.Services.Base;
using Kloon.EmployeePerformance.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.WebAPI
{
    public class IdentityApiService : IIdentityService
    {
        public LoginUserModel GetCurrentUser()
        {
            return new LoginUserModel()
            {
                User = new LoginUserModel.LoginUser()
                {
                    Id = 1,
                    PositionId = 1,
                    Email = "Admin@admin.com",
                    FirstName = "ABCD",
                    LastName = "ÁDmin",
                    Role = Models.Common.Roles.ADMINISTRATOR
                }
            };
        }
    }
}
