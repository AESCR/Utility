using System;

namespace Common.Utility.Autofac
{
    /// <summary>
    /// 依赖模块
    /// </summary>
    public class DependsOnAttribute : Attribute
    {
        public DependsOnAttribute(params Type[] dependedModuleTypes)
        {
            DependedModuleTypes = dependedModuleTypes;
        }

        /// <summary>
        /// 类型的依赖模块
        /// </summary>
        public Type[] DependedModuleTypes { get; private set; }
    }
}