using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondHandAuth.Commons
{
    public class UserSession
    {
        public int PK_AccountID { get; set; }
        public string Username { get; set; }
        public int Permission { get; set; }

        public UserSession()
        {

        }

        public UserSession(string username, int permission, int id)
        {
            this.PK_AccountID = id;
            this.Username = username;
            this.Permission = permission;
        }
    }
}