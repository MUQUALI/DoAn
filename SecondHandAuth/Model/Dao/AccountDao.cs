using Model.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Dao
{
    public class AccountDao
    {
        AccountBus Bus = null;
        SecondHandDbContext DbContext = null;
        public AccountDao()
        {
            Bus = new AccountBus();
            DbContext = DataProvider.GetInstance();
        }
        public int GetTotalRecord(string key)
        {
            return Bus.GetTotalRecord(key);
        }
        public IEnumerable<object> ReadPagination(int page, int limit, string key)
        {
            return Bus.ReadPagegination(page, limit, key);
        }

        public int Login(string username, string password)
        {
            return Bus.Login(username, password); 
        }

        public Account GetUserInfo(string username, string password)
        {
            return Bus.GetUserInfo(username, password);
        }

        public Account GetUserInfo(int? UserID)
        {
            return Bus.GetUserInfo(UserID);
        }

        public string Create(Account Entity)
        {
            return Bus.Create(Entity);
        }

        public string Edit(Account Entity)
        {
            return Bus.Edit(Entity);
        }

        public string Delete(int Id)
        {
            return Bus.Delete(Id);
        }

        public int OnComment(string ProductID, int UserID)
        {
            int OnComment = 0;
            Account AccInfo = DbContext.Accounts.Find(UserID);
            List<Bill> YourBill = DbContext.Bills.Where(x => (x.FK_AccountID == UserID || x.FK_CustomerID == AccInfo.FK_CustomerID)).ToList();
            foreach (Bill item in YourBill)
            {
                if(item.BillDetails.Where(x => x.ProductID.Equals(ProductID)).Count() > 0)
                {
                    OnComment++;
                }
            }
            return OnComment;
        }
    }
}