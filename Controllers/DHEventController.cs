using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.DataHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{
    public class DHEventController : Controller
    {
        // GET: DHEvent
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DHEvent/Create");
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
        public ActionResult Create(FormCollection fc, Boolean isDeleted)
        {
            DHEvent dhEventModel = new DHEvent();
            DHEventBL objBL = new DHEventBL();
            int errorcode = 0;
            dhEventModel.EventId = Convert.ToInt16(0);
            dhEventModel.EventName = fc["EventName"].ToString();
            dhEventModel.Description = fc["Description"].ToString();
            dhEventModel.isDeleted = isDeleted;
            errorcode = objBL.SaveDHEvent(dhEventModel);
            if (errorcode == 500001 || errorcode == 500002)
            {
                return RedirectToAction("GetList");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(FormCollection fc, Boolean isDeleted)
        {
            DHEvent dhEventModel = new DHEvent();
            DHEventBL objBL = new DHEventBL();
            int errorcode = 0;
            dhEventModel.EventId = Convert.ToInt16(fc["EventId"].ToString());
            dhEventModel.EventName = fc["EventName"].ToString();
            dhEventModel.Description = fc["Description"].ToString();
            dhEventModel.isDeleted = isDeleted;
            errorcode = objBL.SaveDHEvent(dhEventModel);
            if (errorcode == 500001 || errorcode == 500002)
            {
                return RedirectToAction("GetList");
            }

            return View();
        }

        public ActionResult Edit(Int16 EventId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DHEvent/GetList");
            if (FindFlag)
            {
                try
                {
                    DHEvent dhEventModel = new DHEvent();
                    DHEventBL objBL = new DHEventBL();
                    dhEventModel = objBL.DHEventGetSelected(EventId);
                    return View(dhEventModel);
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DHEvent/GetList");
            if (FindFlag)
            {
                try
                {
                    List<DHEvent> dhEventList = new List<DHEvent>();
                    DHEventBL objEvent = new DHEventBL();
                    dhEventList = objEvent.GetDHEventList();
                    return View(dhEventList);
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