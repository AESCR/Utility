using System;

namespace Common.Utility.Autofac
{
    public class DependsOnAttribute : Attribute
    {
        #region Public Constructors

        public DependsOnAttribute(params Type[] dependedModuleTypes)
        {
            DependedModuleTypes = dependedModuleTypes;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// 类型的依赖模块
        /// </summary>
        public Type[] DependedModuleTypes { get; private set; }

        #endregion Public Properties
    }
}