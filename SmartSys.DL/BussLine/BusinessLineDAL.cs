using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.Business_Line
{
    public class BusinessLineDAL
   {
        public DataSet GetBussLineList(string UserId)
        {
            DataSet dsBussLine = new DataSet();
            try
            {
                SqlParameter[] ObjParam = new SqlParameter[2];
                ObjParam[0] = new SqlParameter("@User_Id",SqlDbType.NVarChar);
                ObjParam[0].Value = UserId;
                dsBussLine = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysBussLineGetList", ObjParam);               
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsBussLine;
        }

        public int SaveBussLine(DataSet dsBussLine, string User_Id)
        {

            SqlParameter[] parameters = new SqlParameter[5];
            parameters[0] = new SqlParameter("@BusinessLineId", SqlDbType.SmallInt);
            parameters[0].Value = dsBussLine.Tables[0].Rows[0]["BusinessLineId"];

            parameters[1] = new SqlParameter("@BusinessLineName", SqlDbType.VarChar);
            parameters[1].Value = dsBussLine.Tables[0].Rows[0]["BusinessLineName"].ToString();
         
            parameters[2] = new SqlParameter("@Description", SqlDbType.VarChar);
            parameters[2].Value = dsBussLine.Tables[0].Rows[0]["Description"].ToString();

            parameters[3] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
            parameters[3].Value = User_Id;

            parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
            parameters[4].Value = 0;
            parameters[4].Direction = ParameterDirection.InputOutput;
            try
            {
                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysBusinessLineSave", parameters);
               
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            if (parameters[4].Value != null)
                return Convert.ToInt32(parameters[4].Value.ToString());
            else
                return 0;
        }

        public DataSet BussLineGetSelected(int BusinessLineId)
        {
            DataSet dsBussLine = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@BusinessLineId", SqlDbType.SmallInt);
                parameters[0].Value = BusinessLineId;

                dsBussLine = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SysBussLineGetSelectedList", parameters);
                
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsBussLine;
        }

    }
}
