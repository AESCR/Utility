﻿using System;

namespace Common.Utility.Extensions.System
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns> </returns>
        public static string GetTimeStamp(this DateTime time)
        {
            TimeSpan ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 获取格式化时间字符串 HH:mm:ss
        /// </summary>
        /// <param name="time"> </param>
        /// <returns> </returns>
        public static string HHmmss(this DateTime time)
        {
            return time.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 获取中文间隔时间差
        /// </summary>
        /// <param name="time"> </param>
        /// <returns> </returns>
        public static string TimeSpanChinese(this DateTime time, DateTime? nowTime = null)
        {
            var now = nowTime.HasValue ? nowTime.Value : DateTime.Now;
            var span = now.Subtract(time);
            var day = 60 * 24;//天
            var hour = 60;
            if (span.Minutes >= day * 4)
            {
                return string.Format("{0}年{1}月{2}日", time.Year, time.Month, time.Day);
            }
            else if (span.Minutes >= day * 3 && span.Minutes < day * 4)
            {
                return string.Format("{0}天前", span.Days);
            }
            else if (span.Minutes >= day * 2 && span.Minutes < day * 3)
            {
                return string.Format("{0}天前", span.Days);
            }
            else if (span.Minutes > day && span.Minutes < day * 2)
            {
                return string.Format("{0}天前", span.Days);
            }
            else if (span.Minutes < day && span.Minutes >= hour)
            {
                return string.Format("{0}小时前", span.Minutes % 60);
            }
            else if (span.Minutes < hour && span.Minutes >= 1)
            {
                return string.Format("{0}分钟前", span.Minutes);
            }
            else
            {
                return "刚刚";
            }
        }

        /*
                /// <summary>
                /// 获取格式化时间字符串 yyyy-MM-dd HH:mm:ss
                /// </summary>
                /// <param name="time"> </param>
                /// <returns> </returns>
                public static string yyyMMddHHmmss(this DateTime time)
                {
                    return time.ToString("yyyy-MM-dd HH:mm:ss");
                }

                /// <summary>
                /// 获取格式化时间字符串 yyyy/MM/dd HH:mm:ss
                /// </summary>
                /// <param name="time"> </param>
                /// <returns> </returns>
                public static string yyyMMddHHmmss2(this DateTime time)
                {
                    return time.ToString("yyyy/MM/dd HH:mm:ss");
                }
        */
    }
}