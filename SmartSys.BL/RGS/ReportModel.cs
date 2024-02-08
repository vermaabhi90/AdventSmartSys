using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSys.DAL;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace SmartSys.BL
{
    public class Report
    {
        [Display(Name ="Report Id")]
        public string ReportId { get; set; }
        public int BaseReportId { get; set; }

        [Display(Name = "Report Name")]
        public string ReportName { get; set; }
        public int BusinessLineId { get; set; }

        [Display(Name = "Business Line Name")]
        public string BusinessLineName { get; set; }

        [Display(Name = "Report Type")]
        public string ReportType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name ="Modified Date")]
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "Version")]
        public int Version { get; set; }
        public string FileName { get; set; }
        public string BaseRptModifiedBy { get; set; }
        public DateTime BaseRptModifiedDate { get; set; }
        public byte[] FileBinary { get; set; }
        public bool isActive { get; set; }
        public List<BaseReportDBObj> BaseDBObjects { get; set; }
        public List<ReportParameter> ReportParams { get; set; }
    }

    public class StoredProcedureList
    {
        public string objName { get; set; }
        public string Title { get; set; }
        public List<string> SPName { get; set; }
    }

    public class BaseReportDBObj
    {
        public int BaseObjId { get; set; }
        public string ObjName { get; set; }
        public int BaseReportId { get; set; }
        public string SPName { get; set; }
        public string ServerName { get; set; }
        public string DBName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public string ConnectionType { get; set; }
        public int ConnectionId { get; set; }
    }

    public class ReportParameter
    {
        public int BaseObjId { get; set; }
        public string ReportId { get; set; }
        public string ParamName { get; set; }
        public string LableName { get; set; }
        public string DataType { get; set; }
        public string DefaultValue { get; set; }
        public string ParamAlias { get; set; }
        public bool isMandatory { get; set; }
        public bool isLinked { get; set; }
        public string ObjName { get; set; }
        public string BaseDefaultValue { get; set; }
    }

    public class BusinessLine
    {
        public int BusinessLineId { get; set; }
        public string BusinessLineName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class ReportType
    {
        public string ReportTypeID { get; set; }
        public string ReportTypeName { get; set; }
        public string Description { get; set; }
    }
    public class RGSGenModel
    {
        [Display(Name = "Gen Id")]
        public Int16 GenId { get; set; }

         [Display(Name = "Gen Type")]
        public string GenType { get; set; }

         [Display(Name = "Host")]
        public string Host { get; set; }

         [Display(Name = "Port")]
        public string Port { get; set; }

         [Display(Name = "Polling Interval")]
        public Int16 PollingInterval { get; set; }

         [Display(Name = "Process Id")]
        public Int32 ProcessId { get; set; }

         [Display(Name = "is Active")]
        public bool isActive { get; set; }
        public Int16 BusinessLineId { get; set; }

         [Display(Name = "Business Line Name")]
        public string BusinessLineName { get; set; }
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

        [Display(Name="Action")]
        public string GenAction { get; set; }
        public List<BusinessLine> bussList { get; set; }
    }
}
