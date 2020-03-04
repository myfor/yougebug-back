using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

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
        public async Task<IActionResult> NewAskAsync()
        {

            throw new NotImplementedException();
            return Redirect($"questions/");
        }
    }
}
