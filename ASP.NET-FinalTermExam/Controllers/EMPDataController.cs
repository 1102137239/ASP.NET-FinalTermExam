using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP.NET_FinalTermExam.Models;
namespace ASP.NET_FinalTermExam.Controllers
{
    public class EMPDataController : Controller
    {
        Models.CodeService codeService = new Models.CodeService();
        private Models.EMP EMP;

        public ActionResult Index()
        {
            ViewBag.Tit = codeService.GetTitle();
            ViewBag.City = codeService.GetCity();
            ViewBag.Country = codeService.GetCountry();
            ViewBag.Gender = codeService.GetGender();
            
            return View();
        }
        /// <summary>
        /// 取得員工查詢結果
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult Index(Models.EMPSearchArg arg)
        {
            ViewBag.Tit = codeService.GetTitle();
            ViewBag.City = codeService.GetCity();
            ViewBag.Country = codeService.GetCountry();
            ViewBag.Gender = codeService.GetGender();
            Models.EMPService EMPService = new Models.EMPService();
            ViewBag.SearchResult = EMPService.GetEMPByCondtioin(arg);
            return View("Index");
        }

        /// <summary>
        /// 刪除員工
        /// </summary>
        /// <param name="EMPId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteEMP(string EMPId)
        {
            try
            {
                Models.EMPService EMPService = new Models.EMPService();
                EMPService.DeleteEMPById(EMPId);
                return this.Json(true);
            }
            catch (Exception)
            {
                return this.Json(false);
            }
        }

        /// <summary>
        /// 取得系統時間
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSysDate()
        {
            return PartialView("_SysDatePartial");
        }
    }
}