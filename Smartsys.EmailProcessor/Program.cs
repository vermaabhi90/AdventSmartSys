using log4net;
using log4net.Config;
using SmartSys.BL;
using SmartSys.BL.EmailProcessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ActiveUp.Net.Mail;
using System.Data;

//Here is the once-per-application setup information
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Smartsys.EmailProcessor
{
    class Program
    {
   
        private static ILog log;
        private static void InitializeLogger(string genNo)
        {
            if (log4net.LogManager.GetCurrentLoggers().Length == 0)
            {
                string configFile = ConfigurationManager.AppSettings["GetLogForMailProcessor"];
                log4net.GlobalContext.Properties["InfoLogFileName"] = "ep_" + genNo + "_InfoLog.log";
                log4net.GlobalContext.Properties["ErrorLogFileName"] = "ep_" + genNo + "_ErrorLog.log";
                log4net.Config.XmlConfigurator.Configure(new FileInfo(configFile));
            }
            log = log4net.LogManager.GetLogger(typeof(Program));
            log.Info("Email " + genNo + " process starting up.");

        }
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid number of arguments EmailProcessor can not be initialized. Aborting !!!");
                return;
            }
            else
            {
                if (Convert.ToInt16(args[0]) <= 0)
                {
                    Console.WriteLine("Invalid arguments EmailProcessor can not be initialized. Aborting !!!");
                    return;
                }
            }
            InitializeLogger(args[0]);
            InitializeEmailProcessor();
            log.Info("Generator " + args[0] + " process shutting down.");

        }
        private static void InitializeEmailProcessor()
        {
            try
            {
                int pollingIntervel;

                pollingIntervel = Convert.ToInt32(ConfigurationManager.AppSettings["Getpollinginterval"]);

                log.Info("Starting EmailProcess Polling and  Saving");
                EmailProcess(pollingIntervel);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        private static void EmailProcess(int pollingIntervel)
        {

            try
            {
                int PollingInternetDelay = Convert.ToInt32(ConfigurationManager.AppSettings["PollingInternetDelay"]);
                WebClient cl = new WebClient();
                try
                {
                    using (var stream = cl.OpenRead("http://www.google.com"))
                    {
                        string Connection = "Successful";
                        log.Info("Internet connection " + Connection);
                        cl.Dispose();
                    }

                }
                catch
                {
                    cl.Dispose();
                    string Connection = "Unsuccessful";
                    log.Error("Internet connection " + Connection);
                    log.Info("Try to connect in 20 minuts");
                    Thread.Sleep(PollingInternetDelay * 60 * 1000);
                    InitializeEmailProcessor();
                }
                MailBL objbl = new MailBL();

                while (true)
                {
                    log.Info("Fetching Employees List having Email Config");
                    List<EmailConfig> listmail = objbl.GetEmployeeMailDetail();
                    log.Info(listmail.Count.ToString() + " Employee(s) found having Email Config");

                    foreach (var emailcnf in listmail)
                    {
                        try
                        {
                            int EmpId = emailcnf.EmpId;
                            log.Info("Fething Mail For This EmpId : " + EmpId + ", Email Server: " + emailcnf.EmailServer + ", Port: " + emailcnf.EmailPort.ToString() + ", Email : " + emailcnf.EmailUserName);
                            string EmailServer = emailcnf.EmailServer;
                            int EmailPort = Convert.ToInt32(emailcnf.EmailPort);
                            string EmailUserName = emailcnf.EmailUserName;
                            string EmailPassword = emailcnf.EmailPassword;
                            var mailRepository = new MailRepository(EmailServer, EmailPort, true, EmailUserName, EmailPassword);
                            log.Info("Mail Box Connected Successfully, Fetching un read mails");
                            List<mailmodel> maillst = new List<mailmodel>();
                            int errcode = mailRepository.GetMails("inbox", "UNSEEN", EmpId, log);
                            
                            if (errcode == 0)
                            {
                                log.Info("All Mails Processed Successfully.");
                            }
                            else
                            {
                                log.Error("problem for Saving Few Mail Details");
                            }
                        }
                        catch (Exception ex)
                        {
                            string sentence = ex.Message;
                            if (sentence.Contains("login"))
                            {
                                if (sentence.Contains("failed"))
                                {
                                    log.Error("Incorrect UserName or Password");

                                }

                            }
                        }
                    }
                    log.Info("System Finished Checking Mails now will stop for " + pollingIntervel.ToString() + " Minutes then will poll again.");
                    Thread.Sleep(pollingIntervel * 60 * 1000);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }
       

    }

    public class MailRepository
    {
        private Imap4Client client;

        public MailRepository(string mailServer, int port, bool ssl, string login, string password)
        {
            try
            {
                if (ssl)

                    Client.ConnectSsl(mailServer, port);
                else
                    Client.Connect(mailServer, port);
                Client.Login(login, password);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public IEnumerable<ActiveUp.Net.Mail.Message> GetAllMails(string mailBox)
        //{
        //    return GetMails(mailBox, "ALL").Cast<ActiveUp.Net.Mail.Message>();
        //}

        //public IEnumerable<ActiveUp.Net.Mail.Message> GetUnreadMails(string mailBox)
        //{
        //    return GetMails(mailBox, "UNSEEN").Cast<ActiveUp.Net.Mail.Message>();
        //}

        protected Imap4Client Client
        {
            get { return client ?? (client = new Imap4Client()); }
        }

        public int GetMails(string mailBox, string searchPhrase, int EmpId, ILog log)
        {
            
            Mailbox mails = Client.SelectMailbox(mailBox);
            int[] ids = mails.Search(searchPhrase);
            log.Info("Total " +ids.Count().ToString() + " unread Messages Found., Now fetching processed emails.");
            MailBL bal = new MailBL();
            int ErrorCode = 0;
            DataSet dsEmailIds = bal.GetEmailIDByEmployee(EmpId);
            log.Info("Processed Messages received successfully.");
            foreach (var messageId in ids)
            {
                if (dsEmailIds != null)
                    if (dsEmailIds.Tables.Count > 0)
                        if (dsEmailIds.Tables[0].Select("ClientMailID=" + messageId.ToString()).Count() > 0)
                            goto SkipEmail;
                log.Info("Processing Email with ID " + messageId.ToString());
                Message message = mails.Fetch.MessageObject(messageId);
                mailmodel model = new mailmodel();
                model.From = message.From.ToString();
                model.Subject = message.Subject.ToString();
                model.Date = message.ReceivedDate;
                string mailaddds = "";
                foreach (Address c in message.To)
                {
                    mailaddds = mailaddds + c.ToString() + ",";
                }
                if (mailaddds.Length > 0)
                    mailaddds = mailaddds.Substring(0, mailaddds.Length - 1);
                model.TO = mailaddds;
                mailaddds = "";
                foreach (Address c in message.Cc)
                {
                    mailaddds = mailaddds + c.ToString() + ",";
                }
                if (mailaddds.Length > 0)
                    mailaddds = mailaddds.Substring(0, mailaddds.Length - 1);
                model.CC = mailaddds;
                mailaddds = "";
                foreach (Address c in message.Bcc)
                {
                    mailaddds = mailaddds + c.ToString() + ",";
                }
                if (mailaddds.Length > 0)
                    mailaddds = mailaddds.Substring(0, mailaddds.Length - 1);
                model.BCC = mailaddds;
                model.BodyText = message.BodyHtml.Text.ToString();
                model.Status = 26;
                model.EmpId = EmpId;
                model.EnquiryId = 0;
                model.Comments = "";
                model.ClientMailId = messageId.ToString();
                var flags = new FlagCollection { "Seen" };
                mails.RemoveFlagsSilent(messageId, flags);

                DataTable dt = new DataTable();
                dt.Columns.Add("Subject");
                dt.Columns.Add("From");
                dt.Columns.Add("To");
                dt.Columns.Add("CC");
                dt.Columns.Add("BCC");
                dt.Columns.Add("BodyText");
                dt.Columns.Add("Date");
                dt.Columns.Add("MailId");
                dt.Columns.Add("Status");
                dt.Columns.Add("EmpId");
                dt.Columns.Add("EnquiryId");
                dt.Columns.Add("Comments");
                dt.Rows.Add(model.Subject, model.From, model.TO, model.CC, model.BCC, model.BodyText, model.Date, model.ClientMailId, model.Status, model.EmpId, model.EnquiryId, model.Comments);
                int ErrCode = bal.SaveMailDetail(dt);

                if (ErrCode != 500000)
                {
                    ErrorCode = 1;
                    log.Info("Message failed to save.");
                }
                else
                    log.Info("Messages Saved successfully.");
            SkipEmail:
                flags = new FlagCollection { "Seen" };
            }

            return ErrorCode;
        }
    }
}
