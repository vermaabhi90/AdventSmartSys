using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.Sysadmin;
using Syncfusion.EJ.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Syncfusion.JavaScript.Models;
using Syncfusion.XlsIO;
using System.Web.Script.Serialization;
using System.Collections;
using System.Reflection;

namespace SmartSys.Controllers
{
    public class BusinessLineController : Controller
    {
        [Authorize]
        // GET: BusinessLine

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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BusinessLine/Create");
            if (FindFlag)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult Create(FormCollection fc)
        {
            BusinessLineBL objBL = new BusinessLineBL();
            string User_Id = User.Identity.GetUserId();
            int errorcode = 0;
            BusinessLineModel BussLineModel = new BusinessLineModel();
            BussLineModel.BusinessLineId = Convert.ToInt16(0);
            BussLineModel.BusinessLineName = fc["BusinessLineName"].ToString();
            BussLineModel.Description = fc["Description"].ToString();
            BussLineModel.ModifiedBy = Convert.ToInt16(1);
            errorcode = objBL.SaveBussLine(BussLineModel, User_Id);
            if (errorcode == 500001 || errorcode == 500002)
            {
                TempData["Msg"] = "Successfully Create...";
                return RedirectToAction("GetList");
            }
            return View();
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BusinessLine/GetList");
            if (FindFlag)
            {
                BusinessLineBL BlObj = new BusinessLineBL();
                List<BusinessLineModel> lstBussLine = BlObj.GetBussLineList(UserId);
                return View(lstBussLine);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [System.Web.Http.ActionName("ExportBussinessLineLst")]
        [AcceptVerbs("POST")]
        public void ExportBussinessLineLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();           
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            BusinessLineBL BlObj = new BusinessLineBL();            
            var DataSource = BlObj.GetBussLineList(User_Id);
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
        [HttpPost]
        public ActionResult Edit(FormCollection fc)
        {

            BusinessLineModel BussLineModel = new BusinessLineModel();
            string User_Id = User.Identity.GetUserId();
            BusinessLineBL objBL = new BusinessLineBL();
            int errorcode = 0;
            BussLineModel.BusinessLineId = Convert.ToInt16(fc["BusinessLineId"].ToString());
            BussLineModel.BusinessLineName = fc["BusinessLineName"].ToString();
            BussLineModel.Description = fc["Description"].ToString();
            BussLineModel.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);
            errorcode = objBL.SaveBussLine(BussLineModel, User_Id);
            if (errorcode == 500001 || errorcode == 500002)
            {
                TempData["Msg"] = "Successfully Update...";
                return RedirectToAction("GetList");
            }

            return View();
        }
        [HttpGet]
        public ActionResult Edit(Int16 BusinessLineId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/BusinessLine/GetList");
            if (FindFlag)
            {
                try
                {
                    BusinessLineModel BussLineModel = new BusinessLineModel();
                    BusinessLineBL objBL = new BusinessLineBL();
                    BussLineModel = objBL.BussLineGetSelected(BusinessLineId);
                    return View(BussLineModel);
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
    }
}