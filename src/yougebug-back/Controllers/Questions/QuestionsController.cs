using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace yougebug_back.Controllers.Questions
{
    [Route("questions")]
    public class QuestionsController : Controller
    {
        /*
        问题列表
         */
        // GET: /<controller>/
        [HttpGet]
        public IActionResult List(string search)
        {
            ViewBag.Title = string.IsNullOrWhiteSpace(search) ? Shared.Defaults.PAGE_DEFAULT_TITLE : "有个bug，提问-" + search;

            return View("List");
        }
    }
}
