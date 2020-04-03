using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using yougebug_back.Shared;

namespace yougebug_back.Controllers.Users
{
    [Route("/users")]
    public class UsersController : ClientsContorller
    {
        /// <summary>
        /// 用户主页
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("/{username}")]
        public async Task<IActionResult> UserInfo(string userName)
        {
            /*
             * 详情要获取用户信息，提问问题，回答记录
             */

            Domain.Clients.User user = Domain.Clients.Hub.GetUserByUserName(userName);

            Domain.Clients.Results.ClientDetail detail = await user.GetUserInfoAsync();
            var currentUser = CurrentUser;

            ViewModels.Users.UserInfo model = new ViewModels.Users.UserInfo
            {
                UserName = detail.UserName,
                Email = detail.Email,
                CreateDate = detail.CreateDate,
                Avatar = detail.Avatar
            };
            model.IsSelf = !currentUser.IsEmpty() && currentUser.GetName().Equals(user.GetName(), StringComparison.OrdinalIgnoreCase);

            //  获取用户的第一页提问列表
            List<Domain.Questions.Models.QuestionItem_UserSelf> questionsList = await user.GetSelfQuestionsByDetailAsync(currentUser.Id);

            model.UserAsks = questionsList;

            SetTitle("用户: " + userName);

            return View(model);
        }

        /*
         * 获取用户的提问列表
         */
        [HttpGet("/{userName}/questions")]
        public async Task<IActionResult> GetUserSelfQuestionsAsync(string userName, int index)
        {
            if (index <= 0)
                index = 1;

            Domain.Clients.User user = Domain.Clients.Hub.GetUserByUserName(userName);

            Paginator pager = Paginator.New(index, 10);
            pager.Params = new Dictionary<string, string>
            {
                ["userId"] = user.Id.ToString(),
                ["currentUserId"] = CurrentUser.Id.ToString()
            };

            Paginator resultPager = await user.GetSelfQuestionsAsync(pager);

            return View("questions", resultPager);
        }

        /// <summary>
        /// 修改用户名信息
        /// </summary>
        /// <returns></returns>
        [HttpPut("username")]
        [Authorize]
        [ClientsLoginCheck]
        public async Task<IActionResult> ChangeUserNameAsync([FromBody]Domain.Clients.Models.UserModify model)
        {
            Domain.Resp r = await CurrentUser.ChangeUserInfoAsync(model);
            return Pack(r);
        }

        /// <summary>
        /// 修改用户名密码
        /// </summary>
        [HttpPut("password")]
        [Authorize]
        [ClientsLoginCheck]
        public async Task<IActionResult> ChangePasswordAsync([FromBody]Domain.Clients.Models.ChangePassword model)
        {
            Domain.Resp r = await CurrentUser.ChangePasswordAsync(model);
            return Pack(r);
        }

        /// <summary>
        /// 登出
        /// </summary>
        [HttpPut("logout")]
        [Authorize]
        [ClientsLoginCheck]
        public async Task<IActionResult> LogoutAync()
        {
            var r = await CurrentUser.LogoutAsync();
            Response.Cookies.Delete(Defaults.ADMIN_AUTH_COOKIE_KEY);
            return Pack(r);
        }
    }
}
