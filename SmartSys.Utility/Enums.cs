using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.Utility
{
    public class Enums
    {
        public enum StatusCode
        {
            DOWN = 0,
            Idle = 1,
            NJ = 2,
            JRUN=3,
            BUSY=4,
            STOP=5,
            STOPPED=6,
            RESTART=7,
            START=8,
            GENSU=9,
            GENER=10,
            NPROC=11,
            PU=12,
            SU=13,
            PLER=14,
            MAXRU=15,
            PARTPRO=16,
            VLdEmail=17,
            InVLdEmail=18,
            RDSDISTSU=19,
            RDSERR=20,
            RDSDowldErr=21,
            RDSReDist=22,
            REDISTSTRT=23,
            APPROVED=24,
            REJECTED=25,
            NEW=26,
            REVIEWED=27,
            Applied=28,
            Submitted=34,
            Enquiry=35,
            NonEnquiry=36,
            VendAloDo=37,
            SendVendEnq=38,
            VendResRes=39,
            RateFinliz=40,
            QuatPrep=41,
            SendApp=42,
            SndToPro=43,
          	SignOff=44,
	        QuotSent=45,
	        EstDone=46,
	        PndDiscsn=47,
	        AsignProce=48,
	        AsignQuat=49,
            QuotNew=50,
            PurOrdApp=54,           
            DispSndPro = 53,
            RcptSndapp=54,
            RDY4PRO=65
        }
        public enum FunctionRef
        {
            GetEmployeeForDropDown=1,
            GetRGSReportForDropDown=2,
            RDSGetClientForDropDown=3,
            SysUserForDropDown=4,
            SysBusinessLineForDropDown=5,
            SysDepartmentDropDown=6,
            SysDesignationDropDown=7,
            SysEducatonDropDown=8,
            EWCompanyDropDown=9,
            CLNTUSERTimeSheet = 10,
            CLNTUSERSAJ = 11,
            CLNTUSERAdvent = 12,
            CLNTUSERDPK = 13,
            CLNTCUSTTimeSheet = 14,
            CLNTCUSTSAJ = 15,
            CLNTCUSTAdvent = 16,
            CLNTCUSTDPK = 17,
            GetBankForDropDown = 18,
            BANKUSERSAJ=19,
            BANKUSERAdvent=20,
            BANKUSERDPK=21,
            BANKUSERAll=22,
            SmartSysCustDropDownByUser = 23,
            SmartSysVendorDropDownByUser = 24 ,
            SmartSysProjectDropDownByUser =25,
            ItemDropDown=26,            
            CityDropDown=27,         
            RegionDropDown=28,
            EmpDropDownByUserId=29,
            FYDropDown = 30,
            CurrencyDrpDwn=31,
            YearDrp=32,
            MonthDrp=33
        }

        public enum GenType
        {
            EW=1,
            CR=2
        }
        public enum MailType
        {
            SendSingVendEnq = 1,
            SendAllVendEnq=2,
            SendCustQuot = 3,
            SendClsEnqMail=4
        }
        public enum GenerationOpt
        {
            Dly  = 1,
            Wkly = 2,
            Mnly = 3,
            Qtly = 4,
            Hfly = 5,
            Yrly = 6
        }
        public enum DistributionOpt
        {
            DAST = 1,
            DWR = 2,
            PIZWR = 3,
            PIZADAST=4
        }

        public enum DistribtionMode
        {
            DistributeMannually=1,
            DistributeAuto=2
        }

        public enum DHLoaderKeywords
        {
            DestConn=1
        }
    }
}
