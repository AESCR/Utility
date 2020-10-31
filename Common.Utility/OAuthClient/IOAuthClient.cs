using System.Threading.Tasks;

namespace Common.Utility.OAuthClient
{
    public interface IOAuthClient
    {
        #region Public Properties

        string CallbackUrl { get; set; }
        string ClientId { get; set; }
        string ClientSecret { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// 获取票据信息
        /// </summary>
        /// <param name="code"> </param>
        /// <returns> </returns>
        Task<AccessTokenObject> GetAccessToken(string code);

        /// <summary>
        /// 获取验证地址
        /// </summary>
        /// <returns> </returns>
        string GetAuthUrl();

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="code"> </param>
        /// <returns> </returns>
        Task<OAuthUserInfo> GetUserInfo(AccessTokenObject code);

        #endregion Public Methods
    }
}