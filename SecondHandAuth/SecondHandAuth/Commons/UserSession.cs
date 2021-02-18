using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondHandAuth.Commons
{
    public class UserSession
    {
        public string Username { get; set; }
        public int Permission { get; set; }

        public UserSession(string username, int permission)
        {
            this.Username = username;
            this.Permission = permission;
        }
    }
}