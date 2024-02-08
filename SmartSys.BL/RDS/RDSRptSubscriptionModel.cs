using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.RDS
{

    public class RDSReport
    {
        public string ReportId { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public int BusinessLineId { get; set; }
        public string BusinessLineName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
       
    }

    public class RDSReportParam
    {
        public int RptSubId { get; set; }
        public string ReportId { get; set; }
        public string ParamName { get; set; }
        public string DefaultValue { get; set; }
        public int FunctionRef { get; set; }
        public string DataType { get; set; }
    }
  public  class RDSRptGenSubscription
    {
      [Display(Name = "Rpt Sub Id")]
      public int RptSubId { get; set; }
      public RDSClientForSub SelectedClient { get; set; }

       [Display(Name = "Rpt Sub Id")]
      public int ClientId { get; set; }

       [Display(Name = "Client Name")]
      public string ClientName { get; set; }

       [Display(Name = "Report Id")]
      public string ReportId { get; set; }

       [Display(Name = "Report Name")]
      public string ReportName { get; set; }
      public int CreatedBy { get; set; }

       [Display(Name = "Created By")]
      public string CreatedByName { get; set; }

       [Display(Name = "Created Date")]
      public DateTime CreatedDate { get; set; }
      public int ModifiedBy { get; set; }

       [Display(Name = "Modified By")]
      public string ModifiedByName { get; set; }

        [Display(Name = "Modified Date")]
      public DateTime ModifiedDate { get; set; }

      [Display(Name="E-mail")]
        public string ClientEmail { get; set; }   
    }
   
    public class RDSRptSubDistrDetails
    {
        public int RptSubId { get; set; }
        public string DistributionType { get; set; }
        //public DateTime DistributionTime { get; set; }
        //public DateTime MaxDistributionTime { get; set; }

        public string DistributionTime { get; set; }
        public string MaxDistributionTime { get; set; }
        public string DistributionMode { get; set; }
        public string LocalFolder { get; set; }
        public Boolean Email { get; set; }
        public Boolean FTP { get; set; }
    }
    public class RDSRptSubGenDetails
    {
        public int RptSubId { get; set; }
        public string Frequency { get; set; }
        public string WeeklyOptions { get; set; }
        //public DateTime StartTime { get; set; }
        //public DateTime EndTime { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Interval { get; set; }        
    }

    public class RDSRptSubGenTimeDetails
    {
        public int RptSubId { get; set; }
        public DateTime GenTime { get; set; }
        public DateTime GenId { get; set; }
    }
    public class RDSClientForSub
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientType { get; set; }
        public string ClientRefId { get; set; }
        public string email { get; set; }
        public string FTPDetails { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class RDSRptSubModel
    {

       public Boolean Monday { get; set; }
       public Boolean TuesDay { get; set; }
       public Boolean Wednesday { get; set; }
       public Boolean Thirsday { get; set; }
       public Boolean Friday { get; set; }
       public Boolean Saturday { get; set; }
       public Boolean Sunday { get; set; }
        public int RptSubId { get; set; }
        public int DlyGenSubId { get; set; }
        public string GenMode { get; set; }
        public string DistMode { get; set; }
        public string DistType { get; set; }
        public string ReportId { get; set; }
        public int ClientId { get; set; }
        public int UserID { get; set; }
        public List<RDSReport> RDSReportList { get; set; }
        public List<RDSClientForSub> ClientList { get; set; }
        public List<RDSReport> SelectedReportList { get; set; }
        public List<RDSReportParam> RDSRptParamList { get; set; }
       
        public RDSRptSubGenDetails RptSubGenrationDetails { get; set; }
        //public List<RDSRptSubGenTimeDetails> RDSRptSubGenTimeDetailsList { get; set; }

        public List<string> RDSRptSubGenTimeList { get; set; }
     
        public RDSRptSubDistrDetails RptSubDistributionDetails { get; set; }
        public List<RDSRptGenSubscription> RDSRptGenSubscription { get; set; }
   
    }
    public class DailyReportBookModel
    {
        [DisplayName("Daily Id")]
        public int DlyGenSubId { get; set; }
         [DisplayName("Report Id")]
        public string ReportId { get; set; }
         [DisplayName("Report Name")]
        public string ReportName { get; set; }
         [DisplayName("Status")]
        public string StatusCode { get; set; }
         [DisplayName("Gen Id")]
        public Int16 GenId { get; set; }
         [DisplayName("RGS Job Id")]
        public int RGSJobId { get; set; }
         [DisplayName("Output File Name")]
        public string OutputFileName { get; set; }
         [DisplayName("Run Date Time")]
        public DateTime RunDateTime { get; set; }
        public string StatusId { get; set; }
        public string ParamName { get; set; }
        public string ParamValue { get; set; }
        public string Description { get; set; }
        public string  ClientName { get; set; }
    }
}
