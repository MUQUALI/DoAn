using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.CustomModel
{
    public class ViewOrder
    {
        public string Supplier { get; set; }
        public decimal TotalMoney { get; set; }
        public string Note { get; set; }

        public string Phone { get; set; }

        // set List Detail
        public string ListDetail { get; set; }

   
        // Item Detail
        public class ItemDetail
        {
            public string ProductID { get; set; }
            public string ProductName { get; set; }
            public int Size { get; set; }
            public string Color { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Money { get; set; }

        }
    }
}