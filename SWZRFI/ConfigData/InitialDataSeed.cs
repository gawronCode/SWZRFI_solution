using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SWZRFI.DAL.Models.IdentityModels;

namespace SWZRFI.ConfigData
{
    public static class InitialDataSeed
    {

        public static async Task Seed(
            UserManager<UserAccount> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        private static async Task SeedUsers(UserManager<UserAccount> userManager)
        {
            if (userManager.FindByEmailAsync("Admin").Result != null) return;
            var user = new UserAccount
            {
                FirstName = "Admin",
                LastName = "Admin",
                Email = "fakeEmail@log.and.change",
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, "sqlcmc!0");
            if (!result.Succeeded) return; 

            await userManager.AddToRoleAsync(user, "SystemAdmin");
            await userManager.AddToRoleAsync(user, "PersonalAccount");
            await userManager.AddToRoleAsync(user, "RecruitersAccount");
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await CreateRole(roleManager, "SystemAdmin");
            await CreateRole(roleManager, "PersonalAccount");
            await CreateRole(roleManager, "RecruitersAccount");
        }

        private static async Task CreateRole(
            RoleManager<IdentityRole> roleManager,
            string roleName)
        {
            if (roleManager.RoleExistsAsync(roleName).Result) return;
            await roleManager.CreateAsync(new IdentityRole
            {
                Name = roleName
            });
        }
    }
}
