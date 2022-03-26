using System;

namespace SWZRFI.DTO.ViewModels
{
    public class ResultViewModel
    {
        public string QuestionnaireName { get; set; }
        public int NumberOfSolvedQuestionnaires { get; set; }
        public DateTime? StudyStart { get; set; }
        public DateTime? LastQuestionnaireDate { get; set; }
        public double[] AverageQuestionnaireScore { get; set; }
        public string PatientEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
    }
}
