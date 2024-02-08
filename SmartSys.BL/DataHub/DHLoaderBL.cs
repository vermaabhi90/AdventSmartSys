using SmartSys.DL.DataHub;
using SmartSys.DL.RGSGen;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.DataHub
{
   public class DHLoaderBL
    {
       public int RegisterLoder(int iLoaderID,int iProcessID)
       {
           int iErrorCode = 0;
           try
           {
               DHLoaderDAL dhObjDAL = new DHLoaderDAL();
               iErrorCode = dhObjDAL.RegisterLoder(iLoaderID, iProcessID);
           }
           catch(Exception ex)
           {
               Common.LogException(ex);
           }
           return iErrorCode;
       }
       public DataSet GetJobForLoader(int iLoaderID)
       {
           DataSet  dsJobDetails  = new DataSet();
           try
           {
               DHLoaderDAL objDAL = new DHLoaderDAL();
               dsJobDetails = objDAL.GetJobForLoader(iLoaderID);
               
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsJobDetails;
       }

       public DataSet GetDataHubLoaderByHost(string HostName)
       {
           DataSet dsLoaderDetails = new DataSet();
           try
           {
               DHLoaderDAL objDAL = new DHLoaderDAL();
               dsLoaderDetails = objDAL.GetDataHubLoaderByHost(HostName);

           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsLoaderDetails;
       }

       public int SetLoaderStatus(int LoaderId, int StatusId)
       {
           
           try
           {
               DHLoaderDAL objDAL = new DHLoaderDAL();
               return objDAL.SetLoaderStatus(LoaderId, StatusId);

           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return 0;
       }


       public List<LoaderTypeModel> DHLoaderTypeGetSelected()
       {
           List<LoaderTypeModel> lstDHLoader = new List<LoaderTypeModel>();
           try
           {
               DHLoaderDAL objDAL = new DHLoaderDAL();
               DataSet dsDHLoader = new DataSet();
               dsDHLoader = objDAL.DHLoaderTypeGetSelected();
                  if (dsDHLoader == null )
                        return null;
                   if (dsDHLoader.Tables.Count > 0)
                   {
                       if (dsDHLoader.Tables[0].Rows.Count > 0)
                       {
                           foreach (DataRow dr in dsDHLoader.Tables[0].Rows)
                           {
                               LoaderTypeModel LoaderTypeModel = new LoaderTypeModel();
                               LoaderTypeModel.LoaderType = dr["FeedTypeCode"].ToString();
                               LoaderTypeModel.LoaderValue = dr["FeedTypeCode1"].ToString();
                               lstDHLoader.Add(LoaderTypeModel);
                           }
                       }
                   }                       
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return lstDHLoader;
       }

       public List<DHLoaderModel> DHLoaderGetList()
       {
           List<DHLoaderModel> lstDHLoader = new List<DHLoaderModel>();
           try
           {
               DHLoaderDAL objDAL = new DHLoaderDAL();
               DataSet dsDHLoader = new DataSet();
               dsDHLoader = objDAL.DHLoaderGetList();
               if (dsDHLoader != null)
               {
                   if (dsDHLoader.Tables.Count > 0)
                   {
                       if (dsDHLoader.Tables[0].Rows.Count > 0)
                       {
                           foreach (DataRow dr in dsDHLoader.Tables[0].Rows)
                           {
                               DHLoaderModel dhLoaderModel = new DHLoaderModel();
                               dhLoaderModel.LoaderId = Convert.ToInt16(dr["LoaderId"].ToString());
                               dhLoaderModel.JobPollingInterval = Convert.ToInt16(dr["JobPollingInterval"].ToString());
                               dhLoaderModel.EventPollingInterval = Convert.ToInt16(dr["EventPollingInterval"].ToString());
                               dhLoaderModel.TriggerPollingInterval = Convert.ToInt16(dr["TriggerPollingInterval"].ToString());
                               dhLoaderModel.BusinessLineId = Convert.ToInt16(dr["BusinessLineId"].ToString());
                               dhLoaderModel.StatusId = Convert.ToInt16(dr["StatusId"].ToString());
                               dhLoaderModel.isActive = Convert.ToBoolean(dr["isActive"].ToString());
                               dhLoaderModel.Port = dr["Port"].ToString();
                               dhLoaderModel.Host = dr["Host"].ToString();
                               dhLoaderModel.StatusShortCode = dr["StatusShortCode"].ToString();
                               dhLoaderModel.BusinessLineName = dr["BusinessLineName"].ToString();
                               dhLoaderModel.LoaderType = dr["LoaderType"].ToString();
                               lstDHLoader.Add(dhLoaderModel);
                           }
                       }
                   }
               }

           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return (lstDHLoader);

       }

       public DHLoaderModel DHLoaderGetSelected(Int16 LoaderId)
       {
           DHLoaderModel dhLoaderModel = new DHLoaderModel();
           try
           {
               DHLoaderDAL objDAL = new DHLoaderDAL();
               DataSet dsDHLoader = new DataSet();
               dsDHLoader = objDAL.DHLoaderGetSelected(LoaderId);
               if (dsDHLoader != null)
               {
                   if (dsDHLoader.Tables.Count > 0)
                   {
                       if (dsDHLoader.Tables[0].Rows.Count > 0)
                       {                          
                           dhLoaderModel.LoaderId = Convert.ToInt16(dsDHLoader.Tables[0].Rows[0]["LoaderId"].ToString());
                           dhLoaderModel.JobPollingInterval = Convert.ToInt16(dsDHLoader.Tables[0].Rows[0]["JobPollingInterval"].ToString());
                           dhLoaderModel.EventPollingInterval = Convert.ToInt16(dsDHLoader.Tables[0].Rows[0]["EventPollingInterval"].ToString());
                           dhLoaderModel.TriggerPollingInterval = Convert.ToInt16(dsDHLoader.Tables[0].Rows[0]["TriggerPollingInterval"].ToString());
                           dhLoaderModel.StatusId = Convert.ToInt16(dsDHLoader.Tables[0].Rows[0]["StatusId"].ToString());
                           dhLoaderModel.isActive = Convert.ToBoolean(dsDHLoader.Tables[0].Rows[0]["isActive"].ToString());
                           dhLoaderModel.Port = dsDHLoader.Tables[0].Rows[0]["Port"].ToString();
                           dhLoaderModel.BusinessLineId = Convert.ToInt16( dsDHLoader.Tables[0].Rows[0]["BusinessLineId"].ToString());
                           dhLoaderModel.Host = dsDHLoader.Tables[0].Rows[0]["Host"].ToString();
                           dhLoaderModel.LoaderType = dsDHLoader.Tables[0].Rows[0]["LoaderType"].ToString();
                           dhLoaderModel.ProcessID = Convert.ToInt32(dsDHLoader.Tables[0].Rows[0]["ProcessID"].ToString());
                       }
                   }
               }

           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return (dhLoaderModel);

       }
       public int SaveDHLoader(DHLoaderModel dhLoaderModel)
       {
           DHLoaderDAL objDAL = new DHLoaderDAL();
           DataSet dsDHLoader = new DataSet();
               try
               {
                   dsDHLoader = objDAL.DHLoaderGetSelected(0);
                   if (dsDHLoader != null)
                   {
                       if (dsDHLoader.Tables.Count > 0)
                       {
                           dsDHLoader.Tables[0].Rows.Clear();
                           DataRow dr = dsDHLoader.Tables[0].NewRow();
                           dr["LoaderId"] = dhLoaderModel.LoaderId;
                           dr["JobPollingInterval"] = dhLoaderModel.JobPollingInterval;
                           dr["EventPollingInterval"] = dhLoaderModel.EventPollingInterval;
                           dr["TriggerPollingInterval"] = dhLoaderModel.TriggerPollingInterval;
                           dr["StatusId"] = dhLoaderModel.StatusId;
                           dr["Host"] = dhLoaderModel.Host;
                           dr["LoaderType"] = dhLoaderModel.LoaderType;
                           dr["Port"] = dhLoaderModel.Port;
                           dr["isActive"] = dhLoaderModel.isActive;
                           dr["BusinessLineId"] = dhLoaderModel.BusinessLineId;
                           dsDHLoader.Tables[0].Rows.Add(dr);
                       }
                   }
               }
               catch (Exception ex)
               {
                   Common.LogException(ex);
               }
               return objDAL.SaveDHLoader(dsDHLoader);

       }

        #region [Xls Loader]
       public void UpdateJobStatus(int iJobID, int iErrorCode)
       {
           try
           {
               DHLoaderDAL objDal = new DHLoaderDAL();
               objDal.UpdateJobStatus(iJobID, iErrorCode);
           }
           catch (Exception ex)
           {
                throw ex;
           }

       }
       public int SaveLoaderData(string connectionStr, DataSet dsLoader)
       {
           int errorcode = 0;
           try
           {
               DHLoaderDAL objDal = new DHLoaderDAL();
               errorcode = objDal.SaveLoaderData(connectionStr, dsLoader);
           }
           catch (Exception ex)
           {

               Common.LogException(ex);
           }
           return errorcode;
       }
       public DataSet GetDHLoaderParameter(int FeedId)
       {
           DataSet dsObj = new DataSet();
           try
           {
               DHLoaderDAL objDal = new DHLoaderDAL();
               dsObj = objDal.GetDHLoaderParameter(FeedId);
           }
           catch (Exception ex)
           {

               Common.LogException(ex);
           }
           return dsObj;
       }
       public ConnectionModel GetConnection(Int16 FeedId, string conType)
       {
           ConnectionModel ConnObj = new ConnectionModel();
           try
           {

               DataSet dsObj = new DataSet();
               DHLoaderDAL objDal = new DHLoaderDAL();
               dsObj = objDal.GetSelectedDB(FeedId, conType);
               if (dsObj != null)
               {
                   if (dsObj.Tables.Count > 0)
                   {
                       ConnObj.ConnectionId = Convert.ToInt16(dsObj.Tables[0].Rows[0]["ConnectionId"]);
                       ConnObj.ConnectionType = dsObj.Tables[0].Rows[0]["ConnectionType"].ToString();
                       ConnObj.DBName = dsObj.Tables[0].Rows[0]["DBName"].ToString();
                       ConnObj.ServerName = dsObj.Tables[0].Rows[0]["ServerName"].ToString();
                       ConnObj.Port = dsObj.Tables[0].Rows[0]["Port"].ToString();
                       ConnObj.UserName = dsObj.Tables[0].Rows[0]["UserName"].ToString();
                       ConnObj.Password = dsObj.Tables[0].Rows[0]["Password"].ToString();
                   }
               }
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return ConnObj;
       }

        #endregion
    }
}
