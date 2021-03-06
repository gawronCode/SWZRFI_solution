using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SWZRFI.DAL.Contexts;
using SWZRFI.DAL.Models;
using SWZRFI.DAL.Repositories.Interfaces;

namespace SWZRFI.DAL.Repositories.Implementations
{
    public class CompanyRepo : ICompanyRepo
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public CompanyRepo(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }


        public async Task<int> CreateCompany(Company company)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
            return company.Id;
        }

        public async Task UpdateCompany(Company company)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            context.Companies.Update(company);

            await context.SaveChangesAsync();
        }



    }
}
