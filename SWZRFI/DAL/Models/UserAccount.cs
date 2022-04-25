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


        public Conversation Conversation { get; set; }
        public int? ConversationId { get; set; }


        public List<UserConversation> UserConversations { get; set; }

        public List<Conversation> Conversations { get; set; }
        public List<Message> Messages { get; set; }



        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public int? CvId { get; set; }
        public Cv Cv { get; set; }

    }
}
