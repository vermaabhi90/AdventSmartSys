using SmartSys.DAL;
using SmartSys.DL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.SysDBCon
{
   public class SysDBConBL
    {


       public int SaveDBConn(SysDBConModel ConnModel, string User_Id)
       {
           SysDBConnDAL objDAL = new SysDBConnDAL();
           DataSet dsDBConn = new DataSet();
           try
           {           
              dsDBConn = objDAL.GetSelectedDBConn( Convert.ToInt16(0));
              if (dsDBConn != null)
              {
                  if (dsDBConn.Tables.Count > 0)
                  {
                      dsDBConn.Tables[0].Rows.Clear();

                      DataRow dr = dsDBConn.Tables[0].NewRow();

                      dr["ConnectionId"] = ConnModel.ConnectionId;
                      dr["ConName"] = ConnModel.ConName;
                      dr["ServerName"] = ConnModel.ServerName;
                      dr["ConnectionType"] = ConnModel.ConnectionType;
                      dr["Port"] = ConnModel.Port;
                      dr["DBName"] = ConnModel.DBName;
                      dr["UserName"] = ConnModel.UserName;
                      dr["Password"] = ConnModel.Password;
                      dr["DefaultTimeOut"] = ConnModel.DefaultTimeOut;
                      dr["ModifiedBy"] = ConnModel.ModifiedBy;
                      dsDBConn.Tables[0].Rows.Add(dr);
                  }
              }

           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return objDAL.SaveDBConn(dsDBConn, User_Id);
       }

       public SysDBConModel GetSelectedDBConn(Int16 ConnectionId)
       {
           DataSet dsDBConn = new DataSet();
           SysDBConModel dbConnModel = new SysDBConModel();
           try
           {
               SysDBConnDAL objDAL = new SysDBConnDAL();
               dsDBConn = objDAL.GetSelectedDBConn(ConnectionId);

               if (dsDBConn != null)
               {
                   if (dsDBConn.Tables.Count > 0)
                   {
                       if (dsDBConn.Tables[0].Rows.Count > 0)
                       {
                           dbConnModel.ConnectionId = ConnectionId;
                           dbConnModel.ServerName = dsDBConn.Tables[0].Rows[0]["ServerName"].ToString();
                           dbConnModel.ConnectionType = dsDBConn.Tables[0].Rows[0]["ConnectionType"].ToString();
                           dbConnModel.Port = dsDBConn.Tables[0].Rows[0]["Port"].ToString();

                           dbConnModel.DBName = dsDBConn.Tables[0].Rows[0]["DBName"].ToString();
                           dbConnModel.UserName = dsDBConn.Tables[0].Rows[0]["UserName"].ToString();

                           dbConnModel.Password = dsDBConn.Tables[0].Rows[0]["Password"].ToString();
                           dbConnModel.DefaultTimeOut = Convert.ToInt32(dsDBConn.Tables[0].Rows[0]["DefaultTimeOut"].ToString());

                           dbConnModel.CreatedByName = dsDBConn.Tables[0].Rows[0]["CreatedByName"].ToString();
                           dbConnModel.ModifiedByName = dsDBConn.Tables[0].Rows[0]["ModifiedByName"].ToString();
                           dbConnModel.CreatedDate = Convert.ToDateTime(dsDBConn.Tables[0].Rows[0]["CreatedDate"].ToString());
                           dbConnModel.ModifiedDate = Convert.ToDateTime(dsDBConn.Tables[0].Rows[0]["ModifiedDate"].ToString());
                           dbConnModel.ConName = dsDBConn.Tables[0].Rows[0]["ConName"].ToString();

                       }
                   }
               }
          
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dbConnModel;
       }


       public List<SysDBConModel> GetDBConnList(string UserId)
       {
           List<SysDBConModel> lstBDConnection = new List<SysDBConModel>();
           try
           {
               SysDBConnDAL objEvent = new SysDBConnDAL();
               DataSet dsBDConnection = new DataSet();
               dsBDConnection = objEvent.GetDBConnList(UserId);

               if (dsBDConnection != null)
               {
                   if (dsBDConnection.Tables.Count > 0)
                   {
                       foreach (DataRow dr in dsBDConnection.Tables[0].Rows)
                       {
                           SysDBConModel dbconn = new SysDBConModel();

                           dbconn.ConnectionId = Convert.ToInt16(dr["ConnectionId"].ToString());
                           dbconn.ServerName = dr["ServerName"].ToString();
                           dbconn.Port = dr["Port"].ToString();
                           dbconn.DBName = dr["DBName"].ToString();
                           dbconn.UserName = dr["UserName"].ToString();
                           dbconn.Password = dr["Password"].ToString();
                           dbconn.DefaultTimeOut = Convert.ToInt32(dr["DefaultTimeOut"].ToString());
                           dbconn.ConnectionType = dr["ConnectionType"].ToString();
                           dbconn.CreatedByName = dr["CreatedByName"].ToString();
                           dbconn.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                           dbconn.ModifiedByName = dr["ModifiedByName"].ToString();
                           dbconn.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                           dbconn.ConName = dr["ConName"].ToString();

                           lstBDConnection.Add(dbconn);
                       }
                   }
               }
               
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return lstBDConnection;
       }
    }
}
