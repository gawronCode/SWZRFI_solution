using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI.DAL.Utils;
using SWZRFI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Repositories.Implementations
{

    public class UserRepo : IUserRepo
    {
        private readonly IDbConnectionChecker _dbConnectionChecker;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UserRepo(IServiceScopeFactory serviceScopeFactory, IDbConnectionChecker dbConnectionChecker)
        {
            _dbConnectionChecker = dbConnectionChecker;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task DeleteUnverifiedAccountsAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            if (!await _dbConnectionChecker.ValidateDbConnection(context))
                throw new Exception("DB not responding");

            var inactiveAccounts =
                await context.UserAccount.Where(ua
                        => ((DateTime)ua.RegistrationDate).AddHours(24) < DateTime.Now &&
                           !ua.EmailConfirmed)
                    .ToListAsync();

            Console.WriteLine($"Liczba nieaktywnych kont = {inactiveAccounts.Count}");

            if(inactiveAccounts.Count == 0) return;
            
            context.UserAccount.RemoveRange(inactiveAccounts);
            await context.SaveChangesAsync();
        }

        public async Task<UserAccount> GetUserByEmailAsync(string email)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            return await context.UserAccount.AsSplitQuery()
                .Include(q => q.Company)
                .ThenInclude(q => q.JobOffers)
                .Include(q => q.Company)
                .ThenInclude(c => c.UserAccounts)
                .FirstOrDefaultAsync(q => q.Email == email);
        }

        public async Task UpdateUser(UserAccount userAccount)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            context.UserAccount.Update(userAccount);

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserRole>> GetUsersRolesForCompany(int companyId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            var userRoles = await context.UserRoles
                .AsNoTracking()
                .ToListAsync();

            var roles = await context.Roles
                .AsNoTracking()
                .ToListAsync();

            var users = await context.UserAccount
                .AsNoTracking()
                .Where(u => (int)u.CompanyId == companyId)
                .ToListAsync();

            var join = users.Join(userRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => new
                {
                    u = u,
                    ur = ur
                }).Join(roles,
                j => j.ur.RoleId,
                r => r.Id,
                (j, r) => new UserRole
                {
                    UserAccount = j.u,
                    IdentityRole = r
                }).ToList();

            return join;
        }


    }
}
