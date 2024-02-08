using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SmartSys.DAL;
using SmartSys.Utility;

namespace SmartSys.BL.DataHub
{
    public class DHEventBL
    {
        public List<DHEvent> GetDHEventList()
        {
            List<DHEvent> lstDhEvent = new List<DHEvent>();
            try
            {
                DHEventDAL objEvent = new DHEventDAL();
                DataSet dsDHEvent = new DataSet();
                dsDHEvent = objEvent.GetDHEventList();
                if (dsDHEvent != null)
                {
                    if (dsDHEvent.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDHEvent.Tables[0].Rows)
                        {
                            DHEvent dheventobj = new DHEvent();
                            dheventobj.EventId = Convert.ToInt16(dr["EventId"].ToString());
                            dheventobj.EventName = dr["EventName"].ToString();
                            dheventobj.Description = dr["Description"].ToString();
                            dheventobj.isDeleted = Convert.ToBoolean(dr["isDeleted"].ToString());
                            lstDhEvent.Add(dheventobj);
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstDhEvent;
        }

        public DHEvent DHEventGetSelected(Int16 eventId)
        {
            DHEvent dheventobj = new DHEvent();
            try
            {
                DHEventDAL objDAL = new DHEventDAL();
                DataSet dsDHEvent = new DataSet();
                dsDHEvent = objDAL.DHEventGetSelected(eventId);
                if (dsDHEvent != null)
                {
                    if (dsDHEvent.Tables.Count > 0)
                    {
                        if (dsDHEvent.Tables[0].Rows.Count > 0)
                        {
                            dheventobj.EventId = eventId;
                            dheventobj.EventName = dsDHEvent.Tables[0].Rows[0]["EventName"].ToString();
                            dheventobj.Description = dsDHEvent.Tables[0].Rows[0]["Description"].ToString();
                            dheventobj.isDeleted = Convert.ToBoolean(dsDHEvent.Tables[0].Rows[0]["isDeleted"].ToString());
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dheventobj;
        }

        public int SaveDHEvent(DHEvent dhEventModel)
        {
            DHEventDAL objDAL = new DHEventDAL();
            DataSet dsDHEvent=new DataSet();
            dsDHEvent = objDAL.DHEventGetSelected(0);
            try
            {
                dsDHEvent.Tables[0].Rows.Clear();
                DataRow dr = dsDHEvent.Tables[0].NewRow();
                dr["EventId"] = dhEventModel.EventId;
                dr["EventName"] = dhEventModel.EventName;
                dr["Description"] = dhEventModel.Description;
                dr["isDeleted"] = dhEventModel.isDeleted;
                dsDHEvent.Tables[0].Rows.Add(dr);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objDAL.SaveDHEvent(dsDHEvent);
        }

    }
}
