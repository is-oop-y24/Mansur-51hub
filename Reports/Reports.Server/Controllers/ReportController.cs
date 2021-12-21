using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reports.DAL.Entities;
using Reports.Server.Services;
using System;
using System.Threading.Tasks;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/reports")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<WeeklyReport> Create()
        {
            return await _service.CreateWeeklyReport();
        }

        [Route("/reports/tasks")]
        [HttpGet]
        public IActionResult GetReports()
        {
            try
            {
                return Ok(_service.GetTasks());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [Route("/reports/{id:Guid}")]
        [HttpPut]
        public async Task<ActionResult<WeeklyReport>> AddReport(Guid id, Report report)
        {
            return await _service.AddReport(id, report);
        }

        [Route("/reports/description/{id:Guid}/{newDescription}")]
        [HttpPut]
        public async Task<ActionResult<WeeklyReport>> UpdateDescription(Guid id, string newDescription)
        {
            return await _service.UpdateDescription(id, newDescription);
        }
    }
}
