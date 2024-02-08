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
    public class RDSEventDAL
    {
        public DataSet GetRDSClientList()
        {
            DataSet dsRDSClientList = new DataSet();
            try
            {
                dsRDSClientList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetClientListByUser");
                
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsRDSClientList;
        }

        public DataSet GetSelectedEvent(int eventId)
        {
            DataSet dsEvent = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@EventId", SqlDbType.SmallInt);
                parameters[0].Value = eventId;

                dsEvent = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetSelectedEvent", parameters);
                
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEvent;
        }


        public int SaveEvent(DataSet dsEvent, string User_Id)
        {

                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@EventId", SqlDbType.SmallInt);
                parameters[0].Value = dsEvent.Tables[0].Rows[0]["EventId"];

                parameters[1] = new SqlParameter("@EventName", SqlDbType.VarChar);
                parameters[1].Value = dsEvent.Tables[0].Rows[0]["EventName"].ToString();

                parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[2].Value = dsEvent.Tables[0].Rows[0]["Description"].ToString();

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;                                                                            
                parameters[4].Direction = ParameterDirection.InputOutput;
                try
                {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSsaveEvent", parameters);
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

        public DataSet GetRDSEventList()
        {
            DataSet dsList = new DataSet();
            try
            {
                dsList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "[sp_RDSGetEventList]");
               
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsList;
        }
    }
}
