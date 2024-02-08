using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.DW
{
    public class ItemModel
    {
        [DisplayName("Item Id")]
        public int ItemId { get; set; }
        public int CategoryId { get; set; }
        public double PurchaseRate { get; set; }
        public double SaleRate { get; set; }
        public string Category { get; set; }

        [DisplayName("Item Name")]
        public string ItemName { get; set; }

        [DisplayName("Modified By")]
        public string ModifiedByName { get; set; }
        public string CreatedBy { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string MPN { get; set; }
        public string CompanyPartNo { get; set; }
        public bool IsActive { get; set; }
        public string ModifiedDateStr { get; set; }
        public Int16 SAJMapStatus { get; set; }
        public Int16 DPKMapStatus { get; set; }
        public Int16 ADVENTMapStatus { get; set; }
        public Int16 BOMMapStatus { get; set; }
        public List<ItemModel> lstItem { get; set; }
    }
    public class ItemDropDwnModel
    {
        public int  ItemId { get; set; }
        public string Description { get; set; }
    }
    public class ItemMappingModel
    {
        public string CompCode { get; set; }
        public string CompName { get; set; }
        public string ClientItemID { get; set; }
        public Int16 ConnectionId { get; set; }
        public string ItemListSP { get; set; }
        public string SaveItemSP { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string MPN { get; set; }
        public string Brand { get; set; }
        public string selectedComp { get; set; }
        public List<ItemMappingModel> lstItemMap { get; set; }
        public List<SysNavRelListModel> GridList { get; set; }
        public List<MapClientItemList> lstClientMapItemList { get; set; }
        public string AutoMapBtn { get; set; }
    }
    public class MapClientItemList
    {
        public string ClientItemID { get; set; }
        public string ClientItemName { get; set; }
    }
    public class SysNavRelListModel
    {
        public string CompItemID { get; set; }
        public string CompName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public Int16 ConnectionId { get; set; }
    }
    public class ItemApproverMap
    {
        [Display(Name = "Item Id")]
        public int ItemId { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "External Item Id")]
        public string Item_Id { get; set; }

        [Display(Name = "External Item Name")]
        public string Item_Name { get; set; }

        [Display(Name = "Company Name")]
        public string CompName { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }
        public List<ItemApproverMap> lstApprover { get; set; }
        public List<SelectPenddingApproveList> SelectApproverLst { get; set; }
    }
    public class SelectPenddingApproveList
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Item_Id { get; set; }
        public string Item_Name { get; set; }
        public string CompName { get; set; }
        public string ModifiedBy { get; set; }
    }
    public class ItemCategory
    {
        public int CategoryId { get; set; }
        public string Category { get; set; }
     
    }
    #region Item allocation
    public class ItemBrandAllocationDetail
    {
        public class ItemBrandDetail
        {
            public int ItemId { get; set; }
            public int BrandId { get; set; }
            public string Brand { get; set; }

        }
        public class DerivedItemBrand
        {
            public int BrandId { get; set; }
            public string BrandName { get; set; }

        }
        public int ItemId { get; set; }
        public int BrandId { get; set; }
        public int VendorId { get; set; }
        public List<DerivedItemBrand> TotalItemBrand { get; set; }
        public List<DerivedItemBrand> AssignedItemBrand { get; set; }
        public List<ItemBrandDetail> LstBrandItemDetail { get; set; }

    }

     public class BrandModel
     {
         public int BrandId { get; set; }
         public string BrandName { get; set; }
         public string CreatedBy { get; set; }
         public DateTime CreatedDate { get; set; }
     }
    #endregion item Allocation
}
