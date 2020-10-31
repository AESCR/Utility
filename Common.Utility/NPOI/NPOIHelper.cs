using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Data;
using System.IO;
using System.Text;

namespace Common.Utility.NPOI
{
    /// <summary>
    /// NPOI操作帮助类
    /// </summary>
    public class NPOIHelper
    {
        #region Public Methods

        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource"> 源DataTable </param>
        /// <param name="strHeaderText"> 表头文本 </param>
        /// <param name="strFileName"> 保存位置 </param>
        public static void Export(DataTable dtSource, string strHeaderText, string strFileName)
        {
            using (var ms = Export(dtSource, strHeaderText))
            {
                using (var fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    var data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource"> 源DataTable </param>
        /// <param name="strHeaderText"> 表头文本 </param>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(); ;
            var sheet = workbook.CreateSheet();

            var dateStyle = workbook.CreateCellStyle();
            var format = workbook.CreateDataFormat();
            dateStyle.DataFormat = format.GetFormat("yyyy-MM-dd");

            #region 取得每列的列宽（最大宽度）

            var arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
                //GBK对应的code page是CP936
                arrColWidth[item.Ordinal] = Encoding.UTF8.GetBytes(item.ColumnName).Length;
            for (var i = 0; i < dtSource.Rows.Count; i++)
                for (var j = 0; j < dtSource.Columns.Count; j++)
                {
                    var intTemp = Encoding.UTF8.GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j]) arrColWidth[j] = intTemp;
                }

            #endregion 取得每列的列宽（最大宽度）

            var rowIndex = 0;

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式

                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0) sheet = workbook.CreateSheet();

                    #region 表头及样式

                    {
                        var headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        var headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        var font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.IsBold = true;
                        headStyle.SetFont(font);

                        headerRow.GetCell(0).CellStyle = headStyle;
                        if (dtSource.Columns.Count - 1 > 1)
                        {
                            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                        }
                    }

                    #endregion 表头及样式

                    #region 列头及样式

                    {
                        var headerRow = sheet.CreateRow(1);
                        var headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.Center;
                        var font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.IsBold = true;
                        headStyle.SetFont(font);

                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                    }

                    #endregion 列头及样式

                    rowIndex = 2;
                }

                #endregion 新建表，填充表头，填充列头，样式

                #region 填充内容

                var contentStyle = workbook.CreateCellStyle();
                contentStyle.Alignment = HorizontalAlignment.Left;
                var dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    var newCell = dataRow.CreateCell(column.Ordinal);
                    newCell.CellStyle = contentStyle;

                    var drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String": //字符串类型
                            newCell.SetCellValue(drValue);
                            break;

                        case "System.DateTime": //日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle; //格式化显示
                            break;

                        case "System.Boolean": //布尔型
                            var boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;

                        case "System.Int16": //整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            var intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;

                        case "System.Decimal": //浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;

                        case "System.DBNull": //空值处理
                            newCell.SetCellValue("");
                            break;

                        default:
                            newCell.SetCellValue("");
                            break;
                    }
                }

                #endregion 填充内容

                rowIndex++;
            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// 读取excel 默认第一行为标头
        /// </summary>
        /// <param name="strFileName"> excel文档路径 </param>
        /// <returns> </returns>
        public static DataTable Import(string strFileName)
        {
            var dt = new DataTable();
            IWorkbook workbook;
            using (var file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                if (Path.GetExtension(strFileName) == "xlsx")
                {
                    workbook = new XSSFWorkbook(file);
                }
                else
                {
                    workbook = new HSSFWorkbook(file);
                }
            }

            var sheet = workbook.GetSheetAt(0);
            var rows = sheet.GetRowEnumerator();

            var headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (var j = 0; j < cellCount; j++)
            {
                var cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                var dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();

                dt.Rows.Add(dataRow);
            }

            return dt;
        }

        #endregion Public Methods
    }
}