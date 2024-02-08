using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.DW;
using SmartSys.BL.Enquiry;
using SmartSys.BL.Project;
using SmartSys.BL.TimeManagement;
using SmartSys.Utility;
using Syncfusion.EJ.Export;
using Syncfusion.JavaScript.Models;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SmartSys.Controllers
{
    [Authorize]
    public class ProjectCenterController : Controller
    {
        #region Project
        #region Risk Task
        public ActionResult RiskManageMent(string ProjectName, int ProjectId, int Taskid, int Modal, string Type)
        {
            ProjectBL objBL = new ProjectBL();
            AdminBL objbl = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            ProjectCustomerAndVendor custvndr = new ProjectCustomerAndVendor();
            custvndr = objBL.GetProjectCustomerAndVendor(ProjectId);
            ViewBag.ProjVendor = custvndr.ProjVendorName;
            ViewBag.ProjCustomer = custvndr.ProjCustomerName;
            List<DrpItem> CUstomer = new List<DrpItem>();
            ProjectBL ObjBl = new ProjectBL();
            CUstomer = ObjBl.GetCustomerListByUser(User_Id);
            ViewBag.CustList = new SelectList(CUstomer, "Id", "DisplayName");

            ViewBag.Type = Type;

            List<DrpItem> LstModel = new List<DrpItem>();
            LstModel = objBL.GetProjectTaskVendorList(ProjectId, Taskid, User_Id);
            ViewBag.VendorList = new SelectList(LstModel, "Id", "DisplayName");

            List<SysEmploy> DrpDwnEmp = new List<SysEmploy>();
            DrpDwnEmp = objbl.EmployeeDropDown();
            ViewBag.EmpList = DrpDwnEmp;
            ViewBag.Employee = DrpDwnEmp;
            TaskDetails model = new TaskDetails();

            model.LstProjectTaskDoc = new List<ProcjectDocModel>();
            model = objBL.GetSelectedRiskList(ProjectId, Taskid);
            if (ProjectName == "")
            {
                List<ProjectModel> lstproj = objBL.GetProjectList(User_Id);
                foreach (var item in lstproj)
                {
                    int projectid = item.ProjectId;
                    if (projectid == ProjectId)
                    {
                        model.ProjectName = item.ProjectName;
                        break;
                    }
                }
            }
            else
            {
                model.ProjectName = ProjectName;
            }

            model.LstCommentList = new List<Commentmodel>();
            if (Taskid > 0)
            {

                foreach (SysEmploy e in DrpDwnEmp)
                {
                    foreach (object o in model.ResourceID)
                    {
                        if (Convert.ToInt32(o.ToString()) == e.EmpId)
                        {
                            e.selected = true;
                        }
                    }
                }
                ViewBag.Employee = DrpDwnEmp;

                model.LstCommentList = objBL.GetSelectedProjectCommentList(ProjectId, Taskid);
            }
            if (model.LstCommentList.Count > 0)
            {
                foreach (Commentmodel temp in model.LstCommentList)
                {
                    if (temp.Status == "NEW" && temp.User_Id == User_Id)
                    {
                        model.AllowClose = "Yes";
                    }
                }

                var last = model.LstCommentList.Last();
                var CommentID = last.CommentId;
                var Comments = last.Comments;
                model.CommentId = CommentID;

            }

            model.Type = Type;
            model.Modal = Modal;
            model.ProjectId = ProjectId;


            return View(model);
        }

        [HttpPost]
        public ActionResult RiskManageMent(FormCollection fc)
        {
            TaskDetails model = new TaskDetails();
            ProjectBL Blobj = new ProjectBL();
            string User_Id = User.Identity.GetUserId();
            try
            {

                if (fc.AllKeys.Contains("Statuscode"))
                {
                    model.Statuscode = Convert.ToInt32(fc["Statuscode"].ToString());
                    model.Comments = fc["Comments"].ToString();
                }


                if (fc["Type"].ToString() == "Risk")
                {
                    model.TaskType = 2;
                }
                else
                {
                    model.TaskType = 3;
                }
                model.Progress = Convert.ToInt32(fc["txtPerComp"].ToString());
                model.ProjectId = Convert.ToInt32(fc["ProjectId"].ToString());
                model.CommentId = Convert.ToInt32(fc["CommentId"].ToString());
                model.TaskName = fc["TaskName"].ToString();
                model.TaskID = Convert.ToInt32(fc["TaskID"].ToString());
                model.StartDate = fc["StartDate"].ToString();
                model.EndDate = fc["EndDate"].ToString();
                model.Description = fc["Description"].ToString();
                int Duration = 0;
                if (fc.AllKeys.Contains("VendorId"))
                {
                    if (fc["VendorId"].ToString() != "")
                    { model.VendorId = Convert.ToInt32(fc["VendorId"].ToString()); }

                }
                if (fc.AllKeys.Contains("CustomerId"))
                {
                    if (fc["CustomerId"].ToString() != "")
                    { model.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString()); }

                }
                string ResourceIDs = fc["Employee"].ToString();
                int errorcode = Blobj.SaveRiskTask(model.ProjectId, model.TaskName, model.StartDate, model.EndDate, model.Description, ResourceIDs, model.TaskID, Duration, model.Statuscode, model.Comments, model.CommentId, model.TaskType, User_Id, model.Progress, model.VendorId, model.CustomerId);
                if (errorcode != null)
                {
                    return RedirectToAction("SendRiskCaseAlerts", new { ProjectId = model.ProjectId, TaskId = errorcode });
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }

            return View();
        }
        public ActionResult GetCaseList()
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/GetCaseList");
            if (FindFlag)
            {
                string UserName = User.Identity.Name;
                string UserID = User.Identity.GetUserId();
                ProjectBL Blobj = new ProjectBL();
                List<TaskDetails> Risklist = new List<TaskDetails>();
                Risklist = Blobj.GetCaseDetailList(UserName, UserID);
                return View(Risklist);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult GetRiskList()
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/GetRiskList");
            if (FindFlag)
            {
                string UserName = User.Identity.Name;
                string UserID = User.Identity.GetUserId();
                ProjectBL Blobj = new ProjectBL();
                List<TaskDetails> Risklist = new List<TaskDetails>();
                Risklist = Blobj.GetRiskDetailList(UserName, UserID);
                ViewBag.CaseList = Risklist;
                return View(Risklist);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult SendRiskCaseAlerts(int ProjectId, int TaskId)
        {
            TaskDetails model = new TaskDetails();
            try
            {
                ProjectModel proj = new ProjectModel();
                ProjectBL Blobj = new ProjectBL();
                DataSet ds = new DataSet();
                string User_Id = User.Identity.GetUserId();
                var userID = User.Identity.Name;
                string CCEmail = userID;
                SystemUser user = new SystemUser();
                SmartSys.BL.AdminBL smartsysBlObj = new SmartSys.BL.AdminBL();
                List<SystemUser> lstSysUserList = smartsysBlObj.GetSysUserList();
                foreach (var item in lstSysUserList)
                {
                    var UserName = item.UserName;
                    if (UserName == userID)
                    {
                        user.Email = item.Email;
                        break;
                    }

                }
                string EmailId = ConfigurationManager.AppSettings["SupportMail"];

                model.LstCommentList = Blobj.GetSelectedProjectCommentList(ProjectId, TaskId);
                if (model.LstCommentList[0].toMail.Trim().Length > 0)
                    EmailId = model.LstCommentList[0].toMail.Trim();
                string body = string.Empty;
                body +=
                    "Comment History Detail:" +
                     "<br /> <br />";
                body += "<table border='1'><tr style='background-color:#CEF6F5'><th>Comments</th><th>Status</th><th>Commented By</th><th>Comment Date</th></tr>";
                foreach (var item in model.LstCommentList)
                {
                    body += "<tr><td>" + item.Comments + "</td>";
                    body += "<td>" + item.Status + "</td>";
                    body += "<td>" + item.CommentedBy + "</td>";
                    body += "<td>" + item.CommentDate + "</td></tr> ";

                }
                if (EmailId.ToString() != "")
                {
                    model = Blobj.GetSelectedRiskList(ProjectId, TaskId);
                    List<ProjectModel> lstproj = Blobj.GetProjectList(User_Id);
                    List<TaskDetails> Risklist = new List<TaskDetails>();
                    Risklist = Blobj.GetRiskList(userID);
                    foreach (var item in lstproj)
                    {
                        if (item.ProjectId == ProjectId)
                        {
                            proj.ProjectName = item.ProjectName;
                            proj.ProjectManager = item.ProjectManager;
                            proj.Email = item.Email;
                            break;
                        }

                    }
                    string ccmail = proj.Email + "," + user.Email;
                    foreach (var item in Risklist)
                    {
                        if (item.ProjectId == ProjectId && item.TaskID == TaskId)
                        {
                            model.TaskName = item.TaskName;
                            model.TskType = item.TskType;
                            model.Status = item.Status;
                            model.StartDate = item.StartDate;
                            model.EndDate = item.EndDate;
                            model.Description = item.Description;

                            break;
                        }
                    }
                    string Subject = proj.ProjectName;
                    string MailBody = "<html>" +
                      "<head>" +
                       "<meta http-equiv= " + "Content-Type" + "content= " + "text/html; charset=UTF-8" + ">" +
                       "<meta name=" + "generator" + " content=" + "SautinSoft.RtfToHtml.dll" + ">" +
                       "<title>" + "Leave Mail" + "</title>" +
                       "<style type=" + "text/css>" +
                      ".st1{font-family:Calibri;font-size:11pt;color:#000000;font-weight:bold}" +
                      ".st2{font-family:Calibri;font-size:11pt;color:#000000;}" +
                      "</style>" +
                       "</head>" +
                       "<body>" +
                       "<div>" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Project Id : " + "</b>" + ProjectId + "</span>" + "</div>" +
                          "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Task Id : " + "</b>" + TaskId + "</span>" + "</div>" +
                         "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Project Name : " + "</b>" + proj.ProjectName + "</span>" + "</div>" +
                         "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Project Manager : " + "</b>" + proj.ProjectManager + "</span>" + "</div>" +
                          "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Description: " + "</b>" + model.Description + "</span>" + "</div>" +

                         "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Task Type : " + "</b>" + model.TskType + "</span>" + "</div>" +
                                                  "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + "Task Name: " + "</b>" + model.TaskName + "</span>" + "</div>" +
                         "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Start Date : " + "</b>" + model.StartDate + "</span>" + "</div>" +
                         "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " End Date : " + "</b>" + model.EndDate + "</span>" + "</div>" +
                         "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Status : " + "</b>" + model.Status + "</span>" + "</div>" +
                          "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Comments : " + "</b>" + body + "</span>" + "</div>" +
                        "</body>" +
                       "</html>";
                    List<ProcjectDocModel> lst = model.LstProjectTaskDoc;

                    ArrayList attachments = new ArrayList();

                    Stream responseStream = null;
                    foreach (var item in lst)
                    {
                        var attachment = item.FileName;
                        if (attachment.ToString() != "")
                        {

                            String FTPServer = Common.GetConfigValue("FTP");
                            String FTPUserId = Common.GetConfigValue("FTPUid");
                            String FTPPwd = Common.GetConfigValue("FTPPWD");

                            FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + attachment);
                            requestFileDownload.UseBinary = true;
                            requestFileDownload.KeepAlive = false;
                            requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                            requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                            FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                            responseStream = responseFileDownload.GetResponseStream();
                            System.Net.Mail.Attachment at = new System.Net.Mail.Attachment(responseStream, attachment);
                            attachments.Add(at);

                        }
                    }

                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            int mail = Utility.Common.SendMail(Subject, MailBody, "", EmailId, ccmail, attachments);
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (i == 2)
                            {
                                throw ex;
                            }
                        }
                    }
                    responseStream.Close();

                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return RedirectToAction("RiskManageMent", new { ProjectId = ProjectId, taskid = TaskId, Modal = 0, ProjectName = "", Type = "" });
        }

        public ActionResult ReviewRiskTaskList()
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/ReviewRiskTaskList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                ProjectBL objBL = new ProjectBL();
                List<TaskDetails> Risklist = new List<TaskDetails>();
                Risklist = objBL.GetRiskReviewList(User_Id);
                return View(Risklist);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ReViewRiskTask(int ProjectId, int Taskid)
        {
            AdminBL objbl = new AdminBL();
            List<SysEmploy> DrpDwnEmp = new List<SysEmploy>();
            DrpDwnEmp = objbl.EmployeeDropDown();
            ViewBag.EmpList = new SelectList(DrpDwnEmp, "EmpId", "EmpName");
            TaskDetails model = new TaskDetails();
            ProjectBL objBL = new ProjectBL();
            model = objBL.GetSelectedRiskList(ProjectId, Taskid);
            return PartialView(model);
        }

        public ActionResult SaveReViewRiskTask(int statusCode, int TaskId, int ProjectId)
        {
            string User_Id = User.Identity.GetUserId();
            ProjectBL Blobj = new ProjectBL();
            int errorcode = Blobj.UpdateRiskTaskReview(statusCode, ProjectId, TaskId, User_Id);
            if (errorcode == 25)
            {
                return RedirectToAction("ReviewRiskTaskList");
            }
            else
            {
                return RedirectToAction("ReviewRiskTaskList");
            }

        }

        public ActionResult ApprovalRiskTaskList()
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/ApprovalRiskTaskList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                ProjectBL objBL = new ProjectBL();
                List<TaskDetails> ApprovalList = new List<TaskDetails>();
                ApprovalList = objBL.GetRiskApprovalList(User_Id);
                return View(ApprovalList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ApproveRiskTask(int ProjectId, int Taskid)
        {

            AdminBL objbl = new AdminBL();
            List<SysEmploy> DrpDwnEmp = new List<SysEmploy>();
            DrpDwnEmp = objbl.EmployeeDropDown();
            ViewBag.EmpList = new SelectList(DrpDwnEmp, "EmpId", "EmpName");
            TaskDetails model = new TaskDetails();
            ProjectBL objBL = new ProjectBL();
            model = objBL.GetSelectedRiskList(ProjectId, Taskid);
            return PartialView(model);
        }
        public ActionResult ViewRiskCaseDetail(string ProjectName, int ProjectId, int Taskid, int Modal, string Type)
        {
            ProjectBL objBL = new ProjectBL();
            AdminBL objbl = new AdminBL();
            List<SysEmploy> DrpDwnEmp = new List<SysEmploy>();
            DrpDwnEmp = objbl.EmployeeDropDown();
            ViewBag.EmpList = DrpDwnEmp;
            ViewBag.Employee = DrpDwnEmp;
            TaskDetails model = new TaskDetails();
            string User_Id = User.Identity.GetUserId();
            if (ProjectName == null)
            {
                List<ProjectModel> lstproj = objBL.GetProjectList(User_Id);
                foreach (var item in lstproj)
                {
                    int projectid = item.ProjectId;
                    if (projectid == ProjectId)
                    {
                        model.ProjectName = item.ProjectName;
                        break;
                    }
                }
            }
            else
            {
                model.ProjectName = ProjectName;
            }

            model.LstCommentList = new List<Commentmodel>();
            if (Taskid > 0)
            {
                model = objBL.GetSelectedRiskList(ProjectId, Taskid);
                foreach (SysEmploy e in DrpDwnEmp)
                {
                    foreach (object o in model.ResourceID)
                    {
                        if (Convert.ToInt32(o.ToString()) == e.EmpId)
                        {
                            e.selected = true;
                        }
                    }
                }
                ViewBag.Employee = DrpDwnEmp;

                model.LstCommentList = objBL.GetSelectedProjectCommentList(ProjectId, Taskid);
            }
            if (model.LstCommentList.Count > 0)
            {
                var last = model.LstCommentList.Last();
                var CommentID = last.CommentId;
                var Comments = last.Comments;
                model.CommentId = CommentID;

            }

            model.Type = Type;
            //if (Modal == 0)
            //{
            //    return PartialView(model);
            //}
            model.Modal = Modal;
            model.ProjectId = ProjectId;


            return PartialView(model);
        }
        public ActionResult UpdateApproveRiskTask(int statusCode, int TaskId, int ProjectId)
        {
            string User_Id = User.Identity.GetUserId();
            ProjectBL Blobj = new ProjectBL();
            int errorcode = Blobj.UpdateRiskTaskApproval(statusCode, ProjectId, TaskId, User_Id);
            if (errorcode == 25)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("ApprovalRiskTaskList");
            }
            return View();
        }
        #endregion Risk Task

        public ActionResult ProjectEditMode(int TaskID, int ProjectId, string Type)
        {
            TaskDetails Model = new TaskDetails();
            ProjectBL Blobj = new ProjectBL();
            string User_Id = User.Identity.GetUserId();
            Model = Blobj.GetSelectedProjectTask(ProjectId, TaskID);
            ViewBag.CustList = new SelectList(Blobj.GetCustomerListByUser(User_Id), "Id", "DisplayName");
            ViewBag.VendList = new SelectList(Blobj.GetVendorListByUser(User_Id), "Id", "DisplayName");
            ViewBag.Type = Type;
            Model.LstCommentList = new List<Commentmodel>();
            Model.LstCommentList = Blobj.GetSelectedProjectCommentList(ProjectId, TaskID);
            return View(Model);
        }
        [HttpPost]
        public ActionResult ProjectEditMode(FormCollection fc)
        {
            TaskDetails Model = new TaskDetails();
            try
            {
                string User_Id = User.Identity.GetUserId();
                ProjectBL objBL = new ProjectBL();
                Model.EstimationTime = fc["EstimationTime"].ToString();
                Model.ProjectId = Convert.ToInt32(fc["ProjectId"].ToString());
                Model.TaskID = Convert.ToInt32(fc["TaskID"].ToString());
                Model.TaskName = fc["TaskName"].ToString();
                Model.TaskName = fc["TaskName"].ToString();
                Model.Comments = fc["Comments"].ToString();
                Model.Description = fc["Description"].ToString();
                if (fc.AllKeys.Contains("CustomerId"))
                {
                    if (fc["CustomerId"].ToString().Trim().Length > 0)
                        Model.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                }
                if (fc.AllKeys.Contains("VendorId"))
                {
                    if (fc["VendorId"].ToString().Trim().Length > 0)
                        Model.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                }
                Model.Progress = Convert.ToInt32(fc["txtPerComp"].ToString());
                int errrcode = objBL.UpdateProjectTaskDetails(Model, User_Id);
                if (errrcode == 500000 || errrcode == 500001)
                {
                    return RedirectToAction("VwProject", new { ProjectId = Model.ProjectId });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("VwProject", new { ProjectId = Model.ProjectId });
        }

        public ActionResult ProjectTaskAttachment(int ProjectId, int TaskId , string Path)
        {
            ProcjectDocModel objmodel = new ProcjectDocModel();
            objmodel.ProjectId = ProjectId;
            objmodel.TaskId = TaskId;
            objmodel.Path = Path;
            return PartialView(objmodel);
        }
        [HttpPost]
        public ActionResult ProjectTaskAttachment(HttpPostedFileBase file, FormCollection fc)
        {
            ProcjectDocModel Attachmentmodel = new ProcjectDocModel();
            ProjectBL objBL = new ProjectBL();
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");
            string fileName = System.IO.Path.GetFileName(file.FileName);
            string User_Id = User.Identity.GetUserId();
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    try
                    {
                        string[] FileSplit = fileName.Split('.');
                        string FileEx = FileSplit[1].ToString();
                        String guid = Guid.NewGuid().ToString();
                        string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                        string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        string FileName = Convert.ToString("/ProjectAttachmentDoc/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
                        string localPath = Server.MapPath("~/App_Data/uploads");
                        if (!System.IO.Directory.Exists(localPath))
                        {
                            System.IO.Directory.CreateDirectory(localPath);
                        }
                        file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName));
                        if (ftpServer.Trim().Length > 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {

                                FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                                requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                                int bufferLength = 2048;
                                byte[] buffer = new byte[file.InputStream.Length];
                                Stream uploadStream = requestFTPUploader.GetRequestStream();
                                int contentLength = file.InputStream.Read(buffer, 0, Convert.ToInt32(file.InputStream.Length));
                                while (contentLength != 0)
                                {
                                    uploadStream.Write(buffer, 0, contentLength);
                                    contentLength = file.InputStream.Read(buffer, 0, bufferLength);
                                }
                                uploadStream.Close();
                                //fileStream.Close();
                                requestFTPUploader = null;
                                Attachmentmodel.FileName = FileName;
                                break;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            Attachmentmodel.Description = fc["Description"].ToString();
            Attachmentmodel.ProjectId = Convert.ToInt32(fc["ProjectId"].ToString());
            Attachmentmodel.TaskId = Convert.ToInt32(fc["TaskId"].ToString());
            Attachmentmodel.Path = fc["Path"].ToString();
            int errcode = objBL.SaveProjectTaskDoc(Attachmentmodel, User_Id);
            if (errcode == 500001 || errcode == 500002)
            {
                if(Attachmentmodel.Path== "ProjectEditMode")
                return RedirectToAction("ProjectEditMode", new { TaskID = Attachmentmodel.TaskId, ProjectId = Attachmentmodel.ProjectId });
                else
               return RedirectToAction("RiskManageMent", new { TaskID = Attachmentmodel.TaskId, ProjectId = Attachmentmodel.ProjectId, Modal = 1, Type = "Case", ProjectName =""});
            }
            else
            {
                if (file.ContentLength > 0)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        try
                        {
                            FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(ftpServer + Attachmentmodel.FileName);
                            requestFileDelete.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                            requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                            FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                            requestFileDelete = null;

                            break;
                        }
                        catch (Exception ex)
                        {
                            if (x > 2)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            return RedirectToAction("ProjectEditMode", new { TaskID = Attachmentmodel.TaskId, ProjectId = Attachmentmodel.ProjectId });
        }

        public ActionResult PrjectDocDownload(int TaskID, int ProjectId, string Filename)
        {
            try
            {
                String FTPServer = Common.GetConfigValue("FTP");
                String FTPUserId = Common.GetConfigValue("FTPUid");
                String FTPPwd = Common.GetConfigValue("FTPPWD");

                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + Filename);
                requestFileDownload.UseBinary = true;
                requestFileDownload.KeepAlive = false;
                requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                byte[] OPData = ReadFully(responseStream);
                responseStream.Close();

                Response.AddHeader("content-disposition", "attachment; filename=" + Filename);
                Response.ContentType = "application/zip";
                Response.BinaryWrite(OPData);
                Response.End();
            }
            catch (Exception e)
            {
                throw e;
            }

            return RedirectToAction("ProjectEditMode", new { TaskID = TaskID, ProjectId = ProjectId });
        }
        public ActionResult VwProject(int ProjectId)
        {
            string User_Id = User.Identity.GetUserId();
            ProjectBL objBL = new ProjectBL();
            ProjectMaster ProjMast = objBL.GetProjectTaskData(ProjectId, User_Id);
            List<BL.Project.Resources> EmpList = objBL.GetResourceDataAllEmp();
            ViewBag.EmpList = EmpList;
            return View(ProjMast);
        }

        [HttpPost]
        public int VwProjectAddNew(ProjectModel ProjDetail)
        {
            string User_Id = User.Identity.GetUserId();
            int ProjectId = 0;
            ProjectBL objBL = new ProjectBL();
            ProjectId = objBL.SaveProject(ProjDetail, User_Id);
            return ProjectId;
        }
        public ActionResult Index()
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool AllowDelete;
            bool FindFlag;
            AllowDelete = modelObj.FindMenu(lstTaskmodel, "IsDeleted");
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/Index");
            if (FindFlag)
            {
                if (AllowDelete == true)
                {
                    TempData["Message"] = "AllowDelete";
                }
                string User_Id = User.Identity.GetUserId();
                string UserName = User.Identity.Name;

                ProjectBL objBL = new ProjectBL();
                ProjectModel proj = new ProjectModel();
                List<ProjectModel> lstproj = objBL.GetProjectList(User_Id);
                proj.AllowDelete = AllowDelete;
                List<ProjectTypeModel> lstProjType = objBL.GetProjectTypeData();
                AdminBL objbl = new AdminBL();
                List<SysEmploy> DrpDwnEmp = new List<SysEmploy>();

                List<TaskDetails> Risklist = new List<TaskDetails>();
                Risklist = objBL.GetRiskList(UserName);
                ViewBag.SelectedRiskDetail = Risklist;
                DrpDwnEmp = objbl.EmployeeDropDown();
                ViewBag.EmpList = new SelectList(DrpDwnEmp, "EmpId", "EmpName");
                ViewBag.ProjTypeList = new SelectList(lstProjType, "ProjectTypeId", "ProjectType");

                //BudgetModel budgetModel = new BudgetModel();
                //budgetModel.CustomerLst = new List<BudgetCustomerModel>();
                //CustomerBL objbl1 = new CustomerBL();
                //budgetModel.CustomerLst = objbl1.GetBudgetCustomerList(); //Customer List
                //ViewBag.CustomerList = new SelectList(budgetModel.CustomerLst, "CustomerId", "CustomerName");
                //List<EmployeeVendor> lstvend = objbl.GetEmployeeVendorListForDropDown(0);
                //ViewBag.VendorList = new SelectList(lstvend, "VendorId", "VendorName");
                CustomerMappingModel CustomerMap = new CustomerMappingModel();
                CustomerBL BlObj = new CustomerBL();
                CustomerMap.lstCustomerMap = BlObj.GetDWCompList();
                ViewBag.CompanyList = new SelectList(CustomerMap.lstCustomerMap, "CompCode", "CompName");

                ViewBag.CustomerList = new SelectList(objBL.GetCustomerListByUser(User_Id), "Id", "DisplayName");
                ViewBag.VendorList = new SelectList(objBL.GetVendorListByUser(User_Id), "Id", "DisplayName");

                List<ItemSegment> SegmentLIst = new List<ItemSegment>();
                AdminBL objblseg = new AdminBL();
                SegmentLIst = objblseg.GetItemSegmentList(User_Id);
                ViewBag.SegmentList = new SelectList(SegmentLIst, "SegmentId", "SegmentName");

                return View(lstproj);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult DeleteProject(int ProjectId, int ManagerId, string ProjectName)
        {
            ProjectModel proj = new ProjectModel();
            ProjectBL objBL = new ProjectBL();
            int projid = objBL.DeleteProject(ProjectId);
            if (projid != null)
            {
                return RedirectToAction("SendProjDeleteAlerts", new { ProjectId = ProjectId, ManagerId = ManagerId, ProjectName = ProjectName });
            }
            return RedirectToAction("SendProjDeleteAlerts", new { ProjectId = ProjectId, ManagerId = ManagerId, ProjectName = ProjectName });
        }
        public ActionResult SendProjDeleteAlerts(int ProjectId, int ManagerId, string ProjectName)
        {
            TaskDetails model = new TaskDetails();
            try
            {
                ProjectModel proj = new ProjectModel();
                ProjectBL Blobj = new ProjectBL();
                DataSet ds = new DataSet();
                string User_Id = User.Identity.GetUserId();
                var userID = User.Identity.Name;
                string CCEmail = userID;
                SysUser objUserModel = new SysUser();
                EmployeeModel EmpModel = new EmployeeModel();
                AdminBL objBL = new AdminBL();
                EmpModel = objBL.GetSelectedEmployeeList(ManagerId);
                string Tomail = EmpModel.emailId;
                string EmailId = Tomail;
                string managername = EmpModel.FirstName + ' ' + EmpModel.LastName;
                objUserModel = objBL.GetUserProfile(User_Id);
                var Name = objUserModel.Name;
                if (EmailId.ToString() != "")
                {

                    string Subject = " Project has been  Deleted By  " + Name + "";
                    string MailBody = "<html>" +
                      "<head>" +
                       "<meta http-equiv= " + "Content-Type" + "content= " + "text/html; charset=UTF-8" + ">" +
                       "<meta name=" + "generator" + " content=" + "SautinSoft.RtfToHtml.dll" + ">" +
                       "<title>" + "Leave Mail" + "</title>" +
                       "<style type=" + "text/css>" +
                      ".st1{font-family:Calibri;font-size:11pt;color:#000000;font-weight:bold}" +
                      ".st2{font-family:Calibri;font-size:11pt;color:#000000;}" +
                      "</style>" +
                       "</head>" +
                       "<body>" +
                       "<div>" +
                          "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Project Id : " + "</b>" + ProjectId + "</span>" + "</div>" +
                          "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Project Name : " + "</b>" + ProjectName + "</span>" + "</div>" +
                          "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Manager : " + "</b>" + managername + "</span>" + "</div>" +

                       "</html>";

                    ArrayList attachments = new ArrayList();
                    attachments = null;
                    Stream responseStream = null;

                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            int mail = Utility.Common.SendMail(Subject, MailBody, "Ajit_Photo_2522016_1053_7df16aff-f9f2-4f58-83d6-3b6d44703fb0.JPG", EmailId, CCEmail, attachments);
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (i == 2)
                            {
                                throw ex;
                            }
                        }
                    }
                    responseStream.Close();

                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return RedirectToAction("Index");
        }
        public ActionResult VwProjectTaskSave(TaskDetails taskDetail, List<BL.Project.Resources> Resource, int ProjectId)
        {
            string User_Id = User.Identity.GetUserId();
            List<object> lstres = new List<object>();
            if (Resource != null)
            {
                foreach (BL.Project.Resources r in Resource)
                {
                    lstres.Add(r.ResourceID);
                }
            }
            taskDetail.ProjectId = ProjectId;
            //taskDetail.ResourceID = lstres;
            ProjectBL objBL = new ProjectBL();
            objBL.SaveProjectTask(taskDetail, User_Id);
            return View();
        }

        [HttpPost]
        public ActionResult VwProjectTaskDelete(TaskDetails taskDetail, int ProjectId)
        {
            string User_Id = User.Identity.GetUserId();
            taskDetail.ProjectId = ProjectId;
            ProjectBL objBL = new ProjectBL();
            objBL.DeleteProjectTask(taskDetail, User_Id);
            return View();
        }

        #endregion Project

        #region Project Type
        public ActionResult VwProjectType(int ProjectTypeId)
        {
            ProjectBL objBL = new ProjectBL();
            ProjectTypeMaster objMast = objBL.GetProjectTypeTaskData(ProjectTypeId);
            return View(objMast);
        }

        public int VwProjectTypeNew(ProjectTypeModel projectTypeDetail)
        {
            string User_Id = User.Identity.GetUserId();
            ProjectBL objBL = new ProjectBL();
            int ProjectTypeId = objBL.SaveProjectType(projectTypeDetail, User_Id);
            return (ProjectTypeId);
        }

        public ActionResult VwProjectTypeList()
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/VwProjectTypeList");
            if (FindFlag)
            {
                ProjectBL objBL = new ProjectBL();
                List<ProjectTypeModel> lstProjType = objBL.GetProjectTypeData();
                return View(lstProjType);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public JsonResult GetProjectTypeList()
        {
            ProjectBL objBL = new ProjectBL();
            List<ProjectTypeModel> lstProjType = objBL.GetProjectTypeData();
            return Json(lstProjType, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult VwProjectTypeDelete(ProjectTypeTaskDetails taskDetail, int ProjTypeId)
        {
            string User_Id = User.Identity.GetUserId();
            taskDetail.ProjectTypeId = ProjTypeId;
            ProjectBL objBL = new ProjectBL();
            objBL.DeleteProjectTypeTask(taskDetail, User_Id);
            return View();
        }

        [HttpPost]
        public ActionResult VwProjectTypeAddNew(ProjectTypeTaskDetails taskDetail, int ProjTypeId)
        {
            string User_Id = User.Identity.GetUserId();
            taskDetail.ProjectTypeId = ProjTypeId;
            ProjectBL objBL = new ProjectBL();
            objBL.SaveProjectTypeTask(taskDetail, User_Id);
            return View();
        }

        [HttpPost]
        public ActionResult VwProjectTypeUpdate(ProjectTypeTaskDetails taskDetail, int ProjTypeId)
        {
            string User_Id = User.Identity.GetUserId();
            taskDetail.ProjectTypeId = ProjTypeId;
            ProjectBL objBL = new ProjectBL();
            objBL.SaveProjectTypeTask(taskDetail, User_Id);
            return View();
        }

        public ActionResult EditMode(int ProjectTypeTaskID, int ProjectTypeId)
        {
            ProjectTypeTaskDetails Model = new ProjectTypeTaskDetails();
            ProjectBL Blobj = new ProjectBL();
            Model = Blobj.GetSelectedProjectEditModeList(ProjectTypeId, ProjectTypeTaskID);
            return View(Model);
        }
        [HttpPost]
        public ActionResult EditMode(FormCollection fc)
        {
            ProjectTypeTaskDetails Model = new ProjectTypeTaskDetails();
            try
            {
                string User_Id = User.Identity.GetUserId();
                ProjectBL objBL = new ProjectBL();
                Model.ProjectTypeId = Convert.ToInt32(fc["ProjectTypeId"].ToString());
                Model.ProjectTypeTaskID = Convert.ToInt32(fc["ProjectTypeTaskID"].ToString());
                Model.TaskName = fc["TaskName"].ToString();
                Model.Description = fc["Description"].ToString();
                int errrcode = objBL.UpdatePrjectTypeTaskDetails(Model, User_Id);
                if (errrcode == 500000 || errrcode == 500001)
                {
                    return RedirectToAction("VwProjectType", new { ProjectTypeId = Model.ProjectTypeId });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("VwProjectType", new { ProjectTypeId = Model.ProjectTypeId });
        }
        public ActionResult PrjectTaskAttachment(int ProjectTypeId, int ProjectTypeTaskId)
        {
            ProcjectTypeDocModel objmodel = new ProcjectTypeDocModel();
            objmodel.ProjectTypeId = ProjectTypeId;
            objmodel.ProjectTypeTaskId = ProjectTypeTaskId;
            return PartialView(objmodel);
        }
        [HttpPost]
        public ActionResult PrjectTaskAttachment(HttpPostedFileBase file, FormCollection fc)
        {
            ProcjectTypeDocModel Attachmentmodel = new ProcjectTypeDocModel();
            ProjectBL objBL = new ProjectBL();
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");
            string fileName = System.IO.Path.GetFileName(file.FileName);
            string User_Id = User.Identity.GetUserId();
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    try
                    {
                        string[] FileSplit = fileName.Split('.');
                        string FileEx = FileSplit[1].ToString();
                        String guid = Guid.NewGuid().ToString();
                        string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                        string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        string FileName = Convert.ToString(FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
                        string localPath = Server.MapPath("~/App_Data/uploads");
                        if (!System.IO.Directory.Exists(localPath))
                        {
                            System.IO.Directory.CreateDirectory(localPath);
                        }
                        file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName));
                        if (ftpServer.Trim().Length > 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {

                                FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                                requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                                int bufferLength = 2048;
                                byte[] buffer = new byte[file.InputStream.Length];
                                Stream uploadStream = requestFTPUploader.GetRequestStream();
                                int contentLength = file.InputStream.Read(buffer, 0, Convert.ToInt32(file.InputStream.Length));
                                while (contentLength != 0)
                                {
                                    uploadStream.Write(buffer, 0, contentLength);
                                    contentLength = file.InputStream.Read(buffer, 0, bufferLength);
                                }
                                uploadStream.Close();
                                //fileStream.Close();
                                requestFTPUploader = null;
                                Attachmentmodel.FileName = FileName;
                                break;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            Attachmentmodel.Description = fc["Description"].ToString();
            Attachmentmodel.ProjectTypeId = Convert.ToInt32(fc["ProjectTypeId"].ToString());
            Attachmentmodel.ProjectTypeTaskId = Convert.ToInt32(fc["ProjectTypeTaskId"].ToString());
            int errcode = objBL.SavePrjectDoc(Attachmentmodel, User_Id);
            if (errcode == 500001 || errcode == 500002)
            {

                return RedirectToAction("EditMode", new { ProjectTypeTaskID = Attachmentmodel.ProjectTypeTaskId, ProjectTypeId = Attachmentmodel.ProjectTypeId });
            }
            else
            {
                if (file.ContentLength > 0)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        try
                        {
                            FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(ftpServer + Attachmentmodel.FileName);
                            requestFileDelete.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                            requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                            FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                            requestFileDelete = null;

                            break;
                        }
                        catch (Exception ex)
                        {
                            if (x > 2)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            return RedirectToAction("EditMode", new { ProjectTypeTaskID = Attachmentmodel.ProjectTypeTaskId, ProjectTypeId = Attachmentmodel.ProjectTypeId });
        }

        public ActionResult DocDownload(int ProjectTypeTaskID, int ProjectTypeId, string Filename)
        {
            try
            {
                String FTPServer = Common.GetConfigValue("FTP");
                String FTPUserId = Common.GetConfigValue("FTPUid");
                String FTPPwd = Common.GetConfigValue("FTPPWD");

                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + Filename);
                requestFileDownload.UseBinary = true;
                requestFileDownload.KeepAlive = false;
                requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                byte[] OPData = ReadFully(responseStream);
                responseStream.Close();

                Response.AddHeader("content-disposition", "attachment; filename=" + Filename);
                Response.ContentType = "application/zip";
                Response.BinaryWrite(OPData);
                Response.End();
            }
            catch (Exception e)
            {
                throw e;
            }

            return RedirectToAction("CreateProjectTAskMOM", new { ProjectTypeTaskID = ProjectTypeTaskID, ProjectTypeId = ProjectTypeId });
        }
        #endregion Project Type

        #region Project Task MOM
        public ActionResult ProjTaskMOMList()
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/ProjTaskMOMList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                ProjectBL objBL = new ProjectBL();
                List<ProjectTaskMoM> lstprojMOM = objBL.ProjTaskMOMList(User_Id);
                return View(lstprojMOM);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        //public ActionResult DeleteMomActionPointParticipants(int ActionPointId, string ParticipantsType, int MOMId, int UserId)
        //{
        //    MOMActionPoint model = new MOMActionPoint();
        //    model.MOMId = MOMId;
        //    ProjectBL objBL = new ProjectBL();
        //    int errcode = objBL.DeleteMomActionPointParticipants(ActionPointId, ParticipantsType, UserId);
        //    return RedirectToAction("CreateProjectTAskMOM", new { MOMID = MOMId, tabIndex = 0 });
        //}
        public void ActionPointExport(string GridModel)
        {
            string user_Id = User.Identity.GetUserId();
            EnquiryBL BLObj = new EnquiryBL();


            var DataSource = Session["exportdetail"] as List<MOMActionPoint>;
            Session["exportdetail"] = null;
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        private GridProperties ConvertGridObject(string gridProperty)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IEnumerable div = (IEnumerable)serializer.Deserialize(gridProperty, typeof(IEnumerable));
            GridProperties gridProp = new GridProperties();
            foreach (KeyValuePair<string, object> ds in div)
            {
                var property = gridProp.GetType().GetProperty(ds.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (property != null)
                {
                    Type type = property.PropertyType;
                    string serialize = serializer.Serialize(ds.Value);
                    object value = serializer.Deserialize(serialize, type);
                    property.SetValue(gridProp, value, null);
                }
            }
            return gridProp;
        }
        public ActionResult CreateProjectTAskMOM(int MOMId, int tabIndex)
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/CreateProjectTAskMOM?MOMId=0&tabIndex=0");
            FindFlag = true;
            if (FindFlag)
            {
                ProjectTaskMoM MOMModel = new ProjectTaskMoM();
                //This is for avoiding the session timeout as tactical solution
                if (Session["UserMenu"] == null)
                {
                    lstTaskmodel = objBl.GetTaskMenuList(UserId);
                    Session["UserMenu"] = lstTaskmodel;
                }

                lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
                FindFlag = modelObj.FindMenu(lstTaskmodel, "MOMManagementView");
                if (FindFlag)
                {
                    ViewBag.findflag = 1;
                }
                TempData["UserID"] = null;
                ProjectBL objBL = new ProjectBL();
                string User_Id = User.Identity.GetUserId();
                List<ProjectModel> lstproj = objBL.GetProjectlistByUserId(User_Id);
                ViewBag.project = new SelectList(lstproj, "ProjectId", "ProjectName");
                MOMModel.LstProjTask = new List<ProjTaskModel>();
                if (Session["Participant"] != null)
                {
                    TempData["msg"] = Session["Participant"] as string;
                }
                Session["Participant"] = null;
                MOMModel.LstMoMParticipant = new List<MoMParticipantModel>();
                MOMModel.LstMOMAttachment = new List<MoMAttachmentModel>();
                MOMModel.ActionPointList = new List<MOMActionPoint>();
                List<MOMType> MOMType = new List<MOMType>();
                MOMType = objBL.MOMType();
                //   ViewBag.LstActionPointParticipant = objBL.GetselectedMOMAParticipant(MOMId);
                ViewBag.MOMTypeList = new SelectList(MOMType, "MOMTypeKey", "MOMTypeValue");
                if (MOMId > 0)
                {
                    ProjectBL BLObj = new ProjectBL();


                    MOMModel = BLObj.GetSelectedProjectTaskMOM(MOMId);
                    MOMModel.LstMoMParticipant = BLObj.MOMParticipantList(MOMId);
                    MOMModel.LstMOMAttachment = BLObj.MOMAttachmentList(MOMId);
                    MOMModel.TabIndex = tabIndex;
                    MOMModel.LstProjTask = objBL.ProjTaskList(MOMModel.ProjectId);
                    MOMModel.ActionPointList = BLObj.MOMActionPointList(MOMId);

                    Session["exportdetail"] = MOMModel.ActionPointList;
                    ViewBag.ProjectTask = new SelectList(MOMModel.LstProjTask, "TaskID", "TaskName");
                }
                ProjectBL Blobj = new ProjectBL();
                ViewBag.CustomerList = new SelectList(Blobj.GetCustomerListByUser(User_Id), "Id", "DisplayName");
                ViewBag.VendorList = new SelectList(Blobj.GetVendorListByUser(User_Id), "Id", "DisplayName");
                return View(MOMModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateProjectTAskMOM(FormCollection FC)
        {
            try
            {
                ProjectBL objBL = new ProjectBL();
                string User_Id = User.Identity.GetUserId();
                ProjectTaskMoM MOMModel = new ProjectTaskMoM();
                if (FC["MOMId"].ToString() == "")
                {
                    MOMModel.MOMId = 0;
                }
                else
                {
                    MOMModel.MOMId = Convert.ToInt32(FC["MOMId"].ToString());
                }
                MOMModel.ProjectId = Convert.ToInt32(FC["ProjectId"].ToString());
                MOMModel.MOMTypeKey = FC["MOMTypeKey"].ToString();
                MOMModel.TaskId = Convert.ToInt32(FC["TaskId"].ToString());
                MOMModel.MOMDate = Convert.ToDateTime(FC["MOMDate"]);
                MOMModel.Title = FC["Title"].ToString();
                MOMModel.Description = FC["Description"].ToString();
                MOMModel.LocalDescription = FC["LocalDescription"].ToString();
                if (FC.AllKeys.Contains("ManageMentView"))
                    MOMModel.ManageMentView = FC["ManageMentView"].ToString();
                if(FC["CustomerId"].ToString() != "")
                MOMModel.CustomerId= Convert.ToInt32(FC["CustomerId"].ToString());
                if (FC["VendorId"].ToString() != "")
                    MOMModel.VendorId = Convert.ToInt32(FC["VendorId"].ToString());
                int errcode = objBL.SaveProjTaskMOM(MOMModel, User_Id);
                if (errcode == 500001 || errcode == 500002 || errcode != null)
                {
                    return RedirectToAction("CreateProjectTAskMOM", new { MOMId = errcode, tabIndex = 0 });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        public ActionResult AddParticipants(int MOMId, int ProjectId)
        {
            MoMParticipantModel model = new MoMParticipantModel();
            model.MOMId = MOMId;
            string User_Id = User.Identity.GetUserId();
            AdminBL objbl = new AdminBL();
            List<SysEmploy> DrpDwnEmp = new List<SysEmploy>();
            DrpDwnEmp = objbl.EmployeeDropDown();
            ViewBag.Employee = new SelectList(DrpDwnEmp, "EmpId", "EmpName");

            VendorListModel VendorModel = new VendorListModel();
            VendorBL VendorBL = new VendorBL();
            VendorModel.vendorGedList = VendorBL.GetVendorListHavingContacts(ProjectId, User_Id);
            ViewBag.Vendor = new SelectList(VendorModel.vendorGedList, "VendorId", "VendorName");

            CustomerListModel CustomerModel = new CustomerListModel();
            CustomerBL Customer = new CustomerBL();
            CustomerModel.customerGedList = Customer.GetCustomerListHavingContacts(ProjectId, User_Id);
            ViewBag.Customer = new SelectList(CustomerModel.customerGedList, "CustomerId", "CustomerName");
            return PartialView(model);
        }

        public ActionResult AddParticipantsDetail(string PType, string PID, int MOMID, bool FYI)
        {

            if (PID.ToString() == "0" || PID.ToString() == "")
            {
                return View();
            }
            ProjectBL objBL = new ProjectBL();
            string User_Id = User.Identity.GetUserId();
            string[] selitems = PID.Split(',');

            int errcode = objBL.Saveparticipatns(PType, selitems, MOMID, User_Id, FYI);
            if (errcode == 500001 || errcode == 500002)
            {
                Session["Participant"] = null;
                return RedirectToAction("CreateProjectTAskMOM", new { MOMID = MOMID, tabIndex = 1 });
            }
            else if (errcode == 500000)
            {

                Session["Participant"] = "This Participant already added";
                return RedirectToAction("AddParticipants", new { MOMID = MOMID });
            }
            else
            {
                return RedirectToAction("CreateProjectTAskMOM", new { MOMID = MOMID, tabIndex = 1 });
            }

        }
        public JsonResult GettaskByProject(int ProjectId)
        {
            ProjectTaskMoM MOMModel = new ProjectTaskMoM();
            try
            {
                ProjectBL objBL = new ProjectBL();
                MOMModel.LstProjTask = new List<ProjTaskModel>();
                MOMModel.LstMoMParticipant = new List<MoMParticipantModel>();
                MOMModel.LstProjTask = objBL.ProjTaskList(ProjectId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(MOMModel.LstProjTask, "TaskID", "TaskName"));
        }
        public JsonResult GetContactBYCustomerId(int CustomerId)
        {
            List<CustomerContact> ContactList = new List<CustomerContact>();

            try
            {
                ProjectBL objBL = new ProjectBL();

                ContactList = objBL.CustomerContactByCustomerId(CustomerId);
                ViewBag.custcontact = new SelectList(ContactList, "CustomerContactId", "ContactName");


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(ContactList, "CustomerContactId", "ContactName"));
        }
        public JsonResult GetContactBYVendorId(int VendorId)
        {
            List<VendorContact> VendoeContactList = new List<VendorContact>();

            try
            {
                ProjectBL objBL = new ProjectBL();

                VendoeContactList = objBL.VendorContactByVendorId(VendorId);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(VendoeContactList, "VendorContactId", "ContactName"));
        }
        public ActionResult MOMAttachment(int MOMID)
        {
            MoMAttachmentModel objmodel = new MoMAttachmentModel();
            objmodel.MOMId = MOMID;
            return PartialView(objmodel);
        }
        [HttpPost]
        public ActionResult MOMAttachment(HttpPostedFileBase file, FormCollection fc)
        {
            MoMAttachmentModel Attachmentmodel = new MoMAttachmentModel();
            ProjectBL objBL = new ProjectBL();
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");
            string fileName = "";

            if (file != null)
            {
                fileName = System.IO.Path.GetFileName(file.FileName);
                if (file.ContentLength > 0)
                {
                    try
                    {
                        string[] FileSplit = fileName.Split('.');
                        string FileEx = FileSplit[1].ToString();
                        String guid = Guid.NewGuid().ToString();
                        string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                        string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        string FileName = Convert.ToString("/MOMAttachment/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
                        string localPath = Server.MapPath("~/App_Data/uploads");
                        if (!System.IO.Directory.Exists(localPath))
                        {
                            System.IO.Directory.CreateDirectory(localPath);
                        }
                        file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName));
                        if (ftpServer.Trim().Length > 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {

                                FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                                requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                                int bufferLength = 2048;
                                byte[] buffer = new byte[file.InputStream.Length];
                                Stream uploadStream = requestFTPUploader.GetRequestStream();
                                int contentLength = file.InputStream.Read(buffer, 0, Convert.ToInt32(file.InputStream.Length));
                                while (contentLength != 0)
                                {
                                    uploadStream.Write(buffer, 0, contentLength);
                                    contentLength = file.InputStream.Read(buffer, 0, bufferLength);
                                }
                                uploadStream.Close();
                                //fileStream.Close();
                                requestFTPUploader = null;
                                Attachmentmodel.FileName = FileName;
                                break;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.LogException(ex);
                    }
                }
            }
            Attachmentmodel.Description = fc["Description"].ToString();
            Attachmentmodel.MOMId = Convert.ToInt32(fc["MOMId"].ToString());
            int errcode = objBL.SaveMOMAttachment(Attachmentmodel.FileName, Attachmentmodel.Description, Attachmentmodel.MOMId);
            if (errcode == 500001 || errcode == 500002)
            {

                return RedirectToAction("CreateProjectTAskMOM", new { MOMID = Attachmentmodel.MOMId, tabIndex = 0 });
            }
            else
            {
                if (file.ContentLength > 0)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        try
                        {
                            FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(ftpServer + fileName);
                            requestFileDelete.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                            requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                            FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                            requestFileDelete = null;

                            break;
                        }
                        catch (Exception ex)
                        {
                            if (x > 2)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            return View();
        }

        public ActionResult Download(int MOMID, string Filename)
        {
            try
            {
                String FTPServer = Common.GetConfigValue("FTP");
                String FTPUserId = Common.GetConfigValue("FTPUid");
                String FTPPwd = Common.GetConfigValue("FTPPWD");

                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + Filename);
                requestFileDownload.UseBinary = true;
                requestFileDownload.KeepAlive = false;
                requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                byte[] OPData = ReadFully(responseStream);
                responseStream.Close();

                Response.AddHeader("content-disposition", "attachment; filename=" + Filename);
                Response.ContentType = "application/zip";
                Response.BinaryWrite(OPData);
                Response.End();
            }
            catch (Exception e)
            {
                throw e;
            }

            return RedirectToAction("CreateProjectTAskMOM", new { MOMID = MOMID, tabIndex = 0 });
        }
        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public ActionResult DeleteParticipants(int MOMID, int ParticipantId, string ParticipantsType)
        {

            ProjectBL objBL = new ProjectBL();

            int errcode = objBL.DeleteParticipants(MOMID, ParticipantId, ParticipantsType);
            if (errcode == 500003)
            {

                return RedirectToAction("CreateProjectTAskMOM", new { MOMID = MOMID, tabIndex = 0 });
            }
            return RedirectToAction("CreateProjectTAskMOM", new { MOMID = MOMID, tabIndex = 0 });
        }

        public ActionResult SendMom(int MomId)
        {
            ProjectTaskMoM MOMModel = new ProjectTaskMoM();
            DataSet ds = new DataSet();
            try
            {
                TempData["MailDetail"] = null;
                ArrayList MailDetail = new ArrayList();
                ProjectBL BLObj = new ProjectBL();
                EnquiryModel EnqDetail = new EnquiryModel();
                string User_Id = User.Identity.GetUserId();
                EnqDetail.lstMail = new List<EnqSendVendorModel>();
                EnquiryBL Enqbl = new EnquiryBL();
                EnqDetail = Enqbl.GetCustEmailDeatiltoSendQuotation(0, "", User_Id);
                if (EnqDetail.lstMail[0].EmailUserName != "" && EnqDetail.lstMail[0].EmailPassword != "" && EnqDetail.lstMail[0].EmailServer != "" && EnqDetail.lstMail[0].EmailPort != "")
                {
                    MailDetail.Add(EnqDetail.lstMail[0].EmailServer);
                    MailDetail.Add(EnqDetail.lstMail[0].EmailUserName);
                    MailDetail.Add(EnqDetail.lstMail[0].EmailPassword);
                    MailDetail.Add(EnqDetail.lstMail[0].EmailPort);

                }

                TempData["MailDetail"] = MailDetail;
                ds = BLObj.GetMOMParticipendsDetailList(MomId);

                string Email = "";
                string ccMail = "";
                Boolean NonEmployeeFlag = false;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["FYI"].ToString() != "True")
                    {
                        if (dr["Participantemail"].ToString() != "")
                        {
                            if (Email != "")
                            {
                                Email = Email + ",";
                            }

                            Email = Email + dr["Participantemail"].ToString();
                            if (dr["ParticipantType"].ToString() != "Employee")
                                NonEmployeeFlag = true;
                        }
                    }
                    else
                    {
                        if (dr["Participantemail"].ToString() != "")
                        {
                            if (ccMail != "")
                            {
                                ccMail = ccMail + ",";
                            }

                            ccMail = ccMail + dr["Participantemail"].ToString();
                            if (dr["ParticipantType"].ToString() != "Employee")
                                NonEmployeeFlag = true;
                        }
                    }

                }
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    if (dr["Resource"].ToString() != "")
                    {
                        if (Email != "")
                        {
                            Email = Email + ",";
                        }

                        Email = Email + dr["Resource"].ToString();
                    }
                }
                Email = Email.Trim();
                List<string> uniques = Email.Split(',').Reverse().Distinct().Reverse().ToList();
                Email = string.Join(",", uniques);

                if (NonEmployeeFlag == false)
                    return RedirectToAction("SendMomWithInternalDescription", new { MOMId = MomId });
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> MOM Participants List>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                string txt = string.Empty;
                txt +=
                  "Details List" +
                   "<br /> <br />";
                txt += "<table border='1'><tr style='background-color:#000000;color:white'><th>Participant Type</th><th>Participant Name</th></tr>";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    if (dr["FYI"].ToString() != "True")
                    {
                        txt += "<tr><td>" + dr["ParticipantType"].ToString() + "</td>";
                        txt += "<td>" + dr["ParticipantName"].ToString() + "</td></tr>";
                    }

                }
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>End>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //---------------------------------------------MOMActionPoint List With Resource------------------------------------------------------

                txt += "</table>";

                MOMModel.ActionPointList = new List<MOMActionPoint>();

                MOMModel.ActionPointList = BLObj.MOMActionPointList(MomId);
                string body = string.Empty;
                body +=
                    "Details List" +
                     "<br /> <br />";
                body += "<table border='1'><tr style='background-color:#000000;color:white'><th>Description</th><th>Resource</th><th>Status</th><th>DueDate</th></tr>";
                foreach (var item in MOMModel.ActionPointList)
                {
                    body += "<tr><td>" + item.ActionDescription + "</td>";
                    body += "<td>" + item.Resource + "</td>";
                    body += "<td>" + item.StatusShortCode + "</td>";
                    body += "<td>" + item.DueDate + "</td></tr>";
                }
                body += "</table>";
                //--------------------------------------------------------------------End--------------------------------------------------------------
                if (Email.ToString() != "")
                {
                    MOMModel = BLObj.GetSelectedProjectTaskMOM(MomId);
                    string Subject = "MOM : " + MOMModel.Title.ToString();
                    string MailBody = "<html>" +
                      "<head>" +
                       "<meta http-equiv= " + "Content-Type" + "content= " + "text/html; charset=UTF-8" + ">" +
                       "<meta name=" + "generator" + " content=" + "SautinSoft.RtfToHtml.dll" + ">" +
                       "<title>" + "Leave Mail" + "</title>" +
                       "<style type=" + "text/css>" +
                      ".st1{font-family:Calibri;font-size:11pt;color:#000000;font-weight:bold}" +
                      ".st2{font-family:Calibri;font-size:11pt;color:#000000;}" +
                      "</style>" +
                       "</head>" +
                       "<body>" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Title: " + "</b>" + MOMModel.Title + "</span>" + "</div>" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " MOMType: " + "</b>" + MOMModel.MOMTypeKey + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Project Name : " + "</b>" + MOMModel.ProjectName + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Task : " + "</b>" + MOMModel.TaskName + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Date : " + "</b>" + MOMModel.MOMDate.ToLongDateString() + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " MOM Owner : " + "</b>" + MOMModel.Employee + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Created On : " + "</b>" + MOMModel.CreatedDate.ToLongDateString() + " - " + MOMModel.CreatedDate.ToLongTimeString() + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Created By : " + "</b>" + MOMModel.CreatedByName + "</span>" + "</div>" +

                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Last Modified On : " + "</b>" + MOMModel.ModifiedDate.ToLongDateString() + " - " + MOMModel.ModifiedDate.ToLongTimeString() + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Last Modified By : " + "</b>" + MOMModel.ModifiedByName + "</span>" + "</div>" +
                        "<br />" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:orange'>" + " Participants : " + "</b>" + "</span>" + txt + "</div>" + "<br />" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:orange'>" + " MOM Action Point : " + "</b>" + "</span>" + body + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "</br></br><span class=st1>" + "<b style='color:red'>" + "Description : " + "</b>" + "</span>" + "</div>" +

                       MOMModel.Description +

                       "</body>" +
                       "</html>";

                    ArrayList attachments = new ArrayList();
                    List<MoMAttachmentModel> lstatModel = BLObj.MOMAttachmentList(MomId);
                    Stream responseStream = null;
                    foreach (MoMAttachmentModel AttachmentModel in lstatModel)
                    {

                        String FTPServer = Common.GetConfigValue("FTP");
                        String FTPUserId = Common.GetConfigValue("FTPUid");
                        String FTPPwd = Common.GetConfigValue("FTPPWD");

                        FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + AttachmentModel.FileName);
                        requestFileDownload.UseBinary = true;
                        requestFileDownload.KeepAlive = false;
                        requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                        requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                        FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                        responseStream = responseFileDownload.GetResponseStream();
                        System.Net.Mail.Attachment at = new System.Net.Mail.Attachment(responseStream, AttachmentModel.FileName);
                        attachments.Add(at);

                    }

                    //FileStream objFileStream = new FileStream(saveLocation + "\\" + "Ajit_Photo_2522016_1053_7df16aff-f9f2-4f58-83d6-3b6d44703fb0.JPG", FileMode.Create, FileAccess.Write);
                    //objFileStream.Write(OPData, 0, OPData.Length);
                    //objFileStream.Close();



                    //Response.AddHeader("content-disposition", "attachment; filename=" + Filename);
                    //Response.ContentType = "application/zip";
                    //Response.BinaryWrite(OPData);
                    //Response.End();



                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            //int mail = Utility.Common.SendMail(Subject, MailBody, MailDetail, Email, ccMail, attachments);
                            int mail1 = Utility.Common.SendMailDynamic(Subject, MailBody, "", Email, ccMail, attachments, EnqDetail.lstMail[0].EmailServer.ToString(), EnqDetail.lstMail[0].EmailUserName.ToString(), EnqDetail.lstMail[0].EmailPassword.ToString(), EnqDetail.lstMail[0].EmailPort.ToString(), "", false);
                            break;

                        }
                        catch (Exception ex)
                        {
                            if (i == 2)
                            {
                                throw ex;
                            }
                        }
                    }
                    responseStream.Close();

                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            if (MOMModel.LocalDescription.Length > 0)
                return RedirectToAction("SendMomWithInternalDescription", new { MOMId = MomId });
            else
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["ManagementView"].ToString() != "")
                    {
                        return RedirectToAction("SendMOMWithManagementView", new { MOMId = MomId });
                        break;
                    }
                }

            return RedirectToAction("CreateProjectTAskMOM", new { MOMId = MomId, tabIndex = 0 });
        }
        public ActionResult SendMomWithInternalDescription(int MomId)
        {
            ProjectTaskMoM MOMModel = new ProjectTaskMoM();
            DataSet ds = new DataSet();
            ArrayList Maildetail = new ArrayList();
            try
            {

                if (TempData["MailDetail"] != null)
                {

                    Maildetail = (ArrayList)TempData["MailDetail"];

                }
                ProjectBL BLObj = new ProjectBL();
                // List<string> mailInfo = new List<string>();

                ds = BLObj.GetMOMParticipendsDetailList(MomId);
                string Email = "";
                string ccmail = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["FYI"].ToString() != "True")
                    {

                        if (dr["Participantemail"].ToString() != "" && dr["ParticipantType"].ToString() == "Employee")
                        {
                            Email = Email + dr["Participantemail"].ToString();
                            if (Email != "")
                            {
                                Email = Email + ",";
                            }
                        }
                    }
                    else
                    {

                        if (dr["Participantemail"].ToString() != "" && dr["ParticipantType"].ToString() == "Employee")
                        {
                            if (ccmail != "")
                            {
                                ccmail = ccmail + ",";
                            }
                            ccmail = ccmail + dr["Participantemail"].ToString();

                        }
                    }

                }
                foreach (DataRow dr in ds.Tables[1].Rows)
                {

                    if (dr["Resource"].ToString() != "")
                        Email = Email + dr["Resource"].ToString();
                    if (Email != "")
                    {
                        Email = Email + ",";
                    }

                }
                Email = Email.TrimEnd(',');
                Email = Email.Trim();
                List<string> uniques = Email.Split(',').Reverse().Distinct().Reverse().ToList();
                Email = string.Join(",", uniques);
                Email.Trim();
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> MOM Participants List>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                string txt = string.Empty;
                txt +=
                  "Details List" +
                   "<br /> <br />";
                txt += "<table border='1'><tr style='background-color:#000000;color:white'><th>Participant Type</th><th>Participant Name</th></tr>";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["FYI"].ToString() != "True")
                    {
                        txt += "<tr><td>" + dr["ParticipantType"].ToString() + "</td>";
                        txt += "<td>" + dr["ParticipantName"].ToString() + "</td></tr>";
                    }

                }
                txt += "</table>";
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> End>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //---------------------------------------------MOMActionPoint List With Resource------------------------------------------------------


                MOMModel.ActionPointList = new List<MOMActionPoint>();

                MOMModel.ActionPointList = BLObj.MOMActionPointList(MomId);
                string body = string.Empty;
                body +=
                    "Details List" +
                     "<br /> <br />";
                body += "<table border='1'><tr style='background-color:#000000;color:white'><th>Description</th><th>Resource</th><th>Status</th><th>DueDate</th></tr>";
                foreach (var item in MOMModel.ActionPointList)
                {
                    body += "<tr><td>" + item.ActionDescription + "</td>";
                    body += "<td>" + item.Resource + "</td>";
                    body += "<td>" + item.StatusShortCode + "</td>";
                    body += "<td>" + item.DueDate + "</td></tr>";
                }
                body += "</table>";
                //--------------------------------------------------------------------End--------------------------------------------------------------
                if (Email.ToString() != "")
                {
                    MOMModel = BLObj.GetSelectedProjectTaskMOM(MomId);
                    string Subject = "MOM : " + MOMModel.Title.ToString();
                    string MailBody = "<html>" +
                      "<head>" +
                       "<meta http-equiv= " + "Content-Type" + "content= " + "text/html; charset=UTF-8" + ">" +
                       "<meta name=" + "generator" + " content=" + "SautinSoft.RtfToHtml.dll" + ">" +
                       "<title>" + "Leave Mail" + "</title>" +
                       "<style type=" + "text/css>" +
                      ".st1{font-family:Calibri;font-size:11pt;color:#000000;font-weight:bold}" +
                      ".st2{font-family:Calibri;font-size:11pt;color:#000000;}" +
                      "</style>" +
                       "</head>" +
                       "<body>" +

                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Title : " + "</b>" + MOMModel.Title + "</span>" + "</div>" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " MOM Type: " + "</b>" + MOMModel.MOMTypeKey + "</span>" + "</div>" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Project Name : " + "</b>" + MOMModel.ProjectName + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Task : " + "</b>" + MOMModel.TaskName + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Date : " + "</b>" + MOMModel.MOMDate.ToLongDateString() + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " MOM Owner : " + "</b>" + MOMModel.Employee + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Created On : " + "</b>" + MOMModel.CreatedDate.ToLongDateString() + " - " + MOMModel.CreatedDate.ToLongTimeString() + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Created By : " + "</b>" + MOMModel.CreatedByName + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Last Modified On : " + "</b>" + MOMModel.ModifiedDate.ToLongDateString() + " - " + MOMModel.ModifiedDate.ToLongTimeString() + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Last Modified By : " + "</b>" + MOMModel.ModifiedByName + "</span>" + "</div>" + "</div>" + "<br />" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Participants : " + "</b>" + txt + "</span>" + "</div>" + "<br />" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " MOM Action Point : " + "</b>" + body + "</span>" + "</div>";
                    if (MOMModel.Description.Length > 0)
                        MailBody = MailBody + "<div style=" + "background-color:#F2EFFB;>" + "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "</br></br><span class=st1>" + "<b style='color:red'>" + "Description : " + "</b>" + "</span>" + MOMModel.Description + "</div>" + "</div>";
                    if (MOMModel.LocalDescription.Length > 0)
                        MailBody = MailBody + "<div style=" + "background-color:#FBEFF2;>" + "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "</br></br><span class=st1>" + "<b style='color:red'>" + "Internal Description : " + "</b>" + "</span>" + MOMModel.LocalDescription + "</div>" + "</div>";

                    MailBody = MailBody + "</body>" +
                       "</html>";

                    ArrayList attachments = new ArrayList();
                    List<MoMAttachmentModel> lstatModel = BLObj.MOMAttachmentList(MomId);
                    Stream responseStream = null;
                    foreach (MoMAttachmentModel AttachmentModel in lstatModel)
                    {

                        String FTPServer = Common.GetConfigValue("FTP");
                        String FTPUserId = Common.GetConfigValue("FTPUid");
                        String FTPPwd = Common.GetConfigValue("FTPPWD");

                        FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + AttachmentModel.FileName);
                        requestFileDownload.UseBinary = true;
                        requestFileDownload.KeepAlive = false;
                        requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                        requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                        FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                        responseStream = responseFileDownload.GetResponseStream();
                        System.Net.Mail.Attachment at = new System.Net.Mail.Attachment(responseStream, AttachmentModel.FileName);
                        attachments.Add(at);

                    }

                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            string Server = "";
                            string Username = "";
                            string EmailPassword = "";
                            string EmailPort = "";
                            if (Maildetail.Count > 0)
                            {
                                Server = Maildetail[0].ToString();
                                Username = Maildetail[1].ToString();
                                EmailPassword = Maildetail[2].ToString();
                                EmailPort = Maildetail[3].ToString();
                            }
                            int mail = Utility.Common.SendMailDynamic(Subject, MailBody, "", Email, ccmail, attachments, Server.ToString(), Username.ToString(), EmailPassword.ToString(), EmailPort.ToString(), "", false);
                            //int mail = Utility.Common.SendMail(Subject, MailBody,  FileName, Email, ccmail, attachments);
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (i == 2)
                            {
                                throw ex;
                            }
                        }
                    }
                    responseStream.Close();

                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["ManagementView"].ToString() != "")
                    {
                        return RedirectToAction("SendMOMWithManagementView", new { MOMId = MomId });
                        break;
                    }
                }

            }
            return RedirectToAction("CreateProjectTAskMOM", new { MOMId = MomId, tabIndex = 0 });
        }
        public ActionResult SendMOMWithManagementView(int MomId)
        {
            ProjectTaskMoM MOMModel = new ProjectTaskMoM();
            ArrayList Maildetail = new ArrayList();
            try
            {
                if (TempData["MailDetail"] != null)
                {
                    Maildetail = (ArrayList)TempData["MailDetail"];
                }
                ProjectBL BLObj = new ProjectBL();
                DataSet ds = new DataSet();
                ds = BLObj.GetMOMParticipendsDetailList(MomId);
                string Email = "";
                string ccmail = "";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (ds.Tables[0].Rows[0]["MOMCreater"].ToString() != "" && Email.ToString() == "")
                    {
                        Email = dr["MOMCreater"].ToString();
                    }
                    if (dr["FYI"].ToString() != "True")
                    {
                        if (dr["ManagementView"].ToString() != "")
                        {

                            if (dr["Participantemail"].ToString() != "" && dr["ParticipantType"].ToString() == "Employee")
                            {
                                if (Email != "")
                                {
                                    Email = Email + ",";
                                }
                                Email = Email + dr["Participantemail"].ToString();

                            }
                        }
                    }
                    else
                    {
                        if (dr["ManagementView"].ToString() != "")
                        {

                            if (dr["Participantemail"].ToString() != "" && dr["ParticipantType"].ToString() == "Employee")
                            {
                                if (ccmail != "")
                                {
                                    ccmail = ccmail + ",";
                                }
                                ccmail = ccmail + dr["Participantemail"].ToString();

                            }
                        }
                    }

                }

                Email = Email.Trim();
                List<string> uniques = Email.Split(',').Reverse().Distinct().Reverse().ToList();
                Email = string.Join(",", uniques);
                Email.Trim();
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> MOM Participants List>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                string txt = string.Empty;
                txt +=
                  "Details List" +
                   "<br /> <br />";
                txt += "<table border='1'><tr style='background-color:#000000;color:white'><th>Participant Type</th><th>Participant Name</th></tr>";
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["FYI"].ToString() != "True")
                    {
                        txt += "<tr><td>" + dr["ParticipantType"].ToString() + "</td>";
                        txt += "<td>" + dr["ParticipantName"].ToString() + "</td></tr>";
                    }

                }
                txt += "</table>";
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> End>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                //---------------------------------------------MOMActionPoint List With Resource------------------------------------------------------


                MOMModel.ActionPointList = new List<MOMActionPoint>();

                MOMModel.ActionPointList = BLObj.MOMActionPointList(MomId);
                string body = string.Empty;
                body +=
                    "Details List" +
                     "<br /> <br />";
                body += "<table border='1'><tr style='background-color:#000000;color:white'><th>Description</th><th>Resource</th><th>Status</th><th>DueDate</th></tr>";
                foreach (var item in MOMModel.ActionPointList)
                {
                    body += "<tr><td>" + item.ActionDescription + "</td>";
                    body += "<td>" + item.Resource + "</td>";
                    body += "<td>" + item.StatusShortCode + "</td>";
                    body += "<td>" + item.DueDate + "</td></tr>";
                }
                body += "</table>";
                //--------------------------------------------------------------------End--------------------------------------------------------------
                if (Email.ToString() != "" || ccmail.ToString() != "")
                {
                    MOMModel = BLObj.GetSelectedProjectTaskMOM(MomId);
                    string Subject = "MOM : " + MOMModel.Title.ToString();
                    string MailBody = "<html>" +
                      "<head>" +
                       "<meta http-equiv= " + "Content-Type" + "content= " + "text/html; charset=UTF-8" + ">" +
                       "<meta name=" + "generator" + " content=" + "SautinSoft.RtfToHtml.dll" + ">" +
                       "<title>" + "Leave Mail" + "</title>" +
                       "<style type=" + "text/css>" +
                      ".st1{font-family:Calibri;font-size:11pt;color:#000000;font-weight:bold}" +
                      ".st2{font-family:Calibri;font-size:11pt;color:#000000;}" +
                      "</style>" +
                       "</head>" +
                       "<body>" +

                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Title : " + "</b>" + MOMModel.Title + "</span>" + "</div>" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " MOM Type: " + "</b>" + MOMModel.MOMTypeKey + "</span>" + "</div>" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Project Name : " + "</b>" + MOMModel.ProjectName + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Task : " + "</b>" + MOMModel.TaskName + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Date : " + "</b>" + MOMModel.MOMDate.ToLongDateString() + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " MOM Owner : " + "</b>" + MOMModel.Employee + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Created On : " + "</b>" + MOMModel.CreatedDate.ToLongDateString() + " - " + MOMModel.CreatedDate.ToLongTimeString() + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Created By : " + "</b>" + MOMModel.CreatedByName + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Last Modified On : " + "</b>" + MOMModel.ModifiedDate.ToLongDateString() + " - " + MOMModel.ModifiedDate.ToLongTimeString() + "</span>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Last Modified By : " + "</b>" + MOMModel.ModifiedByName + "</span>" + "</div>" + "</div>" +
                        "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Last Modified By : " + "</b>" + MOMModel.ModifiedByName + "</span>" + "</div>" + "</div>" + "<br/>" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Participants : " + "</b>" + txt + "</span>" + "</div>" + "<br/>" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " MOM Action Point : " + "</b>" + body + "</span>" + "</div>";
                    if (MOMModel.Description.Length > 0)
                        MailBody = MailBody + "<div style=" + "background-color:#F2EFFB;>" + "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "</br></br><span class=st1>" + "<b style='color:red' class = 'btn btn-info glyphicon glyphicon-send'>" + "Description : " + "</b>" + "</span>" + MOMModel.Description + "</div>" + "</div>";
                    if (MOMModel.LocalDescription.Length > 0)
                        MailBody = MailBody + "<div style=" + "background-color:#FBEFF2;>" + "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "</br></br><span class=st1>" + "<b style='color:red' class = 'btn btn-info glyphicon glyphicon-send'>" + "Internal Description : " + "</b>" + "</span>" + MOMModel.LocalDescription + "</div>" + "</div>";
                    if (MOMModel.ManageMentView.Length > 0)
                        MailBody = MailBody + "<div style=" + "background-color:#F5B7B1;>" + "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "</br></br><span class=st1>" + "<b style='color:red' class = 'btn btn-info glyphicon glyphicon-send'>" + "ManagementView : " + "</b>" + "</span>" + MOMModel.ManageMentView + "</div>" + "</div>";

                    MailBody = MailBody + "</body>" +
                       "</html>";

                    ArrayList attachments = new ArrayList();
                    List<MoMAttachmentModel> lstatModel = BLObj.MOMAttachmentList(MomId);
                    Stream responseStream = null;
                    foreach (MoMAttachmentModel AttachmentModel in lstatModel)
                    {

                        String FTPServer = Common.GetConfigValue("FTP");
                        String FTPUserId = Common.GetConfigValue("FTPUid");
                        String FTPPwd = Common.GetConfigValue("FTPPWD");

                        FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + AttachmentModel.FileName);
                        requestFileDownload.UseBinary = true;
                        requestFileDownload.KeepAlive = false;
                        requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                        requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                        FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                        responseStream = responseFileDownload.GetResponseStream();
                        System.Net.Mail.Attachment at = new System.Net.Mail.Attachment(responseStream, AttachmentModel.FileName);
                        attachments.Add(at);

                    }

                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            string Server = "";
                            string Username = "";
                            string EmailPassword = "";
                            string EmailPort = "";
                            if (Maildetail.Count > 0)
                            {
                                Server = Maildetail[0].ToString();
                                Username = Maildetail[1].ToString();
                                EmailPassword = Maildetail[2].ToString();
                                EmailPort = Maildetail[3].ToString();
                            }
                            int mail = Utility.Common.SendMailDynamic(Subject, MailBody, "", Email, ccmail, attachments, Server.ToString(), Username.ToString(), EmailPassword.ToString(), EmailPort.ToString(), "", false);
                            //  int mail = Utility.Common.SendMail(Subject, MailBody, FileName, Email, ccmail, attachments);
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (i == 2)
                            {
                                throw ex;
                            }
                        }
                    }
                    responseStream.Close();

                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return RedirectToAction("CreateProjectTAskMOM", new { MOMId = MomId, tabIndex = 0 });
        }
        public ActionResult DeleteUser(string ParticipantType, string UserId, int ActionPointId , int MOMId)
        {
            int UID = Convert.ToInt32(UserId);
            string Tr = "";
            //var ParticipantType = "";
            List<int> optionList = new List<int>();
            if (TempData["UserID"] == null)
            {
                optionList.Add(UID);
            }
            else
            {
                optionList = TempData["UserID"] as List<int>;
                optionList.Add(UID);
            }
            TempData["UserID"] = optionList;
            string ptype = ParticipantType.Replace("\n", String.Empty).Trim();
            MOMActionPoint model = new MOMActionPoint();
            model.MOMId = MOMId;
            ProjectBL objBL = new ProjectBL();
            int errcode = objBL.DeleteMomActionPointParticipants(ActionPointId, ptype, UID);


            return RedirectToAction("CreateProjectTAskMOM", new { MOMId = MOMId, tabIndex = 0 });
        }
        public ActionResult MOMActionPoint(int MOMId, int ActionPointId, int ProjectId)
        {
            MOMActionPoint model = new MOMActionPoint();
           
            ProjectBL objBL = new ProjectBL();
            string User_Id = User.Identity.GetUserId();
            List<MOMActionPointUser> Userlist = new List<MOMActionPointUser>();
            Userlist = objBL.MOMActionUserList();

            if (MOMId > 0)
            {
                model = objBL.GetselectedMOMActionPoint(ActionPointId);
                model.LstActionPointParticipant = objBL.GetselectedMOMActionPointParticipant(ActionPointId);
                model.MOMId = MOMId;
            }
            ViewBag.UserList = new SelectList(Userlist, "UserId", "UserName");
            List<MOMStatusCodes> StatusList = new List<MOMStatusCodes>();
            StatusList = objBL.MomStatusList();
            ViewBag.Status = new SelectList(StatusList, "Status", "StatusShortCode");

            VendorListModel VendorModel = new VendorListModel();
            VendorBL VendorBL = new VendorBL();
            VendorModel.vendorGedList = VendorBL.GetVendorListHavingContacts(ProjectId, User_Id);
            ViewBag.Vendor = new SelectList(VendorModel.vendorGedList, "VendorId", "VendorName");

            CustomerListModel CustomerModel = new CustomerListModel();
            CustomerBL Customer = new CustomerBL();
            CustomerModel.customerGedList = Customer.GetCustomerListHavingContacts(ProjectId, User_Id);
            ViewBag.Customer = new SelectList(CustomerModel.customerGedList, "CustomerId", "CustomerName");
            if(ActionPointId > 0)
            {
                return PartialView(model);
            }
            return PartialView(model);
        }


        public ActionResult MOMActionPointSave(string Description, int Status, int ActionPointId, int MOMId, string PType, string PID, DateTime Duedate, string Comment)
        {
            string User_Id = User.Identity.GetUserId();
            MOMActionPoint model = new MOMActionPoint();
            string[] selitemsUpdated = PID.Split(',');
            List<string> lst = new List<string>();
            foreach (var i in selitemsUpdated)
            {
                lst.Add(i);
            }
            ProjectBL objBL = new ProjectBL();
            model.MOMId = MOMId;
            if (ActionPointId != 0)
            {
                model.ActionPointId = ActionPointId; ;
            }
            else
            {
                model.ActionPointId = 0;
            }
            model.ActionDescription = Description;
            model.Comment = Comment;
            model.Status = Status;
            if (Duedate.ToString() == "01/01/0001 12:00:00 AM" || Duedate.ToString() == "1/1/0001 12:00:00 AM")
            {
                model.DueDate = null;
            }
            else
            {
                model.DueDate = Duedate;
            }

            List<int> optionList = new List<int>();
            if (TempData["UserID"] != null)
            {
                optionList = TempData["UserID"] as List<int>;

                foreach (var item in optionList)
                {

                    lst.Remove(item.ToString());

                }
            }
            TempData["UserID"] = null;

            selitemsUpdated = lst.ToArray();

            int Errorcode = objBL.SaveMOMActionPointList(model, User_Id, selitemsUpdated, PType);
            if (Errorcode != 0)
            {
                return RedirectToAction("CreateProjectTAskMOM", new { MOMId = MOMId, tabIndex = 0 });
            }
            return View();
        }
        #endregion Project Task MOM

        #region Project Expenses
        public ActionResult ProjectExpensesList()
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/ProjectExpensesList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                ProjectBL objBL = new ProjectBL();
                ProjectExpensesModel Model = new ProjectExpensesModel();
                Model.LstPrjectExp = objBL.TMProjectExpensesList(User_Id);

                return View(Model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ProjectPaymentStatusList()
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/ProjectPaymentStatusList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                ProjectBL objBL = new ProjectBL();
                List<ProjectExpensesModel> Lst = new List<ProjectExpensesModel>();
                ProjectExpensesModel Model = new ProjectExpensesModel();
                Model.LstPrjectExp = objBL.PaymentStatusList(User_Id);
                Session["ListDetails"] = null;
                Session["ListDetails"] = Model.LstPrjectExp;
                ViewBag.PmtStatus = Model.LstPrjectExp;

                return View(Model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult PaymentList(int RefId)
        {

            string User_Id = User.Identity.GetUserId();
            ProjectBL objBL = new ProjectBL();
            List<PaymentExpenseStatus> Lst = new List<PaymentExpenseStatus>();
            Lst = objBL.TmGetselectedPaymentList(RefId);
            try
            {
                ProjectExpensesModel Model = new ProjectExpensesModel();
                Model.LstPrjectExp = Session["ListDetails"] as List<ProjectExpensesModel>;
                foreach (var item in Model.LstPrjectExp)
                {
                    if (Convert.ToInt32(item.ExpenseId) == Convert.ToInt32(RefId))
                    {
                        ViewBag.Amount = item.Amount;
                        ViewBag.Employee = item.Employee;
                        ViewBag.RefId = item.ExpenseId;
                        ViewBag.EmpId = item.EmpId;

                        break;
                    }
                }
                if (Lst.Count > 0)
                {
                    foreach (var item in Lst)
                    {
                        ViewBag.TotalPaid = item.TotalPaid;
                        break;
                    }
                }
                else
                {
                    ViewBag.TotalPaid = 0;
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View(Lst);
        }
        public ActionResult DeletePaymentDetails(int Id, int RefId, string Remark)
        {
            ProjectBL objBL = new ProjectBL();
            int Errorcode = objBL.DeletePaymentDetails(Id, Remark);
            return RedirectToAction("PaymentList", new { RefId = RefId });
        }
        [HttpGet]
        public ActionResult ProjectPayMentStatusDetail(int Id, float Amount, string EmpName, int RefId, int EmpId, float TotalPaid)
        {
            PaymentExpenseStatus model = new PaymentExpenseStatus();
            ProjectBL objBL = new ProjectBL();
            if (Id > 0)
                model = objBL.TmGetselectedPayment(Id);
            model.Amount = Amount;
            ViewBag.Amount = Amount;
            model.Employee = EmpName;
            model.RefId = RefId;
            model.EmpId = EmpId;
            model.TotalPaid = TotalPaid;
            return View(model);
        }
        [HttpPost]
        public ActionResult ProjectPayMentStatusDetail(FormCollection fc)
        {
            PaymentExpenseStatus model = new PaymentExpenseStatus();
            try
            {
                ProjectBL objBL = new ProjectBL();
                string User_Id = User.Identity.GetUserId();
                model.PaymentId = Convert.ToInt32(fc["PaymentId"]);
                model.EmpId = Convert.ToInt32(fc["EmpId"]);
                model.RefId = Convert.ToInt32(fc["RefId"]);
                TempData["RefId"] = model.RefId;
                model.Amount = Convert.ToDouble(fc["NewAmount"]);
                model.PaymentType = Convert.ToString(fc["PaymentType"]);
                model.Remark = Convert.ToString(fc["Remark"]);
                int ErrorCode = objBL.SavePaymentDetails(model, User_Id);
                if (ErrorCode != null)
                {

                    return RedirectToAction("PaymentList", new { RefId = model.RefId });
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View();
        }
        public ActionResult CreateProjectExpenses(int ExpenseId)
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }
            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/CreateProjectExpenses?ExpenseId=0");
            if (FindFlag)
            {
                ProjectExpensesModel ExpeModel = new ProjectExpensesModel();
                ProjectBL objBL = new ProjectBL();
                string User_Id = User.Identity.GetUserId();
                List<ProjectModel> lstproj = objBL.GetProjectList(User_Id);          
                List<ExpenseTypeModel> ExptypeList = new List<ExpenseTypeModel>();
                ExptypeList = objBL.GetExpenseTypeList();
                ViewBag.ExpnsType = new SelectList(ExptypeList, "ExpTypeId", "ExpenceType");
                ViewBag.project = new SelectList(lstproj, "ProjectId", "ProjectName");
                ExpeModel.LstProjTask = new List<ProjTaskModel>();
                ExpeModel.LstExpDoc = new List<ExpensesDocModel>();
                if (ExpenseId > 0)
                {
                    ProjectBL BLObj = new ProjectBL();
                    ExpeModel = BLObj.TMGetSelectedProjectExpensesList(ExpenseId);
                    ExpeModel.LstProjTask = objBL.ProjTaskList(ExpeModel.ProjectId);
                    ViewBag.ProjectTask = new SelectList(ExpeModel.LstProjTask, "TaskID", "TaskName");
                }
                else
                {
                    ExpeModel.ExpenseDate = DateTime.Now;
                }
                ProjectBL Blobj = new ProjectBL();
                List<DrpItem> LstModel = new List<DrpItem>();
                ViewBag.CustomerList = new SelectList(Blobj.GetCustomerListForTM(User_Id, ExpeModel.ProjectId, ExpeModel.TaskId), "Id", "DisplayName");
                ViewBag.VendorList = new SelectList(Blobj.GetProjectTaskVendorList(ExpeModel.ProjectId, ExpeModel.TaskId, User_Id), "Id", "DisplayName");
                return View(ExpeModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateProjectExpenses(FormCollection fc, HttpPostedFileBase file)
        {
            ProjectExpensesModel ExpeModel = new ProjectExpensesModel();
            string User_Id = User.Identity.GetUserId();
            ProjectBL BlObj = new ProjectBL();
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");

            try
            {
                ExpeModel.ExpenseId = Convert.ToInt32(fc["ExpenseId"].ToString());
                ExpeModel.ExpTypeId = Convert.ToInt32(fc["ExpTypeId"].ToString());
                ExpeModel.ProjectId = Convert.ToInt32(fc["ProjectId"].ToString());
                ExpeModel.TaskId = Convert.ToInt32(fc["TaskId"].ToString());
                if (fc["CustomerId"].ToString() != null && fc["CustomerId"].ToString() != "")
                    ExpeModel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                if(fc["VendorId"].ToString() != null && fc["VendorId"].ToString() != "")
                ExpeModel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                ExpeModel.Amount = Convert.ToDouble(fc["Amount"].ToString());
                ExpeModel.ExpenseDate = Convert.ToDateTime(fc["ExpenseDate"].ToString());
                ExpeModel.Remark = fc["Remark"].ToString();
                ExpeModel.Description = fc["Description"].ToString();
                if (file != null)
                {
                    string fileName = System.IO.Path.GetFileName(file.FileName);
                    if (file.ContentLength > 0)
                    {
                        try
                        {
                            string[] FileSplit = fileName.Split('.');
                            string FileEx = FileSplit[1].ToString();
                            String guid = Guid.NewGuid().ToString();
                            string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                            string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                            string FileName = Convert.ToString("/ProjectExpenceDoc/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
                            string localPath = Server.MapPath("~/App_Data/uploads");
                            if (!System.IO.Directory.Exists(localPath))
                            {
                                System.IO.Directory.CreateDirectory(localPath);
                            }
                            file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName));
                            if (ftpServer.Trim().Length > 0)
                            {
                                for (int i = 0; i < 3; i++)
                                {

                                    FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                                    requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                    requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                                    int bufferLength = 2048;
                                    byte[] buffer = new byte[file.InputStream.Length];
                                    Stream uploadStream = requestFTPUploader.GetRequestStream();
                                    int contentLength = file.InputStream.Read(buffer, 0, Convert.ToInt32(file.InputStream.Length));
                                    while (contentLength != 0)
                                    {
                                        uploadStream.Write(buffer, 0, contentLength);
                                        contentLength = file.InputStream.Read(buffer, 0, bufferLength);
                                    }
                                    uploadStream.Close();
                                    //fileStream.Close();
                                    requestFTPUploader = null;
                                    ExpeModel.DocumentPath = FileName;
                                    break;

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                int errcode = BlObj.SaveProjectExpenses(ExpeModel, User_Id);
                if (errcode > 0)
                {
                    return RedirectToAction("CreateProjectExpenses", new { ExpenseId = errcode });
                }
                else
                {
                    if (file.ContentLength > 0)
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            try
                            {
                                FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(ftpServer + ExpeModel.DocumentPath);
                                requestFileDelete.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                                FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                                requestFileDelete = null;

                                break;
                            }
                            catch (Exception ex)
                            {
                                if (x > 2)
                                {
                                    throw ex;
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
            return RedirectToAction("CreateProjectExpenses", new { ExpenseId = 0 });
        }

        public ActionResult DownloadExpenses(int ExpenseId, string Filename)
        {
            try
            {
                String FTPServer = Common.GetConfigValue("FTP");
                String FTPUserId = Common.GetConfigValue("FTPUid");
                String FTPPwd = Common.GetConfigValue("FTPPWD");

                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + Filename);
                requestFileDownload.UseBinary = true;
                requestFileDownload.KeepAlive = false;
                requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                byte[] OPData = ReadFully(responseStream);
                responseStream.Close();

                Response.AddHeader("content-disposition", "attachment; filename=" + Filename);
                Response.ContentType = "application/zip";
                Response.BinaryWrite(OPData);
                Response.End();
            }
            catch (Exception e)
            {
                throw e;
            }

            return RedirectToAction("CreateProjectExpenses", new { ExpenseId = ExpenseId });
        }
        #endregion Project Expenses  

        #region Project Expenses Approval

        public ActionResult ProjectExpensesApprovalList()
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/ProjectExpensesApprovalList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                ProjectBL objBL = new ProjectBL();
                ProjectExpensesModel Model = new ProjectExpensesModel();
                Model.LstPrjectExp = objBL.TMProjectExpensesApprovalList(User_Id);
                return View(Model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ExpenseSendForApprove(int ExpenseId, int statusCode)
        {
            try
            {
                string User_Id = User.Identity.GetUserId();
                ProjectBL objbl = new ProjectBL();
                int ErrorCode = objbl.ExpenseSendForApprove(statusCode, User_Id, ExpenseId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("ProjectExpensesList");
        }
        public ActionResult ApproveRejectExpenses(int ExpensesId)
        {
            ProjectExpensesModel Model = new ProjectExpensesModel();
            try
            {
                ProjectBL objBL = new ProjectBL();
                List<ExpenseTypeModel> ExptypeList = new List<ExpenseTypeModel>();
                ExptypeList = objBL.GetExpenseTypeList();
                ViewBag.ExpnsType = new SelectList(ExptypeList, "ExpTypeId", "ExpenceType");
                Model = objBL.TMGetSelectedProjectExpensesList(ExpensesId);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(Model);
        }
        [HttpPost]
        public ActionResult ApproveRejectExpenses(FormCollection fc)
        {
            ProjectExpensesModel Model = new ProjectExpensesModel();
            ProjectBL objBL = new ProjectBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                Model.ExpenseId = Convert.ToInt32(fc["ExpenseId"].ToString());
                Model.ManagerRemark = fc["ManagerRemark"].ToString();
                Model.StatusCode = fc["Decision"].ToString();
                if (Model.StatusCode == "Reject")
                {
                    Model.Status = 25;
                }
                else
                {
                    Model.Status = 24;
                }
                int errcode = objBL.SaveApvRejExpenses(Model, User_Id);
                return RedirectToAction("ProjectExpensesApprovalList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(Model);
        }
        #endregion Project Expenses Approval

        #region  Project Expenses Approval by Accounts
        public ActionResult ProjectExpensesApprovalListbyAccounts()
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/ProjectExpensesApprovalList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                ProjectBL objBL = new ProjectBL();
                ProjectExpensesModel Model = new ProjectExpensesModel();
                Model.LstPrjectExp = objBL.TMProjectExpensesApprovalListbyAccount(User_Id);
                return View(Model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ApproveRejectExpensesbyAccount(int ExpensesId)
        {
            ProjectExpensesModel Model = new ProjectExpensesModel();
            try
            {
                ProjectBL objBL = new ProjectBL();
                List<ExpenseTypeModel> ExptypeList = new List<ExpenseTypeModel>();
                ExptypeList = objBL.GetExpenseTypeList();
                ViewBag.ExpnsType = new SelectList(ExptypeList, "ExpTypeId", "ExpenceType");
                Model = objBL.TMGetSelectedProjectExpensesListbyAccount(ExpensesId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(Model);
        }
        [HttpPost]
        public ActionResult ApproveRejectExpensesbyAccount(FormCollection fc)
        {
            ProjectExpensesModel Model = new ProjectExpensesModel();
            ProjectBL objBL = new ProjectBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                Model.ExpenseId = Convert.ToInt32(fc["ExpenseId"].ToString());
                Model.ManagerRemark = fc["ManagerRemark"].ToString();
                Model.StatusCode = fc["Decision"].ToString();
                if (Model.StatusCode == "Reject")
                {
                    Model.Status = 25;
                }
                else
                {
                    Model.Status = 57;
                }
                int errcode = objBL.SaveApvRejExpenses(Model, User_Id);
                return RedirectToAction("ProjectExpensesApprovalListbyAccounts");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(Model);
        }
        #endregion Project Expenses Approval by Accounts
        #region Expense Type
        public ActionResult ExpenseTypeList()
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/ExpenseTypeList");
            if (FindFlag)
            {
                ProjectBL objBL = new ProjectBL();
                List<ExpenseTypeModel> List = new List<ExpenseTypeModel>();
                List = objBL.GetExpenseTypeList();
                ViewBag.test = List;
                return View(List);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportExpenseTypeLst")]
        [AcceptVerbs("POST")]
        public void ExportExpenseTypeLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            ProjectBL objBL = new ProjectBL();
            var DataSource = objBL.GetExpenseTypeList();
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateExpenseType(int ExpTypeId)
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProjectCenter/CreateExpenseType?ExpTypeId=0");
            if (FindFlag)
            {
                ExpenseTypeModel model = new ExpenseTypeModel();
                ProjectBL objBL = new ProjectBL();
                model.ExpTypeId = 0;
                if (ExpTypeId > 0)
                {
                    model = objBL.GetselectedExpenseType(ExpTypeId);
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateExpenseType(FormCollection fc)
        {

            ExpenseTypeModel model = new ExpenseTypeModel();
            ProjectBL objBL = new ProjectBL();
            string User_id = User.Identity.GetUserId();
            if (fc["ExpTypeId"].ToString() != "0")
            {
                model.ExpTypeId = Convert.ToInt32(fc["ExpTypeId"].ToString());
            }
            else
            {
                model.ExpTypeId = 0;
            }

            model.ExpenceType = fc["ExpenceType"].ToString();
            model.Description = fc["Description"].ToString();
            int errorcode = objBL.SaveExpenseType(model, User_id);
            if (errorcode == 500001 || errorcode == 500002)
            {
                return RedirectToAction("ExpenseTypeList");
            }
            else
            {
                return RedirectToAction("ExpenseTypeList");
            }

        }
        #endregion Expense Type

        #region Project Equipment and Items 
        public ActionResult ProjectEquipment(int ProjectId, string projectName)
        {
            TMEquipmentModel Model = new TMEquipmentModel();
            Model.lstEquipment = new List<TMEquipmentModel>();
            ProjectBL blObj = new ProjectBL();
            try
            {
                TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                List<TMEquipmentModel> lstTMEquipment = ObjTMBL.GetTMEquipmentList();
                ViewBag.EquipmentItems = new SelectList(lstTMEquipment, "EquipmentId", "EquipmentName");
                Model.ProjectId = ProjectId;
                Model.ProjectName = projectName;
                Model.lstEquipment = blObj.ProjectEquipmentList(ProjectId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(Model);
        }
        [HttpPost]
        public ActionResult ProjectEquipment(FormCollection fc)
        {
            TMEquipmentModel Model = new TMEquipmentModel();
            ProjectBL blObj = new ProjectBL();
            try
            {
                Model.ProjectId = Convert.ToInt32(fc["ProjectId"]);
                Model.ProjectName = fc["ProjectName"];
                Model.EquipmentId = Convert.ToInt32(fc["EquipmentId"]);
                Model.Quantity = Convert.ToDouble(fc["Quantity"]);
                int errCode = blObj.SaveProjectEquipment(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("ProjectEquipment", new { ProjectId = Model.ProjectId, ProjectName = Model.ProjectName });

        }

        public ActionResult ProjectItems(int ProjectId, string projectName)
        {
            TMEquipmentItem Model = new TMEquipmentItem();
            Model.lstEquipItems = new List<TMEquipmentItem>();
            ProjectBL blObj = new ProjectBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                ItemModel itemModel = new ItemModel();
                ItemBL BLObj = new ItemBL();
                itemModel.lstItem = new List<ItemModel>();
                itemModel.lstItem = BLObj.GetItemList(User_Id);
                ViewBag.itemlist = new SelectList(itemModel.lstItem, "ItemId", "ItemName");

                TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                Model.ProjectId = ProjectId;
                Model.ProjectName = projectName;
                Model.lstEquipItems = blObj.ProjectItemList(ProjectId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(Model);
        }

        [HttpPost]
        public ActionResult ProjectItems(FormCollection fc)
        {
            TMEquipmentItem Model = new TMEquipmentItem();
            ProjectBL blObj = new ProjectBL();
            try
            {
                Model.ProjectId = Convert.ToInt32(fc["ProjectId"]);
                Model.ProjectName = fc["ProjectName"];
                Model.ItemId = Convert.ToInt32(fc["ItemId"]);
                Model.Quantity = Convert.ToDouble(fc["Quantity"]);
                Model.TAM = Convert.ToDouble(fc["TAM"]);
                int errCode = blObj.SaveProjectItem(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("ProjectItems", new { ProjectId = Model.ProjectId, ProjectName = Model.ProjectName });

        }
        #endregion Project Equipment and Items



    }
}