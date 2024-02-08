using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.SysDBCon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{
    public class SysDBConController : Controller
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/SysDBCon/Create");
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
            SysDBConModel ConnModel = new SysDBConModel();
            string User_Id = User.Identity.GetUserId();
            SysDBConBL objBL = new SysDBConBL();
            int errorcode = 0;
            ConnModel.ConnectionId = Convert.ToInt16(0);
            ConnModel.ConName = fc["ConName"].ToString();
            ConnModel.ServerName = fc["ServerName"].ToString();
            ConnModel.ConnectionType = fc["ConnectionType"].ToString();
            ConnModel.Port = fc["Port"].ToString();
            ConnModel.DBName = fc["DBName"].ToString();
            ConnModel.UserName = fc["UserName"].ToString();
            ConnModel.Password = fc["Txt_Pwd"].ToString();
            ConnModel.DefaultTimeOut = Convert.ToInt32(fc["DefaultTimeOut"].ToString());
            ConnModel.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);
            errorcode = objBL.SaveDBConn(ConnModel, User_Id);
            if (errorcode == 500001 || errorcode == 500002)
            {
                return RedirectToAction("GetList");
            }

            return View();
        }



        [HttpPost]
        public ActionResult Edit(FormCollection fc)
        {

            SysDBConModel ConnModel = new SysDBConModel();
            string User_Id = User.Identity.GetUserId();
            SysDBConBL objBL = new SysDBConBL();
            int errorcode = 0;
            ConnModel.ConnectionId = Convert.ToInt16(fc["ConnectionId"].ToString());
            ConnModel.ConName = fc["ConName"].ToString();
            ConnModel.ServerName = fc["ServerName"].ToString();
            ConnModel.ConnectionType = fc["ConnectionType"].ToString();
            ConnModel.Port = fc["Port"].ToString();
            ConnModel.DBName = fc["DBName"].ToString();
            ConnModel.UserName = fc["UserName"].ToString();
            ConnModel.Password = fc["Txt_Pwd"].ToString();
            ConnModel.DefaultTimeOut = Convert.ToInt32(fc["DefaultTimeOut"].ToString());
            ConnModel.ModifiedBy = Convert.ToInt16(1);//WebMatrix.WebData.WebSecurity.CurrentUserId);
            errorcode = objBL.SaveDBConn(ConnModel, User_Id);

            if (errorcode == 500001 || errorcode == 500002)
            {
                return RedirectToAction("GetList");
            }

            return View();
        }

        public ActionResult Edit(Int16 ConnectionId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/SysDBCon/GetList");
            if (FindFlag)
            {
                SysDBConModel ConnModel = new SysDBConModel();
                SysDBConBL objBL = new SysDBConBL();
                ConnModel = objBL.GetSelectedDBConn(ConnectionId);
                return View(ConnModel);
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/SysDBCon/GetList");
            if (FindFlag)
            {
                List<SysDBConModel> lstBDConnection = new List<SysDBConModel>();
                SysDBConBL objBL = new SysDBConBL();
                lstBDConnection = objBL.GetDBConnList(UserId);
                ViewBag.Datasource = lstBDConnection;
                List<ConType> lstConType = new List<ConType>();
                lstConType.Add(new ConType("MySql"));
                lstConType.Add(new ConType("Native"));
                lstConType.Add(new ConType("ODBC"));
                lstConType.Add(new ConType("OLEDB"));
                ViewBag.DataSourceConType = lstConType;
                return View(lstBDConnection);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

        public class ConType
        {
            public ConType(string ConType)
            {
                ConnectionType = ConType;
            }
            public string ConnectionType { get; set; }
        }
    }
}