using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.RGS
{
    public class InstanceManagerModel
    {
        [Display(Name = "IM Id")]
        public Int16 IMId { get; set; }

         [Display(Name = "Host")]
        public string Host { get; set; }

         [Display(Name = "Port")]
        public string Port { get; set; }

         [Display(Name = "Primary")]
        public bool isPrimary { get; set; }

         [Display(Name = "Polling Interval")]
        public int PollingInterval { get; set; }

         [Display(Name = "Process Id")]
        public int ProcessId { get; set; }
        public Int16 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int16 ModifiedBy { get; set; }

         [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
        public string CreatedByName { get; set; }

         [Display(Name = "Modified By")]
        public string ModifiedByName { get; set; }
        public Int16 StatusId { get; set; }

         [Display(Name = "Status")]
        public string StatusName { get; set; }
    }
}
