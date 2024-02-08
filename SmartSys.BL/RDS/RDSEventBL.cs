using SmartSys.DAL;
using SmartSys.DL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.RDS
{
   public class RDSEventBL
    {

       public DataSet GetRDSClientList()
       {
           DataSet dsRdsClient = new DataSet();
           try
           {
               RDSEventDAL objDAL = new RDSEventDAL();
               dsRdsClient = objDAL.GetRDSClientList();
           }
           catch (Exception ex)
           {

           }
           return dsRdsClient;


       }


       public DataSet GetSelectedEvent(int eventId)
       {
           try
           {
               RDSEventDAL objEvent = new RDSEventDAL();
               DataSet dsEvent = new DataSet();
               dsEvent = objEvent.GetSelectedEvent(eventId);
               return dsEvent;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public int SaveEvent(DataSet dsEvent, string User_Id)
       {
           try
           {
               RDSEventDAL objBL = new RDSEventDAL();
               return objBL.SaveEvent(dsEvent, User_Id);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public List<RDSEventModel> GetRDSEventList()
       {
           List<RDSEventModel> lstRDSEvent = new List<RDSEventModel>();
           try
           {
               RDSEventDAL objDAL = new RDSEventDAL();
               DataSet dsRDSEvent = new DataSet();
               dsRDSEvent = objDAL.GetRDSEventList();
               if (dsRDSEvent != null)
            {
                if (dsRDSEvent.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsRDSEvent.Tables[0].Rows)
                    {
                        RDSEventModel RDSeEventobj = new RDSEventModel();
                        RDSeEventobj.EventId = Convert.ToInt16(dr["EventId"].ToString());
                        RDSeEventobj.EventName = dr["EventName"].ToString();
                        RDSeEventobj.Description = dr["Description"].ToString();
                        RDSeEventobj.CreatedBy = 1;
                        RDSeEventobj.CreatedByName = dr["CreatedByName"].ToString();
                        RDSeEventobj.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        RDSeEventobj.ModifiedBy = 1;
                        RDSeEventobj.ModifiedByName = dr["ModifiedByName"].ToString();
                        RDSeEventobj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        lstRDSEvent.Add(RDSeEventobj);
                    }
                }
            }
               
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return lstRDSEvent;
       }
    }
}
