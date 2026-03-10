
using E_commerce.Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using E_commerce.Persistence.ProgramServices;
using E_commerce.Web.Middleware;
namespace E_commerce.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // get from presistence services 
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddDataProtection();
            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddDefaultTokenProviders();
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
