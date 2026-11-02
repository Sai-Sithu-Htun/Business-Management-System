using System.ComponentModel.DataAnnotations;

namespace bms.Api.DTOs;
public class UpdateCustomerDto
{
    [MaxLength(100)] public string? CustomerName { get; set; }
    [MaxLength(100)]
    public string? CompanyName { get; set; }

    [EmailAddress]
    [MaxLength(255)]
    public string? Email { get; set; }

    [MaxLength(30)]
    public string? Phone { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }
}