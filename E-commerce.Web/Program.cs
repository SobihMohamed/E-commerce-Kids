using E_commerce.Domain.Models.User;
using E_commerce.Persistence.Extensions;
using E_commerce.Persistence.ProgramServices;
using E_commerce.Presentation.Extentions;
using E_commerce.Services.AutoMapper;
using E_commerce.Web.Extensions;
using E_commerce.Web.Hubs.Notification;
using E_commerce.Web.Middleware;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// =======================================================
// 1. Add Services to the container (DI Configuration)
// =======================================================

// Database & Identity
builder.Services.InjectDatabaseService(builder.Configuration);
builder.Services.InjectIdentityCore();

// Application Logic & External Services
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.InjectAutoMapperService();

// Security & Limits
builder.Services.AddDataProtection();
builder.Services.InjectRateLimiting();

// Add JWT Authentication and CORS configuration
builder.Services.AddJwtAuthentication(builder.Configuration, builder.Environment);
builder.Services.AddCustomCors(builder.Configuration);

// API Documentation (OpenAPI & Scalar)
builder.Services.AddOpenApiDocumentation();

// Controllers & JSON Options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Convert enums to strings in JSON responses for better readability
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddSignalR();



// =======================================================
// 2. Build the Application
// =======================================================
var app = builder.Build();

// Seed Database (Top-level statements natively support 'await')
await app.SeedDatabaseAsync();


// =======================================================
// 3. Configure the HTTP Request Pipeline (Middlewares)
// =======================================================

app.UseMiddleware<GlobalErrorHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarDocumentation();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

// Security Middlewares (Must be in this specific order)
app.UseCors("CorsPolicy"); 
app.UseAuthentication();
app.UseAuthorization();

// Rate Limiting
app.UseRateLimiter();

// Map Endpoints
app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");// notification hub endpoint for SignalR

// Run the application
app.Run();