using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.DataHub
{
   public class DHTriggerDAL
    {
       public DataSet GetDHTriggerList()
       {
           DataSet dsDHTrigger = new DataSet();
           try
           {
               dsDHTrigger = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHGetTriggerList");
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsDHTrigger;
       }
       public DataSet DHTriggerGetSelected(Int16 TriggerId)
       {
           DataSet dsDHTrigger = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@TriggerId", SqlDbType.SmallInt);
               parameters[0].Value = TriggerId;
               dsDHTrigger = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHGetSelectedTriggerList", parameters);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsDHTrigger;
       }
       public int SaveDHTrigger(DataSet dsDHTrigg, string User_Id)
       {
           try
           {

               SqlParameter[] parameters = new SqlParameter[5];
               parameters[0] = new SqlParameter("@TriggerId", SqlDbType.SmallInt);
               parameters[0].Value = dsDHTrigg.Tables[0].Rows[0]["TriggerId"];

               parameters[1] = new SqlParameter("@TriggerName", SqlDbType.VarChar);
               parameters[1].Value = dsDHTrigg.Tables[0].Rows[0]["TriggerName"].ToString();

               parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
               parameters[2].Value = dsDHTrigg.Tables[0].Rows[0]["Description"].ToString();

               parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
               parameters[3].Value = User_Id;

               parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               parameters[4].Value = 0;
               parameters[4].Direction = ParameterDirection.InputOutput;

               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHSaveTrigger", parameters);
               if (parameters[4].Value != null)
                   return Convert.ToInt32(parameters[4].Value.ToString());
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
