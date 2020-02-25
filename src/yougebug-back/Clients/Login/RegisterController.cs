using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using yougebug_back.Shared;

namespace yougebug_back.Clients.Login
{
    [Route(Defaults.CLIENT_DEFAULT_ROUTE)]
    [AllowAnonymous]
    public class RegisterController : ClientBaseController
    {
        /*
        注册一个用户账号
         */
        // GET: api/<controller>
        [HttpPost]
        public async Task<IActionResult> Index([FromBody]Domain.Clients.Models.RegisterInfo register)
        {
            (bool isValid, string msg) = register.IsValid();
            if (!isValid)
                return Pack(Resp.Fault(Resp.NONE, msg));
            Domain.Clients.Hub clientHub = new Domain.Clients.Hub();
            Resp r = await clientHub.Register(register);
            return Pack(r);
        }
    }
}
