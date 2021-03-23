using SecondHandAuth.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandAuth.Areas.Admin.Controllers
{
    [SessionFilter]
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (TempData["UserID"] != null)
            {
                ViewBag.UserID = TempData["UserID"].ToString();
            }
            return View();
        }
    }
}