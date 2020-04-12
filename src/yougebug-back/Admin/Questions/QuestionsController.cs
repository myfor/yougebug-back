using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using yougebug_back.Shared;

namespace yougebug_back.Admin.Questions
{
    /// <summary>
    /// 问题列表
    /// </summary>
    [Route(Defaults.ADMIN_DEFAULT_ROUTE)]
    public class QuestionsController : AdminBaseController
    {
        /// <summary>
        /// 获取问题列表
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetListAsync(int index, int size, string search, int? state)
        {
            /*
                管理员获取的问题列表，不包括被移除的，移除的列表在回收站中查看
             */

            Paginator pager = Paginator.New(index, size, 2);
            pager["search"] = search;
            pager["state"] = state.ToString();

            Domain.Questions.Hub hub = new Domain.Questions.Hub();
            Resp resp = await hub.GetListAsync(pager, Domain.Questions.Hub.QuestionListSource.Admin);
            return Pack(resp);
        }

        /// <summary>
        /// 获取举报列表
        /// </summary>
        [HttpGet("reports")]
        public async Task<IActionResult> GetReportQuestionList(int index, int size, string title)
        {
            Paginator pager = Paginator.New(index, size, 1);
            pager["title"] = string.IsNullOrWhiteSpace(title) ? "" : title;

            Domain.Questions.Hub questionHub = new Domain.Questions.Hub();
            var r = await questionHub.GetListAsync(pager, Domain.Questions.Hub.QuestionListSource.Reports);
            return Pack(r);
        }
        /// <summary>
        /// 获取举报列表详情
        /// </summary>
        [HttpGet("reports/{id}")]
        public async Task<IActionResult> GetReportDetailAsync(int id)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            var r = await question.GetDetailAsync(Domain.Questions.Question.DetailSource.Report);
            return Pack(r);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync(int id)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp resp = await question.GetDetailAsync(Domain.Questions.Question.DetailSource.Admin);
            return Pack(resp);
        }

        /// <summary>
        /// 软删除一个问题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuestionAsync(int id)
        {
            Domain.Questions.Hub questionHub = new Domain.Questions.Hub();
            Resp r = await questionHub.DeleteQuestionAsync(id);
            return Pack(r);
        }

        ///// <summary>
        ///// 获取答案列表
        ///// </summary>
        //[HttpGet("answers")]
        //public async Task<IActionResult> GetAnswersAsync(int questionId, int index)
        //{
        //    Domain.Paginator pager = new Domain.Paginator
        //    {
        //        Index = index
        //    };

        //    Domain.Answers.Hub hub = new Domain.Answers.Hub();
        //    Domain.Resp resp = await hub.GetAnswersAsync(pager, questionId);
        //    return Pack(resp);
        //}

        /// <summary>
        /// 退回一个问题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description">原因</param>
        /// <returns></returns>
        [HttpPatch("{id}/back")]
        public async Task<IActionResult> BackAsync(int id, string descpiption)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp resp = await question.BackAsync(descpiption);
            return Pack(resp);
        }

        /// <summary>
        /// 启用一个问题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}/enabled")]
        public async Task<IActionResult> EnabledAsync(int id)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Resp resp = await question.EnabledAsync();
            return Pack(resp);
        }
    }
}
