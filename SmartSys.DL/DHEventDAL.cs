using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SmartSys.Utility;
using System.Data.SqlClient;

namespace SmartSys.DAL
{
    public class DHEventDAL
    {
        public DataSet GetDHEventList()
        {
            DataSet dsList = new DataSet();
            try
            {
                dsList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHEventGetList");               
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsList;
        }

        public DataSet DHEventGetSelected(int eventId)
        {
            DataSet dsEvent = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@EventId", SqlDbType.SmallInt);
                parameters[0].Value = eventId;
                dsEvent = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHEventGetSelected", parameters);             
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEvent;
        }
        public int SaveDHEvent(DataSet dsEvent)
        {
            try
            {
                
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@EventId", SqlDbType.SmallInt);
                parameters[0].Value = dsEvent.Tables[0].Rows[0]["EventId"];

                parameters[1] = new SqlParameter("@EventName", SqlDbType.VarChar);
                parameters[1].Value = dsEvent.Tables[0].Rows[0]["EventName"].ToString();

                parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[2].Value = dsEvent.Tables[0].Rows[0]["Description"].ToString();

                parameters[3] = new SqlParameter("@isDeleted", SqlDbType.Bit);
                parameters[3].Value = dsEvent.Tables[0].Rows[0]["isDeleted"];

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHEventSave", parameters);
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
