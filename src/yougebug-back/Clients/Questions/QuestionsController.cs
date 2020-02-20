using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using yougebug_back.Shared;

namespace yougebug_back.Clients.Questions
{
    /// <summary>
    /// 客户端，问题控制器
    /// </summary>
    [Route(Defaults.CLIENT_DEFAULT_ROUTE)]
    public class QuestionsController : YGBBaseController
    {
        /// <summary>
        /// 获取问题列表
        /// </summary>
        [HttpGet]
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
        /// 举报一个问题
        /// </summary>
        [HttpPatch("{id}/report")]
        public async Task<IActionResult> ReportAsync(int id, [FromForm]string reason, [FromForm]string description)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(id);
            Domain.Resp r = await question.ReportAsync(reason, description);
            return Pack(r);
        }
    }
}
