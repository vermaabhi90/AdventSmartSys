using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.Sysadmin
{
   public  class BusinessLineModel
    {
        [Display(Name = "Business Line Id")]
       public Int16 BusinessLineId { get; set; }
       [Display(Name = "Business Line Name")]
       public string BusinessLineName { get; set; }
       [Display(Name = "Description")]
       public string Description { get; set; }
       public Int16 CreatedBy { get; set; }
       [Display(Name = "Created Date")]
       public DateTime CreatedDate { get; set; }
       public Int16 ModifiedBy { get; set; }
       [Display(Name = "ModifiedDate")]
       public DateTime ModifiedDate { get; set; }
       [Display(Name = "Created By")]
       public string CreatedByName { get; set; }
       [Display(Name = "Modified By")]
       public string ModifiedByName { get; set; }
    }
}
