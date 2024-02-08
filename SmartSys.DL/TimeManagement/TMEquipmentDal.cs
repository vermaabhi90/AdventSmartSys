using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SmartSys.DAL;
using SmartSys.Utility;
using System.Data.SqlClient;

namespace SmartSys.DL.TimeManagement
{
    public class TMEquipmentDal
    {
        public DataSet GetTMEquipmentList()
        {
            DataSet dsData = new DataSet();
            dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMEquipmentList");
            return dsData;
        }
        public DataSet GetTMEquipmentItemList()
        {
            DataSet dsData = new DataSet();
            dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMEquipmentItemList");
            return dsData;
        }
        public DataSet GetSelectedEquipment(int EquipmentId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                objParam[0].Value = EquipmentId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetSelectedEquipment", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetParentEquipmentList(int EquipmentId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                objParam[0].Value = EquipmentId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMParentEquipmentList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
       
        public DataSet GetCurrencyCodes()
        {
            DataSet ds = new DataSet();
            try
            {

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetCurrencyCodes");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSegmentList(int EquipmentId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                objParam[0].Value = EquipmentId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSegmentListForEquipment", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveEquipmentDetails(DataSet ds, string User_Id)
        {
           int errCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[12];
                objParam[0] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                objParam[0].Value = ds.Tables[0].Rows[0]["EquipmentId"];

                objParam[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                objParam[2].Value = ds.Tables[0].Rows[0]["Description"];

                objParam[3] = new SqlParameter("@EquipmentName", SqlDbType.VarChar);
                objParam[3].Value = ds.Tables[0].Rows[0]["EquipmentName"];

                objParam[4] = new SqlParameter("@User_ID", SqlDbType.VarChar);
                objParam[4].Value = User_Id;

                objParam[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[5].Value = 0;
                objParam[5].Direction = ParameterDirection.InputOutput;

                objParam[6] = new SqlParameter("@ParentEquipmentId", SqlDbType.VarChar);
                objParam[6].Value = ds.Tables[0].Rows[0]["ParentEquipmentId"];

                objParam[7] = new SqlParameter("@TAM", SqlDbType.Float);
                objParam[7].Value = ds.Tables[0].Rows[0]["TAM"];

                objParam[8] = new SqlParameter("@TAMSource", SqlDbType.VarChar);
                objParam[8].Value = ds.Tables[0].Rows[0]["TAMSource"];

                objParam[9] = new SqlParameter("@Rate", SqlDbType.Float);
                objParam[9].Value = ds.Tables[0].Rows[0]["Rate"];

                objParam[10] = new SqlParameter("@TAMDate", SqlDbType.DateTime);
                objParam[10].Value = ds.Tables[0].Rows[0]["TAMDate"];

                objParam[11] = new SqlParameter("@CurrencyCode", SqlDbType.VarChar);
                objParam[11].Value = ds.Tables[0].Rows[0]["CurrencyCode"];


                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveEquipmentDetails", objParam);
                if (objParam[5].Value != null)
                    errCode = Convert.ToInt32(objParam[5].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet GetSelectedEquipmentItem(int EquipmentId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                objParam[0].Value = EquipmentId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetSelectedEquipmentList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public DataSet GetItemPurchaseSalesRate(int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                objParam[0].Value = ItemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetPurchaseSalesRateByItem", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public int DeleteEquipmentItem(int ItemId , int EqipmentId, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                objParam[0].Value = ItemId;
            
                objParam[1] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                objParam[1].Value = EqipmentId;

                objParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[2].Value = 0;
                objParam[2].Direction = ParameterDirection.InputOutput;

                objParam[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[3].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDeleteEquipmentItem", objParam);
                if (objParam[2].Value != null)
                    errCode = Convert.ToInt32(objParam[2].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int DeleteEquipmentSegment(int SegmentId, int EqipmentId,string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@SegmentId", SqlDbType.Int);
                objParam[0].Value = SegmentId;

                objParam[1] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                objParam[1].Value = EqipmentId;

                objParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[2].Value = 0;
                objParam[2].Direction = ParameterDirection.InputOutput;

                objParam[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[3].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDeleteEquipmentSegment", objParam);
                if (objParam[2].Value != null)
                    errCode = Convert.ToInt32(objParam[2].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveEquipmentItem(int EquipmentId, double Quantity, int Itemid, string User_Id ,double PurchaseRate, double SaleRate)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[7];
                objParam[0] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                objParam[0].Value = EquipmentId;

                objParam[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                objParam[1].Value = Itemid;

                objParam[2] = new SqlParameter("@Quantity", SqlDbType.Float);
                objParam[2].Value = Quantity;

                objParam[3] = new SqlParameter("@User_ID", SqlDbType.VarChar);
                objParam[3].Value = User_Id;

                objParam[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[4].Value = 0;
                objParam[4].Direction = ParameterDirection.InputOutput;

                objParam[5] = new SqlParameter("@PurchaseRate", SqlDbType.Float);
                objParam[5].Value = PurchaseRate;

                objParam[6] = new SqlParameter("@SaleRate", SqlDbType.Float);
                objParam[6].Value = SaleRate;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveEquipmentItem", objParam);
                if (objParam[4].Value != null)
                    errCode = Convert.ToInt32(objParam[4].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveSegmentDetail(int SegmentId, int EquipmentId,string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                objParam[0].Value = EquipmentId;

                objParam[1] = new SqlParameter("@SegmentId", SqlDbType.Int);
                objParam[1].Value = SegmentId;
                
                objParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[2].Value = 0;
                objParam[2].Direction = ParameterDirection.InputOutput;

                objParam[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[3].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveEquipmentSegment", objParam);
                if (objParam[2].Value != null)
                    errCode = Convert.ToInt32(objParam[2].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
    }
}
