using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VisitorSystem.Util
{
    public class RequestParameter
    {

        //Dictionary로 굳이쓰는 이유, 파라미터가 들어오지 않아 Null일 경우에 대한 핸들링을 Try Get Value에서 해주기 때문
        //HP에서 웹코딩할때 권고사항으로 이용.

        /// <summary>
        /// Query String에서 Dictionary로 크롤링하는 함수
        /// Get 요청시 데이터 Submit하지 않고 Url로 데이터 받는 경우
        /// </summary>
        /// <param name="request">요청 컨텍스트</param>
        /// <returns></returns>
        public static Dictionary<string, string> QuerystringToDictionary(HttpRequestBase request)
        {
            Dictionary<string, string> parameters = request.QueryString.Keys.Cast<string>()
                .ToDictionary(key => key, value => request.QueryString[value]);

            return parameters;

        }


        /// <summary>
        /// FormCollection으로 Submit한 데이터를 Dictionary로 크롤링
        /// Post 요청에서 사용함
        /// </summary>
        /// <param name="FormCol"></param>
        /// <returns></returns>
        public static Dictionary<string, string> FormCollectionToDictionary(FormCollection FormCol)
        {
            Dictionary<string, string> parameters = FormCol.AllKeys.ToDictionary(k => k, v => FormCol[v]);

            return parameters;

        }

    }
}