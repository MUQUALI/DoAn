using Model;
using Model.Dao;
using SecondHandAuth.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandAuth.Controllers
{
    public class CustomerController : Controller
    {
        CustomerDao Dao = new CustomerDao();
        ProductDao prDao = new ProductDao();
        // GET: Customer
        public ActionResult Index()
        {
            UserSession UserInfo = (UserSession)Session[Constants.USER_SESION];

            List<Bill> Model = Dao.GetMyBills(UserInfo.PK_AccountID).OrderByDescending(x => x.CreatedDate).ToList();

            List<Firm> ListFirm = prDao.GetListFirm();
            ViewData["ListFirm"] = ListFirm;
            return View(Model);
        }
    }
}