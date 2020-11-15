using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility.AOP
{
    /// <summary>
    /// AoP不拦截属性
    /// </summary>
    public class NoInterceptAttribute: Attribute
    {
        public NoInterceptAttribute(params Type[] interceptTypes)
        {
            InterceptTypes = interceptTypes;
        }
        /// <summary>
        /// 不拦截类型的类型
        /// </summary>
        public Type[] InterceptTypes { get; private set; }
    }
}
