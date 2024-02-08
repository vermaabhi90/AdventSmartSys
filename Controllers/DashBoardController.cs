using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.DashBoard;
using SmartSys.BL.Enquiry;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{
    public class DashBoardController : Controller
    {

        #region[Organisation Chart]
        public ActionResult OrgChart()
        {
            return PartialView();
        }
        public JsonResult GetorgchartData()
        {
            AdminBL objBL = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            List<EmpChartModel> lstChartEmp = new List<EmpChartModel>();
            List<EmployeeModel> lstEmp = objBL.GetEmployeeList(User_Id);

            List<EmployeeModel> mp = lstEmp.Where(o => o.ManagerId == o.EmpId).ToList();

            lstEmp = lstEmp.Where(o => o.ManagerId != o.EmpId).ToList();

            foreach (EmployeeModel e in mp)
            {
                EmpChartModel ce = new EmpChartModel();
                ce.id = e.EmpId;
                ce.EmpName = e.EmpName;
                if (e.EmpId == e.ManagerId)
                    ce.parentId = -1;
                else
                    ce.parentId = e.ManagerId;
                ce.Designation = e.Designation;
                if (e.ManagerId > 7)
                    ce.image = "";
                else
                    ce.image = "";
                lstChartEmp.Add(ce);
                GetEmpList(e, lstChartEmp, lstEmp);
            }

            return Json(lstChartEmp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult orgchartData()
        {
            AdminBL objBL = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            List<EmployeeModel> lstEmp = objBL.GetEmployeeList(User_Id);
            return Json(lstEmp, JsonRequestBehavior.AllowGet);
        }

        private void GetEmpList(EmployeeModel emp, List<EmpChartModel> lstEmpChart, List<EmployeeModel> empList)
        {
            List<EmployeeModel> lstEmp = empList.Where(o => o.ManagerId == emp.EmpId).ToList();
            if (lstEmp.Count > 0)
            {
                foreach (EmployeeModel em in lstEmp)
                {
                    if (em.Deleted == false)
                    {
                        EmpChartModel e = new EmpChartModel();
                        e.EmpName = em.EmpName;
                        e.id = em.EmpId;
                        e.parentId = em.ManagerId;
                        e.Designation = em.Designation;
                        e.Manager = em.ManagerName;
                        e.image = "";
                        lstEmpChart.Add(e);
                        GetEmpList(em, lstEmpChart, empList);
                    }
                }
            }
        }
        #endregion[Organisation Chart]


        public ActionResult salesChart()
        {
            return PartialView();
        }
        public ActionResult MoniterChart()
        {
            return PartialView();
        }
        public ActionResult CustermerChart()
        {
            return PartialView();
        }

        #region [Sales Chart]
        public ActionResult DPKTwelveMonthSalesData()
        {
            ViewBag.CompanyName = "DPK Electrosales Ltd";
            string strCompCode = "DPK";
            string strAction = "DPKTwelveMonthSalesData";
            List<Dashboard> lstdashboard = new List<Dashboard>();
            if (Session["UserDashBord"] != null)
            {
                lstdashboard = Session["UserDashBord"] as List<Dashboard>;
            }
            int iConnectionID = lstdashboard.Find(x => x.ActionName == strAction).ConnectionId;
            MonthlySalesDataModel model = new MonthlySalesDataModel();
            // List<SmartSys.BL.DashBoard.MonthlySalesDataModel> lstMonthySales = new List<SmartSys.BL.DashBoard.MonthlySalesDataModel>();
            SmartSys.BL.DashBoard.DashBoardBL objBL = new SmartSys.BL.DashBoard.DashBoardBL();

            model.lstSaleChart = objBL.GetMonthlySalesData(strCompCode, iConnectionID);
            return PartialView(model);

        }

        public ActionResult AdventTwelveMonthSalesData()
        {
            ViewBag.CompanyName = "Advent Electrosales Ltd";
            string strCompCode = "Advent";
            string strAction = "AdventTwelveMonthSalesData";
            List<Dashboard> lstdashboard = new List<Dashboard>();
            if (Session["UserDashBord"] != null)
            {
                lstdashboard = Session["UserDashBord"] as List<Dashboard>;
            }
            int iConnectionID = lstdashboard.Find(x => x.ActionName == strAction).ConnectionId;
            MonthlySalesDataModel model = new MonthlySalesDataModel();
            // List<SmartSys.BL.DashBoard.MonthlySalesDataModel> lstMonthySales = new List<SmartSys.BL.DashBoard.MonthlySalesDataModel>();
            SmartSys.BL.DashBoard.DashBoardBL objBL = new SmartSys.BL.DashBoard.DashBoardBL();

            model.lstSaleChart = objBL.GetMonthlySalesData(strCompCode, iConnectionID);
            return PartialView(model);
        }
        public ActionResult SAJTwelveMonthSalesData()
        {
            ViewBag.CompanyName = "SAJ Electrosales Ltd";
            string strCompCode = "SAJ";
            string strAction = "SAJTwelveMonthSalesData";
            List<Dashboard> lstdashboard = new List<Dashboard>();
            if (Session["UserDashBord"] != null)
            {
                lstdashboard = Session["UserDashBord"] as List<Dashboard>;
            }
            int iConnectionID = lstdashboard.Find(x => x.ActionName == strAction).ConnectionId;
            MonthlySalesDataModel model = new MonthlySalesDataModel();
            // List<SmartSys.BL.DashBoard.MonthlySalesDataModel> lstMonthySales = new List<SmartSys.BL.DashBoard.MonthlySalesDataModel>();
            SmartSys.BL.DashBoard.DashBoardBL objBL = new SmartSys.BL.DashBoard.DashBoardBL();

            model.lstSaleChart = objBL.GetMonthlySalesData(strCompCode, iConnectionID);
            return PartialView(model);
        }

        public ActionResult AdventtwoYearQuartersales()
        {
            ViewBag.CompanyName = "Advent Electrosales Ltd";
            string strCompCode = "Advent";
            string strAction = "AdventtwoYearQuartersales";
            List<Dashboard> lstdashboard = new List<Dashboard>();
            if (Session["UserDashBord"] != null)
            {
                lstdashboard = Session["UserDashBord"] as List<Dashboard>;
            }
            int iConnectionID = lstdashboard.Find(x => x.ActionName == strAction).ConnectionId;
            List<TwoYearsQuarterSalesModel> model = new List<TwoYearsQuarterSalesModel>();
            // List<SmartSys.BL.DashBoard.MonthlySalesDataModel> lstMonthySales = new List<SmartSys.BL.DashBoard.MonthlySalesDataModel>();
            SmartSys.BL.DashBoard.DashBoardBL objBL = new SmartSys.BL.DashBoard.DashBoardBL();

            model = objBL.TwoYearsQuarterSales(strCompCode, iConnectionID);
            return PartialView(model);
        }
        public ActionResult SAJTwoYearQuarterSales()
        {
            ViewBag.CompanyName = "SAJ Electrosales Ltd";
            string strCompCode = "SAJ";
            string strAction = "SAJTwoYearQuarterSales";
            List<Dashboard> lstdashboard = new List<Dashboard>();
            if (Session["UserDashBord"] != null)
            {
                lstdashboard = Session["UserDashBord"] as List<Dashboard>;
            }
            else
            {
                return null;
            }
            int iConnectionID = lstdashboard.Find(x => x.ActionName == strAction).ConnectionId;
            List<TwoYearsQuarterSalesModel> model = new List<TwoYearsQuarterSalesModel>();
            SmartSys.BL.DashBoard.DashBoardBL objBL = new SmartSys.BL.DashBoard.DashBoardBL();
            model = objBL.TwoYearsQuarterSales(strCompCode, iConnectionID);

            return PartialView(model);
        }
        public ActionResult SAJLastTwoYearSalesData()
        {
            ViewBag.CompanyName = "SAJ Electrosales Ltd";
            string strCompCode = "SAJ";
            string strAction = "SAJLastTwoYearSalesData";
            List<Dashboard> lstdashboard = new List<Dashboard>();
            if (Session["UserDashBord"] != null)
            {
                lstdashboard = Session["UserDashBord"] as List<Dashboard>;
            }
            else
            {
                return null;
            }
            int iConnectionID = lstdashboard.Find(x => x.ActionName == strAction).ConnectionId;
            List<LasTTwoYearsSalesModel> model = new List<LasTTwoYearsSalesModel>();
            SmartSys.BL.DashBoard.DashBoardBL objBL = new SmartSys.BL.DashBoard.DashBoardBL();
            model = objBL.GetYearlySalesData(strCompCode, iConnectionID);

            return PartialView(model);
        }
        public ActionResult AdventLastTwoYearSalesData()
        {
            ViewBag.CompanyName = "Advent Electrosales Ltd";
            string strCompCode = "Advent";
            string strAction = "AdventLastTwoYearSalesData";
            List<Dashboard> lstdashboard = new List<Dashboard>();
            if (Session["UserDashBord"] != null)
            {
                lstdashboard = Session["UserDashBord"] as List<Dashboard>;
            }
            else
            {
                return null;
            }
            int iConnectionID = lstdashboard.Find(x => x.ActionName == strAction).ConnectionId;
            List<LasTTwoYearsSalesModel> model = new List<LasTTwoYearsSalesModel>();
            SmartSys.BL.DashBoard.DashBoardBL objBL = new SmartSys.BL.DashBoard.DashBoardBL();
            model = objBL.GetYearlySalesData(strCompCode, iConnectionID);

            return PartialView(model);
        }
        public ActionResult ALLCompTwelveMonthSalesData()
        {
            ViewBag.CompanyName = "DPK and Advent and SAJ Electronics Sales Data";
            string strCompCode = "ALL";
            string strAction = "ALLCompTwelveMonthSalesData";
            List<Dashboard> lstdashboard = new List<Dashboard>();
            if (Session["UserDashBord"] != null)
            {
                lstdashboard = Session["UserDashBord"] as List<Dashboard>;
            }
            int iConnectionID = lstdashboard.Find(x => x.ActionName == strAction).ConnectionId;
            MonthlySalesDataModel model = new MonthlySalesDataModel();
            // List<SmartSys.BL.DashBoard.MonthlySalesDataModel> lstMonthySales = new List<SmartSys.BL.DashBoard.MonthlySalesDataModel>();
            SmartSys.BL.DashBoard.DashBoardBL objBL = new SmartSys.BL.DashBoard.DashBoardBL();

            model.lstSaleChart = objBL.GetMonthlySalesData(strCompCode, iConnectionID);
            return PartialView(model);
        }

        //public JsonResult GetMothWiseSalesData(string strCompCode)
        //{
        //    string[] str = strCompCode.Split('.');
        //    strCompCode = str[0].ToString();
        //    string strAction = str[1].ToString();
        //    List<Dashboard> lstdashboard = new List<Dashboard>();
        //    if (Session["UserDashBord"] != null)
        //    {
        //        lstdashboard = Session["UserDashBord"] as List<Dashboard>;
        //    }
        //    int iConnectionID = lstdashboard.Find(x => x.ActionName == strAction).ConnectionId;

        //    List<SmartSys.BL.DashBoard.MonthlySalesDataModel> lstMonthySales = new List<SmartSys.BL.DashBoard.MonthlySalesDataModel>();
        //    SmartSys.BL.DashBoard.DashBoardBL objBL = new SmartSys.BL.DashBoard.DashBoardBL();

        //    lstMonthySales = objBL.GetMonthlySalesData(strCompCode, iConnectionID);

        //    return Json(lstMonthySales, JsonRequestBehavior.AllowGet);
        //}

        #endregion [Sales Chart]

        #region Approval And Review Pendding List

        public ActionResult PenddingApproverlList()
        {
            ApprovalList ApprovalModel = new ApprovalList();
            try
            {
                AdminBL BlObj = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                ApprovalModel.lstApprover = BlObj.PenddingApproverlList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(ApprovalModel);
        }
        public ActionResult PenddingReviewlList()
        {
            ReviewList ReviewModel = new ReviewList();
            try
            {
                AdminBL BlObj = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                ReviewModel.lstApprover = BlObj.PenddingReviewlList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(ReviewModel);
        }
        #endregion Approval And Review Pendding List

        #region Enquiry Details
        public ActionResult Last7DaysEnquiryChart()
        {
            string strAction = "Last7DaysEnquiryChart";
            List<Dashboard> lstdashboard = new List<Dashboard>();
            if (Session["UserDashBord"] != null)
            {
                lstdashboard = Session["UserDashBord"] as List<Dashboard>;
            }
            int iConnectionID = lstdashboard.Find(x => x.ActionName == strAction).ConnectionId;

            LastSevenDayDataModel model = new LastSevenDayDataModel();
            SmartSys.BL.DashBoard.DashBoardBL objBL = new SmartSys.BL.DashBoard.DashBoardBL();
            model.lstSevenDaysRecord = objBL.GetLastSevenDaysData(iConnectionID);
            return PartialView(model);
        }
        #endregion Enquiry Details

        #region Pending Task Management

        public ActionResult PenddingTaskMgmtList()
        {
            PendingTaskListModel ReviewtaskModel = new PendingTaskListModel();
            try
            {
                AdminBL BlObj = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                ReviewtaskModel.lstApproverTask = BlObj.PenddingTaskList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(ReviewtaskModel);
        }

        #endregion  Pending Task Management

        #region Collection List
        public ActionResult CollectionList()
        {
            CollectionDetailsModel CollectionModel = new CollectionDetailsModel();
            try
            {
                AdminBL BlObj = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                CollectionModel.lstCollection = BlObj.CollectionList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(CollectionModel);
        }

        #endregion Collection List

        #region ProjectTaskOverdue
        public ActionResult ProjectTaskOverdue()
        {
            ProjectTaskOverdueModel ProjectTaskOverdue = new ProjectTaskOverdueModel();
            try
            {
                AdminBL BlObj = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                ProjectTaskOverdue.lstProjectTaskOverdue = BlObj.ProjectTaskOverdue(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(ProjectTaskOverdue);

        }

        #endregion ProjectTaskOverdue

        #region PendingCustomerKYC
        public ActionResult PendingCustomerKYC()
        {
            SmartSys.BL.DW.CustomerModel PendingCustomerKYC = new SmartSys.BL.DW.CustomerModel();

            AdminBL BlObj = new AdminBL();
            DataSet dsBLDetails = new DataSet();
            dsBLDetails = BlObj.PendingCustomerKYC();
            // int i = Convert.ToInt32(dsBLDetails.Tables[0].Rows[0]["CustomerCount"]);
            ViewBag.Count = 0;

            return PartialView();

        }
        #endregion PendingCustomerKYC


        public ActionResult Test1()
        {
            AdminBL BlObj = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            RiskCaseListAllDetail TaskDetail = new RiskCaseListAllDetail();
            TaskDetail = BlObj.GetAllRiskCaseDetailInpercent(User_Id);
            List<RiskCaseListPercent> CasePercentList = TaskDetail.CasePercentList;
            List<RiskCaseListPercent> RiskPercentList = TaskDetail.RiskPercentList;
            List<RiskCaseListPercent> PendingRiskPercentList = new List<RiskCaseListPercent>();
            PendingRiskPercentList = TaskDetail.PendingRiskPercent;
            List<RiskCaseListPercent> PendingCasePercentList = TaskDetail.PendingCasePercent;


            return View(TaskDetail);
        }

        #region IssueDetail
        public ActionResult GetIssueDetailList()
        {
            IssueModel model = new IssueModel();

            string User_Id = User.Identity.GetUserId();
            try
            {
                AdminBL BlObj = new AdminBL();
                model = BlObj.GetIssueDetailListForDashBoard(User_Id);
                List<IssueModel> TotalIssues = model.TotalIssues;
                List<IssueModel> MyIssues = model.MyIssues;
                ViewBag.TotalIssues = TotalIssues;
                ViewBag.MyIssues = MyIssues;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView();
        }
        public ActionResult GetTop10ProjectByTam()
        {
            List<SmartSys.BL.Project.ProjectModel> lstProject = new List<SmartSys.BL.Project.ProjectModel>();

            string User_Id = User.Identity.GetUserId();
            try
            {
                AdminBL BlObj = new AdminBL();
                lstProject = BlObj.GetProjectListBYTam();
                ViewBag.ProjLst = lstProject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(lstProject);
        }
        #endregion issueDetail

        public ActionResult GetTop10QuotationByAmount()
        {
            List<QuotationModel> LstQuot = new List<QuotationModel>();

            string User_Id = User.Identity.GetUserId();
            try
            {
                AdminBL BlObj = new AdminBL();
                LstQuot = BlObj.GetTop10QoutationByAmount();
                List<EnquiryItemVendorResponseDetailModel> CustItemList = new List<EnquiryItemVendorResponseDetailModel>();
                EnquiryBL Enqbl = new EnquiryBL();
                CustItemList = Enqbl.GetCustQuotItem(2, 0);
                ViewBag.CustItemList = CustItemList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(LstQuot);
        }

        public JsonResult GetQuotationDetail(int EnqId)
        {
            EnquiryModel Enqmodel = new EnquiryModel();
            EnquiryBL Enqbl = new EnquiryBL();
            Enqmodel = Enqbl.GetSelectedEnqQuotation(EnqId, 0);
            ViewBag.QuotList = Enqmodel.QuotList;
            return Json(Enqmodel.QuotList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetQuotItemList(int EnqId)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            EnquiryModel Enqmodel = new EnquiryModel();
            List<EnquiryItemVendorResponseDetailModel> CustItemList = new List<EnquiryItemVendorResponseDetailModel>();
            //    ResponseList = Enqbl.GetVendorResponseByItem(EnqId);
            CustItemList = Enqbl.GetCustQuotItem(EnqId, 0);
            ViewBag.CustItemList = CustItemList;
            return Json(CustItemList, JsonRequestBehavior.AllowGet);
        }
    }
}