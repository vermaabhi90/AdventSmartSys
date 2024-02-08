using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.EmailProcess
{
   public class EmailModel
    {
        public string  MailID { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool Processed { get; set; }
        public Int32 StatusId { get; set; }
    }
}
