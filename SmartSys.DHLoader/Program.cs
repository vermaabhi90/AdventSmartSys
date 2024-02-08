using log4net;
using SmartSys.BL.DataHub;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using SmartSys.Utility;
using SmartSys.DL;
using SmartSys.BL.RGS;
using System.Collections;
using SmartSys.BL.SysDBCon;
using SmartSys.DL.Loader;

//Here is the once-per-application setup information

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace SmartSys.DHLoader
{
    class Program
    {
        private static ILog log; 

        private static void InitializeLogger(string LoaderNo)
        {
            if (log4net.LogManager.GetCurrentLoggers().Length == 0)
            {
                string configFile = ConfigurationManager.AppSettings["DHLog4NetConfigFile"];
                log4net.GlobalContext.Properties["InfoLogFileName"] = "Loader_" + LoaderNo + "_InfoLog.log";
                log4net.GlobalContext.Properties["ErrorLogFileName"] = "Loader_" + LoaderNo + "_ErrorLog.log";
                log4net.Config.XmlConfigurator.Configure(new FileInfo(configFile));

            }
            log = log4net.LogManager.GetLogger(typeof(Program));
            log.Info("Loader " + LoaderNo + " process starting up.");
        }      
        static void Main(string[] args)
        {
            try
            {
                if(args.Length != 1)
                {
                 Console.WriteLine("Invalid number of arguments Loader can not be initialized. Aborting !!!");
                }
                else
                {
                    if(Convert.ToInt16(args[0]) <= 0)
                    {
                        Console.WriteLine("Invalid arguments Loader can not be initialized. Aborting !!!");
                    }
                }               
                InitializeLogger(args[0]);
                InitializeLoader(Convert.ToInt16(args[0]));
                log.Info("Loder " + args[0] + " process shutting down.");
               
                Console.ReadLine();
            }
            catch(Exception ex)
            {  
                Thread.Sleep(5000);
                log.Error(ex.Message, ex);
                return;
            }
           
        }       
        public static void InitializeLoader(Int16 DBLoaderId)
        {
            try
            {
                DHLoaderModel dhLoaderModel = new DHLoaderModel();
                DHLoaderBL ObjLoaderBL = new DHLoaderBL();
                dhLoaderModel = ObjLoaderBL.DHLoaderGetSelected(DBLoaderId);
                int errorrCode,iProcessID;
                int EventPollingInterval;
                int JobPollingInterval;
                int TriggerPollingInterval;
                 if (dhLoaderModel == null)
                {
                    log.Error("No such Loader found in metaData. Aborting!!!");
                    return;
                }
                if (dhLoaderModel.isActive != true)
                {
                    log.Error("This Loader is Deactivated, Aborting!!!");
                    return;
                }
                EventPollingInterval = dhLoaderModel.EventPollingInterval;
                JobPollingInterval = dhLoaderModel.JobPollingInterval;
                TriggerPollingInterval = dhLoaderModel.TriggerPollingInterval;
                iProcessID = dhLoaderModel.ProcessID;
                if (ProcessExists(iProcessID))
                {
                    log.Error("Found the process for Xls,CSV,DBLoader hence aborting.....");
                    return;
                }
                log.Info("Process ID " + iProcessID.ToString() + " is not live...Hence assigning current processID");
                Process pr = Process.GetCurrentProcess();
                iProcessID = pr.Id;
                errorrCode = ObjLoaderBL.RegisterLoder(DBLoaderId, iProcessID);
                if (errorrCode == 600001)
                {
                    log.Info("Loader registered successfully...");
                }
                else if (errorrCode == 600003)
                {
                    log.Error("Error in RegisterLoader() method Loader " + DBLoaderId.ToString() + " doesnot exits hence aborting the process.....");
                    return;
                }
                else
                {
                    log.Error("Unkown error occured while registering loader..hence aborting..");
                    return;
                }
                log.Info("Starting the Job polling and Its loading using Loader");
                LoaderProcess(DBLoaderId, JobPollingInterval);
            }
            catch (Exception ex)
            {
               log.Error("Excetpion occured in DBLoader: "+ ex.Message);
            }
        }
      

        private static bool ProcessExists(int iProcessID)
        {
            return Process.GetProcessesByName("XlsAndCSVLoader").Any(p => p.Id == iProcessID);
        }
        public static void LoaderProcess(int DBLoaderId, int JobPollingInterval)
        {
            DataSet dsJobDetails = null;
            DHLoaderBL objBL = new DHLoaderBL();
            while (true)
            {
                try
                {

                    dsJobDetails = objBL.GetJobForLoader(DBLoaderId);
                    if (dsJobDetails.Tables[0].Rows.Count == 0)
                    {
                        log.Info("No job for polling ... polling again for " + JobPollingInterval.ToString() + " seconds..");
                        goto WaitForNextJob;
                    }
                    if (dsJobDetails.Tables[0].Rows.Count > 0 && dsJobDetails.Tables[0].Columns.Count > 1)
                    {
                        log.Info("Found one job with JobID : " + dsJobDetails.Tables[0].Rows[0]["JobId"].ToString() + " and Starting loading..");
                        if (dsJobDetails.Tables[0].Rows[0]["FeedTypecode"].ToString() == "XLSLoader")
                        {
                            LoadXls(dsJobDetails, DBLoaderId);
                        }
                        else if (dsJobDetails.Tables[0].Rows[0]["FeedTypecode"].ToString() == "CSVLoader")
                        {
                            StartCSVLoader(Convert.ToInt16(dsJobDetails.Tables[0].Rows[0]["FeedId"]), Convert.ToInt32(dsJobDetails.Tables[0].Rows[0]["JobId"]), dsJobDetails.Tables[1].Rows[0]["FileName"].ToString());
                        }
                        else if (dsJobDetails.Tables[0].Rows[0]["FeedTypecode"].ToString() == "DBLoader")
                        {
                            StartDBLoader(Convert.ToInt16(dsJobDetails.Tables[0].Rows[0]["FeedId"]), Convert.ToInt32(dsJobDetails.Tables[0].Rows[0]["JobId"]));
                        }
                    }
                    else if (dsJobDetails.Tables[0].Columns.Count == 1 && dsJobDetails.Tables[0].Columns[0].ColumnName == "ErrorDescription")
                    {
                        log.Error("There was a error while fetching job...");
                        log.Info("Loder is polling again for " + JobPollingInterval.ToString() + " seconds");
                        goto WaitForNextJob;
                    }
                    goto NextJob;
                WaitForNextJob:
                    Thread.Sleep(JobPollingInterval * 1000);
                NextJob:
                    continue;
                }

                catch (Exception ex)
                {
                    log.Error("Excetpion occured in DBLoader: " + ex.Message);
                }
            }
        }
        #region CSV Section 
        private static void StartCSVLoader(Int16 FeedId,int JobId,string FileName)
        {
            try
            {
                log.Info("Start CSV Loader For FeedId=" + FeedId + "JobId=" + JobId);
                int errCode = 0;
                DataSet dsObj = new DataSet();
                int error = 0;
                string DestconnectionString = "";
                SmartSys.DL.Loader.ConnectionModel connectionModel = new SmartSys.DL.Loader.ConnectionModel();
                DataSet loaderDS = new DataSet();
                string User_Id = "";
                string proce = "";
                string FTPServerName = "";
                string FTPuserName = "";
                string FTPPass = "";
                LoaderBL BLObj = new LoaderBL();
                dsObj = BLObj.GetDHLoaderParameter(FeedId);
                DataRow dataRow = dsObj.Tables[1].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ActionTypeCode"]) == "DestTable");
                DataRow dr = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "DestSP");
                if (dr != null)
                {
                    proce = dr["Value"].ToString();
                }
                DataRow tempServer = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "FTPServer");
                if (tempServer != null)
                {
                    FTPServerName = tempServer["Value"].ToString();
                }
                DataRow tempUserName = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "FTPUser");
                if (tempUserName != null)
                {
                    FTPuserName = tempUserName["Value"].ToString();
                }
                DataRow tempPass = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "FTPPwd");
                if (tempPass != null)
                {
                    FTPPass = tempPass["Value"].ToString();
                }
                log.Info("Start Downloading File From FTP");
                for (int i = 0; i < 3; i++)
                {
                    errCode = GetDowmloadCSVFile(FTPServerName, FTPuserName, FTPPass, FileName);
                    if (errCode == 60001)
                    {
                        log.Info("File Download Sucessfully From FTP");
                        break;
                    }
                }

                DataTable dt = new DataTable(Convert.ToString(dataRow["Description"]));
                /// Actions Loop
                if (errCode == 60001)
                {
                    for (int i = 0; i < dsObj.Tables[1].Rows.Count; i++)
                    {

                        switch (dsObj.Tables[1].Rows[i]["ActionTypeCode"].ToString())
                        {
                            case "SourceInfo":
                                {
                                    break;
                                }
                            case "DestConn":
                                {
                                    connectionModel = BLObj.GetConnection(FeedId, Enums.DHLoaderKeywords.DestConn.ToString());
                                    DestconnectionString = "Data Source =" + connectionModel.ServerName + ";" + "Initial Catalog =" + connectionModel.DBName + ";" + "uid =" + connectionModel.UserName + ";" + "pwd =" + connectionModel.Password + ";";
                                    break;
                                }
                            case "DestTable":
                                {
                                    dt = CreateDestTable1(dsObj, dt);
                                    break;
                                }
                            case "ProcessData":
                                {
                                    loaderDS = LoadDataintoDestTable(dt, dsObj, FileName);
                                    log.Info("CSV File Data are Successfilly loaded in DataSet");
                                    break;
                                }
                            case "LoadData":
                                {
                                    log.Info("Start CSV File Data are Stored in Database...");
                                    log.Info("Total Number of Rows in Dataset is=" + loaderDS.Tables[0].Rows.Count);
                                    error = SaveLoaderData(DestconnectionString, loaderDS, proce, User_Id, JobId);
                                    break;
                                }
                            default:
                                break;
                        }

                    }
                }
                else
                {
                    log.Info("File Download  not Sucessfully From FTP While Escape the JobId=" + JobId + " and Feed Id =" + FeedId);
                }
            }
            catch (Exception ex)
            {
                log.Info("CSV File Parse not Sucessfully Done While Escape the JobId=" + JobId + " and Feed Id =" +FeedId);
                log.Error(ex.Message, ex);
                UpdateJobStatus(JobId, 600002);
            }           
        }
        private static int SaveLoaderData(string DestconnectionString, DataSet loaderDS, string proce, string User_Id, int JobId)
        {
            int errRows = 0;
            int errCode = 0;
            string errRow = "";
            LoaderBL BLObj = new LoaderBL();
            log.Info("Rows which are not Saved in Data base .....");
            try
            {
                foreach (DataRow dr in loaderDS.Tables[0].Rows)
                {
                    DataSet SaveDataDS = new DataSet();                
                    SaveDataDS.Tables.Add(loaderDS.Tables[0].Clone());                  
                    SaveDataDS.Tables[0].ImportRow(dr);
                    errCode = BLObj.SaveLoaderData(DestconnectionString, SaveDataDS, proce, User_Id, "");
                    if (errCode == 600002)
                    {
                        errRows++;
                        errRow = "";
                        foreach (var item in dr.ItemArray)
                        {
                            errRow = errRow + item.ToString() + "  /";
                        }
                        log.Info(errRow);
                    }
                }
                if (errRows > 0)
                {
                    log.Info("Total Number of Rows which are not Saved in Data base is=" + errRows);
                }
                else
                {
                    log.Info("All rows Successfully Saved in database...");
                }
                log.Info("Update JobId Status to Loader Job Completed Successfuly....");
                UpdateJobStatus(JobId, 600001);
            }
            catch (Exception)
            {
                throw;
            }
            return errCode;
        }

        private static DataTable CreateDestTable1(DataSet dsloaderobj, DataTable dt)
        {
            for (int a = 0; a < dsloaderobj.Tables[0].Rows.Count; a++)
            {
                string COlS = Convert.ToString(dsloaderobj.Tables[0].Rows[a]["Value"]);
                string[] ColSplit = COlS.Split(',');
                if (ColSplit.Length > 1 && dsloaderobj.Tables[0].Rows[a]["ActionTypeCode"].ToString() == "DestTable")
                {
                    dt.Columns.Add(ColSplit[0].Trim(), Type.GetType(ColSplit[1].ToString()));
                }
            }

            return dt;
        }

        private static DataSet LoadDataintoDestTable(DataTable dt, DataSet ds, string fileName)
        {
            DataSet dsLoaderData = new DataSet();
            try
            {
                string fileType = "";
                int NoColInFile = 0;
                int NoHeaderInFile = 0;
                int NoFooterInFile = 0;
                char separator = ' ';
                int Lineseparator = 10;
                DataRow dataRow = null;
                dataRow = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "LineSeparator");
                Lineseparator = Convert.ToInt32(dataRow["Value"].ToString());

                dataRow = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "Separator");
                separator = (Convert.ToChar(dataRow["Value"]));

                dataRow = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "NoHeaderLine");
                NoHeaderInFile = (Convert.ToInt32(dataRow["Value"]));

                dataRow = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "NoFooterLine");
                NoFooterInFile = (Convert.ToInt32(dataRow["Value"]));

                dataRow = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "FileType");
                fileType = (Convert.ToString(dataRow["Value"]));

                dataRow = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "NoColInFile");
                NoColInFile = (Convert.ToInt32(dataRow["Value"]));

                DataRow loaderDR = dt.NewRow();
                DataRow[] drFileType = ds.Tables[0].Select("ParamName<>'ColumnDef' and ActionTypeCode='SourceInfo'");            

                //  var fileName = Path.GetFileName(file.FileName);
                var path = (Common.GetConfigValue("TempLocation").ToString() + fileName);
                //  file.SaveAs(path);
                StreamReader sr = new StreamReader(path);
                String line = sr.ReadToEnd();

                string[] CSVRow = line.Split((Char)Lineseparator);
                sr.Close();
                int CSVlineLen = CSVRow.Length;
                foreach (string demo in CSVRow.Skip(NoHeaderInFile))
                {
                    loaderDR = dt.NewRow();
                    string demoStr = demo.Replace('\r', ' ');
                    string NewMailFrom = Regex.Replace(demoStr, @"""("""")*""", " ");
                    //   demoStr = demo.Replace('\"', ' ');
                    // var result = Regex.Split(demo, ",(?=(?:[^']*[^']*')*[^']*$)");
                    MatchCollection matches = new Regex("((?<=\")[^\"]*(?=\"(,|$)+)|(?<=,|^)[^,\"]*(?=,|$))").Matches(NewMailFrom);
                    string[] CsvData = new string[matches.Count];
                    int i = 0;
                    foreach (var match in matches)
                    {
                        CsvData[i] = match.ToString();
                        i++;
                    }
                    // string[] CsvData = demoStr.Split(separator).Select(x => x.Substring(1, x.Length - 2)).ToArray();
                    DataRow[] drColDef = ds.Tables[0].Select("ParamName ='ColumnDef' and ActionTypeCode='SourceInfo'");
                    foreach (DataRow dr in drColDef)
                    {
                        if (dr["ActionTypeCode"].ToString() == "SourceInfo" && dr["ParamName"].ToString() == "ColumnDef" && demo.Trim() != "")
                        {
                            string COlS = dr["Value"].ToString();
                            string[] ColSplit = COlS.Split(separator);

                            switch (ColSplit[1].ToString())
                            {
                                case "":
                                    {
                                        break;
                                    }
                                case "System.String":
                                    {
                                        loaderDR[ColSplit[0]] = Convert.ToString(CsvData[Convert.ToInt32(ColSplit[2])]);
                                        if (Convert.ToString(ColSplit[0]) == "ImagePath")
                                        {
                                            string str = DownloadAndUploadData(Convert.ToString(CsvData[Convert.ToInt32(ColSplit[2])]));
                                            if (str != "")
                                            {
                                                loaderDR["ImageftpPath"] = str;
                                            }
                                        }
                                        break;
                                    }
                                case "System.Boolean":
                                    {
                                        loaderDR[ColSplit[0]] = Convert.ToBoolean(CsvData[Convert.ToInt32(ColSplit[2])]);
                                        break;
                                    }
                                case "System.Int32":
                                    {
                                        loaderDR[ColSplit[0]] = Convert.ToInt32(CsvData[Convert.ToInt32(ColSplit[2])]);
                                        break;
                                    }
                                case "System.DateTime":
                                    {
                                        loaderDR[ColSplit[0]] = Convert.ToDateTime(CsvData[Convert.ToInt32(ColSplit[2])]);
                                        break;
                                    }
                                case "System.Decimal":
                                    {
                                        loaderDR[ColSplit[0]] = Convert.ToDecimal(CsvData[Convert.ToInt32(ColSplit[2])]);
                                        break;
                                    }
                            }
                        }
                    }
                    dt.Rows.Add(loaderDR);
                }
                dsLoaderData.Tables.Add(dt);
                System.IO.File.Delete(path);            
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsLoaderData;
        }
        private static int GetDowmloadCSVFile(string ServerName, string UserName, string Password, string FileName)
        {          
            try
            {                
                string LocalLocation = Common.GetConfigValue("TempLocation");
                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(ServerName + FileName);
                requestFileDownload.Credentials = new NetworkCredential(UserName,Password);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                FileStream writeStream = new FileStream(LocalLocation + FileName, FileMode.Create);
                int Length = 2048;
                Byte[] buffer = new Byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);
                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }
                responseStream.Close();
                writeStream.Close();
                requestFileDownload = null;
                responseFileDownload = null;               
                return 60001;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return 600002;
        }
        private static string DownloadAndUploadData(string link)
        {
            string Path = "";
            Stream stream = null;
            try
            {
                if (link.Length > 5)
                {
                    link = "http:" + link;
                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(link);
                    WebResponse myResp = myReq.GetResponse();
                    Stream strm = myResp.GetResponseStream();
                    byte[] b = null;
                    using (stream = myResp.GetResponseStream())
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            byte[] buf = new byte[1024];
                            count = stream.Read(buf, 0, 1024);
                            ms.Write(buf, 0, count);
                        } while (stream.CanRead && count > 0);
                        b = ms.ToArray();
                    }
                    string ftpServer = Common.GetConfigValue("FTP");
                    string ftpUid = Common.GetConfigValue("FTPUid");
                    string ftpPwd = Common.GetConfigValue("FTPPWD");
                    string[] FileSplit = link.Split('.');
                    string FileEx = FileSplit[(FileSplit.Length) - 1].ToString();
                    String guid = Guid.NewGuid().ToString();
                    string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                    string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    string FileName = Convert.ToString(FileSplit[1].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
                    FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                    requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                    requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                    Stream uploadStream = null;
                    using (uploadStream = requestFTPUploader.GetRequestStream())
                    {
                        uploadStream.Write(b, 0, b.Length);
                    }
                    uploadStream.Close();
                    requestFTPUploader = null;
                    Path = FileName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Path;
        }
        #endregion CSV Section

        #region DBLoader Section
        private static void ExecuteStoredProcedure(int ActionId, int JobId)
        {
            try
            {
                Int16 ConnectionId = 0;
                string SPName = "";
                DataRow dataRow = null;
                bool isParam = false;
                string TableName = "";

                LoaderBL BLObj = new LoaderBL();
                DataSet dsObj = new DataSet();
                dsObj = BLObj.GetDHActionParameter(ActionId);

                dataRow = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "ExecSPConnId");
                ConnectionId = Convert.ToInt16(dataRow["Value"].ToString());

                dataRow = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "ExceSPName");
                SPName = dataRow["Value"].ToString();
                foreach (DataRow dr in dsObj.Tables[0].Rows)
                {
                    if (dr["ParamName"] == "TableName")
                    {
                        isParam = true;
                        TableName = dataRow["Value"].ToString();
                    }
                }
                DataSet dsParam = new DataSet();
                if (isParam)
                {
                    DataTable DT = new DataTable();
                    DT.Columns.Add("Param", typeof(System.String));

                    DataRow dr = DT.NewRow();
                    dr["Param"] = TableName;
                    DT.Rows.Add(dr);
                    dsParam.Tables.Add(DT);

                }
                else
                {
                    dsParam.Tables.Add();
                }
                ArrayList ConInfo = new ArrayList();
                ConInfo = SourceConnectionConfig(ConnectionId, JobId);
                GeneratorBL objbl = new GeneratorBL();

                objbl.ExecuteUnderlayingSP(dsParam, SPName, ConInfo);

            }
            catch (Exception ex)
            {
                UpdateJobStatus(JobId, 600002);
                log.Error("Excetpion occured: " + ex.Message);
            }
        }

        private static void DBLoaderProcess(int ActionId, int JobId)
        {
            try
            {

                Int16 SourceConnId = 0;
                string SourceSP = "";
                Int16 DestConnId = 0;
                string DestSP = "";
                string DestTempTable = "";
                LoaderBL BLObj = new LoaderBL();
                DataSet dsObj = new DataSet();
                dsObj = BLObj.GetDHActionParameter(ActionId);
                DataRow dataRow = null;
                dataRow = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "SourceConnId");
                SourceConnId = Convert.ToInt16(dataRow["Value"].ToString());

                dataRow = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "SourceSP");
                SourceSP = dataRow["Value"].ToString();

                dataRow = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "DestConnId");
                DestConnId = Convert.ToInt16(dataRow["Value"].ToString());

                dataRow = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "DestSP");
                DestSP = dataRow["Value"].ToString();

                dataRow = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "DestinationTbl");
                DestTempTable = dataRow["Value"].ToString();

                GeneratorBL objbl = new GeneratorBL();
                DataSet dsParam = new DataSet();
                dsParam.Tables.Add();
                log.Info("Get Source Connection Configuration where Connection Id=" + SourceConnId);
                ArrayList ConInfo = new ArrayList();
                ConInfo = SourceConnectionConfig(SourceConnId, JobId);
                DataSet TempdsSource = new DataSet();
                TempdsSource = objbl.ExecuteUnderlayingSP(dsParam, SourceSP, ConInfo);
                if (TempdsSource.Tables[0].Rows.Count > 0)
                {
                    log.Info("Get Source DataSet got" + TempdsSource.Tables[0].Rows.Count + "Rows....");
                }
                else
                {
                    log.Info("Get Source DataSet Do not have Any Rows....");
                }
                ConInfo = DesteConnectionConfig(DestConnId, JobId);
                foreach (DataRow dr in TempdsSource.Tables[0].Rows)
                {

                    string QuaryString = GetQuearyString(dr, DestTempTable, dsObj, JobId);
                    log.Info(QuaryString);
                    objbl.ExecuteUnderlyingCommand(QuaryString, ConInfo);
                }
                int abc = TempdsSource.Tables[0].Rows.Count;
                ConInfo = SourceConnectionConfig(DestConnId, JobId);
                objbl.ExecuteUnderlayingSP(dsParam, DestSP, ConInfo);
            }
            catch (Exception ex)
            {
                UpdateJobStatus(JobId, 600002);
                log.Error("Excetpion occured: " + ex.Message);
            }

        }
        private static ArrayList SourceConnectionConfig(Int16 ConnId, int JobId)
        {
            ArrayList ConnInfo = new ArrayList();
            try
            {
                SysDBConBL objcon = new SysDBConBL();
                SysDBConModel conmodel = objcon.GetSelectedDBConn(ConnId);
                ConnInfo.Add(conmodel.ServerName);
                ConnInfo.Add(conmodel.Port);
                ConnInfo.Add(conmodel.DBName);
                ConnInfo.Add(conmodel.UserName);
                ConnInfo.Add(conmodel.Password);
                ConnInfo.Add(conmodel.ConnectionType);
            }
            catch (Exception ex)
            {
                UpdateJobStatus(JobId, 600002);
                log.Error("Excetpion occured: " + ex.Message);
            }
            return ConnInfo;
        }
        private static ArrayList DesteConnectionConfig(Int16 ConnId, int JobId)
        {
            ArrayList ConnInfo = new ArrayList();
            try
            {
                SysDBConBL objcon = new SysDBConBL();
                SysDBConModel conmodel = objcon.GetSelectedDBConn(ConnId);
                ConnInfo.Add(conmodel.ConnectionType);
                ConnInfo.Add(conmodel.Port);
                ConnInfo.Add(conmodel.ServerName);
                ConnInfo.Add(conmodel.DBName);
                ConnInfo.Add(conmodel.UserName);
                ConnInfo.Add(conmodel.Password);

            }
            catch (Exception ex)
            {
                UpdateJobStatus(JobId, 600002);
                log.Error("Excetpion occured: " + ex.Message);
            }
            return ConnInfo;
        }
        private static String GetQuearyString(DataRow dr, string DestTempTable, DataSet dsObj, int JobId)
        {
            string QuaryString = "";
            try
            {

                QuaryString = "Insert into " + DestTempTable + " ( ";

                for (int a = 0; a < dsObj.Tables[0].Rows.Count; a++)
                {
                    if (dsObj.Tables[0].Rows[a]["ParamName"].ToString() == "ColMapConfig")
                    {
                        string COlS = Convert.ToString(dsObj.Tables[0].Rows[a]["Value"]);
                        string[] ColSplit = COlS.Split(',');
                        QuaryString = QuaryString + "[" + ColSplit[1] + "],";
                    }

                }
                QuaryString = QuaryString.Substring(0, QuaryString.Length - 1);
                QuaryString = QuaryString + " ) Values ( ";

                for (int a = 0; a < dsObj.Tables[0].Rows.Count; a++)
                {
                    if (dsObj.Tables[0].Rows[a]["ParamName"].ToString() == "ColMapConfig")
                    {
                        string COlS = Convert.ToString(dsObj.Tables[0].Rows[a]["Value"]);
                        string[] ColSplit = COlS.Split(',');
                        switch (ColSplit[2].ToString())
                        {
                            case "System.String":
                                QuaryString = QuaryString + "'" + dr[ColSplit[0]].ToString().Replace("'", "''") + "',";
                                break;
                            case "System.Int32":
                                int iOut = 0;
                                if (Int32.TryParse(dr[ColSplit[0]].ToString(), out iOut))
                                    QuaryString = QuaryString + dr[ColSplit[0]] + ",";
                                else
                                    QuaryString = QuaryString + "null" + ",";
                                break;
                            case "System.Boolean":
                                QuaryString = QuaryString + "'" + dr[ColSplit[0]] + "',";
                                break;
                            case "System.DateTime":
                                QuaryString = QuaryString + "'" + dr[ColSplit[0]] + "',";
                                break;
                            case "System.Decimal":
                                decimal dcOut = 0;
                                if (Decimal.TryParse(dr[ColSplit[0]].ToString(), out dcOut))
                                    QuaryString = QuaryString + dr[ColSplit[0]] + ",";
                                else
                                    QuaryString = QuaryString + "null" + ",";
                                break;
                            default:
                                QuaryString = QuaryString + dr[ColSplit[0]] + ",";
                                break;
                        }
                    }

                }
                QuaryString = QuaryString.Substring(0, QuaryString.Length - 1);
                QuaryString = QuaryString + " )";
            }
            catch (Exception ex)
            {
                UpdateJobStatus(JobId, 600002);
                log.Error("Excetpion occured: " + ex.Message);
            }
            return QuaryString;
        }

        private static void StartDBLoader(int FeedId, int JobId)
        {
            try
            {
                LoaderBL BLObj = new LoaderBL();
                DataSet dsObj = new DataSet();
                dsObj = BLObj.GetDHLoaderParameter(FeedId);
                if (dsObj != null)
                {
                    if (dsObj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[1].Rows)
                        {
                            if (dr["ActionTypeCode"].ToString() == "DBLoaderProcess")
                            {
                                DBLoaderProcess(Convert.ToInt32(dr["FActionId"]), JobId);
                                log.Info("Go to Execute DBLoaderProcess() where FActionId is " + Convert.ToInt32(dr["FActionId"]));
                            }
                            else if (dr["ActionTypeCode"].ToString() == "ExceSP")
                            {
                                ExecuteStoredProcedure(Convert.ToInt32(dr["FActionId"]), JobId);
                                log.Info("Go to Execute ExecuteStoredProcedure() where FActionId is " + Convert.ToInt32(dr["FActionId"]));
                            }

                        }
                        log.Info("Update JobId Status to Loader Job Completed Successfuly....");
                        UpdateJobStatus(JobId, 600001);
                    }
                    else
                    {
                        UpdateJobStatus(JobId, 600002);
                        log.Info("There are no ActinTypeCode.");
                    }
                }
                else
                {
                    UpdateJobStatus(JobId, 600002);
                    log.Info("Feed Action list Dataset is Null.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Excetpion occured in DBLoader: " + ex.Message);
            }
        }

        private static void ExcecuteExcelFileLoad()
        {
            string FileNameWithPath = "";

        }
        #endregion DBLoader Section

        #region XLS Section 
        private static void LoadXls(DataSet dsJobDetails,int iLoaderID)
        {
            try
            {
                 DHLoaderBL BLObj = new DHLoaderBL();
                //SmartSys.DL.Loader.LoaderBL BLObj = new DL.Loader.LoaderBL();
                DataSet dsObj = BLObj.GetDHLoaderParameter(Convert.ToInt32(dsJobDetails.Tables[0].Rows[0]["FeedId"].ToString()));
                int ErrorCode = 0;
                DataTable dtDestinationTable = null;
                string strDestconn = "";
                SmartSys.BL.DataHub.ConnectionModel connectionModel = new SmartSys.BL.DataHub.ConnectionModel();
                //SmartSys.DL.Loader.ConnectionModel connectionModel = new SmartSys.DL.Loader.ConnectionModel();
                DataSet dsLoadData = new DataSet();
                XlsSheet xlsSheetObj = null;
                bool isFileDownloaded = false;

                /// Actions Loop
                for (int i = 0; i < dsObj.Tables[1].Rows.Count; i++)
                {

                    switch (dsObj.Tables[1].Rows[i]["ActionTypeCode"].ToString())
                    {
                        case "SourceInfo":
                            {
                                //create new object for each sheet to be exctrated
                                xlsSheetObj = new XlsSheet();
                                dsLoadData = new DataSet();
                                xlsSheetObj.SheetName = dsObj.Tables[1].Rows[i]["description"].ToString().Trim();
                                xlsSheetObj.JobID = Convert.ToInt32(dsJobDetails.Tables[0].Rows[0]["JobId"].ToString());
                                GetSourceXlsInfo(ref xlsSheetObj, dsObj);
                                break;
                            }
                        case "DestConn":
                            {
                                connectionModel = BLObj.GetConnection(Convert.ToInt16(dsJobDetails.Tables[0].Rows[0]["FeedId"].ToString()), Enums.DHLoaderKeywords.DestConn.ToString());
                                strDestconn = "Data Source =" + connectionModel.ServerName + ";" + "Initial Catalog =" + connectionModel.DBName + ";" + "uid =" + connectionModel.UserName + ";" + "pwd =" + connectionModel.Password + ";";
                                break;
                            }
                        case "DestTable":
                            {
                                dtDestinationTable = CreateDestTable(dsObj, xlsSheetObj.SheetName);
                                break;
                            }
                        case "ProcessData":
                            {
                                string[] strArray = System.Reflection.Assembly.GetExecutingAssembly().Location.Split(new string[] { "bin" }, StringSplitOptions.None);
                                xlsSheetObj.LocalFilePath = strArray[0] + "TempFiles\\" + dsJobDetails.Tables[0].Rows[0]["FileName"].ToString();
                                if (!isFileDownloaded)
                                {
                                    DownloadFileFromFTPServer(dsJobDetails.Tables[0].Rows[0]["FileName"].ToString(), xlsSheetObj);
                                    isFileDownloaded = true;
                                }
                                LoadDataIntoDestTable(dtDestinationTable, ref dsLoadData, xlsSheetObj,
                                    Convert.ToInt32(dsJobDetails.Tables[0].Rows[0]["JobId"].ToString()));
                                
                                break;
                            }
                        case "LoadData":
                            {
                                ErrorCode = BLObj.SaveLoaderData(strDestconn, dsLoadData);
                                if (ErrorCode==600001)
                                {
                                    log.Info("Sheet '" + xlsSheetObj.SheetName+ "' extracted successfully...");

                                }
                                else if (ErrorCode==600002)
                                {                                   
                                    log.Error("Some Error occured while inserting data from  Sheet '" + xlsSheetObj.SheetName + "' into database...");
                                }
                                dsLoadData = null;
                                break;
                            }
                        default:
                            break;
                    }
                }
                //update status in dhjob
                UpdateJobStatus(Convert.ToInt32(dsJobDetails.Tables[0].Rows[0]["JobId"].ToString()), ErrorCode);
               
                //Delete the file from the temp folder..
                DeleteFileFromTempFolder(xlsSheetObj);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        public static void DeleteFileFromTempFolder(XlsSheet xlsSheetObj)
        {
            try
            {
                FileInfo file = new FileInfo(xlsSheetObj.LocalFilePath);
                if(file.Exists)
                {
                    file.Delete();
                    log.Info("Local file deleted successfully.....");
                }
            }
            catch(Exception ex)
            {
                log.Error("Some error occured while deleting file from temp folder....");
            }
        }

        private static void UpdateJobStatus(int iJobID,int iErrorCode)
        {
            try
            {
                DHLoaderBL dhLoaderBLObj = new DHLoaderBL();
                dhLoaderBLObj.UpdateJobStatus(iJobID, iErrorCode);
                log.Info("Job Status Updated.....");
            }
            catch(Exception ex)
            {
                log.Error("Some error occured while updating job status..");
            }
        }

        public static void GetSourceXlsInfo(ref XlsSheet xlsSheetObj, DataSet dsObj)
        {
            try
            {
                DataRow[] drSourceInfo = dsObj.Tables[0].Select("ActionTypeCode='SourceInfo' and Description = '" + xlsSheetObj.SheetName + "'");
                foreach (DataRow dr in drSourceInfo)
                {
                    switch (dr["ParamName"].ToString())
                    {
                        case "NoOfSheets":
                            xlsSheetObj.NoOfSheets = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "HeaderStartLineNo":
                            xlsSheetObj.HeaderStartLineInfo = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "DataStartLineNo":
                            xlsSheetObj.DataStartLineInfo = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "DataEndLineInfo":
                            xlsSheetObj.DataEndLineInfo = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "ColumnDef":
                            xlsSheetObj.SourceXlsColumnInfo.Add(dr["Value"].ToString());
                            break;
                        case "FTPServer":
                            xlsSheetObj.FTPServer = dr["Value"].ToString();
                            break;
                        case "FTPUser":
                            xlsSheetObj.FTPUser = dr["Value"].ToString();
                            break;
                        case "FTPPwd":
                            xlsSheetObj.FTPPwd = dr["Value"].ToString();
                            break;
                       
                        default:
                            break;
                    }

                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

        }

        public static DataTable CreateDestTable(DataSet dsObj, string strSheetName)
        {
            DataTable dtDestTable = new DataTable();
            try
            {
                DataRow[] drDestTableInfo = dsObj.Tables[0].Select("ActionTypeCode='DestTable' and Description like '%" + strSheetName + "%'");
                foreach (DataRow dr in drDestTableInfo)
                {
                    if (dr["value"] != null)
                    {
                        string[] arrColumninfo = dr["value"].ToString().Split(',');
                        dtDestTable.Columns.Add(arrColumninfo[0].ToString(), Type.GetType(arrColumninfo[1].ToString()));
                    }
                }
                dtDestTable.TableName = drDestTableInfo[0]["Description"].ToString().Split('.')[1];
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return dtDestTable;
        }
        public static void DownloadFileFromFTPServer(string strFileName, XlsSheet xlsSheetOBJ)
        {
            FtpWebRequest ftpRq;
            try
            {
                		//	"D:\\Aniruddh\\WorkDirMvc5\\SmartSys.DHLoader\\bin\\Debug\\XlsAndCSVLoader.exe\\TempFiles\\salesabhi.xlsx"	string
               // string[] strArray = System.Reflection.Assembly.GetExecutingAssembly().Location.Split(new string[]{"bin"},StringSplitOptions.None);
                //string strFilePath = strArray[0];
               // xlsSheetOBJ.LocalFilePath = strArray[0] + "TempFiles\\" + strFileName;

                FileStream fileStream = new FileStream(xlsSheetOBJ.LocalFilePath, FileMode.Create);
                ftpRq = (FtpWebRequest)FtpWebRequest.Create(new Uri(xlsSheetOBJ.FTPServer) + strFileName);
                ftpRq.Method = WebRequestMethods.Ftp.DownloadFile;
                ftpRq.UseBinary = true;
                ftpRq.Credentials = new NetworkCredential(xlsSheetOBJ.FTPUser, xlsSheetOBJ.FTPPwd);
                FtpWebResponse ftpResponse = (FtpWebResponse)ftpRq.GetResponse();
                Stream ftpStream = ftpResponse.GetResponseStream();

                long c1 = ftpResponse.ContentLength;
                int iBufferSize = 2048;
                int iReadCount;
                byte[] buffer = new byte[iBufferSize];
                iReadCount = ftpStream.Read(buffer, 0, iBufferSize);
                while(iReadCount>0)
                {
                    fileStream.Write(buffer, 0, iReadCount);
                    iReadCount = ftpStream.Read(buffer, 0, iBufferSize);
                }
                ftpStream.Close();
                fileStream.Close();
                ftpResponse.Close();

            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        private static Cell GetCell(string strCellAddress, SheetData sheetdata)
        {
            Cell cl=null;
            try
            {
                uint rowIndex = uint.Parse(Regex.Match(strCellAddress, @"[0-9]+").Value);
                cl= sheetdata.Descendants<Row>().FirstOrDefault(p => p.RowIndex == rowIndex).Descendants<Cell>().FirstOrDefault(c=>c.CellReference==strCellAddress);
            }
            catch(Exception ex)
            {
                log.Error(ex.Message, ex);
            }
            return cl;
        }

        public  static void LoadDataIntoDestTable(DataTable dtDestinationTable,
                                            ref DataSet dsLoadData, XlsSheet xlsSheetObj,int iJobID)
        {
            WorkbookPart workbookPart; List<Row> rows;
            SpreadsheetDocument document = SpreadsheetDocument.Open(xlsSheetObj.LocalFilePath, false);
            try
            {
                //var filename = Path.GetFileName(file.FileName);

               // var filepath = Path.Combine(Server.MapPath("~/App_Data/Uploads"), filename);
                // file.SaveAs(filepath);


                //var data = new XlsData();
               

                //FileStream filStream = new FileStream(filePath, FileMode.Open);

                

                workbookPart = document.WorkbookPart;
                var sheets = workbookPart.Workbook.Descendants<Sheet>();

                //get the sheet by name of the sheet
                var sheet = sheets.FirstOrDefault(s => s.Name == xlsSheetObj.SheetName);

                var workSheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;

                var columns = workSheet.Descendants<Columns>().FirstOrDefault();
                // data.ColumnCofiguration = columns;
                var sheetdata = workSheet.Elements<SheetData>().First();
                rows = sheetdata.Elements<Row>().ToList();


                Dictionary<string, Dictionary<string, string>> DictSourceDestColumnMapping = new Dictionary<string, Dictionary<string, string>>();
                //Dictionary<string, string> DictSourceDestColumnMapping = new Dictionary<string, string>();  

                foreach (string strColumInfo in xlsSheetObj.SourceXlsColumnInfo)
                {
                    string[] strArray = strColumInfo.Split(',');
                    Dictionary<string, string> columnToDataTypeMapping = new Dictionary<string, string>();
                    columnToDataTypeMapping.Add(strArray[0], strArray[1] + "," + strArray[3]);
                    // strArray[2] contains Xls column like A,B,C and  strArray[0] contains Columnname in dest Table
                    if (strArray.Count() >= 2)
                    {
                        DictSourceDestColumnMapping.Add(strArray[2], columnToDataTypeMapping);

                    }
                }

                var regex = new Regex("[A-Za-z]+");
                // var cellFormats = workbookPart.WorkbookStylesPart.Stylesheet.CellFormats;
                // var numberingFormats = workbookPart.WorkbookStylesPart.Stylesheet.NumberingFormats;

                for (int i = xlsSheetObj.DataStartLineInfo; i < rows.Count; i++)
                {
                    Row r = rows[i];
                    DataRow dr = dtDestinationTable.NewRow();
                    bool isWrongData = false;
                   
                    foreach(var key in  DictSourceDestColumnMapping.Keys)
                    {
                        string strCellAddress = key.ToString() + i.ToString();
                        Cell c = GetCell(strCellAddress, sheetdata);

                        var match = regex.Match(strCellAddress);
                        string strColumnName = match.Value;
                        string strText = "";

                        //if cell is null then chk that if it is mandatory. if yes then skip entire row else continue with the next cell
                        // in open xml if cell is not used by user at all then it gives cell as null...
                        //else it gives the object of the cell..hence to check end of the data we need to check for null cells
                        // hence following condtion is added..
                            if (c==null)
                            {
                                if (Convert.ToInt32(DictSourceDestColumnMapping[strColumnName].Values.FirstOrDefault().Split(',')[1].ToString()) == 1)
                                {
                                    isWrongData = true;
                                    log.Error("This row is skipped form extraction due to error: JobID :" +iJobID.ToString()+
                                                "\n Row Number: " + i.ToString() +"\n Column number: " +strCellAddress );
                                    break;
                                }
                                else if (Convert.ToInt32(DictSourceDestColumnMapping[strColumnName].Values.FirstOrDefault().Split(',')[1].ToString()) == 0)
                                {
                                    continue;
                                }
                            }
                   
                           else if (Convert.ToInt32(DictSourceDestColumnMapping[strColumnName].Values.FirstOrDefault().Split(',')[1].ToString())==1
                                && c.CellValue == null)
                            {
                                isWrongData = true;
                                log.Error("This row is skipped form extraction due to error: JobID :" + iJobID.ToString() +
                                                "\n Row Number: " + i.ToString() + "\n Column number: " + strCellAddress);
                                break;
                            }
                           //if column/cell  in xls is not manadatory but still does not contain any data then continue with next cell                            
                            else if (Convert.ToInt32(DictSourceDestColumnMapping[strColumnName].Values.FirstOrDefault().Split(',')[1].ToString()) == 0
                                && c.CellValue == null)
                            {
                                continue;
                            }
                            strText = c.CellValue.InnerXml;
                            if (c.DataType != null && c.DataType.Value == CellValues.SharedString)
                            {
                                var cellvalue = c.CellValue;
                                strText = workbookPart.SharedStringTablePart.SharedStringTable
                                     .Elements<SharedStringItem>().ElementAt(
                                     Convert.ToInt32(c.CellValue.InnerText)).InnerText;

                                dr[DictSourceDestColumnMapping[strColumnName].Keys.FirstOrDefault()] = strText;
                            }
                            // else it is a date and xls stores date in interge format
                            else if (DictSourceDestColumnMapping[strColumnName].Values.FirstOrDefault().Split(',')[0].ToString() == "System.DateTime")
                            {
                                DateTime date;
                                if (DateTime.TryParse(DateTime.FromOADate(Convert.ToDouble(strText)).ToLongDateString(), out date))
                                {
                                    date = DateTime.FromOADate(Convert.ToDouble(strText));
                                    dr[DictSourceDestColumnMapping[strColumnName].Keys.FirstOrDefault()] = date;
                                }
                                //else it is not valid date...
                                else
                                {
                                    isWrongData = true;
                                    log.Error("This row is skipped form extraction due to error - Invalid Date: JobID :" + iJobID.ToString() +
                                               "\n Row Number: " + i.ToString() + "\n Column number: " + strCellAddress);
                                    break;
                                }
                            }
                            // else cell contains interger or decimal value
                            else if (DictSourceDestColumnMapping[strColumnName].Values.FirstOrDefault().Split(',')[0].ToString() == "System.Int32"
                                || DictSourceDestColumnMapping[strColumnName].Values.FirstOrDefault().Split(',')[0].ToString() == "System.Decimal")
                            {
                                //check that cell contains valid integer of decimal data..
                                int iNum = 0; Decimal dNum = 0;
                                if (Decimal.TryParse(strText, out dNum) || Int32.TryParse(strText, out iNum))
                                {
                                    dr[DictSourceDestColumnMapping[strColumnName].Keys.FirstOrDefault()] = strText;
                                }
                                //skip the entire row as one of the cell contains unappropriate data
                                else
                                {
                                    isWrongData = true;
                                    log.Error("This row is skipped form extraction due to error - Invalid Interger/Decimal entry: JobID :"
                                        + iJobID.ToString() +"\n Row Number: " + i.ToString() + "\n Column number: " + strCellAddress);
                                    break;
                                }
                            }
                        //}
                        
                       
                    }
                    //add job ID to each row of the table for future references
                    dr["JobID"] = xlsSheetObj.JobID;
                    if (!isWrongData)
                    {
                        dtDestinationTable.Rows.Add(dr);
                    }
                }
                dsLoadData.Tables.Add(dtDestinationTable);                
            }
        
            catch (Exception ex)
            {
               
                log.Error(ex.Message, ex);
            }
            finally
            {
                document.Close();
            }
           
        }


    }

    /*****************************************
     * This class is for xls sheet in the ls file
     * IT stores all information about the xls sheet
     * 
     **************************************/
    public class XlsSheet
    {
        public int NoOfSheets { get; set; }
        public int HeaderStartLineInfo { get; set; }
        public int DataStartLineInfo { get; set; }
        public int DataEndLineInfo { get; set; }
        public int NumberOfColumns { get; set; }
        public string SheetName { get; set; }
        public string FTPServer { get; set; }
        public string FTPUser { get; set; }
        public string FTPPwd { get; set; }
        public string LocalFilePath { get; set; }

        public int JobID { get; set; }
        public List<string> SourceXlsColumnInfo { get; set; }

        public XlsSheet()
        {
            NoOfSheets = 1;
            HeaderStartLineInfo = 1;
            DataStartLineInfo = 1;
            SourceXlsColumnInfo = new List<string>();
        }
    }
#endregion XLS Section 
}
