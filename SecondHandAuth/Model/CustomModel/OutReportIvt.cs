using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.CustomModel
{
    public class OutReportIvt
    {
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public int BuyQty { get; set; }

        public int SaleQty { get; set; }

        public int Inventory { get; set; }

    }
}