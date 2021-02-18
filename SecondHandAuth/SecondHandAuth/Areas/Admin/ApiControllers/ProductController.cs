using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace SecondHandAuth.Areas.Admin.ApiControllers
{
    public class ProductController : ApiController
    {
        ProductDao Dao = new ProductDao();

        [HttpGet]
        public JsonResult<string> DuplicateCode(string code)
        {
            return Json(Dao.DuplicateCode(code));
        }

    }
}
