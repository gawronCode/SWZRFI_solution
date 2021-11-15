using System.ComponentModel.DataAnnotations;

namespace SWZRFI.DAL.Models
{
    /// <summary>
    /// Klasa reprezentująca relację wiele do wielu dla tabel Company oraz UserAccount
    /// </summary>
    public class CompanyUserAccount
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CompanyId { get; set; }
        public Company Company { get; set; }
        [Required]
        [MaxLength(450)]
        public string UserAccountId { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
