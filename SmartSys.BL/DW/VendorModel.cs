using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.DW
{

    public class VendorListModel
    {
        [DisplayName("Vendor Id")]
        public int VendorId { get; set; }

        [DisplayName("Vendor Name")]
        public string VendorName { get; set; }

        [DisplayName("Region")]
        public string Region { get; set; }

        [DisplayName("City")]
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        [DisplayName("Email Id")]
        public string emailId { get; set; }

        [DisplayName("Is Customer")]
        public bool IsCustomer { get; set; }

        [DisplayName("Is Supplier")]
        public bool IsSupplier { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        public bool IsManufacturer { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }        

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }
        [DisplayName("Authorized Dealer")]
        public bool AuthorizedDealer { get; set; }
        public bool AllowBrand { get; set; }
        public Int16 CompRelMapCount { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public string SAJVendor_No { get; set; }
        public string DPKVendor_No { get; set; }
        public string ADVENTVendor_No { get; set; }
        public string BOMVendor_No { get; set; }
        public string TimeSheetVendor_No { get; set; }
        public Int16 SAJMapStatus { get; set; }
        public Int16 DPKMapStatus { get; set; }
        public Int16 ADVENTMapStatus { get; set; }
        public Int16 BOMMapStatus { get; set; }
        public Int16 TimeSheetMapStatus { get; set; }
        public bool isSales { get; set; }
        public bool isPurchase { get; set; }
        public List<VendorListModel> vendorGedList { get; set; }
    }
    public class VendorModel
    {
        [DisplayName("Vendor Id")]
        public int VendorId { get; set; }

        [DisplayName("Vendor Name")]
        public string VendorName { get; set; }

        [DisplayName("Region")]
        public string Region { get; set; }

        [DisplayName("City")]
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        [DisplayName("Email Id")]
        public string emailId { get; set; }

        [DisplayName("Is Customer")]
        public bool IsCustomer { get; set; }

        [DisplayName("Is Supplier")]
        public bool IsSupplier { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        public bool IsManufacturer { get; set; }

        [DisplayName("Created By")]
        public int CreatedBy { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Modified By")]
        public int ModifiedBy { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }
        [DisplayName("Authorized Dealer")]
        public bool AuthorizedDealer { get; set; }
        public Int16 CompRelMapCount { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public string SAJVendor_No { get; set; }
        public string DPKVendor_No { get; set; }
        public string ADVENTVendor_No { get; set; }
        public string BOMVendor_No { get; set; }
        public string TimeSheetVendor_No { get; set; }
        public Int16 SAJMapStatus { get; set; }
        public Int16 DPKMapStatus { get; set; }
        public Int16 ADVENTMapStatus { get; set; }
        public Int16 BOMMapStatus { get; set; }
        public Int16 TimeSheetMapStatus { get; set; }
        public string VAT { get; set; }
        public string CST { get; set; }
        public string ExciseNo { get; set; }
        public string ExciseRange { get; set; }
        public string ExciseDivision { get; set; }
        public string ExciseCommissionRate { get; set; }
        public string PAN { get; set; }
        public string TAN { get; set; }
        public string Remark { get; set; }
        public string Website { get; set; }
        public int TabIndex { get; set; }
        public List<VendorModel> vendorModelList { get; set; }
        public List<VendorBankDetailModel> VendorBankDetailLst { get; set; }
        public List<VendorContactDetailsModel> VendorContactLst { get; set; }
        public List<VendorLibaryModel> VendorLibaryList { get; set; }
        public List<VendorLibaryModel> VendorKYCList { get; set; }
        public List<VendorTurnoverModel> VendorTurnoverList { get; set; }
        public List<VendorCertificationModel> VendorCertificationList { get; set; }
        public List<VendorCompetitorModel> VendorCompetitorList { get; set; }
        public List<VendorCompetitorModel> VendorCompetitorCustList { get; set; }
        public List<VendorAddressModel> AddressList { get; set; }
        public List<VendorProductModel> ProductList { get; set; }       

    }
    public class VendorContactDetailsModel
    {
        public int VendorContactId { get; set; }
        public int VendorId { get; set; }
        public string ContactName { get; set; }
        public string NewContactName { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string PhoneNo { get; set; }
        public string Experties { get; set; }
        public string Email { get; set; }
        public string Qualification { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthDateStr { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Remark { get; set; }
        public int MyProperty { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

    }
    public class VendorBankDetailModel
    {
        public int VendorId { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string NewAccountNo { get; set; }
        public double Limit { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class VendorCompetitorModel
    {
        public int VendorId { get; set; }
        public int CompetitorId { get; set; }
        public string CompetitorName { get; set; }
        public string CustomerName { get; set; }
        public int NewCompetitorId { get; set; }
        public string Region { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class VendorCertificationModel
    {
        public int VendorId { get; set; }
        public string VendorCertification { get; set; }
        public string NewVendorCertification { get; set; }
        public DateTime CertificateDate { get; set; }
        public string CertificateDateStr { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class VendorLibaryModel
    {
        public int VendorId { get; set; }
        public string DocumentTitle { get; set; }
        public string isKYC { get; set; }
        public Boolean KYC { get; set; }
        public string Description { get; set; }
        public string DocumentPath { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class VendorTurnoverModel
    {
        public int VendorId { get; set; }
        public string TurnoverYear { get; set; }
        [Required(ErrorMessage = "This field can not be empty.")]
        public string NewTurnoverYear { get; set; }
        public double Turnover { get; set; }
        public double ProjectedTurnover { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class VendorAddressModel
    {
        public int AddressId { get; set; }
        public int VendorId { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Pin { get; set; }
        public string LandMark { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public Boolean isPrimary { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public List<CountryModel> LstCountry { get; set; }
        public List<StateModel> LstState { get; set; }
        public List<CityModel> LstCity { get; set; }
    }
    public class VendorCountryModel
    {
        public string CountryId { get; set; }
        public string CountryName { get; set; }
    }
    public class VendorStateModel
    {
        public string StateId { get; set; }
        public string StateName { get; set; }
    }
    public class VendorCityModel
    {
        public string CityId { get; set; }
        public string CityName { get; set; }
    }
    public class VendorProductModel
    {
        public int EquipmentId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string EquipmentName { get; set; }
        public List<VendorProductModel> ItemList { get; set; }
    }
    public class VendorItemmodel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
    }

    public class VendorMappingModel
    {
        public string CompCode { get; set; }
        public string CompName { get; set; }
        public string CompVendorID { get; set; }
        public Int16 ConnectionId { get; set; }
        public string VendorListSP { get; set; }
        public string SaveVendorSP { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string selectedComp { get; set; }
        public List<VendorMappingModel> lstVendorMap { get; set; }
        public List<SysNavRelVendorListModel> GridList { get; set; }
        public List<MapClientVendorList> lstClientMapVendorList { get; set; }
        public string AutoMapBtn { get; set; }
    }
    public class MapClientVendorList
    {
        public string ClientVendorID { get; set; }
        public string ClientVendorName { get; set; }
    }
    public class SysNavRelVendorListModel
    {
        public string CompVendorID { get; set; }
        public string CompName { get; set; }
        public int VendorId { get; set; }
        public string CompVendorName { get; set; }
        public Int16 ConnectionId { get; set; }
    }
    public class PenndingVendorApproverMap
    {
        [Display(Name = "Vendor Id")]
        public int VendorId { get; set; }

        [Display(Name = "Vendor Name")]
        public String VendorName { get; set; }

        [Display(Name = "External Vendor No")]
        public String Vendor_No { get; set; }

        [Display(Name = "External Vender Name")]
        public String SourceVenderName { get; set; }

        [Display(Name = "Company Code")]
        public String CompCode { get; set; }

        [Display(Name = "Modified By")]
        public String ModifiedBy { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
        public List<PenndingVendorApproverMap> lstApprover { get; set; }
        public List<SelectPenddingVendorApproveList> SelectApproverLst { get; set; }
    }
    public class SelectPenddingVendorApproveList
    {
        public int VendorId { get; set; }
        public String VendorName { get; set; }
        public String Vendor_No { get; set; }
        public String SourceVenderName { get; set; }
        public string CompName { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class VendorUserModel
    {
        public int VendorId { get; set; }
        public int VendorContactId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<VendorUserModel> LstUserModel { get; set; }
    }

    public class FreightForwarderModel
    {
        [DisplayName("FreightForwarder Id")]
        public int FFId { get; set; }

        [DisplayName("Vendor Name")]
        public string VendorName { get; set; }

        [DisplayName("Region")]
        public string Region { get; set; }

        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public int VendorId { get; set; }
        public string Website { get; set; }

    }
    public class VendoridModel
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }
    }
    public class VendorBrandAllocateModel
    {
        public int vendorId { get; set; }
        public string VendorName { get; set; }
        public int  BrandId { get; set; }
        public string BrandName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<VendorBrandAllocateModel> AllocateBrand { get; set; }
        public List<VendorBrandAllocateModel> UnAllocateBrand { get; set; }
    }
}
