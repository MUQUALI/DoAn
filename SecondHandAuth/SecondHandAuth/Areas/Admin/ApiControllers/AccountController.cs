using Model;
using Model.Dao;
using Newtonsoft.Json;
using SecondHandAuth.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace SecondHandAuth.Areas.Admin.ApiControllers
{
    [SessionFilter]
    public class AccountController : ApiController
    {
        public static string ACCOUNT_CACHE = "";
        AccountDao Dao = new AccountDao();
        [HttpPost]
        public JsonResult<string> Create(Model.Account Model)
        {
            return Json(Dao.Create(Model));
        }

        [HttpPost]
        public JsonResult<string> Edit(Model.Account Model)
        {
            return Json(Dao.Edit(Model));
        }

        [HttpPost]
        public JsonResult<string> Delete(object ID)
        {
            JsonModel data = JsonConvert.DeserializeObject<JsonModel>(ID.ToString());
            return Json(Dao.Delete(int.Parse(data.ID)));
        }


        [HttpGet]
        public JsonResult<int> GetTotalRecord(string key)
        {
            return Json(Dao.GetTotalRecord(key));
        }

        [HttpGet]
        public JsonResult<IEnumerable<object>> ReadPagination(int page, int limit, string key = "")
        {
            try

            {
                if(key == "" && ACCOUNT_CACHE != "" && key != null)
                {
                    return Json(Dao.ReadPagination(page, limit, ACCOUNT_CACHE));
                }
                if(key != "" && key != null)
                {
                    ACCOUNT_CACHE = key;
                    return Json(Dao.ReadPagination(page, limit, ACCOUNT_CACHE));
                }
                ACCOUNT_CACHE = "";
                return Json(Dao.ReadPagination(page, limit, ACCOUNT_CACHE));


            }
            catch (Exception e)
            {
                string mes = e.Message;
                return Json(new List<object>().AsEnumerable());
            }
            
        }

        private class JsonModel
        {
            public string ID { get; set; }
        }
        
    }
}
