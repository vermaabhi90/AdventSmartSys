using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSys.DAL;
using SmartSys.Utility;
using System.Data;
using SmartSys.BL.RDS;
using SmartSys.DL;

namespace SmartSys.BL
{
    public class DHFeedBL
    {
        DHFeedDal ObjDal = new DHFeedDal();
        #region [FEED]
        public string SaveFeedDetails(DHFeedActionModel dhFeedActionModel, string User_Id)
        {
            string strResult = "";
            try
            {
                DataSet dsFeedDetails = new DataSet();

                //Get Dhfeed master details in table;
                DataTable dtFeedDetails = new DataTable("tbl_DHFeedMast");
                dtFeedDetails.Columns.Add("FeedId", typeof(System.Int32));
                dtFeedDetails.Columns.Add("FeedName", typeof(System.String));
                dtFeedDetails.Columns.Add("Description", typeof(System.String));
                dtFeedDetails.Columns.Add("FeedTypeCode", typeof(System.String));
                dtFeedDetails.Columns.Add("CreateDate", typeof(System.DateTime));
                dtFeedDetails.Columns.Add("CreatedBy", typeof(System.Int32));
                dtFeedDetails.Columns.Add("ModifyDate", typeof(System.DateTime));
                dtFeedDetails.Columns.Add("ModifiedBy", typeof(System.Int32));

                DataRow drFeedMaster = dtFeedDetails.NewRow();
                drFeedMaster["FeedId"] = dhFeedActionModel.FeedModel.FeedID;
                drFeedMaster["FeedName"] = dhFeedActionModel.FeedModel.FeedName;
                drFeedMaster["Description"] = dhFeedActionModel.FeedModel.Description;
                drFeedMaster["FeedTypeCode"] = dhFeedActionModel.FeedModel.FeedTypeCode;
                // drFeedMaster["CreateDate"] = ;
                drFeedMaster["CreatedBy"] = dhFeedActionModel.iUserID;
                // drFeedMaster["ModifyDate"] = ;
                drFeedMaster["ModifiedBy"] = dhFeedActionModel.iUserID;
                dtFeedDetails.Rows.Add(drFeedMaster);

                dsFeedDetails.Tables.Add(dtFeedDetails);

                //Get DHfeed action details in table
                DataTable dtFeedAction = new DataTable("tbl_DHFeedAction");
                dtFeedAction.Columns.Add("FeedId", typeof(System.Int32));
                dtFeedAction.Columns.Add("ActionTypeCode", typeof(System.String));
                dtFeedAction.Columns.Add("Description", typeof(System.String));
                dtFeedAction.Columns.Add("SequenceNo", typeof(System.Int32));

                foreach (DHActionModel dhActionModel in dhFeedActionModel.ListOfFeedActions)
                {
                    DataRow drFeedActions = dtFeedAction.NewRow();
                    drFeedActions["ActionTypeCode"] = dhActionModel.ActionType;
                    drFeedActions["Description"] = dhActionModel.Description;
                    drFeedActions["SequenceNo"] = dhActionModel.SequenceNO;
                    dtFeedAction.Rows.Add(drFeedActions);
                }
                dsFeedDetails.Tables.Add(dtFeedAction);

                //Get Parmater and values for action in table
                DataTable dtFeedActionParam = new DataTable("tbl_DHFeedActionParam");
                dtFeedActionParam.Columns.Add("FActionId", typeof(System.Int32));
                dtFeedActionParam.Columns.Add("ActionType", typeof(System.String));
                dtFeedActionParam.Columns.Add("ParamName", typeof(System.String));
                dtFeedActionParam.Columns.Add("Value", typeof(System.String));
                dtFeedActionParam.Columns.Add("SequenceNo", typeof(System.Int32));

                foreach (DHFeedActionParam dhFeedActionParam in dhFeedActionModel.ListOfFeedActionParameters)
                {
                    DataRow drFeedActParam = dtFeedActionParam.NewRow();
                    drFeedActParam["ActionType"] = dhFeedActionParam.ActionType;
                    drFeedActParam["ParamName"] = dhFeedActionParam.ParamName;
                    drFeedActParam["Value"] = dhFeedActionParam.Value;
                    drFeedActParam["SequenceNo"] = dhFeedActionParam.SequenceNO;
                    dtFeedActionParam.Rows.Add(drFeedActParam);
                }
                dsFeedDetails.Tables.Add(dtFeedActionParam);

                SmartSys.DAL.DHFeedDal dhFeedDALOBJ = new DHFeedDal();
                int iErrorCode = dhFeedDALOBJ.SaveFeedDetails(dsFeedDetails, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return strResult;
        }


        public DHFeedActionModel GetFeedDetails(int iFeedID)
        {
            DHFeedActionModel dhFeedActionModel = new DHFeedActionModel();
            int SourceFAction = 0;
            string StrSourceFAction = "";
            int DestTableFaction = 0;
            string StrDestTableFAction = "";
            try
            {
                SmartSys.DAL.DHFeedDal dhFeedDALOBJ = new DHFeedDal();
                DataSet dsFeedDetails = dhFeedDALOBJ.GetFeedDetails(iFeedID);

                if (dsFeedDetails != null)
                {
                    dhFeedActionModel.FeedModel = new DHFeedModel();
                    //Add feed details to Model 
                    if (dsFeedDetails.Tables[0].Rows.Count > 0)
                    {
                        dhFeedActionModel.FeedModel.FeedID = Convert.ToInt32(dsFeedDetails.Tables[0].Rows[0]["FeedId"]);
                        dhFeedActionModel.FeedModel.FeedName = dsFeedDetails.Tables[0].Rows[0]["FeedName"].ToString();
                        dhFeedActionModel.FeedModel.Description = dsFeedDetails.Tables[0].Rows[0]["Description"].ToString();
                        dhFeedActionModel.FeedModel.FeedType = dsFeedDetails.Tables[0].Rows[0]["FeedType"].ToString();
                        dhFeedActionModel.FeedTypeCode = dsFeedDetails.Tables[0].Rows[0]["FeedTypeCode"].ToString();
                    }

                    //add feed action details to model
                    //if(dsFeedDetails.Tables[1].Rows.Count > 0)
                    {
                        dhFeedActionModel.ListOfFeedActions = new List<DHActionModel>();
                        foreach (DataRow drFdAction in dsFeedDetails.Tables[1].Rows)
                        {
                            DHActionModel dhActionModel = new DHActionModel();
                            dhActionModel.FActionId = Convert.ToInt32(drFdAction["FActionId"]);
                            dhActionModel.FeedID = Convert.ToInt32(drFdAction["FeedId"]);
                            dhActionModel.Description = drFdAction["Description"].ToString();
                            dhActionModel.SequenceNO = Convert.ToInt32(drFdAction["SequenceNo"]);

                            dhActionModel.dhActionType = new DHActionType();
                            dhActionModel.ActionType = drFdAction["ActionTypeCode"].ToString();
                            if (dhActionModel.ActionType == "SourceInfo" || dhActionModel.ActionType == "EWSourceIn" || dhActionModel.ActionType == "DBLoaderProcess")
                            {
                                SourceFAction = dhActionModel.FActionId;
                                StrSourceFAction = dhActionModel.ActionType;
                            }
                            else if (dhActionModel.ActionType == "DestTable" || dhActionModel.ActionType == "EWDestTabl" || dhActionModel.ActionType == "ExceSP")
                            {
                                DestTableFaction = dhActionModel.FActionId;
                                StrDestTableFAction = dhActionModel.ActionType;
                            }
                            dhFeedActionModel.ListOfFeedActions.Add(dhActionModel);
                        }
                    }

                    //if(dsFeedDetails.Tables[2].Rows.Count > 0)
                    {
                        dhFeedActionModel.ListOfFeedActionParameters = new List<DHFeedActionParam>();
                        foreach (DataRow drActionParam in dsFeedDetails.Tables[2].Rows)
                        {
                            DHFeedActionParam dhFeedActionParam = new DHFeedActionParam();
                            dhFeedActionParam.FeedActionID = Convert.ToInt32(drActionParam["FActionId"]);
                            dhFeedActionParam.FeedActionParamID = Convert.ToInt32(drActionParam["FActionParamId"]);
                            dhFeedActionParam.SequenceNO = Convert.ToInt32(drActionParam["SequenceNo"]);
                            dhFeedActionParam.ParamName = drActionParam["ParamName"].ToString();
                            dhFeedActionParam.Value = drActionParam["Value"].ToString();
                            dhFeedActionParam.ActionType = drActionParam["ActionTypeCode"].ToString();
                            //if (dhFeedActionParam.FeedActionID == SourceFAction)
                            //{
                            //    dhFeedActionParam.ActionType = StrSourceFAction;
                            //}
                            //else if (dhFeedActionParam.FeedActionID == DestTableFaction)
                            //{
                            //    dhFeedActionParam.ActionType = StrDestTableFAction;
                            //}
                            dhFeedActionModel.ListOfFeedActionParameters.Add(dhFeedActionParam);
                        }
                    }


                    dhFeedActionModel.ListFeedType = new List<DhFeedType>();
                    dhFeedActionModel.ListActionType = new List<DHActionType>();
                    foreach (DataRow drFeedType in dsFeedDetails.Tables[3].Rows)
                    {
                        DhFeedType dhFeedType = new DhFeedType();
                        dhFeedType.Description = drFeedType["Description"].ToString();
                        dhFeedType.FeedTypeCode = drFeedType["FeedTypeCode"].ToString();

                        dhFeedActionModel.ListFeedType.Add(dhFeedType);
                    }
                    foreach (DataRow drActionType in dsFeedDetails.Tables[4].Rows)
                    {
                        DHActionType dhActionType = new DHActionType();
                        dhActionType.Description = drActionType["Description"].ToString();
                        dhActionType.ActionTypeCode = drActionType["ActionTypeCode"].ToString();
                        dhFeedActionModel.ListActionType.Add(dhActionType);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dhFeedActionModel;
        }
        public DataSet ChkFeedType()
        {
            DataSet dsObj = new DataSet();
            try
            {
                DHFeedDal dataHubDALObj = new DHFeedDal();
                dsObj = dataHubDALObj.GetFeedList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsObj;
        }
        public List<DHFeedModel> GetFeedList()
        {
            List<DHFeedModel> lstFeed = new List<DHFeedModel>();
            try
            {
                DHFeedDal dataHubDALObj = new DHFeedDal();
                DataSet dsFeedLists = dataHubDALObj.GetFeedList();
                if (dsFeedLists == null) return null;

                if (dsFeedLists.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsFeedLists.Tables[0].Rows)
                    {
                        DHFeedModel dhFeedModelObj = new DHFeedModel();
                        dhFeedModelObj.FeedID = Convert.ToInt32(dr["FeedId"].ToString());
                        dhFeedModelObj.FeedName = dr["FeedName"].ToString();
                        dhFeedModelObj.FeedType = dr["Description"].ToString();
                        dhFeedModelObj.FeedTypeCode = dr["FeedTypeCode"].ToString();
                        dhFeedModelObj.ModifiedBy = Convert.ToInt16(dr["ModifiedBy"].ToString());
                        dhFeedModelObj.ModifyDate = Convert.ToDateTime(dr["ModifyDate"].ToString());
                        dhFeedModelObj.Description = dr["Description"].ToString();
                        dhFeedModelObj.ModifiedByName = dr["ModifiedByName"].ToString();

                        lstFeed.Add(dhFeedModelObj);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstFeed;
        }

        public List<DHActionDeatil> GetDHActionDetail()
        {
            List<DHActionDeatil> lstActionDet = new List<DHActionDeatil>();
            try
            {
                DHFeedDal dataHubDALObj = new DHFeedDal();
                DataSet dsObj = new DataSet();
                dsObj = dataHubDALObj.GetDHActionDetail();
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            DHActionDeatil ModelObj = new DHActionDeatil();
                            ModelObj.ActionTypeCode = dr["ActionTypeCode"].ToString();
                            ModelObj.KeyName = dr["KeyName"].ToString();
                            ModelObj.Value = dr["Value"].ToString();
                            lstActionDet.Add(ModelObj);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstActionDet;
        }
        public List<DhFeedType> GetFeedTypeList()
        {
            List<DhFeedType> lstFeedType = new List<DhFeedType>();
            try
            {
                DHFeedDal dataHubDALObj = new DHFeedDal();
                DataSet dsFeedTypeList = dataHubDALObj.GetFeedTypeList();
                if (dsFeedTypeList == null) return null;

                if (dsFeedTypeList.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsFeedTypeList.Tables[0].Rows)
                    {
                        DhFeedType dhFeedTypeObj = new DhFeedType();
                        dhFeedTypeObj.FeedTypeCode = dr["FeedTypeCode"].ToString();
                        dhFeedTypeObj.Description = dr["Description"].ToString();
                        lstFeedType.Add(dhFeedTypeObj);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstFeedType;
        }

        public List<DHActionType> ActionTypDrpList(string FeedType)
        {
            List<DHActionType> lstActionTypeDrpList = new List<DHActionType>();
            try
            {
                DHFeedDal dataHubDALObj = new DHFeedDal();
                DataSet dsActionTypeList = dataHubDALObj.GetActionType(FeedType);
                if (dsActionTypeList == null) return null;

                if (dsActionTypeList.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsActionTypeList.Tables[0].Rows)
                    {
                        DHActionType dhActionTypeObj = new DHActionType();
                        dhActionTypeObj.ActionTypeCode = dr["ActionTypeCode"].ToString();
                        dhActionTypeObj.Description = dr["Description"].ToString();
                        lstActionTypeDrpList.Add(dhActionTypeObj);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstActionTypeDrpList;
        }
        public List<DHActionType> GetActionTypeList()
        {
            List<DHActionType> lstActionType = new List<DHActionType>();
            try
            {
                DHFeedDal dataHubDALObj = new DHFeedDal();
                DataSet dsActionTypeList = dataHubDALObj.GetActionTypeList();
                if (dsActionTypeList == null) return null;

                if (dsActionTypeList.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsActionTypeList.Tables[0].Rows)
                    {
                        DHActionType dhActionTypeObj = new DHActionType();
                        dhActionTypeObj.ActionTypeCode = dr["ActionTypeCode"].ToString();
                        dhActionTypeObj.Description = dr["Description"].ToString();
                        lstActionType.Add(dhActionTypeObj);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstActionType;
        }

        public List<DHFeedActionParam> GetSelectedFeedParamName()
        {
            List<DHFeedActionParam> LstParamName = new List<DHFeedActionParam>();
            try
            {
                DHFeedDal dataHubDALObj = new DHFeedDal();
                DataSet dsObj = new DataSet();
                dsObj = dataHubDALObj.GetSelectedFeedParamName();
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            DHFeedActionParam ParamNameModel = new DHFeedActionParam();
                            ParamNameModel.ParamName = dr["Parameter Name"].ToString();
                            ParamNameModel.Value = dr["Parameter Name"].ToString();
                            LstParamName.Add(ParamNameModel);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstParamName;

        }
        #endregion


        #region for DHFeedEvent and Trigger Dep Work

        public DHFeedEvtTriggModel GetDHFeedEvtTriggModelDetail(int FeedId)
        {
            DHFeedEvtTriggModel dhFeedEvtTriggMod = new DHFeedEvtTriggModel();
            dhFeedEvtTriggMod.lstDHFeedEventDep = new List<DHFeedEventDep>();
            dhFeedEvtTriggMod.lstDHFeedTriggerDep = new List<DHFeedTriggerDep>();
            try
            {
                SmartSys.DAL.DHFeedDal dhFeedDALOBJ = new DHFeedDal();
                DataSet dsFeedDetails = dhFeedDALOBJ.GetFeedDetails(FeedId);
                if (dsFeedDetails == null)
                    return null;
                if (dsFeedDetails.Tables[0].Rows.Count > 0)
                {
                    dhFeedEvtTriggMod.FeedId = Convert.ToInt32(dsFeedDetails.Tables[0].Rows[0]["FeedId"]);
                    dhFeedEvtTriggMod.FeedName = dsFeedDetails.Tables[0].Rows[0]["FeedName"].ToString();
                    dhFeedEvtTriggMod.Description = dsFeedDetails.Tables[0].Rows[0]["Description"].ToString();
                    dhFeedEvtTriggMod.FeedTypeCode = dsFeedDetails.Tables[0].Rows[0]["FeedTypeCode"].ToString();
                    dhFeedEvtTriggMod.ModifiedByName = dsFeedDetails.Tables[0].Rows[0]["ModifiedByName"].ToString();
                    dhFeedEvtTriggMod.ModifyDate = Convert.ToDateTime(dsFeedDetails.Tables[0].Rows[0]["ModifyDate"].ToString());
                }
                foreach (DataRow dr in dsFeedDetails.Tables[5].Rows)
                {

                    DHFeedEventDep dhReportDep = new DHFeedEventDep();
                    dhReportDep.EventId = Convert.ToInt16(dr["EventId"].ToString());
                    dhReportDep.EventName = dr["EventName"].ToString();
                    dhReportDep.FeedId = Convert.ToInt32(dr["FeedId"].ToString());
                    dhReportDep.CreatedBy = Convert.ToInt16(dr["CreatedBy"].ToString());
                    dhReportDep.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                    dhReportDep.ModifiedByName = dr["ModifiedByName"].ToString();
                    dhFeedEvtTriggMod.lstDHFeedEventDep.Add(dhReportDep);
                }

                foreach (DataRow dr in dsFeedDetails.Tables[6].Rows)
                {
                    DHFeedTriggerDep dhTriggerDep = new DHFeedTriggerDep();
                    dhTriggerDep.TriggerId = Convert.ToInt16(dr["TriggerId"].ToString());
                    dhTriggerDep.TriggerName = dr["TriggerName"].ToString();
                    dhTriggerDep.FeedId = Convert.ToInt32(dr["FeedId"].ToString());
                    dhTriggerDep.CreatedBy = Convert.ToInt16(dr["CreatedBy"].ToString());
                    dhTriggerDep.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                    dhTriggerDep.ModifiedByName = dr["ModifiedByName"].ToString();
                    dhFeedEvtTriggMod.lstDHFeedTriggerDep.Add(dhTriggerDep);
                }

            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return dhFeedEvtTriggMod;
        }


        public int SaveEdit(DHFeedEvtTriggModel dhFeedEvtTriggModel, string User_Id)
        {
            int iErrorCode = 0;

            try
            {
                DataSet dsSaveEdit = new DataSet();

                DHFeedDal ObjDL = new DHFeedDal();


                #region for save Event
                DataTable dtDHFeedEvtList = new DataTable("tbl_DHFeedEventDep");
                dtDHFeedEvtList.Columns.Add("FeedId", typeof(System.Int32));
                dtDHFeedEvtList.Columns.Add("EventId", typeof(System.Int16));
                dtDHFeedEvtList.Columns.Add("CreatedBy", typeof(System.Int16));

                foreach (DHFeedEventDep DHFeedDep in dhFeedEvtTriggModel.lstDHFeedEventDep)
                {
                    DataRow drDHFeedEvtList = dtDHFeedEvtList.NewRow();

                    drDHFeedEvtList["EventId"] = DHFeedDep.EventId;
                    drDHFeedEvtList["FeedId"] = dhFeedEvtTriggModel.FeedId;
                    drDHFeedEvtList["CreatedBy"] = DHFeedDep.CreatedBy;
                    dtDHFeedEvtList.Rows.Add(drDHFeedEvtList);
                }
                dsSaveEdit.Tables.Add(dtDHFeedEvtList);
                #endregion for save Event

                #region for save Trigger
                DataTable dtDHTriggerList = new DataTable("tbl_RDSReportTriggerDep");
                dtDHTriggerList.Columns.Add("FeedId", typeof(System.Int32));
                dtDHTriggerList.Columns.Add("TriggerId", typeof(System.Int16));
                dtDHTriggerList.Columns.Add("CreatedBy", typeof(System.Int16));

                foreach (DHFeedTriggerDep DHTriggerDep in dhFeedEvtTriggModel.lstDHFeedTriggerDep)
                {
                    DataRow drDHTriggerList = dtDHTriggerList.NewRow();

                    drDHTriggerList["FeedId"] = dhFeedEvtTriggModel.FeedId;
                    drDHTriggerList["TriggerId"] = DHTriggerDep.TriggerId;
                    drDHTriggerList["CreatedBy"] = DHTriggerDep.CreatedBy;
                    dtDHTriggerList.Rows.Add(drDHTriggerList);
                }
                dsSaveEdit.Tables.Add(dtDHTriggerList);
                #endregion for save Trigger

                iErrorCode = ObjDL.SaveEdit(dsSaveEdit, User_Id);

            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }

            return iErrorCode;

        }

        public List<DHFeedEvent> GetEventList()
        {

            List<DHFeedEvent> lstEvent = new List<DHFeedEvent>();
            try
            {

                RDSEventDAL objEvent = new RDSEventDAL();
                DataSet dsEventList = objEvent.GetRDSEventList();
                if (dsEventList != null)
                {
                    if (dsEventList.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsEventList.Tables[0].Rows)
                        {
                            DHFeedEvent eventobj = new DHFeedEvent();
                            eventobj.EventId = Convert.ToInt16(dr["EventId"].ToString());
                            eventobj.EventName = dr["EventName"].ToString();
                            lstEvent.Add(eventobj);

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }

            return lstEvent;
        }

        public List<DHFeedTrigger> GetTriggertList()
        {
            List<DHFeedTrigger> lstTrigger = new List<DHFeedTrigger>();
            try
            {

                RDSReportDAL objDAL = new RDSReportDAL();
                DataSet dsTrigger = objDAL.GetTriggertList();
                if (dsTrigger != null)
                {
                    if (dsTrigger.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsTrigger.Tables[0].Rows)
                        {
                            DHFeedTrigger objTrigger = new DHFeedTrigger();
                            objTrigger.TriggerId = Convert.ToInt16(dr["TriggerId"].ToString());
                            objTrigger.TriggerName = dr["TriggerName"].ToString();
                            lstTrigger.Add(objTrigger);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return lstTrigger;
        }

        #endregion 

        public DataSet GetFTPDetail(int FeedId)
        {

            DataSet ds = new DataSet();
            try
            {
                DHFeedDal dataHubDALObj = new DHFeedDal();
                ds = dataHubDALObj.GetFTPDetail(FeedId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveUploadData(DHAchocRun dhAdhocModel, string @User_Id)
        {
            int Result = 0;
            try
            {
                DataSet DSUploadData = new DataSet();

                DataTable DtDHJob = new DataTable("tbl_DHJob");
                DtDHJob.Columns.Add("FeedId", typeof(System.Int32));
                DtDHJob.Columns.Add("LoaderId", typeof(System.Int16));
                DtDHJob.Columns.Add("StatusId", typeof(System.Int16));

                DataRow dr = DtDHJob.NewRow();
                dr["FeedId"] = dhAdhocModel.FeedId;
                dr["LoaderId"] = dhAdhocModel.LoaderId;
                dr["StatusId"] = dhAdhocModel.StatusId;
                DtDHJob.Rows.Add(dr);
                DSUploadData.Tables.Add(DtDHJob);


                DataTable DHAdhocUpload = new DataTable("tbl_DHAdhocUpload");
                DHAdhocUpload.Columns.Add("FeedId", typeof(System.Int32));
                DHAdhocUpload.Columns.Add("FileName", typeof(System.String));
                DHAdhocUpload.Columns.Add("CreatedBy", typeof(System.Int32));

                DataRow drAdhoc = DHAdhocUpload.NewRow();
                drAdhoc["FeedId"] = dhAdhocModel.FeedId;
                drAdhoc["FileName"] = dhAdhocModel.FileName;
                drAdhoc["CreatedBy"] = dhAdhocModel.CreatedBy;

                DHAdhocUpload.Rows.Add(drAdhoc);
                DSUploadData.Tables.Add(DHAdhocUpload);

                DHFeedDal dataHubDALObj = new DHFeedDal();
                Result = dataHubDALObj.SaveUploadData(DSUploadData, @User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Result;

        }
        public List<DHAchocRun> GetDHAdhocRunList()
        {
            List<DHAchocRun> lstDHAchoc = new List<DHAchocRun>();
            DataSet dsObj = new DataSet();
            try
            {
                DHFeedDal dataHubDALObj = new DHFeedDal();
                dsObj = dataHubDALObj.GetDHAdhocRunList();
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            DHAchocRun dhAdhoModel = new DHAchocRun();
                            dhAdhoModel.AdhocId = Convert.ToInt32(dr["AdhocId"].ToString());
                            dhAdhoModel.FeedName = dr["FeedName"].ToString();
                            dhAdhoModel.FileName = dr["FileName"].ToString();
                            dhAdhoModel.JobId = Convert.ToInt32(dr["JobId"]);
                            if (dr["LoaderId"].ToString() != "")
                            {
                                dhAdhoModel.LoaderId = Convert.ToInt16(dr["LoaderId"]);
                                dhAdhoModel.LoaderType = dr["LoaderType"].ToString();
                            }
                            dhAdhoModel.StatusName = dr["StatusShortCode"].ToString();
                            dhAdhoModel.CreatedByName = dr["CreatedBy"].ToString();
                            lstDHAchoc.Add(dhAdhoModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstDHAchoc;

        }

        #region Review
        #region Inventory Ageing Analysis
        public List<InventoryAgeingAnalysisModel> GetInventoryAgeingAnalysisList(string CompCode, DateTime AsOfDate,string User_Id)
        {
            List<InventoryAgeingAnalysisModel> List = new List<InventoryAgeingAnalysisModel>();
            DataSet Ds = new DataSet();
            try
            {
                Ds = ObjDal.GetInventoryAgeingAnalysisList(CompCode, AsOfDate, User_Id);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            InventoryAgeingAnalysisModel Model = new InventoryAgeingAnalysisModel();
                            Model.ItemNo = Dr["ItemNo"].ToString();
                            Model.Description = Dr["Description"].ToString();
                            Model.make = Dr["make"].ToString();
                            Model.UnitCost = Convert.ToDouble(Dr["UnitCost"].ToString());                           
                            Model.ZerotoThirty = Convert.ToDouble(Dr["ZerotoThirty"].ToString());
                            Model.ZerotoThirtyValue= Convert.ToDouble(Dr["ZerotoThirtyValue"].ToString());
                            Model.ThirtytoSixty = Convert.ToDouble(Dr["ThirtytoSixty"].ToString());
                            Model.ThirtytoSixtyValue = Convert.ToDouble(Dr["ThirtytoSixtyValue"].ToString());
                            Model.SixtyToNintyValue= Convert.ToDouble(Dr["SixtyToNintyValue"].ToString());
                            Model.SixtyToNinty = Convert.ToDouble(Dr["SixtyToNinty"].ToString());
                            Model.NintyPlus = Convert.ToDouble(Dr["NintyPlus"].ToString());
                            Model.NintyPlusValue = Convert.ToDouble(Dr["NintyPlusValue"].ToString());
                            Model.TotalQty= Convert.ToDouble(Dr["TotalQty"].ToString());
                            Model.TotalValue= Convert.ToDouble(Dr["TotalValue"].ToString());
                            Model.Status_OB = Dr["Status_OB"].ToString();
                            Model.Customer = Dr["Customer"].ToString();
                            Model.SONO = Dr["SONO"].ToString();
                            Model.Quantity = Convert.ToDouble(Dr["Quantity"].ToString());
                            Model.OutstandingQuantity = Convert.ToDouble(Dr["OutstandingQuantity"].ToString());
                            Model.Comment= Dr["Comment"].ToString();
                            Model.FullName= Dr["FullName"].ToString();
                            List.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return List;
        }
        public int SaveInvAgingAdvent(string ItemNo, string Description, string Comment, string BunchId, string User_Id)
        {
            int ErroCode = 0;
            try
            {
                ErroCode = ObjDal.SaveInvAgingAdvent(ItemNo, Description, Comment, BunchId, User_Id);
                return ErroCode;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
           
        }
        public List<InventoryAgeingAnalysisModel> ShowInvAgingAdvent(string CompCode, String ItemNo)
        {
            List<InventoryAgeingAnalysisModel> List = new List<InventoryAgeingAnalysisModel>();
            DataSet Ds = new DataSet();
            try
            {
                Ds = ObjDal.ShowInvAgingAdvent(CompCode, ItemNo);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            InventoryAgeingAnalysisModel Model = new InventoryAgeingAnalysisModel();
                            Model.ItemNo = Dr["Item No_"].ToString();
                            Model.Description = Dr["PartNo"].ToString();
                            Model.Comment = Dr["Comment"].ToString();
                            if (Dr["CreateDate"].ToString() != null && Dr["CreateDate"].ToString() != "")
                            {
                                Model.Date = Dr["CreateDate"].ToString();
                                Model.Createdate = Convert.ToDateTime(Model.Date).ToString("dd/MM/yyyy hh:mm tt");
                                Model.CreateDate = Convert.ToDateTime(Dr["CreateDate"].ToString());
                            }
                            Model.FullName = Dr["FullName"].ToString();
                            List.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return List;
        }
        #endregion Inventory Ageing Analysis
        #region Inventory
        public List<InventoryModel> GetInventoryList(string CompCode, DateTime AsOfDate,string User_Id)
        {
            List<InventoryModel> List = new List<InventoryModel>();
            DataSet Ds = new DataSet();
            try
            {
                Ds = ObjDal.GetInventoryList(CompCode, AsOfDate, User_Id);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            InventoryModel Model = new InventoryModel();
                            Model.ItemNo = Dr["Item No_"].ToString();
                            Model.Description = Dr["Description"].ToString();
                            Model.make = Dr["make"].ToString();
                            Model.UnitCost = Convert.ToDouble(Dr["Unit_Cost"].ToString());
                            Model.Unit = Dr["Unit"].ToString();
                            Model.Remaining_Quantity= Convert.ToDouble(Dr["Remaining_Quantity"].ToString());
                            Model.Value = Convert.ToDouble(Dr["Value"].ToString());
                            Model.Status_OB = Dr["Status_OB"].ToString();
                            Model.Customer = Dr["Customer"].ToString();
                            Model.SONO = Dr["SONO"].ToString();
                            if(Dr["Quantity"].ToString()!="")
                            Model.Quantity= Convert.ToDouble(Dr["Quantity"].ToString());
                            if (Dr["OutstandingQuantity"].ToString() != "")
                                Model.OutstandingQuantity= Convert.ToDouble(Dr["OutstandingQuantity"].ToString());
                            Model.Comment = Dr["Comment"].ToString();
                            Model.FullName = Dr["FullName"].ToString();
                            List.Add(Model);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return List;
        }
        public int SaveInventoryAdventData(string ItemNo, string Description, string Comment, string BunchId, string User_Id)
        { 
            int ErroCode = 0;
            try
            {
                ErroCode = ObjDal.SaveInventoryAdventData(ItemNo, Description, Comment, BunchId, User_Id);
                return ErroCode;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        public List<InventoryModel> ShowInventoryAdvent(string CompCode, string ItemNo)
        {
            List<InventoryModel> List = new List<InventoryModel>();
            DataSet Ds = new DataSet();
            try
            {
                Ds = ObjDal.GetInventoryDatetoShow(CompCode, ItemNo);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            InventoryModel Model = new InventoryModel();
                            Model.ItemNo = Dr["Item No_"].ToString();
                            Model.Description = Dr["Description"].ToString();
                            Model.Comment = Dr["Comment"].ToString();
                            if (Dr["CreateDate"].ToString() != null && Dr["CreateDate"].ToString() != "")
                            {
                                Model.Date = Dr["CreateDate"].ToString();
                                Model.Createdate = Convert.ToDateTime(Model.Date).ToString("dd/MM/yyyy hh:mm tt");
                                Model.CreateDate = Convert.ToDateTime(Dr["CreateDate"].ToString());
                            }
                            Model.FullName = Dr["FullName"].ToString();
                            List.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return List;
        }
        #endregion Inventory
        #region Material Procured Vs Reduction
        public List<MaterialProcuredVsReductionModel> GetMaterialProcuredVsReductionList(string CompCode, DateTime FromDate, DateTime ToDate,string User_Id)
        {
            List<MaterialProcuredVsReductionModel> List = new List<MaterialProcuredVsReductionModel>();
            DataSet Ds = new DataSet();
            try
            {
                Ds = ObjDal.GetMaterialProcuredVsReductionList(CompCode, FromDate, ToDate, User_Id);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            MaterialProcuredVsReductionModel Model = new MaterialProcuredVsReductionModel();
                            Model.DocumentNo = Dr["DocumentNo"].ToString();
                            Model.VendorName = Dr["VendorName"].ToString();
                            Model.VendorNo = Dr["VendorNo"].ToString();
                            Model.ItemName = Dr["ItemName"].ToString();
                            Model.Make = Dr["Make"].ToString();
                            Model.Quantity = Convert.ToDouble(Dr["Quantity"].ToString());
                            Model.CurrentPurchaseRate = Convert.ToDouble(Dr["CurrentPurchaseRate"].ToString());
                            Model.LastPurchaseRate = Convert.ToDouble(Dr["LastPurchaseRate"].ToString());
                            Model.Comment = Dr["Comment"].ToString();
                            Model.FullName = Dr["FullName"].ToString();
                            List.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return List;
        }
        public List<MaterialProcuredVsReductionModel> ShowMaterialProcuredVsReduction(string CompCode, DateTime FromDate, DateTime ToDate, string DocumentNo, string ItemName)
        {
            List<MaterialProcuredVsReductionModel> List = new List<MaterialProcuredVsReductionModel>();
            DataSet Ds = new DataSet();
            try
            {
                Ds = ObjDal.ShowMaterialProcuredVsReduction(CompCode, FromDate, ToDate, DocumentNo, ItemName);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            MaterialProcuredVsReductionModel Model = new MaterialProcuredVsReductionModel();
                            Model.DocumentNo = Dr["DocumentNo"].ToString();
                            Model.VendorName = Dr["VendorName"].ToString();
                           // Model.VendorNo = Dr["CreateDate"].ToString();
                            //Model.VendorName = Convert.ToDateTime(Model.VendorNo).ToString("dd/MM/yyyy");
                            Model.ItemName = Dr["ItemName"].ToString();                                                       
                            Model.Comment = Dr["Comment"].ToString();
                            if(Dr["CreateDate"].ToString()!=null && Dr["CreateDate"].ToString() != "")
                            {
                                Model.Date = Dr["CreateDate"].ToString();
                                Model.Createdate = Convert.ToDateTime(Model.Date).ToString("dd/MM/yyyy hh:mm tt");
                                Model.CreateDate = Convert.ToDateTime(Dr["CreateDate"].ToString());
                            }
                            Model.FullName = Dr["FullName"].ToString();
                            List.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return List;
        }
        public int SaveMPRAdvent(string ItemName, string DocumentNo, string Comment, string BunchId, string User_Id)
        {
            int ErroCode = 0;
            ErroCode = ObjDal.SaveMPRAdvent(ItemName, DocumentNo, Comment, BunchId, User_Id);
            return ErroCode;
        }
        #endregion Material Procured Vs Reduction
        #region NNR 
        public List<NNRModel> GetNNRList(string CompCode, DateTime FromDate, DateTime ToDate,string User_Id)
        {
            List<NNRModel> List = new List<NNRModel>();
            DataSet Ds = new DataSet();
            try
            {
                Ds = ObjDal.GetNNRList(CompCode, FromDate, ToDate, User_Id);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            NNRModel Model = new NNRModel();
                            Model.DocumentNo = Dr["Document No_"].ToString();
                            Model.ItemNo = Dr["No_"].ToString();
                            Model.NNR= Dr["NNR"].ToString();
                            Model.Description= Dr["Description"].ToString();
                            //Model.PostingDate=Convert.ToDateTime(Dr["Posting Date"].ToString());
                            Model.PoDate = Dr["Posting Date"].ToString();
                            Model.PoDate = Convert.ToDateTime(Model.PoDate).ToString("dd/MM/yyyy");
                            Model.MPN= Dr["MPN"].ToString();
                            Model.Branch= Dr["Branch"].ToString();
                            Model.Make = Dr["Make"].ToString();
                            Model.SONO= Dr["SO_No"].ToString(); 
                            Model.CustomerOrderNo = Dr["Customer Order No_"].ToString();
                            Model.CustomerName= Dr["Sell-to Customer Name"].ToString(); 
                            Model.SalesPersonName= Dr["Sales_person_Name"].ToString();
                            Model.Quantity = Convert.ToDouble(Dr["Quantity"].ToString());
                            Model.ExtnResale = Convert.ToDouble(Dr["Extn_Resale"].ToString());
                            Model.UnitResale = Convert.ToDouble(Dr["Unit_Resale"].ToString());
                            Model.UnitCost = Convert.ToDouble(Dr["Unit_Cost"].ToString());
                            Model.BasicUnit = Convert.ToDouble(Dr["Basic_Unit"].ToString());
                            Model.Comment= Dr["Comment"].ToString();
                            Model.FullName= Dr["FullName"].ToString();
                            List.Add(Model);

                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return List;
        }
        public List<NNRModel> ShowNNRList(string DocumentNo, string ItemNo, string CompCode, DateTime FromDate, DateTime ToDate)
        {
            List<NNRModel> List = new List<NNRModel>();
            DataSet Ds = new DataSet();
            try
            {
                Ds = ObjDal.ShowNNRListAdvent(DocumentNo, ItemNo, CompCode, FromDate, ToDate);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            NNRModel Model = new NNRModel();
                            Model.ItemNo = Dr["Document No_"].ToString();
                            Model.MPN = Dr["MPN"].ToString();
                            Model.Comment = Dr["Comment"].ToString();
                            if (Dr["CreateDate"].ToString() != null && Dr["CreateDate"].ToString() != "")
                            {
                                Model.Date = Dr["CreateDate"].ToString();
                                Model.Createdate = Convert.ToDateTime(Model.Date).ToString("dd/MM/yyyy hh:mm tt");
                                Model.CreateDate = Convert.ToDateTime(Dr["CreateDate"].ToString());
                            }
                            Model.FullName = Dr["FullName"].ToString();
                            List.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return List;
        }
        public int SaveNNRAdvent(string DocumentNo, string MPN, string Comment, string ItemNo,  string BunchId, string User_Id)
        {            
            int ErroCode = 0;
            try
            {
                ErroCode = ObjDal.SaveNNRAdventData(DocumentNo,  MPN, Comment, ItemNo, BunchId, User_Id);
                return ErroCode;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        #endregion NNR
        #region Purchase Report Monthwise
        public List<PurchaseMonthwiseModel> GetPurchaseMonthwiseList(string CompCode, DateTime FromDate, DateTime ToDate,string User_Id)
        {
            List<PurchaseMonthwiseModel> List = new List<PurchaseMonthwiseModel>();
            DataSet Ds = new DataSet();
            try
            {
                Ds = ObjDal.GetPurchaseMonthwiseList(CompCode, FromDate, ToDate, User_Id);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            PurchaseMonthwiseModel Model = new BL.PurchaseMonthwiseModel();
                            Model.No = Dr["No_"].ToString();
                            Model.LineNo = Dr["Line No_"].ToString();
                            Model.Date = Dr["Posting Date"].ToString();
                            Model.PostingDate = Convert.ToDateTime(Model.Date).ToString("dd/MM/yyyy");
                          //  Model.PostingDate = Convert.ToDateTime(Dr["Posting Date"].ToString());
                            Model.DocumentNo = Dr["Document No_"].ToString();
                            Model.PayToName = Dr["Pay-to Name"].ToString();
                            Model.Description = Dr["Description"].ToString();
                            Model.Make = Dr["Shortcut Dimension 2 Code"].ToString();
                            Model.BaseUnitPrice = Convert.ToDouble(Dr["Base Unit Price"].ToString());
                            Model.Quantity = Convert.ToDouble(Dr["Quantity"].ToString());
                            Model.SONO =Dr["SONO"].ToString();
                            Model.Name = Dr["Name"].ToString();
                            Model.SalesQty = Convert.ToDouble(Dr["SalesQty"].ToString());
                            Model.OutstandingQuantity = Convert.ToDouble(Dr["OutQty"].ToString());
                            Model.Total_Value = Convert.ToDouble(Dr["Total_Value"].ToString());
                            Model.Comment = Dr["Comment"].ToString();
                            Model.FullName = Dr["FullName"].ToString();
                            List.Add(Model);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return List;
        }
        public List<PurchaseMonthwiseModel> ShowPurchaseMonthwiseList(string DocumentNo, string ItemNo,int LineNo, string CompCode, DateTime FromDate, DateTime ToDate)
        {
            List<PurchaseMonthwiseModel> List = new List<PurchaseMonthwiseModel>();
            DataSet Ds = new DataSet();
            try
            {
                Ds = ObjDal.ShowPurchaseMonthwiseAdvent(DocumentNo, ItemNo, LineNo, CompCode, FromDate, ToDate);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            PurchaseMonthwiseModel Model = new PurchaseMonthwiseModel();
                            Model.DocumentNo = Dr["Document No_"].ToString();
                            Model.ItemNo = Dr["No_"].ToString();
                            Model.MPN = Dr["Description"].ToString();
                            Model.Comment = Dr["Comment"].ToString();
                            if (Dr["CreateDate"].ToString() != null && Dr["CreateDate"].ToString() != "")
                            {
                                Model.Date = Dr["CreateDate"].ToString();
                                Model.Createdate = Convert.ToDateTime(Model.Date).ToString("dd/MM/yyyy hh:mm tt");
                                Model.CreateDate = Convert.ToDateTime(Dr["CreateDate"].ToString());
                            }
                            Model.FullName = Dr["FullName"].ToString();
                            List.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return List;
        }
        public int SavePurchaseMonthwiseData(string DocumentNo, string Comment, string ItemNo,string LineNo, string BunchId, string User_Id)
        {
            int ErroCode = 0;
            try
            {
                ErroCode = ObjDal.SavePurchMonthwiseAdventData(DocumentNo,Comment, ItemNo, LineNo, BunchId, User_Id);
                return ErroCode;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
        #endregion Purchase Report Monthwise
        #region Pending PO
        public List<PendingPOModel> PendingPOList(string CompCode,string User_Id)
        {
            List<PendingPOModel> List = new List<BL.PendingPOModel>();
            try
            {
                DataSet Ds = new DataSet();
                Ds = ObjDal.GetPendingPOList(CompCode,User_Id);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            PendingPOModel Model = new BL.PendingPOModel();
                            Model.DocumentNo = Dr["DocumentNo"].ToString();
                            Model.LineNo = Convert.ToInt32(Dr["LineNum"].ToString());                           
                            Model.OrderDate = Dr["OrderDate"].ToString();
                            Model.OrderDate = Convert.ToDateTime(Model.OrderDate).ToString("dd/MM/yyyy");
                            Model.VendNo= Dr["VendNo"].ToString();
                            Model.Vendname= Dr["Vendname"].ToString();
                            Model.ItemNo= Dr["ItemNo"].ToString();
                            Model.Description= Dr["Description"].ToString();
                            Model.Quantity=Convert.ToDouble(Dr["Quantity"].ToString());
                            Model.QuantityReceived= Convert.ToDouble(Dr["QuantityReceived"].ToString());
                            Model.QuantityInvoiced= Convert.ToDouble(Dr["QuantityInvoiced"].ToString());
                            Model.Balance= Convert.ToDouble(Dr["Balance"].ToString());
                            Model.DirectUnitCost= Convert.ToDouble(Dr["DirectUnitCost"].ToString());
                            Model.Amount= Convert.ToDouble(Dr["Amount"].ToString());
                            Model.Comment = Dr["Comment"].ToString();
                            Model.FullName = Dr["FullName"].ToString();
                            List.Add(Model);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return List;
        }
        public List<PendingPOModel> ShowPendPOList(string DocumentNo, string ItemNo, int LineNo, string CompCode)
        {
            List<PendingPOModel> List = new List<PendingPOModel>();
            DataSet Ds = new DataSet();
            try
            {
                Ds = ObjDal.ShowPendPOListAdvent(DocumentNo, ItemNo, LineNo,CompCode);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            PendingPOModel Model = new PendingPOModel();
                            Model.DocumentNo = Dr["Doc_No"].ToString();
                            Model.Description = Dr["Description"].ToString();
                            Model.Comment = Dr["Comment"].ToString();
                            if (Dr["CreateDate"].ToString() != null && Dr["CreateDate"].ToString() != "")
                            {
                                Model.Date = Dr["CreateDate"].ToString();
                                Model.Createdate = Convert.ToDateTime(Model.Date).ToString("dd/MM/yyyy hh:mm tt");
                                Model.CreateDate = Convert.ToDateTime(Dr["CreateDate"].ToString());
                            }
                            Model.FullName = Dr["FullName"].ToString();
                            List.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return List;
        }
        public int SavePendPOAdvent(string DocumentNo, string ItemNo, string LineNo, string Comment, string BunchId, string User_Id)
        {
            int ErroCode = 0;
            try
            {
                ErroCode = ObjDal.SavePendPOAdventData(DocumentNo, ItemNo, LineNo, Comment, BunchId, User_Id);
                return ErroCode;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
        #endregion Pending PO
        #region Franchise 
        public List<FranchiseModel> GetFranchiseList(string CompCode, DateTime FromDate, DateTime ToDate,string User_Id)
        {
            List<FranchiseModel> List = new List<FranchiseModel>();
            DataSet Ds = new DataSet();
            try
            {
                Ds = ObjDal.GetFranchiseList(CompCode, FromDate, ToDate, User_Id);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            FranchiseModel Model = new FranchiseModel();
                            Model.VendorNo = Dr["VendorNo"].ToString();
                            Model.VendorName = Dr["VendorName"].ToString();
                            Model.CurrencyCode = Dr["CurrencyCode"].ToString();
                            Model.TargetForYear = Convert.ToDouble(Dr["TargetForYear"].ToString());
                            Model.Achived_Till= Convert.ToDouble(Dr["Achived_Till"].ToString());
                            Model.BacklogTill= Convert.ToDouble(Dr["BacklogTill"].ToString());
                            Model.Pending_to_be_achieve= Convert.ToDouble(Dr["Pending_to_be_achieve"].ToString());
                            Model.Comment= Dr["Comment"].ToString();
                            Model.FullName = Dr["FullName"].ToString();
                            List.Add(Model);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return List;
        }
        public List<FranchiseModel> ShowFranchiseList(string VendorNo, string CompCode, DateTime FromDate, DateTime ToDate)
        {
            List<FranchiseModel> List = new List<FranchiseModel>();
            DataSet Ds = new DataSet();
            try
            {
                Ds = ObjDal.ShowFranchiseListAdvent(VendorNo, CompCode, FromDate, ToDate);
                if (Ds == null)
                    return null;
                if (Ds.Tables.Count > 0)
                {
                    if (Ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow Dr in Ds.Tables[0].Rows)
                        {
                            FranchiseModel Model = new FranchiseModel();
                            Model.VendorNo = Dr["VendorNo"].ToString();
                            Model.VendorName = Dr["VendName"].ToString();
                            Model.Comment = Dr["Comment"].ToString();
                            if (Dr["CreateDate"].ToString() != null && Dr["CreateDate"].ToString() != "")
                            {
                                Model.Date = Dr["CreateDate"].ToString();
                                Model.Createdate = Convert.ToDateTime(Model.Date).ToString("dd/MM/yyyy hh:mm tt");
                                Model.CreateDate = Convert.ToDateTime(Dr["CreateDate"].ToString());
                            }
                            Model.FullName = Dr["FullName"].ToString();
                            List.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return List;
        }
        public int SaveFranchiseAdvent(string VendorName, string VendorNo, string Comment, string BunchId, string User_Id)
        {
            int ErroCode = 0;
            try
            {
                ErroCode = ObjDal.SaveFranchiseAdventData(VendorName, VendorNo, Comment,BunchId, User_Id);               
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return ErroCode;
        }
        #endregion Franchise
        #endregion Review
    }
}
