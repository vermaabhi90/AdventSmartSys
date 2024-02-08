using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartSys.BL
{
  public  class DHFeedModel
    {
      [Display(Name = "Feed Id")]
      public int FeedID { get; set; }
      
      [Required(ErrorMessage="Feed Action Name cannot be empty.")]
      [Display(Name="Feed Name")]
      public String FeedName { get; set; }
      public String Description { get; set; }
      
      [Display(Name="Feed Type")]
      public String FeedType { get; set; }
       [Display(Name = "Feed Type Code")]
      public String FeedTypeCode { get; set; }

      public Int16 ModifiedBy { get; set; }
       [Display(Name = "Modified Date")]
      public DateTime ModifyDate { get; set; }
       [Display(Name = "Modified By")]
      public string ModifiedByName { get; set; }
    }

    public class DhFeedType
    {
        public string FeedTypeCode { get; set; }

        [Display(Name="Feed Type")]
        public string Description { get; set; }
    }

    public class DHActionType
    {
        public string ActionTypeCode { get; set; }
        [Display(Name = "Action Type")]
        public string Description { get; set; }
    }


    public class DHActionModel
    {
        public int FActionId { get; set; }
        public int FeedID { get; set; }
        public string ActionType { get; set; }
        public DHActionType dhActionType { get; set; }
        public string Description { get; set; }
        
        [Required(ErrorMessage="Please Enter Sequence No")]
        [Display(Name = "Sequence Number")]
        public int SequenceNO { get; set; }
       
    }
    public class DHFeedActionParam
    {
        public int FeedActionParamID { get; set; }
        public int FeedActionID { get; set; }
        public string ActionType { get; set; }

        [Required(ErrorMessage="* Requrired")]
        public string ParamName { get; set; }
        [Required(ErrorMessage = "* Requrired")]
        public string Value { get; set; }
        public int SequenceNO { get; set; }
    }

    public class DHActionDeatil
    {
        public string ActionTypeCode { get; set; }
        public string KeyName { get; set; }
        public string Value { get; set; }
        public string ValueDataType { get; set; }

    }
    public class DHFeedActionModel
    {
        public int iUserID;
        public int FeedActionID { get; set; }
        public DHFeedModel FeedModel { get; set; }
        public int Sequence { get; set; }
        public string Description { get; set; }
        public string FeedTypeCode { get; set; }
     //   public string Description { get; set; }

        public string Actiontype { get; set; }
        public List<DHFeedActionParam> DrpDwnlstParamName { get; set; }
        public List<DHActionModel> ListOfFeedActions { get; set; }

        public List<DHActionType> ListActionType { get; set; }
        public List<DhFeedType> ListFeedType { get; set; }
       
        public List<DHFeedActionParam> ListOfFeedActionParameters { get; set; }
    }

    public class DHFeedEvtTriggModel
    {
        public int FeedId { get; set; }
        public string FeedName { get; set; }
        public string Description { get; set; }
        public string FeedTypeCode { get; set; }
        public Int16 ModifiedBy { get; set; }
        public int CreatedBy { get; set; }
        public string ModifiedByName { get; set; }
        public DateTime ModifyDate { get; set; }
        public List<DHFeedEventDep> lstDHFeedEventDep { get; set; }
        public  List<DHFeedTriggerDep> lstDHFeedTriggerDep { get; set; }
    }
    public class DHFeedEvent
    {
        public Int16 EventId { get; set; }
        public string EventName { get; set; }
    }
    public class DHFeedEventDep
    {
        public int FeedId { get; set; }
        public Int16 EventId { get; set; }
        public string EventName { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedByName { get; set; }
    }

    public class DHFeedTrigger
    {
         public Int16 TriggerId { get; set; }
        public string TriggerName { get; set; }
    }
    public class DHFeedTriggerDep
    {

        public Int16 TriggerId { get; set; }
        public string TriggerName { get; set; }
        public int FeedId { get; set; }
        public Int16 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedByName { get; set; }
    }

#region 
    public class DHAchocRun
    {
        public int AdhocId { get; set; }
        public int FeedId { get; set; }
        public string FeedName { get; set; }
        public string FileName { get; set; }
        public int JobId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Int16 StatusId { get; set; }
        public Int16 LoaderId { get; set; }
        public string LoaderType { get; set; }
        public string StatusName { get; set; }
        public string CreatedByName { get; set; }
        public string CHkFeedType { get; set; }
        public List<DHAchocRun> LstadhocRun { get; set; }
    }

    #endregion
    #region Review
    #region Inventory Ageing Analysis
    public class InventoryAgeingAnalysisModel
    {
        public string CompCode { get; set; }
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public string make { get; set; }
        public double UnitCost { get; set; }
        public double Remaining_Quantity { get; set; }
        public double ZerotoThirty { get; set; }
        public double ZerotoThirtyValue { get; set; }
        public double ThirtytoSixty { get; set; }
        public double ThirtytoSixtyValue { get; set; }
        public double SixtyToNinty { get; set; }
        public double SixtyToNintyValue { get; set; }
        public double NintyPlus { get; set; }
        public double NintyPlusValue { get; set; }
        public double TotalQty { get; set; }
        public double TotalValue { get; set; }
        public string Status_OB { get; set; }
        public string Customer { get; set; }
        public string SONO { get; set; }
        public double Quantity { get; set; }
        public double OutstandingQuantity { get; set; }
        public string Comment { get; set; }
        public string FullName { get; set; }
        public string Date { get; set; }
        public string Createdate { get; set; }
        public DateTime CreateDate { get; set; }
    }
    #endregion Inventory Ageing Analysis
    #region Inventory
    public class InventoryModel
    {
        public string CompCode { get; set; }
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public string make { get; set; }
        public double UnitCost { get; set; }
        public string Unit { get; set; }
        public double Remaining_Quantity { get; set; }
        public double Value { get; set; }
        public string Status_OB { get; set; }
        public string Customer { get; set; }
        public string SONO { get; set; }
        public double Quantity { get; set; }
        public double OutstandingQuantity { get; set; }
        public string Comment { get; set; }
        public string FullName { get; set; }
        public string Date { get; set; }
        public string Createdate { get; set; }
        public DateTime CreateDate { get; set; }
    }
    #endregion Inventory
    #region Material Procured Vs Reduction
    public class MaterialProcuredVsReductionModel
    {
        public string DocumentNo { get; set; }
        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public string ItemName { get; set; }
        public string Make { get; set; }
        public double Quantity { get; set; }
        public double CurrentPurchaseRate { get; set; }
        public double LastPurchaseRate { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
        public string Createdate { get; set; }
        public DateTime CreateDate { get; set; }
        public string FullName { get; set; }

    }

    #endregion Material Procured Vs Reduction
    #region NNR 
    public class NNRModel
    {
        public string Company { get; set; }
        public string DocumentNo { get; set; }
        public string ItemNo { get; set; }
        public string NNR { get; set; }
        public string Description { get; set; }
        public DateTime PostingDate { get; set; }
        public string PoDate { get; set; }
        public string MPN { get; set; }
        public string Branch { get; set; }
        public string Make { get; set; }
        public string SONO { get; set; }
        public string CustomerOrderNo { get; set; }
        public string CustomerName { get; set; }
        public string SalesPersonName { get; set; }
        public double Quantity { get; set; }
        public double ExtnResale { get; set; }
        public double UnitResale { get; set; }
        public double UnitCost { get; set; }
        public double BasicUnit { get; set; }
        public string Comment { get; set; }
        public string FullName { get; set; }
        public string Date { get; set; }
        public string Createdate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
    #endregion NNR
    #region Purchase Report Monthwise
    public class PurchaseMonthwiseModel
    {
        public string No { get; set; }
        public string LineNo { get; set; }
        public string PostingDate { get; set; }
        public string DocumentNo { get; set; }
        public string PayToName { get; set; }
        public string ItemNo { get; set; }
        public string MPN { get; set; }
        public string Description { get; set; }
        public string Make { get; set; }
        public double BaseUnitPrice { get; set; }
        public double Quantity { get; set; }
        public string SONO { get; set; }
        public string Name { get; set; }
        public double SalesQty { get; set; }
        public double OutstandingQuantity { get; set; }
        public double Total_Value { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
        public string Createdate { get; set; }
        public DateTime CreateDate { get; set; }
        public string FullName { get; set; }
    }
    #endregion Purchase Report Monthwise
    #region Pending PO
     public class PendingPOModel
    {
        public string DocumentNo { get; set; }
        public int LineNo { get; set; }
        public string OrderDate { get; set; }
        public string VendNo { get; set; }
        public string Vendname { get; set; }
        public string ItemNo { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double QuantityReceived { get; set; }
        public double Balance { get; set; }
        public double QuantityInvoiced { get; set; }
        public double DirectUnitCost { get; set; }
        public double Amount { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
        public string Createdate { get; set; }
        public DateTime CreateDate { get; set; }
        public string FullName { get; set; }
    }
    #endregion Pending PO
    #region Franchise 
    public class FranchiseModel
    {
        public string VendorNo { get; set; }
        public string VendorName { get; set; }
        public string CurrencyCode { get; set; }
        public double TargetForYear { get; set; }
        public double Achived_Till { get; set; }
        public double BacklogTill { get; set; }
        public double Pending_to_be_achieve { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
        public string Createdate { get; set; }
        public DateTime CreateDate { get; set; }
        public string FullName { get; set; }
    }
    #endregion Franchise
    #endregion Review

}
