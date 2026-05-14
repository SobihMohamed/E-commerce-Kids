using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Exceptions;
using E_commerce.Domain.Models.Category;
using E_commerce.Domain.Models.Lookup;
using E_commerce.Domain.Models.Product;
using E_commerce.Domain.Models.User;
using E_commerce.Shared.EnumsHelper.Product; // تأكد من مسار Enum الـ TargetGender
using E_commerce.Shared.EnumsHelper.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

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

        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            string adminEmail = config["AdminSettings:Email"];
            string adminPassword = config["AdminSettings:Password"];
            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            {
                throw new InvalidOperationException("Admin email or password is not configured in appsettings.json. Please provide valid credentials.");
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin",
                    UserType = UserType.Admin,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newAdmin, adminPassword);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(newAdmin, UserType.Admin.ToString());
            }
        }

        public static async Task SeedBaseGarmentsAsync(IUnitOfWork unitOfWork)
        {
            var productRepo = unitOfWork.GetRepository<ProductEntity, int>();
            var categoryRepo = unitOfWork.GetRepository<CategoryEntity, int>();
            var sizeRepo = unitOfWork.GetRepository<SizeEntity, int>();
            var colorRepo = unitOfWork.GetRepository<ColorEntity, int>();

            // ==========================================
            // 1. Seeding للأقسام (Categories)
            // ==========================================
            var allCategories = await categoryRepo.GetAllAsync();

            var boyCategory = allCategories.FirstOrDefault(c => c.Title == "Boys Customization");
            if (boyCategory == null)
            {
                boyCategory = new CategoryEntity { 
                    Title = "Boys Customization",
                    IsBaseGarment=true,
                    Description = "Base garments for boys customization", 
                    PictureUrl = "uploads/Customization/Boy.png"
                };
                await categoryRepo.AddAsync(boyCategory);
                await unitOfWork.SaveChangesAsync(); 
            }

            var girlCategory = allCategories.FirstOrDefault(c => c.Title == "Girls Customization");
            if (girlCategory == null)
            {
                girlCategory = new CategoryEntity
                {
                    Title = "Girls Customization",
                    IsBaseGarment = true,
                    Description = "Base garments for girls customization",
                    PictureUrl = "uploads/Customization/Girl.png"
                };
                await categoryRepo.AddAsync(girlCategory);
                await unitOfWork.SaveChangesAsync();
            }

            // ==========================================
            // 2. Seeding للمقاسات (Sizes)
            // ==========================================
            var sizesList = new List<string> { "4", "6", "8", "10", "12" };
            var existingSizes = await sizeRepo.GetAllAsync();

            foreach (var sizeName in sizesList)
            {
                if (!existingSizes.Any(s => s.Name == sizeName))
                {
                    await sizeRepo.AddAsync(new SizeEntity { Name = sizeName });
                }
            }
            await unitOfWork.SaveChangesAsync();

            // ==========================================
            // 3. Seeding للألوان (Colors)
            // ==========================================
            var colorsList = new List<(string Name, string HexCode)>
            {
                ("Black", "#000000"),
                ("White", "#FFFFFF"), 
                ("Baby Blue", "#7bafeb"),
                ("Purple", "#9A74A8"),
                ("Bright Yellow", "#fdda0e")
            };
            var existingColors = await colorRepo.GetAllAsync();

            foreach (var colorData in colorsList)
            {
                if (!existingColors.Any(c => c.Name == colorData.Name))
                {
                    await colorRepo.AddAsync(new ColorEntity { Name = colorData.Name, HexCode = colorData.HexCode });
                }
            }
            await unitOfWork.SaveChangesAsync();

            // ==========================================
            // 4. Seeding للمنتجات الأساسية (Base Garments)
            // ==========================================
            var allProducts = await productRepo.GetAllAsync();

            // التيشرت الولادي
            if (!allProducts.Any(p => p.IsBaseGarment && p.TargetGender == TargetGender.Boy))
            {
                await productRepo.AddAsync(new ProductEntity
                {
                    Name = "Boys Customizable Blank Tee",
                    Description = "The base garment used for the Boys Customizer Design Studio.",
                    MainImageUrl = "uploads/Customization/Boy.png",
                    Price = 10m,
                    CategoryId = boyCategory.Id,
                    TargetGender = TargetGender.Boy,
                    IsBaseGarment = true
                });
            }

            // التيشرت البناتي
            if (!allProducts.Any(p => p.IsBaseGarment && p.TargetGender == TargetGender.Girl))
            {
                await productRepo.AddAsync(new ProductEntity
                {
                    Name = "Girls Customizable Blank Tee",
                    Description = "The base garment used for the Girls Customizer Design Studio.",
                    Price = 10m,
                    MainImageUrl = "uploads/Customization/Girl.png",
                    CategoryId = girlCategory.Id,
                    TargetGender = TargetGender.Girl,
                    IsBaseGarment = true
                });
            }

            await unitOfWork.SaveChangesAsync();
        }
    }
}