using System;
using System.ComponentModel.DataAnnotations;


namespace SWZRFI.DTO.BindingModels
{
    public class JobOfferB
    {
        [Required(ErrorMessage = "Proszę podać tytuł oferty")]
        [Display(Name = "Nazwa oferty")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Opis jest wymagany")]
        [Display(Name = "Opis oferty")]
        public string Description { get; set; }
        [Display(Name = "Data wygaśnięcia oferty)")]
        public DateTime? ExpirationDate { get; set; }
        [Required(ErrorMessage = "Pole wymagane")]
        [Display(Name = "Czy aktywna?")]
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Oferta musi mieć zdefiniowane widełki płacowe")]
        [Display(Name = "Dolne widełki płacowe")]
        public decimal LowerBoundSallary { get; set; }
        [Required(ErrorMessage = "Oferta musi mieć zdefiniowane widełki płacowe")]
        [Display(Name = "Górne widełki płacowe")]
        public decimal UpperBoundSallary { get; set; }


    }
}
