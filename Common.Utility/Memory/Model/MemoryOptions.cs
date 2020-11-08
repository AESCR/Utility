namespace Common.Utility.Memory.Model
{
    public class MemoryOptions
    {
        #region Public Properties

        public int DbIndex { get; set; } = 0;
        public string Host { get; set; } = "localhost";
        public string Password { get; set; }
        public int Port { get; set; } = 6379;

        /// <summary>
        /// 失败后默认重试3次
        /// </summary>
        public int ReconnectAttempts { get; set; } = 3;

        public int ReconnectWait { get; set; } = 200;

        /// <summary>
        /// Connection timeout in milliseconds
        /// </summary>
        public int Timeout { get; set; } = 3000;

        /// <summary>
        /// 是否默认注入Redis
        /// </summary>
        public bool UseRedis { get; set; } = false;

        #endregion Public Properties
    }
}