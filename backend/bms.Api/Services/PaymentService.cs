using bms.Api.Data;
using bms.Api.DTOs;
using bms.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace bms.Api.Services;

public class PaymentService
{
    private readonly BmsDbContext _context;
    public PaymentService(BmsDbContext context)
    {
        _context = context;
    }

    public async Task<List<PaymentDto>> GetAllAsync()
    {
        return await _context.Payments
            .AsNoTracking()
            .Select(p => new PaymentDto
            {
                PaymentId = p.PaymentId,
                ProjectId = p.ProjectId,
                PaymentMethodId = p.PaymentMethodId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                ReferenceNumber = p.ReferenceNumber,
                Notes = p.Notes,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    public async Task<PaymentDto?> GetByIdAsync(int id)
    {
        return await _context.Payments
            .AsNoTracking()
            .Where(p => p.PaymentId == id)
            .Select(p => new PaymentDto
            {
                PaymentId = p.PaymentId,
                ProjectId = p.ProjectId,
                PaymentMethodId = p.PaymentMethodId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                ReferenceNumber = p.ReferenceNumber,
                Notes = p.Notes,
                CreatedAt = p.CreatedAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
    {
        var projectExists = await _context.Projects
            .AnyAsync(p => p.ProjectId == dto.ProjectId);

        if (!projectExists)
            throw new InvalidOperationException("Project not found.");

        var payment = new Payment
        {
            ProjectId = dto.ProjectId,
            PaymentMethodId = dto.PaymentMethodId,
            Amount = dto.Amount,
            PaymentDate = dto.PaymentDate ?? DateTime.Now,
            ReferenceNumber = dto.ReferenceNumber,
            Notes = dto.Notes
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        return new PaymentDto
        {
            PaymentId = payment.PaymentId,
            ProjectId = payment.ProjectId,
            PaymentMethodId = payment.PaymentMethodId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            ReferenceNumber = payment.ReferenceNumber,
            Notes = payment.Notes,
            CreatedAt = payment.CreatedAt
        };
    }

    public async Task<PaymentDto?> UpdateAsync(
        int id,
        UpdatePaymentDto dto)
    {
        var payment = await _context.Payments
            .FirstOrDefaultAsync(p => p.PaymentId == id);

        if (payment == null)
            return null;

        var projectExists = await _context.Projects
            .AnyAsync(p => p.ProjectId == dto.ProjectId);

        if (!projectExists)
            throw new InvalidOperationException("Project not found.");

        payment.ProjectId = dto.ProjectId;
        payment.PaymentMethodId = dto.PaymentMethodId;
        payment.Amount = dto.Amount;
        payment.PaymentDate = dto.PaymentDate ?? payment.PaymentDate;
        payment.ReferenceNumber = dto.ReferenceNumber;
        payment.Notes = dto.Notes;

        await _context.SaveChangesAsync();

        return new PaymentDto
        {
            PaymentId = payment.PaymentId,
            ProjectId = payment.ProjectId,
            PaymentMethodId = payment.PaymentMethodId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            ReferenceNumber = payment.ReferenceNumber,
            Notes = payment.Notes,
            CreatedAt = payment.CreatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var payment = await _context.Payments
            .FirstOrDefaultAsync(p => p.PaymentId == id);

        if (payment == null)
            return false;

        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();

        return true;
    }
}