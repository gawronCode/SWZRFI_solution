using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWZRFI.DAL.Models
{
    public class Questionnaire
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime? CreationDate { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public int? SolvedCounter { get; set; }

        public List<Question> Questions { get; set; }
        public List<UserQuestionnaireAnswer> UserQuestionnaireAnswers { get; set; }


    }
}
