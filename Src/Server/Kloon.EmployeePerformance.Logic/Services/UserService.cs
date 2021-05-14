using Kloon.EmployeePerformance.DataAccess;
using Kloon.EmployeePerformance.DataAccess.Domain;
using Kloon.EmployeePerformance.Logic.Services.Base;
using Kloon.EmployeePerformance.Models.Common;
using Kloon.EmployeePerformance.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Kloon.EmployeePerformance.Logic.Services
{
    public interface IUserService
    {
        ResultModel<List<UserModel>> GetAll(string searchText = "");
        ResultModel<UserModel> GetById(int userId);
        ResultModel<UserModel> Create(UserModel userModel);
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

        public ResultModel<UserModel> Create(UserModel userModel)
        {
            var result = _logicService
                .Start()
                .ThenAuthorize(Roles.ADMINISTRATOR)
                .ThenValidate(current =>
                {
                    var error = ValidateUser(userModel);
                    if (error != null)
                    {
                        return error;
                    };

                    return null;
                })
               .ThenImplement(current =>
               {
                   var user = new User()
                   {
                       Email = userModel.Email,
                       FirstName = userModel.FirstName.Trim(),
                       LastName = userModel.LastName.Trim(),
                       PositionId = userModel.PositionId,
                       Sex = userModel.Sex,
                       DoB = userModel.DoB,
                       PhoneNo = userModel.PhoneNo,
                       RoleId = userModel.RoleId,
                       CreatedBy = current.Id,
                   };
                   _users.Add(user);
                   int result = _dbContext.Save();
                   return result;
               });
            return null;
        }

        public ResultModel<bool> Delete(int userId)
        {
            throw new NotImplementedException();
        }

        public ResultModel<List<UserModel>> GetAll(string searchText)
        {
            throw new NotImplementedException();
        }

        public ResultModel<UserModel> GetById(int userId)
        {
            throw new NotImplementedException();
        }

        public ResultModel<UserModel> Update(UserModel userModel)
        {
            var now = DateTime.UtcNow;
            User user = null;
            var result = _logicService
                .Start()
                .ThenAuthorize(Roles.ADMINISTRATOR)
                .ThenValidate(current =>
                {
                    var error = ValidateUser(userModel);
                    if (error != null)
                    {
                        return error;
                    }

                    user = _users.Query(x => x.Id == userModel.Id).FirstOrDefault();
                    if (user == null)
                    {
                        return new ErrorModel(ErrorType.NOT_EXIST, "User with Id = " + userModel.Id + " not found");
                    }
                    if (user.DeletedBy != null && user.DeletedDate != null)
                    {
                        return new ErrorModel(ErrorType.NOT_EXIST, "User with Id = " + userModel.Id + " has been deleted");
                    }
                    return null;
                })
                .ThenImplement(current =>
                {
                    user.Email = userModel.Email;
                    user.FirstName = userModel.FirstName.Trim();
                    user.LastName = userModel.LastName.Trim();
                    user.PositionId = userModel.PositionId;
                    user.Sex = userModel.Sex;
                    user.DoB = userModel.DoB;
                    user.PhoneNo = userModel.PhoneNo;
                    user.RoleId = userModel.RoleId;
                    user.ModifiedBy = current.Id;
                    user.ModifiedDate = now;
                    int result = _dbContext.Save();
                    return userModel;
                });
            return result;
        }
        private ErrorModel ValidateUser(UserModel userModel)
        {
            #region UserModel
            if (userModel == null)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Please fill in the required fields");
            }
            #endregion

            #region FirstName
            if (string.IsNullOrEmpty(userModel.FirstName))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "First name is required");
            }
            if (userModel.FirstName.Length < 1)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Min length of first name is 2");
            }
            if (userModel.FirstName.Length > 20)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Max length of first name is 20");
            }
            if (Regex.IsMatch(userModel.FirstName, @"^\W$"))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "First name cannot contain digits or special characters");
            }
            #endregion 

            #region LastName
            if (string.IsNullOrEmpty(userModel.LastName))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Last name is required");
            }
            if (userModel.LastName.Length < 1)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Min length of Last name is 2");
            }
            if (userModel.LastName.Length > 20)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Max length of Last name is 20");
            }
            if (Regex.IsMatch(userModel.LastName, @"^\W$"))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Last name cannot contain digits or special characters");
            }
            #endregion

            #region Email
            if (string.IsNullOrEmpty(userModel.Email))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Email is required");
            }
            if (userModel.Email.Length > 20)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Max length of email is 40");
            }
            if (Regex.IsMatch(userModel.Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Email must have a correct format");
            }

            var registedEmail = _logicService.Cache.Users.GetValues().Any(x => x.Email.Equals(userModel.Email));
            if (registedEmail)
            {
                return new ErrorModel(ErrorType.DUPLICATED, "Email has been taken!");
            }

            #endregion

            #region Phone
            if (string.IsNullOrEmpty(userModel.PhoneNo))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Phone number is required");
            }
            if (userModel.PhoneNo.Length > 11)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Max length of phone number is 10");
            }
            if (Regex.IsMatch(userModel.PhoneNo, @"^(84|0[3|5|7|8|9])+([0-9]{8})\b$"))
            {
                return new ErrorModel(ErrorType.DUPLICATED, "Phone must follow Vietnam format!");
            }
            var takenPhone = _logicService.Cache.Users.GetValues().Any(x => x.PhoneNo.Equals(userModel.PhoneNo));
            if (takenPhone)
            {
                return new ErrorModel(ErrorType.DUPLICATED, "Phone has been taken!");
            }
            #endregion

            #region Selectable Attribute
            if (string.IsNullOrEmpty(userModel.PositionId.ToString()))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Position is required");
            }
            if (string.IsNullOrEmpty(userModel.Sex.ToString()))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Gender is required");
            }
            if (string.IsNullOrEmpty(userModel.DoB.ToString()))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Date of  birth is required");
            }
            if (string.IsNullOrEmpty(userModel.RoleId.ToString()))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "Role is required");
            }
            #endregion

            return null;
        }
    }
}
