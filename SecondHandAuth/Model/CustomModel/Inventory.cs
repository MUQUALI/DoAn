using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.CustomModel
{
    public class Inventory
    {
        public string ProductCode { get; set; }
        public int FK_CustomID { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
        public int Quantity { get; set; }
    }
}