using System.ComponentModel.DataAnnotations;

namespace SWZRFI.DAL.Models
{
    public class UserQuestionnaire
    {
        [Key]
        public int Id { get; set; }

        public string PatientEmail { get; set; }

        public int QuestionnaireId { get; set; }

    }
}
