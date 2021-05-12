using Kloon.EmployeePerformance.Logic.Services;
using Kloon.EmployeePerformance.Models.Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public ProjectViewModel GetAll([FromQuery] string searchText)
        {
            var result = _projectService.GetAll(searchText);
            return result.ToResponse();
        }
    }
}
