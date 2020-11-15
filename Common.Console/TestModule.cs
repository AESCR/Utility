using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common.Utility.AOP;
using Common.Utility.Autofac;
using Common.Utility.Memory.Cache;

namespace Common.Console2
{
    public class TestModule: AppModule
    {
        public override void Initialize()
        {
            base.Initialize();
            ModuleAssembly=Assembly.GetAssembly(typeof(CacheInterceptor));
        }
    }
}
