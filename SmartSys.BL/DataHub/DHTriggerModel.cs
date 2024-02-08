using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.DataHub
{
    public class DHTriggerModel
    {
        [Display(Name = "Trigger Id")]
        public Int16 TriggerId { get; set; }

         [Display(Name = "Trigger Name")]
        public string TriggerName { get; set; }

         [Display(Name = "Description")]
        public string Description { get; set; }
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
