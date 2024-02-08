using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.DW
{
   public class BudgetModel
    {
        public int BudgetId { get; set; }
        public int CustomerId { get; set; }
       [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        public string Application { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string EndEquipment { get; set; }
        public double UnitPrice { get; set; }
        public double INRUnitPrice { get; set; }
        public double USDUnitPrice { get; set; }
        public double USDRate { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public int RegionId { get; set; }
        public string Region { get; set; }
        public string EmployeeName { get; set; }
        public int Sequence { get; set; }
        public string MPN { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }
        public string Check { get; set; }
        public string CCY { get; set; }
        public string Finyear { get; set; }
        public double AprQty { get; set; }
        public double MayQty { get; set; }
        public double JunQty { get; set; }
        public double JulQty { get; set; }
        public double AugQty { get; set; }
        public double SepQty { get; set; }
        public double OctQty { get; set; }
        public double NovQty { get; set; }
        public double DecQty { get; set; }
        public double JanQty { get; set; }
        public double FebQty { get; set; }
        public double MarQty { get; set; }
        public double sunQuantity { get; set; }
        public double sumAmount { get; set; }
        public double AprUSD { get; set; }
        public double MayUSD { get; set; }
        public double JunUSD { get; set; }
        public double JulUSD { get; set; }
        public double AugUSD { get; set; }
        public double SepUSD { get; set; }
        public double OctUSD { get; set; }
        public double NovUSD { get; set; }
        public double DecUSD { get; set; }
        public double JanUSD { get; set; }
        public double FebUSD { get; set; }
        public double MarUSD { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ReportId { get; set; }
        public string ParamId { get; set; }
        public string ParamName { get; set; }
        public string TxtParamValue { get; set; }
        public string hidText { get; set; }
        public int StatusId { get; set; }
        public string OutputLocation { get; set; }
        public DateTime RunDate { get; set; }
        public List<BudgetModel> lstBudgetDetail { get; set; }
        public List<BudgetModel> lstBudgetDetailCustView { get; set; }
        public List<BudgetCustomerModel> CustomerLst  { get; set; }
        public List<BudgetItemModel> ItemLst { get; set; }
        public List<BudgetCustmerModel> EmployeeLst { get; set; }
        public List<BudgetEquipmentModel> EquipmentLst { get; set; }
    }
   public class BudgetCustomerModel
   {
        public int CustomerId { get; set; }    
        public string CustomerName { get; set; }
   }
   public class BudgetItemModel
   {
       public int ItemId { get; set; }
       public string ItemName { get; set; }
   }
   public class BudgetCustmerModel
   {
       public int EmployeeId { get; set; }
       public string EmployeeName { get; set; }
   }
   public class BudgetEquipmentModel
   {
       public int EquipmentId { get; set; }
       public string EquipmentName { get; set; }
   }
}
