using Common.Utility.Extensions.Asp.NetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureJson("ratelimit.json")
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseStartup<Startup>()
                        .ConfigureLogging(logging =>
                        {
#if !DEBUG
                            logging.ClearProviders(); //�Ƴ��Ѿ�ע���������־�������
#endif

                            logging.SetMinimumLevel(LogLevel.Trace); //������С����־����
                        });
                });
            return hostBuilder;
        }
    }
}