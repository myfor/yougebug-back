using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using yougebug_back.ViewModels.Questions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace yougebug_back.Controllers.Questions
{
    [Route("questions")]
    public class QuestionsController : Controller
    {
        /*
        最新问题列表
         */
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Newest()
        {
            ViewBag.Title =  "有个bug，最新提问";

            return View("Newest");
        }

        /*
        搜索
         */
        [HttpGet("search/{s}")]
        public IActionResult SearchResult(string s, int index, int size)
        {
            if (string.IsNullOrWhiteSpace(s))
                return Newest();

            ViewBag.Title = "有个bug，提问 - " + s;

            Domain.Paginator page = Domain.Paginator.New(index, size);
            page.TotalRows = 100;

            SearchModel model = new SearchModel();
            model.Search = s;
            model.Page = page;

            return View("Search", model);
        }
    }
}
