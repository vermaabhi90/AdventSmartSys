using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.BOMAdmin
{
   
       public class VendorBrandModel
       {
           public int BrandId { get; set; }
           public string BrandName { get; set; }
           public string CreatedBy { get; set; }
           public DateTime CreatedDate { get; set; }
       }
       public class Paytermsmodel
       {
           public int PTId { get; set; }
           public string PTCode { get; set; }
           public string Description { get; set; }
           public string CreatedBy { get; set; }
           public DateTime CreatedDate { get; set; }
           public string ModifiedBy { get; set; }
           public DateTime ModifiedDate { get; set; }

       }
       public class BrandItemModel
       {
           public int BrandId { get; set; }
           public string BrandName { get; set; }
           public int ItemId { get; set; }
           public string ItemName { get; set; }         
           public string CreatedBy { get; set; }
           public DateTime CreatedDate { get; set; }
           public List<BrandItemModel> AllocateItemList { get; set; }
           public List<BrandItemModel> UnAllocateItemList { get; set; }
       }


       public class BrandVendorModel
       {
           public int BrandId { get; set; }
           public string BrandName { get; set; }
           public int VendorId { get; set; }
           public string VendorName { get; set; }
           public string CreatedBy { get; set; }
           public DateTime CreatedDate { get; set; }
           public List<BrandVendorModel> AllocateVendorList { get; set; }
           public List<BrandVendorModel> UnAllocateVendorList { get; set; }
       }

        #region Bank Cheque
           public   class BankChequeModel
           {
               public int ChequeId { get; set; }
               public int BankId { get; set; }
               public int Status { get; set; }
               public string ChequeNumber { get; set; }
               public string CompCode { get; set; }
               public DateTime ChequeDate { get; set; }
               public string ChequeDateStr { get; set; }
               public string ChequeDocument { get; set; }
               public int IssuedToId { get; set; }
               public string IssuedToType { get; set; }
               public DateTime IssuedDate { get; set; }
               public string IssuedToOther { get; set; }
               public double Amount { get; set; }
               public string Remark { get; set; }
               public string Currency { get; set; }
               public DateTime ClearingDate { get; set; }
               public string CreatedBy { get; set; }
               public DateTime CreatedDate { get; set; }
               public string ModifiedBy { get; set; }
               public DateTime ModifiedDate { get; set; }

               public string BankName { get; set; }
               public string IssuedToName { get; set; }
           }
        #endregion Bank Cheque

           #region  Inward Document
           public class InwardDocumentModel
           {
               public int DocId { get; set; }
               public int STLocationId { get; set; }
               public string StkLocName { get; set; }
                public string DocName { get; set; }         
               public string NatureOfDoc { get; set; }
               public string CompCode { get; set; }
               public string NatureOfDocOther { get; set; }         
               public int DocToId { get; set; }
               public string DocToName { get; set; }
               public string DocToType { get; set; }
               public DateTime DocDate { get; set; }
               public string DocDateStr { get; set; }
               public string DocToOther { get; set; }
               public double Amount { get; set; }            
               public string Currency { get; set; }          
               public string CreatedBy { get; set; }
               public DateTime CreatedDate { get; set; }
               public string ModifiedBy { get; set; }
               public DateTime ModifiedDate { get; set; }
               public List<InwardDocumentDetailModel> DocDetailList { get; set; }
           }
           public class InwardDocumentDetailModel
           {
               public int DocId { get; set; }
               public string FileName { get; set; }
               public string DocumentTitle { get; set; }
               public string CreatedBy { get; set; }
               public DateTime CreatedDate { get; set; }             
           }
           #endregion Inward Document
           #region TaxOnTax
           public class TaxModel
           {
               public int TaxId { get; set; }
               public string TaxName { get; set; }
               public string TaxType { get; set; }
               public double TaxRate { get; set; }
               public double TaxAmount { get; set; }
               public string ParentTaxName { get; set; }
               public List<TaxOnTax> TaxOnTaxList { get; set; }
               public List<TaxOnTaxGrid> TaxGrid { get; set; }
               public List<TaxModel> TaxList { get; set; }

           }
           public class TaxTypeModel
           {
               public int TaxType { get; set; }
               public string TaxTypeTitle { get; set; }
           }
           public class TaxOnTax
           {
               public int TaxId { get; set; }
               public int ChildTaxId { get; set; }
               public string TaxName { get; set; }
               public string ParentTaxName { get; set; }
               public int TaxType { get; set; }
           }
           public class TaxOnTaxGrid
           {
               public int TaxId { get; set; }
               public string TaxName { get; set; }
               public string TaxTypeId { get; set; }
               public int TaxOnTaxId { get; set; }
               public string TaxType { get; set; }
               public string TaxOnTax { get; set; }
           }
       #endregion TaxOnTax
}
