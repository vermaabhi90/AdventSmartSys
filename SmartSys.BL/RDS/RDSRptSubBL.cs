using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSys.Utility;
using SmartSys.DAL;
using System.Data;
using SmartSys.DL.RDS;

namespace SmartSys.BL.RDS
{
    public class RDSRptSubBL
    {
        SmartSys.DL.RDS.RDSRptSubDAL rdsRptSubDALObj = null;
        public RDSRptSubModel GetReportClientListByUser(int iUserID)
        {
            RDSRptSubModel rdsRptSubModel = new RDSRptSubModel();
            try
            {
                rdsRptSubDALObj = new DL.RDS.RDSRptSubDAL();
                DataSet dsReportClientList =  new DataSet();
                dsReportClientList =   rdsRptSubDALObj.GetReportClientListByUser(iUserID);

                if (dsReportClientList == null)
                    return null;
                rdsRptSubModel.RDSReportList = new List<RDSReport>();
                foreach(DataRow dr in dsReportClientList.Tables[0].Rows)
                {
                    RDSReport rdsReport = new RDSReport();
                    rdsReport.ReportId = dr["ReportId"].ToString();
                    rdsReport.ReportName = dr["ReportName"].ToString();
                    rdsReport.ReportType = dr["ReportType"].ToString();
                    rdsReport.BusinessLineId = Convert.ToInt32(dr["BusinessLineId"]);
                    rdsReport.BusinessLineName = dr["BusinessLineName"].ToString();
                    rdsRptSubModel.RDSReportList.Add(rdsReport);
                }
                rdsRptSubModel.ClientList = new List<RDSClientForSub>();
                foreach(DataRow dr in dsReportClientList.Tables[1].Rows)
                {
                    RDSClientForSub rdsClientForSub = new RDSClientForSub();
                    rdsClientForSub.ClientId = Convert.ToInt32(dr["ClientId"]);
                    rdsClientForSub.ClientName = dr["ClientName"].ToString();
                    rdsClientForSub.ClientRefId = dr["ClientRefId"].ToString();
                    rdsClientForSub.ClientType = dr["ClientType"].ToString();
                    rdsClientForSub.email = dr["email"].ToString();
                    rdsClientForSub.FTPDetails = dr["FTPDetails"].ToString();
                    rdsRptSubModel.ClientList.Add(rdsClientForSub);
                }

            }
            catch(Exception ex)
            {
                Common.LogException(ex);
            }
            return rdsRptSubModel;
        }

        public string SaveRptSubDetails(SmartSys.BL.RDS.RDSRptSubModel rdsRptSubModel)
        {
            string strResult = "";
            try
            {
                DataSet dsRptSubDetails = new DataSet();

                DataTable dtRdsRptSubscription = new DataTable("tbl_RDSRptSubscription");
                dtRdsRptSubscription.Columns.Add("RptSubId", typeof(System.Int32));
                dtRdsRptSubscription.Columns.Add("ClientId", typeof(System.Int32));
                dtRdsRptSubscription.Columns.Add("ReportId", typeof(System.String));
                dtRdsRptSubscription.Columns.Add("CreatedBy", typeof(System.Int32));
                dtRdsRptSubscription.Columns.Add("CreatedDate", typeof(System.DateTime));
                dtRdsRptSubscription.Columns.Add("ModifiedBy", typeof(System.Int32));
                dtRdsRptSubscription.Columns.Add("ModifiedDate", typeof(System.DateTime));

                foreach(RDSReport rdsRpt   in  rdsRptSubModel.RDSReportList)
                {
                    foreach(RDSRptGenSubscription rdsRptGenSub in rdsRptSubModel.RDSRptGenSubscription)
                    {
                        DataRow drRdsRptsub = dtRdsRptSubscription.NewRow();
                        drRdsRptsub["RptSubId"] = 0;
                        drRdsRptsub["ClientId"] = rdsRptGenSub.SelectedClient.ClientId;
                        drRdsRptsub["ReportId"] = rdsRpt.ReportId;
                        drRdsRptsub["CreatedBy"] = rdsRptSubModel.UserID;
                        drRdsRptsub["ModifiedBy"] = rdsRptSubModel.UserID;
                        dtRdsRptSubscription.Rows.Add(drRdsRptsub);
                    }
                }

                dsRptSubDetails.Tables.Add(dtRdsRptSubscription);

                DataTable dtRdsRptSubParam = new DataTable("tbl_RDSRptSubParamOverride");
                dtRdsRptSubParam.Columns.Add("RptSubId", typeof(System.Int32));
                dtRdsRptSubParam.Columns.Add("ReportId", typeof(System.String));
                dtRdsRptSubParam.Columns.Add("ParamName", typeof(System.String));
                dtRdsRptSubParam.Columns.Add("DefaultValue", typeof(System.String));

                //foreach (DataRow dr in dsRptSubDetails.Tables["tbl_RDSRptSubscription"].Rows)
                {
                    foreach(RDSReportParam rdsRptParam in rdsRptSubModel.RDSRptParamList)
                    {
                        DataRow drRdsParam = dtRdsRptSubParam.NewRow();
                        drRdsParam["RptSubId"] = 0;
                        drRdsParam["ReportId"] = rdsRptParam.ReportId;
                        drRdsParam["ParamName"] = rdsRptParam.ParamName;
                        drRdsParam["DefaultValue"] = rdsRptParam.DefaultValue;
                        dtRdsRptSubParam.Rows.Add(drRdsParam);
                    }
                }
                dsRptSubDetails.Tables.Add(dtRdsRptSubParam);


                // Create table and rows for RDSRptSubGenDetails
                string strFreq = "";
                int iVar = Convert.ToInt32(rdsRptSubModel.RptSubGenrationDetails.Frequency);
                if (iVar ==  Convert.ToInt32(SmartSys.Utility.Enums.GenerationOpt.Dly))
                {
                    strFreq = "Dly";
                }
                else if(iVar ==  Convert.ToInt32(SmartSys.Utility.Enums.GenerationOpt.Hfly))
                {
                    strFreq = "Hfly";
                }
                else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.GenerationOpt.Mnly))
                {
                    strFreq = "Mnly";
                }
                else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.GenerationOpt.Qtly))
                {
                    strFreq = "Qtly";
                }
                else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.GenerationOpt.Wkly))
                {
                    strFreq = "Wkly";
                }
                else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.GenerationOpt.Yrly))
                {
                    strFreq = "Yrly";
                }

                DataTable dtRdsRptSubGen = new DataTable("tbl_RDSRptSubGenDetails");
                dtRdsRptSubGen.Columns.Add("RptSubId", typeof(System.Int32));
                dtRdsRptSubGen.Columns.Add("Frequency", typeof(System.String));
                dtRdsRptSubGen.Columns.Add("WeeklyOptions", typeof(System.String));
                dtRdsRptSubGen.Columns.Add("StartTime", typeof(System.TimeSpan));
                dtRdsRptSubGen.Columns.Add("EndTime", typeof(System.TimeSpan));
                dtRdsRptSubGen.Columns.Add("Interval", typeof(System.Int32));

                //foreach(DataRow dr in dsRptSubDetails.Tables["tbl_RDSRptSubscription"].Rows)
                {
                    DataRow drRdsRptSubGen = dtRdsRptSubGen.NewRow();
                    drRdsRptSubGen["RptSubId"] = 0;
                    drRdsRptSubGen["Frequency"] = strFreq;
                    drRdsRptSubGen["WeeklyOptions"] = rdsRptSubModel.RptSubGenrationDetails.WeeklyOptions;
                    drRdsRptSubGen["StartTime"] = rdsRptSubModel.RptSubGenrationDetails.StartTime;
                    drRdsRptSubGen["EndTime"] = rdsRptSubModel.RptSubGenrationDetails.EndTime;
                    drRdsRptSubGen["Interval"] = rdsRptSubModel.RptSubGenrationDetails.Interval;

                    dtRdsRptSubGen.Rows.Add(drRdsRptSubGen);
                }

                dsRptSubDetails.Tables.Add(dtRdsRptSubGen);

               


                //Create table for RdsRptSubGenTime 
                DataTable dtRdsRptSubGenTime = new DataTable("tbl_RDSRptSubGenTimeDetails");
                dtRdsRptSubGenTime.Columns.Add("RptSubId", typeof(System.Int32));
                dtRdsRptSubGenTime.Columns.Add("GenTime", typeof(System.TimeSpan));
                //foreach(DataRow dr in dsRptSubDetails.Tables["tbl_RDSRptSubscription"].Rows)
                {
                    foreach(string GenTime in  rdsRptSubModel.RDSRptSubGenTimeList)
                    {
                        DataRow drRdsRptSubGenTime = dtRdsRptSubGenTime.NewRow();
                        drRdsRptSubGenTime["GenTime"] = GenTime;
                        drRdsRptSubGenTime["RptSubId"] = 0;
                        dtRdsRptSubGenTime.Rows.Add(drRdsRptSubGenTime);
                    }
                }
                dsRptSubDetails.Tables.Add(dtRdsRptSubGenTime);

                
                //Create table for RdsRptDistDetails
                DataTable dtRdsRptDistDetails = new DataTable("tbl_RDSRptSubDistDetails");
                dtRdsRptDistDetails.Columns.Add("RptSubId", typeof(System.Int32));
                dtRdsRptDistDetails.Columns.Add("DistType", typeof(System.String));
                dtRdsRptDistDetails.Columns.Add("DistTime", typeof(System.TimeSpan) );
                dtRdsRptDistDetails.Columns.Add("MaxDistTime", typeof(System.TimeSpan));
                dtRdsRptDistDetails.Columns.Add("DistMode", typeof(System.String));
                dtRdsRptDistDetails.Columns.Add("LocalFolder", typeof(System.String));
                
                string strDistType = "";
                iVar = Convert.ToInt32(rdsRptSubModel.RptSubDistributionDetails.DistributionType); 
                if(iVar == Convert.ToInt32(SmartSys.Utility.Enums.DistributionOpt.DAST))
                {
                    strDistType = "DAST";
                }
                else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.DistributionOpt.DWR))
                {
                    strDistType = "DWR";
                }
                else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.DistributionOpt.PIZWR))
                {
                    strDistType = "PIZWR";
                }
                else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.DistributionOpt.PIZADAST))
                {
                    strDistType = "PIZADAST";
                }
                string strDistMode = "";
                iVar =  Convert.ToInt32(rdsRptSubModel.RptSubDistributionDetails.DistributionMode);
                if(iVar == Convert.ToInt32(SmartSys.Utility.Enums.DistribtionMode.DistributeMannually))
                {
                    strDistMode = "Man";
                }
                else if(iVar == Convert.ToInt32(SmartSys.Utility.Enums.DistribtionMode.DistributeAuto))
                {
                    strDistMode = "Aut";
                }

                //foreach (DataRow dr in dsRptSubDetails.Tables["tbl_RDSRptSubscription"].Rows)
                {
                    DataRow drRdsRptDistDetails = dtRdsRptDistDetails.NewRow();
                    drRdsRptDistDetails["RptSubId"] = 0;
                    drRdsRptDistDetails["DistType"] = strDistType;
                    drRdsRptDistDetails["DistTime"] =rdsRptSubModel.RptSubDistributionDetails.DistributionTime;
                    drRdsRptDistDetails["MaxDistTime"] =rdsRptSubModel.RptSubDistributionDetails.MaxDistributionTime;
                    drRdsRptDistDetails["DistMode"] = strDistMode;
                    drRdsRptDistDetails["LocalFolder"] = rdsRptSubModel.RptSubDistributionDetails.LocalFolder;

                    dtRdsRptDistDetails.Rows.Add(drRdsRptDistDetails);
                }
                dsRptSubDetails.Tables.Add(dtRdsRptDistDetails);


                SmartSys.DL.RDS.RDSRptSubDAL rdsRptSbuDALObj = new DL.RDS.RDSRptSubDAL();
                int iErrorCode = rdsRptSbuDALObj.SaveRptSubDetails(dsRptSubDetails);
            }
            catch(Exception ex)
            {
                Common.LogException(ex);
            }
            return strResult;
        }

        public List<RDSReportParam> GetReportParam(string strReportID)
        {
            List<RDSReportParam> rdsReportParamList = new List<RDSReportParam>();
           try
           {
               rdsRptSubDALObj = new DL.RDS.RDSRptSubDAL();
               DataSet dsReportParamList = rdsRptSubDALObj.GetReportParam(strReportID);
               if (dsReportParamList == null)
                   return null;

               //rdsReport.RDSRptParamList = new List<RDSReportParam>();
               foreach(DataRow dr in dsReportParamList.Tables[0].Rows)
               {
                   RDSReportParam rDSReportParam = new RDSReportParam();
                   rDSReportParam.ReportId = strReportID;
                   rDSReportParam.ParamName = dr["ParamName"].ToString();
                   rDSReportParam.DefaultValue = dr["DefaultValue"].ToString();
                   rDSReportParam.DataType = dr["DataType"].ToString();
                   rdsReportParamList.Add(rDSReportParam);
               }
               
           }
          catch(Exception ex)
           {
               Common.LogException(ex);
           }
           return rdsReportParamList;
        }


        #region for Single Subscription to Gen 
        public List<RDSRptGenSubscription> SubscriptionList()
        {
            List<RDSRptGenSubscription> lstRptSubscripList = new List<RDSRptGenSubscription>();
            try
            {
                RDSRptSubDAL objBL = new RDSRptSubDAL();
                DataSet dsRptSubsCrip = new DataSet();
                dsRptSubsCrip = objBL.SubscriptionList();
                if(dsRptSubsCrip != null)
                {
                    if(dsRptSubsCrip.Tables[0].Rows.Count > 0)
                    {
                        foreach(DataRow dr in dsRptSubsCrip.Tables[0].Rows)
                        {
                            RDSRptGenSubscription rptSubcripList = new RDSRptGenSubscription();
                            rptSubcripList.RptSubId = Convert.ToInt32(dr["RptSubId"].ToString());
                            rptSubcripList.ClientId = Convert.ToInt32(dr["ClientId"].ToString());
                            rptSubcripList.ClientName = dr["ClientName"].ToString();
                            rptSubcripList.ReportId = dr["ReportId"].ToString();
                            rptSubcripList.ClientEmail = dr["ClientEmail"].ToString();
                            rptSubcripList.ReportName = dr["ReportName"].ToString();
                            rptSubcripList.CreatedByName = dr["CreatedByName"].ToString();
                            rptSubcripList.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            rptSubcripList.ModifiedByName = dr["ModifiedByName"].ToString();
                            rptSubcripList.ModifiedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            lstRptSubscripList.Add(rptSubcripList);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstRptSubscripList;
        }


        public int SaveSingleRptSubDetails(RDSRptSubModel rdsRptSubModel, int RptSubId, DataSet dsParam, string User_Id)
        {
            int errorCode = 0;

            DataSet dsRptSubDetails = new DataSet();
            try
            {
         
            DataTable dtRdsRptSubscription = new DataTable("tbl_RDSRptSubscription");
            dtRdsRptSubscription.Columns.Add("RptSubId", typeof(System.Int32));
            dtRdsRptSubscription.Columns.Add("ClientId", typeof(System.Int32));
            dtRdsRptSubscription.Columns.Add("ReportId", typeof(System.String));
            dtRdsRptSubscription.Columns.Add("CreatedBy", typeof(System.Int32));
            dtRdsRptSubscription.Columns.Add("CreatedDate", typeof(System.DateTime));
            dtRdsRptSubscription.Columns.Add("ModifiedBy", typeof(System.Int32));
            dtRdsRptSubscription.Columns.Add("ModifiedDate", typeof(System.DateTime));


            DataRow drRdsRptsub = dtRdsRptSubscription.NewRow();
            drRdsRptsub["RptSubId"] = rdsRptSubModel.RptSubId;
            drRdsRptsub["ClientId"] = rdsRptSubModel.ClientId;
            drRdsRptsub["ReportId"] = rdsRptSubModel.ReportId;
            drRdsRptsub["CreatedBy"] = rdsRptSubModel.UserID;
            drRdsRptsub["ModifiedBy"] = rdsRptSubModel.UserID;
            dtRdsRptSubscription.Rows.Add(drRdsRptsub);

            dsRptSubDetails.Tables.Add(dtRdsRptSubscription);


            string strFreq = "";
            int iVar = Convert.ToInt32(rdsRptSubModel.RptSubGenrationDetails.Frequency);
            if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.GenerationOpt.Dly))
            {
                strFreq = "Dly";
            }
            else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.GenerationOpt.Hfly))
            {
                strFreq = "Hfly";
            }
            else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.GenerationOpt.Mnly))
            {
                strFreq = "Mnly";
            }
            else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.GenerationOpt.Qtly))
            {
                strFreq = "Qtly";
            }
            else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.GenerationOpt.Wkly))
            {
                strFreq = "Wkly";
            }
            else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.GenerationOpt.Yrly))
            {
                strFreq = "Yrly";
            }

            DataTable dtRdsRptSubGen = new DataTable("tbl_RDSRptSubGenDetails");
            dtRdsRptSubGen.Columns.Add("RptSubId", typeof(System.Int32));
            dtRdsRptSubGen.Columns.Add("Frequency", typeof(System.String));
            dtRdsRptSubGen.Columns.Add("WeeklyOptions", typeof(System.String));
            dtRdsRptSubGen.Columns.Add("StartTime", typeof(System.TimeSpan));
            dtRdsRptSubGen.Columns.Add("EndTime", typeof(System.TimeSpan));
            dtRdsRptSubGen.Columns.Add("Interval", typeof(System.Int32));

            //foreach(DataRow dr in dsRptSubDetails.Tables["tbl_RDSRptSubscription"].Rows)
            {
                DataRow drRdsRptSubGen = dtRdsRptSubGen.NewRow();
                drRdsRptSubGen["RptSubId"] = rdsRptSubModel.RptSubId; 
                drRdsRptSubGen["Frequency"] = strFreq;
                drRdsRptSubGen["WeeklyOptions"] = rdsRptSubModel.RptSubGenrationDetails.WeeklyOptions;
                if (rdsRptSubModel.RptSubGenrationDetails.StartTime != null && rdsRptSubModel.RptSubGenrationDetails.StartTime != ""
                    && rdsRptSubModel.RptSubGenrationDetails.EndTime != null && rdsRptSubModel.RptSubGenrationDetails.EndTime !="" 
                    && rdsRptSubModel.RptSubGenrationDetails.Interval != null)
                {
                    drRdsRptSubGen["StartTime"] = rdsRptSubModel.RptSubGenrationDetails.StartTime;
                    drRdsRptSubGen["EndTime"] = rdsRptSubModel.RptSubGenrationDetails.EndTime;
                    drRdsRptSubGen["Interval"] = rdsRptSubModel.RptSubGenrationDetails.Interval;
                    dtRdsRptSubGen.Rows.Add(drRdsRptSubGen);
                }
                else
                {
                    drRdsRptSubGen["StartTime"] = "00:00";
                    drRdsRptSubGen["EndTime"] = "00:00";
                    drRdsRptSubGen["Interval"] = 0;
                    dtRdsRptSubGen.Rows.Add(drRdsRptSubGen);
                }
               
            }

            dsRptSubDetails.Tables.Add(dtRdsRptSubGen);

            DataTable dtRdsRptSubGenTime = new DataTable("tbl_RDSRptSubGenTimeDetails");
            dtRdsRptSubGenTime.Columns.Add("RptSubId", typeof(System.Int32));
            dtRdsRptSubGenTime.Columns.Add("GenTime", typeof(System.TimeSpan));
            if (rdsRptSubModel.RDSRptSubGenTimeList != null)
            {
                foreach (string GenTime in rdsRptSubModel.RDSRptSubGenTimeList)
                {
                    DataRow drRdsRptSubGenTime = dtRdsRptSubGenTime.NewRow();
                    drRdsRptSubGenTime["GenTime"] = GenTime;
                    drRdsRptSubGenTime["RptSubId"] = rdsRptSubModel.RptSubId;
                    dtRdsRptSubGenTime.Rows.Add(drRdsRptSubGenTime);
                }
            }
            else
            {
                DataRow drRdsRptSubGenTime = dtRdsRptSubGenTime.NewRow();
                drRdsRptSubGenTime["GenTime"] ="00:00";
                drRdsRptSubGenTime["RptSubId"] = rdsRptSubModel.RptSubId;
                dtRdsRptSubGenTime.Rows.Add(drRdsRptSubGenTime);
            }
            dsRptSubDetails.Tables.Add(dtRdsRptSubGenTime);

            DataTable dtRdsRptDistDetails = new DataTable("tbl_RDSRptSubDistDetails");
            dtRdsRptDistDetails.Columns.Add("RptSubId", typeof(System.Int32));
            dtRdsRptDistDetails.Columns.Add("DistType", typeof(System.String));
            dtRdsRptDistDetails.Columns.Add("DistTime", typeof(System.TimeSpan));
            dtRdsRptDistDetails.Columns.Add("MaxDistTime", typeof(System.TimeSpan));
            dtRdsRptDistDetails.Columns.Add("DistMode", typeof(System.String));
            dtRdsRptDistDetails.Columns.Add("Email", typeof(System.Boolean));
            dtRdsRptDistDetails.Columns.Add("FTP", typeof(System.Boolean));

            string strDistType = "";
            iVar = Convert.ToInt32(rdsRptSubModel.RptSubDistributionDetails.DistributionType);
            if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.DistributionOpt.DAST))
            {
                strDistType = "DAST";
            }
            else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.DistributionOpt.DWR))
            {
                strDistType = "DWR";
            }
            else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.DistributionOpt.PIZWR))
            {
                strDistType = "PIZWR";
            }
            else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.DistributionOpt.PIZADAST))
            {
                strDistType = "PIZADAST";
            }
            string strDistMode = "";
            iVar = Convert.ToInt32(rdsRptSubModel.RptSubDistributionDetails.DistributionMode);
            if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.DistribtionMode.DistributeMannually))
            {
                strDistMode = "Man";
            }
            else if (iVar == Convert.ToInt32(SmartSys.Utility.Enums.DistribtionMode.DistributeAuto))
            {
                strDistMode = "Aut";
            }

           
            {
                DataRow drRdsRptDistDetails = dtRdsRptDistDetails.NewRow();
                drRdsRptDistDetails["RptSubId"] = rdsRptSubModel.RptSubId;
                drRdsRptDistDetails["DistType"] = strDistType;
                drRdsRptDistDetails["DistTime"] = rdsRptSubModel.RptSubDistributionDetails.DistributionTime;
                drRdsRptDistDetails["MaxDistTime"] = rdsRptSubModel.RptSubDistributionDetails.MaxDistributionTime;
                drRdsRptDistDetails["DistMode"] = strDistMode;              
                drRdsRptDistDetails["Email"] = rdsRptSubModel.RptSubDistributionDetails.Email;
                drRdsRptDistDetails["FTP"] = rdsRptSubModel.RptSubDistributionDetails.FTP;

                dtRdsRptDistDetails.Rows.Add(drRdsRptDistDetails);
            }
            dsRptSubDetails.Tables.Add(dtRdsRptDistDetails);

            RDSRptSubDAL objDAL = new RDSRptSubDAL();
            errorCode = objDAL.SaveSingleRptSubDetails(dsRptSubDetails, RptSubId, dsParam, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errorCode;
        }
        public RDSRptSubModel GetSelectedRptSubScrip(int RptSubID)
        {
            RDSRptSubModel rdsRptSubModel = new RDSRptSubModel();
            rdsRptSubModel.RptSubGenrationDetails = new RDSRptSubGenDetails();
            rdsRptSubModel.RDSRptSubGenTimeList = new List<string>();
            rdsRptSubModel.RptSubDistributionDetails = new RDSRptSubDistrDetails(); 
            DataSet dsSeletList = new DataSet();
            RDSRptSubDAL objDAL = new RDSRptSubDAL();
            dsSeletList = objDAL.GetSelectedRptSubScrip(RptSubID);
            if (dsSeletList == null)
                return null;
            if(dsSeletList.Tables[0].Rows.Count > 0)
            {
                rdsRptSubModel.RptSubId = Convert.ToInt32(dsSeletList.Tables[0].Rows[0]["RptSubId"].ToString());
                rdsRptSubModel.ClientId = Convert.ToInt32(dsSeletList.Tables[0].Rows[0]["ClientId"].ToString());
                rdsRptSubModel.ReportId = dsSeletList.Tables[0].Rows[0]["ReportId"].ToString();
            }
            if(dsSeletList.Tables[1].Rows.Count > 0)
            {
                rdsRptSubModel.RptSubGenrationDetails.Frequency = dsSeletList.Tables[1].Rows[0]["Frequency"].ToString();
                rdsRptSubModel.RptSubGenrationDetails.WeeklyOptions = dsSeletList.Tables[1].Rows[0]["WeeklyOptions"].ToString();
                rdsRptSubModel.RptSubGenrationDetails.StartTime= dsSeletList.Tables[1].Rows[0]["StartTime"].ToString();
                rdsRptSubModel.RptSubGenrationDetails.EndTime = dsSeletList.Tables[1].Rows[0]["EndTime"].ToString();
                rdsRptSubModel.RptSubGenrationDetails.Interval = Convert.ToInt32(dsSeletList.Tables[1].Rows[0]["Interval"].ToString());
            }
            string word=rdsRptSubModel.RptSubGenrationDetails.WeeklyOptions;
           
            if(word[0] == '1')
            {
                rdsRptSubModel.Monday = true;
            }
            else
            {
                rdsRptSubModel.Monday = false;
            }
            if (word[1] == '1')
            {
                rdsRptSubModel.TuesDay = true;
            }
            else
            {
                rdsRptSubModel.TuesDay = false;
            }
            if (word[2] == '1')
            {
                rdsRptSubModel.Wednesday = true;
            }
            else
            {
                rdsRptSubModel.Wednesday = false;
            }

            if (word[3] == '1')
            {
                rdsRptSubModel.Thirsday = true;
            }
            else
            {
                rdsRptSubModel.Thirsday = false;
            }

            if (word[4] == '1')
            {
                rdsRptSubModel.Friday = true;
            }
            else
            {
                rdsRptSubModel.Friday = false;
            }
           
            if (word[5] == '1')
            {
                rdsRptSubModel.Saturday = true;
            }
            else
            {
                rdsRptSubModel.Saturday = false;
            }
            if (word[6] == '1')
            {
                rdsRptSubModel.Sunday = true;
            }
            else
            {
                rdsRptSubModel.Sunday = false;
            }
            string Freq = "";
            string iVar = rdsRptSubModel.RptSubGenrationDetails.Frequency;
            if (iVar == "Dly")
            {
                Freq = "1";
            }
            else if (iVar == "Hfly")
            {
                Freq = "5";
            }
            else if (iVar == "Mnly")
            {
                Freq = "3";
            }
            else if (iVar == "Qtly")
            {
                Freq = "4";
            }
            else if (iVar == "Wkly")
            {
                Freq = "2";
            }
            else if (iVar == "Yrly")
            {
                Freq = "6";
            }
            rdsRptSubModel.GenMode = Freq;
            if (dsSeletList.Tables[2].Rows.Count > 0)
            {
                foreach(DataRow dr in dsSeletList.Tables[2].Rows)
                {
                    rdsRptSubModel.RDSRptSubGenTimeList.Add(dr["GenTime"].ToString());  

                }
            }

            if(dsSeletList.Tables[3].Rows.Count > 0)
            {
                rdsRptSubModel.RptSubDistributionDetails.DistributionType = dsSeletList.Tables[3].Rows[0]["DistType"].ToString();
                rdsRptSubModel.RptSubDistributionDetails.DistributionTime = dsSeletList.Tables[3].Rows[0]["DistTime"].ToString();
                rdsRptSubModel.RptSubDistributionDetails.MaxDistributionTime = dsSeletList.Tables[3].Rows[0]["MaxDistTime"].ToString();
                rdsRptSubModel.RptSubDistributionDetails.DistributionMode = dsSeletList.Tables[3].Rows[0]["DistMode"].ToString();
                rdsRptSubModel.RptSubDistributionDetails.LocalFolder = dsSeletList.Tables[3].Rows[0]["LocalFolder"].ToString();
                rdsRptSubModel.RptSubDistributionDetails.Email = Convert.ToBoolean(dsSeletList.Tables[3].Rows[0]["email"].ToString());
                rdsRptSubModel.RptSubDistributionDetails.FTP = Convert.ToBoolean(dsSeletList.Tables[3].Rows[0]["FTP"].ToString());
            }

            string strDistType = "";
            iVar = rdsRptSubModel.RptSubDistributionDetails.DistributionType;
            if (iVar == "DAST")
            {
                strDistType = "1";
            }
            else if (iVar == "DWR")
            {
                strDistType = "2";
            }
            else if (iVar == "PIZWR")
            {
                strDistType = "3";
            }
            else if (iVar == "PIZADAST")
            {
                strDistType = "4";
            }
            rdsRptSubModel.DistType = strDistType;

            string strDistMode = "";
            iVar = rdsRptSubModel.RptSubDistributionDetails.DistributionMode;
            if (iVar == "Man")
            {
                strDistMode = "1";
            }
            else if (iVar == "Aut")
            {
                strDistMode = "2";
            }
            rdsRptSubModel.DistMode = strDistMode;
            return rdsRptSubModel;
        }

        public int DeletedSeletedRptSub(RDSRptSubModel rdsRPTSubModel)
        {
            RDSRptSubDAL objDAL = new RDSRptSubDAL();
            int err=0;
            try
            {
                DataSet dsDelete = new DataSet();
                DataTable dtDelete = new DataTable("tbl_RDSRptSubscription");
                dtDelete.Columns.Add("RptSubId", typeof(System.Int32));
                dtDelete.Columns.Add("UserID", typeof(System.Int32));

                DataRow drDelete = dtDelete.NewRow();
                drDelete["RptSubId"] = rdsRPTSubModel.RptSubId;
                drDelete["UserID"] = rdsRPTSubModel.UserID;
                dtDelete.Rows.Add(drDelete);
                dsDelete.Tables.Add(dtDelete);
                err = objDAL.DeletedSeletedRptSub(dsDelete);
            }
            catch(Exception ex)
            {
                Common.LogException(ex);
            }
            return err;
        }

        #endregion for Single Subscription to Gen
        #region for Daily ReportBook Deatil
        public List<DailyReportBookModel> GetDailyRptBookList()
        {
            List<DailyReportBookModel> lstDailyRptBook = new List<DailyReportBookModel>();
            try
            {
                RDSRptSubDAL objDAL = new RDSRptSubDAL();
                DataSet dsDailyRptBook = new DataSet();
                dsDailyRptBook = objDAL.GetDailyRptBookList();
                if(dsDailyRptBook !=null)
                {
                    if(dsDailyRptBook.Tables.Count > 0)
                    {
                        foreach(DataRow dr in dsDailyRptBook.Tables[0].Rows)
                        {
                            DailyReportBookModel demo = new DailyReportBookModel();
                            demo.DlyGenSubId = Convert.ToInt32(dr["DlyGenSubId"]);
                            demo.ReportId = dr["ReportId"].ToString();
                            demo.ReportName = dr["ReportName"].ToString();
                            demo.StatusCode = dr["StatusCode"].ToString();
                            demo.StatusId = dr["StatusId"].ToString();
                            if (dr["GenId"].ToString() != "")
                            demo.GenId = Convert.ToInt16(dr["GenId"]);
                            if (dr["RGSJobId"].ToString() != "")
                            demo.RGSJobId = Convert.ToInt32(dr["RGSJobId"]);
                            if (dr["OutputFileName"].ToString() != "")
                            demo.OutputFileName = dr["OutputFileName"].ToString();
                            demo.RunDateTime = Convert.ToDateTime(dr["RunDateTime"].ToString());
                            demo.ClientName = dr["ClientName"].ToString();
                            lstDailyRptBook.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstDailyRptBook;
        }

        public List<DailyReportBookModel> GetDailyRptBookParam(int DlyGenSubId)
        {
            List<DailyReportBookModel> adhocParamModel = new List<DailyReportBookModel>();
            try
            {
                RDSRptSubDAL objDAL = new RDSRptSubDAL();
                DataSet dsParmView = new DataSet();
                dsParmView = objDAL.GetDailyRptBookParam(DlyGenSubId);
                foreach (DataRow dr in dsParmView.Tables[0].Rows)
                {

                    DailyReportBookModel Demo = new DailyReportBookModel();
                    Demo.ParamName = dr["ParamName"].ToString();
                    Demo.ParamValue = dr["ParamValue"].ToString();

                    adhocParamModel.Add(Demo);
                }

            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }

            return adhocParamModel;
        }
        public int SetReDistStatus(int DlyGenSubId)
        {
            int errCode = 0; 
            try
            {
                RDSRptSubDAL objDAL = new RDSRptSubDAL();
                errCode = objDAL.SetReDistStatus(DlyGenSubId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet GetDlyBookErr(int DlyGenSubId)
        {
            DataSet dserr = new DataSet();
            try
            {
                RDSRptSubDAL objDAL = new RDSRptSubDAL();
                dserr = objDAL.GetDlyBookErr(DlyGenSubId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            
            return dserr;
        }
        public int GetRptSubId(int DlyGenSubId)
        {
            int RptSubID=0;
            try
            {
                RDSRptSubDAL objDAL = new RDSRptSubDAL();
                RptSubID = objDAL.GetRptSubId(DlyGenSubId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return RptSubID;
        }
        #endregion for Daily ReportBook Deatil
    }

  }