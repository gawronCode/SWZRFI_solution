using System.ComponentModel.DataAnnotations;


namespace SWZRFI.DTO.BindingModels
{
    public class SkillRequirementB
    {
        [Required(ErrorMessage = "Proszę podać nazwę tej umiejętności")]
        [Display(Name = "Nazwa umiejętności")]
        public string Name { get; set; }
        [Display(Name = "Opis umiejętności")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Określenie poziomu jest wymagane")]
        [Display(Name = "Wymagany poziom w skali 1-10")]
        public int Level { get; set; }
        [Display(Name = "Umiejętność mile widzialna (nieobowiązkowa)")]
        public bool IsOptional { get; set; }

    }
}
