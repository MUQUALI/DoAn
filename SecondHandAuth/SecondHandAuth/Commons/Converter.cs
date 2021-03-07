using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace SecondHandAuth.Commons
{
    public class Converter
    {
        public static DateTime StringToStartDay(string strDate)
        {
            string[] arrDate = strDate.Split('-');
            int day = int.Parse(arrDate[2]);
            int month = int.Parse(arrDate[1]);
            int year = int.Parse(arrDate[0]);
            return new DateTime(year, month, day, 0, 0, 0, 0);
        }

        public static DateTime StringToEndDay(string strDate)
        {
            string[] arrDate = strDate.Split('-');
            int day = int.Parse(arrDate[2]);
            int month = int.Parse(arrDate[1]);
            int year = int.Parse(arrDate[0]);
            return new DateTime(year, month, day, 23, 59, 59, 59);
        }

        public static string PriceToVND(decimal num)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"
            return num.ToString("#,###", cul.NumberFormat) + " VNĐ";
        }
    }
}