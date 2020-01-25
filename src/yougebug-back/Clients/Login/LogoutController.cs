using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using yougebug_back.Shared;

namespace yougebug_back.Clients.Login
{
    /// <summary>
    /// 客户端登出
    /// </summary>
    [Route(Defaults.CLIENT_DEFAULT_ROUTE)]
    public class LogoutController : ClientBaseController
    {
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public async Task<ActionResult> Index()
        {
            return Ok();
        }
    }
}
