using System.ComponentModel.DataAnnotations;

namespace bms.Api.DTOs;
public class CreateTaskDto
{
    [Required] public int ProjectId { get; set; }
    public int? EmployeeId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public int StatusId { get; set; }

    [Required]
    public int PriorityId { get; set; }
}