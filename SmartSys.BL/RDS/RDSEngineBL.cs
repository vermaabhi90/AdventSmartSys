using SmartSys.DL.RDS;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.RDS
{
   public  class RDSEngineBL
    {
       public void SetDLYGenSubStatus(Int32 DlyGenSubId, Int16 statusId)
       {
           try
           {
               RDSEngineDAL objDL = new RDSEngineDAL();
               objDL.SetDLYGenSubStatus(DlyGenSubId, statusId);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public int RDSProcessandCreateRGSJob(int DlyGenSubId)
       {
           try
           {
               RDSEngineDAL objDL = new RDSEngineDAL();
               return objDL.RDSProcessandCreateRGSJob(DlyGenSubId);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public DataSet GetJobs(Int16 RDSEngineId)
       {
           try
           {
               RDSEngineDAL objDL = new RDSEngineDAL();
               return objDL.GetJobs(RDSEngineId);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public int RegisterRDSEngine(Int16 EngineId, int ProcessId)
       {
           int errorCode = 0;
           try
           {
               RDSEngineDAL objDL = new RDSEngineDAL();
               errorCode = objDL.RegisterRDSEngine(EngineId, ProcessId);
           }
           catch (Exception ex)
           {
               throw ex;
           }
           return errorCode;
       }

       public DataSet GetDistDetails(Int32 DlyGenSubId)
       {
           try
           {
               RDSEngineDAL objDL = new RDSEngineDAL();
               return objDL.GetDistDetails(DlyGenSubId);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public DataSet GetRDSReDistributionList()
       {
           try
           {
               RDSEngineDAL objDL = new RDSEngineDAL();
               return objDL.GetRDSReDistributionList();
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public DataSet GetRDSEngineInfo(Int16 EngineId)
       {
           try
           {
               RDSEngineDAL objDL = new RDSEngineDAL();
               return objDL.RDSGetSelectedEngine(EngineId);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public int SetEngineStatus(Int16 EngineId, Int16 StatusId)
       {
           try
           {
               RDSEngineDAL objDL = new RDSEngineDAL();
               return objDL.SetEngineStatus(EngineId, StatusId);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public DataSet GetRDSEngineInfoByHost(string HostName)
       {
           DataSet dsEngine = new DataSet();
           try
           {
               RDSEngineDAL objDL = new RDSEngineDAL();
               dsEngine = objDL.GetRDSEngineInfoByHost(HostName);

           }
           catch (Exception ex)
           {
               throw ex;
           }
           return dsEngine;
       }
       public List<RDSEngineModel> RDSGetEngineList()
       {
           
           List<RDSEngineModel> lstRDSEngine = new List<RDSEngineModel>();
           try
           {

               RDSEngineDAL objDAL = new RDSEngineDAL();
               DataSet dsRDSEngine = new DataSet();
               dsRDSEngine = objDAL.RDSGetEngineList();
               if (dsRDSEngine != null)
               {
                   if (dsRDSEngine.Tables.Count > 0)
                   {
                       foreach (DataRow dr in dsRDSEngine.Tables[0].Rows)
                       {
                           RDSEngineModel rdsEngineModel = new RDSEngineModel();
                           rdsEngineModel.RDSEngineId = Convert.ToInt16(dr["RDSEngineId"].ToString());
                           rdsEngineModel.Host = dr["Host"].ToString();
                           rdsEngineModel.Port = dr["Port"].ToString();
                           rdsEngineModel.JobPollingInterval = Convert.ToInt16(dr["JobPollingInterval"].ToString());
                           rdsEngineModel.ProcessPollingInterval = Convert.ToInt16(dr["ProcessPollingInterval"].ToString());
                           rdsEngineModel.EventPollingIntterval = Convert.ToInt16(dr["EventPollingIntterval"].ToString());
                           rdsEngineModel.TriggerPollingInterval = Convert.ToInt16(dr["TriggerPollingInterval"].ToString());
                           rdsEngineModel.StatusId = Convert.ToInt16(dr["StatusId"].ToString());
                           rdsEngineModel.ProcessId = Convert.ToInt32(dr["ProcessId"].ToString());
                           rdsEngineModel.isActive = Convert.ToBoolean(dr["isActive"].ToString());                         
                           rdsEngineModel.ModifiedBy = Convert.ToInt16(dr["ModifiedBy"].ToString());
                           rdsEngineModel.ModifiedByName = dr["ModifiedByName"].ToString();
                           rdsEngineModel.StatusName = dr["StatusName"].ToString();
                           rdsEngineModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                           lstRDSEngine.Add(rdsEngineModel);
                       }
                   }
               }

           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return lstRDSEngine;
       }
       public RDSEngineModel RDSGetSelectedEngine(Int16 RDSEngineId)
       {
           RDSEngineModel rdsEngineModel = new RDSEngineModel();
           try
           {

               RDSEngineDAL objDAL = new RDSEngineDAL();
               DataSet dsRDSEngine = new DataSet();
               dsRDSEngine = objDAL.RDSGetSelectedEngine(RDSEngineId);

               if (dsRDSEngine != null)
               {
                   if (dsRDSEngine.Tables.Count > 0)
                   {
                       foreach (DataRow dr in dsRDSEngine.Tables[0].Rows)
                       {
                          
                           rdsEngineModel.RDSEngineId = Convert.ToInt16(dr["RDSEngineId"].ToString());
                           rdsEngineModel.Host = dr["Host"].ToString();
                           rdsEngineModel.Port = dr["Port"].ToString();
                           rdsEngineModel.JobPollingInterval = Convert.ToInt16(dr["JobPollingInterval"].ToString());
                           rdsEngineModel.ProcessPollingInterval = Convert.ToInt16(dr["ProcessPollingInterval"].ToString());
                           rdsEngineModel.EventPollingIntterval = Convert.ToInt16(dr["EventPollingIntterval"].ToString());
                           rdsEngineModel.TriggerPollingInterval = Convert.ToInt16(dr["TriggerPollingInterval"].ToString());
                           rdsEngineModel.StatusId = Convert.ToInt16(dr["StatusId"].ToString());
                           rdsEngineModel.ProcessId = Convert.ToInt32(dr["ProcessId"].ToString());
                           rdsEngineModel.isActive = Convert.ToBoolean(dr["isActive"].ToString());
                           rdsEngineModel.ModifiedBy = Convert.ToInt16(dr["ModifiedBy"].ToString());
                           rdsEngineModel.ModifiedByName = dr["ModifiedByName"].ToString();
                           rdsEngineModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                          
                       }
                   }
               }

           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return rdsEngineModel;
       }

       public int SaveRDSEngine(RDSEngineModel rdsEngineModel, string User_Id)
       {
           RDSEngineDAL objDAL = new RDSEngineDAL();
           DataSet dsRDSEngine = new DataSet();
           try
           {
               dsRDSEngine = objDAL.RDSGetSelectedEngine(0);
               if (dsRDSEngine != null)
               {
                   if (dsRDSEngine.Tables.Count > 0)
                   {
                       dsRDSEngine.Tables[0].Rows.Clear();
                       DataRow dr = dsRDSEngine.Tables[0].NewRow();
                       dr["RDSEngineId"] = rdsEngineModel.RDSEngineId;
                       dr["Host"] = rdsEngineModel.Host;
                       dr["Port"] = rdsEngineModel.Port;
                       dr["JobPollingInterval"] = rdsEngineModel.JobPollingInterval;
                       dr["EventPollingIntterval"] = rdsEngineModel.EventPollingIntterval;
                       dr["TriggerPollingInterval"] = rdsEngineModel.TriggerPollingInterval;
                       dr["ProcessPollingInterval"] = rdsEngineModel.ProcessPollingInterval;
                       dr["StatusId"] = rdsEngineModel.StatusId;
                       dr["ProcessId"] = rdsEngineModel.ProcessId;                     
                       dr["isActive"] = rdsEngineModel.isActive;
                       dr["ModifiedBy"] = rdsEngineModel.ModifiedBy;
                       dsRDSEngine.Tables[0].Rows.Add(dr);
                   }
               }
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return objDAL.SaveRDSEngine(dsRDSEngine, User_Id);

       }
    }
}
