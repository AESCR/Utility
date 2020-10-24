﻿using System;

namespace Common.Utility.JwtBearer
{
    /// <summary>
    ///     授权访问Token
    /// </summary>
    public class AccessToken
    {
        public AccessToken()
        {
        }

        public AccessToken(string token, string tokenType, DateTime expired, string refreshToken)
        {
            Token = token;
            TokenType = tokenType;
            RefreshToken = refreshToken;
            Expired = expired;
        }

        public AccessToken(string token, string tokenType, DateTime expired) : this(token, tokenType, expired, null)
        {
        }

        /// <summary>
        ///     Token
        /// </summary>
        public string Token { get; }

        /// <summary>
        ///     Token 类型
        /// </summary>
        public string TokenType { get; }

        /// <summary>
        ///     刷新令牌
        /// </summary>
        public string RefreshToken { get; }

        /// <summary>
        ///     过期时间
        /// </summary>
        public DateTime Expired { get; }
    }
}