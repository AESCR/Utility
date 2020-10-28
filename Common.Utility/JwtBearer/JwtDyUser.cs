namespace Common.Utility.JwtBearer
{
    /// <summary>
    /// JWT载体
    /// </summary>
    public abstract class JwtDyUser
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string JwtId { get; set; }
    }
}