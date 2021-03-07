using Model.CustomModel;
using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Bus
{
    public class OrderBus
    {
        SecondHandDbContext DbContext = null;
        public OrderBus()
        {
            DbContext = DataProvider.GetInstance();
        }
        public void CheckDuplicateCustom(int size, string color)
        {
            color = color.ToLower().Trim();
            Custom item = DbContext.Customs.Where(x => x.Size == size && x.Color.Equals(color)).FirstOrDefault();
            if(item == null)
            {
                item = new Custom();
                item.Color = color;
                item.Size = size;
                DbContext.Customs.Add(item);
                DbContext.SaveChanges();
            }
        }

        public ViewOrderDetail GetDetail(string code)
        {
            Product info = DbContext.Products.Find(code);
            ViewOrderDetail Detail = new ViewOrderDetail();
            Detail.Code = info.PK_ProductID;
            Detail.DetailName = info.Name;
            Detail.BuyPrice = info.BuyPrice;

            return Detail;
        }

        public int CreateOrderRecord(ViewOrder Model, int AccountID, List<ViewOrder.ItemDetail> ListDetail)
        {
            // create new order
            Order OrderRecord = new Order();
            OrderRecord.FK_AccountID = AccountID;
            OrderRecord.Supplier = Model.Supplier + " - " + Model.Phone;
            OrderRecord.CreatedDate = DateTime.Now;
            OrderRecord.TotalMoney = Model.TotalMoney;
            OrderRecord.Note = Model.Note;
            OrderRecord.DelFlg = 0;
            //save
            DbContext.Orders.Add(OrderRecord);
            DbContext.SaveChanges();

            // create order detail
            int OrderID = OrderRecord.PK_OrderID;
            
            foreach(ViewOrder.ItemDetail item in ListDetail)
            {
                Product ItemChange = DbContext.Products.Find(item.ProductID);
                OrderDetail detail = new OrderDetail();
                detail.OrderID = OrderID;
                detail.ProductID = item.ProductID;
                // find custom
                Custom ct = DbContext.Customs.Where(x => x.Size == item.Size && x.Color.Equals(item.Color)).FirstOrDefault();

                detail.FK_CustomID = ct.CustomID;
                detail.ProductName = item.ProductName;
                detail.Quantity = item.Quantity;
                detail.UnitPrice = item.UnitPrice;
                detail.Money = item.Money;
                //change qty product
                ItemChange.Quantity += item.Quantity;
                DbContext.SaveChanges();
                //save to db
                DbContext.OrderDetails.Add(detail);
                DbContext.SaveChanges();
            }

            return OrderID;
        }

        public List<OutViewOrder> Search(string OrderID, DateTime? FromDate, DateTime? ToDate)
        {
            List<Order> ListData = DbContext.Orders.Where(x => x.DelFlg == 0).ToList();

            if(!String.IsNullOrEmpty(OrderID))
            {
                ListData = ListData.Where(x => x.PK_OrderID == int.Parse(OrderID)).ToList();
            }

            if(FromDate != null)
            {
                ListData = ListData.Where(x => x.CreatedDate >= FromDate).ToList();
            }

            if(ToDate != null)
            {
                ListData = ListData.Where(x => x.CreatedDate <= ToDate).ToList();
            }
            //List<Order> Datas = ListData.ToList();
            List<OutViewOrder> OutListData = new List<OutViewOrder>();

            foreach (Order item in ListData)
            {
                string CreatedUser = item.Account.Employee != null ? item.Account.Employee.Name : "";
                OutViewOrder OutItem = new OutViewOrder(item.PK_OrderID, item.Account.Username, CreatedUser, item.Supplier, item.CreatedDate, item.TotalMoney, item.Note);
                OutListData.Add(OutItem);
            }
            return OutListData;
        }

        public List<OutViewOrderDetail> GetDetaislFromID(string ID)
        {
            int OrderID = int.Parse(ID);
            List<OrderDetail> ListDetail = DbContext.OrderDetails.Where(x => x.OrderID == OrderID).ToList();

            List<OutViewOrderDetail> OutData = new List<OutViewOrderDetail>();

            foreach (OrderDetail item in ListDetail)
            {
                OutViewOrderDetail Oitem = new OutViewOrderDetail(item.ProductID, item.ProductName, item.Custom.Size, item.Custom.Color, item.Quantity, item.UnitPrice, item.Money);
                OutData.Add(Oitem);
            }

            return OutData;
        }
    }
}