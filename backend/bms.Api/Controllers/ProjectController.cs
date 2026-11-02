using bms.Api.DTOs;
using bms.Api.Services;
using Microsoft.AspNetCore.Mvc;
using bms.Api.Models;

namespace bms.Api.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly ProjectService _projectService;
    public ProjectsController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectDto>>> GetAllProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectDto>> GetProjectById(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);

        if (project == null)
        {
            return NotFound(new
            {
                message = "Project not found."
            });
        }

        return Ok(project);
    }

    [HttpPost] public async Task<ActionResult<ProjectDto>> CreateProject( CreateProjectDto dto)
    {
        try
        {
            var project = await _projectService.CreateProjectAsync(dto);
            return CreatedAtAction(
            nameof(GetProjectById),
            new { id = project.ProjectId },
            project);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProjectDto>> UpdateProject( int id, UpdateProjectDto dto)
    {
        try
        {
            var project = await _projectService.UpdateProjectAsync(id, dto);
        if (project == null)
        {
            return NotFound(new
            {
                message = "Project not found."
            });
        }

            return Ok(project);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}