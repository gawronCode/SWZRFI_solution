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
    public class JobOfferRepo : IJobOfferRepo
    {
        private readonly IDbConnectionChecker _dbConnectionChecker;
        private readonly IServiceScopeFactory _serviceScopeFactory;


        public JobOfferRepo(IServiceScopeFactory serviceScopeFactory, IDbConnectionChecker dbConnectionChecker)
        {
            _dbConnectionChecker = dbConnectionChecker;
            _serviceScopeFactory = serviceScopeFactory;
        }

        

        public async Task CreateJobOfferAsync(JobOffer jobOffer)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            await context.JobOffers.AddAsync(jobOffer);
            await context.SaveChangesAsync();
        }

        public async Task UpdateJobOfferAsync(JobOffer jobOffer)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            context.JobOffers.Update(jobOffer);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<JobOffer>> GetAllJobOffers()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            return await context.JobOffers.AsSplitQuery().Include(q => q.JobOfferSkillRequirements)
                .Include(q => q.JobOfferLocations).ToListAsync();
        }

        public async Task<JobOffer> GetJobOfferByIdAsync(int id)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            return await context.JobOffers.AsSplitQuery().Include(q => q.JobOfferSkillRequirements)
                .Include(q => q.JobOfferLocations)
                .FirstOrDefaultAsync(q => q.Id == id);
        }


        public async Task<IEnumerable<JobOffer>> GetAllJobOffersByCompanyIdAsync(int companyId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            await using var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            return await context.JobOffers.Where(q => q.CompanyId == companyId)
                .AsSplitQuery().Include(q => q.JobOfferSkillRequirements)
                .Include(q => q.JobOfferLocations).ToListAsync();
        }

    }
}
