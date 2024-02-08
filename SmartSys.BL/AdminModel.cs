using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SmartSys.BL
{
    #region[System admin Classes]
    [DataContract]
    public class AdminModel
    {
        [DataMember]
        public int TaskId { get; set; }
        [DataMember]
        public int ParentTaskId { get; set; }
        [DataMember]
        public string MenuCaption { get; set; }

        [DataMember]
        public string Href { get; set; }


    }


    /*
     * This class is for adding Storing System User Details from SysterUsers Master web page.
     * 
     */



    public class SystemUser
    {
        [Display(Name = "User Id")]
        public int UserID { get; set; }
        public int EmpId { get; set; }
        public string Email { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        [Display(Name = "Password Hint")]
        public string PasswordHint { get; set; }
        public string UserType { get; set; }
        public string PersonName { get; set; }


    }


    public class SystemRoles
    {
        [Display(Name = "Role ID")]
        public string RoleID { get; set; }
        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Role Name")]
        public String RoleName { get; set; }
    }


    public class SysUserRoleModel
    {
        public SystemUser UserDetails { get; set; }

        public List<SystemRoles> ListRoles { get; set; }
        public List<SystemRoles> AssignedRoles { get; set; }

        public IEnumerable<string> SelectedRoles { get; set; }
        public IEnumerable<string> SelectedRolesToUser { get; set; }

    }

    [DataContract]
    public class SystemGroups
    {
        [DataMember(Order = 1)]
        public int GroupID { get; set; }

        [DataMember(Order = 2)]
        public string GroupName { get; set; }

        [DataMember(Order = 3)]
        public string GroupDescription { get; set; }

    }

    [DataContract]
    public class SysUserGroupList
    {
        [DataMember]
        public SystemGroups SystemGroupsDetails { get; set; }

        [DataMember]
        public int UserID { get; set; }

    }

    [DataContract]
    public class SystemUserDetails
    {
        [DataMember(Order = 1)]
        public List<SystemUser> SysUserList { get; set; }
        [DataMember(Order = 2)]
        public List<SysUserGroupList> SysUserGroupsList { get; set; }
    }

    public class DBConnection
    {
        public int ConnectionId { get; set; }
        public string ConnectionType { get; set; }
        public string ServerName { get; set; }
        public string DBName { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int BaseObjId { get; set; }
        public string ObjName { get; set; }
    }

    #endregion

    #region Menu Models

    public class SysRoleTaskModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string Href { get; set; }
        public int pid { get; set; }
        public bool HasChild { get; set; }
        public bool IsChecked { get; set; }
    }

    public class SysTaskModel
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string MenuAction { get; set; }
        public string ImageUrl { get; set; }
        public string Href { get; set; }
        public int ParentMenuId { get; set; }
        public string Description { get; set; }
        public string TaskName { get; set; }
        public Boolean isMenu { get; set; }
        public List<SysTaskModel> MenuItems { get; set; }

        public bool FindMenu(List<SysTaskModel> modelTasks, String SearchHref)
        {
            bool FindFlag;
            FindFlag = false;
            if (modelTasks != null)
                foreach (SysTaskModel m in modelTasks)
                {

                    if (m.Href == SearchHref)
                    {
                        return true;
                    }
                    else
                    {
                        if (m.MenuItems != null)
                            if (m.MenuItems.Count > 0)
                            {
                                FindFlag = FindMenu(m.MenuItems, SearchHref);
                                if (FindFlag == true)
                                    return true;
                            }
                    }
                }
            return FindFlag;
        }

    }
    public class Dashboard
    {
        public Int16 DashboardId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public Int16 Width { get; set; }
        public Int16 Sequence { get; set; }
        public int ConnectionId { get; set; }
    }
    public class Departmentmodel
    {
        [DisplayName("Department Id")]
        public Int16 DeptId { get; set; }

        [DisplayName("Department Name")]
        public string DeptName { get; set; }
        public int DeptHead { get; set; }
        public int UseDept { get; set; }
        public Int16 CreatedBy { get; set; }
        [DisplayName("Created By")]
        public string CreatedByName { get; set; }
        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }
        public Int16 ModifiedBy { get; set; }
        [DisplayName("Modified By")]
        public string ModifiedByName { get; set; }
        [DisplayName("Department Head")]
        public string DeptHeadName { get; set; }
        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }

    }
    public class SysEmploy
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string Color { get; set; }
        public Boolean selected { get; set; }

    }
    public class SysUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public List<SysTaskModel> Tasks { get; set; }
        public List<Dashboard> dashboardList { get; set; }
    }
    #endregion

    #region [Company Model] 
    public class SysCompanyModel
    {
        public int CompId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pin { get; set; }
        public string TagLine { get; set; }
        public byte[] Logo { get; set; }
        public string CST { get; set; }
        public string BST { get; set; }
        public string ServiceTN { get; set; }
        public string ImagePath { get; set; }
    }
    #endregion

    #region 
    public class SysRolesTaskModel
    {
        [Required(ErrorMessage = "Enter Role ID")]
        public SystemRoles Role { get; set; }
        public List<SysRoleTasks> lstSysRolesTasks { get; set; }
        public List<SysRoleTasks> lstsysRoleTasks { get; set; }

        public List<SysRoleTasks> AssignedTaskList { get; set; }
        public IEnumerable<string> AssignedTasksToRole { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string Href { get; set; }
        public int pid { get; set; }
        public bool HasChild { get; set; }
        public bool IsChecked { get; set; }
    }

    public class SysRoleTasks
    {

        public string RoleID { get; set; }
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public int id { get; set; }
        public int DeptId { get; set; }
        public string name { get; set; }
        public string Href { get; set; }
        public int pid { get; set; }
        public bool HasChild { get; set; }
        public bool IsChecked { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
    #endregion

    #region [Role Report Mapping Model]

    public class RoleRptMapingModel
    {
        public class RoleReportMapping
        {
            public string RoleID { get; set; }
            public string ReportID { get; set; }

        }
        public class DerivedReport
        {
            public string ReportID { get; set; }

            [DisplayName("Report Name")]
            public string ReportName { get; set; }
        }

        public SystemRoles RoleInfo { get; set; }

        public List<DerivedReport> lstRpts { get; set; }
        public List<DerivedReport> lstAssignedRpts { get; set; }
        public List<RoleReportMapping> lstRoleRptMapList { get; set; }

    }



    #endregion

    #region EmployeeCustomerDetail
    public class EmployeeCustomerDetail
    {
        public class EmpCustomerMapping
        {
            public int CustomerId { get; set; }
            public int EmpId { get; set; }
            public int DeptId { get; set; }
            public string DeptName { get; set; }
            public string CustomerName { get; set; }

        }
        public class DerivedCustomer
        {
            public int CustomerId { get; set; }
            public string CustomerName { get; set; }

        }

        //    public SystemRoles RoleInfo { get; set; }

        public string Employee { get; set; }
        public List<DerivedCustomer> lstCustomer { get; set; }
        public List<DerivedCustomer> lstAssignedCustomer { get; set; }
        public List<EmpCustomerMapping> lstEmpCustomerMapping { get; set; }

    }
    public class SysEmp
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
    }

    #endregion EmployeeCustomerDetail

    #region VendorCustomerDetail
    public class EmployeeVendorDetail
    {
        public class EmpVendorMapping
        {
            public int VendorId { get; set; }
            public int EmpId { get; set; }
            public string VendorName { get; set; }

        }
        public class DerivedVendor
        {
            public int VendorId { get; set; }
            public string VendorName { get; set; }

        }

        //    public SystemRoles RoleInfo { get; set; }

        public string Employee { get; set; }
        public List<DerivedVendor> lstVendor { get; set; }
        public List<DerivedVendor> lstAssignedVendor { get; set; }
        public List<EmpVendorMapping> lstEmpVendorMapping { get; set; }

    }
    #endregion VendorCustomerDetail

    #region for Employee Model

    public class EmpChartModel
    {
        public int id { get; set; }
        public int parentId { get; set; }
        [DisplayName("Employee Name")]
        public string EmpName { get; set; }
        [DisplayName("Manager Name")]
        public string Manager { get; set; }
        public string Designation { get; set; }
        public string image { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoBase { get; set; }
        public string Gender { get; set; }
        public string UserName { get; set; }
        public string ReportId { get; set; }
        public string ParamId { get; set; }
        public string ParamName { get; set; }
        public int TxtParamValue { get; set; }
        public string hidText { get; set; }
        public int StatusId { get; set; }
        public string OutputLocation { get; set; }
        public DateTime RunDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProjectName { get; set; }
        public int Timespend { get; set; }
        public List<EmpChartModel> TimeUtilizationLst { get; set; }
    }
    public class EmpSalaryStructureModel
    {
        public int EmpId { get; set; }
        public double Rate { get; set; }
        public int ComponentId { get; set; }
        public double VariableRate { get; set; }
        public double TotalAmount { get; set; }
        public double FixedRate { get; set; }
        public string Name { get; set; }
        public string DRCR { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class EmpTimesheetList
    {
        public int TimeSheetId { get; set; }
        public int TimeSheetEntryId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int TaskId { get; set; }
        public string TaskName { get; set; }
        public string TMDay { get; set; }
        public int TotalTime { get; set; }
        public DateTime TMDate { get; set; }
        public string Remark { get; set; }
    }
    public class EmployeeModel
    {
        [DisplayName("Emp Id")]
        public int EmpId { get; set; }
        public string Gender { get; set; }
        public string CompCode { get; set; }
        public double AnnualFixPay { get; set; }
        public double AnnualVariablePay { get; set; }
        public string Matched { get; set; }
        public string MatchedEmailConfig { get; set; }
        public string PhoneNumber { get; set; }
        public string Mobile { get; set; }
        public string PAN { get; set; }
        public string AADHAR { get; set; }
        public string PassportNumber { get; set; }
        public int LeaveOpBal { get; set; }
        public string EmpsalaryComponent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        [DisplayName("email Id")]
        public string emailId { get; set; }
        [DisplayName("Employee Name")]
        public string EmpName { get; set; }
        [DisplayName("Date Of Join")]
        public DateTime DateOfJoin { get; set; }
        public string DteOfJoin { get; set; }
        [DisplayName("Designation")]
        public string Designation { get; set; }
        [DisplayName("Qualification")]
        public string Qualification { get; set; }
        [DisplayName("Date")]
        public DateTime LastDateOfWork { get; set; }
        [DisplayName("Active")]
        public bool Deleted { get; set; }
        [DisplayName("Remark")]
        public string Remark { get; set; }
        public Int16 UserId { get; set; }
        [DisplayName("User")]
        public string UserName { get; set; }
        public int ManagerId { get; set; }
        [DisplayName("Manager")]
        public string ManagerName { get; set; }
        public Int16 DeptId { get; set; }
        [DisplayName("Department Name")]
        public string DeptName { get; set; }
        public int Region { get; set; }
        public Int16 CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Int16 ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public string DisplayName { get; set; }
        public string AssetsAllow { get; set; }
        public List<SystemUser> lstsysUser { get; set; }
        public bool isClient { get; set; }
        public int TabIndex { get; set; }
        public Int16 CompRelMapCount { get; set; }
        public Byte[] Photo { get; set; }
        public List<ExperienceModel> LstExperience { get; set; }
        public List<ExpertiesModel> LstExperties { get; set; }
        public List<EmployeeBankDetailModel> EmployeeBankDetailLst { get; set; }
        public List<EmployeeAddressModel> AddressList { get; set; }
        public List<EmployeeCustomer> CustList { get; set; }
        public List<EmployeeVendor> VendList { get; set; }
        public EmailConfig emailConfig { get; set; }
        public List<SysEmployeeAssetsModel> SysEmpAssetsList { get; set; }
        public List<Documents> DocList { get; set; }
        public List<Departmentmodel> DeptList { get; set; }
    }
    public class Documents
    {
        public int EmpId { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentName { get; set; }
    }
    public class EmployeeCustomer
    {
        public int EmpId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string DepartmentName { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }

    public class EmployeeVendor
    {
        public int EmpId { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }

    public class EmployeeAddressModel
    {
        public int AddressId { get; set; }
        public int EmpId { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Pin { get; set; }
        public string LandMark { get; set; }
        public string Country { get; set; }
        public int CountryId { get; set; }
        public string State { get; set; }
        public int StateId { get; set; }
        public Boolean isPrimary { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public string PhotoPath { get; set; }
        public byte[] Photobyte { get; set; }
        public List<EmployeeCountryModel> LstCountry { get; set; }
        public List<EmployeeStateModel> LstState { get; set; }
        public List<EmployeeCityModel> LstCity { get; set; }
    }
    public class EmployeeCountryModel
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
    public class EmployeeStateModel
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
    }
    public class EmployeeCityModel
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
    public class EmployeeBankDetailModel
    {
        public int EmpId { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string NewAccountNo { get; set; }
        public double Limit { get; set; }
        public string IFSCCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class ExperienceModel
    {
        public int EmpId { get; set; }
        public string NewCompanyName { get; set; }
        public string CompanyName { get; set; }
        public string Designation { get; set; }
        public string JobProfile { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
    public class ExpertiesModel
    {
        public int EmpId { get; set; }
        public string Experties { get; set; }
        public string NewExperties { get; set; }
        public int Exp_Level { get; set; }
        public double ExpInYears { get; set; }
    }
    public class EmoloyeeMapping
    {
        public string CompCode { get; set; }
        public string CompanyName { get; set; }
        public Int16 ConnectionId { get; set; }
        public string EmpListSP { get; set; }
        public string selectedComp { get; set; }
        public string Emp_No { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public List<EmoloyeeMapping> lstEmolyeeMap { get; set; }
        public List<MapClientEmpList> lstClientMapEmpList { get; set; }
        public List<SysNavRelListModel> GridList { get; set; }
    }
    public class MapClientEmpList
    {
        public string ComEmpID { get; set; }
        public string ComEmpName { get; set; }
    }
    public class SysNavRelListModel
    {
        public string CompEmpID { get; set; }
        public string CompName { get; set; }
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public Int16 ConnectionId { get; set; }
    }
    #endregion for Employee Model

    #region Employee Asset
    public class SysEmployeeAssetsModel
    {
        public int AssetId { get; set; }
        public string AssetName { get; set; }
        public int EmpId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime AllocationDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
    #endregion Employee Asset

    #region for Employee User Mapping Model

    public class SysDashBoardModel
    {

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public Int16 Sequence { get; set; }
        public List<UserDashboardModel> lstUserDashBoard { get; set; }
    }
    public class ListDashBoard
    {
        public Int16 DashboardId { get; set; }
        public string Description { get; set; }
    }
    public class UserDashboardModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public Int16 DashboardId { get; set; }
        public Int16 Sequence { get; set; }
    }
    #endregion for Employee User Mapping Model

    #region  Client Application
    public class ClientAppModel
    {
        [DisplayName("Client App Id")]
        public Int16 ClientAppId { get; set; }

        [DisplayName("App Name")]
        public string AppName { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Login Name")]
        public string LoginName { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Modified By")]
        public int ModifiedBy { get; set; }

        [DisplayName("Modified Date")]
        public DateTime ModifiedDate { get; set; }

        public string User_Id { get; set; }
        public string ModifiedByName { get; set; }
    }
    #endregion  Client Application

    #region  Company

    public class CompanyModel
    {
        public Int16 CompId { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Pin { get; set; }
        public string TagLine { get; set; }
        public byte[] Logo { get; set; }
        public string Cst { get; set; }
        public string Bst { get; set; }
        public string ServiceTN { get; set; }
    }

    #endregion  Company

    #region Approval And Review Pendding List
    public class ApprovalList
    {
        public string Description { get; set; }

        [DisplayName("Pending Approvel")]
        public int PendingApprovel { get; set; }
        public List<ApprovalList> lstApprover { get; set; }
        public string HyperLink { get; set; }
    }
    public class ReviewList
    {
        public string Description { get; set; }

        [DisplayName("Pending Review")]
        public int PendingReview { get; set; }
        public List<ReviewList> lstApprover { get; set; }
        public string HyperLink { get; set; }
    }
    #endregion Approval And Review Pendding List

    #region Bank Detail List

    public class BankListModel
    {
        public string BankId { get; set; }
        public string BankName { get; set; }
    }
    #endregion Bank Detail List 
    #region Customer Id

    public class CustomeridModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
    #endregion Customer id

    #region Assets
    public class SysAssetsModel
    {
        public int AssetId { get; set; }
        public int AssetTypeId { get; set; }
        public string AssetType { get; set; }
        public string AssetName { get; set; }
        public string Description { get; set; }
        // public string AllocatedTo { get; set; }
        public Boolean isDeleted { get; set; }
        public Nullable<System.DateTime> AssetInDate { get; set; }
        public string ManufacturerDetails { get; set; }
        public byte[] Photo { get; set; }
        public double Quantity { get; set; }

        public double TotalQty { get; set; }


        public double Cost { get; set; }
        public double DepRate { get; set; }
        public Nullable<System.Int32> OfficeLocationId { get; set; }

        public string countryName { get; set; }
        public Nullable<System.Int32> LocationId { get; set; }
        public string CityName { get; set; }
        public string AssetAcRef { get; set; }
        public Nullable<System.DateTime> DisposalDate { get; set; }
        public double DisposalValue { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }

    public class SyaAssetTypeModel
    {
        public int AssetTypeId { get; set; }
        public int ParentAssetTypeId { get; set; }
        public string AssetType { get; set; }
        public string Description { get; set; }

    }

    public class SysAssetAllocationModel
    {
        public int AssignmentId { get; set; }
        public int AssetId { get; set; }

        public string AssetName { get; set; }


        public string AssignedTo { get; set; }
        public int StatusId { get; set; }

        public string Status { get; set; }

        public int AssigntoEmpId { get; set; }
        public string AssignedType { get; set; }
        public DateTime AssignedDate { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
        public string ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int count { get; set; }

        public bool Approve { get; set; }
        public bool Reject { get; set; }
        public List<SysAssetAllocationModel> AssetAllocationList { get; set; }
        public List<SysAssetAllocationModel> ApprovalList { get; set; }
    }


    #endregion Assets
    public class SalaryComponentmodel
    {
        public int ComponentId { get; set; }
        public int EmpId { get; set; }
        public string Name { get; set; }
        public string DRCR { get; set; }
        public string Frequency { get; set; }
        public Boolean Fixed { get; set; }
        public Double VariableRate { get; set; }
        public int ParentComponentId { get; set; }
        public Boolean Taxable { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class IssueModel
    {
        public int IssueId { get; set; }
        public int Count { get; set; }
        public Double EstimatedHours { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DevDescription { get; set; }
        public int IsDevloper { get; set; }
        public string Attachment { get; set; }
        public string IsAdmin { get; set; }
        public string Status { get; set; }
        public string StatusShortCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int TabIndex { get; set; }
        public string Comments { get; set; }
        public string to_MailList { get; set; }
        public int Statuscode { get; set; }
        public string TicketType { get; set; }
        public string isAllow { get; set; }
        public List<CommentDatamodel> LstCommentList { get; set; }
        public List<IssueModel> TotalIssues { get; set; }
        public List<IssueModel> MyIssues { get; set; }
    }
    public class CommentDatamodel
    {
        public int IssueId { get; set; }
        public string Attachment { get; set; }
        public string Comments { get; set; }
        public DateTime CommentDate { get; set; }
        public string CommentedBy { get; set; }
        public string Status { get; set; }
    }
    #region RoleFeedMappingModel

    public class RoleFeedMapingModel
    {
        public class RoleFeedMapping
        {
            public string RoleID { get; set; }
            public string FeedID { get; set; }
        }
        public class DerivedFeed
        {
            public string FeedID { get; set; }

            [DisplayName("Feed Name")]
            public string FeedName { get; set; }
        }

        public SystemRoles RoleInfo { get; set; }

        public List<DerivedFeed> lstFeed { get; set; }
        public List<DerivedFeed> lstAssignedFeed { get; set; }
        public List<RoleFeedMapping> lstRoleFeedMapList { get; set; }

    }


    #endregion RoleFeedMappingModel

    #region Address Work
    public class ContyStateCityModel
    {
        public int AddressId { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Pin { get; set; }
        public string LandMark { get; set; }
        public string Country { get; set; }
        public int Countryid { get; set; }
        public int Stateid { get; set; }
        public int RegionId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }

        public int StateId { get; set; }
        public string StateName { get; set; }
        public string Region { get; set; }
        public string Zone { get; set; }
        public string Area { get; set; }
        public List<AddRegionModel> LstRegion { get; set; }
        public List<AddZoneModel> LstZone { get; set; }
        public List<AddAreaModel> LstArea { get; set; }
        public List<ContyStateCityModel> TotalLIst { get; set; }
        public List<ContyStateCityModel> AllocatedList { get; set; }
        public List<AddCountryModel> LstCountry { get; set; }
        public List<AddStateModel> LstState { get; set; }
        public List<AddStateModel> assigned { get; set; }
        public List<AddCityModel> LstCity { get; set; }
    }
    public class AreaCityMappingdetail
    {
        public class AreaCityDetail
        {
            public int AreaId { get; set; }
            public int CityId { get; set; }
            public string City { get; set; }

        }
        public class DerivedCityArea
        {
            public int CityId { get; set; }
            public string City { get; set; }

        }

        //    public SystemRoles RoleInfo { get; set; }

        public int Area { get; set; }
        public List<DerivedCityArea> TotalCityArea { get; set; }
        public List<DerivedCityArea> AssignedCityArea { get; set; }
        public List<AreaCityDetail> LstAreaCitymapping { get; set; }

    }

    public class RegionStateMapping
    {
        public class RegionstateMapdetails
        {
            public int RegionId { get; set; }
            public int StateId { get; set; }
            public string StateName { get; set; }

        }
        public class DerivedRegion
        {
            public int StateId { get; set; }
            public string RegionId { get; set; }

        }

        //    public SystemRoles RoleInfo { get; set; }

        public int RegionId { get; set; }
        public List<DerivedRegion> TotalStateList { get; set; }
        public List<DerivedRegion> AssignedStateList { get; set; }
        public List<RegionstateMapdetails> LstRegionAreamapping { get; set; }

    }
    public class AddRegionModel
    {
        public int AreaId { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public string StateId { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Checked { get; set; }
        public string CreatedBy { get; set; }
        public List<AddRegionModel> TotalList { get; set; }
        public List<AddRegionModel> AllocatedList { get; set; }
        public List<AddAreaModel> AreaList { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class AddZoneModel
    {
        public int ZoneId { get; set; }
        public int StateId { get; set; }
        public string ZoneName { get; set; }
        public string RegionName { get; set; }
        public string Checked { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class AddAreaModel
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public string ZoneName { get; set; }
        public string Checked { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class AddCountryModel
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string Checked { get; set; }
    }
    public class AddStateModel
    {
        public int StateId { get; set; }
        public int Region { get; set; }
        public string StateName { get; set; }
        public string Checked { get; set; }
        public string CountryName { get; set; }
    }
    public class AddCityModel
    {
        public int CityId { get; set; }
        public string Checked { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
    }

    #endregion Address Work
    public class CompanyLibraryModel
    {
        public int DocId { get; set; }
        public string CompCode { get; set; }
        public string Email { get; set; }
        public string DocName { get; set; }
        public string Description { get; set; }
        public Boolean distributeMail { get; set; }
        public string Attachment { get; set; }
        public string FileName { get; set; }
        public int TabIndex { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CompanyName { get; set; }

    }
    public class EmailConfig
    {
        public int EmpId { get; set; }
        public string EmailServer { get; set; }
        public string SendingMailServer { get; set; }
        public string EmailUserName { get; set; }
        public string EmailPassword { get; set; }
        public string EmailPort { get; set; }
        public string SendingEmailPort { get; set; }
        public bool SSL { get; set; }
    }

    #region approval Task Mangement
    public class PendingTaskListModel
    {
        public string Description { get; set; }
        [DisplayName("Pending Approvel")]
        public int PendingApprovel { get; set; }
        public int SubordinateCount { get; set; }
        public List<PendingTaskListModel> lstApproverTask { get; set; }
        public string HyperLink { get; set; }
        public string SubOrdinateHyperLink { get; set; }
    }
    public class CollectionDetailsModel
    {
        public string Type { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Amount { get; set; }
        public List<CollectionDetailsModel> lstCollection { get; set; }

    }
    public class ProjectTaskOverdueModel
    {
        [DisplayName("Stages")]
        public string stages { get; set; }
        public int OnTrack { get; set; }
        public int Completed { get; set; }
        public int Delayed { get; set; }
        public string HyperLink { get; set; }
        public List<ProjectTaskOverdueModel> lstProjectTaskOverdue { get; set; }

    }
    public class RiskCaseListAllDetail
    {
        public int CreatedByMeCount { get; set; }
        public int TotalPending { get; set; }
        public int TotalCreated { get; set; }
        public int InProgress { get; set; }
        public int Closed { get; set; }
        public int OnHold { get; set; }
        public int Complete { get; set; }
        public int REVIEWED { get; set; }
        public int New { get; set; }
        public int REJECTED { get; set; }
        public int APPROVED { get; set; }
        public int TotalReviewd { get; set; }
        public int TotalApproved { get; set; }

        public int ReviewdByMe { get; set; }
        public int ApprovedByMe { get; set; }
        public int TaskType { get; set; }
        public int PendingReview { get; set; }
        public int PendingApproval { get; set; }
        public List<RiskCaseListPercent> RiskPercentList { get; set; }
        public List<RiskCaseListPercent> CasePercentList { get; set; }
        public List<RiskCaseListPercent> PendingRiskPercent { get; set; }
        public List<RiskCaseListPercent> PendingCasePercent { get; set; }

    }
    public class RiskCaseListPercent
    {
        public int ReviewdByMePercent { get; set; }
        public int ReviewdByMeCount { get; set; }
        public int ApprovedByMeePercent { get; set; }
        public int ApprovedByMeeCount { get; set; }
        public int InprogressPercent { get; set; }
        public int InprogressCount { get; set; }
        public int ClosedPercent { get; set; }
        public int ClosedCount { get; set; }
        public int OnholdPercent { get; set; }
        public int OnholdCount { get; set; }
        public int CompletePercent { get; set; }
        public int Completecount { get; set; }
        public int ReviewedPercent { get; set; }
        public int ReviewedPerCount { get; set; }
        public int NewPercent { get; set; }
        public int NewPerCount { get; set; }
        public int RejectedPercent { get; set; }
        public int RejectedCount { get; set; }
        public int ApprovedPercent { get; set; }
        public int ApprovedCount { get; set; }
        public int TotalCreatedPercent { get; set; }
        public int PendingInprogressCount { get; set; }
        public int PendingOnholdCount { get; set; }
        public int TotalCreatedPercentCount { get; set; }


    }
    #endregion approval Task Mangement

    #region Segement

    public class ItemSegment
    {
        public int SegmentId { get; set; }
        public int ParentSegmentId { get; set; }
        public string ParentSegmentName { get; set; }
        public string SegmentName { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<ItemSegment> ChildSegList { get; set; }
        public List<EquipmentSegmentmodel> EquipSegList { get; set; }
        public List<EquipmentSegmentmodel> EquipmentgrdList { get; set; }
    }

    public class EquipmentSegmentmodel
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }

        public string SegmentName { get; set; }


    }
    public class EquipmentSegmentgrdmodel
    {
        public string EquipmentName { get; set; }

        public string SegmentName { get; set; }


    }

    #endregion Segment

    #region Itemcategory
    public class ItemCategory
    {
        public int CategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public string ParentCategorytName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<ItemCategory> ChildItemCategoryList { get; set; }
    }
    #endregion ItemCategory

    #region System Mail Config 
    public class SystemEmailConfig
    {
        public int MailTemplateId { get; set; }
        public string MailContent { get; set; }
        public string TemplateName { get; set; }
        public string DocumentType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
    public class DocumentTypeModel
    {
        public string DocumentType { get; set; }
        public string Description { get; set; }
    }
    #endregion System Mail Config

    #region Exchange Rate 
    public class CurrencyModel
    {
        public String CurrencyCode { get; set; }
        public Double ExchangeRate { get; set; }
        public String Source { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public List<CurrencyModel> currencyModelList { get; set; }


    }
    #endregion Exchange Rate

    #region StockLocation
    public class Employee
    {
        public string Text { get; set; }
        public string Role { get; set; }
        public string Country { get; set; }
        public string Image { get; set; }
        public string ImgAttr { get; set; }
    }
    //public class Currencymodel
    //{
    //    public string CurrencyCode { get; set; }
    //    public Double ExchangeRate { get; set; }
    //    public string Source { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //    public DateTime ModifiedDate { get; set; }
    //}
    public class StockLocation
    {
        public int STLocationId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentLocationId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<StockLocation> ChildLocList { get; set; }

    }
    #endregion StockLocation

    #region EmployeeSalaryStructure
    public class EmpsalStructureModel
    {
        public int EmpId { get; set; }
        public int ComponentId { get; set; }
        public double Amount { get; set; }
        public string CreatedBy { get; set; }
        public string ComponentName { get; set; }
        public string EmployeeName { get; set; }
        public string ParentComponentName { get; set; }
        public int ParentComponentId { get; set; }
        public DateTime CreatedDate { get; set; }

    }
    #endregion EmployeeSalaryStructure

    #region Public Holiday
    public class PublicHolidayModel
    {
        public int HolidayId { get; set; }
        public string FinYear { get; set; }
        public DateTime HolidayDate { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public string Title { get; set; }     
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }      
    }
    #endregion Public Holiday 

    #region Weekly Sal Component

    public class WeekLySalaryModel
    {
        public int TimeSheetId { get; set; }
        public string EmployeeName { get; set; }
        public string CompanyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime WorkStartsession { get; set; }
        public DateTime WorkEndsession { get; set; }
        public int WeekNo { get; set; }
        public int TotalSunday { get; set; }
        public Double WeeklyWorkHour { get; set; }
        public Double WeeklyPayment{ get; set; }
        public Double HourRate { get; set; }
        public Double AnnualPay { get; set; }
        public Double WorkingDays { get; set; }
        public int PublicHoliday { get; set; }
        public Double TotalYearDays { get; set; }
        public double CompanyWorkHour { get; set; }
    }

    #endregion Weekly Sal Component
}
