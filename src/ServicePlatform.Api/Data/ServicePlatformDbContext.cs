using Microsoft.EntityFrameworkCore;
using ServicePlatform.Api.Models;

namespace ServicePlatform.Api.Data;

public sealed class ServicePlatformDbContext(DbContextOptions<ServicePlatformDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
}