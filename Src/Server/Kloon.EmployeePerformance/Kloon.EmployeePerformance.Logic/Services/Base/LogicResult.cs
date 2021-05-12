using Kloon.EmployeePerformance.DataAccess.Domain;
using Kloon.EmployeePerformance.Models.Common;
using Kloon.EmployeePerformance.Models.User;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.Logic.Services.Base
{
    public class LogicResult
    {
        public readonly LoginUserModel.LoginUser CurrentUser;
        public readonly ICommonService Services;

        public bool IsValid { get; private set; } = true;

        private ErrorModel _error;
        public ErrorModel Error
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
                IsValid = _error == null;
            }
        }

        public LogicResult(LoginUserModel.LoginUser currentUser, ICommonService commonService)
        {
            CurrentUser = currentUser;
            Services = commonService;
        }

        public ValidationAuthenResult ThenAuthorize(params Roles[] roles)
        {
            if (roles != null && roles.Length != 0)
            {
                if (CurrentUser == null)
                {
                    Error = new ErrorModel(ErrorType.NOT_AUTHENTICATED, "Not Authorized");
                }
                else
                {
                    var user = Services.DbContext.GetRepository<User>().Query(x => x.Id == CurrentUser.Id).FirstOrDefault();
                    if (user.DeletedBy != null && user.DeletedDate != null)
                    {
                        Error = new ErrorModel(ErrorType.NOT_EXIST, "Role not found");
                    }
                    if (!roles.Contains(CurrentUser.Role))
                    {
                        Error = new ErrorModel(ErrorType.NO_ROLE, "No Role");
                    }
                }
            }
            return new ValidationAuthenResult(this);
        }

    }

    public class ValidationResult
    {
        protected readonly LogicResult _result;
        public ValidationResult(LogicResult logicResult)
        {
            _result = logicResult;
        }

        public virtual ImplementResult ThenValidate(Func<ErrorModel> func)
        {
            if (_result.IsValid)
            {
                try
                {
                    _result.Error = func.Invoke();
                }
                catch (Exception ex)
                {
                    _result.Services.Logger.LogError("VALIDATEDATA: " + ex.ToString());
                    _result.Error = new ErrorModel(ErrorType.INTERNAL_ERROR, ex.Message);
                }
            }
            return new ImplementResult(_result);
        }
    }

    public class ImplementResult
    {
        protected readonly LogicResult _result;
        public ImplementResult(LogicResult result)
        {
            _result = result;
        }
        public ResultModel<T> ThenImplement<T>(Func<T> func)
        {
            if (_result.IsValid)
            {
                try
                {
                    var data = func.Invoke();
                    return new ResultModel<T>(data);
                }
                catch (Exception ex)
                {
                    _result.Services.Logger.LogError("IMPLEMENT: " + ex.ToString());
                    _result.Error = new ErrorModel(ErrorType.INTERNAL_ERROR, ex.Message);
                }
            }
            return new ResultModel<T>(_result.Error);
        }
    }
}
