using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.Enquiry
{
    public class EnquiryDAL
    {
        public DataSet GetSeletedCustEnquiryList(int EnqId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSeletedEnquiryList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveAssignedEnqToEmp(int EnqId, int EmpId, string User_Id, int Status, string Type)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[1].Value = EmpId;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                parameters[4] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[4].Value = Status;

                parameters[5] = new SqlParameter("@Type", SqlDbType.VarChar);
                parameters[5].Value = Type;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveCustEnqToEmpployee", parameters);
                if (parameters[3].Value != null)
                    errCode = Convert.ToInt32(parameters[3].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet GetDelectedQuotation(int EnqId, int QuotId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@QuotId", SqlDbType.NVarChar);
                parameters[1].Value = QuotId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedEnqQuotation", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSeletedCustEnquiryItemBrandList(int EnqId, int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetEnquiryItemSelectBrandList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int DeleteEnqItem(int EnqId, int ItemId)
        {
            int ErroCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDeleteEnquiryItem", parameters);
                if (parameters[1].Value != null)
                    ErroCode = Convert.ToInt32(parameters[2].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErroCode;
        }
        public DataSet GetItemResponseList(int EnqId, int ItemId)
        //public DataSet GetCustEnquiryList(string User_Id)
        //{
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetItemResponseList", parameters);
                //SqlParameter[] parameters = new SqlParameter[2];
                //parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                //parameters[0].Value = User_Id;

                //ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCustEnquiryList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SendEnqForQuatation(int EnqId, int StatusId, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;

                parameters[2] = new SqlParameter("@StatusId", SqlDbType.Int);
                parameters[2].Value = StatusId;

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWEnqSendQuatationPrepration", parameters);
                if (parameters[1].Value != null)
                    errCode = Convert.ToInt32(parameters[1].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet GetEnqListForQuotation(string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = UserId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetEnquityForQuotation", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetCustEnquiryList(string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = UserId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCustEnquiryList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetCustEnquiryListforAssign(string UserId, int StatusId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = UserId;

                parameters[1] = new SqlParameter("@StatusId", SqlDbType.NVarChar);
                parameters[1].Value = StatusId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCustEnquiryListForAssign", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveCustomerEnquiry(DataSet dsObj, string User_Id)
        {
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            int Errcode = 0;
            try
            {
                SqlParameter[] Parameter = new SqlParameter[14];
                Parameter[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                Parameter[0].Value = dsObj.Tables[0].Rows[0]["EnqId"];
                Parameter[0].Direction = ParameterDirection.InputOutput;

                Parameter[1] = new SqlParameter("@EnqNumber", SqlDbType.VarChar);
                Parameter[1].Value = dsObj.Tables[0].Rows[0]["EnqNumber"];

                Parameter[2] = new SqlParameter("@CustomerId", SqlDbType.Int);
                Parameter[2].Value = dsObj.Tables[0].Rows[0]["CustomerId"];

                Parameter[3] = new SqlParameter("@Remark", SqlDbType.VarChar);
                Parameter[3].Value = dsObj.Tables[0].Rows[0]["Remark"];

                Parameter[4] = new SqlParameter("@EnqDate", SqlDbType.SmallDateTime);
                Parameter[4].Value = dsObj.Tables[0].Rows[0]["EnqDate"];

                Parameter[5] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Parameter[5].Value = User_Id;

                Parameter[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[6].Value = 0;
                Parameter[6].Direction = ParameterDirection.InputOutput;

                Parameter[7] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                Parameter[7].Value = dsObj.Tables[0].Rows[0]["CompCode"];

                Parameter[8] = new SqlParameter("@Priority", SqlDbType.VarChar);
                Parameter[8].Value = dsObj.Tables[0].Rows[0]["Priority"];

                Parameter[9] = new SqlParameter("@CustomerContactId", SqlDbType.Int);
                Parameter[9].Value = dsObj.Tables[0].Rows[0]["CustomerContactId"];

                Parameter[10] = new SqlParameter("@categories", SqlDbType.VarChar);
                Parameter[10].Value = dsObj.Tables[0].Rows[0]["categories"];

                Parameter[11] = new SqlParameter("@categoriesRefId", SqlDbType.Int);
                Parameter[11].Value = dsObj.Tables[0].Rows[0]["categoriesRefId"];

                Parameter[12] = new SqlParameter("@Rating", SqlDbType.Int);
                Parameter[12].Value = dsObj.Tables[0].Rows[0]["Rating"];

                Parameter[13] = new SqlParameter("@Currency", SqlDbType.VarChar);
                Parameter[13].Value = dsObj.Tables[0].Rows[0]["Currancy"];
                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DwSaveCustEnquiry", Parameter);
                if (Parameter[0].Value != null)
                    Errcode = Convert.ToInt32(Parameter[0].Value.ToString());
                else
                    return 0;

                if (Errcode > 0)
                {
                    //if (!(SaveEnquiryDetail(dsObj, sqlTransaction, User_Id, Errcode) == 600002))
                    //{
                    //    sqlTransaction.Rollback();
                    //    sqlConn.Close();
                    //    return 3;
                    //}
                    sqlTransaction.Commit();
                    sqlConn.Close();
                }
                else
                {
                    sqlTransaction.Commit();
                    sqlConn.Close();
                }

            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                sqlConn.Close();
                Common.LogException(ex);
            }
            return Errcode;
        }
        private int SaveEnquiryDetail(DataSet ds, SqlTransaction tr, string User_Id, int EnqId)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[7];
                    parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                    parameters[0].Value = EnqId;
                    parameters[0].Direction = ParameterDirection.InputOutput;

                    parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                    parameters[1].Value = dr["ItemId"];

                    parameters[2] = new SqlParameter("@Quantity", SqlDbType.Float);
                    parameters[2].Value = dr["Quantity"];

                    parameters[3] = new SqlParameter("@Remark", SqlDbType.VarChar);
                    parameters[3].Value = dr["Remark"];

                    parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[4].Value = User_Id;

                    parameters[5] = new SqlParameter("@ExpectedDate", SqlDbType.DateTime);
                    parameters[5].Value = dr["ExpectedDate"];

                    parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[6].Value = 0;
                    parameters[6].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_DwSaveCustEnquiryDetails", parameters);
                    errCode = (int)parameters[6].Value;
                    if (errCode != 600002)
                    {
                        errCode = 600001;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveCustItemFromExcel(DataTable dsObj, string User_Id)
        {
            int Errcode = 0;
            try
            {
                foreach (DataRow dr in dsObj.Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[7];
                    parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                    parameters[0].Value = dr["EnqId"];

                    parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                    parameters[1].Value = dr["ItemId"];

                    parameters[2] = new SqlParameter("@Quantity", SqlDbType.Float);
                    parameters[2].Value = dr["Quantity"];

                    parameters[3] = new SqlParameter("@Remark", SqlDbType.VarChar);
                    parameters[3].Value = dr["Remark"];

                    parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[4].Value = User_Id;

                    parameters[5] = new SqlParameter("@ExpectedDate", SqlDbType.DateTime);
                    parameters[5].Value = dr["ExpectedDate"];

                    parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[6].Value = 0;
                    parameters[6].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveExcelCustEnquiryDetails", parameters);
                    Errcode = (int)parameters[6].Value;
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Errcode;
        }
        public int SaveCustomerItemBrand(DataSet dsObj, string User_Id, string CPN)
        {
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            int Errcode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[8];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = dsObj.Tables[1].Rows[0]["EnqId"];

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = dsObj.Tables[1].Rows[0]["ItemId"];

                parameters[2] = new SqlParameter("@Quantity", SqlDbType.Float);
                parameters[2].Value = dsObj.Tables[1].Rows[0]["Quantity"];

                parameters[3] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[3].Value = dsObj.Tables[1].Rows[0]["Remark"];

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@ExpectedDate", SqlDbType.DateTime);
                parameters[5].Value = dsObj.Tables[1].Rows[0]["ExpectedDate"];

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                parameters[6].Direction = ParameterDirection.InputOutput;

                parameters[7] = new SqlParameter("@CPN", SqlDbType.VarChar);
                parameters[7].Value = CPN;
                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DwSaveCustEnquiryDetails", parameters);
                Errcode = (int)parameters[6].Value;
                if (Errcode == 600002)
                {
                    SqlParameter[] parameters1 = new SqlParameter[7];
                    parameters1[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                    parameters1[0].Value = dsObj.Tables[1].Rows[0]["EnqId"];

                    parameters1[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                    parameters1[1].Value = dsObj.Tables[1].Rows[0]["ItemId"];

                    parameters1[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters1[3].Value = 0;
                    parameters1[3].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWDeleteCustEnquityItemBrand", parameters1);
                    int Err = (int)parameters1[3].Value;
                    if (Err == 500002)
                    {
                        if (!(SaveEnquiryItemBrandDetail(dsObj, sqlTransaction, User_Id) == 600002))
                        {
                            sqlTransaction.Rollback();
                            sqlConn.Close();
                            return 3;
                        }
                        sqlTransaction.Commit();
                        sqlConn.Close();
                    }
                    else
                    {
                        sqlTransaction.Rollback();
                        sqlConn.Close();
                        return 3;
                    }
                }
                else
                {
                    sqlTransaction.Rollback();
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                sqlConn.Close();
                Common.LogException(ex);
            }
            return Errcode;
        }
        private int SaveEnquiryItemBrandDetail(DataSet ds, SqlTransaction tr, string User_Id)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[4];
                    parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                    parameters[0].Value = dr["EnqId"];

                    parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                    parameters[1].Value = dr["ItemId"];

                    parameters[2] = new SqlParameter("@BrandId", SqlDbType.Float);
                    parameters[2].Value = dr["BrandId"];

                    parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[3].Value = 0;
                    parameters[3].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_DwSaveCustEnquiryItamBrandDetails", parameters);
                    errCode = (int)parameters[3].Value;
                    if (errCode != 600002)
                    {
                        errCode = 600001;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int EnqSendForProcess(int EnqId, int StatusId, string user_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@StatusId", SqlDbType.Int);
                parameters[1].Value = StatusId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                parameters[3] = new SqlParameter("@user_Id", SqlDbType.NVarChar);
                parameters[3].Value = user_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWEnqSendForProcess", parameters);
                errCode = (int)parameters[2].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        public DataSet GetQuotationListByItem(int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[0].Value = ItemId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetQuotationListByItem", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetEnquiryReport(string user_Id)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@user_Id", SqlDbType.VarChar);
                param[0].Value = user_Id;

                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEnquiryReport", param);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }
        public DataSet GetEnquiryQuotItemReport(string user_Id)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@user_Id", SqlDbType.VarChar);
                param[0].Value = user_Id;

                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEnquiryQuotItemDetReport", param);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }
        public int UpdateQuotFileName(int QuotId, string FilePath)
        {
            int errcode = 0;
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@QuotId", SqlDbType.Int);
                param[0].Value = QuotId;

                param[1] = new SqlParameter("@FilePath", SqlDbType.VarChar);
                param[1].Value = FilePath;

                param[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                param[2].Value = 0;
                param[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWUpdateQuotFileName", param);
                errcode = (int)param[2].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errcode;
        }
        #region Customer Process Enquiry
        public DataSet GetSeletedEnquiryProcessList(int EnqId,string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSeletedEnquiryProcessList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetEnquiryProcessBrandVendorList(int EnqId, int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetEnquiryProcessItemBrandList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SendVenderEnquiryDetail(DataSet ds, int EnqId, string ItemId, string User_Id)
        {
            int errCode = 0;
            int Err = 0;
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            try
            {
                //foreach (DataRow drItem in ds.Tables[1].Rows)
                //{
                //    SqlParameter[] parameters1 = new SqlParameter[7];
                //    parameters1[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                //    parameters1[0].Value = EnqId;

                //    parameters1[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                //    parameters1[1].Value = drItem["ItemId"];

                //    parameters1[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                //    parameters1[3].Value = 0;
                //    parameters1[3].Direction = ParameterDirection.InputOutput;
                //    SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWDeleteProcessItemVendor", parameters1);
                //    Err = (int)parameters1[3].Value;
                //}
                //if (Err == 600001)
                //{
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[6];
                    parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                    parameters[0].Value = dr["EnqId"];

                    parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                    parameters[1].Value = dr["ItemId"];

                    parameters[2] = new SqlParameter("@VendorId", SqlDbType.Float);
                    parameters[2].Value = dr["VendorId"];

                    parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[3].Value = 0;
                    parameters[3].Direction = ParameterDirection.InputOutput;

                    parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[4].Value = User_Id;

                    parameters[5] = new SqlParameter("@ContactId", SqlDbType.Int);
                    parameters[5].Value = dr["ContactId"];
                    SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWSaveProcessEnquiryItemVendor", parameters);
                    errCode = (int)parameters[3].Value;
                    if (errCode == 500000)
                    {
                       // sqlTransaction.Rollback();
                       // sqlConn.Close();
                      //  break;
                    }
                }
                //}
                sqlTransaction.Commit();
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                sqlConn.Close();
                Common.LogException(ex);

            }
            return errCode;
        }
        public DataSet GetItemVendorDetailList(int EnqId,string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;
            
                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWItemVendorDetailList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetVendorEmailDeatiltoSendEnq(int EnqId, int ItemId, string DocType, string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;

                parameters[2] = new SqlParameter("@DocType", SqlDbType.VarChar);
                parameters[2].Value = DocType;

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetMailbodyToVendorEnq", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public int UpdateVendorEnqstatus(int EnqId, int ItemId, int VendorId, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;

                parameters[2] = new SqlParameter("@VendorId", SqlDbType.Float);
                parameters[2].Value = VendorId;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_UpdateVendorEnqStatus", parameters);
                errCode = (int)parameters[3].Value;
            }
            catch (Exception)
            {
                throw;
            }
            return errCode;
        }
        public DataSet GetVendorItemEnqforCSV(int EnqId, int VendorId,string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[1].Value = VendorId;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetVendorItemEnqforCSV", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        #endregion Customer Process Enquiry

        #region Enquiry Item Vendor Response Detail
        public DataSet EnquiryItemVendorResponseDetailList(string User_Id)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwEnquiryItemVendorResponseList", ObjParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEnquiryItemVendorResponseDetailList(int ResponseId)
        {
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ResponseId", SqlDbType.Int);
                param[0].Value = ResponseId;
                DataSet ds = new DataSet();
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwGetEnquiryItemVendorResponseList", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCustItemEnqListForCSV(int QuotId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@QuotId", SqlDbType.Int);
                parameters[0].Value = QuotId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCustItemEnqforCSV", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetCustEmailDeatiltoSendQuotation(int QuotId, string DocType, string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@QuotId", SqlDbType.Int);
                parameters[0].Value = QuotId;

                parameters[1] = new SqlParameter("@DocType", SqlDbType.VarChar);
                parameters[1].Value = DocType;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetMailbodyToCustEnq", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet CloseEnquiryMail(int EnqId, string DocType, string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@DocType", SqlDbType.VarChar);
                parameters[1].Value = DocType;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_CloseEnqSendMail", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int UpdateCustEnqStatusAfterMailFail(int EnqId, int Status, string User_Id, string Remark)
        {
            DataSet ds = new DataSet();
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[0].Value = Status;

                parameters[1] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[1].Value = EnqId;

                parameters[2] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[2].Value = Remark;

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWUpdateStatusCloseEnquiry", parameters);
                errCode = (int)parameters[4].Value;


            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return errCode;
        }
        public DataSet GetQuotationListForApproval(string User_Id)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWQuotationListForapproval", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateQuotation(int Terms, int QuotationId, string Currency, string Remark, int Rating,string User_Id)
        {
            int errcode = 0;
            if (Currency != "")
            {
                SqlParameter[] param = new SqlParameter[7];
                param[0] = new SqlParameter("@Terms", SqlDbType.Int);
                param[0].Value = Terms;

                param[1] = new SqlParameter("@QuotationId", SqlDbType.Int);
                param[1].Value = QuotationId;

                param[2] = new SqlParameter("@Currency", SqlDbType.VarChar);
                param[2].Value = Currency;

                param[3] = new SqlParameter("@Remark", SqlDbType.VarChar);
                param[3].Value = Remark;

                param[4] = new SqlParameter("@Rating", SqlDbType.Int);
                param[4].Value = Rating;

                param[5] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                param[5].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWUpdateQuotation", param);
                errcode = 50000;
                DataSet ds = new DataSet();
                SqlParameter[] parameter = new SqlParameter[1];
                parameter[0] = new SqlParameter("@QuotationId", SqlDbType.Int);
                parameter[0].Value = QuotationId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCurrencyConvertingList", parameter);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["TOCcy"].ToString() != "")
                    {


                        SqlParameter[] parameters = new SqlParameter[5];
                        parameters[0] = new SqlParameter("@FromCcy", SqlDbType.VarChar);
                        parameters[0].Value = dr["FromCcy"];

                        parameters[1] = new SqlParameter("@ToCcy", SqlDbType.VarChar);
                        parameters[1].Value = dr["TOCcy"];


                        parameters[2] = new SqlParameter("@Amount", SqlDbType.Float);
                        parameters[2].Value = dr["Rate"];
                        parameters[2].Direction = ParameterDirection.InputOutput;

                        parameters[3] = new SqlParameter("@ResponseDetailId", SqlDbType.Int);
                        parameters[3].Value = dr["ResponseDetailId"];

                        parameters[4] = new SqlParameter("@QuotationId", SqlDbType.Int);
                        parameters[4].Value = dr["QuotationId"];
                        SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWConvertCurrencyRate", parameters);
                    }
                }
            }
            return errcode;
        }
        public Double GetExchageRate(string Toccy, string FromCCy, Double Amount)
        {

            DataSet ds = new DataSet();
            SqlParameter[] param = new SqlParameter[3];
            param[0] = new SqlParameter("@Fromccy", SqlDbType.VarChar);
            param[0].Value = FromCCy;

            param[1] = new SqlParameter("@Toccy", SqlDbType.VarChar);
            param[1].Value = Toccy;

            param[2] = new SqlParameter("@Amount", SqlDbType.Float);
            param[2].Value = Amount;
            param[2].Direction = ParameterDirection.InputOutput;
            SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCurrencyExchangeRate", param);
            Double Amt = (Double)param[2].Value;

            return Amt;
        }
        public int UpdateQuotationForApprovals(int EnqId, int QuotationId, int StatusId, string Remark, string User_Id)
        {
            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@EnqId", SqlDbType.Int);
            param[0].Value = EnqId;

            param[1] = new SqlParameter("@QuotationId", SqlDbType.Int);
            param[1].Value = QuotationId;

            param[2] = new SqlParameter("@StatusId", SqlDbType.Int);
            param[2].Value = StatusId;

            param[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            param[3].Value = 0; ;
            param[3].Direction = ParameterDirection.InputOutput;

            param[4] = new SqlParameter("@Remark", SqlDbType.VarChar);
            param[4].Value = Remark;

            param[5] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            param[5].Value = User_Id;
            SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWUpdateQuotaionApprovals", param);
            int errcode = (int)param[3].Value;

            if (errcode != 0 && StatusId == 45)
            {
                SqlParameter[] parameter = new SqlParameter[14];
                parameter[0] = new SqlParameter("@EnqId", SqlDbType.VarChar);
                parameter[0].Value = EnqId;

                parameter[1] = new SqlParameter("@ToStatus", SqlDbType.Int);
                parameter[1].Value = SmartSys.Utility.Enums.StatusCode.QuotSent;

                parameter[2] = new SqlParameter("@FromStatus", SqlDbType.Int);
                parameter[2].Value = SmartSys.Utility.Enums.StatusCode.APPROVED;


                parameter[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameter[3].Value = User_Id;

                parameter[4] = new SqlParameter("@QuatId", SqlDbType.Int);
                parameter[4].Value = QuotationId;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveQuotaionStatusDetail", parameter);

            }
            else
            {
                SqlParameter[] parameter = new SqlParameter[14];
                parameter[0] = new SqlParameter("@EnqId", SqlDbType.VarChar);
                parameter[0].Value = EnqId;

                if (StatusId == 25)
                {
                    parameter[1] = new SqlParameter("@ToStatus", SqlDbType.Int);
                    parameter[1].Value = SmartSys.Utility.Enums.StatusCode.REJECTED;
                }
                if (StatusId == 42)
                {
                    parameter[1] = new SqlParameter("@ToStatus", SqlDbType.Int);
                    parameter[1].Value = SmartSys.Utility.Enums.StatusCode.SendApp;
                }
                else
                {
                    parameter[1] = new SqlParameter("@ToStatus", SqlDbType.Int);
                    parameter[1].Value = SmartSys.Utility.Enums.StatusCode.APPROVED;
                }
                if (StatusId == 24 || StatusId == 25)
                {
                    parameter[2] = new SqlParameter("@FromStatus", SqlDbType.Int);
                    parameter[2].Value = SmartSys.Utility.Enums.StatusCode.SendApp;
                }
                else
                {
                    parameter[2] = new SqlParameter("@FromStatus", SqlDbType.Int);
                    parameter[2].Value = SmartSys.Utility.Enums.StatusCode.QuotNew;
                }

                parameter[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameter[3].Value = User_Id;

                parameter[4] = new SqlParameter("@QuatId", SqlDbType.Int);
                parameter[4].Value = QuotationId;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveQuotaionStatusDetail", parameter);
            }
            return errcode;
        }
        public int CreateQuotation(int ItemId, int CustomerId, string QuotationNumber, int EnqId, string User_Id)
        {
            int errcode = 0;

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
            param[0].Value = CustomerId;

            param[1] = new SqlParameter("@Status", SqlDbType.Int);
            param[1].Value = SmartSys.Utility.Enums.StatusCode.QuotNew;

            param[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            param[2].Value = 0;
            param[2].Direction = ParameterDirection.InputOutput;

            param[3] = new SqlParameter("@EnqId", SqlDbType.VarChar);
            param[3].Value = EnqId;

            param[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            param[4].Value = User_Id;

            param[5] = new SqlParameter("@QuotationNumber", SqlDbType.VarChar);
            param[5].Value = QuotationNumber;

            SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCreateEnqQuotation", param);
            errcode = (int)param[2].Value;
            if (errcode != 0)
            {
                SqlParameter[] paramQ = new SqlParameter[14];
                paramQ[0] = new SqlParameter("@EnqId", SqlDbType.VarChar);
                paramQ[0].Value = EnqId;

                paramQ[1] = new SqlParameter("@ToStatus", SqlDbType.Int);
                paramQ[1].Value = SmartSys.Utility.Enums.StatusCode.QuotNew;

                paramQ[2] = new SqlParameter("@FromStatus", SqlDbType.Int);
                paramQ[2].Value = SmartSys.Utility.Enums.StatusCode.QuatPrep;

                paramQ[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                paramQ[3].Value = User_Id;

                paramQ[4] = new SqlParameter("@QuatId", SqlDbType.Int);
                paramQ[4].Value = errcode;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveQuotaionStatusDetail", paramQ);
            }
            return errcode;
        }
        public int ReviewedQuotation(int QuotId, int EnqId, string User_Id, string QuotationNumber)
        {
            int errcode = 0;

            SqlParameter[] param = new SqlParameter[6];
            param[0] = new SqlParameter("@QuotId", SqlDbType.Int);
            param[0].Value = QuotId;

            param[1] = new SqlParameter("@Status", SqlDbType.Int);
            param[1].Value = SmartSys.Utility.Enums.StatusCode.QuotNew;

            param[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            param[2].Value = 0;
            param[2].Direction = ParameterDirection.InputOutput;

            param[3] = new SqlParameter("@EnqId", SqlDbType.VarChar);
            param[3].Value = EnqId;

            param[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            param[4].Value = User_Id;

            param[5] = new SqlParameter("@QuotationNumber", SqlDbType.VarChar);
            param[5].Value = QuotationNumber;

            SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCreateEnqReviewedQuotation", param);
            errcode = (int)param[2].Value;
            if (errcode != 0)
            {
                SqlParameter[] paramQ = new SqlParameter[14];
                paramQ[0] = new SqlParameter("@EnqId", SqlDbType.VarChar);
                paramQ[0].Value = EnqId;

                paramQ[1] = new SqlParameter("@ToStatus", SqlDbType.Int);
                paramQ[1].Value = SmartSys.Utility.Enums.StatusCode.QuotNew;

                paramQ[2] = new SqlParameter("@FromStatus", SqlDbType.Int);
                paramQ[2].Value = SmartSys.Utility.Enums.StatusCode.QuatPrep;

                paramQ[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                paramQ[3].Value = User_Id;

                paramQ[4] = new SqlParameter("@QuatId", SqlDbType.Int);
                paramQ[4].Value = errcode;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveQuotaionStatusDetail", paramQ);
            }
            return errcode;
        }
        public int SaveCustQuotItemDetails(DataTable dt)
        {
            SqlParameter[] param = new SqlParameter[13];
            param[0] = new SqlParameter("@QuotationId", SqlDbType.Int);
            param[0].Value = dt.Rows[0]["QuotId"];

            param[1] = new SqlParameter("@ItemId", SqlDbType.Int);
            param[1].Value = dt.Rows[0]["ItemId"];

            param[2] = new SqlParameter("@BrandId", SqlDbType.Int);
            param[2].Value = dt.Rows[0]["BrandId"];

            param[3] = new SqlParameter("@Rate", SqlDbType.Float);
            param[3].Value = dt.Rows[0]["Rate"];

            param[4] = new SqlParameter("@MinimumQty", SqlDbType.Float);
            param[4].Value = dt.Rows[0]["MinimumQty"];

            param[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            param[5].Value = 0;
            param[5].Direction = ParameterDirection.InputOutput;

            param[6] = new SqlParameter("@ResponseDetailId", SqlDbType.Int);
            param[6].Value = dt.Rows[0]["ResponseId"]; ;

            param[7] = new SqlParameter("@BatchNumber", SqlDbType.VarChar);
            param[7].Value = dt.Rows[0]["BatchNumber"];

            SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveCustQuotItemDetails", param);
            int errcode = (int)param[5].Value;
            return errcode;
        }
        public int SaveQuotation(int ItemId, int ResponseId, int BrandId, float Rate, float MinimumQty, int EnqId, string User_Id, string Remark,int QuotId)
        {
            SqlParameter[] param = new SqlParameter[11];
            param[0] = new SqlParameter("@ResponseDetailId", SqlDbType.Int);
            param[0].Value = ResponseId;

            param[1] = new SqlParameter("@ItemId", SqlDbType.Int);
            param[1].Value = ItemId;

            param[2] = new SqlParameter("@BrandId", SqlDbType.Int);
            param[2].Value = BrandId;

            param[3] = new SqlParameter("@Rate", SqlDbType.Float);
            param[3].Value = Rate;

            param[4] = new SqlParameter("@MinimumQty", SqlDbType.Float);
            param[4].Value = MinimumQty;

            param[5] = new SqlParameter("@Status", SqlDbType.Int);
            param[5].Value = SmartSys.Utility.Enums.StatusCode.QuotNew;

            param[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            param[6].Value = 0;
            param[6].Direction = ParameterDirection.InputOutput;

            param[7] = new SqlParameter("@EnqId", SqlDbType.VarChar);
            param[7].Value = EnqId;

            param[8] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            param[8].Value = User_Id;

            param[9] = new SqlParameter("@Remark", SqlDbType.VarChar);
            param[9].Value = Remark;

            param[10] = new SqlParameter("@QuotId", SqlDbType.VarChar);
            param[10].Value = QuotId;

            SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SaveEnquiryQuotation", param);


            int errcode = (int)param[7].Value;

            if (errcode != 0)
            {
                SqlParameter[] parameter = new SqlParameter[5];
                parameter[0] = new SqlParameter("@EnqId", SqlDbType.VarChar);
                parameter[0].Value = EnqId;

                parameter[1] = new SqlParameter("@ToStatus", SqlDbType.Int);
                parameter[1].Value = SmartSys.Utility.Enums.StatusCode.QuotNew;

                parameter[2] = new SqlParameter("@FromStatus", SqlDbType.Int);
                parameter[2].Value = SmartSys.Utility.Enums.StatusCode.QuatPrep;


                parameter[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameter[3].Value = User_Id;

                //parameter[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                //parameter[4].Value = 0;

                parameter[4] = new SqlParameter("@QuatId", SqlDbType.Int);
                parameter[4].Value = QuotId;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveQuotaionStatusDetail", parameter);
            }
            return errcode;
        }
        public DataSet GetVendorResponseByItem(int ItemId, int EnqId, int QuotId)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                param[0].Value = ItemId;


                param[1] = new SqlParameter("@EnqId", SqlDbType.Int);
                param[1].Value = EnqId;

                param[2] = new SqlParameter("@QuotId", SqlDbType.Int);
                param[2].Value = QuotId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorResponseByItem", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCustQuotItemforApproval(int EnqId, int QuotId)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                param[0].Value = EnqId;

                param[1] = new SqlParameter("@QuotId", SqlDbType.Int);
                param[1].Value = QuotId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWQuotItemListForApproval", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int SaveEnquiryItemVendorResponseDetailList(DataSet ds, SqlTransaction tr, string User_Id, int ResponseId, int VendorId)
        {
            int errcode = 600002;
            try
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    if (dr["Chk"].ToString() == "OK")
                    {
                        SqlParameter[] param = new SqlParameter[12];
                        param[0] = new SqlParameter("@ResponseId", SqlDbType.Int);
                        param[0].Value = ResponseId;

                        param[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                        param[1].Value = dr["ItemId"];

                        param[2] = new SqlParameter("@BrandId", SqlDbType.Int);
                        param[2].Value = dr["BrandId"];

                        param[3] = new SqlParameter("@Rate", SqlDbType.Float);
                        param[3].Value = dr["Rate"];

                        param[4] = new SqlParameter("@MinimumQty", SqlDbType.Int);
                        param[4].Value = dr["MinimumQty"];

                        param[5] = new SqlParameter("@BatchSize", SqlDbType.VarChar);
                        param[5].Value = dr["BatchNumber"];

                        param[6] = new SqlParameter("@VendorPromiseDate", SqlDbType.VarChar);
                        param[6].Value = dr["VendorPromiseDate"];

                        param[7] = new SqlParameter("@SPQ", SqlDbType.Int);
                        param[7].Value = dr["SPQ"];

                        param[8] = new SqlParameter("@MOQ", SqlDbType.Int);
                        param[8].Value = dr["MOQ"];

                        param[9] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                        param[9].Value = errcode;
                        param[9].Direction = ParameterDirection.InputOutput;

                        param[10] = new SqlParameter("@VendorId", SqlDbType.Int);
                        param[10].Value = VendorId;

                        param[11] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                        param[11].Value = User_Id;
                        SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_DWSaveEnquiryItemVendorRespose", param);
                        errcode = (int)param[9].Value;
                        if (errcode == 500001 || errcode == 500002)
                        {
                            errcode = 600002;
                        }
                        else
                            return 500000;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }


        public int SaveEnquiryItemVendorResponse(DataSet dsObj, string User_Id)
        {
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            int Errcode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[10];
                parameters[0] = new SqlParameter("@ResponseId", SqlDbType.Int);
                parameters[0].Value = dsObj.Tables[0].Rows[0]["ResponseId"];
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[1].Value = dsObj.Tables[0].Rows[0]["VendorId"];

                parameters[2] = new SqlParameter("@ContactId", SqlDbType.Int);
                parameters[2].Value = dsObj.Tables[0].Rows[0]["ContactId"];

                parameters[3] = new SqlParameter("@ReciptMethod", SqlDbType.VarChar);
                parameters[3].Value = dsObj.Tables[0].Rows[0]["ReciptMethod"];

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@ReciptDate", SqlDbType.DateTime);
                parameters[5].Value = dsObj.Tables[0].Rows[0]["ReciptDate"];

                parameters[6] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[6].Value = dsObj.Tables[0].Rows[0]["Remark"];

                parameters[7] = new SqlParameter("@Currency", SqlDbType.VarChar);
                parameters[7].Value = dsObj.Tables[0].Rows[0]["Currency"];

                parameters[8] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[8].Value = 0;
                parameters[8].Direction = ParameterDirection.InputOutput;

                parameters[9] = new SqlParameter("@Rating", SqlDbType.VarChar);
                parameters[9].Value = dsObj.Tables[0].Rows[0]["Rating"];
                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWSaveItemVendorResponse", parameters);
                Errcode = Convert.ToInt32(parameters[0].Value);
                if (Errcode > 0)
                {
                    sqlTransaction.Commit();
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                sqlConn.Close();
                Common.LogException(ex);
            }
            return Errcode;
        }

        public int SaveEnquiryItemVendorResponseDetails(DataSet dsObj, string User_Id, int ResponseId, int VendorId)
        {
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            int Errcode = 0;
            try
            {
                SqlParameter[] parameters1 = new SqlParameter[3];
                parameters1[0] = new SqlParameter("@ResponseId", SqlDbType.Int);
                parameters1[0].Value = ResponseId;

                parameters1[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters1[1].Value = 0;
                parameters1[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWDeleteitemVendorResponse", parameters1);
                int Err = Convert.ToInt32(parameters1[1].Value);
                if (Err == 500003)
                {
                    if (!(SaveEnquiryItemVendorResponseDetailList(dsObj, sqlTransaction, User_Id, ResponseId, VendorId) == 600002))
                    {
                        sqlTransaction.Rollback();
                        sqlConn.Close();
                        return 3;
                    }
                    sqlTransaction.Commit();
                    sqlConn.Close();
                }
                else
                {
                    sqlTransaction.Rollback();
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                sqlConn.Close();
                Common.LogException(ex);
            }
            return Errcode;
        }
        #endregion

        #region Enquiry Purchase Order
        public DataSet GetSeletedCustPOList(int PurchaseOrderId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@PurchaseOrderId", SqlDbType.Int);
                parameters[0].Value = PurchaseOrderId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSeletedPOList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetCustomerPOList(string User_Id, int Status)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                //parameters[1] = new SqlParameter("@StatusId", SqlDbType.Int);
                //parameters[1].Value = Status;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCustPOList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetCustomerPOApprovalList(string User_Id, int Status)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                parameters[1] = new SqlParameter("@StatusId", SqlDbType.Int);
                parameters[1].Value = Status;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCustPOApprovalList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public DataSet GetSalesOrderList(string CompCode, int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                parameters[1] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[1].Value = CustomerId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSalesOrderbyCustomer", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveCustomerPO(DataSet dsObj, string User_Id)
        {
            int Errcode = 0;
            try
            {
                SqlParameter[] Parameter = new SqlParameter[10];
                Parameter[0] = new SqlParameter("@PurchaseOrderId", SqlDbType.Int);
                Parameter[0].Value = dsObj.Tables[0].Rows[0]["PurchaseOrderId"];
                Parameter[0].Direction = ParameterDirection.InputOutput;

                Parameter[1] = new SqlParameter("@PurchaseOrderNumber", SqlDbType.VarChar);
                Parameter[1].Value = dsObj.Tables[0].Rows[0]["PurchaseOrderNumber"];

                Parameter[2] = new SqlParameter("@CustomerId", SqlDbType.Int);
                Parameter[2].Value = dsObj.Tables[0].Rows[0]["CustomerId"];

                Parameter[3] = new SqlParameter("@Remark", SqlDbType.VarChar);
                Parameter[3].Value = dsObj.Tables[0].Rows[0]["Remark"];

                Parameter[4] = new SqlParameter("@PODate", SqlDbType.SmallDateTime);
                Parameter[4].Value = dsObj.Tables[0].Rows[0]["PODate"];

                Parameter[5] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Parameter[5].Value = User_Id;

                Parameter[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[6].Value = 0;
                Parameter[6].Direction = ParameterDirection.InputOutput;

                Parameter[7] = new SqlParameter("@DocumentFile", SqlDbType.NVarChar);
                Parameter[7].Value = dsObj.Tables[0].Rows[0]["DocumentFile"];

                Parameter[8] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Parameter[8].Value = dsObj.Tables[0].Rows[0]["CompCode"];

                Parameter[9] = new SqlParameter("@Rating", SqlDbType.Int);
                Parameter[9].Value = dsObj.Tables[0].Rows[0]["Rating"];
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveCustPO", Parameter);
                if (Parameter[0].Value != null)
                    Errcode = Convert.ToInt32(Parameter[0].Value.ToString());
                else
                    return 0;

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Errcode;
        }
        public DataSet GetQuotationListfordrpdwn(string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = UserId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetQuotationListForDrpDwn", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        private int SavePODetailItem(DataSet dsObj, SqlTransaction tr, string User_Id, int PurchaseOrderId)
        {
            int Errcode = 0;
            try
            {
                foreach (DataRow dr in dsObj.Tables[1].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[9];
                    parameters[0] = new SqlParameter("@PurchaseOrderId", SqlDbType.Int);
                    parameters[0].Value = dr["PurchaseOrderId"];

                    parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                    parameters[1].Value = dr["ItemId"];

                    parameters[2] = new SqlParameter("@Quantity", SqlDbType.Float);
                    parameters[2].Value = dr["Quantity"];

                    parameters[3] = new SqlParameter("@Remark", SqlDbType.VarChar);
                    parameters[3].Value = dr["Remark"];

                    parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[4].Value = User_Id;

                    parameters[5] = new SqlParameter("@ExpectedDate", SqlDbType.DateTime);
                    parameters[5].Value = dr["ExpectedDate"];

                    parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[6].Value = 0;
                    parameters[6].Direction = ParameterDirection.InputOutput;

                    parameters[7] = new SqlParameter("@Rate", SqlDbType.Float);
                    parameters[7].Value = dr["Rate"];

                    parameters[8] = new SqlParameter("@QuotationId", SqlDbType.Int);
                    parameters[8].Value = dr["QuotationId"];
                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveCustPODetail", parameters);
                    Errcode = (int)parameters[6].Value;
                    if (Errcode != 600002)
                    {
                        Errcode = 600001;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Errcode;
        }
        public int SavePODetail(DataSet dsObj, string User_Id, int PurchaseOrderId)
        {
            int Errcode = 0;
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();            
            try
            {
                foreach (DataRow dr in dsObj.Tables[1].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[10];
                    parameters[0] = new SqlParameter("@PurchaseOrderId", SqlDbType.Int);
                    parameters[0].Value = dr["PurchaseOrderId"];

                    parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                    parameters[1].Value = dr["ItemId"];

                    parameters[2] = new SqlParameter("@Quantity", SqlDbType.Float);
                    parameters[2].Value = dr["Quantity"];

                    parameters[3] = new SqlParameter("@Remark", SqlDbType.VarChar);
                    parameters[3].Value = dr["Remark"];

                    parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[4].Value = User_Id;

                    parameters[5] = new SqlParameter("@ExpectedDate", SqlDbType.DateTime);
                    parameters[5].Value = dr["ExpectedDate"];

                    parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[6].Value = 0;
                    parameters[6].Direction = ParameterDirection.InputOutput;

                    parameters[7] = new SqlParameter("@Rate", SqlDbType.Float);
                    parameters[7].Value = dr["Rate"];

                    parameters[8] = new SqlParameter("@QuotationId", SqlDbType.Int);
                    parameters[8].Value = dr["QuotationId"];

                    parameters[9] = new SqlParameter("@PurchaseDetailOrderId", SqlDbType.Int);
                    parameters[9].Value = dr["PurchaseDetailOrderId"];
                    SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DwSaveCustPODetail", parameters);
                    Errcode = (int)parameters[6].Value;
                }
                if(Errcode == 600002 || Errcode == 500001)
                {
                    sqlTransaction.Commit();
                    sqlConn.Close();
                }
                else
                {
                    sqlTransaction.Rollback();
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                sqlConn.Close();
                Common.LogException(ex);
            }
            return Errcode;
        }
        public int DeletePurchaseDetail(int PurchaseDetailOrderId)
        {
            int Errcode = 0;
            try
            {
                SqlParameter[] Parameter = new SqlParameter[2];
                Parameter[0] = new SqlParameter("@PurchaseDetailOrderId", SqlDbType.Int);
                Parameter[0].Value = PurchaseDetailOrderId;

                Parameter[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[1].Value = 0;
                Parameter[1].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DW_DeletePODetail", Parameter);
                if (Parameter[1].Value != null)
                    Errcode = Convert.ToInt32(Parameter[1].Value.ToString());
                else
                    return 0;

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Errcode;
        }
        public int SendPOForApproval(int PurchaeOrderId, int StatusId, string User_Id)
        {
            int Errcode = 0;
            try
            {
                SqlParameter[] Parameter = new SqlParameter[4];
                Parameter[0] = new SqlParameter("@PurchaeOrderId", SqlDbType.Int);
                Parameter[0].Value = PurchaeOrderId;

                Parameter[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[1].Value = 0;
                Parameter[1].Direction = ParameterDirection.InputOutput;

                Parameter[2] = new SqlParameter("@Status", SqlDbType.Int);
                Parameter[2].Value = StatusId;

                Parameter[3] = new SqlParameter("@user_Id", SqlDbType.NVarChar);
                Parameter[3].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWUpdateCustPOForApproval", Parameter);
                if (Parameter[1].Value != null)
                    Errcode = Convert.ToInt32(Parameter[1].Value.ToString());
                else
                    return 0;

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Errcode;
        }
        public int POFApprovalReject(int PurchaeOrderId, int StatusId, string User_Id)
        {
            int Errcode = 0;
            try
            {
                SqlParameter[] Parameter = new SqlParameter[4];
                Parameter[0] = new SqlParameter("@PurchaeOrderId", SqlDbType.Int);
                Parameter[0].Value = PurchaeOrderId;

                Parameter[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[1].Value = 0;
                Parameter[1].Direction = ParameterDirection.InputOutput;

                Parameter[2] = new SqlParameter("@Status", SqlDbType.Int);
                Parameter[2].Value = StatusId;

                Parameter[3] = new SqlParameter("@user_Id", SqlDbType.NVarChar);
                Parameter[3].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustPOApproveReject", Parameter);
                if (Parameter[1].Value != null)
                    Errcode = Convert.ToInt32(Parameter[1].Value.ToString());
                else
                    return 0;

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Errcode;
        }

        public int SaveSONumber(int PurchaeOrderId, string SONumber, string User_Id)
        {
            int Errcode = 0;
            try
            {
                SqlParameter[] Parameter = new SqlParameter[4];
                Parameter[0] = new SqlParameter("@PurchaeOrderId", SqlDbType.Int);
                Parameter[0].Value = PurchaeOrderId;

                Parameter[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[1].Value = 0;
                Parameter[1].Direction = ParameterDirection.InputOutput;

                Parameter[2] = new SqlParameter("@SONumber", SqlDbType.VarChar);
                Parameter[2].Value = SONumber;

                Parameter[3] = new SqlParameter("@user_Id", SqlDbType.NVarChar);
                Parameter[3].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveSalesOrder", Parameter);
                if (Parameter[1].Value != null)
                    Errcode = Convert.ToInt32(Parameter[1].Value.ToString());
                else
                    return 0;

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Errcode;
        }

        public int EditSODetail(int PurchaseOrderId, string OldSOId, string SOId, string User_Id, string PurchaseOrderNumber,DateTime PODate, string Remark)
        {
            int Errcode = 0;
            try
            {
                SqlParameter[] Parameter = new SqlParameter[8];
                Parameter[0] = new SqlParameter("@PurchaseOrderId", SqlDbType.Int);
                Parameter[0].Value = PurchaseOrderId;

                Parameter[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[1].Value = 0;
                Parameter[1].Direction = ParameterDirection.InputOutput;

                Parameter[2] = new SqlParameter("@OldSOId", SqlDbType.VarChar);
                Parameter[2].Value = OldSOId;

                Parameter[3] = new SqlParameter("@user_Id", SqlDbType.NVarChar);
                Parameter[3].Value = User_Id;

                Parameter[4] = new SqlParameter("@SOId", SqlDbType.VarChar);
                Parameter[4].Value = SOId;

                Parameter[5] = new SqlParameter("@PurchaseOrderNumber", SqlDbType.VarChar);
                Parameter[5].Value = PurchaseOrderNumber;

                Parameter[6] = new SqlParameter("@PODate", SqlDbType.DateTime);
                Parameter[6].Value = PODate;

                Parameter[7] = new SqlParameter("@Remark", SqlDbType.VarChar);
                Parameter[7].Value = Remark;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwUpdateSOOrderNo", Parameter);
                if (Parameter[1].Value != null)
                    Errcode = Convert.ToInt32(Parameter[1].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Errcode;
        }
        #endregion Enquiry Purchase Order 

        #region Purchase Receipt
        public DataSet GetSelectedPurchaseReceiptList(int ReceiptId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                parameters[0].Value = ReceiptId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedPurchaseReceiptList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedFFNotification(string AirwaybillNumber,int ReceiptId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@AirwaybillNumber", SqlDbType.VarChar);
                parameters[0].Value = AirwaybillNumber;

                parameters[1] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                parameters[1].Value = ReceiptId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedNotificationList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveFFNotificationDetail(string AirwaybillNo, int NotificationId, string RefNo, DateTime NotificationDate, string PermitNo, double GSTAmount, string Currency, string User_ID,int Receiptid)
        {
            int ErrCod = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[10];
                objParam[0] = new SqlParameter("@AirwaybillNo", SqlDbType.VarChar);
                objParam[0].Value = AirwaybillNo;

                objParam[1] = new SqlParameter("@NotificationId", SqlDbType.Int);
                objParam[1].Value = NotificationId;

                objParam[2] = new SqlParameter("@Currency", SqlDbType.VarChar);
                objParam[2].Value = @Currency;

                objParam[3] = new SqlParameter("@GSTAmount", SqlDbType.Float);
                objParam[3].Value = GSTAmount;

                objParam[4] = new SqlParameter("@NotificationDate", SqlDbType.DateTime);
                objParam[4].Value = NotificationDate;

                objParam[5] = new SqlParameter("@RefNo", SqlDbType.VarChar);
                objParam[5].Value = RefNo;

                objParam[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[6].Value = 0;
                objParam[6].Direction = ParameterDirection.InputOutput;

                objParam[7] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                objParam[7].Value = User_ID;

                objParam[8] = new SqlParameter("@PermitNo", SqlDbType.VarChar);
                objParam[8].Value = PermitNo;

                objParam[9] = new SqlParameter("@Receiptid", SqlDbType.Int);
                objParam[9].Value = Receiptid;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveFFNotificationDetail", objParam);
                if (objParam[6].Value != null)
                    ErrCod = Convert.ToInt32(objParam[6].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCod;
        }
        public int SaveOtherChargesPurReceipt(int ReceiptId, string ChargesDescription, string Currency, float Amount)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[5];
                objParam[0] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                objParam[0].Value = ReceiptId;

                objParam[1] = new SqlParameter("@ChargesDescription", SqlDbType.VarChar);
                objParam[1].Value = ChargesDescription;

                objParam[2] = new SqlParameter("@Currency", SqlDbType.VarChar);
                objParam[2].Value = Currency;

                objParam[3] = new SqlParameter("@Amount", SqlDbType.VarChar);
                objParam[3].Value = Amount;

                objParam[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[4].Value = 0;
                objParam[4].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveOtherChargesPurReceipt", objParam);
                if (objParam[4].Value != null)
                    return Convert.ToInt32(objParam[4].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }
        public DataSet GetOtherChargesList(int ReceiptId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                parameters[0].Value = ReceiptId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwGetOtherChargesPurReceipt", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetPurchaseReceiptList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id",SqlDbType.NVarChar);
                ObjParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetPurchaseReceiptList", ObjParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SendPurchaseReceipt(DataSet ds, string User_Id, int ReceiptId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[17];
                parameters[0] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["ReceiptId"];
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["CompCode"];

                parameters[2] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[2].Value = ds.Tables[0].Rows[0]["VendorId"];

                parameters[3] = new SqlParameter("@ReceiptDate", SqlDbType.SmallDateTime);
                parameters[3].Value = ds.Tables[0].Rows[0]["ReceiptDate"];

                parameters[4] = new SqlParameter("@FreightForwarderId", SqlDbType.Int);
                parameters[4].Value = ds.Tables[0].Rows[0]["FreightForwarderId"];

                parameters[5] = new SqlParameter("@FFRef_No", SqlDbType.VarChar);
                parameters[5].Value = ds.Tables[0].Rows[0]["FFRef_No"];

                parameters[6] = new SqlParameter("@Currency", SqlDbType.VarChar);
                parameters[6].Value = ds.Tables[0].Rows[0]["Currency"];

                parameters[7] = new SqlParameter("@VendorInvNo", SqlDbType.VarChar);
                parameters[7].Value = ds.Tables[0].Rows[0]["VendorInvNo"];

                //parameters[8] = new SqlParameter("@freightCharges", SqlDbType.Float);
                //parameters[8].Value = ds.Tables[0].Rows[0]["freightCharges"];

                parameters[9] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[9].Value = User_Id;

                parameters[10] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[10].Value = 0;
                parameters[10].Direction = ParameterDirection.InputOutput;

                parameters[11] = new SqlParameter("@VendorInvDate", SqlDbType.SmallDateTime);
                parameters[11].Value = ds.Tables[0].Rows[0]["VendorInvDate"];

                parameters[12] = new SqlParameter("@PermitType", SqlDbType.VarChar);
                parameters[12].Value = ds.Tables[0].Rows[0]["PermitType"];

                parameters[13] = new SqlParameter("@ReceiptFile", SqlDbType.VarChar);
                parameters[13].Value = ds.Tables[0].Rows[0]["ReceiptFile"];

                parameters[14] = new SqlParameter("@Rating", SqlDbType.Int);
                parameters[14].Value = ds.Tables[0].Rows[0]["Rating"];

                parameters[15] = new SqlParameter("@MRIId", SqlDbType.Int);
                parameters[15].Value = ds.Tables[0].Rows[0]["MRIId"];

                parameters[16] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[16].Value = ds.Tables[0].Rows[0]["Remark"];

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSavePurchaseReceipt", parameters);
                errCode = (int)parameters[0].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SendPurchaseReceiptDetail(DataSet ds, string User_Id, int ReceiptId, int ReceiptDetailId)
        {
            int errCode = 0;
            int Err = 0;
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            bool Allow = true;
            sqlTransaction = sqlConn.BeginTransaction();
            try
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[10];
                    parameters[0] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                    parameters[0].Value = ReceiptId;

                    parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                    parameters[1].Value = dr["ItemId"];

                    parameters[2] = new SqlParameter("@Quantity", SqlDbType.Float);
                    parameters[2].Value = dr["Quantity"];

                    parameters[3] = new SqlParameter("@PO_No", SqlDbType.VarChar);
                    parameters[3].Value = dr["PO_No"];

                    parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[4].Value = User_Id;

                    parameters[5] = new SqlParameter("@COO", SqlDbType.Int);
                    parameters[5].Value = dr["COO"];

                    parameters[6] = new SqlParameter("@CartonId", SqlDbType.Int);
                    parameters[6].Value = dr["CartonId"];

                    parameters[7] = new SqlParameter("@InvoiceAmount", SqlDbType.Float);
                    parameters[7].Value = dr["InvoiceAmount"];

                    parameters[8] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[8].Value = 0;
                    parameters[8].Direction = ParameterDirection.InputOutput;

                    parameters[9] = new SqlParameter("@ReceiptDetailId", SqlDbType.Int);
                    parameters[9].Value = ReceiptDetailId;
                    parameters[9].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DwSavePurchaseReceiptDetail", parameters);
                    errCode = (int)parameters[9].Value;
                }

                if (errCode > 0)
                {
                    SqlParameter[] parameterss = new SqlParameter[2];
                    parameterss[0] = new SqlParameter("@ReceiptDetailId", SqlDbType.Float);
                    parameterss[0].Value = ReceiptDetailId;

                    parameterss[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameterss[1].Value = 0;
                    parameterss[1].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWDeletePurchaseReceiptDetail", parameterss);
                    Err = (int)parameterss[1].Value;

                    if (Err == 500002)
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {

                            if (SavePurchaseReceiptTaxDetail(ds, sqlTransaction, User_Id, errCode) != 600002)
                            {
                                Allow = false;
                                sqlTransaction.Rollback();
                                sqlConn.Close();
                            }

                        }
                        else
                        {
                            sqlTransaction.Commit();
                            sqlConn.Close();
                            Allow = false;
                        }
                        if (Allow)
                        {
                            sqlTransaction.Commit();
                            sqlConn.Close();
                        }
                    }
                    else
                    {
                        sqlTransaction.Rollback();
                        sqlConn.Close();
                    }

                }
                else
                {
                    sqlTransaction.Rollback();
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                sqlConn.Close();
                Common.LogException(ex);
            }
            return errCode;
        }

        public int DeleteReceiptDetail(int ReceiptDetailId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameterss = new SqlParameter[2];
                parameterss[0] = new SqlParameter("@ReceiptDetailId", SqlDbType.Int);
                parameterss[0].Value = ReceiptDetailId;

                parameterss[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameterss[1].Value = 0;
                parameterss[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDeleteReceiptDetail", parameterss);
                errCode = (int)parameterss[1].Value;
            }
            catch (Exception)
            {

                throw;
            }
            return errCode;
        }
        public int DeleteReceiptItemLocation(int ReceiptItemDetailId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameterss = new SqlParameter[2];
                parameterss[0] = new SqlParameter("@ReceiptItemDetailId", SqlDbType.Int);
                parameterss[0].Value = ReceiptItemDetailId;

                parameterss[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameterss[1].Value = 0;
                parameterss[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDeleteReceiptItemLocation", parameterss);
                errCode = (int)parameterss[1].Value;
            }
            catch (Exception)
            {

                throw;
            }
            return errCode;
        }
        public DataSet GetReceiptItemDetail(int ReceiptDetailId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameterss = new SqlParameter[1];
                parameterss[0] = new SqlParameter("@ReceiptDetailId", SqlDbType.Int);
                parameterss[0].Value = ReceiptDetailId;

                ds=SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwGetReceiptItemLocationDetail", parameterss);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet CheckReceiptItemLocationDup(int ReceiptDetailId,int ItemId,int LocationId,int COOId,string DateCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameterss = new SqlParameter[5];
                parameterss[0] = new SqlParameter("@ItemDetId", SqlDbType.Int);
                parameterss[0].Value = ReceiptDetailId;

                parameterss[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameterss[1].Value = ItemId;

                parameterss[2] = new SqlParameter("@LocationId", SqlDbType.Int);
                parameterss[2].Value = LocationId;

                parameterss[3] = new SqlParameter("@COOId", SqlDbType.Int);
                parameterss[3].Value = COOId;

                parameterss[4] = new SqlParameter("@DateCode", SqlDbType.VarChar);
                parameterss[4].Value = DateCode;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCheckReceiptItemLocationDup", parameterss);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public int SaveItemLocationDetails(int ItemDetId, int ItemId, int LocationId, int COOId, int Qty, string DateCode,string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[8];
                objParam[0] = new SqlParameter("@ItemDetId", SqlDbType.Int);
                objParam[0].Value = ItemDetId;

                objParam[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                objParam[1].Value = ItemId;

                objParam[2] = new SqlParameter("@LocationId", SqlDbType.Int);
                objParam[2].Value = LocationId;

                objParam[3] = new SqlParameter("@COOId", SqlDbType.Int);
                objParam[3].Value = COOId;

                objParam[4] = new SqlParameter("@Qty", SqlDbType.Int);
                objParam[4].Value = Qty;

                objParam[5] = new SqlParameter("@DateCode", SqlDbType.VarChar);
                objParam[5].Value = DateCode;

                objParam[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[6].Value = 0;
                objParam[6].Direction = ParameterDirection.InputOutput;

                objParam[7] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[7].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveReceiptItemLocationDetail", objParam);
                if (objParam[6].Value != null)
                    return Convert.ToInt32(objParam[6].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }

        //private int SavePurchaseReceiptDetail(DataSet ds, SqlTransaction tr, string User_Id, int ReceiptId)
        //{
        //    int errCode = 0;
        //    try
        //    {
        //        foreach (DataRow dr in ds.Tables[1].Rows)
        //        {
        //            SqlParameter[] parameters = new SqlParameter[9];
        //            parameters[0] = new SqlParameter("@ReceiptId", SqlDbType.Int);
        //            parameters[0].Value = ReceiptId;                   

        //            parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
        //            parameters[1].Value = dr["ItemId"];

        //            parameters[2] = new SqlParameter("@Quantity", SqlDbType.Float);
        //            parameters[2].Value = dr["Quantity"];

        //            parameters[3] = new SqlParameter("@PO_No", SqlDbType.VarChar);
        //            parameters[3].Value = dr["PO_No"];

        //            parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
        //            parameters[4].Value = User_Id;

        //            parameters[5] = new SqlParameter("@COO", SqlDbType.Int);
        //            parameters[5].Value = dr["COO"];

        //            parameters[6] = new SqlParameter("@CartonId", SqlDbType.Int);
        //            parameters[6].Value = dr["CartonId"];

        //            parameters[7] = new SqlParameter("@InvoiceAmount", SqlDbType.Float);
        //            parameters[7].Value = dr["InvoiceAmount"];

        //            parameters[8] = new SqlParameter("@ErrorCode", SqlDbType.Int);
        //            parameters[8].Value = 0;
        //            parameters[8].Direction = ParameterDirection.InputOutput;
        //            SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_DwSavePurchaseReceiptDetail", parameters);
        //            errCode = (int)parameters[8].Value;
        //            if (errCode != 600002)
        //            {
        //                errCode = 600001;
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.LogException(ex);
        //    }
        //    return errCode;
        //}
        private int SavePurchaseReceiptTaxDetail(DataSet ds, SqlTransaction tr, string User_Id, int ReceiptDetailId)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[7];
                    parameters[0] = new SqlParameter("@ReceiptDetailId", SqlDbType.Int);
                    parameters[0].Value = ReceiptDetailId;

                    parameters[1] = new SqlParameter("@TaxId", SqlDbType.Int);
                    parameters[1].Value = dr["TaxId"];

                    parameters[2] = new SqlParameter("@TaxRate", SqlDbType.Float);
                    parameters[2].Value = dr["TaxRate"];

                    parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[3].Value = User_Id;

                    parameters[4] = new SqlParameter("@TaxAmount", SqlDbType.Float);
                    parameters[4].Value = dr["TaxAmount"];

                    parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[5].Value = 0;
                    parameters[5].Direction = ParameterDirection.InputOutput;

                    parameters[6] = new SqlParameter("@PaidByVendor", SqlDbType.Bit);
                    parameters[6].Value = dr["PaidByVendor"];
                    SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_DwSavePurchaseReceiptTaxDetail", parameters);
                    errCode = (int)parameters[5].Value;
                    if (errCode != 600002)
                    {
                        errCode = 600001;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet GetVendorListByCompCode(string CompCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorListByCompCode", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetVendorListByCompCodeFromMRI(string CompCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorListByCompCodeFromMRI", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetPOforReceiptByItem(string CompCode, int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWPOListForReceiptByItem", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetIntimationListByVendor(string CompCode, int VendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                parameters[1] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[1].Value = VendorId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetIntimationByVendorFromMRI", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetItemsByReceiptIdFromMRI(int ReceiptId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                parameters[0].Value = ReceiptId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetMRIItembyReceipId", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public DataSet GetPObyItemFromMRI(int MRIID, int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@MRIID", SqlDbType.VarChar);
                parameters[0].Value = MRIID;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetPObyItemFromMRI", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetItemListByReceipt(string CompCode, int VendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                parameters[1] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[1].Value = VendorId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWItemListForReceipt", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetReceiptItemTaxDetail(int ReceiptId, int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                parameters[0].Value = ReceiptId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetReceiptItemTax", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetTaxOnTaxList(int ChildTaxId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ChildTaxId", SqlDbType.Int);
                parameters[0].Value = ChildTaxId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetTaxOnTaxList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetPurchaseInvForReceiptByVendor(string CompCode, int VendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                parameters[1] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[1].Value = VendorId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWPurchInvListForReceiptByVendor", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveProcessPurchaseReceipt(int StatusId, int ReceiptId, string PurcInv, string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[5];
                objParam[0] = new SqlParameter("@StatusId", SqlDbType.Int);
                objParam[0].Value = StatusId;

                objParam[1] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                objParam[1].Value = ReceiptId;

                objParam[2] = new SqlParameter("@PurcInv", SqlDbType.VarChar);
                objParam[2].Value = PurcInv;

                objParam[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[3].Value = 0;
                objParam[3].Direction = ParameterDirection.InputOutput;

                objParam[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[4].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWProcessPurchaseReceiptInvoice", objParam);
                if (objParam[3].Value != null)
                    return Convert.ToInt32(objParam[3].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }
        //public  int  SavereciptCarton(int Recipt, double Height, double width, double lenght, double Weight, string Remark)
        //{
        //    int ErrCode = 0;
        //    try
        //    {
        //        SqlParameter[] parameters = new SqlParameter[7];
        //        parameters[0] = new SqlParameter("@ReceiptId", SqlDbType.Int);
        //        parameters[0].Value = Recipt;

        //        parameters[1] = new SqlParameter("@Height", SqlDbType.Float);
        //        parameters[1].Value = Height;

        //        parameters[2] = new SqlParameter("@width", SqlDbType.Float);
        //        parameters[2].Value = width;

        //        parameters[3] = new SqlParameter("@lenght", SqlDbType.Float);
        //        parameters[3].Value = lenght;

        //        parameters[4] = new SqlParameter("@Weight", SqlDbType.Float);
        //        parameters[4].Value = Weight;

        //        parameters[5] = new SqlParameter("@Remark", SqlDbType.VarChar);
        //        parameters[5].Value = Remark;

        //        parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
        //        parameters[6].Value = 0;
        //        parameters[6].Direction = ParameterDirection.InputOutput;

        //        SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSavePurchaseReceiptCarton", parameters);
        //        ErrCode = (int)parameters[6].Value;
        //    }
        //    catch (Exception EX)
        //    {                
        //        Common.LogException(EX);
        //    }
        //    return ErrCode;
        //}
        public int UpdateEditCarton(int CartId, double Weight, double Length, double Width, double Height, string Remark)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@Height", SqlDbType.Float);
                parameters[0].Value = Height;

                parameters[1] = new SqlParameter("@width", SqlDbType.Float);
                parameters[1].Value = Width;

                parameters[2] = new SqlParameter("@lenght", SqlDbType.Float);
                parameters[2].Value = Length;

                parameters[3] = new SqlParameter("@Weight", SqlDbType.Float);
                parameters[3].Value = Weight;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                parameters[5] = new SqlParameter("@CartId", SqlDbType.Int);
                parameters[5].Value = CartId;

                parameters[6] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[6].Value = Remark;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwUpdatePurchaseReceiptCarton", parameters);
                errCode = (int)parameters[4].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet GetSelectedCartonList(int ReceiptId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                parameters[0].Value = ReceiptId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCartonListByReceiptId", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int DeleteReceiptTax(int ReceiptDetailId, int TaxId)
        {
           int errCode=0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ReceiptDetailId", SqlDbType.Int);
                parameters[0].Value = ReceiptDetailId;

                parameters[1] = new SqlParameter("@TaxId", SqlDbType.Int);
                parameters[1].Value = TaxId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwDeletePurchaseReceiptTax", parameters);
                errCode = (int)parameters[2].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion Purchase Receipt

        #region Review Purchase Receipt
        public DataSet GetReviewPurchaseReceiptList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetReviewPurchaseReceiptList",ObjParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int ReviewRejectPurchaseReceipt(int StatusId, int ReceiptId, string User_Id,string ReviewedApprovedRejectRemark)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@StatusId", SqlDbType.Int);
                parameters[0].Value = StatusId;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;

                parameters[2] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                parameters[2].Value = ReceiptId;

                parameters[3] = new SqlParameter("@ReviewedApprovedRejectRemark", SqlDbType.VarChar);
                parameters[3].Value = ReviewedApprovedRejectRemark;
                
                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveReviewRejectReceipt", parameters);
                ErrCode = (int)parameters[4].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public DataSet GetProcessedPurchaseReceiptList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetPurchaseReceiptListforProcess", ObjParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetApprovePurchaseReceiptList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter [] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetApprovePurchaseReceiptList", ObjParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        #endregion Review Purchase Receipt

        #region Dispatch
        public DataSet GetDispatchList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetDispatchList", ObjParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public DataSet GetSelectedDispatch(int DispatchId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@DispatchId", SqlDbType.Int);
                parameters[0].Value = DispatchId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedDispatchList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet CustomerListByCompCode(string CompCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerListByCompCode", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet InvoiceListByCustomerId(string CompCode, int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                parameters[1] = new SqlParameter("@CustomerId", SqlDbType.VarChar);
                parameters[1].Value = CustomerId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWInvoiceListByCustomer", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetItemListBySalInvoiceId(string CompCode, string Invoice_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                parameters[1] = new SqlParameter("@Invoice_Id", SqlDbType.VarChar);
                parameters[1].Value = Invoice_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_ItemListBySalInvoice", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetItemRateByInvoiceId(string CompCode, string Invoice_Id, int ItemId, int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                parameters[1] = new SqlParameter("@Invoice_Id", SqlDbType.VarChar);
                parameters[1].Value = Invoice_Id;

                parameters[2] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[2].Value = ItemId;

                parameters[3] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[3].Value = CustomerId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWItemRateByInVoice", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int UpdateDispatchApproval(int Status, string User_Id, int DispatchId)
        {
            int errCode = 0;

            try
            {

                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@DispatchId", SqlDbType.Int);
                parameters[0].Value = DispatchId;


                parameters[1] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[1].Value = Status;


                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;


                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWUpdateDispatchApproval", parameters);
                errCode = (int)parameters[0].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);

            }
            return errCode;

        }
        public int SaveDispatch(DataSet ds, string User_Id, int DispatchId)
        {
            int errCode = 0;
            try
            {

                SqlParameter[] parameters = new SqlParameter[12];
                parameters[0] = new SqlParameter("@DispatchId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["DispatchId"];
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["CompCode"];

                parameters[2] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[2].Value = ds.Tables[0].Rows[0]["CustomerId"];

                //parameters[3] = new SqlParameter("@COO", SqlDbType.Int);
                //parameters[3].Value = ds.Tables[0].Rows[0]["COO"];

                parameters[4] = new SqlParameter("@FreightForwarderId", SqlDbType.Int);
                parameters[4].Value = ds.Tables[0].Rows[0]["FreightForwarder"];

                parameters[5] = new SqlParameter("@DispatchDate", SqlDbType.SmallDateTime);
                parameters[5].Value = ds.Tables[0].Rows[0]["DispatchDate"];

                parameters[6] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[6].Value = User_Id;

                parameters[7] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[7].Value = 0;
                parameters[7].Direction = ParameterDirection.InputOutput;

                parameters[8] = new SqlParameter("@STLocationId", SqlDbType.Int);
                parameters[8].Value = 1;

                parameters[9] = new SqlParameter("@Invoice_No", SqlDbType.VarChar);
                parameters[9].Value = ds.Tables[0].Rows[0]["Invoice_No"];

                parameters[10] = new SqlParameter("@ExportPermitNo", SqlDbType.VarChar);
                parameters[10].Value = ds.Tables[0].Rows[0]["ExportPermitNo"];

                parameters[11] = new SqlParameter("@AirwayBillNo", SqlDbType.VarChar);
                parameters[11].Value = ds.Tables[0].Rows[0]["AirwayBillNo"];

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveDispatch", parameters);
                errCode = (int)parameters[0].Value;

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int DispatchSendForProcess(int DispatchId, int StatusId, string user_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@DispatchId", SqlDbType.Int);
                parameters[0].Value = DispatchId;

                parameters[1] = new SqlParameter("@StatusId", SqlDbType.Int);
                parameters[1].Value = StatusId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                parameters[3] = new SqlParameter("@user_Id", SqlDbType.NVarChar);
                parameters[3].Value = user_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDispatchSendForProcess", parameters);
                errCode = (int)parameters[2].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveDispatchDetail(DataSet ds, string User_Id, int DispatchId)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[9];
                    parameters[0] = new SqlParameter("@DispatchId", SqlDbType.Int);
                    parameters[0].Value = DispatchId;

                    parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                    parameters[1].Value = dr["ItemId"];

                    parameters[2] = new SqlParameter("@Quantity", SqlDbType.Float);
                    parameters[2].Value = dr["Quantity"];

                    parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[3].Value = User_Id;

                    parameters[4] = new SqlParameter("@Rate", SqlDbType.Float);
                    parameters[4].Value = dr["Rate"];

                    parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[5].Value = 0;
                    parameters[5].Direction = ParameterDirection.InputOutput;

                    parameters[6] = new SqlParameter("@CartonId", SqlDbType.Int);
                    parameters[6].Value = dr["CartonId"];

                    parameters[7] = new SqlParameter("@DispatchDetailId", SqlDbType.Int);
                    parameters[7].Value = dr["DispatchDetailId"];
                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveDispatchDetail", parameters);
                    errCode = (int)parameters[5].Value;
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet CustomerListByDispatchCompCode(string CompCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCustomerByDispatchCompCode", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public DataSet InvoiceListByDispatchCustomerId(string CompCode, int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                parameters[1] = new SqlParameter("@CustomerId", SqlDbType.VarChar);
                parameters[1].Value = CustomerId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetInvoiceByDispatchCustomer", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int DeleteDispatchDetail(int DispatchDetailId)
        {
            int Errcode = 0;
            try
            {
                SqlParameter[] Parameter = new SqlParameter[2];
                Parameter[0] = new SqlParameter("@DispatchDetailId", SqlDbType.Int);
                Parameter[0].Value = DispatchDetailId;

                Parameter[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[1].Value = 0;
                Parameter[1].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDeleteDispatchDetail", Parameter);
                if (Parameter[1].Value != null)
                    Errcode = Convert.ToInt32(Parameter[1].Value.ToString());
                else
                    return 0;

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Errcode;
        }
        public int DeleteDispatch(int DispatchId)
        {
            int Errcode = 0;
            try
            {
                SqlParameter[] Parameter = new SqlParameter[2];
                Parameter[0] = new SqlParameter("@DispatchId", SqlDbType.Int);
                Parameter[0].Value = DispatchId;

                Parameter[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[1].Value = 0;
                Parameter[1].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDeleteDispatch", Parameter);
                if (Parameter[1].Value != null)
                    Errcode = Convert.ToInt32(Parameter[1].Value.ToString());
                else
                    return 0;

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Errcode;
        }
        #region Dispatch Carton
        public DataSet GetSelectedDispatchCartonList(int DispatchId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@DispatchId", SqlDbType.Int);
                parameters[0].Value = DispatchId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCartonListByDispatchId", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveReceiptCarton(int ReceiptId, int CartonId, double Weight, string Remark)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                parameters[0].Value = ReceiptId;

                parameters[1] = new SqlParameter("@CartonId", SqlDbType.Float);
                parameters[1].Value = CartonId;

                parameters[2] = new SqlParameter("@Weight", SqlDbType.Float);
                parameters[2].Value = Weight;

                parameters[3] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[3].Value = Remark;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSavePurchaseReceiptCarton", parameters);
                ErrCode = (int)parameters[4].Value;
            }
            catch (Exception EX)
            {
                Common.LogException(EX);
            }
            return ErrCode;
        }
        public int SaveDispatchCarton(int DispatchId, int CartonId, double Weight, string Remark)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@DispatchId", SqlDbType.Int);
                parameters[0].Value = DispatchId;

                parameters[1] = new SqlParameter("@CartonId", SqlDbType.Float);
                parameters[1].Value = CartonId;

                parameters[2] = new SqlParameter("@Weight", SqlDbType.Float);
                parameters[2].Value = Weight;

                parameters[3] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[3].Value = Remark;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveDispatchCarton", parameters);
                ErrCode = (int)parameters[4].Value;
            }
            catch (Exception EX)
            {
                Common.LogException(EX);
            }
            return ErrCode;
        }
        public int DeleteDispatchCart(int DispatchId, int CartonId)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@DispatchId", SqlDbType.Int);
                parameters[0].Value = DispatchId;

                parameters[1] = new SqlParameter("@CartonId", SqlDbType.Float);
                parameters[1].Value = CartonId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDeleteDispatchCarton", parameters);
                ErrCode = (int)parameters[2].Value;
            }
            catch (Exception EX)
            {
                Common.LogException(EX);
            }
            return ErrCode;
        }

        #endregion Dispatch Carton

        #region Dispatch Doc
        public int SaveDispatchDoc(int DispatchId, string DocName, string FilePath, string User_Id)
        {
            int Errcode = 0;
            try
            {
                SqlParameter[] Parameter = new SqlParameter[5];
                Parameter[0] = new SqlParameter("@DispatchId", SqlDbType.Int);
                Parameter[0].Value = DispatchId;

                Parameter[1] = new SqlParameter("@DocTitle", SqlDbType.VarChar);
                Parameter[1].Value = DocName;

                Parameter[2] = new SqlParameter("@FilePath", SqlDbType.VarChar);
                Parameter[2].Value = FilePath;

                Parameter[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[3].Value = 0;
                Parameter[3].Direction = ParameterDirection.InputOutput;

                Parameter[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Parameter[4].Value = User_Id;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveDispatchDocument", Parameter);
                if (Parameter[3].Value != null)
                    Errcode = Convert.ToInt32(Parameter[3].Value.ToString());
                else
                    return 0;

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Errcode;
        }
        #endregion Dispatch Doc

        #endregion Dispatch

        #region FFCharges
        //public DataSet GetPurchaseReceiptList()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetPurchaseReceiptList");
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.LogException(ex);
        //    }
        //    return ds;
        //}
        public DataSet GetFFChargesList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] Objparam = new SqlParameter[2];
                Objparam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Objparam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwGetFFChargesList", Objparam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public DataSet GetAirwaysReceiptList(int FFId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@FFId", SqlDbType.Int);
                parameters[0].Value = FFId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetAirwaysReceiptList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedFFCharges(int RecieptId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                parameters[0].Value = RecieptId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwGetSelectedFFCharges", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public int SaveFFCharges(DataSet ds,string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[8];
                parameters[0] = new SqlParameter("@ReceiptId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["ReceiptId"];

                parameters[1] = new SqlParameter("@FreightInvNo ", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["FreightInvNo"];

                parameters[2] = new SqlParameter("@FreightInvDate", SqlDbType.SmallDateTime);
                parameters[2].Value = ds.Tables[0].Rows[0]["FreightInvDate"];

                parameters[3] = new SqlParameter("@FrieghtInvReceivedDate", SqlDbType.SmallDateTime);
                parameters[3].Value = ds.Tables[0].Rows[0]["FrieghtInvReceivedDate"];

                parameters[4] = new SqlParameter("@Amount", SqlDbType.Float);
                parameters[4].Value = ds.Tables[0].Rows[0]["Amount"];

                parameters[5] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[5].Value = User_Id;

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                parameters[6].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveFFcharges", parameters);
                errCode = (int)parameters[6].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion FFCharges

        #region Status Config

        public DataSet StatusConfigDetailList()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWEnqTrackingStatusConfigList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int UpdateStatusConfigDetail(int ID, int OutstandingResponseTime, int MinimumTime, int MaximumTime, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@ID", SqlDbType.Int);
                parameters[0].Value = ID;

                parameters[1] = new SqlParameter("@OutstandingResponseTime", SqlDbType.Int);
                parameters[1].Value = OutstandingResponseTime;

                parameters[2] = new SqlParameter("@MinimumTime", SqlDbType.Int);
                parameters[2].Value = MinimumTime;

                parameters[3] = new SqlParameter("@MaximumTime", SqlDbType.Int);
                parameters[3].Value = MaximumTime;

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWUpdateEnqTrackingStatusConfig", parameters);
                errCode = (int)parameters[5].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion Status Config

        #region  InvoiveDEtail

        public DataSet GetInvoiceList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] Objparam = new SqlParameter[2];
                Objparam[0] = new SqlParameter("@User_Id",SqlDbType.NVarChar);
                Objparam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetInvoiceList", Objparam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedInvoiceDetails(int InvId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@InvId", SqlDbType.Int);
                parameters[0].Value = InvId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedInvoiceDetail", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveInvoiceDetail(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@InvId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["InvId"];

                parameters[1] = new SqlParameter("@InvRefNo ", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["InvRefNo"];

                parameters[2] = new SqlParameter("@InvFile", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["InvFile"];

                parameters[3] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[0].Rows[0]["CompCode"];

                parameters[4] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[4].Value = ds.Tables[0].Rows[0]["CustomerId"];

                parameters[5] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[5].Value = User_Id;

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                parameters[6].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveInvoiceDetails", parameters);
                errCode = (int)parameters[6].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion InvoiveDEtail

        #region Carton
        public DataSet CartonList()
        {
            DataSet dsvendor = new DataSet();
            try
            {
                dsvendor = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCartonList");
            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsvendor;
        }
        public int DWSaveCarton(int Id, Double Height, Double width, Double lenght, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[9];
                parameters[0] = new SqlParameter("@Height", SqlDbType.Float);
                parameters[0].Value = Height;

                parameters[1] = new SqlParameter("@width", SqlDbType.Float);
                parameters[1].Value = width;

                parameters[2] = new SqlParameter("@lenght", SqlDbType.Float);
                parameters[2].Value = lenght;

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                parameters[5] = new SqlParameter("@Id", SqlDbType.Int);
                parameters[5].Value = Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveCarton", parameters);
                errCode = (int)parameters[4].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion Carton

        #region Freight Forworder  Invoice
        public DataSet GetFreightFwdInvoiceList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] Objparam = new SqlParameter[2];
                Objparam[0] = new SqlParameter("@User_Id",SqlDbType.NVarChar);
                Objparam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWFreightFwdInvoiceList", Objparam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedFreightFwdInvoiceList(int FreightInvId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@FreightInvId", SqlDbType.Int);
                parameters[0].Value = FreightInvId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedFreightFwdInvoiceList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveFreightFwdInvoice(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[9];
                    parameters[0] = new SqlParameter("@FreightInvId", SqlDbType.Int);
                    parameters[0].Value = dr["FreightInvId"]; ;
                    parameters[0].Direction = ParameterDirection.InputOutput;

                    parameters[1] = new SqlParameter("@InvoiceDate", SqlDbType.DateTime);
                    parameters[1].Value = dr["InvoiceDate"];

                    parameters[2] = new SqlParameter("@InvoiceNumber", SqlDbType.VarChar);
                    parameters[2].Value = dr["InvoiceNumber"];

                    parameters[3] = new SqlParameter("@VendorId", SqlDbType.Int);
                    parameters[3].Value = dr["VendorId"];

                    parameters[4] = new SqlParameter("@Remark", SqlDbType.VarChar);
                    parameters[4].Value = dr["Remark"];

                    parameters[5] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[5].Value = User_Id;

                    parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[6].Value = 0;
                    parameters[6].Direction = ParameterDirection.InputOutput;

                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveFreightFwdInvoice", parameters);
                    errCode = (int)parameters[0].Value;
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveFreightFwdInvoiceDetail(DataSet ds, string User_Id, int FreightInvId, int FreightInvDetailId)
        {
            int errCode = 0;
            int Err = 0;
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            bool Allow = true;
            try
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[7];
                    parameters[0] = new SqlParameter("@FreightInvId", SqlDbType.Int);
                    parameters[0].Value = FreightInvId;

                    parameters[1] = new SqlParameter("@RefDate", SqlDbType.DateTime);
                    parameters[1].Value = dr["RefDate"];

                    parameters[2] = new SqlParameter("@FFRef_No", SqlDbType.VarChar);
                    parameters[2].Value = dr["FFRef_No"];


                    parameters[4] = new SqlParameter("@Amount", SqlDbType.Float);
                    parameters[4].Value = dr["Amount"];

                    parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[5].Value = 0;
                    parameters[5].Direction = ParameterDirection.InputOutput;

                    parameters[6] = new SqlParameter("@FreightFwdDetailId", SqlDbType.Int);
                    parameters[6].Value = dr["FreightFwdDetailId"]; ;
                    parameters[6].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DwSaveFreightFwdInvoiceDetail", parameters);
                    errCode = (int)parameters[6].Value;
                }
                if (errCode > 0)
                {
                    SqlParameter[] parameterss = new SqlParameter[2];
                    parameterss[0] = new SqlParameter("@FreightInvDetailId", SqlDbType.Float);
                    parameterss[0].Value = errCode;

                    parameterss[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameterss[1].Value = 0;
                    parameterss[1].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWDeleteFreightFwdInvoiceTaxes", parameterss);
                    Err = (int)parameterss[1].Value;

                    if (Err == 500002)
                    {
                        if (ds.Tables[2].Rows.Count > 0)
                        {

                            if (SaveFreightFwdInvoiceTaxes(ds, sqlTransaction, User_Id, errCode) != 600002)
                            {
                                Allow = false;
                                sqlTransaction.Rollback();
                                sqlConn.Close();
                            }
                        }
                        else
                        {
                            sqlTransaction.Commit();
                            sqlConn.Close();
                            Allow = false;
                        }
                        if (Allow)
                        {
                            sqlTransaction.Commit();
                            sqlConn.Close();
                        }
                    }
                    else
                    {
                        sqlTransaction.Rollback();
                        sqlConn.Close();
                    }

                }
                else
                {
                    sqlTransaction.Rollback();
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                sqlTransaction.Rollback();
                sqlConn.Close();
            }
            return errCode;
        }
        public int SaveFreightFwdInvoiceTaxes(DataSet ds, SqlTransaction tr, string User_Id, int FreightInvId)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[4];
                    parameters[0] = new SqlParameter("@FreightFwdDetailId", SqlDbType.Int);
                    parameters[0].Value = FreightInvId;

                    parameters[1] = new SqlParameter("@TaxId", SqlDbType.Int);
                    parameters[1].Value = dr["TaxId"];

                    parameters[2] = new SqlParameter("@Amount", SqlDbType.Float);
                    parameters[2].Value = dr["Amount"];

                    parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[3].Value = 0;
                    parameters[3].Direction = ParameterDirection.InputOutput;

                    SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_DwSaveFreightFwdInvoiceTaxes", parameters);
                    errCode = (int)parameters[3].Value;
                    if (errCode != 600002)
                    {
                        errCode = 600001;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet GetReceiptDrpList(int FFId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@FFId", SqlDbType.Int);
                parameters[0].Value = FFId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwReceiptListForDrpDwn", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public int DeleteFFDetail(int FreightInvDetailId)
        {
            int errCode = 0;
            int Err = 0;
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            try
            {
                SqlParameter[] parameterss = new SqlParameter[2];
                parameterss[0] = new SqlParameter("@FreightInvDetailId", SqlDbType.Int);
                parameterss[0].Value = errCode;

                parameterss[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameterss[1].Value = 0;
                parameterss[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWDeleteFreightFwdInvoiceTaxes", parameterss);
                Err = (int)parameterss[1].Value;

                if (Err == 500002)
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@FreightInvDetailId", SqlDbType.Int);
                    parameters[0].Value = FreightInvDetailId;

                    parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[1].Value = 0;
                    parameters[1].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWDeleteFreightFwdInvoiceDetail", parameters);
                    errCode = (int)parameters[1].Value;
                    if (errCode == 600001)
                    {
                        sqlTransaction.Commit();
                        sqlConn.Close();
                    }
                    else
                    {
                        sqlTransaction.Rollback();
                        sqlConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                sqlConn.Close();
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion Freight Forworder  Invoice

        #region Enquiry Reason 

        public DataSet GetReasonList(string User_Id)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                param[0].Value = User_Id;
                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetReasonList", param);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }
        public DataSet GetSelectedReasonList(int ReasonId)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@ReasonId", SqlDbType.Int);
                param[0].Value = ReasonId;

                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedReasonDetail", param);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }

        public int DWSaveReasonDetail(int ReasonId, string Reason, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@ReasonId", SqlDbType.Int);
                parameters[0].Value = ReasonId;

                parameters[1] = new SqlParameter("@Reason", SqlDbType.VarChar);
                parameters[1].Value = Reason;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveReasonDetail", parameters);
                errCode = (int)parameters[3].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion Enquiry reason

        #region Enquiry Reason 

        public DataSet GetFollowupTypeList(string User_Id)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                ObjParam[0].Value = User_Id;
                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetFollowupTypeList", ObjParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }
        public DataSet GetSelectedFollowupTypeList(int ReasonId)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@FollowupTypeId", SqlDbType.Int);
                param[0].Value = ReasonId;

                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedFollowupType", param);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }

        public int DWSaveFollowupTypeDetail(int FollowupTypeId, string FollowupReason, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@FollowupTypeId", SqlDbType.Int);
                parameters[0].Value = FollowupTypeId;

                parameters[1] = new SqlParameter("@FollowupReason", SqlDbType.VarChar);
                parameters[1].Value = FollowupReason;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveFollowupType", parameters);
                errCode = (int)parameters[3].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion Enquiry reason

        #region Close Enquiry
        public int SaveCloseEnquiry(int EnqId, int ReasonId, string Remark, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@ReasonId", SqlDbType.Int);
                parameters[0].Value = ReasonId;

                parameters[1] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[1].Value = EnqId;

                parameters[2] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[2].Value = Remark;

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCloseEnquiry", parameters);
                errCode = (int)parameters[4].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion Close Enquiry
        public DataSet GetCustEnquiryVendResponseReport(string user_Id)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@user_Id", SqlDbType.VarChar);
                param[0].Value = user_Id;

                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEnquiryVendResponseReport", param);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }

        #region Quotation Followup
        public DataSet GetQuotFollowupReminderList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWQuotFollowupList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetQuotMyFollowupList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWQuotFollowupMyList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetQuotSubordinateFollowupList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWQuotFollowupSubordinateList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetQuotFollowupListInReport(string user_Id)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@user_Id", SqlDbType.VarChar);
                param[0].Value = user_Id;

                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptDWQuotFollowupListRD", param);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }
        public int SaveFollowupDaetail(int QuotationId, DateTime Followupdate, DateTime NextFollowUp, string FollowupRemark, string User_Id,int FollowupReasonId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@QuotationId", SqlDbType.Int);
                parameters[0].Value = QuotationId;

                parameters[1] = new SqlParameter("@Followupdate", SqlDbType.DateTime);
                parameters[1].Value = Followupdate;

                parameters[2] = new SqlParameter("@NextFollowUp", SqlDbType.DateTime);
                parameters[2].Value = NextFollowUp;

                parameters[3] = new SqlParameter("@FollowupRemark", SqlDbType.VarChar);
                parameters[3].Value = FollowupRemark;

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;

                parameters[6] = new SqlParameter("@FollowupReasonId", SqlDbType.Int);
                parameters[6].Value = FollowupReasonId;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveFollowupDetail", parameters);
                errCode = (int)parameters[5].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        private int SaveNewOuatItem(DataSet ds, SqlTransaction tr, string User_Id, int QuotationId,int OldQuotId,int Status)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[7];
                    parameters[0] = new SqlParameter("@QuotationId", SqlDbType.Int);
                    parameters[0].Value = QuotationId; 

                    parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                    parameters[1].Value = dr["ItemId"];

                    parameters[2] = new SqlParameter("@Remark", SqlDbType.VarChar);
                    parameters[2].Value = dr["FollowupReason"];

                    parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[3].Value = User_Id;
                  
                    parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[4].Value = 0;
                    parameters[4].Direction = ParameterDirection.InputOutput;

                    parameters[5] = new SqlParameter("@OldQuotId", SqlDbType.Int);
                    parameters[5].Value = OldQuotId;

                    parameters[6] = new SqlParameter("@Status", SqlDbType.Int);
                    parameters[6].Value = Status;
                    SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_DWCreateNewQuotItemByFollowup", parameters);
                    errCode = (int)parameters[4].Value;
                    if (errCode != 600002)
                    {
                        errCode = 600001;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet SaveNewOuatDetail(int QuotationId,string FollowupRes, string FollowupRemark, string User_Id,DataSet ds,int Status)
        {
            int errCode = 0;
            DataSet dsset = new DataSet();
            int OldQuotId = QuotationId;
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();       
            sqlTransaction = sqlConn.BeginTransaction();
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@QuotationId", SqlDbType.Int);
                parameters[0].Value = QuotationId;
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@FollowupRes", SqlDbType.VarChar);
                parameters[1].Value = FollowupRes;

                parameters[2] = new SqlParameter("@FollowupRemark", SqlDbType.VarChar);
                parameters[2].Value = FollowupRemark;

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                dsset=SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWCreateNewQuotByFollowup", parameters);
                errCode = (int)parameters[0].Value;
                if (errCode > 0)
                {

                    if (SaveNewOuatItem(ds, sqlTransaction, User_Id, errCode, OldQuotId, Status) != 600002)
                    {
                        sqlTransaction.Rollback();
                        sqlConn.Close();
                    }

                    else
                    {
                        sqlTransaction.Commit();
                        sqlConn.Close();
                    }
                }
                else
                {
                    sqlTransaction.Rollback();
                    sqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                sqlConn.Close();
                Common.LogException(ex);
            }
            return dsset;
        }
        public int UpdateOuatItem(DataSet ds, string User_Id,int NewStatus)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[7];
                    parameters[0] = new SqlParameter("@QuotationId", SqlDbType.Int);
                    parameters[0].Value = dr["QuotationId"]; 

                    parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                    parameters[1].Value = dr["ItemId"];

                    parameters[2] = new SqlParameter("@Remark", SqlDbType.VarChar);
                    parameters[2].Value = dr["FollowupReason"];

                    parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[3].Value = User_Id;

                    parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[4].Value = 0;
                    parameters[4].Direction = ParameterDirection.InputOutput;

                    parameters[5] = new SqlParameter("@NewStatus", SqlDbType.NVarChar);
                    parameters[5].Value = NewStatus;
                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWUpdateQuotItemByFollowup", parameters);
                    errCode = (int)parameters[4].Value;
                    if (errCode != 600002)
                    {
                        errCode = 600001;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet GetQuotFollowupHistoryList(int QuotationId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@QuotationId", SqlDbType.Int);
                parameters[0].Value = QuotationId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetFollowupHistoryList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetFollowupQuotDetail(int QuotId)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] param = new SqlParameter[1];                
                param[0] = new SqlParameter("@QuotId", SqlDbType.Int);
                param[0].Value = QuotId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetFollowupQuotDetailList", param);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Quotation Followup

        #region TO DO
        public DataSet GetAllPendingWorkList(string User_Id, string DateType,string TabName)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                parameters[1] = new SqlParameter("@DateType", SqlDbType.NVarChar);
                parameters[1].Value = DateType;

                parameters[2] = new SqlParameter("@TabName", SqlDbType.VarChar);
                parameters[2].Value = TabName;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetPendingWorkOfEmp", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSubOrdinateList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                                
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwGetSubOrdinateEmployeeDrpWithUser_Id", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetTODOGroupList()
        {
            DataSet ds = new DataSet();
            try
            {               
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetTODOGroupList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        #endregion TO DO

        #region Preliminary Quotation
        public DataSet GetSelectedPreliminaryQuotList(int PreQuotId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PreQuotId", SqlDbType.NVarChar);
                parameters[0].Value = PreQuotId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedEnqPreliminaryQuotation", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedPreQuotRateList(int PreQuotId,int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@PreQuotId", SqlDbType.Int);
                parameters[0].Value = PreQuotId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetPreQuotForEnquiry", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetPreliminaryQuotList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWEnqPreliminaryQuotationList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public int UpdateItemRate(int PreQuotId, int ItemId,double Rate,string User_Id)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@PreQuotId", SqlDbType.Int);
                parameters[0].Value = PreQuotId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                parameters[3] = new SqlParameter("@Rate", SqlDbType.Float);
                parameters[3].Value = Rate;

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWUpdatePreliminaryQuotRate", parameters);
                ErrCode = (int)parameters[3].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        #endregion Preliminary Quotation

        #region Nav POSO Mapping
        public DataSet GetNavPOSOMappingList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetNavPOSOMappingList", ObjParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedNavPOSOMappingList(string NAVPONO,string NAVSONO,string CompCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@NAVPONO", SqlDbType.VarChar);
                parameters[0].Value = NAVPONO;

                parameters[1] = new SqlParameter("@NAVSONO", SqlDbType.VarChar);
                parameters[1].Value = NAVSONO;

                parameters[2] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[2].Value = CompCode;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedNavPOSOMappingList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveNavPOSOMapping(string NAVPONO , string NAVSONO, string CompCode, string User_Id, string OldNAVPONO, string OldNAVSONO, string OldCompCode)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[8];
                parameters[0] = new SqlParameter("@NAVPONO", SqlDbType.VarChar);
                parameters[0].Value = NAVPONO;

                parameters[1] = new SqlParameter("@NAVSONO", SqlDbType.VarChar);
                parameters[1].Value = NAVSONO;

                parameters[2] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[2].Value = CompCode;
              
                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                parameters[5] = new SqlParameter("@OldNAVPONO", SqlDbType.VarChar);
                parameters[5].Value = OldNAVPONO;

                parameters[6] = new SqlParameter("@OldNAVSONO", SqlDbType.VarChar);
                parameters[6].Value = OldNAVSONO;

                parameters[7] = new SqlParameter("@OldCompCode", SqlDbType.VarChar);
                parameters[7].Value = OldCompCode;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveNavPOSOMapping", parameters);
                errCode = (int)parameters[4].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion Nav POSO Mapping

        #region Online MPN Process 
        public int GetEnquiryForPreQuot()
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter("@EnqID", SqlDbType.Int);
                parameters[0].Value = 0;
                parameters[0].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetEnqPendingForPreQuot", parameters);
                errCode = (int)parameters[0].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet GetEnqDetailForProcess(int EnqId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetEnquiryDetailForProcess", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveImportData(int EnqId, string MPN, int SearchId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@MPN", SqlDbType.VarChar);
                parameters[1].Value = MPN;

                parameters[2] = new SqlParameter("@SearchId", SqlDbType.Int);
                parameters[2].Value = SearchId;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveImportData", parameters);
                errCode = (int)parameters[3].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveImportDataSearch(int EnqId, string MPN)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];

                parameters[0] = new SqlParameter("@MPN", SqlDbType.VarChar);
                parameters[0].Value = MPN;

                parameters[1] = new SqlParameter("@SearchId", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;

                parameters[2] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[2].Value = EnqId;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveImportDataSearch", parameters);
                errCode = (int)parameters[1].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveImportDataDigiKey(int EnqId, string MPN, int SearchId, double PriceBreak, double UnitPrice, double ExtendedPrice)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];

                parameters[0] = new SqlParameter("@MPN", SqlDbType.VarChar);
                parameters[0].Value = MPN;

                parameters[1] = new SqlParameter("@SearchId", SqlDbType.Int);
                parameters[1].Value = SearchId;

                parameters[2] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[2].Value = EnqId;

                parameters[3] = new SqlParameter("@PriceBreak", SqlDbType.Float);
                parameters[3].Value = PriceBreak;

                parameters[4] = new SqlParameter("@UnitPrice", SqlDbType.Float);
                parameters[4].Value = UnitPrice;

                parameters[5] = new SqlParameter("@ExtendedPrice", SqlDbType.Float);
                parameters[5].Value = ExtendedPrice;

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                parameters[6].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveImportDataDigiKey", parameters);
                errCode = (int)parameters[6].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveImportDataZauba(int EnqId, string MPN, int SearchId, string TransactionDate, string HSCode, string Description, string OriginCountry, string PortofDischarge, string Unit, double Quantity, double ValueINR, double PerUnitINR)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[13];

                parameters[0] = new SqlParameter("@MPN", SqlDbType.VarChar);
                parameters[0].Value = MPN;

                parameters[1] = new SqlParameter("@SearchId", SqlDbType.Int);
                parameters[1].Value = SearchId;

                parameters[2] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[2].Value = EnqId;

                parameters[3] = new SqlParameter("@TransactionDate", SqlDbType.VarChar);
                parameters[3].Value = TransactionDate;

                parameters[4] = new SqlParameter("@HSCode", SqlDbType.VarChar);
                parameters[4].Value = HSCode;

                parameters[5] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[5].Value = Description;

                parameters[6] = new SqlParameter("@OriginCountry", SqlDbType.VarChar);
                parameters[6].Value = OriginCountry;

                parameters[7] = new SqlParameter("@PortofDischarge", SqlDbType.VarChar);
                parameters[7].Value = PortofDischarge;

                parameters[8] = new SqlParameter("@Unit", SqlDbType.VarChar);
                parameters[8].Value = Unit;

                parameters[9] = new SqlParameter("@Quantity", SqlDbType.Float);
                parameters[9].Value = Quantity;

                parameters[10] = new SqlParameter("@ValueINR", SqlDbType.Float);
                parameters[10].Value = ValueINR;

                parameters[11] = new SqlParameter("@PerUnitINR", SqlDbType.Float);
                parameters[11].Value = PerUnitINR;

                parameters[12] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[12].Value = 0;
                parameters[12].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveImportDataZauba", parameters);
                errCode = (int)parameters[12].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SavePreQuotDetail(int EnqId, DataSet ds)
        {
            int ErroCode = 0;
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            try
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[2];
                    parameters[0] = new SqlParameter("@", SqlDbType.Int);
                    parameters[0].Value = dr["ItemId"];

                    parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                    parameters[0].Value = EnqId;

                    parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[1].Value = 0;
                    parameters[1].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWSavePreQuotDetailByLoader", parameters);
                    ErroCode = (int)parameters[1].Value;
                    if (ErroCode == 600001)
                    {
                        sqlTransaction.Commit();
                        sqlConn.Close();
                    }
                    else
                    {
                        sqlTransaction.Rollback();
                        sqlConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                sqlConn.Close();
                Common.LogException(ex);
            }
            return ErroCode;
        }
        #endregion Online MPN Process 

        #region Material Dispatch Intimetion
        public DataSet GetSelectedPurMRIntimetionList(int MRIId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@MRIId", SqlDbType.Int);
                parameters[0].Value = MRIId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedPurMRIntimetionList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetPurMRIntimetionList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetPurMRIntimetionList", ObjParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int DWSavePurMRIntimetion(DataSet DS, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[8];
                parameters[0] = new SqlParameter("@MRIId", SqlDbType.Int);
                parameters[0].Value = DS.Tables[0].Rows[0]["MRIId"];
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[1].Value = DS.Tables[0].Rows[0]["CompCode"];

                parameters[2] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[2].Value = DS.Tables[0].Rows[0]["VendorId"];

                parameters[3] = new SqlParameter("@MRIDate", SqlDbType.DateTime);
                parameters[3].Value = DS.Tables[0].Rows[0]["MRIDate"];

                parameters[4] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[4].Value = DS.Tables[0].Rows[0]["Remark"];

                parameters[5] = new SqlParameter("@DocumentNo", SqlDbType.VarChar);
                parameters[5].Value = DS.Tables[0].Rows[0]["DocumentNo"];

                parameters[6] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[6].Value = User_Id;

                parameters[7] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[7].Value = 0;
                parameters[7].Direction = ParameterDirection.InputOutput;
                
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSavePurMRIntimetion", parameters);
                errCode = (int)parameters[0].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SavePurMRIntimetionItem(int Itemid, int Quantity, string PONo, DateTime PODate, Double Rate, int MRIId, string User_Id,int MRIDetailId)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[9];
                parameters[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[0].Value = Itemid;
                
                parameters[1] = new SqlParameter("@PONo", SqlDbType.VarChar);
                parameters[1].Value = PONo;

                parameters[2] = new SqlParameter("@Quantity", SqlDbType.Int);
                parameters[2].Value = Quantity;

                parameters[3] = new SqlParameter("@PODate", SqlDbType.DateTime);
                parameters[3].Value = PODate;

                parameters[4] = new SqlParameter("@Rate", SqlDbType.Float);
                parameters[4].Value = Rate;

                parameters[5] = new SqlParameter("@MRIId", SqlDbType.Int);
                parameters[5].Value = MRIId;

                parameters[6] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[6].Value = User_Id;

                parameters[7] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[7].Value = 0;
                parameters[7].Direction = ParameterDirection.InputOutput;

                parameters[8] = new SqlParameter("@MRIDetailId", SqlDbType.Int);
                parameters[8].Value = MRIDetailId;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSavePurMRIntimetionItem", parameters);
                ErrCode = (int)parameters[7].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public int DeleteIntimetionItem(int MRIDetailId)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@Id", SqlDbType.Int);
                parameters[0].Value = MRIDetailId;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDeleteIntimationItem", parameters);
                ErrCode= (int)parameters[1].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        #endregion Material Dispatch Intimetion

        #region Quotation Analyst 
        public DataSet getEnquiryQuotAnalystList(string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = UserId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetEnquiryListForQuotAnalyst", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetQuotAnalystDetailList(int EnqId,string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWEnqQuotAnalystDetail", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetQuotItemAnalystDetailList(int EnqId, int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWEnqQuotItemAnalystDetail", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveAnalystRemark(int EnqId, int ItemId, string Remark,string User_Id)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[9];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;

                parameters[2] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[2].Value = Remark;                

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;
              
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveEnqItemAnalystremark", parameters);
                ErrCode = (int)parameters[4].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        #endregion Quotation Analyst 

        #region Temporary Receipt Entry
        public DataSet TempGetPOforReceiptByItem(string CompCode, int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TempDWPOListForReceiptByItem", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet TempGetItemListByReceipt(string CompCode, int VendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                parameters[1] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[1].Value = VendorId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TempDWItemListForReceipt", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        #endregion Temporary Receipt Entry

        #region Item Stock Audit
        public DataSet GetSystemStockReport(string user_Id,string CompanyCode)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@user_Id", SqlDbType.VarChar);
                param[0].Value = user_Id;

                param[1] = new SqlParameter("@CompanyCode", SqlDbType.VarChar);
                param[1].Value = CompanyCode;

                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWSystemStockReport", param);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }
        public DataSet GetItemStockAuditList()
        {
            DataSet DsProj = new DataSet();
            try
            {
                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetItemStockAuditList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }
        public DataSet GetSelectedItemStockAuditList(int AuditId)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@AuditId", SqlDbType.Int);
                param[0].Value = AuditId;

                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedItemStockAuditList", param);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }
        public int DWSaveItemStockAudit(int AuditId,string CompCode,DateTime AuditDate, string User_Id,string Remark)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@AuditId", SqlDbType.Int);
                parameters[0].Value = AuditId;
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[1].Value = CompCode;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                parameters[4] = new SqlParameter("@AuditDate", SqlDbType.DateTime);
                parameters[4].Value = AuditDate;               

                parameters[5] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[5].Value = Remark;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveItemStockAudit", parameters);
                errCode = (int)parameters[0].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int DWSaveItemStockAuditDetail(int AuditDetailId, int AuditId, int ItemId, int Quantity, string User_Id,int LocationId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@AuditId", SqlDbType.Int);
                parameters[0].Value = AuditId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                parameters[4] = new SqlParameter("@Quantity", SqlDbType.Int);
                parameters[4].Value = Quantity;

                parameters[5] = new SqlParameter("@AuditDetailId", SqlDbType.Int);
                parameters[5].Value = AuditDetailId;

                parameters[6] = new SqlParameter("@LocationId", SqlDbType.Int);
                parameters[6].Value = LocationId;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveItemStockAuditDetail", parameters);
                errCode = (int)parameters[3].Value;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet GetItemStockListForDrp(string CompCode)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                param[0].Value = CompCode;
                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetItemListForStock", param);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }
        public DataSet CheckMPNforStockAudit()
        {
            DataSet DsProj = new DataSet();
            try
            {
                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetItemForStockAudit");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }
        public int SaveStockAuditExcelData(DataSet DS, int AuditId,string User_Id)
        {
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            int errCode = 0;
            try
            {
                SqlParameter[] Delparameters = new SqlParameter[2];
                Delparameters[0] = new SqlParameter("@AuditId", SqlDbType.Int);
                Delparameters[0].Value = AuditId;

                Delparameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Delparameters[1].Value = 0;
                Delparameters[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWDeleteItemStockDetail", Delparameters);
                int DelerrCode = (int)Delparameters[1].Value;
                if (DelerrCode == 500003)
                {
                    foreach (DataRow dr in DS.Tables[1].Rows)
                    {
                        SqlParameter[] parameters = new SqlParameter[7];
                        parameters[0] = new SqlParameter("@AuditId", SqlDbType.Int);
                        parameters[0].Value = AuditId;

                        parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                        parameters[1].Value = Convert.ToInt32(dr["ItemId"]);

                        parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                        parameters[2].Value = User_Id;

                        parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                        parameters[3].Value = 0;
                        parameters[3].Direction = ParameterDirection.InputOutput;

                        parameters[4] = new SqlParameter("@Quantity", SqlDbType.Int);
                        parameters[4].Value = Convert.ToInt32(dr["Quantity"]);

                        parameters[5] = new SqlParameter("@AuditDetailId", SqlDbType.Int);
                        parameters[5].Value = Convert.ToInt32(dr["AuditId"]);

                        parameters[6] = new SqlParameter("@LocationId", SqlDbType.Int);
                        parameters[6].Value = Convert.ToInt32(dr["LocationId"]);
                        SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWSaveItemStockAuditDetail", parameters);
                        errCode = (int)parameters[3].Value;
                        if (errCode != 500001 && errCode != 500002)
                        {
                            sqlTransaction.Rollback();
                            sqlConn.Close();
                            break;
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
            return errCode;
        }
        #endregion Item Stock Audit
    }

}
