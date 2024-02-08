using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.Adhoc;
using SmartSys.BL.BOMAdmin;
using SmartSys.BL.DW;
using SmartSys.BL.Enquiry;
using SmartSys.BL.Project;
using SmartSys.BL.ProViews;
using SmartSys.BL.TimeManagement;
using SmartSys.Utility;
using Syncfusion.EJ.Export;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SmartSys.Controllers
{
    public class EnquiryController : Controller
    {

        #region Create And Edit Emquiry
        public ActionResult CustomerEnquiry()
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
            if (Session["info"] != null)
            {
                string str = Session["info"] as string;
                TempData["Message"] = str;
                Session["info"] = null;
            }
            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/CustomerEnquiry");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                EnquiryModel EnquModel = new EnquiryModel();
                UserId = User.Identity.GetUserId();
                EnquiryBL BLObj = new EnquiryBL();
                EnquModel = BLObj.getEnquiryList(User_Id);
                try
                {
                    if (Session["EnquiryDetail"] != null)
                    {
                        Session["EnquiryDetail"] = null;
                    }
                   
                    //  EnquModel = BLObj.getEnquiryList(User_Id);                    
                    string user_Id = User.Identity.GetUserId();

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
                    Session["ParamDescrip"] = prmmodParamDemo;

                    int CurrentYear = DateTime.Today.Year;
                    int PreviousYear = DateTime.Today.Year - 1;
                    int NextYear = DateTime.Today.Year + 1;
                    string PreYear = PreviousYear.ToString();
                    string NexYear = NextYear.ToString();
                    string CurYear = CurrentYear.ToString();
                    string TxtParamValue = "";
                    string FinYear = null;
                    if (DateTime.Today.Month > 3)
                        FinYear = CurYear + "-" + NexYear.ToString().Substring(2, 2).ToString();
                    else
                        FinYear = PreYear + "-" + CurYear.ToString().Substring(2, 2).ToString();
                    TxtParamValue = FinYear.Trim();
                    EnquModel.ParamId = "@FIYear";
                    EnquModel.ParamName = "@FIYear";
                    EnquModel.hidText = "";
                    EnquModel.ReportId = "MGMT007";
                    EnquModel.TxtParamValue = TxtParamValue;
                    ReportNameModel Model = new ReportNameModel();
                    Model = BLObj.GetOpenReport(User_Id);
                    EnquModel.OutputLocation = Model.OutputLocation;
                    EnquModel.RunDate = Model.RunDate;
                    EnquModel.StatusId = Model.StatusId;
                    EnquModel.ReportId = Model.ReportId;
                   

                }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                }

                return View(EnquModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult GetEnqData()
        {
            EnquiryModel EnquModel = new EnquiryModel();
            string User_Id = User.Identity.GetUserId();
            try
            {
                EnquiryBL BLObj = new EnquiryBL();
                EnquModel = BLObj.getEnquiryList(User_Id);
                IEnumerable data = EnquModel.lstEnquiry;
                var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult EnquiryListForAssignment()
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
            if (Session["info"] != null)
            {
                string str = Session["info"] as string;
                TempData["Message"] = str;
                Session["info"] = null;
            }
            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/CustomerEnquiry");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                EnquiryModel EnquModel = new EnquiryModel();
                UserId = User.Identity.GetUserId();
                try
                {
                    if (Session["EnquiryDetail"] != null)
                    {
                        Session["EnquiryDetail"] = null;
                    }
                    EnquiryBL BLObj = new EnquiryBL();
                    EnquModel = BLObj.getEnquiryListforAssignment(User_Id, "Process");

                }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                }

                return View(EnquModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportEnquiryListForAssignment")]
        [AcceptVerbs("POST")]
        public void ExportEnquiryListForAssignment(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            EnquiryModel EnquModel = new EnquiryModel();
            EnquModel = BLObj.getEnquiryListforAssignment(User_Id, "Process");
            var DataSource = EnquModel.lstEnquiry;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult EnquiryListForQuotatationAssignment()
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
            if (Session["info"] != null)
            {
                string str = Session["info"] as string;
                TempData["Message"] = str;
                Session["info"] = null;
            }
            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/CustomerEnquiry");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                EnquiryModel EnquModel = new EnquiryModel();
                UserId = User.Identity.GetUserId();
                try
                {
                    if (Session["EnquiryDetail"] != null)
                    {
                        Session["EnquiryDetail"] = null;
                    }
                    EnquiryBL BLObj = new EnquiryBL();
                    EnquModel = BLObj.getEnquiryListforAssignment(User_Id, "Quotation");

                }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                }

                return View(EnquModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportEnquiryListForQuotatationAssignment")]
        [AcceptVerbs("POST")]
        public void ExportEnquiryListForQuotatationAssignment(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            EnquiryModel EnquModel = new EnquiryModel();
            EnquModel = BLObj.getEnquiryListforAssignment(User_Id, "Quotation");
            var DataSource = EnquModel.lstEnquiry;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult SaveAssignedEmployee(string EmpId, int EnqId)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            int Status = Convert.ToInt32(SmartSys.Utility.Enums.StatusCode.SndToPro);//Send For  processing
            int Errorcode = Enqbl.saveAssignedEnqToEmployee(EnqId, EmpId, User_Id, Status, "Process");
            return RedirectToAction("EnquiryListForAssignment");
        }
        public ActionResult SaveAssignedEmployeetoQuat(string EmpId, int EnqId)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            int Status = Convert.ToInt32(SmartSys.Utility.Enums.StatusCode.QuatPrep);//Send For  processing
            int Errorcode = Enqbl.saveAssignedEnqToEmployee(EnqId, EmpId, User_Id, Status, "Quat");
            return RedirectToAction("EnquiryListForQuotatationAssignment");
        }
        public ActionResult AssignEnquiryToCust(int EnqId, string Type)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/CretaeCustomerEnquiry?EnqId=0");
            if (FindFlag)
            {
                EnquiryModel EnquModel = new EnquiryModel();
                EnquiryBL Enqbl = new EnquiryBL();
                string User_Id = User.Identity.GetUserId();
                FindFlag = true;
                try
                {
                    ItemModel itemModel = new ItemModel();
                    BudgetModel budgetModel = new BudgetModel();
                    BudgetBL ObjBl = new BudgetBL();
                    ItemBL BLObj = new ItemBL();
                    EnquModel.lstEnquiryDetail = new List<EnquiryDetailModel>();
                    itemModel.lstItem = new List<ItemModel>();
                    itemModel.lstItem = BLObj.GetItemList(User_Id);
                    if (EnqId > 0)
                    {
                        EnquModel = Enqbl.getSelectedEnquiryDetail(EnqId);
                    }
                    ViewBag.itemlist = new SelectList(itemModel.lstItem, "ItemId", "ItemName");

                    ProjectBL Blobj = new ProjectBL();
                    ViewBag.CustomerLst = new SelectList(Blobj.GetCustomerListByUser(User_Id), "Id", "DisplayName");

                    ItemBrandAllocationDetail model = new ItemBrandAllocationDetail();
                    ItemBL objbl1 = new ItemBL();
                    model = objbl1.GetItemBrandAllocation(1);
                    model.AssignedItemBrand = new List<ItemBrandAllocationDetail.DerivedItemBrand>();
                    ViewBag.BrandListLst = model.TotalItemBrand;

                    ItemBL BlObj = new ItemBL();
                    ItemMappingModel ItemMap = new ItemMappingModel();
                    ItemMap.lstItemMap = BlObj.GetDWCompList();
                    ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");

                    List<EmployeeModel> lstEmp = new List<EmployeeModel>();
                    List<EmployeeModel> lstEmpAc = new List<EmployeeModel>();
                    AdminBL objBL = new AdminBL();
                    lstEmp = objBL.GetEmployeeList(UserId);
                    foreach (EmployeeModel temp in lstEmp)
                    {
                        if (!temp.Deleted)
                        {
                            lstEmpAc.Add(temp);
                        }
                    }
                    ViewBag.EmpList = new SelectList(lstEmpAc, "EmpId", "EmpName");
                    EnquModel.Types = Type;
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
        public ActionResult ViewItemResponse(int EnqId, int ItemId)
        {
            EnquiryBL BLObj = new EnquiryBL();
            EnquiryRespModel ResponseModel = new EnquiryRespModel();
            ResponseModel.lstEnquiryResponse = new List<EnquiryRespModel>();
            ArrayList list = new ArrayList();
            try
            {
                ResponseModel = BLObj.GetItemResponseList(EnqId, ItemId);
            }
            catch (Exception)
            {

                throw;
            }
            return PartialView(ResponseModel);
        }
        public ActionResult DeleteEnqItem(int EnqId, int ItemId)
        {
            EnquiryBL BLObj = new EnquiryBL();
            try
            {
                int ErrCode = BLObj.DeleteEnqItem(EnqId, ItemId);
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("CretaeCustomerEnquiry", new { EnqId = EnqId });
        }
        public ActionResult UploadExcelFile()
        {
            EnquiryModel EnquModel = new EnquiryModel();
            string User_Id = User.Identity.GetUserId();
            EnquModel.lstEnquiryDetail = new List<EnquiryDetailModel>();
            if (Session["EnquiryDetail"] != null)
            {
                EnquModel = Session["EnquiryDetail"] as EnquiryModel;
            }
            if (EnquModel.lstEnquiryDetail == null)
                EnquModel.lstEnquiryDetail = new List<EnquiryDetailModel>();
            try
            {

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
                        //var data = new XlsData();
                        WorkbookPart workbookPart; List<Row> rows;

                        //FileStream filStream = new FileStream(filePath, FileMode.Open);

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

                        for (int i = 1; i < rows.Count; i++)
                        {
                            EnquiryDetailModel Model = new EnquiryDetailModel();
                            Model.Status = "New";
                            Model.ModifiedBy = User.Identity.Name;
                            Model.ModifiedDate = DateTime.Now.ToString();
                            Row r = rows[i];
                            foreach (DocumentFormat.OpenXml.Spreadsheet.Cell c in r.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>())
                            {
                                var match = regex.Match(c.CellReference);
                                string strColumnName = match.Value;
                                string strText = "";
                                if (strColumnName == "A")
                                {
                                    strText = c.CellValue.InnerXml;
                                    strText = workbookPart.SharedStringTablePart.SharedStringTable
                                         .Elements<SharedStringItem>().ElementAt(
                                         Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                    Model.MPN = strText;
                                }
                                else if (strColumnName ==
                                    "B")
                                {
                                    strText = c.CellValue.InnerXml;
                                    strText = workbookPart.SharedStringTablePart.SharedStringTable
                                             .Elements<SharedStringItem>().ElementAt(
                                             Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                    Model.Remark = strText;
                                }
                                else if (strColumnName == "C")
                                {
                                    strText = c.CellValue.InnerXml;
                                    Model.Quantity = Convert.ToInt32(strText);
                                }
                                else if (strColumnName == "D")
                                {
                                    strText = c.CellValue.InnerXml;
                                    DateTime d = DateTime.FromOADate(Convert.ToDouble(strText));
                                    Model.ExpectedDateStr = Convert.ToString(d);
                                    Model.ExpectedDate = Convert.ToDateTime(Model.ExpectedDateStr);
                                }
                            }
                            EnquModel.lstEnquiryDetail.Add(Model);
                        }
                        document.Close();
                        System.IO.File.Delete(filepath);

                    }
                }
                //List<EnquiryDetailModel> lst = new List<EnquiryDetailModel>();
                //ItemBL objbl = new ItemBL();
                //ItemModel itemModel = new ItemModel();
                //itemModel.lstItem = objbl.GetItemList();
                //foreach(var item in  EnquModel.lstEnquiryDetail)
                //{
                //    string itemname = item.MPN;

                //    foreach (var itms in itemModel.lstItem.Where(s => s.MPN == itemname))
                //    {
                //        string Itmnm = itms.MPN;
                //        if (Itmnm == itemname)
                //        {
                //            EnquiryDetailModel model = new EnquiryDetailModel();
                //            model.ItemName = itemname;
                //            model.ItemId = itms.ItemId;
                //            model.Remark = item.Remark;
                //            model.ExpectedDateStr = item.ExpectedDateStr;
                //            model.Quantity = item.Quantity;
                //            model.Status = item.Status;
                //            model.ModifiedBy = item.ModifiedBy;
                //            model.ModifiedDate = item.ModifiedDate;
                //            lst.Add(model);
                //            break;
                //        }
                //    }
                //}           
                Session["ItemList"] = null;
                EnquiryBL BLObj = new EnquiryBL();
                EnquModel.lstEnquiryDetail = BLObj.GetEnqCompList(EnquModel, User_Id);
                Session["EnquiryDetail"] = EnquModel;
                return Json(EnquModel.lstEnquiryDetail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CretaeCustomerEnquiry(int EnqId)
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
            if (Session["info"] != null)
            {
                string str = Session["info"] as string;
                TempData["Message"] = str;
                Session["info"] = null;
            }
            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/CretaeCustomerEnquiry?EnqId=0");
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
                    List<ItemListModel> lstItem = new List<ItemListModel>();
                    lstItem = Enqbl.GetItemLst();
                    //itemModel.lstItem = BLObj.GetItemListwithMPN();
                    ViewBag.itemlist = lstItem;
                    if (EnqId > 0)
                    {
                        EnquModel = Enqbl.getSelectedEnquiryDetail(EnqId);
                        EnquModel.isOutSourcePerson = Enqbl.CheckIsOutsource(User_Id);
                        EnquModel.IsChange = "";
                    }
                    //ViewBag.itemlist = new SelectList(itemModel.lstItem, "ItemId", "ItemName");

                    ProjectBL Blobj = new ProjectBL();
                    ViewBag.CustomerLst = new SelectList(Blobj.GetCustomerListByUser(User_Id), "Id", "DisplayName");
                    ViewBag.VendorList = new SelectList(Blobj.GetVendorListByUser(User_Id), "Id", "DisplayName");
                    EnquiryModel EnquModelDrp = new EnquiryModel();
                    EnquModelDrp = Enqbl.getEnquiryList(User_Id);
                    ViewBag.EnqList = new SelectList(EnquModelDrp.lstEnquiry, "EnqId", "EnqId");

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
                    if (EnquModel.ResponsPerson == null)
                        EnquModel.ResponsPerson = "";
                }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                }
                Session["EnquiryDetail"] = EnquModel;
                return View(EnquModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportEnqDetailList")]
        [AcceptVerbs("POST")]
        public void ExportEnqDetailList(string GridModel, int EnqId)
        {
            string User_Id = User.Identity.GetUserId();
            EnquiryModel EnquModel = new EnquiryModel();
            EnquModel.lstEnquiryDetail = new List<EnquiryDetailModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            EnquModel = Enqbl.getSelectedEnquiryDetail(EnqId);
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = EnquModel.lstEnquiryDetail;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult GetUpdateItems()
        {
            List<ItemListModel> lstItem = new List<ItemListModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            lstItem = Enqbl.GetItemLst();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SendEnqForQuatation(int EnqId)
        {
            int errCode = 0;
            string user_Id = User.Identity.GetUserId();
            try
            {
                EnquiryBL BLObj = new EnquiryBL();
                errCode = BLObj.SendEnqForQuatation(EnqId, Convert.ToInt32(SmartSys.Utility.Enums.StatusCode.AsignQuat), user_Id);
                if (errCode != 600002)
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public JsonResult EnqSendForProcess(int EnqId)
        {
            int ErrCode = 0;
            EnquiryBL BLObj = new EnquiryBL();
            EnquiryModel EnquModel = new EnquiryModel();
            string user_Id = User.Identity.GetUserId();
            try
            {
                if (Session["EnquiryDetail"] != null)
                {
                    EnquModel = Session["EnquiryDetail"] as EnquiryModel;

                }
                if (EnquModel.lstEnquiryDetail.Count != null)
                {
                    if (EnquModel.lstEnquiryDetail.Count > 0)
                    {
                        ErrCode = BLObj.EnqSendForProcess(EnqId, Convert.ToInt32(SmartSys.Utility.Enums.StatusCode.RDY4PRO), user_Id);
                        if (ErrCode == 600001 || ErrCode == 600002)
                        {
                            return Json("Success", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddItemEnquiry(int EnqId, int ItemId)
        {
            List<BrandItemModel> model = new List<BrandItemModel>();
            EnquiryBL BLObj = new EnquiryBL();
            try
            {
                model = BLObj.GetSeletedCustEnquiryItemBrandList(EnqId, ItemId);
                //    ItemBL BLObj = new ItemBL();              
                //    itemModel.lstItem = new List<ItemModel>();
                //    itemModel.lstItem = BLObj.GetItemList();               
                //    ViewBag.itemlist = new SelectList(itemModel.lstItem, "ItemId", "ItemName");
                //    string DocumentNo = (String)Session["DocumentNo"];
                //    ArrayList list = new ArrayList();
                //    list = Session["EnqInfo"] as ArrayList;
                //    model.EnqId = list.IndexOf(0);
                //    model.ItemId = list.IndexOf(1);
                //    ItemBrandAllocationDetail model1 = new ItemBrandAllocationDetail();
                //    model1.AssignedItemBrand = new List<ItemBrandAllocationDetail.DerivedItemBrand>();
                //    ViewBag.BrandListLst = model1.AssignedItemBrand;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "BrandId", "BrandName"));

        }
        public ActionResult GetItemBrandList(int ItemId)
        {
            ItemBrandAllocationDetail model = new ItemBrandAllocationDetail();
            try
            {
                model.AssignedItemBrand = new List<ItemBrandAllocationDetail.DerivedItemBrand>();
                ItemBL objbl = new ItemBL();
                model = objbl.GetItemBrandAllocation(ItemId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model.AssignedItemBrand, "BrandId", "BrandName"));
        }
        public ActionResult GetItemCPN(int ItemId, int CustomerId)
        {
            CPNModel CPNModel = new CPNModel();
            try
            {
                CustomerBL objbl = new CustomerBL();
                CPNModel = objbl.GetSelectedCPNList(CustomerId, ItemId);
                if (CPNModel.CPN == null)
                    CPNModel.CPN = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(CPNModel.CPN, JsonRequestBehavior.AllowGet);
        }
        public FileResult Downloadsample()
        {
            var filepath = Path.Combine(Server.MapPath("~/App_Data/MyFiles/SampleExcel.xlsx"));
            string fileName = "SampleExcel.xlsx";
            return File(filepath, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public JsonResult AddEnqDataFromExcel(int EnqId)
        {
            try
            {
                EnquiryBL BLObj = new EnquiryBL();
                EnquiryModel EnquModel = new EnquiryModel();
                EnquModel = Session["EnquiryDetail"] as EnquiryModel;
                string user_Id = User.Identity.GetUserId();
                int errCode = BLObj.SaveEnqItemsFromExcel(EnquModel.lstEnquiryDetail, user_Id, EnqId);
                return Json(0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult AddEnqData(int itemId, string remark, int Quantity, DateTime ExpectedDate, string ItemName, int EnqID, string CPN)
        {
            try
            {
                EnquiryModel EnquModel = new EnquiryModel();
                EnquiryBL BLObj = new EnquiryBL();
                string user_Id = User.Identity.GetUserId();
                if (Session["EnquiryDetail"] != null)
                {
                    EnquModel = Session["EnquiryDetail"] as EnquiryModel;

                }

                if (EnquModel.lstEnquiryDetail == null)
                    EnquModel.lstEnquiryDetail = new List<EnquiryDetailModel>();
                if (EnquModel.lstEnquiryDetail.Count > 0)
                {
                    var actionToDelete = from actionRepDel in EnquModel.lstEnquiryDetail
                                         where actionRepDel.ItemId == itemId
                                         select actionRepDel;
                    if (actionToDelete.Count() > 0)
                        EnquModel.lstEnquiryDetail.Remove(actionToDelete.ElementAt(0));
                }
                EnquiryDetailModel EnqModelDetail = new EnquiryDetailModel();
                EnqModelDetail.EnqId = EnqID;
                EnqModelDetail.ItemId = itemId;
                EnqModelDetail.ItemName = ItemName;
                EnqModelDetail.Remark = remark;
                EnqModelDetail.Quantity = Quantity;
                EnqModelDetail.ExpectedDate = ExpectedDate;
                EnqModelDetail.CPN = CPN;
                EnqModelDetail.ExpectedDateStr = ExpectedDate.ToShortDateString();
                EnqModelDetail.Brand = "";
                EnquModel.lstEnquiryDetail.Add(EnqModelDetail);
                int errCode = BLObj.SaveEnquiryItemBrand(EnqModelDetail, user_Id);
                Session["EnquiryDetail"] = EnquModel;
                var jsonResult = Json(EnquModel.lstEnquiryDetail, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult CretaeCustomerEnquiry(FormCollection fc)
        {
            int errCode = 0;
            EnquiryModel EnquModel = new EnquiryModel();
            EnquiryBL BLObj = new EnquiryBL();
            try
            {
                string user_Id = User.Identity.GetUserId();
                EnquModel.EnqId = Convert.ToInt32(fc["EnqId"]);
                EnquModel.CustomerId = Convert.ToInt32(fc["CustomerId"]);
                EnquModel.CustomerContactId = Convert.ToInt32(fc["CustomerContactId"]);
                EnquModel.ExpectedDate = Convert.ToDateTime(fc["ExpectedDate"]);
                EnquModel.EnqNumber = fc["EnqNumber"].ToString();
                EnquModel.Remark = fc["Remark"].ToString();
                EnquModel.CompCode = fc["CompCode"].ToString();
                EnquModel.Priority = fc["Priority"].ToString();
                EnquModel.categories = fc["categories"].ToString();
                EnquModel.Rating = Convert.ToInt32(fc["Rating"].ToString());
                EnquModel.Currancy = fc["Currancy"].ToString();
                if (EnquModel.categories == "Lead")
                {
                    EnquModel.categoriesRefId = Convert.ToInt32(fc["VendRefId"]);
                }
                else if (EnquModel.categories == "RepeatEnq")
                {
                    EnquModel.categoriesRefId = Convert.ToInt32(fc["EnqRefId"]);
                }
                else
                {
                    EnquModel.categoriesRefId = 0;
                }
                errCode = BLObj.SaveCustomerEnquiry(EnquModel, user_Id);
                if (errCode != 0)
                {
                    return RedirectToAction("CretaeCustomerEnquiry", new { EnqId = errCode });
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("CustomerEnquiry", new { EnqId = EnquModel.EnqId });
        }
        #endregion Create And Edit Emquiry

        #region Process Enquiry
        public ActionResult EnquiryProcessList()
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
            if (Session["info"] != null)
            {
                string str = Session["info"] as string;
                TempData["Message"] = str;
                Session["info"] = null;
            }
            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/EnquiryProcessList");
            if (FindFlag)
            {
                string user_Id = User.Identity.GetUserId();
                EnquiryModel EnquModel = new EnquiryModel();
                UserId = User.Identity.GetUserId();
                try
                {
                    EnquiryBL BLObj = new EnquiryBL();
                    EnquModel = BLObj.getEnquiryProcessingList(user_Id);

                }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                }

                return View(EnquModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportEnquiryProcessList")]
        [AcceptVerbs("POST")]
        public void ExportEnquiryProcessList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            EnquiryModel EnquModel = new EnquiryModel();
            EnquModel = BLObj.getEnquiryProcessingList(User_Id);
            var DataSource = EnquModel.lstEnquiry;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult ProcessCustomerEnquiry(int EnqId,Boolean IsPerson)
        {
            string ErrDesp = "";
            if (Session["VendSendEnq"] != null)
            {
                ErrDesp = Session["VendSendEnq"] as string;
            }
            Session["VendSendEnq"] = null;
            EnquiryModel EnquModel = new EnquiryModel();
            EnquModel.lstEnquiryDetail = new List<EnquiryDetailModel>();
            List<EnqItemVendorContact> DetailList = new List<EnqItemVendorContact>();
            EnquiryBL Enqbl = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                EnquModel = Enqbl.GetSeletedEnquiryProcessList(EnqId, User_Id);
                ViewBag.LstEnqItem = new SelectList(EnquModel.lstEnquiryDetail, "ItemId", "MPN");
                EnquModel.isOutSourcePerson = IsPerson;
                DetailList = Enqbl.GetItemVendorDetailList(EnqId, User_Id);
                ViewBag.EnqItemVendorContList=DetailList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(EnquModel);
        }
        public ActionResult GetItemVendorList(int EnqId, int ItemId)
        {
            List<EnquiryBrandVemdorModel> model = new List<EnquiryBrandVemdorModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                model = Enqbl.GetEnquiryProcessBrandVendorList(EnqId, ItemId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "VendorId", "VendorName"));
        }
        public ActionResult GetItemVendorAllocateList(int EnqId, int ItemId)
        {
            List<EnquiryBrandVemdorModel> model = new List<EnquiryBrandVemdorModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                model = Enqbl.GetEnquiryProcessAllocateItemVendorList(EnqId, ItemId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "VendorId", "VendorName"));
        }
        //public ActionResult SendVenderEnquiryDetail(string ItemId, string Vendor, int EnqId)
        //{
        //    Vendor = Vendor.Remove(Vendor.Length - 1);
        //    ItemId = ItemId.Remove(ItemId.Length - 1);
        //    EnquiryBL Enqbl = new EnquiryBL();
        //    string User_Id = User.Identity.GetUserId();
        //    try
        //    {
        //        int errCode = Enqbl.SendVenderEnquiryDetail(Vendor, EnqId, ItemId, User_Id);
        //        if (errCode == 500002)
        //        {
        //            return Json("Success", JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Json("Failed", JsonRequestBehavior.AllowGet);
        //}
        public ActionResult VendorEmailDeatiltoSendEnq(int ItemId, int EnqId, string Type,Boolean IsPerson)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            int errCode = 0;
            string User_Id = User.Identity.GetUserId();
            EnqSendVendorModel EnqDetail = new EnqSendVendorModel();
            try
            {
                if (Type == "All")
                {
                    EnqDetail = Enqbl.GetVendorEmailDeatiltoSendAllEnq(EnqId, ItemId, SmartSys.Utility.Enums.MailType.SendSingVendEnq.ToString(), User_Id);
                    if (EnqDetail.EmailUserName != "" || EnqDetail.EmailPassword != "" || EnqDetail.EmailServer != "" || EnqDetail.EmailPort != "")
                    {
                        foreach (EnqSendVendorModel demoModel in EnqDetail.lstEnqSendVendorModel)
                        {
                            ArrayList attachments = new ArrayList();
                            Stream responseStream = null;
                            DataSet dsItem = new DataSet();
                            dsItem = Enqbl.GetVendorItemEnqforCSV(EnqId, demoModel.VendorId, User_Id);
                            string csv = string.Empty;

                            foreach (DataColumn column in dsItem.Tables[0].Columns)
                            {
                                csv += column.ColumnName + ',';
                            }
                            csv = csv.TrimEnd(',');
                            csv += "\r\n";
                            foreach (DataRow dr in dsItem.Tables[0].Rows)
                            {
                                foreach (DataColumn column in dsItem.Tables[0].Columns)
                                {
                                    //Add the Data rows.
                                    if (dr[column.ColumnName].ToString() != "")
                                    {
                                        if (dr[column.ColumnName].ToString().Contains(','))
                                        {
                                            string tempstr = dr[column.ColumnName].ToString();
                                            tempstr = '"' + tempstr + '"';
                                            csv += tempstr + ',';
                                        }
                                        else
                                        {
                                            csv += dr[column.ColumnName].ToString() + ',';
                                        }
                                    }

                                }
                                csv = csv.TrimEnd(',');
                                //Add new line.
                                csv += "\r\n";
                            }
                            responseStream = Utility.Common.GenerateStreamFromString(csv);
                            System.Net.Mail.Attachment at = new System.Net.Mail.Attachment(responseStream, EnqId.ToString() + ".CSV");
                            attachments.Add(at);
                            string CC = "";
                            string Subject = "Vendor Enquiry ";
                            string ToMail = demoModel.emailId;
                            string MailBody = demoModel.MailContent;
                            MailBody = MailBody.Replace("{enquiry_num}", EnqId.ToString());
                            MailBody = MailBody.Replace("{ItemName}", demoModel.ItemName);
                            ArrayList AL = Utility.Common.GetCCMail();
                            foreach (string ArrLst in AL)
                            {
                                if (ArrLst != EnqDetail.EmailUserName)
                                    CC = CC + ArrLst + ",";
                            }
                            CC = CC.Remove(CC.Length - 1);
                            for (int i = 0; i < 3; i++)
                            {
                                try
                                {
                                    int mail = Utility.Common.SendMailDynamic(Subject, MailBody, EnqId.ToString(), EnqDetail.EmailUserName, CC, attachments, EnqDetail.EmailServer, EnqDetail.EmailUserName, EnqDetail.EmailPassword, EnqDetail.EmailPort, ToMail, false);
                                    attachments = null;
                                    at.Dispose();
                                    responseStream.Close();
                                    responseStream = null;
                                    int err = Enqbl.UpdateVendorEnqstatus(EnqId, demoModel.ItemId, demoModel.VendorId, User_Id);
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    System.Threading.Thread.Sleep(5000);
                                    if (i == 2)
                                    {
                                        throw ex;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Session["VendSendEnq"] = " Login User Do not have Complete User Config";
                        return RedirectToAction("ProcessCustomerEnquiry", new { EnqId = EnqId ,IsPerson= IsPerson});
                    }
                }
                else
                {
                    string ToMail = "";
                    EnqDetail = Enqbl.GetVendorEmailDeatiltoSendEnq(EnqId, ItemId, SmartSys.Utility.Enums.MailType.SendSingVendEnq.ToString(), User_Id);
                    if (EnqDetail.EmailUserName != "" || EnqDetail.EmailPassword != "" || EnqDetail.EmailServer != "" || EnqDetail.EmailPort != "")
                    {
                        foreach (EnqSendVendorModel VendModel in EnqDetail.lstEnqSendVendorModel)
                        {
                            ToMail = ToMail + VendModel.emailId + ",";
                        }
                        ToMail = ToMail.Substring(0, ToMail.Length - 1);
                        foreach (EnqSendVendorModel demoModel in EnqDetail.lstEnqSendVendorModel)
                        {
                            string CC = "";
                            ArrayList AL = Utility.Common.GetCCMail();
                            foreach (string ArrLst in AL)
                            {
                                if (ArrLst != EnqDetail.EmailUserName)
                                    CC = CC + ArrLst + ",";
                            }
                            CC = CC.Remove(CC.Length - 1);
                            string Subject = "Vendor Enquiry";
                            string MailBody = demoModel.MailContent;
                            MailBody = MailBody.Replace("{enquiry_num}", EnqId.ToString());
                            MailBody = MailBody.Replace("{ItemName}", demoModel.ItemName);

                            for (int i = 0; i < 3; i++)
                            {
                                try
                                {
                                    int mail = Utility.Common.SendMailDynamic(Subject, MailBody, EnqId.ToString(), EnqDetail.EmailUserName, CC, null, EnqDetail.EmailServer, EnqDetail.EmailUserName, EnqDetail.EmailPassword, EnqDetail.EmailPort, ToMail, false);
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    System.Threading.Thread.Sleep(5000);
                                    if (i == 2)
                                    {
                                        throw ex;
                                    }
                                }
                            }
                            int err = Enqbl.UpdateVendorEnqstatus(EnqId, demoModel.ItemId, 0, User_Id);
                        }
                    }
                    else
                    {
                        Session["VendSendEnq"] = " Login User Do not have Complete User Config";
                        return RedirectToAction("ProcessCustomerEnquiry", new { EnqId = EnqId, IsPerson= IsPerson });
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("ProcessCustomerEnquiry", new { EnqId = EnqId, IsPerson= IsPerson });
        }
        public ActionResult AttchDownload(int EnqId, string DocumentPath)
        {
            try
            {
                String FTPServer = Common.GetConfigValue("FTP");
                String FTPUserId = Common.GetConfigValue("FTPUid");
                String FTPPwd = Common.GetConfigValue("FTPPWD");

                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + DocumentPath);
                requestFileDownload.UseBinary = true;
                requestFileDownload.KeepAlive = false;
                requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                byte[] OPData = ReadFully(responseStream);
                responseStream.Close();

                Response.AddHeader("content-disposition", "attachment; filename=" + DocumentPath);
                Response.ContentType = "application/zip";
                Response.BinaryWrite(OPData);
                Response.End();
                return RedirectToAction("ProcessCustomerEnquiry", new { EnqId = EnqId, IsPerson=false });
            }
            catch (Exception e)
            {
                return RedirectToAction("ProcessCustomerEnquiry", new { EnqId = EnqId ,IsPerson = false });
            }

        }
        public ActionResult AddItemVendorContact(int EnqId,string ItemName,int VendorId,int ContactId)
        {
            ItemName = ItemName.Remove(ItemName.Length - 1);
            EnquiryBL Enqbl = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            List<EnqItemVendorContact> DetailList = new List<EnqItemVendorContact>();
            try
            {
                int errCode = Enqbl.AddItemVendorContact(VendorId, EnqId, ItemName, User_Id, ContactId);
                
                    DetailList = Enqbl.GetItemVendorDetailList(EnqId, User_Id);
                    return Json(DetailList, JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return Json("Failed", JsonRequestBehavior.AllowGet);            
        }
        #endregion Process Enquiry

        #region Enquiry Item Vendor Response Detail

        public ActionResult EnquiryItemVendorResponseDetail()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/EnquiryItemVendorResponseDetail");
            if (FindFlag)
            {
                EnquiryItemVendorResponseDetailModel model = new EnquiryItemVendorResponseDetailModel();
                try
                {
                    EnquiryBL objbl = new EnquiryBL();
                    string User_Id = User.Identity.GetUserId();
                    model = objbl.EnquiryItemVendorResponseDetailList(User_Id);              
                    ReportNameModel Model = new ReportNameModel();
                    Model = objbl.GetCustEnquiryVendResponseReport(User_Id);
                    model.OutputLocation = Model.OutputLocation;
                    model.RunDate = Model.RunDate;
                    model.StatusId = Model.StatusId;
                    model.ReportId = Model.ReportId;
                    model.ParamId = "@FIYear";
                    model.ParamName = "@FIYear";
                    model.hidText = "";
                    model.TxtParamValue = Model.TxtParamValue;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportVendorRespDetailList")]
        [AcceptVerbs("POST")]
        public void ExportVendorRespDetailList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            EnquiryBL objbl = new EnquiryBL();
            EnquiryItemVendorResponseDetailModel model = new EnquiryItemVendorResponseDetailModel();
            model = objbl.EnquiryItemVendorResponseDetailList(User_Id);
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = model.listEnqItemVendRespDetail;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult GetVendorContactList(int VendorId)
        {
            VendorModel VendorModel = new VendorModel();
            try
            {
                VendorBL objblContect = new VendorBL();
                VendorModel = objblContect.DWvendorContactGetselected(VendorId);
                ViewBag.ContactList = new SelectList(VendorModel.VendorContactLst, "VendorContactId", "ContactName");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(VendorModel.VendorContactLst, "VendorContactId", "ContactName"));
        }
        public ActionResult GetCustomerContactList(int CustomerID)
        {
            CustomerModel CustomerModel = new CustomerModel();
            try
            {
                CustomerBL objblContect = new CustomerBL();
                CustomerModel = objblContect.GetSelectedCustomerContact(CustomerID);
                ViewBag.CustContactList = new SelectList(CustomerModel.CustmerContactLst, "CustomerContactId", "ContactName");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(CustomerModel.CustmerContactLst, "CustomerContactId", "ContactName"));
        }
        public ActionResult CreateEnquiryItemVendorResponse(int responseId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/CreateEnquiryItemVendorResponse?responseId=0");
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
                    list = itembl.GetItemListwithMPNbyVendorId(model.VendorId);
                    ViewBag.ItemList = list;

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

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateEnquiryItemVendorResponse(FormCollection fc)
        {
            try
            {
                int errcode = 0;
                string User_id = User.Identity.GetUserId();
                EnquiryBL objbl = new EnquiryBL();
                EnquiryItemVendorResponseDetailModel VendRespModel = new EnquiryItemVendorResponseDetailModel();
                if (Session["VendorresponseDetail"] != null)
                {
                    VendRespModel = Session["VendorresponseDetail"] as EnquiryItemVendorResponseDetailModel;
                }
                VendRespModel.ResponseId = 0;
                if (fc["ResponseId"].ToString() != null || fc["ResponseId"].ToString() != "0")
                {
                    VendRespModel.ResponseId = Convert.ToInt32(fc["ResponseId"].ToString());
                }
                VendRespModel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                VendRespModel.ContactId = Convert.ToInt32(fc["ContactId"].ToString());
                VendRespModel.ReciptMethod = fc["ReciptMethod"].ToString();
                VendRespModel.Remark = fc["Remark"].ToString();
                VendRespModel.Currency = fc["Currency"].ToString();
                VendRespModel.rating = Convert.ToInt32(fc["Rating"].ToString());
                if (fc["ReciptDate"].ToString() != "")
                    VendRespModel.ReciptDate = Convert.ToDateTime(fc["ReciptDate"].ToString());

                errcode = objbl.SaveEnquiryItemVendorResponse(VendRespModel, User_id);
                if (errcode > 0)
                {
                    return RedirectToAction("CreateEnquiryItemVendorResponse", new { responseId = errcode });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("EnquiryItemVendorResponseDetail");
        }
        public ActionResult CheckDupli(int ItemId, int BrandId, double Rate, int MinimumQty, int SeqNo)
        {
            if (SeqNo == 0)
            {
                EnquiryItemVendorResponseDetailModel VendRespModel = new EnquiryItemVendorResponseDetailModel();
                if (Session["VendorresponseDetail"] != null)
                {
                    VendRespModel = Session["VendorresponseDetail"] as EnquiryItemVendorResponseDetailModel;
                }
                if (VendRespModel.listEnqItemVendRespDetail.Count > 0)
                {
                    var actionToDelete = from actionRepDel in VendRespModel.listEnqItemVendRespDetail.Where(s => (s.ItemId == ItemId) && (s.BrandId == BrandId) && (s.Rate == Rate) && (s.MinimumQty == MinimumQty))
                                         where (actionRepDel.ItemId == ItemId) && (actionRepDel.BrandId == BrandId) && (actionRepDel.Rate == Rate) && (actionRepDel.MinimumQty == MinimumQty)
                                         select actionRepDel;

                    if (actionToDelete.Count() > 0)
                    {
                        return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveItemVendorResponse(int ItemId, string ItemName, int BrandId, string BrandName, double Rate, int MinimumQty, string BatchSize, string VendorPromiseDate, int SeqNo, int SPQ, int MOQ)
        {
            EnquiryItemVendorResponseDetailModel VendRespModel = new EnquiryItemVendorResponseDetailModel();
            int errcode = 0;
            int TempSeqNo = 0;
            string User_id = User.Identity.GetUserId();
            EnquiryBL objbl = new EnquiryBL();
            try
            {
                EnquiryItemVendorResponseDetailModel Model = new EnquiryItemVendorResponseDetailModel();
                if (Session["VendorresponseDetail"] != null)
                {
                    VendRespModel = Session["VendorresponseDetail"] as EnquiryItemVendorResponseDetailModel;
                }
                if (VendRespModel.listEnqItemVendRespDetail.Count > 0)
                {
                    //var actionToDelete = from actionRepDel in VendRespModel.listEnqItemVendRespDetail.Where(s => (s.ItemId == ItemId) && (s.BrandId == BrandId) && (s.Rate == Rate) && (s.MinimumQty == MinimumQty))
                    //                     where (actionRepDel.ItemId == ItemId )&& (actionRepDel.BrandId == BrandId) && (actionRepDel.Rate == Rate) && (actionRepDel.MinimumQty == MinimumQty)
                    //                     select actionRepDel;
                    var actionToDelete = from actionRepDel in VendRespModel.listEnqItemVendRespDetail.Where(s => (s.SeqNo == SeqNo))
                                         where (actionRepDel.SeqNo == SeqNo)
                                         select actionRepDel;
                    if (actionToDelete.Count() > 0)
                    {
                        VendRespModel.listEnqItemVendRespDetail.Remove(actionToDelete.ElementAt(0));
                        TempSeqNo = SeqNo;
                    }
                    else
                    {
                        TempSeqNo = VendRespModel.listEnqItemVendRespDetail.Count + 1;
                    }
                }
                else
                {
                    TempSeqNo = VendRespModel.listEnqItemVendRespDetail.Count + 1;
                }
                if (VendRespModel.listEnqItemVendRespDetail == null)
                    VendRespModel.listEnqItemVendRespDetail = new List<EnquiryItemVendorResponseDetailModel>();
                Model.SeqNo = TempSeqNo;
                Model.ItemId = ItemId;
                Model.ItemName = ItemName;
                Model.MPN = ItemName;
                Model.BrandId = BrandId;
                Model.BrandName = BrandName;
                Model.Rate = Rate;
                Model.MinimumQty = MinimumQty;
                Model.BatchNumber = BatchSize;
                Model.StrVendorPromiseDate = VendorPromiseDate.ToString();
                Model.VendorPromiseDate = VendorPromiseDate;
                Model.SPQ = SPQ;
                Model.MOQ = MOQ;

                VendRespModel.listEnqItemVendRespDetail.Add(Model);
                Session["VendorresponseDetail"] = VendRespModel;
                errcode = objbl.SaveEnquiryItemVendorResponseDetails(VendRespModel, User_id, VendRespModel.ResponseId, VendRespModel.VendorId);
                Session["VendorresponseDetail"] = VendRespModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            var jsonResult = Json(VendRespModel.listEnqItemVendRespDetail, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public FileResult DownloadResponseSample()
        {
            var filepath = Path.Combine(Server.MapPath("~/App_Data/MyFiles/SampleResponseExcel.xlsx"));
            string fileName = "SampleResponseExcel.xlsx";
            return File(filepath, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult UploadResponseExcelFile()
        {
            string User_Id = User.Identity.GetUserId();
            EnquiryItemVendorResponseDetailModel VendRespModel = new EnquiryItemVendorResponseDetailModel();
            VendRespModel.listEnqItemVendRespDetail = new List<EnquiryItemVendorResponseDetailModel>();
            if (Session["VendorresponseDetail"] != null)
            {
                VendRespModel = Session["VendorresponseDetail"] as EnquiryItemVendorResponseDetailModel;
            }
            if (VendRespModel.listEnqItemVendRespDetail == null)
                VendRespModel.listEnqItemVendRespDetail = new List<EnquiryItemVendorResponseDetailModel>();
            try
            {

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
                        //var data = new XlsData();
                        WorkbookPart workbookPart; List<Row> rows;

                        //FileStream filStream = new FileStream(filePath, FileMode.Open);

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

                        for (int i = 1; i < rows.Count; i++)
                        {
                            EnquiryItemVendorResponseDetailModel Model = new EnquiryItemVendorResponseDetailModel();

                            Row r = rows[i];
                            foreach (DocumentFormat.OpenXml.Spreadsheet.Cell c in r.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>())
                            {
                                var match = regex.Match(c.CellReference);
                                string strColumnName = match.Value;
                                string strText = "";
                                if (strColumnName == "A")
                                {
                                    strText = c.CellValue.InnerXml;
                                    Model.ItemId = Convert.ToInt32(strText);
                                }
                                else if (strColumnName == "B")
                                {
                                    strText = c.CellValue.InnerXml;
                                    strText = workbookPart.SharedStringTablePart.SharedStringTable
                                         .Elements<SharedStringItem>().ElementAt(
                                         Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                    Model.MPN = strText;
                                }
                                else if (strColumnName == "C")
                                {
                                    strText = c.CellValue.InnerXml;
                                    strText = workbookPart.SharedStringTablePart.SharedStringTable
                                             .Elements<SharedStringItem>().ElementAt(
                                             Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                    Model.ItemName = strText;
                                }
                                else if (strColumnName == "D")
                                {
                                    strText = c.CellValue.InnerXml;
                                    strText = workbookPart.SharedStringTablePart.SharedStringTable
                                             .Elements<SharedStringItem>().ElementAt(
                                             Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                    Model.BrandName = strText;
                                }
                                else if (strColumnName == "E")
                                {
                                    strText = c.CellValue.InnerXml;
                                    Model.MinimumQty = Convert.ToDouble(strText);
                                }
                                else if (strColumnName == "F")
                                {
                                    strText = c.CellValue.InnerXml;
                                    Model.Rate = Convert.ToDouble(strText);
                                }
                                else if (strColumnName == "G")
                                {
                                    strText = c.CellValue.InnerXml;
                                    strText = workbookPart.SharedStringTablePart.SharedStringTable
                                           .Elements<SharedStringItem>().ElementAt(
                                           Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                    Model.Currency = strText;
                                }
                                else if (strColumnName == "H")
                                {
                                    strText = c.CellValue.InnerXml;
                                    Model.SPQ = Convert.ToInt32(strText);
                                }
                                else if (strColumnName == "I")
                                {
                                    strText = c.CellValue.InnerXml;
                                    Model.MOQ = Convert.ToInt32(strText);
                                }
                                else if (strColumnName == "J")
                                {
                                    strText = c.CellValue.InnerXml;
                                    strText = workbookPart.SharedStringTablePart.SharedStringTable
                                            .Elements<SharedStringItem>().ElementAt(
                                            Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                    Model.VendorPromiseDate = strText;
                                }
                                else if (strColumnName == "K")
                                {
                                    strText = c.CellValue.InnerXml;
                                    DateTime d = DateTime.FromOADate(Convert.ToDouble(strText));
                                    Model.StrVendorPromiseDate = Convert.ToString(d);
                                    Model.Remark = Model.StrVendorPromiseDate;
                                }
                            }
                            VendRespModel.listEnqItemVendRespDetail.Add(Model);
                        }
                        document.Close();
                        System.IO.File.Delete(filepath);

                    }
                }

                EnquiryBL BLObj = new EnquiryBL();
                VendRespModel.listEnqItemVendRespDetail = BLObj.GetVendResponseCompList(VendRespModel, User_Id);
                Session["EnquiryDetail"] = VendRespModel;
                return Json(VendRespModel.listEnqItemVendRespDetail, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SaveExceData(int ResponseId, int VendorId)
        {
            EnquiryItemVendorResponseDetailModel VendRespModel = new EnquiryItemVendorResponseDetailModel();
            if (Session["EnquiryDetail"] != null)
            {
                VendRespModel = Session["EnquiryDetail"] as EnquiryItemVendorResponseDetailModel;
            }
            EnquiryBL BLSave = new EnquiryBL();
            int errcode = BLSave.SaveVendorResponseByItem(VendRespModel, ResponseId, VendorId);
            return Json(VendRespModel.listEnqItemVendRespDetail, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Quotation

        #region New Quotation Work 
        public ActionResult Quotation()
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
            if (Session["info"] != null)
            {
                string str = Session["info"] as string;
                TempData["Message"] = str;
                Session["info"] = null;
            }
            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/Quotation");
            if (FindFlag)
            {
                EnquiryModel EnquModel = new EnquiryModel();
                try
                {
                    string User_Id = User.Identity.GetUserId();
                    EnquiryBL BLObj = new EnquiryBL();
                    //EnquModel = BLObj.GetEnquiryListForQuotation(User_Id);

                }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                }

                return View(EnquModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult QuotationList()
        {
            EnquiryModel EnquModel = new EnquiryModel();
            EnquModel.lstEnquiry = new List<EnquiryModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                EnquiryBL BLObj = new EnquiryBL();
                EnquModel = BLObj.GetEnquiryListForQuotation(User_Id);
                IEnumerable data = EnquModel.lstEnquiry;
                var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [System.Web.Http.ActionName("ExportQuotationDetailList")]
        [AcceptVerbs("POST")]
        public void ExportQuotationDetailList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            EnquiryBL BLObj = new EnquiryBL();
            EnquiryModel EnquModel = new EnquiryModel();
            EnquModel = BLObj.GetEnquiryListForQuotation(User_Id);
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = EnquModel.lstEnquiry;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public JsonResult CreateQuotation(int ItemId, int CustomerId, int EnqId)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";
            string characters = numbers;
            characters += alphabets + small_alphabets + numbers;
            int length = 5;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            String QuotationNumber = "QT" + otp + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year;

            int Errorcode = Enqbl.CreateQuotation(ItemId, CustomerId, QuotationNumber, EnqId, User_Id);
            return Json(Errorcode, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReviewedQuotation(int QuotId, int EnqId)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";
            string characters = numbers;
            characters += alphabets + small_alphabets + numbers;
            int length = 5;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            String QuotationNumber = "QT" + otp + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year;

            int Errorcode = Enqbl.ReviwedQuotation(QuotId, EnqId, User_Id, QuotationNumber);
            return RedirectToAction("PrepareQuotation", new { EnqId = EnqId, QuotId = Errorcode });
        }
        #endregion New Quotation Work 


        public ActionResult PrepareQuotation(int EnqId, int QuotId)
        {
            EnquiryModel Enqmodel = new EnquiryModel();
            try
            {

                if (Session["info"] != null)
                {
                    string str = Session["info"] as string;
                    TempData["Message"] = str;
                    Session["info"] = null;
                }
                Enqmodel.QuotList = new List<QuotationModel>();
                QuotationModel quotmodel = new QuotationModel();
                EnquiryBL Enqbl = new EnquiryBL();
                Enqmodel = Enqbl.GetSelectedEnqQuotation(EnqId, QuotId);
                string User_Id = User.Identity.GetUserId();
                List<EnquiryItemVendorResponseDetailModel> CustItemList = new List<EnquiryItemVendorResponseDetailModel>();
                CustItemList = Enqbl.GetCustQuotItem(EnqId, QuotId);
                ViewBag.CustItemList = CustItemList;
                List<Paytermsmodel> PTList = new List<Paytermsmodel>();
                BOMAdminBL objbl = new BOMAdminBL();
                PTList = objbl.GetPaymentTermsList(User_Id);
                ViewBag.Paymentterms = new SelectList(PTList, "PTId", "PTCode", Enqmodel.TermId);
                List<DrpItem> CUstomer = new List<DrpItem>();
                ProjectBL ObjBl = new ProjectBL();
                CUstomer = ObjBl.GetCustomerListByUser(User_Id);
                ViewBag.CustList = new SelectList(CUstomer, "Id", "DisplayName");

                TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                List<CurrencyCodes> lstModel = new List<CurrencyCodes>();
                lstModel = ObjTMBL.GetCurrencyCodes();
                ViewBag.Currency = new SelectList(lstModel, "Key", "Value");

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View(Enqmodel);
        }
        public ActionResult ResponseListByItem(int EnqId, string GenerateMode, int QuotId, double Margin, int ItemId)
        {

            List<EnquiryItemVendorResponseDetailModel> ResponseList = new List<EnquiryItemVendorResponseDetailModel>();
            try
            {

                EnquiryBL Enqbl = new EnquiryBL();
                ViewBag.EnqId = EnqId;
                ViewBag.QuotId = QuotId;
                ResponseList = Enqbl.GetVendorResponseByItem(ItemId, EnqId, QuotId);
                List<EnquiryItemVendorResponseDetailModel> ALLResponseList = new List<EnquiryItemVendorResponseDetailModel>();
                ALLResponseList = Enqbl.GetAllVendorResponseByItem(ItemId, EnqId, QuotId);
                if (GenerateMode.ToString() == "Auto")
                {
                    foreach (var item in ALLResponseList)
                    {
                        EnquiryItemVendorResponseDetailModel model = new EnquiryItemVendorResponseDetailModel();
                        model.BrandId = item.BrandId;
                        Double Rate = item.Rate;
                        Double margin = Rate * Margin / 100;
                        Double rate = margin + Rate;
                        model.ItemId = item.ItemId;
                        model.MinimumQty = Convert.ToDouble(item.MinimumQty);
                        model.Rate = rate;
                        model.Remark = "Auto Generated";
                        model.ResponseId = item.ResponseId;
                        model.BatchNumber = item.BatchNumber;
                        int Errorcode = Enqbl.SaveCustQuotItemDetails(model, QuotId);
                    }
                }


            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return RedirectToAction("Quotation");
            }
            if (GenerateMode.ToString() == "Auto")
            {
                return RedirectToAction("PrepareQuotation", new { EnqId = EnqId, QuotId = QuotId });
            }
            else
            {
                return View(ResponseList);
            }

        }

        public JsonResult GetExchangeRate(string Toccy, string FromCCy, Double Amount)
        {
            Double ERate = 0;
            EnquiryBL Enqbl = new EnquiryBL();
            if (Toccy == "")
            {

            }
            else
            {
                ERate = Enqbl.GetExchangeRate(Toccy, FromCCy, Amount);
            }
            return Json(ERate, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveResponse(int ItemId, int ResponseId, int BrandId, float Rate, float MinimumQty, int EnqId, string Remark, int QuotId)
        {
            try
            {
                EnquiryBL Enqbl = new EnquiryBL();
                string User_Id = User.Identity.GetUserId();
                //ArrayList lst = (ArrayList)TempData["lst"];
                //var QuotationNumber = lst[0].ToString();

                //var QuotationId = Convert.ToInt32(lst[1].ToString());
                int Errorcode = Enqbl.SaveQuotation(ItemId, ResponseId, BrandId, Rate, MinimumQty, EnqId, User_Id, Remark, QuotId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("PrepareQuotation", new { EnqId = EnqId, QuotId = QuotId });

        }
        public ActionResult UpdateQuotationTerm(int Terms, int EnqId, int QuotId, string Currency, string Remark, int Rating)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            int Errorcode = Enqbl.UpdateQuotation(Terms, QuotId, Currency, Remark, Rating, User_Id);
            //  ArrayList lst = (ArrayList)Session["myValues"];
            //return RedirectToAction("Quotation");
            return RedirectToAction("PrepareQuotation", new { EnqId = EnqId, QuotId = QuotId });
        }
        public void QuotPreviewExport(string GridModel)
        {
            // string user_Id = User.Identity.GetUserId();
            EnquiryBL Enqbl = new EnquiryBL();
            string QuotId = Session["QuotId"] as string;
            var DataSource = Enqbl.GetPreviewListData(Convert.ToInt32(QuotId));
            // var DataSource = Session["PreviewQuot"] as List<PreViewQuotModel>;
            // Session["PreviewQuot"] = null;
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public void EnquiryExport(string GridModel)
        {
            string user_Id = User.Identity.GetUserId();
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            EnquiryModel EnquModel = new EnquiryModel();

            EnquModel = BLObj.getEnquiryList(User_Id);

            var DataSource = EnquModel.lstEnquiry;
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult QuotaionListForApproval()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/QuotaionListForApproval");
            if (FindFlag)
            {
                EnquiryBL Enqbl = new EnquiryBL();
                string User_Id = User.Identity.GetUserId();
                List<QuotationModel> QuotList = new List<QuotationModel>();
                QuotList = Enqbl.GetQuotaionListForApproval(User_Id);
                return View(QuotList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportQuotaionListForApproval")]
        [AcceptVerbs("POST")]
        public void ExportQuotaionListForApproval(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL Enqbl = new EnquiryBL();
            var DataSource = Enqbl.GetQuotaionListForApproval(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult ApprovalView(int EnqId, int QuotId)
        {
            EnquiryModel Enqmodel = new EnquiryModel();
            Enqmodel.QuotList = new List<QuotationModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            Enqmodel = Enqbl.GetSelectedEnqQuotation(EnqId, QuotId);
            List<EnquiryItemVendorResponseDetailModel> CustItemList = new List<EnquiryItemVendorResponseDetailModel>();
            CustItemList = Enqbl.GetCustQuotItemforApproval(EnqId, QuotId);
            ViewBag.CustItemList = CustItemList;

            return View(Enqmodel);
        }
        public ActionResult UpdateQuotation(int EnqId, int StatusId, int QuotationId, string Remark)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            int ErrorCode = Enqbl.UpdateQuotationForApprovals(EnqId, StatusId, QuotationId, Remark, User_Id);
            if (StatusId == 24)
            {
                return RedirectToAction("QuotaionListForApproval");
            }
            else
            {
                return RedirectToAction("PrepareQuotation", new { EnqId = EnqId, QuotId = QuotationId });
            }

        }
        public ActionResult PreviewQuotation(int QuotId, string QuotNumber, int EnqId, string FileName)
        {
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");
            ArrayList ConnInfo = new ArrayList();
            ConnInfo = Common.GetConnectionInfoFromWebConfig();
            if (FileName == "")
            {
                CrystalDecisions.Shared.ConnectionInfo crconnectioninfo = new CrystalDecisions.Shared.ConnectionInfo();
                crconnectioninfo.ServerName = ConnInfo[0].ToString().Trim();
                crconnectioninfo.DatabaseName = ConnInfo[1].ToString().Trim();
                crconnectioninfo.UserID = ConnInfo[2].ToString().Trim();
                crconnectioninfo.Password = ConnInfo[3].ToString().Trim();

                CustQuotation rptquot = new SmartSys.CustQuotation();

                CrystalDecisions.Shared.TableLogOnInfos crtablelogoninfos = new CrystalDecisions.Shared.TableLogOnInfos();
                CrystalDecisions.Shared.TableLogOnInfo crtablelogoninfo = new CrystalDecisions.Shared.TableLogOnInfo();

                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in rptquot.Database.Tables)
                {
                    crtablelogoninfo = CrTable.LogOnInfo;
                    crtablelogoninfo.ConnectionInfo = crconnectioninfo;
                    CrTable.ApplyLogOnInfo(crtablelogoninfo);
                }

                rptquot.SetParameterValue(0, QuotId);
                //rptquot.VerifyDatabase();
                Stream stream = rptquot.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                Stream stream1 = rptquot.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, System.IO.SeekOrigin.Begin);

                string FilePath = Convert.ToString("/Quotations/" + QuotNumber + "_" + EnqId + "_" + DateTime.Now.ToString("ddMMyyyyhhmm") + "." + "pdf");
                FileStream fileStream = stream as FileStream;
                FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FilePath);
                requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                int bufferLength = 2048;
                byte[] buffer = new byte[bufferLength];
                Stream uploadStream = requestFTPUploader.GetRequestStream();
                int contentLength = fileStream.Read(buffer, 0, bufferLength);
                while (contentLength != 0)
                {
                    uploadStream.Write(buffer, 0, contentLength);
                    contentLength = fileStream.Read(buffer, 0, bufferLength);
                }
                uploadStream.Close();
                fileStream.Close();
                requestFTPUploader = null;
                EnquiryBL Enqbl = new EnquiryBL();
                int errcode = Enqbl.UpdateQuotFileName(QuotId, FilePath);
                return new FileStreamResult(stream1, "application/pdf");
            }
            else
            {
                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                requestFileDownload.UseBinary = true;
                requestFileDownload.KeepAlive = false;
                requestFileDownload.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                return new FileStreamResult(responseStream, "application/pdf");
            }


            ///EnquiryBL Enqbl = new EnquiryBL();           
            // Session["QuotId"] = QuotId;
            ///List<PreViewQuotModel> lst = new List<PreViewQuotModel>();
            ///lst = Enqbl.GetPreviewListData(QuotId);            
            // Session["PreviewQuot"] = lst;
            ///return PartialView(lst);
        }
        public ActionResult RefreshPreview(int QuotId, string QuotNumber, int EnqId, string FileName)
        {
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");
            try
            {
                FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                requestFileDelete.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                requestFileDelete = null;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            ArrayList ConnInfo = new ArrayList();
            FileName = "";
            ConnInfo = Common.GetConnectionInfoFromWebConfig();
            if (FileName == "")
            {
                CrystalDecisions.Shared.ConnectionInfo crconnectioninfo = new CrystalDecisions.Shared.ConnectionInfo();
                crconnectioninfo.ServerName = ConnInfo[0].ToString().Trim();
                crconnectioninfo.DatabaseName = ConnInfo[1].ToString().Trim();
                crconnectioninfo.UserID = ConnInfo[2].ToString().Trim();
                crconnectioninfo.Password = ConnInfo[3].ToString().Trim();

                CustQuotation rptquot = new SmartSys.CustQuotation();

                CrystalDecisions.Shared.TableLogOnInfos crtablelogoninfos = new CrystalDecisions.Shared.TableLogOnInfos();
                CrystalDecisions.Shared.TableLogOnInfo crtablelogoninfo = new CrystalDecisions.Shared.TableLogOnInfo();

                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in rptquot.Database.Tables)
                {
                    crtablelogoninfo = CrTable.LogOnInfo;
                    crtablelogoninfo.ConnectionInfo = crconnectioninfo;
                    CrTable.ApplyLogOnInfo(crtablelogoninfo);
                }

                rptquot.SetParameterValue(0, QuotId);
                //rptquot.VerifyDatabase();
                Stream stream = rptquot.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                Stream stream1 = rptquot.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, System.IO.SeekOrigin.Begin);

                string FilePath = Convert.ToString("/Quotations/" + QuotNumber + "_" + EnqId + "_" + DateTime.Now.ToString("ddMMyyyyhhmm") + "." + "pdf");
                FileStream fileStream = stream as FileStream;
                FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FilePath);
                requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                int bufferLength = 2048;
                byte[] buffer = new byte[bufferLength];
                Stream uploadStream = requestFTPUploader.GetRequestStream();
                int contentLength = fileStream.Read(buffer, 0, bufferLength);
                while (contentLength != 0)
                {
                    uploadStream.Write(buffer, 0, contentLength);
                    contentLength = fileStream.Read(buffer, 0, bufferLength);
                }
                uploadStream.Close();
                fileStream.Close();
                requestFTPUploader = null;
                EnquiryBL Enqbl = new EnquiryBL();
                int errcode = Enqbl.UpdateQuotFileName(QuotId, FilePath);
                return new FileStreamResult(stream1, "application/pdf");
            }
            return RedirectToAction("PrepareQuotation", new { EnqId = EnqId, QuotId = QuotId });
        }
        public ActionResult CustomerEmailDeatiltoSendQuotation(int QuotId, string QuotNumber, int EnqId, string FileName)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            string FilePath = "";
            string Type = "All";
            string User_Id = User.Identity.GetUserId();
            EnquiryModel EnqDetail = new EnquiryModel();
            EnqDetail.QuotList = new List<QuotationModel>();
            EnqDetail.lstMail = new List<EnqSendVendorModel>();
            try
            {
                if (Type == "All")
                {
                    EnqDetail = Enqbl.GetCustEmailDeatiltoSendQuotation(QuotId, SmartSys.Utility.Enums.MailType.SendCustQuot.ToString(), User_Id);
                    if (EnqDetail.lstMail[0].EmailUserName != "" && EnqDetail.lstMail[0].EmailPassword != "" && EnqDetail.lstMail[0].EmailServer != "" && EnqDetail.lstMail[0].EmailPort != "")
                    {
                        ArrayList attachments = new ArrayList();
                        DataSet dsItem = new DataSet();
                        string ftpServer = Common.GetConfigValue("FTP");
                        string ftpUid = Common.GetConfigValue("FTPUid");
                        string ftpPwd = Common.GetConfigValue("FTPPWD");
                        ArrayList ConnInfo = new ArrayList();
                        ConnInfo = Common.GetConnectionInfoFromWebConfig();
                        if (FileName == "")
                        {
                            CrystalDecisions.Shared.ConnectionInfo crconnectioninfo = new CrystalDecisions.Shared.ConnectionInfo();
                            crconnectioninfo.ServerName = ConnInfo[0].ToString().Trim();
                            crconnectioninfo.DatabaseName = ConnInfo[1].ToString().Trim();
                            crconnectioninfo.UserID = ConnInfo[2].ToString().Trim();
                            crconnectioninfo.Password = ConnInfo[3].ToString().Trim();
                            CustQuotation rptquot = new SmartSys.CustQuotation();
                            CrystalDecisions.Shared.TableLogOnInfos crtablelogoninfos = new CrystalDecisions.Shared.TableLogOnInfos();
                            CrystalDecisions.Shared.TableLogOnInfo crtablelogoninfo = new CrystalDecisions.Shared.TableLogOnInfo();
                            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in rptquot.Database.Tables)
                            {
                                crtablelogoninfo = CrTable.LogOnInfo;
                                crtablelogoninfo.ConnectionInfo = crconnectioninfo;
                                CrTable.ApplyLogOnInfo(crtablelogoninfo);
                            }

                            CrystalDecisions.Shared.ExportOptions eo = new CrystalDecisions.Shared.ExportOptions();
                            eo.ExportFormatType = CrystalDecisions.Shared.ExportFormatType.PortableDocFormat;
                            eo.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
                            rptquot.SetParameterValue(0, QuotId);
                            Stream stream = rptquot.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            Stream stream1 = rptquot.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            stream.Seek(0, System.IO.SeekOrigin.Begin);

                            FilePath = Convert.ToString("Q" + "_" + QuotNumber + "_" + EnqId + "_" + DateTime.Now.ToString("ddMMyyyyhhmm") + "." + "pdf");
                            FileStream fileStream = stream as FileStream;
                            FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FilePath);
                            requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                            requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                            int bufferLength = 2048;
                            byte[] buffer = new byte[bufferLength];
                            Stream uploadStream = requestFTPUploader.GetRequestStream();
                            int contentLength = fileStream.Read(buffer, 0, bufferLength);
                            while (contentLength != 0)
                            {
                                uploadStream.Write(buffer, 0, contentLength);
                                contentLength = fileStream.Read(buffer, 0, bufferLength);
                            }
                            int errcode = Enqbl.UpdateQuotFileName(QuotId, FilePath);
                            System.Net.Mail.Attachment at1 = new System.Net.Mail.Attachment(fileStream, "Q_" + EnqDetail.QuotList[0].QuotationNumber + "_" + EnqDetail.QuotList[0].EnqNumber + ".pdf");
                            attachments.Add(at1);
                            uploadStream.Close();
                            fileStream.Close();
                            requestFTPUploader = null;
                        }
                        else
                        {
                            FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                            requestFileDownload.UseBinary = true;
                            requestFileDownload.KeepAlive = false;
                            requestFileDownload.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                            requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
                            FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                            Stream rs = responseFileDownload.GetResponseStream();
                            System.Net.Mail.Attachment at2 = new System.Net.Mail.Attachment(rs, "Q_" + EnqDetail.QuotList[0].QuotationNumber + "_" + EnqDetail.QuotList[0].EnqNumber + ".pdf");
                            attachments.Add(at2);
                        }

                        string Subject = "Customer Quotation";
                        string ToMail = EnqDetail.QuotList[0].EMailId;
                        string MailBody = EnqDetail.QuotList[0].MailContent;
                        MailBody = MailBody.Replace("{enquiry_num}", Convert.ToString(EnqDetail.QuotList[0].EnqId));
                        MailBody = MailBody.Replace("{customer_name}", EnqDetail.QuotList[0].CustomerName);
                        MailBody = MailBody.Replace("{company_name}", EnqDetail.QuotList[0].CompCode);
                        MailBody = MailBody.Replace("{siteLogo}", "");
                        MailBody = MailBody.Replace("{siteMail}", EnqDetail.lstMail[0].EmailUserName);

                        for (int i = 0; i < 3; i++)
                        {
                            try

                            {

                                int mail = Utility.Common.SendMailDynamic(Subject, MailBody, EnqDetail.QuotList[0].EnqNumber, EnqDetail.lstMail[0].EmailUserName, ToMail, attachments, EnqDetail.lstMail[0].EmailServer, EnqDetail.lstMail[0].EmailUserName, EnqDetail.lstMail[0].EmailPassword, EnqDetail.lstMail[0].EmailPort, "", false);
                                attachments = null;
                                Session["info"] = "Email Succesfully Send to Customer.";
                                int ErrorCode = Enqbl.UpdateQuotationForApprovals(EnqDetail.QuotList[0].EnqId, 45, QuotId, "Quatation Sent To the Customer", User_Id);
                                break;
                            }
                            catch (Exception ex)
                            {
                                System.Threading.Thread.Sleep(5000);
                                attachments = null;
                                attachments = new ArrayList();
                                if (FilePath != "")
                                {
                                    FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(ftpServer + FilePath);
                                    requestFileDownload.UseBinary = true;
                                    requestFileDownload.KeepAlive = false;
                                    requestFileDownload.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                    requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
                                    FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                                    Stream rs = responseFileDownload.GetResponseStream();
                                    System.Net.Mail.Attachment at2 = new System.Net.Mail.Attachment(rs, "Q_" + EnqDetail.QuotList[0].QuotationNumber + "_" + EnqDetail.QuotList[0].EnqNumber + ".pdf");
                                    attachments.Add(at2);
                                }
                                if (i == 2)
                                {
                                    throw ex;
                                }
                            }
                        }

                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                Session["info"] = "Email Not Succesfully Send to Customer.Please Try Again !";
                return RedirectToAction("PrepareQuotation", new { EnqId = EnqId, QuotId = QuotId });
            }
            return RedirectToAction("PrepareQuotation", new { EnqId = EnqId, QuotId = QuotId });
        }
        public ActionResult CloseEnquiryMail(int EnqId,string SourcePage, int Status, string Remark)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            string FilePath = "";
            string Type = "All";
            string User_Id = User.Identity.GetUserId();
            EnquiryModel EnqDetail = new EnquiryModel();
            EnqDetail.QuotList = new List<QuotationModel>();
            EnqDetail.lstMail = new List<EnqSendVendorModel>();
            try
            {
                EnqDetail = Enqbl.CloseEnquiryMail(EnqId, SmartSys.Utility.Enums.MailType.SendClsEnqMail.ToString(), User_Id);
                if (EnqDetail.lstMail[0].EmailUserName != "" && EnqDetail.lstMail[0].EmailPassword != "" && EnqDetail.lstMail[0].EmailServer != "" && EnqDetail.lstMail[0].EmailPort != "")
                {
                    ArrayList attachments = new ArrayList();
                    DataSet dsItem = new DataSet();
                    string ftpServer = Common.GetConfigValue("FTP");
                    string ftpUid = Common.GetConfigValue("FTPUid");
                    string ftpPwd = Common.GetConfigValue("FTPPWD");
                    ArrayList ConnInfo = new ArrayList();
                    ConnInfo = Common.GetConnectionInfoFromWebConfig();
                    string Subject = "Close Enquiry";
                    string TOMailId =  Utility.Common.GetCEOId();
                    string ToMail = TOMailId;//EnqDetail.QuotList[0].EMailId; //"vijaya@supermatictech.com";
                    string MailBody = EnqDetail.QuotList[0].MailContent;
                    //DataSet dsItem = new DataSet();
                    dsItem = Enqbl.CloseEnquiryMailItems(EnqId, SmartSys.Utility.Enums.MailType.SendClsEnqMail.ToString(), User_Id);
                    string txt = string.Empty;
                    txt += "<table border='1'><tr style='background-color:#000000;color:white'></tr>";
                    foreach (DataColumn column in dsItem.Tables[2].Columns)
                    {
                        txt += "<th>" + column.ColumnName + "</th>";
                    }
                    txt += "</tr>";
                    foreach (DataRow dr in dsItem.Tables[2].Rows)
                    {
                        txt += "<tr>";
                        foreach (DataColumn column in dsItem.Tables[2].Columns)
                        {
                            //Add the Data rows.
                            string tempstr = dr[column.ColumnName].ToString();
                            txt += "<td>" + tempstr + "</td>";
                        }
                        txt += "</tr>";
                    }
                    txt += "</table>";
                    MailBody = MailBody.Replace("{Items:}", txt.ToString());
                    MailBody = MailBody.Replace("{EnqId:}", EnqId.ToString());
                    MailBody=MailBody.Replace("{UserId}", EnqDetail.lstMail[0].EmpName);
                    try
                    {
                        int mail = Utility.Common.SendMailDynamic(Subject, MailBody, "", EnqDetail.lstMail[0].EmailUserName, ToMail, attachments, EnqDetail.lstMail[0].EmailServer, EnqDetail.lstMail[0].EmailUserName, EnqDetail.lstMail[0].EmailPassword, EnqDetail.lstMail[0].EmailPort, "", false);
                        Session["info"] = "Email Succesfully Send.";
                        //int ErrCode = 0;
                        //ErrCode = Enqbl.UpdateCustEnqStatusAfterMailFail(EnqId, Status, User_Id, Remark);
                    }
                    catch (Exception Ex)
                    {
                        Common.LogException(Ex);                       
                        int ErrCode = 0;
                        ErrCode= Enqbl.UpdateCustEnqStatusAfterMailFail(EnqId, Status ,User_Id, Remark);
                        Session["info"] = "Email Not Succesfully Send..hence enquiry is not get close.. Please Try Again !";
                        return RedirectToAction(SourcePage);
                        throw;
                    }
                    
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                Session["info"] = "Email Not Succesfully Send.Please Try Again !";
                return RedirectToAction(SourcePage);
            }           
            return RedirectToAction(SourcePage);
        }
        #endregion Quotation

        #region Purchase Order
        public ActionResult CustPOList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/CustPOList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                CustomerPOModel POModel = new CustomerPOModel();
                UserId = User.Identity.GetUserId();
                try
                {
                    if (Session["PODetail"] != null)
                    {
                        Session["PODetail"] = null;
                    }
                    EnquiryBL BLObj = new EnquiryBL();
                    POModel = BLObj.getCustPOList(User_Id, 0);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(POModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportSODetailList")]
        [AcceptVerbs("POST")]
        public void ExportSODetailList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            EnquiryBL BLObj = new EnquiryBL();
            CustomerPOModel POModel = new CustomerPOModel();
            POModel = BLObj.getCustPOList(User_Id, 0);
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = POModel.POlist;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CretaePurchaseOrder(int PurchaseOrderId)
        {
            CustomerPOModel POModel = new CustomerPOModel();
            EnquiryBL Enqbl = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                if (Session["infoSO"] != null)
                {
                    string str = Session["infoSO"] as string;
                    TempData["Message"] = str;
                    Session["infoSO"] = null;
                }
                ItemModel itemModel = new ItemModel();
                BudgetModel budgetModel = new BudgetModel();
                BudgetBL ObjBl = new BudgetBL();
                ItemBL BLObj = new ItemBL();
                POModel.PODetaillist = new List<CustomerPODetailModel>();
                itemModel.lstItem = new List<ItemModel>();
                POModel.DocumentFile = "";
                if (PurchaseOrderId > 0)
                {
                    POModel = Enqbl.getSelectedPODetail(PurchaseOrderId);
                    itemModel.lstItem = BLObj.GetItemListFromCustQuotation(POModel.CustomerId);
                }

                ViewBag.itemlist = new SelectList(itemModel.lstItem, "ItemId", "ItemName");
                ProjectBL Blobj = new ProjectBL();
                ViewBag.CustomerLst = new SelectList(Blobj.GetCustomerListByUser(User_Id), "Id", "DisplayName");
                List<QuotationModel> lstQuot = new List<QuotationModel>();
                lstQuot = Enqbl.GetQuotationListfordrpdwn(User_Id);
                ViewBag.QuotLst = new SelectList(lstQuot, "QuotationId", "QuotationNumber");

                ItemBL BlObj = new ItemBL();
                ItemMappingModel ItemMap = new ItemMappingModel();
                ItemMap.lstItemMap = BlObj.GetDWCompList();
                ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            Session["PODetail"] = POModel;
            return View(POModel);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CretaePurchaseOrder(HttpPostedFileBase file, FormCollection fc)
        {
            int ErrCode = 0;
            string User_Id = User.Identity.GetUserId();
            CustomerPOModel POModel = new CustomerPOModel();
            EnquiryBL Enqbl = new EnquiryBL();
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");
            string fileName = "";
            POModel.DocumentFile = fc["DocumentFile"].ToString();
            POModel.DocumentFileOld = fc["DocumentFileOld"].ToString();
            if (file != null)
            {
                fileName = System.IO.Path.GetFileName(file.FileName);
                if (file.ContentLength > 0)
                {
                    try
                    {
                        string[] FileSplit = fileName.Split('.');
                        string FileEx = FileSplit[FileSplit.Length - 1].ToString();
                        String guid = Guid.NewGuid().ToString();
                        string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                        string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        string FileName = Convert.ToString("/SO/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
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
                                try
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
                                    requestFTPUploader = null;
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    if (i > 2)
                                    {
                                        Common.LogException(ex);
                                        break;
                                    }
                                }

                            }
                            POModel.DocumentFile = FileName;
                            if (POModel.DocumentFileOld != "")
                            {
                                for (int a = 0; a < 3; a++)
                                {
                                    try
                                    {
                                        FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(ftpServer + POModel.DocumentFileOld);
                                        requestFileDelete.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                        requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                                        FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                                        requestFileDelete = null;

                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        if (a > 2)
                                        {
                                            Common.LogException(ex);
                                            break;
                                        }
                                    }
                                }
                            }
                            POModel.DocumentFileOld = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.LogException(ex);
                    }
                }
            }
            try
            {
                if (POModel.DocumentFileOld != "")
                {
                    POModel.DocumentFile = POModel.DocumentFileOld;
                }
                POModel.PurchaseOrderId = Convert.ToInt32(fc["PurchaseOrderId"]);
                POModel.PurchaseOrderNumber = fc["PurchaseOrderNumber"].ToString();
                POModel.CustomerId = Convert.ToInt32(fc["CustomerId"]);
                POModel.Remark = fc["Remark"].ToString();
                POModel.CompCode = fc["CompCode"].ToString();
                POModel.PODate = Convert.ToDateTime(fc["PODate"]);
                POModel.Rating = Convert.ToInt32(fc["Rating"]);
                ErrCode = Enqbl.SaveCustomerPO(POModel, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CretaePurchaseOrder", new { PurchaseOrderId = ErrCode });
        }
        public ActionResult GetQuotationListByItem(int ItemId)
        {
            List<QuotationModel> QuotationLst = new List<QuotationModel>();
            EnquiryBL BLObj = new EnquiryBL();
            try
            {
                QuotationLst = BLObj.GetQuotationListByItem(ItemId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(QuotationLst, "QuotationId", "QuotationNumber"));
        }
        public JsonResult AddPODetailNewData(int itemId, string remark, double Quantity, DateTime ExpectedDate, string ItemName, int QuotationId, string QuotationName, int PurchaseOrderId, double Rate, int PurchaseDetailOrderId)
        {
            try
            {
                CustomerPOModel POModel = new CustomerPOModel();
                EnquiryBL BLObj = new EnquiryBL();
                string user_Id = User.Identity.GetUserId();
                if (Session["PODetail"] != null)
                {
                    POModel = Session["PODetail"] as CustomerPOModel;

                }

                if (POModel.PODetaillist == null)
                    POModel.PODetaillist = new List<CustomerPODetailModel>();
                if (POModel.PODetaillist.Count > 0)
                {
                    var actionToDelete = from actionRepDel in POModel.PODetaillist
                                         where actionRepDel.ItemId == itemId
                                         select actionRepDel;
                    if (actionToDelete.Count() > 0)
                        POModel.PODetaillist.Remove(actionToDelete.ElementAt(0));
                }
                CustomerPODetailModel POModelDetail = new CustomerPODetailModel();
                POModelDetail.PurchaseOrderId = PurchaseOrderId;
                POModelDetail.ItemId = itemId;
                POModelDetail.ItemName = ItemName;
                POModelDetail.Remark = remark;
                POModelDetail.Quantity = Quantity;
                POModelDetail.ExpectedDate = ExpectedDate;
                POModelDetail.ExpectedDateStr = ExpectedDate.ToShortDateString();
                POModelDetail.QuotationId = QuotationId;
                POModelDetail.QuotationName = QuotationName;
                POModelDetail.Rate = Rate;
                POModel.PODetaillist.Add(POModelDetail);
                POModel = BLObj.SaveEnquiryPODetail(POModel, user_Id, PurchaseDetailOrderId, PurchaseOrderId);
                Session["PODetail"] = POModel;
                var jsonResult = Json(POModel.PODetaillist, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult AddPODetailData(int itemId, string remark, double Quantity, DateTime ExpectedDate, string ItemName, int QuotationId, string QuotationName, int PurchaseOrderId, double Rate, int OldItemId, int PurchaseDetailOrderId)
        {
            try
            {
                CustomerPOModel POModel = new CustomerPOModel();
                EnquiryBL BLObj = new EnquiryBL();
                string user_Id = User.Identity.GetUserId();
                if (Session["PODetail"] != null)
                {
                    POModel = Session["PODetail"] as CustomerPOModel;

                }
                if (POModel.PODetaillist == null)
                    POModel.PODetaillist = new List<CustomerPODetailModel>();
                if (POModel.PODetaillist.Count > 0)
                {
                    var actionToDelete = from actionRepDel in POModel.PODetaillist
                                         where actionRepDel.PurchaseDetailOrderId == PurchaseDetailOrderId
                                         select actionRepDel;
                    if (actionToDelete.Count() > 0)
                        POModel.PODetaillist.Remove(actionToDelete.ElementAt(0));
                }
                CustomerPODetailModel POModelDetail = new CustomerPODetailModel();
                POModelDetail.PurchaseOrderId = PurchaseOrderId;
                POModelDetail.ItemId = itemId;
                POModelDetail.ItemName = ItemName;
                POModelDetail.Remark = remark;
                POModelDetail.Quantity = Quantity;
                POModelDetail.ExpectedDate = ExpectedDate;
                POModelDetail.ExpectedDateStr = ExpectedDate.ToShortDateString();
                POModelDetail.QuotationId = QuotationId;
                POModelDetail.QuotationName = QuotationName;
                POModelDetail.Rate = Rate;
                POModelDetail.PurchaseDetailOrderId = PurchaseDetailOrderId;
                POModel.PODetaillist.Add(POModelDetail);
                POModel = BLObj.SaveEnquiryPODetail(POModel, user_Id, PurchaseDetailOrderId, PurchaseOrderId);
                Session["PODetail"] = POModel;
                var jsonResult = Json(POModel.PODetaillist, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DeletePurchaseDetail(int PurchaseDetailOrderId)
        {
            int ErrCode = 0;
            CustomerPOModel POModel = new CustomerPOModel();
            try
            {

                EnquiryBL BLObj = new EnquiryBL();
                string user_Id = User.Identity.GetUserId();
                if (Session["PODetail"] != null)
                {
                    POModel = Session["PODetail"] as CustomerPOModel;

                }
                if (POModel.PODetaillist == null)
                    POModel.PODetaillist = new List<CustomerPODetailModel>();

                if (POModel.PODetaillist.Count > 0)
                {
                    var actionToDelete = from actionRepDel in POModel.PODetaillist
                                         where actionRepDel.PurchaseDetailOrderId == PurchaseDetailOrderId
                                         select actionRepDel;
                    if (actionToDelete.Count() > 0)
                        POModel.PODetaillist.Remove(actionToDelete.ElementAt(0));
                }
                ErrCode = BLObj.DeletePurchaseDetail(PurchaseDetailOrderId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            var jsonResult = Json(POModel.PODetaillist, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult Download(int PurchaseOrderId, string Filename)
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
                Common.LogException(e);
            }

            return RedirectToAction("CretaePurchaseOrder", new { PurchaseOrderId = PurchaseOrderId });
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
        public ActionResult ChkPOItemDup(int Itemid, int PurchaseOrderId)
        {
            CustomerPOModel POModel = new CustomerPOModel();
            if (Session["PODetail"] != null)
            {
                POModel = Session["PODetail"] as CustomerPOModel;
            }
            try
            {
                foreach (CustomerPODetailModel tempModel in POModel.PODetaillist)
                {
                    if (tempModel.PurchaseOrderId == PurchaseOrderId && tempModel.ItemId == Itemid)
                    {
                        return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult SendPOForApproval(int PurchaseOrderId)
        {
            int errCode = 0;
            try
            {
                EnquiryBL BLObj = new EnquiryBL();
                string User_Id = User.Identity.GetUserId();
                errCode = BLObj.SendPOForApproval(PurchaseOrderId, Convert.ToInt32(Utility.Enums.StatusCode.SendApp), User_Id);
                if (errCode == 500001)
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditSOOrder(int PurchaseOrderid, string OldSONo, string OldPONumber, DateTime PODate, string Remark)
        {
            SalesOrderModel SOModel = new SalesOrderModel();
            try
            {

                SOModel.SOId = OldSONo;
                SOModel.PurchaseOrderId = PurchaseOrderid;
                SOModel.PurchaseOrderNumber = OldPONumber;
                SOModel.PODate = PODate;
                SOModel.Remark = Remark;
                SOModel.OldSOId = OldSONo;
                SOModel.OldPurchaseOrderNumber = OldPONumber;
                SOModel.OldPODate = PODate;
                SOModel.OldRemark = Remark;
            }
            catch (Exception EX)
            {
                Common.LogException(EX);
            }
            return PartialView(SOModel);
        }
        [HttpPost]
        public ActionResult EditSOOrder(FormCollection fc)
        {
            SalesOrderModel SOModel = new SalesOrderModel();
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                SOModel.PurchaseOrderId = Convert.ToInt32(fc["PurchaseOrderId"].ToString());
                SOModel.OldSOId = fc["OldSOId"].ToString();
                SOModel.SOId = fc["SOId"].ToString();

                SOModel.OldPurchaseOrderNumber = fc["OldPurchaseOrderNumber"].ToString();
                SOModel.PurchaseOrderNumber = fc["PurchaseOrderNumber"].ToString();

                SOModel.OldPODate = Convert.ToDateTime(fc["OldPODate"].ToString());
                SOModel.PODate = Convert.ToDateTime(fc["PODate"].ToString());

                SOModel.OldRemark = fc["OldRemark"].ToString();
                SOModel.Remark = fc["Remark"].ToString();
                if (SOModel.OldSOId == SOModel.SOId && SOModel.OldPurchaseOrderNumber == SOModel.PurchaseOrderNumber && SOModel.OldPODate == SOModel.PODate && SOModel.Remark == SOModel.OldRemark)
                {
                    Session["infoSO"] = "Sales Order Number Succesfully Update.";
                }
                else
                {
                    int ErrCode = BLObj.EditSODetail(SOModel.PurchaseOrderId, SOModel.OldSOId, SOModel.SOId, User_Id, SOModel.PurchaseOrderNumber, SOModel.PODate, SOModel.Remark);
                    if (ErrCode == 600005)
                    {
                        Session["infoSO"] = "Sales Order Number Already Linked in  System.";
                    }
                    else if (ErrCode == 600002)
                    {
                        Session["infoSO"] = "Sales Order Number Succesfully Update.";
                    }
                }
            }
            catch (Exception EX)
            {
                Common.LogException(EX);
            }
            return RedirectToAction("CretaePurchaseOrder", new { PurchaseOrderId = SOModel.PurchaseOrderId });
        }
        #region Approval Purchase Order
        public ActionResult POApprovalList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/POApprovalList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                CustomerPOModel POModel = new CustomerPOModel();
                UserId = User.Identity.GetUserId();
                try
                {
                    if (Session["PODetail"] != null)
                    {
                        Session["PODetail"] = null;
                    }
                    EnquiryBL BLObj = new EnquiryBL();
                    POModel = BLObj.getCustPOList(User_Id, Convert.ToInt32(Utility.Enums.StatusCode.SendApp));

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(POModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportPOApprovalList")]
        [AcceptVerbs("POST")]
        public void ExportPOApprovalList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            CustomerPOModel POModel = new CustomerPOModel();
            POModel = BLObj.getCustPOList(User_Id, Convert.ToInt32(Utility.Enums.StatusCode.SendApp));
            var DataSource = POModel.POlist;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult POApproveReject(int PurchaseOrderId)
        {
            CustomerPOModel POModel = new CustomerPOModel();
            try
            {
                EnquiryBL Enqbl = new EnquiryBL();
                POModel = Enqbl.getSelectedPODetail(PurchaseOrderId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return PartialView(POModel);
        }
        public ActionResult SavePOApproveReject(int PurchaseOrderId, int StatusId)
        {
            try
            {
                EnquiryBL Enqbl = new EnquiryBL();
                int ErrCode = Enqbl.POApprovalReject(PurchaseOrderId, StatusId, User.Identity.GetUserId());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("POApprovalList");
        }
        #endregion Approval Purchase Order

        #region PO Link With Sales Order
        public ActionResult ApprovedPOList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/POApprovalList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                CustomerPOModel POModel = new CustomerPOModel();
                UserId = User.Identity.GetUserId();
                try
                {
                    EnquiryBL BLObj = new EnquiryBL();
                    POModel = BLObj.getCustPOList(User_Id, Convert.ToInt32(Utility.Enums.StatusCode.APPROVED));

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(POModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [System.Web.Http.ActionName("ExportApprovedPOList")]
        [AcceptVerbs("POST")]
        public void ExportApprovedPOList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            CustomerPOModel POModel = new CustomerPOModel();
            POModel = BLObj.getCustPOList(User_Id, Convert.ToInt32(Utility.Enums.StatusCode.APPROVED));
            var DataSource = POModel.POlist;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult ApprovedPODetail(int PurchaseOrderId)
        {
            CustomerPOModel POModel = new CustomerPOModel();
            List<SalesOrderModel> LstSO = new List<SalesOrderModel>();
            try
            {
                EnquiryBL Enqbl = new EnquiryBL();
                POModel = Enqbl.getSelectedPODetail(PurchaseOrderId);
                LstSO = Enqbl.GetSalesOrderList(POModel.CompCode, POModel.CustomerId);
                ViewBag.SalesOrderLst = new SelectList(LstSO, "SOId", "SOName");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return PartialView(POModel);
        }

        public ActionResult SaveSONumber(int PurchaseOrderId, string SONumber)
        {
            try
            {
                EnquiryBL Enqbl = new EnquiryBL();
                int ErrCode = Enqbl.SaveSONumber(PurchaseOrderId, SONumber, User.Identity.GetUserId());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("ApprovedPOList");
        }
        #endregion PO Link With Sales Order

        #endregion Purchase Order

        #region Recipt
        public ActionResult PurchaseReciptList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<ReceiptModel> LstReceipt = new List<ReceiptModel>();
            try
            {
                Session["ReceiptDetail"] = null;               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstReceipt);   
        }
        public ActionResult GetReceiptData()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<ReceiptModel> LstReceipt = new List<ReceiptModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                Session["ReceiptDetail"] = null;
                LstReceipt = BLObj.GetPurchaseReceiptList(User_Id);
                var jsonResult = Json(LstReceipt, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                jsonResult.RecursionLimit = int.MaxValue;
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
            EnquiryBL BLObj = new EnquiryBL();


            var DataSource = BLObj.GetPurchaseReceiptList(user_Id);
            Session["PreviewQuot"] = null;
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
        public ActionResult PurchaseCreateRecipt(int ReceiptId)
        {
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            ReceiptModel LstReceipt = new ReceiptModel();
            LstReceipt.LstReceiptModel = new List<ReceiptDetailModel>();
            LstReceipt.LstReceiptTaxModel = new List<ReceiptTaxModel>();
            List<CartonModel> LstCarronModel = new List<CartonModel>();
            ViewBag.LstCart = LstCarronModel;
            List<ReceiptModel> LstOtherCharges = new List<ReceiptModel>();
            ViewBag.LstOtherChages = LstOtherCharges;
            List<ReceiptTaxModel> LstTaxModel = new List<ReceiptTaxModel>();
            ViewBag.LsttaxList = LstTaxModel;
            List<InvoiceItemModel> LstItemModel = new List<InvoiceItemModel>();
            try
            {
                if (ReceiptId > 0)
                {

                    LstReceipt = BLObj.GetSelectedReceipt(ReceiptId);
                }
                else
                {
                    ItemBL BlObj = new ItemBL();
                    ItemMappingModel ItemMap = new ItemMappingModel();
                    ItemMap.lstItemMap = BlObj.GetDWCompList();
                    ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");

                    //List<Paytermsmodel> PTList = new List<Paytermsmodel>();
                    //BOMAdminBL objbl = new BOMAdminBL();
                    //PTList = objbl.GetPaymentTermsList();
                    //ViewBag.Paymentterms = new SelectList(PTList, "PTId", "PTCode");
                }

                LstItemModel = BLObj.GetItemsByReceiptIdFromMRI(ReceiptId);
                ViewBag.LstItems = new SelectList(LstItemModel, "ItemId", "ItemName"); ;
                BOMAdminBL taxbl = new BOMAdminBL();
                LstReceipt.TaxDrp = taxbl.TaxList(User_Id);
                ViewBag.TaxListDrp = new SelectList(LstReceipt.TaxDrp, "TaxId", "TaxName");

                List<FreightForwarderModel> lstf = new List<FreightForwarderModel>();
                VendorBL vendorbl = new VendorBL();
                lstf = vendorbl.FreightForvordarList();
                ViewBag.FFLst = new SelectList(lstf, "FFId", "VendorName");

                ContyStateCityModel addressModel = new ContyStateCityModel();
                AdminBL BLObjCOO = new AdminBL();
                addressModel = BLObjCOO.GetCountryStateCityList();
                List<AddCountryModel> cntrylst = addressModel.LstCountry;
                ViewBag.CountryLst = new SelectList(cntrylst, "CountryId", "CountryName");

                List<CurrencyCodes> lstModel = new List<CurrencyCodes>();
                TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                lstModel = ObjTMBL.GetCurrencyCodes();
                ViewBag.Currency = new SelectList(lstModel, "Key", "Value", LstReceipt.Currency);
                Session["ReceiptDetail"] = LstReceipt;

                List<CartonModel> CartonList = new List<CartonModel>();
                CartonList = BLObj.CartonList();
                ViewBag.Carton = new SelectList(CartonList, "Id", "Dimension");

                AdminBL AdminBLObj = new AdminBL();
                List<StockLocation> LststkLoc = new List<StockLocation>();
                LststkLoc = AdminBLObj.GetStockLocationListForDrp();
                ViewBag.LstStockLocation = new SelectList(LststkLoc, "STLocationId", "Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstReceipt);
        }
        [HttpPost]
        public ActionResult PurchaseCreateRecipt(FormCollection fc, HttpPostedFileBase file)
        {
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");
            string fileName = "";
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            ReceiptModel LstReceipt = new ReceiptModel();
            if (Session["ReceiptDetail"] != null)
            {
                LstReceipt = Session["ReceiptDetail"] as ReceiptModel;
            }
            if (Convert.ToInt32(fc["ReceiptId"]) > 0)
            {
                if (file != null && fc["ReceiptFile"].ToString() != "")
                {

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServer + fc["ReceiptFile"]);

                    request.Method = WebRequestMethods.Ftp.DeleteFile;
                    request.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                    request.Proxy = null;
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    response.Close();
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
                        string FileName = Convert.ToString("/PurchaseReceiptDoc/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
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
                                LstReceipt.ReceiptFile = FileName;
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
                LstReceipt.ReceiptFile = fc["ReceiptFile"].ToString();
            }


            try
            {
                LstReceipt.ReceiptId = Convert.ToInt32(fc["ReceiptId"].ToString());
                LstReceipt.ReceiptDate = Convert.ToDateTime(fc["ReceiptDate"].ToString());
                LstReceipt.CompCode = fc["CompCode"].ToString();
                LstReceipt.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                if (fc["FreightForwarderId"].ToString() != "")
                    LstReceipt.FreightForwarderId = Convert.ToInt32(fc["FreightForwarderId"].ToString());
                else
                {
                    LstReceipt.FreightForwarderId = 0;
                }
                LstReceipt.FFRef_No = fc["FFRef_No"].ToString();
                LstReceipt.SupplierInvN0 = fc["SupplierInvN0"].ToString();
                LstReceipt.SupplierInvDate = Convert.ToDateTime(fc["SupplierInvDate"].ToString());
                LstReceipt.PermitType = fc["PermitType"].ToString();
                LstReceipt.Currency = fc["Currency"].ToString();
                LstReceipt.Rating = Convert.ToInt32(fc["Rating"].ToString());
                LstReceipt.MRIId = Convert.ToInt32(fc["MRIId"].ToString());
                LstReceipt.Remark = fc["Remark"].ToString();
                string[] str = LstReceipt.Currency.Split(',');
                foreach (string str1 in str)
                {
                    if (str1.Length == 3)
                    {
                        LstReceipt.Currency = str1;
                        break;
                    }
                }
                int errCode = BLObj.SavePurchaseReceipt(LstReceipt, User_Id);
                return RedirectToAction("PurchaseCreateRecipt", new { ReceiptId = errCode });
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("PurchaseReciptList");
        }
        //public ActionResult SaveCartonDetail(int Recipt, double Height, double width, double lenght, double Weight, string Remark)
        //{
        //    try
        //    {
        //        EnquiryBL BLObj = new EnquiryBL();
        //    int Err = BLObj.SavereciptCarton(Recipt, Height, width, lenght, Weight, Remark);
        //        if (Err == 600002)
        //        {
        //            List<CartonModel> LstCarronModel = new List<CartonModel>();
        //            LstCarronModel = BLObj.GetSelectedCartonList(Recipt);
        //            return Json(LstCarronModel, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return Json("Success", JsonRequestBehavior.AllowGet);
        //}
        public ActionResult SaveCartonDetail(int ReceiptId, int CartonId, double Weight, string Remark)
        {
            try
            {
                EnquiryBL BLObj = new EnquiryBL();
                int Err = BLObj.SaveReceiptCarton(ReceiptId, CartonId, Weight, Remark);
                if (Err == 600002)
                {
                    List<CartonModel> LstCarronModel = new List<CartonModel>();
                    LstCarronModel = BLObj.GetSelectedCartonList(ReceiptId);
                    return Json(LstCarronModel, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCartonDetai(int Recipt)
        {
            try
            {
                EnquiryBL BLObj = new EnquiryBL();

                List<CartonModel> LstCarronModel = new List<CartonModel>();
                LstCarronModel = BLObj.GetSelectedCartonList(Recipt);
                return Json(LstCarronModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult DrpCarton(int Recipt)
        {
            try
            {
                EnquiryBL BLObj = new EnquiryBL();

                List<CartonModel> LstCarronModel = new List<CartonModel>();
                LstCarronModel = BLObj.DrpCartonList(Recipt);
                return Json(new SelectList(LstCarronModel, "CartonId", "Dimension"));
            }
            catch (Exception)
            {

                throw;
            }
        }
        public JsonResult EditPurchCarton(int CartId, double Weight, double Length, double Width, double Height, string Remark)
        {
            string User_Id = User.Identity.GetUserId();
            EnquiryBL BLObj = new EnquiryBL();
            int ErrCode = BLObj.EditCartonint(CartId, Weight, Length, Width, Height, Remark);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveOtherChargesPurReceipt(int ReceiptId, string ChargesDescription, string Currency, float Amount)
        {
            int errorCode = 0;
            EnquiryBL objBL = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            errorCode = objBL.SaveOtherChargesPurReceipt(ReceiptId, ChargesDescription, Currency, Amount);
            if (errorCode != 0)
                Session["ReceiptModel"] = null;
            return Json("Sucess", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetOtherChargesList(int ReceiptId)
        {

            List<ReceiptModel> LstOtherCharges = new List<ReceiptModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            LstOtherCharges = Enqbl.OtherCharges(ReceiptId);
            return Json(LstOtherCharges, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetVendorListByCompCode(string CompCode)
        {
            List<EnquiryBrandVemdorModel> model = new List<EnquiryBrandVemdorModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                model = Enqbl.GetVendorListByCompCode(CompCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "VendorId", "VendorName"));
        }
        public ActionResult GetVendorListByCompCodeFromMRI(string CompCode)
        {
            List<EnquiryBrandVemdorModel> model = new List<EnquiryBrandVemdorModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                model = Enqbl.GetVendorListByCompCodeFromMRI(CompCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "VendorId", "VendorName"));
        }
        public ActionResult GetIntimationListByVendor(string CompCode, int VendorId)
        {
            List<MDIModel> ModelList = new List<MDIModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                ModelList = Enqbl.GetIntimationListByVendor(CompCode, VendorId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(ModelList, "MRIId", "DocumentNo"));
        }
        public ActionResult GetPOByItemFromMRI(int MRIID, int ItemId)
        {
            List<InvoiceModel> model = new List<InvoiceModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                model = Enqbl.GetPObyItemFromMRI(MRIID, ItemId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "Invoice_Id", "InvoiceName"));
        }
        public ActionResult GetPOforReceiptByItem(string CompCode, int ItemId)
        {
            List<InvoiceModel> model = new List<InvoiceModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                model = Enqbl.GetPOforReceiptByItem(CompCode, ItemId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "Invoice_Id", "InvoiceName"));
        }
        public ActionResult GetItemListByReceipt(string CompCode, int VendorId)
        {
            List<InvoiceItemModel> model = new List<InvoiceItemModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                model = Enqbl.GetItemListByReceipt(CompCode, VendorId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "ItemId", "ItemName"));
        }
        public ActionResult SaveNewItemDetails(int Itemid, string PO_NO, double Quantity, int COO, int CartonId, double InvoiceAmount, string ItemName, string Country, int ReceiptId, int OldItemid, int OldCartonId, double OldQuantity, string Carton, int ReceiptDetailId)
        {
            ReceiptModel LstReceipt = new ReceiptModel();
            LstReceipt.LstReceiptModel = new List<ReceiptDetailModel>();
            string User_Id = User.Identity.GetUserId();
            if (Session["ReceiptDetail"] != null)
            {
                LstReceipt = Session["ReceiptDetail"] as ReceiptModel;
            }
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                if (OldItemid > 0)
                {
                    if (LstReceipt.LstReceiptModel.Count > 0)
                    {
                        var actionToDelete = from actionRepDel in LstReceipt.LstReceiptModel.Where(s => (s.ItemId == Itemid))
                                             where (actionRepDel.ReceiptId == ReceiptId) && (actionRepDel.ItemId == OldItemid) && (actionRepDel.Quantity == OldQuantity) && (actionRepDel.CartonId == OldCartonId)
                                             select actionRepDel;
                        if (actionToDelete.Count() > 0)
                            LstReceipt.LstReceiptModel.Remove(actionToDelete.ElementAt(0));
                    }
                }


                ReceiptDetailModel demoModel = new ReceiptDetailModel();
                demoModel.ItemId = Itemid;
                demoModel.ItemName = ItemName;
                demoModel.PO_No = PO_NO;
                demoModel.Quantity = Quantity;
                demoModel.CartonId = CartonId;
                demoModel.COO = COO;
                demoModel.Country = Country;
                demoModel.InvoiceAmount = InvoiceAmount;
                demoModel.ReceiptDetailId = ReceiptDetailId;
                if (CartonId > 0)
                {
                    demoModel.Dimension = Carton;
                }
                else
                {
                    demoModel.Dimension = "By Hand";
                }
                LstReceipt.LstReceiptModel.Add(demoModel);
                LstReceipt = Enqbl.SavePurchaseReceiptDetail(LstReceipt, User_Id, ReceiptDetailId, ReceiptId);
                BOMAdminBL taxbl = new BOMAdminBL();
                LstReceipt.TaxDrp = taxbl.TaxList(User_Id);
                Session["ReceiptDetail"] = LstReceipt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return Json(LstReceipt.LstReceiptModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveItemDetails(int Itemid, string PO_NO, double Quantity, int COO, int CartonId, double InvoiceAmount, int ReceiptId, int ReceiptDetailId)
        {
            ReceiptModel LstReceipt = new ReceiptModel();
            LstReceipt.LstReceiptModel = new List<ReceiptDetailModel>();
            string User_Id = User.Identity.GetUserId();
            if (Session["ReceiptDetail"] != null)
            {
                LstReceipt = Session["ReceiptDetail"] as ReceiptModel;
            }
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                if (ReceiptDetailId > 0)
                {
                    if (LstReceipt.LstReceiptModel.Count > 0)
                    {
                        var actionToDelete = from actionRepDel in LstReceipt.LstReceiptModel.Where(s => (s.ReceiptDetailId == ReceiptDetailId))
                                             where (actionRepDel.ReceiptDetailId == ReceiptDetailId)
                                             select actionRepDel;
                        if (actionToDelete.Count() > 0)
                            LstReceipt.LstReceiptModel.Remove(actionToDelete.ElementAt(0));
                    }
                }

                ReceiptDetailModel demoModel = new ReceiptDetailModel();
                demoModel.ItemId = Itemid;
                demoModel.PO_No = PO_NO;
                demoModel.Quantity = Quantity;
                demoModel.CartonId = CartonId;
                demoModel.COO = COO;
                demoModel.InvoiceAmount = InvoiceAmount;
                demoModel.ReceiptDetailId = ReceiptDetailId;
                LstReceipt.LstReceiptModel.Add(demoModel);
                LstReceipt = Enqbl.SavePurchaseReceiptDetail(LstReceipt, User_Id, ReceiptDetailId, ReceiptId);
                BOMAdminBL taxbl = new BOMAdminBL();
                LstReceipt.TaxDrp = taxbl.TaxList(User_Id);
                Session["ReceiptDetail"] = LstReceipt;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return Json(LstReceipt.LstReceiptModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemTaxDetail(int ReceiptDetailId)
        {
            List<ReceiptTaxModel> LstTaxModel = new List<ReceiptTaxModel>();
            ReceiptModel LstReceipt = new ReceiptModel();
            EnquiryBL Enqbl = new EnquiryBL();
            if (Session["ReceiptDetail"] != null)
            {
                LstReceipt = Session["ReceiptDetail"] as ReceiptModel;
            }
            try
            {
                //LstTaxModel = Enqbl.GetReceiptItemTaxDetail(ReceiptId, ItemId);
                foreach (ReceiptTaxModel tempModel in LstReceipt.LstReceiptTaxModel)
                {
                    if (tempModel.ReceiptDetailId == ReceiptDetailId)
                    {
                        LstTaxModel.Add(tempModel);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(LstTaxModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemTaxUpdateDetail(int ReceiptDetailId, int ReceiptId)
        {
            List<ReceiptTaxModel> LstTaxModel = new List<ReceiptTaxModel>();
            string User_Id = User.Identity.GetUserId();
            ReceiptModel LstReceipt = new ReceiptModel();
            EnquiryBL Enqbl = new EnquiryBL();
            LstReceipt = Enqbl.GetSelectedReceipt(ReceiptId);
            BOMAdminBL taxbl = new BOMAdminBL();
            LstReceipt.TaxDrp = taxbl.TaxList(User_Id);
            Session["ReceiptDetail"] = LstReceipt;
            try
            {
                //LstTaxModel = Enqbl.GetReceiptItemTaxDetail(ReceiptId, ItemId);
                foreach (ReceiptTaxModel tempModel in LstReceipt.LstReceiptTaxModel)
                {
                    if (tempModel.ReceiptDetailId == ReceiptDetailId)
                    {
                        LstTaxModel.Add(tempModel);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(LstTaxModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTaxDropDwn(int ReceiptDetailId)
        {
            List<TaxModel> taxontaxlist = new List<TaxModel>();
            List<ReceiptTaxModel> LstTaxModel = new List<ReceiptTaxModel>();
            ReceiptModel LstReceipt = new ReceiptModel();
            EnquiryBL Enqbl = new EnquiryBL();
            if (Session["ReceiptDetail"] != null)
            {
                LstReceipt = ((ReceiptModel)Session["ReceiptDetail"]);
                foreach (TaxModel t in LstReceipt.TaxDrp)
                {
                    taxontaxlist.Add(t);

                }
            }
            try
            {
                if (LstReceipt.LstReceiptTaxModel.Count > 0)
                {
                    //LstTaxModel = Enqbl.GetReceiptItemTaxDetail(ReceiptId, ItemId);
                    foreach (ReceiptTaxModel tempModel in LstReceipt.LstReceiptTaxModel.Where(s => (s.ReceiptDetailId == ReceiptDetailId)))
                    {
                        var actionToDelete = from actionRepDel in LstReceipt.TaxDrp.Where(s => (s.TaxId == tempModel.TaxId))
                                             where (actionRepDel.TaxId == tempModel.TaxId)
                                             select actionRepDel;
                        if (actionToDelete.Count() > 0)
                            taxontaxlist.Remove(actionToDelete.ElementAt(0));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(taxontaxlist, "TaxId", "TaxName"));
        }
        public ActionResult SaveItemTax(int TaxId, string TaxName, double TaxRate, double TaxAmount, int ReceiptDetailId, bool PaidByVendor)
        {
            try
            {
                ReceiptModel LstReceipt = new ReceiptModel();
                string User_Id = User.Identity.GetUserId();
                LstReceipt.LstReceiptModel = new List<ReceiptDetailModel>();
                if (Session["ReceiptDetail"] != null)
                {
                    LstReceipt = Session["ReceiptDetail"] as ReceiptModel;
                }
                if (LstReceipt.LstReceiptTaxModel == null)
                    LstReceipt.LstReceiptTaxModel = new List<ReceiptTaxModel>();
                if (LstReceipt.LstReceiptTaxModel.Count > 0)
                {
                    var actionToDelete = from actionRepDel in LstReceipt.LstReceiptTaxModel.Where(s => (s.ReceiptDetailId == ReceiptDetailId))
                                         where (actionRepDel.TaxId == TaxId) && (actionRepDel.ReceiptDetailId == ReceiptDetailId)
                                         select actionRepDel;
                    if (actionToDelete.Count() > 0)
                        LstReceipt.LstReceiptTaxModel.Remove(actionToDelete.ElementAt(0));
                }
                ReceiptTaxModel demoModel = new ReceiptTaxModel();
                demoModel.TaxId = TaxId;
                demoModel.TaxName = TaxName;
                demoModel.ReceiptDetailId = ReceiptDetailId;
                demoModel.TaxRate = TaxRate;
                demoModel.TaxAmount = TaxAmount;
                demoModel.PaidByVendor = PaidByVendor;
                LstReceipt.LstReceiptTaxModel.Add(demoModel);
                BOMAdminBL taxbl = new BOMAdminBL();
                LstReceipt.TaxDrp = taxbl.TaxList(User_Id);

                Session["ReceiptDetail"] = LstReceipt;
                return Json("Success", JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult GetTaxRate(int TaxId)
        {
            ReceiptModel LstReceipt = new ReceiptModel();
            List<TaxOnTax> LstTaxOnTaxModel = new List<TaxOnTax>();
            TaxModel temp1 = new TaxModel();
            EnquiryBL BLObj = new EnquiryBL();
            double Amount = 0.0;
            if (Session["ReceiptDetail"] != null)
            {
                LstReceipt = Session["ReceiptDetail"] as ReceiptModel;
            }
            foreach (TaxModel temp in LstReceipt.TaxDrp.Where(s => (s.TaxId == TaxId)))
            {
                temp1.TaxRate = temp.TaxRate;
                temp1.TaxType = temp.TaxType;
                break;
            }
            if (temp1.TaxType == "Tax On Tax")
            {
                LstTaxOnTaxModel = BLObj.GetTaxOnTaxList(TaxId);
                foreach (TaxOnTax temp in LstTaxOnTaxModel)
                {
                    foreach (ReceiptTaxModel tempTax in LstReceipt.LstReceiptTaxModel)
                    {
                        if (tempTax.TaxId == temp.TaxId)
                        {
                            Amount = tempTax.TaxAmount;
                        }
                    }
                }
            }
            temp1.TaxAmount = Amount;
            return Json(temp1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChkItemDup(int Itemid, int ReceiptId, int Quantity, int CartonId)
        {
            ReceiptModel LstReceipt = new ReceiptModel();
            if (Session["ReceiptDetail"] != null)
            {
                LstReceipt = Session["ReceiptDetail"] as ReceiptModel;
            }
            try
            {
                foreach (ReceiptDetailModel tempModel in LstReceipt.LstReceiptModel)
                {
                    if (tempModel.ReceiptId == ReceiptId && tempModel.ItemId == Itemid && tempModel.Quantity == Quantity && tempModel.CartonId == CartonId)
                    {
                        return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteReceiptDetail(int ReceiptDetailId)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            ReceiptModel LstReceipt = new ReceiptModel();
            LstReceipt.LstReceiptModel = new List<ReceiptDetailModel>();
            string User_Id = User.Identity.GetUserId();
            if (Session["ReceiptDetail"] != null)
            {
                LstReceipt = Session["ReceiptDetail"] as ReceiptModel;
            }
            try
            {
                if (ReceiptDetailId > 0)
                {
                    if (LstReceipt.LstReceiptModel.Count > 0)
                    {
                        var actionToDelete = from actionRepDel in LstReceipt.LstReceiptModel.Where(s => (s.ReceiptDetailId == ReceiptDetailId))
                                             where (actionRepDel.ReceiptDetailId == ReceiptDetailId)
                                             select actionRepDel;
                        if (actionToDelete.Count() > 0)
                            LstReceipt.LstReceiptModel.Remove(actionToDelete.ElementAt(0));
                    }
                }
                int ErrCode = Enqbl.DeleteReceiptDetail(ReceiptDetailId);
                if (ErrCode == 500002)
                {
                    Session["ReceiptDetail"] = LstReceipt;
                    return Json(LstReceipt.LstReceiptModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    LstReceipt = Session["ReceiptDetail"] as ReceiptModel;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(LstReceipt.LstReceiptModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteTaxDetail(int ReceiptDetailId, int TaxId)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                int errcode = Enqbl.DeleteReceiptDetailTax(ReceiptDetailId, TaxId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetItemDetailsList(int ReceiptDetailId)
        {
            ReceiptItemLocationDetail Model = new ReceiptItemLocationDetail();
            Model.ModelLst = new List<ReceiptItemLocationDetail>();
            EnquiryBL BLObj = new EnquiryBL();
            Model = BLObj.GetReceiptItemDetail(ReceiptDetailId);
            return Json(Model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckReceiptItemLocationDup(int ReceiptDetailId, int ItemId, int LocationId, int COOId, string DateCode)
        {
            EnquiryBL BLObj = new EnquiryBL();
            try
            {
                int ErrCode = BLObj.CheckReceiptItemLocationDup(ReceiptDetailId, ItemId, LocationId, COOId, DateCode);
                if (ErrCode == 0)
                    return Json("OK", JsonRequestBehavior.AllowGet);
                else
                    return Json("NOTOK", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SaveItemLocationDetails(int ItemDetId, int ItemId, int LocationId, int COOId, int Qty,string DateCode)
        {
            ReceiptItemLocationDetail Model = new ReceiptItemLocationDetail();
            Model.ModelLst = new List<ReceiptItemLocationDetail>();
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            int ErrCode = BLObj.SaveItemLocationDetails(ItemDetId, ItemId, LocationId, COOId, Qty, DateCode, User_Id);
            Model = BLObj.GetReceiptItemDetail(ItemDetId);
            return Json(Model, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteReceiptItemLocation(int ReceiptDetailId,int ReceiptItemDetailId)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            ReceiptItemLocationDetail Model = new ReceiptItemLocationDetail();
            Model.ModelLst = new List<ReceiptItemLocationDetail>();
            try
            {                
                int ErrCode = Enqbl.DeleteReceiptItemLocation(ReceiptItemDetailId);
                Model = Enqbl.GetReceiptItemDetail(ReceiptDetailId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(Model, JsonRequestBehavior.AllowGet);
        }
        #endregion Recipt

        #region Review Purchase receipt
        public ActionResult GetReviewPurchaseReceiptList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<ReceiptModel> LstReceipt = new List<ReceiptModel>();
            string User_Id = User.Identity.GetUserId();
            Session["ReviewReceiptDetail"] = null;
            try
            {
                LstReceipt = BLObj.GetReviewPurchaseReceiptList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(LstReceipt);
        }
        [System.Web.Http.ActionName("ExportReviewPurchaseReceiptList")]
        [AcceptVerbs("POST")]
        public void ExportReviewPurchaseReceiptList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            var DataSource = BLObj.GetReviewPurchaseReceiptList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult ReviewdPurchaseReceipt(int ReceiptId)
        {
            ReceiptModel LstReceipt = new ReceiptModel();
            LstReceipt.LstReceiptModel = new List<ReceiptDetailModel>();
            LstReceipt.LstReceiptTaxModel = new List<ReceiptTaxModel>();
            try
            {
                if (ReceiptId > 0)
                {
                    EnquiryBL BLObj = new EnquiryBL();
                    LstReceipt = BLObj.GetSelectedReceipt(ReceiptId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Session["ReviewReceiptDetail"] = LstReceipt;
            return View(LstReceipt);
        }
        public ActionResult GetReviewItemTaxDetail(int ReceiptDetailId)
        {
            List<ReceiptTaxModel> LstTaxModel = new List<ReceiptTaxModel>();
            ReceiptModel LstReceipt = new ReceiptModel();
            EnquiryBL Enqbl = new EnquiryBL();
            if (Session["ReviewReceiptDetail"] != null)
            {
                LstReceipt = Session["ReviewReceiptDetail"] as ReceiptModel;
            }
            try
            {
                //LstTaxModel = Enqbl.GetReceiptItemTaxDetail(ReceiptId, ItemId);
                foreach (ReceiptTaxModel tempModel in LstReceipt.LstReceiptTaxModel)
                {
                    if (tempModel.ReceiptDetailId == ReceiptDetailId)
                    {
                        LstTaxModel.Add(tempModel);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(LstTaxModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReviewRejectPurchaseReceipt(int StatusId, int ReceiptId, string Type,string ReviewedApprovedRejectRemark)
        {
            string User_Id = User.Identity.GetUserId();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                int errCode = Enqbl.ReviewRejectPurchaseReceipt(StatusId, ReceiptId, User_Id, ReviewedApprovedRejectRemark);
                if (errCode == 500002)
                {
                    Session["ReviewReceiptDetail"] = null;
                    if (Type == "Review")
                    {
                        return RedirectToAction("GetReviewPurchaseReceiptList");
                    }
                    else if (Type == "Process")
                    {
                        return RedirectToAction("GetProcessedPurchaseReceiptList");
                    }
                    else if (Type == "Receipt")
                    {
                        return RedirectToAction("PurchaseReciptList");
                    }
                    else
                    {
                        return RedirectToAction("GetApprovePurchaseReceiptList");
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetReviewPurchaseReceiptList");
        }
        #endregion Review Purchase receipt

        #region Approve Purchase Receipt
        public ActionResult GetApprovePurchaseReceiptList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<ReceiptModel> LstReceipt = new List<ReceiptModel>();
            string User_Id = User.Identity.GetUserId();
            Session["ReviewReceiptDetail"] = null;
            try
            {
                LstReceipt = BLObj.GetApprovePurchaseReceiptList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstReceipt);
        }
        [System.Web.Http.ActionName("ExportApprovePurchaseReceiptList")]
        [AcceptVerbs("POST")]
        public void ExportApprovePurchaseReceiptList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            var DataSource = BLObj.GetApprovePurchaseReceiptList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult GetProcessedPurchaseReceiptList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<ReceiptModel> LstReceipt = new List<ReceiptModel>();
            string User_Id = User.Identity.GetUserId();
            Session["ReviewReceiptDetail"] = null;
            try
            {
                LstReceipt = BLObj.GetPurchaseReceiptListForProcess(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstReceipt);
        }
        [System.Web.Http.ActionName("ExportProcessedPurchaseReceiptList")]
        [AcceptVerbs("POST")]
        public void ExportProcessedPurchaseReceiptList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            var DataSource = BLObj.GetPurchaseReceiptListForProcess(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult ProcessPurchaseReceipt(int ReceiptId)
        {
            ReceiptModel LstReceipt = new ReceiptModel();
            LstReceipt.LstReceiptModel = new List<ReceiptDetailModel>();
            LstReceipt.LstReceiptTaxModel = new List<ReceiptTaxModel>();
            try
            {
                if (ReceiptId > 0)
                {
                    EnquiryBL BLObj = new EnquiryBL();
                    LstReceipt = BLObj.GetSelectedReceipt(ReceiptId);
                    List<AirwaysReceiptModel> Model = new List<AirwaysReceiptModel>();
                    Model = BLObj.GetPurchaseInvForReceiptByVendor(LstReceipt.CompCode, LstReceipt.VendorId);
                    ViewBag.PurchInvList = new SelectList(Model, "FFRef_No", "FFRef_No");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Session["ReviewReceiptDetail"] = LstReceipt;
            return View(LstReceipt);
        }
        public ActionResult SaveProcessPurchaseReceipt(int StatusId, int ReceiptId, string PurcInv)
        {
            int ErrCode = 0;
            string User_Id = User.Identity.GetUserId();
            try
            {
                EnquiryBL BLObj = new EnquiryBL();
                ErrCode = BLObj.SaveProcessPurchaseReceipt(StatusId, ReceiptId, PurcInv, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetProcessedPurchaseReceiptList");
        }
        public ActionResult ApprovedPurchaseReceipt(int ReceiptId)
        {
            ReceiptModel LstReceipt = new ReceiptModel();
            LstReceipt.LstReceiptModel = new List<ReceiptDetailModel>();
            LstReceipt.LstReceiptTaxModel = new List<ReceiptTaxModel>();
            try
            {
                if (ReceiptId > 0)
                {
                    EnquiryBL BLObj = new EnquiryBL();
                    LstReceipt = BLObj.GetSelectedReceipt(ReceiptId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Session["ReviewReceiptDetail"] = LstReceipt;
            return View(LstReceipt);
        }
        #endregion Approve Purchase Receipt

        #region Dispatched
        public ActionResult DispatchList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<DispatchModel> LstDispatch = new List<DispatchModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                Session["ReceiptDetail"] = null;
                LstDispatch = BLObj.GetDispatchList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstDispatch);
        }
        [System.Web.Http.ActionName("ExportDispatchDetailList")]
        [AcceptVerbs("POST")]
        public void ExportDispatchDetailList(string GridModel)
        {
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            List<DispatchModel> LstDispatch = new List<DispatchModel>();
            LstDispatch = BLObj.GetDispatchList(User_Id);
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = LstDispatch;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateDispatch(int DispatchId)
        {
            DispatchModel DispatchModel = new DispatchModel();
            DispatchModel.LstDispatchDetail = new List<DispatchDetailModel>();
            try
            {
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
            return View(DispatchModel);
        }
        [HttpPost]
        public ActionResult CreateDispatch(FormCollection fc)
        {
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            DispatchModel DispatchModel = new DispatchModel();
            DispatchModel.LstDispatchDetail = new List<DispatchDetailModel>();
            if (Session["DispatchDetail"] != null)
            {
                DispatchModel = Session["DispatchDetail"] as DispatchModel;
            }
            try
            {


                string[] str = fc["DispatchId"].ToString().Split(',');
                foreach (string str1 in str)
                {
                    DispatchModel.DispatchId = Convert.ToInt32(str1);
                    break;
                }
                DispatchModel.CompCode = fc["CompCode"].ToString();
                DispatchModel.AirwayBillNo = fc["AirwayBillNo"].ToString();
                DispatchModel.ExportPermitNo = fc["ExportPermitNo"].ToString();
                DispatchModel.Invoice_No = fc["Invoice_No"].ToString();
                DispatchModel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                // DispatchModel.COO = Convert.ToInt32(fc["COO"].ToString());
                if (fc.AllKeys.Contains("FreightForwarderId"))
                    if (fc["FreightForwarderId"].ToString() != "")
                        DispatchModel.FreightForwarderId = Convert.ToInt32(fc["FreightForwarderId"].ToString());
                DispatchModel.DispatchDate = Convert.ToDateTime(fc["DispatchDate"].ToString());
                int errCode = BLObj.SaveDispatch(DispatchModel, User_Id);
                return RedirectToAction("CreateDispatch", new { DispatchId = errCode });
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("DispatchList");
        }
        public JsonResult DispatchSendForProcess(int DispatchId)
        {
            int ErrCode = 0;
            EnquiryBL BLObj = new EnquiryBL();
            DispatchModel DispatchModel = new DispatchModel();
            DispatchModel.LstDispatchDetail = new List<DispatchDetailModel>();
            string user_Id = User.Identity.GetUserId();
            try
            {
                if (Session["DispatchDetail"] != null)
                {
                    DispatchModel = Session["DispatchDetail"] as DispatchModel;

                }
                if (DispatchModel.LstDispatchDetail.Count != null)
                {

                    ErrCode = BLObj.DispatchSendForProcess(DispatchId, Convert.ToInt32(SmartSys.Utility.Enums.StatusCode.DispSndPro), user_Id);
                    if (ErrCode == 600001 || ErrCode == 600002)
                    {
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }

                    else
                    {
                        return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DispatchApprovalList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<DispatchModel> LstDispatch = new List<DispatchModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                LstDispatch = BLObj.GetDispatchListForApproval(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstDispatch);

        }
        [System.Web.Http.ActionName("ExportDispatchApprovalList")]
        [AcceptVerbs("POST")]
        public void ExportDispatchApprovalList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            var DataSource = BLObj.GetDispatchListForApproval(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult ApproveRejectDispatch(int DispatchId)
        {
            DispatchModel model = new DispatchModel();
            EnquiryBL BLObj = new EnquiryBL();
            model = BLObj.GetSelectedDispatch(DispatchId);
            return View(model);
        }
        public ActionResult UpdateApprovalDetail(int DispatchId, int Status)
        {
            EnquiryBL bl = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            int errorcode = bl.UpdateDispatchApproval(Status, User_Id, DispatchId);
            return RedirectToAction("DispatchApprovalList");
        }
        public ActionResult CustomerListByCompCode(string CompCode)
        {
            List<CustomerDrpModel> model = new List<CustomerDrpModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                model = Enqbl.CustomerListByDispatchCompCode(CompCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "CustomerId", "CustomerName"));
        }
        public ActionResult InvoiceListByCustomerId(string CompCode, int CustomerId)
        {
            List<InvoiceModel> model = new List<InvoiceModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                model = Enqbl.InvoiceListByDispatchCustomerId(CompCode, CustomerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "Invoice_Id", "InvoiceName"));
        }
        public ActionResult GetItemListBySalInvoiceId(string CompCode, string Invoice_Id)
        {
            List<InvoiceItemModel> model = new List<InvoiceItemModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                model = Enqbl.GetItemListBySalInvoiceId(CompCode, Invoice_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "ItemId", "ItemName"));
        }
        public ActionResult GetItemRateByInvoiceId(string CompCode, string Invoice_Id, int ItemId, int CustomerId)
        {
            DispatchDetailModel DispatchdetailModel = new DispatchDetailModel();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                DispatchdetailModel = Enqbl.GetItemRateByInvoiceId(CompCode, Invoice_Id, ItemId, CustomerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(DispatchdetailModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChkSalInvoceDetailDup(int Itemid, int DispatchId, int CartonId, string Invoice_No)
        {
            DispatchModel DispatchModel = new DispatchModel();
            DispatchModel.LstDispatchDetail = new List<DispatchDetailModel>();
            if (Session["DispatchDetail"] != null)
            {
                DispatchModel = Session["DispatchDetail"] as DispatchModel;
            }
            try
            {
                foreach (DispatchDetailModel tempModel in DispatchModel.LstDispatchDetail)
                {
                    if (tempModel.DispatchId == DispatchId && tempModel.ItemId == Itemid && tempModel.CartonId == CartonId)
                    {
                        return Json("Error", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveNewDispatchDetails(int CartonId, int Itemid, string Invoice_No, double Quantity, double Rate, string ItemName, int DispatchId)
        {
            DispatchModel DispatchModel = new DispatchModel();
            DispatchModel.LstDispatchDetail = new List<DispatchDetailModel>();
            if (Session["DispatchDetail"] != null)
            {
                DispatchModel = Session["DispatchDetail"] as DispatchModel;
            }
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                //if (DispatchModel.LstDispatchDetail.Count > 0)
                //{
                //    var actionToDelete = from actionRepDel in DispatchModel.LstDispatchDetail.Where(s => (s.ItemId == Itemid))
                //                         where (actionRepDel.ItemId == Itemid)
                //                         select actionRepDel;
                //    if (actionToDelete.Count() > 0)
                //        DispatchModel.LstDispatchDetail.Remove(actionToDelete.ElementAt(0));
                //}
                DispatchDetailModel demoModel = new DispatchDetailModel();
                demoModel.DispatchDetailId = 0;
                demoModel.DispatchId = DispatchId;
                demoModel.CartonId = CartonId;
                demoModel.ItemId = Itemid;
                demoModel.ItemName = ItemName;
                demoModel.Invoice_No = Invoice_No;
                demoModel.Quantity = Quantity;
                demoModel.Rate = Rate;
                DispatchModel = Enqbl.SaveDispatchDetail(demoModel, User.Identity.GetUserId(), DispatchId);
                Session["DispatchDetail"] = DispatchModel;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return Json(DispatchModel.LstDispatchDetail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveDispatchDetails(int DispatchDetailId, int CartonId, int Itemid, string Invoice_No, double Quantity, double Rate, string ItemName, int DispatchId)
        {
            DispatchModel DispatchModel = new DispatchModel();
            DispatchModel.LstDispatchDetail = new List<DispatchDetailModel>();
            if (Session["DispatchDetail"] != null)
            {
                DispatchModel = Session["DispatchDetail"] as DispatchModel;
            }
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                if (DispatchModel.LstDispatchDetail.Count > 0)
                {
                    var actionToDelete = from actionRepDel in DispatchModel.LstDispatchDetail.Where(s => (s.ItemId == Itemid))
                                         where (actionRepDel.DispatchDetailId == DispatchDetailId)
                                         select actionRepDel;
                    if (actionToDelete.Count() > 0)
                        DispatchModel.LstDispatchDetail.Remove(actionToDelete.ElementAt(0));
                }
                DispatchDetailModel demoModel = new DispatchDetailModel();
                demoModel.DispatchDetailId = DispatchDetailId;
                demoModel.DispatchId = DispatchId;
                demoModel.CartonId = CartonId;
                demoModel.ItemId = Itemid;
                demoModel.ItemName = ItemName;
                demoModel.Invoice_No = Invoice_No;
                demoModel.Quantity = Quantity;
                demoModel.Rate = Rate;
                DispatchModel = Enqbl.SaveDispatchDetail(demoModel, User.Identity.GetUserId(), DispatchId);
                Session["DispatchDetail"] = DispatchModel;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return Json(DispatchModel.LstDispatchDetail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteDispatchDetail(int DispatchDetailId)
        {
            EnquiryBL Enqbl = new EnquiryBL();
            DispatchModel DispatchModel = new DispatchModel();
            DispatchModel.LstDispatchDetail = new List<DispatchDetailModel>();
            if (Session["DispatchDetail"] != null)
            {
                DispatchModel = Session["DispatchDetail"] as DispatchModel;
            }
            try
            {
                if (DispatchDetailId > 0)
                {
                    if (DispatchModel.LstDispatchDetail.Count > 0)
                    {
                        var actionToDelete = from actionRepDel in DispatchModel.LstDispatchDetail.Where(s => (s.DispatchDetailId == DispatchDetailId))
                                             where (actionRepDel.DispatchDetailId == DispatchDetailId)
                                             select actionRepDel;
                        if (actionToDelete.Count() > 0)
                            DispatchModel.LstDispatchDetail.Remove(actionToDelete.ElementAt(0));
                    }
                }
                int ErrCode = Enqbl.DeleteDispatchDetail(DispatchDetailId);
                if (ErrCode == 500002)
                {
                    Session["DispatchDetail"] = DispatchModel;
                    return Json(DispatchModel.LstDispatchDetail, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    DispatchModel = Session["ReceiptDetail"] as DispatchModel;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(DispatchModel.LstDispatchDetail, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteDispatch(int DispatchId)
        {
            try
            {
                EnquiryBL EnqBL = new EnquiryBL();
                int errCode = EnqBL.DeleteDispatch(DispatchId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("DispatchList");
        }
        #region Dispatch Carton 
        public ActionResult SaveDispatchCartonDetail(int DispatchId, int CartonId, double Weight, string Remark)
        {
            try
            {
                EnquiryBL BLObj = new EnquiryBL();
                int Err = BLObj.SaveDispatchCarton(DispatchId, CartonId, Weight, Remark);
                if (Err == 600002)
                {
                    List<CartonModel> LstCarronModel = new List<CartonModel>();
                    LstCarronModel = BLObj.GetSelectedDispatchCartonList(DispatchId);
                    return Json(LstCarronModel, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteDispatchCart(int DispatchId, int CartonId)
        {
            try
            {
                EnquiryBL BLObj = new EnquiryBL();
                int Err = BLObj.DeleteDispatchCart(DispatchId, CartonId);
                if (Err == 600002)
                {
                    List<CartonModel> LstCarronModel = new List<CartonModel>();
                    LstCarronModel = BLObj.GetSelectedDispatchCartonList(DispatchId);
                    return Json(LstCarronModel, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetDispatchCartonDetai(int DispatchId)
        {
            try
            {
                EnquiryBL BLObj = new EnquiryBL();

                List<CartonModel> LstCarronModel = new List<CartonModel>();
                LstCarronModel = BLObj.GetSelectedDispatchCartonList(DispatchId);
                return Json(LstCarronModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult DrpDispatchCarton(int DispatchId)
        {
            try
            {
                EnquiryBL BLObj = new EnquiryBL();

                List<CartonModel> LstCarronModel = new List<CartonModel>();
                LstCarronModel = BLObj.DrpDispatchCartonList(DispatchId);
                return Json(new SelectList(LstCarronModel, "CartonId", "Dimension"));
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion Dispatch Carton

        #region Dispatch Document
        public ActionResult DispatchDocument(int DiapatchId)
        {
            DispatchDocument DispatchDoc = new DispatchDocument();
            try
            {
                DispatchDoc.DispatchId = DiapatchId;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return PartialView(DispatchDoc);
        }
        [HttpPost]
        public ActionResult DispatchDocument(FormCollection fc, HttpPostedFileBase file)
        {
            DispatchDocument DispatchDoc = new DispatchDocument();
            try
            {
                string fileName = Path.GetFileName(file.FileName);
                string ftpServer = Common.GetConfigValue("FTP");
                string[] FileSplit = fileName.Split('.');
                string FileEx = FileSplit[1].ToString();
                String guid = Guid.NewGuid().ToString();
                string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                string FileName = Convert.ToString("/DispatchDoc/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;

                DispatchDoc.DispatchId = Convert.ToInt32(fc["DispatchId"].ToString());
                DispatchDoc.DocumentTitle = fc["DocumentTitle"].ToString();
                DispatchDoc.FilePath = FileName;
                string localPath = Path.Combine(Server.MapPath("~//App_Data/uploads"), fileName);
                file.SaveAs(localPath);
                if (ftpServer.Trim().Length > 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            string ftpUid = Common.GetConfigValue("FTPUid");
                            string ftpPwd = Common.GetConfigValue("FTPPWD");
                            FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                            requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                            requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                            FileInfo fileInfo = new FileInfo(localPath);
                            FileStream fileStream = fileInfo.OpenRead();
                            int bufferLength = 2048;
                            byte[] buffer = new byte[bufferLength];
                            Stream uploadStream = requestFTPUploader.GetRequestStream();
                            int contentLength = fileStream.Read(buffer, 0, bufferLength);
                            while (contentLength != 0)
                            {
                                uploadStream.Write(buffer, 0, contentLength);
                                contentLength = fileStream.Read(buffer, 0, bufferLength);
                            }
                            uploadStream.Close();
                            fileStream.Close();
                            requestFTPUploader = null;
                            EnquiryBL BLobj = new EnquiryBL();
                            string UserId = User.Identity.GetUserId();
                            int ErrCode = BLobj.SaveDispatchDoc(DispatchDoc.DispatchId, DispatchDoc.DocumentTitle, DispatchDoc.FilePath, UserId);
                            if (ErrCode == 500002)
                            {
                                return RedirectToAction("CreateDispatch", new { DispatchId = DispatchDoc.DispatchId });
                            }
                            else
                            {
                                for (int x = 0; x < 3; x++)
                                {
                                    try
                                    {
                                        FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                                        requestFileDelete.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                        requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                                        FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                                        requestFileDelete = null;
                                        Session["DisDocError"] = "This  Document Title  Already Created or Some Error Occurd in Save Please try again..";
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        if (i > 2)
                                        {
                                            throw ex;
                                        }
                                    }
                                }
                                return RedirectToAction("CreateDispatch", new { DispatchId = DispatchDoc.DispatchId });
                            }
                        }
                        catch (Exception ex)
                        {
                            Common.LogException(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("CreateDispatch", new { DispatchId = DispatchDoc.DispatchId });
        }

        public ActionResult dispatchDocDownload(int DispatchId, string DocumentPath)
        {
            try
            {
                String FTPServer = Common.GetConfigValue("FTP");
                String FTPUserId = Common.GetConfigValue("FTPUid");
                String FTPPwd = Common.GetConfigValue("FTPPWD");

                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + DocumentPath);
                requestFileDownload.UseBinary = true;
                requestFileDownload.KeepAlive = false;
                requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                byte[] OPData = ReadFully(responseStream);
                responseStream.Close();

                Response.AddHeader("content-disposition", "attachment; filename=" + DocumentPath);
                Response.ContentType = "application/zip";
                Response.BinaryWrite(OPData);
                Response.End();
                return RedirectToAction("CreateDispatch", new { DispatchId = DispatchId });
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return RedirectToAction("CreateDispatch", new { DispatchId = DispatchId });
            }

        }
        #endregion Dispatch Document
        #endregion Dispatched

        #region FFCharges
        public ActionResult FFChargesList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<ReceiptModel> FFCharges = new List<ReceiptModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {

                FFCharges = BLObj.GetFFChargesList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(FFCharges);
        }
        [System.Web.Http.ActionName("ExportFFChargesLst")]
        [AcceptVerbs("POST")]
        public void ExportFFChargesLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            var DataSource = BLObj.GetFFChargesList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateFFCharges(int RID)
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<ReceiptModel> FFCharges = new List<ReceiptModel>();
            ReceiptModel model = new ReceiptModel();
            model.ReceiptId = RID;
            try
            {

                List<FreightForwarderModel> lstf = new List<FreightForwarderModel>();
                VendorBL vendorbl = new VendorBL();
                if (RID > 0)
                {
                    model = BLObj.getSelectedFFCharges(RID);
                }
                lstf = vendorbl.FreightForvordarList();

                ViewBag.FFDrp = new SelectList(lstf, "FFId", "VendorName");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateFFCharges(FormCollection fc)
        {
            EnquiryBL BLObj = new EnquiryBL();

            ReceiptModel model = new ReceiptModel();
            string User_Id = User.Identity.GetUserId();

            try
            {
                model.ReceiptId = Convert.ToInt32(fc["ReceiptId"]);
                model.FreightInvNo = fc["FreightInvNo"];
                model.FreightInvDate = Convert.ToDateTime(fc["FreightInvDate"]);
                model.FrieghtInvReceivedDate = Convert.ToDateTime(fc["FrieghtInvReceivedDate"]);
                model.Amount = Convert.ToDouble(fc["Amount"]);
                int errorcode = BLObj.SaveFFCharges(model, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("FFChargesList");
        }
        public JsonResult GetAirWaysRecieptList(int FFId)
        {
            List<AirwaysReceiptModel> lst = new List<AirwaysReceiptModel>();
            EnquiryBL BLObj = new EnquiryBL();
            lst = BLObj.GetAirwaysListForDrp(FFId);
            return Json(new SelectList(lst, "ReceiptId", "FFRef_No"));
        }

        #endregion FFCharges

        #region Tracking Status Config
        public ActionResult TrackingStatusConfigList()
        {
            List<TrackingStatusConfigModel> LstConfigModel = new List<TrackingStatusConfigModel>();
            try
            {
                EnquiryBL EnqBL = new EnquiryBL();
                LstConfigModel = EnqBL.StatusConfigDetailList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstConfigModel);
        }
        [System.Web.Http.ActionName("ExportTrackingStatusConfigLst")]
        [AcceptVerbs("POST")]
        public void ExportTrackingStatusConfigLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL EnqBL = new EnquiryBL();
            var DataSource = EnqBL.StatusConfigDetailList();
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        [HttpPost]
        public string SaveStatusConfig(TrackingStatusConfigModel Model)
        {
            string str = "Error";
            try
            {
                string User_Id = User.Identity.GetUserId();
                EnquiryBL EnqBL = new EnquiryBL();
                int errCode = EnqBL.UpdateStatusConfig(Model, User_Id);
                if (errCode == 500001)
                {
                    str = "Success";
                    return str;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str;
        }
        #endregion Tracking Status Config

        #region InvoiceDetails


        public ActionResult InvoiceDetailList()
        {

            EnquiryBL BLObj = new EnquiryBL();
            List<InvoiceDetailModel> InvList = new List<InvoiceDetailModel>();
            string User_Id = User.Identity.GetUserId();
            InvList = BLObj.GetInvoicedetail(User_Id);
            return View(InvList);
        }
        [System.Web.Http.ActionName("ExportInvoiceDetailList")]
        [AcceptVerbs("POST")]
        public void ExportInvoiceDetailList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            var DataSource = BLObj.GetInvoicedetail(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateInvoiceDetail(int InvId)
        {
            ItemBL BlObj = new ItemBL();
            EnquiryBL BL = new EnquiryBL();
            InvoiceDetailModel model = new InvoiceDetailModel();
            model.Status = "New";
            ItemMappingModel ItemMap = new ItemMappingModel();
            if (InvId > 0)
            {
                model = BL.GetselectedInvoiceDetail(InvId);
            }

            ItemMap.lstItemMap = BlObj.GetDWCompList();
            ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateInvoiceDetail(FormCollection fc, HttpPostedFileBase file)
        {
            InvoiceDetailModel model = new InvoiceDetailModel();
            EnquiryBL BLObj = new EnquiryBL();
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");
            string fileName = "";
            string User_Id = User.Identity.GetUserId();
            if (Convert.ToInt32(fc["InvId"]) > 0)
            {
                if (file != null && fc["InvFile"].ToString() != "")
                {

                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServer + fc["InvFile"]);

                    request.Method = WebRequestMethods.Ftp.DeleteFile;
                    request.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                    request.Proxy = null;
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    response.Close();
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
                        string FileName = Convert.ToString("/Invoice/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
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
                                model.InvFile = FileName;
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
                model.InvFile = fc["InvFile"].ToString();
            }

            model.InvId = Convert.ToInt32(fc["InvId"]);
            model.CompCode = fc["CompCode"].ToString();
            model.CustomerId = Convert.ToInt32(fc["CustomerId"]);
            model.InvRefNo = fc["InvRefNo"].ToString();

            int Errorcode = BLObj.SaveInvoiceDetail(model, User_Id);
            return RedirectToAction("InvoiceDetailList");
        }
        public ActionResult InvoiceDownLoad(string Filename)
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
                Common.LogException(e);
            }

            return RedirectToAction("InvoiceDetailList");
        }
        public ActionResult CustomerListByInvoiceCompCode(string CompCode)
        {
            List<CustomerDrpModel> model = new List<CustomerDrpModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                model = Enqbl.CustomerListByCompCode(CompCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "CustomerId", "CustomerName"));
        }
        public ActionResult InvoiceListBYCustomer(string CompCode, int CustomerId)
        {
            List<InvoiceModel> model = new List<InvoiceModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                model = Enqbl.InvoiceListByCustomerId(CompCode, CustomerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "Invoice_Id", "InvoiceName"));
        }
        #endregion InvoiceDetails

        #region Carton
        public ActionResult CartonList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<CartonModel> CartonList = new List<CartonModel>();
            try
            {
                CartonList = BLObj.CartonList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(CartonList);
        }
        [System.Web.Http.ActionName("ExportCartonList")]
        [AcceptVerbs("POST")]
        public void ExportCartonList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            var DataSource = BLObj.CartonList();
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public JsonResult SaveCarton(CartonModel Model)
        {
            string User_Id = User.Identity.GetUserId();
            EnquiryBL BLObj = new EnquiryBL();
            int ErrCode = BLObj.SaveCarton(Model, User_Id);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        #endregion Carton

        #region Freight Forworder  Invoice
        public ActionResult GetFreightFwdInvoiceList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<FreightFwdInvoiceModel> LstFFInvoice = new List<FreightFwdInvoiceModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                Session["FFinvoiceDetail"] = null;
                LstFFInvoice = BLObj.GetFreightFwdInvoiceList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstFFInvoice);
        }
        [System.Web.Http.ActionName("ExportFreightFwdInvoiceList")]
        [AcceptVerbs("POST")]
        public void ExportFreightFwdInvoiceList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            var DataSource = BLObj.GetFreightFwdInvoiceList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateFreightFwdInvoice(int FreightInvId)
        {
            FreightFwdInvoiceModel FFModel = new FreightFwdInvoiceModel();
            FFModel.LstDetal = new List<FreightFwdInvoiceDetailModel>();
            FFModel.LstTax = new List<FreightFwdInvoiceTaxesModel>();
            EnquiryBL BLObj = new EnquiryBL();
          
            try
            {
                if (FreightInvId > 0)
                {
                    FFModel = BLObj.GetSelectedFFInvoice(FreightInvId);
                }
                List<FreightForwarderModel> lstf = new List<FreightForwarderModel>();
                VendorBL vendorbl = new VendorBL();
                lstf = vendorbl.FreightForvordarList();
                ViewBag.FFLst = new SelectList(lstf, "FFId", "VendorName");

                string User_Id = User.Identity.GetUserId();
                FFModel.TaxDrp = new List<TaxModel>();
                BOMAdminBL taxbl = new BOMAdminBL();
                FFModel.TaxDrp = taxbl.TaxList(User_Id);
                ViewBag.TaxListDrp = new SelectList(FFModel.TaxDrp, "TaxId", "TaxName");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Session["FFinvoiceDetail"] = FFModel;
            return View(FFModel);
        }
        [HttpPost]
        public ActionResult CreateFreightFwdInvoice(FormCollection fc)
        {
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            FreightFwdInvoiceModel LstModel = new FreightFwdInvoiceModel();
            try
            {
                LstModel.FreightInvId = Convert.ToInt32(fc["FreightInvId"].ToString());
                LstModel.InvoiceDate = Convert.ToDateTime(fc["InvoiceDate"].ToString());
                LstModel.InvoiceNumber = fc["InvoiceNumber"].ToString();
                LstModel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                LstModel.Remark = fc["Remark"].ToString();
                int errCode = BLObj.SaveFFInvoice(LstModel, User_Id);
                return RedirectToAction("CreateFreightFwdInvoice", new { FreightInvId = errCode });
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("GetFreightFwdInvoiceList");
        }
        public ActionResult FFInvoiceGetItemTaxDetail(int FreightInvDetailId)
        {
            List<FreightFwdInvoiceTaxesModel> LstTaxModel = new List<FreightFwdInvoiceTaxesModel>();
            FreightFwdInvoiceModel FFModel = new FreightFwdInvoiceModel();
            EnquiryBL Enqbl = new EnquiryBL();
            if (Session["FFinvoiceDetail"] != null)
            {
                FFModel = Session["FFinvoiceDetail"] as FreightFwdInvoiceModel;
            }
            try
            {
                //LstTaxModel = Enqbl.GetReceiptItemTaxDetail(ReceiptId, ItemId);
                foreach (FreightFwdInvoiceTaxesModel tempModel in FFModel.LstTax)
                {
                    if (tempModel.FreightInvDetailId == FreightInvDetailId)
                    {
                        LstTaxModel.Add(tempModel);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(LstTaxModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FFInvoiceGetTaxDropDwn(int FreightInvDetailId)
        {
            List<TaxModel> taxontaxlist = new List<TaxModel>();
            List<FreightFwdInvoiceTaxesModel> LstTaxModel = new List<FreightFwdInvoiceTaxesModel>();
            FreightFwdInvoiceModel FFModel = new FreightFwdInvoiceModel();
            EnquiryBL Enqbl = new EnquiryBL();
            if (Session["FFinvoiceDetail"] != null)
            {
                FFModel = ((FreightFwdInvoiceModel)Session["FFinvoiceDetail"]);
                foreach (TaxModel t in FFModel.TaxDrp)
                {
                    taxontaxlist.Add(t);
                }
            }
            try
            {
                if (FFModel.LstTax.Count > 0)
                {
                    //LstTaxModel = Enqbl.GetReceiptItemTaxDetail(ReceiptId, ItemId);
                    foreach (FreightFwdInvoiceTaxesModel tempModel in FFModel.LstTax.Where(s => (s.FreightInvDetailId == FreightInvDetailId)))
                    {
                        var actionToDelete = from actionRepDel in FFModel.TaxDrp.Where(s => (s.TaxId == tempModel.TaxId))
                                             where (actionRepDel.TaxId == tempModel.TaxId)
                                             select actionRepDel;
                        if (actionToDelete.Count() > 0)
                            taxontaxlist.Remove(actionToDelete.ElementAt(0));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(taxontaxlist, "TaxId", "TaxName"));
        }
        public ActionResult ReceiptDrpDwn()
        {
            List<FreightFwdReceiptDrpModel> LstModel = new List<FreightFwdReceiptDrpModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                LstModel = Enqbl.ReceiptDrpDwn(0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(LstModel, "ReceiptId", "ReceiptName"));
        }
        public ActionResult FFInvoiceSaveItemTax(int TaxId, string TaxName, double TaxAmount, int FreightInvDetailId, int FreightInvId)
        {
            try
            {
                string User_Id = User.Identity.GetUserId();
                FreightFwdInvoiceModel FFModel = new FreightFwdInvoiceModel();
                FFModel.LstDetal = new List<FreightFwdInvoiceDetailModel>();
                if (Session["FFinvoiceDetail"] != null)
                {
                    FFModel = Session["FFinvoiceDetail"] as FreightFwdInvoiceModel;
                }
                if (FFModel.LstDetal == null)
                    FFModel.LstTax = new List<FreightFwdInvoiceTaxesModel>();
                if (FFModel.LstTax.Count > 0)
                {
                    var actionToDelete = from actionRepDel in FFModel.LstTax.Where(s => (s.FreightInvDetailId == FreightInvDetailId))
                                         where (actionRepDel.TaxId == TaxId) && (actionRepDel.FreightInvDetailId == FreightInvDetailId)
                                         select actionRepDel;
                    if (actionToDelete.Count() > 0)
                        FFModel.LstTax.Remove(actionToDelete.ElementAt(0));
                }
                FreightFwdInvoiceTaxesModel demoModel = new FreightFwdInvoiceTaxesModel();
                demoModel.TaxId = TaxId;
                demoModel.TaxName = TaxName;
                demoModel.FreightInvDetailId = FreightInvDetailId;
                demoModel.Amount = TaxAmount;
                FFModel.LstTax.Add(demoModel);
                BOMAdminBL taxbl = new BOMAdminBL();
                FFModel.TaxDrp = taxbl.TaxList(User_Id);
                Session["FFinvoiceDetail"] = FFModel;
                return Json(FFModel.LstTax, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult SaveFFInvoiceDetails(int FreightInvDetailId, string FFRef_No, DateTime RefDate, double InvoiceAmount, int FreightInvId)
        {
            FreightFwdInvoiceModel FFInvoiceModel = new FreightFwdInvoiceModel();
            FFInvoiceModel.LstDetal = new List<FreightFwdInvoiceDetailModel>();
            if (Session["FFinvoiceDetail"] != null)
            {
                FFInvoiceModel = Session["FFinvoiceDetail"] as FreightFwdInvoiceModel;
            }
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                if (FreightInvDetailId > 0)
                {
                    if (FFInvoiceModel.LstDetal.Count > 0)
                    {
                        var actionToDelete = from actionRepDel in FFInvoiceModel.LstDetal.Where(s => (s.FreightInvDetailId == FreightInvDetailId))
                                             where (actionRepDel.FreightInvDetailId == FreightInvDetailId)
                                             select actionRepDel;
                        if (actionToDelete.Count() > 0)
                            FFInvoiceModel.LstDetal.Remove(actionToDelete.ElementAt(0));
                    }
                }
                FreightFwdInvoiceDetailModel demoModel = new FreightFwdInvoiceDetailModel();
                demoModel.FreightInvDetailId = FreightInvDetailId;
                demoModel.FFRef_No = FFRef_No;
                demoModel.RefDate = RefDate;
                demoModel.Amount = InvoiceAmount;
                demoModel.FreightInvId = FreightInvId;
                FFInvoiceModel.LstDetal.Add(demoModel);
                FFInvoiceModel = Enqbl.SaveFFInvoiceDetail(FFInvoiceModel, User.Identity.GetUserId(), FreightInvDetailId, FreightInvId);
                Session["FFinvoiceDetail"] = demoModel;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                throw ex;
            }
            return Json(FFInvoiceModel.LstDetal, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateEditFFInvoiceDetails(int FreightInvDetailId, int FreightInvId, int FFId)
        {
            string User_Id = User.Identity.GetUserId();
            FreightFwdInvoiceModel FFInvoiceModel = new FreightFwdInvoiceModel();
            FreightFwdInvoiceDetailModel FFDetailModel = new FreightFwdInvoiceDetailModel();
            FFDetailModel.LstTax = new List<FreightFwdInvoiceTaxesModel>();
            FFDetailModel.FreightInvDetailId = FreightInvDetailId;
            FFDetailModel.FreightInvId = FreightInvId;
            if (Session["FFinvoiceDetail"] != null)
            {
                FFInvoiceModel = Session["FFinvoiceDetail"] as FreightFwdInvoiceModel;
            }
            if (FreightInvDetailId > 0 && FreightInvId > 0)
            {
                foreach (FreightFwdInvoiceDetailModel detMod in FFInvoiceModel.LstDetal.Where(s => (s.FreightInvDetailId == FreightInvDetailId)))
                {
                    FFDetailModel.FreightInvDetailId = detMod.FreightInvDetailId;
                    FFDetailModel.FreightInvId = detMod.FreightInvId;
                    FFDetailModel.FFRef_No = detMod.FFRef_No;
                    FFDetailModel.RefDate = detMod.RefDate;
                    FFDetailModel.Amount = detMod.Amount;
                    FFDetailModel.ReceiptId = detMod.ReceiptId;
                }
            }
            try
            {
                List<FreightFwdReceiptDrpModel> LstModel = new List<FreightFwdReceiptDrpModel>();
                EnquiryBL Enqbl = new EnquiryBL();
                LstModel = Enqbl.ReceiptDrpDwn(FFId);
                ViewBag.LstAirwayBil = LstModel;
                ViewBag.LstAirwayBil = new SelectList(LstModel, "ReceiptName", "ReceiptName");

                FreightFwdInvoiceModel FFModel = new FreightFwdInvoiceModel();
                FFModel.TaxDrp = new List<TaxModel>();
                BOMAdminBL taxbl = new BOMAdminBL();
                FFModel.TaxDrp = taxbl.TaxList(User_Id);
                ViewBag.TaxListDrp = new SelectList(FFModel.TaxDrp, "TaxId", "TaxName");
                ViewBag.LstTaxParse = FFDetailModel.LstTax;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(FFDetailModel);
        }
        public ActionResult DeleteFFInvTax(int FreightInvDetailId, int TaxId)
        {
            FreightFwdInvoiceModel FFInvoiceModel = new FreightFwdInvoiceModel();
            FFInvoiceModel.LstTax = new List<FreightFwdInvoiceTaxesModel>();
            if (Session["FFinvoiceDetail"] != null)
            {
                FFInvoiceModel = Session["FFinvoiceDetail"] as FreightFwdInvoiceModel;
            }
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                if (FreightInvDetailId > 0)
                {
                    if (FFInvoiceModel.LstTax.Count > 0)
                    {
                        var actionToDelete = from actionRepDel in FFInvoiceModel.LstTax.Where(s => (s.FreightInvDetailId == FreightInvDetailId) && (s.TaxId == TaxId))
                                             where (actionRepDel.FreightInvDetailId == FreightInvDetailId) && (actionRepDel.TaxId == TaxId)
                                             select actionRepDel;
                        if (actionToDelete.Count() > 0)
                            FFInvoiceModel.LstTax.Remove(actionToDelete.ElementAt(0));
                    }
                }
                Session["FFinvoiceDetail"] = FFInvoiceModel;
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DeleteFFDetail(int FreightInvDetailId, int FreightInvId)
        {
            FreightFwdInvoiceModel FFInvoiceModel = new FreightFwdInvoiceModel();
            FFInvoiceModel.LstDetal = new List<FreightFwdInvoiceDetailModel>();
            if (Session["FFinvoiceDetail"] != null)
            {
                FFInvoiceModel = Session["FFinvoiceDetail"] as FreightFwdInvoiceModel;
            }
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                FFInvoiceModel = Enqbl.DeleteFFDetail(FreightInvDetailId, FreightInvId);
                Session["FFinvoiceDetail"] = FFInvoiceModel;
                return Json(FFInvoiceModel.LstDetal, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Freight Forworder  Invoice

        #region Freight Forwarder Notification
        public ActionResult FFNotificationList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<ReceiptModel> LstReceipt = new List<ReceiptModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                Session["ReceiptDetail"] = null;
                LstReceipt = BLObj.FFNotificationList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstReceipt);
        }
        [System.Web.Http.ActionName("ExportFFNotificationList")]
        [AcceptVerbs("POST")]
        public void ExportFFNotificationList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            var DataSource = BLObj.FFNotificationList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult FFNotificationEdit(string AirwaybillNumber, int ReceiptId)
        {
            EnquiryBL BLObj = new EnquiryBL();
            FFNotificationModel NotificationModel = new FFNotificationModel();
            try
            {
                if (AirwaybillNumber != "")
                {
                    NotificationModel = BLObj.GetSelectedFFNotification(AirwaybillNumber, ReceiptId);
                    NotificationModel.ReceiptId = ReceiptId;
                }
                List<CurrencyCodes> lstModel = new List<CurrencyCodes>();
                TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                lstModel = ObjTMBL.GetCurrencyCodes();
                ViewBag.Currency = new SelectList(lstModel, "Key", "Value", NotificationModel.Currency);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(NotificationModel);
        }
        [HttpPost]
        public ActionResult FFNotificationEdit(FormCollection fc)
        {
            EnquiryBL BLObj = new EnquiryBL();
            FFNotificationModel NotificationModel = new FFNotificationModel();
            string User_ID = User.Identity.GetUserId();
            try
            {
                NotificationModel.ReceiptId = Convert.ToInt32(fc["ReceiptId"]);
                NotificationModel.AirwaybillNumber = fc["FFRef_No"];
                NotificationModel.NotificationId = Convert.ToInt32(fc["NotificationId"]);
                NotificationModel.RefNo = fc["RefNo"].ToString();
                NotificationModel.NotificationDate = Convert.ToDateTime(fc["NotificationDate"]);
                NotificationModel.PermitNo = fc["PermitNo"].ToString();
                NotificationModel.GSTAmount = Convert.ToDouble(fc["GSTAmount"]);
                NotificationModel.Currency = fc["Currency"].ToString();
                string[] str = NotificationModel.Currency.Split(',');
                NotificationModel.Currency = str[0];
                int ErrCod = BLObj.SaveFFNotificationDetail(NotificationModel.AirwaybillNumber, NotificationModel.NotificationId, NotificationModel.RefNo, NotificationModel.NotificationDate, NotificationModel.PermitNo, NotificationModel.GSTAmount, NotificationModel.Currency, User_ID, NotificationModel.ReceiptId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("PurchaseReciptList");
        }
        #endregion Freight Forwarder Notification

        #region Enq Reason
        public ActionResult GetReasonList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<ReasonModel> LstReason = new List<ReasonModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                LstReason = BLObj.GetReasonList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstReason);
        }
        [System.Web.Http.ActionName("ExportReasonList")]
        [AcceptVerbs("POST")]
        public void ExportReasonList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            var DataSource = BLObj.GetReasonList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateReason(int ReasonId)
        {
            EnquiryBL BLObj = new EnquiryBL();
            ReasonModel ReasonModel = new ReasonModel();
            try
            {
                if (ReasonId > 0)
                    ReasonModel = BLObj.GetSelectedReasonList(ReasonId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ReasonModel);
        }
        [HttpPost]
        public ActionResult CreateReason(FormCollection fc)
        {
            EnquiryBL BLObj = new EnquiryBL();
            ReasonModel ReasonModel = new ReasonModel();
            string User_ID = User.Identity.GetUserId();
            try
            {
                ReasonModel.ReasonId = Convert.ToInt32(fc["ReasonId"]);
                ReasonModel.Reason = fc["Reason"].ToString();
                int errCode = BLObj.SaveReasonDetail(ReasonModel.ReasonId, ReasonModel.Reason, User_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetReasonList");
        }
        #endregion Enq Reason

        #region Quotation Followup Reason
        public ActionResult GetFollowupReasonList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            List<FollowupTypeModel> LstFollowupReason = new List<FollowupTypeModel>();
            try
            {
                LstFollowupReason = BLObj.GetFollowupTypeList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstFollowupReason);
        }
        [System.Web.Http.ActionName("ExportFollowupReasonList")]
        [AcceptVerbs("POST")]
        public void ExportFollowupReasonList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            var DataSource = BLObj.GetFollowupTypeList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateFollowupReason(int ReasonId)
        {
            EnquiryBL BLObj = new EnquiryBL();
            FollowupTypeModel FollowupReasonModel = new FollowupTypeModel();
            try
            {
                if (ReasonId > 0)
                    FollowupReasonModel = BLObj.GetSelectedFollowupTypeList(ReasonId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(FollowupReasonModel);
        }
        [HttpPost]
        public ActionResult CreateFollowupReason(FormCollection fc)
        {
            EnquiryBL BLObj = new EnquiryBL();
            FollowupTypeModel FollowupReasonModel = new FollowupTypeModel();
            string User_ID = User.Identity.GetUserId();
            try
            {
                FollowupReasonModel.FollowupTypeId = Convert.ToInt32(fc["FollowupTypeId"]);
                FollowupReasonModel.FollowupReason = fc["FollowupReason"].ToString();
                int errCode = BLObj.DWSaveFollowupTypeDetail(FollowupReasonModel.FollowupTypeId, FollowupReasonModel.FollowupReason, User_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetFollowupReasonList");
        }
        #endregion Quotation Followup Reason

        #region Close Enquiry
        public ActionResult CloseEnq(int EnqId, string Source, int Status)
        {
            ReasonModel ReasonModel = new ReasonModel();
            List<ReasonModel> LstReason = new List<ReasonModel>();
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                ReasonModel.EnqId = EnqId;
                ReasonModel.Source = Source;
                ReasonModel.Status = Status;
                LstReason = BLObj.GetReasonList(User_Id);
                ViewBag.LstReasonDrp = new SelectList(LstReason, "ReasonId", "Reason");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return PartialView(ReasonModel);
        }
        [HttpPost]
        public ActionResult CloseEnq(FormCollection fc)
        {
            ReasonModel ReasonModel = new ReasonModel();
            EnquiryBL BLObj = new EnquiryBL();
            string User_ID = User.Identity.GetUserId();
            int ErrorCode = 0;
            try
            {
                ReasonModel.EnqId = Convert.ToInt32(fc["EnqId"]);
                ReasonModel.ReasonId = Convert.ToInt32(fc["Reason"]);
                ReasonModel.Remark = fc["Remark"].ToString();
                ReasonModel.Source = fc["Source"].ToString();
                int ErrCode = BLObj.SaveCloseEnquiry(ReasonModel.EnqId, ReasonModel.ReasonId, ReasonModel.Remark, User_ID);
                ErrorCode = ErrCode;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Json(ErrorCode, JsonRequestBehavior.AllowGet);
           // return RedirectToAction(ReasonModel.Source);
        }
        #endregion Close Enquiry

        #region Quotation FollowUp
        public ActionResult GetMyQuotFollowuplist()
        {
            List<QoutFollowupModel> QuotReminderList = new List<QoutFollowupModel>();
            QoutFollowupModel QoutFollModel = new QoutFollowupModel();

            try
            {
                string User_Id = User.Identity.GetUserId();
                EnquiryBL BLObj = new EnquiryBL();
                QoutFollModel.QuotReminderList = BLObj.GetQoutMYFollowUpList(User_Id);
                // QuotReminderList = BLObj.GetQuotFollowupReminderList(User_Id);
                DateTime TxtParamValue1 = System.DateTime.Now;
                DateTime TxtParamValue2 = DateTime.Now.AddDays(-30);
                string TxtParamValue = TxtParamValue2.ToShortDateString() + "," + TxtParamValue1.ToShortDateString();
                QoutFollModel.ParamId = "@FromDate,@ToDate";
                QoutFollModel.ParamName = "@FromDate,@ToDate";
                QoutFollModel.hidText = "";
                QoutFollModel.ReportId = "MGMT012";
                QoutFollModel.TxtParamValue = TxtParamValue;
                ReportNameModel Model = new ReportNameModel();
                Model = BLObj.GetOpenReportForQuotFolloupLst(User_Id);
                if (Model.ReportId != null)
                {
                    QoutFollModel.OutputLocation = Model.OutputLocation;
                    QoutFollModel.RunDate = Model.RunDate;
                    QoutFollModel.StatusId = Model.StatusId;
                    QoutFollModel.ReportId = Model.ReportId;
                    QoutFollModel.TxtParamValue = TxtParamValue;
                }



            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View(QoutFollModel);

        }
        public ActionResult GetSubOrdinateQuotFollowuplist()
        {
            List<QoutFollowupModel> QuotReminderList = new List<QoutFollowupModel>();
            QoutFollowupModel QoutFollModel = new QoutFollowupModel();

            try
            {
                string User_Id = User.Identity.GetUserId();
                EnquiryBL BLObj = new EnquiryBL();
                QoutFollModel.QuotReminderList = BLObj.GetQoutSubordinateFollowUpList(User_Id);
                // QuotReminderList = BLObj.GetQuotFollowupReminderList(User_Id);
                DateTime TxtParamValue1 = System.DateTime.Now;
                DateTime TxtParamValue2 = DateTime.Now.AddDays(-30);
                string TxtParamValue = TxtParamValue2.ToShortDateString() + "," + TxtParamValue1.ToShortDateString();
                QoutFollModel.ParamId = "@FromDate,@ToDate";
                QoutFollModel.ParamName = "@FromDate,@ToDate";
                QoutFollModel.hidText = "";
                QoutFollModel.ReportId = "MGMT012";
                QoutFollModel.TxtParamValue = TxtParamValue;
                ReportNameModel Model = new ReportNameModel();
                Model = BLObj.GetOpenReportForQuotFolloupLst(User_Id);
                if (Model.ReportId != null)
                {
                    QoutFollModel.OutputLocation = Model.OutputLocation;
                    QoutFollModel.RunDate = Model.RunDate;
                    QoutFollModel.StatusId = Model.StatusId;
                    QoutFollModel.ReportId = Model.ReportId;
                    QoutFollModel.TxtParamValue = TxtParamValue;
                }



            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View(QoutFollModel);

        }
        public ActionResult GetQuotFollowupReminderList()
        {
            List<QoutFollowupModel> QuotReminderList = new List<QoutFollowupModel>();
            QoutFollowupModel QoutFollModel = new QoutFollowupModel();
            Session["NewQuotDetail"] = null;
            try
            {
                string User_Id = User.Identity.GetUserId();
                EnquiryBL BLObj = new EnquiryBL();
                QoutFollModel.QuotReminderList = BLObj.GetQuotFollowupReminderList(User_Id);
                // QuotReminderList = BLObj.GetQuotFollowupReminderList(User_Id);
                string user_Id = User.Identity.GetUserId();

                if (Session["info"] != null)
                {
                    string str = Session["info"] as string;
                    TempData["Message"] = str;
                    Session["info"] = null;
                }
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
                Session["ParamDescrip"] = prmmodParamDemo;

                DateTime TxtParamValue1 = System.DateTime.Now;
                DateTime TxtParamValue2 = DateTime.Now.AddDays(-30);
                string TxtParamValue = TxtParamValue2.ToShortDateString() + "," + TxtParamValue1.ToShortDateString();
                QoutFollModel.ParamId = "@FromDate,@ToDate";
                QoutFollModel.ParamName = "@FromDate,@ToDate";
                QoutFollModel.hidText = "";
                QoutFollModel.ReportId = "MGMT012";
                QoutFollModel.TxtParamValue = TxtParamValue;
                ReportNameModel Model = new ReportNameModel();
                Model = BLObj.GetOpenReportForQuotFolloupLst(User_Id);
                if (Model.ReportId != null)
                {
                    QoutFollModel.OutputLocation = Model.OutputLocation;
                    QoutFollModel.RunDate = Model.RunDate;
                    QoutFollModel.StatusId = Model.StatusId;
                    QoutFollModel.ReportId = Model.ReportId;
                    QoutFollModel.TxtParamValue = TxtParamValue;
                }



            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View(QoutFollModel);
        }
        [System.Web.Http.ActionName("ExportQuotFollowupReminderList")]
        [AcceptVerbs("POST")]
        public void ExportQuotFollowupReminderList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            var DataSource = BLObj.GetQuotFollowupReminderList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateFollowup(int QuotationId)
        {
            QoutFollowupModel FollwupModel = new QoutFollowupModel();
            List<QuotFollowupDetailModel> QuotFollowupDetailList = new List<QuotFollowupDetailModel>();
            List<FollowupTypeModel> LstFollowupReason = new List<FollowupTypeModel>();
            List<QoutFollowupHistoryModel> QuotReminderList = new List<QoutFollowupHistoryModel>();
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                FollwupModel.QuotationId = QuotationId;
                LstFollowupReason = BLObj.GetFollowupTypeList(User_Id);
                ViewBag.LstFollowupReason = LstFollowupReason;
                QuotReminderList = BLObj.GetQuotFollowupHistoryList(QuotationId);
                QuotFollowupDetailList = BLObj.GetFollowupQuotDetailList(QuotationId);
                ViewBag.LstFollowupQuotDetail = QuotFollowupDetailList;
                ViewBag.QuotReminderList = QuotReminderList;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return PartialView(FollwupModel);
        }
        [HttpPost]
        public ActionResult CreateFollowup(FormCollection fc)
        {
            QoutFollowupModel FollwupModel = new QoutFollowupModel();
            string User_Id = User.Identity.GetUserId();
            EnquiryBL BLObj = new EnquiryBL();
            int ErrorCode = 0;
            try
            {
                FollwupModel.QuotationId = Convert.ToInt32(fc["QuotationId"].ToString());
                FollwupModel.FollowupReasonId = Convert.ToInt32(fc["FollowupRes"].ToString());
                FollwupModel.Followupdate = Convert.ToDateTime(fc["Followupdate"].ToString());
                FollwupModel.NextFollowUp = Convert.ToDateTime(fc["NextFollowUp"].ToString());
                FollwupModel.FollowupRemark = fc["FollowupRemark"].ToString();
                string ResonText = fc["FollowupText"].ToString();
                ErrorCode = BLObj.SaveFollowupDaetail(FollwupModel, User_Id);
                if (ErrorCode == 600002)
                {
                    List<QoutFollowupHistoryModel> Lst = new List<QoutFollowupHistoryModel>();
                    QoutFollowupHistoryModel Model = new QoutFollowupHistoryModel();
                    if (Session["NewQuotDetail"] != null)
                    {
                        Lst = Session["NewQuotDetail"] as List<QoutFollowupHistoryModel>;
                    }
                    if (Lst.Count > 0)
                        NewOuatDetail(FollwupModel.QuotationId, ResonText, FollwupModel.FollowupRemark, FollwupModel.FollowupReasonId);
                    else
                        return RedirectToAction("GetQuotFollowupReminderList");
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("GetQuotFollowupReminderList");
        }
        public ActionResult TempItem(int QuotationId, int ItemId, string Remark, string MPN, string ItemName)
        {
            List<QoutFollowupHistoryModel> Lst = new List<QoutFollowupHistoryModel>();
            QoutFollowupHistoryModel Model = new QoutFollowupHistoryModel();
            if (Session["NewQuotDetail"] != null)
            {
                Lst = Session["NewQuotDetail"] as List<QoutFollowupHistoryModel>;
            }
            Model.QuotationId = QuotationId;
            Model.ItemId = ItemId;
            Model.FollowupReason = Remark;
            Model.ItemName = ItemName;
            Model.MPN = MPN;
            if (Lst.Count > 0)
            {
                var actionToDelete = from actionRepDel in Lst
                                     where actionRepDel.ItemId == ItemId
                                     select actionRepDel;
                if (actionToDelete.Count() > 0)
                    Lst.Remove(actionToDelete.ElementAt(0));
            }
            Lst.Add(Model);
            Session["NewQuotDetail"] = Lst;
            return View();
        }
        public ActionResult EmptyTempItem()
        {
            Session["NewQuotDetail"] = null;
            return Json("", JsonRequestBehavior.AllowGet);
        }
        public ActionResult HaveItem()
        {
            List<QoutFollowupHistoryModel> Lst = new List<QoutFollowupHistoryModel>();
            QoutFollowupHistoryModel Model = new QoutFollowupHistoryModel();
            if (Session["NewQuotDetail"] != null)
            {
                Lst = Session["NewQuotDetail"] as List<QoutFollowupHistoryModel>;
                if (Lst.Count > 0)
                {
                    return Json("Have", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("NotHave", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("NotHave", JsonRequestBehavior.AllowGet);
            }
        }
        public void NewOuatDetail(int QuotationId, string FollowupRes, string FollowupRemark, int FollowupReasonId)
        {
            List<QoutFollowupHistoryModel> Lst = new List<QoutFollowupHistoryModel>();
            string User_Id = User.Identity.GetUserId();
            if (Session["NewQuotDetail"] != null)
            {
                Lst = Session["NewQuotDetail"] as List<QoutFollowupHistoryModel>;
            }
            EnquiryBL BLObj = new EnquiryBL();
            int ErrCode = BLObj.SaveNewOuatDetail(Lst, QuotationId, FollowupRes, FollowupRemark, User_Id, FollowupReasonId);
            Session["NewQuotDetail"] = null;
        }
        public ActionResult FollowupHistory(int QuotationId)
        {
            List<QoutFollowupHistoryModel> QuotReminderList = new List<QoutFollowupHistoryModel>();
            try
            {
                string User_Id = User.Identity.GetUserId();
                EnquiryBL BLObj = new EnquiryBL();
                QuotReminderList = BLObj.GetQuotFollowupHistoryList(QuotationId);
                ViewBag.QuotReminderList = QuotReminderList;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return PartialView(QuotReminderList);
        }
        public ActionResult QuotationView(int QuotId, int EnqId)
        {
            EnquiryModel Enqmodel = new EnquiryModel();
            Enqmodel.QuotList = new List<QuotationModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            Enqmodel = Enqbl.GetSelectedEnqQuotation(EnqId, QuotId);
            List<EnquiryItemVendorResponseDetailModel> CustItemList = new List<EnquiryItemVendorResponseDetailModel>();
            CustItemList = Enqbl.GetCustQuotItemforApproval(EnqId, QuotId);
            ViewBag.CustItemList = CustItemList;
            return PartialView(Enqmodel);
        }
        #endregion Quotation FollowUp

        #region TO DO
        public ActionResult AllPendingWork()
        {
            List<AllPendingWorkModel> PendingWorkList = new List<AllPendingWorkModel>();
            AllPendingWorkModel Model = new AllPendingWorkModel();
            List<SubordinateListModel> SubOrdinateList = new List<SubordinateListModel>();
            EnquiryBL BLObj = new EnquiryBL();
            try
            {
                string User_Id = User.Identity.GetUserId();

                if (Session["User_IdStr"] != null)
                {
                    User_Id = Session["User_IdStr"] as string;
                    //Session["User_IdStr"] = User_Id;
                }
                else
                {
                    Session["User_IdStr"] = User_Id;
                }
                ViewBag.Gernal = PendingWorkList;
                SubOrdinateList = BLObj.GetSubOrdinateList(User.Identity.GetUserId());
                ViewBag.LstEmp = new SelectList(SubOrdinateList, "User_Id", "EmployeeName", User_Id);
                //Model.Datetype = "ToDo";
                //string DateType = Model.Datetype;
                //PendingWorkList = BLObj.GetAllPendingWorkList(User_Id, DateType);
                //ViewBag.ToDoList = PendingWorkList;

                //Model.Datetype = "OverDue";
                //string DateType1 = Model.Datetype;
                //PendingWorkList = BLObj.GetAllPendingWorkList(User_Id, DateType1);
                //ViewBag.OverDueList = PendingWorkList;

                //Model.Datetype = "Upcoming";
                //string DateType2 = Model.Datetype;
                //PendingWorkList = BLObj.GetAllPendingWorkList(User_Id, DateType2);
                //ViewBag.Upcoming = PendingWorkList;

                ArrayList ar = new ArrayList();
                ar = BLObj.GetTODOGroupList();
                ViewBag.Group = ar;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View();
        }
        public ActionResult GetToDOData(string TabName)
        {
            List<AllPendingWorkModel> PendingWorkList = new List<AllPendingWorkModel>();
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            if (Session["User_IdStr"] != null)
            {
                User_Id = Session["User_IdStr"] as string;
                // Session["User_IdStr"] = null;
            }
            PendingWorkList = BLObj.GetAllPendingWorkList(User_Id, "", TabName);
            return Json(PendingWorkList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubordinateData(string User_Id)
        {
            Session["User_IdStr"] = User_Id;
            return RedirectToAction("AllPendingWork");
        }

        public ActionResult CreatePlanForTODO(string DocType, int DocId, int DocRefId, int PlanId)
        {
            BL.TMPlan.TMPlanModel objmodel = new BL.TMPlan.TMPlanModel();
            BL.TMPlan.TMPlanBL BLObj = new BL.TMPlan.TMPlanBL();
            string User_Id = User.Identity.GetUserId();
            BL.Tmleave.TimeSheetDlyEntryList TMModel = new BL.Tmleave.TimeSheetDlyEntryList();
            BL.Tmleave.TMLeaveBL BLObj1 = new BL.Tmleave.TMLeaveBL();
            AdminBL objbl = new AdminBL();
            BL.TMPlan.TMPlanBL objbl1 = new BL.TMPlan.TMPlanBL();
            int Eid = objbl.GetEmployeeByUserId(User_Id);
            try
            {
                if (PlanId > 0)
                {
                    objmodel = objbl1.GetTMPlanSelectedList(PlanId);
                }
                else
                {
                    objmodel.StartDate = DateTime.Now;
                    objmodel.EndDate = DateTime.Now;
                }
                objmodel.EmpId = Eid;
                objmodel.DocType = DocType;
                objmodel.DocId = DocId;
                objmodel.DocRefId = DocRefId;
                objmodel.User_Id = User_Id;
                TMModel.TMProjectlist = BLObj1.GetProjectList(User_Id);
                ViewBag.TMProjectlist = new SelectList(TMModel.TMProjectlist, "ProjectId", "ProjectName");

                TMModel.TMTasklist = BLObj1.GetTaskList(objmodel.ProjectId, User_Id);
                ViewBag.TMTasklist = new SelectList(TMModel.TMTasklist, "TaskId", "TaskName");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(objmodel);
        }
        [HttpPost]
        public ActionResult CreatePlanForTODO(FormCollection fc)
        {
            BL.TMPlan.TMPlanModel objmodel = new BL.TMPlan.TMPlanModel();
            BL.TMPlan.TMPlanBL BLObj = new BL.TMPlan.TMPlanBL();
            string User_Id = User.Identity.GetUserId();
            List<BL.TMPlan.TMPlanModel> Listmodel = new List<BL.TMPlan.TMPlanModel>();
            ViewBag.LstPlan = Listmodel;
            try
            {
                objmodel.PlanId = Convert.ToInt32(fc["PlanId"].ToString());
                objmodel.EmpId = Convert.ToInt32(fc["EmpId"].ToString());
                objmodel.ProjectId = Convert.ToInt32(fc["ProjectId"].ToString());
                objmodel.TaskId = Convert.ToInt32(fc["TaskId"].ToString());
                objmodel.StartDate = Convert.ToDateTime(fc["dtpStartDate"].ToString());
                objmodel.EndDate = Convert.ToDateTime(fc["dtpEndDate"].ToString());
                objmodel.AllDay = false;
                objmodel.Repete = false;
                objmodel.Never = false;
                objmodel.Remark = fc["Remark"].ToString();
                objmodel.FrequencyType = 'E';
                objmodel.WeekNo = 1;
                objmodel.MonthNo = 1;
                objmodel.OnDate = objmodel.StartDate.Date;
                objmodel.RecurrenceRule = "";
                objmodel.WeekDays = "0000000";
                objmodel.DocType = fc["DocType"].ToString();
                objmodel.DocId = Convert.ToInt32(fc["DocId"].ToString());
                objmodel.DocRefId = Convert.ToInt32(fc["DocRefId"].ToString());
                int errCode = BLObj.saveTMPlanDetails(objmodel, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("AllPendingWork");
        }
        public ActionResult GetPlanData(int EmpId, DateTime Date)
        {
            List<BL.TMPlan.TMPlanModel> Listmodel = new List<BL.TMPlan.TMPlanModel>();
            BL.TMPlan.TMPlanBL BLObj = new BL.TMPlan.TMPlanBL();
            Listmodel = BLObj.GetPlanDataByDate(EmpId, Date);
            return Json(Listmodel, JsonRequestBehavior.AllowGet);
        }
        #endregion TO DO

        #region Preliminary Quotation
        public ActionResult GetPreliminaryQuotList()
        {
            List<PreliminaryQuotationModel> PreQuotModelLst = new List<PreliminaryQuotationModel>();
            try
            {
                string User_Id = User.Identity.GetUserId();
                EnquiryBL BLObj = new EnquiryBL();
                PreQuotModelLst = BLObj.GetPreliminaryQuotList(User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View(PreQuotModelLst);
        }
        public ActionResult PreQuotDetail(int PreQuotId)
        {
            PreliminaryQuotationModel PreQuotModel = new PreliminaryQuotationModel();
            PreQuotModel.LstDetail = new List<PreliminaryQuotDetailModel>();
            EnquiryBL BLObj = new EnquiryBL();
            try
            {
                PreQuotModel = BLObj.GetSelectedPreliminaryQuotList(PreQuotId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View(PreQuotModel);
        }
        public ActionResult ViewEditPreQuotDetail(int PreQuotId, int ItemId)
        {
            PreQuotRateDetailModel PreQuotRateModel = new PreQuotRateDetailModel();
            EnquiryBL BLObj = new EnquiryBL();
            try
            {
                PreQuotRateModel = BLObj.GetSelectedPreQuotRateList(PreQuotId, ItemId);
                PreQuotRateModel.PreQuotId = PreQuotId;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return PartialView(PreQuotRateModel);
        }
        public ActionResult UpdateItemRate(int PreQuotId, int ItemId, double Rate)
        {
            PreQuotRateDetailModel PreQuotRateModel = new PreQuotRateDetailModel();
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                int errcode = BLObj.UpdateItemRate(PreQuotId, ItemId, Rate, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("PreQuotDetail", new { PreQuotId = PreQuotId });
        }
        #endregion Preliminary Quotation

        #region Nav POSO Mapping
        public ActionResult GetNavPOSOMappingList()
        {
            List<NavPOSOMapModel> LstModel = new List<NavPOSOMapModel>();
            try
            {
                if (Session["infoMap"] != null)
                {
                    string str = Session["infoMap"] as string;
                    TempData["Message"] = str;
                    Session["infoMap"] = null;
                }
                string User_Id = User.Identity.GetUserId();
                EnquiryBL BLObj = new EnquiryBL();
                LstModel = BLObj.GetNavPOSOMappingList(User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View(LstModel);

        }
        public ActionResult CreateNavPOSOMapping(string NAVPONO, string NAVSONO, string CompCode)
        {
            NavPOSOMapModel Model = new NavPOSOMapModel();
            try
            {
                if (NAVPONO != null && NAVSONO != null && CompCode != null)
                {
                    EnquiryBL BLObj = new EnquiryBL();
                    Model = BLObj.GetSelectedNavPOSOMappingList(NAVPONO, NAVSONO, CompCode);
                }
                ItemBL BlObj = new ItemBL();
                ItemMappingModel ItemMap = new ItemMappingModel();
                ItemMap.lstItemMap = BlObj.GetDWCompList();
                ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return PartialView(Model);
        }
        [HttpPost]
        public ActionResult CreateNavPOSOMapping(FormCollection fc)
        {
            NavPOSOMapModel Model = new NavPOSOMapModel();
            try
            {
                string User_Id = User.Identity.GetUserId();
                Model.NAVPONO = fc["NAVPONO"].ToString();
                Model.NAVSONO = fc["NAVSONO"].ToString();
                Model.CompCode = fc["CompCode"].ToString();
                Model.OldNAVPONO = fc["OldNAVPONO"].ToString();
                Model.OldNAVSONO = fc["OldNAVSONO"].ToString();
                Model.OldCompCode = fc["OldCompCode"].ToString();
                EnquiryBL BLObj = new EnquiryBL();
                if (Model.OldNAVPONO == Model.NAVPONO && Model.OldNAVSONO == Model.NAVSONO && Model.OldCompCode.ToUpper() == Model.CompCode.ToUpper())
                {
                    Session["infoMap"] = "Data Succesfully Update.";
                }
                else
                {
                    int ErrCode = BLObj.SaveNavPOSOMapping(Model.NAVPONO, Model.NAVSONO, Model.CompCode, User_Id, Model.OldNAVPONO, Model.OldNAVSONO, Model.OldCompCode);
                    if (ErrCode == 600005)
                    {
                        Session["infoMap"] = "Record Already Exists in System.";
                    }
                    else if (ErrCode == 600001)
                    {
                        Session["infoMap"] = "Data Succesfully Update.";
                    }
                    else if (ErrCode == 600002)
                    {
                        Session["infoMap"] = "Data Succesfully Save.";
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("GetNavPOSOMappingList");
        }
        #endregion Nav POSO Mapping

        #region Material Dispatch Intimetion

        public ActionResult MDIList()
        {
            List<MDIModel> ModelList = new List<MDIModel>();
            string User_Id = User.Identity.GetUserId();
            EnquiryBL BLObj = new EnquiryBL();
            try
            {
                ModelList = BLObj.GetPurMRIntimetionList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ModelList);
        }
        public ActionResult CreateMDI(int MRIId)
        {
            MDIModel MDIModel = new MDIModel();
            EnquiryBL BLObj = new EnquiryBL();
            ProjectBL objBL = new ProjectBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                MDIModel = BLObj.GetSelectedPurMRIntimetionList(MRIId);
                ItemBL BlObj = new ItemBL();
                ItemMappingModel ItemMap = new ItemMappingModel();
                ItemMap.lstItemMap = BlObj.GetDWCompList();
                ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");
                // ViewBag.VendorList = new SelectList(objBL.GetVendorListByUser(User_Id), "Id", "DisplayName");
                if (MRIId > 0)
                {
                    List<InvoiceItemModel> model = new List<InvoiceItemModel>();
                    model = BLObj.GetItemListByReceipt(MDIModel.CompCode, MDIModel.VendorId);
                    ViewBag.itemlist = model;
                }
                else
                {
                    MDIModel.Remark = "Material received";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View(MDIModel);
        }
        [HttpPost]
        public ActionResult CreateMDI(FormCollection fc)
        {
            MDIModel MDIModel = new MDIModel();
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                MDIModel.MRIId = Convert.ToInt32(fc["MRIId"].ToString());
                MDIModel.CompCode = fc["CompCode"].ToString();
                MDIModel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                MDIModel.DocumentNo = fc["DocumentNo"].ToString();
                MDIModel.MRIDate = Convert.ToDateTime(fc["MRIDate"].ToString());
                MDIModel.Remark = fc["Remark"].ToString();
                int errCode = BLObj.DWSavePurMRIntimetion(MDIModel, User_Id);
                return RedirectToAction("CreateMDI", new { MRIId = errCode });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult SavePurMRIntimetionItem(int Itemid, int Quantity, string PONo, DateTime ExpectedDate, Double Rate, int MRIId, int MRIDetailId)
        {
            MDIItemModel DetailModel = new MDIItemModel();
            EnquiryBL BLObj = new EnquiryBL();
            string User_Id = User.Identity.GetUserId();
            //DetailModel.MRIDetailId = MRIDetailId;
            //DetailModel.MRIId = MRIId;
            //DetailModel.ItemId = Itemid;
            //DetailModel.Quantity = Quantity;
            //DetailModel.Rate = Rate;
            //DetailModel.NAVPONO = PONo;
            //DetailModel.NAVPODate = ExpectedDate;
            MDIModel Model = new MDIModel();
            Model.LstMDIItemList = new List<MDIItemModel>();
            Model.LstMDIItemList = BLObj.SavePurMRIntimetionItem(Itemid, Quantity, PONo, ExpectedDate, Rate, MRIId, User_Id, MRIDetailId);
            return Json(Model.LstMDIItemList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteIntimetionItem(int MRIId, int MRIDetailId)
        {
            EnquiryBL BLObj = new EnquiryBL();
            MDIModel Model = new MDIModel();
            Model.LstMDIItemList = new List<MDIItemModel>();
            Model.LstMDIItemList = BLObj.DeleteIntimetionItem(MRIId, MRIDetailId);
            return Json(Model.LstMDIItemList, JsonRequestBehavior.AllowGet);
        }
        #endregion Material Dispatch Intimetion

        #region Quotation Analyst
        public ActionResult EnquiryListForQuotAnalyst()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Enquiry/CustomerEnquiry");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                EnquiryModel EnquModel = new EnquiryModel();
                UserId = User.Identity.GetUserId();
                try
                {                    
                    EnquiryBL BLObj = new EnquiryBL();                    
                }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                }

                return View(EnquModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult DataForQuotAnalyst()
        {
            EnquiryModel EnquModel = new EnquiryModel();
            string User_Id = User.Identity.GetUserId();
            try
            {
                EnquiryBL BLObj = new EnquiryBL();
                EnquModel = BLObj.getEnquiryQuotAnalystList(User_Id);
                IEnumerable data = EnquModel.lstEnquiry;
                var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 50000000;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [System.Web.Http.ActionName("EnquiryQuotAnalystExport")]
        [AcceptVerbs("POST")]
        public void EnquiryQuotAnalystExport(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            EnquiryModel EnquModel = new EnquiryModel();
            EnquModel = BLObj.getEnquiryQuotAnalystList(User_Id);
            IEnumerable data = EnquModel.lstEnquiry;
            exp.Export(obj, data, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult QuotAnalystDetailList(int EnqId)
        {
            QuotAnalystModel AnalystModel = new QuotAnalystModel();
            AnalystModel.Last5lossQuot = new List<QoutFollowupModel>();
            AnalystModel.Last5WinQuot = new List<QoutFollowupModel>();
            AnalystModel.QuotFollowupDetailList = new List<QoutFollowupHistoryModel>();
            AnalystModel.ItemDetail = new QuotItem();
            EnquiryBL BLObj = new EnquiryBL();
            try
            {
                string UserId = User.Identity.GetUserId();
                AnalystModel.EnqId = EnqId;
                if (EnqId > 0)
                {
                    AnalystModel = BLObj.GetQuotAnalystDetailList(EnqId, UserId);
                    AnalystModel.ItemDetail = BLObj.GetQuotAnalystItemDetailList(EnqId, Convert.ToInt32(AnalystModel.QuotItemList[0].ItemId));
                    ViewBag.Remark = AnalystModel.ItemDetail.AnalystRemark;
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);                
            }
            return View(AnalystModel);
        }
        public ActionResult GetQuotItemDyndata(string ItemId, int EnqId)
        {
            QuotItem ItemModel = new QuotItem();
            EnquiryBL BLObj = new EnquiryBL();
            ItemModel = BLObj.GetQuotAnalystItemDetailList(EnqId, Convert.ToInt32(ItemId));
            return Json(ItemModel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveAnalystRemark(int EnqId,int ItemId,string Remark)
        {
            try
            {
                EnquiryBL BLObj = new EnquiryBL();
                string User_Id = User.Identity.GetUserId();
                int ErrCode = BLObj.SaveAnalystRemark(EnqId, ItemId, Remark, User_Id);              
                return Json("Remark Save SucessFully..", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return Json("occur Some Error Please Try Again!! ", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion Quotation Analyst

        #region Temporary Receipt Entry
        public ActionResult TempPurchaseReciptList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<ReceiptModel> LstReceipt = new List<ReceiptModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                Session["ReceiptDetail"] = null;
                LstReceipt = BLObj.GetPurchaseReceiptList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstReceipt);
        }
        public ActionResult TempPurchaseCreateRecipt(int ReceiptId)
        {
            string User_Id = User.Identity.GetUserId();
            EnquiryBL BLObj = new EnquiryBL();
            ReceiptModel LstReceipt = new ReceiptModel();
            LstReceipt.LstReceiptModel = new List<ReceiptDetailModel>();
            LstReceipt.LstReceiptTaxModel = new List<ReceiptTaxModel>();
            List<CartonModel> LstCarronModel = new List<CartonModel>();
            ViewBag.LstCart = LstCarronModel;
            List<ReceiptModel> LstOtherCharges = new List<ReceiptModel>();
            ViewBag.LstOtherChages = LstOtherCharges;
            List<ReceiptTaxModel> LstTaxModel = new List<ReceiptTaxModel>();
            ViewBag.LsttaxList = LstTaxModel;
            List<InvoiceItemModel> LstItemModel = new List<InvoiceItemModel>();
            try
            {
                if (ReceiptId > 0)
                {

                    LstReceipt = BLObj.GetSelectedReceipt(ReceiptId);
                }
                else
                {
                    ItemBL BlObj = new ItemBL();
                    ItemMappingModel ItemMap = new ItemMappingModel();
                    ItemMap.lstItemMap = BlObj.GetDWCompList();
                    ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");
                }

                LstItemModel = BLObj.TempGetItemListByReceipt(LstReceipt.CompCode, LstReceipt.VendorId);
                ViewBag.LstItems = new SelectList(LstItemModel, "ItemId", "ItemName"); ;
                BOMAdminBL taxbl = new BOMAdminBL();
                LstReceipt.TaxDrp = taxbl.TaxList(User_Id);
                ViewBag.TaxListDrp = new SelectList(LstReceipt.TaxDrp, "TaxId", "TaxName");

                List<FreightForwarderModel> lstf = new List<FreightForwarderModel>();
                VendorBL vendorbl = new VendorBL();
                lstf = vendorbl.FreightForvordarList();
                ViewBag.FFLst = new SelectList(lstf, "FFId", "VendorName");

                ContyStateCityModel addressModel = new ContyStateCityModel();
                AdminBL BLObjCOO = new AdminBL();
                addressModel = BLObjCOO.GetCountryStateCityList();
                List<AddCountryModel> cntrylst = addressModel.LstCountry;
                ViewBag.CountryLst = new SelectList(cntrylst, "CountryId", "CountryName");

                List<CurrencyCodes> lstModel = new List<CurrencyCodes>();
                TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                lstModel = ObjTMBL.GetCurrencyCodes();
                ViewBag.Currency = new SelectList(lstModel, "Key", "Value", LstReceipt.Currency);
                Session["ReceiptDetail"] = LstReceipt;

                List<CartonModel> CartonList = new List<CartonModel>();
                CartonList = BLObj.CartonList();
                ViewBag.Carton = new SelectList(CartonList, "Id", "Dimension");                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstReceipt);
        }
        public ActionResult TempGetPOforReceiptByItem(string CompCode, int ItemId)
        {
            List<InvoiceModel> model = new List<InvoiceModel>();
            EnquiryBL Enqbl = new EnquiryBL();
            try
            {
                model = Enqbl.TempGetPOforReceiptByItem(CompCode, ItemId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(model, "Invoice_Id", "InvoiceName"));
        }
        #endregion Temporary Receipt Entey

        #region Item Stock Audit
        public ActionResult GetItemStockAuditList()
        {
            EnquiryBL BLObj = new EnquiryBL();
            List<ItemStockAuditModel> LstAudit = new List<ItemStockAuditModel>();
            try
            {
                Session["StockAuditItems"] = null;
                Session["StockAuditExcelData"] = null;
                LstAudit = BLObj.GetItemStockAuditList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(LstAudit);
        }
        [System.Web.Http.ActionName("ExportItemStockList")]
        [AcceptVerbs("POST")]
        public void ExportItemStockList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            EnquiryBL BLObj = new EnquiryBL();
            var DataSource = BLObj.GetReasonList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateAudit(int AuditId)
        {
            EnquiryBL BLObj = new EnquiryBL();
            ItemStockAuditModel AuditModel = new ItemStockAuditModel();
            string User_Id = User.Identity.GetUserId();
            try
            {
                AdminBL AdminBLObj = new AdminBL();
                List<StockLocation> LststkLoc = new List<StockLocation>();
                LststkLoc= AdminBLObj.GetStockLocationListForDrp();
                ViewBag.LstStockLocation = new SelectList(LststkLoc, "STLocationId", "Name");
                ItemBL BlObj = new ItemBL();
                ItemMappingModel ItemMap = new ItemMappingModel();
                ItemMap.lstItemMap = BlObj.GetDWCompList();
                ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");
                List<ItemListModel> lstItem = new List<ItemListModel>();                
                if (AuditId > 0)
                {
                    AuditModel = BLObj.GetSelectedItemStockAuditList(AuditId);
                    lstItem = BLObj.GetItemStockListForDrp(AuditModel.CompCode);
                    Session["StockAuditItems"] = AuditModel.DetailList;
                }
                ViewBag.itemlist = lstItem;
                string TxtParamValue = null;
                TxtParamValue = AuditModel.CompCode;
                AuditModel.ParamId = "@CompanyCode";
                AuditModel.ParamName = "@CompanyCode";
                AuditModel.hidText = "";
                AuditModel.ReportId = "MGMT017";
                AuditModel.TxtParamValue = TxtParamValue;
                string CompanyCode = AuditModel.CompCode;
                ReportNameModel Model = new ReportNameModel();
                Model = BLObj.GetOpenSystemStockReport(User_Id, CompanyCode);
                AuditModel.OutputLocation = Model.OutputLocation;
                AuditModel.RunDate = Model.RunDate;
                AuditModel.CurrentDate = DateTime.Now;
                AuditModel.StatusId = Model.StatusId;
                AuditModel.ReportId = Model.ReportId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(AuditModel);
        }
        [HttpPost]
        public ActionResult CreateAudit(FormCollection fc)
        {
            EnquiryBL BLObj = new EnquiryBL();
            ItemStockAuditModel AuditModel = new ItemStockAuditModel();
            string User_ID = User.Identity.GetUserId();
            int errCode = 0;
            try
            {
                AuditModel.AuditId = Convert.ToInt32(fc["AuditId"]);
                AuditModel.CompCode = fc["CompCode"].ToString();               
                AuditModel.Remark = fc["Remark"].ToString();
                AuditModel.AuditDate = Convert.ToDateTime(fc["AuditDate"]);
                errCode = BLObj.DWSaveItemStockAudit(AuditModel.AuditId, AuditModel.CompCode, AuditModel.AuditDate, User_ID, AuditModel.Remark);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CreateAudit",new { AuditId = errCode });
        }
        public JsonResult GetOutputLocationDetails(int AuditId)
        {
            ItemStockAuditModel AuditModel = new ItemStockAuditModel();
            string User_Id = User.Identity.GetUserId();
            EnquiryBL BLObj = new EnquiryBL();
            AuditModel = BLObj.GetSelectedItemStockAuditList(AuditId);
            ReportNameModel Model = new ReportNameModel();
            string CompanyCode = AuditModel.CompCode;
            Model = BLObj.GetOpenSystemStockReport(User_Id, CompanyCode);
            var OutputLocation = Model.OutputLocation;
            return Json(OutputLocation, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveItemAuditDetail(int Itemid,int Quantity,int AuditId,int AuditDetailId,int LocationId)
        {
            EnquiryBL BLObj = new EnquiryBL();
            string User_ID = User.Identity.GetUserId();
            int errCode = BLObj.DWSaveItemStockAuditDetail(AuditDetailId, AuditId, Itemid, Quantity, User_ID, LocationId);
            ItemStockAuditModel AuditModel = new ItemStockAuditModel();
            AuditModel = BLObj.GetSelectedItemStockAuditList(AuditId);
            Session["StockAuditItems"] = null;
            Session["StockAuditItems"] = AuditModel.DetailList;
            return Json(AuditModel.DetailList, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult StockExcelFile()
        //{            
        //    List<ItemStockAuditDetailModel> Lststock  = new List<ItemStockAuditDetailModel>();
        //    Lststock= Session["StockAuditItems"] as List<ItemStockAuditDetailModel>;
        //    EnquiryBL BLObj = new EnquiryBL();
        //    try
        //    {
        //        if (Request.Files.Count > 0)
        //        {
        //            foreach (string files in Request.Files)
        //            {
        //                var file = Request.Files[files];
        //                var filename = Path.GetFileName(file.FileName);
        //                string[] str = filename.Split('.');
        //                string str1 = Guid.NewGuid().ToString() + "." + str[str.Length - 1];
        //                file.FileName.Replace(file.FileName, str1);
        //                var filepath = Path.Combine(Server.MapPath("~/App_Data/Uploads"), str1);
        //                file.SaveAs(filepath);                       
        //                WorkbookPart workbookPart; List<Row> rows;                        
        //                var document = SpreadsheetDocument.Open(filepath, false);
        //                workbookPart = document.WorkbookPart;
        //                var sheets = workbookPart.Workbook.Descendants<Sheet>();                        
        //                var sheet = sheets.FirstOrDefault();
        //                var workSheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
        //                var columns = workSheet.Descendants<Columns>().FirstOrDefault();                       
        //                var sheetdata = workSheet.Elements<SheetData>().Last();
        //                rows = sheetdata.Elements<Row>().ToList();
        //                var regex = new Regex("[A-Za-z]+");
        //                for (int i = 1; i < rows.Count; i++)
        //                {
        //                    bool Allow = true;
        //                    ItemStockAuditDetailModel Model = new ItemStockAuditDetailModel();                           
        //                    Row r = rows[i];
        //                    foreach (DocumentFormat.OpenXml.Spreadsheet.Cell c in r.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>())
        //                    {                                
        //                        var match = regex.Match(c.CellReference);
        //                        string strColumnName = match.Value;
        //                        string strText = "";
        //                        if (strColumnName == "A")
        //                        {
        //                            if (c.CellValue != null)
        //                            {
        //                                strText = c.CellValue.InnerXml;
        //                                strText = workbookPart.SharedStringTablePart.SharedStringTable
        //                                     .Elements<SharedStringItem>().ElementAt(
        //                                     Convert.ToInt32(c.CellValue.InnerText)).InnerText;
        //                                Model.MPN = strText;
        //                            }
        //                            else
        //                            {
        //                                Allow = false;
        //                                break;
        //                            }
        //                        }
        //                        else if (strColumnName == "B")
        //                        {
        //                            if (c.CellValue != null)
        //                            {
        //                                if (Convert.ToInt32(c.CellValue.InnerXml) > 0)
        //                                {
        //                                    strText = c.CellValue.InnerXml;
        //                                    Model.Quantity = Convert.ToInt32(strText);
        //                                }
        //                                else
        //                                {
        //                                    Allow = false;
        //                                    break;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                Allow = false;
        //                                break;
        //                            }
        //                        }
        //                        else if (strColumnName == "C")
        //                        {
        //                            if (c.CellValue != null)
        //                            {
        //                                strText = c.CellValue.InnerXml;
        //                                Model.LocationId = Convert.ToInt32(strText);
        //                            }
        //                            else
        //                            {
        //                                Model.LocationId = 0;
        //                                Model.LocationIdDesc = "Location Not Define";
        //                            }
        //                        }
        //                    }
        //                    if(Allow)
        //                    Lststock.Add(Model);
        //                }
        //                document.Close();
        //                System.IO.File.Delete(filepath);
        //            }
        //        }
        //        Lststock = BLObj.CheckStockExcelItem(Lststock);
        //        Session["StockAuditExcelData"] = Lststock;
        //        return Json(Lststock,JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex, JsonRequestBehavior.AllowGet);
        //    }
        //}
        public ActionResult StockExcelFile()
        {
            List<ItemStockAuditDetailModel> Lststock = new List<ItemStockAuditDetailModel>();
            Lststock = Session["StockAuditItems"] as List<ItemStockAuditDetailModel>;
            EnquiryBL BLObj = new EnquiryBL();
            try
            {
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
                        var sheet = sheets.FirstOrDefault();
                        var workSheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;
                        var columns = workSheet.Descendants<Columns>().FirstOrDefault();
                        var sheetdata = workSheet.Elements<SheetData>().Last();
                        rows = sheetdata.Elements<Row>().ToList();
                        var regex = new Regex("[A-Za-z]+");
                        for (int i = 1; i < rows.Count; i++)
                        {
                            bool Allow = true;
                            ItemStockAuditDetailModel Model = new ItemStockAuditDetailModel();
                            Row r = rows[i];
                            foreach (DocumentFormat.OpenXml.Spreadsheet.Cell c in r.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>())
                            {
                                var match = regex.Match(c.CellReference);
                                string strColumnName = match.Value;
                                string strText = "";
                                if (strColumnName == "E")
                                {
                                    if (c.CellValue != null)
                                    {
                                        strText = c.CellValue.InnerXml;
                                        strText = workbookPart.SharedStringTablePart.SharedStringTable
                                             .Elements<SharedStringItem>().ElementAt(
                                             Convert.ToInt32(c.CellValue.InnerText)).InnerText;
                                        Model.MPN = strText;
                                    }
                                    else
                                    {
                                        Allow = false;
                                        break;
                                    }
                                }
                                else if (strColumnName == "K")
                                {
                                    if (c.CellValue != null)
                                    {
                                        if (Convert.ToInt32(c.CellValue.InnerXml) > 0)
                                        {
                                            strText = c.CellValue.InnerXml;
                                            Model.Quantity = Convert.ToInt32(strText);
                                        }
                                        else
                                        {
                                            Allow = false;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Allow = false;
                                        break;
                                    }
                                }
                                else if (strColumnName == "O")
                                {
                                    if (c.CellValue != null)
                                    {
                                        strText = c.CellValue.InnerXml;
                                        Model.LocationId = Convert.ToInt32(strText);
                                    }
                                    else
                                    {
                                        Model.LocationId = 0;
                                        Model.LocationIdDesc = "Location Not Define";
                                    }
                                }
                            }
                            if (Allow)
                                Lststock.Add(Model);
                        }
                        document.Close();
                        System.IO.File.Delete(filepath);
                    }
                }
                Lststock = BLObj.CheckStockExcelItem(Lststock);
                Session["StockAuditExcelData"] = Lststock;
                return Json(Lststock, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveDataFromExcel(int AuditId)
        {
            try
            {
                EnquiryBL BLObj = new EnquiryBL();
                List<ItemStockAuditDetailModel> Lststock = new List<ItemStockAuditDetailModel>();
                Lststock = Session["StockAuditExcelData"] as List<ItemStockAuditDetailModel>;
                string user_Id = User.Identity.GetUserId();
                int errCode = BLObj.SaveStockAuditExcelData(Lststock, AuditId, user_Id);
                ItemStockAuditModel AuditModel = new ItemStockAuditModel();
                AuditModel = BLObj.GetSelectedItemStockAuditList(AuditId);
                Session["StockAuditExcelData"] = null;
                Session["StockAuditItems"] = null;
                Session["StockAuditItems"] = AuditModel.DetailList;
                return Json(AuditModel.DetailList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public FileResult DownloadItemStocksample()
        {
            var filepath = Path.Combine(Server.MapPath("~/App_Data/MyFiles/SampleItemStock.xlsx"));
            string fileName = "SampleItemStock.xlsx";
            return File(filepath, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        #endregion Item Stock Audit
        public ActionResult EnqAndQuotItemDetailReport()
        {
            EnquiryModel EnquModel = new EnquiryModel();
            try
            {
                string User_Id = User.Identity.GetUserId();
                List<AdhocModel> lstAdhocModel = new List<AdhocModel>();
                AdhocBL ObjBL = new AdhocBL();
                lstAdhocModel = ObjBL.GetEnqAndQuotItemReportList(User_Id);
                ViewBag.ReportList = lstAdhocModel;

                paramMadal prmmodParamDemo = new paramMadal();                
                prmmodParamDemo.lstAdhocParam = new List<paramMadal>();
                paramMadal prmmodParam = new paramMadal();
                EnquiryBL BLObj = new EnquiryBL();
                if (Session["ParamDescrip"] != null)
                {
                    prmmodParam = Session["ParamDescrip"] as paramMadal;
                }
                string TxtParamValue = "";
                prmmodParam.ParamName = "@userID";
                prmmodParam.ParamValue = User_Id;
                prmmodParamDemo.lstAdhocParam.Add(prmmodParam);
                Session["ParamDescrip"] = prmmodParamDemo;
                EnquModel.ParamId = "";
                EnquModel.ParamName = "";
                EnquModel.hidText = "";
                EnquModel.ReportId = "MGMT015";
                EnquModel.TxtParamValue = TxtParamValue;
                ReportNameModel Model = new ReportNameModel();
                Model = BLObj.GetOpenEnquiryQuotItemReport(User_Id);
                EnquModel.OutputLocation = Model.OutputLocation;
                EnquModel.RunDate = Model.RunDate;
                EnquModel.StatusId = Model.StatusId;
                EnquModel.ReportId = Model.ReportId;
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
            return View(EnquModel);
        }
    }
}