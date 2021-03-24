using System.Web.Optimization;

namespace VisitorSystem
{
    /// <summary>
    /// 김남훈 최초생성
    /// StyleSheet와 Script 구성하는 번들 설정
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// 번들관리하는 상위 함수 
        /// MVC 4.0 에서 번들 관리하는 게 추가되었고.
        /// 한번 해보고 싶어서 만듬.
        /// 어느정도의 속도 향상에 도움이 된다고 하며 암호화 하여 관리도 가능하다 한다.
        /// </summary>
        /// <param name="bundles">번들 컬렉션</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScript(bundles);

            RegisterCss(bundles);

            RegisterSignPad(bundles);

            RegisterVaildate(bundles);

            RegisterBootStrap(bundles);

            RegisterPaging(bundles);
        }

        /// <summary>
        /// 스크립트 번들 등록
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterScript(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Script/Js").Include(
                "~/Scripts/jquery-3.3.1.js"));

            bundles.Add(new ScriptBundle("~/Script/JsMigrate").Include(
                "~/Scripts/jquery-migrate-1.2.0"));
        }


        /// <summary>
        /// 스타일 시트 번들 등록
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterCss(BundleCollection bundles)
        {
            //관리자 설정 Css 구성
            bundles.Add(new StyleBundle("~/Content/Css").Include(
                "~/Content/Css/common.css"));

            bundles.Add(new StyleBundle("~/Content/Font").Include(
                "~/Content/Css/font.css"));

            bundles.Add(new StyleBundle("~/Content/Layout").Include(
                "~/Content/Css/layout.css"));

            bundles.Add(new StyleBundle("~/Content/Layout2").Include(
                "~/Content/Css/layout2.css"));

            //이 밑의 영역은 타블렛 설정 Css 구성
            bundles.Add(new StyleBundle("~/Content/TabletCss").Include(
                "~/Content/Css/Tablet_common.css"));

            bundles.Add(new StyleBundle("~/Content/TabletFont").Include(
                "~/Content/Css/Tablet_font.css"));

            bundles.Add(new StyleBundle("~/Content/TabletLayout").Include(
                "~/Content/Css/Tablet_layout.css"));
        }

        /// <summary>
        /// 사인패드 관련 스타일, 스크립트 번들 등록
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterSignPad(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/SignPad/Numeric").Include(
                "~/Content/assets/numeric-1.2.6.js"));

            bundles.Add(new ScriptBundle("~/SignPad/Bezier").Include(
                "~/Content/assets/bezier.js"));

            bundles.Add(new ScriptBundle("~/SignPad/Jq").Include(
                "~/Content/Js/jquery.signaturepad.js"));

            bundles.Add(new ScriptBundle("~/SignPad/Json").Include(
                "~/Content/assets/json2.js"));

            bundles.Add(new StyleBundle("~/SignPad/Css").Include(
                "~/Content/assets/jquery.signaturepad.css"));


        }

        /// <summary>
        /// Jquery 유효성 관련 체크 
        /// Form Submit 할때 사용하는 Validate Jquery 폼들이므로 유효성 검증할때 해당 번들 참조.
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterVaildate(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Script/Validate").Include(
                "~/Scripts/jquery.validate.js"));

            bundles.Add(new ScriptBundle("~/Script/Validate/Unobtrusive").Include(
                "~/Scripts/jquery.validate.unobtrusive.js"));

        }

        /// <summary>
        /// BootStrap UI (반응성 웹 기본틀)
        /// UI 관련 BootStrap 참조
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterBootStrap(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Css/BootStrap").Include(
                "~/Content/Css/BootStrap-3.3.7.css"));

            bundles.Add(new ScriptBundle("~/JS/BootStrap").Include(
                "~/Content/Js/BootStrap-3.3.7.js"));
        }

        /// <summary>
        /// Jquery UI Paging Plugin
        /// 페이징 관련 UI 참조
        /// </summary>
        /// <param name="bundles"></param>
        private static void RegisterPaging(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Js/Jquery/Pagination").Include(
                "~/Content/Js/Jquery-Pagination-1.4.2.js"));
        }
    }
}


