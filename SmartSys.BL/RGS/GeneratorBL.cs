using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SmartSys.DAL;
using SmartSys.DL.RGS;
using SmartSys.DL.RGSGen;
using System.Collections;
using SmartSys.Utility;

 
namespace SmartSys.BL.RGS
{
    public class GeneratorBL
    {

        public List<Report> getRGSReportList()
        {
            GeneratorBL objBl = new GeneratorBL();
            System.Data.DataSet ds = objBl.GetReportList();
            List<Report> reportList = new List<Report>();
            foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
            {
                Report reportObj = new Report();
                reportObj.ReportId = dr["ReportId"].ToString();
                reportObj.ReportName = dr["ReportName"].ToString();
                reportObj.BaseReportId = Convert.ToInt16(dr["BaseReportId"].ToString());
                reportObj.BusinessLineId = Convert.ToInt16(dr["BusinessLineId"].ToString());
                reportObj.BusinessLineName = dr["BusinessLineName"].ToString();
                reportObj.ReportType = dr["ReportType"].ToString();
                reportObj.ModifiedBy = dr["ModifiedBy"].ToString();
                reportObj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                reportObj.Version = Convert.ToInt16(dr["Version"].ToString());
                reportList.Add(reportObj);
            }
            return reportList;
        }

        public int GetJobStatus(int JobId)
        {
            try
            {
                GeneratorDal objDL = new GeneratorDal();
                return objDL.GetJobStatus(JobId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetGeneratorDetails(int genId)
        {
            DataSet dsGenDetails = new DataSet();
            try
            {
                GeneratorDal objGenDA = new GeneratorDal();
                dsGenDetails = objGenDA.GetGeneratorDetails(genId);
                return dsGenDetails;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ExecuteUnderlayingSP(DataSet dsParam, string SPName, ArrayList ConInfo)
        {
            DataSet dsRptData = new DataSet();
            try
            {
                GeneratorDal objGenDA = new GeneratorDal();
                dsRptData = objGenDA.ExecuteUnderlayingSP(dsParam, SPName, ConInfo);
                return dsRptData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ExecuteUnderlyingCommand(string cmdText, ArrayList ConInfo)
        {
            DataSet dsRptData = new DataSet();
            try
            {
                GeneratorDal objGenDA = new GeneratorDal();
                dsRptData = objGenDA.ExecuteUnderlyingCommand(cmdText, ConInfo);
                return dsRptData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int SetGeneratorStatus(Int16 genId, Int16 statusId)
        {
            try
            {
                GeneratorDal objGenDA = new GeneratorDal();
                return objGenDA.SetGeneratorStatus(genId, statusId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int SetJobStatus(int JobId, Int16 statusId, string desc, int genId, string opFileName, string opLocation, int opSize)
        {
            try
            {
                GeneratorDal objGenDA = new GeneratorDal();
                return objGenDA.SetJobStatus(JobId, statusId, desc,genId,opFileName,opLocation,opSize);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetGeneratorDetails(string HostName)
        {
            DataSet dsGenDetails = new DataSet();
            try
            {
                GeneratorDal objGenDA = new GeneratorDal();
                dsGenDetails = objGenDA.GetGeneratorDetails(HostName);
                return dsGenDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This Method will call the storedprocedure to set the new ProcessId to the generator config
        /// </summary>
        /// <param name="genId"> Generator Id to update</param>
        /// <param name="processId"> New Process Id which will be allocated to Generator </param>
        /// <returns> Thie Process returns Error Codes. 600003 represents Generator ID not exist, 600001 Represents Successful updation</returns>
        public int RegisterGenerator(int genId, int processId)
        {
            try
            {
                GeneratorDal objGenDA = new GeneratorDal();
                return objGenDA.RegisterGenerator(genId, processId);
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
        
        public DataSet GetNewGenRequest(int genId)
        {
            DataSet dsJobDetails = new DataSet();
            try
            {
                GeneratorDal objGenDA = new GeneratorDal();
                dsJobDetails = objGenDA.GetNewGenRequest(genId);
                return dsJobDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet GetReportDetails(string reportId)
        {
            DataSet dsRptDetails = new DataSet();
            try
            {
                GeneratorDal objGenDA = new GeneratorDal();
                dsRptDetails = objGenDA.GetReportDetails(reportId);
                return dsRptDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet GetReportList()
        {
            DataSet dsRptDetails = new DataSet();
            try
            {
                GeneratorDal objGenDA = new GeneratorDal();
                dsRptDetails = objGenDA.GetReportList();
                return dsRptDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet GetReportTypeList()
        {
            DataSet dsRptDetails = new DataSet();
            try
            {
                GeneratorDal objGenDA = new GeneratorDal();
                dsRptDetails = objGenDA.GetReportTypeList();
                return dsRptDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int SaveReportDetails(DataSet dsReportDetails, string User_Id)
        {
            try
            {
                GeneratorDal objGenDA = new GeneratorDal();
                return objGenDA.SaveReportDetails(dsReportDetails, User_Id);
                

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }





        public List<RGSGenModel> RGSGenGetList()
            {
                List<RGSGenModel> lstSysUser = new List<RGSGenModel>();
                RGSGenDAL objDAL = new RGSGenDAL();
                try
                {

                    DataSet dsRGSGenGetList = new DataSet();
                    dsRGSGenGetList = objDAL.RGSGenGetList();
                    if (dsRGSGenGetList != null)
                    {
                        if (dsRGSGenGetList.Tables.Count > 0)
                        {
                            foreach (DataRow dr in dsRGSGenGetList.Tables[0].Rows)
                            {
                                RGSGenModel objGenModel = new RGSGenModel();
                                objGenModel.GenId = Convert.ToInt16(dr["GenId"].ToString());
                                objGenModel.GenType = dr["GenType"].ToString();
                                objGenModel.Host = dr["Host"].ToString();
                                objGenModel.Port = dr["Port"].ToString();
                                objGenModel.isActive = Convert.ToBoolean(dr["isActive"].ToString());
                                objGenModel.PollingInterval = Convert.ToInt16(dr["PollingInterval"].ToString());
                                objGenModel.BusinessLineId = Convert.ToInt16(dr["BusinessLineId"].ToString());
                                objGenModel.BusinessLineName = dr["BusinessLineName"].ToString();
                                objGenModel.ProcessId = Convert.ToInt32(dr["ProcessId"].ToString());
                                objGenModel.CreatedByName = dr["CreatedByName"].ToString();
                                objGenModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                objGenModel.ModifiedBy = 1;
                                objGenModel.ModifiedByName = dr["ModifiedByName"].ToString();
                                objGenModel.StatusId = Convert.ToInt16(dr["StatusId"].ToString());
                                objGenModel.StatusName = dr["StatusName"].ToString();
                                objGenModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                objGenModel.GenAction = dr["GenAction"].ToString();

                                lstSysUser.Add(objGenModel);
                            }
                        }
                    }
                    return lstSysUser;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        public int SaveGenrator(RGSGenModel rgsGenModel, string User_Id)
            {
                RGSGenDAL objDAL = new RGSGenDAL();
                DataSet dsGen = new DataSet();
                try
                {

                    dsGen = objDAL.GetSelectedGenList(0);
                    if (dsGen != null)
                    {
                        if (dsGen.Tables.Count > 0)
                        {
                            dsGen.Tables[0].Rows.Clear();
                            DataRow dr = dsGen.Tables[0].NewRow();

                            dr["GenId"] = rgsGenModel.GenId;
                            dr["Host"] = rgsGenModel.Host;
                            dr["GenType"] = rgsGenModel.GenType;
                            dr["Port"] = rgsGenModel.Port;
                            dr["isActive"] = rgsGenModel.isActive;
                            dr["BusinessLineId"] = rgsGenModel.BusinessLineId;
                            dr["PollingInterval"] = rgsGenModel.PollingInterval;
                            dr["ModifiedBy"] = rgsGenModel.ModifiedBy;
                            dsGen.Tables[0].Rows.Add(dr);
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                }
                return objDAL.SaveGenrator(dsGen, User_Id);
        
        }
        public RGSGenModel GetSelectedGenList(Int16 GenId)
            {
                RGSGenModel rgsGenModel = new RGSGenModel();
                try
                {
                    RGSGenDAL objDAL = new RGSGenDAL();
                    DataSet dsGen = new DataSet();
                    dsGen = objDAL.GetSelectedGenList(GenId);
                    if (dsGen != null)
            {
                if (dsGen.Tables.Count > 0)
                {
                    if (dsGen.Tables[0].Rows.Count > 0)
                    {
                        rgsGenModel.GenId = GenId;
                        rgsGenModel.GenType = dsGen.Tables[0].Rows[0]["GenType"].ToString();
                        rgsGenModel.Host = dsGen.Tables[0].Rows[0]["Host"].ToString();
                        rgsGenModel.BusinessLineId = Convert.ToInt16(dsGen.Tables[0].Rows[0]["BusinessLineId"].ToString());
                        rgsGenModel.isActive = Convert.ToBoolean(dsGen.Tables[0].Rows[0]["isActive"].ToString());
                        rgsGenModel.Port = dsGen.Tables[0].Rows[0]["Port"].ToString();
                        rgsGenModel.PollingInterval = Convert.ToInt16(dsGen.Tables[0].Rows[0]["PollingInterval"].ToString());
                        rgsGenModel.ProcessId = Convert.ToInt16(dsGen.Tables[0].Rows[0]["ProcessId"].ToString());
                        rgsGenModel.CreatedByName = dsGen.Tables[0].Rows[0]["CreatedByName"].ToString();
                        rgsGenModel.ModifiedByName = dsGen.Tables[0].Rows[0]["ModifiedByName"].ToString();
                        rgsGenModel.CreatedDate = Convert.ToDateTime(dsGen.Tables[0].Rows[0]["CreatedDate"].ToString());
                        rgsGenModel.ModifiedDate = Convert.ToDateTime(dsGen.Tables[0].Rows[0]["ModifiedDate"].ToString());

                    }
                }
            }
         
              }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                }
                return (rgsGenModel);

            }

        public List<BusinessLine> GetBussList()
            {
                List<BusinessLine> lstBuss = new List<BusinessLine>();
                try
                {                   
                   DataSet dsBussLine = new DataSet();
                    RGSGenDAL objDAL = new RGSGenDAL();
                    dsBussLine = objDAL.GetBussList();
                    if (dsBussLine != null)
                    {
                        if (dsBussLine.Tables.Count > 0)
                        {

                            foreach (DataRow dr in dsBussLine.Tables[0].Rows)
                            {
                                BusinessLine lstbl = new BusinessLine();
                                lstbl.BusinessLineId = Convert.ToInt16(dr["BusinessLineId"].ToString());
                                lstbl.BusinessLineName = dr["BusinessLineName"].ToString();
                                lstBuss.Add(lstbl);
                            }
                        }
                    }
                  
                }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                }
                return lstBuss;
            }

        public DataSet GetBaseReports()
        {
            GeneratorDal objGen = new GeneratorDal();
            DataSet dsBaseReport = objGen.GetBaseReports();
            return dsBaseReport;
        }
        #region RDS Report
   
        public int SaveRDSRpt(Report rptmodel, string User_Id)
        {
            int errcode = 0;
            try
            {
                GeneratorDal objdal = new GeneratorDal();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedRDSRptList("");
                if (ds != null)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["ReportId"] = rptmodel.ReportId;
                    dr["ReportName"] = rptmodel.ReportName;
                    dr["ReportType"] = rptmodel.ReportType;
                    dr["BusinessLineId"] = rptmodel.BusinessLineId;

               
                   

                    ds.Tables[0].Rows.Add(dr);
                    errcode = objdal.SaveRDSReport(ds, User_Id);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return errcode;
        }

        public Report GetSelectedRDSList(string ReportId)
        {
            Report objmodel = new Report();
            try
            {
                GeneratorDal objdal = new GeneratorDal();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedRDSRptList(ReportId);
                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            objmodel.ReportId = ds.Tables[0].Rows[0]["ReportId"].ToString();
                            objmodel.ReportName = ds.Tables[0].Rows[0]["ReportName"].ToString();
                            objmodel.ReportType = ds.Tables[0].Rows[0]["ReportType"].ToString();
                            objmodel.BusinessLineId = Convert.ToInt16(ds.Tables[0].Rows[0]["BusinessLineId"]);
                            objmodel.CreatedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            objmodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            objmodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objmodel;
        }
       #endregion RDS Report
    }
}
