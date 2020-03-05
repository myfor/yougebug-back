using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Domain;

namespace yougebug_back.Controllers
{
    [Route("/")]
    public class HomeController : ClientsContorller
    {
        public IActionResult Index()
        {
            return Redirect("questions");
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        [HttpGet("login")]
        public IActionResult Login()
        {
            SetTitle("有个bug，登录");

            return View();
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        [HttpPut("login")]
        public async Task<IActionResult> LoginAsync()
        {
#warning 未实现
            throw new NotImplementedException();
        }

        /// <summary>
        /// 注册页面
        /// </summary>
        [HttpGet("register")]
        public IActionResult Register()
        {
            SetTitle("有个bug，注册");

            return View();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm]string email, [FromForm]string password)
        {
            const string REDIRECT = "register?" + ALERT_WARNING;

            Domain.Clients.Models.RegisterInfo register = new Domain.Clients.Models.RegisterInfo
            {
                Email = email,
                Password = password
            };

            Domain.Clients.Hub clientHub = new Domain.Clients.Hub();
            Resp r = await clientHub.RegisterAsync(register);
            if (r.IsSuccess)
                //  去登录页面
                return Login();
            return Redirect(string.Format(REDIRECT, r.Message));
        }
    }
}