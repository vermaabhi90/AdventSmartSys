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
    public class DHLoaderDAL
    {

        public DataSet DHLoaderGetList()
        {
            DataSet dsDHLoader = new DataSet();
            try
            {
                dsDHLoader = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHLoaderGetList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsDHLoader;
        }
        public DataSet DHLoaderGetSelected(Int16 LoaderId)
        {
            DataSet dsDHLoader = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@LoaderId", SqlDbType.SmallInt);
                parameters[0].Value = LoaderId;
                dsDHLoader = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHLoaderGetSelected", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsDHLoader;
        }
        public DataSet DHLoaderTypeGetSelected()
        {
            DataSet dsDHLoader = new DataSet();
            try
            {
                dsDHLoader = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHloaderTypeList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsDHLoader;
        }


        public int SaveDHLoader(DataSet dsDHLoader)
        {

            SqlParameter[] parameters = new SqlParameter[11];
            parameters[0] = new SqlParameter("@LoaderId", SqlDbType.SmallInt);
            parameters[0].Value = dsDHLoader.Tables[0].Rows[0]["LoaderId"];

            parameters[1] = new SqlParameter("@JobPollingInterval", SqlDbType.SmallInt);
            parameters[1].Value = dsDHLoader.Tables[0].Rows[0]["JobPollingInterval"];

            parameters[2] = new SqlParameter("@EventPollingInterval", SqlDbType.SmallInt);
            parameters[2].Value = dsDHLoader.Tables[0].Rows[0]["EventPollingInterval"];

            parameters[3] = new SqlParameter("@TriggerPollingInterval", SqlDbType.SmallInt);
            parameters[3].Value = dsDHLoader.Tables[0].Rows[0]["TriggerPollingInterval"];

            parameters[4] = new SqlParameter("@StatusId", SqlDbType.SmallInt);
            parameters[4].Value = dsDHLoader.Tables[0].Rows[0]["StatusId"];

            parameters[5] = new SqlParameter("@LoaderType", SqlDbType.VarChar);
            parameters[5].Value = dsDHLoader.Tables[0].Rows[0]["LoaderType"];

            parameters[6] = new SqlParameter("@Host", SqlDbType.VarChar);
            parameters[6].Value = dsDHLoader.Tables[0].Rows[0]["Host"].ToString();

            parameters[7] = new SqlParameter("@isActive", SqlDbType.Bit);
            parameters[7].Value = dsDHLoader.Tables[0].Rows[0]["isActive"].ToString();

            parameters[8] = new SqlParameter("@Port", SqlDbType.VarChar);
            parameters[8].Value = dsDHLoader.Tables[0].Rows[0]["Port"].ToString();

            parameters[9] = new SqlParameter("@BusinessLineId", SqlDbType.SmallInt);
            parameters[9].Value = dsDHLoader.Tables[0].Rows[0]["BusinessLineId"].ToString();

          
            parameters[10] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[10].Value = 0;
            parameters[10].Direction = ParameterDirection.InputOutput;
            try
            {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHLoadertSave", parameters);
                if (parameters[10].Value != null)
                    return Convert.ToInt32(parameters[10].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region [Xls Loader]   
        public void UpdateJobStatus(int iJobID, int iErrorCode)
        {
           
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@JobID", SqlDbType.Int);
                parameters[0].Value = iJobID;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = iErrorCode;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHUpdateJobStatus", parameters);

            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        public int SaveLoaderData(string connectionStr, DataSet dsLoader)
        {
            int Errorcode = 0;
            try
            {
                string QryStr = "";
                foreach (DataRow dr in dsLoader.Tables[0].Rows)
                {
                    QryStr = "Insert into " + dsLoader.Tables[0].TableName + " ( ";

                    foreach (DataColumn c in dsLoader.Tables[0].Columns)
                    {
                        QryStr = QryStr + "[" + c.ColumnName + "],";
                    }
                    QryStr = QryStr.Substring(0, QryStr.Length - 1);
                    QryStr = QryStr + " ) Values ( ";
                    foreach (DataColumn c in dsLoader.Tables[0].Columns)
                    {
                        switch (c.DataType.ToString())
                        {
                            case "System.String":
                                QryStr = QryStr + "'" + dr[c.ColumnName] + "',";
                                break;
                            case "System.Int32":
                                int iOut = 0;
                                if (Int32.TryParse(dr[c.ColumnName].ToString(), out iOut))
                                    QryStr = QryStr + dr[c.ColumnName] + ",";
                                else
                                    QryStr = QryStr + "null" + ",";
                                break;
                            case "System.Boolean":
                                QryStr = QryStr + "'" + dr[c.ColumnName] + "',";
                                break;
                            case "System.DateTime":
                                QryStr = QryStr + "'" + dr[c.ColumnName] + "',";
                                break;
                            case "System.Decimal":
                                decimal dcOut = 0;
                                if (Decimal.TryParse(dr[c.ColumnName].ToString(), out dcOut))
                                    QryStr = QryStr + dr[c.ColumnName] + ",";
                                else
                                    QryStr = QryStr + "null" + ",";
                                break;
                            default:
                                QryStr = QryStr + dr[c.ColumnName] + ",";
                                break;
                        }
                    }
                    QryStr = QryStr.Substring(0, QryStr.Length - 1);
                    QryStr = QryStr + " )";

                    SqlHelper.ExecuteDataset(connectionStr, CommandType.Text, QryStr);
                }
            }
            catch (Exception ex)
            {
                Errorcode = 600002;
                Common.LogException(ex);
            }
            Errorcode = 600001;
            return Errorcode;
        }
        public DataSet GetDHLoaderParameter(int FeedId)
        {
            DataSet dsLoader = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@FeedId", SqlDbType.Int);
                parameters[0].Value = FeedId;
                dsLoader = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHLoaderParameterList", parameters);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsLoader;
        }
        public DataSet GetSelectedDB(Int16 FeedId, string ConType)
        {
            DataSet dsLoader = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@FeedId", SqlDbType.Int);
                parameters[0].Value = FeedId;
                parameters[1] = new SqlParameter("@ConType", SqlDbType.VarChar);
                parameters[1].Value = ConType;

                dsLoader = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHGetDBConnection", parameters);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsLoader;
        }

      public int RegisterLoder(int iLoaderID,int iProcessID)
       {
           int iErrorCode = 0;
           try
           {
               SqlParameter[] parameters = new SqlParameter[3];
               parameters[0] = new SqlParameter("@LoaderId", SqlDbType.Int);
               parameters[0].Value = iLoaderID;

               parameters[1] = new SqlParameter("@ProcessID", SqlDbType.Int);
               parameters[1].Value = iProcessID;

               parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               parameters[2].Value = iErrorCode;
               parameters[2].Direction = ParameterDirection.InputOutput;
               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHRegisterLoader", parameters);
               iErrorCode = (int)parameters[2].Value;

           }
           catch(Exception ex)
           {
               Common.LogException(ex);
           }
           return iErrorCode;
       }

        public DataSet GetJobForLoader(int iLoaderID)
        {
            int iErrorCode = 0;
            DataSet dsJobDetails = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@LoaderId", SqlDbType.Int);
                parameters[0].Value = iLoaderID;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = iErrorCode;
                
                dsJobDetails = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHGetJobForLoder", parameters);
                iErrorCode = (int)parameters[1].Value;
               
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsJobDetails;
        }

        public DataSet GetDataHubLoaderByHost(string HostName)
        {
            DataSet dsHostDetails = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@HostName", SqlDbType.VarChar);
                parameters[0].Value = HostName;

                dsHostDetails = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHLoaderGetSelectedByHost", parameters);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsHostDetails;
        }

        public int SetLoaderStatus(int LoaderId, int StatusId)
        {
            DataSet dsHostDetails = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@HostName", SqlDbType.Int);
                parameters[0].Value = LoaderId;

                parameters[1] = new SqlParameter("@HostName", SqlDbType.Int);
                parameters[1].Value = StatusId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHSetLoaderStatus", parameters);

                return Convert.ToInt32(parameters[2].Value);


            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }
        #endregion
    }  
}
