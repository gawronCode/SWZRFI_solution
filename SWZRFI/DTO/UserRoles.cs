using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SWZRFI.DAL.Models;

namespace SWZRFI.DTO
{
    public class UserRoles
    {
        public IEnumerable<IdentityRole> IdentityRoles { get; set; }
        public UserAccount UserAccount { get; set; }
    }
}
