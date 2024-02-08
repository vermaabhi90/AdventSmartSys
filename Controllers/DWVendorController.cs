using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.DW;
using SmartSys.BL.SysDBCon;
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
    public class DWVendorController : Controller
    {
        public ActionResult VendorList()
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
            bool BrandAllocation = true;
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWVendor/VendorList");
            if (FindFlag)
            {
                Session["VendorMapping"] = null;
                VendorListModel VendorModel = new VendorListModel();
                VendorModel.vendorGedList = new List<VendorListModel>();
                List<ActiveCompanyModel> CompanyModel = new List<ActiveCompanyModel>();
                try
                {
                    VendorBL objbl = new VendorBL();
                    CustomerBL objblCust = new CustomerBL();
                    VendorModel.vendorGedList = objbl.GetVendorList(UserId);
                    CompanyModel = objblCust.GetActiveCompanyList(UserId);
                    ViewBag.lstCompany = CompanyModel;
                    BrandAllocation = modelObj.FindMenu(lstTaskmodel, "AllowAllocateBrand");
                    if (BrandAllocation)
                    {
                        VendorModel.AllowBrand = true;
                    }
                    else
                    {
                        VendorModel.AllowBrand = false;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(VendorModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public void ExportToExcel(string GridModel)
        {
            string user_Id = User.Identity.GetUserId();
            VendorBL objbl = new VendorBL();
            var DataSource = objbl.GetVendorList(user_Id);
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
        public ActionResult CreateVendor(int VendorId, int tabIndex)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWVendor/CreateVendor?VendorId=0&tabIndex=0");
            if (FindFlag)
            {
                if (Session["CustomerError"] != null)
                {
                    TempData["Message"] = Session["CustomerError"] as string;
                }
                Session["CustomerError"] = null;
                Session["AddressList"] = null;
                VendorModel VendorModel = new VendorModel();
                VendorBL objbl = new VendorBL();
                VendorModel = objbl.DWvendorGetselected(VendorId);
                VendorModel.AddressList = objbl.GetSelectedAddressByCustomer(VendorId);
                VendorModel.ProductList = objbl.GetVendorProductList(VendorId);
                VendorModel.TabIndex = tabIndex;
                List<VendorListModel> VendLst = new List<VendorListModel>();
                VendLst = objbl.GetVendorList(UserId);
                ViewBag.VendorList = VendLst;
                return View(VendorModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult CreateVendor(FormCollection fc, bool IsActive, bool AuthorizedDealer, bool IsManufacturer)
        {

            VendorBL objbl = new VendorBL();
            string User_Id = User.Identity.GetUserId();
            VendorModel vendorModel = new VendorModel();
            int errorCode = 0;

            vendorModel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
            vendorModel.VendorName = fc["VendorName"].ToString();
            vendorModel.Region = fc["Region"].ToString();
            vendorModel.IsActive = IsActive;
            vendorModel.IsManufacturer = IsManufacturer;
            vendorModel.ModifiedBy = Convert.ToInt16(1);
            vendorModel.AuthorizedDealer = AuthorizedDealer;
            errorCode = objbl.saveVendor(vendorModel, User_Id);
            if (errorCode > 0)
            {
                return RedirectToAction("CreateVendor", new { VendorId = errorCode, tabIndex = 0 });
            }
            return RedirectToAction("VendorList");
        }
        public ActionResult Checkduplicacy(string VendorName)
        {
            bool Result = true;
            VendorBL objbl = new VendorBL();
            string User_Id = User.Identity.GetUserId();
            Result = objbl.CheckVendorDuplicacy(VendorName, User_Id);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        #region Professional Information
        public ActionResult CreateVendorProfessionalInfo(int VendorId)
        {
            //SysTaskModel modelObj = new SysTaskModel();
            //List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            //bool FindFlag;

            //FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWCustomer/CustomerList");
            //if (FindFlag)
            //{               
            try
            {
                VendorModel customerModel = new VendorModel();
                VendorBL objbl = new VendorBL();
                customerModel = objbl.DWvendorGetselected(VendorId);
                return PartialView(customerModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}           
        }
        [HttpPost]
        public ActionResult CreateVendorProfessionalInfo(FormCollection fc)
        {
            try
            {
                VendorBL objbl = new VendorBL();
                string User_Id = User.Identity.GetUserId();
                VendorModel VendorModel = new VendorModel();
                int errorCode = 0;

                VendorModel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                VendorModel.emailId = fc["emailId"].ToString();
                VendorModel.VAT = fc["VAT"].ToString();
                VendorModel.PAN = fc["PAN"].ToString();
                VendorModel.CST = fc["CST"].ToString();
                VendorModel.TAN = fc["TAN"].ToString();
                VendorModel.ExciseRange = fc["ExciseRange"].ToString(); ;
                VendorModel.ExciseDivision = fc["ExciseDivision"].ToString(); ;
                VendorModel.ExciseNo = fc["ExciseNo"].ToString();
                VendorModel.ExciseCommissionRate = fc["ExciseCommissionRate"].ToString();
                VendorModel.Website = fc["Website"].ToString();
                VendorModel.Remark = fc["Remark"].ToString();
                errorCode = objbl.saveVendorProfessionalInfo(VendorModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {
                    TempData["Msg"] = "Successfully Create ....";
                    return RedirectToAction("CreateVendor", new { VendorId = VendorModel.VendorId, tabIndex = 0 });
                }

                return RedirectToAction("CreateVendor", new { VendorId = VendorModel.VendorId, tabIndex = 0 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Professional Information Work

        #region Customer  Contact Informations
        public ActionResult CreateVendorContactDetails(string VendorDetail)
        {
            //SysTaskModel modelObj = new SysTaskModel();
            //List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            //bool FindFlag;
            string[] values = VendorDetail.Split(',');
            //FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWCustomer/CustomerList");
            //if (FindFlag)
            //{               
            try
            {
                VendorBL objbl = new VendorBL();
                VendorContactDetailsModel Model = new VendorContactDetailsModel();
                if (values[1] != "")
                {
                    Model = objbl.GetSelectedVendorContactDetails(Convert.ToInt32(values[0]), values[1]);
                    Model.NewContactName = Model.ContactName;
                }
                else
                {
                    Model.VendorId = Convert.ToInt32(values[0]);
                    Model.NewContactName = "New Entry";

                }
                return PartialView(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}           
        }
        [HttpPost]
        public ActionResult CreateVendorContactDetails(FormCollection fc)
        {
            try
            {
                VendorBL objbl = new VendorBL();
                string User_Id = User.Identity.GetUserId();
                VendorContactDetailsModel VendorModel = new VendorContactDetailsModel();
                int errorCode = 0;
                VendorModel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                VendorModel.ContactName = fc["ContactName"].ToString();
                VendorModel.NewContactName = fc["NewContactName"].ToString();
                VendorModel.Designation = fc["Designation"].ToString();
                VendorModel.Email = fc["Email"].ToString();
                VendorModel.MobileNo = fc["MobileNo"].ToString();
                VendorModel.PhoneNo = fc["PhoneNo"].ToString();
                VendorModel.Experties = fc["Experties"].ToString();
                VendorModel.Qualification = fc["Qualification"].ToString();
                VendorModel.Remark = fc["Remark"].ToString();
                if (fc["BirthDate"].ToString() != "")
                {
                    VendorModel.BirthDate = Convert.ToDateTime(fc["BirthDate"].ToString());
                }

                if (VendorModel.NewContactName != "New Entry")
                {
                    string temp = VendorModel.NewContactName;
                    VendorModel.NewContactName = VendorModel.ContactName;
                    VendorModel.ContactName = temp;
                }

                errorCode = objbl.saveVendorContactDetailsInfo(VendorModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {
                    TempData["Msg"] = "Successfully Create ....";
                    return RedirectToAction("CreateVendor", new { VendorId = VendorModel.VendorId, tabIndex = 1 });
                }
                else if (errorCode == 500000)
                {

                    Session["CustomerError"] = "This  Vendor  Contact Already Created ";
                    return RedirectToAction("CreateVendor", new { VendorId = VendorModel.VendorId, tabIndex = 1 });
                }
                return RedirectToAction("CreateVendor", new { VendorId = VendorModel.VendorId, tabIndex = 1 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion Customer  Contact Informations

        #region BankDetail Work
        //public ActionResult CreateVendorBankDetails(string BankDetail)
        //{
        //    //SysTaskModel modelObj = new SysTaskModel();
        //    //List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
        //    //lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
        //    //bool FindFlag;
        //    string[] values = BankDetail.Split(',');
        //    //FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWCustomer/CustomerList");
        //    //if (FindFlag)
        //    //{               
        //    try
        //    {
        //        VendorBL objbl = new VendorBL();
        //        VendorBankDetailModel Model = new VendorBankDetailModel();
        //        Model.AccountNo = values[1];
        //        if (Model.AccountNo.Length !=  0)
        //        {
        //            Model = objbl.GetSelectedVendorBankDetail(Convert.ToInt32(values[0]), values[1]);
        //            Model.NewAccountNo = Model.AccountNo;
        //        }
        //        else
        //        {
        //            Model.VendorId = Convert.ToInt32(values[0]);
        //            Model.NewAccountNo = "New Entry";

        //        }
        //        return PartialView(Model);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //}
        //    //else
        //    //{
        //    //    return RedirectToAction("Index", "Home");
        //    //}           
        //}

        public ActionResult CreateVendorBankDetails(int VendorId, string AccountNo)
        {

            try
            {
                VendorBankDetailModel Model = new VendorBankDetailModel();
                VendorBL objbl = new VendorBL();
                if (AccountNo != null)
                {
                    Model = objbl.GetSelectedVendorBankDetail(VendorId, AccountNo);
                    Model.NewAccountNo = Model.AccountNo;
                }
                else
                {
                    Model.VendorId = VendorId;
                    Model.NewAccountNo = "New Entry";
                }
                return PartialView(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult CreateVendorBankDetails(FormCollection fc)
        {
            try
            {
                VendorBL objbl = new VendorBL();
                string User_Id = User.Identity.GetUserId();
                VendorBankDetailModel VendorModel = new VendorBankDetailModel();
                int errorCode = 0;
                VendorModel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                VendorModel.AccountNo = fc["AccountNo"].ToString();
                VendorModel.NewAccountNo = fc["NewAccountNo"].ToString();
                VendorModel.BankName = fc["BankName"].ToString();
                VendorModel.Limit = Convert.ToDouble(fc["Limit"].ToString());
                if (VendorModel.NewAccountNo != "New Entry")
                {
                    string temp = VendorModel.NewAccountNo;
                    VendorModel.NewAccountNo = VendorModel.AccountNo;
                    VendorModel.AccountNo = temp;
                }
                errorCode = objbl.saveVendorBankDetailsInfo(VendorModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {
                    TempData["Msg"] = "Successfully Create ....";
                    return RedirectToAction("CreateVendor", new { VendorId = VendorModel.VendorId, tabIndex = 2 });
                }
                else if (errorCode == 500000)
                {

                    Session["CustomerError"] = "This  Bank Account Number  Already Created ";
                    return RedirectToAction("CreateVendor", new { VendorId = VendorModel.VendorId, tabIndex = 2 });
                }
                return RedirectToAction("CreateVendor", new { VendorId = VendorModel.VendorId, tabIndex = 2 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion BankDetail Work

        #region Libary Work

        public ActionResult VendorLibary(int VendorId, string IsKyc)
        {
            //SysTaskModel modelObj = new SysTaskModel();
            //List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            //bool FindFlag;

            //FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWCustomer/CustomerList");
            //if (FindFlag)
            //{               
            try
            {
                VendorLibaryModel Libarymodel = new VendorLibaryModel();
                Libarymodel.isKYC = IsKyc;
                Libarymodel.VendorId = VendorId;
                return PartialView(Libarymodel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}  
        }
        [HttpPost]
        public ActionResult VendorLibary(HttpPostedFileBase file, FormCollection fc)
        {
            VendorLibaryModel Vendormodel = new VendorLibaryModel();
            try
            {
                string fileName = Path.GetFileName(file.FileName);
                string ftpServer = Common.GetConfigValue("FTP");
                string[] FileSplit = fileName.Split('.');
                string FileEx = FileSplit[1].ToString();
                String guid = Guid.NewGuid().ToString();
                string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                string FileName = Convert.ToString("/VendorDoc/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;

                Vendormodel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                Vendormodel.isKYC = Convert.ToString(fc["isKYC"].ToString());
                if (Vendormodel.isKYC == "KYC")
                {
                    Vendormodel.KYC = Convert.ToBoolean(1);
                }
                else
                {
                    Vendormodel.KYC = Convert.ToBoolean(0);
                }
                Vendormodel.DocumentTitle = fc["DocumentTitle"].ToString();
                Vendormodel.Description = fc["Description"].ToString();
                Vendormodel.DocumentPath = FileName;
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
                            VendorBL BLobj = new VendorBL();
                            string UserId = User.Identity.GetUserId();
                            int ErrCode = BLobj.SaveVendorLibraryDetailInfo(Vendormodel, UserId);
                            if (ErrCode == 500002 || ErrCode == 500001)
                            {
                                return RedirectToAction("CreateVendor", new { VendorId = Vendormodel.VendorId, tabIndex = 4 });
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
                                return RedirectToAction("CreateVendor", new { VendorId = Vendormodel.VendorId, tabIndex = 4 });
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
            return RedirectToAction("CreateVendor", new { VendorId = Vendormodel.VendorId, tabIndex = 4 });
        }
        public ActionResult VendorDownload(int VendorId, string DocumentPath)
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
                return RedirectToAction("CreateVendor", new { VendorId = VendorId });
            }
            catch (Exception e)
            {
                Session["CustomerError"] = e.Message.ToString();
                return RedirectToAction("CreateVendor", new { VendorId = VendorId });
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

        #endregion Libary Work

        #region Certification Work
        public ActionResult CreateVendorCertificationDetails(int VendorId, string Certification)
        {
            //SysTaskModel modelObj = new SysTaskModel();
            //List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            //bool FindFlag;

            //FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWCustomer/CustomerList");
            //if (FindFlag)
            //{               
            try
            {
                VendorBL objbl = new VendorBL();
                VendorCertificationModel Model = new VendorCertificationModel();
                if (Certification != null)
                {
                    Model = objbl.GetSelectedVendorCertificationDetails(VendorId, Certification);
                    Model.NewVendorCertification = Model.VendorCertification;
                }
                else
                {
                    Model.VendorId = VendorId;
                    Model.NewVendorCertification = "New Entry";
                    Model.CertificateDate = Convert.ToDateTime(System.DateTime.Today);
                }
                Model.CertificateDate = Convert.ToDateTime(Model.CertificateDate.ToShortDateString());
                return PartialView(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}           
        }
        [HttpPost]
        public ActionResult CreateVendorCertificationDetails(FormCollection fc)
        {
            try
            {
                VendorBL objbl = new VendorBL();
                string User_Id = User.Identity.GetUserId();
                VendorCertificationModel CertificationModel = new VendorCertificationModel();
                int errorCode = 0;
                CertificationModel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                CertificationModel.VendorCertification = fc["VendorCertification"].ToString();
                CertificationModel.NewVendorCertification = fc["NewVendorCertification"].ToString();
                CertificationModel.CertificateDate = Convert.ToDateTime(fc["CertificateDate"].ToString());
                if (CertificationModel.NewVendorCertification != "New Entry")
                {
                    string temp = CertificationModel.NewVendorCertification;
                    CertificationModel.NewVendorCertification = CertificationModel.VendorCertification;
                    CertificationModel.VendorCertification = temp;
                }
                errorCode = objbl.saveVendorCertificationDetailsInfo(CertificationModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {
                    TempData["Msg"] = "Successfully Create ....";
                    return RedirectToAction("CreateVendor", new { VendorId = CertificationModel.VendorId, tabIndex = 6 });
                }
                else if (errorCode == 500000)
                {

                    Session["CustomerError"] = "This  Customer  Certification Already Created ";
                    return RedirectToAction("CreateVendor", new { VendorId = CertificationModel.VendorId, tabIndex = 6 });
                }
                return RedirectToAction("CreateVendor", new { VendorId = CertificationModel.VendorId, tabIndex = 6 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Certification Work

        #region TurnOver Work
        //public ActionResult CreateVendorTurnoverDetails(string TurnoverDetails)
        //{

        //    string[] values = TurnoverDetails.Split(',');
        //    VendorTurnoverModel Model = new VendorTurnoverModel();
        //    finanacialYear yr = new finanacialYear();
        //    List<string> FyYear = new List<string>();
        //    FyYear = yr.FY().ToList();
        //    try
        //    {
        //        VendorBL objbl = new VendorBL();
        //        ViewBag.FYYear = new SelectList(FyYear);



        //        Model.TurnoverYear = values[1];
        //        if (Model.TurnoverYear.Length != 0)
        //        {
        //            Model = objbl.GetSelectedVendorTurnoverDetails(Convert.ToInt32(values[0]), values[1]);
        //            Model.NewTurnoverYear = (string)Model.TurnoverYear;
        //        }
        //        else
        //        {

        //            Model.VendorId = Convert.ToInt32(values[0]);
        //            Model.NewTurnoverYear = "";
        //        }
        //        if (Model.NewTurnoverYear == "")
        //        {
        //            return PartialView(Model);
        //        }

        //        return View(Model);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }


        //}
        public ActionResult CreateVendorTurnoverDetails(int VendorId, string TurnoverYear)
        {
            //SysTaskModel modelObj = new SysTaskModel();
            //List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            //bool FindFlag;
            //FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWCustomer/CustomerList");
            //if (FindFlag)
            //{     
            finanacialYear yr = new finanacialYear();

            List<string> FyYear = new List<string>();
            FyYear = yr.FY().ToList();
            try
            {
                VendorBL objbl = new VendorBL();
                ViewBag.FYYear = new SelectList(FyYear);
                VendorTurnoverModel Model = new VendorTurnoverModel();
                if (TurnoverYear != "")
                {
                    Model = objbl.GetSelectedVendorTurnoverDetails(VendorId, TurnoverYear);
                    Model.NewTurnoverYear = (string)Model.TurnoverYear;
                }
                else
                {
                    Model.VendorId = VendorId;
                    Model.NewTurnoverYear = "";
                }
                return PartialView(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}           
        }
        [HttpPost]
        public ActionResult CreateVendorTurnoverDetails(FormCollection fc)
        {
            try
            {
                VendorBL objbl = new VendorBL();
                string User_Id = User.Identity.GetUserId();
                VendorTurnoverModel TurnoverModel = new VendorTurnoverModel();
                int errorCode = 0;
                TurnoverModel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                TurnoverModel.TurnoverYear = (fc["TurnoverYear"].ToString());
                TurnoverModel.NewTurnoverYear = (fc["NewTurnoverYear"].ToString());
                TurnoverModel.Turnover = Convert.ToDouble(fc["Turnover"].ToString());
                TurnoverModel.ProjectedTurnover = Convert.ToDouble(fc["ProjectedTurnover"].ToString());
                if (TurnoverModel.NewTurnoverYear != "")
                {
                    string temp = TurnoverModel.NewTurnoverYear;
                    TurnoverModel.NewTurnoverYear = TurnoverModel.TurnoverYear;
                    TurnoverModel.TurnoverYear = temp;
                }
                errorCode = objbl.saveTurnoverVendorDetailsInfo(TurnoverModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {
                    TempData["Msg"] = "Successfully Create ....";
                    return RedirectToAction("CreateVendor", new { VendorId = TurnoverModel.VendorId, tabIndex = 6 });
                }
                else if (errorCode == 500000)
                {

                    Session["CustomerError"] = "This  Vendor  Turnover Already Created ";
                    return RedirectToAction("CreateVendor", new { VendorId = TurnoverModel.VendorId, tabIndex = 6 });
                }
                return RedirectToAction("CreateVendor", new { VendorId = TurnoverModel.VendorId, tabIndex = 6 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion TurnOver Work

        #region Competitor Work
        public ActionResult CreateVendorCompetitorDetails(int VendorId, int CompetitorId)
        {
            //SysTaskModel modelObj = new SysTaskModel();
            //List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            //bool FindFlag;

            //FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWCustomer/CustomerList");
            //if (FindFlag)
            //{               
            try
            {
                VendorBL objbl = new VendorBL();
                VendorCompetitorModel Model = new VendorCompetitorModel();
                if (CompetitorId != 0)
                {
                    Model = objbl.GetSelectedVendorCompetitorDetails(VendorId, CompetitorId);
                    Model.NewCompetitorId = (int)Model.CompetitorId;
                }
                else
                {
                    Model.VendorId = VendorId;
                    Model.NewCompetitorId = 0;
                }
                List<VendorCompetitorModel> CompetitorList = new List<VendorCompetitorModel>();
                CompetitorList = objbl.GetVendorCompetitorList();
                ViewBag.CompetitorLst = new SelectList(CompetitorList, "VendorId", "CustomerName", Model.CompetitorId);
                if (Model.NewCompetitorId == 0)
                {
                    return PartialView(Model);
                }
                return View(Model);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}           
        }
        [HttpPost]
        public ActionResult CreateVendorCompetitorDetails(FormCollection fc)
        {
            try
            {
                VendorBL objbl = new VendorBL();
                string User_Id = User.Identity.GetUserId();
                VendorCompetitorModel TurnoverModel = new VendorCompetitorModel();
                int errorCode = 0;
                TurnoverModel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                TurnoverModel.CompetitorId = Convert.ToInt32(fc["CompetitorId"].ToString());
                TurnoverModel.NewCompetitorId = Convert.ToInt32(fc["NewCompetitorId"].ToString());
                if (TurnoverModel.NewCompetitorId > 0)
                {
                    int temp = TurnoverModel.NewCompetitorId;
                    TurnoverModel.NewCompetitorId = TurnoverModel.CompetitorId;
                    TurnoverModel.CompetitorId = temp;
                }
                errorCode = objbl.saveVendorCompetitorDetailsInfo(TurnoverModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {
                    TempData["Msg"] = "Successfully Create ....";
                    return RedirectToAction("CreateVendor", new { VendorId = TurnoverModel.VendorId, tabIndex = 6 });
                }
                else if (errorCode == 500000)
                {

                    Session["CustomerError"] = "This  Customer  Competitor Already Created ";
                    return RedirectToAction("CreateVendor", new { VendorId = TurnoverModel.VendorId, tabIndex = 6 });
                }
                return RedirectToAction("CreateVendor", new { VendorId = TurnoverModel.VendorId, tabIndex = 6 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Competitor Work

        #region Address Work

        public ActionResult CreateVendorAddress(int VendorId, int AddressId)
        {
            VendorAddressModel addressModel = new VendorAddressModel();
            try
            {
                CustomerBL objbl = new CustomerBL();
                VendorBL objbl1 = new VendorBL();
                addressModel.AddressId = AddressId;
                addressModel.LstCountry = objbl.GetCountryList();
                ViewBag.CountryList = new SelectList(addressModel.LstCountry, "CountryId", "CountryName");

            }

            catch (Exception)
            {

                throw;
            }

            return PartialView(addressModel);
        }
        [HttpPost]
        public ActionResult CreateNewVendorAddress(FormCollection fc, bool isPrimary)
        {
            VendorAddressModel objmodel = new VendorAddressModel();
            try
            {
                VendorBL objbl = new VendorBL();
                int errorcode = 0;
                objmodel.VendorId = Convert.ToInt32(fc["VendorId"].ToString());
                if (objmodel.VendorId > 0)
                {
                    objmodel.AddressId = Convert.ToInt32(fc["AddressId"]);
                }
                else
                {
                    objmodel.AddressId = 0;
                }
                objmodel.Line1 = fc["Line1"].ToString();
                objmodel.Line2 = fc["Line2"].ToString();
                objmodel.State = fc["State"].ToString();
                objmodel.City = fc["CityId"].ToString();
                objmodel.Country = fc["Country"].ToString();
                objmodel.Pin = fc["Pin"].ToString();
                objmodel.Description = fc["Description"].ToString();
                objmodel.LandMark = fc["LandMark"].ToString();
                // objmodel.isPrimary = Convert.ToDouble(fc["isPrimary"].ToString());
                errorcode = objbl.SaveVendorAdressDeatils(objmodel, isPrimary, objmodel.Description);
                if (errorcode == 500001 || errorcode == 500002)
                {
                    Session["AddressList"] = null;
                    TempData["Msg"] = "Successfully Create...";
                    return RedirectToAction("CreateVendor", new { VendorId = objmodel.VendorId, tabIndex = 3 });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CreateVendor", new { VendorId = objmodel.VendorId, tabIndex = 3 });

        }

        public ActionResult SaveCreateAddress(int AddressId, int VendorId, string Line1, string Line2, string Pin, string LandMark, string Country, string State, string City, string Description, bool isPrimary)
        {
            VendorAddressModel objmodel = new VendorAddressModel();
            try
            {
                VendorBL objbl = new VendorBL();
                int errorcode = 0;
                objmodel.VendorId = VendorId;
                if (objmodel.VendorId > 0)
                {
                    objmodel.AddressId = AddressId;
                }
                else
                {
                    objmodel.AddressId = 0;
                }
                objmodel.Line1 = Line1;
                objmodel.Line2 = Line2;
                objmodel.State = State;
                objmodel.City = City;
                objmodel.Country = Country;
                objmodel.Pin = Pin;
                objmodel.Description = Description;
                objmodel.LandMark = LandMark;
                errorcode = objbl.SaveVendorAdressDeatils(objmodel, isPrimary, Description);
                if (errorcode == 500001 || errorcode == 500002)
                {
                    Session["AddressList"] = null;
                    TempData["Msg"] = "Successfully Create...";
                    return RedirectToAction("CreateVendor", new { VendorId = objmodel.VendorId, tabIndex = 3 });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CreateVendor", new { VendorId = objmodel.VendorId, tabIndex = 3 });
        }

        public ActionResult CreateNewVendorAddress(int VendorId, int AddressId, int State, int country)
        {
            VendorAddressModel addressModel = new VendorAddressModel();


            VendorBL objbl = new VendorBL();
            CustomerBL objbl1 = new CustomerBL();
            List<StateModel> lst = new List<StateModel>();
            if (AddressId > 0)
            {
                addressModel = objbl.GetSelectedVendorAddressDetail(VendorId, AddressId);
            }
            addressModel.LstCountry = objbl1.GetCountryList();
            ViewBag.CountryList = new SelectList(addressModel.LstCountry, "CountryId", "CountryName");
            ViewBag.CountryL = addressModel.LstCountry;
            lst = objbl1.GetStateList(country);
            ViewBag.StateList = new SelectList(lst, "StateId", "StateName");
            ViewBag.StateList1 = lst;
            addressModel.LstCity = objbl1.GetCityList(State);
            ViewBag.Citylist = new SelectList(addressModel.LstCity, "CityId", "CityName");


            return PartialView(addressModel);
        }
        public JsonResult GetStateList(int Country)
        {
            VendorAddressModel addressModel = new VendorAddressModel();
            List<StateModel> lst = new List<StateModel>();
            try
            {
                CustomerBL objbl = new CustomerBL();


                addressModel.CountryId = Country;
                lst = objbl.GetStateList(Country);

                ViewBag.List = new SelectList(lst, "StateId", "StateName");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(new SelectList(lst, "StateId", "StateName"));
        }
        public JsonResult GetCityList(int State)
        {
            VendorAddressModel addressModel = new VendorAddressModel();
            try
            {
                CustomerBL objbl = new CustomerBL();

                addressModel.StateId = State;
                addressModel.LstCity = objbl.GetCityList(State);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(addressModel.LstCity, "CityId", "CityName"));
        }
        #endregion Address Work

        #region Product Work
        public ActionResult CreateVendorProduct(int VendorId, string VendorName)
        {

            VendorProductModel objmodel = new VendorProductModel();
            VendorBL objbl = new VendorBL();
            objmodel.ItemList = objbl.VendorProductGetselected(VendorId);
            objmodel.VendorId = VendorId;
            objmodel.VendorName = VendorName;
            ViewBag.EquipmentLst = new SelectList(objmodel.ItemList, "EquipmentId", "EquipmentName");
            
            return PartialView(objmodel);
        }
        [HttpPost]
        public ActionResult CreateVendorProduct(FormCollection fc)
        {
            VendorProductModel objmodel = new VendorProductModel();

            string User_Id = User.Identity.GetUserId();
            VendorBL BLobj = new VendorBL();
            objmodel.VendorId = Convert.ToInt32(fc["VendorId"]);
            objmodel.EquipmentId = Convert.ToInt32(fc["EquipmentId"]);
            int errorcode = BLobj.SaveVendorproductDetails(objmodel);
            if (errorcode == 500001 || errorcode == 500002)
            {

                return RedirectToAction("CreateVendor", new { VendorId = objmodel.VendorId, tabIndex = 5 });

            }

            else if (errorcode == 500000)
            {

                Session["CustomerError"] = "This  Product  Name Already Created ";
                return RedirectToAction("CreateVendor", new { VendorId = objmodel.VendorId, tabIndex = 5 });
            }
            return RedirectToAction("CreateVendor", new { VendorId = 0 });


        }
        public ActionResult DeleteVendorEquipment(int VendorId, int EquipmentId)
        {
            VendorBL objbl = new VendorBL();
            int errcode = objbl.DeleteVendorEquipment(VendorId, EquipmentId);
            return RedirectToAction("CreateVendor", new { VendorId = VendorId, tabIndex = 5 });
        }
        public ActionResult selectedParam(int ItemId, double Quantity, int productid, string productname)
        {
            string User_Id = User.Identity.GetUserId();

            VendorBL objbl = new VendorBL();
            int errcode = objbl.saveVendorItemDetails(User_Id, ItemId, Quantity, productid, productname);
            return View();
        }
        #endregion Product Work

        #region Assign UserId

        public ActionResult AssignVendorUserId(int VendorId,int VendorContactId, int UserId, string UserName)
        {
            VendorUserModel UserModel = new VendorUserModel();
            try
            {
                VendorBL BLObj = new VendorBL();
                UserModel = BLObj.GetVendorUserList(VendorContactId);
                UserModel.VendorContactId = VendorContactId;
                UserModel.UserId = UserId;
                UserModel.VendorId = VendorId;
                if (UserName != null)
                {
                    UserModel.UserName = UserName;
                }
                else
                {
                    UserModel.UserName = "";
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return PartialView(UserModel);
        }
        public ActionResult SaveAssignVendorUserId(int userId, int VendorContactId, int VendorId)
        {
            int errCode = 0;
            try
            {
                VendorBL BLObj = new VendorBL();
                errCode = BLObj.SaveAssignVendorUserId(userId, VendorContactId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("CreateVendor", new { VendorId = VendorId, TabIndex = 1 });
        }
        public ActionResult DeleteAssignVendorUserId(int VendorContactId)
        {
            int errCode = 0;
            try
            {
                VendorBL BLObj = new VendorBL();
                errCode = BLObj.DeleteAssignVendorUserId(VendorContactId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion Assign UserId

        #region Vendor Mapping
        public ActionResult DWVendorMapping()
        {
            if (Session["VendorError"] != null)
            {
                TempData["Message"] = Session["VendorError"] as string;
            }
            Session["VendorError"] = null;
            VendorBL BlObj = new VendorBL();
            VendorMappingModel VendorMap = new VendorMappingModel();
            try
            {
                if (Session["VendorMapping"] != null)
                {
                    VendorMap = Session["VendorMapping"] as VendorMappingModel;
                    VendorMap.lstVendorMap = new List<VendorMappingModel>();
                    VendorMap.lstVendorMap = BlObj.GetDWCompList();
                    foreach (SysNavRelVendorListModel demo in VendorMap.GridList)
                    {
                        var actionToDelete = from SeleVendor in VendorMap.lstVendorMap
                                             where SeleVendor.CompCode.ToUpper() == demo.CompName.ToUpper()
                                             select SeleVendor;
                        VendorMap.lstVendorMap.Remove(actionToDelete.ElementAt(0));

                    }
                    ViewBag.VendorMapList = new SelectList(VendorMap.lstVendorMap, "CompCode", "CompName");
                    if (VendorMap.lstClientMapVendorList != null)
                        ViewBag.ComVendorList = new SelectList(VendorMap.lstClientMapVendorList, "ClientVendorID", "ClientVendorName");

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(VendorMap);
        }
        public ActionResult GetVendorMappingList(int VendorId, string VendorName)
        {

            VendorMappingModel VendorMap = new VendorMappingModel();
            VendorBL BlObj = new VendorBL();
            VendorMap.GridList = BlObj.GetVendorNavRelList(VendorId);
            VendorMap.VendorName = VendorName;
            VendorMap.VendorId = VendorId;
            foreach (SysNavRelVendorListModel DWVendor in VendorMap.GridList)
            {
                SysDBConModel ConnModel = new SysDBConModel();
                SysDBConBL ConnobjBL = new SysDBConBL();
                ConnModel = ConnobjBL.GetSelectedDBConn(DWVendor.ConnectionId);
                ArrayList ConnInfo = new ArrayList();
                ConnInfo.Add(ConnModel.ConnectionId);
                ConnInfo.Add(ConnModel.ConnectionType);
                ConnInfo.Add(ConnModel.ServerName);
                ConnInfo.Add(ConnModel.DBName);
                ConnInfo.Add(ConnModel.UserName);
                ConnInfo.Add(ConnModel.Password);
                string StrTemp = BlObj.GetClientItemName(DWVendor.CompName, DWVendor.CompVendorID, ConnInfo);
                DWVendor.CompVendorName = StrTemp;
            }
            Session["VendorMapping"] = VendorMap;
            return RedirectToAction("DWVendorMapping");

        }
        public ActionResult GetAutoMapping(string VendorName, string CompCode, int VendorId)
        {
            try
            {
                string User_Id = User.Identity.GetUserId();
                VendorMappingModel VendorMap = new VendorMappingModel();
                VendorBL BlObj = new VendorBL();
                VendorMap.lstVendorMap = BlObj.GetDWCompList();
                string SPName = "";
                Int16 ConnId = 0;
                foreach (VendorMappingModel TempModel in VendorMap.lstVendorMap)
                {
                    if (CompCode == TempModel.CompCode)
                    {
                        SPName = TempModel.SaveVendorSP;
                        ConnId = TempModel.ConnectionId;
                        VendorMap.selectedComp = CompCode;
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

                int errcode = BlObj.GetAutoMapping(User_Id, CompCode, VendorName, VendorId, SPName, ConnInfo);
                if (errcode == 500004)
                {
                    return RedirectToAction("GetVendorMappingList", new { VendorId = VendorId, VendorName = VendorName });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetVendorMappingList", new { VendorId = VendorId, VendorName = VendorName });
        }
        public ActionResult SaveMapping(string Vendor_No, string CompCode, int VendorId, string VendorName)
        {
            if (Vendor_No == null || Vendor_No == "" || CompCode == null || CompCode == "" || VendorId == 0 || VendorId == null)
            {
                return RedirectToAction("VendorList");
            }
            string Errorcode = "0";
            VendorMappingModel VendorMap = new VendorMappingModel();
            string User_Id = User.Identity.GetUserId();
            VendorBL BlObj = new VendorBL();
            VendorMap.CompVendorID = Vendor_No;
            VendorMap.CompCode = CompCode;
            VendorMap.VendorId = VendorId;
            Errorcode = BlObj.SaveMapping(VendorMap, User_Id);
            if (Errorcode == "500002" || Errorcode == "500001")
            {
                return RedirectToAction("GetVendorMappingList", new { VendorId = VendorId, VendorName = VendorName });
            }
            else if (Errorcode != "0")
            {

                Session["VendorError"] = "This External Vendor Already Mapped with " + Errorcode;
                return RedirectToAction("GetVendorMappingList", new { VendorId = VendorId, VendorName = VendorName });
            }
            else
            {

                return RedirectToAction("GetVendorMappingList", new { VendorId = VendorId, VendorName = VendorName });
            }

        }
        public ActionResult GetClientVendorList(string CompCode)
        {

            VendorMappingModel VendorMap = new VendorMappingModel();
            VendorMap = Session["VendorMapping"] as VendorMappingModel;
            VendorBL BlObj = new VendorBL();
            VendorMap.lstVendorMap = BlObj.GetDWCompList();
            string SPName = "";
            Int16 ConnId = 0;

            foreach (VendorMappingModel TempModel in VendorMap.lstVendorMap)
            {
                if (CompCode == TempModel.CompCode)
                {
                    SPName = TempModel.VendorListSP;
                    ConnId = TempModel.ConnectionId;
                    VendorMap.selectedComp = CompCode;
                    VendorMap.AutoMapBtn = TempModel.SaveVendorSP;
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


            VendorMap.lstClientMapVendorList = BlObj.GetCompanyVendor(CompCode, SPName, ConnInfo);
            Session["VendorMapping"] = VendorMap;
            return RedirectToAction("DWVendorMapping");
        }
        public ActionResult DeleteVendorMapping(string CompName, int VendorId, string VendorName)
        {
            int Errorcod = 0;
            VendorMappingModel VendorMap = new VendorMappingModel();
            VendorBL BlObj = new VendorBL();
            Errorcod = BlObj.DeleteVendorMapping(CompName, VendorId);
            if (Errorcod == 500003)
            {
                VendorMap.GridList = BlObj.GetVendorNavRelList(VendorId);
                VendorMap.CompName = CompName;
                VendorMap.VendorId = VendorId;
                VendorMap.VendorName = VendorName;

                foreach (SysNavRelVendorListModel DwVendor in VendorMap.GridList)
                {
                    SysDBConModel ConnModel = new SysDBConModel();
                    SysDBConBL ConnobjBL = new SysDBConBL();
                    ConnModel = ConnobjBL.GetSelectedDBConn(DwVendor.ConnectionId);
                    ArrayList ConnInfo = new ArrayList();
                    ConnInfo.Add(ConnModel.ConnectionId);
                    ConnInfo.Add(ConnModel.ConnectionType);
                    ConnInfo.Add(ConnModel.ServerName);
                    ConnInfo.Add(ConnModel.DBName);
                    ConnInfo.Add(ConnModel.UserName);
                    ConnInfo.Add(ConnModel.Password);
                    string StrTemp = BlObj.GetClientItemName(DwVendor.CompName, DwVendor.CompVendorID, ConnInfo);
                    DwVendor.CompVendorName = StrTemp;

                }
                Session["VendorMapping"] = VendorMap;
                return RedirectToAction("DWVendorMapping");
            }
            else
            {
                return RedirectToAction("DWVendorMapping");
            }
        }
        #endregion Vendor Mapping

        #region Pendding Approval Vendor Mapping
        [Authorize]
        public ActionResult VendorPenndingApproverMapping(int RefershFlag)
        {

            //if (Session["Vendor"] != null)
            //{
            //    TempData["Message"] = Session["Vendor"] as string;
            //}
            //Session["Vendor"] = null;
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWVendor/VendorPenndingApproverMapping?RefershFlag=0");
            if (FindFlag)
            {
                PenndingVendorApproverMap VenPendApproverMap = new PenndingVendorApproverMap();
                VenPendApproverMap.SelectApproverLst = new List<SelectPenddingVendorApproveList>();
                VenPendApproverMap.lstApprover = new List<PenndingVendorApproverMap>();
                try
                {
                    string User_Id = User.Identity.GetUserId();
                    VendorBL BlObj = new VendorBL();
                    if (RefershFlag == 0)
                    {
                        Session["VendorApproverList"] = null;
                    }
                    if (Session["VendorApproverList"] != null)
                    {
                        VenPendApproverMap = Session["VendorApproverList"] as PenndingVendorApproverMap;
                    }
                    else
                    {
                        VenPendApproverMap.lstApprover = BlObj.GetVenderPenddingApproverList(User_Id, (int)Utility.Enums.StatusCode.REVIEWED);
                    }

                    if (VenPendApproverMap.lstApprover != null && VenPendApproverMap.lstApprover.Count > 0)
                    {
                        Session["VendorApproverList"] = VenPendApproverMap;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(VenPendApproverMap);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportVendorPenndingApproverMapLst")]
        [AcceptVerbs("POST")]
        public void ExportVendorPenndingApproverMapLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            VendorBL BlObj = new VendorBL();
            var DataSource = BlObj.GetVenderPenddingApproverList(User_Id, (int)Utility.Enums.StatusCode.REVIEWED);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult AddtoVenList(string Vendor_No, string SourceVenderName, string CompName, int VendorId, string VendorName, string ModifiedBy)
        {
            try
            {

                PenndingVendorApproverMap VenPendApproverMap = new PenndingVendorApproverMap();
                SelectPenddingVendorApproveList demo = new SelectPenddingVendorApproveList();
                if (Session["VendorApproverList"] != null)
                {
                    VenPendApproverMap = Session["VendorApproverList"] as PenndingVendorApproverMap;
                }
                var actionToDelete = from actionRepDel in VenPendApproverMap.lstApprover
                                     where actionRepDel.Vendor_No == Vendor_No && actionRepDel.CompCode == CompName
                                     select actionRepDel;
                VenPendApproverMap.lstApprover.Remove(actionToDelete.ElementAt(0));

                if (VenPendApproverMap.SelectApproverLst == null)
                    VenPendApproverMap.SelectApproverLst = new List<SelectPenddingVendorApproveList>();
                demo.VendorId = VendorId;
                demo.Vendor_No = Vendor_No;
                demo.CompName = CompName;
                demo.VendorName = VendorName;
                demo.SourceVenderName = SourceVenderName;
                demo.ModifiedBy = ModifiedBy;
                VenPendApproverMap.SelectApproverLst.Add(demo);
                Session["VendorApproverList"] = VenPendApproverMap;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("VendorPenndingApproverMapping", new { RefershFlag = 1 });
        }
        public ActionResult DeleteSelPendingApprover(string CompCode, string Vendor_No, string SourceVendorName, int VendorId, string VendorName, string ModifiedBy)
        {
            try
            {
                PenndingVendorApproverMap VenPendApproverMap = new PenndingVendorApproverMap();
                VenPendApproverMap.lstApprover = new List<PenndingVendorApproverMap>();
                if (Session["VendorApproverList"] != null)
                {
                    VenPendApproverMap = Session["VendorApproverList"] as PenndingVendorApproverMap;
                }
                var actionToDelete = from actionRepDel in VenPendApproverMap.SelectApproverLst
                                     where actionRepDel.Vendor_No == Vendor_No && actionRepDel.CompName == CompCode
                                     select actionRepDel;
                VenPendApproverMap.SelectApproverLst.Remove(actionToDelete.ElementAt(0));

                if (VenPendApproverMap.lstApprover == null)
                    VenPendApproverMap.lstApprover = new List<PenndingVendorApproverMap>();

                PenndingVendorApproverMap Demo = new PenndingVendorApproverMap();
                Demo.VendorId = VendorId;
                Demo.VendorName = VendorName;
                Demo.Vendor_No = Vendor_No;
                Demo.SourceVenderName = SourceVendorName;
                Demo.CompCode = CompCode;
                Demo.ModifiedBy = ModifiedBy;
                VenPendApproverMap.lstApprover.Add(Demo);
                Session["VendorApproverList"] = VenPendApproverMap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("VendorPenndingApproverMapping", new { RefershFlag = 1 });
        }
        public ActionResult ApvRejVendor(int statusCode)
        {
            try
            {
                if (statusCode == 20)
                {
                    Session["VendorApproverList"] = null;
                    return RedirectToAction("VendorPenndingApproverMapping", new { RefershFlag = 1 });
                }
                string User_Id = User.Identity.GetUserId();
                PenndingVendorApproverMap VenPendApproverMap = new PenndingVendorApproverMap();
                if (Session["VendorApproverList"] != null)
                {
                    VenPendApproverMap = Session["VendorApproverList"] as PenndingVendorApproverMap;
                }
                VendorBL BLObj = new VendorBL();
                int errCode = BLObj.ApvRejVendor(VenPendApproverMap, statusCode, User_Id);
                Session["VendorApproverList"] = null;
            }
            catch (Exception ex)
            {
                throw ex;
                //Session["Vendor"] = " please select Atleast One Vendor";
            }
            return RedirectToAction("VendorPenndingApproverMapping", new { RefershFlag = 1 });
        }
        public ActionResult RefreshVenApprovalList()
        {
            try
            {
                return RedirectToAction("VendorPenndingApproverMapping", new { RefershFlag = 0 });
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion Pendding Approval Vendor Mapping

        #region DW Pendding Mapping Review
        [Authorize]
        public ActionResult VendorPenndingReviewMapping(int RefershFlag)
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
            //if (Session["Vendor"] != null)
            //{
            //    TempData["Message"] = Session["Vendor"] as string;
            //}
            //Session["Vendor"] = null;
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWVendor/VendorPenndingReviewMapping?RefershFlag=0");
            if (FindFlag)
            {
                PenndingVendorApproverMap VenPendReviewMap = new PenndingVendorApproverMap();
                VenPendReviewMap.SelectApproverLst = new List<SelectPenddingVendorApproveList>();
                VenPendReviewMap.lstApprover = new List<PenndingVendorApproverMap>();
                try
                {
                    if (RefershFlag == 0)
                    {
                        Session["VendorReviewList"] = null;
                    }
                    string User_Id = User.Identity.GetUserId();
                    VendorBL BlObj = new VendorBL();
                    if (Session["VendorReviewList"] != null)
                    {
                        VenPendReviewMap = Session["VendorReviewList"] as PenndingVendorApproverMap;
                    }
                    else
                    {
                        VenPendReviewMap.lstApprover = BlObj.GetVenderPenddingApproverList(User_Id, (int)Utility.Enums.StatusCode.NEW);
                    }
                    if (VenPendReviewMap.lstApprover != null && VenPendReviewMap.lstApprover.Count > 0)
                    {
                        Session["VendorReviewList"] = VenPendReviewMap;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(VenPendReviewMap);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportVendorPenndingReviewMapLst")]
        [AcceptVerbs("POST")]
        public void ExportVendorPenndingReviewMapLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            VendorBL BlObj = new VendorBL();
            var DataSource = BlObj.GetVenderPenddingApproverList(User_Id, (int)Utility.Enums.StatusCode.NEW);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult AddtoVenReviewList(string Vendor_No, string SourceVenderName, string CompName, int VendorId, string VendorName, string ModifiedBy)
        {
            try
            {

                PenndingVendorApproverMap VenPendReviewMap = new PenndingVendorApproverMap();
                SelectPenddingVendorApproveList demo = new SelectPenddingVendorApproveList();
                if (Session["VendorReviewList"] != null)
                {
                    VenPendReviewMap = Session["VendorReviewList"] as PenndingVendorApproverMap;
                }
                var actionToDelete = from actionRepDel in VenPendReviewMap.lstApprover
                                     where actionRepDel.Vendor_No == Vendor_No && actionRepDel.CompCode == CompName
                                     select actionRepDel;
                VenPendReviewMap.lstApprover.Remove(actionToDelete.ElementAt(0));
                if (VenPendReviewMap.SelectApproverLst == null)
                    VenPendReviewMap.SelectApproverLst = new List<SelectPenddingVendorApproveList>();
                demo.VendorId = VendorId;
                demo.Vendor_No = Vendor_No;
                demo.CompName = CompName;
                demo.VendorName = VendorName;
                demo.SourceVenderName = SourceVenderName;
                demo.ModifiedBy = ModifiedBy;
                VenPendReviewMap.SelectApproverLst.Add(demo);
                Session["VendorReviewList"] = VenPendReviewMap;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("VendorPenndingReviewMapping", new { RefershFlag = 1 });
        }
        public ActionResult DeleteSelPendingReview(string CompCode, string Vendor_No, string SourceVendorName, int VendorId, string VendorName, string ModifiedBy)
        {
            try
            {
                PenndingVendorApproverMap VenPendReviewMap = new PenndingVendorApproverMap();
                VenPendReviewMap.lstApprover = new List<PenndingVendorApproverMap>();
                if (Session["VendorReviewList"] != null)
                {
                    VenPendReviewMap = Session["VendorReviewList"] as PenndingVendorApproverMap;
                }
                var actionToDelete = from actionRepDel in VenPendReviewMap.SelectApproverLst
                                     where actionRepDel.Vendor_No == Vendor_No && actionRepDel.CompName == CompCode
                                     select actionRepDel;
                VenPendReviewMap.SelectApproverLst.Remove(actionToDelete.ElementAt(0));
                if (VenPendReviewMap.lstApprover == null)
                    VenPendReviewMap.lstApprover = new List<PenndingVendorApproverMap>();

                PenndingVendorApproverMap Demo = new PenndingVendorApproverMap();
                Demo.VendorId = VendorId;
                Demo.VendorName = VendorName;
                Demo.Vendor_No = Vendor_No;
                Demo.SourceVenderName = SourceVendorName;
                Demo.CompCode = CompCode;
                Demo.ModifiedBy = ModifiedBy;
                VenPendReviewMap.lstApprover.Add(Demo);
                Session["VendorReviewList"] = VenPendReviewMap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("VendorPenndingReviewMapping", new { RefershFlag = 1 });
        }

        public ActionResult ApvRejVendorReview(int statusCode)
        {
            try
            {
                if (statusCode == 20)
                {
                    Session["VendorReviewList"] = null;
                    return RedirectToAction("VendorPenndingReviewMapping", new { RefershFlag = 0 });
                }
                string User_Id = User.Identity.GetUserId();
                PenndingVendorApproverMap VenPendApproverMap = new PenndingVendorApproverMap();
                if (Session["VendorReviewList"] != null)
                {
                    VenPendApproverMap = Session["VendorReviewList"] as PenndingVendorApproverMap;
                }
                VendorBL BLObj = new VendorBL();
                int errCode = BLObj.ApvRejVendorReview(VenPendApproverMap, statusCode, User_Id);
                Session["VendorReviewList"] = null;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                //Session["Vendor"] = "Please Select Atleast One Vendor";
                return RedirectToAction("VendorPenndingReviewMapping", new { RefershFlag = 1 });
            }
            return RedirectToAction("VendorPenndingReviewMapping", new { RefershFlag = 1 });
        }
        public ActionResult RefreshVenReviewList()
        {
            try
            {
                return RedirectToAction("VendorPenndingReviewMapping", new { RefershFlag = 0 });
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion DW Pendding Mapping Review

        #region FreightForwarder
        public ActionResult GetFreightForwarderList()
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWVendor/GetFreightForwarderList");
            if (FindFlag)
            {
                FreightForwarderModel obmodel = new FreightForwarderModel();
                List<FreightForwarderModel> lstf = new List<FreightForwarderModel>();
                try
                {
                    VendorBL vendorbl = new VendorBL();
                    lstf = vendorbl.FreightForvordarList();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(lstf);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportFreightForwarderLst")]
        [AcceptVerbs("POST")]
        public void ExportFreightForwarderLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            VendorBL BlObj = new VendorBL();
            var DataSource = BlObj.FreightForvordarList();
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        [HttpGet]
        public ActionResult CreateFreightForwarder(int FFId)
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWVendor/CreateFreightForwarder?FFId=0");
            if (FindFlag)
            {
                List<FreightForwarderModel> lstdrpvendorname = new List<FreightForwarderModel>();
                FreightForwarderModel ffmodel = new FreightForwarderModel();
                VendorBL vendorbl = new VendorBL();
                try
                {

                    lstdrpvendorname = vendorbl.DrpVendorList();
                    ViewBag.vendornamelist = new SelectList(lstdrpvendorname, "VendorId", "VendorName");
                    if (FFId > 0)
                    {
                        ffmodel = vendorbl.GetSelectedfreightlist(FFId);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(ffmodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateFreightForwarder(FormCollection fc, int FFId)
        {
            int errorcode;
            string User_Id = User.Identity.GetUserId();
            FreightForwarderModel fmodel = new FreightForwarderModel();
            try
            {
                fmodel.FFId = 0;
                if (FFId > 0)
                {
                    fmodel.FFId = FFId;
                }

                fmodel.VendorName = fc["VendorName"].ToString();
                VendorBL vendorbl = new VendorBL();


                if (fc["VendorName"].ToString() == "" && FFId > 0)
                {
                    errorcode = vendorbl.DeleteFreightFDetail(fmodel.FFId);
                }
                else
                {
                    errorcode = vendorbl.saveFreightForwardordetail(fmodel, User_Id);
                }

                if (errorcode == 500001 || errorcode == 500002 || errorcode == 500000)
                {
                    return RedirectToAction("GetFreightForwarderList");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        #endregion FreightForwarder

        #region Vendor Brand
        public ActionResult AllocateVendorBrand(int VendorId, string VendorName)
        {
            VendorBrandAllocateModel VendorBrandModel = new VendorBrandAllocateModel();
            VendorBL BLObj = new VendorBL();
            try
            {
                VendorBrandModel = BLObj.GetVendorAllocateList(VendorId);
                VendorBrandModel.vendorId = VendorId;
                VendorBrandModel.VendorName = VendorName;
                ViewBag.UnAllocateBrand = new SelectList(VendorBrandModel.UnAllocateBrand, "BrandId", "BrandName");
                ViewBag.AllocateBrand = new SelectList(VendorBrandModel.AllocateBrand, "BrandId", "BrandName");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(VendorBrandModel);
        }
        [HttpPost]
        public ActionResult AllocateVendorBrand(FormCollection fc)
        {
            string User_Id = User.Identity.GetUserId();
            int ErrCode = 0;
            int VendorId = 0;
            VendorBrandAllocateModel VendorBrandModel = new VendorBrandAllocateModel();
            VendorBrandModel.AllocateBrand = new List<VendorBrandAllocateModel>();
            foreach (string Key in fc.Keys)
            {
                VendorId = Convert.ToInt32(fc["vendorId"]);
                if (Key.ToString() == "AllocateBrand")
                {
                    List<string> LstAllocateBrand = fc["AllocateBrand"].Split(',').ToList();


                    foreach (var str in LstAllocateBrand)
                    {
                        VendorBrandAllocateModel BrandModel = new VendorBrandAllocateModel();
                        BrandModel.BrandId = Convert.ToInt32(str);
                        BrandModel.vendorId = Convert.ToInt32(VendorId);
                        VendorBrandModel.AllocateBrand.Add(BrandModel);
                    }
                }

            }
            VendorBL BLObj = new VendorBL();
            ErrCode = BLObj.SaveAllocatedVendorBrand(VendorBrandModel, User_Id, VendorId);
            return RedirectToAction("VendorList");
        }
        #endregion Vendor Brand

    }
}