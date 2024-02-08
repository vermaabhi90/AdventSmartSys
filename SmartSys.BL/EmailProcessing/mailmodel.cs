using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.EmailProcessing
{
    public class mailmodel
    {
        public string message { get; set; }
        public string MailDetail { get; set; }
        public string ClientMailId { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string BodyText { get; set; }
        public string Headers { get; set; }
        public string Employee { get; set; }
        public string TO { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Subject { get; set; }
        public string Day { get; set; }
        public string timeofday { get; set; }
        public int MailId { get; set; }
        public string Comments { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int EmpId { get; set; }
        public int EnquiryId { get; set; }
        public int Status { get; set; }
        public string StatusShortCode { get; set; }
        public List<mailmodel> MailConfig { get; set; }
        public List<mailmodel> MailList { get; set; }
    }
}
