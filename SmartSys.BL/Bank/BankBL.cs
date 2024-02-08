using SmartSys.DAL;
using SmartSys.DL.Bank;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.Bank
{
    public class BankBL
    {
        public List<BankDetailModel> bankDetailList(string UserId)
        {
            BankDetailModel objmodel = new BankDetailModel();
            objmodel.BankDetailList = new List<BankDetailModel>();

            try
            {
                DataSet ds = new DataSet();
                BankDAL obj = new BankDAL();
                ds = obj.BankDetailsList(UserId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BankDetailModel model = new BankDetailModel();
                            model.BankId = Convert.ToInt32(dr["BankId"].ToString());
                            model.BankName = dr["BankName"].ToString();
                            model.EmpBankName = dr["EmpBankName"].ToString();
                            model.IFSCCode = dr["IFSCCode"].ToString();
                            model.AddressId = Convert.ToInt32(dr["AddressId"].ToString());
                          //  model.CompanyName = dr["CompanyName"].ToString();
                            //   model.isDeleted = Convert.ToBoolean(dr["BankId"].ToString());
                            model.CreatedBy = dr["CreatedBy"].ToString();
                            model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            model.ModifiedBy = dr["ModifiedBy"].ToString();
                            model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            objmodel.BankDetailList.Add(model);
                        }
                    }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objmodel.BankDetailList;
        }

       public BankDetailModel GetSelectedBankDetails(int BankId)
        {
            BankDetailModel objmodel = new BankDetailModel();
            try
            {
                DataSet ds = new DataSet();


                BankDAL objdal = new BankDAL();
                ds = objdal.GetSelectedBankDetail(BankId);
                if (ds != null)
                {


                    objmodel.BankId = Convert.ToInt32(ds.Tables[0].Rows[0]["BankId"].ToString());
                    objmodel.BankName = ds.Tables[0].Rows[0]["BankName"].ToString();
                    objmodel.IFSCCode = ds.Tables[0].Rows[0]["IFSCCode"].ToString();
                    objmodel.AddressId = Convert.ToInt32(ds.Tables[0].Rows[0]["AddressId"].ToString());
                    objmodel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                    objmodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                    objmodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                    objmodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());


                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objmodel;
        }
        public List<AddressModel> GetSelectedAddressList(int AddressId)
        {          
            List<AddressModel> objmodel = new List<AddressModel>();
            try
            {
                DataSet ds = new DataSet();

                BankDAL objdal = new BankDAL();
                ds = objdal.GetSelectedAddressList(AddressId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            AddressModel obj = new AddressModel();
                            obj.AddressId = Convert.ToInt32(dr["AddressId"].ToString());
                            obj.Line1 = dr["Line1"].ToString();
                            obj.Line2 = dr["Line2"].ToString();
                            obj.StateId = Convert.ToInt32(dr["StateId"].ToString());
                            obj.CountryId = Convert.ToInt32(dr["CountryId"].ToString());
                            obj.CityId = Convert.ToInt32(dr["CityId"].ToString());
                            obj.State = dr["State"].ToString();
                            obj.City = dr["City"].ToString();
                            obj.Country = dr["Country"].ToString();
                            obj.Pin = dr["Pin"].ToString();
                            obj.LandMark = dr["LandMark"].ToString();
                            objmodel.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objmodel;
        }

        public int SaveBankDetails(BankDetailModel objmodel, string User_Id)
        {
            int errcode = 0;
            try
            {
                DataSet dsAddress = new DataSet();

                BankDAL objdal = new BankDAL();

                dsAddress = objdal.GetSelectedAddressList(0);
                DataSet DSBank = new DataSet();
                DSBank = objdal.GetSelectedBankDetail(0);


                if (dsAddress == null)
                {
                    return 0;
                }
                if (dsAddress != null)
                {
                    dsAddress.Tables[0].Rows.Clear();
                    DataRow dr = dsAddress.Tables[0].NewRow();
                    dr["AddressId"] = objmodel.AddressId;
                    dr["Line1"] = objmodel.Line1;
                    dr["Line2"] = objmodel.Line2;
                    dr["StateId"] = objmodel.StateId;
                    dr["CityId"] = objmodel.CityId;
                    dr["CountryId"] = objmodel.Countryid;
                    dr["Pin"] = objmodel.Pin;
                    dr["LandMark"] = objmodel.LandMark;
                    dsAddress.Tables[0].Rows.Add(dr);

                    DSBank.Tables[0].Rows.Clear();
                    DataRow dr1 = DSBank.Tables[0].NewRow();
                    dr1["BankId"] = objmodel.BankId;
                    dr1["BankName"] = objmodel.BankName;
                    dr1["IFSCCode"] = objmodel.IFSCCode;




                    DSBank.Tables[0].Rows.Add(dr1);


                }

                errcode = objdal.SaveAddressDetails(dsAddress, DSBank, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }

        public int SaveEditedBankDetails(BankDetailModel objmodel, string User_Id)
        {
            int errcode = 0;
            try
            {

                DataSet ds = new DataSet();
                BankDAL objdal = new BankDAL();
                ds = objdal.GetSelectedBankDetail(0);
                if (ds != null)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["BankId"] = objmodel.BankId;
                    dr["BankName"] = objmodel.BankName;
                    dr["IFSCCode"] = objmodel.IFSCCode;
                    dr["AddressId"] = objmodel.AddressId;
                    ds.Tables[0].Rows.Add(dr);
                }

                errcode = objdal.SaveEditedBankDetails(ds, User_Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }

        public int SaveEditedAddressDetails(BankDetailModel objmodel,int Bankid)
        {
            int errcode = 0;
            try
            {

                DataSet ds = new DataSet();
                BankDAL objdal = new BankDAL();
                ds = objdal.GetSelectedAddressList(0);
                if (ds != null)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["AddressId"] = objmodel.AddressId;
                    dr["Line1"] = objmodel.Line1;
                    dr["Line2"] = objmodel.Line2;
                    dr["StateId"] = objmodel.StateId;
                    dr["CityId"] = objmodel.CityId;
                    dr["CountryId"] = objmodel.Countryid;
                    dr["Pin"] = objmodel.Pin;
                    dr["LandMark"] = objmodel.LandMark;
                    ds.Tables[0].Rows.Add(dr);
                }

                errcode = objdal.SaveEditedAddressDetails(ds, Bankid);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }

        public int DeleteBankDetails(int BankId,string User_Id)
        {
            int errcode = 0;
            try
            {
                BankDAL objdal = new BankDAL();
                errcode = objdal.DeleteBankDetails(BankId, User_Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }


        public BankDetailModel GetSelectedAddressDetails(int AddressId)
        {
            BankDetailModel objmodel = new BankDetailModel();
            try
            {
                DataSet ds = new DataSet();


                BankDAL objdal = new BankDAL();
                ds = objdal.GetSelectedAddressList(AddressId);
                if (ds != null)
                {
                    objmodel.AddressId = Convert.ToInt32(ds.Tables[0].Rows[0]["AddressId"].ToString());
                    objmodel.Line1 = ds.Tables[0].Rows[0]["Line1"].ToString();
                    objmodel.Line2 = ds.Tables[0].Rows[0]["Line2"].ToString();
                    objmodel.CityId = Convert.ToInt32(ds.Tables[0].Rows[0]["CityId"].ToString());
                    objmodel.StateId = Convert.ToInt32(ds.Tables[0].Rows[0]["StateId"].ToString());
                    objmodel.Countryid = Convert.ToInt32(ds.Tables[0].Rows[0]["Countryid"].ToString());
                    objmodel.State = ds.Tables[0].Rows[0]["State"].ToString();
                    objmodel.City = ds.Tables[0].Rows[0]["City"].ToString();
                    objmodel.Country = ds.Tables[0].Rows[0]["Country"].ToString();
                    objmodel.Pin = ds.Tables[0].Rows[0]["Pin"].ToString();
                    objmodel.LandMark = ds.Tables[0].Rows[0]["LandMark"].ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objmodel;
        }

        public List<BankCountryModel> GetCountryList()
        {
            BankDetailModel addressModel = new BankDetailModel();
            addressModel.countryLst = new List<BankCountryModel>();
            try
            {
                BankDAL DALObj = new BankDAL();
                DataSet DSObj = new DataSet();
                DSObj = DALObj.GetCountryList();
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in DSObj.Tables[0].Rows)
                        {
                            BankCountryModel demo = new BankCountryModel();
                            demo.CountryId =Convert.ToInt32(dr["Country ID"].ToString());
                            demo.CountryName = dr["Country Name"].ToString();
                            addressModel.countryLst.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Common.LogException(ex);
                throw ex;
            }
            return addressModel.countryLst;
        }
        public List<BankStateModel> GetStateList(int CountryId)
        {
            BankDetailModel addressModel = new BankDetailModel();
            addressModel.stateLst = new List<BankStateModel>();

            try
            {
                AdminDal DALObj = new AdminDal();
                DataSet DSObj = new DataSet();
                DSObj = DALObj.GetStateList(CountryId);
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in DSObj.Tables[0].Rows)
                        {
                            BankStateModel demo = new BankStateModel();
                            demo.StateId =Convert.ToInt32(dr["State ID"].ToString());
                            demo.StateName = dr["State Name"].ToString();
                            addressModel.stateLst.Add(demo);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Common.LogException(ex);
                throw ex;
            }
            return addressModel.stateLst;
        }
        public List<BankCityModel> GetCityList(int State)
        {
            BankDetailModel addressModel = new BankDetailModel();

            addressModel.cityLst = new List<BankCityModel>();
            try
            {
                AdminDal DALObj = new AdminDal();
                DataSet DSObj = new DataSet();
                DSObj = DALObj.GetCityList(State);
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in DSObj.Tables[0].Rows)
                        {
                            BankCityModel demo = new BankCityModel();
                            demo.CityId =Convert.ToInt32( dr["City ID"].ToString());
                            demo.CityName = dr["City Name"].ToString();
                            addressModel.cityLst.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //  Common.LogException(ex); 
                throw ex;
            }
            return addressModel.cityLst;
        }

        #region CompanyBankDetail
        public List<CompanyBankModel> CompanyBankList()
        {
            List<CompanyBankModel> CompBnkLst = new List<CompanyBankModel>();
           try
            {
                DataSet ds = new DataSet();
                BankDAL obj = new BankDAL();
                ds = obj.GetCompanyBankDetailList();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CompanyBankModel model = new CompanyBankModel();
                            model.CompBankAcId = Convert.ToInt32(dr["CompBankAcId"].ToString());
                            model.BankName =dr["BankName"].ToString();
                            model.CompCode = dr["CompCode"].ToString();
                            model.AccountNumber = dr["AccountNumber"].ToString();
                            model.Remark = dr["Remark"].ToString();
                            model.isActive = Convert.ToBoolean(dr["isActive"].ToString());
                            if (model.isActive)
                                model.Active = "True";
                            else
                                model.Active = "false";


                            model.CreatedBy = dr["CreatedBy"].ToString();
                            model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            model.ModifiedBy = dr["ModifiedBy"].ToString();
                            model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            CompBnkLst.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CompBnkLst;
        }

        public CompanyBankModel GetSelectedCompanyBankDetails(int CompBankAcId)
        {
            CompanyBankModel objmodel = new CompanyBankModel();
            try
            {
                DataSet ds = new DataSet();
                BankDAL objdal = new BankDAL();
                ds = objdal.GetSelectedCompanyBankDetail(CompBankAcId);
                if (ds != null)
                {
                    objmodel.BankId = Convert.ToInt32(ds.Tables[0].Rows[0]["BankId"].ToString());
                    objmodel.CompBankAcId = Convert.ToInt32(ds.Tables[0].Rows[0]["CompBankAcId"].ToString());
                    objmodel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                    objmodel.AccountNumber = ds.Tables[0].Rows[0]["AccountNumber"].ToString();
                    objmodel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                    objmodel.isActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["isActive"].ToString());
                    objmodel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                    objmodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                    objmodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                    objmodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objmodel;
        }
        public int SaveCompanyBankDetails(CompanyBankModel objmodel, string User_Id)
        {
            int errcode = 0;
            try
            {

                DataSet ds = new DataSet();
                BankDAL objdal = new BankDAL();
                ds = objdal.GetSelectedCompanyBankDetail(0);
                if (ds != null)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["CompBankAcId"] = objmodel.CompBankAcId;
                    dr["CompCode"] = objmodel.CompCode;
                    dr["BankId"] = objmodel.BankId;
                    dr["AccountNumber"] = objmodel.AccountNumber;
                    dr["Remark"] = objmodel.Remark;
                    dr["isActive"] = objmodel.isActive;
                    ds.Tables[0].Rows.Add(dr);
                }

                errcode = objdal.SaveCompanyBankDetail(ds, User_Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }
        #endregion CompanyBankDetail
    }
}
