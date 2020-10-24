﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;

namespace Common.Utility.JwtBearer
{
    public class AccessTokenGenerate : IAccessTokenGenerate
    {
        private readonly AccessTokenOptions _options;

        public AccessTokenGenerate(AccessTokenOptions options)
        {
            _options = options;
        }

        /// <summary>
        ///     生成Token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public AccessToken Generate(IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddMinutes(_options.Expires);
            var issuer = _options.Issuer;
            var audience = _options.Audience;
            var signingCredentials = _options.SigningCredentials;

            var jwt = new JwtSecurityToken(
                issuer,
                audience,
                claims.ToArray(),
                now,
                expires,
                signingCredentials
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new AccessToken(encodedJwt, "Bearer", expires);
        }

        /// <summary>
        ///     生成Token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public AccessToken Generate(Dictionary<string, string> claims)
        {
            var claimList = new List<Claim>();
            foreach (var claim in claims) claimList.Add(new Claim(claim.Key, claim.Value));
            return Generate(claimList);
        }


        private JwtSecurityToken GetJwtSecurityToken(IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;
            var expires = DateTime.UtcNow.AddMinutes(_options.Expires);
            var issuer = _options.Issuer;
            var audience = _options.Audience;
            var signingCredentials = _options.SigningCredentials;

            var jwt = new JwtSecurityToken(
                issuer,
                audience,
                claims.ToArray(),
                now,
                expires,
                signingCredentials
            );
            return jwt;
        }
    }

    public static class AccessTokenGenerateExtensions
    {
        public static AccessToken Generate(this IAccessTokenGenerate tokenGenerate, object user)
        {
            var claimList = new List<Claim>();
            claimList.Add(new Claim("user", JsonConvert.SerializeObject(user)));
            return tokenGenerate.Generate(claimList);
        }
    }
}