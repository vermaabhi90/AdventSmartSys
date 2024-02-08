using SmartSys.BL.ProViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{
    public class QuickViewController : Controller
    {
        // GET: QuickView
        public ActionResult CaseTimeLine(int ProjectId, int TaskId)
        {
            ViewsBL blobj = new ViewsBL();
            GetTaskdurationByStatusModel model = new GetTaskdurationByStatusModel();
            model = blobj.GetTaskdurationByStatus(ProjectId, TaskId);
            List<GetTaskdurationByStatusModel> lst = model.LstStatus;
            ViewBag.TimeDuration = lst;
            return PartialView(model);
        }

    }
}