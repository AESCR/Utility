using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Common.Utilities
{
    public static class DataTableExtensions
    {
        public static List<T> ToEntities<T>(this DataTable table) where T : new()
        {
            var entities = new List<T>();
            if (table == null)
                return null;
            foreach (DataRow row in table.Rows)
            {
                var entity = new T();
                foreach (var item in entity.GetType().GetProperties())
                    if (table.Columns.Contains(item.Name))
                        if (DBNull.Value != row[item.Name])
                        {
                            var newType = item.PropertyType;
                            //判断type类型是否为泛型，因为nullable是泛型类,
                            if (newType.IsGenericType
                                && newType.GetGenericTypeDefinition() == typeof(Nullable<>)
                            ) //判断convertsionType是否为nullable泛型类
                            {
                                //如果type为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                                var nullableConverter = new NullableConverter(newType);
                                //将type转换为nullable对的基础基元类型
                                newType = nullableConverter.UnderlyingType;
                            }

                            item.SetValue(entity, Convert.ChangeType(row[item.Name], newType), null);
                        }

                entities.Add(entity);
            }

            return entities;
        }

        public static T ToEntity<T>(this DataTable table) where T : new()
        {
            var entity = new T();
            foreach (DataRow row in table.Rows)
                foreach (var item in entity.GetType().GetProperties())
                    if (row.Table.Columns.Contains(item.Name))
                        if (DBNull.Value != row[item.Name])
                        {
                            var newType = item.PropertyType;
                            //判断type类型是否为泛型，因为nullable是泛型类,
                            if (newType.IsGenericType
                                && newType.GetGenericTypeDefinition() == typeof(Nullable<>)
                            ) //判断convertsionType是否为nullable泛型类
                            {
                                //如果type为nullable类，声明一个NullableConverter类，该类提供从Nullable类到基础基元类型的转换
                                var nullableConverter = new NullableConverter(newType);
                                //将type转换为nullable对的基础基元类型
                                newType = nullableConverter.UnderlyingType;
                            }

                            item.SetValue(entity, Convert.ChangeType(row[item.Name], newType), null);
                        }

            return entity;
        }
    }
}