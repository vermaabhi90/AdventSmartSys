using SmartSys.DAL;
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
    public class RDSClientBL
    {
    public List<RDSClientModel> RDSGetClientList()
        {
            List<RDSClientModel> rdsClientList = new List<RDSClientModel>();
            try
            {
                RDSClientDAL DALObj = new RDSClientDAL();
                DataSet dsRDSClient = new DataSet();
                dsRDSClient = DALObj.RDSGetClientList();
                if (dsRDSClient == null)
                    return null;
                if(dsRDSClient.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow dr in dsRDSClient.Tables[0].Rows)
                    {
                        RDSClientModel lstClient = new RDSClientModel();
                        lstClient.ClientId =Convert.ToInt32(dr["ClientId"].ToString());
                        lstClient.ClientName = dr["ClientName"].ToString();
                        lstClient.ClientType = dr["ClientType"].ToString();
                        lstClient.ClientRefId = dr["ClientRefId"].ToString();
                        lstClient.email = dr["email"].ToString();
                        lstClient.FTPDetails = dr["FTPDetails"].ToString();
                        lstClient.ModifiedByName = dr["ModifiedByName"].ToString();
                        lstClient.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        lstClient.CreatedByName = dr["CreatedByName"].ToString();
                        lstClient.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        rdsClientList.Add(lstClient);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return rdsClientList;
        }


    public RDSClientModel RDSGetSelectedClient(int ClientId )
    {
        RDSClientModel lstClient = new RDSClientModel();
        try
        {
            RDSClientDAL DALObj = new RDSClientDAL();
            DataSet dsRDSClient = new DataSet();
            dsRDSClient = DALObj.RDSGetSelectedClient(ClientId);
            if(dsRDSClient.Tables[0].Rows[0]["FTPDetails"].ToString() !="")
            {
                string str = dsRDSClient.Tables[0].Rows[0]["FTPDetails"].ToString();
                string[] FTPdemo = str.Split(',');
                if (str != null)
                {
                    lstClient.Server = FTPdemo[0];
                    lstClient.UserName = FTPdemo[1];
                    lstClient.Password = FTPdemo[2];
                }
            }
           
            if (dsRDSClient == null)
                return null;
            if (dsRDSClient.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsRDSClient.Tables[0].Rows)
                {
                   
                    lstClient.ClientId = Convert.ToInt32(dr["ClientId"].ToString());
                    lstClient.ClientName = dr["ClientName"].ToString();
                    lstClient.ClientType = dr["ClientType"].ToString();
                    lstClient.ClientRefId = dr["ClientRefId"].ToString();
                    lstClient.email = dr["email"].ToString();
                    lstClient.ModifiedByName = dr["ModifiedByName"].ToString();
                    lstClient.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                    lstClient.CreatedByName = dr["CreatedByName"].ToString();
                    lstClient.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                   
                }
            }

        }
        catch (Exception ex)
        {
            Common.LogException(ex);
        }
        return lstClient;
    }
    public int SaveRDSClient(RDSClientModel rdsClientModel, string User_Id)
    {

        RDSClientDAL objDAL = new RDSClientDAL();
        DataSet dsrdsClient = new DataSet();
        try
        {
            dsrdsClient = objDAL.RDSGetSelectedClient(0);
            if (dsrdsClient != null)
            {
                if (dsrdsClient.Tables.Count > 0)
                {
                    dsrdsClient.Tables[0].Rows.Clear();
                    DataRow dr = dsrdsClient.Tables[0].NewRow();

                    dr["ClientId"] = rdsClientModel.ClientId;
                    dr["ClientName"] = rdsClientModel.ClientName;
                    dr["ClientType"] = rdsClientModel.ClientType;
                    dr["ClientRefId"] = rdsClientModel.ClientRefId;
                    dr["email"] = rdsClientModel.email;
                    dr["FTPDetails"] = rdsClientModel.FTPDetails;
                    dr["ModifiedBy"] = rdsClientModel.ModifiedBy;
                    dsrdsClient.Tables[0].Rows.Add(dr);

                }
            }
        }
        catch (Exception ex)
        {
            Common.LogException(ex);
        }
        return objDAL.SaveRDSClient(dsrdsClient, User_Id);
    }

    }
}
