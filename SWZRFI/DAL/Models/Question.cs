using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWZRFI.DAL.Models
{
    public class Question
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Contents { get; set; }
        [Required]
        public int QuestionnaireId { get; set; }
        public Questionnaire Questionnaire { get; set; }
        public List<Answer> Answers { get; set; }

    }
}
