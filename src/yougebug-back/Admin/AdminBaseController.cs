using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using yougebug_back.Auth;

namespace yougebug_back.Admin
{
    /// <summary>
    /// 管理端控制器基类
    /// </summary>
    [ClientsLoginCheck]
    [Authorize]
    public abstract class AdminBaseController : Shared.YGBBaseController
    {
        private Domain.Administrators.Account _currentAccount;
        protected Domain.Administrators.Account CurrentAccount
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

                _currentAccount = Domain.Administrators.Hub.GetAccount(token);
                return _currentAccount;
            }
        }
    }
}
