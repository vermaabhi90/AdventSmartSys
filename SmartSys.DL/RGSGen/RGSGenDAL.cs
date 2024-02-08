using SmartSys.DAL;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.DL.RGSGen
{


   public class RGSGenDAL
    {

       public int SaveGenrator(DataSet dsRGSGen, string User_Id)
       {
           try
           {
           SqlParameter[] parameters = new SqlParameter[11];
           parameters[0] = new SqlParameter("@GenId", SqlDbType.SmallInt);
           parameters[0].Value = dsRGSGen.Tables[0].Rows[0]["GenId"];

           parameters[1] = new SqlParameter("@GenType", SqlDbType.VarChar);
           parameters[1].Value = dsRGSGen.Tables[0].Rows[0]["GenType"];

           parameters[2] = new SqlParameter("@Host", SqlDbType.VarChar);
           parameters[2].Value = dsRGSGen.Tables[0].Rows[0]["Host"].ToString();

           parameters[3] = new SqlParameter("@isActive", SqlDbType.Bit);
           parameters[3].Value = dsRGSGen.Tables[0].Rows[0]["isActive"].ToString();

           parameters[4] = new SqlParameter("@Port", SqlDbType.VarChar);
           parameters[4].Value = dsRGSGen.Tables[0].Rows[0]["Port"].ToString();

           parameters[5] = new SqlParameter("@PollingInterval", SqlDbType.Int);
           parameters[5].Value = dsRGSGen.Tables[0].Rows[0]["PollingInterval"].ToString();

           parameters[6] = new SqlParameter("@BusinessLineId", SqlDbType.SmallInt);
           parameters[6].Value = dsRGSGen.Tables[0].Rows[0]["BusinessLineId"].ToString();

           parameters[7] = new SqlParameter("@ProcessId", SqlDbType.Int);
           parameters[7].Value = 0;


           parameters[8] = new SqlParameter("@User_Id", SqlDbType.NVarChar);
           parameters[8].Value = User_Id;

           parameters[9] = new SqlParameter("@StatusId", SqlDbType.SmallInt);
           parameters[9].Value = 0;


           parameters[10] = new SqlParameter("@ErrorCode", SqlDbType.Int);
           parameters[10].Value = 0;
           parameters[10].Direction = ParameterDirection.InputOutput;
           
           
               SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSSaveGen", parameters);
               if (parameters[10].Value != null)
                   return Convert.ToInt32(parameters[10].Value.ToString());
               else
                   return 0;
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
               return 0;
           }
       }
       public DataSet RGSGenGetList()
       {
           DataSet dsRGSGen = new DataSet();
           try
           {
               dsRGSGen = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetGenList");              
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsRGSGen;
       }
       public DataSet GetBussList()
       {
           DataSet dsGen = new DataSet();
           try
           {
               dsGen = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetBusinesslineId");       
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsGen;
       }

       public DataSet GetSelectedGenList(int GenId)
       {
           DataSet dsRGSGen = new DataSet();

           SqlParameter[] parameters = new SqlParameter[1];
           parameters[0] = new SqlParameter("@GenId", SqlDbType.SmallInt);
           parameters[0].Value = GenId;
           try
           {
               dsRGSGen = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_RGSGetSelectedGenList", parameters);
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return dsRGSGen;
       }

    }


}
