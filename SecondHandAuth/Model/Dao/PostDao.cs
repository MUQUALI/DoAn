using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Dao
{
    public class PostDao
    {
        SecondHandDbContext DbContext = null;
        public PostDao()
        {
            DbContext = DataProvider.GetInstance();
        }

        public List<Post> GetListPost()
        {
            return DbContext.Posts.ToList();
        }
    }
}