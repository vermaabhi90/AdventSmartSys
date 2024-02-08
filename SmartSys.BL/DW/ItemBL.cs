using SmartSys.BL.RGS;
using SmartSys.DAL;
using SmartSys.DL.DW;
using SmartSys.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.DW
{
    public class ItemBL
    {

        public List<ItemModel> GetItemList(string User_Id)
        {
            List<ItemModel> lstItem = new List<ItemModel>();
            try
            {
                DataSet dsItem = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsItem = DALObj.GetItemList(User_Id);
                if (dsItem != null)
                {
                    if (dsItem.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsItem.Tables[0].Rows)
                        {
                            ItemModel objmodel = new ItemModel();
                            objmodel.ItemId = Convert.ToInt32(dr["ItemId"]);
                            objmodel.ItemName = dr["ItemName"].ToString();
                            objmodel.BrandName = dr["BrandName"].ToString();
                            objmodel.SaleRate = Convert.ToDouble(dr["SaleRate"]);
                            objmodel.PurchaseRate = Convert.ToDouble(dr["PurchaseRate"]);
                            objmodel.Category = dr["Category"].ToString();
                            objmodel.ModifiedByName = dr["ModifiedByName"].ToString();
                            objmodel.CreatedBy = dr["CreatedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            objmodel.ModifiedDateStr = objmodel.ModifiedDate.ToShortDateString();
                            //objmodel.SAJMapStatus = Convert.ToInt16(dr["SAJMapStatus"].ToString());
                            //objmodel.DPKMapStatus = Convert.ToInt16(dr["DPKMapStatus"].ToString());
                            //objmodel.ADVENTMapStatus = Convert.ToInt16(dr["ADVENTMapStatus"].ToString());
                            //objmodel.BOMMapStatus = Convert.ToInt16(dr["BOMMapStatus"].ToString());
                            objmodel.MPN = dr["MPN"].ToString();
                            lstItem.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstItem;
        }
        public List<ItemDropDwnModel> GetAllItemDrp(string User_Id)
        {
            List<ItemDropDwnModel> lstItem = new List<ItemDropDwnModel>();
            try
            {
                DataSet dsItem = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsItem = DALObj.GetItemList(User_Id);
                DataColumn[] columns = dsItem.Tables[0].Columns.Cast<DataColumn>().ToArray();
                if (dsItem != null)
                {
                    if (dsItem.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsItem.Tables[0].Rows)
                        {
                            ItemDropDwnModel objmodel = new ItemDropDwnModel();                                                                           
                            objmodel.ItemId = Convert.ToInt32(dr["ItemId"]);
                            objmodel.Description = dr["ItemName"].ToString() + " - " + dr["MPN"].ToString() ;
                            lstItem.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstItem;
        }
        public bool CheckItemDuplicacy(string MPN,string User_Id)
        {
            bool Result = true;
            DataSet dsItem = new DataSet();
            ItemDAL DALObj = new ItemDAL();
            dsItem = DALObj.GetItemList(User_Id);
            DataColumn[] columns = dsItem.Tables[0].Columns.Cast<DataColumn>().ToArray();
            Result = dsItem.Tables[0].AsEnumerable()
                .Any(row => columns.Any(col => row[col].ToString() == MPN));
            return Result;
        }
        public List<ItemModel> GetFilterItemList(string MPN)
        {
            List<ItemModel> lstItem = new List<ItemModel>();
            try
            {
                DataSet dsItem = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsItem = DALObj.GetFilterItemList(MPN);
                if (dsItem != null)
                {
                    if (dsItem.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsItem.Tables[0].Rows)
                        {
                            ItemModel objmodel = new ItemModel();
                            objmodel.ItemId = Convert.ToInt32(dr["ItemId"]);
                            objmodel.ItemName = dr["ItemName"].ToString();
                            objmodel.BrandName = dr["ManufacturerName"].ToString();
                            objmodel.BrandId = Convert.ToInt32(dr["BrandId"]);
                            objmodel.MPN = dr["MPN"].ToString();
                            objmodel.CompanyPartNo = dr["CompanyPartNo"].ToString();
                            //objmodel.ModifiedByName = dr["ModifiedByName"].ToString();
                            //objmodel.CreatedBy = dr["CreatedBy"].ToString();
                            //objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            //objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            //objmodel.ModifiedDateStr = objmodel.ModifiedDate.ToShortDateString();
                            //objmodel.SAJMapStatus = Convert.ToInt16(dr["SAJMapStatus"].ToString());
                            //objmodel.DPKMapStatus = Convert.ToInt16(dr["DPKMapStatus"].ToString());
                            //objmodel.ADVENTMapStatus = Convert.ToInt16(dr["ADVENTMapStatus"].ToString());
                            //objmodel.BOMMapStatus = Convert.ToInt16(dr["BOMMapStatus"].ToString());                           
                            lstItem.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstItem;
        }
        public List<ItemModel> GetItemListwithMPNbyVendorId(int VendorId)
        {
            List<ItemModel> lstItem = new List<ItemModel>();
            try
            {
                DataSet dsItem = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsItem = DALObj.GetItemListwithMPNbyVendorId(VendorId);
                if (dsItem != null)
                {
                    if (dsItem.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsItem.Tables[0].Rows)
                        {
                            ItemModel objmodel = new ItemModel();
                            objmodel.ItemId = Convert.ToInt32(dr["ItemId"]);
                            objmodel.ItemName = dr["MPN"].ToString() + "    " + dr["ItemName"].ToString();
                            lstItem.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstItem;
        }
        public List<ItemModel> GetItemListFromCustQuotation(int custId)
        {
            List<ItemModel> lstItem = new List<ItemModel>();
            try
            {
                DataSet dsItem = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsItem = DALObj.GetItemListFromCustQuotation(custId);
                if (dsItem != null)
                {
                    if (dsItem.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsItem.Tables[0].Rows)
                        {
                            ItemModel objmodel = new ItemModel();
                            objmodel.ItemId = Convert.ToInt32(dr["ItemId"]);
                            objmodel.ItemName = dr["MPN"].ToString() + "    " + dr["ItemName"].ToString();
                            lstItem.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstItem;
        }
        public List<ItemModel> GetItemListwithMPN()
        {
            List<ItemModel> lstItem = new List<ItemModel>();
            try
            {
                DataSet dsItem = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsItem = DALObj.GetItemListfordrp();
                if (dsItem != null)
                {
                    if (dsItem.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsItem.Tables[0].Rows)
                        {
                            ItemModel objmodel = new ItemModel();
                            objmodel.ItemId = Convert.ToInt32(dr["ItemId"]);
                            objmodel.ItemName = dr["MPN"].ToString() + "    " + dr["ItemName"].ToString();
                            lstItem.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstItem;
        }
        public List<BudgetItemModel> GetBudgetItemList()
        {
            List<BudgetItemModel> lstItem = new List<BudgetItemModel>();
            try
            {
                DataSet dsItem = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsItem = DALObj.GetItemListfordrp();
                if (dsItem != null)
                {
                    if (dsItem.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsItem.Tables[0].Rows)
                        {
                            BudgetItemModel objmodel = new BudgetItemModel();
                            objmodel.ItemId = Convert.ToInt32(dr["ItemId"]);
                            objmodel.ItemName = dr["ItemName"].ToString();
                            lstItem.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstItem;
        }

        public List<ItemCategory> GetitemcategoryList()
        {
            List<ItemCategory> lstItemcatg = new List<ItemCategory>();
            try
            {
                DataSet dsItem = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsItem = DALObj.GetItemcategoryList();
                if (dsItem != null)
                {
                    if (dsItem.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsItem.Tables[0].Rows)
                        {
                            ItemCategory objmodel = new ItemCategory();
                            objmodel.CategoryId = Convert.ToInt32(dr["CategoryId"]);
                            objmodel.Category = dr["Category"].ToString();
                            lstItemcatg.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstItemcatg;
        }

        public ItemModel GetSelectedItem(int ItemId)
        {
            ItemModel itemModel = new ItemModel();
            try
            {
                DataSet dsItem = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsItem = DALObj.GetSelectedItem(ItemId);
                if (dsItem != null)
                {
                    if (dsItem.Tables.Count > 0)
                    {
                        itemModel.ItemId = Convert.ToInt32(dsItem.Tables[0].Rows[0]["ItemId"]);
                        itemModel.SaleRate = Convert.ToDouble(dsItem.Tables[0].Rows[0]["SaleRate"]);
                        itemModel.PurchaseRate = Convert.ToDouble(dsItem.Tables[0].Rows[0]["PurchaseRate"]);
                        itemModel.ItemName = dsItem.Tables[0].Rows[0]["ItemName"].ToString();
                        itemModel.BrandId = Convert.ToInt32(dsItem.Tables[0].Rows[0]["BrandId"].ToString());
                        itemModel.IsActive = Convert.ToBoolean(dsItem.Tables[0].Rows[0]["IsActive"]);
                        itemModel.CategoryId = Convert.ToInt32(dsItem.Tables[0].Rows[0]["CategoryId"].ToString());
                        itemModel.ModifiedByName = dsItem.Tables[0].Rows[0]["ModifiedByName"].ToString();
                        itemModel.ModifiedDate = Convert.ToDateTime(dsItem.Tables[0].Rows[0]["ModifiedDate"]);
                        itemModel.CreatedBy = dsItem.Tables[0].Rows[0]["CreatedBy"].ToString();
                        itemModel.CreatedDate = Convert.ToDateTime(dsItem.Tables[0].Rows[0]["CreatedDate"]);
                        itemModel.MPN = dsItem.Tables[0].Rows[0]["MPN"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);

            }
            return itemModel;
        }

        public List<BrandModel> brandList()
        {
            List<BrandModel> brandlist = new List<BrandModel>();
            try
            {
                ItemDAL objdal = new ItemDAL();
                DataSet ds = new DataSet();
                ds = objdal.BrandList();

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            BrandModel model = new BrandModel();
                            model.BrandId = Convert.ToInt32(dr["BrandId"].ToString());
                            model.BrandName = dr["BrandName"].ToString();
                            brandlist.Add(model);

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }
            return brandlist;
        }

        public int SaveItem(ItemModel itemModel, string User_Id)
        {
            int errCode = 0;
            try
            {
                DataSet dsItem = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsItem = DALObj.GetSelectedItem(0);
                if (dsItem != null)
                {
                    if (dsItem.Tables.Count > 0)
                    {
                        dsItem.Tables[0].Rows.Clear();
                        DataRow dr = dsItem.Tables[0].NewRow();
                        dr["ItemId"] = itemModel.ItemId;
                        dr["BrandId"] = itemModel.BrandId;
                        dr["CategoryId"] = itemModel.CategoryId;
                        dr["IsActive"] = itemModel.IsActive;
                        dr["SaleRate"] = itemModel.SaleRate;
                        dr["PurchaseRate"] = itemModel.PurchaseRate;
                        dr["ItemName"] = itemModel.ItemName;
                        dr["MPN"] = itemModel.MPN;
                        dsItem.Tables[0].Rows.Add(dr);
                        errCode = DALObj.SaveItem(dsItem, User_Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #region DW Item Mapping 
        public int GetAutoMapping(string User_Id, string CompCode, string ItemName, int ItemId, string SPName, ArrayList ConnArrList)
        {
            int errCode = 0;
            try
            {
                GeneratorBL objbl = new GeneratorBL();
                DataSet dsParam = new DataSet();
                DataTable dt = new DataTable();
                dt.Columns.Add("DataType", typeof(System.String));
                dt.Columns.Add("ParamName", typeof(System.String));
                dt.Columns.Add("ParamValue", typeof(System.String));
                DataRow dr = dt.NewRow();
                dr["DataType"] = "StringParameter";
                dr["ParamName"] = "ItemName";
                dr["ParamValue"] = ItemName;
                dt.Rows.Add(dr);
                dsParam.Tables.Add(dt);
                DataSet Itemds = new DataSet();
                Itemds = objbl.ExecuteUnderlayingSP(dsParam, SPName, ConnArrList);
                if (Itemds != null)
                {
                    if (Itemds.Tables.Count > 0)
                    {
                        if (Itemds.Tables[0].Rows.Count > 0)
                        {
                            ItemDAL DALObj = new ItemDAL();
                            DataSet dsDBData = new DataSet();
                            DataTable dtDBData = new DataTable("ItenNavRel");
                            dtDBData.Columns.Add("Item_No", typeof(System.String));
                            dtDBData.Columns.Add("CompCode", typeof(System.String));
                            dtDBData.Columns.Add("ItemId", typeof(System.Int32));

                            DataRow drDBData = dtDBData.NewRow();
                            drDBData["Item_No"] = Itemds.Tables[0].Rows[0]["Item_No"];
                            drDBData["CompCode"] = CompCode;
                            drDBData["ItemId"] = ItemId;
                            dtDBData.Rows.Add(drDBData);
                            dsDBData.Tables.Add(dtDBData);
                            string ErrorCode = DALObj.SaveMapping(dsDBData, User_Id);
                            if (ErrorCode == "500001" || ErrorCode == "500002")
                            {
                                string Description = "";
                                if (ErrorCode == "500001")
                                {
                                    Description = "Update Item " + ", " + CompCode + " , " + ItemName;
                                }
                                Description = "New Item " + ", " + CompCode + " , " + ItemName;
                                errCode = DALObj.SaveExtSysLog(Description, Itemds.Tables[0].Rows[0]["Item_No"].ToString(), User_Id);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        public List<ItemMappingModel> GetDWCompList()
        {
            List<ItemMappingModel> lstItemMap = new List<ItemMappingModel>();
            try
            {
                ItemDAL DALObj = new ItemDAL();
                DataSet dsObj = new DataSet();
                dsObj = DALObj.GetDWCompItemList();
                if (dsObj != null)
                {
                    if (dsObj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            ItemMappingModel MapObj = new ItemMappingModel();
                            MapObj.CompCode = dr["CompCode"].ToString();
                            MapObj.CompName = dr["CompanyName"].ToString();
                            MapObj.ConnectionId = Convert.ToInt16(dr["ConnectionId"]);
                            MapObj.ItemListSP = dr["ItemListSP"].ToString();
                            MapObj.SaveItemSP = dr["SaveItemSP"].ToString();
                            lstItemMap.Add(MapObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstItemMap;
        }
        public List<MapClientItemList> GetCompanyEmp(string CompCode, string SPName, ArrayList ConnInfo)
        {
            List<MapClientItemList> lstMapEmp = new List<MapClientItemList>();
            try
            {
                DataSet dsobj = new DataSet();
                AdminDal DALObj = new AdminDal();
                dsobj = DALObj.GetCompanyEmp(CompCode, SPName, ConnInfo);
                if (dsobj != null)
                {
                    if (dsobj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            MapClientItemList MapItemTemp = new MapClientItemList();
                            MapItemTemp.ClientItemID = dr["Item_No"].ToString();
                            MapItemTemp.ClientItemName = dr["ItemName"].ToString();
                            lstMapEmp.Add(MapItemTemp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstMapEmp;
        }
        public string SaveMapping(ItemMappingModel ItemMap, string User_Id)
        {
            string ErrorCode = "0";
            try
            {
                ItemDAL DALObj = new ItemDAL();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("tbl_DHFeedEventDep");
                dt.Columns.Add("Item_No", typeof(System.String));
                dt.Columns.Add("CompCode", typeof(System.String));
                dt.Columns.Add("ItemId", typeof(System.Int32));

                DataRow dr = dt.NewRow();
                dr["Item_No"] = ItemMap.ClientItemID;
                dr["CompCode"] = ItemMap.CompCode;
                dr["ItemId"] = ItemMap.ItemId;
                dt.Rows.Add(dr);
                dsObj.Tables.Add(dt);
                ErrorCode = DALObj.SaveMapping(dsObj, User_Id);
            }
            catch (Exception)
            {

                throw;
            }

            return ErrorCode;
        }
        public List<SysNavRelListModel> GetItemNavRelList(int ItemId)
        {
            List<SysNavRelListModel> lstEmpMap = new List<SysNavRelListModel>();
            try
            {
                DataSet dsobj = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsobj = DALObj.GetItemNavRelList(ItemId);
                if (dsobj != null)
                {
                    if (dsobj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            SysNavRelListModel MapTemp = new SysNavRelListModel();
                            MapTemp.CompItemID = dr["Item_No"].ToString();
                            MapTemp.CompName = dr["CompCode"].ToString();
                            MapTemp.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            MapTemp.ConnectionId = Convert.ToInt16(dr["ConnectionId"].ToString());
                            lstEmpMap.Add(MapTemp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstEmpMap;
        }
        public string GetClientItemName(string CompName, string CompItemID, ArrayList ConnInfo)
        {
            string str = "";
            try
            {
                ItemDAL DalObj = new ItemDAL();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetClientItemName(CompName, CompItemID, ConnInfo);
                if (dsObj != null)
                {
                    str = dsObj.Tables[0].Rows[0]["ItemName"].ToString();
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                str = "No Connection";
            }

            return str;
        }
        public int DeleteItemMapping(string CompName, int ItemId)
        {
            int errcod = 0;
            try
            {
                ItemDAL DALObj = new ItemDAL();
                errcod = DALObj.DeleteItemMapping(CompName, ItemId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errcod;
        }
        #endregion DW ItemMapping

        #region DW Approver
        public List<ItemApproverMap> GetItemApproverList(string User_Id, int Status_Id)
        {
            List<ItemApproverMap> ItemApproverList = new List<ItemApproverMap>();
            try
            {
                DataSet ds = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                ds = DALObj.GetItemApproverList(User_Id, Status_Id);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ItemApproverMap temp = new ItemApproverMap();
                            temp.ItemId = Convert.ToInt32(dr["ItemId"]);
                            temp.ItemName = dr["ItemName"].ToString();
                            temp.Item_Id = dr["Item_No"].ToString();
                            temp.Item_Name = dr["SourceItemName"].ToString();
                            temp.CompName = dr["CompCode"].ToString();
                            temp.ModifiedBy = dr["ModifiedBy"].ToString();
                            ItemApproverList.Add(temp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ItemApproverList;
        }
        public int ApvRejItem(ItemApproverMap itemApproveModel, int SttusCode, string User_Id)
        {
            int errCode = 0;
            try
            {
                ItemDAL DALObj = new ItemDAL();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("ApvRejItem");
                dt.Columns.Add("Item_Id", typeof(System.String));
                dt.Columns.Add("CompCode", typeof(System.String));
                dt.Columns.Add("StatusCode", typeof(System.Int32));
                foreach (SelectPenddingApproveList demo in itemApproveModel.SelectApproverLst)
                {
                    DataRow dr = dt.NewRow();
                    dr["Item_Id"] = demo.Item_Id;
                    dr["CompCode"] = demo.CompName;
                    dr["StatusCode"] = SttusCode;
                    dt.Rows.Add(dr);
                }
                dsObj.Tables.Add(dt);
                errCode = DALObj.ApvRejItem(dsObj, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion DW Item Approver

        #region DW Item Approver Review
        public int ApvRejItemReview(ItemApproverMap itemApproveModel, int SttusCode, string User_Id)
        {
            int errCode = 0;
            try
            {
                ItemDAL DALObj = new ItemDAL();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("ApvRejItem");
                dt.Columns.Add("Item_Id", typeof(System.String));
                dt.Columns.Add("CompCode", typeof(System.String));
                dt.Columns.Add("StatusCode", typeof(System.Int32));
                foreach (SelectPenddingApproveList demo in itemApproveModel.SelectApproverLst)
                {
                    DataRow dr = dt.NewRow();
                    dr["Item_Id"] = demo.Item_Id;
                    dr["CompCode"] = demo.CompName;
                    dr["StatusCode"] = SttusCode;
                    dt.Rows.Add(dr);
                }
                dsObj.Tables.Add(dt);
                errCode = DALObj.ApvRejItemReview(dsObj, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion DW Item Approver Review

        #region ItemBrandAllocation
        public ItemBrandAllocationDetail GetItemBrandAllocation(int ItemId)
        {
            ItemBrandAllocationDetail model = new ItemBrandAllocationDetail();
            model.TotalItemBrand = new List<ItemBrandAllocationDetail.DerivedItemBrand>();
            model.AssignedItemBrand = new List<ItemBrandAllocationDetail.DerivedItemBrand>();

            try
            {
                DataSet dsItem = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsItem = DALObj.GetBrandListForItem(ItemId);
                if (dsItem != null)
                {
                    if (dsItem.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsItem.Tables[1].Rows)
                        {
                            ItemBrandAllocationDetail.DerivedItemBrand objmodel = new ItemBrandAllocationDetail.DerivedItemBrand();
                            objmodel.BrandId = Convert.ToInt32(dr["BrandId"]);
                            objmodel.BrandName = dr["BrandName"].ToString();
                            model.TotalItemBrand.Add(objmodel);
                        }

                        foreach (DataRow dr in dsItem.Tables[0].Rows)
                        {
                            ItemBrandAllocationDetail.DerivedItemBrand objmodel = new ItemBrandAllocationDetail.DerivedItemBrand();
                            objmodel.BrandId = Convert.ToInt32(dr["BrandId"]);
                            objmodel.BrandName = dr["BrandName"].ToString();
                            model.AssignedItemBrand.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return model;
        }
        public int SaveItemBrand(ItemBrandAllocationDetail ItemBrand, int ItemId, string User_Id)
        {
            int iErrorCode = 0;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable("DW_ItemBrand");
                dt.Columns.Add("ItemId", typeof(System.Int32));
                dt.Columns.Add("BrandId", typeof(System.Int32));
                if (ItemBrand.LstBrandItemDetail != null)
                {
                    foreach (ItemBrandAllocationDetail.ItemBrandDetail AreaCityMap in ItemBrand.LstBrandItemDetail)
                    {
                        DataRow dr = dt.NewRow();
                        dr["ItemId"] = AreaCityMap.ItemId;
                        dr["BrandId"] = AreaCityMap.BrandId;
                        dt.Rows.Add(dr);
                    }
                }
                ds.Tables.Add(dt);
                ItemDAL DalObj = new ItemDAL();
                iErrorCode = DalObj.SaveItemBrand(ds, ItemId, User_Id);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return iErrorCode;
        }

        #endregion ItemBrandAllocation
    }
}
