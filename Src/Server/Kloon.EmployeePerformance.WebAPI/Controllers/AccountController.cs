using Kloon.EmployeePerformance.Logic.Caches;
using Kloon.EmployeePerformance.Logic.Services;
using Kloon.EmployeePerformance.Models.Authentication;
using Kloon.EmployeePerformance.Models.Common;
using Kloon.EmployeePerformance.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;
        private readonly CacheProvider _cache;

        public AccountController(IConfiguration configuration, IAuthenticationService authenticationService,CacheProvider cacheProvider)
        {
            _authenticationService = authenticationService;
            _configuration = configuration;
            _cache = cacheProvider;
        }


        /// <summary>
        /// Login with an account
        /// </summary>
        /// <remarks>
        /// Return the status whether login is successed of failed
        /// </remarks>
        /// <param name="login">Information of the account</param>
        /// <returns>Return the status whether login is successed of failed</returns>
        /// <response code="200">Successful operation</response>
        /// <response code="204">Successful operation, no content is returned</response>
        /// <response code="400">Invalid Id supplied</response>
        /// <response code="401">User is unauthorized</response>
        /// <response code="403">User is not authenticated</response>
        /// <response code="404">Request is inaccessible</response>
        /// <response code="409">Data is conflicted</response>
        /// <response code="500">Server side error</response>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (!String.IsNullOrEmpty(login.Email))
            {
                login.Email = login.Email.Trim();
            }

            var result = _authenticationService.Login(login.Email, login.Password);
            if (result.Error != null)
            {
                throw new LogicException(new ErrorModel(ErrorType.BAD_REQUEST, result.Error.Message));
            }

            var user = result;

            var roles = user.Data.Role;
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Sid, user.Data.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, login.Email));
            claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtExpiryInMinutes"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );

            var projectRole = _cache.Users.GetProjects(user.Data.Id);

            var results = new LoginResult
            {
                IsSuccessful = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                FirstName = user.Data.FirstName,
                LastName = user.Data.LastName,
                AppRole = user.Data.Role,
                Email = user.Data.Email,
                ProjectRoles = projectRole?.Select(t => new ProjectRoleModel
                { 
                    ProjectId = t.Id,
                    ProjectRoleId = (ProjectRoles)t.ProjectRoleId
                }).ToList()
            };
            return Ok(results);
        }
    }
}
