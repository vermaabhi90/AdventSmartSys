using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.Tmleave;
using SmartSys.BL.TMPlan;
using SmartSys.Utility;
using Syncfusion.EJ.Export;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SmartSys.Controllers
{
    public class TMPlanController : Controller
    {
        public ActionResult Index(int EmpCode)
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

                FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMPlan/Index?EmpCode=0");
                if (FindFlag)
                {
                    if (Session["PlanError"] != null)
                    {
                        TempData["Message"] = Session["PlanError"] as string;
                    }
                    Session["PlanError"] = null;
                    TMPlanModel objmodel = new TMPlanModel();
                    TMPlanBL objbl = new TMPlanBL();
                    string User_Id = User.Identity.GetUserId();
                    objmodel.LstEmployee = objbl.GetTMPlanEmplist(User_Id);

                    List<TMPlanModel> CalLst = new List<TMPlanModel>();
                    CalLst = objbl.GetTMPlanCalLst(User_Id);
                    ViewBag.PlanCalList = CalLst;

                    List<EmpSubordinatesModel> EMPSubordinatesList = new List<EmpSubordinatesModel>();
                    EMPSubordinatesList = objbl.EmpSubordinatesList(User_Id);
                    ViewBag.EmpSubordinatesList = new SelectList(EMPSubordinatesList, "EmpId", "EmpName");

                    string TxtParamValue = "";
                    objmodel.ParamId = "@EmpCode";
                    objmodel.ParamName = "@EmpCode";
                    objmodel.hidText = "";
                    objmodel.ReportId = "MGMT010";
                    objmodel.TxtParamValue = TxtParamValue;
                    ReportNameModel Model = new ReportNameModel();
                    Model = objbl.GetOpenReport(User_Id, EmpCode);
                    if (Model.ReportId != null)
                    {
                        objmodel.OutputLocation = Model.OutputLocation;
                        objmodel.RunDate = Model.RunDate;
                        objmodel.RunEmpName = Model.RunEmpName;
                        objmodel.StatusId = Model.StatusId;
                        objmodel.ReportId = Model.ReportId;
                        objmodel.TxtParamValue = TxtParamValue;
                    }

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
        [System.Web.Http.ActionName("ExportPlanLst")]
        [AcceptVerbs("POST")]
        public void ExportPlanLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            TMPlanBL objbl = new TMPlanBL();
            var DataSource = objbl.GetTMPlanCalLst(User_Id); 
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        private Syncfusion.JavaScript.Models.GridProperties ConvertGridObject(string gridProperty)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IEnumerable div = (IEnumerable)serializer.Deserialize(gridProperty, typeof(IEnumerable));
            Syncfusion.JavaScript.Models.GridProperties gridProp = new Syncfusion.JavaScript.Models.GridProperties();
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
        public ActionResult DeletePlan(int PlanId)
        {
            TMPlanBL objbl = new TMPlanBL();
            string User_id = User.Identity.GetUserId();
            int errCode = objbl.DeletePlan(PlanId, User_id);
            return RedirectToAction("IndexPlan", "Calendar");
        }
        public JsonResult PlanTasklist(int ProjectId, string User_Id)
        {
            TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
            try
            {
                TMLeaveBL BLObj = new TMLeaveBL();
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

        public ActionResult CreateNewPlan()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMPlan/Index?EmpCode=0");
            if (FindFlag)
            {
                DataSet ds = new DataSet();
                TMPlanModel objmodel = new TMPlanModel();
                TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
                TMLeaveBL BLObj = new TMLeaveBL();
                TMPlanBL objbl = new TMPlanBL();
                string User_Id = User.Identity.GetUserId();

                ds = objbl.PlanGetEmpDetail(0, User_Id);

                if (ds != null && ds.Tables.Count > 0)
                {
                    objmodel.EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                    objmodel.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                    objmodel.User_Id = ds.Tables[0].Rows[0]["User_Id"].ToString();
                }
                return RedirectToAction("CreatePlan", new { EmpID = objmodel.EmpId, PlanID = 0 });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult CreatePlan(int EmpID, int PlanID)
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

                FindFlag = modelObj.FindMenu(lstTaskmodel, "/TMPlan/Index?EmpCode=0");
                if (FindFlag)
                {
                    DataSet ds = new DataSet();
                    TMPlanModel objmodel = new TMPlanModel();
                    TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
                    TMLeaveBL BLObj = new TMLeaveBL();
                    TMPlanBL objbl = new TMPlanBL();
                    string User_Id = User.Identity.GetUserId();
                    objmodel = objbl.GetTMPlanSelectedList(PlanID);
                    if (PlanID > 0)
                    {
                        ds = objbl.PlanGetEmpDetail(objmodel.EmpId, User_Id);

                    }
                    else
                    {
                        ds = objbl.PlanGetEmpDetail(EmpID, User_Id);
                    }
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            objmodel.EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                            objmodel.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                            objmodel.User_Id = ds.Tables[0].Rows[0]["User_Id"].ToString();
                        }
                    }
                    if (objmodel.EmpId == 0)
                    {
                        Session["PlanError"] = "Employee Do not have a User Id";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        EmpID = objmodel.EmpId;
                        TMModel.TMProjectlist = BLObj.GetProjectList(objmodel.User_Id);
                        ViewBag.TMProjectlist = new SelectList(TMModel.TMProjectlist, "ProjectId", "ProjectName");

                        TMModel.TMTasklist = BLObj.GetTaskList(objmodel.ProjectId, objmodel.User_Id);
                        ViewBag.TMTasklist = new SelectList(TMModel.TMTasklist, "TaskId", "TaskName");
                        return View(objmodel);
                    }
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
        [HttpPost]
        public ActionResult CreatePlan(FormCollection fc, Boolean AllDay, Boolean Repete, Boolean Never)
        {
            TMPlanModel objmodel = new TMPlanModel();
            TMPlanBL BLObj = new TMPlanBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                objmodel.PlanId = Convert.ToInt32(fc["PlanId"].ToString());
                objmodel.EmpId = Convert.ToInt32(fc["EmpId"].ToString());
                objmodel.ProjectId = Convert.ToInt32(fc["ProjectId"].ToString());
                objmodel.TaskId = Convert.ToInt32(fc["TaskId"].ToString());
                objmodel.StartDate = Convert.ToDateTime(fc["dtpStartDate"].ToString());
                objmodel.EndDate = Convert.ToDateTime(fc["dtpEndDate"].ToString());
                objmodel.AllDay = AllDay;
                objmodel.Repete = Repete;
                objmodel.Never = Never;
                objmodel.Remark = fc["Remark"].ToString();
                objmodel.FrequencyType = Convert.ToChar(fc["FrequencyType"].ToString());
                objmodel.WeekNo = Convert.ToInt32(fc["WeekNo"].ToString());
                objmodel.MonthNo = Convert.ToInt32(fc["MonthNo"].ToString());
                if (fc["OnDate"].ToString() != "")
                    objmodel.OnDate = Convert.ToDateTime(fc["OnDate"].ToString());
                StringBuilder strFrequency = new StringBuilder("");
                StringBuilder strRecurrenceRule = new StringBuilder("");
                StringBuilder strBYDAY = new StringBuilder("");
                if (fc["Sunday"].Contains("true"))
                {
                    strFrequency.Append("1");
                    strBYDAY.Append("SU,");
                }
                else
                {
                    strFrequency.Append("0");
                }
                if (fc["Monday"].Contains("true"))
                {
                    strFrequency.Append("1");
                    strBYDAY.Append("MO,");
                }
                else
                {
                    strFrequency.Append("0");
                }
                if (fc["TuesDay"].Contains("true"))
                {
                    strFrequency.Append("1");
                    strBYDAY.Append("TU,");
                }
                else
                {
                    strFrequency.Append("0");
                }
                if (fc["Wednesday"].Contains("true"))
                {
                    strFrequency.Append("1");
                    strBYDAY.Append("WE,");
                }
                else
                {
                    strFrequency.Append("0");
                }
                if (fc["Thursday"].Contains("true"))
                {
                    strFrequency.Append("1");
                    strBYDAY.Append("TH,");
                }
                else
                {
                    strFrequency.Append("0");
                }
                if (fc["Friday"].Contains("true"))
                {
                    strFrequency.Append("1");
                    strBYDAY.Append("FR,");
                }
                else
                {
                    strFrequency.Append("0");
                }
                if (fc["Saturday"].Contains("true"))
                {
                    strFrequency.Append("1");
                    strBYDAY.Append("SA,");
                }
                else
                {
                    strFrequency.Append("0");
                }
                if (Repete.ToString() == "True")
                {
                    switch (objmodel.FrequencyType)
                    {
                        case 'D':
                            {
                                strRecurrenceRule.Append("FREQ=DAILY;INTERVAL=1");
                                if (!Never)
                                {
                                    TimeSpan difference = objmodel.OnDate.Date - objmodel.StartDate.Date;
                                    int days = ((int)difference.TotalDays) + 1;
                                    strRecurrenceRule.Append(";COUNT=" + days.ToString());
                                }
                                objmodel.RecurrenceRule = strRecurrenceRule.ToString();
                                break;
                            }
                        case 'W':
                            {
                                string BYDAY = strBYDAY.ToString();
                                BYDAY = BYDAY.Substring(0, strBYDAY.Length - 1);
                                strRecurrenceRule.Append("FREQ=WEEKLY;INTERVAL=1");
                                strRecurrenceRule.Append(";BYDAY=" + BYDAY);
                                int count = 0;
                                if (!Never)
                                {
                                    int i = 0;
                                    Char[] WeekFrq = strFrequency.ToString().ToCharArray();
                                    for (i = 0; i < 7; i++)
                                    {
                                        if (WeekFrq[i].ToString() == "1")
                                        {
                                            switch (i)
                                            {
                                                case 0:
                                                    {
                                                        count = count + Common.CountDays(DayOfWeek.Sunday, objmodel.StartDate, objmodel.OnDate.Date);
                                                        break;
                                                    }
                                                case 1:
                                                    {
                                                        count = count + Common.CountDays(DayOfWeek.Monday, objmodel.StartDate, objmodel.OnDate.Date);
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        count = count + Common.CountDays(DayOfWeek.Tuesday, objmodel.StartDate, objmodel.OnDate.Date);
                                                        break;
                                                    }
                                                case 3:
                                                    {
                                                        count = count + Common.CountDays(DayOfWeek.Wednesday, objmodel.StartDate, objmodel.OnDate.Date);
                                                        break;
                                                    }
                                                case 4:
                                                    {
                                                        count = count + Common.CountDays(DayOfWeek.Thursday, objmodel.StartDate, objmodel.OnDate.Date);
                                                        break;
                                                    }
                                                case 5:
                                                    {
                                                        count = count + Common.CountDays(DayOfWeek.Friday, objmodel.StartDate, objmodel.OnDate.Date);
                                                        break;
                                                    }
                                                case 6:
                                                    {
                                                        count = count + Common.CountDays(DayOfWeek.Saturday, objmodel.StartDate, objmodel.OnDate.Date);
                                                        break;
                                                    }

                                            }
                                        }
                                    }
                                    count = count + 1;
                                    strRecurrenceRule.Append(";COUNT=" + count.ToString());
                                }
                                objmodel.RecurrenceRule = strRecurrenceRule.ToString();
                                break;
                            }
                        case 'M':
                            {
                                string BYDAY = fc["WeekMonth"].ToString();
                                strRecurrenceRule.Append("FREQ=MONTHLY;INTERVAL=1");
                                strRecurrenceRule.Append(";BYDAY=" + BYDAY);
                                strRecurrenceRule.Append(";BYSETPOS=" + objmodel.WeekNo);
                                DateTime StertTemp = new DateTime();
                                DateTime OndateTemp = new DateTime();

                                if (!Never)
                                {
                                    switch (BYDAY)
                                    {
                                        case "MO":
                                            {
                                                StertTemp = Utility.Common.NthOf(objmodel.StartDate, objmodel.WeekNo, DayOfWeek.Monday);
                                                OndateTemp = Utility.Common.NthOf(objmodel.OnDate, objmodel.WeekNo, DayOfWeek.Monday);
                                                break;
                                            }
                                        case "TU":
                                            {
                                                StertTemp = Utility.Common.NthOf(objmodel.StartDate, objmodel.WeekNo, DayOfWeek.Tuesday);
                                                OndateTemp = Utility.Common.NthOf(objmodel.OnDate, objmodel.WeekNo, DayOfWeek.Tuesday);
                                                break;
                                            }
                                        case "WE":
                                            {
                                                StertTemp = Utility.Common.NthOf(objmodel.StartDate, objmodel.WeekNo, DayOfWeek.Wednesday);
                                                OndateTemp = Utility.Common.NthOf(objmodel.OnDate, objmodel.WeekNo, DayOfWeek.Wednesday);
                                                break;
                                            }
                                        case "TH":
                                            {
                                                StertTemp = Utility.Common.NthOf(objmodel.StartDate, objmodel.WeekNo, DayOfWeek.Thursday);
                                                OndateTemp = Utility.Common.NthOf(objmodel.OnDate, objmodel.WeekNo, DayOfWeek.Thursday);
                                                break;
                                            }
                                        case "FR":
                                            {
                                                StertTemp = Utility.Common.NthOf(objmodel.StartDate, objmodel.WeekNo, DayOfWeek.Friday);
                                                OndateTemp = Utility.Common.NthOf(objmodel.OnDate, objmodel.WeekNo, DayOfWeek.Friday);
                                                break;
                                            }
                                        case "SA":
                                            {
                                                StertTemp = Utility.Common.NthOf(objmodel.StartDate, objmodel.WeekNo, DayOfWeek.Saturday);
                                                OndateTemp = Utility.Common.NthOf(objmodel.OnDate, objmodel.WeekNo, DayOfWeek.Saturday);
                                                break;
                                            }
                                        case "SU":
                                            {
                                                StertTemp = Utility.Common.NthOf(objmodel.StartDate, objmodel.WeekNo, DayOfWeek.Sunday);
                                                OndateTemp = Utility.Common.NthOf(objmodel.OnDate, objmodel.WeekNo, DayOfWeek.Sunday);
                                                break;
                                            }
                                    }

                                    int difference = (objmodel.OnDate.Month - objmodel.StartDate.Month) + 1;
                                    if (objmodel.StartDate > StertTemp)
                                    {
                                        difference = difference - 1;
                                    }
                                    if (objmodel.OnDate < OndateTemp && objmodel.OnDate.Month != OndateTemp.Month)
                                    {
                                        difference = difference - 1;
                                    }
                                    strRecurrenceRule.Append(";COUNT=" + difference.ToString());
                                }
                                objmodel.RecurrenceRule = strRecurrenceRule.ToString();
                                break;
                            }
                        case 'Y':
                            {
                                int MoNo = objmodel.MonthNo - 1;
                                string BYDAY = fc["WeekMonth"].ToString();
                                strRecurrenceRule.Append("FREQ=YEARLY;INTERVAL=1");
                                strRecurrenceRule.Append(";BYDAY=" + BYDAY);
                                strRecurrenceRule.Append(";BYSETPOS=" + objmodel.WeekNo);
                                strRecurrenceRule.Append(";BYMONTH=" + MoNo);
                                if (!Never)
                                {
                                    DateTime dt = DateTime.Now;
                                    int difference = (objmodel.OnDate.Year - objmodel.StartDate.Year) + 1;
                                    strRecurrenceRule.Append(";COUNT=" + difference.ToString());
                                }
                                objmodel.RecurrenceRule = strRecurrenceRule.ToString();
                                break;
                            }
                    }
                }
                else
                {
                    objmodel.RecurrenceRule = "";
                    objmodel.Repete = false;
                }
                if (objmodel.RecurrenceRule == null)
                {
                    objmodel.RecurrenceRule = "";
                    objmodel.Repete = false;
                }
                objmodel.WeekDays = strFrequency.ToString();
                int errCode = BLObj.saveTMPlanDetails(objmodel, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Redirect("/Calendar/IndexPlan");
        }

        public ActionResult ApprovePlan(int EmpID, int PlanID)
        {
            TMPlanModel objmodel = new TMPlanModel();
            try
            {
                DataSet ds = new DataSet();
                TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
                TMLeaveBL BLObj = new TMLeaveBL();
                TMPlanBL objbl = new TMPlanBL();
                string User_Id = User.Identity.GetUserId();
                objmodel = objbl.GetTMPlanSelectedList(PlanID);
                if (PlanID > 0)
                {
                    ds = objbl.PlanGetEmpDetail(objmodel.EmpId, User_Id);

                }
                else
                {
                    ds = objbl.PlanGetEmpDetail(EmpID, User_Id);
                }
                if (ds != null && ds.Tables.Count > 0)
                {
                    objmodel.EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                    objmodel.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                    objmodel.User_Id = ds.Tables[0].Rows[0]["User_Id"].ToString();
                }
                EmpID = objmodel.EmpId;
                TMModel.TMProjectlist = BLObj.GetProjectList(objmodel.User_Id);
                ViewBag.TMProjectlist = new SelectList(TMModel.TMProjectlist, "ProjectId", "ProjectName");

                TMModel.TMTasklist = BLObj.GetTaskList(objmodel.ProjectId, objmodel.User_Id);
                ViewBag.TMTasklist = new SelectList(TMModel.TMTasklist, "TaskId", "TaskName");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return PartialView(objmodel);
        }

        [HttpPost]
        public ActionResult ApprovePlan(FormCollection fc)
        {
            TMPlanModel objmodel = new TMPlanModel();
            TMPlanBL BLObj = new TMPlanBL();
            string User_Id = User.Identity.GetUserId();
            int errCode = 0;
            try
            {
                objmodel.PlanId = Convert.ToInt32(fc["PlanId"].ToString());
                objmodel.statusId = Convert.ToInt32(fc["statusId"].ToString());
                errCode = BLObj.SaveTMPlanStatus(objmodel.PlanId, objmodel.statusId, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("Index");
        }
    }
}