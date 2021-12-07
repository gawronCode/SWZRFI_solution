using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWZRFI.DAL.Repositories.Interfaces;
using SWZRFI.DTO.BindingModels;

namespace SWZRFI.Views.JobOffersManager
{
    public class IndexModel : PageModel
    {
        private readonly IJobOfferRepo _jobOfferRepo;

        public IndexModel(IJobOfferRepo jobOfferRepo)
        {
            _jobOfferRepo = jobOfferRepo;
        }

        [BindProperty]
        public JobOfferB InputModel { get; set; }


    }
}
