using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> GetListAsync(int index, string title)
        {
            Domain.Paginator pager = new Domain.Paginator
            {
                Index = index,
                Params = new Dictionary<string, string>
                { 
                    ["title"] = title
                }
            };

            Domain.Questions.Hub hub = new Domain.Questions.Hub();
            Domain.Resp resp = await hub.GetListAsync(pager, Domain.Share.Platform.Admin);
            return Pack(resp);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetDetailAsync(int id)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp resp = await question.GetDetailAsync();
            return Pack(resp);
        }

        /// <summary>
        /// 获取答案列表
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> GetAnswersAsync(int questionId, int index)
        {
            Domain.Paginator pager = new Domain.Paginator
            { 
                Index = index
            };

            Domain.Answers.Hub hub = new Domain.Answers.Hub();
            Domain.Resp resp = await hub.GetAnswersAsync(pager, questionId);
            return Pack(resp);
        }

        /// <summary>
        /// 退回一个问题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description">原因</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<ActionResult> BackAsync(int id, [FromBody]string description)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp resp = await question.BackAsync(description);
            return Pack(resp);
        }
    }
}
