using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.RGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{
    public class RGSInsMgrController : Controller
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RGSInsMgr/GetList");
            if (FindFlag)
            {
                InstanceManagerBL BlObj = new InstanceManagerBL();
                List<InstanceManagerModel> lstInstMgrList = BlObj.GetInstMgrList();
                return View(lstInstMgrList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public ActionResult Edit(Int16 IMId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RGSInsMgr/GetList");
            if (FindFlag)
            {
                InstanceManagerModel InstMgrModel = new InstanceManagerModel();
                InstanceManagerBL objBL = new InstanceManagerBL();
                InstMgrModel = objBL.InstMgrGetSelected(IMId);
                return View(InstMgrModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Edit(FormCollection fc, Boolean isPrimary)
        {
            InstanceManagerBL objBL = new InstanceManagerBL();
            int errorcode = 0;
            string User_Id = User.Identity.GetUserId();
            InstanceManagerModel InstMgrModel = new InstanceManagerModel();
            InstMgrModel.IMId = Convert.ToInt16(fc["IMId"].ToString());
            InstMgrModel.Host = fc["Host"].ToString();
            InstMgrModel.Port = fc["Port"].ToString();
            InstMgrModel.isPrimary = isPrimary;
            InstMgrModel.PollingInterval = Convert.ToInt32(fc["PollingInterval"].ToString());
            InstMgrModel.ProcessId = 0;
            InstMgrModel.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);
            errorcode = objBL.SaveInstMgr(InstMgrModel, User_Id);
            if (errorcode == 500003)
            {
                ViewBag.Msg = "This Host already run in server you can not change...";
                return RedirectToAction("Edit", new { IMId = InstMgrModel.IMId });
            }
            else if (errorcode == 500001 || errorcode == 500002)
            {
                TempData["Message"] = "Successfully Update...";
                return RedirectToAction("Edit", new { IMId = InstMgrModel.IMId });
            }


            return View();
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RGSInsMgr/Create");
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
        public ActionResult Create(FormCollection fc, Boolean isPrimary)
        {
            int errorcode = 0;
            string User_Id = User.Identity.GetUserId();
            InstanceManagerBL objBL = new InstanceManagerBL();
            InstanceManagerModel InstMgrModel = new InstanceManagerModel();
            InstMgrModel.IMId = Convert.ToInt16(0);
            InstMgrModel.Host = fc["Host"].ToString();
            InstMgrModel.Port = fc["Port"].ToString();
            InstMgrModel.isPrimary = isPrimary;
            InstMgrModel.PollingInterval = Convert.ToInt32(fc["PollingInterval"].ToString());
            InstMgrModel.ProcessId = 0;
            InstMgrModel.ModifiedBy = 1;// Convert.ToInt16(WebMatrix.WebData.WebSecurity.CurrentUserId);

            errorcode = objBL.SaveInstMgr(InstMgrModel, User_Id);
            if (errorcode == 500003)
            {
                ViewBag.Msg = "This Host already run in server please select Another Host Name";
                return RedirectToAction("Create");
            }
            else if (errorcode == 500001 || errorcode == 500002)
            {
                ViewBag.Msg = "Successfully Create...";
                return RedirectToAction("Create");
            }
            return View();
        }

    }
}
