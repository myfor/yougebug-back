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

        [HttpGet]
        public async Task<ActionResult> GetListAsync(int index, int size, string title)
        {
            Domain.Paginator pager = new Domain.Paginator
            { 
                Index = index,
                Size = size
            };

            Domain.Questions.Hub hub = new Domain.Questions.Hub();
            Domain.Resp resp = await hub.GetListAsync(pager, Domain.Share.Platform.Admin);
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

        }
    }
}
