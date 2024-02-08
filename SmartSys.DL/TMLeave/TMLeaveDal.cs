using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.TMLeave
{
  public class TMLeaveDal
    {
        #region TMLeaveType
        public DataSet GetTMList(string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMLeaveList", ObjParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int SaveTMLeave(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {

                SqlParameter[] parameters = new SqlParameter[10];

                parameters[0] = new SqlParameter("@LeaveTypeId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["LeaveTypeId"];

                parameters[1] = new SqlParameter("@LeaveType", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["LeaveType"];

                parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["Description"];

                parameters[3] = new SqlParameter("@IsPaid", SqlDbType.Bit);
                parameters[3].Value = ds.Tables[0].Rows[0]["IsPaid"];

                parameters[4] = new SqlParameter("@YearlyQuota", SqlDbType.Int);
                parameters[4].Value = ds.Tables[0].Rows[0]["YearlyQuota"];

                parameters[5] = new SqlParameter("@CanLaps", SqlDbType.Bit);
                parameters[5].Value = ds.Tables[0].Rows[0]["CanLaps"];

                parameters[6] = new SqlParameter("@MaxLeaveCarryover", SqlDbType.Int);
                parameters[6].Value = ds.Tables[0].Rows[0]["MaxLeaveCarryover"];

                parameters[7] = new SqlParameter("@Enable", SqlDbType.Bit);
                parameters[7].Value = ds.Tables[0].Rows[0]["Enable"];

                parameters[8] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[8].Value = User_Id;


                parameters[9] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[9].Value = 0;
                parameters[9].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMLeaveSave", parameters);
                if (parameters[9].Value != null)
                    errCode = Convert.ToInt32(parameters[9].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        public DataSet GetselectedTMLeaveList(int LeaveTypeId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@LeaveTypeId", SqlDbType.Int);
                objParam[0].Value = LeaveTypeId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMLeaveGetSelectedList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;

        }
        #endregion TMLeaveType

        #region Leave Approval
        public DataSet GetTMPendingLeaveApprovalList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMLeavePendingApprovalByUser", objParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Leave Approval

        #region Leave Details

        public DataSet GetAllLeaveDetails(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetAllTypeLeaveDetailByUserId", objParam);                
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetBalanceLeave(string User_Id, string LeaveCategory, int LeaveTypeId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;

                objParam[1] = new SqlParameter("@LeaveCategoryn", SqlDbType.VarChar);
                objParam[1].Value = LeaveCategory;

                objParam[2] = new SqlParameter("@LeaveTypeId", SqlDbType.Int);
                objParam[2].Value = LeaveTypeId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetBalanceLeaveByUserId", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetTMLeaveList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMLeaveByUser", objParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetDropdownForStatusCodes()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetDrpdwnforStatusCodes");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetSelectedTMLeaveDetail(int LeaveId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@LeaveId", SqlDbType.Int);
                objParam[0].Value = LeaveId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMLeaveGetSelected", objParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int SaveTMLeaveDetail(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {

                SqlParameter[] parameters = new SqlParameter[13];
                parameters[0] = new SqlParameter("@LeaveId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["LeaveId"];

                parameters[1] = new SqlParameter("@LeaveTypeId", SqlDbType.Int);
                parameters[1].Value = ds.Tables[0].Rows[0]["LeaveTypeId"];

                parameters[2] = new SqlParameter("@ManagerId", SqlDbType.Int);
                parameters[2].Value = ds.Tables[0].Rows[0]["ManagerId"];

                parameters[3] = new SqlParameter("@FromDate", SqlDbType.SmallDateTime);
                parameters[3].Value = ds.Tables[0].Rows[0]["FromDate"];

                parameters[4] = new SqlParameter("@ToDate", SqlDbType.SmallDateTime);
                parameters[4].Value = ds.Tables[0].Rows[0]["ToDate"];

                parameters[5] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[5].Value = ds.Tables[0].Rows[0]["Remark"];

                parameters[6] = new SqlParameter("@ApprovedDate", SqlDbType.SmallDateTime);
                parameters[6].Value = ds.Tables[0].Rows[0]["ApprovedDate"];

                parameters[7] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[7].Value = User_Id;

                parameters[8] = new SqlParameter("@ManagerRemark", SqlDbType.VarChar);
                parameters[8].Value = ds.Tables[0].Rows[0]["ManagerRemark"];

                parameters[9] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[9].Value = 0;
                parameters[9].Direction = ParameterDirection.InputOutput;

                parameters[10] = new SqlParameter("@Status", SqlDbType.VarChar);
                parameters[10].Value = ds.Tables[0].Rows[0]["Status"];

                parameters[11] = new SqlParameter("@LeaveCategory", SqlDbType.VarChar);
                parameters[11].Value = ds.Tables[0].Rows[0]["LeaveCategory"];

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveLeaveDetails", parameters);
                if (parameters[9].Value != null)
                    errCode = Convert.ToInt32(parameters[9].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        #endregion Leave Details

        #region Time Sheet Daily Entry
        public DataSet GetTMProjectList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@UserId", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetProjectList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public DataSet GetMoMByProject(string User_Id, int ProjectId, int TaskId, DateTime Date)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;

                objParam[1] = new SqlParameter("@ProjectId", SqlDbType.Int);
                objParam[1].Value = ProjectId;

                objParam[2] = new SqlParameter("@TaskId", SqlDbType.Int);
                objParam[2].Value = TaskId;

                objParam[3] = new SqlParameter("@Date", SqlDbType.DateTime);
                objParam[3].Value = Date;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetMomByProject", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetTMTaskList(int  ProjectId, string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@ProjectId", SqlDbType.NVarChar);
                objParam[0].Value = ProjectId;
                objParam[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[1].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetTaskList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetTMHistoryList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetHistoryList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetTimeSheetEntryList(int TimeSheetId,string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@TimeSheetId", SqlDbType.Int);
                objParam[0].Value = TimeSheetId;

                objParam[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[1].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetTimeSheetEntryList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }


        public int UpdateStatusTMEntry(int TimeSheetId, int StatusCode)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@TimeSheetId", SqlDbType.Int);
                objParam[0].Value = TimeSheetId;

                objParam[1] = new SqlParameter("@StatusCode", SqlDbType.Int);
                objParam[1].Value = StatusCode;

                objParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[2].Value = 0;
                objParam[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMUpdateTimeSheetStatus", objParam);
                if (objParam[2].Value != null)
                    ErrCode = Convert.ToInt32(objParam[2].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public int SaveTimeSheetEntryLastApproval(int TimeSheetId, string User_Id)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@TimeSheetId", SqlDbType.Int);
                objParam[0].Value = TimeSheetId;

                objParam[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[1].Value = User_Id;

                objParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[2].Value = 0;
                objParam[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveTimeSheetLastApprovalByUser", objParam);
                if (objParam[2].Value != null)
                    ErrCode = Convert.ToInt32(objParam[2].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public int DeleteTimeSheetEntry(int TimeSheetEntryId)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@TimeSheetEntryId", SqlDbType.Int);
                objParam[0].Value = TimeSheetEntryId;

                objParam[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[1].Value = 0;
                objParam[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMDeleteTimeSheetEntry", objParam);
                if (objParam[1].Value != null)
                    ErrCode = Convert.ToInt32(objParam[1].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public DataSet GetTMIdByWeekNo(int WeekNo, string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@WeekNo", SqlDbType.Int);
                objParam[0].Value = WeekNo;

                objParam[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[1].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetTimeSheetIdByWeekNo", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedTimeSheetEntryList(int TimeSheetEntryId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@TimeSheetEntryId", SqlDbType.Int);
                objParam[0].Value = TimeSheetEntryId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetSelectedTimeSheetEntryList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet CheckProjDisp(int ProjectId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                objParam[0].Value = ProjectId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMCheckProjectResource", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int DeleteTMEntryDetail(int TimeSheetEntryDetailId,string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@TimeSheetEntryDetailId", SqlDbType.Int);
                objParam[0].Value = TimeSheetEntryDetailId;

                objParam[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[1].Value = 0;
                objParam[1].Direction = ParameterDirection.InputOutput;

                objParam[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[2].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMDeleteTimesheetEntryDetail", objParam);
                errCode = Convert.ToInt32(objParam[1].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        public int UpdateTMEntryDetail(int TimeSheetEntryDetailId, int CustomerId, int VendorId, int MOMId, string Remark)
        {
            try
            {
                SqlParameter[] objParam = new SqlParameter[5];
                objParam[0] = new SqlParameter("@TimeSheetEntryDetailId", SqlDbType.Int);
                objParam[0].Value = TimeSheetEntryDetailId;

                objParam[1] = new SqlParameter("@Remark", SqlDbType.VarChar);
                objParam[1].Value = Remark;

                objParam[2] = new SqlParameter("@CustomerId", SqlDbType.Int);
                objParam[2].Value = CustomerId;

                objParam[3] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[3].Value = VendorId;

                objParam[4] = new SqlParameter("@MOMId", SqlDbType.Int);
                objParam[4].Value = MOMId;


                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMUpdateTimeSheetDetailEntry", objParam);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 1;
        }
        public DataSet GetTimeSheetDayTimeList(int ProjectId,int TaskId,int DayCode,int TimeSheetId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                objParam[0].Value = ProjectId;

                objParam[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                objParam[1].Value = TaskId;

                objParam[2] = new SqlParameter("@DayCode", SqlDbType.Int);
                objParam[2].Value = DayCode;

                objParam[3] = new SqlParameter("@TimeSheetId", SqlDbType.Int);
                objParam[3].Value = TimeSheetId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetDayTimeList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }


        public int UpdateTimeSheetEntryDetail(DataSet ds, string User_Id)
        {
            int errCode = 0;
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            try
            {

                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@TimeSheetEntryId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["TimeSheetEntryId"];
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@TimeSheetId", SqlDbType.Int);
                parameters[1].Value = ds.Tables[0].Rows[0]["TimeSheetId"];

                parameters[2] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[2].Value = ds.Tables[0].Rows[0]["TaskId"];

                parameters[3] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[3].Value = ds.Tables[0].Rows[0]["ProjectId"];                  

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;
     
                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;

             
            

                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_TMUpdateTimeSheetEntryDetail", parameters);
                if (parameters[0].Value != null)
                    errCode = Convert.ToInt32(parameters[0].Value.ToString());

                if (errCode > 0)
                {
                    foreach (DataRow dr in ds.Tables["tbl_TimeSheet"].Rows)
                    {
                        dr["TimeSheetEntryId"] = errCode;
                        if (!(SaveTMDayTime(dr,sqlTransaction) == 600002))
                        {
                            sqlTransaction.Rollback();
                            sqlConn.Close();
                            break;                                              
                        }
                    }
                    sqlTransaction.Commit();
                    sqlConn.Close();
                }
                else
                {
                    sqlTransaction.Rollback();
                    sqlConn.Close();
                    if (parameters[5].Value != null)
                        errCode = Convert.ToInt32(parameters[5].Value.ToString());
                    return errCode;
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        private int SaveTMDayTime(DataRow dr, SqlTransaction tr)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[9];
                parameters[0] = new SqlParameter("@TimeSheetEntryId", SqlDbType.Int);
                parameters[0].Value = dr["TimeSheetEntryId"];

                parameters[1] = new SqlParameter("@DayCode", SqlDbType.Int);
                parameters[1].Value = dr["DayCode"];

                parameters[2] = new SqlParameter("@FromTime", SqlDbType.VarChar);
                parameters[2].Value = dr["FromTime"];

                parameters[3] = new SqlParameter("@ToTime", SqlDbType.VarChar);
                parameters[3].Value = dr["ToTime"];

                parameters[4] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[4].Value = dr["VendorId"];

                parameters[5] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[5].Value = dr["CustomerId"];

                parameters[6] = new SqlParameter("@MOMId", SqlDbType.Int);
                parameters[6].Value = dr["MOMId"];

                parameters[7] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[7].Value = dr["Remark"];

                parameters[8] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[8].Value = 0;
                parameters[8].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_TMSaveDayTimeEntryDeatil", parameters);
                errCode = (int)parameters[8].Value;
                if(errCode == 500001 || errCode == 500002)
                {
                    errCode = 600002;
                }
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }


        public int SaveTimeSheetEntryDetail(DataSet ds, string User_Id,int WeekNo,string Remark,DateTime dt)
        {
            int errCode = 0;
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            try
            {

                SqlParameter[] parameters = new SqlParameter[11];
                parameters[0] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[0].Value = 26;

                parameters[1] = new SqlParameter("@TimeSheetId", SqlDbType.Int);
                parameters[1].Value = 0;

                parameters[2] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[2].Value = ds.Tables[0].Rows[0]["TaskId"];

                parameters[3] = new SqlParameter("@TimeSheetEntryId", SqlDbType.Int);
                parameters[3].Value = ds.Tables[0].Rows[0]["TimeSheetEntryId"];
                parameters[3].Direction = ParameterDirection.InputOutput;

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@WeekNo", SqlDbType.Int);
                parameters[5].Value = WeekNo;

                parameters[6] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[6].Value = ds.Tables[0].Rows[0]["ProjectId"];   

                parameters[7] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[7].Value = Remark;

                parameters[8] = new SqlParameter("@StartDate", SqlDbType.DateTime);
                parameters[8].Value = dt;

                parameters[9] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[9].Value = 0;
                parameters[9].Direction = ParameterDirection.InputOutput;


             

                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_TMSaveTimeSheetEntryDetail", parameters);
                if (parameters[3].Value != null)
                    errCode = Convert.ToInt32(parameters[3].Value.ToString());
                if (errCode > 0)
                {
                    foreach (DataRow dr in ds.Tables["tbl_TimeSheet"].Rows)
                    {
                        dr["TimeSheetEntryId"] = errCode;
                        if (!(SaveTMDayTime(dr, sqlTransaction) == 600002))
                        {
                            sqlTransaction.Rollback();
                            sqlConn.Close();
                            break;
                        }
                    }
                    sqlTransaction.Commit();
                    sqlConn.Close();
                }
                else
                {
                    sqlTransaction.Rollback();
                    sqlConn.Close();
                    if (parameters[9].Value != null)
                        errCode = Convert.ToInt32(parameters[9].Value.ToString());
                    return errCode;
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion Time Sheet Daily Entry

        #region Project Category

        public DataSet GetTMProjectCategoryList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetProjectCategoryMastList", objParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetSelectedTMProjectCategoryList(int ProjectCategoryId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ProjCategoryId", SqlDbType.Int);
                objParam[0].Value = ProjectCategoryId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetSelectedProjectCategoryMastList", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public int SaveTMProjectCategory(DataSet ds, string User_Id)
        {
            int errCode = 0;
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];

                parameters[0] = new SqlParameter("@ProjCategoryId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["ProjCategoryId"];
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@CategoryName", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["CategoryName"];

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_TMSaveProjectCategoryMastDetail", parameters);
                if (parameters[0].Value != null)
                    errCode = Convert.ToInt32(parameters[0].Value.ToString());

                if ((TMDeleteProjectCategory(errCode, sqlTransaction) == 500002))
                {
                    if (errCode > 0)
                    {
                        foreach (DataRow dr in ds.Tables["tbl_PrjectList"].Rows)
                        {
                            dr["ProjCategoryId"] = errCode;
                            if (!(SaveTMPrjectList(dr, sqlTransaction) == 500002))
                            {
                                sqlTransaction.Rollback();
                                sqlConn.Close();
                                break;
                            }
                        }
                        sqlTransaction.Commit();
                        sqlConn.Close();
                    }
                    else
                    {
                        sqlTransaction.Rollback();
                        sqlConn.Close();
                        if (parameters[5].Value != null)
                            errCode = Convert.ToInt32(parameters[5].Value.ToString());
                        return errCode;
                    }
                }
                else
                {
                    sqlTransaction.Rollback();
                    sqlConn.Close();
                    if (parameters[5].Value != null)
                        errCode = Convert.ToInt32(parameters[5].Value.ToString());
                    return errCode;
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        private int SaveTMPrjectList(DataRow dr, SqlTransaction tr)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@CategoryId", SqlDbType.Int);
                parameters[0].Value = dr["ProjCategoryId"];

                parameters[1] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[1].Value = dr["ProjectId"];                    

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_TMSaveProjectCategoryDeatil", parameters);
                errCode = (int)parameters[2].Value;               
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        private int TMDeleteProjectCategory(int CategoryId, SqlTransaction tr)
        {
            int errcode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CategoryId", SqlDbType.Int);
                parameters[0].Value = CategoryId;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_TMDeleteProjectCategoryDeatilByCategoryId", parameters);
                if (parameters[1].Value != null)
                    errcode = Convert.ToInt32(parameters[1].Value.ToString());
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errcode;
        }
        public DataSet GetProjectListbyCategory(int CategoryId, string User_Id)
        {
                DataSet daobj=new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@CategoryId", SqlDbType.Int);
                parameters[0].Value = CategoryId;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;

                daobj= SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetProjectCategoryList", parameters);                
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return daobj;
        }
        #endregion Project Category

        #region Timesheet Entry Approval
        public DataSet GetTimesheetEntryPendingApprovalList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMTimeSheetEntryPendingApprovalByUser", objParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateTimesheetEntry(int statusCode, string ApproverRemark, int TimeSheetId,string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@TimeSheetId", SqlDbType.Int);
                parameters[0].Value = TimeSheetId;

                parameters[1] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[1].Value = statusCode;

                parameters[2] = new SqlParameter("@ApproverRemark", SqlDbType.VarChar);
                parameters[2].Value = ApproverRemark;


                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

              

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_UpdateTMTimesheet", parameters);
                if (parameters[4].Value != null)
                    errCode = Convert.ToInt32(parameters[4].Value.ToString());
                else
                    return 0;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }


        public DataSet GetEmpmanagerInfo(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysEmpMangerInfoByUserId", objParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetLeaveInfo(int LeaveId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@LeaveId", SqlDbType.Int);
                objParam[0].Value = LeaveId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetLeaveDetailsByLeaveId", objParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Timesheet Entry Approval

        #region Time Sheet Travel Request

        public DataSet GetTimeTravelRequestList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetTravelRequestList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;

        }

        public int SaveTMTravelRequest(DataSet ds, string User_Id, string PurposeOfVisitDocs)
        {
            int errCode = 0;
            try
            {

                SqlParameter[] parameters = new SqlParameter[11];

                parameters[0] = new SqlParameter("@RequestId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["RequestId"];
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["Description"];

                parameters[2] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[2].Value = ds.Tables[0].Rows[0]["CustomerId"];

                parameters[3] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[3].Value = ds.Tables[0].Rows[0]["VendorId"];

                parameters[4] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[4].Value = ds.Tables[0].Rows[0]["Status"];

                parameters[5] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[5].Value = ds.Tables[0].Rows[0]["EmpId"];

                parameters[6] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[6].Value = User_Id;


                parameters[7] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[7].Value = 0;
                parameters[7].Direction = ParameterDirection.InputOutput;

                parameters[8] = new SqlParameter("@PurposeofVisitDocs", SqlDbType.VarChar);
                parameters[8].Value = PurposeOfVisitDocs;

                parameters[9] = new SqlParameter("@PostComment", SqlDbType.VarChar);
                parameters[9].Value = ds.Tables[0].Rows[0]["PostComment"];
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveTravelRequestDetails", parameters);
                if (parameters[0].Value != null)
                    errCode = Convert.ToInt32(parameters[0].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }


        public DataSet GetselectedTMTravelRequestList(int RequestId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@RequestId", SqlDbType.Int);
                objParam[0].Value = RequestId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetSelectedTravelRequestList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }


        public int SaveTravelRequestParticipants(string PType, string[] selitems, int RequestId, string User_Id)
        {
            SqlParameter[] parameters = new SqlParameter[9];
            foreach (var item in selitems)
            {

                parameters[0] = new SqlParameter("@RequestId", SqlDbType.Int);
                parameters[0].Value = RequestId;

                parameters[1] = new SqlParameter("@ParticipantId", SqlDbType.Int);
                parameters[1].Value = item;

                parameters[2] = new SqlParameter("@ParticipantType", SqlDbType.VarChar);
                parameters[2].Value = PType;

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveTravelRequestParticipant", parameters);
            }
            return Convert.ToInt32(parameters[4].Value.ToString());
        }


        public DataSet TravelRequestParticipantList(int RequestId)
        {
            DataSet dsTask = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@RequestId", SqlDbType.Int);
                parameters[0].Value = RequestId;

                dsTask = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMTravelRequestParticipantList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTask;
        }

        public int DeleteTravelRequestParticipants(int RequestId, int ParticipantId, string ParticipantsType)
        {
            SqlParameter[] parameters = new SqlParameter[9];
            parameters[0] = new SqlParameter("@RequestId", SqlDbType.Int);
            parameters[0].Value = RequestId;

            parameters[1] = new SqlParameter("@ParticipantId", SqlDbType.Int);
            parameters[1].Value = ParticipantId;

            parameters[2] = new SqlParameter("@ParticipantsType", SqlDbType.VarChar);
            parameters[2].Value = ParticipantsType;

            parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[3].Value = 0;
            parameters[3].Direction = ParameterDirection.InputOutput;
            try
            {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMDeleteTravelRequestParticipants", parameters);
                if (parameters[3].Value != null)
                    return Convert.ToInt32(parameters[3].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }

        public int SaveTravelBudget(string FileAttachment, string Remark, int RequestId, Double Budget, string BudgetTitle)
        {
            SqlParameter[] parameters = new SqlParameter[9];
            parameters[0] = new SqlParameter("@RequestId", SqlDbType.Int);
            parameters[0].Value = RequestId;

            parameters[1] = new SqlParameter("@FileAttachment", SqlDbType.VarChar);
            parameters[1].Value = FileAttachment;

            parameters[2] = new SqlParameter("@Remark", SqlDbType.VarChar);
            parameters[2].Value = Remark;

            parameters[3] = new SqlParameter("@BudgetTitle", SqlDbType.VarChar);
            parameters[3].Value = BudgetTitle;

            parameters[4] = new SqlParameter("@Budget", SqlDbType.Float);
            parameters[4].Value = Budget;

            parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[5].Value = 0;
            parameters[5].Direction = ParameterDirection.InputOutput;
            try
            {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveTravelBudget", parameters);
                if (parameters[5].Value != null)
                    return Convert.ToInt32(parameters[5].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }
        public DataSet GetBudgetTravelRequesList(int RequestId)
        {
            DataSet dsTask = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@RequestId", SqlDbType.Int);
                parameters[0].Value = RequestId;

                dsTask = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMBudgetTravelList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTask;
        }

        public DataSet GetEditBudgetTravelRequesList(int RequestId, string BudgetTitle)
        {
            DataSet dsTask = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@RequestId", SqlDbType.Int);
                parameters[0].Value = RequestId;
                parameters[1] = new SqlParameter("@BudgetTitle", SqlDbType.VarChar);
                parameters[1].Value = BudgetTitle;

                dsTask = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMBudgetTravelListForEdit", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTask;

        }
        #endregion Time Sheet Travel Request

        #region TravelRequestApprovalList

        public DataSet TravelRequestApprovalLIst(string User_Id)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMTravelRequestApprovalList", parameters);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet TravelRequestReviewList(string User_Id)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMTravelRequestReviewList", parameters);

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ApproveRejectTravel(int statusCode, string ApproverRemark, string User_Id, int RequestId, string Step)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[0].Value = statusCode;

                parameters[1] = new SqlParameter("@ApproverRemark", SqlDbType.VarChar);
                parameters[1].Value = ApproverRemark;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;


                parameters[4] = new SqlParameter("@RequestId", SqlDbType.Int);
                parameters[4].Value = RequestId;

                parameters[5] = new SqlParameter("@Step", SqlDbType.VarChar);
                parameters[5].Value = Step;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveApproveRejectRequestTravel", parameters);
                if (parameters[3].Value != null)
                    errCode = Convert.ToInt32(parameters[3].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int RequestSendForReview(int statusCode, string User_Id, int RequestId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[0].Value = statusCode;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                parameters[3] = new SqlParameter("@RequestId", SqlDbType.Int);
                parameters[3].Value = RequestId;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMRequestSendForReview", parameters);
                if (parameters[2].Value != null)
                    errCode = Convert.ToInt32(parameters[2].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion
    }
}
