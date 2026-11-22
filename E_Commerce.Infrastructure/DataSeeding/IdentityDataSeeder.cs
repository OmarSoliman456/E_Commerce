using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities.Identity;
using E_Commerce.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeding
{
    public class IdentityDataSeeder(
        StoreIdentityDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        ) : IDataSeeder
    {
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pagenation = await context.Database.GetPendingMigrationsAsync(ct);
                if (pagenation == null)
                {
                    await context.Database.MigrateAsync(ct);
                    return;
                }
                if (!await roleManager.Roles.AnyAsync())
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!await userManager.Users.AnyAsync())
                {
                    var user = new ApplicationUser()
                    {
                        DisplayName = "admin",
                        UserName = "admin",
                        Email = "admin123@gmail.com",
                        PhoneNumber = "1234567890"
                    };
                    var result = await userManager.CreateAsync(user, "P@ssW0rd");
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(user, "Admin");

                }
            }
            catch(Exception ex)
            {
                
            }

        }
    }
}
