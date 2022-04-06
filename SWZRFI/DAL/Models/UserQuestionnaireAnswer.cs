using System;
using System.Collections.Generic;

namespace SWZRFI.DAL.Models
{
    public class UserQuestionnaireAnswer
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public int QuestionnaireId { get; set; }
        public Questionnaire Questionnaire { get; set; }
        public DateTime? AnswerDate { get; set; }
        public List<UserAnswer> UserAnswers { get; set; }
    }
}
