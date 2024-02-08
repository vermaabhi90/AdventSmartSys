using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SmartSys.DAL;
using SmartSys.DAL.Project;
using System.Collections;
using SmartSys.Utility;
using SmartSys.BL.TimeManagement;


namespace SmartSys.BL.Project
{
    public class ProjectBL
    {
        public int SaveProjectType(ProjectTypeModel ProjectModel,string User_Id)
        {
            try
            {
                ProjectDal objDL = new ProjectDal();
                return objDL.SaveProjectType(ProjectModel.ProjectTypeId, ProjectModel.ProjectType, ProjectModel.isActive, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
                return 0;
            }
        }
        public int DeleteProject(int ProjectId)
        {
            try
            {
                ProjectDal objDL = new ProjectDal();
                return objDL.DeleteProject(ProjectId);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public List<ProjectTypeModel> GetProjectTypeData()
        {
            List<ProjectTypeModel> lstProjectType = new List<ProjectTypeModel>();
            ProjectDal objProj = new ProjectDal();
            DataSet dsData = objProj.GetProjectTypeList();
            if(dsData!=null)
                if(dsData.Tables.Count>0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        ProjectTypeModel project = new ProjectTypeModel();
                        project.ProjectTypeId = Convert.ToInt32(dr["ProjectTypeId"].ToString());
                        project.ProjectType = dr["ProjectType"].ToString();
                        project.isActive = Convert.ToBoolean(dr["isActive"].ToString());
                        project.ProjectType = dr["ProjectType"].ToString();
                        project.CreatedByName = dr["CreatedByName"].ToString();
                        project.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        project.ModifiedByName = dr["ModifiedByName"].ToString();
                        project.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        lstProjectType.Add(project);
                    }
            return lstProjectType;
        }

        public ProjectTypeMaster GetProjectTypeTaskData(int ProjectTypeId)
        {
            ProjectTypeMaster objMast = new ProjectTypeMaster();
            List<ProjectTypeTaskDetails> tasklst = new List<ProjectTypeTaskDetails>();
            ProjectDal objProj = new ProjectDal();
            DataSet dsData = objProj.GetProjectTypeTaskList(ProjectTypeId);

            if (dsData != null)
                if (dsData.Tables.Count > 1)
                {
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsData.Tables[0].Select("ParentTaskId=0"))
                        {
                            ProjectTypeTaskDetails task = new ProjectTypeTaskDetails();
                            task.Duration = Convert.ToInt32(dr["Duration"].ToString());
                            task.EndDate = dr["EndDate"].ToString();
                            task.StartDate = dr["StartDate"].ToString();
                            task.ProjectTypeId = ProjectTypeId;
                            task.TaskName = dr["TaskName"].ToString();
                            task.ParentTaskId = Convert.ToInt32(dr["ParentTaskId"].ToString());
                            task.TaskType = Convert.ToInt16(dr["TaskType"].ToString());
                            task.ProjectTypeTaskID = Convert.ToInt32(dr["ProjectTypeTaskId"].ToString());
                            task.Predecessors = dr["Predecessors"].ToString();
                            task.Description = dr["Description"].ToString();
                            task.TaskTypeDesc = dr["TaskTypeDesc"].ToString();
                            task.Progress = Convert.ToInt32(dr["Progress"].ToString());
                            task.SubTasks = getSubTaskofProjectType(dsData, Convert.ToInt32(dr["ProjectTypeTaskId"]));
                            tasklst.Add(task);
                        }
                        objMast.ProjectTypeTasks = tasklst;
                        objMast.ProjectTypeId = ProjectTypeId;
                        objMast.startdate = Convert.ToDateTime(dsData.Tables[1].Rows[0]["ParentStartDate"].ToString());
                        objMast.enddate = Convert.ToDateTime(dsData.Tables[1].Rows[0]["ParentEndDate"].ToString());
                    }
                }
            return objMast;

        }

        private List<ProjectTypeTaskDetails> getSubTaskofProjectType(DataSet dsData, Int32 ParentTaskId)
        {
            List<ProjectTypeTaskDetails> subtasks = new List<ProjectTypeTaskDetails>();
            foreach (DataRow dr in dsData.Tables[0].Select("ParentTaskId=" + ParentTaskId.ToString()))
            {
                ProjectTypeTaskDetails newtask = new ProjectTypeTaskDetails();
                newtask.Duration = Convert.ToInt32(dr["Duration"].ToString());
                newtask.EndDate = dr["EndDate"].ToString();
                newtask.StartDate = dr["StartDate"].ToString();
                newtask.TaskName = dr["TaskName"].ToString();
                newtask.Description = dr["Description"].ToString();
                newtask.ParentTaskId = ParentTaskId;
                newtask.ProjectTypeTaskID = Convert.ToInt32(dr["ProjectTypeTaskId"].ToString());
                newtask.Progress = Convert.ToInt32(dr["Progress"].ToString());
                newtask.Predecessors = dr["Predecessors"].ToString();
                newtask.TaskTypeDesc = dr["TaskTypeDesc"].ToString();
                newtask.TaskType = Convert.ToInt16(dr["TaskType"].ToString());
                newtask.SubTasks = getSubTaskofProjectType(dsData, Convert.ToInt32(dr["ProjectTypeTaskId"]));
                subtasks.Add(newtask);
            }
            return subtasks;
        }

        public void DeleteProjectTask(TaskDetails task, string User_Id)
        {
            try
            {
                ArrayList taskidlist = new ArrayList();
                taskidlist.Add(task.TaskID);
                taskidlist = getSubProjectTaskIds(taskidlist, task);
                ProjectDal objDL = new ProjectDal();
                objDL.DeleteProjectTask(task.ProjectId, taskidlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProjectTypeTask(ProjectTypeTaskDetails task, string User_Id)
        {
            try
            {
                ArrayList taskidlist = new ArrayList();
                taskidlist.Add(task.ProjectTypeTaskID);
                taskidlist = getSubProjectTypeTaskIds(taskidlist, task);
                ProjectDal objDL = new ProjectDal();
                objDL.DeleteProjectTypeTask(task.ProjectTypeId, taskidlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ArrayList getSubProjectTaskIds(ArrayList taskidlist, TaskDetails task)
        {
            if (task.SubTasks != null)
                foreach (TaskDetails st in task.SubTasks)
                {
                    taskidlist.Add(st.TaskID);
                    getSubProjectTaskIds(taskidlist, st);
                }
            return taskidlist;
        }

        private ArrayList getSubProjectTypeTaskIds(ArrayList taskidlist, ProjectTypeTaskDetails task)
        {
            if (task.SubTasks != null)
                foreach (ProjectTypeTaskDetails st in task.SubTasks)
                {
                    taskidlist.Add(st.ProjectTypeTaskID);
                    getSubProjectTypeTaskIds(taskidlist, st);
                }
                return taskidlist;
        }
        public void SaveProjectTypeTask(ProjectTypeTaskDetails task, string User_Id)
        {
            try
            {
                ProjectDal objDL = new ProjectDal();
                ArrayList projTypeParent = new ArrayList();
                if (task.SubTasks != null)
                    foreach (ProjectTypeTaskDetails p in task.SubTasks)
                    {
                        int[] t = new int[2];
                        t[0] = p.ProjectTypeTaskID;
                        t[1] = task.ProjectTypeTaskID;
                        projTypeParent.Add(t);
                    }
                objDL.SaveProjectTypeTask(task.ProjectTypeId, task.ProjectTypeTaskID, task.TaskName, task.Duration, Convert.ToDateTime(task.StartDate.Substring(0, task.StartDate.IndexOf(":") - 3)), Convert.ToDateTime(task.EndDate.Substring(0, task.EndDate.IndexOf(":") - 3)), task.ParentTaskId, task.Predecessors, User_Id, projTypeParent,task.Description,task.Progress);

            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        public List<DrpItem> GetProjectTaskCustomerList(int ProjectId, int TaskId)
        {
            List<DrpItem> CustList = new List<DrpItem>();
            ProjectDal dalobj = new ProjectDal();
            DataSet dsdata = dalobj.GetProjectTaskCustomerList(ProjectId, TaskId);
            if(dsdata!=null)
                if(dsdata.Tables.Count>0)
                    foreach(DataRow dr in dsdata.Tables[0].Rows)
                    {
                        DrpItem itm = new DrpItem();
                        itm.Id = Convert.ToInt32(dr["CustomerId"].ToString());
                        itm.DisplayName = dr["CustomerName"].ToString();
                        if (Convert.ToInt32(dr["Selected"].ToString()) == 0)
                            itm.Selected = false;
                        else
                            itm.Selected = true;
                        CustList.Add(itm);
                    }
            return CustList;
        }

        public List<DrpItem> GetCustomerListByUser(string User_Id)
        {
            List<DrpItem> CustList = new List<DrpItem>();
            ProjectDal dalobj = new ProjectDal();
            DataSet dsdata = dalobj.GetCustomerListByUser(User_Id,0,0);
            if (dsdata != null)
                if (dsdata.Tables.Count > 0)
                    foreach (DataRow dr in dsdata.Tables[0].Rows)
                    {
                        DrpItem itm = new DrpItem();
                        itm.Id = Convert.ToInt32(dr["CustomerId"].ToString());
                        itm.DisplayName = dr["CustomerName"].ToString();
                        CustList.Add(itm);
                    }
            return CustList;
        }
        public List<DrpItem> GetCustomerListForTM(string User_Id,int ProjectId,int TaskId)
        {
            List<DrpItem> CustList = new List<DrpItem>();
            ProjectDal dalobj = new ProjectDal();
            DataSet dsdata = dalobj.GetCustomerListByUser(User_Id, ProjectId,TaskId);
            if (dsdata != null)
                if (dsdata.Tables.Count > 0)
                    foreach (DataRow dr in dsdata.Tables[0].Rows)
                    {
                        DrpItem itm = new DrpItem();
                        itm.Id = Convert.ToInt32(dr["CustomerId"].ToString());
                        itm.DisplayName = dr["CustomerName"].ToString();
                        CustList.Add(itm);
                    }
            return CustList;
        }
        public List<DrpItem> GetProjectTaskVendorList(int ProjectId, int TaskId, string User_Id)
        {
            List<DrpItem> VendList = new List<DrpItem>();
            ProjectDal dalobj = new ProjectDal();
            DataSet dsdata = dalobj.GetVendorListByUser(User_Id,ProjectId, TaskId);
            if (dsdata != null)
                if (dsdata.Tables.Count > 0)
                    foreach (DataRow dr in dsdata.Tables[0].Rows)
                    {
                        DrpItem itm = new DrpItem();
                        itm.Id = Convert.ToInt32(dr["VendorId"].ToString());
                        itm.DisplayName = dr["VendorName"].ToString();                       
                        VendList.Add(itm);
                    }
            return VendList;
        }

        public List<DrpItem> GetVendorListByUser(string User_Id)
        {
            List<DrpItem> VendList = new List<DrpItem>();
            ProjectDal dalobj = new ProjectDal();
            DataSet dsdata = dalobj.GetVendorListByUser(User_Id,0,0);
            if (dsdata != null)
                if (dsdata.Tables.Count > 0)
                    foreach (DataRow dr in dsdata.Tables[0].Rows)
                    {
                        DrpItem itm = new DrpItem();
                        itm.Id = Convert.ToInt32(dr["VendorId"].ToString());
                        itm.DisplayName = dr["VendorName"].ToString();
                        VendList.Add(itm);
                    }
            return VendList;
        }
        public List<DrpItem> GetQuotationlistForCustomer(int CustomerId)
        {
            List<DrpItem> CustList = new List<DrpItem>();
            ProjectDal dalobj = new ProjectDal();
            DataSet dsdata = dalobj.GetQuotationlistForCustomer(CustomerId);
            if (dsdata != null)
                if (dsdata.Tables.Count > 0)
                    foreach (DataRow dr in dsdata.Tables[0].Rows)
                    {
                        DrpItem itm = new DrpItem();
                        if (dsdata.Tables[0].Rows[0]["QuotationId"].ToString() != "")                       
                       {
                            itm.Id = Convert.ToInt32(dr["QuotationId"].ToString());
                            itm.DisplayName = dr["QuotationNumber"].ToString();
                            CustList.Add(itm);
                        }
                    }
            return CustList;
        }
        public int UpdateQuotStatus(int QuotId, string User_Id)
        {
                        
            try
            {
                ProjectDal objDL = new ProjectDal();
                return objDL.UpdateQuotStatus(QuotId, User_Id);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public TaskDetails GetSelectedProjectTask(int ProjectId, int TaskId)
        {
            TaskDetails ProjectTskModel = new TaskDetails();
            ProjectTskModel.LstProjectTaskDoc = new List<ProcjectDocModel>();
            DataSet ds = new DataSet();
            ProjectDal dalobj = new ProjectDal();
            ds = dalobj.GetSelectedProjectDocList(ProjectId, TaskId);
            if (ds != null)
            {
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ProjectTskModel.ProjectId = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectId"].ToString());
                        ProjectTskModel.TaskID = Convert.ToInt32(ds.Tables[0].Rows[0]["TaskID"].ToString());
                        ProjectTskModel.TaskName = ds.Tables[0].Rows[0]["TaskName"].ToString();
                        ProjectTskModel.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                        ProjectTskModel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        ProjectTskModel.ProjectName = ds.Tables[0].Rows[0]["ProjectName"].ToString();
                        ProjectTskModel.Progress =Convert.ToInt32(ds.Tables[0].Rows[0]["Progress"].ToString());
                        if (ds.Tables[0].Rows[0]["EstimationTime"].ToString() != "")
                        {
                            ProjectTskModel.EstimationTime = ds.Tables[0].Rows[0]["EstimationTime"].ToString();
                            ProjectTskModel.EstimationHH = Convert.ToInt32(ProjectTskModel.EstimationTime) / 60;
                            ProjectTskModel.EstimationMM = Convert.ToInt32(ProjectTskModel.EstimationTime) % 60;
                        }
                        else
                        {
                            ProjectTskModel.EstimationTime ="0";
                            ProjectTskModel.EstimationHH = 0;
                            ProjectTskModel.EstimationHH = 0;
                        }
                        if (ds.Tables[0].Rows[0]["Status"].ToString() == "")
                        {
                            ProjectTskModel.Statuscode = 0;
                        }
                        else
                        {
                            ProjectTskModel.Statuscode = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"].ToString());
                        }
                        ProjectTskModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        if (ds.Tables[0].Rows[0]["CustomerId"].ToString() != "")
                            ProjectTskModel.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString());
                        if (ds.Tables[0].Rows[0]["VendorId"].ToString() != "")
                            ProjectTskModel.VendorId = Convert.ToInt32(ds.Tables[0].Rows[0]["VendorId"].ToString());
                        ProjectTskModel.ProjCustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectCustomer"].ToString());
                        ProjectTskModel.ProjVendorId = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectVendor"].ToString());                
                    }
                }
            }
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                ProcjectDocModel newtask = new ProcjectDocModel();
                newtask.FileName = dr["FileName"].ToString();
                newtask.Description = dr["Description"].ToString();
                newtask.ModifiedBy = dr["ModifiedBy"].ToString();
                newtask.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                ProjectTskModel.LstProjectTaskDoc.Add(newtask);
            }
            if (ds.Tables.Count > 2)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    ProjectTskModel.TotalChild =Convert.ToInt32(ds.Tables[2].Rows[0]["TotalChild"].ToString());
                    if(ProjectTskModel.TotalChild > 0)
                    {
                        if (Convert.ToInt32(ds.Tables[2].Rows[0]["EstimationTime"].ToString()) > 0)
                        {
                            ProjectTskModel.EstimationTime = ds.Tables[2].Rows[0]["EstimationTime"].ToString();
                            ProjectTskModel.EstimationHH = Convert.ToInt32(ProjectTskModel.EstimationTime) / 60;
                            ProjectTskModel.EstimationMM = Convert.ToInt32(ProjectTskModel.EstimationTime) % 60;
                        }
                        else
                        {
                            ProjectTskModel.EstimationTime = "0";
                            ProjectTskModel.EstimationHH = 0;
                            ProjectTskModel.EstimationHH = 0;
                        }
                    }

                }
            }
            return ProjectTskModel;
        }

        public int SaveProject(ProjectModel p,string User_Id)
        {
            ProjectDal objDL = new ProjectDal();
            return objDL.SaveProject(p.ProjectId,p.ProjectTypeId,p.ProjectManagerId,p.ProjectName,p.StatusId,p.Region,p.Description,p.Remark,User_Id, p.StartDate,p.CustomerId,p.VendorId,p.CompCode, p.SegmentId);
        }

        #region Risk Task
        public int UpdateRiskTaskReview(int statusCode,int ProjectId, int TaskID, string User_Id)
        {
            int errorcode = 0;
            try
            {
                ProjectDal objDL = new ProjectDal();
                errorcode = objDL.UpdateRiskTaskReviewDetail(statusCode,ProjectId, TaskID, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errorcode;
        }
        public int UpdateRiskTaskApproval(int statusCode, int ProjectId, int TaskID, string User_Id)
        {
            int errorcode = 0;
            try
            {
                ProjectDal objDL = new ProjectDal();
                errorcode = objDL.UpdateRiskTaskApprovalDetail(statusCode, ProjectId, TaskID, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errorcode;
        }
        public List<TaskDetails> GetRiskReviewList(string User_Id)
        {
            List<TaskDetails> lstRisk = new List<TaskDetails>();
            DataSet ds = new DataSet();
            try
            {
                ProjectDal objDL = new ProjectDal();
                ds = objDL.GetRiskReviewList(User_Id);
                if (ds == null) return null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    TaskDetails Model = new TaskDetails();
                    Model.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                    Model.TaskID = Convert.ToInt32(dr["TaskID"].ToString());
                    Model.TaskName = dr["RiskTaskName"].ToString();
                    Model.TskType = dr["TSKType"].ToString();
                    Model.StartDate = dr["StartDate"].ToString();
                    Model.Duration = Convert.ToInt32(dr["Duration"].ToString());
                    Model.EndDate = dr["EndDate"].ToString();
                    Model.Status = dr["StatusShortCode"].ToString();
                    Model.Description = dr["Description"].ToString();
                    Model.ModifiedBy = dr["ModifiedBy"].ToString();
                    Model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                    lstRisk.Add(Model);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstRisk;
        }
        public List<TaskDetails> GetRiskApprovalList( string User_Id)
        {
            List<TaskDetails> lstRisk = new List<TaskDetails>();
            DataSet ds = new DataSet();
            try
            {
                ProjectDal objDL = new ProjectDal();
                ds = objDL.GetRiskApprovalList(User_Id);
                if (ds == null) return null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    TaskDetails Model = new TaskDetails();
                    Model.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                    Model.TaskID = Convert.ToInt32(dr["TaskID"].ToString());
                    Model.TaskName = dr["RiskTaskName"].ToString();
                    Model.TskType = dr["TSKType"].ToString();
                    Model.StartDate = dr["StartDate"].ToString();
                    Model.Duration = Convert.ToInt32(dr["Duration"].ToString());
                    Model.EndDate = dr["EndDate"].ToString();
                    Model.Status = dr["StatusShortCode"].ToString();
                    Model.Description = dr["Description"].ToString();
                    Model.ModifiedBy = dr["ModifiedBy"].ToString();
                    Model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                    lstRisk.Add(Model);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstRisk;
        }
        public int SaveRiskTask(int ProjectId, string TaskName, string StartDate, string EndDate, string Description, string ResourceIDs, int TaskID, int Duration, int StatusCode, string Comments, int CommentId, int TaskType, string User_Id, int progress, int VendorId,int CustomerId)
        {
            int errorcode = 0;
            try
            {
                 ProjectDal objDL = new ProjectDal();
                 errorcode = objDL.SaveRiskTask(ProjectId, TaskName, StartDate, EndDate, Description, ResourceIDs, TaskID, Duration, StatusCode, Comments, CommentId, TaskType, User_Id, progress, VendorId,CustomerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errorcode;
        }
        public List<TaskDetails> GetRiskList(string User_Id )
        {
            List<TaskDetails> lstRisk = new List<TaskDetails>();
            DataSet ds = new DataSet();
            try
            {
                ProjectDal objDL = new ProjectDal();
                ds = objDL.GetProjectRiskList(User_Id);
               if (ds == null) return null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                   
                    TaskDetails Model = new TaskDetails();
                    Model.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                    Model.TaskID = Convert.ToInt32(dr["TaskID"].ToString());
                    Model.TaskName = dr["RiskTaskName"].ToString();
                    Model.TskType = dr["tsktype"].ToString();
                    Model.Status = dr["StatusShortCode"].ToString();
                    if (dr["StatusShortCode"].ToString()=="NEW")
                    if(dr["UserName"].ToString()== dr["ModifiedBy"].ToString())
                    {
                        Model.Edit = "Edit";
                    }
                    var StartDate = Convert.ToDateTime(dr["StartDate"].ToString());  // Return 00/00/0000 00:00:00
                    var dateOnlyString = StartDate.ToShortDateString(); //Return 00/00/0000
                    Model.StartDate = dateOnlyString;

                    Model.Duration = Convert.ToInt32(dr["Duration"].ToString());
                    var EndDate = Convert.ToDateTime(dr["EndDate"].ToString());  // Return 00/00/0000 00:00:00
                    var EndDte = EndDate.ToShortDateString(); //Return 00/00/0000
                    Model.EndDate = EndDte;

                    Model.Status = dr["StatusShortCode"].ToString();
                    Model.Description = dr["Description"].ToString();
                    Model.ModifiedBy = dr["ModifiedBy"].ToString();
                    Model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                    lstRisk.Add(Model);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstRisk;
        }

        public List<TaskDetails> GetCaseDetailList(string UserName, string UserID)
        {
            List<TaskDetails> lstCase = new List<TaskDetails>();
            DataSet ds = new DataSet();
            try
            {
                ProjectDal objDL = new ProjectDal();
                ds = objDL.GetCaseRiskList(UserID);
                if (ds == null) return null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    if (dr["AssignedTome"].ToString() != "NotAssigned")
                    {
                        TaskDetails Model = new TaskDetails();

                        Model.TaskID = Convert.ToInt32(dr["TaskID"].ToString());
                        Model.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                        Model.TaskName = dr["TaskName"].ToString();
                        Model.ProjectName = dr["ProjectName"].ToString();
                        var StartDate = Convert.ToDateTime(dr["StartDate"].ToString());  // Return 00/00/0000 00:00:00
                        var dateOnlyString = StartDate.ToShortDateString(); //Return 00/00/0000
                        Model.StartDate = dateOnlyString;
                        Model.Resources = dr["Resources"].ToString();
                        Model.ApprovedBy = dr["ApprovedBy"].ToString();
                        Model.ReviewedBy = dr["ReviewedBy"].ToString();
                        Model.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                        Model.ReviewedDate = Convert.ToDateTime(dr["ReviewedDate"].ToString());
                        var EndDate = Convert.ToDateTime(dr["EndDate"].ToString());  // Return 00/00/0000 00:00:00
                        var EndDte = EndDate.ToShortDateString(); //Return 00/00/0000
                        Model.EndDate = EndDte;

                        Model.Status = dr["StatusShortCode"].ToString();
                        Model.AssignedTome = dr["AssignedTome"].ToString();
                        Model.ModifiedByName = dr["ModifiedBy"].ToString();
                        if (Model.ModifiedByName == UserName && (dr["AssignedTome"].ToString()=="AssignedToMe"))
                        {
                            Model.AssignedTome = "";
                            Model.AssignedByMe = "AssignedByMe";
                        }
                        Model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        lstCase.Add(Model);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstCase;
        }
        public List<TaskDetails> GetRiskDetailList(string UserName, string UserID)
        {
            List<TaskDetails> lstCaseRisk = new List<TaskDetails>();
            DataSet ds = new DataSet();
            try
            {
                ProjectDal objDL = new ProjectDal();
                ds = objDL.GetCaseRiskList(UserID);
                if (ds == null) return null;

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    if (dr["AssignedTome"].ToString() != "NotAssigned")
                    {
                        TaskDetails Model = new TaskDetails();
                        Model.TaskID = Convert.ToInt32(dr["TaskID"].ToString());
                        Model.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                        Model.TaskName = dr["TaskName"].ToString();
                        Model.ProjectName = dr["ProjectName"].ToString();
                        var StartDate = Convert.ToDateTime(dr["StartDate"].ToString());  // Return 00/00/0000 00:00:00
                        var dateOnlyString = StartDate.ToShortDateString(); //Return 00/00/0000
                        Model.StartDate = dateOnlyString;
                        Model.ApprovedBy = dr["ApprovedBy"].ToString();
                        Model.ReviewedBy = dr["ReviewedBy"].ToString();
                        Model.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                        Model.ReviewedDate = Convert.ToDateTime(dr["ReviewedDate"].ToString());
                        Model.AssignedTome = dr["AssignedTome"].ToString();
                        var EndDate = Convert.ToDateTime(dr["EndDate"].ToString());  // Return 00/00/0000 00:00:00
                        var EndDte = EndDate.ToShortDateString(); //Return 00/00/0000
                        Model.EndDate = EndDte;

                        Model.Status = dr["StatusShortCode"].ToString();
                        Model.ModifiedByName = dr["ModifiedBy"].ToString();
                        if (Model.ModifiedByName == UserName  && (dr["AssignedTome"].ToString()=="AssignedToMe"))
                        {
                            Model.AssignedTome = "";
                            Model.AssignedByMe = "AssignedByMe";
                        }
                        Model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        lstCaseRisk.Add(Model);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstCaseRisk;
        }
        public TaskDetails GetSelectedRiskList(int ProjectId, int TaskId )
        {
            TaskDetails Model = new TaskDetails();
            DataSet ds = new DataSet();
            try
            {
                Model.LstProjectTaskDoc = new List<ProcjectDocModel>();
                ProjectDal objDL = new ProjectDal();
                ds = objDL.GetSelectedRiskTaskList(ProjectId, TaskId);
                if (ds == null) return null;

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
     
                            Model.ProjectId = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectId"].ToString());
                            Model.TaskID = Convert.ToInt32(ds.Tables[0].Rows[0]["TaskID"].ToString());
                            Model.TaskName = ds.Tables[0].Rows[0]["RiskTaskName"].ToString();
                            Model.ProjectName = ds.Tables[0].Rows[0]["ProjectName"].ToString();
                            Model.TskType = ds.Tables[0].Rows[0]["TskName"].ToString();
                            Model.StartDate = ds.Tables[0].Rows[0]["StartDate"].ToString();
                            Model.Resources = ds.Tables[0].Rows[0]["Resources"].ToString();
                            Model.Status = ds.Tables[0].Rows[0]["StatusShortCode"].ToString();
                            Model.ProjVendorName = ds.Tables[0].Rows[0]["VendorName"].ToString();
                            Model.ProjCustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                            if (ds.Tables[0].Rows[0]["VendorId"].ToString()!="")
                            {
                                Model.VendorId = Convert.ToInt32(ds.Tables[0].Rows[0]["VendorId"].ToString());
                            }
                            if (ds.Tables[0].Rows[0]["CustomerId"].ToString()!="")
                            {
                                Model.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString());
                            }
                           
                            var StartDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString());  // Return 00/00/0000 00:00:00
                            var dateOnlyString = StartDate.ToShortDateString(); //Return 00/00/0000
                            Model.StartDate = dateOnlyString;

                            Model.Employee = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());

                            Model.Statuscode = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"].ToString());
                            Model.Progress = Convert.ToInt32(ds.Tables[0].Rows[0]["Progress"].ToString());
                           
                            var EndDate =  Convert.ToDateTime(ds.Tables[0].Rows[0]["EndDate"].ToString());  // Return 00/00/0000 00:00:00
                            var EndDte = EndDate.ToShortDateString(); //Return 00/00/0000
                            Model.EndDate = EndDte;

                            Model.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                            Model.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            Model.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                            Model.ResourceID = new List<object>();
                            foreach(DataRow dr in ds.Tables[0].Rows)
                            {
                                Model.ResourceID.Add(dr["EmpId"]);
                            }
                         }
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            ProcjectDocModel newtask = new ProcjectDocModel();
                            newtask.FileName = dr["FileName"].ToString();
                            newtask.Description = dr["Description"].ToString();
                            newtask.ModifiedBy = dr["ModifiedBy"].ToString();
                            newtask.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            Model.LstProjectTaskDoc.Add(newtask);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Model;
        }
        public ProjectCustomerAndVendor GetProjectCustomerAndVendor(int ProjectId)
        {
            ProjectCustomerAndVendor Model = new ProjectCustomerAndVendor();
            DataSet ds = new DataSet();
            try
            {

                ProjectDal objDL = new ProjectDal();
                ds = objDL.GetProjectVendorAndCustomer(ProjectId);
                if (ds == null) return null;

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Model.ProjCustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                            Model.ProjVendorName = ds.Tables[0].Rows[0]["VendorName"].ToString();

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Model;
        }
        public List<Commentmodel> GetSelectedProjectCommentList(int ProjectId, int TaskId)
        {
            TaskDetails taskmodel = new TaskDetails();
            taskmodel.LstCommentList = new List<Commentmodel>();
            DataSet ds = new DataSet();
            try
            {
                ProjectDal objDL = new ProjectDal();
                ds = objDL.GetSelectedProjectCommentList(ProjectId, TaskId);
                if (ds == null) return null;

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                Commentmodel Model = new Commentmodel();
                                Model.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                                Model.CommentId = Convert.ToInt32(dr["CommentId"].ToString());
                                Model.TaskID = Convert.ToInt32(dr["TaskID"].ToString());
                                Model.Comments = dr["Comment"].ToString();
                                Model.CommentedBy = dr["CommentedBy"].ToString();
                                Model.CommentDate = Convert.ToDateTime(dr["CommentDate"].ToString());
                                Model.Status = dr["Status"].ToString();
                                Model.User_Id = dr["User_Id"].ToString();
                                if (ds.Tables.Count > 1)
                                {
                                    if (ds.Tables[1].Rows.Count > 0)
                                        Model.toMail = ds.Tables[1].Rows[0]["to_MailList"].ToString();
                                }
                                taskmodel.LstCommentList.Add(Model);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return taskmodel.LstCommentList;
        }
        #endregion Risk Task
        
        public void SaveProjectTask(TaskDetails task, string User_Id)
        {
            try
            {
                ArrayList Resource = new ArrayList();

                if (task.ResourceID != null)
                {
                    for (int i = 0; i < task.ResourceID.Count; i++)
                        Resource.Add(Convert.ToInt32(task.ResourceID[i].ToString()));
                }
                ProjectDal objDL = new ProjectDal();
                ArrayList projParent = new ArrayList();
                if (task.SubTasks != null)
                    foreach (TaskDetails p in task.SubTasks)
                    {
                        int[] t = new int[2];
                        t[0] = p.TaskID;
                        t[1] = task.TaskID;
                        projParent.Add(t);
                    }
                objDL.SaveProjectTask(task.ProjectId, task.TaskID, task.TaskName, task.Duration, Convert.ToDateTime(task.StartDate.Substring(0, task.StartDate.IndexOf(":") - 3)), Convert.ToDateTime(task.EndDate.Substring(0, task.EndDate.IndexOf(":") - 3)), task.ParentTaskId, task.Predecessors, User_Id, Resource, task.Progress, 1, projParent, task.Description);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public List<ProjectModel> GetProjectList(string User_Id)
        {
            List<ProjectModel> lstProject = new List<ProjectModel>();
            ProjectDal objDL = new ProjectDal();
            DataSet dsData = objDL.GetProjectList(User_Id);
             if(dsData != null)
                if(dsData.Tables.Count>0)
                    foreach(DataRow dr in dsData.Tables[0].Rows)
                    {
                        ProjectModel proj = new ProjectModel();
                        proj.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                        proj.ProjectName = dr["ProjectName"].ToString();
                        proj.ProjectTypeId = Convert.ToInt32(dr["ProjectTypeId"].ToString());
                        proj.Description = dr["Description"].ToString();
                        proj.VendorName = dr["VendorName"].ToString();
                        proj.CustomerName = dr["CustomerName"].ToString();
                        proj.StatusDescription = dr["StatusDescription"].ToString();
                        proj.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                        proj.ItemPermission = dr["ItemPermission"].ToString();
                        proj.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                        proj.ProjectManager = dr["ProjectManager"].ToString();
                        proj.Region = dr["Region"].ToString();
                        proj.Email = dr["emailId"].ToString();
                        proj.Remark = dr["Remark"].ToString();
                        if (dr["CustomerId"].ToString() != "")
                        proj.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                        if (dr["VendorId"].ToString() != "")
                            proj.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                        if (dr["CompCode"].ToString() != "")
                            proj.CompCode = dr["CompCode"].ToString();
                        if (dr["SegmentId"].ToString() != "")
                            proj.SegmentId = Convert.ToInt32(dr["SegmentId"].ToString());
                        proj.SegmentName = dr["SegmentName"].ToString();
                        proj.ProjectManagerId = Convert.ToInt32(dr["ProjectManagerId"].ToString());
                        proj.CreatedByName = dr["CreatedByName"].ToString();
                        proj.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        proj.ModifiedByName = dr["ModifiedByName"].ToString();
                        proj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        lstProject.Add(proj);
                    }
            return lstProject;
        }
        public List<ProjectModel> GetSelectedProjectTaskOverDueList(string User_Id, string TaskName, string Status)
        {
            List<ProjectModel> lstProject = new List<ProjectModel>();
            ProjectDal objDL = new ProjectDal();
            DataSet dsData = objDL.GetSelectedProjectTaskOverDueList(User_Id, TaskName, Status);
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        ProjectModel proj = new ProjectModel();
                        proj.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                        proj.ProjectName = dr["ProjectName"].ToString();
                        proj.ProjectManager = dr["ProjectManager"].ToString();
                        lstProject.Add(proj);
                    }
            return lstProject;
        }


        public int SaveProjectTaskDoc(ProcjectDocModel Model, string User_Id)
        {
            int ErrCode = 0;
            try
            {
                DataSet ds = new DataSet();
                ProjectDal dalobj = new ProjectDal();
                ds = dalobj.GetSelectedProjectDocList(0, 0);

                ds.Tables[1].Rows.Clear();
                DataRow dr = ds.Tables[1].NewRow();
                dr["ProjectId"] = Model.ProjectId;
                dr["TaskId"] = Model.TaskId;
                dr["Description"] = Model.Description;
                dr["FileName"] = Model.FileName;
                ds.Tables[1].Rows.Add(dr);
                ErrCode = dalobj.SaveProjectTaskDoc(ds, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ErrCode;
        }


        public ProjectMaster GetProjectTaskData(int ProjectId, string User_Id)
        {
            ProjectMaster objMast = new ProjectMaster();
            List<TaskDetails> tasklst = new List<TaskDetails>();
            ProjectDal objProj = new ProjectDal();
            DataSet dsData = objProj.GetProjectTaskList(ProjectId, User_Id);

            if (dsData != null)
                if (dsData.Tables.Count > 1)
                {
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        objMast.ProjectName = dsData.Tables[0].Rows[0]["ProjectName"].ToString();
                        foreach (DataRow dr in dsData.Tables[0].Select("ParentTaskId=0"))
                        {
                            TaskDetails task = new TaskDetails();
                            task.Duration = Convert.ToInt32(dr["Duration"].ToString());
                            task.EndDate = dr["EndDate"].ToString();
                            task.StartDate = dr["StartDate"].ToString();
                            task.ProjectId = ProjectId;
                            task.TaskName = dr["TaskName"].ToString();
                            task.ParentTaskId = Convert.ToInt32(dr["ParentTaskId"].ToString());
                            task.TaskType = Convert.ToInt16(dr["TaskType"].ToString());
                            task.TaskID = Convert.ToInt32(dr["TaskId"].ToString());
                            task.Predecessors = dr["Predecessors"].ToString();
                            task.Progress = Convert.ToInt32(dr["Progress"].ToString());
                            task.Description = dr["Description"].ToString();
                            task.TaskTypeDesc = dr["TaskTypeDesc"].ToString();
                            task.ResourceID = getTaskResources(dsData, Convert.ToInt32(dr["TaskId"]));
                            task.SubTasks = getSubTaskofProject(dsData, Convert.ToInt32(dr["TaskId"]));
                            if (dr["AllowEdit"].ToString() == "1")
                                task.AllowEdit = true;
                            else
                                task.AllowEdit = true;
                            tasklst.Add(task);
                        }
                        objMast.Tasks = tasklst;
                        objMast.ProjectId = ProjectId;
                        objMast.startdate = Convert.ToDateTime(dsData.Tables[1].Rows[0]["ParentStartDate"].ToString());
                        objMast.enddate = Convert.ToDateTime(dsData.Tables[1].Rows[0]["ParentEndDate"].ToString());
                    }
                }
            return objMast;

        }

        private List<object> getTaskResources(DataSet dsData, Int32 ParentTaskId)
        {
            List<object> lstResources = new List<object>();
            foreach (DataRow dr in dsData.Tables[2].Select("TaskId=" + ParentTaskId.ToString()))
            {
                lstResources.Add(Convert.ToInt32(dr["EmpId"].ToString()));
            }
            return lstResources;

        }
        private List<TaskDetails> getSubTaskofProject(DataSet dsData, Int32 ParentTaskId)
        {
            List<TaskDetails> subtasks = new List<TaskDetails>();
            foreach (DataRow dr in dsData.Tables[0].Select("ParentTaskId=" + ParentTaskId.ToString()))
            {
                TaskDetails newtask = new TaskDetails();
                newtask.Duration = Convert.ToInt32(dr["Duration"].ToString());
                newtask.EndDate = dr["EndDate"].ToString();
                newtask.StartDate = dr["StartDate"].ToString();
                newtask.TaskName = dr["TaskName"].ToString();
                newtask.ParentTaskId = ParentTaskId;
                newtask.ResourceID = getTaskResources(dsData, Convert.ToInt32(dr["TaskId"]));
                newtask.TaskType = Convert.ToInt16( dr["TaskType"].ToString());
                newtask.TaskID = Convert.ToInt32(dr["TaskId"].ToString());
                newtask.Progress = Convert.ToInt32(dr["Progress"].ToString());
                newtask.Predecessors = dr["Predecessors"].ToString();
                newtask.Description = dr["Description"].ToString();
                newtask.TaskTypeDesc = dr["TaskTypeDesc"].ToString();
                if (dr["AllowEdit"].ToString() == "1")
                newtask.AllowEdit = true;
                else
                    newtask.AllowEdit = false;
                newtask.SubTasks = getSubTaskofProject(dsData, Convert.ToInt32(dr["TaskId"]));
                subtasks.Add(newtask);
            }
            return subtasks;
        }

        public ProjectTypeTaskDetails GetSelectedProjectEditModeList(int ProjectTypeId, int ProjectTypeTaskId)
        {
            ProjectTypeTaskDetails ProjectMdel = new ProjectTypeTaskDetails();
            ProjectMdel.LstProjectTaskDoc = new List<ProcjectTypeDocModel>();
            DataSet ds = new DataSet();
            ProjectDal dalobj = new ProjectDal();
            ds = dalobj.GetSelectedProjectEditModeList(ProjectTypeId, ProjectTypeTaskId);
            if(ds != null)
            {
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ProjectMdel.ProjectTypeId = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectTypeId"].ToString());
                        ProjectMdel.ProjectTypeTaskID = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjectTypeTaskID"].ToString());
                        ProjectMdel.TaskName = ds.Tables[0].Rows[0]["TaskName"].ToString();
                        ProjectMdel.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                        ProjectMdel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        ProjectMdel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                    }
                }
            }
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                ProcjectTypeDocModel newtask = new ProcjectTypeDocModel();
                newtask.FileName = dr["FileName"].ToString();
                newtask.Description = dr["Description"].ToString();
                newtask.ModifiedBy = dr["ModifiedBy"].ToString();
                newtask.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                ProjectMdel.LstProjectTaskDoc.Add(newtask);
            }
            return ProjectMdel;
        }

        public int SavePrjectDoc(ProcjectTypeDocModel Model,string User_Id)
        {
            int ErrCode = 0;
            try
            {
                DataSet ds = new DataSet();
                ProjectDal dalobj = new ProjectDal();
                ds = dalobj.GetSelectedProjectEditModeList(0,0);

                ds.Tables[1].Rows.Clear();
                DataRow dr = ds.Tables[1].NewRow();
                dr["ProjectTypeId"] = Model.ProjectTypeId;
                dr["ProjectTypeTaskId"] = Model.ProjectTypeTaskId;
                dr["Description"] = Model.Description;
                dr["FileName"] = Model.FileName;
                ds.Tables[1].Rows.Add(dr);
                ErrCode = dalobj.SavePrjectTypeTaskDoc(ds, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ErrCode;
        }

        public int UpdateProjectTaskDetails(TaskDetails Model, string User_Id)
        {
            int ErrCode = 0;
            try
            {
                DataSet ds = new DataSet();
                ProjectDal dalobj = new ProjectDal();
                ds = dalobj.GetSelectedProjectDocList(0, 0);

                ds.Tables[0].Rows.Clear();
                DataRow dr = ds.Tables[0].NewRow();
                dr["EstimationTime"] = Model.EstimationTime;
                dr["ProjectId"] = Model.ProjectId;
                dr["TaskId"] = Model.TaskID;
                dr["Description"] = Model.Description;
                dr["TaskName"] = Model.TaskName;
                dr["CustomerId"] = Model.CustomerId;
                dr["VendorId"] = Model.VendorId;
                dr["Progress"] = Model.Progress;
                string Comments = Model.Comments;
                ds.Tables[0].Rows.Add(dr);
                ErrCode = dalobj.UpdateProjectTask(ds, User_Id, Comments);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ErrCode;
        }


        public int UpdatePrjectTypeTaskDetails(ProjectTypeTaskDetails Model, string User_Id)
        {
            int ErrCode = 0;
            try
            {
                DataSet ds = new DataSet();
                ProjectDal dalobj = new ProjectDal();
                ds = dalobj.GetSelectedProjectEditModeList(0, 0);

                ds.Tables[0].Rows.Clear();
                DataRow dr = ds.Tables[0].NewRow();
                dr["ProjectTypeId"] = Model.ProjectTypeId;
                dr["ProjectTypeTaskId"] = Model.ProjectTypeTaskID;
                dr["Description"] = Model.Description;
                dr["TaskName"] = Model.TaskName;
                ds.Tables[0].Rows.Add(dr);
                ErrCode = dalobj.UpdatePrjectTypeTask(ds, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ErrCode;
        }
        #region Hardcoded Data for Project
        //public List<TaskDetails> GetTaskData()
        //{
        //    List<TaskDetails> tasks = new List<TaskDetails>();


        //    tasks.Add(new TaskDetails()
        //    {
        //        TaskID = 1,
        //        TaskName = "Project Schedule",
        //        StartDate = "02/03/2014",
        //        EndDate = "03/07/2014"
        //    });

        //    tasks[0].SubTasks = new List<TaskDetails>();

        //    tasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 2,
        //        TaskName = "Planning",
        //        StartDate = "02/03/2014",
        //        EndDate = "02/07/2014"
        //    });

        //    tasks[0].SubTasks[0].SubTasks = new List<TaskDetails>();


        //    tasks[0].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 3,
        //        TaskName = "Plan timeline",
        //        StartDate = "02/03/2014",
        //        EndDate = "02/07/2014",
        //        Duration = 5,
        //        Progress = "100",
        //        ResourceID = new List<Resources>() { 1 }
        //    });
        //    tasks[0].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 4,
        //        TaskName = "Plan budget",
        //        StartDate = "02/03/2014",
        //        EndDate = "02/07/2014",
        //        Duration = 5,
        //        Progress = "100",
        //        ResourceID = new List<object>() { 1 }
        //    });
        //    tasks[0].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 5,
        //        TaskName = "Allocate resources",
        //        StartDate = "02/03/2014",
        //        EndDate = "02/07/2014",
        //        Duration = 5,
        //        Progress = "100",
        //        ResourceID = new List<object>() { 1 }
        //    });
        //    tasks[0].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 6,
        //        TaskName = "Planning complete",
        //        StartDate = "02/07/2014",
        //        EndDate = "02/07/2014",
        //        Duration = 0,
        //        Predecessors = "3FS,4FS,5FS"
        //    });

        //    tasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 7,
        //        TaskName = "Design",
        //        StartDate = "02/10/2014",
        //        EndDate = "02/14/2014"
        //    });

        //    tasks[0].SubTasks[1].SubTasks = new List<TaskDetails>();

        //    tasks[0].SubTasks[1].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 8,
        //        TaskName = "Software Specification",
        //        StartDate = "02/10/2014",
        //        EndDate = "02/12/2014",
        //        Duration = 3,
        //        Progress = "60",
        //        Predecessors = "6FS",
        //        ResourceID = new List<object>() { 2 }
        //    });
        //    tasks[0].SubTasks[1].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 9,
        //        TaskName = "Develop prototype",
        //        StartDate = "02/10/2014",
        //        EndDate = "02/12/2014",
        //        Duration = 3,
        //        Progress = "100",
        //        Predecessors = "6FS",
        //        ResourceID = new List<object>() { 3 }
        //    });
        //    tasks[0].SubTasks[1].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 10,
        //        TaskName = "Get approval from customer",
        //        StartDate = "02/13/2014",
        //        EndDate = "02/14/2014",
        //        Duration = 2,
        //        Progress = "100",
        //        Predecessors = "9FS",
        //        ResourceID = new List<object>() { 1 }
        //    });
        //    tasks[0].SubTasks[1].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 11,
        //        TaskName = "Design complete",
        //        StartDate = "02/14/2014",
        //        EndDate = "02/14/2014",
        //        Duration = 0,
        //        Predecessors = "10FS"
        //    });


        //    tasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 12,
        //        TaskName = "Implementation Phase",
        //        StartDate = "02/17/2014",
        //        EndDate = "02/27/2014"
        //    });

        //    tasks[0].SubTasks[2].SubTasks = new List<TaskDetails>();

        //    tasks[0].SubTasks[2].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 13,
        //        TaskName = "Phase 1",
        //        StartDate = "02/17/2014",
        //        EndDate = "02/27/2014"
        //    });

        //    tasks[0].SubTasks[2].SubTasks[0].SubTasks = new List<TaskDetails>();

        //    tasks[0].SubTasks[2].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 14,
        //        TaskName = "Implementation Module 1",
        //        StartDate = "02/17/2014",
        //        EndDate = "02/27/2014"
        //    });

        //    tasks[0].SubTasks[2].SubTasks[0].SubTasks[0].SubTasks = new List<TaskDetails>();


        //    tasks[0].SubTasks[2].SubTasks[0].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 15,
        //        TaskName = "Development Task 1",
        //        StartDate = "02/17/2014",
        //        EndDate = "02/19/2014",
        //        Duration = 3,
        //        Progress = "50",
        //        Predecessors = "11FS",
        //        ResourceID = new List<object>() { 3 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[0].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 16,
        //        TaskName = "Development Task 2",
        //        StartDate = "02/17/2014",
        //        EndDate = "02/19/2014",
        //        Duration = 3,
        //        Progress = "50",
        //        Predecessors = "11FS",
        //        ResourceID = new List<object>() { 3 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[0].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 17,
        //        TaskName = "Testing",
        //        StartDate = "02/20/2014",
        //        EndDate = "02/21/2014",
        //        Duration = 2,
        //        Progress = "0",
        //        Predecessors = "15FS,16FS",
        //        ResourceID = new List<object>() { 4 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[0].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 18,
        //        TaskName = "Bug fix",
        //        StartDate = "02/24/2014",
        //        EndDate = "02/25/2014",
        //        Duration = 2,
        //        Progress = "0",
        //        Predecessors = "17FS",
        //        ResourceID = new List<object>() { 3 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[0].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 19,
        //        TaskName = "Customer review meeting",
        //        StartDate = "02/26/2014",
        //        EndDate = "02/27/2014",
        //        Duration = 2,
        //        Progress = "0",
        //        Predecessors = "18FS",
        //        ResourceID = new List<object>() { 1 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[0].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 20,
        //        TaskName = "Phase 1 complete",
        //        StartDate = "02/27/2014",
        //        EndDate = "02/27/2014",
        //        Duration = 0,
        //        Predecessors = "19FS"
        //    });

        //    tasks[0].SubTasks[2].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 21,
        //        TaskName = "Phase 2",
        //        StartDate = "02/17/2014",
        //        EndDate = "02/28/2014"
        //    });

        //    tasks[0].SubTasks[2].SubTasks[1].SubTasks = new List<TaskDetails>();

        //    tasks[0].SubTasks[2].SubTasks[1].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 22,
        //        TaskName = "Implementation Module 2",
        //        StartDate = "02/17/2014",
        //        EndDate = "02/28/2014"
        //    });

        //    tasks[0].SubTasks[2].SubTasks[1].SubTasks[0].SubTasks = new List<TaskDetails>();

        //    tasks[0].SubTasks[2].SubTasks[1].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 23,
        //        TaskName = "Development Task 1",
        //        StartDate = "02/17/2014",
        //        EndDate = "02/20/2014",
        //        Duration = 4,
        //        Progress = "50",
        //        ResourceID = new List<object>() { 3 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[1].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 24,
        //        TaskName = "Development Task 2",
        //        StartDate = "02/17/2014",
        //        EndDate = "02/20/2014",
        //        Duration = 4,
        //        Progress = "50",
        //        ResourceID = new List<object>() { 3 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[1].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 25,
        //        TaskName = "Testing",
        //        StartDate = "02/21/2014",
        //        EndDate = "02/24/2014",
        //        Duration = 2,
        //        Progress = "0",
        //        Predecessors = "23FS,24FS",
        //        ResourceID = new List<object>() { 4 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[1].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 26,
        //        TaskName = "Bug fix",
        //        StartDate = "02/25/2014",
        //        EndDate = "02/26/2014",
        //        Duration = 2,
        //        Progress = "0",
        //        Predecessors = "25FS",
        //        ResourceID = new List<object>() { 3 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[1].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 27,
        //        TaskName = "Customer review meeting",
        //        StartDate = "02/27/2014",
        //        EndDate = "02/28/2014",
        //        Duration = 2,
        //        Progress = "0",
        //        Predecessors = "26FS",
        //        ResourceID = new List<object>() { 1 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[1].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 28,
        //        TaskName = "Phase 2 complete",
        //        StartDate = "02/28/2014",
        //        EndDate = "02/28/2014",
        //        Duration = 0,
        //        Predecessors = "27FS"
        //    });

        //    tasks[0].SubTasks[2].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 29,
        //        TaskName = "Phase 3",
        //        StartDate = "02/17/2014",
        //        EndDate = "02/27/2014"
        //    });
        //    tasks[0].SubTasks[2].SubTasks[2].SubTasks = new List<TaskDetails>();

        //    tasks[0].SubTasks[2].SubTasks[2].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 30,
        //        TaskName = "Implementation Module 3",
        //        StartDate = "02/17/2014",
        //        EndDate = "02/27/2014"
        //    });

        //    tasks[0].SubTasks[2].SubTasks[2].SubTasks[0].SubTasks = new List<TaskDetails>();

        //    tasks[0].SubTasks[2].SubTasks[2].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 31,
        //        TaskName = "Development Task 1",
        //        StartDate = "02/17/2014",
        //        EndDate = "02/19/2014",
        //        Duration = 3,
        //        Progress = "50",
        //        ResourceID = new List<object>() { 3 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[2].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 32,
        //        TaskName = "Development Task 2",
        //        StartDate = "02/17/2014",
        //        EndDate = "02/19/2014",
        //        Duration = 3,
        //        Progress = "50",
        //        ResourceID = new List<object>() { 3 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[2].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 33,
        //        TaskName = "Testing",
        //        StartDate = "02/20/2014",
        //        EndDate = "02/21/2014",
        //        Duration = 2,
        //        Progress = "0",
        //        Predecessors = "31FS,32FS",
        //        ResourceID = new List<object>() { 4 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[2].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 34,
        //        TaskName = "Bug fix",
        //        StartDate = "02/24/2014",
        //        EndDate = "02/25/2014",
        //        Duration = 2,
        //        Progress = "0",
        //        Predecessors = "33FS",
        //        ResourceID = new List<object>() { 3 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[2].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 35,
        //        TaskName = "Customer review meeting",
        //        StartDate = "02/26/2014",
        //        EndDate = "02/27/2014",
        //        Duration = 2,
        //        Progress = "0",
        //        Predecessors = "34FS",
        //        ResourceID = new List<object>() { 1 }
        //    });
        //    tasks[0].SubTasks[2].SubTasks[2].SubTasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 36,
        //        TaskName = "Phase 3 complete",
        //        StartDate = "02/27/2014",
        //        EndDate = "02/27/2014",
        //        Duration = 0,
        //        Predecessors = "35FS"
        //    });

        //    tasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 37,
        //        TaskName = "Integration",
        //        StartDate = "03/03/2014",
        //        EndDate = "03/05/2014",
        //        Duration = 2,
        //        Progress = "0",
        //        Predecessors = "20FS,28FS,36FS",
        //        ResourceID = new List<object>() { 3 }
        //    });
        //    tasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 38,
        //        TaskName = "Final Testing",
        //        StartDate = "03/06/2014",
        //        EndDate = "03/07/2014",
        //        Duration = 2,
        //        Progress = "0",
        //        Predecessors = "37FS",
        //        ResourceID = new List<object>() { 4 }
        //    });
        //    tasks[0].SubTasks.Add(new TaskDetails()
        //    {
        //        TaskID = 39,
        //        TaskName = "Final Delivery",
        //        StartDate = "03/07/2014",
        //        EndDate = "03/07/2014",
        //        Duration = 0,
        //        Predecessors = "38FS"
        //    });

        //    return tasks;

        //}
        #endregion Hardcoded Data for Project
        public List<Resources> GetResourceData()
        {
            List<Resources> resourceDetails = new List<Resources>();
            AdminDal objadm = new AdminDal();
            DataSet dsEmp = objadm.GetEmpList();
            foreach (DataRow dr in dsEmp.Tables[0].Rows)
            {
                Resources newres = new Resources();
                newres.ResourceID = Convert.ToInt32(dr["EmpId"].ToString());
                newres.EmpName = dr["EmpName"].ToString();
                resourceDetails.Add(newres);
            }
            return resourceDetails;
        }
        public List<Resources> GetResourceDataAllEmp()
        {
            List<Resources> resourceDetails = new List<Resources>();
            AdminDal objadm = new AdminDal();
            DataSet dsEmp = objadm.GetAllEmpList();
            foreach (DataRow dr in dsEmp.Tables[0].Rows)
            {
                Resources newres = new Resources();
                newres.ResourceID = Convert.ToInt32(dr["EmpId"].ToString());
                newres.EmpName = dr["EmpName"].ToString();
                resourceDetails.Add(newres);
            }
            return resourceDetails;
        }
        #region Project Task Mom Details
        public List<ProjectTaskMoM> ProjTaskMOMList(string User_Id)
        {
            List<ProjectTaskMoM> MOMList = new List<ProjectTaskMoM>();
          try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.ProjTaskMOMList(User_Id);
                if (ds != null)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            ProjectTaskMoM MOMModel = new ProjectTaskMoM();
                            MOMModel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                          //  MOMModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            MOMModel.Employee = dr["Employee"].ToString();
                          //  MOMModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            MOMModel.ProjectName = dr["ProjectName"].ToString();
                            MOMModel.MOMTypeKey = dr["MOMType"].ToString();
                          //  MOMModel.TaskId = Convert.ToInt32(dr["TaskID"].ToString());
                            MOMModel.TaskName = dr["TaskName"].ToString();
                            MOMModel.ModifiedByName = dr["ModifiedBy"].ToString();
                            MOMModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            //  MOMModel.Description = dr["Description"].ToString();
                            MOMModel.CustomerName = dr["CustomerName"].ToString();
                            MOMModel.VendorName = dr["VendorName"].ToString();
                            MOMModel.Title = dr["Title"].ToString();
                            MOMModel.MOMDate = Convert.ToDateTime(dr["MOMDate"].ToString());
                            MOMList.Add(MOMModel);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MOMList;
        }
        public List<ProjTaskModel> ProjTaskList(int ProjectId)
        {
            ProjectTaskMoM objmodel = new ProjectTaskMoM();
            objmodel.LstProjTask = new List<ProjTaskModel>();
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.ProjTaskList(ProjectId);
                if(ds!= null)
                {   if(ds.Tables[0].Rows.Count > 0)
                    {
                        foreach(DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjTaskModel Taskmodel = new ProjTaskModel();
                            Taskmodel.TaskID =Convert.ToInt32(dr["TaskID"].ToString());
                            Taskmodel.TaskName = dr["TaskName"].ToString();
                            objmodel.LstProjTask.Add(Taskmodel);
                         }
                    }

                }
                   
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return objmodel.LstProjTask;
        }
        //public List<MOMActionPointUser> GetselectedMOMAParticipant(int MOMId)
        //{

        //    List<MOMActionPointUser> LstParticipant = new List<MOMActionPointUser>();

        //    try
        //    {
        //        ProjectDal objProj = new ProjectDal();
        //        DataSet ds = new DataSet();
        //        ds = objProj.GetselectedParticipantList(MOMId);
        //        if (ds != null)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in ds.Tables[0].Rows)
        //                {
        //                    MOMActionPointUser user = new MOMActionPointUser();
        //                    user.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
        //                    user.UserName = dr["UserName"].ToString();
        //                    user.ParticipantType = dr["UserType"].ToString();
        //                    user.UserId = Convert.ToInt32(dr["UserId"].ToString());
        //                    LstParticipant.Add(user);
        //                }
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return LstParticipant;
        //}


        public int DeleteMomActionPointParticipants(int ActionPointId, string ParticipantsType, int UserId)
        {
            try
            {
                ProjectDal objDAL = new ProjectDal();
                return objDAL.DeleteMomActonPointParticipants(ActionPointId, ParticipantsType, UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int SaveProjTaskMOM(ProjectTaskMoM objmodel ,string User_Id)
        {
            ProjectDal objDAL = new ProjectDal();
            DataSet dsMOM = new DataSet();      
            try
            {
                dsMOM = objDAL.GetselectedProjTaskMOM(0);
                if (dsMOM != null)
                {
                    if (dsMOM.Tables.Count > 0)
                    {
                        dsMOM.Tables[0].Rows.Clear();
                        DataRow dr = dsMOM.Tables[0].NewRow();
                        dr["MOMId"] = objmodel.MOMId;
                        dr["MOMType"] = objmodel.MOMTypeKey;
                        dr["TaskId"] = objmodel.TaskId;
                        dr["ManageMentView"] = objmodel.ManageMentView;
                        dr["ProjectId"] = objmodel.ProjectId;
                        dr["Description"] = objmodel.Description;
                        dr["Title"] = objmodel.Title;
                        dr["MOMDate"] = objmodel.MOMDate;
                        dr["LocalDescription"] = objmodel.LocalDescription;
                        dr["CustomerId"] = objmodel.CustomerId;
                        dr["VendorId"] = objmodel.VendorId;
                        dsMOM.Tables[0].Rows.Add(dr);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objDAL.SaveProjTaskMOM(dsMOM, User_Id);
        }
        public ProjectTaskMoM GetSelectedProjectTaskMOM(int MOMId)
        {
            DataSet DS = new DataSet();
            ProjectTaskMoM MOModel = new ProjectTaskMoM();
            try
            {
                ProjectDal objDAL = new ProjectDal();
                DS = objDAL.GetselectedProjTaskMOM(MOMId);
                if (DS != null)
                {
                    if (DS.Tables.Count > 0)
                    {
                        if (DS.Tables[0].Rows.Count > 0)
                        {
                            MOModel.MOMId = MOMId;
                            MOModel.ProjectName = DS.Tables[0].Rows[0]["ProjectName"].ToString();
                            MOModel.ProjectId = Convert.ToInt32(DS.Tables[0].Rows[0]["ProjectId"].ToString());
                            MOModel.Employee = DS.Tables[0].Rows[0]["Employee"].ToString();
                            MOModel.CustomerId = Convert.ToInt32(DS.Tables[0].Rows[0]["CustomerId"].ToString());
                            MOModel.VendorId = Convert.ToInt32(DS.Tables[0].Rows[0]["VendorId"].ToString());
                            MOModel.TaskId = Convert.ToInt32(DS.Tables[0].Rows[0]["TaskId"].ToString());
                            MOModel.TaskName = DS.Tables[0].Rows[0]["TaskName"].ToString();
                            MOModel.MOMTypeKey = DS.Tables[0].Rows[0]["MOMType"].ToString();
                            MOModel.Title = DS.Tables[0].Rows[0]["Title"].ToString();
                            MOModel.MOMDate = Convert.ToDateTime(DS.Tables[0].Rows[0]["MOMDate"].ToString());
                            MOModel.Description = DS.Tables[0].Rows[0]["Description"].ToString();
                            MOModel.LocalDescription = DS.Tables[0].Rows[0]["LocalDescription"].ToString();
                            MOModel.ManageMentView = DS.Tables[0].Rows[0]["ManageMentView"].ToString();
                            MOModel.CreatedByName = DS.Tables[0].Rows[0]["CreatedBy"].ToString();
                            MOModel.ModifiedByName = DS.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            MOModel.CreatedDate = Convert.ToDateTime(DS.Tables[0].Rows[0]["CreatedDate"].ToString());
                            MOModel.ModifiedDate = Convert.ToDateTime(DS.Tables[0].Rows[0]["ModifiedDate"].ToString());
                         }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return MOModel;
        }
        public List<MoMParticipantModel> MOMParticipantList(int MOMId)
        {
            ProjectTaskMoM objmodel = new ProjectTaskMoM();
            objmodel.LstMoMParticipant = new List<MoMParticipantModel>();
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.MOMParticipantList(MOMId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            MoMParticipantModel Participantmodel = new MoMParticipantModel();
                            Participantmodel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            Participantmodel.ParticipantId = Convert.ToInt32(dr["ParticipantId"].ToString());
                            Participantmodel.Name = dr["Name"].ToString();
                            Participantmodel.FYI = Convert.ToBoolean(dr["FYI"].ToString());
                            if (Participantmodel.FYI == true)
                                Participantmodel.FYIValue = "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=glyphicon glyphicon-ok >" + "<b style='color:green' class='glyphicon glyphicon-ok'>" + " " + "</b>" + "</span>" + "</div>";
                            else
                                Participantmodel.FYIValue = "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:Red' class='glyphicon glyphicon-remove'>" + " " + "</b>" + "</span>" + "</div>";
                            Participantmodel.ParticipantType = dr["ParticipantType"].ToString();
                            objmodel.LstMoMParticipant.Add(Participantmodel);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return   objmodel.LstMoMParticipant ;
        }
        public List<MOMType> MOMType()
        {
            List<MOMType> LstType = new List<MOMType>();
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.MOMType();
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            MOMType MomType = new MOMType();
                            MomType.MOMTypeKey = dr["MOMTypeKey"].ToString();
                            MomType.MOMTypeValue = dr["MOMTypeValue"].ToString();
                            LstType.Add(MomType);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstType;
        }
        public List<CustomerContact> CustomerContactByCustomerId(int CustomerId)
        {
            List<CustomerContact> ContactList = new List<CustomerContact>();      
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.CustomerContactListByCustomerId(CustomerId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CustomerContact objmodel = new CustomerContact();
                            objmodel.CustomerContactId = Convert.ToInt32(dr["CustomerContactId"].ToString());
                            objmodel.ContactName = dr["ContactName"].ToString();
                            ContactList.Add(objmodel);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ContactList;
        }
        public List<VendorContact> VendorContactByVendorId(int VendorId)
        {
            List<VendorContact> ContactList = new List<VendorContact>();
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.VendorContactListByVendorId(VendorId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            VendorContact objmodel = new VendorContact();
                            objmodel.VendorContactId = Convert.ToInt32(dr["VendorContactId"].ToString());
                            objmodel.ContactName = dr["ContactName"].ToString();
                            ContactList.Add(objmodel);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ContactList;
        }

        public List<MoMAttachmentModel> MOMAttachmentList(int MOMId)
        {
            ProjectTaskMoM objmodel = new ProjectTaskMoM();
            objmodel.LstMOMAttachment = new List<MoMAttachmentModel>();
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.MOMAttachmentList(MOMId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            MoMAttachmentModel Attachmentmodel = new MoMAttachmentModel();
                            Attachmentmodel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            Attachmentmodel.FileName = dr["FileName"].ToString();
                            Attachmentmodel.Description = dr["Description"].ToString();
                            objmodel.LstMOMAttachment.Add(Attachmentmodel);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objmodel.LstMOMAttachment;
        }


        public int Saveparticipatns(string PType, string[] selitems, int MOMID, string User_Id, bool FYI)
        {
          try
          {
              ProjectDal objDAL = new ProjectDal();
              return objDAL.SaveParticipants(PType, selitems, MOMID, User_Id,FYI);
          }
            catch (Exception ex)
          {
              throw ex;
          }
           
        }


        public int SaveMOMAttachment(string FileName, string Description, int MOMID)
        {
            try
            {
                ProjectDal objDAL = new ProjectDal();
                return objDAL.SaveAttachments(FileName, Description, MOMID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int DeleteParticipants(int MOMID, int ParticipantId, string ParticipantsType)
        {
            try
            {
                ProjectDal objDAL = new ProjectDal();
                return objDAL.DeleteParticipants(MOMID, ParticipantId, ParticipantsType);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetMOMParticipendsDetailList(int MOMId )
        {
            DataSet ds = new DataSet();
            //string name = "";
            //string Email = "";
            //List<string> mailInfo = new List<string>();
            try
            {
                ProjectDal objDAL = new ProjectDal();
                ds = objDAL.GetMOMParticipendsDetailList(MOMId);
                //foreach (DataRow dr in ds.Tables[0].Rows)
                //{
                //    if (name != "")
                //    {
                //        name = name + ", ";
                //    }
                //    if (dr["ParticipantName"].ToString() != "")
                //        name = name + dr["ParticipantName"].ToString();
                //    if (Email != "")
                //    {
                //        Email = Email + ",";
                //    }
                //    if (dr["Participantemail"].ToString() != "")
                //        Email = Email + dr["Participantemail"].ToString();
                //}
                //mailInfo.Add(name.Trim());
                //mailInfo.Add(Email.Trim());
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return ds;
        }
        #endregion Project Task Mom Details
        #region MomActionPoint
        public List<MOMStatusCodes> MomStatusList( )
        {
            List<MOMStatusCodes> StatusList = new  List<MOMStatusCodes>();
           
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.GetStatusCodeForMomActionPoint();
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            MOMStatusCodes Statusmodel = new MOMStatusCodes();
                            Statusmodel.Status = Convert.ToInt32(dr["StatusId"].ToString());
                            Statusmodel.StatusShortCode = dr["StatusShortCode"].ToString();
                            StatusList.Add(Statusmodel);
                          
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusList;
        }

        public List<MOMActionPoint> MOMActionPointList(int MOMId)
        {
          
            ProjectTaskMoM model = new ProjectTaskMoM();
            model.ActionPointList = new List<MOMActionPoint>();
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.GetMOMActionPointList(MOMId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            MOMActionPoint ActionPoint = new MOMActionPoint();
                            ActionPoint.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                            ActionPoint.ActionDescription = dr["ActionDescription"].ToString();
                           if(dr["DueDate"].ToString()!="")
                                ActionPoint.DueDate = Convert.ToDateTime(dr["DueDate"].ToString());
                            ActionPoint.StatusShortCode = dr["Status"].ToString();
                            ActionPoint.AssignedBy = dr["AssignedBy"].ToString();
                            ActionPoint.Resource = dr["Resource"].ToString() + ',' + dr["Vendor"].ToString() + ',' + dr["Customer"].ToString();
                            ActionPoint.ModifiedByName = dr["ModifiedBy"].ToString();
                            ActionPoint.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            model.ActionPointList.Add(ActionPoint);

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model.ActionPointList;
        }
        public MOMActionPoint GetselectedMOMActionPoint(int ActionPointId)
        {
            MOMActionPoint ActionPoint = new MOMActionPoint();
            ActionPoint.MOMActionPointCommentList = new List<MOMActionPointComment>();
          
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.GetSelectedMOMActionPointList(ActionPointId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            
                            ActionPoint.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                            ActionPoint.Resource = dr["Resource"].ToString();
                            ActionPoint.Vendor = dr["Vendor"].ToString();
                            ActionPoint.Customer = dr["Customer"].ToString();
                            ActionPoint.ActionDescription = dr["ActionDescription"].ToString();
                            ActionPoint.Status = Convert.ToInt32(dr["Status"].ToString());
                            ActionPoint.AssignedBy = dr["AssignedBy"].ToString();
                            ActionPoint.CreatedByName = dr["CreatedByName"].ToString();
                            ActionPoint.ModifiedByName = dr["ModifiedBy"].ToString();
                            ActionPoint.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            if (dr["DueDate"].ToString()!= "")
                            {
                                ActionPoint.DueDateNULL = Convert.ToDateTime(dr["DueDate"].ToString());
                            }
                         
                            ActionPoint.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                           

                        }
                    }

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            MOMActionPointComment comments = new MOMActionPointComment();
                            comments.Comment = dr["Comment"].ToString();
                            comments.CommentedBy = dr["CommentedBy"].ToString();
                            comments.StatusShortCode = dr["StatusShortCode"].ToString();
                            comments.CommentDate = Convert.ToDateTime(dr["CommentDate"].ToString());
                            ActionPoint.MOMActionPointCommentList.Add(comments);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ActionPoint;
        }
        public List<MOMActionPointUser> GetselectedMOMActionPointParticipant(int ActionPointId)
        {

            List<MOMActionPointUser> LstParticipant =new  List<MOMActionPointUser>();

            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.GetselectedMOMactionPointParticipantList(ActionPointId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            MOMActionPointUser user = new MOMActionPointUser();

                            user.UserName = dr["UserName"].ToString();
                            user.UserId = Convert.ToInt32(dr["UserId"].ToString());
                            user.ParticipantType = dr["UserType"].ToString();
                            LstParticipant.Add(user);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstParticipant;
        }

        public List<MOMActionPointUser> MOMActionUserList()
        {

            List<MOMActionPointUser> Userlist = new List<MOMActionPointUser>();
          
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.MOMActionPointUserList();
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            MOMActionPointUser ActionPointuser = new MOMActionPointUser();
                            ActionPointuser.UserId = Convert.ToInt32(dr["UserId"].ToString());
                            ActionPointuser.UserName = dr["UserName"].ToString();
                            Userlist.Add(ActionPointuser);

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Userlist;
        }
        public int SaveMOMActionPointList(MOMActionPoint objmodel, string User_Id, string[] selitems, string PType )
        {
            ProjectDal objDAL = new ProjectDal();
            DataSet dsMOM = new DataSet();
            string Comment = "";
            try
            {
                dsMOM = objDAL.GetSelectedMOMActionPointList(0);
                if (dsMOM != null)
                {
                    if (dsMOM.Tables.Count > 0)
                    {
                        dsMOM.Tables[0].Rows.Clear();
                        DataRow dr = dsMOM.Tables[0].NewRow();
                        dr["ActionPointId"] = objmodel.ActionPointId;
                        dr["MOMId"] = objmodel.MOMId;
                        dr["ActionDescription"] = objmodel.ActionDescription;
                        dr["Status"] = objmodel.Status;
                        if (objmodel.DueDate.ToString()!="")
                        {
                            dr["DueDate"] = objmodel.DueDate;
                        }
                        Comment = objmodel.Comment;
              
                        dsMOM.Tables[0].Rows.Add(dr);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objDAL.SaveMOMActionPoint(dsMOM, User_Id, selitems, PType, Comment);
        }
        #endregion MomActionPoint

        #region Project Expenses
        public List<ProjectExpensesModel> TMProjectExpensesList(string User_Id)
        {
            List<ProjectExpensesModel> LstExpenses = new List<ProjectExpensesModel>();
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.ProjectExpensesList(User_Id);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjectExpensesModel ProExpModel = new ProjectExpensesModel();
                            ProExpModel.ExpenseId = Convert.ToInt32(dr["ExpenseId"].ToString());
                            ProExpModel.ExpenseType = dr["ExpenceType"].ToString();
                            ProExpModel.Employee = dr["Employee"].ToString();
                            ProExpModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            ProExpModel.ProjectName = dr["ProjectName"].ToString();
                            ProExpModel.TaskId = Convert.ToInt32(dr["TaskID"].ToString());
                            ProExpModel.TaskName = dr["TaskName"].ToString();
                            ProExpModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            ProExpModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            ProExpModel.ExpenseDate = Convert.ToDateTime(dr["ExpenseDate"].ToString());
                            ProExpModel.StatusCode = dr["StatusShortCode"].ToString();
                            ProExpModel.Amount = Convert.ToDouble(dr["Amount"].ToString());
                            ProExpModel.Remark = dr["Remark"].ToString();
                            LstExpenses.Add(ProExpModel);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstExpenses;
        }

        public List<ProjectExpensesModel> PaymentStatusList(string User_Id)
        {
            List<ProjectExpensesModel> LstExpenses = new List<ProjectExpensesModel>();
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.ProjectExpensesList(User_Id);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            if (dr["StatusShortCode"].ToString() == "APPROVED")
                            { 
                            ProjectExpensesModel ProExpModel = new ProjectExpensesModel();
                            ProExpModel.ExpenseId = Convert.ToInt32(dr["ExpenseId"].ToString());
                            ProExpModel.ExpenseType = dr["ExpenceType"].ToString();

                          //  if (dr["PaymentId"].ToString() != "")
                               // ProExpModel.PaymentId = Convert.ToInt32(dr["PaymentId"].ToString());
                            ProExpModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            ProExpModel.Employee = dr["Employee"].ToString();
                            // ProExpModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            ProExpModel.ProjectName = dr["ProjectName"].ToString();
                            // ProExpModel.TaskId = Convert.ToInt32(dr["TaskID"].ToString());
                            ProExpModel.TaskName = dr["TaskName"].ToString();
                            if (dr["TotalPaid"].ToString()!="")
                            ProExpModel.TotalPaid = Convert.ToDouble(dr["TotalPaid"].ToString());
                            ProExpModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            ProExpModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            ProExpModel.ExpenseDate = Convert.ToDateTime(dr["ExpenseDate"].ToString());
                            ProExpModel.StatusCode = dr["StatusShortCode"].ToString();
                            ProExpModel.Amount = Convert.ToDouble(dr["Amount"].ToString());
                            if (dr["NewAmount"].ToString() != "")
                            ProExpModel.NewAmount = Convert.ToDouble(dr["NewAmount"].ToString());
                            if (ProExpModel.TotalPaid == ProExpModel.Amount)
                            {
                                ProExpModel.PaymentStatus = "FullyPaid";
                            }
                            else
                                if (ProExpModel.TotalPaid < ProExpModel.Amount && ProExpModel.NewAmount != 0)
                            {
                                ProExpModel.PaymentStatus = "PartiallyPaid";
                            }
                            else
                            {
                                    ProExpModel.PaymentStatus = "NotPaid";
                            }
                            ProExpModel.Remark = dr["Remark"].ToString();
                            LstExpenses.Add(ProExpModel);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstExpenses;
        }

        public int SavePaymentDetails(PaymentExpenseStatus objmodel, string User_Id)
        {
            ProjectDal objDAL = new ProjectDal();
            DataSet dsMOM = new DataSet();
           try
            {
                dsMOM = objDAL.GetselectedPaymentdetails(0);
                if (dsMOM != null)
                {
                    if (dsMOM.Tables.Count > 0)
                    {
                        dsMOM.Tables[0].Rows.Clear();
                        DataRow dr = dsMOM.Tables[0].NewRow();
                        dr["PaymentId"] = objmodel.PaymentId;
                        dr["PaymentType"] = objmodel.PaymentType;
                        dr["Amount"] = objmodel.Amount;
                        dr["Remark"] = objmodel.Remark;
                        dr["RefId"] = objmodel.RefId;
                        dr["EmpId"] = objmodel.EmpId;
                        dsMOM.Tables[0].Rows.Add(dr);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
           return objDAL.SavePaymentDetails(dsMOM, User_Id);
        }
        public int DeletePaymentDetails(int PaymentId, string Remark)
        {
             ProjectDal ProjDal = new ProjectDal();
             return ProjDal.DeletePaymentDetails(PaymentId, Remark);
        }
        public PaymentExpenseStatus TmGetselectedPayment(int Id)
        {
            PaymentExpenseStatus Model = new PaymentExpenseStatus();
            
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.GetselectedPaymentdetails(Id);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                      
                        DataRow dr =  ds.Tables[0].Rows[0];
                        Model.Remark = dr["Remark"].ToString();
                        Model.NewAmount = Convert.ToDouble(dr["Amount"].ToString());
                        Model.Amount = Convert.ToDouble(dr["ExpenseAmount"].ToString());
                        Model.PaymentId = Convert.ToInt32(dr["PaymentId"].ToString());
                        Model.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                        Model.Employee = Convert.ToString(dr["Employee"].ToString());
                        Model.RefId = Convert.ToInt32(dr["RefId"].ToString());
                        Model.PaymentType = Convert.ToString(dr["PaymentType"].ToString());
                        Model.CreatedBy = Convert.ToString(dr["CreatedBy"].ToString());
                        Model.ModifiedBy = Convert.ToString(dr["ModifiedBy"].ToString());
                        Model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        Model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Model;
        }
        public List<PaymentExpenseStatus> TmGetselectedPaymentList(int ReFId)
        {
            List<PaymentExpenseStatus> Lst = new List<PaymentExpenseStatus>();
           try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.GetselectedPaymentdetailsList(ReFId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PaymentExpenseStatus Model = new PaymentExpenseStatus();
                            Model.Remark = dr["Remark"].ToString();
                            if (dr["Amount"].ToString()!="")
                            Model.NewAmount = Convert.ToDouble(dr["Amount"].ToString());
                            if (dr["ExpenseAmount"].ToString() != "")
                            Model.Amount = Convert.ToDouble(dr["ExpenseAmount"].ToString());
                            if (dr["TotalPaid"].ToString() != "")
                            Model.TotalPaid = Convert.ToDouble(dr["TotalPaid"].ToString());
                            Model.PaymentId = Convert.ToInt32(dr["PaymentId"].ToString());
                            Model.Isdeleted = Convert.ToString(dr["Isdeleted"].ToString());
                            Model.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            Model.Employee = Convert.ToString(dr["Employee"].ToString());
                            Model.RefId = Convert.ToInt32(dr["RefId"].ToString());
                            Model.PaymentType = Convert.ToString(dr["PaymentType"].ToString());
                            Model.CreatedBy = Convert.ToString(dr["CreatedBy"].ToString());
                            Model.ModifiedBy = Convert.ToString(dr["ModifiedBy"].ToString());
                            Model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            Model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            Lst.Add(Model);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
           return Lst;
        }
        public ProjectExpensesModel TMGetSelectedProjectExpensesList(int ProjectExpensesId)
        {
            ProjectExpensesModel ProExpModel = new ProjectExpensesModel();
            ProExpModel.LstExpDoc = new List<ExpensesDocModel>();
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.GetSelectedProjectExpensesList(ProjectExpensesId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {                           
                            ProExpModel.ExpenseId = Convert.ToInt32(dr["ExpenseId"].ToString());
                            ProExpModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            if(dr["ExpTypeId"].ToString()!="")
                            {
                                ProExpModel.ExpTypeId = Convert.ToInt32(dr["ExpTypeId"].ToString()); 
                            }
                       
                            ProExpModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            ProExpModel.ProjectName = dr["ProjectName"].ToString();
                            ProExpModel.TaskId = Convert.ToInt32(dr["TaskID"].ToString());
                            ProExpModel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            ProExpModel.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            ProExpModel.TaskName = dr["TaskName"].ToString();
                            ProExpModel.Remark = dr["Remark"].ToString();
                            ProExpModel.Description = dr["Description"].ToString();
                            ProExpModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            ProExpModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            ProExpModel.StatusCode = dr["StatusShortCode"].ToString();
                            ProExpModel.Amount = Convert.ToDouble(dr["Amount"].ToString());
                            ProExpModel.ExpenseDate = Convert.ToDateTime(dr["ExpenseDate"].ToString());
                            ProExpModel.CreatedBy = dr["CreatedBy"].ToString();
                            ProExpModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            ProExpModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            ProExpModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        }
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            ExpensesDocModel demo=new ExpensesDocModel();
                            demo.ExpenseId = Convert.ToInt32(dr["ExpenseId"].ToString());
                            demo.Description = dr["Description"].ToString();
                            demo.DocumentPath = dr["DocumentPath"].ToString();
                            ProExpModel.LstExpDoc.Add(demo);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ProExpModel;
        }

        public int SaveProjectExpenses(ProjectExpensesModel Model,string UserId)
        {
            int errcode = 0;
            try
            {
                DataSet dsObj = new DataSet();
                ProjectDal dalObj = new ProjectDal();
                DataTable dtTMPrjExpe = new DataTable("tbl_PrjeExpe");
                dtTMPrjExpe.Columns.Add("ExpenseId", typeof(System.Int32));
                dtTMPrjExpe.Columns.Add("ExpTypeId", typeof(System.Int32));
                dtTMPrjExpe.Columns.Add("ProjectId", typeof(System.Int32));
                dtTMPrjExpe.Columns.Add("CustomerId", typeof(System.Int32));
                dtTMPrjExpe.Columns.Add("VendorId", typeof(System.Int32));
                dtTMPrjExpe.Columns.Add("TaskId", typeof(System.String));
                dtTMPrjExpe.Columns.Add("Amount", typeof(System.Double));
                dtTMPrjExpe.Columns.Add("Remark", typeof(System.String));
                dtTMPrjExpe.Columns.Add("ExpenseDate", typeof(System.DateTime));
                dtTMPrjExpe.Columns.Add("DocumentPath", typeof(System.String));
                dtTMPrjExpe.Columns.Add("Description", typeof(System.String));
                
                DataRow drTMProExpe = dtTMPrjExpe.NewRow();
                drTMProExpe["ExpenseId"] = Convert.ToInt32(Model.ExpenseId);
                drTMProExpe["ProjectId"] = Convert.ToInt32(Model.ProjectId);
                drTMProExpe["ExpTypeId"] = Convert.ToInt32(Model.ExpTypeId);
                drTMProExpe["TaskId"] = Convert.ToInt32(Model.TaskId);
                drTMProExpe["CustomerId"] = Convert.ToInt32(Model.CustomerId);
                drTMProExpe["VendorId"] = Convert.ToInt32(Model.VendorId);
                drTMProExpe["Amount"] = Convert.ToDouble(Model.Amount);
                drTMProExpe["Remark"] = Model.Remark;
                drTMProExpe["ExpenseDate"] = Model.ExpenseDate;
                drTMProExpe["DocumentPath"] = Model.DocumentPath;
                drTMProExpe["Description"] = Model.Description;
                dtTMPrjExpe.Rows.Add(drTMProExpe);
                dsObj.Tables.Add(dtTMPrjExpe);
                errcode = dalObj.SaveProjectExpenses(dsObj, UserId);
                
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return errcode;
        }
       #endregion project Expenses

        #region Project Expenses Approval 
        public List<ProjectExpensesModel> TMProjectExpensesApprovalList(string User_Id)
        {
            List<ProjectExpensesModel> LstExpenses = new List<ProjectExpensesModel>();
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.ProjectExpensesApprovalList(User_Id);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjectExpensesModel ProExpModel = new ProjectExpensesModel();
                            ProExpModel.ExpenseId = Convert.ToInt32(dr["ExpenseId"].ToString());
                            //ProExpModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            ProExpModel.Employee = dr["Employee"].ToString();
                            // ProExpModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            ProExpModel.ProjectName = dr["ProjectName"].ToString();
                            // ProExpModel.TaskId = Convert.ToInt32(dr["TaskID"].ToString());
                            ProExpModel.TaskName = dr["TaskName"].ToString();
                            ProExpModel.Remark = dr["Remark"].ToString();
                            ProExpModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            ProExpModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            ProExpModel.ExpenseDate = Convert.ToDateTime(dr["ExpenseDate"].ToString());
                            ProExpModel.StatusCode = dr["StatusShortCode"].ToString();
                            ProExpModel.Amount = Convert.ToDouble(dr["Amount"].ToString());
                            LstExpenses.Add(ProExpModel);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstExpenses;
        }
        public int ExpenseSendForApprove(int statusCode, string User_Id, int ExpenseId)
        {
            try
            {
                ProjectDal objdal = new ProjectDal();
                return objdal.ExpenseSendForApprove(statusCode, User_Id, ExpenseId);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public int SaveApvRejExpenses(ProjectExpensesModel model,string User_Id)
        {
            int errcode = 0;
            try
            {
                 ProjectDal objProj = new ProjectDal();
                 errcode = objProj.SaveApvRejExpenses(model.ExpenseId, model.ManagerRemark, model.Status, User_Id);
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            return errcode;
        }
        #endregion Project Expenses Approval

        #region Project Expenses Approval by Account
        public List<ProjectExpensesModel> TMProjectExpensesApprovalListbyAccount(string User_Id)
        {
            List<ProjectExpensesModel> LstExpenses = new List<ProjectExpensesModel>();
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.ProjectExpensesApprovalListbyAccount(User_Id);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                ProjectExpensesModel ProExpModel = new ProjectExpensesModel();
                                ProExpModel.ExpenseId = Convert.ToInt32(dr["ExpenseId"].ToString());
                                //ProExpModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                                ProExpModel.Employee = dr["Employee"].ToString();
                                // ProExpModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                                ProExpModel.ProjectName = dr["ProjectName"].ToString();
                                // ProExpModel.TaskId = Convert.ToInt32(dr["TaskID"].ToString());
                                ProExpModel.TaskName = dr["TaskName"].ToString();
                                ProExpModel.Remark = dr["Remark"].ToString();
                                ProExpModel.ModifiedBy = dr["ModifiedBy"].ToString();
                                ProExpModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                ProExpModel.ExpenseDate = Convert.ToDateTime(dr["ExpenseDate"].ToString());
                                ProExpModel.StatusCode = dr["StatusShortCode"].ToString();
                                ProExpModel.Amount = Convert.ToDouble(dr["Amount"].ToString());
                                LstExpenses.Add(ProExpModel);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstExpenses;
        }

        public ProjectExpensesModel TMGetSelectedProjectExpensesListbyAccount(int ProjectExpensesId)
        {
            ProjectExpensesModel ProExpModel = new ProjectExpensesModel();
            ProExpModel.LstExpDoc = new List<ExpensesDocModel>();
            try
            {
                ProjectDal objProj = new ProjectDal();
                DataSet ds = new DataSet();
                ds = objProj.GetSelectedProjectExpensesListbyAccount(ProjectExpensesId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProExpModel.ExpenseId = Convert.ToInt32(dr["ExpenseId"].ToString());
                            //ProExpModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                           
                           // ProExpModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            ProExpModel.ProjectName = dr["ProjectName"].ToString();
                         ///   ProExpModel.TaskId = Convert.ToInt32(dr["TaskID"].ToString());
                            ProExpModel.TaskName = dr["TaskName"].ToString();
                            ProExpModel.Remark = dr["Remark"].ToString();
                            ProExpModel.Description = dr["Description"].ToString();
                            ProExpModel.ExpenseType = dr["ExpenceType"].ToString();
                            ProExpModel.ApproverName = dr["ApprovedBy"].ToString();
                            ProExpModel.ApproverRemark = dr["ApproverRemark"].ToString();
                            ProExpModel.ApproverDate = Convert.ToDateTime(dr["ApproverDate"].ToString());
                            ProExpModel.StatusCode = dr["StatusShortCode"].ToString();
                            ProExpModel.Amount = Convert.ToDouble(dr["Amount"].ToString());
                            ProExpModel.ExpenseDate = Convert.ToDateTime(dr["ExpenseDate"].ToString());
                            ProExpModel.CreatedBy = dr["CreatedBy"].ToString();
                            ProExpModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            ProExpModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            ProExpModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        }
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            ExpensesDocModel demo = new ExpensesDocModel();
                            demo.ExpenseId = Convert.ToInt32(dr["ExpenseId"].ToString());
                            demo.Description = dr["Description"].ToString();
                            demo.DocumentPath = dr["DocumentPath"].ToString();
                            ProExpModel.LstExpDoc.Add(demo);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ProExpModel;
        }

        public int SaveApvRejExpensesbyAccounts(ProjectExpensesModel model, string User_Id)
        {
            int errcode = 0;
            try
            {
                ProjectDal objProj = new ProjectDal();
                errcode = objProj.SaveApvRejExpensesByAccounts(model.ExpenseId, model.ManagerRemark, model.Status, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }
        #endregion Project Expenses Approval by Account

        #region Expense Type

        public List<ExpenseTypeModel> GetExpenseTypeList()
        {
            ProjectDal dalobj = new ProjectDal();
            List<ExpenseTypeModel> ExpenseTypeList = new List<ExpenseTypeModel>();
            DataSet ds = dalobj.GetExpenseTypeList();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ExpenseTypeModel Exptypemodel = new ExpenseTypeModel();
                        Exptypemodel.ExpTypeId = Convert.ToInt32(dr["ExpTypeId"]);
                        Exptypemodel.ExpenceType = dr["ExpenceType"].ToString();
                        Exptypemodel.Description = dr["Description"].ToString();
                        Exptypemodel.CreatedBy = dr["CreatedBy"].ToString(); ;
                        Exptypemodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        Exptypemodel.ModifiedBy = dr["ModifiedBy"].ToString();
                        Exptypemodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                        ExpenseTypeList.Add(Exptypemodel);

                    }
                }
            }
            return ExpenseTypeList;
        }

        public int SaveExpenseType(  ExpenseTypeModel model ,string User_id)
        {
            ProjectDal dalobj = new ProjectDal();
            DataSet ds = new DataSet();
            int errorcode = 0;
            try
            {            
            
                 ds = dalobj.GetSelectedExpenseType(0);
                if (ds != null)
                    ds.Tables[0].Rows.Clear();
                DataRow dr = ds.Tables[0].NewRow();
                dr["ExpTypeId"] = model.ExpTypeId;
                dr["ExpenceType"] = model.ExpenceType;
                dr["Description"] = model.Description;
                ds.Tables[0].Rows.Add(dr);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return errorcode = dalobj.SaveExpenseType(ds, User_id);
        }

        public ExpenseTypeModel GetselectedExpenseType(int exptypeid)
        {
            ExpenseTypeModel model = new ExpenseTypeModel();
         
            DataSet ds = new DataSet();
            ProjectDal dalobj = new ProjectDal();
            ds = dalobj.GetSelectedExpenseType(exptypeid);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        model.ExpTypeId = Convert.ToInt32(ds.Tables[0].Rows[0]["ExpTypeId"].ToString());
                        model.ExpenceType = ds.Tables[0].Rows[0]["ExpenceType"].ToString();
                        model.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                        model.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        model.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        model.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        model.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                    }
                }
            }

            return model;
        }

        #endregion Expense Type

        #region Project Equipment and Items

        public int SaveProjectEquipment(TMEquipmentModel Model)
        {
            int errCode = 0;
            ProjectDal BlObj = new ProjectDal();
            try
            {
                errCode = BlObj.SaveProjectEquipment(Model.ProjectId, Model.EquipmentId, Model.Quantity);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public List<TMEquipmentModel> ProjectEquipmentList(int ProjectId)
        {
            DataSet ds = new DataSet();
            ProjectDal BlObj = new ProjectDal();
            List<TMEquipmentModel> Modellst = new List<TMEquipmentModel>(); 
            try
            {
                ds = BlObj.ProjectEquipmentList(ProjectId);
               if(ds != null)
               {
                   if(ds.Tables.Count > 0)
                   {
                       if(ds.Tables[0].Rows.Count > 0)
                       {
                           foreach(DataRow dr in ds.Tables[0].Rows)
                           {
                               TMEquipmentModel Model = new TMEquipmentModel();
                               Model.ProjectName = dr["ProjectName"].ToString();
                               Model.EquipmentName = dr["EquipmentName"].ToString();
                               Model.Quantity = Convert.ToDouble(dr["Quantity"].ToString());
                               Modellst.Add(Model);
                           }
                       }
                   }
               }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Modellst;
        }

        public int SaveProjectItem(TMEquipmentItem Model)
        {
            int errCode = 0;
            ProjectDal BlObj = new ProjectDal();
            try
            {
                errCode = BlObj.SaveProjectItems(Model.ProjectId, Model.ItemId, Model.Quantity,Model.TAM);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public List<TMEquipmentItem> ProjectItemList(int ProjectId)
        {
            DataSet ds = new DataSet();
            ProjectDal BlObj = new ProjectDal();
            List<TMEquipmentItem> Modellst = new List<TMEquipmentItem>();
            try
            {
                ds = BlObj.ProjectItemList(ProjectId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                TMEquipmentItem Model = new TMEquipmentItem();
                                Model.ProjectName = dr["ProjectName"].ToString();
                                Model.ItemName = dr["ItemName"].ToString();
                                Model.Quantity = Convert.ToDouble(dr["Quantity"].ToString());
                                Model.TAM = Convert.ToDouble(dr["TAM"].ToString());
                                Modellst.Add(Model);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Modellst;
        }
        #endregion Project Equipment and Items
        public List<ProjectModel> GetProjectlistByUserId(string User_Id)
        {
            List<ProjectModel> lstProject = new List<ProjectModel>();
            ProjectDal objDL = new ProjectDal();
            DataSet dsData = objDL.GetProjectlistByUserId(User_Id);
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        ProjectModel proj = new ProjectModel();
                        proj.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                        proj.ProjectName = dr["ProjectName"].ToString();                       
                        lstProject.Add(proj);
                    }
            return lstProject;
        }
    }
}
