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
    public class RGSGenController : Controller
    {

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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RGSGen/Create");
            if (FindFlag)
            {
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

            string User_Id = User.Identity.GetUserId();
            RGSGenModel rgsGenModel = new RGSGenModel();
            GeneratorBL objBL = new GeneratorBL();
            int errorcode = 0;

            rgsGenModel.GenId = Convert.ToInt16(0);
            rgsGenModel.Host = fc["Host"].ToString();
            rgsGenModel.GenType = fc["GenType"].ToString();
            rgsGenModel.Port = fc["Port"].ToString();
            rgsGenModel.isActive = isActive;
            rgsGenModel.BusinessLineId = Convert.ToInt16(fc["BusinessLineId"].ToString());
            rgsGenModel.PollingInterval = Convert.ToInt16(fc["PollingInterval"].ToString());
            rgsGenModel.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);


            errorcode = objBL.SaveGenrator(rgsGenModel, User_Id);
            if (errorcode == 500001 || errorcode == 500002)
            {
                TempData["Msg"] = "Successfully Create...";
                return RedirectToAction("Create");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(FormCollection fc, Boolean isActive)
        {
            RGSGenModel rgsGenModel = new RGSGenModel();
            GeneratorBL objBL = new GeneratorBL();
            int errorcode = 0;
            string User_Id = User.Identity.GetUserId();
            rgsGenModel.GenId = Convert.ToInt16(fc["GenId"].ToString());
            rgsGenModel.Host = fc["Host"].ToString();
            rgsGenModel.GenType = fc["GenType"].ToString();
            rgsGenModel.Port = fc["Port"].ToString();
            rgsGenModel.isActive = isActive;
            rgsGenModel.BusinessLineId = Convert.ToInt16(fc["BusinessLineId"].ToString());
            rgsGenModel.PollingInterval = Convert.ToInt16(fc["PollingInterval"].ToString());
            rgsGenModel.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);
            errorcode = objBL.SaveGenrator(rgsGenModel, User_Id);
            if (errorcode == 500001 || errorcode == 500002)
            {

                TempData["Msg"] = "Successfully Update...";
                return RedirectToAction("Edit", new { GenId = rgsGenModel.GenId });
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(Int16 GenId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RGSGen/GetList");
            if (FindFlag)
            {
                try
                {
                    RGSGenModel rgsGenModel = new RGSGenModel();
                    rgsGenModel.bussList = new List<BusinessLine>();
                    GeneratorBL objBL1 = new GeneratorBL();
                    rgsGenModel = objBL1.GetSelectedGenList(GenId);

                    rgsGenModel.bussList = objBL1.GetBussList();
                    ViewBag.BussList = new SelectList(rgsGenModel.bussList, "BusinessLineId", "BusinessLineName");
                    return View(rgsGenModel);
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

        [HttpGet]
        public ActionResult Status(Int16 GenId, Int16 Status)
        {
            GeneratorBL StatusDemo = new GeneratorBL();
            Int16 StstusDemo = Status;
            int errorCode = StatusDemo.SetGeneratorStatus(GenId, StstusDemo);
            if (errorCode == 500001)
            {
                TempData["Msg"] = "Status Successfully Update...";
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RGSGen/GetList");
            if (FindFlag)
            {
                GeneratorBL smartsysBlObj = new GeneratorBL();
                List<RGSGenModel> lstInstMgrList = smartsysBlObj.RGSGenGetList();
                return View(lstInstMgrList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}