using Model.CustomModel;
using Model.Dao;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Model.Bus
{
    public class ProductBus
    {
        SecondHandDbContext DbContext = null;

        public ProductBus()
        {
            DbContext = DataProvider.GetInstance();
        }

        public string Create(Product Model)
        {
            DbContext.Products.Add(Model);
            DbContext.SaveChanges();
            return "200";
        }

        public string Edit(Product Model)
        {
            Product ItemEdit = DbContext.Products.Find(Model.PK_ProductID);
            // edit
            ItemEdit.Name = Model.Name;
            ItemEdit.FK_ProductTypeID = Model.FK_ProductTypeID;
            ItemEdit.FK_FirmID = Model.FK_FirmID;
            if(Model.Images != "")
            {
                ItemEdit.Images = Model.Images;
            }
            ItemEdit.BuyPrice = Model.BuyPrice;
            ItemEdit.SalePrice = Model.SalePrice;
            ItemEdit.Status = Model.Status;
            ItemEdit.Description = Model.Description;

            DbContext.SaveChanges();
            return "200";
            
        }

        public string DuplicateCode(string Code)
        {
            if(DbContext.Products.Count(x => x.PK_ProductID.Equals(Code)) > 0)
            {
                return "300";
            }
            return "";
        }

        public List<Product> ViewProducts()
        {
            return DbContext.Products.Where(x => x.DelFlg.Equals(0)).ToList();
        }

        public List<ProductType> GetListProductType()
        {
            return DbContext.ProductTypes.ToList();
        }

        public List<Firm> GetListFirm()
        {
            return DbContext.Firms.ToList();
        }

        public int CreateProductType(ProductType Model)
        {
            DbContext.ProductTypes.Add(Model);
            DbContext.SaveChanges();
            return Model.PK_TypeID;
           
            
        }

        public int CreateFirm(Firm Model)
        {
            DbContext.Firms.Add(Model);
            DbContext.SaveChanges();
            return Model.PK_FirmID;
        }

        public Product GetDetail(string id)
        {
            return DbContext.Products.Where(x => x.PK_ProductID.Equals(id)).FirstOrDefault();
        }

        public List<Inventory> GetInventory(string code)
        {
            List<Inventory> ListOrderIvt, ListSaleIvt, ListBackIvt;
            // get query tồn kho từ đơn nhập hàng
            var GetOrderInventory = 
                        from odt in DbContext.OrderDetails
                        where odt.ProductID.Equals(code)
                        group odt by odt.FK_CustomID
                        into odtg
                        let ivt = odtg.Sum(x => x.Quantity)
                        select new Inventory()
                        {
                            ProductCode = odtg.FirstOrDefault().ProductID,
                            Size = odtg.FirstOrDefault().Custom.Size,
                            Color = odtg.FirstOrDefault().Custom.Color,
                            Quantity = ivt
                        };

            // get query tồn kho từ hóa đơn bán
            var GetSaleInventory =
                        from bdt in DbContext.BillDetails
                        where bdt.ProductID.Equals(code)
                        group bdt by bdt.FK_CustomID
                        into bdtg
                        let ivt = bdtg.Sum(x => x.Quantity)
                        select new Inventory()
                        {
                            ProductCode = bdtg.FirstOrDefault().ProductID,
                            Size = bdtg.FirstOrDefault().Custom.Size,
                            Color = bdtg.FirstOrDefault().Custom.Color,
                            Quantity = ivt
                        };

            // get query tồn kho từ hàng trả lại
            var GetBackInventory =
                        from gdt in DbContext.GiveBackDetails
                        where gdt.ProductID.Equals(code)
                        group gdt by gdt.FK_CustomID
                        into gdtg
                        let ivt = gdtg.Sum(x => x.Quantity)
                        select new Inventory()
                        {
                            ProductCode = gdtg.FirstOrDefault().ProductID,
                            Size = gdtg.FirstOrDefault().Custom.Size,
                            Color = gdtg.FirstOrDefault().Custom.Color,
                            Quantity = ivt
                        };

            ListOrderIvt = GetOrderInventory.ToList();
            ListSaleIvt = GetSaleInventory.ToList();
            ListBackIvt = GetBackInventory.ToList();

            //Inventory item = ListOrderIvt.Find(x => x.Size == 42 && x.Color.Equals("đen"));
            if(ListSaleIvt.Count > 0)
            {
                foreach (Inventory item in ListSaleIvt)
                {
                    if (ListOrderIvt.IndexOf(item) >= 0)
                    {
                        int index = ListOrderIvt.IndexOf(item);
                        ListOrderIvt[index].Quantity -= item.Quantity;
                    }
                }
            }

            if (ListBackIvt.Count > 0)
            {
                foreach (Inventory item in ListBackIvt)
                {
                    if (ListOrderIvt.IndexOf(item) >= 0)
                    {
                        int index = ListOrderIvt.IndexOf(item);
                        ListOrderIvt[index].Quantity += item.Quantity;
                    }
                }
            }


            return ListOrderIvt;
        }
    }
}