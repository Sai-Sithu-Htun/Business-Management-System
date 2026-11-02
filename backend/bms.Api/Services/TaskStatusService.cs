using bms.Api.Data; using bms.Api.DTOs; using Microsoft.EntityFrameworkCore;
namespace bms.Api.Services;
public class TaskStatusService { private readonly BmsDbContext _context;
public TaskStatusService(BmsDbContext context)
{
    _context = context;
}

public async Task<List<TaskStatusDto>> GetAllAsync()
{
    return await _context.TaskStatuses
        .OrderBy(s => s.StatusId)
        .Select(s => new TaskStatusDto
        {
            StatusId = s.StatusId,
            StatusName = s.StatusName
        })
        .ToListAsync();
}
}