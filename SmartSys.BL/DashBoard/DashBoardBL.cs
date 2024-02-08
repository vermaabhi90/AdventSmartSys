using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartSys.DAL;
using System.Data;
using SmartSys.Utility;

namespace SmartSys.BL.DashBoard
{
   public class DashBoardBL
    {
        SmartSys.DL.DashBoard.DashBoardDAL objDashBoard = null;
        public List<MonthlySalesDataModel> GetMonthlySalesData(string strCompName, int iConnectionID)
        {
            List<MonthlySalesDataModel> lstSalesData = new List<MonthlySalesDataModel>();
            DataSet dsMenu = new DataSet();
            try
            {
                objDashBoard = new SmartSys.DL.DashBoard.DashBoardDAL();
                DataSet dsSalesData = objDashBoard.GetMonthlySalesData(strCompName, iConnectionID);
                foreach (DataRow dr in dsSalesData.Tables[0].Rows)
                {
                    MonthlySalesDataModel salesdata = new MonthlySalesDataModel();
                    salesdata.Month = dr["Month"].ToString();
                    salesdata.TotalSales = Convert.ToDecimal(dr["Total Sale"].ToString());
                    salesdata.CompanyCode = dr["CompCode"].ToString();
                    lstSalesData.Add(salesdata);
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstSalesData;
        }

        public List<TwoYearsQuarterSalesModel> TwoYearsQuarterSales(string strCompName, int iConnectionID)
        {


            List<TwoYearsQuarterSalesModel> lstModel = new List<TwoYearsQuarterSalesModel>();
            DataSet dsMenu = new DataSet();
            try
            {


                objDashBoard = new SmartSys.DL.DashBoard.DashBoardDAL();
                DataSet dsSalesData = objDashBoard.Gettwoyearsquartersales(strCompName, iConnectionID);

                foreach (DataRow dr in dsSalesData.Tables[0].Rows)
                {
                    TwoYearsQuarterSalesModel salesdata = new TwoYearsQuarterSalesModel();
                    salesdata.Month = dr["SaleMonthName"].ToString();
                    salesdata.TotalSalesCurYear = Convert.ToDecimal(dr["TotalSaleCurYear"].ToString());
                    salesdata.TotalSalesPrvYear = Convert.ToDecimal(dr["TotalSalePrvYear"].ToString());
                    lstModel.Add(salesdata);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstModel;
        }
        public List<LasTTwoYearsSalesModel> GetYearlySalesData(string strCompName, int iConnectionID)
        {


            List<LasTTwoYearsSalesModel> lstModel = new List<LasTTwoYearsSalesModel>();
            DataSet dsMenu = new DataSet();
            try
            {
                

                objDashBoard = new SmartSys.DL.DashBoard.DashBoardDAL();
                DataSet dsSalesData = objDashBoard.GetYearlySalesData(strCompName, iConnectionID);
                
                foreach (DataRow dr in dsSalesData.Tables[0].Rows)
                {
                    LasTTwoYearsSalesModel salesdata = new LasTTwoYearsSalesModel();
                    salesdata.Month = dr["SaleMonthName"].ToString();
                    salesdata.TotalSalesCurYear = Convert.ToDecimal(dr["TotalSaleCurYear"].ToString());
                    salesdata.TotalSalesPrvYear = Convert.ToDecimal(dr["TotalSalePrvYear"].ToString());
                    lstModel.Add(salesdata);
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstModel;
        }
        public List<LastSevenDayDataModel> GetLastSevenDaysData(int iConnectionID)
        {
            List<LastSevenDayDataModel> lstSevenDaysData = new List<LastSevenDayDataModel>();
            DataSet dsMenu = new DataSet();
            try
            {
                objDashBoard = new SmartSys.DL.DashBoard.DashBoardDAL();
                DataSet dsData = objDashBoard.GetSevenDaysEnquiryData(iConnectionID);
                foreach (DataRow dr in dsData.Tables[0].Rows)
                {
                    LastSevenDayDataModel enquirydata = new LastSevenDayDataModel();

                    enquirydata.Date = Convert.ToDateTime(dr["date(date)"].ToString());
                    enquirydata.dateStr = enquirydata.Date.ToString("dd/MMM/yyyy");
                    enquirydata.TotalcountOfDatewiseData = Convert.ToInt32(dr["count('date')"]);

                    lstSevenDaysData.Add(enquirydata);
                }

            }
            catch (Exception ex)
            {
                Common.LogException(ex);
            }
            return lstSevenDaysData;
        }
    }
}
