using System.ComponentModel.DataAnnotations;
using ServicePlatform.Api.DTOs;

namespace ServicePlatform.Api.Tests;

public class CustomerTests
{
    [Fact]
    public void NewCustomer_ShouldInitializeCreatedAtUtc()
    {
        // Arrange & Act
        var customer = new ServicePlatform.Api.Models.Customer
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };

        // Assert
        Assert.NotEqual(default, customer.CreatedAtUtc);
        Assert.Equal(DateTimeKind.Utc, customer.CreatedAtUtc.Kind);
    }

    [Fact]
    public void CreateCustomerRequest_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };

        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            new ValidationContext(request),
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CreateCustomerRequest_WithInvalidEmail_ShouldFailValidation()
    {
        // Arrange
        var request = new CreateCustomerRequest
        {
            Name = "John Doe",
            Email = "not-an-email"
        };

        var validationResults = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(
            request,
            new ValidationContext(request),
            validationResults,
            validateAllProperties: true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(
            validationResults,
            result => result.MemberNames.Contains(nameof(CreateCustomerRequest.Email)));
    }
}
