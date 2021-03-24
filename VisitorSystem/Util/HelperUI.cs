using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace VisitorSystem.Util
{
    //MvcHtmlString 확장 속성으로 만든 Custom UI
    public static class HelperUI
    {
        /// <summary>
        /// 기존 MVC Helper CheckBox가 Hidden을 생성하기 때문에 Submit 받을때 문제가 생길 여지가 많으므로. Simple과 SimpleFor를 정의하여 쓰게 변경
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxSimple(this HtmlHelper htmlHelper, string name)
        {
            string checkBoxWithHidden = htmlHelper.CheckBox(name).ToHtmlString().Trim();
            string pureCheckBox = checkBoxWithHidden.Substring(0, checkBoxWithHidden.IndexOf("<input", 1));
            return new MvcHtmlString(pureCheckBox);
        }

        public static MvcHtmlString CheckBoxSimple(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        {
            string checkBoxWithHidden = htmlHelper.CheckBox(name, htmlAttributes).ToHtmlString().Trim();
            string pureCheckBox = checkBoxWithHidden.Substring(0, checkBoxWithHidden.IndexOf("<input", 1));
            return new MvcHtmlString(pureCheckBox);
        }

        public static MvcHtmlString CheckBoxSimpleFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression)
        {
            string checkBoxWithHidden = htmlHelper.CheckBoxFor(expression).ToHtmlString().Trim();
            string pureCheckBox = checkBoxWithHidden.Substring(0, checkBoxWithHidden.IndexOf("<input", 1));
            return new MvcHtmlString(pureCheckBox);
        }

        public static MvcHtmlString CheckBoxSimpleFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, object htmlAttributes)
        {
            string checkBoxWithHidden = htmlHelper.CheckBoxFor(expression, htmlAttributes).ToHtmlString().Trim();
            string pureCheckBox = checkBoxWithHidden.Substring(0, checkBoxWithHidden.IndexOf("<input", 1));
            return new MvcHtmlString(pureCheckBox);
        }




    }
}
