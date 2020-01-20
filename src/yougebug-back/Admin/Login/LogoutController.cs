using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using yougebug_back.Shared;

namespace yougebug_back.Admin.Login
{
    /// <summary>
    /// 管理员端登出
    /// </summary>
    [Route(Defaults.ADMIN_DEFAULT_ROUTE)]
    public class LogoutController : AdminBaseController
    {
        [HttpPatch]
        public async Task<ActionResult> Logout()
        {
            Domain.Resp resp = await CurrentAccount.Logout();
            Response.Cookies.Delete(Defaults.ADMIN_AUTH_COOKIE_KEY);
            return Pack(resp);
        }
    }
}
