using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecondHandAuth.Commons
{
    public class Constants
    {
        // CHỦ CỬA HÀNG
        public static int SUPPER_ADMIN = 1;
        // PHÓ CỬA HÀNG
        public static int LESS_ADMIN = 2;
        // NHÂN VIÊN
        public static int EMPLOYEE = 3;
        // KHÁCH HÀNG
        public static int CUSTOMER = 4;

        //Sesion
        public static string USER_SESION = "USER_SESION";

        // Page index 
        public static int PAGE_INDEX = 1;

        // product page size
        public static int PRODUCT_PAGE_SIZE = 5;

        public static int STATUS_0 = 0;

        public static int STATUS_1 = 1;

        public static int STATUS_2 = 2;

        public static int STATUS_3 = 3;

        public static int STATUS_4 = 4;

        public static string STR_STATUS_0 = "Chờ xác nhận";

        public static string STR_STATUS_1 = "Đã xác nhận";

        public static string STR_STATUS_2 = "Đang giao";

        public static string STR_STATUS_3 = "Đã giao hàng";

        public static string STR_STATUS_4 = "Hủy";

        public static int ID_KHACHLE = 3;
    }
}