using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.Loader
{
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
           public int Header  { get; set; }

           [Display(Name = "No. of Footer")]
           public int Footer { get; set; }

           [Display(Name = "Separator")]
           public char Separator { get; set; }

           [Display(Name = "Line Separator")]
           public char LineSeparator { get; set; }

           [Display(Name = "Col Position")]
           public int ColPosition { get; set; }
        }
    public class FeedMasterModel
    {
        public int FeedId { get; set; }
        public string FeedName { get; set; }
    }

    

}
