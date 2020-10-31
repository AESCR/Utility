using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace Common.Utility.JwtBearer
{
    /// <summary>
    /// 当前会话对象
    /// </summary>
    public class NullDySession
    {
        #region Private Constructors

        private NullDySession()
        {
        }

        #endregion Private Constructors

        #region Public Properties

        /// <summary>
        /// 获取DySession实例
        /// </summary>
        public static NullDySession Instance { get; } = new NullDySession();

        /// <summary>
        /// 获取当前用户信息 Json
        /// </summary>
        public JwtDyUser DyUser
        {
            get
            {
                var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;

                var claimsIdentity = claimsPrincipal?.Identity as ClaimsIdentity;

                var userClaim = claimsIdentity?.Claims.FirstOrDefault(c => c.Type == "user");
                if (userClaim == null || string.IsNullOrEmpty(userClaim.Value)) return null;
                return JsonConvert.DeserializeObject<JwtDyUser>(userClaim.Value);
            }
        }

        #endregion Public Properties
    }
}