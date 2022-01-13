using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace SWZRFI.DAL.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Proszę podać nazwę")]
        [MaxLength(255)]
        [MinLength(5, ErrorMessage = "Minimalna długość nazwy oferty to 5 znaków")]
        [Display(Name = "Nazwa Firmy")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Proszę podać email")]
        [MaxLength(255)]
        [Display(Name = "Email firmowy")]
        public string CorporationalEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
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

        public ICollection<UserAccount> UserAccounts { get; set; }
        public ICollection<JobOffer> JobOffers { get; set; }
    }
}
