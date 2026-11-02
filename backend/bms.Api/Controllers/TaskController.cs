using bms.Api.DTOs;
using bms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace bms.Api.Controllers;
[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly TaskService _taskService;
    public TasksController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskDto>>> GetAllTasks()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("project/{projectId:int}")]
    public async Task<ActionResult<List<TaskDto>>> GetTasksByProjectId( int projectId)
    {
        var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
        return Ok(tasks);
    }

    [HttpGet("status/{statusId:int}")]
    public async Task<ActionResult<List<TaskDto>>> GetTasksByStatusId( int statusId)
    {
        var tasks = await _taskService.GetTasksByStatusIdAsync(statusId);
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskDto>> GetTaskById(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);

        if (task == null)
        {
            return NotFound(new { message = "Task not found." });
        }

        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask(
        CreateTaskDto dto)
    {
        try
        {
            var task = await _taskService.CreateTaskAsync(dto);

            return CreatedAtAction(
                nameof(GetTaskById),
                new { id = task.TaskId },
                task);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskDto>> UpdateTask( int id, UpdateTaskDto dto)
    {
        try
        {
            var task = await _taskService.UpdateTaskAsync(id, dto);
            if (task == null)
            {
                return NotFound(new { message = "Task not found." });
            }

            return Ok(task);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}