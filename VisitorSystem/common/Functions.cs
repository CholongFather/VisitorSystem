using System;
using System.Collections;
using System.Reflection;

namespace VisitorSystem.common
{
    public class Functions
    {
        public static Hashtable MakeMappedParams(object objParams)
        {
            Hashtable htResult = new Hashtable();

            Type t = objParams.GetType();
            foreach (PropertyInfo info in t.GetProperties())
            {
                htResult.Add(info.Name, (info.GetValue(objParams, null) == null ? "" : info.GetValue(objParams, null).ToString()));
            }
            return htResult;
        }
    }
}