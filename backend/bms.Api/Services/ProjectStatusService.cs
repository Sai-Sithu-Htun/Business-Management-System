using bms.Api.DTOs;
using bms.Api.Models;
using Microsoft.EntityFrameworkCore;
using bms.Api.Data;

namespace bms.Api.Services;
public class ProjectStatusService
{
    private readonly BmsDbContext _context;
    public ProjectStatusService(BmsDbContext context)
    {
        _context = context;
    }

public async Task<List<ProjectStatusDto>> GetAllAsync()
{
    return await _context.ProjectStatuses
        .OrderBy(ps => ps.StatusId)
        .Select(ps => new ProjectStatusDto
        {
            StatusId = ps.StatusId,
            StatusName = ps.StatusName
        })
        .ToListAsync();
}

public async Task<ProjectStatusDto?> GetByIdAsync(int id)
{
    return await _context.ProjectStatuses
        .Where(ps => ps.StatusId == id)
        .Select(ps => new ProjectStatusDto
        {
            StatusId = ps.StatusId,
            StatusName = ps.StatusName
        })
        .FirstOrDefaultAsync();
}

public async Task<ProjectStatusDto> CreateAsync(
    CreateProjectStatusDto dto)
{
    string statusName = dto.StatusName.Trim();

    bool exists = await _context.ProjectStatuses
        .AnyAsync(ps => ps.StatusName == statusName);

    if (exists)
    {
        throw new InvalidOperationException(
            "Project status already exists.");
    }

    var status = new ProjectStatus
    {
        StatusName = statusName
    };

    _context.ProjectStatuses.Add(status);
    await _context.SaveChangesAsync();

    return new ProjectStatusDto
    {
        StatusId = status.StatusId,
        StatusName = status.StatusName
    };
}

public async Task<ProjectStatusDto?> UpdateAsync(
    int id, UpdateProjectStatusDto dto)
{
    var status = await _context.ProjectStatuses.FindAsync(id);

    if (status == null)
        return null;

    string statusName = dto.StatusName.Trim();

    bool exists = await _context.ProjectStatuses
        .AnyAsync(ps =>
            ps.StatusName == statusName &&
            ps.StatusId != id);

    if (exists)
    {
        throw new InvalidOperationException(
            "Project status already exists.");
    }

    status.StatusName = statusName;

    await _context.SaveChangesAsync();

    return new ProjectStatusDto
    {
        StatusId = status.StatusId,
        StatusName = status.StatusName
    };
}

public async Task<bool> DeleteAsync(int id)
{
    var status = await _context.ProjectStatuses.FindAsync(id);

    if (status == null)
        return false;

    _context.ProjectStatuses.Remove(status);
    await _context.SaveChangesAsync();

    return true;
}
}