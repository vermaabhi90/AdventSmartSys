using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using SmartSys.Utility;
using System.Data.SqlClient;
using SmartSys.DAL;
using MySql.Data.MySqlClient;



namespace SmartSys.DL.DashBoard
{
  public  class DashBoardDAL
    {
      #region[Monthly Sales Data]
      public DataSet GetMonthlySalesData(string strCompName, int iConnectionID)
      {
          DataSet dsSalesData = new DataSet();
          try
          {
              SysDBConnDAL sysDBConnDal = new SysDBConnDAL();
              DataSet dsConn = sysDBConnDal.GetSelectedDBConn(Convert.ToInt16(iConnectionID));
              StringBuilder strConn = new StringBuilder();
              strConn.Append(@"Data Source=" + dsConn.Tables[0].Rows[0]["ServerName"].ToString() + ";");
              strConn.Append(@"Initial Catalog=" + dsConn.Tables[0].Rows[0]["DBName"].ToString() + ";");
              strConn.Append(@"uid=" + dsConn.Tables[0].Rows[0]["UserName"].ToString() + ";");
              strConn.Append(@"pwd=" + dsConn.Tables[0].Rows[0]["Password"].ToString() + ";");
              //string strConn = @"Data Source=ETPL02\SMDB;Initial Catalog=DPK;uid=sa;pwd=sa123";
              SqlParameter[] parameters = new SqlParameter[1];
              parameters[0] = new SqlParameter("@CompanyCode", SqlDbType.VarChar);
              parameters[0].Value = strCompName;
              dsSalesData = SqlHelper.ExecuteDataset(strConn.ToString(), CommandType.StoredProcedure, "sp_SysDashBDTwelveMonthSales", parameters);
          }
          catch (Exception ex)
          {
              Common.LogException(ex);
          }
          return dsSalesData;
      }
      public DataSet GetYearlySalesData(string strCompName, int iConnectionID)
      {
          DataSet dsSalesData = new DataSet();
          try
          {
              SysDBConnDAL sysDBConnDal = new SysDBConnDAL();
              DataSet dsConn = sysDBConnDal.GetSelectedDBConn(Convert.ToInt16(iConnectionID));
              StringBuilder strConn = new StringBuilder();
              strConn.Append(@"Data Source=" + dsConn.Tables[0].Rows[0]["ServerName"].ToString() + ";");
              strConn.Append(@"Initial Catalog=" + dsConn.Tables[0].Rows[0]["DBName"].ToString() + ";");
              strConn.Append(@"uid=" + dsConn.Tables[0].Rows[0]["UserName"].ToString() + ";");
              strConn.Append(@"pwd=" + dsConn.Tables[0].Rows[0]["Password"].ToString() + ";");
              SqlParameter[] parameters = new SqlParameter[1];
              parameters[0] = new SqlParameter("@CompanyCode", SqlDbType.VarChar);
              parameters[0].Value = strCompName;
              dsSalesData = SqlHelper.ExecuteDataset(strConn.ToString(), CommandType.StoredProcedure, "sp_SysGetLastTwoYearsSales", parameters);
          }
          catch (Exception ex)
          {
              Common.LogException(ex);
          }
          return dsSalesData;
      }

      public DataSet Gettwoyearsquartersales(string strCompName, int iConnectionID)
      {
          DataSet dsSalesData = new DataSet();
          try
          {
              SysDBConnDAL sysDBConnDal = new SysDBConnDAL();
              DataSet dsConn = sysDBConnDal.GetSelectedDBConn(Convert.ToInt16(iConnectionID));
              StringBuilder strConn = new StringBuilder();
              strConn.Append(@"Data Source=" + dsConn.Tables[0].Rows[0]["ServerName"].ToString() + ";");
              strConn.Append(@"Initial Catalog=" + dsConn.Tables[0].Rows[0]["DBName"].ToString() + ";");
              strConn.Append(@"uid=" + dsConn.Tables[0].Rows[0]["UserName"].ToString() + ";");
              strConn.Append(@"pwd=" + dsConn.Tables[0].Rows[0]["Password"].ToString() + ";");
              SqlParameter[] parameters = new SqlParameter[1];
              parameters[0] = new SqlParameter("@CompanyCode", SqlDbType.VarChar);
              parameters[0].Value = strCompName;
              dsSalesData = SqlHelper.ExecuteDataset(strConn.ToString(), CommandType.StoredProcedure, "sp_SysGetTwoYearsQuarterSales", parameters);
          }
          catch (Exception ex)
          {
              Common.LogException(ex);
          }
          return dsSalesData;
      }
      public DataSet GetSevenDaysEnquiryData(int iConnectionID)
      {
          DataSet dsData = new DataSet();
          SysDBConnDAL sysDBConnDal = new SysDBConnDAL();
          DataSet dsConn = sysDBConnDal.GetSelectedDBConn(Convert.ToInt16(iConnectionID));
          string conStr = "SERVER=" + dsConn.Tables[0].Rows[0]["ServerName"].ToString() + ";" + "DATABASE=" + dsConn.Tables[0].Rows[0]["DBName"].ToString() + ";" + "UID=" + dsConn.Tables[0].Rows[0]["UserName"].ToString() + ";" + "PASSWORD=" + dsConn.Tables[0].Rows[0]["Password"].ToString() + ";";
          MySqlConnection connection = new MySqlConnection(conStr);
          connection.Open();
          try
          {

              MySqlCommand sqlComm = new MySqlCommand("sp_DBCGetSevenDaysEnqDetails", connection);

              sqlComm.CommandType = CommandType.StoredProcedure;
              MySqlDataAdapter da = new MySqlDataAdapter();
              da.SelectCommand = sqlComm;
              da.Fill(dsData);
              connection.Close();
          }
          catch (Exception ex)
          {
              connection.Close();
              Common.LogException(ex);
          }
          return dsData;
      }
      #endregion
    }
}
