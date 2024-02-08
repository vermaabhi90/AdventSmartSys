using ActiveUp.Net.Mail;
using SmartSys.DL.MailProcessing;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.EmailProcessing
{
   public class MailBL
    {

        public int SaveMails(List<mailmodel> maillst)
        {
            int ErrCode = 0;
            try
            {
                MailDal DALObj = new MailDal();
                DataTable dt = new DataTable();
                dt.Columns.Add("Subject");
                dt.Columns.Add("From");
                dt.Columns.Add("To");
                dt.Columns.Add("BodyText");
                dt.Columns.Add("Date");
                dt.Columns.Add("MailId");
                dt.Columns.Add("Status");
                dt.Columns.Add("EmpId");
                dt.Columns.Add("EnquiryId");
                dt.Columns.Add("Comments");

                foreach (mailmodel model in maillst)
                {
                    dt.Rows.Add(model.Subject, model.From, model.TO,model.BodyText, model.Date, model.ClientMailId, model.Status, model.EmpId, model.EnquiryId, model.Comments);
                }
                 ErrCode = DALObj.SaveMailDetail(dt);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }

       public DataSet GetEmailIDByEmployee(int EmpId)
        {
            MailDal dal = new MailDal();
            return dal.GetEmailIDByEmployee(EmpId);
        }

       public int SaveMailDetail(DataTable dt)
       {
           MailDal dal = new MailDal();
           return dal.SaveMailDetail(dt);
       }
        public List<EmailConfig> GetEmployeeMailDetail()
        {
            List<EmailConfig> Lstmail = new List<EmailConfig>();
            try
            {
                DataSet dsobj = new DataSet();
                MailDal DALObj = new MailDal();
                dsobj = DALObj.GetEmplyeeMailList();
                if (dsobj != null)
                {
                    if (dsobj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            EmailConfig EmailCnf = new EmailConfig();
                            EmailCnf.EmailUserName = dr["EmailUserName"].ToString();
                            EmailCnf.EmailPassword = dr["EmailPassword"].ToString();
                            EmailCnf.EmailPort = dr["EmailPort"].ToString();
                            EmailCnf.EmailServer = dr["EmailServer"].ToString();
                            EmailCnf.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            Lstmail.Add(EmailCnf);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Lstmail;
        }
    }
  
}
