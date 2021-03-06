using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [InverseProperty(nameof(JobOffer.CreatorUserAccount))]
        public ICollection<JobOffer> CreatedJobOffers { get; set; }

        [InverseProperty(nameof(JobOffer.EditorUserAccount))]
        public ICollection<JobOffer> EditedJobOffers { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

    }
}
