using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SWZRFI.DAL.Models
{
    public class UserAccount : IdentityUser
    {

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public ICollection<JobOffer> JobOffers { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

    }
}
