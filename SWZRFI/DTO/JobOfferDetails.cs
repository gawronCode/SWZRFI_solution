using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DTO
{
    public class JobOfferDetails
    {
        public int JobOfferId { get; set; }
        public string Title { get; set; }
        public string SkillLevel { get; set; }
        public IEnumerable<string> Description { get; set; }
        public IEnumerable<string> Requirements { get; set; }
        public IEnumerable<string> OptionalRequirements { get; set; }
        public IEnumerable<string> AdditionalBenefits { get; set; }
        public int LowerBoundSallary { get; set; }
        public int UpperBoundSallary { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string CompanyName { get; set; }
        public IEnumerable<string> CompanyDescription { get; set; }
        public string CompanyCountry { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyStreet { get; set; }
        public string CompanyHouseNumber { get; set; }
    }
}
