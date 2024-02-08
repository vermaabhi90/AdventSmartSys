using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.Adhoc
{
   public class AdhocDAL
    {
       public int SaveAdhocRun(DataSet dsAdhoc, string User_Id)
       {
           SqlConnection cn = new SqlConnection(Common.SqlConnectionString);
           cn.Open();
           SqlTransaction Tr = cn.BeginTransaction();
           try
           {

           SqlParameter[] parameters = new SqlParameter[4];
           parameters[0] = new SqlParameter("@User_Id", SqlDbType.VarChar);
           parameters[0].Value = User_Id;

           parameters[1] = new SqlParameter("@ReportId", SqlDbType.VarChar);
           parameters[1].Value = dsAdhoc.Tables[0].Rows[0]["ReportId"].ToString();

           parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
           parameters[2].Value = 0;
           parameters[2].Direction = ParameterDirection.InputOutput;

           parameters[3] = new SqlParameter("@AdhocRunId", SqlDbType.Int);
           parameters[3].Value = 0;
           parameters[3].Direction = ParameterDirection.InputOutput;
           SqlHelper.ExecuteDataset(Tr, CommandType.StoredProcedure, "sp_RGSSaveAdhocRun", parameters);
               if (parameters[2].Value != null)
                   if (Convert.ToInt32(parameters[2].Value.ToString()) == 500001)
                   {
                       if (parameters[3].Value != null)
                       {
                           if (Convert.ToInt32(parameters[3].Value.ToString()) > 0)
                           {
                               foreach (DataRow dr in dsAdhoc.Tables[1].Rows)
                               {
                                   if (!(SaveAdhocRunPram(Convert.ToInt32(parameters[3].Value.ToString()), dr["ParamName"].ToString(), dr["ParamValue"].ToString(),Tr) == 500001))
                                   {
                                       Tr.Rollback();
                                       cn.Close();
                                       return 3;
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
                               return 2;
                           }
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
               else
               {
                   Tr.Rollback();
                   cn.Close();
                   return 0;
               }
           }
           catch (Exception ex)
           {
               if (cn.State == ConnectionState.Open)
               {
                   Tr.Rollback();
                   cn.Close();
               }
               throw ex;
           }
       }


       private int SaveAdhocRunPram(int AdhocRunId, string ParamName, string ParamValue,SqlTransaction tr)
       {
           SqlParameter[] parameters = new SqlParameter[4];
           parameters[0] = new SqlParameter("@AdhocRunId", SqlDbType.Int);
           parameters[0].Value = AdhocRunId;

           parameters[1] = new SqlParameter("@ParamName", SqlDbType.VarChar);
           parameters[1].Value = ParamName;

           parameters[2] = new SqlParameter("@ParamValue", SqlDbType.VarChar);
           parameters[2].Value = ParamValue;

           parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
           parameters[3].Value = 0;
           parameters[3].Direction = ParameterDirection.InputOutput;
           try
           {
               SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_RGSSaveAdhocRunParam", parameters);
               if (parameters[3].Value != null)
                   return Convert.ToInt32(parameters[3].Value.ToString());
               else
                   return 0;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }


       public DataSet GetSelectedAchocList()
       {
           DataSet dsAdhoc = new DataSet();
           dsAdhoc = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetSelectedAdhocParamList");
           return dsAdhoc;
       }



       public DataSet GetSelectedList(int AdhocRunId)
       {

           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@AdhocRunId", SqlDbType.VarChar);
               parameters[0].Value = AdhocRunId;
               DataSet dsAdhoc = new DataSet();
               dsAdhoc = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetSelectedAdhocRunList", parameters);
               return dsAdhoc;
           }
           catch (Exception)
           {
               
               throw;
           }
           
       }
       public DataSet GetParamList(string ReportId)
       {
           DataSet dsParam = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@ReportId", SqlDbType.VarChar);
               parameters[0].Value = ReportId;
               dsParam = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetReportParameter", parameters);
           }
           catch (Exception)
           {
               
               throw;
           }
           return dsParam;
       }
       public DataSet GetViewParamList(int AdhocRunId)
       {
           DataSet dsParam = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@AdhocRunId", SqlDbType.Int);
               parameters[0].Value = AdhocRunId;
               dsParam = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSSelectedViewParam", parameters);
           }
           catch (Exception)
           {

               throw;
           }
           return dsParam;
       }
       public DataSet GetList(string user_Id)
       {
           DataSet dsAdhoc=new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@user_Id", SqlDbType.VarChar);
               parameters[0].Value = user_Id;
               dsAdhoc = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetAdhocRunList", parameters);
              
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsAdhoc;
       }
       public DataSet GetReportNameList(string user_Id)
       {
           DataSet dsAdhoc = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@user_Id", SqlDbType.VarChar);
               parameters[0].Value = user_Id;
               dsAdhoc = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetReportListByUser", parameters);
               return dsAdhoc;
           }
           catch (Exception)
           {

               throw;
           }

       }
        public DataSet FYDropDown()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptFinancialYear");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet GetClientEmpCodeLst(string CompCode)
       {
           DataSet DsObj = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
               parameters[0].Value = CompCode;
               DsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSAdhocClientEmpCodeLst", parameters);
           }
           catch (Exception)
           {

               throw;
           }
           return DsObj;
       }
       public DataSet GetClientCustCodeLst(string CompCode)
       {
           DataSet DsObj = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
               parameters[0].Value = CompCode;
               DsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSAdhocClientCustomerLst", parameters);
           }
           catch (Exception)
           {

               throw;
           }
           return DsObj;
       }
    }
}
