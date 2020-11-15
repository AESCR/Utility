using System;
using Common.Utility.Autofac;

namespace Common.Utility.Random.Time
{
    public class RandomTime: ISingletonDependency
    {
        /// <summary>
        /// 得到随机日期
        /// </summary>
        /// <param name="time1"> 起始日期 </param>
        /// <param name="time2"> 结束日期 </param>
        /// <returns> 间隔日期之间的 随机日期 </returns>
        public DateTime GetRandomTime(DateTime time1, DateTime time2)
        {
            var random = new System.Random();
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
    }
}