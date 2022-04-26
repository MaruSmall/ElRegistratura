using ElRegistratura.Models;
using Microsoft.AspNetCore.Identity;

using System.Linq;
using System.Threading.Tasks;

namespace ElRegistratura.Data
{
    public class SeedData
    {
        public static async Task SeedRolesAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(EnumRole.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(EnumRole.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(EnumRole.Basic.ToString()));
        }
        public static async Task SeedSuperAdminAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new User
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                FirstName = "Марина",
                LastName = "Грицаник",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, EnumRole.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, EnumRole.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, EnumRole.SuperAdmin.ToString());
                }

            }
        }
    }
}
