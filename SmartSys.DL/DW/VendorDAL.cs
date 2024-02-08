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
    public class VendorDAL
    {
        public DataSet VendorList(string User_Id)
        {
            DataSet dsvendor = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                dsvendor = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWvendorlist", objParam);
            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsvendor;
        }
        public DataSet VendorListHavingVendorContacts(int ProjectId, string User_Id)
        {
            DataSet dsvendor = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                objParam[0].Value = ProjectId;

                objParam[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[1].Value = User_Id;
                dsvendor = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWvendorlistHavingVendorContact", objParam);
            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsvendor;
        }
        public DataSet GetSelectedVendor(int VendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWgetselectedvendorlist", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedVendorContact(int VendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWgetselectedvendorContactlist", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int savevendor(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[8];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["VendorId"];
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("VendorName", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["VendorName"];

                parameters[2] = new SqlParameter("Region", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["Region"];
               
                parameters[3] = new SqlParameter("IsActive", SqlDbType.Bit);
                parameters[3].Value = ds.Tables[0].Rows[0]["IsActive"]; ;
           
                parameters[4] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@AuthorizedDealer", SqlDbType.Bit);
                parameters[5].Value = ds.Tables[0].Rows[0]["AuthorizedDealer"];

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                parameters[6].Direction = ParameterDirection.InputOutput;

                parameters[7] = new SqlParameter("@IsManufacturer", SqlDbType.Bit);
                parameters[7].Value = ds.Tables[0].Rows[0]["IsManufacturer"];

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_Dw_savevendor", parameters);
                if (parameters[0].Value != null)
                    errCode = Convert.ToInt32(parameters[0].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        public int saveVendirProfessionalInfo(DataSet ds, string User_Id)
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

                parameters[10] = new SqlParameter("@VendorId", SqlDbType.VarChar);
                parameters[10].Value = ds.Tables[0].Rows[0]["VendorId"];

                parameters[11] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[11].Value = ds.Tables[0].Rows[0]["Remark"];

                parameters[12] = new SqlParameter("@Website", SqlDbType.VarChar);
                parameters[12].Value = ds.Tables[0].Rows[0]["Website"];


                parameters[13] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[13].Value = 0;
                parameters[13].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveVendorProfessionalInfo", parameters);
                if (parameters[13].Value != null)
                    errCode = Convert.ToInt32(parameters[13].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        public DataSet GetSelectedVendorContactDetails(int VendorId, string ContactName)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = VendorId;

                parameters[1] = new SqlParameter("@ContactName", SqlDbType.VarChar);
                parameters[1].Value = ContactName;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorContactGetSelected", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }
        public int saveVendorContactDetailsInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[13];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[2].Rows[0]["VendorId"];

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
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveVendorContactDetails", parameters);
                if (parameters[11].Value != null)
                    errCode = Convert.ToInt32(parameters[11].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_DW_VendorContact'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }

        public DataSet GetSelectedVendorBankDetail(int VendorId, string AccountNo)
        {
            DataSet dsObj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = VendorId;

                parameters[1] = new SqlParameter("@AccountNo", SqlDbType.VarChar);
                parameters[1].Value = AccountNo;

                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorBankGetSelected", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return dsObj;
        }

        public int SaveVendorBankDetailsInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[1].Rows[0]["VendorId"];

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

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveVendorBankDetails", parameters);
                if (parameters[6].Value != null)
                    errCode = Convert.ToInt32(parameters[6].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_DW_VendorBankDetails'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }

        public int SaveVendorLibaryDetailInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[3].Rows[0]["VendorId"];

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

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveVendorLibraryDetails", parameters);
                if (parameters[5].Value != null)
                    errCode = Convert.ToInt32(parameters[5].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
      
        public int saveVendorCertificationDetailsInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[5].Rows[0]["VendorId"];

                parameters[1] = new SqlParameter("@VendorCertification", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[5].Rows[0]["VendorCertification"];

                parameters[2] = new SqlParameter("@NewVendorCertification", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[5].Rows[0]["NewCustomerCertification"];

                parameters[3] = new SqlParameter("@CertificateDate", SqlDbType.SmallDateTime);
                parameters[3].Value = ds.Tables[5].Rows[0]["CertificateDate"];

                parameters[4] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveVendorCertificationDetails", parameters);
                if (parameters[5].Value != null)
                    errCode = Convert.ToInt32(parameters[5].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_DW_VendorCertification'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }
        public DataSet GetSelectedVendoCertificationDetails(int VendorId, string CustomerCertification)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = VendorId;

                parameters[1] = new SqlParameter("@VendorCertification", SqlDbType.VarChar);
                parameters[1].Value = CustomerCertification;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorCertificationGetSelected", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }

        public int saveVendorCompetitorDetailsInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[6].Rows[0]["VendorId"];

                parameters[1] = new SqlParameter("@CompetitorId", SqlDbType.Int);
                parameters[1].Value = ds.Tables[6].Rows[0]["CompetitorId"];

                parameters[2] = new SqlParameter("@NewCompetitorId", SqlDbType.Int);
                parameters[2].Value = ds.Tables[6].Rows[0]["NewCompetitorId"];

                parameters[3] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveVendorCompetitorDetails", parameters);
                if (parameters[4].Value != null)
                    errCode = Convert.ToInt32(parameters[4].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_DW_VendorCompetitor'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }

        public DataSet GetSelectedVendorCompetitorDetails(int VendorId, int CompetitorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = VendorId;

                parameters[1] = new SqlParameter("@CompetitorId", SqlDbType.Int);
                parameters[1].Value = CompetitorId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorCompetitorGetSelected", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }
        public DataSet GetVendorCompetitorList()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCompetitorVendorList");
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }

        public int saveVendorTurnoverDetailsInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[4].Rows[0]["VendorId"];

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

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveVendorTurnoverDetails", parameters);
                if (parameters[6].Value != null)
                    errCode = Convert.ToInt32(parameters[6].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_DW_VendorTurnover'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }

        public DataSet GetSelectedVendorTurnoverDetails(int VendorId, string Turnover)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = VendorId;

                parameters[1] = new SqlParameter("@TurnoverYear", SqlDbType.NVarChar);
                parameters[1].Value = Turnover;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorTurnoverGetSelected", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }

        #region Assign Customer User

        public DataSet GetVendtUserList(int VendorContactId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = VendorContactId;

                parameters[1] = new SqlParameter("@IDType", SqlDbType.NVarChar);
                parameters[1].Value = "VENDOR";

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetUserListNonEmployee", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }
        public int SaveAssignVendorUserId(int userId, int VendorContactId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@userId", SqlDbType.Int);
                parameters[0].Value = userId;

                parameters[1] = new SqlParameter("@VendorContactId", SqlDbType.Int);
                parameters[1].Value = VendorContactId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMUpdateVendorUser", parameters);
                if (parameters[2].Value != null)
                    errCode = Convert.ToInt32(parameters[2].Value.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            return errCode;
        }
        public int DeleteAssignVendorUserId(int VendorContactId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@VendorContactId", SqlDbType.Int);
                parameters[0].Value = VendorContactId;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeleteVendorUser", parameters);
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
        public int SaveVendorAdressDeatils(DataSet ds, int VendorId, Boolean isPrimary, string Description)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[12];

                parameters[0] = new SqlParameter("@AddressId", SqlDbType.VarChar);
                parameters[0].Value = ds.Tables[0].Rows[0]["AddressId"];

                parameters[1] = new SqlParameter("@Line1", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["Line1"];

                parameters[2] = new SqlParameter("@Line2", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["Line2"];

                parameters[3] = new SqlParameter("@State", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[0].Rows[0]["State"];

                parameters[4] = new SqlParameter("@City", SqlDbType.VarChar);
                parameters[4].Value = ds.Tables[0].Rows[0]["City"];

                parameters[5] = new SqlParameter("@Country", SqlDbType.VarChar);
                parameters[5].Value = ds.Tables[0].Rows[0]["Country"];

                parameters[6] = new SqlParameter("@Pin", SqlDbType.VarChar);
                parameters[6].Value = ds.Tables[0].Rows[0]["Pin"];

                parameters[7] = new SqlParameter("@LandMark", SqlDbType.VarChar);
                parameters[7].Value = ds.Tables[0].Rows[0]["LandMark"];

                parameters[8] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[8].Value = VendorId;

                parameters[9] = new SqlParameter("@isPrimary", SqlDbType.Bit);
                parameters[9].Value = isPrimary;

                parameters[10] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[10].Value = Description;

                parameters[11] = new SqlParameter("@Errorcode", SqlDbType.Int);
                parameters[11].Value = 0;
                parameters[11].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveVendorAddressDetails", parameters);
                if (parameters[11].Value != null)
                    return Convert.ToInt32(parameters[11].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }

        }
        public DataSet GetSelectedVendorAddressDetail(int VendorId, int AddressId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                objParam[1] = new SqlParameter("@AddressId", SqlDbType.Int);
                objParam[1].Value = AddressId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSelectedAddressByVendor", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedVendorAddress(int AddressId)
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


        public DataSet GetSelectedAddressByVendor(int VendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedAddressByVendor", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

       #endregion Address Work 

        #region Product Work
        public DataSet DWVendorProductList(int VendorId)
        {
            DataSet dsVendorproduct = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                dsVendorproduct = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetVendorProductList", objParam);
            }
            catch (Exception ex)
            {
                 Common.LogException(ex);
            }
            return dsVendorproduct;
        }
        public DataSet GetItemListForVendor()
        {
            DataSet ds = new DataSet();
            try
            {

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetItemListforVendor");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedVendorProduct(int VendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedVendorProductList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveVendorproduct(int VendorId,int EquipmentId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = VendorId;

                parameters[1] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                parameters[1].Value = EquipmentId;               

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveVendorProductDetails", parameters);
                if (parameters[2].Value != null)
                    errCode = Convert.ToInt32(parameters[2].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of UNIQUE KEY constraint 'IX_DW_VendorProduct'")
                {
                    errCode = 500000;
                }
                if (Chk == "Violation of UNIQUE KEY constraint 'IX_tbl_VendorProduct'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }
        public int DeleteVendorEquipment(int VendorId, int EquipmentId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = VendorId;

                parameters[1] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                parameters[1].Value = EquipmentId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWUpdateVendorEquipment", parameters);
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
        public DataSet GetSelectedItemByVendor(int ProductId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ProductId", SqlDbType.Int);
                objParam[0].Value = ProductId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedVendorProductItemList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveVendoritem(string User_Id, int ItemId, double Quantity, int productid, string productname)
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

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveVendorItemdetails", parameters);
                if (parameters[5].Value != null)
                    errCode = Convert.ToInt32(parameters[5].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'dbo.DW_VendorProduct'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }



        #endregion Product Work

        #region Vendor Mapping

        public DataSet GetVendorNavRelList(int VendorId)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = VendorId;
                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetVendorNavRelList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;

        }
        public DataSet GetDWCompVendorList()
        {
            DataSet dsobj = new DataSet();
            try
            {

                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCompanyVendorList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public DataSet GetClientVendorName(string CompName, string CompItemID, ArrayList ConnInfo)
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
                            MySqlCommand sqlComm = new MySqlCommand("sp_GetVendorName", connection);
                            sqlComm.Parameters.Add("CompanyCode", MySqlDbType.VarChar);
                            sqlComm.Parameters.Add("VendorCode", MySqlDbType.Int32);
                            sqlComm.Parameters["CompanyCode"].Value = CompName;
                            sqlComm.Parameters["VendorCode"].Value = CompItemID;
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
                        parameters[1].Value = CompItemID;
                        dsobj = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "sp_GetVendorName", parameters);
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public string SaveMapping(DataSet dsVendorMapping, string User_Id)
        {

            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@Vendor_No", SqlDbType.VarChar);
                parameters[0].Value = dsVendorMapping.Tables[0].Rows[0]["Vendor_No"].ToString();


                parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[1].Value = dsVendorMapping.Tables[0].Rows[0]["CompCode"].ToString(); ;

                parameters[2] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[2].Value = Convert.ToInt32(dsVendorMapping.Tables[0].Rows[0]["VendorId"].ToString());

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;
                DataSet ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveVendorMapping", parameters);

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
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_DW_VendorNavRel'")
                {
                    return "500000";
                }
                return "0";
            }

        }
        public int DeleteVendorMapping(string CompName, int VendorId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CompName", SqlDbType.VarChar);
                parameters[0].Value = CompName;

                parameters[1] = new SqlParameter("@VendorId", SqlDbType.VarChar);
                parameters[1].Value = VendorId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDeleteVendorNavRel", parameters);

                if(parameters[2].Value != null)
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
        public DataSet GetCompanyVendor(string CompCode, string SPName, ArrayList ConnInfo)
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
        #endregion Vendor Mapping

        #region Pendding Approval Vendor Mapping
        public DataSet GetVenderPenddingApproverList(string User_Id, int StatusCode)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                parameters[1] = new SqlParameter("@Status_Id", SqlDbType.Int);
                parameters[1].Value = StatusCode;

                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetVendorMapListByStatus", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public int ApvRejVendor(DataSet dsObj, string User_Id)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in dsObj.Tables[0].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@Vendor_Id", SqlDbType.VarChar);
                    parameters[0].Value = dr["Vendor_Id"];

                    parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                    parameters[1].Value = dr["CompCode"];

                    parameters[2] = new SqlParameter("@StatusCode", SqlDbType.Int);
                    parameters[2].Value = dr["StatusCode"];

                    parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[3].Value = User_Id;

                    parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[4].Value = 0;
                    parameters[4].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorApproveReject", parameters);
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
        public int ApvRejVendorReview(DataSet dsObj, string User_Id)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in dsObj.Tables[0].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@Vendor_Id", SqlDbType.VarChar);
                    parameters[0].Value = dr["Vendor_Id"];

                    parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                    parameters[1].Value = dr["CompCode"];

                    parameters[2] = new SqlParameter("@StatusCode", SqlDbType.Int);
                    parameters[2].Value = dr["StatusCode"];

                    parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[3].Value = User_Id;

                    parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[4].Value = 0;
                    parameters[4].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorReviwReject", parameters);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion Pendding Review Vendor Mapping

        #region FreightForwarder
        public DataSet GetFreightForwarderList()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysFreightForwarderList");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDropVendorList()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDrpVendorList");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetSelectedFreightFdetail(int FFId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@FFId", SqlDbType.Int);
                objParam[0].Value = FFId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedFreightForwordor", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int saveFreightForwordarDeatail(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@FFId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["FFId"];

                parameters[1] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[1].Value = ds.Tables[0].Rows[0]["VendorId"];

                parameters[2] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveFreightForWardorDetail", parameters);
                if (parameters[3].Value != null)
                    errCode = Convert.ToInt32(parameters[3].Value.ToString());
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        public int DeleteFreightForwardorDetail(int FFId)
        {
            int errcode = 0;
            try
            {

                SqlParameter[] objparam = new SqlParameter[2];


                objparam[0] = new SqlParameter("@FFId", SqlDbType.Int);
                objparam[0].Value = FFId;

                objparam[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objparam[1].Value = 0;
                objparam[1].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DeleteFreightForwordorDetail", objparam);

                if (objparam[1].Value != null)
                    errcode = Convert.ToInt32(objparam[1].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }
        #endregion FreightForwarder        
        #region Vendor Brand Allocation 
        public DataSet GetVendorAllocateList(int VendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorBrandList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveAllocatedVendorBrand(DataSet ds,string User_Id,int VendorId)
        {
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            int ErrCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;

                objParam[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[1].Value = 0;
                objParam[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWDeleteVendorBrand", objParam);
                if (objParam[1].Value != null)
                    ErrCode = Convert.ToInt32(objParam[1].Value.ToString());
                if (ErrCode == 500003)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SqlParameter[] Parameter = new SqlParameter[4];
                        Parameter[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                        Parameter[0].Value = dr["VendorId"];

                        Parameter[1] = new SqlParameter("@BrandId", SqlDbType.Int);
                        Parameter[1].Value = dr["BrandId"];

                        Parameter[2] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                        Parameter[2].Value = User_Id;

                        Parameter[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                        Parameter[3].Value = 0;
                        Parameter[3].Direction = ParameterDirection.InputOutput;
                        SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWSaveVendorBrandDetail", Parameter);
                        if (Parameter[3].Value != null)
                            ErrCode = Convert.ToInt32(Parameter[3].Value.ToString());
                        else
                            return 0;
                        if (ErrCode != 500002)
                        {
                            sqlTransaction.Rollback();
                            sqlConn.Close();
                        }
                    }
                }
                sqlTransaction.Commit();
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                sqlTransaction.Rollback();
                sqlConn.Close();
            }
            return ErrCode;
        }
        #endregion Vendor Brand Allocation


        public DataSet GetVendorIdList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptVendorId", parameters);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }


    }
}
