using SmartSys.BL.ProViews;
using SmartSys.DL.TMLeave;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.Tmleave
{
  public  class TMLeaveBL
    {
        #region TMLeaveType
        public List<TMLeaveModel> GetTMList(string UserId)
        {

            List<TMLeaveModel> TMLeavelst = new List<TMLeaveModel>();

            try
            {
                TMLeaveDal objdal = new TMLeaveDal();
                DataSet ds = new DataSet();
                ds = objdal.GetTMList(UserId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            TMLeaveModel objmodel = new TMLeaveModel();
                            objmodel.LeaveTypeId = Convert.ToInt32(dr["LeaveTypeId"].ToString());
                            objmodel.LeaveType = dr["LeaveType"].ToString();
                            objmodel.Description = dr["Description"].ToString();
                            objmodel.IsPaid = Convert.ToBoolean(dr["IsPaid"].ToString());
                            objmodel.YearlyQuota = Convert.ToInt32(dr["YearlyQuota"].ToString());
                            objmodel.CanLaps = Convert.ToBoolean(dr["CanLaps"].ToString());
                            objmodel.MaxLeaveCarryover = Convert.ToInt32(dr["MaxLeaveCarryover"].ToString());
                            objmodel.Enable = Convert.ToBoolean(dr["Enable"].ToString());
                            objmodel.CreatedByName = dr["CreatedBy"].ToString();
                            objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            objmodel.ModifiedByName = dr["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            TMLeavelst.Add(objmodel);

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TMLeavelst;
        }

        public int SaveTMLeave(TMLeaveModel objmodel, string User_Id)
        {
            int errcode = 0;
            try
            {
                TMLeaveDal objdal = new TMLeaveDal();
                DataSet ds = new DataSet();
                ds = objdal.GetselectedTMLeaveList(0);
                if (ds != null)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["LeaveTypeId"] = objmodel.LeaveTypeId;
                    dr["LeaveType"] = objmodel.LeaveType;
                    dr["Description"] = objmodel.Description;
                    dr["IsPaid"] = objmodel.IsPaid;
                    dr["YearlyQuota"] = objmodel.YearlyQuota;
                    dr["CanLaps"] = objmodel.CanLaps;
                    dr["MaxLeaveCarryover"] = objmodel.MaxLeaveCarryover;
                    dr["Enable"] = objmodel.Enable;
                    ds.Tables[0].Rows.Add(dr);
                    errcode = objdal.SaveTMLeave(ds, User_Id);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return errcode;
        }
        public TMLeaveModel GetSelectedTMLeaveList(int LeaveTypeId)
        {
            TMLeaveModel objmodel = new TMLeaveModel();
            try
            {
                TMLeaveDal objdal = new TMLeaveDal();
                DataSet ds = new DataSet();
                ds = objdal.GetselectedTMLeaveList(LeaveTypeId);
                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            objmodel.LeaveTypeId = Convert.ToInt32(ds.Tables[0].Rows[0]["LeaveTypeId"].ToString());
                            objmodel.LeaveType = ds.Tables[0].Rows[0]["LeaveType"].ToString();
                            objmodel.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                            objmodel.IsPaid = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsPaid"]);
                            objmodel.YearlyQuota = Convert.ToInt16(ds.Tables[0].Rows[0]["YearlyQuota"]);
                            objmodel.CanLaps = Convert.ToBoolean(ds.Tables[0].Rows[0]["CanLaps"]);
                            objmodel.MaxLeaveCarryover = Convert.ToInt16(ds.Tables[0].Rows[0]["MaxLeaveCarryover"]);
                            objmodel.Enable = Convert.ToBoolean(ds.Tables[0].Rows[0]["Enable"]);
                            objmodel.CreatedByName = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            objmodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            objmodel.ModifiedByName = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
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
        #endregion TMLeavelist
        #region TMLeave Approval
      public List<TMLeaveDetailModel> TMPendingLeaveApprovalList(string User_Id)
      {
          List<TMLeaveDetailModel> TMLeavelstDetail = new List<TMLeaveDetailModel>();

            try
            {
                TMLeaveDal objdal = new TMLeaveDal();
                DataSet ds = new DataSet();
                ds = objdal.GetTMPendingLeaveApprovalList(User_Id);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            TMLeaveDetailModel objmodel = new TMLeaveDetailModel();
                            objmodel.LeaveId = Convert.ToInt32(dr["LeaveId"].ToString());
                            objmodel.LeaveTypeName = dr["LeaveTypeId"].ToString();
                            objmodel.Employee = dr["EmpName"].ToString();
                            objmodel.ManagerName = dr["ManagerName"].ToString();
                            objmodel.FromDate = Convert.ToDateTime(dr["FromDate"].ToString());
                            objmodel.ToDate = Convert.ToDateTime(dr["ToDate"].ToString());
                            if (dr["ApprovedDate"].ToString() != "")
                                if (dr["ApprovedDate"].ToString() != "1/1/2001 12:00:00 AM")
                                    objmodel.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                            objmodel.Remark = dr["Remark"].ToString();
                            objmodel.ManagerRemark = dr["ManagerRemark"].ToString();
                            objmodel.StatusShortCode = dr["Status"].ToString();
                            objmodel.CreatedByName = dr["CreatedByName"].ToString();
                            objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            objmodel.ModifiedByName = dr["ModifiedByName"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());                            
                            TMLeavelstDetail.Add(objmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TMLeavelstDetail;
      }
        #endregion TMLeave Approval

        #region Leave
      public double GetLeaveBalance(string User_Id,string LeaveCategory, int LeaveTypeId)
      {
          double leaveBal = 0;
          try
          {
              TMLeaveDal ObjDAL = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = ObjDAL.GetBalanceLeave(User_Id, LeaveCategory, LeaveTypeId);
              if (ds != null)
              {
                  if (ds.Tables.Count > 0)
                  {
                      
                      if(ds.Tables[0].Rows.Count > 0)
                      {
                          leaveBal = Convert.ToDouble(ds.Tables[0].Rows[0]["BalanceLeave"]);
                      }
                  }
              }
          }
          catch (Exception ex)
          {
              Common.LogException(ex);
          }
          return leaveBal;
      }




      public List<LeaveDescription> GetLeaveDescription(string User_Id)
      {
          List<LeaveDescription> LstModel = new List<LeaveDescription>();
          try
          {
              TMLeaveDal ObjDAL = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = ObjDAL.GetAllLeaveDetails(User_Id);
              if(ds != null)
              {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            LeaveDescription Model = new LeaveDescription();
                            Model.LeaveType = dr["LeaveType"].ToString();
                            if (dr["Total Leave"].ToString() == "")
                            {
                                Model.TotalLeave = 0;
                            }
                            else
                            {
                                Model.TotalLeave = Convert.ToDouble(dr["Total Leave"].ToString());
                            }
                            Model.YearlyQuota = Convert.ToDouble(dr["YearlyQuota"].ToString());
                            Model.MaxLeaveCarryover = Convert.ToDouble(dr["MaxLeaveCarryover"].ToString());
                            if (dr["BalanceLeave"].ToString() == "")
                            {
                                Model.BalanceLeave = Model.MaxLeaveCarryover;
                            }
                            else
                            {
                                Model.BalanceLeave = Convert.ToDouble(dr["BalanceLeave"].ToString());
                            }
                            if (dr["CarryForwardLeave"].ToString() == "")
                            {
                                Model.LastYearCarryover = Convert.ToDouble(0);
                            }
                            else
                            {
                                Model.LastYearCarryover = Convert.ToDouble(dr["CarryForwardLeave"].ToString());
                            }
                            LstModel.Add(Model);
                        }
                    }
              }
          }
          catch (Exception ex)
          {
              Common.LogException(ex);
          }
          return LstModel;
      }
         
      public List<TMLeaveDetailModel> TMLeaveList(string User_Id)
        {
            List<TMLeaveDetailModel> TMLeavelstDetail = new List<TMLeaveDetailModel>();

            try
            {
                TMLeaveDal objdal = new TMLeaveDal();
                DataSet ds = new DataSet();
                ds = objdal.GetTMLeaveList(User_Id);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            TMLeaveDetailModel objmodel = new TMLeaveDetailModel();
                            objmodel.LeaveId = Convert.ToInt32(dr["LeaveId"].ToString());
                            objmodel.LeaveTypeName = dr["LeaveTypeId"].ToString();
                            objmodel.Employee = dr["EmpName"].ToString();
                            objmodel.LeaveCategory = dr["LeaveCategory"].ToString();
                            objmodel.ManagerName = dr["ManagerName"].ToString();
                            objmodel.FromDate = Convert.ToDateTime(dr["FromDate"].ToString());
                            objmodel.ToDate = Convert.ToDateTime(dr["ToDate"].ToString());
                            if (dr["ApprovedDate"].ToString()== "")
                            {
                              
                            }
                            else if (dr["ApprovedDate"].ToString() == "1/1/2001 12:00:00 AM")
                            {

                            }
                            else
                            {
                                objmodel.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                            }
                           
                            objmodel.Remark = dr["Remark"].ToString();
                            objmodel.ManagerRemark = dr["ManagerRemark"].ToString();
                            objmodel.StatusShortCode = dr["Status"].ToString();
                            objmodel.CreatedByName = dr["CreatedBy"].ToString();
                            objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            objmodel.ModifiedByName = dr["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            objmodel.LeaveLevel = dr["LeaveLevel"].ToString();
                            TMLeavelstDetail.Add(objmodel);

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TMLeavelstDetail;
        }
      public List<StatusModel> GetStatusListForDropDown()
      {
          List<StatusModel> StatusList = new List<StatusModel>();

          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetDropdownForStatusCodes();
              if (ds != null)
              {
                  if (ds.Tables.Count > 0)
                  {

                      foreach (DataRow dr in ds.Tables[0].Rows)
                      {
                          StatusModel objmodel = new StatusModel();
                          objmodel.StatusId = Convert.ToInt32(dr["StatusId"].ToString());

                          objmodel.StatusShortCode = dr["StatusShortCode"].ToString();

                          StatusList.Add(objmodel);

                      }

                  }

              }

          }
          catch (Exception ex)
          {
              throw ex;
          }
          return StatusList;
      }
      public int SaveTMLeaveDetails(TMLeaveDetailModel objmodel, string User_Id)
      {
          int errcode = 0;
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetSelectedTMLeaveDetail(0);
              if (ds != null)
              {
                  ds.Tables[0].Rows.Clear();
                  DataRow dr = ds.Tables[0].NewRow();
                  dr["LeaveId"] = objmodel.LeaveId;
                  dr["LeaveTypeId"] = objmodel.LeaveTypeId;
                  dr["ManagerId"] = objmodel.ManagerId;
                  dr["LeaveCategory"] = objmodel.LeaveCategory;
                  dr["FromDate"] = objmodel.FromDate;
                  dr["ToDate"] = objmodel.ToDate;
                  dr["Remark"] = objmodel.Remark;
                  if (objmodel.ApprovedDate !=null)
                  {
                      dr["ApprovedDate"] = objmodel.ApprovedDate;
                  }
                 
                  dr["ManagerRemark"] = objmodel.ManagerRemark;
                  dr["Status"] = objmodel.Status;
                  ds.Tables[0].Rows.Add(dr);
                  errcode = objdal.SaveTMLeaveDetail(ds, User_Id);
              }
          }
          catch (Exception ex)
          {

              throw ex;
          }
          return errcode;
      }
      public TMLeaveDetailModel GetSelectedTMLeaveDetail(int LeaveId)
      {
          TMLeaveDetailModel objmodel = new TMLeaveDetailModel();
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetSelectedTMLeaveDetail(LeaveId);
              if (ds != null)
              {

                  if (ds.Tables.Count > 0)
                  {

                      if (ds.Tables[0].Rows.Count > 0)
                      {
                          objmodel.LeaveId = Convert.ToInt32(ds.Tables[0].Rows[0]["LeaveId"].ToString());
                          objmodel.LeaveTypeId = Convert.ToInt32(ds.Tables[0].Rows[0]["LeaveTypeId"].ToString());
                          objmodel.Employee = ds.Tables[0].Rows[0]["EmpName"].ToString();
                          objmodel.LeaveTypeName = ds.Tables[0].Rows[0]["LeaveType"].ToString();
                          objmodel.ManagerId =Convert.ToInt32( ds.Tables[0].Rows[0]["ManagerId"].ToString());
                          objmodel.FromDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["FromDate"]);
                          objmodel.ToDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ToDate"]);
                          objmodel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                          objmodel.LeaveCategory = ds.Tables[0].Rows[0]["LeaveCategory"].ToString();
                          objmodel.ManagerRemark = ds.Tables[0].Rows[0]["ManagerRemark"].ToString();
                          objmodel.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["Status"]);
                          if (ds.Tables[0].Rows[0]["ApprovedDate"].ToString() == "")
                             {
                              
                             }
                            else if (ds.Tables[0].Rows[0]["ApprovedDate"].ToString() == "1/1/2001 12:00:00 AM")
                             {

                             }
                            
                            else
                            {
                              objmodel.ApprovedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ApprovedDate"].ToString());
                            }
                          objmodel.CreatedByName = ds.Tables[0].Rows[0]["CreatedByName"].ToString();
                          objmodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                          objmodel.ModifiedByName = ds.Tables[0].Rows[0]["ModifiedByName"].ToString();
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
        #endregion Leave

        #region Time Sheet Daily Entry

      public List<TMProjectList> GetProjectList(string User_Id)
      {          
          List<TMProjectList> TMModel = new List<TMProjectList>();
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetTMProjectList(User_Id);
              if (ds != null)
              {                  
                  if (ds.Tables.Count > 0)
                  {
                      foreach (DataRow dr in ds.Tables[0].Rows)
                      {
                          TMProjectList projectmodel = new TMProjectList();
                          projectmodel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                          projectmodel.ProjectName = dr["ProjectName"].ToString();
                          TMModel.Add(projectmodel);
                      }
                  }
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return TMModel;
      }
      public List<TMMOMList> GetMoMByProject(string User_Id, int ProjectId, int TaskId,DateTime  Date)
      {
          List<TMMOMList> TMModel = new List<TMMOMList>();
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetMoMByProject(User_Id, ProjectId, TaskId, Date);
              if (ds != null)
              {
                  if (ds.Tables.Count > 0)
                  {

                      foreach (DataRow dr in ds.Tables[0].Rows)
                      {
                          TMMOMList projectmodel = new TMMOMList();
                          projectmodel.MoMId = Convert.ToInt32(dr["MoMId"].ToString());
                          projectmodel.Title = dr["Title"].ToString();
                          TMModel.Add(projectmodel);
                      }
                  }
              }

          }
          catch (Exception ex)
          {
              throw ex;
          }
          return TMModel;
      }
      public List<TMTaskList> GetTaskList(int ProjectId, string User_Id)
      {          
          List<TMTaskList> TMModel = new List<TMTaskList>();
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetTMTaskList(ProjectId, User_Id);
              if (ds != null)
              {
                  if (ds.Tables.Count > 0)
                  {

                      foreach (DataRow dr in ds.Tables[0].Rows)
                      {
                          TMTaskList Taskmodel = new TMTaskList();
                          Taskmodel.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                          Taskmodel.TaskName = dr["TaskName"].ToString();
                          TMModel.Add(Taskmodel);
                      }
                  }                  
              }

          }
          catch (Exception ex)
          {
              throw ex;
          }
          return TMModel;
      }
      public TimesheetHistory GetTMHistoryList(string User_Id)
       {
          TimesheetHistory TMModel = new TimesheetHistory();
          TMModel.LstHistory = new List<TimesheetHistory>();
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetTMHistoryList(User_Id);
              if (ds != null)
              {
                  if (ds.Tables.Count > 0)
                  {
                      foreach (DataRow dr in ds.Tables[0].Rows)
                      {
                          TimesheetHistory Taskmodel = new TimesheetHistory();
                          Taskmodel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                          Taskmodel.EmpName = dr["EmpName"].ToString();
                          Taskmodel.TimeSheetId = Convert.ToInt32(dr["TimeSheetId"].ToString());                         
                          Taskmodel.StatusCode = dr["StatusShortCode"].ToString();
                          Taskmodel.WeekNo = Convert.ToInt32(dr["WeekNo"].ToString());                          
                          Taskmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                          Taskmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                          if (dr["WorkingHour"].ToString() != "")
                          Taskmodel.WorkingHour = Convert.ToDouble(dr["WorkingHour"].ToString());
                          if (Taskmodel.WeekNo !=0)
                          {
                              DateTime dt = Convert.ToDateTime(dr["StartDate"].ToString());
                              int year = dt.Year;
                              int weekno = Taskmodel.WeekNo;
                              Taskmodel.FromDate=FirstDateOfWeekISO8601(year,weekno);
                              Taskmodel.ToDate = Taskmodel.FromDate.AddDays(6);
                          }
                          TMModel.LstHistory.Add(Taskmodel);
                      }
                  }                 
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return TMModel;
      }
      public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
      {
          DateTime jan1 = new DateTime(year, 1, 1);
          int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

          DateTime firstThursday = jan1.AddDays(daysOffset);
          var cal = CultureInfo.CurrentCulture.Calendar;
          int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

          var weekNum = weekOfYear;
          if (firstWeek <= 1)
          {
              weekNum -= 1;
          }
          var result = firstThursday.AddDays(weekNum * 7);
          return result.AddDays(-3);
      }
      public int UpdateStatusTMEntry(int TimeSheetId,int StatusCode)
      {
          int errCode = 0;
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              errCode = objdal.UpdateStatusTMEntry(TimeSheetId, StatusCode);
          }
          catch (Exception ex)
          {              
              throw ex;
          }
          return errCode;
      }
      public int SaveTimeSheetEntryLastApproval(int TimeSheetId, string User_Id)
      {
          int errCode = 0;
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              errCode = objdal.SaveTimeSheetEntryLastApproval(TimeSheetId, User_Id);
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return errCode;
      }

      public int DeleteTimeSheetEntry(int TimeSheetEntryId)
      {
          int errCode = 0;
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              errCode = objdal.DeleteTimeSheetEntry(TimeSheetEntryId);
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return errCode;
      }
      public TimeSheetDlyEntryList GetTimeSheetEntryList(int TimeSheetId,string User_Id)
      {
          TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
          TMModel.timeSheetEntrylist = new List<TimeSheetDlyEntryList>();
          TMModel.TMdatelist = new List<TMDateList>();
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetTimeSheetEntryList(TimeSheetId, User_Id);
              if (ds != null)
              {
                  if (ds.Tables.Count > 1)
                  {

                      foreach (DataRow dr in ds.Tables[0].Rows)
                      {
                          TimeSheetDlyEntryList Taskmodel = new TimeSheetDlyEntryList();
                          Taskmodel.TimeSheetEntryId = Convert.ToInt32(dr["TimeSheetEntryId"].ToString());
                          Taskmodel.TimeSheetId = Convert.ToInt32(dr["TimeSheetId"].ToString());
                          Taskmodel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                          Taskmodel.ProjectName = dr["ProjectName"].ToString();
                          Taskmodel.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                          Taskmodel.TaskName = dr["TaskName"].ToString();                                                       
                          TMModel.timeSheetEntrylist.Add(Taskmodel);
                      }
                  }
                  foreach (DataRow dr in ds.Tables[1].Rows)
                  {
                      TMDateList demo = new TMDateList();
                      demo.WeekNo = Convert.ToInt32(dr["TimeSheetId"]);
                      DateTime dt = Convert.ToDateTime(dr["StartDate"].ToString());
                          int year = dt.Year;
                          int weekno = demo.WeekNo;
                        TMModel.FromDate = dt;
                          TMModel.ToDate = TMModel.FromDate.AddDays(6);
                      demo.Date = TMModel.FromDate.ToShortDateString() + "  To  " + TMModel.ToDate.ToShortDateString();                     
                      TMModel.TMdatelist.Add(demo);
                  }
                  if (ds.Tables.Count > 2)
                  {
                      if (ds.Tables[2].Rows.Count > 0)
                      {
                          TMModel.WeekNo = Convert.ToInt32(ds.Tables[2].Rows[0]["WeekNo"]);
                      }
                  }
                  if (ds.Tables.Count > 3)
                  {
                      foreach(DataRow dr in ds.Tables[3].Rows)
                      {
                          foreach(TimeSheetDlyEntryList demo in TMModel.timeSheetEntrylist)
                          {
                              if (demo.TimeSheetEntryId == Convert.ToInt32(dr["TimeSheetEntryId"].ToString()))
                              {
                                  switch (Convert.ToInt32(dr["DayCode"].ToString()))
                                  {
                                      case 1:
                                          {
                                              demo.Day = "Monday";
                                              if (demo.MonTime !=null)
                                              {
                                                  demo.MonTime = demo.MonTime + " ,";
                                              }
                                              demo.MonTime = demo.MonTime  + (dr["FromTime"].ToString() + "  To  " + dr["ToTime"].ToString());
                                              demo.MonTime.ToString().TrimStart();
                                              break;
                                          }
                                      case 2:
                                          {
                                              demo.Day = "Tuesday";
                                              if (demo.TusTime != null)
                                              {
                                                  demo.TusTime = demo.TusTime + " ,";
                                              }
                                              demo.TusTime = demo.TusTime + (dr["FromTime"].ToString() + "  To  " + dr["ToTime"].ToString());
                                              break;
                                          }
                                      case 3:
                                          {
                                              demo.Day = "Wednesday";                                       
                                              if (demo.WedTime != null)
                                              {
                                                  demo.WedTime = demo.WedTime + " ,";
                                              }
                                              demo.WedTime = demo.WedTime  + (dr["FromTime"].ToString() + "  To  " + dr["ToTime"].ToString());
                                              break;
                                          }
                                      case 4:
                                          {
                                              demo.Day = "Thursday";
                                              if (demo.ThusTime != null)
                                              {
                                                  demo.ThusTime = demo.ThusTime + " ,";
                                              }
                                              demo.ThusTime = demo.ThusTime  + (dr["FromTime"].ToString() + "  To  " + dr["ToTime"].ToString());
                                              break;
                                          }
                                      case 5:
                                          {
                                              demo.Day = "Friday";
                                              if (demo.FriTime != null)
                                              {
                                                  demo.FriTime = demo.FriTime + " ,";
                                              }
                                              demo.FriTime = demo.FriTime  + (dr["FromTime"].ToString() + "  To  " + dr["ToTime"].ToString());
                                              break;
                                          }
                                      case 6:
                                          {
                                              demo.Day = "Saturday";
                                              if (demo.SatTime != null)
                                              {
                                                  demo.SatTime = demo.SatTime + " ,";
                                              }
                                              demo.SatTime = demo.SatTime  + (dr["FromTime"].ToString() + "  To  " + dr["ToTime"].ToString());
                                              break;
                                          }
                                      case 7:
                                          {
                                              demo.Day = "Sunday";
                                              if (demo.SunTime != null)
                                              {
                                                  demo.SunTime = demo.SunTime + " ,";
                                              }
                                              demo.SunTime = demo.SunTime  + (dr["FromTime"].ToString() + "  To  " + dr["ToTime"].ToString());
                                              break;
                                          }
                                  }
                                  break;
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
          return TMModel;
      }



      public List<TimeSheetViewModel> GetTimeSheetEnterList(int TimeSheetId)
      {
          List<TimeSheetViewModel> Model = new List<TimeSheetViewModel>();
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet dsUser = new DataSet();
              dsUser = objdal.GetTimeSheetEntryList(TimeSheetId,"");
              if (dsUser != null)
              {
                  if (dsUser.Tables.Count > 4)
                  {
                      if (dsUser.Tables[0].Rows.Count > 0)
                      {
                          foreach (DataRow dr in dsUser.Tables[4].Rows)
                          {
                              TimeSheetViewModel demo = new TimeSheetViewModel();
                              demo.TimeSheetId = Convert.ToInt32(dr["TimeSheetId"]);
                              demo.TimeSheetEntryDetailId = Convert.ToInt32(dr["TimeSheetEntryDetailId"]);
                              demo.EmployeeName = dr["EmployeeName"].ToString();
                              demo.TimeSheetEntryId = Convert.ToInt32(dr["TimeSheetEntryId"]);
                              demo.WeekNo = Convert.ToInt32(dr["WeekNo"]);
                              demo.ProjectName = dr["ProjectName"].ToString();
                              demo.TaskName = dr["TaskName"].ToString();
                              demo.StartDate = Convert.ToDateTime(dr["StartDate"]);
                              demo.StartDateStr = demo.StartDate.ToShortDateString();
                              demo.StatusShortCode = dr["StatusShortCode"].ToString();
                              if (dr["ApproverName"].ToString() != "")
                                  demo.ApproverName = dr["ApproverName"].ToString();
                              if (dr["ApprovedDate"].ToString() != "")
                              {
                                  demo.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"]);
                                  demo.ApprovedDateStr = demo.ApprovedDate.ToShortDateString();
                              }
                              else
                              {
                                  demo.ApprovedDateStr = "";
                              }
                              demo.ApproverRemark = dr["ApproverRemark"].ToString();
                              demo.Remark = dr["Remark"].ToString();
                              demo.CreatedByName = dr["CreatedByName"].ToString();
                              demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                              demo.CreatedDateStr = demo.CreatedDate.ToShortDateString();
                              demo.ModifiedByName = dr["ModifiedByName"].ToString();
                              demo.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                              demo.ModifiedDateStr = demo.ModifiedDate.ToShortDateString();
                              demo.TimeSheetEntryId = Convert.ToInt32(dr["TimeSheetEntryId"]);
                              demo.DayText = dr["DayText"].ToString();
                              demo.CustomerName = dr["CustomerName"].ToString();
                              demo.VendorName = dr["VendorName"].ToString();
                              demo.MOMTitle = dr["MOMTitle"].ToString();
                              demo.FromTime = dr["FromTime"].ToString();
                              demo.ToTime = dr["ToTime"].ToString();
                              demo.TotalTime =Convert.ToInt32(dr["TotalTime"].ToString());
                              if (dr["MOMId"].ToString() != "")
                              {
                                  demo.MOMId = Convert.ToInt32(dr["MOMId"]);
                              }
                              else
                              {
                                  demo.MOMId = 0;
                              }

                              Model.Add(demo);
                          }
                      }
                  }
              }
          }
          catch (Exception)
          {
              throw;
          }
          return Model;
      }


      public int GetTMIdByWeekNo(int WeekNo, string User_Id)
      {
          int ErrCode = 0;
          try
          {
              TMLeaveDal DALObj=new TMLeaveDal();
              DataSet dsObj = new DataSet();
              dsObj = DALObj.GetTMIdByWeekNo(WeekNo, User_Id);
              if(dsObj !=null)
              {
                  if(dsObj.Tables.Count > 0)
                  {
                      if (dsObj.Tables[0].Rows.Count > 0)
                      ErrCode = Convert.ToInt32(dsObj.Tables[0].Rows[0]["TimeSheetId"].ToString());
                  }
              }
              
          }
          catch (Exception)
          {

              throw;
          }
          return ErrCode;
      }
      public string CheckProjDisp(int ProjectId)
      {
          string str = "Show";
          try
          {
              TMLeaveDal DALObj = new TMLeaveDal();
              DataSet dsObj = new DataSet();
              dsObj = DALObj.CheckProjDisp(ProjectId);
              if (dsObj != null)
              {
                  if (dsObj.Tables.Count > 0)
                  {
                      if (dsObj.Tables[0].Rows.Count > 0)
                      {
                          if (Convert.ToInt32(dsObj.Tables[0].Rows[0]["CustomerId"]) > 0 || Convert.ToInt32(dsObj.Tables[0].Rows[0]["CustomerId"]) > 0)
                          str = "Hide";
                      }
                      else
                      {
                          str = "Show";
                      }
                  }
              }

          }
          catch (Exception)
          {

              throw;
          }
          return str;
      }
      public int DeleteTMEntryDetail(int TimeSheetEntryDetailId,string User_Id)
      {
          int errCode = 0;
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              errCode = objdal.DeleteTMEntryDetail(TimeSheetEntryDetailId, User_Id);
          }
          catch (Exception ex)
          {
              Common.LogException(ex);
          }
          return errCode;
      }

      public int UpdateTMEntryDetail(DayTimeLstModel Model)
      {
          int errCode = 0;
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              errCode = objdal.UpdateTMEntryDetail(Model.TimeSheetEntryDetailId,Model.CustomerId,Model.VendorId,Model.MOMId,Model.Remark);
          }
          catch (Exception ex)
          {
              Common.LogException(ex);
          }
          return errCode;
      }

      public List<DayTimeLstModel> GetDayTimeList(int ProjectId,int TaskId,string DayCode,int TimeSheetId)
      {
          List<DayTimeLstModel> LstTMModel = new List<DayTimeLstModel>();
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              int DCode = 0;
                switch (DayCode)
                              {
                                  case  "Monday":
                                      {
                                          DCode = 1;
                                         
                                          break;
                                      }
                                  case "Tuesday":
                                      {
                                          DCode = 2;
                                          
                                          break;
                                      }
                                  case "Wednesday":
                                      {
                                          DCode = 3;
                                       
                                          break;
                                      }
                                  case "Thursday":
                                      {
                                          DCode = 4;
                                          
                                          break;
                                      }
                                  case "Friday":
                                      {
                                          DCode = 5;
                                         
                                          break;
                                      }
                                  case "Saturday":
                                      {
                                          DCode = 6;
                                         
                                          break;
                                      }
                                  case "Sunday":
                                      {
                                          DCode = 7;
                                         
                                          break;
                                      }
                              }
                ds = objdal.GetTimeSheetDayTimeList(ProjectId, TaskId, DCode, TimeSheetId);
              if (ds != null)
              {
                  if (ds.Tables.Count > 0)
                  {
                      foreach (DataRow dr in ds.Tables[0].Rows)
                      {
                          DayTimeLstModel TMModel = new DayTimeLstModel();
                          TMModel.TimeSheetEntryDetailId = Convert.ToInt32(dr["TimeSheetEntryDetailId"].ToString());
                          TMModel.TimeSheetId = TimeSheetId;
                          TMModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                          TMModel.ProjectName = dr["ProjectName"].ToString();
                          TMModel.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                          TMModel.TaskName = dr["TaskName"].ToString();
                          TMModel.DayCode = Convert.ToInt32(dr["DayCode"].ToString());
                          TMModel.Remark = dr["Remark"].ToString();
                          TMModel.CustomerName = dr["CustomerName"].ToString();
                          TMModel.VendorName = dr["VendorName"].ToString();
                          TMModel.MOMTitle = dr["MOMTitle"].ToString();
                          TMModel.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                          TMModel.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                          if (dr["MOMId"].ToString() != "")
                          TMModel.MOMId = Convert.ToInt32(dr["MOMId"].ToString());
                          TMModel.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                          switch (Convert.ToInt32(dr["DayCode"].ToString()))
                          {
                              case 1:
                                  {
                                      TMModel.Day = "Monday";
                                      TMModel.FromTime = Convert.ToDateTime(dr["FromTime"].ToString());
                                      TMModel.ToTime = Convert.ToDateTime(dr["ToTime"].ToString());
                                      TMModel.strFromTime = dr["FromTime"].ToString();
                                      TMModel.strToTime = dr["ToTime"].ToString();
                                      break;
                                  }
                              case 2:
                                  {
                                      TMModel.Day = "Tuesday";
                                      TMModel.FromTime = Convert.ToDateTime(dr["FromTime"].ToString());
                                      TMModel.ToTime = Convert.ToDateTime(dr["ToTime"].ToString());
                                      TMModel.strFromTime = dr["FromTime"].ToString();
                                      TMModel.strToTime = dr["ToTime"].ToString();
                                      break;
                                  }
                              case 3:
                                  {
                                      TMModel.Day = "Wednesday";
                                      TMModel.FromTime = Convert.ToDateTime(dr["FromTime"].ToString());
                                      TMModel.ToTime = Convert.ToDateTime(dr["ToTime"].ToString());
                                      TMModel.strFromTime = dr["FromTime"].ToString();
                                      TMModel.strToTime = dr["ToTime"].ToString();
                                      break;
                                  }
                              case 4:
                                  {
                                      TMModel.Day = "Thursday";
                                      TMModel.FromTime = Convert.ToDateTime(dr["FromTime"].ToString());
                                      TMModel.ToTime = Convert.ToDateTime(dr["ToTime"].ToString());
                                      TMModel.strFromTime = dr["FromTime"].ToString();
                                      TMModel.strToTime = dr["ToTime"].ToString();
                                      break;
                                  }
                              case 5:
                                  {
                                      TMModel.Day = "Friday";
                                      TMModel.FromTime = Convert.ToDateTime(dr["FromTime"].ToString());
                                      TMModel.strFromTime = dr["FromTime"].ToString();
                                      TMModel.strToTime = dr["ToTime"].ToString();
                                      TMModel.ToTime = Convert.ToDateTime(dr["ToTime"].ToString());
                                      break;
                                  }
                              case 6:
                                  {
                                      TMModel.Day = "Saturday";
                                      TMModel.FromTime = Convert.ToDateTime(dr["FromTime"].ToString());
                                      TMModel.ToTime = Convert.ToDateTime(dr["ToTime"].ToString());
                                      TMModel.strFromTime = dr["FromTime"].ToString();
                                      TMModel.strToTime = dr["ToTime"].ToString();
                                      break;
                                  }
                              case 7:
                                  {
                                      TMModel.Day = "Sunday";
                                      TMModel.FromTime = Convert.ToDateTime(dr["FromTime"].ToString());
                                      TMModel.ToTime = Convert.ToDateTime(dr["ToTime"].ToString());
                                      TMModel.strFromTime = dr["FromTime"].ToString();
                                      TMModel.strToTime = dr["ToTime"].ToString();
                                      break;
                                  }
                          }
                          LstTMModel.Add(TMModel);
                      }
                  }
              }

          }
          catch (Exception ex)
          {
              throw ex;
          }
          return LstTMModel;
      }

      public TMGetSelectedModel GetSelectedTimeSheetEntryList(int TimeSheetentryId,int TimeSheetId,string User_ID)
      {
          TMGetSelectedModel TMModel = new TMGetSelectedModel();         
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              DataSet ds1 = new DataSet();
              ds = objdal.GetSelectedTimeSheetEntryList(TimeSheetentryId);
              ds1 = objdal.GetTimeSheetEntryList(TimeSheetId, User_ID);
              if (TimeSheetId > 0)
              {
                  if (ds1.Tables[2].Rows.Count > 0)
                   {                     
                      TMModel.StartDate = Convert.ToDateTime(ds1.Tables[2].Rows[0]["StartDate"].ToString());
                      TMModel.EndDate = TMModel.StartDate.AddDays(6);
                      TMModel.WeekNo = Convert.ToInt32(ds1.Tables[2].Rows[0]["WeekNo"].ToString());
                  }
              }
              if (ds != null)
              {
                  if (ds.Tables.Count > 0)
                  {

                      foreach (DataRow dr in ds.Tables[0].Rows)
                      {
                          TMModel.TimeSheetEntryId = Convert.ToInt32(dr["TimeSheetEntryId"].ToString());
                          TMModel.TimeSheetId = Convert.ToInt32(dr["TimeSheetId"].ToString());
                          TMModel.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                          TMModel.ProjectName = dr["ProjectName"].ToString();
                          TMModel.TaskId = Convert.ToInt32(dr["TaskId"].ToString());                         
                          TMModel.TaskName = dr["TaskName"].ToString();

                          foreach (DataRow dr1 in ds.Tables[1].Rows)
                          {
                              switch (Convert.ToInt32(dr1["DayCode"].ToString()))
                              {
                                  case 1:
                                      {
                                          TMModel.Day = "Monday";
                                          TMModel.MonFromTime = Convert.ToDateTime(dr1["FromTime"].ToString());
                                          TMModel.MonToTime = Convert.ToDateTime(dr1["ToTime"].ToString());
                                          break;
                                      }
                                  case 2:
                                      {
                                          TMModel.Day = "Tuesday";
                                          TMModel.TusFromTime = Convert.ToDateTime(dr1["FromTime"].ToString());
                                          TMModel.TusToTime = Convert.ToDateTime(dr1["ToTime"].ToString());
                                          break;
                                      }
                                  case 3:
                                      {
                                          TMModel.Day = "Wednesday";
                                          TMModel.WedFromTime = Convert.ToDateTime(dr1["FromTime"].ToString());
                                          TMModel.WedToTime = Convert.ToDateTime(dr1["ToTime"].ToString());
                                          break;
                                      }
                                  case 4:
                                      {
                                          TMModel.Day = "Thursday";
                                          TMModel.ThusFromTime = Convert.ToDateTime(dr1["FromTime"].ToString());
                                          TMModel.ThusToTime = Convert.ToDateTime(dr1["ToTime"].ToString());
                                          break;
                                      }
                                  case 5:
                                      {
                                          TMModel.Day = "Friday";
                                          TMModel.FriFromTime = Convert.ToDateTime(dr1["FromTime"].ToString());
                                          TMModel.FriToTime = Convert.ToDateTime(dr1["ToTime"].ToString());
                                          break;
                                      }
                                  case 6:
                                      {
                                          TMModel.Day = "Saturday";
                                          TMModel.SatFromTime = Convert.ToDateTime(dr1["FromTime"].ToString());
                                          TMModel.SatToTime = Convert.ToDateTime(dr1["ToTime"].ToString());
                                          break;
                                      }
                                  case 7:
                                      {
                                          TMModel.Day = "Sunday";
                                          TMModel.SunFromTime = Convert.ToDateTime(dr1["FromTime"].ToString());
                                          TMModel.SunToTime = Convert.ToDateTime(dr1["ToTime"].ToString());
                                          break;
                                      }
                              }
                          }                                      
                      }
                      if (ds.Tables[2].Rows.Count > 0)
                      {

                          if (Convert.ToInt32(ds.Tables[2].Rows[0]["CustomerId"]) > 0 || Convert.ToInt32(ds.Tables[2].Rows[0]["VendorId"]) > 0)
                          {
                              TMModel.Check = "Hide";
                          }
                          else
                          {
                              TMModel.Check = "Show";
                          }
                      }                     
                  }
              }

          }
          catch (Exception ex)
          {
              throw ex;
          }
          return TMModel;
      }

      public int UpdateTimeSheetEntryDetail(TMGetSelectedModel Model, string User_Id)
      {
          int ErrorCode = 0;
          int dayCode = 1;
          TimeSheetDlyEntryList TMModel = new TimeSheetDlyEntryList();
          try
          {
              TMLeaveDal DalObj = new TMLeaveDal();
              DataSet dsTMUpdateEntry = new DataSet();

              DataTable dtTMUpdateEntry = new DataTable("tbl_RDSRptSubscription");
              dtTMUpdateEntry.Columns.Add("TimeSheetEntryId", typeof(System.Int32));
              dtTMUpdateEntry.Columns.Add("TimeSheetId", typeof(System.Int32));
              dtTMUpdateEntry.Columns.Add("ProjectId", typeof(System.Int32));
              dtTMUpdateEntry.Columns.Add("TaskId", typeof(System.Int32));

              DataRow drTMUpdateEntry = dtTMUpdateEntry.NewRow();
              drTMUpdateEntry["TimeSheetEntryId"] = Model.TimeSheetEntryId;
              drTMUpdateEntry["TimeSheetId"] = Model.TimeSheetId;
              drTMUpdateEntry["TaskId"] = Convert.ToInt32(Model.TaskId);
              drTMUpdateEntry["ProjectId"] = Model.ProjectId;
              dtTMUpdateEntry.Rows.Add(drTMUpdateEntry);
              dsTMUpdateEntry.Tables.Add(dtTMUpdateEntry);

              DataTable dtTMTime = new DataTable("tbl_TimeSheet");
              dtTMTime.Columns.Add("TimeSheetEntryId", typeof(System.Int32));
              dtTMTime.Columns.Add("DayCode", typeof(System.Int32));
              dtTMTime.Columns.Add("FromTime", typeof(System.DateTime));
              dtTMTime.Columns.Add("ToTime", typeof(System.DateTime));
              dtTMTime.Columns.Add("CustomerId", typeof(System.Int32));
              dtTMTime.Columns.Add("VendorId", typeof(System.Int32));
              dtTMTime.Columns.Add("MOMID", typeof(System.Int32));
              dtTMTime.Columns.Add("Remark", typeof(System.String));
              for (int i = 1; i < 8; i++)
              {
                  DataRow drTMTime = dtTMTime.NewRow();
                  switch (i)
                  {
                      case 1:
                          {
                              if (!(Model.MonFromTime.ToShortTimeString() == Model.MonToTime.ToShortTimeString()))
                              {
                                  dayCode = i;
                                  drTMTime["TimeSheetEntryId"] = 0;
                                  drTMTime["DayCode"] = i;
                                  drTMTime["FromTime"] = Model.MonFromTime.ToShortTimeString();
                                  drTMTime["ToTime"] = Model.MonToTime.ToShortTimeString();
                                  drTMTime["CustomerId"] = Model.CustomerId;
                                  drTMTime["VendorId"] = Model.VendorId;
                                  drTMTime["MOMId"] = Model.MOMId;
                                  drTMTime["Remark"] = Model.Remark;
                                  dtTMTime.Rows.Add(drTMTime);
                              }                                                               
                              break;
                          }
                      case 2:
                          {
                              if (!(Model.TusFromTime.ToShortTimeString() == Model.TusToTime.ToShortTimeString()))
                              {
                                  dayCode = i;
                                  drTMTime["TimeSheetEntryId"] = 0;
                                  drTMTime["DayCode"] = i;
                                  drTMTime["FromTime"] = Model.TusFromTime.ToShortTimeString();
                                  drTMTime["ToTime"] = Model.TusToTime.ToShortTimeString();
                                  drTMTime["CustomerId"] = Model.CustomerId;
                                  drTMTime["VendorId"] = Model.VendorId;
                                  drTMTime["MOMId"] = Model.MOMId;
                                  drTMTime["Remark"] = Model.Remark;
                                  dtTMTime.Rows.Add(drTMTime);
                              }                                                                                             
                              break;
                          }
                      case 3:
                          {
                              if (!(Model.WedFromTime.ToShortTimeString() == Model.WedToTime.ToShortTimeString()))
                              {
                                  dayCode = i;
                                  drTMTime["TimeSheetEntryId"] = 0;
                                  drTMTime["DayCode"] = i;
                                  drTMTime["FromTime"] = Model.WedFromTime.ToShortTimeString();
                                  drTMTime["ToTime"] = Model.WedToTime.ToShortTimeString();
                                  drTMTime["CustomerId"] = Model.CustomerId;
                                  drTMTime["VendorId"] = Model.VendorId;
                                  drTMTime["MOMId"] = Model.MOMId;
                                  drTMTime["Remark"] = Model.Remark;
                                  dtTMTime.Rows.Add(drTMTime);                              
                              }
                              break;
                          }
                      case 4:
                          {
                              if (!(Model.ThusFromTime.ToShortTimeString() == Model.ThusToTime.ToShortTimeString()))
                              {
                                  dayCode = i;
                                  drTMTime["TimeSheetEntryId"] = 0;
                                  drTMTime["DayCode"] = i;
                                  drTMTime["FromTime"] = Model.ThusFromTime.ToShortTimeString();
                                  drTMTime["ToTime"] = Model.ThusToTime.ToShortTimeString();
                                  drTMTime["CustomerId"] = Model.CustomerId;
                                  drTMTime["VendorId"] = Model.VendorId;
                                  drTMTime["MOMId"] = Model.MOMId;
                                  drTMTime["Remark"] = Model.Remark;
                                  dtTMTime.Rows.Add(drTMTime);                             
                              }  
                              break;
                          }
                      case 5:
                          {
                              if (!(Model.FriFromTime.ToShortTimeString() == Model.FriToTime.ToShortTimeString()))
                              {
                                  dayCode = i;
                                  drTMTime["TimeSheetEntryId"] = 0;
                                  drTMTime["DayCode"] = i;
                                  drTMTime["FromTime"] = Model.FriFromTime.ToShortTimeString();
                                  drTMTime["ToTime"] = Model.FriToTime.ToShortTimeString();
                                  drTMTime["CustomerId"] = Model.CustomerId;
                                  drTMTime["VendorId"] = Model.VendorId;
                                  drTMTime["MOMId"] = Model.MOMId;
                                  drTMTime["Remark"] = Model.Remark;
                                  dtTMTime.Rows.Add(drTMTime);
                              }                                                                  
                              break;
                          }
                      case 6:
                          {
                              if (!(Model.SatFromTime.ToShortTimeString() == Model.SatToTime.ToShortTimeString()))
                              {
                                  dayCode = i;
                                  drTMTime["TimeSheetEntryId"] = 0;
                                  drTMTime["DayCode"] = i;
                                  drTMTime["FromTime"] = Model.SatFromTime.ToShortTimeString();
                                  drTMTime["ToTime"] = Model.SatToTime.ToShortTimeString();
                                  drTMTime["CustomerId"] = Model.CustomerId;
                                  drTMTime["VendorId"] = Model.VendorId;
                                  drTMTime["MOMId"] = Model.MOMId;
                                  drTMTime["Remark"] = Model.Remark;
                                  dtTMTime.Rows.Add(drTMTime);                             
                              }                                 
                              break;
                          }
                      case 7:
                          {
                              if (!(Model.SunFromTime.ToShortTimeString() == Model.SunToTime.ToShortTimeString()))
                              {
                                  dayCode = i;   
                                  drTMTime["TimeSheetEntryId"] = 0;
                                  drTMTime["DayCode"] = i;
                                  drTMTime["FromTime"] = Model.SunFromTime.ToShortTimeString();
                                  drTMTime["ToTime"] = Model.SunToTime.ToShortTimeString();
                                  drTMTime["CustomerId"] = Model.CustomerId;
                                  drTMTime["VendorId"] = Model.VendorId;
                                  drTMTime["MOMId"] = Model.MOMId;
                                  drTMTime["Remark"] = Model.Remark;
                                  dtTMTime.Rows.Add(drTMTime);                             
                              }                                 
                              break;
                          }
                  }
              }

              dsTMUpdateEntry.Tables.Add(dtTMTime);

              if (Model.TimeSheetId == 0)
              {
                   CultureInfo ciCurr = CultureInfo.CurrentCulture;
                  int weekno = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                  weekno = weekno - 1;
                  DateTime dt = DateTime.Now;
                  int year = dt.Year;                  
                  dt = FirstDateOfWeekISO8601(year, weekno);
                  ErrorCode = DalObj.SaveTimeSheetEntryDetail(dsTMUpdateEntry, User_Id, weekno, "", dt);
              }
              else
              {
                  ErrorCode = DalObj.UpdateTimeSheetEntryDetail(dsTMUpdateEntry, User_Id);
              }

          }
          catch (Exception ex)
          {
              throw ex;
          }
          return ErrorCode;
      }     
        #endregion Time Sheet Daily Entry

        #region Project Category
      public ProjectCategoryModel GetTMProjectCategoryList(string User_Id)
      {

          ProjectCategoryModel TMProjectCategory = new ProjectCategoryModel();
          TMProjectCategory.ProjCateList = new List<ProjectCategoryModel>();
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetTMProjectCategoryList(User_Id);
              if (ds != null)
              {
                  if (ds.Tables.Count > 0)
                  {

                      foreach (DataRow dr in ds.Tables[0].Rows)
                      {
                          ProjectCategoryModel objmodel = new ProjectCategoryModel();
                          objmodel.ProjCategoryId = Convert.ToInt32(dr["ProjCategoryId"].ToString());
                          objmodel.CategoryName = dr["CategoryName"].ToString();
                          objmodel.CreatedBy = dr["CreatedBy"].ToString();
                          objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                          objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                          objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                          TMProjectCategory.ProjCateList.Add(objmodel);
                      }

                  }

              }

          }
          catch (Exception ex)
          {
              throw ex;
          }
          return TMProjectCategory;
      }

      public ProjectCategoryModel GetSelectedTMProjectCategoryList(int ProjectCateId)
      {
          ProjectCategoryModel TMProjectCategory = new ProjectCategoryModel();         
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetSelectedTMProjectCategoryList(ProjectCateId);
              if (ds != null)
              {
                  if (ds.Tables.Count > 0)
                  {
                      foreach (DataRow dr in ds.Tables[0].Rows)
                      {                          
                          TMProjectCategory.ProjCategoryId = Convert.ToInt32(ds.Tables[0].Rows[0]["ProjCategoryId"].ToString());
                          TMProjectCategory.CategoryName = ds.Tables[0].Rows[0]["CategoryName"].ToString();
                          TMProjectCategory.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                          TMProjectCategory.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                          TMProjectCategory.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                          TMProjectCategory.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());                       
                      }
                  }
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return TMProjectCategory;
      }

      public int SaveProjectCategory(ProjectCategoryModel objmodel, string User_Id)
      {
          int errcode = 0;
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetSelectedTMProjectCategoryList(0);
              if (ds != null)
              {
                  ds.Tables[0].Rows.Clear();
                  DataRow dr = ds.Tables[0].NewRow();
                  dr["ProjCategoryId"] = objmodel.ProjCategoryId;
                  dr["CategoryName"] = objmodel.CategoryName;                 
                  ds.Tables[0].Rows.Add(dr);                  
              }

              DataTable dtTMPrjectList = new DataTable("tbl_PrjectList");
              dtTMPrjectList.Columns.Add("ProjCategoryId", typeof(System.Int32));
              dtTMPrjectList.Columns.Add("ProjectId", typeof(System.Int32));
              dtTMPrjectList.Columns.Add("ProjectName", typeof(System.String));

              foreach(TMProjectList demo in objmodel.ProjectList)
              {
                  DataRow drTMUpdateEntry = dtTMPrjectList.NewRow();
                  drTMUpdateEntry["ProjCategoryId"] = 0;
                  drTMUpdateEntry["ProjectId"] = Convert.ToInt32(demo.ProjectId);
                  drTMUpdateEntry["ProjectName"] = demo.ProjectName; 
                  dtTMPrjectList.Rows.Add(drTMUpdateEntry);                  
              }
              ds.Tables.Add(dtTMPrjectList);
              errcode = objdal.SaveTMProjectCategory(ds, User_Id);

          }
          catch (Exception ex)
          {

              throw ex;
          }
          return errcode;
      }
      public List<TMProjectList> GetProjectListbyCategory(int CategoryId, string User_Id)
      {
          ProjectCategoryModel TMProjectCategory = new ProjectCategoryModel();
          TMProjectCategory.ProjectList=new List<TMProjectList>();
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetProjectListbyCategory(CategoryId, User_Id);
              if (ds != null)
              {
                  if (ds.Tables.Count > 0)
                  {
                      foreach (DataRow dr in ds.Tables[0].Rows)
                      {
                          TMProjectList demo=new TMProjectList();
                          demo.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                          demo.ProjectName = dr["ProjectName"].ToString(); 
                          TMProjectCategory.ProjectList.Add(demo);
                      }
                  }
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return TMProjectCategory.ProjectList;
      }
        #endregion Project Category

        #region TimesheetEntry Approval
      public List<TimesheetHistory> TMTimesheetEntryApproval(string User_Id)
      {
          List<TimesheetHistory> TMHistory = new List<TimesheetHistory>();

          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetTimesheetEntryPendingApprovalList(User_Id);
              if (ds != null)
              {
                  if (ds.Tables.Count > 0)
                  {

                      foreach (DataRow dr in ds.Tables[0].Rows)
                      {
                          TimesheetHistory objmodel = new TimesheetHistory();
                          objmodel.TimeSheetId = Convert.ToInt32(dr["TimeSheetId"].ToString());
                          objmodel.EmpName = dr["EmpName"].ToString();
                          objmodel.FromDate = Convert.ToDateTime(dr["StartDate"].ToString());
                          objmodel.StatusCode = dr["StatusShortCode"].ToString();
                          objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                          objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                          TMHistory.Add(objmodel);
                      }
                  }
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return TMHistory;
      }

      public int UpdateTimesheetEntry(int statusCode, string ApproverRemark, int TimeSheetId, string User_Id)
      {
       
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              return objdal.UpdateTimesheetEntry(statusCode, ApproverRemark, TimeSheetId, User_Id);
   
          }
          catch (Exception ex)
          {

              throw ex;
          }
         
      }
        #endregion  TimesheetEntry Approval

      public DataSet GetEmpmanagerInfo(string User_Id)
      {
            DataSet ds = new DataSet();
          try
          {
               
              TMLeaveDal objdal = new TMLeaveDal();

              ds = objdal.GetEmpmanagerInfo(User_Id);             
          }
          catch (Exception ex)
          {
              
              throw;
          }
          return ds;
      }
      public DataSet GetLeaveInfo(int LeaveId)
      {
          DataSet ds = new DataSet();
          try
          {

              TMLeaveDal objdal = new TMLeaveDal();

              ds = objdal.GetLeaveInfo(LeaveId);
          }
          catch (Exception ex)
          {

              throw;
          }
          return ds;
      }

      #region Timesheet Travel Request


      public List<ProjectTravelRequestModel> GetTimeTravelReuestlist(string User_Id)
      {
          List<ProjectTravelRequestModel> tmtravelreuest = new List<ProjectTravelRequestModel>();
          try
          {

              DataSet Ds = new DataSet();
              TMLeaveDal ObjDal = new TMLeaveDal();
              Ds = ObjDal.GetTimeTravelRequestList(User_Id);
              if (Ds != null)
              {
                  if (Ds.Tables.Count > 0)
                  {

                      foreach (DataRow dr in Ds.Tables[0].Rows)
                      {
                          ProjectTravelRequestModel ObjModel = new ProjectTravelRequestModel();
                          ObjModel.RequestId = Convert.ToInt32(dr["RequestId"].ToString());
                          ObjModel.PurposeofVisitDocs = dr["PurposeofVisitDocs"].ToString();
                          ObjModel.Description = dr["Description"].ToString();
                          ObjModel.EmployeeName = dr["EmpId"].ToString();
                          ObjModel.VendorName = dr["VendorId"].ToString();
                          ObjModel.ApproverBy = dr["ApproverId"].ToString();

                          if (dr["ApprovedDate"].ToString() != null && dr["ApprovedDate"].ToString() != "")
                          {

                              ObjModel.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                          }
                          ObjModel.CustomerName = dr["CustomerId"].ToString();

                          ObjModel.StatusName = dr["Status"].ToString();

                          ObjModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                          ObjModel.CreatedBy = dr["CreatedBy"].ToString();
                          ObjModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                          ObjModel.ModifiedBy = dr["ModifiedBy"].ToString();
                          tmtravelreuest.Add(ObjModel);
                      }
                  }
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return tmtravelreuest;
      }

      public int SaveTMTravelRequest(ProjectTravelRequestModel objmodel, string User_Id)
      {
          int errcode = 0;
          try
          {
              TMLeaveDal objdal = new TMLeaveDal();
              DataSet ds = new DataSet();
              ds = objdal.GetselectedTMTravelRequestList(0);
              if (ds != null)
              {
                  ds.Tables[0].Rows.Clear();
                  DataRow dr = ds.Tables[0].NewRow();
                  dr["RequestId"] = objmodel.RequestId;
                  dr["Description"] = objmodel.Description;
                  dr["CustomerId"] = objmodel.CustomerId;
                  dr["VendorId"] = objmodel.VendorId;
                  dr["EmpId"] = objmodel.EmpId;
                  dr["PostComment"] = objmodel.PostComment;
                  ds.Tables[0].Rows.Add(dr);
                  errcode = objdal.SaveTMTravelRequest(ds, User_Id, objmodel.PurposeofVisitDocs);
              }
          }
          catch (Exception ex)
          {

              throw ex;
          }
          return errcode;
      }

      public ProjectTravelRequestModel GetProjectTravelRequestDatail(int RequestId)
      {
          ProjectTravelRequestModel travelrequest = new ProjectTravelRequestModel();
          try
          {

              TMLeaveDal ObDal = new TMLeaveDal();
              DataSet ds = ObDal.GetselectedTMTravelRequestList(RequestId);


                if (ds != null)
                {
                    travelrequest.RequestId = Convert.ToInt32(ds.Tables[0].Rows[0]["RequestId"].ToString());
                    travelrequest.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                    travelrequest.PostComment = ds.Tables[0].Rows[0]["PostComment"].ToString();
                    travelrequest.PurposeofVisitDocs = ds.Tables[0].Rows[0]["PurposeofVisitDocs"].ToString();
                    travelrequest.EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                    travelrequest.VendorId = Convert.ToInt32(ds.Tables[0].Rows[0]["VendorId"].ToString());
                    travelrequest.CustomerId = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString());
                    travelrequest.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                    travelrequest.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                    travelrequest.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                    travelrequest.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                    travelrequest.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                    if (ds.Tables[0].Rows[0]["ApprovedDate"].ToString() != null && ds.Tables[0].Rows[0]["ApprovedDate"].ToString() != "")
                    {
                        travelrequest.ApprovedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ApprovedDate"].ToString());
                    }

                    if (ds.Tables[0].Rows[0]["ApproverId"].ToString() != null && ds.Tables[0].Rows[0]["ApproverId"].ToString() != "")
                    {
                        travelrequest.ApproverBy = ds.Tables[0].Rows[0]["ApproverId"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["ApproverRemark"].ToString() != null && ds.Tables[0].Rows[0]["ApproverRemark"].ToString() != "")
                    {
                        travelrequest.ApproverRemark = ds.Tables[0].Rows[0]["ApproverRemark"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["ReviewedDate"].ToString() != null && ds.Tables[0].Rows[0]["ReviewedDate"].ToString() != "")
                    {
                        travelrequest.ReviewedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ReviewedDate"].ToString());
                    }

                    if (ds.Tables[0].Rows[0]["ReviewedBy"].ToString() != null && ds.Tables[0].Rows[0]["ReviewedBy"].ToString() != "")
                    {
                        travelrequest.ReviewedBy = ds.Tables[0].Rows[0]["ReviewedBy"].ToString();
                    }
                    if (ds.Tables[0].Rows[0]["ReviewRemark"].ToString() != null && ds.Tables[0].Rows[0]["ReviewRemark"].ToString() != "")
                    {
                        travelrequest.ReviewdRemark = ds.Tables[0].Rows[0]["ReviewRemark"].ToString();
                    }
                }

            }
            catch (Exception ex)
          {
              throw ex;
          }


          return travelrequest;

      }

      public int SaveTravelRequestparticipatns(string PType, string[] selitems, int RequestId, string User_Id)
      {
          try
          {
              TMLeaveDal objDAL = new TMLeaveDal();
              return objDAL.SaveTravelRequestParticipants(PType, selitems, RequestId, User_Id);
          }
          catch (Exception ex)
          {
              throw ex;
          }

      }


      public List<ProjectTravelRequestModel> GetTimeTravelReuestParticipantlist(int RequestId)
      {
          ProjectTravelRequestModel ObModel = new ProjectTravelRequestModel();
          ObModel.LstTravelRequestParticipant = new List<ProjectTravelRequestModel>();

          try
          {
              DataSet Ds = new DataSet();
              TMLeaveDal ObjDal = new TMLeaveDal();
              Ds = ObjDal.TravelRequestParticipantList(RequestId);
              if (Ds != null)
              {
                  if (Ds.Tables.Count > 0)
                  {

                      foreach (DataRow dr in Ds.Tables[0].Rows)
                      {
                          ProjectTravelRequestModel ObjModel = new ProjectTravelRequestModel();
                          ObjModel.RequestId = Convert.ToInt32(dr["RequestId"].ToString());
                          ObjModel.ParticipantId = Convert.ToInt32(dr["ParticipantId"].ToString());
                          ObjModel.ParticipantType = dr["ParticipantType"].ToString();
                          ObjModel.Name = dr["Name"].ToString();

                          ObModel.LstTravelRequestParticipant.Add(ObjModel);



                      }

                  }

              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return ObModel.LstTravelRequestParticipant;
      }

      public int DeleteTravelRequestParticipants(int RequestId, int ParticipantId, string ParticipantsType)
      {
          try
          {
              TMLeaveDal ObDAL = new TMLeaveDal();
              return ObDAL.DeleteTravelRequestParticipants(RequestId, ParticipantId, ParticipantsType);
          }
          catch (Exception ex)
          {
              throw ex;
          }

      }
      public int SaveTravelBudgetRequest(string FileAttachment, string Remark, int RequestId, string BudgetTitle, double Budget)
      {
          try
          {

              TMLeaveDal objDAL = new TMLeaveDal();
              return objDAL.SaveTravelBudget(FileAttachment, Remark, RequestId, Budget, BudgetTitle);
          }
          catch (Exception ex)
          {
              throw ex;
          }

      }

      public List<TravelBudgetModel> GetBudgetTravelReuestlist(int RequestId)
      {
          ProjectTravelRequestModel ObModel = new ProjectTravelRequestModel();

          ObModel.LstTravelBudget = new List<TravelBudgetModel>();

          try
          {
              DataSet Ds = new DataSet();
              TMLeaveDal ObjDal = new TMLeaveDal();
              Ds = ObjDal.GetBudgetTravelRequesList(RequestId);
              if (Ds != null)
              {
                  if (Ds.Tables.Count > 0)
                  {

                      foreach (DataRow dr in Ds.Tables[0].Rows)
                      {
                          TravelBudgetModel ObjModel = new TravelBudgetModel();
                          ObjModel.RequestId = Convert.ToInt32(dr["RequestId"].ToString());
                          ObjModel.Budget = Convert.ToDouble(dr["Budget"].ToString());
                          ObjModel.BudgetTitle = dr["BudgetTitle"].ToString();
                          ObjModel.FileAttachment = dr["FileAttachment"].ToString();
                          ObjModel.Remark = dr["Remark"].ToString();

                          ObModel.LstTravelBudget.Add(ObjModel);



                      }

                  }

              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return ObModel.LstTravelBudget;
      }

      public TravelBudgetModel GetSelectedTravelBudgetList(int RequestId, string BudgetTitle)
      {
          TravelBudgetModel budgetmodel = new TravelBudgetModel();
          DataSet ds = new DataSet();
          try
          {

              TMLeaveDal objdl = new TMLeaveDal();
              ds = objdl.GetEditBudgetTravelRequesList(RequestId, BudgetTitle);
              if (ds == null) return null;

              if (ds != null)
              {
                  if (ds.Tables.Count > 0)
                  {
                      if (ds.Tables[0].Rows.Count > 0)
                      {
                          budgetmodel.RequestId = Convert.ToInt32(ds.Tables[0].Rows[0]["RequestId"].ToString());
                          budgetmodel.BudgetTitle = ds.Tables[0].Rows[0]["BudgetTitle"].ToString();
                          budgetmodel.Budget = Convert.ToDouble(ds.Tables[0].Rows[0]["Budget"].ToString());
                          budgetmodel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                          budgetmodel.FileAttachment = ds.Tables[0].Rows[0]["FileAttachment"].ToString();
                      }
                  }
              }
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return budgetmodel;
      }
        #endregion Timesheet Travel Request

        #region TravelRequestApprovalLIst
        public List<ProjectTravelRequestModel> TravelRequestReviewList(string User_Id)
        {
            List<ProjectTravelRequestModel> list = new List<ProjectTravelRequestModel>();
            TMLeaveDal objdal = new TMLeaveDal();
            DataSet ds = new DataSet();
            ds = objdal.TravelRequestReviewList(User_Id);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ProjectTravelRequestModel obj = new ProjectTravelRequestModel();
                        obj.RequestId = Convert.ToInt32(dr["RequestId"].ToString());
                        obj.Description = dr["Description"].ToString();
                        obj.EmployeeName = dr["EmpName"].ToString();
                        if (dr["CustomerName"].ToString() != null && dr["CustomerName"].ToString() != "")
                        {
                            obj.CustomerName = dr["CustomerName"].ToString();
                        }
                        if (dr["VendorName"].ToString() != null && dr["VendorName"].ToString() != "")
                        {
                            obj.VendorName = dr["VendorName"].ToString();
                        }
                        if (dr["ApproverBy"].ToString() != null && dr["ApproverBy"].ToString() != "")
                        {

                            obj.ApproverBy = dr["ApproverBy"].ToString();
                        }
                        if (dr["ApprovedDate"].ToString() != null && dr["ApprovedDate"].ToString() != "")
                        {
                            obj.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                        }

                        obj.StatusName = dr["Status"].ToString();

                        if (dr["ApproverRemark"].ToString() != null && dr["ApproverRemark"].ToString() != "")
                        {
                            obj.ApproverRemark = dr["ApproverRemark"].ToString();
                        }
                        obj.CreatedBy = dr["CreatedBy"].ToString();
                        obj.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        obj.ModifiedBy = dr["ModifiedBy"].ToString();
                        obj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        list.Add(obj);
                    }
                }
            }
            return list;
        }
        public List<ProjectTravelRequestModel> TravelRequestApprovalList(string User_Id)
        {
            List<ProjectTravelRequestModel> list = new List<ProjectTravelRequestModel>();
            TMLeaveDal objdal = new TMLeaveDal();
            DataSet ds = new DataSet();
            ds = objdal.TravelRequestApprovalLIst(User_Id);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        ProjectTravelRequestModel obj = new ProjectTravelRequestModel();
                        obj.RequestId = Convert.ToInt32(dr["RequestId"].ToString());
                        obj.Description = dr["Description"].ToString();
                        obj.EmployeeName = dr["EmpName"].ToString();
                        if (dr["CustomerName"].ToString() != null && dr["CustomerName"].ToString() != "")
                        {
                            obj.CustomerName = dr["CustomerName"].ToString();
                        }
                        if (dr["VendorName"].ToString() != null && dr["VendorName"].ToString() != "")
                        {
                            obj.VendorName = dr["VendorName"].ToString();
                        }
                        if (dr["ReviewedBy"].ToString() != null && dr["ReviewedBy"].ToString() != "")
                        {

                            obj.ApproverBy = dr["ReviewedBy"].ToString();
                        }
                        if (dr["ReviewedDate"].ToString() != null && dr["ReviewedDate"].ToString() != "")
                        {
                            obj.ApprovedDate = Convert.ToDateTime(dr["ReviewedDate"].ToString());
                        }

                        obj.StatusName = dr["Status"].ToString();

                        if (dr["ReviewRemark"].ToString() != null && dr["ReviewRemark"].ToString() != "")
                        {
                            obj.ApproverRemark = dr["ReviewRemark"].ToString();
                        }
                        obj.Description = dr["Description"].ToString();
                        obj.CreatedBy = dr["CreatedBy"].ToString();
                        obj.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        obj.ModifiedBy = dr["ModifiedBy"].ToString();
                        obj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        list.Add(obj);
                    }
                }
            }
            return list;
        }
        public int ApproveRejectTravel(int statusCode, string ApproverRemark, string User_Id, int RequestId, string Step)
        {
            try
            {
                TMLeaveDal objdal = new TMLeaveDal();
                return objdal.ApproveRejectTravel(statusCode, ApproverRemark, User_Id, RequestId, Step);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public int RequestSendForReview(int statusCode, string User_Id, int RequestId)
        {
            try
            {
                TMLeaveDal objdal = new TMLeaveDal();
                return objdal.RequestSendForReview(statusCode, User_Id, RequestId);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion
    }
}
