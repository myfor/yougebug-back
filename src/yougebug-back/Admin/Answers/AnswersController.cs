using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using yougebug_back.Shared;

namespace yougebug_back.Admin.Answers
{
    /// <summary>
    /// 管理员答案控制器
    /// </summary>
    [Route(Defaults.ADMIN_DEFAULT_ROUTE)]
    public class AnswersController : AdminBaseController
    {
        /// <summary>
        /// 获取问题下的答案列表
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetAnswersList(int questionId, int index, int size)
        {
            Domain.Paginator pager = Domain.Paginator.New(index, size);

            Domain.Answers.Hub answerHub = new Domain.Answers.Hub();
            var r = await answerHub.GetAnswersAsync(pager, questionId);
            return Pack(r);
        }

        /*
         * 获取所有的答案列表
         */
        [HttpGet("all")]
        public async Task<IActionResult> GetDisabledListAsync(int index, int size, string questionTitle, int state)
        {
            Domain.Paginator pager = Domain.Paginator.New(index, size);
            pager.Params = new Dictionary<string, string>
            { 
                ["questionTitle"] = questionTitle
            };

            Domain.Answers.Hub answerHub = new Domain.Answers.Hub();
            var r = await answerHub.GetAnswersList(pager, (Domain.Answers.Answer.AnswerState)state);
            return Pack(r);
        }

        /// <summary>
        /// 启用答案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}/enabled")]
        public async Task<IActionResult> EnabledAsync(int id)
        {
            var answer = Domain.Answers.Hub.GetAnswer(id);
            Domain.Resp r = await answer.EnabledAsync();
            return Pack(r);
        }

        /// <summary>
        /// 退回答案
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}/disabled")]
        public async Task<IActionResult> BackAsync(int id, string description)
        {
            var answer = Domain.Answers.Hub.GetAnswer(id);
            Domain.Resp r = await answer.BackAsync(description);
            return Pack(r);
        }
    }
}
