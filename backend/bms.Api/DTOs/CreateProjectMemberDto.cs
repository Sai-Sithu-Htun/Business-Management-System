using System.ComponentModel.DataAnnotations;
namespace bms.Api.DTOs;
public class CreateProjectMemberDto
{
    [Required]
    public int ProjectId { get; set; }

    [Required]
    public int EmployeeId { get; set; }

    [MaxLength(255)]
    public string? Responsibility { get; set; }
}