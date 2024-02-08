using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Net.Mail;
using System.Collections;
using System.IO;
using System.Globalization;

namespace SmartSys.Utility
{
    public static class Common
    {
        public static string GetTimespanForMailEnquiry(int Minutes)
        {
            string Timespan = Minutes.ToString();
            TimeSpan ts = TimeSpan.FromTicks(2000);
            TimeSpan ts1 = TimeSpan.FromHours(2000);
            TimeSpan ts2 = TimeSpan.FromDays(2000);
            TimeSpan ts3 = TimeSpan.FromMinutes((double)3040);
            string x = ts2.ToString();
            var hour1 = ts3.TotalHours;
            var t = hour1 % 1;
            var Days1 = ts3.TotalDays;
            var f = String.Format("{0:0.00}", hour1);

            if (Minutes > 60)
            {
                Double Hours = (double)Minutes / 60;
                double min = Hours % 1;
                // TimeSpan span = TimeSpan.FromMinutes(Minutes);
                //string Hour = span.ToString(@"hh\.mm");
                Timespan = String.Concat(Hours, "   Hours"); ;
                if (Convert.ToDouble(Hours) > 24)
                {

                    Double Days = (double)Hours / 24;
                    Timespan = String.Concat(Days, "   Days"); ;

                    double hour = Days % 1;
                    if (Days > 7)
                    {
                        Double Week = (double)Days / 7;
                        Timespan = String.Concat(Week, "   Weeks");

                    }
                }
            }
            return Timespan;
        }
        public static string SqlConnectionString = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
        public static void LogException(System.Exception e)
        {
            try
            {
                //CustomException.ExceptionMgr.PublishException(e);
            }
            catch
            { }
        }
        public static ArrayList GetCCMail()
        {
            ArrayList CCList = new ArrayList();
            CCList.Add("purchase@adventelectronics.com");
            CCList.Add("Sourcing@adventelectronics.com");
            return CCList;
        }
        public static string GetCEOId()
        {
            string EmailId;
            EmailId = "ravi@adventelectronics.com";
            return EmailId;
        }
        public static string GetConfigValue(string KeyString)
        {
            string DynamicSetting;
            DynamicSetting = ConfigurationManager.AppSettings[KeyString];
            return DynamicSetting;
        }
        public static int CountDays(DayOfWeek day, DateTime start, DateTime end)
        {
            TimeSpan ts = end - start;                       // Total duration
            int count = (int)Math.Floor(ts.TotalDays / 7);   // Number of whole weeks
            int remainder = (int)(ts.TotalDays % 7);         // Number of remaining days
            int sinceLastDay = (int)(end.DayOfWeek - day);   // Number of days since last [day]
            if (sinceLastDay < 0) sinceLastDay += 7;         // Adjust for negative days since last [day]

            // If the days in excess of an even week are greater than or equal to the number days since the last [day], then count this one, too.
            if (remainder >= sinceLastDay) count++;

            return count;
        }
        public static DateTime NthOf(this DateTime CurDate, int Occurrence, DayOfWeek Day)
        {
            var fday = new DateTime(CurDate.Year, CurDate.Month, 1);

            var fOc = fday.DayOfWeek == Day ? fday : fday.AddDays(Day - fday.DayOfWeek);
            // CurDate = 2011.10.1 Occurance = 1, Day = Friday >> 2011.09.30 FIX. 
            if (fOc.Month < CurDate.Month) Occurrence = Occurrence + 1;
            return fOc.AddDays(7 * (Occurrence - 1));
        }
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        public static int SendMail(string subject, string body, string Filename, string toMailAdd, string ccMailId, ArrayList attachments)
        {
            int ErrCode = 0;
            try
            {
                String Server = Common.GetConfigValue("EmailServer");
                String username = Common.GetConfigValue("EmailUserName");
                String password = Common.GetConfigValue("EmailPassword");
                String EmailPort = Common.GetConfigValue("EmailPort");
                String SupportMail = Common.GetConfigValue("SupportMail");

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Server);

                mail.From = new MailAddress(username);
                if (toMailAdd.ToString() != "")
                {
                    mail.To.Add(toMailAdd);
                }

                mail.CC.Add(SupportMail);
                if (ccMailId.Length > 3)
                    if (ccMailId.ToString() != "")
                    {
                        mail.CC.Add(ccMailId);
                    }
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                if (attachments != null)
                {
                    for (int i = 0; i < attachments.Count; i++)
                    {
                        Attachment attachment = (Attachment)attachments[i];
                        mail.Attachments.Add(attachment);
                    }
                }
                SmtpServer.Port = Convert.ToInt32(EmailPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
                if (Server.ToString() == "smtp.gmail.com")
                {
                    SmtpServer.EnableSsl = true;
                }
                else
                {
                    SmtpServer.EnableSsl = false;
                }
                SmtpServer.Send(mail);
                mail.Dispose();
                mail = null;
                ErrCode = 500001;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ErrCode;
        }
        public static int SendMailDynamic(string subject, string body, string Filename, string toMailAdd, string ccMailId, ArrayList attachments, string Server, string username, string password, string EmailPort, string BCC,bool SSL)
        {
            int ErrCode = 0;
            SmtpClient SmtpServer = new SmtpClient(Server);
            try
            {
                if (Server == "" && username == "" && password == "" && EmailPort == "")
                {
                    Server = Common.GetConfigValue("EmailServer");
                    username = Common.GetConfigValue("EmailUserName");
                    password = Common.GetConfigValue("EmailPassword");
                    EmailPort = Common.GetConfigValue("EmailPort");
                }

                String SupportMail = Common.GetConfigValue("SupportMail");

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(username);
                mail.To.Add(toMailAdd);
                mail.CC.Add(username);
                if (ccMailId.Length > 3)
                    mail.CC.Add(ccMailId);
                if (BCC.Length > 3)
                    mail.Bcc.Add(BCC);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                if (attachments != null)
                {
                    for (int i = 0; i < attachments.Count; i++)
                    {
                        Attachment attachment = (Attachment)attachments[i];
                        mail.Attachments.Add(attachment);
                    }
                }
                SmtpServer.Port = Convert.ToInt32(EmailPort);
                SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
                if (Server.ToString() == "smtp.gmail.com" || Server.ToString() == "smtp.zoho.com")
                {
                    SmtpServer.EnableSsl = true;
                }
                else
                {
                    SmtpServer.EnableSsl = false;
                }
                if (SSL)
                    SmtpServer.EnableSsl = SSL;
                SmtpServer.Send(mail);
                SmtpServer.Dispose();
                mail.Dispose();
                mail = null;
                ErrCode = 500001;
            }
            catch (Exception ex)
            {
                SmtpServer.Dispose();
                throw ex;
            }
            return ErrCode;
        }
        public static ArrayList GetConnectionInfoFromWebConfig()
        {
            ArrayList ConnInfo = new ArrayList();
            string Connstring = ConfigurationManager.ConnectionStrings["SqlConnectionString"].ConnectionString;
            string[] str = Connstring.Split(';');
            string[] SerName = str[0].ToString().Split('=');
            string[] DBName = str[1].ToString().Split('=');
            string[] UId = str[2].ToString().Split('=');
            string[] Pass = str[3].ToString().Split('=');
            ConnInfo.Add(SerName[1].ToString());
            ConnInfo.Add(DBName[1].ToString());
            ConnInfo.Add(UId[1].ToString());
            ConnInfo.Add(Pass[1].ToString());
            return ConnInfo;
        }
        public static String GetTimeINHHMM(int takeMin)
        {
            string TimeTaken = "";
            if (takeMin == 0)
            {
                TimeTaken = "00:00";
            }
            else
            {
                string MinStr = "";
                int remMin = takeMin % 60;
                if (remMin < 10)
                {
                    MinStr = "0" + remMin.ToString();
                }
                else
                {
                    MinStr = remMin.ToString();
                }
                int Remhr = (takeMin / 60) % 24;
                int Totalhr = (takeMin / 60) / 24;
                TimeTaken = Convert.ToString((Totalhr * 8) + Remhr) + ":" + MinStr.ToString();
            }
            return TimeTaken;
        }
    }

    [DataContract]
    public class FaultExceptionError
    {
        [DataMember]
        public bool Result { get; set; }

        [DataMember]
        public String ErrorMsg { get; set; }

        [DataMember]
        public string Description { get; set; }


    }

    public enum ErroCode
    {
        UserAlreadyPresent = 600001,
        InsertSuccessfully = 600002,
        ModifiedSuccessfully = 600003

    }
    public class finanacialYear
    {

        public List<string> FY()
        {

            List<string> FyYear = new List<string>();
            int i; int z = 10; int y = 9;
            DateTime dateTime = DateTime.Now;
            for (i = 0; i <= 10; i++)
            {
                z--; y--;
                string x = "FY:" + ((dateTime.Year - z).ToString() + '-' + (dateTime.Year - y).ToString());
                FyYear.Add(x);
            }
            return FyYear;
        }




    }


    public class CurrencyConvertor
    {
        public List<string> currencyConvertor()
        {
            List<string> currencyCode = new List<string>();
            foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo country = new RegionInfo(culture.LCID);
                String Currency = country.ISOCurrencySymbol;
                currencyCode.Add(Currency);
            }
            return currencyCode;
        }
    }



}
