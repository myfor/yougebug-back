using Domain;
using Domain.Administrators;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using yougebug_back.Shared;

namespace yougebug_back.Admin.Login
{
    /// <summary>
    /// 管理员端登录
    /// </summary>
    [Route(Defaults.ADMIN_DEFAULT_ROUTE)]
    public class LoginController : YGBBaseController
    {
        /// <summary>
        /// 登录
        /// </summary>
        [HttpPatch]
        public async Task<ActionResult> Login([FromBody]Models.LoginInfo loginInfo)
        {
            (bool isSuccess, Resp result) = await Domain.Administrators.Account.Login(loginInfo);

            if (!isSuccess)
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
