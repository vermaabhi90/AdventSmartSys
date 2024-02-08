using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.TMPlan
{
    public class TMPlanModel
    {
        public int PlanId { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StrStartDate { get; set; }
        public string StrEndDate { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public bool AllDay { get; set; }
        public bool Repete { get; set; }
        public char FrequencyType { get; set; }
        public string FrequencyTypeDetail { get; set; }
        public string WeekDays { get; set; }
        public int WeekNo { get; set; }
        public int MonthNo { get; set; }
        public string RecurrenceRule { get; set; }
        public bool Monday { get; set; }
        public bool TuesDay { get; set; }
        public bool Wednesday { get; set; }
        public bool Thirsday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public bool Never { get; set; }
        public DateTime OnDate { get; set; }
        public string WeekMonth { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string User_Id { get; set; }
        public int ApprovedBy { get; set; }
        public int ApprovedDate { get; set; }
        public int statusId { get; set; }
        public string status { get; set; }
        public string Remark { get; set; }
        public string OutputLocation { get; set; }
        public DateTime RunDate { get; set; }
        public string ReportId { get; set; }
        public string ParamId { get; set; }
        public string ParamName { get; set; }
        public string TxtParamValue { get; set; }
        public string hidText { get; set; }
        public int StatusId { get; set; }
        public string RunEmpName { get; set; }
        public string DocType { get; set; }
        public int DocId { get; set; }
        public int DocRefId { get; set; }
        public List<EmpPlanList> LstEmployee { get; set; }
    }
    public class EmpPlanList
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
    }
    public class EmpSubordinatesModel
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
    }
    public class ReportNameModel
    {
        public string OutputLocation { get; set; }
        public DateTime RunDate { get; set; }
        public string ReportId { get; set; }
        public string ParamId { get; set; }
        public string ParamName { get; set; }
        public string TxtParamValue { get; set; }
        public string hidText { get; set; }
        public int StatusId { get; set; }
        public string RunEmpName { get; set; }
    }

}
