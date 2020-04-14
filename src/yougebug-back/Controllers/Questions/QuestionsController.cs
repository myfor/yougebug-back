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
    public class QuestionsController : ClientsContorller
    {
        /*
        最新问题列表
         */
        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> Newest(int index, int size)
        {
            SetTitle("最新提问");

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

            SetTitle(s);

            Domain.Paginator page = Domain.Paginator.New(index, size);
            page.Params.Add("search", s);

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
        [HttpGet("{id}/{title}")]
        public async Task<IActionResult> GetQuestionDetailAsync(int id, string title, int index, int size)
        {
            SetTitle(title);
            index = index == 0 ? 1 : index;
            size = size == 0 ? 10 : size;

            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp resp = await question.GetDetailAsync(Domain.Questions.Question.DetailSource.Client, index, size);
            if (!resp.IsSuccess)
                return Redirect(string.Format($"/questions/?{ALERT_WARNING}", resp.Message));
            Domain.Questions.Results.QuestionDetail model = resp.GetData<Domain.Questions.Results.QuestionDetail>();
            //  是否为本人
            model.IsSelf = CurrentUser.Id == model.User.Id;

            return View("QuestionDetail", model);
        }

        /*
         * 编辑页
         */
        [HttpGet("{id}/edit")]
        public async Task<IActionResult> EditQuestionAsync(int id)
        {
            SetTitle("修改提问");

            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Questions.Results.QuestionEditInfo model = await question.GetEditInfoAsync(id);
            if (model == default)
                return NotFound();
            return View("Edit", model);
        }

        /// <summary>
        /// 追问提问
        /// </summary>
        [HttpPost("{id}/comment")]
        public async Task<IActionResult> PostComment(int id, [FromBody]Domain.Questions.Models.NewComment model)
        {
            //  必须登录
            if (CurrentUser.IsEmpty())
                return Pack(Domain.Resp.NeedLogin());

            if (string.IsNullOrWhiteSpace(model.Comment))
                return Pack(Domain.Resp.Fault(Domain.Resp.NONE, "追问内容不能为空"));

            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp r = await question.AddCommentAsyns(CurrentUser.Id, model.Comment);
            return Pack(r);
        }

        /// <summary>
        /// 举报
        /// </summary>
        [HttpPost("{id}/report")]
        public async Task<IActionResult> PostReportAsync(int id, [FromBody]Domain.Questions.Models.NewReport model)
        {
            int reporterId;
            if (CurrentUser.IsEmpty())
                reporterId = 0;
            else
                reporterId = CurrentUser.Id;

            var question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp r = await question.ReportAsync(model.Reason, model.Description, reporterId);
            return Pack(r);
        }

        /// <summary>
        /// 修改提问
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditQuestionAsync(int id, [FromBody]Domain.Questions.Models.EditQuestion model)
        {
            if (CurrentUser.IsEmpty())
                return Pack(Domain.Resp.NeedLogin(Domain.Resp.NONE, "请重新登录"));
            model.CurrentUserId = CurrentUser.Id;
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            var r = await question.EditAsync(model);
            return Pack(r);
        }

        /// <summary>
        /// 删除一个提问
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var questionHub = new Domain.Questions.Hub();
            Domain.Resp r = await questionHub.DeleteQuestionAsync(id, true);
            return Pack(r);
        }

        /// <summary>
        /// 提交一个答案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/answer")]
        public async Task<IActionResult> PostAnswerAsync(int id, [FromBody]Domain.Answers.Models.PostAnswer model)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);

            bool isLogin = !CurrentUser.IsEmpty();
            if (isLogin != model.IsLogin)
                return Pack(Domain.Resp.NeedLogin(Domain.Resp.NONE, "登录超时"));

            if (isLogin)
                return Pack(await question.AddAnswerAsync(CurrentUser.Id, model.Content));
            return Pack(await question.AddAnswerAsync(model.NickName, model.Content));
        }

        /// <summary>
        /// 同意
        /// </summary>
        [HttpPatch("{id}/like")]
        public async Task<IActionResult> LikeAsync(int id)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp r = await question.LikeAsync();
            return Pack(r);
        }

        /// <summary>
        /// 不同意
        /// </summary>
        [HttpPatch("{id}/unlike")]
        public async Task<IActionResult> UnLikeAsync(int id)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp r = await question.UnLikeAsync();
            return Pack(r);
        }
    }
}
