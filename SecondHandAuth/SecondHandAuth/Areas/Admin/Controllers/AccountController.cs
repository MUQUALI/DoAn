using Model.Dao;
using SecondHandAuth.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandAuth.Areas.Admin.Controllers
{
    [SessionFilter]
    public class AccountController : Controller
    {
        
        // GET: Admin/Account
        [HttpGet]
        public ActionResult Index()
        {
            UserSession UserAccess = (UserSession)Session[Constants.USER_SESION];
            if (UserAccess.Permission.Equals(Constants.SUPPER_ADMIN))
            {
                return View();
            }
            return View("NoPermission");
        }
        
        
    }
}