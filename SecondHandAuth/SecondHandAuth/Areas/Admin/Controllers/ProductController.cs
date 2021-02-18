using Model;
using Model.CustomModel;
using Model.Dao;
using PagedList;
using SecondHandAuth.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandAuth.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        ProductDao Dao = new ProductDao();
        // GET: Admin/Product
        [HttpGet]
        public ActionResult Index(int? page)
        {
            if(TempData["MesOk"] != null)
            {
                ViewBag.MesOk = TempData["MesOk"].ToString();
            }
            int PageStart = Commons.Constants.PAGE_INDEX;
            PageStart = page.HasValue ? int.Parse(page.ToString()) : 1;
            IPagedList<Product> Data = Dao.ViewProducts().ToPagedList(PageStart, Commons.Constants.PRODUCT_PAGE_SIZE);
            return View(Data);
        }

        [HttpGet]
        public ActionResult Create()
        {
            dynamic Model = new ExpandoObject();
            Model.ListProductType = Dao.GetListProductType();
            Model.ListFirm = Dao.GetListFirm();

            if (TempData["Mes"] != null)
            {
                ViewBag.Mes = TempData["Mes"].ToString();
            }
            return View(Model);
        }

        [HttpPost]
        public ActionResult Create(ProductFormModel FormModel)
        {
            // đang lỗi trùng tên file
            string RootPath = Path.Combine(Server.MapPath("~/ImagesUpload/"));

            foreach(HttpPostedFileBase file in FormModel.Images)
            {
                if(file != null && file.ContentLength > 0)
                {
                    file.SaveAs(Path.Combine(RootPath, file.FileName));
                }
            }

            Product Model = new Product();
            Model.PK_ProductID = FormModel.Code;
            Model.FK_ProductTypeID = FormModel.FK_ProductTypeID;
            Model.FK_FirmID = FormModel.FK_FirmID;
            Model.Name = FormModel.Name;
            Model.Images = FormModel.StrImages;
            Model.Quantity = 0;
            Model.BuyPrice = FormModel.BuyPrice;
            Model.SalePrice = FormModel.SalePrice;
            Model.Status = FormModel.Status;
            Model.Description = FormModel.Description;
            Model.DelFlg = 0;
            
            if(Dao.Create(Model).Equals("200"))
            {
                TempData["MesOk"] = "Thêm mới thành công";
                return RedirectToAction("Index");
            }
            else
            {
                // chưa xử lý mes lỗi bên form create

                TempData["Mes"] = "Ohh, có thứ gì đó lỗi !";
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        // chưa remove file trong upload
        public ActionResult Edit(ProductFormModel FormModel)
        {
            string RootPath = Path.Combine(Server.MapPath("~/ImagesUpload/"));

            if(FormModel.Images.Length > 0)
            {
                foreach (HttpPostedFileBase file in FormModel.Images)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        file.SaveAs(Path.Combine(RootPath, file.FileName));
                    }
                }
            }
            

            Product Model = new Product();
            Model.PK_ProductID = FormModel.Code;
            Model.FK_ProductTypeID = FormModel.FK_ProductTypeID;
            Model.FK_FirmID = FormModel.FK_FirmID;
            Model.Name = FormModel.Name;
            if(FormModel.Images.Length > 0 && FormModel.Images[0] != null)
            {
                Model.Images = FormModel.StrImages;
            }
            else
            {
                // nếu là rỗng thì sẽ không sửa ảnh
                Model.Images = "";
            }
            Model.BuyPrice = FormModel.BuyPrice;
            Model.SalePrice = FormModel.SalePrice;
            Model.Status = FormModel.Status;
            Model.Description = FormModel.Description;

            if (Dao.Edit(Model).Equals("200"))
            {
                TempData["Mes"] = "Chỉnh sửa thành công";
                return RedirectToAction("Detail/" + Model.PK_ProductID);
            }else
            {
                TempData["Mes"] = "Ohh, có thứ gì đó lỗi !";
                return RedirectToAction("Detail/" + Model.PK_ProductID);
            }
        }

        [HttpPost]
        public ActionResult CreateProductType(ProductType ViewModel)
        {
            int result = Dao.CreateProductType(ViewModel);
            if(result > 0)
            {
                TempData["Mes"] = "Thêm kiểu thành công";
                return RedirectToAction("Create");
            }
            else
            {
                TempData["Mes"] = "Ohh, có thứ gì đó lỗi !";
                return RedirectToAction("Create");
            }
        }

        [HttpPost]
        public ActionResult CreateFirm(Firm ViewModel)
        {
            int result = Dao.CreateFirm(ViewModel);
            if (result > 0)
            {
                TempData["Mes"] = "Thêm hãng thành công";
                return RedirectToAction("Create");
            }
            else
            {
                TempData["Mes"] = "Ohh, có thứ gì đó lỗi !";
                return RedirectToAction("Create");
            }
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            Product Detail = Dao.GetDetail(id);
            List<ProductType> ListProductType = Dao.GetListProductType();
            List<Firm> ListFirm = Dao.GetListFirm();
            List<Inventory> Inventory = Dao.GetInventory(id);

            ViewData["ListProductType"] = ListProductType;
            ViewData["ListFirm"] = ListFirm;
            ViewData["Inventory"] = Inventory;

            // message edit
            if (TempData["Mes"] != null)
            {
                ViewBag.Mes = TempData["Mes"].ToString();
            }

            return View(Detail);
        }
    }
}