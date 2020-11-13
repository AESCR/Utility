using Microsoft.AspNetCore.Builder;

namespace Common.Utility.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        ///设置默认打开文件
        /// </summary>
        /// <param name="this"></param>
        /// <param name="homePage"></param>
        public static void UseDefaultFile(this IApplicationBuilder @this,
            string homePage = "index.html")
        {
            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add(homePage);
            @this.UseDefaultFiles(defaultFilesOptions);
        }
    }
}