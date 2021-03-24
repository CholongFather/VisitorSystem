using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VisitorSystem.Util;
using IBatisNet.DataMapper;
using VisitorSystem.Models;

namespace VisitorSystem.Dao
{
    public class AdminDao
    {

        public List<VisitorInfo> SelectVisitorHistoryList()
        {
            try
            {
                IList<VisitorInfo> list = Mapper.Instance().QueryForList<VisitorInfo>("Admin.GetVisitorHistoryList", null);

                return list.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return null;
        }

        public VisitorInfo SelectVisitorHistory(int VisitorHistorySeq)
        {
            try
            {
                VisitorInfo visitor = Mapper.Instance().QueryForObject<VisitorInfo>("Admin.GetVisitorHistory", VisitorHistorySeq);

                return visitor;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return null;
        }

        public List<VisitorInfo> SelectTodayVisitorHistory()
        {
            try
            {
                IList<VisitorInfo> list = Mapper.Instance().QueryForList<VisitorInfo>("Admin.GetTodayVisitorHistory", null);

                return list.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return new List<VisitorInfo>();
        }


        public List<Visitor> SelectVisitorList()
        {
            try
            {
                IList<Visitor> list = Mapper.Instance().QueryForList<Visitor>("Admin.GetVisitorList", null);

                return list.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return new List<Visitor>();
        }

        public bool UpdateVisitor(Visitor visitor)
        {
            try
            {
                 Mapper.Instance().Update("Admin.SetVisitor", visitor);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        public bool InsertVisitor(Visitor visitor)
        {
            try
            {
                Mapper.Instance().Insert("Admin.AddVisitor", visitor);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        public bool DeleteVisitor(int visitorID)
        {
            try
            {
                Mapper.Instance().Update("Admin.DeleteVisitor", visitorID);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        public List<Location> SelectLocationList()
        {
            try
            {
                IList<Location> list = Mapper.Instance().QueryForList<Location>("Admin.GetLocationList", null);

                return list.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return new List<Location>();
        }

        public List<Host> SelectHostRanked()
        {
            try
            {
                IList<Host> list = Mapper.Instance().QueryForList<Host>("Admin.GetHostRank", null);

                return list.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return new List<Host>();
        }

        public List<Host> SelectHostList()
        {
            try
            {
                IList<Host> list = Mapper.Instance().QueryForList<Host>("Admin.GetHostList", null);

                return list.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return new List<Host>();
        }

        public bool InsertHost(Host host)
        {
            try
            {
                Mapper.Instance().Insert("Admin.AddHost", host);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        public bool UpdateHost(Host host)
        {
            try
            {
                Mapper.Instance().Update("Admin.SetHost", host);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        public bool DeleteHost(int hostID)
        {
            try
            {
                Mapper.Instance().Update("Admin.DeleteHost", hostID);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        public List<Card> SelectCardList()
        {
            try
            {
                IList<Card> list = Mapper.Instance().QueryForList<Card>("Admin.GetCardList", null);

                return list.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return new List<Card>();
        }

        public bool InsertCard(Card card)
        {
            try
            {
                Mapper.Instance().Insert("Admin.AddCard", card);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        public bool UpdateCard(Card card)
        {
            try
            {
                Mapper.Instance().Update("Admin.SetCard", card);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        public bool DeleteCard(string cardID)
        {
            try
            {
                Mapper.Instance().Delete("Admin.DeleteCard", cardID);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        public bool InsertLocation(Location location)
        {
            try
            {
                Mapper.Instance().Insert("Admin.AddLocation", location);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        public bool UpdateLocation(Location location)
        {
            try
            {
                Mapper.Instance().Update("Admin.SetLocation", location);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        public bool DeleteLocation(string locationID)
        {
            try
            {
                Mapper.Instance().Delete("Admin.DeleteLocation", locationID);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// Admin User Login
        /// </summary>
        /// <param name="adminUser">Admin 내용 담긴 User 클래스</param>
        /// <returns></returns>
        public AdminUser SelectAdminUser(AdminUser adminUser,string LocationID)
        {
            try
            {
                AdminUser LoginedAdminUser = Mapper.Instance().QueryForObject<AdminUser>("Admin.GetAdminUser", adminUser);

                if (LoginedAdminUser != null)
                {
                    SetAdminUserLog(adminUser.AdminID, LocationID, "관리자 접속");
                    Mapper.Instance().Update("Admin.SetAdminUserLoginDate", adminUser);
                }
                else
                    SetAdminUserLog(adminUser.AdminID, LocationID, "관리자 접속 실패 : 암호 OR ID 틀림");

                return LoginedAdminUser;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// VisitorBlackList
        /// </summary>
        /// <returns></returns>
        public List<VisitorBlackList> SelectVisitorBlackList()
        {
            try
            {
                IList<VisitorBlackList> VisitorBlackLists = Mapper.Instance().QueryForList<VisitorBlackList>("Admin.GetVisitorBlackList", null);

                return VisitorBlackLists.ToList();
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return new List<VisitorBlackList>();
        }

        /// <summary>
        /// VisitorBlackList
        /// </summary>
        /// <returns></returns>
        public bool InsertVisitorBlackList(VisitorBlackList visitorBlackList)
        {
            try
            {
                Mapper.Instance().Insert("Admin.AddVisitorBlackList", visitorBlackList);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// VisitorBlackList
        /// </summary>
        /// <returns></returns>
        public bool UpdateVisitorBlackList(VisitorBlackList visitorBlackList)
        {
            try
            {
                Mapper.Instance().Update("Admin.SetVisitorBlackList", visitorBlackList);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// VisitorBlackList
        /// </summary>
        /// <returns></returns>
        public bool DeleteVisitorBlackList(int visitorID)
        {
            try
            {
                Mapper.Instance().Delete("Admin.DeleteVisitorBlackList", visitorID);

                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        public bool UpdateAdminPassword(AdminUser adminUser, string LocationID)
        {
            try
            {
                Mapper.Instance().Update("Admin.SetAdminUserPassword", adminUser);
                SetAdminUserLog(adminUser.AdminID, LocationID, "관리자 암호 정책 1개월 넘어서 강제 변경");


                return true;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        public void SetAdminUserLog(string adminID, string LocationID, string Action)
        {
            try
            {
                AdminLog adminLog = new AdminLog()
                {
                    AdminID = adminID,
                    Action = Action,
                    LocationID = Convert.ToInt32(LocationID)
                };
                //Admin Log 설정
                Mapper.Instance().Insert("Admin.SetAdminLog", adminLog);

            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }
        }

    }
}