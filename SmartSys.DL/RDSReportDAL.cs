using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL
{
    public class RDSReportDAL
    {
        public DataSet GetRDSReportOPTypeList(string ReportType)
        {
            DataSet dsOutputType = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@OutputType", SqlDbType.VarChar);
                parameters[0].Value = ReportType;
                dsOutputType = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSReportO/PTypeList", parameters);
                return dsOutputType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTriggertList()
        {
            DataSet dsRDS = new DataSet();
            try
            {
                dsRDS = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSReportGetTriggerList");
                return dsRDS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetList()
        {
            DataSet dsRDS = new DataSet();
            try
            {
                dsRDS = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSReportGetList");
                return dsRDS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSelectedListReport(string ReportId)
        {
            DataSet dsRDS = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ReportId", SqlDbType.VarChar);
                parameters[0].Value = ReportId;
                dsRDS = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RDSReportGetSelectedReport", parameters);
                return dsRDS;
            } 
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private int ClearReport(string ReportId,SqlTransaction Tr)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@ReportId", SqlDbType.VarChar);
                parameters[0].Value = ReportId;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Tr, CommandType.StoredProcedure, "sp_RDSClearReportEventTrig", parameters);
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

        public int SaveEdit(DataSet dsSaveEdit, string User_Id)
        {
            SqlConnection cn = new SqlConnection(Common.SqlConnectionString);
            cn.Open();
            SqlTransaction Tr = cn.BeginTransaction();
           
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@ReportName", SqlDbType.VarChar);
                parameters[0].Value = dsSaveEdit.Tables[0].Rows[0]["ReportName"];

                parameters[1] = new SqlParameter("@ReportId", SqlDbType.VarChar);
                parameters[1].Value = dsSaveEdit.Tables[0].Rows[0]["ReportId"];

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Tr, CommandType.StoredProcedure, "sp_RDSReportUpdate", parameters);
                ClearReport(dsSaveEdit.Tables[0].Rows[0]["ReportId"].ToString(),Tr);
               
                    if (parameters[1].Value != null)
                    {
                        if (Convert.ToInt32(parameters[3].Value.ToString()) == 500001 )
                        {
                            foreach (DataRow dr in dsSaveEdit.Tables[1].Rows)
                            {
                                if (!(SaveEvent(Convert.ToInt16(dr["EventId"].ToString()), dr["ReportId"].ToString(), User_Id, Tr) == 500002))
                                {
                                    Tr.Rollback();
                                    cn.Close();
                                    return 2;
                                }
                            }
                            foreach (DataRow dr in dsSaveEdit.Tables[2].Rows)
                            {
                                if (!(SaveTrigger(Convert.ToInt16(dr["TriggerId"].ToString()), dr["ReportId"].ToString(), User_Id,Tr) == 500002))
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

           
            catch(Exception ex)
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

        private int SaveTrigger(Int16 TriggerId, string ReportId, string User_Id, SqlTransaction Tr)
        {
            try
            {
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@TriggerId", SqlDbType.SmallInt);
            parameters[0].Value = TriggerId;

            parameters[1] = new SqlParameter("@ReportId", SqlDbType.VarChar);
            parameters[1].Value = ReportId;

            parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            parameters[2].Value = User_Id;

            parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[3].Value = 0;
            parameters[3].Direction = ParameterDirection.InputOutput;


            SqlHelper.ExecuteDataset(Tr, CommandType.StoredProcedure, "sp_RDSSaveReportTriggerDep", parameters);
                if (parameters[3].Value != null)
                    return Convert.ToInt32(parameters[3].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }
        private int SaveEvent(Int16 EventId, string ReportId, string User_Id, SqlTransaction Tr)
        {
            try
            {
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@EventId", SqlDbType.SmallInt);
            parameters[0].Value = EventId;

            parameters[1] = new SqlParameter("@ReportId", SqlDbType.VarChar);
            parameters[1].Value = ReportId;

            parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            parameters[2].Value = User_Id;

            parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[3].Value = 0;
            parameters[3].Direction = ParameterDirection.InputOutput;
            
                SqlHelper.ExecuteDataset(Tr, CommandType.StoredProcedure, "sp_RDSSaveReportEventDep", parameters);
                if (parameters[3].Value != null)
                    return Convert.ToInt32(parameters[3].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
                
            }
        }

        public int UpdateStatus(Int32 DlyGenSubId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@DlyGenSubId", SqlDbType.SmallInt);
                parameters[0].Value = DlyGenSubId;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_UpdateRDSRptSubDLYGenStatus", parameters);

                errCode = Convert.ToInt32(parameters[1].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
    }
}
