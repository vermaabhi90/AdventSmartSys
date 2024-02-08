using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Collections;
using MySql.Data.MySqlClient;

namespace SmartSys.DAL
{

    #region [Admin module methods]
    public class AdminDal
    {

        public int SendEngineHeartBeat(int EngineId, string SystemComponent)
        {
            int errorCode = 0;
            try
            {
                SqlParameter[] SysParam = new SqlParameter[3];
                SysParam[0] = new SqlParameter("@EngineId", SqlDbType.Int);
                SysParam[0].Value = EngineId;
                SysParam[1] = new SqlParameter("@SystemComponent", SqlDbType.VarChar);
                SysParam[1].Value = SystemComponent;
                SysParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                SysParam[2].Value = 0;
                SysParam[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteNonQuery(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysEngineHearbeatSend", SysParam);
                errorCode = Convert.ToInt32(SysParam[2].Value);
                return errorCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetUserCompCode(string User_ID)
        {
            DataSet DS;
            try
            {
                SqlParameter sysUSerid = new SqlParameter("@User_id", SqlDbType.NVarChar);

                sysUSerid.Value = User_ID;
                DS = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetUSerCompcode", sysUSerid);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return null;
            }
            return DS;
        }
        public DataSet GetMenuTask(string User_ID)
        {
            DataSet dsTaskDetail;
            try
            {
                SqlParameter sysUSerid = new SqlParameter("@User_ID", SqlDbType.NVarChar);

                sysUSerid.Value = User_ID;
                dsTaskDetail = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetMenuItem", sysUSerid);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return null;
            }
            return dsTaskDetail;
        }

        public DataSet GetRoleList()
        {
            DataSet dsRoleList = new DataSet();
            try
            {
                SqlParameter[] SysuserParam = new SqlParameter[1];
                SysuserParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                SysuserParam[0].Value = 0;
                dsRoleList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetRoleList", SysuserParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsRoleList;
        }

        public DataSet GetSystemUser(string User_Id)
        {
            DataSet dsUser = new DataSet();
            try
            {
                SqlParameter[] SysuserParam = new SqlParameter[1];
                SysuserParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                SysuserParam[0].Value = User_Id;
                dsUser = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedUser", SysuserParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsUser;
        }
        public DataSet GetDashboardByUser(string User_Id)
        {
            DataSet dsUser = new DataSet();
            try
            {
                SqlParameter[] SysuserParam = new SqlParameter[1];
                SysuserParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                SysuserParam[0].Value = User_Id;
                dsUser = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetDashBoardByUser", SysuserParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsUser;
        }

        public DataSet UserAssignRoleList(string User_Id)
        {
            DataSet dsRoleList = new DataSet();
            try
            {
                SqlParameter[] SysuserParam = new SqlParameter[1];
                SysuserParam[0] = new SqlParameter("@UserId", SqlDbType.NVarChar);
                SysuserParam[0].Value = User_Id;

                dsRoleList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysUserAssignRoleList", SysuserParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsRoleList;
        }

        public DataSet GetRoleList(string User_Id)
        {
            DataSet dsRoleList = new DataSet();
            try
            {
                SqlParameter[] SysuserParam = new SqlParameter[1];
                SysuserParam[0] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                SysuserParam[0].Value = User_Id;

                dsRoleList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetRoleList", SysuserParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsRoleList;
        }


        public int SaveSystemUser(SqlTransaction sqlTransaction, DataSet dsSysUserDetails, out int iUserID)
        {
            int iErrorCode = 0;

            try
            {
                SqlParameter[] SysuserParam = new SqlParameter[6];

                SysuserParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                SysuserParam[0].Value = dsSysUserDetails.Tables[0].Rows[0]["UserID"];
                SysuserParam[0].Direction = ParameterDirection.InputOutput;

                SysuserParam[1] = new SqlParameter("@Password", SqlDbType.VarChar);
                SysuserParam[1].Value = dsSysUserDetails.Tables[0].Rows[0]["Password"];

                SysuserParam[2] = new SqlParameter("@FullName", SqlDbType.VarChar);
                SysuserParam[2].Value = dsSysUserDetails.Tables[0].Rows[0]["FullName"];

                SysuserParam[2] = new SqlParameter("@FullName", SqlDbType.VarChar);
                SysuserParam[2].Value = dsSysUserDetails.Tables[0].Rows[0]["FullName"];

                SysuserParam[3] = new SqlParameter("@LoginName", SqlDbType.VarChar);
                SysuserParam[3].Value = dsSysUserDetails.Tables[0].Rows[0]["LoginName"];

                SysuserParam[4] = new SqlParameter("@ModifiedBy", SqlDbType.Int);
                SysuserParam[4].Value = dsSysUserDetails.Tables[0].Rows[0]["ModifiedBy"];

                SysuserParam[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                SysuserParam[5].Value = 0;
                SysuserParam[5].Direction = ParameterDirection.Output;

                //  SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveUser", SysuserParam);

                SqlHelper.ExecuteNonQuery(sqlTransaction, CommandType.StoredProcedure, "sp_SysSaveUser", SysuserParam);

                iErrorCode = (int)SysuserParam[5].Value;

                //Call this method to save UserID and group details...
                iUserID = int.Parse(SysuserParam[0].Value.ToString());
                //SaveSystemUserDetails(dsSysUserDetails, iUserID);


            }
            catch (SqlException sqlex)
            {
                SmartSys.Utility.Common.LogException(sqlex);
                SmartSys.Utility.FaultExceptionError fault = new Utility.FaultExceptionError();
                fault.Result = false;
                fault.ErrorMsg = sqlex.Message;
                fault.Description = sqlex.Message;
                iErrorCode = 0;
                throw new FaultException<SmartSys.Utility.FaultExceptionError>(fault, sqlex.Message);
            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);

                SmartSys.Utility.FaultExceptionError fault = new Utility.FaultExceptionError();
                fault.Result = false;
                fault.ErrorMsg = ex.Message;
                fault.Description = ex.Message;
                iErrorCode = 0;
                throw new FaultException<SmartSys.Utility.FaultExceptionError>(fault, ex.Message);
            }
            return iErrorCode;
        }

        public int SaveSysemUserRoleDetails(DataSet dsSystemUserRoleDetails)
        {
            int iErrorCode = 0;
            SqlTransaction sqltransaction;
            SqlConnection sqlconn = new SqlConnection(Common.SqlConnectionString);
            sqlconn.Open();
            sqltransaction = sqlconn.BeginTransaction();
            try
            {
                if (dsSystemUserRoleDetails != null)
                {
                    SqlCommand insertCommandVersion = SqlHelper.CreateCommand(sqlconn, "sp_SysSaveUserRoleDetails", "UserId", "RoleId");
                    insertCommandVersion.Parameters["@ErrorCode"].Value = 0;
                    insertCommandVersion.Transaction = sqltransaction;

                    SqlCommand updateCommandVersion = SqlHelper.CreateCommand(sqlconn, "sp_SysSaveUserRoleDetails", "UserId", "RoleId");
                    updateCommandVersion.Parameters["@ErrorCode"].Value = 0;
                    updateCommandVersion.Transaction = sqltransaction;

                    SqlCommand deleteCommandVersion = SqlHelper.CreateCommand(sqlconn, "sp_SysDeleteUserRole", "UserID", "RoleId");
                    deleteCommandVersion.Parameters["@ErrorCode"].Value = 0;
                    deleteCommandVersion.Transaction = sqltransaction;


                    ArrayList ouptFromSP = new ArrayList();
                    SqlHelper.UpdateDataset(insertCommandVersion, deleteCommandVersion, updateCommandVersion, dsSystemUserRoleDetails, dsSystemUserRoleDetails.Tables["webpages_UsersInRoles"].TableName, ref ouptFromSP);

                    if (ouptFromSP.Count > 0)
                    {
                        iErrorCode = int.Parse(ouptFromSP[0].ToString());
                    }
                    sqltransaction.Commit();
                }
            }
            catch (SqlException sqlex)
            {
                sqltransaction.Rollback();
                Common.LogException(sqlex);
            }
            catch (Exception ex)
            {
                sqltransaction.Rollback();
                Common.LogException(ex);
            }

            return iErrorCode;

        }

        public int SaveSystemUser(DataSet dsSysUser)
        {
            int errorCode = 0;
            try
            {

                SqlParameter[] SysuserParam = new SqlParameter[4];

                SysuserParam[0] = new SqlParameter("@UserID", SqlDbType.Int);
                SysuserParam[0].Value = dsSysUser.Tables[0].Rows[0]["UserID"];
                SysuserParam[1] = new SqlParameter("@UserName", SqlDbType.NVarChar);
                SysuserParam[1].Value = dsSysUser.Tables[0].Rows[0]["UserName"];
                SysuserParam[2] = new SqlParameter("@DisplayName", SqlDbType.VarChar);
                SysuserParam[2].Value = dsSysUser.Tables[0].Rows[0]["DisplayName"];
                SysuserParam[3] = new SqlParameter("@PasswordHint", SqlDbType.VarChar);
                SysuserParam[3].Value = dsSysUser.Tables[0].Rows[0]["PasswordHint"];
                SqlHelper.ExecuteNonQuery(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveUser", SysuserParam);
                return 1;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errorCode;
        }

        public int DeleteUserRole(int Userid)
        {
            int errcode = 0;
            try
            {
                SqlParameter[] UserRoleDel = new SqlParameter[2];
                UserRoleDel[0] = new SqlParameter("@UserID", SqlDbType.Int);
                UserRoleDel[0].Value = Userid;

                UserRoleDel[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                UserRoleDel[1].Value = 0;
                UserRoleDel[1].Direction = ParameterDirection.Output;
                errcode = SqlHelper.ExecuteNonQuery(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeleteUserRoles", UserRoleDel);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errcode;
        }

        public int SaveUserRoles(DataSet dsUserRole)
        {

            int errorCode = 0;
            SqlConnection cn = new SqlConnection(Common.SqlConnectionString);
            cn.Open();
            SqlTransaction trn = cn.BeginTransaction();
            try
            {

                SqlParameter[] UserRoleDel = new SqlParameter[2];
                UserRoleDel[0] = new SqlParameter("@UserID", SqlDbType.Int);
                UserRoleDel[0].Value = Convert.ToInt32(dsUserRole.Tables[0].Rows[0]["UserID"]);

                UserRoleDel[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                UserRoleDel[1].Value = 0;
                UserRoleDel[1].Direction = ParameterDirection.Output;
                SqlHelper.ExecuteNonQuery(trn, CommandType.StoredProcedure, "sp_SysDeleteUserRoles", UserRoleDel);

                if (Convert.ToInt32(UserRoleDel[1].Value.ToString()) == 600004)
                {
                    for (int i = 0; i < dsUserRole.Tables[0].Rows.Count; i++)
                    {
                        SqlParameter[] UserRolesave = new SqlParameter[3];

                        UserRolesave[0] = new SqlParameter("@UserID", SqlDbType.Int);
                        UserRolesave[0].Value = dsUserRole.Tables[0].Rows[i]["UserID"];

                        UserRolesave[1] = new SqlParameter("@RoleId", SqlDbType.NVarChar);
                        UserRolesave[1].Value = dsUserRole.Tables[0].Rows[i]["RoleId"].ToString();

                        UserRolesave[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                        UserRolesave[2].Value = 0;
                        UserRolesave[2].Direction = ParameterDirection.InputOutput;

                        SqlHelper.ExecuteNonQuery(trn, CommandType.StoredProcedure, "sp_SysSaveUserRoleDetails", UserRolesave);

                        if (Convert.ToInt32(UserRolesave[2].Value.ToString()) != 6000004)
                        {
                            trn.Rollback();
                            cn.Close();
                            return Convert.ToInt32(UserRolesave[2].Value.ToString());
                        }
                    }
                }
                trn.Commit();
                cn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                trn.Rollback();
                cn.Close();
            }
            return errorCode;
        }

        public int SaveSystemUserDetails(DataSet dsSysUserDetails)
        {
            int iErrorCode = 0, iErrorCode1 = 0, iUserID = 0;
            SqlTransaction sqltransaction;
            SqlConnection sqlconn = new SqlConnection(Common.SqlConnectionString);
            sqlconn.Open();
            sqltransaction = sqlconn.BeginTransaction();
            try
            {
                if (dsSysUserDetails != null)
                {
                    iErrorCode = SaveSystemUser(sqltransaction, dsSysUserDetails, out iUserID);

                    SqlCommand insertCommandVersion = SqlHelper.CreateCommand(sqlconn, "ssp_SysSaveUserGroupDetails", "UserID", "GroupID");
                    //insertCommandVersion.Parameters["@UserID"].Value = iUserID;

                    foreach (DataRow dr in dsSysUserDetails.Tables["tbl_UserGroup"].Rows)
                    {
                        dr["UserID"] = iUserID;
                    }

                    insertCommandVersion.Parameters["@ErrorCode"].Value = iErrorCode1;
                    insertCommandVersion.Transaction = sqltransaction;

                    SqlCommand updateCommandVersion = SqlHelper.CreateCommand(sqlconn, "ssp_SysSaveUserGroupDetails", "UserID", "GroupID");

                    //updateCommandVersion.Parameters["@UserID"].Value = iUserID;

                    updateCommandVersion.Parameters["@ErrorCode"].Value = iErrorCode1;
                    updateCommandVersion.Transaction = sqltransaction;

                    SqlCommand deleteCommandVersion = SqlHelper.CreateCommand(sqlconn, "dsp_SysDeleteUserGroup", "UserID", "GroupID");

                    deleteCommandVersion.Parameters["@ErrorCode"].Value = iErrorCode1;
                    deleteCommandVersion.Transaction = sqltransaction;

                    ArrayList ouptFromSP = new ArrayList();
                    SqlHelper.UpdateDataset(insertCommandVersion, deleteCommandVersion, updateCommandVersion, dsSysUserDetails, dsSysUserDetails.Tables["tbl_UserGroup"].TableName, ref ouptFromSP);

                    if (ouptFromSP.Count > 0)
                    {
                        iErrorCode1 = int.Parse(ouptFromSP[0].ToString());
                    }
                    sqltransaction.Commit();

                }
            }
            catch (SqlException sqlex)
            {
                sqltransaction.Rollback();
                SmartSys.Utility.Common.LogException(sqlex);
                SmartSys.Utility.FaultExceptionError fault = new Utility.FaultExceptionError();
                fault.Result = false;
                fault.ErrorMsg = sqlex.Message;
                fault.Description = sqlex.Message;
                iErrorCode = 0;
                throw new FaultException<SmartSys.Utility.FaultExceptionError>(fault, sqlex.Message);
            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);

                sqltransaction.Rollback();
                SmartSys.Utility.FaultExceptionError fault = new Utility.FaultExceptionError();
                fault.Result = false;
                fault.ErrorMsg = ex.Message;
                fault.Description = ex.Message;
                iErrorCode = 0;
                throw new FaultException<SmartSys.Utility.FaultExceptionError>(fault, ex.Message);
            }
            return iErrorCode;
        }


        public DataSet GetSysUserDetails()
        {
            DataSet dsUserDetails = new DataSet();
            try
            {

                //  SqlParameter sqlParmUserID = new SqlParameter("@UserID", SqlDbType.Int);
                // sqlParmUserID.Value = iUserID;
                // dsUserDetails = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetUserDetails", sqlParmUserID);
                dsUserDetails = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetUserDetails");

            }
            catch (SqlException sqlex)
            {

                SmartSys.Utility.Common.LogException(sqlex);
                SmartSys.Utility.FaultExceptionError fault = new Utility.FaultExceptionError();
                fault.Result = false;
                fault.ErrorMsg = sqlex.Message;
                fault.Description = sqlex.Message;
                throw new FaultException<SmartSys.Utility.FaultExceptionError>(fault, sqlex.Message);
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
            return dsUserDetails;
        }

        public DataSet GetSystemUserList()
        {
            DataSet dsUserList = new DataSet();
            try
            {
                dsUserList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetUserList");

            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsUserList;
        }

        public DataSet GetGroupList()
        {
            DataSet dsGroupList = new DataSet();
            try
            {
                dsGroupList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetGroupList");
            }
            catch (SqlException sqlex)
            {
                SmartSys.Utility.Common.LogException(sqlex);
                SmartSys.Utility.FaultExceptionError fault = new Utility.FaultExceptionError();
                fault.Result = false;
                fault.ErrorMsg = sqlex.Message;
                fault.Description = sqlex.Message;

                throw new FaultException<SmartSys.Utility.FaultExceptionError>(fault, sqlex.Message);
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
            return dsGroupList;

        }

        public DataSet GetBusinessLineList()
        {
            DataSet dsBusinessLines = new DataSet();
            try
            {
                dsBusinessLines = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetBusinessLineList");
                return dsBusinessLines;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataSet GetDBConnectionList()
        {
            DataSet dsDBCon = new DataSet();
            try
            {
                dsDBCon = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetDBConnections");
                return dsDBCon;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataSet GetSelectedDBConnection(int ConnectionId)
        {
            DataSet dsDBCon = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ConnectionId", SqlDbType.Int);
                objParam[0].Value = ConnectionId;
                dsDBCon = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDBConnectionsGetSelected", objParam);
                return dsDBCon;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataSet GetDepartmentlist(string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id",SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetDepartmentList", ObjParam);
                return ds;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataSet GetSelectedDepartmentList(Int16 DeptId)
        {
            DataSet dsEvent = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@DeptId", SqlDbType.Int);
                parameters[0].Value = DeptId;

                dsEvent = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedDepartment", parameters);
                return dsEvent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSysCompanyLogo(int iCompID)
        {
            DataSet dsComp = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@CompId", SqlDbType.Int);
                parameters[0].Value = iCompID;

                dsComp = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetCompanyLogo", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsComp;
        }

        public DataSet GetSysCompanyList()
        {
            DataSet dsSysCompList = new DataSet();
            try
            {
                dsSysCompList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetCompanyList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsSysCompList;
        }

        public int SaveSysCompanyDetails(DataSet dsSysCompDetails)
        {
            int iErroCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[14];

                parameters[0] = new SqlParameter("@Name", SqlDbType.VarChar);
                parameters[0].Value = dsSysCompDetails.Tables["tbl_SysCompany"].Rows[0]["Name"];

                parameters[1] = new SqlParameter("@ShortName", SqlDbType.VarChar);
                parameters[1].Value = dsSysCompDetails.Tables["tbl_SysCompany"].Rows[0]["ShortName"];

                parameters[2] = new SqlParameter("@AddressLine1", SqlDbType.VarChar);
                parameters[2].Value = dsSysCompDetails.Tables["tbl_SysCompany"].Rows[0]["AddressLine1"];

                parameters[3] = new SqlParameter("@AddressLine2", SqlDbType.VarChar);
                parameters[3].Value = dsSysCompDetails.Tables["tbl_SysCompany"].Rows[0]["AddressLine2"];

                parameters[4] = new SqlParameter("@City", SqlDbType.VarChar);
                parameters[4].Value = dsSysCompDetails.Tables["tbl_SysCompany"].Rows[0]["City"];

                parameters[5] = new SqlParameter("@State", SqlDbType.VarChar);
                parameters[5].Value = dsSysCompDetails.Tables["tbl_SysCompany"].Rows[0]["State"];

                parameters[6] = new SqlParameter("@Country", SqlDbType.VarChar);
                parameters[6].Value = dsSysCompDetails.Tables["tbl_SysCompany"].Rows[0]["Country"];

                parameters[7] = new SqlParameter("@Pin", SqlDbType.VarChar);
                parameters[7].Value = dsSysCompDetails.Tables["tbl_SysCompany"].Rows[0]["Pin"];

                parameters[8] = new SqlParameter("@TagLine", SqlDbType.VarChar);
                parameters[8].Value = dsSysCompDetails.Tables["tbl_SysCompany"].Rows[0]["TagLine"];

                parameters[9] = new SqlParameter("@Logo", SqlDbType.VarBinary);
                parameters[9].Value = dsSysCompDetails.Tables["tbl_SysCompany"].Rows[0]["Logo"];

                parameters[10] = new SqlParameter("@Cst", SqlDbType.VarChar);
                parameters[10].Value = dsSysCompDetails.Tables["tbl_SysCompany"].Rows[0]["Cst"];

                parameters[11] = new SqlParameter("@Bst", SqlDbType.VarChar);
                parameters[11].Value = dsSysCompDetails.Tables["tbl_SysCompany"].Rows[0]["Bst"];

                parameters[12] = new SqlParameter("@ServiceTN", SqlDbType.VarChar);
                parameters[12].Value = dsSysCompDetails.Tables["tbl_SysCompany"].Rows[0]["ServiceTN"];

                parameters[13] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[13].Value = 0;
                parameters[13].Direction = ParameterDirection.Output;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveCompanyDetails", parameters);

                iErroCode = Convert.ToInt32(parameters[13].Value);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }

            return iErroCode;
        }

        public int SaveDepartment(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {

                SqlParameter[] parameters = new SqlParameter[5];

                parameters[0] = new SqlParameter("@DeptId", SqlDbType.SmallInt);
                parameters[0].Value = ds.Tables[0].Rows[0]["DeptId"];

                parameters[1] = new SqlParameter("@DeptName", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["DeptName"];

                parameters[2] = new SqlParameter("@DeptHeadId", SqlDbType.Int);
                parameters[2].Value = ds.Tables[0].Rows[0]["DeptHeadId"];

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveDepartment", parameters);
                if (parameters[4].Value != null)
                    errCode = Convert.ToInt32(parameters[4].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        public int SaveDepartmentTask(DataSet ds,string User_Id,int DeptId)
        {
            int ErrCode = 0;                       
            SqlTransaction sqltransaction;
            SqlConnection sqlconn = new SqlConnection(Common.SqlConnectionString);
            sqlconn.Open();
            sqltransaction = sqlconn.BeginTransaction();
            try
            {
                SqlParameter[] SysRole = new SqlParameter[2];
                SysRole[0] = new SqlParameter("@DeptId", SqlDbType.Int);
                SysRole[0].Value = DeptId;

                SysRole[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                SysRole[1].Value = 0;
                SysRole[1].Direction = ParameterDirection.Output;

                  SqlHelper.ExecuteDataset(sqltransaction, CommandType.StoredProcedure, "sp_SysDeleteDepartmentTask", SysRole);
                if (SysRole[1].Value != null)
                    ErrCode= Convert.ToInt32(SysRole[1].Value.ToString());
                if (ErrCode == 600001)
                {
                    if(ds.Tables.Count > 0)
                    {
                        if(ds.Tables[0].Rows.Count > 0)
                        {
                            foreach(DataRow dr in ds.Tables[0].Rows)
                            {
                                SqlParameter[] Param = new SqlParameter[4];
                                Param[0] = new SqlParameter("@DeptId", SqlDbType.Int);
                                Param[0].Value = dr["DeptId"];

                                Param[1] = new SqlParameter("@TaskId", SqlDbType.Int);
                                Param[1].Value = dr["TaskId"];

                                Param[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                                Param[2].Value = User_Id;

                                Param[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                                Param[3].Value = 0;
                                Param[3].Direction = ParameterDirection.Output;

                                SqlHelper.ExecuteDataset(sqltransaction, CommandType.StoredProcedure, "sp_SysSaveDepartmentTask", Param);
                                if (Param[3].Value != null)
                                    ErrCode = Convert.ToInt32(Param[3].Value.ToString());
                                if (ErrCode != 500002)
                                {
                                    sqltransaction.Rollback();
                                    break;
                                }
                            }
                            sqltransaction.Commit();
                        }
                        else
                        {
                            sqltransaction.Commit();
                        }
                    }
                }
                else
                {
                    sqltransaction.Rollback();                    
                }

            }
            catch (Exception ex)
            {
                sqltransaction.Rollback();
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public DataSet GetUserList()
        {
            DataSet dsList = new DataSet();
            try
            {
                dsList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetUserList");
                return dsList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataSet GetEmpList()
        {
            DataSet dsList = new DataSet();
            try
            {
                dsList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetEmpListForDropDown");
                return dsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllEmpList()
        {
            DataSet dsList = new DataSet();
            try
            {
                dsList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetAllEmpListForDropDown");
                return dsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region RoleTask
        public DataSet GetRoleTasks(string iRoleID,int DeptId,string User_Id)
        {
            DataSet dsRoleTasks = new DataSet();
            try
            {
                SqlParameter[] paramter = new SqlParameter[3];
                paramter[0] = new SqlParameter("@RoleId", SqlDbType.NVarChar);
                paramter[0].Value = iRoleID;

                paramter[1] = new SqlParameter("@DeptId", SqlDbType.Int);
                paramter[1].Value = DeptId;

                paramter[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                paramter[2].Value = User_Id;
                dsRoleTasks = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetRoleTasks", paramter);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsRoleTasks;
        }


        public int SaveSystemRole(SqlTransaction sqltransaction, DataSet dsRoleTasks, out string RoleId)
        {
            RoleId = "";
            int iErrorCode = 0;
            try
            {
                SqlParameter[] SysRoleParam = new SqlParameter[3];

                SysRoleParam[0] = new SqlParameter("@RoleId", SqlDbType.NVarChar);
                SysRoleParam[0].Value = dsRoleTasks.Tables["tbl_SysRole"].Rows[0]["RoleId"];
                SysRoleParam[0].Direction = ParameterDirection.InputOutput;

                SysRoleParam[1] = new SqlParameter("@RoleName", SqlDbType.VarChar);
                SysRoleParam[1].Value = dsRoleTasks.Tables["tbl_SysRole"].Rows[0]["RoleName"];

                SysRoleParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                SysRoleParam[2].Value = 0;
                SysRoleParam[2].Direction = ParameterDirection.Output;


                SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysSaveRole", SysRoleParam);

                iErrorCode = (int)SysRoleParam[2].Value;

                RoleId = SysRoleParam[0].Value.ToString();


            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return iErrorCode;
        }
        public int DeleteRoleTask(string RoleID)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] SysRole = new SqlParameter[2];
                SysRole[0] = new SqlParameter("@RoleId", SqlDbType.NVarChar);
                SysRole[0].Value = RoleID;

                SysRole[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                SysRole[1].Value = 0;
                SysRole[1].Direction = ParameterDirection.Output;

                errCode = SqlHelper.ExecuteNonQuery(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeleteRoleTask", SysRole);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveRoleTask(DataSet dsRoleTasks, string RoleType, string User_Id)
        {
            int iErrorCode = 0, iErrorCode1 = 0;
            string iRoleID = "0";
            SqlTransaction sqltransaction;
            SqlConnection sqlconn = new SqlConnection(Common.SqlConnectionString);
            sqlconn.Open();
            sqltransaction = sqlconn.BeginTransaction();
            try
            {
                if (dsRoleTasks != null)
                {
                    iErrorCode = SaveSystemRole(sqltransaction, dsRoleTasks, out iRoleID);
                    if (RoleType == "NewRole")
                    {
                        //sqltransaction.Commit();
                        //return 1;
                    }
                }

                foreach (DataRow dr in dsRoleTasks.Tables["tbl_SysRoleTask"].Rows)
                {
                    dr["RoleId"] = iRoleID;
                }

                SqlCommand insertCommandVersion = SqlHelper.CreateCommand(sqlconn, "sp_SysSaveRoleTasks", "RoleId", "TaskId", "CreatedBy", "ModifiedBy", "User_Id");
                insertCommandVersion.Parameters["@ErrorCode"].Value = iErrorCode1;
                insertCommandVersion.Transaction = sqltransaction;

                SqlCommand updateCommandVersion = SqlHelper.CreateCommand(sqlconn, "sp_SysSaveRoleTasks", "RoleId", "TaskId", "CreatedBy", "ModifiedBy", "User_Id");


                updateCommandVersion.Parameters["@ErrorCode"].Value = iErrorCode1;
                updateCommandVersion.Transaction = sqltransaction;

                SqlCommand deleteCommandVersion = SqlHelper.CreateCommand(sqlconn, "sp_SysDeleteRoleTask", "RoleId");

                deleteCommandVersion.Parameters["@ErrorCode"].Value = iErrorCode1;
                deleteCommandVersion.Transaction = sqltransaction;

                ArrayList ouptFromSP = new ArrayList();
                SqlHelper.UpdateDataset(insertCommandVersion, deleteCommandVersion, updateCommandVersion, dsRoleTasks, dsRoleTasks.Tables["tbl_SysRoleTask"].TableName, ref ouptFromSP);

                if (ouptFromSP.Count > 0)
                {
                    iErrorCode1 = int.Parse(ouptFromSP[0].ToString());
                }
                sqltransaction.Commit();



            }
            catch (Exception ex)
            {
                sqltransaction.Rollback();
                Common.LogException(ex);
            }


            return iErrorCode;
        }

        #endregion

        #region Employee

        public DataSet GetEmployeeList(string UserId)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@UserId",SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetEmployeeList", ObjParam);

            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsEmpList;
        }
        public DataSet GetSelectedEmployeeList(int EmpId)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedEmployee", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;

        }

        public DataSet EmployeeExpertiesList(int EmpId)
        {
            DataSet dsExprt = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                dsExprt = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysEmployeeExpertiesList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsExprt;

        }
        public DataSet GetSelectedExperties(int EmpId, string Experties)
        {
            DataSet dsExprt = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;

                objParam[1] = new SqlParameter("@Experties", SqlDbType.VarChar);
                objParam[1].Value = Experties;
                dsExprt = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetSelectedEmployeeExpertiesList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsExprt;

        }
        public int Saveempexperties(DataSet dsEmp, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];

                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = dsEmp.Tables[0].Rows[0]["EmpId"].ToString();

                parameters[1] = new SqlParameter("@Experties", SqlDbType.VarChar);
                parameters[1].Value = dsEmp.Tables[0].Rows[0]["Experties"].ToString();

                parameters[2] = new SqlParameter("@Exp_Level", SqlDbType.VarChar);
                parameters[2].Value = dsEmp.Tables[0].Rows[0]["Exp_Level"].ToString();

                parameters[3] = new SqlParameter("@ExpInYears", SqlDbType.Float);
                parameters[3].Value = dsEmp.Tables[0].Rows[0]["ExpInYears"].ToString();

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                parameters[5] = new SqlParameter("@NewExperties", SqlDbType.VarChar);
                parameters[5].Value = dsEmp.Tables[0].Rows[0]["NewExperties"].ToString();
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SaveEmployeeExperties", parameters);

                errCode = Convert.ToInt32(parameters[4].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_tbl_SysEmployeeExperties'")
                {
                    errCode = 600001;
                }
                return errCode; ;
            }
            return errCode;
        }
        public int SaveEmployee(DataSet dsEmp, string User_Id, bool isClient, string CheckNull)
        {
            int errCode = 0;
            try
            {

                SqlParameter[] parameters = new SqlParameter[33];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = dsEmp.Tables[0].Rows[0]["EmpId"];
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@FirstName", SqlDbType.VarChar);
                parameters[1].Value = dsEmp.Tables[0].Rows[0]["FirstName"];

                parameters[2] = new SqlParameter("@MiddleName", SqlDbType.VarChar);
                parameters[2].Value = dsEmp.Tables[0].Rows[0]["MiddleName"].ToString();

                parameters[3] = new SqlParameter("@LastName", SqlDbType.VarChar);
                parameters[3].Value = dsEmp.Tables[0].Rows[0]["LastName"].ToString();

                parameters[4] = new SqlParameter("@emailId", SqlDbType.VarChar);
                parameters[4].Value = dsEmp.Tables[0].Rows[0]["emailId"].ToString();

                parameters[5] = new SqlParameter("@Designation", SqlDbType.VarChar);
                parameters[5].Value = dsEmp.Tables[0].Rows[0]["Designation"].ToString();

                parameters[6] = new SqlParameter("@Qualification", SqlDbType.VarChar);
                parameters[6].Value = dsEmp.Tables[0].Rows[0]["Qualification"].ToString();

                parameters[7] = new SqlParameter("@ManagerId", SqlDbType.Int);
                parameters[7].Value = dsEmp.Tables[0].Rows[0]["ManagerId"].ToString();

                parameters[8] = new SqlParameter("@UserId", SqlDbType.SmallInt);
                parameters[8].Value = dsEmp.Tables[0].Rows[0]["UserId"].ToString();

                parameters[9] = new SqlParameter("@DeptId", SqlDbType.SmallInt);
                parameters[9].Value = 0;

                parameters[10] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[10].Value = User_Id;

                if (dsEmp.Tables[0].Rows[0]["Photo"].ToString() != "")
                {
                    parameters[11] = new SqlParameter("@Photo", SqlDbType.Image);
                    parameters[11].Value = (Byte[])(dsEmp.Tables[0].Rows[0]["Photo"]);
                }
                else
                {
                    parameters[11] = new SqlParameter("@Photo", SqlDbType.Image);
                    parameters[11].Value = dsEmp.Tables[0].Rows[0]["Photo"];
                }

                parameters[12] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[12].Value = 0;
                parameters[12].Direction = ParameterDirection.InputOutput;

                parameters[13] = new SqlParameter("@AnnualFixPay", SqlDbType.Float);
                parameters[13].Value = dsEmp.Tables[0].Rows[0]["AnnualFixPay"].ToString();

                parameters[14] = new SqlParameter("@AnnualVariablePay", SqlDbType.Float);
                parameters[14].Value = dsEmp.Tables[0].Rows[0]["AnnualVariablePay"].ToString();

                parameters[15] = new SqlParameter("@DateOfJoin", SqlDbType.DateTime);
                parameters[15].Value = dsEmp.Tables[0].Rows[0]["DateOfJoin"].ToString();

                parameters[16] = new SqlParameter("@PAN", SqlDbType.VarChar);
                parameters[16].Value = dsEmp.Tables[0].Rows[0]["PAN"].ToString();

                parameters[17] = new SqlParameter("@Mobile", SqlDbType.VarChar);
                parameters[17].Value = dsEmp.Tables[0].Rows[0]["Mobile"].ToString();

                parameters[18] = new SqlParameter("@PhoneNumber", SqlDbType.VarChar);
                parameters[18].Value = dsEmp.Tables[0].Rows[0]["PhoneNumber"].ToString();

                parameters[19] = new SqlParameter("@AADHAR", SqlDbType.VarChar);
                parameters[19].Value = dsEmp.Tables[0].Rows[0]["AADHAR"].ToString();

                parameters[20] = new SqlParameter("@LeaveOpBal", SqlDbType.VarChar);
                parameters[20].Value = dsEmp.Tables[0].Rows[0]["LeaveOpBal"].ToString();

                parameters[21] = new SqlParameter("@PassportNumber", SqlDbType.VarChar);
                parameters[21].Value = dsEmp.Tables[0].Rows[0]["PassportNumber"].ToString();

                parameters[22] = new SqlParameter("@EmailServer", SqlDbType.VarChar);
                parameters[22].Value = dsEmp.Tables[0].Rows[0]["EmailServer"].ToString();

                parameters[23] = new SqlParameter("@EmailPort", SqlDbType.VarChar);
                parameters[23].Value = dsEmp.Tables[0].Rows[0]["EmailPort"].ToString();

                parameters[24] = new SqlParameter("@EmailUserName", SqlDbType.VarChar);
                parameters[24].Value = dsEmp.Tables[0].Rows[0]["EmailUserName"].ToString();

                parameters[25] = new SqlParameter("@EmailPassword", SqlDbType.VarChar);
                parameters[25].Value = dsEmp.Tables[0].Rows[0]["EmailPassword"].ToString();

                parameters[26] = new SqlParameter("@SendingMailServer", SqlDbType.VarChar);
                parameters[26].Value = dsEmp.Tables[0].Rows[0]["SendingMailServer"].ToString();

                parameters[27] = new SqlParameter("@SendingEmailPort", SqlDbType.VarChar);
                parameters[27].Value = dsEmp.Tables[0].Rows[0]["SendingEmailPort"].ToString();

                parameters[28] = new SqlParameter("@Gender", SqlDbType.VarChar);
                parameters[28].Value = dsEmp.Tables[0].Rows[0]["Gender"].ToString();

                parameters[29] = new SqlParameter("@CheckNull", SqlDbType.VarChar);
                parameters[29].Value = CheckNull;

                parameters[30] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[30].Value = dsEmp.Tables[0].Rows[0]["CompCode"].ToString();

                parameters[31] = new SqlParameter("@RegionId", SqlDbType.Int);
                parameters[31].Value = dsEmp.Tables[0].Rows[0]["RegionId"].ToString();

                if(dsEmp.Tables[0].Rows[0]["SSL"].ToString() == "")
                {
                    parameters[32] = new SqlParameter("@SSL", SqlDbType.Bit);
                    parameters[32].Value = false;
                }
                else
                {
                    parameters[32] = new SqlParameter("@SSL", SqlDbType.Bit);
                    parameters[32].Value = dsEmp.Tables[0].Rows[0]["SSL"].ToString();
                }
               
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_Sys_SaveEmployee", parameters);

                int empid = Convert.ToInt32(parameters[0].Value.ToString());

                if (isClient)
                {
                    errCode = SaveClentEmpMap(dsEmp, User_Id, empid);
                }
                else
                {
                    errCode = DeleteClentEmpMap(empid);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
            return 0;
        }

        public DataSet GetEmployeeExprience(int EmpId)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;

                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetEmployeeExprience", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;

        }
        public DataSet GetSelectedEmployeeExprience(int EmpId, string CompanyName)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;

                objParam[1] = new SqlParameter("@CompanyName", SqlDbType.VarChar);
                objParam[1].Value = CompanyName;
                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedEmployeeExprience", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;

        }

        public int SaveEmployeeExprience(DataSet dsEmp)
        {
            int errCode = 0;
            try
            {

                SqlParameter[] parameters = new SqlParameter[8];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = dsEmp.Tables[0].Rows[0]["EmpId"];

                parameters[1] = new SqlParameter("@CompanyName", SqlDbType.VarChar);
                parameters[1].Value = dsEmp.Tables[0].Rows[0]["CompanyName"];

                parameters[2] = new SqlParameter("@Designation", SqlDbType.VarChar);
                parameters[2].Value = dsEmp.Tables[0].Rows[0]["Designation"].ToString();

                parameters[3] = new SqlParameter("@StartDate", SqlDbType.DateTime);
                parameters[3].Value = dsEmp.Tables[0].Rows[0]["StartDate"].ToString();

                parameters[4] = new SqlParameter("@EndDate", SqlDbType.DateTime);
                parameters[4].Value = dsEmp.Tables[0].Rows[0]["EndDate"].ToString();

                parameters[5] = new SqlParameter("@NewCompanyName", SqlDbType.VarChar);
                parameters[5].Value = dsEmp.Tables[0].Rows[0]["NewCompanyName"].ToString();

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                parameters[6].Direction = ParameterDirection.InputOutput;

                parameters[7] = new SqlParameter("@JobProfile", SqlDbType.VarChar);
                parameters[7].Value = dsEmp.Tables[0].Rows[0]["JobProfile"].ToString();

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveEmployeeExprience", parameters);
                if (parameters[6] != null)
                    errCode = Convert.ToInt32(parameters[6].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_tbl_SysEmployeeExprience'")
                {
                    errCode = 600001;
                }
                return errCode;
            }
            return errCode;
        }

        public int DeleteClentEmpMap(int empid)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = empid;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeleteClentEmpMap", parameters);

                errCode = Convert.ToInt32(parameters[1].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public int SaveClentEmpMap(DataSet dsEmp, string User_Id, int EmpId)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];

                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = EmpId;

                parameters[1] = new SqlParameter("@ClientRefId", SqlDbType.VarChar);
                parameters[1].Value = EmpId.ToString();

                parameters[2] = new SqlParameter("@EmpName", SqlDbType.VarChar);
                parameters[2].Value = dsEmp.Tables[0].Rows[0]["FirstName"] + " " + dsEmp.Tables[0].Rows[0]["MiddleName"].ToString()
                                    + " " + dsEmp.Tables[0].Rows[0]["LastName"].ToString();

                parameters[3] = new SqlParameter("@ClientType", SqlDbType.VarChar);
                parameters[3].Value = "Employee";

                parameters[4] = new SqlParameter("@emailId", SqlDbType.VarChar);
                parameters[4].Value = dsEmp.Tables[0].Rows[0]["emailId"].ToString();

                parameters[5] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[5].Value = User_Id;

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                parameters[6].Direction = ParameterDirection.InputOutput;


                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveClentEmpMap", parameters);

                errCode = Convert.ToInt32(parameters[6].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet GetUserListNonEmployee(int EmpId)
        {
            DataSet dsEmpList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                dsEmpList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetUserListNonEmployee", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;
        }

        public int DeleteEmployee(int EmpId, DateTime Date, bool Active, string remark,string User_Id)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = EmpId;

                parameters[1] = new SqlParameter("@LastDateOfWork", SqlDbType.DateTime);
                parameters[1].Value = Date;

                parameters[2] = new SqlParameter("@Deleted", SqlDbType.Bit);
                parameters[2].Value = Active;

                parameters[3] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[3].Value = remark;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                parameters[5] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[5].Value = User_Id;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysEmpDelete", parameters);
                if (parameters[4].Value != null)
                    return Convert.ToInt32(parameters[4].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
                return 0;
            }
        }
        public int SysUesrRemove(int EmpId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = EmpId;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysEmpSysUserDelete", parameters);
                if (parameters[1].Value != null)
                    return Convert.ToInt32(parameters[1].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
                return 0;
            }
        }

        public DataSet GetDWCompList()
        {
            DataSet dsobj = new DataSet();
            try
            {

                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCompanyList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }

        public DataSet GetCompanyEmp(string CompCode, string SPName, ArrayList ConnInfo)
        {
            DataSet dsEmpList = new DataSet();
            try
            {

                string ConnectionString = "";
                switch (ConnInfo[1].ToString())
                {
                    case "MySql":
                        ConnectionString = "SERVER=" + ConnInfo[2].ToString() + ";" + "DATABASE=" + ConnInfo[3].ToString() + ";" + "UID=" + ConnInfo[4].ToString() + ";" + "PASSWORD=" + ConnInfo[5].ToString() + ";";
                        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                        {
                            MySqlCommand sqlComm = new MySqlCommand(SPName, connection);
                            sqlComm.CommandType = CommandType.StoredProcedure;
                            MySqlDataAdapter da = new MySqlDataAdapter();
                            da.SelectCommand = sqlComm;
                            da.Fill(dsEmpList);
                        }

                        break;
                    default:
                        ConnectionString = "Data Source=" + ConnInfo[2].ToString() + ";" + "Initial Catalog=" + ConnInfo[3].ToString() + ";" + "uid=" + ConnInfo[4].ToString() + ";" + "pwd=" + ConnInfo[5].ToString() + ";";
                        SqlParameter[] objParam = new SqlParameter[1];
                        objParam[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                        objParam[0].Value = CompCode;
                        dsEmpList = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, SPName, objParam);
                        break;
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsEmpList;
        }
        public int SaveMapping(DataSet dsEmpMapping)
        {

            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@Emp_No", SqlDbType.VarChar);
                parameters[0].Value = dsEmpMapping.Tables[0].Rows[0]["Emp_No"].ToString();


                parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                parameters[1].Value = dsEmpMapping.Tables[0].Rows[0]["CompCode"].ToString(); ;

                parameters[2] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[2].Value = Convert.ToInt32(dsEmpMapping.Tables[0].Rows[0]["EmpId"].ToString());

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;
                DataSet ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveEmployeeMapping", parameters);

                if (parameters[3].Value != null)
                    return Convert.ToInt32(parameters[3].Value.ToString());
                else
                    return 0;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }

        }

        public DataSet GetEmployeeNavRelList(int EmpId)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.VarChar);
                parameters[0].Value = EmpId;
                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetEmployeeNavRelList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }

        public int DeleteEmpMapping(string CompName, int EmpID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@CompName", SqlDbType.VarChar);
                parameters[0].Value = CompName;

                parameters[1] = new SqlParameter("@EmpID", SqlDbType.VarChar);
                parameters[1].Value = EmpID;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DW_DeleteEmpNavRel", parameters);

                if (parameters[2].Value != null)
                    return Convert.ToInt32(parameters[2].Value.ToString());
                else
                    return 0;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }

        public DataSet GetClientEmpName(string CompName, string CompEmpID, ArrayList ConnInfo)
        {
            DataSet dsobj = new DataSet();
            string ConnectionString = "";
            try
            {
                switch (ConnInfo[1].ToString())
                {
                    case "MySql":
                        ConnectionString = "SERVER=" + ConnInfo[2].ToString() + ";" + "DATABASE=" + ConnInfo[3].ToString() + ";" + "UID=" + ConnInfo[4].ToString() + ";" + "PASSWORD=" + ConnInfo[5].ToString() + ";";
                        using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                        {
                            MySqlCommand sqlComm = new MySqlCommand("sp_GetEmployeeName", connection);
                            sqlComm.Parameters.AddWithValue("CompCode", CompEmpID);
                            sqlComm.Parameters.AddWithValue("EmpId", CompEmpID);
                            sqlComm.CommandType = CommandType.StoredProcedure;
                            MySqlDataAdapter da = new MySqlDataAdapter();
                            da.SelectCommand = sqlComm;
                            da.Fill(dsobj);
                        }

                        break;
                    default:
                        ConnectionString = "Data Source=" + ConnInfo[2].ToString() + ";" + "Initial Catalog=" + ConnInfo[3].ToString() + ";" + "uid=" + ConnInfo[4].ToString() + ";" + "pwd=" + ConnInfo[5].ToString() + ";";
                        SqlParameter[] parameters = new SqlParameter[2];
                        parameters[0] = new SqlParameter("@CompCode", SqlDbType.VarChar);
                        parameters[0].Value = CompName;

                        parameters[1] = new SqlParameter("@Emp_Id", SqlDbType.VarChar);
                        parameters[1].Value = CompEmpID;

                        dsobj = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "sp_GetEmployeeName", parameters);
                        break;
                }



            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return dsobj;
        }

        public int SaveEmployeeDepartment(int EmpId,int DeptId,string User_Id)
        {
            int ErrCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = EmpId;
            
                parameters[1] = new SqlParameter("@DeptId", SqlDbType.Int);
                parameters[1].Value = DeptId;

                parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[2].Value = User_Id;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveEmployeeDepartment", parameters);
                if (parameters[3] != null)
                    ErrCode = Convert.ToInt32(parameters[3].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        public int DeleteEmployeeDepartment(int EmpID,int DeptId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = EmpID;

                parameters[1] = new SqlParameter("@DeptId", SqlDbType.Int);
                parameters[1].Value = DeptId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeleteEmployeeDepartment", parameters);

                if (parameters[2].Value != null)
                    return Convert.ToInt32(parameters[2].Value.ToString());
                else
                    return 0;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }
        #endregion Employee
        #region EmolyeeCustomerDetails
        public DataSet GetEmolyeeCustomerDetails(int EmpId)
        {
            DataSet dsRoleRpt = new DataSet();
            try
            {
                SqlParameter paramter = new SqlParameter("@EmpId", SqlDbType.NVarChar);
                paramter.Value = EmpId;
                dsRoleRpt = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetEmployeeCustomerDetails", paramter);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsRoleRpt;
        }
        public int SaveEmployeeCustomer(DataSet Ds, int EmpId, string User_Id,int DeptId)
        {
            int iErrorCode = 0;
            SqlTransaction sqltransaction;
            SqlConnection sqlconn = new SqlConnection(Common.SqlConnectionString);
            sqlconn.Open();
            sqltransaction = sqlconn.BeginTransaction();
            try
            {
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                    param[0].Value = Ds.Tables["tbl_SysEmployeeCustomer"].Rows[0]["EmpId"].ToString();

                    param[1] = new SqlParameter("@DeptId", SqlDbType.Int);
                    param[1].Value = DeptId;
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysDeleteCustomerByEmpId", param);
                }
                else
                {
                    SqlParameter[] paramter = new SqlParameter[1];

                    SqlParameter[] param = new SqlParameter[2];
                    param[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                    param[0].Value = EmpId;

                    param[1] = new SqlParameter("@DeptId", SqlDbType.Int);
                    param[1].Value = DeptId;
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysDeleteCustomerByEmpId", param);
                }

                foreach (DataRow dr in Ds.Tables["tbl_SysEmployeeCustomer"].Rows)
                {
                    iErrorCode = 0;
                    SqlParameter[] SysRoleRptParam = new SqlParameter[5];

                    SysRoleRptParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                    SysRoleRptParam[0].Value = dr["EmpId"].ToString();
                    SysRoleRptParam[1] = new SqlParameter("@CustomerId", SqlDbType.Int);
                    SysRoleRptParam[1].Value = dr["CustomerId"].ToString();
                    SysRoleRptParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    SysRoleRptParam[2].Value = 0;
                    SysRoleRptParam[2].Direction = ParameterDirection.InputOutput;
                    SysRoleRptParam[3] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                    SysRoleRptParam[3].Value = User_Id;
                    SysRoleRptParam[4] = new SqlParameter("@DeptId", SqlDbType.NVarChar);
                    SysRoleRptParam[4].Value = DeptId;
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysSaveEmployeeCustomer", SysRoleRptParam);
                    iErrorCode = Convert.ToInt32(SysRoleRptParam[2].Value);
                    if (iErrorCode != 500002)
                    {
                        sqltransaction.Rollback();
                        sqlconn.Close();
                        return Convert.ToInt32(SysRoleRptParam[2].Value.ToString());
                    }
                }


                sqltransaction.Commit();

            }
            catch (Exception ex)
            {
                sqltransaction.Rollback();
                Common.LogException(ex);
            }

            return iErrorCode;
        }
        #endregion EmolyeeCustomerDetails
        #region EmployeeVendorDetail
        public DataSet GetEmolyeeVendorDetails(int EmpId)
        {
            DataSet dsRoleRpt = new DataSet();
            try
            {
                SqlParameter paramter = new SqlParameter("@EmpId", SqlDbType.Int);
                paramter.Value = EmpId;
                dsRoleRpt = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetEmployeeVendorDetails", paramter);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsRoleRpt;
        }
        public int SaveVendorCustomer(DataSet Ds, int EmpId, string User_Id)
        {
            int iErrorCode = 0;
            SqlTransaction sqltransaction;
            SqlConnection sqlconn = new SqlConnection(Common.SqlConnectionString);
            sqlconn.Open();
            sqltransaction = sqlconn.BeginTransaction();
            try
            {
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@EmpId", SqlDbType.NVarChar);
                    param[0].Value = Ds.Tables["tbl_SysEmployeeVendor"].Rows[0]["EmpId"].ToString();
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysDeleteVendorByEmpId", param);
                }
                else
                {


                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@EmpId", SqlDbType.NVarChar);
                    param[0].Value = EmpId;
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysDeleteVendorByEmpId", param);
                }

                foreach (DataRow dr in Ds.Tables["tbl_SysEmployeeVendor"].Rows)
                {
                    iErrorCode = 0;
                    SqlParameter[] SysRoleRptParam = new SqlParameter[4];

                    SysRoleRptParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                    SysRoleRptParam[0].Value = dr["EmpId"].ToString();
                    SysRoleRptParam[1] = new SqlParameter("@VendorId", SqlDbType.Int);
                    SysRoleRptParam[1].Value = dr["VendorId"].ToString();
                    SysRoleRptParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    SysRoleRptParam[2].Value = 0;
                    SysRoleRptParam[2].Direction = ParameterDirection.InputOutput;
                    SysRoleRptParam[3] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                    SysRoleRptParam[3].Value = User_Id;
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysSaveEmployeeVendor", SysRoleRptParam);
                    iErrorCode = Convert.ToInt32(SysRoleRptParam[2].Value);
                    if (iErrorCode != 500002)
                    {
                        sqltransaction.Rollback();
                        sqlconn.Close();
                        return Convert.ToInt32(SysRoleRptParam[2].Value.ToString());
                    }
                }


                sqltransaction.Commit();

            }
            catch (Exception ex)
            {
                sqltransaction.Rollback();
                Common.LogException(ex);
            }

            return iErrorCode;
        }
        #endregion EmployeeVendorDetail

        #region Assets
        public DataSet SysAssetsList(int AssignmentId,string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@AssignmentId", SqlDbType.Int);
                objParam[0].Value = AssignmentId;

                objParam[1] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                objParam[1].Value = UserId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysAssetsList", objParam);

            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return ds;
        }

        public DataSet SysAllAssetsListByEmployee()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetAllEmployeeAssets");

            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSysAssetTypelist(string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetAssetTypeList", ObjParam);
                return ds;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public DataSet GetSelectedSysAssetForEdit(int AssetId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@AssetId", SqlDbType.Int);
                objParam[0].Value = AssetId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelecteSysAsset", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedSysAssetTypeForEdit(int AssetTypeId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@AssetTypeId", SqlDbType.VarChar);
                objParam[0].Value = AssetTypeId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedAssetType", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int SaveSysAssetDetails(DataSet ds, string User_ID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[17];

                parameters[0] = new SqlParameter("@AssetId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["AssetId"];

                parameters[1] = new SqlParameter("@AssetTypeId", SqlDbType.Int);
                parameters[1].Value = ds.Tables[0].Rows[0]["AssetTypeId"];

                parameters[2] = new SqlParameter("@AssetName", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["AssetName"];

                parameters[3] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[0].Rows[0]["Description"];

                parameters[4] = new SqlParameter("@AssetInDate", SqlDbType.DateTime);
                parameters[4].Value = ds.Tables[0].Rows[0]["AssetInDate"];

                parameters[5] = new SqlParameter("@ManufacturerDetails", SqlDbType.VarChar);
                parameters[5].Value = ds.Tables[0].Rows[0]["ManufacturerDetails"];

                parameters[6] = new SqlParameter("@Photo", SqlDbType.Image);
                parameters[6].Value = ds.Tables[0].Rows[0]["Photo"];

                parameters[7] = new SqlParameter("TotalQty", SqlDbType.Float);
                parameters[7].Value = ds.Tables[0].Rows[0]["TotalQty"];

                parameters[8] = new SqlParameter("@Cost", SqlDbType.Float);
                parameters[8].Value = ds.Tables[0].Rows[0]["Cost"];

                parameters[9] = new SqlParameter("@DepRate", SqlDbType.Float);
                parameters[9].Value = ds.Tables[0].Rows[0]["DepRate"];

                parameters[10] = new SqlParameter("@OfficeLocationId", SqlDbType.Int);
                parameters[10].Value = ds.Tables[0].Rows[0]["OfficeLocationId"];

                parameters[11] = new SqlParameter("@LocationId", SqlDbType.Int);
                parameters[11].Value = ds.Tables[0].Rows[0]["LocationId"];

                parameters[12] = new SqlParameter("@AssetAcRef", SqlDbType.VarChar);
                parameters[12].Value = ds.Tables[0].Rows[0]["AssetAcRef"];

                parameters[13] = new SqlParameter("@DisposalDate", SqlDbType.DateTime);
                parameters[13].Value = ds.Tables[0].Rows[0]["DisposalDate"];

                parameters[14] = new SqlParameter("@DisposalValue", SqlDbType.Float);
                parameters[14].Value = ds.Tables[0].Rows[0]["DisposalValue"];

                parameters[15] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[15].Value = User_ID;

                parameters[16] = new SqlParameter("@Errorcode", SqlDbType.Int);
                parameters[16].Value = 0;
                parameters[16].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveSysAsset", parameters);
                if (parameters[16].Value != null)
                    return Convert.ToInt32(parameters[16].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }

        }

        public int SaveAssetTypeDetails(DataSet ds,string User_Id)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];

                parameters[0] = new SqlParameter("@AssetTypeId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["AssetTypeId"];

                parameters[1] = new SqlParameter("@ParentAssetTypeId", SqlDbType.Int);
                parameters[1].Value = ds.Tables[0].Rows[0]["ParentAssetTypeId"];

                parameters[2] = new SqlParameter("@AssetType", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["AssetType"];

                parameters[3] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[3].Value = ds.Tables[0].Rows[0]["Description"];

                parameters[4] = new SqlParameter("@Errorcode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                parameters[5] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[5].Value = User_Id;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveAssetType", parameters);
                if (parameters[4].Value != null)
                    return Convert.ToInt32(parameters[4].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }

        }
        public int DeleteSysAssetsDetails(int AssetId,string User_Id)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@AssetId", SqlDbType.Int);
                parameters[0].Value = AssetId;
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_UpdateSysAssets", parameters);
                if (parameters[0].Value != null)
                    return Convert.ToInt32(parameters[0].Value.ToString());
                else
                    return 0;
            }


            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetSysAssetAllocationlist(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysAssetAllocationList", parameters);
                return ds;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public DataSet GetSelectedSysAssetAllocation(int AssignmentId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@AssignmentId", SqlDbType.VarChar);
                objParam[0].Value = AssignmentId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedAssetAllocation", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public int SaveSysAssetAllocationDetails(DataSet ds, string User_ID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[8];

                parameters[0] = new SqlParameter("@AssignmentId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["AssignmentId"];

                parameters[1] = new SqlParameter("@AssetId", SqlDbType.Int);
                parameters[1].Value = ds.Tables[0].Rows[0]["AssetId"];

                parameters[2] = new SqlParameter("@AssignedTo", SqlDbType.Int);
                parameters[2].Value = ds.Tables[0].Rows[0]["AssignedTo"];

                parameters[3] = new SqlParameter("@AssignedDate", SqlDbType.DateTime);
                parameters[3].Value = ds.Tables[0].Rows[0]["AssignedDate"];

                parameters[4] = new SqlParameter("@ReturnDate", SqlDbType.DateTime);
                parameters[4].Value = ds.Tables[0].Rows[0]["ReturnDate"];

                parameters[5] = new SqlParameter("@AssignedType", SqlDbType.NVarChar);
                parameters[5].Value = ds.Tables[0].Rows[0]["AssignedType"];

                parameters[6] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[6].Value = User_ID;

                parameters[7] = new SqlParameter("@Errorcode", SqlDbType.Int);
                parameters[7].Value = 0;
                parameters[7].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveAssetAllocation", parameters);
                if (parameters[7].Value != null)
                    return Convert.ToInt32(parameters[7].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }

        }


        public int UpdateAssetApproval(int AssignmentId, string User_Id, int Status)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];

                parameters[0] = new SqlParameter("@AssignmentId", SqlDbType.Int);
                parameters[0].Value = AssignmentId;
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;

                parameters[2] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[2].Value = Status;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysUpdateAssetAllocationApproval", parameters);
                if (parameters[0].Value != null)
                    return Convert.ToInt32(parameters[0].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }




        #endregion Assets

        #region[RoleReportMapping]
        public DataSet GetReportListByRole(string strRoleID)
        {
            DataSet dsRoleRpt = new DataSet();
            try
            {
                SqlParameter paramter = new SqlParameter("@RoleId", SqlDbType.NVarChar);
                paramter.Value = strRoleID;
                dsRoleRpt = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetRoleReportList", paramter);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsRoleRpt;
        }

        public int SaveRoleReportMapping(DataSet dsRoleRpt, string RoleID)
        {
            int iErrorCode = 0;
            SqlTransaction sqltransaction;
            SqlConnection sqlconn = new SqlConnection(Common.SqlConnectionString);
            sqlconn.Open();
            sqltransaction = sqlconn.BeginTransaction();
            try
            {
                if (dsRoleRpt.Tables[0].Rows.Count > 0)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@RoleId", SqlDbType.NVarChar);
                    param[0].Value = dsRoleRpt.Tables["tbl_SysRoleReport"].Rows[0]["RoleId"].ToString();
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysDeleteRoleReport", param);
                }
                else
                {
                    SqlParameter paramter = new SqlParameter("@RoleId", SqlDbType.NVarChar);
                    paramter.Value = RoleID;
                    dsRoleRpt = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeleteReportBYRoleId", paramter);
                }

                foreach (DataRow dr in dsRoleRpt.Tables["tbl_SysRoleReport"].Rows)
                {
                    iErrorCode = 0;
                    SqlParameter[] SysRoleRptParam = new SqlParameter[3];

                    SysRoleRptParam[0] = new SqlParameter("@RoleId", SqlDbType.NVarChar);
                    SysRoleRptParam[0].Value = dr["RoleId"].ToString();
                    SysRoleRptParam[1] = new SqlParameter("@ReportId", SqlDbType.VarChar);
                    SysRoleRptParam[1].Value = dr["ReportId"].ToString();
                    SysRoleRptParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    SysRoleRptParam[2].Value = 0;
                    SysRoleRptParam[2].Direction = ParameterDirection.InputOutput;

                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysSaveRoleReportMapping", SysRoleRptParam);
                    iErrorCode = Convert.ToInt32(SysRoleRptParam[2].Value);
                    if (iErrorCode != 6000001)
                    {
                        sqltransaction.Rollback();
                        sqlconn.Close();
                        return Convert.ToInt32(SysRoleRptParam[2].Value.ToString());
                    }
                }


                sqltransaction.Commit();

            }
            catch (Exception ex)
            {
                sqltransaction.Rollback();
                Common.LogException(ex);
            }

            return iErrorCode;
        }
        #endregion

        #region User DashBoard Mapping
        public DataSet GetDashBordList(int UserId)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@UserId", SqlDbType.Int);
                parameters[0].Value = UserId;

                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDashBoardModelList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public DataSet GetDashBordDrpDwnList()
        {
            DataSet dsobj = new DataSet();
            try
            {
                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDashBoardDrpdwnList");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }

        public int SaveAssignDashBoard(DataSet dsSaveDashBoard, int UserId)
        {
            int ErrCode = 0;
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            try
            {
                DataSet dsobj = new DataSet();

                SqlParameter[] Delparameters = new SqlParameter[2];
                Delparameters[0] = new SqlParameter("@UserId", SqlDbType.Int);
                Delparameters[0].Value = UserId;

                Delparameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Delparameters[1].Value = 0;
                Delparameters[1].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeleteDashBoardModel", Delparameters);
                ErrCode = Convert.ToInt32(Delparameters[1].Value.ToString());

                if (ErrCode == 600001)
                {
                    SqlParameter[] parameters = new SqlParameter[4];
                    foreach (DataRow dr in dsSaveDashBoard.Tables[0].Rows)
                    {

                        parameters[0] = new SqlParameter("@UserId", SqlDbType.Int);
                        parameters[0].Value = Convert.ToInt32(dr["UserId"]);


                        parameters[1] = new SqlParameter("@DashboardId", SqlDbType.SmallInt);
                        parameters[1].Value = Convert.ToInt16(dr["DashboardId"]);

                        parameters[2] = new SqlParameter("@Sequence", SqlDbType.SmallInt);
                        parameters[2].Value = Convert.ToInt16(dr["Sequence"]);

                        parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                        parameters[3].Value = 0;
                        parameters[3].Direction = ParameterDirection.InputOutput;
                        DataSet ds = SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_SysSaveAssignDashBoard", parameters);
                        if (parameters[3].Value != null)
                        {
                            if (Convert.ToInt32(parameters[3].Value.ToString()) != 600004)
                            {
                                sqlTransaction.Rollback();
                                sqlConn.Close();
                                return 1;
                            }
                        }
                        else
                        {
                            sqlTransaction.Rollback();
                            sqlConn.Close();
                            return 2;
                        }
                    }

                    sqlTransaction.Commit();
                    sqlConn.Close();
                    return 4;
                }
                else
                {
                    sqlTransaction.Rollback();
                    sqlConn.Close();
                    return 3;
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ErrCode;
        }
        #endregion User DAshBoard Mapping

        #region  Client Application
        public DataSet GetClientAppList(string UserId)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetClientAppList", ObjParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }

        public DataSet GetSelectedClientApp(Int16 ClientAppId)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter("@ClientAppId", SqlDbType.SmallInt);
                parameters[0].Value = ClientAppId;

                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedClientApp", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public int SaveClientApp(DataSet dsObj, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[7];

                parameters[0] = new SqlParameter("@ClientAppId", SqlDbType.SmallInt);
                parameters[0].Value = dsObj.Tables[0].Rows[0]["ClientAppId"];

                parameters[1] = new SqlParameter("@AppName", SqlDbType.VarChar);
                parameters[1].Value = dsObj.Tables[0].Rows[0]["AppName"];

                parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[2].Value = dsObj.Tables[0].Rows[0]["Description"];

                parameters[3] = new SqlParameter("@LoginName", SqlDbType.VarChar);
                parameters[3].Value = dsObj.Tables[0].Rows[0]["LoginName"];

                parameters[4] = new SqlParameter("@Password", SqlDbType.VarChar);
                parameters[4].Value = dsObj.Tables[0].Rows[0]["Password"];

                parameters[5] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[5].Value = User_Id;

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                parameters[6].Direction = ParameterDirection.InputOutput;


                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveClientApp", parameters);

                errCode = Convert.ToInt32(parameters[6].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion  Client Application

        #region  Company
        public DataSet GetCompanyList(string UserId)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_sysGetCompanyLst", ObjParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }

        public DataSet GetSelectedCompany(Int16 CompId)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter("@CompId", SqlDbType.SmallInt);
                parameters[0].Value = CompId;

                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_sysGetSelectedCompany", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }

        public int SaveCompany(DataSet dsObj,string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[17];

                parameters[0] = new SqlParameter("@CompId", SqlDbType.SmallInt);
                parameters[0].Value = dsObj.Tables[0].Rows[0]["CompId"];

                parameters[1] = new SqlParameter("@Name", SqlDbType.VarChar);
                parameters[1].Value = dsObj.Tables[0].Rows[0]["Name"];

                parameters[2] = new SqlParameter("@ShortName", SqlDbType.VarChar);
                parameters[2].Value = dsObj.Tables[0].Rows[0]["ShortName"];

                parameters[3] = new SqlParameter("@AddressLine1", SqlDbType.VarChar);
                parameters[3].Value = dsObj.Tables[0].Rows[0]["AddressLine1"];

                parameters[4] = new SqlParameter("@AddressLine2", SqlDbType.VarChar);
                parameters[4].Value = dsObj.Tables[0].Rows[0]["AddressLine2"];

                parameters[5] = new SqlParameter("@City", SqlDbType.VarChar);
                parameters[5].Value = dsObj.Tables[0].Rows[0]["City"];

                parameters[6] = new SqlParameter("@State", SqlDbType.VarChar);
                parameters[6].Value = dsObj.Tables[0].Rows[0]["State"];

                parameters[7] = new SqlParameter("@Country", SqlDbType.VarChar);
                parameters[7].Value = dsObj.Tables[0].Rows[0]["Country"];

                parameters[8] = new SqlParameter("@Pin", SqlDbType.VarChar);
                parameters[8].Value = dsObj.Tables[0].Rows[0]["Pin"];

                parameters[9] = new SqlParameter("@TagLine", SqlDbType.VarChar);
                parameters[9].Value = dsObj.Tables[0].Rows[0]["TagLine"];

                parameters[10] = new SqlParameter("@Logo", SqlDbType.VarBinary);
                parameters[10].Value = dsObj.Tables[0].Rows[0]["Logo"];

                parameters[11] = new SqlParameter("@Cst", SqlDbType.VarChar);
                parameters[11].Value = dsObj.Tables[0].Rows[0]["Cst"];

                parameters[12] = new SqlParameter("@Bst", SqlDbType.VarChar);
                parameters[12].Value = dsObj.Tables[0].Rows[0]["Bst"];

                parameters[13] = new SqlParameter("@ServiceTN", SqlDbType.VarChar);
                parameters[13].Value = dsObj.Tables[0].Rows[0]["ServiceTN"];

                parameters[14] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[14].Value = 0;
                parameters[14].Direction = ParameterDirection.InputOutput;

                parameters[15] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[15].Value = User_Id;


                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveCompany", parameters);

                errCode = Convert.ToInt32(parameters[14].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        #endregion  Company

        #region Approval And Review Pendding List
        public DataSet PenddingApproverlList(string User_Id)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetApprovalTaskList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }

        public DataSet PenddingReviewlList(string User_Id)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetReviewTaskList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }

        #endregion Approval And Review Pendding List

        public int CreateUserLog(string User_Name)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Name", SqlDbType.VarChar);
                parameters[0].Value = User_Name;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysUserLogSave", parameters);

                errCode = Convert.ToInt32(parameters[1].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        #region Bank Detail List
        public DataSet GetBankList(string CompCode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@BankCode", SqlDbType.VarChar);
                parameters[0].Value = CompCode;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptBankName", parameters);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetCustomerIdList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_rptCustomerId", parameters);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedEmployeeBankDetail(int EmpId, string AccountNo)
        {
            DataSet dsObj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = EmpId;

                parameters[1] = new SqlParameter("@AccountNo", SqlDbType.VarChar);
                parameters[1].Value = AccountNo;

                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysEmployeeBankGetSelected", parameters);
            }
            catch (Exception)
            {
                throw;
            }
            return dsObj;
        }

        public int SaveEmployeeBankDetailsInfo(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[8];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[1].Rows[0]["EmpId"];

                parameters[1] = new SqlParameter("@BankName", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[1].Rows[0]["BankName"];

                parameters[2] = new SqlParameter("@AccountNo", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[1].Rows[0]["AccountNo"];

                parameters[3] = new SqlParameter("@Limit", SqlDbType.Float);
                parameters[3].Value = ds.Tables[1].Rows[0]["Limit"];

                parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@NewAccountNo", SqlDbType.NVarChar);
                parameters[5].Value = ds.Tables[1].Rows[0]["NewAccountNo"];

                parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[6].Value = 0;
                parameters[6].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveEmployeeBankDetails", parameters);
                if (parameters[6].Value != null)
                    errCode = Convert.ToInt32(parameters[6].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_tbl_SysEmployeeBankDetails'")
                {
                    errCode = 500000;
                }
                return errCode;
            }
            return errCode;
        }

        #endregion Bank Detail List

        #region Address Work

        public int SaveAdressDeatils(DataSet ds, int EmpId, Boolean isPrimary, string Description, string PhotoPath)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[13];

                parameters[0] = new SqlParameter("@AddressId", SqlDbType.VarChar);
                parameters[0].Value = ds.Tables[0].Rows[0]["AddressId"];

                parameters[1] = new SqlParameter("@Line1", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["Line1"];

                parameters[2] = new SqlParameter("@Line2", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["Line2"];

                parameters[3] = new SqlParameter("@State", SqlDbType.Int);
                parameters[3].Value = ds.Tables[0].Rows[0]["StateId"];

                parameters[4] = new SqlParameter("@City", SqlDbType.Int);
                parameters[4].Value = ds.Tables[0].Rows[0]["CityId"];

                parameters[5] = new SqlParameter("@Country", SqlDbType.Int);
                parameters[5].Value = ds.Tables[0].Rows[0]["CountryId"];

                parameters[6] = new SqlParameter("@Pin", SqlDbType.VarChar);
                parameters[6].Value = ds.Tables[0].Rows[0]["Pin"];

                parameters[7] = new SqlParameter("@LandMark", SqlDbType.VarChar);
                parameters[7].Value = ds.Tables[0].Rows[0]["LandMark"];

                parameters[8] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[8].Value = EmpId;

                parameters[9] = new SqlParameter("@isPrimary", SqlDbType.Bit);
                parameters[9].Value = isPrimary;

                parameters[10] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[10].Value = Description;

                parameters[11] = new SqlParameter("@PhotoPath", SqlDbType.VarChar);
                parameters[11].Value = PhotoPath;

                parameters[12] = new SqlParameter("@Errorcode", SqlDbType.Int);
                parameters[12].Value = 0;
                parameters[12].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveEmployeeAdress", parameters);
                if (parameters[12].Value != null)
                    return Convert.ToInt32(parameters[12].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }

        }

        public DataSet GetSelectedAddressForEdit(int AddressId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@AddressId", SqlDbType.Int);
                objParam[0].Value = AddressId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedAddress", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public int DeleteEmployeeCustomer(int EmpId, int CustomerId, string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                objParam[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[1].Value = EmpId;
                objParam[2] = new SqlParameter("@CustomerId", SqlDbType.Int);
                objParam[2].Value = CustomerId;
                objParam[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[3].Value = 0;
                objParam[3].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeleteEmployeeCustomer", objParam);
                if (objParam[3].Value != null)
                    return Convert.ToInt32(objParam[3].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }

        public int SaveEmployeeCustomer(int EmpId, int CustomerId, string User_Id,int AssignDept)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[5];
                objParam[0] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                objParam[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[1].Value = EmpId;
                objParam[2] = new SqlParameter("@CustomerId", SqlDbType.Int);
                objParam[2].Value = CustomerId;
                objParam[3] = new SqlParameter("@DeptId", SqlDbType.Int);
                objParam[3].Value = AssignDept;
                objParam[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[4].Value = 0;
                objParam[4].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveEmployeeCustomer", objParam);
                if (objParam[3].Value != null)
                    return Convert.ToInt32(objParam[3].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }

        public DataSet GetEmpCustbyDept(int EmpId,int DeptId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;

                objParam[1] = new SqlParameter("@DeptId", SqlDbType.Int);
                objParam[1].Value = DeptId;
                ds= SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetEmployeeCustomerByDeptId", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public int DeleteEmployeeVendor(int EmpId, int VendorId, string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                objParam[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[1].Value = EmpId;
                objParam[2] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[2].Value = VendorId;
                objParam[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[3].Value = 0;
                objParam[3].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeleteEmployeeVendor", objParam);
                if (objParam[3].Value != null)
                    return Convert.ToInt32(objParam[3].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }
        public int SaveEmployeeVendor(int EmpId, int VendorId, string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                objParam[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[1].Value = EmpId;
                objParam[2] = new SqlParameter("@VendorId", SqlDbType.Int);
                objParam[2].Value = VendorId;
                objParam[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[3].Value = 0;
                objParam[3].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveEmployeeVendor", objParam);
                if (objParam[3].Value != null)
                    return Convert.ToInt32(objParam[3].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }

        public DataSet GetEmployeeVendorListForDropDown(int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWVendorlistForDropDown", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }

        public DataSet GetEmployeeCustomerListForDropDown(int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCustomerlistForDropDown", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet AddressListByEmployee(int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysAddressByEmployee", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetCountryList()
        {
            DataSet DSObj = new DataSet();
            try
            {
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetCountryList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public DataSet GetStateList(int CountryId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@CountryId", SqlDbType.Int);
                objParam[0].Value = CountryId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetStateList", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public DataSet GetEmployeeByUserId(string User_ID)
        {
            DataSet dsemp;
            try
            {
                SqlParameter sysUSerid = new SqlParameter("@User_ID", SqlDbType.NVarChar);

                sysUSerid.Value = User_ID;
                dsemp = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetEmployeeByUserId", sysUSerid);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return null;
            }
            return dsemp;
        }
        public DataSet GetBudgetReport(string user_Id)
        {
            DataSet Ds = new DataSet();
            try
            {
                SqlParameter[] param = new SqlParameter[1];
                param[0] = new SqlParameter("@user_Id", SqlDbType.VarChar);
                param[0].Value = user_Id;

                Ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWBudgetAllocationReport", param);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Ds;
        }
        public DataSet GetCityList(int State)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];
                objParam[0] = new SqlParameter("@State", SqlDbType.Int);
                objParam[0].Value = State;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetCityList", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        #endregion Address Work 


        #region salary Component

        public DataSet GetSalaryComponentList(string UserId)
        {
            DataSet dsSalaryList = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id",SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
                dsSalaryList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysgetsalarycomponentList", ObjParam);

            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
                
            }
            return dsSalaryList;
        }
        public DataSet GetSalaryStructureDetail(int EmpId)
        {
            DataSet dsSalaryList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId ", SqlDbType.Int);
                objParam[0].Value = EmpId;
                dsSalaryList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetEmpSlryStructureDetail", objParam);

            }
            catch (Exception ex)
            {
                SmartSys.Utility.Common.LogException(ex);
            }
            return dsSalaryList;
        }
        public DataSet GetParentComponentList(int ComponentId, int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@ComponentId ", SqlDbType.Int);
                objParam[0].Value = ComponentId;


                objParam[1] = new SqlParameter("@EmpId ", SqlDbType.Int);
                objParam[1].Value = EmpId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_TMParentSalaryComponent", objParam);
            }
            catch (Exception ex)
            {
                //Common.LogException(ex);
                throw ex;
            }
            return ds;
        }
        public DataSet GetSelectedSalaryComponent(int ComponentId)
        {
            DataSet dsSalaryList = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ComponentId", SqlDbType.Int);
                objParam[0].Value = ComponentId;
                dsSalaryList = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedSalaryComponent", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsSalaryList;

        }

        public int SaveSalary(DataSet dsEdssalary, string User_Id, bool Taxable, bool Fixed)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[11];
                parameters[0] = new SqlParameter("@ComponentId", SqlDbType.Int);
                parameters[0].Value = dsEdssalary.Tables[0].Rows[0]["ComponentId"];

                parameters[1] = new SqlParameter("@Name", SqlDbType.VarChar);
                parameters[1].Value = dsEdssalary.Tables[0].Rows[0]["Name"];

                parameters[2] = new SqlParameter("@DRCR", SqlDbType.VarChar);
                parameters[2].Value = dsEdssalary.Tables[0].Rows[0]["DRCR"].ToString();

                parameters[3] = new SqlParameter("@Frequency", SqlDbType.VarChar);
                parameters[3].Value = dsEdssalary.Tables[0].Rows[0]["Frequency"].ToString();

                parameters[4] = new SqlParameter("@Taxable", SqlDbType.VarChar);
                parameters[4].Value = Taxable;

                parameters[5] = new SqlParameter("@Fixed", SqlDbType.Bit);
                parameters[5].Value = Fixed;

                parameters[6] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[6].Value = User_Id;

                parameters[7] = new SqlParameter("@VariableRate", SqlDbType.NVarChar);
                parameters[7].Value = dsEdssalary.Tables[0].Rows[0]["VariableRate"].ToString();

                parameters[8] = new SqlParameter("@ParentComponentId", SqlDbType.NVarChar);
                parameters[8].Value = dsEdssalary.Tables[0].Rows[0]["ParentComponentId"].ToString();

                parameters[9] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[9].Value = 0;
                parameters[9].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveSalaryDetail", parameters);
                if (parameters[9].Value != null)
                    return Convert.ToInt32(parameters[9].Value.ToString());
                else
                    return 0;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }

        }
        public int DeleteEmployeeSalaryStructure(int EmpId, int ComponentId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ComponentId", SqlDbType.Int);
                parameters[0].Value = ComponentId;

                parameters[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[1].Value = EmpId;

                parameters[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[2].Value = 0;
                parameters[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeleteEmpSlryStructureDetail", parameters);
                if (parameters[2].Value != null)
                    return Convert.ToInt32(parameters[2].Value.ToString());
                else
                    return 0;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }

        }
        public int saveSalaryStructure(DataTable dt, string User_Id)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[11];
                parameters[0] = new SqlParameter("@ComponentId", SqlDbType.Int);
                parameters[0].Value = dt.Rows[0]["ComponentId"];

                parameters[1] = new SqlParameter("@Amount", SqlDbType.Float);
                parameters[1].Value = dt.Rows[0]["Amount"];

                parameters[2] = new SqlParameter("@EmpId", SqlDbType.VarChar);
                parameters[2].Value = dt.Rows[0]["EmpId"];

                parameters[3] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveSalaryStructureDetail", parameters);
                if (parameters[4].Value != null)
                    return Convert.ToInt32(parameters[4].Value.ToString());
                else
                    return 0;
            }

            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }

        }
        #endregion salary Component


        #region Sys_Issue_Details

        public DataSet GetIssueList(string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetIssueList", ObjParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetselectedIssue(int IssueId, string User_Id)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@IssueId", SqlDbType.Int);
                parameters[0].Value = IssueId;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedIssue", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public int SaveIssueDetail(DataSet ds, string User_Id, string Status, string Comments, string Attachment, string TicketType)
        {
            SqlParameter[] parameters = new SqlParameter[11];
            parameters[0] = new SqlParameter("@IssueId", SqlDbType.Int);
            parameters[0].Value = ds.Tables[0].Rows[0]["IssueId"].ToString();
            parameters[0].Direction = ParameterDirection.InputOutput;

            parameters[1] = new SqlParameter("@Title", SqlDbType.VarChar);
            parameters[1].Value = ds.Tables[0].Rows[0]["Title"].ToString();

            parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
            parameters[2].Value = ds.Tables[0].Rows[0]["Description"].ToString();

            parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            parameters[3].Value = User_Id;

            parameters[4] = new SqlParameter("@Statuscode", SqlDbType.Int);
            parameters[4].Value = Status;

            parameters[5] = new SqlParameter("@TicketType", SqlDbType.VarChar);
            parameters[5].Value = TicketType;
            parameters[6] = new SqlParameter("@Comments", SqlDbType.VarChar);
            parameters[6].Value = Comments;

            parameters[7] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[7].Value = 0;
            parameters[7].Direction = ParameterDirection.InputOutput;
            if (Attachment == null)
            {
                parameters[8] = new SqlParameter("@Attachment", SqlDbType.VarChar);
                parameters[8].Value = 0;
            }
            else
            {
                parameters[8] = new SqlParameter("@Attachment", SqlDbType.VarChar);
                parameters[8].Value = Attachment;
            }
            parameters[9] = new SqlParameter("@EstimatedHours", SqlDbType.Float);
            parameters[9].Value = ds.Tables[0].Rows[0]["EstimatedHours"].ToString();

            parameters[10] = new SqlParameter("@DevDescription", SqlDbType.VarChar);
            parameters[10].Value = ds.Tables[0].Rows[0]["DevDescription"].ToString();
            try
            {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveIssueTask", parameters);
                if (parameters[0].Value != null)
                    return Convert.ToInt32(parameters[0].Value.ToString());

                else
                    return 0;

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }

        public DataSet GetSelectedIssueList(int IssueId, string User_Id)
        {
            DataSet dsData = new DataSet();
            try
            {

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@IssueId", SqlDbType.Int);
                parameters[0].Value = IssueId;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;

                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedIssue", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsData;
        }

        public DataSet GetSelectedProjectCommentList(int IssueId)
        {
            DataSet dsData = new DataSet();
            try
            {

                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@IssueId", SqlDbType.Int);
                parameters[0].Value = IssueId;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysCommentList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsData;
        }

        public DataSet GetIssueAprovalList(string User_Id)
        {
            DataSet dsData = new DataSet();
            try
            {

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysIssueListForApproval", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsData;
        }
        public DataSet GetSubordinateIssueAprovalList(string User_Id)
        {
            DataSet dsData = new DataSet();
            try
            {

                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSubordinateIssueListForApproval", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsData;
        }
        public int EstimationDone(int IssueId, float EstimationHours, int statusid, string User_Id)
        {

            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@IssueId", SqlDbType.Int);
                parameters[0].Value = IssueId;

                parameters[1] = new SqlParameter("@EstimationHours", SqlDbType.Float);
                parameters[1].Value = EstimationHours;

                parameters[2] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[2].Value = statusid;
                parameters[2].Direction = ParameterDirection.InputOutput;

                parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[3].Value = User_Id;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_sysIssueEstimationUpdation", parameters);
                return Convert.ToInt32(parameters[2].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateSysIssue(int IssueId, string User_Id, int statusid, string Comments)
        {

            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@IssueId", SqlDbType.Int);
                parameters[0].Value = IssueId;

                parameters[1] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[1].Value = User_Id;

                parameters[2] = new SqlParameter("@Status", SqlDbType.Int);
                parameters[2].Value = statusid;
                parameters[2].Direction = ParameterDirection.InputOutput;

                parameters[3] = new SqlParameter("@Comment", SqlDbType.VarChar);
                parameters[3].Value = Comments;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_UpdateSysIssueDetail", parameters);
                return Convert.ToInt32(parameters[2].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetIssueListWithCompleteStatus(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetComplitStatusIssueList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubordinateIssueListWithPenddingStatus(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetPendingSubordinateStatusIssueList", parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateSysIssueDetails(int IssueId,string User_Id)
        {
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@IssueId", SqlDbType.Int);
            parameters[0].Value = IssueId;

            parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[1].Value = 0;
            parameters[1].Direction = ParameterDirection.InputOutput;

            parameters[2] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            parameters[2].Value = User_Id;
            try
            {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysUpdateIssueDetail", parameters);
                if (parameters[1].Value != null)
                    return Convert.ToInt32(parameters[1].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }
        #endregion Sys_Issue_Details


        #region Feed Mapping
        #region RoleFeedMapping


        public DataSet GetFeedListByRole(string strRoleID)
        {
            DataSet dsRoleFeed = new DataSet();
            try
            {
                SqlParameter paramter = new SqlParameter("@RoleId", SqlDbType.NVarChar);
                paramter.Value = strRoleID;
                dsRoleFeed = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetRoleFeedList", paramter);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsRoleFeed;
        }

        public int SaveRoleFeedMapping(DataSet dsRoleFeed, string RoleID)
        {
            int iErrorCode = 0;
            SqlTransaction sqltransaction;
            SqlConnection sqlconn = new SqlConnection(Common.SqlConnectionString);
            sqlconn.Open();
            sqltransaction = sqlconn.BeginTransaction();
            try
            {

                if (dsRoleFeed.Tables[0].Rows.Count > 0)
                {

                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@RoleId", SqlDbType.NVarChar);
                    param[0].Value = dsRoleFeed.Tables["tbl_SysRoleFeed"].Rows[0]["RoleId"].ToString();
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysDeleteRoleFeed", param);

                }
                else
                {
                    SqlParameter paramter = new SqlParameter("@RoleId", SqlDbType.NVarChar);
                    paramter.Value = RoleID;
                    dsRoleFeed = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeleteFeedBYRoleId", paramter);
                }
                foreach (DataRow dr in dsRoleFeed.Tables["tbl_SysRoleFeed"].Rows)
                {
                    iErrorCode = 0;
                    SqlParameter[] SysRoleFeedParam = new SqlParameter[3];

                    SysRoleFeedParam[0] = new SqlParameter("@RoleId", SqlDbType.NVarChar);
                    SysRoleFeedParam[0].Value = dr["RoleId"].ToString();
                    SysRoleFeedParam[1] = new SqlParameter("@FeedId", SqlDbType.VarChar);
                    SysRoleFeedParam[1].Value = dr["FeedId"].ToString();
                    SysRoleFeedParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    SysRoleFeedParam[2].Value = 0;
                    SysRoleFeedParam[2].Direction = ParameterDirection.InputOutput;

                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysSaveRoleFeedMapping", SysRoleFeedParam);
                    iErrorCode = Convert.ToInt32(SysRoleFeedParam[2].Value);
                    if (iErrorCode != 6000001)
                    {
                        sqltransaction.Rollback();
                        sqlconn.Close();
                        return Convert.ToInt32(SysRoleFeedParam[2].Value.ToString());
                    }
                }

                sqltransaction.Commit();


            }
            catch (Exception ex)
            {
                sqltransaction.Rollback();
                Common.LogException(ex);
            }

            return iErrorCode;
        }
        #endregion RoleFeedMapping

        #endregion Feed Mapping

        #region Address Country State City Work


        public DataSet GetCountryStateCityDetails(int CountryId, int StateId)
        {
            DataSet dsObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@CountryId", SqlDbType.Int);
                objParam[0].Value = CountryId;

                objParam[1] = new SqlParameter("@StateId", SqlDbType.Int);
                objParam[1].Value = StateId;

                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetCountryStateCityDetail", objParam);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsObj;
        }
        public int SaveAreaCityMapping(DataSet Ds, int AreaId, string User_Id)
        {
            int iErrorCode = 0;
            SqlTransaction sqltransaction;
            SqlConnection sqlconn = new SqlConnection(Common.SqlConnectionString);
            sqlconn.Open();
            sqltransaction = sqlconn.BeginTransaction();
            try
            {
                if (Ds.Tables[0].Rows.Count > 0)
                {
                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@AreaId", SqlDbType.Int);
                    param[0].Value = Ds.Tables["DW_AddAreaCityMap"].Rows[0]["AreaId"].ToString();
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_DwDeleteAreaByArea&City", param);
                }
                else
                {
                    SqlParameter[] paramter = new SqlParameter[1];

                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@AreaId", SqlDbType.Int);
                    param[0].Value = AreaId;
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_DwDeleteAreaByArea&City", param);
                }

                foreach (DataRow dr in Ds.Tables["DW_AddAreaCityMap"].Rows)
                {
                    iErrorCode = 0;
                    SqlParameter[] SysRoleRptParam = new SqlParameter[4];

                    SysRoleRptParam[0] = new SqlParameter("@AreaId", SqlDbType.Int);
                    SysRoleRptParam[0].Value = dr["AreaId"].ToString();
                    SysRoleRptParam[1] = new SqlParameter("@CityId", SqlDbType.Int);
                    SysRoleRptParam[1].Value = dr["CityId"].ToString();
                    SysRoleRptParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    SysRoleRptParam[2].Value = 0;
                    SysRoleRptParam[2].Direction = ParameterDirection.InputOutput;
                    SysRoleRptParam[3] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                    SysRoleRptParam[3].Value = User_Id;
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysSaveAreaCityMapping", SysRoleRptParam);
                    iErrorCode = Convert.ToInt32(SysRoleRptParam[2].Value);
                    if (iErrorCode != 500002)
                    {
                        sqltransaction.Rollback();
                        sqlconn.Close();
                        return Convert.ToInt32(SysRoleRptParam[2].Value.ToString());
                    }
                }


                sqltransaction.Commit();

            }
            catch (Exception ex)
            {
                sqltransaction.Rollback();
                Common.LogException(ex);
            }

            return iErrorCode;
        }
        public DataSet GetAreaCityMappingList(int AreaId)
        {
            DataSet dsObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@AreaId", SqlDbType.Int);
                objParam[0].Value = AreaId;
                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetCityAreamappingListdetail", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsObj;
        }
        public int DeleteCountryStateCityDetail(int CountryId, string Addvalue)
        {
            int errcode = 0;
            try
            {

                SqlParameter[] objparam = new SqlParameter[4];


                objparam[0] = new SqlParameter("@CountryId", SqlDbType.Int);
                objparam[0].Value = CountryId;

                objparam[1] = new SqlParameter("@Addvalue", SqlDbType.VarChar);
                objparam[1].Value = Addvalue;

                objparam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objparam[2].Value = 0;
                objparam[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDeleteCountryStateCityDetail", objparam);

                if (objparam[2].Value != null)
                    errcode = Convert.ToInt32(objparam[2].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;
        }
        public DataSet GetCountryStateCityList()
        {
            DataSet dsObj = new DataSet();
            try
            {

                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetAllCountryStateCityList");

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsObj;
        }
        public int SaveCountryStateCityDetails(string Country, int Countryid, string City, string state, int Stateid, string Addvalue)
        {
            int Errcode = 0;
            try
            {
                SqlParameter[] Parameter = new SqlParameter[7];
                Parameter[0] = new SqlParameter("@Country", SqlDbType.VarChar);
                Parameter[0].Value = Country;

                Parameter[1] = new SqlParameter("@Countryid", SqlDbType.Int);
                Parameter[1].Value = Countryid;

                Parameter[2] = new SqlParameter("@City", SqlDbType.VarChar);
                Parameter[2].Value = City;

                Parameter[3] = new SqlParameter("@Stateid", SqlDbType.Int);
                Parameter[3].Value = Stateid;

                Parameter[4] = new SqlParameter("@state", SqlDbType.VarChar);
                Parameter[4].Value = state;

                Parameter[5] = new SqlParameter("@Addvalue", SqlDbType.VarChar);
                Parameter[5].Value = Addvalue;

                Parameter[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[6].Value = 0;
                Parameter[6].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveCountryStateCityDetails", Parameter);
                if (Parameter[6].Value != null)
                    Errcode = Convert.ToInt32(Parameter[6].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Errcode;
        }
        public int SaveRegionStatemap(DataSet dsRoleFeed, int Region, string User_Id)
        {
            int iErrorCode = 0;
            SqlTransaction sqltransaction;
            SqlConnection sqlconn = new SqlConnection(Common.SqlConnectionString);
            sqlconn.Open();
            sqltransaction = sqlconn.BeginTransaction();
            try
            {

                if (dsRoleFeed.Tables[0].Rows.Count > 0)
                {

                    SqlParameter[] param = new SqlParameter[1];
                    param[0] = new SqlParameter("@RegionId", SqlDbType.NVarChar);
                    param[0].Value = dsRoleFeed.Tables["DW_AddRegionStateMap"].Rows[0]["RegionId"].ToString();
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysDeleteRegionByRegionId", param);

                }
                else
                {
                    SqlParameter paramter = new SqlParameter("@RegionId", SqlDbType.NVarChar);
                    paramter.Value = Region;
                    dsRoleFeed = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeleteRegionByRegionId", paramter);
                }
                foreach (DataRow dr in dsRoleFeed.Tables["DW_AddRegionStateMap"].Rows)
                {
                    iErrorCode = 0;
                    SqlParameter[] SysRoleFeedParam = new SqlParameter[4];

                    SysRoleFeedParam[0] = new SqlParameter("@RegionId", SqlDbType.Int);
                    SysRoleFeedParam[0].Value = dr["RegionId"].ToString();
                    SysRoleFeedParam[1] = new SqlParameter("@StateId", SqlDbType.Int);
                    SysRoleFeedParam[1].Value = dr["StateId"].ToString();
                    SysRoleFeedParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    SysRoleFeedParam[2].Value = 0;
                    SysRoleFeedParam[2].Direction = ParameterDirection.InputOutput;
                    SysRoleFeedParam[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                    SysRoleFeedParam[3].Value = User_Id;
                    SqlHelper.ExecuteNonQuery(sqltransaction, CommandType.StoredProcedure, "sp_SysSaveregionStateMap", SysRoleFeedParam);
                    iErrorCode = Convert.ToInt32(SysRoleFeedParam[2].Value);
                    if (iErrorCode != 6000001)
                    {
                        sqltransaction.Rollback();
                        sqlconn.Close();
                        return Convert.ToInt32(SysRoleFeedParam[2].Value.ToString());
                    }
                }

                sqltransaction.Commit();


            }
            catch (Exception ex)
            {
                sqltransaction.Rollback();
                Common.LogException(ex);
            }

            return iErrorCode;
        }
        public DataSet GetStatewiseList(int Region)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] Parameter = new SqlParameter[8];
                Parameter[0] = new SqlParameter("@RegionId", SqlDbType.Int);
                Parameter[0].Value = Region;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetStateWiseList", Parameter);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int SaveRegionZoneAreaDetails(string Region, int Regionid, string Area, string Zone, int zoneid, string Addvalue, string User_Id)
        {
            int Errcode = 0;
            try
            {
                SqlParameter[] Parameter = new SqlParameter[8];
                Parameter[0] = new SqlParameter("@Region", SqlDbType.VarChar);
                Parameter[0].Value = Region;

                Parameter[1] = new SqlParameter("@Regionid", SqlDbType.Int);
                Parameter[1].Value = Regionid;

                Parameter[2] = new SqlParameter("@Area", SqlDbType.VarChar);
                Parameter[2].Value = Area;

                Parameter[3] = new SqlParameter("@Zoneid", SqlDbType.Int);
                Parameter[3].Value = zoneid;

                Parameter[4] = new SqlParameter("@Zone", SqlDbType.VarChar);
                Parameter[4].Value = Zone;

                Parameter[5] = new SqlParameter("@Addvalue", SqlDbType.VarChar);
                Parameter[5].Value = Addvalue;
                Parameter[6] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                Parameter[6].Value = User_Id;

                Parameter[7] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                Parameter[7].Value = 0;
                Parameter[7].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveRegionAreaZoneDetails", Parameter);
                if (Parameter[7].Value != null)
                    Errcode = Convert.ToInt32(Parameter[7].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return Errcode;
        }

        public DataSet GetRegionAreaZoneDetails(int RegionId, int ZoneId)
        {
            DataSet dsObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@RegionId", SqlDbType.Int);
                objParam[0].Value = RegionId;

                objParam[1] = new SqlParameter("@ZoneId", SqlDbType.Int);
                objParam[1].Value = ZoneId;

                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetRegionZoneAreaDetail", objParam);

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsObj;
        }
        public DataSet GetZoneStateList(int ZoneId)
        {
            DataSet dsObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ZoneId", SqlDbType.Int);
                objParam[0].Value = ZoneId;
                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetZoneStateList", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsObj;
        }

        public int SaveAddZoneStateMap(DataSet ds, string User_Id, int ZoneId)
        {
            SqlTransaction sqlTransaction;
            SqlConnection sqlConn = new SqlConnection(Common.SqlConnectionString);
            sqlConn.Open();
            sqlTransaction = sqlConn.BeginTransaction();
            int ErrCode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@ZoneId", SqlDbType.Int);
                objParam[0].Value = ZoneId;
                SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_DeleteMappedZonebyZoneId", objParam);


                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    SqlParameter[] Parameter = new SqlParameter[4];
                    Parameter[0] = new SqlParameter("@ZoneId", SqlDbType.Int);
                    Parameter[0].Value = dr["ZoneId"];

                    Parameter[1] = new SqlParameter("@StateId", SqlDbType.Int);
                    Parameter[1].Value = dr["StateId"];

                    Parameter[2] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                    Parameter[2].Value = User_Id;

                    Parameter[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    Parameter[3].Value = 0;
                    Parameter[3].Direction = ParameterDirection.InputOutput;
                    SqlHelper.ExecuteDataset(sqlTransaction, CommandType.StoredProcedure, "sp_SaveAddZoneStateMapDetail", Parameter);
                    if (Parameter[3].Value != null)
                        ErrCode = Convert.ToInt32(Parameter[3].Value.ToString());
                    else
                        return 0;
                    if (ErrCode != 500002)
                    {
                        sqlTransaction.Rollback();
                        sqlConn.Close();
                    }
                }
                sqlTransaction.Commit();
                sqlConn.Close();
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                sqlTransaction.Rollback();
                sqlConn.Close();
            }
            return ErrCode;
        }
        public DataSet GetRegionZoneAreaList()
        {
            DataSet dsObj = new DataSet();
            try
            {

                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetAllRegionAreaZoneList");

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsObj;
        }
        #endregion Address Country State City Work 

        public int SaveUserLogin(string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                parameters[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[1].Value = 0;
                parameters[1].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveUserLog", parameters);

                errCode = Convert.ToInt32(parameters[1].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }

        #region CompLibrary

        public DataSet GetCompLibraryList(string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id",SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetLibraryList", ObjParam);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetDropDownCompCodeList()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWCompanyList");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataSet GetselectedLibrary(int DocId)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@DocId", SqlDbType.Int);
                parameters[0].Value = DocId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedLibraryDetail", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public int SaveCompLibraryDetail(DataSet ds, string User_Id, string Attachment)
        {
            SqlParameter[] parameters = new SqlParameter[8];
            parameters[0] = new SqlParameter("@DocId", SqlDbType.Int);
            parameters[0].Value = ds.Tables[0].Rows[0]["DocId"].ToString();
            parameters[0].Direction = ParameterDirection.InputOutput;

            parameters[1] = new SqlParameter("@CompCode", SqlDbType.VarChar);
            parameters[1].Value = ds.Tables[0].Rows[0]["CompCode"].ToString();

            parameters[2] = new SqlParameter("@DocName", SqlDbType.VarChar);
            parameters[2].Value = ds.Tables[0].Rows[0]["DocName"].ToString();

            parameters[3] = new SqlParameter("@Description", SqlDbType.VarChar);
            parameters[3].Value = ds.Tables[0].Rows[0]["Description"].ToString();

            parameters[4] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            parameters[4].Value = User_Id;

            parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[5].Value = 0;
            parameters[5].Direction = ParameterDirection.InputOutput;
            if (Attachment == null)
            {
                parameters[6] = new SqlParameter("@Attachment", SqlDbType.VarChar);
                parameters[6].Value = 0;
            }
            else
            {
                parameters[6] = new SqlParameter("@Attachment", SqlDbType.VarChar);
                parameters[6].Value = Attachment;
            }

            try
            {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveLibraryDeatail", parameters);
                if (parameters[0].Value != null)
                    return Convert.ToInt32(parameters[0].Value.ToString());

                else
                    return 0;

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }

        #endregion CompLibrary

        #region ApprovalTask List
        public DataSet PenddingTaskList(string User_Id)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetTaskManagementList", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public DataSet CollectionList()
        {
            DataSet dsobj = new DataSet();
            try
            {
                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWPurchaseSaleDashboard");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public DataSet ProjectTaskOverdue(string User_Id)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWprojectTaskOverdue", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public DataSet PendingCustomerKYC()
        {
            DataSet dsobj = new DataSet();
            try
            {
                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_PendingCustomerKYC");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        public DataSet GetAllRiskCaseDetailInpercent(string User_Id)
        {
            DataSet dsobj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;

                dsobj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetAllCaseRiskDetail", parameters);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsobj;
        }
        #endregion ApprovalTask List

        #region Employee Assets
        public DataSet AssetListByEmployee(int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysAssetByEmployee", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedAssetListByEmployee(int EmpId, int AssetId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;
                objParam[1] = new SqlParameter("@AssetId", SqlDbType.Int);
                objParam[1].Value = AssetId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedAssetByEmployee", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return ds;
        }
        public DataSet GetSelectedEmployeeAsset(int AssetId)
        {
            DataSet dsExprt = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];
                objParam[0] = new SqlParameter("@AssetId", SqlDbType.Int);
                objParam[0].Value = AssetId;


                dsExprt = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedEmployeeAssetType", objParam);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsExprt;

        }


        public int SaveEmpAsset(DataSet dsEmp, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];

                parameters[0] = new SqlParameter("@AssetId", SqlDbType.Int);
                parameters[0].Value = dsEmp.Tables[0].Rows[0]["AssetId"].ToString();

                parameters[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[1].Value = dsEmp.Tables[0].Rows[0]["EmpId"].ToString();

                parameters[2] = new SqlParameter("@AllocationDate", SqlDbType.SmallDateTime);
                parameters[2].Value = Convert.ToDateTime(dsEmp.Tables[0].Rows[0]["AllocationDate"].ToString());

                //parameters[3] = new SqlParameter("@DeAllocationDate", SqlDbType.SmallDateTime);
                //parameters[3].Value = Convert.ToDateTime(dsEmp.Tables[0].Rows[0]["DeAllocationDate"].ToString());

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                parameters[4] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;
                //parameters[5] = new SqlParameter("@CreatedBy", SqlDbType.NVarChar);
                //parameters[5].Value = dsEmp.Tables[0].Rows[0]["CreatedBy"].ToString();
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveEmpAssetDetails", parameters);

                errCode = Convert.ToInt32(parameters[3].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                string[] words = ex.Message.ToString().Split('.');
                string Chk = words[0];
                if (Chk == "Violation of PRIMARY KEY constraint 'PK_tbl_SysEmployeeAssets'")
                {
                    errCode = 600001;
                }
                return errCode; ;
            }
            return errCode;
        }



        public int DeallocateAsset(int AssetId, int EmpId)
        {
            int errorcode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];

                objParam[0] = new SqlParameter("@AssetId", SqlDbType.Int);
                objParam[0].Value = AssetId;
                objParam[0].Direction = ParameterDirection.InputOutput;
                objParam[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[1].Value = EmpId;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDeallocateEmployeeAsset", objParam);

                errorcode = Convert.ToInt32(objParam[0].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errorcode;

        }


        #endregion Employee Assets

        #region Segement
        public DataSet GetItemSegmentList(string UserId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id",SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;

                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWItemSegmentList", ObjParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }

        public DataSet GetSelectedItemSegment(int SegmentId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@SegmentId", SqlDbType.Int);
                objParam[0].Value = SegmentId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedItemSegment", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }

        public DataSet EquipmentListForSegment(int SegmentId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@SegmentId", SqlDbType.Int);
                objParam[0].Value = SegmentId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWEquipmentListForSegment", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public DataSet GetdropdownForsegment(int SegmentId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@SegmentId", SqlDbType.Int);
                objParam[0].Value = SegmentId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetDropdownForDWItemsegment", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public int SaveEquipmentForSegment(int SegmentId, int EquipmentId,string User_Id)
        {

            try
            {
                SqlParameter[] objParam = new SqlParameter[4];
                objParam[0] = new SqlParameter("@EquipmentId", SqlDbType.Int);
                objParam[0].Value = EquipmentId;

                objParam[1] = new SqlParameter("@SegmentId", SqlDbType.Int);
                objParam[1].Value = SegmentId;

                objParam[2] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[2].Value = 0;
                objParam[2].Direction = ParameterDirection.InputOutput;

                objParam[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[3].Value = User_Id;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SaveEquipmentforsegment", objParam);
                if (objParam[2].Value != null)
                    return Convert.ToInt32(objParam[2].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }
        public int SaveItemSegmentDetails(DataSet ds, string User_Id)
        {

            try
            {
                SqlParameter[] objParam = new SqlParameter[6];
                objParam[0] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;

                objParam[1] = new SqlParameter("@SegmentId", SqlDbType.Int);
                objParam[1].Value = ds.Tables[0].Rows[0]["SegmentId"].ToString();

                objParam[2] = new SqlParameter("@ParentSegmentId ", SqlDbType.Int);
                objParam[2].Value = ds.Tables[0].Rows[0]["ParentSegmentId"].ToString();

                objParam[3] = new SqlParameter("@SegmentName", SqlDbType.VarChar);
                objParam[3].Value = ds.Tables[0].Rows[0]["SegmentName"].ToString();

                objParam[4] = new SqlParameter("@Description", SqlDbType.VarChar);
                objParam[4].Value = ds.Tables[0].Rows[0]["Description"].ToString();

                objParam[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[5].Value = 0;
                objParam[5].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveItemSegment", objParam);
                if (objParam[5].Value != null)
                    return Convert.ToInt32(objParam[5].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }

        #endregion Segment

        #region Itemcategory
        public DataSet GetItemcategoryList(string UserId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;

                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWItemCategorytList", ObjParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }

        public DataSet GetSelectedItemCategory(int CategoryId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CategoryId", SqlDbType.Int);
                objParam[0].Value = CategoryId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWGetSelectedItemCategory", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }

        public DataSet GetDropdownForcategory(int CategoryId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@CategoryId", SqlDbType.Int);
                objParam[0].Value = CategoryId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetDropdownForDWItemCategory", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public int SaveItemCategoryDetails(DataSet ds, string User_Id)
        {

            try
            {
                SqlParameter[] objParam = new SqlParameter[6];
                objParam[0] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;

                objParam[1] = new SqlParameter("@CategoryId", SqlDbType.Int);
                objParam[1].Value = ds.Tables[0].Rows[0]["CategoryId"].ToString();

                objParam[2] = new SqlParameter("@ParentCategoryId ", SqlDbType.Int);
                objParam[2].Value = ds.Tables[0].Rows[0]["ParentCategoryId"].ToString();

                objParam[3] = new SqlParameter("@Category", SqlDbType.VarChar);
                objParam[3].Value = ds.Tables[0].Rows[0]["Category"].ToString();

                objParam[4] = new SqlParameter("@Description", SqlDbType.VarChar);
                objParam[4].Value = ds.Tables[0].Rows[0]["Description"].ToString();

                objParam[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[5].Value = 0;
                objParam[5].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWSaveItemCategory", objParam);
                if (objParam[5].Value != null)
                    return Convert.ToInt32(objParam[5].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }
        #endregion ItemCategory

        #region System Mail Config

        public DataSet GetSystemEmailConfigList()
        {
            DataSet DSObj = new DataSet();
            try
            {
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetMailConfigList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }

        public DataSet GetSelectedSystemEmailConfigList(int MailTemplateId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@MailTemplateId", SqlDbType.Int);
                objParam[0].Value = MailTemplateId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedMailConfigList", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public int SaveSystemEmailConfigDetails(DataSet ds, string User_Id)
        {

            try
            {
                SqlParameter[] objParam = new SqlParameter[6];
                objParam[0] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;

                objParam[1] = new SqlParameter("@MailTemplateId", SqlDbType.Int);
                objParam[1].Value = ds.Tables[0].Rows[0]["MailTemplateId"].ToString();

                objParam[2] = new SqlParameter("@MailContent", SqlDbType.NVarChar);
                objParam[2].Value = ds.Tables[0].Rows[0]["MailContent"].ToString();

                objParam[3] = new SqlParameter("@TemplateName", SqlDbType.VarChar);
                objParam[3].Value = ds.Tables[0].Rows[0]["TemplateName"].ToString();

                objParam[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[4].Value = 0;
                objParam[4].Direction = ParameterDirection.InputOutput;

                objParam[5] = new SqlParameter("@DocumentType", SqlDbType.VarChar);
                objParam[5].Value = ds.Tables[0].Rows[0]["DocumentType"].ToString();
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveMailConfigDetail", objParam);
                if (objParam[4].Value != null)
                    return Convert.ToInt32(objParam[4].Value);
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return 0;
        }
        public DataSet GetDocumentTypeforDropDwnList(string MailTemplateId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@MailTemplateId", SqlDbType.VarChar);
                objParam[0].Value = MailTemplateId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysDocumentTypeListforDrpDwn", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        #endregion System Mail Config

        #region EmployeeDocuments
        public int SaveEmployeeDocuments(string DocumentPath, string DocumentName, int EmpId)
        {
            SqlParameter[] parameters = new SqlParameter[4];
            parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
            parameters[0].Value = EmpId;
            parameters[0].Direction = ParameterDirection.InputOutput;

            parameters[1] = new SqlParameter("@DocumentName", SqlDbType.VarChar);
            parameters[1].Value = DocumentName;

            parameters[2] = new SqlParameter("@DocumentPath", SqlDbType.VarChar);
            parameters[2].Value = DocumentPath;

            try
            {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveEmployeeDocuments", parameters);
                if (parameters[0].Value != null)
                    return Convert.ToInt32(parameters[0].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }
        public DataSet GetDocumentsByEmployee(int EmpId)
        {
            DataSet DS = new DataSet();
            try
            {
                SqlParameter paramter = new SqlParameter("@EmpId", SqlDbType.Int);
                paramter.Value = EmpId;
                DS = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysgetEmployeeDocuments", paramter);
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return DS;
        }
        #endregion EmployeeDocuments 


        #region Exchange Rate
        public int CurrencyExchange(DataTable dt)

        {
            int errcode = 0;
            try
            {
                foreach (DataRow dr in dt.Rows)
                {

                    SqlParameter[] parameters = new SqlParameter[4];
                    parameters[0] = new SqlParameter("@CurrencyCode", SqlDbType.VarChar);
                    parameters[0].Value = dr["currencycode"].ToString();

                    parameters[1] = new SqlParameter("@ExchangeRate", SqlDbType.Float);
                    parameters[1].Value = Convert.ToDouble(dr["Rate"].ToString());

                    parameters[2] = new SqlParameter("@Source", SqlDbType.NVarChar);
                    parameters[2].Value = dr["Source"].ToString();

                    parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[3].Value = 0;
                    parameters[3].Direction = ParameterDirection.InputOutput;

                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveCurrencyExchangeRate", parameters);
                    errcode = Convert.ToInt32(parameters[3].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;

        }

        public List<string> CurrencyList()
        {
            List<string> list = new List<string>();
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedCurrencyCodes");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        string item = "";
                        item = dr["CurrencyCode"].ToString();
                        list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return list;
        }

        #endregion Exchange Rate

        #region IssueDetails
        public DataSet GetIssueDetailList(string User_Id)
        {
            DataSet DSObj = new DataSet();
            try
            {

                SqlParameter[] objParam = new SqlParameter[1];
                objParam[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                objParam[0].Value = User_Id;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetAllIssueDetail", objParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public DataSet GetTop10ProjectByTam()
        {
            DataSet DSObj = new DataSet();
            try
            {
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetTop10ProjectByTAM");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        #endregion IssueDetails

        #region StockLocation
        public DataSet GetStockLocationList()
        {
            DataSet dsData = new DataSet();
            dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_dwStockLocationList");
            return dsData;
        }
        public DataSet GetSelectedStockLocation(int STLocationId)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@STLocationId", SqlDbType.Int);
            parameters[0].Value = STLocationId;
            dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwGetSelectedStockLocation", parameters);
            return dsData;
        }
        public int SaveStockLocation(DataSet ds, string User_Id)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];

                parameters[0] = new SqlParameter("@STLocationId", SqlDbType.Int);
                parameters[0].Value = ds.Tables[0].Rows[0]["STLocationId"].ToString();

                parameters[1] = new SqlParameter("@Name", SqlDbType.VarChar);
                parameters[1].Value = ds.Tables[0].Rows[0]["Name"].ToString();

                parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["Description"].ToString();

                parameters[3] = new SqlParameter("@ParentLocationId", SqlDbType.Int);
                parameters[3].Value = ds.Tables[0].Rows[0]["ParentLocationId"].ToString();

                parameters[4] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                parameters[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[5].Value = 0;
                parameters[5].Direction = ParameterDirection.InputOutput;
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwSaveStockLocation", parameters);

                errCode = Convert.ToInt32(parameters[5].Value.ToString());
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet GetParentStockLocationLst(int STLocationId)
        {
            DataSet dsData = new DataSet();
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@STLocationId", SqlDbType.Int);
            parameters[0].Value = STLocationId;
            dsData = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DwGetParentStockLocationList", parameters);
            return dsData;
        }
        #endregion StockLocation
        public DataSet GetTop10QuotationByAmount()
        {
            DataSet DSObj = new DataSet();
            try
            {
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VWGetTop10QuotationByRate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }

        #region CurrencyModel
        public DataSet CurrencyExchangeRate(string User_Id)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_ID",SqlDbType.NVarChar);
                ObjParam[0].Value = User_Id;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysCurrencyExchangeRateList", ObjParam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public DataSet GetSelectedCurrency(string CurrencyCode)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[4];
                parameters[0] = new SqlParameter("@CurrencyCode", SqlDbType.VarChar);
                parameters[0].Value = CurrencyCode;

                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedCurrExchRate", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }

        public int SaveCurrency(DataSet ds,string User_Id)
        {
            int errcode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@CurrencyCode", SqlDbType.VarChar);
                parameters[0].Value = ds.Tables[0].Rows[0]["CurrencyCode"];

                parameters[1] = new SqlParameter("@ExchangeRate", SqlDbType.Float);
                parameters[1].Value = ds.Tables[0].Rows[0]["ExchangeRate"];

                parameters[2] = new SqlParameter("@Source", SqlDbType.NVarChar);
                parameters[2].Value = ds.Tables[0].Rows[0]["Source"];

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                parameters[4] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSaveCurrencyExchangeRate", parameters);
                errcode = Convert.ToInt32(parameters[3].Value.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errcode;

        }
        #endregion CurrencyModel

        #region EmployeeSalaryStructure

        public DataSet GetEmpListForSalaryStructure(string User_Id)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysEmpListForSalaryStructure", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        public DataSet GetSelectedEmpSalaryStructureDetails(int EmpId)
        {
            DataSet DSObj = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = EmpId;
                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedEmpSalStructure", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }

        public int UpdateEmployeeDetails(double FixedRate, int EmpId, double VariableRate,string User_Id)
        {
            DataSet ds = new DataSet();
            int ErrorCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[6];
                parameters[0] = new SqlParameter("@AnnualFixPay", SqlDbType.Float);
                parameters[0].Value = FixedRate;

                parameters[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[1].Value = EmpId;

                parameters[2] = new SqlParameter("@AnnualVariablePay ", SqlDbType.Float);
                parameters[2].Value = VariableRate;

                parameters[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[3].Value = 0;
                parameters[3].Direction = ParameterDirection.InputOutput;

                parameters[4] = new SqlParameter("@User_ID", SqlDbType.NVarChar);
                parameters[4].Value = User_Id;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_sysupdateEmployeePayRate", parameters);

                ErrorCode = Convert.ToInt32(parameters[3].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ErrorCode;
        }
        public DataSet GetComponentDetail(int ComponentDetail)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@ComponentId", SqlDbType.Int);
                parameters[0].Value = ComponentDetail;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetComponentDetail", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public DataSet GetComponentListforSalaryStructure()
        {
            DataSet DSObj = new DataSet();
            try
            {

                DSObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DSObj;
        }
        #endregion EmployeeSalaryStructure

        #region Public Holiday
        public DataSet GetPublicHolidayList()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetPublicHolidayList");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetselectedPublicHolidayList(int HolidayId)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@HolidayId", SqlDbType.Int);
                parameters[0].Value = HolidayId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetSelectedPublicHolidayList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public int SavePublicHolidayDetail(DataSet ds, string User_Id)
        {
            SqlParameter[] parameters = new SqlParameter[7];
            parameters[0] = new SqlParameter("@HolidayId", SqlDbType.Int);
            parameters[0].Value = ds.Tables[0].Rows[0]["HolidayId"].ToString();        

            parameters[1] = new SqlParameter("@Finyear", SqlDbType.VarChar);
            parameters[1].Value = ds.Tables[0].Rows[0]["Finyear"].ToString();

            parameters[2] = new SqlParameter("@HolidayDate", SqlDbType.SmallDateTime);
            parameters[2].Value = ds.Tables[0].Rows[0]["HolidayDate"].ToString();

            parameters[3] = new SqlParameter("@RegionId", SqlDbType.Int);
            parameters[3].Value = ds.Tables[0].Rows[0]["RegionId"].ToString();

            parameters[4] = new SqlParameter("@Title", SqlDbType.VarChar);
            parameters[4].Value = ds.Tables[0].Rows[0]["Title"].ToString();

            parameters[5] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            parameters[5].Value = User_Id;

            parameters[6] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[6].Value = 0;
            parameters[6].Direction = ParameterDirection.InputOutput;
            try
            {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysSavePublicHoliday", parameters);
                if (parameters[0].Value != null)
                    return Convert.ToInt32(parameters[0].Value.ToString());
                else
                    return 0;

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                return 0;
            }
        }

        #endregion Public Holidays

        #region Weekly Sal Component
        public DataSet GetWeeklySalryList(string User_Id)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
                parameters[0].Value = User_Id;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysGetWeeklyPaymentList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        #endregion Weekly Sal Component
    }
}
