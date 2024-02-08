using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.TMPlan
{
    public class TMPlanDAL
    {
        public DataSet GetTMPlanEmplist(string User_Name)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Name;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMPlanEmployeeListByUser", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet PlanGetEmpDetail(int EmpId, string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = EmpId;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMPlanGetEmpDetail", parameters);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int DeletePlan(int PlanId, string User_id)
        {
            int errode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@PlanId", SqlDbType.Int);
                parameters[0].Value = PlanId;

                parameters[1] = new SqlParameter("@User_id", SqlDbType.NVarChar);
                parameters[1].Value = User_id;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMDeletePlan", parameters);
                if (parameters[2].Value != null)
                    errode = Convert.ToInt32(parameters[2].Value.ToString());
                else
                    errode = 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errode;
        }
        public DataSet GetCalLst(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetPlanCalenderList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetEmpSubordinatesList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_EmpAndSubordinatesList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;

        }
        public DataSet GetTMPlanEmplistReportInView(string user_Id, int EmpCode)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@user_Id", SqlDbType.VarChar);
                param[0].Value = user_Id;

                param[1] = new SqlParameter("@EmpCode", SqlDbType.Int);
                param[1].Value = EmpCode;
                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptGetPlanCalenderListView", param);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }
        public DataSet GetTMPlanSelectedList(int PlanId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PlanId", SqlDbType.Int);
                parameters[0].Value = PlanId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMPlanGetSelectedList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveTMPlanDetails(DataSet dsPlan, string User_Id)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[21];
                parameters[0] = new SqlParameter("@PlanId", SqlDbType.Int);
                parameters[0].Value = dsPlan.Tables[0].Rows[0]["PlanId"];

                parameters[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[1].Value = dsPlan.Tables[0].Rows[0]["EmpId"].ToString();

                parameters[2] = new SqlParameter("@StartDate", SqlDbType.SmallDateTime);
                parameters[2].Value = dsPlan.Tables[0].Rows[0]["StartDate"].ToString();

                parameters[3] = new SqlParameter("@EndDate", SqlDbType.SmallDateTime);
                parameters[3].Value = dsPlan.Tables[0].Rows[0]["EndDate"];

                parameters[4] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[4].Value = dsPlan.Tables[0].Rows[0]["ProjectId"];

                parameters[5] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[5].Value = dsPlan.Tables[0].Rows[0]["TaskId"].ToString();

                parameters[6] = new SqlParameter("@AllDay", SqlDbType.Bit);
                parameters[6].Value = dsPlan.Tables[0].Rows[0]["AllDay"].ToString();

                parameters[7] = new SqlParameter("@Repete", SqlDbType.Bit);
                parameters[7].Value = dsPlan.Tables[0].Rows[0]["Repete"];

                parameters[8] = new SqlParameter("@FrequencyType", SqlDbType.Char);
                parameters[8].Value = dsPlan.Tables[0].Rows[0]["FrequencyType"];

                parameters[9] = new SqlParameter("@WeekDays", SqlDbType.VarChar);
                parameters[9].Value = dsPlan.Tables[0].Rows[0]["WeekDays"].ToString();

                parameters[10] = new SqlParameter("@WeekNo", SqlDbType.TinyInt);
                parameters[10].Value = dsPlan.Tables[0].Rows[0]["WeekNo"].ToString();

                parameters[11] = new SqlParameter("@MonthNo", SqlDbType.Int);
                parameters[11].Value = dsPlan.Tables[0].Rows[0]["MonthNo"].ToString();

                parameters[12] = new SqlParameter("@RecurrenceRule", SqlDbType.NVarChar);
                parameters[12].Value = dsPlan.Tables[0].Rows[0]["RecurrenceRule"];

                parameters[13] = new SqlParameter("@Never", SqlDbType.NVarChar);
                parameters[13].Value = dsPlan.Tables[0].Rows[0]["Never"];

                parameters[14] = new SqlParameter("@OnDate", SqlDbType.NVarChar);
                parameters[14].Value = dsPlan.Tables[0].Rows[0]["OnDate"];

                parameters[15] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[15].Value = User_Id;

                parameters[16] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[16].Value = 0;
                parameters[16].Direction = ParameterDirection.InputOutput;

                parameters[17] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[17].Value = dsPlan.Tables[0].Rows[0]["Remark"];

                parameters[18] = new SqlParameter("@DocType", SqlDbType.VarChar);
                parameters[18].Value = dsPlan.Tables[0].Rows[0]["DocType"];

                parameters[19] = new SqlParameter("@DocId", SqlDbType.Int);
                parameters[19].Value = dsPlan.Tables[0].Rows[0]["DocId"];

                parameters[20] = new SqlParameter("@DocRefId", SqlDbType.Int);
                parameters[20].Value = dsPlan.Tables[0].Rows[0]["DocRefId"];
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSavePlan", parameters);
                if (parameters[16].Value != null)
                    return Convert.ToInt32(parameters[16].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }
        public int SaveTMPlanStatus(int PlanId, int status, string User_id)
        {
            int errode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@PlanId", SqlDbType.Int);
                parameters[0].Value = PlanId;

                parameters[1] = new SqlParameter("@status", SqlDbType.Int);
                parameters[1].Value = status;

                parameters[2] = new SqlParameter("@User_id", SqlDbType.NVarChar);
                parameters[2].Value = User_id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMPlanUpdatestatus", parameters);
                if (parameters[3].Value != null)
                    errode = Convert.ToInt32(parameters[3].Value.ToString());
                else
                    errode = 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errode;
        }
        public DataSet GetPlanDataByDate(int EmpId,DateTime Date)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = EmpId;

                parameters[1] = new SqlParameter("@Date", SqlDbType.DateTime);
                parameters[1].Value = Date;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetPlanDataByDate", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
    }
}
