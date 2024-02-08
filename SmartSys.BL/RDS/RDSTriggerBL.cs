using SmartSys.DL;
using SmartSys.DL.RDS;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SmartSys.BL.RDS
{
   public class RDSTriggerBL
    {
       public List<RDSTriggerModel> GetRDSTriggeList()
       {
           List<RDSTriggerModel> lstRDSTrigger = new List<RDSTriggerModel>();
           try
           {

               RDSTriggerDAL objDAL = new RDSTriggerDAL();
               DataSet dsRDSTrigge = new DataSet();
               dsRDSTrigge = objDAL.GetRDSTriggerList();
               if (dsRDSTrigge != null)
               {
                   if (dsRDSTrigge.Tables.Count > 0)
                   {
                       foreach (DataRow dr in dsRDSTrigge.Tables[0].Rows)
                       {
                           RDSTriggerModel rdsTriggModel = new RDSTriggerModel();
                           rdsTriggModel.TriggerId = Convert.ToInt16(dr["TriggerId"].ToString());
                           rdsTriggModel.TriggerName = dr["TriggerName"].ToString();
                           rdsTriggModel.Description = dr["Description"].ToString();
                           rdsTriggModel.CreatedBy = Convert.ToInt16(dr["CreatedBy"].ToString());
                           rdsTriggModel.CreatedByName = dr["CreatedByName"].ToString();
                           rdsTriggModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                           rdsTriggModel.ModifiedBy = Convert.ToInt16(dr["ModifiedBy"].ToString());
                           rdsTriggModel.ModifiedByName = dr["ModifiedByName"].ToString();
                           rdsTriggModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                           lstRDSTrigger.Add(rdsTriggModel);
                       }
                   }
               }

           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return lstRDSTrigger;
       }

       public RDSTriggerModel RDSTriggerGetSelected(Int16 TriggerId)
       {
           RDSTriggerModel rdsTriggModel = new RDSTriggerModel();
           try
           {
               RDSTriggerDAL objDAL = new RDSTriggerDAL();
               DataSet dsRDSTrigg = new DataSet();
               dsRDSTrigg = objDAL.RDSTriggerGetSelected(TriggerId);
               if (dsRDSTrigg != null)
               {
                   if (dsRDSTrigg.Tables.Count > 0)
                   {
                       if (dsRDSTrigg.Tables[0].Rows.Count > 0)
                       {
                           rdsTriggModel.TriggerId = TriggerId;
                           rdsTriggModel.TriggerName = dsRDSTrigg.Tables[0].Rows[0]["TriggerName"].ToString();
                           rdsTriggModel.Description = dsRDSTrigg.Tables[0].Rows[0]["Description"].ToString();
                           rdsTriggModel.CreatedDate = Convert.ToDateTime(dsRDSTrigg.Tables[0].Rows[0]["CreatedDate"].ToString());
                           rdsTriggModel.CreatedByName = dsRDSTrigg.Tables[0].Rows[0]["CreatedBy"].ToString();
                           rdsTriggModel.ModifiedDate = Convert.ToDateTime(dsRDSTrigg.Tables[0].Rows[0]["ModifiedDate"].ToString());
                           rdsTriggModel.ModifiedByName = dsRDSTrigg.Tables[0].Rows[0]["ModifiedBy"].ToString();
                           rdsTriggModel.ModifiedDate = Convert.ToDateTime(dsRDSTrigg.Tables[0].Rows[0]["ModifiedDate"].ToString());

                       }
                   }
               }

           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return rdsTriggModel;
       }
       public int SaveRDSTrigger(RDSTriggerModel rdsTriggModel, string User_Id)
       {
           RDSTriggerDAL objDAL = new RDSTriggerDAL();
           DataSet dsRDSTrigg = new DataSet();
           dsRDSTrigg = objDAL.RDSTriggerGetSelected(0);
           try
           {
               dsRDSTrigg.Tables[0].Rows.Clear();
               DataRow dr = dsRDSTrigg.Tables[0].NewRow();
               dr["TriggerId"] = rdsTriggModel.TriggerId;
               dr["TriggerName"] = rdsTriggModel.TriggerName;
               dr["Description"] = rdsTriggModel.Description;
               dr["ModifiedBy"] = rdsTriggModel.ModifiedBy;
               dsRDSTrigg.Tables[0].Rows.Add(dr);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return objDAL.SaveRDSTrigger(dsRDSTrigg, User_Id);
       }


    }
}
