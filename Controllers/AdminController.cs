using Microsoft.AspNet.Identity;
using SmartSys.BL;
using SmartSys.BL.Bank;
using SmartSys.BL.DW;
using SmartSys.BL.ProViews;
using SmartSys.BL.SysDBCon;
using SmartSys.Utility;
using Syncfusion.EJ.Export;
using Syncfusion.JavaScript.Models;
using Syncfusion.XlsIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using static SmartSys.BL.EmployeeCustomerDetail;

namespace SmartSys.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult GetMainMenu()
        {

            List<SmartSys.BL.SysTaskModel> lstTaskmodel = new List<SysTaskModel>();
            string UserIdr = User.Identity.GetUserName();
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            // int day = (int)DateTime.Now.DayOfWeek;
            ViewBag.WeekNO = weekNum;
            string Date = DateTime.Now.ToString("dd-MM-yyy");
            ViewBag.DateTime = Date;
            if (Request.IsAuthenticated)
            {
                string UserId = User.Identity.GetUserId();
                AdminBL objBl = new AdminBL();
                string LoginAs = objBl.GetEmployeeName(UserId);
                ViewBag.LoginAs = LoginAs;
                // WebMatrix.WebData.WebSecurity.CurrentUserId;

                if (Session["UserMenu"] == null)
                {
                    lstTaskmodel = objBl.GetTaskMenuList(UserId);

                    Session["UserMenu"] = lstTaskmodel;
                }
                else
                    lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];

                var userID = User.Identity.Name;
                var claimsIdentity = User.Identity as ClaimsIdentity;

                if (claimsIdentity != null)
                {
                    // the principal identity is a claims identity.
                    // now we need to find the NameIdentifier claim
                    var userIdClaim = claimsIdentity.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                    if (userIdClaim != null)
                    {
                        var userIdValue = userIdClaim.Value;
                    }
                }
            }
            else
            {
                Session["UserMenu"] = null;
            }
            return PartialView(lstTaskmodel);
        }
        public ActionResult GetChildMenu(List<SmartSys.BL.SysTaskModel> Chmenu)
        {
            return PartialView(Chmenu);
        }
        public ActionResult GetSideBar()
        {
            return PartialView();

        }
        public ActionResult NoAccessToPage()
        {
            return View();
        }

        #region [User]
        [Authorize]
        public ActionResult UserList()

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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/UserList");
            if (FindFlag)
            {
                Session["SysDashBoardModel"] = null;
                SmartSys.BL.AdminBL smartsysBlObj = new SmartSys.BL.AdminBL();
                List<SystemUser> lstSysUserList = smartsysBlObj.GetSysUserList();

                return View(lstSysUserList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditUser(string User_ID, string UserName, string DispName)
        {
            string UserId = User.Identity.GetUserId();
            SmartSys.BL.AdminBL adminblObj = new AdminBL();
            List<SystemRoles> lstRoles = adminblObj.GetUnAssignedRoleList(UserId);
            List<SystemRoles> lstAssignedRoles = adminblObj.GetUserAssignList(User_ID);

            foreach (SystemRoles rrv in lstAssignedRoles)
            {
                string roleid = rrv.RoleID;
                var actionToDelete = from actionRepDel in lstRoles
                                     where actionRepDel.RoleID == roleid
                                     select actionRepDel;
                if (actionToDelete.Count() > 0)
                    lstRoles.Remove(actionToDelete.ElementAt(0));
            }


            SmartSys.BL.SysUserRoleModel model = new SysUserRoleModel();
            model.UserDetails = new SystemUser();
            model.ListRoles = lstRoles;
            model.UserDetails.UserID = Convert.ToInt32(User_ID);
            model.UserDetails.UserName = UserName;
            model.UserDetails.DisplayName = DispName;

            ViewBag.Roles = new SelectList(lstRoles, "RoleId", "RoleName");
            ViewBag.AssignedRoles = new SelectList(lstAssignedRoles, "RoleId", "RoleName");
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditUser(FormCollection fc)
        {
            SmartSys.BL.AdminBL adminblObj = new AdminBL();
            List<string> lstSelectedRoles = new List<string>();
            List<SystemRoles> lstAssignedRoles = new List<SystemRoles>();
            if (fc.AllKeys.Contains("AssignedRoles"))
            {
                lstSelectedRoles = fc["AssignedRoles"].ToString().Split(',').ToList();

            }
            else
            {
                int UserId = Convert.ToInt32(fc["UserDetails.UserID"].ToString());
                adminblObj.DeleteUserRole(UserId);
                return RedirectToAction("UserList", "Admin");

            }



            SystemUser SysUser = new SystemUser();
            SysUser.UserID = Convert.ToInt32(fc["UserDetails.UserID"].ToString());
            SysUser.UserName = fc["UserDetails.UserName"].ToString();
            SysUser.DisplayName = fc["UserDetails.DisplayName"].ToString();
            SysUser.PasswordHint = fc["UserDetails.PasswordHint"].ToString();
            adminblObj.SaveUserRoles(SysUser, lstSelectedRoles);
            return RedirectToAction("UserList", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(SysUserRoleModel sysuserrolemodel)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    //WebSecurity.CreateUserAndAccount(sysuserrolemodel.UserDetails.UserName, sysuserrolemodel.UserDetails.Password);
                    //sysuserrolemodel.UserDetails.UserID = WebSecurity.GetUserId(sysuserrolemodel.UserDetails.UserName);
                    SmartSys.BL.AdminBL adminblObj = new AdminBL();
                    string strResult = adminblObj.SaveSystemUserRoleDetails(sysuserrolemodel);
                }
                catch (MembershipCreateUserException ex)
                {
                    ModelState.AddModelError("", ex.StatusCode.ToString());

                }
            }

            return RedirectToAction("UserList", "Admin");
        }
        #endregion [User]

        #region [Department]
        [Authorize]
        public ActionResult GetDepartmentListold()
        {
            List<Departmentmodel> lst = new List<Departmentmodel>();
            AdminBL objbl = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            DataSet ds = objbl.GetDepartmentlist(User_Id);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Departmentmodel DModel = new Departmentmodel();
                        DModel.DeptId = Convert.ToInt16(dr["DeptId"].ToString());
                        DModel.DeptName = dr["DeptName"].ToString();
                        DModel.DeptHead = Convert.ToInt32(dr["DeptHead"].ToString());
                        DModel.CreatedBy = 1;
                        DModel.CreatedByName = dr["CreatedByName"].ToString();
                        DModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        DModel.ModifiedBy = 1;
                        DModel.ModifiedByName = dr["ModifiedByName"].ToString();
                        DModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        DModel.DeptHeadName = dr["DeptHeadName"].ToString();
                        lst.Add(DModel);

                    }

                }


            }

            return View(lst);
        }

        [Authorize]
        public ActionResult GetDepartmentList()
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/GetDepartmentList");
            if (FindFlag)
            {
                List<Departmentmodel> lst = new List<Departmentmodel>();
                AdminBL objbl = new AdminBL();
                DataSet ds = objbl.GetDepartmentlist(UserId);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            Departmentmodel DModel = new Departmentmodel();
                            DModel.DeptId = Convert.ToInt16(dr["DeptId"].ToString());
                            DModel.DeptName = dr["DeptName"].ToString();
                            DModel.DeptHead = Convert.ToInt32(dr["DeptHead"].ToString());
                            DModel.CreatedBy = 1;
                            DModel.CreatedByName = dr["CreatedByName"].ToString();
                            DModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            DModel.ModifiedBy = 1;
                            DModel.ModifiedByName = dr["ModifiedByName"].ToString();
                            DModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            DModel.DeptHeadName = dr["DeptHeadName"].ToString();
                            lst.Add(DModel);
                        }
                    }
                }
                return View(lst);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportDepartmentLst")]
        [AcceptVerbs("POST")]
        public void ExportDepartmentLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            List<Departmentmodel> lst = new List<Departmentmodel>();
            AdminBL objbl = new AdminBL();
            DataSet ds = objbl.GetDepartmentlist(User_Id);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Departmentmodel DModel = new Departmentmodel();
                        DModel.DeptId = Convert.ToInt16(dr["DeptId"].ToString());
                        DModel.DeptName = dr["DeptName"].ToString();
                        DModel.DeptHead = Convert.ToInt32(dr["DeptHead"].ToString());
                        DModel.CreatedBy = 1;
                        DModel.CreatedByName = dr["CreatedByName"].ToString();
                        DModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        DModel.ModifiedBy = 1;
                        DModel.ModifiedByName = dr["ModifiedByName"].ToString();
                        DModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        DModel.DeptHeadName = dr["DeptHeadName"].ToString();
                        lst.Add(DModel);
                    }
                }
            }
            var DataSource = lst;
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        [Authorize]
        public ActionResult Create()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/Create");
            if (FindFlag)
            {
                List<SysEmploy> drp = new List<SysEmploy>();
                AdminBL objbl = new AdminBL();
                DataSet ds = objbl.GetEmpList();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SysEmploy objmodel = new SysEmploy();
                            objmodel.EmpId = Convert.ToInt16(dr["EmpId"]);
                            objmodel.EmpName = dr["EmpName"].ToString();
                            drp.Add(objmodel);
                        }
                        ViewBag.DepartmentHead = new SelectList(drp, "EmpId", "EmpName");
                    }
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(FormCollection formcollection)
        {
            DataSet ds = new DataSet();
            AdminBL objbl = new AdminBL();
            ds = objbl.GetSelectedDepartmentList(0);
            string User_Id = User.Identity.GetUserId();
            int errorcode = 0;
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["DeptId"] = 0;
                    dr["DeptName"] = formcollection["DeptName"].ToString();
                    dr["DeptHeadId"] = formcollection["DeptHead"].ToString();

                    ds.Tables[0].Rows.Add(dr);
                    errorcode = objbl.SaveDepartment(ds, User_Id);
                    if (errorcode == 500001 || errorcode == 500002)
                    {
                        return RedirectToAction("GetDepartmentList");
                    }
                }
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult EditDepartment(Int16 DeptId)
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/GetDepartmentList");
            if (FindFlag)
            {
                List<SysEmploy> lst = new List<SysEmploy>();
                AdminBL objbl = new AdminBL();
                DataSet ds = objbl.GetEmpList();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SysEmploy objmodel = new SysEmploy();
                            objmodel.EmpId = Convert.ToInt16(dr["EmpId"]);
                            objmodel.EmpName = dr["EmpName"].ToString();
                            lst.Add(objmodel);
                        }
                        ViewBag.DepartmentHead = new SelectList(lst, "EmpId", "EmpName");
                    }
                }
                {
                    DataSet dataset = new DataSet();
                    AdminBL objBL = new AdminBL();
                    dataset = objBL.GetSelectedDepartmentList(DeptId);
                    Departmentmodel obj = new Departmentmodel();
                    Session["department"] = dataset;
                    if (dataset != null)
                    {
                        if (dataset.Tables.Count > 0)
                        {
                            if (dataset.Tables[0].Rows.Count > 0)
                            {
                                obj.DeptId = DeptId;
                                obj.DeptName = dataset.Tables[0].Rows[0]["DeptName"].ToString();
                                obj.DeptHead = Convert.ToInt32(dataset.Tables[0].Rows[0]["DeptHeadId"].ToString());
                                obj.CreatedByName = dataset.Tables[0].Rows[0]["CreatedBy"].ToString();
                                obj.ModifiedByName = dataset.Tables[0].Rows[0]["ModifiedBy"].ToString();
                                obj.CreatedDate = Convert.ToDateTime(dataset.Tables[0].Rows[0]["CreatedDate"].ToString());
                                obj.DeptHeadName = dataset.Tables[0].Rows[0]["DeptHeadName"].ToString();
                                obj.ModifiedDate = Convert.ToDateTime(dataset.Tables[0].Rows[0]["ModifiedDate"].ToString());
                                ViewBag.DeptHeadId = obj.DeptHead;
                            }
                        }
                    }
                    string User_Id = User.Identity.GetUserId();
                    SmartSys.BL.SysRolesTaskModel sysRolesTaskModel = new SysRolesTaskModel();
                    AdminBL adminBlObj = new AdminBL();
                    sysRolesTaskModel = adminBlObj.GetRoleTasks("0", DeptId, User_Id);
                    // sysRolesTaskModel = adminBlObj.GetRoleTasks("0", DeptId);
                    // ViewBag.datasource = sysRolesTaskModel.lstsysRoleTasks;
                    List<int> idslst = new List<int>();
                    ViewBag.ids = idslst;
                    foreach (var id in sysRolesTaskModel.AssignedTaskList)
                    {
                        idslst.Add(id.TaskID);
                    }
                    ViewBag.datasource = sysRolesTaskModel.lstsysRoleTasks;
                    ViewBag.AssignedTask = new SelectList(sysRolesTaskModel.AssignedTaskList, "TaskID", "TaskName");
                    return View(obj);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditDepartment(FormCollection formcollection, Int16 DeptId)
        {
            DataSet ds;
            AdminBL objBL = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            int errorcode = 0;
            if (Session["department"] != null)
            {
                ds = (DataSet)Session["department"];

                ds.Tables[0].Rows[0]["DeptName"] = formcollection["DeptName"].ToString();
                ds.Tables[0].Rows[0]["DeptHeadId"] = formcollection["DeptHeadId"].ToString();
                ds.Tables[0].Rows[0]["ModifiedBy"] = 1;
                errorcode = objBL.SaveDepartment(ds, User_Id);
                if (errorcode == 500001 || errorcode == 500002)
                {
                    return RedirectToAction("GetDepartmentList");
                }
            }
            return View();
        }
        public ActionResult SaveDepartmentTasks(string str, int DeptId)
        {
            bool key = true;
            List<SmartSys.BL.SysRolesTaskModel> data = new List<SysRolesTaskModel>();
            SmartSys.BL.SysRolesTaskModel sysRolesTaskModel = new SysRolesTaskModel();
            string User_Id = User.Identity.GetUserId();

            JavaScriptSerializer js = new JavaScriptSerializer();
            data = js.Deserialize<List<SmartSys.BL.SysRolesTaskModel>>(str);
            sysRolesTaskModel.lstSysRolesTasks = new List<SysRoleTasks>();
            if (key)
            {
                sysRolesTaskModel.lstSysRolesTasks = new List<SysRoleTasks>();
                foreach (SmartSys.BL.SysRolesTaskModel task in data)
                {
                    SysRoleTasks sysRoleTasks = new SysRoleTasks();
                    sysRoleTasks.DeptId = DeptId;
                    sysRoleTasks.TaskID = Convert.ToInt32(task.id);
                    sysRolesTaskModel.lstSysRolesTasks.Add(sysRoleTasks);
                }
                AdminBL adminblObj = new AdminBL();
                int strResult = adminblObj.SaveDepartmentTask(sysRolesTaskModel, User_Id, DeptId);
            }

            return RedirectToAction("SytemRoleList");
        }
        #endregion [Department]

        #region[Company]

        [Authorize]
        [HttpGet]
        public ActionResult SystemCompanyList()
        {
            List<SmartSys.BL.SysCompanyModel> lstCompany = new List<SmartSys.BL.SysCompanyModel>();
            AdminBL objBl = new AdminBL();
            lstCompany = objBl.GetSysCompanyList();

            foreach (SmartSys.BL.SysCompanyModel sysCompanyModel in lstCompany)
            {
                MemoryStream ms = new MemoryStream(sysCompanyModel.Logo, 0, sysCompanyModel.Logo.Length);
                ms.Write(sysCompanyModel.Logo, 0, sysCompanyModel.Logo.Length);
                Image image = Image.FromStream(ms);

                Bitmap bitMap = new Bitmap(image);

                image.Dispose();

                string strFileName = sysCompanyModel.Name.Split(' ')[0] + ".png";
                var path = Path.Combine(Server.MapPath("~/Images/DataBaseImges"), strFileName);

                sysCompanyModel.ImagePath = "~/Images/DataBaseImges/" + strFileName;
                bitMap.Save(path);
                bitMap.Dispose();
            }
            return View(lstCompany);
        }
        [Authorize]
        [HttpGet]
        public ActionResult CreateCompany()
        {
            SmartSys.BL.SysCompanyModel sysCompanyModel = null;
            if (Session["SysCompanyModel"] != null)
            {
                sysCompanyModel = Session["SysCompanyModel"] as SmartSys.BL.SysCompanyModel;

            }
            else
            {
                sysCompanyModel = new SysCompanyModel();
            }


            Session["SysCompanyModel"] = sysCompanyModel;
            return View(sysCompanyModel);
        }

        public string GetCompanyLogo()
        {
            string strLogoPath = "";
            if (Session["strLogoPath"] == null)
            {

                AdminBL adminObjBL = new AdminBL();
                ArrayList arrList = adminObjBL.GetComanyLogo(1);



                MemoryStream ms = new MemoryStream(arrList[1] as byte[], 0, (arrList[1] as byte[]).Length);
                ms.Write(arrList[1] as byte[], 0, (arrList[1] as byte[]).Length);
                Image image = Image.FromStream(ms);

                Bitmap bitMap = new Bitmap(image);

                image.Dispose();

                string strFileName = arrList[0].ToString().Split(' ')[0] + ".png";
                var path = Path.Combine(Server.MapPath("~/Images/DataBaseImges"), strFileName);

                strLogoPath = "~/Images/DataBaseImges/" + strFileName;
                bitMap.Save(path);
                bitMap.Dispose();
                Session["strLogoPath"] = strLogoPath;
            }
            else
                strLogoPath = Session["strLogoPath"].ToString();

            return strLogoPath;

        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateCompany(FormCollection frmCollection)
        {
            SmartSys.BL.SysCompanyModel sysCompanyModel = null;
            if (Session["SysCompanyModel"] != null)
            {
                sysCompanyModel = Session["SysCompanyModel"] as SmartSys.BL.SysCompanyModel;

            }
            sysCompanyModel.Name = frmCollection["Name"].ToString();
            sysCompanyModel.ShortName = frmCollection["ShortName"].ToString();
            sysCompanyModel.Pin = frmCollection["Pin"].ToString();
            sysCompanyModel.ServiceTN = frmCollection["ServiceTN"].ToString();
            sysCompanyModel.State = frmCollection["State"].ToString();
            sysCompanyModel.TagLine = frmCollection["TagLine"].ToString();
            sysCompanyModel.AddressLine1 = frmCollection["AddressLine1"].ToString();
            sysCompanyModel.AddressLine2 = frmCollection["AddressLine2"].ToString();
            sysCompanyModel.BST = frmCollection["BST"].ToString();
            sysCompanyModel.City = frmCollection["City"].ToString();
            sysCompanyModel.Country = frmCollection["Country"].ToString();
            sysCompanyModel.CST = frmCollection["CST"].ToString();

            AdminBL adminObj = new AdminBL();
            string strResult = adminObj.SaveSysCompanyDetails(sysCompanyModel);

            Session.Remove("SysCompanyModel");
            return RedirectToAction("SystemCompanyList", "Admin");
        }



        [HttpGet]
        public ActionResult UploadLogo()
        {

            return PartialView();
        }

        [HttpPost]
        public ActionResult UploadLoggo(HttpPostedFileBase file)
        {
            if (file == null)
                RedirectToAction("CreateCompany", "Admin");
            else
            {
                SmartSys.BL.SysCompanyModel sysCompanyModel = null;
                if (Session["SysCompanyModel"] != null)
                {
                    sysCompanyModel = Session["SysCompanyModel"] as SmartSys.BL.SysCompanyModel;

                }

                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                file.SaveAs(filePath);

                FileStream objFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                int iLength = Convert.ToInt32(objFileStream.Length);
                byte[] objData = new byte[iLength];
                objFileStream.Read(objData, 0, iLength);
                objFileStream.Close();

                //deletelocal file
                System.IO.File.Delete(filePath);

                sysCompanyModel.Logo = objData;
                Session["SysCompanyModel"] = sysCompanyModel;
            }

            return RedirectToAction("CreateCompany", "Admin");
        }

        #endregion[Company]

        #region SytemRoles and Tasks
        [Authorize]
        public ActionResult SytemRoleList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/SytemRoleList");
            if (FindFlag)
            {
                if (Session["RoleRptMappingModel"] != null)
                {
                    Session["RoleRptMappingModel"] = null;
                }
                AdminBL adminBlOBJ = new AdminBL();
                List<SystemRoles> lstRoles = adminBlOBJ.GetRoleList(User.Identity.GetUserId());
                return View(lstRoles);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        public ActionResult CreateEditRole(string iRoleID)
        {
            AdminBL adminBlObj = new AdminBL();
            List<SmartSys.BL.SysRoleTaskModel> lstsysRoleTasks = adminBlObj.GetRoleTask(iRoleID);
            ViewBag.datasource = lstsysRoleTasks;
            return View(lstsysRoleTasks);
        }

        public ActionResult SaveRoleTask(string str)
        {
            List<SmartSys.BL.SysRoleTaskModel> data = new List<SysRoleTaskModel>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            data = js.Deserialize<List<SmartSys.BL.SysRoleTaskModel>>(str);
            foreach (SmartSys.BL.SysRoleTaskModel task in data)
            {

            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //----------------------------------------------------------
        [Authorize]
        public ActionResult ManageRole(string iRoleID, string rolename)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/ManageRole?iRoleID=0");
            if (FindFlag)
            {
                SmartSys.BL.SysRolesTaskModel sysRolesTaskModel = new SysRolesTaskModel();
                AdminBL adminBlObj = new AdminBL();
                sysRolesTaskModel = adminBlObj.GetRoleTasks(iRoleID, 0, "");

                string roleid = sysRolesTaskModel.Role.RoleID;
                string Rolename = sysRolesTaskModel.Role.RoleName;


                if (iRoleID == null)
                {

                    ViewBag.roleid = roleid;
                    ViewBag.Rolename = Rolename;
                    foreach (SysRoleTasks rrv in sysRolesTaskModel.AssignedTaskList)
                    {
                        Int32 TskId = rrv.TaskID;
                        var actionToDelete = from actionRepDel in sysRolesTaskModel.lstSysRolesTasks
                                             where actionRepDel.TaskID == TskId
                                             select actionRepDel;
                        sysRolesTaskModel.lstSysRolesTasks.Remove(actionToDelete.ElementAt(0));
                    }
                }
                else
                {
                    sysRolesTaskModel = adminBlObj.GetRoleTasks(iRoleID, 0, "");

                    ViewBag.roleid = roleid;
                    ViewBag.Rolename = Rolename;
                }
                ViewBag.datasource = sysRolesTaskModel.lstsysRoleTasks;
                List<int> idslst = new List<int>();
                ViewBag.ids = idslst;
                foreach (var id in sysRolesTaskModel.AssignedTaskList)
                {
                    idslst.Add(id.TaskID);
                }
                ViewBag.datasource = sysRolesTaskModel.lstsysRoleTasks;
                ViewBag.AssignedTask = new SelectList(sysRolesTaskModel.AssignedTaskList, "TaskID", "TaskName");

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        //----------------------------------------------------------
        [Authorize]
        public ActionResult CreateRole(string iRoleID)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/SytemRoleList");
            if (FindFlag)
            {
                SmartSys.BL.SysRolesTaskModel sysRolesTaskModel = new SysRolesTaskModel();
                AdminBL adminBlObj = new AdminBL();
                sysRolesTaskModel = adminBlObj.GetRoleTasks(iRoleID, 0, "");

                foreach (SysRoleTasks rrv in sysRolesTaskModel.AssignedTaskList)
                {
                    Int32 TskId = rrv.TaskID;
                    var actionToDelete = from actionRepDel in sysRolesTaskModel.lstSysRolesTasks
                                         where actionRepDel.TaskID == TskId
                                         select actionRepDel;
                    sysRolesTaskModel.lstSysRolesTasks.Remove(actionToDelete.ElementAt(0));
                }

                ViewBag.Tasks = new SelectList(sysRolesTaskModel.lstSysRolesTasks, "TaskID", "TaskName");
                ViewBag.AssignedTask = new SelectList(sysRolesTaskModel.AssignedTaskList, "TaskID", "TaskName");

                return View(sysRolesTaskModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult SaveRoleTasks(string str, string RoleName, string RoleId)
        {
            bool key = true;
            List<SmartSys.BL.SysRolesTaskModel> data = new List<SysRolesTaskModel>();
            FormCollection frmCollection = new FormCollection();
            SmartSys.BL.SysRolesTaskModel sysRolesTaskModel = new SysRolesTaskModel();
            string User_Id = User.Identity.GetUserId();
            sysRolesTaskModel.Role = new SystemRoles();
            sysRolesTaskModel.Role.RoleID = RoleId;
            sysRolesTaskModel.Role.RoleName = RoleName;
            JavaScriptSerializer js = new JavaScriptSerializer();
            data = js.Deserialize<List<SmartSys.BL.SysRolesTaskModel>>(str);
            sysRolesTaskModel.lstSysRolesTasks = new List<SysRoleTasks>();

            //foreach (var keys in frmCollection.AllKeys)
            //{
            //    if (keys.ToString() == "AssignedTasksToRole")
            //    {
            //        key = true;
            //    }
            //}
            if (key)
            {
                sysRolesTaskModel.lstSysRolesTasks = new List<SysRoleTasks>();
                //List<string> lstSelectedTaks = frmCollection["AssignedTasksToRole"].ToString().Split(',').ToList();
                foreach (SmartSys.BL.SysRolesTaskModel task in data)
                {
                    SysRoleTasks sysRoleTasks = new SysRoleTasks();
                    sysRoleTasks.RoleID = sysRolesTaskModel.Role.RoleID;
                    sysRoleTasks.TaskID = Convert.ToInt32(task.id);
                    sysRoleTasks.CreatedBy = 1;// WebSecurity.CurrentUserId;
                    sysRoleTasks.ModifiedBy = 1;// WebSecurity.CurrentUserId;
                    sysRolesTaskModel.lstSysRolesTasks.Add(sysRoleTasks);
                }
                AdminBL adminblObj = new AdminBL();
                string strResult = adminblObj.SaveRoleTasks(sysRolesTaskModel, User_Id);
            }
            else if (sysRolesTaskModel.Role.RoleID == "0")
            {
                SysRoleTasks sysRoleTasks = new SysRoleTasks();
                sysRoleTasks.RoleID = sysRolesTaskModel.Role.RoleID;
                sysRoleTasks.CreatedBy = 1;// WebSecurity.CurrentUserId;
                sysRoleTasks.ModifiedBy = 1;// WebSecurity.CurrentUserId;
                AdminBL adminblObj = new AdminBL();
                string strResult = adminblObj.SaveRoleTasks(sysRolesTaskModel, User_Id);
                return RedirectToAction("SytemRoleList");
            }
            else
            {
                AdminBL DelBL = new AdminBL();

                string RoleId1 = sysRolesTaskModel.Role.RoleID;
                DelBL.DeleteRoletask(RoleId1);
            }


            return RedirectToAction("SytemRoleList");
        }
        #endregion

        #region Emplyee work
        [Authorize]
        public ActionResult GetEmployeeList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/GetEmployeeList");
            if (FindFlag)
            {
                List<EmployeeModel> lstEmp = new List<EmployeeModel>();
                AdminBL objBL = new AdminBL();
                lstEmp = objBL.GetEmployeeList(UserId);
                if (Session["EmplloyeeModel"] != null)
                {
                    Session["EmplloyeeModel"] = null;
                }
                if (Session["EmpMapping"] != null)
                {
                    Session["EmpMapping"] = null;
                }
                return View(lstEmp);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [System.Web.Http.ActionName("ExportToExcelEmployee")]
        [AcceptVerbs("POST")]
        public void ExportToExcelEmployee(string GridModel)
        {
            AdminBL objBL = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = objBL.GetEmployeeList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        [Authorize]
        [HttpGet]
        public ActionResult EditEmployee(int EmpId)
        {
            EmployeeModel EmpModel = new EmployeeModel();
            EmpModel.emailConfig = new EmailConfig();
            EmpModel.DeptList = new List<Departmentmodel>();
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
            bool EmpSalaryComponent;
            bool AllEmpSalaryComponent;
            bool EmpAllEmailConfig;
            bool EmpAssetConfig;
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/GetEmployeeList");
            if (FindFlag)
            {
                AdminBL objbl = new AdminBL();
                List<SysEmploy> DrpDwnEmp = new List<SysEmploy>();
                DrpDwnEmp = objbl.EmployeeDropDown();
                ViewBag.EmpList = new SelectList(DrpDwnEmp, "EmpId", "EmpName");


                string User_Id = User.Identity.GetUserId();

                if (Session["Error"] != null)
                {
                    TempData["Message"] = Session["Error"] as string;
                }
                Session["Error"] = null;
                EmpModel.LstExperties = new List<ExpertiesModel>();

                if (Session["EmplloyeeModel"] == null)
                {
                    AdminBL objBL = new AdminBL();
                    EmpModel = objBL.GetSelectedEmployeeList(EmpId);

                    Session["EmplloyeeModel"] = EmpModel;
                }
                else
                {

                    EmpModel = Session["EmplloyeeModel"] as EmployeeModel;
                    if (EmpModel.EmpId != EmpId)
                    {
                        AdminBL objBL = new AdminBL();
                        EmpModel = objBL.GetSelectedEmployeeList(EmpId);
                        Session["EmplloyeeModel"] = EmpModel;
                    }
                }
                EmpModel.DeptList = objbl.GetEmployeeDept(EmpId);
                List<Departmentmodel> DrpDwnSysDept = new List<Departmentmodel>();
                DrpDwnSysDept = objbl.DeptDropdown(UserId);

                if (EmpModel.DeptList.Count > 0)
                {
                    foreach (Departmentmodel Model in EmpModel.DeptList)
                    {
                        var actionToDelete = from actionRepDel in DrpDwnSysDept.Where(s => (s.DeptId == Model.DeptId))
                                             where (actionRepDel.DeptId == Model.DeptId)
                                             select actionRepDel;
                        if (actionToDelete.Count() > 0)
                            DrpDwnSysDept.Remove(actionToDelete.ElementAt(0));
                    }

                }

                ViewBag.SysDeptList = new SelectList(DrpDwnSysDept, "DeptId", "DeptName");
                ViewBag.SysAssignDeptList = new SelectList(EmpModel.DeptList, "DeptId", "DeptName");

                EmpSalaryComponent = modelObj.FindMenu(lstTaskmodel, "EmpSalaryComponent");
                AllEmpSalaryComponent = modelObj.FindMenu(lstTaskmodel, "AllEmpSalaryComponent");
                if (AllEmpSalaryComponent)
                {
                    EmpModel.Matched = "Matched";
                    EmpModel.EmpsalaryComponent = "Editable";
                }
                else
                {
                    if (EmpSalaryComponent)
                    {
                        int Eid = objbl.GetEmployeeByUserId(User_Id);
                        if (Eid == EmpId)
                        {
                            EmpModel.Matched = "Matched";
                            EmpModel.EmpsalaryComponent = "Readonly";
                        }
                        else
                        {
                            EmpModel.Matched = "Unmatched";
                        }
                    }
                    else
                    {
                        EmpModel.Matched = "Unmatched";
                    }
                }

                EmpAllEmailConfig = modelObj.FindMenu(lstTaskmodel, "AllEmpEmailConfig");
                if (EmpAllEmailConfig)
                {
                    EmpModel.MatchedEmailConfig = "Matched";
                }
                else
                {
                    int Eid = objbl.GetEmployeeByUserId(User_Id);
                    if (Eid == EmpId)
                    {
                        EmpModel.MatchedEmailConfig = "Matched";
                    }
                    else
                    {
                        EmpModel.MatchedEmailConfig = "Unmatched";
                    }
                }

                EmpAssetConfig = modelObj.FindMenu(lstTaskmodel, "AllowEmpAssets");
                if (EmpAssetConfig)
                {
                    EmpModel.AssetsAllow = "Matched";
                }
                else
                {
                    EmpModel.AssetsAllow = "Unmatched";
                }
                EmpModel.LstExperience = new List<ExperienceModel>();
                EmpModel.LstExperties = objbl.GetEmployeeExpertiesList(EmpId);
                EmpModel.LstExperience = objbl.GetEmployeeExprienceList(EmpId);
                EmpModel.EmployeeBankDetailLst = new List<EmployeeBankDetailModel>();
                EmpModel.EmployeeBankDetailLst = objbl.GetBankListByEmployee(EmpId);
                EmpModel.AddressList = new List<EmployeeAddressModel>();
                EmpModel.AddressList = objbl.AddressListByEmployee(EmpId);
                List<EmployeeCustomer> lstcust = objbl.GetEmployeeCustomerListForDropDown(EmpId);
                ViewBag.CustomerList = new SelectList(lstcust, "CustomerId", "CustomerName");
                List<EmployeeVendor> lstvend = objbl.GetEmployeeVendorListForDropDown(0);
                ViewBag.VendorList = new SelectList(lstvend, "VendorId", "VendorName");
                EmpModel.SysEmpAssetsList = new List<SysEmployeeAssetsModel>();
                EmpModel.SysEmpAssetsList = objbl.AssetListByEmployee(EmpId);
                EmpModel.DocList = objbl.GetDocumentsByEmployee(EmpId);
                ItemMappingModel ItemMap = new ItemMappingModel();
                ItemBL BlObj = new ItemBL();
                ItemMap.lstItemMap = BlObj.GetDWCompList();
                ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");
                ContyStateCityModel addressModel = new ContyStateCityModel();
                addressModel = objbl.GetCountryStateCityList();
                List<AddRegionModel> rntrylst = addressModel.LstRegion;
                ViewBag.regiondrppp = new SelectList(rntrylst, "RegionId", "RegionName");
                return View(EmpModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult CheckEmailCongig(string Server,string username,string password,string EmailPort,bool SSL)
        {
            try
            {
                string Sub = "Test Mail Config";
                string body = "Testing Your Email Configuration by Supermatic System";
                int ErrCode = SmartSys.Utility.Common.SendMailDynamic(Sub, body, "", username, "", null, Server, username, password, EmailPort, "", SSL);
                if (ErrCode == 500001)
                    return Json("Success", JsonRequestBehavior.AllowGet);
                else
                    return Json("Error", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return Json("Error", JsonRequestBehavior.AllowGet);
            }            
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditEmployee(FormCollection fc, bool isClient, HttpPostedFileBase uploadFile)
        {
            EmployeeModel EmpModel = new EmployeeModel();
            EmpModel.emailConfig = new EmailConfig();
            string User_Id = User.Identity.GetUserId();
            string checkNull = "";
            AdminBL objBL = new AdminBL();
            int errorCode = 0;
            EmpModel.EmpId = Convert.ToInt32(fc["EmpId"].ToString());
            EmpModel.FirstName = fc["FirstName"].ToString();
            if (fc.AllKeys.Contains("AnnualVariablePay"))
            {
                if (fc["AnnualVariablePay"].ToString() != "")
                    EmpModel.AnnualVariablePay = Convert.ToDouble(fc["AnnualVariablePay"].ToString());
                if (fc["AnnualFixPay"].ToString() != "")
                    EmpModel.AnnualFixPay = Convert.ToDouble(fc["AnnualFixPay"].ToString());
            }
            else
            {
                EmpModel.AnnualVariablePay = 0;
                EmpModel.AnnualFixPay = 0;
            }
            EmpModel.MiddleName = fc["MiddleName"].ToString();
            EmpModel.CompCode = fc["CompCode"].ToString();
            EmpModel.PhoneNumber = fc["PhoneNumber"].ToString();
            EmpModel.Gender = fc["Gender"].ToString();
            EmpModel.Mobile = fc["Mobile"].ToString();
            EmpModel.PAN = fc["PAN"].ToString();
            EmpModel.AADHAR = fc["AADHAR"].ToString();
            EmpModel.LeaveOpBal = Convert.ToInt32(fc["LeaveOpBal"].ToString());
            EmpModel.PassportNumber = fc["PassportNumber"].ToString();
            EmpModel.LastName = fc["LastName"].ToString();
            EmpModel.emailId = fc["emailId"].ToString();
            EmpModel.Designation = fc["Designation"].ToString();
            EmpModel.Qualification = fc["Qualification"].ToString();
            EmpModel.Region = Convert.ToInt32(fc["Region"].ToString());
            if (Convert.ToInt16(fc["UserId"].ToString()) == 0)
            {
                EmpModel.UserId = Convert.ToInt16(0);
            }
            else
            {
                EmpModel.UserId = Convert.ToInt16(fc["UserId"].ToString());
            }

            EmpModel.DeptId = Convert.ToInt16(0);
            EmpModel.ManagerId = Convert.ToInt32(fc["ManagerId"].ToString());
            EmpModel.DateOfJoin = Convert.ToDateTime(fc["DateOfJoin"].ToString());
            EmpModel.ModifiedBy = Convert.ToInt16(1);
            EmpModel.isClient = isClient;

            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                EmpModel.Photo = new byte[uploadFile.ContentLength];
                uploadFile.InputStream.Read(EmpModel.Photo, 0, uploadFile.ContentLength);
                checkNull = "NotNull";
            }
            else
            {
                EmployeeModel demo = new EmployeeModel();
                demo = Session["EmplloyeeModel"] as EmployeeModel;
                EmpModel.Photo = null;
                checkNull = "Null";
            }
            if (fc.AllKeys.Contains("emailConfig.EmailServer"))
            {
                EmpModel.emailConfig.SendingMailServer = fc["emailConfig.SendingMailServer"].ToString();
                EmpModel.emailConfig.EmailServer = fc["emailConfig.EmailServer"].ToString();
                EmpModel.emailConfig.EmailPort = fc["emailConfig.EmailPort"].ToString();
                EmpModel.emailConfig.SendingEmailPort = fc["emailConfig.SendingEmailPort"].ToString();
                EmpModel.emailConfig.EmailUserName = fc["emailConfig.EmailUserName"].ToString();
                EmpModel.emailConfig.EmailPassword = fc["Txt_Pwd"].ToString();
                if (fc.AllKeys.Contains("Txt_SSL"))
                {
                    if (fc["Txt_SSL"].ToString() == "on")
                    {
                        EmpModel.emailConfig.SSL = true;
                    }
                }
                else
                {
                    EmpModel.emailConfig.SSL = false;
                }


                //fc["Txt_SSL"].ToString();

            }
            errorCode = objBL.SaveEmployee(EmpModel, User_Id, isClient, checkNull);
            if (errorCode == 500001 || errorCode == 500002)
            {
                TempData["Msg"] = "Successfully Update ....";
                return RedirectToAction("GetEmployeeList");
            }
            else
            {
                TempData["Msg"] = "There are some Error Please Try Again...";
                return RedirectToAction("GetEmployeeList");
            }
        }

        [HttpPost]
        public int SaveEmployeeCustomer(int EmpId, int CustomerId, int AssignDept)
        {
            int errorCode = 0;
            AdminBL objBL = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            errorCode = objBL.SaveEmployeeCustomer(EmpId, CustomerId, User_Id, AssignDept);
            if (errorCode != 0)
                Session["EmplloyeeModel"] = null;
            return errorCode;
        }
        public ActionResult EmployeeDocuments(int EmpId)
        {
            Documents docs = new Documents();
            docs.EmpId = EmpId;
            return PartialView(docs);
        }
        [HttpPost]
        public ActionResult EmployeeDocuments(FormCollection fc, HttpPostedFileBase file)
        {

            AdminBL ObjBl = new AdminBL();
            Documents docs = new Documents();
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");
            string fileName = "";

            if (file != null)
            {
                fileName = System.IO.Path.GetFileName(file.FileName);
                if (file.ContentLength > 0)
                {
                    try
                    {
                        string[] FileSplit = fileName.Split('.');
                        string FileEx = FileSplit[1].ToString();
                        String guid = Guid.NewGuid().ToString();
                        string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                        string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        string FileName = Convert.ToString("/EmployeeDoc/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
                        Session["FileName"] = FileName;
                        string localPath = Server.MapPath("~/App_Data/uploads");
                        if (!System.IO.Directory.Exists(localPath))
                        {
                            System.IO.Directory.CreateDirectory(localPath);
                        }
                        file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName));
                        if (ftpServer.Trim().Length > 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {

                                FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                                requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                                int bufferLength = 2048;
                                byte[] buffer = new byte[file.InputStream.Length];
                                Stream uploadStream = requestFTPUploader.GetRequestStream();
                                int contentLength = file.InputStream.Read(buffer, 0, Convert.ToInt32(file.InputStream.Length));
                                while (contentLength != 0)
                                {
                                    uploadStream.Write(buffer, 0, contentLength);
                                    contentLength = file.InputStream.Read(buffer, 0, bufferLength);
                                }
                                uploadStream.Close();
                                //fileStream.Close();
                                requestFTPUploader = null;
                                docs.DocumentPath = FileName;
                                break;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.LogException(ex);
                    }
                }
            }
            docs.DocumentName = fc["DocumentName"].ToString();
            docs.EmpId = Convert.ToInt32(fc["EmpId"].ToString());
            int errcode = ObjBl.SaveEmployeeDocuments(docs.DocumentPath, docs.DocumentName, docs.EmpId);
            if (errcode != 0)
            {

                return RedirectToAction("EditEmployee", new { EmpId = errcode });
            }
            else
            {
                if (file.ContentLength > 0)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        try
                        {
                            string DocPath = (string)Session["FileName"];
                            FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(ftpServer + DocPath);
                            requestFileDelete.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                            requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                            FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                            requestFileDelete = null;

                            break;
                        }
                        catch (Exception ex)
                        {
                            if (x > 2)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            return RedirectToAction("GetEmployeeList");
        }
        [HttpPost]
        public int SaveEmployeeVendor(int EmpId, int VendorId)
        {
            int errorCode = 0;
            AdminBL objBL = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            errorCode = objBL.SaveEmployeeVendor(EmpId, VendorId, User_Id);
            if (errorCode != 0)
                Session["EmplloyeeModel"] = null;
            return errorCode;
        }
        public ActionResult DeleteEmployeeCustomer(int EmpId, int CustomerId)
        {
            int errorCode = 0;
            AdminBL objBL = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            errorCode = objBL.DeleteEmployeeCustomer(EmpId, CustomerId, User_Id);
            if (errorCode != 0)
                Session["EmplloyeeModel"] = null;
            return RedirectToAction("EditEmployee", new { EmpId = EmpId });
        }
        public ActionResult DeleteEmployeeVendor(int EmpId, int VendorId)
        {
            int errorCode = 0;
            AdminBL objBL = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            errorCode = objBL.DeleteEmployeeVendor(EmpId, VendorId, User_Id);
            if (errorCode != 0)
                Session["EmplloyeeModel"] = null;
            return RedirectToAction("EditEmployee", new { EmpId = EmpId });
        }
        public ActionResult createExperties(int EmpId, string Experties)
        {
            ExpertiesModel ExprtModel = new ExpertiesModel();
            AdminBL objBL = new AdminBL();
            ExprtModel = objBL.GetSelectedEXperties(EmpId, Experties);
            if (Experties != null)
            {
                ExprtModel = objBL.GetSelectedEXperties(EmpId, Experties);
                ExprtModel.Experties = ExprtModel.NewExperties;
            }
            else
            {
                ExprtModel.EmpId = EmpId;
                ExprtModel.NewExperties = "New Entry";

            }
            
                return PartialView(ExprtModel);
           
        }
        [HttpPost]
        public ActionResult createExperties(FormCollection fc)
        {
            try
            {
                ExpertiesModel ExprtModel = new ExpertiesModel();
                string User_Id = User.Identity.GetUserId();
                AdminBL objBL = new AdminBL();
                int errorCode = 0;
                ExprtModel.EmpId = Convert.ToInt32(fc["EmpId"].ToString());
                ExprtModel.Experties = fc["Experties"].ToString();
                ExprtModel.NewExperties = fc["NewExperties"].ToString();

                if (ExprtModel.NewExperties != "New Entry")
                {
                    string temp = ExprtModel.NewExperties;
                    ExprtModel.NewExperties = ExprtModel.Experties;
                    ExprtModel.Experties = temp;
                }
                ExprtModel.ExpInYears = Convert.ToDouble(fc["ExpInYears"].ToString());
                ExprtModel.Exp_Level = Convert.ToInt32(fc["Exp_Level"].ToString());
                errorCode = objBL.SaveEmployeeExperties(ExprtModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {

                    return RedirectToAction("EditEmployee", new { EmpId = ExprtModel.EmpId });
                }
                else if (errorCode == 600001)
                {
                    Session["Error"] = "This Employee Experties Already Created";
                    return RedirectToAction("EditEmployee", new { EmpId = ExprtModel.EmpId });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        [Authorize]
        public ActionResult SysUesrList(int EmpId)
        {
            List<SystemUser> lstUser = new List<SystemUser>();
            AdminBL objBL = new AdminBL();
            lstUser = objBL.GetUserListNonEmployee(EmpId);
            return PartialView(lstUser);
        }
        [Authorize]
        public ActionResult SysUesrAdd(Int16 UserId, string UserName, string DisplayName, int EmpId)
        {
            EmployeeModel EmpModel = new EmployeeModel();
            if (Session["EmplloyeeModel"] != null)
                EmpModel = Session["EmplloyeeModel"] as EmployeeModel;

            EmpModel.UserId = UserId;
            EmpModel.UserName = UserName;
            EmpModel.DisplayName = DisplayName;
            Session["EmplloyeeModel"] = EmpModel;
            if (EmpId > 0)
            {
                return RedirectToAction("EditEmployee", new { EmpId = EmpModel.EmpId });
            }
            else
            {
                return RedirectToAction("CreateEmployee");
            }
        }
        [Authorize]
        public ActionResult CreateEmployee()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/CreateEmployee");
            if (FindFlag)
            {

                AdminBL objbl = new AdminBL();
                List<SysEmploy> DrpDwnEmp = new List<SysEmploy>();
                DrpDwnEmp = objbl.EmployeeDropDown();
                ViewBag.EmpList = new SelectList(DrpDwnEmp, "EmpId", "EmpName");

                List<Departmentmodel> DrpDwnSysDept = new List<Departmentmodel>();
                DrpDwnSysDept = objbl.DeptDropdown(UserId);
                ViewBag.SysDeptList = new SelectList(DrpDwnSysDept, "DeptId", "DeptName");

                EmployeeModel EmpModel = new EmployeeModel();
                ItemMappingModel ItemMap = new ItemMappingModel();
                ItemBL BlObj = new ItemBL();
                ItemMap.lstItemMap = BlObj.GetDWCompList();
                ViewBag.CompList = new SelectList(ItemMap.lstItemMap, "CompCode", "CompName");

                ContyStateCityModel addressModel = new ContyStateCityModel();
                addressModel = objbl.GetCountryStateCityList();
                List<AddRegionModel> rntrylst = addressModel.LstRegion;
                ViewBag.regiondrppp = new SelectList(rntrylst, "RegionId", "RegionName");
                if (Session["EmplloyeeModel"] == null)
                {
                    EmpModel.UserId = 0;
                    EmpModel.UserName = " ";
                    EmpModel.DisplayName = " ";
                    Session["EmplloyeeModel"] = EmpModel;
                }

                EmpModel = Session["EmplloyeeModel"] as EmployeeModel;

                return View(EmpModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult CreateEmployee(FormCollection fc, bool isClient, HttpPostedFileBase uploadFile)
        {
            EmployeeModel EmpModel = new EmployeeModel();
            EmpModel.emailConfig = new EmailConfig();
            string User_Id = User.Identity.GetUserId();
            AdminBL objBL = new AdminBL();
            int errorCode = 0;

            EmpModel.FirstName = fc["FirstName"].ToString();
            EmpModel.Gender = fc["Gender"].ToString();
            EmpModel.CompCode = fc["CompCode"].ToString();
            EmpModel.MiddleName = fc["MiddleName"].ToString();
            EmpModel.LastName = fc["LastName"].ToString();
            EmpModel.PhoneNumber = fc["PhoneNumber"].ToString();
            EmpModel.Mobile = fc["Mobile"].ToString();
            EmpModel.PAN = fc["PAN"].ToString();
            EmpModel.emailId = fc["emailId"].ToString();
            EmpModel.DateOfJoin = Convert.ToDateTime(fc["DateOfJoin"].ToString());
            EmpModel.Designation = fc["Designation"].ToString();
            EmpModel.Qualification = fc["Qualification"].ToString();

            if (Convert.ToInt16(fc["UserId"].ToString()) == 0)
            {
                EmpModel.UserId = Convert.ToInt16(0);
            }
            else
            {
                EmpModel.UserId = Convert.ToInt16(fc["UserId"].ToString());
            }
            EmpModel.UserId = Convert.ToInt16(fc["UserId"].ToString());
            EmpModel.Region = Convert.ToInt16(fc["Region"].ToString());
            EmpModel.ManagerId = Convert.ToInt32(fc["ManagerId"].ToString());

            EmpModel.ModifiedBy = Convert.ToInt16(1);
            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                EmpModel.Photo = new byte[uploadFile.ContentLength];
                uploadFile.InputStream.Read(EmpModel.Photo, 0, uploadFile.ContentLength);
            }
            else
            {
                EmpModel.Photo = null;
            }
            errorCode = objBL.SaveEmployee(EmpModel, User_Id, isClient, "");
            if (errorCode == 500001 || errorCode == 500002)
            {
                TempData["Msg"] = "Successfully Create ....";
                return RedirectToAction("GetEmployeeList");
            }
            else
            {
                TempData["Msg"] = "There are some Error Please Try Again...";
                return RedirectToAction("GetEmployeeList", new { EmpId = EmpModel.EmpId });
            }
        }
        [Authorize]
        public ActionResult SysUesrRemove(int EmpId)
        {
            EmployeeModel EmpModel = new EmployeeModel();
            int erroorCode = 0;
            AdminBL objbl = new AdminBL();
            erroorCode = objbl.SysUesrRemove(EmpId);

            EmpModel = Session["EmplloyeeModel"] as EmployeeModel;
            EmpModel.UserId = 0;
            EmpModel.UserName = null;
            EmpModel.DisplayName = null;
            Session["EmplloyeeModel"] = EmpModel;
            if (erroorCode == 500003)
            {
                return RedirectToAction("EditEmployee", new { EmpId = EmpId });
            }
            return View();
        }
        [Authorize]
        [HttpGet]
        public ActionResult DeleteEmployeeList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/DeleteEmployeeList");
            if (FindFlag)
            {
                List<EmployeeModel> lstEmp = new List<EmployeeModel>();
                AdminBL objBL = new AdminBL();
                lstEmp = objBL.GetEmployeeList(UserId);
                return View(lstEmp);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportEnableEmpLst")]
        [AcceptVerbs("POST")]
        public void ExportEnableEmpLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            AdminBL objBL = new AdminBL();          
            var DataSource = objBL.GetEmployeeList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        [Authorize]
        [HttpGet]
        public ActionResult DeleteEmployee(int EmpID)
        {
            EmployeeModel EmpModel = new EmployeeModel();
            AdminBL objBL = new AdminBL();
            EmpModel = objBL.GetSelectedEmployeeList(EmpID);
            return PartialView(EmpModel);
        }
        [Authorize]
        [HttpPost]
        public ActionResult DeleteEmployee(FormCollection fc, Boolean Deleted, DateTime LastDateOfWork)
        {
            string tempDate = "01/01/2015";
            int errcode = 0;
            AdminBL BLObj = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            EmployeeModel EmpModel = new EmployeeModel();
            EmpModel.EmpId = Convert.ToInt32(fc["EmpId"]);
            if (fc["LastDateOfWork"].ToString() == "1/1/0001 12:00:00 AM")
            {

                EmpModel.LastDateOfWork = Convert.ToDateTime(tempDate);
            }
            else
            {
                EmpModel.LastDateOfWork = LastDateOfWork;
            }

            EmpModel.Remark = fc["Remark"].ToString();
            EmpModel.Deleted = Deleted;
            errcode = BLObj.DeleteEmployee(EmpModel, User_Id);
            return RedirectToAction("DeleteEmployeeList");

        }
        public ActionResult EmployeeExprience(int Empid, string CompanyName)
        {
            ExperienceModel Model = new ExperienceModel();
            AdminBL BLobj = new AdminBL();
            try
            {
                Model = BLobj.GetSelectedEmployeeExprienceList(Empid, CompanyName);
                if (CompanyName != null)
                {
                    Model = BLobj.GetSelectedEmployeeExprienceList(Empid, CompanyName);
                    Model.NewCompanyName = Model.CompanyName;
                }
                else
                {
                    Model.EmpId = Empid;
                    Model.NewCompanyName = "New Entry";
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
           
                return PartialView(Model);
           
        }

        [HttpPost]
        public ActionResult EmployeeExprience(FormCollection fc)
        {
            ExperienceModel Model = new ExperienceModel();
            AdminBL BLobj = new AdminBL();
            try
            {
                Model.EmpId = Convert.ToInt32(fc["EmpId"].ToString());
                Model.CompanyName = fc["CompanyName"].ToString();
                Model.NewCompanyName = fc["NewCompanyName"].ToString();
                Model.Designation = fc["Designation"].ToString();
                Model.JobProfile = fc["JobProfile"].ToString();
                Model.StartDate = Convert.ToDateTime(fc["StartDate"].ToString());
                Model.EndDate = Convert.ToDateTime(fc["EndDate"].ToString());

                if (Model.NewCompanyName != "New Entry")
                {
                    string temp = Model.NewCompanyName;
                    Model.NewCompanyName = Model.CompanyName;
                    Model.CompanyName = temp;
                }
                int errcode = BLobj.SaveEmployeeExprienceList(Model);
                if (errcode == 500001 || errcode == 500002)
                {
                    return RedirectToAction("EditEmployee", new { Empid = Model.EmpId });
                }
                if (errcode == 600001)
                {
                    Session["Error"] = "This Company Experience Already Created";
                    return RedirectToAction("EditEmployee", new { Empid = Model.EmpId });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("EditEmployee", new { Empid = Model.EmpId });
        }

        public ActionResult SaveEmployeeDepartment(int EmpId, int DeptId)
        {
            int ErrCode = 0;
            try
            {
                AdminBL BlObj = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                ErrCode = BlObj.SaveEmployeeDepartment(EmpId, DeptId, User_Id);
                if (ErrCode > 0)
                {
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteEmployeeDepartment(int EmpID, int DeptId)
        {
            AdminBL objBL = new AdminBL();
            int errCode = objBL.DeleteEmployeeDepartment(EmpID, DeptId);
            return RedirectToAction("EditEmployee", new { EmpId = EmpID });
        }
        #endregion employee work

        #region Employee Assest

        public ActionResult CreateAssetDetails(int EmpId, int AssetId)
        {
            SysEmployeeAssetsModel AssetModel = new SysEmployeeAssetsModel();
            AdminBL objBL = new AdminBL();
            string UserId = User.Identity.GetUserId();
            //  AssetModel = objBL.GetSelectedEmployeeAsset(EmpId, AssetId);
            if (AssetId > 0)
            {
                AssetModel = objBL.GetSelectedEmployeeAsset(EmpId, AssetId);
                //  AssetModel.Experties = ExprtModel.NewExperties;
            }
            else
            {
                AssetModel.EmpId = EmpId;
                // AssetModel.NewExperties = "New Entry";SysAllAssetsListByEmployee
            }
            List<SysAssetsModel> assetlist = new List<SysAssetsModel>();
            List<SysAssetsModel> EmloyeeAssetList = new List<SysAssetsModel>();
            assetlist = objBL.SysAssetsList(UserId);
            EmloyeeAssetList = objBL.SysAllAssetsListByEmployee();
            foreach (SysAssetsModel demo in EmloyeeAssetList)
            {
                var actionToDelete = from Assets in assetlist
                                     where Assets.AssetId == demo.AssetId
                                     select Assets;
                assetlist.Remove(actionToDelete.ElementAt(0));
            }
            ViewBag.AssetDropDown = new SelectList(assetlist, "AssetId", "AssetName");

            return PartialView(AssetModel);


        }


        [HttpPost]
        public ActionResult CreateAssetDetails(FormCollection fc)
        {
            try
            {
                SysEmployeeAssetsModel AssetModel = new SysEmployeeAssetsModel();
                string User_Id = User.Identity.GetUserId();
                AdminBL objBL = new AdminBL();
                int errorCode = 0;

                AssetModel.EmpId = Convert.ToInt32(fc["EmpId"].ToString());
                AssetModel.AssetId = Convert.ToInt32(fc["AssetId"].ToString());


                AssetModel.AllocationDate = Convert.ToDateTime(fc["AllocationDate"].ToString());
                //if (fc["CreatedBy"].ToString() !=null)
                //{
                //    AssetModel.CreatedBy = fc["CreatedBy"].ToString();
                //}
                errorCode = objBL.SaveEmployeeAsset(AssetModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {

                    return RedirectToAction("EditEmployee", new { EmpId = AssetModel.EmpId });
                }
                else if (errorCode == 600001)
                {
                    Session["Error"] = "This Employee Asset has been Already Created";
                    return RedirectToAction("EditEmployee", new { EmpId = AssetModel.EmpId });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        public ActionResult DeallocateAsset(int EmpId, int AssetId)
        {
            int errcode = 0;
            try
            {
                AdminBL objbl = new AdminBL();
                errcode = objbl.DeallocateAsset(AssetId, EmpId);
                if (errcode == AssetId)
                {
                    return RedirectToAction("EditEmployee", new { EmpId = EmpId });
                }
                else
                    return RedirectToAction("EditEmployee", new { EmpId = EmpId });
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }


        #endregion Employee Assest

        #region [EmployeeCompMapping]
        [Authorize]
        public ActionResult EmployeeMapping(EmoloyeeMapping EmpMapModel)
        {
            AdminBL BlObj = new AdminBL();
            EmoloyeeMapping EmpMap = new EmoloyeeMapping();


            if (Session["EmpMapping"] != null)
            {
                EmpMap = Session["EmpMapping"] as EmoloyeeMapping;
                EmpMap.lstEmolyeeMap = BlObj.GetDWCompList();
                ViewBag.EmpMapList = new SelectList(EmpMap.lstEmolyeeMap, "CompCode", "CompanyName");
                if (EmpMap.lstClientMapEmpList != null)
                    ViewBag.ComEmpList = new SelectList(EmpMap.lstClientMapEmpList, "ComEmpID", "ComEmpName");

                //List<SysEmploy> lstEmplyee = new List<SysEmploy>();
                //lstEmplyee = BlObj.EmployeeDropDown();
                //ViewBag.Emplst = new SelectList(lstEmplyee, "EmpId", "EmpName");

            }

            else
            {

                EmpMap.lstEmolyeeMap = BlObj.GetDWCompList();  //for Compant list populate
                Session["EmpMapping"] = EmpMap;
                ViewBag.EmpMapList = new SelectList(EmpMap.lstEmolyeeMap, "CompCode", "CompanyName");
                //List<SysEmploy> lstEmplyee = new List<SysEmploy>();
                //lstEmplyee = BlObj.EmployeeDropDown();
                //ViewBag.Emplst = new SelectList(lstEmplyee, "EmpId", "EmpName");
            }

            return View(EmpMap);
        }
        [Authorize]
        public ActionResult GetCompanyEmp(string CompCode)
        {

            EmoloyeeMapping EmpMap = new EmoloyeeMapping();
            EmpMap = Session["EmpMapping"] as EmoloyeeMapping;
            AdminBL BlObj = new AdminBL();
            EmpMap.lstEmolyeeMap = BlObj.GetDWCompList();
            string SPName = "";
            Int16 ConnId = 0;

            foreach (EmoloyeeMapping TempModel in EmpMap.lstEmolyeeMap)
            {
                if (CompCode == TempModel.CompCode)
                {
                    SPName = TempModel.EmpListSP;
                    ConnId = TempModel.ConnectionId;
                    EmpMap.selectedComp = CompCode;
                }
            }
            SysDBConModel ConnModel = new SysDBConModel();
            SysDBConBL ConnobjBL = new SysDBConBL();
            ConnModel = ConnobjBL.GetSelectedDBConn(ConnId);
            ArrayList ConnInfo = new ArrayList();
            ConnInfo.Add(ConnModel.ConnectionId);
            ConnInfo.Add(ConnModel.ConnectionType);
            ConnInfo.Add(ConnModel.ServerName);
            ConnInfo.Add(ConnModel.DBName);
            ConnInfo.Add(ConnModel.UserName);
            ConnInfo.Add(ConnModel.Password);


            EmpMap.lstClientMapEmpList = BlObj.GetCompanyEmp(CompCode, SPName, ConnInfo);
            Session["EmpMapping"] = EmpMap;
            return RedirectToAction("EmployeeMapping", "Admin");
        }
        [Authorize]
        public ActionResult SaveMapping(string Emp_No, string CompCode, int EmpId)
        {
            if (Emp_No == null || Emp_No == "" || CompCode == null || CompCode == "" || EmpId == 0 || EmpId == null)
            {
                return RedirectToAction("GetEmployeeList");
            }
            int Errorcode = 0;
            EmoloyeeMapping EmpMap = new EmoloyeeMapping();
            AdminBL BlObj = new AdminBL();
            EmpMap.Emp_No = Emp_No;
            EmpMap.CompCode = CompCode;
            EmpMap.EmpId = EmpId;
            Errorcode = BlObj.SaveMapping(EmpMap);
            if (Errorcode == 500002)
            {
                return RedirectToAction("GetEmployeeList");
            }
            else
            {
                return RedirectToAction("GetEmployeeList");
            }

        }
        [Authorize]
        public ActionResult GetEmpMappingList(int EmpId, string SysEmpName)
        {
            EmoloyeeMapping EmpMap = new EmoloyeeMapping();

            AdminBL BlObj = new AdminBL();

            EmpMap.GridList = BlObj.GetEmployeeNavRelList(EmpId);
            EmpMap.EmpName = SysEmpName;
            EmpMap.EmpId = EmpId;
            foreach (BL.SysNavRelListModel SysEmp in EmpMap.GridList)
            {
                SysDBConModel ConnModel = new SysDBConModel();
                SysDBConBL ConnobjBL = new SysDBConBL();
                ConnModel = ConnobjBL.GetSelectedDBConn(SysEmp.ConnectionId);
                ArrayList ConnInfo = new ArrayList();
                ConnInfo.Add(ConnModel.ConnectionId);
                ConnInfo.Add(ConnModel.ConnectionType);
                ConnInfo.Add(ConnModel.ServerName);
                ConnInfo.Add(ConnModel.DBName);
                ConnInfo.Add(ConnModel.UserName);
                ConnInfo.Add(ConnModel.Password);


                string StrTemp = BlObj.GetClientEmpName(SysEmp.CompName, SysEmp.CompEmpID, ConnInfo);
                SysEmp.EmpName = StrTemp;

            }

            Session["EmpMapping"] = EmpMap;
            return RedirectToAction("EmployeeMapping", "Admin");

        }
        [Authorize]
        public ActionResult DeleteEmpMapping(string CompName, int EmpID)
        {
            int Errorcod = 0;
            EmoloyeeMapping EmpMap = new EmoloyeeMapping();
            AdminBL BlObj = new AdminBL();
            Errorcod = BlObj.DeleteEmpMapping(CompName, EmpID);
            if (Errorcod == 500003)
            {
                EmpMap = Session["EmpMapping"] as EmoloyeeMapping;
                EmpMap.GridList = BlObj.GetEmployeeNavRelList(EmpID);
                EmpMap.EmpId = EmpID;
                foreach (BL.SysNavRelListModel SysEmp in EmpMap.GridList)
                {
                    SysDBConModel ConnModel = new SysDBConModel();
                    SysDBConBL ConnobjBL = new SysDBConBL();
                    ConnModel = ConnobjBL.GetSelectedDBConn(SysEmp.ConnectionId);
                    ArrayList ConnInfo = new ArrayList();
                    ConnInfo.Add(ConnModel.ConnectionId);
                    ConnInfo.Add(ConnModel.ConnectionType);
                    ConnInfo.Add(ConnModel.ServerName);
                    ConnInfo.Add(ConnModel.DBName);
                    ConnInfo.Add(ConnModel.UserName);
                    ConnInfo.Add(ConnModel.Password);
                    string StrTemp = BlObj.GetClientEmpName(SysEmp.CompName, SysEmp.CompEmpID, ConnInfo);
                    SysEmp.EmpName = StrTemp;

                }
                Session["EmpMapping"] = EmpMap;
                return RedirectToAction("EmployeeMapping", "Admin");
            }
            else
            {
                return RedirectToAction("EmployeeMapping", "Admin");
            }
        }

        #endregion [EmployeeCompMapping]

        #region [RoleReportMapping]

        [Authorize]
        public ActionResult RoleReportMapping(string strRoleID)
        {
            RoleRptMapingModel rlrptModel = null;
            try
            {



                if (Session["RoleRptMappingModel"] == null)
                {
                    rlrptModel = new RoleRptMapingModel();
                    AdminBL adminBlObj = new AdminBL();
                    rlrptModel = adminBlObj.GetReportListByRole(strRoleID);

                    Session["RoleRptMappingModel"] = rlrptModel;
                }
                else
                {
                    rlrptModel = Session["RoleRptMappingModel"] as RoleRptMapingModel;
                }

                ViewBag.RptList = new SelectList(rlrptModel.lstRpts, "ReportID", "ReportName");
                ViewBag.AssignedRptList = new SelectList(rlrptModel.lstAssignedRpts, "ReportID", "ReportName");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return View(rlrptModel);
        }

        [HttpPost]
        public ActionResult SaveRoleReportMapping(FormCollection frmCollection)
        {
            RoleRptMapingModel rlrptModel = null;
            string RoleID = frmCollection["RoleInfo.RoleID"].ToString();
            if (Session["RoleRptMappingModel"] != null)
            {
                rlrptModel = Session["RoleRptMappingModel"] as RoleRptMapingModel;
            }
            else
            {
                rlrptModel = new RoleRptMapingModel();
            }
            foreach (string Key in frmCollection.Keys)
            {
                if (Key.ToString() == "lstAssignedRpts")
                {
                    List<string> lstSelectedRptID = frmCollection["lstAssignedRpts"].Split(',').ToList();

                    string strRoleID = frmCollection["RoleInfo.RoleID"].ToString();

                    rlrptModel.lstRoleRptMapList = new List<RoleRptMapingModel.RoleReportMapping>();
                    foreach (string str in lstSelectedRptID)
                    {
                        RoleRptMapingModel.RoleReportMapping roleReportMapping = new RoleRptMapingModel.RoleReportMapping();
                        roleReportMapping.RoleID = strRoleID;
                        roleReportMapping.ReportID = str;
                        rlrptModel.lstRoleRptMapList.Add(roleReportMapping);
                    }
                }
            }
            AdminBL adminBlObj = new AdminBL();
            int iErrorCode = adminBlObj.SaveRoleReportMapping(rlrptModel, RoleID);
            Session["RoleRptMappingModel"] = null;
            return RedirectToAction("SytemRoleList", "Admin");
        }



        #endregion [RoleReportMapping]

        public ActionResult Information(string Msg)
        {
            ViewBag.Msg = Msg;

            return PartialView();
        }

        #region Employee User DashBoard Mapping
        public ActionResult DashBoardMap(string UserName, int UserId)
        {
            try
            {
                AdminBL BLObj = new AdminBL();
                SysDashBoardModel sysBoard = new SysDashBoardModel();
                List<ListDashBoard> DrpDashBoard = new List<ListDashBoard>();
                if (Session["Errmsg"] != null)
                {
                    TempData["MSG"] = (string)Session["Errmsg"];
                }
                Session["Errmsg"] = null;
                if (Session["SysDashBoardModel"] == null)
                {
                    sysBoard = BLObj.GetDashBordList(UserId);
                    DrpDashBoard = BLObj.DrpDwnDashBoardLst();
                    sysBoard.UserId = UserId;
                    sysBoard.UserName = UserName;
                    foreach (UserDashboardModel demo in sysBoard.lstUserDashBoard)
                    {
                        var actionToDelete = from actionRepDel in DrpDashBoard
                                             where actionRepDel.DashboardId == demo.DashboardId
                                             select actionRepDel;
                        DrpDashBoard.Remove(actionToDelete.ElementAt(0));
                    }
                    Session["SysDashBoardModel"] = sysBoard;
                    ViewBag.LstDashBoard = new SelectList(DrpDashBoard, "DashboardId", "Description");
                    Session["DashboardCompnent"] = null;
                    Session["DashboardCompnent"] = sysBoard.lstUserDashBoard;
                    return View(sysBoard);

                }
                else
                {
                    sysBoard = Session["SysDashBoardModel"] as SysDashBoardModel;
                    DrpDashBoard = BLObj.DrpDwnDashBoardLst();
                    sysBoard.UserId = UserId;
                    foreach (UserDashboardModel demo in sysBoard.lstUserDashBoard)
                    {
                        var actionToDelete = from actionRepDel in DrpDashBoard
                                             where actionRepDel.DashboardId == demo.DashboardId
                                             select actionRepDel;
                        DrpDashBoard.Remove(actionToDelete.ElementAt(0));
                    }
                    ViewBag.LstDashBoard = new SelectList(DrpDashBoard, "DashboardId", "Description");
                    Session["DashboardCompnent"] = null;
                    Session["DashboardCompnent"] = sysBoard.lstUserDashBoard;
                    return View(sysBoard);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ActionResult SaveAssignDashBoard(int UserId)
        {
            try
            {
                int ErrCode = 0;
                SysDashBoardModel SysDashBord = new SysDashBoardModel();
                SysDashBord = Session["SysDashBoardModel"] as SysDashBoardModel;
                AdminBL BLObj = new AdminBL();
                ErrCode = BLObj.SaveAssignDashBoard(SysDashBord, UserId);
                if (ErrCode == 4)
                {
                    Session["SysDashBoardModel"] = null;
                    return RedirectToAction("UserList");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }
        public ActionResult AddClientEmp(Int16 lstDashBoard, Int16 Sequence, int Uid, string UserName, string Description)
        {
            SysDashBoardModel SysDashBord = new SysDashBoardModel();
            //  SysDashBord.lstUserDashBoard = new List<UserDashboardModel>();
            if (Session["SysDashBoardModel"] != null)
            {
                SysDashBord = Session["SysDashBoardModel"] as SysDashBoardModel;
            }
            bool Flag = false;
            List<UserDashboardModel> lst = Session["DashboardCompnent"] as List<UserDashboardModel>;
            foreach (UserDashboardModel item in lst)
            {
                var existseq = item.Sequence;
                if (existseq == Sequence)
                { Flag = true; break; }

            }
            UserDashboardModel DemoModel = new UserDashboardModel();
            DemoModel.DashboardId = lstDashBoard;
            DemoModel.UserId = Uid;
            DemoModel.Sequence = Sequence;
            DemoModel.UserName = UserName;
            DemoModel.Description = Description;

            if (Flag == false)
            {
                SysDashBord.lstUserDashBoard.Add(DemoModel);
                Session["SysDashBoardModel"] = SysDashBord;
            }

            if (Flag)
                Session["Errmsg"] = "This Sequence Number Already Exist. Please try with another Sequence number";
            return RedirectToAction("DashBoardMap", new { UserId = Uid, UserName = UserName });

        }
        public ActionResult DeleteClientEmp(Int16 lstDashBoard, int Uid)
        {
            SysDashBoardModel SysDashBord = new SysDashBoardModel();
            SysDashBord.lstUserDashBoard = new List<UserDashboardModel>();
            if (Session["SysDashBoardModel"] != null)
            {
                SysDashBord = Session["SysDashBoardModel"] as SysDashBoardModel;
            }
            var actionToDelete = from actionRepDel in SysDashBord.lstUserDashBoard
                                 where actionRepDel.DashboardId == lstDashBoard
                                 select actionRepDel;
            SysDashBord.lstUserDashBoard.Remove(actionToDelete.ElementAt(0));
            Session["SysDashBoardModel"] = SysDashBord;
            return RedirectToAction("DashBoardMap", new { UserId = Uid });
        }
        #endregion  Employee User DashBoard Mapping

        #region  Client Application
        public ActionResult GetClientAppList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/GetClientAppList");
            if (FindFlag)
            {
                List<ClientAppModel> lstClientApp = new List<ClientAppModel>();
                try
                {
                    AdminBL BLobj = new AdminBL();
                    lstClientApp = BLobj.GetClientAppList(UserId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(lstClientApp);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult CreateClientApp()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/CreateClientApp");
            if (FindFlag)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult EditClientApp(Int16 ClientAppId)
        {
            ClientAppModel ClientModel = new ClientAppModel();
            try
            {
                AdminBL BLobj = new AdminBL();
                ClientModel = BLobj.GetSelecteedClientApp(ClientAppId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(ClientModel);
        }
        [HttpPost]
        public ActionResult EditClientApp(FormCollection fc)
        {
            AdminBL BLobj = new AdminBL();
            ClientAppModel ClientModel = new ClientAppModel();
            ClientModel.ClientAppId = Convert.ToInt16(fc["ClientAppId"].ToString());
            ClientModel.AppName = fc["AppName"].ToString();
            ClientModel.Description = fc["Description"].ToString();
            ClientModel.LoginName = fc["LoginName"].ToString();
            ClientModel.Password = fc["Password"].ToString();
            ClientModel.User_Id = User.Identity.GetUserId();
            int errCode = BLobj.SaveClientApp(ClientModel);
            if (errCode == 500001)
            {
                return RedirectToAction("GetClientAppList");
            }
            return View();
        }
        [HttpPost]
        public ActionResult CreateClientApp(FormCollection fc)
        {
            try
            {
                AdminBL BLobj = new AdminBL();
                ClientAppModel ClientModel = new ClientAppModel();
                ClientModel.ClientAppId = Convert.ToInt16(0);
                ClientModel.AppName = fc["AppName"].ToString();
                ClientModel.Description = fc["Description"].ToString();
                ClientModel.LoginName = fc["LoginName"].ToString();
                ClientModel.Password = fc["Password"].ToString();
                ClientModel.User_Id = User.Identity.GetUserId();
                int errCode = BLobj.SaveClientApp(ClientModel);
                if (errCode == 500002)
                {
                    return RedirectToAction("GetClientAppList");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        #endregion  Client Application

        #region Company Detail

        public ActionResult GetCompanyList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/GetCompanyList");
            if (FindFlag)
            {
                List<CompanyModel> lstCompany = new List<CompanyModel>();
                try
                {
                    AdminBL BLobj = new AdminBL();
                    lstCompany = BLobj.GetCompanyList(UserId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(lstCompany);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportSysCompanyLst")]
        [AcceptVerbs("POST")]
        public void ExportSysCompanyLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            AdminBL BLobj = new AdminBL();          
            var DataSource = BLobj.GetCompanyList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult EditCompany(Int16 CompId)
        {
            CompanyModel companyModel = new CompanyModel();
            try
            {
                AdminBL BLobj = new AdminBL();
                companyModel = BLobj.GetSelectedCompany(CompId);
                Session["logo"] = companyModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(companyModel);
        }
        [HttpPost]
        public ActionResult EditCompany(FormCollection fc, HttpPostedFileBase uploadFile)
        {
            try
            {

                AdminBL BLobj = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                CompanyModel companyModel = new CompanyModel();
                companyModel.CompId = Convert.ToInt16(fc["CompId"]);
                companyModel.Name = fc["Name"].ToString();
                companyModel.ShortName = fc["ShortName"].ToString();
                companyModel.AddressLine1 = fc["AddressLine1"].ToString();
                companyModel.AddressLine2 = fc["AddressLine2"].ToString();
                companyModel.City = fc["City"].ToString();
                companyModel.State = fc["State"].ToString();
                companyModel.Country = fc["Country"].ToString();
                companyModel.Pin = fc["Pin"].ToString();
                companyModel.TagLine = fc["TagLine"].ToString();
                companyModel.Cst = fc["Cst"].ToString();
                companyModel.Bst = fc["Bst"].ToString();
                companyModel.ServiceTN = fc["ServiceTN"].ToString();

                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    companyModel.Logo = new byte[uploadFile.ContentLength];
                    uploadFile.InputStream.Read(companyModel.Logo, 0, uploadFile.ContentLength);
                }
                else
                {
                    CompanyModel demo = new CompanyModel();
                    demo = Session["logo"] as CompanyModel;
                    companyModel.Logo = demo.Logo;
                }
                int errCode = BLobj.SaveCompany(companyModel, User_Id);
                if (errCode == 500001)
                {
                    Session["logo"] = null;
                    return RedirectToAction("GetCompanyList");
                }
                else
                {
                    return RedirectToAction("GetCompanyList");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult CreateSysCompany()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/CreateSysCompany");
            if (FindFlag)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateSysCompany(FormCollection fc, HttpPostedFileBase uploadFile)
        {
            try
            {
                AdminBL BLobj = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                CompanyModel companyModel = new CompanyModel();
                companyModel.CompId = Convert.ToInt16(0);
                companyModel.Name = fc["Name"].ToString();
                companyModel.ShortName = fc["ShortName"].ToString();
                companyModel.AddressLine1 = fc["AddressLine1"].ToString();
                companyModel.AddressLine2 = fc["AddressLine2"].ToString();
                companyModel.City = fc["City"].ToString();
                companyModel.State = fc["State"].ToString();
                companyModel.Country = fc["Country"].ToString();
                companyModel.Pin = fc["Pin"].ToString();
                companyModel.TagLine = fc["TagLine"].ToString();
                companyModel.Cst = fc["Cst"].ToString();
                companyModel.Bst = fc["Bst"].ToString();
                companyModel.ServiceTN = fc["ServiceTN"].ToString();

                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    companyModel.Logo = new byte[uploadFile.ContentLength];
                    uploadFile.InputStream.Read(companyModel.Logo, 0, uploadFile.ContentLength);
                }

                int errCode = BLobj.SaveCompany(companyModel, User_Id);
                if (errCode == 500002)
                {
                    return RedirectToAction("GetCompanyList");
                }
                else
                {
                    return RedirectToAction("GetCompanyList");
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion  Company Detail

        #region BankDetail Work
        public ActionResult CreateEmployeeBankDetails(int EmpId, string AccountNo)
        {

            try
            {
                EmployeeBankDetailModel Model = new EmployeeBankDetailModel();
                AdminBL objbl = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                // bank Dropdown
                BankDetailModel objmodel = new BankDetailModel();
                objmodel.BankDetailList = new List<BankDetailModel>();
                BankBL obj = new BankBL();
                objmodel.BankDetailList = obj.bankDetailList(User_Id);
                ViewBag.Banks = new SelectList(objmodel.BankDetailList, "EmpBankName", "EmpBankName");

                if (AccountNo != null)
                {
                    Model = objbl.GetSelectedEmployeeBankDetail(EmpId, AccountNo);
                    Model.NewAccountNo = Model.AccountNo;
                }
                else
                {
                    Model.EmpId = EmpId;
                    Model.NewAccountNo = "New Entry";
                }
              
                    return PartialView(Model);
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpPost]
        public ActionResult CreateEmployeeBankDetails(FormCollection fc)
        {
            try
            {
                AdminBL objbl = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                EmployeeBankDetailModel EmployeeModel = new EmployeeBankDetailModel();
                int errorCode = 0;
                EmployeeModel.EmpId = Convert.ToInt32(fc["EmpId"].ToString());
                EmployeeModel.AccountNo = fc["AccountNo"].ToString();
                EmployeeModel.NewAccountNo = fc["NewAccountNo"].ToString();
                EmployeeModel.BankName = fc["BankName"].ToString();

                EmployeeModel.Limit = Convert.ToDouble(fc["Limit"].ToString());
                if (EmployeeModel.NewAccountNo != "New Entry")
                {
                    string temp = EmployeeModel.NewAccountNo;
                    EmployeeModel.NewAccountNo = EmployeeModel.AccountNo;
                    EmployeeModel.AccountNo = temp;
                }
                errorCode = objbl.saveEmployeeBankDetailsInfo(EmployeeModel, User_Id);
                if (errorCode == 500001 || errorCode == 500002)
                {
                    TempData["Msg"] = "Successfully Create ....";
                    return RedirectToAction("EditEmployee", new { EmpId = EmployeeModel.EmpId, tabIndex = 2 });
                }
                else if (errorCode == 500000)
                {

                    Session["Error"] = "This  Bank Account Number  Already Created ";
                    return RedirectToAction("EditEmployee", new { EmpId = EmployeeModel.EmpId, tabIndex = 2 });
                }
                return RedirectToAction("EditEmployee", new { EmpId = EmployeeModel.EmpId, tabIndex = 2 });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion BankDetail Work

        #region Employee Address
        public ActionResult CreateEmployeeAddress(int EmpId, int AddressId)
        {
            EmployeeAddressModel addressModel = new EmployeeAddressModel();
            try
            {
                AdminBL objbl = new AdminBL();

                addressModel.AddressId = AddressId;
                addressModel.LstCountry = objbl.GetCountryList();
                ViewBag.CountryList = new SelectList(addressModel.LstCountry, "CountryId", "CountryName");

            }

            catch (Exception)
            {

                throw;
            }

            return PartialView(addressModel);
        }
        [HttpPost]
        public ActionResult CreateEmployeeAddress(FormCollection fc, bool isPrimary)
        {
            EmployeeAddressModel objmodel = new EmployeeAddressModel();

            try
            {
                AdminBL objbl = new AdminBL();
                int errorcode = 0;
                objmodel.EmpId = Convert.ToInt32(fc["EmpId"].ToString());
                if (objmodel.EmpId > 0)
                {
                    objmodel.AddressId = Convert.ToInt32(fc["AddressId"]);
                }
                else
                {
                    objmodel.AddressId = 0;
                }
                objmodel.Line1 = fc["Line1"].ToString();
                objmodel.Line2 = fc["Line2"].ToString();
                objmodel.StateId = Convert.ToInt32(fc["StateId"].ToString());
                objmodel.CityId = Convert.ToInt32(fc["CityId"].ToString());
                objmodel.CountryId = Convert.ToInt32(fc["CountryId"].ToString());
                objmodel.Pin = fc["Pin"].ToString();
                objmodel.Description = fc["Description"].ToString();
                objmodel.LandMark = fc["LandMark"].ToString();
                errorcode = objbl.SaveAddressDeatil(objmodel, isPrimary, objmodel.Description);
                if (errorcode == 500001 || errorcode == 500002)
                {

                    TempData["Msg"] = "Successfully Create...";
                    return RedirectToAction("EditEmployee", new { EmpId = objmodel.EmpId, tabIndex = 3 });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("EditEmployee", new { EmpId = objmodel.EmpId, tabIndex = 3 });

        }

        //public ActionResult SaveCreateAddress(int AddressId, int EmpId, string Line1, string Line2, string Pin, string LandMark, string Country, string State, string City, string Description, bool isPrimary)
        //{
        //    EmployeeAddressModel objmodel = new EmployeeAddressModel();
        //    try
        //    {
        //        AdminBL objbl = new AdminBL();
        //        int errorcode = 0;
        //        objmodel.EmpId = EmpId;
        //        if (objmodel.EmpId > 0)
        //        {
        //            objmodel.AddressId = AddressId;
        //        }
        //        else
        //        {
        //            objmodel.AddressId = 0;
        //        }
        //        objmodel.Line1 = Line1;
        //        objmodel.Line2 = Line2;
        //        objmodel.State = State;
        //        objmodel.City = City;
        //        objmodel.Country = Country;
        //        objmodel.Pin = Pin;
        //        objmodel.Description = Description;
        //        objmodel.LandMark = LandMark;
        //        errorcode = objbl.SaveAddressDeatil(objmodel, isPrimary, Description);
        //        if (errorcode == 500001 || errorcode == 500002)
        //        {
        //            Session["AddressList"] = null;
        //            TempData["Msg"] = "Successfully Create...";
        //            return RedirectToAction("EditEmployee", new { EmpId = objmodel.EmpId, tabIndex = 3 });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return RedirectToAction("EditEmployee", new { EmpId = objmodel.EmpId, tabIndex = 3 });
        //}


        public ActionResult EditEmployeeAddress(int EmpId, int AddressId, int State, int country)
        {
            EmployeeAddressModel addressModel = new EmployeeAddressModel();
            AdminBL objbl = new AdminBL();
            List<EmployeeStateModel> StateList = new List<EmployeeStateModel>();
            if (AddressId > 0)
            {
                addressModel = objbl.GetAddressSelectedList(AddressId);
            }
            addressModel.LstCountry = objbl.GetCountryList();
            ViewBag.CountryList = new SelectList(addressModel.LstCountry, "CountryId", "CountryName");
            ViewBag.CountryL = addressModel.LstCountry;
            StateList = objbl.GetStateList(country);
            ViewBag.StateList = new SelectList(StateList, "StateId", "StateName");
            ViewBag.StateList1 = StateList;
            addressModel.LstCity = objbl.GetCityList(State);
            ViewBag.Citylist = new SelectList(addressModel.LstCity, "CityId", "CityName", addressModel.CityId);

            return PartialView(addressModel);
        }
        public JsonResult GetStateList(int CountryId)
        {
            EmployeeAddressModel addressModel = new EmployeeAddressModel();
            List<EmployeeStateModel> lst = new List<EmployeeStateModel>();
            try
            {
                AdminBL objbl = new AdminBL();


                addressModel.CountryId = CountryId;
                lst = objbl.GetStateList(CountryId);

                ViewBag.List = new SelectList(lst, "StateId", "StateName");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(new SelectList(lst, "StateId", "StateName"));
        }
        public JsonResult GetCityList(int StateId)
        {
            EmployeeAddressModel addressModel = new EmployeeAddressModel();
            try
            {
                AdminBL objbl = new AdminBL();

                addressModel.StateId = StateId;
                addressModel.LstCity = objbl.GetCityList(StateId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(new SelectList(addressModel.LstCity, "CityId", "CityName"));
        }

        #endregion Employee Address

        #region Salary Component
        public ActionResult SalaryComponentList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/SalaryComponentList");
            if (FindFlag)
            {
                List<SalaryComponentmodel> lstsalary = new List<SalaryComponentmodel>();
                try
                {
                    AdminBL adminbl = new AdminBL();
                    lstsalary = adminbl.SalaryComponentList(UserId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(lstsalary);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportSysComponentLst")]
        [AcceptVerbs("POST")]
        public void ExportSysComponentLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            AdminBL adminbl = new AdminBL();         
            var DataSource = adminbl.SalaryComponentList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        [HttpGet]
        public ActionResult CreateSalarycomponent()
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
            AdminBL adminbl = new AdminBL();
            List<SalaryComponentmodel> lstsalaryComponent = adminbl.ParentComponentList(0);
            ViewBag.ComponentList = new SelectList(lstsalaryComponent, "ComponentId", "Name");
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/CreateSalarycomponent");
            if (FindFlag)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateSalarycomponent(FormCollection Fc, bool Taxable, bool Fixed)
        {
            SalaryComponentmodel objmodel = new SalaryComponentmodel();
            string User_Id = User.Identity.GetUserId();
            int errorCode = 0;
            try
            {
                AdminBL adminbl = new AdminBL();
                objmodel.ComponentId = 0;
                objmodel.Name = Fc["Name"].ToString();
                if (Fc.AllKeys.Contains("ParentId"))
                    objmodel.ParentComponentId = Convert.ToInt32(Fc["ParentId"].ToString());
                if (Fc["VariableRate"].ToString() != "")
                    objmodel.VariableRate = Convert.ToDouble(Fc["VariableRate"].ToString());

                objmodel.DRCR = Fc["DRCR"].ToString();
                objmodel.Frequency = Fc["Frequency"].ToString();

                errorCode = adminbl.SaveSalaryComponent(objmodel, User_Id, Taxable, Fixed);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (errorCode == 500001 || errorCode == 500002)
            {
                TempData["Msg"] = "Successfully Create ....";
                return RedirectToAction("SalaryComponentList");
            }
            else
            {
                TempData["Msg"] = "There are some Error Please Try Again...";
                return RedirectToAction("SalaryComponentList");
            }

        }
        [HttpGet]
        public ActionResult EditSalaryComponent(int ComponentId)
        {
            SalaryComponentmodel salarymodel = new SalaryComponentmodel();
            try
            {

                AdminBL adminbl = new AdminBL();
                List<SalaryComponentmodel> lstsalaryComponent = adminbl.ParentComponentList(ComponentId);
                ViewBag.ComponentList = new SelectList(lstsalaryComponent, "ComponentId", "Name");
                salarymodel = adminbl.GetSelectedSalaryComponent(ComponentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(salarymodel);

        }
        public ActionResult GetActiveEmployeeList()
        {
            List<EmpChartModel> EmpModelList = new List<EmpChartModel>();
            ViewsBL BLObj = new ViewsBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                EmpModelList = BLObj.GetEmployeeViewList(User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(EmpModelList);
        }
        [System.Web.Http.ActionName("ExportSysActiveEmpLst")]
        [AcceptVerbs("POST")]
        public void ExportSysActiveEmpLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            ViewsBL BLObj = new ViewsBL();
            var DataSource = BLObj.GetEmployeeViewList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult GetEmployeeSalaryStructure(int EmpId, string EmpName)
        {
            List<EmpSalaryStructureModel> salStructureDetail = new List<EmpSalaryStructureModel>();
            AdminBL BLObj = new AdminBL();
            try
            {
                salStructureDetail = BLObj.SalaryStructureList(EmpId);
                ViewBag.EmpId = EmpId;
                ViewBag.EmpName = EmpName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(salStructureDetail);
        }
        public ActionResult DeleteEmployeeSalaryStructure(int EmpId, int ComponentId)
        {

            AdminBL BLObj = new AdminBL();
            try
            {
                AdminBL adminbl = new AdminBL();
                int Errorcode = BLObj.DeleteEmployeeSalaryStructure(EmpId, ComponentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetselectedEmpSalaryStructureDetails", new { EmpId = EmpId });
        }
        public ActionResult AddEmployeeSalaryStructureDetail(int EmpId, double AnnualFixPay, double TotalAmount)
        {
            AdminBL BLObj = new AdminBL();
            try
            {

                ViewBag.AnnualFixPay = AnnualFixPay;
                ViewBag.TotalAmount = TotalAmount;
                AdminBL adminbl = new AdminBL();
                List<SalaryComponentmodel> lstsalaryComponent = adminbl.GetComponentListforDrp(0, EmpId);
                ViewBag.ComponentList = new SelectList(lstsalaryComponent, "ComponentId", "Name");
                ViewBag.EmpId = EmpId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView();
        }

        public ActionResult AddSalaryStructure(int ParentId, Double VariableRate, int EmpId)
        {
            EmpSalaryStructureModel objmodel = new EmpSalaryStructureModel();
            AdminBL BLObj = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                objmodel.EmpId = EmpId;
                objmodel.ComponentId = ParentId;
                objmodel.Amount = VariableRate;
                int ErrorCode = BLObj.SaveEmployeeSalaryStructure(objmodel, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetselectedEmpSalaryStructureDetails", new { EmpId = EmpId });
        }
        [HttpPost]
        public ActionResult EditSalaryComponent(FormCollection fc, bool Taxable, bool Fixed)
        {
            int errcode = 0;
            try
            {
                string User_Id = User.Identity.GetUserId();
                SalaryComponentmodel objmodel = new SalaryComponentmodel();
                AdminBL objbl = new AdminBL();
                if (Fixed)
                {
                    objmodel.ParentComponentId = 0;
                    if (fc["VariableRate"].ToString() != "")
                        objmodel.VariableRate = Convert.ToDouble(fc["VariableRate"].ToString());
                }
                else
                {
                    if (fc["VariableRate"].ToString() != "")
                        objmodel.VariableRate = Convert.ToDouble(fc["VariableRate"].ToString());
                    if (fc["ParentId"].ToString() != "")
                        objmodel.ParentComponentId = Convert.ToInt32(fc["ParentId"].ToString());
                }


                objmodel.Name = fc["Name"].ToString();
                objmodel.ComponentId = Convert.ToInt32(fc["ComponentId"].ToString());
                objmodel.DRCR = fc["DRCR"].ToString();
                objmodel.Frequency = fc["Frequency"].ToString();

                errcode = objbl.SaveSalaryComponent(objmodel, User_Id, Taxable, Fixed);
                if (errcode == 500001 || errcode == 500002)
                {
                    return RedirectToAction("SalaryComponentList");

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        #endregion Salary Component

        #region Sys_Issue_Details

        public ActionResult GetIssueList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/GetIssueList");
            if (FindFlag)
            {
                List<IssueModel> LstIssueList = new List<IssueModel>();
                try
                {
                    AdminBL issuebl = new AdminBL();
                    LstIssueList = issuebl.GetIssueDetailList(UserId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(LstIssueList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportIssueList")]
        [AcceptVerbs("POST")]
        public void ExportIssueList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            AdminBL issuebl = new AdminBL();
            var DataSource = issuebl.GetIssueDetailList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateIssue(int IssueId, int tabIndex)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/CreateIssue?IssueId=0&tabIndex=0");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                IssueModel issumodel = new IssueModel();
                AdminBL issubl = new AdminBL();
                string Status = "";
                issumodel.LstCommentList = new List<CommentDatamodel>();
                if (IssueId > 0)
                {

                    issumodel = issubl.GetSelectedIssueList(IssueId, User_Id);
                    if (issumodel.IsAdmin.ToString() == "1")
                    {
                        ViewBag.Isadmin = "Admin";
                    }
                    ViewBag.Status = issumodel.Status;
                    issumodel.LstCommentList = issubl.GetSelectedProjectCommentList(IssueId);
                    ViewBag.comments = issumodel.LstCommentList;
                }
                if (issumodel.LstCommentList.Count > 0)
                {
                    var last = issumodel.LstCommentList.Last();

                    var Id = last.IssueId;
                    var Comments = last.Comments;
                    issumodel.IssueId = Id;
                    issumodel.Comments = "";
                    Status = last.Status;
                }
                if (Status.ToString() == "NEW" || IssueId == 0)
                {
                    issumodel.Status = Convert.ToString("0");
                }
                else
                {
                    issumodel.Status = issumodel.Status;
                }




                lstTaskmodel = (List<SysTaskModel>)Session["UserMenu"];
                bool Chk = modelObj.FindMenu(lstTaskmodel, "AllowTicketType");
                if (Chk)
                {
                    issumodel.isAllow = "true";
                }
                else
                {
                    issumodel.isAllow = "false";
                }

                return View(issumodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ApprovalDetail(int IssueId)
        {
            IssueModel issumodel = new IssueModel();
            AdminBL issubl = new AdminBL();
            issumodel.LstCommentList = new List<CommentDatamodel>();
            if (IssueId > 0)
            {
                string User_Id = User.Identity.GetUserId();
                issumodel = issubl.GetSelectedIssueList(IssueId, User_Id);
                issumodel.LstCommentList = issubl.GetSelectedProjectCommentList(IssueId);
                ViewBag.comments = issumodel.LstCommentList;
            }
            return View(issumodel);
        }
        public ActionResult IssueApprovalList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/IssueApprovalList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                List<IssueModel> LstIssueList = new List<IssueModel>();
                try
                {
                    AdminBL issuebl = new AdminBL();
                    LstIssueList = issuebl.GetIssueListForApproval(User_Id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(LstIssueList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportToExcelPendingIssueApprovalList")]
        [AcceptVerbs("POST")]
        public void ExportToExcelPendingIssueApprovalList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            AdminBL issuebl = new AdminBL();
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = issuebl.GetIssueListForApproval(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }

        public ActionResult SubordinateIssueApprovalList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/IssueApprovalList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                List<IssueModel> LstIssueList = new List<IssueModel>();
                try
                {
                    AdminBL issuebl = new AdminBL();
                    LstIssueList = issuebl.GetSubordinateIssueAprovalList(User_Id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(LstIssueList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportToExcelSubordinatePendingIssueApprovalList")]
        [AcceptVerbs("POST")]
        public void ExportToExcelSubordinatePendingIssueApprovalList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            AdminBL issuebl = new AdminBL();
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            var DataSource = issuebl.GetIssueListForApproval(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }

        public ActionResult IssueApprovalDetail(int IssueId)
        {
            IssueModel issumodel = new IssueModel();
            AdminBL issubl = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            issumodel = issubl.GetSelectedIssueList(IssueId, User_Id);
            return PartialView(issumodel);
        }
        public ActionResult IssueRejectedComments(int IssueId, int statusid, string View)
        {
            IssueModel issumodel = new IssueModel();
            issumodel.IssueId = IssueId;
            issumodel.Statuscode = statusid;
            TempData["View"] = View;
            ViewBag.View = View;
            return PartialView(issumodel);
        }
        public ActionResult EstimationDone(int IssueId, float EstimationHours)
        {
            IssueModel issumodel = new IssueModel();
            string User_Id = User.Identity.GetUserId();
            AdminBL issubl = new AdminBL();
            issumodel.IssueId = IssueId;
            int statusid = 46;
            int Errorcode = issubl.EstimationDone(IssueId, EstimationHours, statusid, User_Id);
            return View();
        }
        public ActionResult UpdateApprovalDetail(int IssueId, int statusid, string Comments)
        {
            AdminBL objbl = new AdminBL();
            string View = "";
            string User_Id = User.Identity.GetUserId();
            if (TempData["View"] != null)
            {
                View = (string)TempData["View"];
            }
            int Ercode = objbl.updateSysIssues(IssueId, User_Id, statusid, Comments);
            TempData["View"] = null;
            if (Ercode != 0)
            {
                if (View == "Approval")
                {
                    return RedirectToAction("IssueApprovalList");
                }
                else
                {
                    return RedirectToAction("CreateIssue", new { IssueId = IssueId, tabIndex = 0 });

                }

            }

            return RedirectToAction("CreateIssue", new { IssueId = IssueId, tabIndex = 0 });
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateIssue(HttpPostedFileBase file, FormCollection fc)
        {
            string User_Id = User.Identity.GetUserId();
            IssueModel issumodel = new IssueModel();
            AdminBL issuebl = new AdminBL();
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");
            string fileName = "";

            if (file != null)
            {
                fileName = System.IO.Path.GetFileName(file.FileName);
                if (file.ContentLength > 0)
                {
                    try
                    {
                        string[] FileSplit = fileName.Split('.');
                        string FileEx = FileSplit[FileSplit.Length - 1].ToString();
                        String guid = Guid.NewGuid().ToString();
                        string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                        string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        string FileName = Convert.ToString(FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
                        string localPath = Server.MapPath("~/App_Data/uploads");
                        if (!System.IO.Directory.Exists(localPath))
                        {
                            System.IO.Directory.CreateDirectory(localPath);
                        }
                        file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName));
                        if (ftpServer.Trim().Length > 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {

                                FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                                requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                                int bufferLength = 2048;
                                byte[] buffer = new byte[file.InputStream.Length];
                                Stream uploadStream = requestFTPUploader.GetRequestStream();
                                int contentLength = file.InputStream.Read(buffer, 0, Convert.ToInt32(file.InputStream.Length));
                                while (contentLength != 0)
                                {
                                    uploadStream.Write(buffer, 0, contentLength);
                                    contentLength = file.InputStream.Read(buffer, 0, bufferLength);
                                }
                                uploadStream.Close();
                                requestFTPUploader = null;
                                issumodel.Attachment = FileName;
                                break;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            try
            {
                if (fc.AllKeys.Contains("Status"))
                {
                    issumodel.Status = fc["Status"].ToString();
                    issumodel.EstimatedHours = Convert.ToDouble(fc["EstimatedHours"].ToString());
                }
                else
                {
                    issumodel.Status = Convert.ToString("26");
                }

                issumodel.IssueId = Convert.ToInt32(fc["IssueId"].ToString());
                if (issumodel.IssueId > 0)
                {
                    issumodel.Comments = fc["Comments"];
                }

                issumodel.Title = fc["Title"].ToString();
                issumodel.Description = fc["Description"].ToString();
                if(issumodel.IssueId == 0)
                {
                    issumodel.DevDescription = "";
                }
                else
                {
                    issumodel.DevDescription = fc["DevDescription"].ToString();
                }                
                issumodel.TicketType = fc["TicketType"].ToString();
                if (issumodel.TicketType.ToString() == "46")
                {
                    issumodel.TicketType = "";
                    issumodel.Status = "46";

                }
                int errcode = issuebl.SaveIssueDetail(issumodel, User_Id, issumodel.Status, issumodel.Comments, issumodel.Attachment, issumodel.TicketType);

                if (errcode == 500001 || errcode == 500002 || errcode != null)
                {
                    return RedirectToAction("Sendissue", new { IssueId = errcode });
                }
                else
                {
                    if (file.ContentLength > 0)
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            try
                            {
                                FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(ftpServer + fileName);
                                requestFileDelete.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                                FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                                requestFileDelete = null;

                                break;
                            }
                            catch (Exception ex)
                            {
                                if (x > 2)
                                {
                                    throw ex;
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
            return View(issumodel);
        }

        public ActionResult Download(int IssueId, string Filename)
        {
            try
            {
                String FTPServer = Common.GetConfigValue("FTP");
                String FTPUserId = Common.GetConfigValue("FTPUid");
                String FTPPwd = Common.GetConfigValue("FTPPWD");

                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + Filename);
                requestFileDownload.UseBinary = true;
                requestFileDownload.KeepAlive = false;
                requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                byte[] OPData = ReadFully(responseStream);
                responseStream.Close();

                Response.AddHeader("content-disposition", "attachment; filename=" + Filename);
                Response.ContentType = "application/zip";
                Response.BinaryWrite(OPData);
                Response.End();
            }
            catch (Exception e)
            {
                Common.LogException(e);
            }
            if (IssueId.ToString() != "0")
            {
                return RedirectToAction("CreateIssue", new { IssueId = IssueId, tabIndex = 0 });
            }
            else
            {
                return RedirectToAction("GetBankChequeList", "BOMAdmin");
            }

        }

        public ActionResult DownloadEmployeeDocuments(int EmpId, string DocumentPath)
        {
            try
            {
                String FTPServer = Common.GetConfigValue("FTP");
                String FTPUserId = Common.GetConfigValue("FTPUid");
                String FTPPwd = Common.GetConfigValue("FTPPWD");

                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + DocumentPath);
                requestFileDownload.UseBinary = true;
                requestFileDownload.KeepAlive = false;
                requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                byte[] OPData = ReadFully(responseStream);
                responseStream.Close();

                Response.AddHeader("content-disposition", "attachment; filename=" + DocumentPath);
                Response.ContentType = "application/zip";
                Response.BinaryWrite(OPData);
                Response.End();
            }
            catch (Exception e)
            {
                Common.LogException(e);
            }
            if (EmpId == 0)
                return RedirectToAction("GetTravelRequestList", "TMLeave");
            else
                return RedirectToAction("EditEmployee", new { EmpId = EmpId });
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

        public ActionResult Sendissue(int IssueId)
        {
            IssueModel Issuemodel = new IssueModel();
            SystemUser user = new SystemUser();
            Issuemodel.LstCommentList = new List<CommentDatamodel>();
            try
            {
                AdminBL issubl = new AdminBL();
                DataSet ds = new DataSet();
                string userID = User.Identity.Name;
                string User_Id = User.Identity.GetUserId();
                SmartSys.BL.AdminBL smartsysBlObj = new SmartSys.BL.AdminBL();
                List<SystemUser> lstSysUserList = smartsysBlObj.GetSysUserList();
                foreach (var item in lstSysUserList)
                {
                    var UserName = item.UserName;
                    if (UserName == userID)
                    {
                        user.Email = item.Email;
                        break;
                    }

                }


                string EmailId = ConfigurationManager.AppSettings["SupportMail"];

                Issuemodel.LstCommentList = issubl.GetSelectedProjectCommentList(IssueId);
                string body = string.Empty;
                body +=
                    "Comment History Detail:" +
                     "<br /> <br />";
                body += "<table border='1'><tr style='background-color:#CEF6F5'><th>CommentDate</th><th>Comments</th><th>Status</th><th>CommentedBy</th></tr>";
                foreach (var item in Issuemodel.LstCommentList)
                {
                    body += "<tr><td>" + item.CommentDate + "</td>";
                    body += "<td>" + item.Comments + "</td>";
                    body += "<td>" + item.Status + "</td>";
                    body += "<td>" + item.CommentedBy + "</td></tr> ";


                }


                if (EmailId.ToString() != "")
                {
                    Issuemodel = issubl.GetSelectedIssueList(IssueId, User_Id);

                    string Subject = Issuemodel.Title.ToString();
                    string MailBody = "<html>" +
                      "<head>" +
                       "<meta http-equiv= " + "Content-Type" + "content= " + "text/html; charset=UTF-8" + ">" +
                       "<meta name=" + "generator" + " content=" + "SautinSoft.RtfToHtml.dll" + ">" +
                       "<title>" + "Leave Mail" + "</title>" +
                       "<style type=" + "text/css>" +
                      ".st1{font-family:Calibri;font-size:11pt;color:#000000;font-weight:bold}" +
                      ".st2{font-family:Calibri;font-size:11pt;color:#000000;}" +
                      "</style>" +
                       "</head>" +
                       "<body>" +
                       "<div>" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " IssueId : " + "</b>" + Issuemodel.IssueId + "</span>" + "</div>" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Title : " + "</b>" + Issuemodel.Title + "</span>" + "</div>" +
                         "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Status : " + "</b>" + Issuemodel.StatusShortCode + "</span>" + "</div>" +
                         "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Description : " + "</b>" + Issuemodel.Description + "</span>" + "</div>" +
                         "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " History : " + "</b>" + body + "</span>" + "</div>" +
                        "</body>" +
                       "</html>";



                    Issuemodel.LstCommentList = issubl.GetSelectedProjectCommentList(IssueId);
                    ArrayList attachments = new ArrayList();
                    Stream responseStream = null;
                    foreach (CommentDatamodel model in Issuemodel.LstCommentList)
                    {
                        if (model.Attachment.ToString() != "")
                        {
                            String FTPServer = Common.GetConfigValue("FTP");
                            String FTPUserId = Common.GetConfigValue("FTPUid");
                            String FTPPwd = Common.GetConfigValue("FTPPWD");

                            FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + model.Attachment);
                            requestFileDownload.UseBinary = true;
                            requestFileDownload.KeepAlive = false;
                            requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                            requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                            FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                            responseStream = responseFileDownload.GetResponseStream();
                            System.Net.Mail.Attachment at = new System.Net.Mail.Attachment(responseStream, model.Attachment);
                            attachments.Add(at);
                        }
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            int mail = Utility.Common.SendMail(Subject, MailBody, "", EmailId, user.Email + Issuemodel.to_MailList, attachments);
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (i == 2)
                            {
                                throw ex;
                            }
                        }
                    }
                    responseStream.Close();

                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return RedirectToAction("CreateIssue", new { IssueId = IssueId, tabIndex = 0 });
        }
        public void SignoffListExport(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            AdminBL objBl = new AdminBL();
            List<IssueModel> LstIssueCompleteList = new List<IssueModel>();
            LstIssueCompleteList = objBl.GetIssueDetailListHaveCompleteStatus(User_Id);
            var DataSource = LstIssueCompleteList;

            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }

        public ActionResult GetIssueListWithCompleteStatus()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/GetIssueListWithCompleteStatus");
            if (FindFlag)
            {
                List<IssueModel> LstIssueCompleteList = new List<IssueModel>();
                string User_Id = User.Identity.GetUserId();
                try
                {
                    AdminBL issuebl = new AdminBL();
                    LstIssueCompleteList = issuebl.GetIssueDetailListHaveCompleteStatus(User_Id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(LstIssueCompleteList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult GetSubordinatePenddingIssueListWith()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/GetIssueListWithCompleteStatus");
            if (FindFlag)
            {
                List<IssueModel> LstIssueCompleteList = new List<IssueModel>();
                string User_Id = User.Identity.GetUserId();
                try
                {
                    AdminBL issuebl = new AdminBL();
                    LstIssueCompleteList = issuebl.GetSubordinateIssueDetailListHavePenddingStatus(User_Id);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(LstIssueCompleteList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportToExcelPendingSubordinateIssue")]
        [AcceptVerbs("POST")]
        public void ExportToExcelPendingSubordinateIssue(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            AdminBL issuebl = new AdminBL();
            var DataSource = issuebl.GetSubordinateIssueDetailListHavePenddingStatus(User_Id);

            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateIssuewithcompleteStatus(int IssueId, int tabIndex)
        {
            IssueModel issumodel = new IssueModel();
            AdminBL issubl = new AdminBL();
            string Status = "";

            issumodel.LstCommentList = new List<CommentDatamodel>();
            if (IssueId > 0)
            {
                string User_Id = User.Identity.GetUserId();
                issumodel = issubl.GetSelectedIssueList(IssueId, User_Id);
                issumodel.LstCommentList = issubl.GetSelectedProjectCommentList(IssueId);
                ViewBag.comments = issumodel.LstCommentList;
            }
            if (issumodel.LstCommentList.Count > 0)
            {
                var last = issumodel.LstCommentList.Last();
                var Id = last.IssueId;
                var Comments = last.Comments;
                issumodel.IssueId = Id;
                issumodel.Comments = "";
                Status = last.Status;
            }
            if (Status.ToString() == "NEW" || IssueId == 0)
            {
                issumodel.Status = Convert.ToString("0");
            }
            else
            {
                issumodel.Status = issumodel.Status;
            }
            return PartialView(issumodel);
        }
        public ActionResult UpdateIssueDetails(int IssueId)
        {
            AdminBL objbl = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            int Ercode = objbl.updateSysIssuesDetails(IssueId, User_Id);
            if (Ercode != 0)
            {
                return RedirectToAction("GetIssueListWithCompleteStatus");
            }
            return RedirectToAction("GetIssueListWithCompleteStatus");
        }
        #endregion Sys_Issue_Details

        #region RoleFeedMapping

        public ActionResult RoleFeedMapping(string strRoleID)
        {
            RoleFeedMapingModel rlfeedModel = null;

            try
            {


                if (Session["RoleRptMappingModel"] == null)
                {
                    Session["RoleFeedMapingModel"] = null;
                }
                if (Session["RoleFeedMapingModel"] == null)
                {
                    rlfeedModel = new RoleFeedMapingModel();
                    AdminBL adminBlObj = new AdminBL();
                    rlfeedModel = adminBlObj.GetFeedListByRole(strRoleID);

                    Session["RoleFeedMapingModel"] = rlfeedModel;
                }
                else
                {
                    rlfeedModel = Session["RoleFeedMapingModel"] as RoleFeedMapingModel;
                }

                ViewBag.Feedlst = new SelectList(rlfeedModel.lstFeed, "FeedID", "FeedName");
                ViewBag.AssignedFeedList = new SelectList(rlfeedModel.lstAssignedFeed, "FeedID", "FeedName");
            }
            catch (Exception ex)
            {

            }
            return View(rlfeedModel);
        }


        [HttpPost]
        public ActionResult SaveRoleFeedMapping(FormCollection frmCollection)
        {
            RoleFeedMapingModel rlfeedmodel = null;
            string RoleID = frmCollection["RoleInfo.RoleID"].ToString();
            List<string> lstSelectedFeedID;
            if (Session["RoleFeedMapingModel"] != null)
            {
                rlfeedmodel = Session["RoleFeedMapingModel"] as RoleFeedMapingModel;
            }
            else
            {
                rlfeedmodel = new RoleFeedMapingModel();
            }
            foreach (string Key in frmCollection.Keys)
            {
                if (Key.ToString() == "lstAssignedFeed")
                {
                    lstSelectedFeedID = frmCollection["lstAssignedFeed"].Split(',').ToList();
                    string strRoleID = frmCollection["RoleInfo.RoleID"].ToString();
                    rlfeedmodel.lstRoleFeedMapList = new List<RoleFeedMapingModel.RoleFeedMapping>();
                    foreach (string str in lstSelectedFeedID)
                    {
                        RoleFeedMapingModel.RoleFeedMapping roleFeedMapping = new RoleFeedMapingModel.RoleFeedMapping();
                        roleFeedMapping.RoleID = strRoleID;
                        roleFeedMapping.FeedID = str;
                        rlfeedmodel.lstRoleFeedMapList.Add(roleFeedMapping);
                    }
                }
            }
            AdminBL adminBlObj = new AdminBL();
            int iErrorCode = adminBlObj.SaveRoleFeedMapping(rlfeedmodel, RoleID);
            Session["RoleFeedMapingModel"] = null;
            return RedirectToAction("SytemRoleList", "Admin");
        }

        #endregion RoleFeedMapping

        #region Country,State and City


        public ActionResult CreateAddressCityStateCountryRef()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/CreateAddressCityStateCountryRef");
            if (FindFlag)
            {
                AdminBL BLObj = new AdminBL();
                ContyStateCityModel addressModel = new ContyStateCityModel();
                try
                {

                    if (Session["Duplication"] != null)
                    {
                        TempData["Message"] = Session["Duplication"] as string;
                    }
                    Session["Duplication"] = null;
                    //addressModel = BLObj.GetCountryStateCityDetails(Countryid, Stateid);
                    addressModel = BLObj.GetCountryStateCityList();
                    List<AddCountryModel> cntrylst = addressModel.LstCountry;
                    ViewBag.cntrys = new SelectList(cntrylst, "CountryId", "CountryName");
                    List<AddRegionModel> rntrylst = addressModel.LstRegion;

                    ViewBag.rntrys = new SelectList(rntrylst, "RegionId", "RegionName");
                    AdminBL adminbl = new AdminBL();
                    //List<ContyStateCityModel> lststate = addressModel.TotalLIst;

                    //ViewBag.statelistwise = new SelectList(lststate, "StateId", "StateName");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(addressModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public JsonResult GetZoneListDetails(int Regionid, int Zoneid)
        {
            ContyStateCityModel addressModel = new ContyStateCityModel();
            List<AddZoneModel> ZoneList = new List<AddZoneModel>();
            try
            {
                AdminBL objbl = new AdminBL();

                addressModel = objbl.GetRegionZoneAreaDetails(Regionid, Zoneid);
                ZoneList = addressModel.LstZone;
                ViewBag.Rntrys = new SelectList(ZoneList, "ZoneId", "ZoneName");


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(new SelectList(ZoneList, "ZoneId", "ZoneName"));
        }
        public JsonResult Getallocatedregion(int Region)
        {
            AdminBL objbl = new AdminBL();
            ContyStateCityModel model = new ContyStateCityModel();
            List<AddStateModel> LstState = new List<AddStateModel>();
            model = objbl.GetListOfStatewise(Region);
            return Json(new SelectList(model.assigned, "StateId", "StateName"));
        }


        public ActionResult AddRegionStateMap()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/admin/AddRegionStateMap");
            if (FindFlag)
            {
                AdminBL objbl = new AdminBL();
                ContyStateCityModel model = new ContyStateCityModel();
                List<AddStateModel> LstState = new List<AddStateModel>();
                model = objbl.GetCountryStateCityList();
                List<AddRegionModel> rntrylst = model.LstRegion;
                ViewBag.region = new SelectList(rntrylst, "RegionId", "RegionName");

                model = objbl.GetListOfStatewise(0);
                ViewBag.TotalList = new SelectList(model.LstState, "StateId", "StateName");

                ViewBag.Allocated = new SelectList(model.AllocatedList, "StateId", "StateName");


                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult AddRegionStateMaplist(string item, int id)
        {


            if (item.ToString() == "")
            {
                return RedirectToAction("AddRegionStateMap");
            }
            RegionStateMapping StateMapDtl = null;

            int RegionId = id;
            string User_Id = User.Identity.GetUserId();

            StateMapDtl = new RegionStateMapping();


            if (item.ToString() != "")
            {
                List<string> LstGetAssignedAreaCity = item.Split(',').ToList();

                // string EmpId = frmCollection["EmpId"];
                List<RegionStateMapping.RegionstateMapdetails> list = new List<RegionStateMapping.RegionstateMapdetails>();
                StateMapDtl.LstRegionAreamapping = list;
                foreach (var Item in LstGetAssignedAreaCity)
                {
                    RegionStateMapping.RegionstateMapdetails StateMapping = new RegionStateMapping.RegionstateMapdetails();
                    StateMapping.StateId = Convert.ToInt32(Item);
                    StateMapping.RegionId = RegionId;
                    list.Add(StateMapping);
                }
            }

            AdminBL adminBlObj = new AdminBL();
            int iErrorCode = adminBlObj.SaveRegionStateMap(StateMapDtl, RegionId, User_Id);

            return RedirectToAction("AddRegionStateMap", "Admin");
        }
        public ActionResult AddZoneStateMap()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/admin/AddZoneStateMap");
            if (FindFlag)
            {
                AdminBL objbl = new AdminBL();
                ContyStateCityModel addressModel = new ContyStateCityModel();
                List<AddStateModel> AllocateStateList = new List<AddStateModel>();
                addressModel = objbl.GetZoneStateList();
                ViewBag.ZoneList = new SelectList(addressModel.LstZone, "ZoneId", "ZoneName");
                ViewBag.StateList = new SelectList(addressModel.LstState, "StateId", "StateName");
                ViewBag.AllocateStateList = new SelectList(AllocateStateList, "StateId", "StateName");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddZoneStateMap(FormCollection fc)
        {
            string User_Id = User.Identity.GetUserId();
            int ErrCode = 0;
            int ZoneId = 0;
            ContyStateCityModel addressModel = new ContyStateCityModel();
            addressModel.LstZone = new List<AddZoneModel>();
            foreach (string Key in fc.Keys)
            {
                if (Key.ToString() == "State")
                {
                    List<string> lstgetselectedCustomer = fc["State"].Split(',').ToList();
                    ZoneId = Convert.ToInt32(fc["ZoneId"]);

                    foreach (var str in lstgetselectedCustomer)
                    {
                        AddZoneModel ZoneList = new AddZoneModel();
                        ZoneList.ZoneId = Convert.ToInt32(ZoneId);
                        ZoneList.StateId = Convert.ToInt32(str);
                        addressModel.LstZone.Add(ZoneList);
                    }
                }
            }
            AdminBL BlObj = new AdminBL();
            ErrCode = BlObj.SaveAddZoneStateMap(addressModel, User_Id, ZoneId);
            return RedirectToAction("AddZoneStateMap");
        }
        public JsonResult GetZoneAllocatedStateList(int Zoneid)
        {
            ContyStateCityModel addressModel = new ContyStateCityModel();
            List<AddStateModel> StateList = new List<AddStateModel>();
            try
            {
                AdminBL objbl = new AdminBL();

                addressModel = objbl.GetZoneAllocatedStateList(Zoneid);
                StateList = addressModel.LstState;
                ViewBag.Rntrys = new SelectList(StateList, "StateId", "StateName");


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(new SelectList(StateList, "StateId", "StateName"));
        }
        public JsonResult GetStateListDetails(int Countryid, int Stateid)
        {
            ContyStateCityModel addressModel = new ContyStateCityModel();
            List<AddStateModel> StateList = new List<AddStateModel>();
            try
            {
                AdminBL objbl = new AdminBL();

                addressModel = objbl.GetCountryStateCityDetails(Countryid, Stateid);
                StateList = addressModel.LstState;
                ViewBag.cntrys = new SelectList(StateList, "StateId", "StateName");


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(new SelectList(StateList, "StateId", "StateName"));
        }

        public ActionResult AddRegionZoneArea(string Region, string Addvalue, string Zone, string Area)
        {
            AdminBL objbl = new AdminBL();
            int Regionid = 0;
            int Zoneid = 0;
            string User_Id = User.Identity.GetUserId();
            if (Addvalue == "Zone")
            {
                Regionid = Convert.ToInt32(Region);
            }
            else
                if (Addvalue == "Area")
            {
                Zoneid = Convert.ToInt32(Zone);
            }
            int errorcode = objbl.SaveRegionZoneAreaDetails(Region, Regionid, Area, Zone, Zoneid, Addvalue, User_Id);
            if (errorcode == 0)
            {
                Session["Duplication"] = "Duplication Not Allowed";
                return RedirectToAction("GetStateListDetails", new { Countryid = 0, Stateid = 0 });
            }
            return View();
        }
        public ActionResult AddCountryStateCity(string Country, string Addvalue, string State, string City)
        {
            AdminBL objbl = new AdminBL();
            int Countryid = 0;
            int Stateid = 0;
            if (Addvalue == "State")
            {
                Countryid = Convert.ToInt32(Country);
            }
            else
                if (Addvalue == "City")
            {
                Stateid = Convert.ToInt32(State);
            }
            int errorcode = objbl.SaveCountryStateCityDetails(Country, Countryid, City, State, Stateid, Addvalue);
            if (errorcode == 0)
            {
                Session["Duplication"] = "Duplication Not Allowed";
                return RedirectToAction("GetStateListDetails", new { Countryid = 0, Stateid = 0 });
            }
            return View();
        }
        public ActionResult RemoveCountryStateCity(int CountryId, string Addvalue)
        {
            int errorcode = 0;
            AddCountryModel objmodel = new AddCountryModel();
            try
            {
                AdminBL adminbl = new AdminBL();

                errorcode = adminbl.DeleteCountryStateCity(CountryId, Addvalue);
                if (errorcode != null)
                {
                    return RedirectToAction("CreateAddressCityStateCountryRef");
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();

        }

        public JsonResult GetAllocatedCity(int AreaId)
        {
            AddRegionModel model = new AddRegionModel();
            AdminBL objbl = new AdminBL();

            model = objbl.GetCityAreamappingList(AreaId);
            List<AddRegionModel> AllocatedList = model.AllocatedList;
            return Json(new SelectList(AllocatedList, "CityId", "City"));
        }
        public ActionResult AddAreaCityMap()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/admin/AddAreaCityMap");
            if (FindFlag)
            {
                AddRegionModel model = new AddRegionModel();
                AdminBL objbl = new AdminBL();

                model = objbl.GetCityAreamappingList(0);
                List<AddRegionModel> TotalList = model.TotalList;
                List<AddRegionModel> AllocatedList = model.AllocatedList;
                List<AddAreaModel> AreaList = model.AreaList;
                ViewBag.TotalList = new SelectList(TotalList, "CityId", "City");
                ViewBag.AllocatedList = new SelectList(AllocatedList, "CityId", "City");
                ViewBag.Area = new SelectList(AreaList, "AreaId", "AreaName");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult AddAreaCityMap(FormCollection frmCollection)
        {

            AreaCityMappingdetail CityAreaDtl = null;

            int AreaId = Convert.ToInt32(frmCollection["AreaId"]);
            string User_Id = User.Identity.GetUserId();
            if (Session["AddAreaCityMap"] != null)
            {
                CityAreaDtl = Session["AddAreaCityMap"] as AreaCityMappingdetail;
            }
            else
            {
                CityAreaDtl = new AreaCityMappingdetail();
            }
            foreach (string Key in frmCollection.Keys)
            {
                if (Key.ToString() == "AssignedCityArea")
                {
                    List<string> LstGetAssignedAreaCity = frmCollection["AssignedCityArea"].Split(',').ToList();

                    // string EmpId = frmCollection["EmpId"];
                    List<AreaCityMappingdetail.AreaCityDetail> list = new List<AreaCityMappingdetail.AreaCityDetail>();
                    CityAreaDtl.LstAreaCitymapping = list;
                    foreach (var Item in LstGetAssignedAreaCity)
                    {
                        AreaCityMappingdetail.AreaCityDetail AreaMapping = new AreaCityMappingdetail.AreaCityDetail();
                        AreaMapping.CityId = Convert.ToInt32(Item);
                        AreaMapping.AreaId = AreaId;
                        list.Add(AreaMapping);
                    }
                }
            }
            AdminBL adminBlObj = new AdminBL();
            int iErrorCode = adminBlObj.SaveAreaCityMapping(CityAreaDtl, AreaId, User_Id);
            Session["AddAreaCityMap"] = null;
            return RedirectToAction("AddAreaCityMap", "Admin");
        }
        #endregion  Country,State and City

        #region Company Libary       
        public ActionResult GetLibraryList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/GetLibraryList");
            if (FindFlag)
            {
                CompanyLibraryModel objmodel = new CompanyLibraryModel();
                List<CompanyLibraryModel> lstlibrary = new List<CompanyLibraryModel>();
                try
                {
                    AdminBL adminbl = new AdminBL();
                    lstlibrary = adminbl.GetListOfCompLibrary(UserId);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(lstlibrary);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportGetLibraryList")]
        [AcceptVerbs("POST")]
        public void ExportGetLibraryList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            AdminBL adminbl = new AdminBL();
            var DataSource = adminbl.GetListOfCompLibrary(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateLibrary(int tabIndex, int DocId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/CreateLibrary?tabIndex=0&DocId=0");
            if (FindFlag)
            {
                CompanyLibraryModel libarymodel = new CompanyLibraryModel();
                List<CompanyLibraryModel> lstdropdown = new List<CompanyLibraryModel>();
                try
                {
                    AdminBL adminbl = new AdminBL();
                    lstdropdown = adminbl.GetListOfCompanyForDropDown();
                    ViewBag.DropdownCompCode = new SelectList(lstdropdown, "CompCode", "CompanyName");
                    if (DocId > 0)
                    {
                        libarymodel = adminbl.GetSelectedLibraryList(DocId);
                    }
                    libarymodel.distributeMail = Convert.ToBoolean(0);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(libarymodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateLibrary(HttpPostedFileBase file, FormCollection fc, int DocId, bool distributeMail)
        {
            CompanyLibraryModel obmodel = new CompanyLibraryModel();
            string User_Id = User.Identity.GetUserId();
            int errorcode;
            string ftpServer = Common.GetConfigValue("FTP");
            string ftpUid = Common.GetConfigValue("FTPUid");
            string ftpPwd = Common.GetConfigValue("FTPPWD");
            string fileName = "";
            obmodel.FileName = fc["FileName"].ToString();

            if (file != null)
            {
                if (obmodel.FileName != null || obmodel.FileName != "")
                {
                    for (int x = 0; x < 3; x++)
                    {
                        try
                        {
                            FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(ftpServer + obmodel.FileName);
                            requestFileDelete.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                            requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                            FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                            requestFileDelete = null;
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (x > 2)
                            {
                                throw ex;
                            }
                        }
                    }

                }
                fileName = System.IO.Path.GetFileName(file.FileName);
                if (file.ContentLength > 0)
                {
                    try
                    {
                        string[] FileSplit = fileName.Split('.');
                        string FileEx = FileSplit[FileSplit.Length - 1].ToString();
                        String guid = Guid.NewGuid().ToString();
                        string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                        string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        string FileName = Convert.ToString("/CompanyLib/" + FileSplit[0].ToString()) + "_" + date + "_" + time + "_" + guid + "." + FileEx;
                        string localPath = Server.MapPath("~/App_Data/uploads");
                        if (!System.IO.Directory.Exists(localPath))
                        {
                            System.IO.Directory.CreateDirectory(localPath);
                        }
                        file.SaveAs(Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName));
                        if (ftpServer.Trim().Length > 0)
                        {
                            for (int i = 0; i < 3; i++)
                            {

                                FtpWebRequest requestFTPUploader = (FtpWebRequest)WebRequest.Create(ftpServer + FileName);
                                requestFTPUploader.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                requestFTPUploader.Method = WebRequestMethods.Ftp.UploadFile;
                                int bufferLength = 2048;
                                byte[] buffer = new byte[file.InputStream.Length];
                                Stream uploadStream = requestFTPUploader.GetRequestStream();
                                int contentLength = file.InputStream.Read(buffer, 0, Convert.ToInt32(file.InputStream.Length));
                                while (contentLength != 0)
                                {
                                    uploadStream.Write(buffer, 0, contentLength);
                                    contentLength = file.InputStream.Read(buffer, 0, bufferLength);
                                }
                                uploadStream.Close();
                                requestFTPUploader = null;
                                obmodel.FileName = FileName;
                                break;

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            try
            {
                obmodel.DocId = 0;
                if (DocId > 0)
                {
                    obmodel.DocId = DocId;
                }
                obmodel.DocName = fc["DocName"].ToString();
                obmodel.CompCode = fc["CompCode"].ToString();
                obmodel.Description = fc["Description"].ToString();
                Session["distributeMail"] = null;
                obmodel.distributeMail = distributeMail;
                Session["distributeMail"] = obmodel.distributeMail;
                AdminBL adminbl = new AdminBL();
                errorcode = adminbl.SaveLibraryDetail(obmodel, User_Id, obmodel.FileName);
                if (errorcode == 500001 || errorcode == 500002 || errorcode != null)
                {
                    return RedirectToAction("SendLibraryDetail", new { DocId = errorcode });
                }
                else
                {
                    if (file.ContentLength > 0)
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            try
                            {
                                FtpWebRequest requestFileDelete = (FtpWebRequest)WebRequest.Create(ftpServer + fileName);
                                requestFileDelete.Credentials = new NetworkCredential(ftpUid, ftpPwd);
                                requestFileDelete.Method = WebRequestMethods.Ftp.DeleteFile;
                                FtpWebResponse responseFileDelete = (FtpWebResponse)requestFileDelete.GetResponse();
                                requestFileDelete = null;

                                break;
                            }
                            catch (Exception ex)
                            {
                                if (x > 2)
                                {
                                    throw ex;
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
            return View(obmodel);
        }

        private static byte[] ReadFullyFile(Stream input)
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

        public ActionResult DownloadFile(string Filename, int DocId)
        {
            try
            {
                String FTPServer = Common.GetConfigValue("FTP");
                String FTPUserId = Common.GetConfigValue("FTPUid");
                String FTPPwd = Common.GetConfigValue("FTPPWD");

                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + Filename);
                requestFileDownload.UseBinary = true;
                requestFileDownload.KeepAlive = false;
                requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                Stream responseStream = responseFileDownload.GetResponseStream();
                byte[] OPData = ReadFully(responseStream);
                responseStream.Close();

                Response.AddHeader("content-disposition", "attachment; filename=" + Filename);
                Response.ContentType = "application/zip";
                Response.BinaryWrite(OPData);
                Response.End();
            }
            catch (Exception e)
            {
                Common.LogException(e);
            }

            return RedirectToAction("CreateLibrary", new { DocId = DocId, tabIndex = 0 });
        }
        public ActionResult SendLibraryDetail(int DocId)
        {
            string User_Id = User.Identity.GetUserId();
            Boolean distributeMail = (Boolean)Session["distributeMail"];
            if (distributeMail == false)
            {
                return RedirectToAction("GetLibraryList");
            }

            try
            {
                CompanyLibraryModel libarymodel = new CompanyLibraryModel();
                AdminBL adminbl = new AdminBL();
                libarymodel = adminbl.GetSelectedLibraryList(DocId);
                var Title = libarymodel.DocName;
                int Docid = libarymodel.DocId;
                string CompanyName = libarymodel.CompanyName;
                string ModifiedBy = libarymodel.ModifiedBy;
                string Description = libarymodel.Description;
                var Date = libarymodel.ModifiedDate;
                string ToMail = libarymodel.Email;

                string CCMail = ConfigurationManager.AppSettings["SupportMail"];

                if (ToMail.ToString() != "")
                {

                    string MailBody = "<html>" +
                      "<head>" +
                       "<meta http-equiv= " + "Content-Type" + "content= " + "text/html; charset=UTF-8" + ">" +
                       "<meta name=" + "generator" + " content=" + "SautinSoft.RtfToHtml.dll" + ">" +
                       "<title>" + "Leave Mail" + "</title>" +
                       "<style type=" + "text/css>" +
                      ".st1{font-family:Calibri;font-size:11pt;color:#000000;font-weight:bold}" +
                      ".st2{font-family:Calibri;font-size:11pt;color:#000000;}" +
                      "</style>" +
                       "</head>" +
                       "<body>" +
                       "<div>" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Document Id : " + "</b>" + Docid + "</span>" + "</div>" +
                       "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Title : " + "</b>" + Title + "</span>" + "</div>" +
                         "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Company Name : " + "</b>" + CompanyName + "</span>" + "</div>" +
                         "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + " Modified By : " + "</b>" + ModifiedBy + "</span>" + "</div>" +
                          "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + "Modified Date : " + "</b>" + Date + "</span>" + "</div>" +
                          "<div style=" + "margin:0pt 0pt 10pt 0pt;line-height:1.15;>" + "<span class=st1>" + "<b style='color:red'>" + "Description : " + "</b>" + Description + "</span>" + "</div>" +

                        "</body>" +
                       "</html>";


                    List<CompanyLibraryModel> lstlibrary = new List<CompanyLibraryModel>();
                    lstlibrary = adminbl.GetListOfCompLibrary(User_Id);


                    ArrayList attachments = new ArrayList();
                    Stream responseStream = null;
                    foreach (CompanyLibraryModel model in lstlibrary)
                    {
                        if (model.FileName != "")
                        {
                            if (model.DocId == DocId)
                            {
                                String FTPServer = Common.GetConfigValue("FTP");
                                String FTPUserId = Common.GetConfigValue("FTPUid");
                                String FTPPwd = Common.GetConfigValue("FTPPWD");

                                FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FTPServer + model.FileName);
                                requestFileDownload.UseBinary = true;
                                requestFileDownload.KeepAlive = false;
                                requestFileDownload.Credentials = new NetworkCredential(FTPUserId, FTPPwd);
                                requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                                FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
                                responseStream = responseFileDownload.GetResponseStream();
                                System.Net.Mail.Attachment at = new System.Net.Mail.Attachment(responseStream, model.FileName);
                                attachments.Add(at);
                            }
                        }
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            int mail = Utility.Common.SendMail(Title, MailBody, "", ToMail, CCMail, attachments);
                            break;
                        }
                        catch (Exception ex)
                        {
                            if (i == 2)
                            {
                                throw ex;
                            }
                        }
                    }
                    responseStream.Close();

                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return RedirectToAction("GetLibraryList");
        }
        #endregion Company Libary      

        #region CustomerBYEmployee
        public ActionResult EmployeeCustomerDetails(int EmpId)
        {
            EmployeeCustomerDetail EmpCustDtl = null;
            try
            {

                List<EmployeeModel> lstEmp = new List<EmployeeModel>();
                AdminBL objBL = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                lstEmp = objBL.GetEmployeeList(User_Id);
                foreach (var Emp in lstEmp)
                {
                    int empid = Emp.EmpId;
                    if (empid == EmpId)
                    {
                        EmployeeCustomerDetail.DerivedCustomer emp = new EmployeeCustomerDetail.DerivedCustomer();
                        string EmpName = Emp.EmpName;
                        ViewBag.EmpName = EmpName;
                        ViewBag.EmpId = EmpId;
                        break;
                    }

                }
                Session["EmployeeCustomerDetail"] = null;
                if (Session["EmployeeCustomerDetail"] == null)
                {
                    EmpCustDtl = new EmployeeCustomerDetail();
                    AdminBL adminBlObj = new AdminBL();
                    EmpCustDtl = adminBlObj.GetCustomerByEmployee(EmpId);

                    Session["EmployeeCustomerDetail"] = EmpCustDtl;
                }
                else
                {
                    EmpCustDtl = Session["EmployeeCustomerDetail"] as EmployeeCustomerDetail;
                }

                foreach (EmployeeCustomerDetail.DerivedCustomer demo in EmpCustDtl.lstAssignedCustomer)
                {
                    var actionToDelete = from Customer in EmpCustDtl.lstCustomer
                                         where Customer.CustomerId == demo.CustomerId
                                         select Customer;
                    EmpCustDtl.lstCustomer.Remove(actionToDelete.ElementAt(0));
                }
                ViewBag.CustomerList = new SelectList(EmpCustDtl.lstCustomer, "CustomerId", "CustomerName");
                ViewBag.AssignedCustomer = new SelectList(EmpCustDtl.lstAssignedCustomer, "CustomerId", "CustomerName");
                EmployeeModel EmpModel = new EmployeeModel();
                EmpModel.DeptList = new List<Departmentmodel>();
                AdminBL objbl = new AdminBL();
                EmpModel.DeptList = objbl.GetEmployeeDept(EmpId);
                ViewBag.SysAssignDeptList = new SelectList(EmpModel.DeptList, "DeptId", "DeptName");
            }
            catch (Exception ex)
            {

            }
            return PartialView(EmpCustDtl);
        }
        [HttpPost]
        public ActionResult EmployeeCustomerDetails(FormCollection frmCollection)
        {
            EmployeeCustomerDetail EmpCustDtl = null;
            int EmpId = Convert.ToInt32(frmCollection["EmpId"]);
            int DeptId = Convert.ToInt32(frmCollection["AssignDeptId"]);
            string User_Id = User.Identity.GetUserId();
            if (Session["EmployeeCustomerDetail"] != null)
            {
                EmpCustDtl = Session["EmployeeCustomerDetail"] as EmployeeCustomerDetail;
            }
            else
            {
                EmpCustDtl = new EmployeeCustomerDetail();
            }
            foreach (string Key in frmCollection.Keys)
            {
                if (Key.ToString() == "lstAssignedCustomer")
                {
                    List<string> lstgetselectedCustomer = frmCollection["lstAssignedCustomer"].Split(',').ToList();

                    // string EmpId = frmCollection["EmpId"];

                    EmpCustDtl.lstEmpCustomerMapping = new List<EmployeeCustomerDetail.EmpCustomerMapping>();
                    foreach (var str in lstgetselectedCustomer)
                    {
                        EmployeeCustomerDetail.EmpCustomerMapping Customermapping = new EmployeeCustomerDetail.EmpCustomerMapping();
                        Customermapping.CustomerId = Convert.ToInt32(str);
                        Customermapping.EmpId = EmpId;
                        EmpCustDtl.lstEmpCustomerMapping.Add(Customermapping);
                    }
                }
            }
            AdminBL adminBlObj = new AdminBL();
            int iErrorCode = adminBlObj.SaveEmployeeCustomerMapping(EmpCustDtl, EmpId, User_Id, DeptId);
            Session["EmployeeCustomerDetail"] = null;
            return RedirectToAction("GetEmployeeList", "Admin");

        }

        public ActionResult GetAssignCustByDept(int EmpId, int DeptId)
        {
            List<EmpCustomerMapping> LstModel = new List<EmpCustomerMapping>();
            AdminBL adminBlObj = new AdminBL();
            try
            {
                LstModel = adminBlObj.GetEmpCustbyDept(EmpId, DeptId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Json(new SelectList(LstModel, "CustomerId", "CustomerName"));
        }
        #endregion CustomerBYEmployee

        #region   VendorByEmployee
        public ActionResult EmployeeVendorDetails(int EmpId)
        {
            EmployeeVendorDetail EmpVndrtDtl = null;
            try
            {

                List<EmployeeModel> lstEmp = new List<EmployeeModel>();
                AdminBL objBL = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                lstEmp = objBL.GetEmployeeList(User_Id);
                foreach (var Emp in lstEmp)
                {
                    int empid = Emp.EmpId;
                    if (empid == EmpId)
                    {
                        EmployeeVendorDetail.DerivedVendor emp = new EmployeeVendorDetail.DerivedVendor();
                        string EmpName = Emp.EmpName;
                        ViewBag.EmpName = EmpName;
                        ViewBag.EmpId = EmpId;
                        break;
                    }

                }
                Session["EmployeeVendorDetail"] = null;
                if (Session["EmployeeVendorDetail"] == null)
                {
                    EmpVndrtDtl = new EmployeeVendorDetail();
                    AdminBL adminBlObj = new AdminBL();
                    EmpVndrtDtl = adminBlObj.GetVendorByEmployee(EmpId);

                    Session["EmployeeVendorDetail"] = EmpVndrtDtl;
                }
                else
                {
                    EmpVndrtDtl = Session["EmployeeVendorDetail"] as EmployeeVendorDetail;
                }
                foreach (EmployeeVendorDetail.DerivedVendor demo in EmpVndrtDtl.lstAssignedVendor)
                {
                    var actionToDelete = from Vendor in EmpVndrtDtl.lstVendor
                                         where Vendor.VendorId == demo.VendorId
                                         select Vendor;
                    EmpVndrtDtl.lstVendor.Remove(actionToDelete.ElementAt(0));
                }
                ViewBag.VendorList = new SelectList(EmpVndrtDtl.lstVendor, "VendorId", "VendorName");
                ViewBag.AssignedVendor = new SelectList(EmpVndrtDtl.lstAssignedVendor, "VendorId", "VendorName");
            }
            catch (Exception ex)
            {

            }
            return PartialView(EmpVndrtDtl);
        }
        [HttpPost]
        public ActionResult EmployeeVendorDetails(FormCollection frmCollection)
        {
            EmployeeVendorDetail EmpVndrDtl = null;
            int EmpId = Convert.ToInt32(frmCollection["EmpId"]);
            string User_Id = User.Identity.GetUserId();
            if (Session["EmployeeVendorDetail"] != null)
            {
                EmpVndrDtl = Session["EmployeeVendorDetail"] as EmployeeVendorDetail;
            }
            else
            {
                EmpVndrDtl = new EmployeeVendorDetail();
            }
            foreach (string Key in frmCollection.Keys)
            {
                if (Key.ToString() == "lstAssignedVendor")
                {
                    List<string> lstgetselectedVendor = frmCollection["lstAssignedVendor"].Split(',').ToList();

                    // string EmpId = frmCollection["EmpId"];

                    EmpVndrDtl.lstEmpVendorMapping = new List<EmployeeVendorDetail.EmpVendorMapping>();
                    foreach (var str in lstgetselectedVendor)
                    {
                        EmployeeVendorDetail.EmpVendorMapping Vendormapping = new EmployeeVendorDetail.EmpVendorMapping();
                        Vendormapping.VendorId = Convert.ToInt32(str);
                        Vendormapping.EmpId = EmpId;
                        EmpVndrDtl.lstEmpVendorMapping.Add(Vendormapping);
                    }
                }
            }
            AdminBL adminBlObj = new AdminBL();
            int iErrorCode = adminBlObj.SaveEmployeeVendorMapping(EmpVndrDtl, EmpId, User_Id);
            Session["EmployeeVendorDetail"] = null;
            return RedirectToAction("GetEmployeeList", "Admin");

        }
        #endregion  VendorByEmployee

        #region Assets
        public ActionResult SysAssetsList()
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/SysAssetsList");
            if (FindFlag)
            {
                List<SysAssetsModel> lstSysAssets = new List<SysAssetsModel>();
                try
                {
                    AdminBL adminbl = new AdminBL();
                    lstSysAssets = adminbl.SysAssetsList(UserId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(lstSysAssets);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult CreateSysAssets(int AssetId)
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/CreateSysAssets?AssetId=0");
            if (FindFlag)
            {
                SysAssetsModel objmodel = new SysAssetsModel();
                AdminBL objbl = new AdminBL();
                try
                {
                    if (AssetId == 0)
                    {
                        objmodel.AssetId = AssetId;
                    }
                    if (AssetId > 0)
                    {
                        objmodel = objbl.GetSelectedSysAssetList(AssetId);
                    }

                    List<SyaAssetTypeModel> AssetTypeList = new List<SyaAssetTypeModel>();
                    AssetTypeList = objbl.DropdownSysAssetType(UserId);
                    ViewBag.SysAssetTypeListtttt = new SelectList(AssetTypeList, "AssetTypeId", "AssetType");

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(objmodel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult CreateSysAssets(FormCollection fc, HttpPostedFileBase uploadFile)     // bool isPrimary
        {
            SysAssetsModel objmodel = new SysAssetsModel();
            try
            {
                string User_Id = User.Identity.GetUserId();
                objmodel.AssetId = Convert.ToInt32(fc["AssetId"].ToString());

                if (objmodel.AssetId > 0)
                {
                    objmodel.AssetId = Convert.ToInt32(fc["AssetId"]);
                }
                else
                {
                    objmodel.AssetId = 0;
                }

                objmodel.AssetTypeId = Convert.ToInt32(fc["AssetTypeId"]);

                objmodel.AssetName = fc["AssetName"].ToString();

                objmodel.Description = fc["Description"].ToString();

                if (fc["AssetInDate"].ToString() != "")
                {
                    objmodel.AssetInDate = Convert.ToDateTime(fc["AssetInDate"].ToString());
                }
                else
                {
                    objmodel.AssetInDate = null;
                }

                objmodel.AssetAcRef = fc["AssetAcRef"].ToString();
                if (Convert.ToDouble(fc["DepRate"].ToString()) != 0.0)
                {
                    objmodel.DepRate = Convert.ToDouble(fc["DepRate"].ToString());
                }

                if (Convert.ToDouble(fc["Cost"].ToString()) != 0.0)
                {
                    objmodel.Cost = Convert.ToDouble(fc["Cost"].ToString());
                }
                objmodel.TotalQty = Convert.ToDouble(fc["TotalQty"].ToString());
                if (fc["OfficeLocationId"].ToString() != "")
                {
                    objmodel.OfficeLocationId = Convert.ToInt32(fc["OfficeLocationId"].ToString());
                }
                else
                {
                    objmodel.OfficeLocationId = null;
                }
                if (fc["LocationId"].ToString() != "")
                {
                    objmodel.LocationId = Convert.ToInt32(fc["LocationId"].ToString());
                }
                else
                {
                    objmodel.LocationId = null;
                }
                objmodel.ManufacturerDetails = fc["ManufacturerDetails"].ToString();
                if (fc["DisposalDate"].ToString() != "")
                {
                    objmodel.DisposalDate = Convert.ToDateTime(fc["DisposalDate"].ToString());
                }
                else
                {
                    objmodel.DisposalDate = null;
                }


                objmodel.DisposalValue = Convert.ToDouble(fc["DisposalValue"].ToString());

                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    objmodel.Photo = new byte[uploadFile.ContentLength];
                    uploadFile.InputStream.Read(objmodel.Photo, 0, uploadFile.ContentLength);
                }
                else
                {
                    objmodel.Photo = null;
                }




                AdminBL objbl = new AdminBL();
                int errorcode = objbl.SaveSysAsset(objmodel, User_Id);

                if (errorcode == 500001 || errorcode == 500002)
                {
                    return RedirectToAction("SysAssetsList");
                }
            }


            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("SysAssetsList");
        }


        public ActionResult DeleteAssetDetails(int AssetId)
        {
            try
            {
                int errcode = 0;
                AdminBL objbl = new AdminBL();
                string User_Id = User.Identity.GetUserId();
                errcode = objbl.DeleteSysAssetDetails(AssetId, User_Id);
                if (errcode == AssetId)
                {
                    return RedirectToAction("SysAssetsList");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("SysAssetsList");
        }
        public ActionResult AssetTypeList()
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/AssetTypeList");
            if (FindFlag)
            {
                AdminBL objbl = new AdminBL();
                List<SyaAssetTypeModel> AssetTypeList = new List<SyaAssetTypeModel>();
                AssetTypeList = objbl.DropdownSysAssetType(UserId);
                return View(AssetTypeList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportAssetTypeLst")]
        [AcceptVerbs("POST")]
        public void ExportAssetTypeLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            AdminBL objbl = new AdminBL();
            var DataSource = objbl.DropdownSysAssetType(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateAssetType(int AstTypeId)
        {
            SyaAssetTypeModel model = new SyaAssetTypeModel();
            AdminBL objbl = new AdminBL();
            string User_Id = User.Identity.GetUserId();

            if (AstTypeId != 0)
            {
                model = objbl.GetSelectedSysAssetType(AstTypeId);
            }

            List<SyaAssetTypeModel> list1 = new List<SyaAssetTypeModel>();
            list1 = objbl.DropdownSysAssetType(User_Id);
            var item = list1.Find(m => m.AssetTypeId == model.AssetTypeId);
            list1.Remove(item);
            ViewBag.parentAssetType = new SelectList(list1, "ParentAssetTypeId", "AssetType");  //AssetTypeId

            return View(model);
        }
        [HttpPost]
        public ActionResult CreateAssetType(FormCollection fc)
        {
            SyaAssetTypeModel model = new SyaAssetTypeModel();
            AdminBL objbl = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            if (fc.AllKeys.Contains("AssetTypeId"))
            {
                model.AssetTypeId = Convert.ToInt32(fc["AssetTypeId"].ToString());
            }
            else
            {
                model.AssetTypeId = 0;
            }
            model.ParentAssetTypeId = Convert.ToInt32(fc["ParentAssetTypeId"].ToString());
            model.AssetType = fc["AssetType"].ToString();
            model.Description = fc["Description"].ToString();

            int errorcode = objbl.SaveAssetType(model, User_Id);

            if (errorcode == 500001 || errorcode == 500002)
            {
                return RedirectToAction("AssetTypeList");
            }
            return View();
        }
        public void ExportToExcel(string GridModel)
        {
            string user_Id = User.Identity.GetUserId();
            AdminBL BLObj = new AdminBL();
            var DataSource = BLObj.SysAssetsList(user_Id);
            ExcelExport exp = new ExcelExport();
            GridProperties obj = ConvertGridObject(GridModel);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }

        private GridProperties ConvertGridObject(string gridProperty)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            IEnumerable div = (IEnumerable)serializer.Deserialize(gridProperty, typeof(IEnumerable));
            GridProperties gridProp = new GridProperties();
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

        public ActionResult AssetAllocationList()

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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/AssetAllocationList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                SysAssetAllocationModel model = new SysAssetAllocationModel();
                AdminBL objbl = new AdminBL();

                model = objbl.SysAssetAllocationType(User_Id);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpGet]
        public ActionResult CreateSysAssetAllocation(int AssignmentId)
        {
            try
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
                FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/CreateSysAssetAllocation?AssignmentId=0");
                if (FindFlag)
                {
                    AdminBL objbl = new AdminBL();
                    SysAssetAllocationModel model = new SysAssetAllocationModel();

                    model.AssignmentId = 0;
                    if (AssignmentId != 0)
                    {
                        model = objbl.GetSelectedSysAssetAllocation(AssignmentId);

                        List<SysAssetsModel> AssetList = new List<SysAssetsModel>();
                        AssetList = objbl.SysAssetsListDrpDwnEdit(AssignmentId, UserId);
                        ViewBag.assetList = new SelectList(AssetList, "AssetId", "AssetName");
                    }


                    List<EmployeeModel> lstEmp = new List<EmployeeModel>();

                    lstEmp = objbl.GetEmployeeList(UserId);
                    ViewBag.EmpDropDowmn = new SelectList(lstEmp, "Empid", "EmpName");

                    if (AssignmentId == 0)
                    {
                        List<SysAssetsModel> AssetList123 = new List<SysAssetsModel>();
                        AssetList123 = objbl.SysAssetsListDrpDwn(UserId);
                        ViewBag.assetList = new SelectList(AssetList123, "AssetId", "AssetName");

                    }


                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }


        }

        [HttpPost]
        public ActionResult CreateSysAssetAllocation(FormCollection fc)
        {
            try
            {
                string User_Id = User.Identity.GetUserId();
                SysAssetAllocationModel objmodel = new SysAssetAllocationModel();
                AdminBL objbl = new AdminBL();

                objmodel.AssignmentId = Convert.ToInt32(fc["AssignmentId"].ToString());
                if (objmodel.AssignmentId > 0)
                {
                    objmodel.AssignmentId = Convert.ToInt32(fc["AssignmentId"]);
                }
                else
                {
                    objmodel.AssignmentId = 0;
                }
                objmodel.AssetId = Convert.ToInt32(fc["AssetId"].ToString());
                objmodel.AssigntoEmpId = Convert.ToInt32(fc["AssigntoEmpId"].ToString());
                objmodel.AssignedType = fc["AssignedType"].ToString();
                objmodel.AssignedDate = Convert.ToDateTime(fc["AssignedDate"].ToString());

                if (fc["ReturnDate"].ToString() != "")
                {
                    objmodel.ReturnDate = Convert.ToDateTime(fc["ReturnDate"].ToString());
                }
                else
                {
                    objmodel.ReturnDate = null;
                }



                int errcode = 0;
                errcode = objbl.SaveSysAssetAllocation(objmodel, User_Id);

                if (errcode == 500001)
                {
                    return RedirectToAction("CreateSysAssetAllocation", new { AssignmentId = objmodel.AssignmentId });
                }
                else
                {
                    return RedirectToAction("AssetAllocationList");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public ActionResult ApproveAssetAllocatioList()
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/ApproveAssetAllocatioList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                SysAssetAllocationModel model = new SysAssetAllocationModel();
                try
                {
                    AdminBL objbl = new AdminBL();


                    model = objbl.SysAssetAllocationType(User_Id);


                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [System.Web.Http.ActionName("ExportAssetAprvLst")]
        [AcceptVerbs("POST")]
        public void ExportAssetAprvLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            AdminBL objbl = new AdminBL();
            var DataSource = objbl.SysAssetAllocationType(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult ApproveAssetAllocation(int AssignmentId)
        {

            AdminBL objbl = new AdminBL();

            string User_Id = User.Identity.GetUserId();
            SysAssetAllocationModel model = new SysAssetAllocationModel();
            model = objbl.GetSelectedSysAssetAllocation(AssignmentId);

            List<SysAssetsModel> AssetList = new List<SysAssetsModel>();
            AssetList = objbl.SysAssetsListDrpDwnEdit(AssignmentId, User_Id);
            ViewBag.assetList = new SelectList(AssetList, "AssetId", "AssetName");

            List<EmployeeModel> lstEmp = new List<EmployeeModel>();

            lstEmp = objbl.GetEmployeeList(User_Id);
            ViewBag.EmpDropDowmn = new SelectList(lstEmp, "Empid", "EmpName");

            return View(model);

        }

        public ActionResult UpdateAssetApproval(int AssignmentId, int Status)
        {
            string User_Id = User.Identity.GetUserId();
            AdminBL objbl = new AdminBL();
            int errcode = 0;
            errcode = objbl.UpdateAssetApproval(AssignmentId, User_Id, Status);
            return RedirectToAction("ApproveAssetAllocatioList");
        }


        #endregion Assets

        #region Segement
        public ActionResult GetItemSegmentList()
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/GetItemSegmentList");
            if (FindFlag)
            {
                List<ItemSegment> SegmentLIst = new List<ItemSegment>();
                AdminBL objbl = new AdminBL();
                SegmentLIst = objbl.GetItemSegmentList(UserId);
                return View(SegmentLIst);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportSegmentLst")]
        [AcceptVerbs("POST")]
        public void ExportSegmentLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            AdminBL objbl = new AdminBL();
            var DataSource = objbl.GetItemSegmentList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateItemSegment(int SegmentId)
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/CreateItemSegment?SegmentId=0");
            if (FindFlag)
            {
                AdminBL objbl = new AdminBL();
                ItemSegment itemsegment = new ItemSegment();
                List<ItemSegment> SegmentLIst = new List<ItemSegment>();
                if (SegmentId > 0)
                {

                    SegmentLIst = objbl.GetdropdownForsegment(SegmentId); ViewBag.ItemSegment = new SelectList(SegmentLIst, "SegmentId", "SegmentName");
                    itemsegment = objbl.GetselectedItemSegment(SegmentId);
                    List<EquipmentSegmentgrdmodel> EquipmentGrd = objbl.Eqipmentsegmentgrdlist(SegmentId);
                    ViewBag.EquipmentGrd = EquipmentGrd;
                    List<EquipmentSegmentmodel> EquipmentSegmentdrp = objbl.EqipmentsegmentDrplist(SegmentId);
                    ViewBag.EquipmentSegment = new SelectList(EquipmentSegmentdrp, "EquipmentId", "EquipmentName");
                }
                if (SegmentId == 0)
                {
                    SegmentLIst = objbl.GetItemSegmentList(UserId);
                    ViewBag.ItemSegment = new SelectList(SegmentLIst, "SegmentId", "SegmentName");
                }


                return View(itemsegment);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult SaveEquipmentForSegment(int SegmentId, int EquipmentId)
        {
            AdminBL objbl = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            int errorcode = objbl.SaveEquipmentforsegment(SegmentId, EquipmentId, User_Id);

            return RedirectToAction("CreateItemSegment", new { SegmentId = SegmentId });
        }
        [HttpPost]
        public ActionResult CreateItemSegment(FormCollection fc)
        {
            ItemSegment itemsegment = new ItemSegment();
            AdminBL objbl = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                if (fc["SegmentId"].ToString() != "0")
                {
                    itemsegment.SegmentId = Convert.ToInt32(fc["SegmentId"].ToString());
                }
                else
                {
                    itemsegment.SegmentId = 0;
                }
                if (fc["ParentSegmentId"].ToString() == "")
                {
                    itemsegment.ParentSegmentId = 0;
                }
                else
                {
                    itemsegment.ParentSegmentId = Convert.ToInt32(fc["ParentSegmentId"].ToString());

                }
                itemsegment.SegmentName = fc["SegmentName"].ToString();
                itemsegment.Description = fc["Description"].ToString();
                int errorcode = objbl.SaveItemSegmentDetails(itemsegment, User_Id);
                if (errorcode == 500001 || errorcode == 500002)
                {
                    return RedirectToAction("GetItemSegmentList");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }

        #endregion Segment

        #region Itemcategory
        public ActionResult GetItemCategoryList()
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/GetItemCategoryList");
            if (FindFlag)
            {
                List<BL.ItemCategory> CategoryList = new List<BL.ItemCategory>();
                AdminBL objbl = new AdminBL();
                CategoryList = objbl.GetItemCategoryList(UserId);
                ViewBag.SelectedCategoryItems = CategoryList;
                return View(CategoryList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult CreateItemCategory(int CategoryId)
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
            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/CreateItemCategory?CategoryId=0");
            if (FindFlag)
            {
                AdminBL objbl = new AdminBL();
                BL.ItemCategory itemcategory = new BL.ItemCategory();
                List<BL.ItemCategory> CategoryList = new List<BL.ItemCategory>();
                if (CategoryId > 0)
                {
                    CategoryList = objbl.GetDropdownForItemCategory(CategoryId);
                    ViewBag.ItemCategory = new SelectList(CategoryList, "CategoryId", "Category");
                    itemcategory = objbl.GetselectedItemCategory(CategoryId);
                }

                if (CategoryId == 0)
                {
                    CategoryList = objbl.GetItemCategoryList(UserId);
                    ViewBag.ItemCategory = new SelectList(CategoryList, "CategoryId", "Category");
                }
                return View(itemcategory);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateItemCategory(FormCollection fc)
        {
            BL.ItemCategory itemcategory = new BL.ItemCategory();
            AdminBL objbl = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                if (fc["CategoryId"].ToString() != "0")
                {
                    itemcategory.CategoryId = Convert.ToInt32(fc["CategoryId"].ToString());
                }
                else
                {
                    itemcategory.CategoryId = 0;
                }
                if (fc["ParentCategoryId"].ToString() == "")
                {
                    itemcategory.ParentCategoryId = 0;
                }
                else
                {
                    itemcategory.ParentCategoryId = Convert.ToInt32(fc["ParentCategoryId"].ToString());

                }

                itemcategory.Category = fc["Category"].ToString();
                itemcategory.Description = fc["Description"].ToString();
                int errorcode = objbl.SaveItemcategoryDetails(itemcategory, User_Id);
                if (errorcode == 500001 || errorcode == 500002)
                {
                    return RedirectToAction("GetItemCategoryList");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        #endregion ItemCategory

        #region System Mail Config 
        public ActionResult GetSystemEmailConfigList()
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/GetSystemEmailConfigList");
            if (FindFlag)
            {
                List<SystemEmailConfig> SystemEmailList = new List<SystemEmailConfig>();
                AdminBL objbl = new AdminBL();
                SystemEmailList = objbl.GetSystemEmailConfigList();
                ViewBag.SelectedCategoryItems = SystemEmailList;
                return View(SystemEmailList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportSystemEmailConfigLst")]
        [AcceptVerbs("POST")]
        public void ExportSystemEmailConfigLst(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            AdminBL objbl = new AdminBL();
            var DataSource = objbl.GetSystemEmailConfigList();
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateSystemEmailConfig(int MailTemplateId)
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

            FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/CreateSystemEmailConfig?MailTemplateId=0");
            if (FindFlag)
            {
                AdminBL objbl = new AdminBL();
                SystemEmailConfig SystemEmailModel = new SystemEmailConfig();
                if (MailTemplateId > 0)
                {
                    SystemEmailModel = objbl.GetSelectedSystemEmailConfigList(MailTemplateId);
                    //    SystemEmailModel.MailContent = SystemEmailModel.MailContent.Replace("{enquiry_num}", "10/TempTtemby/845632454555");
                }
                List<DocumentTypeModel> DocListModel = new List<DocumentTypeModel>();
                DocListModel = objbl.GetDocumentTypeforDropDwnList(SystemEmailModel.DocumentType);
                ViewBag.DocTypeLst = new SelectList(DocListModel, "DocumentType", "Description");
                return View(SystemEmailModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult CreateSystemEmailConfig(FormCollection fc)
        {
            SystemEmailConfig SystemEmailModel = new SystemEmailConfig();
            AdminBL objbl = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            try
            {
                SystemEmailModel.MailTemplateId = Convert.ToInt32(fc["MailTemplateId"].ToString());
                SystemEmailModel.TemplateName = fc["TemplateName"].ToString();
                SystemEmailModel.MailContent = fc["MailContent"].ToString();
                SystemEmailModel.DocumentType = fc["DocumentType"].ToString();
                int errorcode = objbl.SaveSystemEmailConfigDetails(SystemEmailModel, User_Id);
                if (errorcode == 500001 || errorcode == 500002)
                {
                    return RedirectToAction("GetSystemEmailConfigList");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View();
        }
        #endregion System Mail Config

        #region MigrateFiles
        public ActionResult MigrateFile()
        {
            DirectoryInfo sourceinfo = new DirectoryInfo(@"C:\_Source");
            DirectoryInfo target = new DirectoryInfo(@"C:\_Destination");
            if (!Directory.Exists(target.FullName))
            {
                Directory.CreateDirectory(target.FullName);
            }
            foreach (FileInfo fi in sourceinfo.GetFiles())
            {
                if (fi.Length != 0)
                    fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
                fi.Delete();
            }
            return View();
        }
        #endregion MigrateFile

        #region StockLocation
        public ActionResult StockLocationList()
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
            bool FindFlag = true;
            AdminBL BLObj = new AdminBL();

            // FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/StockLocationLocationList");
            if (FindFlag)
            {
                StockLocation objmodel = new StockLocation();
                ViewBag.LstStockLocation = BLObj.GetStockLocationList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult CreateStockLocation(int STLocationId)
        {
            List<StockLocation> Lst = new List<StockLocation>();
            StockLocation objmodel = new StockLocation();
            AdminBL BLObj = new AdminBL();
            Lst = BLObj.GetParentStockLocation(STLocationId);
            ViewBag.SLDrp = new SelectList(Lst, "STLocationId", "Name");
            if (STLocationId > 0)
            {
                objmodel = BLObj.GetSelectedStockLocation(STLocationId);
            }
            return View(objmodel);
        }
        [HttpPost]
        public ActionResult CreateStockLocation(FormCollection fc)
        {
            StockLocation objmodel = new StockLocation();
            string User_Id = User.Identity.GetUserId();
            AdminBL BLObj = new AdminBL();
            try
            {
                objmodel.Name = fc["Name"].ToString();
                if (fc.AllKeys.Contains("STLocationId"))
                    objmodel.STLocationId = Convert.ToInt32(fc["STLocationId"].ToString());
                if (fc["ParentLocationId"].ToString() != "")
                    objmodel.ParentLocationId = Convert.ToInt32(fc["ParentLocationId"].ToString());
                objmodel.Description = fc["Description"].ToString();
                int ErrorCode = BLObj.SaveStockLocation(objmodel, User_Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("StockLocationList");
        }
        #endregion StockLocation

        #region CurrencyGenerater


        public ActionResult GetCurrencyList()
        {
            AdminBL Blobj = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            List<CurrencyModel> CurrencyList = new List<CurrencyModel>();
            CurrencyList = Blobj.CurrencyList(User_Id);
            return View(CurrencyList);
        }
        [System.Web.Http.ActionName("ExportCurrencyList")]
        [AcceptVerbs("POST")]
        public void ExportCurrencyList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            AdminBL Blobj = new AdminBL();
            var DataSource = Blobj.CurrencyList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreateCurrencyCodes(string Currency)
        {
            AdminBL Blobj = new AdminBL();
            CurrencyModel model = new CurrencyModel();
            model.Source = "Manual";
            model.CurrencyCode = "";
            if (Currency.ToString() != "Create")
            {

                model = Blobj.GetSelectedCurrency(Currency);
            }

            return View(model);
        }
        [HttpPost]
        public ActionResult CreateCurrencyCodes(FormCollection FC)
        {
            AdminBL Blobj = new AdminBL();
            CurrencyModel model = new CurrencyModel();
            string User_Id = User.Identity.GetUserId();
            int errorcode = 0;
            try
            {
                model.CurrencyCode = FC["CurrencyCode"].ToString();
                model.Source = "Manual";
                model.ExchangeRate = Convert.ToDouble(FC["ExchangeRate"].ToString());
                model.CurrencyCode = FC["CurrencyCode"].ToString();
                errorcode = Blobj.SaveCurrency(model, User_Id);
            }
            catch (Exception Ex)
            {
                Common.LogException(Ex);
            }
            return RedirectToAction("GetCurrencyList");
        }
        #endregion CurrencyGenerater 

        #region EmployeeSalaryStruvture
        public ActionResult GetEmpListForSalStructure()
        {
            AdminBL Blobj = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            List<EmployeeModel> lstEmp = new List<EmployeeModel>();
            lstEmp = Blobj.GetEmployeeListForSlaryStructure(User_Id);
            return View(lstEmp);
        }

        public ActionResult GetselectedEmpSalaryStructureDetails(int EmpId)
        {
            AdminBL Blobj = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            EmployeeModel EmpObj = new EmployeeModel();
            EmpSalaryStructureModel model = new EmpSalaryStructureModel();
            EmpObj = Blobj.GetselectedEmpSlaryStructureDetails(EmpId);
            List<EmpSalaryStructureModel> salStructureDetail = new List<EmpSalaryStructureModel>();
            salStructureDetail = Blobj.SalaryStructureList(EmpId);
            if (salStructureDetail.Count > 0)
            {
                var last = salStructureDetail.Last();

                ViewBag.TotalAmount = last.TotalAmount;
            }
            else
            {
                ViewBag.TotalAmount = 0;
            }
            ViewBag.salStructureDetail = salStructureDetail;
            return View(EmpObj);
        }
        public JsonResult GetComponentDetails(int ComponentId)
        {
            string details = "";
            string DRCR = "";
            ArrayList lst = new ArrayList();
            AdminBL Blobj = new AdminBL();
            EmployeeModel EmpObj = new EmployeeModel();
            DataSet ds = new DataSet();
            ds = Blobj.Getcomponentdetail(ComponentId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                details = ds.Tables[0].Rows[0]["ParentName"].ToString();
                DRCR = ds.Tables[0].Rows[0]["DRCR"].ToString();
                lst.Add(details);
                lst.Add(DRCR);
                lst.Add(ds.Tables[0].Rows[0]["VariableRate"].ToString());
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        #endregion EmployeeSalaryStruvture

        [HttpPost]
        public ActionResult GetselectedEmpSalaryStructureDetails(FormCollection Fc)
        {
            AdminBL Blobj = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            EmpSalaryStructureModel model = new EmpSalaryStructureModel();
            model.VariableRate = Convert.ToDouble(Fc["AnnualVariablePay"].ToString());
            model.FixedRate = Convert.ToDouble(Fc["AnnualFixPay"].ToString());
            model.EmpId = Convert.ToInt32(Fc["EmpId"].ToString());
            int Errorcode = Blobj.UpdateEmployeeDetail(model, User_Id);

            return RedirectToAction("GetselectedEmpSalaryStructureDetails", new { EmpId = model.EmpId });
        }

        #region Public Holiday
        public ActionResult GetPublicHolidayList()
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
            bool FindFlag = true;
            AdminBL BLObj = new AdminBL();
            //FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/StockLocationLocationList");
            if (FindFlag)
            {
                List<PublicHolidayModel> LstPubHoliday = new List<PublicHolidayModel>();
                LstPubHoliday = BLObj.GetPublicHolidayList();
                return View(LstPubHoliday);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportPublicHolidayList")]
        [AcceptVerbs("POST")]
        public void ExportPublicHolidayList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            AdminBL BLObj = new AdminBL();
            var DataSource = BLObj.GetPublicHolidayList();
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult CreatePublicHoliday(int HolidayId)
        {
            PublicHolidayModel PublicHolidaymodel = new PublicHolidayModel();
            AdminBL BLObj = new AdminBL();

            ContyStateCityModel addressModel = new ContyStateCityModel();
            addressModel = BLObj.GetCountryStateCityList();
            List<AddRegionModel> rntrylst = addressModel.LstRegion;
            ViewBag.regiondrppp = new SelectList(rntrylst, "RegionId", "RegionName");

            if (HolidayId > 0)
            {
                PublicHolidaymodel = BLObj.GetselectedPublicHolidayList(HolidayId);
            }
            return PartialView(PublicHolidaymodel);
        }
        [HttpPost]
        public ActionResult CreatePublicHoliday(FormCollection fc)
        {
            PublicHolidayModel PublicHolidaymodel = new PublicHolidayModel();
            AdminBL BLObj = new AdminBL();
            string User_Id = User.Identity.GetUserId();
            PublicHolidaymodel.HolidayId = Convert.ToInt32(fc["HolidayId"]);
            PublicHolidaymodel.HolidayDate = Convert.ToDateTime(fc["HolidayDate"]);
            PublicHolidaymodel.FinYear = fc["FinYear"].ToString();
            PublicHolidaymodel.RegionId = Convert.ToInt32(fc["RegionId"].ToString());
            PublicHolidaymodel.Title = fc["Title"].ToString();
            int ErrCode = BLObj.SavePublicHolidayDetail(PublicHolidaymodel, User_Id);
            return RedirectToAction("GetPublicHolidayList");
        }
        #endregion Public Holiday 

        #region Weekly Sal Component

        public ActionResult GetWeeklySalryList()
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
            bool FindFlag = true;
            AdminBL BLObj = new AdminBL();
            //FindFlag = modelObj.FindMenu(lstTaskmodel, "/Admin/StockLocationLocationList");
            if (FindFlag)
            {
                string User_Id = User.Identity.GetUserId();
                // User_Id = "5f474b7e-7b65-4acf-9de2-8d156054cdd2";
                List<WeekLySalaryModel> SalList = new List<WeekLySalaryModel>();
                SalList = BLObj.GetWeeklySalryList(User_Id);
                return View(SalList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [System.Web.Http.ActionName("ExportWeeklySalryList")]
        [AcceptVerbs("POST")]
        public void ExportWeeklySalryList(string GridModel)
        {
            string User_Id = User.Identity.GetUserId();
            ExcelExport exp = new ExcelExport();
            Syncfusion.JavaScript.Models.GridProperties obj = ConvertGridObject(GridModel);
            AdminBL BLObj = new AdminBL();
            var DataSource = BLObj.GetWeeklySalryList(User_Id);
            exp.Export(obj, DataSource, "Export.xlsx", ExcelVersion.Excel2010, false, false, "flat-saffron");
        }
        public ActionResult ViewDetailWeeklyPay(double WeeklyWorkHour, double HourRate, double WorkingDays, int WeekNo, DateTime startdate, DateTime EndDate, int PublicHoliday, int TotalSunday)
        {
            WeekLySalaryModel WeeklyPaymodel = new WeekLySalaryModel();
            try
            {
                string User_Id = User.Identity.GetUserId();
                // User_Id = "5f474b7e-7b65-4acf-9de2-8d156054cdd2";
                AdminBL ObjBL = new AdminBL();
                WeeklyPaymodel = ObjBL.GetSelectedWeeklySalryList(User_Id);
                WeeklyPaymodel.StartDate = Convert.ToDateTime(startdate);
                WeeklyPaymodel.EndDate = Convert.ToDateTime(EndDate);

                int Sdt = startdate.Year;
                DateTime D1 = Convert.ToDateTime("04/01/" + Sdt);
                DateTime D2 = Convert.ToDateTime("03/31/" + (Sdt + 1));
                if (startdate >= D1 && startdate <= D2)
                {
                    WeeklyPaymodel.WorkStartsession = D1;
                    WeeklyPaymodel.WorkEndsession = D2;
                }

                WeeklyPaymodel.WeeklyWorkHour = WeeklyWorkHour;
                WeeklyPaymodel.HourRate = HourRate;
                WeeklyPaymodel.WorkingDays = WorkingDays;
                WeeklyPaymodel.TotalYearDays = WorkingDays + PublicHoliday + TotalSunday;
                WeeklyPaymodel.WeekNo = WeekNo;
                WeeklyPaymodel.PublicHoliday = PublicHoliday;
                WeeklyPaymodel.TotalSunday = TotalSunday;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }

            return PartialView(WeeklyPaymodel);
        }
        #endregion Weekly Sal Component
    }
}