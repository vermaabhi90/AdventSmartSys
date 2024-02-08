using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.RGS
{
    public class InstanceManagerDAL
    {
        public DataSet GetInstanceManagerByHost(string HostName)
        {
            DataSet dsIM = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@HostName", SqlDbType.VarChar);
                objParam[0].Value = HostName;
                dsIM = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetInstanceManagerByHost", objParam);
                return dsIM;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public int SetInstanceManagerProcessId(int IMId, int ProcessId)
        {
            int ErrorCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@IMId", SqlDbType.Int);
                objParam[0].Value = IMId;
                objParam[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
                objParam[1].Value = ProcessId; 
                objParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[2].Value = 0;
                objParam[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSSetInstanceManagerProcessId", objParam);
                if (objParam[2].Value != null)
                    ErrorCode = Convert.ToInt32(objParam[2].Value);
                return ErrorCode;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
