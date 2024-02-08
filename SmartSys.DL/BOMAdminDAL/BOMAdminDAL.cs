using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.BOMAdminDAL
{
  public  class BOMAdminDAL
    {
        #region Paymentterms
        public DataSet GetPaymentTermsList(string UserId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_ID",SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;

                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWPaymentTermsList", ObjParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }

        public DataSet GetSelectedPaymentTerms(int PTId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@PTId", SqlDbType.Int);
                objParam[0].Value = PTId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedPaymentTerms", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }


        public int SavepaymentTerms(DataSet ds, string User_Id)
        {

            try
            {
                SqlParameter[] objParam = new SqlParameter[6];
                objParam[0] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;

                objParam[1] = new SqlParameter("@PTId", SqlDbType.VarChar);
                objParam[1].Value = ds.Tables[0].Rows[0]["PTId"].ToString();


                objParam[2] = new SqlParameter("@PTCode", SqlDbType.VarChar);
                objParam[2].Value = ds.Tables[0].Rows[0]["PTCode"].ToString();

                objParam[3] = new SqlParameter("@Description", SqlDbType.VarChar);
                objParam[3].Value = ds.Tables[0].Rows[0]["Description"].ToString();

                objParam[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[4].Value = 0;
                objParam[4].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSavePaymentTerms", objParam);
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

        #endregion Paymentterms

        #region Brand Master

        public DataSet BrandList(string UserId)
        {
            DataSet dsvendor = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_ID",SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
                dsvendor = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWBrandList", ObjParam);
            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsvendor;
        }

        public DataSet GetSelectedBrandList(int BrandId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@BrandId", SqlDbType.Int);
                objParam[0].Value = BrandId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWBrandSelectedList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public int SaveBrandDetail(string User_Id, int BrandId, string BrandName)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[8];
                parameters[0] = new SqlParameter("@BrandId", SqlDbType.Int);
                parameters[0].Value = BrandId;

                parameters[1] = new SqlParameter("@BrandName", SqlDbType.VarChar);
                parameters[1].Value = BrandName;

                parameters[2] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveBrandDetails", parameters);
                if (parameters[3].Value != null)
                    errCode = Convert.ToInt32(parameters[3].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return errCode;
            }
            return errCode;
        }

        #endregion Brand Master

        #region Brand Item  Allocation
        public DataSet GetBrandItemAllocateList(int BrandId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@BrandId", SqlDbType.Int);
                objParam[0].Value = BrandId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWBrandItemList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveAllocatedBrandItem(DataSet ds, string User_Id, int BrandId)
        {
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            int ErrCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@BrandId", SqlDbType.Int);
                objParam[0].Value = BrandId;

                objParam[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[1].Value = 0;
                objParam[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWDeleteBrandItem", objParam);
                if (objParam[1].Value != null)
                    ErrCode = Convert.ToInt32(objParam[1].Value.ToString());
                if (ErrCode == 500003)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        SqlParameter[] Parameter = new SqlParameter[4];
                        Parameter[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                        Parameter[0].Value = dr["ItemId"];

                        Parameter[1] = new SqlParameter("@BrandId", SqlDbType.Int);
                        Parameter[1].Value = dr["BrandId"];

                        Parameter[2] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                        Parameter[2].Value = User_Id;

                        Parameter[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                        Parameter[3].Value = 0;
                        Parameter[3].Direction = ParameterDirection.InputOutput;
                        SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWSaveItemAlloacteBrand", Parameter);
                        if (Parameter[3].Value != null)
                            ErrCode = Convert.ToInt32(Parameter[3].Value.ToString());
                        else
                            return 0;
                        if (ErrCode != 600002)
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
        #endregion Brand Item  Allocation       

        #region Brand Vendor  Allocation
        public DataSet GetBrandVendorAllocateList(int BrandId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@BrandId", SqlDbType.Int);
                objParam[0].Value = BrandId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWBrandVendorList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveAllocatedBrandVendor(DataSet ds, string User_Id, int BrandId)
        {
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            int ErrCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@BrandId", SqlDbType.Int);
                objParam[0].Value = BrandId;

                objParam[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[1].Value = 0;
                objParam[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DWDeleteBrandVendor", objParam);
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
        #endregion Brand Vendor  Allocation

        #region EmailProcessing
        public DataSet GetMailDetailList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] Parameter = new SqlParameter[1];
                Parameter[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Parameter[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwEmailProcessingList", Parameter);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSubordinateMailDetailList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] Parameter = new SqlParameter[1];
                Parameter[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Parameter[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSubordinateEmailProcessingList", Parameter);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedMailDetail(int ClientMailId, int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] Parameter = new SqlParameter[2];
                Parameter[0] = new SqlParameter("@ClientMailId", SqlDbType.Int);
                Parameter[0].Value = ClientMailId;

                Parameter[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                Parameter[1].Value = EmpId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwGetSelectedMailDetail", Parameter);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetEmailDetailForAttachment(int MailId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] Parameter = new SqlParameter[1];
                Parameter[0] = new SqlParameter("@MailId", SqlDbType.Int);
                Parameter[0].Value = MailId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetEmailDetailForAttachment", Parameter);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int UpdateEmailProcessing(int Status, int MailId, int CustomerId, string enqnumber, string User_Id, string FilePath,int CustomerContactId)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] Parameter = new SqlParameter[8];
                Parameter[0] = new SqlParameter("@Status", SqlDbType.Int);
                Parameter[0].Value = Status;

                Parameter[1] = new SqlParameter("@MailId", SqlDbType.Int);
                Parameter[1].Value = MailId;

                Parameter[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[2].Value = 0;
                Parameter[2].Direction = ParameterDirection.InputOutput;

                Parameter[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Parameter[3].Value = User_Id;

                Parameter[4] = new SqlParameter("@CustomerId", SqlDbType.Int);
                Parameter[4].Value = CustomerId;

                Parameter[5] = new SqlParameter("@enqnumber", SqlDbType.NVarChar);
                Parameter[5].Value = enqnumber;

                Parameter[6] = new SqlParameter("@FilePath", SqlDbType.VarChar);
                Parameter[6].Value = FilePath;

                Parameter[7] = new SqlParameter("@CustomerContactId", SqlDbType.Int);
                Parameter[7].Value = CustomerContactId;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWUpdateEmailPRocessingDetail", Parameter);
                if (Parameter[2].Value != null)
                   ErrCode = Convert.ToInt32(Parameter[2].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        #endregion EmailProcessing

        #region Bank Cheque 
        public DataSet GetBankChequeList(string UserId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;

                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetBankChequelist", ObjParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }

        public DataSet GetSelectedBankChequeList(int ChequeId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ChequeId", SqlDbType.Int);
                objParam[0].Value = ChequeId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedBankChequelist", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }

        public int SaveBankChequeDetail(DataSet ds, string User_Id)
        {

            try
            {
                SqlParameter[] objParam = new SqlParameter[16];

                objParam[0] = new SqlParameter("@ChequeId", SqlDbType.Int);
                objParam[0].Value = ds.Tables[0].Rows[0]["ChequeId"].ToString();

                objParam[1] = new SqlParameter("@BankId", SqlDbType.Int);
                objParam[1].Value = ds.Tables[0].Rows[0]["BankId"].ToString();

                objParam[2] = new SqlParameter("@ChequeNumber", SqlDbType.VarChar);
                objParam[2].Value = ds.Tables[0].Rows[0]["ChequeNumber"].ToString();

                objParam[3] = new SqlParameter("@ChequeDate", SqlDbType.DateTime);
                objParam[3].Value = ds.Tables[0].Rows[0]["ChequeDate"].ToString();

                objParam[4] = new SqlParameter("@IssuedToId", SqlDbType.Int);
                objParam[4].Value = ds.Tables[0].Rows[0]["IssuedToId"].ToString();

                objParam[5] = new SqlParameter("@IssuedToType", SqlDbType.VarChar);
                objParam[5].Value = ds.Tables[0].Rows[0]["IssuedToType"].ToString();

                objParam[6] = new SqlParameter("@IssuedDate", SqlDbType.DateTime);
                objParam[6].Value = ds.Tables[0].Rows[0]["IssuedDate"].ToString();

                objParam[7] = new SqlParameter("@IssuedToOther", SqlDbType.VarChar);
                objParam[7].Value = ds.Tables[0].Rows[0]["IssuedToOther"].ToString();

                objParam[8] = new SqlParameter("@Amount", SqlDbType.Float);
                objParam[8].Value = ds.Tables[0].Rows[0]["Amount"].ToString();

                objParam[9] = new SqlParameter("@Remark", SqlDbType.VarChar);
                objParam[9].Value = ds.Tables[0].Rows[0]["Remark"].ToString();

                objParam[10] = new SqlParameter("@Currency", SqlDbType.VarChar);
                objParam[10].Value = ds.Tables[0].Rows[0]["Currency"].ToString();

                objParam[11] = new SqlParameter("@ClearingDate", SqlDbType.DateTime);
                objParam[11].Value = ds.Tables[0].Rows[0]["ClearingDate"].ToString();

                objParam[12] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                objParam[12].Value = User_Id;

                objParam[13] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[13].Value = 0;
                objParam[13].Direction = ParameterDirection.InputOutput;

                objParam[14] = new SqlParameter("@ChequeDocument", SqlDbType.VarChar);
                objParam[14].Value = ds.Tables[0].Rows[0]["ChequeDocument"].ToString();

                objParam[15] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                objParam[15].Value = ds.Tables[0].Rows[0]["CompCode"].ToString();
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveBankCheque", objParam);
                if (objParam[13].Value != null)
                    return Convert.ToInt32(objParam[13].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }
        #endregion Bank Cheque

        #region TaxOnTax
       
        public DataSet GetTaxList(string UserId)
        {
            DataSet dsTaxList = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_ID",SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
                dsTaxList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_Sysgettaxlist", ObjParam);

            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsTaxList;
        }
        public DataSet GetTaxType()
        {
            DataSet dsTaxType = new DataSet();
            try
            {
                dsTaxType = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetTaxType");

            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsTaxType;
        }
        public DataSet GetSelectedTaxDetail(int TaxId)
        {
            DataSet dsTax = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@TaxId", SqlDbType.Int);
                objParam[0].Value = TaxId;
                dsTax = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysgetSelectedtax", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTax;

        }
        public int SaveTaxDetail(DataSet dstax, int TaxId, string TaxType,string User_Id)
        {
            SqlConnection cn = new SqlConnection(Common.SqlConnectionString);
            cn.Open();
            SqlTransaction Tr = cn.BeginTransaction();
            try
            {
                SqlParameter[] parameters = new SqlParameter[8];
                parameters[0] = new SqlParameter("@TaxId", SqlDbType.Int);
                parameters[0].Value = dstax.Tables[0].Rows[0]["TaxId"];
                var tax = parameters[0].Value;
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@TaxName", SqlDbType.VarChar);
                parameters[1].Value = dstax.Tables[0].Rows[0]["TaxName"];

                parameters[2] = new SqlParameter("@TaxType", SqlDbType.VarChar);
                parameters[2].Value = dstax.Tables[0].Rows[0]["TaxType"].ToString();
                var t = parameters[2].Value;

                parameters[3] = new SqlParameter("@TaxRate", SqlDbType.Float);
                parameters[3].Value = dstax.Tables[0].Rows[0]["TaxRate"].ToString();

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                parameters[5] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[5].Value = User_Id;
                SqlHelper.ExecuteDataset(Tr, CommandType.StoredProcedure, "sp_SysSaveTaxDetail", parameters);
               
                    int Err = 500002;
                    if (Convert.ToInt32(parameters[4].Value.ToString()) == 500002 || Convert.ToInt32(parameters[4].Value.ToString()) == 500001)
                    {

                        SqlParameter[] parameters1 = new SqlParameter[2];
                        parameters1[0] = new SqlParameter("@TaxId", SqlDbType.Int);
                        parameters1[0].Value = TaxId;

                        parameters1[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                        parameters1[1].Value = 0;
                        parameters1[1].Direction = ParameterDirection.InputOutput;
                        SqlHelper.ExecuteDataset(Tr, CommandType.StoredProcedure, "sp_SysDeleteTaxonTaxDetail", parameters1);
                        Err = (int)parameters1[1].Value;
                        if (Err == 500002 && TaxType == "3")
                        {

                            if (Convert.ToInt32(t) == 3)
                            {
                                if (parameters[0].Value != null || (Convert.ToInt32(tax) == 0))
                                {
                                    foreach (DataRow dr in dstax.Tables[2].Rows)
                                    {
                                        if (!(SaveTaxOnTax(Convert.ToInt32(parameters[0].Value.ToString()), Convert.ToInt32(dr["ChildTaxId"].ToString()), Tr) == 500002))
                                        {
                                            Tr.Rollback();
                                            cn.Close();
                                            return 2;
                                        }
                                    }
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
                        else
                        {
                            Tr.Commit();
                            cn.Close();
                            return 4;
                        }
                    }
                    else
                    {
                        Tr.Rollback();
                        cn.Close();
                        return 1;
                    }                
            }

            catch (Exception)
            {
                Tr.Rollback();
                cn.Close();
                return 0;
            }         
        }
        private int SaveTaxOnTax(int TaxId, int ParentTaxId, SqlTransaction tr)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@TaxId", SqlDbType.Int);
                parameters[0].Value = TaxId;

                parameters[1] = new SqlParameter("@ChildTaxId", SqlDbType.Int);
                parameters[1].Value = ParentTaxId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_SaveTaxOnTaxDetails", parameters);
                if (parameters[2].Value != null)
                    return Convert.ToInt32(parameters[2].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        public DataSet TaxOnTaxList(int TaxId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] Parameter = new SqlParameter[1];
                Parameter[0] = new SqlParameter("@TaxId", SqlDbType.Int);
                Parameter[0].Value = TaxId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TaxOnTaxList", Parameter);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        #endregion TaxOnTax

        #region Inward Document
        public DataSet GetStockLocationListForInward()
        {
            DataSet DSObj = new DataSet();
            try
            {

                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwGetStockLocationForInwardDoc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public DataSet GetInwardDocumentList(string UserId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] Objparam = new SqlParameter[2];
                Objparam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Objparam[0].Value = UserId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetInwardDocumentlist", Objparam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public DataSet GetSelectedInwardDocumentList(int DocId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@DocId", SqlDbType.Int);
                objParam[0].Value = DocId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedInwardDocumentlist", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public int SaveInwardDocument(DataSet ds, string User_Id)
        {
            int ErrorCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[14];

                objParam[0] = new SqlParameter("@DocId", SqlDbType.Int);
                objParam[0].Value = ds.Tables[0].Rows[0]["DocId"].ToString();
                objParam[0].Direction = ParameterDirection.InputOutput;

                objParam[1] = new SqlParameter("@DocName", SqlDbType.NVarChar);
                objParam[1].Value = ds.Tables[0].Rows[0]["DocName"].ToString();

                objParam[2] = new SqlParameter("@DocToId", SqlDbType.Int);
                objParam[2].Value = ds.Tables[0].Rows[0]["DocToId"].ToString();

                objParam[3] = new SqlParameter("@DocTOType", SqlDbType.NVarChar);
                objParam[3].Value = ds.Tables[0].Rows[0]["DocTOType"].ToString();

                objParam[4] = new SqlParameter("@DocDate", SqlDbType.DateTime);
                objParam[4].Value = ds.Tables[0].Rows[0]["DocDate"].ToString();

                objParam[5] = new SqlParameter("@DocToOther", SqlDbType.VarChar);
                objParam[5].Value = ds.Tables[0].Rows[0]["DocToOther"].ToString();

                objParam[6] = new SqlParameter("@NatureOfDoc", SqlDbType.VarChar);
                objParam[6].Value = ds.Tables[0].Rows[0]["NatureOfDoc"].ToString();

                objParam[7] = new SqlParameter("@NatureOfDocOther", SqlDbType.VarChar);
                objParam[7].Value ="";

                objParam[8] = new SqlParameter("@Amount", SqlDbType.Float);
                objParam[8].Value = ds.Tables[0].Rows[0]["Amount"].ToString();
               
                objParam[9] = new SqlParameter("@Currency", SqlDbType.VarChar);
                objParam[9].Value = ds.Tables[0].Rows[0]["Currency"].ToString();               

                objParam[10] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                objParam[10].Value = User_Id;

                objParam[11] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[11].Value = 0;
                objParam[11].Direction = ParameterDirection.InputOutput;


                objParam[12] = new SqlParameter("@STLocationId", SqlDbType.Int);
                objParam[12].Value = ds.Tables[0].Rows[0]["STLocationId"].ToString();

                objParam[13] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                objParam[13].Value = ds.Tables[0].Rows[0]["CompCode"].ToString();

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveInwardDocument", objParam);
                if (objParam[0].Value != null)
                    ErrorCode = Convert.ToInt32(objParam[0].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrorCode;
        }
        public int SaveInwardDocumentDetail(DataSet ds, string User_Id)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[5];

                objParam[0] = new SqlParameter("@DocId", SqlDbType.Int);
                objParam[0].Value = ds.Tables[1].Rows[0]["DocId"].ToString();

                objParam[1] = new SqlParameter("@FileName", SqlDbType.VarChar);
                objParam[1].Value = ds.Tables[1].Rows[0]["FileName"].ToString();
                
                objParam[2] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                objParam[2].Value = User_Id;

                objParam[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[3].Value = 0;
                objParam[3].Direction = ParameterDirection.InputOutput;

                objParam[4] = new SqlParameter("@DocumentTitle", SqlDbType.VarChar);
                objParam[4].Value = ds.Tables[1].Rows[0]["DocumentTitle"].ToString();
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveInwardDocumentDetail", objParam);
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
        #endregion Inward Document
    }
}
