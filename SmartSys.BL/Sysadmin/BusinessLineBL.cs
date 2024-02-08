using SmartSys.DL.Business_Line;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.Sysadmin
{
   public  class BusinessLineBL
    {
       public List<BusinessLineModel> GetBussLineList(string UserId)
       {
           List<BusinessLineModel> lstBussLine = new List<BusinessLineModel>();
           BusinessLineDAL objDAL = new BusinessLineDAL();
           try
           {

               DataSet dsBussLine = new DataSet();
               dsBussLine = objDAL.GetBussLineList(UserId);
               if (dsBussLine != null)
               {
                   if (dsBussLine.Tables.Count > 0)
                   {
                       foreach (DataRow dr in dsBussLine.Tables[0].Rows)
                       {
                           BusinessLineModel bussLineObj = new BusinessLineModel();
                           bussLineObj.BusinessLineId = Convert.ToInt16(dr["BusinessLineId"].ToString());
                           bussLineObj.BusinessLineName = dr["BusinessLineName"].ToString();
                           bussLineObj.Description = dr["Description"].ToString();
                           bussLineObj.CreatedBy = Convert.ToInt16(dr["CreatedBy"].ToString());
                           bussLineObj.CreatedByName = dr["CreatedByName"].ToString();
                           bussLineObj.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                           bussLineObj.ModifiedBy = Convert.ToInt16(dr["ModifiedBy"].ToString());
                           bussLineObj.ModifiedByName = dr["ModifiedByName"].ToString();
                           bussLineObj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());

                           lstBussLine.Add(bussLineObj);
                       }
                   }
               }             
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return lstBussLine;
       }


       public int SaveBussLine(BusinessLineModel BussLineModel, string User_Id)
       {
           BusinessLineDAL objDAL = new BusinessLineDAL();
           DataSet dsBussLine = new DataSet();
           try
           {
               dsBussLine = objDAL.BussLineGetSelected(0);
               if (dsBussLine != null)
               {
                   if (dsBussLine.Tables.Count > 0)
                   {
                       dsBussLine.Tables[0].Rows.Clear();

                       DataRow dr = dsBussLine.Tables[0].NewRow();

                       dr["BusinessLineId"] = BussLineModel.BusinessLineId;
                       dr["BusinessLineName"] = BussLineModel.BusinessLineName;
                       dr["Description"] = BussLineModel.Description;
                       dr["ModifiedBy"] = BussLineModel.ModifiedBy;
                       dsBussLine.Tables[0].Rows.Add(dr);
                   }
               }
               
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return objDAL.SaveBussLine(dsBussLine, User_Id);
       }
       public BusinessLineModel BussLineGetSelected(Int16 BusinessLineId)
       {
           BusinessLineModel bussLineModel = new BusinessLineModel();
           try
           {
               BusinessLineDAL objDAL = new BusinessLineDAL();
               DataSet dsBussLine = new DataSet();
               dsBussLine = objDAL.BussLineGetSelected(BusinessLineId);
               if (dsBussLine != null)
               {
                   if (dsBussLine.Tables.Count > 0)
                   {
                       if (dsBussLine.Tables[0].Rows.Count > 0)
                       {
                           bussLineModel.BusinessLineId = BusinessLineId;
                           bussLineModel.BusinessLineName = dsBussLine.Tables[0].Rows[0]["BusinessLineName"].ToString();
                           bussLineModel.Description = dsBussLine.Tables[0].Rows[0]["Description"].ToString();
                           bussLineModel.CreatedByName = dsBussLine.Tables[0].Rows[0]["CreatedByName"].ToString();
                           bussLineModel.ModifiedByName = dsBussLine.Tables[0].Rows[0]["ModifiedByName"].ToString();
                           bussLineModel.CreatedDate = Convert.ToDateTime(dsBussLine.Tables[0].Rows[0]["CreatedDate"].ToString());
                           bussLineModel.ModifiedDate = Convert.ToDateTime(dsBussLine.Tables[0].Rows[0]["ModifiedDate"].ToString());
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return bussLineModel;
       }
    }
}
