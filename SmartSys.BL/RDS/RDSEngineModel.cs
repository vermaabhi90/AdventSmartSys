using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.RDS
{
   public class RDSEngineModel
    {
       [Display(Name = "RDS Engine Id")]
       public Int16 RDSEngineId{get;set;}

        [Display(Name = "Host")]
		public string  Host{get;set;}

        [Display(Name = "Port")]
		public string  Port{get;set;}

        [Display(Name = "Job Polling Interval")]
		public Int16 JobPollingInterval{get;set;}

        [Display(Name = "Process Polling Interval")]
		public Int16 ProcessPollingInterval{get;set;}

        [Display(Name = "Event Polling Intterval")]
		public Int16 EventPollingIntterval{get;set;}

        [Display(Name = "Trigger Polling Interval")]
		public Int16 TriggerPollingInterval{get;set;}

        [Display(Name = "Process Id")]

	    public int ProcessId{get;set;}

        [Display(Name = "is Active")]
		public Boolean isActive{get;set;}
		public Int16 StatusId{get;set;}
		public Int16 ModifiedBy{get;set;}

        [Display(Name = "Modified Date")]
		public DateTime ModifiedDate{get;set;}

        [Display(Name = "Modified By")]
        public string ModifiedByName { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }
    }
}
