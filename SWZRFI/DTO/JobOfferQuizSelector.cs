using SWZRFI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DTO
{
    public class JobOfferQuizSelector
    {
        public int JobOfferId { get; set; }
        public List<QuizSelected> Questionnaires { get; set; }
    }
}
