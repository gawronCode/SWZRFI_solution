using System.Collections.Generic;

namespace SWZRFI.DTO.ViewModels
{
    public class PatientQuestionnaireManagerViewModel
    {
        public string PatientEmail { get; set; }
        public List<PatientQuestionnaireViewModel> PatientQuestionnaires { get; set; }
        public List<QuestionnaireViewModel> Questionnaires { get; set; }
    }
}
