using Model.CustomModel;
using Model.Dao;
using Newtonsoft.Json;
using PagedList;
using SecondHandAuth.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandAuth.Areas.Admin.Controllers
{
    [SessionFilter]
    public class OrderController : Controller
    {
        OrderDao Dao = new OrderDao();
        // GET: Admin/Order
        [HttpGet]
        public ActionResult Index(int? page, string OrderID, string FromDate, string ToDate)
        {
            int PageStart = Commons.Constants.PAGE_INDEX;
            PageStart = page.HasValue ? int.Parse(page.ToString()) : 1;

            // convert datetime
            DateTime? dtFormDate = null;
            DateTime? dtToDate = null;
            if (FromDate != null && FromDate != "")
            {
                OrderID = null;
                dtFormDate = Converter.StringToStartDay(FromDate);
            }
            if (ToDate != null && ToDate != "")
            {
                OrderID = null;
                dtToDate = Converter.StringToEndDay(ToDate);
            }

            ViewBag.OrderID = OrderID;
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            IPagedList<OutViewOrder> Data = Dao.Search(OrderID, dtFormDate, dtToDate).ToPagedList(PageStart, Commons.Constants.PRODUCT_PAGE_SIZE);

            return View(Data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if(TempData["Mes"] != null)
            {
                ViewBag.Mes = TempData["Mes"].ToString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(ViewOrder Model)
        {
            string ts = JsonConvert.SerializeObject(Model);
            //string bla = Model.ToString();
            List<ViewOrder.ItemDetail> ListDetail = JsonConvert.DeserializeObject<List<ViewOrder.ItemDetail>>(Model.ListDetail);
            UserSession UserLogin = (UserSession)Session[Constants.USER_SESION];
            if(Dao.CreateOrderRecord(Model, UserLogin.PK_AccountID, ListDetail) > 0)
            {
                TempData["Mes"] = "Tạo đơn đặt hàng thành công";
                return RedirectToAction("Create");
            }
            return RedirectToAction("Create");
        }
    }
}