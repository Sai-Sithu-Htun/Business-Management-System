using bms.Api.DTOs;
using bms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace bms.Api.Controllers;
[ApiController]
[Route("api/project-members")]

public class ProjectMembersController : ControllerBase
{
    private readonly ProjectMemberService _service;
    public ProjectMembersController(ProjectMemberService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProjectMemberDto>>> GetAll()
    {
        var members = await _service.GetAllAsync();
        return Ok(members);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProjectMemberDto>> GetById(int id)
    {
        var member = await _service.GetByIdAsync(id);

        if (member == null)
            return NotFound(new { message = "Project member not found." });

        return Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectMemberDto>> Create(
        CreateProjectMemberDto dto)
    {
        try
        {
            var member = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = member.ProjectMemberId },
                member);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProjectMemberDto>> Update(
        int id,
        UpdateProjectMemberDto dto)
    {
        try
        {
            var member = await _service.UpdateAsync(id, dto);

            if (member == null)
                return NotFound(new { message = "Project member not found." });

            return Ok(member);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}