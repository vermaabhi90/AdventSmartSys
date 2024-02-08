using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.DataHub;
using SmartSys.BL.RGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{
    public class DHLoaderController : Controller
    {
        // GET: DHLoader
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DHLoader/GetList");
            if (FindFlag)
            {
                DHLoaderBL BlObj = new DHLoaderBL();
                List<DHLoaderModel> lstDHLoader = new List<DHLoaderModel>();
                lstDHLoader = BlObj.DHLoaderGetList();
                return View(lstDHLoader);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Edit(Int16 LoaderId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DHLoader/GetList");
            if (FindFlag)
            {
                DHLoaderModel dhLoaderModel = new DHLoaderModel();
                DHLoaderBL BlObj = new DHLoaderBL();
                dhLoaderModel = BlObj.DHLoaderGetSelected(LoaderId);

                dhLoaderModel.lstLoader = new List<LoaderTypeModel>();
                dhLoaderModel.lstLoader = BlObj.DHLoaderTypeGetSelected();
                ViewBag.LoaderList = new SelectList(dhLoaderModel.lstLoader, "LoaderType", "LoaderValue", dhLoaderModel.LoaderType);


                RGSGenModel rgsGenModel = new RGSGenModel();
                rgsGenModel.bussList = new List<BusinessLine>();
                GeneratorBL objBL1 = new GeneratorBL();
                rgsGenModel.bussList = objBL1.GetBussList();
                ViewBag.BussList = new SelectList(rgsGenModel.bussList, "BusinessLineId", "BusinessLineName");

                return View(dhLoaderModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Edit(FormCollection fc, Boolean isActive)
        {
            DHLoaderModel dhLoaderModel = new DHLoaderModel();
            DHLoaderBL BlObj = new DHLoaderBL();
            int errorcode = 0;

            dhLoaderModel.LoaderId = Convert.ToInt16(fc["LoaderId"].ToString());
            dhLoaderModel.JobPollingInterval = Convert.ToInt16(fc["JobPollingInterval"].ToString());
            dhLoaderModel.EventPollingInterval = Convert.ToInt16(fc["EventPollingInterval"].ToString());
            dhLoaderModel.TriggerPollingInterval = Convert.ToInt16(fc["TriggerPollingInterval"].ToString());
            dhLoaderModel.StatusId = Convert.ToInt16(SmartSys.Utility.Enums.StatusCode.Idle);
            dhLoaderModel.Host = fc["Host"].ToString();
            dhLoaderModel.LoaderType = fc["LoaderType"].ToString();
            dhLoaderModel.Port = fc["Port"].ToString();
            dhLoaderModel.isActive = isActive;
            dhLoaderModel.BusinessLineId = Convert.ToInt16(fc["BusinessLineId"].ToString());
            errorcode = BlObj.SaveDHLoader(dhLoaderModel);
            if (errorcode == 500001 || errorcode == 500002)
            {
                return RedirectToAction("GetList");
            }
            return View();
        }
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DHLoader/Create");
            if (FindFlag)
            {
                List<LoaderTypeModel> lstLoader = new List<LoaderTypeModel>();
                DHLoaderBL BlObj = new DHLoaderBL();
                lstLoader = BlObj.DHLoaderTypeGetSelected();
                ViewBag.LoaderList = new SelectList(lstLoader, "LoaderType", "LoaderValue");

                RGSGenModel rgsGenModel = new RGSGenModel();
                GeneratorBL objBL = new GeneratorBL();
                rgsGenModel.bussList = objBL.GetBussList();
                ViewBag.BussList = new SelectList(rgsGenModel.bussList, "BusinessLineId", "BusinessLineName");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Create(FormCollection fc, Boolean isActive)
        {
            DHLoaderModel dhLoaderModel = new DHLoaderModel();
            DHLoaderBL BlObj = new DHLoaderBL();
            int errorcode = 0;

            dhLoaderModel.LoaderId = Convert.ToInt16(0);
            dhLoaderModel.JobPollingInterval = Convert.ToInt16(fc["JobPollingInterval"].ToString());
            dhLoaderModel.EventPollingInterval = Convert.ToInt16(fc["EventPollingInterval"].ToString());
            dhLoaderModel.TriggerPollingInterval = Convert.ToInt16(fc["TriggerPollingInterval"].ToString());
            dhLoaderModel.StatusId = Convert.ToInt16(SmartSys.Utility.Enums.StatusCode.Idle);
            dhLoaderModel.Host = fc["Host"].ToString();
            dhLoaderModel.LoaderType = fc["LoaderType"].ToString();
            dhLoaderModel.Port = fc["Port"].ToString();
            dhLoaderModel.isActive = isActive;
            dhLoaderModel.BusinessLineId = Convert.ToInt16(fc["BusinessLineId"].ToString());

            errorcode = BlObj.SaveDHLoader(dhLoaderModel);
            if (errorcode == 500001 || errorcode == 500002)
            {
                TempData["Msg"] = "Successfully Create...";
                return RedirectToAction("GetList");
            }
            return View();

        }
    }
}