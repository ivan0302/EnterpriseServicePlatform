using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicePlatform.Api.Data;
using ServicePlatform.Api.Models;
using ServicePlatform.Api.DTOs;

namespace ServicePlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CustomersController(ServicePlatformDbContext dbContext) : ControllerBase
{
    
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetCustomers(CancellationToken cancellationToken)
    {
        var customers = await dbContext.Customers
        .AsNoTracking()
        .OrderBy(c => c.Name)
        .Select(c => new CustomerResponse(c.Id, c.Name, c.Email, c.CreatedAtUtc))
        .ToListAsync(cancellationToken);
        return Ok(customers);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> CreateCustomer([FromBody] CreateCustomerRequest request
    , CancellationToken cancellationToken)
    {
        var customer = new Customer
        {
            Name = request.Name,
            Email = request.Email,
            CreatedAtUtc = DateTime.UtcNow
        };
        dbContext.Customers.Add(customer);

        await dbContext.SaveChangesAsync(cancellationToken);
        
        var response = new CustomerResponse(customer.Id, customer.Name, customer.Email, customer.CreatedAtUtc);

        return CreatedAtAction(nameof(GetCustomers), new { id = customer.Id }, response);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomer(int id, CancellationToken cancellationToken)
    {
        var customer = await dbContext.Customers
        .AsNoTracking()
        .Where(c => c.Id == id)
        .Select(c => new CustomerResponse(c.Id, c.Name, c.Email, c.CreatedAtUtc))   
        .FirstOrDefaultAsync(cancellationToken);
        if (customer == null)
        {
            return NotFound();
        }
        return Ok(customer);
    }
}