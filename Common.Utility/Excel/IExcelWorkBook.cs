using Autofac;
using Common.Utility.Excel.Model;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Common.Utility.Excel
{
    public interface IExcelWorkBook
    {
        #region Public Methods

        /// <summary>
        /// 读取xls/xlsx文件
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="xlsx"></param>
        /// <returns></returns>
        DataTable ReadExcel(Stream stream, bool xlsx = true);

        /// <summary>
        /// 写Excel xls文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        byte[] WriteExcel(DataTable data);

        #endregion Public Methods
    }
}