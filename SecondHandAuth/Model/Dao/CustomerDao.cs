using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Dao
{
    public class CustomerDao
    {
        SecondHandDbContext DbContext = null;

        public CustomerDao()
        {
            DbContext = DataProvider.GetInstance();
        }

        public List<Bill> GetMyBills(int UserID)
        {
            Account Context = DbContext.Accounts.Find(UserID);
            int CusID = 0;
            if (Context.FK_CustomerID == null)
            {
                CusID = Context.PK_AccountID;
                return DbContext.Bills.Where(x => x.FK_AccountID == CusID).ToList();
            }
            CusID = (int)Context.FK_CustomerID;
            return DbContext.Bills.Where(x => x.FK_CustomerID == CusID).ToList();
        }
    }
}