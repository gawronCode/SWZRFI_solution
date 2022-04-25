using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Models
{
    public class UserConversation
    {
        [Key]
        public int Id { get; set; }
        public Conversation Conversation { get; set; }
        public int ConversationId { get; set; }
        public UserAccount UserAccount { get; set; }
        public string UserAccountId { get; set; }
    }
}
