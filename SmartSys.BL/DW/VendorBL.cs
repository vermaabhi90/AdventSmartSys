using SmartSys.BL.RGS;
using SmartSys.DAL;
using SmartSys.DL.DW;
using SmartSys.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.DW
{
    public class VendorBL
    {
        public List<VendorListModel> GetVendorList(string User_Id)
        {
            List<VendorListModel> lstVendorModel = new List<VendorListModel>();
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.VendorList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            VendorListModel objmodel = new VendorListModel();
                            objmodel.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            objmodel.VendorName = dr["VendorName"].ToString();
                            objmodel.Region = dr["Region"].ToString();
                            objmodel.emailId = dr["emailId"].ToString();
                            objmodel.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                            objmodel.IsManufacturer = Convert.ToBoolean(dr["IsManufacturer"].ToString());
                            objmodel.ModifiedByName = dr["ModifiedByName"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                            //objmodel.SAJVendor_No = dr["SAJVendor_No"].ToString();
                            //objmodel.DPKVendor_No = dr["DPKVendor_No"].ToString();
                            //objmodel.ADVENTVendor_No = dr["ADVENTVendor_No"].ToString();
                            //objmodel.BOMVendor_No = dr["BOMVendor_No"].ToString();
                            //objmodel.TimeSheetVendor_No = dr["TimeSheetVendor_No"].ToString();
                            //objmodel.SAJMapStatus = Convert.ToInt16(dr["SAJMapStatus"].ToString());
                            //objmodel.DPKMapStatus = Convert.ToInt16(dr["DPKMapStatus"].ToString());
                            //objmodel.ADVENTMapStatus = Convert.ToInt16(dr["ADVENTMapStatus"].ToString());
                            //objmodel.BOMMapStatus = Convert.ToInt16(dr["BOMMapStatus"].ToString());
                            //objmodel.TimeSheetMapStatus = Convert.ToInt16(dr["TimeSheetMapStatus"].ToString());
                            objmodel.AuthorizedDealer = Convert.ToBoolean(dr["AuthorizedDealer"].ToString());
                            lstVendorModel.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstVendorModel;
        }
        public bool CheckVendorDuplicacy(string VendorName,string User_Id)
        {
            bool Result = true;
            VendorDAL objdal = new VendorDAL();
            DataSet ds = new DataSet();
            ds = objdal.VendorList(User_Id);
            DataColumn[] columns = ds.Tables[0].Columns.Cast<DataColumn>().ToArray();
            Result = ds.Tables[0].AsEnumerable()
                .Any(row => columns.Any(col => row[col].ToString() == VendorName));
            return Result;
        }
        public List<VendorListModel> GetVendorListHavingContacts(int ProjectId,string User_Id)
        {
            List<VendorListModel> lstVendorModel = new List<VendorListModel>();
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.VendorListHavingVendorContacts(ProjectId, User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                { 
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            VendorListModel objmodel = new VendorListModel();
                            objmodel.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            objmodel.VendorName = dr["VendorName"].ToString();
                            lstVendorModel.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstVendorModel;
        }
        public int saveVendor(VendorModel vendorModel, string User_Id)
        {

            VendorDAL objdal = new VendorDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedVendor(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow Dr = ds.Tables[0].NewRow();
                    Dr["VendorId"] = vendorModel.VendorId;
                    Dr["VendorName"] = vendorModel.VendorName;
                    Dr["Region"] = vendorModel.Region;
                    Dr["City"] = vendorModel.City;
                    Dr["Country"] = vendorModel.Country;
                    Dr["IsManufacturer"] = vendorModel.IsManufacturer;
                    Dr["State"] = vendorModel.State;
                    Dr["ModifiedBy"] = vendorModel.ModifiedBy;
                    Dr["emailId"] = vendorModel.emailId;                   
                    Dr["IsActive"] = vendorModel.IsActive;
                    Dr["AuthorizedDealer"] = vendorModel.AuthorizedDealer;
                    ds.Tables[0].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.savevendor(ds, User_Id);
        }
        public VendorModel DWvendorGetselected(int VendorId)
        {
            VendorModel objmodel = new VendorModel();
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedVendor(VendorId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            objmodel.VendorId = VendorId;
                            objmodel.VendorName = ds.Tables[0].Rows[0]["VendorName"].ToString();
                            objmodel.Region = ds.Tables[0].Rows[0]["Region"].ToString();
                            objmodel.City = ds.Tables[0].Rows[0]["City"].ToString();
                            objmodel.Country = ds.Tables[0].Rows[0]["Country"].ToString();
                            objmodel.State = ds.Tables[0].Rows[0]["State"].ToString();
                            objmodel.emailId = ds.Tables[0].Rows[0]["emailId"].ToString();                           
                            objmodel.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsActive"].ToString());
                            objmodel.IsManufacturer = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsManufacturer"].ToString());
                            objmodel.PAN = ds.Tables[0].Rows[0]["PAN"].ToString();
                            objmodel.CST = ds.Tables[0].Rows[0]["CST"].ToString();
                            objmodel.TAN = ds.Tables[0].Rows[0]["TAN"].ToString();
                            objmodel.VAT = ds.Tables[0].Rows[0]["VAT"].ToString();
                            objmodel.ExciseCommissionRate = ds.Tables[0].Rows[0]["ExciseCommissionRate"].ToString();
                            objmodel.ExciseDivision = ds.Tables[0].Rows[0]["ExciseDivision"].ToString();
                            objmodel.ExciseNo = ds.Tables[0].Rows[0]["ExciseNo"].ToString();
                            objmodel.ExciseRange = ds.Tables[0].Rows[0]["ExciseRange"].ToString();
                            objmodel.CreatedByName = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            objmodel.ModifiedByName = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            objmodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            objmodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                            objmodel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                            objmodel.Website = ds.Tables[0].Rows[0]["Website"].ToString();
                            objmodel.AuthorizedDealer = Convert.ToBoolean(ds.Tables[0].Rows[0]["AuthorizedDealer"].ToString());
                        }
                        objmodel.VendorBankDetailLst = new List<VendorBankDetailModel>();
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                VendorBankDetailModel Demo = new VendorBankDetailModel();
                                Demo.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                                Demo.BankName = dr["BankName"].ToString();
                                Demo.AccountNo = dr["AccountNo"].ToString();
                                Demo.Limit = Convert.ToDouble(dr["Limit"].ToString());
                                Demo.CreatedBy = dr["CreatedBy"].ToString();
                                Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                objmodel.VendorBankDetailLst.Add(Demo);
                            }
                        }
                        objmodel.VendorContactLst = new List<VendorContactDetailsModel>();
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[2].Rows)
                            {
                                VendorContactDetailsModel Demo = new VendorContactDetailsModel();
                                Demo.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                                Demo.ContactName = dr["ContactName"].ToString();
                                Demo.Designation = dr["Designation"].ToString();
                                Demo.MobileNo = dr["MobileNo"].ToString();
                                Demo.Email = dr["emailId"].ToString();
                                Demo.Experties = dr["Experties"].ToString();
                                Demo.Qualification = dr["Qualification"].ToString();
                                Demo.VendorContactId = Convert.ToInt32(dr["VendorContactId"].ToString());
                                if (dr["UserId"].ToString() != "")
                                    Demo.UserId = Convert.ToInt32(dr["UserId"].ToString());
                                if (dr["UserName"].ToString() != "")
                                    Demo.UserName = dr["UserName"].ToString();
                                if (dr["BirthDate"].ToString()!= "")
                                {
                                    Demo.BirthDate = Convert.ToDateTime(dr["BirthDate"].ToString());
                                    if (Demo.BirthDate != null)
                                        Demo.BirthDateStr = Demo.BirthDate.ToShortDateString();
                                }
                                
                                Demo.CreatedBy = dr["CreatedBy"].ToString();
                                Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                Demo.ModifiedBy = dr["ModifiedBy"].ToString();
                                Demo.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                objmodel.VendorContactLst.Add(Demo);
                            }
                        }
                        objmodel.VendorLibaryList = new List<VendorLibaryModel>();
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[3].Rows)
                            {
                                if (dr["IsKYC"].ToString() == "False")
                                {
                                    VendorLibaryModel Demo = new VendorLibaryModel();
                                    Demo.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                                    Demo.DocumentTitle = dr["DocumentTitle"].ToString();
                                    Demo.Description = dr["Description"].ToString();
                                    Demo.DocumentPath = dr["DocumentPath"].ToString();
                                    Demo.CreatedBy = dr["CreatedBy"].ToString();
                                    Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                    objmodel.VendorLibaryList.Add(Demo);
                                }
                            }
                        }
                        objmodel.VendorKYCList = new List<VendorLibaryModel>();
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[3].Rows)
                            {
                                if (dr["IsKYC"].ToString() == "True")
                                {
                                    VendorLibaryModel Demo = new VendorLibaryModel();
                                    Demo.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                                    Demo.DocumentTitle = dr["DocumentTitle"].ToString();
                                    Demo.Description = dr["Description"].ToString();
                                    Demo.DocumentPath = dr["DocumentPath"].ToString();
                                    Demo.CreatedBy = dr["CreatedBy"].ToString();
                                    Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                    objmodel.VendorKYCList.Add(Demo);
                                }
                            }
                        }
                        objmodel.VendorTurnoverList = new List<VendorTurnoverModel>();
                        if (ds.Tables[4].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[4].Rows)
                            {
                                VendorTurnoverModel Demo = new VendorTurnoverModel();
                                Demo.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                                Demo.TurnoverYear = (dr["TurnoverYear"].ToString());
                                Demo.Turnover = Convert.ToDouble(dr["Turnover"].ToString());
                                Demo.ProjectedTurnover = Convert.ToDouble(dr["ProjectedTurnover"].ToString());
                                Demo.CreatedBy = dr["CreatedBy"].ToString();
                                Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                objmodel.VendorTurnoverList.Add(Demo);
                            }
                        }
                        objmodel.VendorCertificationList = new List<VendorCertificationModel>();
                        if (ds.Tables[5].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[5].Rows)
                            {
                                VendorCertificationModel Demo = new VendorCertificationModel();
                                Demo.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                                Demo.VendorCertification = dr["VendorCertification"].ToString();
                                Demo.CertificateDate = Convert.ToDateTime(dr["CertificateDate"].ToString());
                                if (Demo.CertificateDate != null)
                                    Demo.CertificateDateStr = Demo.CertificateDate.ToShortDateString();
                                Demo.CreatedBy = dr["CreatedBy"].ToString();
                                Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                objmodel.VendorCertificationList.Add(Demo);
                            }
                        }
                        objmodel.VendorCompetitorList = new List<VendorCompetitorModel>();
                        if (ds.Tables[6].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[6].Rows)
                            {
                                VendorCompetitorModel Demo = new VendorCompetitorModel();
                                Demo.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                                Demo.CompetitorId = Convert.ToInt32(dr["CompetitorId"].ToString());
                                Demo.CompetitorName = dr["VendorName"].ToString();
                                Demo.Region = dr["Region"].ToString();
                                Demo.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                                Demo.CreatedBy = dr["CreatedBy"].ToString();
                                Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                objmodel.VendorCompetitorList.Add(Demo);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }
        public VendorModel DWvendorContactGetselected(int VendorId)
        {
            VendorModel objmodel = new VendorModel();
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedVendorContact(VendorId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {                       
                        objmodel.VendorContactLst = new List<VendorContactDetailsModel>();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                VendorContactDetailsModel Demo = new VendorContactDetailsModel();
                                Demo.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                                Demo.ContactName = dr["ContactName"].ToString();
                                Demo.Designation = dr["Designation"].ToString();
                                Demo.MobileNo = dr["MobileNo"].ToString();
                                Demo.Email = dr["emailId"].ToString();
                                Demo.Experties = dr["Experties"].ToString();
                                Demo.Qualification = dr["Qualification"].ToString();
                                Demo.VendorContactId = Convert.ToInt32(dr["VendorContactId"].ToString());
                                if (dr["UserId"].ToString() != "")
                                    Demo.UserId = Convert.ToInt32(dr["UserId"].ToString());
                                if (dr["UserName"].ToString() != "")
                                    Demo.UserName = dr["UserName"].ToString();
                                if (dr["BirthDate"].ToString() != "")
                                {
                                    Demo.BirthDate = Convert.ToDateTime(dr["BirthDate"].ToString());
                                    if (Demo.BirthDate != null)
                                        Demo.BirthDateStr = Demo.BirthDate.ToShortDateString();
                                }

                                Demo.CreatedBy = dr["CreatedBy"].ToString();
                                Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                Demo.ModifiedBy = dr["ModifiedBy"].ToString();
                                Demo.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                objmodel.VendorContactLst.Add(Demo);
                            }
                        }  
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }
        public int saveVendorProfessionalInfo(VendorModel VendorModel, string User_Id)
        {

            VendorDAL objdal = new VendorDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedVendor(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow Dr = ds.Tables[0].NewRow();
                    Dr["VendorId"] = VendorModel.VendorId;
                    Dr["emailId"] = VendorModel.emailId;
                    Dr["VAT"] = VendorModel.VAT;
                    Dr["PAN"] = VendorModel.PAN;
                    Dr["TAN"] = VendorModel.TAN;
                    Dr["CST"] = VendorModel.CST;
                    Dr["ExciseCommissionRate"] = VendorModel.ExciseCommissionRate;
                    Dr["ExciseDivision"] = VendorModel.ExciseDivision;
                    Dr["ExciseNo"] = VendorModel.ExciseNo;
                    Dr["ExciseRange"] = VendorModel.ExciseRange;
                    Dr["Remark"] = VendorModel.Remark;
                    Dr["Website"] = VendorModel.Website;
                    ds.Tables[0].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.saveVendirProfessionalInfo(ds, User_Id);
        }

        public int saveVendorBankDetailsInfo(VendorBankDetailModel BankDetailModel, string User_Id)
        {

            VendorDAL objdal = new VendorDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedVendor(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[1].Rows.Clear();
                    DataRow Dr = ds.Tables[1].NewRow();
                    Dr["VendorId"] = BankDetailModel.VendorId;
                    Dr["AccountNo"] = BankDetailModel.AccountNo;
                    Dr["NewAccountNo"] = BankDetailModel.NewAccountNo;
                    Dr["BankName"] = BankDetailModel.BankName;
                    Dr["Limit"] = BankDetailModel.Limit;
                    ds.Tables[1].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.SaveVendorBankDetailsInfo(ds, User_Id);
        }
        public VendorBankDetailModel GetSelectedVendorBankDetail(int VendorId, string AccountNo)
        {
            VendorBankDetailModel VendorModel = new VendorBankDetailModel();
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedVendorBankDetail(VendorId, AccountNo);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            VendorModel.VendorId = Convert.ToInt32(ds.Tables[0].Rows[0]["VendorId"].ToString());
                            VendorModel.BankName = ds.Tables[0].Rows[0]["BankName"].ToString();
                            VendorModel.AccountNo = ds.Tables[0].Rows[0]["AccountNo"].ToString();
                            VendorModel.Limit = Convert.ToDouble(ds.Tables[0].Rows[0]["Limit"].ToString());
                            VendorModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            VendorModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return VendorModel;
        }
        public VendorContactDetailsModel GetSelectedVendorContactDetails(int VendorId, string ContactName)
        {
            VendorContactDetailsModel VendorModel = new VendorContactDetailsModel();
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedVendorContactDetails(VendorId, ContactName);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            VendorModel.VendorId = Convert.ToInt32(ds.Tables[0].Rows[0]["VendorId"].ToString());
                            VendorModel.ContactName = ds.Tables[0].Rows[0]["ContactName"].ToString();
                            VendorModel.Designation = ds.Tables[0].Rows[0]["Designation"].ToString();
                            VendorModel.MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                            VendorModel.PhoneNo = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                            VendorModel.Email = ds.Tables[0].Rows[0]["emailId"].ToString();
                            VendorModel.Experties = ds.Tables[0].Rows[0]["Experties"].ToString();
                            VendorModel.Qualification = ds.Tables[0].Rows[0]["Qualification"].ToString();
                            if (ds.Tables[0].Rows[0]["BirthDate"].ToString() != "")
                            {
                                VendorModel.BirthDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["BirthDate"].ToString());
                            }
                            VendorModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            VendorModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            VendorModel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            VendorModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                            VendorModel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return VendorModel;
        }
        public int saveVendorContactDetailsInfo(VendorContactDetailsModel VendorModel, string User_Id)
        {

            VendorDAL objdal = new VendorDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedVendor(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[2].Rows.Clear();
                    DataRow Dr = ds.Tables[2].NewRow();
                    Dr["VendorId"] = VendorModel.VendorId;
                    Dr["ContactName"] = VendorModel.ContactName;
                    Dr["NewContactName"] = VendorModel.NewContactName;
                    Dr["Designation"] = VendorModel.Designation;
                    Dr["emailId"] = VendorModel.Email;
                    Dr["MobileNo"] = VendorModel.MobileNo;
                    Dr["PhoneNo"] = VendorModel.PhoneNo;
                    Dr["Experties"] = VendorModel.Experties;
                    Dr["Qualification"] = VendorModel.Qualification;
                    if (VendorModel.BirthDate.ToString() != "1/1/0001 12:00:00 AM")
                    {
                        Dr["BirthDate"] = VendorModel.BirthDate;
                    }
                   
                    Dr["Remark"] = VendorModel.Remark;
                    ds.Tables[2].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.saveVendorContactDetailsInfo(ds, User_Id);
        }
        public int SaveVendorLibraryDetailInfo(VendorLibaryModel LibaryModel, string User_Id)
        {
            VendorDAL objdal = new VendorDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedVendor(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[3].Rows.Clear();
                    DataRow Dr = ds.Tables[3].NewRow();
                    Dr["VendorId"] = LibaryModel.VendorId;
                    Dr["IsKYC"] = LibaryModel.KYC;
                    Dr["DocumentTitle"] = LibaryModel.DocumentTitle;
                    Dr["Description"] = LibaryModel.Description;
                    Dr["DocumentPath"] = LibaryModel.DocumentPath;                   
                    ds.Tables[3].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.SaveVendorLibaryDetailInfo(ds, User_Id);
        }
        public VendorTurnoverModel GetSelectedVendorTurnoverDetails(int VendorId, string turnoverYear)
        {
            VendorTurnoverModel TurnoverModel = new VendorTurnoverModel();
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedVendorTurnoverDetails(VendorId, turnoverYear);
                if (ds != null) 
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            TurnoverModel.VendorId = Convert.ToInt32(ds.Tables[0].Rows[0]["VendorId"].ToString());
                            TurnoverModel.TurnoverYear = ds.Tables[0].Rows[0]["TurnoverYear"].ToString();
                            TurnoverModel.Turnover = Convert.ToDouble(ds.Tables[0].Rows[0]["Turnover"].ToString());
                            TurnoverModel.ProjectedTurnover = Convert.ToDouble(ds.Tables[0].Rows[0]["ProjectedTurnover"].ToString());
                            TurnoverModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            TurnoverModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TurnoverModel;
        }

        public int saveTurnoverVendorDetailsInfo(VendorTurnoverModel TurnoverModel, string User_Id)
        {
            VendorDAL objdal = new VendorDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedVendor(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[4].Rows.Clear();
                    DataRow Dr = ds.Tables[4].NewRow();
                    Dr["VendorId"] = TurnoverModel.VendorId;
                    Dr["TurnoverYear"] = TurnoverModel.TurnoverYear;
                    Dr["NewTurnoverYear"] = TurnoverModel.NewTurnoverYear;
                    Dr["Turnover"] = TurnoverModel.Turnover;
                    Dr["ProjectedTurnover"] = TurnoverModel.ProjectedTurnover;
                    ds.Tables[4].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.saveVendorTurnoverDetailsInfo(ds, User_Id);
        }

        public int saveVendorCertificationDetailsInfo(VendorCertificationModel CertificatinModel, string User_Id)
        {
            VendorDAL objdal = new VendorDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedVendor(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[5].Rows.Clear();
                    DataRow Dr = ds.Tables[5].NewRow();
                    Dr["VendorId"] = CertificatinModel.VendorId;
                    Dr["VendorCertification"] = CertificatinModel.VendorCertification;
                    Dr["NewCustomerCertification"] = CertificatinModel.NewVendorCertification;
                    Dr["CertificateDate"] = CertificatinModel.CertificateDate;
                    ds.Tables[5].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.saveVendorCertificationDetailsInfo(ds, User_Id);
        }
        public VendorCertificationModel GetSelectedVendorCertificationDetails(int VendorId, string Certification)
        {
            VendorCertificationModel TurnoverModel = new VendorCertificationModel();
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedVendoCertificationDetails(VendorId, Certification);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            TurnoverModel.VendorId = Convert.ToInt32(ds.Tables[0].Rows[0]["VendorId"].ToString());
                            TurnoverModel.VendorCertification = ds.Tables[0].Rows[0]["VendorCertification"].ToString();
                            TurnoverModel.CertificateDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CertificateDate"].ToString());
                            TurnoverModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            TurnoverModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TurnoverModel;
        }

        public int saveVendorCompetitorDetailsInfo(VendorCompetitorModel CompetitorModel, string User_Id)
        {

            VendorDAL objdal = new VendorDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedVendor(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[6].Rows.Clear();
                    DataRow Dr = ds.Tables[6].NewRow();
                    Dr["VendorId"] = CompetitorModel.VendorId;
                    Dr["CompetitorId"] = CompetitorModel.CompetitorId;
                    Dr["NewCompetitorId"] = CompetitorModel.NewCompetitorId;
                    ds.Tables[6].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.saveVendorCompetitorDetailsInfo(ds, User_Id);
        }

        public VendorCompetitorModel GetSelectedVendorCompetitorDetails(int VendorId, int CompetitorId)
        {
            VendorCompetitorModel CompetitorModel = new VendorCompetitorModel();
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedVendorCompetitorDetails(VendorId, CompetitorId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            CompetitorModel.VendorId = Convert.ToInt32(ds.Tables[0].Rows[0]["VendorId"].ToString());
                            CompetitorModel.CompetitorId = Convert.ToInt32(ds.Tables[0].Rows[0]["CompetitorId"].ToString());
                            CompetitorModel.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsActive"].ToString());
                            CompetitorModel.Region = ds.Tables[0].Rows[0]["Region"].ToString();
                            CompetitorModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            CompetitorModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CompetitorModel;
        }

        public List<VendorCompetitorModel> GetVendorCompetitorList()
        {
            List<VendorCompetitorModel> CompetitorModel = new List<VendorCompetitorModel>();
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetVendorCompetitorList();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                VendorCompetitorModel Demo = new VendorCompetitorModel();
                                Demo.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                                Demo.CustomerName = dr["VendorName"].ToString();
                                CompetitorModel.Add(Demo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CompetitorModel;
        }
        #region Assign User
        public int SaveAssignVendorUserId(int userId, int VendorContactId)
        {
            int errCode = 0;
            try
            {
                VendorDAL DALObj = new VendorDAL();
                errCode = DALObj.SaveAssignVendorUserId(userId, VendorContactId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int DeleteAssignVendorUserId(int VendorContactId)
        {
            int errCode = 0;
            try
            {
                VendorDAL DALObj = new VendorDAL();
                errCode = DALObj.DeleteAssignVendorUserId(VendorContactId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public VendorUserModel GetVendorUserList(int VendorContactId)
        {
            VendorUserModel VendModelList = new VendorUserModel();
            VendModelList.LstUserModel = new List<VendorUserModel>();
            try
            {
                VendorDAL DALObj = new VendorDAL();
                DataSet ds = new DataSet();
                ds = DALObj.GetVendtUserList(VendorContactId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            VendorUserModel Model = new VendorUserModel();
                            Model.UserId = Convert.ToInt32(dr["UserId"].ToString());
                            Model.UserName = dr["UserName"].ToString();
                            VendModelList.LstUserModel.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return VendModelList;
        }
        #endregion Assign User

        #region Address Work

        public VendorAddressModel GetSelectedVendorAddressDetail(int VendorId, int addressid)
        {
            VendorAddressModel addressModel = new VendorAddressModel();
            VendorDAL DALobj = new VendorDAL();
            try
            {
                DataSet DSObj = new DataSet();
                DSObj = DALobj.GetSelectedVendorAddressDetail(VendorId, addressid);
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        addressModel.AddressId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["AddressId"].ToString());
                        addressModel.CityId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["CityId"].ToString());
                        addressModel.Line1 = DSObj.Tables[0].Rows[0]["Line1"].ToString();
                        addressModel.Line2 = DSObj.Tables[0].Rows[0]["Line2"].ToString();
                        addressModel.LandMark = DSObj.Tables[0].Rows[0]["LandMark"].ToString();
                        addressModel.Pin = DSObj.Tables[0].Rows[0]["Pin"].ToString();
                        addressModel.Description = DSObj.Tables[0].Rows[0]["Description"].ToString();
                        addressModel.isPrimary = Convert.ToBoolean(DSObj.Tables[0].Rows[0]["isPrimary"].ToString());
                        addressModel.State = DSObj.Tables[0].Rows[0]["State"].ToString();
                        addressModel.Country = DSObj.Tables[0].Rows[0]["Country"].ToString();
                        addressModel.City = DSObj.Tables[0].Rows[0]["City"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return addressModel;
        }

        public List<VendorAddressModel> GetSelectedAddressByCustomer(int VendorId)
        {

            List<VendorAddressModel> addlst = new List<VendorAddressModel>();

            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedAddressByVendor(VendorId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                VendorAddressModel objmodel = new VendorAddressModel();
                                objmodel.Line1 = dr["Line1"].ToString();
                                objmodel.Line2 = dr["Line2"].ToString();
                                objmodel.LandMark = dr["LandMark"].ToString();
                                objmodel.AddressId = Convert.ToInt32(dr["AddressId"].ToString());
                                objmodel.State = dr["State"].ToString();
                                objmodel.City = dr["City"].ToString();
                                objmodel.Country = dr["Country"].ToString();
                                objmodel.CityId = Convert.ToInt32(dr["CityId"].ToString());
                                objmodel.CountryId = Convert.ToInt32(dr["CountryId"].ToString());
                                objmodel.State = dr["State"].ToString();
                                objmodel.StateId = Convert.ToInt32(dr["StateId"].ToString());
                                objmodel.Pin = dr["Pin"].ToString();
                                objmodel.isPrimary = Convert.ToBoolean(dr["isPrimary"].ToString());
                                objmodel.Description = dr["Description"].ToString();
                                addlst.Add(objmodel);

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return addlst;
        }


        public int SaveVendorAdressDeatils(VendorAddressModel addressmodel, Boolean isPrimary, string Description)
        {
            VendorDAL objdal = new VendorDAL();
            int ErrCode = 0;
            try
            {
                DataSet DSObj = new DataSet();
                DSObj = objdal.GetSelectedVendorAddress(0);
                if (DSObj == null)
                    return 0;
                if (DSObj.Tables.Count > 0)
                {
                    DSObj.Tables[0].Rows.Clear();
                    DataRow Dr = DSObj.Tables[0].NewRow();
                    Dr["AddressId"] = addressmodel.AddressId;
                    Dr["Line1"] = addressmodel.Line1;
                    Dr["Line2"] = addressmodel.Line2;
                    Dr["State"] = addressmodel.State;
                    Dr["City"] = addressmodel.City;
                    Dr["Country"] = addressmodel.Country;
                    Dr["Pin"] = addressmodel.Pin;
                    Dr["LandMark"] = addressmodel.LandMark;
                    DSObj.Tables[0].Rows.Add(Dr);
                    ErrCode = objdal.SaveVendorAdressDeatils(DSObj, addressmodel.VendorId, isPrimary, Description);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }       
        #endregion Address Work

        #region Product Work
        public List<VendorProductModel> GetVendorProductList(int VendorId)
        {
            VendorModel objmodel = new VendorModel();
            objmodel.ProductList = new List<VendorProductModel>();
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.DWVendorProductList(VendorId);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            VendorProductModel Demo = new VendorProductModel();
                            Demo.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                            Demo.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            Demo.VendorName = dr["VendorName"].ToString();
                            Demo.EquipmentName = dr["EquipmentName"].ToString();                            
                            objmodel.ProductList.Add(Demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return objmodel.ProductList;
        }
        public List<ItemModel> GetVendorItemList()
        {
            List<ItemModel> lstItem = new List<ItemModel>();
            try
            {
                DataSet dsItem = new DataSet();
                VendorDAL DALObj = new VendorDAL();
                dsItem = DALObj.GetItemListForVendor();
                if (dsItem != null)
                {
                    if (dsItem.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsItem.Tables[0].Rows)
                        {
                            ItemModel objmodel = new ItemModel();
                            objmodel.ItemId = Convert.ToInt32(dr["ItemId"]);
                            objmodel.ItemName = dr["ItemName"].ToString();
                            lstItem.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstItem;
        }
        public int SaveVendorproductDetails(VendorProductModel Productmodel)
        {
            VendorDAL objdal = new VendorDAL();
            DataSet ds = new DataSet();
           int erroCode = 0;
            try
            {
                erroCode = objdal.SaveVendorproduct(Productmodel.VendorId,Productmodel.EquipmentId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return erroCode;
        }
        public int DeleteVendorEquipment(int VendorId, int EquipmentId)
        {
            VendorDAL objdal = new VendorDAL();
            int errCode = 0;
            try
            {
                errCode = objdal.DeleteVendorEquipment(VendorId, EquipmentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        public List<VendorProductModel> VendorProductGetselected(int VendorId)
        {
            List<VendorProductModel> objmodel = new List<VendorProductModel>();
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedVendorProduct(VendorId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                VendorProductModel Model = new VendorProductModel();
                                Model.EquipmentId = Convert.ToInt32(dr["EquipmentId"]);
                                Model.EquipmentName = dr["EquipmentName"].ToString();
                                objmodel.Add(Model);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }

        //public List<VendorItemmodel> GetSelectedItemByVendor(int productid)
        //{
        //    VendorProductModel model = new VendorProductModel();
        //    model.ItemList = new List<VendorItemmodel>();
        //    try
        //    {
        //        VendorDAL objdal = new VendorDAL();
        //        DataSet ds = new DataSet();
        //        ds = objdal.GetSelectedItemByVendor(productid);

        //        if (ds != null)
        //        {
        //            if (ds.Tables.Count > 0)
        //            {
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    foreach (DataRow dr in ds.Tables[0].Rows)
        //                    {
        //                        VendorItemmodel objmodel = new VendorItemmodel();
        //                        objmodel.ItemId = Convert.ToInt32(dr["ItemId"]);
        //                        objmodel.ItemName = dr["ItemName"].ToString();
        //                        objmodel.Quantity = Convert.ToDouble(dr["Quantity"].ToString());
        //                        model.ItemList.Add(objmodel);
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return model.ItemList;
        //}
        public int saveVendorItemDetails(string User_Id, int ItemId, double Quantity, int productid, string productname)
        {
            try
            {
                VendorDAL objdal = new VendorDAL();
                return objdal.SaveVendoritem(User_Id, ItemId, Quantity, productid, productname);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Product Work

        #region Vendor Mapping

        public List<SysNavRelVendorListModel> GetVendorNavRelList(int Vendor)
        {
            List<SysNavRelVendorListModel> lstVendorMap = new List<SysNavRelVendorListModel>();
            try
            {
                DataSet dsobj = new DataSet();
                VendorDAL DALObj = new VendorDAL();
                dsobj = DALObj.GetVendorNavRelList(Vendor);
                if (dsobj != null)
                {
                    if (dsobj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            SysNavRelVendorListModel MapTemp = new SysNavRelVendorListModel();
                            MapTemp.CompVendorID = dr["Vendor_No"].ToString();
                            MapTemp.CompName = dr["CompCode"].ToString();
                            MapTemp.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            MapTemp.ConnectionId = Convert.ToInt16(dr["ConnectionId"].ToString());
                            lstVendorMap.Add(MapTemp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstVendorMap;
        }
        public List<VendorMappingModel> GetDWCompList()
        {
            List<VendorMappingModel> lstVendorMap = new List<VendorMappingModel>();

            try
            {
                VendorDAL DALObj = new VendorDAL();
                DataSet dsObj = new DataSet();
                dsObj = DALObj.GetDWCompVendorList();
                if (dsObj != null)
                {
                    if (dsObj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            VendorMappingModel MapObj = new VendorMappingModel();
                            MapObj.CompCode = dr["CompCode"].ToString();
                            MapObj.CompName = dr["CompanyName"].ToString();
                            MapObj.ConnectionId = Convert.ToInt16(dr["ConnectionId"]);
                            MapObj.VendorListSP = dr["VendorListSP"].ToString();
                            MapObj.SaveVendorSP = dr["SaveVerdorSP"].ToString();
                            lstVendorMap.Add(MapObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstVendorMap;
        }
        public List<MapClientVendorList> GetCompanyVendor(string CompCode, string SPName, ArrayList ConnInfo)
        {
            List<MapClientVendorList> lstMapVendor = new List<MapClientVendorList>();
            try
            {
                DataSet dsobj = new DataSet();
                VendorDAL DALObj = new VendorDAL();
                dsobj = DALObj.GetCompanyVendor(CompCode, SPName, ConnInfo);
                if (dsobj != null)
                {
                    if (dsobj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            MapClientVendorList MapItemTemp = new MapClientVendorList();
                            MapItemTemp.ClientVendorID = dr["VendorNo"].ToString();
                            MapItemTemp.ClientVendorName = dr["VendorDisplayName"].ToString();
                            lstMapVendor.Add(MapItemTemp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstMapVendor;
        }
        public string SaveMapping(VendorMappingModel ItemMap, string User_Id)
        {
            string ErrorCode = "0";
            try
            {
                VendorDAL DALObj = new VendorDAL();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("tbl_DHFeedEventDep");
                dt.Columns.Add("Vendor_No", typeof(System.String));
                dt.Columns.Add("CompCode", typeof(System.String));
                dt.Columns.Add("VendorId", typeof(System.Int32));

                DataRow dr = dt.NewRow();
                dr["Vendor_No"] = ItemMap.CompVendorID;
                dr["CompCode"] = ItemMap.CompCode;
                dr["VendorId"] = ItemMap.VendorId;
                dt.Rows.Add(dr);
                dsObj.Tables.Add(dt);
                ErrorCode = DALObj.SaveMapping(dsObj, User_Id);
            }
            catch (Exception)
            {

                throw;
            }

            return ErrorCode;
        }
        public string GetClientItemName(string CompName, string CompItemID, ArrayList ConnInfo)
        {
            string str = "";
            try
            {
                VendorDAL DalObj = new VendorDAL();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetClientVendorName(CompName, CompItemID, ConnInfo);
                if (dsObj != null)
                {
                    str = dsObj.Tables[0].Rows[0]["VendorDisplayName"].ToString();
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                str = "No Connection";
            }

            return str;
        }
        public int DeleteVendorMapping(string CompName, int VendorId)
        {
            int errcod = 0;
            try
            {
                VendorDAL DALObj = new VendorDAL();
                errcod = DALObj.DeleteVendorMapping(CompName, VendorId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errcod;
        }


        public int GetAutoMapping(string User_Id, string CompCode, string VandorName, int VendorId, string SPName, ArrayList ConnArrList)
        {
            int errCode = 0;
            try
            {
                GeneratorBL objbl = new GeneratorBL();
                DataSet dsParam = new DataSet();
                DataTable dt = new DataTable();
                dt.Columns.Add("DataType", typeof(System.String));
                dt.Columns.Add("ParamName", typeof(System.String));
                dt.Columns.Add("ParamValue", typeof(System.String));
                DataRow dr = dt.NewRow();
                dr["DataType"] = "StringParameter";
                dr["ParamName"] = "VendorName";
                dr["ParamValue"] = VandorName;
                dt.Rows.Add(dr);
                dsParam.Tables.Add(dt);
                DataSet Vendor = new DataSet();
                Vendor = objbl.ExecuteUnderlayingSP(dsParam, SPName, ConnArrList);
                if (Vendor != null)
                {
                    if (Vendor.Tables.Count > 0)
                    {
                        if (Vendor.Tables[0].Rows.Count > 0)
                        {
                            VendorDAL DALObj = new VendorDAL();
                            ItemDAL ItemDAL = new ItemDAL();
                            DataSet dsDBData = new DataSet();
                            DataTable dtDBData = new DataTable("ItenNavRel");
                            dtDBData.Columns.Add("Vendor_No", typeof(System.String));
                            dtDBData.Columns.Add("CompCode", typeof(System.String));
                            dtDBData.Columns.Add("VendorId", typeof(System.Int32));

                            DataRow drDBData = dtDBData.NewRow();
                            drDBData["Vendor_No"] = Vendor.Tables[0].Rows[0]["VendorNo"];
                            drDBData["CompCode"] = CompCode;
                            drDBData["VendorId"] = VendorId;
                            dtDBData.Rows.Add(drDBData);
                            dsDBData.Tables.Add(dtDBData);
                            string ErrorCode = DALObj.SaveMapping(dsDBData, User_Id);
                            if (ErrorCode == "500001" || ErrorCode == "500002")
                            {
                                string Description = "";
                                if (ErrorCode == "500001")
                                {
                                    Description = "Update Vendor " + ", " + CompCode + " , " + VandorName;
                                }
                                Description = "New Vendor " + ", " + CompCode + " , " + VandorName;
                                errCode = ItemDAL.SaveExtSysLog(Description, Vendor.Tables[0].Rows[0]["VendorNo"].ToString(), User_Id);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion Vendor Mapping

        #region Pendding Approval Vendor Mapping

        public List<PenndingVendorApproverMap> GetVenderPenddingApproverList(string User_Id,int StatusCode)
        {
            List<PenndingVendorApproverMap> VenderApproverList = new List<PenndingVendorApproverMap>();
            try
            {
                DataSet ds = new DataSet();
                VendorDAL DALObj = new VendorDAL();
                ds = DALObj.GetVenderPenddingApproverList(User_Id, StatusCode);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PenndingVendorApproverMap temp = new PenndingVendorApproverMap();
                            temp.VendorId = Convert.ToInt32(dr["VendorId"]);
                            temp.VendorName = dr["VendorName"].ToString();
                            temp.Vendor_No = dr["Vendor_No"].ToString();
                            temp.SourceVenderName = dr["SourceVenderName"].ToString();
                            temp.CompCode = dr["CompCode"].ToString();
                            temp.ModifiedBy = dr["ModifiedBy"].ToString();
                            temp.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            VenderApproverList.Add(temp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return VenderApproverList;
        }
        public int ApvRejVendor(PenndingVendorApproverMap VendorApproveModel, int SttusCode, string User_Id)
        {
            int errCode = 0;
            try
            {
                VendorDAL DALObj = new VendorDAL();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("ApvRejItem");
                dt.Columns.Add("Vendor_Id", typeof(System.String));
                dt.Columns.Add("CompCode", typeof(System.String));
                dt.Columns.Add("StatusCode", typeof(System.Int32));
                foreach (SelectPenddingVendorApproveList demo in VendorApproveModel.SelectApproverLst)
                {
                    DataRow dr = dt.NewRow();
                    dr["Vendor_Id"] = demo.Vendor_No;
                    dr["CompCode"] = demo.CompName;
                    dr["StatusCode"] = SttusCode;
                    dt.Rows.Add(dr);
                }
                dsObj.Tables.Add(dt);
                errCode = DALObj.ApvRejVendor(dsObj, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion Pendding Approval Vendor Mapping

         #region Pendding Review Vendor Mapping
        public int ApvRejVendorReview(PenndingVendorApproverMap VendorApproveModel, int SttusCode, string User_Id)
        {
            int errCode = 0;
            try
            {
                VendorDAL DALObj = new VendorDAL();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("ApvRejItem");
                dt.Columns.Add("Vendor_Id", typeof(System.String));
                dt.Columns.Add("CompCode", typeof(System.String));
                dt.Columns.Add("StatusCode", typeof(System.Int32));
                foreach (SelectPenddingVendorApproveList demo in VendorApproveModel.SelectApproverLst)
                {
                    DataRow dr = dt.NewRow();
                    dr["Vendor_Id"] = demo.Vendor_No;
                    dr["CompCode"] = demo.CompName;
                    dr["StatusCode"] = SttusCode;
                    dt.Rows.Add(dr);
                }
                dsObj.Tables.Add(dt);
                errCode = DALObj.ApvRejVendorReview(dsObj, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
         #endregion Pendding Review Vendor Mapping

        #region FreightForwarder
        public List<FreightForwarderModel> FreightForvordarList()
        {
            List<FreightForwarderModel> lstfreightflist = new List<FreightForwarderModel>();
            DataSet ds = new DataSet();
            try
            {
                VendorDAL vendordal = new VendorDAL();
                ds = vendordal.GetFreightForwarderList();
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        FreightForwarderModel obmodel = new FreightForwarderModel();
                        obmodel.FFId = Convert.ToInt32(dr["FFId"].ToString());
                        obmodel.VendorName = dr["VendorName"].ToString();
                        obmodel.Region = dr["Region"].ToString();
                        obmodel.Website = dr["Website"].ToString();
                        obmodel.CreatedBy = dr["CreatedBy"].ToString();
                        obmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        obmodel.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                        lstfreightflist.Add(obmodel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return lstfreightflist;
        }
        public List<FreightForwarderModel> DrpVendorList()
        {
            List<FreightForwarderModel> drpvendorlist = new List<FreightForwarderModel>();
            DataSet ds = new DataSet();
            try
            {
                VendorDAL vendordal = new VendorDAL();
                ds = vendordal.GetDropVendorList();
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        FreightForwarderModel obmodel = new FreightForwarderModel();
                        obmodel.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                        obmodel.VendorName = dr["VendorName"].ToString();
                        drpvendorlist.Add(obmodel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return drpvendorlist;
        }
        public int saveFreightForwardordetail(FreightForwarderModel FreightModel, string User_Id)
        {

            VendorDAL objdal = new VendorDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedFreightFdetail(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow Dr = ds.Tables[0].NewRow();
                    Dr["FFId"] = FreightModel.FFId;
                    Dr["VendorId"] = FreightModel.VendorName;
                    Dr["CreatedBy"] = FreightModel.CreatedBy;
                    Dr["CreatedDate"] = FreightModel.CreatedDate;

                    ds.Tables[0].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.saveFreightForwordarDeatail(ds, User_Id);
        }
        public FreightForwarderModel GetSelectedfreightlist(int FFId)
        {
            FreightForwarderModel Model = new FreightForwarderModel();
            DataSet ds = new DataSet();
            try
            {

                VendorDAL vendordal = new VendorDAL();
                ds = vendordal.GetSelectedFreightFdetail(FFId);
                if (ds == null)
                    return null;

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            Model.FFId = Convert.ToInt32(ds.Tables[0].Rows[0]["FFId"].ToString());
                            Model.VendorName = ds.Tables[0].Rows[0]["VendorId"].ToString();

                            Model.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            Model.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Model;
        }
        public int DeleteFreightFDetail(int FFId)
        {
            int errcode = 0;
            try
            {

                VendorDAL objdal = new VendorDAL();
                errcode = objdal.DeleteFreightForwardorDetail(FFId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }

        #endregion FreightForwarder

        #region Vendor Brand Allocation 
        public VendorBrandAllocateModel GetVendorAllocateList(int VendorId)
        {
            VendorBrandAllocateModel objmodel = new VendorBrandAllocateModel();
            objmodel.AllocateBrand=new List<VendorBrandAllocateModel>();
            objmodel.UnAllocateBrand = new List<VendorBrandAllocateModel>();
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetVendorAllocateList(VendorId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                VendorBrandAllocateModel Model = new VendorBrandAllocateModel();
                                Model.BrandId = Convert.ToInt32(dr["BrandId"]);
                                Model.BrandName = dr["BrandName"].ToString();
                                objmodel.AllocateBrand.Add(Model);
                            }
                        }
                    }
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                VendorBrandAllocateModel Model = new VendorBrandAllocateModel();
                                Model.BrandId = Convert.ToInt32(dr["BrandId"]);
                                Model.BrandName = dr["BrandName"].ToString();
                                objmodel.UnAllocateBrand.Add(Model);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }
        public int SaveAllocatedVendorBrand(VendorBrandAllocateModel Model, string User_Id, int VendorId)
        {
            int errCode = 0;
            try
            {
                VendorDAL objdal = new VendorDAL();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("tbl_Zone");
                dt.Columns.Add("BrandId", typeof(System.Int32));
                dt.Columns.Add("vendorId", typeof(System.Int32));
                foreach (VendorBrandAllocateModel BrandModel in Model.AllocateBrand)
                {
                    DataRow dr = dt.NewRow();
                    dr["BrandId"] = BrandModel.BrandId;
                    dr["vendorId"] = BrandModel.vendorId;
                    dt.Rows.Add(dr);
                }
                dsObj.Tables.Add(dt);
                errCode = objdal.SaveAllocatedVendorBrand(dsObj, User_Id, VendorId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion Vendor Brand Allocation 
        public List<VendoridModel> GetVendorIdList(string User_Id)
        {
            List<VendoridModel> VendorList = new List<VendoridModel>();
            try
            {
                DataSet dsobj = new DataSet();
                VendorDAL DALObj = new VendorDAL();
                dsobj = DALObj.GetVendorIdList(User_Id);
                if (dsobj != null)
                {
                    if (dsobj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            VendoridModel Demo = new VendoridModel();
                            Demo.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            Demo.VendorName = dr["VendorName"].ToString();
                            VendorList.Add(Demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return VendorList;
        }


    }
}
