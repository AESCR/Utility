using System;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Common.Utility.Autofac;
using Common.Utility.Memory.Model;
using Common.Utility.Memory.Redis;
using ToolBox.Time;

namespace Common.Utility.LogDb
{
    public static class LogRedisExtensions
    {
        public static void RegisterLogRedis(this ContainerBuilder @this, Action<LogRedisgOptions> option)
        {
            var opt = new LogRedisgOptions();
            option(opt);
            @this.RegisterType<LogRedis>().AsImplementedInterfaces()
                .SingleInstance().WithParameter(new TypedParameter(typeof(LogRedisgOptions), opt));
            ;
        }
    }

    /// <summary>
    ///     写入Redis日志
    /// </summary>
    public class LogRedis : ISingletonDependency
    {
        private readonly string _logName;
        private readonly IRedisCache redisCache;

        public LogRedis(LogRedisgOptions options)
        {
            _logName = "logs-" + Assembly.GetExecutingAssembly().GetName().Name?.ToLower();
            if (!string.IsNullOrWhiteSpace(options.LogName)) _logName = options.LogName;
            redisCache = new RedisCache(options);
            redisCache.SwitchDb(options.DbIndex);
        }

        public void Error(object msg)
        {
            Task.Run(() =>
            {
                redisCache.AddList(_logName + "-error-" + TimeHelper.FormatDate(DateTime.Now, "2"), msg);
            });
        }

        public void Info(object msg)
        {
            Task.Run(() =>
            {
                redisCache.AddList(_logName + "-info-" + TimeHelper.FormatDate(DateTime.Now, "2"), msg);
            });
        }

        public void Warn(object msg)
        {
            Task.Run(() =>
            {
                redisCache.AddList(_logName + "-warn-" + TimeHelper.FormatDate(DateTime.Now, "2"), msg);
            });
        }
    }

    /// <summary>
    ///     日志Redis配置
    /// </summary>
    public class LogRedisgOptions : MemoryOptions
    {
        public string LogName { get; set; }
        public new bool UseRedis { get; } = true;
    }
}