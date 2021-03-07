using Model;
using Model.CustomModel;
using Model.Dao;
using SecondHandAuth.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace SecondHandAuth.Areas.Admin.ApiControllers
{
    [SessionFilter]
    public class ProductController : ApiController
    {
        ProductDao Dao = new ProductDao();

        [HttpGet]
        public JsonResult<string> DuplicateCode(string code)
        {
            return Json(Dao.DuplicateCode(code));
        }

        [HttpGet]
        public JsonResult<string> GetProductName(string code)
        {
            try
            {
                return Json(Dao.GetDetail(code).Name);
            }
            catch (Exception e)
            {
                return Json("Không tồn tại mã sản phẩm !");
            }
           
        }


        [HttpGet]
        public JsonResult<string> GetPriceOfProduct(string id)
        {
            try
            {

                return Json(Dao.GetDetail(id).SalePrice.ToString());
            }
            catch (Exception e)
            {
                return Json("");
            }

        }
    }
}
