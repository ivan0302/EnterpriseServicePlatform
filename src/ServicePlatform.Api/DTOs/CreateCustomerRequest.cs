using System.ComponentModel.DataAnnotations;

namespace ServicePlatform.Api.DTOs;

public sealed class CreateCustomerRequest
{
    [Required]
    [StringLength(150, MinimumLength = 2)]
    public required string Name { get; init; }

    [Required]
    [EmailAddress]
    [StringLength(254)]
    public required string Email { get; init; }
}