using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VisitorSystem.Util;
using System.Web.Optimization;

/// <summary>
/// 김남훈 최초생성
/// MvcApplication의 최초 세팅을 하는 곳.
/// Global.asax를 거치지 않는 웹 서비스는 없음.
/// </summary>
namespace VisitorSystem
{

    public class MvcApplication : System.Web.HttpApplication
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(typeof(MvcApplication));
        protected void Application_Start()
        {
            //Web Server Application이 시작 될 때

            //Log4net 설정
            log4net.Config.XmlConfigurator.Configure();

            LogUtil.InfoLog("Web System Start");
            AreaRegistration.RegisterAllAreas();


            //필터 추가
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);


            //루트 추가
            RouteConfig.RegisterRoutes(RouteTable.Routes);


            //번들 추가.
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End()
        {
            //Web Server Application이 종료될때 
            LogUtil.InfoLog("Web System End");
        }

        internal protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Request 시작시 생기는 이벤트

        }

        internal protected void Session_OnStart(object sender, EventArgs e)
        {
            //세션이 시작되면 생기는 이벤트
            //모든 Web Request 시작시 해당 이벤트
            HttpContext context = Context;

            HttpResponse response = context.Response;
            HttpRequest request = context.Request;

            //UserAgent로 처리하는것은 이제 불가. Tablet이 PC로 대체됨
            string userAgent = request.Headers.Get("User-agent");

            //IP로 비교해서 Location 읽어서 처리 하려고 함
            Client.GetIpValue(request, out string ipAddress);
            LogUtil.InfoLog("OnSession Event IP : " + ipAddress);
            //세션이 시작되면 생기는 이벤트
            Session["ipAddress"] = ipAddress;
        }

        internal protected void Session_OnEnd(object sender, EventArgs e)
        {
            //세션이 끝나면 생기는 이벤트
        }



        internal protected void Application_EndRequest(object sender, EventArgs e)
        {
            //Request가 끝나면 발생하는 이벤트

        }

        /// <summary>
        /// 웹서버에서 핸들링 되어있지 않은 에러가 도착했을때 최후의 방어선.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = ((MvcApplication)sender).Context;
            var currentController = " ";
            var currentAction = " ";
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }

            var ex = Server.GetLastError();
            var routeData = new RouteData();

            //여기서 뽑히는 Error를 핸들링 하려면 HttpCode별로 에러페이지를 다 구현시켜야된다.
            //보안 취약점으로 꼽히는건 Custom하지 않은 Error 페이지의 내용이라 그냥 Home/Error로 리다이렉트.

            if (ex is HttpException)
            {
                var httpEx = ex as HttpException;

                switch (httpEx.GetHttpCode())
                {
                    default:
                        LogUtil.ErrorLog("Error 발생, Http 코드 :" + httpEx.GetHttpCode());
                        LogUtil.ErrorLog("ex : " + ex.Message);
                        break;

                        // others if any
                }
            }

            

            HttpContext.Current.Server.ClearError();
            //HttpContext.Current.Response.Redirect("~/Home/Error");
        }

    }
}