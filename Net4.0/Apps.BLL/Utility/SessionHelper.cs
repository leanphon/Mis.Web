﻿using Apps.Model.Privilege;
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
            HttpSessionState session = HttpContext.Current.Session;
            string db = session["CompanyName"] as string;

            return db ?? "null";
        }
        public static long GetUserId()
        {
            HttpSessionState session = HttpContext.Current.Session;
            User u = session["currentUser"] as User;

            return u != null ? u.id : -1;
        }
    }
}
