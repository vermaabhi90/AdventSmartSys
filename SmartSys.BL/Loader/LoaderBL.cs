using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSys.DL.Loader;
using System.Data;
using SmartSys.Utility;

namespace SmartSys.DL.Loader
{
   public class LoaderBL
    {
       
       

       public ConnectionModel GetConnection(Int16 FeedId, string conType)
       {
           ConnectionModel ConnObj = new ConnectionModel();
           try
           {
          
           DataSet dsObj = new DataSet();
           LoaderDal objDal=new LoaderDal();
           dsObj = objDal.GetSelectedDB(FeedId, conType);
           if(dsObj !=null)
           {
               if(dsObj.Tables.Count > 0)
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
       public int SaveLoaderData(string connectionStr,DataSet dsLoader,string proce,string User_ID,string BunchId)
       {
          int errorcode= 0;
           try
           {
               LoaderDal objDal = new LoaderDal();
               errorcode = objDal.SaveLoaderData(connectionStr, dsLoader, proce, User_ID, BunchId);
           }
           catch (Exception ex) 
           {

               Common.LogException(ex);
           }
           return errorcode;
       }

       public List<FeedMasterModel> DHFeedMasterList(string User_Id)
       {
           List<FeedMasterModel> lstFeedMast = new List<FeedMasterModel>();
           try
           {
               DataSet dsFeedMast = new DataSet();
               LoaderDal DalObj = new LoaderDal();
               dsFeedMast = DalObj.DHFeedMasterList(User_Id);
               if(dsFeedMast != null)
               {
                   if(dsFeedMast.Tables[0].Rows.Count > 0)
                   {
                       foreach(DataRow dr in dsFeedMast.Tables[0].Rows)
                       {
                           FeedMasterModel FeedObj = new FeedMasterModel();
                           FeedObj.FeedId = Convert.ToInt32(dr["FeedId"]);
                           FeedObj.FeedName = dr["FeedName"].ToString();
                           lstFeedMast.Add(FeedObj);
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return lstFeedMast;
       }
       public DataSet GetDHActionParameter(int FActionID)
       {
           DataSet dsObj = new DataSet();
           try
           {
               LoaderDal objDal = new LoaderDal();
               dsObj = objDal.GetDHLoaderActionParameter(FActionID);
           }
           catch (Exception ex)
           {

               Common.LogException(ex);
           }
           return dsObj;
       }
       public DataSet GetDHLoaderParameter(int FeedId)
       {
           DataSet dsObj = new DataSet();
           try
           {
               LoaderDal objDal = new LoaderDal();
               dsObj = objDal.GetDHLoaderParameter(FeedId);
           }
           catch (Exception ex)
           {

               Common.LogException(ex);
           }
           return dsObj;
       }
      

    }
}
