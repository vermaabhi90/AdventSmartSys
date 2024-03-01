using SmartSys.BL.Enquiry;
using SmartSys.BL.Project;
using SmartSys.BL.TimeManagement;
using SmartSys.BL.Tmleave;
using SmartSys.DAL;
using SmartSys.DAL.Project;
using SmartSys.DL.Enquiry;
using SmartSys.DL.ProViews;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.ProViews
{
    public class ViewsBL
    {
        public List<CaseRiskViewModel> GetCaseRiskList(string User_Id, string Filter)
        {
            List<CaseRiskViewModel> Model = new List<CaseRiskViewModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.GetCaseRiskList(User_Id, Filter);
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {
                                CaseRiskViewModel demo = new CaseRiskViewModel();
                                demo.ProjectId = Convert.ToInt32(dr["ProjectId"]);
                                demo.Status = Convert.ToInt32(dr["Status"]);
                                demo.TaskId = Convert.ToInt32(dr["TaskId"]);
                                demo.ProjectManager = dr["ProjectManager"].ToString();
                                demo.ProjectId = Convert.ToInt32(dr["ProjectId"]);
                                demo.ProjectName = dr["ProjectName"].ToString();
                                demo.CustomerName = dr["CustomerName"].ToString();
                                demo.VendorName = dr["VendorName"].ToString();
                                demo.TaskName = dr["TaskName"].ToString();
                                demo.Description = dr["Description"].ToString();
                                demo.StartDate = Convert.ToDateTime(dr["StartDate"]);
                                demo.EndDate = Convert.ToDateTime(dr["EndDate"]);
                                demo.TaskTypeText = dr["TaskTypeText"].ToString();
                                demo.ReviewedByName = dr["ReviewedByName"].ToString();
                                if (dr["ReviewedDate"].ToString() != "")
                                    demo.ReviewedDate = Convert.ToDateTime(dr["ReviewedDate"]);
                                demo.ApprovedByName = dr["ApprovedByName"].ToString();
                                if (dr["ApprovedDate"].ToString() != "")
                                    demo.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"]);
                                demo.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                                demo.ModifiedByName = dr["ModifiedByName"].ToString();
                                demo.StatusName = dr["StatusName"].ToString();
                                demo.Resources = dr["Resources"].ToString();
                                Model.Add(demo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Model;
        }
        public List<CaseRiskViewModel> GetPendingCaseRiskListByProject(string User_Id, int ProjectId)
        {
            List<CaseRiskViewModel> Model = new List<CaseRiskViewModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.GetRiskCasePendingByProject(User_Id, ProjectId);
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {
                                CaseRiskViewModel demo = new CaseRiskViewModel();
                                demo.ProjectId = Convert.ToInt32(dr["ProjectId"]);
                                demo.Status = Convert.ToInt32(dr["Status"]);
                                demo.TaskId = Convert.ToInt32(dr["TaskId"]);
                                demo.ProjectManager = dr["ProjectManager"].ToString();
                                demo.ProjectId = Convert.ToInt32(dr["ProjectId"]);
                                demo.ProjectName = dr["ProjectName"].ToString();
                                demo.CustomerName = dr["CustomerName"].ToString();
                                demo.VendorName = dr["VendorName"].ToString();
                                demo.TaskName = dr["TaskName"].ToString();
                                demo.Description = dr["Description"].ToString();
                                demo.StartDate = Convert.ToDateTime(dr["StartDate"]);
                                demo.EndDate = Convert.ToDateTime(dr["EndDate"]);
                                demo.TaskTypeText = dr["TaskTypeText"].ToString();
                                demo.ReviewedByName = dr["ReviewedByName"].ToString();
                                if (dr["ReviewedDate"].ToString() != "")
                                    demo.ReviewedDate = Convert.ToDateTime(dr["ReviewedDate"]);
                                demo.ApprovedByName = dr["ApprovedByName"].ToString();
                                if (dr["ApprovedDate"].ToString() != "")
                                    demo.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"]);
                                demo.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                                demo.ModifiedByName = dr["ModifiedByName"].ToString();
                                demo.StatusName = dr["StatusName"].ToString();
                                demo.Resources = dr["Resources"].ToString();
                                Model.Add(demo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Model;
        }
        public List<CaseRiskViewModel> GetApprovedCaseRiskListByUser(string User_Id)
        {
            List<CaseRiskViewModel> Model = new List<CaseRiskViewModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.GetCaseRiskList(User_Id, "");
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {
                                if (dr["Status"].ToString() == "24")
                                {
                                    CaseRiskViewModel demo = new CaseRiskViewModel();
                                    demo.ProjectId = Convert.ToInt32(dr["ProjectId"]);
                                    demo.Status = Convert.ToInt32(dr["Status"]);
                                    demo.TaskId = Convert.ToInt32(dr["TaskId"]);
                                    demo.ProjectManager = dr["ProjectManager"].ToString();
                                    demo.ProjectId = Convert.ToInt32(dr["ProjectId"]);
                                    demo.ProjectName = dr["ProjectName"].ToString();
                                    demo.CustomerName = dr["CustomerName"].ToString();
                                    demo.VendorName = dr["VendorName"].ToString();
                                    demo.TaskName = dr["TaskName"].ToString();
                                    demo.Description = dr["Description"].ToString();
                                    demo.StartDate = Convert.ToDateTime(dr["StartDate"]);
                                    demo.EndDate = Convert.ToDateTime(dr["EndDate"]);
                                    demo.TaskTypeText = dr["TaskTypeText"].ToString();
                                    demo.ReviewedByName = dr["ReviewedByName"].ToString();
                                    if (dr["ReviewedDate"].ToString() != "")
                                        demo.ReviewedDate = Convert.ToDateTime(dr["ReviewedDate"]);
                                    demo.ApprovedByName = dr["ApprovedByName"].ToString();
                                    if (dr["ApprovedDate"].ToString() != "")
                                        demo.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"]);
                                    demo.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                                    demo.ModifiedByName = dr["ModifiedByName"].ToString();
                                    demo.StatusName = dr["StatusName"].ToString();
                                    demo.Resources = dr["Resources"].ToString();
                                    Model.Add(demo);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Model;
        }
        public List<TimeSheetViewModel> GetTimeSheetEnterList(string User_Id, int FromWeek, int ToWeek, int EmpId,int Year)
        {
            List<TimeSheetViewModel> Model = new List<TimeSheetViewModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.GetTimeSheetEntryList(User_Id, FromWeek, ToWeek, EmpId, Year);
                int Count = dsUser.Tables[0].Rows.Count;
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {
                                TimeSheetViewModel demo = new TimeSheetViewModel();
                                demo.TimeSheetId = Convert.ToInt32(dr["TimeSheetId"]);
                                demo.TimeSheetEntryDetailId = Convert.ToInt32(dr["TimeSheetEntryDetailId"]);
                                demo.EmployeeName = dr["EmployeeName"].ToString();
                                demo.TimeSheetEntryId = Convert.ToInt32(dr["TimeSheetEntryId"]);
                                demo.WeekNo = Convert.ToInt32(dr["WeekNo"]);
                                demo.ProjectName = dr["ProjectName"].ToString();
                                demo.TaskName = dr["TaskName"].ToString();
                                demo.StartDate = Convert.ToDateTime(dr["StartDate"]);
                                demo.StartDateStr = demo.StartDate.ToShortDateString();
                                demo.StatusShortCode = dr["StatusShortCode"].ToString();
                                if (dr["ApproverName"].ToString() != "")
                                    demo.ApproverName = dr["ApproverName"].ToString();
                                if (dr["ApprovedDate"].ToString() != "")
                                {
                                    demo.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"]);
                                    demo.ApprovedDateStr = demo.ApprovedDate.ToShortDateString();
                                }
                                else
                                {
                                    demo.ApprovedDateStr = "";
                                }
                                demo.ApproverRemark = dr["ApproverRemark"].ToString();
                                demo.Remark = dr["Remark"].ToString();
                                demo.CreatedByName = dr["CreatedByName"].ToString();
                                demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                                demo.CreatedDateStr = demo.CreatedDate.ToShortDateString();
                                demo.ModifiedByName = dr["ModifiedByName"].ToString();
                                demo.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                                demo.ModifiedDateStr = demo.ModifiedDate.ToShortDateString();
                                demo.TimeSheetEntryId = Convert.ToInt32(dr["TimeSheetEntryId"]);
                                demo.DayText = dr["DayText"].ToString();
                                demo.CustomerName = dr["CustomerName"].ToString();
                                demo.VendorName = dr["VendorName"].ToString();
                                demo.MOMTitle = dr["MOMTitle"].ToString();
                                demo.FromTime = dr["FromTime"].ToString();
                                demo.ToTime = dr["ToTime"].ToString();
                                demo.TotalTime = Convert.ToInt32(dr["TotalTime"].ToString());
                                if (dr["MOMId"].ToString() != "")
                                {
                                    demo.MOMId = Convert.ToInt32(dr["MOMId"]);
                                }
                                else
                                {
                                    demo.MOMId = 0;
                                }
                                demo.BudgetCustomer = dr["IsBudgeted"].ToString();
                                Model.Add(demo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Model;
        }
        public List<TimeSheetViewModel> GetTimeSheetListByEmployee(string User_Id, int CustomerId, int EmpId)
        {
            List<TimeSheetViewModel> Model = new List<TimeSheetViewModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.GetTimeSheetListByEmployee(User_Id, CustomerId, EmpId);
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {
                                TimeSheetViewModel demo = new TimeSheetViewModel();
                                demo.EmployeeName = dr["EmployeeName"].ToString();
                                demo.CustomerName = dr["CustomerName"].ToString();
                                demo.ExactMonth = dr["ExactMonth"].ToString();
                                demo.Quarter = dr["Quarter"].ToString();
                                demo.Year = dr["Year"].ToString();
                                demo.TotalTime =Convert.ToInt32(dr["TotalTime"].ToString());
                                Model.Add(demo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Model;
        }
        #region SalesBacklogList
        public List<SalesBacklogViewModel> GetSalesBacklogList(string User_Id)
        {
            List<SalesBacklogViewModel> Model = new List<SalesBacklogViewModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.GetSalesBacklogList(User_Id);
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {
                                SalesBacklogViewModel demo = new SalesBacklogViewModel();
                                demo.Company = dr["Company"].ToString();
                                demo.DocumentNo = dr["Document No_"].ToString();
                                demo.ItemNo = dr["No_"].ToString();
                                demo.LineNo = Convert.ToInt32(dr["Line No_"].ToString());
                                demo.BillToName = dr["Bill-to Name"].ToString();
                                demo.Description = dr["Description"].ToString();
                                demo.CustomerOrderNo_ = dr["Customer Order No_"].ToString();
                                demo.Cust_PO_Date = Convert.ToDateTime(dr["Cust_PO_Date"]);
                                demo.Posting_Date = Convert.ToDateTime(dr["Posting_Date"]);
                                demo.CRD = Convert.ToDateTime(dr["CRD"]);
                                demo.VPD = Convert.ToDateTime(dr["VPD"]);
                                demo.VendorPartNo = dr["Vendor Part No_"].ToString();
                                demo.Make = dr["Shortcut Dimension 2 Code"].ToString();
                                demo.UnitPrice = Convert.ToDouble(dr["Unit Price"]);
                                demo.Quantity = Convert.ToDouble(dr["Quantity"]);
                                if (dr["Status"].ToString() != "")
                                {
                                    demo.Status = Convert.ToInt32(dr["Status"]);
                                }

                                demo.LineAmount = Convert.ToDouble(dr["Line Amount"]);
                                demo.OutstandingQuantity = Convert.ToDouble(dr["Outstanding Quantity"]);
                                demo.OutstandingAmount = Convert.ToDouble(dr["Outstanding Amount"]);
                                demo.Item_Qty_On_Hand = Convert.ToDouble(dr["Item_Qty_On_Hand"]);
                                demo.Qty_On_PO = Convert.ToDouble(dr["Qty_On_PO"]);
                                demo.SalespersonCode = dr["Salesperson Code"].ToString();
                                demo.Full_Name = dr["Full_Name"].ToString();
                                Model.Add(demo);

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Model;

        }
        public int SaveSalesBacklogComment(SalesBacklogViewModel model, string User_id, int Status)
        {
            ViewsDAL objDAL = new ViewsDAL();
            DataSet dsUser = new DataSet();
            int errorcode = 0;
            dsUser = objDAL.GetSelectedBacklogs(0);
            try
            {

                dsUser.Tables[0].Rows.Clear();
                DataRow dr = dsUser.Tables[0].NewRow();
                int BacklogCommentId = 0;
                // dr["BacklogCommentId"] = model.BacklogCommentId;
                dr["OrderNo"] = model.OrderNo;

                dr["Line_No"] = model.LineNo;
                dr["Comment"] = model.Comments;
                dr["CompCode"] = model.Company;
                dsUser.Tables[0].Rows.Add(dr);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errorcode = objDAL.SaveSalesBacklogComment(dsUser, User_id, Status);
        }

        public List<Commentmodel> GetselectedBacklogComments(string Orderno)
        {

            List<Commentmodel> list = new List<Commentmodel>();
            SalesBacklogViewModel model = new SalesBacklogViewModel();
            model.CommentsList = new List<Commentmodel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.GetSelectedBacklogComments(Orderno);
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {
                                Commentmodel demo = new Commentmodel();
                                demo.DocumentNo = dr["OrderNo"].ToString();
                                demo.LineNo = Convert.ToInt32(dr["Line_No"].ToString());
                                demo.Comments = dr["Comment"].ToString();
                                demo.Company = dr["CompanyName"].ToString();
                                demo.CommentDate = Convert.ToDateTime(dr["CommentDate"].ToString());
                                list.Add(demo);

                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return list;

        }
        #endregion SalesBacklogList

        #region Pending MOM Action Point
        public List<PendingMOMActions> PendingMOMActionPoint(string User_Id)
        {
            List<PendingMOMActions> list = new List<PendingMOMActions>();

            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetPendingMomActionPoint(User_Id);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PendingMOMActions ActionPoint = new PendingMOMActions();
                            ActionPoint.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                            ActionPoint.ActionDescription = dr["ActionDescription"].ToString();
                            ActionPoint.StatusShortCode = dr["Status"].ToString();
                            ActionPoint.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            ActionPoint.MOMDueDate = Convert.ToDateTime(dr["DueDate"].ToString());
                            ActionPoint.OverDueDays = Convert.ToInt32(dr["OverDueDays"].ToString());
                            ActionPoint.AssignedBy = dr["AssignedBy"].ToString();
                            ActionPoint.Resource = dr["Resource"].ToString();
                            ActionPoint.Customer = dr["CustomerName"].ToString();
                            ActionPoint.Vendor = dr["VendorName"].ToString();
                            ActionPoint.ModifiedByName = dr["ModifiedBy"].ToString();
                            ActionPoint.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            list.Add(ActionPoint);

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public List<PendingMOMActions> PendingSubordinateMOMActionPoint(string User_Id)
        {
            List<PendingMOMActions> list = new List<PendingMOMActions>();

            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetSubordinatePendingMomActionPoint(User_Id);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PendingMOMActions ActionPoint = new PendingMOMActions();
                            ActionPoint.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                            ActionPoint.ActionDescription = dr["ActionDescription"].ToString();
                            ActionPoint.StatusShortCode = dr["Status"].ToString();
                            ActionPoint.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            ActionPoint.AssignedBy = dr["AssignedBy"].ToString();
                            ActionPoint.Resource = dr["Resource"].ToString() + ',' + dr["Vendor"].ToString() + ',' + dr["Customer"].ToString();
                            ActionPoint.ModifiedByName = dr["ModifiedBy"].ToString();

                            ActionPoint.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            list.Add(ActionPoint);

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public List<PendingMOMActions> CompleteMOMActionPoint(string User_Id)
        {
            List<PendingMOMActions> list = new List<PendingMOMActions>();

            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetCompletedMomActionPoint(User_Id);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PendingMOMActions ActionPoint = new PendingMOMActions();
                            ActionPoint.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                            ActionPoint.ActionDescription = dr["ActionDescription"].ToString();
                            ActionPoint.StatusShortCode = dr["Status"].ToString();
                            ActionPoint.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            ActionPoint.AssignedBy = dr["AssignedBy"].ToString();
                            ActionPoint.Resource = dr["Resource"].ToString();
                            ActionPoint.ModifiedByName = dr["ModifiedBy"].ToString();

                            ActionPoint.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            list.Add(ActionPoint);

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public List<PendingMOMActions> CompleteSubordinateMOMActionPoint(string User_Id)
        {
            List<PendingMOMActions> list = new List<PendingMOMActions>();

            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetCompletedSubordinateMomActionPoint(User_Id);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PendingMOMActions ActionPoint = new PendingMOMActions();
                            ActionPoint.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                            ActionPoint.ActionDescription = dr["ActionDescription"].ToString();
                            ActionPoint.StatusShortCode = dr["Status"].ToString();
                            ActionPoint.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            ActionPoint.AssignedBy = dr["AssignedBy"].ToString();
                            ActionPoint.Resource = dr["Resource"].ToString();
                            ActionPoint.ModifiedByName = dr["ModifiedBy"].ToString();

                            ActionPoint.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            list.Add(ActionPoint);

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public List<PendingMOMActions> PendingMOMActionPointBYMe(string User_Id)
        {
            List<PendingMOMActions> list = new List<PendingMOMActions>();

            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetPendingMomActionPointByMe(User_Id);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PendingMOMActions ActionPoint = new PendingMOMActions();
                            ActionPoint.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                            ActionPoint.ActionDescription = dr["ActionDescription"].ToString();
                            ActionPoint.StatusShortCode = dr["Status"].ToString();
                            ActionPoint.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            ActionPoint.AssignedBy = dr["AssignedBy"].ToString();
                            ActionPoint.Resource = dr["Resource"].ToString();
                            ActionPoint.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            ActionPoint.ModifiedByName = dr["ModifiedBy"].ToString();
                            ActionPoint.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            list.Add(ActionPoint);

                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        #endregion MOM action pOInt

        public List<CustomerEnqStatus> EnqStatusList(int EnqId)
        {
            List<CustomerEnqStatus> StatusLst = new List<CustomerEnqStatus>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.EnqStatusList(EnqId);
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {


                                CustomerEnqStatus demo = new CustomerEnqStatus();
                                demo.StatusId = Convert.ToInt32(dr["StatusId"]);
                                demo.SeqNo = Convert.ToInt32(dr["SeqNo"]);
                                demo.LableDesc = dr["LabelDesc"].ToString();
                                demo.tooltiptext = dr["tooltiptext"].ToString();
                                demo.CurrentState = dr["CurrentState"].ToString();
                                StatusLst.Add(demo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return StatusLst;
        }
        public MailEnquiryTimespan MailEnqTimespan(int EnqId)
        {
            MailEnquiryTimespan model = new MailEnquiryTimespan();
            model.MailEnqTimespanLst = new List<MailEnquiryTimespan>();
            List<MailEnquiryTimespan> MailEnqTimespanLst = new List<MailEnquiryTimespan>();

            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.GetMailEnqTimeSpan(EnqId);
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {


                                MailEnquiryTimespan demo = new MailEnquiryTimespan();
                                demo.MinuteDiff = Convert.ToInt32(dr["MinuteDiff"]);
                                demo.ID = Convert.ToInt32(dr["ID"]);
                                demo.OutstandingResponseTime = Convert.ToInt32(dr["OutstandingResponseTime"]);
                                demo.MinimumTime = Convert.ToInt32(dr["MinimumTime"]);
                                demo.MaximumTime = Convert.ToInt32(dr["MaximumTime"]);
                                demo.StatusDesc = dr["StatusDesc"].ToString();
                                if (demo.MinuteDiff > demo.MaximumTime)
                                {
                                    demo.TimeAxis = Convert.ToInt32(dr["MinuteDiff"]);
                                }
                                else
                                {
                                    demo.TimeAxis = Convert.ToInt32(dr["MaximumTime"]);
                                }
                                demo.TimeAxis = (Convert.ToInt32(((demo.TimeAxis + 100) / 100)) * 100);
                                model.MailEnqTimespanLst.Add(demo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }
        public int CheckEnquirytoPerson(int EnqId, string User_Id)
        {
            ViewsDAL objDAL = new ViewsDAL();
            EnqId = objDAL.CheckEnquirytoPerson(EnqId, User_Id);
            return EnqId;
        }
        public EnquiryTrackingViewDetail MailEnqTimeDifference(int EnqId, string PersonType)
        {
            EnquiryTrackingViewDetail model = new EnquiryTrackingViewDetail();
            model.EMailEnqTeackingLst = new List<EnquiryTrackingViewDetail>();
            model.EnqTrackdetailsLst = new List<EnquiryTrackingViewDetail>();
            List<EnquiryTrackingViewDetail> EMailEnqTeackingLst = new List<EnquiryTrackingViewDetail>();
            model.TestLst = new List<EnquiryTrackingViewDetailChart>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.GetMailEnqTimeDifference(EnqId);
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {
                                string[] strParent;
                                EnquiryTrackingViewDetailChart demo = new EnquiryTrackingViewDetailChart();
                                demo.Enqid = EnqId;
                                demo.SourceId = Convert.ToInt32(dr["SourceId"]);
                                demo.SId = Convert.ToInt32(dr["SId"]);
                                demo.DataLevel = Convert.ToInt32(dr["DataLevel"]);
                                if (PersonType == "Sales" && demo.DataLevel == 6)
                                {
                                    demo.title = "Item Allocate to Vendor";
                                    demo.description = "Item Allocate to Vendor";
                                }
                                else if (PersonType == "Sales" && demo.DataLevel == 7)
                                {
                                    demo.title = "Enquiry Send to Vendor";
                                    demo.description = "Enquiry Send to Vendor";
                                }
                                else if (PersonType == "Sales" && demo.DataLevel == 8)
                                {
                                    demo.title = "Vendor Response recived";
                                    demo.description = "Vendor Response recived";
                                }
                                else
                                {
                                    demo.title = dr["Title"].ToString();
                                    demo.description = dr["Description"].ToString();
                                }
                                demo.itemTitleColor = dr["itemTitleColor"].ToString();
                                demo.id = Convert.ToInt32(dr["Id"]);
                                strParent = dr["Parent"].ToString().Split(',');
                                demo.parents = strParent;
                                demo.MinuteDiff = Convert.ToInt32(dr["MinuteDiff"]);
                                demo.OTRT = Convert.ToInt32(dr["OTRT"]);
                                demo.MinimumTime = Convert.ToInt32(dr["MinimumTime"]);
                                demo.MaximumTime = Convert.ToInt32(dr["MaximumTime"]);
                                //demo.TimeAxis = Convert.ToInt32(dr["MaximumTime"]) + 100;
                                if (demo.MinuteDiff > demo.MaximumTime)
                                {
                                    demo.TimeAxis = (Convert.ToInt32(dr["MinuteDiff"]) + 100);
                                }
                                else
                                {
                                    demo.TimeAxis = (Convert.ToInt32(dr["MaximumTime"]) + 100);
                                }

                                model.TestLst.Add(demo);
                            }
                        }
                        if (dsUser.Tables[1].Rows.Count > 0)
                        {

                            model.CustomerId = Convert.ToInt32(dsUser.Tables[1].Rows[0]["CustomerId"]);
                            model.CustomerName = dsUser.Tables[1].Rows[0]["CustomerName"].ToString();
                            model.EnqiryGetBy = dsUser.Tables[1].Rows[0]["EnqiryGetBy"].ToString();
                            model.EnqiryDate = Convert.ToDateTime(dsUser.Tables[1].Rows[0]["EnqiryDate"]);
                            model.PreprationDate = Convert.ToDateTime(dsUser.Tables[1].Rows[0]["PreprationDate"]);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex) ;
            }
            return model;
        }
        public List<CustomerDrpModel> CustomerListBySalesPerson(string User_Id)
        {
            List<CustomerDrpModel> CustomerLst = new List<CustomerDrpModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.CustomerListBySalesPerson(User_Id);
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {
                                CustomerDrpModel demo = new CustomerDrpModel();
                                demo.CustomerId = Convert.ToInt32(dr["CustomerId"]);
                                demo.CustomerName = dr["CustomerName"].ToString();
                                CustomerLst.Add(demo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return CustomerLst;
        }
        public List<QoutFollowupModel> GetFindQuotationData(string Type, int CustomerId)
        {
            List<QoutFollowupModel> LstQuotModel = new List<QoutFollowupModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetFindData(Type, CustomerId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            QoutFollowupModel tempQuotModel = new QoutFollowupModel();
                            tempQuotModel.QuotationId = Convert.ToInt32(dr["QuotationId"]);
                            tempQuotModel.QuotationNumber = dr["QuotationNumber"].ToString();
                            tempQuotModel.EnqId = Convert.ToInt32(dr["EnqId"]);
                            tempQuotModel.QuotCost = Convert.ToDouble(dr["QuotCost"]);
                            tempQuotModel.CustomerName = dr["CustomerName"].ToString();
                            tempQuotModel.SalesPersonName = dr["SalesPerson"].ToString();
                            tempQuotModel.Term = dr["Term"].ToString();
                            tempQuotModel.CompCode = dr["CompCode"].ToString();
                            tempQuotModel.Currency = dr["Currancy"].ToString();
                            tempQuotModel.CreatedBy = dr["CreatedBy"].ToString();
                            tempQuotModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            tempQuotModel.CreatedDatestr = tempQuotModel.CreatedDate.ToShortDateString();
                            LstQuotModel.Add(tempQuotModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstQuotModel;
        }
        public List<EnquiryModel> GetFindEnqData(string Type, int CustomerId)
        {
            List<EnquiryModel> LstEnqModel = new List<EnquiryModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetFindData(Type, CustomerId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EnquiryModel tempEnqModel = new EnquiryModel();
                            tempEnqModel.EnqId = Convert.ToInt32(dr["EnqId"]);
                            tempEnqModel.EnqNumber = dr["EnqNumber"].ToString();
                            tempEnqModel.EnqDate = Convert.ToDateTime(dr["EnqDate"]);
                            tempEnqModel.ModifiedBy = tempEnqModel.EnqDate.ToShortDateString();
                            tempEnqModel.CustomerName = dr["CustomerName"].ToString();
                            tempEnqModel.SalesPerson = dr["SalesPerson"].ToString();
                            tempEnqModel.Remark = dr["Remark"].ToString();
                            tempEnqModel.StatusStr = dr["StatusStr"].ToString();
                            tempEnqModel.Priority = dr["Priority"].ToString();
                            tempEnqModel.ContactName = dr["ContactName"].ToString();
                            tempEnqModel.CompCode = dr["CompCode"].ToString();
                            LstEnqModel.Add(tempEnqModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstEnqModel;
        }
        #region DataLevel

        public EnquiryModel DataLevelOne(int Enqid, int DataLevel)
        {
            EnquiryModel objmodel = new EnquiryModel();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelOne(Enqid, DataLevel);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            objmodel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            objmodel.EnqNumber = dr["EnqNumber"].ToString();
                            objmodel.EnqDate = Convert.ToDateTime(dr["EnqDate"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.ContactName = dr["ContactName"].ToString();
                            objmodel.Remark = dr["Remark"].ToString();
                            objmodel.Priority = dr["Priority"].ToString();
                            objmodel.CompCode = dr["CompCode"].ToString();
                            if (dr["EnqType"].ToString() == "0")
                            {
                                objmodel.Types = "Manual";
                            }
                            else
                            {
                                objmodel.Types = "By Mail";
                            }
                            objmodel.CreatedBy = dr["CreatedBy"].ToString();
                            objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            objmodel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objmodel;
        }
        public DataLevelTwo DataLevelTwo(int Enqid, int DataLevel, int SourceId)
        {
            DataLevelTwo LeveltwoModel = new DataLevelTwo();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelTwo(Enqid, DataLevel, SourceId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LeveltwoModel.ItemId = Convert.ToInt32(ds.Tables[0].Rows[0]["ItemId"].ToString());
                        LeveltwoModel.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                        LeveltwoModel.MPN = ds.Tables[0].Rows[0]["MPN"].ToString();
                        LeveltwoModel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                        if (ds.Tables[0].Rows[0]["Quantity"].ToString() != "")
                            LeveltwoModel.Quantity = Convert.ToInt32(ds.Tables[0].Rows[0]["Quantity"].ToString());
                        if (ds.Tables[0].Rows[0]["ExpectedDate"].ToString() != "")
                            LeveltwoModel.ExpectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ExpectedDate"].ToString());
                        LeveltwoModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        if (ds.Tables[0].Rows[0]["CreatedDate"].ToString() != "")
                            LeveltwoModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        LeveltwoModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LeveltwoModel;
        }
        public DataLevelThree DataLevelThree(int Enqid, int DataLevel, int SourceId)
        {
            DataLevelThree LevelThreeModel = new DataLevelThree();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelThree(Enqid, DataLevel, SourceId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelThreeModel.PreparedBy = ds.Tables[0].Rows[0]["PreparedBy"].ToString();
                        if (ds.Tables[0].Rows[0]["PreparedDate"].ToString() != "")
                            LevelThreeModel.PreparedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PreparedDate"].ToString());
                        LevelThreeModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelThreeModel;
        }
        public DataLevelFour DataLevelFour(int Enqid, int DataLevel, int SourceId)
        {
            DataLevelFour LevelFourModel = new DataLevelFour();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelFour(Enqid, DataLevel, SourceId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelFourModel.AssignedBy = ds.Tables[0].Rows[0]["AssignedBy"].ToString();
                        LevelFourModel.AssignedTo = ds.Tables[0].Rows[0]["AssignedTo"].ToString();
                        if (ds.Tables[0].Rows[0]["AssignedDate"].ToString() != "")
                            LevelFourModel.AssignedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["AssignedDate"].ToString());
                        LevelFourModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelFourModel;
        }
        public DataLevelFive DataLevelFive(int Enqid, int DataLevel, int SourceId)
        {
            DataLevelFive LevelFiveModel = new DataLevelFive();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelFive(Enqid, DataLevel, SourceId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelFiveModel.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                        LevelFiveModel.MPN = ds.Tables[0].Rows[0]["MPN"].ToString();
                        LevelFiveModel.Quantity = Convert.ToInt32(ds.Tables[0].Rows[0]["Quantity"].ToString());
                        if (ds.Tables[0].Rows[0]["ExpectedDate"].ToString() != "")
                            LevelFiveModel.ExpectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ExpectedDate"].ToString());
                        LevelFiveModel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelFiveModel;
        }
        public DataLevelSix DataLevelSix(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelSix DTLevelSixModel = new DataLevelSix();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelSix(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DTLevelSixModel.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                            DTLevelSixModel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                            DTLevelSixModel.VendorName = ds.Tables[0].Rows[0]["VendorName"].ToString();
                            DTLevelSixModel.AllocatedBy = ds.Tables[0].Rows[0]["AllocatedBy"].ToString();
                            DTLevelSixModel.MPN = ds.Tables[0].Rows[0]["MPN"].ToString();
                            DTLevelSixModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DTLevelSixModel;
        }
        public DataLevelSeven DataLevelSeven(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelSeven DTLevelSevenModel = new DataLevelSeven();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelSeven(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DTLevelSevenModel.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                            DTLevelSevenModel.VendorName = ds.Tables[0].Rows[0]["VendorName"].ToString();
                            DTLevelSevenModel.MPN = ds.Tables[0].Rows[0]["MPN"].ToString();
                            DTLevelSevenModel.SendBy = ds.Tables[0].Rows[0]["SendBy"].ToString();
                            DTLevelSevenModel.ExpectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ExpectedDate"].ToString());
                            DTLevelSevenModel.SendDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SendDate"].ToString());
                            DTLevelSevenModel.Quantity = Convert.ToInt32(ds.Tables[0].Rows[0]["Quantity"].ToString());
                            DTLevelSevenModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DTLevelSevenModel;
        }
        public DataLevelEight DataLevelEight(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelEight DTLevelEightModel = new DataLevelEight();
            DTLevelEightModel.DetailList = new List<LevelEightDetail>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelEight(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DTLevelEightModel.ResponseId = Convert.ToInt32(ds.Tables[0].Rows[0]["ResponseId"].ToString());
                            DTLevelEightModel.VendorName = ds.Tables[0].Rows[0]["VendorName"].ToString();
                            DTLevelEightModel.ContactName = ds.Tables[0].Rows[0]["ContactName"].ToString();
                            DTLevelEightModel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                            DTLevelEightModel.Currency = ds.Tables[0].Rows[0]["Currency"].ToString();
                            DTLevelEightModel.ReciptDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ReciptDate"].ToString());
                            DTLevelEightModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            DTLevelEightModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            DTLevelEightModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                        }
                    }
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            LevelEightDetail DTLevelDetail = new LevelEightDetail();
                            DTLevelDetail.ItemName = ds.Tables[1].Rows[0]["ItemName"].ToString();
                            DTLevelDetail.BrandName = ds.Tables[1].Rows[0]["BrandName"].ToString();
                            DTLevelDetail.VendorPromiseDate = ds.Tables[1].Rows[0]["VendorPromiseDate"].ToString();
                            DTLevelDetail.Rate = Convert.ToDouble(ds.Tables[1].Rows[0]["Rate"].ToString());
                            DTLevelDetail.MinimumQty = Convert.ToInt32(ds.Tables[1].Rows[0]["MinimumQty"].ToString());
                            DTLevelDetail.MOQ = ds.Tables[1].Rows[0]["MOQ"].ToString();
                            DTLevelDetail.SPQ = ds.Tables[1].Rows[0]["SPQ"].ToString();
                            DTLevelDetail.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[1].Rows[0]["TimeTake"].ToString()));
                            DTLevelEightModel.DetailList.Add(DTLevelDetail);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DTLevelEightModel;
        }
        public DataLevelNine DataLevelNine(int Enqid, int DataLevel, int SourceId, int SId)
        {
            DataLevelNine DTLevelNineModel = new DataLevelNine();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelNine(Enqid, DataLevel, SourceId, SId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        DTLevelNineModel.ResPreparedBy = ds.Tables[0].Rows[0]["ResPreparedBy"].ToString();
                        if (ds.Tables[0].Rows[0]["ResPreparedDate"].ToString() != "")
                            DTLevelNineModel.ResPreparedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ResPreparedDate"].ToString());
                        DTLevelNineModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DTLevelNineModel;
        }
        public DataLevelTen DataLevelTen(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTen LevelFourModel = new DataLevelTen();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelTen(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelFourModel.AssignedBy = ds.Tables[0].Rows[0]["AssignedBy"].ToString();
                        LevelFourModel.AssignedTo = ds.Tables[0].Rows[0]["AssignedTo"].ToString();
                        if (ds.Tables[0].Rows[0]["AssignedDate"].ToString() != "")
                            LevelFourModel.AssignedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["AssignedDate"].ToString());
                        LevelFourModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelFourModel;
        }
        public DataLevelEleven DataLevelEleven(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelEleven DTLevelEleven = new DataLevelEleven();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelEleven(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        DTLevelEleven.QuotationId = Convert.ToInt32(ds.Tables[0].Rows[0]["QuotationId"].ToString());
                        DTLevelEleven.QuotationNumber = ds.Tables[0].Rows[0]["QuotationNumber"].ToString();
                        DTLevelEleven.EnqId = Convert.ToInt32(ds.Tables[0].Rows[0]["EnqId"].ToString());
                        DTLevelEleven.Currancy = ds.Tables[0].Rows[0]["Currancy"].ToString();
                        DTLevelEleven.EnqId = Convert.ToInt32(ds.Tables[0].Rows[0]["EnqId"].ToString());
                        DTLevelEleven.QuotDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["QuotDate"].ToString());
                        DTLevelEleven.Terms = ds.Tables[0].Rows[0]["Terms"].ToString();
                        DTLevelEleven.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                        DTLevelEleven.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                        DTLevelEleven.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        DTLevelEleven.FileName = ds.Tables[0].Rows[0]["FileName"].ToString();
                        DTLevelEleven.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        DTLevelEleven.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DTLevelEleven;
        }
        public DataLevelTwelve DataLevelTwelve(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwelve DTLevelTwelve = new DataLevelTwelve();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelTwelve(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        DTLevelTwelve.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                        DTLevelTwelve.VendorRate = Convert.ToDouble(ds.Tables[0].Rows[0]["VendorRate"].ToString());
                        DTLevelTwelve.Rate = Convert.ToDouble(ds.Tables[0].Rows[0]["Rate"].ToString());
                        DTLevelTwelve.BrandName = ds.Tables[0].Rows[0]["BrandName"].ToString();
                        DTLevelTwelve.MPN = ds.Tables[0].Rows[0]["MPN"].ToString();
                        DTLevelTwelve.Quantity = Convert.ToInt32(ds.Tables[0].Rows[0]["Quantity"].ToString());
                        DTLevelTwelve.SPQ = ds.Tables[0].Rows[0]["SPQ"].ToString();
                        DTLevelTwelve.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                        DTLevelTwelve.MOQ = ds.Tables[0].Rows[0]["MOQ"].ToString();
                        DTLevelTwelve.VendorName = ds.Tables[0].Rows[0]["VendorName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DTLevelTwelve;
        }
        public DataLevelthirteen DataLevelThirteen(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelthirteen LevelThirteenModel = new DataLevelthirteen();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelthirteen(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelThirteenModel.QuotPreparedBy = ds.Tables[0].Rows[0]["QuotPreparedBy"].ToString();
                        if (ds.Tables[0].Rows[0]["QuotPreparedDate"].ToString() != "")
                            LevelThirteenModel.QuotPreparedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["QuotPreparedDate"].ToString());
                        LevelThirteenModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelThirteenModel;
        }
        public DataLevelFourteen DataLevelFourteen(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelFourteen LevelFourteenModel = new DataLevelFourteen();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelFourteen(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelFourteenModel.ApprovedBy = ds.Tables[0].Rows[0]["ApprovedBy"].ToString();
                        if (ds.Tables[0].Rows[0]["ApprovedDate"].ToString() != "")
                            LevelFourteenModel.ApprovedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ApprovedDate"].ToString());
                        LevelFourteenModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelFourteenModel;
        }
        public DataLevelfifteen DataLevelfifteen(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelfifteen LevelfifteenModel = new DataLevelfifteen();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelfifteen(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelfifteenModel.SendTo = ds.Tables[0].Rows[0]["SendTOCust"].ToString();
                        LevelfifteenModel.SendBy = ds.Tables[0].Rows[0]["SendBy"].ToString();
                        if (ds.Tables[0].Rows[0]["SendDate"].ToString() != "")
                            LevelfifteenModel.SendDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SendDate"].ToString());
                        LevelfifteenModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelfifteenModel;
        }
        public DataLevelSixteen DataLevelSixteen(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelSixteen LevelfifteenModel = new DataLevelSixteen();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelSixteen(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelfifteenModel.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                        LevelfifteenModel.MPN = ds.Tables[0].Rows[0]["MPN"].ToString();
                        LevelfifteenModel.BrandName = ds.Tables[0].Rows[0]["BrandName"].ToString();
                        LevelfifteenModel.Remark = ds.Tables[0].Rows[0]["BrandName"].ToString();
                        LevelfifteenModel.Rate = Convert.ToDouble(ds.Tables[0].Rows[0]["BrandName"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelfifteenModel;
        }
        public DataLevelSeventeen DataLevelSeventeen(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelSeventeen LevelSeventeenModel = new DataLevelSeventeen();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelSeventeen(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelSeventeenModel.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                        LevelSeventeenModel.DocumentFile = ds.Tables[0].Rows[0]["DocumentFile"].ToString();
                        LevelSeventeenModel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                        LevelSeventeenModel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                        LevelSeventeenModel.PODate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PODate"].ToString());
                        LevelSeventeenModel.SalesOrderNo_ = ds.Tables[0].Rows[0]["SalesOrderNo_"].ToString();
                        LevelSeventeenModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        LevelSeventeenModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        LevelSeventeenModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelSeventeenModel;
        }
        public DataLevelEighteen DataLevelEighteen(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelEighteen LevelEighteenModel = new DataLevelEighteen();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelEighteen(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelEighteenModel.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                        LevelEighteenModel.MPN = ds.Tables[0].Rows[0]["MPN"].ToString();
                        LevelEighteenModel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                        LevelEighteenModel.Rate = Convert.ToDouble(ds.Tables[0].Rows[0]["Rate"].ToString());
                        LevelEighteenModel.ExpectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ExpectedDate"].ToString());
                        LevelEighteenModel.Quantity = Convert.ToInt32(ds.Tables[0].Rows[0]["Quantity"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelEighteenModel;
        }
        public DataLevelNineteen DataLevelNineteen(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelNineteen LevelNineteenModel = new DataLevelNineteen();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelNineteen(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelNineteenModel.SendBy = ds.Tables[0].Rows[0]["SendBy"].ToString();
                        if (ds.Tables[0].Rows[0]["SendDate"].ToString() != "")
                            LevelNineteenModel.SendDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SendDate"].ToString());
                        LevelNineteenModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelNineteenModel;
        }
        public DataLevelTwenty DataLevelTwenty(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwenty LevelTwentyModel = new DataLevelTwenty();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelTwenty(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelTwentyModel.ApprovedBy = ds.Tables[0].Rows[0]["ApprovedBy"].ToString();
                        if (ds.Tables[0].Rows[0]["ApprovedDate"].ToString() != "")
                            LevelTwentyModel.ApprovedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ApprovedDate"].ToString());
                        LevelTwentyModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelTwentyModel;
        }
        public DataLevelTwentyOne DataLevelTwentyOne(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyOne LevelTwentyOneModel = new DataLevelTwentyOne();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelTwentyOne(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelTwentyOneModel.SalesOrderNo_ = ds.Tables[0].Rows[0]["SalesOrderNo_"].ToString();
                        LevelTwentyOneModel.SOCreatedBy = ds.Tables[0].Rows[0]["SOCreatedBy"].ToString();
                        if (ds.Tables[0].Rows[0]["SOCreatedDate"].ToString() != "")
                            LevelTwentyOneModel.SOCreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SOCreatedDate"].ToString());
                        LevelTwentyOneModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelTwentyOneModel;
        }
        public DataLevelTwentyTwo DataLevelTwentyTwo(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyTwo LevelTwentyTwoModel = new DataLevelTwentyTwo();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelTwentyTwo(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelTwentyTwoModel.SalesOrderNo_ = ds.Tables[0].Rows[0]["SalesOrderNo_"].ToString();
                        LevelTwentyTwoModel.PONumber = ds.Tables[0].Rows[0]["PONumber"].ToString();
                        if (ds.Tables[0].Rows[0]["PostingDate"].ToString() != "")
                            LevelTwentyTwoModel.PostingDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PostingDate"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelTwentyTwoModel;
        }
        public DataLevelTwentyThree DataLevelTwentyThree(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyThree LevelTwentyThreeModel = new DataLevelTwentyThree();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelTwentyThree(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelTwentyThreeModel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                        LevelTwentyThreeModel.VendorName = ds.Tables[0].Rows[0]["VendorName"].ToString();
                        if (ds.Tables[0].Rows[0]["ReceiptDate"].ToString() != "")
                            LevelTwentyThreeModel.ReceiptDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ReceiptDate"].ToString());
                        LevelTwentyThreeModel.FreightForwarder = ds.Tables[0].Rows[0]["FreightForwarder"].ToString();
                        LevelTwentyThreeModel.FFRef_No = ds.Tables[0].Rows[0]["FFRef_No"].ToString();
                        LevelTwentyThreeModel.Currency = ds.Tables[0].Rows[0]["Currency"].ToString();
                        LevelTwentyThreeModel.VendorInvNo = ds.Tables[0].Rows[0]["VendorInvNo"].ToString();
                        LevelTwentyThreeModel.VendorInvDate = ds.Tables[0].Rows[0]["VendorInvDate"].ToString();
                        LevelTwentyThreeModel.PermitType = ds.Tables[0].Rows[0]["PermitType"].ToString();
                        LevelTwentyThreeModel.ReceiptFile = ds.Tables[0].Rows[0]["ReceiptFile"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelTwentyThreeModel;
        }
        public DataLevelTwentyFour DataLevelTwentyFour(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyFour LevelTwentyFourModel = new DataLevelTwentyFour();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelTwentyFour(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelTwentyFourModel.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                        LevelTwentyFourModel.COO = ds.Tables[0].Rows[0]["COO"].ToString();
                        if (ds.Tables[0].Rows[0]["CreatedDate"].ToString() != "")
                            LevelTwentyFourModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        LevelTwentyFourModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        LevelTwentyFourModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                        LevelTwentyFourModel.Quantity = Convert.ToInt32(ds.Tables[0].Rows[0]["Quantity"].ToString());
                        LevelTwentyFourModel.InvoiceAmount = Convert.ToDouble(ds.Tables[0].Rows[0]["InvoiceAmount"].ToString());
                        LevelTwentyFourModel.PO_No = ds.Tables[0].Rows[0]["PO_No"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelTwentyFourModel;
        }
        public DataLevelTwentyFive DataLevelTwentyFive(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyFive LevelTwentyFiveModel = new DataLevelTwentyFive();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelTwentyFive(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelTwentyFiveModel.PermitType = ds.Tables[0].Rows[0]["PermitType"].ToString();
                        if (LevelTwentyFiveModel.PermitType == "ImportPermit")
                        {
                            LevelTwentyFiveModel.RefNo = ds.Tables[0].Rows[0]["RefNo"].ToString();
                            if (ds.Tables[0].Rows[0]["NotificationDate"].ToString() != "")
                                LevelTwentyFiveModel.NotificationDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["NotificationDate"].ToString());
                            if (ds.Tables[0].Rows[0]["ReceiptDate"].ToString() != "")
                                LevelTwentyFiveModel.ReceiptDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ReceiptDate"].ToString());
                            LevelTwentyFiveModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                            LevelTwentyFiveModel.GSTAmount = Convert.ToDouble(ds.Tables[0].Rows[0]["GSTAmount"].ToString());
                            LevelTwentyFiveModel.AirwayBillNo = ds.Tables[0].Rows[0]["AirwayBillNo"].ToString();
                            LevelTwentyFiveModel.VendorName = ds.Tables[0].Rows[0]["VendorName"].ToString();
                            LevelTwentyFiveModel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                            LevelTwentyFiveModel.FreightForwarder = ds.Tables[0].Rows[0]["FreightForwarder"].ToString();
                            LevelTwentyFiveModel.PermitNo = ds.Tables[0].Rows[0]["PermitNo"].ToString();
                            LevelTwentyFiveModel.Currency = ds.Tables[0].Rows[0]["Currency"].ToString();
                            LevelTwentyFiveModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            LevelTwentyFiveModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            LevelTwentyFiveModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelTwentyFiveModel;
        }
        public DataLevelTwentySix DataLevelTwentySix(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentySix LevelTwentySixModel = new DataLevelTwentySix();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelTwentySix(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelTwentySixModel.SendBy = ds.Tables[0].Rows[0]["SendBy"].ToString();
                        if (ds.Tables[0].Rows[0]["SendDate"].ToString() != "")
                            LevelTwentySixModel.SendDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["SendDate"].ToString());
                        LevelTwentySixModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelTwentySixModel;
        }
        public DataLevelTwentySeven DataLevelTwentySeven(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentySeven LevelTwentySevenModel = new DataLevelTwentySeven();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelTwentySeven(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelTwentySevenModel.ReviewedBy = ds.Tables[0].Rows[0]["ReviewedBy"].ToString();
                        if (ds.Tables[0].Rows[0]["ReviewedDate"].ToString() != "")
                            LevelTwentySevenModel.ReviewedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ReviewedDate"].ToString());
                        LevelTwentySevenModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelTwentySevenModel;
        }
        public DataLevelTwentyEight DataLevelTwentyEight(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyEight LevelTwentyEightModel = new DataLevelTwentyEight();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelTwentyEight(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelTwentyEightModel.ApprovedBy = ds.Tables[0].Rows[0]["ApprovedBy"].ToString();
                        if (ds.Tables[0].Rows[0]["ApprovedDate"].ToString() != "")
                            LevelTwentyEightModel.ApprovedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ApprovedDate"].ToString());
                        LevelTwentyEightModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelTwentyEightModel;
        }
        public DataLevelTwentyNine DataLevelTwentyNine(int Enqid, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyNine LevelTwentyNineModel = new DataLevelTwentyNine();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.DataLevelTwentyNine(Enqid, DataLevel, SourceId, Sid);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        LevelTwentyNineModel.PurchInvNo = ds.Tables[0].Rows[0]["PurchInvNo"].ToString();
                        LevelTwentyNineModel.ProcessBy = ds.Tables[0].Rows[0]["ProcessBy"].ToString();
                        if (ds.Tables[0].Rows[0]["ProcessDate"].ToString() != "")
                            LevelTwentyNineModel.ProcessDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ProcessDate"].ToString());
                        LevelTwentyNineModel.TimeTaken = Common.GetTimeINHHMM(Convert.ToInt32(ds.Tables[0].Rows[0]["TimeTake"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LevelTwentyNineModel;
        }
        #endregion DataLevel 

        #region Project View 
        public List<ProjectTaskMoM> GetProjectTaskMOMViewList(int ProjectId, int TaskId)
        {
            List<ProjectTaskMoM> MOMList = new List<ProjectTaskMoM>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetProjectTaskMOMViewList(ProjectId, TaskId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjectTaskMoM MOMModel = new ProjectTaskMoM();
                            MOMModel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            MOMModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            MOMModel.Employee = dr["EmpName"].ToString();
                            MOMModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            MOMModel.ProjectName = dr["ProjectName"].ToString();
                            MOMModel.MOMTypeKey = dr["MOMType"].ToString();
                            MOMModel.TaskId = Convert.ToInt32(dr["TaskID"].ToString());
                            MOMModel.TaskName = dr["TaskName"].ToString();
                            MOMModel.ModifiedByName = dr["ModifiedBy"].ToString();
                            MOMModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            MOMModel.ModifiedDateStr = MOMModel.ModifiedDate.ToString();
                            MOMModel.CreatedByName = dr["CreatedByname"].ToString();
                            MOMModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            MOMModel.CreatedDateStr = MOMModel.CreatedDate.ToString();
                            MOMModel.Description = dr["LocDesc"].ToString();
                            MOMModel.Title = dr["Title"].ToString();
                            MOMModel.MOMDate = Convert.ToDateTime(dr["MOMDate"].ToString());
                            MOMModel.MOMDateStr = MOMModel.MOMDate.Year.ToString();
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
        public List<ProjectTMView> GetProjectTaskTimeSheetList(int ProjectId, int TaskId)
        {
            List<ProjectTMView> TMList = new List<ProjectTMView>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetProjectTaskTimeSheetList(ProjectId, TaskId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjectTMView TMModel = new ProjectTMView();
                            TMModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            TMModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            TMModel.EmpName = dr["EmpName"].ToString();
                            TMModel.TotalTime = dr["TotalTime"].ToString();
                            TMList.Add(TMModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TMList;
        }
        public List<ProjectTMView> GetTaskList(int ProjectId, string User_Id)
        {
            List<ProjectTMView> objMast = new List<ProjectTMView>();
            ProjectDal objProj = new ProjectDal();
            DataSet dsData = objProj.GetProjectTaskList(ProjectId, User_Id);

            if (dsData != null)
                if (dsData.Tables.Count > 1)
                {
                    if (dsData.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsData.Tables[0].Rows)
                        {
                            ProjectTMView TMTempModel = new ProjectTMView();
                            TMTempModel.TaskId = Convert.ToInt16(dr["TaskId"].ToString());
                            TMTempModel.TaskName = dr["TaskName"].ToString();
                            objMast.Add(TMTempModel);
                        }
                    }
                }
            return objMast;

        }
        public List<TimeSheetViewModel> GetProjectTaskTimeSheetDetailList(int ProjectId, int EmpId, int taskId)
        {
            List<TimeSheetViewModel> TMList = new List<TimeSheetViewModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                if (taskId > 0)
                {
                    ds = objDAL.GetProjectPlanVsActualTimeSheetList(ProjectId, taskId);
                }
                else
                {
                    ds = objDAL.GetProjectTaskTimeSheetDetailList(ProjectId, EmpId);
                }
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            TimeSheetViewModel TempTMModel = new TimeSheetViewModel();
                            TempTMModel.EmployeeName = dr["EmpName"].ToString();
                            TempTMModel.TaskName = dr["TaskName"].ToString();
                            TempTMModel.FromTime = dr["FromTime"].ToString();
                            TempTMModel.ToTime = dr["ToTime"].ToString();
                            TempTMModel.DayText = dr["TMDay"].ToString();
                            TempTMModel.CreatedDate = Convert.ToDateTime(dr["TMDate"].ToString());
                            TempTMModel.DateText = TempTMModel.CreatedDate.ToString("dd/MMM/yyyy");
                            TempTMModel.TotalTime =Convert.ToInt32(dr["TotalTime"].ToString());
                            TMList.Add(TempTMModel);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TMList;
        }

        public List<PendingMOMActions> GetProjectViewPendActionPoint(int ProjectId)
        {
            List<PendingMOMActions> MOMActionList = new List<PendingMOMActions>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetProjectViewPendActionPoint(ProjectId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["StatusId"].ToString() != "30" && dr["StatusId"].ToString() != "31" && dr["StatusId"].ToString() != "32")
                            {
                                PendingMOMActions TempMOMActionModel = new PendingMOMActions();
                                TempMOMActionModel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                                TempMOMActionModel.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                                TempMOMActionModel.ActionDescription = dr["ActionDescription"].ToString();
                                TempMOMActionModel.StatusShortCode = dr["Status"].ToString();
                                TempMOMActionModel.AssignedBy = dr["AssignedBy"].ToString();
                                TempMOMActionModel.AssignedTo = dr["AssignedTo"].ToString();
                                if (dr["DueDate"].ToString() != "")
                                {
                                    DateTime DD = Convert.ToDateTime(dr["DueDate"].ToString());
                                    TempMOMActionModel.DueDate = DD.ToShortDateString();
                                }
                                else
                                {
                                    TempMOMActionModel.DueDate = dr["DueDate"].ToString();
                                }
                                TempMOMActionModel.CreatedByName = dr["CreatedBy"].ToString();
                                TempMOMActionModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                TempMOMActionModel.ModifiedByName = dr["ModifiedBy"].ToString();
                                TempMOMActionModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                MOMActionList.Add(TempMOMActionModel);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MOMActionList;
        }
        public PendingMOMActions GetProjectViewActionPointList(int ProjectId)
        {
            PendingMOMActions lstActions = new PendingMOMActions();
            lstActions.LstAllActionPointList = new List<PendingMOMActions>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetProjectViewPendActionPoint(ProjectId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["StatusId"].ToString() == "30" || dr["StatusId"].ToString() == "31" || dr["StatusId"].ToString() == "32")
                            {
                                PendingMOMActions TempMOMActionModel = new PendingMOMActions();
                                TempMOMActionModel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                                TempMOMActionModel.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                                TempMOMActionModel.ActionDescription = dr["ActionDescription"].ToString();
                                TempMOMActionModel.StatusShortCode = dr["Status"].ToString();
                                TempMOMActionModel.AssignedBy = dr["AssignedBy"].ToString();
                                TempMOMActionModel.AssignedTo = dr["AssignedTo"].ToString();
                                if (dr["DueDate"].ToString() != "")
                                {
                                    DateTime DD = Convert.ToDateTime(dr["DueDate"].ToString());
                                    TempMOMActionModel.DueDate = DD.ToShortDateString();
                                }
                                else
                                {
                                    TempMOMActionModel.DueDate = dr["DueDate"].ToString();
                                }
                                TempMOMActionModel.CreatedByName = dr["CreatedBy"].ToString();
                                TempMOMActionModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                TempMOMActionModel.ModifiedByName = dr["ModifiedBy"].ToString();
                                TempMOMActionModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                lstActions.LstAllActionPointList.Add(TempMOMActionModel);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstActions;
        }
        public List<TMEquipmentModel> GetProjectViewEquipmentList(int ProjectId)
        {
            List<TMEquipmentModel> EquipmentListmodel = new List<TMEquipmentModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetProjectViewEquipmentList(ProjectId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            TMEquipmentModel TempEquipModel = new TMEquipmentModel();
                            TempEquipModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            TempEquipModel.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                            TempEquipModel.ProjectName = dr["ProjectName"].ToString();
                            TempEquipModel.EquipmentName = dr["EquipmentName"].ToString();
                            TempEquipModel.ParentEquipmentName = dr["ParentEquipment"].ToString();
                            TempEquipModel.Quantity = Convert.ToDouble(dr["Quantity"]);
                            if (dr["TAM"].ToString() != "")
                                TempEquipModel.ETAM = Convert.ToInt32(dr["TAM"].ToString());
                            if (dr["Tier1TAM"].ToString() != "")
                                TempEquipModel.Tier1Tam = Convert.ToInt32(dr["Tier1TAM"].ToString());
                            if (dr["OEMTAM"].ToString() != "")
                                TempEquipModel.OemTam = Convert.ToInt32(dr["OEMTAM"].ToString());
                            EquipmentListmodel.Add(TempEquipModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EquipmentListmodel;
        }
        public List<ProjectAttachmentView> GetProjectAllAttchmentList(int ProjectId)
        {
            List<ProjectAttachmentView> AttachmentmodelList = new List<ProjectAttachmentView>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetProjectAllAttchmentList(ProjectId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjectAttachmentView AttachmentModel = new ProjectAttachmentView();
                            AttachmentModel.Type = dr["Type"].ToString();
                            AttachmentModel.Description = dr["Description"].ToString();
                            AttachmentModel.FileName = dr["FileName"].ToString();
                            AttachmentmodelList.Add(AttachmentModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AttachmentmodelList;
        }

        public List<ProjectPlanVsActualView> GetProjectPlanVsActualList(int ProjectId)
        {
            List<ProjectPlanVsActualView> PlanActualList = new List<ProjectPlanVsActualView>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetProjectPlanVsActualList(ProjectId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjectPlanVsActualView PlanActualModel = new ProjectPlanVsActualView();
                            PlanActualModel.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                            PlanActualModel.TaskName = dr["TaskName"].ToString();
                            PlanActualModel.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                            PlanActualModel.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                            if (dr["PlannedMin"].ToString() != "")
                                PlanActualModel.PlannedMin = Convert.ToInt32(dr["PlannedMin"].ToString());
                            if (dr["ActualTime"].ToString() != "")
                                PlanActualModel.ActualTime = Convert.ToInt32(dr["ActualTime"].ToString());
                            if (PlanActualModel.PlannedMin != 0)
                                PlanActualModel.ActualTimeper = PlanActualModel.ActualTime * 100 / PlanActualModel.PlannedMin;
                            PlanActualList.Add(PlanActualModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PlanActualList;
        }
        public List<ProjectResourceView> GetProjecrResourceList(int ProjectId)
        {
            List<ProjectResourceView> ResourceList = new List<ProjectResourceView>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetProjecrResourceList(ProjectId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjectResourceView ResourceModel = new ProjectResourceView();
                            ResourceModel.Id = Convert.ToInt32(dr["Id"].ToString());
                            ResourceModel.Name = dr["Name"].ToString();
                            ResourceModel.Type = dr["Type"].ToString();
                            ResourceList.Add(ResourceModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ResourceList;
        }
        public ProjectResourceDetailsViewModel GetProjecrResourceDetailList(int ProjectId, int Id, string UserType)
        {
            ProjectResourceDetailsViewModel ResourceDetailList = new ProjectResourceDetailsViewModel();
            ResourceDetailList.LstMOM = new List<ProjectResourceDetailsViewModel>();
            ResourceDetailList.LstComment = new List<MOMActionPointComment>();
            ResourceDetailList.LstActionPoint = new List<PendingMOMActions>();
            ResourceDetailList.LstTaskResource = new List<ProjectPlanVsActualView>();
            ResourceDetailList.LstTimesheet = new List<TimeSheetViewModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetProjecrResourceDetailList(ProjectId, Id, UserType);
                if (ds != null)
                {
                    if (ds.Tables.Count > 4)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjectResourceDetailsViewModel TempMOM = new ProjectResourceDetailsViewModel();
                            TempMOM.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            TempMOM.Title = dr["Title"].ToString();
                            TempMOM.MOMType = dr["MOMType"].ToString();
                            TempMOM.MOMDate = Convert.ToDateTime(dr["MOMDate"].ToString());
                            TempMOM.Name = dr["Name"].ToString();
                            TempMOM.ParticipantType = dr["ParticipantType"].ToString();
                            ResourceDetailList.LstMOM.Add(TempMOM);
                        }
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            PendingMOMActions TempActionPoint = new PendingMOMActions();
                            TempActionPoint.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                            TempActionPoint.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            TempActionPoint.ActionDescription = dr["ActionDescription"].ToString();
                            TempActionPoint.StatusShortCode = dr["Status"].ToString();
                            TempActionPoint.AssignedBy = dr["AssignedBy"].ToString();
                            ResourceDetailList.LstActionPoint.Add(TempActionPoint);
                        }
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            MOMActionPointComment TempComment = new MOMActionPointComment();
                            TempComment.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                            TempComment.CommentedBy = dr["CommentedBy"].ToString();
                            TempComment.Comment = dr["Comment"].ToString();
                            TempComment.StatusShortCode = dr["Status"].ToString();
                            TempComment.CommentDate = Convert.ToDateTime(dr["CommentDate"].ToString());
                            ResourceDetailList.LstComment.Add(TempComment);
                        }
                        foreach (DataRow dr in ds.Tables[3].Rows)
                        {
                            TimeSheetViewModel TempTimesheet = new TimeSheetViewModel();
                            TempTimesheet.TaskName = dr["TaskName"].ToString();
                            TempTimesheet.EmployeeName = dr["EmpName"].ToString();
                            TempTimesheet.DateText = dr["TMDate"].ToString();
                            TempTimesheet.FromTime = dr["FromTime"].ToString();
                            TempTimesheet.ToTime = dr["ToTime"].ToString();
                            TempTimesheet.TotalTime =Convert.ToInt32(dr["TotalTime"].ToString());
                            TempTimesheet.Remark = dr["Remark"].ToString();
                            ResourceDetailList.LstTimesheet.Add(TempTimesheet);
                        }
                        foreach (DataRow dr in ds.Tables[4].Rows)
                        {
                            ProjectPlanVsActualView TempTaskdetail = new ProjectPlanVsActualView();
                            TempTaskdetail.TaskName = dr["TaskName"].ToString();
                            TempTaskdetail.TaskType = dr["TaskType"].ToString();
                            TempTaskdetail.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                            TempTaskdetail.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                            TempTaskdetail.PlannedMin = Convert.ToInt32(dr["PlanTime"].ToString());
                            if (dr["SpendTime"].ToString() != "")
                                TempTaskdetail.ActualTime = Convert.ToInt32(dr["SpendTime"].ToString());
                            ResourceDetailList.LstTaskResource.Add(TempTaskdetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ResourceDetailList;
        }

        public List<ProjectExpensesModel> GetProjectExpenceViewList(int projectid)
        {
            List<ProjectExpensesModel> LstExpenses = new List<ProjectExpensesModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetProjectExpenceViewList(projectid);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            if (dr["StatusShortCode"].ToString() != "")
                            {
                                ProjectExpensesModel ProExpModel = new ProjectExpensesModel();
                                ProExpModel.ExpenseId = Convert.ToInt32(dr["ExpenseId"].ToString());
                                ProExpModel.ExpenseType = dr["ExpenceType"].ToString();

                                ProExpModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                                ProExpModel.Employee = dr["Employee"].ToString();
                                ProExpModel.ProjectName = dr["ProjectName"].ToString();
                                ProExpModel.TaskName = dr["TaskName"].ToString();
                                if (dr["TotalPaid"].ToString() != "")
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

        public List<PaymentExpenseStatus> GetProjectExpencePaymentViewList(int projectid)
        {
            List<PaymentExpenseStatus> Lst = new List<PaymentExpenseStatus>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetProjectExpencePaymentViewList(projectid);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PaymentExpenseStatus Model = new PaymentExpenseStatus();
                            Model.ExpenseId = Convert.ToInt32(dr["RefId"].ToString());
                            Model.Remark = dr["Remark"].ToString();
                            Model.Isdeleted = dr["Isdeleted"].ToString();
                            Model.NewAmount = Convert.ToDouble(dr["Amount"].ToString());
                            Model.Amount = Convert.ToDouble(dr["ExpenseAmount"].ToString());
                            Model.TotalPaid = Convert.ToDouble(dr["TotalPaid"].ToString());
                            Model.PaymentId = Convert.ToInt32(dr["PaymentId"].ToString());
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
        #endregion Project View 

        #region Employee VIew 

        public List<EmpChartModel> GetEmployeeViewList(string User_Id)
        {
            List<EmpChartModel> EmpModelList = new List<EmpChartModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.GetEmployeeViewList(User_Id);
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {
                                EmpChartModel TempEmpModel = new EmpChartModel();
                                TempEmpModel.id = Convert.ToInt32(dr["EmpId"]);
                                TempEmpModel.EmpName = dr["EmployeeName"].ToString();
                                TempEmpModel.Designation = dr["Designation"].ToString();
                                if (dr["UserName"].ToString() != null)
                                    TempEmpModel.UserName = dr["UserName"].ToString();
                                TempEmpModel.Manager = dr["ManagerName"].ToString();
                                TempEmpModel.Gender = dr["Gender"].ToString();
                                //if (dr["Photo"].ToString() != "")
                                //{                                   
                                //    var base64 = Convert.ToBase64String((Byte[])(dr["Photo"]));
                                //    TempEmpModel.PhotoBase = (base64);                                  
                                //}
                                //else
                                //{
                                //  if(TempEmpModel.Gender =="Male")
                                //  {
                                //      byte[] imageArray = System.IO.File.ReadAllBytes(@"H:\SyncFusionSmartSys\SmartSys\Images\MalePerson.jpg");
                                //      TempEmpModel.PhotoBase = Convert.ToBase64String(imageArray);
                                //  }
                                //  else
                                //  {
                                //      byte[] imageArray = System.IO.File.ReadAllBytes(@"H:\SyncFusionSmartSys\SmartSys\Images\FemailPerson.jpg");
                                //      TempEmpModel.PhotoBase = Convert.ToBase64String(imageArray);
                                //  }

                                //}
                                TempEmpModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                                TempEmpModel.CreatedBy = dr["CreatedBy"].ToString();
                                EmpModelList.Add(TempEmpModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return EmpModelList;
        }
        public List<EmpTimesheetList> GetEmployeeTimeSheetList(int EmpId)
        {
            List<EmpTimesheetList> TmList = new List<EmpTimesheetList>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.GetTimesheetListByEmployee(EmpId);
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {
                                EmpTimesheetList TMmodel = new EmpTimesheetList();
                                TMmodel.TimeSheetId = Convert.ToInt32(dr["TimeSheetId"]);
                                TMmodel.TimeSheetEntryId = Convert.ToInt32(dr["TimeSheetEntryId"]);
                                TMmodel.ProjectId = Convert.ToInt32(dr["ProjectId"]);
                                TMmodel.ProjectName = dr["ProjectName"].ToString();
                                TMmodel.TaskName = dr["TaskName"].ToString();
                                TMmodel.TMDay = dr["TMDay"].ToString();
                                TMmodel.Remark = dr["Remark"].ToString();
                                TMmodel.TotalTime = Convert.ToInt32(dr["TotalTime"].ToString());
                                TMmodel.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                                TMmodel.TMDate = Convert.ToDateTime(dr["TMDate"]);
                                TmList.Add(TMmodel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return TmList;
        }
        public List<TMLeaveDetailModel> GetLeavelistByEmployee(int EmpId)
        {
            List<TMLeaveDetailModel> TMLeavelst = new List<TMLeaveDetailModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetLeaveListByEmployee(EmpId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            TMLeaveDetailModel objmodel = new TMLeaveDetailModel();
                            objmodel.LeaveId = Convert.ToInt32(dr["LeaveId"].ToString());
                            objmodel.LeaveTypeName = dr["LeaveType"].ToString();
                            objmodel.Employee = dr["EmpName"].ToString();
                            ////  objmodel.LeaveCategory = dr["LeaveCategory"].ToString();
                            //  objmodel.ManagerName = dr["ManagerName"].ToString();
                            objmodel.FromDate = Convert.ToDateTime(dr["FromDate"].ToString());
                            objmodel.FromDateStr = objmodel.FromDate.ToShortDateString();
                            objmodel.ToDate = Convert.ToDateTime(dr["ToDate"].ToString());
                            objmodel.ToDateStr = objmodel.ToDate.ToShortDateString();
                            if (dr["ApprovedDate"].ToString() == "")
                            {

                            }
                            else if (dr["ApprovedDate"].ToString() == "1/1/2001 12:00:00 AM")
                            {

                            }
                            else
                            {
                                objmodel.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                                objmodel.ApprovedDatestr = objmodel.ApprovedDate.ToString();
                            }
                            objmodel.Remark = dr["Remark"].ToString();
                            objmodel.ManagerRemark = dr["ManagerRemark"].ToString();
                            objmodel.StatusShortCode = dr["Status"].ToString();
                            objmodel.CreatedByName = dr["CreatedBy"].ToString();
                            objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            objmodel.CreatedDatestr = objmodel.CreatedDate.ToShortDateString();
                            objmodel.ModifiedByName = dr["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            objmodel.ModifiedDatestr = objmodel.ModifiedDate.ToShortDateString();
                            // objmodel.LeaveLevel = dr["LeaveLevel"].ToString();
                            TMLeavelst.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TMLeavelst;
        }
        public List<PendingMOMActions> GetPendingMOMActionPointByEmployee(int EmpId)
        {
            List<PendingMOMActions> MOMActionList = new List<PendingMOMActions>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetActionPointListByEmployee(EmpId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["Status"].ToString() == "Inprogress" || dr["Status"].ToString() == "NEW")
                            {
                                PendingMOMActions TempMOMActionModel = new PendingMOMActions();
                                TempMOMActionModel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                                TempMOMActionModel.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                                TempMOMActionModel.ActionDescription = dr["ActionDescription"].ToString();
                                TempMOMActionModel.Resource = dr["Resource"].ToString();
                                TempMOMActionModel.Vendor = dr["Vendor"].ToString();
                                TempMOMActionModel.MOMTitle = dr["Title"].ToString();
                                TempMOMActionModel.Customer = dr["Customer"].ToString();
                                TempMOMActionModel.StatusShortCode = dr["Status"].ToString();
                                TempMOMActionModel.AssignedBy = dr["AssignedBy"].ToString();
                                if (dr["DueDate"].ToString() != "")
                                {
                                    DateTime DD = Convert.ToDateTime(dr["DueDate"].ToString());
                                    TempMOMActionModel.DueDate = DD.ToShortDateString();
                                }
                                else
                                {
                                    TempMOMActionModel.DueDate = dr["DueDate"].ToString();
                                }
                                TempMOMActionModel.CreatedByName = dr["CreatedBy"].ToString();
                                TempMOMActionModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                TempMOMActionModel.ModifiedByName = dr["ModifiedBy"].ToString();
                                TempMOMActionModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                MOMActionList.Add(TempMOMActionModel);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MOMActionList;
        }
        public List<PendingMOMActions> GetAllMOMActionPointByEmployee(int EmpId)
        {
            List<PendingMOMActions> MOMActionList = new List<PendingMOMActions>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetActionPointListByEmployee(EmpId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            PendingMOMActions TempMOMActionModel = new PendingMOMActions();
                            TempMOMActionModel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            TempMOMActionModel.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                            TempMOMActionModel.ActionDescription = dr["ActionDescription"].ToString();
                            TempMOMActionModel.Resource = dr["Resource"].ToString();
                            TempMOMActionModel.Assigned = dr["Assigned"].ToString();
                            TempMOMActionModel.Vendor = dr["Vendor"].ToString();
                            TempMOMActionModel.MOMTitle = dr["Title"].ToString();
                            TempMOMActionModel.Customer = dr["Customer"].ToString();
                            TempMOMActionModel.StatusShortCode = dr["Status"].ToString();
                            TempMOMActionModel.AssignedBy = dr["AssignedBy"].ToString();
                            if (dr["DueDate"].ToString() != "")
                            {
                                DateTime DD = Convert.ToDateTime(dr["DueDate"].ToString());
                                TempMOMActionModel.DueDate = DD.ToShortDateString();
                            }
                            else
                            {
                                TempMOMActionModel.DueDate = dr["DueDate"].ToString();
                            }
                            TempMOMActionModel.CreatedByName = dr["CreatedBy"].ToString();
                            TempMOMActionModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            TempMOMActionModel.StrCreatedDate = TempMOMActionModel.CreatedDate.ToShortDateString();
                            TempMOMActionModel.ModifiedByName = dr["ModifiedBy"].ToString();
                            TempMOMActionModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            TempMOMActionModel.StrModifiedDate = TempMOMActionModel.ModifiedDate.ToShortDateString();
                            MOMActionList.Add(TempMOMActionModel);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MOMActionList;
        }
        public EmpChartModel GetSelectedEmployeeViewList(int EmpId)
        {
            EmpChartModel EmpModel = new EmpChartModel();
            try
            {
                AdminDal DALObj = new AdminDal();
                DataSet dsUser = new DataSet();
                dsUser = DALObj.GetSelectedEmployeeList(EmpId);
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {
                                EmpModel.id = Convert.ToInt32(dr["EmpId"]);
                                EmpModel.EmpName = dr["FirstName"].ToString() + " " + dr["LastName"].ToString();
                                EmpModel.Gender = dr["Gender"].ToString();
                                if (dr["Photo"].ToString() != "")
                                {
                                    var base64 = Convert.ToBase64String((Byte[])(dr["Photo"]));
                                    EmpModel.PhotoBase = (base64);
                                }
                               
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return EmpModel;
        }
        public List<ProjectTaskMoM> GetSelectedProjectTaskMOM(int EmpId)
        {
            DataSet DS = new DataSet();
            List<ProjectTaskMoM> MOMList = new List<ProjectTaskMoM>();

            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DS = objDAL.GetMomListByEmployee(EmpId);
                if (DS != null)
                {
                    if (DS.Tables.Count > 0)
                    {
                        foreach (DataRow dr in DS.Tables[0].Rows)
                        {
                            ProjectTaskMoM MOModel = new ProjectTaskMoM();

                            MOModel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            MOModel.ProjectName = dr["ProjectName"].ToString();
                            MOModel.Resource = dr["Resource"].ToString();
                            MOModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            MOModel.Employee = dr["Employee"].ToString();
                            MOModel.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                            MOModel.TaskName = dr["TaskName"].ToString();
                            MOModel.Assigned = dr["Assigned"].ToString();
                            MOModel.MOMTypeKey = dr["MOMType"].ToString();
                            MOModel.Title = dr["Title"].ToString();
                            MOModel.MOMDate = Convert.ToDateTime(dr["MOMDate"].ToString());
                            MOModel.MOMDateStr = MOModel.MOMDate.ToShortDateString();
                            MOModel.Description = dr["Description"].ToString();
                            MOModel.CreatedByName = dr["CreatedBy"].ToString();
                            MOModel.ModifiedByName = dr["ModifiedBy"].ToString();
                            MOModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            MOModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            MOModel.CreatedDateStr = MOModel.CreatedDate.ToShortDateString();
                            MOModel.ModifiedDateStr = MOModel.ModifiedDate.ToShortDateString();
                            MOMList.Add(MOModel);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return MOMList;
        }
        public List<ProjectModel> GetProjectsByEmployee(int EmpId)
        {
            List<ProjectModel> lstProject = new List<ProjectModel>();
            ViewsDAL objDL = new ViewsDAL();
            DataSet dsData = objDL.GetProjectByEmployee(EmpId);
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        ProjectModel proj = new ProjectModel();
                        proj.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                        proj.ProjectName = dr["ProjectName"].ToString();
                        proj.ProjectType = dr["ProjectType"].ToString();
                        proj.Description = dr["Description"].ToString();
                        proj.VendorName = dr["VendorName"].ToString();
                        proj.CustomerName = dr["CustomerName"].ToString();
                        proj.StatusDescription = dr["StatusDescription"].ToString();
                        proj.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                        proj.StartDateStr = proj.StartDate.ToShortDateString();
                        proj.ItemPermission = dr["ItemPermission"].ToString();
                        proj.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                        proj.EndDateStr = proj.EndDate.ToShortDateString();
                        proj.ProjectManager = dr["ProjectManager"].ToString();
                        proj.Region = dr["Region"].ToString();
                        proj.Email = dr["emailId"].ToString();
                        proj.Remark = dr["Remark"].ToString();
                        proj.CompCode = dr["CompCode"].ToString();
                        proj.SegmentName = dr["SegmentName"].ToString();
                        proj.CreatedByName = dr["CreatedByName"].ToString();
                        proj.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        proj.CreatedDateStr = proj.CreatedDate.ToShortDateString();
                        proj.ModifiedByName = dr["ModifiedByName"].ToString();
                        proj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        proj.ModifiedDateStr = proj.ModifiedDate.ToShortDateString();
                        lstProject.Add(proj);
                    }
            return lstProject;
        }
        public List<CaseRiskViewModel> GetCaseRiskEbyEmployee(int EmpId)
        {
            List<CaseRiskViewModel> Model = new List<CaseRiskViewModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.GetProjTaskByEmployee(EmpId);

                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsUser.Tables[0].Rows)
                            {
                                CaseRiskViewModel demo = new CaseRiskViewModel();
                                demo.ProjectId = Convert.ToInt32(dr["ProjectId"]);
                                demo.TaskId = Convert.ToInt32(dr["TaskId"]);
                                demo.ProjectManager = dr["ProjectManager"].ToString();
                                demo.ProjectId = Convert.ToInt32(dr["ProjectId"]);
                                demo.ProjectName = dr["ProjectName"].ToString();
                                demo.Assigned = dr["TaskOwner"].ToString();
                                //demo.VendorName = dr["VendorName"].ToString();
                                demo.TaskName = dr["TaskName"].ToString();
                                demo.Description = dr["Description"].ToString();
                                demo.StartDate = Convert.ToDateTime(dr["StartDate"]);
                                demo.StartDatestr = demo.StartDate.ToShortDateString();
                                demo.EndDate = Convert.ToDateTime(dr["EndDate"]);
                                demo.EndDatestr = demo.EndDate.ToShortDateString();
                                demo.TaskTypeText = dr["TaskTypeText"].ToString();
                                demo.ReviewedByName = dr["ReviewedByName"].ToString();
                                if (dr["ReviewedDate"].ToString() != "")
                                {
                                    demo.ReviewedDate = Convert.ToDateTime(dr["ReviewedDate"]);
                                    demo.ReviewedDatestr = demo.ReviewedDate.ToShortDateString();
                                }
                                demo.ApprovedByName = dr["ApprovedByName"].ToString();
                                if (dr["ApprovedDate"].ToString() != "")
                                {
                                    demo.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"]);
                                    demo.ApprovedDatestr = demo.ApprovedDate.ToShortDateString();
                                }
                                demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                                demo.ModifiedDatestr = demo.CreatedDate.ToShortDateString();
                                demo.CreatedBy = dr["CreatedBy"].ToString();
                                demo.StatusName = dr["StatusName"].ToString();
                                demo.Resources = dr["Resources"].ToString();
                                Model.Add(demo);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Model;
        }

        public List<ProjectTravelRequestModel> TravelRequestListByEmployee(int EmpId)
        {
            List<ProjectTravelRequestModel> list = new List<ProjectTravelRequestModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetTravelRequestByEmployee(EmpId);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjectTravelRequestModel model = new ProjectTravelRequestModel();

                            model.RequestId = Convert.ToInt32(dr["RequestId"].ToString());
                            model.Description = dr["Description"].ToString();
                            model.EmployeeName = dr["EmployeeName"].ToString();
                            if (dr["ApprovedBy"].ToString() != "" && dr["ApprovedBy"].ToString() != null)
                            {
                                model.ApproverBy = dr["ApprovedBy"].ToString();
                            }
                            if (dr["ApprovedDate"].ToString() != "" && dr["ApprovedDate"].ToString() != null)
                            {
                                model.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                            }
                            if (dr["Status"].ToString() != "" && dr["Status"].ToString() != null)
                            {
                                model.Status = dr["Status"].ToString();
                            }
                            model.ApproverRemark = dr["ApproverRemark"].ToString();
                            model.CreatedBy = dr["CreatedBy"].ToString();
                            model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            model.ModifiedBy = dr["ModifiedBy"].ToString();
                            model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());


                            list.Add(model);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        public List<EnquiryModel> EnquiryListByEmployee(int EmpId, string type)
        {
            List<EnquiryModel> list = new List<EnquiryModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                if (type == "OWN")
                {
                    ds = objdal.GetOwnEnquiryByEmployee(EmpId);
                }
                else
                {
                    ds = objdal.GetEnquiryByEmployee(EmpId);
                }

                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EnquiryModel objmodel = new EnquiryModel();
                            objmodel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            objmodel.EnqNumber = dr["EnqNumber"].ToString();
                            objmodel.ItemName = dr["ItemName"].ToString();
                            objmodel.BrandName= dr["BrandName"].ToString();
                            objmodel.MPN = dr["MPN"].ToString();
                            objmodel.EnqDate = Convert.ToDateTime(dr["EnqDate"].ToString());
                            objmodel.EnqDateStr = objmodel.EnqDate.ToShortDateString();
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.ContactName = dr["ContactName"].ToString();
                            objmodel.Remark = dr["Remark"].ToString();
                            objmodel.StatusStr = dr["StatusStr"].ToString();
                            objmodel.Priority = dr["Priority"].ToString();
                            objmodel.CompCode = dr["CompCode"].ToString();
                            objmodel.ModifiedBy = dr["CreatedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            objmodel.ModifiedDateStr = objmodel.ModifiedDate.ToShortDateString();
                            if (dr["EnqType"].ToString() == "0")
                            {
                                objmodel.Types = "Manual";
                            }
                            else
                            {
                                objmodel.Types = "By Mail";
                            }
                            list.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public List<ProjectExpensesModel> TMProjectExpensesList(int EmpId)
        {
            List<ProjectExpensesModel> LstExpenses = new List<ProjectExpensesModel>();
            try
            {
                ViewsDAL objProj = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objProj.ProjectExpensesList(EmpId);
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
                            ProExpModel.ModifiedDateStr = ProExpModel.ModifiedDate.ToShortDateString();
                            ProExpModel.ExpenseDate = Convert.ToDateTime(dr["ExpenseDate"].ToString());
                            ProExpModel.ExpenseDateStr = ProExpModel.ExpenseDate.ToShortDateString();
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
        public List<EnquiryModel> EmpQuotaionList(int EmpId)
        {
            List<EnquiryModel> EmpQuotaionList = new List<EnquiryModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.EmpQuotaionList(EmpId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EnquiryModel model = new EnquiryModel();
                            model.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            model.CustomerName = dr["CustomerName"].ToString();
                            model.QuotationId = Convert.ToInt32(dr["QuotationId"].ToString());
                            model.SaleQuantity = Convert.ToInt32(dr["saleQty"].ToString());
                            model.SaleRate = Convert.ToDouble(dr["SaleRate"].ToString());
                            model.SaleAmount = Convert.ToDouble(dr["SaleAmount"].ToString());
                            model.QuotationNumber = dr["QuotationNumber"].ToString();
                            model.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            model.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            model.ItemName = dr["ItemName"].ToString();
                            model.BrandName= dr["BrandName"].ToString();
                            model.MPN = dr["MPN"].ToString();
                            model.EnquiryQuantity = Convert.ToDouble(dr["enqQty"].ToString());
                            model.Status = Convert.ToInt32(dr["Status"].ToString());
                            model.StatusStr = dr["StatusStr"].ToString();
                            model.Currancy = dr["Currancy"].ToString();
                            model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            model.CreatedDatestr = model.CreatedDate.ToShortDateString();
                            model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            model.ModifiedDateStr = model.ModifiedDate.ToShortDateString();
                            EmpQuotaionList.Add(model);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EmpQuotaionList;
        }
        public List<AccountReceivableModel> AccountReceivable(int EmpId)
        {
            List<AccountReceivableModel> ARLst = new List<AccountReceivableModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.EmpAccounReceivable(EmpId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            AccountReceivableModel model = new AccountReceivableModel();
                            model.Company = dr["CompCode"].ToString();
                            model.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            model.CustomerNo = dr["Customer_No"].ToString();
                            model.CustomerName = dr["CustomerName"].ToString();
                            model.Currency = dr["Currency"].ToString();
                            model.RemainingAmount = Convert.ToDecimal(dr["Amount"].ToString());
                            model.RemainingAmountLCY = Convert.ToDecimal(dr["Amount_LCY"].ToString());
                            ARLst.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ARLst;
        }
        public List<InvetoryModel> EmpInventoryList(int EmpId)
        {
            List<InvetoryModel> InvLst = new List<InvetoryModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.EmpInventoryList(EmpId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            InvetoryModel model = new InvetoryModel();
                            model.CompCode = dr["Company"].ToString();
                            model.CustomerName = dr["CustomerName"].ToString();
                            model.ItemName = dr["ItemName"].ToString();
                            model.MPN = dr["MPN"].ToString();
                            model.SalesOrderNo = dr["SalesOrderNo"].ToString();
                            model.CustomerOrderNo = dr["CustomerOrderNo"].ToString();
                            model.PurchaseOrderNo = dr["PurchaseOrderNo"].ToString();
                            model.SalesQuantity = Convert.ToInt32(dr["SalesQuantity"].ToString());
                            model.SalesOutstandingQty = Convert.ToInt32(dr["SalesOutstandingQty"].ToString());
                            model.TotalDispatch = Convert.ToInt32(dr["TotalDispatch"].ToString());
                            model.TotalReceive = Convert.ToInt32(dr["TotalReceive"].ToString());
                            model.PurchaseQuantity = Convert.ToInt32(dr["PurchaseQuantity"].ToString());
                            model.PurchaseOutstandingQty = Convert.ToInt32(dr["PurchaseOutstandingQty"].ToString());
                            model.TotalInventory = Convert.ToInt32(dr["TotalInventory"].ToString());
                            InvLst.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return InvLst;
        }
        public ReportNameModel GetReportOpenInEMPView(string user_Id, int EmpId)
        {
            ReportNameModel Model = new ReportNameModel();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetReportOpenInEMPView(user_Id, EmpId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Model.OutputLocation = ds.Tables[0].Rows[0]["OutputLocation"].ToString();
                            Model.RunDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["RunDate"].ToString());
                            Model.ReportId = ds.Tables[0].Rows[0]["ReportId"].ToString();
                            Model.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"].ToString());
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
        public List<EmpCalenderModel> EmpTaskListByDate(DateTime StartDate, DateTime EndDate, int EmpId)
        {
            List<EmpCalenderModel> EmpTaskList = new List<EmpCalenderModel>();
            ViewsDAL objDal = new ViewsDAL();
            DataSet ds = new DataSet();
            ds = objDal.EmpTaskListByDate(StartDate, EndDate, EmpId);
            if (ds != null)
                if (ds.Tables.Count > 0)
                    if (ds.Tables[0].Rows.Count > 0)
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (Convert.ToInt32(dr["ParentId"].ToString()) == 0)
                            {
                                EmpCalenderModel Model = new EmpCalenderModel();
                                Model.Id = Convert.ToInt32(dr["Id"].ToString());
                                Model.ParentId = Convert.ToInt32(dr["ParentId"].ToString());
                                Model.ProjectName = dr["ProjectName"].ToString();
                                Model.Day = dr["DayText"].ToString();
                                Model.Timespend = Convert.ToInt32(dr["Timespend"].ToString());
                                Model.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                                Model.ActualDate = Convert.ToDateTime(dr["ActualDate"].ToString());
                                Model.Status = dr["Status"].ToString();
                                Model.Remark = dr["Remark"].ToString();
                                Model.CustomerName = dr["CustomerName"].ToString();
                                Model.VendorName = dr["VendorName"].ToString();
                                //Model.ProjectManagerName = dr["ProjectManagerName"].ToString();
                                if (dr["Type"].ToString() != "Task"  )
                                    Model.GetChildList = GetChildData(Model.Id, Model.StartDate, ds);
                                EmpTaskList.Add(Model);
                            }

                        }
            return EmpTaskList;
        }
        public List<EmpCalenderModel> GetChildData(int id, DateTime StartDate, DataSet ds)
        {
            List<EmpCalenderModel> EmpTaskListData = new List<EmpCalenderModel>();
            if (ds != null)
                if (ds.Tables.Count > 0)
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(dr["ParentId"].ToString()) == id && Convert.ToDateTime(dr["StartDate"].ToString())== StartDate)
                        {
                            EmpCalenderModel Model = new EmpCalenderModel();
                            Model.Id = Convert.ToInt32(dr["Id"].ToString());
                            Model.ParentId = Convert.ToInt32(dr["ParentId"].ToString());
                            Model.ProjectName = dr["ProjectName"].ToString();
                            Model.Day = dr["DayText"].ToString();
                            Model.Timespend = Convert.ToInt32(dr["Timespend"].ToString());
                            Model.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                            Model.ActualDate = Convert.ToDateTime(dr["ActualDate"].ToString());
                            Model.Status = dr["Status"].ToString();
                            Model.Remark = dr["Remark"].ToString();
                            Model.CustomerName = dr["CustomerName"].ToString();
                            Model.VendorName = dr["VendorName"].ToString();
                            if (dr["Type"].ToString() != "Task")
                                Model.GetChildList = GetChildData(Model.Id, Model.StartDate, ds);
                            EmpTaskListData.Add(Model);
                        }
                    }
            return EmpTaskListData;

        }
        public List<EmpChartModel> EmpPieChartData(DateTime StartDate, DateTime EndDate, int EmpId)
        {
            List<EmpChartModel> EmpPieChartDataList = new List<EmpChartModel>();
            try
            {
                ViewsDAL objDal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDal.EmpPieChartData(StartDate, EndDate, EmpId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                EmpChartModel Model = new EmpChartModel();
                                Model.ProjectName = dr["ProjectName"].ToString();                              
                                Model.Timespend = Convert.ToInt32(dr["Timespend"].ToString());                               
                                EmpPieChartDataList.Add(Model);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EmpPieChartDataList;
        }




        #endregion Employee View 


        #region Customer View

        public List<ProjectTaskMoM> GetAllMOMActionPointByCustomer(int CustomerId)
        {
            List<ProjectTaskMoM> MOMActionList = new List<ProjectTaskMoM>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetAllMOMActionPointByCustomer(CustomerId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            ProjectTaskMoM TempMOMActionModel = new ProjectTaskMoM();
                            TempMOMActionModel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            TempMOMActionModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            TempMOMActionModel.ProjectName = dr["ProjectName"].ToString();
                            TempMOMActionModel.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                            TempMOMActionModel.TaskName = dr["TaskName"].ToString();
                            TempMOMActionModel.MOMDate = Convert.ToDateTime(dr["MOMDate"].ToString());
                            TempMOMActionModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            TempMOMActionModel.MOMTypeKey = dr["MOMType"].ToString();
                            TempMOMActionModel.Employee = dr["Employee"].ToString();
                            TempMOMActionModel.Title = dr["Title"].ToString();
                            TempMOMActionModel.Description = dr["Description"].ToString();
                            TempMOMActionModel.CreatedByName = dr["CreatedBy"].ToString();
                            TempMOMActionModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            TempMOMActionModel.ModifiedByName = dr["ModifiedBy"].ToString();
                            TempMOMActionModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            MOMActionList.Add(TempMOMActionModel);
                            //  }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MOMActionList;
        }

        public List<PendingMOMActions> GetAllAtionPointListBYCustomer(int CustomerId)
        {
            List<PendingMOMActions> MOMActionList = new List<PendingMOMActions>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetAllAtionPointListBYCustomer(CustomerId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            //if (dr["Status"].ToString() == "Complete" || dr["Status"].ToString() == "Cancelled")
                            //{
                            PendingMOMActions TempMOMActionModel = new PendingMOMActions();


                            TempMOMActionModel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            TempMOMActionModel.ActionDescription = dr["ActionDescription"].ToString();
                            TempMOMActionModel.Status = Convert.ToInt32(dr["StatusId"].ToString());
                            TempMOMActionModel.StatusDescrition = dr["StatusDescrition"].ToString();
                            TempMOMActionModel.AssignedBy = dr["AssignedBy"].ToString();
                            TempMOMActionModel.DueDate = dr["DueDate"].ToString();
                            TempMOMActionModel.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());


                            //if (dr["DueDate"].ToString() != "")
                            //{
                            //    DateTime DD = Convert.ToDateTime(dr["DueDate"].ToString());
                            //    TempMOMActionModel.DueDate = DD.ToShortDateString();
                            //}
                            //else
                            //{
                            //    TempMOMActionModel.DueDate = dr["DueDate"].ToString();
                            //}
                            TempMOMActionModel.CreatedByName = dr["CreatedBy"].ToString();
                            TempMOMActionModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            TempMOMActionModel.ModifiedByName = dr["ModifiedBy"].ToString();
                            TempMOMActionModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            MOMActionList.Add(TempMOMActionModel);
                            //  }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MOMActionList;
        }


        public List<EmployeeModel> GetAllEmplistByCustomer(int CustomerId, string User_Id)
        {
            List<EmployeeModel> CustomerEmployeeList = new List<EmployeeModel>();

            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetAllEmplistByCustomer(CustomerId, User_Id);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EmployeeModel EmpObj = new EmployeeModel();
                            EmpObj.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            EmpObj.EmpName = dr["EmpName"].ToString();
                            EmpObj.Gender = dr["Gender"].ToString();
                            EmpObj.ManagerName = dr["ManagerName"].ToString();
                            EmpObj.UserName = dr["UserName"].ToString();
                            EmpObj.emailId = dr["emailId"].ToString();
                            EmpObj.DateOfJoin = Convert.ToDateTime(dr["DateOfJoin"].ToString());
                            EmpObj.Designation = dr["Designation"].ToString();
                            EmpObj.Qualification = dr["Qualification"].ToString();
                            EmpObj.DeptName = dr["DeptName"].ToString();
                            EmpObj.ManagerId = Convert.ToInt32(dr["ManagerId"].ToString());
                            EmpObj.Deleted = Convert.ToBoolean(dr["Deleted"].ToString());
                            if (dr["LastDateOfWork"].ToString() != "")
                            {
                                EmpObj.LastDateOfWork = Convert.ToDateTime(dr["LastDateOfWork"]);
                            }
                            if (dr["Remark"].ToString() != "")
                            {
                                EmpObj.Remark = dr["Remark"].ToString();
                            }
                            EmpObj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            EmpObj.CompRelMapCount = Convert.ToInt16(dr["CompRelMapCount"].ToString());
                            //if (dsEmp.Tables[0].Rows[0]["Photo"].ToString() != "")
                            //    EmpObj.Photo = (Byte[])(dsEmp.Tables[0].Rows[0]["Photo"]);
                            CustomerEmployeeList.Add(EmpObj);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CustomerEmployeeList;
        }

        public List<EnquiryModel> EnquiryListBYCustomer(int CustomerId)
        {
            List<EnquiryModel> EnquiryModel = new List<EnquiryModel>();

            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.EnquiryListBYCustomer(CustomerId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EnquiryModel objmodel = new EnquiryModel();
                            objmodel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            objmodel.EnqNumber = dr["EnqNumber"].ToString();
                            objmodel.EnqDate = Convert.ToDateTime(dr["EnqDate"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            objmodel.StatusStr = dr["StatusStr"].ToString();
                            objmodel.Status = Convert.ToInt32(dr["Status"].ToString());
                            objmodel.Priority = dr["Priority"].ToString();
                            //  objmodel.ExpectedDate = Convert.ToDateTime(dr["ExpectedDate"].ToString());
                            objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            EnquiryModel.Add(objmodel);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EnquiryModel;
        }

        public ProjectModel ProjectModelForCustomer(int CustomerId)
        {
            ProjectModel model = new ProjectModel();
            model.Projectmodel = new List<ProjectModel>();
            model.RiskCase = new List<CaseRiskViewModel>();
            try
            {
                DataSet ds = new DataSet();
                ViewsDAL objdal = new ViewsDAL();
                ds = objdal.ProjectModelForCustomer(CustomerId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjectModel projmodel = new ProjectModel();

                            projmodel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            projmodel.ProjectName = dr["ProjectName"].ToString();
                            projmodel.ProjectType = dr["ProjectType"].ToString();
                            projmodel.Description = dr["Description"].ToString();
                            projmodel.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                            projmodel.StatusDescription = dr["StatusDescription"].ToString();
                            projmodel.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                            projmodel.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                            projmodel.ProjectManager = dr["ProjectManager"].ToString();
                            projmodel.ProjectManagerId = Convert.ToInt32(dr["ProjectManagerId"].ToString());
                            projmodel.Region = dr["Region"].ToString();

                            projmodel.Email = dr["emailId"].ToString();
                            projmodel.Remark = dr["Remark"].ToString();
                            projmodel.CreatedByName = dr["CreatedByName"].ToString();
                            projmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            projmodel.ModifiedByName = dr["ModifiedByName"].ToString();
                            projmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());

                            projmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            projmodel.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            projmodel.VendorName = dr["VendorName"].ToString();
                            projmodel.CustomerName = dr["CustomerName"].ToString();
                            if (dr["SegmentId"].ToString() != "")
                                projmodel.SegmentId = Convert.ToInt32(dr["SegmentId"].ToString());
                            projmodel.SegmentName = dr["SegmentName"].ToString();

                            projmodel.CompCode = dr["CompCode"].ToString();
                            projmodel.ItemPermission = dr["ItemPermission"].ToString();

                            model.Projectmodel.Add(projmodel);
                        }
                    }


                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {



                            CaseRiskViewModel demo = new CaseRiskViewModel();

                            demo.ProjectId = Convert.ToInt32(dr["ProjectId"]);
                            demo.Status = Convert.ToInt32(dr["Status"]);
                            demo.TaskId = Convert.ToInt32(dr["TaskId"]);
                            demo.ProjectManager = dr["ProjectManager"].ToString();
                            demo.ProjectName = dr["ProjectName"].ToString();
                            demo.CustomerName = dr["CustomerName"].ToString();
                            demo.VendorName = dr["VendorName"].ToString();
                            demo.TaskName = dr["TaskName"].ToString();
                            demo.Description = dr["Description"].ToString();
                            demo.StartDate = Convert.ToDateTime(dr["StartDate"]);
                            demo.EndDate = Convert.ToDateTime(dr["EndDate"]);
                            demo.TaskTypeText = dr["TaskTypeText"].ToString();
                            demo.ReviewedByName = dr["ReviewedByName"].ToString();
                            if (dr["ReviewedDate"].ToString() != "")
                                demo.ReviewedDate = Convert.ToDateTime(dr["ReviewedDate"]);
                            demo.ApprovedByName = dr["ApprovedByName"].ToString();
                            if (dr["ApprovedDate"].ToString() != "")
                                demo.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"]);
                            demo.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                            demo.ModifiedByName = dr["ModifiedByName"].ToString();
                            demo.StatusName = dr["StatusName"].ToString();
                            demo.Resources = dr["Resources"].ToString();

                            model.RiskCase.Add(demo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

            return model;

        }


        public List<ProjectTravelRequestModel> GetAllTravelRequestByCustomer(int CustomerId)
        {
            List<ProjectTravelRequestModel> travelRequest = new List<ProjectTravelRequestModel>();

            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.TravelRequestForCustomer(CustomerId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjectTravelRequestModel TravelRequestModel = new ProjectTravelRequestModel();
                            TravelRequestModel.RequestId = Convert.ToInt32(dr["RequestId"].ToString());
                            TravelRequestModel.Description = dr["Description"].ToString();
                            TravelRequestModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            TravelRequestModel.EmployeeName = dr["EmpName"].ToString();

                            if (dr["ApproverName"].ToString() != "" && dr["ApproverName"].ToString() != null)
                            {
                                TravelRequestModel.ApproverBy = dr["ApproverName"].ToString();
                            }
                            if (dr["ApprovedDate"].ToString() != "" && dr["ApprovedDate"].ToString() != null)
                            {
                                TravelRequestModel.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                            }
                            TravelRequestModel.Status = dr["Status"].ToString();
                            if (dr["ApproverRemark"].ToString() != "" && dr["ApproverRemark"].ToString() != null)
                            {
                                TravelRequestModel.ApproverRemark = dr["ApproverRemark"].ToString();
                            }
                            TravelRequestModel.CreatedBy = dr["CreatedBy"].ToString();
                            TravelRequestModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            if (dr["ReviewedBy"].ToString() != "" && dr["ReviewedBy"].ToString() != null)
                            {
                                TravelRequestModel.ReviewedBy = dr["ReviewedBy"].ToString();
                            }
                            if (dr["ReviewedDate"].ToString() != "" && dr["ReviewedDate"].ToString() != null)
                            {
                                TravelRequestModel.ReviewedDate = Convert.ToDateTime(dr["ReviewedDate"].ToString());
                            }
                            travelRequest.Add(TravelRequestModel);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return travelRequest;
        }


        public List<TimeSheetViewModel> TimeSheetDetailsByCustomer(int CustomerId)
        {
            List<TimeSheetViewModel> timesheetModel = new List<TimeSheetViewModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.TimeSheetDetailsForCustomer(CustomerId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        TimeSheetViewModel model = new TimeSheetViewModel();
                        model.TimeSheetId = Convert.ToInt32(dr["TimeSheetId"].ToString());
                        model.TimeSheetEntryId = Convert.ToInt32(dr["TimeSheetEntryId"].ToString());
                        model.TimeSheetEntryDetailId = Convert.ToInt32(dr["TimeSheetEntryDetailId"].ToString());
                        model.DayText = dr["DayText"].ToString();
                        model.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                        model.ProjectName = dr["ProjectName"].ToString();
                        model.TaskName = dr["TaskName"].ToString();
                        model.EmployeeName = dr["EmpName"].ToString();
                        model.ApproverName = dr["ApproverName"].ToString();
                        if (dr["ApprovedDate"].ToString() != null && dr["ApprovedDate"].ToString() != "")
                        {
                            model.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                        }
                        model.StatusShortCode = dr["StatusShortCode"].ToString();
                        model.ApproverRemark = dr["ApproverRemark"].ToString();
                        model.CreatedByName = dr["CreatedBy"].ToString();
                        model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        model.ModifiedByName = dr["ModifiedBy"].ToString();
                        model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());

                        timesheetModel.Add(model);


                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return timesheetModel;
        }
        public List<CustomerItemWiseModel> CustomerItemWise(int CustomerId)
        {
            List<CustomerItemWiseModel> ItemList = new List<CustomerItemWiseModel>();
            try
            {
                ViewsDAL Dalobj = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = Dalobj.CustomerItemWise(CustomerId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CustomerItemWiseModel model = new CustomerItemWiseModel();
                            model.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            model.CustomerName = dr["CustomerName"].ToString();
                            model.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            model.ItemId = Convert.ToInt32(dr["CustomerId"].ToString());
                            model.ItemName = dr["ItemName"].ToString();
                            model.MPN = dr["MPN"].ToString();
                            model.BrandName= dr["BrandName"].ToString();
                            model.EnqQuantity = Convert.ToDouble(dr["EnqQuantity"].ToString());
                            model.PurchaseQtyAdvent = Convert.ToDouble(dr["PurchaseQtyAdvent"].ToString());
                            model.PurchaseQtySAJ = Convert.ToDouble(dr["PurchaseQtySAJ"].ToString());
                            model.SaleQtyAdvent = Convert.ToDouble(dr["SaleQtyAdvent"].ToString());
                            model.SaleQtySAJ = Convert.ToDouble(dr["SaleQtySAJ"].ToString());
                            ItemList.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;

        }
        public List<AccountReceivableModel> CustAccounReceivable(int CustomerId)
        {
            List<AccountReceivableModel> ARLst = new List<AccountReceivableModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.CustAccounReceivable(CustomerId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            AccountReceivableModel model = new AccountReceivableModel();
                            model.Company = dr["CompCode"].ToString();
                            model.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            model.CustomerNo = dr["Customer_No"].ToString();
                            model.CustomerName = dr["CustomerName"].ToString();
                            model.Currency = dr["Currency"].ToString();
                            model.RemainingAmount = Convert.ToDecimal(dr["Amount"].ToString());
                            model.RemainingAmountLCY = Convert.ToDecimal(dr["Amount_LCY"].ToString());
                            ARLst.Add(model);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ARLst;
        }
        public List<AccountReceivableModel> CustAccounReceivableLedgerEntries(string CustomerNo, string Company)
        {
            List<AccountReceivableModel> ARLst = new List<AccountReceivableModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.CustAccounReceivableLedgerEntries(CustomerNo, Company);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            AccountReceivableModel model = new AccountReceivableModel();
                            model.Company = dr["CompCode"].ToString();
                            model.CustomerNo = dr["Customer_No"].ToString();
                            model.PostingDate = Convert.ToDateTime(dr["PostingDate"].ToString());
                            model.DocumentNo = dr["DocNo"].ToString();
                            model.Amount = Convert.ToDouble(dr["Amount"].ToString());
                            model.Amount_LCY = Convert.ToDouble(dr["Amount_LCY"].ToString());
                            model.CreditAmount = Convert.ToDouble(dr["CreditAmount"].ToString());
                            model.CreditAmountLCY = Convert.ToDouble(dr["CreditAmountLCY"].ToString());
                            model.DebitAmount = Convert.ToDouble(dr["DebitAmount"].ToString());
                            model.DebitAmountLCY = Convert.ToDouble(dr["DebitAmountLCY"].ToString());
                            ARLst.Add(model);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ARLst;
        }
        public List<CustomerItemWiseModel> CustomerItemInventory(int CustomerId)
        {
            List<CustomerItemWiseModel> ItemInventoryList = new List<CustomerItemWiseModel>();
            try
            {
                ViewsDAL Dalobj = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = Dalobj.CustomerItemInventory(CustomerId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CustomerItemWiseModel model = new CustomerItemWiseModel();
                            model.CompCode = dr["Company"].ToString();
                            model.Description = dr["Description"].ToString();
                            model.VPN = dr["VPN"].ToString();
                            model.CPN = dr["CPN"].ToString();
                            model.MPN = dr["MPN"].ToString();
                            model.BrandName = dr["BrandName"].ToString();
                            model.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            model.ItemName = dr["ItemName"].ToString();
                            model.UnitPrice = Convert.ToDouble(dr["UnitPrice"].ToString());
                            model.ItemCurQtyInHand = Convert.ToDouble(dr["ItemCurQtyInHand"].ToString());
                            model.BalanceAmount = Convert.ToDouble(dr["OutAmt"].ToString());
                            model.BalanceQuantity = Convert.ToDouble(dr["OutQuantity"].ToString());
                            model.Value = Convert.ToDouble(dr["Value"].ToString());
                           // model.CustOrdNo = dr["CustOrdNo"].ToString();
                            ItemInventoryList.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemInventoryList;
        }
        public List<SalesOrderBaklogModel> CustomerSalesOrderBacklog(int CustomerId)
        {
            List<SalesOrderBaklogModel> SalesOrderBacklogLst = new List<SalesOrderBaklogModel>();
            try
            {
                ViewsDAL Dalobj = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = Dalobj.CustomerSalesOrderBacklog(CustomerId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SalesOrderBaklogModel model = new SalesOrderBaklogModel();
                            model.CompCode = dr["CompCode"].ToString();
                            model.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            model.CustomerName = dr["CustomerName"].ToString();
                            model.DocumentNo = dr["DocumentNo"].ToString();
                            model.Description = dr["Description"].ToString();
                            model.CustOrdNo = dr["CustOrdNo"].ToString();
                            model.SalePerCode = dr["SalePerCode"].ToString();
                            model.FullName = dr["FullName"].ToString();
                            model.VPN = dr["VPN"].ToString();
                            model.Make = dr["Make"].ToString();
                            model.UnitPrice = Convert.ToDouble(dr["UnitPrice"].ToString());
                            model.Quantity = Convert.ToDouble(dr["Quantity"].ToString());
                            model.LineAmt = Convert.ToDouble(dr["LineAmt"].ToString());
                            model.OutQuantity = Convert.ToDouble(dr["OutQuantity"].ToString());
                            model.OutAmt = Convert.ToDouble(dr["OutAmt"].ToString());
                            if (dr["Cust_PO_Date"].ToString() != "")
                                model.Cust_PO_Date = Convert.ToDateTime(dr["Cust_PO_Date"].ToString());
                            if (dr["CRD"].ToString() != "")
                                model.CRD = Convert.ToDateTime(dr["CRD"].ToString());
                            if (dr["Posting_Date"].ToString() != "")
                                model.Posting_Date = Convert.ToDateTime(dr["Posting_Date"].ToString());
                            if (dr["VPD"].ToString() != "")
                                model.VPD = Convert.ToDateTime(dr["VPD"].ToString());
                            SalesOrderBacklogLst.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SalesOrderBacklogLst;

        }


        #endregion Customer View

        #region VendorView
        public List<EmployeeModel> GetAllEmplistByVendor(int VendorId, string User_Id)
        {
            List<EmployeeModel> VendorEmployeeList = new List<EmployeeModel>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetEmployeeByVendor(VendorId, User_Id);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EmployeeModel EmpObj = new EmployeeModel();
                            EmpObj.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            EmpObj.EmpName = dr["EmpName"].ToString();
                            EmpObj.Gender = dr["Gender"].ToString();
                            EmpObj.ManagerName = dr["ManagerName"].ToString();
                            //    EmpObj.UserName = dr["UserName"].ToString();
                            EmpObj.emailId = dr["emailId"].ToString();
                            EmpObj.DateOfJoin = Convert.ToDateTime(dr["DateOfJoin"].ToString());
                            EmpObj.Designation = dr["Designation"].ToString();
                            EmpObj.Qualification = dr["Qualification"].ToString();
                            EmpObj.DeptName = dr["DeptName"].ToString();
                            EmpObj.ManagerName = dr["ManagerName"].ToString();
                            EmpObj.Deleted = Convert.ToBoolean(dr["Deleted"].ToString());
                            if (dr["LastDateOfWork"].ToString() != "")
                            {
                                EmpObj.LastDateOfWork = Convert.ToDateTime(dr["LastDateOfWork"]);
                            }
                            if (dr["Remark"].ToString() != "")
                            {
                                EmpObj.Remark = dr["Remark"].ToString();
                            }
                            EmpObj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            EmpObj.CompRelMapCount = Convert.ToInt16(dr["CompRelMapCount"].ToString());
                            VendorEmployeeList.Add(EmpObj);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return VendorEmployeeList;
        }
        public List<ProjectTaskMoM> GetSelectedProjectTaskMOMBYvendor(int VendorId)
        {
            DataSet DS = new DataSet();
            List<ProjectTaskMoM> MOMList = new List<ProjectTaskMoM>();

            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DS = objDAL.GetMoMByVendor(VendorId);
                if (DS != null)
                {
                    if (DS.Tables.Count > 0)
                    {
                        foreach (DataRow dr in DS.Tables[0].Rows)
                        {
                            ProjectTaskMoM MOModel = new ProjectTaskMoM();
                            MOModel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            MOModel.ProjectName = dr["ProjectName"].ToString();
                            MOModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            MOModel.Employee = dr["Employee"].ToString();
                            MOModel.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                            MOModel.TaskName = dr["TaskName"].ToString();
                            MOModel.MOMTypeKey = dr["MOMType"].ToString();
                            MOModel.Title = dr["Title"].ToString();
                            MOModel.MOMDate = Convert.ToDateTime(dr["MOMDate"].ToString());
                            MOModel.Description = dr["Description"].ToString();
                            MOModel.CreatedByName = dr["CreatedBy"].ToString();
                            MOModel.ModifiedByName = dr["ModifiedBy"].ToString();
                            MOModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            MOModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            MOMList.Add(MOModel);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return MOMList;
        }
        public List<PendingMOMActions> GetAllMOMActionPointByVendor(int VendorId)
        {
            List<PendingMOMActions> MOMActionList = new List<PendingMOMActions>();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetMOMActionPointListByVendor(VendorId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PendingMOMActions TempMOMActionModel = new PendingMOMActions();
                            TempMOMActionModel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                            TempMOMActionModel.ActionPointId = Convert.ToInt32(dr["ActionPointId"].ToString());
                            TempMOMActionModel.ActionDescription = dr["ActionDescription"].ToString();
                            //TempMOMActionModel.Resource = dr["Resource"].ToString();
                            // TempMOMActionModel.Vendor = dr["Vendor"].ToString();
                            //   TempMOMActionModel.MOMTitle = dr["Title"].ToString();
                            //   TempMOMActionModel.Customer = dr["Customer"].ToString();
                            TempMOMActionModel.StatusShortCode = dr["StatusDescrition"].ToString();
                            TempMOMActionModel.AssignedBy = dr["AssignedBy"].ToString();
                            if (dr["DueDate"].ToString() != "")
                            {
                                DateTime DD = Convert.ToDateTime(dr["DueDate"].ToString());
                                TempMOMActionModel.DueDate = DD.ToShortDateString();
                            }
                            else
                            {
                                TempMOMActionModel.DueDate = dr["DueDate"].ToString();
                            }
                            TempMOMActionModel.CreatedByName = dr["CreatedBy"].ToString();
                            TempMOMActionModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            TempMOMActionModel.ModifiedByName = dr["ModifiedBy"].ToString();
                            TempMOMActionModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            MOMActionList.Add(TempMOMActionModel);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MOMActionList;
        }



        public ProjectModel ProjectModelForVendor(int VendorId)
        {
            ProjectModel model = new ProjectModel();
            model.Projectmodel = new List<ProjectModel>();
            model.RiskCase = new List<CaseRiskViewModel>();
            try
            {
                DataSet ds = new DataSet();
                ViewsDAL objdal = new ViewsDAL();
                ds = objdal.ProjectModelForVendor(VendorId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjectModel projmodel = new ProjectModel();

                            projmodel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                            projmodel.ProjectName = dr["ProjectName"].ToString();
                            // projmodel.ProjectType = dr["ProjectType"].ToString();
                            projmodel.Description = dr["Description"].ToString();
                            projmodel.StatusDescription = dr["StatusDescription"].ToString();
                            if (dr["StartDate"].ToString() != "" && dr["StartDate"].ToString() != null)
                                projmodel.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                            if (dr["EndDate"].ToString() != "" && dr["EndDate"].ToString() != null)
                                projmodel.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                            projmodel.ProjectManager = dr["ProjectManager"].ToString();
                            projmodel.ProjectManagerId = Convert.ToInt32(dr["ProjectManagerId"].ToString());
                            projmodel.Region = dr["Region"].ToString();
                            projmodel.Email = dr["emailId"].ToString();
                            projmodel.Remark = dr["Remark"].ToString();
                            projmodel.CreatedByName = dr["CreatedByName"].ToString();
                            projmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            projmodel.ModifiedByName = dr["ModifiedByName"].ToString();
                            projmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            projmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            projmodel.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            projmodel.VendorName = dr["VendorName"].ToString();
                            projmodel.CustomerName = dr["CustomerName"].ToString();
                            if (dr["SegmentId"].ToString() != "" && dr["SegmentId"].ToString() != null)
                                projmodel.SegmentId = Convert.ToInt32(dr["SegmentId"].ToString());
                            projmodel.SegmentName = dr["SegmentName"].ToString();
                            if (dr["CompCode"].ToString() != "" && dr["CompCode"].ToString() != null)
                                projmodel.CompCode = dr["CompCode"].ToString();
                            projmodel.ItemPermission = dr["ItemPermission"].ToString();

                            model.Projectmodel.Add(projmodel);
                        }
                    }


                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            CaseRiskViewModel demo = new CaseRiskViewModel();

                            demo.ProjectId = Convert.ToInt32(dr["ProjectId"]);
                            demo.Status = Convert.ToInt32(dr["Status"]);
                            demo.TaskId = Convert.ToInt32(dr["TaskId"]);
                            demo.ProjectManager = dr["ProjectManager"].ToString();
                            demo.ProjectName = dr["ProjectName"].ToString();
                            demo.CustomerName = dr["CustomerName"].ToString();
                            demo.VendorName = dr["VendorName"].ToString();
                            demo.TaskName = dr["TaskName"].ToString();
                            demo.Description = dr["Description"].ToString();
                            demo.StartDate = Convert.ToDateTime(dr["StartDate"]);
                            demo.EndDate = Convert.ToDateTime(dr["EndDate"]);
                            demo.TaskTypeText = dr["TaskTypeText"].ToString();
                            demo.ReviewedByName = dr["ReviewedByName"].ToString();
                            if (dr["ReviewedDate"].ToString() != "")
                                demo.ReviewedDate = Convert.ToDateTime(dr["ReviewedDate"]);
                            demo.ApprovedByName = dr["ApprovedByName"].ToString();
                            if (dr["ApprovedDate"].ToString() != "")
                                demo.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"]);
                            demo.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                            demo.ModifiedByName = dr["ModifiedByName"].ToString();
                            demo.StatusName = dr["StatusName"].ToString();
                            demo.Resources = dr["Resources"].ToString();

                            model.RiskCase.Add(demo);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }

            return model;

        }



        public List<ProjectTravelRequestModel> GetAllTravelRequestByVendor(int VendorId)
        {
            List<ProjectTravelRequestModel> travelRequest = new List<ProjectTravelRequestModel>();

            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.TravelRequestForVendor(VendorId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ProjectTravelRequestModel TravelRequestModel = new ProjectTravelRequestModel();
                            TravelRequestModel.RequestId = Convert.ToInt32(dr["RequestId"].ToString());
                            TravelRequestModel.Description = dr["Description"].ToString();
                            TravelRequestModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            TravelRequestModel.EmployeeName = dr["EmpName"].ToString();
                            if (dr["ApproverId"].ToString() != "" && dr["ApproverId"].ToString() != null)
                            {
                                TravelRequestModel.ApproverId = Convert.ToInt32(dr["ApproverId"].ToString());
                            }



                            TravelRequestModel.ApproverBy = dr["ApproverName"].ToString();
                            if (dr["ApprovedDate"].ToString() != "" && dr["ApprovedDate"].ToString() != null)
                            {
                                TravelRequestModel.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                            }
                            TravelRequestModel.Status = dr["StatusShortCode"].ToString();


                            TravelRequestModel.ApproverRemark = dr["ApproverRemark"].ToString();

                            TravelRequestModel.CreatedBy = dr["CreatedBy"].ToString();

                            TravelRequestModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());

                            TravelRequestModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            TravelRequestModel.ModifiedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());

                            travelRequest.Add(TravelRequestModel);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return travelRequest;
        }


        public List<TimeSheetViewModel> TimeSheetDetailsByVendor(int VendorId)
        {
            List<TimeSheetViewModel> timesheetModel = new List<TimeSheetViewModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.TimeSheetDetailsForVendor(VendorId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        TimeSheetViewModel model = new TimeSheetViewModel();
                        model.TimeSheetId = Convert.ToInt32(dr["TimeSheetId"].ToString());
                        model.TimeSheetEntryId = Convert.ToInt32(dr["TimeSheetEntryId"].ToString());
                        model.TimeSheetEntryDetailId = Convert.ToInt32(dr["TimeSheetEntryDetailId"].ToString());
                        model.DayText = dr["DayText"].ToString();
                        model.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                        model.ProjectName = dr["ProjectName"].ToString();
                        model.TaskName = dr["TaskName"].ToString();
                        model.EmployeeName = dr["EmpName"].ToString();
                        model.ApproverName = dr["ApproverName"].ToString();
                        if (dr["ApprovedDate"].ToString() != null && dr["ApprovedDate"].ToString() != "")
                        {
                            model.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                        }
                        model.StatusShortCode = dr["Description"].ToString();
                        model.ApproverRemark = dr["ApproverRemark"].ToString();
                        model.CreatedByName = dr["CreatedBy"].ToString();
                        model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        model.ModifiedByName = dr["ModifiedBy"].ToString();
                        model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());

                        timesheetModel.Add(model);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return timesheetModel;
        }
        #endregion VendorView
        public List<EnquiryModel> EnquiryListByVendor(int VendorId)
        {
            List<EnquiryModel> list = new List<EnquiryModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.EnquiryListByVendor(VendorId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EnquiryModel objmodel = new EnquiryModel();
                            objmodel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            objmodel.EnqNumber = dr["EnqNumber"].ToString();
                            objmodel.EnqDate = Convert.ToDateTime(dr["EnqDate"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.ContactName = dr["ContactName"].ToString();
                            objmodel.Remark = dr["Remark"].ToString();
                            objmodel.StatusStr = dr["StatusStr"].ToString();
                            objmodel.Priority = dr["Priority"].ToString();
                            objmodel.CompCode = dr["CompCode"].ToString();
                            objmodel.ModifiedBy = dr["CreatedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            if (dr["EnqType"].ToString() == "0")
                            {
                                objmodel.Types = "Manual";
                            }
                            else
                            {
                                objmodel.Types = "By Mail";
                            }
                            list.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }
        public TimeSheetViewModel TimeSheetDetails(int TimeSheetId)
        {
            TimeSheetViewModel objModel = new TimeSheetViewModel();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.TimeSheetDetails(TimeSheetId);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        objModel.TimeSheetId = Convert.ToInt32(dr["TimeSheetId"].ToString());
                        objModel.EmployeeName = dr["EmpName"].ToString();
                        objModel.WeekNo = Convert.ToInt32(dr["WeekNo"].ToString());
                        objModel.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                        objModel.StatusShortCode = dr["Description"].ToString();
                        objModel.ApproverName = dr["ApproverName"].ToString();
                        if (dr["ApprovedDate"].ToString() != null && dr["ApprovedDate"].ToString() != "")
                        {
                            objModel.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                        }
                        objModel.ApproverRemark = dr["ApproverRemark"].ToString();
                        objModel.CreatedByName = dr["CreatedByName"].ToString();
                        objModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        objModel.ModifiedByName = dr["ModifiedByName"].ToString();
                        objModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        objModel.ProjectName = dr["ProjectName"].ToString();
                        //  objModel.TaskName = dr["TaskName"].ToString();
                    }

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objModel;
        }
        public GetTaskdurationByStatusModel GetTaskdurationByStatus(int Projectid, int TaskId)
        {
            GetTaskdurationByStatusModel model = new GetTaskdurationByStatusModel();
            model.LstStatus = new List<GetTaskdurationByStatusModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetTaskdurationByStatus(Projectid, TaskId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        model.TaskName = dr["TaskName"].ToString();
                        if (dr["ReviewedDate"].ToString() != "")
                            model.ReviewedDate = Convert.ToDateTime(dr["ReviewedDate"].ToString());
                        if (dr["RVWTime"].ToString() != "")
                            model.ReviewedEndTime = Convert.ToInt32(dr["RVWTime"].ToString());



                        if (dr["ApprovedDate"].ToString() != "")
                            model.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                        if (dr["AprTime"].ToString() != "")
                            model.ApprovedTime = Convert.ToInt32(dr["AprTime"].ToString());
                        if (dr["InprogressDate"].ToString() != "")
                            model.InprogressDate = Convert.ToDateTime(dr["InprogressDate"].ToString());
                        if (dr["InpTime"].ToString() != "")
                            model.InprogressTime = Convert.ToInt32(dr["InpTime"].ToString());
                        if (dr["CompleteDate"].ToString() != "")
                            model.CompleteDate = Convert.ToDateTime(dr["CompleteDate"].ToString());
                        if (dr["CmpltTime"].ToString() != "")
                            model.CompleteTime = Convert.ToInt32(dr["CmpltTime"].ToString());
                        model.LstStatus.Add(model);
                        int Total = model.ReviewedTime + model.ApprovedTime + model.InprogressTime + model.CompleteTime;
                        if (model.ReviewedTime > 0)
                            model.ReviewedTime = (int)(0.5f + (100f * model.ReviewedTime) / Total);
                        int ApproveEndTime = (int)(0.5f + (100f * model.ApprovedTime) / Total);

                        int InprogressEndTime = (int)(0.5f + (100f * model.InprogressTime) / Total);
                        int CompleteEndTime = (int)(0.5f + (100f * model.CompleteTime) / Total);
                        model.ApproveEndTime = model.ReviewedTime + ApproveEndTime;
                        model.InprogressEndTime = model.ApproveEndTime + InprogressEndTime;
                        model.CompleteEndTime = model.InprogressEndTime + CompleteEndTime;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
        public List<SysRoleTasks> GetSupermaticAccessList(int EmpId)
        {
            SmartSys.BL.SysRolesTaskModel sysRolesTaskModel = new SysRolesTaskModel();
            List<SysRoleTasks> roletask = new List<SysRoleTasks>();
            List<SmartSys.BL.SysRoleTaskModel> lstsysRoleTasks = new List<SysRoleTaskModel>();
            sysRolesTaskModel.lstSysRolesTasks = new List<SysRoleTasks>();
            ViewsDAL objdal = new ViewsDAL();
            DataSet dsRoleTask = objdal.GetEmployeeSupermaticAcessList(EmpId);
            if (dsRoleTask.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsRoleTask.Tables[0].Rows)
                {
                    SysRoleTasks sysRoleTasks = new SysRoleTasks();
                    //SmartSys.BL.SysRoleTaskModel task = new SysRoleTaskModel();

                    sysRoleTasks.id = Convert.ToInt32(dr["TaskId"].ToString());
                    sysRoleTasks.pid = Convert.ToInt32(dr["ParentTaskId"].ToString());
                    sysRoleTasks.name = dr["TaskName"].ToString();
                    //  sysRoleTasks.RoleID = dr["RoleID"].ToString();

                    if (Convert.ToInt32(dr["hasChild"].ToString()) == 0)
                        sysRoleTasks.HasChild = false;
                    else
                        sysRoleTasks.HasChild = true;
                    roletask.Add(sysRoleTasks);
                }
            }
            return roletask;
        }
        #region ItemView
        public List<ProjectModel> GetProjectListForItemView(int ItemId)
        {
            List<ProjectModel> lstProject = new List<ProjectModel>();
            ViewsDAL objDL = new ViewsDAL();
            DataSet dsData = objDL.GetProjectListForItemVw(ItemId);
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
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
                        // proj.SegmentName = dr["SegmentName"].ToString();
                        proj.ProjectManagerId = Convert.ToInt32(dr["ProjectManagerId"].ToString());
                        proj.CreatedByName = dr["CreatedByName"].ToString();
                        proj.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        proj.ModifiedByName = dr["ModifiedByName"].ToString();
                        proj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        lstProject.Add(proj);
                    }
            return lstProject;
        }
        public List<DispatchModel> GetDispatchListByItem(int ItemId)
        {
            List<DispatchModel> LstDispatchModel = new List<DispatchModel>();
            try
            {
                ViewsDAL ObjDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetDispatchListByItem(ItemId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DispatchModel tempDispatchModel = new DispatchModel();
                            tempDispatchModel.DispatchId = Convert.ToInt32(dr["DispatchId"]);
                            tempDispatchModel.CompanyName = dr["CompanyName"].ToString();
                            tempDispatchModel.CustomerName = dr["CustomerName"].ToString();
                            tempDispatchModel.FreightForwarderName = dr["FreightForwarderName"].ToString();
                            if (dr["DispatchDate"].ToString() != "")
                                tempDispatchModel.DispatchDate = Convert.ToDateTime(dr["DispatchDate"].ToString());
                            tempDispatchModel.StatusId = Convert.ToInt32(dr["StatusId"]);
                            tempDispatchModel.Status = dr["StatusShortCode"].ToString();
                            tempDispatchModel.StatusDesc = dr["StatusDesc"].ToString();
                            tempDispatchModel.AirwayBillNo = dr["AirwayBillNo"].ToString();
                            tempDispatchModel.Invoice_No = dr["Invoice_No"].ToString();
                            tempDispatchModel.ExportPermitNo = dr["ExportPermitNo"].ToString();
                            tempDispatchModel.FreightForwarderName = dr["FreightForwarderName"].ToString();
                            tempDispatchModel.CreatedBy = dr["CreatedBy"].ToString();
                            tempDispatchModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            tempDispatchModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            tempDispatchModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            LstDispatchModel.Add(tempDispatchModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstDispatchModel;
        }
        public List<EnquiryModel> getEnquiryListByItem(int ItemId)
        {
            EnquiryModel EnquiryModel = new EnquiryModel();
            EnquiryModel.lstEnquiry = new List<EnquiryModel>();
            try
            {
                ViewsDAL ObjDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetCustEnquiryListByItem(ItemId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            EnquiryModel objmodel = new EnquiryModel();
                            objmodel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            objmodel.EnqNumber = dr["EnqNumber"].ToString();
                            objmodel.EnqDate = Convert.ToDateTime(dr["EnqDate"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            objmodel.StatusStr = dr["StatusStr"].ToString();
                            objmodel.Status = Convert.ToInt32(dr["Status"].ToString());
                            objmodel.Priority = dr["Priority"].ToString();
                            objmodel.CustomerContactId = Convert.ToInt32(dr["CustomerContactId"].ToString());
                            objmodel.ContactName = dr["ContactName"].ToString();
                            objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            EnquiryModel.lstEnquiry.Add(objmodel);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return EnquiryModel.lstEnquiry;
        }

        public List<TMEquipmentModel> GetTMEquipmentListByItem(int ItemId)
        {
            List<TMEquipmentModel> lstEquipments = new List<TMEquipmentModel>();
            ViewsDAL objDal = new ViewsDAL();
            DataSet dsData = objDal.GetTMEquipmentListByItem(ItemId);
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        TMEquipmentModel tmObj = new TMEquipmentModel();
                        tmObj.Id = Convert.ToInt32(dr["Id"].ToString());
                        tmObj.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                        tmObj.EquipmentName = dr["EquipmentName"].ToString();
                        tmObj.Description = dr["Description"].ToString();
                        tmObj.TAM = Convert.ToDouble(dr["TAM"].ToString());
                        tmObj.CurrencyCodes = dr["CurrencyCode"].ToString();
                        tmObj.Rate = Convert.ToDouble(dr["Rate"].ToString());
                        tmObj.ParentEquipmentId = Convert.ToInt32(dr["ParentEquipmentId"].ToString());
                        tmObj.SegmentId = Convert.ToInt32(dr["SegmentId"].ToString());
                        tmObj.ModifiedByName = dr["ModifiedByName"].ToString();
                        tmObj.ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString());
                        tmObj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        lstEquipments.Add(tmObj);
                    }
            return lstEquipments;
        }

        public List<CustomerPOModel> getCustPOListByItem(int ItemId)
        {
            CustomerPOModel POmodelModel = new CustomerPOModel();
            POmodelModel.POlist = new List<CustomerPOModel>();
            try
            {
                ViewsDAL ObjDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetCustomerPOListByItem(ItemId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            CustomerPOModel objmodel = new CustomerPOModel();
                            objmodel.PurchaseOrderId = Convert.ToInt32(dr["PurchaseOrderId"].ToString());
                            objmodel.PurchaseOrderNumber = dr["PurchaseOrderNumber"].ToString();
                            objmodel.PODate = Convert.ToDateTime(dr["PODate"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            objmodel.StatusStr = dr["StatusStr"].ToString();
                            objmodel.Status = Convert.ToInt32(dr["Status"].ToString());
                            //  objmodel.ExpectedDate = Convert.ToDateTime(dr["ExpectedDate"].ToString());
                            objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            POmodelModel.POlist.Add(objmodel);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return POmodelModel.POlist;
        }

        public List<QuotationModel> GetQuotaionListByItem(int Item_Id)
        {
            List<QuotationModel> QuotList = new List<QuotationModel>();
            ViewsDAL objdal = new ViewsDAL();
            DataSet ds = new DataSet();
            ds = objdal.GetQuotationListByItem(Item_Id);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            QuotationModel demomodel = new QuotationModel();
                            demomodel.QuotationId = Convert.ToInt32(dr["QuotationId"].ToString());
                            demomodel.QuotationNumber = dr["QuotationNumber"].ToString();
                            demomodel.QuotDate = Convert.ToDateTime(dr["QuotDate"].ToString());
                            demomodel.Terms = dr["Terms"].ToString();
                            demomodel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            demomodel.CustomerName = dr["CustomerName"].ToString();
                            demomodel.EnqNumber = dr["EnqNumber"].ToString();
                            demomodel.CompCode = dr["CompCode"].ToString();
                            demomodel.Status = dr["Status"].ToString();
                            demomodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            demomodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            QuotList.Add(demomodel);
                        }
                    }

                }
            }
            return QuotList;
        }

        public List<EnquiryItemVendorResponseDetailModel> VendorResponseByItem(int ItemId)
        {

            EnquiryItemVendorResponseDetailModel VendRespmodel = new EnquiryItemVendorResponseDetailModel();
            VendRespmodel.listEnqItemVendRespDetail = new List<EnquiryItemVendorResponseDetailModel>();
            try
            {
                DataSet ds = new DataSet();
                ViewsDAL objdal = new ViewsDAL();
                ds = objdal.VendorResponseByItem(ItemId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                EnquiryItemVendorResponseDetailModel Tempmode = new EnquiryItemVendorResponseDetailModel();
                                Tempmode.ResponseId = Convert.ToInt32(dr["ResponseId"].ToString());
                                Tempmode.VendorName = dr["VendorName"].ToString();
                                Tempmode.ContactName = dr["ContactName"].ToString();
                                Tempmode.Currency = dr["Currency"].ToString();
                                Tempmode.ReciptMethod = dr["ReciptMethod"].ToString();
                                Tempmode.Remark = dr["Remark"].ToString();
                                Tempmode.ReciptDate = Convert.ToDateTime(dr["ReciptDate"].ToString());
                                Tempmode.CreatedBy = dr["CreatedBy"].ToString();
                                Tempmode.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                Tempmode.ModifiedBy = dr["ModifiedBy"].ToString();
                                Tempmode.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                VendRespmodel.listEnqItemVendRespDetail.Add(Tempmode);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return VendRespmodel.listEnqItemVendRespDetail;
        }
        #endregion ItemView

        #region Equipment Detail
        public List<ItemSegment> GetSegmentListForView()
        {
            List<ItemSegment> lstSegment = new List<ItemSegment>();
            ViewsDAL objdal = new ViewsDAL();
            DataSet dsData = objdal.GetSegmentListForView();
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(dr["ParentSegmentId"].ToString()) == 0)
                        {
                            ItemSegment tmObj = new ItemSegment();
                            tmObj.SegmentId = Convert.ToInt32(dr["SegmentId"].ToString());
                            tmObj.SegmentName = dr["SegmentName"].ToString();
                            tmObj.Description = dr["Description"].ToString();
                            tmObj.ParentSegmentId = Convert.ToInt32(dr["ParentSegmentId"].ToString());
                            tmObj.CreatedBy = dr["CreatedBy"].ToString();
                            tmObj.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            tmObj.ChildSegList = GetChildSegment(tmObj.SegmentId, dsData);
                            lstSegment.Add(tmObj);
                        }
                    }
            return lstSegment;
        }
        public List<ItemSegment> GetChildSegment(int Id, DataSet dsData)
        {
            List<ItemSegment> ChildlstSegment = new List<ItemSegment>();
            foreach (DataRow dr in dsData.Tables[0].Rows)
            {
                if (Convert.ToInt32(dr["ParentSegmentId"].ToString()) == Id)
                {
                    ItemSegment tmObj = new ItemSegment();
                    tmObj.SegmentId = Convert.ToInt32(dr["SegmentId"].ToString());
                    tmObj.SegmentName = dr["SegmentName"].ToString();
                    tmObj.Description = dr["Description"].ToString();
                    tmObj.ParentSegmentId = Convert.ToInt32(dr["ParentSegmentId"].ToString());
                    tmObj.CreatedBy = dr["CreatedBy"].ToString();
                    tmObj.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                    tmObj.ChildSegList = GetChildSegment(tmObj.SegmentId, dsData);
                    ChildlstSegment.Add(tmObj);
                }
            }
            return ChildlstSegment;
        }

        public List<TMEquipmentModel> GetEquipmentListBySegment(int SegmentId, string User_Id)
        {
            List<TMEquipmentModel> lstSegment = new List<TMEquipmentModel>();
            ViewsDAL objdal = new ViewsDAL();
            DataSet dsData = new DataSet();
            if (SegmentId > 0)
            {
                dsData = objdal.GetEquipmentListbySegment(SegmentId, User_Id);
            }
            else
            {
                dsData = objdal.GetEquipmentListbySalesPerson(User_Id);
            }
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(dr["ParentEquipmentId"].ToString()) == 0)
                        {
                            TMEquipmentModel tmObj = new TMEquipmentModel();
                            tmObj.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                            tmObj.EquipmentName = dr["EquipmentName"].ToString();
                            tmObj.Description = dr["Typ"].ToString();
                            tmObj.ParentEquipmentId = Convert.ToInt32(dr["ParentEquipmentId"].ToString());
                            tmObj.TAM = Convert.ToDouble(dr["TAM"].ToString());
                            tmObj.Rate = Convert.ToDouble(dr["Rate"].ToString());
                            tmObj.AdventCost = Convert.ToDouble(dr["AdventCOST"].ToString());
                            tmObj.CustomerPotential = Convert.ToDouble(dr["CustomerPotential"].ToString());
                            tmObj.AdventPotential = Convert.ToDouble(dr["AdventPotential"].ToString());
                            tmObj.CurrencyCodes = dr["Currency"].ToString();
                            if (tmObj.EquipmentName != "#Item")
                                tmObj.lstEquipment = ChildListEquip(tmObj.EquipmentId, dsData);
                            lstSegment.Add(tmObj);
                        }
                    }
            return lstSegment;
        }
        public List<TMEquipmentModel> ChildListEquip(int id, DataSet dsData)
        {
            List<TMEquipmentModel> ChildlstSegment = new List<TMEquipmentModel>();
            foreach (DataRow dr in dsData.Tables[0].Rows)
            {
                if (Convert.ToInt32(dr["ParentEquipmentId"].ToString()) == id)
                {
                    TMEquipmentModel tmObj = new TMEquipmentModel();
                    tmObj.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                    tmObj.EquipmentName = dr["EquipmentName"].ToString();
                    tmObj.Description = dr["Typ"].ToString();
                    tmObj.ParentEquipmentId = Convert.ToInt32(dr["ParentEquipmentId"].ToString());
                    tmObj.TAM = Convert.ToDouble(dr["TAM"].ToString());
                    tmObj.Rate = Convert.ToDouble(dr["Rate"].ToString());
                    tmObj.AdventCost = Convert.ToDouble(dr["AdventCOST"].ToString());
                    tmObj.CustomerPotential = Convert.ToDouble(dr["CustomerPotential"].ToString());
                    tmObj.AdventPotential = Convert.ToDouble(dr["AdventPotential"].ToString());
                    tmObj.CurrencyCodes = dr["Currency"].ToString();
                    if (tmObj.EquipmentName != "#Item")
                        tmObj.lstEquipment = ChildListEquip(tmObj.EquipmentId, dsData);
                    ChildlstSegment.Add(tmObj);
                }
            }
            return ChildlstSegment;
        }

        #endregion Equipment Detail

        public ReportNameModel GetOpenReport(string user_Id, int ProjectId)
        {
            ReportNameModel Model = new ReportNameModel();
            try
            {
                ViewsDAL objDAL = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetReportOpenForProjectView(user_Id, ProjectId);
                Model.OutputLocation = ds.Tables[0].Rows[0]["OutputLocation"].ToString();
                Model.RunDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["RunDate"].ToString());
                Model.ReportId = ds.Tables[0].Rows[0]["ReportId"].ToString();
                Model.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Model;
        }
        #region BrandView
        public List<BrandListViewModel> BrandList()
        {
            List<BrandListViewModel> BrandLst = new List<BrandListViewModel>();
            try
            {
                ViewsDAL ObjDal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = ObjDal.BrandViewList();
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BrandListViewModel objmodel = new BrandListViewModel();
                            objmodel.BrandId = Convert.ToInt32(dr["BrandId"].ToString());
                            objmodel.BrandName = dr["BrandName"].ToString();
                            objmodel.CreatedByEmp = dr["CreatedByEmp"].ToString();
                            objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            BrandLst.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return BrandLst;
        }

        public BrandListViewModel VendorBrand(int BrandId)
        {
            BrandListViewModel Model = new BrandListViewModel();
            Model.VendorList = new List<BrandListViewModel>();
            ViewsDAL ObjDal = new ViewsDAL();
            DataSet ds = new DataSet();
            ds = ObjDal.GetVendorListByBrand(BrandId);
            if (ds == null)
                return null;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BrandListViewModel objmodel = new BrandListViewModel();
                        objmodel.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                        objmodel.VendorName = dr["VendorName"].ToString();
                        objmodel.BrandName = dr["VendorName"].ToString();
                        Model.VendorList.Add(objmodel);
                    }
                }
                Model.BrandName = ds.Tables[0].Rows[0]["BrandName"].ToString();
            }
            return Model;
        }
        public List<BrandListViewModel> ItemListByBrand(int BrandId)
        {
            List<BrandListViewModel> BrandLstByVend = new List<BrandListViewModel>();
            try
            {
                ViewsDAL ObjDal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = ObjDal.GetItemListByBrand(BrandId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BrandListViewModel objmodel = new BrandListViewModel();
                            objmodel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            objmodel.ItemName = dr["ItemName"].ToString();
                            objmodel.MPN = dr["MPN"].ToString();
                            // objmodel.CreatedByEmp = dr["CreatedByEmp"].ToString();
                            //objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            BrandLstByVend.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return BrandLstByVend;
        }
        public List<BrandListViewModel> EnquiryListByBrand(int BrandId)
        {
            List<BrandListViewModel> BrandLstByEnq = new List<BrandListViewModel>();
            try
            {
                ViewsDAL ObjDal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = ObjDal.GetEnquiryListByBrand(BrandId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BrandListViewModel objmodel = new BrandListViewModel();
                            objmodel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            objmodel.EnqNumber = dr["EnqNumber"].ToString();
                            objmodel.ItemName = dr["ItemName"].ToString();
                            objmodel.MPN = dr["MPN"].ToString();
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.Quantity = Convert.ToInt32(dr["Quantity"].ToString());
                            // objmodel.CreatedByEmp = dr["CreatedByEmp"].ToString();
                            //objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            BrandLstByEnq.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return BrandLstByEnq;
        }
        public List<BrandListViewModel> GetCustomerListByBrand(int BrandId)
        {
            List<BrandListViewModel> BrandLstByCust = new List<BrandListViewModel>();
            try
            {
                ViewsDAL ObjDal = new ViewsDAL();
                DataSet ds = new DataSet();
                ds = ObjDal.GetCustomerListByBrand(BrandId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BrandListViewModel objmodel = new BrandListViewModel();
                            objmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();

                            BrandLstByCust.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return BrandLstByCust;
        }

        #endregion BrandView

        #region Backlogs
        public List<BacklogsModel> GetPurchaseBackLogsDetail(string Type,int Id)
        {
            List<BacklogsModel> objListModel = new List<BacklogsModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                if(Type == "Item")
                {
                    ds = objdal.GetPurchaseItemBacklogs(Id);
                }
                if (Type == "Brand")
                {
                    ds = objdal.GetPurchaseBrandBacklogs(Id);
                }
                if (Type == "Vendor")
                {
                    ds = objdal.GetPurchaseVendorBacklogs(Id);
                }
                if (Type == "Customer")
                {
                    ds = objdal.GetPurchaseCustomerBacklogs(Id);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BacklogsModel Model = new BacklogsModel();
                        Model.PONumber = dr["PONumber"].ToString();
                        Model.PODate = Convert.ToDateTime(dr["Order Date"].ToString());
                        Model.VendorName = dr["VendorName"].ToString();
                        Model.Description = dr["Description"].ToString();
                        Model.Quantity = Convert.ToDouble(dr["Quantity"].ToString());
                        Model.QtyRec = Convert.ToDouble(dr["QtyRec"].ToString());
                        Model.QtyInv = Convert.ToDouble(dr["QtyInv"].ToString());
                        Model.BalanceQty = Convert.ToDouble(dr["BalanceQty"].ToString());
                        Model.UnitCost = Convert.ToDouble(dr["UnitCost"].ToString());
                        Model.Amount = Convert.ToDouble(dr["Amount"].ToString());
                        if(dr["CRD"].ToString() == "" || dr["CRD"].ToString() == "1/1/1753 12:00:00 AM")
                        {
                            Model.StrCRD = "";
                        }
                        else
                        {
                            Model.CRD = Convert.ToDateTime(dr["CRD"].ToString());
                            Model.StrCRD = Model.CRD.ToShortDateString();
                        }
                        if (dr["VPD"].ToString() == "" || dr["VPD"].ToString() == "1/1/1753 12:00:00 AM")
                        {
                            Model.StrVPD = "";
                        }
                        else
                        {
                            Model.VPD = Convert.ToDateTime(dr["VPD"].ToString());
                            Model.StrVPD = Model.VPD.ToShortDateString();
                        }
                       
                        Model.SalesOrderNumber = dr["SalesOrderNumber"].ToString();
                        objListModel.Add(Model);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objListModel;
        }
        public List<BacklogsModel> GetSalesBackLogsDetail(string Type, int Id)
        {
            List<BacklogsModel> objListModel = new List<BacklogsModel>();
            try
            {
                ViewsDAL objdal = new ViewsDAL();
                DataSet ds = new DataSet();
                if (Type == "Item")
                {
                    ds = objdal.GetSalesItemBacklogs(Id);
                }
                if (Type == "Brand")
                {
                    ds = objdal.GetSalesBrandBacklogs(Id);
                }
                if (Type == "Customer")
                {
                    ds = objdal.GetSalesCustomerBacklogs(Id);
                }
                if (Type == "Vendor")
                {
                    ds = objdal.GetSalesVendorBacklogs(Id);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        BacklogsModel Model = new BacklogsModel();
                        Model.PONumber = dr["PONumber"].ToString();
                        Model.PODate = Convert.ToDateTime(dr["PODate"].ToString());
                        Model.CustomerName = dr["CustomerName"].ToString();
                        Model.Description = dr["Description"].ToString();
                        Model.Quantity = Convert.ToDouble(dr["Quantity"].ToString());
                        Model.QtyRec = Convert.ToDouble(dr["QtyRec"].ToString());
                        Model.QtyInv = Convert.ToDouble(dr["QtyInv"].ToString());
                        Model.BalanceQty = Convert.ToDouble(dr["BalanceQty"].ToString());
                        Model.UnitCost = Convert.ToDouble(dr["UnitCost"].ToString());
                        Model.Amount = Convert.ToDouble(dr["Amount"].ToString());
                        if (dr["CRD"].ToString() == "" || dr["CRD"].ToString() == "1/1/1753 12:00:00 AM")
                        {
                            Model.StrCRD = "";
                        }
                        else
                        {
                            Model.CRD = Convert.ToDateTime(dr["CRD"].ToString());
                            Model.StrCRD = Model.CRD.ToShortDateString();
                        }
                        if (dr["VPD"].ToString() == "" || dr["VPD"].ToString() == "1/1/1753 12:00:00 AM")
                        {
                            Model.StrVPD = "";
                        }
                        else
                        {
                            Model.VPD = Convert.ToDateTime(dr["VPD"].ToString());
                            Model.StrVPD = Model.VPD.ToShortDateString();
                        }

                        Model.SalesOrderNumber = dr["SalesOrderNumber"].ToString();
                        objListModel.Add(Model);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objListModel;
        }
        #endregion Backlogs
    }
}
