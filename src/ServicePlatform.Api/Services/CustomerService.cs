using Microsoft.EntityFrameworkCore;
using ServicePlatform.Api.Data;
using ServicePlatform.Api.DTOs;
using ServicePlatform.Api.Models;

namespace ServicePlatform.Api.Services;

public sealed class CustomerService(ServicePlatformDbContext dbContext) : ICustomerService
{
    public async Task<List<CustomerResponse>> GetCustomersAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Customers
            .AsNoTracking()
            .OrderBy(customer => customer.Name)
            .Select(customer => new CustomerResponse(
                customer.Id,
                customer.Name,
                customer.Email,
                customer.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }

    public async Task<CustomerResponse?> GetCustomerByIdAsync(int id, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
        .AsNoTracking()
        .FirstOrDefaultAsync(
            customer => customer.Id == id,
            cancellationToken);

        if (customer is null)
        {
            return null;
        }

        return new CustomerResponse(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.CreatedAtUtc);
    }

    public async Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        };

        dbContext.Customers.Add(customer);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new CustomerResponse(
            customer.Id,
            customer.Name,
            customer.Email,
            customer.CreatedAtUtc);
    }

    public async Task<bool> UpdateCustomerAsync(int id, CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
            .FirstOrDefaultAsync(
                customer => customer.Id == id,
                cancellationToken);
        if (customer == null)
        {
            return false;
        }

        customer.Name = request.Name.Trim();
        customer.Email = request.Email.Trim();

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteCustomerAsync(int id, CancellationToken cancellationToken)
    {
         var customer = await dbContext.Customers
            .FirstOrDefaultAsync(
                customer => customer.Id == id,
                cancellationToken);
        if (customer == null)
        {
            return false;
        }

        dbContext.Customers.Remove(customer);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}