using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Country { get; set; }
        [Required]
        [MaxLength(255)]
        public string PostCode { get; set; }
        [Required]
        [MaxLength(255)]
        public string City { get; set; }
        [Required]
        [MaxLength(255)]
        public string Street { get; set; }
        [Required]
        [MaxLength(255)]
        public string HouseNumber { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public int JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; }

        //https://positionstack.com/documentation TODO - zrobić geokodowanie i pokazywanie mapki
    }
}
