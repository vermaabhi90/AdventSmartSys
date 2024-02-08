using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.DashBoard
{
  
  public class MonthlySalesDataModel
  {
      public string Month { get; set; }
      public decimal TotalSales { get; set; }
      public string CompanyCode { get; set; }
      public List<MonthlySalesDataModel> lstSaleChart { get; set; }
  }
  public class LasTTwoYearsSalesModel
  {
      public string Month { get; set; }
      public decimal TotalSalesPrvYear { get; set; }
      public decimal TotalSalesCurYear { get; set; }
  }
  public class TwoYearsQuarterSalesModel
  {
      public string Month { get; set; }
      public decimal TotalSalesPrvYear { get; set; }
      public decimal TotalSalesCurYear { get; set; }
  }
  public class LastSevenDayDataModel
  {
      //public string Date { get; set; }
      public DateTime Date { get; set; }
      public string dateStr { get; set; }
      public int TotalcountOfDatewiseData { get; set; }

      public List<LastSevenDayDataModel> lstSevenDaysRecord { get; set; }
  }
}
