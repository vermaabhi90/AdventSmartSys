using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.Bank
{
    public class BankDetailModel
    {
        public int BankId { get; set; }
        public string IFSCCode { get; set; }
        public string EmpBankName { get; set; }
        public string BankName { get; set; }
        public int AddressId { get; set; }
        public Boolean isDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int Countryid { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string Country { get; set; }
        public string Pin { get; set; }
        public string LandMark { get; set; }
        public List<BankDetailModel> BankDetailList { get; set; }
        public List<AddressModel> AddressModelList { get; set; }
        public List<CompanyCodeModel> ComList { get; set; }
        public List<BankCountryModel> countryLst { get; set; }
        public List<BankStateModel> stateLst { get; set; }
        public List<BankCityModel> cityLst { get; set; }
    }

    public class CompanyCodeModel
    {
        public string CompCode { get; set; }
        public string CompanyName { get; set; }

    }

    public class AddressModel
    {
        public int AddressId { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public string Pin { get; set; }
        public string LandMark { get; set; }

    }


    public class BankCountryModel
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
    public class BankStateModel
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
    public class BankCityModel
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
    #region CompanyBankDetail
    public class CompanyBankModel
    {
        public int CompBankAcId { get; set; }
        public string CompCode { get; set; }
        public string BankName { get; set; }
             public string Active { get; set; }
        public int BankId { get; set; }
        public string AccountNumber { get; set; }
        public string Remark { get; set; }
        public Boolean isActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
       
    }
    #endregion CompanyBankDetail

}
