namespace bms.Api.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public int? UserId { get; set; }

    public int DepartmentId { get; set; }

    public string JobTitle { get; set; } = string.Empty;

    public string? Phone { get; set; }

    public DateTime HireDate { get; set; }

    public string? EmployeePic { get; set; }

    public bool IsActive { get; set; } = true;
}