namespace bms.Api.DTOs;

public class ProjectDto
{
    public int ProjectId { get; set; }
    public int? CustomerId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public int? ManagerId { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int StatusId { get; set; }
    public DateTime CreatedAt { get; set; }
}