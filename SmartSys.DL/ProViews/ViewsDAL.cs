using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.ProViews
{
    public class ViewsDAL
    {
        public DataSet GetCaseRiskList(string User_Id, string Filter)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                parameters[1] = new SqlParameter("@Filter", SqlDbType.NVarChar);
                parameters[1].Value = Filter;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetCaseRiskList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetRiskCasePendingByProject(string User_Id, int ProjectId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                parameters[1] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[1].Value = ProjectId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VwPendingcaseRiskListByProject", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetTimeSheetEntryList(string User_Id, int FromWeek, int ToWeek, int EmpId, int Year)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                parameters[1] = new SqlParameter("@FromWeek", SqlDbType.Int);
                parameters[1].Value = FromWeek;

                parameters[2] = new SqlParameter("@ToWeek", SqlDbType.Int);
                parameters[2].Value = ToWeek;

                parameters[3] = new SqlParameter("@EmployeeId", SqlDbType.Int);
                parameters[3].Value = EmpId;

                parameters[4] = new SqlParameter("@Year", SqlDbType.Int);
                parameters[4].Value = Year;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetTimeSheetList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetTimeSheetListByEmployee(string User_Id, int CustomerId, int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                parameters[1] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[1].Value = CustomerId;

                parameters[2] = new SqlParameter("@EmployeeId", SqlDbType.Int);
                parameters[2].Value = EmpId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetTimeSheetListByEmployee", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetSalesBacklogList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetSalesBacklog");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



        public DataSet GetSelectedBacklogs(int BacklogCommentId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@BacklogCommentId", SqlDbType.Int);
                parameters[0].Value = BacklogCommentId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHGetSelectedCommentSalesBacklogs", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetSelectedBacklogComments(string OrderNo)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@OrderNo", SqlDbType.VarChar);
                parameters[0].Value = OrderNo;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetselectedBacklogsComments", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int SaveSalesBacklogComment(DataSet ds, string User_Id, int Status)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[8];
                parameters[0] = new SqlParameter("@BacklogCommentId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["BacklogCommentId"];

                parameters[1] = new SqlParameter("@OrderNo", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["OrderNo"].ToString();

                parameters[2] = new SqlParameter("@Line_No", SqlDbType.Int);
                parameters[2].Value = ds.Tables[0].Rows[0]["Line_No"].ToString();

                parameters[3] = new SqlParameter("@Comment", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[0].Rows[0]["Comment"].ToString();


                parameters[4] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[4].Value = ds.Tables[0].Rows[0]["CompCode"].ToString();

                parameters[5] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[5].Value = User_Id;

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                parameters[6].Direction = ParameterDirection.InputOutput;

                parameters[7] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[7].Value = Status;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHSaveBacklogComments", parameters);
                if (parameters[6].Value != null)
                    return Convert.ToInt32(parameters[6].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region mompendingActionpoint
        public DataSet GetPendingMomActionPoint(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetPendingMOMActionPointList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public DataSet GetPendingMomActionPointByMe(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetPendingMOMActionPointListByMe", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSubordinatePendingMomActionPoint(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetSubordinatePendingMOMActionPointList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCompletedMomActionPoint(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetCompletedMOMActionPointList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCompletedSubordinateMomActionPoint(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetSubordinateCompletedMOMActionPointList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion MOMpendingactionPoint
        public DataSet EnqStatusList(int EnqId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@EnquiryId", SqlDbType.Int);
                parameters[0].Value = EnqId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEnquiryOverAllProgress", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetMailEnqTimeSpan(int EnqId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetTimespanforMailEnquiry", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Enquiry Tracking View
        public int CheckEnquirytoPerson(int EnqId, string User_Id)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;

                parameters[2] = new SqlParameter("@ErrCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_CheckEnquirytoPerson", parameters);
                if (parameters[2].Value != null)
                    ErrCode = Convert.ToInt32(parameters[2].Value.ToString());
                return ErrCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetMailEnqTimeDifference(int EnqId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                parameters[0].Value = EnqId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetTimespanforMailEnquiryTracking", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion Enquiry Tracking view

        #region Project View
        public DataSet GetProjectTaskMOMViewList(int ProjectId, int TaskId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[1].Value = TaskId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWProjectViewMOM", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectTaskTimeSheetList(int ProjectId, int TaskId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[1].Value = TaskId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWProjectTimeSheetViewList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectTaskTimeSheetDetailList(int ProjectId, int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                parameters[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[1].Value = EmpId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWProjectTimeSheetViewDettailList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectPlanVsActualTimeSheetList(int ProjectId, int TaslId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[1].Value = TaslId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWProjectPlanVsActualTimeSheetViewList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectViewPendActionPoint(int ProjectId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWProjectViewPendActionPointList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectViewEquipmentList(int ProjectId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWProjectViewEquipmentList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectAllAttchmentList(int ProjectId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWProjectAllAttachmentsView", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectPlanVsActualList(int ProjectId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWProjectPlanVsActualViewList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjecrResourceList(int ProjectId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWProjectAllResourceList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjecrResourceDetailList(int ProjectId, int Id, string UserType)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                parameters[1] = new SqlParameter("@Id", SqlDbType.Int);
                parameters[1].Value = Id;

                parameters[2] = new SqlParameter("@UserType", SqlDbType.VarChar);
                parameters[2].Value = UserType;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWProjectResourceDetailView", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectExpenceViewList(int ProjectId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetProjectExpensesViewList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetProjectExpencePaymentViewList(int ProjectId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWProjectExpensesPaymentList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Project View

        #region Employee View
        public DataSet GetEmployeeViewList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.VarChar);
                parameters[0].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetEmployeeViewList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetTimesheetListByEmployee(int EmpId)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEmployeeTimeSheetViewList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;

        }
        public DataSet GetLeaveListByEmployee(int EmpId)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VwLeaveListByEmployee", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;

        }
        public DataSet GetActionPointListByEmployee(int EmpId)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWActionPointListByEmployee", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;

        }
        public DataSet GetMomListByEmployee(int EmpId)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWMOMListBYEmployee", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;

        }
        public DataSet GetProjectByEmployee(int EmpId)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VwGetProjectByEmployee", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;

        }
        public DataSet GetProjTaskByEmployee(int EmpId)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VwProjTaskByEmployee", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;

        }


        public DataSet GetTravelRequestByEmployee(int EmpId)
        {
            DataSet dsTravelList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                dsTravelList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VwTravelRequestByEmployee", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTravelList;

        }
        public DataSet GetEnquiryByEmployee(int EmpId)
        {
            DataSet dsList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                dsList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEnquiryListByEmployee", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsList;
        }
        public DataSet GetOwnEnquiryByEmployee(int EmpId)
        {
            DataSet dsList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                dsList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWOwnEnquiryListByEmployee", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsList;
        }
        public DataSet ProjectExpensesList(int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetProjectExpensesListByEmp", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet EmpQuotaionList(int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                param[0].Value = EmpId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWQuotationDetailsByEmp", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet EmpAccounReceivable(int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                param[0].Value = EmpId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWAccountReceivable", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet EmpInventoryList(int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                param[0].Value = EmpId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetInventorybySalesPerson", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet GetReportOpenInEMPView(string user_Id, int EmpId)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@user_Id", SqlDbType.VarChar);
                param[0].Value = user_Id;

                param[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                param[1].Value = EmpId;

                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEmpEncounterInProview", param);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }
        public DataSet EmpTaskListByDate(DateTime StartDate, DateTime EndDate, int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@StartDate", SqlDbType.DateTime);
                param[0].Value = StartDate;

                param[1] = new SqlParameter("@EndDate", SqlDbType.DateTime);
                param[1].Value = EndDate;

                param[2] = new SqlParameter("@EmpId", SqlDbType.Int);
                param[2].Value = EmpId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEmpCalender", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet EmpPieChartData(DateTime StartDate, DateTime EndDate, int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[3];
                param[0] = new SqlParameter("@StartDate", SqlDbType.DateTime);
                param[0].Value = StartDate;

                param[1] = new SqlParameter("@EndDate", SqlDbType.DateTime);
                param[1].Value = EndDate;

                param[2] = new SqlParameter("@EmpId", SqlDbType.Int);
                param[2].Value = EmpId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEmpCalenderChart", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        #endregion Employee view


        #region Customer View

        public DataSet GetAllMOMActionPointByCustomer(int CustomerId)
        {
            DataSet dsCustList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CustId", SqlDbType.Int);
                objParam[0].Value = CustomerId;
                dsCustList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWMOMListBYCustomer", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsCustList;

        }

        public DataSet GetAllAtionPointListBYCustomer(int CustomerId)
        {
            DataSet dsCustList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CustId", SqlDbType.Int);
                objParam[0].Value = CustomerId;
                dsCustList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWAtionPointListBYCustomer", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsCustList;

        }


        public DataSet GetAllEmplistByCustomer(int CustomerId, string User_Id)
        {
            DataSet dsEmpCustList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CustId", SqlDbType.Int);
                objParam[0].Value = CustomerId;
                dsEmpCustList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEmpListBYCustomer", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpCustList;
        }

        public DataSet EnquiryListBYCustomer(int CustomerId)
        {
            DataSet dsEnqLst = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CustId", SqlDbType.Int);
                objParam[0].Value = CustomerId;
                dsEnqLst = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEnquiryListBYCustomer", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEnqLst;
        }


        public DataSet ProjectModelForCustomer(int CustomerId)
        {
            DataSet dsproject = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CustId", SqlDbType.Int);
                objParam[0].Value = CustomerId;
                dsproject = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWProjectDetailsBYCustomer", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsproject;
        }


        public DataSet TravelRequestForCustomer(int CustomerId)
        {
            DataSet dsTravelRequest = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CustId", SqlDbType.Int);
                objParam[0].Value = CustomerId;
                dsTravelRequest = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWTravelRequestListBYCustomer", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTravelRequest;
        }

        public DataSet TimeSheetDetailsForCustomer(int CustomerId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CustId", SqlDbType.Int);
                objParam[0].Value = CustomerId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWTimeSheetDetailsBYCustomer", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet CustomerItemWise(int CustomerId)
        {
            DataSet dsItem = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                objParam[0].Value = CustomerId;
                dsItem = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWCustomerItemWise", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsItem;
        }
        public DataSet CustAccounReceivable(int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                param[0].Value = CustomerId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWAccountReceivableForCust", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet CustAccounReceivableLedgerEntries(string CustomerNo, string Company)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@CustomerNo", SqlDbType.VarChar);
                param[0].Value = CustomerNo;
                param[1] = new SqlParameter("@Company", SqlDbType.VarChar);
                param[1].Value = Company;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWAccountReceivableForCustLegerEntry", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet DispatchDetailsCustomerWise(int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                param[0].Value = CustomerId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWCustomerItemInventory", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet CustomerItemInventory(int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                param[0].Value = CustomerId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWCustomerItemInventory", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet CustomerSalesOrderBacklog(int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                param[0].Value = CustomerId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWCustSalesBacklog", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        #endregion Customer View


        #region VendorView sp_VWMOMListBYVendor
        public DataSet GetEmployeeByVendor(int VendorId, string User_Id)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEmpListBYVendor", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;

        }
        public DataSet GetMoMByVendor(int VendorId)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWMOMListBYVendor", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;

        }
        public DataSet GetMOMActionPointListByVendor(int VendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWAtionPointListBYvendor", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;

        }

        public DataSet ProjectModelForVendor(int VendorId)
        {
            DataSet dsproject = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                dsproject = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWProjectDetailsBYvendor", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsproject;
        }

        public DataSet TravelRequestForVendor(int VendorId)
        {
            DataSet dsTravelRequest = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                dsTravelRequest = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWTravelRequestListBYVendor", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTravelRequest;
        }

        public DataSet TimeSheetDetailsForVendor(int VendorId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWTimeSheetDetailsBYVendor", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }

        public DataSet EnquiryListByVendor(int VendorId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[0].Value = VendorId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEnquiryListByVendor", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet CustomerListBySalesPerson(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWCustomerListByUser", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetFindData(string Type, int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@Type", SqlDbType.VarChar);
                objParam[0].Value = Type;

                objParam[1] = new SqlParameter("@CustomerId", SqlDbType.Int);
                objParam[1].Value = CustomerId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWFindQuot&EnqByCustomer", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        #region DataLevel
        public DataSet DataLevelOne(int Enqid, int DataLevel)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevel", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelTwo(int Enqid, int DataLevel, int SourceId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelTwo", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelThree(int Enqid, int DataLevel, int SourceId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelThree", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelFour(int Enqid, int DataLevel, int SourceId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelFour", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelFive(int Enqid, int DataLevel, int SourceId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelFive", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelSix(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelSix", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelSeven(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelSeven", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelEight(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelEight", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelNine(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelNine", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelTen(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelTen", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelEleven(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelEleven", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelTwelve(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelTwelve", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelthirteen(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelthirteen", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelFourteen(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelfourteen", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelfifteen(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelfifteen", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelSixteen(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelsixteen", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelSeventeen(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelseventeen", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelEighteen(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelEighteen", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelNineteen(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelNineteen", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelTwenty(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelTwenty", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelTwentyOne(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelTwentyOne", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelTwentyTwo(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelTwentyTwo", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelTwentyThree(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelTwentyThree", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelTwentyFour(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelTwentyFour", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelTwentyFive(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelTwentyFive", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelTwentySix(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelTwentySix", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelTwentySeven(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelTwentySeven", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelTwentyEight(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelTwentyeight", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        public DataSet DataLevelTwentyNine(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EnqId", SqlDbType.Int);
                objParam[0].Value = Enqid;

                objParam[1] = new SqlParameter("@DataLevel", SqlDbType.Int);
                objParam[1].Value = DataLevel;

                objParam[2] = new SqlParameter("@SourceId", SqlDbType.Int);
                objParam[2].Value = SourceId;

                objParam[3] = new SqlParameter("@SId", SqlDbType.Int);
                objParam[3].Value = SId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEnquiryInfobyDatalevelTwentyNine", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }
        #endregion DataLevel


        #endregion VendorView
        public DataSet TimeSheetDetails(int TimeSheetId)
        {
            DataSet dsTimeSheet = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@TimeSheetId", SqlDbType.Int);
                objParam[0].Value = TimeSheetId;
                dsTimeSheet = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWTimeSheetDetails", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsTimeSheet;
        }

        public DataSet GetTaskdurationByStatus(int ProjectId, int TaskId)
        {
            DataSet Taskduration = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@projectId", SqlDbType.Int);
                objParam[0].Value = ProjectId;

                objParam[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                objParam[1].Value = TaskId;

                Taskduration = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetTaskdurationByStatus", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Taskduration;
        }
        #region OrgChart
        //public DataSet GetMailEnqTimeDifference(int EnqId)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlParameter[] parameters = new SqlParameter[1];
        //        parameters[0] = new SqlParameter("@EnqId", SqlDbType.Int);
        //        parameters[0].Value = EnqId;
        //        ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetTimespanforMailEnquiryTracking", parameters);
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion OrgChart
        public DataSet GetEmployeeSupermaticAcessList(int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = EmpId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VwGetEmployeeSupermaticAccess", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #region ItemView
        public DataSet GetProjectListForItemVw(int itemId)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@itemId", SqlDbType.Int);
            parameters[0].Value = itemId;
            dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VwTMProjectList", parameters);
            return dsData;
        }

        public DataSet GetDispatchListByItem(int itemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@itemId", SqlDbType.Int);
                parameters[0].Value = itemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetDispatchList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetCustEnquiryListByItem(int itemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@itemId", SqlDbType.Int);
                parameters[0].Value = itemId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCustEnquiryListByItem", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetTMEquipmentListByItem(int itemId)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@itemId", SqlDbType.Int);
            parameters[0].Value = itemId;

            dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VwEquipmentListByItem", parameters);
            return dsData;
        }

        public DataSet GetCustomerPOListByItem(int itemId)
        {
            DataSet ds = new DataSet();
            try
            {
                DataSet dsData = new DataSet();
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@itemId", SqlDbType.Int);
                parameters[0].Value = itemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VwGetCustPOListByItem", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public DataSet GetQuotationListByItem(int itemId)
        {
            try
            {
                DataSet dsData = new DataSet();
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@itemId", SqlDbType.Int);
                parameters[0].Value = itemId;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VwQuotationListByItem", parameters);
                return dsData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet VendorResponseByItem(int itemId)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet dsData = new DataSet();
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@itemId", SqlDbType.Int);
                parameters[0].Value = itemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWVendorResponseListByItem", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion ItemView
        public DataSet GetSegmentListForView()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetSegmentList");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetEquipmentListbySegment(int SegmentId, string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@SegmentId", SqlDbType.Int);
                parameters[0].Value = SegmentId;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEquipmentListbySegment", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetEquipmentListbySalesPerson(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWEquipmentListbySalesPerson", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetReportOpenForProjectView(string user_Id, int ProjectId)
        {
            DataSet DsProj = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[2];
                param[0] = new SqlParameter("@user_Id", SqlDbType.VarChar);
                param[0].Value = user_Id;

                param[1] = new SqlParameter("@ProjectId", SqlDbType.Int);
                param[1].Value = ProjectId;
                DsProj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetAdhocRunReportInProview", param);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DsProj;
        }
        #region BrandView
        public DataSet BrandViewList()
        {
            DataSet Ds = new DataSet();
            try
            {
                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWBrandList");
                return Ds;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendorListByBrand(int BrandId)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@BrandId", SqlDbType.Int);
                param[0].Value = BrandId;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWBrandVendor", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ds;
        }
        public DataSet GetItemListByBrand(int BrandId)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@BrandId", SqlDbType.Int);
                param[0].Value = BrandId;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWBrandItem", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ds;
        }
        public DataSet GetEnquiryListByBrand(int BrandId)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@BrandId", SqlDbType.Int);
                param[0].Value = BrandId;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWBrandEnquiry", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ds;
        }
        public DataSet GetCustomerListByBrand(int BrandId)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@BrandId", SqlDbType.Int);
                param[0].Value = BrandId;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWBrandCustomer", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ds;
        }
        #endregion BrandView

        #region Backlogs
        public DataSet GetPurchaseItemBacklogs(int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[0].Value = ItemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_ItemBacklogList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetPurchaseBrandBacklogs(int BrandId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@BrandId", SqlDbType.Int);
                parameters[0].Value = BrandId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_BrandBacklogList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetPurchaseVendorBacklogs(int VendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = VendorId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VendorBacklogList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetPurchaseCustomerBacklogs(int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_CustomerBacklogList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetSalesItemBacklogs(int ItemId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[0].Value = ItemId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SalesItemBacklogList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetSalesBrandBacklogs(int BrandId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@BrandId", SqlDbType.Int);
                parameters[0].Value = BrandId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_BrandSalesBacklogList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetSalesVendorBacklogs(int VendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[0].Value = VendorId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VendorsalesBacklogList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetSalesCustomerBacklogs(int CustomerId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_CustomerSalesBacklogList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion Backlogs

    }
}
