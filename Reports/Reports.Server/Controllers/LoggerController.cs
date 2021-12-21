using Microsoft.AspNetCore.Mvc;
using Reports.Server.Services;
using System;
using System.Collections.Generic;
using System.Net;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/logs")]
    public class LoggerController : ControllerBase
    {
        private readonly ITaskLoggerService _service;

        public LoggerController(ITaskLoggerService service)
        {
            _service = service;
        }

        [Route("/logs/{id:Guid}")]
        [HttpGet]
        public IActionResult FindTaskLogs(Guid id)
        {
            if (id != Guid.Empty)
            {
                List<string> result = _service.GetTaskLogs(id);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int)HttpStatusCode.BadRequest);
        }
    }
}
