namespace Common.Utility.OAuthClient
{
    public class OAuthClientFactory
    {
        #region Public Methods

        public static IOAuthClient GetOAuthClient(string clientId, string clientSecret, string callbackUrl, AuthType oAuthClientType)
        {
            IOAuthClient authToken = null;
            switch (oAuthClientType)
            {
                case AuthType.QQ:
                    authToken = new OAuthClientQQ(clientId, clientSecret, callbackUrl);
                    break;

                case AuthType.Sina:
                    authToken = new OAuthClientSina(clientId, clientSecret, callbackUrl);
                    break;
            }
            return authToken;
        }

        #endregion Public Methods
    }
}