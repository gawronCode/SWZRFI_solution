using SWZRFI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DTO
{
    public class JobOfferApplication
    {
        public JobOffer JobOffer { get; set; }
        public IEnumerable<CandidateApplication> CandidateApplications { get; set; }
    }
}
