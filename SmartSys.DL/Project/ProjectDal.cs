using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SmartSys.Utility;
using SmartSys.DAL;
using System.Data.SqlClient;
using System.Collections;

namespace SmartSys.DAL.Project
{
    public class ProjectDal
    {
        public DataSet GetProjectTypeList()
        {
            DataSet dsData = new DataSet();
            dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjectTypeList");
            return dsData;
        }

        public int DeleteProject(int ProjectId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;
                parameters[0].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteNonQuery(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_UpdateTmProject", parameters);
                return (Convert.ToInt32(parameters[0].Value.ToString()));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public int SaveProjectType(int ProjectTypeId, string ProjectType, Boolean isActive, string User_Id)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@ProjectTypeId", SqlDbType.Int);
            parameters[0].Value = ProjectTypeId;
            parameters[0].Direction = ParameterDirection.InputOutput;
            parameters[1] = new SqlParameter("@ProjectType", SqlDbType.VarChar);
            parameters[1].Value = ProjectType;
            parameters[2] = new SqlParameter("@isActive", SqlDbType.Bit);
            parameters[2].Value = isActive;
            parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            parameters[3].Value = User_Id;
            parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[4].Value = 0;
            parameters[4].Direction = ParameterDirection.InputOutput;
            SqlHelper.ExecuteNonQuery(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveProjectType", parameters);
            if (parameters[4].Value != null)
                if ((Convert.ToInt32(parameters[4].Value.ToString()) == 500001 || Convert.ToInt32(parameters[4].Value.ToString()) == 500002) && Convert.ToInt32(parameters[0].Value.ToString()) > 0)
                    return (Convert.ToInt32(parameters[0].Value.ToString()));
                else
                    return 0;
            else
                return 0;
        }

        public DataSet GetProjectTypeTaskList(int ProjectTypeId)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@ProjectTypeId", SqlDbType.Int);
            parameters[0].Value = ProjectTypeId;
            dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjectTypeTaskList", parameters);
            return dsData;
        }

        public void DeleteProjectTask(int ProjectId, ArrayList ProjectTaskLst)
        {
            SqlConnection cn = new SqlConnection(Common.SqlConnectionString);
            cn.Open();
            SqlTransaction trn = cn.BeginTransaction();
            try
            {
                for (int i = 0; i < ProjectTaskLst.Count; i++)
                {
                    SqlParameter[] parameters = new SqlParameter[10];
                    parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                    parameters[0].Value = ProjectId;
                    parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                    parameters[1].Value = ProjectTaskLst[i];
                    SqlHelper.ExecuteNonQuery(trn, CommandType.StoredProcedure, "sp_TMDeleteProjectTask", parameters);
                }
                trn.Commit();
                cn.Close();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                cn.Close();
                throw ex;
            }

        }

        public void DeleteProjectTypeTask(int ProjectTypeId, ArrayList ProjectTypeTaskLst)
        {
            SqlConnection cn = new SqlConnection(Common.SqlConnectionString);
            cn.Open();
            SqlTransaction trn = cn.BeginTransaction();
            try
            {
                for (int i = 0; i < ProjectTypeTaskLst.Count; i++)
                {
                    SqlParameter[] parameters = new SqlParameter[10];
                    parameters[0] = new SqlParameter("@ProjectTypeId", SqlDbType.Int);
                    parameters[0].Value = ProjectTypeId;
                    parameters[1] = new SqlParameter("@ProjectTypeTaskId", SqlDbType.Int);
                    parameters[1].Value = ProjectTypeTaskLst[i];
                    SqlHelper.ExecuteNonQuery(trn, CommandType.StoredProcedure, "sp_TMDeleteProjectTypeTask", parameters);
                }
                trn.Commit();
                cn.Close();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                cn.Close();
                throw ex;
            }
            
        }

        public void SaveProjectTypeTask(int ProjectTypeId, int ProjectTypeTaskId, string TaskName, float Duration, DateTime StartDate, DateTime EndDate, int ParentTaskId, string Predecessors, string User_Id, ArrayList projTypeParent,string Description, int Progress)
        {
            SqlConnection cn = new SqlConnection(Common.SqlConnectionString);
            cn.Open();
            SqlTransaction trn = cn.BeginTransaction();
            try
            {

                SqlParameter[] parameters = new SqlParameter[12];
                parameters[0] = new SqlParameter("@ProjectTypeId", SqlDbType.Int);
                parameters[0].Value = ProjectTypeId;
                parameters[1] = new SqlParameter("@ProjectTypeTaskId", SqlDbType.Int);
                parameters[1].Value = ProjectTypeTaskId;
                parameters[2] = new SqlParameter("@TaskName", SqlDbType.VarChar);
                parameters[2].Value = TaskName;
                parameters[3] = new SqlParameter("@Duration", SqlDbType.Float);
                parameters[3].Value = Duration;
                parameters[4] = new SqlParameter("@StartDate", SqlDbType.SmallDateTime);
                parameters[4].Value = StartDate;
                parameters[5] = new SqlParameter("@EndDate", SqlDbType.SmallDateTime);
                parameters[5].Value = EndDate;
                parameters[6] = new SqlParameter("@ParentTaskId", SqlDbType.Int);
                parameters[6].Value = ParentTaskId;
                parameters[7] = new SqlParameter("@Predecessors", SqlDbType.VarChar);
                parameters[7].Value = Predecessors;
                parameters[8] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[8].Value = User_Id;
                parameters[9] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[9].Value = Description;
                parameters[10] = new SqlParameter("@Progress", SqlDbType.Int);
                parameters[10].Value = Progress;
                parameters[11] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[11].Value = 0;
                parameters[11].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteNonQuery(trn, CommandType.StoredProcedure, "sp_TMSaveProjectTypeTask", parameters);

                for (int i = 0; i < projTypeParent.Count; i++)
                {
                    int[] arr = (int[])projTypeParent[i];
                    parameters = new SqlParameter[3];
                    parameters[0] = new SqlParameter("@ProjectTypeId", SqlDbType.Int);
                    parameters[0].Value = ProjectTypeId;
                    parameters[1] = new SqlParameter("@ProjectTypeTaskId", SqlDbType.Int);
                    parameters[1].Value = arr[0];
                    parameters[2] = new SqlParameter("@ParentTaskId", SqlDbType.Int);
                    parameters[2].Value = arr[1];
                    SqlHelper.ExecuteNonQuery(trn, CommandType.StoredProcedure, "sp_TMSaveProjectTypeTaskParent", parameters);
                }
                trn.Commit();
                cn.Close();
            }
            catch (Exception ex)
            {
                trn.Rollback();
                cn.Close();
                throw ex;
            }

        }

        public DataSet GetProjectList(string User_Id)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("@User_Id", SqlDbType.VarChar);
            parameters[0].Value = User_Id;
            dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjectList", parameters);
            return dsData;
        }
        public DataSet GetSelectedProjectTaskOverDueList(string User_Id, string TaskName, string Status)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@User_Id", SqlDbType.VarChar);
            parameters[0].Value = User_Id;

            parameters[1] = new SqlParameter("@TaskName", SqlDbType.VarChar);
            parameters[1].Value = TaskName;

            parameters[2] = new SqlParameter("@Status", SqlDbType.VarChar);
            parameters[2].Value = Status;

            dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWprojectTaskOverdueDet", parameters);
            return dsData;
        }

        public DataSet GetProjectTaskList(int ProjectId, string User_Id)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            parameters[0].Value = User_Id;
            parameters[1] = new SqlParameter("@ProjectId", SqlDbType.Int);
            parameters[1].Value = ProjectId;
            dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjectTaskList", parameters);
            return dsData;
        }
        #region Risk Task
        public int UpdateRiskTaskReviewDetail(int statusCode,int ProjectId, int TaskId, string User_Id)
        {
            DataSet dsData = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[1].Value = TaskId;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[3].Value = statusCode;
                parameters[3].Direction = ParameterDirection.InputOutput;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0; 
                parameters[4].Direction = ParameterDirection.InputOutput;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_UpdateTMprojRiskTaskReview", parameters);
                if (parameters[3].Value != null)
                    return Convert.ToInt32(parameters[3].Value.ToString());
                else
                    return 0;
            }
           catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public int UpdateRiskTaskApprovalDetail(int statusCode, int ProjectId, int TaskId, string User_Id)
        {
            DataSet dsData = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[1].Value = TaskId;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[3].Value = statusCode;
                parameters[3].Direction = ParameterDirection.InputOutput;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_UpdateTMprojRiskTaskApproval", parameters);
                if (parameters[3].Value != null)
                    return Convert.ToInt32(parameters[3].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int SaveRiskTask(int ProjectId, string TaskName, string StartDate, string EndDate, string Description, string ResourceIDs, int TaskID, int Duration, int StatusCode, string Comments, int CommentId, int TaskType, string User_Id, int progress, int VendorId, int CustomerId)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[17];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                parameters[1] = new SqlParameter("@TaskName", SqlDbType.VarChar);
                parameters[1].Value = TaskName;
                                
                parameters[2] = new SqlParameter("@StartDate", SqlDbType.VarChar);
                parameters[2].Value = StartDate;

                parameters[3] = new SqlParameter("@EndDate", SqlDbType.VarChar);
                parameters[3].Value = EndDate;
                            
                parameters[4] = new SqlParameter("@Duration", SqlDbType.Float);
                parameters[4].Value = Duration;

                parameters[5] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[5].Value = User_Id;
                          
                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                parameters[6].Direction = ParameterDirection.InputOutput;

                parameters[7] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[7].Value = Description;

                parameters[8] = new SqlParameter("@ResourceIDs", SqlDbType.VarChar);
                parameters[8].Value = ResourceIDs;

                parameters[9] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[9].Value = TaskID;
                parameters[9].Direction = ParameterDirection.InputOutput;

                parameters[10] = new SqlParameter("@Statuscode", SqlDbType.Int);
                parameters[10].Value = StatusCode;

                parameters[11] = new SqlParameter("@Comments", SqlDbType.VarChar);
                parameters[11].Value = Comments;

                parameters[12] = new SqlParameter("@CommentId", SqlDbType.VarChar);
                parameters[12].Value = CommentId;

                parameters[13] = new SqlParameter("@TaskType", SqlDbType.Int);
                parameters[13].Value = TaskType;

                parameters[14] = new SqlParameter("@progress", SqlDbType.Int);
                parameters[14].Value = progress;


                parameters[15] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[15].Value = VendorId;


                parameters[16] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[16].Value = CustomerId;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveProjectRiskTask", parameters);
                if (parameters[9].Value != null)

                {                 SqlParameter[] parameter = new SqlParameter[4];
                parameter[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameter[0].Value = ProjectId;

                parameter[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameter[1].Value = TaskID;

                parameter[2] = new SqlParameter("@StatusId", SqlDbType.Int);
                parameter[2].Value = StatusCode;

                parameter[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameter[3].Value = 0;
                parameter[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveProjectTaskStatusAuditDetail", parameter);

                return Convert.ToInt32(parameters[9].Value.ToString());
                }

                else 
            
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }
        public DataSet GetProjectRiskList(string User_Id)
        {
            DataSet dsData = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@UserId", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjectRiskList", parameters);

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return dsData;
        }
        public DataSet GetCaseRiskList(string UserName)
        {
            DataSet dsData = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = UserName;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetCaseRiskList", parameters);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsData;
        }
        public DataSet GetRiskReviewList(string User_Id)
         {
            DataSet dsData = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjRiskReviewList", parameters);

            }
            catch(Exception ex)
            {
                throw ex;
            }
            return dsData;
        }
           public DataSet GetRiskApprovalList(string User_Id)
           {
               DataSet dsData = new DataSet();
               try
               {
                   SqlParameter[] parameters = new SqlParameter[2];
                   parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                   parameters[0].Value = User_Id;
                   dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjRiskApprovalList", parameters);

               }
               catch (Exception ex)
               {
                   throw ex;
               }
               return dsData;
           }
        public DataSet GetSelectedRiskTaskList(int ProjectId, int TaskId)
        {
              DataSet dsData = new DataSet();
              try
              {

                  SqlParameter[] parameters = new SqlParameter[2];
                  parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                  parameters[0].Value = ProjectId;

                  parameters[1] = new SqlParameter("@TaskId", SqlDbType.VarChar);
                  parameters[1].Value = TaskId;
                  dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetSelectedRiskList", parameters);
              }
              catch (Exception ex)
              {
                  throw ex;
              }
            return dsData;
        }

        public DataSet GetSelectedProjectCommentList(int ProjectId, int TaskId)
        {
            DataSet dsData = new DataSet();
            try
            {

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                parameters[1] = new SqlParameter("@TaskId", SqlDbType.VarChar);
                parameters[1].Value = TaskId;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetProjectCommentList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsData;
        }
        public DataSet GetProjectVendorAndCustomer(int ProjectId)
        {
            DataSet dsData = new DataSet();
           
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMgetProjectVendorandCustomer", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsData;
        }
        #endregion Risk Task
  
        public int SaveProject(int ProjectId, int ProjectTypeId, int ProjectManager, string ProjectName, int StatusId, string Region, string Description, string Remark,string User_Id, DateTime startDate, int CustomerId,int VendorId,string CompCode, int SegmentId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[15];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;
                parameters[0].Direction = ParameterDirection.InputOutput;
                parameters[1] = new SqlParameter("@ProjectTypeId", SqlDbType.Int);
                parameters[1].Value = ProjectTypeId;
                parameters[2] = new SqlParameter("@ProjectName", SqlDbType.VarChar);
                parameters[2].Value = ProjectName;
                parameters[3] = new SqlParameter("@StatusId", SqlDbType.Int);
                parameters[3].Value = StatusId;
                parameters[4] = new SqlParameter("@Region", SqlDbType.VarChar);
                parameters[4].Value = Region;
                parameters[5] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[5].Value = Description;
                parameters[6] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[6].Value = Remark;
                parameters[7] = new SqlParameter("@ProjectManager", SqlDbType.Int);
                parameters[7].Value = ProjectManager;
                parameters[8] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[8].Value = User_Id;
                parameters[9] = new SqlParameter("@StartDate", SqlDbType.SmallDateTime);
                parameters[9].Value = startDate;
                parameters[10] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[10].Value = CustomerId;
                parameters[11] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[11].Value = VendorId;
                parameters[12] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[12].Value = CompCode;
                parameters[13] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[13].Value = 0;
                parameters[13].Direction = ParameterDirection.InputOutput;
                parameters[14] = new SqlParameter("@SegmentId", SqlDbType.Int);
                parameters[14].Value = SegmentId;
                SqlHelper.ExecuteNonQuery(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveProject", parameters);
                if (parameters[13].Value != null)
                    if ((Convert.ToInt32(parameters[13].Value.ToString()) == 500001 || Convert.ToInt32(parameters[13].Value.ToString()) == 500002) && Convert.ToInt32(parameters[0].Value.ToString()) > 0)
                        return (Convert.ToInt32(parameters[0].Value.ToString()));
                    else
                        return 0;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                
                Common.LogException(ex);
            }
            return 0;
        }


        public void SaveProjectTask(int ProjectId, int TaskId, string TaskName, float Duration, DateTime StartDate, DateTime EndDate, int ParentTaskId, string Predecessors, string User_Id, ArrayList Resources, int Progress, int TaskType, ArrayList projParent, string Description)
        {
            SqlConnection cn = new SqlConnection(Common.SqlConnectionString);
            cn.Open();
            SqlTransaction Tr = cn.BeginTransaction();
            try
            {

                SqlParameter[] parameters = new SqlParameter[13];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;
                parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[1].Value = TaskId;
                parameters[2] = new SqlParameter("@TaskName", SqlDbType.VarChar);
                parameters[2].Value = TaskName;
                parameters[3] = new SqlParameter("@Duration", SqlDbType.Float);
                parameters[3].Value = Duration;
                parameters[4] = new SqlParameter("@StartDate", SqlDbType.SmallDateTime);
                parameters[4].Value = StartDate;
                parameters[5] = new SqlParameter("@EndDate", SqlDbType.SmallDateTime);
                parameters[5].Value = EndDate;
                parameters[6] = new SqlParameter("@ParentTaskId", SqlDbType.Int);
                parameters[6].Value = ParentTaskId;
                parameters[7] = new SqlParameter("@Predecessors", SqlDbType.VarChar);
                parameters[7].Value = Predecessors;
                parameters[8] = new SqlParameter("@TaskType", SqlDbType.Int);
                parameters[8].Value = TaskType;
                parameters[9] = new SqlParameter("@Progress", SqlDbType.Int);
                parameters[9].Value = Progress;
                parameters[10] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[10].Value = User_Id;
                parameters[11] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[11].Value = Description;
                parameters[12] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[12].Value = 0;
                parameters[12].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteNonQuery(Tr, CommandType.StoredProcedure, "sp_TMSaveProjectTask", parameters);
                if (parameters[12].Value != null)
                    if (Convert.ToInt32(parameters[12].Value.ToString()) == 500001 || Convert.ToInt32(parameters[12].Value.ToString()) == 500002)
                    {
                        for (int i = 0; i < Resources.Count; i++)
                        {
                            parameters = new SqlParameter[4];
                            parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                            parameters[0].Value = ProjectId;
                            parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                            parameters[1].Value = TaskId;
                            parameters[2] = new SqlParameter("@EmpId", SqlDbType.Int);
                            parameters[2].Value = Resources[i];
                            parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                            parameters[3].Value = 0;
                            parameters[3].Direction = ParameterDirection.InputOutput;
                            SqlHelper.ExecuteNonQuery(Tr, CommandType.StoredProcedure, "sp_TMSaveProjectTaskResource", parameters);
                        }
                        for (int i = 0; i < projParent.Count; i++)
                        {
                            int[] arr = (int[])projParent[i];
                            parameters = new SqlParameter[3];
                            parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                            parameters[0].Value = ProjectId;
                            parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                            parameters[1].Value = arr[0];
                            parameters[2] = new SqlParameter("@ParentTaskId", SqlDbType.Int);
                            parameters[2].Value = arr[1];
                            SqlHelper.ExecuteNonQuery(Tr, CommandType.StoredProcedure, "sp_TMSaveProjectTaskParent", parameters);
                        }
                        Tr.Commit();
                        cn.Close();
                    }
                    else
                    {
                        Tr.Rollback();
                        cn.Close();
                    }
            }
            catch (Exception)
            {
                if (cn.State == ConnectionState.Open)
                {
                    Tr.Rollback();
                    cn.Close();
                }
                throw;
            }
        }

        public DataSet GetSelectedProjectDocList(int ProjectId, int TaskId)
        {
            DataSet dsData = new DataSet();
            try
            {

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[1].Value = TaskId;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjectDocList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsData;
        }

        public DataSet GetProjectTaskCustomerList(int ProjectId, int TaskId)
        {
            DataSet dsData = new DataSet();
            try
            {

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[1].Value = TaskId;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjectTaskCustlist", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsData;
        }
        public DataSet GetProjectTaskVendorList(int ProjectId, int TaskId)
        {
            DataSet dsData = new DataSet();
            try
            {

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ProjectId;

                parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[1].Value = TaskId;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjectTaskVendlist", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsData;
        }

        public DataSet GetCustomerListByUser(string User_Id, int ProjectId, int TaskId)
        {
            DataSet dsData = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                parameters[1] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[1].Value = ProjectId;

                parameters[2] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[2].Value = TaskId;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerlistForDropDownbyUser", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }            
            return dsData;
        }

        public DataSet GetVendorListByUser(string User_Id,int ProjectId, int TaskId)
        {
            DataSet dsData = new DataSet();
            try
            {

                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                parameters[1] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[1].Value = ProjectId;

                parameters[2] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[2].Value = TaskId;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorlistForDropDownbyUser", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsData;
        }

        public DataSet GetQuotationlistForCustomer(int CustomerId)
        {
            DataSet Dsdata = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("CustomerId", SqlDbType.Int);
                parameters[0].Value = CustomerId;
                Dsdata = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWQuotationlistForCustomer", parameters);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Dsdata;
        }
        public int UpdateQuotStatus(int QuotId,string User_Id)
        {
            DataSet Dsdata = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("QuotId", SqlDbType.Int);
                parameters[0].Value = QuotId;
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;
                SqlHelper.ExecuteNonQuery(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWChangeQuotationStatus", parameters);
                return (Convert.ToInt32(parameters[0].Value.ToString()));
               
            }
            catch (Exception ex)
            {
                throw ex;
            }                    
        }

        public DataSet GetSelectedProjectEditModeList(int ProjectTypeId, int ProjectTypeTaskId)
        {
            DataSet dsData = new DataSet();
            try
            {
               
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ProjectTypeId", SqlDbType.Int);
                parameters[0].Value = ProjectTypeId;

                parameters[1] = new SqlParameter("@ProjectTypeTaskId", SqlDbType.Int);
                parameters[1].Value = ProjectTypeTaskId;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjectTypeDocList", parameters);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
           
            return dsData;
        }
        //

        public int SaveProjectTaskDoc(DataSet ds, string User_Id)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[1].Rows[0]["ProjectId"];

                parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[1].Value = ds.Tables[1].Rows[0]["TaskId"].ToString();

                parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[1].Rows[0]["Description"].ToString();

                parameters[3] = new SqlParameter("@FileName", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[1].Rows[0]["FileName"];

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveProjectTaskDocs", parameters);
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

        public int SavePrjectTypeTaskDoc(DataSet ds,string User_Id)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@ProjectTypeId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[1].Rows[0]["ProjectTypeId"];

                parameters[1] = new SqlParameter("@ProjectTypeTaskId", SqlDbType.Int);
                parameters[1].Value = ds.Tables[1].Rows[0]["ProjectTypeTaskId"].ToString();

                parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[1].Rows[0]["Description"].ToString();

                parameters[3] = new SqlParameter("@FileName", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[1].Rows[0]["FileName"];

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveProjectTypeTaskDocs", parameters);
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

        public int UpdateProjectTask(DataSet ds, string User_Id, string Comments)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[11];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["ProjectId"];

                parameters[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[1].Value = ds.Tables[0].Rows[0]["TaskId"].ToString();

                parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["Description"].ToString();

                parameters[3] = new SqlParameter("@TaskName", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[0].Rows[0]["TaskName"];

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;

                parameters[6] = new SqlParameter("@CustomerId", SqlDbType.Int);
                parameters[6].Value = ds.Tables[0].Rows[0]["CustomerId"].ToString(); 

                parameters[7] = new SqlParameter("@VendorId", SqlDbType.Int);
                parameters[7].Value = ds.Tables[0].Rows[0]["VendorId"].ToString();

                parameters[8] = new SqlParameter("@Progress", SqlDbType.Int);
                parameters[8].Value = ds.Tables[0].Rows[0]["Progress"].ToString();

                parameters[9] = new SqlParameter("@Comments", SqlDbType.VarChar);
                parameters[9].Value = Comments;

                parameters[10] = new SqlParameter("@EsimationTime", SqlDbType.Int);
                parameters[10].Value = ds.Tables[0].Rows[0]["EstimationTime"].ToString();

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMUpdateProjectTask", parameters);
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

        public int UpdatePrjectTypeTask(DataSet ds, string User_Id)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@ProjectTypeId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["ProjectTypeId"];

                parameters[1] = new SqlParameter("@ProjectTypeTaskId", SqlDbType.Int);
                parameters[1].Value = ds.Tables[0].Rows[0]["ProjectTypeTaskId"].ToString();

                parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["Description"].ToString();

                parameters[3] = new SqlParameter("@TaskName", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[0].Rows[0]["TaskName"];

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMUpdateProjectTypeTask", parameters);
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
        #region Project Task MOM Details
        public DataSet ProjTaskList(int ProjectId)
        {
            DataSet dsTask = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.NVarChar);
                parameters[0].Value = ProjectId;
                dsTask = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_MOMProjTasKList", parameters);
            }
            catch( Exception ex)
            {
                throw ex;
            }
            return dsTask;
        }
        public DataSet ProjTaskMOMList(string User_Id)
        {
            DataSet dsTask = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                dsTask = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjTaskMOMList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTask;
        }
        public DataSet GetselectedProjTaskMOM(int MOMId)
        {
            DataSet dsTask = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@MOMId", SqlDbType.Int);
                parameters[0].Value = MOMId;

                dsTask = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetSelectedProjTaskMOM", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTask;
        }
        public int SaveProjTaskMOM(DataSet ds, string User_Id)
        {
            SqlParameter[] parameters = new SqlParameter[14];
            parameters[0] = new SqlParameter("@MOMId", SqlDbType.Int);
            parameters[0].Value = ds.Tables[0].Rows[0]["MOMId"].ToString();
            parameters[0].Direction = ParameterDirection.InputOutput;

            parameters[1] = new SqlParameter("@ProjectId", SqlDbType.Int);
            parameters[1].Value = ds.Tables[0].Rows[0]["ProjectId"].ToString();

            parameters[2] = new SqlParameter("@TaskId", SqlDbType.Int);
            parameters[2].Value = ds.Tables[0].Rows[0]["TaskId"].ToString();

            parameters[3] = new SqlParameter("@MOMDate", SqlDbType.SmallDateTime);
            parameters[3].Value = ds.Tables[0].Rows[0]["MOMDate"].ToString();

            parameters[4] = new SqlParameter("@EmpId", SqlDbType.Int);
            parameters[4].Value = 1;

            parameters[5] = new SqlParameter("@Title", SqlDbType.VarChar);
            parameters[5].Value = ds.Tables[0].Rows[0]["Title"].ToString();

            parameters[6] = new SqlParameter("@Description", SqlDbType.VarChar);
            parameters[6].Value = ds.Tables[0].Rows[0]["Description"].ToString();

            parameters[7] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            parameters[7].Value = User_Id;

            parameters[8] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[8].Value = 0;
            parameters[8].Direction = ParameterDirection.InputOutput;

            parameters[9] = new SqlParameter("@LocalDescription", SqlDbType.VarChar);
            parameters[9].Value = ds.Tables[0].Rows[0]["LocalDescription"].ToString();

            parameters[10] = new SqlParameter("@MOMType", SqlDbType.VarChar);
            parameters[10].Value = ds.Tables[0].Rows[0]["MOMType"].ToString();

            parameters[11] = new SqlParameter("@ManagementView", SqlDbType.VarChar);
            parameters[11].Value = ds.Tables[0].Rows[0]["ManageMentView"].ToString();

            parameters[12] = new SqlParameter("@CustomerId", SqlDbType.Int);
            parameters[12].Value = ds.Tables[0].Rows[0]["CustomerId"].ToString();

            parameters[13] = new SqlParameter("@VendorId", SqlDbType.Int);
            parameters[13].Value = ds.Tables[0].Rows[0]["VendorId"].ToString();
            try
            {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveProjectTaskMOM", parameters);
                if (parameters[0].Value != null)
                    return Convert.ToInt32(parameters[0].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }
        public DataSet MOMType()
        {
            DataSet MOMTYPE = new DataSet();

            try
            {
                MOMTYPE = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetMOMType");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MOMTYPE;
        }
        public DataSet MOMParticipantList(int MOMId)
        {
            DataSet dsTask = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@MOMId", SqlDbType.Int);
                parameters[0].Value = MOMId;

                dsTask = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_MOMParticipantList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTask;
        }
        public DataSet CustomerContactListByCustomerId(int CustomerId)
        {
            DataSet dsTask = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@CustomerId", SqlDbType.NVarChar);
                parameters[0].Value = CustomerId;
                dsTask = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerContactListByCustomerId", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTask;
        }
        public DataSet VendorContactListByVendorId(int VendorId)
        {
            DataSet dsTask = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@VendorId", SqlDbType.NVarChar);
                parameters[0].Value = VendorId;
                dsTask = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorContactListByVendorId", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTask;
        }
        //public DataSet GetselectedParticipantList(int MOMId)
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlParameter[] parameters = new SqlParameter[1];
        //        parameters[0] = new SqlParameter("@MOMId ", SqlDbType.Int);
        //        parameters[0].Value = MOMId;
        //        ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedMOMActionList", parameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.LogException(ex);
        //    }
        //    return ds;
        //}
        public int DeleteMomActonPointParticipants(int ActionPointId, string ParticipantsType, int UserId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@ActionPointId", SqlDbType.Int);
                parameters[0].Value = ActionPointId;

                parameters[1] = new SqlParameter("@ParticipantsType", SqlDbType.VarChar);
                parameters[1].Value = ParticipantsType;


                parameters[2] = new SqlParameter("@UserId", SqlDbType.Int);
                parameters[2].Value = UserId;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DeleteMOMActionPointParticipantList", parameters);
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
        public DataSet MOMAttachmentList(int MOMId)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@MOMId", SqlDbType.Int);
                parameters[0].Value = MOMId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_MOMAttachmentList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public int SaveParticipants(string PType, string[] selitems, int MOMID, string User_Id, bool FYI)
        {
            SqlParameter[] parameters = new SqlParameter[9];
            foreach(var item in selitems)
            { 
          
            parameters[0] = new SqlParameter("@MOMId", SqlDbType.Int);
            parameters[0].Value = MOMID;

            parameters[1] = new SqlParameter("@ParticipantId", SqlDbType.Int);
            parameters[1].Value = item;

            parameters[2] = new SqlParameter("@ParticipantType", SqlDbType.VarChar);
            parameters[2].Value = PType;

            parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            parameters[3].Value = User_Id;

            parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[4].Value = 0;
            parameters[4].Direction = ParameterDirection.InputOutput;

            parameters[5] = new SqlParameter("@FYI", SqlDbType.Bit);
            parameters[5].Value = FYI;
            SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveMOMParticipant", parameters);

          
         
          }
                        
            return Convert.ToInt32(parameters[4].Value.ToString());
        }

        public int SaveAttachments(string FileName, string Description, int MOMID)
        {
            SqlParameter[] parameters = new SqlParameter[9];
            parameters[0] = new SqlParameter("@MOMId", SqlDbType.Int);
            parameters[0].Value = MOMID;

            parameters[1] = new SqlParameter("@FileName", SqlDbType.VarChar);
            parameters[1].Value = FileName;

            parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
            parameters[2].Value = Description;
          
            parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[3].Value = 0;
            parameters[3].Direction = ParameterDirection.InputOutput;
            try
            {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveMOMAttachment", parameters);
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
        public int DeleteParticipants(int MOMID, int ParticipantId, string ParticipantsType)
        {
            SqlParameter[] parameters = new SqlParameter[9];
            parameters[0] = new SqlParameter("@MOMId", SqlDbType.Int);
            parameters[0].Value = MOMID;

            parameters[1] = new SqlParameter("@ParticipantId", SqlDbType.Int);
            parameters[1].Value = ParticipantId;

            parameters[2] = new SqlParameter("@ParticipantsType", SqlDbType.VarChar);
            parameters[2].Value = ParticipantsType;

            parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[3].Value = 0;
            parameters[3].Direction = ParameterDirection.InputOutput;
            try
            {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DeleteMOMParticipants", parameters);
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
        public DataSet GetMOMParticipendsDetailList(int MOMId )
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@MOMId", SqlDbType.Int);
                parameters[0].Value = MOMId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMMOMParticipantListByMoMId", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public DataSet GetProjectlistByUserId(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjectListForDrpDwnByUser", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        #endregion Project Task MOM Details

        #region MOMActionPoint
        public DataSet GetStatusCodeForMomActionPoint()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysStatusCodeforMOMActionPoint");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetMOMActionPointList(int MOMId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@MOMId", SqlDbType.Int);
                parameters[0].Value = MOMId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMgetMOMActionPointList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public DataSet MOMActionPointUserList()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMgetMOMActionPointUserList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedMOMActionPointList(int ActionPointId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ActionPointId ", SqlDbType.Int);
                parameters[0].Value = ActionPointId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMgetselectedMOMActionPointList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetselectedMOMactionPointParticipantList(int ActionPointId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ActionPointId ", SqlDbType.Int);
                parameters[0].Value = ActionPointId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_MOMActionPointParticipantList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveMOMActionPoint(DataSet ds, string User_Id, string[] selitems,string PType,string Comment)
        {

            int ActionPointId = 0;
            SqlParameter[] parameters = new SqlParameter[8];
            parameters[0] = new SqlParameter("@MOMId", SqlDbType.Int);
            parameters[0].Value = ds.Tables[0].Rows[0]["MOMId"];

            parameters[1] = new SqlParameter("@ActionPointId", SqlDbType.Int);
            parameters[1].Value = ds.Tables[0].Rows[0]["ActionPointId"];
            parameters[1].Direction = ParameterDirection.InputOutput;

            parameters[2] = new SqlParameter("@ActionDescription", SqlDbType.VarChar);
            parameters[2].Value = ds.Tables[0].Rows[0]["ActionDescription"];

            parameters[3] = new SqlParameter("@Status", SqlDbType.Int);
            parameters[3].Value = ds.Tables[0].Rows[0]["Status"];

            parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[4].Value = 0;
          

            parameters[5] = new SqlParameter("@user_Id", SqlDbType.NVarChar);
            parameters[5].Value = User_Id;

            parameters[6] = new SqlParameter("@DueDate", SqlDbType.SmallDateTime);
            parameters[6].Value = ds.Tables[0].Rows[0]["DueDate"];

            parameters[7] = new SqlParameter("@Comment", SqlDbType.VarChar);
            parameters[7].Value = Comment;
            try
            {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMsaveMOMActionPoint", parameters);
                ActionPointId = Convert.ToInt32(parameters[1].Value);
                if (ActionPointId != 0)
                {
                    int i = 0;
                    foreach (string UserId in selitems)
                    {
                        i++;
                        SqlParameter[] param = new SqlParameter[5];
                        param[0] = new SqlParameter("@ActionPointId", SqlDbType.Int);
                        param[0].Value = ActionPointId;

                        param[1] = new SqlParameter("@UserId", SqlDbType.Int);
                        param[1].Value = UserId;

                        param[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                        param[2].Value = 0;
                        param[2].Direction = ParameterDirection.InputOutput;

                        param[3] = new SqlParameter("@Delete", SqlDbType.Int);
                        param[3].Value = i;

                        param[4] = new SqlParameter("@UserType", SqlDbType.VarChar);
                        param[4].Value = PType;

                    
                        SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMsaveMOMActionPointUser", param);

                      
                    }
                }
                   
                    return Convert.ToInt32(parameters[1].Value.ToString());
               
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }
        #endregion MomActionPoint

        #region Project Expenses
        public DataSet GetselectedPaymentdetails(int PaymentId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@PaymentId", SqlDbType.Int);
                parameters[0].Value = PaymentId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMgetSelectedPayment", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetselectedPaymentdetailsList(int RefId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@RefId", SqlDbType.Int);
                parameters[0].Value = RefId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetSelectedPaymentList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet ProjectExpensesList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetProjectExpensesList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetSelectedProjectExpensesList(int ExpensesId)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ExpensesId", SqlDbType.Int);
                parameters[0].Value = ExpensesId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetSelectedProjectExpensesList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public int DeletePaymentDetails(int PaymentId, string Remark)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@PaymentId", SqlDbType.Int);
            parameters[0].Value = PaymentId;
            parameters[1] = new SqlParameter("@Errorcode", SqlDbType.Int);
            parameters[1].Value = 0;
            parameters[1].Direction = ParameterDirection.InputOutput;
            parameters[2] = new SqlParameter("@Remark", SqlDbType.VarChar);
            parameters[2].Value = Remark;
            SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TmDeletePayment", parameters);
            return (int)parameters[1].Value;
        }
        public int SavePaymentDetails( DataSet ds ,string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[9];
                parameters[0] = new SqlParameter("@PaymentId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["PaymentId"].ToString(); 

                parameters[1] = new SqlParameter("@PaymentType", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["PaymentType"].ToString();

                parameters[2] = new SqlParameter("@Amount", SqlDbType.Float);
                parameters[2].Value = ds.Tables[0].Rows[0]["Amount"].ToString();

                parameters[3] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[0].Rows[0]["Remark"].ToString();

                parameters[4] = new SqlParameter("@RefId ", SqlDbType.Int);
                parameters[4].Value = ds.Tables[0].Rows[0]["RefId"].ToString();

                parameters[5] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[5].Value = ds.Tables[0].Rows[0]["EmpId"].ToString();

                parameters[6] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[6].Value = User_Id;

                parameters[7] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[7].Value = 0;
                parameters[7].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSavePayment", parameters);
                errCode = (int)parameters[7].Value;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        public int SaveProjectExpenses(DataSet ds, string User_Id)
        {
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            int errcode = 0;
            SqlParameter[] parameters = new SqlParameter[13];
            parameters[0] = new SqlParameter("@ExpenseId", SqlDbType.Int);
            parameters[0].Value = ds.Tables[0].Rows[0]["ExpenseId"].ToString();
            parameters[0].Direction = ParameterDirection.InputOutput;

            parameters[1] = new SqlParameter("@ProjectId", SqlDbType.Int);
            parameters[1].Value = ds.Tables[0].Rows[0]["ProjectId"].ToString();

            parameters[2] = new SqlParameter("@TaskId", SqlDbType.Int);
            parameters[2].Value = ds.Tables[0].Rows[0]["TaskId"].ToString();

            parameters[3] = new SqlParameter("@Amount", SqlDbType.Float);
            parameters[3].Value = ds.Tables[0].Rows[0]["Amount"].ToString();

            parameters[4] = new SqlParameter("@Remark", SqlDbType.VarChar);
            parameters[4].Value = ds.Tables[0].Rows[0]["Remark"].ToString();

            parameters[5] = new SqlParameter("@DocumentPath", SqlDbType.VarChar);
            parameters[5].Value = ds.Tables[0].Rows[0]["DocumentPath"].ToString();

            parameters[6] = new SqlParameter("@Description", SqlDbType.VarChar);
            parameters[6].Value = ds.Tables[0].Rows[0]["Description"].ToString();

            parameters[7] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            parameters[7].Value = User_Id;

            parameters[8] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[8].Value = 0;
            parameters[8].Direction = ParameterDirection.InputOutput;

            parameters[9] = new SqlParameter("@ExpenseDate", SqlDbType.NVarChar);
            parameters[9].Value = ds.Tables[0].Rows[0]["ExpenseDate"];

            parameters[10] = new SqlParameter("@ExpTypeId", SqlDbType.Int);
            parameters[10].Value = ds.Tables[0].Rows[0]["ExpTypeId"].ToString();

            parameters[11] = new SqlParameter("@CustomerId", SqlDbType.Int);
            parameters[11].Value = ds.Tables[0].Rows[0]["CustomerId"].ToString();

            parameters[12] = new SqlParameter("@VendorId", SqlDbType.Int);
            parameters[12].Value = ds.Tables[0].Rows[0]["VendorId"].ToString();
            try
            {
                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_TMSaveProjectExpenses", parameters);
                if (parameters[0].Value != null)
                    errcode=Convert.ToInt32(parameters[0].Value.ToString());
               if(errcode > 0 && ds.Tables[0].Rows[0]["DocumentPath"].ToString() != "")
               {
                   if (!(SaveTMPrjectExpeDoc(errcode, ds, sqlTransaction) == 500002))
                   {
                       sqlTransaction.Rollback();
                       sqlConn.Close();                      
                   }
                   sqlTransaction.Commit();
                   sqlConn.Close();
               }
               else
               {
                   sqlTransaction.Commit();
                   sqlConn.Close();
               }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
            return errcode;
        }
        private int SaveTMPrjectExpeDoc(int ExpenseId, DataSet ds, SqlTransaction tr)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@ExpenseId", SqlDbType.Int);
                parameters[0].Value = ExpenseId;

                parameters[1] = new SqlParameter("@DocumentPath", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["DocumentPath"].ToString();

                parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["Description"].ToString();

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_TMSaveProjectExpenseDocs", parameters);
                errCode = (int)parameters[3].Value;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        #endregion Project Expenses

        #region Project Expenses Approval

        public DataSet ProjectExpensesApprovalList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMProjectExpensesPendingApprovalByUser", parameters);               
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int ExpenseSendForApprove(int statusCode, string User_Id, int ExpenseId)
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

                parameters[3] = new SqlParameter("@ExpenseId", SqlDbType.Int);
                parameters[3].Value = ExpenseId;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_ExpenseSendForApprove", parameters);
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
        public int SaveApvRejExpenses(int ExpenseId, string ManagerRemark,int Status,string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@ExpenseId", SqlDbType.Int);
                parameters[0].Value = ExpenseId;

                parameters[1] = new SqlParameter("@ManagerRemark", SqlDbType.VarChar);
                parameters[1].Value = ManagerRemark;

                parameters[2] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[2].Value = Status;

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveExpensesApproveRejectByAccounts", parameters);
                errCode = (int)parameters[4].Value;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion Project Expenses Approval

        #region Project Expenses Approval by Account

        public DataSet ProjectExpensesApprovalListbyAccount(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetExpensesListbyAccountHead", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public DataSet GetSelectedProjectExpensesListbyAccount(int ExpensesId)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ExpensesId", SqlDbType.Int);
                parameters[0].Value = ExpensesId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetSelectedExpensesListbyAccountHead", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public int SaveApvRejExpensesByAccounts(int ExpenseId, string ManagerRemark, int Status, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];
                parameters[0] = new SqlParameter("@ExpenseId", SqlDbType.Int);
                parameters[0].Value = ExpenseId;

                parameters[1] = new SqlParameter("@ManagerRemark", SqlDbType.VarChar);
                parameters[1].Value = ManagerRemark;

                parameters[2] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[2].Value = Status;

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveExpensesApproveReject", parameters);
                errCode = (int)parameters[4].Value;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        #endregion Project Expenses Approval by Account

        #region Expense Type

        public DataSet GetExpenseTypeList()
        {

            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetExpenseTypeList");
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetSelectedExpenseType(int expenseId)
        {
            DataSet dsData = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ExpTypeId", SqlDbType.Int);
                parameters[0].Value = expenseId;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetSelectedExpenseType", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsData;
        }



        public int SaveExpenseType(DataSet ds,string User_id)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@ExpTypeId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["ExpTypeId"];

                parameters[1] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["Description"].ToString();

                parameters[2] = new SqlParameter("@ExpenceType", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["ExpenceType"].ToString();
                             
                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveExpenseType", parameters);
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
        #endregion Expense Type

        #region Project Equipment and Items
        public int SaveProjectEquipment(int projectId,int EquipmentId,double Quantity)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@projectId", SqlDbType.Int);
                parameters[0].Value = projectId;

                parameters[1] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                parameters[1].Value = EquipmentId;

                parameters[2] = new SqlParameter("@Quantity", SqlDbType.Float);
                parameters[2].Value = Quantity;
                
                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveProjectEquipment", parameters);
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
        public DataSet ProjectEquipmentList(int projectId)
        {
            DataSet ds = new DataSet();
            try
            {

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = projectId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetProjectEquipmentList", parameters);                
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public int SaveProjectItems(int projectId, int ItemId, double Quantity,double TAM)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@projectId", SqlDbType.Int);
                parameters[0].Value = projectId;

                parameters[1] = new SqlParameter("@ItemId", SqlDbType.Int);
                parameters[1].Value = ItemId;

                parameters[2] = new SqlParameter("@Quantity", SqlDbType.Float);
                parameters[2].Value = Quantity;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                parameters[4] = new SqlParameter("@TAM", SqlDbType.Float);
                parameters[4].Value = TAM;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMSaveProjectItems", parameters);
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
        public DataSet ProjectItemList(int projectId)
        {
            DataSet ds = new DataSet();
            try
            {

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ProjectId", SqlDbType.Int);
                parameters[0].Value = projectId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMGetProjectItemsList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        #endregion Project Equipment  and Items
    }
   
}
