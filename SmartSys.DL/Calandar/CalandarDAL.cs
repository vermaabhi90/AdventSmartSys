using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.Calandar
{
    public class CalandarDAL
    {

        #region Plan
        public DataSet GetPlanList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetPlanList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;

        }
        #endregion Plan

        #region Calendar Work

        public DataSet GetCalendarList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetCalenderList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;

        }

        public int SaveTMCalendarDetails(DataSet ds, string User_Id, List<int> LstOwner)
        {
            int errCode = 0;         
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            try
            {
                SqlParameter[] parameters = new SqlParameter[15];

                parameters[0] = new SqlParameter("@ScheduleId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["ScheduleId"];
                parameters[0].Direction = ParameterDirection.InputOutput;


                parameters[1] = new SqlParameter("@Subject", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["Subject"];

                parameters[2] = new SqlParameter("@Location", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["Location"];

                parameters[3] = new SqlParameter("@StartTime", SqlDbType.DateTime);
                parameters[3].Value = ds.Tables[0].Rows[0]["StartTime"];

                parameters[4] = new SqlParameter("@EndTime", SqlDbType.DateTime);
                parameters[4].Value = ds.Tables[0].Rows[0]["EndTime"];

                parameters[5] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[5].Value = ds.Tables[0].Rows[0]["Description"];

                parameters[6] = new SqlParameter("@Owner", SqlDbType.Int);
                parameters[6].Value = ds.Tables[0].Rows[0]["Owner"];

                parameters[7] = new SqlParameter("@Priority", SqlDbType.Int);
                parameters[7].Value = ds.Tables[0].Rows[0]["Priority"];

                parameters[8] = new SqlParameter("@Recurrence", SqlDbType.Int);
                parameters[8].Value = ds.Tables[0].Rows[0]["Recurrence"];

                parameters[9] = new SqlParameter("@AllDay", SqlDbType.Bit);
                parameters[9].Value = ds.Tables[0].Rows[0]["AllDay"];

                parameters[10] = new SqlParameter("@StartTimeZone", SqlDbType.VarChar);
                parameters[10].Value = ds.Tables[0].Rows[0]["StartTimeZone"];

                parameters[11] = new SqlParameter("@EndTimeZone", SqlDbType.VarChar);
                parameters[11].Value = ds.Tables[0].Rows[0]["EndTimeZone"];

                parameters[12] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[12].Value = User_Id;

                parameters[13] = new SqlParameter("@RecurrenceRule", SqlDbType.NVarChar);
                parameters[13].Value = ds.Tables[0].Rows[0]["RecurrenceRule"]; ;

                parameters[14] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[14].Value = 0;
                parameters[14].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_TMSaveSchedule", parameters);
                if (parameters[0].Value != null)
                    errCode = Convert.ToInt32(parameters[0].Value.ToString());
                int ab = Convert.ToInt32(parameters[14].Value.ToString());
                LstOwner.Add(ab);
                if (errCode > 0)
                {
                    if ((TMDeleteTMScheduleParticipants(errCode, sqlTransaction) == 500002))
                    {
                        foreach (int x in LstOwner)
                        {
                            if (!(SaveTMScheduleParticipants(errCode, x, sqlTransaction) == 500002))
                            {
                                sqlTransaction.Rollback();
                                sqlConn.Close();
                                break;
                            }
                        }

                        sqlTransaction.Commit();
                        sqlConn.Close();
                        return errCode;
                    }
                    else
                    {
                        sqlTransaction.Rollback();
                        sqlConn.Close();
                        return 0;
                    }
                }
                else
                {
                    sqlTransaction.Rollback();
                    sqlConn.Close();
                    errCode = 0;
                    return errCode;
                }               
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                sqlTransaction.Rollback();
                sqlConn.Close();
                errCode = 0;
            }
            return errCode;
        }

        private int TMDeleteTMScheduleParticipants(int ScheduleId, SqlTransaction tr)
        {
            int errcode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ScheduleId", SqlDbType.Int);
                parameters[0].Value = ScheduleId;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_TMDeleteScheduleParticipants", parameters);
                if (parameters[1].Value != null)
                    errcode = Convert.ToInt32(parameters[1].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errcode;
        }
        private int SaveTMScheduleParticipants(int ScheduleId, int ParticipantId, SqlTransaction tr)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@ScheduleId", SqlDbType.Int);
                parameters[0].Value = ScheduleId;

                parameters[1] = new SqlParameter("@ParticipantId", SqlDbType.Int);
                parameters[1].Value = ParticipantId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_TMSaveScheduleParticipants", parameters);
                errCode = (int)parameters[2].Value;
                if(errCode == 500001 || errCode == 500002)
                {
                    errCode = 500002;
                }
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        public int DeleteCalendarAppointment(int ScheduleId, string User_Id)
        {
            int errcode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ScheduleId", SqlDbType.Int);
                parameters[0].Value = ScheduleId;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMDeleteScheduleAppointment", parameters);
                if (parameters[2].Value != null)
                    errcode = Convert.ToInt32(parameters[2].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errcode;
        }
        public DataSet GetScheduleParticipendsDetailList(int ScheduleId)
        {
            DataSet ds=new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ScheduleId", SqlDbType.Int);
                parameters[0].Value = ScheduleId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetCalenderParticipantListByScheduleId", parameters);              
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        #endregion Calendar Work
    }
}
