using Model.Bus;
using Model.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Dao
{
    public class CartDao
    {
        CartBus Bus = null;
        public CartDao()
        {
            Bus = new CartBus();
        }
        public string AddToCart(InViewCart Model, int? UserID)
        {
            return Bus.AddToCart(Model, UserID);
        }

        public string EditCart(int CartID, InViewCart.InViewCartDetail Item)
        {
            return Bus.EditCart(CartID, Item);
        }

        public InViewCart GetMyCart(int CartID)
        {
            return Bus.GetMyCart(CartID);
        }

        public string UpdateCart(int CartID, string ProductCode, int Qty, int FK_Custom)
        {
            return Bus.UpdateCart(CartID, ProductCode, Qty, FK_Custom);
        }
    }
}