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
    public class CriteriaController: ControllerBase
    {
        private readonly ICriteriaService _criteriaService;
        public CriteriaController(ICriteriaService criteriaService)
        {
            _criteriaService = criteriaService;
        }

        [HttpGet]
        public ActionResult<List<CriteriaModel>> GetAll()
        {
            return _criteriaService.GetAll();
        }

        [HttpPost]
        public ActionResult<CriteriaModel> Add(CriteriaModel model)
        {
            return _criteriaService.Add(model);
        }

        [HttpPut]
        public ActionResult<CriteriaModel> Edit(CriteriaModel model)
        {
            return _criteriaService.Edit(model);
        }

        [HttpDelete]
        public ActionResult<bool> Delete(Guid Id)
        {
            return _criteriaService.Delete(Id);
        }
    }
}
