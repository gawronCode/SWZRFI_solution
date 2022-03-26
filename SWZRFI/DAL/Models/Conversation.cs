using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Models
{
    public class Conversation
    {
        public int Id { get; set; }
        public string FirstInterOcutorId { get; set; }
        public string SecondInterOcutorId { get; set; }
        public string Title { get; set; }
        public ICollection<Message> Messages { get; set; }

    }
}
