using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.DataHub
{
    public class DHEvent
    {
        [Display(Name = "Event Id")]
        public Int16 EventId { get; set; }
          [Display(Name = "Event Name")]
        public string EventName { get; set; }
          [Display(Name = "Description")]
        public string Description { get; set; }
          [Display(Name = "Deleted")]
        public bool isDeleted { get; set; }
    }
}
