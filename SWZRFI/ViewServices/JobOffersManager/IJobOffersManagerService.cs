using System.Collections.Generic;
using System.Threading.Tasks;
using SWZRFI.DAL.Models;

namespace SWZRFI.ViewServices.JobOffersManager
{
    public interface IJobOffersManagerService
    {
        Task<IEnumerable<JobOffer>> GetIndexPageData(string email);
        Task CreateJobOffer(string email, JobOffer jobOffer);
    }
}