using Apps.Model.Privilege;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Apps.BLL.Utility
{
    public class SessionHelper
    {
        public static string GetDbName()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                string db = HttpContext.Current.Session["CompanyName"] as string;
                return db ?? "null";
            }
            return "null";
        }
        public static long GetUserId()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                User u = HttpContext.Current.Session["currentUser"] as User;
                return u?.id ?? -1;
            }

            return -1;
        }
    }
}
