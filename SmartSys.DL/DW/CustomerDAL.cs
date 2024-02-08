using MySql.Data.MySqlClient;
using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.DW
{
    public class CustomerDAL
    {
        public DataSet CustomerLIst()
        {
            DataSet dsCustomer = new DataSet();
            try
            {
                dsCustomer = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerlist");
            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsCustomer;
        }
        public DataSet CustomerLIstbySalesPerson(string User_Id)
        {
            DataSet dsCustomer = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                dsCustomer = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerlistBySalesPerson", parameters);
            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsCustomer;
        }
        public DataSet CustomerLIstHavingContacts(int ProjectId, string User_Id)
        {
            DataSet dsCustomer = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;
                dsCustomer = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerlistHavingcontact", parameters);
            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsCustomer;
        }
        public DataSet GetActiveCompanyList(string UserId)
        {
            DataSet dsCompany = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id",SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
                dsCompany = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysActiveCompanyList", ObjParam);
            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsCompany;
        }
        public DataSet GetSelectedCustomer(int CustomerID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                objParam[0].Value = CustomerID;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWgetselectedCustomerlist", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedCustomerContact(int CustomerID)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                objParam[0].Value = CustomerID;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWgetselectedCustomerContactlist", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int saveCustomer(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[11];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["CustomerId"];
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@CustomerName", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["CustomerName"];

                parameters[2] = new SqlParameter("@Region", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["Region"];

                parameters[3] = new SqlParameter("@IsActive", SqlDbType.Bit);
                parameters[3].Value = ds.Tables[0].Rows[0]["IsActive"];

                parameters[4] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@AuthorizedDealer", SqlDbType.Bit);
                parameters[5].Value = ds.Tables[0].Rows[0]["AuthorizedDealer"];

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                //   parameters[6].Direction = ParameterDirection.InputOutput;

                parameters[7] = new SqlParameter("@PTId", SqlDbType.Int);
                parameters[7].Value = ds.Tables[0].Rows[0]["PTId"];

                parameters[8] = new SqlParameter("@CustLevel", SqlDbType.Int);
                parameters[8].Value = ds.Tables[0].Rows[0]["CustLevel"];

                parameters[9] = new SqlParameter("@SalesPersonId", SqlDbType.Int);
                parameters[9].Value = ds.Tables[0].Rows[0]["SalesPersonId"];

                parameters[10] = new SqlParameter("@CommunicateToCSR", SqlDbType.Int);
                parameters[10].Value = ds.Tables[0].Rows[0]["CommToCSR"];
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveCustomer", parameters);
                if (parameters[0].Value != null)
                    errCode = Convert.ToInt32(parameters[0].Value.ToString());
                // int errCode1 = Convert.ToInt32(parameters[6].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        public int SaveLibaryDetailInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[3].Rows[0]["CustomerId"];

                parameters[1] = new SqlParameter("@DocumentTitle", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[3].Rows[0]["DocumentTitle"];

                parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[3].Rows[0]["Description"];

                parameters[3] = new SqlParameter("@DocumentPath", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[3].Rows[0]["DocumentPath"];

                parameters[4] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;

                parameters[6] = new SqlParameter("@IsKYC", SqlDbType.Bit);
                parameters[6].Value = ds.Tables[3].Rows[0]["IsKYC"]; ;


                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveCustomerLibraryDetails", parameters);
                if (parameters[5].Value != null)
                    errCode = Convert.ToInt32(parameters[5].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveBankDetailsInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[1].Rows[0]["CustomerId"];

                parameters[1] = new SqlParameter("@BankName", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[1].Rows[0]["BankName"];

                parameters[2] = new SqlParameter("@AccountNo", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[1].Rows[0]["AccountNo"];

                parameters[3] = new SqlParameter("@Limit", SqlDbType.Float);
                parameters[3].Value = ds.Tables[1].Rows[0]["Limit"];

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@NewAccountNo", SqlDbType.NVarChar);
                parameters[5].Value = ds.Tables[1].Rows[0]["NewAccountNo"];

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                parameters[6].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveCustomerBankDetails", parameters);
                if (parameters[6].Value != null)
                    errCode = Convert.ToInt32(parameters[6].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_DW_CustomerBankDetails'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }
        public DataSet GetSelectedBankDetail(int CustomerId, string AccountNo)
        {
            DataSet dsObj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;

                parameters[1] = new SqlParameter("@AccountNo", SqlDbType.VarChar);
                parameters[1].Value = AccountNo;

                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerBankGetSelected", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return dsObj;
        }
        public DataSet GetSelectedContactDetails(int CustomerId, string ContactName)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;

                parameters[1] = new SqlParameter("@ContactName", SqlDbType.VarChar);
                parameters[1].Value = ContactName;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerContactGetSelected", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }
        public DataSet GetSelectedTurnoverDetails(int CustomerId, string Turnover)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;

                parameters[1] = new SqlParameter("@TurnoverYear", SqlDbType.NVarChar);
                parameters[1].Value = Turnover;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerTurnoverGetSelected", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }
        public int saveContactDetailsInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[13];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[2].Rows[0]["CustomerId"];

                parameters[1] = new SqlParameter("@ContactName", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[2].Rows[0]["ContactName"];

                parameters[2] = new SqlParameter("@Designation", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[2].Rows[0]["Designation"];

                parameters[3] = new SqlParameter("@MobileNo", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[2].Rows[0]["MobileNo"];

                parameters[4] = new SqlParameter("@PhoneNo", SqlDbType.VarChar);
                parameters[4].Value = ds.Tables[2].Rows[0]["PhoneNo"];

                parameters[5] = new SqlParameter("@Experties", SqlDbType.VarChar);
                parameters[5].Value = ds.Tables[2].Rows[0]["Experties"];

                parameters[6] = new SqlParameter("@Qualification", SqlDbType.VarChar);
                parameters[6].Value = ds.Tables[2].Rows[0]["Qualification"];

                parameters[7] = new SqlParameter("@BirthDate", SqlDbType.SmallDateTime);
                parameters[7].Value = ds.Tables[2].Rows[0]["BirthDate"];

                parameters[8] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[8].Value = User_Id;

                parameters[9] = new SqlParameter("@NewContactName", SqlDbType.VarChar);
                parameters[9].Value = ds.Tables[2].Rows[0]["NewContactName"];

                parameters[10] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[10].Value = ds.Tables[2].Rows[0]["Remark"];

                parameters[11] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[11].Value = 0;
                parameters[11].Direction = ParameterDirection.InputOutput;

                parameters[12] = new SqlParameter("@email", SqlDbType.VarChar);
                parameters[12].Value = ds.Tables[2].Rows[0]["emailId"];

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveCustomerContactDetails", parameters);
                if (parameters[11].Value != null)
                    errCode = Convert.ToInt32(parameters[11].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_DW_CustomerContact'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }
        public int saveCustomerProfessionalInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[14];
                parameters[0] = new SqlParameter("@emailId", SqlDbType.VarChar);
                parameters[0].Value = ds.Tables[0].Rows[0]["emailId"];

                parameters[1] = new SqlParameter("@VAT", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["VAT"];

                parameters[2] = new SqlParameter("@PAN", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["PAN"];

                parameters[3] = new SqlParameter("@TAN", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[0].Rows[0]["TAN"];

                parameters[4] = new SqlParameter("@CST", SqlDbType.VarChar);
                parameters[4].Value = ds.Tables[0].Rows[0]["CST"];

                parameters[5] = new SqlParameter("@ExciseCommissionRate", SqlDbType.VarChar);
                parameters[5].Value = ds.Tables[0].Rows[0]["ExciseCommissionRate"];

                parameters[6] = new SqlParameter("@ExciseDivision", SqlDbType.VarChar);
                parameters[6].Value = ds.Tables[0].Rows[0]["ExciseDivision"];

                parameters[7] = new SqlParameter("@ExciseNo", SqlDbType.VarChar);
                parameters[7].Value = ds.Tables[0].Rows[0]["ExciseNo"];

                parameters[8] = new SqlParameter("@ExciseRange", SqlDbType.VarChar);
                parameters[8].Value = ds.Tables[0].Rows[0]["ExciseRange"];

                parameters[9] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[9].Value = User_Id;

                parameters[10] = new SqlParameter("@CustomerId", SqlDbType.VarChar);
                parameters[10].Value = ds.Tables[0].Rows[0]["CustomerId"];

                parameters[11] = new SqlParameter("@Website", SqlDbType.VarChar);
                parameters[11].Value = ds.Tables[0].Rows[0]["Website"];

                parameters[12] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[12].Value = ds.Tables[0].Rows[0]["Remark"];

                parameters[13] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[13].Value = 0;
                parameters[13].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveProfessionalInfo", parameters);
                if (parameters[13].Value != null)
                    errCode = Convert.ToInt32(parameters[13].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int saveTurnoverDetailsInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[4].Rows[0]["CustomerId"];

                parameters[1] = new SqlParameter("@TurnoverYear", SqlDbType.NVarChar);
                parameters[1].Value = ds.Tables[4].Rows[0]["TurnoverYear"];

                parameters[2] = new SqlParameter("@NewTurnoverYear", SqlDbType.NVarChar);
                parameters[2].Value = ds.Tables[4].Rows[0]["NewTurnoverYear"];

                parameters[3] = new SqlParameter("@Turnover", SqlDbType.Float);
                parameters[3].Value = ds.Tables[4].Rows[0]["Turnover"];

                parameters[4] = new SqlParameter("@ProjectedTurnover", SqlDbType.Float);
                parameters[4].Value = ds.Tables[4].Rows[0]["ProjectedTurnover"];

                parameters[5] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[5].Value = User_Id;

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                parameters[6].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveCustomerTurnoverDetails", parameters);
                if (parameters[6].Value != null)
                    errCode = Convert.ToInt32(parameters[6].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_DW_CustomerTurnover'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }

        public int saveCertificationDetailsInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[5].Rows[0]["CustomerId"];

                parameters[1] = new SqlParameter("@CustomerCertification", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[5].Rows[0]["CustomerCertification"];

                parameters[2] = new SqlParameter("@NewCustomerCertification", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[5].Rows[0]["NewCustomerCertification"];

                parameters[3] = new SqlParameter("@CertificateDate", SqlDbType.SmallDateTime);
                parameters[3].Value = ds.Tables[5].Rows[0]["CertificateDate"];

                parameters[4] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveCustomerCertificationDetails", parameters);
                if (parameters[5].Value != null)
                    errCode = Convert.ToInt32(parameters[5].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_DW_CustomerCertification'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }
        public DataSet GetSelectedCertificationDetails(int CustomerId, string CustomerCertification)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;

                parameters[1] = new SqlParameter("@CustomerCertification", SqlDbType.VarChar);
                parameters[1].Value = CustomerCertification;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerCertificationGetSelected", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }

        public int saveCompetitorDetailsInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[6].Rows[0]["CustomerId"];

                parameters[1] = new SqlParameter("@CompetitorId", SqlDbType.Int);
                parameters[1].Value = ds.Tables[6].Rows[0]["CompetitorId"];

                parameters[2] = new SqlParameter("@NewCompetitorId", SqlDbType.Int);
                parameters[2].Value = ds.Tables[6].Rows[0]["NewCompetitorId"];

                parameters[3] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveCustomerCompetitorDetails", parameters);
                if (parameters[4].Value != null)
                    errCode = Convert.ToInt32(parameters[4].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_DW_CustomerCompetitor'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }

        public DataSet GetSelectedCompetitorDetails(int CustomerId, int CompetitorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;

                parameters[1] = new SqlParameter("@CompetitorId", SqlDbType.Int);
                parameters[1].Value = CompetitorId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerCompetitorGetSelected", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }
        public DataSet GetCompetitorList()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCompetitorCustomerList");
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }
        #region CPN
        public DataSet GetCPNList(int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCPNListbyCustomer", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }
        public DataSet GetItemforCPNList(int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetItemListForCPN", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet GetSelectedCPN(int CustomerId, int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedCPNbyCustomer", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public int saveCPNDetail(int CustomerId, int ItemId, string CPN, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;

                parameters[2] = new SqlParameter("@CPN", SqlDbType.VarChar);
                parameters[2].Value = CPN;

                parameters[3] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveCustomerCPN", parameters);
                if (parameters[4].Value != null)
                    errCode = Convert.ToInt32(parameters[4].Value.ToString());
            }

            catch (Exception ex)
            {
                return errCode;
            }
            return errCode;
        }
        #endregion CPN

        #region Assign Customer User

        public DataSet GetCustUserList(int CustomerContactId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = CustomerContactId;

                parameters[1] = new SqlParameter("@IDType", SqlDbType.NVarChar);
                parameters[1].Value = "CUSTOMER";

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetUserListNonEmployee", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }
        public int SaveAssignCustomerUserId(int userId, int CustomerContactId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@userId", SqlDbType.Int);
                parameters[0].Value = userId;

                parameters[1] = new SqlParameter("@CustomerContactId", SqlDbType.Int);
                parameters[1].Value = CustomerContactId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMUpdateCustomerUser", parameters);
                if (parameters[2].Value != null)
                    errCode = Convert.ToInt32(parameters[2].Value.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return errCode;
        }
        public int DeleteAssignCustomerUserId(int CustomerContactId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CustomerContactId", SqlDbType.Int);
                parameters[0].Value = CustomerContactId;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeleteCustomerUser", parameters);
                if (parameters[1].Value != null)
                    errCode = Convert.ToInt32(parameters[1].Value.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return errCode;
        }
        #endregion Assign Customer User

        #region Product Work
        public DataSet DWCustomerProductList(int CustomerId)
        {
            DataSet dsCustomerproduct = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                objParam[0].Value = CustomerId;
                dsCustomerproduct = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetCustomerProductList", objParam);
            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsCustomerproduct;
        }
        public DataSet GetItemListForCustomer()
        {
            DataSet ds = new DataSet();
            try
            {

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetItemListforCustomer");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedProduct(int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                objParam[0].Value = CustomerId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedProductList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int Saveproduct(int CustomerId, int EquipmentId, int TAM, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;

                parameters[1] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                parameters[1].Value = EquipmentId;

                parameters[2] = new SqlParameter("@TAM", SqlDbType.Int);
                parameters[2].Value = TAM;

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveProductDetails", parameters);
                if (parameters[2].Value != null)
                    errCode = Convert.ToInt32(parameters[2].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'IX_tbl_CustomerProduct'")
                {
                    errCode = 500000;
                }
                if (Chk == "Violation of UNIQUE KEY constraint 'IX_tbl_CustomerProduct'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }

        public int DeleteCustomerEquipment(int CustomerId, int EquipmentId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;

                parameters[1] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                parameters[1].Value = EquipmentId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWUpdateCustEquipment", parameters);
                if (parameters[2].Value != null)
                    errCode = Convert.ToInt32(parameters[2].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
            return errCode;
        }
        public DataSet GetSelectedItemByCustomer(int ProductId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ProductId", SqlDbType.Int);
                objParam[0].Value = ProductId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedCustomerProductItemList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int Saveitem(string User_Id, int ItemId, double Quantity, int productid, string productname)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[8];
                parameters[0] = new SqlParameter("@ProductId", SqlDbType.Int);
                parameters[0].Value = productid;

                parameters[1] = new SqlParameter("@ProductName", SqlDbType.VarChar);
                parameters[1].Value = productname;

                parameters[2] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@Quantity", SqlDbType.Float);
                parameters[3].Value = Quantity;

                parameters[4] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[4].Value = ItemId;

                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveitemdetails", parameters);
                if (parameters[5].Value != null)
                    errCode = Convert.ToInt32(parameters[5].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'dbo.DW_CustomerProduct'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }



        #endregion Product Work

        #region Address Work
        public int DeleteAddressDetail(int CustomerId, int AddressId)
        {
            int errcode = 0;
            try
            {
                SqlParameter[] objparam = new SqlParameter[4];
                objparam[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                objparam[0].Value = CustomerId;

                objparam[1] = new SqlParameter("@AddressId", SqlDbType.Int);
                objparam[1].Value = AddressId;

                objparam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objparam[2].Value = 0;
                objparam[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DeleteCustomerAddressDetail", objparam);

                if (objparam[2].Value != null)
                    errcode = Convert.ToInt32(objparam[2].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }
        public int SaveAdressDeatils(DataSet ds, int CustomerID, Boolean isPrimary, string Description, string PhotoPath)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[13];

                parameters[0] = new SqlParameter("@AddressId", SqlDbType.VarChar);
                parameters[0].Value = ds.Tables[0].Rows[0]["AddressId"];

                parameters[1] = new SqlParameter("@Line1", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["Line1"];

                parameters[2] = new SqlParameter("@Line2", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["Line2"];

                parameters[3] = new SqlParameter("@State", SqlDbType.Int);
                parameters[3].Value = ds.Tables[0].Rows[0]["Stateid"];

                parameters[4] = new SqlParameter("@City", SqlDbType.Int);
                parameters[4].Value = ds.Tables[0].Rows[0]["CityId"];

                parameters[5] = new SqlParameter("@Country", SqlDbType.Int);
                parameters[5].Value = ds.Tables[0].Rows[0]["CountryId"];

                parameters[6] = new SqlParameter("@Pin", SqlDbType.VarChar);
                parameters[6].Value = ds.Tables[0].Rows[0]["Pin"];

                parameters[7] = new SqlParameter("@LandMark", SqlDbType.VarChar);
                parameters[7].Value = ds.Tables[0].Rows[0]["LandMark"];

                parameters[8] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[8].Value = CustomerID;

                parameters[9] = new SqlParameter("@isPrimary", SqlDbType.Bit);
                parameters[9].Value = isPrimary;

                parameters[10] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[10].Value = Description;

                parameters[11] = new SqlParameter("@PhotoPath", SqlDbType.VarChar);
                parameters[11].Value = PhotoPath;

                parameters[12] = new SqlParameter("@Errorcode", SqlDbType.Int);
                parameters[12].Value = 0;
                parameters[12].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveAddressDetails", parameters);
                if (parameters[12].Value != null)
                    return Convert.ToInt32(parameters[12].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }

        }
        public DataSet GetSelectedAddressDetail(int CustomerId, int AddressId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                objParam[0].Value = CustomerId;
                objParam[1] = new SqlParameter("@AddressId", SqlDbType.Int);
                objParam[1].Value = AddressId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWEditSelectedAddressByCustomer", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedAddress(int AddressId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@AddressId", SqlDbType.Int);
                objParam[0].Value = AddressId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetSelectedAddress", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }


        public DataSet GetSelectedAddressByCustomer(int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                objParam[0].Value = CustomerId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedAddressByCustomer", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetCountryList()
        {
            DataSet DSObj = new DataSet();
            try
            {
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetCountryList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public DataSet GetStateList(int Country)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@CountryId", SqlDbType.Int);
                objParam[0].Value = Country;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetStateList", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public DataSet GetCityList(int State)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@State", SqlDbType.VarChar);
                objParam[0].Value = State;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetCityList", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        #endregion Address Work 

        #region Customer Mapping]

        public DataSet GetDWCompCustomerList()
        {
            DataSet dsobj = new DataSet();
            try
            {

                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCompanyCustomerList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public DataSet GetCustomerNavRelList(int CustomerId)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;
                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCustomerNavRelList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;

        }
        public DataSet GetClientCustomerName(string CompName, string CompCustomerID, ArrayList ConnInfo)
        {
            DataSet dsobj = new DataSet();
            string ConnectionString = "";
            try
            {

                switch (ConnInfo[1].ToString())
                {
                    case "MySql":
                        ConnectionString = "SERVER=" + ConnInfo[2].ToString() + ";" + "DATABASE=" + ConnInfo[3].ToString() + ";" + "UID=" + ConnInfo[4].ToString() + ";" + "PASSWORD=" + ConnInfo[5].ToString() + ";";
                        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                        {
                            MySqlCommand sqlComm = new MySqlCommand("sp_GetCustomerName", connection);
                            sqlComm.Parameters.Add("CompanyCode", MySqlDbType.VarChar);
                            sqlComm.Parameters.Add("VendorCode", MySqlDbType.Int32);
                            sqlComm.Parameters["CompanyCode"].Value = CompName;
                            sqlComm.Parameters["VendorCode"].Value = CompCustomerID;
                            sqlComm.CommandType = CommandType.StoredProcedure;
                            MySqlDataAdapter da = new MySqlDataAdapter();
                            da.SelectCommand = sqlComm;
                            da.Fill(dsobj);
                        }

                        break;
                    default:
                        ConnectionString = "Data Source=" + ConnInfo[2].ToString() + ";" + "Initial Catalog=" + ConnInfo[3].ToString() + ";" + "uid=" + ConnInfo[4].ToString() + ";" + "pwd=" + ConnInfo[5].ToString() + ";";
                        SqlParameter[] parameters = new SqlParameter[2];
                        parameters[0] = new SqlParameter("@CompanyCode", SqlDbType.VarChar);
                        parameters[0].Value = CompName;

                        parameters[1] = new SqlParameter("@VendorCode", SqlDbType.VarChar);
                        parameters[1].Value = CompCustomerID;

                        dsobj = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "sp_GetCustomerName", parameters);
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public DataSet GetCompanyCustomer(string CompCode, string SPName, ArrayList ConnInfo)
        {
            DataSet dsEmpList = new DataSet();
            try
            {

                string ConnectionString = "";
                switch (ConnInfo[1].ToString())
                {
                    case "MySql":
                        ConnectionString = "SERVER=" + ConnInfo[2].ToString() + ";" + "DATABASE=" + ConnInfo[3].ToString() + ";" + "UID=" + ConnInfo[4].ToString() + ";" + "PASSWORD=" + ConnInfo[5].ToString() + ";";
                        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                        {
                            MySqlCommand sqlComm = new MySqlCommand(SPName, connection);
                            sqlComm.CommandType = CommandType.StoredProcedure;
                            MySqlDataAdapter da = new MySqlDataAdapter();
                            da.SelectCommand = sqlComm;
                            da.Fill(dsEmpList);
                        }

                        break;
                    default:
                        ConnectionString = "Data Source=" + ConnInfo[2].ToString() + ";" + "Initial Catalog=" + ConnInfo[3].ToString() + ";" + "uid=" + ConnInfo[4].ToString() + ";" + "pwd=" + ConnInfo[5].ToString() + ";";
                        SqlParameter[] objParam = new SqlParameter[1];
                        objParam[0] = new SqlParameter("@CompanyCode", SqlDbType.VarChar);
                        objParam[0].Value = CompCode;
                        dsEmpList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, SPName, objParam);
                        break;
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;
        }
        public string SaveCustomerMapping(DataSet dsCustomerMapping, string User_Id)
        {

            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@Customer_No", SqlDbType.VarChar);
                parameters[0].Value = dsCustomerMapping.Tables[0].Rows[0]["Customer_No"].ToString();


                parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[1].Value = dsCustomerMapping.Tables[0].Rows[0]["CompCode"].ToString(); ;

                parameters[2] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[2].Value = Convert.ToInt32(dsCustomerMapping.Tables[0].Rows[0]["CustomerId"].ToString());

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;
                DataSet ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveCustomerMapping", parameters);

                if (parameters[4].Value != null)
                {
                    if (parameters[4].Value.ToString() == "500000")
                    {
                        if (ds != null)
                            if (ds.Tables.Count > 0)
                                if (ds.Tables[0].Rows.Count > 0)
                                {

                                    return ds.Tables[0].Rows[0][0].ToString();
                                }
                    }
                    return parameters[4].Value.ToString();
                }
                else
                    return "0";
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_DW_CustomerNavRel'")
                {
                    return "500000";
                }
                return "0";
            }

        }
        public int DeleteCustomerMapping(string CompName, int CustomerId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CompName", SqlDbType.VarChar);
                parameters[0].Value = CompName;

                parameters[1] = new SqlParameter("@CustomerId", SqlDbType.VarChar);
                parameters[1].Value = CustomerId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDeleteCustomerNavRel", parameters);

                if (parameters[2].Value != null)
                    return Convert.ToInt32(parameters[2].Value.ToString());
                else
                    return 0;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }
        #endregion Customer Mapping 

        #region Pendding Approval Vendor Mapping
        public DataSet GetCustomerPenddingApproverList(string User_Id, int Status_Id)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                parameters[1] = new SqlParameter("@Status_Id", SqlDbType.Int);
                parameters[1].Value = Status_Id;

                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCustomerMapListByStatus", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public int ApvRejCustomer(DataSet dsObj, string User_Id)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in dsObj.Tables[0].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@Customer_Id", SqlDbType.VarChar);
                    parameters[0].Value = dr["Customer_Id"];

                    parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                    parameters[1].Value = dr["CompCode"];

                    parameters[2] = new SqlParameter("@StatusCode", SqlDbType.Int);
                    parameters[2].Value = dr["StatusCode"];

                    parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[3].Value = User_Id;

                    parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[4].Value = 0;
                    parameters[4].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerApproveReject", parameters);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion Pendding Approval Vendor Mapping

        #region Pendding Review Vendor Mapping

        public int ApvRejCustomerReview(DataSet dsObj, string User_Id)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in dsObj.Tables[0].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@Customer_Id", SqlDbType.VarChar);
                    parameters[0].Value = dr["Customer_Id"];

                    parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                    parameters[1].Value = dr["CompCode"];

                    parameters[2] = new SqlParameter("@StatusCode", SqlDbType.Int);
                    parameters[2].Value = dr["StatusCode"];

                    parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[3].Value = User_Id;

                    parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[4].Value = 0;
                    parameters[4].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerReviewReject", parameters);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion Pendding Review Vendor Mapping

    }
}
