using System.ComponentModel.DataAnnotations;

namespace bms.Api.DTOs;

public class CreateUserDto
{
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(225)]
    public string UserEmail { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public int RoleId { get; set; }
}