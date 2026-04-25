using E_commerce.Domain.Models.User;
using E_commerce.Shared.EnumsHelper.User;
using Microsoft.AspNetCore.Identity;
namespace E_commerce.Persistence.Seeds
{
    public static class SeederAsync
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetNames(typeof(UserType)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            string adminEmail = "admin@softbridge.com";

            // ensure existence of admin
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail.ToUpper(),
                    Email = adminEmail,
                    FullName = "System Admin",
                    UserType = UserType.Admin,
                    EmailConfirmed = true
                };

                // create admin
                var result = await userManager.CreateAsync(newAdmin, "Admin@123456");

                // if created 
                if (result.Succeeded)
                {

                    await userManager.AddToRoleAsync(newAdmin, UserType.Admin.ToString());
                }
            }
        }

    }
}
