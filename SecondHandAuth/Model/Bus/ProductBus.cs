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
            string UnSign = CommonDao.convertToUnSign(Model.Name);
            string[] ArrName = UnSign.ToLower().Split(' ');
            string HotName = String.Join("-", ArrName);
            Model.NameSearch = CommonDao.convertToUnSign(HotName);
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

        public string Delete(string id)
        {
            Product DelItem = DbContext.Products.Find(id);
            DelItem.DelFlg = 1;
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

        public List<Product> ViewProducts(string code)
        {
            return DbContext.Products.Where(x => x.DelFlg.Equals(0) && x.PK_ProductID.Contains(code)).ToList();
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
                            FK_CustomID = odtg.FirstOrDefault().Custom.CustomID,
                            Color = odtg.FirstOrDefault().Custom.Color,
                            Quantity = ivt
                        };

            // get query tồn kho từ hóa đơn bán
            var GetSaleInventory =
                        from bdt in DbContext.BillDetails
                        where bdt.ProductID.Equals(code) && bdt.Bill.Status != 4
                        group bdt by bdt.FK_CustomID
                        into bdtg
                        let ivt = bdtg.Sum(x => x.Quantity)
                        select new Inventory()
                        {
                            ProductCode = bdtg.FirstOrDefault().ProductID,
                            Size = bdtg.FirstOrDefault().Custom.Size,
                            FK_CustomID = bdtg.FirstOrDefault().Custom.CustomID,
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
                            FK_CustomID = gdtg.FirstOrDefault().Custom.CustomID,
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
                    Inventory ItemSearch = ListOrderIvt.Find(x => x.FK_CustomID == item.FK_CustomID);
                    if (ItemSearch != null)
                    {
                        ItemSearch.Quantity -= item.Quantity;
                    }
                }
            }

            if (ListBackIvt.Count > 0)
            {
                foreach (Inventory item in ListBackIvt)
                {
                    Inventory ItemSearch = ListOrderIvt.Find(x => x.FK_CustomID == item.FK_CustomID);
                    if (ItemSearch != null)
                    {
                        ItemSearch.Quantity += item.Quantity;
                    }
                }
            }


            return ListOrderIvt;
        }

        // for user view
        public List<Product> GetListOfMenu(string menu, int? type)
        {
            List<Product> ListFilter = DbContext.Products.Where(x => x.Firm.FirmName.Equals(menu)).ToList(); ;
            if(type != null)
            {
                ListFilter = ListFilter.Where(x => x.FK_ProductTypeID == type).ToList();
            }
            return ListFilter;
        }

        public List<OutSubMenu> GetListType(string menu)
        {
            List<ProductType> ListFilter = DbContext.ProductTypes.ToList();
            List<OutSubMenu> Submenu = new List<OutSubMenu>();
            foreach (ProductType item in ListFilter)
            {
                OutSubMenu Sub = new OutSubMenu();
                Sub.FirmName = menu;
                Sub.Type = (int)item.PK_TypeID;
                Sub.TypeName = item.TypeName;

                Submenu.Add(Sub);
            }
            return Submenu;
        }
    }
}