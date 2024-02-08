using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.SysDBCon;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{

    [Authorize]
    public class DhFeedController : Controller
    {
        DHFeedBL ObjBl = new DHFeedBL();
        // GET: DhFeed
        #region DH Feed Master
        public ActionResult FeedList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DhFeed/FeedList");
            if (FindFlag)
            {
                try
                {
                    //Make the sestion null for the current FeedRecord.
                    if (Session["dhFeedActionModel"] != null)
                    {
                        Session["dhFeedActionModel"] = null;
                    }

                    SmartSys.BL.DHFeedBL dataHubBlObj = new DHFeedBL();
                    List<DHFeedModel> lstFeed = dataHubBlObj.GetFeedList();
                    return View(lstFeed);
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
        public ActionResult CreateFeed(int iFeedID)
        {
            try
            {
                SmartSys.BL.DHFeedActionModel dhFeedActionModel = new DHFeedActionModel();
                //Get List of FeedTypeCode From database
                if (Session["dhFeedActionModel"] == null)
                {
                    SmartSys.BL.DHFeedBL dhFeedBlOBj = new DHFeedBL();
                    dhFeedActionModel = dhFeedBlOBj.GetFeedDetails(iFeedID);
                    dhFeedActionModel.FeedActionID = iFeedID;
                    ViewBag.DrpDwnDHActionMast = new SelectList(dhFeedActionModel.ListActionType, "ActionTypeCode", "Description");
                    Session["dhFeedActionModel"] = dhFeedActionModel;
                }
                else
                {
                    dhFeedActionModel = Session["dhFeedActionModel"] as SmartSys.BL.DHFeedActionModel;
                    dhFeedActionModel.FeedActionID = iFeedID;
                    ViewBag.DrpDwnDHActionMast = new SelectList(dhFeedActionModel.ListActionType, "ActionTypeCode", "Description");
                }
                return View(dhFeedActionModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public ActionResult SaveFeedDetails(FormCollection frmCollection)
        {

            try
            {
                SmartSys.BL.DHFeedActionModel dhFeedActionModel = null;
                string User_Id = User.Identity.GetUserId();
                if (Session["dhFeedActionModel"] != null)
                {
                    dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
                }
                dhFeedActionModel.FeedModel.FeedName = frmCollection["FeedModel.FeedName"].ToString();
                dhFeedActionModel.FeedModel.FeedTypeCode = frmCollection["FeedTypeCode"].ToString();
                dhFeedActionModel.FeedModel.Description = frmCollection["FeedModel.Description"].ToString();
                dhFeedActionModel.iUserID = 1;//WebSecurity.CurrentUserId;
                SmartSys.BL.DHFeedBL dhfeedBlObj = new DHFeedBL();
                string strResult = dhfeedBlObj.SaveFeedDetails(dhFeedActionModel, User_Id);
                //Make feedActionModel null for new request
                Session["dhFeedActionModel"] = null;
                return RedirectToAction("FeedList", "DhFeed");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult DeleteAction(int iSequenceNumber)
        {
            SmartSys.BL.DHFeedActionModel dhFeedActionModel = null;
            if (Session["dhFeedActionModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
            }

            var actionToDelete = from dhFeedAction in dhFeedActionModel.ListOfFeedActions
                                 where dhFeedAction.SequenceNO == iSequenceNumber
                                 select dhFeedAction;
            dhFeedActionModel.ListOfFeedActions.Remove(actionToDelete.ElementAt(0));



            //  dhFeedActionModel.ListOfFeedActionParameters.RemoveAll(remove => remove.SequenceNO == iSequenceNumber);

            Session["dhFeedActionModel"] = dhFeedActionModel;

            return RedirectToAction("CreateFeed", new { iFeedID = dhFeedActionModel.FeedModel.FeedID });
        }

        [HttpGet]
        public ActionResult DeleteParam(string Value, string strParamName, string url)
        {

            SmartSys.BL.DHFeedActionModel dhFeedActionModel = null;
            if (Session["dhFeedActionModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
            }

            var paramDelete = from dhActionParam in dhFeedActionModel.ListOfFeedActionParameters
                              where dhActionParam.Value == Value && dhActionParam.ParamName == strParamName
                              select dhActionParam;

            dhFeedActionModel.ListOfFeedActionParameters.Remove(paramDelete.ElementAt(0));
            Session["dhFeedActionModel"] = dhFeedActionModel;
            if (url == "Create")
            {
                return RedirectToAction("CreateFeed", "DhFeed", new { iFeedID = 0 });
            }
            else if (url == "XLSDestTable")
            {
                return RedirectToAction("XLSDestTable", "DhFeed", new { ActionType = url, SequenceNo = dhFeedActionModel.Sequence });
            }
            return RedirectToAction("SourceInfo", new { ActionType = dhFeedActionModel.Actiontype, SequenceNo = 0});
        }

        [HttpGet]
        public ActionResult AddActions()
        {
            SmartSys.BL.DHFeedActionModel dhFeedActionModel = null;
            if (Session["dhFeedActionModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;

                if (dhFeedActionModel.ListActionType == null)
                {
                    SmartSys.BL.DHFeedBL dhFeedBlOBj = new DHFeedBL();
                    dhFeedActionModel.ListActionType = dhFeedBlOBj.GetActionTypeList();
                    ViewBag.ActionTypeList = new SelectList(dhFeedActionModel.ListActionType, "ActionTypeCode", "Description");
                    //dhFeedActionModel.ListActionType = lstDHActionType;
                    Session["dhFeedActionModel"] = dhFeedActionModel;
                }
                else
                {
                    ViewBag.ActionTypeList = new SelectList(dhFeedActionModel.ListActionType, "ActionTypeCode", "Description");
                }
            }

            SmartSys.BL.DHActionModel dhActionModel = new DHActionModel();
            return PartialView(dhActionModel);
        }

        [HttpPost]
        public ActionResult AddActionsToFeed(FormCollection frmCollection)
        {
            DHActionModel dhActionModel = new DHActionModel();
            dhActionModel.dhActionType = new DHActionType();

            dhActionModel.dhActionType.ActionTypeCode = frmCollection["ActionTypeCode"].ToString();
            dhActionModel.SequenceNO = Convert.ToInt32(frmCollection["SequenceNO"].ToString());

            DHFeedActionModel dhFeedActionModel = null;
            if (Session["dhFeedActionModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
            }
            //Get Action Name from the list of action type present in model
            var actionName = from dhActionType in dhFeedActionModel.ListActionType
                             where dhActionType.ActionTypeCode == dhActionModel.dhActionType.ActionTypeCode
                             select dhActionType.Description;

            dhActionModel.dhActionType.Description = actionName.ElementAt(0);
            if (frmCollection["Description"].ToString() == "")
            {
                dhActionModel.Description = actionName.ElementAt(0);
            }
            else
            {
                dhActionModel.Description = frmCollection["Description"].ToString();
            }
            {
                dhFeedActionModel.ListOfFeedActions.Add(dhActionModel);
                Session["dhFeedActionModel"] = dhFeedActionModel;
            }

            return RedirectToAction("CreateFeed", new { iFeedID = dhFeedActionModel.FeedModel.FeedID });
        }

        //This is Json method to check for duplicate sequnece number in Action List
        public JsonResult CheckForDuplicateSequenceNo(string strSeqNumber)
        {
            int iSeqNo = 0;
            int.TryParse(strSeqNumber, out iSeqNo);
            DHFeedActionModel dhFeedActionModel = null;
            if (Session["dhFeedActionModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
            }
            var ActionSqNo = from dhAction in dhFeedActionModel.ListOfFeedActions
                             where dhAction.SequenceNO == iSeqNo
                             select dhAction.SequenceNO;
            if (ActionSqNo.Count() > 0)
            {
                //sequence number is already present
                return new JsonResult { Data = true, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult { Data = false, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        public JsonResult ValidateParamName(string strParamName, int iSequenceNo)
        {
            DHFeedActionModel dhFeedActionModel = null;
            if (Session["dhFeedActionModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
            }

            var ParamRecord = from dhActionParam in dhFeedActionModel.ListOfFeedActionParameters
                              where dhActionParam.ParamName == strParamName && dhActionParam.SequenceNO == iSequenceNo
                              select dhActionParam;
            if (ParamRecord.Count() > 0)
            {
                return new JsonResult { Data = true, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult { Data = false, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        [HttpGet]
        public ActionResult AddParametersAndValuesToAction(int iSequenceNumber)
        {

            DHFeedActionParam dhFeedActionParam = new DHFeedActionParam();
            TempData["sequenceNo"] = iSequenceNumber;
            dhFeedActionParam.ParamName = "";
            dhFeedActionParam.SequenceNO = iSequenceNumber;
            dhFeedActionParam.Value = "";

            return PartialView(dhFeedActionParam);
        }

        [HttpPost]
        public ActionResult AddParametersAndValuesToAction(DHFeedActionParam dhFeedActionParam)
        {
            if (TempData["sequenceNo"] != null)
            {
                dhFeedActionParam.SequenceNO = Convert.ToInt32(TempData["sequenceNo"].ToString());
            }
            DHFeedActionModel dhFeedActionModel = null;
            if (Session["dhFeedActionModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
            }
            dhFeedActionModel.ListOfFeedActionParameters.Add(dhFeedActionParam);
            Session["dhFeedActionModel"] = dhFeedActionModel;
            return RedirectToAction("CreateFeed", "DhFeed", new { iFeedID = dhFeedActionModel.FeedModel.FeedID });
        }

        public ActionResult GetActionTypDrpList(string FeedType)
        {
            DHFeedBL dhFeedBlOBj = new DHFeedBL();
            DHFeedActionModel dhFeedActionModel = new DHFeedActionModel();
            dhFeedActionModel.ListActionType = new List<DHActionType>();
            dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
            dhFeedActionModel.ListActionType = dhFeedBlOBj.ActionTypDrpList(FeedType);
            dhFeedActionModel.FeedTypeCode = FeedType;
            Session["dhFeedActionModel"] = dhFeedActionModel;
            return RedirectToAction("CreateFeed", new { iFeedID = 0 });

        }
        public ActionResult ConnectionList()
        {
            List<SysDBConModel> lstConn = new List<SysDBConModel>();
            SysDBConBL objBL = new SysDBConBL();
            string User_Id = User.Identity.GetUserId();
            lstConn = objBL.GetDBConnList(User_Id);
            return View(lstConn);
        }

        public ActionResult AddConnectionList(Int16 ConnectionId)
        {
            DHFeedActionModel dhFeedActionModel = null;
            if (Session["dhFeedActionModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
            }
            DHActionModel dhDemo = new DHActionModel();
            dhDemo = Session["ActionInfo"] as DHActionModel;
            dhDemo.Description = Convert.ToString(ConnectionId);

            dhFeedActionModel.ListOfFeedActions.Add(dhDemo);
            Session["dhFeedActionModel"] = dhFeedActionModel;
            return RedirectToAction("CreateFeed", "DhFeed", new { iFeedID = 0 });

        }
        public ActionResult DHActionDetail(string ActionType, string Description, int Sequence, string FeedType)
        {
            DHFeedActionModel dhFeedActionModel = null;
            if (Session["dhFeedActionModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
            }

            List<DHActionDeatil> lstActionDet = new List<DHActionDeatil>();
            DHFeedBL dhFeedBlOBj = new DHFeedBL();

            lstActionDet = dhFeedBlOBj.GetDHActionDetail();
            Session["ActionInfo"] = null;
            foreach (DHActionDeatil temp in lstActionDet)
            {
                if (temp.ActionTypeCode == ActionType)
                {
                    DHActionModel dhDemo = new DHActionModel();
                    dhDemo.dhActionType = new DHActionType();
                    dhDemo.ActionType = ActionType;
                    dhDemo.Description = Description;
                    dhDemo.SequenceNO = Sequence;
                    Session["ActionInfo"] = dhDemo;
                    if (ActionType == "SourceInfo" && FeedType == "XLSLoader" || ActionType == "DBLoaderProcess" || ActionType == "ExceSP")
                    {
                        dhFeedActionModel.ListOfFeedActions.Add(dhDemo);
                        Session["dhFeedActionModel"] = dhFeedActionModel;
                        return RedirectToAction("EWSourceInfo", "DhFeed", new { ActionType = ActionType, SequenceNo = Sequence });
                    }
                    if (ActionType == "SourceInfo" && FeedType == "CSVLoader")
                    {
                        dhFeedActionModel.ListOfFeedActions.Add(dhDemo);
                        Session["dhFeedActionModel"] = dhFeedActionModel;
                        return RedirectToAction("SourceInfo", "DhFeed", new { ActionType = ActionType, SequenceNo = Sequence });
                    }
                    else if (ActionType == "DestTable" && FeedType == "XLSLoader")
                    {
                        dhFeedActionModel.ListOfFeedActions.Add(dhDemo);
                        Session["dhFeedActionModel"] = dhFeedActionModel;
                        return RedirectToAction("XLSDestTable", "DhFeed", new { ActionType = ActionType, SequenceNo = Sequence });
                    }
                    else if (ActionType == "DestTable" && FeedType == "CSVLoader")
                    {
                        dhFeedActionModel.ListOfFeedActions.Add(dhDemo);
                        Session["dhFeedActionModel"] = dhFeedActionModel;
                        return RedirectToAction("XLSDestTable", "DhFeed", new { ActionType = ActionType, SequenceNo = Sequence });
                    }
                    else if (ActionType == "DestConn")
                    {
                        return RedirectToAction(temp.Value, "DhFeed");
                    }
                    else
                    {
                        dhFeedActionModel.ListOfFeedActions.Add(dhDemo);
                        Session["dhFeedActionModel"] = dhFeedActionModel;
                        return RedirectToAction(temp.Value, "DhFeed", new { ActionType = ActionType });
                    }
                }

            }
            DHActionModel dhDemo1 = new DHActionModel();
            dhDemo1.dhActionType = new DHActionType();
            dhDemo1.ActionType = ActionType;
            dhDemo1.Description = Description;
            dhDemo1.SequenceNO = Sequence;
            dhFeedActionModel.ListOfFeedActions.Add(dhDemo1);
            Session["dhFeedActionModel"] = dhFeedActionModel;
            return View();
        }
        public ActionResult SourceInfo(string ActionType, int SequenceNo)
        {

            DHFeedBL dhFeedBlOBj = new DHFeedBL();
            DHFeedActionModel dhFeedActionModel = new DHFeedActionModel();
            if (Session["dhFeedActionModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
            }
            if (ActionType != null)
                dhFeedActionModel.Actiontype = ActionType;
            dhFeedActionModel.Sequence = SequenceNo;
            dhFeedActionModel.DrpDwnlstParamName = new List<DHFeedActionParam>();
            dhFeedActionModel.DrpDwnlstParamName = dhFeedBlOBj.GetSelectedFeedParamName();
            ViewBag.ParamNameList = new SelectList(dhFeedActionModel.DrpDwnlstParamName, "Value", "ParamName");
            return View(dhFeedActionModel);
        }
        public ActionResult XLSDestTable(string ActionType, int SequenceNo)
        {
            DHFeedBL dhFeedBlOBj = new DHFeedBL();
            DHFeedActionModel dhFeedActionModel = new DHFeedActionModel();
            if (Session["dhFeedActionModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
            }
            if (ActionType != null)
                dhFeedActionModel.Actiontype = ActionType;
            dhFeedActionModel.Sequence = SequenceNo;
            dhFeedActionModel.DrpDwnlstParamName = new List<DHFeedActionParam>();
            dhFeedActionModel.DrpDwnlstParamName = dhFeedBlOBj.GetSelectedFeedParamName();
            ViewBag.ParamNameList = new SelectList(dhFeedActionModel.DrpDwnlstParamName, "Value", "ParamName");
            return View(dhFeedActionModel);
        }
        public ActionResult EWSourceInfo(string ActionType, int SequenceNo)
        {
            DHFeedBL dhFeedBlOBj = new DHFeedBL();
            DHFeedActionModel dhFeedActionModel = new DHFeedActionModel();
            if (Session["dhFeedActionModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
            }
            if (ActionType != null)
                dhFeedActionModel.Actiontype = ActionType;
            dhFeedActionModel.Sequence = SequenceNo;
            dhFeedActionModel.DrpDwnlstParamName = new List<DHFeedActionParam>();
            dhFeedActionModel.DrpDwnlstParamName = dhFeedBlOBj.GetSelectedFeedParamName();
            ViewBag.ParamNameList = new SelectList(dhFeedActionModel.DrpDwnlstParamName, "Value", "ParamName");
            return View(dhFeedActionModel);
        }

        public ActionResult AddAtionParam(string ParamName, string Value, string method)
        {
            DHFeedActionModel dhFeedActionModel = new DHFeedActionModel();
            if (Session["dhFeedActionModel"] != null)
            {
                dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
            }
            DHFeedActionParam tempModel = new DHFeedActionParam();
            tempModel.ParamName = ParamName;
            tempModel.Value = Value;
            tempModel.ActionType = dhFeedActionModel.Actiontype;
            tempModel.SequenceNO = dhFeedActionModel.Sequence;
            dhFeedActionModel.ListOfFeedActionParameters.Add(tempModel);
            Session["dhFeedActionModel"] = dhFeedActionModel;
            if (method == "EWSourceInfo")
            {
                return RedirectToAction(method, "DhFeed", new { SequenceNo = dhFeedActionModel.Sequence });
            }
            else
                return RedirectToAction(method, "DhFeed", new { ActionType = dhFeedActionModel.Actiontype, SequenceNo = dhFeedActionModel.Sequence });

        }
        //[HttpPost]
        //public ActionResult SourceInfo(string ParamName, string ParamValue)
        //{
        //    DHFeedActionModel dhFeedActionModel = null;
        //    if (Session["dhFeedActionModel"] != null)
        //    {
        //        dhFeedActionModel = Session["dhFeedActionModel"] as DHFeedActionModel;
        //    }
        //    dhFeedActionModel.ListOfFeedActionParameters = new List<DHFeedActionParam>();
        //    DHFeedActionParam TempModel = new DHFeedActionParam();
        //    TempModel.ParamName = ParamName;
        //    TempModel.Value = ParamValue;
        //    dhFeedActionModel.ListOfFeedActionParameters.Add(TempModel);
        //    Session["dhFeedActionModel"] = dhFeedActionModel;
        //    return RedirectToAction("SourceInfo", "DhFeed");
        //}

        public ActionResult DHAdhocRun(int Flag)
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/DhFeed/DHAdhocRun?Flag=0");
            if (FindFlag)
            {
                if (Flag == 0)
                {
                    Session["FeedTypeCode"] = null;
                }
                SmartSys.BL.DHFeedBL dataHubBlObj = new DHFeedBL();
                List<DHFeedModel> lstFeed = dataHubBlObj.GetFeedList();
                ViewBag.ParamNameList = new SelectList(lstFeed, "FeedID", "FeedName");
                DHAchocRun DHAdhocModel = new DHAchocRun();
                if (Session["FeedTypeCode"] != null)
                {
                    DHAdhocModel = Session["FeedTypeCode"] as DHAchocRun;
                }
                DHAdhocModel.LstadhocRun = dataHubBlObj.GetDHAdhocRunList();
                return View(DHAdhocModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ChkFeedType(int FeedId)
        {
            try
            {
                DataSet dsobj = new DataSet();
                DHFeedBL dataHubBlObj = new DHFeedBL();
                dsobj = dataHubBlObj.ChkFeedType();
                DHAchocRun DHAdhocModel = new DHAchocRun();
                DataRow dataRow = null;
                dataRow = dsobj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToInt32(r["FeedId"]) == FeedId);
                DHAdhocModel.CHkFeedType = dataRow["FeedTypeCode"].ToString();
                DHAdhocModel.FeedId = FeedId;
                Session["FeedTypeCode"] = DHAdhocModel;
                return RedirectToAction("DHAdhocRun", new { Flag = 1 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult DHAdhocRun(HttpPostedFileBase file, FormCollection fc)
        {
            try
            {
                SmartSys.BL.DHFeedBL dataHubBlObj = new DHFeedBL();
                DataSet dsFtp = new DataSet();
                dsFtp = dataHubBlObj.GetFTPDetail(Convert.ToInt16(fc["FeedId"]));
                string FTPServer = "";
                string Uid = "";
                string Pwd = "";

                if (dsFtp != null)
                {
                    if (dsFtp.Tables.Count > 0)
                    {
                        if (dsFtp.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsFtp.Tables[0].Rows)
                            {
                                if (dr["ParamName"].ToString() == "FTPServer")
                                {
                                    FTPServer = dr["Value"].ToString();
                                }
                                else if (dr["ParamName"].ToString() == "FTPUser")
                                {
                                    Uid = dr["Value"].ToString();
                                }
                                else if (dr["ParamName"].ToString() == "FTPPwd")
                                {
                                    Pwd = dr["Value"].ToString();
                                }
                            }
                        }
                        else
                        {
                            DHAchocRun dhAdhocModel1 = new DHAchocRun();
                            //  dhAdhocModel.FileName = FileName;
                            dhAdhocModel1.FeedId = Convert.ToInt16(fc["FeedId"]);
                            dhAdhocModel1.CreatedBy = 1;
                            dhAdhocModel1.StatusId = 2;
                            string @User_Id = User.Identity.GetUserId();
                            int Result = dataHubBlObj.SaveUploadData(dhAdhocModel1, @User_Id);

                            if (Result == 4)
                            {
                                return RedirectToAction("DHAdhocRun", new { Flag = 0 });
                            }
                        }
                    }
                }
                string fileName = Path.GetFileName(file.FileName);
                string[] FileSplit = fileName.Split('.');
                string FileEx = FileSplit[FileSplit.Length - 1].ToString();
                String guid = Guid.NewGuid().ToString();
                string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                string FileName = FileSplit[0].ToString() + "_" + date + "_" + time + "_" + guid + "." + FileEx;
                int len = FileName.Length;

                string localPath = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(localPath);
                FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(FTPServer + FileName);
                requestFTPUploader.Credentials = new NetworkCredential(Uid, Pwd);
                requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;

                FileInfo fileInfo = new FileInfo(localPath);
                FileStream fileStream = fileInfo.OpenRead();
                int bufferLength = 2048;
                byte[] buffer = new byte[bufferLength];
                Stream uploadStream = requestFTPUploader.GetRequestStream();
                int contentLength = fileStream.Read(buffer, 0, bufferLength);
                while (contentLength != 0)
                {
                    uploadStream.Write(buffer, 0, contentLength);
                    contentLength = fileStream.Read(buffer, 0, bufferLength);
                }
                uploadStream.Close();
                fileStream.Close();
                requestFTPUploader = null;

                DHAchocRun dhAdhocModel = new DHAchocRun();
                dhAdhocModel.FileName = FileName;
                dhAdhocModel.FeedId = Convert.ToInt16(fc["FeedId"]);
                dhAdhocModel.CreatedBy = 1;
                dhAdhocModel.StatusId = 2;
                string @User_Id1 = User.Identity.GetUserId();
                int Result1 = dataHubBlObj.SaveUploadData(dhAdhocModel, @User_Id1);

                if (Result1 == 4)
                {
                    return RedirectToAction("DHAdhocRun", new { Flag = 0 });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("DHAdhocRun", new { Flag = 0 });
        }
        #endregion DH Feed Master
        #region Review
        #region Inventory Ageing Analysis
        public ActionResult GetInventoryAgeingAnalysisList()
        {
            List<InventoryAgeingAnalysisModel> List = new List<InventoryAgeingAnalysisModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                DateTime AsOfDate = DateTime.Today;
                String CompCode = "Advent";
                List = ObjBl.GetInventoryAgeingAnalysisList(CompCode, AsOfDate, User_Id);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return View(List);

        }
        public JsonResult GetInventoryAgeingAnalysisDate(String CompCode,DateTime AsOfDate)
        {
            List<InventoryAgeingAnalysisModel> List = new List<InventoryAgeingAnalysisModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                List = ObjBl.GetInventoryAgeingAnalysisList(CompCode, AsOfDate, User_Id);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }            
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveInvAgingAdvent(string ItemNo, string Description, string Comment)
        {
            InventoryAgeingAnalysisModel Model = new InventoryAgeingAnalysisModel();
            int ErrorCode = 0;
            Model.ItemNo = ItemNo;
            Model.Description = Description;
            Model.Comment = Comment;
            string BunchId = DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "_" + Guid.NewGuid().ToString();
            string User_Id = User.Identity.GetUserId();
            ErrorCode = ObjBl.SaveInvAgingAdvent(ItemNo, Description, Comment, BunchId, User_Id);
            return Json(ErrorCode, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowInvAgingAdvent(string Description, string ItemNo)
        {
            InventoryAgeingAnalysisModel Model = new InventoryAgeingAnalysisModel();
            List<InventoryAgeingAnalysisModel> MPRList = new List<InventoryAgeingAnalysisModel>();
            string CompCode = "Advent";
            try
            {
                MPRList = ObjBl.ShowInvAgingAdvent(CompCode, ItemNo);
                ViewBag.MPRLst = MPRList;
            }
            catch (Exception)
            {

                throw;
            }
            return Json(MPRList, JsonRequestBehavior.AllowGet);
        }

        #endregion Inventory Ageing Analysis
        #region Inventory
        public ActionResult InventoryView()
        {
            return View();
        }
        public JsonResult GetInventoryData(string CompCode,DateTime AsOfDate)
        {
            List<InventoryModel> List = new List<InventoryModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                List = ObjBl.GetInventoryList(CompCode, AsOfDate, User_Id);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return Json(List,JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowHistoryInventoryData(string CompCode, string ItemNo)
        {
            List<InventoryModel> HistoryList = new List<InventoryModel>();
            try
            {
                HistoryList = ObjBl.ShowInventoryAdvent(CompCode, ItemNo);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return Json(HistoryList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveInventoryAdventData(string ItemNo, string Description, string Comment)
        {
            InventoryModel Model = new InventoryModel();
            int ErrorCode = 0;
            Model.ItemNo = ItemNo;
            Model.Description = Description;
            Model.Comment = Comment;
            string BunchId = DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "_" + Guid.NewGuid().ToString();
            string User_Id = User.Identity.GetUserId();
            ErrorCode = ObjBl.SaveInventoryAdventData(ItemNo, Description, Comment, BunchId, User_Id);
            return Json(ErrorCode, JsonRequestBehavior.AllowGet);
        }
        #endregion Inventory
        #region MaterialProcuredVsReduction
        public ActionResult GetMaterialProcuredVsReductionList()
        {           
            return View();
        }
        public JsonResult GetMPVRAdventData(string CompCode, DateTime FromDate, DateTime ToDate)
        {
            List<MaterialProcuredVsReductionModel> List = new List<MaterialProcuredVsReductionModel>();
            string User_Id = User.Identity.GetUserId();        
            List = ObjBl.GetMaterialProcuredVsReductionList(CompCode, FromDate, ToDate, User_Id);
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowMaterialProcuredVsReduction(string DocumentNo,string ItemName, DateTime FromDate, DateTime ToDate, string CompCode)
        {
            MaterialProcuredVsReductionModel Model = new MaterialProcuredVsReductionModel();
            List<MaterialProcuredVsReductionModel> MPRList = new List<MaterialProcuredVsReductionModel>();
            try
            {             
                MPRList = ObjBl.ShowMaterialProcuredVsReduction(CompCode, FromDate, ToDate, DocumentNo, ItemName);
                ViewBag.MPRLst = MPRList;
            }
            catch (Exception)
            {

                throw;
            }
            return Json(MPRList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowMPRAdvent(string DocumentNo, string ItemName, DateTime FromDate, DateTime ToDate, string CompCode)
        {
            MaterialProcuredVsReductionModel Model = new MaterialProcuredVsReductionModel();
            List<MaterialProcuredVsReductionModel> MPRList = new List<MaterialProcuredVsReductionModel>();
            try
            {               
                MPRList = ObjBl.ShowMaterialProcuredVsReduction(CompCode, FromDate, ToDate, DocumentNo, ItemName);
                ViewBag.MPRLst = MPRList;
            }
            catch (Exception)
            {

                throw;
            }
            return Json(MPRList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveMPRA(string ItemName, string DocumentNo,string Comment)
        {
            MaterialProcuredVsReductionModel Model = new MaterialProcuredVsReductionModel();
            int ErrorCode = 0;
            Model.ItemName = ItemName;
            Model.DocumentNo = DocumentNo;
            Model.Comment = Comment;
            string BunchId = DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "_" + Guid.NewGuid().ToString();
            string User_Id = User.Identity.GetUserId();
            ErrorCode = ObjBl.SaveMPRAdvent(ItemName, DocumentNo, Comment, BunchId, User_Id);
            return Json(ErrorCode, JsonRequestBehavior.AllowGet);
        }
        #endregion MaterialProcuredVsReduction
        #region NNR
        public ActionResult GetNNRList()
        {
            return View();
        }
        public JsonResult GetNNRAdventData(string CompCode,DateTime FromDate,DateTime ToDate)
        {
            List<NNRModel> List = new List<NNRModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                List = ObjBl.GetNNRList(CompCode, FromDate, ToDate, User_Id);
            }
            catch (Exception)
            {

                throw;
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowNNRAdventData(string DocumentNo,string ItemNo, string CompCode,DateTime FromDate,DateTime ToDate)
        {
            List<NNRModel> List = new List<NNRModel>();
            try
            {
                List = ObjBl.ShowNNRList(DocumentNo, ItemNo, CompCode, FromDate, ToDate);
            }
            catch (Exception)
            {

                throw;
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveNNRAdventData(string DocumentNo, string MPN,string ItemNo, string Comment)
        {
            NNRModel Model = new NNRModel();
            int ErrorCode = 0;
            Model.DocumentNo = DocumentNo;
            Model.MPN = MPN;
            Model.Comment = Comment;
            string BunchId = DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "_" + Guid.NewGuid().ToString();
            string User_Id = User.Identity.GetUserId();
            ErrorCode = ObjBl.SaveNNRAdvent(DocumentNo, MPN, Comment, ItemNo, BunchId, User_Id);
            return Json(ErrorCode, JsonRequestBehavior.AllowGet);
        }
        #endregion NNR       
        #region Purchase Report Monthwise
        public ActionResult GetPurchaseRepotMonthWiseList()
        {
            return View();
        }
        public JsonResult GetPurchaseRepotMonthWiseData(string CompCode, DateTime FromDate, DateTime ToDate)
        {
            List<PurchaseMonthwiseModel> List = new List<PurchaseMonthwiseModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                List = ObjBl.GetPurchaseMonthwiseList(CompCode, FromDate, ToDate, User_Id);
            }
            catch (Exception)
            {

                throw;
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowPurchaseMonthwiseList(string DocumentNo, string ItemNo,int LineNo, string CompCode, DateTime FromDate, DateTime ToDate)
        {
            List<PurchaseMonthwiseModel> List = new List<PurchaseMonthwiseModel>();
            try
            {
                List = ObjBl.ShowPurchaseMonthwiseList(DocumentNo, ItemNo, LineNo, CompCode, FromDate, ToDate);
            }
            catch (Exception)
            {

                throw;
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SavePurchMonthwiseAdventData(string DocumentNo, string ItemNo,string LineNo, string Comment)
        {           
            NNRModel Model = new NNRModel();
            int ErrorCode = 0;
            Model.DocumentNo = DocumentNo;
            Model.ItemNo = ItemNo;
            Model.Comment = Comment;
            string BunchId = DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "_" + Guid.NewGuid().ToString();
            string User_Id = User.Identity.GetUserId();
            ErrorCode = ObjBl.SavePurchaseMonthwiseData(DocumentNo, Comment, ItemNo, LineNo, BunchId, User_Id);
            return Json(ErrorCode, JsonRequestBehavior.AllowGet);
        }
        #endregion Purchase Report Monthwise
        #region Pending PO
        public ActionResult GetPendingPOList()
        {
            List<PendingPOModel> List = new List<PendingPOModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                string CompCode = "Advent";
                List = ObjBl.PendingPOList(CompCode, User_Id);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return View(List);
        }
        public JsonResult GetPendingPOData(string CompCode)
        {
            List<PendingPOModel> List = new List<PendingPOModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                List = ObjBl.PendingPOList(CompCode,User_Id);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowPendingPOList(string DocumentNo, string ItemNo, int LineNo, string CompCode)
        {
            List<PendingPOModel> List = new List<PendingPOModel>();
            try
            {
                List = ObjBl.ShowPendPOList(DocumentNo, ItemNo, LineNo, CompCode);
            }
            catch (Exception)
            {

                throw;
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SavePendingPOAdventData(string DocumentNo, string LineNo, string ItemNo,  string Comment)
        {
            PendingPOModel Model = new PendingPOModel();
            int ErrorCode = 0;
            Model.DocumentNo = DocumentNo;
            Model.ItemNo = ItemNo;
            Model.Comment = Comment;
            string BunchId = DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "_" + Guid.NewGuid().ToString();
            string User_Id = User.Identity.GetUserId();
            ErrorCode = ObjBl.SavePendPOAdvent(DocumentNo, ItemNo, LineNo, Comment, BunchId, User_Id);
            return Json(ErrorCode, JsonRequestBehavior.AllowGet);
        }
        #endregion Pending PO
        #region Franchise 
        public ActionResult GetFranchiseList()
        {
            return View();
        }
        public JsonResult GetFranchiseListData(String CompCode,DateTime FromDate, DateTime ToDate)
        {
            List<FranchiseModel> List = new List<FranchiseModel>();
            string User_Id = User.Identity.GetUserId();
            try
            {
                List = ObjBl.GetFranchiseList(CompCode, FromDate, ToDate, User_Id);
            }
            catch (Exception)
            {

                throw;
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowFranchiseList(string VendorNo,string CompCode, DateTime FromDate, DateTime ToDate)
        {
            List<FranchiseModel> List = new List<FranchiseModel>();
            try
            {
                List = ObjBl.ShowFranchiseList(VendorNo, CompCode, FromDate, ToDate);
            }
            catch (Exception)
            {

                throw;
            }
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveFranchiseAdventData(string VendorName, string VendorNo, string Comment)
        {
            FranchiseModel Model = new FranchiseModel();
            int ErrorCode = 0;
            Model.VendorNo = VendorNo;
            Model.VendorName = VendorName;
            Model.Comment = Comment;
            string BunchId = DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "_" + Guid.NewGuid().ToString();
            string User_Id = User.Identity.GetUserId();
            ErrorCode = ObjBl.SaveFranchiseAdvent(VendorName, VendorNo, Comment, BunchId, User_Id);
            return Json(ErrorCode, JsonRequestBehavior.AllowGet);
        }
        #endregion Franchise
        #endregion Review
    }
}