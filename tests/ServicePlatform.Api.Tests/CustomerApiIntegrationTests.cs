using System.Net;
using ServicePlatform.Api.Tests.Factory;
using System.Net.Http.Json;
using ServicePlatform.Api.DTOs;

namespace ServicePlatform.Api.Tests;

public class CustomerApiIntegrationTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CustomerApiIntegrationTests(
        CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCustomers_ShouldReturn200Ok()
    {
        // Act
        var response = await _client.GetAsync("/api/Customers");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturn201AndPersistCustomer()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            Name = "Integration Test Customer",
            Email = "integration.test@example.com"
        };

        // Act: create a customer through HTTP.
        var response = await _client.PostAsJsonAsync(
            "/api/customers",
            request);

        // Assert: the API should return 201 Created.
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdCustomer =
            await response.Content.ReadFromJsonAsync<CustomerResponse>();

        Assert.NotNull(createdCustomer);
        Assert.True(createdCustomer.Id > 0);
        Assert.Equal(request.Name, createdCustomer.Name);
        Assert.Equal(request.Email, createdCustomer.Email);

        // Assert: Location should point to the newly created customer.
        Assert.NotNull(response.Headers.Location);

        // Assert: the Location header should point to the new customer.
        Assert.True(
        string.Equals(
            $"/api/customers/{createdCustomer.Id}",
            response.Headers.Location?.AbsolutePath,
            StringComparison.OrdinalIgnoreCase),
        $"Expected Location path for customer {createdCustomer.Id}, " +
        $"but received '{response.Headers.Location?.AbsolutePath}'.");

        // Act: retrieve the newly created customer.
        var getResponse = await _client.GetAsync(
            $"/api/customers/{createdCustomer.Id}");

        // Assert: the customer should be persisted and retrievable.
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var retrievedCustomer =
            await getResponse.Content.ReadFromJsonAsync<CustomerResponse>();

        Assert.NotNull(retrievedCustomer);
        Assert.Equal(createdCustomer.Id, retrievedCustomer.Id);
        Assert.Equal(request.Name, retrievedCustomer.Name);
    }

    [Fact]
    public async Task CreateCustomer_WithInvalidEmail_ShouldReturn400BadRequest()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            Name = "Invalid Customer",
            Email = "not-an-email"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/Customers",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

}
