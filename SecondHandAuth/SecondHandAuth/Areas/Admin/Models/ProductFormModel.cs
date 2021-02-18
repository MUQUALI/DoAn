using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondHandAuth.Areas.Admin.Models
{
    public class ProductFormModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int FK_ProductTypeID { get; set; }
        public int FK_FirmID { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SalePrice { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        public HttpPostedFileBase[] Images { get; set; }

        // exec to return string list image url
        public string StrImages
        {
            get
            {
                string Storage = "";

                foreach(HttpPostedFileBase file in Images)
                {
                    if(file != null)
                    {
                        Storage += file.FileName + ";";
                    }
                }
                // loại bỏ đi dấu ; ở cuối chuỗi
                return Storage.Remove(Storage.Length - 1, 1);
            }
        }
    }
}