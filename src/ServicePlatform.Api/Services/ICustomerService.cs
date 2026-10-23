using ServicePlatform.Api.DTOs;

namespace ServicePlatform.Api.Services;

public interface ICustomerService
{
    Task<List<CustomerResponse>> GetCustomersAsync(CancellationToken cancellationToken);
    Task<CustomerResponse?> GetCustomerByIdAsync(int id, CancellationToken cancellationToken);
    Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request,
                                                CancellationToken cancellationToken);
    Task<bool> UpdateCustomerAsync(int id, CreateCustomerRequest request,
                                 CancellationToken cancellationToken);
    Task<bool> DeleteCustomerAsync(int id, CancellationToken cancellationToken);
}