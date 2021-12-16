using System;
using System.Collections.Generic;
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
        [Required(ErrorMessage = "Proszę podać tytuł oferty")]
        [MinLength(6, ErrorMessage = "Minimalna długość nazwy oferty to 6 znaków")]
        [Display(Name = "Nazwa oferty")]
        [MaxLength(255)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Opis jest wymagany")]
        [Display(Name = "Opis oferty")]
        [MaxLength(450)]
        public string Description { get; set; }
        public DateTime? CreationDate { get; set; }
        [Display(Name = "Data wygaśnięcia oferty)")]
        public DateTime? ExpirationDate { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [Display(Name = "Czy aktywna?")]
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Oferta musi mieć zdefiniowane widełki płacowe")]
        [Display(Name = "Dolne widełki płacowe")]
        [Column(TypeName = "decimal(16, 2")]
        public decimal LowerBoundSallary { get; set; }
        [Required(ErrorMessage = "Oferta musi mieć zdefiniowane widełki płacowe")]
        [Display(Name = "Górne widełki płacowe")]
        [Column(TypeName = "decimal(16, 2")]
        public decimal UpperBoundSallary { get; set; }

        
        public string UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<SkillRequirement> SkillRequirements { get; set; }
        public ICollection<Location> Locations { get; set; }
        
    }
}
