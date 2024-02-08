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
    public class RDSClientController : Controller
    {
        public ActionResult ClientList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSClient/ClientList");
            if (FindFlag)
            {
                List<RDSClientModel> rdsClientList = new List<RDSClientModel>();
                RDSClientBL BLObj = new RDSClientBL();
                rdsClientList = BLObj.RDSGetClientList();
                return View(rdsClientList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public ActionResult Edit(int ClientId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSClient/ClientList");
            if (FindFlag)
            {
                RDSClientModel rdsClientList = new RDSClientModel();
                RDSClientBL BLObj = new RDSClientBL();
                rdsClientList = BLObj.RDSGetSelectedClient(ClientId);
                return View(rdsClientList);
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
            RDSClientModel rdsClientList = new RDSClientModel();
            RDSClientBL BLObj = new RDSClientBL();
            int errorcode = 0;
            rdsClientList.FTPDetails = fc["Server"].ToString() + "," + fc["UserName"].ToString() + "," + fc["Password"].ToString();
            rdsClientList.ClientId = Convert.ToInt32(fc["ClientId"].ToString());
            rdsClientList.ClientName = fc["ClientName"].ToString();
            rdsClientList.ClientType = fc["ClientType"].ToString();
            rdsClientList.ClientRefId = fc["ClientRefId"].ToString();
            rdsClientList.email = fc["email"].ToString();
            rdsClientList.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);
            errorcode = BLObj.SaveRDSClient(rdsClientList, User_Id);
            if (errorcode == 500001 || errorcode == 500002)
            {

                TempData["Msg"] = "Successfully Update...";
                return RedirectToAction("Edit", new { ClientId = rdsClientList.ClientId });
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSClient/Create");
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
            RDSClientModel rdsClientList = new RDSClientModel();
            RDSClientBL BLObj = new RDSClientBL();
            int errorcode = 0;
            rdsClientList.FTPDetails = fc["Server"].ToString() + "," + fc["UserName"].ToString() + "," + fc["Password"].ToString();
            rdsClientList.ClientId = Convert.ToInt32(0);
            rdsClientList.ClientName = fc["ClientName"].ToString();
            rdsClientList.ClientType = fc["ClientType"].ToString();
            rdsClientList.ClientRefId = fc["ClientRefId"].ToString();
            rdsClientList.email = fc["email"].ToString();
            rdsClientList.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);
            errorcode = BLObj.SaveRDSClient(rdsClientList, User_Id);
            if (errorcode == 500001 || errorcode == 500002)
            {

                TempData["Msg"] = "Successfully Update...";
                return RedirectToAction("ClientList");
            }
            return View();
        }
    }
}