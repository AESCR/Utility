using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Common.Utility.Extensions.Asp.NetCore
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// 添加Json配置文件
        /// </summary>
        /// <param name="this"> </param>
        /// <param name="paths"> </param>
        /// <returns> </returns>
        public static IHostBuilder ConfigureJson(this IHostBuilder @this, params string[] paths)
        {
            return @this.ConfigureAppConfiguration((host, config) =>
            {
                foreach (var path in paths)
                {
                    config.AddJsonFile(path: path, optional: true, reloadOnChange: true);
                }
            });
        }
    }
}