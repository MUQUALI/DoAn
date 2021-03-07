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
                    UserSession UserInfo = new UserSession(UserLogin.Username, UserLogin.FK_RuleID, UserLogin.PK_AccountID);

                    Session[Constants.USER_SESION] = UserInfo;
                    HttpCookie UserCookie = new HttpCookie("UserID", UserInfo.PK_AccountID.ToString());
                    UserCookie.Expires.AddDays(30);
                    Response.Cookies.Add(UserCookie);

                    if (UserInfo.Permission != Constants.CUSTOMER)
                    {
                        return RedirectToAction("Index", "Home", new { Area = "Admin", UserID = UserInfo.PK_AccountID.ToString() });
                    }
                    else
                    {
                        TempData["UserID"] = UserInfo.PK_AccountID;
                        return RedirectToAction("Index", "Home", new { Area = ""});
                    }
                    
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