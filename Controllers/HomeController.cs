#region Copyright Syncfusion Inc. 2001 - 2016
// Copyright Syncfusion Inc. 2001 - 2016. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Microsoft.AspNet.Identity;
using SmartSys.BL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");

            }
            else
                return View();
        }

        public JsonResult EmailNotification()
        {
            int cnt = 0;
            string MSg = "";
            string User_Id = User.Identity.GetUserId();

            if (Session["count"] != null)
            {
                cnt = (int)Session["count"];
            }
            SmartSys.BL.BOMAdmin.BOMAdminBL BLObj = new SmartSys.BL.BOMAdmin.BOMAdminBL();
            SmartSys.BL.EmailProcessing.mailmodel model = BLObj.GetMailProcessingDetail(User_Id);
            List<SmartSys.BL.EmailProcessing.mailmodel> MailList = model.MailList;
            int count = MailList.Count;
            int newcount = count - cnt;
            if (newcount > 0)
            { MSg = " You Have " + newcount + "  New e-Mails.</br>" + Environment.NewLine; }

            {
                if (newcount == 1)
                    MSg = MSg + "Subject: " + MailList[MailList.Count - 1].Subject;
                if (count != cnt)
                {
                    Session["count"] = count;
                }

            }

            return Json(MSg, JsonRequestBehavior.AllowGet);

        }
        [Authorize]
        public ActionResult Dashboard()
        {
            if (Request.IsAuthenticated)
            {
                List<IssueModel> LstIssueCompleteList = new List<IssueModel>();
                string User_Id = User.Identity.GetUserId();
                AdminBL issuebl = new AdminBL();
                LstIssueCompleteList = issuebl.GetIssueDetailListHaveCompleteStatus(User_Id);
                ViewBag.signoffCount = LstIssueCompleteList.Count();
                AdminBL objbl = new AdminBL();
                List<Dashboard> lstdashboard = new List<Dashboard>();
                if (Session["UserDashBord"] != null)
                {
                    lstdashboard = Session["UserDashBord"] as List<Dashboard>;
                }
                else
                {
                    lstdashboard = objbl.GetDashBoardByUser(User.Identity.GetUserId());
                    Session["UserDashBord"] = lstdashboard;
                }
                //lstdashboard = objbl.GetDashBoardByUser(User.Identity.GetUserId());

                //SysUser objUser = new SysUser();
                //if (Session["UserProfile"] != null)
                //    return View(lstdashboard);
                //else
                //{

                //    return View(lstdashboard);
                //}
                return View(lstdashboard);

            }
            else
                return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Supermatic application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Supermatic contact page.";

            return View();
        }
        public ActionResult Clearcache()
        {
            Session.Abandon();
            return RedirectToAction("Dashboard", "Home");
        }
        public ActionResult GetMainMenu()
        {

            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            string UserIdr = User.Identity.GetUserName();
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            // int day = (int)DateTime.Now.DayOfWeek;
            ViewBag.WeekNO = weekNum;
            string Date = DateTime.Now.ToString("dd-MM-yyy");
            ViewBag.DateTime = Date;
            if (Request.IsAuthenticated)
            {
                string UserId = User.Identity.GetUserId();
                AdminBL objBl = new AdminBL();
                string LoginAs = objBl.GetEmployeeName(UserId);
                ViewBag.LoginAs = LoginAs;
                string Compcode = objBl.GetUserCompCode(UserId);
                ViewBag.Compcode = Compcode;
                // WebMatrix.WebData.WebSecurity.CurrentUserId;

                if (Session["UserMenu"] == null)
                {
                    lstTaskmodel = objBl.GetMenuList(UserId);

                    Session["UserMenu"] = lstTaskmodel;
                }
                else
                    lstTaskmodel = new List<SysTaskModel>();
                    lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];

                var userID = User.Identity.Name;
                var claimsIdentity = User.Identity as ClaimsIdentity;

                if (claimsIdentity != null)
                {
                    // the principal identity is a claims identity.
                    // now we need to find the NameIdentifier claim
                    var userIdClaim = claimsIdentity.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                    if (userIdClaim != null)
                    {
                        var userIdValue = userIdClaim.Value;
                    }
                }
            }
            else
            {
                Session["UserMenu"] = null;
            }
            return PartialView(lstTaskmodel);
        }

        //SmartSysStorageServices _blobStorageServices = new SmartSysStorageServices();
        //public ActionResult upload()
        //{
        //    CloudBlobContainer blobContainer = _blobStorageServices.GetCloudeTempBlobContainer();
        //    List<String> blobs = new List<string>();
        //    foreach(var blobItem in blobContainer.ListBlobs())
        //    {
        //        blobs.Add(blobItem.Uri.ToString());
        //    }
        //    return View(blobs);
        //}

        //[HttpPost]
        //public ActionResult upload(HttpPostedFileBase files)
        //{
        //    if(files != null)
        //    if(files.ContentLength>0)
        //    {
        //        CloudBlobContainer blobContainer = _blobStorageServices.GetCloudeTempBlobContainer();
        //        CloudBlockBlob blob = blobContainer.GetBlockBlobReference(files.FileName.ToLower());

        //        blob.UploadFromStream(files.InputStream, accessCondition: AccessCondition.GenerateEmptyCondition());

        //    }
        //    return RedirectToAction("upload");
        //}

        public ActionResult Calculator()
        {
            return PartialView();
        }
        public ActionResult CurrencyConverter()
        {
            return PartialView();
        }
    }
}