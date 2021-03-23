using Model;
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
    public class BillController : Controller
    {
        BillDao Dao = new BillDao();
        // GET: Admin/Bill
        public ActionResult Index(int? page, string BillID, string FromDate, string ToDate, int? Status)
        {
            int PageStart = Commons.Constants.PAGE_INDEX;
            PageStart = page.HasValue ? int.Parse(page.ToString()) : 1;

            // convert datetime
            DateTime? dtFormDate = null;
            DateTime? dtToDate = null;
            if (FromDate != null && FromDate != "")
            {
                BillID = null;
                dtFormDate = Converter.StringToStartDay(FromDate);
            }
            if (ToDate != null && ToDate != "")
            {
                BillID = null;
                dtToDate = Converter.StringToEndDay(ToDate);
            }

            ViewBag.BillID = BillID;
            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            ViewBag.Status = Status == null ? "" : Status + "";

            IPagedList<Bill> Data = Dao.GetBills(BillID, dtFormDate, dtToDate, Status).ToPagedList(PageStart, Commons.Constants.PRODUCT_PAGE_SIZE);
            return View("BillHome", Data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (TempData["Mes"] != null)
            {
                ViewBag.Mes = TempData["Mes"].ToString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(InBillCreate Model)
        {
            string ts = JsonConvert.SerializeObject(Model);
            //string bla = Model.ToString();
            List<InBillCreate.ItemDetail> ListDetail = JsonConvert.DeserializeObject<List<InBillCreate.ItemDetail>>(Model.ListDetail);
            UserSession UserLogin = (UserSession)Session[Constants.USER_SESION];
            if (Dao.CreateBill(Model, ListDetail, Commons.Constants.ID_KHACHLE).Equals("200"))
            {
                TempData["Mes"] = "Thanh toán thành công";
                return RedirectToAction("Create");
            }
            TempData["Mes"] = "Thanh toán thất bại";
            return RedirectToAction("Create");
        }

        [HttpGet]
        public ActionResult Detail(int BillID)
        {
            if (TempData["Mes"] != null)
            {
                ViewBag.Mes = TempData["Mes"].ToString();
            }

            Bill OutModel = Dao.GetBillDetail(BillID);
            return View(OutModel);
        }

        [HttpPost]
        public ActionResult GiveBack(int BillID, string Note)
        {
            if (TempData["Mes"] != null)
            {
                ViewBag.Mes = TempData["Mes"].ToString();
            }
            int BillChange = Dao.GiveBack(BillID, Note);
            Bill OutModel = Dao.GetBillDetail(BillChange);
            return View("Detail", OutModel);
        }
    }
}