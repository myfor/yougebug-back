using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
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
        public async Task<IActionResult> GetListAsync(int index, int size, string search)
        {
            Paginator pager = Paginator.New(index, size);
            pager.Params = new Dictionary<string, string>
            {
                ["search"] = search
            };

            Domain.Questions.Hub hub = new Domain.Questions.Hub();
            Resp resp = await hub.GetListAsync(pager, Share.Platform.Admin);
            return Pack(resp);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync(int id)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp resp = await question.GetDetailAsync();
            return Pack(resp);
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
