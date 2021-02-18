using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SecondHandAuth.Areas.Admin.Models
{
    [Serializable]
    public class LoginModel
    {
        [Required(ErrorMessage = "Hãy nhập Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Hãy nhập Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}