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
    public class RDSClientDAL
    {
        public DataSet RDSGetClientList()
        {

            DataSet dsObj = new DataSet();
            try
            {
                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetClientList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsObj;
        }
        public DataSet RDSGetSelectedClient(int ClientId)
        {
            DataSet dsObj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ClientId", SqlDbType.SmallInt);
                parameters[0].Value = ClientId;
                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetSelectedClient", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsObj;
        }
        public int SaveRDSClient(DataSet dsRDSClient, string User_Id)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[12];
                parameters[0] = new SqlParameter("@ClientId", SqlDbType.Int);
                parameters[0].Value = dsRDSClient.Tables[0].Rows[0]["ClientId"];

                parameters[1] = new SqlParameter("@ClientName", SqlDbType.VarChar);
                parameters[1].Value = dsRDSClient.Tables[0].Rows[0]["ClientName"].ToString();

                parameters[2] = new SqlParameter("@ClientType", SqlDbType.VarChar);
                parameters[2].Value = dsRDSClient.Tables[0].Rows[0]["ClientType"].ToString();

                parameters[3] = new SqlParameter("@ClientRefId", SqlDbType.VarChar);
                parameters[3].Value = dsRDSClient.Tables[0].Rows[0]["ClientRefId"];

                parameters[4] = new SqlParameter("@email", SqlDbType.VarChar);
                parameters[4].Value = dsRDSClient.Tables[0].Rows[0]["email"];

                parameters[5] = new SqlParameter("@FTPDetails", SqlDbType.VarChar);
                parameters[5].Value = dsRDSClient.Tables[0].Rows[0]["FTPDetails"];

                parameters[6] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[6].Value = User_Id;

                parameters[7] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[7].Value = 0;
                parameters[7].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSSaveClient", parameters);
                if (parameters[7].Value != null)
                    return Convert.ToInt32(parameters[7].Value.ToString());
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
