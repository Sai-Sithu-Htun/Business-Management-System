using System.ComponentModel.DataAnnotations;
namespace bms.Api.DTOs;
public class UpdateProjectDto
{
    public int? CustomerId { get; set; }
    [Required]
    [MaxLength(200)]
    public string ProjectName { get; set; } = string.Empty;

    public int? ManagerId { get; set; }

    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    public int StatusId { get; set; }
}