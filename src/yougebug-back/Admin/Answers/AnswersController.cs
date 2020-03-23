using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using yougebug_back.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace yougebug_back.Admin.Answers
{
    /// <summary>
    /// 管理员答案控制器
    /// </summary>
    [Route(Defaults.ADMIN_DEFAULT_ROUTE)]
    public class AnswersController : AdminBaseController
    {
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
        public async Task<IActionResult> BackAsync(int id, [FromBody]string description)
        {
            var answer = Domain.Answers.Hub.GetAnswer(id);
            Domain.Resp r = await answer.BackAsync(description);
            return Pack(r);
        }
    }
}
