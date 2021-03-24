using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using VisitorSystem.Models;
using VisitorSystem.Service;
using VisitorSystem.Util;

namespace VisitorSystem.Controllers
{
    /// <summary>
    /// 김남훈 최초생성
    /// 최초 Home 들어올 경우 Start 해서 정의 함수 Session의 iPaddress 판단하는 구성
    /// </summary>
    public class HomeController : BaseController
    {
        // GET: /Home/Index
        /// <summary>
        /// 최초 웹 인입시 거치는 통로
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            LogUtil.InfoLog("Home Start");

            LogUtil.InfoLog(Session["ipAddress"].ToString());

            HomeService service = new HomeService();
            Location location = service.GetLocationAction(Session["ipAddress"].ToString());

            if (location != null)
            {
                LogUtil.InfoLog("Location ID : " + location.LocationID);
                Session["LocationID"] = location.LocationID;
                Session["LocationName"] = location.LocationName;
            }
            else
            {
                ViewBag.IpAddress = Session["ipAddress"].ToString();
                return View();
            }

            if (location.LocationFlag == 'A')
                return RedirectToAction("Login","Admin");
            else
                return RedirectToAction("Intro","Tablet");
        }


        // GET: /Home/Error
        /// <summary>
        /// 커스텀한 에러 받아주는 View
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Error()
        {
                return View();
        }
    }
}
