using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSys.DL.RGS;
using SmartSys.DL.RGSInsMgr;
using SmartSys.Utility;

namespace SmartSys.BL.RGS
{
    public class InstanceManagerBL
    {
        public DataSet GetInstanceManagerByHost(string HostName)
        {
            try
            {
                InstanceManagerDAL objDL = new InstanceManagerDAL();
                DataSet dsIM = objDL.GetInstanceManagerByHost(HostName);
                return dsIM;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int SetInstanceManagerProcessId(int IMId, int processId)
        {
            try
            {
                InstanceManagerDAL objDL = new InstanceManagerDAL();
                return objDL.SetInstanceManagerProcessId(IMId, processId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<InstanceManagerModel> GetInstMgrList()
        {
            List<InstanceManagerModel> lstInstMgr = new List<InstanceManagerModel>();
            RGSInsMgrDAL objDAL = new RGSInsMgrDAL();
            try
            {

                DataSet dsInstMgr = new DataSet();
                dsInstMgr = objDAL.GetInstMgrList();
                if (dsInstMgr != null)
                {
                    if (dsInstMgr.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsInstMgr.Tables[0].Rows)
                        {
                            InstanceManagerModel InstMgrobj = new InstanceManagerModel();
                            InstMgrobj.IMId = Convert.ToInt16(dr["IMId"].ToString());
                            InstMgrobj.Host = dr["Host"].ToString();
                            InstMgrobj.Port = dr["Port"].ToString();
                            InstMgrobj.isPrimary = Convert.ToBoolean(dr["isPrimary"].ToString());
                            InstMgrobj.PollingInterval = Convert.ToInt32(dr["PollingInterval"].ToString());
                            InstMgrobj.ProcessId = Convert.ToInt32(dr["ProcessId"].ToString());
                            InstMgrobj.CreatedByName = dr["CreatedByName"].ToString();
                            InstMgrobj.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            InstMgrobj.ModifiedBy = 1;
                            InstMgrobj.ModifiedByName = dr["ModifiedByName"].ToString();
                            InstMgrobj.StatusId = Convert.ToInt16(dr["StatusId"].ToString());
                            InstMgrobj.StatusName = dr["StatusShortCode"].ToString();
                            InstMgrobj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());

                            lstInstMgr.Add(InstMgrobj);
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstInstMgr;
        }
        public int SaveInstMgr(InstanceManagerModel InstMgrModel, string User_Id)
        {
            RGSInsMgrDAL objDAL = new RGSInsMgrDAL();
            DataSet dsInstMgr = new DataSet();
            try
            {
                        dsInstMgr = objDAL.InstMgrGetSelected(0);
                        dsInstMgr.Tables[0].Rows.Clear();
                        DataRow dr = dsInstMgr.Tables[0].NewRow();

                        dr["IMId"] = InstMgrModel.IMId;
                        dr["Host"] = InstMgrModel.Host;
                        dr["Port"] = InstMgrModel.Port;
                        dr["isPrimary"] = InstMgrModel.isPrimary;
                        dr["PollingInterval"] = InstMgrModel.PollingInterval;
                        dr["ModifiedBy"] = InstMgrModel.ModifiedBy;
                        dr["ProcessId"] = InstMgrModel.ProcessId;
                        dsInstMgr.Tables[0].Rows.Add(dr);  
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objDAL.SaveInstMgr(dsInstMgr, User_Id);
        }
        public InstanceManagerModel InstMgrGetSelected(Int16 IMId)
        {
            InstanceManagerModel InstMgrModel = new InstanceManagerModel();
            try
            {
                RGSInsMgrDAL objEvent = new RGSInsMgrDAL();
                DataSet dsInstMgr = new DataSet();
                dsInstMgr = objEvent.InstMgrGetSelected(IMId);
                if (dsInstMgr != null)
                {
                    if (dsInstMgr.Tables.Count > 0)
                    {
                        if (dsInstMgr.Tables[0].Rows.Count > 0)
                        {
                            InstMgrModel.IMId = IMId;
                            InstMgrModel.Host = dsInstMgr.Tables[0].Rows[0]["Host"].ToString();
                            InstMgrModel.isPrimary = Convert.ToBoolean(dsInstMgr.Tables[0].Rows[0]["isPrimary"].ToString());
                            InstMgrModel.Port = dsInstMgr.Tables[0].Rows[0]["Port"].ToString();
                            InstMgrModel.PollingInterval = Convert.ToInt32(dsInstMgr.Tables[0].Rows[0]["PollingInterval"].ToString());
                            InstMgrModel.ProcessId = Convert.ToInt32(dsInstMgr.Tables[0].Rows[0]["ProcessId"].ToString());
                            InstMgrModel.CreatedByName = dsInstMgr.Tables[0].Rows[0]["CreatedByName"].ToString();
                            InstMgrModel.ModifiedByName = dsInstMgr.Tables[0].Rows[0]["ModifiedByName"].ToString();
                            InstMgrModel.CreatedDate = Convert.ToDateTime(dsInstMgr.Tables[0].Rows[0]["CreatedDate"].ToString());
                            InstMgrModel.ModifiedDate = Convert.ToDateTime(dsInstMgr.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
               Common.LogException(ex);
            }
            return InstMgrModel;
        }
    }
}
