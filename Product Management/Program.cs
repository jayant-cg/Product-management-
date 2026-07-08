using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Product_Management.Data;
using Product_Management.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Product Service for Dependency Injection
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Product Management API",
        Description = "ASP.NET Core Web API for managing products with CRUD operations and search functionality",
        Contact = new OpenApiContact
        {
            Name = "Product Management Team",
            Email = "support@productmanagement.com"
        }
    });

    // Enable XML comments if available
    options.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable CORS
app.UseCors("AllowAll");

// Enable Swagger in all environments for testing
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Management API v1");
    options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    options.DocumentTitle = "Product Management API Documentation";
});

// Commented out for easier testing - uncomment for production
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
