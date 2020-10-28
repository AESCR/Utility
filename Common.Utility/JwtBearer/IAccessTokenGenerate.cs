﻿using System.Collections.Generic;
 using System.IdentityModel.Tokens.Jwt;
 using System.Security.Claims;

namespace Common.Utility.JwtBearer
{
    public interface IAccessTokenGenerate
    {
        /// <summary>
        ///     生成授权访问Tokan
        /// </summary>
        /// <param name="claims">Token 声明</param>
        /// <returns></returns>
        AccessToken Generate(Dictionary<string, string> claims);

        /// <summary>
        ///     生成Token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        AccessToken Generate(IEnumerable<Claim> claims);

        /// <summary>
        ///     生成Token
        /// </summary>
        /// <param name="JwtDyUser"></param>
        /// <returns></returns>
        AccessToken Generate(JwtDyUser user);
        /// <summary>
        /// 验证前面
        /// </summary>
        /// <param name="token"></param>
        /// <param name="securityToken"></param>
        /// <returns></returns>
        bool ValidateToken(string token, out JwtSecurityToken securityToken);
    }
}