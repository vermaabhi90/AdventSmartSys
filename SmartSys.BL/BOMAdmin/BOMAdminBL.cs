using ActiveUp.Net.Mail;
using SmartSys.BL.EmailProcessing;
using SmartSys.DL.BOMAdminDAL;
using SmartSys.Utility;
using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
namespace SmartSys.BL.BOMAdmin
{
   public class BOMAdminBL
    {
        #region Paymentterms
        public List<Paytermsmodel> GetPaymentTermsList(string UserId)
        {

            List<Paytermsmodel> PTList = new List<Paytermsmodel>();

            try
            {
                BOMAdminDAL objdal = new BOMAdminDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetPaymentTermsList(UserId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                Paytermsmodel objmodel = new Paytermsmodel();
                                objmodel.PTId = Convert.ToInt32(dr["PTId"].ToString());
                                objmodel.PTCode = dr["PTCode"].ToString();
                                objmodel.Description = dr["Description"].ToString();
                                objmodel.CreatedBy = dr["CreatedBy"].ToString();
                                objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                                objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                PTList.Add(objmodel);


                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return PTList;
        }


        public Paytermsmodel GetSelectedPaymentTerms(int PTId)
        {
            Paytermsmodel PTmodel = new Paytermsmodel();
            DataSet ds = new DataSet();
            try
            {

                BOMAdminDAL objdl = new BOMAdminDAL();
                ds = objdl.GetSelectedPaymentTerms(PTId);
                if (ds == null) return null;

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            PTmodel.PTId = Convert.ToInt32(ds.Tables[0].Rows[0]["PTId"].ToString());
                            PTmodel.PTCode = ds.Tables[0].Rows[0]["PTCode"].ToString();
                            PTmodel.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                            PTmodel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            PTmodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            PTmodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            PTmodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PTmodel;
        }

        public int SavePaymentTerms(Paytermsmodel objmodel, string User_Id)
        {
            BOMAdminDAL admindal = new BOMAdminDAL();
            DataSet ds = new DataSet();

            try
            {
                ds = admindal.GetSelectedPaymentTerms(0);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].Rows.Clear();
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["PTId"] = objmodel.PTId;
                        dr["PTCode"] = objmodel.PTCode;
                        dr["Description"] = objmodel.Description;
                        ds.Tables[0].Rows.Add(dr);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return admindal.SavepaymentTerms(ds, User_Id);
        }
        #endregion Paymentterms

        #region Brand Master

        public int SaveBrandDetail(string User_Id, int BrandId, string BrandName)
        {
            try
            {
                BOMAdminDAL objdal = new BOMAdminDAL();
                return objdal.SaveBrandDetail(User_Id, BrandId, BrandName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<VendorBrandModel> GetBrandList(string UserId)
        {
            List<VendorBrandModel> LstBrandmodel = new List<VendorBrandModel>();
            try
            {
                BOMAdminDAL objdal = new BOMAdminDAL();
                DataSet ds = new DataSet();
                ds = objdal.BrandList(UserId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                VendorBrandModel Model = new VendorBrandModel();
                                Model.BrandId = Convert.ToInt32(dr["BrandId"]);
                                Model.BrandName = dr["BrandName"].ToString();
                                Model.CreatedBy = dr["CreatedBy"].ToString();
                                Model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                                LstBrandmodel.Add(Model);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstBrandmodel;
        }

        public VendorBrandModel GetSelectedBrandList(int BrandId)
        {
            VendorBrandModel Brandmodel = new VendorBrandModel();
            try
            {
                BOMAdminDAL objdal = new BOMAdminDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedBrandList(BrandId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                Brandmodel.BrandId = Convert.ToInt32(dr["BrandId"]);
                                Brandmodel.BrandName = dr["BrandName"].ToString();
                                Brandmodel.CreatedBy = dr["CreatedBy"].ToString();
                                Brandmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Brandmodel;
        }
        #endregion Brand Master

        #region  Brand Item Allocation
        public BrandItemModel GetBrandItemAllocateList(int BrandId)
        {
            BrandItemModel objmodel = new BrandItemModel();
            objmodel.AllocateItemList = new List<BrandItemModel>();
            objmodel.UnAllocateItemList = new List<BrandItemModel>();
            try
            {
                BOMAdminDAL objdal = new BOMAdminDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetBrandItemAllocateList(BrandId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                BrandItemModel Model = new BrandItemModel();
                                Model.ItemId = Convert.ToInt32(dr["ItemId"]);
                                Model.ItemName = dr["ItemName"].ToString();
                                objmodel.AllocateItemList.Add(Model);
                            }
                        }
                    }
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                BrandItemModel Model = new BrandItemModel();
                                Model.ItemId = Convert.ToInt32(dr["ItemId"]);
                                Model.ItemName = dr["ItemName"].ToString();
                                objmodel.UnAllocateItemList.Add(Model);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }
        public int SaveAllocatedBrandItem(BrandItemModel Model, string User_Id, int BrandId)
        {
            int errCode = 0;
            try
            {
                BOMAdminDAL objdal = new BOMAdminDAL();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("tbl_Item");
                dt.Columns.Add("ItemId", typeof(System.Int32));
                dt.Columns.Add("BrandId", typeof(System.Int32));
                foreach (BrandItemModel ItemModel in Model.AllocateItemList)
                {
                    DataRow dr = dt.NewRow();
                    dr["ItemId"] = ItemModel.ItemId;
                    dr["BrandId"] = ItemModel.BrandId;
                    dt.Rows.Add(dr);
                }
                dsObj.Tables.Add(dt);
                errCode = objdal.SaveAllocatedBrandItem(dsObj, User_Id, BrandId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion Brand Item Allocation

        #region  Brand Vendor Allocation
        public BrandVendorModel GetBrandVendorAllocateList(int BrandId)
        {
            BrandVendorModel objmodel = new BrandVendorModel();
            objmodel.AllocateVendorList = new List<BrandVendorModel>();
            objmodel.UnAllocateVendorList = new List<BrandVendorModel>();
            try
            {
                BOMAdminDAL objdal = new BOMAdminDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetBrandVendorAllocateList(BrandId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                BrandVendorModel Model = new BrandVendorModel();
                                Model.VendorId = Convert.ToInt32(dr["VendorId"]);
                                Model.VendorName = dr["VendorName"].ToString();
                                objmodel.AllocateVendorList.Add(Model);
                            }
                        }
                    }
                    if (ds.Tables.Count > 1)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                BrandVendorModel Model = new BrandVendorModel();
                                Model.VendorId = Convert.ToInt32(dr["VendorId"]);
                                Model.VendorName = dr["VendorName"].ToString();
                                objmodel.UnAllocateVendorList.Add(Model);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }
        public int SaveAllocatedBrandVendor(BrandVendorModel Model, string User_Id, int BrandId)
        {
            int errCode = 0;
            try
            {
                BOMAdminDAL objdal = new BOMAdminDAL();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("tbl_Vendor");
                dt.Columns.Add("VendorId", typeof(System.Int32));
                dt.Columns.Add("BrandId", typeof(System.Int32));
                foreach (BrandVendorModel VendorModel in Model.AllocateVendorList)
                {
                    DataRow dr = dt.NewRow();
                    dr["VendorId"] = VendorModel.VendorId;
                    dr["BrandId"] = VendorModel.BrandId;
                    dt.Rows.Add(dr);
                }
                dsObj.Tables.Add(dt);
                errCode = objdal.SaveAllocatedBrandVendor(dsObj, User_Id, BrandId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion Brand Vendor Allocation

        #region EmailProcessing
        public mailmodel GetMailProcessingDetail(string User_Id)
        {

            mailmodel mailmodel = new mailmodel();
            mailmodel.MailList = new List<mailmodel>();
            mailmodel.MailConfig = new List<mailmodel>();
            try
            {
                BOMAdminDAL objdal = new BOMAdminDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetMailDetailList(User_Id);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {

                                mailmodel objmodel = new mailmodel();
                                objmodel.MailId = Convert.ToInt32(dr["MailId"].ToString());
                                objmodel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                                objmodel.ClientMailId = dr["ClientMailID"].ToString();
                                objmodel.Employee = dr["Employee"].ToString();
                                objmodel.Subject = dr["Subject"].ToString();
                                objmodel.From = dr["MailFrom"].ToString();
                                objmodel.TO = dr["MailTo"].ToString();
                                objmodel.BCC = dr["MailBCC"].ToString();
                                objmodel.CC = dr["MailCC"].ToString();
                              
                                objmodel.StatusShortCode = dr["StatusShortCode"].ToString();
                                objmodel.Date = Convert.ToDateTime(dr["ReceivedDate"].ToString());
                                objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                                objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                mailmodel.MailList.Add(objmodel);


                            }
                           
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                mailmodel objmodel = new mailmodel();
                                objmodel.MailDetail = dr["MailDetail"].ToString();
                                mailmodel.MailConfig.Add(objmodel);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return mailmodel;
        }
        public mailmodel GetSubordinateMailProcessingDetail(string User_Id)
        {

            mailmodel mailmodel = new mailmodel();
            mailmodel.MailList = new List<mailmodel>();
            mailmodel.MailConfig = new List<mailmodel>();
            try
            {
                BOMAdminDAL objdal = new BOMAdminDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSubordinateMailDetailList(User_Id);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {

                                mailmodel objmodel = new mailmodel();
                                objmodel.MailId = Convert.ToInt32(dr["MailId"].ToString());
                                objmodel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                                objmodel.ClientMailId = dr["ClientMailID"].ToString();
                                objmodel.Employee = dr["Employee"].ToString();
                                objmodel.Subject = dr["Subject"].ToString();
                                objmodel.From = dr["MailFrom"].ToString();
                                objmodel.TO = dr["MailTo"].ToString();
                                objmodel.BCC = dr["MailBCC"].ToString();
                                objmodel.CC = dr["MailCC"].ToString();

                                objmodel.StatusShortCode = dr["StatusShortCode"].ToString();
                                objmodel.Date = Convert.ToDateTime(dr["ReceivedDate"].ToString());
                                objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                                objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                mailmodel.MailList.Add(objmodel);


                            }

                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                mailmodel objmodel = new mailmodel();
                                objmodel.MailDetail = dr["MailDetail"].ToString();
                                mailmodel.MailConfig.Add(objmodel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return mailmodel;
        }
        public int UpdateEmailProcessing(int Status, int MailId,int CustomerId,string enqnumber,string User_Id,string Type,int CustomerContactId)
        {
            int ErrCode = 0;
            BOMAdminDAL admindal = new BOMAdminDAL();
            DataSet ds = new DataSet();
            try
            {
                if (Type == "Enq")
                {
                    ds = admindal.GetEmailDetailForAttachment(MailId);
                    var mailRepository = new MailRepository(ds.Tables[0].Rows[0]["EmailServer"].ToString(), Convert.ToInt32(ds.Tables[0].Rows[0]["EmailPort"]), true, ds.Tables[0].Rows[0]["EmailUserName"].ToString(), ds.Tables[0].Rows[0]["EmailPassword"].ToString());
                    string FilePath = mailRepository.GetMails("inbox", "UNSEEN", Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"]), Convert.ToInt32(ds.Tables[0].Rows[0]["ClientMailID"]));
                    if (FilePath != "")
                    {
                        ErrCode = admindal.UpdateEmailProcessing(Status, MailId, CustomerId, enqnumber, User_Id, FilePath,CustomerContactId);
                    }              
                }
                else
                {
                    ErrCode = admindal.UpdateEmailProcessing(Status, MailId, CustomerId, enqnumber, User_Id, "", CustomerContactId);
                }
              
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }

            return ErrCode;
        }
        #region Attachment Enq
        public class MailRepository
        {
            private Imap4Client client;

            public MailRepository(string mailServer, int port, bool ssl, string login, string password)
            {
                try
                {
                    if (ssl)

                        Client.ConnectSsl(mailServer, port);
                    else
                        Client.Connect(mailServer, port);
                    Client.Login(login, password);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }       

            protected Imap4Client Client
            {
                get { return client ?? (client = new Imap4Client()); }
            }

            public string GetMails(string mailBox, string searchPhrase, int EmpId, int messageId)
            {
                string str = "";
                try
                {
                    Mailbox mails = Client.SelectMailbox(mailBox);                   
                    Message message = mails.Fetch.MessageObject(messageId);
                    mailmodel model = new mailmodel();
                    model.From = message.From.Email.ToString();
                    model.Subject = message.Subject.ToString();
                    model.Date = message.ReceivedDate;
                    string mailaddds = "";

                    foreach (Address c in message.To)
                    {

                        mailaddds = mailaddds+c.Email + ",";
                    }
                    if (mailaddds.Length > 0)
                        mailaddds = mailaddds.Substring(0, mailaddds.Length - 1);
                    model.TO = mailaddds;
                    string CC = "";
                    foreach (Address c in message.Cc)
                    {
                        CC = CC + c.Email + ",";
                    }
                    if (CC.Length > 0)
                        CC = CC.Substring(0, CC.Length - 1);
                    model.CC = CC;
                    string Bcc = "";
                    foreach (Address c in message.Bcc)
                    {
                        Bcc = Bcc + c.ToString() + ",";
                    }
                    if (Bcc.Length > 0)
                        Bcc = Bcc.Substring(0, Bcc.Length - 1);
                    model.BCC = Bcc;
                    model.BodyText = message.BodyHtml.Text.ToString();                   
                     var flags = new FlagCollection { "Seen" };
                    mails.RemoveFlagsSilent(messageId, flags);                   

                    MailAddress from = new MailAddress(model.From);
                   
                    MailMessage mailMsg = new MailMessage();
                    mailMsg.From = new MailAddress(model.From);
                    mailMsg.To.Add(model.TO);
                    mailMsg.Subject = model.Subject;
                    mailMsg.IsBodyHtml = true;
                    mailMsg.Body = model.BodyText;
                    mailMsg.CC.Add(model.CC);
                  ///  mailMsg.Body = model.BCC;
                    // Add the attachments                  
                    foreach (ActiveUp.Net.Mail.MimePart strFileName in message.Attachments)
                    {
                       
                        Stream stream = new MemoryStream(strFileName.BinaryContent);
                        System.Net.Mail.Attachment at = new System.Net.Mail.Attachment(stream, strFileName.Filename);
                        mailMsg.Attachments.Add(at);
                    }
                   
                    string Folder1 =  Guid.NewGuid().ToString();                
                    string Folder = HttpContext.Current.Server.MapPath("~/App_Data/" + Folder1);
                    SmtpClient client = new SmtpClient("mtp-mail.outlook.com");
                    client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    client.Host = "127.0.0.1";
                    if (!Directory.Exists(Folder))
                        Directory.CreateDirectory(Folder);
                    client.PickupDirectoryLocation = Folder;
                    client.Send(mailMsg); 
                    byte[] b;
                    b = System.IO.File.ReadAllBytes(new System.IO.DirectoryInfo(Folder).GetFiles().First().FullName);
                     str = UploadMail(Folder, b);
                     if (System.IO.Directory.Exists(Folder))
                     {
                         foreach (string file in Directory.GetFiles(Folder))
                         {
                             File.Delete(file);
                         }
                         Directory.Delete(Folder, true);
                     }                                         
                    client.Dispose();
                    
                }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                }

                return str;
            }

            public string   UploadMail(string Path,byte[] byt)
            {
                string FilePath = "";
                FileStream fileStream = null;
                Stream uploadStream = null;
                FileInfo fileInfo = null;
                try
                {                  
                    string ftpServer = Common.GetConfigValue("FTP");                                  
                    String guid = Guid.NewGuid().ToString();
                    string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                    string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                           FilePath = Convert.ToString("Email" + "_" + date + "_" + time + "_" + guid + "." + "eml");
                                     
                    if (ftpServer.Trim().Length > 0)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            try
                            {
                                string ftpUid = Common.GetConfigValue("FTPUid");
                                string ftpPwd = Common.GetConfigValue("FTPPWD");
                                FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FilePath);
                                requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                                string[] array1 = Directory.GetFiles(Path);
                                 fileInfo = new FileInfo(array1[0]);
                                requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                                fileStream = File.OpenRead(array1[0]);
                                int bufferLength = 1024;
                                byte[] buffer = new byte[fileStream.Length];
                                int ByteRead = 0;
                                 uploadStream = requestFTPUploader.GetRequestStream();
                                int contentLength = byt.Length;
                                do
                                {
                                    if (contentLength < bufferLength)
                                        ByteRead = fileStream.Read(buffer, 0, contentLength);
                                    else
                                        ByteRead = fileStream.Read(buffer, 0, bufferLength);
                                    uploadStream.Write(buffer, 0, ByteRead);
                                }
                                while (ByteRead != 0);
                                fileStream.Close();
                                fileStream.Dispose();                              
                                uploadStream.Dispose();
                                fileInfo.Delete();
                                requestFTPUploader = null;
                                fileStream = null;
                                 uploadStream = null;
                                 fileInfo = null;
                                break;                                                          
                            }
                            catch (Exception ex)
                            {

                                if (i > 1)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    fileStream.Close();
                    fileStream.Dispose();
                    uploadStream.Dispose();
                    fileInfo.Delete();
                    FilePath = "";
                    Common.LogException(ex);               
                }                  
                return FilePath;
            }
        }

        #endregion Attachment Enq
        public mailmodel GetSelectedMailDetail(int ClientMailId, int EmpId)
        {
            mailmodel objmodel = new mailmodel(); 

            try
            {
                BOMAdminDAL objdal = new BOMAdminDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedMailDetail(ClientMailId, EmpId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {         
                                objmodel.Subject = dr["Subject"].ToString();
                                objmodel.From = dr["MailFrom"].ToString();
                                objmodel.TO = dr["MailTo"].ToString();
                                objmodel.BCC = dr["MailBCC"].ToString();
                                objmodel.CC = dr["MailCC"].ToString();
                                objmodel.BodyText = dr["BodyText"].ToString();
                             
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objmodel;
        }
        #endregion EmailProcessing

        #region Bank Cheque
        public List<BankChequeModel> GetBankChequeList(string UserId)
        {
            List<BankChequeModel> ChequeList = new List<BankChequeModel>();
            try
            {
                BOMAdminDAL objdal = new BOMAdminDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetBankChequeList(UserId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                BankChequeModel Chequemodel = new BankChequeModel();
                                Chequemodel.ChequeId = Convert.ToInt32(dr["ChequeId"].ToString());
                                Chequemodel.CompCode = Convert.ToString(dr["CompCode"].ToString());
                                Chequemodel.BankName = dr["BankName"].ToString();
                                Chequemodel.ChequeNumber = dr["ChequeNumber"].ToString();
                                Chequemodel.ChequeDocument = dr["ChequeDocument"].ToString();
                                Chequemodel.ChequeDate = Convert.ToDateTime(dr["ChequeDate"].ToString());
                                if (Chequemodel.ChequeDate.ToString() == "1/1/1900 12:00:00 AM")
                                {
                                    Chequemodel.ChequeDateStr = "";
                                }
                                else
                                {
                                    Chequemodel.ChequeDateStr = Chequemodel.ChequeDate.ToShortDateString();
                                }
                                Chequemodel.IssuedToType = dr["IssuedToType"].ToString();
                                Chequemodel.IssuedToName = dr["IssuedToName"].ToString();
                                Chequemodel.Amount = Convert.ToDouble(dr["Amount"].ToString());
                                Chequemodel.Remark = dr["Remark"].ToString();
                                Chequemodel.Currency = dr["Currency"].ToString();
                                Chequemodel.CreatedBy = dr["CreatedBy"].ToString();
                                Chequemodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                Chequemodel.ModifiedBy = dr["ModifiedBy"].ToString();
                                Chequemodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                ChequeList.Add(Chequemodel);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ChequeList;
        }
        public BankChequeModel GetSelectedBankChequeList(int ChequeId)
        {
            BankChequeModel Chequemodel = new BankChequeModel();
            DataSet ds = new DataSet();
            try
            {

                BOMAdminDAL objdl = new BOMAdminDAL();
                ds = objdl.GetSelectedBankChequeList(ChequeId);
                if (ds == null) return null;
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            Chequemodel.ChequeId = Convert.ToInt32(ds.Tables[0].Rows[0]["ChequeId"].ToString());
                            Chequemodel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                            Chequemodel.BankId = Convert.ToInt32(ds.Tables[0].Rows[0]["BankId"].ToString());
                            Chequemodel.BankName = ds.Tables[0].Rows[0]["BankName"].ToString();
                            Chequemodel.ChequeNumber = ds.Tables[0].Rows[0]["ChequeNumber"].ToString();
                            Chequemodel.ChequeDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ChequeDate"].ToString());
                            Chequemodel.ChequeDocument = ds.Tables[0].Rows[0]["ChequeDocument"].ToString();
                            Chequemodel.IssuedToType = ds.Tables[0].Rows[0]["IssuedToType"].ToString();
                            Chequemodel.IssuedToName = ds.Tables[0].Rows[0]["IssuedToName"].ToString();
                            Chequemodel.IssuedToOther = ds.Tables[0].Rows[0]["IssuedToOther"].ToString();
                            Chequemodel.IssuedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["IssuedDate"].ToString());
                            Chequemodel.IssuedToId = Convert.ToInt32(ds.Tables[0].Rows[0]["IssuedToId"].ToString());
                            Chequemodel.Amount = Convert.ToDouble(ds.Tables[0].Rows[0]["Amount"].ToString());
                            Chequemodel.Remark = ds.Tables[0].Rows[0]["Remark"].ToString();
                            Chequemodel.ClearingDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ClearingDate"].ToString());
                            Chequemodel.Currency = ds.Tables[0].Rows[0]["Currency"].ToString();
                            Chequemodel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            Chequemodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            Chequemodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            Chequemodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Chequemodel;
        }

        public int SaveBankChequeDetail(BankChequeModel objmodel, string User_Id)
        {
            BOMAdminDAL admindal = new BOMAdminDAL();
            DataSet ds = new DataSet();

            try
            {
                ds = admindal.GetSelectedBankChequeList(0);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].Rows.Clear();
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["ChequeId"] = objmodel.ChequeId;
                        dr["BankId"] = objmodel.BankId;
                        dr["ChequeNumber"] = objmodel.ChequeNumber;
                        dr["CompCode"] = objmodel.CompCode;
                        dr["ChequeDocument"] = objmodel.ChequeDocument;
                        if (objmodel.ChequeDate.ToString() == "1/1/0001 12:00:00 AM")
                        {
                            dr["ChequeDate"] = "1/1/1900 12:00:00 AM";
                        }
                        else
                        {
                            dr["ChequeDate"] = objmodel.ChequeDate;
                        }
                        dr["IssuedToId"] = objmodel.IssuedToId;
                        dr["IssuedToType"] = objmodel.IssuedToType;

                        if (objmodel.IssuedDate.ToString() == "1/1/0001 12:00:00 AM")
                        {
                            dr["IssuedDate"] = "1/1/1900 12:00:00 AM"; ;
                        }
                        else
                        {
                            dr["IssuedDate"] = objmodel.IssuedDate;
                        }
                     
                        dr["IssuedToOther"] = objmodel.IssuedToOther;
                        dr["Amount"] = objmodel.Amount;
                        dr["Remark"] = objmodel.Remark;
                        dr["Currency"] = objmodel.Currency;
                        if (objmodel.ClearingDate.ToString() == "1/1/0001 12:00:00 AM")
                        {
                            dr["ClearingDate"] = "1/1/1900 12:00:00 AM"; ;
                        }
                        else
                        {
                            dr["ClearingDate"] = objmodel.ClearingDate;
                        }
                       
                        ds.Tables[0].Rows.Add(dr);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return admindal.SaveBankChequeDetail(ds, User_Id);
        }
        #endregion Bank Cheque

        #region TaxOnTax
        public List<TaxModel> TaxList(string UserId)
        {
            List<TaxModel> taxlist = new List<TaxModel>();
            try
            {
                DataSet ds = new DataSet();
                BOMAdminDAL taxdal = new BOMAdminDAL();
                ds = taxdal.GetTaxList(UserId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            TaxModel taxmodel = new TaxModel();
                            taxmodel.TaxId = Convert.ToInt32(dr["TaxId"].ToString());
                            taxmodel.TaxName = dr["TaxName"].ToString();
                            taxmodel.TaxType = dr["TaxType"].ToString();
                            taxmodel.TaxRate = Convert.ToDouble(dr["TaxRate"].ToString());
                            taxlist.Add(taxmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return taxlist;
        }
        public List<TaxTypeModel> GetTaxTypeList()
        {
            List<TaxTypeModel> lsttaxtype = new List<TaxTypeModel>();
            try
            {
                DataSet dsobj = new DataSet();
                BOMAdminDAL taxdal = new BOMAdminDAL();
                dsobj = taxdal.GetTaxType();
                if (dsobj != null)
                {
                    if (dsobj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            TaxTypeModel taxmodel = new TaxTypeModel();
                            taxmodel.TaxType = Convert.ToInt32(dr["TaxType"].ToString());
                            taxmodel.TaxTypeTitle = dr["TaxTypeTitle"].ToString();
                            lsttaxtype.Add(taxmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lsttaxtype;
        }
        public int SaveTaxDetail(TaxModel taxmodel,string User_Id)
        {
            DataSet ds = new DataSet();
            BOMAdminDAL taxdal = new BOMAdminDAL();
            try
            {
                ds = taxdal.GetSelectedTaxDetail(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["TaxId"] = taxmodel.TaxId;
                    dr["TaxName"] = taxmodel.TaxName;
                    dr["TaxType"] = taxmodel.TaxType;
                    dr["TaxRate"] = taxmodel.TaxRate;
                    ds.Tables[0].Rows.Add(dr);
                }
                if (taxmodel.TaxId == 0 & taxmodel.TaxType == "3" & taxmodel.TaxGrid != null)
                {
                    DataTable dt = new DataTable("tbl_TaxOnTax");
                    dt.Columns.Add("TaxId", typeof(System.Int32));
                    dt.Columns.Add("ChildTaxId", typeof(System.Int16));
                    foreach (TaxOnTaxGrid taxontax in taxmodel.TaxGrid)
                    {
                        DataRow dr11 = dt.NewRow();

                        dr11["TaxId"] = taxmodel.TaxId;
                        dr11["ChildTaxId"] = taxontax.TaxOnTaxId;
                        dt.Rows.Add(dr11);
                    }
                    ds.Tables.Add(dt);
                }
                if (taxmodel.TaxId > 0 & taxmodel.TaxType == "3" & taxmodel.TaxOnTaxList != null)
                {
                    DataTable dt = new DataTable("tbl_TaxOnTax");
                    dt.Columns.Add("TaxId", typeof(System.Int32));
                    dt.Columns.Add("ChildTaxId", typeof(System.Int16));
                    foreach (TaxOnTax taxontax in taxmodel.TaxOnTaxList)
                    {
                        DataRow dr11 = dt.NewRow();

                        dr11["TaxId"] = taxmodel.TaxId;
                        dr11["ChildTaxId"] = taxontax.ChildTaxId;

                        dt.Rows.Add(dr11);
                    }
                    ds.Tables.Add(dt);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return taxdal.SaveTaxDetail(ds, taxmodel.TaxId, taxmodel.TaxType, User_Id);

        }
        public TaxModel GetSelectedTaxDetail(int TaxId)
        {
            TaxModel taxmodel = new TaxModel();
            taxmodel.TaxList = new List<TaxModel>();
            try
            {
                DataSet ds = new DataSet();
                BOMAdminDAL taxdal = new BOMAdminDAL();
                ds = taxdal.GetSelectedTaxDetail(TaxId);

                if (ds != null)
                {
                    taxmodel.TaxId = Convert.ToInt32(ds.Tables[0].Rows[0]["TaxId"].ToString());
                    taxmodel.TaxName = ds.Tables[0].Rows[0]["TaxName"].ToString();
                    taxmodel.TaxType = ds.Tables[0].Rows[0]["TaxType"].ToString();
                    taxmodel.TaxRate = Convert.ToDouble(ds.Tables[0].Rows[0]["TaxRate"].ToString());
                }
                if (ds.Tables.Count > 1)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        TaxModel taxdel = new TaxModel();
                        taxdel.TaxId = Convert.ToInt32(dr["TaxId"].ToString());
                        taxdel.TaxName = dr["TaxName"].ToString();
                        taxmodel.TaxList.Add(taxdel);

                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return taxmodel;
        }

        public List<TaxModel> TaxOnTaxLst(int taxId)
        {
            TaxModel taxmodel = new TaxModel();
            taxmodel.TaxList = new List<TaxModel>();

            try
            {
                DataSet ds = new DataSet();
                BOMAdminDAL taxdal = new BOMAdminDAL();
                ds = taxdal.GetSelectedTaxDetail(taxId);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            TaxModel taxdel = new TaxModel();
                            taxdel.TaxId = Convert.ToInt32(dr["TaxId"].ToString());
                            taxdel.TaxName = dr["TaxName"].ToString();

                            taxmodel.TaxList.Add(taxdel);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return taxmodel.TaxList;
        }
        public List<TaxOnTax> TaxOnTaxList(int TaxId)
        {
            TaxModel taxmodel = new TaxModel();
            taxmodel.TaxOnTaxList = new List<TaxOnTax>();

            try
            {
                DataSet ds = new DataSet();
                BOMAdminDAL taxdal = new BOMAdminDAL();

                ds = taxdal.TaxOnTaxList(TaxId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            TaxOnTax demo = new TaxOnTax();
                            demo.TaxId = Convert.ToInt32(dr["TaxId"].ToString());
                            demo.TaxName = dr["TaxName"].ToString();
                            demo.ChildTaxId = Convert.ToInt32(dr["ChildTaxId"].ToString());
                            demo.ParentTaxName = dr["ParentTaxName"].ToString();
                            //demo.TaxType = Convert.ToInt32(dr["TaxType"].ToString());
                            taxmodel.TaxOnTaxList.Add(demo);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return taxmodel.TaxOnTaxList;
        }

        #endregion TaxonTax

        #region Inward Document 
        public List<StockLocation> GetStockLocationListForInward()
        {
            List<StockLocation> SLlst = new List<StockLocation>();
            BOMAdminDAL ObjDal = new BOMAdminDAL();
            DataSet ds = new DataSet();
            ds = ObjDal.GetStockLocationListForInward();
                if (ds.Tables[0].Rows.Count > 0)
            {
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    StockLocation model = new StockLocation();
                    model.STLocationId = Convert.ToInt32(dr["STLocationId"]);
                    model.Name = dr["Name"].ToString();
                    SLlst.Add(model);
                }
            }
            return SLlst;
        }
        public List<InwardDocumentModel> GetInwardDocumentList(string UserId)
        {
            List<InwardDocumentModel> ChequeList = new List<InwardDocumentModel>();
            try
            {
                BOMAdminDAL objdal = new BOMAdminDAL();
                DataSet ds = new DataSet();
                ds = objdal.GetInwardDocumentList(UserId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                InwardDocumentModel Chequemodel = new InwardDocumentModel();
                                Chequemodel.DocId = Convert.ToInt32(dr["DocId"].ToString());
                                Chequemodel.StkLocName = dr["Name"].ToString();
                                Chequemodel.DocName = dr["DocName"].ToString();
                                Chequemodel.DocToType = dr["DocToType"].ToString();
                                Chequemodel.DocToOther = dr["DocToOther"].ToString();
                                Chequemodel.DocDate = Convert.ToDateTime(dr["DocDate"].ToString());
                                if (Chequemodel.DocDate.ToString() == "1/1/1900 12:00:00 AM")
                                {
                                    Chequemodel.DocDateStr = "";
                                }
                                else
                                {
                                    Chequemodel.DocDateStr = Chequemodel.DocDate.ToShortDateString();
                                }
                                Chequemodel.NatureOfDoc = dr["NatureOfDoc"].ToString();
                                Chequemodel.CompCode = dr["CompCode"].ToString();
                                Chequemodel.DocToName = dr["DocToName"].ToString();
                                Chequemodel.Amount = Convert.ToDouble(dr["Amount"].ToString());                                
                                Chequemodel.Currency = dr["Currency"].ToString();
                                Chequemodel.CreatedBy = dr["CreatedBy"].ToString();
                                Chequemodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                Chequemodel.ModifiedBy = dr["ModifiedBy"].ToString();
                                Chequemodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                ChequeList.Add(Chequemodel);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ChequeList;
        }
        public InwardDocumentModel GetSelectedInwardDocumentList(int DocId)
        {
            InwardDocumentModel Chequemodel = new InwardDocumentModel();
            Chequemodel.DocDetailList = new List<InwardDocumentDetailModel>();
            DataSet ds = new DataSet();
            try
            {

                BOMAdminDAL objdl = new BOMAdminDAL();
                ds = objdl.GetSelectedInwardDocumentList(DocId);
                if (ds == null) return null;
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            Chequemodel.DocId = Convert.ToInt32(ds.Tables[0].Rows[0]["DocId"].ToString());
                            Chequemodel.STLocationId = Convert.ToInt32(ds.Tables[0].Rows[0]["STLocationId"].ToString());
                            Chequemodel.DocName = ds.Tables[0].Rows[0]["DocName"].ToString();
                            Chequemodel.DocToType = ds.Tables[0].Rows[0]["DocToType"].ToString();
                            Chequemodel.DocToId = Convert.ToInt32(ds.Tables[0].Rows[0]["DocToId"].ToString());       
                            Chequemodel.DocDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["DocDate"].ToString());
                            Chequemodel.DocToOther = ds.Tables[0].Rows[0]["DocToOther"].ToString();
                            Chequemodel.NatureOfDoc = ds.Tables[0].Rows[0]["NatureOfDoc"].ToString();
                            Chequemodel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                            Chequemodel.DocToName = ds.Tables[0].Rows[0]["DocToName"].ToString();                           
                            Chequemodel.Amount = Convert.ToDouble(ds.Tables[0].Rows[0]["Amount"].ToString());                           
                            Chequemodel.Currency = ds.Tables[0].Rows[0]["Currency"].ToString();
                            Chequemodel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            Chequemodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            Chequemodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            Chequemodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        }
                    }
                    if (ds.Tables.Count > 1)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            InwardDocumentDetailModel DetailModel = new InwardDocumentDetailModel();
                            DetailModel.DocId = Convert.ToInt32(dr["DocId"].ToString());
                            DetailModel.DocumentTitle = dr["DocumentTitle"].ToString();
                            DetailModel.FileName = dr["FileName"].ToString();
                            DetailModel.CreatedBy = dr["CreatedBy"].ToString();
                            DetailModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            Chequemodel.DocDetailList.Add(DetailModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Chequemodel;
        }
        public int SaveInwardDocument(InwardDocumentModel objmodel, string User_Id)
        {
            BOMAdminDAL admindal = new BOMAdminDAL();
            DataSet ds = new DataSet();

            try
            {
                ds = admindal.GetSelectedInwardDocumentList(0);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].Rows.Clear();
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["DocId"] = objmodel.DocId;
                        dr["DocName"] = objmodel.DocName;
                        dr["DocToId"] = objmodel.DocToId;
                        dr["STLocationId"] = objmodel.STLocationId;
                        dr["DocToType"] = objmodel.DocToType;
                        dr["DocToOther"] = objmodel.DocToOther;
                        if (objmodel.DocDate.ToString() == "1/1/0001 12:00:00 AM")
                        {
                            dr["DocDate"] = "1/1/1900 12:00:00 AM"; ;
                        }
                        else
                        {
                            dr["DocDate"] = objmodel.DocDate;
                        }
                        dr["NatureOfDoc"] = objmodel.NatureOfDoc;
                        dr["CompCode"] = objmodel.CompCode;
                        dr["Amount"] = objmodel.Amount;                      
                        dr["Currency"] = objmodel.Currency;                        
                        ds.Tables[0].Rows.Add(dr);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return admindal.SaveInwardDocument(ds, User_Id);
        }
        public int SaveInwardDocumentDetail(InwardDocumentDetailModel objmodel, string User_Id)
        {
            BOMAdminDAL admindal = new BOMAdminDAL();
            DataSet ds = new DataSet();

            try
            {
                ds = admindal.GetSelectedInwardDocumentList(0);
                if (ds != null)
                {
                    if (ds.Tables.Count > 1)
                    {
                        ds.Tables[1].Rows.Clear();
                        DataRow dr = ds.Tables[1].NewRow();
                        dr["DocId"] = objmodel.DocId;
                        dr["FileName"] = objmodel.FileName;
                        dr["DocumentTitle"] = objmodel.DocumentTitle; 
                        ds.Tables[1].Rows.Add(dr);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return admindal.SaveInwardDocumentDetail(ds, User_Id);
        }
        #endregion Inward Document
    }
}
