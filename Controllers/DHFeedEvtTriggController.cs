using Microsoft.AspNet.Identity;
using SmartSys.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{
    [Authorize]
    public class DHFeedEvtTriggController : Controller
    {
        // GET: DHFeedEvtTrigg
        public ActionResult DHFeedList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DHFeedEvtTrigg/DHFeedList");
            if (FindFlag)
            {
                DHFeedBL dataHubBlObj = new DHFeedBL();
                List<DHFeedModel> lstFeed = dataHubBlObj.GetFeedList();

                if (Session["dhFeedEvtTriggModel"] != null)
                {
                    Session["dhFeedEvtTriggModel"] = null;
                }

                return View(lstFeed);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult Edit(int FeedId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DHFeedEvtTrigg/DHFeedList");
            if (FindFlag)
            {
                DHFeedEvtTriggModel dhFeedActionModel = null;

                if (Session["dhFeedEvtTriggModel"] == null)
                {
                    DHFeedBL dhFeedBlOBj = new DHFeedBL();

                    dhFeedActionModel = dhFeedBlOBj.GetDHFeedEvtTriggModelDetail(FeedId);

                    Session["dhFeedEvtTriggModel"] = dhFeedActionModel;
                }
                else
                {
                    dhFeedActionModel = Session["dhFeedEvtTriggModel"] as DHFeedEvtTriggModel;

                }


                return View(dhFeedActionModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult Edit()
        {
            DHFeedEvtTriggModel dhFeedActionModel = null;
            string User_Id = User.Identity.GetUserId();
            DHFeedBL dhFeedBlOBj = new DHFeedBL();
            if (Session["dhFeedEvtTriggModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedEvtTriggModel"] as DHFeedEvtTriggModel;

            }


            dhFeedActionModel.CreatedBy = 1;// WebMatrix.WebData.WebSecurity.CurrentUserId;

            int result = dhFeedBlOBj.SaveEdit(dhFeedActionModel, User_Id);
            if (result == 4)
            {

                Session["dhFeedEvtTriggModel"] = null;
                Session["dhFeedEvtTriggModel"] = null;
                return RedirectToAction("DHFeedList", "DHFeedEvtTrigg");
            }
            else
                TempData["Msg"] = " Occur some Error Please Try Again ...";
            return RedirectToAction("Edit", "DHFeedEvtTrigg", new { FeedId = dhFeedActionModel.FeedId });

        }

        #region for EventDep
        public ActionResult AddEvent()
        {
            DHFeedEvtTriggModel dhFeedEvtTriggModel = null;
            List<DHFeedEvent> lstEvent = new List<DHFeedEvent>();

            if (Session["dhFeedEvtTriggModel"] != null)
            {
                dhFeedEvtTriggModel = Session["dhFeedEvtTriggModel"] as DHFeedEvtTriggModel;


            }

            DHFeedBL ObjBL = new DHFeedBL();
            lstEvent = ObjBL.GetEventList();


            foreach (DHFeedEventDep rrv in dhFeedEvtTriggModel.lstDHFeedEventDep)
            {
                Int16 EventId = rrv.EventId;
                var actionToDelete = from actionRepDel in lstEvent
                                     where actionRepDel.EventId == EventId
                                     select actionRepDel;
                lstEvent.Remove(actionToDelete.ElementAt(0));
            }
            Session["dhFeedEvtTriggModel"] = dhFeedEvtTriggModel;
            return PartialView(lstEvent);
        }

        public ActionResult AddEventToEdit(Int16 EventId, string EventName)
        {

            Session["Consul"] = EventId;
            DHFeedEvtTriggModel dhFeedEvtTriggModel = null;

            if (Session["dhFeedEvtTriggModel"] != null)
            {
                dhFeedEvtTriggModel = Session["dhFeedEvtTriggModel"] as DHFeedEvtTriggModel;
            }
            DHFeedEventDep dhEvent = new DHFeedEventDep();
            dhEvent.EventId = EventId;
            dhEvent.EventName = EventName;
            dhFeedEvtTriggModel.lstDHFeedEventDep.Add(dhEvent);
            Session["dhFeedEvtTriggModel"] = dhFeedEvtTriggModel;
            return RedirectToAction("Edit", new { FeedId = dhFeedEvtTriggModel.FeedId });

        }

        public ActionResult DeleteEventAction(Int16 EventId)
        {
            DHFeedEvtTriggModel dhFeedEvtTriggModel = null;

            if (Session["dhFeedEvtTriggModel"] != null)
            {
                dhFeedEvtTriggModel = Session["dhFeedEvtTriggModel"] as DHFeedEvtTriggModel;
            }

            var actionToDelete = from actionRepDel in dhFeedEvtTriggModel.lstDHFeedEventDep
                                 where actionRepDel.EventId == EventId
                                 select actionRepDel;
            dhFeedEvtTriggModel.lstDHFeedEventDep.Remove(actionToDelete.ElementAt(0));

            Session["dhFeedEvtTriggModel"] = dhFeedEvtTriggModel;

            return RedirectToAction("Edit", new { FeedId = dhFeedEvtTriggModel.FeedId });

        }

        #endregion for EventDep

        #region for Trigger Dep
        public ActionResult AddTrigger()
        {
            DHFeedEvtTriggModel dhFeedEvtTriggModel = null;
            List<DHFeedTrigger> lstTrigger = new List<DHFeedTrigger>();

            if (Session["dhFeedEvtTriggModel"] != null)
            {
                dhFeedEvtTriggModel = Session["dhFeedEvtTriggModel"] as DHFeedEvtTriggModel;

            }
            DHFeedBL ObjBL = new DHFeedBL();
            lstTrigger = ObjBL.GetTriggertList();
            foreach (DHFeedTriggerDep rrv in dhFeedEvtTriggModel.lstDHFeedTriggerDep)
            {
                Int16 TriggerId = rrv.TriggerId;
                var actionToDelete = from actionRepDel in lstTrigger
                                     where actionRepDel.TriggerId == TriggerId
                                     select actionRepDel;
                lstTrigger.Remove(actionToDelete.ElementAt(0));


            }
            Session["dhFeedEvtTriggModel"] = dhFeedEvtTriggModel;

            return PartialView(lstTrigger);
        }

        public ActionResult AddTriggerToEdit(Int16 TriggerId, string TriggerName)
        {
            Session["Consul"] = TriggerId;
            DHFeedEvtTriggModel dhFeedEvtTriggModel = null;

            if (Session["dhFeedEvtTriggModel"] != null)
            {
                dhFeedEvtTriggModel = Session["dhFeedEvtTriggModel"] as DHFeedEvtTriggModel;

            }
            DHFeedTriggerDep dhFeedTrigger = new DHFeedTriggerDep();
            dhFeedTrigger.TriggerId = TriggerId;
            dhFeedTrigger.TriggerName = TriggerName;
            dhFeedEvtTriggModel.lstDHFeedTriggerDep.Add(dhFeedTrigger);
            Session["RDSReportModel"] = dhFeedEvtTriggModel;
            return RedirectToAction("Edit", new { FeedId = dhFeedEvtTriggModel.FeedId });
        }
        public ActionResult DeleteTriggerAction(Int16 TriggerId)
        {
            DHFeedEvtTriggModel dhFeedEvtTriggModel = null;

            if (Session["dhFeedEvtTriggModel"] != null)
            {
                dhFeedEvtTriggModel = Session["dhFeedEvtTriggModel"] as DHFeedEvtTriggModel;

            }
            var actionToDelete = from actionRepDel in dhFeedEvtTriggModel.lstDHFeedTriggerDep
                                 where actionRepDel.TriggerId == TriggerId
                                 select actionRepDel;
            dhFeedEvtTriggModel.lstDHFeedTriggerDep.Remove(actionToDelete.ElementAt(0));

            Session["RDSReportModel"] = dhFeedEvtTriggModel;

            return RedirectToAction("Edit", new { FeedId = dhFeedEvtTriggModel.FeedId });
        }
        #endregion for Trigger Dep
    }
}