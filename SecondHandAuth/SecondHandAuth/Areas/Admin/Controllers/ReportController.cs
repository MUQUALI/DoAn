using Model.Bus;
using Model.CustomModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandAuth.Areas.Admin.Controllers
{
    public class ReportController : Controller
    {
        ReportBus Bus = new ReportBus();
        // GET: Admin/Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetReportInventory(int? page)
        {
            int PageStart = Commons.Constants.PAGE_INDEX;
            PageStart = page.HasValue ? int.Parse(page.ToString()) : 1;

            IPagedList<OutReportIvt> Data = Bus.GetListReportInventory().ToPagedList(PageStart, Commons.Constants.REPORT_PAGE_SIZE);
            return View(Data);
        }
    }
}