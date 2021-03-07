using Model.Bus;
using Model.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Dao
{
    public class OrderDao
    {
        OrderBus Bus = null;

        public OrderDao()
        {
            Bus = new OrderBus();
        }

        public void CheckDuplicateCustom(int size, string color)
        {
            Bus.CheckDuplicateCustom(size, color);
        }

        public ViewOrderDetail GetDetail(string code)
        {
            return Bus.GetDetail(code);
        }

        public int CreateOrderRecord(ViewOrder Model, int AccountID, List<ViewOrder.ItemDetail> ListDetail)
        {
            return Bus.CreateOrderRecord(Model, AccountID, ListDetail);
        }

        public List<OutViewOrder> Search(string OrderID, DateTime? FromDate, DateTime? ToDate)
        {
            return Bus.Search(OrderID, FromDate, ToDate);
        }

        public List<OutViewOrderDetail> GetDetaislFromID(string ID)
        {
            return Bus.GetDetaislFromID(ID);
        }
    }
}