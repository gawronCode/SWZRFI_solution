using SWZRFI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DTO
{
    public class CandidateApplication
    {
        public UserAccount UserAccount { get; set; }
        public Application Application { get; set; }
        public bool QuestionnaireInvitation { get; set; }
        public bool SolvedQuestionnaire { get; set; }
        public bool InvitedForReview { get; set; }
        public Cv Cv { get; set; }
    }
}
