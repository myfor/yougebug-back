using Domain;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using yougebug_back.Shared;

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
        public async Task<IActionResult> LoginAsync([FromBody]Domain.Clients.Models.LoginInfo loginInfo)
        {
            Resp result = await Domain.Clients.User.LoginAsync(loginInfo);

            if (!result.IsSuccess)
                return Pack(result);

            Claim[] claims = new Claim[]
            {
                //  token
                new Claim(ClaimTypes.Authentication, result.Data.Token.ToString()),
                //  人员 ID
                new Claim(ClaimTypes.PrimarySid, result.Data.Id.ToString())
            };

            string jwt = Auth.JWT.CreateJwtToken(claims);
            Response.Cookies.Append(Defaults.ADMIN_AUTH_COOKIE_KEY, jwt);

            return Pack(result);
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
                return Redirect("/login");
            return Redirect(string.Format(REDIRECT, r.Message));
        }
    }
}