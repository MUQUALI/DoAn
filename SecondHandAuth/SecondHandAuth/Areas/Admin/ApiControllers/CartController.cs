using Model;
using Model.CustomModel;
using Model.Dao;
using Newtonsoft.Json;
using SecondHandAuth.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace SecondHandAuth.Areas.Admin.ApiControllers
{
    public class CartController : ApiController
    {
        CartDao Dao = new CartDao();
        AccountDao AcDao = new AccountDao();
        [HttpGet]
        public JsonResult<UserSession> CheckLogin(int? UserId)
        {
            if(UserId != null)
            {
                Account find = AcDao.GetUserInfo(UserId);
                UserSession UserSes = new UserSession();
                UserSes.Username = find.Username;
                return Json(UserSes);
            }
            
            return Json(new UserSession());
        }

        [HttpPost]
        public JsonResult<string> AddToCart(object Model)
        {
            InViewCart InData = JsonConvert.DeserializeObject<InViewCart>(Model.ToString());

            return Json(Dao.AddToCart(InData, InData.UserID));
        }

        [HttpPost]
        public JsonResult<string> EditCart(object Model)
        {
            InViewCart InData = JsonConvert.DeserializeObject<InViewCart>(Model.ToString());

            return Json(Dao.EditCart(InData.CartID, InData.ItemAddToCart));
        }

        [HttpGet]
        public JsonResult<InViewCart> GetMyCart(int CartID)
        {
            return Json(Dao.GetMyCart(CartID));
        }

        [HttpPost]
        public JsonResult<string> UpdateCart()
        {
            int CartID = int.Parse(HttpContext.Current.Request.Form["cartID"].ToString());
            int Qty = int.Parse(HttpContext.Current.Request.Form["qty"].ToString()); ;
            string ProductID = HttpContext.Current.Request.Form["code"].ToString();
            int FK_Custom = int.Parse(HttpContext.Current.Request.Form["custom"].ToString());

            return Json(Dao.UpdateCart(CartID, ProductID, Qty, FK_Custom));
        }
    }
}
