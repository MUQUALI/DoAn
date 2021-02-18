using Model;
using Model.Dao;
using SecondHandAuth.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandAuth.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        AccountDao dao = new AccountDao();
        // GET: Admin/Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Models.LoginModel model)
        {
            if(ModelState.IsValid)
            {
                string username = model.Username;
                string password = model.Password;
                bool RememberMe = model.RememberMe;
                if(dao.Login(username, password) > 0)
                {
                    Account UserLogin = dao.GetUserInfo(username, password);
                    UserSession UserInfo = new UserSession(UserLogin.Username, UserLogin.FK_RuleID);

                    Session[Constants.USER_SESION] = UserInfo;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng");
                }
            }
            return View(model);
        }
    }
}