using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.Configuration;
using System.IO;
using System.Threading;
using SmartSys.BL.RDS;
using System.Data;
using SmartSys.Utility;
using SmartSys.BL;
using SmartSys.BL.RGS;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace SmartSys.RDSEngine
{
    public class RDSDlyGenSub
    {
        public int DlyGenSubId { get; set; }
        public int RptSubId { get; set; }
        public int ClientId { get; set; }
        public string ReportId { get; set; }
    }

    class Program
    {
        private static ILog log;
        private static bool _PrimaryRDSEngine;
        private static int _ProcessPollingInterval;
        private static int _EventPollingIntterval;
        private static int _TriggerPollingInterval;
        private static int _JobPollingInterval;

        public static int QueueLength;


        /// <summary>
        /// This method initilizes the Logger by picking up the cofig file location from configuration.
        /// </summary>
        /// <param name="genNo">This is required to create new logfile for the specific generator.</param>
        private static void InitializeLogger(string EngineNo)
        {
            if (log4net.LogManager.GetCurrentLoggers().Length == 0)
            {
                string configFile = ConfigurationManager.AppSettings["GenLog4NetConfigFile"];
                log4net.GlobalContext.Properties["InfoLogFileName"] = "RDSEngine_" + EngineNo + "_InfoLog.log";
                log4net.GlobalContext.Properties["ErrorLogFileName"] = "RDSEngine_" + EngineNo + "_ErrorLog.log";
                log4net.Config.XmlConfigurator.Configure(new FileInfo(configFile));

            }
            log = log4net.LogManager.GetLogger(typeof(Program));
            log.Info("Generator " + EngineNo + " process starting up.");
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid number of arguments RDS Engine can not be initialized. Aborting !!!");
                Thread.Sleep(5000);
                return;
            }
            else
            {
                if (Convert.ToInt16(args[0]) <= 0)
                {
                    Console.WriteLine("Invalid arguments RDS Engine can not be initialized. Aborting !!!");
                    Thread.Sleep(5000);
                    return;
                }
            }
            InitializeLogger(args[0]);
            InitializeRDSEngine(Convert.ToInt16(args[0]));
            log.Info("RDS Engine " + args[0] + " process shutting down.");
            Thread.Sleep(5000);
        }

        private static void InitializeRDSEngine(Int16 EngineNo)
        {
            try
            {
                _PrimaryRDSEngine = false;
                RDSEngineBL objBL = new RDSEngineBL();
                log.Info("Fetching RDS Engine Configuration for Engine no : " + EngineNo.ToString());
                DataSet dsEngine = objBL.GetRDSEngineInfo(EngineNo);
                int processId;
                if (dsEngine.Tables.Count<=0)
                {
                    log.Error("Invalid Engine No " + EngineNo.ToString() + ". Aborting !!!");
                    return;
                }
                if(dsEngine.Tables[0].Rows.Count<=0)
                {
                    log.Error("Invalid Engine No " + EngineNo.ToString() + ". Aborting !!!");
                    return;
                }
                log.Info("Checking the Host Validation for the RDS engine no "  + EngineNo.ToString());
                if(dsEngine.Tables[0].Rows[0]["Host"].ToString() != Environment.MachineName)
                {
                    log.Error("This Engine " + EngineNo.ToString() + "(" + dsEngine.Tables[0].Rows[0]["Host"].ToString() + ") is not configured for this host (" + Environment.MachineName + ") Aborting !!!");
                    return;
                }
                processId = Convert.ToInt32(dsEngine.Tables[0].Rows[0]["ProcessId"]);
                log.Error("Checking for process ID " + processId.ToString());
                if (ProcessExists(processId))
                {
                    log.Error("Found the process of type RDSEngine, hence Aborting.");
                    return;
                }
                Process p = Process.GetCurrentProcess();
                processId = p.Id;
                int errorCode = objBL.RegisterRDSEngine(EngineNo, processId);
                if (errorCode == 600003)
                {
                    log.Error("Error occured in RegisterGenerator() Method, RDS Engine ID not found, aborting !!!");
                    return;
                }
                if (errorCode == 600001)
                    log.Info("RDS Engine Registered Successfully.");
                else
                {
                    log.Error("Unknown Error occured in RegisterRDSEngine() Method, aborting !!!");
                    return;
                }

                log.Info("Engine Validation is complete Starting Engine Main Process");
                _ProcessPollingInterval = Convert.ToInt32( dsEngine.Tables[0].Rows[0]["ProcessPollingInterval"].ToString());
                _JobPollingInterval = Convert.ToInt32(dsEngine.Tables[0].Rows[0]["JobPollingInterval"].ToString());
                _EventPollingIntterval = Convert.ToInt32(dsEngine.Tables[0].Rows[0]["EventPollingIntterval"].ToString());
                _TriggerPollingInterval = Convert.ToInt32(dsEngine.Tables[0].Rows[0]["TriggerPollingInterval"].ToString());
                RDSMainEngineProcess(EngineNo);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
        private static bool ProcessExists(int id)
        {
            return Process.GetProcessesByName("CRGenerator").Any(x => x.Id == id);
        }
        public static void ProcessJob(Object obj)
        {
            int StatusId, StatusPolCountFail = 0; 
            int StatusPolCount = 0;
            int maxRunTime = Convert.ToInt32(Common.GetConfigValue("MaxReportRunTime")) * 6;

            log.Info("Thread " + Thread.CurrentThread.GetHashCode() + " consumes " + ((DataRow)obj)["DlyGenSubId"].ToString());

            DataRow dr = ((DataRow)obj);
            RDSEngineBL objbl = new RDSEngineBL();
            int jobId = objbl.RDSProcessandCreateRGSJob(Convert.ToInt32(((DataRow)obj)["DlyGenSubId"].ToString()));
            GeneratorBL objRGSBL = new GeneratorBL();
            while (true)
            {
                StatusPolCount++;
                StatusId = objRGSBL.GetJobStatus(jobId);
                if (StatusId == 9)
                {
                    //Generation Successfully Complete Update the Dly Gen Sub status
                    log.Info("Generation of Daily Gen Sub Id " + ((DataRow)obj)["DlyGenSubId"].ToString() + "Is Complete updating status to Success ");
                    objbl.SetDLYGenSubStatus(Convert.ToInt32(((DataRow)obj)["DlyGenSubId"].ToString()), (Int16)Enums.StatusCode.SU);
                    log.Info("Gen Sub Id " + ((DataRow)obj)["DlyGenSubId"].ToString() + " status updated to Success ");
                    RDSDistributer(Convert.ToInt32(((DataRow)obj)["DlyGenSubId"].ToString()));
                    break;
                }
                if (StatusId == 10)
                {
                    //Do Something as there is some error in Generation update status to error
                    log.Info("There was some error in Generation of Daily Gen Sub Id " + ((DataRow)obj)["DlyGenSubId"].ToString() + "Is Complete updating status to Error ");
                    objbl.SetDLYGenSubStatus(Convert.ToInt32(((DataRow)obj)["DlyGenSubId"].ToString()), (Int16)Enums.StatusCode.GENER);
                    log.Info("Gen Sub Id " + ((DataRow)obj)["DlyGenSubId"].ToString() + " status updated to Error ");
                    break;
                }
                if (StatusId == 0 && StatusPolCountFail > 5)
                {
                    //Do Something as unable to poll status of the RGS Job. -- Set status RGS Job Status Polling Error
                    log.Info("Unable to poll for Gen Sub Id " + ((DataRow)obj)["DlyGenSubId"].ToString() + " status updated to Polling Error ");
                    objbl.SetDLYGenSubStatus(Convert.ToInt32(((DataRow)obj)["DlyGenSubId"].ToString()), (Int16)Enums.StatusCode.PLER);
                    log.Info("Unable to update status for  Gen Sub Id " + ((DataRow)obj)["DlyGenSubId"].ToString() + " to Polling Error ");
                    break;
                }
                if (StatusId == 0)
                {
                    log.Info("Unable to poll for Gen Sub Id " + ((DataRow)obj)["DlyGenSubId"].ToString() + ", System will again attempt " + (5 - StatusPolCountFail) + " Times ");
                    StatusPolCountFail++;
                }
                if (StatusPolCount > maxRunTime)
                {
                    //Do Something Max Run Time Arrived
                    log.Info("Gen Sub Id " + ((DataRow)obj)["DlyGenSubId"].ToString() + " identified as Long Running updating status to max run error ");
                    objbl.SetDLYGenSubStatus(Convert.ToInt32(((DataRow)obj)["DlyGenSubId"].ToString()), (Int16)Enums.StatusCode.MAXRU);
                    log.Info("Gen Sub Id " + ((DataRow)obj)["DlyGenSubId"].ToString() + " status update to max run error ");
                    break;
                }
                log.Info("Gen Sub Id " + ((DataRow)obj)["DlyGenSubId"].ToString() + " Still in Running Status, System will again attempt " + (maxRunTime - StatusPolCount) + " Times ");
                Thread.Sleep(10000);
            }
            QueueLength--;
        }
        private static void RDSRedistribution()
        {
            while (true)
            {
                try
                {
                    RDSEngineBL objBL = new RDSEngineBL();
                    log.Info("Fetching RDS Re-Distribution List...");
                    DataSet dsEngine = objBL.GetRDSReDistributionList();
                    if (dsEngine != null)
                    {
                        if (dsEngine.Tables.Count > 0)
                        {
                            if (dsEngine.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow dr in dsEngine.Tables[0].Rows)
                                {
                                    log.Info("DlyGenSubId =" + dr["DlyGenSubId"].ToString() + " Start for Re-Distribution");
                                    RDSDistributer(Convert.ToInt32(dr["DlyGenSubId"].ToString()));
                                }

                            }
                        }
                    }
                    Thread.Sleep(1000 * _ProcessPollingInterval);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    Thread.Sleep(1000 * _ProcessPollingInterval);
                }
            }
        }
        private static void RDSMainEngineProcess(Int16 EngineNo)
        {
            try
            {

                ThreadStart REDistribution = new ThreadStart(RDSRedistribution);
                Thread Child = new Thread(REDistribution);
                Child.Start();
                Int16 NoofThreads = 0;
                NoofThreads = Convert.ToInt16(Common.GetConfigValue("RDSJobThreads"));
                log.Info("Starting Heartbeat Sender Thread.");
                Thread t_HeartBeatSender = new Thread(() => RDSHeartBeatSender(EngineNo));
                t_HeartBeatSender.Start();
                RDSEngineBL objBL = new RDSEngineBL();
                DataSet dsJobs;
                while(true)
                {
                    try
                    {

                        if (_PrimaryRDSEngine == true)
                        {
                            QueueLength = 0;
                            log.Info("This is Primary RDS Engine now Starting " + NoofThreads.ToString() + " Threads pool");
                            dsJobs = objBL.GetJobs(EngineNo);
                            foreach (DataRow dr in dsJobs.Tables[0].Rows)
                            {
                                QueueLength++;
                                ThreadPool.SetMaxThreads(5, 5);
                                ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessJob), dr);
                            }
                            while (QueueLength >= 1)
                            {
                                log.Info("Total " + QueueLength + " Jobs are in process");
                                int workThrd, compPortThrd;
                                ThreadPool.GetAvailableThreads(out workThrd, out compPortThrd);
                                //Console.WriteLine("Going to wait Thread# " + workThrd.ToString() + "," + compPortThrd.ToString());
                                Thread.Sleep(700);
                            }
                        }
                        Thread.Sleep(1000 * _ProcessPollingInterval);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message, ex);
                        log.Info("System will attempt again in some time.");
                        Thread.Sleep(1000 * _ProcessPollingInterval);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// HeartBeat Sender will keep checking if the Primary Engine is Live or not if if its not then this will become primary and keep sending its live status.
        /// </summary>
        /// <param name="EngineNo"></param>
        private static void RDSHeartBeatSender(Int16 EngineNo)
        {
            try
            {
                while (true)
                {
                    AdminBL objBL = new AdminBL();
                    log.Info("Sending Engine HeartBeat for RDS Engine No - " + EngineNo.ToString());
                    int errorCode = objBL.SendEngineHeartBeat(EngineNo, "RDS");
                    if (errorCode == 500001) 
                    {
                        _PrimaryRDSEngine = true;
                    }
                    if (errorCode == 500002)
                    {
                        log.Info("There Seems no Live primary RDS Engine, hence making this engine as live RDS Engine.");
                        _PrimaryRDSEngine = true;
                    }
                    if (errorCode == 500003)
                    {
                        log.Info("Heart beat sent successfully, Primary RDS Engine is Live running fine.");
                        _PrimaryRDSEngine = false;
                    }
                    Thread.Sleep(1000 * Convert.ToInt32(Common.GetConfigValue("RDSHeartBeatSendInterval")));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
        private static void RDSEventListner(Int16 EngineNo)
        {
            try
            {

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
        private static void RDSTriggerListner(Int16 EngineNo)
        {
            try
            {

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
        private static void RDSDistributer(Int32 DlyGenSubId)
        {
            RDSEngineBL objBL = new RDSEngineBL(); 
            try
            {

                DataSet dsData = new DataSet();
                dsData = objBL.GetDistDetails(DlyGenSubId);
                int ErrCode = 0;
                if (dsData != null)
                    if (dsData.Tables.Count>0)
                        foreach (DataRow dr in dsData.Tables[0].Rows)
                        {
                            if (Convert.ToBoolean(dr["isEmail"]))
                            {
                                if (dr["OutputFileName"].ToString() != "")
                                {
                                    String localPath = Common.GetConfigValue("LocalPath");
                                    String StagingAreaLocation = Common.GetConfigValue("StagingAreaLocation");
                                    if (File.Exists(StagingAreaLocation + dr["OutputFileName"].ToString()))
                                    {
                                        string fileName = dr["OutputFileName"].ToString();
                                        string sourcePath = StagingAreaLocation;
                                        string targetPath = localPath;

                                        string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                                        string destFile = System.IO.Path.Combine(targetPath, fileName);
                                        if (!System.IO.Directory.Exists(targetPath))
                                        {
                                            System.IO.Directory.CreateDirectory(targetPath);
                                        }
                                        System.IO.File.Copy(sourceFile, destFile, true);
                                        ErrCode = 500004;
                                    }
                                    else
                                    {
                                        for (int i = 0; i <= 3; i++)
                                        {
                                            ErrCode = DownloadFileFrmServer(dr["OutputFileName"].ToString());
                                            if (ErrCode == 500004)
                                            {
                                                break;
                                            }
                                            else if (i == 2 && ErrCode != 500004)
                                            {
                                                objBL.SetDLYGenSubStatus(DlyGenSubId, (Int16)Enums.StatusCode.RDSDowldErr);
                                            }
                                        }

                                    }
                                   
                                }
                                else
                                {
                                    log.Error("DlyGenSubId =" + dr["OutputFileName"].ToString() + "have Empty Output File Name");
                                }
                               
                                if (dr["emailAddress"].ToString() != "" && ErrCode == 500004)
                                {
                                    ErrCode = SendMail(dr["OutputFileName"].ToString(), dr["emailAddress"].ToString(), DlyGenSubId, dr["ReportName"].ToString());
                                    if (ErrCode == 500001)
                                    {
                                        log.Info("DlyGenSubId =" + dr["OutputFileName"].ToString() + "Successfully Complete Email Process..");
                                    }
                                    else
                                    {
                                        log.Error("DlyGenSubId =" + dr["OutputFileName"].ToString() + "Not Successfully Complete Email Process..");
                                    }
                                }
                                else
                                {
                                    log.Error("DlyGenSubId =" + DlyGenSubId + "Email Address is not avaliable.... or FTP File Download  not Successful");
                                }


                                if (dr["FTPDetails"].ToString() != "")
                                {
                                    string[] FTPDetails = dr["FTPDetails"].ToString().Split(',');
                                    int len = FTPDetails.Length;
                                    if (len == 3)
                                    {
                                        string server = FTPDetails[0].ToString();
                                        string UserName = FTPDetails[1].ToString();
                                        string Password = FTPDetails[2].ToString();
                                        ErrCode = UploadFileFrmClientServer(server, UserName, Password, dr["OutputFileName"].ToString());
                                        if (ErrCode == 500001)
                                        {
                                            log.Info("DlyGenSubId =" + dr["OutputFileName"].ToString() + "Successfully Complete FTP Client Side..");
                                        }
                                        else
                                        {
                                            log.Info("DlyGenSubId =" + dr["OutputFileName"].ToString() + " Not Successfully Complete FTP Client Side..");
                                        }
                                    }
                                }
                            }
                            if (ErrCode == 500001)
                            {
                                RDSReportBL ObjBL = new RDSReportBL();
                                int success = ObjBL.UpdateStatus(DlyGenSubId);
                                if (success == 500001)
                                {
                                     String localPath = Common.GetConfigValue("LocalPath");
                                     string dir = localPath + dr["OutputFileName"].ToString();
                                   File.Delete(localPath + dr["OutputFileName"].ToString());
                                }                             
                            }                        
                        }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                objBL.SetDLYGenSubStatus(DlyGenSubId, (Int16)Enums.StatusCode.RDSERR);
            }
        }
        private static int UploadFileFrmClientServer(string server, string UserName, string Password,string FileName)
        {
            int ErrCode = 0;
            try
            {
                String localPath = Common.GetConfigValue("LocalPath");
                FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(server + FileName);
                requestFTPUploader.Credentials = new NetworkCredential(UserName, Password);
                requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;

                FileInfo fileInfo = new FileInfo(localPath + "\\" + FileName);
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
                ErrCode = 500001;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return ErrCode;
        }
        private static int  DownloadFileFrmServer(string FileName)
        {
            int errCode = 0;
            try
            {
                String FTPServer = Common.GetConfigValue("FTP");
                String FTPUserId = Common.GetConfigValue("FTPUid");
                String FTPPwd = Common.GetConfigValue("FTPPWD");
                String localPath = Common.GetConfigValue("LocalPath");
               
                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + FileName);
                requestFileDownload.UseBinary = true;
                requestFileDownload.KeepAlive = false;
                requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                FileStream writeStream = new FileStream(localPath + FileName, FileMode.Create);

                int Length = 2048;
                Byte[] buffer = new Byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);

                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }
                writeStream.Close();
                writeStream.Dispose();
                requestFileDownload = null;
                responseFileDownload = null;
               

                responseStream.Close();
                responseStream.Dispose();
                errCode = 500004;
            }
            catch (Exception ex)
            { 
                log.Error(ex.Message, ex);
            }
           return errCode;
        }
        private static int SendMail(string Filename, string toMailAdd, Int32 DlyGenSubId, string ReportName)
        {
            int ErrCode = 0;
            RDSEngineBL objBL = new RDSEngineBL(); 
            try
            {
                String Server = Common.GetConfigValue("EmailServer");
                String username = Common.GetConfigValue("EmailUserName");
                String password = Common.GetConfigValue("EmailPassword");
                String localPath = Common.GetConfigValue("LocalPath");
                String EmailPort = Common.GetConfigValue("EmailPort");
                String SupportMail = Common.GetConfigValue("SupportMail");
                String emailSSL = Common.GetConfigValue("emailSSL");

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Server);

                mail.From = new MailAddress(username);
                mail.To.Add(toMailAdd);
                mail.To.Add(SupportMail);
                mail.Subject = "Report Delevery from SmartSys for " + ReportName;
                mail.Body = "This is Auto Genrated Mail Please Don't Response this mail";
                Attachment attachment = new Attachment(localPath + Filename);
                mail.Attachments.Add(attachment);

                SmtpServer.Port = Convert.ToInt32(EmailPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
                SmtpServer.EnableSsl = Convert.ToBoolean(emailSSL);
                SmtpServer.Send(mail);
                mail.Dispose();
                mail = null;
                ErrCode = 500001; 
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                objBL.SetDLYGenSubStatus(DlyGenSubId,(Int16)Enums.StatusCode.InVLdEmail);
                throw ex;
            }
            return ErrCode;
        }
    }
}
