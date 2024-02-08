using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using SmartSys.Utility;
using SmartSys.DAL;
using System.Collections;

namespace SmartSys.DAL
{
    public class DHFeedDal
    {

        #region Feed DAL

        public int SaveFeed(SqlTransaction sqlTransaction, DataSet dsFeedDetails, out int iFeedID, string User_Id)
        {
            int iErrorCode = 0;
            iFeedID = 0;
            try
            {
                SqlParameter[] DHFeedParam = new SqlParameter[7];

                DHFeedParam[0] = new SqlParameter("@FeedID", SqlDbType.Int);
                DHFeedParam[0].Value = dsFeedDetails.Tables["tbl_DHFeedMast"].Rows[0]["FeedId"];
                DHFeedParam[0].Direction = ParameterDirection.InputOutput;

                DHFeedParam[1] = new SqlParameter("@FeedName", SqlDbType.VarChar);
                DHFeedParam[1].Value = dsFeedDetails.Tables["tbl_DHFeedMast"].Rows[0]["FeedName"];

                DHFeedParam[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                DHFeedParam[2].Value = dsFeedDetails.Tables["tbl_DHFeedMast"].Rows[0]["Description"];

                DHFeedParam[3] = new SqlParameter("@FeedTypeCode", SqlDbType.VarChar);
                DHFeedParam[3].Value = dsFeedDetails.Tables["tbl_DHFeedMast"].Rows[0]["FeedTypeCode"];

                DHFeedParam[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                DHFeedParam[4].Value = User_Id;

                DHFeedParam[5] = new SqlParameter("@ModifiedBy", SqlDbType.Int);
                DHFeedParam[5].Value = dsFeedDetails.Tables["tbl_DHFeedMast"].Rows[0]["ModifiedBy"];

                DHFeedParam[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                DHFeedParam[6].Value = 0;
                DHFeedParam[6].Direction=ParameterDirection.Output;

                SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, "sp_DHSaveFeedMaster", DHFeedParam);

                iErrorCode = (int)DHFeedParam[6].Value;
                iFeedID = int.Parse(DHFeedParam[0].Value.ToString());

            }
            catch(Exception ex)
            {
                Common.LogException(ex);
            }

            return iErrorCode;

        }
        private int SaveActionParam(int SequenceNo, int FActionId, DataSet dsFeed, SqlTransaction tr)
        {
            int errCode = 0;
            try
            {
                foreach (DataRow dr in dsFeed.Tables["tbl_DHFeedActionParam"].Rows)
                {
                    if (Convert.ToInt32(dr["SequenceNo"].ToString()) == SequenceNo)
                    {
                        SqlParameter[] parameters = new SqlParameter[4];
                        parameters[0] = new SqlParameter("@FActionId", SqlDbType.Int);
                        parameters[0].Value = FActionId;

                        parameters[1] = new SqlParameter("@ParamName", SqlDbType.VarChar);
                        parameters[1].Value = dr["ParamName"];

                        parameters[2] = new SqlParameter("@Value", SqlDbType.VarChar);
                        parameters[2].Value = dr["Value"];

                        parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                        parameters[3].Value = 0;
                        parameters[3].Direction = ParameterDirection.InputOutput;
                        SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_DHSaveFeedActionParameters", parameters);
                        errCode = (int)parameters[3].Value;
                    }
                    else
                    {
                        errCode = 600002;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveFeedDetails(DataSet dsFeedDetails, string User_Id)
        {
            int iErrorCode = 0, iFeedID = 0;
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            try
            {
                if(dsFeedDetails!=null)
                {
                    int iErrorCode1 = SaveFeed(sqlTransaction, dsFeedDetails, out iFeedID, User_Id);
                    
                    foreach(DataRow dr in dsFeedDetails.Tables["tbl_DHFeedAction"].Rows)
                    {
                        dr["FeedId"] = iFeedID;
                    }

                    SqlParameter[] Param = new SqlParameter[2];
                    Param[0] = new SqlParameter("@FeedID", SqlDbType.Int);
                    Param[0].Value = iFeedID;

                    Param[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    Param[1].Value = 0;
                    Param[1].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DHDeleteFeedDetail", Param);
                    int errCode = (int)Param[1].Value;
                    if (errCode == 600001)
                    {
                        foreach (DataRow dr in dsFeedDetails.Tables[1].Rows)
                        {
                            SqlParameter[] parameters = new SqlParameter[6];
                            parameters[0] = new SqlParameter("@FeedId", SqlDbType.Int);
                            parameters[0].Value = dr["FeedId"];

                            parameters[1] = new SqlParameter("@ActionTypeCode", SqlDbType.VarChar);
                            parameters[1].Value = dr["ActionTypeCode"];

                            parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                            parameters[2].Value = dr["Description"];

                            parameters[3] = new SqlParameter("@SequenceNo", SqlDbType.Int);
                            parameters[3].Value = dr["SequenceNo"];

                            parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                            parameters[4].Value = 0;
                            parameters[4].Direction = ParameterDirection.InputOutput;

                            parameters[5] = new SqlParameter("@FactionID", SqlDbType.Int);
                            parameters[5].Value = 0;
                            parameters[5].Direction = ParameterDirection.InputOutput;

                            SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DHSaveFeedActions", parameters);

                            if (dr["ActionTypeCode"].ToString() == "SourceInfo" || dr["ActionTypeCode"].ToString() == "DestTable" || dr["ActionTypeCode"].ToString() == "DBLoaderProcess" || dr["ActionTypeCode"].ToString() == "ExceSP")
                            {
                                if (!(SaveActionParam(Convert.ToInt32(dr["SequenceNo"].ToString()), Convert.ToInt32(parameters[5].Value.ToString()), dsFeedDetails, sqlTransaction) == 600002))
                                {
                                    sqlTransaction.Rollback();
                                    sqlConn.Close();
                                    return 3;
                                }
                            }
                        }
                    }
                    ////int iErrorCode2 = SaveFeedActionDetails(sqlTransaction, dsFeedDetails.Tables["tbl_DHFeedAction"], out iFeedActionID);


                    //SqlCommand insertCommandVersion = SqlHelper.CreateCommand(sqlConn, "sp_DHSaveFeedActions", "FeedId", "ActionTypeCode","Description", "SequenceNo");
                    //insertCommandVersion.Parameters["@ErrorCode"].Value = 0;
                    //insertCommandVersion.Transaction = sqlTransaction;

                    //SqlCommand updateCommandVersion = SqlHelper.CreateCommand(sqlConn, "sp_DHSaveFeedActions", "FeedId", "ActionTypeCode", "Description", "SequenceNo");
                    //updateCommandVersion.Parameters["@ErrorCode"].Value = 0;
                    //updateCommandVersion.Transaction = sqlTransaction;

                    //SqlCommand deleteCommandVersion = SqlHelper.CreateCommand(sqlConn, "sp_DHDeleteFeedActions", "FeedId", "ActionTypeCode", "SequenceNo");
                    //deleteCommandVersion.Parameters["@ErrorCode"].Value = 0;
                    //deleteCommandVersion.Transaction = sqlTransaction;


                    //ArrayList ouptFromSP = new ArrayList();
                    //SqlHelper.UpdateDataset(insertCommandVersion, deleteCommandVersion, updateCommandVersion, dsFeedDetails, dsFeedDetails.Tables["tbl_DHFeedAction"].TableName, ref ouptFromSP);

                    //if (ouptFromSP.Count > 0)
                    //{
                    //    iErrorCode = int.Parse(ouptFromSP[0].ToString());
                    //}

                    ////

                    //SqlCommand insertCommandVersion1 = SqlHelper.CreateCommand(sqlConn, "sp_DHSaveFeedActionParameters", "ParamName", "Value", "SequenceNo");
                    //insertCommandVersion1.Parameters["@ErrorCode"].Value = 0;
                    //insertCommandVersion1.Transaction = sqlTransaction;

                    //SqlCommand updateCommandVersion1 = SqlHelper.CreateCommand(sqlConn, "sp_DHSaveFeedActionParameters", "ParamName", "Value", "SequenceNo");
                    //updateCommandVersion1.Parameters["@ErrorCode"].Value = 0;
                    //updateCommandVersion1.Transaction = sqlTransaction;

                    //SqlCommand deleteCommandVersion1 = SqlHelper.CreateCommand(sqlConn, "sp_DHDeleteActinParameters", "FActionId","ParamName", "Value");
                    //deleteCommandVersion1.Parameters["@ErrorCode"].Value = 0;
                    //deleteCommandVersion1.Transaction = sqlTransaction;


                    //ArrayList ouptFromSP1 = new ArrayList();
                    //SqlHelper.UpdateDataset(insertCommandVersion1, deleteCommandVersion1, updateCommandVersion1, dsFeedDetails, dsFeedDetails.Tables["tbl_DHFeedActionParam"].TableName, ref ouptFromSP1);

                    //if (ouptFromSP.Count > 0)
                    //{
                    //    iErrorCode = int.Parse(ouptFromSP[0].ToString());
                    //}


                    sqlTransaction.Commit();
                    sqlConn.Close();
                    }
            }
            catch(Exception ex)
            {
                sqlTransaction.Rollback();
                Common.LogException(ex);
            }
            return iErrorCode;
        }


        public DataSet GetFeedDetails(int iFeedID)
        {
            DataSet dsFeedDetails = new DataSet();
            try
            {
                SqlParameter sqlParamFeedID = new SqlParameter("@FeedId", SqlDbType.Int);
                sqlParamFeedID.Value = iFeedID;

                dsFeedDetails = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHGetFeedDetails", sqlParamFeedID);
            }
            catch(Exception ex)
            {
                Common.LogException(ex);
            }
            return dsFeedDetails;
        }

        public DataSet GetFeedList()
        {
            DataSet dsFeedList = new DataSet();
            try
            {
                dsFeedList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHGetFeedList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsFeedList;
        }
        public DataSet GetDHActionDetail()
        {
             DataSet dsAtionDetailList = new DataSet();
            try
            {
                dsAtionDetailList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHGetActionDetail");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsAtionDetailList;
        }

        public DataSet GetFeedTypeList()
        {
            DataSet dsFeedTypeList = new DataSet();
            try
            {
                dsFeedTypeList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHGetFeedTypeList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsFeedTypeList;

        }
        public DataSet GetActionTypeList()
        {
            DataSet dsActionTypeList = new DataSet();
            try
            {
                dsActionTypeList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHGetActionTypeList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsActionTypeList;

        }

        public DataSet GetSelectedFeedParamName()
        {
            DataSet dsObj = new DataSet();
            try
            {
                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHGetSelectedFeedParamName");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsObj;

        }


        
        #endregion

        #region DHFeed Add Dependency
        private int ClearReport(string FeedId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@FeedId", SqlDbType.VarChar);
                parameters[0].Value = FeedId;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHClearFeedEventTrig", parameters);
                if (parameters[1].Value != null)
                    return Convert.ToInt32(parameters[1].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int SaveTrigger(Int16 TriggerId, int FeedId, Int16 CreatedBy,string User_Id, SqlTransaction tr)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@TriggerId", SqlDbType.SmallInt);
                parameters[0].Value = TriggerId;

                parameters[1] = new SqlParameter("@FeedId", SqlDbType.VarChar);
                parameters[1].Value = FeedId;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;


                SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_DHSaveFeedTriggerDep", parameters);
                if (parameters[3].Value != null)
                    return Convert.ToInt32(parameters[3].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int SaveEvent(Int16 EventId, int FeedId, Int16 CreatedBy,string User_Id, SqlTransaction tr)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@EventId", SqlDbType.SmallInt);
                parameters[0].Value = EventId;

                parameters[1] = new SqlParameter("@FeedId", SqlDbType.VarChar);
                parameters[1].Value =  FeedId;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_DHSaveFeedEventDep", parameters);
                if (parameters[3].Value != null)
                    return Convert.ToInt32(parameters[3].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SaveEdit(DataSet dsSaveEdit, string User_Id)
        {
            SqlConnection cn = new SqlConnection(Common.SqlConnectionString);
            cn.Open();
            SqlTransaction Tr = cn.BeginTransaction();

            try
            {
               SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@FeedId", SqlDbType.VarChar);
                parameters[0].Value = dsSaveEdit.Tables[0].Rows[0]["FeedId"]; ;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Tr, CommandType.StoredProcedure, "sp_DHClearFeedEventTrig", parameters);
                
                if (parameters[0].Value != null)
                {
                    if (Convert.ToInt32(parameters[1].Value.ToString()) == 500001)
                    {
                        foreach (DataRow dr in dsSaveEdit.Tables[0].Rows)
                        {
                            if (!(SaveEvent(Convert.ToInt16(dr["EventId"].ToString()), Convert.ToInt32(dr["FeedId"].ToString()), Convert.ToInt16(dr["CreatedBy"]), User_Id, Tr) == 500002))
                            {
                                Tr.Rollback();
                                cn.Close();
                                return 2;
                            }
                        }
                        foreach (DataRow dr in dsSaveEdit.Tables[1].Rows)
                        {
                            if (!(SaveTrigger(Convert.ToInt16(dr["TriggerId"].ToString()), Convert.ToInt32(dr["FeedId"].ToString()), Convert.ToInt16(dr["CreatedBy"]), User_Id, Tr) == 500002))
                            {
                                Tr.Rollback();
                                cn.Close();
                                return 2;
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
                        return 1;
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
        #endregion DHFeed Add Dependency

        public DataSet GetFTPDetail(int FeedId)
        {
            DataSet dsObj = new DataSet();
            try
            {
                SqlParameter[] Parameter = new SqlParameter[7];

                Parameter[0] = new SqlParameter("@FeedId", SqlDbType.Int);
                Parameter[0].Value = FeedId;
                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHAdhocRunGetSelected", Parameter);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsObj;

        }
        public DataSet GetActionType(string FeedType)
        {
            DataSet dsObj = new DataSet();
            try
            {
                SqlParameter[] Parameter = new SqlParameter[7];

                Parameter[0] = new SqlParameter("@FeedTypeCode", SqlDbType.VarChar);
                Parameter[0].Value = FeedType;
                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHFeedGetActionTypList", Parameter);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsObj;

        }
        public int SaveUploadData(DataSet DSUploadData, string @User_Id)
        {
           int  Errorcode = 0;
            try
            {
                SqlConnection cn = new SqlConnection(Common.SqlConnectionString);
                cn.Open();
                SqlTransaction Tr = cn.BeginTransaction();
                try
                {
                    SqlParameter[] parameters = new SqlParameter[5];
                    parameters[0] = new SqlParameter("@FeedId", SqlDbType.Int);
                    parameters[0].Value = DSUploadData.Tables[0].Rows[0]["FeedId"];

                    parameters[1] = new SqlParameter("@LoaderId", SqlDbType.SmallInt);
                    parameters[1].Value = DSUploadData.Tables[0].Rows[0]["LoaderId"];

                    parameters[2] = new SqlParameter("@StatusId", SqlDbType.SmallInt);
                    parameters[2].Value = DSUploadData.Tables[0].Rows[0]["StatusId"];

                    parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[3].Value = 0;
                    parameters[3].Direction = ParameterDirection.InputOutput;

                    parameters[4] = new SqlParameter("@JobId", SqlDbType.Int);
                    parameters[4].Value = 0;
                    parameters[4].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(Tr, CommandType.StoredProcedure, "sp_DHJobSave", parameters);
                    if (parameters[2].Value != null)
                        if (Convert.ToInt32(parameters[3].Value.ToString()) == 500001)
                        {
                            if (parameters[4].Value != null)
                            {
                                if (Convert.ToInt32(parameters[4].Value.ToString()) > 0)
                                {
                                    if (!(SaveDHAdhocUpload(Convert.ToInt32(parameters[4].Value.ToString()), DSUploadData.Tables[1].Rows[0]["FileName"].ToString(), Convert.ToInt32(DSUploadData.Tables[1].Rows[0]["FeedId"]), Convert.ToInt32(DSUploadData.Tables[1].Rows[0]["CreatedBy"]), @User_Id, Tr) == 500001))
                                    {
                                        Tr.Rollback();
                                        cn.Close();
                                        return 3;
                                    }
                                    Tr.Commit();
                                    cn.Close();
                                    return 4;
                                }
                                else
                                {
                                    Tr.Rollback();
                                    cn.Close();
                                    return 2;
                                }
                            }
                            else
                            {
                                Tr.Rollback();
                                cn.Close();
                                return 1;
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
                }
            }
            catch (Exception ex) 
            {
                
                Common.LogException(ex);
            }
            return Errorcode;
        }

        private int SaveDHAdhocUpload(int JobId, string FileName, int FeedId,int createdBy,string @User_Id, SqlTransaction tr)
        {
            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@JobId", SqlDbType.Int);
            parameters[0].Value = JobId;

            parameters[1] = new SqlParameter("@FileName", SqlDbType.VarChar);
            parameters[1].Value = FileName;

            parameters[2] = new SqlParameter("@FeedId", SqlDbType.Int);
            parameters[2].Value = FeedId;

            parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            parameters[3].Value = @User_Id;

            parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[4].Value = 0;
            parameters[4].Direction = ParameterDirection.InputOutput;
            try
            {
                SqlHelper.ExecuteDataset(tr, CommandType.StoredProcedure, "sp_DHAdhocUploadSave", parameters);
                if (parameters[4].Value != null)
                    return Convert.ToInt32(parameters[4].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetDHAdhocRunList()
        {
            DataSet dsObj = new DataSet();
            try
            {
                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHAdhocRunList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsObj;

        }
        #region Review
        #region Inventory Ageing Analysis 
        public DataSet GetInventoryAgeingAnalysisList(string CompCode, DateTime AsOfDate,string User_Id)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[4];
                Param[0] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[0].Value = CompCode;

                Param[1] = new SqlParameter("@AsOfDate", SqlDbType.NVarChar);
                Param[1].Value = AsOfDate;

                Param[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[2].Value = User_Id;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptInventory_Ageing_Analysis", Param);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return Ds;
        }
        public int SaveInvAgingAdvent(string ItemNo, string Description, string Comment, string BunchId, string User_Id)
        {
            int ErroCode = 0;
            try
            {
                SqlParameter[] Param = new SqlParameter[6];
                Param[0] = new SqlParameter("@ItemNo", SqlDbType.NVarChar);
                Param[0].Value = ItemNo;

                Param[1] = new SqlParameter("@PartNo", SqlDbType.NVarChar);
                Param[1].Value = Description;

                Param[2] = new SqlParameter("@Comment", SqlDbType.NVarChar);
                Param[2].Value = Comment;

                Param[3] = new SqlParameter("@BunchId", SqlDbType.NVarChar);
                Param[3].Value = BunchId;

                Param[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[4].Value = User_Id;

                Param[5] = new SqlParameter("@ErroCode", SqlDbType.Int);
                Param[5].Value = 0;
                Param[5].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHSaveInventoryAdventComment", Param);
                ErroCode = (int)Param[5].Value;
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return ErroCode;
        }
        public DataSet ShowInvAgingAdvent(string CompCode, String ItemNo)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[5];
                Param[0] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[0].Value = CompCode;

                Param[1] = new SqlParameter("@ItemNo", SqlDbType.NVarChar);
                Param[1].Value = ItemNo;
             
                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptInvAgeingAComment", Param);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return Ds;
        }
        #endregion Inventory Ageing Analysis
        #region Inventory
        public DataSet GetInventoryList(string CompCode, DateTime AsOfDate,string User_Id)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[4];
                Param[0] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[0].Value = CompCode;

                Param[1] = new SqlParameter("@AsOfDate", SqlDbType.NVarChar);
                Param[1].Value = AsOfDate;

                Param[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[2].Value = User_Id;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptInventory", Param);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return Ds;
        }
        public DataSet GetInventoryDatetoShow(string CompCode, string ItemNo)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[0].Value = CompCode;

                Param[1] = new SqlParameter("@ItemNo", SqlDbType.NVarChar);
                Param[1].Value = ItemNo;

                //Param[2] = new SqlParameter("@AsOfDate", SqlDbType.NVarChar);
                //Param[2].Value = AsOfDate;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptInvReview", Param);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return Ds;
        }
        public int SaveInventoryAdventData(string ItemNo, string Description, string Comment, string BunchId, string User_Id)
        {
            int ErroCode = 0;
            try
            {
                SqlParameter[] Param = new SqlParameter[6];
                Param[0] = new SqlParameter("@ItemNo", SqlDbType.NVarChar);
                Param[0].Value = ItemNo;

                Param[1] = new SqlParameter("@Description", SqlDbType.NVarChar);
                Param[1].Value = Description;

                Param[2] = new SqlParameter("@Comment", SqlDbType.NVarChar);
                Param[2].Value = Comment;

                Param[3] = new SqlParameter("@BunchId", SqlDbType.NVarChar);
                Param[3].Value = BunchId;

                Param[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[4].Value = User_Id;

                Param[5] = new SqlParameter("@ErroCode", SqlDbType.Int);
                Param[5].Value = 0;
                Param[5].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHSaveInvAdventComment", Param);
                ErroCode = (int)Param[5].Value;
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return ErroCode;
        }
        #endregion Inventory
        #region Material Procured Vs Reduction
        public DataSet GetMaterialProcuredVsReductionList(string CompCode, DateTime FromDate, DateTime ToDate,string User_Id)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[5];
                Param[0] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[0].Value = CompCode;

                Param[1] = new SqlParameter("@FromDate", SqlDbType.NVarChar);
                Param[1].Value = FromDate;

                Param[2] = new SqlParameter("@ToDate", SqlDbType.NVarChar);
                Param[2].Value = ToDate;

                Param[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[3].Value = User_Id;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptMaterialProcuredVsReduction", Param);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return Ds;
        }
        public DataSet ShowMaterialProcuredVsReduction(string CompCode, DateTime FromDate, DateTime ToDate, string DocumentNo, string ItemName)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[5];
                Param[0] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[0].Value = CompCode;

                Param[1] = new SqlParameter("@FromDate", SqlDbType.NVarChar);
                Param[1].Value = FromDate;

                Param[2] = new SqlParameter("@ToDate", SqlDbType.NVarChar);
                Param[2].Value = ToDate;

                Param[3] = new SqlParameter("@DocumentNo", SqlDbType.NVarChar);
                Param[3].Value = DocumentNo;

                Param[4] = new SqlParameter("@ItemName", SqlDbType.NVarChar);
                Param[4].Value = ItemName;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptMPVsRComment", Param);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return Ds;
        }
        public int SaveMPRAdvent(string ItemName, string DocumentNo, string Comment,string BunchId,string User_Id)
        {
            int ErroCode = 0;
            try
            {
                SqlParameter[] Param = new SqlParameter[6];
                Param[0] = new SqlParameter("@PartNo", SqlDbType.NVarChar);
                Param[0].Value = ItemName;

                Param[1] = new SqlParameter("@DocumentNo", SqlDbType.NVarChar);
                Param[1].Value = DocumentNo;

                Param[2] = new SqlParameter("@Comment", SqlDbType.NVarChar);
                Param[2].Value = Comment;

                Param[3] = new SqlParameter("@BunchId", SqlDbType.NVarChar);
                Param[3].Value = BunchId;

                Param[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[4].Value = User_Id;

                Param[5] = new SqlParameter("@ErroCode", SqlDbType.Int);
                Param[5].Value = 0;
                Param[5].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHSaveMaterialAdventComment", Param);
                ErroCode = (int)Param[5].Value;
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return ErroCode;
        }
        #endregion Material Procured Vs Reduction
        #region NNR 
        public DataSet GetNNRList(string CompCode,DateTime FromDate,DateTime ToDate,string User_Id)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[5];
                Param[0] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[0].Value = CompCode;

                Param[1] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                Param[1].Value = FromDate;

                Param[2]= new SqlParameter("@ToDate", SqlDbType.DateTime);
                Param[2].Value = ToDate;

                Param[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[3].Value = User_Id;
                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptSalesReportNNR", Param);
            }
            catch (Exception)
            {

                throw;
            }
            return Ds;
        }
        public DataSet ShowNNRListAdvent(string DocumentNo, string ItemNo, string CompCode, DateTime FromDate, DateTime ToDate)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[6];
                Param[0] = new SqlParameter("@DocumentNo", SqlDbType.NVarChar);
                Param[0].Value = DocumentNo;

                Param[1] = new SqlParameter("@ItemNo", SqlDbType.NVarChar);
                Param[1].Value = ItemNo;

                Param[2] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[2].Value = CompCode;

                Param[3] = new SqlParameter("@FromDate", SqlDbType.NVarChar);
                Param[3].Value = FromDate;

                Param[4] = new SqlParameter("@ToDate", SqlDbType.NVarChar);
                Param[4].Value = ToDate;

                Ds=SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptSales_NNR_Review", Param);
               
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return Ds;
        }
        public int SaveNNRAdventData(string DocumentNo, string MPN, string Comment, string ItemNo, string BunchId, string User_Id)
        {
            int ErroCode = 0;
            try
            {
                SqlParameter[] Param = new SqlParameter[7];
                Param[0] = new SqlParameter("@InvoiceNo", SqlDbType.NVarChar);
                Param[0].Value = DocumentNo;

                Param[1] = new SqlParameter("@MPN", SqlDbType.NVarChar);
                Param[1].Value = MPN;

                Param[2] = new SqlParameter("@Comment", SqlDbType.NVarChar);
                Param[2].Value = Comment;

                Param[3] = new SqlParameter("@BunchId", SqlDbType.NVarChar);
                Param[3].Value = BunchId;

                Param[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[4].Value = User_Id;

                Param[5] = new SqlParameter("@ErroCode", SqlDbType.Int);
                Param[5].Value = 0;
                Param[5].Direction = ParameterDirection.InputOutput;

                Param[6] = new SqlParameter("@ItemNo", SqlDbType.NVarChar);
                Param[6].Value = ItemNo;


                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHSaveNNRAdventComment", Param);
                ErroCode = (int)Param[5].Value;
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return ErroCode;
        }
        #endregion NNR
        #region Purchase Report Monthwise
        public DataSet GetPurchaseMonthwiseList(string CompCode, DateTime FromDate, DateTime ToDate,string User_Id)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[5];
                Param[0] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[0].Value = CompCode;

                Param[1] = new SqlParameter("@FromDate", SqlDbType.NVarChar);
                Param[1].Value = FromDate;

                Param[2] = new SqlParameter("@ToDate", SqlDbType.NVarChar);
                Param[2].Value = ToDate;

                Param[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[3].Value = User_Id;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptPurchaseRptByMonth", Param);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return Ds;
        }
        public DataSet ShowPurchaseMonthwiseAdvent(string DocumentNo, string ItenNo,int LineNo, string CompCode, DateTime FromDate, DateTime ToDate)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[6];
                Param[0] = new SqlParameter("@DocumentNo", SqlDbType.NVarChar);
                Param[0].Value = DocumentNo;

                Param[1] = new SqlParameter("@ItemNo", SqlDbType.NVarChar);
                Param[1].Value = ItenNo;

                Param[2] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[2].Value = CompCode;

                Param[3] = new SqlParameter("@FromDate", SqlDbType.NVarChar);
                Param[3].Value = FromDate;

                Param[4] = new SqlParameter("@ToDate", SqlDbType.NVarChar);
                Param[4].Value = ToDate;

                Param[5] = new SqlParameter("@LineNo", SqlDbType.NVarChar);
                Param[5].Value = LineNo;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptPurchRpt_Review", Param);

            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return Ds;
        }
        public int SavePurchMonthwiseAdventData(string DocumentNo,string Comment, string ItemNo, string LineNo, string BunchId, string User_Id)
        {
            int ErroCode = 0;
            try
            {
                SqlParameter[] Param = new SqlParameter[7];
                Param[0] = new SqlParameter("@InvoiceNo", SqlDbType.NVarChar);
                Param[0].Value = DocumentNo;

                Param[1] = new SqlParameter("@No", SqlDbType.NVarChar);
                Param[1].Value = ItemNo;

                Param[2] = new SqlParameter("@Comment", SqlDbType.NVarChar);
                Param[2].Value = Comment;

                Param[3] = new SqlParameter("@BunchId", SqlDbType.NVarChar);
                Param[3].Value = BunchId;

                Param[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[4].Value = User_Id;

                Param[5] = new SqlParameter("@ErroCode", SqlDbType.Int);
                Param[5].Value = 0;
                Param[5].Direction = ParameterDirection.InputOutput;

                Param[6] = new SqlParameter("@LineNo", SqlDbType.NVarChar);
                Param[6].Value = LineNo;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHSavePurchaseAdventComment", Param);
                ErroCode = (int)Param[5].Value;
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return ErroCode;
        }
        #endregion Purchase Report Monthwise
        #region Pending PO
        public DataSet GetPendingPOList(string CompCode,string User_Id)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[3];
                Param[0] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[0].Value = CompCode;

                Param[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[1].Value = User_Id;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptPending_PO", Param);
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return Ds;
        }
        public DataSet ShowPendPOListAdvent(string DocumentNo, string ItemNo, int LineNo, string CompCode)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[6];
                Param[0] = new SqlParameter("@DocumentNo", SqlDbType.NVarChar);
                Param[0].Value = DocumentNo;

                Param[1] = new SqlParameter("@ItemNo", SqlDbType.NVarChar);
                Param[1].Value = ItemNo;

                Param[2] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[2].Value = CompCode;

                Param[3] = new SqlParameter("@LineNo", SqlDbType.Int);
                Param[3].Value = LineNo;           

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptPen_PO_Review", Param);

            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return Ds;
        }
        public int SavePendPOAdventData(string DocumentNo, string ItemNo, string LineNo, string Comment, string BunchId, string User_Id)
        {
            int ErroCode = 0;
            try
            {
                SqlParameter[] Param = new SqlParameter[7];
                Param[0] = new SqlParameter("@OrderNo", SqlDbType.NVarChar);
                Param[0].Value = DocumentNo;

                Param[1] = new SqlParameter("@ItemNo", SqlDbType.NVarChar);
                Param[1].Value = ItemNo;

                Param[2] = new SqlParameter("@Comment", SqlDbType.NVarChar);
                Param[2].Value = Comment;

                Param[3] = new SqlParameter("@BunchId", SqlDbType.NVarChar);
                Param[3].Value = BunchId;

                Param[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[4].Value = User_Id;

                Param[5] = new SqlParameter("@ErroCode", SqlDbType.Int);
                Param[5].Value = 0;
                Param[5].Direction = ParameterDirection.InputOutput;

                Param[6] = new SqlParameter("@LineNo", SqlDbType.Int);
                Param[6].Value = LineNo;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHSavePendingPOAdventComment", Param);
                ErroCode = (int)Param[5].Value;
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return ErroCode;
        }
        #endregion Pending PO
        #region Franchise 
        public DataSet GetFranchiseList(string CompCode,DateTime FromDate,DateTime ToDate,string User_Id)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[5];
                Param[0] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[0].Value = CompCode;

                Param[1] = new SqlParameter("@FromDate", SqlDbType.DateTime);
                Param[1].Value = FromDate;

                Param[2] = new SqlParameter("@ToDate", SqlDbType.DateTime);
                Param[2].Value = ToDate;

                Param[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[3].Value = User_Id;
                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptFranchiseReview", Param);
            }
            catch (Exception)
            {

                throw;
            }
            return Ds;
        }
        public DataSet ShowFranchiseListAdvent(string VendorNo, string CompCode, DateTime FromDate, DateTime ToDate)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] Param = new SqlParameter[6];
                Param[0] = new SqlParameter("@VendorNo", SqlDbType.NVarChar);
                Param[0].Value = VendorNo;               

                Param[1] = new SqlParameter("@CompCode", SqlDbType.NVarChar);
                Param[1].Value = CompCode;

                Param[2] = new SqlParameter("@FromDate", SqlDbType.NVarChar);
                Param[2].Value = FromDate;

                Param[3] = new SqlParameter("@ToDate", SqlDbType.NVarChar);
                Param[3].Value = ToDate;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptF_R_Review", Param);
            }
            catch (Exception Ex)
            {

                throw Ex;
            }
            return Ds;
        }
        public int SaveFranchiseAdventData(string VendorName, string VendorNo, string Comment, string BunchId, string User_Id)
        {
            int ErroCode = 0;
            try
            {
                SqlParameter[] Param = new SqlParameter[7];
                Param[0] = new SqlParameter("@SupplierName", SqlDbType.NVarChar);
                Param[0].Value = VendorName;

                Param[1] = new SqlParameter("@VendorNo", SqlDbType.NVarChar);
                Param[1].Value = VendorNo;

                Param[2] = new SqlParameter("@Comment", SqlDbType.NVarChar);
                Param[2].Value = Comment;

                Param[3] = new SqlParameter("@BunchId", SqlDbType.NVarChar);
                Param[3].Value = BunchId;

                Param[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                Param[4].Value = User_Id;

                Param[5] = new SqlParameter("@ErroCode", SqlDbType.Int);
                Param[5].Value = 0;
                Param[5].Direction = ParameterDirection.InputOutput;               

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DHSaveFranchiseAdventComment", Param);
                ErroCode = (int)Param[5].Value;
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
