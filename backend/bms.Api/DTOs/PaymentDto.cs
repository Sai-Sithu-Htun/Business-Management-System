namespace bms.Api.DTOs;
public class PaymentDto
{
    public int PaymentId { get; set; }
    public int ProjectId { get; set; }

    public int? PaymentMethodId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? ReferenceNumber { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
}