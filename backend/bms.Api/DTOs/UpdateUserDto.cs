using System.ComponentModel.DataAnnotations;

namespace bms.Api.DTOs;

public class UpdateUserDto
{
    [Required]
    [MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string UserEmail { get; set; } = string.Empty;

    [Required]
    public int RoleId { get; set; }

    [Required]
    public bool IsActive { get; set; }
}