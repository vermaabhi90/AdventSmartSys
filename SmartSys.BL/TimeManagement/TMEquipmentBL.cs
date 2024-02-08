using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SmartSys.DL.TimeManagement;

namespace SmartSys.BL.TimeManagement
{
    public class TMEquipmentBL
    {
        public List<TMEquipmentModel> GetTMEquipmentList()
        {
            List<TMEquipmentModel> lstEquipments = new List<TMEquipmentModel>();
            lstEquipments = new List<TMEquipmentModel>();
            TMEquipmentDal objDal = new TMEquipmentDal();
            DataSet dsData = objDal.GetTMEquipmentList();
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(dr["ParentEquipmentId"].ToString()) == 0)
                        {
                            TMEquipmentModel tmObj = new TMEquipmentModel();
                            tmObj.Id = Convert.ToInt32(dr["Id"].ToString());
                            tmObj.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                            tmObj.EquipmentName = dr["EquipmentName"].ToString();
                            tmObj.Description = dr["Description"].ToString();
                            tmObj.TAM = Convert.ToDouble(dr["TAM"].ToString());
                            tmObj.CurrencyCodes = dr["CurrencyCode"].ToString();
                            tmObj.Rate = Convert.ToDouble(dr["Rate"].ToString());
                            tmObj.ParentEquipmentId = Convert.ToInt32(dr["ParentEquipmentId"].ToString());
                            tmObj.SegmentId = Convert.ToInt32(dr["SegmentId"].ToString());
                            tmObj.ModifiedByName = dr["ModifiedByName"].ToString();
                            tmObj.ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString());
                            tmObj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            if (tmObj.EquipmentName != "#Item")
                                tmObj.lstEquipment = GetChildData(tmObj.Id, dsData);
                            lstEquipments.Add(tmObj);
                        }
                    }
            return lstEquipments;
        }
        public List<TMEquipmentModel> GetChildData(int id, DataSet dsData)
        {
            List<TMEquipmentModel> lstEquipmentsChild = new List<TMEquipmentModel>();
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(dr["ParentEquipmentId"].ToString()) == id)
                        {
                            TMEquipmentModel tmObj = new TMEquipmentModel();
                            tmObj.Id = Convert.ToInt32(dr["Id"].ToString());
                            tmObj.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                            tmObj.EquipmentName = dr["EquipmentName"].ToString();
                            tmObj.Description = dr["Description"].ToString();
                            tmObj.TAM = Convert.ToDouble(dr["TAM"].ToString());
                            tmObj.CurrencyCodes = dr["CurrencyCode"].ToString();
                            tmObj.Rate = Convert.ToDouble(dr["Rate"].ToString());
                            tmObj.ParentEquipmentId = Convert.ToInt32(dr["ParentEquipmentId"].ToString());
                            tmObj.SegmentId = Convert.ToInt32(dr["SegmentId"].ToString());
                            tmObj.ModifiedByName = dr["ModifiedByName"].ToString();
                            tmObj.ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString());
                            tmObj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            if (tmObj.EquipmentName != "#Item")
                                tmObj.lstEquipment = GetChildData(tmObj.Id, dsData);
                            lstEquipmentsChild.Add(tmObj);
                        }
                    }
            return lstEquipmentsChild;
        }


        public List<TMEquipmentItem> GetEquipmentItemList()
        {
            TMEquipmentDal objDal = new TMEquipmentDal();
            DataSet dsData = objDal.GetTMEquipmentItemList();
            List<TMEquipmentItem> lstItems = new List<TMEquipmentItem>();
            if (dsData.Tables.Count > 0)
            {
                foreach (DataRow dr in dsData.Tables[0].Rows)
                {
                    TMEquipmentItem objItem = new TMEquipmentItem();
                    objItem.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                    objItem.Quantity = Convert.ToDouble(dr["Quantity"].ToString());
                    objItem.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                    objItem.ItemName = dr["ItemName"].ToString();
                    objItem.ModifiedBy = Convert.ToInt32(dr["ModifiedBy"].ToString());
                    objItem.ModifiedByName = dr["ModifiedByName"].ToString();
                    lstItems.Add(objItem);
                }
            }
            return lstItems;
        }
        public List<TMEquipmentItem> GetselectedEquipmentItem(int EquipmentId)
        {
            TMEquipmentModel objmodel = new TMEquipmentModel();
            objmodel.lstItems = new List<TMEquipmentItem>();
            try
            {
                TMEquipmentDal objDal = new TMEquipmentDal();
                DataSet dsData = objDal.GetSelectedEquipmentItem(EquipmentId);
                if(dsData!= null)
                    foreach(DataRow dr in dsData.Tables[0].Rows)
                    {

                        TMEquipmentItem EItem = new TMEquipmentItem();

                        EItem.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                        EItem.ItemName = dr["ItemName"].ToString();
                        EItem.MPN = dr["MPN"].ToString();
                        EItem.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                        EItem.Quantity = Convert.ToDouble(dr["Quantity"].ToString());
                        EItem.SaleRate = Convert.ToDouble(dr["SaleRate"].ToString());
                        EItem.PurchaseRate = Convert.ToDouble(dr["PurchaseRate"].ToString());
                        EItem.ModifiedById = dr["ModifiedById"].ToString();
                        EItem.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        objmodel.lstItems.Add(EItem);
                    }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return objmodel.lstItems;
        }
        public DataSet GetItemPurchaseSalesRate(int ItemId)
        {
            DataSet ds = new DataSet();

            TMEquipmentDal objDal = new TMEquipmentDal();
            try
            {
                ds = objDal.GetItemPurchaseSalesRate(ItemId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public List<CurrencyCodes> GetCurrencyCodes()
        {
            List<CurrencyCodes> lstModel = new List<CurrencyCodes>();
            try
            {
                TMEquipmentDal objDal = new TMEquipmentDal();
                DataSet dsData = objDal.GetCurrencyCodes();
                if (dsData != null)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {

                        CurrencyCodes CurrencyModel = new CurrencyCodes();

                        CurrencyModel.Key = dr["Key"].ToString();
                        CurrencyModel.Value = dr["Value"].ToString();
                        lstModel.Add(CurrencyModel);
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstModel;
        }
        public List<SegmentList> GetParentEquipmentList(int EquipmentId)
        {
            List<SegmentList> lstModel = new List<SegmentList>();
            try
            {
                TMEquipmentDal objDal = new TMEquipmentDal();
                DataSet dsData = objDal.GetParentEquipmentList(EquipmentId);
                if (dsData != null)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {

                        SegmentList EquipmentModel = new SegmentList();

                        EquipmentModel.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                        EquipmentModel.EquipmentName = dr["EquipmentName"].ToString();
                        lstModel.Add(EquipmentModel);
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstModel;
        }
        public int saveEquipmentDetails( TMEquipmentModel EquipmentModel, string User_Id)
        {

            TMEquipmentDal objDal = new TMEquipmentDal();
            DataSet ds = new DataSet();
            try
            {
                ds = objDal.GetSelectedEquipment(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow Dr = ds.Tables[0].NewRow();
                    Dr["EquipmentId"] = EquipmentModel.EquipmentId;
                    Dr["CurrencyCode"] = EquipmentModel.CurrencyCodes;
                    Dr["EquipmentName"] = EquipmentModel.EquipmentName;
                    Dr["Description"] = EquipmentModel.Description;
                    Dr["ParentEquipmentId"] = EquipmentModel.ParentEquipmentId;
                    Dr["TAM"] = EquipmentModel.TAM;
                    Dr["TAMDate"] = EquipmentModel.TAMDate;
                    Dr["TAMSource"] = EquipmentModel.TAMSource;
                    Dr["Rate"] = EquipmentModel.Rate;
                    ds.Tables[0].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objDal.SaveEquipmentDetails(ds, User_Id);
        }

        public TMEquipmentModel GetSelectedEquipment(int EquipmentId)
        {
            TMEquipmentModel EquipmentModel = new TMEquipmentModel();
            EquipmentModel.lstSegment = new List<SegmentList>();
            try
            {
                TMEquipmentDal objDal = new TMEquipmentDal();
                DataSet ds = new DataSet();
                ds = objDal.GetSelectedEquipment(EquipmentId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            EquipmentModel.EquipmentId = Convert.ToInt32(ds.Tables[0].Rows[0]["EquipmentId"].ToString());
                            EquipmentModel.EquipmentName = ds.Tables[0].Rows[0]["EquipmentName"].ToString();
                            EquipmentModel.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                            EquipmentModel.ParentEquipmentId = Convert.ToInt32(ds.Tables[0].Rows[0]["ParentEquipmentId"].ToString());
                            EquipmentModel.ModifiedByName = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            EquipmentModel.CurrencyCodes = ds.Tables[0].Rows[0]["CurrencyCode"].ToString();
                            EquipmentModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                            EquipmentModel.TAM = Convert.ToDouble(ds.Tables[0].Rows[0]["TAM"].ToString());
                            EquipmentModel.Rate = Convert.ToDouble(ds.Tables[0].Rows[0]["Rate"].ToString());
                            EquipmentModel.TAMSource = ds.Tables[0].Rows[0]["TAMSource"].ToString();
                            EquipmentModel.ParentEquipmentName = ds.Tables[0].Rows[0]["ParentEquipmentName"].ToString();
                            if (ds.Tables[0].Rows[0]["TAMDate"].ToString()!="")
                             EquipmentModel.TAMDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["TAMDate"].ToString());
                        }

                    }
                    if (ds.Tables.Count > 1)
                    {
                        foreach(DataRow dr in ds.Tables[1].Rows)
                        {
                            SegmentList model = new SegmentList();
                            model.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                            model.EquipmentName = dr["EquipmentName"].ToString();
                            model.SegmentName = dr["SegmentName"].ToString();
                            model.SegmentId = Convert.ToInt32(dr["SegmentId"].ToString());
                            EquipmentModel.lstSegment.Add(model);
                        }
                    }
                    if (ds.Tables.Count > 2)
                    {
                        EquipmentModel.AdventRate = Convert.ToDouble(ds.Tables[2].Rows[0]["AdventRate"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EquipmentModel;
        }
        public int saveEquipmentItem(int EquipmentId, double Quantity, int Itemid, string User_Id, double PurchaseRate, double SaleRate)
        {
            try
            {
                TMEquipmentDal objDal = new TMEquipmentDal();
                DataSet ds = new DataSet();
                return objDal.SaveEquipmentItem(EquipmentId, Quantity, Itemid, User_Id, PurchaseRate, SaleRate);

            }
            catch(Exception ex)
            {
                throw ex;
            }           
        }
        public int DeleteEquipmentItem(int ItemId, int EqipmentId,string User_Id)
        {
            TMEquipmentDal objDal = new TMEquipmentDal();
            return objDal.DeleteEquipmentItem(ItemId, EqipmentId, User_Id);
        }
        public int DeleteEquipmentSegment(int SegmentId, int EqipmentId, string User_Id)
        {
            TMEquipmentDal objDal = new TMEquipmentDal();
            return objDal.DeleteEquipmentSegment(SegmentId, EqipmentId, User_Id);
        }
        public int SaveSegmentDetail(int SegmentId, int EquipmentId,string User_Id)
        {
            try
            {
                TMEquipmentDal objDal = new TMEquipmentDal();
                DataSet ds = new DataSet();
                return objDal.SaveSegmentDetail(SegmentId,EquipmentId, User_Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SegmentList> GetSegmentList(int EquipmentId)
        {
            List<SegmentList> LstSegment = new List<SegmentList>();
            try
            {
                TMEquipmentDal objDal = new TMEquipmentDal();
                DataSet dsData = objDal.GetSegmentList(EquipmentId);
                if (dsData != null)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        SegmentList model = new SegmentList();
                        model.SegmentId = Convert.ToInt32(dr["SegmentId"]);
                        model.SegmentName = dr["SegmentName"].ToString();
                        LstSegment.Add(model);
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstSegment;
        }
    }
}
