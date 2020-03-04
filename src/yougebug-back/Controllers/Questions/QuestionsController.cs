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
        public async Task<IActionResult> Newest(int index, int size)
        {
            ViewBag.Title = "有个bug，最新提问";

            Domain.Paginator page = Domain.Paginator.New(index, size);

            Domain.Questions.Hub hub = new Domain.Questions.Hub();
            page = await hub.GetNewestQuestionsPager(page);
            NewestModel model = new NewestModel
            { 
                Page = page
            };

            return View("Newest", model);
        }

        /*
        搜索
         */
        [HttpGet("search/{s}")]
        public async Task<IActionResult> SearchResult(string s, int index, int size)
        {
            if (string.IsNullOrWhiteSpace(s))
                return await Newest(0, 0);

            ViewBag.Title = "有个bug，提问 - " + s;

            Domain.Paginator page = Domain.Paginator.New(index, size);

            Domain.Questions.Hub hub = new Domain.Questions.Hub();
            page = await hub.GetClientQuestionPager(page);

            SearchModel model = new SearchModel
            {
                Search = s,
                Page = page
            };

            return View("Search", model);
        }

        /*
         * 获取一个提问的详情
         */ 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
