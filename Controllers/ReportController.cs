using CrystalDecisions.CrystalReports.Engine;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.RGS;
using Syncfusion.EJ.Export;

using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SmartSys.Controllers
{
    public class SLExcelStatus
    {
        public string Message { get; set; }
        public bool Success
        {
            get { return string.IsNullOrWhiteSpace(Message); }
        }
    }

    public class SLExcelData
    {
        public SLExcelStatus Status { get; set; }
        public Columns ColumnConfigurations { get; set; }
        public List<string> Headers { get; set; }
        public List<List<string>> DataRows { get; set; }
        public string SheetName { get; set; }

        public SLExcelData()
        {
            Status = new SLExcelStatus();
            Headers = new List<string>();
            DataRows = new List<List<string>>();
        }
    }
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult UploadReport()
        {
            return PartialView();
        }

        public ActionResult GetReportList()
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Report/GetReportList");
            if (FindFlag)
            {
                GeneratorBL objgBl = new GeneratorBL();
                List<Report> reportList = objgBl.getRGSReportList();
                return View(reportList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public void ExportToExcel(string GridModel)
        {
            GeneratorBL objBl = new GeneratorBL();
            var DataSource = objBl.getRGSReportList();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }

        public void ExportToWord(string GridModel)
        {
            GeneratorBL objBl = new GeneratorBL();
            WordExport exp = new WordExport();
            var DataSource = objBl.getRGSReportList();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            exp.Export(obj, DataSource, "Export.docx", false, false, "flat-saffron");
        }
        public void ExportToPdf(string GridModel)
        {
            GeneratorBL objBl = new GeneratorBL();
            PdfExport exp = new PdfExport();
            var DataSource = objBl.getRGSReportList();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            exp.Export(obj, DataSource, "Export.pdf", false, false, "flat-saffron");
        }

        private Syncfusion.JavaScript.Models.GridProperties ConvertGridObject(string gridProperty)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IEnumerable div = (IEnumerable)serializer.Deserialize(gridProperty, typeof(IEnumerable));
            Syncfusion.JavaScript.Models.GridProperties gridProp = new Syncfusion.JavaScript.Models.GridProperties();
            foreach (KeyValuePair<string, object> ds in div)
            {
                var property = gridProp.GetType().GetProperty(ds.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (property != null)
                {
                    Type type = property.PropertyType;
                    string serialize = serializer.Serialize(ds.Value);
                    object value = serializer.Deserialize(serialize, type);
                    property.SetValue(gridProp, value, null);
                }
            }
            return gridProp;
        }

        public ActionResult SaveDefault(IEnumerable<HttpPostedFileBase> UploadDefault)
        {
            foreach (var file in UploadDefault)
            {
                var fileName = Path.GetFileName(file.FileName);
                var destinationPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                //file.SaveAs(destinationPath);

            }
            return Content("");
        }
        public ActionResult RemoveDefault(string[] fileNames)
        {
            foreach (var fullName in fileNames)
            {
                var fileName = Path.GetFileName(fullName);
                var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                if (System.IO.File.Exists(physicalPath))
                {
                    System.IO.File.Delete(physicalPath);
                }
            }
            return Content("");
        }
        [HttpPost]
        public ActionResult UploadReport(HttpPostedFileBase file)
        {

            DataSet dsReport = new DataSet();
            GeneratorBL objBL = new GeneratorBL();
            string ReportId;
            ReportId = "";
            if (Session["ReportDS"] != null)
            {
                dsReport = (DataSet)Session["ReportDS"];
                if (dsReport.Tables.Count > 0)
                    if (dsReport.Tables[0].Rows.Count > 0)
                        ReportId = dsReport.Tables[0].Rows[0]["ReportId"].ToString();
                if (ReportId == "")
                    ReportId = "New";
            }
            else
            {
                dsReport = objBL.GetReportDetails("New");
                ReportId = "New";
            }
            if (file != null)
            {

                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the fielname
                    var fileName = Path.GetFileName(file.FileName);
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    if (!Directory.Exists(Server.MapPath("~/App_Data/uploads")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/App_Data/uploads"));
                    }
                    file.SaveAs(path);
                    string[] fileattr = fileName.Split('.');
                    if (fileattr.Count() < 2)
                    {
                        TempData["Msg"] = "Invalid Report Template File Selection.";
                        return RedirectToAction("GetReport");
                    }
                    if (fileattr[1].ToUpper() == "RPT")
                        ParseCrystalReport(path, dsReport, fileName);
                    else
                    {
                        if (file.ContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" && file.ContentType != "application/vnd.ms-excel.sheet.macroEnabled.12")
                        {
                            TempData["Msg"] = "Please upload a valid excel file of version 2007 and above";
                            return RedirectToAction("GetReport");
                        }
                    }
                    ParseExcelReport(path, dsReport, fileName, file);

                }
                Session["ReportDS"] = dsReport;
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("GetReport");
        }
        private void ParseExcelReport(string path, DataSet dsReport, string fileName, HttpPostedFileBase file)
        {
            var data = new SLExcelData();
            WorkbookPart workbookPart; List<Row> rows;
            try
            {
                FileStream objFileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                int intLength = Convert.ToInt32(objFileStream.Length);
                byte[] objData;
                objData = new byte[intLength];
                objFileStream.Read(objData, 0, intLength);

                var document = SpreadsheetDocument.Open(file.InputStream, false);
                workbookPart = document.WorkbookPart;
                ArrayList Markerlst = new ArrayList();
                ArrayList tmplst = new ArrayList();
                var sheets = workbookPart.Workbook.Descendants<Sheet>();
                SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                SharedStringTable sst = sstpart.SharedStringTable;
                dsReport.Tables[1].Rows.Clear();
                dsReport.Tables[2].Rows.Clear();
                DataRow drRpt;
                string[] rptName = fileName.Split('.');
                if (dsReport.Tables[0].Rows.Count <= 0)
                {
                    drRpt = dsReport.Tables[0].NewRow();
                    drRpt["FileName"] = fileName;
                    drRpt["FileBinary"] = objData;
                    drRpt["BaseReportId"] = 0;
                    drRpt["BusinessLineId"] = 0;
                    drRpt["ReportType"] = "EW";
                    drRpt["Version"] = 1;
                    drRpt["isActive"] = true;
                    drRpt["ReportId"] = rptName[0];
                    dsReport.Tables[0].Rows.Add(drRpt);
                }
                else
                {
                    drRpt = dsReport.Tables[0].Rows[0];
                    drRpt["FileBinary"] = objData;
                }
                foreach (Sheet sheet in sheets)
                {
                    data.SheetName = sheet.Name;

                    var workSheet = ((WorksheetPart)workbookPart
                        .GetPartById(sheet.Id)).Worksheet;

                    var sheetData = workSheet.Elements<SheetData>().First();
                    rows = sheetData.Elements<Row>().ToList();
                    foreach (Row row in rows)
                    {
                        foreach (DocumentFormat.OpenXml.Spreadsheet.Cell c in row.Elements<DocumentFormat.OpenXml.Spreadsheet.Cell>())
                        {
                            string str = "";
                            if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                            {
                                int ssid = int.Parse(c.CellValue.Text);
                                str = sst.ChildElements[ssid].InnerText;
                            }
                            else if (c.CellValue != null)
                            {
                                str = c.CellValue.Text;
                            }
                            if ((!tmplst.Contains(str)) && str.Contains("%%="))
                            {
                                string marker = str.Split('.')[0];
                                if (!Markerlst.Contains(marker))
                                    Markerlst.Add(marker);
                            }
                        }
                    }
                }

                for (int i = 0; i < Markerlst.Count; i++)
                {
                    DataRow dr = dsReport.Tables[1].NewRow();
                    dr["ObjName"] = Markerlst[i].ToString();
                    dr["SPName"] = "";
                    dr["BaseObjId"] = 0;
                    dsReport.Tables[1].Rows.Add(dr);
                }

                objFileStream.Close();
                objFileStream.Dispose();
            }
            catch (Exception e)
            {
                data.Status.Message = "Unable to open the file";
                return;
            }
        }
        private void ParseCrystalReport(string path, DataSet dsReport, string fileName)
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(path);
            PropertyInfo prop = null;
            CrystalDecisions.ReportAppServer.DataDefModel.ISCRTable rasTable = null;
            dsReport.Tables[1].Rows.Clear();
            dsReport.Tables[2].Rows.Clear();

            FileStream objFileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            int intLength = Convert.ToInt32(objFileStream.Length);
            byte[] objData;
            objData = new byte[intLength];
            objFileStream.Read(objData, 0, intLength);
            objFileStream.Close();
            string[] rptName = fileName.Split('.');
            DataRow drRpt;
            if (dsReport.Tables[0].Rows.Count <= 0)
            {
                drRpt = dsReport.Tables[0].NewRow();
                drRpt["FileName"] = fileName;
                drRpt["FileBinary"] = objData;
                drRpt["BaseReportId"] = 0;
                drRpt["BusinessLineId"] = 0;
                drRpt["Version"] = 1;
                drRpt["isActive"] = true;
                drRpt["ReportId"] = rptName[0];
                dsReport.Tables[0].Rows.Add(drRpt);
            }
            else
            {
                drRpt = dsReport.Tables[0].Rows[0];
                drRpt["FileBinary"] = objData;
            }


            foreach (CrystalDecisions.CrystalReports.Engine.Table dt in rd.Database.Tables)
            {
                DataRow dr = dsReport.Tables[1].NewRow();
                prop = dt.GetType().GetProperty("RasTable", BindingFlags.NonPublic | BindingFlags.Instance);
                rasTable = (CrystalDecisions.ReportAppServer.DataDefModel.ISCRTable)prop.GetValue(dt, null);
                if (dsReport != null)
                    if (dsReport.Tables.Count > 0)
                        if (dsReport.Tables[0].Rows.Count > 0)
                            if (dsReport.Tables[0].Rows[0]["ReportId"].ToString().Trim().Length > 0)
                                dr["ObjName"] = dsReport.Tables[0].Rows[0]["ReportId"].ToString();
                if (dr["ObjName"].ToString() == "")
                    dr["ObjName"] = rptName[0].ToUpper();
                dr["SPName"] = rasTable.QualifiedName;
                dr["BaseObjId"] = 0;
                dsReport.Tables[1].Rows.Add(dr);
            }

            foreach (ReportDocument subrpt in rd.Subreports)
            {
                foreach (CrystalDecisions.CrystalReports.Engine.Table dt in subrpt.Database.Tables)
                {
                    DataRow dr = dsReport.Tables[1].NewRow();
                    prop = dt.GetType().GetProperty("RasTable", BindingFlags.NonPublic | BindingFlags.Instance);
                    rasTable = (CrystalDecisions.ReportAppServer.DataDefModel.ISCRTable)prop.GetValue(dt, null);

                    dr["ObjName"] = subrpt.Name;
                    dr["BaseObjId"] = 0;
                    dr["SPName"] = rasTable.QualifiedName;
                    dsReport.Tables[1].Rows.Add(dr);
                }
            }

            foreach (ParameterFieldDefinition pf in rd.DataDefinition.ParameterFields)
            {
                DataRow dr = dsReport.Tables[2].NewRow();
                dr["ParamName"] = pf.Name;
                if (dsReport != null)
                    if (dsReport.Tables.Count > 0)
                        if (dsReport.Tables[0].Rows.Count > 0)
                            if (dsReport.Tables[0].Rows[0]["ReportId"].ToString().Trim().Length > 0)
                                dr["ReportId"] = dsReport.Tables[0].Rows[0]["ReportId"].ToString();
                if (dr["ReportId"].ToString() == "")
                    dr["ReportId"] = rptName[0].ToUpper();
                if (pf.ReportName.Trim() == "")
                    dr["ObjName"] = dr["ReportId"];
                else
                    dr["ObjName"] = pf.ReportName;
                dr["DataType"] = pf.ParameterValueKind;

                if (pf.DefaultValues.Count > 0)  //this parameter has default values
                {
                    foreach (CrystalDecisions.Shared.ParameterDiscreteValue paramVal in pf.DefaultValues)
                    {
                        dr["BaseDefaultValue"] = paramVal.Value.ToString();
                        dr["DefaultValue"] = paramVal.Value.ToString();
                    }
                }
                dr["ParamAlias"] = pf.Name;
                dr["isMandatory"] = "False";
                dr["BaseObjId"] = 0;
                dr["isLinked"] = pf.IsLinked();
                dsReport.Tables[2].Rows.Add(dr);
                //MessageBox.Show(pf.Name);
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetReportFile()
        {
            return View();
        }


        public ActionResult SetExcelMarkerSP(string ObjName, string SPName)
        {
            return RedirectToAction("GetReport");
        }

        public ActionResult SetReportDBConnection(string ObjName, int ConnectionId)
        {
            System.Data.DataSet dsReport;
            System.Data.DataSet dsCon;
            string ReportId;
            AdminBL ObjBL = new AdminBL();
            if (Session["ReportDS"] != null)
            {
                dsReport = (DataSet)Session["ReportDS"];
                ReportId = dsReport.Tables[0].Rows[0]["ReportId"].ToString();

                foreach (DataRow dr in dsReport.Tables[1].Rows)
                {
                    if (dr["ObjName"].ToString() == ObjName)
                    {
                        dsCon = ObjBL.GetSelectedDBConnectionList(ConnectionId);
                        if (dsCon.Tables[0].Rows.Count > 0)
                        {
                            dr["ConnectionId"] = ConnectionId;
                            dr["ServerName"] = dsCon.Tables[0].Rows[0]["ServerName"];
                            dr["DBName"] = dsCon.Tables[0].Rows[0]["DBName"];
                            dr["UserName"] = dsCon.Tables[0].Rows[0]["UserName"];
                            dr["Password"] = dsCon.Tables[0].Rows[0]["Password"];
                            dr["Port"] = dsCon.Tables[0].Rows[0]["Port"];
                            dr["ConnectionType"] = dsCon.Tables[0].Rows[0]["ConnectionType"];
                        }
                    }
                }
            }

            return RedirectToAction("GetReport");
        }
        [HttpPost]
        public ActionResult GetReport(Report rpt, FormCollection fc)
        {
            DataSet dsReport;
            string User_Id = User.Identity.GetUserId();
            if (fc["submit"].ToString() == "Cancel")
                return RedirectToAction("GetReportList");


            if (fc["submit"].ToString() == "Download")
            {
                dsReport = (DataSet)Session["ReportDS"];
                byte[] fileData = (byte[])dsReport.Tables[0].Rows[0]["FileBinary"];
                string[] rptName = fc["FileName"].ToString().Split('.');
                return File(fileData, rptName[1].ToUpper(), fc["FileName"].ToString());

            }
            string _ReportId = "";
            if (fc["ReportName"].ToString() == "")
            {
                TempData["Msg"] = "Report Name Can not be Blank.";
                return RedirectToAction("GetReport");
            }
            if (Session["ReportDS"] != null)
            {
                dsReport = (DataSet)Session["ReportDS"];
                if (dsReport.Tables.Count <= 0)
                {
                    TempData["Msg"] = "Please Upload New Report Template.";
                    return RedirectToAction("GetReport");
                }
                if (dsReport.Tables[0].Rows.Count <= 0)
                {
                    TempData["Msg"] = "Please Upload New Report Template.";
                    return RedirectToAction("GetReport");
                }
                else
                {
                    dsReport.Tables[0].Rows[0]["ReportName"] = fc["ReportName"].ToString();
                }
                if (dsReport.Tables.Count <= 1)
                {
                    TempData["Msg"] = "Please Upload Report Template With at least one Data Source.";
                    return RedirectToAction("GetReport");
                }
                if (dsReport.Tables[1].Rows.Count <= 0)
                {
                    TempData["Msg"] = "Please Upload Report Template With at least one Data Source.";
                    return RedirectToAction("GetReport");
                }
                foreach (DataRow dr in dsReport.Tables[1].Rows)
                {
                    if (dr["ServerName"].ToString() == "" || dr["DBName"].ToString() == "")
                    {
                        TempData["Msg"] = "Please Provide Connection Information to all Data Sources.";
                        return RedirectToAction("GetReport");
                    }
                }

                GeneratorBL objBL = new GeneratorBL();
                _ReportId = dsReport.Tables[0].Rows[0]["ReportId"].ToString();
                DataSet dsOldRpt = objBL.GetReportDetails(_ReportId);
                dsReport.Tables[0].Rows[0]["BusinessLineId"] = fc["BLId"];
                dsReport.Tables[0].Rows[0]["ReportType"] = fc["ReportTypeId"];
                if (dsOldRpt.Tables.Count > 0)
                    if (dsOldRpt.Tables[0].Rows.Count > 0)
                    {
                        //return RedirectToAction("ConfirmReportReplace","Report",dsReport.Tables[0].Rows[0]["ReportId"].ToString());
                        dsReport.Tables[0].Rows[0]["BaseReportId"] = dsOldRpt.Tables[0].Rows[0]["BaseReportId"];
                        bool SameReport = true;
                        foreach (DataTable dt in dsReport.Tables)
                        {
                            if (dt.Rows.Count != dsOldRpt.Tables[dt.TableName].Rows.Count || SameReport == false)
                            {
                                SameReport = false;
                                break;
                            }
                            for (int r = 0; r < dt.Rows.Count; r++)
                            {
                                for (int c = 0; c < dt.Columns.Count; c++)
                                {
                                    if (dt.Rows[r][c].ToString() != dsOldRpt.Tables[dt.TableName].Rows[r][c].ToString())
                                    {
                                        SameReport = false;
                                        break;
                                    }
                                }
                            }

                        }
                        if (SameReport)
                        {
                            TempData["Msg"] = "There is no change in Report Configuration.";
                            return RedirectToAction("GetReport");
                        }

                    }
                dsReport.Tables[0].Rows[0]["ModifiedBy"] = 1;
                dsReport.Tables[0].Rows[0]["CreatedBy"] = 1;
                objBL.SaveReportDetails(dsReport, User_Id);
                Session["ReportDS"] = null;
                TempData["Msg"] = "Data has been saved succeessfully";
            }
            return RedirectToAction("GetReport", "Report", new { ReportId = _ReportId });
        }

        public ActionResult ConfirmReportReplace(string ReportId)
        {
            ReportId = "RPT005";
            GeneratorBL objBl = new GeneratorBL();
            System.Data.DataSet dsReport;
            if (Session["ReportDS"] != null && ReportId == null)
            {
                dsReport = (DataSet)Session["ReportDS"];
                if (dsReport.Tables.Count > 0)
                    if (dsReport.Tables[0].Rows.Count > 0)
                        ReportId = dsReport.Tables[0].Rows[0]["ReportId"].ToString();
                if (ReportId == "")
                    ReportId = "New";
            }
            else
                dsReport = objBl.GetReportDetails(ReportId);
            Report reportObj = new Report();
            if (dsReport.Tables[0].Rows.Count > 0)
            {
                reportObj.ReportId = dsReport.Tables[0].Rows[0]["ReportId"].ToString();
                reportObj.BaseReportId = Convert.ToInt16(dsReport.Tables[0].Rows[0]["BaseReportId"].ToString());
                reportObj.ReportName = dsReport.Tables[0].Rows[0]["ReportName"].ToString();
                reportObj.BusinessLineId = Convert.ToInt16(dsReport.Tables[0].Rows[0]["BusinessLineId"].ToString());
                reportObj.BusinessLineName = dsReport.Tables[0].Rows[0]["BusinessLineName"].ToString();
                reportObj.ReportType = dsReport.Tables[0].Rows[0]["ReportType"].ToString();
                reportObj.CreatedBy = dsReport.Tables[0].Rows[0]["CreatedBy"].ToString();
                if (dsReport.Tables[0].Rows[0]["CreatedDate"].ToString() != "")
                    reportObj.CreatedDate = Convert.ToDateTime(dsReport.Tables[0].Rows[0]["CreatedDate"].ToString());
                reportObj.ModifiedBy = dsReport.Tables[0].Rows[0]["ModifiedBy"].ToString();
                if (dsReport.Tables[0].Rows[0]["ModifiedDate"].ToString() != "")
                    reportObj.ModifiedDate = Convert.ToDateTime(dsReport.Tables[0].Rows[0]["ModifiedDate"].ToString());
                reportObj.FileName = dsReport.Tables[0].Rows[0]["FileName"].ToString();

                reportObj.Version = Convert.ToInt16(dsReport.Tables[0].Rows[0]["Version"].ToString());
                reportObj.BaseRptModifiedBy = dsReport.Tables[0].Rows[0]["BaseRptModifiedBy"].ToString();
                if (dsReport.Tables[0].Rows[0]["BaseRptModifiedDate"].ToString() != "")
                    reportObj.BaseRptModifiedDate = Convert.ToDateTime(dsReport.Tables[0].Rows[0]["BaseRptModifiedDate"].ToString());
                reportObj.FileBinary = (byte[])dsReport.Tables[0].Rows[0]["FileBinary"];
                reportObj.isActive = Convert.ToBoolean(dsReport.Tables[0].Rows[0]["isActive"].ToString());
            }
            //return View(reportObj);
            return File(reportObj.FileBinary, "RPT", reportObj.FileName);
        }

        [HttpPost]
        public ActionResult ConfirmReportReplace()
        {
            return RedirectToAction("GetReport");
        }

        public ActionResult GetReport(string ReportId)
        {
            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Report/GetReport?ReportId=New");
            if (FindFlag)
            {
                GeneratorBL objgBl = new GeneratorBL();
                System.Data.DataSet dsReport;
                if (ReportId == "New")
                {
                    Session["ReportDS"] = null;
                }
                if (Session["ReportDS"] != null && ReportId == null)
                {
                    dsReport = (DataSet)Session["ReportDS"];
                    if (dsReport.Tables.Count > 0)
                        if (dsReport.Tables[0].Rows.Count > 0)
                            ReportId = dsReport.Tables[0].Rows[0]["ReportId"].ToString();
                    if (ReportId == "")
                        ReportId = "New";
                }
                else
                    dsReport = objgBl.GetReportDetails(ReportId);
                Report reportObj = new Report();
                if (dsReport.Tables[0].Rows.Count > 0)
                {
                    reportObj.ReportId = dsReport.Tables[0].Rows[0]["ReportId"].ToString();
                    reportObj.BaseReportId = Convert.ToInt16(dsReport.Tables[0].Rows[0]["BaseReportId"].ToString());
                    reportObj.ReportName = dsReport.Tables[0].Rows[0]["ReportName"].ToString();
                    reportObj.BusinessLineId = Convert.ToInt16(dsReport.Tables[0].Rows[0]["BusinessLineId"].ToString());
                    reportObj.BusinessLineName = dsReport.Tables[0].Rows[0]["BusinessLineName"].ToString();
                    reportObj.ReportType = dsReport.Tables[0].Rows[0]["ReportType"].ToString();
                    reportObj.CreatedBy = dsReport.Tables[0].Rows[0]["CreatedBy"].ToString();
                    if (dsReport.Tables[0].Rows[0]["CreatedDate"].ToString() != "")
                        reportObj.CreatedDate = Convert.ToDateTime(dsReport.Tables[0].Rows[0]["CreatedDate"].ToString());
                    reportObj.ModifiedBy = dsReport.Tables[0].Rows[0]["ModifiedBy"].ToString();
                    if (dsReport.Tables[0].Rows[0]["ModifiedDate"].ToString() != "")
                        reportObj.ModifiedDate = Convert.ToDateTime(dsReport.Tables[0].Rows[0]["ModifiedDate"].ToString());
                    reportObj.FileName = dsReport.Tables[0].Rows[0]["FileName"].ToString();

                    reportObj.Version = Convert.ToInt16(dsReport.Tables[0].Rows[0]["Version"].ToString());
                    reportObj.BaseRptModifiedBy = dsReport.Tables[0].Rows[0]["BaseRptModifiedBy"].ToString();
                    if (dsReport.Tables[0].Rows[0]["BaseRptModifiedDate"].ToString() != "")
                        reportObj.BaseRptModifiedDate = Convert.ToDateTime(dsReport.Tables[0].Rows[0]["BaseRptModifiedDate"].ToString());
                    reportObj.FileBinary = (byte[])dsReport.Tables[0].Rows[0]["FileBinary"];
                    reportObj.isActive = Convert.ToBoolean(dsReport.Tables[0].Rows[0]["isActive"].ToString());
                    List<BaseReportDBObj> lstDBObj = new List<BaseReportDBObj>();
                    List<ReportParameter> lstParams = new List<ReportParameter>();

                    foreach (System.Data.DataRow dr in dsReport.Tables[1].Rows)
                    {
                        BaseReportDBObj dbobj = new BaseReportDBObj();
                        dbobj.ObjName = dr["ObjName"].ToString();
                        dbobj.SPName = dr["SPName"].ToString();
                        dbobj.ServerName = dr["ServerName"].ToString();
                        dbobj.DBName = dr["DBName"].ToString();
                        dbobj.UserName = dr["UserName"].ToString();
                        dbobj.Password = dr["Password"].ToString();
                        if (dr["Port"].ToString() != dr["Port"].ToString())
                            dbobj.Port = Convert.ToInt16(dr["Port"].ToString());
                        dbobj.ConnectionType = dr["ConnectionType"].ToString();
                        if (dr["ConnectionId"].ToString() != "")
                            dbobj.ConnectionId = Convert.ToInt16(dr["ConnectionId"].ToString());
                        dbobj.BaseObjId = Convert.ToInt32(dr["BaseObjId"].ToString());
                        lstDBObj.Add(dbobj);
                    }

                    foreach (System.Data.DataRow dr in dsReport.Tables[2].Rows)
                    {
                        ReportParameter dbobj = new ReportParameter();
                        dbobj.ReportId = dr["ReportId"].ToString();
                        dbobj.ParamName = dr["ParamName"].ToString();
                        dbobj.DataType = dr["DataType"].ToString();
                        dbobj.DefaultValue = dr["DefaultValue"].ToString();
                        dbobj.ParamAlias = dr["ParamAlias"].ToString();
                        dbobj.LableName = dr["LableName"].ToString();
                        dbobj.isMandatory = Convert.ToBoolean(dr["isMandatory"].ToString());
                        if (!(dr["isLinked"].ToString() == ""))
                            dbobj.isLinked = Convert.ToBoolean(dr["isLinked"].ToString());
                        dbobj.ObjName = dr["ObjName"].ToString();
                        dbobj.BaseDefaultValue = dr["BaseDefaultValue"].ToString();
                        dbobj.BaseObjId = Convert.ToInt32(dr["BaseObjId"].ToString());
                        lstParams.Add(dbobj);
                    }
                    reportObj.BaseDBObjects = lstDBObj;
                    reportObj.ReportParams = lstParams;
                }
                else
                {
                    reportObj.ReportId = "New";
                    List<BaseReportDBObj> lstDBObj = new List<BaseReportDBObj>();
                    List<ReportParameter> lstParams = new List<ReportParameter>();
                    reportObj.BaseDBObjects = lstDBObj;
                    reportObj.ReportParams = lstParams;
                }

                List<BusinessLine> businessLines = new List<BusinessLine>();
                AdminBL objAdmin = new AdminBL();

                DataSet ds = objAdmin.GetBusinessLineList();
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    BusinessLine businessLine = new BusinessLine();
                    businessLine.BusinessLineId = Convert.ToInt16(dr["BusinessLineId"].ToString());
                    businessLine.BusinessLineName = dr["BusinessLineName"].ToString();
                    businessLines.Add(businessLine);
                }

                List<ReportType> reportTypes = new List<ReportType>();
                ds = objgBl.GetReportTypeList();
                foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                {
                    ReportType reportType = new ReportType();
                    reportType.ReportTypeID = dr["ReportType"].ToString();
                    reportType.ReportTypeName = dr["ReportTypeName"].ToString();
                    reportTypes.Add(reportType);
                }

                Session["ReportDS"] = dsReport;
                ViewBag.ListOfRT = new SelectList(reportTypes, "ReportTypeID", "ReportTypeName");
                ViewBag.ReportTypeID = reportObj.ReportType;

                ViewBag.ListOfBL = new SelectList(businessLines, "BusinessLineId", "BusinessLineName");
                ViewBag.BLId = reportObj.BusinessLineId;

                ViewBag.ReportId = reportObj.ReportId;
                return View(reportObj);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult GetSP(FormCollection fc)
        {
            string s = fc["cmbSPName"].ToString();
            string objName = fc["ObjName"].ToString();
            DataSet dsReport = new DataSet();
            if (Session["ReportDS"] != null)
            {
                dsReport = (DataSet)Session["ReportDS"];
            }
            foreach (DataRow dr in dsReport.Tables[1].Rows)
            {
                if (dr["ObjName"].ToString() == objName)
                {
                    DataRow[] advRow = dsReport.Tables[2].Select("ObjName='" + objName + "'");
                    for (int i = advRow.Length - 1; i >= 0; i--)
                    {
                        dsReport.Tables[2].Rows.Remove(advRow[i]);
                    }
                    dsReport.Tables[2].AcceptChanges();
                    ArrayList ConnInfo = new ArrayList();
                    ConnInfo.Add(dr["ConnectionId"]);
                    ConnInfo.Add(dr["ConnectionType"]);
                    ConnInfo.Add(dr["ServerName"]);
                    ConnInfo.Add(dr["DBName"]);
                    ConnInfo.Add(dr["UserName"]);
                    ConnInfo.Add(dr["Password"]);
                    ConnInfo.Add(dr["Port"]);
                    GeneratorBL objgen = new GeneratorBL();
                    DataSet dsParams = new DataSet();
                    if (dr["ConnectionType"].ToString().ToUpper() == "MYSQL")
                    {
                        dsParams = objgen.ExecuteUnderlyingCommand("SELECT PARAMETER_NAME Parameter_name, DATA_TYPE Type FROM information_schema.parameters WHERE SPECIFIC_NAME = '" + s + "';", ConnInfo);
                        dr["SPName"] = s;
                        if (dsParams != null)
                            if (dsParams.Tables.Count > 0)
                                foreach (DataRow drParam in dsParams.Tables[0].Rows)
                                {
                                    DataRow newrow = dsReport.Tables[2].NewRow();
                                    newrow["ReportId"] = dsReport.Tables[0].Rows[0]["ReportId"];
                                    newrow["ParamName"] = drParam["Parameter_name"];
                                    switch (drParam["Type"].ToString())
                                    {
                                        case "varchar":
                                        case "nvarchar":
                                            newrow["DataType"] = "StringParameter";
                                            break;
                                        case "date":
                                            newrow["DataType"] = "DateTimeParameter";
                                            break;
                                        case "int":
                                        case "bigint":
                                            newrow["DataType"] = "NumberParameter";
                                            break;
                                        default:
                                            newrow["DataType"] = "NumberParameter";
                                            break;
                                    }
                                    newrow["ParamAlias"] = drParam["Parameter_name"];
                                    newrow["LableName"] = drParam["LableName"];
                                    newrow["isMandatory"] = 1;
                                    newrow["isLinked"] = 0;
                                    newrow["ObjName"] = objName;
                                    newrow["BaseObjId"] = 0;
                                    dsReport.Tables[2].Rows.Add(newrow);

                                }
                    }
                    else
                    {
                        dsParams = objgen.ExecuteUnderlyingCommand("sp_help '" + s + "'", ConnInfo);
                        dr["SPName"] = s;
                        if (dsParams != null)
                            if (dsParams.Tables.Count > 1)
                                foreach (DataRow drParam in dsParams.Tables[1].Rows)
                                {
                                    DataRow newrow = dsReport.Tables[2].NewRow();
                                    newrow["ReportId"] = dsReport.Tables[0].Rows[0]["ReportId"];
                                    newrow["ParamName"] = drParam["Parameter_name"];
                                    switch (drParam["Type"].ToString())
                                    {
                                        case "varchar":
                                        case "nvarchar":
                                            newrow["DataType"] = "StringParameter";
                                            break;
                                        case "date":
                                            newrow["DataType"] = "DateTimeParameter";
                                            break;
                                        case "int":
                                            newrow["DataType"] = "NumberParameter";
                                            break;
                                        case "float":
                                            newrow["DataType"] = "FloatParameter";
                                            break;
                                        default:
                                            newrow["DataType"] = "NumberParameter";
                                            break;
                                    }
                                    newrow["ParamAlias"] = drParam["Parameter_name"];
                                    // newrow["LableName"] = drParam["LableName"];
                                    newrow["isMandatory"] = 1;
                                    newrow["isLinked"] = 0;
                                    newrow["ObjName"] = objName;
                                    newrow["BaseObjId"] = 0;
                                    dsReport.Tables[2].Rows.Add(newrow);

                                }
                    }

                }
            }
            return RedirectToAction("GetReport");
        }
        public ActionResult GetSP(string ObjName, int ConnectionId)
        {
            StoredProcedureList sp = new StoredProcedureList();
            List<string> lst = new List<string>();
            sp.objName = ObjName;
            if (ConnectionId == 0)
                sp.Title = "Please Select Valid Connection First.";
            else
            {
                sp.Title = "Select Stored Procedure.";
                AdminBL objBL = new AdminBL();
                DataSet dsCon = objBL.GetSelectedDBConnectionList(ConnectionId);
                if (dsCon != null)
                {
                    if (dsCon.Tables.Count > 0)
                    {
                        if (dsCon.Tables[0].Rows.Count > 0)
                        {
                            GeneratorBL objgen = new GeneratorBL();
                            string cmdtext = "";
                            if (dsCon.Tables[0].Rows[0]["ConnectionType"].ToString().ToUpper() == "MYSQL")
                            {
                                cmdtext = "show procedure status";
                            }
                            else
                            {
                                cmdtext = "SELECT name  FROM dbo.sysobjects WHERE (type = 'P') and  name not like '%asp%' and name not like '%sp_DH%' and name not like '%sp_DW%' and name not like '%sp_RDS%' and name not like '%sp_RGS%' and name not like '%sp_Save%' and name not like '%sp_Sys%'order by 1"; 
                            }
                            ArrayList ConnInfo = new ArrayList();
                            ConnInfo.Add(dsCon.Tables[0].Rows[0]["ConnectionId"]);
                            ConnInfo.Add(dsCon.Tables[0].Rows[0]["ConnectionType"]);
                            ConnInfo.Add(dsCon.Tables[0].Rows[0]["ServerName"]);
                            ConnInfo.Add(dsCon.Tables[0].Rows[0]["DBName"]);
                            ConnInfo.Add(dsCon.Tables[0].Rows[0]["UserName"]);
                            ConnInfo.Add(dsCon.Tables[0].Rows[0]["Password"]);
                            ConnInfo.Add(dsCon.Tables[0].Rows[0]["Port"]);

                            DataSet dsSPList = objgen.ExecuteUnderlyingCommand(cmdtext, ConnInfo);
                            if (dsSPList != null)
                                if (dsSPList.Tables.Count > 0)
                                    foreach (DataRow dr in dsSPList.Tables[0].Rows)
                                    {
                                        lst.Add(dr["name"].ToString());
                                    }
                            sp.SPName = lst;
                            ViewBag.SPList = new SelectList(sp.SPName);
                        }
                        else
                            sp.Title = "Could not find establish connection, please try later.";
                    }
                    else
                        sp.Title = "Could not find establish connection, please try later.";
                }
                else
                    sp.Title = "Could not find establish connection, please try later.";
            }

            return PartialView(sp);
        }
        public ActionResult GetDBConnection(string ObjName)
        {
            List<DBConnection> lstDBCon = new List<DBConnection>();
            AdminBL objBL = new AdminBL();
            DataSet ds = objBL.GetDBConnectionList();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DBConnection cn = new DBConnection();
                cn.ConnectionId = Convert.ToInt16(dr["ConnectionId"].ToString());
                cn.ConnectionType = dr["ConnectionType"].ToString();
                cn.ServerName = dr["ServerName"].ToString();
                cn.DBName = dr["DBName"].ToString();
                cn.Port = dr["Port"].ToString();
                cn.UserName = dr["UserName"].ToString();
                cn.Password = dr["Password"].ToString();
                cn.ModifiedBy = Convert.ToInt16(dr["ModifiedBy"].ToString());
                cn.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                cn.ObjName = ObjName;
                lstDBCon.Add(cn);
            }
            return PartialView(lstDBCon);
        }

        [HttpGet]
        public ActionResult GetReportParam(int BaseObjId, string ReportId, string ParamName)
        {
            ReportParameter reportParameter = new ReportParameter();
            DataSet dsReport;
            if (Session["ReportDS"] != null)
            {
                dsReport = (DataSet)Session["ReportDS"];

                foreach (DataRow dr in dsReport.Tables[2].Rows)
                {
                    if (Convert.ToInt32(dr["BaseObjId"]) == BaseObjId && ReportId == dsReport.Tables[0].Rows[0]["ReportId"].ToString() && dr["ParamName"].ToString() == ParamName)
                    {
                        reportParameter.BaseObjId = BaseObjId;
                        reportParameter.ReportId = dr["ReportId"].ToString();
                        reportParameter.ParamName = dr["ParamName"].ToString();
                        reportParameter.DataType = dr["DataType"].ToString();
                        reportParameter.DefaultValue = dr["DefaultValue"].ToString();
                        reportParameter.ParamAlias = dr["ParamAlias"].ToString();
                        reportParameter.LableName = dr["LableName"].ToString();
                        reportParameter.isMandatory = Convert.ToBoolean(dr["isMandatory"].ToString());
                        if (dr["isLinked"].ToString() != "")
                            reportParameter.isLinked = Convert.ToBoolean(dr["isLinked"].ToString());
                        else
                            reportParameter.isLinked = false;
                        reportParameter.ObjName = dr["ObjName"].ToString();
                        reportParameter.BaseDefaultValue = dr["BaseDefaultValue"].ToString();

                    }
                }
            }
            return PartialView(reportParameter);
        }

        [HttpPost]
        public ActionResult GetReportParam(int BaseObjId, string ReportId, string ParamName, string LableName, string ParamAlias, string DefaultValue, bool isMandatory)
        {
            DataSet dsReport;
            if (Session["ReportDS"] != null)
            {
                dsReport = (DataSet)Session["ReportDS"];

                foreach (DataRow dr in dsReport.Tables[2].Rows)
                {
                    if (Convert.ToInt32(dr["BaseObjId"]) == BaseObjId && ReportId == dsReport.Tables[0].Rows[0]["ReportId"].ToString() && dr["ParamName"].ToString() == ParamName)
                    {
                        dr["DefaultValue"] = DefaultValue;
                        dr["ParamAlias"] = ParamAlias;
                        dr["LableName"] = LableName;
                        dr["isMandatory"] = isMandatory;
                    }
                }
            }
            return RedirectToAction("GetReport");
        }
        public ActionResult CreateBaseReport()
        {

            SysTaskModel modelObj = new SysTaskModel();
            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            //This is for avoiding the session timeout as tactical solution
            string UserId = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            if (Session["UserMenu"] == null)
            {
                lstTaskmodel = objBl.GetTaskMenuList(UserId);
                Session["UserMenu"] = lstTaskmodel;
            }

            lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
            bool FindFlag;
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Report/GetReportList");
            if (FindFlag)
            {
                List<Report> reportList = new List<Report>();
                try
                {
                    GeneratorBL objgBl = new GeneratorBL();
                    System.Data.DataSet ds = objgBl.GetReportList();
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
                    ViewBag.BussList = new SelectList(reportList, "ReportId", "ReportName");
                    Session["BaseReport"] = reportList;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
    }
}