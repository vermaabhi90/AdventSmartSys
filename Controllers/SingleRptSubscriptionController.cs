using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.Adhoc;
using SmartSys.BL.RDS;
using SmartSys.BL.RGS;
using SmartSys.BL.Sysadmin;
using SmartSys.Utility;
using Syncfusion.EJ.Export;
using Syncfusion.JavaScript.Models;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SmartSys.Controllers
{
    public class SingleRptSubscriptionController : Controller
    {
        public ActionResult SubscriptionList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/SingleRptSubscription/SubscriptionList");
            if (FindFlag)
            {
                List<RDSRptGenSubscription> lstRptSubscripList = new List<RDSRptGenSubscription>();
                RDSRptSubBL objBL = new RDSRptSubBL();
                lstRptSubscripList = objBL.SubscriptionList();
                return View(lstRptSubscripList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ReportParam(string ReportId, int RptSubId, int ClientId)
        {
            RDSClientModel rdsClientList = new RDSClientModel();
            RDSClientBL BLObj = new RDSClientBL();
            string User_Id = User.Identity.GetUserId();
            rdsClientList = BLObj.RDSGetSelectedClient(ClientId);

            GeneratorBL objBl = new GeneratorBL();
            DataSet dsReport = new DataSet();
            dsReport = objBl.GetReportDetails(ReportId);


            AdhocModel objAdhocRun = new AdhocModel();
            objAdhocRun.ReportName = dsReport.Tables[0].Rows[0]["ReportName"].ToString();
            objAdhocRun.ClientName = rdsClientList.ClientName;
            AdhocBL ObjBL = new AdhocBL();
            DataSet dsParam = new DataSet();
            dsParam = ObjBL.GetParamList(ReportId);
            objAdhocRun.ReportId = ReportId;
            bool HasParameter = false;
            List<AdhocParameterModel> lstParams = new List<AdhocParameterModel>();

            paramMadal prmmodParamDemo = new paramMadal();
            prmmodParamDemo.lstAdhocParam = new List<paramMadal>();

            if (dsParam != null)
            {
                if (dsParam.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsParam.Tables[0].Rows)
                    {

                        if (dr["ParamName"].ToString() == "PrintedBy")
                        {
                            paramMadal prmmodParam = new paramMadal();
                            if (Session["ParamDescrip"] != null)
                            {
                                prmmodParam = Session["ParamDescrip"] as paramMadal;
                            }

                            string tempParamValue = getPrintBy();
                            prmmodParam.ParamName = "PrintedBy";
                            prmmodParam.ParamValue = tempParamValue;
                            prmmodParamDemo.lstAdhocParam.Add(prmmodParam);
                            Session["ParamDescrip"] = prmmodParamDemo;
                        }
                        else if (dr["ParamName"].ToString() == "Systemdate")
                        {
                            paramMadal prmmodParam = new paramMadal();
                            if (Session["ParamDescrip"] != null)
                            {
                                prmmodParam = Session["ParamDescrip"] as paramMadal;
                            }
                            string tempParamValue = getSystemDate();
                            prmmodParam.ParamName = "Systemdate";
                            prmmodParam.ParamValue = tempParamValue;
                            prmmodParamDemo.lstAdhocParam.Add(prmmodParam);
                            Session["ParamDescrip"] = prmmodParamDemo;
                        }
                        else if (dr["ParamName"].ToString() == "userID")
                        {
                            paramMadal prmmodParam = new paramMadal();
                            if (Session["ParamDescrip"] != null)
                            {
                                prmmodParam = Session["ParamDescrip"] as paramMadal;
                            }
                            string tempParamValue = getUserId();
                            prmmodParam.ParamName = "userID";
                            prmmodParam.ParamValue = tempParamValue;
                            prmmodParamDemo.lstAdhocParam.Add(prmmodParam);
                            Session["ParamDescrip"] = prmmodParamDemo;
                        }

                        else if (dr["ParamName"].ToString() == "ReportTitle")
                        {
                            paramMadal prmmodParam = new paramMadal();
                            if (Session["ParamDescrip"] != null)
                            {
                                prmmodParam = Session["ParamDescrip"] as paramMadal;
                            }

                            string tempParamValue = ReportTitle(ReportId);
                            prmmodParam.ParamName = "ReportTitle";
                            prmmodParam.ParamValue = tempParamValue;
                            prmmodParamDemo.lstAdhocParam.Add(prmmodParam);
                            Session["ParamDescrip"] = prmmodParamDemo;
                        }
                        else
                        {
                            AdhocParameterModel Demo = new AdhocParameterModel();
                            HasParameter = true;
                            Demo.ParamName = dr["ParamName"].ToString();
                            Demo.DataType = dr["DataType"].ToString();
                            Demo.ParamId = dr["ParamId"].ToString();
                            Demo.ReportId = dr["ReportId"].ToString();
                            Demo.FunctionRef = dr["FunctionRef"].ToString();
                            lstParams.Add(Demo);
                        }

                    }
                }
            }
            objAdhocRun.ParamList = lstParams;
            foreach (AdhocParameterModel prmmod in lstParams)
            {
                if (prmmod.FunctionRef != null && prmmod.FunctionRef.ToString() != "")
                {

                    #region for Employee DropDown
                    //---------------------Employee DropDown list----------------------------------------------//           
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.GetEmployeeForDropDown.ToString())
                    {
                        List<SysEmploy> lstEmp = new List<SysEmploy>();
                        AdminBL objbl = new AdminBL();

                        DataSet ds = objbl.GetEmpList();
                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                foreach (DataRow dr in ds.Tables[0].Rows)
                                {
                                    SysEmploy objmodel = new SysEmploy();
                                    objmodel.EmpId = Convert.ToInt16(dr["EmpId"]);
                                    objmodel.EmpName = dr["EmpName"].ToString();
                                    lstEmp.Add(objmodel);

                                }
                            }
                        }

                        ViewBag.EmpList = new SelectList(lstEmp, "EmpId", "FirstName");
                    }
                    #endregion for Employee DropDown

                    #region for RDS Client DropDown
                    //------------------------------------RDS Client Dropdown List--------------------------------//
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.RDSGetClientForDropDown.ToString())
                    {
                        List<RDSClient> lstRDSClient = new List<RDSClient>();
                        RDSEventBL objRDSClient = new RDSEventBL();
                        DataSet dsRDSclient = objRDSClient.GetRDSClientList();
                        if (dsRDSclient != null)
                        {
                            if (dsRDSclient.Tables.Count > 0)
                            {
                                foreach (DataRow dr in dsRDSclient.Tables[0].Rows)
                                {
                                    RDSClient objmodel = new RDSClient();
                                    objmodel.ClientId = Convert.ToInt16(dr["ClientId"]);
                                    objmodel.ClientName = dr["ClientName"].ToString();
                                    lstRDSClient.Add(objmodel);

                                }
                            }
                        }
                        ViewBag.RDSClientList = new SelectList(lstRDSClient, "ClientId", "ClientName");
                    }
                    #endregion for RDS Client DropDown

                    #region for RGS Report DropDown List
                    //------------------------------------Report Dropdown List--------------------------------//
                    //if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.GetRGSReportForDropDown.ToString())
                    //{
                    //    List<RGSReportNameModel> lstReport = new List<RGSReportNameModel>();
                    //    DataSet dsReport = new DataSet();
                    //    AdhocBL objBL = new AdhocBL();
                    //    dsReport = objBL.GetReportNameList();
                    //    if (dsReport != null)
                    //    {
                    //        if (dsReport.Tables.Count > 0)
                    //        {

                    //            foreach (DataRow dr in dsReport.Tables[0].Rows)
                    //            {
                    //                RGSReportNameModel lstbl = new RGSReportNameModel();
                    //                lstbl.ReportId = dr["ReportId"].ToString();
                    //                lstbl.ReportName = dr["ReportName"].ToString();
                    //                lstReport.Add(lstbl);
                    //            }
                    //        }
                    //    }

                    //    ViewBag.ReportNameList = new SelectList(lstReport, "ReportId", "ReportName");
                    //}
                    #endregion for RGS Report DropDown List

                    #region for SysUser DropDown List
                    //------------------------------------Sys User Dropdown List--------------------------------//
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.SysUserForDropDown.ToString())
                    {
                        List<SysUser> lstSysUser = new List<SysUser>();
                        AdminBL SysUserBL = new AdminBL();

                        DataSet dsSysUser = SysUserBL.GetUserList();
                        if (dsSysUser != null)
                        {
                            if (dsSysUser.Tables.Count > 0)
                            {
                                foreach (DataRow dr in dsSysUser.Tables[0].Rows)
                                {
                                    SysUser objmodel = new SysUser();
                                    objmodel.UserId = Convert.ToInt16(dr["UserId"]);
                                    objmodel.UserName = dr["UserName"].ToString();
                                    lstSysUser.Add(objmodel);

                                }
                            }
                        }
                        ViewBag.SysUserList = new SelectList(lstSysUser, "UserId", "UserName");
                    }
                    #endregion for SysUser DropDown List

                    #region for Business Line DropDown
                    //---------------------Business Line DropDown list----------------------------------------------//           
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.SysBusinessLineForDropDown.ToString())
                    {
                        List<BusinessLineModel> lstBussLine = new List<BusinessLineModel>();
                        AdminBL objBussLine = new AdminBL();

                        DataSet dsBussLine = objBussLine.GetBusinessLineList();
                        if (dsBussLine != null)
                        {
                            if (dsBussLine.Tables.Count > 0)
                            {
                                foreach (DataRow dr in dsBussLine.Tables[0].Rows)
                                {
                                    BusinessLineModel objmodel = new BusinessLineModel();
                                    objmodel.BusinessLineId = Convert.ToInt16(dr["BusinessLineId"]);
                                    objmodel.BusinessLineName = dr["BusinessLineName"].ToString();
                                    lstBussLine.Add(objmodel);
                                }
                            }
                        }

                        ViewBag.BussLineList = new SelectList(lstBussLine, "BusinessLineId", "BusinessLineName");
                    }

                    #endregion for Business Line DropDown

                    #region for Sys Department Dropdown List
                    //------------------------------------Sys Department Dropdown List--------------------------------//
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.SysDepartmentDropDown.ToString())
                    {
                        List<Departmentmodel> lstSysDept = new List<Departmentmodel>();
                        AdminBL SysDeptBL = new AdminBL();

                        DataSet dsSysDept = SysDeptBL.GetDepartmentlist(User_Id);
                        if (dsSysDept != null)
                        {
                            if (dsSysDept.Tables.Count > 0)
                            {
                                foreach (DataRow dr in dsSysDept.Tables[0].Rows)
                                {
                                    Departmentmodel objmodel = new Departmentmodel();
                                    objmodel.DeptId = Convert.ToInt16(dr["DeptId"]);
                                    objmodel.DeptName = dr["DeptName"].ToString();
                                    lstSysDept.Add(objmodel);

                                }
                            }
                        }
                        ViewBag.SysDeptList = new SelectList(lstSysDept, "DeptId", "DeptName");
                    }
                    #endregion for Sys Department Dropdown List

                    #region for EW Company DropDown List
                    //------------------------------------EW Company  Dropdown List--------------------------------//
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.EWCompanyDropDown.ToString())
                    {
                        AdminBL BlObj = new AdminBL();
                        EmoloyeeMapping EmpMap = new EmoloyeeMapping();
                        EmpMap.lstEmolyeeMap = BlObj.GetDWCompList();
                        ViewBag.EWComapnyList = new SelectList(EmpMap.lstEmolyeeMap, "CompCode", "CompanyName");
                    }
                    #endregion for EW Company DropDown List

                    #region for Client Emp  DropDown List
                    //------------------------------------Sys Dropdown List--------------------------------//

                    if (prmmod.FunctionRef.ToString().Length > 11 && prmmod.FunctionRef.ToString().Substring(0, 8) == "CLNTUSER")
                    {
                        // dr["ParamName"].ToString().Length > 11 && dr["ParamName"].ToString().Substring(0,7) =="CLNTUSER"
                        string CompCode = prmmod.FunctionRef.ToString().Substring(8, prmmod.FunctionRef.ToString().Length - 8);
                        AdhocBL BlObj = new AdhocBL();
                        List<ClientEmpCode> clientEmpCodeModel = new List<ClientEmpCode>();
                        clientEmpCodeModel = BlObj.GetClientEmpCodeLst(CompCode);
                        ViewBag.CLNTUSER = new SelectList(clientEmpCodeModel, "Emp_No", "Name");
                    }
                    #endregion for Client Emp DropDown List

                }
            }
            if (RptSubId > 0)
            {
                foreach (AdhocParameterModel Demo in lstParams)
                {

                }
            }
            if (HasParameter)
                return View(objAdhocRun);
            else
                return RedirectToAction("ReportParam", new { ReportId = ReportId, Parmname = "ABC", ParamValue = "XYZ" });
        }
        private string getPrintBy()
        {
            return User.Identity.Name;
        }

        private string getSystemDate()
        {
            string strDate = DateTime.Now.ToString("MM/dd/yyyy");
            return strDate;
        }
        private string getUserId()
        {
            string UserId = User.Identity.GetUserId();
            return UserId;
        }
        private string ReportTitle(string ReportId)
        {
            string RptTitle = "";
            DataSet dsAdhoc = new DataSet();
            GeneratorBL objBl = new GeneratorBL();
            dsAdhoc = objBl.GetReportDetails(ReportId);
            RptTitle = dsAdhoc.Tables[0].Rows[0]["ReportName"].ToString();
            return RptTitle;
        }
        [HttpPost]
        public ActionResult ReportParam(FormCollection fc, string ReportId, string Parmname, string ParamValue)
        {
            try
            {
                string Username = fc["hidText"].ToString();
                int AdhocRunId = 0;
                DataSet dsAdhoc = new DataSet();
                AdhocBL objBL = new AdhocBL();
                dsAdhoc = objBL.GetSelectedList(0);
                if (dsAdhoc != null)
                {
                    if (dsAdhoc.Tables.Count > 0)
                    {
                        dsAdhoc.Tables[0].Rows.Clear();

                        DataRow dr = dsAdhoc.Tables[0].NewRow();
                        //  dr["UserId"] = User.Identity.GetUserId();
                        if (fc == null)
                            dr["ReportId"] = ReportId;
                        else
                            dr["ReportId"] = fc["ReportId"].ToString();
                        dsAdhoc.Tables[0].Rows.Add(dr);

                        //AdhocRunId = objBL.SaveReport(dsAdhoc);
                        //------------------------------------------------------------------------------------//      

                        DataSet dsAdhoc1 = new DataSet();
                        dsAdhoc1 = objBL.GetSelectedAchocList();

                        if (dsAdhoc != null)
                        {
                            if (dsAdhoc1.Tables.Count > 0)
                            {
                                dsAdhoc1.Tables[0].TableName = "AdhocParams";
                                dsAdhoc.Tables.Add(dsAdhoc1.Tables[0].Clone());
                                if (fc != null)
                                {
                                    if (fc.Count > 1)
                                    {
                                        string[] strParam = fc["ParamId"].ToString().Split(',');
                                        string[] strParamVal = fc["TxtParamValue"].ToString().Split(',');

                                        for (int i = 0; i < strParam.Count(); i++)
                                        {
                                            DataRow dr1 = dsAdhoc.Tables[1].NewRow();
                                            dr1["AdhocRunId"] = AdhocRunId;
                                            dr1["ParamName"] = strParam[i];
                                            dr1["ParamValue"] = strParamVal[i];
                                            dsAdhoc.Tables[1].Rows.Add(dr1);
                                            if (strParam[i] == "UserID")
                                            {
                                                DataRow dr2 = dsAdhoc.Tables[1].NewRow();
                                                dr2["AdhocRunId"] = AdhocRunId;
                                                dr2["ParamName"] = "UserName";
                                                dr2["ParamValue"] = Username;
                                                dsAdhoc.Tables[1].Rows.Add(dr2);
                                            }
                                        }
                                    }
                                }

                                if (Session["ParamDescrip"] != null)
                                {
                                    paramMadal prmmodParam = new paramMadal();
                                    prmmodParam = Session["ParamDescrip"] as paramMadal;

                                    foreach (paramMadal DemoModel in prmmodParam.lstAdhocParam)
                                    {
                                        DataRow dr1 = dsAdhoc.Tables[1].NewRow();
                                        dr1["AdhocRunId"] = AdhocRunId;
                                        dr1["ParamName"] = DemoModel.ParamName;
                                        dr1["ParamValue"] = DemoModel.ParamValue;
                                        dsAdhoc.Tables[1].Rows.Add(dr1);
                                    }
                                }
                            }
                        }
                        Session["DataSet"] = dsAdhoc;
                        return RedirectToAction("SaveSubscriptionwithParam");
                        //   errorcode = objBL.SaveAdhocRun(dsAdhoc);                     
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("GetList");
        }

        [HttpGet]
        public ActionResult Create()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/SingleRptSubscription/Create");
            if (FindFlag)
            {
                RDSRptSubModel rdsRPTSubModel = new RDSRptSubModel();
                RDSRptSubBL objBL = new RDSRptSubBL();
                rdsRPTSubModel = objBL.GetReportClientListByUser(1);
                ViewBag.ReportList = new SelectList(rdsRPTSubModel.RDSReportList, "ReportId", "ReportName");
                ViewBag.ClientList = new SelectList(rdsRPTSubModel.ClientList, "ClientId", "ClientName");
                rdsRPTSubModel.RDSRptSubGenTimeList = new List<string>();
                return View(rdsRPTSubModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Create(FormCollection frmCollection, Boolean Email, Boolean FTP)
        {
            string User_Id = User.Identity.GetUserId();
            RDSRptSubModel rdsRptSubModel = new RDSRptSubModel();
            rdsRptSubModel.RptSubGenrationDetails = new RDSRptSubGenDetails();
            rdsRptSubModel.ReportId = frmCollection["ReportList"].ToString();
            rdsRptSubModel.ClientId = Convert.ToInt32(frmCollection["ClientList"].ToString());
            StringBuilder strFrequency = new StringBuilder("");

            if (frmCollection["Monday"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            if (frmCollection["TuesDay"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            if (frmCollection["Wednesday"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            if (frmCollection["Thirsday"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            if (frmCollection["Friday"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            if (frmCollection["Saturday"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            if (frmCollection["Sunday"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            rdsRptSubModel.RptSubGenrationDetails.WeeklyOptions = strFrequency.ToString();
            rdsRptSubModel.RptSubGenrationDetails.Frequency = frmCollection["GenMode"].ToString();
            rdsRptSubModel.RptSubGenrationDetails.StartTime = frmCollection["txtStartTime"].ToString();
            rdsRptSubModel.RptSubGenrationDetails.EndTime = frmCollection["txtEndTime"].ToString();
            int iInterval = 0;
            Int32.TryParse(frmCollection["txtInterval"].ToString(), out iInterval);
            rdsRptSubModel.RptSubGenrationDetails.Interval = iInterval;
            if (frmCollection["RDSRptSubGenTimeList"] != null)
            {
                rdsRptSubModel.RDSRptSubGenTimeList = new List<string>();
                rdsRptSubModel.RDSRptSubGenTimeList.AddRange(frmCollection["RDSRptSubGenTimeList"].ToString().Split(',').ToList());
            }
            if (rdsRptSubModel.RptSubGenrationDetails.StartTime != null && rdsRptSubModel.RptSubGenrationDetails.EndTime != null
                && rdsRptSubModel.RptSubGenrationDetails.Interval != 0)
            {
                DateTime dStart = Convert.ToDateTime(rdsRptSubModel.RptSubGenrationDetails.StartTime);
                DateTime dEnd = Convert.ToDateTime(rdsRptSubModel.RptSubGenrationDetails.EndTime);

                TimeSpan tDiff = dEnd.Subtract(dStart);

                int iCount = (tDiff.Hours * 60) + tDiff.Minutes;
                iCount = iCount / rdsRptSubModel.RptSubGenrationDetails.Interval;

                //Add start time to list of RptSubGenList
                if (rdsRptSubModel.RDSRptSubGenTimeList != null)
                {
                    if (!rdsRptSubModel.RDSRptSubGenTimeList.Contains(dStart.ToString("HH:mm")))
                        rdsRptSubModel.RDSRptSubGenTimeList.Add(dStart.ToString("HH:mm"));
                }
                else
                {
                    rdsRptSubModel.RDSRptSubGenTimeList = new List<string>();
                    rdsRptSubModel.RDSRptSubGenTimeList.Add(dStart.ToString("HH:mm"));
                }
                for (int i = 0; i < iCount; i++)
                {
                    DateTime dTemp = new DateTime();
                    dTemp = dStart.AddMinutes(rdsRptSubModel.RptSubGenrationDetails.Interval);
                    if (!rdsRptSubModel.RDSRptSubGenTimeList.Contains(dTemp.ToString("HH:mm")))
                    {
                        rdsRptSubModel.RDSRptSubGenTimeList.Add(dTemp.ToString("HH:mm"));
                    }
                    dStart = dTemp;
                }

            }
            rdsRptSubModel.RptSubDistributionDetails = new RDSRptSubDistrDetails();
            rdsRptSubModel.RptSubDistributionDetails.DistributionType = frmCollection["DistType"].ToString();
            rdsRptSubModel.RptSubDistributionDetails.DistributionMode = frmCollection["DistMode"].ToString();
            rdsRptSubModel.RptSubDistributionDetails.DistributionTime = frmCollection["txtDistriTime"].ToString();
            rdsRptSubModel.RptSubDistributionDetails.MaxDistributionTime = frmCollection["txtMaxDistriTime"].ToString();

            rdsRptSubModel.RptSubDistributionDetails.Email = Email;
            rdsRptSubModel.RptSubDistributionDetails.FTP = FTP;

            rdsRptSubModel.UserID = 1;//WebSecurity.CurrentUserId;
            RDSRptSubBL BLObj = new RDSRptSubBL();
            rdsRptSubModel.RptSubId = 0;
            AdhocModel objAdhocRun = new AdhocModel();
            AdhocBL ObjBL = new AdhocBL();
            DataSet dsParam = new DataSet();
            dsParam = ObjBL.GetParamList(rdsRptSubModel.ReportId);
            Session["RptSubscrip"] = rdsRptSubModel;
            int errorcode = 0;
            if (dsParam != null)
            {
                if (dsParam.Tables.Count > 0)
                {
                    if (dsParam.Tables[0].Rows.Count > 0)
                    {
                        return RedirectToAction("ReportParam", new { ReportId = rdsRptSubModel.ReportId, RptSubId = rdsRptSubModel.RptSubId, ClientId = rdsRptSubModel.ClientId });
                    }
                    else
                    {
                        errorcode = BLObj.SaveSingleRptSubDetails(rdsRptSubModel, rdsRptSubModel.RptSubId, null, User_Id);
                    }
                }
            }
            if (errorcode == 4)
            {
                return RedirectToAction("SubscriptionList");
            }
            return View();
        }
        public ActionResult SaveSubscriptionwithParam()
        {
            try
            {
                int errorcode = 0;
                string User_Id = User.Identity.GetUserId();
                RDSRptSubBL BLObj = new RDSRptSubBL();
                DataSet dsParam = (DataSet)Session["DataSet"];
                RDSRptSubModel rdsRptSubModel = new RDSRptSubModel();
                rdsRptSubModel = Session["RptSubscrip"] as RDSRptSubModel;
                errorcode = BLObj.SaveSingleRptSubDetails(rdsRptSubModel, rdsRptSubModel.RptSubId, dsParam, User_Id);
                if (errorcode == 4)
                {
                    Session["RptSubscrip"] = null;
                    Session["DataSet"] = null;
                    Session["ParamDescrip"] = null;
                    return RedirectToAction("SubscriptionList");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }
        public SelectList slst { get; set; }
        [HttpGet]
        public ActionResult Edit(int RptSubID)
        {

            RDSRptSubModel rdsRPTSubModel = new RDSRptSubModel();
            RDSRptSubBL objBL = new RDSRptSubBL();
            rdsRPTSubModel = objBL.GetReportClientListByUser(0);
            ViewBag.ReportList = new SelectList(rdsRPTSubModel.RDSReportList, "ReportId", "ReportName");
            ViewBag.ClientList = new SelectList(rdsRPTSubModel.ClientList, "ClientId", "ClientName");
            rdsRPTSubModel = objBL.GetSelectedRptSubScrip(RptSubID);

            slst = new SelectList(rdsRPTSubModel.RDSRptSubGenTimeList);

            ViewBag.ListGenTime = rdsRPTSubModel.RDSRptSubGenTimeList.Select(x =>
                                  new SelectListItem()
                                  {
                                      Text = x.ToString(),
                                      Value = x.ToString()
                                  });
            return View(rdsRPTSubModel);


        }
        [HttpPost]
        public ActionResult Edit(FormCollection frmCollection, Boolean Email, Boolean FTP)
        {
            string User_Id = User.Identity.GetUserId();
            RDSRptSubModel rdsRptSubModel = new RDSRptSubModel();
            rdsRptSubModel.RptSubGenrationDetails = new RDSRptSubGenDetails();
            rdsRptSubModel.RptSubId = Convert.ToInt32(frmCollection["RptSubId"].ToString());
            rdsRptSubModel.ReportId = frmCollection["ReportId"].ToString();
            rdsRptSubModel.ClientId = Convert.ToInt32(frmCollection["ClientId"].ToString());
            StringBuilder strFrequency = new StringBuilder("");

            if (frmCollection["Monday"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            if (frmCollection["TuesDay"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            if (frmCollection["Wednesday"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            if (frmCollection["Thirsday"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            if (frmCollection["Friday"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            if (frmCollection["Saturday"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            if (frmCollection["Sunday"].Contains("true"))
            {
                strFrequency.Append("1");
            }
            else
            {
                strFrequency.Append("0");
            }
            rdsRptSubModel.RptSubGenrationDetails.WeeklyOptions = strFrequency.ToString();
            rdsRptSubModel.RptSubGenrationDetails.Frequency = frmCollection["GenMode"].ToString();
            if (frmCollection["StartTime"] != "" && frmCollection["txtEndTime"] != "" && frmCollection["txtInterval"] != "")
            {
                rdsRptSubModel.RptSubGenrationDetails.StartTime = frmCollection["StartTime"].ToString();

                rdsRptSubModel.RptSubGenrationDetails.EndTime = frmCollection["txtEndTime"].ToString();
                int iInterval = 0;
                Int32.TryParse(frmCollection["txtInterval"].ToString(), out iInterval);
                rdsRptSubModel.RptSubGenrationDetails.Interval = iInterval;
            }
            rdsRptSubModel.RDSRptSubGenTimeList = new List<string>();
            if (frmCollection["RDSRptSubGenTimeList"] != null)
            {
                rdsRptSubModel.RDSRptSubGenTimeList.AddRange(frmCollection["RDSRptSubGenTimeList"].ToString().Split(',').ToList());
            }
            if (rdsRptSubModel.RptSubGenrationDetails.StartTime != "" && rdsRptSubModel.RptSubGenrationDetails.EndTime != ""
                && rdsRptSubModel.RptSubGenrationDetails.Interval != 0)
            {
                DateTime dStart = Convert.ToDateTime(rdsRptSubModel.RptSubGenrationDetails.StartTime);
                DateTime dEnd = Convert.ToDateTime(rdsRptSubModel.RptSubGenrationDetails.EndTime);

                TimeSpan tDiff = dEnd.Subtract(dStart);

                int iCount = (tDiff.Hours * 60) + tDiff.Minutes;
                iCount = iCount / rdsRptSubModel.RptSubGenrationDetails.Interval;
                for (int i = 0; i < iCount; i++)
                {
                    DateTime dTemp = new DateTime();
                    dTemp = dStart.AddMinutes(rdsRptSubModel.RptSubGenrationDetails.Interval);
                    if (!rdsRptSubModel.RDSRptSubGenTimeList.Contains(dTemp.ToString("HH:mm")))
                    {
                        rdsRptSubModel.RDSRptSubGenTimeList.Add(dTemp.ToString("HH:mm"));
                    }
                    dStart = dTemp;
                }

            }
            rdsRptSubModel.RptSubDistributionDetails = new RDSRptSubDistrDetails();
            rdsRptSubModel.RptSubDistributionDetails.DistributionType = frmCollection["DistType"].ToString();
            rdsRptSubModel.RptSubDistributionDetails.DistributionMode = frmCollection["DistMode"].ToString();
            rdsRptSubModel.RptSubDistributionDetails.DistributionTime = frmCollection["txtDistriTime"].ToString();
            rdsRptSubModel.RptSubDistributionDetails.MaxDistributionTime = frmCollection["txtMaxDistriTime"].ToString();
            rdsRptSubModel.RptSubDistributionDetails.Email = Email;
            rdsRptSubModel.RptSubDistributionDetails.FTP = FTP;

            rdsRptSubModel.UserID = 1;//WebSecurity.CurrentUserId;
            RDSRptSubBL BLObj = new RDSRptSubBL();
            rdsRptSubModel.RptSubId = Convert.ToInt32(frmCollection["RptSubId"].ToString()); ;
            AdhocModel objAdhocRun = new AdhocModel();
            AdhocBL ObjBL = new AdhocBL();
            DataSet dsParam = new DataSet();
            dsParam = ObjBL.GetParamList(rdsRptSubModel.ReportId);
            Session["RptSubscrip"] = rdsRptSubModel;
            int errorcode = 0;
            if (dsParam != null)
            {
                if (dsParam.Tables.Count > 0)
                {
                    if (dsParam.Tables[0].Rows.Count > 0)
                    {
                        return RedirectToAction("ReportParam", new { ReportId = rdsRptSubModel.ReportId, RptSubId = rdsRptSubModel.RptSubId, ClientId = rdsRptSubModel.ClientId });
                    }
                    else
                    {
                        errorcode = BLObj.SaveSingleRptSubDetails(rdsRptSubModel, rdsRptSubModel.RptSubId, null, User_Id);
                    }
                }
            }
            if (errorcode == 4)
            {
                return RedirectToAction("SubscriptionList");
            }
            return View();
        }

        public ActionResult Delete(int RptSubId)
        {
            int errorCode = 0;
            RDSRptSubModel rdsRPTSubModel = new RDSRptSubModel();
            RDSRptSubBL objBL = new RDSRptSubBL();
            rdsRPTSubModel.RptSubId = RptSubId;
            rdsRPTSubModel.UserID = 1;// WebMatrix.WebData.WebSecurity.CurrentUserId;
            errorCode = objBL.DeletedSeletedRptSub(rdsRPTSubModel);
            if (errorCode == 500001)
            {
                TempData["Msg"] = "Successfully Delete...";
                return RedirectToAction("SubscriptionList");
            }
            else
            {
                TempData["Msg"] = "Occor Some Please Try Again...";
                return RedirectToAction("SubscriptionList");
            }
        }

        #region Daily ReportBook Details

        public ActionResult DailyReportBook()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/SingleRptSubscription/DailyReportBook");
            if (FindFlag)
            {
                List<DailyReportBookModel> lstDailyRptBook = new List<DailyReportBookModel>();
                try
                {
                    RDSRptSubBL objBL = new RDSRptSubBL();
                    lstDailyRptBook = objBL.GetDailyRptBookList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(lstDailyRptBook);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public void ExportToExcel(string GridModel)
        {
            string user_Id = User.Identity.GetUserId();
            RDSRptSubBL objBL = new RDSRptSubBL();
            var DataSource = objBL.GetDailyRptBookList();
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
        public ActionResult ViewSubscriptionDetail(int RptSubID, int DlyGenSubId)
        {
            RDSRptSubModel rdsRPTSubModel = new RDSRptSubModel();
            try
            {
                RDSRptSubBL objBL = new RDSRptSubBL();
                if (DlyGenSubId > 0)
                {
                    RptSubID = objBL.GetRptSubId(DlyGenSubId);

                }

                rdsRPTSubModel = objBL.GetReportClientListByUser(0);
                ViewBag.ReportList = new SelectList(rdsRPTSubModel.RDSReportList, "ReportId", "ReportName");
                ViewBag.ClientList = new SelectList(rdsRPTSubModel.ClientList, "ClientId", "ClientName");
                rdsRPTSubModel = objBL.GetSelectedRptSubScrip(RptSubID);

                slst = new SelectList(rdsRPTSubModel.RDSRptSubGenTimeList);

                ViewBag.ListGenTime = rdsRPTSubModel.RDSRptSubGenTimeList.Select(x =>
                                      new SelectListItem()
                                      {
                                          Text = x.ToString(),
                                          Value = x.ToString()
                                      });
                if (DlyGenSubId > 0)
                {
                    rdsRPTSubModel.RptSubId = RptSubID;
                    rdsRPTSubModel.DlyGenSubId = DlyGenSubId;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(rdsRPTSubModel);
        }
        public ActionResult ViewDailyRptParam(int DlyGenSubId)
        {
            List<DailyReportBookModel> lstDailyRptBookParam = new List<DailyReportBookModel>();
            try
            {
                RDSRptSubBL objBL = new RDSRptSubBL();
                lstDailyRptBookParam = objBL.GetDailyRptBookParam(DlyGenSubId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(lstDailyRptBookParam);
        }

        public ActionResult Download(string FileName)
        {
            try
            {
                String FTPServer = Common.GetConfigValue("FTP");
                String FTPUserId = Common.GetConfigValue("FTPUid");
                String FTPPwd = Common.GetConfigValue("FTPPWD");

                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + FileName);
                requestFileDownload.UseBinary = true;
                requestFileDownload.KeepAlive = false;
                requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                byte[] OPData = ReadFully(responseStream);
                responseStream.Close();

                Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
                Response.ContentType = "application/zip";
                Response.BinaryWrite(OPData);
                Response.End();
                return RedirectToAction("GetList");
            }
            catch (Exception e)
            {

                throw e;
            }

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
        public ActionResult ShowError(int DlyGenSubId)
        {
            RDSRptSubBL objBL = new RDSRptSubBL();
            DataSet dsAdhoc = objBL.GetDlyBookErr(DlyGenSubId);
            DailyReportBookModel dailyBookModel = new DailyReportBookModel();
            dailyBookModel.Description = dsAdhoc.Tables[0].Rows[0]["Description"].ToString();
            ViewBag.ErrorMsg = dailyBookModel.Description;
            return PartialView();
        }
        public ActionResult SetReDistStatus(int DlyGenSubId)
        {
            int errcode;
            try
            {
                RDSRptSubBL objBL = new RDSRptSubBL();
                errcode = objBL.SetReDistStatus(DlyGenSubId);
                if (errcode == 500001)
                {
                    return RedirectToAction("DailyReportBook");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("DailyReportBook");
        }
        #endregion Daily ReportBook DAtails
    }
}