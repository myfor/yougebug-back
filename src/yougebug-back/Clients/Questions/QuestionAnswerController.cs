using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using yougebug_back.Shared;

namespace yougebug_back.Clients.Questions
{
    /// <summary>
    /// 客户端，问题答案控制器
    /// </summary>
    [Route(Defaults.CLIENT_DEFAULT_ROUTE)]
    public class QuestionAnswerController : ClientBaseController
    {
        /// <summary>
        /// 新回答
        /// </summary>
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPost("{questionId}")]
        public async Task<IActionResult> AddAnswerAsync(int questionId, [FromBody]string content)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(questionId);
            Domain.Resp r = await question.AddAnswerAsync(CurrentAccount.Id, content);
            return Pack(r);
        }

        /// <summary>
        /// 提交审核
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{questionId}")]
        public async Task<IActionResult> ToAuditAsync(int questionId, [FromBody]string description)
        {
            Domain.Questions.Question question = Domain.Questions.Hub.GetQuestion(questionId);
            Domain.Resp r = await question.ToAudit(description);
            return Pack(r);
        }
    }
}
