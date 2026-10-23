using Microsoft.EntityFrameworkCore;
using ServicePlatform.Api.Data;
using ServicePlatform.Api.Services;
using ServicePlatform.Api.Exceptions;
using ServicePlatform.Api.Middleware;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Register Entity Framework Core and SQL Server.
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<ServicePlatformDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<ServicePlatformDbContext>(
        name: "database");

builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(resource =>
        resource.AddService("EnterpriseServicePlatform.Api"))
    .WithTracing(tracing =>
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation()
            .AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri("http://localhost:4317");
                options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
            }));

// builder.Services
//     .AddOpenTelemetry()
//     .ConfigureResource(resource =>
//         resource.AddService("EnterpriseServicePlatform.Api"))
//     .WithTracing(tracing =>
//         tracing
//             .AddAspNetCoreInstrumentation()
//             .AddHttpClientInstrumentation()
//             .AddOtlpExporter(options =>
//             {
//                 options.Endpoint = new Uri("http://localhost:4317");
//                 options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
//             }));

var app = builder.Build();

app.MapHealthChecks("/health");

// Log each HTTP request, including handled failures.
app.UseMiddleware<RequestLoggingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // 2. Levanta la interfaz gráfica de Swagger
    app.UseSwaggerUI(options =>
    {
        // Apunta al JSON generado por MapOpenApi
        options.SwaggerEndpoint("/openapi/v1.json", "v1"); 
        
        // Saca a Swagger de la subruta '/swagger' y lo adueña de la raíz principal '/'
        options.RoutePrefix = string.Empty; 
    });
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }