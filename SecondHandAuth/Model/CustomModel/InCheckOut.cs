using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.CustomModel
{
    public class InCheckOut
    {
        public string ReceiverName { get; set; }

        public string Phone { get; set; }

        public string ReceiverAddress { get; set; }

        public int CartID { get; set; }
    }
}