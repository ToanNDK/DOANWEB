using DoAnWeb2024.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DoAnWeb2024.Repository
{
    public static class SeedData
    {
        public static void SeedingData(DataContext _context, IServiceProvider serviceProvider)
        {
            _context.Database.Migrate();

            if (!_context.Products.Any())
            {
                // Tạo dữ liệu mẫu cho Category và Brand
                CategoryModel mb = new CategoryModel { Name = "Điện thoại", Slug = "xiaomi", Description = "Mobile", Status = 1 };
                CategoryModel hp = new CategoryModel { Name = "Tai nghe", Slug = "headphone", Description = "Headphone", Status = 0 };

                BrandModel xm = new BrandModel { Name = "Điện thoại Xiaomi", Slug = "xiaomi", Description = "Mobile", Status = 1 };
                BrandModel apple = new BrandModel { Name = "Airpod", Slug = "headphone", Description = "Headphone by Apple", Status = 0 };

                _context.Products.AddRange(
                    new ProductModel { Name = "Airpod Pro Max", Slug = "Apple", Description = "Tai nghe Bluetooth", Image = "AirpodProMax.jpg", Category = hp, Brand = apple, Price = 1234 },
                    new ProductModel { Name = "Xiaomi ", Slug = "xiaomi", Description = "Điện thoại Xiaomi", Image = "Xiaomi.jpg", Category = mb, Brand = xm, Price = 3500 }
                );
                _context.SaveChanges();
            }

            // Seed roles and admin user
            SeedRolesAndAdminUser(serviceProvider).Wait();
        }

        private static async Task SeedRolesAndAdminUser(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUserModel>>();

            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminUser = new AppUserModel
            {
                UserName = "admin",
                Email = "admin@example.com"
            };

            string adminPassword = "Admin@123";

            var user = await userManager.FindByEmailAsync(adminUser.Email);

            if (user == null)
            {
                var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
                if (createAdminUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
