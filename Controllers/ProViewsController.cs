using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.BOMAdmin;
using SmartSys.BL.DW;
using SmartSys.BL.Enquiry;
using SmartSys.BL.Project;
using SmartSys.BL.ProViews;
using SmartSys.BL.TimeManagement;
using SmartSys.BL.Tmleave;
using SmartSys.Utility;
using Syncfusion.DocIO.DLS;
using Syncfusion.EJ.Export;
using Syncfusion.JavaScript.Models;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SmartSys.Controllers
{
    public class ProViewsController : Controller

    {
        public string Filter { get; set; }

        public ActionResult CustomerEnquiryTracking(int EnqId)
        {
            if (Session["EnqList"] != null)
            {
                Session["EnqList"] = null;
            }
            string User_Id = User.Identity.GetUserId();
            EnquiryTrackingViewDetail MailEnqTimeDifference = new EnquiryTrackingViewDetail();
            ViewsBL BLObj = new ViewsBL();
            List<EnquiryTrackingViewDetail> EMailEnqTeackingLst = new List<EnquiryTrackingViewDetail>();
            if (EnqId > 0)
            {
                int errCode = BLObj.CheckEnquirytoPerson(EnqId, User_Id);
                if (errCode == 50001)
                {
                    EnqId = 0;
                    TempData["Message"] = "You Do have Access to this Enquiry.";
                }
            }
            MailEnqTimeDifference = BLObj.MailEnqTimeDifference(EnqId, "Mgmt");
            ViewBag.test = MailEnqTimeDifference.TestLst;
            MailEnqTimeDifference.EnqStatusList = BLObj.EnqStatusList(EnqId);
            Session["EnqList"] = MailEnqTimeDifference;
            return View(MailEnqTimeDifference);
        }
        public ActionResult CustomerEnquiryTrackingForSales(int EnqId)
        {
            if (Session["EnqList"] != null)
            {
                Session["EnqList"] = null;
            }
            string User_Id = User.Identity.GetUserId();
            EnquiryTrackingViewDetail MailEnqTimeDifference = new EnquiryTrackingViewDetail();
            ViewsBL BLObj = new ViewsBL();
            if (EnqId > 0)
            {
                int errCode = BLObj.CheckEnquirytoPerson(EnqId, User_Id);
                if (errCode == 50001)
                {
                    EnqId = 0;
                    TempData["Message"] = "You Do have Access to this Enquiry.";
                }
            }
            List<EnquiryTrackingViewDetail> EMailEnqTeackingLst = new List<EnquiryTrackingViewDetail>();
            MailEnqTimeDifference = BLObj.MailEnqTimeDifference(EnqId, "Sales");
            ViewBag.test = MailEnqTimeDifference.TestLst;
            MailEnqTimeDifference.EnqStatusList = BLObj.EnqStatusList(EnqId);
            Session["EnqList"] = MailEnqTimeDifference;
            return View(MailEnqTimeDifference);
        }
        public ActionResult EnquiryTrackingView(int EnqId)
        {
            MailEnquiryTimespan mailenqtimespan = new MailEnquiryTimespan();
            ViewsBL BLObj = new ViewsBL();
            List<MailEnquiryTimespan> MailEnqTimespanLst = new List<MailEnquiryTimespan>();
            mailenqtimespan = BLObj.MailEnqTimespan(EnqId);
            // mailenqtimespan.MinuteDiff = 0;
            //if (Session["EnqId"]!=null)
            //{
            //    int Time = (int)Session["EnqId"];
            //    mailenqtimespan.MinuteDiff = Time;
            //}
            return View(mailenqtimespan);
        }
        public ActionResult EnquiryTrackingViewDetail(int EnqId)
        {
            if (Session["EnqList"] != null)
            {
                Session["EnqList"] = null;
            }
            string User_Id = User.Identity.GetUserId();
            EnquiryTrackingViewDetail MailEnqTimeDifference = new EnquiryTrackingViewDetail();
            ViewsBL BLObj = new ViewsBL();
            List<EnquiryTrackingViewDetail> EMailEnqTeackingLst = new List<EnquiryTrackingViewDetail>();
            MailEnqTimeDifference = BLObj.MailEnqTimeDifference(EnqId, "");
            ViewBag.test = MailEnqTimeDifference.TestLst;
            Session["EnqList"] = MailEnqTimeDifference;
            return View(MailEnqTimeDifference);
        }

        public ActionResult FindEnquiryDetail(string Type, string Dep)
        {
            EnquiryModel EnquModel = new EnquiryModel();
            try
            {
                List<CustomerDrpModel> CustomerLst = new List<CustomerDrpModel>();
                List<QoutFollowupModel> QuotList = new List<QoutFollowupModel>();
                EnquModel.lstEnquiry = new List<EnquiryModel>();
                ViewBag.EnqList = EnquModel.lstEnquiry;
                ViewBag.QuotLst = QuotList;
                EnquModel.Types = Type;
                EnquModel.Dep = Dep;
                ViewsBL BLObj = new ViewsBL();
                string User_Id = User.Identity.GetUserId();
                CustomerLst = BLObj.CustomerListBySalesPerson(User_Id);
                ViewBag.LstCust = new SelectList(CustomerLst, "CustomerId", "CustomerName");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return PartialView(EnquModel);
        }
        public ActionResult GetFindData(string Type, int CustomerId)
        {
            List<QoutFollowupModel> QuotList = new List<QoutFollowupModel>();
            List<EnquiryModel> EnqList = new List<EnquiryModel>();
            ViewsBL BLObj = new ViewsBL();
            try
            {
                if (Type == "Quot")
                {
                    QuotList = BLObj.GetFindQuotationData(Type, CustomerId);
                    return Json(QuotList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    EnqList = BLObj.GetFindEnqData(Type, CustomerId);
                    return Json(EnqList, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("CustomerEnquiryTracking", new { EnqId = 0 });
        }
        #region Enquiry Data Level View
        public ActionResult EnqTrackGraph(int id, int DataLevel)
        {
            EnquiryTrackingViewDetail MailEnqTimeDifference = new EnquiryTrackingViewDetail();
            MailEnqTimeDifference = Session["EnqList"] as EnquiryTrackingViewDetail;
            MailEnqTimeDifference.id = id;
            return PartialView(MailEnqTimeDifference);
        }
        public ActionResult DataLevelOne(int EnqId, int DataLevel, string description, int id)
        {
            ViewsBL BLObj = new ViewsBL();
            EnquiryModel EnqDetailModel = new EnquiryModel();
            EnqDetailModel = BLObj.DataLevelOne(EnqId, DataLevel);
            EnqDetailModel.Description = description;
            ViewBag.Data = EnqDetailModel;

            //EnquiryTrackingViewDetail MailEnqTimeDifference = new EnquiryTrackingViewDetail();
            //MailEnqTimeDifference = Session["EnqList"] as EnquiryTrackingViewDetail;
            //MailEnqTimeDifference.id = id;
            return PartialView(EnqDetailModel);
        }
        public ActionResult DataLevelTwo(int EnqId, int DataLevel, string description, int SourceId)
        {
            DataLevelTwo DTLevelTwoModel = new DataLevelTwo();
            ViewsBL BLObj = new ViewsBL();
            DTLevelTwoModel = BLObj.DataLevelTwo(EnqId, DataLevel, SourceId);
            DTLevelTwoModel.Description = description;
            return PartialView(DTLevelTwoModel);
        }
        public ActionResult DataLevelThree(int EnqId, int DataLevel, string description, int SourceId)
        {
            DataLevelThree DTLevelThreeModel = new DataLevelThree();
            ViewsBL BLObj = new ViewsBL();
            DTLevelThreeModel = BLObj.DataLevelThree(EnqId, DataLevel, SourceId);
            DTLevelThreeModel.Description = description;
            return PartialView(DTLevelThreeModel);
        }
        public ActionResult DataLevelFour(int EnqId, int DataLevel, string description, int SourceId)
        {
            DataLevelFour DTLevelFourModel = new DataLevelFour();
            ViewsBL BLObj = new ViewsBL();
            DTLevelFourModel = BLObj.DataLevelFour(EnqId, DataLevel, SourceId);
            DTLevelFourModel.Description = description;
            return PartialView(DTLevelFourModel);
        }
        public ActionResult DataLevelFive(int EnqId, int DataLevel, string description, int SourceId)
        {
            DataLevelFive DTLevelTwoModel = new DataLevelFive();
            ViewsBL BLObj = new ViewsBL();
            DTLevelTwoModel = BLObj.DataLevelFive(EnqId, 2, SourceId);
            DTLevelTwoModel.Description = description;
            return PartialView(DTLevelTwoModel);
        }
        public ActionResult DataLevelSix(int EnqId, int DataLevel, string description, int SourceId, int Sid)
        {
            DataLevelSix DTLevelSixModel = new DataLevelSix();
            ViewsBL BLObj = new ViewsBL();
            DTLevelSixModel = BLObj.DataLevelSix(EnqId, DataLevel, SourceId, Sid);
            DTLevelSixModel.Description = description;
            return PartialView(DTLevelSixModel);
        }
        public ActionResult DataLevelSeven(int EnqId, int DataLevel, string description, int SourceId, int Sid)
        {
            DataLevelSeven DTLevelSevenModel = new DataLevelSeven();
            ViewsBL BLObj = new ViewsBL();
            DTLevelSevenModel = BLObj.DataLevelSeven(EnqId, DataLevel, SourceId, Sid);
            DTLevelSevenModel.Description = description;
            return PartialView(DTLevelSevenModel);
        }
        public ActionResult DataLevelEight(int EnqId, int DataLevel, string description, int SourceId, int Sid)
        {
            DataLevelEight DTLevelEightModel = new DataLevelEight();
            ViewsBL BLObj = new ViewsBL();
            DTLevelEightModel = BLObj.DataLevelEight(EnqId, DataLevel, SourceId, Sid);
            DTLevelEightModel.Description = description;
            return PartialView(DTLevelEightModel);
        }
        public ActionResult DataLevelNine(int EnqId, int DataLevel, string description, int SourceId, int Sid)
        {
            DataLevelNine DTLevelNineModel = new DataLevelNine();
            ViewsBL BLObj = new ViewsBL();
            DTLevelNineModel = BLObj.DataLevelNine(EnqId, DataLevel, SourceId, Sid);
            DTLevelNineModel.Description = description;
            return PartialView(DTLevelNineModel);
        }
        public ActionResult DataLevelTen(int EnqId, int DataLevel, string description, int SourceId, int Sid)
        {
            DataLevelTen LevelFourModel = new DataLevelTen();
            ViewsBL BLObj = new ViewsBL();
            LevelFourModel = BLObj.DataLevelTen(EnqId, DataLevel, SourceId, Sid);
            LevelFourModel.Description = description;
            return PartialView(LevelFourModel);
        }
        public ActionResult DataLevelEleven(int EnqId, int DataLevel, string description, int SourceId, int Sid)
        {
            DataLevelEleven DTLevelEleven = new DataLevelEleven();
            ViewsBL BLObj = new ViewsBL();
            DTLevelEleven = BLObj.DataLevelEleven(EnqId, DataLevel, SourceId, Sid);
            DTLevelEleven.Description = description;
            return PartialView(DTLevelEleven);
        }
        public ActionResult DataLevelTwelve(int EnqId, int DataLevel, string description, int SourceId, int Sid)
        {
            DataLevelTwelve DTLevelTwelve = new DataLevelTwelve();
            ViewsBL BLObj = new ViewsBL();
            DTLevelTwelve = BLObj.DataLevelTwelve(EnqId, DataLevel, SourceId, Sid);
            DTLevelTwelve.Description = description;
            return PartialView(DTLevelTwelve);
        }
        public ActionResult DataLevelThirteen(int EnqId, int DataLevel, string description, int SourceId, int Sid)
        {
            DataLevelthirteen LevelThirteenModel = new DataLevelthirteen();
            ViewsBL BLObj = new ViewsBL();
            LevelThirteenModel = BLObj.DataLevelThirteen(EnqId, DataLevel, SourceId, Sid);
            LevelThirteenModel.Description = description;
            return PartialView(LevelThirteenModel);
        }
        public ActionResult DataLevelFourteen(int EnqId, int DataLevel, string description, int SourceId, int Sid)
        {
            DataLevelFourteen LevelFourteenModel = new DataLevelFourteen();
            ViewsBL BLObj = new ViewsBL();
            LevelFourteenModel = BLObj.DataLevelFourteen(EnqId, DataLevel, SourceId, Sid);
            LevelFourteenModel.Description = description;
            return PartialView(LevelFourteenModel);
        }
        public ActionResult DataLevelfifteen(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelfifteen LevelfifteenModel = new DataLevelfifteen();
            ViewsBL BLObj = new ViewsBL();
            LevelfifteenModel = BLObj.DataLevelfifteen(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelfifteenModel);
        }
        public ActionResult DataLevelSixteen(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelSixteen LevelSixteenModel = new DataLevelSixteen();
            ViewsBL BLObj = new ViewsBL();
            LevelSixteenModel = BLObj.DataLevelSixteen(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelSixteenModel);
        }
        public ActionResult DataLevelSeventeen(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelSeventeen LevelSeventeenModel = new DataLevelSeventeen();
            ViewsBL BLObj = new ViewsBL();
            LevelSeventeenModel = BLObj.DataLevelSeventeen(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelSeventeenModel);
        }
        public ActionResult DataLevelEighteen(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelEighteen LevelEighteenModel = new DataLevelEighteen();
            ViewsBL BLObj = new ViewsBL();
            LevelEighteenModel = BLObj.DataLevelEighteen(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelEighteenModel);
        }
        public ActionResult DataLevelNineteen(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelNineteen LevelNineteenModel = new DataLevelNineteen();
            ViewsBL BLObj = new ViewsBL();
            LevelNineteenModel = BLObj.DataLevelNineteen(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelNineteenModel);
        }
        public ActionResult DataLevelTwenty(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwenty LevelTwentyModel = new DataLevelTwenty();
            ViewsBL BLObj = new ViewsBL();
            LevelTwentyModel = BLObj.DataLevelTwenty(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelTwentyModel);
        }
        public ActionResult DataLevelTwentyOne(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyOne LevelTwentyOneModel = new DataLevelTwentyOne();
            ViewsBL BLObj = new ViewsBL();
            LevelTwentyOneModel = BLObj.DataLevelTwentyOne(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelTwentyOneModel);
        }
        public ActionResult DataLevelTwentyTwo(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyTwo LevelTwentyTwoModel = new DataLevelTwentyTwo();
            ViewsBL BLObj = new ViewsBL();
            LevelTwentyTwoModel = BLObj.DataLevelTwentyTwo(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelTwentyTwoModel);
        }
        public ActionResult DataLevelTwentyThree(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyThree LevelTwentythreeModel = new DataLevelTwentyThree();
            ViewsBL BLObj = new ViewsBL();
            LevelTwentythreeModel = BLObj.DataLevelTwentyThree(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelTwentythreeModel);
        }
        public ActionResult DataLevelTwentyFour(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyFour LevelTwentyFourModel = new DataLevelTwentyFour();
            ViewsBL BLObj = new ViewsBL();
            LevelTwentyFourModel = BLObj.DataLevelTwentyFour(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelTwentyFourModel);
        }
        public ActionResult DataLevelTwentyFive(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyFive LevelTwentyFiveModel = new DataLevelTwentyFive();
            ViewsBL BLObj = new ViewsBL();
            LevelTwentyFiveModel = BLObj.DataLevelTwentyFive(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelTwentyFiveModel);
        }
        public ActionResult DataLevelTwentySix(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentySix LevelTwentySixModel = new DataLevelTwentySix();
            ViewsBL BLObj = new ViewsBL();
            LevelTwentySixModel = BLObj.DataLevelTwentySix(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelTwentySixModel);
        }
        public ActionResult DataLevelTwentySeven(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentySeven LevelTwentySevenModel = new DataLevelTwentySeven();
            ViewsBL BLObj = new ViewsBL();
            LevelTwentySevenModel = BLObj.DataLevelTwentySeven(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelTwentySevenModel);
        }
        public ActionResult DataLevelTwentyEight(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyEight LevelTwentyEightModel = new DataLevelTwentyEight();
            ViewsBL BLObj = new ViewsBL();
            LevelTwentyEightModel = BLObj.DataLevelTwentyEight(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelTwentyEightModel);
        }
        public ActionResult DataLevelTwentyNine(int EnqId, int DataLevel, int SourceId, int Sid)
        {
            DataLevelTwentyNine LevelTwentyNineModel = new DataLevelTwentyNine();
            ViewsBL BLObj = new ViewsBL();
            LevelTwentyNineModel = BLObj.DataLevelTwentyNine(EnqId, DataLevel, SourceId, Sid);
            return PartialView(LevelTwentyNineModel);
        }
        #endregion Enquiry Data Level View 

        //public ActionResult GetCaseRiskList()
        //{
        //    List<CaseRiskViewModel> Model = new List<CaseRiskViewModel>();
        //    try
        //    {
        //        string User_Id = User.Identity.GetUserId();
        //        ViewsBL BLObj = new ViewsBL();
        //        Model = BLObj.GetCaseRiskList(User_Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return View(Model);
        //}
        public ActionResult GetCaseRiskList(string Filter)
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProViews/GetCaseRiskList");
            if (FindFlag)
            {
                CaseRiskViewModel Model = new CaseRiskViewModel();
                try
                {
                    string User_Id = User.Identity.GetUserId();
                    ViewsBL BLObj = new ViewsBL();
                    if (Filter == null)
                        Filter = "";
                    Model.LstCaseRisk = BLObj.GetCaseRiskList(User_Id, Filter);
                    Model.FilterType = Filter;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(Model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [System.Web.Http.ActionName("ExportToExcelCaseRiskList")]
        [AcceptVerbs("POST")]
        public void ExportToExcelCaseRiskList(string GridModel)
        {
            Filter = "";
            string User_Id = User.Identity.GetUserId();
            List<CaseRiskViewModel> Model = new List<CaseRiskViewModel>();
            ViewsBL BLObj = new ViewsBL();
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekno = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            string empId = "0";
            if (Session["TMEmpId"] != null)
            {
                empId = Session["TMEmpId"] as string;
            }
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = BLObj.GetCaseRiskList(User_Id, Filter);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult GetApprovedCaseRiskListByUser()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProViews/GetApprovedCaseRiskListByUser");
            if (FindFlag)
            {
                List<CaseRiskViewModel> Model = new List<CaseRiskViewModel>();
                try
                {
                    string User_Id = User.Identity.GetUserId();
                    ViewsBL BLObj = new ViewsBL();
                    Model = BLObj.GetApprovedCaseRiskListByUser(User_Id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(Model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public void ExportToExcel(string GridModel)
        {

            string User_Id = User.Identity.GetUserId();
            List<CaseRiskViewModel> Model = new List<CaseRiskViewModel>();
            ViewsBL BLObj = new ViewsBL();
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekno = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            string empId = "0";
            if (Session["TMEmpId"] != null)
            {
                empId = Session["TMEmpId"] as string;
            }
            ArrayList list;
            list = (ArrayList)Session["TimesheetDetail"];
            int Year = System.DateTime.Now.Year;
            if (list == null)
            {
                list = new ArrayList();
                list.Add(weekno - 5);
                list.Add(weekno - 1);
                list.Add(0);
                list.Add(Year);
            }
            var DataSource = BLObj.GetTimeSheetEnterList(User_Id, Convert.ToInt32(list[0]), Convert.ToInt32(list[1]), Convert.ToInt32(list[2]), Convert.ToInt32(list[3]));
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);

            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }

        public void ExportToWord(string[] GridModel)
        {
            WordExport exp = new WordExport();
            string User_Id = User.Identity.GetUserId();
            IWordDocument document = null;
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekno = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            string empId = "0";
            int Year = System.DateTime.Now.Year;
            if (Session["TMEmpId"] != null)
            {
                empId = Session["TMEmpId"] as string;
            }
            ArrayList list;
            list = (ArrayList)Session["TimesheetDetail"];
            if (list == null)
            {
                list = new ArrayList();
                list.Add(weekno - 5);
                list.Add(weekno - 1);
                list.Add(0);
                list.Add(Year);
            }
            ViewsBL BLObj = new ViewsBL();
            var DataSource = BLObj.GetTimeSheetEnterList(User_Id, Convert.ToInt32(list[0]), Convert.ToInt32(list[1]), Convert.ToInt32(list[2]), Convert.ToInt32(list[3]));
            //var DataSource = BLObj.GetTimeSheetEnterList(User_Id, (weekno - 5), (weekno - 1), Convert.ToInt32(empId), Year);
            bool initial = true; ;
            foreach (string gridProperty in GridModel)
            {
                GridProperties gridProp = ConvertObject(gridProperty);


                exp.Export(gridProp, DataSource, "Export.docx", true, true, "flat-saffron", false, document, "Second Grid");

            }
        }
        private GridProperties ConvertObject(string gridProperty)
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
        private GridProperties ConvertGridObject(string gridProperty)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IEnumerable div = (IEnumerable)serializer.Deserialize(gridProperty, typeof(IEnumerable));
            GridProperties gridProp = new GridProperties();
            foreach (KeyValuePair<string, object> ds in div)
            {
                var property = gridProp.GetType().GetProperty(ds.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (ds.Key == "Filter")
                {
                    Filter = ds.Value.ToString();
                    continue;
                }
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
        public ActionResult GetTimeSheetEntryList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProViews/GetTimeSheetEntryList");
            if (FindFlag)
            {
                List<TimeSheetViewModel> Model = new List<TimeSheetViewModel>();
                CultureInfo ciCurr = CultureInfo.CurrentCulture;
                int weekno = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                weekno = weekno - 1;
                int ToWeek = weekno;
                int FromWeek = weekno - 4;
                int Year = System.DateTime.Now.Year;
                //int Year = 0;
                try
                {
                    TimeSheetViewModel TmModel = new TimeSheetViewModel();
                    string User_Id = User.Identity.GetUserId();
                    ViewsBL BLObj = new ViewsBL();
                    Model = BLObj.GetTimeSheetEnterList(User_Id, Year, FromWeek, ToWeek, 0);
                    BudgetModel budgetModel = new BudgetModel();
                    AdminBL objBL = new AdminBL();
                    budgetModel.EmployeeLst = objBL.GetBudgetEmployeeList(User_Id);
                    ViewBag.EmployeeList = new SelectList(budgetModel.EmployeeLst, "EmployeeId", "EmployeeName");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(Model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult GetTMData()
        {
            List<TimeSheetViewModel> Model = new List<TimeSheetViewModel>();
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            Session["TimesheetDetail"] = null;
            int weekno = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            weekno = weekno - 1;
            int ToWeek = weekno;
            int FromWeek = weekno - 4;
            int Year = System.DateTime.Now.Year;
            try
            {

                TimeSheetViewModel TmModel = new TimeSheetViewModel();
                string User_Id = User.Identity.GetUserId();
                ViewsBL BLObj = new ViewsBL();
                Model = BLObj.GetTimeSheetEnterList(User_Id, FromWeek, ToWeek, 0, Year );
                var jsonResult = Json(Model, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetTimeSheetListForWeek(int Employee, int ToWeek, int FromWeek,int Year)
        {
            List<TimeSheetViewModel> Model = new List<TimeSheetViewModel>();
            string User_Id = User.Identity.GetUserId();
            if (Session["TMEmpId"] != null)
            {
                Session["TMEmpId"] = null;
            }
            Session["TMEmpId"] = Employee.ToString();
            ArrayList Arlist = new ArrayList();
            Arlist.Add(Employee);
            Arlist.Add(Year);
            Session["TMEmpId"] = Arlist;
            ViewsBL BLObj = new ViewsBL();
            try
            {
                if (ToWeek == FromWeek)
                {
                    CultureInfo ciCurr = CultureInfo.CurrentCulture;
                    int weekno = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                    weekno = weekno - 1;
                    ToWeek = weekno;
                    FromWeek = weekno - 4;
                }
                ArrayList list = new ArrayList();
                list.Add(FromWeek);
                list.Add(ToWeek);
                list.Add(Employee);
                list.Add(Year);
                Session["TimesheetDetail"] = list;
                Model = BLObj.GetTimeSheetEnterList(User_Id, FromWeek, ToWeek, Employee, Year);
            }
            catch (Exception ex)
            {
                throw;
            }
            var jsonResult = Json(Model, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        public ActionResult TimesheetMonthwise()
        {
            List<TimeSheetViewModel> Model = new List<TimeSheetViewModel>();
            int CustomerId = 0;
            try
            {
                ProjectBL Blobj = new ProjectBL();
                string User_Id = User.Identity.GetUserId();
                ViewBag.CustomerList = new SelectList(Blobj.GetCustomerListByUser(User_Id), "Id", "DisplayName");
                TimeSheetViewModel TmModel = new TimeSheetViewModel();
                ViewsBL BLObj = new ViewsBL();
                Model = BLObj.GetTimeSheetListByEmployee(User_Id, CustomerId, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(Model);

        }
        public ActionResult GetTMEmpCustData()
        {
            List<TimeSheetViewModel> Model = new List<TimeSheetViewModel>();
            int CustomerId = 0;
            try
            {
                string User_Id = User.Identity.GetUserId();
                TimeSheetViewModel TmModel = new TimeSheetViewModel();
                ViewsBL BLObj = new ViewsBL();
                Model = BLObj.GetTimeSheetListByEmployee(User_Id, CustomerId, 0);
                var jsonResult = Json(Model, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetTimeSheetDetilByCust(int CustomerId, int Employee)
        {
            List<TimeSheetViewModel> Model = new List<TimeSheetViewModel>();
            string User_Id = User.Identity.GetUserId();
            ViewsBL BLObj = new ViewsBL();
            try
            {
                Model = BLObj.GetTimeSheetListByEmployee(User_Id, CustomerId, Employee);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            var jsonResult = Json(Model, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #region SalesBacklogList
        public JsonResult GetBacklogsComments(string DocumentNo)
        {
            SalesBacklogViewModel model = new SalesBacklogViewModel();
            try
            {
                Session["DocumentNo"] = null;
                Session["DocumentNo"] = DocumentNo;
                //ViewsBL BLObj = new ViewsBL();
                //Commentmodel cmt = new Commentmodel();
                //model.CommentsList = BLObj.GetselectedBacklogComments(DocumentNo);
                //ViewBag.backlog = model.CommentsList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCommentPartial()
        {
            SalesBacklogViewModel model = new SalesBacklogViewModel();
            try
            {

                string DocumentNo = (String)Session["DocumentNo"];
                ViewsBL BLObj = new ViewsBL();
                SmartSys.BL.Project.Commentmodel cmt = new SmartSys.BL.Project.Commentmodel();
                model.CommentsList = BLObj.GetselectedBacklogComments(DocumentNo);
                ViewBag.backlog = model.CommentsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(model);

        }

        public ActionResult GetSalesBacklogList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProViews/GetSalesBacklogList");
            if (FindFlag)
            {
                SalesBacklogViewModel Model = new SalesBacklogViewModel();
                Model.BacklogList = new List<SalesBacklogViewModel>();
                Model.CommentsList = new List<SmartSys.BL.ProViews.Commentmodel>();
                try
                {
                    List<SmartSys.BL.ProViews.Commentmodel> cmtmodel = new List<SmartSys.BL.ProViews.Commentmodel>();
                    string User_Id = User.Identity.GetUserId();
                    ViewsBL BLObj = new ViewsBL();
                    Model.BacklogList = BLObj.GetSalesBacklogList(User_Id);
                    ViewBag.backlog = cmtmodel;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(Model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult AddBacklogComments(string Company, string DocumentNo, int LineNo, int Status)
        {
            SalesBacklogViewModel model = new SalesBacklogViewModel();
            ViewsBL BLObj = new ViewsBL();
            SmartSys.BL.ProViews.Commentmodel cmt = new SmartSys.BL.ProViews.Commentmodel();
            model.Company = Company;
            model.DocumentNo = DocumentNo;
            model.LineNo = LineNo;
            model.Status = Status;
            model.CommentsList = BLObj.GetselectedBacklogComments(DocumentNo);
            ViewBag.commentlist = model.CommentsList;
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult AddBacklogComments(FormCollection fc)
        {

            SalesBacklogViewModel model = new SalesBacklogViewModel();
            ViewsBL BLObj = new ViewsBL();
            string User_id = User.Identity.GetUserId();
            int BacklogCommentId = 0;
            model.Company = fc["Company"].ToString();
            model.LineNo = Convert.ToInt32(fc["LineNo"].ToString());
            model.OrderNo = fc["DocumentNo"].ToString();
            model.Comments = fc["Comments"].ToString();
            int Status = Convert.ToInt32(fc["Status"].ToString());

            int errorcode = BLObj.SaveSalesBacklogComment(model, User_id, Status);
            if (errorcode == 500002)
            {
                return RedirectToAction("GetSalesBacklogList");
            }
            return View(model);
        }


        #endregion SalesBacklogList

        #region MOMActionpointpending list
        public ActionResult MOMPendingActionPointList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProViews/MOMPendingActionPointList");

            if (FindFlag)
            {
                string User_id = User.Identity.GetUserId();
                List<PendingMOMActions> Pendinglist = new List<PendingMOMActions>();
                ViewsBL BLObj = new ViewsBL();
                Pendinglist = BLObj.PendingMOMActionPoint(User_id);
                return View(Pendinglist);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [System.Web.Http.ActionName("ExportToExcelPendingMOMAction")]
        [AcceptVerbs("POST")]
        public void ExportToExcelPendingMOMAction(string GridModel)
        {
            string User_id = User.Identity.GetUserId();
            List<PendingMOMActions> Pendinglist = new List<PendingMOMActions>();
            ViewsBL BLObj = new ViewsBL();
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = BLObj.PendingMOMActionPoint(User_id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult MOMSubordinatePendingActionPointList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProViews/MOMPendingActionPointList");

            if (FindFlag)
            {
                string User_id = User.Identity.GetUserId();
                List<PendingMOMActions> Pendinglist = new List<PendingMOMActions>();
                ViewsBL BLObj = new ViewsBL();
                Pendinglist = BLObj.PendingSubordinateMOMActionPoint(User_id);
                return View(Pendinglist);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [System.Web.Http.ActionName("ExportToExcelMOMSubordinatePendingActionPoint")]
        [AcceptVerbs("POST")]
        public void ExportToExcelMOMSubordinatePendingActionPoint(string GridModel)
        {
            string User_id = User.Identity.GetUserId();
            List<PendingMOMActions> Pendinglist = new List<PendingMOMActions>();
            ViewsBL BLObj = new ViewsBL();
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = BLObj.PendingSubordinateMOMActionPoint(User_id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult MOMCompleteActionPointList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProViews/MOMPendingActionPointList");

            if (FindFlag)
            {
                string User_id = User.Identity.GetUserId();
                List<PendingMOMActions> Pendinglist = new List<PendingMOMActions>();
                ViewsBL BLObj = new ViewsBL();
                Pendinglist = BLObj.CompleteMOMActionPoint(User_id);
                return View(Pendinglist);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public void ExportToExcelPendingMOMCompleteActionPoint(string GridModel)
        {
            string User_id = User.Identity.GetUserId();
            List<PendingMOMActions> Pendinglist = new List<PendingMOMActions>();
            ViewsBL BLObj = new ViewsBL();
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = BLObj.CompleteMOMActionPoint(User_id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult MOMCompleteSubordinateActionPointList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProViews/MOMPendingActionPointList");

            if (FindFlag)
            {
                string User_id = User.Identity.GetUserId();
                List<PendingMOMActions> Pendinglist = new List<PendingMOMActions>();
                ViewsBL BLObj = new ViewsBL();
                Pendinglist = BLObj.CompleteSubordinateMOMActionPoint(User_id);
                return View(Pendinglist);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [System.Web.Http.ActionName("ExportToExcelMOMCompleteSubordinateActionPoint")]
        [AcceptVerbs("POST")]
        public void ExportToExcelMOMCompleteSubordinateActionPoint(string GridModel)
        {
            string User_id = User.Identity.GetUserId();
            List<PendingMOMActions> Pendinglist = new List<PendingMOMActions>();
            ViewsBL BLObj = new ViewsBL();
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = BLObj.CompleteSubordinateMOMActionPoint(User_id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult MOMPendingActionPointListByMe()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProViews/MOMPendingActionPointListByMe");
            if (FindFlag)
            {
                string User_id = User.Identity.GetUserId();
                List<PendingMOMActions> Pendinglist = new List<PendingMOMActions>();
                ViewsBL BLObj = new ViewsBL();
                Pendinglist = BLObj.PendingMOMActionPointBYMe(User_id);
                return View(Pendinglist);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        #endregion MOMActionpointpending list

        #region Project View
        public ActionResult IndexView()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProViews/IndexView");
            if (FindFlag)
            {
                bool AllowDelete;
                AllowDelete = modelObj.FindMenu(lstTaskmodel, "IsDeleted");

                if (AllowDelete == true)
                {
                    TempData["Message"] = "AllowDelete";
                }
                string User_Id = User.Identity.GetUserId();
                string UserName = User.Identity.Name;

                SmartSys.BL.Project.ProjectBL objBL = new SmartSys.BL.Project.ProjectBL();
                SmartSys.BL.Project.ProjectModel proj = new SmartSys.BL.Project.ProjectModel();
                List<SmartSys.BL.Project.ProjectModel> lstproj = objBL.GetProjectList(User_Id);
                proj.AllowDelete = AllowDelete;
                List<SmartSys.BL.Project.ProjectTypeModel> lstProjType = objBL.GetProjectTypeData();
                AdminBL objbl = new AdminBL();
                List<SysEmploy> DrpDwnEmp = new List<SysEmploy>();

                List<SmartSys.BL.Project.TaskDetails> Risklist = new List<SmartSys.BL.Project.TaskDetails>();
                Risklist = objBL.GetRiskList(UserName);
                ViewBag.SelectedRiskDetail = Risklist;
                DrpDwnEmp = objbl.EmployeeDropDown();
                ViewBag.EmpList = new SelectList(DrpDwnEmp, "EmpId", "EmpName");
                ViewBag.ProjTypeList = new SelectList(lstProjType, "ProjectTypeId", "ProjectType");

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
        public ActionResult GetSelectedProjectTaskOverDueList(string TaskName, string Status)
        {
            List<SmartSys.BL.Project.ProjectModel> lstProject = new List<ProjectModel>();
            //List<TimeSheetViewModel> TMList = new List<TimeSheetViewModel>();
            ProjectBL BLObj = new ProjectBL();

            try
            {
                string User_Id = User.Identity.GetUserId();
                lstProject = BLObj.GetSelectedProjectTaskOverDueList(User_Id, TaskName, Status);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(lstProject);
        }
        public ActionResult ProjectView(int ProjectId)
        {
            ProjectMaster ProjMast = new BL.Project.ProjectMaster();
            try
            {
                string User_Id = User.Identity.GetUserId();
                SmartSys.BL.Project.ProjectBL objBL = new SmartSys.BL.Project.ProjectBL();
                ProjMast = objBL.GetProjectTaskData(ProjectId, User_Id);
                ProjMast.ViewMoMList = new List<BL.Project.ProjectTaskMoM>();
                List<SmartSys.BL.Project.Resources> EmpList = new List<SmartSys.BL.Project.Resources>();
                ViewBag.EmpList = EmpList;
                List<ProjectTMView> TMList = new List<ProjectTMView>();
                ViewsBL BLObj = new ViewsBL();
                TMList = BLObj.GetProjectTaskTimeSheetList(ProjectId, 0);
                ViewBag.TMList = TMList;
                TMList = BLObj.GetTaskList(ProjectId, User_Id);
                ViewBag.TaslList = new SelectList(TMList, "TaskId", "TaskName");
                List<CaseRiskViewModel> PendingTaskslst = new List<CaseRiskViewModel>();
                PendingTaskslst = BLObj.GetPendingCaseRiskListByProject(User_Id, ProjectId);
                ViewBag.PendingTasks = PendingTaskslst;
                ProjMast.ViewMoMList = BLObj.GetProjectTaskMOMViewList(ProjectId, 0);

                List<PendingMOMActions> MOMActionList = new List<PendingMOMActions>();
                List<PendingMOMActions> AllActionList = new List<PendingMOMActions>();
                MOMActionList = BLObj.GetProjectViewPendActionPoint(ProjectId);

                PendingMOMActions lstActions = new PendingMOMActions();
                lstActions.LstAllActionPointList = new List<PendingMOMActions>();
                lstActions = BLObj.GetProjectViewActionPointList(ProjectId);
                lstActions.LstAllActionPointList = lstActions.LstAllActionPointList;
                ViewBag.MOMActionViewList = MOMActionList;
                ViewBag.AllActionList = lstActions.LstAllActionPointList;

                List<TMEquipmentModel> EquipmentListmodel = new List<TMEquipmentModel>();
                EquipmentListmodel = BLObj.GetProjectViewEquipmentList(ProjectId);
                ViewBag.EquipmentList = EquipmentListmodel;

                List<ProjectAttachmentView> AttachmentmodelList = new List<ProjectAttachmentView>();
                AttachmentmodelList = BLObj.GetProjectAllAttchmentList(ProjectId);
                ViewBag.AttachmentList = AttachmentmodelList;

                List<ProjectPlanVsActualView> PlanActualList = new List<ProjectPlanVsActualView>();
                PlanActualList = BLObj.GetProjectPlanVsActualList(ProjectId);
                ViewBag.PlanActualList = PlanActualList;

                List<ProjectResourceView> ResourceList = new List<ProjectResourceView>();
                ResourceList = BLObj.GetProjecrResourceList(ProjectId);
                ViewBag.ResourceList = ResourceList;

                ProjectExpensesModel ExpensesModel = new ProjectExpensesModel();
                ExpensesModel.LstPrjectExp = BLObj.GetProjectExpenceViewList(ProjectId);
                ViewBag.ProjectExpence = ExpensesModel.LstPrjectExp;

                List<PaymentExpenseStatus> ExpensesPaymentModel = new List<PaymentExpenseStatus>();
                ExpensesPaymentModel = BLObj.GetProjectExpencePaymentViewList(ProjectId);
                ViewBag.ExpencePayment = ExpensesPaymentModel;

                string user_Id = User.Identity.GetUserId();
                ReportNameModel Model = new ReportNameModel();
                ProjMast.ParamId = "@SmartsysProject";
                ProjMast.ParamName = "@SmartsysProject";
                ProjMast.hidText = "";
                ProjMast.TxtParamValue = ProjectId;
                ProjMast.ReportId = "MGMT004";
                Model = BLObj.GetOpenReport(User_Id, ProjectId);
                ProjMast.OutputLocation = Model.OutputLocation;
                ProjMast.RunDate = Model.RunDate;
                ProjMast.StatusId = Model.StatusId;
                ProjMast.ReportId = Model.ReportId;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }

            return View(ProjMast);
        }
        public ActionResult ProjectMOMViewList(int MOMID)
        {
            SmartSys.BL.Project.ProjectTaskMoM MOMModel = new SmartSys.BL.Project.ProjectTaskMoM();
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "MOMManagementView");
            if (FindFlag)
            {
                ViewBag.findflag = 1;
            }
            SmartSys.BL.Project.ProjectBL objBL = new SmartSys.BL.Project.ProjectBL();
            string User_Id = User.Identity.GetUserId();
            MOMModel.LstMoMParticipant = new List<SmartSys.BL.Project.MoMParticipantModel>();
            MOMModel.LstMOMAttachment = new List<SmartSys.BL.Project.MoMAttachmentModel>();
            MOMModel.ActionPointList = new List<SmartSys.BL.Project.MOMActionPoint>();
            //List<SmartSys.BL.Project.MOMType> MOMType = new List<SmartSys.BL.Project.MOMType>();
            //MOMType = objBL.MOMType();
            //ViewBag.LstActionPointParticipant = objBL.GetselectedMOMAParticipant(MOMID);
            //ViewBag.MOMTypeList = new SelectList(MOMType, "MOMTypeKey", "MOMTypeValue");
            if (MOMID > 0)
            {
                SmartSys.BL.Project.ProjectBL BLObj = new SmartSys.BL.Project.ProjectBL();
                MOMModel = BLObj.GetSelectedProjectTaskMOM(MOMID);
                MOMModel.LstMoMParticipant = BLObj.MOMParticipantList(MOMID);
                MOMModel.LstMOMAttachment = BLObj.MOMAttachmentList(MOMID);
                MOMModel.ActionPointList = BLObj.MOMActionPointList(MOMID);

            }
            return PartialView(MOMModel);
        }
        public ActionResult ProjectMOMActionPointViewList(int ActionPointId)
        {
            SmartSys.BL.Project.MOMActionPoint model = new BL.Project.MOMActionPoint();
            SmartSys.BL.Project.ProjectBL objBL = new SmartSys.BL.Project.ProjectBL();
            model = objBL.GetselectedMOMActionPoint(ActionPointId);
            model.LstActionPointParticipant = objBL.GetselectedMOMActionPointParticipant(ActionPointId);
            return PartialView(model);
        }
        public ActionResult GetProjectMomListByTask(int ProjectId, int TaskID)
        {
            try
            {
                ViewsBL BLObj = new ViewsBL();
                List<SmartSys.BL.Project.ProjectTaskMoM> lstprojMOM = BLObj.GetProjectTaskMOMViewList(ProjectId, TaskID);
                return Json(lstprojMOM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetProjectTaskTimeSheetDetailList(int ProjectId, int EmpId, int TaskId)
        {
            List<TimeSheetViewModel> TMList = new List<TimeSheetViewModel>();
            ViewsBL BLObj = new ViewsBL();
            try
            {
                TMList = BLObj.GetProjectTaskTimeSheetDetailList(ProjectId, EmpId, TaskId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(TMList);
        }
        public ActionResult PendingTaskDetailView(string ProjectName, int ProjectId, int Taskid, string Type)
        {

            ProjectBL objBL = new ProjectBL();
            AdminBL objbl = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            ProjectCustomerAndVendor custvndr = new ProjectCustomerAndVendor();
            custvndr = objBL.GetProjectCustomerAndVendor(ProjectId);
            ViewBag.ProjVendor = custvndr.ProjVendorName;
            ViewBag.ProjCustomer = custvndr.ProjCustomerName;
            ProjectBL ObjBl = new ProjectBL();
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

            model.LstCommentList = new List<SmartSys.BL.Project.Commentmodel>();
            if (Taskid > 0)
            {

                //foreach (SysEmploy e in DrpDwnEmp)
                //{
                //    foreach (object o in model.ResourceID)
                //    {
                //        if (Convert.ToInt32(o.ToString()) == e.EmpId)
                //        {
                //            e.selected = true;
                //        }
                //    }
                //}
                //ViewBag.Employee = DrpDwnEmp;

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
            model.Modal = 0;
            model.ProjectId = ProjectId;
            return PartialView(model);
        }
        public ActionResult GetProjectEquipmentDetail(int EquipmentId)
        {
            try
            {
                ViewsBL BLObj = new ViewsBL();
                TMEquipmentModel objmodel = new TMEquipmentModel();
                TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                objmodel.lstItems = new List<TMEquipmentItem>();
                objmodel.lstSegment = new List<SegmentList>();
                objmodel = ObjTMBL.GetSelectedEquipment(EquipmentId);
                objmodel.lstItems = ObjTMBL.GetselectedEquipmentItem(EquipmentId);
                return PartialView(objmodel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetProjectResourceDetail(int ProjectId, int Id, string Type)
        {
            ProjectResourceDetailsViewModel ResourceDetailList = new ProjectResourceDetailsViewModel();
            ViewsBL BLObj = new ViewsBL();
            try
            {
                ResourceDetailList = BLObj.GetProjecrResourceDetailList(ProjectId, Id, Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(ResourceDetailList);
        }
        public ActionResult ProjectExpensesPaymentDetail(int Id)
        {
            PaymentExpenseStatus model = new PaymentExpenseStatus();
            ProjectBL objBL = new ProjectBL();
            if (Id > 0)
                model = objBL.TmGetselectedPayment(Id);
            return PartialView(model);
        }
        #endregion Project View

        #region Employee View 
        public ActionResult GetEmployeeViewList()
        {
            List<EmpChartModel> EmpModelList = new List<EmpChartModel>();
            ViewsBL BLObj = new ViewsBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                EmpModelList = BLObj.GetEmployeeViewList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(EmpModelList);
        }
        //public ActionResult GetEmpData()
        //{
        //    List<EmpChartModel> EmpModelList = new List<EmpChartModel>();
        //    ViewsBL BLObj = new ViewsBL();
        //    string User_Id = User.Identity.GetUserId();

        //        EmpModelList = BLObj.GetEmployeeViewList(User_Id);
        //        IEnumerable data = EmpModelList;
        //        var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
        //        jsonResult.MaxJsonLength = int.MaxValue;
        //        return jsonResult;                           
        //}
        public ActionResult GetEmpViewDyndata(string gridName,int EmpId)
        {
            ViewsBL BLObj = new ViewsBL();
            AdminBL objBL = new AdminBL();
            if (gridName == "ActionPoint")
            {
                List<PendingMOMActions> PndngMOMActionList = new List<PendingMOMActions>();
                PndngMOMActionList= BLObj.GetAllMOMActionPointByEmployee(EmpId);
                return Json(PndngMOMActionList, JsonRequestBehavior.AllowGet);
            }
            if (gridName == "MOM Details")
            {
                List<ProjectTaskMoM> MOMList = new List<ProjectTaskMoM>();
                MOMList = BLObj.GetSelectedProjectTaskMOM(EmpId);
                return Json(MOMList, JsonRequestBehavior.AllowGet);
            }
            if (gridName == "Project Details")
            {
                List<ProjectModel> lstProject = new List<ProjectModel>();
                lstProject = BLObj.GetProjectsByEmployee(EmpId);
                return Json(lstProject, JsonRequestBehavior.AllowGet);
            }
            if (gridName == "Task Details")
            {
                List<CaseRiskViewModel> TaskModel = new List<CaseRiskViewModel>();
                TaskModel = BLObj.GetCaseRiskEbyEmployee(EmpId);
                return Json(TaskModel, JsonRequestBehavior.AllowGet);
            }
            if (gridName == "Leave Details")
            {
                List<TMLeaveDetailModel> TMLeavelst = new List<TMLeaveDetailModel>();
                TMLeavelst = BLObj.GetLeavelistByEmployee(EmpId);
                return Json(TMLeavelst, JsonRequestBehavior.AllowGet);
            }
            if (gridName == "Travel Request Details")
            {
                List<ProjectTravelRequestModel> list = new List<ProjectTravelRequestModel>();
                list = BLObj.TravelRequestListByEmployee(EmpId);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            if (gridName == "KYE Document")
            {
                List<Documents> Doclist = new List<Documents>();
                Doclist = objBL.GetDocumentsByEmployee(EmpId);
                return Json(Doclist, JsonRequestBehavior.AllowGet);
            }
            if (gridName == "Budget Details")
            {
                List<BudgetModel> ListbudgetModel = new List<BudgetModel>();
                BudgetBL ObjBl = new BudgetBL();
                ListbudgetModel = ObjBl.GetSelectedBudgetEmpViewLst(EmpId);
                return Json(ListbudgetModel, JsonRequestBehavior.AllowGet);
            }
            if (gridName == "Enquiry Details")
            {
                List<EnquiryModel> Enqlist = new List<EnquiryModel>();
                Enqlist= BLObj.EnquiryListByEmployee(EmpId, "");
                IEnumerable data = Enqlist;
                var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;               
            }
            if (gridName == "Own Enquiry")
            {
                List<EnquiryModel> Enqlist = new List<EnquiryModel>();
                Enqlist = BLObj.EnquiryListByEmployee(EmpId, "OWN");
                IEnumerable data = Enqlist;
                var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;               
            }
            if (gridName == "Expences")
            {
                List<ProjectExpensesModel> LstExpenses = new List<ProjectExpensesModel>();
                LstExpenses = BLObj.TMProjectExpensesList(EmpId);
                return Json(LstExpenses, JsonRequestBehavior.AllowGet);
            }
            if (gridName == "Quotation")
            {
                List<EnquiryModel> Enqlist = new List<EnquiryModel>();
                Enqlist = BLObj.EmpQuotaionList(EmpId);
                IEnumerable data = Enqlist;
                var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;               
            }
            if (gridName == "Account receivables")
            {
                List<AccountReceivableModel> ARLst = new List<AccountReceivableModel>();
                ARLst = BLObj.AccountReceivable(EmpId);
                return Json(ARLst, JsonRequestBehavior.AllowGet);
            }
            if (gridName == "Inventory")
            {
                List<InvetoryModel> InvLst = new List<InvetoryModel>();
                InvLst = BLObj.EmpInventoryList(EmpId);
                return Json(InvLst, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [System.Web.Http.ActionName("ExportInventoryDetail")]
        [AcceptVerbs("POST")]
        public void ExportInventoryDetail(string GridModel, int EmpId)
        {
            ViewsBL BLObj = new ViewsBL();
            List<InvetoryModel> InvLst = new List<InvetoryModel>();
            InvLst = BLObj.EmpInventoryList(EmpId);
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = InvLst;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult EmployeeViewDetail(int EmpId)
        {
            List<PendingMOMActions> PndngMOMActionList = new List<PendingMOMActions>();
            List<ProjectTaskMoM> MOMList = new List<ProjectTaskMoM>();
            List<ProjectModel> lstProject = new List<ProjectModel>();
            List<CaseRiskViewModel> TaskModel = new List<CaseRiskViewModel>();
            List<TMLeaveDetailModel> TMLeavelst = new List<TMLeaveDetailModel>();
            List<ProjectTravelRequestModel> list = new List<ProjectTravelRequestModel>();
            List<Documents> Doclist = new List<Documents>();
            List<BudgetModel> ListbudgetModel = new List<BudgetModel>();
            List<EnquiryModel> Enqlist = new List<EnquiryModel>();
            List<ProjectExpensesModel> LstExpenses = new List<ProjectExpensesModel>();
            List<AccountReceivableModel> ARLst = new List<AccountReceivableModel>();
            List<InvetoryModel> InvLst = new List<InvetoryModel>();

            EmpChartModel EmpModel = new EmpChartModel();
            List<EmpTimesheetList> TmList = new List<EmpTimesheetList>();                       
            List<SysRoleTasks> roletask = new List<SysRoleTasks>();
            List<EmpChartModel> List = new List<EmpChartModel>();            
            ViewsBL BLObj = new ViewsBL();
            AdminBL objBL = new AdminBL();
            BudgetBL ObjBl = new BudgetBL();
            try
            {
                //ViewBag.AllMOMActionPointList = BLObj.GetAllMOMActionPointByEmployee(EmpId);
                ViewBag.AllMOMActionPointList = PndngMOMActionList;
                //MOMList = BLObj.GetSelectedProjectTaskMOM(EmpId);
                ViewBag.MOMListByEmployee = MOMList;
                //ViewBag.ProectListByEmployee = BLObj.GetProjectsByEmployee(EmpId);
                ViewBag.ProectListByEmployee = lstProject;
                //ViewBag.CaseRiskByEmployee = BLObj.GetCaseRiskEbyEmployee(EmpId);
                ViewBag.CaseRiskByEmployee = TaskModel;
                //ViewBag.EmployeeLeaveDetails = BLObj.GetLeavelistByEmployee(EmpId);
                ViewBag.EmployeeLeaveDetails = TMLeavelst;
                // ViewBag.TravelRequestListByEmployee = BLObj.TravelRequestListByEmployee(EmpId);
                ViewBag.TravelRequestListByEmployee=list;                
                //Doclist = objBL.GetDocumentsByEmployee(EmpId);
                ViewBag.DocList = Doclist;                                          
                //ListbudgetModel = ObjBl.GetSelectedBudgetEmpViewLst(EmpId);
                ViewBag.getBudgetList = ListbudgetModel;
                // ViewBag.EnquiryListByEmployee = BLObj.EnquiryListByEmployee(EmpId, "");
                ViewBag.EnquiryListByEmployee = Enqlist;
               // ViewBag.EnquiryListByOwnEmployee = BLObj.EnquiryListByEmployee(EmpId, "OWN");
                ViewBag.EnquiryListByOwnEmployee = Enqlist;
                //ViewBag.EmployeeExpences = BLObj.TMProjectExpensesList(EmpId);
                ViewBag.EmployeeExpences =LstExpenses;
               // ViewBag.EmployeeQuotationDetails = BLObj.EmpQuotaionList(EmpId);
                ViewBag.EmployeeQuotationDetails = Enqlist;
               // ViewBag.EmpAccountReceivable = BLObj.AccountReceivable(EmpId);
                ViewBag.EmpAccountReceivable= InvLst;

                ViewBag.EmpInventory = ARLst;
                EmpModel = BLObj.GetSelectedEmployeeViewList(EmpId);
                roletask = BLObj.GetSupermaticAccessList(EmpId);                
                ViewBag.SupermaticAccessList = roletask;
                ViewBag.TMlist = BLObj.GetEmployeeTimeSheetList(EmpId);
                ViewBag.PndngMOMActionPointList = BLObj.GetPendingMOMActionPointByEmployee(EmpId);                                                                            
                ViewBag.EmpId = EmpId;
                

                string User_Id = User.Identity.GetUserId();
                ReportNameModel Model = new ReportNameModel();
                EmpModel.ParamId = "@EmpCode";
                EmpModel.ParamName = "@EmpCode";
                EmpModel.hidText = "";
                EmpModel.TxtParamValue = EmpId;
                EmpModel.ReportId = "MGMT003";
                Model = BLObj.GetReportOpenInEMPView(User_Id, EmpId);
                EmpModel.OutputLocation = Model.OutputLocation;
                EmpModel.RunDate = Model.RunDate;
                EmpModel.StatusId = Model.StatusId;
                EmpModel.ReportId = Model.ReportId;
                EmpModel.TimeUtilizationLst = List;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View(EmpModel);
        }
        public ActionResult DisplayProjectTaskGrid(DateTime StartDate, DateTime EndDate, int EmpId)
        {
            ViewsBL BLObj = new ViewsBL();
            EmpCalenderModel ECModel = new EmpCalenderModel();
            List<EmpCalenderModel> EmpTaskLst = new List<EmpCalenderModel>();
             ECModel.StartDate= StartDate ;
             ECModel.EndDate=EndDate ;
             ECModel.EmpId= EmpId  ;
            try
            {
               // EmpTaskLst = BLObj.EmpTaskListByDate(StartDate, EndDate, EmpId);
                ViewBag.ShowEmpTaskLst = BLObj.EmpTaskListByDate(StartDate, EndDate, EmpId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(ECModel);
        }
        public JsonResult DisplayProjectTaskLstCount(DateTime StartDate, DateTime EndDate, int EmpId)
        {
            ViewsBL BLObj = new ViewsBL();
            EmpCalenderModel ECModel = new EmpCalenderModel();
            List<EmpCalenderModel> EmpTaskLst = new List<EmpCalenderModel>();
            try
            {
                EmpTaskLst = BLObj.EmpTaskListByDate(StartDate, EndDate, EmpId);
                IEnumerable data = EmpTaskLst;
                var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult LoadPieChart(DateTime StartDate, DateTime EndDate, int EmpId)
        {
            ViewsBL BLObj = new ViewsBL();
            EmpChartModel Model = new EmpChartModel();
            List<EmpChartModel> lst;
            try
            {
                lst = new List<EmpChartModel>();
                lst = BLObj.EmpPieChartData(StartDate, EndDate, EmpId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return PartialView(lst);
        }
        public void ExportToExcelTreeGrid(string TreeGridModel, DateTime StartDate, DateTime EndDate, int EmpId)
        {
            ViewsBL BLObj = new ViewsBL();
            EmpCalenderModel ECModel = new EmpCalenderModel();
            List<EmpCalenderModel> EmpTaskLst = new List<EmpCalenderModel>();
            try
            {
                ExcelExport exp = new ExcelExport();
                ViewBag.ShowEmpTaskLst = BLObj.EmpTaskListByDate(StartDate, EndDate, EmpId);
                var DataSource = BLObj.EmpTaskListByDate(StartDate, EndDate, EmpId);
                TreeGridProperties obj = ConvertTreeGridObject(TreeGridModel);
                exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, new TreeGridExportSettings() { Theme = ExportTheme.FlatSaffron });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private TreeGridProperties ConvertTreeGridObject(string gridProperty)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IEnumerable div = (IEnumerable)serializer.Deserialize(gridProperty, typeof(IEnumerable));
            TreeGridProperties gridProp = new TreeGridProperties();
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
        public ActionResult ViewProjectTask(int ProjectId, int EmpId)
        {
            string User_Id = User.Identity.GetUserId();
            ProjectBL objBL = new ProjectBL();
            ProjectMaster ProjMast = objBL.GetProjectTaskData(ProjectId, User_Id);
            List<SmartSys.BL.Project.Resources> EmpList = objBL.GetResourceData();
            ViewBag.EmpList = EmpList;
            ViewBag.EmpId = EmpId;
            return View(ProjMast);
        }
        #endregion Employee View 

        #region Equipment View
        public ActionResult GetEquipmentList()
        {
            List<ItemSegment> lstSegment = new List<ItemSegment>();
            try
            {
                ViewsBL BLObj = new ViewsBL();
                lstSegment = BLObj.GetSegmentListForView();
                ViewBag.SelectedEquipmentItems = lstSegment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        public ActionResult ShowEquipmentView(int SegmentId)
        {
            List<TMEquipmentModel> lstSegment = new List<TMEquipmentModel>();
            ViewsBL BLObj = new ViewsBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                lstSegment = BLObj.GetEquipmentListBySegment(SegmentId, User_Id);
                ViewBag.EquipmentItems = lstSegment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(lstSegment);
        }
        public ActionResult ShowEquipmentViewBySalesPerson()
        {
            List<TMEquipmentModel> lstSegment = new List<TMEquipmentModel>();
            ViewsBL BLObj = new ViewsBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                lstSegment = BLObj.GetEquipmentListBySegment(0, User_Id);
                ViewBag.EquipmentItems = lstSegment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(lstSegment);
        }
        #endregion Equipment View 

        #region Customer View

        public ActionResult CustomerList()
        {
            CustomerListModel customerModel = new CustomerListModel();
            try
            {
                string user_Id = User.Identity.GetUserId();
                CustomerBL objbl = new CustomerBL();
                customerModel.customerGedList = objbl.GetCustomerList(user_Id);
                customerModel.customerGedList.RemoveAll(x => x.IsActive == false);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return View(customerModel);
        }
        public ActionResult GetTimeSheetByCustomer()
        {
            List<TimeSheetViewModel> timesheetModel = new List<TimeSheetViewModel>();
            int CustomerId = (int)Session["CustomerId"];
            ViewsBL BLObj = new ViewsBL();
            timesheetModel = BLObj.TimeSheetDetailsByCustomer(CustomerId);
            var jsonResult = Json(timesheetModel, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult CustomerViewDetail(int CustomerId, string CustomerName)
        {
            CustomerListModel customerModel = new CustomerListModel();
            ProjectModel projectmodel = new ProjectModel();
            ViewsBL BLObj = new ViewsBL();
            Session["CustomerId"] = null;
            Session["CustomerId"] = CustomerId;
            try
            {
                List<Departmentmodel> DeptList = new List<Departmentmodel>();
                string User_Id = User.Identity.GetUserId();
                AdminBL objbl1 = new AdminBL();
                int Eid = objbl1.GetEmployeeByUserId(User_Id);
                DeptList = objbl1.GetEmployeeDept(Eid);
               
                foreach(Departmentmodel Model in DeptList)
                {
                    if(Model.DeptId == 3)
                    {
                        customerModel.isSales = true;
                    }
                    else if(Model.DeptId == 4)
                    {
                        customerModel.isPurchase = true;
                    }
                }

                ViewBag.AtionPointListByCustomer = BLObj.GetAllAtionPointListBYCustomer(CustomerId);
                ViewBag.AllMOMActionPointList = BLObj.GetAllMOMActionPointByCustomer(CustomerId);
                ViewBag.GetAllEmplistByCustomer = BLObj.GetAllEmplistByCustomer(CustomerId, "");
                ViewBag.EnquiryListBYCustomer = BLObj.EnquiryListBYCustomer(CustomerId);
                ViewBag.TravelRequestByCustomer = BLObj.GetAllTravelRequestByCustomer(CustomerId);
                ViewBag.TimeSheetByCustomer = BLObj.TimeSheetDetailsByCustomer(CustomerId);
                projectmodel = BLObj.ProjectModelForCustomer(CustomerId);
                ViewBag.RiskCaseList = projectmodel.RiskCase;
                ViewBag.ProjectDetailsList = projectmodel.Projectmodel;
                ViewBag.CustomerName = CustomerName;
                CustomerModel CustomerModel = new CustomerModel();
                CustomerBL objbl = new CustomerBL();
                CustomerModel = objbl.CustomerGetselected(CustomerId);
                List<CustomerLibaryModel> lst = new List<CustomerLibaryModel>();
                lst = CustomerModel.CustomerKYCList;
                ViewBag.docList = lst;
                ViewBag.CustId = CustomerId;
                BudgetModel budgetModel = new BudgetModel();
                budgetModel.lstBudgetDetail = new List<BudgetModel>();
                BudgetBL ObjBl = new BudgetBL();
                budgetModel.lstBudgetDetail = ObjBl.GetSelectedBudgetCustView(CustomerId);
                ViewBag.getBudgetList = budgetModel.lstBudgetDetail;
                ViewBag.CustAccountReceivable = BLObj.CustAccounReceivable(CustomerId);
                ViewBag.ItemListByCustomer = BLObj.CustomerItemWise(CustomerId);
                ViewBag.ItemInventoryListByCustomer = BLObj.CustomerItemInventory(CustomerId);
                ViewBag.CustSalesIrderBacklog = BLObj.CustomerSalesOrderBacklog(CustomerId);
                ViewBag.CustPurchaseBacklog = BLObj.GetPurchaseBackLogsDetail("Customer", CustomerId);
                ViewBag.CustSalesBacklog = BLObj.GetSalesBackLogsDetail("Customer", CustomerId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View(customerModel);
        }
        public ActionResult CustAccReceLedgerEntries(string Company, string CustomerNo)
        {
            AccountReceivableModel model = new AccountReceivableModel();
            ViewsBL BLObj = new ViewsBL();
            ViewBag.CustAccountReceivableLedgerEntries = BLObj.CustAccounReceivableLedgerEntries(CustomerNo, Company);
            return PartialView();
        }
        public ActionResult CreateCustomerEnquiryForCust(int EnqId, int ItemId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/ProViews/CreateCustomerEnquiryForCust?EnqId=0");
            FindFlag = true;
            if (FindFlag)
            {
                EnquiryModel EnquModel = new EnquiryModel();
                EnquiryBL Enqbl = new EnquiryBL();
                string User_Id = User.Identity.GetUserId();
                try
                {
                    ItemModel itemModel = new ItemModel();
                    BudgetModel budgetModel = new BudgetModel();
                    BudgetBL ObjBl = new BudgetBL();
                    ItemBL BLObj = new ItemBL();
                    EnquModel.lstEnquiryDetail = new List<EnquiryDetailModel>();
                    itemModel.lstItem = new List<ItemModel>();
                    itemModel.lstItem = BLObj.GetItemListwithMPN();
                    if (EnqId > 0)
                    {
                        EnquModel = Enqbl.getSelectedEnquiryDetail(EnqId);


                        if (ItemId != 0)
                        {
                            foreach (EnquiryDetailModel models in EnquModel.RefList)
                            {
                                if (models.ItemId != ItemId)
                                {
                                    var st = EnquModel.lstEnquiryDetail.Find(c => c.ItemId == models.ItemId);
                                    EnquModel.lstEnquiryDetail.Remove(st);
                                }
                            }
                        }
                    }
                    ViewBag.itemlist = new SelectList(itemModel.lstItem, "ItemId", "ItemName");

                    ProjectBL Blobj = new ProjectBL();
                    ViewBag.CustomerLst = new SelectList(Blobj.GetCustomerListByUser(User_Id), "Id", "DisplayName");

                    ItemBrandAllocationDetail model = new ItemBrandAllocationDetail();
                    ItemBL objbl1 = new ItemBL();
                    model = objbl1.GetItemBrandAllocation(1);
                    model.AssignedItemBrand = new List<ItemBrandAllocationDetail.DerivedItemBrand>();
                    ViewBag.BrandListLst = model.TotalItemBrand;

                    CustomerModel CustomerModel = new CustomerModel();
                    CustomerBL objblContect = new CustomerBL();
                    CustomerModel = objblContect.CustomerGetselected(EnquModel.CustomerId);
                    ViewBag.CustContactList = new SelectList(CustomerModel.CustmerContactLst, "CustomerContactId", "ContactName");

                    ItemBL BlObj = new ItemBL();
                    ItemMappingModel ItemMap = new ItemMappingModel();
                    ItemMap.lstItemMap = BlObj.GetDWCompList();
                    ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");
                }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                }
                Session["EnquiryDetail"] = EnquModel;
                return PartialView(EnquModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult EmployeeInforForCustomer(int EmpId)
        {

            EmployeeModel empmodel = new EmployeeModel();
            AdminBL objbl = new AdminBL();

            empmodel = objbl.GetSelectedEmployeeList(EmpId);


            return PartialView(empmodel);
        }

        #endregion

        #region VendorView
        public ActionResult VendorViewList()
        {
            VendorListModel VendorModel = new VendorListModel();
            VendorBL objbl = new VendorBL();
            string User_Id = User.Identity.GetUserId();
            VendorModel.vendorGedList = objbl.GetVendorList(User_Id);
            VendorModel.vendorGedList.RemoveAll(x => x.IsActive == false);
            return View(VendorModel);
        }
        public ActionResult VendorViewDetails(int VendorId, string VendorName)
        {
            VendorListModel VendModel = new VendorListModel();
            ProjectModel projectmodel = new ProjectModel();
            ViewsBL BLObj = new ViewsBL();
            ViewBag.EMPList = BLObj.GetAllEmplistByVendor(VendorId, "");
            ViewBag.MOMList = BLObj.GetSelectedProjectTaskMOMBYvendor(VendorId);
            ViewBag.AllMOMActionPointList = BLObj.GetAllMOMActionPointByVendor(VendorId);
            VendorModel VendorModel = new VendorModel();
            List<Departmentmodel> DeptList = new List<Departmentmodel>();
            string User_Id = User.Identity.GetUserId();
            AdminBL objbl1 = new AdminBL();
            int Eid = objbl1.GetEmployeeByUserId(User_Id);
            DeptList = objbl1.GetEmployeeDept(Eid);

            foreach (Departmentmodel Model in DeptList)
            {
                if (Model.DeptId == 3)
                {
                    VendModel.isSales = true;
                }
                else if (Model.DeptId == 4)
                {
                    VendModel.isPurchase = true;
                }
            }
            VendorBL objbl = new VendorBL();
            VendorModel = objbl.DWvendorGetselected(VendorId);
            ViewBag.VendorKYCDOCS = VendorModel.VendorKYCList;
            ViewBag.TravelRequestByVendor = BLObj.GetAllTravelRequestByVendor(VendorId);
            projectmodel = BLObj.ProjectModelForVendor(VendorId);
            ViewBag.RiskCaseList = projectmodel.RiskCase;
            ViewBag.ProjectDetailsList = projectmodel.Projectmodel;
            ViewBag.TimeSheetDetailsForVendor = BLObj.TimeSheetDetailsByVendor(VendorId);
            ViewBag.EnquiryListForVendor = BLObj.EnquiryListByVendor(VendorId);
            ViewBag.VendorBacklog = BLObj.GetPurchaseBackLogsDetail("Vendor", VendorId);
            ViewBag.SalesVendorBacklog = BLObj.GetSalesBackLogsDetail("Vendor", VendorId);
            ViewBag.vendorId = VendorId;
            ViewBag.VendorName = VendorName;
            return View(VendModel);
        }
        #endregion VendorView    
        public ActionResult GetTravelrequest(int RequestId)
        {
            ProjectTravelRequestModel Obmodel = new ProjectTravelRequestModel();
            Obmodel.LstTravelRequestParticipant = new List<ProjectTravelRequestModel>();
            Obmodel.LstTravelBudget = new List<TravelBudgetModel>();
            try
            {
                TMLeaveBL TmBL = new TMLeaveBL();
                Obmodel = TmBL.GetProjectTravelRequestDatail(RequestId);
                Obmodel.LstTravelRequestParticipant = TmBL.GetTimeTravelReuestParticipantlist(RequestId);
                Obmodel.LstTravelBudget = TmBL.GetBudgetTravelReuestlist(RequestId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(Obmodel);
        }
        public ActionResult TimeSheetDetails(int TimeSheetId)
        {
            TimeSheetViewModel Model = new TimeSheetViewModel();
            try
            {
                ViewsBL objBL = new ViewsBL();
                Model = objBL.TimeSheetDetails(TimeSheetId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(Model);
        }
        public ActionResult BudgetDetail(int BudgetId)
        {
            BudgetModel budgetModel = new BudgetModel();
            try
            {
                BudgetBL ObjBl = new BudgetBL();
                budgetModel = ObjBl.GetselectedBudget(BudgetId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(budgetModel);
        }
        #region OrgChart
        //public ActionResult EnquiryTrackingViewDetail(int EnqId)
        //{
        //    EnquiryTrackingViewDetail MailEnqTimeDifference = new EnquiryTrackingViewDetail();
        //    ViewsBL BLObj = new ViewsBL();
        //    List<EnquiryTrackingViewDetail> EMailEnqTeackingLst = new List<EnquiryTrackingViewDetail>();
        //    MailEnqTimeDifference = BLObj.MailEnqTimeDifference(EnqId);
        //    ViewBag.test = MailEnqTimeDifference.LstOrgChart;
        //    return View(MailEnqTimeDifference);
        //}
        #endregion OrgChart
        public ActionResult ItemList()
        {

            return View();
        }
        public ActionResult ItemViewDetail(int ItemId, string ItemName)
        {
            VendorListModel VendModel = new VendorListModel();
            List<Departmentmodel> DeptList = new List<Departmentmodel>();
            string User_Id = User.Identity.GetUserId();
            AdminBL objbl1 = new AdminBL();
            int Eid = objbl1.GetEmployeeByUserId(User_Id);
            DeptList = objbl1.GetEmployeeDept(Eid);
            foreach (Departmentmodel Model in DeptList)
            {
                if (Model.DeptId == 3)
                {
                    VendModel.isSales = true;
                }
                else if (Model.DeptId == 4)
                {
                    VendModel.isPurchase = true;
                }
            }
            ViewsBL objBL = new ViewsBL();
            ViewBag.Projectlist = objBL.GetProjectListForItemView(ItemId);
            ViewBag.DispatchList = objBL.GetDispatchListByItem(ItemId);
            ViewBag.EnquiryList = objBL.getEnquiryListByItem(ItemId);
            ViewBag.EquipMentList = objBL.GetTMEquipmentListByItem(ItemId);
            ViewBag.CustPOList = objBL.getCustPOListByItem(ItemId);
            ViewBag.QuotationList = objBL.GetQuotaionListByItem(ItemId);
            ViewBag.VendorResponse = objBL.VendorResponseByItem(ItemId);
            ViewBag.PurchaseItemDetail = objBL.GetPurchaseBackLogsDetail("Item",ItemId);
            ViewBag.SalesItemDetail = objBL.GetSalesBackLogsDetail("Item", ItemId);
            ViewBag.ItmId = ItemId;
            ViewBag.ItemName = ItemName;
            return View(VendModel);
        }
        public ActionResult DispatchDetail(int DispatchId, int ItemId)
        {
            DispatchModel DispatchModel = new DispatchModel();
            DispatchModel.LstDispatchDetail = new List<DispatchDetailModel>();
            try
            {
                ViewBag.ItemId = ItemId;
                if (DispatchId > 0)
                {
                    EnquiryBL BLObj1 = new EnquiryBL();
                    DispatchModel = BLObj1.GetSelectedDispatch(DispatchId);
                }

                ItemBL BlObj = new ItemBL();
                ItemMappingModel ItemMap = new ItemMappingModel();
                ItemMap.lstItemMap = BlObj.GetDWCompList();
                ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");

                ContyStateCityModel addressModel = new ContyStateCityModel();
                AdminBL BLObj = new AdminBL();
                addressModel = BLObj.GetCountryStateCityList();
                List<AddCountryModel> cntrylst = addressModel.LstCountry;
                ViewBag.CountryLst = new SelectList(cntrylst, "CountryId", "CountryName");

                List<FreightForwarderModel> lstf = new List<FreightForwarderModel>();
                VendorBL vendorbl = new VendorBL();
                lstf = vendorbl.FreightForvordarList();
                ViewBag.FFLst = new SelectList(lstf, "FFId", "VendorName");

                EnquiryBL EnqBLObj = new EnquiryBL();
                List<CartonModel> CartonList = new List<CartonModel>();
                CartonList = EnqBLObj.CartonList();
                ViewBag.Carton = new SelectList(CartonList, "Id", "Dimension");

                Session["DispatchDetail"] = DispatchModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(DispatchModel);
        }
        public ActionResult EqiuipmentDetails(int EquipmentId, int ItemId)
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
                FindFlag = modelObj.FindMenu(lstTaskmodel, "/Budget/CreateEquipment?EquipmentId=0");
                FindFlag = true;
                if (FindFlag)

                {
                    ViewBag.ItemId = ItemId;
                    ItemBL BLObj = new ItemBL();
                    ItemModel itemModel = new ItemModel();
                    TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                    itemModel.lstItem = new List<ItemModel>();
                    itemModel.lstItem = BLObj.GetItemListwithMPN();
                    ViewBag.itemlist = new SelectList(itemModel.lstItem, "ItemId", "ItemName");
                    List<CurrencyCodes> lstModel = new List<CurrencyCodes>();
                    lstModel = ObjTMBL.GetCurrencyCodes();
                    ViewBag.Currency = new SelectList(lstModel, "Key", "Value");
                    TMEquipmentModel objmodel = new TMEquipmentModel();

                    objmodel.lstItems = new List<TMEquipmentItem>();
                    objmodel.lstSegment = new List<SegmentList>();
                    List<SegmentList> DrpSegment = new List<SegmentList>();
                    List<SegmentList> lstParentEquipment = new List<SegmentList>();
                    lstParentEquipment = ObjTMBL.GetParentEquipmentList(EquipmentId);
                    ViewBag.ParentEquipment = new SelectList(lstParentEquipment, "EquipmentId", "EquipmentName");
                    if (EquipmentId > 0)
                    {
                        objmodel = ObjTMBL.GetSelectedEquipment(EquipmentId);
                        objmodel.lstItems = ObjTMBL.GetselectedEquipmentItem(EquipmentId);
                        DrpSegment = ObjTMBL.GetSegmentList(EquipmentId);
                        ViewBag.DrpSegment = new SelectList(DrpSegment, "SegmentId", "SegmentName");
                    }

                    return PartialView(objmodel);
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
        public ActionResult PurchaseOrderDetails(int PurchaseOrderId, int ItemId)
        {
            CustomerPOModel POModel = new CustomerPOModel();
            EnquiryBL Enqbl = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                ViewBag.ItemId = ItemId;
                ItemModel itemModel = new ItemModel();
                BudgetModel budgetModel = new BudgetModel();
                BudgetBL ObjBl = new BudgetBL();
                ItemBL BLObj = new ItemBL();
                POModel.PODetaillist = new List<CustomerPODetailModel>();
                itemModel.lstItem = new List<ItemModel>();
                itemModel.lstItem = BLObj.GetItemListwithMPN();
                POModel.DocumentFile = "";
                if (PurchaseOrderId > 0)
                {
                    POModel = Enqbl.getSelectedPODetail(PurchaseOrderId);
                }

                ViewBag.itemlist = new SelectList(itemModel.lstItem, "ItemId", "ItemName");
                ProjectBL Blobj = new ProjectBL();
                ViewBag.CustomerLst = new SelectList(Blobj.GetCustomerListByUser(User_Id), "Id", "DisplayName");
                List<QuotationModel> lstQuot = new List<QuotationModel>();
                lstQuot = Enqbl.GetQuotationListfordrpdwn(User_Id);
                ViewBag.QuotLst = new SelectList(lstQuot, "QuotationId", "QuotationNumber");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            Session["PODetail"] = POModel;
            return PartialView(POModel);
        }
        public ActionResult QuotaionDetails(int EnqId, string CustomerName, string EnqNumber, int ItemId, int QuotationId)
        {
            EnquiryModel Enqmodel = new EnquiryModel();
            ViewBag.ItemId = ItemId;
            Enqmodel.QuotList = new List<QuotationModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            Session["EnqNumber"] = null;
            Session["EnqNumber"] = EnqNumber;
            Enqmodel = Enqbl.GetSelectedEnqQuotation(EnqId, QuotationId);
            var QuotId = Enqmodel.QuotList[0].QuotationId;
            var Terms = Enqmodel.QuotList[0].TermId;
            ViewBag.QuotId = QuotId;
            ViewBag.Terms = Terms;
            string User_Id = User.Identity.GetUserId();
            Enqmodel.CustomerName = CustomerName;
            Enqmodel.EnqId = EnqId;
            Enqmodel.EnqNumber = EnqNumber;
            List<EnquiryItemVendorResponseDetailModel> ResponseList = new List<EnquiryItemVendorResponseDetailModel>();
            List<EnquiryItemVendorResponseDetailModel> CustItemList = new List<EnquiryItemVendorResponseDetailModel>();
            List<EnquiryItemVendorResponseDetailModel> ALLResponseList = new List<EnquiryItemVendorResponseDetailModel>();
            ResponseList = Enqbl.GetAllVendorResponseByItem(0, EnqId, QuotationId);
            CustItemList = Enqbl.GetCustQuotItem(EnqId, QuotationId);
            ALLResponseList = Enqbl.GetAllVendorResponseByItem(0, 0, QuotationId);
            ViewBag.ALLResponseList = ALLResponseList;
            ViewBag.ResponseList = ResponseList;
            ViewBag.CustItemList = CustItemList;
            List<Paytermsmodel> PTList = new List<Paytermsmodel>();
            BOMAdminBL objbl = new BOMAdminBL();
            PTList = objbl.GetPaymentTermsList(User_Id);
            ViewBag.Paymentterms = new SelectList(PTList, "PTId", "PTCode");
            List<DrpItem> CUstomer = new List<DrpItem>();
            ProjectBL ObjBl = new ProjectBL();
            CUstomer = ObjBl.GetCustomerListByUser(User_Id);
            ViewBag.CustList = new SelectList(CUstomer, "Id", "DisplayName");
            return PartialView(Enqmodel);
        }
        public ActionResult VendorResponseDetail(int responseId, int ItemId)
        {
            SysTaskModel modelObj = new SysTaskModel();
            ViewBag.ItemId = ItemId;
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/CreateEnquiryItemVendorResponse?responseId=0");
            FindFlag = true;
            if (FindFlag)
            {
                EnquiryItemVendorResponseDetailModel model = new EnquiryItemVendorResponseDetailModel();
                try
                {
                    EnquiryBL objbl = new EnquiryBL();
                    if (responseId > 0)
                    {
                        model = objbl.GetEnquiryItemVendorResponseDetailList(responseId);
                    }
                    List<ItemModel> list = new List<ItemModel>();
                    ItemBL itembl = new ItemBL();
                    list = itembl.GetItemListwithMPN();
                    ViewBag.ItemList = new SelectList(list, "ItemId", "ItemName");

                    string User_Id = User.Identity.GetUserId();
                    ViewBag.VendorList = new SelectList(objbl.VendorListHaveContact(User_Id), "Id", "DisplayName");

                    VendorModel VendorModel = new VendorModel();
                    VendorBL objblContect = new VendorBL();
                    VendorModel = objblContect.DWvendorGetselected(model.VendorId);
                    ViewBag.ContactList = new SelectList(VendorModel.VendorContactLst, "VendorContactId", "ContactName");
                    Session["VendorresponseDetail"] = model;

                    TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                    List<CurrencyCodes> lstModel = new List<CurrencyCodes>();
                    lstModel = ObjTMBL.GetCurrencyCodes();
                    ViewBag.Currency = new SelectList(lstModel, "Key", "Value", model.Currency);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return PartialView(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #region BrandView
        public ActionResult BrandList()
        {
            List<BrandListViewModel> BrandLst = new List<BrandListViewModel>();
            ViewsBL BLObj = new ViewsBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                BrandLst = BLObj.BrandList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(BrandLst);
        }
        public ActionResult BrandViewDetail(int BrandId)
        {
            ViewsBL BLObj = new ViewsBL();
            BrandListViewModel BrandModel = new BrandListViewModel();            
            List<Departmentmodel> DeptList = new List<Departmentmodel>();
            string User_Id = User.Identity.GetUserId();
            AdminBL objbl1 = new AdminBL();
            int Eid = objbl1.GetEmployeeByUserId(User_Id);
            DeptList = objbl1.GetEmployeeDept(Eid);            
            try
            {
                // ViewBag.ItemByBrand = BLObj.ItemListByBrand(BrandId);
                ViewBag.EnquiryForBrand = BLObj.EnquiryListByBrand(BrandId);
                ViewBag.CustomerForBrand = BLObj.GetCustomerListByBrand(BrandId);
                ViewBag.PurchaseBrandBacklog = BLObj.GetPurchaseBackLogsDetail("Brand",BrandId);
                ViewBag.SalesBrandBacklog = BLObj.GetSalesBackLogsDetail("Brand", BrandId);
                BrandModel = BLObj.VendorBrand(BrandId);
                BrandModel.BrandId = BrandId;
                foreach (Departmentmodel Model in DeptList)
                {
                    if (Model.DeptId == 3)
                    {
                        BrandModel.isSales = true;
                    }
                    else if (Model.DeptId == 4)
                    {
                        BrandModel.isPurchase = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View(BrandModel);
        }
        public ActionResult GetBrandItemData(int BrandId)
        {
            ViewsBL BLObj = new ViewsBL();
            BrandListViewModel BrandModel = new BrandListViewModel();
            try
            {
                IEnumerable data = BLObj.ItemListByBrand(Convert.ToInt32(BrandId));
                var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion BrandView

    }
}