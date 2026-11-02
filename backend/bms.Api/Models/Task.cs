namespace bms.Api.Models;
public class Task
{
    public int TaskId { get; set; }
    public int ProjectId { get; set; }

    public int? EmployeeId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime DueDate { get; set; }

    public int StatusId { get; set; }

    public int PriorityId { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}