using System.ComponentModel.DataAnnotations;
namespace bms.Api.DTOs;
public class CreatePaymentDto
{
    [Required]
    public int ProjectId { get; set; }
    public int? PaymentMethodId { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    [MaxLength(100)]
    public string? ReferenceNumber { get; set; }

    public string? Notes { get; set; }
}