using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWZRFI_Utils.EmailHelper.Models
{
    public class EmailMessage
    {
        public IEnumerable<string> Recipients { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
