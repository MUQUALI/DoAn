using Model;
using Model.CustomModel;
using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandAuth.Controllers
{
    public class CartController : Controller
    {
        ProductDao prDao = new ProductDao();
        CartDao Dao = new CartDao();
        // GET: Cart
        [HttpGet]
        public ActionResult Index()
        {
            if(Session[Commons.Constants.USER_SESION] == null)
            {
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }
            List<Firm> ListFirm = prDao.GetListFirm();
            ViewData["ListFirm"] = ListFirm;
            return View();
        }

        [HttpPost]
        public ActionResult CheckOut(InCheckOut Model)
        {
            Bill Result = Dao.CheckOut(Model);
            return RedirectToAction("Index", "Customer");
        }
    }
}