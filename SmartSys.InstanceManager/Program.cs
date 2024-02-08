using log4net;
using SmartSys.BL.RGS;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SmartSys.Utility;
using SmartSys.BL.RDS;
using SmartSys.BL.DataHub;

//Here is the once-per-application setup information
[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace SmartSys.InstanceManager
{
    class Program
    {
        private static ILog log;
        private static string HostName;
        private static Int32 PrimaryIMProcessId;
        private static Int32 SecondaryIMProcessId;
        private static Int32 PollingInterval;
        private static Int32 CurrentPId;

        /// <summary>
        /// This method initilizes the Logger by picking up the cofig file location from configuration.
        /// </summary>
        /// <param name="genNo">This is required to create new logfile for the specific generator.</param>
        private static void InitializeLogger()
        {
            if (log4net.LogManager.GetCurrentLoggers().Length == 0)
            {
                string configFile = ConfigurationManager.AppSettings["IMLog4NetConfigFile"];
                log4net.GlobalContext.Properties["InfoLogFileName"] = "InstanceManager_InfoLog.log";
                log4net.GlobalContext.Properties["ErrorLogFileName"] = "InstanceManager_ErrorLog.log";
                log4net.Config.XmlConfigurator.Configure(new FileInfo(configFile));

            }
            log = log4net.LogManager.GetLogger(typeof(Program));
            log.Info("Instance Manager process for host " + HostName + " starting up.");
        }

        static void Main(string[] args)
        {
            HostName = System.Environment.MachineName;
            InitializeLogger();
            InitializeInstanceManagerProcess();
            log.Info("Instance manager on Host " + HostName + "Going Down.");
        }

        private static void InitializeInstanceManagerProcess()
        {
            try
            {
                InstanceManagerBL objBL = new InstanceManagerBL();
                DataSet dsIM = objBL.GetInstanceManagerByHost(HostName);
                if (dsIM == null)
                {
                    log.Error("Invalid Host to initiate Instance Manager, Please check IM configuration on Host '" + HostName + "'");
                    return;
                }
                if (dsIM.Tables.Count <= 0)
                {
                    log.Error("Invalid Host to initiate Instance Manager, Please check IM configuration on Host '" + HostName + "'");
                    return;
                }
                if (dsIM.Tables[0].Rows.Count <= 0)
                {
                    log.Error("Invalid Host to initiate Instance Manager, Please check IM configuration on Host '" + HostName + "'");
                    return;
                }
                if (!Convert.ToBoolean(dsIM.Tables[0].Rows[0]["isPrimary"].ToString()))
                {
                    log.Error("Primary Instance Manager Setup on Host '" + HostName + "' is missing.");
                    return;
                }
                if (dsIM.Tables[0].Rows.Count <= 1)
                {
                    log.Error("Secondary Instance Manager on Host '" + HostName + "' is missing");
                    return;
                }
                if (Convert.ToInt32(dsIM.Tables[0].Rows[0]["ProcessId"].ToString()) > 0)
                {
                    log.Info("Checking for Existing process Id " + dsIM.Tables[0].Rows[0]["ProcessId"].ToString() + " On Host " + HostName);
                    if (ProcessExists(Convert.ToInt32(dsIM.Tables[0].Rows[0]["ProcessId"].ToString()),"InstanceManager"))
                    {
                        log.Info("Primary Instance Manager already running on the Host '" + HostName + "' with process ID " + dsIM.Tables[0].Rows[0]["ProcessId"].ToString());
                        PrimaryIMProcessId = Convert.ToInt32(dsIM.Tables[0].Rows[0]["ProcessId"].ToString());
                        if (Convert.ToInt32(dsIM.Tables[0].Rows[1]["ProcessId"].ToString()) > 0)
                        {
                            log.Info("Checking for Secondary Instance Manager Existing process Id " + dsIM.Tables[0].Rows[1]["ProcessId"].ToString() + " On Host " + HostName);
                            if (ProcessExists(Convert.ToInt32(dsIM.Tables[0].Rows[1]["ProcessId"].ToString()),"InstanceManager"))
                            {
                                log.Error("Secondary Instance Manager is also running on Host '" + HostName + "' with Process Id " + dsIM.Tables[0].Rows[1]["ProcessId"].ToString());
                                return;
                            }
                            else
                            {
                                log.Info("No Process Found for Secondary Instance Manager, Registring this as Secondary Instance Manager.");
                                Process p = Process.GetCurrentProcess();
                                SecondaryIMProcessId = p.Id;
                                CurrentPId = p.Id;
                                int errorCode = objBL.SetInstanceManagerProcessId(Convert.ToInt16(dsIM.Tables[0].Rows[1]["IMId"].ToString()), SecondaryIMProcessId);
                                if (errorCode == 0)
                                {
                                    log.Error(CurrentPId.ToString() + "_" + "Unable to set secondary instance manager Process Id.");
                                    return;
                                }
                                PollingInterval = Convert.ToInt32(dsIM.Tables[0].Rows[1]["PollingInterval"].ToString());
                                StartSecondaryProcess();
                            }
                        }
                        else
                        {
                            Process p = Process.GetCurrentProcess();
                            SecondaryIMProcessId = p.Id;
                            CurrentPId = p.Id;
                            int errorCode = objBL.SetInstanceManagerProcessId(Convert.ToInt16(dsIM.Tables[0].Rows[1]["IMId"].ToString()), SecondaryIMProcessId);
                            if (errorCode == 0)
                            {
                                log.Error(CurrentPId.ToString() + "_" + "Unable to set secondary instance manager Process Id.");
                                return;
                            }
                            PollingInterval = Convert.ToInt32(dsIM.Tables[0].Rows[1]["PollingInterval"].ToString());
                            StartSecondaryProcess();
                        }
                    }
                    else
                    {
                        log.Info("Primary Instance Manager process is not in running process, Starting this process as Primary Instance Manager");
                        Process p = Process.GetCurrentProcess();
                        PrimaryIMProcessId = p.Id;
                        CurrentPId = p.Id;
                        int errorCode = objBL.SetInstanceManagerProcessId(Convert.ToInt16(dsIM.Tables[0].Rows[0]["IMId"].ToString()), PrimaryIMProcessId);
                        if (errorCode == 0)
                        {
                            log.Error(CurrentPId.ToString() + "_" + "Unable to set Primary Instance Manager Process Id");
                            return;
                        }
                        PollingInterval = Convert.ToInt32(dsIM.Tables[0].Rows[0]["PollingInterval"].ToString());
                        StartPrimaryProcess();
                    }
                }
                else
                {
                    log.Info("Primary Instance Manager process is not in running process, Starting this process as Primary Instance Manager");
                    Process p = Process.GetCurrentProcess();
                    PrimaryIMProcessId = p.Id;
                    CurrentPId = p.Id;
                    int errorCode = objBL.SetInstanceManagerProcessId(Convert.ToInt16(dsIM.Tables[0].Rows[0]["IMId"].ToString()), PrimaryIMProcessId);
                    if (errorCode == 0)
                    {
                        log.Error(CurrentPId.ToString() + "_" + "Unable to set Primary Instance Manager Process Id");
                        return;
                    }
                    PollingInterval = Convert.ToInt32(dsIM.Tables[0].Rows[0]["PollingInterval"].ToString());
                    StartPrimaryProcess();
                }
            }
            catch (Exception ex)
            {
                log.Info(CurrentPId.ToString() + "_" + ex.Message, ex);
            }
            while(true)
            {
                //Later put code here for thread status check
                Thread.Sleep(10000);
            }
        }

        private static void StartPrimaryProcess()
        {
            try
            {
                log.Info(CurrentPId.ToString() + "_" + "Downloading Missing Templates if any.");
                GetReportTemplatesinLocalLocation();

                log.Info(CurrentPId.ToString() + "_" + "Starting Secondary Instance Manager Polling Thread");
                Thread ThrdSecondaryProcessPolling = new Thread(() => SecondaryProcessPolling());
                ThrdSecondaryProcessPolling.Start();

                log.Info(CurrentPId.ToString() + "_" + "Starting Generator Status Polling Thread");
                Thread ThrdGenStatusPolling = new Thread(() => GeneratorPolling());
                ThrdGenStatusPolling.Start();

                log.Info(CurrentPId.ToString() + "_" + "Starting RDS Engine Status Polling Thread");
                Thread ThrdRDSEngineStatusPolling = new Thread(() => RDSEnginePolling());
                ThrdRDSEngineStatusPolling.Start();

                log.Info(CurrentPId.ToString() + "_" + "Starting RDS Engine Status Polling Thread");
                Thread ThrdDHLoaderStatusPolling = new Thread(() => DataHubLoaderPolling());
                ThrdDHLoaderStatusPolling.Start();
                
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
        private static void StartSecondaryProcess()
        {
            try
            {
                Thread ThrdPrimaryProcessPolling = new Thread(() => PrimaryProcessPolling());
                ThrdPrimaryProcessPolling.Start();
            }
            catch (Exception ex)
            {
                log.Error(CurrentPId.ToString() + "_" + ex.Message, ex);
            }
        }

        private static void DataHubLoaderPolling()
        {
            try
            {
                while (true)
                {
                    try
                    {

                        DataSet dsGen = new DataSet();
                        DHLoaderBL objGenBL = new DHLoaderBL();
                        log.Info(CurrentPId.ToString() + "_" + "Fetching Generator Information.");
                        dsGen = objGenBL.GetDataHubLoaderByHost(HostName);
                        if (dsGen != null)
                        {
                            if (dsGen.Tables.Count > 0)
                            {
                                foreach (DataRow dr in dsGen.Tables[0].Rows)
                                {
                                    Process p;
                                    bool flag = false;
                                    flag = ProcessExists(Convert.ToInt32((dr["ProcessId"].ToString())), Common.GetConfigValue("LoaderProcessTitle"));

                                    if (flag == true)
                                    {
                                        if (Convert.ToBoolean(dr["isActive"].ToString()) == true)
                                        {
                                            switch (Convert.ToInt32(dr["StatusId"].ToString()))
                                            {
                                                case (int)Enums.StatusCode.Idle:
                                                case (int)Enums.StatusCode.BUSY:
                                                    break;
                                                case (int)Enums.StatusCode.START:
                                                    if (objGenBL.SetLoaderStatus(Convert.ToInt32(dr["LoaderId"].ToString()), (Int16)Enums.StatusCode.Idle) != 500001)
                                                    {
                                                        log.Error(CurrentPId.ToString() + "_" + "Unable to set the Loader Status. for GenId " + dr["LoaderId"].ToString());
                                                    }
                                                    break;
                                                case (int)Enums.StatusCode.RESTART:
                                                    log.Info(CurrentPId.ToString() + "_" + "Found request to Restart Loader " + dr["LoaderId"].ToString() + " First Loader will stop.");

                                                    p = Process.GetProcessById(Convert.ToInt32((dr["ProcessId"].ToString())));
                                                    p.Kill();
                                                    log.Info(CurrentPId.ToString() + "_" + "Loader " + dr["LoaderId"].ToString() + " Stopped Successfully, Now Requesting to Start.");
                                                    if (objGenBL.SetLoaderStatus(Convert.ToInt16(dr["LoaderId"].ToString()), (Int16)Enums.StatusCode.START) != 500001)
                                                    {
                                                        log.Error(CurrentPId.ToString() + "_" + "Unable to set the Loader Status. for LoaderId " + dr["LoaderId"].ToString());
                                                    }
                                                    break;
                                                case (int)Enums.StatusCode.STOP:
                                                case (int)Enums.StatusCode.STOPPED:
                                                    p = Process.GetProcessById(Convert.ToInt32((dr["ProcessId"].ToString())));
                                                    p.Kill();
                                                    if (objGenBL.SetLoaderStatus(Convert.ToInt16(dr["LoaderId"].ToString()), (Int16)Enums.StatusCode.STOPPED) != 500001)
                                                    {
                                                        log.Error(CurrentPId.ToString() + "_" + "Unable to set the Loader Status. for LoaderId " + dr["LoaderId"].ToString());
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            log.Info(CurrentPId.ToString() + "_" + "Found Loader " + dr["LoaderId"].ToString() + " up and running which should not be Active hence shutting down.");
                                            p = Process.GetProcessById(Convert.ToInt32((dr["ProcessId"].ToString())));
                                            p.Kill();
                                        }

                                    }
                                    else
                                    {
                                        if (Convert.ToBoolean(dr["isActive"].ToString()) == true)
                                        {
                                            switch (Convert.ToInt32(dr["StatusId"].ToString()))
                                            {
                                                case (int)Enums.StatusCode.Idle:
                                                case (int)Enums.StatusCode.START:
                                                case (int)Enums.StatusCode.RESTART:
                                                case (int)Enums.StatusCode.BUSY:
                                                    {
                                                        log.Info(CurrentPId.ToString() + "_" + "Loader with Id " + dr["LoaderId"].ToString() + " is down starting Loader process.");
                                                        p = new Process();
                                                        p.StartInfo.FileName = Common.GetConfigValue("LoaderExe") + Common.GetConfigValue("LoaderProcessTitle") + ".exe";
                                                        p.StartInfo.Arguments = dr["LoaderId"].ToString();
                                                        p.Start();
                                                        Thread.Sleep(10000);
                                                        break;
                                                    }
                                                case (int)Enums.StatusCode.STOP:
                                                    if (objGenBL.SetLoaderStatus(Convert.ToInt16(dr["LoaderId"].ToString()), (Int16)Enums.StatusCode.STOPPED) != 500001)
                                                    {
                                                        log.Error(CurrentPId.ToString() + "_" + "Unable to set the Loader Status. for LoaderId " + dr["LoaderId"].ToString());
                                                    }
                                                    break;
                                                case (int)Enums.StatusCode.STOPPED:
                                                case (int)Enums.StatusCode.DOWN:
                                                    break;
                                            }
                                        }
                                    }
                                }

                                log.Info(CurrentPId.ToString() + "_" + "Checking for stale Loader.");
                                Thread.Sleep(10000);
                                dsGen = objGenBL.GetDataHubLoaderByHost(HostName);
                                Process[] gens = Process.GetProcessesByName(Common.GetConfigValue("LoaderProcessTitle"));
                                bool flagFound = false;
                                foreach (Process p in gens)
                                {
                                    flagFound = false;
                                    foreach (DataRow dr in dsGen.Tables[0].Rows)
                                    {
                                        if (Convert.ToInt32((dr["ProcessId"].ToString())) == p.Id)
                                        {
                                            switch (Convert.ToInt32(dr["StatusId"].ToString()))
                                            {
                                                case (int)Enums.StatusCode.Idle:
                                                case (int)Enums.StatusCode.START:
                                                case (int)Enums.StatusCode.RESTART:
                                                case (int)Enums.StatusCode.BUSY:
                                                    flagFound = true;
                                                    break;
                                            }
                                            break;
                                        }
                                    }
                                    if (flagFound == false)
                                    {
                                        log.Info(CurrentPId.ToString() + "_" + "Found Loader Process with LoaderId " + p.Id + " of type " + Common.GetConfigValue("LoaderProcessTitle") + " as stale process hence shutting down.");
                                        p.Kill();
                                    }
                                }

                                log.Info(CurrentPId.ToString() + "_" + "Finished Loader status polling next attempt again in " + PollingInterval + " Seconds");
                                Thread.Sleep(PollingInterval * 1000);
                            }
                            else
                            {
                                log.Error(CurrentPId.ToString() + "_" + "Unable to get the Loader statuses, Polling with attempt again in " + PollingInterval + " Seconds");
                                Thread.Sleep(PollingInterval * 1000);
                            }
                        }
                        else
                        {
                            log.Error(CurrentPId.ToString() + "_" + "Unable to get the Loader statuses, Polling with attempt again in " + PollingInterval + " Seconds");
                            Thread.Sleep(PollingInterval * 1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(CurrentPId.ToString() + "_" + ex.Message, ex);
            }
        }

        private static void RDSEnginePolling()
        {
            while (true)
            {
                try
                {
                    DataSet dsEngine = new DataSet();
                    RDSEngineBL objBL = new RDSEngineBL();
                    log.Info(CurrentPId.ToString() + "_" + "Fetching RDS Engine Information.");
                    dsEngine = objBL.GetRDSEngineInfoByHost(HostName);
                    if (dsEngine != null)
                    {
                        if (dsEngine.Tables.Count > 0)
                        {
                            foreach (DataRow dr in dsEngine.Tables[0].Rows)
                            {
                                Process p;
                                bool flag = false;
                                flag = ProcessExists(Convert.ToInt32((dr["ProcessId"].ToString())), Common.GetConfigValue("RDSEngineProcessTitle"));

                                if (flag == true)
                                {
                                    if (Convert.ToBoolean(dr["isActive"].ToString()) == true)
                                    {
                                        switch (Convert.ToInt32(dr["StatusId"].ToString()))
                                        {
                                            case (int)Enums.StatusCode.Idle:
                                            case (int)Enums.StatusCode.BUSY:
                                                break;
                                            case (int)Enums.StatusCode.START:
                                                if (objBL.SetEngineStatus(Convert.ToInt16(dr["RDSEngineId"].ToString()), (Int16)Enums.StatusCode.Idle) != 500001)
                                                {
                                                    log.Error(CurrentPId.ToString() + "_" + "Unable to set the Engine Status. for RDSEngineId " + dr["RDSEngineId"].ToString());
                                                }
                                                break;
                                            case (int)Enums.StatusCode.RESTART:
                                                log.Info(CurrentPId.ToString() + "_" + "Found request to Restart RDS Engine " + dr["RDSEngineId"].ToString() + " First Engine will stop.");

                                                p = Process.GetProcessById(Convert.ToInt32((dr["ProcessId"].ToString())));
                                                p.Kill();
                                                log.Info(CurrentPId.ToString() + "_" + "RDS Engine " + dr["GenId"].ToString() + " Stopped Successfully, Now Requesting to Start.");
                                                if (objBL.SetEngineStatus(Convert.ToInt16(dr["RDSEngineId"].ToString()), (Int16)Enums.StatusCode.START) != 500001)
                                                {
                                                    log.Error(CurrentPId.ToString() + "_" + "Unable to set the Engine Status. for RDSEngineId " + dr["RDSEngineId"].ToString());
                                                }
                                                break;
                                            case (int)Enums.StatusCode.STOP:
                                            case (int)Enums.StatusCode.STOPPED:
                                                p = Process.GetProcessById(Convert.ToInt32((dr["ProcessId"].ToString())));
                                                p.Kill();
                                                if (objBL.SetEngineStatus(Convert.ToInt16(dr["RDSEngineId"].ToString()), (Int16)Enums.StatusCode.STOPPED) != 500001)
                                                {
                                                    log.Error(CurrentPId.ToString() + "_" + "Unable to set the Engine Status. for RDSEngineId " + dr["RDSEngineId"].ToString());
                                                }
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        log.Info(CurrentPId.ToString() + "_" + "Found Engine " + dr["RDSEngineId"].ToString() + " up and running which should not be Active hence shutting down.");
                                        p = Process.GetProcessById(Convert.ToInt32((dr["ProcessId"].ToString())));
                                        p.Kill();
                                    }

                                }
                                else
                                {
                                    if (Convert.ToBoolean(dr["isActive"].ToString()) == true)
                                    {
                                        switch (Convert.ToInt32(dr["StatusId"].ToString()))
                                        {
                                            case (int)Enums.StatusCode.Idle:
                                            case (int)Enums.StatusCode.START:
                                            case (int)Enums.StatusCode.RESTART:
                                            case (int)Enums.StatusCode.BUSY:
                                                {
                                                    log.Info(CurrentPId.ToString() + "_" + "RDS Engine with Id " + dr["RDSEngineId"].ToString() + " is down starting generator process.");
                                                    p = new Process();
                                                    p.StartInfo.FileName = Common.GetConfigValue("RDSEngineExe") + Common.GetConfigValue("RDSEngineProcessTitle") + ".exe";
                                                    p.StartInfo.Arguments = dr["RDSEngineId"].ToString();
                                                    p.Start();
                                                    Thread.Sleep(10000);
                                                    break;
                                                }
                                            case (int)Enums.StatusCode.STOP:
                                                if (objBL.SetEngineStatus(Convert.ToInt16(dr["RDSEngineId"].ToString()), (Int16)Enums.StatusCode.STOPPED) != 500001)
                                                {
                                                    log.Error(CurrentPId.ToString() + "_" + "Unable to set the Engine Status. for RDSEngineId " + dr["RDSEngineId"].ToString());
                                                }
                                                break;
                                            case (int)Enums.StatusCode.STOPPED:
                                            case (int)Enums.StatusCode.DOWN:
                                                break;
                                        }
                                    }
                                }
                            }

                            log.Info(CurrentPId.ToString() + "_" + "Checking for stale RDS Engines.");
                            Thread.Sleep(10000);
                            dsEngine = objBL.GetRDSEngineInfoByHost(HostName);
                            Process[] Engines = Process.GetProcessesByName(Common.GetConfigValue("RDSEngineProcessTitle"));
                            bool flagFound = false;
                            foreach (Process p in Engines)
                            {
                                flagFound = false;

                                foreach (DataRow dr in dsEngine.Tables[0].Rows)
                                {
                                    if (Convert.ToInt32((dr["ProcessId"].ToString())) == p.Id && Convert.ToBoolean(dr["isActive"].ToString()) == true)
                                    {
                                        switch (Convert.ToInt32(dr["StatusId"].ToString()))
                                        {
                                            case (int)Enums.StatusCode.Idle:
                                            case (int)Enums.StatusCode.START:
                                            case (int)Enums.StatusCode.RESTART:
                                            case (int)Enums.StatusCode.BUSY:
                                                flagFound = true;
                                                break;
                                        }
                                        break;
                                    }
                                }
                                if (flagFound == false)
                                {
                                    log.Info(CurrentPId.ToString() + "_" + "Found RDS Engine Process with Id " + p.Id + " of type " + Common.GetConfigValue("RDSEngineProcessTitle") + " as stale process hence shutting down.");
                                    p.Kill();
                                }
                            }

                            log.Info(CurrentPId.ToString() + "_" + "Finished RDS Engine status polling next attempt again in " + PollingInterval + " Seconds");
                            Thread.Sleep(PollingInterval * 1000);
                        }
                        else
                        {
                            log.Error(CurrentPId.ToString() + "_" + "Unable to get the RDS Engine statuses, Polling will attempt again in " + PollingInterval + " Seconds");
                            Thread.Sleep(PollingInterval * 1000);
                        }
                    }
                    else
                    {
                        log.Error(CurrentPId.ToString() + "_" + "Unable to get the RDS Engine statuses, Polling will attempt again in " + PollingInterval + " Seconds");
                        Thread.Sleep(PollingInterval * 1000);
                    }
                }


                catch (Exception ex)
                {
                    log.Error(ex.Message, ex);
                    log.Info("System Will Check again in Some time");
                    Thread.Sleep(PollingInterval * 1000);
                }
            }
        }
        private static void GeneratorPolling()
        {
            try
            {
                while (true)
                {
                    try
                    {

                        DataSet dsGen = new DataSet();
                        GeneratorBL objGenBL = new GeneratorBL();
                        log.Info(CurrentPId.ToString() + "_" + "Fetching Generator Information.");
                        dsGen = objGenBL.GetGeneratorDetails(HostName);
                        if (dsGen != null)
                        {
                            if (dsGen.Tables.Count > 0)
                            {
                                foreach (DataRow dr in dsGen.Tables[0].Rows)
                                {
                                    Process p;
                                    bool flag = false;
                                    if (dr["GenType"].ToString() == Enums.GenType.CR.ToString())
                                        flag = ProcessExists(Convert.ToInt32((dr["ProcessId"].ToString())), Common.GetConfigValue("CRGenProcessTitle"));
                                    else
                                        flag = ProcessExists(Convert.ToInt32((dr["ProcessId"].ToString())), Common.GetConfigValue("EWGenProcessTitle"));

                                    if (flag == true)
                                    {
                                        if (Convert.ToBoolean(dr["isActive"].ToString()) == true)
                                        {
                                            switch (Convert.ToInt32(dr["StatusId"].ToString()))
                                            {
                                                case (int)Enums.StatusCode.Idle:
                                                case (int)Enums.StatusCode.BUSY:
                                                    break;
                                                case (int)Enums.StatusCode.START:
                                                    if (objGenBL.SetGeneratorStatus(Convert.ToInt16(dr["GenId"].ToString()), (Int16)Enums.StatusCode.Idle) != 500001)
                                                    {
                                                        log.Error(CurrentPId.ToString() + "_" + "Unable to set the generator Status. for GenId " + dr["GenId"].ToString());
                                                    }
                                                    break;
                                                case (int)Enums.StatusCode.RESTART:
                                                    log.Info(CurrentPId.ToString() + "_" + "Found request to Restart generator " + dr["GenId"].ToString() + " First Generator will stop.");

                                                    p = Process.GetProcessById(Convert.ToInt32((dr["ProcessId"].ToString())));
                                                    p.Kill();
                                                    log.Info(CurrentPId.ToString() + "_" + "Generator " + dr["GenId"].ToString() + " Stopped Successfully, Now Requesting to Start.");
                                                    if (objGenBL.SetGeneratorStatus(Convert.ToInt16(dr["GenId"].ToString()), (Int16)Enums.StatusCode.START) != 500001)
                                                    {
                                                        log.Error(CurrentPId.ToString() + "_" + "Unable to set the generator Status. for GenId " + dr["GenId"].ToString());
                                                    }
                                                    break;
                                                case (int)Enums.StatusCode.STOP:
                                                case (int)Enums.StatusCode.STOPPED:
                                                    p = Process.GetProcessById(Convert.ToInt32((dr["ProcessId"].ToString())));
                                                    p.Kill();
                                                    if (objGenBL.SetGeneratorStatus(Convert.ToInt16(dr["GenId"].ToString()), (Int16)Enums.StatusCode.STOPPED) != 500001)
                                                    {
                                                        log.Error(CurrentPId.ToString() + "_" + "Unable to set the generator Status. for GenId " + dr["GenId"].ToString());
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            log.Info(CurrentPId.ToString() + "_" + "Found generator " + dr["GenId"].ToString() + " up and running which should not be Active hence shutting down.");
                                            p = Process.GetProcessById(Convert.ToInt32((dr["ProcessId"].ToString())));
                                            p.Kill();
                                        }

                                    }
                                    else
                                    {
                                        if (Convert.ToBoolean(dr["isActive"].ToString()) == true)
                                        {
                                            switch (Convert.ToInt32(dr["StatusId"].ToString()))
                                            {
                                                case (int)Enums.StatusCode.Idle:
                                                case (int)Enums.StatusCode.START:
                                                case (int)Enums.StatusCode.RESTART:
                                                case (int)Enums.StatusCode.BUSY:
                                                    {
                                                        log.Info(CurrentPId.ToString() + "_" + "Generator with Id " + dr["GenId"].ToString() + " is down starting generator process.");
                                                        p = new Process();
                                                        if (dr["GenType"].ToString() == Enums.GenType.CR.ToString())
                                                            p.StartInfo.FileName = Common.GetConfigValue("CRGenExe") + Common.GetConfigValue("CRGenProcessTitle") + ".exe";
                                                        else
                                                            p.StartInfo.FileName = Common.GetConfigValue("EWGenExe") + Common.GetConfigValue("EWGenProcessTitle") + ".exe";
                                                        p.StartInfo.Arguments = dr["GenId"].ToString();
                                                        p.Start();
                                                        Thread.Sleep(10000);
                                                        break;
                                                    }
                                                case (int)Enums.StatusCode.STOP:
                                                    if (objGenBL.SetGeneratorStatus(Convert.ToInt16(dr["GenId"].ToString()), (Int16)Enums.StatusCode.STOPPED) != 500001)
                                                    {
                                                        log.Error(CurrentPId.ToString() + "_" + "Unable to set the generator Status. for GenId " + dr["GenId"].ToString());
                                                    }
                                                    break;
                                                case (int)Enums.StatusCode.STOPPED:
                                                case (int)Enums.StatusCode.DOWN:
                                                    break;
                                            }
                                        }
                                    }
                                }

                                log.Info(CurrentPId.ToString() + "_" + "Checking for stale Generators.");
                                Thread.Sleep(10000);
                                dsGen = objGenBL.GetGeneratorDetails(HostName);
                                Process[] gens = Process.GetProcessesByName(Common.GetConfigValue("CRGenProcessTitle"));
                                bool flagFound = false;
                                foreach (Process p in gens)
                                {
                                    flagFound = false;
                                    foreach (DataRow dr in dsGen.Tables[0].Rows)
                                    {
                                        if (Convert.ToInt32((dr["ProcessId"].ToString())) == p.Id && dr["GenType"].ToString() == Enums.GenType.CR.ToString())
                                        {
                                            switch (Convert.ToInt32(dr["StatusId"].ToString()))
                                            {
                                                case (int)Enums.StatusCode.Idle:
                                                case (int)Enums.StatusCode.START:
                                                case (int)Enums.StatusCode.RESTART:
                                                case (int)Enums.StatusCode.BUSY:
                                                    flagFound = true;
                                                    break;
                                            }
                                            break;
                                        }
                                    }
                                    if (flagFound == false)
                                    {
                                        log.Info(CurrentPId.ToString() + "_" + "Found Generator Process with Id " + p.Id + " of type " + Common.GetConfigValue("CRGenProcessTitle") + " as stale process hence shutting down.");
                                        p.Kill();
                                    }
                                }

                                gens = Process.GetProcessesByName(Common.GetConfigValue("EWGenProcessTitle"));
                                foreach (Process p in gens)
                                {
                                    flagFound = false;
                                    foreach (DataRow dr in dsGen.Tables[0].Rows)
                                    {
                                        if (Convert.ToInt32((dr["ProcessId"].ToString())) == p.Id && dr["GenType"].ToString() == Enums.GenType.EW.ToString())
                                        {
                                            switch (Convert.ToInt32(dr["StatusId"].ToString()))
                                            {
                                                case (int)Enums.StatusCode.Idle:
                                                case (int)Enums.StatusCode.START:
                                                case (int)Enums.StatusCode.RESTART:
                                                case (int)Enums.StatusCode.BUSY:
                                                    flagFound = true;
                                                    break;
                                            }
                                            break;
                                        }
                                    }
                                    if (flagFound == false)
                                    {
                                        log.Info(CurrentPId.ToString() + "_" + "Found Generator Process with Id " + p.Id + " of type " + Common.GetConfigValue("CRGenProcessTitle") + " as stale process hence shutting down.");
                                        p.Kill();
                                    }
                                }
                                log.Info(CurrentPId.ToString() + "_" + "Finished Generator status polling next attempt again in " + PollingInterval + " Seconds");
                                Thread.Sleep(PollingInterval * 1000);
                            }
                            else
                            {
                                log.Error(CurrentPId.ToString() + "_" + "Unable to get the Generator statuses, Polling with attempt again in " + PollingInterval + " Seconds");
                                Thread.Sleep(PollingInterval * 1000);
                            }
                        }
                        else
                        {
                            log.Error(CurrentPId.ToString() + "_" + "Unable to get the Generator statuses, Polling with attempt again in " + PollingInterval + " Seconds");
                            Thread.Sleep(PollingInterval * 1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(CurrentPId.ToString() + "_" + ex.Message, ex);
            }
        }

        private static void PrimaryProcessPolling()
        {
            try
            {
                while (true)
                {
                    if (ProcessExists(PrimaryIMProcessId, "InstanceManager"))
                    {
                        log.Info(CurrentPId.ToString() + "_" + "Primary Process Id " + PrimaryIMProcessId + " is up and running System will check again in " + PollingInterval + "Seconds ");
                        Thread.Sleep(1000 * PollingInterval);
                    }
                    else
                    {
                        log.Error(CurrentPId.ToString() + "_" + "Primary Process seems not running, attempting to make this process as Primary Instance Manager");
                        PrimaryIMProcessId = SecondaryIMProcessId;
                        log.Info(CurrentPId.ToString() + "_" + "Starting new Instance Manager which will become Secondary Instance Manager.");
                        Process p = new Process();
                        p.StartInfo.FileName = Common.GetConfigValue("InstanceManagerLocation");
                        p.Start();
                        SecondaryIMProcessId = p.Id;
                        StartPrimaryProcess();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(CurrentPId.ToString() + "_" + ex.Message, ex);
            }
            
        }

        private static void SecondaryProcessPolling()
        {
            try
            {

                while (true)
                {
                    if (ProcessExists(SecondaryIMProcessId,"InstanceManager"))
                    {
                        log.Info(CurrentPId.ToString() + "_" + "Secondary Process Id " + SecondaryIMProcessId + " is up and running System will check again in " + PollingInterval + "Seconds ");
                        Thread.Sleep(1000 * PollingInterval);
                    }
                    else
                    {
                        log.Error(CurrentPId.ToString() + "_" + "Secondary Process seems not running, attempting to start the Secondary Instance Manager");
                        Process p = new Process();
                        p.StartInfo.FileName = Common.GetConfigValue("InstanceManagerLocation");
                        p.Start();
                        SecondaryIMProcessId = p.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(CurrentPId.ToString() + "_" + ex.Message, ex);
            }

        }

        private static void GetReportTemplatesinLocalLocation()
        {
            try
            {
                GeneratorBL objBL = new GeneratorBL();
                DataSet dsBaseReports = objBL.GetBaseReports();
                string LocalPath = Common.GetConfigValue("LocalReportTemplateLocation");
                foreach(DataRow dr in dsBaseReports.Tables[0].Rows)
                {
                    string[] fileattr = dr["FileName"].ToString().Split('.');
                    string FileName = LocalPath + fileattr[0] + dr["BaseReportId"].ToString() + "_" + dr["Version"].ToString() +"." + fileattr[1];
                    if (!System.IO.File.Exists(FileName))
                    {
                        if (dr["FileBinary"] != null)
                        {
                            log.Info(CurrentPId.ToString() + "_" + "Found New Template downloading " + FileName);
                            byte[] binaryFile = (byte[])dr["FileBinary"];
                            FileStream objFileStream = new FileStream(FileName, FileMode.Create, FileAccess.Write);
                            objFileStream.Write(binaryFile, 0, binaryFile.Length);
                            objFileStream.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(CurrentPId.ToString() + "_" + ex.Message, ex);
            }
        }
        private static bool ProcessExists(int id, string exeTitle)
        {
            return Process.GetProcessesByName(exeTitle).Any(x => x.Id == id);
        }

    }

}
