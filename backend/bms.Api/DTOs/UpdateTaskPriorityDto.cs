using System.ComponentModel.DataAnnotations;

namespace bms.Api.DTOs;

public class UpdateTaskPriorityDto
{
    [Required]
    [MaxLength(50)]
    public string PriorityName { get; set; } = string.Empty;
}