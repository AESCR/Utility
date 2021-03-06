﻿using AutoMapper;
using Common.Utility.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace Common.Utility.AutoMapper
{
    /// <summary>
    /// AutoMapper扩展类
    /// </summary>
    public static class AutoMapperExtension
    {
        /// <summary>
        /// 将源对象映射到目标对象
        /// </summary>
        /// <typeparam name="T"> 实体类型 </typeparam>
        /// <param name="obj"> 源对象 </param>
        /// <returns> 转化之后的实体 </returns>

        public static T MapTo<T>(this object obj) where T : new()
        {
            return obj == null ? default(T) : CommonUtils.ConvertObject<T>(obj);
        }

        public static void RegisterAllProfiles(
            this IServiceCollection @this, string assembly, string endName = "Profiles")
        {
            var types = Assembly.Load(assembly).GetTypes().Where(x => x.Name.EndsWith(endName)).ToArray();
            @this.AddAutoMapper(types);
        }
    }
}