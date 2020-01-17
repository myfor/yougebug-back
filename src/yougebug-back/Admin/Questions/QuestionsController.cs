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
        public async Task<ActionResult> GetList(int index, int size, string title)
        {
            Domain.Paginator pager = new Domain.Paginator
            { 
                Index = index,
                Size = size
            };

            Domain.Questions.Hub hub = new Domain.Questions.Hub();
            Domain.Resp resp = await hub.GetList(pager, Domain.Share.Platform.Admin);
            return Pack(resp);
        }
    }
}
