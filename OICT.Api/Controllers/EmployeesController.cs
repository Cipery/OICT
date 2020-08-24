using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OICT.Application.Commands;
using OICT.Application.Commands.CreateEmployee;
using OICT.Application.Commands.DeleteEmployee;
using OICT.Application.Commands.UpdateEmployee;
using OICT.Application.Dtos;
using OICT.Application.Queries;
using OICT.Application.Queries.EmployeeExists;
using OICT.Application.Queries.GetEmployee;
using OICT.Application.Queries.ListEmployees;
using OICT.Application.Queries.ListEmployeesOlderThan;
using OICT.Domain.Model;
using OICT.Infrastructure;

namespace OICT.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeModel>>> GetEmployee()
        {
            var employees = await _mediator.Send(new ListEmployeeQuery());
            return new ActionResult<IEnumerable<EmployeeModel>>(employees);
        }

        // GET: api/Employees/5
        /// <summary>
        /// Gets an employee with supplied ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A requested employee</returns>
        /// <response code="201">Gets an employee with supplied ID.</response>
        /// <response code="404">If the employee is not found</response>       
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeModel>> GetEmployee(int id)
        {
            var employee = await _mediator.Send(new GetEmployeeQuery(id));

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, UpdateEmployeeModel employeeEntity)
        {
            if (id != employeeEntity.Id)
            {
                return BadRequest();
            }

            try
            {
                await _mediator.Send(new UpdateEmployeeCommand(employeeEntity));
            }
            catch (DBConcurrencyException)
            {
                var exists = await _mediator.Send(new GetEmployeeExistsQuery(id));
                if (!exists)
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<EmployeeEntity>> PostEmployee(CreateEmployeeModel createEmployeeModel)
        {
            var result = await _mediator.Send(new CreateEmployeeCommand(createEmployeeModel));
            return CreatedAtAction("GetEmployee", new {id = result});
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeModel>> DeleteEmployee(int id)
        {
            var employee = await _mediator.Send(new DeleteEmployeeCommand(id));

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // GET: api/Employees/listOlderThan/18
        [HttpGet("listOlderThan/{age}")]
        public async Task<IEnumerable<EmployeeModel>> ListOlderThan(int age) =>
            await _mediator.Send(new ListEmployeesOlderThanQuery(age));
    }
}
