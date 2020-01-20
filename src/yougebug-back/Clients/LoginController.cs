using Domain;
using Domain.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using yougebug_back.Shared;

namespace yougebug_back.Clients
{
    /// <summary>
    /// 客户端登录
    /// </summary>
    [Route(Shared.Defaults.CLIENT_DEFAULT_ROUTE)]
    public class LoginController : YGBBaseController
    {
        /// <summary>
        /// 登录
        /// </summary>
        [HttpPatch]
        public async Task<ActionResult> Login([FromBody]Models.LoginInfo loginInfo)
        { 
            Resp result = await Domain.Clients.User.LoginAsync(loginInfo);

            if (result.Data == Resp.NONE)
                return Pack(result);

            Claim[] claims = new Claim[]
            {
                //  token
                new Claim(ClaimTypes.Authentication, result.Data.Token.ToString()),
                //  人员 ID
                new Claim(ClaimTypes.PrimarySid, result.Data.Id.ToString())
            };

            string jwt = Auth.JWT.CreateJwtToken(claims);
            Response.Cookies.Append(Defaults.ADMIN_AUTH_COOKIE_KEY, jwt);

            return Pack(result);
        }
    }
} 
