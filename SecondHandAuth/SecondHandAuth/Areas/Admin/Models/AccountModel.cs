using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondHandAuth.Areas.Admin.Models
{
    [Serializable]
    public class AccountModel
    {
        public string Username;
        public string Password;
        public string Description;
        public int FK_RuleID;
        public int? FK_EmpID;
        public int? FK_CustomerID;
    }
}