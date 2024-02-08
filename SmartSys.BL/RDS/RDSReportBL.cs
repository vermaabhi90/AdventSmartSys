using SmartSys.DL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.RDS
{
    
  public class RDSReportBL
    {
      public List<RDSReportModel> GetList()
      {
          List<RDSReportModel> lstRDSReport = new List<RDSReportModel>();
          try
          {
              RDSReportDAL ObjDAl = new RDSReportDAL();
              DataSet dsRDSRep = new DataSet();
              dsRDSRep = ObjDAl.GetList();

              if (dsRDSRep == null) return null;

              if (dsRDSRep.Tables[0].Rows.Count > 0)
              {
                  foreach (DataRow dr in dsRDSRep.Tables[0].Rows)
                  {
                      RDSReportModel lst = new RDSReportModel();
                      lst.ReportId = dr["ReportId"].ToString();
                      lst.ReportName = dr["ReportName"].ToString();
                      lst.ReportType = dr["ReportType"].ToString();
                      lst.BusinessLineId = Convert.ToInt16(dr["BusinessLineId"].ToString());
                      lst.BusinessLineName = dr["BusinessLineName"].ToString();
                     // lst.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                     // lst.CreatedByName = dr["CreatedByName"].ToString();
                      lst.ModifiedByName = dr["ModifiedByName"].ToString();
                      lst.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                      lstRDSReport.Add(lst);
                  }
              }
          }
          catch (Exception ex)
          {

              Common.LogException(ex);
          }
          return lstRDSReport;
      }




      public RDSReportModel GetSelectedListReport(String ReportId)
      {
           RDSReportModel lstRDSReport = new  RDSReportModel();
           lstRDSReport.lstReportDep = new List<RDSReportDep>();
           lstRDSReport.lstTriggerDep = new List<RDSTriggerDep>();
           try
           {
               RDSReportDAL ObjDAl = new RDSReportDAL();
               DataSet dsRDSRep = new DataSet();
               dsRDSRep = ObjDAl.GetSelectedListReport(ReportId);
               if (dsRDSRep == null)
                   return null;
               
                   if (dsRDSRep.Tables[0].Rows.Count > 0)
                   {
                       lstRDSReport.ReportId = ReportId;
                       lstRDSReport.ReportName = dsRDSRep.Tables[0].Rows[0]["ReportName"].ToString();
                       lstRDSReport.ReportType = dsRDSRep.Tables[0].Rows[0]["ReportType"].ToString();
                       lstRDSReport.BusinessLineName = dsRDSRep.Tables[0].Rows[0]["BusinessLineName"].ToString();
                       lstRDSReport.ModifiedByName = dsRDSRep.Tables[0].Rows[0]["ModifiedByName"].ToString();

                   }
                  foreach(DataRow dr in dsRDSRep.Tables[1].Rows)
                  {
                      
                      RDSReportDep rdsReportDep=new RDSReportDep();
                      rdsReportDep.EventId = Convert.ToInt16(dr["EventId"].ToString());
                      rdsReportDep.EventName =dr["EventName"].ToString();
                      rdsReportDep.ReportId = dr["ReportId"].ToString();
                      rdsReportDep.CreatedBy = Convert.ToInt16(dr["CreatedBy"].ToString());
                      rdsReportDep.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                      rdsReportDep.ModifiedByName = dr["ModifiedByName"].ToString();
                      lstRDSReport.lstReportDep.Add(rdsReportDep);
                  }

                  foreach (DataRow dr in dsRDSRep.Tables[2].Rows)
                  {

                      RDSTriggerDep rdsTriggerDep=new RDSTriggerDep();
                      rdsTriggerDep.TriggerId = Convert.ToInt16(dr["TriggerId"].ToString());
                      rdsTriggerDep.TriggerName = dr["TriggerName"].ToString();
                      rdsTriggerDep.ReportId = dr["ReportId"].ToString();
                      rdsTriggerDep.CreatedBy = Convert.ToInt16(dr["CreatedBy"].ToString());
                      rdsTriggerDep.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                      rdsTriggerDep.ModifiedByName = dr["ModifiedByName"].ToString();
                      lstRDSReport.lstTriggerDep.Add(rdsTriggerDep);
                  }
           }
           catch (Exception ex)
           {
               
               Common.LogException(ex);
           }
          
               
           

           return lstRDSReport;
      }

      public List<RDSReportTrigger> GetTriggertList()
      {
          List<RDSReportTrigger> lstTrigger = new List<RDSReportTrigger>();
          try
          {

              RDSReportDAL objDAL = new RDSReportDAL();
              DataSet dsTrigger = objDAL.GetTriggertList();
              if (dsTrigger != null)
              {
                  if (dsTrigger.Tables.Count > 0)
                  {
                      foreach (DataRow dr in dsTrigger.Tables[0].Rows)
                      {
                          RDSReportTrigger objTrigger = new RDSReportTrigger();
                          objTrigger.TriggerId = Convert.ToInt16(dr["TriggerId"].ToString());
                          objTrigger.TriggerName = dr["TriggerName"].ToString();
                          lstTrigger.Add(objTrigger);
                      }
                  }

              }
          }
          catch (Exception ex)
          {

              Common.LogException(ex);
          }
          return lstTrigger;
      }
      public List<RDSReportEvent> GetEventList()
      {

          List<RDSReportEvent> lstEvent = new List<RDSReportEvent>();
           try
           {
               RDSEventDAL objDAL = new RDSEventDAL();
               DataSet dsEventList = objDAL.GetRDSEventList();
               if (dsEventList != null)
               {
                   if (dsEventList.Tables.Count > 0)
                   {
                       foreach (DataRow dr in dsEventList.Tables[0].Rows)
                       {
                           RDSReportEvent eventobj = new RDSReportEvent();
                           eventobj.EventId = Convert.ToInt16(dr["EventId"].ToString());
                           eventobj.EventName = dr["EventName"].ToString();
                           lstEvent.Add(eventobj);

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

      public int SaveEdit(RDSReportModel rdsReportModel, string User_Id)
      {
          int iErrorCode = 0;
         
          try
          {
              DataSet dsSaveEdit = new DataSet();
              DataTable dtSaveEdit = new DataTable("tbl_RDSReport");
              dtSaveEdit.Columns.Add("ReportName", typeof(System.String));
              dtSaveEdit.Columns.Add("ReportId", typeof(System.String));
              DataRow drSaveEdit = dtSaveEdit.NewRow();
              drSaveEdit["ReportName"] = rdsReportModel.ReportName;
              drSaveEdit["ReportId"] =  rdsReportModel.ReportId;

              dtSaveEdit.Rows.Add(drSaveEdit);
              dsSaveEdit.Tables.Add(dtSaveEdit);

              RDSReportDAL ObjDL = new RDSReportDAL();
             
             
                  #region for save Event
                  DataTable dtRDSEventList = new DataTable("tbl_RDSReportEventDep");
                  dtRDSEventList.Columns.Add("ReportId", typeof(System.String));
                  dtRDSEventList.Columns.Add("EventId", typeof(System.Int16));
                  dtRDSEventList.Columns.Add("CreatedBy", typeof(System.Int16));

                  foreach (RDSReportDep rdsReportDep in rdsReportModel.lstReportDep)
                  {
                      DataRow drRDSEventList = dtRDSEventList.NewRow();

                      drRDSEventList["EventId"] = rdsReportDep.EventId;
                      drRDSEventList["ReportId"] = rdsReportModel.ReportId;
                      drRDSEventList["CreatedBy"] = rdsReportDep.CreatedBy;
                      dtRDSEventList.Rows.Add(drRDSEventList);
                  }
                  dsSaveEdit.Tables.Add(dtRDSEventList);
                  #endregion for save Event 

                  #region for save Trigger
                  DataTable dtRDSTriggerList = new DataTable("tbl_RDSReportTriggerDep");
                  dtRDSTriggerList.Columns.Add("ReportId", typeof(System.String));
                  dtRDSTriggerList.Columns.Add("TriggerId", typeof(System.Int16));
                  dtRDSTriggerList.Columns.Add("CreatedBy", typeof(System.Int16));

                  foreach (RDSTriggerDep rdsTriggerDep in rdsReportModel.lstTriggerDep)
                  {
                      DataRow drRDSEventList = dtRDSTriggerList.NewRow();

                      drRDSEventList["ReportId"] = rdsReportModel.ReportId;
                      drRDSEventList["TriggerId"] = rdsTriggerDep.TriggerId;
                      drRDSEventList["CreatedBy"] = rdsTriggerDep.CreatedBy;
                      dtRDSTriggerList.Rows.Add(drRDSEventList);
                  }
                  dsSaveEdit.Tables.Add(dtRDSTriggerList);
                  #endregion for save Trigger

                  iErrorCode = ObjDL.SaveEdit(dsSaveEdit, User_Id);
            
          }
          catch (Exception ex)
          {

              Common.LogException(ex);
          }
         
          return iErrorCode;
         
      }

      public int UpdateStatus(Int32 DlyGenSubId)
      {
          int errCode = 0;
          try
          {
              RDSReportDAL ObjDL = new RDSReportDAL();
              errCode = ObjDL.UpdateStatus(DlyGenSubId);
          }
          catch (Exception ex)
          {
              Common.LogException(ex);
          }
          return errCode;
      }

    }
}
