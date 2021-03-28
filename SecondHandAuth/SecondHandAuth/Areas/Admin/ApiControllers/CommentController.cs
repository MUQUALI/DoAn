using Model;
using Model.CustomModel;
using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace SecondHandAuth.Areas.Admin.ApiControllers
{
    public class CommentController : ApiController
    {
        CommentDao Dao = new CommentDao();
        [HttpPost]
        public JsonResult<OutComment> SendComment()
        {
            int UserId = int.Parse(HttpContext.Current.Request.Form["userId"].ToString());
            int PostId = int.Parse(HttpContext.Current.Request.Form["postId"].ToString()); ;
            string Content = HttpContext.Current.Request.Form["content"].ToString();
            return Json(Dao.SendComment(UserId, Content, PostId));
        }

        [HttpPost]
        public JsonResult<string> RemoveComment()
        {
            int CmtID = int.Parse(HttpContext.Current.Request.Form["commentId"].ToString());
            return Json(Dao.RemoveComment(CmtID));
        }
    }
}
