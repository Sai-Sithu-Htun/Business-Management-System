using bms.Api.Data;
using bms.Api.DTOs;
using bms.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace bms.Api.Services;
public class CustomerService
{
    private readonly BmsDbContext _context;
    public CustomerService(BmsDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerDto>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .AsNoTracking()
            .OrderBy(c => c.CustomerId)
            .Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                UserId = c.UserId,
                CustomerName = c.CustomerName,
                CompanyName = c.CompanyName,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        return await _context.Customers
            .AsNoTracking()
            .Where(c => c.CustomerId == id)
            .Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                UserId = c.UserId,
                CustomerName = c.CustomerName,
                CompanyName = c.CompanyName,
                Email = c.Email,
                Phone = c.Phone,
                Address = c.Address,
                CreatedAt = c.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CustomerDto> CreateCustomerAsync(
        CreateCustomerDto dto)
    {
        if (dto.UserId.HasValue)
        {
            var userExists = await _context.Users
                .AnyAsync(u => u.UserId == dto.UserId.Value);

            if (!userExists)
            {
                throw new InvalidOperationException(
                    "User does not exist.");
            }

            var customerExists = await _context.Customers
                .AnyAsync(c => c.UserId == dto.UserId.Value);

            if (customerExists)
            {
                throw new InvalidOperationException(
                    "This user is already assigned to a customer.");
            }
        }

        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            var emailExists = await _context.Customers
                .AnyAsync(c => c.Email == dto.Email);
            if (emailExists)
            {
                throw new InvalidOperationException(
                    "A customer with this email already exists.");
            }
        }

        var customer = new Customer
        {
            UserId = dto.UserId,
            CustomerName = dto.CustomerName,
            CompanyName = dto.CompanyName,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return new CustomerDto
        {
            CustomerId = customer.CustomerId,
            UserId = customer.UserId,
            CustomerName = customer.CustomerName,
            CompanyName = customer.CompanyName,
            Email = customer.Email,
            Phone = customer.Phone,
            Address = customer.Address,
            CreatedAt = customer.CreatedAt
        };
    }
    public async Task<CustomerDto?> UpdateCustomerAsync( int id, UpdateCustomerDto dto)
    {
        var customer = await _context.Customers .FirstOrDefaultAsync(c => c.CustomerId == id);
    if (customer == null)
    {
        return null;
    }

    if (!string.IsNullOrWhiteSpace(dto.Email))
    {
        var emailExists = await _context.Customers
            .AnyAsync(c => c.Email == dto.Email && c.CustomerId != id);
            if (emailExists)
            {
                throw new InvalidOperationException(
                    "A customer with this email already exists.");
            }
    }

    customer.CustomerName = dto.CustomerName;
    customer.CompanyName = dto.CompanyName;
    customer.Email = dto.Email;
    customer.Phone = dto.Phone;
    customer.Address = dto.Address;

    await _context.SaveChangesAsync();

    return new CustomerDto
    {
        CustomerId = customer.CustomerId,
        UserId = customer.UserId,
        CustomerName = customer.CustomerName,
        CompanyName = customer.CompanyName,
        Email = customer.Email,
        Phone = customer.Phone,
        Address = customer.Address,
        CreatedAt = customer.CreatedAt
    };
    }
}