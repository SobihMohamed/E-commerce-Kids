using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Domain.Models.Category;
using E_commerce.Domain.Models.Lookup;
using E_commerce.Domain.Models.Product;
using E_commerce.Domain.Models.User;
using E_commerce.Persistence.E_commerceDbContext;
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
        public static async Task SeedBaseGarmentAsync(IUnitOfWork unitOfWork)
        {
            // 1. استدعاء الـ Repositories اللي هنحتاجها
            var productRepo = unitOfWork.GetRepository<ProductEntity, int>();
            var categoryRepo = unitOfWork.GetRepository<CategoryEntity, int>();
            var sizeRepo = unitOfWork.GetRepository<SizeEntity, int>();
            var colorRepo = unitOfWork.GetRepository<ColorEntity, int>();
            var variantRepo = unitOfWork.GetRepository<ProductVariantEntity, int>();

            // 2. هل التيشرت الأساسي موجود؟
            var allProducts = await productRepo.GetAllAsync();
            var baseProduct = allProducts.FirstOrDefault(p => p.IsBaseGarment);

            if (baseProduct == null)
            {
                // محتاجين Category
                var allCategories = await categoryRepo.GetAllAsync();
                var category = allCategories.FirstOrDefault();

                if (category == null)
                {
                    category = new CategoryEntity
                    {
                        Title = "System Default",
                        Description = "Hidden category for system items"
                    };
                    await categoryRepo.AddAsync(category);
                    await unitOfWork.SaveChangesAsync(); // عشان ناخد الـ Id
                }

                // إنشاء التيشرت الأساسي
                baseProduct = new ProductEntity
                {
                    Name = "Radiant Kids Blank Tee",
                    Description = "The base garment used for the Customizer Design Studio.",
                    Price = 150.00m,
                    CategoryId = category.Id,
                    IsBaseGarment = true,
                    CreatedAt = DateTime.UtcNow
                };

                await productRepo.AddAsync(baseProduct);
                await unitOfWork.SaveChangesAsync();
            }

            // ==========================================
            // 💡 Seeding للمقاسات
            // ==========================================
            var sizesList = new List<string> { "XS", "S", "M", "L", "XL" };
            var dbSizes = new List<SizeEntity>();
            var existingSizes = await sizeRepo.GetAllAsync();

            foreach (var sizeName in sizesList)
            {
                var size = existingSizes.FirstOrDefault(s => s.Name == sizeName);
                if (size == null)
                {
                    size = new SizeEntity { Name = sizeName };
                    await sizeRepo.AddAsync(size);
                    await unitOfWork.SaveChangesAsync();
                }
                dbSizes.Add(size);
            }

            // ==========================================
            // 💡 Seeding للألوان
            // ==========================================
            var colorsList = new List<(string Name, string HexCode)>
            {
                ("White", "#FFFFFF"),
                ("Black", "#000000"),
                ("Navy Blue", "#000080"),
                ("Red", "#FF0000"),
                ("Yellow", "#FFFF00")
            };
            var dbColors = new List<ColorEntity>();
            var existingColors = await colorRepo.GetAllAsync();

            foreach (var colorData in colorsList)
            {
                var color = existingColors.FirstOrDefault(c => c.Name == colorData.Name);
                if (color == null)
                {
                    color = new ColorEntity { Name = colorData.Name, HexCode = colorData.HexCode };
                    await colorRepo.AddAsync(color);
                    await unitOfWork.SaveChangesAsync();
                }
                dbColors.Add(color);
            }

            // ==========================================
            // 💡 إنشاء الـ Variants المدمجة
            // ==========================================
            var allVariants = await variantRepo.GetAllAsync();
            var existingVariantsCount = allVariants.Count(v => v.ProductId == baseProduct.Id);

            if (existingVariantsCount == 0)
            {
                // هنستخدم AddAsync جوه اللوب ونسيف مرة واحدة في الآخر (أفضل للـ Performance)
                foreach (var color in dbColors)
                {
                    foreach (var size in dbSizes)
                    {
                        await variantRepo.AddAsync(new ProductVariantEntity
                        {
                            ProductId = baseProduct.Id,
                            ColorId = color.Id,
                            SizeId = size.Id,
                            StockQuantity = 1000
                        });
                    }
                }

                await unitOfWork.SaveChangesAsync();
            }
        }
    }
}
