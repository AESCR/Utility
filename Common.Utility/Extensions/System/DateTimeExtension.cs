using System;

namespace Common.Utility.Extensions.System
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns> </returns>
        public static long GetTimeStamp(this DateTime time,bool milliseconds=false)
        {
            var ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(milliseconds ? ts.TotalSeconds : ts.TotalMilliseconds);
        }

        /// <summary>
        /// 获取中文间隔时间差
        /// </summary>
        /// <param name="time"> </param>
        /// <param name="nowTime"></param>
        /// <returns> </returns>
        public static string TimeSpanChinese(this DateTime time, DateTime? nowTime = null)
        {
            var now = nowTime ?? DateTime.Now;
            var span = now.Subtract(time);
            var day = 60 * 24;//天
            var hour = 60;
            if (span.Minutes >= day * 4)
            {
                return $"{time.Year}年{time.Month}月{time.Day}日";
            }
            else if (span.Minutes >= day * 3 && span.Minutes < day * 4)
            {
                return $"{span.Days}天前";
            }
            else if (span.Minutes >= day * 2 && span.Minutes < day * 3)
            {
                return $"{span.Days}天前";
            }
            else if (span.Minutes > day && span.Minutes < day * 2)
            {
                return $"{span.Days}天前";
            }
            else if (span.Minutes < day && span.Minutes >= hour)
            {
                return $"{span.Minutes % 60}小时前";
            }
            else if (span.Minutes < hour && span.Minutes >= 1)
            {
                return $"{span.Minutes}分钟前";
            }
            else
            {
                return "刚刚";
            }
        }
    }
}