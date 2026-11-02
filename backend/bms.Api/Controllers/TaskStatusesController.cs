using bms.Api.DTOs;
using bms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace bms.Api.Controllers;
[ApiController]
[Route("api/task-statuses")]
public class TaskStatusesController : ControllerBase
{
    private readonly TaskStatusService _service;
    public TaskStatusesController(TaskStatusService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskStatusDto>>> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }
}