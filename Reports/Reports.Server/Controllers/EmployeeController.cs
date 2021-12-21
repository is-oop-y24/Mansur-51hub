using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reports.DAL.Entities;
using Reports.Server.Services;

namespace Reports.Server.Controllers
{
    [ApiController]
    [Route("/employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("/employees/{name}/{supervisorId:Guid}")]
        public async Task<Employee> Create(string name, Role role, Guid supervisorId)
        {
            return await _service.Create(name, role, supervisorId);
        }

        [HttpGet]
        [Route("/employess/all")]
        public IActionResult GetEmployees()
        {
            try
            {
                return Ok(_service.GetEmployees());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        [HttpGet]
        public IActionResult Find([FromQuery] string name, [FromQuery] Guid id)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Employee result = _service.FindByName(name);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            if (id != Guid.Empty)
            {
                Employee result = _service.FindById(id);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int) HttpStatusCode.BadRequest);
        }

        [HttpGet]
        [Route("/employees/subordinates/{id:Guid}")]
        public IActionResult FindSubordinates(Guid id)
        {
            if (id != Guid.Empty)
            {
                List<Employee> result = _service.FindSubordinates(id);
                if (result != null)
                {
                    return Ok(result);
                }

                return NotFound();
            }

            return StatusCode((int)HttpStatusCode.BadRequest);
        }
       
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(Guid id)
        {
            try
            {
                Employee employeeToDelete = _service.FindById(id);

                if (employeeToDelete == null)
                {
                    return NotFound($"Employee with Id = {id} not found");
                }

                return await _service.Delete(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<Employee>> Update(Guid id, Employee employee)
        {
            try
            {
                if (id != employee.Id)
                {
                    return BadRequest("Employee ID mismatch");
                }

                Employee employeeToUpdate = _service.FindById(id);

                if (employeeToUpdate == null)
                {
                    return NotFound($"Employee with Id = {id} not found");
                }

                return await _service.Update(employee);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }
    }
}