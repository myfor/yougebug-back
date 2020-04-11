using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using yougebug_back.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

            Domain.Questions.Reports reports = new Domain.Questions.Reports();
            var r = await reports.GetReportsListAsync(pager);
            return Pack(r);
        }
    }
}
