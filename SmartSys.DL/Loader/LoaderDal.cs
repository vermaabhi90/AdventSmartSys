using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.Loader
{
    public class LoaderDal
    {
        

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

        public DataSet GetDHLoaderParameter(int FeedId)
        {
            DataSet dsLoader = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@FeedId", SqlDbType.Int);
                parameters[0].Value = FeedId;
                dsLoader = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHLoaderParameterList",parameters);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsLoader;
        }
        public DataSet GetDHLoaderActionParameter(int FActionId)
        {
            DataSet dsLoader = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@FActionId", SqlDbType.Int);
                parameters[0].Value = FActionId;
                dsLoader = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHLoaderActionParameterList", parameters);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsLoader;
        }
        public DataSet DHFeedMasterList(string User_Id)
        {
            DataSet dsFeedMast = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                dsFeedMast = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHFeedMasterGetList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                throw;
            }
            return dsFeedMast;
        }
        public int SaveLoaderData(string connectionStr, DataSet dsLoader, string proce, string User_Id,string BunchId)
        {
            int Errorcode = 0;

            try
            {
                if (proce.ToString() == "")
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
                                    QryStr = QryStr + "'" + dr[c.ColumnName].ToString().Replace("'", "''") + "',";
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
                                    QryStr = QryStr + "'" + dr[c.ColumnName] + "',";
                                    break;
                            }
                        }
                        QryStr = QryStr.Substring(0, QryStr.Length - 1);
                        QryStr = QryStr + " )";


                        SqlHelper.ExecuteDataset(connectionStr, CommandType.Text, QryStr);
                    }
                }
                else
                {
                    int colNo = dsLoader.Tables[0].Columns.Count;
                    int x = -1;
                    SqlParameter[] parameters = new SqlParameter[colNo + 2];

                    foreach (DataRow dr in dsLoader.Tables[0].Rows)
                    {
                        x = -1;
                        foreach (DataColumn c in dsLoader.Tables[0].Columns)
                        {
                            ++x;
                            switch (c.DataType.ToString())
                            {

                                case "System.String":

                                    parameters[x] = new SqlParameter("@" + c.ColumnName, SqlDbType.VarChar);
                                    parameters[x].Value = dr[c.ColumnName];
                                    break;
                                case "System.Int32":
                                    int iOut = 0;
                                    if (Int32.TryParse(dr[c.ColumnName].ToString(), out iOut))
                                    {
                                        parameters[x] = new SqlParameter("@" + c.ColumnName, SqlDbType.Int);
                                        parameters[x].Value = dr[c.ColumnName];
                                    }
                                    else
                                    {
                                        parameters[x] = new SqlParameter("@" + c.ColumnName, SqlDbType.Int);
                                        parameters[x].Value = "";
                                    }
                                    break;
                                case "System.Boolean":
                                    parameters[x] = new SqlParameter("@" + c.ColumnName, SqlDbType.Bit);
                                    parameters[x].Value = dr[c.ColumnName];
                                    break;
                                case "System.DateTime":
                                    parameters[x] = new SqlParameter("@" + c.ColumnName, SqlDbType.DateTime);
                                    parameters[x].Value = dr[c.ColumnName];
                                    break;
                                case "System.Decimal":
                                    decimal dcOut = 0;
                                    if (Decimal.TryParse(dr[c.ColumnName].ToString(), out dcOut))
                                    {
                                        parameters[x] = new SqlParameter("@" + c.ColumnName, SqlDbType.Decimal);
                                        parameters[x].Value = dr[c.ColumnName];
                                    }
                                    else
                                    {
                                        parameters[x] = new SqlParameter("@" + c.ColumnName, SqlDbType.Decimal);
                                        parameters[x].Value = "";
                                    }
                                    break;
                            }
                        }
                        parameters[x + 1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                        parameters[x + 1].Value = User_Id;                                    
                        SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, proce, parameters);
                    }

                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return Errorcode = 600002;               
            }
            Errorcode = 600001;
            return Errorcode;
        }
    }


}
