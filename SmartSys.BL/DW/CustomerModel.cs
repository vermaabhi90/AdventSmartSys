using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.DW
{

    public class CustomerListModel
    {
        [DisplayName("Customer Id")]
        public int CustomerId { get; set; }

        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }

        [DisplayName("Region")]
        public string Region { get; set; }

        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("Country")]
        public string Country { get; set; }
        public string State { get; set; }

        [DisplayName("Email Id")]
        public string emailId { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        public string SalesPersonName { get; set; }
        public string CreatedByName { get; set; }
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }
        [DisplayName("Authorized Dealer")]
        public bool AuthorizedDealer { get; set; }
       
        public DateTime ModifiedDate { get; set; }
        public Int16 CompRelMapCount { get; set; }
        public string ModifiedByName { get; set; }
        public string Description { get; set; }
        public string SAJCustomer_No { get; set; }
        public string DPKCustomer_No { get; set; }
        public string ADVENTCustomer_No { get; set; }
        public string BOMCustomer_No { get; set; }
        public string TimeSheetCustomer_No { get; set; }
        public Int16 SAJMapStatus { get; set; }
        public Int16 DPKMapStatus { get; set; }
        public Int16 ADVENTMapStatus { get; set; }
        public Int16 BOMMapStatus { get; set; }
        public Int16 TimeSheetMapStatus { get; set; }
        public bool isSales { get; set; }
        public bool isPurchase { get; set; }
        public List<CustomerListModel> customerGedList { get; set; }
        
    }
    public class ActiveCompanyModel
    {
        public string CompCode { get; set; }
        public string CompName { get; set; }
        public string CompTemplet { get; set; }
    }
   public  class CustomerModel
    {
       [DisplayName("Customer Id")]
        public int CustomerId { get; set; }
        public int PTId { get; set; }
        public int Count { get; set; }
 
       [DisplayName("Customer Name")]
        public string CustomerName { get; set; }

       [DisplayName("Region")]
        public string Region { get; set; }

       [DisplayName("City")]
        public string City { get; set; }
       public string Description { get; set; }

       [DisplayName("Country")]
        public string Country { get; set; }
        public string State { get; set; }

       [DisplayName("Email Id")]
        public string emailId { get; set; }

       [DisplayName("Is Active")]
        public bool IsActive { get; set; }

       [DisplayName("Created By")]
        public int CreatedBy { get; set; }
       public string CreatedByName { get; set; }
       [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

       [DisplayName("Modified By")]
        public int ModifiedBy { get; set; }      

       [DisplayName("Modified Date")]
       public DateTime ModifiedDate { get; set; }
       [DisplayName("Authorized Dealer")]
       public bool AuthorizedDealer { get; set; }
        public Int16 CompRelMapCount { get; set; }
        public string ModifiedByName { get; set; }
        public string SAJCustomer_No { get; set; }
        public string DPKCustomer_No { get; set; }
        public string ADVENTCustomer_No { get; set; }
        public string BOMCustomer_No { get; set; }
        public string TimeSheetCustomer_No { get; set; }
        public Int16 SAJMapStatus { get; set; }
        public Int16 DPKMapStatus { get; set; }
        public Int16 ADVENTMapStatus { get; set; }
        public Int16 BOMMapStatus { get; set; }
        public Int16 TimeSheetMapStatus { get; set; }
        public string VAT { get; set; }
        public int LevelId { get; set; }
        public string CST { get; set; }
        public string ExciseNo { get; set; }
        public string ExciseRange { get; set; }
        public string ExciseDivision { get; set; }
        public string ExciseCommissionRate { get; set; }
        public string Website { get; set; }
        public string Remark { get; set; }
        public string PAN { get; set; }
        public string TAN { get; set; }
        public int TabIndex { get; set; }
        public int SalesPersonId { get; set; }
        public string SalesPersonName { get; set; }
        public bool CommunicateToCSR { get; set; }
        public List<CustomerModel> customerModelList { get; set;}
        public List<CustomerBankDetailModel> BankDetailLst { get; set; }
        public List<CustomerContactDetailsModel> CustmerContactLst { get; set; }
        public List<AddressModel> AddressList { get; set; }
        public List<CustomerLibaryModel> CustomerLibaryList { get; set; }
        public List<CustomerLibaryModel> CustomerKYCList { get; set; }
        public List<CustomerTurnoverModel> CustomerTurnoverList { get; set; }
        public List<CustomerCertificationModel> CustomerCertificationList { get; set; }
        public List<CustomerCompetitorModel> CustomerCompetitorList { get; set; }
        public List<CustomerCompetitorModel> CompetitorCustList { get; set; }
        public List<ProductModel> ProductList { get; set; }
        public List<LevelModel> LevelList { get; set; }
    }
    public class LevelModel
    {
        public int LevelId { get; set; }
        public string Description { get; set; }
    }
   public class ProductModel
   {
       public int EquipmentId { get; set; }
       public int CustomerId { get; set; }
       public string EquipmentName { get; set; }
       public string CustomerName { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int TAM { get; set; }
        public List<ProductModel> ItemList { get; set; }
   }
   public class Itemmodel
   {
       public int ItemId { get; set; }
       public string ItemName { get; set; }
       public double Quantity { get; set; }
   }
    public class CustUserModel
    {
        public int CustomerId { get; set; }
        public int CustomerContactId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }       
        public List<CustUserModel> LstUserModel { get; set; }
    }
   public class CustomerCompetitorModel
   {
       public int CustomerId { get; set; }
       public int CompetitorId { get; set; }
       public string CompetitorName { get; set; }
       public string CustomerName { get; set; }
       public int NewCompetitorId { get; set; }
       public string Region { get; set; }
       public bool IsActive { get; set; }
       public string CreatedBy { get; set; }
       public DateTime CreatedDate { get; set; }
   }
   public class CustomerCertificationModel
   {
       public int CustomerId { get; set; }
       public string CustomerCertification { get; set; }
       public string NewCustomerCertification { get; set; }
       public DateTime CertificateDate { get; set; }
       public string CertificateDateStr { get; set; }     
       public string CreatedBy { get; set; }
       public DateTime CreatedDate { get; set; }
   }
    public class CustomerLibaryModel
    {
        public int CustomerId { get; set; }
        public string isKYC { get; set; }
        public Boolean KYC { get; set; }
        public string DocumentTitle { get; set; }
        public string Description { get; set; }
        public string DocumentPath { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class CustomerTurnoverModel
    {
        public int CustomerId { get; set; }
        public string TurnoverYear { get; set; }
        [Required(ErrorMessage = "This field can not be empty.")]
        public string NewTurnoverYear { get; set; }
        public double Turnover { get; set; }
        public double ProjectedTurnover { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class CPNModel
    {
        public int CustomerId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public string CPN { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class AddressModel
   {      
       public int AddressId { get; set; }
       public int CustomerId { get; set; }
       public string Line1 { get; set; }
       public string Line2 { get; set; }
       public string Pin { get; set; }

       public int CountryId { get; set; }
       public int StateId { get; set; }
       public int CityId { get; set; }
       public string LandMark { get; set; }
       public string Country { get; set; }
       public string State { get; set; }
       public Boolean isPrimary { get; set; }
       public string Description { get; set; }
       public string City { get; set; }
       public string PhotoPath { get; set; }
       public byte[] Photobyte { get; set; }
       public List<CountryModel> LstCountry { get; set; }
       public List<StateModel> LstState { get; set; }
       public List<CityModel> LstCity { get; set; }
   }
   public class CountryModel
   {
       public string CountryId { get; set; }
       public string CountryName { get; set; }
   }
   public class StateModel
   {
       public string StateId { get; set; }
       public string StateName { get; set; }
   }
   public class CityModel
   {
       public string CityId { get; set; }
       public string CityName { get; set; }
   }
    public class CustomerContactDetailsModel
    {
        public int CustomerContactId { get; set; }
        public int CustomerId { get; set; }
        public string ContactName { get; set; }
        public string  NewContactName { get; set; }
        public string Designation { get; set; }
        public string email { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Experties { get; set; }
        public string Qualification { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthDateStr { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Remark { get; set; }
    }
    public class CustomerBankDetailModel
    {
        public int CustomerId { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string NewAccountNo { get; set; }
        public double Limit { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
   public class CustomerMappingModel
   {
       public string CompCode { get; set; }
       public string CompName { get; set; }
       public string CompCustomerID { get; set; }
       public Int16 ConnectionId { get; set; }
       public string VendorListSP { get; set; }
       public string SaveCustomerSP { get; set; }
       public int CustomerId { get; set; }
       public string CustomerName { get; set; }
       public string selectedComp { get; set; }
       public List<CustomerMappingModel> lstCustomerMap { get; set; }
       public List<SysNavRelCustomerListModel> GridList { get; set; }
       public List<MapClientCustomerList> lstClientMapCustomerList { get; set; }
       public string AutoMapBtn { get; set; }
   }
   public class MapClientCustomerList
   {
       public string ClientCustomerID { get; set; }
       public string ClientCustomerName { get; set; }
   }
   public class SysNavRelCustomerListModel
   {
       public string CompCustomerID { get; set; }
       public string CompName { get; set; }
       public int CustomerId { get; set; }
       public string CompCustomerName { get; set; }
       public Int16 ConnectionId { get; set; }
   }

   public class PenndingCustomerApproverMap
   {
       [Display(Name = "Customer Id")]
       public int CustomerId { get; set; }

       [Display(Name = "Customer Name")]
       public String CustomerName { get; set; }

       [Display(Name = "External Customer No")]
       public String Customer_No { get; set; }

       [Display(Name = "External Customer Name")]
       public String SourceCustomerName { get; set; }

       [Display(Name = "Company Code")]
       public String CompCode { get; set; }

       [Display(Name = "Modified By")]
       public String ModifiedBy { get; set; }

       [Display(Name = "Modified Date")]
       public DateTime ModifiedDate { get; set; }
       public List<PenndingCustomerApproverMap> lstApprover { get; set; }
       public List<SelectPenddingCustomerApproveList> SelectApproverLst { get; set; }
   }
   public class SelectPenddingCustomerApproveList
   {
       public int CustomerId { get; set; }
       public String CustomerName { get; set; }
       public String Customer_No { get; set; }
       public String SourceCustomerName { get; set; }
       public string CompName { get; set; }
       public string ModifiedBy { get; set; }
   }
}
