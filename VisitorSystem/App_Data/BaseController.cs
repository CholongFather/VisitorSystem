using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using VisitorSystem.Util;

namespace VisitorSystem.Controllers
{

    /// <summary>
    /// 김남훈 최초생성 
    /// Controller Base Override
    /// </summary>
    public class BaseController : Controller
    {


        //Framework화 해야지만 귀찮아서 웹에 녹임.
        //HttpVerb가 Put과 Delete일 경우. 해당 Verb로 완벽히 매칭했다고 해도 Post로 다시 리다이렉트 해서 사용하는게 좋다.
        //Client에서 Put, Delete가 차단될수 있음
        //모든 컨트롤러 해당 컨트롤러 상속으로 가져가게 변경할것
        public BaseController()
        {

        }
       
        //인증단 구성시 사용되는 Initialize 컨텍스트
        //OpenApi에서 사용하여 Tokenize 된 값을 참조해서 리퀘스트 실행을 막을 수 있음.
        protected override void Initialize(RequestContext requestContext)
        {
            //RawUrl을 뽑아서 보안 위협이 있는 리퀘스트 Url이 있을 경우 Initialize를 하지 않게 변경
            //여기서 처리하지 않고 에러를 발생시켜서 OnException으로 Handle해서 처리함.
            //string RawUrl = requestContext.HttpContext.Request.RawUrl;

            base.Initialize(requestContext);
        }

        //웹에서 Auth 구성시 사용할 이벤트 (Form Auth로 처리해서 인증 용도로 사용하지 않음)
        protected override void Execute(System.Web.Routing.RequestContext requestContext)
        {
            base.Execute(requestContext);
        }


        /// <summary> 작업 메서드가 호출되기 전에 호출됨
        /// </summary>
        /// <param name="filterContext">현재 요청 및 작업에 대한 정보</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controller = filterContext.RequestContext.RouteData.Values["controller"].ToString().ToLower();
            string action = filterContext.RequestContext.RouteData.Values["action"].ToString().ToLower();

            //action에 Login이 아닌데 Session 없는 경우 로그인창으로 Redirect
            if(controller == "admin")
            {
                if(action != "login")
                {
                    if(filterContext.RequestContext.HttpContext.Session["UserID"] == null)
                    {
                        //Login으로 Redirect
                        filterContext.Result = new RedirectResult("~/Admin/Login");
                        return;

                    }

                }

            }

            //이 메서드가 실행되면 Action으로 넘어감.
            base.OnActionExecuting(filterContext);
        }

        /// <summary> 작업메서드가 호출된후 호출됨
        /// </summary>
        /// <param name="filterContext">현재 요청 및 작업에 대한 정보</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }


        /// <summary> 작업 메서드에서 반환되는 작업 결과가 실행된 후에 호출됨
        /// </summary>
        /// <param name="filterContext">현재 요청 및 작업 결과에 대한 정보.</param>
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }


        /// <summary> 작업 메서드에서 반환되는 작업 결과가 실행되기 전에 호출됨.
        /// </summary>
        /// <param name="filterContext">현재 요청 및 작업 결과에 대한 정보.</param>
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }


        /// <summary> 작업에서 처리되지 않은 예외가 발생할 때 호출됨
        /// 모든 웹페이지 에러는 다음과 같이 잡히므로 커스텀한 Error 페이지를 만들어서 보안 취약점을 해결할 수 있음.
        /// 404 Not Found는 잡히지 않음. 밑에 있는 HandleUnknown에서 잡히게됨
        /// </summary>
        /// <param name="filterContext">현재 요청 및 작업에 대한 정보</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            string errorMessage = "\r\n\t RawUrl[" + filterContext.HttpContext.Request.RawUrl + "]" +
                       "\r\n\t UrlReferrer[" + filterContext.HttpContext.Request.UrlReferrer + "]";

            string actionName = (string)filterContext.RouteData.Values["action"];
            string controllerName = (string)filterContext.RouteData.Values["controller"];


            base.OnException(filterContext);
            var ex = filterContext.Exception;

            LogUtil.ErrorLog(errorMessage + ex.ToString());

            //에러 정보를 담아서 던져줄까 싶어서 만듬.
            filterContext.Controller.TempData["exception"] = ex;

            //보낼 뷰가 확실해지면 해당 주석 풀고 View 넣어서 리턴 밑에 내용으로 대체함
            //this.View("Intro").ExecuteResult(this.ControllerContext);

            //밑의 HandleErrorInfo로 Custom한 Error 페이지로 Redirect
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
            
            //Error FilterContext에 결과 담아서 리턴하게 변경
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };
            //이렇게 꾸미면 @Model.Exception에서 에러 내용을 담아 보낼수 있음.

            //이제 내가 Exception을 Handling 했기때문에 Response를 Clear시키고 커스텀 Error 위치로 날림
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            
        }


        /// <summary> 요청이 이 컨트롤러와 일치하지만 지정된 작업 이름을 포함하는 메서드를 컨트롤러에서 찾을 수 없을 때 호출됨
        /// Error 페이지로 날려보내면서 Controller와 Error 담아서 던짐.
        /// </summary>
        /// <param name="actionName">시도된 작업의 이름입니다.</param>
        protected override void HandleUnknownAction(string actionName)
        {
            //위치도 모르면서 들어오는 주소창 입력일 경우 Action일 경우에 없는 주소를 호출하였기 때문에 Error로 핸들링 하는게 좋을듯.
            LogUtil.ErrorLog("HandleUnknownAction : acionName[" + actionName + "]");

            var model = new HandleErrorInfo(new Exception("왜 없는 URL을 호출하십니까?"), (string)ControllerContext.RouteData.Values["controller"], actionName);
            var result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
            };
            Response.StatusCode = 404;

            //Admin에서 파티셜로 뷰 그린 거가 호출되는데 어떻게 할지 정해야함.
            result.ExecuteResult(ControllerContext);

        }
    }
}





