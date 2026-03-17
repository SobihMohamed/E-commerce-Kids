
using E_commerce.Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using E_commerce.Persistence.ProgramServices;
using E_commerce.Web.Middleware;
using E_commerce.Services.AutoMapper;
using E_commerce.Web.Extensions;
namespace E_commerce.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // get from presistence layer (database configure) 
            builder.Services.InjectDatabaseService(builder.Configuration);
            // get from identity layer in web project (Identity core configure)
            builder.Services.InjectIdentityCore();
            // inject all application services 
            builder.Services.AddApplicationServices(builder.Configuration);
            // get from web layer (rate limiting configure)
            builder.Services.InjectRateLimiting();
            // get from services layer
            builder.Services.InjectAutoMapperService();
            
            // add identity core
            builder.Services.AddDataProtection();

            
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseMiddleware<GlobalErrorHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
