using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.BOMAdmin;
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
    public class ItemDropdown
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
    }
    public class DWCustomerController : Controller
    {
        #region Create Contact Work

        public ActionResult CustomerList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWCustomer/CustomerList");
            if (FindFlag)
            {
                Session["CustomerMapping"] = null;
                Session["AddressList"] = null;
                CustomerListModel customerModel = new CustomerListModel();
                customerModel.customerGedList = new List<CustomerListModel>();
                List<ActiveCompanyModel> CompanyModel = new List<ActiveCompanyModel>();
                try
                {
                    CustomerBL objbl = new CustomerBL();
                    customerModel.customerGedList = objbl.GetCustomerList("");
                    CompanyModel = objbl.GetActiveCompanyList(UserId);
                    ViewBag.lstCompany = CompanyModel;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(customerModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public void ExportToExcel(string GridModel)
        {
            string user_Id = User.Identity.GetUserId();
            CustomerBL objbl = new CustomerBL();
            var DataSource = objbl.GetCustomerList("");
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
        public ActionResult CreateCustomer(int CustomerId, int TabIndex)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWCustomer/CreateCustomer?CustomerId=0&TabIndex=0");
            if (FindFlag)
            {
                if (Session["CustomerError"] != null)
                {
                    TempData["Message"] = Session["CustomerError"] as string;
                }
                Session["CustomerError"] = null;
                Session["AddressList"] = null;
                CustomerModel CustomerModel = new CustomerModel();
                CustomerBL objbl = new CustomerBL();
                CustomerModel = objbl.CustomerGetselected(CustomerId);
                CustomerModel.AddressList = objbl.GetSelectedAddressByCustomer(CustomerId);
                CustomerModel.ProductList = objbl.GetProductList(CustomerId);
                CustomerModel.TabIndex = TabIndex;
                ViewBag.LevelList = new SelectList(CustomerModel.LevelList, "LevelId", "Description");
                List<Paytermsmodel> PTList = new List<Paytermsmodel>();
                List<CPNModel> LstCPNModel = new List<CPNModel>();
                LstCPNModel = objbl.GetCPNList(CustomerId);
                ViewBag.LstCPN = LstCPNModel;
                BOMAdminBL BL = new BOMAdminBL();
                PTList = BL.GetPaymentTermsList(UserId);
                ViewBag.PTList = new SelectList(PTList, "PTId", "Description");
                AdminBL objBL = new AdminBL();
                List<SysEmploy> drp = new List<SysEmploy>();
                drp = objBL.GetEmployeeForDropDown(); // Employee List
                ViewBag.EmployeeList = new SelectList(drp, "EmpId", "EmpName");
                List<CustomerListModel> CustLst = new List<CustomerListModel>();
                CustLst = objbl.GetCustomerList("");
                ViewBag.CustomerList = CustLst;
                return View(CustomerModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateCustomer(FormCollection fc, bool IsActive, bool AuthorizedDealer, bool CommunicateToCSR)
        {

            CustomerBL objbl = new CustomerBL();
            string User_Id = User.Identity.GetUserId();
            CustomerModel CustomerModel = new CustomerModel();
            int errorCode = 0;

            CustomerModel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
            CustomerModel.CustomerName = fc["CustomerName"].ToString();
            CustomerModel.Region = fc["Region"].ToString();
            CustomerModel.IsActive = IsActive;
            CustomerModel.ModifiedBy = Convert.ToInt16(1);
            CustomerModel.AuthorizedDealer = AuthorizedDealer;
            CustomerModel.CommunicateToCSR = CommunicateToCSR;
            CustomerModel.PTId = Convert.ToInt32(fc["PTId"].ToString());
            CustomerModel.LevelId = Convert.ToInt32(fc["LevelId"].ToString());
            CustomerModel.SalesPersonId = Convert.ToInt32(fc["SalesPersonId"].ToString());

            errorCode = objbl.saveCustomer(CustomerModel, User_Id);
            if (errorCode > 0)
            {
                return RedirectToAction("CreateCustomer", new { CustomerId = errorCode, TabIndex = 0 });
            }
            return RedirectToAction("CustomerList");

        }

        public ActionResult Checkduplicacy(string CustomerName)
        {
            bool Result = true;
            CustomerBL objbl = new CustomerBL();
            Result = objbl.CheckDuplicacy(CustomerName);
            return Json(Result, JsonRequestBehavior.AllowGet);
        }
        #region Professional Information
        public ActionResult CreateProfessionalInfo(int CustomerId)
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
                CustomerModel customerModel = new CustomerModel();
                CustomerBL objbl = new CustomerBL();
                customerModel = objbl.CustomerGetselected(CustomerId);
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
        public ActionResult CreateProfessionalInfo(FormCollection fc)
        {
            try
            {
                CustomerBL objbl = new CustomerBL();
                string User_Id = User.Identity.GetUserId();
                CustomerModel CustomerModel = new CustomerModel();
                int errorCode = 0;

                CustomerModel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                CustomerModel.emailId = fc["emailId"].ToString();
                CustomerModel.VAT = fc["VAT"].ToString();
                CustomerModel.PAN = fc["PAN"].ToString();
                CustomerModel.CST = fc["CST"].ToString();
                CustomerModel.TAN = fc["TAN"].ToString();
                CustomerModel.ExciseRange = fc["ExciseRange"].ToString(); ;
                CustomerModel.ExciseDivision = fc["ExciseDivision"].ToString(); ;
                CustomerModel.ExciseNo = fc["ExciseNo"].ToString();
                CustomerModel.ExciseCommissionRate = fc["ExciseCommissionRate"].ToString();
                CustomerModel.Website = fc["Website"].ToString();
                CustomerModel.Remark = fc["Remark"].ToString();
                errorCode = objbl.saveCustomerProfessionalInfo(CustomerModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {
                    TempData["Msg"] = "Successfully Create ....";
                    return RedirectToAction("CreateCustomer", new { CustomerId = CustomerModel.CustomerId, TabIndex = 0 });
                }

                return RedirectToAction("CustomerList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Professional Information Work

        #region Customer  Contact Informations
        public ActionResult TEstCon(string CustomerId, string gfg)
        {
            string[] values = CustomerId.Split(',');
            return View();
        }
        public ActionResult CreateContactDetails(int CustomerId, string ContactName)
        {
              
            try
            {
                CustomerBL objbl = new CustomerBL();
                CustomerContactDetailsModel Model = new CustomerContactDetailsModel();
                if (ContactName != "" && ContactName != null)
                {
                    Model = objbl.GetSelectedContactDetails(CustomerId, ContactName);
                    Model.NewContactName = Model.ContactName;
                }
                else
                {
                    Model.CustomerId = CustomerId;
                    Model.NewContactName = "New Entry";
                    //Model.BirthDate = Convert.ToDateTime(System.DateTime.Today);
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
        public ActionResult CreateContactDetails(FormCollection fc)
        {
            try
            {
                CustomerBL objbl = new CustomerBL();
                string User_Id = User.Identity.GetUserId();
                CustomerContactDetailsModel ContactModel = new CustomerContactDetailsModel();
                int errorCode = 0;
                ContactModel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                ContactModel.ContactName = fc["ContactName"].ToString();
                ContactModel.NewContactName = fc["NewContactName"].ToString();
                ContactModel.Designation = fc["Designation"].ToString();
                ContactModel.email = fc["email"].ToString();
                ContactModel.MobileNo = fc["MobileNo"].ToString();
                ContactModel.PhoneNo = fc["PhoneNo"].ToString();
                ContactModel.Experties = fc["Experties"].ToString();
                ContactModel.Qualification = fc["Qualification"].ToString();
                if (fc["BirthDate"].ToString() != "")
                {
                    ContactModel.BirthDate = Convert.ToDateTime(fc["BirthDate"].ToString());
                }

                ContactModel.Remark = fc["Remark"].ToString();
                if (ContactModel.NewContactName != "New Entry")
                {
                    string temp = ContactModel.NewContactName;
                    ContactModel.NewContactName = ContactModel.ContactName;
                    ContactModel.ContactName = temp;
                }

                errorCode = objbl.saveContactDetailsInfo(ContactModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {
                    TempData["Msg"] = "Successfully Create ....";
                    return RedirectToAction("CreateCustomer", new { CustomerId = ContactModel.CustomerId, TabIndex = 1 });
                }
                else if (errorCode == 500000)
                {

                    Session["CustomerError"] = "This  Customer  Contact Already Created ";
                    return RedirectToAction("CreateCustomer", new { CustomerId = ContactModel.CustomerId, TabIndex = 1 });
                }
                return RedirectToAction("CreateCustomer", new { CustomerId = ContactModel.CustomerId, TabIndex = 1 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion Customer  Contact Informations

        #region BankDetail Work
        public ActionResult CreateBankDetails(int CustomerId, string AccountNo)
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
                CustomerBankDetailModel Model = new CustomerBankDetailModel();
                CustomerBL objbl = new CustomerBL();
                if (AccountNo != null)
                {
                    Model = objbl.GetSelectedBankDetail(CustomerId, AccountNo);
                    Model.NewAccountNo = Model.AccountNo;
                }
                else
                {
                    Model.CustomerId = CustomerId;
                    Model.NewAccountNo = "New Entry";
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
        public ActionResult CreateBankDetails(FormCollection fc)
        {
            try
            {
                CustomerBL objbl = new CustomerBL();
                string User_Id = User.Identity.GetUserId();
                CustomerBankDetailModel CustomerModel = new CustomerBankDetailModel();
                int errorCode = 0;
                CustomerModel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                CustomerModel.AccountNo = fc["AccountNo"].ToString();
                CustomerModel.NewAccountNo = fc["NewAccountNo"].ToString();
                CustomerModel.BankName = fc["BankName"].ToString();
                CustomerModel.Limit = Convert.ToDouble(fc["Limit"].ToString());
                if (CustomerModel.NewAccountNo != "New Entry")
                {
                    string temp = CustomerModel.NewAccountNo;
                    CustomerModel.NewAccountNo = CustomerModel.AccountNo;
                    CustomerModel.AccountNo = temp;
                }
                errorCode = objbl.saveBankDetailsInfo(CustomerModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {
                    TempData["Msg"] = "Successfully Create ....";
                    return RedirectToAction("CreateCustomer", new { CustomerId = CustomerModel.CustomerId, TabIndex = 2 });
                }
                else if (errorCode == 500000)
                {

                    Session["CustomerError"] = "This  Bank Account Number  Already Created ";
                    return RedirectToAction("CreateCustomer", new { CustomerId = CustomerModel.CustomerId, TabIndex = 2 });
                }
                return RedirectToAction("CustomerList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion BankDetail Work

        #region Libary Work

        public ActionResult CustomerLibary(int CustomerId, string IsKyc)
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
                CustomerLibaryModel Customermodel = new CustomerLibaryModel();
                Customermodel.isKYC = IsKyc;
                Customermodel.CustomerId = CustomerId;
                return PartialView(Customermodel);
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
        public ActionResult CustomerLibary(HttpPostedFileBase file, FormCollection fc)
        {
            CustomerLibaryModel Customermodel = new CustomerLibaryModel();
            try
            {
                string fileName = Path.GetFileName(file.FileName);
                string ftpServer = Common.GetConfigValue("FTP");
                string[] FileSplit = fileName.Split('.');
                string FileEx = FileSplit[FileSplit.Length - 1].ToString();
                String guid = Guid.NewGuid().ToString();
                string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                string FileName = Convert.ToString("/CustomerDoc/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;

                Customermodel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                Customermodel.isKYC = Convert.ToString(fc["isKYC"].ToString());
                if (Customermodel.isKYC == "KYC")
                {
                    Customermodel.KYC = Convert.ToBoolean(1);
                }
                else
                {
                    Customermodel.KYC = Convert.ToBoolean(0);
                }
                Customermodel.DocumentTitle = fc["DocumentTitle"].ToString();
                Customermodel.Description = fc["Description"].ToString();
                Customermodel.DocumentPath = FileName;
                //string localPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                string localPath = Server.MapPath("~/App_Data/uploads");
                if (!Directory.Exists(localPath))
                {
                    Directory.CreateDirectory(localPath);
                }
                file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName));
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
                            //FileInfo fileInfo = new FileInfo(localPath);
                            //FileStream fileStream = fileInfo.OpenRead();
                            int bufferLength = 2048;
                            byte[] buffer = new byte[file.InputStream.Length];
                            Stream uploadStream = requestFTPUploader.GetRequestStream();
                            //int contentLength = fileStream.Read(buffer, 0, bufferLength);
                            int contentLength = file.InputStream.Read(buffer, 0, Convert.ToInt32(file.InputStream.Length));
                            while (contentLength != 0)
                            {
                                uploadStream.Write(buffer, 0, contentLength);
                                contentLength = file.InputStream.Read(buffer, 0, bufferLength);
                            }
                            uploadStream.Close();
                            //fileStream.Close();
                            requestFTPUploader = null;
                            CustomerBL BLobj = new CustomerBL();
                            string UserId = User.Identity.GetUserId();
                            int ErrCode = BLobj.SaveLibraryDetailInfo(Customermodel, UserId);
                            if (ErrCode == 500002 || ErrCode == 500001)
                            {
                                return RedirectToAction("CreateCustomer", new { CustomerId = Customermodel.CustomerId, TabIndex = 4 });
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
                                return RedirectToAction("CreateCustomer", new { CustomerId = Customermodel.CustomerId, TabIndex = 4 });
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
            return RedirectToAction("CreateCustomer", new { CustomerId = Customermodel.CustomerId, TabIndex = 4 });
        }
        public ActionResult Download(int CustomerId, string DocumentPath)
        {


            for (int i = 0; i <= 2; i++)
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
                    break;
                }
                catch (Exception e)
                {
                    if (i > 2)
                    {
                        Session["CustomerError"] = e.Message.ToString();
                    }
                }
            }
            return RedirectToAction("CreateCustomer", new { CustomerId = CustomerId, TabIndex = 4 });
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

        #region Address Work
       
        public ActionResult CreateAddress(int CustomerId, int AddressId)
        {
            AddressModel addressModel = new AddressModel();
            try
            {
                CustomerBL objbl = new CustomerBL();
                if (Session["AddressList"] != null)
                {
                    addressModel = Session["AddressList"] as AddressModel;
                }
                else
                {
                    if (AddressId > 0)
                    {
                        addressModel = objbl.GetAddressSelectedList(CustomerId, AddressId);
                    }
                    addressModel.LstState = new List<StateModel>();
                    addressModel.LstCity = new List<CityModel>();
                    addressModel.LstCountry = objbl.GetCountryList();
                    addressModel.CustomerId = CustomerId;
                    if (addressModel.Country != null)
                    {
                        addressModel.LstState = objbl.GetStateList(addressModel.CountryId);

                    }
                    if (addressModel.State != null)
                    {
                        addressModel.LstCity = objbl.GetCityList(addressModel.StateId);
                    }
                }
                if (AddressId > 0)
                {
                    ViewBag.CountryList = new SelectList(addressModel.LstCountry, "CountryId", "CountryName", "India");

                }
                else
                {
                    ViewBag.CountryList = new SelectList(addressModel.LstCountry, "CountryId", "CountryName");
                }

                ViewData["country"] = addressModel.LstCountry.ToList();
                if (addressModel.LstState != null)
                {
                    ViewBag.StateList = new SelectList(addressModel.LstState, "StateId", "StateName", addressModel.State);
                }
                if (addressModel.LstCity != null)
                {
                    ViewBag.CityList = new SelectList(addressModel.LstCity, "CityId", "CityName", addressModel.City);
                    Session["AddressList"] = null;
                }
                //if(addressModel.PhotoPath !=null)
                //{
                //    String FTPServer = Common.GetConfigValue("FTP");
                //    String FTPUserId = Common.GetConfigValue("FTPUid");
                //    String FTPPwd = Common.GetConfigValue("FTPPWD");

                //    FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + addressModel.PhotoPath);
                //    requestFileDownload.UseBinary = true;
                //    requestFileDownload.KeepAlive = false;
                //    requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                //    requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                //    FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                //    Stream responseStream = responseFileDownload.GetResponseStream();
                //    addressModel.Photobyte = ReadFully(responseStream);
                //    responseStream.Close();                                       
                //    Response.End();                  
                //}
            }

            catch (Exception ex)
            {

                throw ex;
            }

            return PartialView(addressModel);
        }
        public ActionResult CreateNewAddress(int CustomerId)
        {
            AddressModel addressModel = new AddressModel();
            try
            {
                CustomerBL objbl = new CustomerBL();
                if (Session["AddressList"] != null)
                {
                    addressModel = Session["AddressList"] as AddressModel;
                }
                else
                {

                    addressModel.LstState = new List<StateModel>();
                    addressModel.LstCity = new List<CityModel>();
                    addressModel.LstCountry = objbl.GetCountryList();

                    if (addressModel.Country != null)
                    {
                        addressModel.LstState = objbl.GetStateList(addressModel.CountryId);

                    }
                    if (addressModel.State != null)
                    {
                        addressModel.LstCity = objbl.GetCityList(addressModel.StateId);
                    }

                    ViewBag.CountryList = new SelectList(addressModel.LstCountry, "CountryId", "CountryName");
                }


                if (addressModel.LstState != null)
                {
                    ViewBag.StateList = new SelectList(addressModel.LstState, "StateId", "StateName");
                }
                if (addressModel.LstCity != null)
                {
                    ViewBag.CityList = new SelectList(addressModel.LstCity, "CityId", "CityName");

                }
            }

            catch (Exception)
            {

                throw;
            }

            return PartialView(addressModel);
        }
        [HttpPost]
        public ActionResult CreateNewAddress(HttpPostedFileBase file, FormCollection fc, bool isPrimary)
        {
            AddressModel objmodel = new AddressModel();
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");
            string fileName;
            fileName = "";
            try
            {
                if (file != null)
                {
                    fileName = Path.GetFileName(file.FileName);
                    if (file.ContentLength > 0)
                    {
                        string[] FileSplit = fileName.Split('.');
                        string FileEx = FileSplit[1].ToString();
                        String guid = Guid.NewGuid().ToString();
                        string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                        string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        string FileName = Convert.ToString(FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
                        string localPath = Server.MapPath("~/App_Data/uploads");
                        if (!Directory.Exists(localPath))
                        {
                            Directory.CreateDirectory(localPath);
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
                                    //FileInfo fileInfo = new FileInfo(localPath);
                                    //FileStream fileStream = fileInfo.OpenRead();
                                    int bufferLength = 2048;
                                    byte[] buffer = new byte[file.InputStream.Length];
                                    Stream uploadStream = requestFTPUploader.GetRequestStream();
                                    //int contentLength = fileStream.Read(buffer, 0, bufferLength);
                                    int contentLength = file.InputStream.Read(buffer, 0, Convert.ToInt32(file.InputStream.Length));
                                    while (contentLength != 0)
                                    {
                                        uploadStream.Write(buffer, 0, contentLength);
                                        contentLength = file.InputStream.Read(buffer, 0, bufferLength);
                                    }
                                    uploadStream.Close();
                                    //fileStream.Close();
                                    requestFTPUploader = null;
                                    objmodel.PhotoPath = FileName;
                                    break;
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                }
                CustomerBL objbl = new CustomerBL();
                int errorcode = 0;
                objmodel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                if (objmodel.CustomerId > 0)
                {
                    objmodel.AddressId = Convert.ToInt32(fc["AddressId"]);
                }
                else
                {
                    objmodel.AddressId = 0;
                }
                objmodel.Line1 = fc["Line1"].ToString();
                objmodel.Line2 = fc["Line2"].ToString();
                objmodel.StateId = Convert.ToInt32(fc["StateId"].ToString());
                objmodel.CityId = Convert.ToInt32(fc["CityId"].ToString());
                objmodel.CountryId = Convert.ToInt32(fc["CountryId"].ToString());
                objmodel.Pin = fc["Pin"].ToString();
                objmodel.Description = fc["Description"].ToString();
                objmodel.LandMark = fc["LandMark"].ToString();
                if (fc["Photopath"] != null)
                    objmodel.PhotoPath = fileName;
                // objmodel.isPrimary = Convert.ToDouble(fc["isPrimary"].ToString());
                errorcode = objbl.SaveAddressDeatil(objmodel, isPrimary, objmodel.Description);
                if (errorcode == 500001 || errorcode == 500002)
                {
                    Session["AddressList"] = null;
                    TempData["Msg"] = "Successfully Create...";
                    return RedirectToAction("CreateCustomer", new { CustomerId = objmodel.CustomerId, TabIndex = 3 });
                }
                else
                {
                    if (file.ContentLength > 0)
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            try
                            {
                                FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(ftpServer + fileName);
                                requestFileDelete.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                                FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                                requestFileDelete = null;
                                Session["CustomerError"] = "This  Document Title  Already Created ";
                                break;
                            }
                            catch (Exception ex)
                            {
                                if (x > 2)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("CreateCustomer", new { CustomerId = objmodel.CustomerId, TabIndex = 3 });

        }
        public ActionResult SaveCreateAddress(int AddressId, int CustomerId, string Line1, string Line2, string Pin, string LandMark, string Country, string State, string City, string Description, bool isPrimary)
        {
            AddressModel objmodel = new AddressModel();
            try
            {
                CustomerBL objbl = new CustomerBL();
                int errorcode = 0;
                objmodel.CustomerId = CustomerId;
                if (objmodel.CustomerId > 0)
                {
                    objmodel.AddressId = AddressId;
                }
                else
                {
                    objmodel.AddressId = 0;
                }
                objmodel.Line1 = Line1;
                objmodel.Line2 = Line2;
                objmodel.StateId = Convert.ToInt32(State);
                objmodel.CityId = Convert.ToInt32(City);
                objmodel.CountryId = Convert.ToInt32(Country);
                objmodel.Pin = Pin;
                objmodel.Description = Description;
                objmodel.LandMark = LandMark;
                errorcode = objbl.SaveAddressDeatil(objmodel, isPrimary, Description);
                if (errorcode == 500001 || errorcode == 500002)
                {
                    Session["AddressList"] = null;
                    TempData["Msg"] = "Successfully Create...";
                    return RedirectToAction("CreateCustomer", new { CustomerId = objmodel.CustomerId, TabIndex = 3 });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CreateCustomer", new { CustomerId = objmodel.CustomerId, TabIndex = 3 });
        }
        
        public JsonResult GetStateList(int Country)
        {
            AddressModel addressModel = new AddressModel();
            List<StateModel> lst = new List<StateModel>();
            try
            {
                CustomerBL objbl = new CustomerBL();

                if (Session["AddressList"] != null)
                {
                    addressModel = Session["AddressList"] as AddressModel;
                }
                addressModel.CountryId = Country;
                lst = objbl.GetStateList(Country);
                Session["AddressList"] = addressModel;
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
            AddressModel addressModel = new AddressModel();
            try
            {
                CustomerBL objbl = new CustomerBL();

                if (Session["AddressList"] != null)
                {
                    addressModel = Session["AddressList"] as AddressModel;
                }

                addressModel.StateId = State;
                addressModel.LstCity = objbl.GetCityList(State);
                Session["AddressList"] = addressModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(addressModel.LstCity, "CityId", "CityName"));
        }
        #endregion Address Work

        #region Product Work
       
        public ActionResult CreateProduct(int CustomerId, string CustomerName)
        {
            ProductModel objmodel = new ProductModel();
            CustomerBL objbl = new CustomerBL();
            objmodel.CustomerId = CustomerId;
            objmodel.CustomerName = CustomerName;
            objmodel.ItemList = objbl.ProductGetselected(CustomerId);
            ViewBag.EquipmentLst = new SelectList(objmodel.ItemList, "EquipmentId", "EquipmentName");
            return PartialView(objmodel);
        }
        [HttpPost]
        public ActionResult CreateProduct(FormCollection fc)
        {
            ProductModel objmodel = new ProductModel();
            string User_Id = User.Identity.GetUserId();
            CustomerBL BLobj = new CustomerBL();
            objmodel.CustomerId = Convert.ToInt32(fc["CustomerId"]);
            objmodel.EquipmentId = Convert.ToInt32(fc["EquipmentId"]);
            objmodel.TAM = Convert.ToInt32(fc["TAM"]);
            int errorcode = BLobj.SaveproductDetails(objmodel, User_Id);
            if (errorcode != 0 && errorcode != 500000)
            {

                return RedirectToAction("CreateCustomer", new { CustomerId = objmodel.CustomerId, TabIndex = 5 });

            }

            else if (errorcode == 500000)
            {

                Session["CustomerError"] = "This  Product  Name Already Created ";
                return RedirectToAction("CreateCustomer", new { CustomerId = objmodel.CustomerId, TabIndex = 5 });
            }
            return RedirectToAction("CreateCustomer", new { CustomerId = objmodel.CustomerId, TabIndex = 5 });


        }
        public ActionResult DeleteCustomerEquipment(int CustomerId, int EquipmentId)
        {
            CustomerBL objbl = new CustomerBL();
            int errcode = objbl.DeleteCustomerEquipment(CustomerId, EquipmentId);
            return RedirectToAction("CreateCustomer", new { CustomerId = CustomerId, TabIndex = 5 });
        }
        public ActionResult selectedParam(int ItemId, double Quantity, int productid, string productname)
        {
            string User_Id = User.Identity.GetUserId();

            CustomerBL objbl = new CustomerBL();
            int errcode = objbl.saveItemDetails(User_Id, ItemId, Quantity, productid, productname);
            return View();
        }
        #endregion Product Work

        #region Certification Work
       
        public ActionResult CreateCertificationDetails(int CustomerId, string Certification)
        {

            try
            {
                CustomerBL objbl = new CustomerBL();
                CustomerCertificationModel Model = new CustomerCertificationModel();
                if (Certification != null)
                {
                    Model = objbl.GetSelectedCertificationDetails(CustomerId, Certification);
                    Model.NewCustomerCertification = Model.CustomerCertification;
                }
                else
                {
                    Model.CustomerId = CustomerId;
                    Model.NewCustomerCertification = "New Entry";
                    Model.CertificateDate = Convert.ToDateTime(System.DateTime.Today);
                }
                Model.CertificateDate = Convert.ToDateTime(Model.CertificateDate.ToShortDateString());
                return PartialView(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult CreateCertificationDetails(FormCollection fc)
        {
            try
            {
                CustomerBL objbl = new CustomerBL();
                string User_Id = User.Identity.GetUserId();
                CustomerCertificationModel CertificationModel = new CustomerCertificationModel();
                int errorCode = 0;
                CertificationModel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                CertificationModel.CustomerCertification = fc["CustomerCertification"].ToString();
                CertificationModel.NewCustomerCertification = fc["NewCustomerCertification"].ToString();
                CertificationModel.CertificateDate = Convert.ToDateTime(fc["CertificateDate"].ToString());
                if (CertificationModel.NewCustomerCertification != "New Entry")
                {
                    string temp = CertificationModel.NewCustomerCertification;
                    CertificationModel.NewCustomerCertification = CertificationModel.CustomerCertification;
                    CertificationModel.CustomerCertification = temp;
                }
                errorCode = objbl.saveCertificationDetailsInfo(CertificationModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {
                    TempData["Msg"] = "Successfully Create ....";
                    return RedirectToAction("CreateCustomer", new { CustomerId = CertificationModel.CustomerId, TabIndex = 6 });
                }
                else if (errorCode == 500000)
                {

                    Session["CustomerError"] = "This  Customer  Certification Already Created ";
                    return RedirectToAction("CreateCustomer", new { CustomerId = CertificationModel.CustomerId, TabIndex = 6 });
                }
                return RedirectToAction("CreateCustomer", new { CustomerId = CertificationModel.CustomerId, TabIndex = 6 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Certification Work

        #region TurnOver Work
      
        public ActionResult CreateTurnoverDetails(int CustomerId, string TurnoverYear)
        {

            finanacialYear yr = new finanacialYear();

            List<string> FyYear = new List<string>();
            FyYear = yr.FY().ToList();

            try
            {
                CustomerBL objbl = new CustomerBL();
                ViewBag.FYYear = new SelectList(FyYear);
                CustomerTurnoverModel Model = new CustomerTurnoverModel();
                if (TurnoverYear != "")
                {
                    Model = objbl.GetSelectedTurnoverDetails(CustomerId, TurnoverYear);
                    Model.NewTurnoverYear = (string)Model.TurnoverYear;

                }
                else
                {
                    Model.CustomerId = CustomerId;
                    Model.NewTurnoverYear = "";
                }
                return PartialView(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult CreateTurnoverDetails(FormCollection fc)
        {
            try

            {

                CustomerBL objbl = new CustomerBL();
                string User_Id = User.Identity.GetUserId();
                CustomerTurnoverModel TurnoverModel = new CustomerTurnoverModel();
                int errorCode = 0;
                TurnoverModel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                TurnoverModel.TurnoverYear = fc["TurnoverYear"].ToString();
                TurnoverModel.NewTurnoverYear = fc["NewTurnoverYear"].ToString();
                TurnoverModel.Turnover = Convert.ToDouble(fc["Turnover"].ToString());
                TurnoverModel.ProjectedTurnover = Convert.ToDouble(fc["ProjectedTurnover"].ToString());
                if (TurnoverModel.NewTurnoverYear != "")
                {
                    string temp = TurnoverModel.NewTurnoverYear;
                    TurnoverModel.NewTurnoverYear = TurnoverModel.TurnoverYear;
                    TurnoverModel.TurnoverYear = temp;
                }
                errorCode = objbl.saveTurnoverDetailsInfo(TurnoverModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {
                    TempData["Msg"] = "Successfully Create ....";
                    return RedirectToAction("CreateCustomer", new { CustomerId = TurnoverModel.CustomerId, TabIndex = 6 });
                }
                else if (errorCode == 500000)
                {

                    Session["CustomerError"] = "This  Customer  Turnover Already Created ";
                    return RedirectToAction("CreateCustomer", new { CustomerId = TurnoverModel.CustomerId, TabIndex = 6 });
                }
                return RedirectToAction("CreateCustomer", new { CustomerId = TurnoverModel.CustomerId, TabIndex = 6 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion TurnOver Work

        #region Competitor Work
        public ActionResult CreateCompetitorDetails(int CustomerId, int CompetitorId)
        {

            try
            {
                CustomerBL objbl = new CustomerBL();
                CustomerCompetitorModel Model = new CustomerCompetitorModel();
                if (CompetitorId != 0)
                {
                    Model = objbl.GetSelectedCompetitorDetails(CustomerId, CompetitorId);
                    Model.NewCompetitorId = (int)Model.CompetitorId;
                }
                else
                {
                    Model.CustomerId = CustomerId;
                    Model.NewCompetitorId = 0;
                }
                List<CustomerCompetitorModel> CompetitorList = new List<CustomerCompetitorModel>();
                CompetitorList = objbl.GetCompetitorList();
                ViewBag.CompetitorLst = new SelectList(CompetitorList, "CustomerId", "CustomerName", Model.CompetitorId);
                return PartialView(Model);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
     
        [HttpPost]
        public ActionResult CreateCompetitorDetails(FormCollection fc)
        {
            try
            {
                CustomerBL objbl = new CustomerBL();
                string User_Id = User.Identity.GetUserId();
                CustomerCompetitorModel TurnoverModel = new CustomerCompetitorModel();
                int errorCode = 0;
                TurnoverModel.CustomerId = Convert.ToInt32(fc["CustomerId"].ToString());
                TurnoverModel.CompetitorId = Convert.ToInt32(fc["CompetitorId"].ToString());
                TurnoverModel.NewCompetitorId = Convert.ToInt32(fc["NewCompetitorId"].ToString());
                if (TurnoverModel.NewCompetitorId > 0)
                {
                    int temp = TurnoverModel.NewCompetitorId;
                    TurnoverModel.NewCompetitorId = TurnoverModel.CompetitorId;
                    TurnoverModel.CompetitorId = temp;
                }
                errorCode = objbl.saveCompetitorDetailsInfo(TurnoverModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {
                    TempData["Msg"] = "Successfully Create ....";
                    return RedirectToAction("CreateCustomer", new { CustomerId = TurnoverModel.CustomerId, TabIndex = 6 });
                }
                else if (errorCode == 500000)
                {

                    Session["CustomerError"] = "This  Customer  Competitor Already Created ";
                    return RedirectToAction("CreateCustomer", new { CustomerId = TurnoverModel.CustomerId, TabIndex = 6 });
                }
                return RedirectToAction("CreateCustomer", new { CustomerId = TurnoverModel.CustomerId, TabIndex = 6 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Competitor Work

        #region Assign UserId

        public ActionResult AssignCustomerUserId(int CustomerId,int CustomerContactId, int userId, string UserName )
        {
            CustUserModel UserModel = new CustUserModel();
            try
            {
                CustomerBL BLObj = new CustomerBL();
                UserModel = BLObj.GetCustUserList(CustomerContactId);
                UserModel.CustomerContactId = CustomerContactId;
                UserModel.UserId = userId;
                UserModel.CustomerId = CustomerId;
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
        public ActionResult SaveAssignCustomerUserId(int userId, int CustomerContactId, int CustomerId)
        {
            int errCode = 0;
            try
            {
                CustomerBL BLObj = new CustomerBL();
                errCode = BLObj.SaveAssignCustomerUserId(userId, CustomerContactId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RedirectToAction("CreateCustomer", new { CustomerId = CustomerId, TabIndex = 1 });
        }
        public ActionResult DeleteAssignCustomerUserId(int CustomerContactId)
        {
            int errCode = 0;
            try
            {
                CustomerBL BLObj = new CustomerBL();
                errCode = BLObj.DeleteAssignCustomerUserId(CustomerContactId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
        #endregion Assign UserId


        #region CPN
        public ActionResult CreateCPN(int CustomerId, int ItemId)
        {
            CPNModel CpnModel = new CPNModel();
            CustomerBL objbl = new CustomerBL();
            List<CPNModel> LstItem = new List<CPNModel>();
            try
            {
                CpnModel.CustomerId = CustomerId;
                CpnModel.ItemId = ItemId;
                if (CustomerId > 0 && ItemId > 0)
                {
                    CpnModel = objbl.GetSelectedCPNList(CustomerId, ItemId);
                }
                else
                {
                    LstItem = objbl.GetItemListforCPN(CustomerId);
                    ViewBag.LstItem = new SelectList(LstItem, "ItemId", "ItemName");
                    List<ItemDropdown> lstitems = new List<ItemDropdown>();
                    foreach (CPNModel c in LstItem)
                    {
                        ItemDropdown Item = new ItemDropdown();
                        Item.ItemId = c.ItemId;
                        Item.ItemName = c.ItemName + " - " + c.ItemId.ToString();
                        lstitems.Add(Item);
                    }
                    ViewBag.NEwItemList = lstitems;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView(CpnModel);
        }
        [HttpPost]
        public ActionResult CreateCPN(FormCollection fc)
        {
            CPNModel CpnModel = new CPNModel();
            CustomerBL objbl = new CustomerBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                string itemname = fc["ItemId"];
                if (itemname.LastIndexOf("-") > 0)
                {
                    string[] itm = itemname.Split('-');

                    CpnModel.CustomerId = Convert.ToInt32(fc["CustomerId"]);
                    CpnModel.ItemId = Convert.ToInt32(itm[itm.Count() - 1].Trim());
                    CpnModel.CPN = fc["CPN"].ToString();
                    int ErrCode = objbl.saveCPNDetail(CpnModel.CustomerId, CpnModel.ItemId, CpnModel.CPN, User_Id);
                }
                else
                {
                    CpnModel.CustomerId = Convert.ToInt32(fc["CustomerId"]);
                    CpnModel.ItemId = Convert.ToInt32(fc["ItemId"]);
                    CpnModel.CPN = fc["CPN"].ToString();
                    int ErrCode = objbl.saveCPNDetail(CpnModel.CustomerId, CpnModel.ItemId, CpnModel.CPN, User_Id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CreateCustomer", new { CustomerId = CpnModel.CustomerId, TabIndex = 0 });
        }
        #endregion CPN

        #endregion  Create Contact Work

        #region Customer Mapping

        public ActionResult DWCustomerMapping()
        {
            if (Session["CustomerError"] != null)
            {
                TempData["Message"] = Session["CustomerError"] as string;
            }
            Session["CustomerError"] = null;
            CustomerMappingModel CustomerMap = new CustomerMappingModel();
            CustomerBL BlObj = new CustomerBL();
            try
            {
                if (Session["CustomerMapping"] != null)
                {
                    CustomerMap = Session["CustomerMapping"] as CustomerMappingModel;
                    CustomerMap.lstCustomerMap = new List<CustomerMappingModel>();
                    CustomerMap.lstCustomerMap = BlObj.GetDWCompList();
                    foreach (SysNavRelCustomerListModel demo in CustomerMap.GridList)
                    {
                        var actionToDelete = from SeleCustomer in CustomerMap.lstCustomerMap
                                             where SeleCustomer.CompCode.ToUpper() == demo.CompName.ToUpper()
                                             select SeleCustomer;
                        CustomerMap.lstCustomerMap.Remove(actionToDelete.ElementAt(0));

                    }
                    ViewBag.VendorMapList = new SelectList(CustomerMap.lstCustomerMap, "CompCode", "CompName");
                    if (CustomerMap.lstClientMapCustomerList != null)
                        ViewBag.ComVendorList = new SelectList(CustomerMap.lstClientMapCustomerList, "ClientCustomerID", "ClientCustomerName");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(CustomerMap);
        }
        public ActionResult GetCustomerMappingList(int CustomerId, string CustomerName)
        {
            CustomerMappingModel CustomerMap = new CustomerMappingModel();
            CustomerBL BlObj = new CustomerBL();

            CustomerMap.GridList = BlObj.GetCustomerNavRelList(CustomerId);
            CustomerMap.CustomerName = CustomerName;
            CustomerMap.CustomerId = CustomerId;
            foreach (SysNavRelCustomerListModel DWVendor in CustomerMap.GridList)
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
                string StrTemp = BlObj.GetClientCustomerName(DWVendor.CompName, DWVendor.CompCustomerID, ConnInfo);
                DWVendor.CompCustomerName = StrTemp;
            }
            Session["CustomerMapping"] = CustomerMap;
            return RedirectToAction("DWCustomerMapping");

        }
        public ActionResult GetClientCustomerList(string CompCode)
        {

            CustomerMappingModel CustomerMap = new CustomerMappingModel();
            CustomerBL BlObj = new CustomerBL();
            CustomerMap = Session["CustomerMapping"] as CustomerMappingModel;
            CustomerMap.lstCustomerMap = BlObj.GetDWCompList();
            string SPName = "";
            Int16 ConnId = 0;

            foreach (CustomerMappingModel TempModel in CustomerMap.lstCustomerMap)
            {
                if (CompCode == TempModel.CompCode)
                {
                    SPName = TempModel.VendorListSP;
                    ConnId = TempModel.ConnectionId;
                    CustomerMap.selectedComp = CompCode;
                    CustomerMap.AutoMapBtn = TempModel.SaveCustomerSP;
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


            CustomerMap.lstClientMapCustomerList = BlObj.GetCompanyCustomer(CompCode, SPName, ConnInfo);
            Session["CustomerMapping"] = CustomerMap;
            return RedirectToAction("DWCustomerMapping");
        }

        public ActionResult GetAutoMapping(string CustomerName, string CompCode, int CustomerId)
        {
            try
            {
                string User_Id = User.Identity.GetUserId();
                CustomerMappingModel CustomerMap = new CustomerMappingModel();
                CustomerBL BlObj = new CustomerBL();
                CustomerMap.lstCustomerMap = BlObj.GetDWCompList();
                string SPName = "";
                Int16 ConnId = 0;
                foreach (CustomerMappingModel TempModel in CustomerMap.lstCustomerMap)
                {
                    if (CompCode == TempModel.CompCode)
                    {
                        SPName = TempModel.SaveCustomerSP;
                        ConnId = TempModel.ConnectionId;
                        CustomerMap.selectedComp = CompCode;
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

                int errcode = BlObj.GetAutoMapping(User_Id, CompCode, CustomerName, CustomerId, SPName, ConnInfo);
                if (errcode == 500004)
                {
                    return RedirectToAction("GetCustomerMappingList", new { CustomerId = CustomerId, CustomerName = CustomerName });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetCustomerMappingList", new { CustomerId = CustomerId, CustomerName = CustomerName });
        }
        public ActionResult SaveCustomerMapping(string Customer_No, string CompCode, int CustomerId, string CustomerName)
        {
            try
            {
                if (Customer_No == null || Customer_No == "" || CompCode == null || CompCode == "" || CustomerId == 0)
                {
                    return RedirectToAction("CustomerList");
                }
                string Errorcode = "0";
                CustomerMappingModel CustomerMap = new CustomerMappingModel();
                string User_Id = User.Identity.GetUserId();
                CustomerBL BlObj = new CustomerBL();
                CustomerMap.CompCustomerID = Customer_No;
                CustomerMap.CompCode = CompCode;
                CustomerMap.CustomerId = CustomerId;
                Errorcode = BlObj.SaveCustomerMapping(CustomerMap, User_Id);
                if (Errorcode == "500002" || Errorcode == "500001")
                {
                    return RedirectToAction("GetCustomerMappingList", new { CustomerId = CustomerId, CustomerName = CustomerName });
                }
                else if (Errorcode != "0")
                {

                    Session["CustomerError"] = "This External Customer  Already Mapped With " + Errorcode;
                    return RedirectToAction("GetCustomerMappingList", new { CustomerId = CustomerId, CustomerName = CustomerName });
                }
                else
                {
                    return RedirectToAction("GetCustomerMappingList", new { VendorId = CustomerId, CustomerName = CustomerName });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public ActionResult DeleteCustomerMapping(string CompName, int CustomerId, string CustomerName)
        {
            int Errorcod = 0;
            CustomerMappingModel CustomerMap = new CustomerMappingModel();
            CustomerBL BlObj = new CustomerBL();
            Errorcod = BlObj.DeleteCustomerMapping(CompName, CustomerId);
            if (Errorcod == 500003)
            {
                CustomerMap.GridList = BlObj.GetCustomerNavRelList(CustomerId);
                CustomerMap.CustomerName = CustomerName;
                CustomerMap.CustomerId = CustomerId;
                foreach (SysNavRelCustomerListModel DWVendor in CustomerMap.GridList)
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
                    string StrTemp = BlObj.GetClientCustomerName(DWVendor.CompName, DWVendor.CompCustomerID, ConnInfo);
                    DWVendor.CompCustomerName = StrTemp;
                }
                Session["CustomerMapping"] = CustomerMap;
                return RedirectToAction("DWCustomerMapping");

            }
            else
            {
                return RedirectToAction("DWCustomerMapping");
            }
        }
        #endregion Customer Mapping

        #region DW Customer Pendding Approval
        [Authorize]
        public ActionResult CustomerPenndingApproverMapping(int RefershFlag)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWCustomer/CustomerPenndingApproverMapping?RefershFlag=0");
            if (FindFlag)
            {
                PenndingCustomerApproverMap CustPendApproverMap = new PenndingCustomerApproverMap();
                CustPendApproverMap.SelectApproverLst = new List<SelectPenddingCustomerApproveList>();
                CustPendApproverMap.lstApprover = new List<PenndingCustomerApproverMap>();
                try
                {
                    string User_Id = User.Identity.GetUserId();
                    CustomerBL BlObj = new CustomerBL();
                    if (RefershFlag == 0)
                    {
                        Session["CustomerApproverList"] = null;
                    }
                    if (Session["CustomerApproverList"] != null)
                    {
                        CustPendApproverMap = Session["CustomerApproverList"] as PenndingCustomerApproverMap;
                    }
                    CustPendApproverMap.lstApprover = BlObj.GetCustomerApproverList(User_Id, (int)Utility.Enums.StatusCode.REVIEWED);
                    if (CustPendApproverMap.SelectApproverLst != null)
                    {
                        foreach (SelectPenddingCustomerApproveList rrv in CustPendApproverMap.SelectApproverLst)
                        {
                            var actionToDelete = from actionRepDel in CustPendApproverMap.lstApprover
                                                 where actionRepDel.Customer_No == rrv.Customer_No && actionRepDel.CompCode == rrv.CompName
                                                 select actionRepDel;

                            CustPendApproverMap.lstApprover.Remove(actionToDelete.ElementAt(0));
                        }
                    }
                    Session["CustomerApproverList"] = CustPendApproverMap;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(CustPendApproverMap);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportCustomerPenndingApproverMapLst")]
        [AcceptVerbs("POST")]
        public void ExportCustomerPenndingApproverMapLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            CustomerBL BlObj = new CustomerBL();
            var DataSource = BlObj.GetCustomerApproverList(User_Id, (int)Utility.Enums.StatusCode.REVIEWED);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult AddtoVenList(string Customer_No, string SourceCustomerName, string CompName, int CustomerId, string CustomerName, string ModifiedBy)
        {
            try
            {

                PenndingCustomerApproverMap CustPendApproverMap = new PenndingCustomerApproverMap();
                SelectPenddingCustomerApproveList demo = new SelectPenddingCustomerApproveList();
                if (Session["CustomerApproverList"] != null)
                {
                    CustPendApproverMap = Session["CustomerApproverList"] as PenndingCustomerApproverMap;
                }
                if (CustPendApproverMap.SelectApproverLst == null)
                    CustPendApproverMap.SelectApproverLst = new List<SelectPenddingCustomerApproveList>();
                demo.CustomerId = CustomerId;
                demo.Customer_No = Customer_No;
                demo.CompName = CompName;
                demo.CustomerName = CustomerName;
                demo.SourceCustomerName = SourceCustomerName;
                demo.ModifiedBy = ModifiedBy;
                CustPendApproverMap.SelectApproverLst.Add(demo);
                Session["CustomerApproverList"] = CustPendApproverMap;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("CustomerPenndingApproverMapping", new { RefershFlag = 1 });
        }

        public ActionResult DeleteSelPendingApprover(string CompCode, string Customer_No)
        {
            try
            {
                PenndingCustomerApproverMap CustPendApproverMap = new PenndingCustomerApproverMap();
                CustPendApproverMap.lstApprover = new List<PenndingCustomerApproverMap>();
                if (Session["CustomerApproverList"] != null)
                {
                    CustPendApproverMap = Session["CustomerApproverList"] as PenndingCustomerApproverMap;
                }
                var actionToDelete = from actionRepDel in CustPendApproverMap.SelectApproverLst
                                     where actionRepDel.Customer_No == Customer_No && actionRepDel.CompName == CompCode
                                     select actionRepDel;
                CustPendApproverMap.SelectApproverLst.Remove(actionToDelete.ElementAt(0));
                Session["CustomerApproverList"] = CustPendApproverMap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CustomerPenndingApproverMapping", new { RefershFlag = 1 });
        }
        public ActionResult ApvRejCustomer(int statusCode)
        {
            try
            {
                if (statusCode == 20)
                {
                    Session["CustomerApproverList"] = null;
                    return RedirectToAction("CustomerPenndingApproverMapping", new { RefershFlag = 0 });
                }
                string User_Id = User.Identity.GetUserId();
                PenndingCustomerApproverMap VenPendApproverMap = new PenndingCustomerApproverMap();
                if (Session["CustomerApproverList"] != null)
                {
                    VenPendApproverMap = Session["CustomerApproverList"] as PenndingCustomerApproverMap;
                }
                CustomerBL BLObj = new CustomerBL();
                int errCode = BLObj.ApvRejCustomer(VenPendApproverMap, statusCode, User_Id);
                Session["CustomerApproverList"] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CustomerPenndingApproverMapping", new { RefershFlag = 1 });
        }
        public ActionResult RefreshCustApprovalList()
        {
            try
            {
                return RedirectToAction("CustomerPenndingApproverMapping", new { RefershFlag = 0 });
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion DW Customer Pendding Approval

        #region DW Customer Pendding Review
        [Authorize]
        public ActionResult CustomerPenndingReviewMapping(int RefershFlag)
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
            //if (Session["Customer"] != null)
            //{
            //    TempData["Message"] = Session["Customer"] as string;
            //}
            //Session["Customer"] = null;
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DWCustomer/CustomerPenndingReviewMapping?RefershFlag=0");
            if (FindFlag)
            {
                PenndingCustomerApproverMap CustPendApproverMap = new PenndingCustomerApproverMap();
                CustPendApproverMap.SelectApproverLst = new List<SelectPenddingCustomerApproveList>();
                CustPendApproverMap.lstApprover = new List<PenndingCustomerApproverMap>();
                try
                {
                    string User_Id = User.Identity.GetUserId();
                    CustomerBL BlObj = new CustomerBL();
                    if (RefershFlag == 0)
                    {
                        Session["CustomerReviewList"] = null;
                    }
                    if (Session["CustomerReviewList"] != null)
                    {
                        CustPendApproverMap = Session["CustomerReviewList"] as PenndingCustomerApproverMap;
                    }
                    CustPendApproverMap.lstApprover = BlObj.GetCustomerApproverList(User_Id, (int)Utility.Enums.StatusCode.NEW);
                    if (CustPendApproverMap.SelectApproverLst != null)
                    {
                        foreach (SelectPenddingCustomerApproveList rrv in CustPendApproverMap.SelectApproverLst)
                        {
                            String Customer_No = rrv.Customer_No;
                            String CompCode = rrv.CompName;
                            var actionToDelete = from actionRepDel in CustPendApproverMap.lstApprover
                                                 where actionRepDel.Customer_No == Customer_No && actionRepDel.CompCode == CompCode
                                                 select actionRepDel;
                            CustPendApproverMap.lstApprover.Remove(actionToDelete.ElementAt(0));
                        }
                    }

                    Session["CustomerReviewList"] = CustPendApproverMap;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(CustPendApproverMap);
            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportCustomerPenndingReviewMapLst")]
        [AcceptVerbs("POST")]
        public void ExportCustomerPenndingReviewMapLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            CustomerBL BlObj = new CustomerBL();
            var DataSource = BlObj.GetCustomerApproverList(User_Id, (int)Utility.Enums.StatusCode.NEW);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult AddtoVenReviewList(string Customer_No, string SourceCustomerName, string CompName, int CustomerId, string CustomerName, string ModifiedBy)
        {
            try
            {

                PenndingCustomerApproverMap CustPendApproverMap = new PenndingCustomerApproverMap();
                SelectPenddingCustomerApproveList demo = new SelectPenddingCustomerApproveList();
                if (Session["CustomerReviewList"] != null)
                {
                    CustPendApproverMap = Session["CustomerReviewList"] as PenndingCustomerApproverMap;
                }
                if (CustPendApproverMap.SelectApproverLst == null)
                    CustPendApproverMap.SelectApproverLst = new List<SelectPenddingCustomerApproveList>();
                demo.CustomerId = CustomerId;
                demo.Customer_No = Customer_No;
                demo.CompName = CompName;
                demo.CustomerName = CustomerName;
                demo.SourceCustomerName = SourceCustomerName;
                demo.ModifiedBy = ModifiedBy;
                CustPendApproverMap.SelectApproverLst.Add(demo);
                Session["CustomerReviewList"] = CustPendApproverMap;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RedirectToAction("CustomerPenndingReviewMapping", new { RefershFlag = 1 });
        }

        public ActionResult DeleteSelPendingReview(string CompCode, string Customer_No)
        {
            try
            {
                PenndingCustomerApproverMap CustPendApproverMap = new PenndingCustomerApproverMap();
                CustPendApproverMap.lstApprover = new List<PenndingCustomerApproverMap>();
                if (Session["CustomerReviewList"] != null)
                {
                    CustPendApproverMap = Session["CustomerReviewList"] as PenndingCustomerApproverMap;
                }
                var actionToDelete = from actionRepDel in CustPendApproverMap.SelectApproverLst
                                     where actionRepDel.Customer_No == Customer_No && actionRepDel.CompName == CompCode
                                     select actionRepDel;
                CustPendApproverMap.SelectApproverLst.Remove(actionToDelete.ElementAt(0));
                Session["CustomerReviewList"] = CustPendApproverMap;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CustomerPenndingReviewMapping", new { RefershFlag = 1 });
        }
        public ActionResult ApvRejCustomerReview(int statusCode)
        {
            try
            {
                if (statusCode == 20)
                {
                    Session["CustomerReviewList"] = null;
                    return RedirectToAction("CustomerPenndingReviewMapping", new { RefershFlag = 0 });
                }
                string User_Id = User.Identity.GetUserId();
                PenndingCustomerApproverMap VenPendApproverMap = new PenndingCustomerApproverMap();
                if (Session["CustomerReviewList"] != null)
                {
                    VenPendApproverMap = Session["CustomerReviewList"] as PenndingCustomerApproverMap;
                }
                CustomerBL BLObj = new CustomerBL();
                int errCode = BLObj.ApvRejCustomerReview(VenPendApproverMap, statusCode, User_Id);
                Session["CustomerReviewList"] = null;
            }
            catch (Exception ex)
            {
                throw ex;
                // Session["Customer"] = "Please Select Atleast One Customer";
            }
            return RedirectToAction("CustomerPenndingReviewMapping", new { RefershFlag = 1 });
        }
        public ActionResult RefreshCustReviewList()
        {
            try
            {
                return RedirectToAction("CustomerPenndingReviewMapping", new { RefershFlag = 0 });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion DW Customer Pendding Review
    }
}