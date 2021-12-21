using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reports.DAL.Entities;
using Reports.Server.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }

        [HttpPost("{responsibleEmployeeId:Guid}/{description}")]
        public async Task<ActionResult<Problem>> Create(Guid responsibleEmployeeId, string description)
        { 
            Problem result = await _service.Create(responsibleEmployeeId, description);
            if(result == null)
            {
                return NotFound($"Employee with id {responsibleEmployeeId} not found");
            }

            return result;
        }

        [HttpGet]
        public IActionResult Find([FromQuery] Guid id, [FromQuery] DateTime time)
        {
            if (id != Guid.Empty)
            {
                Problem result = _service.FindById(id);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int)HttpStatusCode.BadRequest);
        }

        [Route("/tasks/employee")]
        [HttpGet]
        public IActionResult FindEmployeeTasks([FromQuery] Guid employeeId)
        {
            if (employeeId != Guid.Empty)
            {
                List<Problem> result = _service.FindEmployeeTasks(employeeId);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int)HttpStatusCode.BadRequest);
        }

        [Route("/tasks/employee/{id:Guid}/subordinates")]
        [HttpGet]
        public IActionResult FindSubordinatesTasks(Guid id)
        {
            if (id != Guid.Empty)
            {
                List<Problem> result = _service.FindSubordinatesTaks(id);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int)HttpStatusCode.BadRequest);
        }

        [Route("/tasks/all")]
        [HttpGet]
        public IActionResult FindAllTasks()
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

        [Route("description/{id:Guid}")]
        [HttpGet]
        public IActionResult FindTaskDescription(Guid id)
        {
            if (id != Guid.Empty)
            {
                string result = _service.FindDescription(id);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int)HttpStatusCode.BadRequest);
        }

        [Route("description/{id:Guid}/{newDescription}")]
        [HttpPut]
        public async Task<ActionResult<Problem>> UpdateDescription(Guid id, string newDescription)
        {
            Problem taskToUpdate = _service.FindById(id);

            if (taskToUpdate == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            return await _service.UpdateDescription(id, newDescription);
        }

        [Route("status/{id:Guid}")]
        [HttpPut]
        public async Task<ActionResult<Problem>> UpdateStatus(Guid id, Status newStatus)
        {
            Problem taskToUpdate = _service.FindById(id);

            if (taskToUpdate == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            return await _service.UpdateStatus(id, newStatus);
        }

        [Route("responsibleEmployee/{id:Guid}/{employeeId:Guid}")]
        [HttpPut]
        public async Task<ActionResult<Problem>> UpdateEmployee(Guid id, Guid employeeId)
        {
            Problem taskToUpdate = _service.FindById(id);

            if (taskToUpdate == null)
            {
                return NotFound($"Task with Id = {id} not found");
            }

            return await _service.UpdateResponsibleEmployee(id, employeeId);
        }
    }
}
