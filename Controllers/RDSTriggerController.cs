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
    public class RDSTriggerController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRDSTriggerList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSTrigger/GetRDSTriggerList");
            if (FindFlag)
            {
                List<RDSTriggerModel> lstRDSTrigger = new List<RDSTriggerModel>();
                RDSTriggerBL objBL = new RDSTriggerBL();
                lstRDSTrigger = objBL.GetRDSTriggeList();
                return View(lstRDSTrigger);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TriggerId">  what it does </param>
        /// <returns></returns>
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSTrigger/GetRDSTriggerList");
            if (FindFlag)
            {
                RDSTriggerModel rdsTriggModel = new RDSTriggerModel();
                RDSTriggerBL objBL = new RDSTriggerBL();
                rdsTriggModel = objBL.RDSTriggerGetSelected(TriggerId);
                return View(rdsTriggModel);
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
            RDSTriggerModel rdsTriggModel = new RDSTriggerModel();
            RDSTriggerBL objBL = new RDSTriggerBL();
            int errorcode = 0;
            rdsTriggModel.TriggerId = Convert.ToInt16(fc["TriggerId"].ToString());
            rdsTriggModel.TriggerName = fc["TriggerName"].ToString();
            rdsTriggModel.Description = fc["Description"].ToString();
            rdsTriggModel.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);
            errorcode = objBL.SaveRDSTrigger(rdsTriggModel, User_Id);

            if (errorcode == 500001 || errorcode == 500002)
            {
                return RedirectToAction("GetRDSTriggerList");
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSTrigger/Create");
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
            string User_Id = User.Identity.GetUserId();
            RDSTriggerModel rdsTriggModel = new RDSTriggerModel();
            RDSTriggerBL objBL = new RDSTriggerBL();
            int errorcode = 0;

            rdsTriggModel.TriggerId = Convert.ToInt16(0);
            rdsTriggModel.TriggerName = fc["TriggerName"].ToString();
            rdsTriggModel.Description = fc["Description"].ToString();
            rdsTriggModel.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);
            errorcode = objBL.SaveRDSTrigger(rdsTriggModel, User_Id);
            if (errorcode == 500001 || errorcode == 500002)
            {
                TempData["Msg"] = "Successfully Create...";
                return RedirectToAction("Create");
            }
            return View();
        }
    }
}