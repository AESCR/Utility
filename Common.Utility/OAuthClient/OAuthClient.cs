using System;
using System.Threading.Tasks;
using System.Web;

namespace Common.Utility.OAuthClient
{
    public abstract class OAuthClient : IOAuthClient
    {
        public OAuthClient(string clientId, string clientSecret, string callbackUrl)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            CallbackUrl = HttpUtility.UrlEncode(callbackUrl);
        }

        public string CallbackUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public virtual Task<AccessTokenObject> GetAccessToken(string code)
        {
            throw new NotImplementedException();
        }

        public virtual string GetAuthUrl()
        {
            throw new NotImplementedException();
        }

        public virtual Task<OAuthUserInfo> GetUserInfo(AccessTokenObject code)
        {
            throw new NotImplementedException();
        }
    }
}