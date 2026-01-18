using Microsoft.AspNetCore.Identity;
using ProjeAyuDeneme.Models;

namespace ProjeAyuDeneme.Data
{
    public static class SeedData
    {
        public static async Task Initialize(
            IServiceProvider serviceProvider,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // --- 1. ROLLERİ OLUŞTURMA ---
            string[] roleNames = { "Admin", "User", "Yonetici" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Rol yoksa oluştur
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // --- 2. ADMİN KULLANICISINI OLUŞTURMA VE ROL ATAMA ---

            // Kontrol: admin@sinav.com kullanıcısı zaten var mı?
            var adminUser = await userManager.FindByEmailAsync("volkan@gmail.com");

            if (adminUser == null)
            {
                // Kullanıcı yoksa oluştur
                adminUser = new ApplicationUser
                {
                    UserName = "volkan@gmail.com",
                    Email = "volkan@gmail.com",
                    EmailConfirmed = true,
                    AdSoyad = "Volkan Akgul",
                    Birim = "Yönetim",     // Yeni alan
                    Pozisyon = "Proje Yöneticisi" // Yeni alan
                };

                // Şifrenizi BURAYA YAZIN
                await userManager.CreateAsync(adminUser, "123456");
            }

            // Rol Atama: Kullanıcı Admin rolünde değilse ata
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}