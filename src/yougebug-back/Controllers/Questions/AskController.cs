using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace yougebug_back.Controllers.Questions
{
    /// <summary>
    /// 提问
    /// </summary>
    [Route("Ask")]
    public class AskController : ClientsContorller
    {
        [HttpGet]
        public IActionResult Index()
        {
            SetTitle("有个bug - 提问");

            return View("Ask");
        }

        /*
        新的提问
         */
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> NewAskAsync([FromBody]Domain.Questions.Models.PostQuestion model)
        {
            model.UserId = CurrentUser.Id;
            Domain.Resp r = await CurrentUser.AskQuestion(model);

            return Pack(r); ;
        }
    }
}
