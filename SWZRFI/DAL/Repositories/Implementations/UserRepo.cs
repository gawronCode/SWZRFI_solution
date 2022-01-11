using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI.DAL.Utils;

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
                .FirstOrDefaultAsync(q => q.Email == email);
        }

        public async Task UpdateUser(UserAccount userAccount)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            context.UserAccount.Update(userAccount);

            await context.SaveChangesAsync();
        }


    }
}
