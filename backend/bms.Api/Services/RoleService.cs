using bms.Api.Data; 
using bms.Api.DTOs;
using Microsoft.EntityFrameworkCore;

namespace bms.Api.Services;

public class RoleService 
{ 
    private readonly BmsDbContext _db;
    public RoleService(BmsDbContext db)
{
    _db = db;
}

public async Task<List<RoleDto>> GetAllRolesAsync()
{
    var roles = await _db.Roles.ToListAsync();

    return roles.Select(role => new RoleDto
    {
        RoleId = role.RoleId,
        RoleName = role.RoleName
    }).ToList();
}
}