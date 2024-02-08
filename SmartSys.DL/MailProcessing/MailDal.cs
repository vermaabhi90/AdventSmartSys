using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.MailProcessing
{
    public class MailDal
    {

        public DataSet GetEmailIDByEmployee(int EmpId)
        {
            DataSet EmaiIDs = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter("@EmpId", SqlDbType.NVarChar);
                parameters[0].Value = EmpId;

                EmaiIDs = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "SP_GetEmailIDByEmployee", parameters);
           
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            return EmaiIDs;
        }

        public int SaveMailDetail(DataTable dt)
        {
            int errCode = 0;

            //  SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWDeleteMails" );
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SqlParameter[] parameters = new SqlParameter[13];
                    parameters[0] = new SqlParameter("@Subject", SqlDbType.VarChar);
                    parameters[0].Value = dr["Subject"];

                    parameters[1] = new SqlParameter("@From ", SqlDbType.VarChar);
                    parameters[1].Value = dr["From"];

                    parameters[2] = new SqlParameter("@To", SqlDbType.VarChar);
                    parameters[2].Value = dr["To"];

                    parameters[3] = new SqlParameter("@Date", SqlDbType.SmallDateTime);
                    parameters[3].Value = Convert.ToDateTime(dr["Date"]);

                    parameters[4] = new SqlParameter("@ClientMailId", SqlDbType.VarChar);
                    parameters[4].Value = dr["MailId"];

                    parameters[5] = new SqlParameter("@EmpId", SqlDbType.Int);
                    parameters[5].Value = dr["EmpId"];

                    parameters[6] = new SqlParameter("@Status", SqlDbType.Int);
                    parameters[6].Value = dr["Status"];

                    parameters[7] = new SqlParameter("@EnquiryId", SqlDbType.Int);
                    parameters[7].Value = dr["EnquiryId"];

                    parameters[8] = new SqlParameter("@Comments", SqlDbType.VarChar);
                    parameters[8].Value = dr["Comments"];

                    parameters[9] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                    parameters[9].Value = 0;
                    parameters[9].Direction = ParameterDirection.InputOutput;

                    parameters[10] = new SqlParameter("@BodyText", SqlDbType.NVarChar);
                    parameters[10].Value = dr["BodyText"];

                    parameters[11] = new SqlParameter("@CC", SqlDbType.VarChar);
                    parameters[11].Value = dr["CC"];

                    parameters[12] = new SqlParameter("@BCC", SqlDbType.VarChar);
                    parameters[12].Value = dr["BCC"];

                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_DWsaveMailDetail", parameters);

                    errCode = Convert.ToInt32(parameters[9].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return errCode;
        }
        public DataSet GetEmplyeeMailList()
        {
            DataSet dsObj = new DataSet();
            try
            {
                dsObj = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEmployeeMailDetail");
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return dsObj;
        }
    }
}
