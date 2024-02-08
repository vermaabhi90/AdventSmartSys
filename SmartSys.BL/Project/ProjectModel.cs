using SmartSys.BL.ProViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.Project
{
    public class ProcjectDocModel
    {
        public int ProjectId { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class DrpItem
    {
        public int Id { get; set; }
        public int QuotationId { get; set; }
        public string DisplayName { get; set; }
        public Boolean Selected { get; set; }
    }
    public class ProjectCustomerAndVendor
    {
        public string ProjVendorName { get; set; }
        public string ProjCustomerName { get; set; }
    }
    public class TaskDetails
    {
        public int ProjectId { get; set; }
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public String EstimationTime { get; set; }
        public int  EstimationHH { get; set; }
        public int EstimationMM { get; set; }
        public int TotalChild { get; set; }
        public int CommentId { get; set; }
        public string ModifiedByName { get; set; }
        public string AssignedByMe { get; set; }
        public string AssignedTome { get; set; }
        public string Comments { get; set; }
        public string ProjectName { get; set; }
        public string Edit { get; set; }
        public string Type { get; set; }
        public int Modal { get; set; }
        public string TskType { get; set; }
        public Boolean AllowEdit { get; set; }
        public int Employee { get; set; }
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public string Status { get; set; }
        public int  Statuscode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ApprovedBy { get; set; }
        public string ReviewedBy { get; set;}
        public DateTime ReviewedDate { get; set; }
        public DateTime ApprovedDate { get; set; }
        public float Duration { get; set; }
        public Int16 TaskType { get; set; }
        public int Progress { get; set; }
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public int ProjCustomerId { get; set; }
        public int ProjVendorId { get; set; }
        public string ProjVendorName { get; set; }
        public string ProjCustomerName { get; set; }
        public int ParentTaskId { get; set; }
        public string Resources { get; set; }
        public string AllowClose { get; set; }
        public List<TaskDetails> SubTasks { get; set; }
        public List<ProcjectDocModel> LstProjectTaskDoc { get; set; }
        public List<Commentmodel> LstCommentList { get; set; }
        public List<object> ResourceID { get; set; }
        public string Predecessors { get; set; }
        public string Description { get; set; }
        public string TaskTypeDesc { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class Commentmodel
    {
        public int ProjectId { get; set; }
        public int CommentId { get; set; }
        public int TaskID { get; set; }
        public string Comments { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommentedBy { get; set; }
        public string Status { get; set; }
        public string toMail { get; set; }
        public string User_Id { get; set; }
    }
    public class ProjectTypeTaskDetails
    {
        public int ProjectTypeId { get; set; }
        public int ProjectTypeTaskID { get; set; }
        public string TaskName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public float Duration { get; set; }
        public Int16 TaskType { get; set; }
        public string TaskTypeDesc { get; set; }
        public int ParentTaskId { get; set; }
        public List<ProjectTypeTaskDetails> SubTasks { get; set; }
        public List<ProcjectTypeDocModel> LstProjectTaskDoc { get; set; }
        public string Predecessors { get; set; }
        public string Description { get; set; }
        public int Progress { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime  ModifiedDate { get; set; }
    }
    public class ProcjectTypeDocModel
    {
        public int ProjectTypeId { get; set; }
        public int ProjectTypeTaskId { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProjectTypeModel
    {
        public int ProjectTypeId { get; set; }
        public string ProjectType { get; set; }
        public Boolean isActive { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ProjectTypeMaster
    {
        public int ProjectTypeId { get; set; }
        public string ProjectType { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public List<ProjectTypeTaskDetails> ProjectTypeTasks { get; set; }
    }
    public class ProjectModel
    {
        public int ProjectId{get;set;}
        public int TAMCOUNT { get; set; }
        public Boolean AllowDelete { get; set; }
        public Boolean AllowEdit { get; set; }
        public string Edit { get; set; }
        public string Assigned { get; set; }
        public string Status { get; set; }
        public string ProjectType { get; set; }
        public string ProjectName {get;set;}
        public string CustomerName { get; set; }
        public string VendorName { get; set; }
        public int ProjectTypeId { get; set; }
        public string Description { get; set; }
        public string StatusDescription{get;set;}
        public int StatusId { get; set; }
        public DateTime StartDate{get;set;}
        public DateTime EndDate{get;set;}
        public string StartDateStr { get; set; }
        public string EndDateStr { get; set; }
        public string ProjectManager {get;set;}
        public string ItemPermission { get; set; }
        public int ProjectManagerId { get; set; }
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public string Region{get;set;}
        public string Remark{get;set;}
        public string CompCode { get; set; }
        public string CreatedByName { get; set; }
        public string Email { get; set; }
        public int SegmentId { get; set; }
        public string SegmentName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateStr { get; set; }
        public string ModifiedByName { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedDateStr { get; set; }
        public List<ProjectModel> Projectmodel { get; set; }

        public List<CaseRiskViewModel> RiskCase { get; set; }


    
    }

    public class ProjectMaster
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string ReportId { get; set; }
        public string ParamId { get; set; }
        public string ParamName { get; set; }
        public int TxtParamValue { get; set; }
        public string hidText { get; set; }
        public int StatusId { get; set; }
        public string OutputLocation { get; set; }
        public DateTime RunDate { get; set; }
        public List<TaskDetails> Tasks { get; set; }
        public List<ProjectTaskMoM> ViewMoMList { get; set; }
    }
    public class Resources
    {
        public int ResourceID { get; set; }
        public string EmpName { get; set; }
    }
      #region Project TAsk MOM
    public class ProjectTaskMoM
    {
        public int MOMId { get; set; }
        public int TaskId { get; set; }
        public string Resource { get; set; }
        public string TaskName { get; set; }
        public string Assigned { get; set; }
        public int EmpId { get; set; }
        public string Employee { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public int ProjectId { get; set; }
        public int TabIndex { get; set; }
        public string ProjectName { get; set; }
         public string Description { get; set; }
         public string LocalDescription { get; set; }
         public string ManageMentView { get; set; }
         public int CustomerContactId { get; set; }
         public int VendorContactId { get; set; }
         public string MOMTypeKey { get; set; }
        public string Title { get; set; }
        public DateTime MOMDate { get; set; }
        public string MOMDateStr { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string VendorName { get; set; }
        public int VendorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateStr { get; set; }
        public string ModifiedDateStr { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<ProjTaskModel> LstProjTask { get; set; }
        public List<MoMParticipantModel> LstMoMParticipant { get; set; }
        public List<MoMAttachmentModel> LstMOMAttachment { get; set; }
        public List<MOMActionPoint> ActionPointList { get; set; }
    }
    public class MOMType
    {
        public string MOMTypeKey { get; set; }
        public string MOMTypeValue { get; set; }
    }
    public class CustomerContact
    {
       public string  ContactName{ get; set; }
      public int CustomerContactId { get; set; }
    }
    public class VendorContact
    {
        public string ContactName { get; set; }
        public int VendorContactId { get; set; }
    }

    public class ProjTaskModel
    {
   
        public int TaskID { get; set; }
        public string TaskName { get; set; }
    }

    public class MoMParticipantModel
    {
        public Boolean FYI { get; set; }
        public string  FYIValue{ get; set; }
        public int MOMId { get; set; }
        public int ParticipantId { get; set; }
        public string Name { get; set; }
        public string ParticipantType { get; set; }
    }

    public class MoMAttachmentModel
    {

        public int MOMId { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
    }
     #endregion Project Task Mom

    #region Project Expenses
    public class ProjectExpensesModel
    {
        public int ExpenseId { get; set; }
        public Double TotalPaid { get; set; }
        public int PaymentId { get; set; }
        public int ExpTypeId { get; set; }
        public int ProjectId { get; set; }
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public string ProjectName { get; set; }
        public string ExpenseType { get; set; }
        public int EmpId { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public double Amount { get; set; }
        public double NewAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string Remark { get; set; }
        public int ApproverId { get; set; }
        public string ApproverName { get; set; }
        public DateTime ApproverDate { get; set; }
        public string ApproverRemark { get; set; }
        public int Status { get; set; }
        public string StatusCode { get; set; }
        public string Employee { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedDateStr { get; set; }
        public string DocumentPath { get; set; }
        public string Description { get; set; }
        public string ManagerRemark { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ExpenseDateStr { get; set; }
        public List<ProjectExpensesModel> LstPrjectExp { get; set; }
        public List<ProjTaskModel> LstProjTask { get; set; }
        public List<ExpensesDocModel> LstExpDoc { get; set; }
    }

    public class ExpensesDocModel
    {
        public int ExpenseId { get; set; }
        public string DocumentPath { get; set; }
        public string Description { get; set; }
    }
    #endregion Project Expenses

     #region Expense Type
    public class ExpenseTypeModel
    {
        public int ExpTypeId { get; set; }
        public string ExpenceType { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class PaymentExpenseStatus
    {
        public int ExpenseId { get; set; }
        public int PaymentId { get; set; }
        public Double TotalPaid { get; set; }
        public string PaymentType{ get; set; }
        public Double Amount{ get; set; }
        public Double NewAmount { get; set; }
        public string Remark{ get; set; }
        public string Isdeleted { get; set; }
       public int RefId{ get; set; }
       public int EmpId{ get; set; }
       public string Employee { get; set; }
       public string CreatedBy{ get; set; }
       public DateTime CreatedDate{ get; set; }
       public string ModifiedBy{ get; set; }
       public DateTime ModifiedDate { get; set; }
    }
        #endregion Expense Type

#region Actionpoint
     
    public class MOMActionPoint
    {
        public int ActionPointId { get; set; }
     
        public Nullable<DateTime> DueDate  { get; set; }
        public DateTime DueDateNULL { get; set; }
        public string Comment { get; set; }
        public string ParticipantType { get; set; }
        public int MOMId { get; set; }
        public string ActionDescription { get; set; }
        public int Status { get; set; }
        public string StatusShortCode { get; set; }
        public string Resource { get; set; }
        public string Vendor { get; set; }
        public string Customer { get; set; }
        public string AssignedBy { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<MOMActionPointUser> LstActionPointParticipant { get; set; }
        public List<MOMActionPointComment> MOMActionPointCommentList { get; set; }
    }
    public class MOMStatusCodes
    {
        public int Status { get; set; }
           public string StatusShortCode { get; set; }
    }
    public class MOMActionPointUser 
    {
        public int ActionPointId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string ParticipantType { get; set; }

        public int MOMId { get; set; }
    }

    public class MOMActionPointComment
    {
        public int ActionPointId { get; set; }
        public string Comment { get; set; }
        public string CommentedBy { get; set; }
        public DateTime CommentDate { get; set; }
        public string StatusShortCode { get; set; }
    }
#endregion Actionpoint

}
