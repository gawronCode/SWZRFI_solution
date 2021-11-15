using System.ComponentModel.DataAnnotations;

namespace SWZRFI.DAL.Models
{
    /// <summary>
    /// Klasa reprezentująca umiejętność wymaganą lub opcjonalną
    /// </summary>
    public class SkillRequirement
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [MaxLength(450)]
        public string Description { get; set; }
        [Required]
        public int Level { get; set; }
        [Required]
        public bool IsOptional { get; set; }
    }
}
