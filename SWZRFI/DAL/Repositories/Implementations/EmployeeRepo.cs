using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI.DAL.Utils;

namespace SWZRFI.DAL.Repositories.Implementations
{
    public class EmployeeRepo : IEmployeeRepo
    {

        private readonly IDbConnectionChecker _dbConnectionChecker;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public EmployeeRepo(IServiceScopeFactory serviceScopeFactory, IDbConnectionChecker dbConnectionChecker)
        {
            _dbConnectionChecker = dbConnectionChecker;
            _serviceScopeFactory = serviceScopeFactory;
        }



        public async Task CreateCorporationalInvitation(CorporationalInvitation invitation)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            await context.CorporationalInvitations.AddAsync(invitation);
            await context.SaveChangesAsync();
        }

        public async Task<bool> ValidateAgainstEmailAndGuid(string email, Guid guid)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            var invitation = await context.CorporationalInvitations
                .FirstOrDefaultAsync(q 
                    => q.Guid == guid &&
                       q.ExpirationDate.CompareTo(DateTime.Now) >= 0);

            if (invitation == null || invitation.Used)
                return false;

            invitation.Used = true;
            context.CorporationalInvitations.Update(invitation);
            await context.SaveChangesAsync();

            return string.Equals(invitation.Email, email, StringComparison.CurrentCultureIgnoreCase);
        }


    }
}
