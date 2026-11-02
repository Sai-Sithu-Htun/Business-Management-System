public class UserDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;

    public string UserEmail { get; set; } = string.Empty;

    public int RoleId { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }
}