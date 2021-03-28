using Model.CustomModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Dao
{
    public class CommentDao
    {
        SecondHandDbContext DbContext = null;

        public CommentDao()
        {
            DbContext = DataProvider.GetInstance();
        }

        public OutComment SendComment(int UserID, string content, int PostID)
        {
            try
            {
                Comment Cmt = new Comment();
                Cmt.FK_AccountID = UserID;
                Cmt.FK_PostID = PostID;

                Account UserSend = DbContext.Accounts.Find(UserID);
                Cmt.Author = UserSend.Customer != null ? UserSend.Customer.Name : UserSend.Employee.Name;
                Cmt.Contents = content;
                Cmt.CreatedDate = DateTime.Now;
                Cmt.DelFlg = 0;

                DbContext.Comments.Add(Cmt);
                DbContext.SaveChanges();
                return new OutComment(Cmt.PK_CommentID, Cmt.FK_AccountID, Cmt.Author, Cmt.CreatedDate.ToString("dd/MM/yyyy hh:mm:ss tt"), Cmt.Contents);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public List<Comment> GetListComment(int PostID)
        {
            return DbContext.Comments.Where(x => x.FK_PostID == PostID && x.DelFlg == 0).ToList();
        }

        public string RemoveComment(int CmtID)
        {
            try
            {
                Comment Cmt = DbContext.Comments.Find(CmtID);
                Cmt.DelFlg = 1;
                DbContext.SaveChanges();
                return "200";
            }
            catch(Exception e)
            {
                return "";
            }
        }
    }
}