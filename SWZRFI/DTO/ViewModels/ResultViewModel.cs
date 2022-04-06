using SWZRFI.DTO.TmpModels;
using System;
using System.Collections.Generic;

namespace SWZRFI.DTO.ViewModels
{
    public class ResultViewModel
    {
        public string QuestionnaireName { get; set; }
        public int QuestionCount { get; set; }
        public int NumberOfSolvedQuestionnaires { get; set; }
        public DateTime? StudyStart { get; set; }
        public DateTime? LastQuestionnaireDate { get; set; }
        public List<QuestionnaireAnswerDetails> QuestionnaireAnswerDetails { get; set; }
        public int[] QuestionnmaireScore { get; set; }
        public string PatientEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
    }
}
