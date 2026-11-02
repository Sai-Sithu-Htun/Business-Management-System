using bms.Api.DTOs;
using bms.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace bms.Api.Controllers;
[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly CustomerService _customerService;
    public CustomersController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerDto>>> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);

        if (customer == null)
        {
            return NotFound(new { message = "Customer not found." });
        }

        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(
        CreateCustomerDto dto)
    {
        try
        {
            var customer = await _customerService.CreateCustomerAsync(dto);

            return CreatedAtAction(
                nameof(GetCustomerById),
                new { id = customer.CustomerId },
                customer);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomer( int id, UpdateCustomerDto dto)
    { var customer = await _customerService .UpdateCustomerAsync(id, dto);
    if (customer == null)
    {
        return NotFound(new
        {
            message = "Customer not found."
        });
    }

    return Ok(customer);
    }
}