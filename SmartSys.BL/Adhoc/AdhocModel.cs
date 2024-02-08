using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.Adhoc
{
   public  class AdhocModel
    {
       [DisplayName("Adhoc Id")]
       public int AdhocRunId { get; set; }
       public Int16 UserId { get; set; }
       [DisplayName("User")]
       public string UserName { get; set; }
       [DisplayName("Report Id")]
       public string ReportId { get; set; }
       [DisplayName("RGS Job Id")]
       public Int32 RGSJobId { get; set; }
       public string OutputLocation { get; set;}
       public DateTime RunDate { get; set; }
       public Int16 StatusId { get; set; }
       [DisplayName("Report Name")]
       public string ReportName { get; set; }
       [DisplayName("Status")]
       public string StatusShortCode { get; set; }
       [DisplayName("Gen Id")]
       public Int16 GenId { get; set; }
        public string FYearId { get; set; }
        public string FYear { get; set; }
        public List<AdhocModel> FYearDrpDwn { get; set; }

        public string Description { get; set; }
       public string Key { get; set; }
       public List<AdhocModel> listKey { get; set; }
       public List<AdhocParameterModel> ParamList { get; set; }
       public string ClientName { get; set; }
    }
    public class RGSReportNameModel
    {
        public string ReportId { get; set; }
        public string ReportName { get; set; }
    }

    public class paramMadal
{
    public List<paramMadal> lstAdhocParam { get; set; }
    public string ParamName { get; set; }
    public string DisplayName { get; set; }
    public string ParamValue { get; set; }
    public string FunctionRef { get; set; }
}
    public class AdhocParameterModel
    {
        public List<paramMadal> lstAdhocParam { get; set; }
        public string ParamName { get; set; }
        public string DisplayName { get; set; }
        public string ParamValue { get; set; }
        public string DataType { get; set; }
        public string ParamId { get; set; }
        public string ReportId { get; set; }
        public string FunctionRef { get; set; }
        public string TxtParamValue { get; set; }
    }
    public class ClientEmpCode
    {
        public string  Emp_No { get; set; }
        public string  Name { get; set; }
    }
    public class ClientCustCode
    {
        public string Cust_No { get; set; }
        public string Cust_Name { get; set; }
    }
}
