using System.ComponentModel.DataAnnotations;

namespace SWZRFI.DAL.Models
{
    public class UserAnswer
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int Value { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string UserEmail { get; set; }
        public int UserQuestionnaireAnswerId { get; set; }
        public UserQuestionnaireAnswer UserQuestionnaireAnswer { get; set; }

    }
}
