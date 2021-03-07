using Model.CustomModel;
using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Bus
{
    public class CartBus
    {
        SecondHandDbContext DbContext = null;
        public CartBus()
        {
            DbContext = DataProvider.GetInstance();
        }
        public string AddToCart(InViewCart Model, int? UserID)
        {
            int totalItem = 0;
            Cart cart = new Cart();
            cart.FK_UserID = UserID;
            
            
            // save cart
            DbContext.Carts.Add(cart);
            DbContext.SaveChanges();

            int CartID = cart.CartID;
            decimal TotalMoney = 0;
            // add list item khi đã đăng nhập và có giỏ hàng trong localStorage
            // save detail
            if(Model.ListDetail != null)
            {
                foreach (InViewCart.InViewCartDetail item in Model.ListDetail)
                {
                    totalItem += item.qty;
                    CartDetail Detail = new CartDetail();
                    Product info = DbContext.Products.Find(item.productID);
                    Detail.CartID = CartID;
                    Detail.ProductID = item.productID;
                    Detail.ProductName = info.Name;
                    Detail.Quantity = item.qty;
                    Detail.UnitPrice = info.SalePrice;
                    Detail.Money = info.SalePrice * item.qty;
                    Detail.FK_CustomID = item.custom;

                    TotalMoney += Detail.Money;

                    DbContext.CartDetails.Add(Detail);
                    DbContext.SaveChanges();
                }
            }
            // add 1 item vào giỏ khi đăng nhập nhưng chưa có giỏ hàng trong localStorage
            if (Model.ItemAddToCart != null)
            {
                totalItem += Model.ItemAddToCart.qty;
                CartDetail Detail = new CartDetail();
                Product info = DbContext.Products.Find(Model.ItemAddToCart.productID);
                Detail.CartID = CartID;
                Detail.ProductID = Model.ItemAddToCart.productID;
                Detail.ProductName = info.Name;
                Detail.Quantity = Model.ItemAddToCart.qty;
                Detail.UnitPrice = info.SalePrice;
                Detail.Money = info.SalePrice * Model.ItemAddToCart.qty;
                Detail.FK_CustomID = Model.ItemAddToCart.custom;

                TotalMoney += Detail.Money;

                DbContext.CartDetails.Add(Detail);
                DbContext.SaveChanges();
            }
            
            cart.TotalItem = totalItem;
            cart.TotalMoney = TotalMoney;
            DbContext.SaveChanges();
            string OutData = "{\"totalItem\":" + totalItem + ",\"cartID\":" + CartID + " }";

            return OutData;
        }

        public string EditCart(int CartID, InViewCart.InViewCartDetail Item)
        {
            Cart cart = DbContext.Carts.Find(CartID);
            Product info = DbContext.Products.Find(Item.productID);
            CartDetail EditDetail = cart.CartDetails.Where(x => x.ProductID.Equals(Item.productID) && x.FK_CustomID == Item.custom).FirstOrDefault();
            if(EditDetail != null)
            {
                EditDetail.Quantity += Item.qty;
                DbContext.SaveChanges();
            }
            else
            {
                CartDetail Detail = new CartDetail();
                
                Detail.CartID = CartID;
                Detail.ProductID = Item.productID;
                Detail.ProductName = Item.productName;
                Detail.Quantity = Item.qty;
                Detail.UnitPrice = info.SalePrice;
                Detail.Money = info.SalePrice * Item.qty;
                Detail.FK_CustomID = Item.custom;

                cart.CartDetails.Add(Detail);
                DbContext.SaveChanges();
            }
            cart.TotalItem += Item.qty;
            cart.TotalMoney += info.SalePrice * Item.qty;
            DbContext.SaveChanges();

            string OutData = "{\"totalItem\":" + cart.TotalItem + ",\"cartID\":" + CartID + " }";
            return OutData;
        }

        public string UpdateCart(int CartID, string ProductCode, int Qty, int FK_Custom)
        {
            Cart cart = DbContext.Carts.Find(CartID);
            CartDetail ItemEdit = cart.CartDetails.Where(x => x.ProductID.Equals(ProductCode) && x.FK_CustomID == FK_Custom).FirstOrDefault();
            ItemEdit.Quantity += Qty;
            ItemEdit.Money -= ItemEdit.Quantity * ItemEdit.UnitPrice;

            // += do nếu số lượng truyền vào âm thì trở thành phép trừ
            cart.TotalMoney += Qty * ItemEdit.UnitPrice;
            DbContext.SaveChanges();

            string OutData = "{\"totalMoney\":" + cart.TotalMoney + ", \"money\":" + ItemEdit.Money + " }";
            return OutData;
        }

        public InViewCart GetMyCart(int CartID)
        {
            InViewCart OutData = new InViewCart();

            Cart MyCart = DbContext.Carts.Find(CartID);
            OutData.CartID = MyCart.CartID;
            OutData.UserID = (int)MyCart.FK_UserID;
            OutData.TotalMoney = (decimal)MyCart.TotalMoney;

            List<InViewCart.InViewCartDetail> Details = new List<InViewCart.InViewCartDetail>();

            foreach (CartDetail item in MyCart.CartDetails)
            {
                InViewCart.InViewCartDetail Detail = new InViewCart.InViewCartDetail();
                Detail.productID = item.ProductID;
                Detail.productName = item.ProductName;
                Detail.image = DbContext.Products.Find(item.ProductID).Images.Split(';')[0];
                Detail.price = item.UnitPrice;
                Detail.qty = item.Quantity;
                Detail.Money = item.Money;
                Detail.Size = DbContext.Customs.Find(item.FK_CustomID).Size;
                Detail.Color = DbContext.Customs.Find(item.FK_CustomID).Color;
                Detail.custom = DbContext.Customs.Find(item.FK_CustomID).CustomID;

                Details.Add(Detail);
            }

            OutData.ListDetail = Details;

            return OutData;
        }
    }
}