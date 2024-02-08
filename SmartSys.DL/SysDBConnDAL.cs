using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL
{
   public  class SysDBConnDAL
    {

       public int SaveDBConn(DataSet dsDBConn, string User_Id)
       {

           SqlParameter[] parameters = new SqlParameter[11];
           parameters[0] = new SqlParameter("@ConnectionId", SqlDbType.SmallInt);
           parameters[0].Value = dsDBConn.Tables[0].Rows[0]["ConnectionId"];


           parameters[1] = new SqlParameter("@ServerName", SqlDbType.VarChar);
           parameters[1].Value = dsDBConn.Tables[0].Rows[0]["ServerName"].ToString();

           parameters[2] = new SqlParameter("@ConnectionType", SqlDbType.VarChar);
           parameters[2].Value = dsDBConn.Tables[0].Rows[0]["ConnectionType"].ToString();

           parameters[3] = new SqlParameter("@Port", SqlDbType.VarChar);
           parameters[3].Value = dsDBConn.Tables[0].Rows[0]["Port"].ToString();

           parameters[4] = new SqlParameter("@DBName", SqlDbType.VarChar);
           parameters[4].Value = dsDBConn.Tables[0].Rows[0]["DBName"].ToString();

           parameters[5] = new SqlParameter("@UserName", SqlDbType.VarChar);
           parameters[5].Value = dsDBConn.Tables[0].Rows[0]["UserName"].ToString();

           parameters[6] = new SqlParameter("@Password", SqlDbType.VarChar);
           parameters[6].Value = dsDBConn.Tables[0].Rows[0]["Password"].ToString();


           parameters[7] = new SqlParameter("@DefaultTimeOut", SqlDbType.VarChar);
           parameters[7].Value = dsDBConn.Tables[0].Rows[0]["DefaultTimeOut"].ToString();


           parameters[8] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
           parameters[8].Value = User_Id;

           parameters[9] = new SqlParameter("@ConName", SqlDbType.VarChar);
           parameters[9].Value = dsDBConn.Tables[0].Rows[0]["ConName"].ToString();

           parameters[10] = new SqlParameter("@ErrorCode", SqlDbType.Int);
           parameters[10].Value = 0;
           parameters[10].Direction = ParameterDirection.InputOutput;
           try
           {
               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveDBConnections", parameters);
              
           }
           catch (Exception ex)
           {
               Common.LogException(ex);            
           }
           if (parameters[10].Value != null)
               return Convert.ToInt32(parameters[10].Value.ToString());
           else
               return 0;
       }



       public DataSet GetSelectedDBConn(Int16 ConnectionId)
       {
           DataSet dsDBConn = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@ConnectionId", SqlDbType.SmallInt);
               parameters[0].Value = ConnectionId;

               dsDBConn = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDBConnectionsGetSelected", parameters);
              
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsDBConn;
       }



       public DataSet GetDBConnList(string UserId)
       {
           DataSet dsDBConn = new DataSet();
           try
           {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
               dsDBConn = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "[sp_SysGetDBConnectionList]", ObjParam);
              
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsDBConn;
       }
    }
}
