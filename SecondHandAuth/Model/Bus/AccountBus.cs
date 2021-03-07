using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Bus
{
    public class AccountBus
    {
        SecondHandDbContext DbContext = null;
        public AccountBus()
        {
            DbContext = DataProvider.GetInstance();
        }

        public int GetTotalRecord(string key = "")
        {
            if(key != "" && key != null)
            {
                return DbContext.Accounts.Count(x => x.FK_RuleID != 1 && x.Username.Contains(key));
            }
            return DbContext.Accounts.Count(x =>x.FK_RuleID != 1);
        }
        public IEnumerable<object> ReadPagegination(int page, int limit, string key = "")
        {
            var query = from account in DbContext.Accounts
                        where account.FK_RuleID != 1
                        orderby account.PK_AccountID descending
                        select new 
                        {
                            PK_AccountID = account.PK_AccountID,
                            Username = account.Username,
                            Description = account.Description,
                            FK_RuleID = account.FK_RuleID,
                            FK_CustomerID = account.FK_CustomerID,
                            FK_EmpID = account.FK_EmpID
                        };
            IEnumerable<object> myData = null;
            if(!String.IsNullOrEmpty(key.Trim()))
            {
                myData = query.Skip((page - 1) * limit).Take(limit).Where(x => x.Username.Contains(key)).ToList();
            }
            else
            {
                myData = query.Skip((page - 1) * limit).Take(limit).ToList();
            }
            return myData;
        }

        public int Login(string username, string password)
        {
            return DbContext.Accounts
                .Count(x => x.Username.Equals(username) && x.Password.Equals(password));
        }

        public Account GetUserInfo(string username, string password)
        {
            return DbContext.Accounts
                .Where(x => x.Username.Equals(username) && x.Password.Equals(password)).FirstOrDefault();
        }

        public Account GetUserInfo(int? UserId)
        {
            return DbContext.Accounts.Find(UserId);
        }

        public string Create(Account Entity)
        {
            if(DbContext.Accounts.Count(x => x.Username.Equals(Entity.Username)) > 0)
            {
                return "300";
            }
            try
            {
                DbContext.Accounts.Add(Entity);
                DbContext.SaveChanges();
                return "200";
            }catch(Exception ex)
            {
                return "400 - " + ex.Message;
            }
            
        }

        public string Edit(Account Entity)
        {
            try
            {
                Account UserEdit = DbContext.Accounts.Find(Entity.PK_AccountID);
                UserEdit.Description = Entity.Description;
                UserEdit.FK_RuleID = Entity.FK_RuleID;
                if(Entity.FK_EmpID != null )
                {
                    if(CheckExitsEmp(Entity.FK_EmpID))
                    {
                        UserEdit.FK_EmpID = Entity.FK_EmpID;
                    }
                    else
                    {
                        return "201";
                    }
                }
                if(Entity.Password != null)
                {
                    UserEdit.Password = Entity.Password;
                }
                DbContext.SaveChanges();
                return "202";
            }catch(Exception e)
            {
                return "400 - " + e.Message;
            }
        }

        public string Delete(int Id)
        {
            try
            {
                Account RemoveEntity = DbContext.Accounts.Find(Id);
                DbContext.Accounts.Remove(RemoveEntity);
                DbContext.SaveChanges();
                return "200";
            }catch(Exception e)
            {
                return "400 - " + e.Message;
            }
        }

        public bool CheckExitsEmp(int? EmpID)
        {
            if(DbContext.Accounts.Find(EmpID) != null)
            {
                return true;
            }
            return false;
        }
    }
}