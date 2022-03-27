using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Models
{
    public class QuestionnaireAccess
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int QuestionnaireId { get; set; }
        public bool Expired { get; set; }
    }
}
