using Kloon.EmployeePerformance.DataAccess.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.DataAccess
{
    public class EmployeePerformanceDataInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<IUnitOfWork<EmployeePerformanceContext>>();
                InitializeRethinkBaseData(context);

            }
        }

        private static void InitializeRethinkBaseData(IUnitOfWork<EmployeePerformanceContext> context)
        {
            var createDate = DateTime.UtcNow;

            var positions = context.GetRepository<Position>();
            if (!positions.Query().Any())
            {
                var data = new List<Position>
                {
                    new Position
                    {
                        Name = "Admin",
                        CreatedBy = 1,
                        CreatedDate = createDate
                    },
                    new Position
                    {
                        Name = "Developer",
                        CreatedBy = 1,
                        CreatedDate = createDate
                    },
                    new Position
                    {
                        Name = "Tester",
                        CreatedBy = 1,
                        CreatedDate = createDate
                    },
                    new Position
                    {
                        Name = "QA",
                        CreatedBy = 1,
                        CreatedDate = createDate
                    },
                    new Position
                    {
                        Name = "Language Assistant",
                        CreatedBy = 1,
                        CreatedDate = createDate
                    },
                    new Position
                    {
                        Name = "Intern",
                        CreatedBy = 1,
                        CreatedDate = createDate
                    }
                };
                positions.InsertRange(data);
            }

            var users = context.GetRepository<User>();
            if (!users.Query().Any())
            {
                var data = new List<User>
                {
                    new User
                    {
                        Email = "admin@kloon.com",
                        FirstName ="Admin",
                        LastName = "Admin",
                        DoB = new DateTime(1980,1,1),
                        PhoneNo = "123456789",
                        RoleId = 1,
                        PositionId = 1,
                        Sex = true,
                        PasswordHash = "26EFBFBDEFBFBDEFBFBDEFBFBD5FEFBFBD76EFBFBDEFBFBD4774365754EFBFBDEFBFBDEFBFBD5EEFBFBD5B05EFBFBD620373384313EFBFBD22EFBFBD", //123456
                        PasswordSalt ="84b32f39-a6d5-4d5a-908c-538fea22b3d9",
                        CreatedDate = createDate,
                        CreatedBy = 1,
                    },
                    new User
                    {
                        Email = "user@kloon.com",
                        FirstName ="Admin",
                        LastName = "Admin",
                        DoB = new DateTime(1980,1,1),
                        PhoneNo = "123456789",
                        RoleId = 2,
                        PositionId = 1,
                        Sex = true,
                        PasswordHash = "26EFBFBDEFBFBDEFBFBDEFBFBD5FEFBFBD76EFBFBDEFBFBD4774365754EFBFBDEFBFBDEFBFBD5EEFBFBD5B05EFBFBD620373384313EFBFBD22EFBFBD", //123456
                        PasswordSalt ="84b32f39-a6d5-4d5a-908c-538fea22b3d9",
                        CreatedDate = createDate,
                        CreatedBy = 1,
                    }
                };
                users.InsertRange(data);
            }

        }
    }
}
