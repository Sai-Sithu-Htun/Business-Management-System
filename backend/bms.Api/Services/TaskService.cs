using bms.Api.Data;
using bms.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace bms.Api.Services;
public class TaskService
{
    private readonly BmsDbContext _context;
    public TaskService(BmsDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskDto>> GetAllTasksAsync()
    {
        return await _context.Tasks
            .AsNoTracking()
            .OrderBy(t => t.TaskId)
            .Select(t => new TaskDto
            {
                TaskId = t.TaskId,
                ProjectId = t.ProjectId,
                EmployeeId = t.EmployeeId,
                Title = t.Title,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                StatusId = t.StatusId,
                PriorityId = t.PriorityId,
                CompletedAt = t.CompletedAt,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<TaskDto>> GetTasksByProjectIdAsync(int projectId)
    {
        return await _context.Tasks
            .AsNoTracking()
            .Where(t => t.ProjectId == projectId)
            .OrderBy(t => t.TaskId)
            .Select(t => new TaskDto
            {
                TaskId = t.TaskId,
                ProjectId = t.ProjectId,
                EmployeeId = t.EmployeeId,
                Title = t.Title,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                StatusId = t.StatusId,
                PriorityId = t.PriorityId,
                CompletedAt = t.CompletedAt,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<List<TaskDto>> GetTasksByStatusIdAsync(int statusId)
    {
        return await _context.Tasks
            .AsNoTracking()
            .Where(t => t.StatusId == statusId)
            .OrderBy(t => t.TaskId)
            .Select(t => new TaskDto
            {
                TaskId = t.TaskId,
                ProjectId = t.ProjectId,
                EmployeeId = t.EmployeeId,
                Title = t.Title,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                StatusId = t.StatusId,
                PriorityId = t.PriorityId,
                CompletedAt = t.CompletedAt,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<TaskDto?> GetTaskByIdAsync(int id)
    {
        return await _context.Tasks
            .AsNoTracking()
            .Where(t => t.TaskId == id)
            .Select(t => new TaskDto
            {
                TaskId = t.TaskId,
                ProjectId = t.ProjectId,
                EmployeeId = t.EmployeeId,
                Title = t.Title,
                Description = t.Description,
                StartDate = t.StartDate,
                DueDate = t.DueDate,
                StatusId = t.StatusId,
                PriorityId = t.PriorityId,
                CompletedAt = t.CompletedAt,
                CreatedAt = t.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto)
    {
        if (dto.DueDate.Date < dto.StartDate.Date)
        {
            throw new InvalidOperationException(
                "Due date cannot be earlier than start date.");
        }

        if (!await _context.Projects.AnyAsync(
                p => p.ProjectId == dto.ProjectId))
        {
            throw new InvalidOperationException("Project not found.");
        }

        if (dto.EmployeeId.HasValue &&
            !await _context.Employees.AnyAsync(
                e => e.EmployeeId == dto.EmployeeId.Value))
        {
            throw new InvalidOperationException("Employee not found.");
        }

        if (!await _context.TaskStatuses.AnyAsync(
                s => s.StatusId == dto.StatusId))
        {
            throw new InvalidOperationException("Task status not found.");
        }

        if (!await _context.TaskPriorities.AnyAsync(
                p => p.PriorityId == dto.PriorityId))
        {
            throw new InvalidOperationException("Task priority not found.");
        }

        var task = new bms.Api.Models.Task
        {
            ProjectId = dto.ProjectId,
            EmployeeId = dto.EmployeeId,
            Title = dto.Title,
            Description = dto.Description,
            StartDate = dto.StartDate,
            DueDate = dto.DueDate,
            StatusId = dto.StatusId,
            PriorityId = dto.PriorityId,
            CompletedAt = dto.StatusId == 3
                ? DateTime.Now
                : null
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return new TaskDto
        {
            TaskId = task.TaskId,
            ProjectId = task.ProjectId,
            EmployeeId = task.EmployeeId,
            Title = task.Title,
            Description = task.Description,
            StartDate = task.StartDate,
            DueDate = task.DueDate,
            StatusId = task.StatusId,
            PriorityId = task.PriorityId,
            CompletedAt = task.CompletedAt,
            CreatedAt = task.CreatedAt
        };
    }

    public async Task<TaskDto?> UpdateTaskAsync( int id, UpdateTaskDto dto)
    {
        if (dto.DueDate.Date < dto.StartDate.Date)
        {
            throw new InvalidOperationException( "Due date cannot be earlier than start date."); }
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.TaskId == id);

            if (task == null)
            {
                return null;
            }

            if (!await _context.Projects.AnyAsync(
                    p => p.ProjectId == dto.ProjectId))
            {
                throw new InvalidOperationException("Project not found.");
            }

            if (dto.EmployeeId.HasValue &&
                !await _context.Employees.AnyAsync(
                    e => e.EmployeeId == dto.EmployeeId.Value))
            {
                throw new InvalidOperationException("Employee not found.");
            }

            if (!await _context.TaskStatuses.AnyAsync(
                    s => s.StatusId == dto.StatusId))
            {
                throw new InvalidOperationException("Task status not found.");
            }

            if (!await _context.TaskPriorities.AnyAsync(
                    p => p.PriorityId == dto.PriorityId))
            {
                throw new InvalidOperationException("Task priority not found.");
            }

            task.ProjectId = dto.ProjectId;
            task.EmployeeId = dto.EmployeeId;
            task.Title = dto.Title;
            task.Description = dto.Description;
            task.StartDate = dto.StartDate;
            task.DueDate = dto.DueDate;
            task.StatusId = dto.StatusId;
            task.PriorityId = dto.PriorityId;

            if (dto.StatusId == 3 && task.CompletedAt == null)
            {
                task.CompletedAt = DateTime.Now;
            }
            else if (dto.StatusId != 3)
            {
                task.CompletedAt = null;
            }

            await _context.SaveChangesAsync();

            return new TaskDto
            {
                TaskId = task.TaskId,
                ProjectId = task.ProjectId,
                EmployeeId = task.EmployeeId,
                Title = task.Title,
                Description = task.Description,
                StartDate = task.StartDate,
                DueDate = task.DueDate,
                StatusId = task.StatusId,
                PriorityId = task.PriorityId,
                CompletedAt = task.CompletedAt,
                CreatedAt = task.CreatedAt
            };
    }
}