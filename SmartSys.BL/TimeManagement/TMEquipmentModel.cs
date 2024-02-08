using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.TimeManagement
{
    public class TMEquipmentModel
    {
        public int Id { get; set; }
        public int ETAM { get; set; }
        public int Tier1Tam { get; set; }
        public int OemTam { get; set; }
        public Int32 EquipmentId { get; set; }
        public string EquipmentIdstr { get; set; }
        public string EquipmentName { get; set; }
        public string Description { get; set; }
        public Int32 ItemId { get; set; }
        public string CurrencyCodes { get; set; }
        public int ProjectId { get; set; }
        public double TAM { get; set; }
        public string TAMSource { get; set; }
        public DateTime TAMDate { get; set; } 
        public double Rate { get; set; }
        public int ParentEquipmentId { get; set; }
        public string ParentEquipmentName { get; set; }
        public int SegmentId { get; set; }
        public string  SegmentName { get; set; }
        public string ProjectName { get; set; }       
        public Int32 ModifiedBy { get; set; }
        public double Quantity { get; set; }
        public double PurchaseRate { get; set; }
        public double SaleRate { get; set; }
        public double AdventRate { get; set; }
        public double CustomerPotential { get; set; }
        public double AdventPotential { get; set; }      
        public double AdventCost { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedByName { get; set; }
        public List<TMEquipmentItem> lstItems { get; set; }
        public List<TMEquipmentModel> lstEquipment { get; set; }      
        public List<SegmentList> lstSegment { get; set; }
    }
    public class SegmentList
    {
        public int SegmentId { get; set; }
        public string SegmentName { get; set; }
        public Int32 EquipmentId { get; set; }
        public string EquipmentName { get; set; }
    }
    public class CurrencyCodes
    {
      
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class TMEquipmentItem
    {
        public Int32 EquipmentId { get; set; }
        public Int32 ItemId { get; set; }
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public int ProjectId { get; set; }
        public double PurchaseRate { get; set; }
        public double SaleRate { get; set; }
        public double TAM { get; set; }        
        public string ProjectName { get; set; } 
        public double Quantity { get; set; }
        public Int32 ModifiedBy { get; set; }
        public string ModifiedById { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedByName { get; set; }
        public List<TMEquipmentItem> lstEquipItems { get; set; }
    }
}
