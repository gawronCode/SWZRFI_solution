using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace SWZRFI.DAL.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MaxLength(255)]
        public string CorporationalEmail { get; set; }
        public bool EmailConfirmed { get; set; }
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
