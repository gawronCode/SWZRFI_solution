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
        public int EmployeeCount { get; set; }
        public string Description { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }

        public ICollection<UserAccount> UserAccounts { get; set; }
        public ICollection<JobOffer> JobOffers { get; set; }
    }
}
