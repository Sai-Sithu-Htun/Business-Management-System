using bms.Api.DTOs;
using bms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace bms.Api.Controllers;
[ApiController]
[Route("api/project-statuses")]
public class ProjectStatusesController : ControllerBase
{
    private readonly ProjectStatusService _service;
public ProjectStatusesController(ProjectStatusService service)
{
    _service = service;
}

[HttpGet]
public async Task<ActionResult<List<ProjectStatusDto>>> GetAll()
{
    return Ok(await _service.GetAllAsync());
}

[HttpGet("{id:int}")]
public async Task<ActionResult<ProjectStatusDto>> GetById(int id)
{
    var result = await _service.GetByIdAsync(id);

    if (result == null)
        return NotFound();

    return Ok(result);
}

[HttpPost]
public async Task<ActionResult<ProjectStatusDto>> Create(
    CreateProjectStatusDto dto)
{
    try
    {
        var result = await _service.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.StatusId },
            result);
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}

[HttpPut("{id:int}")]
public async Task<ActionResult<ProjectStatusDto>> Update(
    int id, UpdateProjectStatusDto dto)
{
    try
    {
        var result = await _service.UpdateAsync(id, dto);

        if (result == null)
            return NotFound();

        return Ok(result);
    }
    catch (InvalidOperationException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
}

[HttpDelete("{id:int}")]
public async Task<IActionResult> Delete(int id)
{
    bool deleted = await _service.DeleteAsync(id);

    if (!deleted)
        return NotFound();

    return NoContent();
}
}