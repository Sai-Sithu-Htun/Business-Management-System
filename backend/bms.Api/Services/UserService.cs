using bms.Api.Data;
using bms.Api.DTOs;
using Microsoft.EntityFrameworkCore;
using bms.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace bms.Api.Services;
public class UserService
{
    private readonly BmsDbContext _context;
    public UserService(BmsDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        var existingUser = await _context.Users.AnyAsync(u => u.UserEmail == dto.UserEmail);

        if (existingUser)
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var roleExists = await _context.Roles.AnyAsync(r => r.RoleId == dto.RoleId);

        if (!roleExists)
        {
            throw new InvalidOperationException("Role does not exists.");
        }

        var user = new User { UserName = dto.UserName, UserEmail = dto.UserEmail, RoleId = dto.RoleId, IsActive = true, CreatedAt = DateTime.Now };
        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, dto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
};
}

public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto dto)
{
    var user = await _context.Users .FirstOrDefaultAsync(u => u.UserId == id);
    if (user == null)
    {
        return null;
    }

    var emailExists = await _context.Users
        .AnyAsync(u => u.UserEmail == dto.UserEmail
                && u.UserId != id);

    if (emailExists)
    {
        throw new InvalidOperationException("Email already exists.");
    }

    var roleExists = await _context.Roles
        .AnyAsync(r => r.RoleId == dto.RoleId);

    if (!roleExists)
    {
        throw new InvalidOperationException("Role does not exist.");
    }

    user.UserName = dto.UserName;
    user.UserEmail = dto.UserEmail;
    user.RoleId = dto.RoleId;
    user.IsActive = dto.IsActive;

    await _context.SaveChangesAsync();

    return new UserDto
    {
        UserId = user.UserId,
        UserName = user.UserName,
        UserEmail = user.UserEmail,
        RoleId = user.RoleId,
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt
    };
}

public async Task<bool> DeactivateUserAsync(int id)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.UserId == id);

    if (user == null)
    {
        return false;
    }

    user.IsActive = false;

    await _context.SaveChangesAsync();

    return true;
}

public async Task<List<UserDto>> GetAllUsersAsync()
{
    return await _context.Users
        .AsNoTracking()
        .Select(user => new UserDto
        {
            UserId = user.UserId,
            UserName = user.UserName,
            UserEmail = user.UserEmail,
            RoleId = user.RoleId,
            RoleName = user.Role.RoleName,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        })
        .ToListAsync();
}

public async Task<UserDto?> GetUserByIdAsync(int id)
{
    return await _context.Users
        .AsNoTracking()
        .Where(user => user.UserId == id)
        .Select(user => new UserDto
        {
            UserId = user.UserId,
            UserName = user.UserName,
            UserEmail = user.UserEmail,
            RoleId = user.RoleId,
            RoleName = user.Role.RoleName,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        })
        .FirstOrDefaultAsync();
}
}