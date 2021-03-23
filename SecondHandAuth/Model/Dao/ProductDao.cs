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

        public List<Product> SearchUnsign(string key)
        {
            try
            {
                string UnSign = CommonDao.convertToUnSign(key);
                string[] ArrName = UnSign.ToLower().Split(' ');
                string search = String.Join("-", ArrName);
                return DbContext.Products.Where(x => x.NameSearch.Contains(search)).ToList();
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
    }
}