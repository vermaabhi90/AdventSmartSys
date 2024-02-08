using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.Bank
{
    public class BankDAL
    {
        public DataSet BankDetailsList(string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWBankDetailList", ObjParam);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }

        public DataSet GetSelectedBankDetail(int BankId)
        {

            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] Parameter = new SqlParameter[1];
                Parameter[0] = new SqlParameter("@BankId", SqlDbType.Int);
                Parameter[0].Value = BankId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedBankDetail", Parameter);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }


        public DataSet GetSelectedAddressList(int AddressId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] Parameter = new SqlParameter[1];
                Parameter[0] = new SqlParameter("@AddressId", SqlDbType.Int);
                Parameter[0].Value = AddressId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWAddressGetSelected", Parameter);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ds;
        }





        public int SaveAddressDetails(DataSet ds, DataSet ds1, string User_Id)
        {
            SqlConnection cn = new SqlConnection(Common.SqlConnectionString);
            cn.Open();
            SqlTransaction Tr = cn.BeginTransaction();

            try
            {

                SqlParameter[] parameter = new SqlParameter[10];

                parameter[0] = new SqlParameter("@AddressId", SqlDbType.Int);
                parameter[0].Value = ds.Tables[0].Rows[0]["AddressId"];
                parameter[0].Direction = ParameterDirection.InputOutput;

                parameter[1] = new SqlParameter("@Line1", SqlDbType.VarChar);
                parameter[1].Value = ds.Tables[0].Rows[0]["Line1"];

                parameter[2] = new SqlParameter("@Line2", SqlDbType.VarChar);
                parameter[2].Value = ds.Tables[0].Rows[0]["Line2"];

                parameter[3] = new SqlParameter("@State", SqlDbType.Int);
                parameter[3].Value = ds.Tables[0].Rows[0]["StateId"];

                parameter[4] = new SqlParameter("@City", SqlDbType.Int);
                parameter[4].Value = ds.Tables[0].Rows[0]["CityId"];

                parameter[5] = new SqlParameter("@Country", SqlDbType.Int);
                parameter[5].Value = ds.Tables[0].Rows[0]["CountryId"];

                parameter[6] = new SqlParameter("@Pin", SqlDbType.VarChar);
                parameter[6].Value = ds.Tables[0].Rows[0]["Pin"];

                parameter[7] = new SqlParameter("@LandMark", SqlDbType.VarChar);
                parameter[7].Value = ds.Tables[0].Rows[0]["LandMark"];


                parameter[8] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameter[8].Value = 0;
                parameter[8].Direction = ParameterDirection.InputOutput;

                parameter[9] = new SqlParameter("@BankId", SqlDbType.Int);
                parameter[9].Value = 0;
                SqlHelper.ExecuteDataset(Tr, CommandType.StoredProcedure, "sp_DwSaveAddresDetail", parameter);

                if (Convert.ToInt32(parameter[8].Value.ToString()) == 500002)
                {
                    if (parameter[0].Value != null)
                    {
                        //foreach (DataRow dr in ds.Tables[0].Rows)
                        //{
                        //foreach(DataRow dr1 in ds.Tables[1].Rows)
                        //{
                        //  dr1["TaxId"] =Convert.ToInt32(parameter[0].Value.ToString());
                        //  if (!(SaveTaxOnTax(Convert.ToInt16(dr1["TaxId"].ToString()), Convert.ToInt32(dr["ParentTaxId"].ToString()),Tr) == 500002))
                        if (!(SaveBankDetails(Convert.ToInt32(parameter[0].Value.ToString()), ds1, User_Id, Tr) == 500002))
                        {
                            Tr.Rollback();
                            cn.Close();
                            return 2;
                        }
                        //  }
                        //}
                        Tr.Commit();
                        cn.Close();
                        return 4;
                    }
                    else
                    {
                        Tr.Rollback();
                        cn.Close();
                        return 1;
                    }
                }
                else
                {
                    Tr.Rollback();
                    cn.Close();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Tr.Rollback();
                cn.Close();
                throw ex;
            }
        }

        private int SaveBankDetails(int AddressId, DataSet ds1, string User_Id, SqlTransaction tr)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@BankId", SqlDbType.Int);
                parameters[0].Value = ds1.Tables[0].Rows[0]["BankId"];

                parameters[1] = new SqlParameter("@BankName", SqlDbType.VarChar);
                parameters[1].Value = ds1.Tables[0].Rows[0]["BankName"];

                parameters[2] = new SqlParameter("@AddressId", SqlDbType.Int);
                parameters[2].Value = AddressId;

                parameters[3] = new SqlParameter("@IFSCCode", SqlDbType.VarChar);
                parameters[3].Value = ds1.Tables[0].Rows[0]["IFSCCode"];

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;


                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_DwSaveBankDetail", parameters);
                if (parameters[5].Value != null)
                    return Convert.ToInt32(parameters[5].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public int SaveEditedBankDetails(DataSet ds, string User_Id)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@BankId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["BankId"];

                parameters[1] = new SqlParameter("@BankName", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["BankName"];

                parameters[2] = new SqlParameter("@AddressId", SqlDbType.Int);
                parameters[2].Value = ds.Tables[0].Rows[0]["AddressId"];

                parameters[3] = new SqlParameter("@IFSCCode", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[0].Rows[0]["IFSCCode"];

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;


                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveBankDetail", parameters);
                if (parameters[5].Value != null)
                    return Convert.ToInt32(parameters[5].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int SaveEditedAddressDetails(DataSet ds, int Bankid)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[10];
                parameters[0] = new SqlParameter("@AddressId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["AddressId"];

                parameters[1] = new SqlParameter("@Line1", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["Line1"];

                parameters[2] = new SqlParameter("@Line2", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["Line2"];

                parameters[3] = new SqlParameter("@State", SqlDbType.Int);
                parameters[3].Value = ds.Tables[0].Rows[0]["StateId"];

                parameters[4] = new SqlParameter("City", SqlDbType.Int);
                parameters[4].Value = ds.Tables[0].Rows[0]["CityId"];

                parameters[5] = new SqlParameter("@Country", SqlDbType.Int);
                parameters[5].Value = ds.Tables[0].Rows[0]["CountryId"];


                parameters[6] = new SqlParameter("@Pin", SqlDbType.VarChar);
                parameters[6].Value = ds.Tables[0].Rows[0]["Pin"];

                parameters[7] = new SqlParameter("@LandMark", SqlDbType.VarChar);
                parameters[7].Value = ds.Tables[0].Rows[0]["LandMark"];

                parameters[8] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[8].Value = 0;

                parameters[9] = new SqlParameter("@BankId", SqlDbType.Int);
                parameters[9].Value = Bankid;

                parameters[9].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveAddresDetail", parameters);
                if (parameters[9].Value != null)
                    return Convert.ToInt32(parameters[9].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int DeleteBankDetails(int BankId,string User_Id)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@BankId", SqlDbType.Int);
                parameters[0].Value = BankId;
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_UpdateBankDetail", parameters);
                if (parameters[0].Value != null)
                    return Convert.ToInt32(parameters[0].Value.ToString());
                else
                    return 0;
            }


            catch (Exception ex)
            {
                throw ex;
            }
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
        public DataSet GetStateList(string Country)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@Country", SqlDbType.VarChar);
                objParam[0].Value = Country;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetStateList", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }

        public DataSet GetCityList(string State)
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
        #region CompanyBankDetail
        public DataSet GetCompanyBankDetailList()
        {
            DataSet DSObj = new DataSet();
            try
            {
          
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCompanyBankDetailsList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public DataSet GetSelectedCompanyBankDetail(int CompBankAcId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@CompBankAcId", SqlDbType.Int);
                objParam[0].Value = CompBankAcId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedCompanyBankDetails", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public int SaveCompanyBankDetail(DataSet ds, string User_Id)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[10];
                parameters[0] = new SqlParameter("@CompBankAcId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["CompBankAcId"];

                parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["CompCode"];

                parameters[2] = new SqlParameter("@BankId", SqlDbType.Int);
                parameters[2].Value = ds.Tables[0].Rows[0]["BankId"];

                parameters[3] = new SqlParameter("@AccountNumber", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[0].Rows[0]["AccountNumber"];

                parameters[4] = new SqlParameter("Remark", SqlDbType.VarChar);
                parameters[4].Value = ds.Tables[0].Rows[0]["Remark"];

                parameters[5] = new SqlParameter("@isActive", SqlDbType.Bit);
                parameters[5].Value = ds.Tables[0].Rows[0]["isActive"];

                parameters[6] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[6].Value = User_Id;

                parameters[7] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[7].Value = 0;
                parameters[7].Direction = ParameterDirection.InputOutput;


                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveCompanyBankDetails", parameters);
                if (parameters[7].Value != null)
                    return Convert.ToInt32(parameters[7].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion CompanyBankDetail


    }
}
