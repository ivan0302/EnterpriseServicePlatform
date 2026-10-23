
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ServicePlatform.Api.Data;
using ServicePlatform.Api.Tests.Controllers;

namespace ServicePlatform.Api.Tests.Factory;

public class CustomWebApplicationFactory
    : WebApplicationFactory<Program>
{
    // Generate the database name once per factory instance.
    private readonly string _databaseName =
        $"CustomerApiTests_{Guid.NewGuid()}";

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<
                IDbContextOptionsConfiguration<ServicePlatformDbContext>>();

            services.RemoveAll<
                DbContextOptions<ServicePlatformDbContext>>();

            services.RemoveAll<ServicePlatformDbContext>();

            services.AddDbContext<ServicePlatformDbContext>(
                options => options.UseInMemoryDatabase(_databaseName));

            services
                .AddControllers()
                .PartManager
                .ApplicationParts
                .Add(new AssemblyPart(
                    typeof(ExceptionTestController).Assembly));
        });
    }
}
