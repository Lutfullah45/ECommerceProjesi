using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ECommerceProjesi.WebUI.IdentitySeed
{
    public static class SeedIdentity
    {
        public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                string adminRole = "Admin";
                if (!await roleManager.RoleExistsAsync(adminRole))
                {
                    Debug.WriteLine($"'{adminRole}' rolü oluşturuluyor...");
                    await roleManager.CreateAsync(new IdentityRole(adminRole));
                }

                string adminUserEmail = configuration["AdminUser:Email"];
                string adminUserPassword = configuration["AdminUser:Password"];

                if (string.IsNullOrEmpty(adminUserEmail) || string.IsNullOrEmpty(adminUserPassword))
                {
                    Debug.WriteLine("### HATA: appsettings.json dosyasında AdminUser Email veya Password bulunamadı! ###");
                    return; 
                }

                IdentityUser? adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    Debug.WriteLine($"Admin kullanıcısı ({adminUserEmail}) oluşturuluyor...");
                    adminUser = new IdentityUser
                    {
                        UserName = adminUserEmail,
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };

                    IdentityResult result = await userManager.CreateAsync(adminUser, adminUserPassword);

                    if (result.Succeeded)
                    {
                        Debug.WriteLine($"Admin kullanıcısı başarıyla oluşturuldu.");
                        await userManager.AddToRoleAsync(adminUser, adminRole);
                        Debug.WriteLine($"Admin kullanıcısı '{adminRole}' rolüne atandı.");
                    }
                    else
                    {
                        Debug.WriteLine($"### Admin kullanıcısı oluşturulamadı! Hatalar: ###");
                        foreach (var error in result.Errors)
                        {
                            Debug.WriteLine($"- {error.Description}");
                        }
                    }
                }
                else
                {
                    Debug.WriteLine($"Admin kullanıcısı ({adminUserEmail}) zaten mevcut.");
                    if (!await userManager.IsInRoleAsync(adminUser, adminRole))
                    {
                        Debug.WriteLine($"Admin kullanıcısı '{adminRole}' rolünde değil, role atanıyor...");
                        await userManager.AddToRoleAsync(adminUser, adminRole);
                    }
                }
            }
        }
    }
}