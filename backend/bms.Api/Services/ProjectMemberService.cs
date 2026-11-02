using bms.Api.Data;
using bms.Api.DTOs;
using bms.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace bms.Api.Services;
public class ProjectMemberService
{
    private readonly BmsDbContext _context;
    public ProjectMemberService(BmsDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProjectMemberDto>> GetAllAsync()
    {
        return await _context.ProjectMembers
            .AsNoTracking()
            .OrderBy(pm => pm.ProjectMemberId)
            .Select(pm => new ProjectMemberDto
            {
                ProjectMemberId = pm.ProjectMemberId,
                ProjectId = pm.ProjectId,
                EmployeeId = pm.EmployeeId,
                Responsibility = pm.Responsibility,
                JoinedAt = pm.JoinedAt,
                LeftAt = pm.LeftAt
            })
            .ToListAsync();
    }

    public async Task<ProjectMemberDto?> GetByIdAsync(int id)
    {
        var member = await _context.ProjectMembers
            .AsNoTracking()
            .FirstOrDefaultAsync(pm => pm.ProjectMemberId == id);
    if (member == null)
        return null;

    return new ProjectMemberDto
    {
        ProjectMemberId = member.ProjectMemberId,
        ProjectId = member.ProjectId,
        EmployeeId = member.EmployeeId,
        Responsibility = member.Responsibility,
        JoinedAt = member.JoinedAt,
        LeftAt = member.LeftAt
    };
    }

    public async Task<ProjectMemberDto> CreateAsync(
        CreateProjectMemberDto dto)
    {
        var memberExists = await _context.ProjectMembers
            .AnyAsync(pm =>
                pm.ProjectId == dto.ProjectId &&
                pm.EmployeeId == dto.EmployeeId);

        if (memberExists)
            throw new InvalidOperationException(
                "This employee is already assigned to this project.");

        var projectExists = await _context.Projects
            .AnyAsync(p => p.ProjectId == dto.ProjectId);

        if (!projectExists)
            throw new InvalidOperationException("Project not found.");

        var employeeExists = await _context.Employees
            .AnyAsync(e => e.EmployeeId == dto.EmployeeId);

        if (!employeeExists)
            throw new InvalidOperationException("Employee not found.");

        var member = new ProjectMember
        {
            ProjectId = dto.ProjectId,
            EmployeeId = dto.EmployeeId,
            Responsibility = dto.Responsibility,
            JoinedAt = DateTime.Now,
        };

        _context.ProjectMembers.Add(member);
        await _context.SaveChangesAsync();

        await _context.Entry(member)
            .ReloadAsync();

        return new ProjectMemberDto
        {
            ProjectMemberId = member.ProjectMemberId,
            ProjectId = member.ProjectId,
            EmployeeId = member.EmployeeId,
            Responsibility = member.Responsibility,
            JoinedAt = member.JoinedAt,
            LeftAt = member.LeftAt
        };
    }

    public async Task<ProjectMemberDto?> UpdateAsync( int id, UpdateProjectMemberDto dto)
    {
        var member = await _context.ProjectMembers .FirstOrDefaultAsync(pm => pm.ProjectMemberId == id);
        if (member == null)
            return null;

        var duplicateExists = await _context.ProjectMembers
            .AnyAsync(pm =>
            pm.ProjectMemberId != id &&
            pm.ProjectId == dto.ProjectId &&
            pm.EmployeeId == dto.EmployeeId);

        if (duplicateExists)
            throw new InvalidOperationException(
                "This employee is already assigned to this project.");

        var projectExists = await _context.Projects
            .AnyAsync(p => p.ProjectId == dto.ProjectId);

        if (!projectExists)
            throw new InvalidOperationException("Project not found.");

        var employeeExists = await _context.Employees
            .AnyAsync(e => e.EmployeeId == dto.EmployeeId);

        if (!employeeExists)
            throw new InvalidOperationException("Employee not found.");

        member.ProjectId = dto.ProjectId;
        member.EmployeeId = dto.EmployeeId;
        member.Responsibility = dto.Responsibility;
        member.LeftAt = dto.LeftAt;

        await _context.SaveChangesAsync();

        return new ProjectMemberDto
        {
            ProjectMemberId = member.ProjectMemberId,
            ProjectId = member.ProjectId,
            EmployeeId = member.EmployeeId,
            Responsibility = member.Responsibility,
            JoinedAt = member.JoinedAt,
            LeftAt = member.LeftAt
        };
    }
}