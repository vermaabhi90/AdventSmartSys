using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.RDS
{


    public  class RDSEngineDAL
    {
       public DataSet RDSGetEngineList()
       {
           DataSet dsRDSEngine = new DataSet();
           try
           {
               dsRDSEngine = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetEngineList");
           }
           catch (Exception ex)
           {

               Common.LogException(ex);
           }
           return dsRDSEngine;
       }
       public DataSet GetRDSReDistributionList()
       {
           DataSet dsRDSEngine = new DataSet();
           try
           {
               dsRDSEngine = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetRe-DistributionList");
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsRDSEngine;
       }

       public DataSet RDSGetSelectedEngine(Int16 RDSEngineId)
       {
           DataSet dsRDSEngine = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@RDSEngineId", SqlDbType.SmallInt);
               parameters[0].Value = RDSEngineId;
               dsRDSEngine = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetSelectedEngine", parameters);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsRDSEngine;
       }
       public int RegisterRDSEngine(Int16 RDSEngineId, int ProcessId)
       {
           int errorCode = 0;
           try
           {
               SqlParameter[] parameters = new SqlParameter[3];
               parameters[0] = new SqlParameter("@RDSEngineId", SqlDbType.SmallInt);
               parameters[0].Value = RDSEngineId;
               parameters[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
               parameters[1].Value = ProcessId;
               parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               parameters[2].Value = 0;
               parameters[2].Direction = ParameterDirection.InputOutput;
               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSSetEngineProcessId", parameters);
               if (parameters[2].Value != null)
                   errorCode = Convert.ToInt32(parameters[2].Value.ToString());
           }
           catch (Exception ex)
           {
               throw ex;
           }
           return errorCode;
       }

       public DataSet GetRDSEngineInfoByHost(string HostName)
       {
           DataSet dsRDSEngine = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@HostName", SqlDbType.VarChar);
               parameters[0].Value = HostName;
               dsRDSEngine = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetEngineInfoByHost", parameters);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsRDSEngine;
       }

       public int SetEngineStatus(Int16 EngineId, Int16 StatusId)
       {
           int ErrorCode = 0;
           try
           {
               SqlParameter[] parameters = new SqlParameter[3];
               parameters[0] = new SqlParameter("@EngineId", SqlDbType.SmallInt);
               parameters[0].Value = EngineId;
               parameters[1] = new SqlParameter("@StatusId", SqlDbType.SmallInt);
               parameters[1].Value = StatusId;
               parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               parameters[2].Value = 0;
               parameters[2].Direction = ParameterDirection.InputOutput;
               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSSetEngineStatus", parameters);
               if (parameters[2].Value != null)
                   return Convert.ToInt32(parameters[2].Value);
               else
                   return 0;
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return ErrorCode;
       }

        

       public DataSet GetDistDetails(Int32 DlyGenSubId)
       {
           DataSet dsDistDetails = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@DlyGenSubId", SqlDbType.Int);
               parameters[0].Value = DlyGenSubId;
               dsDistDetails = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetDistDetailsforGenSubId", parameters);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsDistDetails;
       }
       public void SetDLYGenSubStatus(Int32 DlyGenSubId, int statusId)
       {
           
           try
           {
               SqlParameter[] parameters = new SqlParameter[2];
               parameters[0] = new SqlParameter("@DlyGenSubId", SqlDbType.Int);
               parameters[0].Value = DlyGenSubId;
               parameters[1] = new SqlParameter("@StatusId", SqlDbType.SmallInt);
               parameters[1].Value = statusId;
               SqlHelper.ExecuteNonQuery(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSSetDLYGenSubStatus", parameters);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
       }
        

       public int RDSProcessandCreateRGSJob( Int32 DlyGenSubId)
       {
           int errorCode = 0;
           int JobId = 0;
           try
           {
               SqlParameter[] parameters = new SqlParameter[3];
               parameters[0] = new SqlParameter("@DlyGenSubId", SqlDbType.SmallInt);
               parameters[0].Value = DlyGenSubId;
               parameters[1] = new SqlParameter("@JobId", SqlDbType.Int);
               parameters[1].Value = 0;
               parameters[1].Direction = ParameterDirection.InputOutput;
               parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               parameters[2].Value = 0;
               parameters[2].Direction = ParameterDirection.InputOutput;
               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSInvokeRGSJob", parameters);
               errorCode = Convert.ToInt32(parameters[2].Value.ToString());
               if (errorCode == 500001)
               {
                   JobId = Convert.ToInt32(parameters[1].Value.ToString());
               }
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return JobId;
       }

       public DataSet GetJobs(Int16 RDSEngineId)
       {
           DataSet dsRDSJobs = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[2];
               parameters[0] = new SqlParameter("@RDSEngineId", SqlDbType.SmallInt);
               parameters[0].Value = RDSEngineId;
               parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.SmallInt);
               parameters[1].Value = 0;

               dsRDSJobs = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetJobs", parameters);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsRDSJobs;
       }

       public int SaveRDSEngine(DataSet dsRDSEngine, string User_Id)
       {
           try
           {

               SqlParameter[] parameters = new SqlParameter[12];
               parameters[0] = new SqlParameter("@RDSEngineId", SqlDbType.SmallInt);
               parameters[0].Value = dsRDSEngine.Tables[0].Rows[0]["RDSEngineId"];

               parameters[1] = new SqlParameter("@Host", SqlDbType.VarChar);
               parameters[1].Value = dsRDSEngine.Tables[0].Rows[0]["Host"].ToString();

               parameters[2] = new SqlParameter("@Port", SqlDbType.VarChar);
               parameters[2].Value = dsRDSEngine.Tables[0].Rows[0]["Port"].ToString();

               parameters[3] = new SqlParameter("@JobPollingInterval", SqlDbType.SmallInt);
               parameters[3].Value = dsRDSEngine.Tables[0].Rows[0]["JobPollingInterval"];

               parameters[4] = new SqlParameter("@EventPollingIntterval", SqlDbType.SmallInt);
               parameters[4].Value = dsRDSEngine.Tables[0].Rows[0]["EventPollingIntterval"];

               parameters[5] = new SqlParameter("@TriggerPollingInterval", SqlDbType.SmallInt);
               parameters[5].Value = dsRDSEngine.Tables[0].Rows[0]["TriggerPollingInterval"];

               parameters[6] = new SqlParameter("@ProcessPollingInterval", SqlDbType.SmallInt);
               parameters[6].Value = dsRDSEngine.Tables[0].Rows[0]["ProcessPollingInterval"];

               parameters[7] = new SqlParameter("@StatusId", SqlDbType.SmallInt);
               parameters[7].Value = dsRDSEngine.Tables[0].Rows[0]["StatusId"];

               parameters[8] = new SqlParameter("@ProcessId", SqlDbType.Int);
               parameters[8].Value = dsRDSEngine.Tables[0].Rows[0]["ProcessId"];

               parameters[9] = new SqlParameter("@isActive", SqlDbType.Int);
               parameters[9].Value = dsRDSEngine.Tables[0].Rows[0]["isActive"];

               parameters[10] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
               parameters[10].Value = User_Id;

               parameters[11] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               parameters[11].Value = 0;
               parameters[11].Direction = ParameterDirection.InputOutput;

               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSEngineSave", parameters);
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
    }
}
