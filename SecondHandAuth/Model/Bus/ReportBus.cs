using Model.CustomModel;
using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Bus
{
    public class ReportBus
    {
        SecondHandDbContext DbContext = null;
        public ReportBus()
        {
            DbContext = DataProvider.GetInstance();
        }
        public decimal[] GetSalePerYear(int? y)
        {
            int year = y == null ? DateTime.Now.Year : (int)y;
            try
            {
                List<decimal> revenues = new List<decimal>();
                for (int i = 1; i < 13; i++)
                {
                    List<Bill> ListSaleOfMonth = DbContext.Bills.Where(x => x.CreatedDate.Year == year && x.CreatedDate.Month == i && x.Status == 3).ToList();
                    if(ListSaleOfMonth.Count > 0)
                    {
                        decimal Revenue = ListSaleOfMonth.Sum(x => x.TotalMoney);
                        revenues.Add(Revenue);
                    }
                    else
                    {
                        revenues.Add(0);
                    }
                    
                }
                return revenues.ToArray();
            }
            catch (Exception e)
            {
                return new List<decimal>().ToArray();
            }
        }

        public decimal[] GetCostPerYear(int? y)
        {
            int year = y == null ? DateTime.Now.Year : (int)y;
            try
            {
                List<decimal> costs = new List<decimal>();
                for (int i = 1; i < 13; i++)
                {
                    List<Order> ListCostOfMonth = DbContext.Orders.Where(x => x.CreatedDate.Year == year && x.CreatedDate.Month == i).ToList();
                    if (ListCostOfMonth.Count > 0)
                    {
                        decimal Cost = ListCostOfMonth.Sum(x => x.TotalMoney);
                        costs.Add(Cost);
                    }
                    else
                    {
                        costs.Add(0);
                    }

                }
                return costs.ToArray();
            }
            catch (Exception e)
            {
                return new List<decimal>().ToArray();
            }
        }

        public decimal[] GetProfitPerYear(int? y)
        {
            int year = y == null ? DateTime.Now.Year : (int)y;
            try
            {
                List<decimal> profits = new List<decimal>();
                for (int i = 1; i < 13; i++)
                {
                    decimal Sale = 0;
                    decimal Cost = 0;
                    decimal Profit = 0;
                    List<Bill> ListSaleOfMonth = DbContext.Bills.Where(x => x.CreatedDate.Year == year && x.CreatedDate.Month == i && x.Status == 3).ToList();
                    List<Order> ListCostOfMonth = DbContext.Orders.Where(x => x.CreatedDate.Year == year && x.CreatedDate.Month == i).ToList();
                    if (ListCostOfMonth.Count > 0)
                    {
                        Cost = ListCostOfMonth.Sum(x => x.TotalMoney);
                    }
                    if(ListSaleOfMonth.Count > 0)
                    {
                        Sale = ListSaleOfMonth.Sum(x => x.TotalMoney);
                    }
                    Profit = Sale - Cost;
                    profits.Add(Profit);
                }
                return profits.ToArray();
            }
            catch (Exception e)
            {
                return new List<decimal>().ToArray();
            }
        }

        public string[] getListLableOfProduct(int? Month, int? Year)
        {
            int month = Month == null ? DateTime.Now.Month : (int)Month;
            int year = Year == null ? DateTime.Now.Year : (int)Year;
            try
            {
                List<string> Labels = new List<string>();
                List<IGrouping<string, BillDetail>> ListLabel = DbContext.BillDetails
                    .Where(x => x.Bill.CreatedDate.Month == month && x.Bill.CreatedDate.Year == year).GroupBy(x => x.ProductID).ToList();

                foreach (IGrouping<string, BillDetail> item in ListLabel)
                {
                    BillDetail detail = item.FirstOrDefault();
                    string Label = detail.ProductID + " - " + detail.ProductName;
                    Labels.Add(Label);
                }
                return Labels.ToArray();
            }
            catch (Exception e)
            {
                return new List<string>().ToArray();
            }
        }

        public int[] GetQtySaleProduct(int? Month, int? Year)
        {
            int month = Month == null ? DateTime.Now.Month : (int)Month;
            int year = Year == null ? DateTime.Now.Year : (int)Year;
            List<int> data = new List<int>();
            try
            {
                string[] labels = getListLableOfProduct(month, year);

                for(int i=0; i< labels.Length; i++)
                {
                    string Code = labels[i].Split('-')[0].TrimEnd();
                    int qty = DbContext.BillDetails
                        .Where(x => x.ProductID.Equals(Code) && x.Bill.CreatedDate.Month == month && x.Bill.CreatedDate.Year == year)
                        .Sum(x => x.Quantity);
                    data.Add(qty);
                }
                return data.ToArray();
            }
            catch (Exception e)
            {
                return new List<int>().ToArray();
            }
        }

        public List<OutReportIvt> GetListReportInventory()
        {
            try
            {
                List<OutReportIvt> OutData = new List<OutReportIvt>();
                List<Product> Products = DbContext.Products.ToList();

                foreach (Product item in Products)
                {
                    int SaleQty = 0;
                    int BuyQty = 0;
                    OutReportIvt DataItem = new OutReportIvt();
                    if (DbContext.BillDetails.Where(x => x.ProductID.Equals(item.PK_ProductID)).ToList().Count > 0)
                    {
                        SaleQty = DbContext.BillDetails.Where(x => x.ProductID.Equals(item.PK_ProductID)).Sum(x => x.Quantity);
                    }
                    if(DbContext.OrderDetails.Where(x => x.ProductID.Equals(item.PK_ProductID)).ToList().Count > 0)
                    {
                        BuyQty = DbContext.OrderDetails.Where(x => x.ProductID.Equals(item.PK_ProductID)).Sum(x => x.Quantity);
                    }

                    DataItem.ProductCode = item.PK_ProductID;
                    DataItem.ProductName = item.Name;
                    DataItem.BuyQty = BuyQty;
                    DataItem.SaleQty = SaleQty;
                    DataItem.Inventory = BuyQty - SaleQty;
                    OutData.Add(DataItem);
                }
                return OutData;
            }
            catch (Exception e)
            {
                return new List<OutReportIvt>();
            }
        }

    }

}