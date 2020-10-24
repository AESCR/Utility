using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using AutoMapper;

namespace Common.Utility.Extensions.AutoMapper
{
    public static class AutoMapperExtension
    {
        /// <summary>
        /// 将List转换为Datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable MapToTable<T>(this IEnumerable list)
        {
            if (list == null)
                return default(DataTable);

            //创建属性的集合
            List<PropertyInfo> pList = new List<PropertyInfo>();
            //获得反射的入口
            Type type = typeof(T);
            DataTable dt = new DataTable();
            //把所有的public属性加入到集合 并添加DataTable的列
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });
            foreach (var item in list)
            {
                //创建一个DataRow实例
                DataRow row = dt.NewRow();
                //给row 赋值
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));
                //加入到DataTable
                dt.Rows.Add(row);
            }
            return dt;
        }

    }
}