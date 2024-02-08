using SmartSys.BL.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.ProViews
{
    public class CaseRiskViewModel
    {
        public string ProjectManager { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Assigned { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string CustomerName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string VendorName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartDatestr { get; set; }
        public string EndDatestr { get; set; }
        public int ReviewedBy { get; set; }
        public int TaskType { get; set; }
        public string TaskTypeText { get; set; }
        public string ReviewedByName { get; set; }
        public DateTime ReviewedDate { get; set; }
        public string ReviewedDatestr { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ApprovedDatestr { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedDatestr { get; set; }
        public string ModifiedByName { get; set; }
        public int Status { get; set; }
        public string StatusName { get; set; }
        public string Resources { get; set; }
        public string FilterType { get; set; }
        public List<CaseRiskViewModel> LstCaseRisk { get; set; }
    }
    public class TimeSheetViewModel
    {
        public int TimeSheetId { get; set; }
        public string EmployeeName { get; set; }
        public int WeekNo { get; set; }
        public string Quarter { get; set; }
        public string Year { get; set; }
        public string ExactMonth { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateStr { get; set; }
        public string StatusShortCode { get; set; }
        public string ApproverName { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ApprovedDateStr { get; set; }
        public string ApproverRemark { get; set; }
        public string Remark { get; set; }
        public string CreatedByName { get; set; }
        public string CustomerName { get; set; }
        public string VendorName { get; set; }
        public string MOMTitle { get; set; }
        public int MOMId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateStr { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedDateStr { get; set; }
        public int TimeSheetEntryId { get; set; }
        public string ProjectName { get; set; }
        public string TaskName { get; set; }
        public string DayText { get; set; }
        public string DateText { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int TotalTime { get; set; }
        public int TimeSheetEntryDetailId { get; set; }
        public string BudgetCustomer { get; set; }
    }
    public class SalesBacklogViewModel
    {

        public int BacklogCommentId { get; set; }
        public string OrderNo { get; set; }
        public string Company { get; set; }
        public string DocumentNo { get; set; }
        public string ItemNo { get; set; }
        public int LineNo { get; set; }
        public string Comments { get; set; }
        public string BillToName { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string CustomerOrderNo_ { get; set; }
        public DateTime Cust_PO_Date { get; set; }
        public DateTime Posting_Date { get; set; }
        public DateTime CRD { get; set; }
        public DateTime VPD { get; set; }
        public DateTime CommentDate { get; set; }
        public string VendorPartNo { get; set; }
        public string Make { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        public double LineAmount { get; set; }
        public double OutstandingQuantity { get; set; }
        public double OutstandingAmount { get; set; }
        public double Item_Qty_On_Hand { get; set; }
        public double Qty_On_PO { get; set; }
        public string SalespersonCode { get; set; }
        public string Full_Name { get; set; }
        public List<Commentmodel> CommentsList { get; set; }
        public List<SalesBacklogViewModel> BacklogList { get; set; }
    }

    public class Commentmodel
    {
        public string OrderNo { get; set; }
        public string Company { get; set; }
        public string DocumentNo { get; set; }
        public DateTime CommentDate { get; set; }
        public int LineNo { get; set; }
        public string Comments { get; set; }
    }
    public class PendingMOMActions
    {
        public int ActionPointId { get; set; }
        public int MOMId { get; set; }
        public string ActionDescription { get; set; }
        public string Assigned { get; set; }
        public string MOMTitle { get; set; }
        public int Status { get; set; }
        public string StatusDescrition { get; set; }
        public string StatusShortCode { get; set; }
        public string DueDate { get; set; }
        public string Resource { get; set; }
        public string Vendor { get; set; }
        public string Customer { get; set; }
        public string AssignedBy { get; set; }
        public string AssignedTo { get; set; }
        public DateTime MOMDueDate { get; set; }
        public int OverDueDays { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string StrCreatedDate { get; set; }
        public string StrModifiedDate { get; set; }
        public List<PendingMOMActions> LstAllActionPointList { get; set; }
    }
    public class CustomerEnqStatus
    {
        public int SeqNo { get; set; }
        public int StatusId { get; set; }
        public string LableDesc { get; set; }
        public string tooltiptext { get; set; }
        public string CurrentState { get; set; }
    }
    public class MailEnquiryTimespan
    {
        public int MinuteDiff { get; set; }
        public int TimeAxis { get; set; }
        public int ID { get; set; }
        public string StatusDesc { get; set; }
        public int OutstandingResponseTime { get; set; }
        public int MinimumTime { get; set; }
        public int MaximumTime { get; set; }
        public List<MailEnquiryTimespan> MailEnqTimespanLst { get; set; }
        public List<CustomerEnqStatus> EnqStatusList { get; set; }
    }
    public class EnquiryTrackingViewDetail
    {
        public int EnqId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int StatusId { get; set; }
        public int QuotationId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string EnqiryGetBy { get; set; }
        public DateTime EnqiryDate { get; set; }
        public DateTime PreprationDate { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int parent { get; set; }
        public string itemTitleColor { get; set; }
        public List<EnquiryTrackingViewDetail> EMailEnqTeackingLst { get; set; }
        public List<EnquiryTrackingViewDetail> EnqTrackdetailsLst { get; set; }
        public List<EnquiryTrackingViewDetailChart> TestLst { get; set; }
        public List<CustomerEnqStatus> EnqStatusList { get; set; }
    }
    public class EnquiryTrackingViewDetailChart
    {
        public int Enqid { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string[] parents { get; set; }
        public string itemTitleColor { get; set; }
        public int MinuteDiff { get; set; }
        public int OTRT { get; set; }
        public int MinimumTime { get; set; }
        public int MaximumTime { get; set; }
        public int TimeAxis { get; set; }
        public int DataLevel { get; set; }
        public int SourceId { get; set; }
        public int SId { get; set; }
        public string description { get; set; }
    }
    public class ProjectTMView
    {
        public int ProjectId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string TotalTime { get; set; }
    }
    public class ProjectAttachmentView
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
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

    }
    public class ProjectPlanVsActualView
    {
        public int TaskId { get; set; }
        public string TaskType { get; set; }
        public string TaskName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PlannedMin { get; set; }
        public int ActualTime { get; set; }
        public int ActualTimeper { get; set; }

    }
    public class ProjectResourceView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
    public class ProjectResourceDetailsViewModel
    {
        public int MOMId { get; set; }
        public string Title { get; set; }
        public string MOMType { get; set; }
        public DateTime MOMDate { get; set; }
        public string Name { get; set; }
        public string ParticipantType { get; set; }
        public List<ProjectResourceDetailsViewModel> LstMOM { get; set; }
        public List<MOMActionPointComment> LstComment { get; set; }
        public List<PendingMOMActions> LstActionPoint { get; set; }
        public List<ProjectPlanVsActualView> LstTaskResource { get; set; }
        public List<TimeSheetViewModel> LstTimesheet { get; set; }
    }

    public class GetTaskdurationByStatusModel
    {

        public DateTime CreatedDate { get; set; }
        public int TaskId { get; set; }
        public int StartValue { get; set; }
        public int projectId { get; set; }
        public int ApproveEndTime { get; set; }
        public int InprogressEndTime { get; set; }
        public int ReviewedEndTime { get; set; }
        public int CompleteEndTime { get; set; }
        public string TaskName { get; set; }
        public DateTime ReviewedDate { get; set; }
        public int ReviewedTime { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int ApprovedTime { get; set; }
        public DateTime InprogressDate { get; set; }
        public int InprogressTime { get; set; }
        public DateTime CompleteDate { get; set; }
        public int CompleteTime { get; set; }
        public int MaxVal { get; set; }
        public List<GetTaskdurationByStatusModel> LstStatus { get; set; }
    }
    #region BrandView
    public class BrandListViewModel
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int EnqId { get; set; }
        public string EnqNumber { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string MPN { get; set; }
        public int Quantity { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByEmp { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EnqDate { get; set; }
        public bool isSales { get; set; }
        public bool isPurchase { get; set; }
        public List<BrandListViewModel> VendorList { get; set; }
    }
    #endregion BrandView
    #region OrgChart
    //public class EnquiryTrackingViewDetailChart
    //{
    //    public int id { get; set; }
    //    public string title { get; set; }
    //    public string[] parents { get; set; }
    //    public string description { get; set; }


    //}
    //public class EnquiryTrackingViewDetail
    //{
    //    public int EnqId { get; set; }
    //    public int ItemId { get; set; }
    //    public string ItemName { get; set; }
    //    public int VendorId { get; set; }
    //    public string VendorName { get; set; }
    //    public int StatusId { get; set; }
    //    public int QuotationId { get; set; }
    //    public int PurchaseOrderId { get; set; }
    //    public int CustomerId { get; set; }
    //    public string CustomerName { get; set; }
    //    public string Title { get; set; }
    //    public string Details { get; set; }
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public int parent { get; set; }
    //    public List<EnquiryTrackingViewDetail> EMailEnqTeackingLst { get; set; }
    //    public List<EnquiryTrackingViewDetailChart> LstOrgChart { get; set; }
    //}
    #endregion OrgChart

    public class AccountReceivableModel
    {

        public string Company { get; set; }
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        public Double Amount { get; set; }
        public DateTime PostingDate { get; set; }
        public Double Amount_LCY { get; set; }
        public Double CreditAmount { get; set; }
        public Double CreditAmountLCY { get; set; }
        public Double DebitAmount { get; set; }
        public Double DebitAmountLCY { get; set; }
        public string Make { get; set; }
        public string Currency { get; set; }
        public decimal RemainingAmount { get; set; }
        public decimal RemainingAmountLCY { get; set; }

    }
    public class CustomerDispatch
    {
        public int DispatchId { get; set; }
        public string CompCode { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Invoice_No { get; set; }
        public string AirwayBillNo { get; set; }
        public string ExportPermitNo { get; set; }
        public DateTime DispatchDate { get; set; }
        public string Status { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }


    }
    public class CustomerItemWiseModel
    {
        public string CompCode { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string VPN { get; set; }
        public string MPN { get; set; }
        public string BrandName { get; set; }
        public string CPN { get; set; }
        public double ItemCurQtyInHand { get; set; }
        public double BalanceAmount { get; set; }
        public double BalanceQuantity { get; set; }
        public double Value { get; set; }
        public int EnqId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public double EnqQuantity { get; set; }
        public double PurchaseQtyAdvent { get; set; }
        public double PurchaseQtySAJ { get; set; }
        public double SaleQtyAdvent { get; set; }
        public double UnitPrice { get; set; }
        public double SaleQtySAJ { get; set; }
        public string CustOrdNo { get; set; }
    }

    public class SalesOrderBaklogModel
    {
        public string CompCode { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Customer_No { get; set; }
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public string CustOrdNo { get; set; }
        public string SalePerCode { get; set; }
        public string FullName { get; set; }
        public string VPN { get; set; }
        public string Make { get; set; }
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        public double LineAmt { get; set; }
        public double OutQuantity { get; set; }
        public double OutAmt { get; set; }
        public DateTime Cust_PO_Date { get; set; }
        public DateTime CRD { get; set; }
        public DateTime Posting_Date { get; set; }
        public DateTime VPD { get; set; }
    }
    public class EmpCalenderModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int EmpId { get; set; }
        public DateTime ActualDate { get; set; }
        public int Timespend { get; set; }
        public int ParentId { get; set; }
        public int ProjectId { get; set; }
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public string Day { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
        public string CustomerName { get; set; }
        public string VendorName { get; set; }
        public string ProjectManagerName { get; set; }
        public double Duration { get; set; }
        public List<EmpCalenderModel> GetChildList { get; set; }
        public List<EmpCalenderModel> EmpTaskLst { get; set; }

    }

    #region Backlogs 
    public class BacklogsModel
    {
        public string PONumber { get; set; }
        public DateTime PODate { get; set; }
        public string VendorNO { get; set; }
        public string CustomerNO { get; set; }
        public string VendorName { get; set; }
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double QtyRec { get; set; }
        public double QtyInv { get; set; }
        public double BalanceQty { get; set; }
        public double UnitCost { get; set; }
        public DateTime CRD { get; set; }
        public DateTime VPD { get; set; }
        public string StrCRD { get; set; }
        public string StrVPD { get; set; }
        public double Amount { get; set; }
        public string SalesOrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string BrandName { get; set; }
    }
    #endregion Backlogs
    public class InvetoryModel
    {
        public string CompCode { get; set; }
        public string CustomerName { get; set; }
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public string SalesOrderNo { get; set; }
        public string PurchaseOrderNo { get; set; }
        public int SalesQuantity { get; set; }
        public int SalesOutstandingQty { get; set; }
        public int TotalDispatch { get; set; }
        public int TotalReceive { get; set; }
        public int PurchaseQuantity { get; set; }
        public int PurchaseOutstandingQty { get; set; }
        public int TotalInventory { get; set; }
        public string CustomerOrderNo { get; set; }
    }
}
