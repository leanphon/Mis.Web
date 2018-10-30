using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apps.BLL.Utility
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
    }
}
