using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.Bank;
using SmartSys.BL.BOMAdmin;
using SmartSys.BL.DW;
using SmartSys.BL.EmailProcessing;
using SmartSys.BL.Project;
using SmartSys.BL.TimeManagement;
using SmartSys.Utility;
using Syncfusion.EJ.Export;
using Syncfusion.JavaScript.Models;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SmartSys.Controllers
{
    public class BOMAdminController : Controller
    {
        // GET: BOMAdmin
        #region Paymentterms
        public ActionResult GetPTList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BOMAdmin/GetPTList");
            if (FindFlag)
            {
                List<Paytermsmodel> PTList = new List<Paytermsmodel>();
                BOMAdminBL objbl = new BOMAdminBL();
                PTList = objbl.GetPaymentTermsList(UserId);
                ViewBag.Paymentterms = PTList;
                return View(PTList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportPaymentLst")]
        [AcceptVerbs("POST")]
        public void ExportPaymentLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            BOMAdminBL objbl = new BOMAdminBL();
            var DataSource = objbl.GetPaymentTermsList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreatePT(int PTId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BOMAdmin/CreatePT?PTId=0");
            if (FindFlag)
            {
                BOMAdminBL objbl = new BOMAdminBL();
                Paytermsmodel paytermsmodel = new Paytermsmodel();
                List<Paytermsmodel> PTList = new List<Paytermsmodel>();
                if (PTId > 0)
                {
                    paytermsmodel = objbl.GetSelectedPaymentTerms(PTId);
                }


                return View(paytermsmodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreatePT(FormCollection fc)
        {
            Paytermsmodel PTmodel = new Paytermsmodel();
            BOMAdminBL objbl = new BOMAdminBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                if (fc["PTId"].ToString() != "0")
                {
                    PTmodel.PTId = Convert.ToInt32(fc["PTId"].ToString());
                }
                else
                {
                    PTmodel.PTId = 0;
                }

                PTmodel.PTCode = fc["PTCode"].ToString();
                PTmodel.Description = fc["Description"].ToString();
                int errorcode = objbl.SavePaymentTerms(PTmodel, User_Id);
                if (errorcode == 500001 || errorcode == 500002)
                {
                    return RedirectToAction("GetPTList");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        #endregion Paymentterms

        #region DW Brand Master
        public ActionResult BrandList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BOMAdmin/BrandList");
            if (FindFlag)
            {
                BOMAdminBL BLObj = new BOMAdminBL();
                List<VendorBrandModel> Brandmodel = new List<VendorBrandModel>();
                try
                {
                    Brandmodel = BLObj.GetBrandList(UserId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(Brandmodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportToExcelBrandList")]
        [AcceptVerbs("POST")]
        public void ExportToExcelBrandList(string GridModel)
        {
            BOMAdminBL BLObj = new BOMAdminBL();
            string User_Id = User.Identity.GetUserId();
            var DataSource = BLObj.GetBrandList(User_Id);
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateBrand(int BrandId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BOMAdmin/CreateBrand?BrandId=0");
            if (FindFlag)
            {
                BOMAdminBL BLObj = new BOMAdminBL();
                VendorBrandModel BrandModel = new VendorBrandModel();
                try
                {
                    if (BrandId > 0)
                    {
                        BrandModel = BLObj.GetSelectedBrandList(BrandId);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(BrandModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult CreateBrand(FormCollection fc)
        {
            VendorBrandModel BrandModel = new VendorBrandModel();
            string UserId = User.Identity.GetUserId();
            BOMAdminBL BLObj = new BOMAdminBL();
            int ErrCode = 0;
            try
            {
                BrandModel.BrandId = Convert.ToInt32(fc["BrandId"].ToString());
                BrandModel.BrandName = fc["BrandName"].ToString();
                ErrCode = BLObj.SaveBrandDetail(UserId, BrandModel.BrandId, BrandModel.BrandName);
                if (ErrCode == 500001 || ErrCode == 500002)
                {
                    return RedirectToAction("BrandList");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("BrandList");
        }
        #endregion DW Breand master

        #region Item Brand
        public ActionResult AllocateBrandItem(int BrandId, string BrandName)
        {
            BrandItemModel BrandItemModel = new BrandItemModel();
            BOMAdminBL BLObj = new BOMAdminBL();
            try
            {
                BrandItemModel = BLObj.GetBrandItemAllocateList(BrandId);
                BrandItemModel.BrandId = BrandId;
                BrandItemModel.BrandName = BrandName;
                ViewBag.UnAllocateItem = new SelectList(BrandItemModel.UnAllocateItemList, "ItemId", "ItemName");
                ViewBag.AllocateItem = new SelectList(BrandItemModel.AllocateItemList, "ItemId", "ItemName");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(BrandItemModel);
        }
        [HttpPost]
        public ActionResult AllocateBrandItem(FormCollection fc)
        {
            string User_Id = User.Identity.GetUserId();
            int ErrCode = 0;
            int BrandId = 0;
            BrandItemModel BrandItemModel = new BrandItemModel();
            BrandItemModel.AllocateItemList = new List<BrandItemModel>();
            foreach (string Key in fc.Keys)
            {
                if (Key.ToString() == "AllocateItem")
                {
                    List<string> LstAllocateBrand = fc["AllocateItem"].Split(',').ToList();
                    BrandId = Convert.ToInt32(fc["BrandId"]);

                    foreach (var str in LstAllocateBrand)
                    {
                        BrandItemModel BrandModel = new BrandItemModel();
                        BrandModel.ItemId = Convert.ToInt32(str);
                        BrandModel.BrandId = Convert.ToInt32(BrandId);
                        BrandItemModel.AllocateItemList.Add(BrandModel);
                    }
                }
            }
            BOMAdminBL BLObj = new BOMAdminBL();
            ErrCode = BLObj.SaveAllocatedBrandItem(BrandItemModel, User_Id, BrandId);
            return RedirectToAction("BrandList");
        }
        #endregion Item Brand

        #region Item Brand
        public ActionResult AllocateBrandVendor(int BrandId, string BrandName)
        {
            BrandVendorModel BrandVendorModel = new BrandVendorModel();
            BOMAdminBL BLObj = new BOMAdminBL();
            try
            {
                BrandVendorModel = BLObj.GetBrandVendorAllocateList(BrandId);
                BrandVendorModel.BrandId = BrandId;
                BrandVendorModel.BrandName = BrandName;
                ViewBag.UnAllocateVendor = new SelectList(BrandVendorModel.UnAllocateVendorList, "VendorId", "VendorName");
                ViewBag.AllocateVendor = new SelectList(BrandVendorModel.AllocateVendorList, "VendorId", "VendorName");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(BrandVendorModel);
        }
        [HttpPost]
        public ActionResult AllocateBrandVendor(FormCollection fc)
        {
            string User_Id = User.Identity.GetUserId();
            int ErrCode = 0;
            int BrandId = 0;
            BrandVendorModel BrandVendorModel = new BrandVendorModel();
            BrandVendorModel.AllocateVendorList = new List<BrandVendorModel>();
            foreach (string Key in fc.Keys)
            {
                if (Key.ToString() == "AllocateVendor")
                {
                    List<string> LstAllocateBrand = fc["AllocateVendor"].Split(',').ToList();
                    BrandId = Convert.ToInt32(fc["BrandId"]);

                    foreach (var str in LstAllocateBrand)
                    {
                        BrandVendorModel BrandModel = new BrandVendorModel();
                        BrandModel.VendorId = Convert.ToInt32(str);
                        BrandModel.BrandId = Convert.ToInt32(BrandId);
                        BrandVendorModel.AllocateVendorList.Add(BrandModel);
                    }
                }
            }
            BOMAdminBL BLObj = new BOMAdminBL();
            ErrCode = BLObj.SaveAllocatedBrandVendor(BrandVendorModel, User_Id, BrandId);
            return RedirectToAction("BrandList");
        }
        #endregion Item Brand

        #region EmailProcessor
        public ActionResult EmailProcessorList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BOMAdmin/EmailProcessorList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                BOMAdminBL BLObj = new BOMAdminBL();
                mailmodel model = BLObj.GetMailProcessingDetail(User_Id);
                List<mailmodel> MailList = model.MailList;
                List<mailmodel> Emailconfig = model.MailConfig;
                ViewBag.Message = "You Don't Have Email Configuration Detail ,Please Talk to Your Manager";
                foreach (var item in Emailconfig)
                {
                    string MailDetail = item.MailDetail;
                    if (MailDetail != "")
                    {
                        ViewBag.Message = "";
                        break;
                    }

                }
                List<DrpItem> CUstomer = new List<DrpItem>();
                ProjectBL ObjBl = new ProjectBL();
                CUstomer = ObjBl.GetCustomerListByUser(User_Id);
                ViewBag.CustList = new SelectList(CUstomer, "Id", "DisplayName");
                return View(MailList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportToExcelPendingEmailSegregation")]
        [AcceptVerbs("POST")]
        public void ExportToExcelPendingEmailSegregation(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            BOMAdminBL BLObj = new BOMAdminBL();
            mailmodel model = BLObj.GetMailProcessingDetail(User_Id);
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = model.MailList;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult SubordinateEmailProcessorList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BOMAdmin/EmailProcessorList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                List<DrpItem> CUstomer = new List<DrpItem>();
                ProjectBL ObjBl = new ProjectBL();
                CUstomer = ObjBl.GetCustomerListByUser(User_Id);
                ViewBag.CustList = new SelectList(CUstomer, "Id", "DisplayName");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult GetSubordinateData()
        {
            string User_Id = User.Identity.GetUserId();
            BOMAdminBL BLObj = new BOMAdminBL();
            try
            {
                mailmodel model = BLObj.GetSubordinateMailProcessingDetail(User_Id);
                IEnumerable data = model.MailList;
                var jsonResult = Json(data, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Filter { get; set; }
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

        public JsonResult getPODetail(int CustomerId)
        {
            List<DrpItem> Quotation = new List<DrpItem>();
            ProjectBL ObjBl = new ProjectBL();
            Quotation = ObjBl.GetQuotationlistForCustomer(CustomerId);
            return Json(new SelectList(Quotation, "Id", "DisplayName"));

        }
        public ActionResult UpdateQuotStatus(int QuotId, int MailId)
        {
            ProjectBL ObjBl = new ProjectBL();
            BOMAdminBL BLObj = new BOMAdminBL();
            string User_Id = User.Identity.GetUserId();
            int QuotnId = ObjBl.UpdateQuotStatus(QuotId, User_Id);
            int Errorcode = BLObj.UpdateEmailProcessing(52, MailId, 0, "", User_Id, "CustResp", 0);
            return RedirectToAction("EmailProcessorList", "BOMAdmin");

        }
        public ActionResult UpdateEmailStatus(int CustomerId, int MailId, int Status, string Enqop, int CustomerContactId)
        {

            string enqnumber = "ENQ" + MailId;
            BOMAdminBL BLObj = new BOMAdminBL();
            Session["ENQOP"] = null;
            Session["ENQOP"] = Enqop;
            string User_Id = User.Identity.GetUserId();
            int Errorcode = BLObj.UpdateEmailProcessing(Status, MailId, CustomerId, enqnumber, User_Id, "Enq", CustomerContactId);
            string Key = (string)Session["ENQOP"];
            if (Key == "SaveEdit" && Errorcode != 50000 && Errorcode != 0)
            {
                return RedirectToAction("CretaeCustomerEnquiry", "Enquiry", new { EnqId = Errorcode });
            }
            else
            {
                return RedirectToAction("EmailProcessorList");
            }
        }
        public ActionResult UpdatenonEnquiry(int Status, int MailId)
        {
            BOMAdminBL BLObj = new BOMAdminBL();
            string User_Id = User.Identity.GetUserId();
            int Errorcode = BLObj.UpdateEmailProcessing(Status, MailId, 0, "", User_Id, "NonEnq", 0);
            return RedirectToAction("EmailProcessorList");
        }
        public JsonResult GetmailParameter()
        {
            mailmodel mailmdl = new mailmodel();
            return Json(mailmdl, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewDetail(int ClientMailId, int EmpId)
        {
            mailmodel mailmdl = new mailmodel();
            BOMAdminBL BLObj = new BOMAdminBL();
            try
            {
                mailmdl = BLObj.GetSelectedMailDetail(ClientMailId, EmpId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }

            return PartialView(mailmdl);
        }
        #endregion EmailProcessor

        #region Bank Cheque

        public ActionResult GetBankChequeList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BOMAdmin/GetBankChequeList");
            if (FindFlag)
            {
                List<BankChequeModel> ChequeList = new List<BankChequeModel>();
                try
                {
                    BOMAdminBL objbl = new BOMAdminBL();
                    ChequeList = objbl.GetBankChequeList(UserId);
                    if (Session["Alert"] != null)
                    {
                        TempData["alert"] = Session["Alert"] as string;
                        Session["Alert"] = null;
                    }


                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(ChequeList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportToExceltoBankCheque")]
        [AcceptVerbs("POST")]
        public void ExportToExceltoBankCheque(string GridModel)
        {

            BOMAdminBL objbl = new BOMAdminBL();
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            string User_Id = User.Identity.GetUserId();
            var DataSource = objbl.GetBankChequeList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateBankCheque(int ChequeId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BOMAdmin/CreateBankCheque?ChequeId=0");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                BudgetModel budgetModel = new BudgetModel();
                BankChequeModel Chequemodel = new BankChequeModel();
                BOMAdminBL objbl = new BOMAdminBL();
                try
                {
                    ProjectBL Blobj = new ProjectBL();
                    budgetModel.EmployeeLst = new List<BudgetCustmerModel>();
                    AdminBL objBL = new AdminBL();
                    budgetModel.EmployeeLst = objBL.GetBudgetEmployeeList(User_Id); // Employee List
                    ViewBag.EmployeeList = new SelectList(budgetModel.EmployeeLst, "EmployeeId", "EmployeeName");
                    ViewBag.CustomerList = new SelectList(Blobj.GetCustomerListByUser(User_Id), "Id", "DisplayName");
                    ViewBag.VendorList = new SelectList(Blobj.GetVendorListByUser(User_Id), "Id", "DisplayName");
                    List<FreightForwarderModel> lstf = new List<FreightForwarderModel>();
                    VendorBL vendorbl = new VendorBL();
                    lstf = vendorbl.FreightForvordarList();
                    ViewBag.FFLst = new SelectList(lstf, "FFId", "VendorName");
                    BankDetailModel objmodel = new BankDetailModel();
                    objmodel.BankDetailList = new List<BankDetailModel>();
                    BankBL obj = new BankBL();
                    objmodel.BankDetailList = obj.bankDetailList(User_Id);
                    ViewBag.BankLst = new SelectList(objmodel.BankDetailList, "BankId", "BankName");
                    TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                    List<CurrencyCodes> lstModel = new List<CurrencyCodes>();
                    lstModel = ObjTMBL.GetCurrencyCodes();
                    ViewBag.Currency = new SelectList(lstModel, "Key", "Value");
                    if (ChequeId > 0)
                    {
                        Chequemodel = objbl.GetSelectedBankChequeList(ChequeId);
                        // Session["ChequeDocument"] = Chequemodel.ChequeDocument;
                    }
                    ItemMappingModel ItemMap = new ItemMappingModel();
                    ItemBL BlObj = new ItemBL();
                    ItemMap.lstItemMap = BlObj.GetDWCompList();
                    ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");
                }


                catch (Exception ex)
                {
                    throw ex;
                }
                return View(Chequemodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateBankCheque(FormCollection fc, HttpPostedFileBase file)
        {
            int ErrCode = 0;
            string User_Id = User.Identity.GetUserId();
            BankChequeModel Chequemodel = new BankChequeModel();
            BOMAdminBL objbl = new BOMAdminBL();
            try
            {
                string ftpServer = Common.GetConfigValue("FTP");
                string ftpUid = Common.GetConfigValue("FTPUid");
                string ftpPwd = Common.GetConfigValue("FTPPWD");
                string fileName = "";
                if (Convert.ToInt32(fc["ChequeId"]) > 0)
                {
                    if (file != null && fc["ChequeDocument"].ToString() != "")
                    {

                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpServer + fc["ChequeDocument"]);
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
                            string FileEx = FileSplit[FileSplit.Length - 1].ToString();
                            String guid = Guid.NewGuid().ToString();
                            string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                            string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                            string FileName = Convert.ToString("/BankCheque/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
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
                                    requestFTPUploader = null;
                                    Chequemodel.ChequeDocument = FileName;
                                    break;

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                else
                {
                    Chequemodel.ChequeDocument = fc["ChequeDocument"].ToString();
                }

                Chequemodel.ChequeId = Convert.ToInt32(fc["ChequeId"]);
                Chequemodel.CompCode = fc["CompCode"];
                Chequemodel.BankId = Convert.ToInt32(fc["BankId"]);
                Chequemodel.ChequeNumber = fc["ChequeNumber"].ToString();
                if (fc["ChequeDate"] != "")
                    Chequemodel.ChequeDate = Convert.ToDateTime(fc["ChequeDate"]);
                Chequemodel.IssuedToType = fc["IssuedToType"].ToString();
                switch (Chequemodel.IssuedToType)
                {
                    case "Employee":
                        {
                            Chequemodel.IssuedToId = Convert.ToInt32(fc["IssuedToIdEmp"]);
                            Chequemodel.IssuedToOther = "";
                            break;
                        }

                    case "Vendor":
                        {
                            Chequemodel.IssuedToId = Convert.ToInt32(fc["IssuedToIdVend"]);
                            Chequemodel.IssuedToOther = "";
                            break;
                        }

                    case "Customer":
                        {
                            Chequemodel.IssuedToId = Convert.ToInt32(fc["IssuedToIdCust"]);
                            Chequemodel.IssuedToOther = "";
                            break;
                        }

                    case "FF":
                        {
                            Chequemodel.IssuedToId = Convert.ToInt32(fc["IssuedToIdFF"]);
                            Chequemodel.IssuedToOther = "";
                            break;
                        }

                    case "Other":
                        {
                            Chequemodel.IssuedToId = 0;
                            Chequemodel.IssuedToOther = fc["IssuedToOther"].ToString();
                            break;
                        }
                }
                if (fc["IssuedDate"] != "")
                    Chequemodel.IssuedDate = Convert.ToDateTime(fc["IssuedDate"]);
                Chequemodel.IssuedToOther = fc["IssuedToOther"].ToString();
                if (fc["Amount"] != "")
                    Chequemodel.Amount = Convert.ToDouble(fc["Amount"]);
                Chequemodel.Remark = fc["Remark"].ToString();
                Chequemodel.Currency = fc["Currency"].ToString();
                if (fc["ClearingDate"] != "")
                    Chequemodel.ClearingDate = Convert.ToDateTime(fc["ClearingDate"]);
                ErrCode = objbl.SaveBankChequeDetail(Chequemodel, User_Id);
                if (ErrCode != 0)
                {
                    Session["Alert"] = "Saved SucessFully.....";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetBankChequeList");
        }
        #endregion  Bank Cheque

        #region TaxonTax
        public ActionResult GetTaxList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BOMAdmin/GetTaxList");
            if (FindFlag)
            {
                List<TaxModel> lsttax = new List<TaxModel>();
                try
                {
                    BOMAdminBL taxbl = new BOMAdminBL();
                    lsttax = taxbl.TaxList(UserId);
                    if (Session["taxmodel"] != null)
                    {
                        Session["taxmodel"] = null;
                    }
                    if (Session["TaxType"] != null)
                    {
                        Session["TaxType"] = null;
                    }
                    if (Session["TaxMaster"] != null)
                    {
                        Session["TaxMaster"] = null;
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(lsttax);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportTaxLst")]
        [AcceptVerbs("POST")]
        public void ExportTaxLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            BOMAdminBL taxbl = new BOMAdminBL();   
            var DataSource = taxbl.TaxList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateTax(int TaxId)
        {
            TaxModel objmodel = new TaxModel();
            List<TaxTypeModel> lsttax = new List<TaxTypeModel>();
            objmodel.TaxList = new List<TaxModel>();
            BOMAdminBL taxbl = new BOMAdminBL();
            try
            {
                if (TaxId > 0)
                {
                    objmodel = taxbl.GetSelectedTaxDetail(TaxId);
                    if (Session["TaxType"] != null)
                    {
                        objmodel.TaxType = Session["TaxType"] as string;
                    }
                }
                lsttax = taxbl.GetTaxTypeList();
                ViewBag.GetTypeList = new SelectList(lsttax, "TaxType", "TaxTypeTitle");
                objmodel.TaxList = taxbl.TaxOnTaxLst(TaxId);
                ViewBag.taxdetaillist = new SelectList(objmodel.TaxList, "TaxId", "TaxName");
                if (objmodel.TaxGrid == null)
                    objmodel.TaxGrid = new List<TaxOnTaxGrid>();
                if (Session["TaxMaster"] != null)
                {
                    objmodel = Session["TaxMaster"] as TaxModel;
                    ViewBag.taxdetaillist = new SelectList(objmodel.TaxList, "TaxId", "TaxName");
                }
                else
                {
                    if (TaxId > 0)
                    {
                        objmodel.TaxOnTaxList = new List<TaxOnTax>();      // this two line of code
                        objmodel.TaxOnTaxList = taxbl.TaxOnTaxList(TaxId);
                        if (Session["TaxMaster"] != null)
                        {
                            objmodel = Session["TaxMaster"] as TaxModel;
                        }
                        if (objmodel.TaxOnTaxList == null)
                            objmodel.TaxOnTaxList = new List<TaxOnTax>();
                        objmodel.TaxId = TaxId;

                    }
                }

                Session["TaxMaster"] = objmodel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(objmodel);
        }
        [HttpPost]
        public ActionResult CreateTax(FormCollection fc, int TaxId)
        {
            int errorcode = 0;
            TaxModel taxmodel = new TaxModel();
            string User_Id = User.Identity.GetUserId();
            try
            {
                taxmodel.TaxId = 0;
                if (TaxId > 0)
                {
                    taxmodel.TaxId = TaxId;
                }
                if (Session["taxmodel"] != null)
                {
                    taxmodel = Session["taxmodel"] as TaxModel;

                }
                if (Session["TaxMaster"] != null)
                {
                    taxmodel = Session["TaxMaster"] as TaxModel;
                }

                taxmodel.TaxName = fc["TaxName"].ToString();
                taxmodel.TaxType = fc["TaxType"].ToString();
                taxmodel.TaxRate = Convert.ToDouble(fc["TaxRate"].ToString());
                BOMAdminBL taxbl = new BOMAdminBL();
                errorcode = taxbl.SaveTaxDetail(taxmodel, User_Id);
                if (errorcode == 4)
                {
                    Session["taxmodel"] = null;
                    return RedirectToAction("GetTaxList");
                }
                else

                    TempData["Msg"] = " Occur some Error Please Try Again ...";
                return RedirectToAction("GetTaxList");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult AddTaxOnTaxLIst(int Taxid, int ChildTaxId, string TaxType, string TaxName, string TaxOnTax, double TaxValue)
        {
            try
            {
                TaxModel taxmodel = new TaxModel();
                BOMAdminBL objbl = new BOMAdminBL();
                taxmodel.TaxOnTaxList = new List<TaxOnTax>();

                taxmodel.TaxOnTaxList = new List<TaxOnTax>();      // this two line of code
                taxmodel.TaxOnTaxList = objbl.TaxOnTaxList(Taxid);

                if (Session["TaxMaster"] != null)
                {
                    taxmodel = Session["TaxMaster"] as TaxModel;
                }

                Session["TaxType"] = TaxType;

                TaxOnTax taxontaxgrid = new TaxOnTax();
                taxontaxgrid.TaxId = Taxid;
                taxontaxgrid.TaxName = TaxName;
                taxontaxgrid.ChildTaxId = ChildTaxId;
                taxontaxgrid.ParentTaxName = TaxOnTax;


                taxmodel.TaxOnTaxList.Add(taxontaxgrid);
                taxmodel.TaxId = Taxid;
                taxmodel.TaxType = TaxType;
                taxmodel.TaxName = TaxName;
                taxmodel.TaxRate = TaxValue;

                var actionToDelete = from actionRepDel in taxmodel.TaxList.Where(s => (s.TaxId == ChildTaxId))
                                     where (actionRepDel.TaxId == ChildTaxId)
                                     select actionRepDel;
                if (actionToDelete.Count() > 0)
                {
                    taxmodel.TaxList.Remove(actionToDelete.ElementAt(0));

                }
                Session["TaxMaster"] = taxmodel;
                return RedirectToAction("CreateTax", new { TaxId = taxontaxgrid.TaxId });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteTaxOnTaxDetails(int Taxid, int ChildTaxId, string ChildTaxName)
        {
            try
            {
                BOMAdminBL objbl = new BOMAdminBL();
                if (Session["TaxMaster"] != null)
                {
                    TaxModel taxmodel = new TaxModel();
                    TaxOnTax taxontaxgrid = new TaxOnTax();
                    taxmodel = Session["TaxMaster"] as TaxModel;

                    var actionToDelete = from actionRepDel in taxmodel.TaxOnTaxList
                                         where actionRepDel.ChildTaxId == ChildTaxId
                                         select actionRepDel;
                    taxmodel.TaxOnTaxList.Remove(actionToDelete.ElementAt(0));
                    TaxModel TaxLst = new TaxModel();
                    TaxLst.TaxId = ChildTaxId;
                    TaxLst.TaxName = ChildTaxName;
                    taxmodel.TaxList.Add(TaxLst);

                    Session["TaxMaster"] = taxmodel;
                }

                return RedirectToAction("CreateTax", new { TaxId = Taxid });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AddTaxOnTaxLIstCreate(string TaxName, string TaxTypeId, int TaxOnTaxId, string TaxType, string TaxOnTax, double TaxValue)
        {

            TaxModel taxmodel = new TaxModel();
            taxmodel.TaxGrid = new List<TaxOnTaxGrid>();
            if (Session["TaxMaster"] != null)
            {
                taxmodel = Session["TaxMaster"] as TaxModel;
            }

            taxmodel.TaxName = TaxName;
            taxmodel.TaxRate = TaxValue;
            taxmodel.TaxType = TaxTypeId;
            TaxOnTaxGrid taxgrid = new TaxOnTaxGrid();
            taxgrid.TaxId = 0;
            taxgrid.TaxName = TaxName;
            taxgrid.TaxTypeId = TaxTypeId;
            taxgrid.TaxType = TaxType;
            taxgrid.TaxOnTaxId = TaxOnTaxId;
            taxgrid.TaxOnTax = TaxOnTax;
            taxmodel.TaxGrid.Add(taxgrid);

            var actionToDelete = from actionRepDel in taxmodel.TaxList.Where(s => (s.TaxId == TaxOnTaxId))
                                 where (actionRepDel.TaxId == TaxOnTaxId)
                                 select actionRepDel;
            if (actionToDelete.Count() > 0)
            {
                taxmodel.TaxList.Remove(actionToDelete.ElementAt(0));

            }
            Session["TaxMaster"] = null;
            Session["TaxMaster"] = taxmodel;
            return RedirectToAction("CreateTax", new { TaxId = taxgrid.TaxId });
        }

        public ActionResult DeleteEventAction(int TaxOnTaxId, string TaxOnTaxName)
        {
            TaxModel taxmodel = new TaxModel();
            if (Session["TaxMaster"] != null)
            {
                taxmodel = Session["TaxMaster"] as TaxModel;
            }

            var actionToDelete = from actionRepDel in taxmodel.TaxGrid
                                 where actionRepDel.TaxOnTaxId == TaxOnTaxId
                                 select actionRepDel;
            taxmodel.TaxGrid.Remove(actionToDelete.ElementAt(0));
            TaxModel TaxLst = new TaxModel();
            TaxLst.TaxId = TaxOnTaxId;
            TaxLst.TaxName = TaxOnTaxName;
            taxmodel.TaxList.Add(TaxLst);
            Session["TaxMaster"] = taxmodel;
            return RedirectToAction("CreateTax", new { TaxId = 0 });
        }

        #endregion TaxonTax

        #region Inward Document

        public ActionResult GetInwardDocumentList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BOMAdmin/GetInwardDocumentList");
            if (FindFlag)
            {
                List<InwardDocumentModel> ChequeList = new List<InwardDocumentModel>();
                try
                {
                    BOMAdminBL objbl = new BOMAdminBL();
                    ChequeList = objbl.GetInwardDocumentList(UserId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(ChequeList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportInwardDocumentList")]
        [AcceptVerbs("POST")]
        public void ExportInwardDocumentList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            BOMAdminBL objbl = new BOMAdminBL();
            var DataSource = objbl.GetInwardDocumentList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateInwardDocument(int DocId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BOMAdmin/CreateInwardDocument?DocId=0");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                BudgetModel budgetModel = new BudgetModel();
                InwardDocumentModel Chequemodel = new InwardDocumentModel();
                Chequemodel.DocDetailList = new List<InwardDocumentDetailModel>();
                BOMAdminBL objbl = new BOMAdminBL();
                try
                {
                    ProjectBL Blobj = new ProjectBL();
                    budgetModel.EmployeeLst = new List<BudgetCustmerModel>();
                    AdminBL objBL = new AdminBL();
                    budgetModel.EmployeeLst = objBL.GetBudgetEmployeeList(User_Id); // Employee List
                    ViewBag.EmployeeList = new SelectList(budgetModel.EmployeeLst, "EmployeeId", "EmployeeName");
                    ViewBag.CustomerList = new SelectList(Blobj.GetCustomerListByUser(User_Id), "Id", "DisplayName");
                    ViewBag.VendorList = new SelectList(Blobj.GetVendorListByUser(User_Id), "Id", "DisplayName");

                    AdminBL BlObj = new AdminBL();
                    EmoloyeeMapping EmpMap = new EmoloyeeMapping();
                    EmpMap.lstEmolyeeMap = BlObj.GetDWCompList();
                    ViewBag.EWComapnyList = new SelectList(EmpMap.lstEmolyeeMap, "CompCode", "CompanyName");

                    List<FreightForwarderModel> lstf = new List<FreightForwarderModel>();
                    VendorBL vendorbl = new VendorBL();
                    lstf = vendorbl.FreightForvordarList();
                    ViewBag.FFLst = new SelectList(lstf, "FFId", "VendorName");
                    BankDetailModel objmodel = new BankDetailModel();
                    objmodel.BankDetailList = new List<BankDetailModel>();
                    BankBL obj = new BankBL();
                    objmodel.BankDetailList = obj.bankDetailList(User_Id);
                    ViewBag.BankLst = new SelectList(objmodel.BankDetailList, "BankId", "BankName");
                    TMEquipmentBL ObjTMBL = new TMEquipmentBL();
                    List<CurrencyCodes> lstModel = new List<CurrencyCodes>();
                    lstModel = ObjTMBL.GetCurrencyCodes();
                    ViewBag.Currency = new SelectList(lstModel, "Key", "Value");

                    List<StockLocation> SLlst = new List<StockLocation>();
                    SLlst = objbl.GetStockLocationListForInward();
                    ViewBag.StockLocation = new SelectList(SLlst, "STLocationId", "Name");
                    if (DocId > 0)
                    {
                        Chequemodel = objbl.GetSelectedInwardDocumentList(DocId);
                    }
                }


                catch (Exception ex)
                {
                    throw ex;
                }
                return View(Chequemodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateInwardDocument(FormCollection fc)
        {
            int ErrCode = 0;
            string User_Id = User.Identity.GetUserId();
            InwardDocumentModel Chequemodel = new InwardDocumentModel();
            BOMAdminBL objbl = new BOMAdminBL();
            try
            {
                Chequemodel.DocId = Convert.ToInt32(fc["DocId"]);
                Chequemodel.STLocationId = Convert.ToInt32(fc["STLocationId"]);
                Chequemodel.DocName = fc["DocName"].ToString();
                Chequemodel.DocToType = fc["DocToType"].ToString();
                switch (Chequemodel.DocToType)
                {
                    case "Employee":
                        {
                            Chequemodel.DocToId = Convert.ToInt32(fc["DocToIdEmp"]);
                            Chequemodel.DocToOther = "";
                            break;
                        }

                    case "Vendor":
                        {
                            Chequemodel.DocToId = Convert.ToInt32(fc["DocToIdVend"]);
                            Chequemodel.DocToOther = "";
                            break;
                        }

                    case "Customer":
                        {
                            Chequemodel.DocToId = Convert.ToInt32(fc["DocToIdCust"]);
                            Chequemodel.DocToOther = "";
                            break;
                        }

                    case "FF":
                        {
                            Chequemodel.DocToId = Convert.ToInt32(fc["DocToIdFF"]);
                            Chequemodel.DocToOther = "";
                            break;
                        }
                    case "Bank":
                        {
                            Chequemodel.DocToId = Convert.ToInt32(fc["DocToIdBank"]);
                            Chequemodel.DocToOther = "";
                            break;
                        }
                    case "Other":
                        {
                            Chequemodel.DocToId = 0;
                            Chequemodel.DocToOther = fc["DocToOther"].ToString();
                            break;
                        }
                }
                if (fc["DocDate"] != "")
                    Chequemodel.DocDate = Convert.ToDateTime(fc["DocDate"]);
                //Chequemodel.DocToOther = fc["DocToOther"].ToString();
                if (fc["Amount"] != "")
                    Chequemodel.Amount = Convert.ToDouble(fc["Amount"]);
                Chequemodel.Currency = fc["Currency"].ToString();
                Chequemodel.NatureOfDoc = fc["NatureOfDocLst"].ToString();
                Chequemodel.CompCode = fc["CompCode"].ToString();
                if (Chequemodel.NatureOfDoc.ToString() == "Other")
                {
                    Chequemodel.NatureOfDoc = fc["NatureOfDocOther"].ToString();
                }
                ErrCode = objbl.SaveInwardDocument(Chequemodel, User_Id);
                if (ErrCode > 0)
                {
                    return RedirectToAction("CreateInwardDocument", new { DocId = ErrCode });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetInwardDocumentList");
        }

        public ActionResult CreateInwardDocDetail(int DocId)
        {
            try
            {
                InwardDocumentDetailModel DocDetailmodel = new InwardDocumentDetailModel();
                DocDetailmodel.DocId = DocId;
                return PartialView(DocDetailmodel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult CreateInwardDocDetail(HttpPostedFileBase file, FormCollection fc)
        {
            InwardDocumentDetailModel Docmodel = new InwardDocumentDetailModel();
            try
            {
                string fileName = Path.GetFileName(file.FileName);
                string ftpServer = Common.GetConfigValue("FTP");
                string[] FileSplit = fileName.Split('.');
                string FileEx = FileSplit[1].ToString();
                String guid = Guid.NewGuid().ToString();
                string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                string FileName = Convert.ToString("/InwardDoc/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;

                Docmodel.DocId = Convert.ToInt32(fc["DocId"].ToString());
                Docmodel.DocumentTitle = fc["DocumentTitle"].ToString();
                Docmodel.FileName = FileName;
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
                            BOMAdminBL BLobj = new BOMAdminBL();
                            string UserId = User.Identity.GetUserId();
                            int ErrCode = BLobj.SaveInwardDocumentDetail(Docmodel, UserId);
                            if (ErrCode == 500002 || ErrCode == 500001)
                            {
                                return RedirectToAction("CreateInwardDocument", new { DocId = Docmodel.DocId });
                            }
                            else if (ErrCode == 500000)
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
                                        Session["CustomerError"] = "This  Document Title  Already Created ";
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
                                return RedirectToAction("CreateInwardDocument", new { DocId = Docmodel.DocId });
                            }
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
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CreateInwardDocument", new { DocId = Docmodel.DocId });
        }
        public ActionResult DocDownload(int DocId, string DocumentPath)
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
                return RedirectToAction("CreateInwardDocument", new { DocId = DocId });
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return RedirectToAction("CreateInwardDocument", new { DocId = DocId });
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
        #endregion Inward Document

    }
}