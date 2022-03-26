using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWZRFI.DAL.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
        public Conversation Conversation { get; set; }
        public string Content { get; set; }
        public DateTime SendDate { get; set; }
    }
}
