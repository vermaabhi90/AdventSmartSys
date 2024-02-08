using SmartSys.BL.RDS;
using SmartSys.DL.DataHub;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.DataHub
{
 public class DHTriggerBL
    {
     public List<DHTriggerModel> GetDHTriggeList()
        {
            List<DHTriggerModel> lstDHTrigger = new List<DHTriggerModel>();
            try
            {
                DHTriggerDAL objDAL = new DHTriggerDAL();
                DataSet dsDHTrigge = new DataSet();
                dsDHTrigge = objDAL.GetDHTriggerList();
                if (dsDHTrigge != null)
                {
                    if (dsDHTrigge.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDHTrigge.Tables[0].Rows)
                        {
                            DHTriggerModel dhTriggModel = new DHTriggerModel();
                            dhTriggModel.TriggerId = Convert.ToInt16(dr["TriggerId"].ToString());
                            dhTriggModel.TriggerName = dr["TriggerName"].ToString();
                            dhTriggModel.Description = dr["Description"].ToString();
                            dhTriggModel.CreatedBy = Convert.ToInt16(dr["CreatedBy"].ToString());
                            dhTriggModel.CreatedByName = dr["CreatedByName"].ToString();
                            dhTriggModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            dhTriggModel.ModifiedBy = Convert.ToInt16(dr["ModifiedBy"].ToString());
                            dhTriggModel.ModifiedByName = dr["ModifiedByName"].ToString();
                            dhTriggModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            lstDHTrigger.Add(dhTriggModel);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstDHTrigger;
        }

        public DHTriggerModel DHTriggerGetSelected(Int16 TriggerId)
        {
            DHTriggerModel dhTriggModel = new DHTriggerModel();
            try
            {
                DHTriggerDAL objDAL = new DHTriggerDAL();
                DataSet dsDHTrigge = new DataSet();
                dsDHTrigge = objDAL.DHTriggerGetSelected(TriggerId);
                if (dsDHTrigge != null)
                {
                    if (dsDHTrigge.Tables.Count > 0)
                    {
                        if (dsDHTrigge.Tables[0].Rows.Count > 0)
                        {
                            dhTriggModel.TriggerId = TriggerId;
                            dhTriggModel.TriggerName = dsDHTrigge.Tables[0].Rows[0]["TriggerName"].ToString();
                            dhTriggModel.Description = dsDHTrigge.Tables[0].Rows[0]["Description"].ToString();
                            dhTriggModel.CreatedByName = dsDHTrigge.Tables[0].Rows[0]["CreatedBy"].ToString();
                            dhTriggModel.ModifiedDate = Convert.ToDateTime(dsDHTrigge.Tables[0].Rows[0]["ModifiedDate"].ToString());
                            dhTriggModel.ModifiedByName = dsDHTrigge.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            dhTriggModel.ModifiedDate = Convert.ToDateTime(dsDHTrigge.Tables[0].Rows[0]["ModifiedDate"].ToString());

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dhTriggModel;
        }
        public int SaveDHTrigger(DHTriggerModel DHTriggModel,string User_Id)
        {
            DHTriggerDAL objDAL = new DHTriggerDAL();
            DataSet dsDHTrigge = new DataSet();
            dsDHTrigge = objDAL.DHTriggerGetSelected(0);
            try
            {
                dsDHTrigge.Tables[0].Rows.Clear();
                DataRow dr = dsDHTrigge.Tables[0].NewRow();
                dr["TriggerId"] = DHTriggModel.TriggerId;
                dr["TriggerName"] = DHTriggModel.TriggerName;
                dr["Description"] = DHTriggModel.Description;
                dr["ModifiedBy"] = DHTriggModel.ModifiedBy;
                dsDHTrigge.Tables[0].Rows.Add(dr);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objDAL.SaveDHTrigger(dsDHTrigge, User_Id);
        }

    }
}
