using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.EmailProcess
{
   public class EmailDAL
    {
       public DataSet ChkEmailProcessFeed(string subject, string MailID)
       {
           DataSet dsLoader = new DataSet();
           try
           {
               SqlParameter[] parameters = new SqlParameter[2];
               parameters[0] = new SqlParameter("@subject", SqlDbType.VarChar);
               parameters[0].Value = subject;

               parameters[1] = new SqlParameter("@MailID", SqlDbType.VarChar);
               parameters[1].Value = MailID;

               dsLoader = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_EWEmailChkFeed", parameters);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsLoader;
       }

       public DataSet SubjectList()
       {
           DataSet dsSubjectList = new DataSet();
           try
           {
               dsSubjectList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_EWGetFeedDescripList");
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsSubjectList;
       }

       public DataSet EmailProcessData()
       {
           DataSet dsEmailProcess = new DataSet();
           try
           {
               dsEmailProcess = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_EWGetNewEmailRequest");
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsEmailProcess;
       }
       public int SaveEmailField(DataSet dsEmail)
       {
           int errCode = 0;
           try
           {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@MailID", SqlDbType.VarChar);
                parameters[0].Value = dsEmail.Tables[0].Rows[0]["MailID"];

                parameters[1] = new SqlParameter("@Subject", SqlDbType.VarChar);
                parameters[1].Value = dsEmail.Tables[0].Rows[0]["Subject"].ToString();

                parameters[2] = new SqlParameter("@From", SqlDbType.VarChar);
                parameters[2].Value = dsEmail.Tables[0].Rows[0]["From"].ToString();

                parameters[3] = new SqlParameter("@To", SqlDbType.VarChar);
                parameters[3].Value = dsEmail.Tables[0].Rows[0]["To"];

                parameters[4] = new SqlParameter("@Processed", SqlDbType.Bit);
                parameters[4].Value = dsEmail.Tables[0].Rows[0]["Processed"].ToString();

                parameters[5] = new SqlParameter("@StatusId", SqlDbType.Int);
                parameters[5].Value = dsEmail.Tables[0].Rows[0]["StatusId"].ToString();

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;                                                                            
                parameters[6].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveEmailProcess", parameters);
                if (parameters[6].Value != null)
                    return Convert.ToInt32(parameters[6].Value.ToString());
                else
                    return 0;
            
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return errCode;
       }



       public int SaveEmailprocessEvent(DataSet dsEmailFeed, Int16 loaderid)
       {
           int errCode = 0;
           try
           {
               SqlParameter[] parameters = new SqlParameter[6];
               parameters[0] = new SqlParameter("@FeedId", SqlDbType.Int);
               parameters[0].Value = dsEmailFeed.Tables[0].Rows[0]["FeedId"];

               parameters[1] = new SqlParameter("@Subject", SqlDbType.VarChar);
               parameters[1].Value = dsEmailFeed.Tables[3].Rows[0]["Subject"].ToString();

               parameters[2] = new SqlParameter("@LoggerId", SqlDbType.SmallInt);
               parameters[2].Value = loaderid;

               parameters[3] = new SqlParameter("@CreatedBy", SqlDbType.VarChar);
               parameters[3].Value = 1;

               parameters[4] = new SqlParameter("@StatusId", SqlDbType.VarChar);
               parameters[4].Value = 2;

               parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
               parameters[5].Value = 0;
               parameters[5].Direction = ParameterDirection.InputOutput;

               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SaveEmailProcessEventLog", parameters);
               if (parameters[5].Value != null)
                   return Convert.ToInt32(parameters[5].Value.ToString());
               else
                   return 0;

           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return errCode;
       }
    }
}
