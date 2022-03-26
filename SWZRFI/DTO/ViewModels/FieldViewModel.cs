using System.Collections.Generic;

namespace SWZRFI.DTO.ViewModels
{
    public class FieldViewModel
    {
        public int Count { get; set; }
        public string Question { get; set; }
        public int QuestionId { get; set; }
        public List<AnswerViewModel> Answers { get; set; }

    }
}
