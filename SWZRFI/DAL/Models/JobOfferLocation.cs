using System.ComponentModel.DataAnnotations;

namespace SWZRFI.DAL.Models
{
    /// <summary>
    /// Klasa reprezentująca połączenie wiele do wielu dla tabel JobOffer oraz Location
    /// </summary>
    public class JobOfferLocation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; }
        [Required]
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}
