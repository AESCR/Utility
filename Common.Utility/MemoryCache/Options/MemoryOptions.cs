namespace Common.Utility.MemoryCache.Options
{
    public class MemoryOptions
    {
        #region Public Properties

        public bool UseRedis { get; set; } = false;
        public int DbIndex { get; set; } = 0;
        public string Host { get; set; } = "localhost";
        public string Password { get; set; }
        public int Port { get; set; } = 6379;

        /// <summary>
        /// 失败后默认重试3次
        /// </summary>
        public int ReconnectAttempts { get; set; } = 3;

        public int ReconnectWait { get; set; } = 200;

        #endregion Public Properties
    }
}