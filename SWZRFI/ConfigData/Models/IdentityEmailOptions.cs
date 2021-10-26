using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.ConfigData.Models
{
    public class IdentityEmailOptions
    {
        public const string IdentityEmail = "IdentityEmail";
        public string Email { get; set; }
        public string Password { get; set; }
        public string Smtp { get; set; }
        public int Port { get; set; }
    }
}
