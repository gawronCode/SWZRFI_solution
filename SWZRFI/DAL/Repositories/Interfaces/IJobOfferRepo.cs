using System.Collections.Generic;
using System.Threading.Tasks;
using SWZRFI.DAL.Models;

namespace SWZRFI.DAL.Repositories.Interfaces
{
    public interface IJobOfferRepo
    {
        Task CreateJobOfferAsync(JobOffer jobOffer);
        Task UpdateJobOfferAsync(JobOffer jobOffer);
        Task<IEnumerable<JobOffer>> GetAllJobOffers();
        Task<JobOffer> GetJobOfferByIdAsync(int id);
        Task<IEnumerable<JobOffer>> GetAllJobOffersByCompanyIdAsync(int companyId);
        Task RemoveJobOfferAsync(JobOffer jobOffer);
    }
}