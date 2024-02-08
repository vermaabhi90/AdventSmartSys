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
    public class DHTriggerController : Controller
    {

        public ActionResult GetDHTriggerList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DHTrigger/GetDHTriggerList");
            if (FindFlag)
            {
                List<DHTriggerModel> lstDHTrigger = new List<DHTriggerModel>();
                DHTriggerBL objBL = new DHTriggerBL();
                lstDHTrigger = objBL.GetDHTriggeList();
                return View(lstDHTrigger);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Edit(Int16 TriggerId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DHTrigger/GetDHTriggerList");
            if (FindFlag)
            {
                DHTriggerModel dhTriggModel = new DHTriggerModel();
                DHTriggerBL objBL = new DHTriggerBL();
                dhTriggModel = objBL.DHTriggerGetSelected(TriggerId);
                return View(dhTriggModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Edit(FormCollection fc)
        {
            string User_Id = User.Identity.GetUserId();
            DHTriggerModel dhTriggModel = new DHTriggerModel();
            DHTriggerBL objBL = new DHTriggerBL();
            int errorcode = 0;
            dhTriggModel.TriggerId = Convert.ToInt16(fc["TriggerId"].ToString());
            dhTriggModel.TriggerName = fc["TriggerName"].ToString();
            dhTriggModel.Description = fc["Description"].ToString();
            dhTriggModel.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);
            errorcode = objBL.SaveDHTrigger(dhTriggModel, User_Id);

            if (errorcode == 500001 || errorcode == 500002)
            {
                return RedirectToAction("GetDHTriggerList");
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DHTrigger/Create");
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
            DHTriggerModel dhTriggModel = new DHTriggerModel();
            string User_Id = User.Identity.GetUserId();
            DHTriggerBL objBL = new DHTriggerBL();
            int errorcode = 0;

            dhTriggModel.TriggerId = Convert.ToInt16(0);
            dhTriggModel.TriggerName = fc["TriggerName"].ToString();
            dhTriggModel.Description = fc["Description"].ToString();
            dhTriggModel.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);
            errorcode = objBL.SaveDHTrigger(dhTriggModel, User_Id);
            if (errorcode == 500001 || errorcode == 500002)
            {
                TempData["Msg"] = "Successfully Create...";
                return RedirectToAction("GetDHTriggerList");
            }
            return View();
        }
    }
}