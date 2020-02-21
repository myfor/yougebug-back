using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using yougebug_back.Shared;

namespace yougebug_back.Clients.Login
{
    /// <summary>
    /// 客户端登出
    /// </summary>
    [Route(Defaults.CLIENT_DEFAULT_ROUTE)]
    [ClientsLoginCheck]
    public class LogoutController : ClientBaseController
    {
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public async Task<ActionResult> Index()
        {
            Domain.Resp resp = await CurrentAccount.LogoutAsync();

            return Pack(resp);
        }
    }
}
