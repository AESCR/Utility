using Common.Utility.IsSafeStrings;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Ulink.Core.WebAPI.Filter.XSSSafe
{
    /// <summary>
    /// XSS 过滤器
    /// </summary>
    public class XssSafeFilterAttribute : ActionFilterAttribute, IOrderedFilter
    {
        #region Public Properties
        /// <summary>
        /// 过滤器执行顺序
        /// </summary>
        public new int Order { get; set; } = -1;

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// 遍历实体的字符串属性
        /// </summary>
        /// <param name="type"> 数据类型 </param>
        /// <param name="obj"> 对象 </param>
        /// <returns> </returns>
        private object PostModelFieldFilter(Type type, object obj)
        {
            if (obj != null)
            {
                foreach (var item in type.GetProperties())
                {
                    if (item.GetValue(obj) != null)
                    {
                        //当参数是str
                        if (item.PropertyType == typeof(string))
                        {
                            string value = item.GetValue(obj)?.ToString();
                            item.SetValue(obj, XssSafeUtils.XssFilter(value));
                        }
                        else if (item.PropertyType.IsClass) //当参数是一个实体
                        {
                            item.SetValue(obj, PostModelFieldFilter(item.PropertyType, item.GetValue(obj)));
                        }
                    }
                }
            }

            return obj;
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="context"> </param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //获取参数集合
            var ps = context.ActionDescriptor.Parameters;
            //遍历参数集合
            foreach (var p in ps)
            {
                if (context.ActionArguments[p.Name] != null)
                {
                    //当参数是str
                    if (p.ParameterType == typeof(string))
                    {
                        context.ActionArguments[p.Name] =
                            XssSafeUtils.XssFilter(context.ActionArguments[p.Name].ToString());
                    }
                    else if (p.ParameterType.IsClass) //当参数是一个实体
                    {
                        PostModelFieldFilter(p.ParameterType, context.ActionArguments[p.Name]);
                    }
                }
            }
        }

        #endregion Public Methods
    }
}