using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DTO
{
    public class CvViewer
    {
        public string UserId { get; set; }
        public int ApplicationId { get; set; }
        public int JobOfferId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Skills { get; set; }
        public IEnumerable<string> Expirience { get; set; }
        public IEnumerable<string> AdditionalSkills { get; set; }
        public IEnumerable<string> Languages { get; set; }
        public IEnumerable<string> OthersWebsites { get; set; }
        public IEnumerable<string> AdditionalInformation { get; set; }
    }
}
