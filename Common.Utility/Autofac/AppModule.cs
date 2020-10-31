using System.Reflection;

namespace Common.Utility.Autofac
{
    public class AppModule
    {
        #region Public Properties

        public Assembly ModuleAssembly { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// 初始化后
        /// </summary>
        public virtual void PostInitialize()
        {
        }

        /// <summary>
        /// 初始化前
        /// </summary>
        public virtual void PreInitialize()
        {
        }

        #endregion Public Methods
    }
}