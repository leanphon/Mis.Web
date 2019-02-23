using System;
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
        public static int CalMonths(DateTime dtBegin, DateTime dtEnd)
        {
            return (dtEnd.Year - dtBegin.Year) * 12 + (dtEnd.Month - dtBegin.Month);
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

        public static TDst AutoCopy<TSrc, TDst>(TSrc parent) where TDst: new()
        {
            TDst child = new TDst();
            var srcType = typeof(TSrc);
            var dstType = typeof(TDst);
            var dstProperties = dstType.GetProperties();

            foreach (var pd in dstProperties)
            {
                //循环遍历属性
                if (pd.CanRead && pd.CanWrite)
                {
                    var ps = srcType.GetProperty(pd.Name);
                    if (ps != null)
                    {
                        //进行属性拷贝
                        pd.SetValue(child, ps.GetValue(parent, null), null);
                    }
                }
            }
            return child;
        }
    }
}
