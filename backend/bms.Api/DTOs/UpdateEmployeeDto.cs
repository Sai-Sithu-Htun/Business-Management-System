using System.ComponentModel.DataAnnotations;

namespace bms.Api.DTOs;
public class UpdateEmployeeDto
{
    [Required]
    public int DepartmentId { get; set; }

    [Required]
    [MaxLength(100)]
    public string JobTitle { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? Phone { get; set; }

    [Required]
    public DateTime HireDate { get; set; }

    [MaxLength(500)]
    public string? EmployeePic { get; set; }

    public bool IsActive { get; set; }
}