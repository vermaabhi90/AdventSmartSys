using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.Adhoc;
using SmartSys.BL.RDS;
using SmartSys.BL.RGS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{
    public class RDSReportController : Controller
    {
        [Authorize]
        //[InitializeSimpleMembership]

        #region for List of RDSReport
        public ActionResult RDSRepotList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSReport/RDSRepotList");
            if (FindFlag)
            {
                List<RDSReportModel> RDSReportLst = new List<RDSReportModel>();

                RDSReportBL ObjBL = new RDSReportBL();
                RDSReportLst = ObjBL.GetList();


                if (Session["RDSReportModel"] != null)
                {
                    Session["RDSReportModel"] = null;
                }
                return View(RDSReportLst);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        #endregion for List of RDSReport

        public ActionResult Edit(String ReportId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSReport/RDSRepotList");
            if (FindFlag)
            {
                RDSReportModel rdsReportModel = null;
                if (Session["RDSReportModel"] == null)
                {
                    rdsReportModel = new RDSReportModel();
                    RDSReportBL ObjBL = new RDSReportBL();
                    rdsReportModel = ObjBL.GetSelectedListReport(ReportId);

                    Session["RDSReportModel"] = rdsReportModel;
                }
                else
                {
                    rdsReportModel = Session["RDSReportModel"] as RDSReportModel;
                }

                return View(rdsReportModel);
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
            RDSReportModel rdsReportModel = null;
            RDSReportDep ObjEvent = new RDSReportDep();
            RDSReportBL objBL = new RDSReportBL();
            if (Session["RDSReportModel"] != null)
            {
                rdsReportModel = Session["RDSReportModel"] as RDSReportModel;

            }
            rdsReportModel.ReportName = fc["ReportName"].ToString();

            ObjEvent.CreatedBy = 1;// WebMatrix.WebData.WebSecurity.CurrentUserId;

            int result = objBL.SaveEdit(rdsReportModel, User_Id);
            if (result == 4)
            {

                Session["RDSReportModel"] = null;
                Session["RDSReportList"] = null;
                return RedirectToAction("RDSRepotList", "RDSReport");
            }
            else
                TempData["Msg"] = " Occer some Error Please Try Again ...";
            return RedirectToAction("Edit", "RDSReport");

        }
        [HttpGet]
        public ActionResult AddEvent()
        {
            RDSReportModel rdsReportModel = null;
            List<RDSReportEvent> lstEvent = new List<RDSReportEvent>();

            if (Session["RDSReportModel"] != null)
            {
                rdsReportModel = Session["RDSReportModel"] as RDSReportModel;

            }
            RDSReportBL ObjBL = new RDSReportBL();
            lstEvent = ObjBL.GetEventList();

            foreach (RDSReportDep rrv in rdsReportModel.lstReportDep)
            {
                Int16 EventId = rrv.EventId;
                var actionToDelete = from actionRepDel in lstEvent
                                     where actionRepDel.EventId == EventId
                                     select actionRepDel;
                lstEvent.Remove(actionToDelete.ElementAt(0));
            }
            Session["RDSReportList"] = lstEvent;

            return PartialView(lstEvent);
        }



        public ActionResult AddEventToEdit(Int16 EventId, string EventName)
        {

            Session["Consul"] = EventId;
            RDSReportModel rdsReportModel = null;

            if (Session["RDSReportModel"] != null)
            {
                rdsReportModel = Session["RDSReportModel"] as RDSReportModel;
            }
            RDSReportDep rdsEvent = new RDSReportDep();
            rdsEvent.EventId = EventId;
            rdsEvent.EventName = EventName;
            rdsReportModel.lstReportDep.Add(rdsEvent);
            Session["RDSReportModel"] = rdsReportModel;
            return RedirectToAction("Edit", new { ReportId = rdsReportModel.ReportId });

        }

        public ActionResult DeleteEventAction(Int16 EventId)
        {
            RDSReportModel rdsReportModel = null;
            if (Session["RDSReportModel"] != null)
            {
                rdsReportModel = Session["RDSReportModel"] as RDSReportModel;

            }
            var actionToDelete = from actionRepDel in rdsReportModel.lstReportDep
                                 where actionRepDel.EventId == EventId
                                 select actionRepDel;
            rdsReportModel.lstReportDep.Remove(actionToDelete.ElementAt(0));

            Session["RDSReportModel"] = rdsReportModel;

            return RedirectToAction("Edit", new { ReportId = rdsReportModel.ReportId });

        }
        public ActionResult AddTrigger()
        {
            RDSReportModel rdsReportModel = null;
            List<RDSReportTrigger> lstTrigger = new List<RDSReportTrigger>();

            if (Session["RDSReportModel"] != null)
            {
                rdsReportModel = Session["RDSReportModel"] as RDSReportModel;

            }
            RDSReportBL ObjBL = new RDSReportBL();
            lstTrigger = ObjBL.GetTriggertList();
            foreach (RDSTriggerDep rrv in rdsReportModel.lstTriggerDep)
            {
                Int16 TriggerId = rrv.TriggerId;
                var actionToDelete = from actionRepDel in lstTrigger
                                     where actionRepDel.TriggerId == TriggerId
                                     select actionRepDel;
                lstTrigger.Remove(actionToDelete.ElementAt(0));


            }
            Session["RDSReportList"] = lstTrigger;

            return PartialView(lstTrigger);
        }
        public ActionResult AddTriggerToEdit(Int16 TriggerId, string TriggerName)
        {
            Session["Consul"] = TriggerId;
            RDSReportModel rdsReportModel = null;

            if (Session["RDSReportModel"] != null)
            {
                rdsReportModel = Session["RDSReportModel"] as RDSReportModel;
            }
            RDSTriggerDep rdsTrigger = new RDSTriggerDep();
            rdsTrigger.TriggerId = TriggerId;
            rdsTrigger.TriggerName = TriggerName;
            rdsReportModel.lstTriggerDep.Add(rdsTrigger);
            Session["RDSReportModel"] = rdsReportModel;
            return RedirectToAction("Edit", new { ReportId = rdsReportModel.ReportId });
        }
        public ActionResult DeleteTriggerAction(Int16 TriggerId)
        {
            RDSReportModel rdsReportModel = null;
            if (Session["RDSReportModel"] != null)
            {
                rdsReportModel = Session["RDSReportModel"] as RDSReportModel;

            }
            var actionToDelete = from actionRepDel in rdsReportModel.lstTriggerDep
                                 where actionRepDel.TriggerId == TriggerId
                                 select actionRepDel;
            rdsReportModel.lstTriggerDep.Remove(actionToDelete.ElementAt(0));

            Session["RDSReportModel"] = rdsReportModel;

            return RedirectToAction("Edit", new { ReportId = rdsReportModel.ReportId });
        }

        #region Transfer RGS Report  RDS
        public ActionResult ReportList()
        {
            List<RDSReportModel> RDSReportLst = new List<RDSReportModel>();
            try
            {
                RDSReportBL ObjBL = new RDSReportBL();
                RDSReportLst = ObjBL.GetList();
                Session["RGSlist"] = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(RDSReportLst);
        }
        public ActionResult RDSCreate()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/RDSReport/RDSCreate");
            if (FindFlag)
            {
                List<BusinessLine> businessLines = new List<BusinessLine>();
                AdminBL objAdmin = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                Report reportObj = new Report();
                DataSet ds = objAdmin.GetBusinessLineList();
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    BusinessLine businessLine = new BusinessLine();
                    businessLine.BusinessLineId = Convert.ToInt16(dr["BusinessLineId"].ToString());
                    businessLine.BusinessLineName = dr["BusinessLineName"].ToString();
                    businessLines.Add(businessLine);
                }
                ViewBag.ListOfBL = new SelectList(businessLines, "BusinessLineId", "BusinessLineName");

                RDSRptSubModel rdsRPTSubModel = new RDSRptSubModel();
                RDSRptSubBL objBL = new RDSRptSubBL();
                rdsRPTSubModel = objBL.GetReportClientListByUser(0);

                List<RGSReportNameModel> RGSReport = new List<RGSReportNameModel>();
                DataSet dsReport = new DataSet();
                AdhocBL objectBL = new AdhocBL();

                dsReport = objectBL.GetReportNameList(User_Id);
                Session["RGSlist"] = dsReport;
                if (dsReport != null)
                {
                    if (dsReport.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsReport.Tables[0].Rows)
                        {
                            RGSReportNameModel lstbl = new RGSReportNameModel();
                            lstbl.ReportId = dr["ReportId"].ToString();
                            lstbl.ReportName = dr["ReportName"].ToString();
                            RGSReport.Add(lstbl);
                        }
                    }
                }
                foreach (RDSReport demo in rdsRPTSubModel.RDSReportList)
                {
                    var actionToDelete = from selRGS in RGSReport
                                         where selRGS.ReportId.ToUpper() == demo.ReportId.ToUpper()
                                         select selRGS;
                    RGSReport.Remove(actionToDelete.ElementAt(0));
                }
                ViewBag.rdsrpt = new SelectList(RGSReport, "ReportId", "ReportName");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult RDSCreate(FormCollection formcollection)
        {
            try
            {
                if (Session["RGSlist"] != null)
                {
                    DataSet dsReport = new DataSet();
                    dsReport = (DataSet)Session["RGSlist"];
                    GeneratorBL objbl = new GeneratorBL();
                    string User_Id = User.Identity.GetUserId();
                    Report objmodel = new Report();
                    objmodel.ReportId = formcollection["ReportId"].ToString();

                    if (formcollection["ReportName"].ToString() == "")
                    {
                        foreach (DataRow dr in dsReport.Tables[0].Rows)
                        {
                            if (objmodel.ReportId == dr["ReportId"].ToString())
                            {
                                string reportname = dr["ReportName"].ToString();
                                string type = dr["ReportType"].ToString();
                                objmodel.ReportType = type;
                                objmodel.ReportName = reportname;
                                break;
                            }
                        }

                    }
                    else
                    {
                        foreach (DataRow dr in dsReport.Tables[0].Rows)
                        {
                            if (objmodel.ReportId == dr["ReportId"].ToString())
                            {
                                string type = dr["ReportType"].ToString();
                                objmodel.ReportType = type;
                                break;
                            }
                        }

                        objmodel.ReportName = formcollection["ReportName"].ToString();
                    }
                    objmodel.BusinessLineId = Convert.ToInt32(formcollection["BusinessLineName"].ToString());

                    int ErrCode = objbl.SaveRDSRpt(objmodel, User_Id);
                    if (ErrCode == 500002)
                    {
                        return RedirectToAction("RDSRepotList");
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View();
        }
        public ActionResult RDSEditReport(string ReportId)
        {
            GeneratorBL objbl = new GeneratorBL();
            Report reportObj = new Report();
            reportObj = objbl.GetSelectedRDSList(ReportId);
            string User_Id = User.Identity.GetUserId();

            List<BusinessLine> businessLines = new List<BusinessLine>();
            AdminBL objAdmin = new AdminBL();

            DataSet ds = objAdmin.GetBusinessLineList();
            foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
            {
                BusinessLine businessLine = new BusinessLine();
                businessLine.BusinessLineId = Convert.ToInt16(dr["BusinessLineId"].ToString());
                businessLine.BusinessLineName = dr["BusinessLineName"].ToString();
                businessLines.Add(businessLine);
            }
            ViewBag.ListOfBL = new SelectList(businessLines, "BusinessLineId", "BusinessLineName");

            return View(reportObj);
        }
        [HttpPost]
        public ActionResult RDSEditReport(FormCollection formcollection)
        {
            try
            {
                GeneratorBL objbl = new GeneratorBL();
                Report objmodel = new Report();
                string User_Id = User.Identity.GetUserId();
                objmodel.ReportId = formcollection["ReportId"].ToString();
                objmodel.ReportName = formcollection["ReportName"].ToString();
                objmodel.ReportType = formcollection["ReportType"].ToString();
                objmodel.BusinessLineId = Convert.ToInt32(formcollection["BusinessLineId"].ToString());
                objmodel.ModifiedBy = "1";
                int ErrCode = objbl.SaveRDSRpt(objmodel, User_Id);
                if (ErrCode == 500001)
                {
                    return RedirectToAction("RDSRepotList");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View();
        }
        #endregion Transfer RGS Report  RDS
    }
}