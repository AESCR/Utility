using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utility.JwtBearer
{
    /// <summary>
    /// 签名算法
    /// </summary>
    public enum SecurityType
    {
        HmacSha256,
        RsaSha256,
        RsaSha512
    }

    public class AccessTokenOptions
    {
        #region Private Fields

        private string _secret;
        private SecurityType _securityType;

        #endregion Private Fields

        #region Public Fields

        public static string DefaultSecret = string.Empty;
        public static SecurityType DefaultSecurityType;

        #endregion Public Fields

        #region Public Constructors

        public AccessTokenOptions()
        {
            Expires = 30; //默认30分钟
            Audience = "AESCR";
            Issuer = "AESCR";
            DefaultScheme = "Bearer";
            InitDefault();
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// 权限
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 默认验证方式
        /// </summary>
        public string DefaultScheme { get; set; }

        /// <summary>
        /// 过期时间(分)
        /// </summary>
        public int Expires { get; set; }

        /// <summary>
        /// 签发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 是否必须启动Https 验证
        /// </summary>
        public bool RequireHttpsMetadata { get; set; }

        /// <summary>
        /// 签名秘钥
        /// </summary>
        public string Secret
        {
            get
            {
                if (string.IsNullOrEmpty(_secret))
                    return DefaultSecret;
                return _secret;
            }
            set => _secret = value;
        }

        /// <summary>
        /// 签发秘钥方式
        /// </summary>
        public SecurityType SecurityType
        {
            get
            {
                if (string.IsNullOrEmpty(_secret))
                    return DefaultSecurityType;
                return _securityType;
            }
            set => _securityType = value;
        }

        /// <summary>
        /// 签名证书
        /// </summary>
        public SigningCredentials SigningCredentials => BuildSigningCredentials(Secret, SecurityType);

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// 组装 HmacSha256 签名证书
        /// </summary>
        /// <param name="secret"> </param>
        /// <returns> </returns>
        private SigningCredentials BuildHmacSha256Credentials(string secret)
        {
            var keyByteArray = Encoding.ASCII.GetBytes(secret);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// 组装 RsaSha256 签名证书
        /// </summary>
        /// <param name="secret"> </param>
        /// <returns> </returns>
        private SigningCredentials BuildRsaSha256Credentials(string secret)
        {
            var param = RSA.DecodePkcsPrivateKey(secret);
            var securityKey = new RsaSecurityKey(param);
            return new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
        }

        /// <summary>
        /// 组装 RsaSha512 签名证书
        /// </summary>
        /// <param name="secret"> </param>
        /// <returns> </returns>
        private SigningCredentials BuildRsaSha512Credentials(string secret)
        {
            var param = RSA.DecodePkcsPrivateKey(secret);
            var securityKey = new RsaSecurityKey(param);
            return new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha512);
        }

        /// <summary>
        /// 组装签名证书
        /// </summary>
        /// <param name="secret"> 秘钥 </param>
        /// <param name="securityType"> 证书类型 </param>
        /// <returns> </returns>
        private SigningCredentials BuildSigningCredentials(string secret, SecurityType securityType)
        {
            switch (securityType)
            {
                case SecurityType.HmacSha256:
                    return BuildHmacSha256Credentials(secret);

                case SecurityType.RsaSha256:
                    return BuildRsaSha256Credentials(secret);

                case SecurityType.RsaSha512:
                    return BuildRsaSha512Credentials(secret);

                default:
                    return BuildHmacSha256Credentials(secret);
            }
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length"> 目标字符串的长度 </param>
        /// <param name="useNum"> 是否包含数字，1=包含，默认为包含 </param>
        /// <param name="useLow"> 是否包含小写字母，1=包含，默认为包含 </param>
        /// <param name="useUpp"> 是否包含大写字母，1=包含，默认为包含 </param>
        /// <param name="useSpe"> 是否包含特殊字符，1=包含，默认为不包含 </param>
        /// <param name="custom"> 要包含的自定义字符，直接输入要包含的字符列表 </param>
        /// <returns> 指定长度的随机字符串 </returns>
        private string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            var b = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(b);
            var r = new System.Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum) str += "0123456789";
            if (useLow) str += "abcdefghijklmnopqrstuvwxyz";
            if (useUpp) str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (useSpe) str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
            for (var i = 0; i < length; i++) s += str.Substring(r.Next(0, str.Length - 1), 1);
            return s;
        }

        private void InitDefault()
        {
            if (!string.IsNullOrEmpty(DefaultSecret))
                return;
            lock (DefaultSecret)
            {
                if (!string.IsNullOrEmpty(DefaultSecret))
                    return;

                DefaultSecret = GetRandomString(256, true, true, true, false, string.Empty);
                DefaultSecurityType = SecurityType.HmacSha256;
            }
        }

        #endregion Private Methods
    }
}