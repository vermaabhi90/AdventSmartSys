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
using System.Data;
using SmartSys.BL.Tmleave;
using SmartSys.Utility;
using System.Text.RegularExpressions;
using SmartSys.BL;
using SmartSys.DAL;

namespace SmartSys.ExchangeRateProcessor
{
    class Program
    {        
        private static ILog log;
        private static void InitializeLogger(string genNo)
        {
            
            if (log4net.LogManager.GetCurrentLoggers().Length == 0)
            {
                string configFile = ConfigurationManager.AppSettings["GetLogForExchangeRateProcessor"];
                log4net.GlobalContext.Properties["InfoLogFileName"] = "Er_" + genNo + "_InfoLog.log";
                log4net.GlobalContext.Properties["ErrorLogFileName"] = "Er_" + genNo + "_ErrorLog.log";
                log4net.Config.XmlConfigurator.Configure(new FileInfo(configFile));
            }
            log = log4net.LogManager.GetLogger(typeof(Program));  
            log.Info("Exchange Rate Processessor  starting up.");

        }
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid number of arguments ExchangeRateProcessor can not be initialized. Aborting !!!");
                return;
            }
            else
            {
                if (Convert.ToInt16(args[0]) <= 0)
                {
                    Console.WriteLine("Invalid arguments ExchangeRateProcessor can not be initialized. Aborting !!!");
                    return;
                }
            }
            InitializeLogger(args[0]);
            InitializeExchangeRateProcessor();
            log.Info("Generator " + args[0] + " process shutting down.");


        }
        private static void InitializeExchangeRateProcessor()
        {
            try
            {
                int pollingIntervel;

                pollingIntervel = Convert.ToInt32(ConfigurationManager.AppSettings["Getpollinginterval"]);

                log.Info("Intiallize Processor For Polling and Save Exchange Rate");
                ProcessExchageRate(pollingIntervel);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }
        private static void ProcessExchageRate(int pollingIntervel)
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
                    InitializeExchangeRateProcessor();
                }
                // cross check 

                AdminBL objbl = new AdminBL();

                while (true)
                {
                    log.Info("Fetching Currency List with Currency codes");

                    List<string> CurrencyExchange = new List<string>();

                    CurrencyConvertor obj = new CurrencyConvertor();
                    CurrencyExchange = obj.currencyConvertor();
                    List<CurrencyModel> currencyModelList = new List<CurrencyModel>();
                    List<string> unique = CurrencyExchange.Distinct().ToList();

                    List<string> Lst = new List<string>();

                    AdminDal objdal = new AdminDal();
                    Lst = objdal.CurrencyList();
                   
                    log.Info("All Currency Code Fetched");
                    foreach (var item in Lst)
                    {
                        try
                        {                          
                            WebClient web = new WebClient();
                            CurrencyModel currModel = new CurrencyModel();
                            double rate = double.Parse(web.DownloadString(@"https://ww.igolder.com/exchangerate.ashx?from=USD&to=" + item.ToUpper() + ""));
                            currModel.Source = "www.igolder.com";
                            currModel.CurrencyCode = item.ToString();
                            currModel.ExchangeRate = Convert.ToDouble(rate);
                            currencyModelList.Add(currModel);
                      
                     }
                        
                   catch (Exception)
                        {
                           log.Error("Fetching of Exchange Rate Failed ,Trying With New Server(Google)");
                           CurrencyConvertorClass objectnew = new CurrencyConvertorClass();
                           currencyModelList = objectnew.CurrencyGoogle();
                           int Errcode = objbl.CurrencyExchange(currencyModelList);
                       if(Errcode!=null)
                       {
                           log.Info("Updating Exchange Rate  SuccessFully");
                       }
                       log.Info("System Finished Updating the Currency Exchange Rates. After " + pollingIntervel / 60 + " Hours then will poll again.");
                       Thread.Sleep(pollingIntervel * 60 * 1000);
                        }
                     }
                 
                   int errcode = objbl.CurrencyExchange(currencyModelList);
                    if (errcode == 500001)
                    {
                        log.Info("All the exchange rates are updated successfully .");
                    }
                    else   if (errcode == 500002)
                    {
                        log.Info("All  the exchange rates are inserted Successfully.");
                    }
                    else
                    {
                        log.Error("problem For Saving Exchange Rate");
                    }
                    log.Info("System Finished Updating the Currency Exchange Rates. After " + pollingIntervel/60 + " Hours then will poll again.");
                    Thread.Sleep(pollingIntervel * 60 * 1000);
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }
    }
}
        public class CurrencyConvertorClass
        {
        public List<CurrencyModel> CurrencyGoogle()
    {

        List<string> CurrencyExchange = new List<string>();

        CurrencyConvertor obj = new CurrencyConvertor();
        CurrencyExchange = obj.currencyConvertor();

        List<string> unique = CurrencyExchange.Distinct().ToList();
        List<CurrencyModel> currencyModelList = new List<CurrencyModel>();
        List<string> Lst = new List<string>();
        AdminDal objdal = new AdminDal();
        Lst = objdal.CurrencyList();

        foreach (var item in Lst)
        {
            CurrencyModel currModel = new CurrencyModel();
            string apiURL = String.Format("https://www.google.com/finance/converter?a={0}&from={1}&to={2}&meta={3}", 1, "USD", item.ToUpper(), Guid.NewGuid().ToString());
            var request = WebRequest.Create(apiURL);

            var streamReader = new StreamReader(request.GetResponse().GetResponseStream(), System.Text.Encoding.ASCII);

            //Grab your converted value (ie 2.45 USD)
            string result = Regex.Matches(streamReader.ReadToEnd(), "<span class=\"?bld\"?>([^<]+)</span>")[0].Groups[1].Value;
            string ExchangeRate = result.Replace(item, "");
            currModel.Source = "www.google.com";
            currModel.CurrencyCode = item.ToString();
            currModel.ExchangeRate = Convert.ToDouble(ExchangeRate.ToString());
            currencyModelList.Add(currModel);


        }
        return currencyModelList;

    }
        }