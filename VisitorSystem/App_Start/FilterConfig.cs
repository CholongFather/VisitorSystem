using System.Web.Mvc;

namespace VisitorSystem
{
    /// <summary>
    /// 김남훈 최초생성
    /// Route 필터 구성 설정
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// 글로벌 필터 옵션
        /// </summary>
        /// <param name="filters">필터 컬렉션</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //에러 처리 관련 속성 Add
            filters.Add(new HandleErrorAttribute());
            //Form 인증 관련 속성 Add
            // Authorize, AllowAnonymous 처리 담당
            filters.Add(new AuthorizeAttribute());
        }
    }
}
