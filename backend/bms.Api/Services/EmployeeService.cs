using bms.Api.Data;
using bms.Api.DTOs;
using bms.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace bms.Api.Services;
public class EmployeeService
{
    private readonly BmsDbContext _context;
    public EmployeeService(BmsDbContext context)
    {
        _context = context;
    }

    public async Task<EmployeeDto> CreateEmployeeAsync(CreateEmployeeDto dto)
    {
        var departmentExists = await _context.Departments
            .AnyAsync(d => d.DepartmentId == dto.DepartmentId);

        if (!departmentExists)
        {
            throw new InvalidOperationException("Department does not exist.");
        }

        if (dto.UserId.HasValue)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.UserId == dto.UserId.Value);

            if (!userExists)
            {
                throw new InvalidOperationException("User does not exist.");
            }

            var employeeExists = await _context.Employees
                .AnyAsync(e => e.UserId == dto.UserId);

            if (employeeExists)
            {
                throw new InvalidOperationException(
                    "This user is already assigned to an employee.");
            }
        }

        var employee = new Employee
        {
            UserId = dto.UserId,
            DepartmentId = dto.DepartmentId,
            JobTitle = dto.JobTitle,
            Phone = dto.Phone,
            HireDate = dto.HireDate,
            EmployeePic = dto.EmployeePic,
            IsActive = true
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return new EmployeeDto
        {
            EmployeeId = employee.EmployeeId,
            UserId = employee.UserId,
            DepartmentId = employee.DepartmentId,
            JobTitle = employee.JobTitle,
            Phone = employee.Phone,
            HireDate = employee.HireDate,
            EmployeePic = employee.EmployeePic,
            IsActive = employee.IsActive
        };
}

public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
{
    return await _context.Employees
        .AsNoTracking()
        .Select(e => new EmployeeDto
        {
            EmployeeId = e.EmployeeId,
            UserId = e.UserId,
            DepartmentId = e.DepartmentId,
            JobTitle = e.JobTitle,
            Phone = e.Phone,
            HireDate = e.HireDate,
            EmployeePic = e.EmployeePic,
            IsActive = e.IsActive
        })
        .ToListAsync();
}

public async Task<EmployeeDto?> GetEmployeeByIdAsync(int id)
{
    return await _context.Employees
        .AsNoTracking()
        .Where(e => e.EmployeeId == id)
        .Select(e => new EmployeeDto
        {
            EmployeeId = e.EmployeeId,
            UserId = e.UserId,
            DepartmentId = e.DepartmentId,
            JobTitle = e.JobTitle,
            Phone = e.Phone,
            HireDate = e.HireDate,
            EmployeePic = e.EmployeePic,
            IsActive = e.IsActive
        })
        .FirstOrDefaultAsync();
}

public async Task<EmployeeDto?> UpdateEmployeeAsync( int id, UpdateEmployeeDto dto)
{
    var employee = await _context.Employees .FirstOrDefaultAsync(e => e.EmployeeId == id);
    if (employee == null)
    {
        return null;
    }

    var departmentExists = await _context.Departments
        .AnyAsync(d => d.DepartmentId == dto.DepartmentId);

    if (!departmentExists)
    {
        throw new InvalidOperationException(
            "Department does not exist.");
    }

    employee.DepartmentId = dto.DepartmentId;
    employee.JobTitle = dto.JobTitle;
    employee.Phone = dto.Phone;
    employee.HireDate = dto.HireDate;
    employee.EmployeePic = dto.EmployeePic;
    employee.IsActive = dto.IsActive;

    await _context.SaveChangesAsync();

    return new EmployeeDto
    {
        EmployeeId = employee.EmployeeId,
        UserId = employee.UserId,
        DepartmentId = employee.DepartmentId,
        JobTitle = employee.JobTitle,
        Phone = employee.Phone,
        HireDate = employee.HireDate,
        EmployeePic = employee.EmployeePic,
        IsActive = employee.IsActive
    };
}

public async Task<bool> DeactivateEmployeeAsync(int id)
{
    var employee = await _context.Employees
        .FirstOrDefaultAsync(e => e.EmployeeId == id);
    if (employee == null)
    {
        return false;
    }

    employee.IsActive = false;

    await _context.SaveChangesAsync();

    return true;
}
}