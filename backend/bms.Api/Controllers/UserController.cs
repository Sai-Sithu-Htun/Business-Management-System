using bms.Api.DTOs;
using bms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace bms.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
    {
        try
        {
            var user = await _userService.CreateUserAsync(dto);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = user.UserId },
                user
            );
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message == "Email already exists.")
            {
                return Conflict(new { message = ex.Message});
            }

            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> UpdateUser( int id, UpdateUserDto dto)
    {
        try
        {
            var user = await _userService.UpdateUserAsync(id, dto);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(user);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message == "Email already exists.")
            {
                return Conflict(new { message = ex.Message });
            }

                return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:int}/deactivate")]
    public async Task<IActionResult> DeactivateUser(int id)
    {
        var success = await _userService.DeactivateUserAsync(id);
    if (!success)
    {
        return NotFound(new { message = "User not found." });
    }

    return Ok(new { message = "User deactivated successfully." });
    }
}