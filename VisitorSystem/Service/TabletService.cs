using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VisitorSystem.Models;
using VisitorSystem.Dao;
using VisitorSystem.Util;
using System.Text.RegularExpressions;

namespace VisitorSystem.Service
{
    public class TabletService
    {
        public bool SetVisitorInfo(VisitorInfo visitor)
        {
            TabletDao Dao = new TabletDao();

            //내방 조회 저장 or 수정 순으로 동작
            int VisitorID = Dao.SelectVisitorID(visitor);
            visitor.VisitDateTime = DateTime.Now;

            if (VisitorID > 0) Dao.UpdateVisitor(visitor);
            else
                VisitorID = Dao.InsertVisitor(visitor);

            //접견조회 저장 or 수정 순으로 동작
            int HostID = Dao.SelectHostID(visitor);

            if (HostID == 0)
                HostID = Dao.InsertHost(visitor);

            //내방 이력 저장
            if (VisitorID > 0 && HostID > 0)
            {
                visitor.HostID = HostID;
                Dao.InsertVisitorHistory(visitor);
            }
            else
                return false;



            return true;
        }

        /// <summary>
        /// 아직 다 안짬
        /// </summary>
        /// <param name="VisitorID"></param>
        /// <returns></returns>
        public VisitorInfo GetVisitorInfo(string VisitorID)
        {
            TabletDao Dao = new TabletDao();
            //VisitorInfo Visitor = Dao.SelectVisitorID(visitor);

            return null;
        }

        /// <summary>
        /// 아직 다 안짬
        /// </summary>
        /// <param name="VisitorID"></param>
        /// <returns></returns>
        public VisitorInfo GetVisitorHistoryInfo(int visitorHistorySeq)
        {
            TabletDao Dao = new TabletDao();
            VisitorInfo Visitor = Dao.SelectVisitorHistoryInfo(visitorHistorySeq);

            return Visitor;
        }

        /// <summary>
        /// 아직 다 안짬
        /// </summary>
        /// <param name="visitor"></param>
        /// <returns></returns>
        public bool UpdateVisitorHistoryAgree(VisitorInfo visitor)
        {
            TabletDao Dao = new TabletDao();
            return Dao.UpdateVisitorHistoryAgree(visitor);
        }

        /// <summary>
        /// 전화번호 정규식
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public int ChackMobileVaildation(string number)
        {
            //010-8898-0001 or 010-000-0000 확인
            var hasMobileNumber = new Regex(@"^\d{3}-\d{3,4}-\d{4}$");

            //PhoneNumber 형식이 아닐 경우
            if (!hasMobileNumber.IsMatch(number))
                return 1;

            return 0;

        }
    }
}