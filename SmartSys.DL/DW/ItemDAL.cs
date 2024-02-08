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
    public class ItemDAL
    {
        public DataSet GetItemList(string User_Id)
        {
            DataSet dsBaseReports = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = User_Id;

                dsBaseReports = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWItemGetSelectedList", ObjParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsBaseReports;
        }
        public DataSet GetFilterItemList(string MPN)
        {
            DataSet dsBaseReports = new DataSet();
            try
            {
                SqlParameter[] itemParam = new SqlParameter[1];
                itemParam[0] = new SqlParameter("@FilterStr", SqlDbType.VarChar);
                itemParam[0].Value = MPN;
                dsBaseReports = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetFilterItemList", itemParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsBaseReports;
        }
        public DataSet GetItemListFromCustQuotation(int CustId)
        {
            DataSet dsBaseReports = new DataSet();
            try
            {
                SqlParameter[] itemParam = new SqlParameter[1];
                itemParam[0] = new SqlParameter("@CustId", SqlDbType.Int);
                itemParam[0].Value = CustId;
                dsBaseReports = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetItemListFromCustQuotation", itemParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsBaseReports;
        }
        public DataSet GetItemListfordrp()
        {
            DataSet dsBaseReports = new DataSet();
            try
            {

                dsBaseReports = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetItemListfordrp");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsBaseReports;
        }
        public DataSet GetItemListwithMPNbyVendorId(int VendorId)
        {
            DataSet dsBaseReports = new DataSet();
            try
            {
                SqlParameter[] itemParam = new SqlParameter[1];
                itemParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                itemParam[0].Value = VendorId;
                dsBaseReports = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetProcessedItemListfordrpByVendorId ", itemParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsBaseReports;
        }
        public DataSet GetSelectedItem(int ItemId)
        {
            DataSet dsBaseReports = new DataSet();
            try
            {
                SqlParameter[] itemParam = new SqlParameter[1];
                itemParam[0] = new SqlParameter("@ItemId", SqlDbType.VarChar);
                itemParam[0].Value = ItemId;
                dsBaseReports = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWItemGetSelected", itemParam);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsBaseReports;
        }


        public DataSet BrandList()
        {
            DataSet BrandList = new DataSet();
            try
            {
                BrandList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetBrandList");

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return BrandList;
        }

        public DataSet GetItemcategoryList()
        {
            DataSet category = new DataSet();
            try
            {

                category = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetitemCategory");

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return category;
        }
        public int SaveItem(DataSet dsItem, string User_Id)
        {
            int ErrorCode = 0;
            try
            {
                SqlParameter[] ItemParam = new SqlParameter[11];
                ItemParam[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                ItemParam[0].Value = dsItem.Tables[0].Rows[0]["ItemId"];

                ItemParam[1] = new SqlParameter("@BrandId", SqlDbType.Int);
                ItemParam[1].Value = dsItem.Tables[0].Rows[0]["BrandId"];

                ItemParam[2] = new SqlParameter("@ItemName", SqlDbType.VarChar);
                ItemParam[2].Value = dsItem.Tables[0].Rows[0]["ItemName"];

                ItemParam[3] = new SqlParameter("@User_Id", SqlDbType.VarChar);
                ItemParam[3].Value = User_Id;

                ItemParam[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                ItemParam[4].Value = 0;
                ItemParam[4].Direction = ParameterDirection.InputOutput;

                ItemParam[5] = new SqlParameter("@CategoryId", SqlDbType.Int);
                ItemParam[5].Value = dsItem.Tables[0].Rows[0]["CategoryId"];

                ItemParam[6] = new SqlParameter("@MPN", SqlDbType.VarChar);
                ItemParam[6].Value = dsItem.Tables[0].Rows[0]["MPN"];

                ItemParam[7] = new SqlParameter("@isActive", SqlDbType.Bit);
                ItemParam[7].Value = dsItem.Tables[0].Rows[0]["isActive"];

                ItemParam[8] = new SqlParameter("@PurchaseRate", SqlDbType.Float);
                ItemParam[8].Value = dsItem.Tables[0].Rows[0]["PurchaseRate"];

                ItemParam[9] = new SqlParameter("@SaleRate", SqlDbType.Float);
                ItemParam[9].Value = dsItem.Tables[0].Rows[0]["SaleRate"];

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveItem", ItemParam);
                if (ItemParam[3].Value != null)
                    ErrorCode = Convert.ToInt32(ItemParam[4].Value);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrorCode;
        }
        #region DW Item Mapping

        public string SaveMapping(DataSet dsItemMapping, string User_Id)
        {

            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@Item_No", SqlDbType.VarChar);
                parameters[0].Value = dsItemMapping.Tables[0].Rows[0]["Item_No"].ToString();


                parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[1].Value = dsItemMapping.Tables[0].Rows[0]["CompCode"].ToString(); ;

                parameters[2] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[2].Value = Convert.ToInt32(dsItemMapping.Tables[0].Rows[0]["ItemId"].ToString());

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;
                DataSet ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveItemMapping", parameters);

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
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_DW_ItemNavRel'")
                {
                    return "500000";
                }
            }
            return "0";
        }
        public DataSet GetItemNavRelList(int ItemId)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[0].Value = ItemId;
                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetItemNavRelList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;

        }
        public int DeleteItemMapping(string CompName, int ItemId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CompName", SqlDbType.VarChar);
                parameters[0].Value = CompName;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.VarChar);
                parameters[1].Value = ItemId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DW_DeleteItemNavRel", parameters);

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
        public DataSet GetDWCompItemList()
        {
            DataSet dsobj = new DataSet();
            try
            {
                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCompanyItemList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public DataSet GetClientItemName(string CompName, string CompItemID, ArrayList ConnInfo)
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
                            MySqlCommand sqlComm = new MySqlCommand("sp_GetItemName", connection);
                            sqlComm.Parameters.Add("CompCode", MySqlDbType.VarChar);
                            sqlComm.Parameters.Add("ItemId", MySqlDbType.Int32);
                            sqlComm.Parameters["CompCode"].Value = CompName;
                            sqlComm.Parameters["ItemId"].Value = CompItemID;
                            sqlComm.CommandType = CommandType.StoredProcedure;
                            MySqlDataAdapter da = new MySqlDataAdapter();
                            da.SelectCommand = sqlComm;
                            da.Fill(dsobj);
                        }

                        break;
                    default:
                        ConnectionString = "Data Source=" + ConnInfo[2].ToString() + ";" + "Initial Catalog=" + ConnInfo[3].ToString() + ";" + "uid=" + ConnInfo[4].ToString() + ";" + "pwd=" + ConnInfo[5].ToString() + ";";
                        SqlParameter[] parameters = new SqlParameter[2];
                        parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                        parameters[0].Value = CompName;

                        parameters[1] = new SqlParameter("@ItemId", SqlDbType.VarChar);
                        parameters[1].Value = CompItemID;

                        dsobj = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "sp_GetItemName", parameters);
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public int SaveExtSysLog(string Description, string Item_Id, string User_Id)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[0].Value = Description;

                parameters[1] = new SqlParameter("@RefId", SqlDbType.VarChar);
                parameters[1].Value = Item_Id;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SaveExtSysLog", parameters);

                if (parameters[3].Value != null)
                    return Convert.ToInt32(parameters[3].Value.ToString());
                else
                    return 0;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }
        #endregion DW Item Mapping

        #region DW Item Approver
        public DataSet GetItemApproverList(string User_Id, int Status_Id)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                parameters[1] = new SqlParameter("@Status_Id", SqlDbType.Int);
                parameters[1].Value = Status_Id;

                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetItemMapListByStatus", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public int ApvRejItem(DataSet dsObj, string User_Id)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in dsObj.Tables[0].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@Item_Id", SqlDbType.VarChar);
                    parameters[0].Value = dr["Item_Id"];

                    parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                    parameters[1].Value = dr["CompCode"];

                    parameters[2] = new SqlParameter("@StatusCode", SqlDbType.Int);
                    parameters[2].Value = dr["StatusCode"];

                    parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[3].Value = User_Id;

                    parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[4].Value = 0;
                    parameters[4].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWItemApproveReject", parameters);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion DW Item Approver

        #region DW Item Approver Review

        public int ApvRejItemReview(DataSet dsObj, string User_Id)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in dsObj.Tables[0].Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@Item_Id", SqlDbType.VarChar);
                    parameters[0].Value = dr["Item_Id"];

                    parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                    parameters[1].Value = dr["CompCode"];

                    parameters[2] = new SqlParameter("@StatusCode", SqlDbType.Int);
                    parameters[2].Value = dr["StatusCode"];

                    parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    parameters[3].Value = User_Id;

                    parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[4].Value = 0;
                    parameters[4].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWItemReviewReject", parameters);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion DW Item Approver Review


        #region ItemBrandAllocation
        public DataSet GetBrandListForItem(int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                objParam[0].Value = ItemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetItemBrandList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveItemBrand(DataSet Ds, int ItemId, string User_Id)
        {
            int iErrorCode = 0;
            SqlTransaction sqltransaction;
            SqlConnection sqlconn = new SqlConnection(Common.SqlConnectionString);
            sqlconn.Open();
            sqltransaction = sqlconn.BeginTransaction();
            try
            {
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                    param[0].Value = Ds.Tables["DW_ItemBrand"].Rows[0]["ItemId"].ToString();
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_DWDeleteItemBrand", param);
                }
                else
                {
                    SqlParameter[] paramter = new SqlParameter[1];

                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                    param[0].Value = ItemId;
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_DWDeleteItemBrand", param);
                }

                foreach (DataRow dr in Ds.Tables["DW_ItemBrand"].Rows)
                {
                    iErrorCode = 0;
                    SqlParameter[] SysRoleRptParam = new SqlParameter[4];

                    SysRoleRptParam[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                    SysRoleRptParam[0].Value = dr["ItemId"].ToString();
                    SysRoleRptParam[1] = new SqlParameter("@BrandId", SqlDbType.Int);
                    SysRoleRptParam[1].Value = dr["BrandId"].ToString();
                    SysRoleRptParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    SysRoleRptParam[2].Value = 0;
                    SysRoleRptParam[2].Direction = ParameterDirection.InputOutput;
                    SysRoleRptParam[3] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                    SysRoleRptParam[3].Value = User_Id;
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_DWSaveItemAlloacteBrand", SysRoleRptParam);
                    iErrorCode = Convert.ToInt32(SysRoleRptParam[2].Value);
                    if (iErrorCode != 600002)
                    {
                        sqltransaction.Rollback();
                        sqlconn.Close();
                        return Convert.ToInt32(SysRoleRptParam[2].Value.ToString());
                    }
                }


                sqltransaction.Commit();

            }
            catch (Exception ex)
            {
                sqltransaction.Rollback();
                Common.LogException(ex);
            }

            return iErrorCode;
        }
        #endregion ItemBrandAllocation
    }
}
