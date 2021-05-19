using Kloon.EmployeePerformance.DataAccess;
using Kloon.EmployeePerformance.DataAccess.Domain;
using Kloon.EmployeePerformance.Logic.Caches.Data;
using Kloon.EmployeePerformance.Logic.Common;
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
        private readonly IEntityRepository<ProjectUser> _projectUsers;
        public UserService(IAuthenLogicService<UserService> logicService)
        {
            _logicService = logicService;
            _dbContext = logicService.DbContext;

            _projectUsers = _dbContext.GetRepository<ProjectUser>();
            _users = _dbContext.GetRepository<User>();
        }

        public ResultModel<UserModel> Create(UserModel userModel)
        {
            var now = DateTime.UtcNow;
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
                    var registedEmail = _logicService.Cache.Users.GetValues().Any(x => x.Email.Equals(userModel.Email));
                    if (registedEmail)
                    {
                        return new ErrorModel(ErrorType.DUPLICATED, "INVALID_MODEL_DUPLICATED_EMAIL");
                    }
                    return null;
                })
               .ThenImplement(current =>
               {
                   var salt = Guid.NewGuid().ToString();

                   var user = new User()
                   {
                       Email = userModel.Email,
                       FirstName = userModel.FirstName.Trim(),
                       LastName = userModel.LastName.Trim(),
                       PositionId = userModel.PositionId,
                       Sex = (int)userModel.Sex,
                       DoB = userModel.DoB,
                       PhoneNo = userModel.PhoneNo,
                       RoleId = userModel.RoleId,
                       CreatedBy = current.Id,
                       CreatedDate = now,
                       PasswordHash = Utils.EncryptedPassword("123456", salt),
                       PasswordSalt = salt
                   };
                   _users.Add(user);
                   int result = _dbContext.Save();

                   userModel.Id = user.Id;
                   _logicService.Cache.Users.Clear();

                   return userModel;
               });
            return result;
        }

        public ResultModel<bool> Delete(int userId)
        {
            var now = DateTime.Now;
            User user = null;
            var result = _logicService
                .Start()
                .ThenAuthorize(Roles.ADMINISTRATOR)
                .ThenValidate(currentUser =>
                {
                    if (currentUser.Id == userId)
                    {
                        return new ErrorModel(ErrorType.CONFLICTED, "Cannot delete yourself");
                    }
                    user = _users
                        .Query(x => x.Id == userId && x.DeletedBy == null && x.DeletedDate == null)
                        .FirstOrDefault();
                    if (user == null)
                    {
                        return new ErrorModel(ErrorType.NOT_EXIST, "User not found");
                    }
                    return null;
                })
                .ThenImplement(currentUser =>
                {
                    List<ProjectUser> projectUsers = _projectUsers.Query(x => x.UserId == userId && x.DeletedBy == null && x.DeletedDate == null)
                        .ToList();

                    foreach (var item in projectUsers)
                    {
                        item.DeletedBy = currentUser.Id;
                        item.DeletedDate = now;
                    }

                    user.DeletedBy = currentUser.Id;
                    user.DeletedDate = now;

                    int result = _dbContext.Save();

                    _logicService.Cache.Users.Clear();
                    return true;
                });
            return result;
        }

        public ResultModel<List<UserModel>> GetAll(string searchText)
        {
            var result = _logicService
                .Start()
                .ThenAuthorize(Roles.ADMINISTRATOR, Roles.USER)
                .ThenValidate(current => null)
                .ThenImplement(current =>
                {

                    var query = _logicService.Cache.Users.GetValues().AsEnumerable();
                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        query = query.Where(t => t.LastName.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                                                || t.FirstName.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                                                || t.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase));
                    }

                    var record = query.OrderBy(x => x.FirstName)
                                      .Select(t => new UserModel
                                      {
                                          Id = t.Id,
                                          Email = t.Email,
                                          FirstName = t.FirstName,
                                          LastName = t.LastName,
                                          PositionId = t.PositionId,
                                          Sex = (SexEnum)t.Sex,
                                          DoB = t.DoB,
                                          PhoneNo = t.PhoneNo,
                                          RoleId = t.RoleId
                                      }).ToList();
                    //record = current.Role == Roles.USER ? record.Where(t => t.RoleId != (int)Roles.ADMINISTRATOR).ToList() : record;

                    return record;

                });
            return result;
        }

        public ResultModel<UserModel> GetById(int userId)
        {
            UserMD userMD = null;
            var result = _logicService
                        .Start()
                        .ThenAuthorize(Roles.ADMINISTRATOR, Roles.USER)
                        .ThenValidate(current =>
                        {
                            userMD = _logicService.Cache.Users.Get(userId);
                            if (userMD == null)
                            {
                                return new ErrorModel(ErrorType.NOT_EXIST, "User not found");
                            }
                            if (userMD.DeletedBy == null && userMD.DeletedDate == null)
                            {
                                return new ErrorModel(ErrorType.NOT_EXIST, "User not found");
                            }

                            return null;
                        })
                        .ThenImplement(current =>
                        {
                            var user = new UserModel()
                            {
                                Id = userMD.Id,
                                Email = userMD.Email,
                                FirstName = userMD.FirstName,
                                LastName = userMD.LastName,
                                PositionId = userMD.PositionId,
                                Sex = (SexEnum)userMD.Sex,
                                DoB = userMD.DoB,
                                PhoneNo = userMD.PhoneNo,
                                RoleId = userMD.RoleId


                            };
                            return user;
                        });
            return result;
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
                    var registedEmail = _logicService.Cache.Users.GetValues().Any(x => x.Id != userModel.Id && x.Email.Equals(userModel.Email));
                    if (registedEmail)
                    {
                        return new ErrorModel(ErrorType.DUPLICATED, "INVALID_MODEL_DUPLICATED_EMAIL");
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
                    user.Sex = (int)userModel.Sex;
                    user.DoB = userModel.DoB;
                    user.PhoneNo = userModel.PhoneNo;
                    user.RoleId = userModel.RoleId;
                    user.ModifiedBy = current.Id;
                    user.ModifiedDate = now;
                    int result = _dbContext.Save();
                    _logicService.Cache.Users.Clear();
                    return userModel;
                });
            return result;
        }
        private ErrorModel ValidateUser(UserModel userModel)
        {
            #region UserModel
            if (userModel == null)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL");
            }
            #endregion

            #region FirstName
            if (string.IsNullOrEmpty(userModel.FirstName))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_FIRST_NAME_NULL");
            }
            if (userModel.FirstName.Length < 1)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_FIRST_NAME_MIN_LENGTH");
            }
            if (userModel.FirstName.Length > 20)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_FIRST_NAME_MAX_LENGTH");
            }
            if (Regex.IsMatch(userModel.FirstName, @"^[a-zA-Z]*$") == false)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_FIRST_NAME_CHARACTERS");
            }
            #endregion 

            #region LastName
            if (string.IsNullOrEmpty(userModel.LastName))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_LAST_NAME_NULL");
            }
            if (userModel.LastName.Length < 1)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_LAST_NAME_MIN_LENGTH");
            }
            if (userModel.LastName.Length > 20)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_LAST_NAME_MAX_LENGTH");
            }
            if (Regex.IsMatch(userModel.LastName, @"^[a-zA-Z]*$") == false)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_LAST_NAME_CHARACTERS");
            }
            #endregion

            #region Email
            if (string.IsNullOrEmpty(userModel.Email))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_EMAIL_NULL");
            }
            if (userModel.Email.Length > 40)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_EMAIL_MAX_LENGTH");
            }

            string validEmail = "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@kloon.vn$";
            if (Regex.IsMatch(userModel.Email, validEmail, RegexOptions.IgnoreCase) == false)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_EMAIL_FORMAT_WRONG");
            }

            #endregion

            #region Phone
            if (userModel.PhoneNo != null && userModel.PhoneNo.Length > 10)
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_PHONE_MAX_LENGTH");
            }
            #endregion

            #region Selectable Attribute
            if (string.IsNullOrEmpty(userModel.PositionId.ToString()))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_POSITION_NULL");
            }
            if (string.IsNullOrEmpty(userModel.Sex.ToString()))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_SEX_NULL");
            }
            if (string.IsNullOrEmpty(userModel.RoleId.ToString()))
            {
                return new ErrorModel(ErrorType.BAD_REQUEST, "INVALID_MODEL_ROLE_NULL");
            }
            #endregion

            return null;
        }
    }
}
