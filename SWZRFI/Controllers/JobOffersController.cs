using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SWZRFI.ControllersServices.JobOffers;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI.DTO.DtoMappingUtils;

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
            var offer = await _jobOffersService.GetIndexPageData();
            return View(offer);
        }

        /// <summary>
        /// wyświetla szczegóły wybranej oferty
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> JobOfferDetails(int id)
        {
            var offer = await _jobOffersService.GetJobOfferDetailsData(id);
            return View(offer.MapToDto());
        }



    }
}
