using Model;
using Model.CustomModel;
using Model.Dao;
using PagedList;
using SecondHandAuth.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandAuth.Controllers
{
    public class HomeController : Controller
    {
        ProductDao prDao = new ProductDao();
        AccountDao dao = new AccountDao();
        [HttpGet]
        public ActionResult Index(string UserID, string SearchKey = "")
        {
            List<Product> ListProduct = null;
            if (SearchKey != "")
            {
                ListProduct = prDao.SearchUnsign(Converter.convertToUnSign(SearchKey.ToLower()));
            }
            else
            {
                ListProduct = prDao.ViewProducts("");
            }
            List<Firm> ListFirm = prDao.GetListFirm();
            ViewData["ListProduct"] = ListProduct;
            ViewData["ListFirm"] = ListFirm;

            ViewBag.Title = "Trang chủ";
            
            if(TempData["UserID"] != null)
            {
                ViewBag.UserID = TempData["UserID"].ToString();
            }
            if(Session[Constants.USER_SESION] == null)
            {
                // check cookie xem đã đăng nhập chưa
                CheckLogin();
            }
            return View(ViewData);
        }

        public void CheckLogin()
        {
            //string UserID = Request.Cookies["UserID"].ToString();
            if(Request.Cookies["UserID"] != null)
            {
                string UserID = Request.Cookies["UserID"].Value.ToString();
                Account UserLogin = dao.GetUserInfo(int.Parse(UserID));
                UserSession UserInfo = new UserSession(UserLogin.Username, UserLogin.FK_RuleID, UserLogin.PK_AccountID);

                Session[Constants.USER_SESION] = UserInfo;
            }
        }

        public ActionResult ViewDetail(string id)
        {
            Product Item = prDao.GetDetail(id);
            List<Inventory> ListCustomOfItem = prDao.GetInventory(id);
            
            ViewData["Detail"] = Item;
            ViewData["Customs"] = ListCustomOfItem;

            List<Firm> ListFirm = prDao.GetListFirm();
            ViewData["ListFirm"] = ListFirm;
            return View(ViewData);
        }

        [HttpGet]
        public ActionResult Filter(int? page, string menu, int? type = null)
        {
            ViewBag.Title = menu;

            List<Firm> ListFirm = prDao.GetListFirm();
            ViewData["ListFirm"] = ListFirm;

            int PageStart = Commons.Constants.PAGE_INDEX;
            PageStart = page.HasValue ? int.Parse(page.ToString()) : 1;


            IPagedList<Product> Data = prDao.GetListOfMenu(menu, type).ToPagedList(PageStart, 16);
            List<OutSubMenu> ListType = prDao.GetListType(menu);
            ViewBag.Menu = menu;

            ViewData["Data"] = Data;
            ViewData["ListType"] = ListType;
            return View(ViewData);
        }
    }
}