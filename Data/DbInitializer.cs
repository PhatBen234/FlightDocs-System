using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FlightDocs_System.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = new string[]
            {
                UserClasses.Role_Admin,
                UserClasses.Role_Owner,
                UserClasses.Role_GOStaff,
                UserClasses.Role_Pilot,
                UserClasses.Role_Stewardess
            };

            foreach (var role in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Áp dụng các Migration nếu chưa có
            await context.Database.MigrateAsync();

            // Seed dữ liệu Role
            await SeedRoles(roleManager);
        }
    }
}
