using Kloon.EmployeePerformance.Logic.Services;
using Kloon.EmployeePerformance.Models.Common;
using Kloon.EmployeePerformance.Models.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public ResultModel<List<UserModel>> GetAll(string searchText)
        {
            return _userService.GetAll(searchText);
        }
        [HttpGet("{id:int}")]
        public ResultModel<UserModel> GetById(int id)
        {
            return _userService.GetById(id);
        }
        [HttpPost]
        public ResultModel<UserModel> Create(UserModel userModel)
        {
            return _userService.Create(userModel);
        }
        [HttpPut]
        public ResultModel<UserModel> Update(UserModel userModel)
        {
            return _userService.Update(userModel);
        }
        [HttpDelete]
        public ActionResult<bool> Delete(int id)
        {
            return _userService.Delete(id).ToResponse();
        }
    }
}
