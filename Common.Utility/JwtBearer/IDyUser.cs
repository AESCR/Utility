using System;
using System.Collections.Generic;

namespace Common.Utility.JwtBearer
{
    public  interface IDyUser
    {
        /// <summary>
        /// 获取当前访问的用户信息
        /// </summary>
        /// <typeparam name="T">信息载体</typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetDyUser<T>(string key);
        T GetDyUser<T>();
    }
}