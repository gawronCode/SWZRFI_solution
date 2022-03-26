using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Models
{
    public class Application
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int CvId { get; set; }
        public Cv Cv { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        public int JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; }
        public bool Opened { get; set; }
        public string UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
