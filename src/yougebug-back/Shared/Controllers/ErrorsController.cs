using Domain;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace yougebug_back.Shared.Controllers
{
    /// <summary>
    /// 异常控制器
    /// </summary>
    [Route("api/[controller]")]
    public class ErrorsController : YGBBaseController
    {
        /// <summary>
        /// 返回 401
        /// </summary>
        [Route("401")]
        public ActionResult NotFound401()
        {
            return Pack(Resp.NeedLogin(Resp.NONE, "请重新登录"));
        }
    }
}
