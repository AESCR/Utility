using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Common.Utilities
{
    public static class ListExtensions
    {
        #region Public Methods

        /// <summary>
        /// 将指定的集合转换成DataTable。
        /// </summary>
        /// <param name="list"> 将指定的集合。 </param>
        /// <returns> 返回转换后的DataTable。 </returns>
        public static DataTable ToDataTable(this IList list)
        {
            var table = new DataTable();
            if (list.Count > 0)
            {
                var propertys = list[0].GetType().GetProperties();
                foreach (var pi in propertys)
                {
                    var pt = pi.PropertyType;
                    if (pt.IsGenericType && pt.GetGenericTypeDefinition() == typeof(Nullable<>))
                        pt = pt.GetGenericArguments()[0];
                    table.Columns.Add(new DataColumn(pi.Name, pt));
                }

                for (var i = 0; i < list.Count; i++)
                {
                    var tempList = new ArrayList();
                    foreach (var pi in propertys)
                    {
                        var obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }

                    var array = tempList.ToArray();
                    table.LoadDataRow(array, true);
                }
            }

            return table;
        }

        public static DataTable ToDataTable<T>(this List<T> list)
        {
            var table = new DataTable();
            //创建列头
            var propertys = typeof(T).GetProperties();
            foreach (var pi in propertys)
            {
                var pt = pi.PropertyType;
                if (pt.IsGenericType && pt.GetGenericTypeDefinition() == typeof(Nullable<>))
                    pt = pt.GetGenericArguments()[0];
                table.Columns.Add(new DataColumn(pi.Name, pt));
            }

            //创建数据行
            if (list.Count > 0)
                for (var i = 0; i < list.Count; i++)
                {
                    var tempList = new ArrayList();
                    foreach (var pi in propertys)
                    {
                        var obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }

                    var array = tempList.ToArray();
                    table.LoadDataRow(array, true);
                }

            return table;
        }

        #endregion Public Methods
    }
}