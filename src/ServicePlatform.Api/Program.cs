using Microsoft.EntityFrameworkCore;
using ServicePlatform.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Register Entity Framework Core and SQL Server.
var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<ServicePlatformDbContext>(options =>
    options.UseSqlServer(connectionString));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
