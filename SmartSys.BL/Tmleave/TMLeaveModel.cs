using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.Tmleave
{
    public class TMLeaveModel
    {
        public int LeaveTypeId { get; set; }
        public string LeaveType { get; set; }
        public string Description { get; set; }
        public Boolean IsPaid { get; set; }
        public int YearlyQuota { get; set; }
        public Boolean CanLaps { get; set; }
        public int MaxLeaveCarryover { get; set; }
        public Boolean Enable { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<TMLeaveModel> lstLeave { get; set; }
    }
    public class TMLeaveDetailModel
    {
        public int LeaveId { get; set; }
        public int LeaveTypeId { get; set; }
        public int BalanceLeave { get; set; }
        public string LeaveTypeName { get; set; }
        public string LeaveCategory { get; set; }
        public int EmpId { get; set; }
        public string Employee { get; set; }
        public string ManagerName { get; set; }
        public int ManagerId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FromDateStr { get; set; }
        public string ToDateStr { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        public string ApprovedDatestr { get; set; }
        public string Remark { get; set; }
        public string ManagerRemark { get; set; }
        public int Status { get; set; }
        public int StatusId { get; set; }
        public string StatusShortCode { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDatestr { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedDatestr { get; set; }
        public List<TMLeaveDetailModel> TMLeaveList { get; set; }
        public List<StatusModel> StatusList { get; set; }
        public string LeaveLevel { get; set; }
        public List<LeaveDescription> LstLeaveDescription { get; set; }
    }

    public class LeaveDescription
    {
        public string LeaveType { get; set; }
        public double TotalLeave { get; set; }
        public double MaxLeaveCarryover { get; set; }
        public double BalanceLeave { get; set; }
        public double LastYearCarryover { get; set; }
        public double YearlyQuota { get; set; }
    }
    public class StatusModel
    {
        public int StatusId { get; set; }
        public string StatusShortCode { get; set; }
    }
    public class TMDateList
    {
        public int WeekNo { get; set; }
        public string Date { get; set; }
    }
    public class TimeSheetDlyEntryList
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TaskId { get; set; }
        public int Status { get; set; }
        public string ApproverRemark { get; set; }
        public string Remark { get; set; }
        public string TaskName { get; set; }
        public int DayCode { get; set; }
        public string Day { get; set; }
        public string MonTime { get; set; }
        public string TusTime { get; set; }
        public string WedTime { get; set; }
        public string ThusTime { get; set; }
        public string FriTime { get; set; }
        public string SatTime { get; set; }
        public string SunTime { get; set; }
        public string AllowBtn { get; set; }
        public DateTime FromTime { get; set; }
        public DateTime ToTime { get; set; }
        public int TimeSheetEntryId { get; set; }
        public int TimeSheetId { get; set; }
        public int WeekNo { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime StartDate { get; set; }
        public List<TMProjectList> TMProjectlist { get; set; }
        public List<TMTaskList> TMTasklist { get; set; }
        public List<TimeSheetDlyEntryList> timeSheetEntrylist { get; set; }
        public List<TMDateList> TMdatelist { get; set; }
    }
    public class TMGetSelectedModel
    {
        public int TimeSheetEntryId { get; set; }
        public int MOMId { get; set; }
        public int TimeSheetId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Remark { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public int DayCode { get; set; }
        public string Day { get; set; }
        public int WeekNo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime MonToTime { get; set; }
        public DateTime MonFromTime { get; set; }

        public DateTime TusToTime { get; set; }
        public DateTime TusFromTime { get; set; }

        public DateTime WedToTime { get; set; }
        public DateTime WedFromTime { get; set; }

        public DateTime ThusToTime { get; set; }
        public DateTime ThusFromTime { get; set; }

        public DateTime FriToTime { get; set; }
        public DateTime FriFromTime { get; set; }

        public DateTime SatToTime { get; set; }
        public DateTime SatFromTime { get; set; }

        public DateTime SunToTime { get; set; }
        public DateTime SunFromTime { get; set; }
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public string Check { get; set; }
        public DateTime ToTime { get; set; }
        public DateTime FromTime { get; set; }
        public List<DayTimeLstModel> lstDayTime { get; set; }
    }

    public class DayTimeLstModel
    {
        public int TimeSheetEntryDetailId { get; set; }
        public int TimeSheetEntryId { get; set; }
        public int TimeSheetId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public int DayCode { get; set; }
        public string Day { get; set; }
        public DateTime ToTime { get; set; }
        public DateTime FromTime { get; set; }
        public string strToTime { get; set; }
        public string strFromTime { get; set; }
        public string Remark { get; set; }
        public string CustomerName { get; set; }
        public string VendorName { get; set; }
        public string MOMTitle { get; set; }
        public int VendorId { get; set; }
        public int CustomerId { get; set; }
        public int MOMId { get; set; }
        public DateTime StartDate { get; set; }

    }
    public class TimesheetHistory
    {
        public int TimeSheetId { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int WeekNo { get; set; }
        public int Status { get; set; }
        public string StatusCode { get; set; }
        public string ApproverRemark { get; set; }
        public string ApproverBy { get; set; }
        public string Remark { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        public string ModifiedBy { get; set; }
        public double WorkingHour { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<TimesheetHistory> LstHistory { get; set; }
    }
    public class TMProjectList
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
    public class TMMOMList
    {
        public int MoMId { get; set; }
        public string Title { get; set; }
    }
    public class TMTaskList
    {
        public int TaskId { get; set; }
        public string TaskName { get; set; }
    }
    public class ProjectCategoryModel
    {
        public int ProjCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<ProjectCategoryModel> ProjCateList { get; set; }
        public List<TMProjectList> ProjectList { get; set; }
    }
    public class ProjectTravelRequestModel
    {
        public int RequestId { get; set; }
        public string Description { get; set; }
        public int EmpId { get; set; }
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public int ApproverId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        public string ApproverRemark { get; set; }
        public string Status { get; set; }
        public string PostComment { get; set; }
        public string StatusName { get; set; }
        public string CustomerName { get; set; }
        public string PurposeofVisitDocs { get; set; }
        public string VendorName { get; set; }
        public string EmployeeName { get; set; }
        public string ApproverBy { get; set; }
        public string ReviewedBy { get; set; }
        public Nullable<DateTime> ReviewedDate { get; set; }
        public string ReviewdRemark { get; set; }
        public int ParticipantId { get; set; }
        public string ParticipantType { get; set; }
        public string Name { get; set; }
        public List<ProjectTravelRequestModel> LstTravelRequestParticipant { get; set; }
        public List<TravelBudgetModel> LstTravelBudget { get; set; }
        public List<ProjectTravelRequestModel> Employeest { get; set; }

    }
    public class TravelBudgetModel
    {
        public int RequestId { get; set; }
        public string BudgetTitle { get; set; }
        public double Budget { get; set; }
        public string Remark { get; set; }
        public string FileAttachment { get; set; }
    }
}
