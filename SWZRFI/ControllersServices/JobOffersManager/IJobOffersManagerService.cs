using System.Collections.Generic;
using System.Threading.Tasks;
using SWZRFI.DAL.Models;

namespace SWZRFI.ControllersServices.JobOffersManager
{
    public interface IJobOffersManagerService
    {
        Task<IEnumerable<JobOffer>> GetIndexPageData(string email);
        Task CreateJobOffer(string email, JobOffer jobOffer);
        Task<JobOffer> GetJobOfferForEdition(int jobOfferId);
        Task SaveEditedJobOffer(string email, JobOffer jobOffer);
        Task<bool> RemoveJobOffer(string email, int jobOfferId);
    }
}