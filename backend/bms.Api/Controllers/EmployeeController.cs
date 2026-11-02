using bms.Api.DTOs;
using bms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace bms.Api.Controllers;
[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly EmployeeService _employeeService;
    public EmployeesController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeDto>>> GetAllEmployees()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);

        if (employee == null)
        {
            return NotFound(new { message = "Employee not found." });
        }

        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(
        CreateEmployeeDto dto)
    {
        try
        {
            var employee = await _employeeService.CreateEmployeeAsync(dto);

            return CreatedAtAction(
                nameof(GetEmployeeById),
                new { id = employee.EmployeeId },
                employee);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<EmployeeDto>> UpdateEmployee( int id, UpdateEmployeeDto dto)
    {
        try
        {
            var employee = await _employeeService .UpdateEmployeeAsync(id, dto);
            if (employee == null)
            {
                return NotFound(new
                {
                    message = "Employee not found."
                });
            }

            return Ok(employee);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:int}/deactivate")]
    public async Task<IActionResult> DeactivateEmployee(int id)
    {
        var success = await _employeeService.DeactivateEmployeeAsync(id);
    if (!success)
    {
        return NotFound(new
        {
            message = "Employee not found."
        });
    }

    return Ok(new
    {
        message = "Employee deactivated successfully."
    });
    }
}