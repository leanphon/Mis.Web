
#define NET_4_0

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Apps.Model.Uitility
{
    public class DataTableHelper
    {
        public static DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            //dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            dt.Columns.AddRange(GetColumnsByType<T>());

            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }

        private static DataColumn[] GetColumnsByType<T>()
        {
            //4.0或以上版本
            try
            {
                var props = typeof(T).GetProperties();

                var cols = props.Select(p => new DataColumn(GetColumnDisplay(p), p.PropertyType)).ToArray();

                return cols;
            }
            catch (Exception ex)
            {
                throw new InvalidCastException();
            }

        }


#if NET_4_0
        private static string GetColumnDisplay(PropertyInfo p)
        {
            //4.0或以上版本
            try
            {
                var arrs = p.GetCustomAttributes(false);
                var arr = arrs.FirstOrDefault() as DisplayAttribute;
                if (arr == null)
                {
                    return arr.Name;
                }

                return "";
            }
            catch (Exception ex)
            {
                throw new InvalidCastException();
            }

        }
#else
        private static string GetColumnDisplay(PropertyInfo p)
        {
            //4.0或以上版本
            try
            {
                var arr = p.GetCustomAttribute<DisplayAttribute>();

                return arr.Name;
            }
            catch (Exception ex)
            {
                throw new InvalidCastException();
            }

        }
#endif
    }
}
