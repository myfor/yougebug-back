using Microsoft.AspNetCore.Authorization;
/*
 * 客户端控制器基类
 * 默认会进行 Authorize 验证
 * 不需要验证的控制器或者 Action 要加上 AllowAnonymous 特性
 * 如果控制器或 Action 需要保证用户登录，则要加上 ClientsLoginCheck 特性
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using yougebug_back.Auth;

namespace yougebug_back.Clients
{
    /// <summary>
    /// 客户端控制器基类
    /// </summary>
    [Authorize]
    public abstract class ClientBaseController : Shared.YGBBaseController
    {
        private Domain.Clients.User _currentAccount;
        protected Domain.Clients.User CurrentAccount
        {
            get
            {
                if (_currentAccount != null)
                    return _currentAccount;

                string jwt = JWT.GetJwtInHeader(Request.Headers);

                List<Claim> claims = JWT.GetClaims(jwt).ToList();

                string token = claims.First(c => c.Type == ClaimTypes.Authentication).Value.Trim();
                if (string.IsNullOrWhiteSpace(token))
                    throw new Exception("未获取到用户有效凭证");

                _currentAccount = Domain.Clients.Hub.GetUser(token);
                return _currentAccount;
            }
        }
    }
}
