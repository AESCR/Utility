﻿using System;
using System.Collections.Generic;

namespace Common.Utility.Extensions.System
{
    public static class CollectionExtension
    {
        /// <summary>
        /// 添加ICollection中不存在的值
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="source"> </param>
        /// <param name="item"> </param>
        /// <returns> </returns>
        public static bool AddIfNotContains<T>(this ICollection<T> source, T item)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (source.Contains(item))
            {
                return false;
            }

            source.Add(item);
            return true;
        }

        /// 判断ICollection 是否有值 或 Null </summary> <typeparam name="T"></typeparam> <param
        /// name="source"></param> <returns></returns>
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }
    }
}