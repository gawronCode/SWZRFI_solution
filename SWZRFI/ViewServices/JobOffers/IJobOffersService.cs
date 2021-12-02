using System.Collections.Generic;
using System.Threading.Tasks;
using SWZRFI.DAL.Models;

namespace SWZRFI.ViewServices.JobOffers
{
    public interface IJobOffersService
    {
        Task<IEnumerable<JobOffer>> GetIndexPageData();
        Task<JobOffer> GetJobOfferDetailsData(int id);
    }
}