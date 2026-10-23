
using System.Net;
using System.Text.Json;
using ServicePlatform.Api.Tests.Factory;

namespace ServicePlatform.Api.Tests.Integration;

public sealed class GlobalExceptionHandlerTests(
    CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task UnhandledException_ShouldReturnSafe500ProblemDetails()
    {
        // Arrange
        using var client = factory.CreateClient();

        // Act
        using var response = await client.GetAsync(
            "/_test/exceptions/throw");

        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(
            HttpStatusCode.InternalServerError,
            response.StatusCode);

        using var document = JsonDocument.Parse(responseBody);
        var root = document.RootElement;

        Assert.Equal(
            500,
            root.GetProperty("status").GetInt32());

        Assert.True(root.TryGetProperty("traceId", out var traceId));
        Assert.False(string.IsNullOrWhiteSpace(traceId.GetString()));

        Assert.DoesNotContain(
            "TEST-ONLY-SECRET",
            responseBody,
            StringComparison.OrdinalIgnoreCase);
    }
}
