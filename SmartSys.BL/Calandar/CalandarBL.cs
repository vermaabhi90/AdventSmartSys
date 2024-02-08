using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSys.DL.Calandar;
using System.Data;
using SmartSys.Utility;

namespace SmartSys.BL.Calandar
{
    public class CalandarBL
    {
        #region Plan
        public List<DefaultSchedule> GetPlanList(string User_Id)
        {
            List<DefaultSchedule> lstCalendar = new List<DefaultSchedule>();
            try
            {
                CalandarDAL objdal = new CalandarDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetPlanList(User_Id);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Boolean Recur = true;
                            if (dr["Recurrence"].ToString() == "0")
                                Recur = false;
                            DefaultSchedule objSch = new DefaultSchedule(
                                Convert.ToInt32(dr["ScheduleId"].ToString()),
                                dr["Subject"].ToString(),
                                dr["Location"].ToString(),
                                dr["StartTime"].ToString(),
                                dr["EndTime"].ToString(),
                                dr["Description"].ToString(),
                                dr["Owner"].ToString(),
                                dr["ParticipantId"].ToString(),
                                dr["Priority"].ToString(),
                                Recur,
                                dr["RecurrenceType"].ToString(),
                                dr["RecurrenceTypeCount"].ToString(),
                                dr["Categorize"].ToString(),
                                dr["CustomStyle"].ToString(),

                                Convert.ToBoolean(dr["AllDay"].ToString()),
                                dr["RecurrenceStartDate"].ToString(),
                                dr["RecurrenceEndDate"].ToString(),
                                dr["RecurrenceRule"].ToString(),
                                dr["StartTimeZone"].ToString(),
                                 dr["EndTimeZone"].ToString()
                                );
                            lstCalendar.Add(objSch);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstCalendar;
        }

        #endregion Plan

        #region Calendar Work
        public List<DefaultSchedule> GetCalenderList(string User_Id)
        {
            List<DefaultSchedule> lstCalendar = new List<DefaultSchedule>();
            try
            {
                CalandarDAL objdal = new CalandarDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetCalendarList(User_Id);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Boolean Recur = true;
                            if (dr["Recurrence"].ToString() == "0")
                                Recur = false;
                            DefaultSchedule objSch = new DefaultSchedule(
                                Convert.ToInt32(dr["ScheduleId"].ToString()),
                                dr["Subject"].ToString(),
                                dr["Location"].ToString(),
                                dr["StartTime"].ToString(),
                                dr["EndTime"].ToString(),
                                dr["Description"].ToString(),
                                dr["Owner"].ToString(),
                                dr["ParticipantId"].ToString(),
                                dr["Priority"].ToString(),
                                Recur,
                                dr["RecurrenceType"].ToString(),
                                dr["RecurrenceTypeCount"].ToString(),
                                dr["Categorize"].ToString(),
                                dr["CustomStyle"].ToString(),
                                
                                Convert.ToBoolean(dr["AllDay"].ToString()),
                                dr["RecurrenceStartDate"].ToString(),
                                dr["RecurrenceEndDate"].ToString(),
                                dr["RecurrenceRule"].ToString(),
                                dr["StartTimeZone"].ToString(),
                                 dr["EndTimeZone"].ToString()
                                );
                            lstCalendar.Add(objSch);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstCalendar;
        }

        public int SaveCalendarDetails(DefaultSchedule Model,string User_Id,List<int> LstOwner)
        {
            int errcode = 0;
            TimeSpan x = Model.StartTime.TimeOfDay;
            TimeSpan y = Model.EndTime.TimeOfDay;
            TimeSpan test = y - x;
            if (test.ToString() =="23:59:59")
            {
               
            }
            try
            {
                CalandarDAL DalObj = new CalandarDAL();
                DataSet dsSchedule = new DataSet();
                DataTable dtTMSchedule = new DataTable("tbl_RDSRptSubscription");
                dtTMSchedule.Columns.Add("ScheduleId", typeof(System.Int32));
                dtTMSchedule.Columns.Add("Subject", typeof(System.String));
                dtTMSchedule.Columns.Add("Location", typeof(System.String));
                dtTMSchedule.Columns.Add("StartTime", typeof(System.DateTime));
                dtTMSchedule.Columns.Add("EndTime", typeof(System.DateTime));
                dtTMSchedule.Columns.Add("Description", typeof(System.String));
                dtTMSchedule.Columns.Add("Owner", typeof(System.Int32));
                dtTMSchedule.Columns.Add("Priority", typeof(System.Int32));
                dtTMSchedule.Columns.Add("Recurrence", typeof(System.Int32));
                dtTMSchedule.Columns.Add("RecurrenceRule", typeof(System.String));
                dtTMSchedule.Columns.Add("AllDay", typeof(System.Boolean));
                dtTMSchedule.Columns.Add("StartTimeZone", typeof(System.String));
                dtTMSchedule.Columns.Add("EndTimeZone", typeof(System.String));


                DataRow drTMSchedule = dtTMSchedule.NewRow();
                drTMSchedule["ScheduleId"] = Convert.ToInt32(Model.Id);
                drTMSchedule["Subject"] = Model.Subject;
                drTMSchedule["Location"] = Model.Location;
                drTMSchedule["StartTime"] = Convert.ToDateTime(Model.StartTime);
                drTMSchedule["EndTime"] = Convert.ToDateTime(Model.EndTime);
                drTMSchedule["Description"] = Model.Description;
                drTMSchedule["Owner"] = Convert.ToInt32(Model.Owner);
                drTMSchedule["Priority"] = Convert.ToInt32(Model.Priority);
                drTMSchedule["Recurrence"] = Convert.ToInt32(Model.Recurrence);
                drTMSchedule["RecurrenceRule"] = Model.RecurrenceRule;
                drTMSchedule["AllDay"] = Convert.ToBoolean(Model.AllDay);
                drTMSchedule["StartTimeZone"] = Model.StartTimeZone;
                drTMSchedule["EndTimeZone"] = Model.EndTimeZone;
                drTMSchedule["RecurrenceRule"] = Model.RecurrenceRule;
                dtTMSchedule.Rows.Add(drTMSchedule);

                dsSchedule.Tables.Add(dtTMSchedule);
                errcode = DalObj.SaveTMCalendarDetails(dsSchedule, User_Id, LstOwner);
            }
            catch (Exception)
            {
                
                throw;
            }
            return errcode;
        }

        public int DeleteCalendarAppointment(int id, string User_Id)
        {
            int errcode = 0;
            try
            {
                 CalandarDAL DalObj = new CalandarDAL();
                 errcode = DalObj.DeleteCalendarAppointment(id, User_Id);
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return errcode;
        }

        public List<string> GetScheduleParticipendsDetailList(int id)
        {
            DataSet ds = new DataSet();
            string name = "";
            string Email = "";
            List<string> mailInfo = new List<string>();
            try
            {
                CalandarDAL DalObj = new CalandarDAL();
                ds = DalObj.GetScheduleParticipendsDetailList(id);
                foreach(DataRow  dr in ds.Tables[0].Rows)
                {
                    if (name != "")
                    {
                        name = name + ", ";
                    }
                    name = name + dr["EmpName"].ToString();
                    if (Email != "")
                    {
                        Email = Email + ",";
                    }
                    Email = Email + dr["emailId"].ToString(); 
                }
                mailInfo.Add(name.Trim());
                mailInfo.Add(Email.Trim());
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return mailInfo;
        }
        #endregion Calendar Work
    }
}
