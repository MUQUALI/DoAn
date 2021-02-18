using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Dao
{
    public class DataProvider
    {
        private static SecondHandDbContext Instance = null;
        private DataProvider() { }

        public static SecondHandDbContext GetInstance()
        {       
            if(Instance == null)
            {
                Instance = new SecondHandDbContext();
            }
            lock (Instance) 
            return Instance;
        }
    }
}