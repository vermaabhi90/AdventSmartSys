using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.RDS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{
    public class RDSEventController : Controller
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSEvent/Create");
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
            RDSEventBL objBL = new RDSEventBL();
            int errorcode = 0;
            string User_Id = User.Identity.GetUserId();
            DataSet dsEvent = new DataSet();

            dsEvent = objBL.GetSelectedEvent(0);
            if (dsEvent != null)
            {
                if (dsEvent.Tables.Count > 0)
                {
                    dsEvent.Tables[0].Rows.Clear();

                    DataRow dr = dsEvent.Tables[0].NewRow();

                    dr["EventId"] = Convert.ToInt16(0);
                    dr["EventName"] = fc["EventName"].ToString();
                    dr["Description"] = fc["Description"].ToString();
                    //    dr["CreatedBy"] = Convert.ToInt16(fc["CreatedBy"].ToString());                    

                    dsEvent.Tables[0].Rows.Add(dr);

                    errorcode = objBL.SaveEvent(dsEvent, User_Id);
                    if (errorcode == 500001 || errorcode == 500002)
                    {
                        return RedirectToAction("GetList");
                    }

                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(FormCollection fc)
        {
            DataSet dsEvent;
            RDSEventBL objBL = new RDSEventBL();
            int errorcode = 0;
            string User_Id = User.Identity.GetUserId();
            if (Session["DHEvent"] != null)
            {
                dsEvent = (DataSet)Session["DHEvent"];
                dsEvent.Tables[0].Rows[0]["EventName"] = fc["EventName"].ToString();
                dsEvent.Tables[0].Rows[0]["Description"] = fc["Description"].ToString();
                dsEvent.Tables[0].Rows[0]["ModifiedBy"] = 1;

                errorcode = objBL.SaveEvent(dsEvent, User_Id);
                if (errorcode == 500001 || errorcode == 500002)
                {
                    return RedirectToAction("GetList");
                }
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSEvent/GetList");
            if (FindFlag)
            {
                DataSet dsEvent = new DataSet();
                RDSEventBL objBL = new RDSEventBL();
                dsEvent = objBL.GetSelectedEvent(EventId);
                RDSEventModel dhEvent = new RDSEventModel();
                Session["DHEvent"] = dsEvent;
                if (dsEvent != null)
                {
                    if (dsEvent.Tables.Count > 0)
                    {
                        if (dsEvent.Tables[0].Rows.Count > 0)
                        {
                            dhEvent.EventId = EventId;
                            dhEvent.EventName = dsEvent.Tables[0].Rows[0]["EventName"].ToString();
                            dhEvent.Description = dsEvent.Tables[0].Rows[0]["Description"].ToString();
                            dhEvent.CreatedByName = dsEvent.Tables[0].Rows[0]["CreatedBy"].ToString();
                            dhEvent.ModifiedByName = dsEvent.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            dhEvent.CreatedDate = Convert.ToDateTime(dsEvent.Tables[0].Rows[0]["CreatedDate"].ToString());
                            dhEvent.ModifiedDate = Convert.ToDateTime(dsEvent.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        }
                    }
                }
                return View(dhEvent);
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSEvent/GetList");
            if (FindFlag)
            {
                List<RDSEventModel> lstRDSEvent = new List<RDSEventModel>();
                RDSEventBL objBL = new RDSEventBL();
                lstRDSEvent = objBL.GetRDSEventList();
                return View(lstRDSEvent);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}