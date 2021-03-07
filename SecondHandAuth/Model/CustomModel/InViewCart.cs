using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.CustomModel
{
    public class InViewCart
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public decimal TotalMoney { get; set; }
        public List<InViewCartDetail> ListDetail { get; set; }

        public InViewCartDetail ItemAddToCart { get; set; }

        public class InViewCartDetail
        {
            public string productID { get; set; }
            public string productName { get; set; }
            public int Size { get; set; }
            public string Color { get; set; }
            public int qty { get; set; }
            public decimal price { get; set; }
            public decimal Money { get; set; }

            public int custom { get; set; }

            public string image { get; set; }

        }
    }
}