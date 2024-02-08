using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.Adhoc;
using SmartSys.BL.DW;
using SmartSys.BL.RDS;
using SmartSys.BL.RGS;
using SmartSys.BL.Sysadmin;
using SmartSys.BL.TimeManagement;
using SmartSys.BL.Tmleave;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{

    [Authorize]
    public class AdhocReportingController : Controller
    {
        public ActionResult ReportParam(string ReportId)
        {
            string User_Id = User.Identity.GetUserId();
            AdhocModel objAdhocRun = new AdhocModel();
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
                        else if (dr["ParamName"].ToString() == "userID" || dr["ParamName"].ToString() == "@userID")
                        {
                            paramMadal prmmodParam = new paramMadal();
                            if (Session["ParamDescrip"] != null)
                            {
                                prmmodParam = Session["ParamDescrip"] as paramMadal;
                            }
                            string tempParamValue = getUserId();
                            prmmodParam.ParamName = dr["ParamName"].ToString();
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
                            Demo.DisplayName = dr["DisplayName"].ToString();
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
                                SysEmploy objmodel1 = new SysEmploy();
                                objmodel1.EmpId = Convert.ToInt16(0);
                                objmodel1.EmpName = "Please Select Employee";
                                lstEmp.Add(objmodel1);
                            }
                        }

                        ViewBag.EmpList = new SelectList(lstEmp, "EmpId", "EmpName");
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

                    if (prmmod.FunctionRef.ToString().Length > 10 && prmmod.FunctionRef.ToString().Substring(0, 8) == "CLNTUSER")
                    {
                        // dr["ParamName"].ToString().Length > 11 && dr["ParamName"].ToString().Substring(0,7) =="CLNTUSER"
                        string CompCode = prmmod.FunctionRef.ToString().Substring(8, prmmod.FunctionRef.ToString().Length - 8);
                        AdhocBL BlObj = new AdhocBL();
                        List<ClientEmpCode> clientEmpCodeModel = new List<ClientEmpCode>();
                        clientEmpCodeModel = BlObj.GetClientEmpCodeLst(CompCode);
                        ViewBag.CLNTUSER = new SelectList(clientEmpCodeModel, "Emp_No", "Name");
                    }
                    #endregion for Client Emp DropDown List

                    #region for Client Customer  DropDown List
                    //------------------------------------Sys Dropdown List--------------------------------//

                    if (prmmod.FunctionRef.ToString().Length > 10 && prmmod.FunctionRef.ToString().Substring(0, 8) == "CLNTCUST")
                    {
                        // dr["ParamName"].ToString().Length > 11 && dr["ParamName"].ToString().Substring(0,7) =="CLNTUSER"
                        string CompCode = prmmod.FunctionRef.ToString().Substring(8, prmmod.FunctionRef.ToString().Length - 8);
                        AdhocBL BlObj = new AdhocBL();
                        List<ClientCustCode> clientCustCodeModel = new List<ClientCustCode>();
                        clientCustCodeModel = BlObj.GetClientCustCodeLst(CompCode);
                        ViewBag.CLNTCUST = new SelectList(clientCustCodeModel, "Cust_No", "Cust_Name");
                    }
                    #endregion for Client Customer DropDown List

                    #region for Bank Detail DropDown
                    //---------------------Business Line DropDown list----------------------------------------------//           
                    if (prmmod.FunctionRef.ToString().Length > 10 && prmmod.FunctionRef.ToString().Substring(0, 8) == "BANKUSER")
                    {
                        string CompCode = prmmod.FunctionRef.ToString().Substring(8, prmmod.FunctionRef.ToString().Length - 8);
                        List<BankListModel> lstBankDetail = new List<BankListModel>();
                        AdminBL BLobj = new AdminBL();
                        lstBankDetail = BLobj.GetBankList(CompCode);
                        ViewBag.BankLstbyCompCode = new SelectList(lstBankDetail, "BankId", "BankId");
                    }

                    #endregion for Bank Detail DropDown

                    #region for Vendor Id DropDown
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.SmartSysVendorDropDownByUser.ToString())
                    {
                        List<VendoridModel> VendorList = new List<VendoridModel>();
                        VendorBL BLobj = new VendorBL();
                        VendorList = BLobj.GetVendorIdList(User_Id);
                        ViewBag.SmartSysVendorDropDownByUser = new SelectList(VendorList, "VendorId", "VendorName");
                    }
                    #endregion for Vendor Id DropDown

                    #region for Customer Id DropDown
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.SmartSysCustDropDownByUser.ToString())
                    {
                        List<CustomeridModel> CustList = new List<CustomeridModel>();
                        AdminBL BLobj = new AdminBL();
                        CustList = BLobj.GetCustomerIdList(User_Id);
                        CustomeridModel Demo = new CustomeridModel();
                        Demo.CustomerId = Convert.ToInt32(0);
                        Demo.CustomerName = "Please Select Customer";
                        CustList.Add(Demo);
                        ViewBag.SmartSysCustDropDownByUser = new SelectList(CustList, "CustomerId", "CustomerName");
                    }
                    #endregion for Customer Id DropDown

                    #region for Vendor Id DropDown
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.SmartSysVendorDropDownByUser.ToString())
                    {
                        List<VendoridModel> VendorList = new List<VendoridModel>();
                        VendorBL BLobj = new VendorBL();
                        VendorList = BLobj.GetVendorIdList(User_Id);
                        ViewBag.SmartSysVendorDropDownByUser = new SelectList(VendorList, "VendorId", "VendorName");
                    }
                    #endregion for Vendor Id DropDown

                    #region for Project Id DropDown
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.SmartSysProjectDropDownByUser.ToString())
                    {

                        //string User_Id = User.Identity.GetUserId();
                        TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
                        TMLeaveBL BLObj = new TMLeaveBL();
                        TMModel.TMProjectlist = BLObj.GetProjectList(User_Id);
                        ViewBag.SmartSysProjectDropDownByUser = new SelectList(TMModel.TMProjectlist, "ProjectId", "ProjectName");
                    }
                    #endregion for Project Id DropDown

                    #region for Item Id DropDown
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.ItemDropDown.ToString())
                    {

                        ItemModel itemModel = new ItemModel();
                        ItemBL BLObj = new ItemBL();
                        itemModel.lstItem = new List<ItemModel>();
                        itemModel.lstItem = BLObj.GetItemListwithMPN();
                        ItemModel objmodel = new ItemModel();
                        objmodel.ItemId = Convert.ToInt32(0);
                        objmodel.ItemName = "Please Select Item";
                        itemModel.lstItem.Add(objmodel);
                        ViewBag.ItemDropDown = new SelectList(itemModel.lstItem, "ItemId", "ItemName");
                    }
                    #endregion for Item Id DropDown

                    #region for RegionId
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.CityDropDown.ToString())
                    {
                        ContyStateCityModel addressModel = new ContyStateCityModel();
                        AdminBL ADBLObj = new AdminBL();
                        addressModel = ADBLObj.GetCountryStateCityList();
                        AddCityModel Citymodel = new AddCityModel();
                        Citymodel.CityId = Convert.ToInt32(0);
                        Citymodel.CityName = "Please Select City";
                        addressModel.LstCity.Add(Citymodel);
                        ViewBag.CityDropDown = new SelectList(addressModel.LstCity, "CityId", "CityName");
                    }
                    #endregion for RegionId

                    #region for CityId
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.RegionDropDown.ToString())
                    {
                        ContyStateCityModel addressModel = new ContyStateCityModel();
                        AdminBL ADBLObj = new AdminBL();
                        addressModel = ADBLObj.GetCountryStateCityList();
                        AddRegionModel regionmodel = new AddRegionModel();
                        regionmodel.RegionId = Convert.ToInt32(0);
                        regionmodel.RegionName = "Please Select Region";
                        addressModel.LstRegion.Add(regionmodel);
                        ViewBag.RegionDropDown = new SelectList(addressModel.LstRegion, "RegionId", "RegionName");
                    }
                    #endregion for CityId

                    #region for Employee by UserId
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.EmpDropDownByUserId.ToString())
                    {
                        BudgetModel budgetModel = new BudgetModel();
                        AdminBL objBL = new AdminBL();
                        budgetModel.EmployeeLst = new List<BudgetCustmerModel>();
                        budgetModel.EmployeeLst = objBL.GetBudgetEmployeeList(User_Id); // Employee List
                        BudgetCustmerModel EmpLstModel = new BudgetCustmerModel();
                        EmpLstModel.EmployeeId = Convert.ToInt32(0);
                        EmpLstModel.EmployeeName = "Please Select Employee";
                        budgetModel.EmployeeLst.Add(EmpLstModel);
                        ViewBag.EmployeeList = new SelectList(budgetModel.EmployeeLst, "EmployeeId", "EmployeeName");
                    }
                    #endregion for Employee by UserId

                    #region for FinacialYear
                    if (prmmod.FunctionRef == SmartSys.Utility.Enums.FunctionRef.FYDropDown.ToString())
                    {
                        AdhocBL objBL = new AdhocBL();
                        AdhocModel FYDrpDwnLst = new AdhocModel();
                        FYDrpDwnLst.FYearDrpDwn = objBL.FYDropDown();
                        AdhocModel FYModel = new AdhocModel();
                        FYModel.FYearId = Convert.ToString(0);
                        FYModel.FYear = "Please Select FYear";
                       // FYModel.Add(FYModel);
                        FYDrpDwnLst.FYearDrpDwn.Add(FYModel);

                        ViewBag.FYDrpDwnLst = new SelectList(FYDrpDwnLst.FYearDrpDwn, "FYearId", "FYear");
                    }
                    #endregion for FinacialYear

                    #region for Currency 
                    if(prmmod.FunctionRef==Enums.FunctionRef.CurrencyDrpDwn.ToString())
                    {
                        TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                        List<CurrencyCodes> lstModel = new List<CurrencyCodes>();
                        CurrencyCodes CurModel = new CurrencyCodes();
                        CurModel.Key = Convert.ToString(0);
                        CurModel.Value = "Please Select Currency";
                        lstModel = ObjTMBL.GetCurrencyCodes();
                        lstModel.Add(CurModel);
                        ViewBag.Currency = new SelectList(lstModel, "Key", "Value");
                    }
                    #endregion for Currency DropDown

                    #region For Year 

                    #endregion For Year 

                    #region For Month 

                    #endregion For Month 
                }
            }
            if (HasParameter)
                return View(objAdhocRun);
            else
                return ReportParam(null, ReportId, "ABC", "XYZ");
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
                string Username = "";
                if (fc != null)
                    Username = fc["hidText"].ToString();
                String User_Id = User.Identity.GetUserId();
                int errorcode = 0;
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
                        errorcode = objBL.SaveAdhocRun(dsAdhoc, User_Id);
                        Session["ParamDescrip"] = null;
                        if (errorcode == 0 || errorcode == 1 || errorcode == 2 || errorcode == 3)
                        {
                            TempData["Msg"] = " Occer some Error Please Try Again ...";
                            return RedirectToAction("GetList");
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("GetList");
        }
        public ActionResult GetList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/AdhocReporting/GetList");
            if (FindFlag)
            {
                string user_Id = User.Identity.GetUserId();
                List<AdhocModel> lstAdhocModel = new List<AdhocModel>();
                AdhocBL ObjBL = new AdhocBL();
                lstAdhocModel = ObjBL.GetList(user_Id);
                //-------------------------------------------------------------------------------------------------------//
                #region RGSRepot List For Drop Down
                List<RGSReportNameModel> lstReport = new List<RGSReportNameModel>();
                DataSet dsReport = new DataSet();
                AdhocBL objBL = new AdhocBL();
                dsReport = objBL.GetReportNameList(user_Id);
                if (dsReport != null)
                {
                    if (dsReport.Tables.Count > 0)
                    {

                        foreach (DataRow dr in dsReport.Tables[0].Rows)
                        {
                            RGSReportNameModel lstbl = new RGSReportNameModel();
                            lstbl.ReportId = dr["ReportId"].ToString();
                            lstbl.ReportName = dr["ReportName"].ToString();
                            lstReport.Add(lstbl);
                        }
                    }
                }

                ViewBag.ReportNameList = new SelectList(lstReport, "ReportId", "ReportName");
                #endregion RGSRepot List For Drop Down

                return View(lstAdhocModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Download(string FileName, FormCollection fc)
        {
            for (int i = 0; i < 3; i++)
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
                    Response.Close();
                    responseFileDownload = null;
                    requestFileDownload = null;
                    responseStream.Close();
                    responseStream.Dispose();
                    return RedirectToAction("GetList");
                }
                catch (Exception ex)
                {
                    if (i > 2)
                    {
                        throw ex;
                    }
                }
            }
            return RedirectToAction("GetList");
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
        public ActionResult ViewAdhocParam(int AdhocRunId)
        {
            List<AdhocParameterModel> adhocModel = new List<AdhocParameterModel>();
            AdhocBL ObjBL = new AdhocBL();
            adhocModel = ObjBL.GetViewParamList(AdhocRunId);

            return PartialView(adhocModel);
        }
        public ActionResult ShowError(int AdhocRunId)
        {
            AdhocBL objBL = new AdhocBL();
            DataSet dsAdhoc = objBL.GetSelectedList(AdhocRunId);
            AdhocModel adhocModel = new AdhocModel();
            adhocModel.Description = dsAdhoc.Tables[0].Rows[0]["Description"].ToString();
            ViewBag.ErrorMsg = adhocModel.Description;
            return PartialView();
        }

    }
}