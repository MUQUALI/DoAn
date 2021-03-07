using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.CustomModel
{
    public class OutViewOrder
    {
        public OutViewOrder(int ID, string Username, string CreatedUser, string Sup, DateTime CreatedDate, decimal Total, string Note)
        {
            OrderID = ID;
            this.Username = Username;
            this.CreatedUser = CreatedUser;
            Supplier = Sup;
            this.CreatedDate = CreatedDate;
            TotalMoney = Total;
            this.Note = Note;
        }

        public int OrderID { get; set; }
        public string Username { get; set; }

        public string CreatedUser { get; set; }
        public string Supplier { get; set; }
        public DateTime CreatedDate { get; set; }
        public string strCreatedDate
        {
            get
            {
                string strTime = CreatedDate.ToString("dd-MM-yyyy hh:mm:ss tt");
                return strTime;
            }
        }
        public decimal TotalMoney { get; set; }
        public string Note { get; set; }
    }
}