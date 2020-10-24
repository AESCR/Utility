using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Utilities
{
    /// <summary>
    ///     系统操作相关的公共类
    /// </summary>
    public static class SysHelper
    {
        #region Public Properties

        /// <summary>
        ///     获取当前应用程序域
        /// </summary>
        public static AppDomain CurrentAppDomain => Thread.GetDomain();

        /// <summary>
        ///     获取GUID值
        /// </summary>
        public static string NewGUID => Guid.NewGuid().ToString();

        /// <summary>
        ///     获取换行字符
        /// </summary>
        public static string NewLine => Environment.NewLine;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        ///     获取指定调用层级的方法名
        /// </summary>
        /// <param name="level">调用的层数</param>
        public static string GetMethodName(int level)
        {
            //创建一个堆栈跟踪
            var trace = new StackTrace();

            //获取指定调用层级的方法名
            return trace.GetFrame(level).GetMethod().Name;
        }

        #region Console

        /// <summary>
        /// 获取对象所使用的内存
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string GetMemory(object @this)
        {
            GCHandle handle = GCHandle.Alloc(@this, GCHandleType.WeakTrackResurrection);
            IntPtr addr = GCHandle.ToIntPtr(handle);
            string result = $"0x{addr.ToString("X")}";
            Console.WriteLine($"----------{@this.GetType().Name}对象信息------------");
            Console.WriteLine($"Memory地址:{result}");
            Console.WriteLine("------------------------------------------------------------");
            return result;
        }

        public static bool WriteEqual(object obj1, object obj2)
        {
            if (obj1.GetHashCode() == obj2.GetHashCode())
            {
                Console.WriteLine($"{obj1.GetType().Name}:相等");
                return true;
            }
            else
            {
                Console.WriteLine($"{obj1.GetType().Name}:不相等");
                return false;
            }
        }

        /// <summary>
        /// 输出对象基本信息
        /// </summary>
        /// <param name="this"></param>
        public static object WriteInfo(object @this)
        {
            var process = Process.GetCurrentProcess().Id;
            var threadid = Thread.CurrentThread.ManagedThreadId;
            var hashCode = @this.GetHashCode();
            Console.WriteLine($"----------{@this.GetType().Name}对象信息------------");
            Console.WriteLine($"进程ID:{process}");
            Console.WriteLine($"线程ID:{threadid}");
            Console.WriteLine($"HashCode:{hashCode}");
            Console.WriteLine("------------------------------------------------------------");
            return @this;
        }
        /// <summary>
        /// 输出进程使用的内存
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static double GetProcessUsedMemory(Process @this)
        {
            double usedMemory = @this.WorkingSet64 / 1024.0 / 1024.0;
            Console.WriteLine($"----------进程名：{@this.ProcessName}------------");
            Console.WriteLine($"进程Id：{@this.Id}所使用内存:{usedMemory}MB");
            Console.WriteLine("------------------------------------------------------------");
            return usedMemory;
        }
        /// <summary>
        /// 获取程序信息
        /// </summary>
        public static void SystemInfo()
        {
            Console.WriteLine("进程的详细信息");
            Process MyProcess = Process.GetCurrentProcess();
            Console.WriteLine("进程映像名：" + MyProcess.ProcessName);
            Console.WriteLine("进程ID："+MyProcess.Id);
            Console.WriteLine("启动线程数："+MyProcess.Threads.Count.ToString());
            Console.WriteLine("CPU占用时间："+MyProcess.TotalProcessorTime.ToString());
            Console.WriteLine("线程优先级："+MyProcess.PriorityClass.ToString());
            Console.WriteLine("启动时间："+MyProcess.StartTime.ToLongTimeString());
            Console.WriteLine("专用内存："+(MyProcess.PrivateMemorySize/1024).ToString()+"K");
            Console.WriteLine("峰值虚拟内存："+(MyProcess.PeakVirtualMemorySize/1024).ToString()+"K");
            Console.WriteLine("峰值分页内存："+(MyProcess.PeakPagedMemorySize/1024).ToString()+"K");
            Console.WriteLine("分页系统内存："+(MyProcess.PagedSystemMemorySize/1024).ToString()+"K");
            Console.WriteLine("分页内存："+(MyProcess.PagedMemorySize/1024).ToString()+"K");
            Console.WriteLine("未分页系统内存："+(MyProcess.NonpagedSystemMemorySize/1024).ToString()+"K");
            Console.WriteLine("物理内存："+(MyProcess.WorkingSet/1024).ToString()+"K");
            Console.WriteLine("虚拟内存："+(MyProcess.VirtualMemorySize/1024).ToString()+"K");
        }

        #endregion
        #endregion Public Methods
    }
}