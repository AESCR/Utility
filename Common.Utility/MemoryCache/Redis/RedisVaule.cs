namespace Common.Utility.MemoryCache.Redis
{
    public enum RedisVaule
    {
        /// <summary>
        /// 字符串
        /// </summary>
        String,

        /// <summary>
        /// 哈希
        /// </summary>
        Hash,

        /// <summary>
        /// 列表
        /// </summary>
        List,

        /// <summary>
        /// 集合
        /// </summary>
        Set,

        /// <summary>
        /// 有序集合
        /// </summary>
        SortedSet,

        /// <summary>
        /// 无类型 未设置
        /// </summary>
        None,
    }
}