using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SmartSys.BL.DataHub
{
   public class DHLoaderModel
    {
       [Display(Name = "Loader Id")]
       public Int16 LoaderId { get; set; }

        [Display(Name = "Job Polling Interval")]
       public Int16 JobPollingInterval { get; set; }

        [Display(Name = "Event Polling Interval")]
       public Int16 EventPollingInterval { get; set; }

         [Display(Name = "Trigger Polling Interval")]
       public Int16 TriggerPollingInterval { get; set; }

        [Display(Name = "Status")]
       public Int16 StatusId { get; set; }
       public string StatusShortCode { get; set; }

        [Display(Name = "is Active")]
       public Boolean isActive { get; set; }
       public Int16 BusinessLineId { get; set; }

        [Display(Name = "Business Line")]
       public string BusinessLineName { get; set; }

        [Display(Name = "Host")]
       public string Host { get; set; }

        [Display(Name = "Port")]
       public string Port { get; set; }

        [Display(Name = "Loader Type")]
       public string LoaderType { get; set; }

        public int ProcessID { get; set; }
       public List<LoaderTypeModel> lstLoader { get; set; }

    }
    public class LoaderTypeModel
    {
        public string LoaderType { get; set; }
        public string LoaderValue { get; set; }
    }

    public class ConnectionModel
    {
        public Int16 ConnectionId { get; set; }
        public string ConnectionType { get; set; }
        public string ServerName { get; set; }
        public string DBName { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        [Display(Name = "Table Name")]
        public string TableName { get; set; }

        [Display(Name = "File Type")]
        public string FileType { get; set; }

        [Display(Name = "No. of Column")]
        [DefaultValue(2)]
        public int ColumnNo { get; set; }

        [Display(Name = "No. of Header")]
        public int Header { get; set; }

        [Display(Name = "No. of Footer")]
        public int Footer { get; set; }

        [Display(Name = "Separator")]
        public char Separator { get; set; }

        [Display(Name = "Line Separator")]
        public char LineSeparator { get; set; }

        [Display(Name = "Col Position")]
        public int ColPosition { get; set; }
    }

}
