using SmartSys.BL.BOMAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.Enquiry
{
    public class EnquiryModel
    {
        public int EnqId { get; set; }
        public int QuotBy { get; set; }
        public string Currency { get; set; }
        public string EnqNumber { get; set; }
        public int CustomerContactId { get; set; }
        public string ContactName { get; set; }
        public string AssignedEmployee { get; set; }
        public DateTime AssignedDate { get; set; }
        public bool isOutSourcePerson { get; set; }
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        public int QuotationId { get; set; }
        public int RevisedQuotationId { get; set; }
        public int Rating { get; set; }
        public Double Rate { get; set; }
        public Double SaleAmount { get; set; }
        public Double PurchaseAmount { get; set; }
        public string QuotationNumber { get; set; }
        public string RevisedQuotationNumber { get; set; }
        public string Currancy { get; set; }
        public DateTime EnqDate { get; set; }
        public string EnqDateStr { get; set; }
        public double SaleRate { get; set; }
        public double PurchaseRate { get; set; }
        public double QuotaionCost { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Remark { get; set; }
        public string SalesPerson { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDatestr { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedDateStr { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string BrandName { get; set; }
        public string MPN { get; set; }
        public string CPN { get; set; }
        public string Dep { get; set; }
        public double SaleQuantity { get; set; }
        public double PurchaseQuantity { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public DateTime ExpectedDate { get; set; }
        public int PercentComplete { get; set; }
        public int RemainprocessItem { get; set; }
        public int Status { get; set; }
        public string StatusStr { get; set; }
        public string CompCode { get; set; }
        public string Types { get; set; }
        public string Priority { get; set; }
        public string ReportId { get; set; }
        public string ParamId { get; set; }
        public string ParamName { get; set; }
        public string TxtParamValue { get; set; }
        public string hidText { get; set; }
        public int StatusId { get; set; }
        public string OutputLocation { get; set; }
        public string Description { get; set; }
        public string ResponsPerson { get; set; }
        public bool isRem { get; set; }
        public int TermId { get; set; }
        public string IsChange { get; set; }
        public string FileName { get; set; }
        public DateTime RunDate { get; set; }
        public double EnquiryQuantity { get; set; }
        public string TimeTaken { get; set; }
        public string categories { get; set; }
        public int categoriesRefId { get; set; }
        public List<EnquiryModel> lstEnquiry { get; set; }
        public List<EnquiryDetailModel> lstEnquiryDetail { get; set; }
        public List<EnquiryDetailModel> RefList { get; set; }
        public List<QuotationModel> QuotList { get; set; }
        public List<EnqSendVendorModel> lstMail { get; set; }
        public List<EnquiryAttachment> lstAttchment { get; set; }
        public List<EnquiryModel> EmpQuotaionList { get; set; }
    }
    public class EnquiryAttachment
    {
        public int EnqId { get; set; }
        public string FileName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class EnquiryRespModel
    {
        public int ResponseId { get; set; }
        public int EnqId { get; set; }
        public string ItemName { get; set; }
        public string VendorName { get; set; }
        public string StatusStr { get; set; }
        public string TempStatusStr { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public List<EnquiryRespModel> lstEnquiryResponse { get; set; }
    }
    public class PreViewQuotModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string BrandName { get; set; }
        public string Currency { get; set; }
        public double Rate { get; set; }
        public int Quantity { get; set; }
        public string BatchNumber { get; set; }
        public string MPN { get; set; }
        public string VendorPromiseDate { get; set; }
        public int SPQ { get; set; }
        public int MOQ { get; set; }
        public string Remark { get; set; }
    }
    public class ItemListModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
    }
    public class QuotationModel
    {
        public int EnqId { get; set; }
        public string MailContent { get; set; }
        public string Currency { get; set; }
        public int TermId { get; set; }
        public int QuotationId { get; set; }
        public Double Amount { get; set; }
        public string QuotationNumber { get; set; }
        public string EMailId { get; set; }
        public string EnqNumber { get; set; }
        public DateTime QuotDate { get; set; }
        public string QuotationDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Terms { get; set; }
        public string CompCode { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public string modifyDt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class EnquiryDetailModel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int EnqId { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string Remark { get; set; }
        public double Quantity { get; set; }
        public string Check { get; set; }
        public string Brand { get; set; }
        public string MPN { get; set; }
        public string CPN { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public DateTime ExpectedDate { get; set; }
        public string ExpectedDateStr { get; set; }
        public string ErrorDetail { get; set; }
    }
    public class EnquiryBrandVemdorModel
    {
        public int VendorId { get; set; }
        public string Vendor_Id { get; set; }
        public string VendorName { get; set; }
    }
    public class InvoiceModel
    {
        public int InvoiceId { get; set; }
        public string Invoice_Id { get; set; }
        public string InvoiceName { get; set; }
    }
    public class InvoiceItemModel
    {
        public int ItemId { get; set; }
        public string Item_Id { get; set; }
        public string ItemName { get; set; }
    }
    public class CustomerDrpModel
    {
        public int CustomerId { get; set; }
        public string Customer_Id { get; set; }
        public string CustomerName { get; set; }
    }
    public class EnqSendVendorModel
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }

        public string EmailServer { get; set; }
        public string EmailPort { get; set; }
        public string EmailUserName { get; set; }
        public string EmailPassword { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string MailContent { get; set; }
        public string emailId { get; set; }
        public string VendorName { get; set; }
        public int VendorId { get; set; }
        public string EnqNumber { get; set; }
        public string Remark { get; set; }
        public DateTime ExpectedDate { get; set; }
        public int Quantity { get; set; }
        public List<EnqSendVendorModel> lstEnqSendVendorModel { get; set; }
        public List<EnqSendVendorModel> lstSingEnqSendVendorModel { get; set; }
    }
    public class EnqItemVendorContact
    {
        public int EnqId { get; set; }
        public int ItemId { get; set; }
        public string MPN { get; set; }
        public string ItemName { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public int  ContactId { get; set; }
        public string ContactName { get; set; }
    }
    public class EnquiryItemVendorResponseDetailModel
    {
        public int SeqNo { get; set; }
        public string Currency { get; set; }
        public string QuotCurrency { get; set; }
        public int ResponseId { get; set; }
        public int ItemId { get; set; }
        public string MPN { get; set; }
        public string QuotNumber { get; set; }
        public string CPN { get; set; }
        public Double Amount { get; set; }
        public Double CRate { get; set; }
        public string ItemName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public double Rate { get; set; }
        public string VendStatus { get; set; }
        public double MinimumQty { get; set; }
        public string BatchNumber { get; set; }
        public string VendorPromiseDate { get; set; }
        public string StrVendorPromiseDate { get; set; }
        public int rating { get; set; }
        public int VendorId { get; set; }
        public int Status { get; set; }
        public string VendorName { get; set; }
        public int ContactId { get; set; }
        public int SPQ { get; set; }
        public int MOQ { get; set; }
        public string ContactName { get; set; }
        public string ReciptMethod { get; set; }
        public DateTime ReciptDate { get; set; }
        public string StrReciptDate { get; set; }
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string modifyDt { get; set; }
        public string ReportId { get; set; }
        public string ParamId { get; set; }
        public string ParamName { get; set; }
        public string TxtParamValue { get; set; }
        public string hidText { get; set; }
        public int StatusId { get; set; }
        public string OutputLocation { get; set; }
        public string Check { get; set; }
        public DateTime RunDate { get; set; }
        public int Quantity { get; set; }
        public double Margin { get; set; }
        public List<EnquiryItemVendorResponseDetailModel> listEnqItemVendRespDetail { get; set; }
        public List<EnquiryItemVendorResponseDetailModel> listCustItem { get; set; }
    }
    public class CustomerPOModel
    {
        public int PurchaseOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public DateTime PODate { get; set; }
        public int QuotationId { get; set; }
        public int CustomerId { get; set; }
        public int OldItemId { get; set; }
        public int Type { get; set; }
        public int Rating { get; set; }
        public string CustomerName { get; set; }
        public string CompCode { get; set; }
        public int Status { get; set; }
        public string StatusStr { get; set; }
        public string DocumentFile { get; set; }
        public string DocumentFileOld { get; set; }
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string SalesOrderNo { get; set; }
        public List<CustomerPOModel> POlist { get; set; }
        public List<CustomerPODetailModel> PODetaillist { get; set; }
    }
    public class CustomerPODetailModel
    {
        public int PurchaseDetailOrderId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public int QuotationId { get; set; }
        public string QuotationName { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public string Remark { get; set; }
        public DateTime ExpectedDate { get; set; }
        public string ExpectedDateStr { get; set; }
        public string ApprovedBy { get; set; }
        public string ApprovedDatestr { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDatestr { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class SalesOrderModel
    {
        public int PurchaseOrderId { get; set; }
        public string OldSOId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public DateTime PODate { get; set; }
        public string Remark { get; set; }
        public string OldPurchaseOrderNumber { get; set; }
        public DateTime OldPODate { get; set; }
        public string OldRemark { get; set; }
        public string SOId { get; set; }
        public string SOName { get; set; }
    }
    public class InvoiceDetailModel
    {
        public int InvId { get; set; }
        public string CompCode { get; set; }
        public string InvRefNo { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string InvFile { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class CartonModel
    {
        public int DispatchId { get; set; }
        public int Id { get; set; }
        public int CartonId { get; set; }
        public int ReceiptId { get; set; }
        public double Weight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Remark { get; set; }
        public string Dimension { get; set; }
        public int AvlBit { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ReceiptModel
    {
        public int ReceiptId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string CompCode { get; set; }
        public string CompanyName { get; set; }
        public string Vendor_Id { get; set; }
        public int VendorId { get; set; }
        public string VendorInvNo { get; set; }
        public string VendorName { get; set; }
        public string FreightInvNo { get; set; }
        public DateTime VendorInvDate { get; set; }
        public DateTime FreightInvDate { get; set; }
        public DateTime FrieghtInvReceivedDate { get; set; }
        public int COO { get; set; }
        public int Rating { get; set; }
        public string PermitType { get; set; }
        public string PermitNumber { get; set; }
        public string County { get; set; }
        public int FreightForwarderId { get; set; }
        public string FreightForwarderName { get; set; }
        public string FFRef_No { get; set; }
        public string Currency { get; set; }
        public int ChargesId { get; set; }
        public string ChargesDescription { get; set; }
        public int PaymentTermId { get; set; }
        public string PaymentTerm { get; set; }
        public double freightCharges { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public double Amount { get; set; }
        public int SaveType { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<ReceiptModel> OtherCharges { get; set; }
        public string SupplierInvN0 { get; set; }
        public DateTime SupplierInvDate { get; set; }
        public string ReceiptFile { get; set; }
        public int MRIId { get; set; }
        public string MRIDOCNo { get; set; }
        public List<ReceiptDetailModel> LstReceiptModel { get; set; }
        public List<ReceiptTaxModel> LstReceiptTaxModel { get; set; }
        public List<TaxModel> TaxDrp { get; set; }
        public List<CartonModel> CartonDrp { get; set; }
        public string ApproveBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string RejectedBy { get; set; }
        public DateTime RejectedDate { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime ReviewedDate { get; set; }
        public string Remark { get; set; }
        public string RejectedRemark { get; set;}
        public string ReviewedRemark { get; set; }
        public string ApprovedRemark { get; set; }
    }
    public class AirwaysReceiptModel
    {
        public int ReceiptId { get; set; }
        public string FFRef_No { get; set; }
    }
    public class ReceiptDetailModel
    {
        public int ReceiptDetailId { get; set; }
        public int ReceiptId { get; set; }
        public int COO { get; set; }
        public string Country { get; set; }
        public string Dimension { get; set; }
        public int CartonId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public double Quantity { get; set; }
        public string PO_No { get; set; }
        public double InvoiceAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class ReceiptTaxModel
    {
        public int ReceiptDetailId { get; set; }
        public int ReceiptId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int TaxId { get; set; }
        public string TaxName { get; set; }
        public double TaxRate { get; set; }
        public double TaxAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool PaidByVendor { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class ReceiptItemLocationDetail
    {
        public int ReceiptItemDetailId { get; set; }
        public int ReceiptDetailId { get; set; }
        public int ItemId { get; set; }
        public string MPN { get; set; }
        public int Quantity { get; set; }
        public int TotalQuantity { get; set; }
        public string DateCode { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int COOId { get; set; }
        public string COOName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public List<ReceiptItemLocationDetail> ModelLst { get; set; }
    }

    #region Dispatch
    public class DispatchModel
    {
        public int DispatchId { get; set; }
        public string CompCode { get; set; }
        public string CompanyName { get; set; }
        public string Customer_Id { get; set; }
        public string StatusDesc { get; set; }
        public string FreightForwarder { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int COO { get; set; }
        public string Invoice_No { get; set; }
        public string County { get; set; }
        public int STLocationId { get; set; }
        public string STLocationName { get; set; }
        public DateTime DispatchDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int SaveType { get; set; }
        public string Dimension { get; set; }
        public double Weight { get; set; }
        public int FinalQuantity { get; set; }
        public int FreightForwarderId { get; set; }
        public string FreightForwarderName { get; set; }
        public string AirwayBillNo { get; set; }
        public string ExportPermitNo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<DispatchDetailModel> LstDispatchDetail { get; set; }
        public List<DispatchCartonDetailModel> LstDispatchCartonDetail { get; set; }
        public List<DispatchDocument> LstDispatchDoc { get; set; }
    }
    public class DispatchDocument
    {
        public int DispatchId { get; set; }
        public string DocumentTitle { get; set; }
        public string FilePath { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class DispatchDetailModel
    {
        public int DispatchDetailId { get; set; }
        public int CartonId { get; set; }
        public int DispatchId { get; set; }
        public string Invoice_No { get; set; }
        public string Item_Id { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public string Dimension { get; set; }
        public double Quantity { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateStr { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class DispatchCartonDetailModel
    {
        public int CartonId { get; set; }
        public int DispatchId { get; set; }
        public double Weight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Remark { get; set; }
    }
    #endregion Dispatch
    public class TrackingStatusConfigModel
    {
        public int ID { get; set; }
        public string StatusDesc { get; set; }
        public string Meaning { get; set; }
        public int OutstandingResponseTime { get; set; }
        public int MinimumTime { get; set; }
        public int MaximumTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class FreightFwdInvoiceModel
    {
        public int FreightInvId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<FreightFwdInvoiceDetailModel> LstDetal { get; set; }
        public List<FreightFwdInvoiceTaxesModel> LstTax { get; set; }
        public List<TaxModel> TaxDrp { get; set; }
    }
    public class FreightFwdInvoiceDetailModel
    {
        public int FreightInvDetailId { get; set; }
        public int FreightInvId { get; set; }
        public string FFRef_No { get; set; }
        public DateTime RefDate { get; set; }
        public string RefDateStr { get; set; }
        public int ReceiptId { get; set; }
        public string ReceiptName { get; set; }
        public double Amount { get; set; }
        public List<FreightFwdInvoiceTaxesModel> LstTax { get; set; }
    }
    public class FreightFwdInvoiceTaxesModel
    {
        public int FreightInvDetailId { get; set; }
        public int TaxId { get; set; }
        public string TaxName { get; set; }
        public double Amount { get; set; }
    }
    public class FreightFwdReceiptDrpModel
    {
        public int ReceiptId { get; set; }
        public string ReceiptName { get; set; }
    }
    public class FFNotificationModel
    {
        public int ReceiptId { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string CompanyName { get; set; }
        public string VendorName { get; set; }
        public string VendorInvNo { get; set; }
        public string VendorInvDate { get; set; }
        public string PermitType { get; set; }
        public string PermitNumber { get; set; }
        public string FreightForwarderName { get; set; }
        public string FFRef_No { get; set; }
        public string AirwaybillNumber { get; set; }
        public string Currency { get; set; }
        public double GSTAmount { get; set; }
        public string PermitNo { get; set; }
        public DateTime NotificationDate { get; set; }
        public string RefNo { get; set; }
        public int NotificationId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
    #region Reason
    public class ReasonModel
    {
        public int EnqId { get; set; }
        public int ReasonId { get; set; }
        public string Reason { get; set; }
        public string Remark { get; set; }
        public string Source { get; set; }
        public int Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    #endregion Reason
    #region Followup Type
    public class FollowupTypeModel
    {
        public int FollowupTypeId { get; set; }
        public string FollowupReason { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    #endregion Followup Type

    #region DataLevel Model
    public class DataLevelTwo
    {
        public string TimeTaken { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string MPN { get; set; }
        public DateTime ExpectedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Remark { get; set; }
    }
    public class DataLevelThree
    {
        public int EnqId { get; set; }
        public int ItemId { get; set; }
        public string Item_Id { get; set; }
        public string PurchCurr { get; set; }
        public int SourceId { get; set; }
        public string Description { get; set; }
        public double LastPurchaseRate { get; set; }
        public string LastPurchaseBy { get; set; }
        public DateTime LastPurchaseDate { get; set; }
        public string EmpName { get; set; }
        public string Manager { get; set; }
        public string TimeTaken { get; set; }
        public string PreparedBy { get; set; }
        public DateTime PreparedDate { get; set; }
    }
    public class DataLevelFour
    {
        public int EnqId { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public string TimeTaken { get; set; }
        public string Description { get; set; }
    }
    public class DataLevelFive
    {
        public string TimeTaken { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public string MPN { get; set; }
        public DateTime ExpectedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Remark { get; set; }
    }
    public class DataLevelSix
    {
        public string ItemName { get; set; }
        public string VendorName { get; set; }
        public string Remark { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime CommunicationDate { get; set; }
        public string Description { get; set; }
        public string AllocatedBy { get; set; }
        public string TimeTaken { get; set; }
        public string MPN { get; set; }
    }
    public class DataLevelSeven
    {
        public string ItemName { get; set; }
        public string VendorName { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpectedDate { get; set; }
        public DateTime SendDate { get; set; }
        public string Description { get; set; }
        public string SendBy { get; set; }
        public string TimeTaken { get; set; }
        public string MPN { get; set; }
    }
    public class DataLevelEight
    {
        public int EnqId { get; set; }
        public int ResponseId { get; set; }
        public string VendorName { get; set; }
        public string ReciptMethod { get; set; }
        public string ContactName { get; set; }
        public string Remark { get; set; }
        public string Currency { get; set; }
        public DateTime ReciptDate { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string TimeTaken { get; set; }
        public List<LevelEightDetail> DetailList { get; set; }
    }
    public class LevelEightDetail
    {
        public string ItemName { get; set; }
        public string BrandName { get; set; }
        public double Rate { get; set; }
        public int MinimumQty { get; set; }
        public string VendorPromiseDate { get; set; }
        public string SPQ { get; set; }
        public string MOQ { get; set; }
        public string TimeTaken { get; set; }
    }
    public class DataLevelNine
    {
        public string AssignedTo { get; set; }
        public string ResPreparedBy { get; set; }
        public DateTime ResPreparedDate { get; set; }
        public string TimeTaken { get; set; }
        public string Description { get; set; }
    }
    public class DataLevelTen
    {
        public int EnqId { get; set; }
        public string AssignedTo { get; set; }
        public string AssignedBy { get; set; }
        public DateTime AssignedDate { get; set; }
        public string TimeTaken { get; set; }
        public string Description { get; set; }
    }
    public class DataLevelEleven
    {
        public int QuotationId { get; set; }
        public string QuotationNumber { get; set; }
        public int EnqId { get; set; }
        public DateTime QuotDate { get; set; }
        public string Currancy { get; set; }
        public string Terms { get; set; }
        public string CompCode { get; set; }
        public string CustomerName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TimeTaken { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
    }
    public class DataLevelTwelve
    {
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public string BrandName { get; set; }
        public int Quantity { get; set; }
        public Double Rate { get; set; }
        public Double VendorRate { get; set; }
        public string Remark { get; set; }
        public string SPQ { get; set; }
        public string MOQ { get; set; }
        public string Description { get; set; }
        public string VendorName { get; set; }
    }
    public class DataLevelthirteen
    {
        public string QuotPreparedBy { get; set; }
        public DateTime QuotPreparedDate { get; set; }
        public string TimeTaken { get; set; }
        public string Description { get; set; }
    }
    public class DataLevelFourteen
    {
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string TimeTaken { get; set; }
        public string Description { get; set; }
    }
    public class DataLevelfifteen
    {
        public string SendTo { get; set; }
        public string SendBy { get; set; }
        public DateTime SendDate { get; set; }
        public string TimeTaken { get; set; }
        public string Description { get; set; }
    }
    public class DataLevelSixteen
    {
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public string BrandName { get; set; }
        public int Quantity { get; set; }
        public Double Rate { get; set; }
        public string Remark { get; set; }
        public string Description { get; set; }
    }
    public class DataLevelSeventeen
    {
        public DateTime PODate { get; set; }
        public string CustomerName { get; set; }
        public string DocumentFile { get; set; }
        public string CompCode { get; set; }
        public string SalesOrderNo_ { get; set; }
        public string Remark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TimeTaken { get; set; }
    }
    public class DataLevelEighteen
    {
        public DateTime ExpectedDate { get; set; }
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public string Remark { get; set; }
    }
    public class DataLevelNineteen
    {
        public string SendBy { get; set; }
        public DateTime SendDate { get; set; }
        public string TimeTaken { get; set; }
        public string Description { get; set; }
    }
    public class DataLevelTwenty
    {
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string TimeTaken { get; set; }
        public string Description { get; set; }
    }
    public class DataLevelTwentyOne
    {
        public string SalesOrderNo_ { get; set; }
        public string SOCreatedBy { get; set; }
        public DateTime SOCreatedDate { get; set; }
        public string TimeTaken { get; set; }
        public string Description { get; set; }
    }
    public class DataLevelTwentyTwo
    {
        public string PONumber { get; set; }
        public DateTime PostingDate { get; set; }
        public string SalesOrderNo_ { get; set; }
    }
    public class DataLevelTwentyThree
    {
        public string CompCode { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string VendorName { get; set; }
        public string FreightForwarder { get; set; }
        public string FFRef_No { get; set; }
        public string Currency { get; set; }
        public string VendorInvNo { get; set; }
        public string VendorInvDate { get; set; }
        public string PermitType { get; set; }
        public string ReceiptFile { get; set; }
    }
    public class DataLevelTwentyFour
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string COO { get; set; }
        public string PO_No { get; set; }
        public double InvoiceAmount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TimeTaken { get; set; }
    }
    public class DataLevelTwentyFive
    {
        public string PermitType { get; set; }
        public DateTime NotificationDate { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string VendorName { get; set; }
        public string FreightForwarder { get; set; }
        public string CompCode { get; set; }
        public string RefNo { get; set; }
        public string AirwayBillNo { get; set; }
        public double GSTAmount { get; set; }
        public string PermitNo { get; set; }
        public string Currency { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TimeTaken { get; set; }
    }
    public class DataLevelTwentySix
    {
        public string SendBy { get; set; }
        public DateTime SendDate { get; set; }
        public string TimeTaken { get; set; }
    }
    public class DataLevelTwentySeven
    {
        public string ReviewedBy { get; set; }
        public DateTime ReviewedDate { get; set; }
        public string TimeTaken { get; set; }
    }
    public class DataLevelTwentyEight
    {
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public string TimeTaken { get; set; }
    }
    public class DataLevelTwentyNine
    {
        public string ProcessBy { get; set; }
        public DateTime ProcessDate { get; set; }
        public string TimeTaken { get; set; }
        public string PurchInvNo { get; set; }
    }
    #endregion DataLevel Model

    #region Quotation Followup
    public class QoutFollowupModel
    {
        public int FollowupId { get; set; }
        public int QuotationId { get; set; }
        public DateTime QuotDate { get; set; }
        public string QuotationNumber { get; set; }
        public int FollowupReasonId { get; set; }
        public string FollowupReason { get; set; }
        public int EnqId { get; set; }
        public string FileName { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string CompCode { get; set; }
        public DateTime Followupdate { get; set; }
        public string Term { get; set; }
        public string CustomerName { get; set; }
        public string SalesPersonName { get; set; }
        public double QuotCost { get; set; }
        public DateTime NextFollowUp { get; set; }
        public string FollowupRemark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDatestr { get; set; }
        public string OutputLocation { get; set; }
        public DateTime RunDate { get; set; }
        public string ReportId { get; set; }
        public string ParamId { get; set; }
        public string ParamName { get; set; }
        public string TxtParamValue { get; set; }
        public string hidText { get; set; }
        public int StatusId { get; set; }
        public List<QoutFollowupModel> QuotReminderList { get; set; }
    }
    public class QuotFollowupDetailModel
    {
        public bool SelectRow { get; set; }
        public int QuotationId { get; set; }
        public int EnqId { get; set; }
        public int ItemId { get; set; }
        public int StatusId { get; set; }
        public string StatusCode { get; set; }
        public string ItemName { get; set; }
        public string BrandName { get; set; }
        public string Currency { get; set; }
        public double Rate { get; set; }
        public double ProposeRate { get; set; }
        public int Quantity { get; set; }
        public string BatchNumber { get; set; }
        public string MPN { get; set; }
        public string VendorPromiseDate { get; set; }
        public int SPQ { get; set; }
        public int MOQ { get; set; }
        public string Remark { get; set; }
    }
    public class QoutFollowupHistoryModel
    {
        public int FollowupId { get; set; }
        public int QuotationId { get; set; }
        public DateTime QuotDate { get; set; }
        public int RevisedQuatId { get; set; }
        public int ItemId { get; set; }
        public string MPN { get; set; }
        public string ItemName { get; set; }
        public string FollowupReason { get; set; }
        public string QuotationNumber { get; set; }
        public DateTime Followupdate { get; set; }
        public DateTime NextFollowUp { get; set; }
        public string FollowupRemark { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    #endregion Quotation Followup

    #region To Do
    public class AllPendingWorkModel
    {
        public int Id { get; set; }
        public int DocId { get; set; }
        public string Datetype { get; set; }
        public string DocType { get; set; }
        public int PlanId { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string StrStartDate { get; set; }
        public string StrDueDate { get; set; }
    }
    public class SubordinateListModel
    {
        public string User_Id { get; set; }
        public string EmployeeName { get; set; }
    }
    #endregion To Do

    #region Preliminary Quotation
    public class PreliminaryQuotationModel
    {
        public int EnqId { get; set; }
        public string Currency { get; set; }
        public int TermId { get; set; }
        public int PreQuotId { get; set; }
        public string PreQuotNumber { get; set; }
        public DateTime QuotDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string FileName { get; set; }
        public string Terms { get; set; }
        public string CompCode { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Remark { get; set; }
        public List<PreliminaryQuotDetailModel> LstDetail { get; set; }
    }
    public class PreliminaryQuotDetailModel
    {
        public int PreQuotId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int Quantity { get; set; }
        public double Rate { get; set; }
        public string Remark { get; set; }
        public double FXRate { get; set; }
    }
    public class PreQuotRateDetailModel
    {
        public int EnqId { get; set; }
        public int PreQuotId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public string BrandName { get; set; }
        public double LastSaleRate { get; set; }
        public double AvgSixMonthRate { get; set; }
        public double VendPriceListRate { get; set; }
        public double ZaubaMax { get; set; }
        public double ZaubaAvg { get; set; }
        public double ZaubaMin { get; set; }
        public double Digikey { get; set; }

    }
    #endregion Preliminary Quotation

    #region Nav POSO Mapping
    public class NavPOSOMapModel
    {
        public string NAVPONO { get; set; }
        public string NAVSONO { get; set; }
        public string CompCode { get; set; }
        public string OldNAVPONO { get; set; }
        public string OldNAVSONO { get; set; }
        public string OldCompCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    #endregion Nav POSO Mapping

    #region Material Dispatch Intimetion
    public class MDIModel
    {
        public int MRIId { get; set; }
        public string DocumentNo { get; set; }
        public DateTime MRIDate { get; set; }
        public string Remark { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string CompCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<MDIItemModel> LstMDIItemList { get; set; }
    }
    public class MDIItemModel
    {
        public int MRIDetailId { get; set; }
        public int MRIId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public int Quantity { get; set; }
        public Double Rate { get; set; }
        public string NAVPONO { get; set; }
        public DateTime NAVPODate { get; set; }
        public string StrNAVPODate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string strCreatedDate { get; set; }
        public int ReceiptId { get; set; }
    }
    #endregion Material Dispatch Intimetion

    #region Quotation Analyst
    public class QuotAnalystModel
    {
        public int EnqId { get; set; }
        public string EnqNumber { get; set; }
        public DateTime EnqDate { get; set; }
        public string status { get; set; }
        public string CustomerName { get; set; }
        public string ContactName { get; set; }
        public string CompCode { get; set; }
        public string categories { get; set; }
        public string Remark { get; set; }
        public string MailAttchment { get; set; }
        public string QuotationPdfAttach { get; set; }
        public string TotalHR { get; set; }
        public List<QoutFollowupHistoryModel> QuotFollowupDetailList { get; set; }
        public List<QoutFollowupModel> Last5WinQuot { get; set; }
        public List<QoutFollowupModel> Last5lossQuot { get; set; }
        public List<QuotItem> QuotItemList { get; set; }
        public QuotItem ItemDetail { get; set; }
    }
    public class QuotItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public int status { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpectedDate { get; set; }
        public string StrExpectedDate { get; set; }
        public string BrandName { get; set; }
        public string CPN { get; set; }
        public double LastSalesRate { get; set; }
        public double LastPurchaseRate { get; set; }
        public double MAXSalesRate { get; set; }
        public double MINSalesRate { get; set; }
        public string AnalystRemark { get; set; }
        public List<EnquiryItemVendorResponseDetailModel> ItemQuotList { get; set; }
        public List<QuotItemHistory> Last5PurchaseItemList { get; set; }
        public List<QuotItemHistory> Last5SalesItemList { get; set; }
        public List<QuotItemHistory> LastSalesItemSameCustList { get; set; }
        public List<EnquiryItemVendorResponseDetailModel> ItemVendorResList { get; set; }       
    }
    public class QuotItemHistory
    {
        public string MPN { get; set; }
        public string Name { get; set; }
        public double Qty { get; set; }
        public string DocNo { get; set; }
        public double Rate { get; set; }
        public DateTime SalesDate { get; set; }
        public string StrSalesDate { get; set; }
    }
    #endregion Quotation Analyst

    #region Item Stock Audit
    public class ItemStockAuditModel
    {
        public int AuditId { get; set; }
        public string CompCode { get; set; }
        public DateTime AuditDate { get; set; }
        public string AuditBy { get; set; }
        public int LocationId { get; set; }
        public string Remark { get; set; }
        public string ReportId { get; set; }
        public string ParamId { get; set; }
        public string ParamName { get; set; }
        public string TxtParamValue { get; set; }
        public string hidText { get; set; }
        public int StatusId { get; set; }
        public string OutputLocation { get; set; }
        public DateTime RunDate { get; set; }
        public DateTime CurrentDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<ItemStockAuditDetailModel> DetailList { get; set; }
    }
    public class ItemStockAuditDetailModel
    {
        public int AuditDetailId { get; set; }
        public int AuditId { get; set; }
        public string MPN { get; set; }
        public string ItemName { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int LocationId { get; set; }
        public string ErrorDetail { get; set; }
        public string Check { get; set; }
        public string LocationIdDesc { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedDate { get; set; }
    }
    #endregion Item Stock Audit
}
