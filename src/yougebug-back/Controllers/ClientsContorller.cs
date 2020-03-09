using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using yougebug_back.Auth;

namespace yougebug_back.Controllers
{
    /// <summary>
    /// 这是MVC控制器基类
    /// </summary>
    public abstract class ClientsContorller : Controller
    {
        /// <summary>
        /// 警告提示
        /// </summary>
        protected const string ALERT_WARNING = "alert-warning={0}";
        ///// <summary>
        ///// 警告提示
        ///// </summary>
        //protected const string ALERT_DANGER = "alert_danger={0}";


        protected void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                ViewBag.Title = "有个bug，程序员的问答社区";
            else
                ViewBag.Title = title;
        }

        /// <summary>
        /// 打包响应数据
        /// </summary>
        /// <returns></returns>
        protected ActionResult Pack(Resp resp)
        {
            ActionResult result = resp.StatusCode switch
            {
                Resp.Code.Success => Ok(resp),
                Resp.Code.Fault => Accepted(resp),
                Resp.Code.NeedLogin => Unauthorized(resp),
                _ => throw new ArgumentException("无效的返回数据")
            };

            return result;
        }

        private Domain.Clients.User _currentUser;
        /// <summary>
        /// 当前登录用户
        /// </summary>
        protected Domain.Clients.User CurrentUser
        {
            get
            {
                if (_currentUser != null)
                    return _currentUser;

                string jwt = JWT.GetJwtInHeader(Request.Headers);

                List<Claim> claims = JWT.GetClaims(jwt).ToList();

                string token = claims.First(c => c.Type == ClaimTypes.Authentication).Value.Trim();
                if (string.IsNullOrWhiteSpace(token))
                    throw new Exception("未获取到用户有效凭证");

                _currentUser = Domain.Clients.Hub.GetUser(token);
                return _currentUser;
            }
        }
        protected bool IsLogged
        {
            get
            {
                try
                {
                    _ = CurrentUser;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
