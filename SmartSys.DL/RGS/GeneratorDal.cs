using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SmartSys.Utility;
using SmartSys.DAL;
using System.Collections;
using MySql.Data.MySqlClient;
 
namespace SmartSys.DL.RGS
{
    public class GeneratorDal
    {

        public int GetJobStatus(int JobId)
        {
            int StatusId = 0;
            try
            {

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@JobId", SqlDbType.Int);
                parameters[0].Value = JobId;
                DataSet dsJob = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetJobStatus", parameters);
                if (dsJob.Tables.Count > 0)
                    if (dsJob.Tables[0].Rows.Count > 0)
                        StatusId = Convert.ToInt32(dsJob.Tables[0].Rows[0]["StatusId"].ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return StatusId;
        }

        /// <summary>
        /// This method will execute Stored procedure to receive the generator details.
        /// </summary>
        /// <param name="GenId">Integer Generator ID for which the details are requested</param>
        /// <returns>Single Generator Row</returns>
        public DataSet GetGeneratorDetails(int GenId)
        {
            DataSet dsGenDetails = new DataSet();
            try
            {
                SqlParameter[] GenParam = new SqlParameter[1];
                GenParam[0] = new SqlParameter("@GenId", SqlDbType.Int);
                GenParam[0].Value = GenId;
                dsGenDetails = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetGeneratorDetails", GenParam);
                return dsGenDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method will execute Stored procedure to receive the generator details.
        /// </summary>
        /// <param name="HostName">Host Name for which generator List is required</param>
        /// <returns>Multiple Generator Row</returns>
        public DataSet GetGeneratorDetails(string HostName)
        {
            DataSet dsGenDetails = new DataSet();
            try
            {
                SqlParameter[] GenParam = new SqlParameter[1];
                GenParam[0] = new SqlParameter("@HostName", SqlDbType.VarChar);
                GenParam[0].Value = HostName;
                dsGenDetails = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetGeneratorByHost", GenParam);
                return dsGenDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        
        /// <summary>
        /// This method will Set the generator Status as required.
        /// </summary>
        /// <param name="GenId">generator Id to change the status</param>
        /// <param name="StatusId">Status Id to change the status of subject generator</param>
        /// <returns>Multiple Generator Row</returns>
        public int SetGeneratorStatus(Int16 GenId, Int16 StatusId)
        {
            int ErrorCode = 0;
            try
            {
                SqlParameter[] GenParam = new SqlParameter[3];
                GenParam[0] = new SqlParameter("@GenId", SqlDbType.Int);
                GenParam[0].Value = GenId;
                GenParam[1] = new SqlParameter("@StatusId", SqlDbType.SmallInt);
                GenParam[1].Value = StatusId;
                GenParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                GenParam[2].Value = 0;
                GenParam[2].Direction = ParameterDirection.InputOutput;

                

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSSetGeneratorStatus", GenParam);
                if (GenParam[2].Value != null)
                    ErrorCode = Convert.ToInt32(GenParam[2].Value);
                return ErrorCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This method will Set the Job Status as required.
        /// </summary>
        /// <param name="JobId">generator Id to change the status</param>
        /// <param name="StatusId">Status Id to change the status of subject generator</param>
        /// <returns>Multiple Generator Row</returns>
        public int SetJobStatus(int JobId, Int16 StatusId, string desc, int genId, string opFileName, string opLocation, int opSize)
        {
            int ErrorCode = 0;
            try
            {
                SqlParameter[] GenParam = new SqlParameter[9];
                GenParam[0] = new SqlParameter("@JobId", SqlDbType.Int);
                GenParam[0].Value = JobId;
                GenParam[1] = new SqlParameter("@StatusId", SqlDbType.SmallInt);
                GenParam[1].Value = StatusId;
                GenParam[2] = new SqlParameter("@GenId", SqlDbType.SmallInt);
                GenParam[2].Value = genId;
                GenParam[3] = new SqlParameter("@Description", SqlDbType.NVarChar);
                GenParam[3].Value = desc;
                GenParam[4] = new SqlParameter("@OutputFileName", SqlDbType.NVarChar);
                GenParam[4].Value = opFileName;
                GenParam[5] = new SqlParameter("@OutputLocation", SqlDbType.NVarChar);
                GenParam[5].Value = opLocation;
                GenParam[6] = new SqlParameter("@OutputSize", SqlDbType.Int);
                GenParam[6].Value = opSize;
                GenParam[7] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                GenParam[7].Value = 0;
                GenParam[7].Direction = ParameterDirection.InputOutput;



                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSSetJobStatus", GenParam);
                if (GenParam[2].Value != null)
                    ErrorCode = Convert.ToInt32(GenParam[7].Value);
                return ErrorCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method will call the storedprocedure to set the new ProcessId to the generator config
        /// </summary>
        /// <param name="genId"> Generator Id to update</param>
        /// <param name="processId"> New Process Id which will be allocated to Generator </param>
        /// <returns> Thie Process returns Error Codes. 600003 represents Generator ID not exist, 600001 Represents Successful updation</returns>
        public int RegisterGenerator(int genId, int processId)
        {
            DataSet dsGenDetails = new DataSet();
            try
            {
                int errorCode;
                SqlParameter[] GenParam = new SqlParameter[3];
                GenParam[0] = new SqlParameter("@GenId", SqlDbType.Int);
                GenParam[0].Value = genId;
                GenParam[1] = new SqlParameter("@ProcessId", SqlDbType.Int);
                GenParam[1].Value = processId;
                GenParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                GenParam[2].Value = 0;
                GenParam[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSRegisterGenerator", GenParam);
                errorCode = (int)GenParam[2].Value;
                return errorCode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetNewGenRequest(int GenId)
        {
            DataSet dsJobDetails = new DataSet();
            try
            {
            
                SqlParameter[] GenParam = new SqlParameter[2];
                GenParam[0] = new SqlParameter("@GenId", SqlDbType.Int);
                GenParam[0].Value = GenId;
                GenParam[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                GenParam[1].Value = 0;
                GenParam[1].Direction = ParameterDirection.InputOutput;
                dsJobDetails = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetNewGenRequest", GenParam);
                return dsJobDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetReportDetails(string reportId)
        {
            DataSet dsRptDetails = new DataSet();

            try
            {
                

                SqlParameter[] GenParam = new SqlParameter[1];
                GenParam[0] = new SqlParameter("@ReportId", SqlDbType.VarChar);
                GenParam[0].Value = reportId;
                dsRptDetails = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetReportDetails", GenParam);
                return dsRptDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }        
        public DataSet GetReportList()
        {
            DataSet dsRptDetails = new DataSet();
            try
            {
                dsRptDetails = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetReportList");
                return dsRptDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetReportTypeList()
        {
            DataSet dsRptDetails = new DataSet();
            try
            {
                dsRptDetails = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetReportTypeList");
                return dsRptDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet ExecuteUnderlayingSP(DataSet dsParam, string SPName, ArrayList ConInfo)
        {
            DataSet dsRptData = new DataSet();
            try
            {
                string conStr = "";
                switch (ConInfo[5].ToString().ToUpper())
                {
                    case "MYSQL":
                        conStr = "SERVER=" + ConInfo[0].ToString() + ";" + "DATABASE=" + ConInfo[2].ToString() + ";" + "UID=" + ConInfo[3].ToString() + ";" + "PASSWORD=" + ConInfo[4].ToString() + ";";

                        if (dsParam.Tables[0].Rows.Count > 0)
                        {
                            MySqlConnection connection = new MySqlConnection(conStr);
                            connection.Open();
                            MySqlCommand sqlComm = new MySqlCommand(SPName, connection);
                            MySqlParameter[] GenParam = new MySqlParameter[dsParam.Tables[0].Rows.Count];
                            for (int i = 0; i < dsParam.Tables[0].Rows.Count; i++)
                            {
                                switch (dsParam.Tables[0].Rows[i]["DataType"].ToString())
                                {
                                    case "StringParameter":
                                        GenParam[i] = new MySqlParameter(dsParam.Tables[0].Rows[i]["ParamName"].ToString(), MySqlDbType.VarChar);
                                        GenParam[i].Value = dsParam.Tables[0].Rows[i]["ParamValue"].ToString();
                                        break;
                                    case "NumberParameter":
                                        GenParam[i] = new MySqlParameter(dsParam.Tables[0].Rows[i]["ParamName"].ToString(), MySqlDbType.Int32);
                                        GenParam[i].Value = dsParam.Tables[0].Rows[i]["ParamValue"].ToString();
                                        break;
                                    case "FloatParameter":
                                        GenParam[i] = new MySqlParameter(dsParam.Tables[0].Rows[i]["ParamName"].ToString(), MySqlDbType.Float);
                                        GenParam[i].Value = dsParam.Tables[0].Rows[i]["ParamValue"].ToString();
                                        break;
                                    case "DateTimeParameter":
                                        GenParam[i] = new MySqlParameter(dsParam.Tables[0].Rows[i]["ParamName"].ToString(), MySqlDbType.Datetime);
                                        GenParam[i].Value = dsParam.Tables[0].Rows[i]["ParamValue"].ToString();
                                        break;
                                    case "BooleanParameter":
                                        GenParam[i] = new MySqlParameter(dsParam.Tables[0].Rows[i]["ParamName"].ToString(), MySqlDbType.Bit);
                                        GenParam[i].Value = dsParam.Tables[0].Rows[i]["ParamValue"].ToString();
                                        break;
                                    default:
                                        break;
                                }
                                sqlComm.Parameters.Add(GenParam[i]);
                            }
                            sqlComm.CommandType = CommandType.StoredProcedure;
                            MySqlDataAdapter da = new MySqlDataAdapter();
                            da.SelectCommand = sqlComm;
                            da.Fill(dsRptData);
                            connection.Close();
                        }
                        else
                        {
                            MySqlConnection connection = new MySqlConnection(conStr);
                            connection.Open();
                            MySqlCommand sqlComm = new MySqlCommand(SPName, connection);
                            sqlComm.CommandType = CommandType.StoredProcedure;
                            MySqlDataAdapter da = new MySqlDataAdapter();
                            da.SelectCommand = sqlComm;
                            da.Fill(dsRptData);
                            
                        }
                        break;
                    case "ODBC":
                        conStr = "server=" + ConInfo[0].ToString() + "; Connection Timeout=800;  User ID=" + ConInfo[3].ToString() + "; password=" + ConInfo[4].ToString() + "; database=" + ConInfo[2].ToString();

                        if (dsParam.Tables[0].Rows.Count > 0)
                        {
                            SqlParameter[] GenParam = new SqlParameter[dsParam.Tables[0].Rows.Count];
                            for (int i = 0; i < dsParam.Tables[0].Rows.Count; i++)
                            {
                                switch (dsParam.Tables[0].Rows[i]["DataType"].ToString())
                                {
                                    case "StringParameter":
                                        GenParam[i] = new SqlParameter(dsParam.Tables[0].Rows[i]["ParamName"].ToString(), SqlDbType.VarChar);
                                        GenParam[i].Value = dsParam.Tables[0].Rows[i]["ParamValue"].ToString();
                                        break;
                                    case "NumberParameter":
                                        GenParam[i] = new SqlParameter(dsParam.Tables[0].Rows[i]["ParamName"].ToString(), SqlDbType.Int);
                                        GenParam[i].Value = dsParam.Tables[0].Rows[i]["ParamValue"].ToString();
                                        break;
                                    case "FloatParameter":
                                        GenParam[i] = new SqlParameter(dsParam.Tables[0].Rows[i]["ParamName"].ToString(), SqlDbType.Float);
                                        GenParam[i].Value = dsParam.Tables[0].Rows[i]["ParamValue"].ToString();
                                        break;
                                    case "DateTimeParameter":
                                        GenParam[i] = new SqlParameter(dsParam.Tables[0].Rows[i]["ParamName"].ToString(), SqlDbType.SmallDateTime);
                                        GenParam[i].Value = dsParam.Tables[0].Rows[i]["ParamValue"].ToString();
                                        break;
                                    case "BooleanParameter":
                                        GenParam[i] = new SqlParameter(dsParam.Tables[0].Rows[i]["ParamName"].ToString(), SqlDbType.Bit);
                                        GenParam[i].Value = dsParam.Tables[0].Rows[i]["ParamValue"].ToString();
                                        break;
                                    default:
                                        break;
                                }
                            }
                            dsRptData = SqlHelper.ExecuteDataset(conStr, CommandType.StoredProcedure, SPName, GenParam);
                        }
                        else
                        {
                            dsRptData = SqlHelper.ExecuteDataset(conStr, CommandType.StoredProcedure, SPName);
                        }

                        break;
                }
                return dsRptData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet ExecuteUnderlyingCommand(string cmdText, ArrayList ConnInfo)
        {
            DataSet dsRptData = new DataSet();
            string ConnectionString;
            try
            {
                switch (ConnInfo[1].ToString().ToUpper())
                {
                    case "MYSQL":
                        ConnectionString = "SERVER=" + ConnInfo[2].ToString() + ";" + "DATABASE=" + ConnInfo[3].ToString() + ";" + "UID=" + ConnInfo[4].ToString() + ";" + "PASSWORD=" + ConnInfo[5].ToString() + ";";
                        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                        {
                            MySqlCommand sqlComm = new MySqlCommand(cmdText, connection);
                            sqlComm.CommandType = CommandType.Text;
                            MySqlDataAdapter da = new MySqlDataAdapter();
                            da.SelectCommand = sqlComm;
                            da.Fill(dsRptData);
                        }

                        break;
                    default:
                        ConnectionString = "Data Source=" + ConnInfo[2].ToString() + ";" + "Initial Catalog=" + ConnInfo[3].ToString() + ";" + "uid=" + ConnInfo[4].ToString() + ";" + "pwd=" + ConnInfo[5].ToString() + ";";
                        dsRptData = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, cmdText);
                        string st = Common.SqlConnectionString;
                        break;
                }
                return dsRptData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int SaveReportDetails(DataSet dsReportDetails,string User_Id)
        {
            DataSet dsRptDetails = new DataSet();
            int returnCode =0;
            int baseReportId = 0;
            SqlConnection cn = new SqlConnection(Common.SqlConnectionString);
            cn.Open();
            SqlTransaction tn = cn.BeginTransaction();
            try
            {
                SqlParameter[] BaseRptParam = new SqlParameter[6];
                BaseRptParam[0] = new SqlParameter("@BaseReportId", SqlDbType.SmallInt);
                BaseRptParam[0].Value = dsReportDetails.Tables[0].Rows[0]["BaseReportId"];
                BaseRptParam[0].Direction = ParameterDirection.InputOutput;
                BaseRptParam[1] = new SqlParameter("@FileName", SqlDbType.VarChar);
                BaseRptParam[1].Value = dsReportDetails.Tables[0].Rows[0]["FileName"];
                BaseRptParam[2] = new SqlParameter("@FileBinary", SqlDbType.VarBinary);
                BaseRptParam[2].Value = dsReportDetails.Tables[0].Rows[0]["FileBinary"];
                BaseRptParam[3] = new SqlParameter("@Varsion", SqlDbType.SmallInt);
                BaseRptParam[3].Value = dsReportDetails.Tables[0].Rows[0]["Version"];
                BaseRptParam[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                BaseRptParam[4].Value = User_Id;
                BaseRptParam[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                BaseRptParam[5].Value = 0;
                BaseRptParam[5].Direction = ParameterDirection.InputOutput;

                //dsRptDetails = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSSaveReport", GenParam);
                SqlHelper.ExecuteNonQuery(tn, CommandType.StoredProcedure, "sp_RGSSaveReportBase", BaseRptParam);

                if (BaseRptParam[5].Value != null)
                    if (Convert.ToInt32(BaseRptParam[5].Value) != 600001 && Convert.ToInt32(BaseRptParam[5].Value) != 600002)
                    {
                        returnCode = 10;
                    }
                    else
                    {
                        baseReportId = Convert.ToInt32(BaseRptParam[0].Value);
                        BaseRptParam = new SqlParameter[1];
                        BaseRptParam[0] = new SqlParameter("@BaseReportId", SqlDbType.SmallInt);
                        BaseRptParam[0].Value = baseReportId;
                        SqlHelper.ExecuteNonQuery(tn, CommandType.StoredProcedure, "sp_RGSDeleteBaseReportObjandParam", BaseRptParam);

                        BaseRptParam = new SqlParameter[1];
                        BaseRptParam[0] = new SqlParameter("@ReportId", SqlDbType.VarChar);
                        BaseRptParam[0].Value = dsReportDetails.Tables[0].Rows[0]["ReportId"];
                        SqlHelper.ExecuteNonQuery(tn, CommandType.StoredProcedure, "sp_RGSDeleteDerivedReportParam", BaseRptParam);
                        

                        dsReportDetails.Tables[0].Rows[0]["BaseReportId"] = baseReportId;
                        returnCode = SaveReport(dsReportDetails, User_Id, baseReportId, tn);
                        if(returnCode != 1)
                        {
                            returnCode = 11; // Error Saving Derived Report
                        }
                        else
                        {
                            returnCode = SaveReportDBObj(dsReportDetails, User_Id, baseReportId, tn);
                            if (returnCode != 1)
                            {
                                returnCode = 12; // Error Saving Report DB Objects
                            }
                            else
                            {
                                returnCode = SaveReportParam(dsReportDetails, User_Id, baseReportId, tn);
                                if (returnCode != 1)
                                {
                                    returnCode = 13; // Error Saving Report Parameters
                                }
                            }
                        }
                    }
                else
                {
                    returnCode = 10; //Error in Saving Base Report
                }
                if (returnCode != 1)
                    tn.Rollback();
                else
                    tn.Commit();
                return returnCode;
            }
            catch (Exception)
            {
                tn.Rollback();
                cn.Close();
                cn.Dispose();
                throw;
            }
        }

        private int SaveReport(DataSet dsReport, string User_Id, int baseReportId, SqlTransaction tn)
        {
            int returnCode = 0;
            SqlParameter[] RptParam = new SqlParameter[8];
            RptParam[0] = new SqlParameter("@ReportId", SqlDbType.VarChar);
            RptParam[0].Value = dsReport.Tables[0].Rows[0]["ReportId"];
            RptParam[1] = new SqlParameter("@ReportName", SqlDbType.VarChar);
            RptParam[1].Value = dsReport.Tables[0].Rows[0]["ReportName"];
            RptParam[2] = new SqlParameter("@ReportType", SqlDbType.VarChar);
            RptParam[2].Value = dsReport.Tables[0].Rows[0]["ReportType"];
            RptParam[3] = new SqlParameter("@BaseReportId", SqlDbType.SmallInt);
            RptParam[3].Value = baseReportId;
            RptParam[4] = new SqlParameter("@BusinessLineId", SqlDbType.SmallInt);
            RptParam[4].Value = dsReport.Tables[0].Rows[0]["BusinessLineId"];
            RptParam[5] = new SqlParameter("@isActive", SqlDbType.Bit);
            RptParam[5].Value = dsReport.Tables[0].Rows[0]["isActive"];
            RptParam[6] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            RptParam[6].Value = User_Id;
            RptParam[7] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            RptParam[7].Value = 0;
            RptParam[7].Direction = ParameterDirection.InputOutput;
            SqlHelper.ExecuteNonQuery(tn, CommandType.StoredProcedure, "sp_RGSSaveReport", RptParam);
            if (RptParam[7] != null)
                if (Convert.ToInt32(RptParam[7].Value) == 600001 || Convert.ToInt32(RptParam[7].Value) == 600002)
                {
                    returnCode  = 1; // Saved / Updated Successfully
                }
            return returnCode;
        }
        private int SaveReportParam(DataSet dsReport, string User_Id, int baseReportId, SqlTransaction tn)
        {
            int returnCode = 0;
            if (dsReport.Tables[2].Rows.Count == 0)
                returnCode = 1;
            foreach (DataRow dr in dsReport.Tables[2].Rows)
            {
                SqlParameter[] RptParam = new SqlParameter[12];
                RptParam[0] = new SqlParameter("@BaseReportId", SqlDbType.SmallInt);
                RptParam[0].Value = baseReportId;
                RptParam[1] = new SqlParameter("@ObjName", SqlDbType.VarChar);
                RptParam[1].Value = dr["ObjName"];
                RptParam[2] = new SqlParameter("@ParamName", SqlDbType.VarChar);
                RptParam[2].Value = dr["ParamName"];
                RptParam[3] = new SqlParameter("@DefaultValue", SqlDbType.VarChar);
                RptParam[3].Value = dr["BaseDefaultValue"];
                RptParam[4] = new SqlParameter("@DataType", SqlDbType.VarChar);
                RptParam[4].Value = dr["DataType"];
                RptParam[5] = new SqlParameter("@isLinked", SqlDbType.Bit);
                RptParam[5].Value = dr["isLinked"];
                RptParam[6] = new SqlParameter("@ReportId", SqlDbType.VarChar);
                RptParam[6].Value = dr["ReportId"];
                RptParam[7] = new SqlParameter("@DerivedDefaultValue", SqlDbType.VarChar);
                RptParam[7].Value = dr["DefaultValue"];
                RptParam[8] = new SqlParameter("@ParamAlias", SqlDbType.VarChar);
                RptParam[8].Value = dr["ParamAlias"];
                RptParam[9] = new SqlParameter("@isMandatory", SqlDbType.Bit);
                RptParam[9].Value = dr["isMandatory"];
                RptParam[10] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                RptParam[10].Value = 0;
                RptParam[11] = new SqlParameter("@LableName", SqlDbType.VarChar);
                RptParam[11].Value = dr["LableName"];
                RptParam[10].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteNonQuery(tn, CommandType.StoredProcedure, "sp_RGSSaveReportParam", RptParam);
                if (RptParam[10] != null)
                    if (Convert.ToInt32(RptParam[10].Value) == 600001 || Convert.ToInt32(RptParam[10].Value) == 600002)
                    {
                        returnCode = 1; // Saved / Updated Successfully
                    }
  
            }
            return returnCode;
        }

        private int SaveReportDBObj(DataSet dsReport, string User_Id, int baseReportId, SqlTransaction tn)
        {
            int returnCode = 0;
            foreach (DataRow dr in dsReport.Tables[1].Rows)
            {
                SqlParameter[] RptParam = new SqlParameter[7];
                RptParam[0] = new SqlParameter("@BaseReportId", SqlDbType.SmallInt);
                RptParam[0].Value = baseReportId;
                RptParam[1] = new SqlParameter("@ObjName", SqlDbType.VarChar);
                RptParam[1].Value = dr["ObjName"];
                RptParam[2] = new SqlParameter("@SPName", SqlDbType.VarChar);
                RptParam[2].Value = dr["SPName"];
                RptParam[3] = new SqlParameter("@ConnectionId", SqlDbType.SmallInt);
                RptParam[3].Value = dr["ConnectionId"];
                RptParam[4] = new SqlParameter("@ConnectionType", SqlDbType.VarChar);
                RptParam[4].Value = dr["ConnectionType"];
                RptParam[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                RptParam[5].Value = 0;
                RptParam[5].Direction = ParameterDirection.InputOutput;
                RptParam[6] = new SqlParameter("@BaseObjId", SqlDbType.Int);
                RptParam[6].Value = 0;
                RptParam[6].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteNonQuery(tn, CommandType.StoredProcedure, "sp_RGSSaveReportBaseDBObj", RptParam);
                if (RptParam[5] != null)
                    if (Convert.ToInt32(RptParam[5].Value) == 600001 || Convert.ToInt32(RptParam[5].Value) == 600002)
                    {
                        dr["BaseObjId"] = RptParam[6].Value;
                        returnCode = 1; // Saved / Updated Successfully
                    }
            }
            return returnCode;
        }

        /// <summary>
        /// Gets the Base Report Templates 
        /// </summary>
        /// <param name="ReportId"> This is optional Parameter if passed blank all Reports will be fetched if passed actual value then only selected base template will be returned</param>
        /// <returns>Dataset with Base Template Details.</returns>
        public DataSet GetBaseReports(string ReportId = "")
        {
            try
            {
                SqlParameter[] RptParam = new SqlParameter[1];
                RptParam[0] = new SqlParameter("@ReportId", SqlDbType.VarChar);
                RptParam[0].Value = ReportId;
                DataSet dsBaseReports = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetReportBaseTemplates");

                return dsBaseReports;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region RDS Report

        public DataSet GetSelectedRDSRptList(string ReportId)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ReportId", SqlDbType.VarChar);
                objParam[0].Value = ReportId;
                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetSelectedReportList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;

        }


        public int SaveRDSReport(DataSet ds, string User_Id)
        {

            try
            {
                SqlParameter[] Parameter = new SqlParameter[6];
                Parameter[0] = new SqlParameter("@ReportId", SqlDbType.VarChar);
                Parameter[0].Value = ds.Tables[0].Rows[0]["ReportId"];

                Parameter[1] = new SqlParameter("@ReportName", SqlDbType.VarChar);
                Parameter[1].Value = ds.Tables[0].Rows[0]["ReportName"];

                Parameter[2] = new SqlParameter("@ReportType", SqlDbType.VarChar);
                Parameter[2].Value = ds.Tables[0].Rows[0]["ReportType"];

                Parameter[3] = new SqlParameter("@BusinessLineId", SqlDbType.SmallInt);
                Parameter[3].Value = ds.Tables[0].Rows[0]["BusinessLineId"];
                
                Parameter[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Parameter[4].Value = User_Id;

                Parameter[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[5].Value = 0;
                Parameter[5].Direction = ParameterDirection.InputOutput;
                try
                {
                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSSaveReport", Parameter);
                    if (Parameter[5].Value != null)

                        return Convert.ToInt32(Parameter[5].Value.ToString());
                    else
                        return 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion RDS Report
    }
}
