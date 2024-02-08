using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.RDS
{
   public class RDSClientModel
    {
       [Display(Name = "Client Id")]
       public int ClientId { get; set; }

       [Display(Name = "Client Name")]
       public string ClientName { get; set; }

       [Display(Name = "Client Type")]
       public string ClientType { get; set; }

       [Display(Name = "Client Ref Id")]
       public string ClientRefId { get; set; }

       [Display(Name = "email")]
       public string email { get; set; }
       public string FTPDetails { get; set; }
       public string Server { get; set; }
       public string UserName { get; set; }
       public string Password { get; set; }
       public Int16 CreatedBy { get; set; }

       [Display(Name = "Created By")]
       public string CreatedByName { get; set; }

       [Display(Name = "Created Date")]
       public DateTime CreatedDate { get; set; }
       public Int16 ModifiedBy { get; set; }

        [Display(Name = "Modified By")]
       public string ModifiedByName { get; set; }

        [Display(Name = "Modified Date")]
       public DateTime ModifiedDate { get; set; }
    
    }
}
