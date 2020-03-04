using Microsoft.AspNetCore.Mvc;

namespace yougebug_back.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("questions");
        }

        /// <summary>
        /// 登录
        /// </summary>
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }
    }
}