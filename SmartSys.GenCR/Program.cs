using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;
using System.Configuration;
using System.Data;
using SmartSys.BL;
using System.Diagnostics;
using System.Threading;
using SmartSys.Utility;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SmartSys.BL.RGS;
using System.Net;
using CrystalDecisions.ReportAppServer.DataDefModel;

//Here is the once-per-application setup information
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace SmartSys.GenCR
{
    class Program
    {
        private static ILog log;


        /// <summary>
        /// This method initilizes the Logger by picking up the cofig file location from configuration.
        /// </summary>
        /// <param name="genNo">This is required to create new logfile for the specific generator.</param>
        private static void InitializeLogger(string genNo)
        {
            if (log4net.LogManager.GetCurrentLoggers().Length == 0)
            {
                string configFile = ConfigurationManager.AppSettings["GenLog4NetConfigFile"];
                log4net.GlobalContext.Properties["InfoLogFileName"] = "Gen_" + genNo + "_InfoLog.log";
                log4net.GlobalContext.Properties["ErrorLogFileName"] = "Gen_" + genNo + "_ErrorLog.log";
                log4net.Config.XmlConfigurator.Configure(new FileInfo(configFile));
                
            }
            log = log4net.LogManager.GetLogger(typeof(Program));
            log.Info("Generator "+ genNo +" process starting up.");
        }
        static void Main(string[] args)
        {
            try
            {

                if (args.Length != 1)
                {
                    Console.WriteLine("Invalid number of arguments Generator can not be initialized. Aborting !!!");
                }
                else
                {
                    if (Convert.ToInt16(args[0]) <= 0)
                    {
                        Console.WriteLine("Invalid arguments Generator can not be initialized. Aborting !!!");
                    }
                }
                InitializeLogger(args[0]);
                InitializeGenerator(Convert.ToInt16(args[0]));
                log.Info("Generator " + args[0] + " process shutting down.");
                
            }
            catch (Exception ex)
            {
                Thread.Sleep(5000);
            }

        }

        /// <summary>
        /// This is the main method to initialize the generator and start polling.
        /// </summary>
        /// <param name="genId"> This Parameter is generator ID if it exists in the processes pool then this process will kill itself. </param>
        private static void InitializeGenerator(int genId)
        {
            try
            {
                System.Data.DataSet dsGenDetails = new System.Data.DataSet();
                GeneratorBL objGen = new GeneratorBL();
                dsGenDetails = objGen.GetGeneratorDetails(genId);
                int processId;
                int errorCode;
                int pollingIntervel;
                if (dsGenDetails == null)
                {
                    log.Error("No Such Generator Found in the metadata. Aborting!!!");
                    return;
                }
                if (dsGenDetails.Tables[0].Rows.Count <= 0)
                {
                    log.Error("No Such Generator Found in the metadata. Aborting!!!");
                    return;
                }

                if(!(bool)dsGenDetails.Tables[0].Rows[0]["isActive"])
                {
                    log.Error("This Generator is deactivated, aborting !!!");
                    return;
                }

                processId = Convert.ToInt32(dsGenDetails.Tables[0].Rows[0]["ProcessId"]);
                pollingIntervel = Convert.ToInt32(dsGenDetails.Tables[0].Rows[0]["PollingInterval"]);
                log.Error("Checking for process ID " + processId.ToString());
                if (ProcessExists(processId))
                {
                    log.Error("Found the process of type CRGenerator, hence Aborting.");
                    return;
                }
                log.Info("process ID " + processId.ToString() + " is not live, hence assigning current processid");
                Process p = Process.GetCurrentProcess();
                processId = p.Id;
                errorCode = objGen.RegisterGenerator(genId, processId);
                if(errorCode == 600003)
                {
                    log.Error("Error occured in RegisterGenerator() Method, Generator ID not found, aborting !!!");
                    return;
                }
                if (errorCode == 600001)
                    log.Info("Generator Registered Successfully.");
                else
                {
                    log.Error("Unknown Error occured in RegisterGenerator() Method, aborting !!!");
                    return;
                }
                log.Info("Starting Generator Polling and Generation");
                GeneratorProcess(genId, pollingIntervel);
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

        private static void GeneratorProcess(int genId, int pollingIntervel)
        {
            try
            {
                //This is Endless Loop to keep Generator alive
                System.Data.DataSet dsJob;
                GeneratorBL objJob = new GeneratorBL();

                while(true)
                {
                    log.Info("Polling for next Reporting job.");
                    
                    dsJob = objJob.GetNewGenRequest(genId);
                    if(dsJob == null)
                    {
                        log.Info("No Job for Processing, Generator will poll again in " + pollingIntervel.ToString() + " Seconds");
                        goto WaitAndGoForNextJob;
                    }
                    if(dsJob.Tables.Count <=1)
                    {
                        log.Error("There was an error while fetching Job");
                        if (dsJob.Tables[0].Rows.Count > 0)
                            log.Error(dsJob.Tables[0].Rows[0]["ErrorDescription"]);
                        log.Info("Generator will poll again in " + pollingIntervel.ToString() + " Seconds");
                        goto WaitAndGoForNextJob;
                    }
                    if (dsJob.Tables[0].Rows.Count > 0)
                    {
                        log.Info("Found one Job with Job Id " + dsJob.Tables[0].Rows[0]["JobId"].ToString() + ", Report Id " + dsJob.Tables[0].Rows[0]["ReportId"].ToString());
                        RunReport(dsJob, genId);
                    }
                    else
                    {
                        log.Info("No Jobs to process, Generator will poll again in " + pollingIntervel.ToString() + " Seconds");
                        goto WaitAndGoForNextJob;
                    }
                    goto NextJob;
                WaitAndGoForNextJob:
                    Thread.Sleep(pollingIntervel * 1000);
                NextJob:
                    continue;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
        private static void RunReport(System.Data.DataSet dsJob, int genId)
        {
            GeneratorBL objRpt = new GeneratorBL();
            int JobId = Convert.ToInt32(dsJob.Tables[0].Rows[0]["JobId"].ToString());
            try
            {
                System.Data.DataSet dsReport;
                
                string Path, ReportId, StagingAreaPath;

                ReportId = dsJob.Tables[0].Rows[0]["ReportId"].ToString();
                dsReport = objRpt.GetReportDetails(ReportId);

                CrystalDecisions.CrystalReports.Engine.ReportDocument rd = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                Path = Common.GetConfigValue("LocalReportTemplateLocation");
                StagingAreaPath = Common.GetConfigValue("StagingAreaLocation");

                log.Info("Checking for Template in " + Path);
                string[] fileattr = dsReport.Tables[0].Rows[0]["FileName"].ToString().Split('.');
                string TemplateName = Path + fileattr[0] + dsReport.Tables[0].Rows[0]["BaseReportId"].ToString() + "_" + dsReport.Tables[0].Rows[0]["Version"].ToString() + "."+ fileattr[1];

                rd.Load(TemplateName);




                foreach (CrystalDecisions.ReportAppServer.DataDefModel.Table table in rd.ReportClientDocument.DatabaseController.Database.Tables)
                {
                    DataRow dr = dsReport.Tables[1].Select("ObjName='" + fileattr[0] + "'")[0];
                    if (dr["ConnectionType"].ToString() == "ODBC")
                    {
                        CrystalDecisions.Shared.ConnectionInfo crConnectionInfo = new CrystalDecisions.Shared.ConnectionInfo();
                        crConnectionInfo.ServerName = dr["ConName"].ToString();
                        crConnectionInfo.DatabaseName = dr["DBName"].ToString();
                        crConnectionInfo.UserID = dr["UserName"].ToString();
                        crConnectionInfo.Password = dr["Password"].ToString();
                        TableLogOnInfo crTableLogoninfo = new TableLogOnInfo();

                        crTableLogoninfo = rd.Database.Tables[table.Name].LogOnInfo;
                        crTableLogoninfo.ConnectionInfo = crConnectionInfo;
                        rd.Database.Tables[table.Name].ApplyLogOnInfo(crTableLogoninfo);
                    }
                    else
                    {
                        PropertyBag connectionAttributes = new PropertyBag();
                        connectionAttributes.Add("Auto Translate", "-1");
                        connectionAttributes.Add("Connect Timeout", "15");
                        connectionAttributes.Add("Data Source", dr["ServerName"].ToString());
                        connectionAttributes.Add("General Timeout", "0");
                        connectionAttributes.Add("Initial Catalog", dr["DBName"].ToString());
                        connectionAttributes.Add("Integrated Security", false);
                        connectionAttributes.Add("Locale Identifier", "1040");
                        connectionAttributes.Add("OLE DB Services", "-5");
                        connectionAttributes.Add("Provider", "SQLOLEDB");
                        connectionAttributes.Add("Tag with column collation when possible", "0");
                        connectionAttributes.Add("Use DSN Default Properties", false);
                        connectionAttributes.Add("Use Encryption for Data", "0");

                        PropertyBag attributes = new PropertyBag();
                        attributes.Add("Database DLL", "crdb_ado.dll");
                        attributes.Add("QE_DatabaseName", dr["DBName"].ToString());
                        attributes.Add("QE_DatabaseType", "OLE DB (ADO)");
                        attributes.Add("QE_LogonProperties", connectionAttributes);
                        attributes.Add("QE_ServerDescription", dr["ServerName"].ToString());
                        attributes.Add("QESQLDB", true);
                        attributes.Add("SSO Enabled", false);

                        CrystalDecisions.ReportAppServer.DataDefModel.ConnectionInfo ci = new CrystalDecisions.ReportAppServer.DataDefModel.ConnectionInfo();
                        ci.Attributes = attributes;
                        ci.Kind = CrConnectionInfoKindEnum.crConnectionInfoKindCRQE;
                        ci.UserName = dr["UserName"].ToString();
                        ci.Password = dr["Password"].ToString();

                        CrystalDecisions.ReportAppServer.DataDefModel.Procedure newTable = new CrystalDecisions.ReportAppServer.DataDefModel.Procedure();
                        newTable.ConnectionInfo = ci;
                        newTable.Name = table.Name;
                        newTable.Alias = table.Alias;
                        newTable.QualifiedName = dr["DBName"].ToString() + ".dbo." + table.Name;
                        rd.ReportClientDocument.DatabaseController.SetTableLocation(table, newTable);
                    }
                }

                foreach (ReportDocument subreport in rd.Subreports)
                {
                    foreach (CrystalDecisions.ReportAppServer.DataDefModel.Table table in rd.ReportClientDocument.SubreportController.GetSubreportDatabase(subreport.Name).Tables)
                    {

                        DataRow dr = dsReport.Tables[1].Select("ObjName='" + subreport.Name + "'")[0];
                        if (dr["ConnectionType"].ToString() == "ODBC")
                        {
                            CrystalDecisions.Shared.ConnectionInfo crConnectionInfo = new CrystalDecisions.Shared.ConnectionInfo();
                            crConnectionInfo.ServerName = dr["ConName"].ToString();
                            crConnectionInfo.DatabaseName = dr["DBName"].ToString();
                            crConnectionInfo.UserID = dr["UserName"].ToString();
                            crConnectionInfo.Password = dr["Password"].ToString();
                            TableLogOnInfo crTableLogoninfo = new TableLogOnInfo();

                            crTableLogoninfo = subreport.Database.Tables[table.Name].LogOnInfo;
                            crTableLogoninfo.ConnectionInfo = crConnectionInfo;
                            subreport.Database.Tables[table.Name].ApplyLogOnInfo(crTableLogoninfo);
                        }
                        else
                        {
                            PropertyBag connectionAttributes = new PropertyBag();
                            connectionAttributes.Add("Auto Translate", "-1");
                            connectionAttributes.Add("Connect Timeout", "15");
                            connectionAttributes.Add("Data Source", dr["ServerName"].ToString());
                            connectionAttributes.Add("General Timeout", "0");
                            connectionAttributes.Add("Initial Catalog", dr["DBName"].ToString());
                            connectionAttributes.Add("Integrated Security", false);
                            connectionAttributes.Add("Locale Identifier", "1040");
                            connectionAttributes.Add("OLE DB Services", "-5");
                            connectionAttributes.Add("Provider", "SQLOLEDB");
                            connectionAttributes.Add("Tag with column collation when possible", "0");
                            connectionAttributes.Add("Use DSN Default Properties", false);
                            connectionAttributes.Add("Use Encryption for Data", "0");

                            PropertyBag attributes = new PropertyBag();
                            attributes.Add("Database DLL", "crdb_ado.dll");
                            attributes.Add("QE_DatabaseName", dr["DBName"].ToString());
                            attributes.Add("QE_DatabaseType", "OLE DB (ADO)");
                            attributes.Add("QE_LogonProperties", connectionAttributes);
                            attributes.Add("QE_ServerDescription", dr["ServerName"].ToString());
                            attributes.Add("QESQLDB", true);
                            attributes.Add("SSO Enabled", false);
                            CrystalDecisions.ReportAppServer.DataDefModel.ConnectionInfo ci = new CrystalDecisions.ReportAppServer.DataDefModel.ConnectionInfo();
                            ci.Attributes = attributes;
                            ci.Kind = CrConnectionInfoKindEnum.crConnectionInfoKindCRQE;
                            ci.UserName = dr["UserName"].ToString();
                            ci.Password = dr["Password"].ToString();


                            CrystalDecisions.ReportAppServer.DataDefModel.Procedure newTable = new CrystalDecisions.ReportAppServer.DataDefModel.Procedure();

                            newTable.ConnectionInfo = ci;
                            newTable.Name = table.Name;
                            newTable.Alias = table.Alias;
                            newTable.QualifiedName = dr["DBName"].ToString() + ".dbo." + table.Name;
                            subreport.ReportClientDocument.SubreportController.SetTableLocation(subreport.Name, table, newTable);

                        }
                    }
                }

                ParameterFieldDefinitions crParameterFieldDefinitions;
                ParameterFieldDefinition crParameterFieldDefinition;
                ParameterValues crParameterValues = new ParameterValues();
                ParameterDiscreteValue crParameterDiscreteValue = new ParameterDiscreteValue();
                crParameterFieldDefinitions = rd.DataDefinition.ParameterFields;
                foreach (DataRow dr in dsReport.Tables[2].Rows)
                {
                    crParameterFieldDefinition = crParameterFieldDefinitions[dr["ParamName"].ToString()];
                    if (crParameterFieldDefinition != null && dr["DefaultValue"].ToString().Trim().Length > 0)
                    {
                        crParameterDiscreteValue.Value = dr["DefaultValue"].ToString();

                        crParameterValues = crParameterFieldDefinition.CurrentValues;
                        crParameterValues.Clear();
                        crParameterValues.Add(crParameterDiscreteValue);
                        crParameterFieldDefinition.ApplyCurrentValues(crParameterValues);
                    }
                }

                foreach (DataRow dr in dsJob.Tables[1].Rows)
                {
                    foreach (ParameterFieldDefinition crParameterdef in crParameterFieldDefinitions) 
                    {
                        if(!crParameterdef.IsLinked())
                        if (crParameterdef.Name == dr["ParamName"].ToString() )
                        {
                            crParameterDiscreteValue.Value = dr["ParamValue"].ToString();

                            crParameterValues = crParameterdef.CurrentValues;
                            crParameterValues.Clear();
                            crParameterValues.Add(crParameterDiscreteValue);
                            crParameterdef.ApplyCurrentValues(crParameterValues);
                        }
                    }
                }
                
                string OutputFileName = ReportId + genId.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyHHMMSS") + Guid.NewGuid().ToString() + ".";
                log.Info("Saving output in Staging Location '" + StagingAreaPath + "', File Name : " + OutputFileName + "PDF");
                rd.ExportToDisk(ExportFormatType.PortableDocFormat,StagingAreaPath + OutputFileName + "PDF" );
                string ftpServer = Common.GetConfigValue("FTP");
                if(ftpServer.Trim().Length>0)
                {
                    log.Info("Request for FTP found. attempt to upload in FTP Location : " + ftpServer);
                    string ftpUid = Common.GetConfigValue("FTPUid");
                    string ftpPwd = Common.GetConfigValue("FTPPWD");
                    FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + OutputFileName + "PDF");
                    requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                    requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                    FileInfo fileInfo = new FileInfo(StagingAreaPath + OutputFileName + "PDF");
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
                }
                objRpt.SetJobStatus(JobId, (int)Enums.StatusCode.GENSU, "Report Successfully Generated", genId, OutputFileName + "PDF", StagingAreaPath, 0);
                log.Info("Job - " + JobId.ToString() + " Status Updated to Generator SUCCESS");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                objRpt.SetJobStatus(JobId, (int)Enums.StatusCode.GENER, ex.Message, genId, "", "", 0);
            }

        }

    }

}
