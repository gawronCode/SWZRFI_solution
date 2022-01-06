using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI.ViewServices.JobOffers;


namespace SWZRFI.Controllers
{
    public class JobOffersController : Controller
    {
        private readonly IJobOffersService _jobOffersService;


        public JobOffersController(IJobOffersService jobOffersService)
        {
            _jobOffersService = jobOffersService;
        }

        /// <summary>
        /// Wyświetlą listę aktualnych ofert pracy
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _jobOffersService.GetIndexPageData());
        }

        /// <summary>
        /// wyświetla szczegóły wybranej oferty
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> JobOfferDetails(int id)
        {
            return View(await _jobOffersService.GetJobOfferDetailsData(id));
        }



    }
}
