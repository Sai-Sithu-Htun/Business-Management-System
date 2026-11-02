using bms.Api.Data;
using bms.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace bms.Api.Services;

public class DepartmentService
{
    private readonly BmsDbContext _context;
    public DepartmentService(BmsDbContext context)
    {
        _context = context;
    }

    public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
    {
        return await _context.Departments
            .AsNoTracking()
            .Select(d => new DepartmentDto
            {
                DepartmentId = d.DepartmentId,
                DepartmentName = d.DepartmentName
            })
            .ToListAsync();
    }
}