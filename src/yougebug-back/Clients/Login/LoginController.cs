using Domain;
using Domain.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using yougebug_back.Shared;

namespace yougebug_back.Clients.Login
{
    /// <summary>
    /// 客户端登录
    /// </summary>
    [Route(Defaults.CLIENT_DEFAULT_ROUTE)]
    [AllowAnonymous]
    public class LoginController : ClientBaseController
    {
        /// <summary>
        /// 登录
        /// </summary>
        [HttpPatch]
        public async Task<ActionResult> Login([FromBody]Models.LoginInfo loginInfo)
        {
            Resp result = await Domain.Clients.User.LoginAsync(loginInfo);

            if (!result.IsSuccess)
                return Pack(result);

            Claim[] claims = new Claim[]
            {
                //  token
                new Claim(ClaimTypes.Authentication, result.Data.Token.ToString()),
                //  人员 ID
                new Claim(ClaimTypes.PrimarySid, result.Data.Id.ToString())
            };

            string jwt = Auth.JWT.CreateJwtToken(claims);
            Response.Cookies.Append(Defaults.CLIENT_AUTH_COOKIE_KEY, jwt);

            return Pack(result);
        }
    }
}
