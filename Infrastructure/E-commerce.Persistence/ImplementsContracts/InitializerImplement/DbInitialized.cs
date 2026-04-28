using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.DbInitializer;
using E_commerce.Domain.Models.User;
using E_commerce.Persistence.E_commerceDbContext;
using E_commerce.Persistence.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Persistence.Implements.InitializerImplement
{
    public class DbInitialized(IUnitOfWork unitOfWork, EcommerceDbContext ecommerceDbContext , UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager) : IDbInitializer
    {
        public async Task DataSeedAsync()
        {
            try
            {
                var pendingMigrations = await ecommerceDbContext.Database.GetPendingMigrationsAsync();
                if (pendingMigrations != null && pendingMigrations.Any())
                    await ecommerceDbContext.Database.MigrateAsync();
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                throw;
            }
            await SeederAsync.SeedRolesAsync(roleManager);
            await SeederAsync.SeedAdminUserAsync(userManager);
            await SeederAsync.SeedBaseGarmentAsync(unitOfWork);
        }
    }
}