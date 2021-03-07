using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.CustomModel
{
    public class OutProduct
    {
        public OutProduct()
        {

        }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public int FK_CustomID { get; set; }
    }
}