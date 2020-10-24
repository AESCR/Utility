using System;
using System.Collections.Generic;
using System.Globalization;

namespace ToolBox.Time
{
    public static class DateTimeUtility
    {
        #region Public Methods

        /// <summary>
        ///     比较计算年季是否大于当前年季
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quarter"></param>
        /// <param name="seq"></param>
        /// <returns></returns>
        public static bool CompareCurrentQuarter(string year, string quarter, string seq)
        {
            var result = false;
            var currentYear = DateTime.Now.Year.ToString();
            var currentMonth = MonthConvertToQuarter(DateTime.Now.Month);
            var calculateYear = int.Parse(year) + int.Parse(seq) - 1;

            result = int.Parse(currentYear + currentMonth) >= int.Parse(calculateYear + quarter) ? false : true;
            return result;
        }

        /// <summary>
        ///     比较DataTale结果集中的年月是否大于当前年月
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static bool CompareCurrentYearMonth(string year, string month)
        {
            var date = DateTime.Now;
            return new DateTime(date.Year, date.Month, 2) <=
                   new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1);
        }

        /// <summary>
        ///     2013-03-21废弃WWF,已有同样方法ParseToDateValue，为什么又添加一个？？？？？？
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ConvertObjectToNullableDate(object obj)
        {
            if (obj == null) return null;
            DateTime tmpDateTime;
            if (DateTime.TryParse(obj.ToString(), out tmpDateTime))
                return tmpDateTime;
            return null;
        }

        public static string FormatDate(string dateString)
        {
            var dt = ParseToDateValue(dateString);
            if (dt == null)
                return string.Empty;
            return string.Format("{0:yyyy-MM-dd}", dt);
        }

        public static string FormatDate(DateTime? date)
        {
            if (date == null)
                return string.Empty;
            return string.Format("{0:yyyy-MM-dd}", date);
        }

        public static string FormatDate(object date)
        {
            if (date == null)
                return string.Empty;
            return string.Format("{0:yyyy-MM-dd}", date);
        }

        public static string FormatDateTime(object date)
        {
            if (date == null)
                return string.Empty;
            return string.Format("{0:yyyy-MM-dd HH:mm}", date);
        }

        /// <summary>
        ///     指定的日期格式格式化日期
        /// </summary>
        /// <param name="dateValue">日期</param>
        /// <param name="dataFormat">日期格式</param>
        /// <returns></returns>
        public static string FormatDateTime(DateTime dateValue, string dataFormat)
        {
            string result = null;
            try
            {
                result = dateValue.ToString(dataFormat);
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        ///     获取两个时间差：返回 天
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string GetDaysOfTwoDate(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null) return string.Empty;
            var TimSpan = endDate.Value.Subtract(startDate.Value);
            return TimSpan.Days.ToString();
        }

        /// <summary>
        ///     获取月份的结束日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static DateTime? GetEndDateOfMonth(int year, int month)
        {
            var startDt = GetStartDateOfMonth(year, month);
            if (startDt == null) return null;
            return startDt.Value.AddMonths(1).AddDays(-1);
        }

        /// <summary>
        ///     获取季度的结束日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public static DateTime? GetEndDateOfQuarter(int year, int quarter)
        {
            var startDt = GetStartDateOfQuarter(year, quarter);
            if (startDt == null) return null;
            return startDt.Value.AddMonths(3).AddDays(-1);
        }

        /// <summary>
        ///     获取两个时间差：返回 月
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int GetMonthsOfTwoDate(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null) return 0;
            var months = (endDate.Value.Year - startDate.Value.Year) * 12 +
                         (endDate.Value.Month - startDate.Value.Month);
            //如果天数还没到，月数要减1
            if (endDate.Value.Day < startDate.Value.Day) months--;
            return months;
        }

        /// <summary>
        ///     获取日期对应的季度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int GetQuarter(this DateTime input)
        {
            if (input.Month % 3 > 0)
                return input.Month / 3 + 1;
            return input.Month / 3;
        }

        public static DateTime? GetStartDateOfHalfYear(int year, int halfYear)
        {
            //halfYear=6 or 12
            var m = halfYear == 6 ? "1" : "7";
            var strDt = year + "-" + m + "-1";
            return ParseToDateValue(strDt);
        }

        /// <summary>
        ///     获取月份的开始日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static DateTime? GetStartDateOfMonth(int year, int month)
        {
            var strDt = year + "-" + month + "-1";
            return ParseToDateValue(strDt);
        }

        /// <summary>
        ///     获取季度的开始日期
        /// </summary>
        /// <param name="year"></param>
        /// <param name="quarter"></param>
        /// <returns></returns>
        public static DateTime? GetStartDateOfQuarter(int year, int quarter)
        {
            var strDt = year + "-" + ((quarter - 1) * 3 + 1) + "-1";
            return ParseToDateValue(strDt);
        }

        /// <summary>
        ///     获取当前日期第一个星期几是哪天
        ///     DateTime dt=getWeekUpOfDate(DateTime.Now,DayOfWeek.Monday,-1);
        ///     这是获取当前日期的上周一的日期
        ///     DateTime dt=getWeekUpOfDate(DateTime.Now,DayOfWeek.Monday,-2);
        ///     这是获取当前日期的上上周一的日期
        ///     DateTime dt=getWeekUpOfDate(DateTime.Now,DayOfWeek.Monday,1);
        ///     这是获取当前日期的下周一的日期
        ///     DateTime dt=getWeekUpOfDate(DateTime.Now,DayOfWeek.Monday,0);
        ///     这是获取本周周一的日期
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="weekday"></param>
        /// <param name="Number"></param>
        /// <returns></returns>
        public static DateTime GetWeekUpOfDate(DateTime dt, DayOfWeek weekday, int Number)
        {
            var wd1 = (int)weekday;
            var wd2 = (int)dt.DayOfWeek;
            return wd2 == wd1 ? dt.AddDays(7 * Number) : dt.AddDays(7 * Number - wd2 + wd1);
        }

        public static List<int> GetYearArray(int start)
        {
            var lst = new List<int>();
            for (var i = start; i <= DateTime.Today.Year; i++) lst.Add(i);
            return lst;
        }

        /// <summary>
        ///     获取两个时间差：返回 年
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static double GetYearsOfTwoDate(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null || endDate == null) return 0;
            var months = GetMonthsOfTwoDate(startDate, endDate);

            var totalyears = months / 12d;
            return double.Parse(string.Format("{0:N2}", totalyears));
        }

        /// <summary>
        ///     计算届别+离开第几年，判读是否大于当前年月
        /// </summary>
        /// <param name="className">届别</param>
        /// <param name="outSeq">工作第几年离开</param>
        /// <param name="month">离开月份</param>
        /// <returns></returns>
        public static bool IsBiggerCurrentDate(string className, string outSeq, string month)
        {
            var result = false;

            //计算年份
            var caculateYearMonth = int.Parse(className) + int.Parse(outSeq) - 1 + month;
            var currentYearMonth = DateTime.Now.ToString("yyyyMM");
            result = int.Parse(caculateYearMonth) > int.Parse(currentYearMonth) ? true : false;
            return result;
        }

        /// <summary>
        ///     将月份转换成季度
        /// </summary>
        /// <param name="month">月份</param>
        /// <returns></returns>
        public static int MonthConvertToQuarter(int month)
        {
            return month / 3 + (month % 3 > 0 ? 1 : 0);
        }

        public static string ParseDateToFullDate(object dt)
        {
            var dtObj = ParseToDateValue(dt);
            if (dtObj == null) return string.Empty;
            return string.Format("{0}年{1}月{2}日", dtObj.Value.Year, dtObj.Value.Month, dtObj.Value.Day);
        }

        public static string ParseDateToYearMonth(object dt)
        {
            var dtObj = ParseToDateValue(dt);
            if (dtObj == null) return string.Empty;
            return string.Format("{0}年{1}月", dtObj.Value.Year, dtObj.Value.Month);
        }

        /// <summary>
        ///     将String转换为DateTime?类型
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns>DateTime?</returns>
        public static DateTime? ParseToDateValue(string dateString)
        {
            if (string.IsNullOrEmpty(dateString)) return null;
            DateTime dtValue;
            if (!DateTime.TryParse(dateString, out dtValue)) return null;
            return dtValue;
        }

        public static DateTime? ParseToDateValue(object dateObj)
        {
            if (dateObj == null) return null;
            DateTime dtValue;
            if (!DateTime.TryParse(dateObj.ToString(), out dtValue)) return null;
            return dtValue;
        }

        public static DateTime? ParseYearMonthToDateTimeValue(string ymString)
        {
            if (string.IsNullOrEmpty(ymString)) return null;
            ymString = ymString.Replace("年", "-").Replace("月", "-1");
            var dateString = ymString;

            DateTime dtValue;
            if (!DateTime.TryParse(dateString, out dtValue)) return null;
            return dtValue;
        }

        /// <summary>
        ///     获取日期是当月的第几周
        /// </summary>
        /// <param name="day"></param>
        /// <param name="WeekStart">1表示 周一至周日 为一周 2表示 周日至周六 为一周</param>
        /// <returns></returns>
        public static int WeekOfMonth(DateTime day, int WeekStart)
        {
            //WeekStart
            //1表示 周一至周日 为一周

            //2表示 周日至周六 为一周

            DateTime FirstofMonth;
            FirstofMonth = Convert.ToDateTime(day.Date.Year + "-" + day.Date.Month + "-" + 1);

            var i = (int)FirstofMonth.Date.DayOfWeek;
            if (i == 0) i = 7;

            if (WeekStart == 1) return (day.Date.Day + i - 2) / 7 + 1;
            if (WeekStart == 2) return (day.Date.Day + i - 1) / 7;
            return 0;
            //错误返回值0
        }

        #endregion Public Methods
    }

    /// <summary>
    ///     时间类
    ///     1、SecondToMinute(int Second) 把秒转换成分钟
    /// </summary>
    public class TimeHelper
    {
        #region Public Methods

        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                //TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                //TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                //TimeSpan ts = ts1.Subtract(ts2).Duration();
                var ts = DateTime2 - DateTime1;
                if (ts.Days >= 1)
                {
                    dateDiff = DateTime1.Month + "月" + DateTime1.Day + "日";
                }
                else
                {
                    if (ts.Hours > 1)
                        dateDiff = ts.Hours + "小时前";
                    else
                        dateDiff = ts.Minutes + "分钟前";
                }
            }
            catch
            {
            }

            return dateDiff;
        }

        /// <summary>
        ///     获得两个日期的间隔
        /// </summary>
        /// <param name="DateTime1">日期一。</param>
        /// <param name="DateTime2">日期二。</param>
        /// <returns>日期间隔TimeSpan。</returns>
        public static TimeSpan DateDiff2(DateTime DateTime1, DateTime DateTime2)
        {
            var ts1 = new TimeSpan(DateTime1.Ticks);
            var ts2 = new TimeSpan(DateTime2.Ticks);
            var ts = ts1.Subtract(ts2).Duration();
            return ts;
        }

        /// <summary>
        ///     格式化日期时间
        /// </summary>
        /// <param name="dateTime1">日期时间</param>
        /// <param name="dateMode">显示模式</param>
        /// <returns>0-9种模式的日期</returns>
        public static string FormatDate(DateTime dateTime1, string dateMode)
        {
            switch (dateMode)
            {
                case "0":
                    return dateTime1.ToString("yyyy-MM-dd");

                case "1":
                    return dateTime1.ToString("yyyy-MM-dd HH:mm:ss");

                case "2":
                    return dateTime1.ToString("yyyy/MM/dd");

                case "3":
                    return dateTime1.ToString("yyyy年MM月dd日");

                case "4":
                    return dateTime1.ToString("MM-dd");

                case "5":
                    return dateTime1.ToString("MM/dd");

                case "6":
                    return dateTime1.ToString("MM月dd日");

                case "7":
                    return dateTime1.ToString("yyyy-MM");

                case "8":
                    return dateTime1.ToString("yyyy/MM");

                case "9":
                    return dateTime1.ToString("yyyy年MM月");

                default:
                    return dateTime1.ToString();
            }
        }

        /// <summary>
        ///     返回某年某月最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static int GetMonthLastDate(int year, int month)
        {
            var lastDay = new DateTime(year, month, new GregorianCalendar().GetDaysInMonth(year, month));
            var Day = lastDay.Day;
            return Day;
        }

        /// <summary>
        ///     得到随机日期
        /// </summary>
        /// <param name="time1">起始日期</param>
        /// <param name="time2">结束日期</param>
        /// <returns>间隔日期之间的 随机日期</returns>
        public static DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            var random = new Random();
            var minTime = new DateTime();
            var maxTime = new DateTime();

            var ts = new TimeSpan(time1.Ticks - time2.Ticks);

            // 获取两个时间相隔的秒数
            var dTotalSecontds = ts.TotalSeconds;
            var iTotalSecontds = 0;

            if (dTotalSecontds > int.MaxValue)
                iTotalSecontds = int.MaxValue;
            else if (dTotalSecontds < int.MinValue)
                iTotalSecontds = int.MinValue;
            else
                iTotalSecontds = (int)dTotalSecontds;

            if (iTotalSecontds > 0)
            {
                minTime = time2;
                maxTime = time1;
            }
            else if (iTotalSecontds < 0)
            {
                minTime = time1;
                maxTime = time2;
            }
            else
            {
                return time1;
            }

            var maxValue = iTotalSecontds;

            if (iTotalSecontds <= int.MinValue)
                maxValue = int.MinValue + 1;

            var i = random.Next(Math.Abs(maxValue));

            return minTime.AddSeconds(i);
        }

        /// <summary>
        ///     把秒转换成分钟
        /// </summary>
        /// <returns></returns>
        public static int SecondToMinute(int Second)
        {
            var mm = Second / (decimal)60;
            return Convert.ToInt32(Math.Ceiling(mm));
        }

        /// <summary>
        ///     将时间格式化成 年月日 的形式,如果时间为null，返回当前系统时间
        /// </summary>
        /// <param name="dt">年月日分隔符</param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string GetFormatDate(DateTime dt, char separator)
        {
            if (dt != null && !dt.Equals(DBNull.Value))
            {
                var tem = string.Format("yyyy{0}MM{1}dd", separator, separator);
                return dt.ToString(tem);
            }

            return GetFormatDate(DateTime.Now, separator);
        }

        /// <summary>
        ///     将时间格式化成 时分秒 的形式,如果时间为null，返回当前系统时间
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public string GetFormatTime(DateTime dt, char separator)
        {
            if (dt != null && !dt.Equals(DBNull.Value))
            {
                var tem = string.Format("hh{0}mm{1}ss", separator, separator);
                return dt.ToString(tem);
            }

            return GetFormatDate(DateTime.Now, separator);
        }

        #endregion Public Methods
    }
}