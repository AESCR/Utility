using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Common.Utility.Autofac;
using NLog;
using NLog.Fluent;
using StackExchange.Profiling.Internal;

namespace Common.Utility.AOP
{
    /// <summary>
    /// 日志 拦截器
    /// </summary>
    public class LoggingInterceptor : AutofacInterceptor, ISingletonDependency
    {
        public LoggingAsyncInterceptor AsyncInterceptor { get; set; }
        public override void SyncIntercept(IInvocation invocation)
        {
            Console.WriteLine(AsyncInterceptor.GetHashCode());
            AsyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }

    public class LoggingAsyncInterceptor : AutofacAsyncInterceptor, ISingletonDependency
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        private static readonly ILogger Logger = LogManager.GetLogger("logs");
        public override void SyncIntercept(IInvocation invocation)
        {
            try
            {
                if (Logger.IsDebugEnabled)
                {

                    Logger.Info("日志拦截器-执行前" + Environment.NewLine + invocation.TargetType.FullName + Environment.NewLine + invocation.Method.Name + Environment.NewLine + $"参数：{invocation.Arguments.ToJson()}:" + Environment.NewLine);
                }
                invocation.Proceed();
                if (Logger.IsDebugEnabled)
                {
                    if (invocation.ReturnValue != null && invocation.ReturnValue is IEnumerable)
                    {
                        dynamic collection = invocation.ReturnValue;
                        Logger.Info("日志拦截器-执行结果：行数：{0}", collection.Count);
                    }
                    else
                    {
                        Logger.Info("日志拦截器-执行结果：行数：{0}", invocation.ReturnValue.ToJson());
                    }
                    Logger.Debug();
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "日志拦截器-异常" + Environment.NewLine + invocation.TargetType.FullName + Environment.NewLine + invocation.Method.Name + Environment.NewLine + $"参数：{invocation.Arguments.ToJson()}:" + Environment.NewLine);
                throw;
            }
        }

        public override void AsynIntercept(IInvocation invocation)
        {
            try
            {
                if (Logger.IsDebugEnabled)
                {

                    Logger.Info("日志拦截器-执行前" + Environment.NewLine + invocation.TargetType.FullName + Environment.NewLine + invocation.Method.Name + Environment.NewLine + $"参数：{invocation.Arguments.ToJson()}:" + Environment.NewLine);
                }
                invocation.Proceed();

                async Task Continuation()
                {
                    await (Task) invocation.ReturnValue;

                    // Step 2. Do something after invocation.
                    if (Logger.IsDebugEnabled)
                    {
                        if (invocation.ReturnValue != null && invocation.ReturnValue is IEnumerable)
                        {
                            dynamic collection = invocation.ReturnValue;
                            Logger.Info("日志拦截器-执行结果：行数：{0}", collection.Count);
                        }
                        else
                        {
                            Logger.Info("日志拦截器-执行结果：行数：{0}", invocation.ReturnValue.ToJson());
                        }
                        Logger.Debug();
                    }
                }

                invocation.ReturnValue = Continuation();

            }
            catch (Exception e)
            {
                Logger.Error(e, "日志拦截器-异常" + Environment.NewLine + invocation.TargetType.FullName + Environment.NewLine + invocation.Method.Name + Environment.NewLine + $"参数：{invocation.Arguments.ToJson()}:" + Environment.NewLine);
                throw;
            }
        }
        public override void AsynInterceptResult<TResult>(IInvocation invocation)
        {
            try
            {
                if (Logger.IsDebugEnabled)
                {

                    Logger.Info("日志拦截器-执行前" + Environment.NewLine + invocation.TargetType.FullName + Environment.NewLine + invocation.Method.Name + Environment.NewLine + $"参数：{invocation.Arguments.ToJson()}:" + Environment.NewLine);
                }
                invocation.Proceed();

                async Task<TResult> Continuation()
                {
                    var task=  (Task<TResult>)invocation.ReturnValue;
                    TResult result = await task;
                    // Step 2. Do something after invocation.
                    if (Logger.IsDebugEnabled)
                    {
                        if (invocation.ReturnValue != null && invocation.ReturnValue is IEnumerable)
                        {
                            dynamic collection = invocation.ReturnValue;
                            Logger.Info("日志拦截器-执行结果：行数：{0}", collection.Count);
                        }
                        else
                        {
                            Logger.Info("日志拦截器-执行结果：行数：{0}", invocation.ReturnValue.ToJson());
                        }
                        Logger.Debug();
                    }

                    return result;
                }

                invocation.ReturnValue = Continuation();

            }
            catch (Exception e)
            {
                Logger.Error(e, "日志拦截器-异常" + Environment.NewLine + invocation.TargetType.FullName + Environment.NewLine + invocation.Method.Name + Environment.NewLine + $"参数：{invocation.Arguments.ToJson()}:" + Environment.NewLine);
                throw;
            }
        }
    }
}
