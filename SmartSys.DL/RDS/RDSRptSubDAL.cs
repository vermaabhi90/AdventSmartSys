using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SmartSys.Utility;
using SmartSys.DAL;
using System.Collections.Generic;
using System.Collections;
namespace SmartSys.DL.RDS
{
   public class RDSRptSubDAL
    {
       public int SaveRdsRptSubDetails(DataSet dsRptSubDetails, SqlTransaction sqlTransaction, out List<int> lstRptSubID)
       {
           
           int iErrorCode = 0;
           int  iRptSubId = 0;
           lstRptSubID = new System.Collections.Generic.List<int>();
           try
           {
              foreach (DataRow dr in dsRptSubDetails.Tables["tbl_RDSRptSubscription"].Rows)
               {

                   SqlParameter[] RdsRptSubParam = new SqlParameter[6];

                   RdsRptSubParam[0] = new SqlParameter("@RptSubId", SqlDbType.Int);
                   RdsRptSubParam[0].Value = dr["RptSubId"];
                   RdsRptSubParam[0].Direction = ParameterDirection.InputOutput;

                   RdsRptSubParam[1] = new SqlParameter("@ReportId", SqlDbType.VarChar);
                   RdsRptSubParam[1].Value = dr["ReportId"];

                   RdsRptSubParam[2] = new SqlParameter("@ClientId", SqlDbType.Int);
                   RdsRptSubParam[2].Value = dr["ClientId"];

                   RdsRptSubParam[3] = new SqlParameter("@CreatedBy", SqlDbType.Int);
                   RdsRptSubParam[3].Value = dr["CreatedBy"];

                   RdsRptSubParam[4] = new SqlParameter("@ModifiedBy", SqlDbType.Int);
                   RdsRptSubParam[4].Value = dr["ModifiedBy"];

                   RdsRptSubParam[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                   RdsRptSubParam[5].Value = 0;
                   RdsRptSubParam[5].Direction = ParameterDirection.Output;

                   SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, "sp_RDSSaveRptSub", RdsRptSubParam);

                   iErrorCode = (int)RdsRptSubParam[5].Value;

                   //Call this method to save RptSubID 
                   iRptSubId = int.Parse(RdsRptSubParam[0].Value.ToString());
                   lstRptSubID.Add(iRptSubId);
               }
           }
           catch(Exception ex)
           {
               Common.LogException(ex);
           }

           return iErrorCode;
       }

       public void FillDataSetWithRptSubID(ref DataSet dsRptSubDetails,List<int> lstRptSbuID)
       {
          try
          {
              DataTable dtRDSRptSubParamOverride = dsRptSubDetails.Tables["tbl_RDSRptSubParamOverride"].Clone();
              dtRDSRptSubParamOverride.TableName = "tbl_RDSRptSubParamOverride";
              foreach(int iRptSubID in lstRptSbuID)
              {
                 foreach(DataRow dr in dsRptSubDetails.Tables["tbl_RDSRptSubParamOverride"].Rows)
                 {
                     DataRow drNew = dtRDSRptSubParamOverride.NewRow();
                     drNew.ItemArray = dr.ItemArray as Object[];
                     drNew["RptSubId"] = iRptSubID;
                     dtRDSRptSubParamOverride.Rows.Add(drNew);
                 }                 
              }

              dsRptSubDetails.Tables.Remove("tbl_RDSRptSubParamOverride");
              dsRptSubDetails.Tables.Add(dtRDSRptSubParamOverride);

              foreach(int iRptSubID in lstRptSbuID)
              {
                  //Add RptSubID to RDSRptSubGenDetails in dataset 
                  DataRow dr = dsRptSubDetails.Tables["tbl_RDSRptSubGenDetails"].NewRow();
                  dr.ItemArray = dsRptSubDetails.Tables["tbl_RDSRptSubGenDetails"].Rows[0].ItemArray as Object[];
                  dr["RptSubId"] = iRptSubID;
                  dsRptSubDetails.Tables["tbl_RDSRptSubGenDetails"].Rows.Add(dr);


                  //Add  RptSubID to tbl_RDSRptSubDistDetails in dataset 
                  DataRow drNew = dsRptSubDetails.Tables["tbl_RDSRptSubDistDetails"].NewRow();
                  drNew.ItemArray = dsRptSubDetails.Tables["tbl_RDSRptSubDistDetails"].Rows[0].ItemArray as Object[];
                  drNew["RptSubId"] = iRptSubID;
                  dsRptSubDetails.Tables["tbl_RDSRptSubDistDetails"].Rows.Add(drNew);
              }
              dsRptSubDetails.Tables["tbl_RDSRptSubGenDetails"].Rows.RemoveAt(0);
              dsRptSubDetails.Tables["tbl_RDSRptSubDistDetails"].Rows.RemoveAt(0);

              //Add RptSubID to GenTimeDetails in the dataset 
              DataTable dtRDSRptSubGenTimeDetails = dsRptSubDetails.Tables["tbl_RDSRptSubGenTimeDetails"].Clone();
              dtRDSRptSubGenTimeDetails.TableName = "tbl_RDSRptSubGenTimeDetails";

              foreach (int iRptSubID in lstRptSbuID)
              {
                  foreach (DataRow dr in dsRptSubDetails.Tables["tbl_RDSRptSubGenTimeDetails"].Rows)
                  {
                      DataRow drNew = dtRDSRptSubGenTimeDetails.NewRow();
                      drNew.ItemArray = dr.ItemArray as Object[];
                      drNew["RptSubId"] = iRptSubID;
                      dtRDSRptSubGenTimeDetails.Rows.Add(drNew);
                  }
              }
              dsRptSubDetails.Tables.Remove("tbl_RDSRptSubGenTimeDetails");
              dsRptSubDetails.Tables.Add(dtRDSRptSubGenTimeDetails);
          }
           catch(Exception ex)
          {
              Common.LogException(ex);
          }
       }
       public int SaveRptSubDetails(DataSet dsRptSubDetails)
       {
           int  iErrorCode = 0;
           SqlTransaction sqltransaction;
           SqlConnection sqlconn = new SqlConnection(Common.SqlConnectionString);
           sqlconn.Open();
           sqltransaction = sqlconn.BeginTransaction();
           try
           {
               
                   List<int> lstRptSubID = new System.Collections.Generic.List<int>();
                   iErrorCode = SaveRdsRptSubDetails(dsRptSubDetails, sqltransaction, out lstRptSubID);
                    
                    //Fill RptSubID in remaining tables  of subscription details for one to many mapping
                   FillDataSetWithRptSubID(ref dsRptSubDetails, lstRptSubID);

                   //Insert RptSubParameter details into the database
                   SqlCommand insertCommandVersion = SqlHelper.CreateCommand(sqlconn, "sp_RDSSaveRptSubParamOverride", "RptSubId", "ReportID", "ParamName", "DefaultValue");
                   insertCommandVersion.Parameters["@ErrorCode"].Value = 0;
                   insertCommandVersion.Transaction = sqltransaction;

                   SqlCommand updateCommandVersion = SqlHelper.CreateCommand(sqlconn, "sp_RDSSaveRptSubParamOverride", "RptSubId", "ReportID", "ParamName", "DefaultValue");
                   updateCommandVersion.Parameters["@ErrorCode"].Value = 0;
                   updateCommandVersion.Transaction = sqltransaction;

                   SqlCommand deleteCommandVersion = SqlHelper.CreateCommand(sqlconn, "sp_RDSDeleteRptSubParam", "RptSubId", "ReportID");
                   deleteCommandVersion.Parameters["@ErrorCode"].Value = 0;
                   deleteCommandVersion.Transaction = sqltransaction;


                   ArrayList ouptFromSP = new ArrayList();
                   SqlHelper.UpdateDataset(insertCommandVersion, deleteCommandVersion, updateCommandVersion, dsRptSubDetails, dsRptSubDetails.Tables["tbl_RDSRptSubParamOverride"].TableName, ref ouptFromSP);

                   if (ouptFromSP.Count > 0)
                   {
                       iErrorCode = int.Parse(ouptFromSP[0].ToString());
                   } 
                
                  if(iErrorCode==0)
                      throw new Exception();

                   //insert  RptGeneration details into the database
                  SqlCommand insertCommandGenDetails = SqlHelper.CreateCommand(sqlconn, "sp_RDSSaveRptSubGenDetails", "RptSubId", "Frequency", "WeeklyOptions", "StartTime", "EndTime", "Interval");
                   insertCommandGenDetails.Parameters["@ErrorCode"].Value = 0;
                   insertCommandGenDetails.Transaction = sqltransaction;

                   SqlCommand updateCommandGenDetails = SqlHelper.CreateCommand(sqlconn, "sp_RDSSaveRptSubGenDetails", "RptSubId", "Frequency", "WeeklyOptions", "StartTime", "EndTime", "Interval");
                   updateCommandGenDetails.Parameters["@ErrorCode"].Value = 0;
                   updateCommandGenDetails.Transaction = sqltransaction;

                   SqlCommand deleteCommandGenDetails = SqlHelper.CreateCommand(sqlconn, "sp_RDSDeleteRptSubGenDetails", "RptSubId");
                   deleteCommandGenDetails.Parameters["@ErrorCode"].Value = 0;
                   deleteCommandGenDetails.Transaction = sqltransaction;


                   ouptFromSP = new ArrayList();
                   SqlHelper.UpdateDataset(insertCommandGenDetails, deleteCommandGenDetails, updateCommandGenDetails, dsRptSubDetails, dsRptSubDetails.Tables["tbl_RDSRptSubGenDetails"].TableName, ref ouptFromSP);

                   if (ouptFromSP.Count > 0)
                   {
                       iErrorCode = int.Parse(ouptFromSP[0].ToString());
                   } 
                
                  if(iErrorCode==0)
                      throw new Exception();


                  //Insert RPTSubDistribution data into database
                  //sp_RDSRptSubDistDetails  sp_RDSDeleteRptSubDistDetails 
                  SqlCommand insertCommandDistDetails = SqlHelper.CreateCommand(sqlconn, "sp_RDSRptSaveSubDistDetails", "RptSubId", "DistType", "DistTime", "MaxDistTime", "DistMode", "LocalFolder");
                  insertCommandDistDetails.Parameters["@ErrorCode"].Value = 0;
                  insertCommandDistDetails.Transaction = sqltransaction;

                  SqlCommand updateCommandDistDetails = SqlHelper.CreateCommand(sqlconn, "sp_RDSRptSaveSubDistDetails", "RptSubId", "DistType", "DistTime", "MaxDistTime", "DistMode", "LocalFolder");
                  updateCommandDistDetails.Parameters["@ErrorCode"].Value = 0;
                  updateCommandDistDetails.Transaction = sqltransaction;

                  SqlCommand deleteCommandDistDetails = SqlHelper.CreateCommand(sqlconn, "sp_RDSDeleteRptSubDistDetails", "RptSubId");
                  deleteCommandDistDetails.Parameters["@ErrorCode"].Value = 0;
                  deleteCommandDistDetails.Transaction = sqltransaction;


                  ouptFromSP = new ArrayList();
                  SqlHelper.UpdateDataset(insertCommandDistDetails, deleteCommandDistDetails, updateCommandDistDetails, dsRptSubDetails, dsRptSubDetails.Tables["tbl_RDSRptSubDistDetails"].TableName, ref ouptFromSP);

                  if (ouptFromSP.Count > 0)
                  {
                      iErrorCode = int.Parse(ouptFromSP[0].ToString());
                  }

                  if (iErrorCode == 0)
                      throw new Exception();
                  

                  //Insert GentimeDetails into Database
                  SqlCommand insertCommandGenTimeDetails = SqlHelper.CreateCommand(sqlconn, "sp_RDSSaveRptSubGenTimeDetails", "RptSubId", "GenTime");
                  insertCommandGenTimeDetails.Parameters["@ErrorCode"].Value = 0;
                  insertCommandGenTimeDetails.Transaction = sqltransaction;

                  SqlCommand updateCommandGenTimeDetails = SqlHelper.CreateCommand(sqlconn, "sp_RDSSaveRptSubGenTimeDetails", "RptSubId", "GenTime");
                  updateCommandGenTimeDetails.Parameters["@ErrorCode"].Value = 0;
                  updateCommandGenTimeDetails.Transaction = sqltransaction;

                  SqlCommand deleteCommandGenTimeDetails = SqlHelper.CreateCommand(sqlconn, "sp_RDSDeleteRptSubGenTimeDetails", "RptSubId");
                  deleteCommandGenTimeDetails.Parameters["@ErrorCode"].Value = 0;
                  deleteCommandGenTimeDetails.Transaction = sqltransaction;


                  ouptFromSP = new ArrayList();
                  SqlHelper.UpdateDataset(insertCommandGenTimeDetails, deleteCommandGenTimeDetails, updateCommandGenTimeDetails, dsRptSubDetails, dsRptSubDetails.Tables["tbl_RDSRptSubGenTimeDetails"].TableName, ref ouptFromSP);

                  if (ouptFromSP.Count > 0)
                  {
                      iErrorCode = int.Parse(ouptFromSP[0].ToString());
                  }

                  if (iErrorCode == 0)
                      throw new Exception();


               sqltransaction.Commit();
            }
           catch (SqlException sqlex)
           {
               sqltransaction.Rollback();
               Common.LogException(sqlex);
           }
           catch (Exception ex)
           {
               sqltransaction.Rollback();
               Common.LogException(ex);
           }

           return iErrorCode;
       }

       public DataSet GetReportClientListByUser(int iUserID)
       {
           DataSet dsGetReportClientList = new DataSet();
           try
           {
               SqlParameter parameterUserId = new SqlParameter("@UserID",SqlDbType.Int);
               parameterUserId.Value = iUserID;
               dsGetReportClientList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetReportListByUser", parameterUserId);

           }
           catch(Exception ex)
           {
               Common.LogException(ex);
           }
           return dsGetReportClientList;
       }
           
       public DataSet GetReportParam(string strReportID)
       {
           DataSet dsReportParam = new DataSet();
           try
           {
               SqlParameter paramReportId = new SqlParameter("@ReportID",SqlDbType.VarChar);
               paramReportId.Value = strReportID;
               dsReportParam = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetRptParamaters", paramReportId);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsReportParam;
       }


       #region for Save Single RDS Subscription Report


       public DataSet SubscriptionList()
       {
           DataSet dsSubscripList = new DataSet();
           try
           {
               dsSubscripList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetRptSubscriptionList");
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsSubscripList;
       }


       public int SaveSingleRptSubDetails(DataSet dsRptSubDetails, int RptSubId, DataSet dsParam, string User_Id)
       {
           SqlConnection cn = new SqlConnection(Common.SqlConnectionString);
           cn.Open();
           SqlTransaction Tr = cn.BeginTransaction();
           int errorCode = 0;
           string ReportId = dsRptSubDetails.Tables[0].Rows[0]["ReportId"].ToString();
           try
           {
              
               SqlParameter[] RdsRptSubParam = new SqlParameter[5];

               RdsRptSubParam[0] = new SqlParameter("@RptSubId", SqlDbType.Int);
               RdsRptSubParam[0].Value = RptSubId;
               RdsRptSubParam[0].Direction = ParameterDirection.InputOutput;

               RdsRptSubParam[1] = new SqlParameter("@ReportId", SqlDbType.VarChar);
               RdsRptSubParam[1].Value = dsRptSubDetails.Tables[0].Rows[0]["ReportId"];

               RdsRptSubParam[2] = new SqlParameter("@ClientId", SqlDbType.Int);
               RdsRptSubParam[2].Value = dsRptSubDetails.Tables[0].Rows[0]["ClientId"];

               RdsRptSubParam[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
               RdsRptSubParam[3].Value = User_Id;

               RdsRptSubParam[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               RdsRptSubParam[4].Value = 0;
               RdsRptSubParam[4].Direction = ParameterDirection.Output;

               SqlHelper.ExecuteNonQuery(Tr, CommandType.StoredProcedure, "sp_RDSSaveRptSub", RdsRptSubParam);
               int RptSubIdDemo =Convert.ToInt32(RdsRptSubParam[0].Value);
               if (RptSubIdDemo > 0)
               {
                   if (!(DelRptSubDetail(RptSubIdDemo, Tr) == 600002))
                   {
                       Tr.Rollback();
                       cn.Close();
                       return 5;
                   }
               }

            if(RdsRptSubParam[4].Value != null)
            {
                if (Convert.ToInt32(RdsRptSubParam[4].Value.ToString()) == 600001 || Convert.ToInt32(RdsRptSubParam[4].Value.ToString()) == 500001)
                 {
                     if (!(SubGenDetail(dsRptSubDetails, Tr, RptSubIdDemo) == 500002))
                     {
                         Tr.Rollback();
                         cn.Close();
                         return 1;
                     }
                     foreach (DataRow dr in dsRptSubDetails.Tables[2].Rows)
                     {
                         if (!(SaveSubGenTimeDetail(RptSubIdDemo, Convert.ToDateTime(dr["GenTime"].ToString()), Tr) == 500002))
                         {
                             Tr.Rollback();
                             cn.Close();
                             return 2;
                         }
                     }
                     if (!(SubDistDetail(dsRptSubDetails, Tr, RptSubIdDemo) == 500002))
                     {
                         Tr.Rollback();
                         cn.Close();
                         return 3;
                     }
                        if (dsParam != null)
                        {
                            foreach (DataRow dr in dsParam.Tables[1].Rows)
                            {
                                if (!(SaveParam(RptSubIdDemo, ReportId, dr["ParamName"].ToString(), dr["ParamValue"].ToString(), Tr) == 500002))
                                {
                                    Tr.Rollback();
                                    cn.Close();
                                    return 3;
                                }
                            }
                        }
                     Tr.Commit();
                     cn.Close();
                     return 4;
                 }
                else
                {
                    Tr.Rollback();
                    cn.Close();
                    return 0;
                }
            }
            else
            {
                Tr.Rollback();
                cn.Close();
                return 0;
            }

           }
           catch (Exception ex)
           {
               if (cn.State == ConnectionState.Open)
               {
                   Tr.Rollback();
                   cn.Close();
               }
               Common.LogException(ex);
               return 0;
           }
          
       }
       private int SubGenDetail(DataSet dsGenDetail,SqlTransaction tr,int RptSubIdDemo)
       {
           try
           {

           SqlParameter[] RdsSubGenDetail = new SqlParameter[7];

           RdsSubGenDetail[0] = new SqlParameter("@RptSubId", SqlDbType.Int);
           RdsSubGenDetail[0].Value = RptSubIdDemo;

           RdsSubGenDetail[1] = new SqlParameter("@Frequency", SqlDbType.VarChar);
           RdsSubGenDetail[1].Value = dsGenDetail.Tables[1].Rows[0]["Frequency"];

           RdsSubGenDetail[2] = new SqlParameter("@WeeklyOptions", SqlDbType.VarChar);
           RdsSubGenDetail[2].Value = dsGenDetail.Tables[1].Rows[0]["WeeklyOptions"];

           RdsSubGenDetail[3] = new SqlParameter("@StartTime", SqlDbType.Time);
           RdsSubGenDetail[3].Value = dsGenDetail.Tables[1].Rows[0]["StartTime"];

           RdsSubGenDetail[4] = new SqlParameter("@EndTime", SqlDbType.Time);
           RdsSubGenDetail[4].Value = dsGenDetail.Tables[1].Rows[0]["EndTime"];

           RdsSubGenDetail[5] = new SqlParameter("@Interval", SqlDbType.Int);
           RdsSubGenDetail[5].Value = dsGenDetail.Tables[1].Rows[0]["Interval"];

           RdsSubGenDetail[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
           RdsSubGenDetail[6].Value = 0;
           RdsSubGenDetail[6].Direction = ParameterDirection.Output;

           SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_RDSSaveRptSubGenDetails", RdsSubGenDetail);
           if  (RdsSubGenDetail[6].Value != null)
               return Convert.ToInt32( RdsSubGenDetail[6].Value.ToString());
           else
               return 0;
           }
           catch (Exception ex)
           {

               Common.LogException(ex);
               return 0;
           }
       }
       private int SaveSubGenTimeDetail(int rptSubId, DateTime GenTime, SqlTransaction tr)
       {
          
           try
           {
               SqlParameter[] RdsSubGenTimeDetail = new SqlParameter[3];

               RdsSubGenTimeDetail[0] = new SqlParameter("@RptSubId", SqlDbType.Int);
               RdsSubGenTimeDetail[0].Value = rptSubId;

               RdsSubGenTimeDetail[1] = new SqlParameter("@GenTime", SqlDbType.DateTime);
               RdsSubGenTimeDetail[1].Value = GenTime;

               RdsSubGenTimeDetail[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               RdsSubGenTimeDetail[2].Value = 0;
               RdsSubGenTimeDetail[2].Direction = ParameterDirection.Output;
               SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_RDSSaveRptSubGenTimeDetails", RdsSubGenTimeDetail);
               if (RdsSubGenTimeDetail[2].Value != null)
                   return Convert.ToInt32(RdsSubGenTimeDetail[2].Value.ToString());
               else
                   return 0;
           }
           catch (Exception ex)
           {

               Common.LogException(ex);
               return 0;
           }
       }

       private int SubDistDetail(DataSet subDistDetail, SqlTransaction tr, int RptSubIdDemo)
       {
           try
           {
               SqlParameter[] RdsSubDistDetail = new SqlParameter[8];

               RdsSubDistDetail[0] = new SqlParameter("@RptSubId", SqlDbType.Int);
               RdsSubDistDetail[0].Value = RptSubIdDemo;

               RdsSubDistDetail[1] = new SqlParameter("@DistType", SqlDbType.VarChar);
               RdsSubDistDetail[1].Value = subDistDetail.Tables[3].Rows[0]["DistType"];

               RdsSubDistDetail[2] = new SqlParameter("@DistTime", SqlDbType.Time);
               RdsSubDistDetail[2].Value = subDistDetail.Tables[3].Rows[0]["DistTime"];

               RdsSubDistDetail[3] = new SqlParameter("@MaxDistTime", SqlDbType.Time);
               RdsSubDistDetail[3].Value = subDistDetail.Tables[3].Rows[0]["MaxDistTime"];

               RdsSubDistDetail[4] = new SqlParameter("@DistMode", SqlDbType.VarChar);
               RdsSubDistDetail[4].Value = subDistDetail.Tables[3].Rows[0]["DistMode"];

               RdsSubDistDetail[5] = new SqlParameter("@email", SqlDbType.VarChar);
               RdsSubDistDetail[5].Value = subDistDetail.Tables[3].Rows[0]["Email"];

               RdsSubDistDetail[6] = new SqlParameter("@FTP", SqlDbType.VarChar);
               RdsSubDistDetail[6].Value = subDistDetail.Tables[3].Rows[0]["FTP"];

               RdsSubDistDetail[7] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               RdsSubDistDetail[7].Value = 0;
               RdsSubDistDetail[7].Direction = ParameterDirection.Output;

               SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_RDSRptSaveSubDistDetails", RdsSubDistDetail);
               if (RdsSubDistDetail[7].Value != null)
                   return Convert.ToInt32(RdsSubDistDetail[7].Value.ToString());
               else
                   return 0;
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
               return 0;
           }
       }

       private int SaveParam(int RptSubId,string RptId,string ParamName,string Vaule,SqlTransaction tr)
       {
           int errCode = 0;
           try
           {
               SqlParameter[] RdsSubGenDetail = new SqlParameter[5];

               RdsSubGenDetail[0] = new SqlParameter("@RptSubId", SqlDbType.Int);
               RdsSubGenDetail[0].Value = RptSubId;

               RdsSubGenDetail[1] = new SqlParameter("@ReportID", SqlDbType.VarChar);
               RdsSubGenDetail[1].Value = RptId;

               RdsSubGenDetail[2] = new SqlParameter("@ParamName", SqlDbType.VarChar);
               RdsSubGenDetail[2].Value = ParamName;

               RdsSubGenDetail[3] = new SqlParameter("@DefaultValue", SqlDbType.VarChar);
               RdsSubGenDetail[3].Value = Vaule;

               RdsSubGenDetail[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               RdsSubGenDetail[4].Value = 0;
               RdsSubGenDetail[4].Direction = ParameterDirection.Output;

               SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_RDSSaveRptSubParamOvride", RdsSubGenDetail);
               if (RdsSubGenDetail[4].Value != null)
                   return Convert.ToInt32(RdsSubGenDetail[4].Value.ToString());
               else
                   return 0;
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return errCode;
       }
       private int DelRptSubDetail(int RptSubId,SqlTransaction tr)
       {
           try
           {
               SqlParameter[] Parameter = new SqlParameter[2];

               Parameter[0] = new SqlParameter("@RptSubId", SqlDbType.Int);
               Parameter[0].Value = RptSubId;

               Parameter[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               Parameter[1].Value = 0;
               Parameter[1].Direction = ParameterDirection.Output;

               SqlHelper.ExecuteNonQuery(tr, CommandType.StoredProcedure, "sp_RDSDeleteRptSubDeatils", Parameter);
               if (Parameter[1].Value != null)
                   return Convert.ToInt32(Parameter[1].Value.ToString());
               else
                   return 0;
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
               return 0;
           }
           
       }
       public DataSet GetSelectedRptSubScrip(int RptSubID)
       {
           DataSet dsRptsub = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@RptSubId", SqlDbType.Int);
               parameters[0].Value = RptSubID;
               dsRptsub = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetSelectedSingleRptSubscription", parameters);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsRptsub;
       }

       public  int DeletedSeletedRptSub(DataSet dsDelete)
       {

           try
           {
               SqlParameter[] Parameter = new SqlParameter[3];

               Parameter[0] = new SqlParameter("@RptSubId", SqlDbType.Int);
               Parameter[0].Value = dsDelete.Tables[0].Rows[0]["RptSubId"]; ;

               Parameter[1] = new SqlParameter("@UserID", SqlDbType.Int);
               Parameter[1].Value = dsDelete.Tables[0].Rows[0]["UserID"];

               Parameter[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               Parameter[2].Value = 0;
               Parameter[2].Direction = ParameterDirection.Output;
               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSFalesSeletedRptSubDeatils", Parameter);
               if (Parameter[2].Value != null)
                   return Convert.ToInt32(Parameter[2].Value.ToString());
               else
                   return 0;
           }
           catch (Exception ex)
           {

               Common.LogException(ex);
               return 0;
           }
       }

        #endregion for Save Single RDS Subscription Report

        #region for Daily ReportBook Deatil
       public DataSet GetDailyRptBookList()
       {
           DataSet dsDailyRptBook = new DataSet();
           try
           {
               dsDailyRptBook = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetDailyReportBookList");
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsDailyRptBook;
       }
       public DataSet GetDailyRptBookParam(int DlyGenSubId)
       {
           DataSet dsRptsub = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@DlyGenSubId", SqlDbType.Int);
               parameters[0].Value = DlyGenSubId;
               dsRptsub = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetDailyReportParamList", parameters);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsRptsub;
       }
       public int SetReDistStatus(int DlyGenSubId)
       {
           try
           {
               SqlParameter[] Parameter = new SqlParameter[2];

               Parameter[0] = new SqlParameter("@DlyGenSubId", SqlDbType.Int);
               Parameter[0].Value = DlyGenSubId;

               Parameter[1] = new SqlParameter("@StatusId", SqlDbType.SmallInt);
               Parameter[1].Value = (int)Enums.StatusCode.RDSReDist;
               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSSetDLYGenSubStatus", Parameter);
               return 0;
           }
           catch (Exception ex)
           {

               Common.LogException(ex);
               return 0;
           }
       }
       public DataSet GetDlyBookErr(int DlyGenSubId)
       {

           try
           {
               SqlParameter[] parameters = new SqlParameter[1];
               parameters[0] = new SqlParameter("@DlyGenSubId", SqlDbType.BigInt);
               parameters[0].Value = DlyGenSubId;
               DataSet dsAdhoc = new DataSet();
               dsAdhoc = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSDailyReportErrorDetail", parameters);
               return dsAdhoc;
           }
           catch (Exception)
           {

               throw;
           }

       }
       public int GetRptSubId(int DlyGenSubId)
       {
           int RptSubID = 0;
           try
           {
               SqlParameter[] parameters = new SqlParameter[2];
               parameters[0] = new SqlParameter("@DlyGenSubId", SqlDbType.VarChar);
               parameters[0].Value = DlyGenSubId;

               parameters[1] = new SqlParameter("@RptSubID", SqlDbType.Int);
               parameters[1].Value = 0;
               parameters[1].Direction = ParameterDirection.Output;

               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSGetRptSubId", parameters);
               if (parameters[1].Value != null)
                   RptSubID= Convert.ToInt32(parameters[1].Value.ToString());
               else
                   return 0;
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
