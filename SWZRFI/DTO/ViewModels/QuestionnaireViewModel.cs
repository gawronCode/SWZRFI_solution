using System.Collections.Generic;

namespace SWZRFI.DTO.ViewModels
{
    public class QuestionnaireViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FieldViewModel> Fields { get; set; }
    }
}
