using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SmartSys.BL.Calandar;
using Microsoft.AspNet.Identity;
using SmartSys.BL;
using System.Data;
using SmartSys.Utility;
using System.Web.Script.Serialization;
using Syncfusion.Pdf;
using Syncfusion.EJ.Export;

namespace SmartSys.Controllers
{
    public class CalendarController : Controller
    {
        List<SysEmploy> owner = new List<SysEmploy>();
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
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Calendar/Index");
            if (FindFlag)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult IndexPlan()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Calendar/IndexPlan");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                string UId = "";
                if (Session["U_Id"] != null)
                {
                    UId = Session["U_Id"] as string;
                }                                               
                List<BL.Enquiry.SubordinateListModel> SubOrdinateList = new List<BL.Enquiry.SubordinateListModel>();
                BL.Enquiry.EnquiryBL BLObj = new BL.Enquiry.EnquiryBL();                
                SubOrdinateList = BLObj.GetSubOrdinateList(User_Id);
                ViewBag.LstEmp = new SelectList(SubOrdinateList, "User_Id", "EmployeeName", UId);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ExportAsPDF(string scheduleModel)
        {
            Syncfusion.JavaScript.Models.ScheduleProperties scheduleObject = new Syncfusion.EJ.Export.SchedulePDFExport().ScheduleSerializeModel(Request.Form["ScheduleModel"]);
            scheduleObject.BlockoutSettings.DataSource = (IEnumerable)new JavaScriptSerializer().Deserialize(Request.Form["ScheduleProcesedIntervalsApps"], typeof(IEnumerable));
            IEnumerable scheduleAppointments = (IEnumerable)new JavaScriptSerializer().Deserialize(Request.Form["ScheduleProcesedApps"], typeof(IEnumerable));
            PdfPageSettings pageSettings = new PdfPageSettings(50f) { Orientation = PdfPageOrientation.Landscape };
            PdfDocument document = new PdfExport().Export(scheduleObject, scheduleAppointments, ExportTheme.FlatSaffron, Request.Form["locale"], pageSettings);
            document.Save("Schedule.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
            return RedirectToAction("PDFExportController");
        }

        public ActionResult SetUserId(string User_Id)
        {
            Session["U_Id"] = User_Id;
            return Json("SetVal", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPlanData()
        {
            string U_Id = Session["U_Id"] as string;
            string User_Id = "";
            if (U_Id != "" && U_Id != null)
            {
                User_Id = U_Id;                
            }
            else
            {
                User_Id = User.Identity.GetUserId();
            }            
            CalandarBL objbl = new CalandarBL();
            List<DefaultSchedule> lstsch = objbl.GetPlanList(User_Id);
            IEnumerable data = lstsch; // nw.Appointment.Take(5);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult ExportAsPDF(string scheduleModel)
        //{
        //    PdfExport exp = new PdfExport();
        //    PdfDocument document = exp.Export(Request.Form["ScheduleModel"], Request.Form["ScheduleProcesedApps"], "flat-saffron", Request.Form["locale"], PdfPageOrientation.Landscape);
        //    document.Save("Schedule.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
        //    return RedirectToAction("PDFExportController");
        //}

        public JsonResult GetData()
        {           
            string User_Id = User.Identity.GetUserId();                 
            CalandarBL objbl = new CalandarBL();
            List<DefaultSchedule> lstsch = objbl.GetCalenderList(User_Id);
            IEnumerable data = lstsch; // nw.Appointment.Take(5);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOwners()
        {
            AdminBL objbl = new AdminBL();
            owner = objbl.EmployeeDropDown();
            IEnumerable data = owner;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteAppontment(int id)
        {
            string User_Id = User.Identity.GetUserId();
            CalandarBL objbl = new CalandarBL();
            int errcode = objbl.DeleteCalendarAppointment(id, User_Id);
            return RedirectToAction("Index");
        }


        public JsonResult Batch(EditParams param)
        {
            string User_Id = User.Identity.GetUserId();
            List<int> lstOwnerModel = new List<int>();
            DefaultSchedule model = new DefaultSchedule();
            bool check = false;
            CalandarBL objbl = new CalandarBL();
            if (param.action == "insert" || (param.action == "batch" && param.added != null))          // this block of code will execute while inserting the appointments
            {
                if (param.added != null)
                    foreach (DefaultSchedule demo in param.added)
                    {
                        model = demo;
                        check = true;
                        if (demo.Participant != null)
                            lstOwnerModel.Add(Convert.ToInt32(demo.Participant.ToString()));
                    }
            }
            if (param.action == "update" || (param.action == "batch" && param.changed != null))          // this block of code will execute while inserting the appointments
            {
                if (param.changed != null)
                    foreach (DefaultSchedule demo in param.changed)
                    {
                        model = demo;
                        check = true;
                        if (demo.Participant != null)
                        {
                            string[] chk = demo.Participant.Split(',');
                            if (chk.Length == 1)
                            {

                                lstOwnerModel.Add(Convert.ToInt32(demo.Participant.ToString()));
                            }
                        }
                    }
            }
            else
            {
                model.Id = 0;
            }

            if (param.value != null)
            {
                if (param.key == null)
                {
                    check = true;
                    model = param.value;
                    model.Id = 0;
                }
            }
            if (param.action == "remove")
            {
                model.Id = Convert.ToInt32(param.key);
                int errcode = objbl.DeleteCalendarAppointment(model.Id, User_Id);
            }
            else
            {
                if (check)
                {
                    int errcode = objbl.SaveCalendarDetails(model, User_Id, lstOwnerModel);
                    if (errcode > 0)
                    {

                        List<string> mailInfo = new List<string>();
                        mailInfo = objbl.GetScheduleParticipendsDetailList(errcode);
                        string[] str = mailInfo[1].ToString().Split(',');
                        if (str.Length > 1)
                        {
                            string subject = "";
                            string subjectTxt = "";
                            string Lcation = "";
                            if (model.Subject == null)
                            {
                                subjectTxt = "No Subject";
                            }
                            else
                            {
                                subjectTxt = model.Subject.ToString();
                            }
                            if (param.changed == null)
                            {
                                subject = "New Appointment " + " " + subjectTxt + " ";
                            }
                            else
                            {
                                subject = "Updated Appointment " + " " + subjectTxt + " ";
                            }
                            string Name = mailInfo[0].ToString();
                            string toMail = mailInfo[1].ToString();
                            if (toMail != "")
                            {
                                string FromeDate = model.StartTime.ToLongDateString();
                                string FromeTime = model.StartTime.ToShortTimeString();
                                string ToDate = model.EndTime.ToLongDateString();
                                string ToTime = model.EndTime.ToShortTimeString();
                                if (model.Location != null)
                                    Lcation = model.Location.ToString();

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
                                           "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "From : " + FromeDate + " - " + FromeTime + "</span>" + "</div>" +
                                           "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "  To : " + ToDate + " - " + ToTime + "</span>" + "</div>" +
                                           "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + " Where :" + "</span>" + "</div>" +
                                           "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + "Location : " + Lcation + "</span>" + "</div>" +
                                           "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + " Participents :" + "</span>" + "</div>" +
                                           "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Name + "</span>" + "</div>" +
                                           "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + " Description :" + "</span>" + "</div>" +
                                           "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + model.Description + "</span>" + "</div>" +
                                           "</body>" +
                                           "</html>";
                                for (int i = 0; i < 3; i++)
                                {
                                    try
                                    {
                                        int mail = Utility.Common.SendMail(subject, MailBody, "", toMail, "", null);
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        if (i == 2)
                                        {

                                            Common.LogException(ex);
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            CalandarBL objbl1 = new CalandarBL();
            List<DefaultSchedule> lstsch = objbl1.GetCalenderList(User_Id);
            IEnumerable data = lstsch; // nw.Appointment.Take(5);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}