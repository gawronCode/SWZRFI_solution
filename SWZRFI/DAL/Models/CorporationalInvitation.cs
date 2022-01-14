using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Models
{
    public class CorporationalInvitation
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public Guid Guid { get; set; }
        [Required(ErrorMessage = "Proszę podać email adresata")]
        [EmailAddress(ErrorMessage = "Adres niepoprawny")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Ze względów bezpieczeństwa pole wymagane")]
        public DateTime ExpirationDate { get; set; }

        public bool Used { get; set; }
        public int AssignedRole { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }

    }
}
