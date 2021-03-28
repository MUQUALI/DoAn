using Model;
using Model.Dao;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SecondHandAuth.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        PostDao Dao = new PostDao();
        // GET: Admin/Post
        public ActionResult Index(int? page)
        {
            int PageStart = Commons.Constants.PAGE_INDEX;
            PageStart = page.HasValue ? int.Parse(page.ToString()) : 1;

            IPagedList<Post> Data = Dao.GetListPost().ToPagedList(PageStart, Commons.Constants.PRODUCT_PAGE_SIZE);
            return View(Data);
        }
    }
}