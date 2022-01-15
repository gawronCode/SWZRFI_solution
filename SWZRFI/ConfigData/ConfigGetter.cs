using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SWZRFI.ConfigData.Models;

namespace SWZRFI.ConfigData
{
    public class ConfigGetter : IConfigGetter
    {

        private readonly IConfiguration _configuration;

        public ConfigGetter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IdentityEmailOptions GetIdentityEmail()
        {
            return _configuration.GetSection(IdentityEmailOptions.IdentityEmail).Get<IdentityEmailOptions>();
        }



    }
}
