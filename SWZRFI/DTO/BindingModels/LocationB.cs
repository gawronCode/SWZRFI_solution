using System.ComponentModel.DataAnnotations;


namespace SWZRFI.DTO.BindingModels
{
    public class LocationB
    {
        [Required(ErrorMessage = "Proszę podać nazwę państwa")]
        [Display(Name = "Kraj")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Proszę podać kod pocztowy")]
        [Display(Name = "Kod pocztowy")]
        public string PostCode { get; set; }
        [Required(ErrorMessage = "Proszę podać nazwę miejscowości")]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        [Required(ErrorMessage = "Proszę podać nazwę ulicy")]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [Required(ErrorMessage = "Proszę podać nr. buynku/lokal")]
        [Display(Name = "Budynek/lokal")]
        public string HouseNumber { get; set; }
    }
}
