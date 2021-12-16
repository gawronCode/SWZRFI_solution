using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DTO.BindingModels
{
    public class JobOfferVm
    {
        public JobOfferB JobOfferB { get; set; }
        public IEnumerable<SkillRequirementB> SkillRequirementBs { get; set; }
        public IEnumerable<LocationB> LocationBs { get; set; }
    }
}
