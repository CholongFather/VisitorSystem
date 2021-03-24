using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using VisitorSystem.Dao;
using VisitorSystem.Models;

namespace VisitorSystem.Service
{
    public class AdminService
    {
        public List<VisitorInfo> GetVisitorInfoList()
        {
            AdminDao Dao = new AdminDao();
            List<VisitorInfo> Visitors = Dao.SelectVisitorHistoryList();

            return Visitors;
        }

        public bool SetVisitorHistoryList(List<VisitorInfo> visitorInfos)
        {

            TabletDao Dao = new TabletDao();

            foreach (VisitorInfo visitor in visitorInfos)
            {
                //내방 조회 저장 or 수정 순으로 동작
                int VisitorID = Dao.SelectVisitorID(visitor);

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
            }

            return true;
        }

        public VisitorInfo GetVisitorInfo(int VisitorHistorySeq)
        {
            AdminDao Dao = new AdminDao();
            VisitorInfo Visitor = Dao.SelectVisitorHistory(VisitorHistorySeq);

            return Visitor;
        }

        public List<VisitorInfo> GetTodayVisitorInfo()
        {
            AdminDao Dao = new AdminDao();
            List<VisitorInfo> Visitors = Dao.SelectTodayVisitorHistory();

            return Visitors;
        }

        public List<Visitor> GetVisitorList()
        {
            AdminDao Dao = new AdminDao();
            return Dao.SelectVisitorList();
        }

        public bool SetVisitor(Visitor visitor)
        {
            AdminDao Dao = new AdminDao();
            return Dao.UpdateVisitor(visitor);
        }

        public bool AddVisitor(Visitor visitor)
        {
            AdminDao Dao = new AdminDao();
            return Dao.InsertVisitor(visitor);

        }

        public bool DeleteVisitor(int visitorID)
        {
            AdminDao Dao = new AdminDao();
            return Dao.DeleteVisitor(visitorID);
        }

        public List<VisitorBlackList> GetVisitorBlackList()
        {
            AdminDao Dao = new AdminDao();
            return Dao.SelectVisitorBlackList();
        }

        public bool SetVisitorBlackList(VisitorBlackList visitorBlackList)
        {
            AdminDao Dao = new AdminDao();
            return Dao.UpdateVisitorBlackList(visitorBlackList);
        }

        public bool InsertVisitorBlackList(int visitorID, string adminID)
        {
            AdminDao Dao = new AdminDao();
            VisitorBlackList visitorBlackList = new VisitorBlackList()
            {
                VisitorID = visitorID,
                AdminID = adminID
            };
            return Dao.InsertVisitorBlackList(visitorBlackList);
        }

        public bool DeleteVisitorBlackList(int VisitorID)
        {
            AdminDao Dao = new AdminDao();
            return Dao.DeleteVisitorBlackList(VisitorID);
        }

        public List<Host> GetHostRanked()
        {
            AdminDao Dao = new AdminDao();
            List<Host> Hosts = Dao.SelectHostRanked();

            return Hosts;
        }

        public List<Host> GetHostList()
        {
            AdminDao Dao = new AdminDao();
            List<Host> Hosts = Dao.SelectHostList();

            return Hosts;
        }

        public bool AddHost(Host host)
        {
            AdminDao Dao = new AdminDao();
            return Dao.InsertHost(host);
        }

        public bool SetHost(Host host)
        {
            AdminDao Dao = new AdminDao();
            return Dao.UpdateHost(host);
        }

        public bool DeleteHost(int hostID)
        {
            AdminDao Dao = new AdminDao();
            return Dao.DeleteHost(hostID);
        }

        public List<Card> GetCardList()
        {
            AdminDao Dao = new AdminDao();
            List<Card> Cards = Dao.SelectCardList();

            return Cards;
        }

        public bool AddCard(Card card)
        {
            AdminDao Dao = new AdminDao();
            return Dao.InsertCard(card);
        }

        public bool SetCard(Card card)
        {
            AdminDao Dao = new AdminDao();
            return Dao.UpdateCard(card);
        }

        public bool DeleteCard(string cardID)
        {
            AdminDao Dao = new AdminDao();
            return Dao.DeleteCard(cardID);
        }

        public List<Location> GetLocationList()
        {
            AdminDao Dao = new AdminDao();
            return Dao.SelectLocationList();
        }

        public bool SetLocation(Location location)
        {
            AdminDao Dao = new AdminDao();
            return Dao.UpdateLocation(location);
        }

        public bool AddLocation(Location location)
        {
            AdminDao Dao = new AdminDao();
            return Dao.InsertLocation(location);
        }

        public bool DeleteLocation(string locationID)
        {
            AdminDao Dao = new AdminDao();
            return Dao.DeleteLocation(locationID);
        }

        public bool SetCardData(VisitorInfo visitorInfo)
        {
            AdminDao Dao = new AdminDao();
            //return Dao.DeleteLocation(visitorInfo);

            return true;
        }


        public int Login(AdminUser adminUser, string locationID)
        {
            //Result 0 없음, 1 기간 만료, 2 성공
            int result = 0;
            AdminDao Dao = new AdminDao();

            


            AdminUser LoginedadminUser = Dao.SelectAdminUser(adminUser, locationID);

            if(LoginedadminUser == null)
            {

            }
            else if((DateTime.Now.AddMonths(-1) - LoginedadminUser.LastChangePWDate).TotalSeconds > 0)
            {
                result = 1;
            }
            else
            {
                result = 2;
            }

            return result;
        }

        public bool SetAdminPassword(AdminUser adminUser, string locationID)
        {
            AdminDao Dao = new AdminDao();
            bool result = Dao.UpdateAdminPassword(adminUser, locationID);

            return result;
        }

        public void SetAdminLog(string adminID, string locationID, string action)
        {
            AdminDao Dao = new AdminDao();
            Dao.SetAdminUserLog(adminID, locationID, action);
        }

        public int ChackPasswordVaildation(string AdminID, string Password)
        {
            int include = 0;

            var hasNumber = new Regex(@"[0-9]+");
            var hasMiniMaxChars = new Regex(@".{10,12}");
            var hasChar = new Regex(@"[a-zA-Z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            var hasSameChar = new Regex(@"(.)\1{2,}");
            var hasSequentialChar = new Regex(@"(abc|bcd|cde|def|efg|fgh|ghi|hij|ijk|jkl|klm|lmn|mno|nop|opq|pqr|qrs|rst|stu|tuv|uvw|vwx|wxy|xyz|012|123|234|345|456|567|678|789)+");

            //문자열 최소 조건 10 ~ 12
            if (!hasMiniMaxChars.IsMatch(Password))
                return 1;

            //특문/숫자/영문 포함 여부
            include += (hasNumber.IsMatch(Password) == true) ? 1 : 0 ;
            include += (hasChar.IsMatch(Password) == true) ? 1 : 0;
            include += (hasSymbols.IsMatch(Password) == true) ? 1 : 0;

            if (include < 2)
                return 2;

            //동일 문자나 숫자가 3개 이상
            if (hasSameChar.IsMatch(Password))
                return 3;

            //연속된 문자나 숫자 3개 이상
            if (hasSequentialChar.IsMatch(Password))
                return 4;

            //password에 ID 포함
            if(Password.Contains(AdminID))
                return 5;

            return 0;

        }

        public int ChackIPVaildation(string IP)
        {
            //IPv4 정규식
            var hasIP = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

            //IP 형식이 아닐 경우
            if (!hasIP.IsMatch(IP))
                return 1;

            return 0;

        }

        public int ChackLengthVaildation(string Text, int length, string type)
        {
            switch(type)
            {
                case "fix":
                    if (Text.Length == length)
                        return 0;
                    break;
                case "over":
                    if (Text.Length > length)
                        return 0;
                    break;
                case "under":
                    if (Text.Length < length)
                        return 0;
                    break;
                default:
                    break;
            }

            return 1;

        }

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