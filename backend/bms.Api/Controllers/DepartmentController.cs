using bms.Api.DTOs;
using bms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace bms.Api.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentsController : ControllerBase
{
    private readonly DepartmentService _departmentService;
    public DepartmentsController(DepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<DepartmentDto>>> GetAllDepartments()
    {
        var departments = await _departmentService.GetAllDepartmentsAsync();

        return Ok(departments);
    }
}