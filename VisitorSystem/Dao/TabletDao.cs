using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IBatisNet.DataMapper;
using VisitorSystem.Util;
using VisitorSystem.Models;

namespace VisitorSystem.Dao
{
    public class TabletDao
    {

        public int SelectVisitorID(VisitorInfo visitorInfo)
        {
            try
            {
                int visitorID = Mapper.Instance().QueryForObject<int>("Tablet.SelectVisitorID", visitorInfo);
                visitorInfo.VisitorID = visitorID;
                return visitorID;
            }

            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return 0;
        }

        /// <summary>
        /// 내방객 정보 저장
        /// </summary>
        /// <param name="visitorInfo"></param>
        /// <returns></returns>
        public int InsertVisitor(VisitorInfo visitorInfo)
        {
            try
            {
                Mapper.Instance().Insert("Tablet.InsertVisitor", visitorInfo);
                return visitorInfo.VisitorID;
            }

            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return 0;
        }

        /// <summary>
        /// 내방객 정보 업데이트
        /// 마지막 내방일자 업데이트 용으로 사용
        /// </summary>
        /// <param name="visitorInfo"></param>
        /// <returns></returns>
        public int UpdateVisitor(VisitorInfo visitorInfo)
        {
            try
            {
                Mapper.Instance().Update("Tablet.UpdateVisitor", visitorInfo);

                return 0;
            }

            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return 0;
        }

        public VisitorInfo SelectVisitorHistoryInfo(int visitorHistorySeq)
        {
            try
            {
                VisitorInfo visitor = Mapper.Instance().QueryForObject<VisitorInfo>("Tablet.SelectVisitorHistorySeq", visitorHistorySeq);
                return visitor;
            }

            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return new VisitorInfo();
        }

        /// <summary>
        /// 내방객 정보 업데이트
        /// 마지막 내방일자 업데이트 용으로 사용
        /// </summary>
        /// <param name="visitorInfo"></param>
        /// <returns></returns>
        public bool UpdateVisitorHistoryAgree(VisitorInfo visitorInfo)
        {
            try
            {
                Mapper.Instance().Update("Tablet.UpdateVisitorHistoryAgree", visitorInfo);

                return true;
            }

            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// 방문 이력 저장 Insert
        /// </summary>
        /// <param name="visitorInfo"></param>
        /// <returns></returns>
        public int InsertVisitorHistory(VisitorInfo visitorInfo)
        {
            try
            {
                Mapper.Instance().Insert("Tablet.InsertVisitorHistory", visitorInfo);

                return 0;
            }

            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return 0;
        }



        /// <summary>
        /// 접견인 ID 조회
        /// </summary>
        /// <param name="visitorInfo"></param>
        /// <returns></returns>
        public int SelectHostID(VisitorInfo visitorInfo)
        {
            try
            {
                int Hostid = Mapper.Instance().QueryForObject<int>("Tablet.SelectHostID", visitorInfo);

                return Hostid;
            }

            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return 0;
        }

        /// <summary>
        /// 접견인 저장 Insert
        /// </summary>
        /// <param name="visitorInfo"></param>
        /// <returns></returns>
        public int InsertHost(VisitorInfo visitorInfo)
        {
            try
            {
                Mapper.Instance().Insert("Tablet.InsertHost", visitorInfo);
                return visitorInfo.HostID;
            }

            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return 0;
        }
    }
}