using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

        public static string convertToUnSign(string s)
        {
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD));
        }
    }
}