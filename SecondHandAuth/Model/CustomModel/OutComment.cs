using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.CustomModel
{
    public class OutComment
    {
        public int CommentID { get; set; }
        public int UserID { get; set; }
        public string Author { get; set; }
        public string Time { get; set; }
        public string Content { get; set; }

        public OutComment() { }

        public OutComment(int commentId, int userId, string author, string time, string content)
        {
            this.CommentID = commentId;
            this.UserID = userId;
            this.Author = author;
            this.Time = time;
            this.Content = content;
        }
    }
}