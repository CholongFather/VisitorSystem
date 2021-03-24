using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VisitorSystem.Dao;
using VisitorSystem.Models;

namespace VisitorSystem.Service
{
    public class HomeService
    {
        public Location GetLocationAction(string ipAddress)
        {
            HomeDao Dao = new HomeDao();
            Location Location = Dao.SelectLocationFlag(ipAddress);

            return Location;
        }
    }
}