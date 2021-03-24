using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VisitorSystem.Util;
using IBatisNet.DataMapper;
using VisitorSystem.Models;

namespace VisitorSystem.Dao
{
    public class HomeDao
    {
        /// <summary>
        /// Home Location 판별
        /// </summary>
        /// <param name="ipAddress">Ip 주소</param>
        /// <returns></returns>
        public Location SelectLocationFlag(string ipAddress)
        {
            try
            {
                Location location = Mapper.Instance().QueryForObject<Location>("Home.GetLocationAction", ipAddress);
                return location;
            }
            catch (Exception ex)
            {
                LogUtil.ErrorLog(ex.ToString());
            }

            return null;
        }

    }
}