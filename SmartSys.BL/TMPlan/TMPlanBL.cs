using SmartSys.DL.TMPlan;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.TMPlan
{
    public class TMPlanBL
    {
        public List<EmpPlanList> GetTMPlanEmplist(string User_ID)
        {
            List<EmpPlanList> lstEmp = new List<EmpPlanList>();
            DataSet dsMenu = new DataSet();
            try
            {
                TMPlanDAL DAlObj = new TMPlanDAL();
                dsMenu = DAlObj.GetTMPlanEmplist(User_ID);
                if (dsMenu == null) return null;
                foreach (DataRow dr in dsMenu.Tables[0].Rows)
                {
                    EmpPlanList sysTaskModel = new EmpPlanList();
                    sysTaskModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                    sysTaskModel.EmpName = dr["EmpName"].ToString();
                    lstEmp.Add(sysTaskModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstEmp;
        }
        public List<EmpSubordinatesModel> EmpSubordinatesList(string User_ID)
        {
            List<EmpSubordinatesModel> Emplist = new List<EmpSubordinatesModel>();
            DataSet ds = new DataSet();
            try
            {
                TMPlanDAL dalobj = new TMPlanDAL();
                ds = dalobj.GetEmpSubordinatesList(User_ID);
                if (ds == null) return null;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    EmpSubordinatesModel Model = new EmpSubordinatesModel();
                    Model.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                    Model.EmpName = dr["EmpName"].ToString();
                    Emplist.Add(Model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Emplist;
        }
        public ReportNameModel GetOpenReport(string user_Id, int EmpCode)
        {
            ReportNameModel Model = new ReportNameModel();
            try
            {
                TMPlanDAL objDAL = new TMPlanDAL();
                DataSet ds = new DataSet();
                ds = objDAL.GetTMPlanEmplistReportInView(user_Id, EmpCode);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Model.OutputLocation = ds.Tables[0].Rows[0]["OutputLocation"].ToString();
                            Model.RunDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["RunDate"].ToString());
                            Model.RunEmpName = ds.Tables[0].Rows[0]["RunEmpName"].ToString();
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
        public List<TMPlanModel> GetTMPlanCalLst(string User_ID)
        {
            List<TMPlanModel> LstPlncal = new List<TMPlanModel>();
            DataSet dsMenu = new DataSet();
            try
            {
                TMPlanDAL DAlObj = new TMPlanDAL();
                dsMenu = DAlObj.GetCalLst(User_ID);
                if (dsMenu == null) return null;
                foreach (DataRow dr in dsMenu.Tables[0].Rows)
                {
                    TMPlanModel Model = new TMPlanModel();
                    Model.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                    Model.PlanId = Convert.ToInt32(dr["PlanId"].ToString());
                    Model.ProjectName = dr["ProjectName"].ToString();
                    Model.TaskName = dr["TaskName"].ToString();
                    Model.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                    Model.OnDate = Convert.ToDateTime(dr["OnDate"].ToString());
                    Model.AllDay = Convert.ToBoolean(dr["AllDay"].ToString());
                    Model.Repete = Convert.ToBoolean(dr["Repete"].ToString());
                    Model.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                    Model.ModifiedBy = dr["ModifiedBy"].ToString();
                    Model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                    Model.statusId = Convert.ToInt32(dr["Status"].ToString());
                    Model.status = dr["StatusShortCode"].ToString();
                    Model.Remark = dr["Remark"].ToString();
                    switch (Convert.ToChar(dr["FrequencyType"].ToString()))
                    {
                        case 'D':
                            {
                                Model.FrequencyTypeDetail = "Daily";
                                break;
                            }
                        case 'W':
                            {
                                Model.FrequencyTypeDetail = "Weekly";
                                break;
                            }
                        case 'M':
                            {
                                Model.FrequencyTypeDetail = "Monthly";
                                break;
                            }
                        case 'Y':
                            {
                                Model.FrequencyTypeDetail = "Yearly";
                                break;
                            }
                        default:
                            {
                                Model.FrequencyTypeDetail = "";
                                break;
                            }
                    }
                    LstPlncal.Add(Model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstPlncal;
        }
        public int DeletePlan(int PlanId, string User_id)
        {
            int errCode = 0;
            try
            {
                TMPlanDAL DALObj = new TMPlanDAL();
                errCode = DALObj.DeletePlan(PlanId, User_id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet PlanGetEmpDetail(int EmpId, string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                TMPlanDAL DALObj = new TMPlanDAL();
                ds = DALObj.PlanGetEmpDetail(EmpId, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public TMPlanModel GetTMPlanSelectedList(int PlanId)
        {
            TMPlanModel PlanModel = new TMPlanModel();
            DataSet ds = new DataSet();
            try
            {
                TMPlanDAL DAlObj = new TMPlanDAL();
                ds = DAlObj.GetTMPlanSelectedList(PlanId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                PlanModel.PlanId = Convert.ToInt32(dr["PlanId"].ToString());
                                PlanModel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                                PlanModel.EmpName = dr["EmpName"].ToString();
                                PlanModel.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                                PlanModel.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                                PlanModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                                PlanModel.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                                PlanModel.AllDay = Convert.ToBoolean(dr["AllDay"].ToString());
                                PlanModel.Repete = Convert.ToBoolean(dr["Repete"].ToString());
                                PlanModel.FrequencyType = Convert.ToChar(dr["FrequencyType"].ToString());
                                PlanModel.WeekDays = dr["WeekDays"].ToString();
                                PlanModel.Remark = dr["Remark"].ToString();
                                PlanModel.WeekNo = Convert.ToInt32(dr["WeekNo"].ToString());
                                PlanModel.MonthNo = Convert.ToInt32(dr["MonthNo"].ToString());
                                PlanModel.Never = Convert.ToBoolean(dr["Never"].ToString());
                                PlanModel.OnDate = Convert.ToDateTime(dr["OnDate"].ToString());
                                string[] WeekOption = dr["RecurrenceRule"].ToString().Split(';');
                                foreach (string str in WeekOption)
                                {
                                    if (str.Length > 5)
                                    {
                                        string temp = str.Substring(0, 5);
                                        if (temp == "BYDAY")
                                        {
                                            temp = str.Substring(str.IndexOf("=") + 1, (str.Length - (str.IndexOf("=") + 1)));
                                            PlanModel.WeekMonth = temp;
                                        }
                                    }
                                }
                                string word = PlanModel.WeekDays;
                                if (word[0] == '1')
                                {
                                    PlanModel.Sunday = true;
                                }
                                else
                                {
                                    PlanModel.Sunday = false;
                                }
                                if (word[1] == '1')
                                {
                                    PlanModel.Monday = true;
                                }
                                else
                                {
                                    PlanModel.Monday = false;
                                }
                                if (word[2] == '1')
                                {
                                    PlanModel.TuesDay = true;
                                }
                                else
                                {
                                    PlanModel.TuesDay = false;
                                }
                                if (word[3] == '1')
                                {
                                    PlanModel.Wednesday = true;
                                }
                                else
                                {
                                    PlanModel.Wednesday = false;
                                }

                                if (word[4] == '1')
                                {
                                    PlanModel.Thirsday = true;
                                }
                                else
                                {
                                    PlanModel.Thirsday = false;
                                }

                                if (word[5] == '1')
                                {
                                    PlanModel.Friday = true;
                                }
                                else
                                {
                                    PlanModel.Friday = false;
                                }

                                if (word[6] == '1')
                                {
                                    PlanModel.Saturday = true;
                                }
                                else
                                {
                                    PlanModel.Saturday = false;
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PlanModel;
        }
        public int saveTMPlanDetails(TMPlanModel PlanModel, string User_Id)
        {

            TMPlanDAL objdal = new TMPlanDAL();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetTMPlanSelectedList(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow Dr = ds.Tables[0].NewRow();
                    Dr["PlanId"] = PlanModel.PlanId;
                    Dr["EmpId"] = PlanModel.EmpId;
                    Dr["StartDate"] = PlanModel.StartDate;
                    Dr["EndDate"] = PlanModel.EndDate;
                    Dr["ProjectId"] = PlanModel.ProjectId;
                    Dr["TaskId"] = PlanModel.TaskId;
                    Dr["AllDay"] = PlanModel.AllDay;
                    Dr["Repete"] = PlanModel.Repete;
                    Dr["FrequencyType"] = PlanModel.FrequencyType;
                    Dr["WeekDays"] = PlanModel.WeekDays;
                    Dr["WeekNo"] = PlanModel.WeekNo;
                    Dr["MonthNo"] = PlanModel.MonthNo;
                    Dr["RecurrenceRule"] = PlanModel.RecurrenceRule;
                    Dr["Never"] = PlanModel.Never;
                    Dr["OnDate"] = PlanModel.OnDate;
                    Dr["Remark"] = PlanModel.Remark;
                    Dr["DocType"] = PlanModel.DocType;
                    Dr["DocId"] = PlanModel.DocId;
                    Dr["DocRefId"] = PlanModel.DocRefId;
                    ds.Tables[0].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.SaveTMPlanDetails(ds, User_Id);
        }
        public int SaveTMPlanStatus(int PlanId, int status, string User_id)
        {
            int errCode = 0;
            try
            {
                TMPlanDAL DALObj = new TMPlanDAL();
                errCode = DALObj.SaveTMPlanStatus(PlanId, status, User_id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public List<TMPlanModel> GetPlanDataByDate(int EmpId,DateTime Date)
        {
            List<TMPlanModel> LstPlncal = new List<TMPlanModel>();
            DataSet dsMenu = new DataSet();
            try
            {
                TMPlanDAL DAlObj = new TMPlanDAL();
                dsMenu = DAlObj.GetPlanDataByDate(EmpId, Date);
                if (dsMenu == null) return null;
                foreach (DataRow dr in dsMenu.Tables[0].Rows)
                {
                    TMPlanModel Model = new TMPlanModel();
                    Model.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                    Model.PlanId = Convert.ToInt32(dr["PlanId"].ToString());
                    Model.ProjectName = dr["ProjectName"].ToString();
                    Model.TaskName = dr["TaskName"].ToString();
                    Model.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                    Model.StrStartDate = Model.StartDate.ToShortDateString() + " " + Model.StartDate.ToShortTimeString();
                    Model.OnDate = Convert.ToDateTime(dr["OnDate"].ToString());
                    Model.AllDay = Convert.ToBoolean(dr["AllDay"].ToString());
                    Model.Repete = Convert.ToBoolean(dr["Repete"].ToString());
                    Model.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                    Model.StrEndDate = Model.EndDate.ToShortDateString() + " " + Model.EndDate.ToShortTimeString();
                    Model.ModifiedBy = dr["ModifiedBy"].ToString();
                    Model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                    Model.statusId = Convert.ToInt32(dr["Status"].ToString());
                    Model.status = dr["StatusShortCode"].ToString();
                    Model.Remark = dr["Remark"].ToString();
                    switch (Convert.ToChar(dr["FrequencyType"].ToString()))
                    {
                        case 'D':
                            {
                                Model.FrequencyTypeDetail = "Daily";
                                break;
                            }
                        case 'W':
                            {
                                Model.FrequencyTypeDetail = "Weekly";
                                break;
                            }
                        case 'M':
                            {
                                Model.FrequencyTypeDetail = "Monthly";
                                break;
                            }
                        case 'Y':
                            {
                                Model.FrequencyTypeDetail = "Yearly";
                                break;
                            }
                        default:
                            {
                                Model.FrequencyTypeDetail = "";
                                break;
                            }
                    }
                    LstPlncal.Add(Model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstPlncal;
        }
    }
}
