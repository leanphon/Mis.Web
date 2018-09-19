
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
    /// <summary>
    /// 从枚举列表转换为DataTable，转换后的表中列名为model的diaplay属性的name值
    /// </summary>
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

        public static DataColumn[] GetColumnsByType<T>()
        {
            //4.0或以上版本
            try
            {
                var props = typeof(T).GetProperties();

                var cols = props.Select(p => new DataColumn(GetColumnDisplay(p), GetColumnType(p))).ToArray();

                return cols;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


#if NET_4_0
        public static string GetColumnDisplay(PropertyInfo p)
        {
            //4.0或以上版本
            try
            {
                var arrs = p.GetCustomAttributes(false);
                var arr = arrs.FirstOrDefault() as DisplayAttribute;
                if (arr != null)
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
        public static string GetColumnDisplay(PropertyInfo p)
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

        private static Type GetColumnType(PropertyInfo p)
        {
            Type colType = p.PropertyType;
            if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                colType = colType.GetGenericArguments()[0];
            }

            return colType;
        }


    }
}
