using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using yougebug_back.Shared;

namespace yougebug_back.Admin.Questions
{

    [Route(Defaults.ADMIN_DEFAULT_ROUTE)]
    public class QuestionReportsController : AdminBaseController
    {
        /// <summary>
        /// 获取举报的提问列表
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetReportsListAsync(int questionId, int index)
        {
            Domain.Paginator pager = Domain.Paginator.New(index, 20, 1);
            pager["questionId"] = questionId.ToString();

            var reportQuestion = Domain.Questions.Reports.GetReportQuestion(questionId);
            var r = await reportQuestion.GetReportsListAsync(pager);
            return Pack(r);
        }

        /// <summary>
        /// 忽略这次举报
        /// </summary>
        [HttpPut("{questionId}/ignore")]
        public async Task<IActionResult> IgnoreAsync(int questionId)
        {
            var reportQuestion = Domain.Questions.Reports.GetReportQuestion(questionId);
            var r = await reportQuestion.IgnoreAsync();
            return Pack(r);
        }

        /// <summary>
        /// 退回这次举报
        /// </summary>
        [HttpPut("{questionId}/back")]
        public async Task<IActionResult> BackAsync(int questionId, [FromBody]string reason)
        {
            var reportQuestion = Domain.Questions.Reports.GetReportQuestion(questionId);
            var r = await reportQuestion.BackAsync(reason);
            return Pack(r);
        }

        /// <summary>
        /// 删除这个答案
        /// </summary>
        [HttpPut("{questionId}/delete")]
        public async Task<IActionResult> DeleteAsync(int questionId)
        {
            var reportQuestion = Domain.Questions.Reports.GetReportQuestion(questionId);
            var r = await reportQuestion.DeleteAsync();
            return Pack(r);
        }
    }
}
