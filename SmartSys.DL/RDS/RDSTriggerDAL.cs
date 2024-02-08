using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.RDS
{
   public  class RDSTriggerDAL
    {
        public DataSet GetRDSTriggerList()
        {
            DataSet dsRDSTrigger = new DataSet();
            try
            {
                dsRDSTrigger = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetTriggerList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsRDSTrigger;
        }
        public DataSet RDSTriggerGetSelected(Int16 TriggerId)
        {
            DataSet dsRDSTrigger = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@TriggerId", SqlDbType.SmallInt);
                parameters[0].Value = TriggerId;
                dsRDSTrigger = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetSelectedTriggerList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsRDSTrigger;
        }
        public int SaveRDSTrigger(DataSet dsRDSTrigg, string User_Id)
        {
            try
            {
                
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@TriggerId", SqlDbType.SmallInt);
                parameters[0].Value = dsRDSTrigg.Tables[0].Rows[0]["TriggerId"];

                parameters[1] = new SqlParameter("@TriggerName", SqlDbType.VarChar);
                parameters[1].Value = dsRDSTrigg.Tables[0].Rows[0]["TriggerName"].ToString();

                parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[2].Value = dsRDSTrigg.Tables[0].Rows[0]["Description"].ToString();

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSSaveTrigger", parameters);
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
