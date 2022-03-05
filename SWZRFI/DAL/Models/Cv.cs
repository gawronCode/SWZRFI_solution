using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Models
{
    public class Cv
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Proszę podać imię")]
        [Display(Name = "Imię")]
        [MaxLength(1024)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Proszę podać nazwisko")]
        [Display(Name = "Nazwisko")]
        [MaxLength(1024)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Proszę podać numer telefonu")]
        [Display(Name = "Telefon")]
        [MaxLength(1024)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Proszę podać umiejętności")]
        [Display(Name = "Umiejętności")]
        [MaxLength(1024)]
        public string Skills { get; set; }
        [Display(Name = "Doświadczenie zawodowe")]
        [MaxLength(1024)]
        public string Expirience { get; set; }
        [Display(Name = "Dodatkowe umiejętności")]
        [MaxLength(1024)]
        public string AdditionalSkills { get; set; }
        [Display(Name = "Znajomość języków")]
        [MaxLength(1024)]
        public string Languages { get; set; }
        [Display(Name = "Zwetnętrzne linki")]
        [MaxLength(1024)]
        public string OthersWebsites { get; set; }
        [Display(Name = "O mnie")]
        public string AdditionalInformation { get; set; }


    }
}
