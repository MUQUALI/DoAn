using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.CustomModel
{
    public class OutViewOrderDetail
    {
        public OutViewOrderDetail(string code, string name, int size, string color, int qty, decimal price, decimal money)
        {
            ProductCode = code;
            ProductName = name;
            Size = size;
            Color = color;
            Quantity = qty;
            UnitPrice = price;
            Money = money;
        }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int CustomID { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Money { get; set; }
    }
}