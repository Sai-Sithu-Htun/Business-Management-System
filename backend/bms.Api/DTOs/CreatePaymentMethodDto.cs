using System.ComponentModel.DataAnnotations;

namespace bms.Api.DTOs;

public class CreatePaymentMethodDto
{
    [Required]
    [MaxLength(50)]
    public string MethodName { get; set; } = string.Empty;
}