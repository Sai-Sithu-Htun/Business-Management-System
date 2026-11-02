using bms.Api.DTOs;
using bms.Api.Models;
using Microsoft.EntityFrameworkCore;
using bms.Api.Data;

namespace bms.Api.Services;
public class PaymentMethodService
{
    private readonly BmsDbContext _context;
    public PaymentMethodService(BmsDbContext context)
    {
        _context = context;
    }

    public async Task<List<PaymentMethodDto>> GetAllAsync()
    {
        return await _context.PaymentMethods
            .Select(pm => new PaymentMethodDto
            {
                PaymentMethodId = pm.PaymentMethodId,
                MethodName = pm.MethodName
            })
            .ToListAsync();
    }

    public async Task<PaymentMethodDto?> GetByIdAsync(int id)
    {
        return await _context.PaymentMethods
            .Where(pm => pm.PaymentMethodId == id)
            .Select(pm => new PaymentMethodDto
            {
                PaymentMethodId = pm.PaymentMethodId,
                MethodName = pm.MethodName
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PaymentMethodDto> CreateAsync(
        CreatePaymentMethodDto dto)
    {
        string methodName = dto.MethodName.Trim();

        bool exists = await _context.PaymentMethods
            .AnyAsync(pm => pm.MethodName == methodName);

        if (exists)
        {
            throw new InvalidOperationException(
                "Payment method already exists.");
        }

        var paymentMethod = new PaymentMethod
        {
            MethodName = methodName
        };

        _context.PaymentMethods.Add(paymentMethod);
        await _context.SaveChangesAsync();

        return new PaymentMethodDto
        {
            PaymentMethodId = paymentMethod.PaymentMethodId,
            MethodName = paymentMethod.MethodName
        };
    }

    public async Task<PaymentMethodDto?> UpdateAsync(
        int id, UpdatePaymentMethodDto dto)
    {
        var paymentMethod = await _context.PaymentMethods
            .FindAsync(id);

        if (paymentMethod == null)
            return null;

        string methodName = dto.MethodName.Trim();

        bool exists = await _context.PaymentMethods
            .AnyAsync(pm =>
                pm.MethodName == methodName &&
                pm.PaymentMethodId != id);

        if (exists)
        {
            throw new InvalidOperationException(
                "Payment method already exists.");
        }

        paymentMethod.MethodName = methodName;

        await _context.SaveChangesAsync();

        return new PaymentMethodDto
        {
            PaymentMethodId = paymentMethod.PaymentMethodId,
            MethodName = paymentMethod.MethodName
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var paymentMethod = await _context.PaymentMethods
            .FindAsync(id);

        if (paymentMethod == null)
            return false;

        _context.PaymentMethods.Remove(paymentMethod);
        await _context.SaveChangesAsync();

        return true;
    }
}