using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace Common.Utility.Extensions.Asp.NetCore
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
            #region 默认首页

            var defaultFilesOptions = new DefaultFilesOptions();
            defaultFilesOptions.DefaultFileNames.Clear();
            defaultFilesOptions.DefaultFileNames.Add(homePage);
            @this.UseDefaultFiles(defaultFilesOptions);
            #endregion
        }

    }
}
