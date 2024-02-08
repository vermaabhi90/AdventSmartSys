using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.Adhoc;
using SmartSys.BL.DW;
using SmartSys.BL.ProViews;
using SmartSys.BL.TimeManagement;
using Syncfusion.EJ.Export;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SmartSys.Controllers
{
    public class BudgetController : Controller
    {
        public ActionResult CreateEquipment(int EquipmentId)
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
                if (FindFlag)
                {
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
        public ActionResult DeleteEquipmentItem(int EquipmentId, int ItemId)
        {
            string User_Id = User.Identity.GetUserId();
            TMEquipmentBL ObjTMBL = new TMEquipmentBL();
            int Errorcode = ObjTMBL.DeleteEquipmentItem(ItemId, EquipmentId, User_Id);
            return RedirectToAction("CreateEquipment", new { EquipmentId = EquipmentId });
        }
        public ActionResult DeleteEquipmentSegment(int EquipmentId, int SegmentId)
        {
            string User_Id = User.Identity.GetUserId();
            TMEquipmentBL ObjTMBL = new TMEquipmentBL();
            int Errorcode = ObjTMBL.DeleteEquipmentSegment(SegmentId, EquipmentId, User_Id);
            return RedirectToAction("CreateEquipment", new { EquipmentId = EquipmentId });
        }
        public JsonResult GetSalesPurchaseRateByItem(int Itemid)
        {

            DataSet ds = new DataSet();
            TMEquipmentBL ObjTMBL = new TMEquipmentBL();
            ds = ObjTMBL.GetItemPurchaseSalesRate(Itemid);
            ArrayList lst = new ArrayList();
            lst.Add(ds.Tables[0].Rows[0]["PurchaseRate"]);
            lst.Add(ds.Tables[0].Rows[0]["SaleRate"]);

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateEquipment(FormCollection fc)
        {
            try
            {
                TMEquipmentModel objmodel = new TMEquipmentModel();
                TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                string User_Id = User.Identity.GetUserId();
                if (fc["EquipmentId"].ToString() == "0")
                {
                    objmodel.EquipmentId = 0;
                }
                else
                {

                    objmodel.EquipmentId = Convert.ToInt32(fc["EquipmentId"].ToString());
                }
                if (fc["ParentEquipmentId"].ToString() == "")
                {
                    objmodel.ParentEquipmentId = 0;
                }
                else
                {

                    objmodel.ParentEquipmentId = Convert.ToInt32(fc["ParentEquipmentId"]);
                }
                objmodel.EquipmentName = fc["EquipmentName"].ToString();
                objmodel.Description = fc["Description"].ToString();
                objmodel.Rate = Convert.ToDouble(fc["Rate"]);
                objmodel.CurrencyCodes = fc["CurrencyCodes"].ToString();
                objmodel.TAM = Convert.ToDouble(fc["TAM"]);
                objmodel.TAMSource = fc["TAMSource"].ToString();
                objmodel.TAMDate = Convert.ToDateTime(fc["TAMDate"]);
                int errCode = ObjTMBL.saveEquipmentDetails(objmodel, User_Id);
                if (errCode == 500001 || errCode == 500002)
                {

                    return RedirectToAction("EquipmentList");

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("EquipmentList");
        }
        public ActionResult saveEquipmentparam(int EquipmentId, double Quantity, int Itemid, double PurchaseRate, double SaleRate)
        {
            string User_Id = User.Identity.GetUserId();
            TMEquipmentBL ObjTMBL = new TMEquipmentBL();
            int errcode = ObjTMBL.saveEquipmentItem(EquipmentId, Quantity, Itemid, User_Id, PurchaseRate, SaleRate);
            return View();
        }
        public ActionResult SaveSegmentDetail(int SegmentId, int EquipmentId)
        {
            string User_Id = User.Identity.GetUserId();
            TMEquipmentBL ObjTMBL = new TMEquipmentBL();
            int errcode = ObjTMBL.SaveSegmentDetail(SegmentId, EquipmentId, User_Id);
            return RedirectToAction("CreateEquipment", new { EquipmentId = EquipmentId });
        }
        public ActionResult EquipmentList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Budget/EquipmentList");
            if (FindFlag)
            {
                TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                List<TMEquipmentModel> lstTMEquipment = new List<TMEquipmentModel>();
                ViewBag.SelectedEquipmentItems = ObjTMBL.GetTMEquipmentList();
                return View(lstTMEquipment);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportEquipmentLst")]
        [AcceptVerbs("POST")]
        public void ExportEquipmentLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.TreeGridProperties obj = ConvertTreeGridObject(GridModel);
            TMEquipmentBL ObjTMBL = new TMEquipmentBL();
            var DataSource = ObjTMBL.GetTMEquipmentList();
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, new TreeGridExportSettings() { Theme = ExportTheme.FlatSaffron });
        }
        private Syncfusion.JavaScript.Models.TreeGridProperties ConvertTreeGridObject(string gridProperty)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IEnumerable div = (IEnumerable)serializer.Deserialize(gridProperty, typeof(IEnumerable));
            Syncfusion.JavaScript.Models.TreeGridProperties gridProp = new Syncfusion.JavaScript.Models.TreeGridProperties();
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

        public ActionResult BudgetList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Budget/BudgetList");
            if (FindFlag)
            {
                BudgetModel budgetModel = new BudgetModel();
                string User_Id = User.Identity.GetUserId();
                try
                {
                    Session["BudgetDetail"] = null;
                    BudgetBL ObjBl = new BudgetBL();
                    AdminBL objBL = new AdminBL();
                    //budgetModel.lstBudgetDetail = ObjBl.getBudgetList();
                    budgetModel.EmployeeLst = objBL.GetBudgetEmployeeList(User_Id); // Employee List
                    ViewBag.EmployeeList = new SelectList(budgetModel.EmployeeLst, "EmployeeId", "EmployeeName");

                    ReportNameModel Model = new ReportNameModel();
                    Model = objBL.GetOpenBudgetReport(User_Id);
                    budgetModel.OutputLocation = Model.OutputLocation;
                    budgetModel.RunDate = Model.RunDate;
                    budgetModel.StatusId = Model.StatusId;
                    budgetModel.ReportId = Model.ReportId;
                    budgetModel.ParamId = "@CityCode,@EmpByUserId,@ItemId,@RegionCode,@SmartSysCust";
                    budgetModel.ParamName = "";
                    budgetModel.hidText = "";
                    budgetModel.TxtParamValue = "0,0,0,0,0";
                    paramMadal prmmodParamDemo = new paramMadal();
                    prmmodParamDemo.lstAdhocParam = new List<paramMadal>();
                    paramMadal prmmodParam = new paramMadal();
                    if (Session["ParamDescrip"] != null)
                    {
                        prmmodParam = Session["ParamDescrip"] as paramMadal;
                    }
                    prmmodParam.ParamName = "@userID";
                    prmmodParam.ParamValue = User_Id;
                    prmmodParamDemo.lstAdhocParam.Add(prmmodParam);
                    Session["ParamDescrip"] = prmmodParamDemo;// Session Used By Vijaya

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(budgetModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public FileResult DownloadBudgetSample()
        {
            var filepath = Path.Combine(Server.MapPath("~/App_Data/MyFiles/SampleBudgetExcel.xlsx"));
            string fileName = "SampleBudget.xlsx";
            return File(filepath, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult GetBudgetData()
        {
            BudgetModel budgetModel = new BudgetModel();
            budgetModel.lstBudgetDetail = new List<BudgetModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                BudgetBL ObjBl = new BudgetBL();
                budgetModel.lstBudgetDetail = ObjBl.getBudgetList(User_Id);
                IEnumerable data = budgetModel.lstBudgetDetail;
                var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetSelectedBudgetCustView(int CustomerId)
        {
            BudgetModel budgetModel = new BudgetModel();
            budgetModel.lstBudgetDetail = new List<BudgetModel>();
            try
            {
                BudgetBL ObjBl = new BudgetBL();
                budgetModel.lstBudgetDetail = ObjBl.GetSelectedBudgetCustView(CustomerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(budgetModel);
        }
        [System.Web.Http.ActionName("ExportBudgetList")]
        [AcceptVerbs("POST")]
        public void ExportBudgetList(string GridModel)
        {
            string user_Id = User.Identity.GetUserId();
            BudgetBL ObjBl = new BudgetBL();
            var DataSource = ObjBl.getBudgetList(user_Id);
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
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
        public ActionResult CreateBudget()
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
            string User_Id = User.Identity.GetUserId();
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Budget/CreateBudget");
            if (FindFlag)
            {
                BudgetModel budgetModel = new BudgetModel();
                try
                {
                    BudgetBL ObjBl = new BudgetBL();

                    if (Session["BudgetDetail"] != null)
                    {

                        budgetModel = Session["BudgetDetail"] as BudgetModel;
                        CustomerBL objbl = new CustomerBL();
                        budgetModel.CustomerLst = objbl.GetBudgetCustomerList(); //Customer List
                        ViewBag.VendrList = new SelectList(budgetModel.CustomerLst, "CustomerId", "CustomerName");

                        budgetModel.ItemLst = new List<BudgetItemModel>();
                        ItemBL BLObj = new ItemBL();
                        budgetModel.ItemLst = BLObj.GetBudgetItemList();  //Item LIst
                        ViewBag.ItemList = new SelectList(budgetModel.ItemLst, "ItemId", "ItemName");

                        budgetModel.EmployeeLst = new List<BudgetCustmerModel>();
                        AdminBL objBL = new AdminBL();
                        budgetModel.EmployeeLst = objBL.GetBudgetEmployeeList(User_Id); // Employee List
                        ViewBag.EmployeeList = new SelectList(budgetModel.EmployeeLst, "EmployeeId", "EmployeeName");
                    }
                    else
                    {
                        budgetModel.CustomerLst = new List<BudgetCustomerModel>();
                        CustomerBL objbl = new CustomerBL();
                        budgetModel.CustomerLst = objbl.GetBudgetCustomerList(); //Customer List
                        ViewBag.VendrList = new SelectList(budgetModel.CustomerLst, "CustomerId", "CustomerName");

                        budgetModel.ItemLst = new List<BudgetItemModel>();
                        ItemBL BLObj = new ItemBL();
                        budgetModel.ItemLst = BLObj.GetBudgetItemList();  //Item LIst
                        ViewBag.ItemList = new SelectList(budgetModel.ItemLst, "ItemId", "ItemName");

                        budgetModel.EmployeeLst = new List<BudgetCustmerModel>();
                        AdminBL objBL = new AdminBL();
                        budgetModel.EmployeeLst = objBL.GetBudgetEmployeeList(User_Id); // Employee List
                        ViewBag.EmployeeList = new SelectList(budgetModel.EmployeeLst, "EmployeeId", "EmployeeName");

                    }
                    ContyStateCityModel addressModel = new ContyStateCityModel();
                    AdminBL ADBLObj = new AdminBL();
                    addressModel = ADBLObj.GetCountryStateCityList();
                    ViewBag.LstRegion = new SelectList(addressModel.LstRegion, "RegionId", "RegionName");
                    ViewBag.LstCitry = new SelectList(addressModel.LstCity, "CityId", "CityName");

                    TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                    List<CurrencyCodes> lstModel = new List<CurrencyCodes>();
                    lstModel = ObjTMBL.GetCurrencyCodes();
                    ViewBag.Currencylst = new SelectList(lstModel, "Key", "Value");

                    if (budgetModel.EquipmentLst == null)
                        budgetModel.EquipmentLst = new List<BudgetEquipmentModel>();
                    if (budgetModel.EquipmentLst != null && budgetModel.EquipmentLst.Count > 0)
                    {
                        ViewBag.EquipmentList = new SelectList(budgetModel.EquipmentLst, "EquipmentId", "EquipmentName");
                    }
                    Session["BudgetDetail"] = budgetModel;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(budgetModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateBudget(FormCollection fc)
        {
            try
            {
                int errCode = 0;
                BudgetBL ObjBl = new BudgetBL();
                BudgetModel budgetModel = new BudgetModel();
                budgetModel.BudgetId = Convert.ToInt32(0);
                budgetModel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                if (fc["ItemId"] != null || fc["ItemId"].ToString() != "")
                {
                    budgetModel.ItemId = Convert.ToInt32(fc["ItemId"].ToString());
                }
                else
                {
                    budgetModel.ItemId = Convert.ToInt32(fc["ItemId"].ToString());
                }
                budgetModel.EmpId = Convert.ToInt32(fc["EmpId"].ToString());
                budgetModel.Application = fc["Application"].ToString();
                if (fc.AllKeys.Contains("EndEquipment"))
                    budgetModel.EndEquipment = Convert.ToString(fc["EndEquipment"].ToString());
                budgetModel.Finyear = fc["Finyear"].ToString();
                budgetModel.CCY = fc["CCY"].ToString().ToUpper();
                budgetModel.UnitPrice = Convert.ToDouble(fc["UnitPrice"].ToString());
                budgetModel.CityId = Convert.ToInt32(fc["CityId"].ToString());
                budgetModel.RegionId = Convert.ToInt32(fc["RegionId"].ToString());
                if (budgetModel.UnitPrice > 0.0)
                {
                    budgetModel.AprQty = Convert.ToDouble(fc["AprQty"].ToString());
                    budgetModel.MayQty = Convert.ToDouble(fc["MayQty"].ToString());
                    budgetModel.JunQty = Convert.ToDouble(fc["JunQty"].ToString());
                    budgetModel.JulQty = Convert.ToDouble(fc["JulQty"].ToString());
                    budgetModel.AugQty = Convert.ToDouble(fc["AugQty"].ToString());
                    budgetModel.SepQty = Convert.ToDouble(fc["SepQty"].ToString());
                    budgetModel.OctQty = Convert.ToDouble(fc["OctQty"].ToString());
                    budgetModel.NovQty = Convert.ToDouble(fc["NovQty"].ToString());
                    budgetModel.DecQty = Convert.ToDouble(fc["DecQty"].ToString());
                    budgetModel.JanQty = Convert.ToDouble(fc["JanQty"].ToString());
                    budgetModel.FebQty = Convert.ToDouble(fc["FebQty"].ToString());
                    budgetModel.MarQty = Convert.ToDouble(fc["MarQty"].ToString());

                    budgetModel.AprUSD = budgetModel.AprQty * budgetModel.UnitPrice;
                    budgetModel.MayUSD = budgetModel.MayQty * budgetModel.UnitPrice;
                    budgetModel.JunUSD = budgetModel.JunQty * budgetModel.UnitPrice;
                    budgetModel.JulUSD = budgetModel.JulQty * budgetModel.UnitPrice;
                    budgetModel.AugUSD = budgetModel.AugQty * budgetModel.UnitPrice;
                    budgetModel.SepUSD = budgetModel.SepQty * budgetModel.UnitPrice;
                    budgetModel.OctUSD = budgetModel.OctQty * budgetModel.UnitPrice;
                    budgetModel.NovUSD = budgetModel.NovQty * budgetModel.UnitPrice;
                    budgetModel.DecUSD = budgetModel.DecQty * budgetModel.UnitPrice;
                    budgetModel.JanUSD = budgetModel.JanQty * budgetModel.UnitPrice;
                    budgetModel.FebUSD = budgetModel.FebQty * budgetModel.UnitPrice;
                    budgetModel.MarUSD = budgetModel.MarQty * budgetModel.UnitPrice;
                }
                else
                {
                    budgetModel.AprQty = Convert.ToDouble(fc["AprQty"].ToString());
                    budgetModel.MayQty = Convert.ToDouble(fc["MayQty"].ToString());
                    budgetModel.JunQty = Convert.ToDouble(fc["JunQty"].ToString());
                    budgetModel.JulQty = Convert.ToDouble(fc["JulQty"].ToString());
                    budgetModel.AugQty = Convert.ToDouble(fc["AugQty"].ToString());
                    budgetModel.SepQty = Convert.ToDouble(fc["SepQty"].ToString());
                    budgetModel.OctQty = Convert.ToDouble(fc["OctQty"].ToString());
                    budgetModel.NovQty = Convert.ToDouble(fc["NovQty"].ToString());
                    budgetModel.DecQty = Convert.ToDouble(fc["DecQty"].ToString());
                    budgetModel.JanQty = Convert.ToDouble(fc["JanQty"].ToString());
                    budgetModel.FebQty = Convert.ToDouble(fc["FebQty"].ToString());
                    budgetModel.MarQty = Convert.ToDouble(fc["MarQty"].ToString());

                    budgetModel.AprUSD = Convert.ToDouble(fc["AprUSD"].ToString());
                    budgetModel.MayUSD = Convert.ToDouble(fc["MayUSD"].ToString());
                    budgetModel.JunUSD = Convert.ToDouble(fc["JunUSD"].ToString());
                    budgetModel.JulUSD = Convert.ToDouble(fc["JulUSD"].ToString());
                    budgetModel.AugUSD = Convert.ToDouble(fc["AugUSD"].ToString());
                    budgetModel.SepUSD = Convert.ToDouble(fc["SepUSD"].ToString());
                    budgetModel.OctUSD = Convert.ToDouble(fc["OctUSD"].ToString());
                    budgetModel.NovUSD = Convert.ToDouble(fc["NovUSD"].ToString());
                    budgetModel.DecUSD = Convert.ToDouble(fc["DecUSD"].ToString());
                    budgetModel.JanUSD = Convert.ToDouble(fc["JanUSD"].ToString());
                    budgetModel.FebUSD = Convert.ToDouble(fc["FebUSD"].ToString());
                    budgetModel.MarUSD = Convert.ToDouble(fc["MarUSD"].ToString());
                }

                errCode = ObjBl.SaveBudgetDetail(budgetModel, User.Identity.GetUserId());
                if (errCode == 500002)
                {
                    Session["BudgetDetail"] = null;
                    return RedirectToAction("BudgetList");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("BudgetList");
        }
        public ActionResult EditBudget(int BudgetId)
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
            string User_Id = User.Identity.GetUserId();
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Budget/BudgetList");
            if (FindFlag)
            {
                BudgetModel budgetModel = new BudgetModel();
                try
                {
                    BudgetBL ObjBl = new BudgetBL();
                    if (Session["BudgetDetail"] != null)
                    {

                        budgetModel = Session["BudgetDetail"] as BudgetModel;

                        ViewBag.VendrList = new SelectList(budgetModel.CustomerLst, "CustomerId", "CustomerName");
                        ViewBag.ItemList = new SelectList(budgetModel.ItemLst, "ItemId", "ItemName");
                        ViewBag.EmployeeList = new SelectList(budgetModel.EmployeeLst, "EmployeeId", "EmployeeName");
                    }
                    else
                    {
                        budgetModel = ObjBl.GetselectedBudget(BudgetId);

                        budgetModel.CustomerLst = new List<BudgetCustomerModel>();
                        CustomerBL objbl = new CustomerBL();
                        budgetModel.CustomerLst = objbl.GetBudgetCustomerList(); //Customer List
                        ViewBag.VendrList = new SelectList(budgetModel.CustomerLst, "CustomerId", "CustomerName");

                        budgetModel.ItemLst = new List<BudgetItemModel>();
                        ItemBL BLObj = new ItemBL();
                        budgetModel.ItemLst = BLObj.GetBudgetItemList();  //Item LIst
                        ViewBag.ItemList = new SelectList(budgetModel.ItemLst, "ItemId", "ItemName");

                        budgetModel.EmployeeLst = new List<BudgetCustmerModel>();
                        AdminBL objBL = new AdminBL();
                        budgetModel.EmployeeLst = objBL.GetBudgetEmployeeList(User_Id); // Employee List
                        ViewBag.EmployeeList = new SelectList(budgetModel.EmployeeLst, "EmployeeId", "EmployeeName");

                        budgetModel.EquipmentLst = new List<BudgetEquipmentModel>();
                        budgetModel.EquipmentLst = ObjBl.getEquipmentList(budgetModel.ItemId);
                    }

                    ContyStateCityModel addressModel = new ContyStateCityModel();
                    AdminBL ADBLObj = new AdminBL();
                    addressModel = ADBLObj.GetCountryStateCityList();
                    ViewBag.LstRegion = new SelectList(addressModel.LstRegion, "RegionId", "RegionName");
                    ViewBag.LstCitry = new SelectList(addressModel.LstCity, "CityId", "CityName");

                    TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                    List<CurrencyCodes> lstModel = new List<CurrencyCodes>();
                    lstModel = ObjTMBL.GetCurrencyCodes();
                    ViewBag.Currencylst = new SelectList(lstModel, "Key", "Value");
                    if (budgetModel.EquipmentLst == null)
                        budgetModel.EquipmentLst = new List<BudgetEquipmentModel>();
                    if (budgetModel.EquipmentLst != null && budgetModel.EquipmentLst.Count > 0)
                    {
                        ViewBag.EquipmentList = new SelectList(budgetModel.EquipmentLst, "EquipmentId", "EquipmentName");
                    }

                    Session["BudgetDetail"] = budgetModel;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(budgetModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult EditBudget(FormCollection fc)
        {
            try
            {
                int errCode = 0;
                BudgetBL ObjBl = new BudgetBL();
                BudgetModel budgetModel = new BudgetModel();
                budgetModel.BudgetId = Convert.ToInt32(fc["BudgetId"].ToString());
                budgetModel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                budgetModel.ItemId = Convert.ToInt32(fc["ItemId"].ToString());
                budgetModel.EmpId = Convert.ToInt32(fc["EmpId"].ToString());
                budgetModel.Application = fc["Application"].ToString();
                if (fc.AllKeys.Contains("EndEquipment"))
                {
                    budgetModel.EndEquipment = fc["EndEquipment"].ToString();
                }
                budgetModel.CCY = fc["CCY"].ToString().ToUpper();
                budgetModel.UnitPrice = Convert.ToDouble(fc["UnitPrice"].ToString());
                budgetModel.Finyear = fc["Finyear"].ToString();
                budgetModel.CityId = Convert.ToInt32(fc["CityId"].ToString());
                budgetModel.RegionId = Convert.ToInt32(fc["RegionId"].ToString());
                if (budgetModel.UnitPrice > 0.0)
                {
                    budgetModel.AprQty = Convert.ToDouble(fc["AprQty"].ToString());
                    budgetModel.MayQty = Convert.ToDouble(fc["MayQty"].ToString());
                    budgetModel.JunQty = Convert.ToDouble(fc["JunQty"].ToString());
                    budgetModel.JulQty = Convert.ToDouble(fc["JulQty"].ToString());
                    budgetModel.AugQty = Convert.ToDouble(fc["AugQty"].ToString());
                    budgetModel.SepQty = Convert.ToDouble(fc["SepQty"].ToString());
                    budgetModel.OctQty = Convert.ToDouble(fc["OctQty"].ToString());
                    budgetModel.NovQty = Convert.ToDouble(fc["NovQty"].ToString());
                    budgetModel.DecQty = Convert.ToDouble(fc["DecQty"].ToString());
                    budgetModel.JanQty = Convert.ToDouble(fc["JanQty"].ToString());
                    budgetModel.FebQty = Convert.ToDouble(fc["FebQty"].ToString());
                    budgetModel.MarQty = Convert.ToDouble(fc["MarQty"].ToString());

                    budgetModel.AprUSD = budgetModel.AprQty * budgetModel.UnitPrice;
                    budgetModel.MayUSD = budgetModel.MayQty * budgetModel.UnitPrice;
                    budgetModel.JunUSD = budgetModel.JunQty * budgetModel.UnitPrice;
                    budgetModel.JulUSD = budgetModel.JulQty * budgetModel.UnitPrice;
                    budgetModel.AugUSD = budgetModel.AugQty * budgetModel.UnitPrice;
                    budgetModel.SepUSD = budgetModel.SepQty * budgetModel.UnitPrice;
                    budgetModel.OctUSD = budgetModel.OctQty * budgetModel.UnitPrice;
                    budgetModel.NovUSD = budgetModel.NovQty * budgetModel.UnitPrice;
                    budgetModel.DecUSD = budgetModel.DecQty * budgetModel.UnitPrice;
                    budgetModel.JanUSD = budgetModel.JanQty * budgetModel.UnitPrice;
                    budgetModel.FebUSD = budgetModel.FebQty * budgetModel.UnitPrice;
                    budgetModel.MarUSD = budgetModel.MarQty * budgetModel.UnitPrice;
                }
                else
                {
                    budgetModel.AprQty = Convert.ToDouble(fc["AprQty"].ToString());
                    budgetModel.MayQty = Convert.ToDouble(fc["MayQty"].ToString());
                    budgetModel.JunQty = Convert.ToDouble(fc["JunQty"].ToString());
                    budgetModel.JulQty = Convert.ToDouble(fc["JulQty"].ToString());
                    budgetModel.AugQty = Convert.ToDouble(fc["AugQty"].ToString());
                    budgetModel.SepQty = Convert.ToDouble(fc["SepQty"].ToString());
                    budgetModel.OctQty = Convert.ToDouble(fc["OctQty"].ToString());
                    budgetModel.NovQty = Convert.ToDouble(fc["NovQty"].ToString());
                    budgetModel.DecQty = Convert.ToDouble(fc["DecQty"].ToString());
                    budgetModel.JanQty = Convert.ToDouble(fc["JanQty"].ToString());
                    budgetModel.FebQty = Convert.ToDouble(fc["FebQty"].ToString());
                    budgetModel.MarQty = Convert.ToDouble(fc["MarQty"].ToString());

                    budgetModel.AprUSD = Convert.ToDouble(fc["AprUSD"].ToString());
                    budgetModel.MayUSD = Convert.ToDouble(fc["MayUSD"].ToString());
                    budgetModel.JunUSD = Convert.ToDouble(fc["JunUSD"].ToString());
                    budgetModel.JulUSD = Convert.ToDouble(fc["JulUSD"].ToString());
                    budgetModel.AugUSD = Convert.ToDouble(fc["AugUSD"].ToString());
                    budgetModel.SepUSD = Convert.ToDouble(fc["SepUSD"].ToString());
                    budgetModel.OctUSD = Convert.ToDouble(fc["OctUSD"].ToString());
                    budgetModel.NovUSD = Convert.ToDouble(fc["NovUSD"].ToString());
                    budgetModel.DecUSD = Convert.ToDouble(fc["DecUSD"].ToString());
                    budgetModel.JanUSD = Convert.ToDouble(fc["JanUSD"].ToString());
                    budgetModel.FebUSD = Convert.ToDouble(fc["FebUSD"].ToString());
                    budgetModel.MarUSD = Convert.ToDouble(fc["MarUSD"].ToString());
                }
                errCode = ObjBl.SaveBudgetDetail(budgetModel, User.Identity.GetUserId());
                if (errCode == 500001)
                {
                    Session["BudgetDetail"] = null;
                    return RedirectToAction("EditBudget", new { BudgetId=budgetModel.BudgetId });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("BudgetList");
        }
        public ActionResult GetEquipmentList(int itemId, int Flag)
        {
            BudgetModel budgetModel = new BudgetModel();
            try
            {
                if (Session["BudgetDetail"] != null)
                {
                    budgetModel = Session["BudgetDetail"] as BudgetModel;
                }
                BudgetBL ObjBl = new BudgetBL();
                budgetModel.EquipmentLst = new List<BudgetEquipmentModel>();
                budgetModel.EquipmentLst = ObjBl.getEquipmentList(itemId);
                budgetModel.ItemId = itemId;
                Session["BudgetDetail"] = budgetModel;
                if (Flag == 0)
                {
                    return RedirectToAction("CreateBudget");
                }
                if (Flag == 1)
                {
                    return RedirectToAction("EditBudget", new { BudgetId = budgetModel.BudgetId });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        public ActionResult SetEmp(string EmpName, string FinYearId)
        {
            string[] str = new string[2];
            str[0] = EmpName;
            str[1] = FinYearId;
            Session["EmpName"] = str;
            return Json("Set", JsonRequestBehavior.AllowGet);
        }
        public ActionResult UploadExcelFile()
        {
            BudgetModel budgetModel = new BudgetModel();
            budgetModel.lstBudgetDetail = new List<BudgetModel>();
            string EmpName = "";
            string FinYear = "";
            bool ChkCell = false;
            bool Allow = true;
            if (Session["EmpName"] != null)
            {
                string[] str = null;
                str = Session["EmpName"] as string[];
                EmpName = str[0].ToString();
                FinYear = str[1].ToString();
            }
            Session["EmpName"] = null;
            if (Session["BudgetDetail"] != null)
            {
                Session["BudgetDetail"] = null;
            }
            if (budgetModel.lstBudgetDetail == null)
                budgetModel.lstBudgetDetail = new List<BudgetModel>();
            if (Request.Files.Count > 0)
            {
                foreach (string files in Request.Files)
                {
                    var file = Request.Files[files];
                    var filename = Path.GetFileName(file.FileName);
                    string[] str = filename.Split('.');
                    string str1 = Guid.NewGuid().ToString() + "." + str[str.Length - 1];
                    file.FileName.Replace(file.FileName, str1);

                    var filepath = Path.Combine(Server.MapPath("~/App_Data/Uploads"), str1);
                    file.SaveAs(filepath);
                    WorkbookPart workbookPart; List<Row> rows;
                    var document = SpreadsheetDocument.Open(filepath, false);
                    workbookPart = document.WorkbookPart;
                    var sheets = workbookPart.Workbook.Descendants<Sheet>();

                    //get the sheet by name of the sheet
                    var sheet = sheets.FirstOrDefault();

                    var workSheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;

                    var columns = workSheet.Descendants<Columns>().FirstOrDefault();
                    // data.ColumnCofiguration = columns;
                    var sheetdata = workSheet.Elements<SheetData>().Last();
                    rows = sheetdata.Elements<Row>().ToList();
                    var regex = new Regex("[A-Za-z]+");
                    for (int i = 7; i < rows.Count + 1; i++)
                    {
                        BudgetModel Model = new BudgetModel();
                        string Message = "";                    
                        // Row r = rows[i];
                        string Alphabet = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,AA,AB,AC,AD,AE,AF,AG,AH,AI,AJ,AK";
                        string[] AlphabetArray = Alphabet.Split(',');
                        ChkCell = false;
                        Allow = true;
                        int B = 0;
                        bool PVTComp = false;
                        foreach (string x in AlphabetArray)
                        {
                            Cell cell = GetCell(workSheet, x, i);
                            string cellval = getCellValue(cell, workbookPart);
                            #region Column

                            switch (x)
                            {
                                case "A":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //     .Elements<SharedStringItem>().ElementAt(
                                            //     Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.Region = cellval;
                                        }
                                        else
                                        {
                                            Model.Region = "";
                                        }
                                        break;
                                    }
                                case "B":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //         .Elements<SharedStringItem>().ElementAt(
                                            //         Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.City = cellval;
                                        }
                                        else
                                        {
                                            Model.City = "";
                                        }
                                        break;
                                    }
                                case "C":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            // .Elements<SharedStringItem>().ElementAt(
                                            // Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            if (cellval != EmpName)
                                            {
                                                ChkCell = true;
                                                Allow = false;
                                            }
                                            Model.EmployeeName = cellval;
                                        }
                                        else
                                        {
                                            Model.EmployeeName = "";
                                            ChkCell = true;
                                            Allow = false;
                                        }
                                        break;
                                    }
                                case "D":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.CustomerName = cellval;
                                        }
                                        else
                                        {
                                            Model.CustomerName = "";
                                        }
                                        break;
                                    }
                                case "E":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.Application = cellval;
                                        }
                                        else
                                        {
                                            Model.Application = "";
                                        }
                                        break;
                                    }
                                case "G":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.ItemName = cellval;
                                            Model.MPN = cellval;
                                        }
                                        else
                                        {
                                            Model.ItemName = "";
                                        }
                                        break;
                                    }
                                case "H":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.EndEquipment = cellval;
                                        }
                                        else
                                        {
                                            Model.EndEquipment = "";
                                        }

                                        break;
                                    }
                                case "I":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.INRUnitPrice = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.INRUnitPrice = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "J":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.USDRate = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.USDRate = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "K":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(Convert.ToDouble(c.CellValue.InnerText))).InnerText;
                                            Model.USDUnitPrice = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.USDUnitPrice = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "M":
                                    {
                                    if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.sunQuantity = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.sunQuantity = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "N":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.AprQty = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.AprQty = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "O":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.AprUSD = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.AprUSD = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }

                                case "P":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.MayQty = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.MayQty = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "Q":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.MayUSD = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.MayUSD = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "R":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.JunQty = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.JunQty = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "S":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.JunUSD = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.JunUSD = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "T":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.JulQty = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.JulQty = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "U":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.JulUSD = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.JulUSD = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }

                                case "V":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.AugQty = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.AugQty = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "W":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.AugUSD = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.AugUSD = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "X":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.SepQty = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.SepQty = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "Y":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.SepUSD = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.SepUSD = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "Z":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.OctQty = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.OctQty = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "AA":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.OctUSD = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.OctUSD = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "AB":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.NovQty = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.NovQty = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "AC":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.NovUSD = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.NovUSD = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "AD":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.DecQty = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.DecQty = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "AE":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.DecUSD = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.DecUSD = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "AF":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.JanQty = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.JanQty = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "AG":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.JanUSD = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.JanUSD = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "AH":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.FebQty = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.FebQty = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "AI":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.FebUSD = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.FebUSD = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "AJ":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.MarQty = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.MarQty = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }
                                case "AK":
                                    {
                                        if (cellval != null && cellval != "")
                                        {

                                            //strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            //.Elements<SharedStringItem>().ElementAt(
                                            //Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                            Model.MarUSD = Convert.ToDouble(cellval);
                                        }
                                        else
                                        {
                                            Model.MarUSD = Convert.ToDouble(0.0);
                                        }
                                        break;
                                    }

                            }
                            #endregion Column 
                        }
                        if (Allow)
                        {
                            Model.Sequence = i;
                            Model.Finyear = FinYear;

                            // Model = BLObj.GetBudgetCompList(Model);
                            budgetModel.lstBudgetDetail.Add(Model);
                        }
                    }
                    BudgetBL BLObj = new BudgetBL();
                    budgetModel.lstBudgetDetail = BLObj.GetBudgetCompList(budgetModel.lstBudgetDetail, EmpName);
                    document.Close();
                    System.IO.File.Delete(filepath);
                }
            }
            Session["BudgetDetail"] = budgetModel;
            var jsonResult = Json(budgetModel.lstBudgetDetail, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult SaveUploadListValidData()
        {
            BudgetModel BudgetModel = new BudgetModel();
            string User_Id = User.Identity.GetUserId();
            try
            {
                if (Session["BudgetDetail"] != null)
                {
                    BudgetModel = Session["BudgetDetail"] as BudgetModel;
                }
                Session["BudgetDetail"] = null;
                if (BudgetModel.lstBudgetDetail.Count > 0)
                {
                    BudgetBL BLObj = new BudgetBL();
                    int errCode = BLObj.SaveUploadValidDataList(BudgetModel, User_Id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction("BudgetList");
        }
     
        private static Cell GetCell(Worksheet worksheet, string columnName, int rowIndex)
        {
            Row row = GetRow(worksheet, rowIndex);
            try
            {
                if (row == null)
                    return null;
                return row.Elements<Cell>().Where(c => string.Compare
                          (c.CellReference.Value, columnName +
                          rowIndex, true) == 0).First();

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private static Row GetRow(Worksheet worksheet, int rowIndex)
        {
            return worksheet.GetFirstChild<SheetData>().
                  Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }
        public static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }
        private string getCellValue(Cell cell, WorkbookPart workbookPart)
        {
            string Value = string.Empty;
            if (cell == null)
                return "";
            if (cell.CellValue != null)
                Value = cell.CellValue.InnerText;
            if (cell.DataType != null)
            {
                if (cell.DataType == CellValues.SharedString)
                {
                    int id = -1;

                    if (Int32.TryParse(cell.InnerText, out id))
                    {
                        SharedStringItem item = GetSharedStringItemById(workbookPart, id);

                        if (item.Text != null)
                        {
                            Value = item.Text.Text;
                        }
                        else if (item.InnerText != null)
                        {
                            Value = item.InnerText;
                        }
                        else if (item.InnerXml != null)
                        {
                            Value = item.InnerXml;
                        }
                    }
                }
            }
            else if (cell.CellValue != null)
            {
                Value = cell.CellValue.InnerText;
            }
            return Value;
        }
    }
}