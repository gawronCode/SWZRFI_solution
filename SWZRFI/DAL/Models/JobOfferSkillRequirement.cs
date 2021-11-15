using System.ComponentModel.DataAnnotations;

namespace SWZRFI.DAL.Models
{
    /// <summary>
    /// Klasa reprezentująca relację wielu do wielu dla tabel JobOffer oraz SkillRequirement
    /// </summary>
    public class JobOfferSkillRequirement
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; }
        [Required]
        public int SkillRequirementId { get; set; }
        public SkillRequirement SkillRequirement { get; set; }
    }
}
