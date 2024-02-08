using SmartSys.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using SmartSys.Utility;
using System.Collections;
using SmartSys.BL.DW;
using SmartSys.BL.Project;
using SmartSys.BL.Enquiry;
using SmartSys.BL.ProViews;

namespace SmartSys.BL
{
    public class AdminBL
    {

        AdminDal adminDalObj = new AdminDal();
        public int SendEngineHeartBeat(int EngineId, string SystemComponent)
        {
            try
            {
                return adminDalObj.SendEngineHeartBeat(EngineId, SystemComponent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetEmployeeByUserId(string User_Id)
        {
            int Empid = 0;
            try
            {
                adminDalObj = new AdminDal();
                DataSet DS = adminDalObj.GetEmployeeByUserId(User_Id);
                foreach (DataRow dr in DS.Tables[1].Rows)
                {
                    Empid = Convert.ToInt32(dr["EmpId"].ToString());
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }

            return Empid;
        }
        public ReportNameModel GetOpenBudgetReport(string user_Id)
        {
            ReportNameModel Model = new ReportNameModel();
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet ds = new DataSet();
                ds = objDAL.GetBudgetReport(user_Id);
                Model.OutputLocation = ds.Tables[0].Rows[0]["OutputLocation"].ToString();
                Model.RunDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["RunDate"].ToString());
                Model.ReportId = ds.Tables[0].Rows[0]["ReportId"].ToString();
                Model.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"].ToString());                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Model;
        }
        public List<SysTaskModel> GetTaskMenu(string User_ID)
        {
            List<SysTaskModel> lstTasks = new List<SysTaskModel>();
            DataSet dsMenu = new DataSet();
            try
            {
                adminDalObj = new AdminDal();
                dsMenu = adminDalObj.GetMenuTask(User_ID);
                if (dsMenu == null) return null;

                foreach (DataRow dr in dsMenu.Tables[0].Rows)
                {
                    SysTaskModel sysTaskModel = new SysTaskModel();
                    sysTaskModel.MenuId = Convert.ToInt32(dr["MenuId"].ToString());
                    sysTaskModel.MenuName = dr["MenuName"].ToString();
                    sysTaskModel.MenuAction = dr["MenuAction"].ToString();
                    sysTaskModel.Href = dr["Href"].ToString();
                    sysTaskModel.ParentMenuId = Convert.ToInt32(dr["ParentMenuId"].ToString());
                    sysTaskModel.Description = dr["Description"].ToString();
                    sysTaskModel.TaskName = dr["TaskName"].ToString();
                    lstTasks.Add(sysTaskModel);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstTasks;
        }
        public SysUser GetUserProfile(String User_Id)
        {
            SysUser objUserModel = new SysUser();
            try
            {
                AdminDal objDAL = new AdminDal();
                List<SysTaskModel> lstTask = new List<SysTaskModel>();
                List<Dashboard> lstDashboard = new List<Dashboard>();
                DataSet dsUser = new DataSet();
                dsUser = objDAL.GetSystemUser(User_Id);
                if (dsUser != null)
                {
                    if (dsUser.Tables.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows.Count > 0)
                        {
                            objUserModel.UserId = Convert.ToInt32(dsUser.Tables[0].Rows[0]["UserId"]);
                            objUserModel.UserName = dsUser.Tables[0].Rows[0]["UserName"].ToString();
                            objUserModel.Name = dsUser.Tables[0].Rows[0]["Name"].ToString();
                            lstTask = GetTaskMenu(User_Id);
                            objUserModel.Tasks = lstTask;
                            lstDashboard = GetDashBoardByUser(User_Id);


                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


            return objUserModel;
        }
        public List<Dashboard> GetDashBoardByUser(string User_Id)
        {
            List<Dashboard> lstDashboard = new List<Dashboard>();
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsobj = new DataSet();
                dsobj = objDAL.GetDashboardByUser(User_Id);
                if (dsobj != null)
                {
                    if (dsobj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            Dashboard objDashboard = new Dashboard();
                            objDashboard.DashboardId = Convert.ToInt16(dr["DashboardId"]);
                            objDashboard.ActionName = dr["ActionName"].ToString();
                            objDashboard.ControllerName = dr["ControllerName"].ToString();
                            objDashboard.Title = dr["Title"].ToString();
                            objDashboard.Description = dr["Description"].ToString();
                            objDashboard.Width = Convert.ToInt16(dr["Width"]);
                            objDashboard.Sequence = Convert.ToInt16(dr["Sequence"]);

                            object obj = dr["ConnectionId"];
                            if (obj == DBNull.Value)
                                objDashboard.ConnectionId = 0;
                            else
                                objDashboard.ConnectionId = Convert.ToInt32(dr["ConnectionId"]);

                            lstDashboard.Add(objDashboard);

                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            return lstDashboard;
        }
        public string GetEmployeeName(string User_ID)
        {
            DataSet dsMenu = new DataSet();
            string EmployeeName = "";
            dsMenu = adminDalObj.GetMenuTask(User_ID);
            if(dsMenu.Tables[1].Rows.Count > 0)
            { 
             EmployeeName = dsMenu.Tables[1].Rows[0]["EmployeeName"].ToString();
            }
            return EmployeeName;
        }
        public string GetUserCompCode(string User_ID)
        {
            DataSet dsMenu = new DataSet();
            string compCode = "";
            dsMenu = adminDalObj.GetUserCompCode(User_ID);
            if (dsMenu.Tables[0].Rows.Count > 0)
            {
                compCode = dsMenu.Tables[0].Rows[0]["CompCode"].ToString();
            }
            return compCode;
        }
        public List<SysTaskModel> GetMenuList(string User_ID)
        {
            List<SysTaskModel> lstTasks = new List<SysTaskModel>();
            DataSet dsMenu = new DataSet();
            try
            {
                adminDalObj = new AdminDal();
                dsMenu = adminDalObj.GetMenuTask(User_ID);
                if (dsMenu == null) return null;

                foreach (DataRow dr in dsMenu.Tables[0].Rows)
                {
                    if (dr["isMenu"].ToString() == "True")
                    {
                        if (Convert.ToInt32(dr["ParentMenuId"].ToString()) == 0)
                        {
                            SysTaskModel sysTaskModel = new SysTaskModel();
                            sysTaskModel.MenuId = Convert.ToInt32(dr["MenuId"].ToString());
                            sysTaskModel.MenuName = dr["MenuName"].ToString();
                            sysTaskModel.MenuAction = dr["MenuAction"].ToString();
                            sysTaskModel.ImageUrl = dr["ImageUrl"].ToString();
                            sysTaskModel.Href = dr["Href"].ToString();
                            sysTaskModel.Description = dr["Description"].ToString();
                            sysTaskModel.TaskName = dr["TaskName"].ToString();
                            sysTaskModel.isMenu = Convert.ToBoolean(dr["isMenu"].ToString());
                            lstTasks.Add(sysTaskModel);
                        }
                        else
                        {
                            SysTaskModel sysTaskModel = new SysTaskModel();
                            sysTaskModel.MenuId = Convert.ToInt32(dr["MenuId"].ToString());
                            sysTaskModel.MenuName = dr["MenuName"].ToString();
                            sysTaskModel.MenuAction = dr["MenuAction"].ToString();
                            sysTaskModel.ImageUrl = dr["ImageUrl"].ToString();
                            sysTaskModel.Href = dr["Href"].ToString();
                            sysTaskModel.ParentMenuId = Convert.ToInt32(dr["ParentMenuId"].ToString());
                            sysTaskModel.Description = dr["Description"].ToString();
                            sysTaskModel.TaskName = dr["TaskName"].ToString();
                            sysTaskModel.isMenu = Convert.ToBoolean(dr["isMenu"].ToString());
                            lstTasks.Add(sysTaskModel);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstTasks;
        }

        public List<SysTaskModel> GetTaskMenuList(string User_ID)
        {
            List<SysTaskModel> lstTasks = new List<SysTaskModel>();
            //DataSet dsMenu = new DataSet();
            //try
            //{
            //    adminDalObj = new AdminDal();
            //    dsMenu = adminDalObj.GetMenuTask(User_ID);
            //    if (dsMenu == null) return null;

            //    foreach (DataRow dr in dsMenu.Tables[0].Rows)
            //    {
            //        if (Convert.ToInt32(dr["ParentMenuId"].ToString()) == 0)
            //        {
            //            SysTaskModel sysTaskModel = new SysTaskModel();
            //            sysTaskModel.MenuId = Convert.ToInt32(dr["MenuId"].ToString());
            //            sysTaskModel.MenuName = dr["MenuName"].ToString();
            //            sysTaskModel.MenuAction = dr["MenuAction"].ToString();
            //            sysTaskModel.ImageUrl = dr["ImageUrl"].ToString();
            //            sysTaskModel.Href = dr["Href"].ToString();
            //            sysTaskModel.ParentMenuId = Convert.ToInt32(dr["ParentMenuId"].ToString());
            //            sysTaskModel.Description = dr["Description"].ToString();
            //            sysTaskModel.TaskName = dr["TaskName"].ToString();
            //            sysTaskModel.isMenu = Convert.ToBoolean(dr["isMenu"].ToString());
            //            sysTaskModel.MenuItems = GetTaskMenuList(sysTaskModel.MenuId, dsMenu);

            //            lstTasks.Add(sysTaskModel);
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            return GetMenuList(User_ID);
        }
        public List<SysTaskModel> GetTaskMenuList(int MenuId, DataSet dsMenu)
        {
            List<SysTaskModel> lstChildTasks = new List<SysTaskModel>();
            foreach (DataRow dr in dsMenu.Tables[0].Rows)
            {
                if (Convert.ToInt32(dr["ParentMenuId"].ToString()) == MenuId)
                {
                    SysTaskModel sysTaskModel = new SysTaskModel();
                    sysTaskModel.MenuId = Convert.ToInt32(dr["MenuId"].ToString());
                    sysTaskModel.MenuName = dr["MenuName"].ToString();
                    sysTaskModel.MenuAction = dr["MenuAction"].ToString();
                    sysTaskModel.Href = dr["Href"].ToString();
                    sysTaskModel.ParentMenuId = Convert.ToInt32(dr["ParentMenuId"].ToString());
                    sysTaskModel.Description = dr["Description"].ToString();
                    sysTaskModel.TaskName = dr["TaskName"].ToString();
                    sysTaskModel.MenuItems = GetTaskMenuList(sysTaskModel.MenuId, dsMenu);

                    lstChildTasks.Add(sysTaskModel);
                }
            }
            return lstChildTasks;
        }


        public String SaveSystemUserRoleDetails(SysUserRoleModel sysuserrolemodel)
        {
            string strResult = "";
            try
            {
                DataSet dsSystemUserRoleDetails = new DataSet();

                DataTable dtUserRole = new DataTable();
                dtUserRole.Columns.Add("UserId", typeof(System.Int32));
                dtUserRole.Columns.Add("RoleId", typeof(System.Int32));

                foreach (string strRoleID in sysuserrolemodel.SelectedRolesToUser)
                {
                    DataRow dr = dtUserRole.NewRow();
                    dr["UserId"] = sysuserrolemodel.UserDetails.UserID;
                    dr["RoleId"] = Convert.ToInt32(strRoleID);
                    dtUserRole.Rows.Add(dr);
                }
                dtUserRole.TableName = "webpages_UsersInRoles";
                dsSystemUserRoleDetails.Tables.Add(dtUserRole);
                adminDalObj = new AdminDal();
                int iErrorCode = adminDalObj.SaveSysemUserRoleDetails(dsSystemUserRoleDetails);
                if (iErrorCode == Convert.ToInt32(ErroCode.InsertSuccessfully))
                {
                    strResult = "User added Successfully to system.";
                }
                else if (iErrorCode == Convert.ToInt32(ErroCode.ModifiedSuccessfully))
                {
                    strResult = "User modified Successfully.";
                }
                else if (iErrorCode == Convert.ToInt32(ErroCode.UserAlreadyPresent))
                {
                    strResult = "User name already present. Please assign different login name";
                }
                else
                {
                    strResult = "Some Error Occured! Please contact administrator.";
                }
            }
            catch (Exception ex)
            {
                strResult = "Error occured while saving user";
                Common.LogException(ex);
            }
            return strResult;


        }

        public String SaveSystemUser(SmartSys.BL.SystemUser sysUser)
        {
            string strResult = "";

            try
            {

                DataSet dsSystemUser = new DataSet();

                DataTable dt = new DataTable();
                dt.Columns.Add("UserID", typeof(System.Int32));
                dt.Columns.Add("UserName", typeof(System.String));
                dt.Columns.Add("DisplayName", typeof(System.String));
                dt.Columns.Add("PasswordHint", typeof(System.String));

                DataRow dr = dt.NewRow();

                dr["UserID"] = sysUser.UserID;
                dr["UserName"] = sysUser.UserName;
                dr["DisplayName"] = sysUser.DisplayName;
                dr["PasswordHint"] = sysUser.PasswordHint;
                //Here add the id of the logged in user...
                dt.Rows.Add(dr);
                dsSystemUser.Tables.Add(dt);

                adminDalObj = new AdminDal();
                int iErrorCode;
                iErrorCode = adminDalObj.SaveSystemUser(dsSystemUser);


                if (iErrorCode == 1)
                {
                    strResult = "User Saved Successfully.";
                }
                else if (iErrorCode == Convert.ToInt32(ErroCode.ModifiedSuccessfully))
                {
                    strResult = "User modified Successfully.";
                }
                else if (iErrorCode == Convert.ToInt32(ErroCode.UserAlreadyPresent))
                {
                    strResult = "User name already present. Please assign different login name";
                }
                else
                {
                    strResult = "Some Error Occured! Please contact administrator.";
                }
            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);

                SmartSys.Utility.FaultExceptionError fault = new Utility.FaultExceptionError();
                fault.Result = false;
                fault.ErrorMsg = ex.Message;
                fault.Description = ex.Message;
                strResult = "Error occured while saving user";
                throw new FaultException<SmartSys.Utility.FaultExceptionError>(fault, ex.Message);
            }

            return strResult;
        }

        public String SaveSystemUser(SmartSys.BL.SystemUser sysUser, List<SmartSys.BL.SystemGroups> lstGroups)
        {
            string strResult = "";

            try
            {

                DataSet dsSystemUser = new DataSet();

                DataTable dt = new DataTable();
                dt.Columns.Add("UserID", typeof(System.Int32));
                dt.Columns.Add("Password", typeof(System.String));
                dt.Columns.Add("FullName", typeof(System.String));
                dt.Columns.Add("LoginName", typeof(System.String));
                dt.Columns.Add("ModifiedBy", typeof(System.Int32));

                DataRow dr = dt.NewRow();

                //dr["UserID"] = sysUser.UserID;
                //dr["Password"] = sysUser.Password;
                //dr["FullName"] = sysUser.FullName;
                //dr["LoginName"] = sysUser.LoginName;

                // Here add the id of the logged in user...
                dr["ModifiedBy"] = 1;


                dt.Rows.Add(dr);

                dsSystemUser.Tables.Add(dt);

                DataTable dtUserGroup = new DataTable();
                dtUserGroup.Columns.Add("UserID", typeof(System.Int32));
                dtUserGroup.Columns.Add("GroupID", typeof(System.Int32));

                // DataRow drUserGrp = dtUserGroup.NewRow();
                foreach (SmartSys.BL.SystemGroups item in lstGroups)
                {
                    DataRow drUserGrp = dtUserGroup.NewRow();
                    drUserGrp["UserID"] = 0;
                    drUserGrp["GroupID"] = item.GroupID;
                    dtUserGroup.Rows.Add(drUserGrp);
                }
                dtUserGroup.TableName = "tbl_UserGroup";
                dsSystemUser.Tables.Add(dtUserGroup);

                adminDalObj = new AdminDal();
                int iErrorCode;
                iErrorCode = adminDalObj.SaveSystemUserDetails(dsSystemUser);


                if (iErrorCode == Convert.ToInt32(ErroCode.InsertSuccessfully))
                {
                    strResult = "User added Successfully to system.";
                }
                else if (iErrorCode == Convert.ToInt32(ErroCode.ModifiedSuccessfully))
                {
                    strResult = "User modified Successfully.";
                }
                else if (iErrorCode == Convert.ToInt32(ErroCode.UserAlreadyPresent))
                {
                    strResult = "User name already present. Please assign different login name";
                }
                else
                {
                    strResult = "Some Error Occured! Please contact administrator.";
                }
            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);

                SmartSys.Utility.FaultExceptionError fault = new Utility.FaultExceptionError();
                fault.Result = false;
                fault.ErrorMsg = ex.Message;
                fault.Description = ex.Message;
                strResult = "Error occured while saving user";
                throw new FaultException<SmartSys.Utility.FaultExceptionError>(fault, ex.Message);
            }

            return strResult;
        }

        public List<SystemUser> GetSysUserList()
        {
            List<SystemUser> lstSysUser = new List<SystemUser>();
            adminDalObj = new AdminDal();
            try
            {
                DataSet dsUserList = new DataSet();
                dsUserList = adminDalObj.GetSystemUserList();
                if (dsUserList.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow dr in dsUserList.Tables[0].Rows)
                    {
                        SystemUser sysuser = new SystemUser();
                        sysuser.UserID = Convert.ToInt32(dr["UserID"].ToString());
                        sysuser.UserName = dr["UserName"].ToString();
                        sysuser.DisplayName = dr["DisplayName"].ToString();
                        sysuser.Email = dr["Email"].ToString();
                        sysuser.PasswordHint = dr["PasswordHint"].ToString();
                        sysuser.UserType = dr["UserType"].ToString();
                        sysuser.PersonName = dr["PersonName"].ToString();
                        lstSysUser.Add(sysuser);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstSysUser;
        }

        public List<SystemRoles> GetUnAssignedRoleList(string User_Id)
        {
            List<SystemRoles> lstRoles = new List<SystemRoles>();
            try
            {
                adminDalObj = new AdminDal();
                DataSet dsRoleList = new DataSet();
                dsRoleList = adminDalObj.GetRoleList(User_Id);
                //DataSet dsAllRoles = adminDalObj.GetRoleList();

                //var set = new HashSet<int>(dsRoleList.Tables[0].AsEnumerable().Select(p => Convert.ToInt32(p["RoleId"])));
                //var result = dsAllRoles.Tables[0].AsEnumerable().Where(i => !set.Contains(Convert.ToInt32(i["RoleId"]))).ToList();

                //if (dsRoleList == null)
                //    return null;
                foreach (DataRow dr in dsRoleList.Tables[0].Rows)
                {
                    SystemRoles sysroles = new SystemRoles();
                    sysroles.RoleID = dr["RoleId"].ToString();
                    sysroles.RoleName = dr["RoleName"].ToString();
                    lstRoles.Add(sysroles);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }

            return lstRoles;
        }

        public SystemUser GetSelectedSysUser(string User_Id)
        {
            SystemUser sysUser = new SystemUser();
            AdminDal objDL = new AdminDal();
            DataSet dsSysUser = objDL.GetSystemUser(User_Id);
            foreach (DataRow dr in dsSysUser.Tables[0].Rows)
            {
                sysUser.UserID = Convert.ToInt32(dr["UserId"].ToString());
                sysUser.UserName = dr["UserName"].ToString();


            }
            return sysUser;
        }
        public List<SystemRoles> GetUserAssignList(string User_Id)
        {
            List<SystemRoles> lstRoles = new List<SystemRoles>();
            try
            {
                adminDalObj = new AdminDal();
                DataSet dsRoleList = adminDalObj.UserAssignRoleList(User_Id);
                if (dsRoleList == null)
                    return null;
                foreach (DataRow dr in dsRoleList.Tables[0].Rows)
                {
                    SystemRoles sysroles = new SystemRoles();
                    sysroles.RoleID = dr["RoleId"].ToString();
                    sysroles.RoleName = dr["RoleName"].ToString();
                    lstRoles.Add(sysroles);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }

            return lstRoles;
        }
        public List<SystemRoles> GetRoleList(string User_Id)
        {
            List<SystemRoles> lstRoles = new List<SystemRoles>();
            try
            {
                adminDalObj = new AdminDal();
                DataSet dsRoleList = adminDalObj.GetRoleList(User_Id);
                if (dsRoleList == null)
                    return null;
                foreach (DataRow dr in dsRoleList.Tables[0].Rows)
                {
                    SystemRoles sysroles = new SystemRoles();
                    sysroles.RoleID = dr["RoleId"].ToString();
                    sysroles.RoleName = dr["RoleName"].ToString();
                    lstRoles.Add(sysroles);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }

            return lstRoles;
        }

        public SystemUserDetails GetSysUserDetails()
        {
            SystemUserDetails sysUserGroupsListObj = new SystemUserDetails();
            try
            {
                adminDalObj = new AdminDal();
                DataSet dsUserDetails = adminDalObj.GetSysUserDetails();
                if (dsUserDetails == null) return null;

                //List<SysUserGroupList> lstSysUserGrpLst = new List<SysUserGroupList>();
                //get user details from dataset
                if (dsUserDetails.Tables[0].Rows.Count != 0)
                {
                    List<SystemUser> lstSysUser = new List<SystemUser>();
                    foreach (DataRow drSysUser in dsUserDetails.Tables[0].Rows)
                    {
                        //SystemUser sysUser = new SystemUser();
                        //sysUser.UserID = Convert.ToInt32(drSysUser["UserId"].ToString());
                        //sysUser.Password = drSysUser["Password"].ToString();
                        //sysUser.LoginName = drSysUser["LoginName"].ToString();
                        //sysUser.FullName = drSysUser["FullName"].ToString();
                        ////sysUserDetailsObj.SysUserDetail = sysUser;
                        //lstSysUser.Add(sysUser);
                    }
                    sysUserGroupsListObj.SysUserList = lstSysUser;
                }
                //Get User group details from dataset
                if (dsUserDetails.Tables[1].Rows.Count != 0)
                {
                    List<SysUserGroupList> lstUserGroupList = new List<SysUserGroupList>();
                    foreach (DataRow drGrp in dsUserDetails.Tables[1].Rows)
                    {
                        //SystemGroups sysGroup = new SystemGroups();
                        //sysGroup.GroupID = Convert.ToInt32(drGrp["GroupID"]);
                        //sysGroup.GroupName = drGrp["GroupName"].ToString();
                        //sysGroup.GroupDescription = drGrp["Description"].ToString();
                        //lstUserGroups.Add(sysGroup);

                        SysUserGroupList sysUserGrpobj = new SysUserGroupList();
                        SystemGroups sysGroup = new SystemGroups();
                        sysGroup.GroupID = Convert.ToInt32(drGrp["GroupID"]);
                        sysGroup.GroupName = drGrp["GroupName"].ToString();
                        sysGroup.GroupDescription = drGrp["Description"].ToString();
                        sysUserGrpobj.SystemGroupsDetails = sysGroup;

                        sysUserGrpobj.UserID = Convert.ToInt32(drGrp["UserID"].ToString());

                        lstUserGroupList.Add(sysUserGrpobj);
                    }
                    sysUserGroupsListObj.SysUserGroupsList = lstUserGroupList;
                }

            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
                SmartSys.Utility.FaultExceptionError fault = new Utility.FaultExceptionError();
                fault.Result = false;
                fault.ErrorMsg = ex.Message;
                fault.Description = ex.Message;
                throw new FaultException<SmartSys.Utility.FaultExceptionError>(fault, ex.Message);
            }

            return sysUserGroupsListObj;

        }

        public List<SmartSys.BL.SystemGroups> GetGroupList()
        {
            adminDalObj = new AdminDal();
            List<SmartSys.BL.SystemGroups> lstGroup = new List<SystemGroups>();
            try
            {
                DataSet dsGroup = new DataSet();
                dsGroup = adminDalObj.GetGroupList();
                if (dsGroup == null) return null;
                if (dsGroup.Tables[0].Rows.Count == 0) return null;

                foreach (DataRow dr in dsGroup.Tables[0].Rows)
                {
                    SystemGroups sysGrpObj = new SystemGroups();
                    sysGrpObj.GroupID = Convert.ToInt32(dr["GroupId"]);
                    sysGrpObj.GroupName = dr["GroupName"].ToString();
                    sysGrpObj.GroupDescription = dr["Description"].ToString();
                    lstGroup.Add(sysGrpObj);
                }

            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);

                SmartSys.Utility.FaultExceptionError fault = new Utility.FaultExceptionError();
                fault.Result = false;
                fault.ErrorMsg = ex.Message;
                fault.Description = ex.Message;
                throw new FaultException<SmartSys.Utility.FaultExceptionError>(fault, ex.Message);
            }
            return lstGroup;
        }



        public DataSet GetBusinessLineList()
        {
            DataSet dsBLDetails = new DataSet();
            try
            {
                AdminDal objAdmDA = new AdminDal();
                dsBLDetails = objAdmDA.GetBusinessLineList();
                return dsBLDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDBConnectionList()
        {
            DataSet dsDBCon = new DataSet();
            try
            {
                AdminDal objAdmDA = new AdminDal();
                dsDBCon = objAdmDA.GetDBConnectionList();
                return dsDBCon;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet GetSelectedDBConnectionList(int ConnectionId)
        {
            DataSet dsDBCon = new DataSet();
            try
            {
                AdminDal objAdmDA = new AdminDal();
                dsDBCon = objAdmDA.GetSelectedDBConnection(ConnectionId);
                return dsDBCon;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataSet GetDepartmentlist(string UserId)
        {


            try
            {
                DataSet ds = new DataSet();
                AdminDal objdal = new AdminDal();
                ds = objdal.GetDepartmentlist(UserId);
                return ds;


            }
            catch (Exception ex)
            {

                throw ex;

            }
        }
        public DataSet GetSelectedDepartmentList(Int16 DeptId)
        {
            try
            {
                AdminDal objdal = new AdminDal();
                DataSet ds = new DataSet();

                ds = objdal.GetSelectedDepartmentList(DeptId);
                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public int SaveDepartment(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                AdminDal objdal = new AdminDal();
                errCode = objdal.SaveDepartment(ds, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveDepartmentTask(SysRolesTaskModel Model,string User_id, int DeptId)
        {
            int ErrCode = 0;
            try
            {
                DataSet dsRoleTask = new DataSet();
                DataTable dtRoleTask = new DataTable();
                dtRoleTask.Columns.Add("DeptId", typeof(System.Int32));
                dtRoleTask.Columns.Add("TaskId", typeof(System.Int32));                
                dtRoleTask.TableName = "tbl_SysRoleTask";

                foreach (SysRoleTasks sysRoleTasks in Model.lstSysRolesTasks)
                {
                    DataRow drRoleTask = dtRoleTask.NewRow();                 
                    drRoleTask["DeptId"] = sysRoleTasks.DeptId;
                    drRoleTask["TaskId"] = sysRoleTasks.TaskID;                                      
                    dtRoleTask.Rows.Add(drRoleTask);
                }
                dsRoleTask.Tables.Add(dtRoleTask);

                adminDalObj = new AdminDal();
                int iErrorCode = adminDalObj.SaveDepartmentTask(dsRoleTask,User_id,DeptId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public DataSet GetUserList()
        {
            try
            {
                DataSet ds = new DataSet();
                AdminDal objDAL = new AdminDal();
                ds = objDAL.GetUserList();
                return ds;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int DeleteUserRole(int UserId)
        {
            int errCode = 0;
            try
            {
                AdminDal objDAL = new AdminDal();
                errCode = objDAL.DeleteUserRole(UserId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public string SaveUserRoles(SystemUser sysUser, List<string> lstRoles)
        {
            string ErrorMsg = "";
            try
            {
                DataSet dsUserRole = new DataSet();
                DataTable dtUserRole = new DataTable();
                dtUserRole.Columns.Add("UserID", typeof(System.Int32));
                dtUserRole.Columns.Add("RoleId", typeof(System.String));

                // DataRow drUserGrp = dtUserGroup.NewRow();
                foreach (string item in lstRoles)
                {
                    DataRow drUserGrp = dtUserRole.NewRow();
                    drUserGrp["UserID"] = sysUser.UserID;
                    drUserGrp["RoleId"] = item;
                    dtUserRole.Rows.Add(drUserGrp);
                }
                dtUserRole.TableName = "tbl_UserRole";
                dsUserRole.Tables.Add(dtUserRole);



                AdminDal objdl = new AdminDal();
                //ErrorMsg = SaveSystemUser(sysUser);
                int errorCode = objdl.SaveUserRoles(dsUserRole);

                if (errorCode == 1)
                {
                    ErrorMsg = "User Saved Successfully.";
                    return ErrorMsg;
                }
                if (errorCode != 1)
                    return "Error Updating User Roles.";


            }
            catch (Exception)
            {

                throw;
            }
            return "";
        }

        public DataSet GetEmpList()
        {
            try
            {
                DataSet ds = new DataSet();
                AdminDal objdal = new AdminDal();
                ds = objdal.GetEmpList();
                return ds;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<SysEmploy> GetEmployeeForDropDown()
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
                }
            }
            return drp;
        }
        public ArrayList GetComanyLogo(int iCompID)
        {
            ArrayList arrList = new ArrayList();
            try
            {
                DataSet dsComp = new DataSet();

                AdminDal adminDalOBJ = new AdminDal();

                dsComp = adminDalOBJ.GetSysCompanyLogo(iCompID);

                arrList.Add(dsComp.Tables[0].Rows[0][0]);
                arrList.Add(dsComp.Tables[0].Rows[0][1]);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return arrList;
        }

        public List<SmartSys.BL.SysCompanyModel> GetSysCompanyList()
        {
            List<SmartSys.BL.SysCompanyModel> lstsysCompany = new List<SysCompanyModel>();
            try
            {
                AdminDal objdal = new AdminDal();
                DataSet dsCompList = objdal.GetSysCompanyList();

                if (dsCompList == null)
                    return null;
                foreach (DataRow dr in dsCompList.Tables[0].Rows)
                {
                    SmartSys.BL.SysCompanyModel sysCompanyModel = new SysCompanyModel();

                    sysCompanyModel.CompId = Convert.ToInt32(dsCompList.Tables[0].Rows[0]["CompId"].ToString());
                    sysCompanyModel.Name = dsCompList.Tables[0].Rows[0]["Name"].ToString();
                    sysCompanyModel.ShortName = dsCompList.Tables[0].Rows[0]["ShortName"].ToString();
                    sysCompanyModel.Pin = dsCompList.Tables[0].Rows[0]["Pin"].ToString();
                    sysCompanyModel.ServiceTN = dsCompList.Tables[0].Rows[0]["ServiceTN"].ToString();
                    sysCompanyModel.State = dsCompList.Tables[0].Rows[0]["State"].ToString();
                    sysCompanyModel.TagLine = dsCompList.Tables[0].Rows[0]["TagLine"].ToString();
                    sysCompanyModel.AddressLine1 = dsCompList.Tables[0].Rows[0]["AddressLine1"].ToString();
                    sysCompanyModel.AddressLine2 = dsCompList.Tables[0].Rows[0]["AddressLine2"].ToString();
                    sysCompanyModel.BST = dsCompList.Tables[0].Rows[0]["BST"].ToString();


                    sysCompanyModel.Logo = dsCompList.Tables[0].Rows[0]["Logo"] as byte[];


                    sysCompanyModel.City = dsCompList.Tables[0].Rows[0]["City"].ToString();
                    sysCompanyModel.Country = dsCompList.Tables[0].Rows[0]["Country"].ToString();
                    sysCompanyModel.CST = dsCompList.Tables[0].Rows[0]["CST"].ToString();

                    lstsysCompany.Add(sysCompanyModel);
                }



            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstsysCompany;
        }

        #region SysCompany
        public string SaveSysCompanyDetails(SmartSys.BL.SysCompanyModel sysCompMoldel)
        {
            string strResult = "";
            int iErrorCode = 0;
            try
            {
                DataSet dsSysCompany = new DataSet();
                DataTable dtSysComp = new DataTable("tbl_SysCompany");
                dtSysComp.Columns.Add("Logo", typeof(System.Byte[]));

                dtSysComp.Columns.Add("Name", typeof(System.String));
                dtSysComp.Columns.Add("ShortName", typeof(System.String));
                dtSysComp.Columns.Add("AddressLine1", typeof(System.String));
                dtSysComp.Columns.Add("AddressLine2", typeof(System.String));
                dtSysComp.Columns.Add("City", typeof(System.String));
                dtSysComp.Columns.Add("State", typeof(System.String));
                dtSysComp.Columns.Add("Country", typeof(System.String));
                dtSysComp.Columns.Add("Pin", typeof(System.String));
                dtSysComp.Columns.Add("TagLine", typeof(System.String));
                dtSysComp.Columns.Add("Cst", typeof(System.String));
                dtSysComp.Columns.Add("Bst", typeof(System.String));
                dtSysComp.Columns.Add("ServiceTN", typeof(System.String));

                DataRow dr = dtSysComp.NewRow();

                dr["Logo"] = sysCompMoldel.Logo;
                dr["Name"] = sysCompMoldel.Name;
                dr["ShortName"] = sysCompMoldel.ShortName;
                dr["AddressLine1"] = sysCompMoldel.AddressLine1;
                dr["AddressLine2"] = sysCompMoldel.AddressLine2;
                dr["City"] = sysCompMoldel.City;
                dr["State"] = sysCompMoldel.State;
                dr["Country"] = sysCompMoldel.Country;
                dr["Pin"] = sysCompMoldel.Pin;
                dr["TagLine"] = sysCompMoldel.TagLine;
                dr["Cst"] = sysCompMoldel.CST;
                dr["Bst"] = sysCompMoldel.BST;
                dr["ServiceTN"] = sysCompMoldel.ServiceTN;

                dtSysComp.Rows.Add(dr);
                dsSysCompany.Tables.Add(dtSysComp);
                AdminDal objdal = new AdminDal();

                iErrorCode = objdal.SaveSysCompanyDetails(dsSysCompany);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }

            if (iErrorCode == 500002)
            {
                strResult = "Company Details added Successfully";
            }
            else
            {
                strResult = "Some Error Occured. Please contact administrator.";
            }
            return strResult;
        }

        #endregion

        public List<SmartSys.BL.SysRoleTaskModel> GetRoleTask(string iRoleID)
        {
            List<SmartSys.BL.SysRoleTaskModel> lstsysRoleTasks = new List<SysRoleTaskModel>();
            AdminDal admindalOBJ = new AdminDal();
            DataSet dsRoleTask = admindalOBJ.GetRoleTasks(iRoleID,0,"");

            if (dsRoleTask == null)
                return null;
            if (dsRoleTask.Tables.Count > 2)
                if (dsRoleTask.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsRoleTask.Tables[3].Rows)
                    {
                        SmartSys.BL.SysRoleTaskModel task = new SysRoleTaskModel();
                        task.id = Convert.ToInt32(dr["TaskId"].ToString());
                        task.pid = Convert.ToInt32(dr["ParentTaskId"].ToString());
                        task.name = dr["TaskName"].ToString();
                        if (dr["RoleID"] == null || dr["RoleID"].ToString() == "")
                            task.IsChecked = false;
                        else
                            task.IsChecked = true;
                        if (Convert.ToInt32(dr["hasChild"].ToString()) == 0)
                            task.HasChild = false;
                        else
                            task.HasChild = true;
                        lstsysRoleTasks.Add(task);
                    }
                }
            return lstsysRoleTasks;
        }

        #region[Role Tasks]
        public SmartSys.BL.SysRolesTaskModel GetRoleTasks(string iRoleID,int DeptId,string User_Id)
        {
            SmartSys.BL.SysRolesTaskModel sysRolesTaskModel = new SysRolesTaskModel();
            List<SmartSys.BL.SysRoleTaskModel> lstsysRoleTasks = new List<SysRoleTaskModel>();
            try
            {
                AdminDal admindalOBJ = new AdminDal();
                DataSet dsRoleTask = admindalOBJ.GetRoleTasks(iRoleID, DeptId, User_Id);
                if (dsRoleTask == null)
                    return null;
                sysRolesTaskModel.Role = new SystemRoles();
                //check whether request is for edit i.e. iRoleID is not zero
                //Otherwise it is new role creation if iRoleID is zero
                if (iRoleID != "0" && dsRoleTask.Tables[0].Rows.Count > 0)
                {
                    sysRolesTaskModel.Role.RoleName = dsRoleTask.Tables[0].Rows[0][0].ToString();
                }

                //Get Assigened tasks to role
                sysRolesTaskModel.AssignedTaskList = new List<SysRoleTasks>();
                foreach (DataRow dr in dsRoleTask.Tables[1].Rows)
                {
                    SysRoleTasks sysRoleTasks = new SysRoleTasks();
                    sysRoleTasks.TaskName = dr["TaskName"].ToString();
                    sysRoleTasks.TaskID = Convert.ToInt32(dr["TaskId"].ToString());

                    sysRolesTaskModel.AssignedTaskList.Add(sysRoleTasks);
                }
                sysRolesTaskModel.Role.RoleID = iRoleID;
                //Get Whole task list
                sysRolesTaskModel.lstSysRolesTasks = new List<SysRoleTasks>();

                foreach (DataRow dr in dsRoleTask.Tables[2].Rows)
                {
                    SysRoleTasks sysRoleTasks = new SysRoleTasks();
                    sysRoleTasks.TaskName = dr["TaskName"].ToString();
                    sysRoleTasks.TaskID = Convert.ToInt32(dr["TaskId"].ToString());

                    sysRolesTaskModel.lstSysRolesTasks.Add(sysRoleTasks);
                }
                sysRolesTaskModel.lstsysRoleTasks = new List<SysRoleTasks>();
                if (dsRoleTask.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsRoleTask.Tables[3].Rows)
                    {
                        SysRoleTasks sysRoleTasks = new SysRoleTasks();
                        //SmartSys.BL.SysRoleTaskModel task = new SysRoleTaskModel();

                        sysRoleTasks.id = Convert.ToInt32(dr["TaskId"].ToString());
                        sysRoleTasks.pid = Convert.ToInt32(dr["ParentTaskId"].ToString());
                        sysRoleTasks.name = dr["TaskName"].ToString();
                        sysRoleTasks.RoleID = dr["RoleID"].ToString();
                        if (dr["RoleID"] == null || dr["RoleID"].ToString() == "")
                            sysRoleTasks.IsChecked = false;
                        else
                            sysRoleTasks.IsChecked = true;
                        if (Convert.ToInt32(dr["hasChild"].ToString()) == 0)
                            sysRoleTasks.HasChild = false;
                        else
                            sysRoleTasks.HasChild = true;
                        sysRolesTaskModel.lstsysRoleTasks.Add(sysRoleTasks);
                    }
                }


            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return sysRolesTaskModel;
        }
        //public SmartSys.BL.SysRolesTaskModel GetRoleTasks(string iRoleID)
        //{
        //    SmartSys.BL.SysRolesTaskModel sysRolesTaskModel = new SysRolesTaskModel();
        //    try
        //    {
        //        AdminDal admindalOBJ = new AdminDal();
        //        DataSet dsRoleTask = admindalOBJ.GetRoleTasks(iRoleID);
        //        if (dsRoleTask == null)
        //            return null;
        //        sysRolesTaskModel.Role = new SystemRoles();
        //        //check whether request is for edit i.e. iRoleID is not zero
        //        //Otherwise it is new role creation if iRoleID is zero
        //        if (iRoleID != "0" && dsRoleTask.Tables[0].Rows.Count > 0) 
        //        {                
        //            sysRolesTaskModel.Role.RoleName = dsRoleTask.Tables[0].Rows[0][0].ToString();
        //        }

        //        //Get Assigened tasks to role
        //        sysRolesTaskModel.AssignedTaskList = new List<SysRoleTasks>();
        //        foreach (DataRow dr in dsRoleTask.Tables[1].Rows)
        //        {
        //            SysRoleTasks sysRoleTasks = new SysRoleTasks();
        //            sysRoleTasks.TaskName = dr["TaskName"].ToString();
        //            sysRoleTasks.TaskID = Convert.ToInt32(dr["TaskId"].ToString());

        //            sysRolesTaskModel.AssignedTaskList.Add(sysRoleTasks);
        //        }
        //        sysRolesTaskModel.Role.RoleID = iRoleID;
        //        //Get Whole task list
        //        sysRolesTaskModel.lstSysRolesTasks = new List<SysRoleTasks>();

        //        foreach (DataRow dr in dsRoleTask.Tables[2].Rows)
        //        {
        //            SysRoleTasks sysRoleTasks = new SysRoleTasks();
        //            sysRoleTasks.TaskName = dr["TaskName"].ToString();
        //            sysRoleTasks.TaskID = Convert.ToInt32(dr["TaskId"].ToString());

        //            sysRolesTaskModel.lstSysRolesTasks.Add(sysRoleTasks);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        Common.LogException(ex);
        //    }
        //    return sysRolesTaskModel;
        //}
        public int DeleteRoletask(string RoleId)
        {
            int errCode = 0;
            try
            {
                AdminDal objDAL = new AdminDal();
                errCode = objDAL.DeleteRoleTask(RoleId);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public string SaveRoleTasks(SmartSys.BL.SysRolesTaskModel sysRolesTaskModel, string User_Id)
        {
            string strResult = "";
            bool NewRole = false;

            try
            {
                DataSet dsRoleTask = new DataSet();

                DataTable dtRole = new DataTable();
                dtRole.Columns.Add("RoleId", typeof(System.String));
                dtRole.Columns.Add("RoleName", typeof(System.String));
                dtRole.TableName = "tbl_SysRole";

                DataRow drRole = dtRole.NewRow();
                if (sysRolesTaskModel.Role.RoleID == "0")
                {
                    sysRolesTaskModel.Role.RoleID = Guid.NewGuid().ToString();
                    NewRole = true;
                }
                drRole["RoleId"] = sysRolesTaskModel.Role.RoleID;
                drRole["RoleName"] = sysRolesTaskModel.Role.RoleName;
                dtRole.Rows.Add(drRole);
                dsRoleTask.Tables.Add(dtRole);
                if (NewRole)
                {
                    //adminDalObj.SaveRoleTask(dsRoleTask, "NewRole", User_Id);
                    //return strResult;
                }

                DataTable dtRoleTask = new DataTable();
                dtRoleTask.Columns.Add("RoleId", typeof(System.String));
                dtRoleTask.Columns.Add("TaskId", typeof(System.Int32));
                dtRoleTask.Columns.Add("CreatedBy", typeof(System.Int32));
                dtRoleTask.Columns.Add("ModifiedBy", typeof(System.Int32));
                dtRoleTask.Columns.Add("User_ID", typeof(System.String));
                dtRoleTask.TableName = "tbl_SysRoleTask";

                foreach (SysRoleTasks sysRoleTasks in sysRolesTaskModel.lstSysRolesTasks)
                {
                    DataRow drRoleTask = dtRoleTask.NewRow();
                    if (sysRoleTasks.RoleID == "0")
                        sysRoleTasks.RoleID = sysRolesTaskModel.Role.RoleID;
                    drRoleTask["RoleId"] = sysRoleTasks.RoleID;
                    drRoleTask["TaskId"] = sysRoleTasks.TaskID;
                    drRoleTask["CreatedBy"] = sysRoleTasks.CreatedBy;
                    drRoleTask["ModifiedBy"] = sysRoleTasks.ModifiedBy;
                    drRoleTask["User_ID"] = User_Id;
                    dtRoleTask.Rows.Add(drRoleTask);
                }
                dsRoleTask.Tables.Add(dtRoleTask);

                adminDalObj = new AdminDal();
                int iErrorCode = adminDalObj.SaveRoleTask(dsRoleTask, "EditRole", User_Id);
                if (iErrorCode == 500002)
                {
                    strResult = "Role details Inserted successfully.";
                }
                else if (iErrorCode == 500003)
                {
                    strResult = "Role details Updated successfully.";
                }

            }
            catch (Exception ex)
            {
                strResult = "Some Error occured please contact administrator.";
                Common.LogException(ex);
            }
            return strResult;
        }
        #endregion

        public EmployeeModel GetSelectedEmployeeList(int EmpId)
        {
            EmployeeModel EmpObj = new EmployeeModel();
            EmpObj.emailConfig = new EmailConfig();
            EmpObj.CustList = new List<EmployeeCustomer>();
            EmpObj.VendList = new List<EmployeeVendor>();
            EmpObj.LstExperience = new List<ExperienceModel>();
            EmpObj.DeptList = new List<Departmentmodel>();
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.GetSelectedEmployeeList(EmpId);
                if (dsEmp == null)
                    return null;
                if (dsEmp.Tables.Count > 0)
                {
                    if (dsEmp.Tables[0].Rows.Count > 0)
                    {


                        EmpObj.EmpId = Convert.ToInt32(dsEmp.Tables[0].Rows[0]["EmpId"].ToString());
                        EmpObj.Gender = dsEmp.Tables[0].Rows[0]["Gender"].ToString();
                        EmpObj.CompCode = dsEmp.Tables[0].Rows[0]["CompCode"].ToString();
                        EmpObj.FirstName = dsEmp.Tables[0].Rows[0]["FirstName"].ToString();
                        EmpObj.MiddleName = dsEmp.Tables[0].Rows[0]["MiddleName"].ToString();
                        EmpObj.LastName = dsEmp.Tables[0].Rows[0]["LastName"].ToString();
                        EmpObj.EmpName = dsEmp.Tables[0].Rows[0]["EmpName"].ToString();
                        EmpObj.PAN = dsEmp.Tables[0].Rows[0]["PAN"].ToString();
                        EmpObj.AADHAR = dsEmp.Tables[0].Rows[0]["AADHAR"].ToString();
                        EmpObj.PassportNumber = dsEmp.Tables[0].Rows[0]["PassportNumber"].ToString();
                        EmpObj.PhoneNumber = dsEmp.Tables[0].Rows[0]["PhoneNumber"].ToString();
                        EmpObj.Mobile = dsEmp.Tables[0].Rows[0]["Mobile"].ToString();
                        EmpObj.LeaveOpBal = Convert.ToInt32(dsEmp.Tables[0].Rows[0]["LeaveOpBal"].ToString());
                        EmpObj.ManagerId = Convert.ToInt32(dsEmp.Tables[0].Rows[0]["ManagerId"].ToString());
                        EmpObj.ManagerName = dsEmp.Tables[0].Rows[0]["managerName"].ToString();
                        EmpObj.UserId = Convert.ToInt16(dsEmp.Tables[0].Rows[0]["UserId"].ToString());
                        EmpObj.AnnualFixPay = Convert.ToDouble(dsEmp.Tables[0].Rows[0]["AnnualFixPay"].ToString());
                        EmpObj.AnnualVariablePay = Convert.ToDouble(dsEmp.Tables[0].Rows[0]["AnnualVariablePay"].ToString());
                        EmpObj.UserName = dsEmp.Tables[0].Rows[0]["UserName"].ToString();
                        EmpObj.emailId = dsEmp.Tables[0].Rows[0]["emailId"].ToString();
                        EmpObj.DateOfJoin = Convert.ToDateTime(dsEmp.Tables[0].Rows[0]["DateOfJoin"].ToString());
                        EmpObj.Designation = dsEmp.Tables[0].Rows[0]["Designation"].ToString();
                        EmpObj.Qualification = dsEmp.Tables[0].Rows[0]["Qualification"].ToString();
                        EmpObj.Region = Convert.ToInt16(dsEmp.Tables[0].Rows[0]["RegionId"].ToString());
                        //EmpObj.DeptName = dsEmp.Tables[0].Rows[0]["DeptName"].ToString();
                        EmpObj.CreatedByName = dsEmp.Tables[0].Rows[0]["CreatedByName"].ToString();
                        EmpObj.CreatedDate = Convert.ToDateTime(dsEmp.Tables[0].Rows[0]["CreatedDate"].ToString());
                        EmpObj.ModifiedByName = dsEmp.Tables[0].Rows[0]["ModifiedByName"].ToString();
                        EmpObj.ModifiedDate = Convert.ToDateTime(dsEmp.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        EmpObj.Deleted = Convert.ToBoolean(dsEmp.Tables[0].Rows[0]["Deleted"].ToString());
                        if (dsEmp.Tables[0].Rows[0]["LastDateOfWork"].ToString() != "")
                            EmpObj.LastDateOfWork = Convert.ToDateTime(dsEmp.Tables[0].Rows[0]["LastDateOfWork"].ToString());
                        EmpObj.Remark = dsEmp.Tables[0].Rows[0]["Remark"].ToString();
                        if (dsEmp.Tables[0].Rows[0]["isDeleted"].ToString() == "")
                        {
                            EmpObj.isClient = false;
                        }
                        else
                        {
                            EmpObj.isClient = Convert.ToBoolean(dsEmp.Tables[0].Rows[0]["isDeleted"]);
                        }
                        if (dsEmp.Tables[0].Rows[0]["Photo"].ToString() != "")
                            EmpObj.Photo = (Byte[])(dsEmp.Tables[0].Rows[0]["Photo"]);

                        if (dsEmp.Tables[0].Rows[0]["EmailServer"].ToString() != "")
                            EmpObj.emailConfig.EmailServer = dsEmp.Tables[0].Rows[0]["EmailServer"].ToString();
                        if (dsEmp.Tables[0].Rows[0]["SendingMailServer"].ToString() != "")
                            EmpObj.emailConfig.SendingMailServer = dsEmp.Tables[0].Rows[0]["SendingMailServer"].ToString();
                        if (dsEmp.Tables[0].Rows[0]["EmailUserName"].ToString() != "")
                            EmpObj.emailConfig.EmailUserName = dsEmp.Tables[0].Rows[0]["EmailUserName"].ToString();
                        if (dsEmp.Tables[0].Rows[0]["EmailPassword"].ToString() != "")
                            EmpObj.emailConfig.EmailPassword = dsEmp.Tables[0].Rows[0]["EmailPassword"].ToString();
                        if (dsEmp.Tables[0].Rows[0]["EmailPort"].ToString() != "")
                            EmpObj.emailConfig.EmailPort = dsEmp.Tables[0].Rows[0]["EmailPort"].ToString();
                        if (dsEmp.Tables[0].Rows[0]["SendingEmailPort"].ToString() != "")
                            EmpObj.emailConfig.SendingEmailPort = dsEmp.Tables[0].Rows[0]["SendingEmailPort"].ToString();
                        if (dsEmp.Tables[0].Rows[0]["SSL"].ToString() != "")
                            EmpObj.emailConfig.SSL = Convert.ToBoolean(dsEmp.Tables[0].Rows[0]["SSL"].ToString());
                    }
                }
                if (dsEmp.Tables.Count > 2)
                {
                    foreach (DataRow dr in dsEmp.Tables[2].Rows)
                    {
                        EmployeeCustomer ec = new EmployeeCustomer();
                        ec.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                        ec.CustomerName = dr["CustomerName"].ToString();
                        ec.DepartmentName = dr["DeptName"].ToString();
                        ec.EmpId = EmpId;
                        ec.ModifiedBy = dr["ModifiedBy"].ToString();
                        ec.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        EmpObj.CustList.Add(ec);
                    }
                }
                if (dsEmp.Tables.Count > 3)
                {
                    foreach (DataRow dr in dsEmp.Tables[3].Rows)
                    {
                        EmployeeVendor ec = new EmployeeVendor();
                        ec.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                        ec.VendorName = dr["VendorName"].ToString();
                        ec.EmpId = EmpId;
                        ec.ModifiedBy = dr["ModifiedBy"].ToString();
                        ec.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        EmpObj.VendList.Add(ec);
                    }
                }
                if (dsEmp.Tables.Count > 4)
                {
                    foreach (DataRow dr in dsEmp.Tables[4].Rows)
                    {
                        Departmentmodel DeptModel = new Departmentmodel();
                        DeptModel.DeptId = Convert.ToInt16(dr["DeptId"].ToString());
                        DeptModel.DeptName = dr["DeptName"].ToString();
                        DeptModel.DeptHeadName = dr["DeptHead"].ToString();
                        DeptModel.CreatedByName = dr["CreatedBy"].ToString();
                        DeptModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        EmpObj.DeptList.Add(DeptModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return (EmpObj);
        }
        public List<Departmentmodel> GetEmployeeDept(int Empid)
        {
            List<Departmentmodel> LstDept = new List<Departmentmodel>();
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.GetSelectedEmployeeList(Empid);
                if (dsEmp.Tables.Count > 4)
                {
                    foreach (DataRow dr in dsEmp.Tables[4].Rows)
                    {
                        Departmentmodel DeptModel = new Departmentmodel();
                        DeptModel.DeptId = Convert.ToInt16(dr["DeptId"].ToString());
                        DeptModel.DeptName = dr["DeptName"].ToString();
                        DeptModel.DeptHeadName = dr["DeptHead"].ToString();
                        DeptModel.UseDept = Convert.ToInt16(dr["UseDept"].ToString());
                        DeptModel.CreatedByName = dr["CreatedBy"].ToString();
                        DeptModel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        LstDept.Add(DeptModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return LstDept;
        }
        public List<ExperienceModel> GetEmployeeExprienceList(int EmpId)
        {
            List<ExperienceModel> EmpObj = new List<ExperienceModel>();
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.GetEmployeeExprience(EmpId);
                if (dsEmp == null)
                    return null;
                if (dsEmp.Tables.Count > 0)
                {
                    if (dsEmp.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsEmp.Tables[0].Rows)
                        {
                            ExperienceModel demo = new ExperienceModel();
                            demo.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            demo.CompanyName = dr["CompanyName"].ToString();
                            demo.Designation = dr["Designation"].ToString();
                            demo.JobProfile = dr["JobProfile"].ToString();
                            demo.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                            demo.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                            EmpObj.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return EmpObj;
        }
        public ExperienceModel GetSelectedEmployeeExprienceList(int EmpId, string CompanyName)
        {
            ExperienceModel EmpObj = new ExperienceModel();
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.GetSelectedEmployeeExprience(EmpId, CompanyName);
                if (dsEmp == null)
                    return null;
                if (dsEmp.Tables.Count > 0)
                {
                    if (dsEmp.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsEmp.Tables[0].Rows)
                        {
                            EmpObj.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            EmpObj.CompanyName = dr["CompanyName"].ToString();
                            EmpObj.Designation = dr["Designation"].ToString();
                            EmpObj.JobProfile = dr["JobProfile"].ToString();
                            EmpObj.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                            EmpObj.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return EmpObj;
        }
        public int SaveEmployeeExprienceList(ExperienceModel Model)
        {
            int errcode = 0;
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.GetSelectedEmployeeExprience(0, "");
                if (dsEmp != null)
                    if (dsEmp.Tables.Count > 0)
                    {
                        dsEmp.Tables[0].Rows.Clear();
                        DataRow dr = dsEmp.Tables[0].NewRow();
                        dr["EmpId"] = Model.EmpId;
                        dr["CompanyName"] = Model.CompanyName;
                        dr["NewCompanyName"] = Model.NewCompanyName;
                        dr["Designation"] = Model.Designation;
                        dr["JobProfile"] = Model.JobProfile;
                        dr["StartDate"] = Model.StartDate;
                        dr["EndDate"] = Model.EndDate;
                        dsEmp.Tables[0].Rows.Add(dr);
                    }
                errcode = objDAL.SaveEmployeeExprience(dsEmp);
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return errcode;
        }
        public List<EmployeeModel> GetEmployeeList(string UserId)
        {
            List<EmployeeModel> lstEmp = new List<EmployeeModel>();
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.GetEmployeeList(UserId);
                if (dsEmp == null)
                    return null;
                if (dsEmp.Tables.Count > 0)
                {
                    if (dsEmp.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsEmp.Tables[0].Rows)
                        {
                            EmployeeModel EmpObj = new EmployeeModel();
                            EmpObj.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            EmpObj.EmpName = dr["EmpName"].ToString();
                            EmpObj.Gender = dr["Gender"].ToString();
                            EmpObj.ManagerName = dr["ManagerName"].ToString();
                            EmpObj.UserName = dr["UserName"].ToString();
                            EmpObj.emailId = dr["emailId"].ToString();
                            EmpObj.DateOfJoin = Convert.ToDateTime(dr["DateOfJoin"].ToString());
                            EmpObj.Designation = dr["Designation"].ToString();
                            EmpObj.Qualification = dr["Qualification"].ToString();
                         //   EmpObj.DeptName = dr["DeptName"].ToString();
                            EmpObj.ManagerId = Convert.ToInt32(dr["ManagerId"].ToString());
                            EmpObj.Deleted = Convert.ToBoolean(dr["Deleted"].ToString());
                            if (dr["LastDateOfWork"].ToString() != "")
                            {
                                EmpObj.LastDateOfWork = Convert.ToDateTime(dr["LastDateOfWork"]);
                            }
                            if (dr["Remark"].ToString() != "")
                            {
                                EmpObj.Remark = dr["Remark"].ToString();
                            }
                            EmpObj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            EmpObj.CompRelMapCount = Convert.ToInt16(dr["CompRelMapCount"].ToString());
                            //if (dsEmp.Tables[0].Rows[0]["Photo"].ToString() != "")
                            //    EmpObj.Photo = (Byte[])(dsEmp.Tables[0].Rows[0]["Photo"]);
                            lstEmp.Add(EmpObj);
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return lstEmp;
        }
        public List<BudgetCustmerModel> GetBudgetEmployeeList(string User_Id)
        {
            List<BudgetCustmerModel> lstEmp = new List<BudgetCustmerModel>();
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.GetEmployeeByUserId(User_Id);
                if (dsEmp == null)
                    return null;
                if (dsEmp.Tables.Count > 0)
                {
                    if (dsEmp.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsEmp.Tables[0].Rows)
                        {
                            BudgetCustmerModel EmpObj = new BudgetCustmerModel();
                            EmpObj.EmployeeId = Convert.ToInt32(dr["EmpId"].ToString());
                            EmpObj.EmployeeName = dr["EmployeeName"].ToString();
                            lstEmp.Add(EmpObj);
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
           
            return lstEmp;
        }
        public List<ExpertiesModel> GetEmployeeExpertiesList(int EmpId)
        {
            EmployeeModel EmpModel = new EmployeeModel();
            EmpModel.LstExperties = new List<ExpertiesModel>();

            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.EmployeeExpertiesList(EmpId);
                if (dsEmp == null)
                    return null;
                if (dsEmp.Tables.Count > 0)
                {
                    if (dsEmp.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsEmp.Tables[0].Rows)
                        {
                            ExpertiesModel ExprtObj = new ExpertiesModel();

                            ExprtObj.Exp_Level = Convert.ToInt32(dr["Exp_Level"].ToString());
                            ExprtObj.ExpInYears = Convert.ToDouble(dr["ExpInYears"].ToString());
                            ExprtObj.Experties = dr["Experties"].ToString();
                            EmpModel.LstExperties.Add(ExprtObj);
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return EmpModel.LstExperties;
        }
        public ExpertiesModel GetSelectedEXperties(int EmpId, string Experties)
        {
            ExpertiesModel ExprtObj = new ExpertiesModel();
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.GetSelectedExperties(EmpId, Experties);
                if (dsEmp == null)
                    return null;
                if (dsEmp.Tables.Count > 0)
                {
                    if (dsEmp.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsEmp.Tables[0].Rows)
                        {
                            ExprtObj.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            ExprtObj.Exp_Level = Convert.ToInt32(dr["Exp_Level"].ToString());
                            ExprtObj.Experties = dr["Experties"].ToString();
                            ExprtObj.NewExperties = dr["NewExperties"].ToString();
                            ExprtObj.ExpInYears = Convert.ToDouble(dr["ExpInYears"].ToString());

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return ExprtObj;
        }
        public int SaveEmployeeExperties(ExpertiesModel ExprtModel, string User_Id)
        {
            AdminDal objDAL = new AdminDal();
            DataSet dsEmp = new DataSet();
            try
            {

                dsEmp = objDAL.GetSelectedExperties(0, "");
                if (dsEmp == null)
                    return 0;
                if (dsEmp.Tables.Count > 0)
                {
                    dsEmp.Tables[0].Rows.Clear();
                    DataRow dr = dsEmp.Tables[0].NewRow();
                    dr["EmpId"] = ExprtModel.EmpId;
                    dr["NewExperties"] = ExprtModel.NewExperties;
                    dr["Experties"] = ExprtModel.Experties;
                    dr["ExpInYears"] = ExprtModel.ExpInYears;
                    dr["Exp_Level"] = ExprtModel.Exp_Level;


                    dsEmp.Tables[0].Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objDAL.Saveempexperties(dsEmp, User_Id);
        }
        public int SaveEmployee(EmployeeModel empModel, string User_Id, bool isClient, string CheckNull)
        {
            AdminDal objDAL = new AdminDal();
            DataSet dsEmp = new DataSet();
            try
            {
                dsEmp = objDAL.GetSelectedEmployeeList(0);
                if (dsEmp == null)
                    return 0;
                if (dsEmp.Tables.Count > 0)
                {
                    dsEmp.Tables[0].Rows.Clear();
                    DataRow dr = dsEmp.Tables[0].NewRow();
                    dr["EmpId"] = empModel.EmpId;
                    dr["CompCode"] = empModel.CompCode;
                    dr["FirstName"] = empModel.FirstName;
                    dr["Gender"] = empModel.Gender;
                    dr["PAN"] = empModel.PAN;
                    dr["AADHAR"] = empModel.AADHAR;
                    dr["PassportNumber"] = empModel.PassportNumber;
                    dr["LeaveOpBal"] = empModel.LeaveOpBal;
                    dr["Mobile"] = empModel.Mobile;
                    dr["PhoneNumber"] = empModel.PhoneNumber;
                    dr["LastName"] = empModel.LastName;
                    dr["MiddleName"] = empModel.MiddleName;
                    dr["emailId"] = empModel.emailId;
                    dr["DateOfJoin"] = empModel.DateOfJoin;
                    dr["AnnualFixPay"] = empModel.AnnualFixPay;
                    dr["AnnualVariablePay"] = empModel.AnnualVariablePay;
                    dr["Designation"] = empModel.Designation;
                    dr["Qualification"] = empModel.Qualification;
                    dr["UserId"] = empModel.UserId;
                    dr["ManagerId"] = empModel.ManagerId;
                    dr["RegionId"] = empModel.Region;
                    dr["ModifiedBy"] = empModel.ModifiedBy;
                    dr["Deleted"] = empModel.Deleted;
                    dr["Photo"] = empModel.Photo;
                    if (empModel.EmpId != 0)
                    {
                        dr["EmailServer"] = empModel.emailConfig.EmailServer;
                        dr["SendingMailServer"] = empModel.emailConfig.SendingMailServer;
                        dr["EmailPort"] = empModel.emailConfig.EmailPort;
                        dr["SendingEmailPort"] = empModel.emailConfig.SendingEmailPort;
                        dr["EmailUserName"] = empModel.emailConfig.EmailUserName;
                        dr["EmailPassword"] = empModel.emailConfig.EmailPassword;
                        dr["SSL"] = empModel.emailConfig.SSL;
                    }

                    dsEmp.Tables[0].Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objDAL.SaveEmployee(dsEmp, User_Id, isClient, CheckNull);
        }
        public List<SystemUser> GetUserListNonEmployee(int EmpId)
        {
            List<SystemUser> LstSysModel = new List<SystemUser>();
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.GetUserListNonEmployee(EmpId);
                if (dsEmp.Tables.Count > 0)
                {
                    if (dsEmp.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsEmp.Tables[0].Rows)
                        {
                            SystemUser SysModel = new SystemUser();
                            SysModel.UserID = Convert.ToInt16(dr["UserId"].ToString());
                            SysModel.UserName = dr["UserName"].ToString();
                            SysModel.EmpId = EmpId;
                            LstSysModel.Add(SysModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return (LstSysModel);
        }
        public List<Departmentmodel> DeptDropdown(string UserId)
        {
            List<Departmentmodel> lstSysDept = new List<Departmentmodel>();
            AdminBL SysDeptBL = new AdminBL();

            DataSet dsSysDept = SysDeptBL.GetDepartmentlist(UserId);
            if (dsSysDept != null)
            {
                if (dsSysDept.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsSysDept.Tables[0].Rows)
                    {
                        Departmentmodel objmodel = new Departmentmodel();
                        objmodel.DeptId = Convert.ToInt16(dr["DeptId"]);
                        objmodel.DeptName = dr["DeptName"].ToString();
                        lstSysDept.Add(objmodel);

                    }
                }
            }
            return lstSysDept;
        }
        public List<SysEmploy> EmployeeDropDown()
        {
            List<SysEmploy> lstEmp = new List<SysEmploy>();
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
                        objmodel.Color = "#51a0ed";
                        lstEmp.Add(objmodel);

                    }
                }
            }
            return lstEmp;
        }
        public int DeleteEmployee(EmployeeModel EmpModel,string User_Id)
        {
            int errcode = 0;
            try
            {
                AdminDal objDAL = new AdminDal();
                errcode = objDAL.DeleteEmployee(EmpModel.EmpId, EmpModel.LastDateOfWork, EmpModel.Deleted, EmpModel.Remark, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errcode;
        }
        public int SysUesrRemove(int EmpId)
        {
            try
            {
                AdminDal objDAL = new AdminDal();
                return objDAL.SysUesrRemove(EmpId);
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
                return 0;
            }

        }
        public List<EmoloyeeMapping> GetDWCompList()
        {
            List<EmoloyeeMapping> lstEmpMap = new List<EmoloyeeMapping>();

            try
            {
                AdminDal DALObj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DALObj.GetDWCompList();
                if (dsObj != null)
                {
                    if (dsObj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            EmoloyeeMapping MapObj = new EmoloyeeMapping();
                            MapObj.CompCode = dr["CompCode"].ToString();
                            MapObj.CompanyName = dr["CompanyName"].ToString();
                            MapObj.ConnectionId = Convert.ToInt16(dr["ConnectionId"]);
                            MapObj.EmpListSP = dr["EmpListSP"].ToString();
                            lstEmpMap.Add(MapObj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstEmpMap;
        }
        public List<MapClientEmpList> GetCompanyEmp(string CompCode, string SPName, ArrayList ConnInfo)
        {
            List<MapClientEmpList> lstMapEmp = new List<MapClientEmpList>();
            try
            {
                DataSet dsobj = new DataSet();
                AdminDal DALObj = new AdminDal();
                dsobj = DALObj.GetCompanyEmp(CompCode, SPName, ConnInfo);
                if (dsobj != null)
                {
                    if (dsobj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            MapClientEmpList MapEmpTemp = new MapClientEmpList();
                            MapEmpTemp.ComEmpID = dr["EmpId"].ToString();
                            MapEmpTemp.ComEmpName = dr["EmpName"].ToString();
                            lstMapEmp.Add(MapEmpTemp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstMapEmp;
        }
        public int SaveMapping(EmoloyeeMapping EmpMap)
        {
            int ErrorCode = 0;
            try
            {
                AdminDal DALObj = new AdminDal();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("tbl_DHFeedEventDep");
                dt.Columns.Add("Emp_No", typeof(System.String));
                dt.Columns.Add("CompCode", typeof(System.String));
                dt.Columns.Add("EmpId", typeof(System.Int32));

                DataRow dr = dt.NewRow();
                dr["Emp_No"] = EmpMap.Emp_No;
                dr["CompCode"] = EmpMap.CompCode;
                dr["EmpId"] = EmpMap.EmpId;
                dt.Rows.Add(dr);
                dsObj.Tables.Add(dt);
                ErrorCode = DALObj.SaveMapping(dsObj);
            }
            catch (Exception)
            {

                throw;
            }

            return ErrorCode;
        }
        public List<SysNavRelListModel> GetEmployeeNavRelList(int EmpId)
        {
            List<SysNavRelListModel> lstEmpMap = new List<SysNavRelListModel>();
            try
            {
                DataSet dsobj = new DataSet();
                AdminDal DALObj = new AdminDal();
                dsobj = DALObj.GetEmployeeNavRelList(EmpId);
                if (dsobj != null)
                {
                    if (dsobj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            SysNavRelListModel MapTemp = new SysNavRelListModel();
                            MapTemp.CompEmpID = dr["Emp_No"].ToString();
                            MapTemp.CompName = dr["CompCode"].ToString();
                            MapTemp.EmpID = Convert.ToInt32(dr["EmpId"].ToString());
                            MapTemp.ConnectionId = Convert.ToInt16(dr["ConnectionId"].ToString());
                            lstEmpMap.Add(MapTemp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstEmpMap;
        }
        public int DeleteEmpMapping(string CompName, int EmpID)
        {
            int errcod = 0;
            try
            {
                AdminDal DALObj = new AdminDal();
                errcod = DALObj.DeleteEmpMapping(CompName, EmpID);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errcod;
        }
        public string GetClientEmpName(string CompName, string CompEmpID, ArrayList ConnInfo)
        {
            string str = "";
            try
            {
                AdminDal DalObj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetClientEmpName(CompName, CompEmpID, ConnInfo);
                if (dsObj != null)
                {
                    str = dsObj.Tables[0].Rows[0]["EmpName"].ToString();
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                str = "Connection Error";
            }

            return str;
        }
        public int SaveUserLogin(string User_Id)
        {
            int errCode = 0;
            try
            {
                AdminDal DALObj = new AdminDal();
                errCode = DALObj.SaveUserLogin(User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveEmployeeDepartment(int EmpId,int DeptId,string User_Id)
        {
            int errCode = 0;
            try
            {
                AdminDal DALObj = new AdminDal();
                errCode = DALObj.SaveEmployeeDepartment(EmpId,DeptId,User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int DeleteEmployeeDepartment(int EmpId, int DeptId)
        {
            int errCode = 0;
            try
            {
                AdminDal DALObj = new AdminDal();
                errCode = DALObj.DeleteEmployeeDepartment(EmpId, DeptId);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #region[RoleReportMapping]
        public RoleRptMapingModel GetReportListByRole(string strRoleID)
        {
            RoleRptMapingModel rlRptMapping = new RoleRptMapingModel();
            try
            {

                AdminDal DalObj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetReportListByRole(strRoleID);
                //rlRptMapping.lstRole = new List<SystemRoles>();
                //foreach(DataRow dr in dsObj.Tables[0].Rows)
                //{
                //    SystemRoles role = new SystemRoles();
                //    role.RoleID = dr["RoleId"].ToString();
                //    role.RoleName = dr["RoleName"].ToString();
                //    rlRptMapping.lstRole.Add(role);
                //}
                rlRptMapping.RoleInfo = new SystemRoles();
                rlRptMapping.RoleInfo.RoleID = dsObj.Tables[0].Rows[0]["RoleId"].ToString();
                rlRptMapping.RoleInfo.RoleName = dsObj.Tables[0].Rows[0]["RoleName"].ToString();

                rlRptMapping.lstRpts = new List<RoleRptMapingModel.DerivedReport>();
                foreach (DataRow dr in dsObj.Tables[1].Rows)
                {
                    RoleRptMapingModel.DerivedReport report = new RoleRptMapingModel.DerivedReport();
                    report.ReportID = dr["ReportId"].ToString();
                    report.ReportName = dr["ReportName"].ToString();
                    rlRptMapping.lstRpts.Add(report);
                }
                rlRptMapping.lstAssignedRpts = new List<RoleRptMapingModel.DerivedReport>();
                foreach (DataRow dr in dsObj.Tables[2].Rows)
                {
                    RoleRptMapingModel.DerivedReport assignedReport = new RoleRptMapingModel.DerivedReport();
                    assignedReport.ReportID = dr["ReportId"].ToString();
                    assignedReport.ReportName = dr["ReportName"].ToString();
                    rlRptMapping.lstAssignedRpts.Add(assignedReport);
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return rlRptMapping;
        }
        public EmployeeCustomerDetail GetCustomerByEmployee(int EmpId)
        {
            EmployeeCustomerDetail EmpCustDtl = new EmployeeCustomerDetail();
            try
            {

                AdminDal DalObj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetEmolyeeCustomerDetails(EmpId);
                //rlRptMapping.lstRole = new List<SystemRoles>();
                //foreach(DataRow dr in dsObj.Tables[0].Rows)
                //{
                //    SystemRoles role = new SystemRoles();
                //    role.RoleID = dr["RoleId"].ToString();
                //    role.RoleName = dr["RoleName"].ToString();
                //    rlRptMapping.lstRole.Add(role);
                //}
                //rlRptMapping.RoleInfo = new SystemRoles();
                //rlRptMapping.RoleInfo.RoleID = dsObj.Tables[0].Rows[0]["RoleId"].ToString();
                //rlRptMapping.RoleInfo.RoleName = dsObj.Tables[0].Rows[0]["RoleName"].ToString();

                EmpCustDtl.lstCustomer = new List<EmployeeCustomerDetail.DerivedCustomer>();
                foreach (DataRow dr in dsObj.Tables[0].Rows)
                {
                    EmployeeCustomerDetail.DerivedCustomer Customer = new EmployeeCustomerDetail.DerivedCustomer();
                    Customer.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                    Customer.CustomerName = dr["CustomerName"].ToString();
                    EmpCustDtl.lstCustomer.Add(Customer);
                }
                EmpCustDtl.lstAssignedCustomer = new List<EmployeeCustomerDetail.DerivedCustomer>();
                foreach (DataRow dr in dsObj.Tables[1].Rows)
                {
                    EmployeeCustomerDetail.DerivedCustomer Assigned = new EmployeeCustomerDetail.DerivedCustomer();
                    Assigned.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                    Assigned.CustomerName = dr["CustomerName"].ToString();
                    EmpCustDtl.lstAssignedCustomer.Add(Assigned);
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return EmpCustDtl;
        }
        public EmployeeVendorDetail GetVendorByEmployee(int EmpId)
        {
            EmployeeVendorDetail EmpvndrtDtl = new EmployeeVendorDetail();
            try
            {

                AdminDal DalObj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetEmolyeeVendorDetails(EmpId);
                //rlRptMapping.lstRole = new List<SystemRoles>();
                //foreach(DataRow dr in dsObj.Tables[0].Rows)
                //{
                //    SystemRoles role = new SystemRoles();
                //    role.RoleID = dr["RoleId"].ToString();
                //    role.RoleName = dr["RoleName"].ToString();
                //    rlRptMapping.lstRole.Add(role);
                //}
                //rlRptMapping.RoleInfo = new SystemRoles();
                //rlRptMapping.RoleInfo.RoleID = dsObj.Tables[0].Rows[0]["RoleId"].ToString();
                //rlRptMapping.RoleInfo.RoleName = dsObj.Tables[0].Rows[0]["RoleName"].ToString();

                EmpvndrtDtl.lstVendor = new List<EmployeeVendorDetail.DerivedVendor>();
                foreach (DataRow dr in dsObj.Tables[0].Rows)
                {
                    EmployeeVendorDetail.DerivedVendor Vendor = new EmployeeVendorDetail.DerivedVendor();
                    Vendor.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                    Vendor.VendorName = dr["VendorName"].ToString();
                    EmpvndrtDtl.lstVendor.Add(Vendor);
                }
                EmpvndrtDtl.lstAssignedVendor = new List<EmployeeVendorDetail.DerivedVendor>();
                foreach (DataRow dr in dsObj.Tables[1].Rows)
                {
                    EmployeeVendorDetail.DerivedVendor Assigned = new EmployeeVendorDetail.DerivedVendor();
                    Assigned.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                    Assigned.VendorName = dr["VendorName"].ToString();
                    EmpvndrtDtl.lstAssignedVendor.Add(Assigned);
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return EmpvndrtDtl;
        }
        public int SaveEmployeeCustomerMapping(EmployeeCustomerDetail EmpCustDtl, int EmpId, string User_Id,int DeptId)
        {
            int iErrorCode = 0;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable("tbl_SysEmployeeCustomer");
                dt.Columns.Add("CustomerId", typeof(System.Int32));
                dt.Columns.Add("EmpId", typeof(System.Int32));
                if (EmpCustDtl.lstEmpCustomerMapping != null)
                {
                    foreach (EmployeeCustomerDetail.EmpCustomerMapping CustmerMap in EmpCustDtl.lstEmpCustomerMapping)
                    {
                        DataRow dr = dt.NewRow();
                        dr["CustomerId"] = CustmerMap.CustomerId;
                        dr["EmpId"] = CustmerMap.EmpId;
                        dt.Rows.Add(dr);
                    }
                }
                ds.Tables.Add(dt);
                AdminDal DalObj = new AdminDal();
                iErrorCode = DalObj.SaveEmployeeCustomer(ds, EmpId, User_Id,DeptId);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return iErrorCode;
        }
        public int SaveEmployeeVendorMapping(EmployeeVendorDetail EmpVndrDtl, int EmpId, string User_Id)
        {
            int iErrorCode = 0;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable("tbl_SysEmployeeVendor");
                dt.Columns.Add("VendorId", typeof(System.Int32));
                dt.Columns.Add("EmpId", typeof(System.Int32));
                if (EmpVndrDtl.lstEmpVendorMapping != null)
                {
                    foreach (EmployeeVendorDetail.EmpVendorMapping VendorMap in EmpVndrDtl.lstEmpVendorMapping)
                    {
                        DataRow dr = dt.NewRow();
                        dr["VendorId"] = VendorMap.VendorId;
                        dr["EmpId"] = VendorMap.EmpId;
                        dt.Rows.Add(dr);
                    }
                }
                ds.Tables.Add(dt);
                AdminDal DalObj = new AdminDal();
                iErrorCode = DalObj.SaveVendorCustomer(ds, EmpId, User_Id);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return iErrorCode;
        }

        public int SaveRoleReportMapping(RoleRptMapingModel rlRptMapping, string RoleID)
        {
            int iErrorCode = 0;
            try
            {
                DataSet dsRoleRpt = new DataSet();
                DataTable dt = new DataTable("tbl_SysRoleReport");
                dt.Columns.Add("ReportId", typeof(System.String));
                dt.Columns.Add("RoleId", typeof(System.String));
                if (rlRptMapping.lstRoleRptMapList != null)
                {
                    foreach (RoleRptMapingModel.RoleReportMapping roleRpt in rlRptMapping.lstRoleRptMapList)
                    {
                        DataRow dr = dt.NewRow();
                        dr["ReportId"] = roleRpt.ReportID;
                        dr["RoleId"] = roleRpt.RoleID;
                        dt.Rows.Add(dr);
                    }
                }
                dsRoleRpt.Tables.Add(dt);
                AdminDal DalObj = new AdminDal();
                iErrorCode = DalObj.SaveRoleReportMapping(dsRoleRpt, RoleID);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return iErrorCode;
        }

        #endregion

        #region System DashBoard Model

        public List<ListDashBoard> DrpDwnDashBoardLst()
        {
            List<ListDashBoard> lstDashBoard = new List<ListDashBoard>();
            try
            {
                DataSet dsObj = new DataSet();
                AdminDal DAlObj = new AdminDal();
                dsObj = DAlObj.GetDashBordDrpDwnList();
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            ListDashBoard DashBoardModel = new ListDashBoard();
                            DashBoardModel.DashboardId = Convert.ToInt16(dr["DashboardId"]);
                            DashBoardModel.Description = dr["Description"].ToString();
                            lstDashBoard.Add(DashBoardModel);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstDashBoard;

        }
        public SysDashBoardModel GetDashBordList(int UserId)
        {
            SysDashBoardModel sysDashBoard = new SysDashBoardModel();
            sysDashBoard.lstUserDashBoard = new List<UserDashboardModel>();
            try
            {
                DataSet dsDashBoard = new DataSet();
                AdminDal DAlObj = new AdminDal();
                dsDashBoard = DAlObj.GetDashBordList(UserId);
                if (dsDashBoard != null)
                {
                    if (dsDashBoard.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsDashBoard.Tables[0].Rows)
                        {
                            UserDashboardModel demo = new UserDashboardModel();
                            demo.UserId = Convert.ToInt32(dr["UserId"]);
                            demo.DashboardId = Convert.ToInt16(dr["DashboardId"]);
                            demo.Sequence = Convert.ToInt16(dr["Sequence"]);
                            demo.Description = dr["Description"].ToString();
                            demo.UserName = dr["UserName"].ToString();
                            sysDashBoard.lstUserDashBoard.Add(demo);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return sysDashBoard;
        }

        public int SaveAssignDashBoard(SysDashBoardModel SysDashBord, int UserId)
        {
            int errCode = 0;
            try
            {
                DataSet dsDashBoard = new DataSet();
                AdminDal DAlObj = new AdminDal();
                dsDashBoard = DAlObj.GetDashBordList(0);
                if (dsDashBoard.Tables.Count > 0)
                {
                    dsDashBoard.Tables[0].Rows.Clear();
                    foreach (UserDashboardModel demo in SysDashBord.lstUserDashBoard)
                    {
                        DataRow dr = dsDashBoard.Tables[0].NewRow();
                        dr["UserId"] = demo.UserId;
                        dr["DashboardId"] = demo.DashboardId;
                        dr["Sequence"] = demo.Sequence;
                        dsDashBoard.Tables[0].Rows.Add(dr);
                    }
                    errCode = DAlObj.SaveAssignDashBoard(dsDashBoard, UserId);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion System Dashboard Model

        #region  Client Application
        public List<ClientAppModel> GetClientAppList(string UserId)
        {
            List<ClientAppModel> lstClientApp = new List<ClientAppModel>();
            try
            {
                AdminDal DALobj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DALobj.GetClientAppList(UserId);
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            ClientAppModel demo = new ClientAppModel();
                            demo.ClientAppId = Convert.ToInt16(dr["ClientAppId"]);
                            demo.AppName = dr["AppName"].ToString();
                            demo.Description = dr["Description"].ToString();
                            demo.LoginName = dr["LoginName"].ToString();
                            demo.Password = dr["Password"].ToString();
                            demo.ModifiedByName = dr["ModifiedByName"].ToString();
                            demo.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                            lstClientApp.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstClientApp;
        }
        public ClientAppModel GetSelecteedClientApp(Int16 ClientAppId)
        {
            ClientAppModel ClientAppModel = new ClientAppModel();
            try
            {
                AdminDal DALobj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DALobj.GetSelectedClientApp(ClientAppId);
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {

                            ClientAppModel.ClientAppId = Convert.ToInt16(dr["ClientAppId"]);
                            ClientAppModel.AppName = dr["AppName"].ToString();
                            ClientAppModel.Description = dr["Description"].ToString();
                            ClientAppModel.LoginName = dr["LoginName"].ToString();
                            ClientAppModel.Password = dr["Password"].ToString();
                            ClientAppModel.ModifiedByName = dr["ModifiedByName"].ToString();
                            ClientAppModel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ClientAppModel;
        }
        public int SaveClientApp(ClientAppModel ClientModel)
        {
            int errCode = 0;
            try
            {
                AdminDal DALobj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DALobj.GetSelectedClientApp(0);
                if (dsObj == null)
                    return 0;
                if (dsObj.Tables.Count > 0)
                {
                    dsObj.Tables[0].Rows.Clear();
                    DataRow dr = dsObj.Tables[0].NewRow();
                    dr["ClientAppId"] = ClientModel.ClientAppId;
                    dr["AppName"] = ClientModel.AppName;
                    dr["Description"] = ClientModel.Description;
                    dr["LoginName"] = ClientModel.LoginName;
                    dr["Password"] = ClientModel.Password;
                    dsObj.Tables[0].Rows.Add(dr);
                    errCode = DALobj.SaveClientApp(dsObj, ClientModel.User_Id);
                }
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion  Client Application

        #region  Client Application

        public List<CompanyModel> GetCompanyList(string UserId)
        {
            List<CompanyModel> lstClientApp = new List<CompanyModel>();
            try
            {
                AdminDal DALobj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DALobj.GetCompanyList(UserId);
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            CompanyModel demo = new CompanyModel();
                            demo.CompId = Convert.ToInt16(dr["CompId"]);
                            demo.Name = dr["Name"].ToString();
                            demo.ShortName = dr["ShortName"].ToString();
                            demo.AddressLine1 = dr["AddressLine1"].ToString();
                            demo.AddressLine2 = dr["AddressLine2"].ToString();
                            demo.City = dr["City"].ToString();
                            demo.State = dr["State"].ToString();
                            demo.Country = dr["Country"].ToString();
                            demo.Pin = dr["Pin"].ToString();
                            demo.TagLine = dr["TagLine"].ToString();
                            demo.Cst = dr["Cst"].ToString();
                            demo.Bst = dr["Bst"].ToString();
                            demo.ServiceTN = dr["ServiceTN"].ToString();
                            //demo.Logo = (Byte[])(dr["Logo"]);
                            lstClientApp.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstClientApp;
        }

        public CompanyModel GetSelectedCompany(Int16 CompId)
        {
            CompanyModel companyModel = new CompanyModel();
            try
            {
                AdminDal DALobj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DALobj.GetSelectedCompany(CompId);
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            companyModel.CompId = Convert.ToInt16(dr["CompId"]);
                            companyModel.Name = dr["Name"].ToString();
                            companyModel.ShortName = dr["ShortName"].ToString();
                            companyModel.AddressLine1 = dr["AddressLine1"].ToString();
                            companyModel.AddressLine2 = dr["AddressLine2"].ToString();
                            companyModel.City = dr["City"].ToString();
                            companyModel.State = dr["State"].ToString();
                            companyModel.Country = dr["Country"].ToString();
                            companyModel.Pin = dr["Pin"].ToString();
                            companyModel.TagLine = dr["TagLine"].ToString();
                            companyModel.Cst = dr["Cst"].ToString();
                            companyModel.Bst = dr["Bst"].ToString();
                            companyModel.ServiceTN = dr["ServiceTN"].ToString();
                            companyModel.Logo = (Byte[])(dr["Logo"]);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return companyModel;
        }
        public int SaveCompany(CompanyModel companyModel,string User_Id)
        {
            int errCode = 0;
            try
            {
                AdminDal DALobj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DALobj.GetSelectedCompany(0);
                if (dsObj == null)
                    return 0;
                if (dsObj.Tables.Count > 0)
                {
                    dsObj.Tables[0].Rows.Clear();
                    DataRow dr = dsObj.Tables[0].NewRow();
                    dr["CompId"] = companyModel.CompId;
                    dr["Name"] = companyModel.Name;
                    dr["ShortName"] = companyModel.ShortName;
                    dr["AddressLine1"] = companyModel.AddressLine1;
                    dr["AddressLine2"] = companyModel.AddressLine2;
                    dr["State"] = companyModel.State;
                    dr["City"] = companyModel.City;
                    dr["Country"] = companyModel.Country;
                    dr["Pin"] = companyModel.Pin;
                    dr["TagLine"] = companyModel.TagLine;
                    dr["Logo"] = companyModel.Logo;
                    dr["Cst"] = companyModel.Cst;
                    dr["Bst"] = companyModel.Bst;
                    dr["ServiceTN"] = companyModel.ServiceTN;

                    dsObj.Tables[0].Rows.Add(dr);
                    errCode = DALobj.SaveCompany(dsObj, User_Id);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion  Client Application

        #region Approval And Review Pendding List

        public List<ApprovalList> PenddingApproverlList(string User_Id)
        {
            List<ApprovalList> ApproveModel = new List<ApprovalList>();
            DataSet dsObj = new DataSet();
            try
            {
                AdminDal DALObj = new AdminDal();
                dsObj = DALObj.PenddingApproverlList(User_Id);
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            ApprovalList demo = new ApprovalList();
                            demo.Description = dr["Description"].ToString();
                            demo.PendingApprovel = Convert.ToInt32(dr["ApprovalCount"].ToString());
                            demo.HyperLink = dr["GotoLink"].ToString();
                            ApproveModel.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ApproveModel;
        }
        public List<ReviewList> PenddingReviewlList(string User_Id)
        {
            List<ReviewList> ApproveModel = new List<ReviewList>();
            DataSet dsObj = new DataSet();
            try
            {
                AdminDal DALObj = new AdminDal();
                dsObj = DALObj.PenddingReviewlList(User_Id);
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            ReviewList demo = new ReviewList();
                            demo.Description = dr["Description"].ToString();
                            demo.PendingReview = Convert.ToInt32(dr["ApprovalCount"].ToString());
                            demo.HyperLink = dr["GotoLink"].ToString();
                            ApproveModel.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ApproveModel;
        }
        #endregion Approval And Review Pendding List

        #region Bank Detail List
        public List<BankListModel> GetBankList(string CompCode)
        {
            List<BankListModel> lstBank = new List<BankListModel>();
            try
            {
                DataSet dsobj = new DataSet();
                AdminDal DALObj = new AdminDal();
                dsobj = DALObj.GetBankList(CompCode);
                if (dsobj != null)
                {
                    if (dsobj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            BankListModel Demo = new BankListModel();
                            Demo.BankId = dr["No_"].ToString();
                            Demo.BankName = dr["Name"].ToString();
                            lstBank.Add(Demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstBank;
        }
        public List<CustomeridModel> GetCustomerIdList(string User_Id)
        {
            List<CustomeridModel> CustList = new List<CustomeridModel>();
            try
            {
                DataSet dsobj = new DataSet();
                AdminDal DALObj = new AdminDal();
                dsobj = DALObj.GetCustomerIdList(User_Id);
                if (dsobj != null)
                {
                    if (dsobj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsobj.Tables[0].Rows)
                        {
                            CustomeridModel Demo = new CustomeridModel();
                            Demo.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                            Demo.CustomerName = dr["CustomerName"].ToString();
                            CustList.Add(Demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return CustList;
        }
        public List<EmployeeBankDetailModel> GetBankListByEmployee(int EmpId)
        {
            EmployeeModel EmpObj = new EmployeeModel();
            EmpObj.EmployeeBankDetailLst = new List<EmployeeBankDetailModel>();

            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.GetSelectedEmployeeList(EmpId);
                if (dsEmp == null)
                    return null;
                if (dsEmp.Tables.Count > 1)
                {

                    if (dsEmp.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsEmp.Tables[1].Rows)
                        {
                            EmployeeBankDetailModel Demo = new EmployeeBankDetailModel();
                            Demo.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            Demo.BankName = dr["BankName"].ToString();
                            Demo.AccountNo = dr["AccountNo"].ToString();
                            Demo.Limit = Convert.ToDouble(dr["Limit"].ToString());
                            Demo.CreatedBy = dr["CreatedBy"].ToString();
                            Demo.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            //Demo.IFSCCode = dr["IFSCCode"].ToString();

                            EmpObj.EmployeeBankDetailLst.Add(Demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EmpObj.EmployeeBankDetailLst;
        }
        public int saveEmployeeBankDetailsInfo(EmployeeBankDetailModel BankDetailModel, string User_Id)
        {

            AdminDal objdal = new AdminDal();
            DataSet ds = new DataSet();
            try
            {
                ds = objdal.GetSelectedEmployeeList(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[1].Rows.Clear();
                    DataRow Dr = ds.Tables[1].NewRow();
                    Dr["EmpId"] = BankDetailModel.EmpId;
                    Dr["AccountNo"] = BankDetailModel.AccountNo;
                    Dr["NewAccountNo"] = BankDetailModel.NewAccountNo;
                    Dr["BankName"] = BankDetailModel.BankName;
                    Dr["Limit"] = BankDetailModel.Limit;
                    ds.Tables[1].Rows.Add(Dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objdal.SaveEmployeeBankDetailsInfo(ds, User_Id);
        }
        public EmployeeBankDetailModel GetSelectedEmployeeBankDetail(int EmpId, string AccountNo)
        {
            EmployeeBankDetailModel EmployeeModel = new EmployeeBankDetailModel();
            try
            {
                AdminDal objdal = new AdminDal();
                DataSet ds = new DataSet();
                ds = objdal.GetSelectedEmployeeBankDetail(EmpId, AccountNo);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            EmployeeModel.EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                            EmployeeModel.BankName = ds.Tables[0].Rows[0]["BankName"].ToString();
                            EmployeeModel.AccountNo = ds.Tables[0].Rows[0]["AccountNo"].ToString();
                            EmployeeModel.Limit = Convert.ToDouble(ds.Tables[0].Rows[0]["Limit"].ToString());
                            EmployeeModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            EmployeeModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EmployeeModel;
        }
        #endregion Bank Detail List

        #region Address Work

        public EmployeeAddressModel GetAddressSelectedList(int addressid)
        {
            EmployeeAddressModel addressModel = new EmployeeAddressModel();

            AdminDal DALobj = new AdminDal();
            try
            {
                DataSet DSObj = new DataSet();
                DSObj = DALobj.GetSelectedAddressForEdit(addressid);
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        addressModel.AddressId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["AddressId"].ToString());
                        addressModel.Line1 = DSObj.Tables[0].Rows[0]["Line1"].ToString();
                        addressModel.Line2 = DSObj.Tables[0].Rows[0]["Line2"].ToString();
                        addressModel.LandMark = DSObj.Tables[0].Rows[0]["LandMark"].ToString();
                        addressModel.Pin = DSObj.Tables[0].Rows[0]["Pin"].ToString();
                        addressModel.Description = DSObj.Tables[0].Rows[0]["Description"].ToString();
                        addressModel.isPrimary = Convert.ToBoolean(DSObj.Tables[0].Rows[0]["isPrimary"].ToString());
                        addressModel.State = DSObj.Tables[0].Rows[0]["State"].ToString();
                        addressModel.Country = DSObj.Tables[0].Rows[0]["Country"].ToString();
                        addressModel.City = DSObj.Tables[0].Rows[0]["City"].ToString();
                        addressModel.CityId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["CityId"].ToString());
                        addressModel.StateId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["StateId"].ToString());
                        addressModel.CountryId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["CountryId"].ToString());
                        addressModel.PhotoPath = DSObj.Tables[0].Rows[0]["PhotoPath"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return addressModel;
        }

        public List<EmployeeCustomer> GetEmployeeCustomerListForDropDown(int EmpId)
        {
            List<EmployeeCustomer> lst = new List<EmployeeCustomer>();
            AdminDal objdal = new AdminDal();

            DataSet dsdata = objdal.GetEmployeeCustomerListForDropDown(EmpId);
            if (dsdata != null)
                if (dsdata.Tables.Count > 0)
                    foreach (DataRow dr in dsdata.Tables[0].Rows)
                    {
                        EmployeeCustomer cust = new EmployeeCustomer();
                        cust.CustomerId = Convert.ToInt32(dr["CustomerId"].ToString());
                        cust.CustomerName = dr["CustomerName"].ToString();
                        cust.EmpId = EmpId;
                        lst.Add(cust);

                    }

            return lst;

        }

        public List<EmployeeVendor> GetEmployeeVendorListForDropDown(int EmpId)
        {
            List<EmployeeVendor> lst = new List<EmployeeVendor>();
            AdminDal objdal = new AdminDal();

            DataSet dsdata = objdal.GetEmployeeVendorListForDropDown(EmpId);
            if (dsdata != null)
                if (dsdata.Tables.Count > 0)
                    foreach (DataRow dr in dsdata.Tables[0].Rows)
                    {
                        EmployeeVendor cust = new EmployeeVendor();
                        cust.VendorId = Convert.ToInt32(dr["VendorId"].ToString());
                        cust.VendorName = dr["VendorName"].ToString();
                        cust.EmpId = EmpId;
                        lst.Add(cust);

                    }

            return lst;

        }

        public int SaveEmployeeCustomer(int EmpId, int CustomerId, string User_Id,int AssignDept)
        {
            AdminDal objdal = new AdminDal();
            return objdal.SaveEmployeeCustomer(EmpId, CustomerId, User_Id, AssignDept);
        }
        public List<SmartSys.BL.EmployeeCustomerDetail.EmpCustomerMapping> GetEmpCustbyDept(int EmpId,int AssignDept)
        {
            List<SmartSys.BL.EmployeeCustomerDetail.EmpCustomerMapping> LstModel = new List<SmartSys.BL.EmployeeCustomerDetail.EmpCustomerMapping>();
            try
            {
                AdminDal objdal = new AdminDal();
                DataSet ds = new DataSet();
                ds= objdal.GetEmpCustbyDept(EmpId,AssignDept);
                if(ds != null)
                {
                    if(ds.Tables.Count > 0)
                    {
                        if(ds.Tables[0].Rows.Count > 0)
                        {
                            foreach(DataRow dr in ds.Tables[0].Rows)
                            {
                                SmartSys.BL.EmployeeCustomerDetail.EmpCustomerMapping Model = new SmartSys.BL.EmployeeCustomerDetail.EmpCustomerMapping();
                                Model.CustomerId = Convert.ToInt32(dr["CustomerId"]);
                                Model.CustomerName = dr["CustomerName"].ToString();
                                LstModel.Add(Model);
                            }
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
        public int SaveEmployeeVendor(int EmpId, int VendorId, string User_Id)
        {
            AdminDal objdal = new AdminDal();
            return objdal.SaveEmployeeVendor(EmpId, VendorId, User_Id);
        }


        public int DeleteEmployeeCustomer(int EmpId, int CustomerId, string User_Id)
        {
            AdminDal objdal = new AdminDal();
            return objdal.DeleteEmployeeCustomer(EmpId, CustomerId, User_Id);
        }

        public int DeleteEmployeeVendor(int EmpId, int VendorId, string User_Id)
        {
            AdminDal objdal = new AdminDal();
            return objdal.DeleteEmployeeVendor(EmpId, VendorId, User_Id);
        }

        public List<EmployeeAddressModel> AddressListByEmployee(int EmpId)
        {

            List<EmployeeAddressModel> addlst = new List<EmployeeAddressModel>();

            try
            {
                AdminDal objdal = new AdminDal();
                DataSet ds = new DataSet();
                ds = objdal.AddressListByEmployee(EmpId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                EmployeeAddressModel objmodel = new EmployeeAddressModel();
                                objmodel.Line1 = dr["Line1"].ToString();
                                objmodel.Line2 = dr["Line2"].ToString();
                                objmodel.LandMark = dr["LandMark"].ToString();
                                objmodel.StateId = Convert.ToInt32(dr["StateId"].ToString());
                                objmodel.CountryId = Convert.ToInt32(dr["CountryId"].ToString());
                                objmodel.CityId = Convert.ToInt32(dr["CityId"].ToString());
                                objmodel.AddressId = Convert.ToInt32(dr["AddressId"].ToString());
                                objmodel.State = dr["State"].ToString();
                                objmodel.City = dr["City"].ToString();
                                objmodel.Country = dr["Country"].ToString();
                                objmodel.Pin = dr["Pin"].ToString();
                                objmodel.isPrimary = Convert.ToBoolean(dr["isPrimary"].ToString());
                                objmodel.Description = dr["Description"].ToString();
                                addlst.Add(objmodel);

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return addlst;
        }
        public int SaveAddressDeatil(EmployeeAddressModel addressmodel, Boolean isPrimary, string Description)
        {
            AdminDal objdal = new AdminDal();
            int ErrCode = 0;
            try
            {
                DataSet DSObj = new DataSet();
                DSObj = objdal.GetSelectedAddressForEdit(0);
                if (DSObj == null)
                    return 0;
                if (DSObj.Tables.Count > 0)
                {
                    DSObj.Tables[0].Rows.Clear();
                    DataRow Dr = DSObj.Tables[0].NewRow();
                    Dr["AddressId"] = addressmodel.AddressId;
                    Dr["Line1"] = addressmodel.Line1;
                    Dr["Line2"] = addressmodel.Line2;
                    Dr["StateId"] = addressmodel.StateId;
                    Dr["CityId"] = addressmodel.CityId;
                    Dr["CountryId"] = addressmodel.CountryId;
                    Dr["Pin"] = addressmodel.Pin;
                    Dr["LandMark"] = addressmodel.LandMark;
                    DSObj.Tables[0].Rows.Add(Dr);
                    ErrCode = objdal.SaveAdressDeatils(DSObj, addressmodel.EmpId, isPrimary, Description, addressmodel.PhotoPath);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;

        }
        public List<EmployeeCountryModel> GetCountryList()
        {
            EmployeeAddressModel addressModel = new EmployeeAddressModel();
            addressModel.LstCountry = new List<EmployeeCountryModel>();
            try
            {
                AdminDal DALObj = new AdminDal();
                DataSet DSObj = new DataSet();
                DSObj = DALObj.GetCountryList();
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in DSObj.Tables[0].Rows)
                        {
                            EmployeeCountryModel demo = new EmployeeCountryModel();
                            demo.CountryId = Convert.ToInt32(dr["Country ID"].ToString());
                            demo.CountryName = dr["Country Name"].ToString();
                            addressModel.LstCountry.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return addressModel.LstCountry;
        }
        public List<EmployeeStateModel> GetStateList(int CountryId)
        {
            EmployeeAddressModel addressModel = new EmployeeAddressModel();
            addressModel.LstState = new List<EmployeeStateModel>();

            try
            {
                AdminDal DALObj = new AdminDal();
                DataSet DSObj = new DataSet();
                DSObj = DALObj.GetStateList(CountryId);
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in DSObj.Tables[0].Rows)
                        {
                            EmployeeStateModel demo = new EmployeeStateModel();
                            demo.StateId = Convert.ToInt32(dr["State ID"].ToString());
                            demo.StateName = dr["State Name"].ToString();
                            addressModel.LstState.Add(demo);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return addressModel.LstState;
        }
        public List<EmployeeCityModel> GetCityList(int State)
        {
            EmployeeAddressModel addressModel = new EmployeeAddressModel();

            addressModel.LstCity = new List<EmployeeCityModel>();
            try
            {
                AdminDal DALObj = new AdminDal();
                DataSet DSObj = new DataSet();
                DSObj = DALObj.GetCityList(State);
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in DSObj.Tables[0].Rows)
                        {
                            EmployeeCityModel demo = new EmployeeCityModel();
                            demo.CityId = Convert.ToInt32(dr["City ID"].ToString());
                            demo.CityName = dr["City Name"].ToString();
                            addressModel.LstCity.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return addressModel.LstCity;
        }
        #endregion Address Work
        public int CreateUserLog(string User_Name)
        {
            int ErrCode = 0;
            try
            {
                AdminDal DALObj = new AdminDal();
                DALObj.CreateUserLog(User_Name);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }

        #region Salary Component
        public List<SalaryComponentmodel> SalaryComponentList(string UserId)
        {
            List<SalaryComponentmodel> lstsalaryComponent = new List<SalaryComponentmodel>();
            try
            {
                DataSet ds = new DataSet();
                AdminDal admindal = new AdminDal();
                ds = admindal.GetSalaryComponentList(UserId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SalaryComponentmodel salarymodel = new SalaryComponentmodel();
                            salarymodel.ComponentId = Convert.ToInt32(dr["ComponentId"].ToString());
                            salarymodel.Fixed = Convert.ToBoolean(dr["Fixed"]);
                            //salarymodel.ParentComponentId = Convert.ToInt32(dr["ParentComponentId"].ToString());
                            salarymodel.VariableRate = Convert.ToDouble(dr["VariableRate"].ToString());
                            salarymodel.Name = dr["Name"].ToString();
                            salarymodel.Name = dr["Name"].ToString();
                            salarymodel.DRCR = dr["DRCR"].ToString();
                            salarymodel.Frequency = dr["Frequency"].ToString();
                            salarymodel.CreatedBy = dr["CreatedBy"].ToString();
                            salarymodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            salarymodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            salarymodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            lstsalaryComponent.Add(salarymodel);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstsalaryComponent;
        }

        public List<EmpSalaryStructureModel> SalaryStructureList(int EmpId)
        {
            List<EmpSalaryStructureModel> lstsalstructure = new List<EmpSalaryStructureModel>();
            try
            {
                DataSet ds = new DataSet();
                AdminDal admindal = new AdminDal();
                ds = admindal.GetSalaryStructureDetail(EmpId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            EmpSalaryStructureModel Salstructure = new EmpSalaryStructureModel();

                            Salstructure.Name = dr["Name"].ToString();
                            Salstructure.VariableRate = Convert.ToDouble(dr["VariableRate"].ToString());
                            Salstructure.Type = dr["Type"].ToString();
                            Salstructure.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            Salstructure.ComponentId = Convert.ToInt32(dr["ComponentId"].ToString());
                            Salstructure.DRCR = dr["DRCR"].ToString();
                            Salstructure.Amount = Convert.ToDouble(dr["Amount"].ToString());
                            if (dr["TotalAmount"].ToString() != "")
                                Salstructure.TotalAmount = Convert.ToDouble(dr["TotalAmount"].ToString());
                            Salstructure.CreatedBy = dr["CreatedBy"].ToString();
                            Salstructure.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            lstsalstructure.Add(Salstructure);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstsalstructure;
        }
        public List<SalaryComponentmodel> ParentComponentList(int ComponentId)
        {
            List<SalaryComponentmodel> lstsalaryComponent = new List<SalaryComponentmodel>();
            try
            {
                DataSet ds = new DataSet();
                AdminDal admindal = new AdminDal();
                ds = admindal.GetParentComponentList(ComponentId, 0);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SalaryComponentmodel salarymodel = new SalaryComponentmodel();
                            salarymodel.ComponentId = Convert.ToInt32(dr["ComponentId"].ToString());
                            salarymodel.Name = dr["Name"].ToString();
                            lstsalaryComponent.Add(salarymodel);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstsalaryComponent;
        }
        public List<SalaryComponentmodel> GetComponentListforDrp(int ComponentId, int EmpId)
        {
            List<SalaryComponentmodel> lstsalaryComponent = new List<SalaryComponentmodel>();
            try
            {
                DataSet ds = new DataSet();
                AdminDal admindal = new AdminDal();
                ds = admindal.GetParentComponentList(ComponentId, EmpId);
                if (ds != null)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            SalaryComponentmodel salarymodel = new SalaryComponentmodel();
                            salarymodel.ComponentId = Convert.ToInt32(dr["ComponentId"].ToString());
                            salarymodel.Name = dr["Name"].ToString();
                            lstsalaryComponent.Add(salarymodel);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstsalaryComponent;
        }
        public int SaveSalaryComponent(SalaryComponentmodel salaryModel, string User_Id, bool Taxable, bool Fixed)
        {
            AdminDal objDAL = new AdminDal();
            DataSet dssalary = new DataSet();
            try
            {
                dssalary = objDAL.GetSelectedSalaryComponent(0);
                if (dssalary == null)
                    return 0;
                if (dssalary.Tables.Count > 0)
                {
                    dssalary.Tables[0].Rows.Clear();
                    DataRow dr = dssalary.Tables[0].NewRow();

                    dr["ComponentId"] = salaryModel.ComponentId;
                    dr["VariableRate"] = salaryModel.VariableRate;
                    dr["ParentComponentId"] = salaryModel.ParentComponentId;
                    dr["ComponentId"] = salaryModel.ComponentId;
                    dr["Name"] = salaryModel.Name;
                    dr["DRCR"] = salaryModel.DRCR;
                    dr["Frequency"] = salaryModel.Frequency;

                    dssalary.Tables[0].Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objDAL.SaveSalary(dssalary, User_Id, Taxable, Fixed);
        }
        public int DeleteEmployeeSalaryStructure(int EmpId, int ComponentId)
        {
            AdminDal objDAL = new AdminDal();
            return objDAL.DeleteEmployeeSalaryStructure(EmpId, ComponentId);
        }
        public int SaveEmployeeSalaryStructure(EmpSalaryStructureModel salaryModel, string User_Id)
        {
            AdminDal objDAL = new AdminDal();

            DataTable dt = new DataTable();
            try
            {

                dt.Columns.Add("EmpId");
                dt.Columns.Add("ComponentId");
                dt.Columns.Add("Amount");


                dt.Rows.Add(salaryModel.EmpId, salaryModel.ComponentId, salaryModel.Amount);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objDAL.saveSalaryStructure(dt, User_Id);
        }

        public SalaryComponentmodel GetSelectedSalaryComponent(int ComponentId)
        {
            SalaryComponentmodel salarymodel = new SalaryComponentmodel();
            try
            {
                DataSet ds = new DataSet();
                AdminDal admindl = new AdminDal();
                ds = admindl.GetSelectedSalaryComponent(ComponentId);
                if (ds != null)
                {
                    salarymodel.ComponentId = Convert.ToInt32(ds.Tables[0].Rows[0]["ComponentId"].ToString());
                    salarymodel.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                    salarymodel.Frequency = ds.Tables[0].Rows[0]["Frequency"].ToString();
                    salarymodel.Fixed = Convert.ToBoolean(ds.Tables[0].Rows[0]["Fixed"].ToString());
                    if (ds.Tables[0].Rows[0]["ParentComponentId"].ToString() != "")
                        salarymodel.ParentComponentId = Convert.ToInt32(ds.Tables[0].Rows[0]["ParentComponentId"].ToString());
                    if (ds.Tables[0].Rows[0]["VariableRate"].ToString() != "")
                        salarymodel.VariableRate = Convert.ToInt32(ds.Tables[0].Rows[0]["VariableRate"].ToString());
                    salarymodel.DRCR = ds.Tables[0].Rows[0]["DRCR"].ToString();
                    salarymodel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                    salarymodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                    salarymodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                    salarymodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                    salarymodel.Taxable = Convert.ToBoolean(ds.Tables[0].Rows[0]["Taxable"].ToString());
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return salarymodel;
        }
        #endregion Salary Component

        #region Sys_Issue_Details
        public List<IssueModel> GetIssueDetailList(string UserId)
        {
            List<IssueModel> LstIssueList = new List<IssueModel>();
            DataSet ds = new DataSet();
            try
            {
                AdminDal objissuedl = new AdminDal();
                ds = objissuedl.GetIssueList(UserId);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        IssueModel issuemodel = new IssueModel();
                        issuemodel.IssueId = Convert.ToInt32(dr["IssueId"].ToString());
                        issuemodel.Title = dr["Title"].ToString();
                        issuemodel.Status = dr["Status"].ToString();
                        issuemodel.CreatedBy = dr["CreatedBy"].ToString();
                        issuemodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        issuemodel.ModifiedBy = dr["ModifiedBy"].ToString();
                        issuemodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        LstIssueList.Add(issuemodel);

                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstIssueList;

        }

        public int SaveIssueDetail(IssueModel objmodel, string User_Id, string Status, string Comments, string Attachment, string TicketType)
        {
            AdminDal issuedal = new AdminDal();
            DataSet ds = new DataSet();

            try
            {
                ds = issuedal.GetselectedIssue(0, User_Id);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].Rows.Clear();
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["IssueId"] = objmodel.IssueId;
                        dr["EstimatedHours"] = objmodel.EstimatedHours;
                        dr["Description"] = objmodel.Description;
                        dr["DevDescription"] = objmodel.DevDescription;
                        dr["Title"] = objmodel.Title;
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return issuedal.SaveIssueDetail(ds, User_Id, Status, Comments, Attachment, TicketType);
        }
        public IssueModel GetSelectedIssueList(int IssueId, string User_Id)
        {
            IssueModel Model = new IssueModel();
            DataSet ds = new DataSet();
            try
            {

                AdminDal objdl = new AdminDal();
                ds = objdl.GetSelectedIssueList(IssueId, User_Id);
                if (ds == null) return null;

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            Model.IssueId = Convert.ToInt32(ds.Tables[0].Rows[0]["IssueId"].ToString());
                            Model.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                            Model.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                            Model.DevDescription = ds.Tables[0].Rows[0]["DevDescription"].ToString();
                            Model.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                            Model.IsAdmin = ds.Tables[0].Rows[0]["IsAdmin"].ToString();
                            if (Model.IsAdmin != "")
                                Model.IsDevloper = 1;                            
                            Model.StatusShortCode = ds.Tables[0].Rows[0]["StatusShortCode"].ToString();
                            Model.Comments = ds.Tables[0].Rows[0]["Comment"].ToString();
                            if (ds.Tables[0].Rows[0]["TicketType"].ToString() != null)
                                Model.TicketType = ds.Tables[0].Rows[0]["TicketType"].ToString();
                            Model.TicketType=Model.TicketType.Replace(",","");
                            Model.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            Model.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            if (ds.Tables[0].Rows[0]["EstimatedHours"].ToString() != "")
                                Model.EstimatedHours = Convert.ToDouble(ds.Tables[0].Rows[0]["EstimatedHours"].ToString());
                            Model.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            Model.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                            Model.to_MailList = ds.Tables[0].Rows[0]["to_MailList"].ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Model;
        }

        public List<CommentDatamodel> GetSelectedProjectCommentList(int IssueId)
        {
            IssueModel issuemodel = new IssueModel();
            issuemodel.LstCommentList = new List<CommentDatamodel>();
            DataSet ds = new DataSet();
            try
            {
                AdminDal Dalobj = new AdminDal();
                ds = Dalobj.GetSelectedProjectCommentList(IssueId);
                if (ds == null) return null;

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                CommentDatamodel Model = new CommentDatamodel();
                                Model.IssueId = Convert.ToInt32(dr["IssueId"].ToString());
                                Model.Comments = dr["Comment"].ToString();
                                Model.CommentedBy = dr["CommentedBy"].ToString();
                                if (dr["AttachmentFileName"].ToString() == "0")
                                {
                                    Model.Attachment = "";
                                }
                                else
                                {
                                    Model.Attachment = dr["AttachmentFileName"].ToString();
                                }

                                Model.CommentDate = Convert.ToDateTime(dr["CommentDate"].ToString());
                                Model.Status = dr["Status"].ToString();
                                issuemodel.LstCommentList.Add(Model);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return issuemodel.LstCommentList;
        }
        public List<IssueModel> GetIssueListForApproval(string User_Id)
        {
            List<IssueModel> LstIssueList = new List<IssueModel>();
            DataSet ds = new DataSet();
            try
            {
                AdminDal objissuedl = new AdminDal();
                ds = objissuedl.GetIssueAprovalList(User_Id);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        IssueModel issuemodel = new IssueModel();
                        issuemodel.IssueId = Convert.ToInt32(dr["IssueId"].ToString());
                        issuemodel.Title = dr["Title"].ToString();
                        issuemodel.Description = dr["Description"].ToString();
                        issuemodel.Status = dr["Status"].ToString();
                        issuemodel.CreatedBy = dr["CreatedBy"].ToString();
                        issuemodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        issuemodel.ModifiedBy = dr["ModifiedBy"].ToString();
                        issuemodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        LstIssueList.Add(issuemodel);

                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstIssueList;
        }
        public List<IssueModel> GetSubordinateIssueAprovalList(string User_Id)
        {
            List<IssueModel> LstIssueList = new List<IssueModel>();
            DataSet ds = new DataSet();
            try
            {
                AdminDal objissuedl = new AdminDal();
                ds = objissuedl.GetSubordinateIssueAprovalList(User_Id);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        IssueModel issuemodel = new IssueModel();
                        issuemodel.IssueId = Convert.ToInt32(dr["IssueId"].ToString());
                        issuemodel.Title = dr["Title"].ToString();
                        issuemodel.Description = dr["Description"].ToString();
                        issuemodel.Status = dr["Status"].ToString();
                        issuemodel.CreatedBy = dr["CreatedBy"].ToString();
                        issuemodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        issuemodel.ModifiedBy = dr["ModifiedBy"].ToString();
                        issuemodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        LstIssueList.Add(issuemodel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstIssueList;
        }
        public int EstimationDone(int IssueId, float EstimationHours, int statusid, string User_Id)
        {
            AdminDal objdal = new AdminDal();
            try
            {
                return objdal.EstimationDone(IssueId, EstimationHours, statusid, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }
        public int updateSysIssues(int IssueId, string User_Id, int statusid, string Comments)
        {
            AdminDal objdal = new AdminDal();
            try
            {
                return objdal.UpdateSysIssue(IssueId, User_Id, statusid, Comments);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }
        public List<IssueModel> GetIssueDetailListHaveCompleteStatus(string User_Id)
        {
            List<IssueModel> LstIssueCompleteList = new List<IssueModel>();
            DataSet ds = new DataSet();
            try
            {
                AdminDal objissuedl = new AdminDal();
                ds = objissuedl.GetIssueListWithCompleteStatus(User_Id);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        IssueModel issuemodel = new IssueModel();
                        issuemodel.IssueId = Convert.ToInt32(dr["IssueId"].ToString());
                        issuemodel.Title = dr["Title"].ToString();
                        issuemodel.Status = dr["Status"].ToString();
                        issuemodel.CreatedBy = dr["CreatedBy"].ToString();
                        issuemodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        issuemodel.ModifiedBy = dr["ModifiedBy"].ToString();
                        issuemodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        LstIssueCompleteList.Add(issuemodel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstIssueCompleteList;
        }

        public List<IssueModel> GetSubordinateIssueDetailListHavePenddingStatus(string User_Id)
        {
            List<IssueModel> LstIssueCompleteList = new List<IssueModel>();
            DataSet ds = new DataSet();
            try
            {
                AdminDal objissuedl = new AdminDal();
                ds = objissuedl.GetSubordinateIssueListWithPenddingStatus(User_Id);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        IssueModel issuemodel = new IssueModel();
                        issuemodel.IssueId = Convert.ToInt32(dr["IssueId"].ToString());
                        issuemodel.Title = dr["Title"].ToString();
                        issuemodel.Status = dr["Status"].ToString();
                        issuemodel.CreatedBy = dr["CreatedBy"].ToString();
                        issuemodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        issuemodel.ModifiedBy = dr["ModifiedBy"].ToString();
                        issuemodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        LstIssueCompleteList.Add(issuemodel);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return LstIssueCompleteList;
        }
        public int updateSysIssuesDetails(int IssueId,string User_Id)
        {
            AdminDal objdal = new AdminDal();
            try
            {
                return objdal.UpdateSysIssueDetails(IssueId, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }
        #endregion Sys_Issue_Details


        #region RoleFeedMapping

        public RoleFeedMapingModel GetFeedListByRole(string strRoleID)
        {
            RoleFeedMapingModel rlFeedMapping = new RoleFeedMapingModel();
            try
            {

                AdminDal DalObj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetFeedListByRole(strRoleID);

                rlFeedMapping.RoleInfo = new SystemRoles();
                rlFeedMapping.RoleInfo.RoleID = dsObj.Tables[0].Rows[0]["RoleId"].ToString();
                rlFeedMapping.RoleInfo.RoleName = dsObj.Tables[0].Rows[0]["RoleName"].ToString();

                rlFeedMapping.lstFeed = new List<RoleFeedMapingModel.DerivedFeed>();
                foreach (DataRow dr in dsObj.Tables[1].Rows)
                {
                    RoleFeedMapingModel.DerivedFeed Feed = new RoleFeedMapingModel.DerivedFeed();
                    Feed.FeedID = dr["FeedID"].ToString();
                    Feed.FeedName = dr["FeedName"].ToString();
                    rlFeedMapping.lstFeed.Add(Feed);

                }
                rlFeedMapping.lstAssignedFeed = new List<RoleFeedMapingModel.DerivedFeed>();
                foreach (DataRow dr in dsObj.Tables[2].Rows)
                {
                    RoleFeedMapingModel.DerivedFeed assignFeed = new RoleFeedMapingModel.DerivedFeed();
                    assignFeed.FeedID = dr["FeedID"].ToString();
                    assignFeed.FeedName = dr["FeedName"].ToString();
                    rlFeedMapping.lstAssignedFeed.Add(assignFeed);

                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return rlFeedMapping;
        }

        public int SaveRoleFeedMapping(RoleFeedMapingModel rlFeedMapping, string RoleID)
        {
            int iErrorCode = 0;

            try
            {
                DataSet dsRoleFeed = new DataSet();
                DataTable dt = new DataTable("tbl_SysRoleFeed");
                dt.Columns.Add("FeedId", typeof(System.String));
                dt.Columns.Add("RoleId", typeof(System.String));
                if (rlFeedMapping.lstRoleFeedMapList != null)
                {
                    foreach (RoleFeedMapingModel.RoleFeedMapping roleFeed in rlFeedMapping.lstRoleFeedMapList)
                    {
                        DataRow dr = dt.NewRow();
                        dr["FeedId"] = roleFeed.FeedID;
                        dr["RoleId"] = roleFeed.RoleID;
                        dt.Rows.Add(dr);
                    }
                }
                dsRoleFeed.Tables.Add(dt);
                AdminDal DalObj = new AdminDal();
                iErrorCode = DalObj.SaveRoleFeedMapping(dsRoleFeed, RoleID);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return iErrorCode;
        }
        #endregion RoleFeedMapping


        #region Address Country State City Entry



        public ContyStateCityModel GetCountryStateCityDetails(int Countryid, int Stateid)
        {
            ContyStateCityModel addressModel = new ContyStateCityModel();
            addressModel.LstCountry = new List<AddCountryModel>();
            addressModel.LstState = new List<AddStateModel>();
            addressModel.LstCity = new List<AddCityModel>();
            try
            {
                AdminDal DalObj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetCountryStateCityDetails(Countryid, Stateid);
                if (dsObj != null)
                {
                    if (dsObj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            AddCountryModel countrymodel = new AddCountryModel();
                            countrymodel.CountryId = Convert.ToInt32(dr["CountryId"].ToString());
                            countrymodel.CountryName = dr["Country"].ToString();
                            addressModel.LstCountry.Add(countrymodel);
                        }
                    }
                    if (dsObj.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[1].Rows)
                        {
                            AddStateModel Statemodel = new AddStateModel();
                            Statemodel.StateId = Convert.ToInt32(dr["StateId"].ToString());
                            Statemodel.StateName = dr["State"].ToString();
                            addressModel.LstState.Add(Statemodel);
                        }
                    }
                    if (dsObj.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[2].Rows)
                        {
                            AddCityModel Citymodel = new AddCityModel();
                            Citymodel.CityId = Convert.ToInt32(dr["CityId"].ToString());
                            Citymodel.CityName = dr["City"].ToString();
                            addressModel.LstCity.Add(Citymodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return addressModel;
        }
        public ContyStateCityModel GetRegionZoneAreaDetails(int Regionid, int Zoneid)
        {
            ContyStateCityModel addressModel = new ContyStateCityModel();
            addressModel.LstRegion = new List<AddRegionModel>();
            addressModel.LstZone = new List<AddZoneModel>();
            addressModel.LstArea = new List<AddAreaModel>();
            try
            {
                AdminDal DalObj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetRegionAreaZoneDetails(Regionid, Zoneid);
                if (dsObj != null)
                {
                    if (dsObj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {

                            AddRegionModel regionmodel = new AddRegionModel();
                            regionmodel.RegionId = Convert.ToInt32(dr["RegionId"].ToString());

                            regionmodel.RegionName = dr["Region"].ToString();
                            addressModel.LstRegion.Add(regionmodel);
                        }
                    }
                    if (dsObj.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[1].Rows)
                        {
                            AddZoneModel Zonemodel = new AddZoneModel();
                            Zonemodel.ZoneId = Convert.ToInt32(dr["ZoneId"].ToString());
                            Zonemodel.ZoneName = dr["Zone"].ToString();
                            addressModel.LstZone.Add(Zonemodel);
                        }
                    }
                    if (dsObj.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[2].Rows)
                        {
                            AddAreaModel Areamodel = new AddAreaModel();
                            Areamodel.AreaId = Convert.ToInt32(dr["AreaId"].ToString());
                            Areamodel.AreaName = dr["Area"].ToString();
                            addressModel.LstArea.Add(Areamodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return addressModel;
        }

        public ContyStateCityModel GetZoneStateList()
        {
            ContyStateCityModel addressModel = new ContyStateCityModel();
            addressModel.LstZone = new List<AddZoneModel>();
            addressModel.LstState = new List<AddStateModel>();
            try
            {
                AdminDal DalObj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetZoneStateList(0);
                if (dsObj != null)
                {
                    if (dsObj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {

                            AddStateModel StateModel = new AddStateModel();
                            StateModel.StateId = Convert.ToInt32(dr["StateId"].ToString());
                            StateModel.StateName = dr["StateName"].ToString();
                            addressModel.LstState.Add(StateModel);
                        }
                    }
                    if (dsObj.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[1].Rows)
                        {
                            AddZoneModel Zonemodel = new AddZoneModel();
                            Zonemodel.ZoneId = Convert.ToInt32(dr["ZoneId"].ToString());
                            Zonemodel.ZoneName = dr["Zone"].ToString();
                            addressModel.LstZone.Add(Zonemodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return addressModel;
        }
        public ContyStateCityModel GetZoneAllocatedStateList(int Zoneid)
        {
            ContyStateCityModel addressModel = new ContyStateCityModel();
            addressModel.LstState = new List<AddStateModel>();
            try
            {
                AdminDal DalObj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetZoneStateList(Zoneid);
                if (dsObj != null)
                {

                    if (dsObj.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[2].Rows)
                        {
                            AddStateModel Statemodel = new AddStateModel();
                            Statemodel.StateId = Convert.ToInt32(dr["StateId"].ToString());
                            Statemodel.StateName = dr["StateName"].ToString();
                            addressModel.LstState.Add(Statemodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return addressModel;
        }
        public ContyStateCityModel GetCountryStateCityList()
        {
            ContyStateCityModel addressModel = new ContyStateCityModel();
            addressModel.LstCountry = new List<AddCountryModel>();
            addressModel.LstState = new List<AddStateModel>();
            addressModel.LstCity = new List<AddCityModel>();
            addressModel.LstRegion = new List<AddRegionModel>();
            addressModel.LstZone = new List<AddZoneModel>();
            addressModel.LstArea = new List<AddAreaModel>();
            try
            {
                AdminDal DalObj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetCountryStateCityList();
                if (dsObj != null)
                {
                    if (dsObj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            AddCountryModel countrymodel = new AddCountryModel();
                            countrymodel.CountryId = Convert.ToInt32(dr["CountryId"].ToString());
                            countrymodel.CountryName = dr["Country"].ToString();
                            countrymodel.Checked = dr["Checked"].ToString();
                            addressModel.LstCountry.Add(countrymodel);
                        }
                    }
                    if (dsObj.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[1].Rows)
                        {
                            AddStateModel Statemodel = new AddStateModel();
                            Statemodel.StateId = Convert.ToInt32(dr["StateId"].ToString());
                            Statemodel.StateName = dr["State"].ToString();
                            Statemodel.CountryName = dr["Country"].ToString();
                            Statemodel.Checked = dr["Checked"].ToString();
                            addressModel.LstState.Add(Statemodel);
                        }
                    }
                    if (dsObj.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[2].Rows)
                        {
                            AddCityModel Citymodel = new AddCityModel();
                            Citymodel.CityId = Convert.ToInt32(dr["CityId"].ToString());
                            Citymodel.CityName = dr["City"].ToString();
                            Citymodel.Checked = dr["Checked"].ToString();
                            Citymodel.StateName = dr["State"].ToString();
                            addressModel.LstCity.Add(Citymodel);
                        }
                    }
                }
                dsObj = DalObj.GetRegionZoneAreaList();
                if (dsObj != null)
                {
                    if (dsObj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            AddRegionModel regionmodel = new AddRegionModel();
                            regionmodel.RegionId = Convert.ToInt32(dr["RegionId"].ToString());
                            regionmodel.RegionName = dr["Region"].ToString();
                            regionmodel.CreatedBy = dr["CreatedBy"].ToString();
                            regionmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());

                            addressModel.LstRegion.Add(regionmodel);
                        }
                    }
                    if (dsObj.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[1].Rows)
                        {
                            AddZoneModel Zonemodel = new AddZoneModel();
                            Zonemodel.ZoneId = Convert.ToInt32(dr["ZoneId"].ToString());
                            Zonemodel.ZoneName = dr["Zone"].ToString();
                            Zonemodel.RegionName = dr["Region"].ToString();
                            Zonemodel.CreatedBy = dr["CreatedBy"].ToString();
                            Zonemodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            addressModel.LstZone.Add(Zonemodel);
                        }
                    }
                    if (dsObj.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[2].Rows)
                        {
                            AddAreaModel Areamodel = new AddAreaModel();
                            Areamodel.AreaId = Convert.ToInt32(dr["AreaId"].ToString());
                            Areamodel.AreaName = dr["Area"].ToString();

                            Areamodel.ZoneName = dr["zone"].ToString();
                            Areamodel.CreatedBy = dr["CreatedBy"].ToString();
                            Areamodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            addressModel.LstArea.Add(Areamodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return addressModel;
        }
        public ContyStateCityModel GetRegionZoneAreaList()
        {
            ContyStateCityModel addressModel = new ContyStateCityModel();
            addressModel.LstRegion = new List<AddRegionModel>();
            addressModel.LstZone = new List<AddZoneModel>();
            addressModel.LstArea = new List<AddAreaModel>();
            try
            {
                AdminDal DalObj = new AdminDal();
                DataSet dsObj = new DataSet();
                dsObj = DalObj.GetRegionZoneAreaList();
                if (dsObj != null)
                {
                    if (dsObj.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            AddRegionModel regionmodel = new AddRegionModel();
                            regionmodel.RegionId = Convert.ToInt32(dr["RegionId"].ToString());
                            regionmodel.RegionName = dr["Region"].ToString();

                            addressModel.LstRegion.Add(regionmodel);
                        }
                    }
                    if (dsObj.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[1].Rows)
                        {
                            AddZoneModel Zonemodel = new AddZoneModel();
                            Zonemodel.ZoneId = Convert.ToInt32(dr["ZoneId"].ToString());
                            Zonemodel.ZoneName = dr["Zone"].ToString();
                            Zonemodel.RegionName = dr["Region"].ToString();

                            addressModel.LstZone.Add(Zonemodel);
                        }
                    }
                    if (dsObj.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[2].Rows)
                        {
                            AddAreaModel Areamodel = new AddAreaModel();
                            Areamodel.AreaId = Convert.ToInt32(dr["AreaId"].ToString());
                            Areamodel.AreaName = dr["Area"].ToString();

                            Areamodel.ZoneName = dr["zone"].ToString();
                            addressModel.LstArea.Add(Areamodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return addressModel;
        }
        public int SaveRegionStateMap(RegionStateMapping admodel, int Region, string User_Id)
        {
            int iErrorCode = 0;

            try
            {
                DataSet dsRoleFeed = new DataSet();
                DataTable dt = new DataTable("DW_AddRegionStateMap");
                dt.Columns.Add("RegionId", typeof(System.String));
                dt.Columns.Add("StateId", typeof(System.String));
                if (admodel.LstRegionAreamapping != null)
                {
                    foreach (RegionStateMapping.RegionstateMapdetails StateMap in admodel.LstRegionAreamapping)
                    {
                        DataRow dr = dt.NewRow();
                        dr["RegionId"] = Region;
                        dr["StateId"] = StateMap.StateId;
                        dt.Rows.Add(dr);
                    }
                }
                dsRoleFeed.Tables.Add(dt);
                AdminDal DalObj = new AdminDal();
                iErrorCode = DalObj.SaveRegionStatemap(dsRoleFeed, Region, User_Id);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return iErrorCode;
        }


        public ContyStateCityModel GetListOfStatewise(int Region)
        {
            ContyStateCityModel model = new ContyStateCityModel();
            model.LstState = new List<AddStateModel>();
            model.assigned = new List<AddStateModel>();
            model.AllocatedList = new List<ContyStateCityModel>();
            try
            {
                DataSet ds = new DataSet();
                AdminDal admindal = new AdminDal();
                ds = admindal.GetStatewiseList(Region);
                if (ds != null)
                {

                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        AddStateModel objmodel = new AddStateModel();
                        objmodel.StateId = Convert.ToInt32(dr["StateId"].ToString());
                        objmodel.StateName = dr["StateName"].ToString();
                        model.LstState.Add(objmodel);
                    }
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        AddStateModel objmodel = new AddStateModel();
                        objmodel.StateId = Convert.ToInt32(dr["StateId"].ToString());
                        objmodel.StateName = dr["StateName"].ToString();
                        objmodel.Region = Convert.ToInt32(dr["RegionId"].ToString());
                        model.assigned.Add(objmodel);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
        public AddRegionModel GetCityAreamappingList(int AreaId)
        {
            AddRegionModel model = new AddRegionModel();
            model.AllocatedList = new List<AddRegionModel>();
            model.TotalList = new List<AddRegionModel>();
            model.AreaList = new List<AddAreaModel>();
            try
            {
                DataSet ds = new DataSet();
                AdminDal admindal = new AdminDal();
                ds = admindal.GetAreaCityMappingList(AreaId);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        AddRegionModel objmodel = new AddRegionModel();
                        objmodel.CityId = Convert.ToInt32(dr["CityId"].ToString());
                        objmodel.City = dr["City"].ToString();
                        model.TotalList.Add(objmodel);
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        AddRegionModel objmodel = new AddRegionModel();
                        objmodel.CityId = Convert.ToInt32(dr["CityId"].ToString());
                        objmodel.City = dr["City"].ToString();
                        objmodel.AreaId = Convert.ToInt32(dr["AreaId"].ToString());
                        model.AllocatedList.Add(objmodel);
                    }
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        AddAreaModel objmodel = new AddAreaModel();
                        objmodel.AreaId = Convert.ToInt32(dr["AreaId"].ToString());
                        objmodel.AreaName = dr["Area"].ToString();
                        model.AreaList.Add(objmodel);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
        public int SaveAreaCityMapping(AreaCityMappingdetail CityAreaDtl, int AreaId, string User_Id)
        {
            int iErrorCode = 0;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable("DW_AddAreaCityMap");
                dt.Columns.Add("CityId", typeof(System.Int32));
                dt.Columns.Add("AreaId", typeof(System.Int32));
                if (CityAreaDtl.LstAreaCitymapping != null)
                {
                    foreach (AreaCityMappingdetail.AreaCityDetail AreaCityMap in CityAreaDtl.LstAreaCitymapping)
                    {
                        DataRow dr = dt.NewRow();
                        dr["CityId"] = AreaCityMap.CityId;
                        dr["AreaId"] = AreaCityMap.AreaId;
                        dt.Rows.Add(dr);
                    }
                }
                ds.Tables.Add(dt);
                AdminDal DalObj = new AdminDal();
                iErrorCode = DalObj.SaveAreaCityMapping(ds, AreaId, User_Id);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return iErrorCode;
        }

        public int SaveRegionZoneAreaDetails(string Region, int Regionid, string Area, string Zone, int Zoneid, string Addvalue, string User_Id)
        {
            int ErrCode = 0;
            try
            {
                AdminDal DalObj = new AdminDal();
                ErrCode = DalObj.SaveRegionZoneAreaDetails(Region, Regionid, Area, Zone, Zoneid, Addvalue, User_Id);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public int DeleteCountryStateCity(int Countryid, string Addvalue)
        {
            int errcode = 0;
            try
            {

                AdminDal objdal = new AdminDal();
                errcode = objdal.DeleteCountryStateCityDetail(Countryid, Addvalue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }

        public int SaveCountryStateCityDetails(string Country, int Countryid, string City, string state, int Stateid, string Addvalue)
        {
            int ErrCode = 0;
            try
            {
                AdminDal DalObj = new AdminDal();
                ErrCode = DalObj.SaveCountryStateCityDetails(Country, Countryid, City, state, Stateid, Addvalue);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }

        public int SaveAddZoneStateMap(ContyStateCityModel Model, string User_Id, int ZoneId)
        {
            int errCode = 0;
            try
            {
                AdminDal DALObj = new AdminDal();
                DataSet dsObj = new DataSet();
                DataTable dt = new DataTable("tbl_Zone");
                dt.Columns.Add("ZoneId", typeof(System.Int32));
                dt.Columns.Add("StateId", typeof(System.Int32));
                foreach (AddZoneModel ZoneModel in Model.LstZone)
                {
                    DataRow dr = dt.NewRow();
                    dr["ZoneId"] = ZoneModel.ZoneId;
                    dr["StateId"] = ZoneModel.StateId;
                    dt.Rows.Add(dr);
                }
                dsObj.Tables.Add(dt);
                errCode = DALObj.SaveAddZoneStateMap(dsObj, User_Id, ZoneId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        #endregion Address Country State City Entry

        #region CompLibrary

        public List<CompanyLibraryModel> GetListOfCompLibrary(string UserId)
        {
            CompanyLibraryModel librarymodel = new CompanyLibraryModel();
            List<CompanyLibraryModel> lstcomplibrary = new List<CompanyLibraryModel>();
            try
            {
                DataSet ds = new DataSet();
                AdminDal admindal = new AdminDal();
                ds = admindal.GetCompLibraryList(UserId);
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        CompanyLibraryModel libmodel = new CompanyLibraryModel();
                        libmodel.DocId = Convert.ToInt32(dr["DocId"].ToString());
                        libmodel.CompCode = dr["CompCode"].ToString();
                        libmodel.DocName = dr["DocName"].ToString();
                        libmodel.Description = dr["Description"].ToString();
                        libmodel.FileName = dr["FileName"].ToString();
                        libmodel.CreatedBy = dr["CreatedBy"].ToString();
                        libmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        libmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                        libmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        lstcomplibrary.Add(libmodel);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstcomplibrary;
        }

        public List<CompanyLibraryModel> GetListOfCompanyForDropDown()
        {

            List<CompanyLibraryModel> lstcomp = new List<CompanyLibraryModel>();
            try
            {
                DataSet ds = new DataSet();
                AdminDal admindal = new AdminDal();
                ds = admindal.GetDropDownCompCodeList();
                if (ds != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        CompanyLibraryModel objmodel = new CompanyLibraryModel();
                        objmodel.CompCode = dr["CompCode"].ToString();
                        objmodel.CompanyName = dr["CompanyName"].ToString();

                        lstcomp.Add(objmodel);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstcomp;
        }

        public int SaveLibraryDetail(CompanyLibraryModel objmodel, string User_Id, string Attachment)
        {
            AdminDal admindal = new AdminDal();
            DataSet ds = new DataSet();

            try
            {
                ds = admindal.GetselectedLibrary(0);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].Rows.Clear();
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["DocId"] = objmodel.DocId;
                        dr["CompCode"] = objmodel.CompCode;
                        dr["DocName"] = objmodel.DocName;
                        dr["Description"] = objmodel.Description;



                        ds.Tables[0].Rows.Add(dr);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return admindal.SaveCompLibraryDetail(ds, User_Id, Attachment);
        }
        public CompanyLibraryModel GetSelectedLibraryList(int DocId)
        {
            CompanyLibraryModel libmodel = new CompanyLibraryModel();
            DataSet ds = new DataSet();
            try
            {

                AdminDal objdl = new AdminDal();
                ds = objdl.GetselectedLibrary(DocId);
                if (ds == null) return null;

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            libmodel.DocId = Convert.ToInt32(ds.Tables[0].Rows[0]["DocId"].ToString());
                            libmodel.DocName = ds.Tables[0].Rows[0]["DocName"].ToString();
                            libmodel.CompCode = ds.Tables[0].Rows[0]["CompCode"].ToString();
                            libmodel.CompanyName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                            libmodel.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                            libmodel.FileName = ds.Tables[0].Rows[0]["FileName"].ToString();
                            libmodel.Email = ds.Tables[0].Rows[0]["Email"].ToString();
                            libmodel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            libmodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            libmodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            libmodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return libmodel;
        }
        #endregion CompLibrary

        #region Assets
        public List<SysAssetsModel> SysAssetsList(string UserId)
        {
            List<SysAssetsModel> lstSysAssets = new List<SysAssetsModel>();
            try
            {
                DataSet ds = new DataSet();
                AdminDal admindal = new AdminDal();
                ds = admindal.SysAssetsList(0, UserId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SysAssetsModel SysAssetsmodel = new SysAssetsModel();
                            SysAssetsmodel.AssetId = Convert.ToInt32(dr["AssetId"].ToString());
                            SysAssetsmodel.AssetType = dr["AssetType"].ToString();
                            SysAssetsmodel.AssetTypeId = Convert.ToInt32(dr["AssetTypeId"].ToString());
                            SysAssetsmodel.AssetName = dr["AssetName"].ToString();
                            SysAssetsmodel.Description = dr["Description"].ToString();

                            if (dr["AssetInDate"].ToString() != "")
                            {
                                SysAssetsmodel.AssetInDate = Convert.ToDateTime(dr["AssetInDate"].ToString());
                            }


                            SysAssetsmodel.ManufacturerDetails = dr["ManufacturerDetails"].ToString();

                            if (dr["Quantity"].ToString() != "")
                            {
                                SysAssetsmodel.Quantity = Convert.ToDouble(dr["Quantity"].ToString());
                            }
                            if (dr["TotalQty"].ToString() != "")
                            {
                                SysAssetsmodel.TotalQty = Convert.ToDouble(dr["TotalQty"].ToString());
                            }

                            if (dr["Cost"].ToString() != "")
                            {
                                SysAssetsmodel.Cost = Convert.ToDouble(dr["Cost"].ToString());
                            }
                            if (dr["DepRate"].ToString() != "")
                            {
                                SysAssetsmodel.DepRate = Convert.ToDouble(dr["DepRate"].ToString());
                            }
                            if (dr["OfficeLocationId"].ToString() != "")
                            {
                                SysAssetsmodel.CityName = dr["OfficeLocationId"].ToString();
                            }

                            if (dr["LocationId"].ToString() != "")
                            {
                                SysAssetsmodel.countryName = dr["LocationId"].ToString();
                            }
                            if (dr["DisposalDate"].ToString() != "")
                            {
                                SysAssetsmodel.DisposalDate = Convert.ToDateTime(dr["DisposalDate"].ToString());
                            }
                            if (dr["DisposalValue"].ToString() != "")
                            {
                                SysAssetsmodel.DisposalValue = Convert.ToInt32(dr["DisposalValue"].ToString());
                            }

                            SysAssetsmodel.AssetAcRef = dr["AssetAcRef"].ToString();




                            //if(dr["AllocatedTo"].ToString()!="")
                            //{
                            //    SysAssetsmodel.AllocatedTo = dr["AllocatedTo"].ToString();
                            //}
                            //else
                            //{
                            //    SysAssetsmodel.AllocatedTo = "Not Allocated";
                            //}

                            SysAssetsmodel.CreatedBy = dr["CreatedBy"].ToString();
                            SysAssetsmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDare"].ToString());
                            SysAssetsmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            SysAssetsmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            lstSysAssets.Add(SysAssetsmodel);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstSysAssets;
        }

        public List<SysAssetsModel> SysAssetsListDrpDwn(string UserId)
        {
            List<SysAssetsModel> lstSysAssets = new List<SysAssetsModel>();
            try
            {
                DataSet ds = new DataSet();
                AdminDal admindal = new AdminDal();
                ds = admindal.SysAssetsList(0, UserId);
                if (ds != null)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            SysAssetsModel SysAssetsmodel = new SysAssetsModel();
                            SysAssetsmodel.AssetId = Convert.ToInt32(dr["AssetId"].ToString());
                            SysAssetsmodel.AssetName = dr["AssetName"].ToString();
                            lstSysAssets.Add(SysAssetsmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstSysAssets;

        }
        public List<SysAssetsModel> SysAssetsListDrpDwnEdit(int AssignmentId,string UserId)
        {
            List<SysAssetsModel> lstSysAssets = new List<SysAssetsModel>();
            try
            {
                DataSet ds = new DataSet();
                AdminDal admindal = new AdminDal();
                ds = admindal.SysAssetsList(AssignmentId, UserId);
                if (ds != null)
                {
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {


                            SysAssetsModel SysAssetsmodel = new SysAssetsModel();
                            SysAssetsmodel.AssetId = Convert.ToInt32(dr["AssetId"].ToString());
                            SysAssetsmodel.AssetName = dr["AssetName"].ToString();
                            lstSysAssets.Add(SysAssetsmodel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstSysAssets;

        }


        public List<SysAssetsModel> SysAllAssetsListByEmployee()
        {
            List<SysAssetsModel> lstSysAssets = new List<SysAssetsModel>();
            try
            {
                DataSet ds = new DataSet();
                AdminDal admindal = new AdminDal();
                ds = admindal.SysAllAssetsListByEmployee();
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SysAssetsModel SysAssetsmodel = new SysAssetsModel();
                            SysAssetsmodel.AssetId = Convert.ToInt32(dr["AssetId"].ToString());
                            SysAssetsmodel.AssetName = dr["AssetName"].ToString();
                            lstSysAssets.Add(SysAssetsmodel);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstSysAssets;
        }



        public List<SyaAssetTypeModel> DropdownSysAssetType(string UserId)
        {
            List<SyaAssetTypeModel> lstSysAssetType = new List<SyaAssetTypeModel>();
            AdminDal objdal = new AdminDal();

            DataSet dsSysDept = objdal.GetSysAssetTypelist(UserId);
            if (dsSysDept != null)
            {
                if (dsSysDept.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsSysDept.Tables[0].Rows)
                    {
                        SyaAssetTypeModel objmodel = new SyaAssetTypeModel();
                        objmodel.AssetTypeId = Convert.ToInt32(dr["AssetTypeId"].ToString());
                        objmodel.ParentAssetTypeId = Convert.ToInt32(dr["AssetTypeId"].ToString());
                        objmodel.AssetType = dr["AssetType"].ToString();
                        objmodel.Description = dr["Description"].ToString();
                        lstSysAssetType.Add(objmodel);

                    }
                }
            }
            return lstSysAssetType;
        }


        public SysAssetsModel GetSelectedSysAssetList(int assetId)
        {
            SysAssetsModel assetModel = new SysAssetsModel();

            AdminDal DALobj = new AdminDal();
            try
            {
                DataSet DSObj = new DataSet();
                DSObj = DALobj.GetSelectedSysAssetForEdit(assetId);
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        assetModel.AssetId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["AssetId"].ToString());
                        assetModel.AssetTypeId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["AssetTypeId"].ToString());
                        assetModel.AssetName = DSObj.Tables[0].Rows[0]["AssetName"].ToString();
                        //  assetModel.AssetType = DSObj.Tables[0].Rows[0]["AssetType"].ToString();


                        if (DSObj.Tables[0].Rows[0]["AssetInDate"].ToString() != "")
                        {
                            assetModel.AssetInDate = Convert.ToDateTime(DSObj.Tables[0].Rows[0]["AssetInDate"].ToString());
                        }

                        if (DSObj.Tables[0].Rows[0]["Quantity"].ToString() != "")
                        {
                            assetModel.Quantity = Convert.ToDouble(DSObj.Tables[0].Rows[0]["Quantity"].ToString());
                        }


                        if (DSObj.Tables[0].Rows[0]["TotalQty"].ToString() != "")
                        {
                            assetModel.TotalQty = Convert.ToDouble(DSObj.Tables[0].Rows[0]["TotalQty"].ToString());
                        }

                        if (DSObj.Tables[0].Rows[0]["Cost"].ToString() != "")
                        {
                            assetModel.Cost = Convert.ToDouble(DSObj.Tables[0].Rows[0]["Cost"].ToString());
                        }
                        if (DSObj.Tables[0].Rows[0]["DepRate"].ToString() != "")
                        {
                            assetModel.DepRate = Convert.ToDouble(DSObj.Tables[0].Rows[0]["DepRate"].ToString());
                        }
                        if (DSObj.Tables[0].Rows[0]["OfficeLocationId"].ToString() != "")
                        {
                            assetModel.OfficeLocationId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["OfficeLocationId"].ToString());
                        }

                        if (DSObj.Tables[0].Rows[0]["DisposalDate"].ToString() != "")
                        {
                            assetModel.DisposalDate = Convert.ToDateTime(DSObj.Tables[0].Rows[0]["DisposalDate"].ToString());
                        }
                        if (DSObj.Tables[0].Rows[0]["DisposalValue"].ToString() != "")
                        {
                            assetModel.DisposalValue = Convert.ToDouble(DSObj.Tables[0].Rows[0]["DisposalValue"].ToString());
                        }
                        if (DSObj.Tables[0].Rows[0]["LocationId"].ToString() != "")
                        {
                            assetModel.LocationId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["LocationId"].ToString());
                        }

                        assetModel.ManufacturerDetails = DSObj.Tables[0].Rows[0]["ManufacturerDetails"].ToString();




                        if (DSObj.Tables[0].Rows[0]["Photo"].ToString() != "")
                            assetModel.Photo = (Byte[])(DSObj.Tables[0].Rows[0]["Photo"]);

                        assetModel.AssetAcRef = DSObj.Tables[0].Rows[0]["AssetAcRef"].ToString();

                        assetModel.CreatedBy = DSObj.Tables[0].Rows[0]["CreatedBy"].ToString();
                        assetModel.CreatedDate = Convert.ToDateTime(DSObj.Tables[0].Rows[0]["CreatedDare"].ToString());
                        assetModel.Description = DSObj.Tables[0].Rows[0]["Description"].ToString();
                        assetModel.ModifiedBy = DSObj.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        assetModel.ModifiedDate = Convert.ToDateTime(DSObj.Tables[0].Rows[0]["ModifiedDate"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return assetModel;
        }


        public int SaveSysAsset(SysAssetsModel assetmodel, string User_ID)
        {
            AdminDal objdal = new AdminDal();
            int ErrCode = 0;
            try
            {
                DataSet DSObj = new DataSet();
                DSObj = objdal.GetSelectedSysAssetForEdit(0);
                if (DSObj == null)
                    return 0;
                if (DSObj.Tables.Count > 0)
                {
                    DSObj.Tables[0].Rows.Clear();
                    DataRow Dr = DSObj.Tables[0].NewRow();
                    Dr["AssetId"] = assetmodel.AssetId;
                    Dr["AssetName"] = assetmodel.AssetName;

                    Dr["Description"] = assetmodel.Description;

                    Dr["AssetTypeId"] = assetmodel.AssetTypeId;
                    if (assetmodel.AssetInDate != null)
                    {
                        Dr["AssetInDate"] = assetmodel.AssetInDate;
                    }
                    Dr["ManufacturerDetails"] = assetmodel.ManufacturerDetails;
                    Dr["Photo"] = assetmodel.Photo;

                    Dr["TotalQty"] = assetmodel.TotalQty;
                    Dr["Cost"] = assetmodel.Cost;
                    Dr["DepRate"] = assetmodel.DepRate;
                    if (assetmodel.OfficeLocationId != null)
                    {
                        Dr["OfficeLocationId"] = assetmodel.OfficeLocationId;
                    }

                    if (assetmodel.LocationId != null)
                    {
                        Dr["LocationId"] = assetmodel.LocationId;
                    }

                    Dr["AssetAcRef"] = assetmodel.AssetAcRef;
                    if (assetmodel.DisposalDate != null)
                    {
                        Dr["DisposalDate"] = assetmodel.DisposalDate;
                    }

                    Dr["DisposalValue"] = assetmodel.DisposalValue;


                    DSObj.Tables[0].Rows.Add(Dr);
                    ErrCode = objdal.SaveSysAssetDetails(DSObj, User_ID);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;

        }

        public int SaveAssetType(SyaAssetTypeModel assetTypemodel,string User_Id)
        {
            AdminDal objdal = new AdminDal();
            int ErrCode = 0;
            try
            {
                DataSet DSObj = new DataSet();
                DSObj = objdal.GetSelectedSysAssetTypeForEdit(0);
                if (DSObj == null)
                    return 0;
                if (DSObj.Tables.Count > 0)
                {
                    DSObj.Tables[0].Rows.Clear();
                    DataRow Dr = DSObj.Tables[0].NewRow();
                    Dr["AssetTypeId"] = assetTypemodel.AssetTypeId;
                    Dr["ParentAssetTypeId"] = assetTypemodel.ParentAssetTypeId;
                    Dr["AssetType"] = assetTypemodel.AssetType;
                    Dr["Description"] = assetTypemodel.Description;
                    DSObj.Tables[0].Rows.Add(Dr);
                    ErrCode = objdal.SaveAssetTypeDetails(DSObj, User_Id);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;

        }
        public SyaAssetTypeModel GetSelectedSysAssetType(int AssetTypeId)
        {
            SyaAssetTypeModel AssetTypemodel = new SyaAssetTypeModel();

            AdminDal DALobj = new AdminDal();
            try
            {
                DataSet DSObj = new DataSet();
                DSObj = DALobj.GetSelectedSysAssetTypeForEdit(AssetTypeId);
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        AssetTypemodel.AssetTypeId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["AssetTypeId"].ToString());
                        AssetTypemodel.AssetType = DSObj.Tables[0].Rows[0]["AssetType"].ToString();
                        AssetTypemodel.Description = DSObj.Tables[0].Rows[0]["Description"].ToString();
                        AssetTypemodel.ParentAssetTypeId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["ParentAssetTypeId"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AssetTypemodel;
        }

        public int DeleteSysAssetDetails(int AssetId,string User_Id)
        {
            int errcode = 0;
            try
            {
                AdminDal objdal = new AdminDal();
                errcode = objdal.DeleteSysAssetsDetails(AssetId, User_Id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }

        public SysAssetAllocationModel SysAssetAllocationType(string User_Id)
        {

            SysAssetAllocationModel model = new SysAssetAllocationModel();
            model.AssetAllocationList = new List<SysAssetAllocationModel>();
            model.ApprovalList = new List<SysAssetAllocationModel>();

            AdminDal objdal = new AdminDal();

            DataSet dsSysAllocation = objdal.GetSysAssetAllocationlist(User_Id);
            if (dsSysAllocation != null)
            {
                if (dsSysAllocation.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsSysAllocation.Tables[0].Rows)
                    {
                        SysAssetAllocationModel objmodel = new SysAssetAllocationModel();
                        objmodel.AssignmentId = Convert.ToInt32(dr["AssignmentId"].ToString());
                        // objmodel.AssetId = Convert.ToInt32(dr["AssetId"].ToString());
                        objmodel.AssetName = dr["AssetName"].ToString();
                        objmodel.AssignedTo = dr["AssignedTo"].ToString();
                        objmodel.Status = dr["Status"].ToString();
                        objmodel.AssignedType = dr["AssignedType"].ToString();
                        objmodel.AssignedDate = Convert.ToDateTime(dr["AssignedDate"].ToString());
                        if (dr["ReturnDate"].ToString() != "")
                        {
                            objmodel.ReturnDate = Convert.ToDateTime(dr["ReturnDate"].ToString());
                        }
                        objmodel.ApprovedBy = dr["ApprovedBy"].ToString();

                        if (dr["ApprovedDate"].ToString() != "")
                        {
                            objmodel.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                        }
                        objmodel.CreatedBy = dr["CreatedBy"].ToString();
                        objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                        objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        model.AssetAllocationList.Add(objmodel);

                    }
                    foreach (DataRow dr in dsSysAllocation.Tables[1].Rows)
                    {
                        SysAssetAllocationModel objmodel = new SysAssetAllocationModel();
                        objmodel.AssignmentId = Convert.ToInt32(dr["AssignmentId"].ToString());
                        // objmodel.AssetId = Convert.ToInt32(dr["AssetId"].ToString());
                        objmodel.AssetName = dr["AssetName"].ToString();
                        objmodel.AssignedTo = dr["AssignedTo"].ToString();
                        objmodel.AssignedType = dr["AssignedType"].ToString();
                        objmodel.Status = dr["Status"].ToString();
                        objmodel.AssignedDate = Convert.ToDateTime(dr["AssignedDate"].ToString());
                        if (dr["ReturnDate"].ToString() != "")
                        {
                            objmodel.ReturnDate = Convert.ToDateTime(dr["ReturnDate"].ToString());
                        }
                        objmodel.ApprovedBy = dr["ApprovedBy"].ToString();

                        if (dr["ApprovedDate"].ToString() != "")
                        {
                            objmodel.ApprovedDate = Convert.ToDateTime(dr["ApprovedDate"].ToString());
                        }
                        objmodel.CreatedBy = dr["CreatedBy"].ToString();
                        objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                        objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        model.ApprovalList.Add(objmodel);

                    }
                }
            }
            return model;
        }

        public int UpdateAssetApproval(int AssignmentId, string User_Id, int Status)
        {
            int errcode = 0;
            AdminDal objdal = new AdminDal();
            errcode = objdal.UpdateAssetApproval(AssignmentId, User_Id, Status);
            return errcode;
        }


        public SysAssetAllocationModel GetSelectedSysAssetAllocation(int AssignmentId)
        {
            SysAssetAllocationModel AssetAllocationmodel = new SysAssetAllocationModel();

            AdminDal DALobj = new AdminDal();
            try
            {
                DataSet DSObj = new DataSet();
                DSObj = DALobj.GetSelectedSysAssetAllocation(AssignmentId);
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        AssetAllocationmodel.AssignmentId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["AssignmentId"].ToString());
                        AssetAllocationmodel.AssetId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["AssetId"].ToString());
                        AssetAllocationmodel.AssigntoEmpId = Convert.ToInt32(DSObj.Tables[0].Rows[0]["AssignedTo"].ToString());
                        AssetAllocationmodel.AssignedType = DSObj.Tables[0].Rows[0]["AssignedType"].ToString();

                        AssetAllocationmodel.AssignedDate = Convert.ToDateTime(DSObj.Tables[0].Rows[0]["AssignedDate"].ToString());
                        if (DSObj.Tables[0].Rows[0]["ReturnDate"].ToString() != "")
                        {
                            AssetAllocationmodel.ReturnDate = Convert.ToDateTime(DSObj.Tables[0].Rows[0]["ReturnDate"].ToString());
                        }

                        AssetAllocationmodel.ApprovedBy = DSObj.Tables[0].Rows[0]["ApprovedBy"].ToString();
                        if (DSObj.Tables[0].Rows[0]["ApprovedDate"].ToString() != "")
                        {
                            AssetAllocationmodel.ApprovedDate = Convert.ToDateTime(DSObj.Tables[0].Rows[0]["ApprovedDate"].ToString());
                        }


                        AssetAllocationmodel.CreatedBy = DSObj.Tables[0].Rows[0]["CreatedBy"].ToString();
                        AssetAllocationmodel.CreatedDate = Convert.ToDateTime(DSObj.Tables[0].Rows[0]["CreatedDate"].ToString());
                        AssetAllocationmodel.ModifiedBy = DSObj.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        AssetAllocationmodel.ModifiedDate = Convert.ToDateTime(DSObj.Tables[0].Rows[0]["ModifiedDate"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return AssetAllocationmodel;
        }



        public int SaveSysAssetAllocation(SysAssetAllocationModel assetAllocmodel, string User_ID)
        {
            AdminDal objdal = new AdminDal();
            int ErrCode = 0;
            try
            {
                DataSet DSObj = new DataSet();
                DSObj = objdal.GetSelectedSysAssetAllocation(0);
                if (DSObj == null)
                    return 0;
                if (DSObj.Tables.Count > 0)
                {
                    DSObj.Tables[0].Rows.Clear();
                    DataRow Dr = DSObj.Tables[0].NewRow();
                    Dr["AssignmentId"] = assetAllocmodel.AssignmentId;
                    Dr["AssignedTo"] = assetAllocmodel.AssigntoEmpId;
                    Dr["AssetId"] = assetAllocmodel.AssetId;
                    Dr["AssignedType"] = assetAllocmodel.AssignedType;
                    if (assetAllocmodel.ReturnDate != null)
                    {
                        Dr["ReturnDate"] = assetAllocmodel.ReturnDate;
                    }
                    Dr["AssignedDate"] = assetAllocmodel.AssignedDate;


                    DSObj.Tables[0].Rows.Add(Dr);
                    ErrCode = objdal.SaveSysAssetAllocationDetails(DSObj, User_ID);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;

        }







        #endregion Assets

        #region Approval Task List

        public List<PendingTaskListModel> PenddingTaskList(string User_Id)
        {
            List<PendingTaskListModel> ApproveModel = new List<PendingTaskListModel>();
            DataSet dsObj = new DataSet();
            try
            {
                AdminDal DALObj = new AdminDal();
                dsObj = DALObj.PenddingTaskList(User_Id);
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            PendingTaskListModel demo = new PendingTaskListModel();
                            demo.Description = dr["Description"].ToString();
                            demo.SubordinateCount = Convert.ToInt32(dr["SubordinateCount"].ToString());
                            demo.PendingApprovel = Convert.ToInt32(dr["Count"].ToString());
                            demo.HyperLink = dr["GotoLink"].ToString();
                            demo.SubOrdinateHyperLink = dr["SubordinateLink"].ToString();
                            ApproveModel.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ApproveModel;
        }
        #endregion Approval Task List

        #region Collection List
        public List<CollectionDetailsModel> CollectionList()
        {
            List<CollectionDetailsModel> lstCollection = new List<CollectionDetailsModel>();
            DataSet dsObj = new DataSet();
            try
            {
                AdminDal DALObj = new AdminDal();
                dsObj = DALObj.CollectionList();
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            CollectionDetailsModel demo = new CollectionDetailsModel();
                            demo.Type = dr["Type"].ToString();
                            demo.Amount = Convert.ToDouble(dr["Amount"].ToString());
                            lstCollection.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstCollection;
        }

        #endregion Collection List

        #region ProjectTaskOverdue
        public List<ProjectTaskOverdueModel> ProjectTaskOverdue(string User_Id)
        {
            List<ProjectTaskOverdueModel> lstProjectTaskOverdue = new List<ProjectTaskOverdueModel>();
            DataSet dsObj = new DataSet();
            try
            {
                AdminDal DALObj = new AdminDal();
                dsObj = DALObj.ProjectTaskOverdue(User_Id);
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            ProjectTaskOverdueModel demo = new ProjectTaskOverdueModel();
                            demo.stages = dr["stages"].ToString();
                            demo.OnTrack = Convert.ToInt32(dr["OnTrack"].ToString());
                            demo.Completed = Convert.ToInt32(dr["Completed"].ToString());
                            demo.Delayed = Convert.ToInt32(dr["Delayed"].ToString());
                            //  demo.HyperLink = dr["GotoLink"].ToString();
                            lstProjectTaskOverdue.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstProjectTaskOverdue;
        }
        //public int PendingCustomerKYC()
        //{
        //    SmartSys.BL.DW.CustomerModel PendingCustomerKYC = new SmartSys.BL.DW.CustomerModel();
        //    DataSet dsObj = new DataSet();
        //    try
        //    {
        //        AdminDal DALObj = new AdminDal();
        //        dsObj = DALObj.PendingCustomerKYC();
        //        if (dsObj != null)
        //        {
        //            if (dsObj.Tables.Count > 0)
        //            {
        //                foreach (DataRow dr in dsObj.Tables[0].Rows)
        //                {
        //                    SmartSys.BL.DW.CustomerModel demo = new SmartSys.BL.DW.CustomerModel();

        //                    demo.Count = Convert.ToInt32(dr["Count"].ToString());



        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.LogException(ex);
        //    }
        //    return dsObj;
        //}

        public DataSet PendingCustomerKYC()
        {
            DataSet dsBLDetails = new DataSet();
            try
            {
                AdminDal objAdmDA = new AdminDal();
                dsBLDetails = objAdmDA.PendingCustomerKYC();
                return dsBLDetails;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #endregion ProjectTaskOverdue

        #region Employee Assets

        public List<SysEmployeeAssetsModel> AssetListByEmployee(int EmpId)
        {

            List<SysEmployeeAssetsModel> assetlst = new List<SysEmployeeAssetsModel>();

            try
            {
                AdminDal objdal = new AdminDal();
                DataSet ds = new DataSet();
                ds = objdal.AssetListByEmployee(EmpId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (dr["ReturnDate"].ToString() == "")
                                {
                                    SysEmployeeAssetsModel objmodel = new SysEmployeeAssetsModel();
                                    objmodel.AssetId = Convert.ToInt32(dr["AssetId"].ToString());
                                    objmodel.AssetName = dr["AssetName"].ToString();
                                    objmodel.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                                    objmodel.AssignedDate = Convert.ToDateTime(dr["AssignedDate"].ToString());
                                    objmodel.CreatedBy = dr["CreatedBy"].ToString();
                                    objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                    objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                                    objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                    assetlst.Add(objmodel);
                                }

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return assetlst;
        }

        public SysEmployeeAssetsModel GetSelectedEmployeeAsset(int EmpId, int AssetId)
        {
            SysEmployeeAssetsModel AssetObj = new SysEmployeeAssetsModel();
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.GetSelectedAssetListByEmployee(EmpId, AssetId);
                if (dsEmp == null)
                    return null;
                if (dsEmp.Tables.Count > 0)
                {
                    if (dsEmp.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsEmp.Tables[0].Rows)
                        {
                            AssetObj.AssetId = Convert.ToInt32(dr["AssetId"].ToString());
                            AssetObj.AssetName = dr["AssetName"].ToString();

                            AssetObj.AllocationDate = Convert.ToDateTime(dr["AllocationDate"].ToString());
                            //   AssetObj.DeAllocationDate = Convert.ToDateTime(dr["DeAllocationDate"].ToString());
                            AssetObj.CreatedBy = dr["CreatedBy"].ToString();
                            AssetObj.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            AssetObj.ModifiedBy = dr["ModifiedBy"].ToString();
                            AssetObj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return AssetObj;
        }

        public int SaveEmployeeAsset(SysEmployeeAssetsModel assetmodel, string User_Id)
        {
            AdminDal objDAL = new AdminDal();
            DataSet dsEmp = new DataSet();
            try
            {

                dsEmp = objDAL.GetSelectedEmployeeAsset(0);
                if (dsEmp == null)
                    return 0;
                if (dsEmp.Tables.Count > 0)
                {
                    dsEmp.Tables[0].Rows.Clear();
                    DataRow dr = dsEmp.Tables[0].NewRow();
                    dr["AssetId"] = assetmodel.AssetId;
                    //  dr["EmpId"] = assetmodel.EmpId;
                    dr["AllocationDate"] = assetmodel.AllocationDate;
                    //dr["CreatedBy"] = assetmodel.CreatedBy;
                    //dr["ExpInYears"] = ExprtModel.ExpInYears;
                    //dr["Exp_Level"] = ExprtModel.Exp_Level;


                    dsEmp.Tables[0].Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objDAL.SaveEmpAsset(dsEmp, User_Id);
        }

        public int DeallocateAsset(int AssetId, int EmpId)
        {
            int errorcode = 0;
            try
            {
                AdminDal objdal = new AdminDal();

                errorcode = objdal.DeallocateAsset(AssetId, EmpId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errorcode;

        }

        #endregion Employee Assets

        #region Segement
        public List<ItemSegment> GetItemSegmentList(string UserId)
        {

            List<ItemSegment> SegmentLIst = new List<ItemSegment>();

            try
            {
                AdminDal objdal = new AdminDal();
                DataSet ds = new DataSet();
                ds = objdal.GetItemSegmentList(UserId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                ItemSegment objmodel = new ItemSegment();
                                objmodel.SegmentId = Convert.ToInt32(dr["SegmentId"].ToString());
                                objmodel.ParentSegmentId = Convert.ToInt32(dr["ParentSegmentId"].ToString());
                                objmodel.SegmentName = dr["SegmentName"].ToString();
                                objmodel.ParentSegmentName = dr["ParentSegmentName"].ToString();
                                objmodel.Description = dr["Description"].ToString();
                                objmodel.CreatedBy = dr["CreatedBy"].ToString();
                                objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                                objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                SegmentLIst.Add(objmodel);


                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return SegmentLIst;
        }


        public List<ItemSegment> GetdropdownForsegment(int SegmentId)
        {

            List<ItemSegment> SegmentLIst = new List<ItemSegment>();

            try
            {
                AdminDal objdal = new AdminDal();
                DataSet ds = new DataSet();
                ds = objdal.GetdropdownForsegment(SegmentId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                ItemSegment objmodel = new ItemSegment();
                                objmodel.SegmentId = Convert.ToInt32(dr["SegmentId"].ToString());
                                objmodel.SegmentName = dr["SegmentName"].ToString();
                                SegmentLIst.Add(objmodel);


                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return SegmentLIst;
        }

        public List<EquipmentSegmentmodel> EqipmentsegmentDrplist(int SegmentId)
        {
            List<EquipmentSegmentmodel> List = new List<EquipmentSegmentmodel>();
            ItemSegment model = new ItemSegment();
            try
            {
                AdminDal objdal = new AdminDal();
                DataSet ds = new DataSet();
                ds = objdal.EquipmentListForSegment(SegmentId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                EquipmentSegmentmodel objmodel = new EquipmentSegmentmodel();
                                objmodel.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                                objmodel.EquipmentName = dr["EquipmentName"].ToString();
                                List.Add(objmodel);


                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return List;
        }

        public List<EquipmentSegmentgrdmodel> Eqipmentsegmentgrdlist(int SegmentId)
        {
            List<EquipmentSegmentgrdmodel> List = new List<EquipmentSegmentgrdmodel>();
            ItemSegment model = new ItemSegment();
            try
            {
                AdminDal objdal = new AdminDal();
                DataSet ds = new DataSet();
                ds = objdal.EquipmentListForSegment(SegmentId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                EquipmentSegmentgrdmodel objmodel = new EquipmentSegmentgrdmodel();

                                objmodel.EquipmentName = dr["EquipmentName"].ToString();
                                objmodel.SegmentName = dr["SegmentName"].ToString();
                                List.Add(objmodel);


                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return List;
        }
        public ItemSegment GetselectedItemSegment(int SegmentId)
        {
            ItemSegment SegmentModel = new ItemSegment();
            DataSet ds = new DataSet();
            try
            {

                AdminDal objdl = new AdminDal();
                ds = objdl.GetSelectedItemSegment(SegmentId);
                if (ds == null) return null;

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            SegmentModel.SegmentId = Convert.ToInt32(ds.Tables[0].Rows[0]["SegmentId"].ToString());
                            SegmentModel.ParentSegmentId = Convert.ToInt32(ds.Tables[0].Rows[0]["ParentSegmentId"].ToString());
                            SegmentModel.SegmentName = ds.Tables[0].Rows[0]["SegmentName"].ToString();
                            SegmentModel.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                            SegmentModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            SegmentModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            SegmentModel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            SegmentModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SegmentModel;
        }
        public int SaveEquipmentforsegment(int SegmentId, int EquipmentId,string User_Id)
        {
            AdminDal admindal = new AdminDal();
            return admindal.SaveEquipmentForSegment(SegmentId, EquipmentId, User_Id);
        }
        public int SaveItemSegmentDetails(ItemSegment objmodel, string User_Id)
        {
            AdminDal admindal = new AdminDal();
            DataSet ds = new DataSet();

            try
            {
                ds = admindal.GetSelectedItemSegment(0);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].Rows.Clear();
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["SegmentId"] = objmodel.SegmentId;
                        dr["SegmentName"] = objmodel.SegmentName;
                        dr["Description"] = objmodel.Description;
                        dr["ParentSegmentId"] = objmodel.ParentSegmentId;
                        ds.Tables[0].Rows.Add(dr);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return admindal.SaveItemSegmentDetails(ds, User_Id);
        }
        #endregion Segment

        #region Itemcategory

        public List<ItemCategory> GetItemCategoryList(string UserId)
        {
            List<ItemCategory> CategoryList = new List<ItemCategory>();
            try
            {
                AdminDal objdal = new AdminDal();
                DataSet ds = new DataSet();
                ds = objdal.GetItemcategoryList(UserId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                if (Convert.ToInt32(dr["ParentCategoryId"].ToString()) == 0)
                                {
                                    ItemCategory objmodel = new ItemCategory();
                                    objmodel.CategoryId = Convert.ToInt32(dr["CategoryId"].ToString());
                                    objmodel.ParentCategoryId = Convert.ToInt32(dr["ParentCategoryId"].ToString());
                                    objmodel.Category = dr["Category"].ToString();
                                    objmodel.ParentCategorytName = dr["ParentCategorytName"].ToString();
                                    objmodel.Description = dr["Description"].ToString();
                                    objmodel.CreatedBy = dr["CreatedBy"].ToString();
                                    objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                    objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                                    objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                    objmodel.ChildItemCategoryList = GetChildItemCategoryList(objmodel.CategoryId, ds);
                                    CategoryList.Add(objmodel);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return CategoryList;
        }
        public List<ItemCategory> GetChildItemCategoryList(int Id, DataSet ds)
        {
            List<ItemCategory> ChildCategoryList = new List<ItemCategory>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Convert.ToInt32(dr["ParentCategoryId"].ToString()) == Id)
                {
                    ItemCategory objmodel = new ItemCategory();
                    objmodel.CategoryId = Convert.ToInt32(dr["CategoryId"].ToString());
                    objmodel.ParentCategoryId = Convert.ToInt32(dr["ParentCategoryId"].ToString());
                    objmodel.Category = dr["Category"].ToString();
                    objmodel.ParentCategorytName = dr["ParentCategorytName"].ToString();
                    objmodel.Description = dr["Description"].ToString();
                    objmodel.CreatedBy = dr["CreatedBy"].ToString();
                    objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                    objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                    objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                    objmodel.ChildItemCategoryList = GetChildItemCategoryList(objmodel.CategoryId, ds);
                    ChildCategoryList.Add(objmodel);
                }
            }
            return ChildCategoryList;
        }



        public List<ItemCategory> GetDropdownForItemCategory(int CategoryId)
        {

            List<ItemCategory> CategoryList = new List<ItemCategory>();

            try
            {
                AdminDal objdal = new AdminDal();
                DataSet ds = new DataSet();
                ds = objdal.GetDropdownForcategory(CategoryId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                ItemCategory objmodel = new ItemCategory();
                                objmodel.CategoryId = Convert.ToInt32(dr["CategoryId"].ToString());
                                objmodel.Category = dr["Category"].ToString();
                                CategoryList.Add(objmodel);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return CategoryList;
        }
        public ItemCategory GetselectedItemCategory(int CategoryId)
        {
            ItemCategory Categorymodel = new ItemCategory();
            DataSet ds = new DataSet();
            try
            {

                AdminDal objdl = new AdminDal();
                ds = objdl.GetSelectedItemCategory(CategoryId);
                if (ds == null) return null;

                if (ds != null)
                {

                    if (ds.Tables.Count > 0)
                    {

                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            Categorymodel.CategoryId = Convert.ToInt32(ds.Tables[0].Rows[0]["CategoryId"].ToString());
                            Categorymodel.ParentCategoryId = Convert.ToInt32(ds.Tables[0].Rows[0]["ParentCategoryId"].ToString());
                            Categorymodel.Category = ds.Tables[0].Rows[0]["Category"].ToString();
                            Categorymodel.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                            Categorymodel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            Categorymodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            Categorymodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            Categorymodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Categorymodel;
        }

        public int SaveItemcategoryDetails(ItemCategory objmodel, string User_Id)
        {
            AdminDal admindal = new AdminDal();
            DataSet ds = new DataSet();

            try
            {
                ds = admindal.GetSelectedItemCategory(0);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].Rows.Clear();
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["CategoryId"] = objmodel.CategoryId;
                        dr["Category"] = objmodel.Category;
                        dr["Description"] = objmodel.Description;
                        dr["ParentCategoryId"] = objmodel.ParentCategoryId;
                        ds.Tables[0].Rows.Add(dr);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return admindal.SaveItemCategoryDetails(ds, User_Id);
        }
        #endregion ItemCategory

        #region System Mail Config
        public List<SystemEmailConfig> GetSystemEmailConfigList()
        {
            List<SystemEmailConfig> EmailConfList = new List<SystemEmailConfig>();
            try
            {
                AdminDal objdal = new AdminDal();
                DataSet ds = new DataSet();
                ds = objdal.GetSystemEmailConfigList();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                SystemEmailConfig objmodel = new SystemEmailConfig();
                                objmodel.MailTemplateId = Convert.ToInt32(dr["MailTemplateId"].ToString());
                                objmodel.TemplateName = dr["TemplateName"].ToString();
                                objmodel.MailContent = dr["MailContent"].ToString();
                                objmodel.CreatedBy = dr["CreatedBy"].ToString();
                                objmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                                objmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                                objmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                                objmodel.DocumentType = dr["DocumentType"].ToString();
                                EmailConfList.Add(objmodel);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return EmailConfList;
        }

        public SystemEmailConfig GetSelectedSystemEmailConfigList(int MailTemplateId)
        {
            SystemEmailConfig SysEmailmodel = new SystemEmailConfig();
            DataSet ds = new DataSet();
            try
            {

                AdminDal objdl = new AdminDal();
                ds = objdl.GetSelectedSystemEmailConfigList(MailTemplateId);
                if (ds == null) return null;
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            SysEmailmodel.MailTemplateId = Convert.ToInt32(ds.Tables[0].Rows[0]["MailTemplateId"].ToString());
                            SysEmailmodel.TemplateName = ds.Tables[0].Rows[0]["TemplateName"].ToString();
                            SysEmailmodel.MailContent = ds.Tables[0].Rows[0]["MailContent"].ToString();
                            SysEmailmodel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            SysEmailmodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            SysEmailmodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            SysEmailmodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                            SysEmailmodel.DocumentType = ds.Tables[0].Rows[0]["DocumentType"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SysEmailmodel;
        }
        public int SaveSystemEmailConfigDetails(SystemEmailConfig objmodel, string User_Id)
        {
            AdminDal admindal = new AdminDal();
            DataSet ds = new DataSet();

            try
            {
                ds = admindal.GetSelectedSystemEmailConfigList(0);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].Rows.Clear();
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["MailTemplateId"] = objmodel.MailTemplateId;
                        dr["TemplateName"] = objmodel.TemplateName;
                        dr["MailContent"] = objmodel.MailContent;
                        dr["DocumentType"] = objmodel.DocumentType;
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return admindal.SaveSystemEmailConfigDetails(ds, User_Id);
        }

        public List<DocumentTypeModel> GetDocumentTypeforDropDwnList(string MailTemplateId)
        {
            List<DocumentTypeModel> DocTypeModel = new List<DocumentTypeModel>();
            try
            {
                AdminDal objdal = new AdminDal();
                DataSet ds = new DataSet();
                ds = objdal.GetDocumentTypeforDropDwnList(MailTemplateId);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                DocumentTypeModel objmodel = new DocumentTypeModel();
                                objmodel.Description = dr["Description"].ToString();
                                objmodel.DocumentType = dr["DocumentType"].ToString();
                                DocTypeModel.Add(objmodel);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DocTypeModel;
        }
        #endregion System Mail Config

        #region EMployeeDocuments
        public int SaveEmployeeDocuments(string DocumentPath, string DocumentName, int EmpId)
        {
            try
            {
                AdminDal objDal = new AdminDal();

                return objDal.SaveEmployeeDocuments(DocumentPath, DocumentName, EmpId);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<Documents> GetDocumentsByEmployee(int EmpId)
        {

            List<Documents> Doclist = new List<Documents>();

            try
            {
                AdminDal DALObj = new AdminDal();
                DataSet DSObj = new DataSet();
                DSObj = DALObj.GetDocumentsByEmployee(EmpId);
                if (DSObj != null)
                {
                    if (DSObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in DSObj.Tables[0].Rows)
                        {
                            Documents Docs = new Documents();
                            Docs.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            Docs.DocumentName = dr["DocumentName"].ToString();
                            Docs.DocumentPath = dr["DocumentPath"].ToString();
                            Doclist.Add(Docs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Doclist;
        }
        #endregion EmployeeDocuments

        public RiskCaseListAllDetail GetAllRiskCaseDetailInpercent(string User_Id)
        {
            RiskCaseListAllDetail TaskDetail = new RiskCaseListAllDetail();

            List<RiskCaseListPercent> RiskPercentList = new List<RiskCaseListPercent>();
            TaskDetail.RiskPercentList = new List<RiskCaseListPercent>();
            List<RiskCaseListPercent> CasePercentList = new List<RiskCaseListPercent>();
            TaskDetail.CasePercentList = new List<RiskCaseListPercent>();
            TaskDetail.PendingRiskPercent = new List<RiskCaseListPercent>();
            TaskDetail.PendingCasePercent = new List<RiskCaseListPercent>();

            DataSet dsObj = new DataSet();
            try
            {
                AdminDal DALObj = new AdminDal();
                dsObj = DALObj.GetAllRiskCaseDetailInpercent(User_Id);
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        if (dsObj.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in dsObj.Tables[0].Rows)
                            {
                              
                                    if (dr["TaskType"].ToString() == "2")
                                {
                                    // Risk
                                    RiskCaseListAllDetail tskDetail2 = new RiskCaseListAllDetail();
                                    RiskCaseListPercent Percent = new RiskCaseListPercent();
                                    tskDetail2.CreatedByMeCount = Convert.ToInt32(dr["CreatedByMeCount"]);
                                    if (tskDetail2.CreatedByMeCount == 0)
                                    {
                                        tskDetail2.CreatedByMeCount = 1;
                                    }
                                        tskDetail2.TotalCreated = Convert.ToInt32(dr["TotalCreated"]);
                                    Percent.TotalCreatedPercentCount = Convert.ToInt32(dr["TotalCreated"]);
                                    if (tskDetail2.TotalCreated.ToString()!= "")
                                    {
                                        int Total = Convert.ToInt32(((decimal)tskDetail2.CreatedByMeCount / (decimal)tskDetail2.TotalCreated) * 100);
                                        Percent.TotalCreatedPercent = Total + 1;
                                    }

                                    tskDetail2.InProgress = Convert.ToInt32(dr["InProgress"]);
                                    Percent.InprogressCount = Convert.ToInt32(dr["InProgress"]);
                                    if (tskDetail2.CreatedByMeCount.ToString() != "")
                                    {
                                       
                                            Percent.InprogressPercent = Convert.ToInt32(((decimal)tskDetail2.InProgress / (decimal)tskDetail2.CreatedByMeCount) * 100);
                                        
                                    }
                                    tskDetail2.Closed = Convert.ToInt32(dr["Closed"]);
                                    Percent.ClosedCount = Convert.ToInt32(dr["Closed"]);
                                    if (tskDetail2.CreatedByMeCount.ToString() != "")
                                    {
                                        Percent.ClosedPercent = Convert.ToInt32(((decimal)tskDetail2.Closed / (decimal)tskDetail2.CreatedByMeCount) * 100);
                                    }
                                    tskDetail2.OnHold = Convert.ToInt32(dr["OnHold"]);
                                    Percent.OnholdCount = Convert.ToInt32(dr["OnHold"]);
                                    if (tskDetail2.CreatedByMeCount.ToString() != "")
                                    {
                                        Percent.OnholdPercent = Convert.ToInt32(((decimal)tskDetail2.OnHold / (decimal)tskDetail2.CreatedByMeCount) * 100);
                                    }
                                    tskDetail2.Complete = Convert.ToInt32(dr["Complete"]);
                                    Percent.Completecount = Convert.ToInt32(dr["Complete"]);
                                    if (tskDetail2.CreatedByMeCount.ToString() != "")
                                    {
                                        Percent.CompletePercent = Convert.ToInt32(((decimal)tskDetail2.Complete / (decimal)tskDetail2.CreatedByMeCount) * 100);
                                    }
                                    tskDetail2.REVIEWED = Convert.ToInt32(dr["REVIEWED"]);
                                    Percent.ReviewedPerCount = Convert.ToInt32(dr["REVIEWED"]);
                                    if (tskDetail2.CreatedByMeCount.ToString() != "")
                                    {
                                        Percent.ReviewedPercent = Convert.ToInt32(((decimal)tskDetail2.REVIEWED / (decimal)tskDetail2.CreatedByMeCount) * 100);
                                    }
                                    tskDetail2.New = Convert.ToInt32(dr["New"]);
                                    Percent.NewPerCount = Convert.ToInt32(dr["New"]);
                                    if (tskDetail2.CreatedByMeCount.ToString() != "")
                                    {
                                        Percent.NewPercent = Convert.ToInt32(((decimal)tskDetail2.New / (decimal)tskDetail2.CreatedByMeCount) * 100);
                                    }
                                    tskDetail2.REJECTED = Convert.ToInt32(dr["REJECTED"]);
                                    Percent.RejectedCount = Convert.ToInt32(dr["REJECTED"]);
                                    if (tskDetail2.CreatedByMeCount.ToString() != "")
                                    {
                                        Percent.RejectedPercent = Convert.ToInt32(((decimal)tskDetail2.REJECTED / (decimal)tskDetail2.CreatedByMeCount) * 100);
                                    }
                                    tskDetail2.APPROVED = Convert.ToInt32(dr["APPROVED"]);
                                    Percent.ApprovedCount = Convert.ToInt32(dr["APPROVED"]);
                                    if (tskDetail2.CreatedByMeCount.ToString() != "")
                                    {
                                        Percent.ApprovedPercent = Convert.ToInt32(((decimal)tskDetail2.APPROVED / (decimal)tskDetail2.CreatedByMeCount) * 100);
                                    }
                                    tskDetail2.TotalReviewd = Convert.ToInt32(dr["TotalReviewd"]);
                                    tskDetail2.TotalApproved = Convert.ToInt32(dr["TotalApproved"]);
                                    tskDetail2.ReviewdByMe = Convert.ToInt32(dr["ReviewdByMe"]);
                                    Percent.ReviewdByMeCount = Convert.ToInt32(dr["ReviewdByMe"]);
                                    if (tskDetail2.TotalReviewd != 0)
                                    {
                                        Percent.ReviewdByMePercent = Convert.ToInt32(((decimal)tskDetail2.ReviewdByMe / (decimal)tskDetail2.TotalReviewd) * 100);
                                    }

                                    tskDetail2.ApprovedByMe = Convert.ToInt32(dr["ApprovedByMe"]);
                                    Percent.ApprovedByMeeCount = Convert.ToInt32(dr["ApprovedByMe"]);
                                    if (tskDetail2.TotalApproved != 0)
                                    {
                                        Percent.ApprovedByMeePercent = Convert.ToInt32(((decimal)tskDetail2.ApprovedByMe / (decimal)tskDetail2.TotalApproved) * 100);
                                    }
                                    TaskDetail.RiskPercentList.Add(Percent);
                                }
                                else
                                {
                                    if (dr["TaskType"].ToString() == "3")
                                    {
                                        RiskCaseListAllDetail tskDetail3 = new RiskCaseListAllDetail();
                                        RiskCaseListPercent Percent = new RiskCaseListPercent();
                                        tskDetail3.CreatedByMeCount = Convert.ToInt32(dr["CreatedByMeCount"]);
                                        if (tskDetail3.CreatedByMeCount == 0)
                                        {
                                            tskDetail3.CreatedByMeCount = 1;
                                        }
                                        tskDetail3.TotalCreated = Convert.ToInt32(dr["TotalCreated"]);
                                        Percent.TotalCreatedPercentCount = Convert.ToInt32(dr["CreatedByMeCount"]);
                                        if (tskDetail3.TotalCreated != 0)
                                        {
                                            int Total = Convert.ToInt32(((decimal)tskDetail3.CreatedByMeCount / (decimal)tskDetail3.TotalCreated) * 100);
                                            Percent.TotalCreatedPercent = Total + 1;
                                        }
                                        tskDetail3.InProgress = Convert.ToInt32(dr["InProgress"]);
                                        Percent.InprogressCount = Convert.ToInt32(dr["InProgress"]);
                                        if (tskDetail3.CreatedByMeCount.ToString() != "")
                                        {
                                            Percent.InprogressPercent = Convert.ToInt32(((decimal)tskDetail3.InProgress / (decimal)tskDetail3.CreatedByMeCount) * 100);
                                        }
                                        tskDetail3.Closed = Convert.ToInt32(dr["Closed"]);
                                        Percent.ClosedCount = Convert.ToInt32(dr["Closed"]);
                                        if (tskDetail3.CreatedByMeCount.ToString() != "")
                                        {
                                            Percent.ClosedPercent = Convert.ToInt32(((decimal)tskDetail3.Closed / (decimal)tskDetail3.CreatedByMeCount) * 100);
                                        }
                                        tskDetail3.OnHold = Convert.ToInt32(dr["OnHold"]);
                                        Percent.OnholdCount = Convert.ToInt32(dr["OnHold"]);
                                        if (tskDetail3.CreatedByMeCount.ToString() != "")
                                        {
                                            Percent.OnholdPercent = Convert.ToInt32(((decimal)tskDetail3.OnHold / (decimal)tskDetail3.CreatedByMeCount) * 100);
                                        }
                                        tskDetail3.Complete = Convert.ToInt32(dr["Complete"]);
                                        Percent.Completecount = Convert.ToInt32(dr["Complete"]);
                                        if (tskDetail3.CreatedByMeCount.ToString() != "")
                                        {
                                            Percent.CompletePercent = Convert.ToInt32(((decimal)tskDetail3.Complete / (decimal)tskDetail3.CreatedByMeCount) * 100);
                                        }
                                        tskDetail3.REVIEWED = Convert.ToInt32(dr["REVIEWED"]);
                                        Percent.ReviewedPerCount = Convert.ToInt32(dr["REVIEWED"]);
                                        if (tskDetail3.CreatedByMeCount.ToString() != "")
                                        {
                                            Percent.ReviewedPercent = Convert.ToInt32(((decimal)tskDetail3.REVIEWED / (decimal)tskDetail3.CreatedByMeCount) * 100);
                                        }
                                        tskDetail3.New = Convert.ToInt32(dr["New"]);
                                        Percent.NewPerCount = Convert.ToInt32(dr["New"]);
                                        if (tskDetail3.CreatedByMeCount.ToString() != "")
                                        {
                                            Percent.NewPercent = Convert.ToInt32(((decimal)tskDetail3.New / (decimal)tskDetail3.CreatedByMeCount) * 100);
                                        }
                                        tskDetail3.REJECTED = Convert.ToInt32(dr["REJECTED"]);
                                        Percent.RejectedCount = Convert.ToInt32(dr["REJECTED"]);
                                        if (tskDetail3.CreatedByMeCount.ToString() != "")
                                        {
                                            Percent.RejectedPercent = Convert.ToInt32(((decimal)tskDetail3.REJECTED / (decimal)tskDetail3.CreatedByMeCount) * 100);
                                        }
                                        tskDetail3.APPROVED = Convert.ToInt32(dr["APPROVED"]);
                                        Percent.ApprovedCount = Convert.ToInt32(dr["APPROVED"]);
                                        if (tskDetail3.CreatedByMeCount.ToString() != "")
                                        {
                                            Percent.ApprovedPercent = Convert.ToInt32(((decimal)tskDetail3.APPROVED / (decimal)tskDetail3.CreatedByMeCount) * 100);

                                            tskDetail3.TotalReviewd = Convert.ToInt32(dr["TotalReviewd"]);
                                            tskDetail3.TotalApproved = Convert.ToInt32(dr["TotalApproved"]);
                                            tskDetail3.ReviewdByMe = Convert.ToInt32(dr["ReviewdByMe"]);
                                            Percent.ReviewdByMeCount = Convert.ToInt32(dr["ReviewdByMe"]);

                                            if (tskDetail3.TotalReviewd != 0)
                                            {
                                                Percent.ReviewdByMePercent = Convert.ToInt32(((decimal)tskDetail3.ReviewdByMe / (decimal)tskDetail3.TotalReviewd) * 100);
                                            }

                                            tskDetail3.ApprovedByMe = Convert.ToInt32(dr["ApprovedByMe"]);
                                            Percent.ApprovedByMeeCount = Convert.ToInt32(dr["ApprovedByMe"]);
                                            if (tskDetail3.TotalApproved != 0)
                                            {
                                                Percent.ApprovedByMeePercent = Convert.ToInt32(((decimal)tskDetail3.ApprovedByMe / (decimal)tskDetail3.TotalApproved) * 100);
                                            }
                                            TaskDetail.CasePercentList.Add(Percent);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Risk
                            RiskCaseListAllDetail tskDetail2 = new RiskCaseListAllDetail();
                            RiskCaseListPercent Percent = new RiskCaseListPercent();
                            tskDetail2.CreatedByMeCount = 0;
                            tskDetail2.TotalCreated = 0;
                            Percent.TotalCreatedPercentCount =0;
                            if (tskDetail2.TotalCreated != 0)
                            {
                                int Total = 0;
                                Percent.TotalCreatedPercent =0;
                            }

                            tskDetail2.InProgress = 0;
                            Percent.InprogressCount = 0;
                            if (tskDetail2.CreatedByMeCount == 0)
                            {
                                Percent.InprogressPercent = 0;
                            }
                            tskDetail2.Closed = 0;
                            Percent.ClosedCount = 0;
                            if (tskDetail2.CreatedByMeCount == 0)
                            {
                                Percent.ClosedPercent = 0;
                            }
                            tskDetail2.OnHold = 0;
                            Percent.OnholdCount = 0;
                            if (tskDetail2.CreatedByMeCount == 0)
                            {
                                Percent.OnholdPercent = 0;
                            }
                            tskDetail2.Complete = 0;
                            Percent.Completecount =0;
                            if (tskDetail2.CreatedByMeCount != 0)
                            {
                                Percent.CompletePercent =0;
                            }
                            tskDetail2.REVIEWED = 0;
                            Percent.ReviewedPerCount = 0;
                            if (tskDetail2.CreatedByMeCount == 0)
                            {
                                Percent.ReviewedPercent = 0;
                            }
                            tskDetail2.New = 0;
                            Percent.NewPerCount = 0;
                            if (tskDetail2.CreatedByMeCount == 0)
                            {
                                Percent.NewPercent = 0;
                            }
                            tskDetail2.REJECTED = 0;
                            Percent.RejectedCount =0;
                            if (tskDetail2.CreatedByMeCount == 0)
                            {
                                Percent.RejectedPercent = 0;
                            }
                            tskDetail2.APPROVED = 0;
                            Percent.ApprovedCount = 0;
                            if (tskDetail2.CreatedByMeCount != 0)
                            {
                                Percent.ApprovedPercent = 0;
                            }
                            tskDetail2.TotalReviewd = 0;
                            tskDetail2.TotalApproved = 0;
                            tskDetail2.ReviewdByMe = 0;
                            Percent.ReviewdByMeCount =0;
                            if (tskDetail2.TotalReviewd == 0)
                            {
                                Percent.ReviewdByMePercent = 0;
                            }

                            tskDetail2.ApprovedByMe = 0;
                            Percent.ApprovedByMeeCount = 0;
                            if (tskDetail2.TotalApproved == 0)
                            {
                                Percent.ApprovedByMeePercent = 0;
                            }
                            TaskDetail.RiskPercentList.Add(Percent);
                            TaskDetail.CasePercentList.Add(Percent);
                        }

                        if (dsObj.Tables[1].Rows.Count > 0)
                        {
                            //Risk
                            foreach (DataRow dr in dsObj.Tables[1].Rows)
                            {

                                if (dr["TaskType"].ToString() == "2")
                                {
                                    RiskCaseListAllDetail tskDetail = new RiskCaseListAllDetail();
                                    RiskCaseListPercent Percent = new RiskCaseListPercent();
                                    tskDetail.TotalPending = Convert.ToInt32(dr["TotalPending"]);
                                    tskDetail.OnHold = Convert.ToInt32(dr["OnHold"]);
                                    tskDetail.InProgress = Convert.ToInt32(dr["InProgress"]);
                                    Percent.PendingInprogressCount = Convert.ToInt32(dr["InProgress"]);
                                    //Percent.ApprovedCount = Convert.ToInt32(dr["Pending"]);
                                    Percent.PendingOnholdCount = Convert.ToInt32(dr["OnHold"]);
                                    if (tskDetail.TotalPending != 0)
                                    {
                                        Percent.ApprovedPercent = Convert.ToInt32(((decimal)Percent.ApprovedCount / (decimal)tskDetail.TotalPending) * 100);
                                        Percent.InprogressPercent = Convert.ToInt32(((decimal)tskDetail.InProgress / (decimal)tskDetail.TotalPending) * 100);
                                        Percent.OnholdPercent = Convert.ToInt32(((decimal)tskDetail.OnHold / (decimal)tskDetail.TotalPending) * 100);
                                        TaskDetail.PendingRiskPercent.Add(Percent);
                                    }
                                }
                              


                                    if (dr["TaskType"].ToString() == "3")
                                    {
                                        RiskCaseListAllDetail tskDetail = new RiskCaseListAllDetail();
                                        RiskCaseListPercent Percent = new RiskCaseListPercent();
                                        tskDetail.TotalPending = Convert.ToInt32(dr["TotalPending"]);
                                        tskDetail.OnHold = Convert.ToInt32(dr["OnHold"]);
                                        tskDetail.InProgress = Convert.ToInt32(dr["InProgress"]);
                                        Percent.PendingInprogressCount = Convert.ToInt32(dr["InProgress"]);
                                      //  Percent.ApprovedCount = Convert.ToInt32(dr["Pending"]);
                                        Percent.PendingOnholdCount = Convert.ToInt32(dr["OnHold"]);
                                        if (tskDetail.TotalPending != 0)
                                        {
                                            Percent.ApprovedPercent = Convert.ToInt32(((decimal)Percent.ApprovedCount / (decimal)tskDetail.TotalPending) * 100);
                                            Percent.InprogressPercent = Convert.ToInt32(((decimal)tskDetail.InProgress / (decimal)tskDetail.TotalPending) * 100);
                                            Percent.OnholdPercent = Convert.ToInt32(((decimal)tskDetail.OnHold / (decimal)tskDetail.TotalPending) * 100);
                                            TaskDetail.PendingCasePercent.Add(Percent);
                                        }
                                    }
                                }

                            }
                        else
                        {
                            if (dsObj.Tables[1].Rows.Count == 0)
                            {
                                RiskCaseListAllDetail tskDetail = new RiskCaseListAllDetail();
                                RiskCaseListPercent Percent = new RiskCaseListPercent();
                                tskDetail.TotalPending = 0;
                                tskDetail.OnHold = 0;
                                tskDetail.InProgress = 0;
                                Percent.PendingInprogressCount = 0;
                                //  Percent.ApprovedCount = Convert.ToInt32(dr["Pending"]);
                                Percent.PendingOnholdCount = 0;
                                if (tskDetail.TotalPending == 0)
                                {
                                    Percent.ApprovedPercent = 0;
                                    Percent.InprogressPercent = 0;
                                    Percent.OnholdPercent = 0;
                                    TaskDetail.PendingCasePercent.Add(Percent);
                                    TaskDetail.PendingRiskPercent.Add(Percent);
                                }
                            }
                        }
                        }
                    }
               
            }


            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return TaskDetail;
        }

        #region EXchange Rate
        public int CurrencyExchange(List<CurrencyModel> List)
        {
            int errcode = 0;
            try
            {
                AdminDal ObjDal = new AdminDal();
                DataTable dt = new DataTable();
                dt.Columns.Add("currencycode");
                dt.Columns.Add("Rate");
                dt.Columns.Add("Source");

                foreach (var item in List)
                {
                    dt.Rows.Add(item.CurrencyCode, item.ExchangeRate, item.Source);
                }
                errcode = ObjDal.CurrencyExchange(dt);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }

        #endregion Exchange Rate

        #region IssueDetail 
        public IssueModel GetIssueDetailListForDashBoard(string User_Id)
        {
            IssueModel model = new IssueModel();
            model.TotalIssues = new List<IssueModel>();
            model.MyIssues = new List<IssueModel>();


            DataSet dsObj = new DataSet();
            try
            {
                AdminDal DALObj = new AdminDal();
                dsObj = DALObj.GetIssueDetailList(User_Id);
                if (dsObj != null)
                {
                    if (dsObj.Tables.Count > 0)
                    {
                        foreach (DataRow dr in dsObj.Tables[0].Rows)
                        {
                            IssueModel demo = new IssueModel();
                            demo.Description = dr["Description"].ToString();
                            demo.Count = Convert.ToInt32(dr["Count"].ToString());
                            demo.Statuscode = Convert.ToInt32(dr["StatusId"].ToString());
                            model.TotalIssues.Add(demo);
                        }

                        foreach (DataRow dr in dsObj.Tables[1].Rows)
                        {
                            IssueModel demo = new IssueModel();
                            demo.Description = dr["Description"].ToString();
                            demo.Count = Convert.ToInt32(dr["Count"].ToString());
                            demo.Statuscode = Convert.ToInt32(dr["StatusId"].ToString());
                            model.MyIssues.Add(demo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return model;
        }
        #endregion IssueDetail

        #region StockLocation
        public List<StockLocation> GetStockLocationList()
        {
            List<StockLocation> LststkLoc = new List<StockLocation>();
            AdminDal objDal = new AdminDal();
            DataSet dsData = objDal.GetStockLocationList();
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(dr["ParentLocationId"].ToString()) == 0)
                        {
                            StockLocation Stklocmodel = new StockLocation();
                            Stklocmodel.STLocationId = Convert.ToInt32(dr["STLocationId"].ToString());
                            Stklocmodel.Id = Convert.ToInt32(dr["Id"].ToString());
                            Stklocmodel.ParentLocationId = Convert.ToInt32(dr["ParentLocationId"].ToString());
                            Stklocmodel.Name = dr["Name"].ToString();
                            Stklocmodel.Description = dr["Description"].ToString();
                            Stklocmodel.CreatedBy = dr["CreatedBy"].ToString();
                            Stklocmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                            Stklocmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                            Stklocmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                            Stklocmodel.ChildLocList = GetChildScockLocation(Stklocmodel.STLocationId, dsData);
                            LststkLoc.Add(Stklocmodel);
                        }
                    }
            return LststkLoc;
        }
        public List<StockLocation> GetStockLocationListForDrp()
        {
            List<StockLocation> LststkLoc = new List<StockLocation>();
            AdminDal objDal = new AdminDal();
            DataSet dsData = objDal.GetStockLocationList();
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                            StockLocation Stklocmodel = new StockLocation();
                            Stklocmodel.STLocationId = Convert.ToInt32(dr["STLocationId"].ToString());                            
                            Stklocmodel.Name = dr["Name"].ToString() + "__" + dr["Description"].ToString();
                        LststkLoc.Add(Stklocmodel);                        
                    }
            return LststkLoc;
        }
        public List<StockLocation> GetChildScockLocation(int LocId, DataSet ds)
        {
            List<StockLocation> ChildLststkLoc = new List<StockLocation>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (Convert.ToInt32(dr["ParentLocationId"].ToString()) == LocId)
                {
                    StockLocation Stklocmodel = new StockLocation();
                    Stklocmodel.STLocationId = Convert.ToInt32(dr["STLocationId"].ToString());
                    Stklocmodel.Id = Convert.ToInt32(dr["Id"].ToString());
                    Stklocmodel.ParentLocationId = Convert.ToInt32(dr["ParentLocationId"].ToString());
                    Stklocmodel.Name = dr["Name"].ToString();
                    Stklocmodel.Description = dr["Description"].ToString();
                    Stklocmodel.CreatedBy = dr["CreatedBy"].ToString();
                    Stklocmodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                    Stklocmodel.ModifiedBy = dr["ModifiedBy"].ToString();
                    Stklocmodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                    Stklocmodel.ChildLocList = GetChildScockLocation(Stklocmodel.STLocationId, ds);
                    ChildLststkLoc.Add(Stklocmodel);
                }
            }
            return ChildLststkLoc;
        }


        public StockLocation GetSelectedStockLocation(int STLocationId)
        {
            StockLocation SLModel = new StockLocation();
            DataSet ds = new DataSet();
            try
            {

                AdminDal objdl = new AdminDal();
                ds = objdl.GetSelectedStockLocation(STLocationId);
                if (ds == null) return null;
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            SLModel.STLocationId = Convert.ToInt32(ds.Tables[0].Rows[0]["STLocationId"].ToString());
                            SLModel.Name = ds.Tables[0].Rows[0]["Name"].ToString();
                            SLModel.Description = ds.Tables[0].Rows[0]["Description"].ToString();
                            SLModel.ParentLocationId = Convert.ToInt32(ds.Tables[0].Rows[0]["ParentLocationId"].ToString());
                            SLModel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            SLModel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            SLModel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            SLModel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SLModel;
        }
        public int SaveStockLocation(StockLocation SLModel, string User_Id)
        {
            AdminDal objDAL = new AdminDal();
            DataSet ds = new DataSet();
            try
            {

                ds = objDAL.GetSelectedStockLocation(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["STLocationId"] = SLModel.STLocationId;
                    dr["ParentLocationId"] = SLModel.ParentLocationId;
                    dr["Name"] = SLModel.Name;
                    dr["Description"] = SLModel.Description;
                    ds.Tables[0].Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objDAL.SaveStockLocation(ds, User_Id);
        }
        public List<StockLocation> GetParentStockLocation(int STLocationId)
        {
            List<StockLocation> lstModel = new List<StockLocation>();
            try
            {
                AdminDal objDal = new AdminDal();
                DataSet dsData = objDal.GetParentStockLocationLst(STLocationId);
                if (dsData != null)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {

                        StockLocation Model = new StockLocation();

                        Model.STLocationId = Convert.ToInt32(dr["STLocationId"].ToString());
                        Model.Name = dr["Name"].ToString();
                        lstModel.Add(Model);
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstModel;
        }
        #endregion StockLocation
        public List<ProjectModel> GetProjectListBYTam()
        {
            List<ProjectModel> lstProject = new List<ProjectModel>();
            AdminDal objDL = new AdminDal();
            DataSet dsData = objDL.GetTop10ProjectByTam();
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        ProjectModel proj = new ProjectModel();
                        proj.ProjectId = Convert.ToInt32(dr["ProjectId"].ToString());
                        proj.ProjectName = dr["ProjectName"].ToString();
                        if (dr["TAMCOUNT"].ToString() != "")
                            proj.TAMCOUNT = Convert.ToInt32(dr["TAMCOUNT"].ToString());
                        else proj.TAMCOUNT = 0;

                        lstProject.Add(proj);
                    }
            return lstProject;
        }
        public List<QuotationModel> GetTop10QoutationByAmount()
        {
            List<QuotationModel> LstQuot = new List<QuotationModel>();
            AdminDal objDL = new AdminDal();
            DataSet dsData = objDL.GetTop10QuotationByAmount();
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        QuotationModel Quot = new QuotationModel();
                        Quot.EnqId = Convert.ToInt32(dr["EnqId"].ToString());
                        Quot.QuotationNumber = dr["QuotationNumber"].ToString();
                        if (dr["Amount"].ToString() != "")
                            Quot.Amount = Convert.ToDouble(dr["Amount"].ToString());
                        else
                            Quot.Amount = 0;
                        LstQuot.Add(Quot);

                    }
            int Totalcnt = LstQuot.Count;
            if (Totalcnt < 10)
            {
                int RemainingCount = 10 - Totalcnt;

                for (var i = 0; i <= RemainingCount; i++)
                {
                    QuotationModel Quot = new QuotationModel();
                    Quot.QuotationId = 0;
                    Quot.QuotationNumber = "0";
                    Quot.Amount = 0;
                    LstQuot.Add(Quot);
                }
            }


            return LstQuot;
        }

        #region CurrencyModel
        public List<CurrencyModel> CurrencyList(string User_Id)
        {
            List<CurrencyModel> CurrencyList = new List<CurrencyModel>();
            try
            {
                DataSet ds = new DataSet();
                AdminDal objdal = new AdminDal();
                ds = objdal.CurrencyExchangeRate(User_Id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        CurrencyModel model = new CurrencyModel();
                        model.CurrencyCode = dr["CurrencyCode"].ToString();
                        model.Source = dr["Source"].ToString();
                        model.ExchangeRate = Convert.ToDouble(dr["ExchangeRate"]);
                        model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);
                        CurrencyList.Add(model);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CurrencyList;
        }
        public CurrencyModel GetSelectedCurrency(string CurrencyCode)
        {
            CurrencyModel model = new CurrencyModel();
            try
            {
                DataSet ds = new DataSet();
                AdminDal objdal = new AdminDal();
                ds = objdal.GetSelectedCurrency(CurrencyCode);

                if (ds.Tables[0].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {

                        model.CurrencyCode = dr["CurrencyCode"].ToString();
                        model.Source = dr["Source"].ToString();
                        model.ExchangeRate = Convert.ToDouble(dr["ExchangeRate"]);
                        model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"]);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public int SaveCurrency(CurrencyModel model,string User_Id)
        {
            DataSet ds = new DataSet();
            AdminDal dalobj = new AdminDal();
            try
            {
                ds = dalobj.GetSelectedCurrency("");
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["CurrencyCode"] = model.CurrencyCode;
                    dr["ExchangeRate"] = model.ExchangeRate;
                    dr["Source"] = model.Source;
                    ds.Tables[0].Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dalobj.SaveCurrency(ds, User_Id);
        }
        #endregion CurrencyModel

        #region EmployeeSalaryStructure
        public List<EmployeeModel> GetEmployeeListForSlaryStructure(string User_Id)
        {
            List<EmployeeModel> lstEmp = new List<EmployeeModel>();
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.GetEmpListForSalaryStructure(User_Id);
                if (dsEmp == null)
                    return null;
                if (dsEmp.Tables.Count > 0)
                {
                    if (dsEmp.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsEmp.Tables[0].Rows)
                        {
                            EmployeeModel EmpObj = new EmployeeModel();
                            EmpObj.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            EmpObj.EmpName = dr["EmployeeName"].ToString();
                            EmpObj.Gender = dr["Gender"].ToString();
                            EmpObj.ManagerName = dr["ManagerName"].ToString();
                            EmpObj.AnnualFixPay = Convert.ToDouble(dr["AnnualFixPay"].ToString());
                            EmpObj.AnnualVariablePay = Convert.ToDouble(dr["AnnualVariablePay"].ToString());
                            EmpObj.Designation = dr["Designation"].ToString();
                            EmpObj.DeptName = dr["DeptName"].ToString();
                            EmpObj.Deleted = Convert.ToBoolean(dr["Deleted"].ToString());
                            EmpObj.ModifiedByName = Convert.ToString(dr["ModifiedBy"].ToString());
                            EmpObj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());

                            lstEmp.Add(EmpObj);
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return lstEmp;
        }
        public int UpdateEmployeeDetail(EmpSalaryStructureModel model,string User_Id)
        {
            AdminDal objDAL = new AdminDal();
            DataSet ds = new DataSet();
            return objDAL.UpdateEmployeeDetails(model.FixedRate, model.EmpId, model.VariableRate, User_Id);


        }
        public DataSet Getcomponentdetail(int ComponentId)
        {
            AdminDal objDAL = new AdminDal();
            DataSet ds = new DataSet();
            string details = "";
            ds = objDAL.GetComponentDetail(ComponentId);

            return ds;
        }
        public EmployeeModel GetselectedEmpSlaryStructureDetails(int EmpId)
        {
            EmployeeModel EmpObj = new EmployeeModel();
            try
            {
                AdminDal objDAL = new AdminDal();
                DataSet dsEmp = new DataSet();
                dsEmp = objDAL.GetSelectedEmpSalaryStructureDetails(EmpId);
                if (dsEmp == null)
                    return null;
                if (dsEmp.Tables.Count > 0)
                {
                    if (dsEmp.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsEmp.Tables[0].Rows)
                        {

                            EmpObj.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                            EmpObj.EmpName = dr["EmployeeName"].ToString();
                            EmpObj.Gender = dr["Gender"].ToString();
                            EmpObj.ManagerName = dr["ManagerName"].ToString();
                            EmpObj.AnnualFixPay = Convert.ToDouble(dr["AnnualFixPay"].ToString());
                            EmpObj.AnnualVariablePay = Convert.ToDouble(dr["AnnualVariablePay"].ToString());
                            EmpObj.Designation = dr["Designation"].ToString();
                            EmpObj.DeptName = dr["DeptName"].ToString();
                            EmpObj.Deleted = Convert.ToBoolean(dr["Deleted"].ToString());
                            EmpObj.ModifiedByName = Convert.ToString(dr["ModifiedBy"].ToString());
                            EmpObj.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());


                        }

                    }
                }

            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
            return EmpObj;
        }
        #endregion EmployeeSalaryStructure

        #region Public Holiday
        public List<PublicHolidayModel> GetPublicHolidayList()
        {
            List<PublicHolidayModel> LstPubHoliday = new List<PublicHolidayModel>();
            AdminDal objDal = new AdminDal();
            DataSet dsData = objDal.GetPublicHolidayList();
            if (dsData != null)
                if (dsData.Tables.Count > 0)
                    foreach (DataRow dr in dsData.Tables[0].Rows)
                    {
                        PublicHolidayModel PublicHolidaymodel = new PublicHolidayModel();
                        PublicHolidaymodel.HolidayId = Convert.ToInt32(dr["HolidayId"].ToString());
                        PublicHolidaymodel.HolidayDate = Convert.ToDateTime(dr["HolidayDate"].ToString());
                        PublicHolidaymodel.RegionName = dr["Region"].ToString();
                        PublicHolidaymodel.FinYear = dr["FinYear"].ToString();
                        PublicHolidaymodel.Title = dr["Title"].ToString();                       
                        PublicHolidaymodel.CreatedBy = dr["CreatedBy"].ToString();
                        PublicHolidaymodel.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        PublicHolidaymodel.ModifiedBy = dr["ModifiedBy"].ToString();
                        PublicHolidaymodel.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        LstPubHoliday.Add(PublicHolidaymodel);
                    }
            return LstPubHoliday;
        }

        public PublicHolidayModel GetselectedPublicHolidayList(int HolidayId)
        {
            PublicHolidayModel PublicHolidaymodel = new PublicHolidayModel();
            DataSet ds = new DataSet();
            try
            {
                AdminDal objdl = new AdminDal();
                ds = objdl.GetselectedPublicHolidayList(HolidayId);
                if (ds == null) return null;
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            PublicHolidaymodel.HolidayId = Convert.ToInt32(ds.Tables[0].Rows[0]["HolidayId"].ToString());
                            PublicHolidaymodel.HolidayDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["HolidayDate"].ToString());
                            if (ds.Tables[0].Rows[0]["RegionId"].ToString() != "" && ds.Tables[0].Rows[0]["RegionId"].ToString() != null)
                            {
                                PublicHolidaymodel.RegionId = Convert.ToInt32(ds.Tables[0].Rows[0]["RegionId"].ToString());
                            }
                            PublicHolidaymodel.FinYear = ds.Tables[0].Rows[0]["FinYear"].ToString();
                            PublicHolidaymodel.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                            PublicHolidaymodel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            PublicHolidaymodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            PublicHolidaymodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            PublicHolidaymodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PublicHolidaymodel;
        }
        public int SavePublicHolidayDetail(PublicHolidayModel SLModel, string User_Id)
        {
            AdminDal objDAL = new AdminDal();
            DataSet ds = new DataSet();
            try
            {
                ds = objDAL.GetselectedPublicHolidayList(0);
                if (ds == null)
                    return 0;
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Rows.Clear();
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["HolidayId"] = SLModel.HolidayId;
                    dr["HolidayDate"] = SLModel.HolidayDate;
                    dr["RegionId"] = SLModel.RegionId;
                    dr["FinYear"] = SLModel.FinYear;
                    dr["Title"] = SLModel.Title;
                    ds.Tables[0].Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return objDAL.SavePublicHolidayDetail(ds, User_Id);
        }
        #endregion Public Holiday 

        #region Weekly Sal Component

        public List<WeekLySalaryModel> GetWeeklySalryList(string User_Id)
        {
            List<WeekLySalaryModel> SalList = new List<WeekLySalaryModel>();
            DataSet ds = new DataSet();
            try
            {
                AdminDal objdl = new AdminDal();
                ds = objdl.GetWeeklySalryList(User_Id);
                if (ds == null) return null;
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach(DataRow dr in ds.Tables[0].Rows)
                            {
                                WeekLySalaryModel WeekSalModel = new WeekLySalaryModel();
                                WeekSalModel.TimeSheetId = Convert.ToInt32(dr["TimeSheetId"].ToString());
                                WeekSalModel.StartDate = Convert.ToDateTime(dr["StartDate"].ToString());
                                WeekSalModel.EndDate = Convert.ToDateTime(dr["EndDate"].ToString());
                                WeekSalModel.WorkStartsession = Convert.ToDateTime(dr["WorkStartSession"].ToString());
                                WeekSalModel.WorkEndsession = Convert.ToDateTime(dr["WorkEndSession"].ToString());
                                WeekSalModel.WeekNo = Convert.ToInt32(dr["WeekNo"].ToString());
                                WeekSalModel.EmployeeName = dr["EmpName"].ToString();
                                WeekSalModel.WeeklyWorkHour = Convert.ToDouble(dr["WeeklyWorkingHour"].ToString());
                                WeekSalModel.WeeklyPayment = Convert.ToDouble(dr["WeeklyPay"].ToString());
                                WeekSalModel.HourRate = Convert.ToDouble(dr["HourRate"].ToString());
                                WeekSalModel.AnnualPay = Convert.ToDouble(dr["AnnualPay"].ToString());
                                WeekSalModel.WorkingDays = Convert.ToDouble(dr["WorkingDays"].ToString());
                                WeekSalModel.PublicHoliday = Convert.ToInt32(dr["PublicHoliday"].ToString());
                                WeekSalModel.TotalSunday = Convert.ToInt32(dr["TotalSunday"].ToString());
                                SalList.Add(WeekSalModel);
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return SalList;
        }
        public WeekLySalaryModel GetSelectedWeeklySalryList(string User_Id)
        {
            WeekLySalaryModel WeekSalModel = new WeekLySalaryModel();
            DataSet ds = new DataSet();
            try
            {
                AdminDal objdl = new AdminDal();
                ds = objdl.GetWeeklySalryList(User_Id);
                if (ds == null) return null;
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {                                                              
                                WeekSalModel.EmployeeName = dr["EmployeeName"].ToString();
                                WeekSalModel.CompanyName = dr["CompanyName"].ToString();
                                WeekSalModel.AnnualPay = Convert.ToDouble(dr["AnnualFixPay"].ToString());
                                WeekSalModel.CompanyWorkHour = Convert.ToDouble(dr["WorkingHour"].ToString());                                                        
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return WeekSalModel;
        }
        #endregion Weekly Sal Component
    }
}
