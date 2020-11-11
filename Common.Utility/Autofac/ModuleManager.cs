using Autofac;
using System;
using System.Linq;
using System.Reflection;

namespace Common.Utility.Autofac
{
    public class ModuleManager
    {
        public ModuleManager()
        {
            ContainerBuilder = new ContainerBuilder();
        }

        private static Type StartupType { get; set; }

        public IContainer Container { get; private set; }
        public ContainerBuilder ContainerBuilder { get; private set; }

        public static ModuleManager Create<TModule>() where TModule : AppModule
        {
            StartupType = typeof(TModule);
            return new ModuleManager();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            var obj = StartupType.GetCustomAttributes<DependsOnAttribute>();
            var attributes = obj.SelectMany(t => t.DependedModuleTypes).ToList();
            foreach (var attr in attributes)
            {
                var module = (AppModule)Activator.CreateInstance(attr);
                module.Initialize();
                ContainerBuilder.AutoInjection(module.ModuleAssembly);
            }
            Container = ContainerBuilder.Build();
        }
    }
}