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
        [Required(ErrorMessage = "Proszę podać tytuł oferty")]
        [MinLength(6, ErrorMessage = "Minimalna długość nazwy oferty to 6 znaków")]
        [Display(Name = "Nazwa oferty")]
        [MaxLength(255)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Proszę wskazać poziom poszukiwanego specjalisty")]
        public int SkillLevel { get; set; }
        [Required(ErrorMessage = "Opis jest wymagany")]
        [Display(Name = "Opis oferty")]
        [MaxLength(1024)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Oferta musi zawierać wymagania")]
        [Display(Name = "Wymagania")]
        [MaxLength(1024)]
        public string Requirements { get; set; }
        [Display(Name = "Umiejętności mile widziane")]
        [MaxLength(1024)]
        public string OptionalRequirements { get; set; }
        [Display(Name = "Dodatkowe korzyści")]
        [MaxLength(1024)]
        public string AdditionalBenefits { get; set; }
        [Display(Name = "Data utworzenia")]
        public DateTime? CreationDate { get; set; }
        [Display(Name = "Data modyfikacji")]
        public DateTime? LastModified { get; set; }
        [Display(Name = "Data wygaśnięcia oferty)")]
        public DateTime? ExpirationDate { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [Display(Name = "Czy aktywna?")]
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Oferta musi mieć zdefiniowane widełki płacowe")]
        [Display(Name = "Dolne widełki płacowe")]
        public int LowerBoundSallary { get; set; }
        [Required(ErrorMessage = "Oferta musi mieć zdefiniowane widełki płacowe")]
        [Display(Name = "Górne widełki płacowe")]
        public int UpperBoundSallary { get; set; }

        [Display(Name = "Kraj")]
        [MaxLength(1024)]
        public string Country { get; set; }
        [Display(Name = "Miasto")]
        [MaxLength(1024)]
        public string City { get; set; }
        [Display(Name = "Ulica")]
        [MaxLength(1024)]
        public string Street { get; set; }
        [Display(Name = "numer budynku")]
        [MaxLength(1024)]
        public string HouseNumber { get; set; }

        [ForeignKey("CreatorUserAccount"), Column(Order = 0)]
        public string CreatorUserAccountId { get; set; }
        public UserAccount CreatorUserAccount { get; set; }

        [ForeignKey("EditorUserAccount"), Column(Order = 1)]
        public string EditorUserAccountId { get; set; }
        public UserAccount EditorUserAccount { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }


        
    }
}
