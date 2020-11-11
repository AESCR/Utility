using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Common.Utility.JwtBearer
{
    public class AccessTokenGenerate : IAccessTokenGenerate
    {
        private readonly AccessTokenOptions _options;
        private readonly TokenValidationParameters _validationParameters;
        private JwtSecurityTokenHandler _tokenHandler;

        public AccessTokenGenerate(AccessTokenOptions options)
        {
            _options = options;
            _validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = options.SigningCredentials.Key,
                ValidateIssuer = true,
                ValidIssuer = options.Issuer,
                ValidateAudience = true,
                ValidAudience = options.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true
            };
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

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="claims"> </param>
        /// <returns> </returns>
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
            _tokenHandler = new JwtSecurityTokenHandler();
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new AccessToken(encodedJwt, "Bearer", expires);
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="claims"> </param>
        /// <returns> </returns>
        public AccessToken Generate(Dictionary<string, string> claims)
        {
            var claimList = new List<Claim>();
            foreach (var claim in claims) claimList.Add(new Claim(claim.Key, claim.Value));
            return Generate(claimList);
        }

        public AccessToken Generate(JwtDyUser user)
        {
            var claimList = new List<Claim>();
            claimList.Add(new Claim("user", JsonConvert.SerializeObject(user)));
            return Generate(claimList);
        }

        public AccessTokenOptions GetAccessTokenOptions()
        {
            return _options;
        }

        public bool ValidateToken(string token, out JwtSecurityToken securityToken)
        {
            securityToken = null;
            try
            {
                _tokenHandler.ValidateToken(token, _validationParameters, out var validatedToken);
                securityToken = (JwtSecurityToken)validatedToken;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}