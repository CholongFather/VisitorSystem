using System.Web.Mvc;
using System.Web.Routing;

namespace VisitorSystem
{
    /// <summary>
    /// 프로그램 기본 생성
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// 컨트롤러 , 액션 정의 Route 설정함수
        /// 해당 Route 설정함수는 자동으로 설정해주므로 수동으로 처리하고 싶다면
        /// Interface.config를 이용해서 읽어서 MapRoute를 생성하게 하며 Defaults 옵션으로 처음 IP 치고 들어왔을때 
        /// 리다이렉션될 위치를 지정할 수 있음. 루트 지정에서 파라미터를 필수, 선택으로 나누게 할수도 있음.
        /// 여기서 사용하는 파라미터 필수, 선택은 웹페이지에서 사용하는게 아닌 Openapi에서 자주 이용
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
