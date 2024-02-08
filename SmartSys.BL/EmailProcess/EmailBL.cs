using SmartSys.DL.EmailProcess;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.EmailProcess
{
   public class EmailBL
    {
       public DataSet SubjectList()
       {
           DataSet dsSubList = new DataSet();
           try
           {
               EmailDAL objDAl = new EmailDAL();
               dsSubList = objDAl.SubjectList();
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsSubList;
       }
       public DataSet EmailProcess()
       {
           DataSet dsEmailProcess = new DataSet();
           try
           {
               EmailDAL objDAL = new EmailDAL();
               dsEmailProcess = objDAL.EmailProcessData();
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
          
           return dsEmailProcess;
       }

       public DataSet ChkEmailProcessFeed(string subject, string MailID)
       {
           DataSet dsEmailProcess = new DataSet();
           try
           {
               EmailDAL objDAL = new EmailDAL();
               dsEmailProcess = objDAL.ChkEmailProcessFeed(subject, MailID);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }

           return dsEmailProcess;
       }

       public int SaveEmailprocessEvent(DataSet dsEmailFeed, Int16 loaderid)
       {
           int errcode = 0;
           try
           {
               EmailDAL objDAL=new EmailDAL();
               errcode = objDAL.SaveEmailprocessEvent(dsEmailFeed,loaderid);
           }
           catch (Exception)
           {

               throw;
           }
           return errcode;
       }
       public int SaveEmailField(EmailModel emailModel)
       {
           int errCode = 0;
           try
           {
               DataSet dsEmailProcess = new DataSet();
               EmailDAL objDAL=new EmailDAL();
               //Get Dhfeed master details in table;
               DataTable dtEmailProcess = new DataTable();
               dtEmailProcess.Columns.Add("MailID", typeof(System.String));
               dtEmailProcess.Columns.Add("Subject", typeof(System.String));
               dtEmailProcess.Columns.Add("From", typeof(System.String));
               dtEmailProcess.Columns.Add("To", typeof(System.String));
               dtEmailProcess.Columns.Add("Processed", typeof(System.String));
               dtEmailProcess.Columns.Add("StatusId", typeof(System.String));

               DataRow drEmailProcess = dtEmailProcess.NewRow();
               drEmailProcess["MailID"] = emailModel.MailID;
               drEmailProcess["Subject"] = emailModel.Subject;
               drEmailProcess["From"] = emailModel.From;
               drEmailProcess["To"] = emailModel.To;
               drEmailProcess["Processed"] = emailModel.Processed;
               drEmailProcess["StatusId"] = emailModel.StatusId;
               dtEmailProcess.Rows.Add(drEmailProcess);

               dsEmailProcess.Tables.Add(dtEmailProcess);
               errCode = objDAL.SaveEmailField(dsEmailProcess);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return errCode;
       }
    }
}
