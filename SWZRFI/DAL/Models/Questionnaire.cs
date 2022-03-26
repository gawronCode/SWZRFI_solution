using System;
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

    }
}
