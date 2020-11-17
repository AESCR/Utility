using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Utility.AOP
{
    /// <summary>
    /// 要拦截的属性 设置后只拦截指定方法
    /// </summary>
    public class InterceptMethodAttribute: Attribute
    {
        public InterceptMethodAttribute(params Type[] interceptTypes)
        {
            InterceptTypes = interceptTypes;
        }
        /// <summary>
        /// 要拦截类型的类型
        /// </summary>
        public Type[] InterceptTypes { get; private set; }
    }
}
