using System.ComponentModel.DataAnnotations;

namespace bms.Api.DTOs;

public class CreateProjectStatusDto
{
    [Required]
    [MaxLength(50)]
    public string StatusName { get; set; } = string.Empty;
}