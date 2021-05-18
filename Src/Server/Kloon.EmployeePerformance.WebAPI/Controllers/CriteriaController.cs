using Kloon.EmployeePerformance.Logic.Services;
using Kloon.EmployeePerformance.Models.Criteria;
using Kloon.EmployeePerformance.Models.Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kloon.EmployeePerformance.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CriteriaController : ControllerBase
    {
        private readonly ICriteriaService _criteriaService;
        public CriteriaController(ICriteriaService criteriaService, IHttpContextAccessor httpContextAccessor)
        {
            _criteriaService = criteriaService;
        }

        [HttpGet]
        public ActionResult<List<CriteriaModel>> GetAll(string key)
        {
            var a = HttpContext.Request;
            return _criteriaService.GetAll(key).ToResponse();
        }

        [HttpGet("{id}")]
        public ActionResult<CriteriaModel> Get(Guid Id)
        {
            var a = HttpContext.Request;
            return _criteriaService.Get(Id).ToResponse();
        }

        [HttpPost]
        public ActionResult<CriteriaModel> Add(CriteriaModel model)
        {
            return _criteriaService.Add(model).ToResponse();
        }

        [HttpPut]
        public ActionResult<CriteriaModel> Edit(CriteriaModel model)
        {
            return _criteriaService.Edit(model).ToResponse();
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(Guid Id)
        {
            return _criteriaService.Delete(Id).ToResponse();
        }

        [HttpPost("Order")]
        public ActionResult<bool> ReOrder(List<CriteriaModel> models) 
        {
            _criteriaService.ReOrder(models).ToResponse();
            return true;
        }
    }
}
