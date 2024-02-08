using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.RDS
{
    public class RDSReportModel
    {
        [Display(Name = "Report Id")]
        public string ReportId { get; set; }

        [Display(Name = "Report Name")]
        public string ReportName { get; set; }

        [Display(Name = "Report Type")]
        public string ReportType { get; set; }

        [Display(Name = "Business Line Id")]
        public Int16 BusinessLineId { get; set; }
        public string BusinessLineName { get; set; }
        public Int16 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByName { get; set; }
        public Int16 ModifiedBy { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedByName { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
   
        public List<RDSReportEvent> lstrdsReportEvent { get; set; }
        public List<RDSReportDep> lstReportDep { get; set; }
        public List<RDSTriggerDep> lstTriggerDep { get; set; }

    }
       public class RDSReportEvent
       {
           public Int16 EventId { get; set; }
           public string EventName { get; set; }
         
       }
    public class RDSReportTrigger
    {
        public Int16 TriggerId { get; set; }
        public string TriggerName { get; set; }     
    }
    public class RDSTriggerDep
    {
        public Int16 TriggerId { get; set; }
        public String  TriggerName { get; set; }
        public string ReportId { get; set; }
        public Int16 CreatedBy{get; set;}
        public DateTime CreatedDate { get; set; }
        public string ModifiedByName { get; set; }
    }
    
    public class RDSReportDep
    {
        public Int16 EventId { get; set; }
        public string EventName { get; set; }
        public string ReportId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedByName { get; set; }
    }
   
}
