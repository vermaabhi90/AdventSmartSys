using SmartSys.BL.BOMAdmin;
using SmartSys.BL.Project;
using SmartSys.BL.ProViews;
using SmartSys.DL.DW;
using SmartSys.DL.Enquiry;
using SmartSys.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmartSys.BL.Enquiry
{
    public class EnquiryBL
    {
        public int SaveCustomerEnquiry(EnquiryModel EnquModel, string user_Id)
        {
            int errCode = 0;
            try
            {
                DataSet ds = new DataSet();
                EnquiryDAL ObjDAL = new EnquiryDAL();
                ds = ObjDAL.GetSeletedCustEnquiryList(0);
                if (ds != null)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow Dr = ds.Tables[0].NewRow();
                    Dr["EnqId"] = EnquModel.EnqId;
                    Dr["EnqNumber"] = EnquModel.EnqNumber;
                    Dr["CustomerId"] = EnquModel.CustomerId;
                    Dr["CustomerContactId"] = EnquModel.CustomerContactId;
                    Dr["Remark"] = EnquModel.Remark;
                    Dr["EnqDate"] = EnquModel.ExpectedDate;
                    Dr["CompCode"] = EnquModel.CompCode;
                    Dr["Priority"] = EnquModel.Priority;
                    Dr["categories"] = EnquModel.categories;
                    Dr["categoriesRefId"] = EnquModel.categoriesRefId;
                    Dr["Rating"] = EnquModel.Rating;
                    Dr["Currancy"] = EnquModel.Currancy;
                    ds.Tables[0].Rows.Add(Dr);
                }
                ds.Tables[1].Rows.Clear();
                //foreach (EnquiryDetailModel EnqDetail in EnquModel.lstEnquiryDetail)
                //{                   
                //    DataRow Dr = ds.Tables[1].NewRow();
                //    Dr["EnqId"] = EnqDetail.EnqId;
                //    Dr["ItemId"] = EnqDetail.ItemId;
                //    Dr["Quantity"] = EnqDetail.Quantity;
                //    Dr["Remark"] = EnqDetail.Remark;
                //    Dr["ExpectedDate"] = EnqDetail.ExpectedDate;                    
                //    ds.Tables[1].Rows.Add(Dr);   
                //}
                errCode = ObjDAL.SaveCustomerEnquiry(ds, user_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveEnqItemsFromExcel(List<EnquiryDetailModel> EnqItemList, string user_Id, int EnqId)
        {
            DataTable dt = new DataTable();
            EnquiryDAL ObjDAL = new EnquiryDAL();
            dt.Columns.Add("EnqId");
            dt.Columns.Add("ItemId");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Remark");
            dt.Columns.Add("ExpectedDate");
            foreach (EnquiryDetailModel model in EnqItemList)
            {
                if (model.Check != "NotOk")
                    dt.Rows.Add(EnqId, model.ItemId, model.Quantity, model.Remark, model.ExpectedDate);
            }
            int Errorcode = ObjDAL.SaveCustItemFromExcel(dt, user_Id);
            return Errorcode;
        }
        public int SaveEnquiryItemBrand(EnquiryDetailModel EnquModel, string user_Id)
        {
            int errCode = 0;
            try
            {
                // string[] BrandList = Brand.Split(',');
                DataSet ds = new DataSet();
                EnquiryDAL ObjDAL = new EnquiryDAL();
                ds = ObjDAL.GetSeletedCustEnquiryList(0);
                ds.Tables[1].Rows.Clear();
                DataRow Dr = ds.Tables[1].NewRow();
                Dr["EnqId"] = EnquModel.EnqId;
                Dr["ItemId"] = EnquModel.ItemId;
                Dr["Quantity"] = EnquModel.Quantity;
                Dr["Remark"] = EnquModel.Remark;
                Dr["ExpectedDate"] = EnquModel.ExpectedDate;
                ds.Tables[1].Rows.Add(Dr);
                ds.Tables[2].Rows.Clear();

                DataRow Dr1 = ds.Tables[2].NewRow();
                Dr1["EnqId"] = EnquModel.EnqId;
                Dr1["ItemId"] = EnquModel.ItemId;
                Dr1["BrandId"] = 0;
                ds.Tables[2].Rows.Add(Dr1);

                errCode = ObjDAL.SaveCustomerItemBrand(ds, user_Id, EnquModel.CPN);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public List<BrandItemModel> GetSeletedCustEnquiryItemBrandList(int EmqId, int ItemId)
        {
            List<BrandItemModel> LstBrand = new List<BrandItemModel>();
            try
            {
                DataSet ds = new DataSet();
                EnquiryDAL ObjDAL = new EnquiryDAL();
                ds = ObjDAL.GetSeletedCustEnquiryItemBrandList(EmqId, ItemId);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    BrandItemModel brandModel = new BrandItemModel();
                    brandModel.BrandId = Convert.ToInt32(dr["BrandId"]);
                    brandModel.BrandName = dr["BrandName"].ToString();
                    LstBrand.Add(brandModel);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstBrand;
        }
        public int DeleteEnqItem(int EmqId, int ItemId)
        {
            int ErrCode = 0;
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                ErrCode = ObjDAL.DeleteEnqItem(EmqId, ItemId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public EnquiryRespModel GetItemResponseList(int EmqId, int ItemId)
        {
            //public EnquiryModel getEnquiryList(string User_Id )
            //{
            EnquiryRespModel LstResponse = new EnquiryRespModel();
            LstResponse.lstEnquiryResponse = new List<EnquiryRespModel>();
            try
            {
                DataSet ds = new DataSet();
                EnquiryDAL ObjDAL = new EnquiryDAL();
                ds = ObjDAL.GetItemResponseList(EmqId, ItemId);
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EnquiryRespModel brandModel = new EnquiryRespModel();
                    brandModel.ResponseId = Convert.ToInt32(dr["ResponseId"]);
                    brandModel.ItemName = dr["ItemName"].ToString();
                    brandModel.VendorName = dr["VendorName"].ToString();
                    brandModel.StatusStr = dr["StatusStr"].ToString();
                    brandModel.CreatedBy = dr["CreatedBy"].ToString();
                    brandModel.CreatedDate = dr["CreatedDate"].ToString();
                    if (dr["VendorResponseDetailId"].ToString() != "")
                    {
                        brandModel.TempStatusStr = "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=glyphicon glyphicon-ok >" + "<b style='color:green' class='glyphicon glyphicon-ok'>" + " " + "</b>" + "</span>" + "</div>";
                    }
                    else
                    {
                        brandModel.TempStatusStr = "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:Red' class='glyphicon glyphicon-remove'>" + " " + "</b>" + "</span>" + "</div>";
                    }
                    LstResponse.lstEnquiryResponse.Add(brandModel);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstResponse;
        }
        public int SendEnqForQuatation(int EnqId, int StatusId, string User_Id)
        {
            int errcode = 0;
            EnquiryDAL ObjDAL = new EnquiryDAL();
            try
            {
                errcode = ObjDAL.SendEnqForQuatation(EnqId, StatusId, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }
        public bool CheckIsOutsource(string UserId)
        {
            EnquiryDAL ObjDAL = new EnquiryDAL();
            EnquiryModel EnquiryModel = new EnquiryModel();
            DataSet ds = new DataSet();
            bool isOutSource=false;
            ds = ObjDAL.GetCustEnquiryList(UserId);
            if (ds.Tables.Count > 2)
            {

                EnquiryModel.isOutSourcePerson = Convert.ToBoolean(ds.Tables[2].Rows[0]["isOutSourcePerson"]);
                isOutSource = EnquiryModel.isOutSourcePerson;
            }
            return isOutSource;
        }
        public EnquiryModel getEnquiryList(string UserId)
        {
            EnquiryModel EnquiryModel = new EnquiryModel();
            EnquiryModel.lstEnquiry = new List<EnquiryModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetCustEnquiryList(UserId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            EnquiryModel objmodel = new EnquiryModel();
                            objmodel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            objmodel.EnqNumber = dr["EnqNumber"].ToString();
                            objmodel.EnqDate = Convert.ToDateTime(dr["EnqDate"].ToString());
                            objmodel.isOutSourcePerson = Convert.ToBoolean(dr["isOutSourcePerson"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.SalesPerson = dr["SalesPerson"].ToString();
                            objmodel.CompCode = dr["CompCode"].ToString();
                            objmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            objmodel.AssignedEmployee = dr["Employee"].ToString();
                            objmodel.StatusStr = dr["StatusStr"].ToString();
                            objmodel.Status = Convert.ToInt32(dr["Status"].ToString());
                            objmodel.Priority = dr["Priority"].ToString();
                            objmodel.CustomerContactId = Convert.ToInt32(dr["CustomerContactId"].ToString());
                            objmodel.ContactName = dr["ContactName"].ToString();
                            objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            EnquiryModel.lstEnquiry.Add(objmodel);

                        }                        
                    }
                    if (ds.Tables.Count > 2)
                    {
                        EnquiryModel.isOutSourcePerson = Convert.ToBoolean(ds.Tables[2].Rows[0]["isOutSourcePerson"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return EnquiryModel;
        }
        public EnquiryModel getEnquiryListforAssignment(string UserId, string Type)
        {
            EnquiryModel EnquiryModel = new EnquiryModel();
            EnquiryModel.lstEnquiry = new List<EnquiryModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                if (Type == "Process")
                {
                    ds = ObjDAL.GetCustEnquiryListforAssign(UserId, 48);
                }
                else
                {
                    ds = ObjDAL.GetCustEnquiryListforAssign(UserId, 49);
                }

                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            EnquiryModel objmodel = new EnquiryModel();
                            objmodel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            objmodel.EnqNumber = dr["EnqNumber"].ToString();
                            objmodel.EnqDate = Convert.ToDateTime(dr["EnqDate"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            objmodel.AssignedEmployee = dr["Employee"].ToString();
                            objmodel.StatusStr = dr["StatusStr"].ToString();
                            objmodel.Status = Convert.ToInt32(dr["Status"].ToString());
                            objmodel.Priority = dr["Priority"].ToString();
                            //  objmodel.ExpectedDate = Convert.ToDateTime(dr["ExpectedDate"].ToString());
                            objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            EnquiryModel.lstEnquiry.Add(objmodel);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return EnquiryModel;
        }
        //  public EnquiryModel getEnquiryProcessingList(string UserId)
        public EnquiryModel GetEnquiryListForQuotation(string User_Id)
        {
            EnquiryModel EnquiryModel = new EnquiryModel();
            EnquiryModel.lstEnquiry = new List<EnquiryModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetEnqListForQuotation(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EnquiryModel objmodel = new EnquiryModel();
                            objmodel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            objmodel.QuotationId = Convert.ToInt32(dr["QuotationId"].ToString());
                            objmodel.QuotaionCost = Convert.ToDouble(dr["QuotCost"].ToString());
                            objmodel.EnqNumber = dr["EnqNumber"].ToString();
                            objmodel.QuotationNumber = dr["QuotationNumber"].ToString();
                            objmodel.EnqDate = Convert.ToDateTime(dr["EnqDate"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            objmodel.StatusStr = dr["StatusStr"].ToString();
                            objmodel.Status = Convert.ToInt32(dr["Status"].ToString());
                            objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            objmodel.CompCode = dr["CompCode"].ToString();
                            EnquiryModel.lstEnquiry.Add(objmodel);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return EnquiryModel;
        }
        public EnquiryModel getEnquiryProcessingList(string user_Id)
        {
            EnquiryModel EnquiryModel = new EnquiryModel();
            EnquiryModel.lstEnquiry = new List<EnquiryModel>();
            try
            {

                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetCustEnquiryList(user_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            EnquiryModel objmodel = new EnquiryModel();
                            objmodel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            objmodel.EnqNumber = dr["EnqNumber"].ToString();
                            objmodel.EnqDate = Convert.ToDateTime(dr["EnqDate"].ToString());
                            objmodel.isOutSourcePerson = Convert.ToBoolean(dr["isOutSourcePerson"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.StatusStr = dr["StatusStr"].ToString();
                            objmodel.Status = Convert.ToInt32(dr["Status"].ToString());
                            //  objmodel.ExpectedDate = Convert.ToDateTime(dr["ExpectedDate"].ToString());
                            objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            objmodel.AssignedEmployee = dr["ProcAssignBy"].ToString();
                            objmodel.AssignedDate = Convert.ToDateTime(dr["ProcAssignDate"].ToString());
                            EnquiryModel.lstEnquiry.Add(objmodel);
                        }
                    }
                    if (ds.Tables.Count > 2)
                    {
                        EnquiryModel.isOutSourcePerson = Convert.ToBoolean(ds.Tables[2].Rows[0]["isOutSourcePerson"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return EnquiryModel;
        }
        public int saveAssignedEnqToEmployee(int EnqId, string EmpId, string User_Id, int Status, string Type)
        {
            int Eemployee = 0;
            EnquiryDAL ObjDAL = new EnquiryDAL();
            string[] strLst = EmpId.Split(',');
            foreach (string Emp in strLst)
            {
                Eemployee = Convert.ToInt32(Emp);
                int errCode = ObjDAL.SaveAssignedEnqToEmp(EnqId, Eemployee, User_Id, Status, Type);
            }
            return Eemployee;
        }
        public EnquiryModel getSelectedEnquiryDetail(int EnqId)
        {
            EnquiryModel objmodel = new EnquiryModel();
            objmodel.lstEnquiryDetail = new List<EnquiryDetailModel>();
            objmodel.RefList = new List<EnquiryDetailModel>();
            int CompleteItem = 0;
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSeletedCustEnquiryList(EnqId);
                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            objmodel.EnqId = Convert.ToInt32(ds.Tables[0].Rows[0]["EnqId"].ToString());
                            if (ds.Tables[0].Rows[0]["EmpId"].ToString() != "")
                                objmodel.EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                            objmodel.EnqNumber = ds.Tables[0].Rows[0]["EnqNumber"].ToString();
                            objmodel.Currancy = ds.Tables[0].Rows[0]["Currancy"].ToString();
                            objmodel.EnqDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["EnqDate"].ToString());
                            objmodel.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();                            
                            objmodel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                            objmodel.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"]);
                            objmodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                            objmodel.StatusStr = ds.Tables[0].Rows[0]["Status"].ToString();
                            objmodel.Status = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"].ToString());
                            objmodel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                            objmodel.Priority = ds.Tables[0].Rows[0]["Priority"].ToString();
                            if (ds.Tables[0].Rows[0]["CustomerContactId"].ToString() != "")
                                objmodel.CustomerContactId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerContactId"].ToString());
                            objmodel.categoriesRefId = Convert.ToInt32(ds.Tables[0].Rows[0]["categoriesRefId"].ToString());
                            objmodel.categories = ds.Tables[0].Rows[0]["categories"].ToString();
                            objmodel.Rating = Convert.ToInt32(ds.Tables[0].Rows[0]["Rating"].ToString());
                            objmodel.FileName = ds.Tables[0].Rows[0]["FileName"].ToString();
                        }
                    }
                    if (ds.Tables.Count > 1)
                    {

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                EnquiryDetailModel EnqDetail = new EnquiryDetailModel();

                                EnqDetail.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                                EnqDetail.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                                EnqDetail.ItemName = dr["ItemName"].ToString();
                                EnqDetail.MPN = dr["MPN"].ToString();
                                EnqDetail.Quantity = Convert.ToInt32(dr["Quantity"].ToString());
                                EnqDetail.Remark = dr["Remark"].ToString();
                                EnqDetail.Status = dr["Status"].ToString();
                                EnqDetail.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                                if (EnqDetail.StatusId == 39 || EnqDetail.StatusId == 45)
                                {
                                    CompleteItem++;
                                }
                                EnqDetail.ModifiedBy = dr["UserName"].ToString();
                                EnqDetail.ModifiedDate = dr["ModifiedDate"].ToString();
                                EnqDetail.Brand = "";
                                if (dr["ExpectedDate"].ToString() != "")
                                    EnqDetail.ExpectedDate = Convert.ToDateTime(dr["ExpectedDate"].ToString());
                                EnqDetail.ExpectedDateStr = EnqDetail.ExpectedDate.ToShortDateString();
                                objmodel.lstEnquiryDetail.Add(EnqDetail);
                                objmodel.RefList.Add(EnqDetail);
                            }
                        }
                    }
                    objmodel.PercentComplete = CompleteItem * 100 / ds.Tables[1].Rows.Count;
                    if(ds.Tables[3].Rows.Count > 0)
                    {
                        if(Convert.ToInt32(ds.Tables[3].Rows[0]["TotCnt"].ToString()) > Convert.ToInt32(ds.Tables[3].Rows[0]["ProcCnt"].ToString()))
                        {
                            objmodel.ResponsPerson = ds.Tables[3].Rows[0]["Persons"].ToString();
                            objmodel.isRem = true;
                        }       
                        else
                        {
                            objmodel.ResponsPerson = ds.Tables[3].Rows[0]["Persons"].ToString();
                            objmodel.isRem = false;
                        }                 
                    }
                   
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }
        public List<ItemListModel> GetItemLst()
        {
            List<ItemListModel> lstItem = new List<ItemListModel>();
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
                            ItemListModel objmodel = new ItemListModel();
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
        public EnquiryModel GetSelectedEnqQuotation(int EnqId, int QuotId)
        {
            EnquiryModel Enqmodel = new EnquiryModel();
            Enqmodel.QuotList = new List<QuotationModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetDelectedQuotation(EnqId, QuotId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            QuotationModel objmodel = new QuotationModel();
                            objmodel.QuotationId = Convert.ToInt32(ds.Tables[0].Rows[0]["QuotationId"].ToString());
                            Enqmodel.QuotationId = Convert.ToInt32(ds.Tables[0].Rows[0]["QuotationId"].ToString());
                            Enqmodel.Rating = Convert.ToInt32(ds.Tables[0].Rows[0]["Rating"].ToString());
                            Enqmodel.RevisedQuotationId = Convert.ToInt32(ds.Tables[0].Rows[0]["RevisedQuotId"].ToString());
                            Enqmodel.RevisedQuotationNumber = ds.Tables[0].Rows[0]["RevisedQuotNumber"].ToString();
                            Enqmodel.EnqId = EnqId;
                            Enqmodel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                            objmodel.EnqId = Convert.ToInt32(ds.Tables[0].Rows[0]["EnqId"].ToString());
                            objmodel.TermId = Convert.ToInt32(ds.Tables[0].Rows[0]["TermId"].ToString());
                            Enqmodel.TermId = Convert.ToInt32(ds.Tables[0].Rows[0]["TermId"].ToString());
                            objmodel.QuotationNumber = ds.Tables[0].Rows[0]["QuotationNumber"].ToString();
                            objmodel.QuotDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["QuotDate"].ToString());
                            Enqmodel.Currency = ds.Tables[0].Rows[0]["Currency"].ToString();
                            objmodel.QuotationDate = ds.Tables[0].Rows[0]["QuotDate"].ToString();
                            objmodel.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                            objmodel.Terms = ds.Tables[0].Rows[0]["Terms"].ToString();
                            objmodel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                            Enqmodel.FileName = ds.Tables[0].Rows[0]["FileName"].ToString();
                            objmodel.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                            objmodel.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"]);
                            Enqmodel.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"]);
                            objmodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            objmodel.modifyDt = ds.Tables[0].Rows[0]["ModifiedDate"].ToString();
                            Enqmodel.QuotList.Add(objmodel);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Enqmodel;
        }
        public EnquiryModel GetCustEmailDeatiltoSendQuotation(int QuotId, string DocType, string User_Id)
        {
            EnquiryModel Enqmodel = new EnquiryModel();
            Enqmodel.lstMail = new List<EnqSendVendorModel>();
            Enqmodel.QuotList = new List<QuotationModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetCustEmailDeatiltoSendQuotation(QuotId, DocType, User_Id);
                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            QuotationModel objmodel = new QuotationModel();
                            objmodel.QuotationNumber = ds.Tables[1].Rows[0]["QuotationNumber"].ToString();
                            objmodel.QuotDate = Convert.ToDateTime(ds.Tables[1].Rows[0]["QuotDate"].ToString());
                            objmodel.MailContent = ds.Tables[1].Rows[0]["MailContent"].ToString();
                            objmodel.CustomerName = ds.Tables[1].Rows[0]["CustomerName"].ToString();
                            objmodel.EnqNumber = ds.Tables[1].Rows[0]["EnqNumber"].ToString();
                            objmodel.Terms = ds.Tables[1].Rows[0]["Terms"].ToString();
                            objmodel.CompCode = ds.Tables[1].Rows[0]["CompCode"].ToString();
                            objmodel.EnqId = Convert.ToInt32(ds.Tables[1].Rows[0]["EnqId"].ToString());
                            objmodel.Currency = ds.Tables[1].Rows[0]["Currancy"].ToString();
                            objmodel.EMailId = ds.Tables[1].Rows[0]["emailId"].ToString();
                            objmodel.ModifiedBy = ds.Tables[1].Rows[0]["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(ds.Tables[1].Rows[0]["ModifiedDate"].ToString());
                            Enqmodel.QuotList.Add(objmodel);
                        }
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            EnqSendVendorModel EnqDetail = new EnqSendVendorModel();
                            EnqDetail.EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                            EnqDetail.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                            EnqDetail.EmailServer = ds.Tables[0].Rows[0]["EmailServer"].ToString();
                            EnqDetail.EmailPassword = ds.Tables[0].Rows[0]["EmailPassword"].ToString();
                            EnqDetail.EmailUserName = ds.Tables[0].Rows[0]["EmailUserName"].ToString();
                            EnqDetail.EmailPort = ds.Tables[0].Rows[0]["EmailPort"].ToString();
                            Enqmodel.lstMail.Add(EnqDetail);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Enqmodel;
        }
        public EnquiryModel CloseEnquiryMail(int EnqId, string DocType, string User_Id)
        {
            EnquiryModel Enqmodel = new EnquiryModel();
            Enqmodel.lstMail = new List<EnqSendVendorModel>();
            Enqmodel.QuotList = new List<QuotationModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.CloseEnquiryMail(EnqId, DocType, User_Id);
                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            QuotationModel objmodel = new QuotationModel();                          
                            objmodel.MailContent = ds.Tables[1].Rows[0]["MailContent"].ToString();                          
                            objmodel.EMailId = ds.Tables[1].Rows[0]["emailId"].ToString();                          
                            Enqmodel.QuotList.Add(objmodel);
                        }
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            EnqSendVendorModel EnqDetail = new EnqSendVendorModel();
                            EnqDetail.EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                            EnqDetail.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                            EnqDetail.EmailServer = ds.Tables[0].Rows[0]["EmailServer"].ToString();
                            EnqDetail.EmailPassword = ds.Tables[0].Rows[0]["EmailPassword"].ToString();
                            EnqDetail.EmailUserName = ds.Tables[0].Rows[0]["EmailUserName"].ToString();
                            EnqDetail.EmailPort = ds.Tables[0].Rows[0]["EmailPort"].ToString();
                            Enqmodel.lstMail.Add(EnqDetail);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Enqmodel;
        }
        public DataSet CloseEnquiryMailItems(int EnqId, string DocType,string User_Id)
        {
            EnquiryDAL ObjDAL = new EnquiryDAL();
            DataSet dsItem = new DataSet();
            dsItem = ObjDAL.CloseEnquiryMail(EnqId, DocType, User_Id);
            return dsItem;
        }
        public List<EnquiryDetailModel> GetEnqCompList(EnquiryModel Model,string User_Id)
        {
            List<EnquiryDetailModel> lstModel = new List<EnquiryDetailModel>();
            try
            {
                DataSet dsItem = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsItem = DALObj.GetItemList(User_Id);
                bool chk = false;
                foreach (EnquiryDetailModel demo in Model.lstEnquiryDetail)
                {
                    DataRow dr = dsItem.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["MPN"]).ToUpper() == demo.MPN.ToString().ToUpper());
                    if (dr != null)
                    {
                        demo.ItemId = Convert.ToInt32(dr["itemId"]);
                        demo.Brand = dr["BrandName"].ToString();
                        if (demo.Brand == "N/A")
                        {
                            demo.ErrorDetail = demo.ErrorDetail + "\n" + "Item do not hvae Brand.";
                            demo.Check = "NotOk";
                        }
                        demo.ExpectedDate = Convert.ToDateTime(demo.ExpectedDateStr);
                    }
                    else
                    {
                        demo.ErrorDetail = demo.ErrorDetail + "\n" + "MPN Not avalable in System.";
                        demo.Check = "NotOk";
                    }
                    lstModel.Add(demo);
                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return lstModel;
        }
        public List<EnquiryItemVendorResponseDetailModel> GetVendResponseCompList(EnquiryItemVendorResponseDetailModel Model,string User_Id)
        {
            List<EnquiryItemVendorResponseDetailModel> lstModel = new List<EnquiryItemVendorResponseDetailModel>();
            try
            {
                DataSet dsItem = new DataSet();
                ItemDAL DALObj = new ItemDAL();
                dsItem = DALObj.GetItemList(User_Id);
                bool Allow = true;
                bool chk = false;
                foreach (EnquiryItemVendorResponseDetailModel demo in Model.listEnqItemVendRespDetail)
                {
                    DataRow dr = dsItem.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["MPN"]).ToUpper().Trim() == demo.MPN.ToString().ToUpper().Trim());
                    if (dr != null)
                    {
                        demo.ItemId = Convert.ToInt32(dr["itemId"]);
                    }
                    else
                    {
                        Allow = false;
                    }
                    DataRow dr1 = dsItem.Tables[1].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["BrandName"]).ToUpper().Trim() == demo.BrandName.ToString().ToUpper().Trim());
                    if (dr1 != null)
                    {
                        demo.BrandId = Convert.ToInt32(dr1["BrandId"]);
                    }
                    else
                    {
                        Allow = false;
                    }
                    if (Allow)
                    {
                        demo.Check = "OK";
                        lstModel.Add(demo);
                    }
                    else
                    {
                        demo.Check = "NotOK";
                        lstModel.Add(demo);
                    }
                    Allow = true;
                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return lstModel;
        }
        public int SaveVendorResponseByItem(EnquiryItemVendorResponseDetailModel mod, int ResponseId, int VendorId)
        {
            int errcode = 0;

            EnquiryDAL objdal = new EnquiryDAL();
            DataSet ds = new DataSet();
            ds = objdal.GetEnquiryItemVendorResponseDetailList(0);
            if (ds != null)
            {
                if (ds.Tables.Count > 1)
                {
                    ds.Tables[1].Rows.Clear();
                    foreach (EnquiryItemVendorResponseDetailModel tempModel in mod.listEnqItemVendRespDetail)
                    {
                        DataRow Dr1 = ds.Tables[1].NewRow();
                        Dr1["ResponseId"] = ResponseId;
                        Dr1["ItemId"] = tempModel.ItemId;
                        Dr1["BrandId"] = tempModel.BrandId;
                        Dr1["Rate"] = tempModel.Rate;
                        Dr1["MinimumQty"] = tempModel.MinimumQty;
                        Dr1["BatchNumber"] = tempModel.BatchNumber;
                        Dr1["VendorPromiseDate"] = tempModel.VendorPromiseDate;
                        Dr1["SPQ"] = tempModel.SPQ;
                        Dr1["MOQ"] = tempModel.MOQ;
                        Dr1["Chk"] = tempModel.Check;
                        ds.Tables[1].Rows.Add(Dr1);
                    }
                }
            }

            errcode = objdal.SaveEnquiryItemVendorResponseDetails(ds, "", ResponseId, VendorId);
            return errcode;
        }
        public int EnqSendForProcess(int EnqId, int StatusId, string user_Id)
        {
            int errcode = 0;
            EnquiryDAL ObjDAL = new EnquiryDAL();
            try
            {
                errcode = ObjDAL.EnqSendForProcess(EnqId, StatusId, user_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }

        public List<QuotationModel> GetQuotationListByItem(int ItemId)
        {
            List<QuotationModel> QuotationLst = new List<QuotationModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetQuotationListByItem(ItemId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            QuotationModel Model = new QuotationModel();
                            Model.QuotationId = Convert.ToInt32(dr["QuotationId"]);
                            Model.QuotationNumber = dr["QuotationNumber"].ToString();
                            QuotationLst.Add(Model);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return QuotationLst;
        }

        public ReportNameModel GetOpenReport(string user_Id)
        {
            ReportNameModel Model = new ReportNameModel();
            try
            {
                EnquiryDAL objDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetEnquiryReport(user_Id);
                Model.OutputLocation = ds.Tables[0].Rows[0]["OutputLocation"].ToString();
                Model.RunDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["RunDate"].ToString());
                Model.ReportId = ds.Tables[0].Rows[0]["ReportId"].ToString();
                Model.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"].ToString());
                Model.TxtParamValue = ds.Tables[0].Rows[0]["TxtParamValue"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Model;
        }
        public ReportNameModel GetOpenEnquiryQuotItemReport(string user_Id)
        {
            ReportNameModel Model = new ReportNameModel();
            try
            {
                EnquiryDAL objDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetEnquiryQuotItemReport(user_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Model.OutputLocation = ds.Tables[0].Rows[0]["OutputLocation"].ToString();
                        Model.RunDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["RunDate"].ToString());
                        Model.ReportId = ds.Tables[0].Rows[0]["ReportId"].ToString();
                        Model.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"].ToString());
                        Model.TxtParamValue = ds.Tables[0].Rows[0]["TxtParamValue"].ToString();
                    }
                }
                }
            catch (Exception ex)
            {
                throw ex;
            }
            return Model;
        }
        public int UpdateQuotFileName(int QuotId, string FilePath)
        {
            EnquiryDAL objdal = new EnquiryDAL();
            return objdal.UpdateQuotFileName(QuotId, FilePath);
        }

        #region Customer Process Enquiry
        public EnquiryModel GetSeletedEnquiryProcessList(int EnqId, string User_Id)
        {
            EnquiryModel objmodel = new EnquiryModel();
            objmodel.lstEnquiryDetail = new List<EnquiryDetailModel>();
            objmodel.lstAttchment = new List<EnquiryAttachment>();
            int processItem = 0;
            int totalItem = 0;
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSeletedEnquiryProcessList(EnqId, User_Id);
                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            objmodel.EnqId = Convert.ToInt32(ds.Tables[0].Rows[0]["EnqId"].ToString());
                            objmodel.EnqNumber = ds.Tables[0].Rows[0]["EnqNumber"].ToString();
                            objmodel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                            objmodel.EnqDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["EnqDate"].ToString());
                            objmodel.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                            objmodel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                            objmodel.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"]);
                            objmodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());

                        }
                    }
                    if (ds.Tables.Count > 1)
                    {

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                EnquiryDetailModel EnqDetail = new EnquiryDetailModel();

                                EnqDetail.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                                EnqDetail.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                                EnqDetail.Quantity = Convert.ToInt32(dr["Quantity"].ToString());
                                EnqDetail.ItemName = dr["ItemName"].ToString();
                                EnqDetail.MPN = dr["MPN"].ToString();
                                EnqDetail.Brand = dr["BrandName"].ToString();
                                EnqDetail.Status = dr["Status"].ToString();
                                EnqDetail.StatusId = Convert.ToInt32(dr["StatusId"].ToString());
                                totalItem = Convert.ToInt32(dr["TotalEnqItem"].ToString());
                                processItem = Convert.ToInt32(dr["processItem"].ToString());
                                if (Convert.ToInt32(dr["processItem"]) > 0)
                                {
                                    processItem++;
                                }
                                objmodel.lstEnquiryDetail.Add(EnqDetail);
                            }
                        }
                    }

                    if (ds.Tables.Count > 2)
                    {

                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[2].Rows)
                            {
                                EnquiryAttachment AttchDetail = new EnquiryAttachment();

                                AttchDetail.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                                AttchDetail.FileName = dr["FileName"].ToString();
                                AttchDetail.CreatedBy = dr["CreatedBy"].ToString();
                                AttchDetail.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                objmodel.lstAttchment.Add(AttchDetail);
                            }
                        }
                    }
                    objmodel.PercentComplete = (processItem * 100) / totalItem;
                    objmodel.RemainprocessItem = Convert.ToInt32(ds.Tables[0].Rows[0]["RemainprocessItem"].ToString());
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }
        public List<EnquiryBrandVemdorModel> GetEnquiryProcessBrandVendorList(int EnqId, int ItemId)
        {
            List<EnquiryBrandVemdorModel> objmodel = new List<EnquiryBrandVemdorModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetEnquiryProcessBrandVendorList(EnqId, ItemId);

                if (ds.Tables.Count > 0)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EnquiryBrandVemdorModel EnqDetail = new EnquiryBrandVemdorModel();

                            EnqDetail.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            EnqDetail.VendorName = dr["VendorName"].ToString();
                            objmodel.Add(EnqDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }
        public List<EnquiryBrandVemdorModel> GetEnquiryProcessAllocateItemVendorList(int EnqId, int ItemId)
        {
            List<EnquiryBrandVemdorModel> objmodel = new List<EnquiryBrandVemdorModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetEnquiryProcessBrandVendorList(EnqId, ItemId);

                if (ds.Tables.Count > 1)
                {

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            EnquiryBrandVemdorModel EnqDetail = new EnquiryBrandVemdorModel();

                            EnqDetail.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            EnqDetail.VendorName = dr["VendorName"].ToString();
                            objmodel.Add(EnqDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }
        //public int SendVenderEnquiryDetail(string Vendor, int EnqId, string ItemId, string User_Id)
        //{
        //    int ErrCode = 0;
        //    try
        //    {
        //        EnquiryDAL ObjDAL = new EnquiryDAL();
        //        DataSet ds = new DataSet();
        //        DataTable DtEnqItem = new DataTable("");
        //        DtEnqItem.Columns.Add("ItemId", typeof(System.Int32));

        //        DataTable DtDWEnq = new DataTable("");
        //        DtDWEnq.Columns.Add("EnqId", typeof(System.Int32));
        //        DtDWEnq.Columns.Add("ItemId", typeof(System.Int32));
        //        DtDWEnq.Columns.Add("VendorId", typeof(System.Int32));
        //        foreach (string str1 in ItemId.Split(','))
        //        {
        //            DataRow dr1 = DtEnqItem.NewRow();
        //            dr1["ItemId"] = Convert.ToInt32(str1);
        //            foreach (string str in Vendor.Split(','))
        //            {
        //                DataRow dr = DtDWEnq.NewRow();
        //                dr["EnqId"] = EnqId;
        //                dr["ItemId"] = Convert.ToInt32(str1);
        //                dr["VendorId"] = Convert.ToInt32(str);
        //                DtDWEnq.Rows.Add(dr);
        //            }
        //            DtEnqItem.Rows.Add(dr1);
        //        }
        //        ds.Tables.Add(DtDWEnq);
        //        ds.Tables.Add(DtEnqItem);
        //        ErrCode = ObjDAL.SendVenderEnquiryDetail(ds, EnqId, ItemId, User_Id);
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.LogException(ex);
        //    }
        //    return ErrCode;
        //}
        public int AddItemVendorContact(int Vendor, int EnqId, string ItemId, string User_Id, int ContactId)
        {
            int ErrCode = 0;
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                DataTable DtEnqItem = new DataTable("");
                DtEnqItem.Columns.Add("ItemId", typeof(System.Int32));

                DataTable DtDWEnq = new DataTable("");
                DtDWEnq.Columns.Add("EnqId", typeof(System.Int32));
                DtDWEnq.Columns.Add("ItemId", typeof(System.Int32));
                DtDWEnq.Columns.Add("VendorId", typeof(System.Int32));
                DtDWEnq.Columns.Add("ContactId", typeof(System.Int32));
                foreach (string str1 in ItemId.Split(','))
                {
                    DataRow dr1 = DtEnqItem.NewRow();
                    dr1["ItemId"] = Convert.ToInt32(str1);
                    DataRow dr = DtDWEnq.NewRow();
                    dr["EnqId"] = EnqId;
                    dr["ItemId"] = Convert.ToInt32(str1);
                    dr["VendorId"] = Convert.ToInt32(Vendor);
                    dr["ContactId"] = Convert.ToInt32(ContactId);
                    DtDWEnq.Rows.Add(dr);
                    DtEnqItem.Rows.Add(dr1);
                }
                ds.Tables.Add(DtDWEnq);
                ds.Tables.Add(DtEnqItem);
                ErrCode = ObjDAL.SendVenderEnquiryDetail(ds, EnqId, ItemId, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public List<EnqItemVendorContact> GetItemVendorDetailList(int EnqId, string User_Id)
        {
            List<EnqItemVendorContact> ListModel = new List<EnqItemVendorContact>();
            try
            {
                DataSet ds = new DataSet();
                EnquiryDAL objdal = new EnquiryDAL();
                ds = objdal.GetItemVendorDetailList(EnqId, User_Id);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                EnqItemVendorContact Tempmode = new EnqItemVendorContact();
                                Tempmode.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                                Tempmode.ContactId = Convert.ToInt32(dr["VendorContactId"].ToString());
                                Tempmode.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                                Tempmode.VendorName = dr["VendorName"].ToString();
                                Tempmode.ContactName = dr["ContactName"].ToString();
                                Tempmode.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                                Tempmode.MPN = dr["MPN"].ToString();
                                Tempmode.ItemName = dr["ItemName"].ToString();
                                ListModel.Add(Tempmode);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return ListModel;
        }
        public EnqSendVendorModel GetVendorEmailDeatiltoSendAllEnq(int EnqId, int ItemId, string DocType, string User_Id)
        {
            EnqSendVendorModel EnqDetail = new EnqSendVendorModel();
            EnqDetail.lstEnqSendVendorModel = new List<EnqSendVendorModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetVendorEmailDeatiltoSendEnq(EnqId, ItemId, DocType, User_Id);
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        EnqDetail.EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                        EnqDetail.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                        EnqDetail.EmailServer = ds.Tables[0].Rows[0]["EmailServer"].ToString();
                        EnqDetail.EmailPassword = ds.Tables[0].Rows[0]["EmailPassword"].ToString();
                        EnqDetail.EmailUserName = ds.Tables[0].Rows[0]["EmailUserName"].ToString();
                        EnqDetail.EmailPort = ds.Tables[0].Rows[0]["EmailPort"].ToString();
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            EnqSendVendorModel model = new EnqSendVendorModel();
                            model.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            model.VendorName = dr["VendorName"].ToString();
                            model.MailContent = dr["MailContent"].ToString();
                            model.ItemName = dr["Item"].ToString();
                            model.emailId = dr["Email"].ToString();
                            model.EnqNumber = dr["EnqNumber"].ToString();
                            EnqDetail.lstEnqSendVendorModel.Add(model);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return EnqDetail;
        }
        public EnqSendVendorModel GetVendorEmailDeatiltoSendEnq(int EnqId, int ItemId, string DocType, string User_Id)
        {
            EnqSendVendorModel EnqDetail = new EnqSendVendorModel();
            EnqDetail.lstEnqSendVendorModel = new List<EnqSendVendorModel>();
            EnqDetail.lstSingEnqSendVendorModel = new List<EnqSendVendorModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetVendorEmailDeatiltoSendEnq(EnqId, ItemId, DocType, User_Id);
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        EnqDetail.EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                        EnqDetail.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                        EnqDetail.EmailServer = ds.Tables[0].Rows[0]["EmailServer"].ToString();
                        EnqDetail.EmailPassword = ds.Tables[0].Rows[0]["EmailPassword"].ToString();
                        EnqDetail.EmailUserName = ds.Tables[0].Rows[0]["EmailUserName"].ToString();
                        EnqDetail.EmailPort = ds.Tables[0].Rows[0]["EmailPort"].ToString();
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            EnqSendVendorModel model = new EnqSendVendorModel();
                            model.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            model.ItemName = dr["ItemName"].ToString();
                            model.Quantity = Convert.ToInt32(dr["Quantity"].ToString());
                            model.Remark = dr["Remark"].ToString();
                            model.EnqNumber = dr["EnqNumber"].ToString();
                            model.ExpectedDate = Convert.ToDateTime(dr["ExpectedDate"].ToString());
                            model.MailContent = dr["MailContent"].ToString();
                            model.emailId = dr["Email"].ToString();
                            EnqDetail.lstEnqSendVendorModel.Add(model);

                        }
                    }
                    if (ds.Tables.Count > 2)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            EnqSendVendorModel model = new EnqSendVendorModel();
                            model.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            model.VendorName = dr["VendorName"].ToString();
                            model.MailContent = dr["MailContent"].ToString();
                            model.ItemName = dr["Item"].ToString();
                            model.emailId = dr["Email"].ToString();
                            model.EnqNumber = dr["EnqNumber"].ToString();
                            EnqDetail.lstSingEnqSendVendorModel.Add(model);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return EnqDetail;
        }
        public int UpdateVendorEnqstatus(int EnqId, int ItemId, int VendorId, string User_Id)
        {
            EnquiryDAL ObjDAL = new EnquiryDAL();
            int ErrCode = 0;
            try
            {
                ErrCode = ObjDAL.UpdateVendorEnqstatus(EnqId, ItemId, VendorId, User_Id);
            }
            catch (Exception)
            {
                throw;
            }
            return ErrCode;
        }
        public DataSet GetVendorItemEnqforCSV(int EnqId, int VendorId, string User_Id)
        {
            EnquiryDAL ObjDAL = new EnquiryDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = ObjDAL.GetVendorItemEnqforCSV(EnqId, VendorId, User_Id);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }
        #endregion  Customer Process Enquiry

        #region  Enquiry Item Vendor Response Detail
        public EnquiryItemVendorResponseDetailModel EnquiryItemVendorResponseDetailList(string User_Id)
        {

            EnquiryItemVendorResponseDetailModel VendRespmodel = new EnquiryItemVendorResponseDetailModel();
            VendRespmodel.listEnqItemVendRespDetail = new List<EnquiryItemVendorResponseDetailModel>();
            try
            {
                DataSet ds = new DataSet();
                EnquiryDAL objdal = new EnquiryDAL();
                ds = objdal.EnquiryItemVendorResponseDetailList(User_Id);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                EnquiryItemVendorResponseDetailModel Tempmode = new EnquiryItemVendorResponseDetailModel();
                                Tempmode.ResponseId = Convert.ToInt32(dr["ResponseId"].ToString());
                                Tempmode.VendorName = dr["VendorName"].ToString();
                                Tempmode.ContactName = dr["ContactName"].ToString();
                                Tempmode.Currency = dr["Currency"].ToString();
                                Tempmode.ReciptMethod = dr["ReciptMethod"].ToString();
                                Tempmode.Remark = dr["Remark"].ToString();
                                Tempmode.ReciptDate = Convert.ToDateTime(dr["ReciptDate"].ToString());
                                Tempmode.CreatedBy = dr["CreatedBy"].ToString();
                                Tempmode.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                Tempmode.ModifiedBy = dr["ModifiedBy"].ToString();
                                Tempmode.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                VendRespmodel.listEnqItemVendRespDetail.Add(Tempmode);
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return VendRespmodel;
        }

        public EnquiryItemVendorResponseDetailModel GetEnquiryItemVendorResponseDetailList(int ResponseId)
        {
            int Seqno = 1;
            EnquiryItemVendorResponseDetailModel model = new EnquiryItemVendorResponseDetailModel();
            model.listEnqItemVendRespDetail = new List<EnquiryItemVendorResponseDetailModel>();
            EnquiryDAL objdal = new EnquiryDAL();
            DataSet ds = new DataSet();
            ds = objdal.GetEnquiryItemVendorResponseDetailList(ResponseId);
            if (ds != null)
            {
                if (ds.Tables.Count > 1)
                {

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        model.ResponseId = Convert.ToInt32(ds.Tables[0].Rows[0]["ResponseId"].ToString());
                        model.VendorId = Convert.ToInt32(ds.Tables[0].Rows[0]["VendorId"].ToString());
                        model.ContactId = Convert.ToInt32(ds.Tables[0].Rows[0]["ContactId"].ToString());
                        model.ReciptMethod = ds.Tables[0].Rows[0]["ReciptMethod"].ToString();
                        model.VendStatus = ds.Tables[0].Rows[0]["VendStatus"].ToString();
                        model.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                        model.ReciptDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ReciptDate"].ToString());
                        model.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        model.Currency = ds.Tables[0].Rows[0]["Currency"].ToString();
                        model.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        model.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        model.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        model.rating = Convert.ToInt32(ds.Tables[0].Rows[0]["Rating"].ToString());
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            EnquiryItemVendorResponseDetailModel demomodel = new EnquiryItemVendorResponseDetailModel();
                            demomodel.SeqNo = Seqno++;
                            demomodel.ResponseId = Convert.ToInt32(dr["ResponseId"].ToString());
                            demomodel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            demomodel.ItemName = dr["ItemName"].ToString();
                            demomodel.MPN = dr["MPN"].ToString();
                            demomodel.BrandId = Convert.ToInt32(dr["BrandId"].ToString());
                            demomodel.BrandName = dr["BrandName"].ToString();
                            demomodel.Rate = Convert.ToDouble(dr["Rate"].ToString());
                            demomodel.MinimumQty = Convert.ToDouble(dr["MinimumQty"].ToString());
                            demomodel.BatchNumber = dr["BatchNumber"].ToString();
                            demomodel.VendorPromiseDate = dr["VendorPromiseDate"].ToString();
                            demomodel.StrVendorPromiseDate = demomodel.VendorPromiseDate;
                            if (dr["SPQ"].ToString() != "")
                                demomodel.SPQ = Convert.ToInt32(dr["SPQ"].ToString());
                            if (dr["MOQ"].ToString() != "")
                                demomodel.MOQ = Convert.ToInt32(dr["MOQ"].ToString());
                            model.listEnqItemVendRespDetail.Add(demomodel);
                        }
                    }
                }
            }
            return model;
        }

        public DataSet GetCustItemEnqListForCSV(int QuotId)
        {
            EnquiryDAL ObjDAL = new EnquiryDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = ObjDAL.GetCustItemEnqListForCSV(QuotId);
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }
        public List<PreViewQuotModel> GetPreviewListData(int QuotId)
        {
            EnquiryDAL ObjDAL = new EnquiryDAL();
            DataSet ds = new DataSet();
            ds = ObjDAL.GetCustItemEnqListForCSV(QuotId);
            List<PreViewQuotModel> lstPreview = new List<PreViewQuotModel>();
            try
            {
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PreViewQuotModel model = new PreViewQuotModel();
                            model.ItemId = Convert.ToInt32(dr["ItemId"]);
                            model.ItemName = Convert.ToString(dr["ItemName"]);
                            model.BrandName = Convert.ToString(dr["BrandName"]);
                            model.Quantity = Convert.ToInt32(dr["Quantity"]);
                            model.Rate = Convert.ToDouble(dr["Rate"]);
                            model.Currency = Convert.ToString(dr["Currancy"]);
                            model.MPN = Convert.ToString(dr["MPN"]);
                            model.VendorPromiseDate = Convert.ToString(dr["LeadTime"]);
                            model.SPQ = Convert.ToInt32(dr["SPQ"]);
                            model.MOQ = Convert.ToInt32(dr["MOQ"]);
                            model.Remark = Convert.ToString(dr["Remark"]);
                            lstPreview.Add(model);
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
            return lstPreview;
        }
        public List<EnquiryItemVendorResponseDetailModel> GetCustQuotItem(int EnqId, int QuotId)
        {
            List<EnquiryItemVendorResponseDetailModel> CustItemList = new List<EnquiryItemVendorResponseDetailModel>();
            EnquiryDAL objdal = new EnquiryDAL();
            DataSet ds = new DataSet();
            ds = objdal.GetVendorResponseByItem(0, EnqId, QuotId);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            EnquiryItemVendorResponseDetailModel demomodel = new EnquiryItemVendorResponseDetailModel();

                            demomodel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            demomodel.ItemName = dr["ItemName"].ToString();
                            demomodel.MPN = dr["MPN"].ToString();
                            if (dr["CRate"].ToString() != "")
                                demomodel.Rate = Convert.ToDouble(dr["CRate"]);
                            demomodel.Remark = dr["Remark"].ToString();
                            demomodel.CRate = Convert.ToDouble(dr["VendRate"]);
                            demomodel.VendorPromiseDate = dr["LeadTime"].ToString();
                            demomodel.MOQ = Convert.ToInt32(dr["MOQ"].ToString());
                            demomodel.SPQ = Convert.ToInt32(dr["SPQ"].ToString());
                            demomodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            demomodel.modifyDt = dr["ModifiedDate"].ToString();
                            demomodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());

                            CustItemList.Add(demomodel);
                        }
                    }

                }
            }
            return CustItemList;
        }

        public List<EnquiryItemVendorResponseDetailModel> GetCustQuotItemforApproval(int EnqId, int QuotId)
        {
            List<EnquiryItemVendorResponseDetailModel> CustItemList = new List<EnquiryItemVendorResponseDetailModel>();
            EnquiryDAL objdal = new EnquiryDAL();
            DataSet ds = new DataSet();
            ds = objdal.GetCustQuotItemforApproval(EnqId, QuotId);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EnquiryItemVendorResponseDetailModel demomodel = new EnquiryItemVendorResponseDetailModel();
                            demomodel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            demomodel.ItemName = dr["ItemName"].ToString();
                            demomodel.MPN = dr["MPN"].ToString();
                            demomodel.CPN = dr["CPN"].ToString();
                            demomodel.BrandName = dr["BrandName"].ToString();
                            demomodel.Currency = dr["Currancy"].ToString();
                            if (dr["CustRate"].ToString() != "")
                                demomodel.CRate = Convert.ToDouble(dr["CustRate"]);
                            demomodel.Remark = dr["Remark"].ToString();
                            demomodel.Quantity = Convert.ToInt32(dr["Quantity"].ToString());
                            if (dr["VendorRate"].ToString() != "")
                                demomodel.Rate = Convert.ToDouble(dr["VendorRate"]);
                            demomodel.SPQ = Convert.ToInt32(dr["SPQ"].ToString());
                            demomodel.MOQ = Convert.ToInt32(dr["MOQ"].ToString());
                            demomodel.VendorPromiseDate = dr["VendorPromiseDate"].ToString();
                            demomodel.Margin = Convert.ToDouble(dr["Margin"]);
                            demomodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            demomodel.modifyDt = dr["ModifiedDate"].ToString();
                            demomodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            CustItemList.Add(demomodel);
                        }
                    }

                }
            }
            return CustItemList;
        }
        public List<EnquiryItemVendorResponseDetailModel> GetVendorResponseByItem(int ItemId, int EnqId, int QuotId)
        {
            List<EnquiryItemVendorResponseDetailModel> ResponseList = new List<EnquiryItemVendorResponseDetailModel>();
            EnquiryDAL objdal = new EnquiryDAL();
            DataSet ds = new DataSet();
            ds = objdal.GetVendorResponseByItem(ItemId, EnqId, QuotId);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EnquiryItemVendorResponseDetailModel demomodel = new EnquiryItemVendorResponseDetailModel();
                            demomodel.ResponseId = Convert.ToInt32(dr["ResponseDetailId"].ToString());
                            demomodel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            demomodel.ItemName = dr["ItemName"].ToString();
                            demomodel.VendorName = dr["VendorName"].ToString();
                            demomodel.BrandId = Convert.ToInt32(dr["BrandId"].ToString());
                            demomodel.BrandName = dr["BrandName"].ToString();
                            demomodel.Currency = dr["Currency"].ToString();
                            demomodel.QuotCurrency = dr["QuotCurrency"].ToString();
                            demomodel.Rate = Convert.ToDouble(dr["Rate"].ToString());
                            demomodel.MinimumQty = Convert.ToDouble(dr["MinimumQty"].ToString());
                            if (dr["CRate"].ToString() != "")
                                demomodel.CRate = Convert.ToDouble(dr["CRate"].ToString());
                            demomodel.Amount = Convert.ToDouble(demomodel.CRate * demomodel.MinimumQty);
                            demomodel.BatchNumber = dr["BatchNumber"].ToString();
                            demomodel.VendorPromiseDate = dr["VendorPromiseDate"].ToString();
                            demomodel.StrVendorPromiseDate = demomodel.VendorPromiseDate.ToString();
                            ResponseList.Add(demomodel);
                        }
                    }

                }
            }
            return ResponseList;
        }

        public List<EnquiryItemVendorResponseDetailModel> GetAllVendorResponseByItem(int ItemId, int EnqId, int QuotId)
        {
            List<EnquiryItemVendorResponseDetailModel> ResponseList = new List<EnquiryItemVendorResponseDetailModel>();
            EnquiryDAL objdal = new EnquiryDAL();
            DataSet ds = new DataSet();
            ds = objdal.GetVendorResponseByItem(ItemId, EnqId, QuotId);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            EnquiryItemVendorResponseDetailModel demomodel = new EnquiryItemVendorResponseDetailModel();
                            demomodel.ResponseId = Convert.ToInt32(dr["ResponseDetailId"].ToString());
                            demomodel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            demomodel.ItemName = dr["ItemName"].ToString();
                            demomodel.VendorName = dr["VendorName"].ToString();
                            demomodel.BrandId = Convert.ToInt32(dr["BrandId"].ToString());
                            demomodel.BrandName = dr["BrandName"].ToString();
                            demomodel.Rate = Convert.ToDouble(dr["Rate"].ToString());
                            demomodel.MinimumQty = Convert.ToDouble(dr["MinimumQty"].ToString());
                            // if (dr["CRate"].ToString() != "")
                            //  demomodel.CRate = Convert.ToDouble(dr["CRate"].ToString());
                            demomodel.Amount = Convert.ToDouble(demomodel.Rate * demomodel.MinimumQty);
                            demomodel.BatchNumber = dr["BatchNumber"].ToString();
                            demomodel.VendorPromiseDate = dr["VendorPromiseDate"].ToString();
                            demomodel.StrVendorPromiseDate = demomodel.VendorPromiseDate.ToString();
                            ResponseList.Add(demomodel);
                        }
                    }

                }
            }
            return ResponseList;
        }
        public List<QuotationModel> GetQuotaionListForApproval(string User_Id)
        {
            List<QuotationModel> QuotList = new List<QuotationModel>();
            EnquiryDAL objdal = new EnquiryDAL();
            DataSet ds = new DataSet();
            ds = objdal.GetQuotationListForApproval(User_Id);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            QuotationModel demomodel = new QuotationModel();
                            demomodel.QuotationId = Convert.ToInt32(dr["QuotationId"].ToString());
                            demomodel.QuotationNumber = dr["QuotationNumber"].ToString();
                            demomodel.QuotDate = Convert.ToDateTime(dr["QuotDate"].ToString());
                            demomodel.Terms = dr["Terms"].ToString();
                            demomodel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            demomodel.CustomerName = dr["CustomerName"].ToString();
                            demomodel.EnqNumber = dr["EnqNumber"].ToString();
                            demomodel.CompCode = dr["CompCode"].ToString();
                            demomodel.Status = dr["Status"].ToString();
                            demomodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            demomodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            QuotList.Add(demomodel);
                        }
                    }

                }
            }
            return QuotList;
        }
        public int SaveCustQuotItemDetails(EnquiryItemVendorResponseDetailModel model, int QuotId)
        {

            EnquiryDAL objdal = new EnquiryDAL();
            DataTable dt = new DataTable();
            dt.Columns.Add("QuotId");
            dt.Columns.Add("ItemId");
            dt.Columns.Add("BrandId");
            dt.Columns.Add("MinimumQty");
            dt.Columns.Add("Rate");
            dt.Columns.Add("Remark");
            dt.Columns.Add("ResponseId");
            dt.Columns.Add("BatchNumber");
            DataRow dr = dt.NewRow();
            dr["QuotId"] = QuotId;
            dr["ItemId"] = model.ItemId;
            dr["BrandId"] = model.BrandId;
            dr["MinimumQty"] = model.MinimumQty;
            dr["Rate"] = model.Rate;
            dr["Remark"] = model.Remark;
            dr["ResponseId"] = model.ResponseId;
            dr["BatchNumber"] = model.BatchNumber;
            dt.Rows.Add(dr);
            return objdal.SaveCustQuotItemDetails(dt);


        }
        public int SaveQuotation(int ItemId, int ResponseId, int BrandId, float Rate, float MinimumQty, int EnqId, string User_Id, string Remark,int QuotId)
        {
            DataSet ds = new DataSet();
            EnquiryDAL objdal = new EnquiryDAL();
            return objdal.SaveQuotation(ItemId, ResponseId, BrandId, Rate, MinimumQty, EnqId, User_Id, Remark, QuotId);
        }
        public int CreateQuotation(int ItemId, int CustomerId, string QuotationNumber, int EnqId, string User_Id)
        {
            DataSet ds = new DataSet();
            EnquiryDAL objdal = new EnquiryDAL();
            return objdal.CreateQuotation(ItemId, CustomerId, QuotationNumber, EnqId, User_Id);
        }
        public int ReviwedQuotation(int QuotId, int EnqId, string User_Id, string QuotationNumber)
        {
            DataSet ds = new DataSet();
            EnquiryDAL objdal = new EnquiryDAL();
            return objdal.ReviewedQuotation(QuotId, EnqId, User_Id, QuotationNumber);
        }
        public Double GetExchangeRate(string Toccy, string FromCCy, Double Amount)
        {
            DataSet ds = new DataSet();

            EnquiryDAL objdal = new EnquiryDAL();
            Double Amt = objdal.GetExchageRate(Toccy, FromCCy, Amount);

            return Amt;
        }
        public int UpdateQuotation(int Terms, int QuotationId, string Currency, string Remark, int Rating,string User_Id)
        {
            DataSet ds = new DataSet();
            EnquiryDAL objdal = new EnquiryDAL();
            return objdal.UpdateQuotation(Terms, QuotationId, Currency, Remark, Rating, User_Id);
        }
        public int UpdateQuotationForApprovals(int EnqId, int Statusid, int QuotationId, string Remark, string User_Id)
        {
            DataSet ds = new DataSet();
            EnquiryDAL objdal = new EnquiryDAL();
            return objdal.UpdateQuotationForApprovals(EnqId, QuotationId, Statusid, Remark, User_Id);
        }
        public int SaveEnquiryItemVendorResponse(EnquiryItemVendorResponseDetailModel model, string Userid)
        {
            int errcode = 0;
            try
            {
                if (model.listEnqItemVendRespDetail == null)
                    model.listEnqItemVendRespDetail = new List<EnquiryItemVendorResponseDetailModel>();
                EnquiryDAL objdal = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetEnquiryItemVendorResponseDetailList(0);
                if (ds != null)
                {
                    if (ds.Tables.Count > 1)
                    {
                        ds.Tables[0].Rows.Clear();
                        DataRow Dr = ds.Tables[0].NewRow();
                        Dr["ResponseId"] = model.ResponseId;
                        Dr["VendorId"] = model.VendorId;
                        Dr["Currency"] = model.Currency;
                        Dr["ContactId"] = model.ContactId;
                        Dr["ReciptMethod"] = model.ReciptMethod;
                        Dr["ReciptDate"] = model.ReciptDate;
                        Dr["Remark"] = model.Remark;
                        Dr["Rating"] = model.rating;
                        ds.Tables[0].Rows.Add(Dr);
                    }
                }
                errcode = objdal.SaveEnquiryItemVendorResponse(ds, Userid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }
        public int SaveEnquiryItemVendorResponseDetails(EnquiryItemVendorResponseDetailModel model, string Userid, int ResponseId, int VendorId)
        {
            int errcode = 0;

            try
            {
                if (model.listEnqItemVendRespDetail == null)
                    model.listEnqItemVendRespDetail = new List<EnquiryItemVendorResponseDetailModel>();
                EnquiryDAL objdal = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetEnquiryItemVendorResponseDetailList(0);
                if (ds != null)
                {
                    if (ds.Tables.Count > 1)
                    {
                        ds.Tables[1].Rows.Clear();
                        foreach (EnquiryItemVendorResponseDetailModel tempModel in model.listEnqItemVendRespDetail)
                        {
                            DataRow Dr1 = ds.Tables[1].NewRow();
                            Dr1["ResponseId"] = tempModel.ResponseId;
                            Dr1["ItemId"] = tempModel.ItemId;
                            Dr1["BrandId"] = tempModel.BrandId;
                            Dr1["Rate"] = tempModel.Rate;
                            Dr1["MinimumQty"] = tempModel.MinimumQty;
                            Dr1["BatchNumber"] = tempModel.BatchNumber;
                            Dr1["VendorPromiseDate"] = tempModel.VendorPromiseDate;
                            Dr1["SPQ"] = tempModel.SPQ;
                            Dr1["MOQ"] = tempModel.MOQ;
                            Dr1["Chk"] = "OK";
                            ds.Tables[1].Rows.Add(Dr1);
                        }
                    }
                }
                errcode = objdal.SaveEnquiryItemVendorResponseDetails(ds, Userid, ResponseId, VendorId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }
        #endregion

        #region Enquiry PO
        public int SaveCustomerPO(CustomerPOModel POModel, string user_Id)
        {
            int errCode = 0;
            try
            {
                DataSet ds = new DataSet();
                EnquiryDAL ObjDAL = new EnquiryDAL();
                ds = ObjDAL.GetSeletedCustPOList(0);
                if (ds != null)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow Dr = ds.Tables[0].NewRow();
                    Dr["PurchaseOrderId"] = POModel.PurchaseOrderId;
                    Dr["PurchaseOrderNumber"] = POModel.PurchaseOrderNumber;
                    Dr["CustomerId"] = POModel.CustomerId;
                    Dr["Remark"] = POModel.Remark;
                    Dr["PODate"] = POModel.PODate;
                    Dr["DocumentFile"] = POModel.DocumentFile;
                    Dr["CompCode"] = POModel.CompCode;
                    Dr["Rating"] = POModel.Rating;
                    ds.Tables[0].Rows.Add(Dr);
                }
                errCode = ObjDAL.SaveCustomerPO(ds, user_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public CustomerPOModel SaveEnquiryPODetail(CustomerPOModel POModel, string user_Id, int PurchaseDetailOrderId, int PurchaseOrderId)
        {
            int errCode = 0;
            try
            {
                DataSet ds = new DataSet();
                EnquiryDAL ObjDAL = new EnquiryDAL();
                ds = ObjDAL.GetSeletedCustPOList(0);
                ds.Tables[1].Rows.Clear();
                foreach (CustomerPODetailModel TempPOModel in POModel.PODetaillist.Where(s => (s.PurchaseDetailOrderId == PurchaseDetailOrderId)))
                {
                    DataRow Dr = ds.Tables[1].NewRow();
                    Dr["PurchaseDetailOrderId"] = TempPOModel.PurchaseDetailOrderId;
                    Dr["PurchaseOrderId"] = TempPOModel.PurchaseOrderId;
                    Dr["ItemId"] = TempPOModel.ItemId;
                    Dr["Quantity"] = TempPOModel.Quantity;
                    Dr["Remark"] = TempPOModel.Remark;
                    Dr["ExpectedDate"] = TempPOModel.ExpectedDate;
                    Dr["QuotationId"] = TempPOModel.QuotationId;
                    Dr["Rate"] = TempPOModel.Rate;
                    ds.Tables[1].Rows.Add(Dr);
                }
                errCode = ObjDAL.SavePODetail(ds, user_Id, POModel.PurchaseOrderId);
                if (errCode == 500001 || errCode == 600002)
                {
                    POModel = getSelectedPODetail(PurchaseOrderId);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return POModel;
        }
        public CustomerPOModel getCustPOList(string UserId, int Status)
        {
            CustomerPOModel POmodelModel = new CustomerPOModel();
            POmodelModel.POlist = new List<CustomerPOModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                if (Status == 42 || Status == 24)
                {
                    ds = ObjDAL.GetCustomerPOApprovalList(UserId, Status);
                }
                else
                {
                    ds = ObjDAL.GetCustomerPOList(UserId, Status);
                }
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            CustomerPOModel objmodel = new CustomerPOModel();
                            objmodel.PurchaseOrderId = Convert.ToInt32(dr["PurchaseOrderId"].ToString());
                            objmodel.PurchaseOrderNumber = dr["PurchaseOrderNumber"].ToString();
                            objmodel.PODate = Convert.ToDateTime(dr["PODate"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.CompCode = dr["CompCode"].ToString();
                            objmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            objmodel.StatusStr = dr["StatusStr"].ToString();
                            objmodel.Status = Convert.ToInt32(dr["Status"].ToString());
                            //  objmodel.ExpectedDate = Convert.ToDateTime(dr["ExpectedDate"].ToString());
                            objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            objmodel.SalesOrderNo = dr["SalesOrderNo_"].ToString();
                            POmodelModel.POlist.Add(objmodel);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return POmodelModel;
        }
        public List<SalesOrderModel> GetSalesOrderList(string CompCode, int CustomerId)
        {
            List<SalesOrderModel> LstSO = new List<SalesOrderModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSalesOrderList(CompCode, CustomerId);
                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                SalesOrderModel SOMOdel = new SalesOrderModel();
                                SOMOdel.SOId = dr["Document No_"].ToString();
                                SOMOdel.SOName = dr["Document No_"].ToString();
                                LstSO.Add(SOMOdel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstSO;
        }
        public CustomerPOModel getSelectedPODetail(int PurchaseOrderId)
        {
            CustomerPOModel objmodel = new CustomerPOModel();
            objmodel.PODetaillist = new List<CustomerPODetailModel>();
            int CompleteItem = 0;
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSeletedCustPOList(PurchaseOrderId);
                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            objmodel.PurchaseOrderId = Convert.ToInt32(ds.Tables[0].Rows[0]["PurchaseOrderId"].ToString());
                            objmodel.PurchaseOrderNumber = ds.Tables[0].Rows[0]["PurchaseOrderNumber"].ToString();
                            objmodel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                            objmodel.PODate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PODate"].ToString());
                            objmodel.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                            objmodel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                            objmodel.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"]);
                            objmodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                            objmodel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            objmodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            objmodel.StatusStr = ds.Tables[0].Rows[0]["Status"].ToString();
                            objmodel.Status = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"].ToString());
                            objmodel.DocumentFile = ds.Tables[0].Rows[0]["DocumentFile"].ToString();
                            objmodel.DocumentFileOld = objmodel.DocumentFile;
                            objmodel.SalesOrderNo = ds.Tables[0].Rows[0]["SalesOrderNo_"].ToString();
                            objmodel.Rating = Convert.ToInt32(ds.Tables[0].Rows[0]["Rating"].ToString());
                        }
                    }
                    if (ds.Tables.Count > 1)
                    {

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                CustomerPODetailModel PODetail = new CustomerPODetailModel();

                                PODetail.PurchaseDetailOrderId = Convert.ToInt32(dr["PurchaseDetailOrderId"].ToString());
                                PODetail.PurchaseOrderId = Convert.ToInt32(dr["PurchaseOrderId"].ToString());
                                PODetail.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                                PODetail.ItemName = dr["ItemName"].ToString();
                                PODetail.MPN = dr["MPN"].ToString();
                                PODetail.Quantity = Convert.ToDouble(dr["Quantity"].ToString());
                                PODetail.Remark = dr["Remark"].ToString();
                                PODetail.Rate = Convert.ToDouble(dr["Rate"].ToString());
                                PODetail.QuotationId = Convert.ToInt32(dr["QuotationId"].ToString());
                                PODetail.QuotationName = dr["QuotationNumber"].ToString();
                                PODetail.ModifiedBy = dr["ModifiedBy"].ToString();

                                PODetail.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                PODetail.ModifiedDatestr = PODetail.ModifiedDate.ToShortDateString();

                                if (dr["ExpectedDate"].ToString() != "")
                                    PODetail.ExpectedDate = Convert.ToDateTime(dr["ExpectedDate"].ToString());
                                PODetail.ExpectedDateStr = PODetail.ExpectedDate.ToShortDateString();
                                objmodel.PODetaillist.Add(PODetail);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }
        public List<QuotationModel> GetQuotationListfordrpdwn(string User_Id)
        {
            List<QuotationModel> LstQuotationModel = new List<QuotationModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetQuotationListfordrpdwn(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            QuotationModel tempQuotModel = new QuotationModel();
                            tempQuotModel.QuotationId = Convert.ToInt32(dr["QuotationId"]);
                            tempQuotModel.QuotationNumber = dr["QuotationNumber"].ToString();
                            LstQuotationModel.Add(tempQuotModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstQuotationModel;
        }
        public int DeletePurchaseDetail(int PurchaseDetailOrderId)
        {
            int ErrCode = 0;
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                ErrCode = DALObj.DeletePurchaseDetail(PurchaseDetailOrderId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public int SendPOForApproval(int PurchaeOrderId, int StatusId, string User_Id)
        {
            int errcode = 0;
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                errcode = DALObj.SendPOForApproval(PurchaeOrderId, StatusId, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errcode;
        }
        public int POApprovalReject(int PurchaeOrderId, int StatusId, string User_Id)
        {
            int errcode = 0;
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                errcode = DALObj.POFApprovalReject(PurchaeOrderId, StatusId, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errcode;
        }
        public int SaveSONumber(int PurchaeOrderId, string SONumber, string User_Id)
        {
            int errcode = 0;
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                errcode = DALObj.SaveSONumber(PurchaeOrderId, SONumber, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errcode;
        }
        public int EditSODetail(int PurchaseOrderId, string OldSOId, string SOId, string User_Id, string PurchaseOrderNumber, DateTime PODate, string Remark)
        {
            EnquiryDAL ObjDAL = new EnquiryDAL();
            int ErrCode = 0;
            try
            {
                ErrCode = ObjDAL.EditSODetail(PurchaseOrderId, OldSOId, SOId, User_Id, PurchaseOrderNumber, PODate, Remark);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        #endregion Enquiry PO

        #region Purchase Receipt 

        public List<ReceiptModel> GetPurchaseReceiptList(string User_Id)
        {
            List<ReceiptModel> LstReceiptModel = new List<ReceiptModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetPurchaseReceiptList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ReceiptModel tempReceiptModel = new ReceiptModel();
                            tempReceiptModel.ReceiptId = Convert.ToInt32(dr["ReceiptId"]);
                            tempReceiptModel.CompanyName = dr["CompanyName"].ToString();
                            tempReceiptModel.VendorName = dr["VendorName"].ToString();
                            //  tempReceiptModel.County = dr["Country"].ToString();
                            tempReceiptModel.VendorName = dr["VendorName"].ToString();
                            tempReceiptModel.FreightForwarderName = dr["FreightForwarder"].ToString();
                            tempReceiptModel.FFRef_No = dr["FFRef_No"].ToString();
                            tempReceiptModel.Currency = dr["Currency"].ToString();
                            // tempReceiptModel.PaymentTerm = dr["PTCode"].ToString();
                            tempReceiptModel.Currency = dr["Currency"].ToString();
                            // tempReceiptModel.freightCharges = Convert.ToDouble(dr["freightCharges"].ToString());
                            tempReceiptModel.Status = dr["StatusShortCode"].ToString();
                            tempReceiptModel.PermitType = dr["PermitType"].ToString();
                            tempReceiptModel.ReceiptDate = Convert.ToDateTime(dr["ReceiptDate"].ToString());
                            tempReceiptModel.VendorInvNo = dr["VendorInvNo"].ToString();
                            tempReceiptModel.CreatedBy = dr["CreatedBy"].ToString();
                            tempReceiptModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            tempReceiptModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            tempReceiptModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            LstReceiptModel.Add(tempReceiptModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstReceiptModel;
        }

        public List<ReceiptModel> FFNotificationList(string User_Id)
        {
            List<ReceiptModel> LstReceiptModel = new List<ReceiptModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetPurchaseReceiptList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["PermitType"].ToString() == "ImportPermit")
                            {
                                ReceiptModel tempReceiptModel = new ReceiptModel();
                                tempReceiptModel.ReceiptId = Convert.ToInt32(dr["ReceiptId"]);
                                tempReceiptModel.CompanyName = dr["CompanyName"].ToString();
                                tempReceiptModel.VendorName = dr["VendorName"].ToString();
                                //  tempReceiptModel.County = dr["Country"].ToString();
                                tempReceiptModel.VendorName = dr["VendorName"].ToString();
                                tempReceiptModel.FreightForwarderName = dr["FreightForwarder"].ToString();
                                tempReceiptModel.FFRef_No = dr["FFRef_No"].ToString();
                                tempReceiptModel.Currency = dr["Currency"].ToString();
                                // tempReceiptModel.PaymentTerm = dr["PTCode"].ToString();
                                tempReceiptModel.Currency = dr["Currency"].ToString();
                                // tempReceiptModel.freightCharges = Convert.ToDouble(dr["freightCharges"].ToString());
                                tempReceiptModel.Status = dr["StatusShortCode"].ToString();
                                tempReceiptModel.PermitType = dr["PermitType"].ToString();
                                tempReceiptModel.ReceiptDate = Convert.ToDateTime(dr["ReceiptDate"].ToString());
                                tempReceiptModel.VendorInvNo = dr["VendorInvNo"].ToString();
                                tempReceiptModel.CreatedBy = dr["CreatedBy"].ToString();
                                tempReceiptModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                tempReceiptModel.ModifiedBy = dr["ModifiedBy"].ToString();
                                tempReceiptModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                LstReceiptModel.Add(tempReceiptModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstReceiptModel;
        }
        public FFNotificationModel GetSelectedFFNotification(string AirwaybillNumber, int ReceiptId)
        {
            FFNotificationModel ReceiptModel = new FFNotificationModel();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedFFNotification(AirwaybillNumber, ReceiptId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReceiptModel.ReceiptId = Convert.ToInt32(ds.Tables[0].Rows[0]["ReceiptId"]);
                        if (ds.Tables[0].Rows[0]["ReceiptDate"].ToString() != "")
                            ReceiptModel.ReceiptDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ReceiptDate"].ToString());
                        ReceiptModel.CompanyName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                        ReceiptModel.VendorName = ds.Tables[0].Rows[0]["VendorName"].ToString();
                        ReceiptModel.FreightForwarderName = ds.Tables[0].Rows[0]["FreightForwarder"].ToString();
                        ReceiptModel.FFRef_No = ds.Tables[0].Rows[0]["AirwayBillNo"].ToString();
                        ReceiptModel.VendorInvNo = ds.Tables[0].Rows[0]["VendorInvNo"].ToString();
                        ReceiptModel.PermitType = ds.Tables[0].Rows[0]["PermitType"].ToString();

                        ReceiptModel.NotificationId = Convert.ToInt32(ds.Tables[0].Rows[0]["NotificationId"].ToString());
                        if (ReceiptModel.NotificationId > 0)
                        {
                            ReceiptModel.RefNo = ds.Tables[0].Rows[0]["RefNo"].ToString();
                            ReceiptModel.NotificationDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["NotificationDate"].ToString());
                            ReceiptModel.GSTAmount = Convert.ToDouble(ds.Tables[0].Rows[0]["GSTAmount"].ToString());
                            ReceiptModel.Currency = ds.Tables[0].Rows[0]["Currency"].ToString();
                            ReceiptModel.PermitNo = ds.Tables[0].Rows[0]["PermitNo"].ToString();
                            ReceiptModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            ReceiptModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            ReceiptModel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            ReceiptModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ReceiptModel;
        }

        public int SaveFFNotificationDetail(string AirwaybillNo, int NotificationId, string RefNo, DateTime NotificationDate, string PermitNo, double GSTAmount, string Currency, string User_ID, int Receiptid)
        {
            int ErrCode = 0;
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                ErrCode = ObjDAL.SaveFFNotificationDetail(AirwaybillNo, NotificationId, RefNo, NotificationDate, PermitNo, GSTAmount, Currency, User_ID, Receiptid);
            }
            catch (Exception)
            {
                throw;
            }
            return ErrCode;
        }
        //public List<ReceiptModel> GetFFChargesList()
        //{
        //    List<ReceiptModel> LstReceiptModel = new List<ReceiptModel>();
        //    try
        //    {
        //        EnquiryDAL ObjDAL = new EnquiryDAL();
        //        DataSet ds = new DataSet();
        //        ds = ObjDAL.GetPurchaseReceiptList();
        //        if (ds == null)
        //            return null;
        //        if (ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                foreach (DataRow dr in ds.Tables[0].Rows)
        //                {
        //                    ReceiptModel tempReceiptModel = new ReceiptModel();
        //                    tempReceiptModel.ReceiptDate = Convert.ToDateTime(dr["ReceiptDate"].ToString());
        //                    tempReceiptModel.VendorInvNo = dr["VendorInvNo"].ToString();
        //                    tempReceiptModel.VendorInvDate = Convert.ToDateTime(dr["VendorInvDate"].ToString());
        //                    tempReceiptModel.FreightInvNo = dr["FreightInvNo"].ToString();
        //                    tempReceiptModel.FreightInvDate = Convert.ToDateTime(dr["FreightInvDate"].ToString());
        //                    tempReceiptModel.FrieghtInvReceivedDate = Convert.ToDateTime(dr["FrieghtInvReceivedDate"].ToString());
        //                    tempReceiptModel.Amount = Convert.ToDouble(dr["Amount"].ToString());
        //                    tempReceiptModel.ReceiptId = Convert.ToInt32(dr["ReceiptId"]);
        //                    tempReceiptModel.CompanyName = dr["CompanyName"].ToString();
        //                    tempReceiptModel.VendorName = dr["VendorName"].ToString();
        //                    tempReceiptModel.County = dr["Country"].ToString();
        //                    tempReceiptModel.FreightForwarderName = dr["FreightForwarder"].ToString();
        //                    tempReceiptModel.FFRef_No = dr["FFRef_No"].ToString();
        //                    tempReceiptModel.Currency = dr["Currency"].ToString();
        //                    tempReceiptModel.PaymentTerm = dr["PTCode"].ToString();
        //                    tempReceiptModel.freightCharges = Convert.ToDouble(dr["freightCharges"].ToString());
        //                    tempReceiptModel.Status = dr["StatusShortCode"].ToString();
        //                    tempReceiptModel.CreatedBy = dr["CreatedBy"].ToString();
        //                    tempReceiptModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
        //                    tempReceiptModel.ModifiedBy = dr["ModifiedBy"].ToString();
        //                    tempReceiptModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
        //                    LstReceiptModel.Add(tempReceiptModel);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.LogException(ex);
        //    }
        //    return LstReceiptModel;
        //}
        public ReceiptModel GetSelectedReceipt(int ReceiptId)
        {
            ReceiptModel ReceiptModel = new ReceiptModel();
            ReceiptModel.LstReceiptModel = new List<ReceiptDetailModel>();
            ReceiptModel.LstReceiptTaxModel = new List<ReceiptTaxModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedPurchaseReceiptList(ReceiptId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReceiptModel.ReceiptId = Convert.ToInt32(ds.Tables[0].Rows[0]["ReceiptId"]);
                        if (ds.Tables[0].Rows[0]["ReceiptDate"].ToString() != "")
                            ReceiptModel.ReceiptDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ReceiptDate"].ToString());
                        ReceiptModel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                        ReceiptModel.CompanyName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                        ReceiptModel.VendorId = Convert.ToInt32(ds.Tables[0].Rows[0]["VendorId"].ToString());
                        ReceiptModel.VendorName = ds.Tables[0].Rows[0]["VendorName"].ToString();
                        ReceiptModel.FreightForwarderId = Convert.ToInt32(ds.Tables[0].Rows[0]["FreightForwarderId"].ToString());
                        ReceiptModel.FreightForwarderName = ds.Tables[0].Rows[0]["FreightForwarder"].ToString();
                        ReceiptModel.FFRef_No = ds.Tables[0].Rows[0]["FFRef_No"].ToString();
                        ReceiptModel.Currency = ds.Tables[0].Rows[0]["Currency"].ToString();
                        ReceiptModel.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"]);
                        if (ds.Tables[0].Rows[0]["MRIId"].ToString() != "")
                            ReceiptModel.MRIId = Convert.ToInt32(ds.Tables[0].Rows[0]["MRIId"]);
                        if (ds.Tables[0].Rows[0]["DocumentNo"].ToString() != "")
                            ReceiptModel.MRIDOCNo = ds.Tables[0].Rows[0]["DocumentNo"].ToString();
                        // ReceiptModel.freightCharges = Convert.ToDouble(ds.Tables[0].Rows[0]["freightCharges"].ToString());
                        if (ds.Tables[0].Rows[0]["VendorInvDate"].ToString() != "")
                            ReceiptModel.SupplierInvDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["VendorInvDate"].ToString());
                        ReceiptModel.SupplierInvN0 = ds.Tables[0].Rows[0]["VendorInvNo"].ToString();
                        ReceiptModel.PermitType = ds.Tables[0].Rows[0]["PermitType"].ToString();
                        ReceiptModel.ReceiptFile = ds.Tables[0].Rows[0]["ReceiptFile"].ToString();
                        ReceiptModel.Rating = Convert.ToInt32(ds.Tables[0].Rows[0]["Rating"].ToString());
                        ReceiptModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        ReceiptModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        ReceiptModel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        ReceiptModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        if (ds.Tables[0].Rows[0]["ApproveBy"].ToString() != "")
                            ReceiptModel.ApproveBy = ds.Tables[0].Rows[0]["ApproveBy"].ToString();
                        if (ds.Tables[0].Rows[0]["ApprovedDate"].ToString() != "")
                            ReceiptModel.ApprovedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ApprovedDate"].ToString());
                        if (ds.Tables[0].Rows[0]["ReviewedBy"].ToString() != "")
                            ReceiptModel.ReviewedBy = ds.Tables[0].Rows[0]["ReviewedBy"].ToString();
                        if (ds.Tables[0].Rows[0]["ReviewedDate"].ToString() != "")
                            ReceiptModel.ReviewedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ReviewedDate"].ToString());
                        if (ds.Tables[0].Rows[0]["RejectedBy"].ToString() != "")
                            ReceiptModel.RejectedBy = ds.Tables[0].Rows[0]["RejectedBy"].ToString();
                        if (ds.Tables[0].Rows[0]["RejectedDate"].ToString() != "")
                            ReceiptModel.RejectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["RejectedDate"].ToString());
                        ReceiptModel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                        ReceiptModel.RejectedRemark = ds.Tables[0].Rows[0]["RejectedRemark"].ToString();
                        ReceiptModel.ReviewedRemark = ds.Tables[0].Rows[0]["ReviewedRemark"].ToString();
                        ReceiptModel.ApprovedRemark = ds.Tables[0].Rows[0]["ApprovedRemark"].ToString();
                        if (ds.Tables.Count > 1)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                ReceiptDetailModel DetailModel = new ReceiptDetailModel();
                                DetailModel.ReceiptDetailId = Convert.ToInt32(dr["ReceiptDetailId"]);
                                DetailModel.ReceiptId = Convert.ToInt32(dr["ReceiptId"]);
                                DetailModel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                                DetailModel.ItemName = dr["ItemName"].ToString();
                                DetailModel.MPN = dr["MPN"].ToString();
                                DetailModel.Dimension = dr["Dimension"].ToString();
                                DetailModel.Country = dr["Country"].ToString();
                                DetailModel.CartonId = Convert.ToInt32(dr["CartonId"]);
                                DetailModel.COO = Convert.ToInt32(dr["COO"]);
                                DetailModel.Quantity = Convert.ToDouble(dr["Quantity"].ToString());
                                DetailModel.PO_No = dr["PO_No"].ToString();
                                DetailModel.InvoiceAmount = Convert.ToDouble(dr["InvoiceAmount"].ToString());
                                DetailModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                DetailModel.CreatedBy = dr["CreatedBy"].ToString();
                                ReceiptModel.LstReceiptModel.Add(DetailModel);
                            }
                        }
                        if (ds.Tables.Count > 2)
                        {
                            foreach (DataRow dr in ds.Tables[2].Rows)
                            {
                                ReceiptTaxModel TaxModel = new ReceiptTaxModel();
                                TaxModel.ReceiptDetailId = Convert.ToInt32(dr["ReceiptDetailId"]);
                                TaxModel.TaxId = Convert.ToInt32(dr["TaxId"].ToString());
                                TaxModel.TaxName = dr["TaxName"].ToString();
                                TaxModel.TaxRate = Convert.ToDouble(dr["TaxRate"].ToString());
                                TaxModel.PaidByVendor = Convert.ToBoolean(dr["PaidByVendor"].ToString());
                                TaxModel.TaxAmount = Convert.ToDouble(dr["TaxAmount"].ToString());
                                TaxModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                TaxModel.CreatedBy = dr["CreatedBy"].ToString();
                                TaxModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                TaxModel.ModifiedBy = dr["ModifiedBy"].ToString();
                                ReceiptModel.LstReceiptTaxModel.Add(TaxModel);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ReceiptModel;
        }
        public int SavePurchaseReceipt(ReceiptModel RecepModel, string User_Id)
        {
            int errCode = 0;

            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedPurchaseReceiptList(0);
                ds.Tables[0].Rows.Clear();
                DataRow Dr = ds.Tables[0].NewRow();
                Dr["ReceiptId"] = RecepModel.ReceiptId;
                Dr["ReceiptDate"] = RecepModel.ReceiptDate;
                Dr["CompCode"] = RecepModel.CompCode;
                Dr["VendorId"] = RecepModel.VendorId;
                Dr["VendorInvNo"] = RecepModel.SupplierInvN0;
                Dr["VendorInvDate"] = RecepModel.SupplierInvDate;
                Dr["FreightForwarderId"] = RecepModel.FreightForwarderId;
                Dr["FFRef_No"] = RecepModel.FFRef_No;
                Dr["Currency"] = RecepModel.Currency;
                Dr["PermitType"] = RecepModel.PermitType;
                Dr["ReceiptFile"] = RecepModel.ReceiptFile;
                Dr["Rating"] = RecepModel.Rating;
                Dr["MRIId"] = RecepModel.MRIId;
                Dr["Remark"] = RecepModel.Remark;
                // Dr["freightCharges"] = RecepModel.freightCharges;
                ds.Tables[0].Rows.Add(Dr);
                ds.Tables[1].Rows.Clear();

                errCode = ObjDAL.SendPurchaseReceipt(ds, User_Id, RecepModel.ReceiptId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        public ReceiptModel SavePurchaseReceiptDetail(ReceiptModel RecepModel, string User_Id, int ReceiptDetailId, int ReceiptId)
        {
            int errCode = 0;
            ReceiptModel RecepMod = new ReceiptModel();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedPurchaseReceiptList(0);
                ds.Tables[1].Rows.Clear();
                foreach (ReceiptDetailModel DetailModel in RecepModel.LstReceiptModel.Where(s => (s.ReceiptDetailId == ReceiptDetailId)))
                {
                    DataRow Dr1 = ds.Tables[1].NewRow();
                    Dr1["ReceiptId"] = DetailModel.ReceiptId;
                    Dr1["ItemId"] = DetailModel.ItemId;
                    Dr1["COO"] = DetailModel.COO;
                    Dr1["CartonId"] = DetailModel.CartonId;
                    Dr1["Quantity"] = DetailModel.Quantity;
                    Dr1["PO_No"] = DetailModel.PO_No;
                    Dr1["InvoiceAmount"] = DetailModel.InvoiceAmount;
                    ds.Tables[1].Rows.Add(Dr1);
                }
                ds.Tables[2].Rows.Clear();
                if (RecepModel.LstReceiptTaxModel != null)
                {
                    foreach (ReceiptTaxModel TaxModel in RecepModel.LstReceiptTaxModel.Where(s => (s.ReceiptDetailId == ReceiptDetailId)))
                    {
                        DataRow Dr1 = ds.Tables[2].NewRow();
                        Dr1["ReceiptDetailId"] = TaxModel.ReceiptDetailId;
                        Dr1["PaidByVendor"] = TaxModel.PaidByVendor;
                        Dr1["TaxId"] = TaxModel.TaxId;
                        Dr1["TaxRate"] = TaxModel.TaxRate;
                        Dr1["TaxAmount"] = TaxModel.TaxAmount;
                        ds.Tables[2].Rows.Add(Dr1);
                    }
                }
                errCode = ObjDAL.SendPurchaseReceiptDetail(ds, User_Id, RecepModel.ReceiptId, ReceiptDetailId);
                if (errCode > 0)
                {
                    RecepMod = GetSelectedReceipt(ReceiptId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RecepMod;
        }
        public int DeleteReceiptDetail(int ReceiptDetailId)
        {
            EnquiryDAL objdal = new EnquiryDAL();
            return objdal.DeleteReceiptDetail(ReceiptDetailId);
        }
        public int DeleteReceiptItemLocation(int ReceiptItemDetailId)
        {
            EnquiryDAL objdal = new EnquiryDAL();
            return objdal.DeleteReceiptItemLocation(ReceiptItemDetailId);
        }
        public ReceiptItemLocationDetail GetReceiptItemDetail(int ReceiptDetailId)
        {
            ReceiptItemLocationDetail ItemLocModel = new ReceiptItemLocationDetail();
            ItemLocModel.ModelLst = new List<ReceiptItemLocationDetail>();            
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetReceiptItemDetail(ReceiptDetailId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ReceiptItemLocationDetail Model = new ReceiptItemLocationDetail();
                            Model.ReceiptItemDetailId = Convert.ToInt32(dr["ReceiptItemDetailId"].ToString());
                            Model.MPN = dr["MPN"].ToString();
                            Model.LocationName = dr["Name"].ToString();
                            Model.COOName = dr["Country"].ToString();
                            Model.DateCode = dr["DateCode"].ToString();
                            Model.Quantity = Convert.ToInt32(dr["Quantity"].ToString());
                            Model.CreatedBy = dr["CreatedBy"].ToString();
                            DateTime dt = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            Model.CreatedDate = dt.ToShortDateString();
                            ItemLocModel.ModelLst.Add(Model);
                        }
                    }
                }
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        ItemLocModel.TotalQuantity = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalQty"]);
                    }
                    else
                    {
                        ItemLocModel.TotalQuantity = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ItemLocModel;
        }
        public int CheckReceiptItemLocationDup(int ReceiptDetailId, int ItemId, int LocationId, int COOId, string DateCode)
        {
            int ErrCode = 0;
            EnquiryDAL objdal = new EnquiryDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.CheckReceiptItemLocationDup(ReceiptDetailId, ItemId, LocationId, COOId, DateCode);
                if(ds != null)
                {
                    ErrCode = ds.Tables[0].Rows.Count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ErrCode;
        }
        public int SaveItemLocationDetails(int ItemDetId, int ItemId, int LocationId, int COOId, int Qty, string DateCode,string User_Id)
        {
            EnquiryDAL objdal = new EnquiryDAL();
            return objdal.SaveItemLocationDetails(ItemDetId, ItemId, LocationId, COOId, Qty, DateCode, User_Id);
        }
        public int DeleteReceiptDetailTax(int ReceiptDetailId, int Tax)
        {
            EnquiryDAL objdal = new EnquiryDAL();
            return objdal.DeleteReceiptTax(ReceiptDetailId, Tax);
        }
        public int SaveOtherChargesPurReceipt(int ReceiptId, string ChargesDescription, string Currency, float Amount)
        {
            EnquiryDAL objdal = new EnquiryDAL();
            return objdal.SaveOtherChargesPurReceipt(ReceiptId, ChargesDescription, Currency, Amount);
        }
        public List<ReceiptModel> OtherCharges(int ReceiptId)
        {
            List<ReceiptModel> LstOtherCharges = new List<ReceiptModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetOtherChargesList(ReceiptId);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ReceiptModel OtherChargesModel = new ReceiptModel();
                            OtherChargesModel.Amount = Convert.ToDouble(dr["Amount"].ToString());
                            OtherChargesModel.ChargesDescription = dr["ChargesDescription"].ToString();
                            OtherChargesModel.Currency = dr["Currency"].ToString();
                            LstOtherCharges.Add(OtherChargesModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstOtherCharges;
        }
        public List<EnquiryBrandVemdorModel> GetVendorListByCompCode(string CompCode)
        {
            List<EnquiryBrandVemdorModel> LstVendorModel = new List<EnquiryBrandVemdorModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetVendorListByCompCode(CompCode);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EnquiryBrandVemdorModel VendorModel = new EnquiryBrandVemdorModel();
                            VendorModel.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            VendorModel.Vendor_Id = dr["Vendor_No"].ToString();
                            VendorModel.VendorName = dr["VendorName"].ToString();
                            LstVendorModel.Add(VendorModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstVendorModel;
        }
        public List<EnquiryBrandVemdorModel> GetVendorListByCompCodeFromMRI(string CompCode)
        {
            List<EnquiryBrandVemdorModel> LstVendorModel = new List<EnquiryBrandVemdorModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetVendorListByCompCodeFromMRI(CompCode);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EnquiryBrandVemdorModel VendorModel = new EnquiryBrandVemdorModel();
                            VendorModel.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                            VendorModel.VendorName = dr["VendorName"].ToString();
                            LstVendorModel.Add(VendorModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstVendorModel;
        }
        public List<MDIModel> GetIntimationListByVendor(string CompCode, int VendorId)
        {
            List<MDIModel> ModelList = new List<MDIModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetIntimationListByVendor(CompCode, VendorId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            MDIModel Model = new MDIModel();
                            Model.MRIId = Convert.ToInt32(dr["MRIId"].ToString());
                            Model.DocumentNo = dr["DocumentNo"].ToString();
                            ModelList.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ModelList;
        }
        public List<InvoiceItemModel> GetItemsByReceiptIdFromMRI(int ReceiptId)
        {
            List<InvoiceItemModel> LstInvItemModel = new List<InvoiceItemModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetItemsByReceiptIdFromMRI(ReceiptId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            InvoiceItemModel InvoiceItemModel = new InvoiceItemModel();
                            InvoiceItemModel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            InvoiceItemModel.ItemName = dr["MPN"].ToString();
                            LstInvItemModel.Add(InvoiceItemModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstInvItemModel;
        }
        public List<InvoiceModel> GetPObyItemFromMRI(int MRIID, int ItemId)
        {
            List<InvoiceModel> LstInvModel = new List<InvoiceModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetPObyItemFromMRI(MRIID, ItemId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            InvoiceModel InvoiceModel = new InvoiceModel();
                            InvoiceModel.Invoice_Id = dr["PurchaseOrderNumber"].ToString();
                            InvoiceModel.InvoiceName = dr["PONumberDesc"].ToString();
                            LstInvModel.Add(InvoiceModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstInvModel;
        }
        public List<InvoiceModel> GetPOforReceiptByItem(string CompCode, int ItemId)
        {
            List<InvoiceModel> LstInvModel = new List<InvoiceModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetPOforReceiptByItem(CompCode, ItemId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            InvoiceModel InvoiceModel = new InvoiceModel();
                            InvoiceModel.Invoice_Id = dr["PurchaseOrderNumber"].ToString();
                            InvoiceModel.InvoiceName = dr["PONumberDesc"].ToString();
                            LstInvModel.Add(InvoiceModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstInvModel;
        }
        public List<InvoiceItemModel> GetItemListByReceipt(string CompCode, int VendorId)
        {
            List<InvoiceItemModel> LstInvItemModel = new List<InvoiceItemModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetItemListByReceipt(CompCode, VendorId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            InvoiceItemModel InvoiceItemModel = new InvoiceItemModel();
                            InvoiceItemModel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            InvoiceItemModel.Item_Id = dr["Item_No"].ToString();
                            InvoiceItemModel.ItemName = dr["ItemName"].ToString();
                            LstInvItemModel.Add(InvoiceItemModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstInvItemModel;
        }
        public List<ReceiptTaxModel> GetReceiptItemTaxDetail(int ReceiptId, int ItemId)
        {
            List<ReceiptTaxModel> LstTaxModel = new List<ReceiptTaxModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetReceiptItemTaxDetail(ReceiptId, ItemId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ReceiptTaxModel ItemTaxModel = new ReceiptTaxModel();
                            ItemTaxModel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            ItemTaxModel.ReceiptId = Convert.ToInt32(dr["ReceiptId"].ToString());
                            ItemTaxModel.TaxId = Convert.ToInt32(dr["TaxId"].ToString());
                            ItemTaxModel.TaxName = dr["TaxName"].ToString();
                            ItemTaxModel.TaxRate = Convert.ToDouble(dr["TaxRate"].ToString());
                            ItemTaxModel.TaxAmount = Convert.ToDouble(dr["TaxAmount"].ToString());
                            ItemTaxModel.CreatedBy = dr["CreatedBy"].ToString();
                            ItemTaxModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            ItemTaxModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            ItemTaxModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            LstTaxModel.Add(ItemTaxModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstTaxModel;
        }
        public List<TaxOnTax> GetTaxOnTaxList(int ChildTaxId)
        {
            List<TaxOnTax> LstTaxModel = new List<TaxOnTax>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetTaxOnTaxList(ChildTaxId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            TaxOnTax ItemTaxModel = new TaxOnTax();
                            ItemTaxModel.TaxId = Convert.ToInt32(dr["TaxId"].ToString());
                            ItemTaxModel.ChildTaxId = Convert.ToInt32(dr["ChildTaxId"].ToString());
                            LstTaxModel.Add(ItemTaxModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstTaxModel;
        }
        //public int SavereciptCarton(int Recipt, double Height, double width, double lenght, double Weight, string Remark)
        //{
        //    int ErrCode = 0;
        //    try
        //    {
        //        EnquiryDAL ObjDAL = new EnquiryDAL();
        //        ErrCode = ObjDAL.SavereciptCarton(Recipt, Height, width, lenght, Weight, Remark);
        //    }
        //    catch (Exception ex)
        //    {                
        //        throw ex;
        //    }
        //    return ErrCode;
        //}

        public List<CartonModel> GetSelectedCartonList(int ReceiptId)
        {
            List<CartonModel> LstCarronModel = new List<CartonModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedCartonList(ReceiptId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CartonModel CartonModel = new CartonModel();
                            CartonModel.CartonId = Convert.ToInt32(dr["CartonId"].ToString());
                            CartonModel.ReceiptId = Convert.ToInt32(dr["ReceiptId"].ToString());
                            CartonModel.Weight = Convert.ToDouble(dr["Weight"].ToString());
                            CartonModel.Height = Convert.ToDouble(dr["Height"].ToString());
                            CartonModel.Width = Convert.ToDouble(dr["Width"].ToString());
                            CartonModel.Length = Convert.ToDouble(dr["Length"].ToString());
                            CartonModel.Remark = dr["Remark"].ToString();
                            LstCarronModel.Add(CartonModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstCarronModel;
        }
        public List<CartonModel> DrpCartonList(int ReceiptId)
        {
            List<CartonModel> LstCarronModel = new List<CartonModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedCartonList(ReceiptId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CartonModel CartonModel = new CartonModel();
                            CartonModel.CartonId = Convert.ToInt32(dr["CartonId"].ToString());
                            CartonModel.Dimension = dr["Dimension"].ToString();
                            LstCarronModel.Add(CartonModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstCarronModel;
        }
        public List<AirwaysReceiptModel> GetPurchaseInvForReceiptByVendor(string CompCode, int VendorId)
        {
            List<AirwaysReceiptModel> LstModel = new List<AirwaysReceiptModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetPurchaseInvForReceiptByVendor(CompCode, VendorId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            AirwaysReceiptModel Model = new AirwaysReceiptModel();
                            Model.FFRef_No = dr["No_"].ToString();
                            LstModel.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstModel;
        }
        public int EditCartonint(int CartId, double Weight, double Length, double Width, double Height, string Remark)
        {
            int ErrCod = 0;
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                ErrCod = DALObj.UpdateEditCarton(CartId, Weight, Length, Width, Height, Remark);
            }
            return ErrCod;
        }
        public int SaveProcessPurchaseReceipt(int StatusId, int ReceiptId, string PurcInv, string User_Id)
        {
            int ErrCode = 0;
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                ErrCode = ObjDAL.SaveProcessPurchaseReceipt(StatusId, ReceiptId, PurcInv, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ErrCode;
        }
        #endregion Purchase Receipt

        #region Review Purchase Receipt
        public List<ReceiptModel> GetReviewPurchaseReceiptList(string User_Id)
        {
            List<ReceiptModel> LstReceiptModel = new List<ReceiptModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetReviewPurchaseReceiptList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ReceiptModel tempReceiptModel = new ReceiptModel();
                            tempReceiptModel.ReceiptId = Convert.ToInt32(dr["ReceiptId"]);
                            tempReceiptModel.CompanyName = dr["CompanyName"].ToString();
                            tempReceiptModel.VendorName = dr["VendorName"].ToString();
                            // tempReceiptModel.County = dr["Country"].ToString();
                            tempReceiptModel.FreightForwarderName = dr["FreightForwarder"].ToString();
                            tempReceiptModel.FFRef_No = dr["FFRef_No"].ToString();
                            tempReceiptModel.Currency = dr["Currency"].ToString();
                            //tempReceiptModel.PaymentTerm = dr["PTCode"].ToString();
                            // tempReceiptModel.freightCharges = Convert.ToDouble(dr["freightCharges"].ToString());
                            tempReceiptModel.Status = dr["StatusShortCode"].ToString();
                            tempReceiptModel.CreatedBy = dr["CreatedBy"].ToString();
                            tempReceiptModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            tempReceiptModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            tempReceiptModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            LstReceiptModel.Add(tempReceiptModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstReceiptModel;
        }
        public int ReviewRejectPurchaseReceipt(int StatusId, int ReceiptId, string User_Id,string ReviewedApprovedRejectRemark)
        {
            int ErrCode = 0;
            EnquiryDAL ObjDAL = new EnquiryDAL();
            try
            {
                ErrCode = ObjDAL.ReviewRejectPurchaseReceipt(StatusId, ReceiptId, User_Id, ReviewedApprovedRejectRemark);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public List<ReceiptModel> GetApprovePurchaseReceiptList(string User_Id)
        {
            List<ReceiptModel> LstReceiptModel = new List<ReceiptModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetApprovePurchaseReceiptList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ReceiptModel tempReceiptModel = new ReceiptModel();
                            tempReceiptModel.ReceiptId = Convert.ToInt32(dr["ReceiptId"]);
                            tempReceiptModel.CompanyName = dr["CompanyName"].ToString();
                            tempReceiptModel.VendorName = dr["VendorName"].ToString();
                            //tempReceiptModel.County = dr["Country"].ToString();
                            tempReceiptModel.FreightForwarderName = dr["FreightForwarder"].ToString();
                            tempReceiptModel.FFRef_No = dr["FFRef_No"].ToString();
                            tempReceiptModel.Currency = dr["Currency"].ToString();
                            // tempReceiptModel.PaymentTerm = dr["PTCode"].ToString();
                            //  tempReceiptModel.freightCharges = Convert.ToDouble(dr["freightCharges"].ToString());
                            tempReceiptModel.Status = dr["StatusShortCode"].ToString();
                            tempReceiptModel.CreatedBy = dr["CreatedBy"].ToString();
                            tempReceiptModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            tempReceiptModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            tempReceiptModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            LstReceiptModel.Add(tempReceiptModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstReceiptModel;
        }
        public List<ReceiptModel> GetPurchaseReceiptListForProcess(string User_Id)
        {
            List<ReceiptModel> LstReceiptModel = new List<ReceiptModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetProcessedPurchaseReceiptList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ReceiptModel tempReceiptModel = new ReceiptModel();
                            tempReceiptModel.ReceiptId = Convert.ToInt32(dr["ReceiptId"]);
                            tempReceiptModel.CompanyName = dr["CompanyName"].ToString();
                            tempReceiptModel.VendorName = dr["VendorName"].ToString();
                            //tempReceiptModel.County = dr["Country"].ToString();
                            tempReceiptModel.FreightForwarderName = dr["FreightForwarder"].ToString();
                            tempReceiptModel.FFRef_No = dr["FFRef_No"].ToString();
                            tempReceiptModel.Currency = dr["Currency"].ToString();
                            // tempReceiptModel.PaymentTerm = dr["PTCode"].ToString();
                            //tempReceiptModel.freightCharges = Convert.ToDouble(dr["freightCharges"].ToString());
                            tempReceiptModel.Status = dr["StatusShortCode"].ToString();
                            tempReceiptModel.CreatedBy = dr["CreatedBy"].ToString();
                            tempReceiptModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            tempReceiptModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            tempReceiptModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            LstReceiptModel.Add(tempReceiptModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstReceiptModel;
        }
        #endregion Review Purchase Receipt

        #region  Dispatch 
        public List<DispatchModel> GetDispatchList(string User_Id)
        {
            List<DispatchModel> LstDispatchModel = new List<DispatchModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetDispatchList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DispatchModel tempDispatchModel = new DispatchModel();
                            tempDispatchModel.DispatchId = Convert.ToInt32(dr["DispatchId"]);
                            tempDispatchModel.CompanyName = dr["CompanyName"].ToString();
                            tempDispatchModel.CustomerName = dr["CustomerName"].ToString();
                            tempDispatchModel.FreightForwarderName = dr["FreightForwarderName"].ToString();
                            if (dr["DispatchDate"].ToString() != "")
                                tempDispatchModel.DispatchDate = Convert.ToDateTime(dr["DispatchDate"].ToString());
                            tempDispatchModel.StatusId = Convert.ToInt32(dr["StatusId"]);
                            tempDispatchModel.Status = dr["StatusShortCode"].ToString();
                            tempDispatchModel.StatusDesc = dr["StatusDesc"].ToString();
                            tempDispatchModel.AirwayBillNo = dr["AirwayBillNo"].ToString();
                            tempDispatchModel.Invoice_No = dr["Invoice_No"].ToString();
                            tempDispatchModel.ExportPermitNo = dr["ExportPermitNo"].ToString();
                            tempDispatchModel.FreightForwarderName = dr["FreightForwarderName"].ToString();
                            tempDispatchModel.CreatedBy = dr["CreatedBy"].ToString();
                            tempDispatchModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            tempDispatchModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            tempDispatchModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            LstDispatchModel.Add(tempDispatchModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstDispatchModel;
        }
        public int UpdateDispatchApproval(int Status, string User_Id, int DispatchId)
        {
            EnquiryDAL OBJDAL = new EnquiryDAL();

            return OBJDAL.UpdateDispatchApproval(Status, User_Id, DispatchId);
        }
        public List<DispatchModel> GetDispatchListForApproval(string User_Id)
        {
            List<DispatchModel> LstDispatchModel = new List<DispatchModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetDispatchList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (dr["StatusShortCode"].ToString() == "DispSndPro")
                            {
                                DispatchModel tempDispatchModel = new DispatchModel();
                                tempDispatchModel.DispatchId = Convert.ToInt32(dr["DispatchId"]);
                                tempDispatchModel.CompanyName = dr["CompanyName"].ToString();
                                tempDispatchModel.CustomerName = dr["CustomerName"].ToString();
                                // tempDispatchModel.STLocationName = dr["STLocationName"].ToString();
                                tempDispatchModel.Invoice_No = dr["Invoice_No"].ToString();
                                tempDispatchModel.AirwayBillNo = dr["AirwayBillNo"].ToString();
                                tempDispatchModel.ExportPermitNo = dr["ExportPermitNo"].ToString();
                                tempDispatchModel.FreightForwarderId = Convert.ToInt32(dr["FreightForwarder"].ToString());
                                tempDispatchModel.FreightForwarder = dr["FreightForwarderName"].ToString();
                                tempDispatchModel.StatusId = Convert.ToInt32(dr["StatusId"]);
                                if (dr["DispatchDate"].ToString() != "")
                                    tempDispatchModel.DispatchDate = Convert.ToDateTime(dr["DispatchDate"].ToString());
                                tempDispatchModel.Status = dr["StatusDesc"].ToString();
                                tempDispatchModel.CreatedBy = dr["CreatedBy"].ToString();
                                tempDispatchModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                tempDispatchModel.ModifiedBy = dr["ModifiedBy"].ToString();
                                tempDispatchModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                LstDispatchModel.Add(tempDispatchModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstDispatchModel;
        }
        public DispatchModel GetSelectedDispatch(int DispatchId)
        {
            DispatchModel DispatchModel = new DispatchModel();
            DispatchModel.LstDispatchDetail = new List<DispatchDetailModel>();
            DispatchModel.LstDispatchCartonDetail = new List<DispatchCartonDetailModel>();
            DispatchModel.LstDispatchDoc = new List<DispatchDocument>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedDispatch(DispatchId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DispatchModel.DispatchId = Convert.ToInt32(ds.Tables[0].Rows[0]["DispatchId"]);
                        DispatchModel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                        DispatchModel.Invoice_No = ds.Tables[0].Rows[0]["Invoice_No"].ToString();
                        DispatchModel.CompanyName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                        DispatchModel.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString());
                        DispatchModel.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                        DispatchModel.STLocationId = Convert.ToInt32(ds.Tables[0].Rows[0]["STLocationId"].ToString());
                        DispatchModel.STLocationName = ds.Tables[0].Rows[0]["LocationName"].ToString();
                        DispatchModel.FreightForwarderId = Convert.ToInt32(ds.Tables[0].Rows[0]["FreightForwarder"].ToString());
                        DispatchModel.FreightForwarder = ds.Tables[0].Rows[0]["FreightForwarderName"].ToString();
                        DispatchModel.AirwayBillNo = ds.Tables[0].Rows[0]["AirwayBillNo"].ToString();
                        DispatchModel.ExportPermitNo = ds.Tables[0].Rows[0]["ExportPermitNo"].ToString();
                        DispatchModel.Status = ds.Tables[0].Rows[0]["StatusShortCode"].ToString();
                        DispatchModel.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"].ToString());
                        if (ds.Tables[0].Rows[0]["DispatchDate"].ToString() != "")
                            DispatchModel.DispatchDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["DispatchDate"].ToString());
                        DispatchModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        DispatchModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        DispatchModel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        DispatchModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());

                        if (ds.Tables.Count > 1)
                        {
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                foreach (DataRow dr in ds.Tables[1].Rows)
                                {
                                    DispatchDetailModel DispatchDetailModel = new DispatchDetailModel();
                                    DispatchDetailModel.DispatchDetailId = Convert.ToInt32(dr["DispatchDetailId"]);
                                    DispatchDetailModel.CartonId = Convert.ToInt32(dr["CartonId"]);
                                    DispatchDetailModel.DispatchId = Convert.ToInt32(dr["DispatchId"]);
                                    DispatchDetailModel.Dimension = dr["Dimension"].ToString();
                                    DispatchDetailModel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                                    DispatchDetailModel.ItemName = dr["ItemName"].ToString();
                                    DispatchDetailModel.MPN = dr["MPN"].ToString();
                                    DispatchDetailModel.Quantity = Convert.ToDouble(dr["Quantity"].ToString());
                                    // DispatchDetailModel.Invoice_No = dr["Invoice_No"].ToString();
                                    DispatchDetailModel.Rate = Convert.ToDouble(dr["Rate"].ToString());
                                    DispatchDetailModel.Amount = DispatchDetailModel.Quantity * DispatchDetailModel.Rate;
                                    DispatchDetailModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                    DispatchDetailModel.CreatedDateStr = DispatchDetailModel.CreatedDate.ToShortDateString();
                                    DispatchDetailModel.CreatedBy = dr["CreatedBy"].ToString();
                                    DispatchModel.LstDispatchDetail.Add(DispatchDetailModel);
                                }
                            }
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                foreach (DataRow dr in ds.Tables[2].Rows)
                                {
                                    DispatchCartonDetailModel DispatchCrtDetail = new DispatchCartonDetailModel();
                                    DispatchCrtDetail.CartonId = Convert.ToInt32(dr["CartonId"]);
                                    DispatchCrtDetail.DispatchId = Convert.ToInt32(dr["DispatchId"]);
                                    DispatchCrtDetail.Weight = Convert.ToDouble(dr["Weight"]);
                                    DispatchCrtDetail.Length = Convert.ToDouble(dr["Length"]);
                                    DispatchCrtDetail.Width = Convert.ToDouble(dr["Width"]);
                                    DispatchCrtDetail.Height = Convert.ToDouble(dr["Height"]);
                                    DispatchCrtDetail.Remark = dr["Remark"].ToString();
                                    DispatchModel.LstDispatchCartonDetail.Add(DispatchCrtDetail);
                                }
                            }
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                foreach (DataRow dr in ds.Tables[3].Rows)
                                {
                                    DispatchDocument DispatchDoc = new DispatchDocument();
                                    DispatchDoc.DocumentTitle = dr["DocName"].ToString();
                                    DispatchDoc.FilePath = dr["FilePath"].ToString();
                                    DispatchDoc.DispatchId = Convert.ToInt32(dr["DispatchId"]);
                                    DispatchDoc.CreatedBy = Convert.ToString(dr["CreatedBy"]);
                                    DispatchDoc.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                                    DispatchModel.LstDispatchDoc.Add(DispatchDoc);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DispatchModel;
        }
        public List<CustomerDrpModel> CustomerListByCompCode(string CompCode)
        {
            List<CustomerDrpModel> LstVendorModel = new List<CustomerDrpModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.CustomerListByCompCode(CompCode);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CustomerDrpModel VendorModel = new CustomerDrpModel();
                            VendorModel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            VendorModel.Customer_Id = dr["Customer_No"].ToString();
                            VendorModel.CustomerName = dr["CustomerName"].ToString();
                            LstVendorModel.Add(VendorModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstVendorModel;
        }
        public List<InvoiceModel> InvoiceListByCustomerId(string CompCode, int CustomerId)
        {
            List<InvoiceModel> LstInvoiceModel = new List<InvoiceModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.InvoiceListByCustomerId(CompCode, CustomerId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            InvoiceModel InvoiceModel = new InvoiceModel();
                            InvoiceModel.Invoice_Id = dr["Document No_"].ToString();
                            InvoiceModel.InvoiceName = dr["Document No_"].ToString();
                            LstInvoiceModel.Add(InvoiceModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstInvoiceModel;
        }
        public List<InvoiceItemModel> GetItemListBySalInvoiceId(string CompCode, string Invoice_Id)
        {
            List<InvoiceItemModel> LstInvItemModel = new List<InvoiceItemModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetItemListBySalInvoiceId(CompCode, Invoice_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            InvoiceItemModel InvoiceItemModel = new InvoiceItemModel();
                            InvoiceItemModel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            InvoiceItemModel.Item_Id = dr["Item_No"].ToString();
                            InvoiceItemModel.ItemName = dr["ItemName"].ToString();
                            LstInvItemModel.Add(InvoiceItemModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstInvItemModel;
        }
        public DispatchDetailModel GetItemRateByInvoiceId(string CompCode, string Invoice_Id, int ItemId, int CustomerId)
        {
            DispatchDetailModel DispatchdetailModel = new DispatchDetailModel();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetItemRateByInvoiceId(CompCode, Invoice_Id, ItemId, CustomerId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DispatchdetailModel.Rate = Convert.ToDouble(ds.Tables[0].Rows[0]["Amount"].ToString());
                        DispatchdetailModel.Quantity = Convert.ToDouble(ds.Tables[0].Rows[0]["Quantity"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DispatchdetailModel;
        }
        public int SaveDispatch(DispatchModel DispatchModel, string User_Id)
        {
            int errCode = 0;
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedDispatch(0);
                ds.Tables[1].Rows.Clear();
                DataRow Dr = ds.Tables[0].NewRow();
                Dr["DispatchId"] = DispatchModel.DispatchId;
                Dr["ExportPermitNo"] = DispatchModel.ExportPermitNo;
                Dr["AirwayBillNo"] = DispatchModel.AirwayBillNo;
                Dr["CompCode"] = DispatchModel.CompCode;
                Dr["CustomerId"] = DispatchModel.CustomerId;
                // Dr["COO"] = DispatchModel.COO;
                Dr["FreightForwarder"] = DispatchModel.FreightForwarderId;
                Dr["Invoice_No"] = DispatchModel.Invoice_No;
                Dr["DispatchDate"] = DispatchModel.DispatchDate;
                ds.Tables[0].Rows.Add(Dr);
                errCode = ObjDAL.SaveDispatch(ds, User_Id, DispatchModel.DispatchId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        public int DispatchSendForProcess(int DispatchId, int StatusId, string user_Id)
        {
            int errcode = 0;
            EnquiryDAL ObjDAL = new EnquiryDAL();
            try
            {
                errcode = ObjDAL.DispatchSendForProcess(DispatchId, StatusId, user_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }
        public DispatchModel SaveDispatchDetail(DispatchDetailModel DispatchDetailModel, string User_Id, int DispatchId)
        {
            DispatchModel DispatchModel = new DispatchModel();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedDispatch(0);
                ds.Tables[1].Rows.Clear();

                DataRow Dr1 = ds.Tables[1].NewRow();
                Dr1["DispatchDetailId"] = DispatchDetailModel.DispatchDetailId;
                Dr1["CartonId"] = DispatchDetailModel.CartonId;
                Dr1["DispatchId"] = DispatchDetailModel.DispatchId;
                Dr1["ItemId"] = DispatchDetailModel.ItemId;
                Dr1["Quantity"] = DispatchDetailModel.Quantity;
                Dr1["Rate"] = DispatchDetailModel.Rate;
                ds.Tables[1].Rows.Add(Dr1);
                int errCode = ObjDAL.SaveDispatchDetail(ds, User_Id, DispatchId);
                DispatchModel = GetSelectedDispatch(DispatchId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DispatchModel;
        }
        public List<CustomerDrpModel> CustomerListByDispatchCompCode(string CompCode)
        {
            List<CustomerDrpModel> LstVendorModel = new List<CustomerDrpModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.CustomerListByDispatchCompCode(CompCode);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CustomerDrpModel VendorModel = new CustomerDrpModel();
                            VendorModel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            VendorModel.CustomerName = dr["CustomerName"].ToString();
                            LstVendorModel.Add(VendorModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstVendorModel;
        }
        public List<InvoiceModel> InvoiceListByDispatchCustomerId(string CompCode, int CustomerId)
        {
            List<InvoiceModel> LstInvoiceModel = new List<InvoiceModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.InvoiceListByDispatchCustomerId(CompCode, CustomerId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            InvoiceModel InvoiceModel = new InvoiceModel();
                            InvoiceModel.Invoice_Id = dr["InvRefNo"].ToString();
                            InvoiceModel.InvoiceName = dr["InvRefNo"].ToString();
                            LstInvoiceModel.Add(InvoiceModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstInvoiceModel;
        }
        public int DeleteDispatch(int DispatchId)
        {
            int ErrCode = 0;
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                ErrCode = DALObj.DeleteDispatch(DispatchId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public int DeleteDispatchDetail(int DispatchDetailId)
        {
            int ErrCode = 0;
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                ErrCode = DALObj.DeleteDispatchDetail(DispatchDetailId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        #region Dispatch  Carton
        public List<CartonModel> GetSelectedDispatchCartonList(int DispatchId)
        {
            List<CartonModel> LstCarronModel = new List<CartonModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedDispatchCartonList(DispatchId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CartonModel CartonModel = new CartonModel();
                            CartonModel.DispatchId = Convert.ToInt32(dr["DispatchId"].ToString());
                            CartonModel.CartonId = Convert.ToInt32(dr["CartonId"].ToString());
                            CartonModel.ReceiptId = Convert.ToInt32(dr["DispatchId"].ToString());
                            CartonModel.Weight = Convert.ToDouble(dr["Weight"].ToString());
                            CartonModel.Height = Convert.ToDouble(dr["Height"].ToString());
                            CartonModel.Width = Convert.ToDouble(dr["Width"].ToString());
                            CartonModel.Length = Convert.ToDouble(dr["Length"].ToString());
                            CartonModel.Remark = dr["Remark"].ToString();
                            CartonModel.Dimension = dr["Dimension"].ToString();
                            CartonModel.AvlBit = Convert.ToInt32(dr["AvlBit"].ToString());
                            LstCarronModel.Add(CartonModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstCarronModel;
        }

        public int SaveDispatchCarton(int DispatchId, int CartonId, double Weight, string Remark)
        {
            int ErrCode = 0;
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                ErrCode = ObjDAL.SaveDispatchCarton(DispatchId, CartonId, Weight, Remark);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ErrCode;
        }
        public int DeleteDispatchCart(int DispatchId, int CartonId)
        {
            int ErrCode = 0;
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                ErrCode = ObjDAL.DeleteDispatchCart(DispatchId, CartonId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ErrCode;
        }

        public int SaveReceiptCarton(int ReceiptId, int CartonId, double Weight, string Remark)
        {
            int ErrCode = 0;
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                ErrCode = ObjDAL.SaveReceiptCarton(ReceiptId, CartonId, Weight, Remark);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ErrCode;
        }
        public List<CartonModel> DrpDispatchCartonList(int DispatchId)
        {
            List<CartonModel> LstCarronModel = new List<CartonModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedDispatchCartonList(DispatchId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CartonModel CartonModel = new CartonModel();
                            CartonModel.CartonId = Convert.ToInt32(dr["CartonId"].ToString());
                            CartonModel.Dimension = dr["Dimension"].ToString();
                            LstCarronModel.Add(CartonModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstCarronModel;
        }
        #endregion Dispatch Carton

        #region Dispatch Doc
        public int SaveDispatchDoc(int DispatchId, string DocName, string FilePath, string User_Id)
        {
            int ErrCode = 0;
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                ErrCode = DALObj.SaveDispatchDoc(DispatchId, DocName, FilePath, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        #endregion Dispatch Doc

        #endregion Dispatch
        public List<DrpItem> VendorListHaveContact(string User_Id)
        {
            List<DrpItem> LstVend = new List<DrpItem>();
            try
            {
                VendorDAL VemdDal = new VendorDAL();
                DataSet ds = new DataSet();
                ds = VemdDal.VendorListHavingVendorContacts(0, User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DrpItem VendModel = new DrpItem();
                            VendModel.Id = Convert.ToInt32(dr["VendorId"].ToString());
                            VendModel.DisplayName = dr["VendorName"].ToString();
                            LstVend.Add(VendModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstVend;
        }

        #region FFCharges
        public List<ReceiptModel> GetFFChargesList(string User_Id)
        {
            List<ReceiptModel> LstReceiptModel = new List<ReceiptModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetFFChargesList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ReceiptModel tempReceiptModel = new ReceiptModel();

                            tempReceiptModel.FreightInvNo = dr["FreightInvNo"].ToString();
                            tempReceiptModel.FreightInvDate = Convert.ToDateTime(dr["FreightInvDate"].ToString());
                            tempReceiptModel.FrieghtInvReceivedDate = Convert.ToDateTime(dr["FrieghtInvReceivedDate"].ToString());
                            tempReceiptModel.Amount = Convert.ToDouble(dr["Amount"].ToString());
                            tempReceiptModel.ReceiptId = Convert.ToInt32(dr["ReceiptId"]);
                            LstReceiptModel.Add(tempReceiptModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstReceiptModel;
        }
        public List<AirwaysReceiptModel> GetAirwaysListForDrp(int FFId)
        {
            List<AirwaysReceiptModel> lst = new List<AirwaysReceiptModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetAirwaysReceiptList(FFId);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        AirwaysReceiptModel model = new AirwaysReceiptModel();
                        model.ReceiptId = Convert.ToInt32(dr["ReceiptId"].ToString());
                        model.FFRef_No = dr["FFRef_No"].ToString();
                        lst.Add(model);

                    }
                }
            }
            catch (Exception Ex)
            {
                Common.LogException(Ex);
            }
            return lst;
        }
        public int SaveFFCharges(ReceiptModel model,string User_Id)
        {
            DataSet ds = new DataSet();
            EnquiryDAL ObjDAL = new EnquiryDAL();
            ds = ObjDAL.GetSelectedFFCharges(0);
            try
            {
                ds.Tables[0].Rows.Clear();
                DataRow Dr = ds.Tables[0].NewRow();
                Dr["FreightInvNo"] = model.FreightInvNo;
                Dr["ReceiptId"] = model.ReceiptId;
                Dr["FreightInvDate"] = model.FreightInvDate;
                Dr["Amount"] = model.Amount;
                Dr["FrieghtInvReceivedDate"] = model.FrieghtInvReceivedDate;
                ds.Tables[0].Rows.Add(Dr);
                return ObjDAL.SaveFFCharges(ds, User_Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ReceiptModel getSelectedFFCharges(int RecieptId)
        {
            ReceiptModel objmodel = new ReceiptModel();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedFFCharges(RecieptId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            objmodel.ReceiptId = Convert.ToInt32(ds.Tables[0].Rows[0]["ReceiptId"].ToString());
                            objmodel.FreightForwarderId = Convert.ToInt32(ds.Tables[0].Rows[0]["FreightForwarderId"].ToString());
                            objmodel.FFRef_No = ds.Tables[0].Rows[0]["FFRef_No"].ToString();

                            objmodel.Amount = Convert.ToDouble(ds.Tables[0].Rows[0]["Amount"].ToString());
                            objmodel.FreightInvNo = ds.Tables[0].Rows[0]["FreightInvNo"].ToString();
                            objmodel.FrieghtInvReceivedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["FrieghtInvReceivedDate"].ToString());
                            objmodel.FreightInvDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["FreightInvDate"].ToString());

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }
        #endregion FFCharges

        #region Status Config
        public List<TrackingStatusConfigModel> StatusConfigDetailList()
        {
            List<TrackingStatusConfigModel> LstCongidModel = new List<TrackingStatusConfigModel>();
            try
            {
                EnquiryDAL ConfigDal = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ConfigDal.StatusConfigDetailList();
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            TrackingStatusConfigModel Model = new TrackingStatusConfigModel();
                            Model.ID = Convert.ToInt32(dr["ID"].ToString());
                            Model.OutstandingResponseTime = Convert.ToInt32(dr["OutstandingResponseTime"].ToString());
                            Model.MaximumTime = Convert.ToInt32(dr["MaximumTime"].ToString());
                            Model.MinimumTime = Convert.ToInt32(dr["MinimumTime"].ToString());
                            Model.StatusDesc = dr["StatusDesc"].ToString();
                            Model.Meaning = dr["Meaning"].ToString();
                            Model.ModifiedBy = dr["ModifiedBy"].ToString();
                            Model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            LstCongidModel.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstCongidModel;
        }

        public int UpdateStatusConfig(TrackingStatusConfigModel Model, string User_Id)
        {
            int errCode = 0;
            try
            {
                EnquiryDAL DalObj = new EnquiryDAL();
                errCode = DalObj.UpdateStatusConfigDetail(Model.ID, Model.OutstandingResponseTime, Model.MinimumTime, Model.MaximumTime, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion Status Config

        #region InvoiceDetail
        public List<InvoiceDetailModel> GetInvoicedetail(string User_Id)
        {
            List<InvoiceDetailModel> InvList = new List<InvoiceDetailModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetInvoiceList(User_Id);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        InvoiceDetailModel model = new InvoiceDetailModel();
                        model.InvId = Convert.ToInt32(dr["InvId"].ToString());
                        model.CompCode = Convert.ToString(dr["CompCode"].ToString());
                        model.InvRefNo = Convert.ToString(dr["InvRefNo"].ToString());
                        model.CustomerName = Convert.ToString(dr["CustomerName"].ToString());
                        model.InvFile = Convert.ToString(dr["InvFile"].ToString());
                        model.Status = Convert.ToString(dr["Status"].ToString());
                        model.CreatedBy = Convert.ToString(dr["CreatedBy"].ToString());
                        model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        model.ModifiedBy = Convert.ToString(dr["ModifiedBy"].ToString());
                        model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        InvList.Add(model);

                    }
                }
            }
            catch (Exception Ex)
            {
                Common.LogException(Ex);
            }
            return InvList;
        }

        public int SaveInvoiceDetail(InvoiceDetailModel model, string User_Id)
        {
            DataSet ds = new DataSet();
            EnquiryDAL ObjDAL = new EnquiryDAL();
            ds = ObjDAL.GetSelectedInvoiceDetails(0);
            try
            {
                ds.Tables[0].Rows.Clear();
                DataRow Dr = ds.Tables[0].NewRow();
                Dr["InvId"] = model.InvId;
                Dr["InvRefNo"] = model.InvRefNo;
                Dr["InvFile"] = model.InvFile;
                Dr["CompCode"] = model.CompCode;
                Dr["CustomerId"] = model.CustomerId;
                ds.Tables[0].Rows.Add(Dr);


            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ObjDAL.SaveInvoiceDetail(ds, User_Id);
        }

        public InvoiceDetailModel GetselectedInvoiceDetail(int InvId)
        {
            InvoiceDetailModel objmodel = new InvoiceDetailModel();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedInvoiceDetails(InvId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            objmodel.InvId = Convert.ToInt32(ds.Tables[0].Rows[0]["InvId"].ToString());
                            objmodel.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString());
                            objmodel.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                            objmodel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                            objmodel.InvRefNo = ds.Tables[0].Rows[0]["InvRefNo"].ToString();
                            objmodel.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                            objmodel.InvFile = ds.Tables[0].Rows[0]["InvFile"].ToString();
                            objmodel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            objmodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            objmodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            objmodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }
        #endregion  InvoiceDetail

        #region Carton
        public List<CartonModel> CartonList()
        {
            List<CartonModel> CartonList = new List<CartonModel>();
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = DALObj.CartonList();
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            CartonModel CartonModel = new CartonModel();
                            CartonModel.Id = Convert.ToInt32(dr["Id"].ToString());
                            CartonModel.Height = Convert.ToDouble(dr["Height"].ToString());
                            CartonModel.Width = Convert.ToDouble(dr["Width"].ToString());
                            CartonModel.Length = Convert.ToDouble(dr["Length"].ToString());
                            CartonModel.Dimension = "H: " + dr["Height"].ToString() + " W:" + dr["Width"].ToString() + " L:" + dr["Length"].ToString();
                            CartonModel.CreatedBy = dr["CreatedBy"].ToString();
                            CartonModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            CartonModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            CartonModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            CartonList.Add(CartonModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return CartonList;
        }
        public int SaveCarton(CartonModel Model, string User_Id)
        {
            int ErrCod = 0;
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                ErrCod = DALObj.DWSaveCarton(Model.Id, Model.Height, Model.Width, Model.Length, User_Id);
            }
            return ErrCod;
        }
        #endregion Carton

        #region Freight Forworder  Invoice
        public List<FreightFwdInvoiceModel> GetFreightFwdInvoiceList(string User_Id)
        {
            List<FreightFwdInvoiceModel> FFInvoiceLstModel = new List<FreightFwdInvoiceModel>();
            try
            {

                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetFreightFwdInvoiceList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            FreightFwdInvoiceModel objmodel = new FreightFwdInvoiceModel();
                            objmodel.FreightInvId = Convert.ToInt32(dr["FreightInvId"].ToString());
                            objmodel.InvoiceNumber = dr["InvoiceNumber"].ToString();
                            objmodel.InvoiceDate = Convert.ToDateTime(dr["InvoiceDate"].ToString());
                            objmodel.VendorName = dr["VendorName"].ToString();
                            objmodel.CreatedBy = dr["CreatedBy"].ToString();
                            objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            FFInvoiceLstModel.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return FFInvoiceLstModel;
        }

        public List<FreightFwdReceiptDrpModel> ReceiptDrpDwn(int FFId)
        {
            List<FreightFwdReceiptDrpModel> LstModel = new List<FreightFwdReceiptDrpModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetReceiptDrpList(FFId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            FreightFwdReceiptDrpModel objmodel = new FreightFwdReceiptDrpModel();
                            objmodel.ReceiptName = dr["FFRef_No"].ToString();
                            LstModel.Add(objmodel);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstModel;
        }

        public FreightFwdInvoiceModel GetSelectedFFInvoice(int FreightInvId)
        {
            FreightFwdInvoiceModel FFInvoiceModel = new FreightFwdInvoiceModel();
            FFInvoiceModel.LstDetal = new List<FreightFwdInvoiceDetailModel>();
            FFInvoiceModel.LstTax = new List<FreightFwdInvoiceTaxesModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedFreightFwdInvoiceList(FreightInvId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FFInvoiceModel.FreightInvId = Convert.ToInt32(ds.Tables[0].Rows[0]["FreightInvId"]);
                        if (ds.Tables[0].Rows[0]["InvoiceDate"].ToString() != "")
                            FFInvoiceModel.InvoiceDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["InvoiceDate"].ToString());
                        FFInvoiceModel.InvoiceNumber = ds.Tables[0].Rows[0]["InvoiceNumber"].ToString();
                        FFInvoiceModel.VendorId = Convert.ToInt32(ds.Tables[0].Rows[0]["VendorId"].ToString());
                        FFInvoiceModel.VendorName = ds.Tables[0].Rows[0]["VendorName"].ToString();
                        FFInvoiceModel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                        FFInvoiceModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        FFInvoiceModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        FFInvoiceModel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        FFInvoiceModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());

                        if (ds.Tables.Count > 1)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                FreightFwdInvoiceDetailModel DetailModel = new FreightFwdInvoiceDetailModel();
                                DetailModel.FreightInvDetailId = Convert.ToInt32(dr["FreightFwdDetailId"]);
                                DetailModel.FreightInvId = Convert.ToInt32(dr["FreightInvId"]);
                                DetailModel.RefDate = Convert.ToDateTime(dr["RefDate"].ToString());
                                DetailModel.RefDateStr = DetailModel.RefDate.ToShortDateString();
                                DetailModel.FFRef_No = dr["FFRef_No"].ToString();
                                DetailModel.Amount = Convert.ToDouble(dr["Amount"].ToString());
                                FFInvoiceModel.LstDetal.Add(DetailModel);
                            }
                        }
                        if (ds.Tables.Count > 2)
                        {
                            foreach (DataRow dr in ds.Tables[2].Rows)
                            {
                                FreightFwdInvoiceTaxesModel TaxModel = new FreightFwdInvoiceTaxesModel();
                                TaxModel.FreightInvDetailId = Convert.ToInt32(dr["FreightFwdDetailId"]);
                                TaxModel.TaxId = Convert.ToInt32(dr["TaxId"].ToString());
                                TaxModel.TaxName = dr["TaxName"].ToString();
                                TaxModel.Amount = Convert.ToDouble(dr["Amount"].ToString());
                                FFInvoiceModel.LstTax.Add(TaxModel);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return FFInvoiceModel;
        }
        public int SaveFFInvoice(FreightFwdInvoiceModel FFInvoiceModel, string User_Id)
        {
            int errCode = 0;
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedFreightFwdInvoiceList(0);
                ds.Tables[0].Rows.Clear();
                DataRow Dr1 = ds.Tables[0].NewRow();
                Dr1["FreightInvId"] = FFInvoiceModel.FreightInvId;
                Dr1["InvoiceDate"] = FFInvoiceModel.InvoiceDate;
                Dr1["InvoiceNumber"] = FFInvoiceModel.InvoiceNumber;
                Dr1["VendorId"] = FFInvoiceModel.VendorId;
                Dr1["Remark"] = FFInvoiceModel.Remark;
                ds.Tables[0].Rows.Add(Dr1);
                errCode = ObjDAL.SaveFreightFwdInvoice(ds, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        public FreightFwdInvoiceModel SaveFFInvoiceDetail(FreightFwdInvoiceModel FFInvoiceModel, string User_Id, int FreightInvDetailId, int FreightInvId)
        {
            int errCode = 0;
            FreightFwdInvoiceModel FFModel = new FreightFwdInvoiceModel();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedFreightFwdInvoiceList(0);
                ds.Tables[1].Rows.Clear();
                foreach (FreightFwdInvoiceDetailModel DetailModel in FFInvoiceModel.LstDetal.Where(s => (s.FreightInvDetailId == FreightInvDetailId)))
                {
                    DataRow Dr1 = ds.Tables[1].NewRow();
                    Dr1["FreightFwdDetailId"] = DetailModel.FreightInvDetailId;
                    Dr1["FFRef_No"] = DetailModel.FFRef_No;
                    Dr1["RefDate"] = DetailModel.RefDate;
                    Dr1["Amount"] = DetailModel.Amount;
                    Dr1["FreightInvId"] = DetailModel.FreightInvId;
                    ds.Tables[1].Rows.Add(Dr1);
                }
                ds.Tables[2].Rows.Clear();
                foreach (FreightFwdInvoiceTaxesModel TaxModel in FFInvoiceModel.LstTax.Where(s => (s.FreightInvDetailId == FreightInvDetailId)))
                {
                    DataRow Dr1 = ds.Tables[2].NewRow();
                    Dr1["FreightFwdDetailId"] = TaxModel.FreightInvDetailId;
                    Dr1["TaxId"] = TaxModel.TaxId;
                    Dr1["Amount"] = TaxModel.Amount;
                    ds.Tables[2].Rows.Add(Dr1);
                }
                errCode = ObjDAL.SaveFreightFwdInvoiceDetail(ds, User_Id, FreightInvId, FreightInvDetailId);
                if (errCode > 0)
                {
                    FFModel = GetSelectedFFInvoice(FreightInvId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FFModel;
        }
        public FreightFwdInvoiceModel DeleteFFDetail(int FreightInvDetailId, int FreightInvId)
        {
            int errCode = 0;
            FreightFwdInvoiceModel FFModel = new FreightFwdInvoiceModel();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                errCode = ObjDAL.DeleteFFDetail(FreightInvDetailId);
                if (errCode == 600001)
                {
                    FFModel = GetSelectedFFInvoice(FreightInvId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FFModel;
        }
        #endregion Freight Forworder  Invoice

        #region Enq Reason
        public List<ReasonModel> GetReasonList(string User_Id)
        {
            List<ReasonModel> ReasonList = new List<ReasonModel>();
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = DALObj.GetReasonList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ReasonModel ReasonModel = new ReasonModel();
                            ReasonModel.ReasonId = Convert.ToInt32(dr["ReasonId"].ToString());
                            ReasonModel.Reason = dr["Reason"].ToString();
                            ReasonModel.CreatedBy = dr["CreatedBy"].ToString();
                            ReasonModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            ReasonModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            ReasonModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            ReasonList.Add(ReasonModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ReasonList;
        }
        public ReasonModel GetSelectedReasonList(int ReasonId)
        {
            ReasonModel ReasonModel = new ReasonModel();
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = DALObj.GetSelectedReasonList(ReasonId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ReasonModel.ReasonId = Convert.ToInt32(ds.Tables[0].Rows[0]["ReasonId"].ToString());
                        ReasonModel.Reason = ds.Tables[0].Rows[0]["Reason"].ToString();
                        ReasonModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        ReasonModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        ReasonModel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        ReasonModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ReasonModel;
        }
        public int SaveReasonDetail(int ReasonId, string Reason, string User_Id)
        {
            int ErrCod = 0;
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                ErrCod = DALObj.DWSaveReasonDetail(ReasonId, Reason, User_Id);
            }
            return ErrCod;
        }
        #endregion Enq Reason

        #region Enq Reason
        public List<FollowupTypeModel> GetFollowupTypeList(string User_Id)
        {
            List<FollowupTypeModel> ReasonList = new List<FollowupTypeModel>();
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = DALObj.GetFollowupTypeList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            FollowupTypeModel ReasonModel = new FollowupTypeModel();
                            ReasonModel.FollowupTypeId = Convert.ToInt32(dr["FollowupTypeId"].ToString());
                            ReasonModel.FollowupReason = dr["FollowupReason"].ToString();
                            ReasonModel.CreatedBy = dr["CreatedBy"].ToString();
                            ReasonModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            ReasonModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            ReasonModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            ReasonList.Add(ReasonModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ReasonList;
        }
        public FollowupTypeModel GetSelectedFollowupTypeList(int ReasonId)
        {
            FollowupTypeModel FollowUpTypeModel = new FollowupTypeModel();
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = DALObj.GetSelectedFollowupTypeList(ReasonId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FollowUpTypeModel.FollowupTypeId = Convert.ToInt32(ds.Tables[0].Rows[0]["FollowupTypeId"].ToString());
                        FollowUpTypeModel.FollowupReason = ds.Tables[0].Rows[0]["FollowupReason"].ToString();
                        FollowUpTypeModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        FollowUpTypeModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        FollowUpTypeModel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        FollowUpTypeModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return FollowUpTypeModel;
        }
        public int DWSaveFollowupTypeDetail(int FollowupTypeId, string FollowupReason, string User_Id)
        {
            int ErrCod = 0;
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                ErrCod = DALObj.DWSaveFollowupTypeDetail(FollowupTypeId, FollowupReason, User_Id);
            }
            return ErrCod;
        }
        #endregion Enq Reason

        #region Close Enquiry
        public int SaveCloseEnquiry(int EnqId, int ReasonId, string Remark, string User_Id)
        {
            int ErrCod = 0;
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                ErrCod = DALObj.SaveCloseEnquiry(EnqId, ReasonId, Remark, User_Id);
            }
            return ErrCod;
        }
        public int UpdateCustEnqStatusAfterMailFail(int EnqId, int Status, string User_Id, string Remark)
        {
            int ErrCod = 0;
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                ErrCod = DALObj.UpdateCustEnqStatusAfterMailFail(EnqId, Status, User_Id, Remark);
            }
            return ErrCod;
        }
        #endregion Close Enquiry
        public ReportNameModel GetCustEnquiryVendResponseReport(string user_Id)
        {
            ReportNameModel Model = new ReportNameModel();
            try
            {
                EnquiryDAL objDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetCustEnquiryVendResponseReport(user_Id);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Model.OutputLocation = ds.Tables[0].Rows[0]["OutputLocation"].ToString();
                        Model.RunDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["RunDate"].ToString());
                        Model.ReportId = ds.Tables[0].Rows[0]["ReportId"].ToString();
                        Model.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"].ToString());
                        Model.TxtParamValue = ds.Tables[0].Rows[0]["TxtParamValue"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Model;
        }

        #region Quotation Followup
        public List<QoutFollowupModel> GetQoutMYFollowUpList(string user_Id)
        {
            List<QoutFollowupModel> LstReminderModel = new List<QoutFollowupModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetQuotMyFollowupList(user_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            QoutFollowupModel tempReminderModel = new QoutFollowupModel();

                            tempReminderModel.QuotationId = Convert.ToInt32(dr["QuotationId"]);
                            tempReminderModel.QuotationNumber = dr["QuotationNumber"].ToString();
                            tempReminderModel.EnqId = Convert.ToInt32(dr["EnqId"]);
                            tempReminderModel.NextFollowUp = Convert.ToDateTime(dr["FollowupDate"].ToString());
                            tempReminderModel.CustomerName = dr["CustomerName"].ToString();
                            tempReminderModel.Term = dr["Term"].ToString();
                            tempReminderModel.CompCode = dr["CompCode"].ToString();
                            tempReminderModel.Currency = dr["Currancy"].ToString();
                            tempReminderModel.CreatedBy = dr["CreatedBy"].ToString();
                            tempReminderModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            LstReminderModel.Add(tempReminderModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstReminderModel;
        }
        public List<QoutFollowupModel> GetQoutSubordinateFollowUpList(string user_Id)
        {
            List<QoutFollowupModel> LstReminderModel = new List<QoutFollowupModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetQuotSubordinateFollowupList(user_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            QoutFollowupModel tempReminderModel = new QoutFollowupModel();

                            tempReminderModel.QuotationId = Convert.ToInt32(dr["QuotationId"]);
                            tempReminderModel.QuotationNumber = dr["QuotationNumber"].ToString();
                            tempReminderModel.EnqId = Convert.ToInt32(dr["EnqId"]);
                            tempReminderModel.NextFollowUp = Convert.ToDateTime(dr["FollowupDate"].ToString());
                            tempReminderModel.CustomerName = dr["CustomerName"].ToString();
                            tempReminderModel.Term = dr["Term"].ToString();
                            tempReminderModel.CompCode = dr["CompCode"].ToString();
                            tempReminderModel.Currency = dr["Currancy"].ToString();
                            tempReminderModel.CreatedBy = dr["CreatedBy"].ToString();
                            tempReminderModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            LstReminderModel.Add(tempReminderModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstReminderModel;
        }
        public List<QoutFollowupModel> GetQuotFollowupReminderList(string user_Id)
        {
            List<QoutFollowupModel> LstReminderModel = new List<QoutFollowupModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetQuotFollowupReminderList(user_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            QoutFollowupModel tempReminderModel = new QoutFollowupModel();

                            tempReminderModel.QuotationId = Convert.ToInt32(dr["QuotationId"]);
                            tempReminderModel.QuotationNumber = dr["QuotationNumber"].ToString();
                            tempReminderModel.SalesPersonName = dr["SalesPerson"].ToString();
                            tempReminderModel.QuotCost = Convert.ToDouble(dr["QuotCost"]);
                            tempReminderModel.Status = dr["Status"].ToString();
                            tempReminderModel.StatusId= Convert.ToInt32(dr["statusId"]);
                            tempReminderModel.EnqId = Convert.ToInt32(dr["EnqId"]);
                            tempReminderModel.NextFollowUp = Convert.ToDateTime(dr["FollowupDate"].ToString());
                            tempReminderModel.CustomerName = dr["CustomerName"].ToString();
                            tempReminderModel.SalesPersonName = dr["SalesPerson"].ToString();
                            tempReminderModel.Term = dr["Term"].ToString();
                            tempReminderModel.CompCode = dr["CompCode"].ToString();
                            tempReminderModel.Currency = dr["Currancy"].ToString();
                            tempReminderModel.CreatedBy = dr["CreatedBy"].ToString();
                            tempReminderModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            tempReminderModel.FileName = dr["FileName"].ToString();
                            LstReminderModel.Add(tempReminderModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstReminderModel;
        }
        public ReportNameModel GetOpenReportForQuotFolloupLst(string user_Id)
        {
            ReportNameModel Model = new ReportNameModel();
            try
            {
                EnquiryDAL objDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetQuotFollowupListInReport(user_Id);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Model.OutputLocation = ds.Tables[0].Rows[0]["OutputLocation"].ToString();
                            Model.RunDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["RunDate"].ToString());
                            Model.ReportId = ds.Tables[0].Rows[0]["ReportId"].ToString();
                            Model.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Model;
        }
        public int SaveFollowupDaetail(QoutFollowupModel model, string User_Id)
        {
            EnquiryDAL ObjDAL = new EnquiryDAL();
            int ErrCode = 0;
            try
            {
                ErrCode = ObjDAL.SaveFollowupDaetail(model.QuotationId, model.Followupdate, model.NextFollowUp, model.FollowupRemark, User_Id, model.FollowupReasonId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public int SaveNewOuatDetail(List<QoutFollowupHistoryModel> Lst, int QuotationId, string FollowupRes, string FollowupRemark, string User_Id, int FollowupReasonId)
        {
            EnquiryDAL ObjDAL = new EnquiryDAL();
            int ErrCode = 0;
            DataSet DSNewQuot = new DataSet();
            int NewStatus = 0;
            try
            {
                DataTable DtNewQuot = new DataTable("tbl_QuotItem");
                DtNewQuot.Columns.Add("QuotationId", typeof(System.Int32));
                DtNewQuot.Columns.Add("ItemId", typeof(System.Int32));
                DtNewQuot.Columns.Add("FollowupReason", typeof(System.String));
                DtNewQuot.Columns.Add("ItemName", typeof(System.String));
                DtNewQuot.Columns.Add("MPN", typeof(System.String));
                foreach (QoutFollowupHistoryModel data in Lst)
                {
                    DataRow deNewQuot = DtNewQuot.NewRow();
                    deNewQuot["QuotationId"] = data.QuotationId;
                    deNewQuot["ItemId"] = data.ItemId;
                    deNewQuot["FollowupReason"] = data.FollowupReason;
                    deNewQuot["ItemName"] = data.ItemName;
                    deNewQuot["MPN"] = data.MPN;
                    DtNewQuot.Rows.Add(deNewQuot);
                }
                DSNewQuot.Tables.Add(DtNewQuot);
                DataSet ds = new DataSet();
                if (FollowupReasonId == 2 || FollowupReasonId == 9 || FollowupReasonId == 12)
                {
                    switch (FollowupReasonId)
                    {
                        case 2:
                            NewStatus = 63;
                            break;
                        case 9:
                            NewStatus = 71;
                            break;
                        case 12:
                            NewStatus = 72;
                            break;
                    }
                    ds = ObjDAL.SaveNewOuatDetail(QuotationId, FollowupRes, FollowupRemark, User_Id, DSNewQuot, NewStatus);
                    ErrCode = Convert.ToInt32(ds.Tables[0].Rows[0]["QuotationId"].ToString());
                    if (ErrCode > 0)
                    {
                        string Subject = "New Quotation Alert by Quotation Follow Up";
                        string body = string.Empty;
                        body +=
                        "New Quotation Item  Detail:" +
                         "<br /> <br />";
                        body += "<table border='1'><tr style='background-color:#CEF6F5'><th style=width:80px>Item ID</th><th style=width:200px>Item Name</th><th style=width:150px>MPN</th><th style=width:200px>Reason</th></tr>";
                        foreach (DataRow dr in DSNewQuot.Tables[0].Rows)
                        {
                            body += "<tr style=width: 500px><td>" + dr["ItemId"].ToString() + "</td>";
                            body += "<td  width=80px>" + dr["ItemName"].ToString() + "</td>";
                            body += "<td>" + dr["MPN"].ToString() + "</td>";
                            body += "<td>" + FollowupRes + "</td></tr> ";
                        }
                        string MailBody = "<html>" +
                         "<head>" +
                          "<meta http-equiv= " + "Content-Type" + "content= " + "text/html; charset=UTF-8" + ">" +
                          "<meta name=" + "generator" + " content=" + "SautinSoft.RtfToHtml.dll" + ">" +
                          "<title>" + "Quotation Alert" + "</title>" +
                          "<style type=" + "text/css>" +
                         ".st1{font-family:Calibri;font-size:11pt;color:#000000;font-weight:bold}" +
                         ".st2{font-family:Calibri;font-size:11pt;color:#000000;}" +
                         "</style>" +
                          "</head>" +
                          "<body>" +
                          "<div>" +
                          "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Enquiry Id : " + "</b>" + ds.Tables[0].Rows[0]["EnqId"].ToString() + "</span>" + "</div>" +
                          "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Quotation NUmber : " + "</b>" + ds.Tables[0].Rows[0]["QuotationNumber"].ToString() + "</span>" + "</div>" +
                            "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Quotation Id : " + "</b>" + ds.Tables[0].Rows[0]["QuotationId"].ToString() + "</span>" + "</div>" +
                            "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " FollowupRemark : " + "</b>" + FollowupRemark + "</span>" + "</div>" +
                            "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Item Detail : " + "</b>" + body + "</span>" + "</div>" +
                           "</body>" +
                          "</html>";
                        for (int i = 0; i < 3; i++)
                        {
                            try
                            {
                               // int mail = Utility.Common.SendMail(Subject, MailBody, "", ds.Tables[0].Rows[0]["EmpMailId"].ToString(), ds.Tables[0].Rows[0]["ManagerMailId"].ToString(), null);
                                break;
                            }
                            catch (Exception ex)
                            {
                                if (i == 2)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                }
                else
                {
                    switch (FollowupReasonId)
                    {
                        case 1:
                            NewStatus = 29;
                            break;
                        case 3:
                            NewStatus = 64;
                            break;
                        case 4:
                            NewStatus = 61;
                            break;
                        case 7:
                            NewStatus = 69;
                            break;
                        case 8:
                            NewStatus = 66;
                            break;
                        case 10:
                            NewStatus = 67;
                            break;
                        case 11:
                            NewStatus = 68;
                            break;
                    }
                    ErrCode = ObjDAL.UpdateOuatItem(DSNewQuot, User_Id, NewStatus);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public List<QoutFollowupHistoryModel> GetQuotFollowupHistoryList(int QuotationId)
        {
            List<QoutFollowupHistoryModel> FollowupModelLst = new List<QoutFollowupHistoryModel>();
            try
            {
                EnquiryDAL objDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetQuotFollowupHistoryList(QuotationId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            QoutFollowupHistoryModel tempReminderModel = new QoutFollowupHistoryModel();

                            tempReminderModel.QuotationId = Convert.ToInt32(dr["QuotationId"]);
                            tempReminderModel.FollowupReason = dr["FollowupReason"].ToString();
                            tempReminderModel.QuotationNumber = dr["QuotationNumber"].ToString();
                            tempReminderModel.Followupdate = Convert.ToDateTime(dr["FollowupDate"].ToString());
                            tempReminderModel.NextFollowUp = Convert.ToDateTime(dr["NextFollowupDate"].ToString());
                            tempReminderModel.FollowupRemark = dr["Remark"].ToString();
                            tempReminderModel.CreatedBy = dr["CreatedBy"].ToString();
                            tempReminderModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            FollowupModelLst.Add(tempReminderModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return FollowupModelLst;
        }
        public List<QuotFollowupDetailModel> GetFollowupQuotDetailList(int QuotationId)
        {
            List<QuotFollowupDetailModel> DetailModelLst = new List<QuotFollowupDetailModel>();
            try
            {
                EnquiryDAL objDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetFollowupQuotDetail(QuotationId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            QuotFollowupDetailModel tempReminderModel = new QuotFollowupDetailModel();
                            tempReminderModel.QuotationId = QuotationId;
                            tempReminderModel.ItemId = Convert.ToInt32(dr["ItemId"]);
                            tempReminderModel.ItemName = dr["ItemName"].ToString();
                            tempReminderModel.MPN = dr["MPN"].ToString();
                            tempReminderModel.StatusId = Convert.ToInt32(dr["StatusId"]);
                            tempReminderModel.StatusCode = dr["StatusCode"].ToString();
                            tempReminderModel.SPQ = Convert.ToInt32(dr["SPQ"]);
                            tempReminderModel.MOQ = Convert.ToInt32(dr["MOQ"]);
                            tempReminderModel.Remark = dr["Remark"].ToString();
                            tempReminderModel.Rate = Convert.ToDouble(dr["CRate"]);
                            DetailModelLst.Add(tempReminderModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DetailModelLst;
        }
        #endregion Quotation Followup

        #region TO DO
        public List<AllPendingWorkModel> GetAllPendingWorkList(string user_Id, string DateType, string TabName)
        {
            List<AllPendingWorkModel> LstModel = new List<AllPendingWorkModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetAllPendingWorkList(user_Id, DateType, TabName);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            AllPendingWorkModel Model = new AllPendingWorkModel();

                            Model.Id = Convert.ToInt32(dr["Id"]);
                            Model.DocId = Convert.ToInt32(dr["DocId"]);
                            Model.DocType = dr["DocType"].ToString();
                            Model.Description = dr["Description"].ToString();
                            Model.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                            Model.DueDate = Convert.ToDateTime(dr["DueDate"].ToString());
                            Model.StrDueDate = Model.DueDate.ToShortDateString();
                            Model.StrStartDate = Model.StartDate.ToShortDateString();
                            Model.PlanId = Convert.ToInt32(dr["PlanId"]);
                            LstModel.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstModel;
        }
        public List<SubordinateListModel> GetSubOrdinateList(string user_Id)
        {
            List<SubordinateListModel> LstModel = new List<SubordinateListModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSubOrdinateList(user_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SubordinateListModel Model = new SubordinateListModel();
                            Model.User_Id = dr["USId"].ToString();
                            Model.EmployeeName = dr["EmpName"].ToString();
                            LstModel.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstModel;
        }
        public ArrayList GetTODOGroupList()
        {
            ArrayList GrpList = new ArrayList();
            EnquiryDAL ObjDAL = new EnquiryDAL();
            DataSet ds = new DataSet();
            ds = ObjDAL.GetTODOGroupList();
            int i = 0;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string[] str = dr["GroupName"].ToString().Split(' ');
                        if (str.Length > 1)
                            GrpList.Add(dr["GroupName"].ToString() + "_" + str[0].ToString() + str[1].ToString());
                        else
                            GrpList.Add(dr["GroupName"].ToString() + "_" + dr["GroupName"].ToString());
                    }
                }
            }
            return GrpList;
        }
        #endregion TO DO

        #region Preliminary Quotation
        public List<PreliminaryQuotationModel> GetPreliminaryQuotList(string UserId)
        {
            List<PreliminaryQuotationModel> PreQuotModelLst = new List<PreliminaryQuotationModel>();
            try
            {
                EnquiryDAL objDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetPreliminaryQuotList(UserId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PreliminaryQuotationModel PreQuotModel = new PreliminaryQuotationModel();
                            PreQuotModel.PreQuotId = Convert.ToInt32(dr["PreQuotId"]);
                            PreQuotModel.EnqId = Convert.ToInt32(dr["EnqId"]);
                            PreQuotModel.PreQuotNumber = dr["PreQuotNumber"].ToString();
                            PreQuotModel.QuotDate = Convert.ToDateTime(dr["QuotDate"]);
                            PreQuotModel.TermId = Convert.ToInt32(dr["TermId"]);
                            PreQuotModel.Terms = dr["Terms"].ToString();
                            PreQuotModel.CustomerName = dr["CustomerName"].ToString();
                            PreQuotModel.Currency = dr["Currency"].ToString();
                            PreQuotModel.CompCode = dr["CompCode"].ToString();
                            PreQuotModel.StatusId = Convert.ToInt32(dr["StatusId"]);
                            PreQuotModel.Status = dr["Status"].ToString();
                            PreQuotModel.FileName = dr["FileName"].ToString();
                            PreQuotModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            PreQuotModel.Remark = dr["Remark"].ToString();
                            PreQuotModelLst.Add(PreQuotModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PreQuotModelLst;
        }
        public PreliminaryQuotationModel GetSelectedPreliminaryQuotList(int PreQuotId)
        {
            PreliminaryQuotationModel PreQuotModel = new PreliminaryQuotationModel();
            PreQuotModel.LstDetail = new List<PreliminaryQuotDetailModel>();
            try
            {
                EnquiryDAL objDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetSelectedPreliminaryQuotList(PreQuotId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PreQuotModel.PreQuotId = Convert.ToInt32(dr["PreQuotId"]);
                            PreQuotModel.PreQuotNumber = dr["PreQuotNumber"].ToString();
                            PreQuotModel.QuotDate = Convert.ToDateTime(dr["QuotDate"]);
                            PreQuotModel.TermId = Convert.ToInt32(dr["TermId"]);
                            PreQuotModel.Terms = dr["Terms"].ToString();
                            PreQuotModel.CustomerName = dr["CustomerName"].ToString();
                            PreQuotModel.Currency = dr["Currency"].ToString();
                            PreQuotModel.CompCode = dr["CompCode"].ToString();
                            PreQuotModel.StatusId = Convert.ToInt32(dr["StatusId"]);
                            PreQuotModel.Status = dr["Status"].ToString();
                            PreQuotModel.FileName = dr["FileName"].ToString();
                            PreQuotModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            PreQuotModel.Remark = dr["Remark"].ToString();
                        }
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            PreliminaryQuotDetailModel DetailModel = new PreliminaryQuotDetailModel();

                            DetailModel.PreQuotId = Convert.ToInt32(dr["PreQuotId"]);
                            DetailModel.ItemId = Convert.ToInt32(dr["ItemId"]);
                            DetailModel.ItemName = dr["ItemName"].ToString();
                            DetailModel.MPN = dr["MPN"].ToString();
                            DetailModel.BrandName = dr["BrandName"].ToString();
                            DetailModel.Quantity = Convert.ToInt32(dr["Quantity"]);
                            DetailModel.Rate = Convert.ToDouble(dr["Rate"]);
                            DetailModel.Remark = dr["Remark"].ToString();
                            PreQuotModel.LstDetail.Add(DetailModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PreQuotModel;
        }
        public PreQuotRateDetailModel GetSelectedPreQuotRateList(int PreQuotId, int ItemId)
        {
            PreQuotRateDetailModel PreQuotRateModel = new PreQuotRateDetailModel();
            try
            {
                EnquiryDAL objDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetSelectedPreQuotRateList(PreQuotId, ItemId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            PreQuotRateModel.EnqId = Convert.ToInt32(dr["EnqId"]);
                            PreQuotRateModel.MPN = dr["MPN"].ToString();
                            PreQuotRateModel.ItemName = dr["ItemName"].ToString();
                            PreQuotRateModel.BrandName = dr["BrandName"].ToString();
                            PreQuotRateModel.ItemId = Convert.ToInt32(dr["ItemId"]);
                            PreQuotRateModel.LastSaleRate = Convert.ToDouble(dr["LastSaleRate"]);
                            PreQuotRateModel.AvgSixMonthRate = Convert.ToDouble(dr["AvgSixMonthRate"]);
                            PreQuotRateModel.VendPriceListRate = Convert.ToDouble(dr["VendPriceListRate"]);
                            PreQuotRateModel.ZaubaMax = Convert.ToDouble(dr["ZaubaMax"]);
                            PreQuotRateModel.ZaubaAvg = Convert.ToDouble(dr["ZaubaAvg"]);
                            PreQuotRateModel.ZaubaMin = Convert.ToDouble(dr["ZaubaMin"]);
                            PreQuotRateModel.Digikey = Convert.ToDouble(dr["Digikey"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PreQuotRateModel;
        }
        public int UpdateItemRate(int PreQuotId, int ItemId, double Rate, string User_Id)
        {
            EnquiryDAL objDAL = new EnquiryDAL();
            return objDAL.UpdateItemRate(PreQuotId, ItemId, Rate, User_Id);
        }
        #endregion Preliminary Quotation

        #region Nav POSO Mapping
        public List<NavPOSOMapModel> GetNavPOSOMappingList(string User_Id)
        {
            List<NavPOSOMapModel> LstModel = new List<NavPOSOMapModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetNavPOSOMappingList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            NavPOSOMapModel Model = new NavPOSOMapModel();
                            Model.NAVPONO = dr["NAVPONO"].ToString();
                            Model.NAVSONO = dr["NAVSONO"].ToString();
                            Model.CompCode = dr["CompanyName"].ToString();
                            Model.CreatedBy = dr["CreatedBy"].ToString();
                            Model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            Model.ModifiedBy = dr["ModifiedBy"].ToString();
                            Model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            LstModel.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstModel;
        }
        public NavPOSOMapModel GetSelectedNavPOSOMappingList(string NAVPONO, string NAVSONO, string CompCode)
        {
            NavPOSOMapModel Model = new NavPOSOMapModel();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedNavPOSOMappingList(NAVPONO, NAVSONO, CompCode);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Model.NAVPONO = ds.Tables[0].Rows[0]["NAVPONO"].ToString();
                        Model.NAVSONO = ds.Tables[0].Rows[0]["NAVSONO"].ToString();
                        Model.CompCode = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                        Model.OldNAVPONO = ds.Tables[0].Rows[0]["NAVPONO"].ToString();
                        Model.OldNAVSONO = ds.Tables[0].Rows[0]["NAVSONO"].ToString();
                        Model.OldCompCode = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                        Model.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        Model.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        Model.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        Model.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Model;
        }
        public int SaveNavPOSOMapping(string NAVPONO, string NAVSONO, string CompCode, string User_Id, string OldNAVPONO, string OldNAVSONO, string OldCompCode)
        {
            EnquiryDAL ObjDAL = new EnquiryDAL();
            int ErrCode = 0;
            try
            {
                ErrCode = ObjDAL.SaveNavPOSOMapping(NAVPONO, NAVSONO, CompCode, User_Id, OldNAVPONO, OldNAVSONO, OldCompCode);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        #endregion Nav POSO Mapping

        #region Online MPN Processing 
        public int GetEnquiryForPreQuot()
        {
            int errCode = 0;
            EnquiryDAL objDAL = new EnquiryDAL();
            errCode = objDAL.GetEnquiryForPreQuot();
            return errCode;
        }
        public DataSet GetEnqDetailForProcess(int EnqId)
        {
            DataSet ds = new DataSet();
            EnquiryDAL objDAL = new EnquiryDAL();
            ds = objDAL.GetEnqDetailForProcess(EnqId);
            return ds;
        }
        public int SaveImportData(int EnqId, string MPN, int SearchId)
        {
            int errCode = 0;
            EnquiryDAL objDAL = new EnquiryDAL();
            errCode = objDAL.SaveImportData(EnqId, MPN, SearchId);
            return errCode;
        }
        public int SaveImportDataSearch(int EnqId, string MPN)
        {
            int errCode = 0;
            EnquiryDAL objDAL = new EnquiryDAL();
            errCode = objDAL.SaveImportDataSearch(EnqId, MPN);
            return errCode;
        }
        public int SaveImportDataDigiKey(int EnqId, string MPN, int SearchId, double PriceBreak, double UnitPrice, double ExtendedPrice)
        {
            int errCode = 0;
            EnquiryDAL objDAL = new EnquiryDAL();
            errCode = objDAL.SaveImportDataDigiKey(EnqId, MPN, SearchId, PriceBreak, UnitPrice, ExtendedPrice);
            return errCode;
        }
        public int SaveImportDataZauba(int EnqId, string MPN, int SearchId, string TransactionDate, string HSCode, string Description, string OriginCountry, string PortofDischarge, string Unit, double Quantity, double ValueINR, double PerUnitINR)
        {
            int errCode = 0;
            EnquiryDAL objDAL = new EnquiryDAL();
            errCode = objDAL.SaveImportDataZauba(EnqId, MPN, SearchId, TransactionDate, HSCode, Description, OriginCountry, PortofDischarge, Unit, Quantity, ValueINR, PerUnitINR);
            return errCode;
        }
        public int SavePreQuotDetail(int EnqId, DataSet DS)
        {
            int errCode = 0;
            EnquiryDAL objDAL = new EnquiryDAL();
            errCode = objDAL.SavePreQuotDetail(EnqId, DS);
            return errCode;
        }
        #endregion Online MPN Processing

        #region Material Dispatch Intimetion
        public List<MDIModel> GetPurMRIntimetionList(string User_Id)
        {
            List<MDIModel> LstModel = new List<MDIModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetPurMRIntimetionList(User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            MDIModel Model = new MDIModel();
                            Model.MRIId = Convert.ToInt32(dr["MRIId"].ToString());
                            Model.MRIDate = Convert.ToDateTime(dr["MRIDate"].ToString());
                            Model.DocumentNo = dr["DocumentNo"].ToString();
                            Model.VendorName = dr["VendorName"].ToString();
                            Model.CompCode = dr["CompCode"].ToString();
                            Model.Remark = dr["Remark"].ToString();
                            Model.CreatedBy = dr["CreatedBy"].ToString();
                            Model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            Model.ModifiedBy = dr["ModifiedBy"].ToString();
                            Model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            LstModel.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstModel;
        }
        public MDIModel GetSelectedPurMRIntimetionList(int MRIId)
        {
            MDIModel Model = new MDIModel();
            Model.LstMDIItemList = new List<MDIItemModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetSelectedPurMRIntimetionList(MRIId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Model.MRIId = Convert.ToInt32(ds.Tables[0].Rows[0]["MRIId"].ToString());
                        Model.MRIDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["MRIDate"].ToString());
                        Model.DocumentNo = ds.Tables[0].Rows[0]["DocumentNo"].ToString();
                        Model.VendorId = Convert.ToInt32(ds.Tables[0].Rows[0]["VendorId"].ToString());
                        Model.VendorName = ds.Tables[0].Rows[0]["VendorName"].ToString();
                        Model.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                        Model.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                        Model.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        Model.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        Model.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        Model.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                    }
                    if (ds.Tables.Count > 1)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            MDIItemModel DetailModel = new MDIItemModel();
                            DetailModel.MRIDetailId = Convert.ToInt32(dr["MRIDetailId"].ToString());
                            DetailModel.ReceiptId = Convert.ToInt32(dr["ReceiptId"].ToString());
                            DetailModel.MRIId = Convert.ToInt32(dr["MRIId"].ToString());
                            DetailModel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            DetailModel.ItemName = dr["ItemName"].ToString();
                            DetailModel.MPN = dr["MPN"].ToString();
                            DetailModel.Rate = Convert.ToDouble(dr["Rate"].ToString());
                            DetailModel.Quantity = Convert.ToInt32(dr["Quantity"].ToString());
                            DetailModel.NAVPONO = dr["NAVPONO"].ToString();
                            DetailModel.NAVPODate = Convert.ToDateTime(dr["NAVPODate"].ToString());
                            DetailModel.StrNAVPODate = DetailModel.NAVPODate.ToShortDateString();
                            DetailModel.CreatedBy = dr["CreatedBy"].ToString();
                            DetailModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            DetailModel.strCreatedDate = DetailModel.CreatedDate.ToShortDateString();
                            Model.LstMDIItemList.Add(DetailModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Model;
        }
        public int DWSavePurMRIntimetion(MDIModel model, string User_Id)
        {
            DataSet ds = new DataSet();
            EnquiryDAL ObjDAL = new EnquiryDAL();
            ds = ObjDAL.GetSelectedPurMRIntimetionList(0);
            try
            {
                ds.Tables[0].Rows.Clear();
                DataRow Dr = ds.Tables[0].NewRow();
                Dr["MRIId"] = model.MRIId;
                Dr["MRIDate"] = model.MRIDate;
                Dr["VendorId"] = model.VendorId;
                Dr["CompCode"] = model.CompCode;
                Dr["Remark"] = model.Remark;
                Dr["DocumentNo"] = model.DocumentNo;
                ds.Tables[0].Rows.Add(Dr);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ObjDAL.DWSavePurMRIntimetion(ds, User_Id);
        }
        public List<MDIItemModel> SavePurMRIntimetionItem(int Itemid, int Quantity, string PONo, DateTime ExpectedDate, Double Rate, int MRIId, string User_Id, int MRIDetailId)
        {
            int ErrCode = 0;
            EnquiryDAL ObjDAL = new EnquiryDAL();
            ErrCode = ObjDAL.SavePurMRIntimetionItem(Itemid, Quantity, PONo, ExpectedDate, Rate, MRIId, User_Id, MRIDetailId);
            MDIModel Model = new MDIModel();
            Model.LstMDIItemList = new List<MDIItemModel>();
            Model = GetSelectedPurMRIntimetionList(MRIId);
            return Model.LstMDIItemList;
        }
        public List<MDIItemModel> DeleteIntimetionItem(int MRIId, int MRIDetailId)
        {
            int ErrCode = 0;
            EnquiryDAL ObjDAL = new EnquiryDAL();
            ErrCode = ObjDAL.DeleteIntimetionItem(MRIDetailId);
            MDIModel Model = new MDIModel();
            Model.LstMDIItemList = new List<MDIItemModel>();
            Model = GetSelectedPurMRIntimetionList(MRIId);
            return Model.LstMDIItemList;
        }
        #endregion Material Dispatch Intimetion

        #region Quotation Analyst
        public EnquiryModel getEnquiryQuotAnalystList(string UserId)
        {
            EnquiryModel EnquiryModel = new EnquiryModel();
            EnquiryModel.lstEnquiry = new List<EnquiryModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.getEnquiryQuotAnalystList(UserId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            EnquiryModel objmodel = new EnquiryModel();
                            objmodel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            objmodel.EnqNumber = dr["EnqNumber"].ToString();
                            objmodel.EnqDate = Convert.ToDateTime(dr["EnqDate"].ToString());
                            objmodel.CustomerName = dr["CustomerName"].ToString();
                            objmodel.SalesPerson = dr["SalesPerson"].ToString();
                            objmodel.CompCode = dr["CompCode"].ToString();
                            objmodel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            objmodel.AssignedEmployee = dr["Employee"].ToString();
                            objmodel.StatusStr = dr["StatusStr"].ToString();
                            objmodel.Status = Convert.ToInt32(dr["Status"].ToString());
                            objmodel.QuotaionCost = Convert.ToDouble(dr["QuotCost"].ToString());
                            objmodel.Priority = dr["Priority"].ToString();
                            objmodel.CustomerContactId = Convert.ToInt32(dr["CustomerContactId"].ToString());
                            objmodel.ContactName = dr["ContactName"].ToString();
                            objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            if (dr["AnalyItem"].ToString() != "0" && dr["AnalyItem"].ToString() != "")
                            {
                                objmodel.PercentComplete = (Convert.ToInt32(dr["AnalyItem"].ToString())) * 100 / Convert.ToInt32(dr["TotalItem"].ToString());
                                if (objmodel.PercentComplete == 100)
                                    objmodel.Description = "Complete";
                                else
                                    objmodel.Description = "Partial Complete";
                            }
                            else
                            {
                                objmodel.PercentComplete = 0;
                                objmodel.Description = "InComplete";
                            }
                            EnquiryModel.lstEnquiry.Add(objmodel);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return EnquiryModel;
        }
        public QuotAnalystModel GetQuotAnalystDetailList(int EnqId, string User_Id)
        {
            QuotAnalystModel AnalystModel = new QuotAnalystModel();
            AnalystModel.Last5lossQuot = new List<QoutFollowupModel>();
            AnalystModel.Last5WinQuot = new List<QoutFollowupModel>();
            AnalystModel.QuotFollowupDetailList = new List<QoutFollowupHistoryModel>();
            AnalystModel.QuotItemList = new List<QuotItem>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetQuotAnalystDetailList(EnqId, User_Id);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        AnalystModel.EnqId = Convert.ToInt32(ds.Tables[0].Rows[0]["EnqId"]);
                        AnalystModel.EnqNumber = ds.Tables[0].Rows[0]["EnqNumber"].ToString();
                        AnalystModel.EnqDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["EnqDate"]);
                        AnalystModel.status = ds.Tables[0].Rows[0]["status"].ToString();
                        AnalystModel.ContactName = ds.Tables[0].Rows[0]["ContactName"].ToString();
                        AnalystModel.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                        AnalystModel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                        AnalystModel.categories = ds.Tables[0].Rows[0]["categories"].ToString();
                        AnalystModel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                        AnalystModel.MailAttchment = ds.Tables[0].Rows[0]["MailAttchment"].ToString();
                        AnalystModel.QuotationPdfAttach = ds.Tables[0].Rows[0]["QuotationPdfAttach"].ToString();
                        AnalystModel.TotalHR = ds.Tables[0].Rows[0]["TotalHR"].ToString();
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            QoutFollowupHistoryModel QuotFollowuoModel = new QoutFollowupHistoryModel();
                            QuotFollowuoModel.QuotationId = Convert.ToInt32(dr["QuotationId"].ToString());
                            QuotFollowuoModel.QuotDate = Convert.ToDateTime(dr["QuotDate"].ToString());
                            QuotFollowuoModel.QuotationNumber = dr["QuotationNumber"].ToString();
                            QuotFollowuoModel.RevisedQuatId = Convert.ToInt32(dr["RevisedQuatId"].ToString());
                            QuotFollowuoModel.FollowupReason = dr["FollowupReason"].ToString();
                            QuotFollowuoModel.Followupdate = Convert.ToDateTime(dr["FollowupDate"].ToString());
                            QuotFollowuoModel.NextFollowUp = Convert.ToDateTime(dr["NextFollowupDate"].ToString());
                            QuotFollowuoModel.CreatedBy = dr["CreatedBy"].ToString();
                            QuotFollowuoModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            AnalystModel.QuotFollowupDetailList.Add(QuotFollowuoModel);
                        }
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            QoutFollowupModel WinQuotModel = new QoutFollowupModel();
                            WinQuotModel.QuotationId = Convert.ToInt32(dr["QuotationId"].ToString());
                            WinQuotModel.QuotDate = Convert.ToDateTime(dr["QuotDate"].ToString());
                            WinQuotModel.QuotationNumber = dr["QuotationNumber"].ToString();
                            WinQuotModel.QuotCost = Convert.ToDouble(dr["QuotCost"].ToString());
                            WinQuotModel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            WinQuotModel.Currency = dr["Currancy"].ToString();
                            WinQuotModel.CompCode = dr["CompCode"].ToString();
                            WinQuotModel.Followupdate = Convert.ToDateTime(dr["FollowupDate"].ToString());
                            WinQuotModel.Term = dr["Term"].ToString();
                            WinQuotModel.CustomerName = dr["CustomerName"].ToString();
                            WinQuotModel.SalesPersonName = dr["SalesPerson"].ToString();
                            WinQuotModel.Status = dr["Status"].ToString();
                            WinQuotModel.CreatedBy = dr["CreatedBy"].ToString();
                            WinQuotModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            AnalystModel.Last5WinQuot.Add(WinQuotModel);
                        }
                    }
                    if (ds.Tables[3].Rows.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[3].Rows)
                        {
                            QoutFollowupModel LossQuotModel = new QoutFollowupModel();
                            LossQuotModel.QuotationId = Convert.ToInt32(dr["QuotationId"].ToString());
                            LossQuotModel.QuotDate = Convert.ToDateTime(dr["QuotDate"].ToString());
                            LossQuotModel.QuotationNumber = dr["QuotationNumber"].ToString();
                            LossQuotModel.QuotCost = Convert.ToDouble(dr["QuotCost"].ToString());
                            LossQuotModel.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                            LossQuotModel.Currency = dr["Currancy"].ToString();
                            LossQuotModel.CompCode = dr["CompCode"].ToString();
                            LossQuotModel.Followupdate = Convert.ToDateTime(dr["FollowupDate"].ToString());
                            LossQuotModel.Term = dr["Term"].ToString();
                            LossQuotModel.CustomerName = dr["CustomerName"].ToString();
                            LossQuotModel.SalesPersonName = dr["SalesPerson"].ToString();
                            LossQuotModel.Status = dr["Status"].ToString();
                            LossQuotModel.FollowupReason = dr["Reason"].ToString();
                            LossQuotModel.CreatedBy = dr["CreatedBy"].ToString();
                            LossQuotModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            AnalystModel.Last5lossQuot.Add(LossQuotModel);
                        }
                    }
                    if (ds.Tables[4].Rows.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[4].Rows)
                        {
                            QuotItem QuoItemtModel = new QuotItem();
                            QuoItemtModel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            QuoItemtModel.ItemName = dr["ItemName"].ToString();
                            QuoItemtModel.MPN = dr["MPN"].ToString();
                            QuoItemtModel.status = Convert.ToInt32(dr["status"].ToString());
                            QuoItemtModel.AnalystRemark = dr["AnalystRemark"].ToString();
                            AnalystModel.QuotItemList.Add(QuoItemtModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return AnalystModel;
        }
        public QuotItem GetQuotAnalystItemDetailList(int EnqId, int ItemId)
        {
            QuotItem ItemModel = new QuotItem();
            try
            {
                ItemModel.ItemQuotList = new List<EnquiryItemVendorResponseDetailModel>();
                ItemModel.Last5PurchaseItemList = new List<QuotItemHistory>();
                ItemModel.Last5SalesItemList = new List<QuotItemHistory>();
                ItemModel.LastSalesItemSameCustList = new List<QuotItemHistory>();
                ItemModel.ItemVendorResList = new List<EnquiryItemVendorResponseDetailModel>();
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.GetQuotItemAnalystDetailList(EnqId, ItemId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ItemModel.MPN = ds.Tables[0].Rows[0]["MPN"].ToString();
                        ItemModel.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                        ItemModel.Quantity = Convert.ToInt32(ds.Tables[0].Rows[0]["Quantity"]);
                        ItemModel.status = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"]);
                        ItemModel.BrandName = ds.Tables[0].Rows[0]["BrandName"].ToString();
                        ItemModel.ExpectedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ExpectedDate"]);
                        ItemModel.StrExpectedDate = ItemModel.ExpectedDate.ToShortDateString();
                        ItemModel.CPN = ds.Tables[0].Rows[0]["CPN"].ToString();
                        ItemModel.AnalystRemark = ds.Tables[0].Rows[0]["AnalystRemark"].ToString();
                        ItemModel.LastSalesRate = Convert.ToDouble(ds.Tables[0].Rows[0]["LastSalesRate"]);
                        ItemModel.LastPurchaseRate = Convert.ToDouble(ds.Tables[0].Rows[0]["LastPurchaseRate"]);
                        ItemModel.MAXSalesRate = Convert.ToDouble(ds.Tables[0].Rows[0]["MAXSalesRate"]);
                        ItemModel.MINSalesRate = Convert.ToDouble(ds.Tables[0].Rows[0]["MINSalesRate"]);
                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            EnquiryItemVendorResponseDetailModel VendResModel = new EnquiryItemVendorResponseDetailModel();
                            VendResModel.MinimumQty = Convert.ToInt32(dr["MinimumQty"].ToString());
                            VendResModel.ReciptDate = Convert.ToDateTime(dr["ReciptDate"].ToString());
                            VendResModel.StrReciptDate = VendResModel.ReciptDate.ToShortDateString();
                            VendResModel.VendorName = dr["VendorName"].ToString();
                            VendResModel.SPQ = Convert.ToInt32(dr["SPQ"].ToString());
                            VendResModel.MOQ = Convert.ToInt32(dr["MOQ"].ToString());
                            VendResModel.Rate = Convert.ToDouble(dr["Rate"].ToString());
                            VendResModel.Currency = dr["Currency"].ToString();
                            VendResModel.VendorPromiseDate = dr["Currency"].ToString();
                            ItemModel.ItemVendorResList.Add(VendResModel);
                        }
                    }
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            EnquiryItemVendorResponseDetailModel QuotModel = new EnquiryItemVendorResponseDetailModel();
                            QuotModel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            QuotModel.Currency = dr["Currancy"].ToString();
                            QuotModel.QuotNumber = dr["QuotationNumber"].ToString();
                            QuotModel.ItemName = dr["ItemName"].ToString();
                            QuotModel.MPN = dr["MPN"].ToString();
                            QuotModel.CPN = dr["CPN"].ToString();
                            QuotModel.CRate = Convert.ToDouble(dr["CustRate"].ToString());
                            QuotModel.BrandName = dr["BrandName"].ToString();
                            QuotModel.Remark = dr["Remark"].ToString();
                            QuotModel.Quantity = Convert.ToInt32(dr["Quantity"].ToString());
                            QuotModel.SPQ = Convert.ToInt32(dr["SPQ"].ToString());
                            QuotModel.MOQ = Convert.ToInt32(dr["MOQ"].ToString());
                            QuotModel.Rate = Convert.ToDouble(dr["VendorRate"].ToString());
                            // QuotModel.VendorPromiseDate = dr["VendorPromiseDate"].ToString();
                            QuotModel.Margin = Convert.ToDouble(dr["Margin"].ToString());
                            ItemModel.ItemQuotList.Add(QuotModel);
                        }
                    }
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[3].Rows)
                        {
                            QuotItemHistory Last5SalesModel = new QuotItemHistory();
                            Last5SalesModel.Qty = Convert.ToDouble(dr["Quantity"].ToString());
                            Last5SalesModel.MPN = dr["MPN"].ToString();
                            Last5SalesModel.DocNo = dr["DocNo"].ToString();
                            Last5SalesModel.Name = dr["CustomerName"].ToString();
                            Last5SalesModel.Rate = Convert.ToDouble(dr["Rate"].ToString());
                            Last5SalesModel.SalesDate = Convert.ToDateTime(dr["QuotDate"].ToString());
                            Last5SalesModel.StrSalesDate = Last5SalesModel.SalesDate.ToShortDateString();
                            ItemModel.Last5SalesItemList.Add(Last5SalesModel);
                        }
                    }
                    if (ds.Tables[4].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[4].Rows)
                        {
                            QuotItemHistory Last5PurchaseModel = new QuotItemHistory();
                            Last5PurchaseModel.Qty = Convert.ToDouble(dr["MinimumQty"].ToString());
                            Last5PurchaseModel.MPN = dr["MPN"].ToString();
                            Last5PurchaseModel.DocNo = dr["DocNo"].ToString();
                            Last5PurchaseModel.Name = dr["VendorName"].ToString();
                            Last5PurchaseModel.Rate = Convert.ToDouble(dr["Rate"].ToString());
                            Last5PurchaseModel.SalesDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            Last5PurchaseModel.StrSalesDate = Last5PurchaseModel.SalesDate.ToShortDateString();
                            ItemModel.Last5PurchaseItemList.Add(Last5PurchaseModel);
                        }
                    }
                    if (ds.Tables[5].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[5].Rows)
                        {
                            QuotItemHistory LastSalesModel = new QuotItemHistory();
                            LastSalesModel.Qty = Convert.ToDouble(dr["Quantity"].ToString());
                            LastSalesModel.MPN = dr["MPN"].ToString();
                            LastSalesModel.DocNo = dr["DocNo"].ToString();
                            LastSalesModel.Name = dr["CustomerName"].ToString();
                            LastSalesModel.Rate = Convert.ToDouble(dr["Rate"].ToString());
                            LastSalesModel.SalesDate = Convert.ToDateTime(dr["QuotDate"].ToString());
                            LastSalesModel.StrSalesDate = LastSalesModel.SalesDate.ToShortDateString();
                            ItemModel.LastSalesItemSameCustList.Add(LastSalesModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemModel;
        }
        public int SaveAnalystRemark(int EnqId, int ItemId, string Remark, string User_Id)
        {
            EnquiryDAL ObjDAL = new EnquiryDAL();
            return ObjDAL.SaveAnalystRemark(EnqId, ItemId, Remark, User_Id);
        }
        #endregion Quotation Analyst

        #region Temporary Receipt Entry'
        public List<InvoiceItemModel> TempGetItemListByReceipt(string CompCode, int VendorId)
        {
            List<InvoiceItemModel> LstInvItemModel = new List<InvoiceItemModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.TempGetItemListByReceipt(CompCode, VendorId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            InvoiceItemModel InvoiceItemModel = new InvoiceItemModel();
                            InvoiceItemModel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            InvoiceItemModel.Item_Id = dr["Item_No"].ToString();
                            InvoiceItemModel.ItemName = dr["ItemName"].ToString();
                            LstInvItemModel.Add(InvoiceItemModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstInvItemModel;
        }
        public List<InvoiceModel> TempGetPOforReceiptByItem(string CompCode, int ItemId)
        {
            List<InvoiceModel> LstInvModel = new List<InvoiceModel>();
            try
            {
                EnquiryDAL ObjDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = ObjDAL.TempGetPOforReceiptByItem(CompCode, ItemId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            InvoiceModel InvoiceModel = new InvoiceModel();
                            InvoiceModel.Invoice_Id = dr["PurchaseOrderNumber"].ToString();
                            InvoiceModel.InvoiceName = dr["PONumberDesc"].ToString();
                            LstInvModel.Add(InvoiceModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstInvModel;
        }
        #endregion Temporary Receipt Entry

        #region Item Stock Audit
        public List<ItemStockAuditModel> GetItemStockAuditList()
        {
            List<ItemStockAuditModel> AuditList = new List<ItemStockAuditModel>();
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = DALObj.GetItemStockAuditList();
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            ItemStockAuditModel AuditModel = new ItemStockAuditModel();
                            AuditModel.AuditId = Convert.ToInt32(dr["AuditId"].ToString());
                            AuditModel.CompCode = dr["CompCode"].ToString();
                            AuditModel.AuditBy = dr["AuditBy"].ToString();
                            AuditModel.AuditDate = Convert.ToDateTime(dr["AuditDate"].ToString());
                            AuditModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            AuditModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            AuditList.Add(AuditModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return AuditList;
        }
        public ItemStockAuditModel GetSelectedItemStockAuditList(int AuditId)
        {
            ItemStockAuditModel StockModel = new ItemStockAuditModel();
            StockModel.DetailList = new List<ItemStockAuditDetailModel>();
            try
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = DALObj.GetSelectedItemStockAuditList(AuditId);
                if (ds == null)
                    return null;
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {                        
                        StockModel.AuditId = Convert.ToInt32(ds.Tables[0].Rows[0]["AuditId"].ToString());
                        StockModel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                        StockModel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                        StockModel.AuditBy = ds.Tables[0].Rows[0]["AuditByName"].ToString();
                        StockModel.AuditDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["AuditDate"].ToString());
                        StockModel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        StockModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                    }
                }
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            ItemStockAuditDetailModel AuditDetailModel = new ItemStockAuditDetailModel();
                            AuditDetailModel.AuditId = Convert.ToInt32(dr["AuditId"].ToString());
                            AuditDetailModel.AuditDetailId = Convert.ToInt32(dr["AuditDetailId"].ToString());
                            AuditDetailModel.ItemName = dr["ItemName"].ToString();
                            AuditDetailModel.MPN = dr["MPN"].ToString();
                            AuditDetailModel.LocationId = Convert.ToInt32(dr["LocationId"].ToString());
                            AuditDetailModel.LocationIdDesc = dr["Description"].ToString();
                            AuditDetailModel.ItemId = Convert.ToInt32(dr["ItemId"].ToString());
                            AuditDetailModel.Quantity = Convert.ToInt32(dr["Quantity"].ToString());
                            AuditDetailModel.CreatedBy = dr["CreatedBy"].ToString();
                            DateTime dt = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            AuditDetailModel.CreatedDate = dt.ToShortDateString();
                            AuditDetailModel.ModifiedBy = dr["ModifiedBy"].ToString();
                            DateTime dt1 = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            AuditDetailModel.ModifiedDate = dt1.ToShortDateString();
                            StockModel.DetailList.Add(AuditDetailModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return StockModel;
        }
        public ReportNameModel GetOpenSystemStockReport(string user_Id,string CompanyCode)
        {
            ReportNameModel Model = new ReportNameModel();
            try
            {
                EnquiryDAL objDAL = new EnquiryDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetSystemStockReport(user_Id, CompanyCode);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        Model.OutputLocation = ds.Tables[0].Rows[0]["OutputLocation"].ToString();
                        Model.RunDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["RunDate"].ToString());
                        Model.ReportId = ds.Tables[0].Rows[0]["ReportId"].ToString();
                        Model.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"].ToString());
                        Model.TxtParamValue = ds.Tables[0].Rows[0]["TxtParamValue"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Model;
        }
        public int DWSaveItemStockAudit(int AuditId, string CompCode, DateTime AuditDate, string User_Id,string remark)
        {
            int ErrCod = 0;
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                ErrCod = DALObj.DWSaveItemStockAudit(AuditId, CompCode, AuditDate, User_Id,remark);
            }
            return ErrCod;
        }
        public int DWSaveItemStockAuditDetail(int AuditDetailId, int AuditId, int ItemId, int Quantity, string User_Id,int LocationId)
        {
            int ErrCod = 0;
            {
                EnquiryDAL DALObj = new EnquiryDAL();
                ErrCod = DALObj.DWSaveItemStockAuditDetail(AuditDetailId,AuditId, ItemId, Quantity, User_Id, LocationId);
            }
            return ErrCod;
        }
        public List<ItemListModel> GetItemStockListForDrp(string CompCode)
        {
            List<ItemListModel> lstItem = new List<ItemListModel>();
            try
            {
                DataSet dsItem = new DataSet();
                EnquiryDAL DALObj = new EnquiryDAL();
                dsItem = DALObj.GetItemStockListForDrp(CompCode);
                if (dsItem != null)
                {
                    if (dsItem.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsItem.Tables[0].Rows)
                        {
                            ItemListModel objmodel = new ItemListModel();
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
        public List<ItemStockAuditDetailModel> CheckStockExcelItem(List<ItemStockAuditDetailModel> Model)
        {
            List<ItemStockAuditDetailModel> lstModel = new List<ItemStockAuditDetailModel>();
            try
            {
                DataSet dsItem = new DataSet();
                EnquiryDAL DALObj = new EnquiryDAL();
                dsItem = DALObj.CheckMPNforStockAudit();
                bool chk = false;
                foreach (ItemStockAuditDetailModel demo in Model)
                {
                    DataRow dr = dsItem.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["MPN"]).ToUpper() == demo.MPN.ToString().ToUpper());
                    if (dr != null)
                    {
                        demo.ItemId = Convert.ToInt32(dr["itemId"]);
                        demo.ItemName = Convert.ToString(dr["ItemName"]);
                    }
                    else
                    {
                        demo.ErrorDetail = demo.ErrorDetail + "\n" + "MPN Not avalable in System.";
                        demo.Check = "NotOk";
                    }
                    DataRow dr1 = dsItem.Tables[1].AsEnumerable().FirstOrDefault(r => Convert.ToInt32(r["STLocationId"]) == demo.LocationId);
                    if (dr1 != null)
                    {
                        demo.LocationIdDesc = Convert.ToString(dr1["Description"]);
                    }
                    lstModel.Add(demo);
                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return lstModel;
        }

        public int SaveStockAuditExcelData(List<ItemStockAuditDetailModel> Lststoc,int AuditId, string User_Id)
        {
            DataSet ds = new DataSet();
            EnquiryDAL ObjDAL = new EnquiryDAL();
            ds = ObjDAL.GetSelectedItemStockAuditList(0);
            try
            {
                ds.Tables[0].Rows.Clear();
                
                foreach (ItemStockAuditDetailModel Model in Lststoc)
                {
                    if (Model.Check != "NotOk")
                    {
                        DataRow Dr = ds.Tables[1].NewRow();
                        Dr["AuditDetailId"] = Model.AuditDetailId;
                        Dr["AuditId"] = Model.AuditId;
                        Dr["ItemId"] = Model.ItemId;
                        Dr["Quantity"] = Model.Quantity;
                        Dr["LocationId"] = Model.LocationId;
                        ds.Tables[1].Rows.Add(Dr);
                    }
                }                
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ObjDAL.SaveStockAuditExcelData(ds, AuditId, User_Id);
        }
        #endregion Item Stock Audit
    }

}

