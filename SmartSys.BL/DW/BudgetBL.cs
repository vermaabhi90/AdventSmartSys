using SmartSys.DL.DW;
using SmartSys.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSys.BL.DW
{
   public class BudgetBL
    {
       public int SaveBudgetDetail(BudgetModel budgetModel,string User_Id)
       {

           BudgetDAL objdal = new BudgetDAL();
           DataSet ds = new DataSet();
           try
           {
               ds = objdal.GetSelectedBudget(0);
               if (ds == null)
                   return 0;
               if (ds.Tables.Count > 0)
               {
                   ds.Tables[0].Rows.Clear();
                   DataRow Dr = ds.Tables[0].NewRow();
                   Dr["BudgetId"] = budgetModel.BudgetId;
                   Dr["CustomerId"] = budgetModel.CustomerId;
                   Dr["ItemId"] = budgetModel.ItemId;
                   Dr["EmpId"] = budgetModel.EmpId;
                   Dr["Application"] = budgetModel.Application;
                   Dr["EndEquipment"] = budgetModel.EndEquipment;                 
                   Dr["UnitPrice"] = budgetModel.UnitPrice;
                   Dr["CityId"] = budgetModel.CityId;
                   Dr["RegionId"] = budgetModel.RegionId;
                   Dr["AprQty"] = budgetModel.AprQty;
                   Dr["MayQty"] = budgetModel.MayQty;
                   Dr["JunQty"] = budgetModel.JunQty;
                   Dr["JulQty"] = budgetModel.JulQty;
                   Dr["AugQty"] = budgetModel.AugQty;
                   Dr["SepQty"] = budgetModel.SepQty;
                   Dr["OctQty"] = budgetModel.OctQty;
                   Dr["NovQty"] = budgetModel.NovQty;
                   Dr["DecQty"] = budgetModel.DecQty;
                   Dr["JanQty"] = budgetModel.JanQty;
                   Dr["FebQty"] = budgetModel.FebQty;
                   Dr["MarQty"] = budgetModel.MarQty;

                   Dr["AprAmount"] = budgetModel.AprUSD;
                   Dr["MayAmount"] = budgetModel.MayUSD;
                   Dr["JunAmount"] = budgetModel.JunUSD;
                   Dr["JulAmount"] = budgetModel.JulUSD;
                   Dr["AugAmount"] = budgetModel.AugUSD;
                   Dr["SepAmount"] = budgetModel.SepUSD;
                   Dr["OctAmount"] = budgetModel.OctUSD;
                   Dr["NovAmount"] = budgetModel.NovUSD;
                   Dr["DecAmount"] = budgetModel.DecUSD;
                   Dr["JanAmount"] = budgetModel.JanUSD;
                   Dr["FebAmount"] = budgetModel.FebUSD;
                   Dr["MarAmount"] = budgetModel.MarUSD;

                   Dr["Finyear"] = budgetModel.Finyear;
                   Dr["CCY"] = budgetModel.CCY;
                   ds.Tables[0].Rows.Add(Dr);
               }
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return objdal.SaveUploadValidDataList(ds, User_Id);
       }
       public List<BudgetModel> getBudgetList(string User_Id)
       {
           List<BudgetModel> lstbudget = new List<BudgetModel>();
           try
           {
               BudgetDAL objdal = new BudgetDAL();
               DataSet ds = new DataSet();
               ds = objdal.GetBudgetLIst(User_Id);
               if (ds == null)
                   return null;
               if (ds.Tables.Count > 0)
               {
                   if (ds.Tables[0].Rows.Count > 0)
                   {
                       foreach (DataRow dr in ds.Tables[0].Rows)
                       {
                           BudgetModel objmodel = new BudgetModel();
                           objmodel.BudgetId = Convert.ToInt32(dr["BudgetId"].ToString());
                           objmodel.CustomerName = dr["CustomerName"].ToString();
                           objmodel.EmployeeName = dr["EmpName"].ToString();
                           objmodel.Application = dr["Application"].ToString();
                           objmodel.ItemName = dr["ItemName"].ToString();
                           objmodel.Finyear = dr["Finyear"].ToString();
                            objmodel.City = dr["City"].ToString();
                            objmodel.Region = dr["Region"].ToString();
                            objmodel.EndEquipment = dr["EndEquipment"].ToString();
                           objmodel.UnitPrice =Convert.ToDouble(dr["UnitPrice"].ToString());                         
                           objmodel.sunQuantity =Convert.ToDouble(dr["SumQuanitity"].ToString());
                           lstbudget.Add(objmodel);
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return lstbudget;
       }
       public BudgetModel GetselectedBudget(int BudgetId)
       {
           BudgetModel objmodel = new BudgetModel();
           try
           {
               BudgetDAL objdal = new BudgetDAL();
               DataSet ds = new DataSet();
               ds = objdal.GetSelectedBudget(BudgetId);
               if (ds != null)
               {
                   if (ds.Tables.Count > 0)
                   {
                       if (ds.Tables[0].Rows.Count > 0)
                       {
                           objmodel.BudgetId = BudgetId;
                           objmodel.BudgetId = Convert.ToInt32(ds.Tables[0].Rows[0]["BudgetId"].ToString());
                           objmodel.CustomerId =Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerId"].ToString());
                           objmodel.CustomerName = ds.Tables[0].Rows[0]["CustomerName"].ToString();
                           objmodel.EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                           objmodel.EmpName = ds.Tables[0].Rows[0]["EmpName"].ToString();
                           objmodel.Application = ds.Tables[0].Rows[0]["Application"].ToString();
                           objmodel.ItemId = Convert.ToInt32(ds.Tables[0].Rows[0]["ItemId"].ToString());
                           objmodel.ItemName = ds.Tables[0].Rows[0]["ItemName"].ToString();
                           objmodel.Finyear = ds.Tables[0].Rows[0]["Finyear"].ToString();
                           objmodel.CCY = ds.Tables[0].Rows[0]["CCY"].ToString();
                           objmodel.CityId = Convert.ToInt32(ds.Tables[0].Rows[0]["CityId"].ToString());
                           objmodel.RegionId = Convert.ToInt32(ds.Tables[0].Rows[0]["RegionId"].ToString());
                           objmodel.EndEquipment = ds.Tables[0].Rows[0]["EndEquipment"].ToString();
                           objmodel.UnitPrice = Convert.ToDouble(ds.Tables[0].Rows[0]["UnitPrice"].ToString());                     
                           objmodel.AprQty = Convert.ToDouble(ds.Tables[0].Rows[0]["AprQty"].ToString());
                           objmodel.MayQty = Convert.ToDouble(ds.Tables[0].Rows[0]["MayQty"].ToString());
                           objmodel.JunQty = Convert.ToDouble(ds.Tables[0].Rows[0]["JunQty"].ToString());
                           objmodel.JulQty = Convert.ToDouble(ds.Tables[0].Rows[0]["JulQty"].ToString());
                           objmodel.AugQty = Convert.ToDouble(ds.Tables[0].Rows[0]["AugQty"].ToString());
                           objmodel.SepQty = Convert.ToDouble(ds.Tables[0].Rows[0]["SepQty"].ToString());
                           objmodel.OctQty = Convert.ToDouble(ds.Tables[0].Rows[0]["OctQty"].ToString());
                           objmodel.NovQty = Convert.ToDouble(ds.Tables[0].Rows[0]["NovQty"].ToString());
                           objmodel.DecQty = Convert.ToDouble(ds.Tables[0].Rows[0]["DecQty"].ToString());
                           objmodel.JanQty = Convert.ToDouble(ds.Tables[0].Rows[0]["JanQty"].ToString());
                           objmodel.FebQty = Convert.ToDouble(ds.Tables[0].Rows[0]["FebQty"].ToString());
                           objmodel.MarQty = Convert.ToDouble(ds.Tables[0].Rows[0]["MarQty"].ToString());

                           objmodel.AprUSD = Convert.ToDouble(ds.Tables[0].Rows[0]["AprAmount"].ToString());
                           objmodel.MayUSD = Convert.ToDouble(ds.Tables[0].Rows[0]["MayAmount"].ToString());
                           objmodel.JunUSD = Convert.ToDouble(ds.Tables[0].Rows[0]["JunAmount"].ToString());
                           objmodel.JulUSD = Convert.ToDouble(ds.Tables[0].Rows[0]["JulAmount"].ToString());
                           objmodel.AugUSD = Convert.ToDouble(ds.Tables[0].Rows[0]["AugAmount"].ToString());
                           objmodel.SepUSD = Convert.ToDouble(ds.Tables[0].Rows[0]["SepAmount"].ToString());
                           objmodel.OctUSD = Convert.ToDouble(ds.Tables[0].Rows[0]["OctAmount"].ToString());
                           objmodel.NovUSD = Convert.ToDouble(ds.Tables[0].Rows[0]["NovAmount"].ToString());
                           objmodel.DecUSD = Convert.ToDouble(ds.Tables[0].Rows[0]["DecAmount"].ToString());
                           objmodel.JanUSD = Convert.ToDouble(ds.Tables[0].Rows[0]["JanAmount"].ToString());
                           objmodel.FebUSD = Convert.ToDouble(ds.Tables[0].Rows[0]["FebAmount"].ToString());
                           objmodel.MarUSD = Convert.ToDouble(ds.Tables[0].Rows[0]["MarAmount"].ToString());
                            objmodel.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                            objmodel.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                            objmodel.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                            objmodel.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                        }
                   }
               }

           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return objmodel;
       }      
       public List<BudgetModel>  GetSelectedBudgetCustView(int CustomerId)
       {
           List<BudgetModel> lstbudget = new List<BudgetModel>();
           try
           {
               BudgetDAL objdal = new BudgetDAL();
               DataSet ds = new DataSet();
               ds = objdal.GetSelectedBudgetCustViewLst(CustomerId);
               if (ds == null)
                   return null;
               if (ds.Tables.Count > 0)
               {
                   if (ds.Tables[0].Rows.Count > 0)
                   {
                       foreach (DataRow dr in ds.Tables[0].Rows)
                       {
                           BudgetModel objmodel = new BudgetModel();
                           objmodel.BudgetId = Convert.ToInt32(dr["BudgetId"].ToString());
                           objmodel.CustomerName = dr["CustomerName"].ToString();
                           objmodel.EmployeeName = dr["EmpName"].ToString();
                           objmodel.Application = dr["Application"].ToString();
                           objmodel.ItemName = dr["ItemName"].ToString();
                           objmodel.MPN = dr["MPN"].ToString();
                           objmodel.BrandName = dr["BrandName"].ToString();
                           objmodel.Finyear = dr["Finyear"].ToString();
                           objmodel.City = dr["City"].ToString();
                           objmodel.Region = dr["Region"].ToString();
                           objmodel.EndEquipment = dr["EndEquipment"].ToString();
                           objmodel.UnitPrice = Convert.ToDouble(dr["UnitPrice"].ToString());
                           objmodel.sunQuantity = Convert.ToDouble(dr["SumQuanitity"].ToString());
                           lstbudget.Add(objmodel);
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return lstbudget;
       }
       public List<BudgetModel> GetSelectedBudgetEmpViewLst(int EmpId)
       {
           List<BudgetModel> lstbudget = new List<BudgetModel>();
           try
           {
               BudgetDAL objdal = new BudgetDAL();
               DataSet ds = new DataSet();
               ds = objdal.GetSelectedBudgetEmpViewLst(EmpId);
               if (ds == null)
                   return null;
               if (ds.Tables.Count > 0)
               {
                   if (ds.Tables[0].Rows.Count > 0)
                   {
                       foreach (DataRow dr in ds.Tables[0].Rows)
                       {
                           BudgetModel objmodel = new BudgetModel();
                           objmodel.BudgetId = Convert.ToInt32(dr["BudgetId"].ToString());
                           objmodel.CustomerName = dr["CustomerName"].ToString();
                           objmodel.EmployeeName = dr["EmpName"].ToString();
                           objmodel.Application = dr["Application"].ToString();
                           objmodel.ItemName = dr["ItemName"].ToString();
                           objmodel.Finyear = dr["Finyear"].ToString();
                           objmodel.City = dr["City"].ToString();
                           objmodel.Region = dr["Region"].ToString();
                           objmodel.EndEquipment = dr["EndEquipment"].ToString();
                           objmodel.UnitPrice = Convert.ToDouble(dr["UnitPrice"].ToString());
                           objmodel.sunQuantity = Convert.ToDouble(dr["SumQuanitity"].ToString());
                            objmodel.sumAmount = Convert.ToDouble(dr["SumAmount"].ToString());
                            lstbudget.Add(objmodel);
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return lstbudget;
       }
       public List<BudgetEquipmentModel> getEquipmentList(int ItemId)
       {
           List<BudgetEquipmentModel> lstbudget = new List<BudgetEquipmentModel>();
           try
           {
               BudgetDAL objdal = new BudgetDAL();
               DataSet ds = new DataSet();
               ds = objdal.GetEquipmentList(ItemId);
               if (ds == null)
                   return null;
               if (ds.Tables.Count > 0)
               {
                   if (ds.Tables[0].Rows.Count > 0)
                   {
                       foreach (DataRow dr in ds.Tables[0].Rows)
                       {
                           BudgetEquipmentModel objmodel = new BudgetEquipmentModel();
                           objmodel.EquipmentId = Convert.ToInt32(dr["EquipmentId"].ToString());
                           objmodel.EquipmentName = dr["EquipmentName"].ToString();
                               lstbudget.Add(objmodel);
                       }
                   }
               }
           }
           catch (Exception ex)
           {
               Common.LogException(ex);
           }
           return lstbudget;
       }
       public List<BudgetModel> GetBudgetCompList(List<BudgetModel> ModelList, string EmployeeName)
       {         
           DataSet dsObj = new DataSet();
           BudgetDAL DALObj = new BudgetDAL();
           List<BudgetModel> BudgetModelList = new List<BudgetModel>();
            try
            {
                dsObj = DALObj.GetBudgetCompList(EmployeeName);
                foreach (BudgetModel Model in ModelList)
                {
                    if (dsObj != null)
                    {
                        if (dsObj.Tables.Count > 5)
                        {
                            if (Model.Region == null)
                            {
                                Model.Description = Model.Description + "\n" + "Region not avalable in Excel Sheet.";
                                Model.Check = "NotOk";
                            }
                            else
                            {
                                DataRow dr1 = dsObj.Tables[0].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["Region"]).ToUpper() == Model.Region.ToString().ToUpper().TrimEnd().TrimEnd());
                                if (dr1 != null)
                                {
                                    Model.RegionId = Convert.ToInt32(dr1["RegionId"]);
                                }
                                else
                                {
                                    Model.Description = Model.Description + "\n" + "Region not avalable in System.";
                                    Model.Check = "NotOk";
                                }
                            }


                            if (Model.City == null)
                            {
                                Model.Description = Model.Description + "\n" + "City not avalable in Excel Sheet.";
                                Model.Check = "NotOk";
                            }
                            else
                            {
                                DataRow dr1 = dsObj.Tables[1].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["City"]).ToUpper() == Model.City.ToString().ToUpper().TrimEnd().TrimEnd());
                                if (dr1 != null)
                                {
                                    Model.CityId = Convert.ToInt32(dr1["CityId"]);
                                }
                                else
                                {
                                    Model.Description = Model.Description + "\n" + "City not avalable in System.";
                                    Model.Check = "NotOk";
                                }
                            }

                            if (Model.EmployeeName == null)
                            {
                                Model.Description = Model.Description + "\n" + "This Sales Person not avalable in Excel Sheet .";
                                Model.Check = "NotOk";
                            }
                            else
                            {
                                DataRow dr2 = dsObj.Tables[2].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["EmployeeName"]).ToUpper() == Model.EmployeeName.ToString().ToUpper().TrimEnd().TrimEnd());
                                if (dr2 != null)
                                {
                                    Model.EmpId = Convert.ToInt32(dr2["EmpId"]);
                                }
                                else
                                {
                                    Model.Description = Model.Description + "\n" + "This Sales Person not avalable in System.";
                                    Model.Check = "NotOk";
                                }
                            }



                            if (Model.CustomerName == null)
                            {
                                Model.Description = Model.Description + "\n" + "This Customer not avalable in Excel sheet.";
                                Model.Check = "NotOk";
                            }
                            else
                            {
                                DataRow dr3 = dsObj.Tables[3].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["CustomerName"]).ToUpper() == Model.CustomerName.ToString().ToUpper().TrimEnd().TrimEnd());
                                if (dr3 != null)
                                {
                                    Model.CustomerId = Convert.ToInt32(dr3["CustomerId"].ToString());
                                }
                                else
                                {
                                    Model.Description = Model.Description + "\n" + "This Customer not avalable in System.";
                                    Model.Check = "NotOk";
                                }
                            }

                            if (Model.MPN != null)
                            {
                                DataRow dr4 = dsObj.Tables[4].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["MPN"]).ToUpper() == Model.MPN.ToString().ToUpper().TrimEnd().TrimEnd());
                                if (dr4 != null)
                                {
                                    Model.ItemId = Convert.ToInt32(dr4["ItemId"].ToString());
                                }
                                else
                                {
                                    Model.Description = Model.Description + "\n" + "This MPN not avalable in System.";
                                    Model.Check = "NotOk";
                                }
                            }
                            else
                            {
                                Model.Description = Model.Description + "\n" + "This MPN not avalable in Excel Sheet.";
                                Model.Check = "NotOk";
                            }
                            if (Model.CustomerName != null)
                            {
                                DataRow dr5 = dsObj.Tables[5].AsEnumerable().FirstOrDefault(r => Convert.ToString(r["CustomerName"]).ToUpper() == Model.CustomerName.ToString().ToUpper().TrimEnd().TrimEnd());
                                if (dr5 == null)
                                {
                                    Model.Description = Model.Description + "\n" + "This Customer  not Assign to ." + Model.EmployeeName;
                                    Model.Check = "NotOk";
                                }
                            }
                            BudgetModelList.Add(Model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Common.LogException(ex);
            }
           return BudgetModelList;
       }

       public int SaveUploadValidDataList(BudgetModel budgetModel, string User_Id)
       {
           int ErrCode = 0;
           try
           {
               DataSet dsBudget = new DataSet();
               BudgetDAL DALObj = new BudgetDAL();
               dsBudget = DALObj.GetSelectedBudget(0);
               Double ExRate =Convert.ToDouble(dsBudget.Tables[1].Rows[0]["ExchangeRate"].ToString());
               if (dsBudget != null)
               {
                   if(dsBudget.Tables.Count > 0)
                   {
                       dsBudget.Tables[0].Rows.Clear();
                       foreach (BudgetModel Model in budgetModel.lstBudgetDetail.Where(r => r.Check != "NotOk"))
                       {
                           if(Model.Check !="NotOk")
                           {
                              
                               DataRow Dr = dsBudget.Tables[0].NewRow();
                               Dr["BudgetId"] = Model.BudgetId;
                               Dr["CustomerId"] = Model.CustomerId;
                               Dr["ItemId"] = Model.ItemId;
                               Dr["EmpId"] = Model.EmpId;
                               Dr["Application"] = Model.Application;
                               Dr["EndEquipment"] = Model.EndEquipment;
                                Dr["CityId"] = Model.CityId;
                                Dr["RegionId"] = Model.RegionId;
                                Dr["Finyear"] = Model.Finyear;
                               
                               //if (Model.INRUnitPrice != 0.0 && Model.USDUnitPrice == 0.0)
                               //{
                               //    Dr["CCY"] = "INR";
                               //    Dr["UnitPrice"] = Model.INRUnitPrice;
                               //    Dr["AprAmount"] = Model.AprQty * Model.INRUnitPrice;
                               //    Dr["MayAmount"] = Model.MayQty * Model.INRUnitPrice;
                               //    Dr["JunAmount"] = Model.JunQty * Model.INRUnitPrice;
                               //    Dr["JulAmount"] = Model.JulQty * Model.INRUnitPrice;
                               //    Dr["AugAmount"] = Model.AugQty * Model.INRUnitPrice;
                               //    Dr["SepAmount"] = Model.SepQty * Model.INRUnitPrice;
                               //    Dr["OctAmount"] = Model.OctQty * Model.INRUnitPrice;
                               //    Dr["NovAmount"] = Model.NovQty * Model.INRUnitPrice;
                               //    Dr["DecAmount"] = Model.DecQty * Model.INRUnitPrice;
                               //    Dr["JanAmount"] = Model.JanQty * Model.INRUnitPrice;
                               //    Dr["FebAmount"] = Model.FebQty * Model.INRUnitPrice;
                               //    Dr["MarAmount"] = Model.MarQty * Model.INRUnitPrice;

                               //    Dr["AprQty"] = Model.AprQty;
                               //    Dr["MayQty"] = Model.MayQty;
                               //    Dr["JunQty"] = Model.JunQty;
                               //    Dr["JulQty"] = Model.JulQty;
                               //    Dr["AugQty"] = Model.AugQty;
                               //    Dr["SepQty"] = Model.SepQty;
                               //    Dr["OctQty"] = Model.OctQty;
                               //    Dr["NovQty"] = Model.NovQty;
                               //    Dr["DecQty"] = Model.DecQty;
                               //    Dr["JanQty"] = Model.JanQty;
                               //    Dr["FebQty"] = Model.FebQty;
                               //    Dr["MarQty"] = Model.MarQty; 
                               //}
                              //else if (Model.INRUnitPrice == 0.0 && Model.USDUnitPrice != 0.0)
                              // {
                                   Dr["CCY"] = "INR";
                                   Dr["UnitPrice"] = Model.USDUnitPrice;
                                   Dr["AprAmount"] = Model.AprQty * Model.USDUnitPrice * ExRate;
                                   Dr["MayAmount"] = Model.MayQty * Model.USDUnitPrice * ExRate;
                                   Dr["JunAmount"] = Model.JunQty * Model.USDUnitPrice * ExRate;
                                   Dr["JulAmount"] = Model.JulQty * Model.USDUnitPrice * ExRate;
                                   Dr["AugAmount"] = Model.AugQty * Model.USDUnitPrice * ExRate;
                                   Dr["SepAmount"] = Model.SepQty * Model.USDUnitPrice * ExRate;
                                   Dr["OctAmount"] = Model.OctQty * Model.USDUnitPrice * ExRate;
                                   Dr["NovAmount"] = Model.NovQty * Model.USDUnitPrice * ExRate;
                                   Dr["DecAmount"] = Model.DecQty * Model.USDUnitPrice * ExRate;
                                   Dr["JanAmount"] = Model.JanQty * Model.USDUnitPrice * ExRate;
                                   Dr["FebAmount"] = Model.FebQty * Model.USDUnitPrice * ExRate;
                                   Dr["MarAmount"] = Model.MarQty * Model.USDUnitPrice * ExRate;

                                   Dr["AprQty"] = Model.AprQty;
                                   Dr["MayQty"] = Model.MayQty;
                                   Dr["JunQty"] = Model.JunQty;
                                   Dr["JulQty"] = Model.JulQty;
                                   Dr["AugQty"] = Model.AugQty;
                                   Dr["SepQty"] = Model.SepQty;
                                   Dr["OctQty"] = Model.OctQty;
                                   Dr["NovQty"] = Model.NovQty;
                                   Dr["DecQty"] = Model.DecQty;
                                   Dr["JanQty"] = Model.JanQty;
                                   Dr["FebQty"] = Model.FebQty;
                                   Dr["MarQty"] = Model.MarQty; 
                              // }
                              //else if (Model.INRUnitPrice != 0.0 && Model.USDUnitPrice != 0.0)
                              // {
                              //     Dr["CCY"] = "INR";
                              //     Dr["UnitPrice"] = Model.INRUnitPrice;
                              //     Dr["AprAmount"] = Model.AprQty * Model.INRUnitPrice;
                              //     Dr["MayAmount"] = Model.MayQty * Model.INRUnitPrice;
                              //     Dr["JunAmount"] = Model.JunQty * Model.INRUnitPrice;
                              //     Dr["JulAmount"] = Model.JulQty * Model.INRUnitPrice;
                              //     Dr["AugAmount"] = Model.AugQty * Model.INRUnitPrice;
                              //     Dr["SepAmount"] = Model.SepQty * Model.INRUnitPrice;
                              //     Dr["OctAmount"] = Model.OctQty * Model.INRUnitPrice;
                              //     Dr["NovAmount"] = Model.NovQty * Model.INRUnitPrice;
                              //     Dr["DecAmount"] = Model.DecQty * Model.INRUnitPrice;
                              //     Dr["JanAmount"] = Model.JanQty * Model.INRUnitPrice;
                              //     Dr["FebAmount"] = Model.FebQty * Model.INRUnitPrice;
                              //     Dr["MarAmount"] = Model.MarQty * Model.INRUnitPrice;

                              //     Dr["AprQty"] = Model.AprQty;
                              //     Dr["MayQty"] = Model.MayQty;
                              //     Dr["JunQty"] = Model.JunQty;
                              //     Dr["JulQty"] = Model.JulQty;
                              //     Dr["AugQty"] = Model.AugQty;
                              //     Dr["SepQty"] = Model.SepQty;
                              //     Dr["OctQty"] = Model.OctQty;
                              //     Dr["NovQty"] = Model.NovQty;
                              //     Dr["DecQty"] = Model.DecQty;
                              //     Dr["JanQty"] = Model.JanQty;
                              //     Dr["FebQty"] = Model.FebQty;
                              //     Dr["MarQty"] = Model.MarQty; 
                              // }
                               //else
                               //{
                               //    Dr["CCY"] = "INR";
                               //    Dr["UnitPrice"] = Model.INRUnitPrice;
                               //    Dr["AprAmount"] = Model.AprQty * Model.INRUnitPrice;
                               //    Dr["MayAmount"] = Model.MayQty * Model.INRUnitPrice;
                               //    Dr["JunAmount"] = Model.JunQty * Model.INRUnitPrice;
                               //    Dr["JulAmount"] = Model.JulQty * Model.INRUnitPrice;
                               //    Dr["AugAmount"] = Model.AugQty * Model.INRUnitPrice;
                               //    Dr["SepAmount"] = Model.SepQty * Model.INRUnitPrice;
                               //    Dr["OctAmount"] = Model.OctQty * Model.INRUnitPrice;
                               //    Dr["NovAmount"] = Model.NovQty * Model.INRUnitPrice;
                               //    Dr["DecAmount"] = Model.DecQty * Model.INRUnitPrice;
                               //    Dr["JanAmount"] = Model.JanQty * Model.INRUnitPrice;
                               //    Dr["FebAmount"] = Model.FebQty * Model.INRUnitPrice;
                               //    Dr["MarAmount"] = Model.MarQty * Model.INRUnitPrice;

                               //    Dr["AprQty"] = Model.AprQty;
                               //    Dr["MayQty"] = Model.MayQty;
                               //    Dr["JunQty"] = Model.JunQty;
                               //    Dr["JulQty"] = Model.JulQty;
                               //    Dr["AugQty"] = Model.AugQty;
                               //    Dr["SepQty"] = Model.SepQty;
                               //    Dr["OctQty"] = Model.OctQty;
                               //    Dr["NovQty"] = Model.NovQty;
                               //    Dr["DecQty"] = Model.DecQty;
                               //    Dr["JanQty"] = Model.JanQty;
                               //    Dr["FebQty"] = Model.FebQty;
                               //    Dr["MarQty"] = Model.MarQty;                                 
                               //}
                               dsBudget.Tables[0].Rows.Add(Dr);
                           }
                       }
                   }
               }
               foreach (DataRow dr in dsBudget.Tables[0].Rows)
               {
                   DataSet SaveDataDS = new DataSet();
                   SaveDataDS.Tables.Add(dsBudget.Tables[0].Clone());
                   SaveDataDS.Tables[0].ImportRow(dr);
                   ErrCode = DALObj.SaveUploadValidDataList(SaveDataDS, User_Id);
               }
           }
           catch (Exception ex)
           {               
               throw ex;
           }
           return ErrCode;
       }
    }
}
