using bms.Api.Data;
using bms.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using bms.Api.Models;

namespace bms.Api.Services;

public class ProjectService
{
    private readonly BmsDbContext _context;
    public ProjectService(BmsDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProjectDto>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .AsNoTracking()
            .OrderBy(p => p.ProjectId)
            .Select(p => new ProjectDto
            {
                ProjectId = p.ProjectId,
                CustomerId = p.CustomerId,
                ProjectName = p.ProjectName,
                ManagerId = p.ManagerId,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                StatusId = p.StatusId,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<ProjectDto?> GetProjectByIdAsync(int id)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.ProjectId == id)
            .Select(p => new ProjectDto
            {
                ProjectId = p.ProjectId,
                CustomerId = p.CustomerId,
                ProjectName = p.ProjectName,
                ManagerId = p.ManagerId,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                StatusId = p.StatusId,
                CreatedAt = p.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto)
    {
        if (dto.EndDate.HasValue && dto.EndDate.Value.Date < dto.StartDate.Date)
        {
            throw new InvalidOperationException( "End date cannot be earlier than start date."); }
            var statusExists = await _context.ProjectStatuses
                .AnyAsync(s => s.StatusId == dto.StatusId);

            if (!statusExists)
            {
                throw new InvalidOperationException(
                    "Invalid project status.");
            }

            if (dto.CustomerId.HasValue)
            {
                var customerExists = await _context.Customers
                    .AnyAsync(c => c.CustomerId == dto.CustomerId.Value);

            if (!customerExists)
            {
                throw new InvalidOperationException(
                    "Customer not found.");
            }
            }

            if (dto.ManagerId.HasValue)
            {
                var managerExists = await _context.Employees
                    .AnyAsync(e => e.EmployeeId == dto.ManagerId.Value);

                if (!managerExists)
                {
                    throw new InvalidOperationException(
                        "Manager employee not found.");
                }
            }

            var project = new Project
            {
                CustomerId = dto.CustomerId,
                ProjectName = dto.ProjectName,
                ManagerId = dto.ManagerId,
                Description = dto.Description,
                StartDate = dto.StartDate.Date,
                EndDate = dto.EndDate?.Date,
                StatusId = dto.StatusId
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return new ProjectDto
            {
            ProjectId = project.ProjectId,
            CustomerId = project.CustomerId,
            ProjectName = project.ProjectName,
            ManagerId = project.ManagerId,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            StatusId = project.StatusId,
            CreatedAt = project.CreatedAt
            };
    }

    public async Task<ProjectDto?> UpdateProjectAsync( int id, UpdateProjectDto dto)
    {
        if (dto.EndDate.HasValue && dto.EndDate.Value.Date < dto.StartDate.Date)
        {
            throw new InvalidOperationException( "End date cannot be earlier than start date."); }
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectId == id);

        if (project == null)
        {
        return null;
        }

        var statusExists = await _context.ProjectStatuses
            .AnyAsync(s => s.StatusId == dto.StatusId);

        if (!statusExists)
        {
            throw new InvalidOperationException("Invalid project status.");
        }

        if (dto.CustomerId.HasValue &&
        !await _context.Customers.AnyAsync(
        c => c.CustomerId == dto.CustomerId.Value))
        {
            throw new InvalidOperationException("Customer not found.");
        }

        if (dto.ManagerId.HasValue &&
        !await _context.Employees.AnyAsync(
        e => e.EmployeeId == dto.ManagerId.Value))
        {
            throw new InvalidOperationException("Manager employee not found.");
        }

    project.CustomerId = dto.CustomerId;
    project.ProjectName = dto.ProjectName;
    project.ManagerId = dto.ManagerId;
    project.Description = dto.Description;
    project.StartDate = dto.StartDate.Date;
    project.EndDate = dto.EndDate?.Date;
    project.StatusId = dto.StatusId;

    await _context.SaveChangesAsync();

    return new ProjectDto
    {
        ProjectId = project.ProjectId,
        CustomerId = project.CustomerId,
        ProjectName = project.ProjectName,
        ManagerId = project.ManagerId,
        Description = project.Description,
        StartDate = project.StartDate,
        EndDate = project.EndDate,
        StatusId = project.StatusId,
        CreatedAt = project.CreatedAt
    };
    }
}