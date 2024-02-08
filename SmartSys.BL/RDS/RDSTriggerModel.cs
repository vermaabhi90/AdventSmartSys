using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.RDS
{
  public class RDSTriggerModel
    {
      public Int16 TriggerId { get; set; }
      public string TriggerName { get; set; }
      public string Description { get; set; }
      public Int16 CreatedBy { get; set; }
      public string  CreatedByName { get; set; }
      public DateTime CreatedDate { get; set; }
      public Int16 ModifiedBy { get; set; }
      public string ModifiedByName { get; set; }
      public DateTime ModifiedDate { get; set; }
    }
}
