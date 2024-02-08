using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.Project;
using SmartSys.BL.ProViews;
using SmartSys.BL.Tmleave;
using SmartSys.Utility;
using Syncfusion.EJ.Export;
using Syncfusion.JavaScript.Models;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SmartSys.Controllers
{
    public class TMLeaveController : Controller
    {
        // GET: TMLeave
        #region TMLeaveTypedetail
        public ActionResult TMLeaveTypeList()
        {
            try
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

                FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/TMLeaveTypeList");
                if (FindFlag)
                {
                    List<TMLeaveModel> objmodel = new List<TMLeaveModel>();
                    TMLeaveBL objbl = new TMLeaveBL();
                    objmodel = objbl.GetTMList(UserId);
                    return View(objmodel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        [System.Web.Http.ActionName("ExportLeaveTypeList")]
        [AcceptVerbs("POST")]
        public void ExportLeaveTypeList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            TMLeaveBL objbl = new TMLeaveBL();
            var DataSource = objbl.GetTMList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateTMLeaveType()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/TMLeaveTypeList");
            if (FindFlag)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateTMLeaveType(FormCollection fc, TMLeaveModel objmodel)
        {
            try
            {
                TMLeaveBL objbl = new TMLeaveBL();
                objmodel.lstLeave = new List<TMLeaveModel>();
                string User_Id = User.Identity.GetUserId();
                objmodel.LeaveTypeId = 0;
                objmodel.LeaveType = fc["LeaveType"].ToString();
                objmodel.Description = fc["Description"].ToString();
                objmodel.YearlyQuota = Convert.ToInt32(fc["YearlyQuota"].ToString());
                objmodel.MaxLeaveCarryover = Convert.ToInt32(fc["MaxLeaveCarryover"].ToString());

                if (fc["IsPaid"].ToString() == "true")
                {
                    objmodel.IsPaid = Convert.ToBoolean(1);
                }
                else
                {
                    objmodel.IsPaid = Convert.ToBoolean(0);
                }

                if (fc["CanLaps"].ToString() == "true")
                {
                    objmodel.CanLaps = Convert.ToBoolean(1);
                }
                else
                {
                    objmodel.CanLaps = Convert.ToBoolean(0);
                }

                if (fc["Enable"].ToString() == "true")
                {
                    objmodel.Enable = Convert.ToBoolean(1);
                }
                else
                {
                    objmodel.Enable = Convert.ToBoolean(0);
                }

                int ErrCode = objbl.SaveTMLeave(objmodel, User_Id);
                if (ErrCode == 500002)
                {
                    return RedirectToAction("TMLeaveTypeList");
                }

                else
                {
                    return RedirectToAction("CreateTMLeaveType");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        public ActionResult EditTMLeaveType(int LeaveTypeId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/TMLeaveTypeList");
            if (FindFlag)
            {
                TMLeaveBL objbl = new TMLeaveBL();
                TMLeaveModel objmodel = new TMLeaveModel();
                objmodel = objbl.GetSelectedTMLeaveList(LeaveTypeId);

                return View(objmodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditTMLeaveType(FormCollection fc)
        {

            try
            {
                TMLeaveModel objmodel = new TMLeaveModel();
                TMLeaveBL objbl = new TMLeaveBL();
                objmodel.lstLeave = new List<TMLeaveModel>();
                string User_Id = User.Identity.GetUserId();
                objmodel.LeaveTypeId = Convert.ToInt32(fc["LeaveTypeId"].ToString());
                objmodel.LeaveType = fc["LeaveType"].ToString();
                objmodel.Description = fc["Description"].ToString();
                objmodel.YearlyQuota = Convert.ToInt32(fc["YearlyQuota"].ToString());
                objmodel.MaxLeaveCarryover = Convert.ToInt32(fc["MaxLeaveCarryover"].ToString());

                if (fc["IsPaid"] == "true")
                {
                    objmodel.IsPaid = Convert.ToBoolean(1);
                }
                else
                {
                    objmodel.IsPaid = Convert.ToBoolean(0);
                }

                if (fc["CanLaps"] == "true")
                {
                    objmodel.CanLaps = Convert.ToBoolean(1);
                }
                else
                {
                    objmodel.CanLaps = Convert.ToBoolean(0);
                }

                if (fc["Enable"] == "true")
                {
                    objmodel.Enable = Convert.ToBoolean(1);
                }
                else
                {
                    objmodel.Enable = Convert.ToBoolean(0);
                }

                int ErrCode = objbl.SaveTMLeave(objmodel, User_Id);
                if (ErrCode == 500001)
                {
                    return RedirectToAction("TMLeaveTypeList");
                }
                return RedirectToAction("TMLeaveTypeList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion TMLeaveTypedetail

        #region Leave Approval
        public ActionResult TMPendingLeaveApprovalList()
        {
            try
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

                FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/TMPendingLeaveApprovalList");
                if (FindFlag)
                {
                    List<TMLeaveDetailModel> objmodel = new List<TMLeaveDetailModel>();
                    TMLeaveBL objbl = new TMLeaveBL();
                    string User_Id = User.Identity.GetUserId();
                    objmodel = objbl.TMPendingLeaveApprovalList(User_Id);
                    return View(objmodel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [System.Web.Http.ActionName("ExportPendingLeaveApprovalList")]
        [AcceptVerbs("POST")]
        public void ExportPendingLeaveApprovalList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            TMLeaveBL objbl = new TMLeaveBL();
            var DataSource = objbl.TMPendingLeaveApprovalList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        [HttpPost]
        public ActionResult ApproveRejectLeave(FormCollection fc)
        {
            try
            {
                TMLeaveDetailModel objmodel = new TMLeaveDetailModel();
                TMLeaveBL ObjTMBL = new TMLeaveBL();
                string User_Id = User.Identity.GetUserId();

                if (fc["LeaveId"].ToString() == "0")
                {
                    return RedirectToAction("TMPendingLeaveApprovalList");
                }
                else
                {
                    objmodel = ObjTMBL.GetSelectedTMLeaveDetail(Convert.ToInt32(fc["LeaveId"].ToString()));
                    objmodel.ManagerRemark = fc["ManagerRemark"].ToString();
                    objmodel.ApprovedDate = DateTime.Now;
                    if (fc["Decision"].ToString() == "Approve")
                        objmodel.Status = 24;
                    else
                        objmodel.Status = 25;
                }
                int errCode = ObjTMBL.SaveTMLeaveDetails(objmodel, User_Id);
                if (errCode == 500001)
                {
                    DataSet ds = new DataSet();
                    ds = ObjTMBL.GetLeaveInfo(objmodel.LeaveId);
                    string Subject = ds.Tables[0].Rows[0]["EmpName"].ToString() + "  On Leave From  " + " " + objmodel.FromDate.ToShortDateString() + " " + "To" + " " + objmodel.ToDate.ToShortDateString() + " ";
                    string ToMail = ds.Tables[0].Rows[0]["EmpEmail"].ToString() + ',' + ds.Tables[0].Rows[0]["ManagerEmail"].ToString();
                    string Decis = fc["Decision"].ToString();
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
                                  "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + " When :" + "</span>" + "</div>" +
                                   "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "From : " + objmodel.FromDate.ToLongDateString() + "</span>" + "</div>" +
                                    "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "To : " + objmodel.ToDate.ToLongDateString() + "</span>" + "</div>" +
                                    "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "Leave Status :" + Decis + "</span>" + "</div>" +
                                      "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "Applicant Name : " + ds.Tables[0].Rows[0]["EmpName"] + ", " + "Applicant Id :" + ds.Tables[0].Rows[0]["EmpId"] + "</span>" + "</div>" +
                                    "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "Manager Name : " + ds.Tables[0].Rows[0]["EmpManagerName"] + "</span>" + "</div>" +
                                     "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "Employee Remark : " + objmodel.Remark + "</span>" + "</div>" +
                                      "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "Manager Remark : " + objmodel.ManagerRemark + "</span>" + "</div>" +
                    "</body>" +
                  "</html>";

                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            int mail = Utility.Common.SendMail(Subject, MailBody, "", ToMail, "", null);
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (i == 2)
                            {
                                break;
                            }
                        }
                    }
                    return RedirectToAction("TMPendingLeaveApprovalList");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("TMPendingLeaveApprovalList");
        }

        public ActionResult ApproveRejectLeave(int LeaveId)
        {
            TMLeaveDetailModel model = new TMLeaveDetailModel();

            if (LeaveId > 0)
            {
                TMLeaveBL ObjTMBL = new TMLeaveBL();
                model = ObjTMBL.GetSelectedTMLeaveDetail(LeaveId);
            }
            return View(model);
        }

        #endregion Leave Approval

        #region Leave
        public ActionResult TmLeaveDetailList()
        {

            try
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

                FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/TMLeaveDetailList");
                if (FindFlag)
                {
                    List<TMLeaveDetailModel> objmodel = new List<TMLeaveDetailModel>();
                    TMLeaveBL objbl = new TMLeaveBL();
                    string User_Id = User.Identity.GetUserId();
                    objmodel = objbl.TMLeaveList(User_Id);
                    return View(objmodel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [System.Web.Http.ActionName("ExportLeaveDetailList")]
        [AcceptVerbs("POST")]
        public void ExportLeaveDetailList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            TMLeaveBL objbl = new TMLeaveBL();
            var DataSource = objbl.TMLeaveList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateTMLeaveDetail(int LeaveId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/CreateTMLeaveDetail?LeaveId=0");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                TMLeaveDetailModel model = new TMLeaveDetailModel();
                List<TMLeaveModel> LeaveTypeList = new List<TMLeaveModel>();
                model.LstLeaveDescription = new List<LeaveDescription>();
                TMLeaveBL objbl = new TMLeaveBL();
                LeaveTypeList = objbl.GetTMList(User_Id);
                ViewBag.LeaveType = new SelectList(LeaveTypeList, "LeaveTypeId", "LeaveType");
                if (Session["errmsg"] != null)
                {
                    TempData["Message"] = Session["errmsg"] as string;
                }
                Session["errmsg"] = null;
                if (LeaveId > 0)
                {
                    TMLeaveBL ObjTMBL = new TMLeaveBL();
                    model = ObjTMBL.GetSelectedTMLeaveDetail(LeaveId);
                }
                model.StatusList = new List<StatusModel>();

                model.StatusList = objbl.GetStatusListForDropDown();
                ViewBag.status = new SelectList(model.StatusList, "StatusId", "StatusShortCode");
                model.LstLeaveDescription = objbl.GetLeaveDescription(User_Id);
                foreach (var item in model.LstLeaveDescription)
                {
                    var leavetype = item.LeaveType;
                    if (leavetype == "Sort Leave")
                    {
                        model.BalanceLeave = Convert.ToInt32(item.BalanceLeave);
                        break;
                    }
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateTMLeaveDetail(FormCollection fc)
        {
            try
            {
                DataSet ds = new DataSet();
                TMLeaveDetailModel objmodel = new TMLeaveDetailModel();
                objmodel.LstLeaveDescription = new List<LeaveDescription>();
                TMLeaveBL ObjTMBL = new TMLeaveBL();
                string User_Id = User.Identity.GetUserId();
                if (fc["BalanceLeave"].ToString() == "0")
                {
                    objmodel.LeaveCategory = "FH";
                }
                else
                {
                    objmodel.LeaveCategory = fc["LeaveCategory"].ToString();
                }
                if (fc["LeaveId"].ToString() == "0")
                {
                    objmodel.LeaveId = 0;
                }
                else
                {
                    objmodel.LeaveId = Convert.ToInt32(fc["LeaveId"].ToString());
                }
                objmodel.LeaveTypeId = Convert.ToInt32(fc["LeaveTypeId"].ToString());

                objmodel.FromDate = Convert.ToDateTime(fc["FromDate"].ToString());
                objmodel.ToDate = Convert.ToDateTime(fc["ToDate"].ToString());
                objmodel.Status = (int)Utility.Enums.StatusCode.Applied; //Applied
                objmodel.Remark = fc["Remark"].ToString();
                Double balLeave = ObjTMBL.GetLeaveBalance(User_Id, objmodel.LeaveCategory, objmodel.LeaveTypeId);
                TimeSpan difference = objmodel.ToDate - objmodel.FromDate;
                Double days = (Double)difference.TotalDays + 1;
                string LeaveType = "";
                if (objmodel.LeaveCategory == "ST")
                {
                    LeaveType = "Short Time";
                }
                else if (objmodel.LeaveCategory == "FT")
                {
                    LeaveType = "Full Time";
                }
                else if (objmodel.LeaveCategory == "FH")
                {
                    LeaveType = "First Half Day";
                    days = days / 2;
                }
                else if (objmodel.LeaveCategory == "SH")
                {
                    LeaveType = "Second Half Day";
                    days = days / 2;
                }
                if (days > balLeave)
                {
                    Session["errmsg"] = "You dont have enough Leave";
                    return RedirectToAction("CreateTMLeaveDetail", new { LeaveId = objmodel.LeaveId });
                }



                int errCode = ObjTMBL.SaveTMLeaveDetails(objmodel, User_Id);
                if (errCode == 500001 || errCode == 500002 || errCode == 0)
                {
                    objmodel.LstLeaveDescription = ObjTMBL.GetLeaveDescription(User_Id);
                    string body = string.Empty;
                    body +=
                        "Details List" +
                         "<br /> <br />";
                    body += "<table border='1'><tr style='background-color:#000000;color:white'><th>LeaveType</th><th>TotalLeave</th><th>MaxLeaveCarryover</th><th>BalanceLeave</th></tr>";
                    foreach (var item in objmodel.LstLeaveDescription)
                    {
                        body += "<tr><td>" + item.LeaveType + "</td>";
                        body += "<td>" + item.TotalLeave + "</td>";
                        body += "<td>" + item.MaxLeaveCarryover + "</td>";
                        body += "<td>" + item.BalanceLeave + "</td></tr>";
                    }
                    body += "</table>";
                    //return RedirectToAction("TmLeaveDetailList");                    
                    ds = ObjTMBL.GetEmpmanagerInfo(User_Id);

                    string Subject = "<" + ds.Tables[0].Rows[0]["EmpName"].ToString() + ">" + "  On Leave From" + "<" + objmodel.FromDate + ">" + "To" + "<" + objmodel.ToDate + ">";
                    string ToMail = ds.Tables[0].Rows[0]["EmpEmail"].ToString() + ',' + ds.Tables[0].Rows[0]["ManagerEmail"].ToString();
                    string CCMail = ds.Tables[0].Rows[0]["HrMailId"].ToString();
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
                                  "<table>" +
                                  "<tr>" +
                                  "<td>" +
                                  "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + " When :" + "</span>" + "</div>" +
                                  "</td>" +
                                  "</tr>" +
                                    "<tr>" +
                                  "<td>" +
                                  "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "From :" + "<input type='text'  value='" + objmodel.FromDate.ToShortDateString() + "'>" + "</span>" + "</div>" +
                                   "</td>" +
                                  "</tr>" +
                                    "<tr>" +
                                  "<td>" +
                                  "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "To :" + "<input type='text'  value='" + objmodel.ToDate.ToShortDateString() + "'>" + "</span>" + "</div>" +
                                    "</td>" +
                                  "</tr>" +
                                     "<tr>" +
                                  "<td>" +
                                  "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Leave:" + "<input type='text'  value='" + LeaveType + "'>" + "</span>" + "</div>" +
                                  "</td>" +
                                  "</tr>" +
                                  "<tr>" +
                                  "<td>" +
                                  "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "Leave Status :" + "<input type='text'  value='Applied (Not Approved)'>" + "</span>" + "</div>" +
                                 "</td>" +
                                  "</tr>" +
                                    "<tr>" +
                                  "<td>" +
                                  "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "Applicant Name :" + "<input type='text'  value='" + ds.Tables[0].Rows[0]["EmpName"] + "'>" + "," + "Applicant Id :" + "<input type='text'  value='" + ds.Tables[0].Rows[0]["EmpId"] + "'>" + "</span>" + "</div>" +
                                   "</td>" +
                                  "</tr>" +
                                    "<tr>" +
                                  "<td>" +
                                  "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "Manager Name :" + "<input type='text'  value='" + ds.Tables[0].Rows[0]["ManagerName"] + "'>" + "</span>" + "</div>" +
                                     "</td>" +
                                  "</tr>" +
                                      "<tr>" +
                                  "<td>" +
                                  "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "Employee Remark :" + "<input type='text'  value='" + objmodel.Remark + "'>" + "</span>" + "</div>" +
                                     "</td>" +
                                  "</tr>" +
                                      "<tr>" +
                                  "<td>" +
                                  "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Leave Detail : " + "</b>" + body + "</span>" + "</div>" +
                                      "</td>" +
                                  "</tr>" +
                                  "</table>" +
                    "</body>" +
                  "</html>";

                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            int mail = Utility.Common.SendMail(Subject, MailBody, "", ToMail, CCMail, null);
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
                    return RedirectToAction("TmLeaveDetailList");
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return RedirectToAction("TmLeaveDetailList");
            }
            return View();

        }
        #endregion Leave

        #region Time Sheet Daily Entry 
        public ActionResult GetDailtEntryList(int TimeSheetId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/GetDailtEntryList?TimeSheetId=0");
            if (FindFlag)
            {
                TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
                try
                {
                    if (Session["TimeError"] != null)
                    {
                        TempData["Message"] = Session["TimeError"] as string;
                    }
                    Session["TimeError"] = null;

                    string User_Id = User.Identity.GetUserId();
                    TMLeaveBL BLObj = new TMLeaveBL();
                    TMModel = BLObj.GetTimeSheetEntryList(TimeSheetId, User_Id);
                    ViewBag.TMDateList = new SelectList(TMModel.TMdatelist, "WeekNo", "Date");
                    TimesheetHistory HistoryModel = new TimesheetHistory();
                    HistoryModel = BLObj.GetTMHistoryList(User_Id);
                    ViewBag.HistoryList = HistoryModel.LstHistory;
                    TMModel.TimeSheetId = TimeSheetId;
                    CultureInfo ciCurr = CultureInfo.CurrentCulture;
                    if (TMModel.StartDate.Year < DateTime.Now.Year)
                    {
                        TMModel.AllowBtn = "Allow";
                    }
                    else
                    {
                        int CurrentWeek = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                        int chk = (CurrentWeek - 1) - TMModel.WeekNo;
                        if (TimeSheetId != 0)
                        {
                            if (chk > 0)
                            {
                                TMModel.AllowBtn = "Allow";
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(TMModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportTimeSheetHistoryLst")]
        [AcceptVerbs("POST")]
        public void ExportTimeSheetHistoryLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            TimesheetHistory HistoryModel = new TimesheetHistory();
            TMLeaveBL BLObj = new TMLeaveBL();
            HistoryModel = BLObj.GetTMHistoryList(User_Id);
            var DataSource = HistoryModel.LstHistory;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
       
        //public void ExportTimeSheetDetailLst(string GridModel)
        //{
        //    string User_Id = User.Identity.GetUserId();
        //    ExcelExport exp = new ExcelExport();
        //    Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
        //    TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
        //    TMLeaveBL BLObj = new TMLeaveBL();
        //    TMModel = BLObj.GetTimeSheetEntryList(TimeSheetId, User_Id);
        //    var DataSource = BlObj.GetBussLineList();
        //    exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        //}
        [HttpGet]
        public ActionResult CreateTMEntry(int TimeSheetEntryId, int TimeSheetId)
        {
            TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
            TMGetSelectedModel TMSelectModel = new TMGetSelectedModel();
            try
            {
                string User_Id = User.Identity.GetUserId();
                TMLeaveBL BLObj = new TMLeaveBL();
                TMSelectModel = BLObj.GetSelectedTimeSheetEntryList(TimeSheetEntryId, TimeSheetId, User_Id);
                TMSelectModel.TimeSheetId = TimeSheetId;
                TMModel.TMProjectlist = BLObj.GetProjectList(User_Id);
                ViewBag.TMProjectlist = new SelectList(TMModel.TMProjectlist, "ProjectId", "ProjectName");

                TMModel.TMTasklist = BLObj.GetTaskList(TMSelectModel.ProjectId, User_Id);
                ViewBag.TMTasklist = new SelectList(TMModel.TMTasklist, "TaskId", "TaskName");
                TMSelectModel.lstDayTime = new List<DayTimeLstModel>();
                TMSelectModel.lstDayTime = BLObj.GetDayTimeList(TMSelectModel.ProjectId, TMSelectModel.TaskId, TMSelectModel.Day, TimeSheetId);
                ViewBag.DayTimeLst = TMSelectModel.lstDayTime;              
                ProjectBL Blobj = new ProjectBL();
                ViewBag.CustomerList = new SelectList(Blobj.GetCustomerListByUser(User_Id), "Id", "DisplayName");
                ViewBag.VendorList = new SelectList(Blobj.GetVendorListByUser(User_Id), "Id", "DisplayName");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(TMSelectModel);
        }
        public JsonResult UpdateTimeSheetEntry(DayTimeLstModel ObjModel , int MOMId)
        {
            TMLeaveBL Blobj = new TMLeaveBL();
            ObjModel.MOMId = MOMId;
            Blobj.UpdateTMEntryDetail(ObjModel);
            return Json(new SelectList("", "MoMId", "Title"));
        }
        public JsonResult TmMoMByProject(int ProjectId, int TaskId, string Day, DateTime StartDate)
        {
            string User_Id = User.Identity.GetUserId();
            TMLeaveBL objBl = new TMLeaveBL();
            switch (Day)
            {
                case "Monday":
                    {
                        StartDate = StartDate.AddDays(0);
                        break;
                    }
                case "Tuesday":
                    {
                        StartDate = StartDate.AddDays(1);
                        break;
                    }
                case "Wednesday":
                    {
                        StartDate = StartDate.AddDays(2);
                        break;
                    }
                case "Thursday":
                    {
                        StartDate = StartDate.AddDays(3);
                        break;
                    }
                case "Friday":
                    {
                        StartDate = StartDate.AddDays(4);
                        break;
                    }
                case "Saturday":
                    {
                        StartDate = StartDate.AddDays(5);
                        break;
                    }
                case "Sunday":
                    {
                        StartDate = StartDate.AddDays(6);
                        break;
                    }
            }
            List<TMMOMList> MoMList = new List<TMMOMList>();
            MoMList = objBl.GetMoMByProject(User_Id, ProjectId, TaskId, StartDate);
            return Json(new SelectList(MoMList, "MoMId", "Title"));
        }
        [HttpPost]
        public ActionResult CreateTMEntry(FormCollection fc)
        {
            int errcode = 0;
            TMGetSelectedModel TMModel = new TMGetSelectedModel();
            TimeSheetDlyEntryList TMCheck = new TimeSheetDlyEntryList();
            string User_Id = User.Identity.GetUserId();
            bool Check = true;
            try
            {
                TMModel.TimeSheetEntryId = Convert.ToInt32(fc["TimeSheetEntryId"].ToString());
                TMModel.TimeSheetId = Convert.ToInt32(fc["TimeSheetId"].ToString());
                if (fc.AllKeys.Contains("MoMId"))
                    if (fc["MoMId"].ToString() != "")
                        TMModel.MOMId = Convert.ToInt32(fc["MoMId"].ToString());
                TMModel.Remark = fc["Remark"].ToString();
                TMModel.TaskId = Convert.ToInt32(fc["TaskId"].ToString());
                TMModel.ProjectId = Convert.ToInt32(fc["ProjectId"].ToString());
                TMModel.Day = "0";
                TMModel.MonFromTime = Convert.ToDateTime(fc["MonFromTime"]);
                TMModel.MonToTime = Convert.ToDateTime(fc["MonToTime"]);

                TMModel.TusFromTime = Convert.ToDateTime(fc["TusFromTime"]);
                TMModel.TusToTime = Convert.ToDateTime(fc["TusToTime"]);

                TMModel.WedFromTime = Convert.ToDateTime(fc["WedFromTime"]);
                TMModel.WedToTime = Convert.ToDateTime(fc["WedToTime"]);

                TMModel.ThusFromTime = Convert.ToDateTime(fc["ThusFromTime"]);
                TMModel.ThusToTime = Convert.ToDateTime(fc["ThusToTime"]);

                TMModel.FriFromTime = Convert.ToDateTime(fc["FriFromTime"]);
                TMModel.FriToTime = Convert.ToDateTime(fc["FriToTime"]);

                TMModel.SatFromTime = Convert.ToDateTime(fc["SatFromTime"]);
                TMModel.SatToTime = Convert.ToDateTime(fc["SatToTime"]);

                TMModel.SunFromTime = Convert.ToDateTime(fc["SunFromTime"]);
                TMModel.SunToTime = Convert.ToDateTime(fc["SunToTime"]);
                if (fc["CustomerId"].ToString() != "")
                    TMModel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                if (fc["VendorId"].ToString() != "")
                    TMModel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                if (Check)
                {
                    TMLeaveBL BLObj = new TMLeaveBL();
                    errcode = BLObj.UpdateTimeSheetEntryDetail(TMModel, User_Id);
                    if (errcode == 500006)
                    {
                        Session["TimeError"] = "This Week Time Sheet already Created";
                        return RedirectToAction("CreateTMEntry", new { TimeSheetEntryId = errcode, TimeSheetId = TMModel.TimeSheetId });
                    }
                    else
                    {
                        return RedirectToAction("CreateTMEntry", new { TimeSheetEntryId = errcode, TimeSheetId = TMModel.TimeSheetId });
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CreateTMEntry", new { TimeSheetEntryId = errcode, TimeSheetId = TMModel.TimeSheetId });
        }
        public JsonResult GetTMEntryDetailByDay(int ProjectId, int TaskId, string DayCode, int TimeSheetId)
        {
            TMGetSelectedModel TMSelectModel = new TMGetSelectedModel();
            try
            {
                TMLeaveBL BLObj = new TMLeaveBL();
                TMSelectModel.lstDayTime = new List<DayTimeLstModel>();
                TMSelectModel.lstDayTime = BLObj.GetDayTimeList(ProjectId, TaskId, DayCode, TimeSheetId);
                ViewBag.DayTimeLst = TMSelectModel.lstDayTime;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(TMSelectModel.lstDayTime, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTMEntryCustomerList(int ProjectId, int TaskId)
        {
            TMGetSelectedModel TMSelectModel = new TMGetSelectedModel();
            string User_Id = User.Identity.GetUserId();
            ProjectBL Blobj = new ProjectBL();
            List<DrpItem> LstModel = new List<DrpItem>();
            try
            {
                LstModel = Blobj.GetCustomerListForTM(User_Id, ProjectId, TaskId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(LstModel, "Id", "DisplayName"));
        }
        public JsonResult GetTMEntryVendorList(int ProjectId, int TaskId)
        {
            TMGetSelectedModel TMSelectModel = new TMGetSelectedModel();
            string User_Id = User.Identity.GetUserId();
            ProjectBL Blobj = new ProjectBL();
            List<DrpItem> LstModel = new List<DrpItem>();
            try
            {
                LstModel = Blobj.GetProjectTaskVendorList(ProjectId, TaskId, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(LstModel, "Id", "DisplayName"));
        }
        public ActionResult DeleteTMEntryDetail(int TimeSheetEntryDetailId, int TimeSheetEntryId, int TimeSheetId)
        {

            TMLeaveBL BLObj = new TMLeaveBL();
            string User_Id = User.Identity.GetUserId();
            int errCode = BLObj.DeleteTMEntryDetail(TimeSheetEntryDetailId, User_Id);
            return RedirectToAction("CreateTMEntry", new { TimeSheetEntryId = TimeSheetEntryId, TimeSheetId = TimeSheetId });
        }
        public ActionResult EditEntry(int TimeSheetEntryId, int TimeSheetId)
        {
            TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
            TMGetSelectedModel TMSelectModel = new TMGetSelectedModel();
            try
            {
                string User_Id = User.Identity.GetUserId();
                TMLeaveBL BLObj = new TMLeaveBL();
                TMSelectModel = BLObj.GetSelectedTimeSheetEntryList(TimeSheetEntryId, TimeSheetId, "");
                TMModel.TMProjectlist = BLObj.GetProjectList(User_Id);
                ViewBag.TMProjectlist = new SelectList(TMModel.TMProjectlist, "ProjectId", "ProjectName");

                TMModel.TMTasklist = BLObj.GetTaskList(TMSelectModel.ProjectId, User_Id);
                ViewBag.TMTasklist = new SelectList(TMModel.TMTasklist, "TaskId", "TaskName");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(TMSelectModel);
        }
        [HttpPost]
        public ActionResult EditEntry(FormCollection fc)
        {
            TMGetSelectedModel TMModel = new TMGetSelectedModel();
            TimeSheetDlyEntryList TMCheck = new TimeSheetDlyEntryList();
            string User_Id = User.Identity.GetUserId();
            bool Check = true;
            try
            {
                TMModel.TimeSheetEntryId = Convert.ToInt32(fc["TimeSheetEntryId"].ToString());
                TMModel.TimeSheetId = Convert.ToInt32(fc["TimeSheetId"].ToString());
                TMModel.TaskId = Convert.ToInt32(fc["TaskId"].ToString());
                TMModel.ProjectId = Convert.ToInt32(fc["ProjectId"].ToString());
                TMModel.Day = "0";
                TMModel.MonFromTime = Convert.ToDateTime(fc["MonFromTime"]);
                TMModel.MonToTime = Convert.ToDateTime(fc["MonToTime"]);

                TMModel.TusFromTime = Convert.ToDateTime(fc["TusFromTime"]);
                TMModel.TusToTime = Convert.ToDateTime(fc["TusToTime"]);

                TMModel.WedFromTime = Convert.ToDateTime(fc["WedFromTime"]);
                TMModel.WedToTime = Convert.ToDateTime(fc["WedToTime"]);

                TMModel.ThusFromTime = Convert.ToDateTime(fc["ThusFromTime"]);
                TMModel.ThusToTime = Convert.ToDateTime(fc["ThusToTime"]);

                TMModel.FriFromTime = Convert.ToDateTime(fc["FriFromTime"]);
                TMModel.FriToTime = Convert.ToDateTime(fc["FriToTime"]);

                TMModel.SatFromTime = Convert.ToDateTime(fc["SatFromTime"]);
                TMModel.SatToTime = Convert.ToDateTime(fc["SatToTime"]);

                TMModel.SunFromTime = Convert.ToDateTime(fc["SunFromTime"]);
                TMModel.SunToTime = Convert.ToDateTime(fc["SunToTime"]);

                if (Check)
                {
                    TMLeaveBL BLObj = new TMLeaveBL();
                    int errcode = BLObj.UpdateTimeSheetEntryDetail(TMModel, User_Id);
                    if (errcode == 500006)
                    {
                        Session["TimeError"] = "This Week Time Sheet already Created";
                        return RedirectToAction("GetDailtEntryList", new { TimeSheetId = TMModel.TimeSheetId });
                    }
                    else
                    {
                        return RedirectToAction("GetDailtEntryList", new { TimeSheetId = TMModel.TimeSheetId });
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetDailtEntryList", new { TimeSheetId = TMModel.TimeSheetId });
        }
        public JsonResult Tasklist(int ProjectId)
        {
            TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
            try
            {
                TMLeaveBL BLObj = new TMLeaveBL();
                string User_Id = User.Identity.GetUserId();
                TMModel.ProjectId = ProjectId;
                TMModel.TMTasklist = BLObj.GetTaskList(ProjectId, User_Id);
                ViewBag.TMTasklist = new SelectList(TMModel.TMTasklist, "TaskId", "TaskName");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(TMModel.TMTasklist, "TaskId", "TaskName"));
        }
        public ActionResult UpdateStatusTMEntry(int TimeSheetId)
        {
            try
            {
                TMLeaveBL BLObj = new TMLeaveBL();
                int errCode = BLObj.UpdateStatusTMEntry(TimeSheetId, Convert.ToInt32(Utility.Enums.StatusCode.Submitted));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetDailtEntryList", new { TimeSheetId = 0 });
        }
        public ActionResult SaveTimeSheetEntryLastApproval(int TimeSheetId)
        {
            try
            {
                TMLeaveBL BLObj = new TMLeaveBL();
                string User_Id = User.Identity.GetUserId();
                int errCode = BLObj.SaveTimeSheetEntryLastApproval(TimeSheetId, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetDailtEntryList", new { TimeSheetId = TimeSheetId });
        }
        public ActionResult DeleteTimeSheetEntry(int TimeSheetEntryId, int TimeSheetId)
        {
            try
            {
                TMLeaveBL BLObj = new TMLeaveBL();
                int errCode = BLObj.DeleteTimeSheetEntry(TimeSheetEntryId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetDailtEntryList", new { TimeSheetId = TimeSheetId });
        }
        public ActionResult GetTMRecordByWeeKNo(int WeekNo)
        {
            int errcode = 0;
            try
            {
                string User_Id = User.Identity.GetUserId();
                TMLeaveBL BLObj = new TMLeaveBL();
               // errcode = BLObj.GetTMIdByWeekNo(WeekNo, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetDailtEntryList", new { TimeSheetId = WeekNo });
        }

        public ActionResult CheckProjDisp(int projectId)
        {
            string str = "";
            try
            {
                TMLeaveBL BLObj = new TMLeaveBL();
                str = BLObj.CheckProjDisp(projectId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(str);
        }

        #endregion Time Sheet Daily Entry

        #region Project Category 
        public ActionResult ProjectCategoryList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/ProjectCategoryList");
            if (FindFlag)
            {
                ProjectCategoryModel TMProjectCategory = new ProjectCategoryModel();
                try
                {
                    Session["ProjectCategory"] = null;
                    TMLeaveBL BLObj = new TMLeaveBL();
                    string User_Id = User.Identity.GetUserId();
                    TMProjectCategory = BLObj.GetTMProjectCategoryList(User_Id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(TMProjectCategory);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult CreateProjectCategory(int ProCateId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/CreateProjectCategory?ProCateId=0");
            if (FindFlag)
            {
                ProjectCategoryModel TMProjectCategory = new ProjectCategoryModel();
                string User_Id = User.Identity.GetUserId();
                try
                {
                    if (Session["ProjectCategory"] != null)
                    {
                        TMProjectCategory = Session["ProjectCategory"] as ProjectCategoryModel;
                    }
                    else
                    {
                        TMLeaveBL BLObj = new TMLeaveBL();
                        TMProjectCategory = BLObj.GetSelectedTMProjectCategoryList(ProCateId);
                        TMProjectCategory.ProjectList = BLObj.GetProjectListbyCategory(ProCateId, User_Id);
                    }
                    ProjectBL objBL = new ProjectBL();
                    List<ProjectModel> lstproj = objBL.GetProjectList(User_Id);
                    if (TMProjectCategory.ProjectList != null)
                    {
                        foreach (TMProjectList rrv in TMProjectCategory.ProjectList)
                        {
                            var actionToDelete = from actionRepDel in lstproj
                                                 where actionRepDel.ProjectId == rrv.ProjectId && (actionRepDel.ProjectName).Trim() == rrv.ProjectName.Trim()
                                                 select actionRepDel;

                            lstproj.Remove(actionToDelete.ElementAt(0));
                        }
                    }
                    ViewBag.TMPrjectlist = new SelectList(lstproj, "ProjectId", "ProjectName");
                    Session["ProjectCategory"] = TMProjectCategory;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(TMProjectCategory);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult RemoveToProjectList(int ProCotegoryId, string CategoryName, int ProjectId, string ProjectName)
        {
            try
            {
                ProjectCategoryModel TMProjectCategory = new ProjectCategoryModel();
                if (Session["ProjectCategory"] != null)
                {
                    TMProjectCategory = Session["ProjectCategory"] as ProjectCategoryModel;
                }
                TMProjectCategory.CategoryName = CategoryName;
                var actionToDelete = from actionRepDel in TMProjectCategory.ProjectList
                                     where actionRepDel.ProjectId == ProjectId && actionRepDel.ProjectName.Trim() == ProjectName.Trim()
                                     select actionRepDel;

                TMProjectCategory.ProjectList.Remove(actionToDelete.ElementAt(0));

                return RedirectToAction("CreateProjectCategory", new { ProCateId = ProCotegoryId });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult AddToProjceList(int ProCotegoryId, string CategoryName, int ProjectId, string ProjectName)
        {
            try
            {
                ProjectCategoryModel TMProjectCategory = new ProjectCategoryModel();
                if (Session["ProjectCategory"] != null)
                {
                    TMProjectCategory = Session["ProjectCategory"] as ProjectCategoryModel;
                }
                TMProjectCategory.CategoryName = CategoryName;
                if (TMProjectCategory.ProjectList == null)
                    TMProjectCategory.ProjectList = new List<TMProjectList>();
                TMProjectList demo = new TMProjectList();
                demo.ProjectId = ProjectId;
                demo.ProjectName = ProjectName;
                TMProjectCategory.ProjectList.Add(demo);
                Session["ProjectCategory"] = TMProjectCategory;
                return RedirectToAction("CreateProjectCategory", new { ProCateId = ProCotegoryId });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult CreateProjectCategory(FormCollection fc)
        {
            ProjectCategoryModel TMProjectCategory = new ProjectCategoryModel();
            string User_Id = User.Identity.GetUserId();
            TMLeaveBL BLObj = new TMLeaveBL();
            try
            {
                if (Session["ProjectCategory"] != null)
                {
                    TMProjectCategory = Session["ProjectCategory"] as ProjectCategoryModel;
                }
                TMProjectCategory.ProjCategoryId = Convert.ToInt32(fc["ProjCategoryId"].ToString());
                TMProjectCategory.CategoryName = fc["CategoryName"].ToString();
                int errcde = BLObj.SaveProjectCategory(TMProjectCategory, User_Id);
                Session["ProjectCategory"] = null;
                if (errcde == 500002)
                {
                    return RedirectToAction("ProjectCategoryList");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("ProjectCategoryList");
        }
        #endregion Project Category

        #region Approval/reject Timesheet
        public ActionResult TMPendingTimesheetApproval()
        {
            try
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

                FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/TMPendingTimesheetApproval");
                if (FindFlag)
                {
                    List<TimesheetHistory> objmodel = new List<TimesheetHistory>();

                    TMLeaveBL objbl = new TMLeaveBL();
                    string User_Id = User.Identity.GetUserId();
                    objmodel = objbl.TMTimesheetEntryApproval(User_Id);
                    return View(objmodel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult ApproveRejectTimesheetEntry(int TimeSheetId)
        {
            List<TimeSheetViewModel> Model = new List<TimeSheetViewModel>();
            TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
            string User_Id = User.Identity.GetUserId();
            TMLeaveBL BLObj = new TMLeaveBL();
            Model = BLObj.GetTimeSheetEnterList(TimeSheetId);
            TMModel.TimeSheetId = TimeSheetId;
            ViewBag.TimeDheetDetail = Model;
            Session["TimeSheetId"] = TimeSheetId.ToString();
            return View(TMModel);
        }

        public void ExportToExcel(string GridModel)
        {

            string User_Id = User.Identity.GetUserId();
            List<CaseRiskViewModel> Model = new List<CaseRiskViewModel>();
            TMLeaveBL BLObj = new TMLeaveBL();
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekno = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            string TimeSheetId = "0";
            if (Session["TimeSheetId"] != null)
            {
                TimeSheetId = Session["TimeSheetId"] as string;
            }
            var DataSource = BLObj.GetTimeSheetEnterList(Convert.ToInt32(TimeSheetId));
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
        public ActionResult ApproveRejectTimesheetEntryDetails(int statusCode, string ApproverRemark, int TimeSheetId)
        {
            try
            {

                TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
                TMLeaveBL ObjTMBL = new TMLeaveBL();
                string User_Id = User.Identity.GetUserId();

                if (statusCode == 20)
                {
                    return RedirectToAction("TMPendingTimesheetApproval");
                }
                else
                {
                    TMModel.ApproverRemark = ApproverRemark;
                    TMModel.Status = Convert.ToInt32(statusCode);

                    if (statusCode == 24)
                        TMModel.Status = 24;
                    else
                        TMModel.Status = 25;
                }
                int errCode = ObjTMBL.UpdateTimesheetEntry(TMModel.Status, TMModel.ApproverRemark, TimeSheetId, User_Id);
                return RedirectToAction("TMPendingTimesheetApproval");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Approval/reject Timesheet

        #region  Travel Request

        public ActionResult GetTravelRequestList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/GetTravelRequestList");
            if (FindFlag)
            {
                List<ProjectTravelRequestModel> travelrequestlist = new List<ProjectTravelRequestModel>();
                try
                {
                    string User_Id = User.Identity.GetUserId();
                    TMLeaveBL ObjBL = new TMLeaveBL();
                    travelrequestlist = ObjBL.GetTimeTravelReuestlist(User_Id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(travelrequestlist);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportTravelRequestList")]
        [AcceptVerbs("POST")]
        public void ExportTravelRequestList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            TMLeaveBL ObjBL = new TMLeaveBL();
            var DataSource = ObjBL.GetTimeTravelReuestlist(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        [HttpGet]
        public ActionResult CreateTravelRequest(int RequestId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/CreateTravelRequest?RequestId=0");
            if (FindFlag)
            {
                ProjectBL Blobj = new ProjectBL();
                ProjectTravelRequestModel Obmodel = new ProjectTravelRequestModel();
                string User_Id = User.Identity.GetUserId();

                List<SysEmploy> DrpDwnEmp = new List<SysEmploy>();
                //DrpDwnEmp = objbl.EmployeeDropDown();
                //ViewBag.EmpList = new SelectList(objbl.EmployeeDropDown(), "EmpId", "EmpName");
                Obmodel.Employeest = new List<ProjectTravelRequestModel>();
                AdminBL objbl = new AdminBL();
                //Obmodel.Employeest = objbl.GetBudgetEmployeeList(User_Id); // Employee List
                //ViewBag.EmployeeList = new SelectList(budgetModel.EmployeeLst, "EmployeeId", "EmployeeName");
                ViewBag.EmpList = new SelectList(objbl.GetBudgetEmployeeList(User_Id), "EmployeeId", "EmployeeName");

                ViewBag.VendorList = new SelectList(Blobj.GetVendorListByUser(User_Id), "Id", "DisplayName");
                ViewBag.CustomerList = new SelectList(Blobj.GetCustomerListByUser(User_Id), "Id", "DisplayName");
                Obmodel.LstTravelRequestParticipant = new List<ProjectTravelRequestModel>();
                if (RequestId > 0)
                {
                    TMLeaveBL TmBL = new TMLeaveBL();
                    Obmodel = TmBL.GetProjectTravelRequestDatail(RequestId);
                    Obmodel.LstTravelRequestParticipant = TmBL.GetTimeTravelReuestParticipantlist(RequestId);
                    Obmodel.LstTravelBudget = TmBL.GetBudgetTravelReuestlist(RequestId);
                }
                return View(Obmodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateTravelRequest(FormCollection fc, HttpPostedFileBase file)
        {
            int errorcode = 0;
            try
            {
                ProjectTravelRequestModel ObjModel = new ProjectTravelRequestModel();
                string User_Id = User.Identity.GetUserId();
                TMLeaveBL ObjBL = new TMLeaveBL();
                string ftpServer = Common.GetConfigValue("FTP");
                string ftpUid = Common.GetConfigValue("FTPUid");
                string ftpPwd = Common.GetConfigValue("FTPPWD");
                string fileName = "";
                if (file != null)
                {
                    if (file != null && fc["PurposeofVisitDocs"].ToString() != "")
                    {
                        try
                        {
                            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServer + fc["PurposeofVisitDocs"]);
                            request.Method = WebRequestMethods.Ftp.DeleteFile;
                            request.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                            request.Proxy = null;
                            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                            response.Close();
                        }
                        catch (Exception ex)
                        {
                            Common.LogException(ex);
                        }                        
                    }
                }
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
                            string FileName = Convert.ToString("/TravelVisitPurposeDoc/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
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
                                    ObjModel.PurposeofVisitDocs = FileName;
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
                else
                {
                    ObjModel.PurposeofVisitDocs = fc["PurposeofVisitDocs"].ToString();
                }
                ObjModel.Description = fc["Description"].ToString();

                if (fc["RequestId"].ToString() == "" || fc["RequestId"].ToString() == null || fc["RequestId"].ToString() == "0")
                {
                    ObjModel.RequestId = 0;

                }
                else
                {

                    ObjModel.RequestId = Convert.ToInt32(fc["RequestId"].ToString());
                }

                if (fc["CustomerId"] == null || fc["CustomerId"].ToString() == "")
                {
                    ObjModel.CustomerId = 0;
                }
                else
                {
                    ObjModel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                }
                if (fc["VendorId"] == null || fc["VendorId"].ToString() == "")
                {
                    ObjModel.VendorId = 0;
                }
                else
                {
                    ObjModel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                }
                if (fc["EmpId"] == null || fc["EmpId"].ToString() == "")
                {
                    ObjModel.EmpId = 0;
                }
                else
                {
                    ObjModel.EmpId = Convert.ToInt32(fc["EmpId"].ToString());
                }
                if (fc.AllKeys.Contains("PostComment"))
                {
                    ObjModel.PostComment = fc["PostComment"].ToString();
                }               
                errorcode = ObjBL.SaveTMTravelRequest(ObjModel, User_Id);
            }
            catch (Exception ex)
            {
                TempData["Msg"] = ex;
                return RedirectToAction("GetTravelRequestList");
            }
            if (errorcode > 0)
            {
                return RedirectToAction("CreateTravelRequest", new { RequestId = errorcode });
            }
            else
            {
                TempData["Msg"] = "There are some Error Please Try Again...";
                return RedirectToAction("GetTravelRequestList");
            }
        }

        public ActionResult AddParticipantsCustomer(int RequestId)
        {

            ProjectTravelRequestModel model = new ProjectTravelRequestModel();
            model.RequestId = RequestId;
            string User_Id = User.Identity.GetUserId();
            AdminBL objbl = new AdminBL();
            List<SysEmploy> DrpDwnEmp = new List<SysEmploy>();
            DrpDwnEmp = objbl.EmployeeDropDown();
            ViewBag.Employee = new SelectList(DrpDwnEmp, "EmpId", "EmpName");

            return PartialView(model);
        }
        public ActionResult SelectedParticipantsCustomer(int RequestId)
        {

            ProjectTravelRequestModel Obmodel = new ProjectTravelRequestModel();
            TMLeaveBL TmBL = new TMLeaveBL();
            Obmodel.LstTravelRequestParticipant = TmBL.GetTimeTravelReuestParticipantlist(RequestId);
            return Json(new SelectList(Obmodel.LstTravelRequestParticipant, "ParticipantId", "Name"));
        }
        public ActionResult AddTravelRquestParticipantsDetail(string PType, string PID, int RequestId)
        {
            if (PID.ToString() == "0" || PID.ToString() == "")
            {
                return View();
            }

            TMLeaveBL obBL = new TMLeaveBL();
            string User_Id = User.Identity.GetUserId();
            string[] selitems = PID.Split(',');

            int errcode = obBL.SaveTravelRequestparticipatns(PType, selitems, RequestId, User_Id);
            if (errcode == 500001 || errcode == 500002)
            {
                Session["Participant"] = null;
                return RedirectToAction("CreateTravelRequest", new { RequestId = RequestId });
            }
            else if (errcode == 500000)
            {

                Session["Participant"] = "This Participant already added";
                return RedirectToAction("AddParticipants", new { RequestId = RequestId });
            }
            else
            {
                return RedirectToAction("CreateTravelRequest", new { RequestId = RequestId });
            }

        }

        public ActionResult DeleteTravelRequestParticipants(int RequestId, int ParticipantId, string ParticipantsType)
        {


            TMLeaveBL ObjBL = new TMLeaveBL();

            int errcode = ObjBL.DeleteTravelRequestParticipants(RequestId, ParticipantId, ParticipantsType);
            if (errcode == 500003)
            {

                return RedirectToAction("CreateTravelRequest", new { RequestId = RequestId });
            }
            return RedirectToAction("CreateTravelRequest", new { RequestId = RequestId });
        }

        public ActionResult AddTravelBudget(int RequestId, string BudgetTitle)
        {
            TravelBudgetModel modeldata = new TravelBudgetModel();

            TMLeaveBL TmBL = new TMLeaveBL();

            if (RequestId > 0)
            {
                modeldata = TmBL.GetSelectedTravelBudgetList(RequestId, BudgetTitle);
            }
            //ProjectTravelRequestModel model = new ProjectTravelRequestModel();
            //model.RequestId = RequestId;
            //string User_Id = User.Identity.GetUserId();
            //AdminBL objbl = new AdminBL();
            //List<SysEmploy> DrpDwnEmp = new List<SysEmploy>();
            //DrpDwnEmp = objbl.EmployeeDropDown();
            //ViewBag.Employee = new SelectList(DrpDwnEmp, "EmpId", "EmpName");

            return PartialView(modeldata);
        }

        [HttpPost]
        public ActionResult AddTravelBudget(HttpPostedFileBase file, FormCollection fc, int RequestId)
        {

            TravelBudgetModel Budgetmodel = new TravelBudgetModel();

            TMLeaveBL objBL = new TMLeaveBL();
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");
            string fileName = "";
            Budgetmodel.FileAttachment = fc["FileAttachment"].ToString();


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
                        string FileName = Convert.ToString("/TravelBudgetDoc/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
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

                                System.Net.FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
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
                                Budgetmodel.FileAttachment = FileName;
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
            Budgetmodel.BudgetTitle = fc["BudgetTitle"].ToString();
            Budgetmodel.Budget = Convert.ToDouble(fc["Budget"].ToString());
            Budgetmodel.Remark = fc["Remark"].ToString();
            Budgetmodel.RequestId = 0;
            if (RequestId > 0)
            {
                Budgetmodel.RequestId = RequestId;
            }
            //Budgetmodel.RequestId = Convert.ToInt32(fc["RequestId"].ToString());
            int errcode = objBL.SaveTravelBudgetRequest(Budgetmodel.FileAttachment, Budgetmodel.Remark, Budgetmodel.RequestId, Budgetmodel.BudgetTitle, Budgetmodel.Budget);
            if (errcode == 500001 || errcode == 500002)
            {

                return RedirectToAction("CreateTravelRequest", new { RequestId = Budgetmodel.RequestId });
            }
            else
            {
                if (file.ContentLength > 0)
                {
                    if (Budgetmodel.FileAttachment != null || Budgetmodel.FileAttachment != "")
                    {

                        for (int x = 0; x < 3; x++)
                        {
                            try
                            {
                                System.Net.FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(ftpServer + fileName);
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
            return View();
        }
        public ActionResult Download(int RequestId, string Filename)
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
            return RedirectToAction("CreateTravelRequest", new { RequestId = RequestId });
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
        #endregion  Travel Request

        #region Travel request Approval List
        public ActionResult TravelRequestApprovalList()
        {

            string User_Id = User.Identity.GetUserId();
            try
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

                FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/TravelRequestApprovalList");
                if (FindFlag)
                {
                    List<ProjectTravelRequestModel> list = new List<ProjectTravelRequestModel>();
                    TMLeaveBL objbl = new TMLeaveBL();
                    list = objbl.TravelRequestApprovalList(User_Id);
                    return View(list);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [System.Web.Http.ActionName("ExportTravelRequestApprovalList")]
        [AcceptVerbs("POST")]
        public void ExportTravelRequestApprovalList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            TMLeaveBL objbl = new TMLeaveBL();
            var DataSource = objbl.TravelRequestApprovalList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult TravelRequestReviewList()
        {

            string User_Id = User.Identity.GetUserId();
            try
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

                FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMLeave/TravelRequestApprovalList");
                if (FindFlag)
                {
                    List<ProjectTravelRequestModel> list = new List<ProjectTravelRequestModel>();
                    TMLeaveBL objbl = new TMLeaveBL();
                    list = objbl.TravelRequestReviewList(User_Id);
                    return View(list);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [System.Web.Http.ActionName("ExportTravelRequestReviewList")]
        [AcceptVerbs("POST")]
        public void ExportTravelRequestReviewList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            TMLeaveBL objbl = new TMLeaveBL();
            var DataSource = objbl.TravelRequestReviewList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult ApproveRejectTravel(int statusCode, string ApproverRemark, int RequestId, string Step)
        {
            try
            {
                TMLeaveBL ObjTMBL = new TMLeaveBL();
                string User_Id = User.Identity.GetUserId();
                int errCode = ObjTMBL.ApproveRejectTravel(statusCode, ApproverRemark, User_Id, RequestId, Step);
                if (Step == "Review")
                    return RedirectToAction("TravelRequestReviewList");
                else
                    return RedirectToAction("TravelRequestApprovalList");
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult RequestSendForReview(int RequestId, int statusCode)
        {
            try
            {
                string User_Id = User.Identity.GetUserId();
                TMLeaveBL objbl = new TMLeaveBL();
                int ErrorCode = objbl.RequestSendForReview(statusCode, User_Id, RequestId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("GetTravelRequestList");
        }
        #endregion Travel request Approval List
    }
}