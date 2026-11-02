using bms.Api.DTOs;
using bms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace bms.Api.Controllers;
[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly PaymentService _paymentService;

    public PaymentsController(PaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PaymentDto>>> GetAll()
    {
        return Ok(await _paymentService.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PaymentDto>> GetById(int id)
    {
        var payment = await _paymentService.GetByIdAsync(id);

        if (payment == null)
            return NotFound(new { message = "Payment not found." });

        return Ok(payment);
    }

    [HttpPost]
    public async Task<ActionResult<PaymentDto>> Create(
        CreatePaymentDto dto)
    {
        try
        {
            var payment = await _paymentService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = payment.PaymentId },
                payment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PaymentDto>> Update(
        int id,
        UpdatePaymentDto dto)
    {
        try
        {
            var payment = await _paymentService.UpdateAsync(id, dto);

            if (payment == null)
                return NotFound(new { message = "Payment not found." });

            return Ok(payment);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _paymentService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Payment not found." });

        return NoContent();
    }
}