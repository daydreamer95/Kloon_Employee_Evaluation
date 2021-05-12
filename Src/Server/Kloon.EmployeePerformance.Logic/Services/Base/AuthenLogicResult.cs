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
    public class ValidationAuthenResult : ValidationResult
    {
        public ValidationAuthenResult(LogicResult result) 
            : base(result)
        {

        }
 
        public ValidationResult ThenAuthorizeUser(int? userId)
        {
            if (_result.IsValid)
            {
                // TODO: Chuyển về cache
                var user = _result.Services.DbContext.GetRepository<User>().Query(x => x.Id == userId.Value && x.DeletedBy == null && x.DeletedDate == null).FirstOrDefault();

                if (user == null)
                {
                    _result.Error = new ErrorModel(ErrorType.NOT_EXIST, "User not found");
                }

                if (_result.CurrentUser.Role == Roles.USER && _result.CurrentUser.Id != userId)
                {
                    _result.Error = new ErrorModel(ErrorType.NO_DATA_ROLE, "Data does not exits");
                }
            }
            return this;
        }

        public ImplementAuthenResult ThenValidate(Func<LoginUserModel.LoginUser, ErrorModel> func)
        {
            if (_result.IsValid)
            {
                try
                {
                    if (func != null)
                    {
                        _result.Error = func.Invoke(_result.CurrentUser);
                    }
                }
                catch (Exception ex)
                {
                    _result.Services.Logger.LogError("VALIDATEDATA: " + ex.ToString());
                    _result.Error = new ErrorModel(ErrorType.INTERNAL_ERROR, ex.Message);
                }
            }
            return new ImplementAuthenResult(_result);
        }

        public override ImplementAuthenResult ThenValidate(Func<ErrorModel> func)
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
            return new ImplementAuthenResult(_result);
        }

    }
    public class ImplementAuthenResult : ImplementResult
    {
        public ImplementAuthenResult(LogicResult result)
            : base(result)
        {

        }

        public ResultModel<T> ThenImplement<T>(Func<LoginUserModel.LoginUser, T> func)
        {
            if (_result.IsValid)
            {
                try
                {
                    var data = func.Invoke(_result.CurrentUser);
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
