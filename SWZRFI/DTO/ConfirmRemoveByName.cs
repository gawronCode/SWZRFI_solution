using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DTO
{
    public class ConfirmRemoveByName
    {

        public int ObjectId { get; set; }

        [Display(Name = "Aby potwierdzić akcję przepisz poniższą nazwę")]
        public string ObjectName { get; set; }

        [Display(Name = "Przepisz nazwę w celu potwierdzenia")]
        [Required(ErrorMessage = "Akcja wymagana w celu zatwierdzenia decyzji")]
        public string RepeatedName { get; set; }
    }
}
