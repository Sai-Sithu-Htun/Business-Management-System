using System.ComponentModel.DataAnnotations;

namespace bms.Api.DTOs;

public class UpdatePaymentMethodDto
{
    [Required]
    [MaxLength(50)]
    public string MethodName { get; set; } = string.Empty;
}