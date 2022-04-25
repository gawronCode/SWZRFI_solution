using SWZRFI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DTO
{
    public class RecruitmentMeeting
    {
        public JobOffer JobOffer { get; set; }
        public JobOfferUserMeeting JobOfferApplication { get; set; }
        public UserAccount Candidate { get; set; }
        public UserAccount Recruiter { get; set; }
    }
}
