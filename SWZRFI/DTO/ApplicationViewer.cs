using SWZRFI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DTO
{
    public class ApplicationViewer
    {
        public CvViewer CvViewer { get; set; }
        public JobOffer JobOffer { get; set; }
        public UserQuestionnaireAnswer UserQuestionnaireAnswer { get; set; }
        public QuestionnaireAccess QuestionnaireAccess { get; set; }
    }
}
