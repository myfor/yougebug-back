using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace yougebug_back.Controllers.Users
{
    [Route("users")]
    public class UsersController : ClientsContorller
    {
        /// <summary>
        /// 修改用户名信息
        /// </summary>
        /// <returns></returns>
        [HttpPut("username")]
        [Authorize]
        public async Task<IActionResult> ChangeUserNameAsync([FromBody]Domain.Clients.Models.UserModify model)
        {
            Domain.Resp r = await CurrentUser.ChangeUserInfoAsync(model);
            return Pack(r);
        }

        /// <summary>
        /// 用户主页
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("{username}")]
        public async Task<IActionResult> UserInfo(string userName)
        {
            /*
             * 详情要获取用户信息，提问问题，回答记录
             */

            Domain.Clients.Results.ClientDetail detail = await CurrentUser.GetUserInfoAsync();

            return View(detail);
        }
    }
}
