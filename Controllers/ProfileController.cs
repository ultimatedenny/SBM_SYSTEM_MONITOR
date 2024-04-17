using SBM_SYSTEM_MONITOR;
using System.Web;
using System;
using System.Web.Mvc;
using SBM_SYSTEM_MONITOR.Classes;

namespace SBM_SYSTEM_MONITOR.Controllers
{
    [JwtAuthentication]
    public class ProfileController : Controller
    {
        readonly MASTERDATA MD = new MASTERDATA();
        public ActionResult Index()
        {
            try
            {
                GetCookies();
                return View();
            }
            catch (Exception)
            {
                return View("Index", "Error");
            }
        }
        public ActionResult Link()
        {
            try
            {
                GetCookies();
                return View();
            }
            catch (Exception)
            {
                return View("Index", "Error");
            }
        }
        public JsonResult GET_MY_LINK()
        {
            try
            {
                return Json(new { data = MD.GET_MY_LINK() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GET_MY_LOG_LINK()
        {
            try
            {
                return Json(new { data = MD.GET_MY_LOG_LINK() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Result = false,
                    StatusCode = ((HttpException)ex).GetHttpCode(),
                    Message = ex.Message.ToString(),
                    Data = "",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public void GetCookies()
        {
            ViewBag.USERID = COOKIES.GetCookies("USERID") ?? "";
            ViewBag.WINDOWSID = COOKIES.GetCookies("WINDOWSID") ?? "";
            ViewBag.NAME = COOKIES.GetCookies("NAME") ?? "";
            ViewBag.EMAIL = COOKIES.GetCookies("EMAIL") ?? "";
            ViewBag.DEPARTMENT = COOKIES.GetCookies("DEPARTMENT") ?? "";
            ViewBag.GROUPS_MDM = COOKIES.GetCookies("GROUPS_MDM") ?? "";
            ViewBag.GROUPS_PBI = COOKIES.GetCookies("GROUPS_PBI") ?? "";
            ViewBag.INITIALS = COOKIES.GetCookies("INITIALS") ?? "";
        }
    }
}