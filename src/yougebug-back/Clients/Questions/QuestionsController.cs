using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using yougebug_back.Shared;

namespace yougebug_back.Clients.Questions
{
    /// <summary>
    /// 客户端，问题控制器
    /// </summary>
    [Route(Defaults.CLIENT_DEFAULT_ROUTE)]
    public class QuestionsController : ClientBaseController
    {
        /// <summary>
        /// 获取提问列表
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetQuestionsAsync(int index, string title)
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
            Domain.Resp resp = await hub.GetListAsync(pager, Domain.Share.Platform.Client);
            return Pack(resp);
        }

        /// <summary>
        /// 获取问题详情
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetQuestionDetail(int id)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp detail = await question.GetDetailAsync();
            return Pack(detail);
        }

        /// <summary>
        /// 举报一个提问
        /// </summary>
        [HttpPatch("{id}/report")]
        [AllowAnonymous]
        public async Task<IActionResult> ReportAsync(int id, Domain.Questions.Models.NewReport report)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp r = await question.ReportAsync(report.Reason, report.Description);
            return Pack(r);
        }

        /// <summary>
        /// 提问
        /// </summary>
        [HttpPost]
        [ClientsLoginCheck]
        public async Task<IActionResult> AskAsync(Domain.Questions.Models.PostQuestion model)
        {
            var r = await CurrentAccount.AskQuestion(model);
            return Pack(r);
        }

        /// <summary>
        /// 新回答
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [ClientsLoginCheck]
        [HttpPost("{id}/answer")]
        public async Task<IActionResult> AddAnswerAsync(int id, [FromBody]string content)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp r = await question.AddAnswerAsync(CurrentAccount.Id, content);
            return Pack(r);
        }

        /// <summary>
        /// 修改提问后提交审核
        /// </summary>
        /// <returns></returns>
        [ClientsLoginCheck]
        [HttpPatch("{id}")]
        public async Task<IActionResult> ToAuditAsync(int id, [FromBody]string description)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp r = await question.ToAudit(description);
            return Pack(r);
        }
    }
}
