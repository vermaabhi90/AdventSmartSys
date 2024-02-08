using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.RGSInsMgr
{
   public  class RGSInsMgrDAL
    {
       public int SaveInstMgr(DataSet dsEvent, string User_Id)
       {

           SqlParameter[] parameters = new SqlParameter[9];
           parameters[0] = new SqlParameter("@IMId", SqlDbType.SmallInt);
           parameters[0].Value = dsEvent.Tables[0].Rows[0]["IMId"];

           parameters[1] = new SqlParameter("@Host", SqlDbType.VarChar);
           parameters[1].Value = dsEvent.Tables[0].Rows[0]["Host"].ToString();

           parameters[2] = new SqlParameter("@isPrimary", SqlDbType.Bit);
           parameters[2].Value = dsEvent.Tables[0].Rows[0]["isPrimary"].ToString();

           parameters[3] = new SqlParameter("@Port", SqlDbType.VarChar);
           parameters[3].Value = dsEvent.Tables[0].Rows[0]["Port"].ToString();

           parameters[4] = new SqlParameter("@PollingInterval", SqlDbType.Int);
           parameters[4].Value = dsEvent.Tables[0].Rows[0]["PollingInterval"].ToString();

           parameters[5] = new SqlParameter("@ProcessId", SqlDbType.Int);
           parameters[5].Value = dsEvent.Tables[0].Rows[0]["ProcessId"].ToString();


           parameters[6] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
           parameters[6].Value = User_Id;

           parameters[7] = new SqlParameter("@StatusId", SqlDbType.SmallInt);
           parameters[7].Value =Convert.ToInt16(0);


           parameters[8] = new SqlParameter("@ErrorCode", SqlDbType.Int);
           parameters[8].Value = 0;
           parameters[8].Direction = ParameterDirection.InputOutput;
           try
           {
               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSSaveInstanceManager", parameters);
              
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           if (parameters[8].Value != null)
               return Convert.ToInt32(parameters[8].Value.ToString());
           else
               return 0;
       }


       public DataSet GetInstMgrList()
       {
           DataSet dsList = new DataSet();
           try
           {
               dsList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetInstanceManagertList");
               return dsList;
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsList;
       }

       public DataSet InstMgrGetSelected(int IMId)
       {
           DataSet dsEvent = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@IMId", SqlDbType.SmallInt);
               parameters[0].Value = IMId;

               dsEvent = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetSelectedInstanceManagerList", parameters);
              
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsEvent;
       }

    }
}
