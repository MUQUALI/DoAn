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
    }
}