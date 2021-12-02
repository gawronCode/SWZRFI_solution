using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWZRFI.DAL.Models;
using SWZRFI.DAL.Repositories.Interfaces;

namespace SWZRFI.ViewServices.JobOffers
{
    public class JobOffersService : IJobOffersService
    {
        private readonly IJobOfferRepo _jobOfferRepo;

        public JobOffersService(IJobOfferRepo jobOfferRepo)
        {
            _jobOfferRepo = jobOfferRepo;
        }

        public async Task<IEnumerable<JobOffer>> GetIndexPageData()
        {
            return await _jobOfferRepo.GetAllJobOffers();
        }

        public async Task<JobOffer> GetJobOfferDetailsData(int id)
        {
            return await _jobOfferRepo.GetJobOfferByIdAsync(id);
        }

    }
}
