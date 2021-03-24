using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace VisitorSystem.Util
{

    public class LogUtil
    {
        private static ILog log = LogManager.GetLogger("VisitorSystem");

        public static void DebugLog(string methodName, string message)
        {
            log.Debug(methodName + "  :  " + message);
        }
        public static void InfoLog(string methodName, string message)
        {
            log.Info(methodName + "  :  " + message);
        }
        public static void WarnLog(string methodName, string message)
        {
            log.Warn(methodName + "  :  " + message);
        }
        public static void FatalLog(string methodName, string message)
        {
            log.Fatal(methodName + "  :  " + message);
        }
        public static void ErrorLog(string methodName, string message)
        {
            log.Error(methodName + "  :  " + message);
        }

        public static void DebugLog(string message)
        {
            log.Debug(message);
        }
        public static void InfoLog(string message)
        {
            log.Info(message);
        }
        public static void WarnLog(string message)
        {
            log.Warn(message);
        }
        public static void FatalLog(string message)
        {
            log.Fatal(message);
        }
        public static void ErrorLog(string message)
        {
            log.Error(message);
        }
    }
}