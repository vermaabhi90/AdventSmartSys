using SmartSys.DL.Adhoc;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.Adhoc
{
    public class AdhocBL
    {
        public DataSet GetSelectedAchocList()
        {
            DataSet dsAdhoc = new DataSet();
            AdhocDAL objDAL = new AdhocDAL();
            dsAdhoc = objDAL.GetSelectedAchocList();
            return dsAdhoc;
        }

        public int SaveAdhocRun(DataSet dsAdhoc, string User_Id)
        {
            AdhocDAL objDAL = new AdhocDAL();
            return objDAL.SaveAdhocRun(dsAdhoc, User_Id);
        }

        public DataSet GetSelectedList(int AdhocRunId)
        {
            DataSet dsAdhoc = new DataSet();
            AdhocDAL objDAL = new AdhocDAL();
            dsAdhoc = objDAL.GetSelectedList(AdhocRunId);
            return dsAdhoc;
        }
        public DataSet GetParamList(string ReportId)
        {
            DataSet dsParam = new DataSet();
            try
            {
                AdhocDAL ObjDAL = new AdhocDAL();
                dsParam = ObjDAL.GetParamList(ReportId);
            }
            catch (Exception)
            {

                throw;
            }
            return dsParam;
        }

        public List<AdhocModel> GetList(string user_Id)
        {
            DataSet dsAdhoc = new DataSet();
            List<AdhocModel> lstEvent = new List<AdhocModel>();
            try
            {

                AdhocDAL objDAL = new AdhocDAL();
                dsAdhoc = objDAL.GetList(user_Id);

                if (dsAdhoc != null)
                {
                    if (dsAdhoc.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsAdhoc.Tables[0].Rows)
                        {
                            AdhocModel Adhobj = new AdhocModel();

                            Adhobj.AdhocRunId = Convert.ToInt32(dr["AdhocRunId"].ToString());
                            Adhobj.UserId = Convert.ToInt16(dr["UserId"].ToString());
                            Adhobj.UserName = dr["UserName"].ToString();
                            Adhobj.ReportId = dr["ReportId"].ToString();
                            Adhobj.ReportName = dr["ReportName"].ToString();
                            Adhobj.StatusShortCode = dr["StatusShortCode"].ToString();

                            if (dr["StatusId"].ToString() != "")
                                Adhobj.StatusId = Convert.ToInt16(dr["StatusId"].ToString());
                            if (dr["RGSJobId"].ToString() != "")
                                Adhobj.RGSJobId = Convert.ToInt32(dr["RGSJobId"].ToString());
                            Adhobj.OutputLocation = dr["OutputLocation"].ToString();
                            Adhobj.RunDate = Convert.ToDateTime(dr["RunDate"].ToString());
                            if (dr["GenId"].ToString() != "")
                                Adhobj.GenId = Convert.ToInt16(dr["GenId"].ToString());


                            lstEvent.Add(Adhobj);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstEvent;
        }
        public List<AdhocModel> GetEnqAndQuotItemReportList(string user_Id)
        {
            DataSet dsAdhoc = new DataSet();
            List<AdhocModel> lstEvent = new List<AdhocModel>();
            try
            {

                AdhocDAL objDAL = new AdhocDAL();
                dsAdhoc = objDAL.GetList(user_Id);

                if (dsAdhoc != null)
                {
                    if (dsAdhoc.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsAdhoc.Tables[0].Rows)
                        {
                            if(dr["ReportId"].ToString()=="MGMT015" && dr["User_Id"].ToString() == user_Id)
                            {
                                AdhocModel Adhobj = new AdhocModel();

                                Adhobj.AdhocRunId = Convert.ToInt32(dr["AdhocRunId"].ToString());
                                Adhobj.UserId = Convert.ToInt16(dr["UserId"].ToString());
                                Adhobj.UserName = dr["UserName"].ToString();
                                Adhobj.ReportId = dr["ReportId"].ToString();
                                Adhobj.ReportName = dr["ReportName"].ToString();
                                Adhobj.StatusShortCode = dr["StatusShortCode"].ToString();

                                if (dr["StatusId"].ToString() != "")
                                    Adhobj.StatusId = Convert.ToInt16(dr["StatusId"].ToString());
                                if (dr["RGSJobId"].ToString() != "")
                                    Adhobj.RGSJobId = Convert.ToInt32(dr["RGSJobId"].ToString());
                                Adhobj.OutputLocation = dr["OutputLocation"].ToString();
                                Adhobj.RunDate = Convert.ToDateTime(dr["RunDate"].ToString());
                                if (dr["GenId"].ToString() != "")
                                    Adhobj.GenId = Convert.ToInt16(dr["GenId"].ToString());

                                lstEvent.Add(Adhobj);
                            }
                           
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstEvent;
        }
        public DataSet GetReportNameList(string user_Id)
        {
            AdhocDAL objDAL = new AdhocDAL();
            try
            {

                DataSet dsEvent = new DataSet();
                dsEvent = objDAL.GetReportNameList(user_Id);
                return dsEvent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<AdhocParameterModel> GetViewParamList(int adhocRunId)
        {
           List <AdhocParameterModel> adhocParamModel = new List<AdhocParameterModel>();
            try
            {
                AdhocDAL objDAL = new AdhocDAL();
                DataSet dsParmView = new DataSet();
                dsParmView = objDAL.GetViewParamList(adhocRunId);
                foreach (DataRow dr in dsParmView.Tables[0].Rows)
                {

                    AdhocParameterModel adhocViewParm = new AdhocParameterModel();
                    adhocViewParm.ParamName = dr["ParamName"].ToString();
                    adhocViewParm.ParamValue = dr["ParamValue"].ToString();

                    adhocParamModel.Add(adhocViewParm);
                }

            }
            catch (Exception ex)
            {
                
                Common.LogException(ex);
            }

            return adhocParamModel;
        }
        public List<AdhocModel> FYDropDown()
        {
            DataSet ds = new DataSet();
            List<AdhocModel> lstFyear = new List<AdhocModel>();
            AdhocDAL ObjDal = new AdhocDAL();
            ds = ObjDal.FYDropDown();
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        AdhocModel Mod = new AdhocModel();
                        Mod.FYearId = dr["FYearId"].ToString();
                        Mod.FYear = dr["FYear"].ToString();
                        lstFyear.Add(Mod);
                    }
                }
            }
            return lstFyear;
        }
        public List<ClientEmpCode> GetClientEmpCodeLst(string CompCode)
        {
            List<ClientEmpCode> lstClientEmpModel = new List<ClientEmpCode>();
            try
            {
                DataSet dsObj = new DataSet();
                AdhocDAL DALObj =new AdhocDAL();
                dsObj = DALObj.GetClientEmpCodeLst(CompCode);
                if(dsObj != null)
                {
                    if(dsObj.Tables.Count > 0)
                    {
                        foreach(DataRow dr in dsObj.Tables[0].Rows)
                        {
                            ClientEmpCode clientModel = new ClientEmpCode();
                            clientModel.Emp_No = dr["Emp_No"].ToString();
                            clientModel.Name = dr["Name"].ToString();
                            lstClientEmpModel.Add(clientModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstClientEmpModel;
        }
        public List<ClientCustCode> GetClientCustCodeLst(string CompCode)
        {
            List<ClientCustCode> lstClientCustModel = new List<ClientCustCode>();
            try
            {
                DataSet dsObj = new DataSet();
                AdhocDAL DALObj = new AdhocDAL();
                dsObj = DALObj.GetClientCustCodeLst(CompCode);
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            ClientCustCode clientModel = new ClientCustCode();
                            clientModel.Cust_No = dr["Customer_No"].ToString();
                            clientModel.Cust_Name = dr["CustomerName"].ToString();
                            lstClientCustModel.Add(clientModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstClientCustModel;
        }
    }
}
