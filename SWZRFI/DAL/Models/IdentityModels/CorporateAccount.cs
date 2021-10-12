using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SWZRFI.DAL.Models.IdentityModels
{
    public class CorporateAccount : IdentityUser
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfEmployees { get; set; }
        public DateTime? DateOfCreation { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }
    }
}
