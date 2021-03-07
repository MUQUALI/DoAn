using Model.CustomModel;
using Model.Dao;
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
    [SessionFilter]
    public class OrderController : ApiController
    {
        OrderDao Dao = new OrderDao();

        [HttpPost]
        public JsonResult<ViewOrderDetail> GetDetail()
        {
            int size = int.Parse(HttpContext.Current.Request.Form["size"].ToString());
            string color = HttpContext.Current.Request.Form["color"].ToString();
            string code = HttpContext.Current.Request.Form["code"].ToString();

            Dao.CheckDuplicateCustom(size, color);

            return Json(Dao.GetDetail(code));
        }

        [HttpGet]
        public JsonResult<List<OutViewOrderDetail>> GetDetaislFromID(string ID)
        {
            return Json(Dao.GetDetaislFromID(ID));
        }
    }
}
