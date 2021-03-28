using Model.Bus;
using Model.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Dao
{
    public class ProductDao
    {
        ProductBus Bus = null;
        SecondHandDbContext DbContext = null;
        public ProductDao()
        {
            Bus = new ProductBus();
            DbContext = DataProvider.GetInstance();
        }

        public string Create(Product Model)
        {
            return Bus.Create(Model);
        }

        public string Edit(Product Model)
        {
            return Bus.Edit(Model);
        }

        public string Delete(string id)
        {
            return Bus.Delete(id);
        }

        public string DuplicateCode(string Code)
        {
            return Bus.DuplicateCode(Code);
        }

        public List<Product> ViewProducts(string code)
        {
            return Bus.ViewProducts(code);
        }

        public List<Product> SearchUnsign(string key, int? size, string color)
        {
            try
            {
                //List<BillDetail> ListDetail = DbContext.BillDetails.Where(x => x.Custom.Size == size).ToList();
                //List<IGrouping<string, BillDetail>> ListCustom = ListDetail.GroupBy(x => x.ProductID).ToList();
                string UnSign = CommonDao.convertToUnSign(key);
                string[] ArrName = UnSign.ToLower().Split(' ');
                string search = String.Join("-", ArrName);
                List<Product> ListData = new List<Product>();
                if(size == null && color == "")
                {
                    ListData = DbContext.Products.Where(x => x.NameSearch.Contains(search)).ToList();
                    return ListData;
                }
                else
                {
                    List<OrderDetail> ListDetail = DbContext.OrderDetails.Select(x => x).ToList();
                    if(size != null)
                    {
                        size = (int)size;
                        ListDetail = ListDetail.Where(x => x.Custom.Size == size).ToList();
                    }
                    if(color != "")
                    {
                        ListDetail = ListDetail.Where(x => x.Custom.Color.Equals(color)).ToList();
                    }
                    List<IGrouping<string, OrderDetail>> ListCustom = ListDetail.GroupBy(x => x.ProductID).ToList();
                    foreach (IGrouping<string, OrderDetail> item in ListCustom)
                    {
                        Product ItemCustom = DbContext.Products.Find(item.FirstOrDefault().ProductID);
                        ListData.Add(ItemCustom);
                    }
                    return ListData.Where(x => x.NameSearch.Contains(search)).ToList();
                }
            }catch(Exception e)
            {
                return new List<Product>();
            }
        }
        public List<ProductType> GetListProductType()
        {
            return Bus.GetListProductType();
        }

        public List<Firm> GetListFirm()
        {
            return Bus.GetListFirm();
        }

        public int CreateProductType(ProductType Model)
        {
            return Bus.CreateProductType(Model);
        }

        public int CreateFirm(Firm Model)
        {
            return Bus.CreateFirm(Model);
        }

        public Product GetDetail(string id)
        {
            return Bus.GetDetail(id);
        }

        public List<Inventory> GetInventory(string code)
        {
            return Bus.GetInventory(code);
        }

        // for user view
        public List<Product> GetListOfMenu(string menu, int? type)
        {
            return Bus.GetListOfMenu(menu, type);
        }

        public List<OutSubMenu> GetListType(string menu)
        {
            return Bus.GetListType(menu);
        }

        public List<int> GetListSize()
        {
            List<IGrouping<int, Custom>> ListSize = DbContext.Customs.GroupBy(x => x.Size).ToList();
            return ListSize.Select(x => x.FirstOrDefault().Size).ToList();
        }

        public List<string> GetListColor()
        {
            List<IGrouping<string, Custom>> ListColor = DbContext.Customs.GroupBy(x => x.Color).ToList();
            return ListColor.Select(x => x.FirstOrDefault().Color).ToList();
        }
    }
}