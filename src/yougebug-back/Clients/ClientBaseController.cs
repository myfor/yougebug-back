using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using yougebug_back.Auth;

namespace yougebug_back.Clients
{
    /// <summary>
    /// 客户端基类，会检查登录
    /// </summary>
    [ClientsLoginCheck]
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
