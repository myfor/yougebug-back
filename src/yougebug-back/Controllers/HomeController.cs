using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

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
        public async Task<IActionResult> RegisterAsync()
        {
#warning 未实现
            throw new NotImplementedException();
        }
    }
}