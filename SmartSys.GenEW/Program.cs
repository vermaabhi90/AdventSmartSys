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
using SmartSys.BL.RGS;
using System.Collections;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Net;

//Here is the once-per-application setup information
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace SmartSys.GenEW
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
            log.Info("Generator " + genNo + " process starting up.");
        }
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid number of arguments Generator can not be initialized. Aborting !!!");
                return;
            }
            else
            {
                if (Convert.ToInt16(args[0]) <= 0)
                {
                    Console.WriteLine("Invalid arguments Generator can not be initialized. Aborting !!!");
                    return;
                }
            }
            InitializeLogger(args[0]);
            InitializeGenerator(Convert.ToInt16(args[0]));
            log.Info("Generator " + args[0] + " process shutting down.");
        }


        /// <summary>
        /// This is the main method to initialize the generator and start polling.
        /// </summary>
        /// <param name="genId"> This Parameter is generator ID if it exists in the processes pool then this process will kill itself. </param>
        private static void InitializeGenerator(int genId)
        {
            try
            {
                DataSet dsGenDetails = new DataSet();
                GeneratorBL objGen = new GeneratorBL();
                dsGenDetails = objGen.GetGeneratorDetails(genId);
                int processId;
                int errorCode;
                int pollingIntervel;
                if (dsGenDetails == null)
                {
                    log.Error("No Such EW Generator Found in the metadata. Aborting!!!");
                    return;
                }
                if (dsGenDetails.Tables[0].Rows.Count <= 0)
                {
                    log.Error("No Such EW Generator Found in the metadata. Aborting!!!");
                    return;
                }

                if (!(bool)dsGenDetails.Tables[0].Rows[0]["isActive"])
                {
                    log.Error("This EW Generator is deactivated, aborting !!!");
                    return;
                }

                processId = Convert.ToInt32(dsGenDetails.Tables[0].Rows[0]["ProcessId"]);
                pollingIntervel = Convert.ToInt32(dsGenDetails.Tables[0].Rows[0]["PollingInterval"]);
                log.Info("Checking for process ID " + processId.ToString());
                if (ProcessExists(processId))
                {
                    log.Error("Found the process of type EWGenerator, hence Aborting.");
                    return;
                }
                log.Info("process ID " + processId.ToString() + " is not live, hence assigning current processid");
                Process p = Process.GetCurrentProcess();
                processId = p.Id;
                errorCode = objGen.RegisterGenerator(genId, processId);
                if (errorCode == 600003)
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
            return Process.GetProcessesByName("EWGenerator").Any(x => x.Id == id);
        }

        private static void GeneratorProcess(int genId, int pollingIntervel)
        {
            try
            {
                //This is Endless Loop to keep Generator alive
                DataSet dsJob;
                GeneratorBL objJob = new GeneratorBL();

                while (true)
                {
                    log.Info("Polling for next Reporting job.");
                    dsJob = objJob.GetNewGenRequest(genId);
                    if (dsJob == null)
                    {
                        log.Info("No Job for Processing, Generator will poll again in " + pollingIntervel.ToString() + " Seconds");
                        goto WaitAndGoForNextJob;
                    }
                    if (dsJob.Tables.Count <= 1)
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
        private static void RunReport(DataSet dsJob, int genId)
        {
            GeneratorBL objRpt = new GeneratorBL();

            try
            {
                DataSet dsReport;
                
                string ReportId;
                ReportId = dsJob.Tables[0].Rows[0]["ReportId"].ToString();
                log.Info("Fetching Report Details.. for ReportId - " + ReportId);
                dsReport = objRpt.GetReportDetails(ReportId);
                if (dsReport == null)
                {
                    log.Info("Unable to retrive Report details for ReportId - " + ReportId + ", aborting generation!");
                    objRpt.SetJobStatus(Convert.ToInt32(dsJob.Tables[0].Rows[0]["JobId"].ToString()), (int)Enums.StatusCode.GENER, "Unable to retrive Report details for ReportId - " + ReportId + ", aborting generation!",  genId,"","",0);

                }

                GenerateExcelReport(dsReport,dsJob,genId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                objRpt.SetJobStatus(Convert.ToInt32(dsJob.Tables[0].Rows[0]["JobId"].ToString()), (int)Enums.StatusCode.GENER, ex.Message, genId, "", "", 0);
            }
        }

        public static Application StartExcel()
        {
            Application instance = null;
            try
            {
                instance = (Application)System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application");
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                instance = new Application();
            }

            return instance;
        }
        private static void GenerateExcelReport(DataSet dsMetadata, DataSet dsJob, int genId)
        {
            string Path;
            Path = Common.GetConfigValue("LocalReportTemplateLocation");
            Application xlApp = new Excel.Application();

            log.Info("Checking for Template in " + Path);
            string[] fileattr = dsMetadata.Tables[0].Rows[0]["FileName"].ToString().Split('.');
            string TemplateName = Path + fileattr[0] + dsMetadata.Tables[0].Rows[0]["BaseReportId"].ToString() + "_" + dsMetadata.Tables[0].Rows[0]["Version"].ToString() + "." + fileattr[1];
            Workbook xlWorkBook = xlApp.Workbooks.Open(TemplateName, ReadOnly: true);
             
            GeneratorBL objBL = new GeneratorBL();
            int JobId = Convert.ToInt32(dsJob.Tables[0].Rows[0]["JobId"].ToString());
            DataSet dsParam = new DataSet();
           
            try
            {
                DataSet dsData = new DataSet();
                dsParam.Tables.Add(dsJob.Tables[1].Copy());
                ArrayList lstMarkers = new ArrayList();
                ArrayList lstMarkerRows = new ArrayList();
                ArrayList lstMarkerRange = new ArrayList();
                ArrayList ConInfo = new ArrayList();
                string FileExt = fileattr[1];


                foreach (DataRow drMarker in dsMetadata.Tables[1].Rows)
                {
                    DataSet dsMarkerDBParam = new DataSet();
                    dsMarkerDBParam = dsParam.Clone();
                    
                    foreach (DataRow drp in dsMetadata.Tables[2].Select("ObjName='" + drMarker["ObjName"].ToString() + "'"))
                    {
                        bool foundflg = false;
                        DataRow newrow = dsMarkerDBParam.Tables[0].NewRow();
                        foreach (DataRow drNew in dsParam.Tables[0].Select("ParamName='" + drp["ParamName"].ToString() + "'"))
                        {
                            newrow["JobId"] = dsJob.Tables[0].Rows[0]["JobId"].ToString();
                            newrow["ParamName"] = drNew["ParamName"].ToString();
                            newrow["ParamValue"] = drNew["ParamValue"].ToString();
                            newrow["DataType"] = drNew["DataType"].ToString();
                            dsMarkerDBParam.Tables[0].Rows.Add(newrow);
                            foundflg = true;
                            log.Info("Parameter " + newrow["ParamName"].ToString() + ", applying value :" + newrow["ParamValue"].ToString());
                        }
                        if (foundflg == false)
                        {
                            log.Info("Parameter " + drp["ParamName"].ToString() + " is not passed in Job, hence applying default value :" + drp["DefaultValue"].ToString());
                            newrow["JobId"] = dsJob.Tables[0].Rows[0]["JobId"].ToString();
                            newrow["ParamName"] = drp["ParamName"].ToString();
                            newrow["ParamValue"] = drp["DefaultValue"].ToString();
                            newrow["DataType"] = drp["DataType"].ToString();
                            dsMarkerDBParam.Tables[0].Rows.Add(newrow);
                        }

                        // -------------- This is to put PArameter Value in Param Markers Place
                        string objName = drp["ObjName"].ToString().Replace("%%=", "%%P=");

                        foreach (Worksheet xlWorkSheet in xlWorkBook.Worksheets)
                        {
                            string FirstAddress = null;
                            Range xlusedRange = xlWorkSheet.UsedRange;
                            Range xlRange = xlusedRange.Find(objName + "." + drp["ParamName"].ToString(), Type.Missing, XlFindLookIn.xlValues, XlLookAt.xlPart, XlSearchOrder.xlByRows, XlSearchDirection.xlNext);
                            if (xlRange != null)
                            {
                                long rwno = xlRange.Row;
                                FirstAddress = xlRange.Address;
                                do
                                {
                                    xlWorkSheet.Cells[rwno, xlRange.Column] = newrow["ParamValue"].ToString();
                                    xlRange = xlWorkSheet.UsedRange.FindNext(xlRange);
                                } while (xlRange != null && xlRange.Address != FirstAddress);
                            }
                        }
                        // -------------- This is to put PArameter Value in Param Markers Place
                    }



                   
                    int i;
                    int totFoundColsinRow = 0;
                    int FirstRowPos = 0;
                    string markerCellText;
                    ConInfo.Clear();
                    ConInfo.Add(drMarker["ServerName"].ToString());
                    ConInfo.Add(drMarker["Port"].ToString());
                    ConInfo.Add(drMarker["DBName"].ToString());
                    ConInfo.Add(drMarker["UserName"].ToString());
                    ConInfo.Add(drMarker["Password"].ToString());
                    ConInfo.Add(drMarker["ConnectionType"].ToString());

                    log.Info("Marker Connection Infor for " + drMarker["ObjName"].ToString() + " Server - '" + ConInfo[0].ToString() + "', Port - '" + ConInfo[1].ToString() + "', Database Name - '" + ConInfo[2].ToString() + "' UserId - '" + ConInfo[3].ToString() + "' Password - '" + ConInfo[4].ToString());
                    log.Info("Fetching Data for " + drMarker["ObjName"].ToString());
                    dsData = objBL.ExecuteUnderlayingSP(dsMarkerDBParam, drMarker["SPName"].ToString(), ConInfo);
                    log.Info("Populating Data for " + drMarker["ObjName"].ToString());
                    foreach (Worksheet xlWorkSheet in xlWorkBook.Worksheets)
                    {
                        int[] dataCols = new int[50];
                        Range xlusedRange = xlWorkSheet.UsedRange;
                        Range xlRange = xlusedRange.Find(drMarker["ObjName"].ToString(), Type.Missing, XlFindLookIn.xlValues, XlLookAt.xlPart, XlSearchOrder.xlByRows, XlSearchDirection.xlNext);
                        string FirstAddress = null;
                        if (xlRange != null)
                        {
                            do
                            {

                                totFoundColsinRow = 0;
                                FirstAddress = xlRange.Address;
                                FirstRowPos = Convert.ToInt32(FirstAddress.Substring(FirstAddress.IndexOf("$", 1) + 1));
                                for (i = 1; i <= 50; i++)
                                {
                                    markerCellText = Convert.ToString(xlWorkSheet.Cells[FirstRowPos, i].value2);
                                    if (markerCellText != null)
                                        for (int j = 0; j < dsData.Tables[0].Columns.Count; j++)
                                        {
                                            if (markerCellText.IndexOf("." + dsData.Tables[0].Columns[j].ColumnName) > 0)
                                            {
                                                dataCols[i-1] = j + 1;
                                                totFoundColsinRow = i;
                                                break;
                                            }
                                            if(dataCols[i] ==0 && markerCellText.IndexOf(".SRNO") > 0)
                                            {
                                                dataCols[i-1] = -1;
                                                totFoundColsinRow = i;
                                                break;
                                            }
                                        }
                                }
                                if (totFoundColsinRow > 0)
                                {
                                    var data = new object[dsData.Tables[0].Rows.Count, totFoundColsinRow];
                                    for (var row = 1; row <= dsData.Tables[0].Rows.Count; row++)
                                    {
                                        for (var column = 1; column <= totFoundColsinRow; column++)
                                        {
                                            if (dataCols[column - 1] > 0)
                                                data[row - 1, column - 1] = dsData.Tables[0].Rows[row - 1][dataCols[column - 1] - 1];
                                            else if (dataCols[column - 1] == -1)
                                                data[row - 1, column - 1] = row;
                                            else
                                                data[row - 1, column - 1] = "";
                                        }
                                    }

                                    var startCell = (Range)xlWorkSheet.Cells[FirstRowPos, 1];
                                    var endCell = (Range)xlWorkSheet.Cells[dsData.Tables[0].Rows.Count-1 + FirstRowPos, totFoundColsinRow];
                                    var writeRange = xlWorkSheet.Range[startCell, endCell];
                                    if (endCell.Row < startCell.Row)
                                    {
                                        endCell = (Range)xlWorkSheet.Cells[FirstRowPos, totFoundColsinRow];
                                        data = new object[1, totFoundColsinRow];
                                        writeRange = xlWorkSheet.Range[startCell, endCell];
                                        for (var column = 1; column <= totFoundColsinRow; column++)
                                        {
                                            data[0, column - 1] = "";
                                        }
                                    }

                                    writeRange.Value2 = data;
                                }
                                //lstMarkerRange.Add(xlRange);
                                //string marker = xlRange.Value2;
                                //log.Info("Processing Marker " + marker);
                                //long FirstRow = xlRange.Row;
                                //long rwno = xlRange.Row;
                                //if (!lstMarkerRows.Contains(xlWorkSheet.Name + marker.Substring(3, marker.IndexOf(".") - 3) + xlRange.Row))
                                //{
                                //    lstMarkerRows.Add(xlWorkSheet.Name + marker.Substring(3, marker.IndexOf(".") - 3) + xlRange.Row);
                                //    foreach (DataRow dr in dsData.Tables[0].Rows)
                                //    {
                                //        switch (marker.Substring(marker.IndexOf(".") + 1, marker.Length - (marker.IndexOf(".") + 1)))
                                //        {
                                //            case "SRNO":
                                //                xlWorkSheet.Cells[rwno, xlRange.Column] = rwno - FirstRow + 1;
                                //                break;
                                //            default:
                                //                xlWorkSheet.Cells[rwno, xlRange.Column] = dr[marker.Substring(marker.IndexOf(".") + 1, marker.Length - (marker.IndexOf(".") + 1))].ToString();
                                //                break;
                                //        }
                                //        Range Line = (Range)xlWorkSheet.Rows[rwno + 1];
                                //        Line.Insert();
                                //        rwno = rwno + 1;
                                //    }
                                //    ((Range)xlWorkSheet.Rows[rwno]).Delete(XlDeleteShiftDirection.xlShiftUp);
                                //    if (dsData.Tables[0].Rows.Count <= 0)
                                //        break;
                                //}
                                //else
                                //{
                                //    foreach (DataRow dr in dsData.Tables[0].Rows)
                                //    {
                                //        xlWorkSheet.Cells[rwno, xlRange.Column] = dr[marker.Substring(marker.IndexOf(".") + 1, marker.Length - (marker.IndexOf(".") + 1))].ToString();
                                //        rwno = rwno + 1;
                                //    }

                                //}
                                xlRange = xlusedRange.Find(drMarker["ObjName"].ToString());
                                if (xlRange != null)
                                    xlRange = xlWorkSheet.UsedRange.FindNext(xlRange);
                            } while (xlRange != null && xlRange.Address != FirstAddress);
                        }
                    }
                    log.Info("Populating Data for " + drMarker["ObjName"].ToString() + " Completed");
                }
                //Clean up activity for Markers
                string StagingArea = Common.GetConfigValue("StagingAreaLocation");

                string OutputFileName = dsMetadata.Tables[0].Rows[0]["ReportId"].ToString() + genId.ToString() + "_" + DateTime.Now.ToString("ddMMyyyyHHMMSS") + Guid.NewGuid().ToString() + "." + FileExt;

                log.Info("Saving output in Staging Location '" + StagingArea + "', File Name : " + OutputFileName);
                xlWorkBook.SaveAs(StagingArea + OutputFileName);
                log.Info("Report output saved successfully, now setting the Job to Generator SUCCESS");
                xlWorkBook.Close();
                xlWorkBook = null;
                
                string ftpServer = Common.GetConfigValue("FTP");
                if (ftpServer.Trim().Length > 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {

                            log.Info("FTP Details found. attempt " + i.ToString() + " to upload in FTP Location : " + ftpServer);
                            string ftpUid = Common.GetConfigValue("FTPUid");
                            string ftpPwd = Common.GetConfigValue("FTPPWD");
                            FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + OutputFileName);
                            requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                            requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                            FileInfo fileInfo = new FileInfo(StagingArea + OutputFileName);
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
                            break;
                        }
                        catch (Exception ex)
                        {

                            if (i > 2)
                            {
                                throw ex;
                            }
                            else
                            {
                                log.Info("Attempt " + i.ToString() + " to upload output failed. System will again try in 5 seconds");
                                Thread.Sleep(5000);
                            }
                        }
                    }
                }
                objBL.SetJobStatus(JobId, (int)Enums.StatusCode.GENSU, "Report Successfully Generated", genId, OutputFileName, StagingArea,0);
                log.Info("Job - "+ JobId.ToString() +" Status Updated to Generator SUCCESS");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message,ex);
                objBL.SetJobStatus(JobId, (int)Enums.StatusCode.GENER,ex.Message,genId,"","",0);
            }
            finally
            {
                log.Info("Closing Report Template if open.");
                if (xlWorkBook != null)
                {
                    log.Info("Closing Workbook.");
                    xlWorkBook.Close(0);
                }
                xlApp.Quit();
                log.Info("Excel Application closed.");
            }
        }


    }
}

