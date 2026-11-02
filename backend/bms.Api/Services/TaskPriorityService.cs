using bms.Api.DTOs;
using bms.Api.Models;
using Microsoft.EntityFrameworkCore;
using bms.Api.Data;

namespace bms.Api.Services;
public class TaskPriorityService
{
    private readonly BmsDbContext _context;
    public TaskPriorityService(BmsDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskPriorityDto>> GetAllAsync()
    {
        return await _context.TaskPriorities
            .OrderBy(p => p.PriorityId)
            .Select(p => new TaskPriorityDto
            {
                PriorityId = p.PriorityId,
                PriorityName = p.PriorityName
            })
            .ToListAsync();
    }

public async Task<TaskPriorityDto?> GetByIdAsync(int id)
{
    return await _context.TaskPriorities
        .Where(p => p.PriorityId == id)
        .Select(p => new TaskPriorityDto
        {
            PriorityId = p.PriorityId,
            PriorityName = p.PriorityName
        })
        .FirstOrDefaultAsync();
}

public async Task<TaskPriorityDto> CreateAsync(
    CreateTaskPriorityDto dto)
{
    string name = dto.PriorityName.Trim();

    bool exists = await _context.TaskPriorities
        .AnyAsync(p => p.PriorityName == name);

    if (exists)
        throw new InvalidOperationException(
            "Task priority already exists.");

    var priority = new TaskPriority
    {
        PriorityName = name
    };

    _context.TaskPriorities.Add(priority);
    await _context.SaveChangesAsync();

    return new TaskPriorityDto
    {
        PriorityId = priority.PriorityId,
        PriorityName = priority.PriorityName
    };
}

public async Task<TaskPriorityDto?> UpdateAsync(
    int id, UpdateTaskPriorityDto dto)
{
    var priority = await _context.TaskPriorities.FindAsync(id);

    if (priority == null)
        return null;

    string name = dto.PriorityName.Trim();

    bool exists = await _context.TaskPriorities
        .AnyAsync(p =>
            p.PriorityName == name &&
            p.PriorityId != id);

    if (exists)
        throw new InvalidOperationException(
            "Task priority already exists.");

    priority.PriorityName = name;

    await _context.SaveChangesAsync();

    return new TaskPriorityDto
    {
        PriorityId = priority.PriorityId,
        PriorityName = priority.PriorityName
    };
}

public async Task<bool> DeleteAsync(int id)
{
    var priority = await _context.TaskPriorities.FindAsync(id);

    if (priority == null)
        return false;

    _context.TaskPriorities.Remove(priority);
    await _context.SaveChangesAsync();

    return true;
}
}