namespace bms.Api.DTOs;

public class CustomerDto
{
    public int CustomerId { get; set; }
    public int? UserId { get; set; }
    public string? CustomerName { get; set; }
    public string? CompanyName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
}