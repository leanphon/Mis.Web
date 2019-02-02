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

        public static TChild AutoCopy<TParent, TChild>(TParent parent) where TChild : TParent, new()
        {
            TChild child = new TChild();
            var parentType = typeof(TParent);
            var properties = parentType.GetProperties();
            foreach (var p in properties)
            {
                //循环遍历属性
                if (p.CanRead && p.CanWrite)
                {
                    //进行属性拷贝
                    p.SetValue(child, p.GetValue(parent, null), null);
                }
            }
            return child;
        }
    }
}
