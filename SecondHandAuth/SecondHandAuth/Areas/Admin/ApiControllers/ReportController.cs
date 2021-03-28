using Model.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace SecondHandAuth.Areas.Admin.ApiControllers
{
    public class ReportController : ApiController
    {
        ReportBus RpBus = new ReportBus();

        [HttpGet]
        public JsonResult<decimal[]> GetSalesOfYear(int? year)
        {
            return Json(RpBus.GetSalePerYear(year));
        }

        [HttpGet]
        public JsonResult<decimal[]> GetCostsOfYear(int? year)
        {
            return Json(RpBus.GetCostPerYear(year));
        }

        [HttpGet]
        public JsonResult<decimal[]> GetProfitOfYear(int? year)
        {
            return Json(RpBus.GetProfitPerYear(year));
        }

        [HttpGet]
        public JsonResult<string[]> getListLableOfProduct(int? month, int? year)
        {
            return Json(RpBus.getListLableOfProduct(month, year));
        }

        [HttpGet]
        public JsonResult<int[]> GetQtySaleProduct(int? month, int? year)
        {
            return Json(RpBus.GetQtySaleProduct(month, year));
        }
    }
}
