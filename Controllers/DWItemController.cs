using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.DW;
using SmartSys.BL.SysDBCon;
using Syncfusion.EJ.Export;
using Syncfusion.JavaScript.Models;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SmartSys.Controllers
{
    public class DWItemController : Controller
    {
        public ActionResult GetItemList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWItem/GetItemList");
            if (FindFlag)
            {
                Session["ItemMapping"] = null;
                ItemModel itemModel = new ItemModel();
                itemModel.lstItem = new List<ItemModel>();
                List<ActiveCompanyModel> CompanyModel = new List<ActiveCompanyModel>();
                try
                {
                    if (Session["info"] != null)
                    {
                        string str = Session["info"] as string;
                        TempData["Message"] = str;
                        Session["info"] = null;
                    }
                    ItemBL BLObj = new ItemBL();
                    CustomerBL objbl = new CustomerBL();                  
                    CompanyModel = objbl.GetActiveCompanyList(UserId);
                    ViewBag.lstCompany = CompanyModel;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(itemModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ItemList(string MPN)
        {
            try
            {
                Session["SearchKey"] = MPN;
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetDigiKeyItemData()
        {
            string MPN = Session["SearchKey"] as string;
            Session["SearchKey"] = null;
            ItemModel itemModel = new ItemModel();
            itemModel.lstItem = new List<ItemModel>();
            ItemBL BLObj = new ItemBL();
            itemModel.lstItem = BLObj.GetFilterItemList(MPN);
            IEnumerable data = itemModel.lstItem;
            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult GetItemData()
        {
            ItemModel itemModel = new ItemModel();
            itemModel.lstItem = new List<ItemModel>();
            List<ActiveCompanyModel> CompanyModel = new List<ActiveCompanyModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                ItemBL BLObj = new ItemBL();
                itemModel.lstItem = BLObj.GetItemList(User_Id);
                IEnumerable data = itemModel.lstItem;
                var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
        public void ExportToExcel(string GridModel)
        {
            string user_Id = User.Identity.GetUserId();
            ItemBL BLObj = new ItemBL();
            var DataSource = BLObj.GetItemList(user_Id);
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
        public ActionResult CreateItem(string ItemName, int BrandId, string MPN)
        {
            ItemModel itemModel = new ItemModel();
            string User_Id = User.Identity.GetUserId();
            ItemBL BLObj = new ItemBL();
            List<SmartSys.BL.DW.ItemCategory> lstItemcatg = new List<SmartSys.BL.DW.ItemCategory>();
            lstItemcatg = BLObj.GetitemcategoryList();
            ViewBag.ItemCategory = new SelectList(lstItemcatg, "CategoryId", "Category");
            List<BrandModel> BrandList = new List<BrandModel>();
            BrandList = BLObj.brandList();
            ViewBag.BrandList = new SelectList(BrandList, "BrandId", "BrandName");
            if (ItemName != null)
                if (ItemName.ToString() != "")
                {
                    itemModel.ItemName = ItemName;
                    itemModel.BrandId = BrandId;
                    itemModel.MPN = MPN;
                    itemModel.IsActive = true;
                }
            List<ItemDropDwnModel> ItemLst = new List<ItemDropDwnModel>();
            //ItemBL BLObj = new ItemBL();
            ItemLst = BLObj.GetAllItemDrp(User_Id);
            IEnumerable data = ItemLst;
            var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            ViewBag.ItemList = jsonResult;
            return View(itemModel);
        }
        [HttpPost]
        public ActionResult CreateItem(FormCollection fc, bool IsActive)
        {
            int ErrCode = 0;
            try
            {
                ItemBL BLObj = new ItemBL();
                ItemModel itemModel = new ItemModel();
                itemModel.ItemId = Convert.ToInt32(0);
                itemModel.IsActive = IsActive;
                itemModel.ItemName = fc["ItemName"].ToString();

                itemModel.BrandId = Convert.ToInt32(fc["BrandId"].ToString());
                itemModel.PurchaseRate = Convert.ToDouble(fc["PurchaseRate"].ToString());
                itemModel.SaleRate = Convert.ToDouble(fc["SaleRate"].ToString());
                itemModel.CategoryId = Convert.ToInt32(fc["CategoryId"].ToString());
                itemModel.MPN = fc["MPN"].ToString();
                string User_Id = User.Identity.GetUserId();
                ErrCode = BLObj.SaveItem(itemModel, User_Id);

                if (ErrCode == 500003)
                {
                    Session["info"] = "Item Name OR MPN Already Exists...";
                    return RedirectToAction("GetItemList");
                }
                else
                {
                    Session["info"] = "Record Successfully Saved....";
                    return RedirectToAction("GetItemList");                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        public ActionResult EditItem(int ItemId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWItem/GetItemList");
            if (FindFlag)
            {
                ItemModel itemModel = new ItemModel();
                try
                {
                    ItemBL BLObj = new ItemBL();
                    itemModel = BLObj.GetSelectedItem(ItemId);
                    List<SmartSys.BL.DW.ItemCategory> lstItemcatg = new List<SmartSys.BL.DW.ItemCategory>();
                    lstItemcatg = BLObj.GetitemcategoryList();
                    ViewBag.ItemCategory = new SelectList(lstItemcatg, "CategoryId", "Category");
                    List<BrandModel> BrandList = new List<BrandModel>();
                    BrandList = BLObj.brandList();
                    ViewBag.BrandList = new SelectList(BrandList, "BrandId", "BrandName");

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(itemModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult EditItem(FormCollection fc, bool IsActive)
        {
            int ErrCode = 0;
            ItemModel itemModel = new ItemModel();
            itemModel.ItemId = Convert.ToInt32(fc["ItemId"]);
            try
            {
                ItemBL BLObj = new ItemBL();

                itemModel.ItemId = Convert.ToInt32(fc["ItemId"]);
                itemModel.BrandId = Convert.ToInt32(fc["BrandId"].ToString());
                itemModel.IsActive = IsActive;
                itemModel.ItemName = fc["ItemName"].ToString();
                itemModel.PurchaseRate = Convert.ToDouble(fc["PurchaseRate"].ToString());
                itemModel.SaleRate = Convert.ToDouble(fc["SaleRate"].ToString());
                itemModel.CategoryId = Convert.ToInt32(fc["CategoryId"].ToString());
                itemModel.MPN = fc["MPN"].ToString();
                string User_Id = User.Identity.GetUserId();
                ErrCode = BLObj.SaveItem(itemModel, User_Id);
                if (ErrCode == 500001)
                {
                    return RedirectToAction("GetItemList");
                }
                if (ErrCode == 500002)
                {
                    return RedirectToAction("EditItem", "DWItem", new { ItemId = itemModel.ItemId });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("EditItem", "DWItem", new { ItemId = itemModel.ItemId });

        }
        public ActionResult Checkduplicacy(string ItemName)
        {
            bool Result = true;
            ItemBL objbl = new ItemBL();
            string User_Id = User.Identity.GetUserId();
            Result = objbl.CheckItemDuplicacy(ItemName, User_Id);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        #region DW Item Mapping
        public ActionResult DWItemMapping()
        {
            if (Session["ItemError"] != null)
            {
                TempData["Message"] = Session["ItemError"] as string;
            }
            Session["ItemError"] = null;
            ItemBL BlObj = new ItemBL();
            ItemMappingModel ItemMap = new ItemMappingModel();
            try
            {
                if (Session["ItemMapping"] != null)
                {
                    ItemMap = Session["ItemMapping"] as ItemMappingModel;
                    ItemMap.lstItemMap = new List<ItemMappingModel>();
                    ItemMap.lstItemMap = BlObj.GetDWCompList();
                    foreach (SmartSys.BL.DW.SysNavRelListModel demo in ItemMap.GridList)
                    {
                        var actionToDelete = from DWItemSelect in ItemMap.lstItemMap
                                             where DWItemSelect.CompCode.ToUpper() == demo.CompName.ToUpper()
                                             select DWItemSelect;
                        ItemMap.lstItemMap.Remove(actionToDelete.ElementAt(0));

                    }
                    ViewBag.ItemMapList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");
                    if (ItemMap.lstClientMapItemList != null)
                        ViewBag.ComItemList = new SelectList(ItemMap.lstClientMapItemList, "ClientItemID", "ClientItemName");

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ItemMap);
        }
        public ActionResult GetItemMapping(int ItemId, string ItemName, string MPN, string Brand)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWItem/GetItemList");
            if (FindFlag)
            {
                try
                {
                    ItemMappingModel ItemMap = new ItemMappingModel();

                    ItemBL BlObj = new ItemBL();

                    ItemMap.GridList = BlObj.GetItemNavRelList(ItemId);
                    ItemMap.ItemName = ItemName;
                    ItemMap.ItemId = ItemId;
                    ItemMap.MPN = MPN;
                    ItemMap.Brand = Brand;
                    foreach (SmartSys.BL.DW.SysNavRelListModel DWItem in ItemMap.GridList)
                    {
                        SysDBConModel ConnModel = new SysDBConModel();
                        SysDBConBL ConnobjBL = new SysDBConBL();
                        ConnModel = ConnobjBL.GetSelectedDBConn(DWItem.ConnectionId);
                        ArrayList ConnInfo = new ArrayList();
                        ConnInfo.Add(ConnModel.ConnectionId);
                        ConnInfo.Add(ConnModel.ConnectionType);
                        ConnInfo.Add(ConnModel.ServerName);
                        ConnInfo.Add(ConnModel.DBName);
                        ConnInfo.Add(ConnModel.UserName);
                        ConnInfo.Add(ConnModel.Password);
                        string StrTemp = BlObj.GetClientItemName(DWItem.CompName, DWItem.CompItemID, ConnInfo);
                        DWItem.ItemName = StrTemp;

                    }

                    Session["ItemMapping"] = ItemMap;
                    return RedirectToAction("DWItemMapping", "DWItem");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GetClientItem(string CompCode)
        {

            ItemMappingModel ItemMap = new ItemMappingModel();
            ItemMap = Session["ItemMapping"] as ItemMappingModel;
            ItemBL BlObj = new ItemBL();
            ItemMap.lstItemMap = BlObj.GetDWCompList();
            string SPName = "";
            Int16 ConnId = 0;

            foreach (ItemMappingModel TempModel in ItemMap.lstItemMap)
            {
                if (CompCode == TempModel.CompCode)
                {
                    SPName = TempModel.ItemListSP;
                    ConnId = TempModel.ConnectionId;
                    ItemMap.selectedComp = CompCode;
                    ItemMap.AutoMapBtn = TempModel.SaveItemSP;
                }
            }
            SysDBConModel ConnModel = new SysDBConModel();
            SysDBConBL ConnobjBL = new SysDBConBL();
            ConnModel = ConnobjBL.GetSelectedDBConn(ConnId);
            ArrayList ConnInfo = new ArrayList();
            ConnInfo.Add(ConnModel.ConnectionId);
            ConnInfo.Add(ConnModel.ConnectionType);
            ConnInfo.Add(ConnModel.ServerName);
            ConnInfo.Add(ConnModel.DBName);
            ConnInfo.Add(ConnModel.UserName);
            ConnInfo.Add(ConnModel.Password);
            ItemMap.lstClientMapItemList = BlObj.GetCompanyEmp(CompCode, SPName, ConnInfo);
            Session["ItemMapping"] = ItemMap;
            return RedirectToAction("DWItemMapping", "DWItem");
        }
        public ActionResult GetAutoMapping(string ItemName, string CompCode, int ItemId)
        {
            try
            {
                string User_Id = User.Identity.GetUserId();
                ItemMappingModel ItemMap = new ItemMappingModel();
                ItemBL BlObj = new ItemBL();
                ItemMap.lstItemMap = BlObj.GetDWCompList();
                string SPName = "";
                Int16 ConnId = 0;
                foreach (ItemMappingModel TempModel in ItemMap.lstItemMap)
                {
                    if (CompCode == TempModel.CompCode)
                    {
                        SPName = TempModel.SaveItemSP;
                        ConnId = TempModel.ConnectionId;
                        ItemMap.selectedComp = CompCode;
                    }
                }
                SysDBConModel ConnModel = new SysDBConModel();
                SysDBConBL ConnobjBL = new SysDBConBL();
                ConnModel = ConnobjBL.GetSelectedDBConn(ConnId);
                ArrayList ConnInfo = new ArrayList();
                ConnInfo.Add(ConnModel.ServerName);
                ConnInfo.Add(ConnModel.ConnectionId);
                ConnInfo.Add(ConnModel.DBName);
                ConnInfo.Add(ConnModel.UserName);
                ConnInfo.Add(ConnModel.Password);
                ConnInfo.Add(ConnModel.ConnectionType);

                int errcode = BlObj.GetAutoMapping(User_Id, CompCode, ItemName, ItemId, SPName, ConnInfo);
                if (errcode == 500004)
                {
                    return RedirectToAction("GetItemMapping", "DWItem", new { ItemId = ItemId, ItemName = ItemName });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetItemMapping", "DWItem", new { ItemId = ItemId, ItemName = ItemName });
        }
        public ActionResult SaveMapping(string Item_No, string CompCode, int ItemId, string ItemName)
        {
            if (Item_No == null || Item_No == "" || CompCode == null || CompCode == "" || ItemId == 0)
            {
                ItemName = Session["FilterStr"] as string;
                return RedirectToAction("GetItemList", new { FilterStr = ItemName });
            }
            string Errorcode = "0";
            string User_Id = User.Identity.GetUserId();
            ItemMappingModel ItemMap = new ItemMappingModel();
            ItemBL BlObj = new ItemBL();
            ItemMap.ClientItemID = Item_No;
            ItemMap.CompCode = CompCode;
            ItemMap.ItemId = ItemId;
            Errorcode = BlObj.SaveMapping(ItemMap, User_Id);
            if (Errorcode == "500002")
            {
                return RedirectToAction("GetItemMapping", "DWItem", new { ItemId = ItemId, ItemName = ItemName });
            }
            else if (Errorcode != "0")
            {
                Session["ItemError"] = "This External Item  Already Mapped with " + Errorcode;
                return RedirectToAction("GetItemMapping", "DWItem", new { ItemId = ItemId, ItemName = ItemName });
            }
            else
            {
                return RedirectToAction("GetItemMapping", "DWItem", new { ItemId = ItemId, ItemName = ItemName });
            }

        }
        public ActionResult DeleteItemMapping(string CompName, int ItemId, string ItemName)
        {
            int Errorcod = 0;
            ItemMappingModel ItemMap = new ItemMappingModel();
            ItemBL BlObj = new ItemBL();
            Errorcod = BlObj.DeleteItemMapping(CompName, ItemId);
            if (Errorcod == 500003)
            {
                ItemMap.GridList = BlObj.GetItemNavRelList(ItemId);
                ItemMap.CompName = CompName;
                ItemMap.ItemId = ItemId;
                ItemMap.ItemName = ItemName;
                foreach (SmartSys.BL.DW.SysNavRelListModel DWItem in ItemMap.GridList)
                {
                    SysDBConModel ConnModel = new SysDBConModel();
                    SysDBConBL ConnobjBL = new SysDBConBL();
                    ConnModel = ConnobjBL.GetSelectedDBConn(DWItem.ConnectionId);
                    ArrayList ConnInfo = new ArrayList();
                    ConnInfo.Add(ConnModel.ConnectionId);
                    ConnInfo.Add(ConnModel.ConnectionType);
                    ConnInfo.Add(ConnModel.ServerName);
                    ConnInfo.Add(ConnModel.DBName);
                    ConnInfo.Add(ConnModel.UserName);
                    ConnInfo.Add(ConnModel.Password);
                    string StrTemp = BlObj.GetClientItemName(DWItem.CompName, DWItem.CompItemID, ConnInfo);
                    DWItem.ItemName = StrTemp;

                }
                Session["ItemMapping"] = ItemMap;
                return RedirectToAction("DWItemMapping");
            }
            else
            {
                return RedirectToAction("DWItemMapping");
            }
        }

        public ActionResult BackToList()
        {
            try
            {
                string ItemName = Session["FilterStr"] as string;
                return RedirectToAction("GetItemList", new { FilterStr = ItemName });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion DW Item Mapping

        #region DW Item Approver
        [Authorize]
        public ActionResult ApproverMapping(int RefershFlag)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWItem/ApproverMapping?RefershFlag=0");
            if (FindFlag)
            {
                ItemApproverMap ApproverMap = new ItemApproverMap();
                ApproverMap.SelectApproverLst = new List<SelectPenddingApproveList>();
                ApproverMap.lstApprover = new List<ItemApproverMap>();
                if (RefershFlag == 0)
                {
                    Session["ItemApproverList"] = null;
                }
                try
                {
                    string User_Id = User.Identity.GetUserId();
                    ItemBL BlObj = new ItemBL();
                    if (Session["ItemApproverList"] != null)
                    {
                        ApproverMap = Session["ItemApproverList"] as ItemApproverMap;
                    }
                    else
                    {
                        ApproverMap.lstApprover = BlObj.GetItemApproverList(User_Id, (int)Utility.Enums.StatusCode.REVIEWED);
                    }
                    if (ApproverMap.lstApprover != null && ApproverMap.lstApprover.Count > 0)
                    {
                        Session["ItemApproverList"] = ApproverMap;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(ApproverMap);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportItemApproverMapLst")]
        [AcceptVerbs("POST")]
        public void ExportItemApproverMapLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            ItemBL BlObj = new ItemBL();
            var DataSource = BlObj.GetItemApproverList(User_Id, (int)Utility.Enums.StatusCode.REVIEWED);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult DeleteSelPendingApprover(string CompCode, string Item_Id, string Item_Name, int ItemId, string ItemName, string ModifiedBy)
        {
            try
            {
                ItemApproverMap ApproverMap = new ItemApproverMap();
                if (Session["ItemApproverList"] != null)
                {
                    ApproverMap = Session["ItemApproverList"] as ItemApproverMap;
                }
                var actionToDelete = from actionRepDel in ApproverMap.SelectApproverLst
                                     where actionRepDel.Item_Id == Item_Id && actionRepDel.CompName == CompCode
                                     select actionRepDel;
                ApproverMap.SelectApproverLst.Remove(actionToDelete.ElementAt(0));

                if (ApproverMap.lstApprover == null)
                    ApproverMap.lstApprover = new List<ItemApproverMap>();
                ItemApproverMap Demo = new ItemApproverMap();
                Demo.ItemId = ItemId;
                Demo.Item_Id = Item_Id;
                Demo.CompName = CompCode;
                Demo.ItemName = ItemName;
                Demo.Item_Name = Item_Name;
                Demo.ModifiedBy = ModifiedBy;
                ApproverMap.lstApprover.Add(Demo);

                Session["ItemApproverList"] = ApproverMap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("ApproverMapping", new { RefershFlag = 1 });
        }
        public ActionResult ApvRejItem(int statusCode)
        {
            try
            {
                if (statusCode == 20)
                {
                    Session["ItemApproverList"] = null;
                    return RedirectToAction("ApproverMapping", new { RefershFlag = 1 });
                }
                string User_Id = User.Identity.GetUserId();
                ItemApproverMap ApproverModel = new ItemApproverMap();
                if (Session["ItemApproverList"] != null)
                {
                    ApproverModel = Session["ItemApproverList"] as ItemApproverMap;
                }
                if (ApproverModel.SelectApproverLst == null)
                {
                    return RedirectToAction("ApproverMapping", new { RefershFlag = 1 });
                }
                ItemBL BLObj = new ItemBL();
                int errCode = BLObj.ApvRejItem(ApproverModel, statusCode, User_Id);
                Session["ItemApproverList"] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("ApproverMapping", new { RefershFlag = 1 });
        }

        public ActionResult AddtoList(string Item_Id, string Item_Name, string CompName, int ItemId, string ItemName, string ModifiedBy)
        {
            try
            {

                ItemApproverMap ApproverModel = new ItemApproverMap();
                SelectPenddingApproveList demo = new SelectPenddingApproveList();
                if (Session["ItemApproverList"] != null)
                {
                    ApproverModel = Session["ItemApproverList"] as ItemApproverMap;
                }
                var actionToDelete = from actionRepDel in ApproverModel.lstApprover
                                     where actionRepDel.Item_Id == Item_Id && actionRepDel.CompName == CompName
                                     select actionRepDel;
                ApproverModel.lstApprover.Remove(actionToDelete.ElementAt(0));

                if (ApproverModel.SelectApproverLst == null)
                    ApproverModel.SelectApproverLst = new List<SelectPenddingApproveList>();
                demo.ItemId = ItemId;
                demo.Item_Id = Item_Id;
                demo.CompName = CompName;
                demo.ItemName = ItemName;
                demo.Item_Name = Item_Name;
                demo.ModifiedBy = ModifiedBy;
                ApproverModel.SelectApproverLst.Add(demo);
                Session["ItemApproverList"] = ApproverModel;
            }
            catch (Exception ex)
            {

                throw;
            }
            return RedirectToAction("ApproverMapping", new { RefershFlag = 1 });
        }
        public ActionResult RefreshApprovealList()
        {
            try
            {
                return RedirectToAction("ApproverMapping", new { RefershFlag = 0 });
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion DW Item Approver

        #region DW Item Pendding Mapping  Review
        [Authorize]
        public ActionResult ApproverMappingReview(int RefershFlag)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWItem/ApproverMappingReview?RefershFlag=0");
            if (FindFlag)
            {
                //if (Session["Item"] != null)
                //{
                //    TempData["Message"] = Session["Item"] as string;
                //}
                //Session["Item"] = null;
                ItemApproverMap ReviewMapModel = new ItemApproverMap();
                ReviewMapModel.SelectApproverLst = new List<SelectPenddingApproveList>();
                ReviewMapModel.lstApprover = new List<ItemApproverMap>();
                if (RefershFlag == 0)
                {
                    Session["ItemApproverReviewList"] = null;
                }
                try
                {
                    string User_Id = User.Identity.GetUserId();
                    ItemBL BlObj = new ItemBL();
                    if (Session["ItemApproverReviewList"] != null)
                    {
                        ReviewMapModel = Session["ItemApproverReviewList"] as ItemApproverMap;
                    }
                    else
                    {
                        ReviewMapModel.lstApprover = BlObj.GetItemApproverList(User_Id, Convert.ToInt32(Utility.Enums.StatusCode.NEW));
                    }
                    if (ReviewMapModel.lstApprover != null && ReviewMapModel.lstApprover.Count > 0)
                    {
                        Session["ItemApproverReviewList"] = ReviewMapModel;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(ReviewMapModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportItemReviewLst")]
        [AcceptVerbs("POST")]
        public void ExportItemReviewLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            ItemBL BlObj = new ItemBL();
            var DataSource = BlObj.GetItemApproverList(User_Id, Convert.ToInt32(Utility.Enums.StatusCode.NEW));
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult DeleteSelPendingReviwed(string CompCode, string Item_Id, string Item_Name, int ItemId, string ItemName, string ModifiedBy)
        {
            try
            {
                ItemApproverMap ReviewMapModel = new ItemApproverMap();
                if (Session["ItemApproverReviewList"] != null)
                {
                    ReviewMapModel = Session["ItemApproverReviewList"] as ItemApproverMap;
                }
                var actionToDelete = from actionRepDel in ReviewMapModel.SelectApproverLst
                                     where actionRepDel.Item_Id == Item_Id && actionRepDel.CompName == CompCode
                                     select actionRepDel;
                ReviewMapModel.SelectApproverLst.Remove(actionToDelete.ElementAt(0));

                if (ReviewMapModel.lstApprover == null)
                    ReviewMapModel.lstApprover = new List<ItemApproverMap>();
                ItemApproverMap Demo = new ItemApproverMap();
                Demo.ItemId = ItemId;
                Demo.Item_Id = Item_Id;
                Demo.CompName = CompCode;
                Demo.ItemName = ItemName;
                Demo.Item_Name = Item_Name;
                Demo.ModifiedBy = ModifiedBy;
                ReviewMapModel.lstApprover.Add(Demo);

                Session["ItemApproverReviewList"] = ReviewMapModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("ApproverMappingReview", new { RefershFlag = 1 });
        }

        public ActionResult AddtoReviewList(string Item_Id, string Item_Name, string CompName, int ItemId, string ItemName, string ModifiedBy)
        {
            try
            {

                ItemApproverMap ReviewMapModel = new ItemApproverMap();
                SelectPenddingApproveList demo = new SelectPenddingApproveList();
                if (Session["ItemApproverReviewList"] != null)
                {
                    ReviewMapModel = Session["ItemApproverReviewList"] as ItemApproverMap;
                }
                var actionToDelete = from actionRepDel in ReviewMapModel.lstApprover
                                     where actionRepDel.Item_Id == Item_Id && actionRepDel.CompName == CompName
                                     select actionRepDel;
                ReviewMapModel.lstApprover.Remove(actionToDelete.ElementAt(0));

                if (ReviewMapModel.SelectApproverLst == null)
                    ReviewMapModel.SelectApproverLst = new List<SelectPenddingApproveList>();
                demo.ItemId = ItemId;
                demo.Item_Id = Item_Id;
                demo.CompName = CompName;
                demo.ItemName = ItemName;
                demo.Item_Name = Item_Name;
                demo.ModifiedBy = ModifiedBy;
                ReviewMapModel.SelectApproverLst.Add(demo);
                Session["ItemApproverReviewList"] = ReviewMapModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("ApproverMappingReview", new { RefershFlag = 1 });
        }
        public ActionResult ApvRejItemReview(int statusCode)
        {
            try
            {
                if (statusCode == 20)
                {
                    Session["ItemApproverReviewList"] = null;
                    return RedirectToAction("ApproverMappingReview", new { RefershFlag = 1 });
                }
                string User_Id = User.Identity.GetUserId();
                ItemApproverMap ReviewModel = new ItemApproverMap();
                if (Session["ItemApproverReviewList"] != null)
                {
                    ReviewModel = Session["ItemApproverReviewList"] as ItemApproverMap;
                }
                ItemBL BLObj = new ItemBL();
                int errCode = BLObj.ApvRejItemReview(ReviewModel, statusCode, User_Id);
                Session["ItemApproverReviewList"] = null;
            }
            catch (Exception ex)
            {
                throw ex;
                //Session["Item"] = "Please Select Atleast one ITem ";
            }
            return RedirectToAction("ApproverMappingReview", new { RefershFlag = 1 });
        }
        public ActionResult RefreshReviewList()
        {
            try
            {
                return RedirectToAction("ApproverMappingReview", new { RefershFlag = 0 });
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion DW Item Pendding Mapping  Review

        #region ItemBrandAllocation
        public ActionResult GetItemBrandList(int ItemId, string ItemName)
        {

            ViewBag.ItemName = ItemName;
            ItemBrandAllocationDetail model = new ItemBrandAllocationDetail();
            model.TotalItemBrand = new List<ItemBrandAllocationDetail.DerivedItemBrand>();
            model.AssignedItemBrand = new List<ItemBrandAllocationDetail.DerivedItemBrand>();
            ItemBL objbl = new ItemBL();
            model = objbl.GetItemBrandAllocation(ItemId);
            model.ItemId = ItemId;
            List<ItemBrandAllocationDetail.DerivedItemBrand> TotalList = model.TotalItemBrand;
            ViewBag.TotalItemBrand = new SelectList(TotalList, "BrandId", "BrandName");
            List<ItemBrandAllocationDetail.DerivedItemBrand> AssignedList = model.AssignedItemBrand;
            ViewBag.AssignedList = new SelectList(AssignedList, "BrandId", "BrandName");
            return View();
        }
        [HttpPost]
        public ActionResult SaveItemBrand(FormCollection frmCollection)
        {

            ItemBrandAllocationDetail Itembrandmodel = null;

            int ItemId = Convert.ToInt32(frmCollection["ItemId"]);
            string User_Id = User.Identity.GetUserId();
            if (Session["SaveItemBrand"] != null)
            {
                Itembrandmodel = Session["SaveItemBrand"] as ItemBrandAllocationDetail;
            }
            else
            {
                Itembrandmodel = new ItemBrandAllocationDetail();
            }
            foreach (string Key in frmCollection.Keys)
            {
                if (Key.ToString() == "AssignedItemBrand")
                {
                    List<string> LstGetAssignedItemBrand = frmCollection["AssignedItemBrand"].Split(',').ToList();
                    List<ItemBrandAllocationDetail.ItemBrandDetail> list = new List<ItemBrandAllocationDetail.ItemBrandDetail>();
                    Itembrandmodel.LstBrandItemDetail = list;
                    foreach (var Item in LstGetAssignedItemBrand)
                    {
                        ItemBrandAllocationDetail.ItemBrandDetail ItemBrand = new ItemBrandAllocationDetail.ItemBrandDetail();
                        ItemBrand.BrandId = Convert.ToInt32(Item);
                        ItemBrand.ItemId = ItemId;
                        list.Add(ItemBrand);
                    }
                }
            }
            ItemBL ItemBL = new ItemBL();
            int iErrorCode = ItemBL.SaveItemBrand(Itembrandmodel, ItemId, User_Id);
            Session["SaveItemBrand"] = null;
            return RedirectToAction("GetItemList", "DWItem");
        }
        #endregion ItemBrandAllocation
    }
}