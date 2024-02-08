using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.DL.Loader;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SmartSys.Controllers
{
    public class LoaderController : Controller
    {
        // GET: Loader
        #region Loader For XLS And CSV

        public class XlsSheet
        {
            public int NoOfSheets { get; set; }
            public int HeaderStartLineInfo { get; set; }
            public int DataStartLineInfo { get; set; }
            public int DataEndLineInfo { get; set; }
            public int NumberOfColumns { get; set; }
            public string SheetName { get; set; }
            public string LocalFilePath { get; set; }

            public List<string> SourceXlsColumnInfo { get; set; }


            public XlsSheet()
            {
                NoOfSheets = 1;
                HeaderStartLineInfo = 1;
                DataStartLineInfo = 1;
                SourceXlsColumnInfo = new List<string>();
            }
        }

        public ActionResult FileLoader()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Loader/FileLoader");
            if (FindFlag)
            {
                if (Session["ErrorReview"] != null)
                {
                    string str = Session["ErrorReview"] as string;
                    TempData["Message"] = str;
                }
                Session["ErrorReview"] = null;
                List<FeedMasterModel> lstFeed = new List<FeedMasterModel>();
                string User_Id = User.Identity.GetUserId();
                LoaderBL BLObj = new LoaderBL();
                lstFeed = BLObj.DHFeedMasterList(User_Id);
                ViewBag.LstFeedMast = new SelectList(lstFeed, "FeedId", "FeedName", "--select option--");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult FileLoader(HttpPostedFileBase file, int FeedId)
        {
            try
            {
                string strExtention = Path.GetExtension(file.FileName);
                TempData["Error"] = null;
                TempData["Uploadedfile"] = file;
                if (strExtention.Trim() == ".xlsx" || strExtention.Trim() == ".xls" || strExtention.Trim() == ".xlsm")
                {
                    return RedirectToAction("XLSLoader", new { FeedId = FeedId });
                }
                else if (strExtention.ToLower().Trim() == ".csv")
                {
                    return RedirectToAction("CSVLoader", new { FeedId = FeedId });
                }
                else
                {
                    TempData["Error"] = "Please Select XLS or CSV file....";
                    return RedirectToAction("FileLoader");
                }
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        #region [XLS Loader]
        [HttpGet]
        public ActionResult XLSLoader(int FeedId)
        {
            try
            {
                HttpPostedFileBase file = null;
                if (TempData["Uploadedfile"] != null)
                {
                    file = TempData["Uploadedfile"] as HttpPostedFileBase;
                }
                var filename = Path.GetFileName(file.FileName);
                var filepath = Path.Combine(Server.MapPath("~/App_Data/Uploads"), filename);
                file.SaveAs(filepath);
                string User_Id = User.Identity.GetUserId();
                string proce = "";
                LoaderBL BLObj = new LoaderBL();
                DataSet dsObj = BLObj.GetDHLoaderParameter(FeedId);
                DataRow dr = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "DestSP");
                if (dr != null)
                {
                    proce = dr["Value"].ToString();
                }
                int ErrorCode = 0;
                DataTable dtDestinationTable = null;
                string strDestconn = "";
                ConnectionModel connectionModel = new ConnectionModel();
                DataSet dsLoadData = new DataSet();
                XlsSheet xlsSheetObj = null;
                string BunchId = DateTime.Now.ToString("dd/MM/yyyy hh:mm") + "_" + Guid.NewGuid().ToString();
                /// Actions Loop
                for (int i = 0; i < dsObj.Tables[1].Rows.Count; i++)
                {

                    switch (dsObj.Tables[1].Rows[i]["ActionTypeCode"].ToString())
                    {
                        case "SourceInfo":
                            {
                                //create new object for each sheet to be exctrated
                                xlsSheetObj = new XlsSheet();
                                dsLoadData = new DataSet();
                                xlsSheetObj.SheetName = dsObj.Tables[1].Rows[i]["description"].ToString().Trim();
                                GetSourceXlsInfo(ref xlsSheetObj, dsObj);
                                break;
                            }
                        case "DestConn":
                            {
                                connectionModel = BLObj.GetConnection(Convert.ToInt16(FeedId), Enums.DHLoaderKeywords.DestConn.ToString());
                                strDestconn = "Data Source =" + connectionModel.ServerName + ";" + "Initial Catalog =" + connectionModel.DBName + ";" + "uid =" + connectionModel.UserName + ";" + "pwd =" + connectionModel.Password + ";";
                                break;
                            }
                        case "DestTable":
                            {
                                dtDestinationTable = CreateDestTable(dsObj, xlsSheetObj.SheetName);
                                break;
                            }
                        case "ProcessData":
                            {
                                LoadDataIntoDestTable(file, dtDestinationTable, ref dsLoadData, xlsSheetObj);

                                break;
                            }
                        case "LoadData":
                            {
                                ErrorCode = BLObj.SaveLoaderData(strDestconn, dsLoadData, proce, User_Id, BunchId);
                                dsLoadData = null;
                                break;
                            }
                        default:
                            break;
                    }

                }
                if(ErrorCode != 600002)
                Session["ErrorReview"] = "Comment successfully Uploaded";
                else
                Session["ErrorReview"] =  "Review not successfully Save.Please try again";
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                Session["ErrorReview"] = ex.Message.ToString() + "Review not successfully Upload.Please try again";
            }

            return RedirectToAction("FileLoader");
        }

        public void GetSourceXlsInfo(ref XlsSheet xlsSheetObj, DataSet dsObj)
        {
            try
            {
                DataRow[] drSourceInfo = dsObj.Tables[0].Select("ActionTypeCode='SourceInfo' and Description = '" + xlsSheetObj.SheetName + "'");
                foreach (DataRow dr in drSourceInfo)
                {
                    switch (dr["ParamName"].ToString())
                    {
                        case "NoOfSheets":
                            xlsSheetObj.NoOfSheets = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "HeaderStartLineNo":
                            xlsSheetObj.HeaderStartLineInfo = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "DataStartLineNo":
                            xlsSheetObj.DataStartLineInfo = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "DataEndLineInfo":
                            xlsSheetObj.DataEndLineInfo = Convert.ToInt32(dr["Value"].ToString());
                            break;
                        case "ColumnDef":
                            xlsSheetObj.SourceXlsColumnInfo.Add(dr["Value"].ToString());
                            break;
                        default:
                            break;
                    }

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable CreateDestTable(DataSet dsObj, string strSheetName)
        {
            DataTable dtDestTable = new DataTable();
            try
            {
                DataRow[] drDestTableInfo = dsObj.Tables[0].Select("ActionTypeCode='DestTable' and Description like '%" + strSheetName + "%'");
                foreach (DataRow dr in drDestTableInfo)
                {
                    if (dr["value"] != null)
                    {
                        string[] arrColumninfo = dr["value"].ToString().Split(',');
                        dtDestTable.Columns.Add(arrColumninfo[0].ToString(), Type.GetType(arrColumninfo[1].ToString()));
                    }
                }
                dtDestTable.TableName = drDestTableInfo[0]["Description"].ToString().Split('.')[1];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtDestTable;
        }
        public void LoadDataIntoDestTable(HttpPostedFileBase file, DataTable dtDestinationTable,
                                            ref DataSet dsLoadData, XlsSheet xlsSheetObj)
        {
            try
            {
                var filename = Path.GetFileName(file.FileName);

                var filepath = Path.Combine(Server.MapPath("~/App_Data/Uploads"), filename);
                // file.SaveAs(filepath);
                //var data = new XlsData();
                DocumentFormat.OpenXml.Packaging.WorkbookPart workbookPart; List<Row> rows;

                //FileStream filStream = new FileStream(filePath, FileMode.Open);

                var document = SpreadsheetDocument.Open(filepath, false);

                workbookPart = document.WorkbookPart;
                var sheets = workbookPart.Workbook.Descendants<Sheet>();

                //get the sheet by name of the sheet
                var sheet = sheets.FirstOrDefault(s => s.Name == xlsSheetObj.SheetName);

                var workSheet = ((WorksheetPart)workbookPart.GetPartById(sheet.Id)).Worksheet;

                var columns = workSheet.Descendants<Columns>().FirstOrDefault();
                // data.ColumnCofiguration = columns;
                var sheetdata = workSheet.Elements<SheetData>().Last();
                rows = sheetdata.Elements<Row>().ToList();


                Dictionary<string, Dictionary<string, string>> DictSourceDestColumnMapping = new Dictionary<string, Dictionary<string, string>>();
                //Dictionary<string, string> DictSourceDestColumnMapping = new Dictionary<string, string>();  

                foreach (string strColumInfo in xlsSheetObj.SourceXlsColumnInfo)
                {
                    string[] strArray = strColumInfo.Split(',');
                    Dictionary<string, string> columnToDataTypeMapping = new Dictionary<string, string>();
                    columnToDataTypeMapping.Add(strArray[0], strArray[1]);
                    // strArray[2] contains Xls column like A,B,C and  strArray[0] contains Columnname in dest Table
                    if (strArray.Count() >= 2)
                    {
                        if (strArray.Count() >= 3)
                            columnToDataTypeMapping.Add("Mandatory", strArray[3]);
                        DictSourceDestColumnMapping.Add(strArray[2], columnToDataTypeMapping);

                    }
                }

                var regex = new Regex("[A-Za-z]+");

                // var cellFormats = workbookPart.WorkbookStylesPart.Stylesheet.CellFormats;
                // var numberingFormats = workbookPart.WorkbookStylesPart.Stylesheet.NumberingFormats;


                for (int i = xlsSheetObj.DataStartLineInfo - 1; i < rows.Count; i++)
                {
                    Row r = rows[i];
                    DataRow dr = dtDestinationTable.NewRow();
                    bool isWrongData = false;
                    foreach (Cell c in r.Elements<Cell>())
                    {
                        var match = regex.Match(c.CellReference);
                        string strColumnName = match.Value;
                        string strText = "";

                        if (DictSourceDestColumnMapping.ContainsKey(strColumnName))
                        {
                            if (c.CellValue != null)
                                strText = c.CellValue.InnerXml;
                            if (c.DataType != null && c.DataType.Value == CellValues.SharedString)
                            {
                                var cellvalue = c.CellValue;
                                strText = workbookPart.SharedStringTablePart.SharedStringTable
                                     .Elements<SharedStringItem>().ElementAt(
                                     Convert.ToInt32(c.CellValue.InnerText)).InnerText;

                                dr[DictSourceDestColumnMapping[strColumnName].Keys.FirstOrDefault()] = strText;
                                if (DictSourceDestColumnMapping[strColumnName]["Mandatory"] == "1" && strText.Length == 0)
                                {
                                    isWrongData = true;
                                }
                            }

                            // else it is a date and xls stores date in interge format
                            else if (DictSourceDestColumnMapping[strColumnName].Values.FirstOrDefault().ToLower() == "system.datetime")
                            {
                                DateTime date;
                                if (DateTime.TryParse(DateTime.FromOADate(Convert.ToDouble(strText)).ToLongDateString(), out date))
                                {
                                    date = DateTime.FromOADate(Convert.ToDouble(strText));
                                    dr[DictSourceDestColumnMapping[strColumnName].Keys.FirstOrDefault()] = date;
                                }
                                //else it is not valid date...
                                else
                                {
                                    isWrongData = true;
                                    break;
                                }
                            }
                            // else cell contains interger or decimal value
                            else if (DictSourceDestColumnMapping[strColumnName].Values.FirstOrDefault().ToLower() == "system.int32"
                                || DictSourceDestColumnMapping[strColumnName].Values.FirstOrDefault().ToLower() == "system.decimal"
                                || DictSourceDestColumnMapping[strColumnName].Values.FirstOrDefault().ToLower() == "system.string")
                            {
                                //check that cell contains valid integer of decimal data..
                                int iNum = 0; Decimal dNum = 0;
                                if (Decimal.TryParse(strText, out dNum) || Int32.TryParse(strText, out iNum))
                                {
                                    dr[DictSourceDestColumnMapping[strColumnName].Keys.FirstOrDefault()] = strText;
                                    if (DictSourceDestColumnMapping[strColumnName]["Mandatory"] == "1" && strText.Length == 0)
                                    {
                                        isWrongData = true;
                                    }
                                }
                                //skip the entire row as one of the cell contains unappropriate data
                                else
                                {
                                    isWrongData = true;
                                    break;
                                }
                            }
                        }
                    }
                    foreach (DataColumn c in dr.Table.Columns)
                    {
                        foreach (KeyValuePair<string, Dictionary<string, string>> entry in DictSourceDestColumnMapping)
                        {
                            string txt = DictSourceDestColumnMapping[entry.Key].Keys.FirstOrDefault();
                            if (entry.Value["Mandatory"] == "1" && dr[c.ColumnName].ToString().Length == 0 && txt == c.ColumnName)
                            {
                                isWrongData = true;
                                break;
                            }
                        }


                    }
                    if (!isWrongData)
                    {

                        dtDestinationTable.Rows.Add(dr);
                    }
                }
                dsLoadData.Tables.Add(dtDestinationTable);
                document.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion [XLS Loader]

        #region [CSV LODER Code]

        [HttpGet]
        public ActionResult CSVLoader(int FeedId)
        {
            HttpPostedFileBase file = null;
            if (TempData["Uploadedfile"] != null)
            {
                file = TempData["Uploadedfile"] as HttpPostedFileBase;
            }

            if (file == null)
            {
                ViewBag.error = "Please select a File.";
                return RedirectToAction("FileLoader  ");
            }
            DataSet dsObj = new DataSet();
            int error = 0;
            string DestconnectionString = "";
            ConnectionModel connectionModel = new ConnectionModel();
            DataSet loaderDS = new DataSet();
            string User_Id = User.Identity.GetUserId();
            string proce = "";
            LoaderBL BLObj = new LoaderBL();
            dsObj = BLObj.GetDHLoaderParameter(FeedId);
            DataRow dataRow = dsObj.Tables[1].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ActionTypeCode"]) == "DestTable");
            DataRow dr = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "DestSP");
            if (dr != null)
            {
                proce = dr["Value"].ToString();
            }
            DataTable dt = new DataTable(Convert.ToString(dataRow["Description"]));
            /// Actions Loop
            for (int i = 0; i < dsObj.Tables[1].Rows.Count; i++)
            {

                switch (dsObj.Tables[1].Rows[i]["ActionTypeCode"].ToString())
                {
                    case "SourceInfo":
                        {
                            break;
                        }
                    case "DestConn":
                        {
                            connectionModel = BLObj.GetConnection(Convert.ToInt16(FeedId), Enums.DHLoaderKeywords.DestConn.ToString());
                            DestconnectionString = "Data Source =" + connectionModel.ServerName + ";" + "Initial Catalog =" + connectionModel.DBName + ";" + "uid =" + connectionModel.UserName + ";" + "pwd =" + connectionModel.Password + ";";
                            break;
                        }
                    case "DestTable":
                        {
                            dt = CreateDestTable(dsObj, dt);
                            break;
                        }
                    case "ProcessData":
                        {
                            loaderDS = LoadDataintoDestTable(dt, dsObj, file);
                            break;
                        }
                    case "LoadData":
                        {
                            error = BLObj.SaveLoaderData(DestconnectionString, loaderDS, proce, User_Id, "");
                            break;
                        }
                    default:
                        break;
                }

            }

            return RedirectToAction("FileLoader");
        }
        private DataTable CreateDestTable(DataSet dsloaderobj, DataTable dt)
        {
            for (int a = 0; a < dsloaderobj.Tables[0].Rows.Count; a++)
            {
                string COlS = Convert.ToString(dsloaderobj.Tables[0].Rows[a]["Value"]);
                string[] ColSplit = COlS.Split(',');
                if (ColSplit.Length > 1 && dsloaderobj.Tables[0].Rows[a]["ActionTypeCode"].ToString() == "DestTable")
                {
                    dt.Columns.Add(ColSplit[0].Trim(), Type.GetType(ColSplit[1].ToString()));
                }
            }

            return dt;
        }
        private DataSet LoadDataintoDestTable(DataTable dt, DataSet ds, HttpPostedFileBase file)
        {
            string fileType = "";
            int NoColInFile = 0;
            int NoHeaderInFile = 0;
            int NoFooterInFile = 0;
            char separator = ' ';
            int Lineseparator = 10;
            DataRow dataRow = null;
            dataRow = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "LineSeparator");
            Lineseparator = Convert.ToInt32(dataRow["Value"].ToString());

            dataRow = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "Separator");
            separator = (Convert.ToChar(dataRow["Value"]));

            dataRow = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "NoHeaderLine");
            NoHeaderInFile = (Convert.ToInt32(dataRow["Value"]));

            dataRow = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "NoFooterLine");
            NoFooterInFile = (Convert.ToInt32(dataRow["Value"]));

            dataRow = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "FileType");
            fileType = (Convert.ToString(dataRow["Value"]));

            dataRow = ds.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["ParamName"]) == "NoColInFile");
            NoColInFile = (Convert.ToInt32(dataRow["Value"]));

            DataRow loaderDR = dt.NewRow();
            DataRow[] drFileType = ds.Tables[0].Select("ParamName<>'ColumnDef' and ActionTypeCode='SourceInfo'");

            DataSet dsLoaderData = new DataSet();

            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            file.SaveAs(path);
            StreamReader sr = new StreamReader(path);
            String line = sr.ReadToEnd();

            string[] CSVRow = line.Split((Char)Lineseparator);
            sr.Close();
            int CSVlineLen = CSVRow.Length;
            foreach (string demo in CSVRow.Skip(NoHeaderInFile))
            {
                loaderDR = dt.NewRow();
                string demoStr = demo.Replace('\r', ' ');
                string NewMailFrom = Regex.Replace(demoStr, @"""("""")*""", " ");
                //   demoStr = demo.Replace('\"', ' ');
                // var result = Regex.Split(demo, ",(?=(?:[^']*[^']*')*[^']*$)");
                MatchCollection matches = new Regex("((?<=\")[^\"]*(?=\"(,|$)+)|(?<=,|^)[^,\"]*(?=,|$))").Matches(NewMailFrom);
                string[] CsvData = new string[matches.Count];
                int i = 0;
                foreach (var match in matches)
                {
                    CsvData[i] = match.ToString();
                    i++;
                }
                // string[] CsvData = demoStr.Split(separator).Select(x => x.Substring(1, x.Length - 2)).ToArray();
                DataRow[] drColDef = ds.Tables[0].Select("ParamName ='ColumnDef' and ActionTypeCode='SourceInfo'");
                foreach (DataRow dr in drColDef)
                {
                    if (dr["ActionTypeCode"].ToString() == "SourceInfo" && dr["ParamName"].ToString() == "ColumnDef" && demo.Trim() != "")
                    {
                        string COlS = dr["Value"].ToString();
                        string[] ColSplit = COlS.Split(separator);

                        switch (ColSplit[1].ToString())
                        {
                            case "":
                                {
                                    break;
                                }
                            case "System.String":
                                {
                                    loaderDR[ColSplit[0]] = Convert.ToString(CsvData[Convert.ToInt32(ColSplit[2])]);
                                    if (Convert.ToString(ColSplit[0]) == "DatasheetPath")
                                    {
                                        string str = DownloadAndUploadData(Convert.ToString(CsvData[Convert.ToInt32(ColSplit[2])]));
                                        loaderDR[ColSplit[0]] = str;
                                    }
                                    if (Convert.ToString(ColSplit[0]) == "ImagePath")
                                    {
                                        string str = DownloadAndUploadData(Convert.ToString(CsvData[Convert.ToInt32(ColSplit[2])]));
                                        loaderDR[ColSplit[0]] = str;
                                    }
                                    break;
                                }
                            case "System.Boolean":
                                {
                                    loaderDR[ColSplit[0]] = Convert.ToBoolean(CsvData[Convert.ToInt32(ColSplit[2])]);
                                    break;
                                }
                            case "System.Int32":
                                {
                                    loaderDR[ColSplit[0]] = Convert.ToInt32(CsvData[Convert.ToInt32(ColSplit[2])]);
                                    break;
                                }
                            case "System.DateTime":
                                {
                                    loaderDR[ColSplit[0]] = Convert.ToDateTime(CsvData[Convert.ToInt32(ColSplit[2])]);
                                    break;
                                }
                            case "System.Decimal":
                                {
                                    loaderDR[ColSplit[0]] = Convert.ToDecimal(CsvData[Convert.ToInt32(ColSplit[2])]);
                                    break;
                                }
                        }
                    }
                }
                dt.Rows.Add(loaderDR);
            }
            dsLoaderData.Tables.Add(dt);
            return dsLoaderData;
        }
        private string DownloadAndUploadData(string link)
        {
            string Path = "";
            Stream stream = null;
            try
            {
                string[] st = link.Split(':');
                if (link.Length > 5)
                {
                    if (st[0].ToString().ToUpper() != "HTTP")
                        link = "http:" + link;
                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(link);
                    WebResponse myResp = myReq.GetResponse();
                    Stream strm = myResp.GetResponseStream();
                    byte[] b = null;
                    using (stream = myResp.GetResponseStream())
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            byte[] buf = new byte[1024];
                            count = stream.Read(buf, 0, 1024);
                            ms.Write(buf, 0, count);
                        } while (stream.CanRead && count > 0);
                        b = ms.ToArray();
                    }
                    string ftpServer = Common.GetConfigValue("FTP");
                    string ftpUid = Common.GetConfigValue("FTPUid");
                    string ftpPwd = Common.GetConfigValue("FTPPWD");
                    string[] FileSplit = link.Split('.');
                    string FileEx = FileSplit[(FileSplit.Length) - 1].ToString();
                    String guid = Guid.NewGuid().ToString();
                    string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                    string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    string FileName = Convert.ToString(FileSplit[1].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
                    FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                    requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                    requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                    Stream uploadStream = null;
                    using (uploadStream = requestFTPUploader.GetRequestStream())
                    {
                        uploadStream.Write(b, 0, b.Length);
                    }
                    uploadStream.Close();
                    requestFTPUploader = null;
                    Path = FileName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Path;
        }
        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        #endregion [CSV LODER Code]

        #region DataBase Loader
        public ActionResult DataBaseLoader(int FeedId)
        {
            try
            {
                LoaderBL BLObj = new LoaderBL();
                DataSet dsObj = new DataSet();
                dsObj = BLObj.GetDHLoaderParameter(FeedId);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            return View();
        }
        #endregion DataBase Loader

        #endregion  Loader For XLS And CSV
    }
}