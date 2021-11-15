using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWZRFI.DAL.Models
{
    /// <summary>
    /// Klasa reprezentująca ofertę pracy
    /// </summary>
    public class JobOffer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        [Required]
        [MaxLength(450)]
        public string Description { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        [Column(TypeName = "decimal(16, 2")]
        public decimal LowerBoundSallary { get; set; }
        [Required]
        [Column(TypeName = "decimal(16, 2")]
        public decimal UpperBoundSallary { get; set; }

        
        public string UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
