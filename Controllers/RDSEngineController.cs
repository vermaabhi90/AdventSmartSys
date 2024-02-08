using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.RDS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{
    public class RDSEngineController : Controller
    {
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSEngine/GetList");
            if (FindFlag)
            {
                RDSEngineBL BlObj = new RDSEngineBL();
                List<RDSEngineModel> lstRDSEngine = new List<RDSEngineModel>();
                lstRDSEngine = BlObj.RDSGetEngineList();
                return View(lstRDSEngine);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSEngine/GetList");
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
        public ActionResult Create(FormCollection fc, Boolean isActive)
        {
            string User_Id = User.Identity.GetUserId();
            RDSEngineModel rdsEngineModel = new RDSEngineModel();
            RDSEngineBL BlObj = new RDSEngineBL();
            int errorcode = 0;

            rdsEngineModel.RDSEngineId = Convert.ToInt16(0);
            rdsEngineModel.Port = fc["Port"].ToString();
            rdsEngineModel.Host = fc["Host"].ToString();
            rdsEngineModel.JobPollingInterval = Convert.ToInt16(fc["JobPollingInterval"].ToString());
            rdsEngineModel.EventPollingIntterval = Convert.ToInt16(fc["EventPollingIntterval"].ToString());
            rdsEngineModel.TriggerPollingInterval = Convert.ToInt16(fc["TriggerPollingInterval"].ToString());
            rdsEngineModel.ProcessPollingInterval = Convert.ToInt16(fc["ProcessPollingInterval"].ToString());
            rdsEngineModel.StatusId = Convert.ToInt16(SmartSys.Utility.Enums.StatusCode.Idle);
            rdsEngineModel.ProcessId = Convert.ToInt32(1);
            rdsEngineModel.isActive = isActive;
            rdsEngineModel.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);

            errorcode = BlObj.SaveRDSEngine(rdsEngineModel, User_Id);
            if (errorcode == 500001 || errorcode == 500002)
            {
                TempData["Msg"] = "Successfully Create...";
                return RedirectToAction("Create");
            }
            return View();

        }

        public ActionResult Edit(Int16 RDSEngineId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSEngine/GetList");
            if (FindFlag)
            {
                RDSEngineModel rdsEngineModel = new RDSEngineModel();
                RDSEngineBL BlObj = new RDSEngineBL();
                rdsEngineModel = BlObj.RDSGetSelectedEngine(RDSEngineId);
                return View(rdsEngineModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Edit(FormCollection fc, Boolean isActive)
        {
            string User_Id = User.Identity.GetUserId();
            RDSEngineModel rdsEngineModel = new RDSEngineModel();
            RDSEngineBL BlObj = new RDSEngineBL();
            int errorcode = 0;

            rdsEngineModel.RDSEngineId = Convert.ToInt16(fc["RDSEngineId"].ToString());
            rdsEngineModel.Port = fc["Port"].ToString();
            rdsEngineModel.Host = fc["Host"].ToString();
            rdsEngineModel.JobPollingInterval = Convert.ToInt16(fc["JobPollingInterval"].ToString());
            rdsEngineModel.EventPollingIntterval = Convert.ToInt16(fc["EventPollingIntterval"].ToString());
            rdsEngineModel.TriggerPollingInterval = Convert.ToInt16(fc["TriggerPollingInterval"].ToString());
            rdsEngineModel.ProcessPollingInterval = Convert.ToInt16(fc["ProcessPollingInterval"].ToString());
            rdsEngineModel.StatusId = Convert.ToInt16(SmartSys.Utility.Enums.StatusCode.Idle);
            rdsEngineModel.ProcessId = Convert.ToInt32(1);
            rdsEngineModel.isActive = isActive;
            rdsEngineModel.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);

            errorcode = BlObj.SaveRDSEngine(rdsEngineModel, User_Id);
            if (errorcode == 500001 || errorcode == 500002)
            {
                return RedirectToAction("GetList");
            }
            return View();
        }
    }
}