using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace yougebug_back.Admin.Account
{

    [Route(Shared.Defaults.ADMIN_DEFAULT_ROUTE)]
    public class AccountsController : AdminBaseController
    {
        /// <summary>
        /// 当前用户是否登录
        /// </summary>
        [HttpGet("islogged")]
        public Resp IsLogged()
        {
            if (CurrentAccount.IsEmpty())
                return Resp.Fault();
            return Resp.Success(Resp.NONE);
        }
    }
}
