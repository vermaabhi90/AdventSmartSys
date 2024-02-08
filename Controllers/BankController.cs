using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.Bank;
using SmartSys.BL.DW;
using Syncfusion.EJ.Export;
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
    public class BankController : Controller
    {
        // GET: Bank

        // GET: Bank

        public ActionResult BankListDetails()
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
                FindFlag = modelObj.FindMenu(lstTaskmodel, "/Bank/BankListDetails");
                if (FindFlag)
                {
                    BankDetailModel objmodel = new BankDetailModel();
                    objmodel.BankDetailList = new List<BankDetailModel>();
                    BankBL obj = new BankBL();
                    objmodel.BankDetailList = obj.bankDetailList(UserId);
                    return View(objmodel.BankDetailList);
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
        [System.Web.Http.ActionName("ExportBankdetailLst")]
        [AcceptVerbs("POST")]
        public void ExportBankdetailLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            BankDetailModel objmodel = new BankDetailModel();
            objmodel.BankDetailList = new List<BankDetailModel>();
            BankBL objBL = new BankBL();          
            var DataSource = objBL.bankDetailList(User_Id);
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
        public ActionResult CreateBankDetails()
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Bank/CreateBankDetails");
            if (FindFlag)
            {
                BankDetailModel objmodel = new BankDetailModel();

                try
                {
                    BankBL objbl = new BankBL();
                    objmodel.countryLst = objbl.GetCountryList();
                    ViewBag.CountryList = new SelectList(objmodel.countryLst, "CountryId", "CountryName");
                    ViewBag.CountryL = objmodel.countryLst;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult CreateBankDetails(FormCollection fc)
        {
            try
            {
                int errcode = 0;
                string User_Id = User.Identity.GetUserId();
                BankDetailModel objmodel = new BankDetailModel();
                objmodel.BankId = 0;
                objmodel.BankName = fc["BankName"].ToString();
                objmodel.IFSCCode = fc["IFSCCode"].ToString();
                objmodel.AddressId = 0;
                objmodel.Line1 = fc["Line1"].ToString();
                objmodel.Line2 = fc["Line2"].ToString();
                objmodel.LandMark = fc["LandMark"].ToString();
                objmodel.Countryid = Convert.ToInt32(fc["Country"].ToString());
                objmodel.StateId = Convert.ToInt32(fc["State"].ToString());
                objmodel.CityId = Convert.ToInt32(fc["City"].ToString());
                objmodel.Pin = fc["Pin"].ToString();

                BankBL objbl = new BankBL();
                errcode = objbl.SaveBankDetails(objmodel, User_Id);
                if (errcode == 4)
                {
                    // Session["taxmastermodel"] = null;
                    return RedirectToAction("BankListDetails");
                }
                else

                    TempData["Msg"] = "Occur some Error Please Try Again ...";
                return RedirectToAction("CreateBankDetails");


            }
            catch (Exception ex)
            {
                throw ex;
            }

            // return View();
        }
        public ActionResult EditBankDetails(int BankId)
        {
            try
            {
                BankDetailModel objmodel = new BankDetailModel();
                objmodel.ComList = new List<CompanyCodeModel>();
                BankBL objbl = new BankBL();
                objmodel = objbl.GetSelectedBankDetails(BankId);
                objmodel.AddressModelList = objbl.GetSelectedAddressList(objmodel.AddressId);
                return View(objmodel);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult EditBankDetails(FormCollection fc)
        {
            try
            {
                int errcode = 0;
                string User_Id = User.Identity.GetUserId();
                BankDetailModel objmodel = new BankDetailModel();
                BankBL objbl = new BankBL();
                objmodel.BankId = Convert.ToInt32(fc["BankId"].ToString());
                objmodel.BankName = fc["BankName"].ToString();
                objmodel.IFSCCode = fc["IFSCCode"].ToString();
                objmodel.AddressId = Convert.ToInt32(fc["AddressId"].ToString());
                errcode = objbl.SaveEditedBankDetails(objmodel, User_Id);
                if (errcode == 500001 || errcode == 500002)
                {
                    return RedirectToAction("BankListDetails");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }
        public ActionResult DeleteBankDetails(int BankId)
        {
            try
            {
                int errcode = 0;
                string User_Id = User.Identity.GetUserId();
                BankBL objbl = new BankBL();
                errcode = objbl.DeleteBankDetails(BankId, User_Id);
                if (errcode == BankId)
                {
                    return RedirectToAction("BankListDetails");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        [HttpGet]

        public ActionResult EditAddressDetails(int AddressId, int country, int state, int BankId)
        {
            try
            {
                BankDetailModel objmodel = new BankDetailModel();
                BankBL objbl = new BankBL();
                objmodel = objbl.GetSelectedAddressDetails(AddressId);

                objmodel.countryLst = objbl.GetCountryList();
                ViewBag.CountryList = new SelectList(objmodel.countryLst, "CountryId", "CountryName");

                objmodel.stateLst = objbl.GetStateList(country);
                ViewBag.StateListttttt = new SelectList(objmodel.stateLst, "StateId", "StateName");

                objmodel.cityLst = objbl.GetCityList(state);
                ViewBag.Citylistttttt = new SelectList(objmodel.cityLst, "CityId", "CityName");
                objmodel.BankId = BankId;
                return PartialView(objmodel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetStateList(int Country)
        {
            BankDetailModel addressModel = new BankDetailModel();
            addressModel.stateLst = new List<BankStateModel>();
            try
            {
                BankBL objbl = new BankBL();
                addressModel.Countryid = Country;
                addressModel.stateLst = objbl.GetStateList(Country);
                ViewBag.List = new SelectList(addressModel.stateLst, "StateId", "StateName");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(new SelectList(addressModel.stateLst, "StateId", "StateName"));
        }
        public JsonResult GetCityList(int State)
        {
            BankDetailModel addressModel = new BankDetailModel();
            try
            {
                BankBL objbl = new BankBL();
                addressModel.StateId = State;
                addressModel.cityLst = objbl.GetCityList(State);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(addressModel.cityLst, "CityId", "CityName"));
        }
        [HttpPost]
        public ActionResult EditAddressDetails(FormCollection fc)
        {
            try
            {
                int errcode = 0;
                BankDetailModel objmodel = new BankDetailModel();
                BankBL objbl = new BankBL();
                objmodel.AddressId = Convert.ToInt32(fc["AddressId"].ToString());
                objmodel.BankId = Convert.ToInt32(fc["BankId"].ToString());
                objmodel.Line1 = fc["Line1"].ToString();
                objmodel.Line2 = fc["Line2"].ToString();
                objmodel.StateId = Convert.ToInt32(fc["StateId"].ToString());
                objmodel.CityId = Convert.ToInt32(fc["CityId"].ToString());
                objmodel.Countryid = Convert.ToInt32(fc["CountryId"].ToString());
                objmodel.Pin = fc["Pin"].ToString();
                objmodel.LandMark = fc["LandMark"].ToString();
                errcode = objbl.SaveEditedAddressDetails(objmodel, objmodel.BankId);
                if (errcode == 2 || errcode == 500002)
                {
                    return RedirectToAction("EditBankDetails", new { BankId = errcode });
                }
                else
                    return RedirectToAction("EditBankDetails", new { BankId = errcode });

            }
            catch (Exception ex)
            {
                throw ex;
            }

            // return View();

        }
        #region CompanyBankDetail
        public ActionResult CompanyBankList()
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
                FindFlag = modelObj.FindMenu(lstTaskmodel, "/Bank/CompanyBankList");
                FindFlag = true;
                if (FindFlag)
                {
                    List<CompanyBankModel> LstCompBank = new List<CompanyBankModel>();
                    BankBL obj = new BankBL();
                    LstCompBank = obj.CompanyBankList();
                    return View(LstCompBank);
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
        public ActionResult CreateCompanyBankDetail(int CompBankAcId)
        {
            BankBL bl = new BankBL();
            string User_Id = User.Identity.GetUserId();
            CompanyBankModel model = new CompanyBankModel();
            ItemBL BlObj = new ItemBL();
            ItemMappingModel ItemMap = new ItemMappingModel();
            ItemMap.lstItemMap = BlObj.GetDWCompList();
            ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");

            BankBL obj = new BankBL();
            BankDetailModel objmodel = new BankDetailModel();
            objmodel.BankDetailList = obj.bankDetailList(User_Id);
            ViewBag.BankLst = new SelectList(objmodel.BankDetailList, "BankId", "BankName");
            if (CompBankAcId > 0)
            {
                model = bl.GetSelectedCompanyBankDetails(CompBankAcId);
            }
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult CreateCompanyBankDetail(FormCollection fc, bool IsActive)
        {
            CompanyBankModel model = new CompanyBankModel();
            int errorcode = 0;
            try
            {
                BankBL bl = new BankBL();
                string User_Id = User.Identity.GetUserId();
                model.CompBankAcId = Convert.ToInt32(fc["CompBankAcId"]);
                model.BankId = Convert.ToInt32(fc["BankId"]);
                model.CompCode = Convert.ToString(fc["CompCode"]);
                model.isActive = IsActive;
                model.Remark = Convert.ToString(fc["Remark"]);
                model.AccountNumber = Convert.ToString(fc["AccountNumber"]);
                errorcode = bl.SaveCompanyBankDetails(model, User_Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("CompanyBankList");


        }
        #endregion CompanyBankDetail

    }
}