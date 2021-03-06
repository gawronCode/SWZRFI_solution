using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;

namespace SWZRFI.ConfigData
{
    public static class InitialDataSeed
    {

        public static async Task Seed(
            UserManager<UserAccount> userManager,
            RoleManager<IdentityRole> roleManager,
            IServiceScopeFactory serviceScopeFactory)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
            await SeedCompanies(serviceScopeFactory);
        }

        private static async Task SeedCompanies(IServiceScopeFactory serviceScopeFactory)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            var user = await context.UserAccount
                .FirstOrDefaultAsync(q => q.Email == "fakeEmail@log.and.change");

            var company = await context.Companies
                .FirstOrDefaultAsync(q => q.CorporationalEmail == "fakeEmail@log.and.change");

            if (user is null || company is not null)
                return;

            company = new Company
            {
                CorporationalEmail = "fakeEmail@log.and.change",
                Name = "Firma admina",
                Description = "Firma admina",
                EmailConfirmed = true,
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();

            user.CompanyId = company.Id;
            context.UserAccount.Update(user);
            await context.SaveChangesAsync();

        }


        private static async Task SeedUsers(UserManager<UserAccount> userManager)
        {
            if (userManager.FindByEmailAsync("Admin").Result != null) return;
            var user = new UserAccount
            {
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "fakeEmail@log.and.change",
                Email = "fakeEmail@log.and.change",
                EmailConfirmed = true,
            };
            var result = await userManager.CreateAsync(user, "R3pasal@S");
            if (!result.Succeeded) return; 

            await userManager.AddToRoleAsync(user, "SystemAdmin");
            await userManager.AddToRoleAsync(user, "PersonalAccount");
            await userManager.AddToRoleAsync(user, "RecruitersAccount");
            await userManager.AddToRoleAsync(user, "ManagerAccount");
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            await CreateRole(roleManager, "SystemAdmin");
            await CreateRole(roleManager, "PersonalAccount");
            await CreateRole(roleManager, "RecruitersAccount");
            await CreateRole(roleManager, "ManagerAccount");
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
