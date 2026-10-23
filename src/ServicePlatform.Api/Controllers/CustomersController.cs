
using Microsoft.AspNetCore.Mvc;
using ServicePlatform.Api.DTOs;
using ServicePlatform.Api.Services;

namespace ServicePlatform.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CustomersController(ICustomerService customerService)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CustomerResponse>>> GetCustomers(
        CancellationToken cancellationToken)
    {
        var customers = await customerService.GetCustomersAsync(
            cancellationToken);

        return Ok(customers);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> CreateCustomer(
        [FromBody] CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var customer = await customerService.CreateCustomerAsync(
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetCustomer),
            new { id = customer.Id },
            customer);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerResponse>> GetCustomer(
        int id,
        CancellationToken cancellationToken)
    {
        var customer = await customerService.GetCustomerByIdAsync(
            id,
            cancellationToken);

        if (customer is null)
        {
            return NotFound();
        }

        return Ok(customer);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateCustomer(
        int id,
        [FromBody] CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var updated = await customerService.UpdateCustomerAsync(
            id,
            request,
            cancellationToken);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCustomer(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await customerService.DeleteCustomerAsync(
            id,
            cancellationToken);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
