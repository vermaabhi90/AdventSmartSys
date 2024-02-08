using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.SysDBCon
{
   public  class SysDBConModel
    {
        [DisplayName("Id")]
       public Int16 ConnectionId { get; set; }
        [DisplayName("Name")]
        public string ConName { get; set; }

        [DisplayName("Server Name")]
       public string ServerName { get; set; }

        [DisplayName("Port")]
       public string Port { get; set; }

        [DisplayName("DB Name")]
       public string DBName { get; set; }

        [DisplayName("User Name")]
       public string UserName { get; set; }

       public string Password { get; set; }

        [DisplayName("Time Out")]
       public int DefaultTimeOut { get; set; }

        [DisplayName("Conn Type")]
       public string ConnectionType { get; set; }
       public Int16 CreatedBy { get; set; }
       public DateTime CreatedDate { get; set; }
       public Int16 ModifiedBy { get; set; }

        [DisplayName("Modified Date")]
       public DateTime ModifiedDate { get; set; }
       public string CreatedByName { get; set; }

        [DisplayName("Modified By")]
       public string ModifiedByName { get; set; }
   
   }
}
