using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.DW
{
   public class BudgetDAL
    {
       public DataSet GetBudgetLIst(string User_Id)
       {
           DataSet dsCustomer = new DataSet();
           try
           {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                ObjParam[0].Value = User_Id;
               dsCustomer = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetBudgetlist", ObjParam);
           }
           catch (Exception ex)
           {
               SmartSys.Utility.Common.LogException(ex);
           }
           return dsCustomer;
       }
       public DataSet GetBudgetCompList(string EmpName)
       {
           DataSet ds = new DataSet();
           try
           {
               SqlParameter[] objParam = new SqlParameter[1];
               objParam[0] = new SqlParameter("@EmpName", SqlDbType.VarChar);
               objParam[0].Value = EmpName;
               ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWChkExcelBudgetList", objParam);
           }
           catch (Exception ex)
           {
               SmartSys.Utility.Common.LogException(ex);
           }
           return ds;
       }
       public DataSet GetSelectedBudget(int BudgetId)
       {
           DataSet ds = new DataSet();
           try
           {
               SqlParameter[] objParam = new SqlParameter[1];
               objParam[0] = new SqlParameter("@BudgetId", SqlDbType.Int);
               objParam[0].Value = BudgetId;
               ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWBudgetGetSelected", objParam);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return ds;
       }
       public DataSet GetSelectedBudgetCustViewLst(int CustomerId)
       {
           DataSet ds = new DataSet();
           try
           {
               SqlParameter[] objParam = new SqlParameter[1];
               objParam[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
               objParam[0].Value = CustomerId;
               ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWBudgetGetSelectedCustView", objParam);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return ds;
       }
       public DataSet GetSelectedBudgetEmpViewLst(int EmpId)
       {
           DataSet ds = new DataSet();
           try
           {
               SqlParameter[] objParam = new SqlParameter[1];
               objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
               objParam[0].Value = EmpId;
               ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWBudgetGetSelectedEmpView", objParam);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return ds;
       }
       public int SaveBudgetDetail(DataSet ds)
       {
           int errCode = 0;
           try
           {
               SqlParameter[] parameters = new SqlParameter[22];
               parameters[0] = new SqlParameter("@BudgetId", SqlDbType.Int);
               parameters[0].Value = ds.Tables[0].Rows[0]["BudgetId"];

               parameters[1] = new SqlParameter("@VendorId", SqlDbType.Int);
               parameters[1].Value = ds.Tables[0].Rows[0]["CustomerId"];

               parameters[2] = new SqlParameter("@ItemId", SqlDbType.Int);
               parameters[2].Value = ds.Tables[0].Rows[0]["ItemId"];

               parameters[3] = new SqlParameter("@EmpId", SqlDbType.Int);
               parameters[3].Value = ds.Tables[0].Rows[0]["EmpId"];

               parameters[4] = new SqlParameter("@Application", SqlDbType.VarChar);
               parameters[4].Value = ds.Tables[0].Rows[0]["Application"];

               parameters[5] = new SqlParameter("@EndEquipment", SqlDbType.VarChar);
               parameters[5].Value = ds.Tables[0].Rows[0]["EndEquipment"];

               parameters[6] = new SqlParameter("@Stage", SqlDbType.VarChar);
               parameters[6].Value = ds.Tables[0].Rows[0]["Stage"];

               parameters[7] = new SqlParameter("@UnitPrice", SqlDbType.Float);
               parameters[7].Value = ds.Tables[0].Rows[0]["UnitPrice"];

               parameters[8] = new SqlParameter("@AprQty", SqlDbType.Float);
               parameters[8].Value = ds.Tables[0].Rows[0]["AprQty"];

               parameters[9] = new SqlParameter("@MayQty", SqlDbType.Float);
               parameters[9].Value = ds.Tables[0].Rows[0]["MayQty"];

               parameters[10] = new SqlParameter("@JunQty", SqlDbType.Float);
               parameters[10].Value = ds.Tables[0].Rows[0]["JunQty"];

               parameters[11] = new SqlParameter("@JulQty", SqlDbType.Float);
               parameters[11].Value = ds.Tables[0].Rows[0]["JulQty"];

               parameters[12] = new SqlParameter("@AugQty", SqlDbType.Float);
               parameters[12].Value = ds.Tables[0].Rows[0]["AugQty"];

               parameters[13] = new SqlParameter("@SepQty", SqlDbType.Float);
               parameters[13].Value = ds.Tables[0].Rows[0]["SepQty"];

               parameters[14] = new SqlParameter("@OctQty", SqlDbType.Float);
               parameters[14].Value = ds.Tables[0].Rows[0]["OctQty"];

               parameters[15] = new SqlParameter("@NovQty", SqlDbType.Float);
               parameters[15].Value = ds.Tables[0].Rows[0]["NovQty"];

               parameters[16] = new SqlParameter("@DecQty", SqlDbType.Float);
               parameters[16].Value = ds.Tables[0].Rows[0]["DecQty"];

               parameters[17] = new SqlParameter("@JanQty", SqlDbType.Float);
               parameters[17].Value = ds.Tables[0].Rows[0]["JanQty"];

               parameters[18] = new SqlParameter("@FebQty", SqlDbType.Float);
               parameters[18].Value = ds.Tables[0].Rows[0]["FebQty"];

               parameters[19] = new SqlParameter("@MarQty", SqlDbType.Float);
               parameters[19].Value = ds.Tables[0].Rows[0]["MarQty"];

               parameters[20] = new SqlParameter("@Finyear", SqlDbType.VarChar);
               parameters[20].Value = ds.Tables[0].Rows[0]["Finyear"]; 

               parameters[21] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               parameters[21].Value = 0;
               parameters[21].Direction = ParameterDirection.InputOutput;

               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveBudget", parameters);
               if (parameters[21].Value != null)
                   errCode = Convert.ToInt32(parameters[21].Value.ToString());
           }

           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return errCode;
       }
       public DataSet GetEquipmentList(int ItemId)
       {
           DataSet ds = new DataSet();
           try
           {
               SqlParameter[] objParam = new SqlParameter[1];
               objParam[0] = new SqlParameter("@ItemId", SqlDbType.Int);
               objParam[0].Value = ItemId;
               ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetEquipmentList", objParam);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return ds;
       }

       public int SaveUploadValidDataList(DataSet ds, string User_Id)
       {
           int ErrCode = 0;
           try
           {
               SqlParameter[] parameters = new SqlParameter[37];
               parameters[0] = new SqlParameter("@BudgetId", SqlDbType.Int);
               parameters[0].Value = ds.Tables[0].Rows[0]["BudgetId"];

               parameters[1] = new SqlParameter("@CustomerId", SqlDbType.Int);
               parameters[1].Value = ds.Tables[0].Rows[0]["CustomerId"];

               parameters[2] = new SqlParameter("@ItemId", SqlDbType.Int);
               parameters[2].Value = ds.Tables[0].Rows[0]["ItemId"];

               parameters[3] = new SqlParameter("@EmpId", SqlDbType.Int);
               parameters[3].Value = ds.Tables[0].Rows[0]["EmpId"];

               parameters[4] = new SqlParameter("@Application", SqlDbType.VarChar);
               parameters[4].Value = ds.Tables[0].Rows[0]["Application"];

               parameters[5] = new SqlParameter("@EndEquipment", SqlDbType.VarChar);
               parameters[5].Value = ds.Tables[0].Rows[0]["EndEquipment"];              

               parameters[6] = new SqlParameter("@UnitPrice", SqlDbType.Float);
               parameters[6].Value = ds.Tables[0].Rows[0]["UnitPrice"];

               parameters[7] = new SqlParameter("@CCY", SqlDbType.VarChar);
               parameters[7].Value = ds.Tables[0].Rows[0]["CCY"];

               parameters[8] = new SqlParameter("@AprQty", SqlDbType.Float);
               parameters[8].Value = ds.Tables[0].Rows[0]["AprQty"];

               parameters[9] = new SqlParameter("@MayQty", SqlDbType.Float);
               parameters[9].Value = ds.Tables[0].Rows[0]["MayQty"];

               parameters[10] = new SqlParameter("@JunQty", SqlDbType.Float);
               parameters[10].Value = ds.Tables[0].Rows[0]["JunQty"];

               parameters[11] = new SqlParameter("@JulQty", SqlDbType.Float);
               parameters[11].Value = ds.Tables[0].Rows[0]["JulQty"];

               parameters[12] = new SqlParameter("@AugQty", SqlDbType.Float);
               parameters[12].Value = ds.Tables[0].Rows[0]["AugQty"];

               parameters[13] = new SqlParameter("@SepQty", SqlDbType.Float);
               parameters[13].Value = ds.Tables[0].Rows[0]["SepQty"];

               parameters[14] = new SqlParameter("@OctQty", SqlDbType.Float);
               parameters[14].Value = ds.Tables[0].Rows[0]["OctQty"];

               parameters[15] = new SqlParameter("@NovQty", SqlDbType.Float);
               parameters[15].Value = ds.Tables[0].Rows[0]["NovQty"];

               parameters[16] = new SqlParameter("@DecQty", SqlDbType.Float);
               parameters[16].Value = ds.Tables[0].Rows[0]["DecQty"];

               parameters[17] = new SqlParameter("@JanQty", SqlDbType.Float);
               parameters[17].Value = ds.Tables[0].Rows[0]["JanQty"];

               parameters[18] = new SqlParameter("@FebQty", SqlDbType.Float);
               parameters[18].Value = ds.Tables[0].Rows[0]["FebQty"];

               parameters[19] = new SqlParameter("@MarQty", SqlDbType.Float);
               parameters[19].Value = ds.Tables[0].Rows[0]["MarQty"];


               parameters[20] = new SqlParameter("@AprAmount", SqlDbType.Float);
               parameters[20].Value = ds.Tables[0].Rows[0]["AprAmount"];

               parameters[21] = new SqlParameter("@MayAmount", SqlDbType.Float);
               parameters[21].Value = ds.Tables[0].Rows[0]["MayAmount"];

               parameters[22] = new SqlParameter("@JunAmount", SqlDbType.Float);
               parameters[22].Value = ds.Tables[0].Rows[0]["JunAmount"];

               parameters[23] = new SqlParameter("@JulAmount", SqlDbType.Float);
               parameters[23].Value = ds.Tables[0].Rows[0]["JulAmount"];

               parameters[24] = new SqlParameter("@AugAmount", SqlDbType.Float);
               parameters[24].Value = ds.Tables[0].Rows[0]["AugAmount"];

               parameters[25] = new SqlParameter("@SepAmount", SqlDbType.Float);
               parameters[25].Value = ds.Tables[0].Rows[0]["SepAmount"];

               parameters[26] = new SqlParameter("@OctAmount", SqlDbType.Float);
               parameters[26].Value = ds.Tables[0].Rows[0]["OctAmount"];

               parameters[27] = new SqlParameter("@NovAmount", SqlDbType.Float);
               parameters[27].Value = ds.Tables[0].Rows[0]["NovAmount"];

               parameters[28] = new SqlParameter("@DecAmount", SqlDbType.Float);
               parameters[28].Value = ds.Tables[0].Rows[0]["DecAmount"];

               parameters[29] = new SqlParameter("@JanAmount", SqlDbType.Float);
               parameters[29].Value = ds.Tables[0].Rows[0]["JanAmount"];

               parameters[30] = new SqlParameter("@FebAmount", SqlDbType.Float);
               parameters[30].Value = ds.Tables[0].Rows[0]["FebAmount"];

               parameters[31] = new SqlParameter("@MarAmount", SqlDbType.Float);
               parameters[31].Value = ds.Tables[0].Rows[0]["MarAmount"];

               parameters[32] = new SqlParameter("@Finyear", SqlDbType.VarChar);
               parameters[32].Value = ds.Tables[0].Rows[0]["Finyear"];

               parameters[33] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               parameters[33].Value = 0;
               parameters[33].Direction = ParameterDirection.InputOutput;

               parameters[34] = new SqlParameter("@User_Id", SqlDbType.VarChar);
               parameters[34].Value = User_Id;

               parameters[35] = new SqlParameter("@CityId", SqlDbType.VarChar);
               parameters[35].Value = ds.Tables[0].Rows[0]["CityId"];

               parameters[36] = new SqlParameter("@RegionId", SqlDbType.VarChar);
               parameters[36].Value = ds.Tables[0].Rows[0]["RegionId"];
               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveValidExcelData", parameters);
               if (parameters[33].Value != null)
                   ErrCode = Convert.ToInt32(parameters[33].Value.ToString());
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return ErrCode;
       }
    }
}
