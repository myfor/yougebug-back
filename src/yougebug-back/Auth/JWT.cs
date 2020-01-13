using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace yougebug_back.Auth
{
    /// <summary>
    /// JWT
    /// </summary>
    public sealed class JWT
    {
        private static readonly JwtSecurityTokenHandler jwt = new JwtSecurityTokenHandler();

        /// <summary>
        /// 密钥
        /// 在配置文件中配置，没有配置则为空值
        /// </summary>
        public static readonly byte[] SecretKey = Encoding.UTF8.GetBytes("PWQBLASD54N8AsD35H7as3GB45AS*FD354Z35ASD4AER6");
        /// <summary>
        /// iss
        /// 在配置文件中配置，没有配置则为空值
        /// </summary>
        public static string Iss => "THIS_SITE";
        /// <summary>
        /// aud
        /// 在配置文件中配置，没有配置则为空值
        /// </summary>
        public static string Aud => "THIS_SITE";
        /// <summary>
        /// expires
        /// 在配置文件中配置，没有配置则为空值
        /// </summary>
        public static int Expires => 5;

        public static void JwtBearerOption(JwtBearerOptions options)
        {
            options.TokenValidationParameters = GetTokenValidationParameters();
        }

        public static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidIssuer = Iss,
                ValidAudience = Aud,
                //ValidateAudience = true,
                //ValidateIssuer = true,
                //ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(SecretKey)
            };
        }

        /// <summary>
        /// 获取头部中的 JWT 参数
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static string GetJwtInHeader(IHeaderDictionary header)
        {
            if (!header.TryGetValue("Authorization", out StringValues value))
                return "";
            string[] jwts = value.ToString().Split(new char[] { ' ' });
            if (jwts.Length != 2 || jwts[0] != "Bearer")
                return "";
            return jwts[1];
        }

        /// <summary>
        /// 获取 JWT 中的声明
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Claim> GetClaims(string token)
        {
            ClaimsPrincipal principal = GetClaimsPrincipal(token);
            return principal.Claims;
        }

        public static ClaimsPrincipal GetClaimsPrincipal(string token) => jwt.ValidateToken(token, GetTokenValidationParameters(), out SecurityToken securityToken);

        /// <summary>
        /// 创建一个 JWT Token 字符串，使用配置文件配置
        /// </summary>
        /// <returns></returns>
        public static string CreateJwtToken(IEnumerable<Claim> claims)
        {
            DateTime authTime = DateTime.UtcNow;
            DateTime expiresAt = authTime.AddDays(Expires);
            string tokenString = CreateJwtToken(claims, Iss, Aud, expiresAt);
            return tokenString;
        }

        /// <summary>
        /// 创建一个 JWT Token 字符串
        /// </summary>
        /// <returns></returns>
        public static string CreateJwtToken(IEnumerable<Claim> claims, DateTime expires)
        {
            string tokenString = CreateJwtToken(claims, Iss, Aud, expires);
            return tokenString;
        }

        /// <summary>
        /// 创建一个 JWT Token 字符串
        /// </summary>
        /// <returns></returns>
        public static string CreateJwtToken(IEnumerable<Claim> claims, string iss, string aud, DateTime expires)
        {
            var secretKey = new SymmetricSecurityKey(SecretKey);

            DateTime authTime = DateTime.UtcNow;

            JwtSecurityToken token = new JwtSecurityToken
            (
                issuer: iss,
                audience: aud,
                notBefore: authTime,
                claims: claims,
                expires: expires,
                signingCredentials: new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature)
            );
            string tokenString = jwt.WriteToken(token);
            return tokenString;
        }

    }
}
