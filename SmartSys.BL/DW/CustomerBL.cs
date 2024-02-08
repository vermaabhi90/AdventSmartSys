using SmartSys.BL.RGS;
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
    public class CustomerBL
    {
        public List<CustomerListModel> GetCustomerList(string str)
        {
            List<CustomerListModel> lstCustomerModel = new List<CustomerListModel>();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                if (str != "")
                {
                    ds = objdal.CustomerLIstbySalesPerson(str);
                }
                else
                {
                    ds = objdal.CustomerLIst();
                }               
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CustomerListModel objmodel = new CustomerListModel();
                            objmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.Region = dr["Region"].ToString();
                            objmodel.SalesPersonName = dr["SalesPersonName"].ToString();
                            //objmodel.City = dr["City"].ToString();
                            // objmodel.Country = dr["Country"].ToString();
                            objmodel.Description = dr["Description"].ToString();
                            // objmodel.State = dr["State"].ToString();
                            objmodel.emailId = dr["emailId"].ToString();
                            objmodel.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                            objmodel.ModifiedByName = dr["ModifiedByName"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                            //objmodel.SAJCustomer_No = dr["SAJCustomer_No"].ToString();
                            //objmodel.DPKCustomer_No = dr["DPKCustomer_No"].ToString();
                            //objmodel.ADVENTCustomer_No = dr["ADVENTCustomer_No"].ToString();
                            //objmodel.BOMCustomer_No = dr["BOMCustomer_No"].ToString();
                            //objmodel.TimeSheetCustomer_No = dr["TimeSheetCustomer_No"].ToString();
                            //objmodel.SAJMapStatus = Convert.ToInt16(dr["SAJMapStatus"].ToString());
                            //objmodel.DPKMapStatus = Convert.ToInt16(dr["DPKMapStatus"].ToString());
                            //objmodel.ADVENTMapStatus = Convert.ToInt16(dr["ADVENTMapStatus"].ToString());
                            //objmodel.BOMMapStatus = Convert.ToInt16(dr["BOMMapStatus"].ToString());
                            //objmodel.TimeSheetMapStatus = Convert.ToInt16(dr["TimeSheetMapStatus"].ToString());
                            objmodel.AuthorizedDealer = Convert.ToBoolean(dr["AuthorizedDealer"].ToString());
                            lstCustomerModel.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstCustomerModel;
        }
        public bool CheckDuplicacy(string CustomerName)
        {
            bool Result = true;
            CustomerDAL objdal = new CustomerDAL();
            DataSet ds = new DataSet();
            ds = objdal.CustomerLIst();
            DataColumn[] columns = ds.Tables[0].Columns.Cast<DataColumn>().ToArray();
            Result = ds.Tables[0].AsEnumerable()
                .Any(row => columns.Any(col => row[col].ToString() == CustomerName));
            return Result;
        }
        public List<CustomerListModel> GetCustomerListHavingContacts(int ProjectId, string User_Id)
        {
            List<CustomerListModel> lstCustomerModel = new List<CustomerListModel>();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.CustomerLIstHavingContacts(ProjectId, User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CustomerListModel objmodel = new CustomerListModel();
                            objmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            lstCustomerModel.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstCustomerModel;
        }
        public List<ActiveCompanyModel> GetActiveCompanyList(string UserId)
        {
            List<ActiveCompanyModel> LstCompany = new List<ActiveCompanyModel>();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetActiveCompanyList(UserId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ActiveCompanyModel objmodel = new ActiveCompanyModel();
                            objmodel.CompCode = dr["CompCode"].ToString();
                            objmodel.CompName = dr["CompName"].ToString();
                            objmodel.CompTemplet = dr["CompTemplet"].ToString();
                            LstCompany.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstCompany;
        }
        public List<BudgetCustomerModel> GetBudgetCustomerList()
        {
            List<BudgetCustomerModel> lstCustomerModel = new List<BudgetCustomerModel>();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.CustomerLIst();
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BudgetCustomerModel objmodel = new BudgetCustomerModel();
                            objmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            lstCustomerModel.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstCustomerModel;
        }
        public int saveCustomer(CustomerModel CustomerModel, string User_Id)
        {

            CustomerDAL objdal = new CustomerDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedCustomer(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow Dr = ds.Tables[0].NewRow();
                    Dr["CustomerId"] = CustomerModel.CustomerId;
                    Dr["PTId"] = CustomerModel.PTId;
                    Dr["CustomerName"] = CustomerModel.CustomerName;
                    Dr["Region"] = CustomerModel.Region;
                    Dr["IsActive"] = CustomerModel.IsActive;
                    Dr["AuthorizedDealer"] = CustomerModel.AuthorizedDealer;
                    Dr["CustLevel"] = CustomerModel.LevelId;
                    Dr["SalesPersonId"] = CustomerModel.SalesPersonId;
                    Dr["CommToCSR"] = CustomerModel.CommunicateToCSR;
                    ds.Tables[0].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.saveCustomer(ds, User_Id);
        }
        public int SaveLibraryDetailInfo(CustomerLibaryModel CustomerModel, string User_Id)
        {
            CustomerDAL objdal = new CustomerDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedCustomer(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[3].Rows.Clear();
                    DataRow Dr = ds.Tables[3].NewRow();
                    Dr["CustomerId"] = CustomerModel.CustomerId;
                    Dr["IsKYC"] = CustomerModel.KYC;
                    Dr["DocumentTitle"] = CustomerModel.DocumentTitle;
                    Dr["Description"] = CustomerModel.Description;
                    Dr["DocumentPath"] = CustomerModel.DocumentPath;
                    ds.Tables[3].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.SaveLibaryDetailInfo(ds, User_Id);
        }
        public int saveBankDetailsInfo(CustomerBankDetailModel BankDetailModel, string User_Id)
        {

            CustomerDAL objdal = new CustomerDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedCustomer(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[1].Rows.Clear();
                    DataRow Dr = ds.Tables[1].NewRow();
                    Dr["CustomerId"] = BankDetailModel.CustomerId;
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
            return objdal.SaveBankDetailsInfo(ds, User_Id);
        }
        public CustomerBankDetailModel GetSelectedBankDetail(int CustomerId, string AccountNo)
        {
            CustomerBankDetailModel CustomerModel = new CustomerBankDetailModel();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedBankDetail(CustomerId, AccountNo);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            CustomerModel.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString());
                            CustomerModel.BankName = ds.Tables[0].Rows[0]["BankName"].ToString();
                            CustomerModel.AccountNo = ds.Tables[0].Rows[0]["AccountNo"].ToString();
                            CustomerModel.Limit = Convert.ToDouble(ds.Tables[0].Rows[0]["Limit"].ToString());
                            CustomerModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            CustomerModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CustomerModel;
        }
        public CustomerContactDetailsModel GetSelectedContactDetails(int CustomerId, string ContactName)
        {
            CustomerContactDetailsModel ContactModel = new CustomerContactDetailsModel();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedContactDetails(CustomerId, ContactName);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ContactModel.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString());
                            ContactModel.ContactName = ds.Tables[0].Rows[0]["ContactName"].ToString();
                            ContactModel.Designation = ds.Tables[0].Rows[0]["Designation"].ToString();
                            ContactModel.MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                            ContactModel.PhoneNo = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                            ContactModel.Experties = ds.Tables[0].Rows[0]["Experties"].ToString();
                            ContactModel.email = ds.Tables[0].Rows[0]["emailId"].ToString();
                            ContactModel.Qualification = ds.Tables[0].Rows[0]["Qualification"].ToString();
                            if (ds.Tables[0].Rows[0]["BirthDate"].ToString() != "")
                            {
                                ContactModel.BirthDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["BirthDate"].ToString());
                            }

                            ContactModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            ContactModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            ContactModel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            ContactModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                            ContactModel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ContactModel;
        }
        public CustomerTurnoverModel GetSelectedTurnoverDetails(int CustomerId, string turnoverYear)
        {
            CustomerTurnoverModel TurnoverModel = new CustomerTurnoverModel();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedTurnoverDetails(CustomerId, turnoverYear);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            TurnoverModel.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString());
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
        public int saveContactDetailsInfo(CustomerContactDetailsModel ContactModel, string User_Id)
        {

            CustomerDAL objdal = new CustomerDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedCustomer(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[2].Rows.Clear();
                    DataRow Dr = ds.Tables[2].NewRow();
                    Dr["CustomerId"] = ContactModel.CustomerId;
                    Dr["ContactName"] = ContactModel.ContactName;
                    Dr["NewContactName"] = ContactModel.NewContactName;
                    Dr["Designation"] = ContactModel.Designation;
                    Dr["MobileNo"] = ContactModel.MobileNo;
                    Dr["PhoneNo"] = ContactModel.PhoneNo;
                    Dr["emailId"] = ContactModel.email;
                    Dr["Experties"] = ContactModel.Experties;
                    Dr["Qualification"] = ContactModel.Qualification;
                    if (ContactModel.BirthDate.ToString() != "1/1/0001 12:00:00 AM" && ContactModel.BirthDate.ToString() != "01/01/0001 12:00:00 AM")
                    {
                        Dr["BirthDate"] = ContactModel.BirthDate;
                    }

                    Dr["Remark"] = ContactModel.Remark;
                    ds.Tables[2].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.saveContactDetailsInfo(ds, User_Id);
        }
        public int saveCustomerProfessionalInfo(CustomerModel CustomerModel, string User_Id)
        {

            CustomerDAL objdal = new CustomerDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedCustomer(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow Dr = ds.Tables[0].NewRow();
                    Dr["CustomerId"] = CustomerModel.CustomerId;
                    Dr["emailId"] = CustomerModel.emailId;
                    Dr["VAT"] = CustomerModel.VAT;
                    Dr["PAN"] = CustomerModel.PAN;
                    Dr["TAN"] = CustomerModel.TAN;
                    Dr["CST"] = CustomerModel.CST;
                    Dr["ExciseCommissionRate"] = CustomerModel.ExciseCommissionRate;
                    Dr["ExciseDivision"] = CustomerModel.ExciseDivision;
                    Dr["ExciseNo"] = CustomerModel.ExciseNo;
                    Dr["ExciseRange"] = CustomerModel.ExciseRange;
                    Dr["Website"] = CustomerModel.Website;
                    Dr["Remark"] = CustomerModel.Remark;
                    ds.Tables[0].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.saveCustomerProfessionalInfo(ds, User_Id);
        }
        public int saveTurnoverDetailsInfo(CustomerTurnoverModel TurnoverModel, string User_Id)
        {

            CustomerDAL objdal = new CustomerDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedCustomer(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[4].Rows.Clear();
                    DataRow Dr = ds.Tables[4].NewRow();
                    Dr["CustomerId"] = TurnoverModel.CustomerId;
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
            return objdal.saveTurnoverDetailsInfo(ds, User_Id);
        }
        public int saveCertificationDetailsInfo(CustomerCertificationModel CertificatinModel, string User_Id)
        {
            CustomerDAL objdal = new CustomerDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedCustomer(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[5].Rows.Clear();
                    DataRow Dr = ds.Tables[5].NewRow();
                    Dr["CustomerId"] = CertificatinModel.CustomerId;
                    Dr["CustomerCertification"] = CertificatinModel.CustomerCertification;
                    Dr["NewCustomerCertification"] = CertificatinModel.NewCustomerCertification;
                    Dr["CertificateDate"] = CertificatinModel.CertificateDate;
                    ds.Tables[5].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.saveCertificationDetailsInfo(ds, User_Id);
        }
        public CustomerCertificationModel GetSelectedCertificationDetails(int CustomerId, string Certification)
        {
            CustomerCertificationModel TurnoverModel = new CustomerCertificationModel();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedCertificationDetails(CustomerId, Certification);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            TurnoverModel.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString());
                            TurnoverModel.CustomerCertification = ds.Tables[0].Rows[0]["CustomerCertification"].ToString();
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

        public int saveCompetitorDetailsInfo(CustomerCompetitorModel CompetitorModel, string User_Id)
        {

            CustomerDAL objdal = new CustomerDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedCustomer(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[6].Rows.Clear();
                    DataRow Dr = ds.Tables[6].NewRow();
                    Dr["CustomerId"] = CompetitorModel.CustomerId;
                    Dr["CompetitorId"] = CompetitorModel.CompetitorId;
                    Dr["NewCompetitorId"] = CompetitorModel.NewCompetitorId;
                    ds.Tables[6].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.saveCompetitorDetailsInfo(ds, User_Id);
        }

        public CustomerCompetitorModel GetSelectedCompetitorDetails(int CustomerId, int CompetitorId)
        {
            CustomerCompetitorModel CompetitorModel = new CustomerCompetitorModel();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedCompetitorDetails(CustomerId, CompetitorId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            CompetitorModel.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString());
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

        public List<CustomerCompetitorModel> GetCompetitorList()
        {
            List<CustomerCompetitorModel> CompetitorModel = new List<CustomerCompetitorModel>();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetCompetitorList();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                CustomerCompetitorModel Demo = new CustomerCompetitorModel();
                                Demo.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                                Demo.CustomerName = dr["CustomerName"].ToString();
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


        public CustomerModel CustomerGetselected(int CustomerID)
        {
            CustomerModel objmodel = new CustomerModel();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedCustomer(CustomerID);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            objmodel.CustomerId = CustomerID;
                            objmodel.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                            objmodel.Region = ds.Tables[0].Rows[0]["Region"].ToString();
                            objmodel.City = ds.Tables[0].Rows[0]["City"].ToString();
                            objmodel.Country = ds.Tables[0].Rows[0]["Country"].ToString();
                            objmodel.State = ds.Tables[0].Rows[0]["State"].ToString();
                            objmodel.emailId = ds.Tables[0].Rows[0]["emailId"].ToString();
                            objmodel.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                            if (ds.Tables[0].Rows[0]["PTId"].ToString() != "")
                                objmodel.PTId = Convert.ToInt32(ds.Tables[0].Rows[0]["PTId"].ToString());
                            objmodel.LevelId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustLevel"].ToString());
                            objmodel.IsActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsActive"].ToString());
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
                            objmodel.SalesPersonId = Convert.ToInt32(ds.Tables[0].Rows[0]["SalesPersonId"].ToString());
                            objmodel.SalesPersonName = ds.Tables[0].Rows[0]["SalesPersonName"].ToString();
                            objmodel.CommunicateToCSR = Convert.ToBoolean(ds.Tables[0].Rows[0]["CommToCSR"].ToString());
                        }
                        objmodel.BankDetailLst = new List<CustomerBankDetailModel>();
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                CustomerBankDetailModel Demo = new CustomerBankDetailModel();
                                Demo.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                                Demo.BankName = dr["BankName"].ToString();
                                Demo.AccountNo = dr["AccountNo"].ToString();
                                Demo.Limit = Convert.ToDouble(dr["Limit"].ToString());
                                Demo.CreatedBy = dr["CreatedBy"].ToString();
                                Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                objmodel.BankDetailLst.Add(Demo);
                            }
                        }
                        objmodel.CustmerContactLst = new List<CustomerContactDetailsModel>();
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[2].Rows)
                            {
                                CustomerContactDetailsModel Demo = new CustomerContactDetailsModel();
                                Demo.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                                Demo.CustomerContactId = Convert.ToInt32(dr["CustomerContactId"].ToString());
                                Demo.ContactName = dr["ContactName"].ToString();
                                Demo.Designation = dr["Designation"].ToString();
                                Demo.email = dr["emailId"].ToString();
                                Demo.MobileNo = dr["MobileNo"].ToString();
                                Demo.Experties = dr["Experties"].ToString();
                                Demo.Qualification = dr["Qualification"].ToString();
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
                                Demo.Remark = dr["Remark"].ToString();
                                objmodel.CustmerContactLst.Add(Demo);
                            }
                        }

                        objmodel.CustomerLibaryList = new List<CustomerLibaryModel>();
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[3].Rows)
                            {
                                if (dr["IsKYC"].ToString() == "False")
                                {

                                    CustomerLibaryModel Demo = new CustomerLibaryModel();
                                    Demo.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                                    Demo.DocumentTitle = dr["DocumentTitle"].ToString();
                                    Demo.Description = dr["Description"].ToString();
                                    Demo.DocumentPath = dr["DocumentPath"].ToString();
                                    Demo.CreatedBy = dr["CreatedBy"].ToString();
                                    Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                    objmodel.CustomerLibaryList.Add(Demo);
                                }
                            }
                        }
                        objmodel.CustomerKYCList = new List<CustomerLibaryModel>();
                        if (ds.Tables[3].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[3].Rows)
                            {
                                if (dr["IsKYC"].ToString() == "True")
                                {

                                    CustomerLibaryModel Demo = new CustomerLibaryModel();
                                    Demo.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                                    Demo.DocumentTitle = dr["DocumentTitle"].ToString();
                                    Demo.Description = dr["Description"].ToString();
                                    Demo.DocumentPath = dr["DocumentPath"].ToString();
                                    Demo.CreatedBy = dr["CreatedBy"].ToString();
                                    Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                    objmodel.CustomerKYCList.Add(Demo);
                                }
                            }
                        }
                        objmodel.CustomerTurnoverList = new List<CustomerTurnoverModel>();
                        if (ds.Tables[4].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[4].Rows)
                            {
                                CustomerTurnoverModel Demo = new CustomerTurnoverModel();
                                Demo.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                                Demo.TurnoverYear = dr["TurnoverYear"].ToString();
                                Demo.Turnover = Convert.ToDouble(dr["Turnover"].ToString());
                                Demo.ProjectedTurnover = Convert.ToDouble(dr["ProjectedTurnover"].ToString());
                                Demo.CreatedBy = dr["CreatedBy"].ToString();
                                Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                objmodel.CustomerTurnoverList.Add(Demo);
                            }
                        }
                        objmodel.CustomerCertificationList = new List<CustomerCertificationModel>();
                        if (ds.Tables[5].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[5].Rows)
                            {
                                CustomerCertificationModel Demo = new CustomerCertificationModel();
                                Demo.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                                Demo.CustomerCertification = dr["CustomerCertification"].ToString();
                                Demo.CertificateDate = Convert.ToDateTime(dr["CertificateDate"].ToString());
                                if (Demo.CertificateDate != null)
                                    Demo.CertificateDateStr = Demo.CertificateDate.ToShortDateString();
                                Demo.CreatedBy = dr["CreatedBy"].ToString();
                                Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                objmodel.CustomerCertificationList.Add(Demo);
                            }
                        }
                        objmodel.CustomerCompetitorList = new List<CustomerCompetitorModel>();
                        if (ds.Tables[6].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[6].Rows)
                            {
                                CustomerCompetitorModel Demo = new CustomerCompetitorModel();
                                Demo.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                                Demo.CompetitorId = Convert.ToInt32(dr["CompetitorId"].ToString());
                                Demo.CompetitorName = dr["CustomerName"].ToString();
                                Demo.Region = dr["Region"].ToString();
                                Demo.IsActive = Convert.ToBoolean(dr["IsActive"].ToString());
                                Demo.CreatedBy = dr["CreatedBy"].ToString();
                                Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                objmodel.CustomerCompetitorList.Add(Demo);
                            }
                        }
                        objmodel.LevelList = new List<LevelModel>();
                        if (ds.Tables[7].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[7].Rows)
                            {
                                LevelModel Demo = new LevelModel();
                                Demo.LevelId = Convert.ToInt32(dr["LevelId"].ToString());
                                Demo.Description = dr["Description"].ToString();
                                objmodel.LevelList.Add(Demo);
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
        public CustomerModel GetSelectedCustomerContact(int CustomerID)
        {
            CustomerModel objmodel = new CustomerModel();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedCustomerContact(CustomerID);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        objmodel.CustmerContactLst = new List<CustomerContactDetailsModel>();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                CustomerContactDetailsModel Demo = new CustomerContactDetailsModel();
                                Demo.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                                Demo.CustomerContactId = Convert.ToInt32(dr["CustomerContactId"].ToString());
                                Demo.ContactName = dr["ContactName"].ToString();
                                Demo.Designation = dr["Designation"].ToString();
                                Demo.email = dr["emailId"].ToString();
                                Demo.MobileNo = dr["MobileNo"].ToString();
                                Demo.Experties = dr["Experties"].ToString();
                                Demo.Qualification = dr["Qualification"].ToString();
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
                                Demo.Remark = dr["Remark"].ToString();
                                objmodel.CustmerContactLst.Add(Demo);
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

        #region CPN

        public List<CPNModel> GetCPNList(int CustomerId)
        {
            List<CPNModel> LstCPNModel = new List<CPNModel>();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetCPNList(CustomerId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                CPNModel Demo = new CPNModel();
                                Demo.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                                Demo.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                                Demo.ItemName = dr["ItemName"].ToString();
                                Demo.MPN = dr["MPN"].ToString();
                                Demo.CPN = dr["CPN"].ToString();
                                Demo.CreatedBy = dr["CreatedBy"].ToString();
                                Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                                Demo.ModifiedBy = dr["ModifiedBy"].ToString();
                                Demo.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                                LstCPNModel.Add(Demo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstCPNModel;
        }
        public CPNModel GetSelectedCPNList(int CustomerId, int ItemId)
        {
            CPNModel CPNModel = new CPNModel();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedCPN(CustomerId, ItemId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            CPNModel.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString());
                            CPNModel.ItemId = Convert.ToInt32(ds.Tables[0].Rows[0]["ItemId"].ToString());
                            CPNModel.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                            CPNModel.MPN = ds.Tables[0].Rows[0]["MPN"].ToString();
                            CPNModel.CPN = ds.Tables[0].Rows[0]["CPN"].ToString();
                            CPNModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            CPNModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"]);
                            CPNModel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            CPNModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CPNModel;
        }
        public int saveCPNDetail(int CustomerId, int ItemId, string CPN, string User_Id)
        {
            CustomerDAL objdal = new CustomerDAL();
            return objdal.saveCPNDetail(CustomerId, ItemId, CPN, User_Id);
        }
        public List<CPNModel> GetItemListforCPN(int CustomerId)
        {
            List<CPNModel> LstModel = new List<CPNModel>();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetItemforCPNList(CustomerId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                CPNModel Demo = new CPNModel();
                                Demo.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                                Demo.ItemName = dr["ItemName"].ToString();
                                LstModel.Add(Demo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstModel;
        }

        #endregion CPN

        #region Assign User
        public int SaveAssignCustomerUserId(int userId, int CustomerContactId)
        {
            int errCode = 0;
            try
            {
                CustomerDAL DALObj = new CustomerDAL();
                errCode = DALObj.SaveAssignCustomerUserId(userId, CustomerContactId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int DeleteAssignCustomerUserId(int CustomerContactId)
        {
            int errCode = 0;
            try
            {
                CustomerDAL DALObj = new CustomerDAL();
                errCode = DALObj.DeleteAssignCustomerUserId(CustomerContactId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public CustUserModel GetCustUserList(int CustomerContactId)
        {
            CustUserModel CustModelList = new CustUserModel();
            CustModelList.LstUserModel = new List<CustUserModel>();
            try
            {
                CustomerDAL DALObj = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = DALObj.GetCustUserList(CustomerContactId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CustUserModel Model = new CustUserModel();
                            Model.UserId = Convert.ToInt32(dr["UserId"].ToString());
                            Model.UserName = dr["UserName"].ToString();
                            CustModelList.LstUserModel.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return CustModelList;
        }
        #endregion Assign User

        #region Product Work
        public List<ProductModel> GetProductList(int CustomerId)
        {
            CustomerModel objmodel = new CustomerModel();
            objmodel.ProductList = new List<ProductModel>();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.DWCustomerProductList(CustomerId);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProductModel Demo = new ProductModel();
                            Demo.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                            Demo.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            Demo.EquipmentName = dr["EquipmentName"].ToString();
                            Demo.CustomerName = dr["CustomerName"].ToString();
                            Demo.TAM = Convert.ToInt32(dr["TAM"].ToString());
                            Demo.ModifiedBy = dr["ModifiedBy"].ToString();
                            Demo.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
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
        public List<ItemModel> GetItemList()
        {
            List<ItemModel> lstItem = new List<ItemModel>();
            try
            {
                DataSet dsItem = new DataSet();
                CustomerDAL DALObj = new CustomerDAL();
                dsItem = DALObj.GetItemListForCustomer();
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
        public int DeleteCustomerEquipment(int CustomerId, int EquipmentId)
        {
            CustomerDAL objdal = new CustomerDAL();
            int errCode = 0;
            try
            {
                errCode = objdal.DeleteCustomerEquipment(CustomerId, EquipmentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        public int SaveproductDetails(ProductModel Productmodel, string User_Id)
        {

            CustomerDAL objdal = new CustomerDAL();
            int errCode = 0;
            try
            {
                errCode = objdal.Saveproduct(Productmodel.CustomerId, Productmodel.EquipmentId, Productmodel.TAM, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public List<ProductModel> ProductGetselected(int CustomerId)
        {
            List<ProductModel> objmodel = new List<ProductModel>();
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedProduct(CustomerId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                ProductModel model = new ProductModel();
                                model.EquipmentId = Convert.ToInt32(dr["EquipmentId"]);
                                model.EquipmentName = dr["EquipmentName"].ToString();
                                model.TAM = 0;
                                objmodel.Add(model);
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

        //public List<Itemmodel> GetSelectedItemByCustomer(int productid)
        //{
        //    ProductModel model = new ProductModel();
        //    model.ItemList = new List<Itemmodel>();
        //    try
        //    {
        //        CustomerDAL objdal = new CustomerDAL();
        //        DataSet ds = new DataSet();
        //        ds = objdal.GetSelectedItemByCustomer(productid);

        //        if (ds != null)
        //        {
        //            if (ds.Tables.Count > 0)
        //            {
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    foreach (DataRow dr in ds.Tables[0].Rows)
        //                    {
        //                        Itemmodel objmodel = new Itemmodel();
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
        public int saveItemDetails(string User_Id, int ItemId, double Quantity, int productid, string productname)
        {
            try
            {
                CustomerDAL objdal = new CustomerDAL();
                return objdal.Saveitem(User_Id, ItemId, Quantity, productid, productname);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Product Work

        #region Address Work

        public AddressModel GetAddressSelectedList(int CustomerId, int addressid)
        {
            AddressModel addressModel = new AddressModel();
            CustomerDAL DALobj = new CustomerDAL();
            try
            {
                DataSet DSObj = new DataSet();
                DSObj = DALobj.GetSelectedAddressDetail(CustomerId, addressid);
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        addressModel.AddressId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["AddressId"].ToString());
                        addressModel.Line1 = DSObj.Tables[0].Rows[0]["Line1"].ToString();
                        addressModel.Line2 = DSObj.Tables[0].Rows[0]["Line2"].ToString();
                        addressModel.LandMark = DSObj.Tables[0].Rows[0]["LandMark"].ToString();
                        addressModel.Pin = DSObj.Tables[0].Rows[0]["Pin"].ToString();
                        addressModel.Description = DSObj.Tables[0].Rows[0]["Description"].ToString();
                        addressModel.isPrimary = Convert.ToBoolean(DSObj.Tables[0].Rows[0]["isPrimary"].ToString());
                        addressModel.State = DSObj.Tables[0].Rows[0]["State"].ToString();
                        addressModel.Country = DSObj.Tables[0].Rows[0]["Country"].ToString();
                        addressModel.CountryId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["CountryId"].ToString());
                        addressModel.StateId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["StateId"].ToString());
                        addressModel.CityId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["CityId"].ToString());
                        addressModel.City = DSObj.Tables[0].Rows[0]["City"].ToString();
                        addressModel.PhotoPath = DSObj.Tables[0].Rows[0]["PhotoPath"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return addressModel;
        }

        public List<AddressModel> GetSelectedAddressByCustomer(int CustomerId)
        {

            List<AddressModel> addlst = new List<AddressModel>();

            try
            {
                CustomerDAL objdal = new CustomerDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedAddressByCustomer(CustomerId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                AddressModel objmodel = new AddressModel();
                                objmodel.Line1 = dr["Line1"].ToString();
                                objmodel.Line2 = dr["Line2"].ToString();
                                objmodel.LandMark = dr["LandMark"].ToString();
                                objmodel.AddressId = Convert.ToInt32(dr["AddressId"].ToString());
                                objmodel.State = dr["State"].ToString();
                                objmodel.City = dr["City"].ToString();
                                objmodel.Country = dr["Country"].ToString();

                                objmodel.StateId = Convert.ToInt32(dr["StateId"].ToString());
                                objmodel.CityId = Convert.ToInt32(dr["CityId"].ToString());
                                objmodel.CountryId = Convert.ToInt32(dr["CountryId"].ToString());

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


        public int SaveAddressDeatil(AddressModel addressmodel, Boolean isPrimary, string Description)
        {
            CustomerDAL objdal = new CustomerDAL();
            int ErrCode = 0;
            try
            {
                DataSet DSObj = new DataSet();
                DSObj = objdal.GetSelectedAddress(0);
                if (DSObj == null)
                    return 0;
                if (DSObj.Tables.Count > 0)
                {
                    DSObj.Tables[0].Rows.Clear();
                    DataRow Dr = DSObj.Tables[0].NewRow();
                    Dr["AddressId"] = addressmodel.AddressId;
                    Dr["Line1"] = addressmodel.Line1;
                    Dr["Line2"] = addressmodel.Line2;
                    Dr["StateId"] = addressmodel.StateId;
                    Dr["CityId"] = addressmodel.CityId;
                    Dr["CountryId"] = addressmodel.CountryId;
                    Dr["Pin"] = addressmodel.Pin;
                    Dr["LandMark"] = addressmodel.LandMark;
                    DSObj.Tables[0].Rows.Add(Dr);
                    ErrCode = objdal.SaveAdressDeatils(DSObj, addressmodel.CustomerId, isPrimary, Description, addressmodel.PhotoPath);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;

        }
        public List<CountryModel> GetCountryList()
        {
            AddressModel addressModel = new AddressModel();
            addressModel.LstCountry = new List<CountryModel>();
            try
            {
                CustomerDAL DALObj = new CustomerDAL();
                DataSet DSObj = new DataSet();
                DSObj = DALObj.GetCountryList();
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in DSObj.Tables[0].Rows)
                        {
                            CountryModel demo = new CountryModel();
                            demo.CountryId = dr["Country ID"].ToString();
                            demo.CountryName = dr["Country Name"].ToString();
                            addressModel.LstCountry.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return addressModel.LstCountry;
        }
        public List<StateModel> GetStateList(int Country)
        {
            AddressModel addressModel = new AddressModel();
            addressModel.LstState = new List<StateModel>();
            List<StateModel> lst = new List<StateModel>();
            try
            {
                CustomerDAL DALObj = new CustomerDAL();
                DataSet DSObj = new DataSet();
                DSObj = DALObj.GetStateList(Country);
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in DSObj.Tables[0].Rows)
                        {
                            StateModel demo = new StateModel();
                            demo.StateId = dr["State ID"].ToString();
                            demo.StateName = dr["State Name"].ToString();
                            addressModel.LstState.Add(demo);
                            lst.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lst;
        }
        public List<CityModel> GetCityList(int State)
        {
            AddressModel addressModel = new AddressModel();

            addressModel.LstCity = new List<CityModel>();
            try
            {
                CustomerDAL DALObj = new CustomerDAL();
                DataSet DSObj = new DataSet();
                DSObj = DALObj.GetCityList(State);
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in DSObj.Tables[0].Rows)
                        {
                            CityModel demo = new CityModel();
                            demo.CityId = dr["City ID"].ToString();
                            demo.CityName = dr["City Name"].ToString();
                            addressModel.LstCity.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return addressModel.LstCity;
        }
        #endregion Address Work

        #region Customer Mapping

        public List<CustomerMappingModel> GetDWCompList()
        {
            List<CustomerMappingModel> lstCustomerMap = new List<CustomerMappingModel>();

            try
            {
                CustomerDAL DALObj = new CustomerDAL();
                DataSet dsObj = new DataSet();
                dsObj = DALObj.GetDWCompCustomerList();
                if (dsObj != null)
                {
                    if (dsObj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            CustomerMappingModel MapObj = new CustomerMappingModel();
                            MapObj.CompCode = dr["CompCode"].ToString();
                            MapObj.CompName = dr["CompanyName"].ToString();
                            MapObj.ConnectionId = Convert.ToInt16(dr["ConnectionId"]);
                            MapObj.VendorListSP = dr["CustomerListSP"].ToString();
                            MapObj.SaveCustomerSP = dr["SaveCustomerSP"].ToString();
                            lstCustomerMap.Add(MapObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstCustomerMap;
        }
        public List<SysNavRelCustomerListModel> GetCustomerNavRelList(int CustomerId)
        {
            List<SysNavRelCustomerListModel> lstCustomerMap = new List<SysNavRelCustomerListModel>();
            try
            {
                DataSet dsobj = new DataSet();
                CustomerDAL DALObj = new CustomerDAL();
                dsobj = DALObj.GetCustomerNavRelList(CustomerId);
                if (dsobj != null)
                {
                    if (dsobj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            SysNavRelCustomerListModel MapTemp = new SysNavRelCustomerListModel();
                            MapTemp.CompCustomerID = dr["Customer_No"].ToString();
                            MapTemp.CompName = dr["CompCode"].ToString();
                            MapTemp.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            MapTemp.ConnectionId = Convert.ToInt16(dr["ConnectionId"].ToString());
                            lstCustomerMap.Add(MapTemp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstCustomerMap;
        }
        public string GetClientCustomerName(string CompName, string CompCustomerID, ArrayList ConnInfo)
        {
            string str = "";
            try
            {
                CustomerDAL DalObj = new CustomerDAL();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetClientCustomerName(CompName, CompCustomerID, ConnInfo);
                if (dsObj != null)
                {
                    str = dsObj.Tables[0].Rows[0]["CustomerDisplayName"].ToString();
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                str = "No Connection";
            }

            return str;
        }
        public List<MapClientCustomerList> GetCompanyCustomer(string CompCode, string SPName, ArrayList ConnInfo)
        {
            List<MapClientCustomerList> lstMapCustomer = new List<MapClientCustomerList>();
            try
            {
                DataSet dsobj = new DataSet();
                CustomerDAL DALObj = new CustomerDAL();
                dsobj = DALObj.GetCompanyCustomer(CompCode, SPName, ConnInfo);
                if (dsobj != null)
                {
                    if (dsobj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            MapClientCustomerList MapCustTemp = new MapClientCustomerList();
                            MapCustTemp.ClientCustomerID = dr["CustomerNo"].ToString();
                            MapCustTemp.ClientCustomerName = dr["CustomerDisplayName"].ToString();
                            lstMapCustomer.Add(MapCustTemp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstMapCustomer;
        }
        public string SaveCustomerMapping(CustomerMappingModel ItemMap, string User_Id)
        {
            string ErrorCode = "0";
            try
            {
                CustomerDAL DALObj = new CustomerDAL();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("tbl_DHFeedEventDep");
                dt.Columns.Add("Customer_No", typeof(System.String));
                dt.Columns.Add("CompCode", typeof(System.String));
                dt.Columns.Add("CustomerId", typeof(System.Int32));

                DataRow dr = dt.NewRow();
                dr["Customer_No"] = ItemMap.CompCustomerID;
                dr["CompCode"] = ItemMap.CompCode;
                dr["CustomerId"] = ItemMap.CustomerId;
                dt.Rows.Add(dr);
                dsObj.Tables.Add(dt);
                ErrorCode = DALObj.SaveCustomerMapping(dsObj, User_Id);
            }
            catch (Exception)
            {

                throw;
            }

            return ErrorCode;
        }
        public int DeleteCustomerMapping(string CompName, int CustomerId)
        {
            int errcod = 0;
            try
            {
                CustomerDAL DALObj = new CustomerDAL();
                errcod = DALObj.DeleteCustomerMapping(CompName, CustomerId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errcod;
        }

        public int GetAutoMapping(string User_Id, string CompCode, string CustomerName, int CustomerId, string SPName, ArrayList ConnArrList)
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
                dr["ParamName"] = "CustomerName";
                dr["ParamValue"] = CustomerName;
                dt.Rows.Add(dr);
                dsParam.Tables.Add(dt);
                DataSet Customer = new DataSet();
                Customer = objbl.ExecuteUnderlayingSP(dsParam, SPName, ConnArrList);
                if (Customer != null)
                {
                    if (Customer.Tables.Count > 0)
                    {
                        if (Customer.Tables[0].Rows.Count > 0)
                        {
                            CustomerDAL DALObj = new CustomerDAL();
                            ItemDAL ItemDAL = new ItemDAL();
                            DataSet dsDBData = new DataSet();
                            DataTable dtDBData = new DataTable("ItenNavRel");
                            dtDBData.Columns.Add("Customer_No", typeof(System.String));
                            dtDBData.Columns.Add("CompCode", typeof(System.String));
                            dtDBData.Columns.Add("CustomerId", typeof(System.Int32));

                            DataRow drDBData = dtDBData.NewRow();
                            drDBData["Customer_No"] = Customer.Tables[0].Rows[0]["CustomerNo"];
                            drDBData["CompCode"] = CompCode;
                            drDBData["CustomerId"] = CustomerId;
                            dtDBData.Rows.Add(drDBData);
                            dsDBData.Tables.Add(dtDBData);
                            string ErrorCode = DALObj.SaveCustomerMapping(dsDBData, User_Id);
                            if (ErrorCode == "500001" || ErrorCode == "500002")
                            {
                                string Description = "";
                                if (ErrorCode == "500001")
                                {
                                    Description = "Update Customer " + ", " + CompCode + " , " + CustomerName;
                                }
                                Description = "New Customer " + ", " + CompCode + " , " + CustomerName;
                                errCode = ItemDAL.SaveExtSysLog(Description, Customer.Tables[0].Rows[0]["CustomerNo"].ToString(), User_Id);
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
        #endregion Customer Mapping

        #region DW Customer Pendding  Approver List
        public List<PenndingCustomerApproverMap> GetCustomerApproverList(string User_Id, int Status_Id)
        {
            List<PenndingCustomerApproverMap> CustomerApproverList = new List<PenndingCustomerApproverMap>();
            try
            {
                DataSet ds = new DataSet();
                CustomerDAL DALObj = new CustomerDAL();
                ds = DALObj.GetCustomerPenddingApproverList(User_Id, Status_Id);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PenndingCustomerApproverMap temp = new PenndingCustomerApproverMap();
                            temp.CustomerId = Convert.ToInt32(dr["CustomerId"]);
                            temp.CustomerName = dr["CustomerName"].ToString();
                            temp.Customer_No = dr["Customer_No"].ToString();
                            temp.SourceCustomerName = dr["SourceCustomerName"].ToString();
                            temp.CompCode = dr["CompCode"].ToString();
                            temp.ModifiedBy = dr["ModifiedBy"].ToString();
                            temp.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            CustomerApproverList.Add(temp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return CustomerApproverList;
        }
        public int ApvRejCustomer(PenndingCustomerApproverMap CustomerApproveModel, int StatusCode, string User_Id)
        {
            int errCode = 0;
            try
            {
                CustomerDAL DALObj = new CustomerDAL();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("ApvRejCustomer");
                dt.Columns.Add("Customer_Id", typeof(System.String));
                dt.Columns.Add("CompCode", typeof(System.String));
                dt.Columns.Add("StatusCode", typeof(System.Int32));
                foreach (SelectPenddingCustomerApproveList demo in CustomerApproveModel.SelectApproverLst)
                {
                    DataRow dr = dt.NewRow();
                    dr["Customer_Id"] = demo.Customer_No;
                    dr["CompCode"] = demo.CompName;
                    dr["StatusCode"] = StatusCode;
                    dt.Rows.Add(dr);
                }
                dsObj.Tables.Add(dt);
                errCode = DALObj.ApvRejCustomer(dsObj, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion DW Customer Pendding  Approver List

        #region DW Customer Pendding  Approver List
        public int ApvRejCustomerReview(PenndingCustomerApproverMap CustomerApproveModel, int StatusCode, string User_Id)
        {
            int errCode = 0;
            try
            {
                CustomerDAL DALObj = new CustomerDAL();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("ApvRejCustomer");
                dt.Columns.Add("Customer_Id", typeof(System.String));
                dt.Columns.Add("CompCode", typeof(System.String));
                dt.Columns.Add("StatusCode", typeof(System.Int32));
                foreach (SelectPenddingCustomerApproveList demo in CustomerApproveModel.SelectApproverLst)
                {
                    DataRow dr = dt.NewRow();
                    dr["Customer_Id"] = demo.Customer_No;
                    dr["CompCode"] = demo.CompName;
                    dr["StatusCode"] = StatusCode;
                    dt.Rows.Add(dr);
                }
                dsObj.Tables.Add(dt);
                errCode = DALObj.ApvRejCustomerReview(dsObj, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion DW Customer Pendding  Approver List

    }
}
