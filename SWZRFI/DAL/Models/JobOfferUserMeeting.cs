using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Models
{
    public class JobOfferUserMeeting
    {
        public int Id { get; set; }
        public UserAccount UserAccount { get; set; }
        public string UserAccountId { get; set; }
        public JobOffer JobOffer { get; set; }
        public int JobOfferId { get; set; }
        public Application Application { get; set; }
        public int ApplicationId { get; set; }
        [Required(ErrorMessage = "Proszę podać datę spotkania")]
        [Display(Name = "Planowana data spotkania")]
        public DateTime MeetingDateTime { get; set; }
        public bool IsMeetingConfirmed { get; set; }
    }
}
