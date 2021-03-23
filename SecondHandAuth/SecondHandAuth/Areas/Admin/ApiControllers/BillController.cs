using Model.CustomModel;
using Model.Dao;
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
    public class BillController : ApiController
    {
        BillDao Dao = new BillDao();
        ProductDao PDao= new ProductDao();

        [HttpPost]
        public JsonResult<string> ChangeStatusBill()
        {
            int bid = int.Parse(HttpContext.Current.Request.Form["bid"].ToString());

            int status = int.Parse(HttpContext.Current.Request.Form["status"].ToString());

            int UserID = int.Parse(HttpContext.Current.Request.Form["userId"].ToString());

            return Json(Dao.ChangeStatusBill(bid, status, UserID));
        }

        [HttpGet]
        public JsonResult<OutBillDetail> GetProductInfo(string code)
        {
            return Json(Dao.GetProductInfo(code));
        }

        [HttpGet] 
        public JsonResult<OutBillDetail> AddInfoProduct(string code, int custom, int qty)
        {
            return Json(Dao.AddInfoProduct(code, custom, qty));
        }

        [HttpGet]
        public JsonResult<string> GetMaxQtyOfProduct(string code, int custom)
        {
            List<Inventory> Invens = PDao.GetInventory(code);

            Inventory Inventory = Invens.Where(x => x.FK_CustomID == custom).FirstOrDefault();

            string result = Inventory.Quantity.ToString();
            return Json(result);
        }

    }

}
