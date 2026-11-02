namespace bms.Api.Models;
public class ProjectMember { public int ProjectMemberId { get; set; }
public int ProjectId { get; set; }

public int EmployeeId { get; set; }

public string? Responsibility { get; set; }

public DateTime JoinedAt { get; set; }

public DateTime? LeftAt { get; set; }
}