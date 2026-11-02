using bms.Api.DTOs;
using bms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace bms.Api.Controllers;
[ApiController]
[Route("api/payment-methods")]
public class PaymentMethodsController : ControllerBase
{
    private readonly PaymentMethodService _service;
    public PaymentMethodsController(PaymentMethodService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<PaymentMethodDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PaymentMethodDto>> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<PaymentMethodDto>> Create(
        CreatePaymentMethodDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = result.PaymentMethodId },
                result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PaymentMethodDto>> Update(
        int id, UpdatePaymentMethodDto dto)
    {
        try
        {
            var result = await _service.UpdateAsync(id, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _service.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}