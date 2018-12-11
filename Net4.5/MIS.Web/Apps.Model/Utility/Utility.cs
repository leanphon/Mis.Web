﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apps.Model.Utility
{
    public class Utility
    {
        public static int CalYears(DateTime dtBegin, DateTime dtEnd)
        {
            int years = dtEnd.Year - dtBegin.Year;
            if (dtEnd.Month < dtBegin.Month || (dtEnd.Month==dtBegin.Month && dtEnd.Day < dtBegin.Day))
            {
                years--;
            }
            return years;
        }

        public static string GetExceptionMsg(Exception ex)
        {
            string msg = ex.Message;
            Exception inner = ex.InnerException;
            while (inner != null)
            {
                msg += ";" + inner.Message;
                inner = inner.InnerException;
            }

            return msg;
        }
    }
}
